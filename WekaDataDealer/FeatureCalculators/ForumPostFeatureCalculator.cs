using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WekaDataDealer.Data;
using WekaDataDealer.Models;
using WekaDataDealer.Models.MappedClasses;

namespace WekaDataDealer.FeatureCalculators
{
    public class ForumPostFeatureCalculator : IFeatureCalculator
    {
        // todo лучше протаскивать instance который возвращает контексты, а не сами контексты
        private static BuhonlineContext buhOnlineContext = (BuhonlineContext) BuhOnlineDBConnection.GetInstance().GetContext;
        private static readonly Dictionary<int, int> TopicCitedCounts = new Dictionary<int, int>();
        private static readonly Dictionary<int, int> MessageCitedCounts = new Dictionary<int, int>();

        private static readonly Dictionary<int, HashSet<int>> ThanksPositionsInTopics =
            new Dictionary<int, HashSet<int>>();

        public ForumPostFeatureCalculator()
        {
            FillCitationCounts();
            GetThanksPositionsInTopics();
        }

        private void FillCitationCounts()
        {
            var topicEx = FeatureRegex.TopicNum;
            var messageEx = FeatureRegex.MessageNum;
            foreach (var message in DBCache.GetPostMessagesFromDB(buhOnlineContext))
            {
                foreach (Match match in topicEx.Matches(message))
                {
                    var topicId = int.Parse(match.Groups[1].Value);
                    if (!TopicCitedCounts.ContainsKey(topicId))
                        TopicCitedCounts[topicId] = 0;
                    TopicCitedCounts[topicId]++;
                }

                foreach (Match match in messageEx.Matches(message))
                {
                    var messageId = int.Parse(match.Groups[1].Value);
                    if (!MessageCitedCounts.ContainsKey(messageId))
                        MessageCitedCounts[messageId] = 0;
                    MessageCitedCounts[messageId]++;
                }
            }
        }

        private static void GetThanksPositionsInTopics()
        {
            var featuresContext = (FeaturesContext) FeaturesDBConnection.GetInstance().GetContext;
            var topicIds = buhOnlineContext.yaf_Topic.Select(p => p.TopicID).ToArray();
            var thankDict = FileProcessing.MessageIdAndIsThankPost.ToDictionary();
            foreach (var topicId in topicIds)
            {
                var messages = DBCache.GetTopicMessages(topicId, buhOnlineContext);
                var posts = messages.Where(m => thankDict[m.MessageID] == 1).Select(m => m.Position);
                ThanksPositionsInTopics.Add(topicId, new HashSet<int>(posts));
            }

        }




        public List<List<object>> GetFeatureVectors(IEnumerable<IDataItem> dataItems)
        {
            return dataItems.Select(item => CalculateFeatures(item as yaf_Message)).ToList();
        }

        public List<object> CalculateFeatures(yaf_Message item)
        {
            return new List<object>
                       {
                           IsDateContained(item),
                           IsQuoteContained(item),
                           IsTopicStarterCited(item),
                           IsUrlContained(item),
                           IsFormulaContained(item),
                           IsSmileContained(item),
                           IsThankwordsContained(item),
                           IsSameAuthorAsTopicStarter(item),
                           IsUrlContained(item),
                           IsBuhWordsContained(item),
                           IsTaxWordsContained(item),
                           IsControlSystemWordsContained(item),
                           IsFormWordsContained(item),
                           IsEdited(item),
                           IsTextHighlighted(item),
                           
                           GetQuestionMarksFeature(item),
                           GetPostPositionFeature(item),
                           GetPostLengthFeature(item),
                           GetPunctuationsCount(item),
                           GetDistinctPunctuationsCount(item),
                           GetQuoteMarksCount(item),
                           GetQuotedWordsCount(item),
                           GetQuotedWordsToAllCount(item),
                           GetUserQuotesCount(item),
                           GetQuoteWordsToAllWordsCount(item),
                           GetMaxQuoteDeepness(item),
                           GetBBTagsCount(item),
                           GetAccentedWordsCount(item),
                           GetAuthorRating(item),
                           GetAuthorForumAge(item),
                           GetAuthorProfession(item),
                           GetAuthorPostsCount(item),
                           GetDigitsCount(item),
                           GetNumbersCount(item),
                           GetUpperLettersCount(item),
                           GetStopWordsToAllWordsCount(item),
                           DistanceToTopicStarter(item),
                           TimeFromTopicStarter(item),
                           TimeFromPreviousPost(item),
                           ThreadLength(item),
                           QuotientPostLengthToAvarageInTopic(item),
                           GetPoints(item),
                           GetBBTagsCount(item),
                           GetTopicCitedCount(item),
                           GetMessageCitedCount(item),
                           GetDistanceToThanks(item)
                       };
        }
        
        #region Utilites

        private static yaf_Message GetNthPost(int topicId, int n)
        {
            return DBCache.GetTopicMessages(topicId, buhOnlineContext).First(p => p.Position == n);
        }

        private static yaf_Message GetTopicStarter(yaf_Message item)
        {
            return GetNthPost(item.TopicID, 0);
        }

        private static yaf_Message GetPreviousPost(yaf_Message item)
        {
            return GetNthPost(item.TopicID, item.Position - 1);
        }

        private static bool IsWordsFromFileContained(yaf_Message item, string fileName)
        {
            return File.ReadAllLines(fileName).Any(item.Message.Contains);
            //TODO MyStem
        }

        private static int GetTaggedWordsCount(yaf_Message item, string tagName)
        {
            var quoteWordsCount = 0;
            var exp = new Regex(@"\[" + tagName + @"(?:=[^\]]+)?\](.*?)\[/" + tagName + @"\]");

            foreach (Match match in exp.Matches(item.Message)) 
            {
                quoteWordsCount += FeatureRegex.Words.Split(match.Groups[1].Value).Count(part => part != "");
            }
            return quoteWordsCount;
        }

        private static yaf_User GetAuthor(yaf_Message item)
        {
            return DBCache.GetUser(item.UserID, buhOnlineContext);
        }

        #endregion
        
        private static int GetQuestionMarksFeature(yaf_Message item)
        {
            return item.Message.Split('?').Length - 1;
        }

        private static int GetPostPositionFeature(yaf_Message item)
        {
            return item.Position;
        }

        private static int GetPostLengthFeature(yaf_Message item)
        {
            return item.Message.Length;
        }
        
        private static int GetPunctuationsCount(yaf_Message item)
        {
            return item.Message.Count(char.IsPunctuation);
        }

        private static int GetDistinctPunctuationsCount(yaf_Message item)
        {
            return item.Message.Where(char.IsPunctuation).Distinct().Count();
        }

        private static int GetQuoteMarksCount(yaf_Message item)
        {
            return FeatureRegex.QuoteMarks.Matches(item.Message).Count;
        }

        private static double GetQuotedWordsToAllCount(yaf_Message item)
        {
            return (double)GetQuotedWordsCount(item)/GetWordsCount(item);
        }

        private static int GetWordsCount(yaf_Message item)
        {
            return FeatureRegex.Tokens.Split(item.Message).Length;
        }

        private static int GetQuotedWordsCount(yaf_Message item)
        {
            var sum = 0;
            var quotedStrings = FeatureRegex.QuotedWords.Matches(item.Message);
            for (var i = 0; i < quotedStrings.Count; i++)
            {
                sum += FeatureRegex.Tokens.Split(quotedStrings[i].Groups[1].Value).Length;
            }
            return sum;
        }

        private static int GetDigitsCount(yaf_Message item)
        {
            return item.Message.Count(char.IsDigit);
        }

        private static int GetNumbersCount(yaf_Message item)
        {
            return FeatureRegex.Numbers.Split(item.Message).Length - 1;
        }

        private static bool IsSmileContained(yaf_Message item)
        {
            var smiles =
                ":) ;-) :( \\:d/ :-\" =d&gt; o:) [-o&lt; [-x #-o =p~ :-k =; :-$ :-# [-( :-s :-({|= 8-[ :-& :^o ](*,) @}-\\-- :-n"
                    .Split(' ');
            return smiles.Any(item.Message.Contains);
        }

        private static int GetUpperLettersCount(yaf_Message item)
        {
            return item.Message.Count(char.IsUpper);
        }

        private static double GetStopWordsToAllWordsCount(yaf_Message item)
        {
            var stopwords = new HashSet<string>(File.ReadAllLines("Dictionaries/stopwords.txt").Select(w => w.Trim()));
            return (double)FeatureRegex.Words.Split(item.Message.ToLower()).Count(stopwords.Contains)/GetWordsCount(item);
        }

        private static bool IsThankwordsContained(yaf_Message item)
        {
            var thankwords = new HashSet<string>(File.ReadAllLines("Dictionaries/thankwords.txt").Select(w => w.Trim()));
            return FeatureRegex.Words.Split(item.Message.ToLower()).Any(thankwords.Contains);
        }

        private static int GetUserQuotesCount(yaf_Message item)
        {
            return FeatureRegex.QuoteExpression.Matches(item.Message).Count;
        }

        private static bool IsQuoteContained(yaf_Message item)
        {
            return FeatureRegex.QuoteExpression.IsMatch(item.Message);
        }

        private static double GetQuoteWordsToAllWordsCount(yaf_Message item)
        {
            var quoteWordsCount = GetTaggedWordsCount(item, "quote");
            return (double)quoteWordsCount / FeatureRegex.Words.Split(item.Message).Count(w => w != "quote" && w != "");
        }

        private static double GetMaxQuoteDeepness(yaf_Message item)
        {
            int i = 0, maxDeepness = 0;
            foreach (Match match in FeatureRegex.QuoteTag.Matches(item.Message))
            {
                if (match.Groups[0].Value == "[quote")
                    i++;
                else if (match.Groups[0].Value == "[/quote")
                    i--;
                if (i > maxDeepness)
                    maxDeepness = i;
            }
            return maxDeepness;
        }

        private static bool IsTopicStarterCited(yaf_Message item)
        {
            var topicStarter = GetTopicStarter(item);
            return
                FeatureRegex.QuoteExpression.Matches(item.Message).Cast<Match>().Any(
                    m => m.Groups[1].Value.Trim().ToLower() == topicStarter.Message.Trim().ToLower());
        }

        internal static int DistanceToTopicStarter(yaf_Message item)
        {
            return item.Position;
        }
        internal static int DistanceToEnd(yaf_Message item)
        {
            return ThreadLength(item) - DistanceToTopicStarter(item);
        }
        private static double TimeFromTopicStarter(yaf_Message item)
        {
            if (item.Position == 0) return 0;
            var ts = GetTopicStarter(item);
            return (item.Posted - ts.Posted).TotalMinutes;
        }

        private static double TimeFromPreviousPost(yaf_Message item)
        {
            if (item.Position == 0) return 0;
            var pp = GetPreviousPost(item);
            return (item.Posted - pp.Posted).TotalMinutes;
        }

        private static int ThreadLength(yaf_Message item)
        {
            return DBCache.GetTopicMessages(item.TopicID, buhOnlineContext).Count();
        }

        private static double QuotientPostLengthToAvarageInTopic(yaf_Message item)
        {
            var avgLen = DBCache.GetTopicMessages(item.TopicID, buhOnlineContext).Average(p => p.Message.Length);
            return item.GetText().Length/avgLen;
        }

        private static bool IsSameAuthorAsTopicStarter(yaf_Message item)
        {
            var topicStarterAuthorID = GetTopicStarter(item).UserID;
            return item.UserID == topicStarterAuthorID;
        }

        private static bool IsUrlContained(yaf_Message item)
        {
            return FeatureRegex.Url.IsMatch(item.Message);
        }

        private static bool IsBuhWordsContained(yaf_Message item)
        {
            return IsWordsFromFileContained(item, "Dictionaries/BuhWords.txt");
        }

        private static bool IsTaxWordsContained(yaf_Message item)
        {
            return IsWordsFromFileContained(item, "Dictionaries/TaxWords.txt");
        }

        private static bool IsControlSystemWordsContained(yaf_Message item)
        {
            return IsWordsFromFileContained(item, "Dictionaries/ControlSystemWords.txt");
        }

        private static bool IsFormWordsContained(yaf_Message item)
        {
            return IsWordsFromFileContained(item, "Dictionaries/FormWords.txt");
        }

        private static bool IsEdited(yaf_Message item)
        {
            return item.Edited != null;
        }

        private static int GetPoints(yaf_Message item)
        {
            return item.Points;
        }

        private static int GetBBTagsCount(yaf_Message item)
        {
            return FeatureRegex.BBTag.Matches(item.Message).Count;
        }

        private static bool IsTextHighlighted(yaf_Message item)
        {
            return FeatureRegex.HighlightBBTag.IsMatch(item.Message);
        }

        private static int GetTopicCitedCount(yaf_Message item)
        {
            var count = TopicCitedCounts.ContainsKey(item.TopicID) ? TopicCitedCounts[item.TopicID] : 0;
            if (MessageCitedCounts.ContainsKey(item.MessageID))
                count += MessageCitedCounts[item.MessageID];
            return count;
        }

        private static int GetMessageCitedCount(yaf_Message item)
        {
            return MessageCitedCounts.ContainsKey(item.MessageID) ? MessageCitedCounts[item.MessageID] : 0;
        }

        private static double GetAccentedWordsCount(yaf_Message item)
        {
            return GetTaggedWordsCount(item, "b") + GetTaggedWordsCount(item, "s") + GetTaggedWordsCount(item, "color") + GetTaggedWordsCount(item, "colour");
        }


        private static bool IsFormulaContained(yaf_Message item)
        {
            var parts = item.Message.Split(new[]{']', '?'});
            return parts.Any(FeatureRegex.Formula.IsMatch);
        }

        private static bool IsDateContained(yaf_Message item)
        {
            return FeatureRegex.Date.IsMatch(item.Message);
        }

        private static int GetAuthorRating(yaf_Message item)
        {
            var author = GetAuthor(item);
            if (author == null)
                return 0;
            return author.Points;
        }

        private static int GetAuthorForumAge(yaf_Message item)
        {
            var author = GetAuthor(item);
            if (author == null || author.Joined > item.Posted)
                return 0;
            return (int)(item.Posted - author.Joined).TotalDays;
        }

        private static string GetAuthorProfession(yaf_Message item)
        {
            var authorData = DBCache.GetUserPersonalData(item.UserID, buhOnlineContext);
            return authorData == null || authorData.Profession == null ? "" : authorData.Profession.ToLower();
        }
        
        private static int GetAuthorPostsCount(yaf_Message item)
        {
            var author = GetAuthor(item);
            if (author == null)
                return 0;
            return author.NumPosts;
        }
        
        private static int GetDistanceToThanks(yaf_Message item)
        {
            var tId = item.TopicID;
            if (!ThanksPositionsInTopics.ContainsKey(tId))
                return int.MaxValue;
            var validatedPositions = ThanksPositionsInTopics[tId].Where(pos => pos >= item.Position).ToArray();
            if (!validatedPositions.Any()) return int.MaxValue;
            return validatedPositions.Min();

        }
    
    }
}
