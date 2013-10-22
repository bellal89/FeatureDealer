using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FeatureDealer.FeatureCalculators;
using FeatureDealer.FeatureSettings;
using FeatureDealer.Models.MappedClasses;
using FeatureDealer.RuLemmatizer;

namespace FeatureDealer.Features
{
    public class QueryIndependentFeatures
    {
        private readonly List<string> buhWords;
        private readonly BuhOnlineDataProvider buhonlineDataProvider;
        private readonly List<string> controlSystemWords;
        private readonly List<string> formWords;
        private readonly Lemmatizer lemmatizer;
        private readonly IDictionary<string, int> professions = new Dictionary<string, int> {{string.Empty, 0}};

        private readonly List<string> smiles = new List<string>
                                                   {
                                                       ":)",
                                                       ";-)",
                                                       ":(",
                                                       "\\:d/",
                                                       @":-\",
                                                       "=d&gt;",
                                                       "o:)",
                                                       "[-o&lt;",
                                                       "[-x",
                                                       "#-o",
                                                       "=p~",
                                                       ":-k",
                                                       "=;",
                                                       ":-$",
                                                       ":-#",
                                                       "[-(",
                                                       ":-s",
                                                       ":-({|=",
                                                       "8-[",
                                                       ":-&",
                                                       ":^o",
                                                       "](*,)",
                                                       "@}-\\--",
                                                       ":-n"
                                                   };

        private readonly List<string> taxWords;

        public QueryIndependentFeatures()
        {
            Console.WriteLine("Initializing QueryIndependentFeatures");
            const string buhWordsFilepath = "Dictionaries/BuhWords.txt";
            const string taxWordsFilepath = "Dictionaries/TaxWords.txt";
            const string controlSystemWordsFilepath = "Dictionaries/ControlSystemWords.txt";
            const string formWordsFilepath = "Dictionaries/FormWords.txt";
            buhWords = File.ReadAllLines(buhWordsFilepath).Select(line => line.Trim().ToLower()).ToList();
            taxWords = File.ReadAllLines(taxWordsFilepath).Select(line => line.Trim().ToLower()).ToList();
            controlSystemWords =
                File.ReadAllLines(controlSystemWordsFilepath).Select(line => line.Trim().ToLower()).ToList();
            formWords = File.ReadAllLines(formWordsFilepath).Select(line => line.Trim().ToLower()).ToList();
            buhonlineDataProvider = new BuhOnlineDataProvider();
            lemmatizer = new Lemmatizer();
        }

        [Feature("MaxQuoteDepth")]
        public double GetMaxQuoteDeepness(FeatureParameters parameters)
        {
            int i = 0, maxDeepness = 0;
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
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

        [Feature("ForumRulesReference")]
        public bool HasForumRulesReference(FeatureParameters parameters)
        {
            // todo
            return false;
        }

        [Feature("IsMessageFromModerator")]
        public bool IsMessageFromModerator(FeatureParameters parameters)
        {
            // todo
            return false;
        }

        [Feature("TopicStarterCited")]
        public bool IsTopicStarterCited(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            yaf_Message topicStarter = GetTopicStarter(item.TopicID);
            return
                FeatureRegex.QuoteExpression.Matches(item.Message).Cast<Match>().Any(
                    m => m.Groups[1].Value.Trim().ToLower() == topicStarter.Message.Trim().ToLower());
        }

        [Feature("PositionInThread")]
        public int PositionInThread(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            return buhonlineDataProvider.ReadMessage(messageId).Position;
        }

        [Feature("PostDistanceToThreadEnd")]
        public int DistanceToEnd(FeatureParameters parameters)
        {
            return ThreadLength(parameters) - PositionInThread(parameters);
        }

        [Feature("PostTimeFromTopicStarterElapsed")]
        public double TimeFromTopicStarter(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            yaf_Message ts = buhonlineDataProvider.ReadTopicStarter(item.TopicID);
            return (item.Posted - ts.Posted).TotalMinutes;
        }

        [Feature("TimeFromPreviousPostElapsed")]
        public double TimeFromPreviousPost(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            if (item.Position == 0) return 0;
            yaf_Message pp = GetPreviousPost(item.TopicID, messageId);
            return (item.Posted - pp.Posted).TotalMinutes;
        }

        [Feature("ThreadLength")]
        public int ThreadLength(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return buhonlineDataProvider.ReadTopicMessages(item.TopicID).Count();
        }

        [Feature("PostQuoteLengthAverageThreadQuoteLengthRatio")]
        public double QuotientPostLengthToAvarageInTopic(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            IEnumerable<yaf_Message> messages = buhonlineDataProvider.ReadTopicMessages(item.TopicID);
            return messages.Average(m => m.Message.Length);
        }

        [Feature("AuthorOfPostIsQuestioner")]
        public bool IsSameAuthorAsTopicStarter(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            int topicStarterAuthorID = GetTopicStarter(item.TopicID).UserID;
            return item.UserID == topicStarterAuthorID;
        }

        [Feature("PostWasEdited")]
        public bool IsEdited(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return item.Edited != null;
        }

        [Feature("PostPoints")]
        public int GetPoints(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return item.Points;
        }

        [Feature("TopicCitationCount")]
        public int GetTopicCitedCount(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            int count = Preprocessing.TopicCitedCounts.ContainsKey(item.TopicID)
                            ? Preprocessing.TopicCitedCounts[item.TopicID]
                            : 0;
            if (Preprocessing.MessageCitedCounts.ContainsKey(item.MessageID))
                count += Preprocessing.MessageCitedCounts[item.MessageID];
            return count;
        }

        [Feature("PostCitationCount")]
        public int GetMessageCitedCount(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return Preprocessing.MessageCitedCounts.ContainsKey(item.MessageID)
                       ? Preprocessing.MessageCitedCounts[item.MessageID]
                       : 0;
        }

        [Feature("AuthorRating")]
        public int GetAuthorRating(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            yaf_User author = GetAuthor(item.UserID);
            if (author == null)
                return 0;
            return author.Points;
        }

        [Feature("AuthorForumAge")]
        public int GetAuthorForumAge(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            yaf_User author = GetAuthor(item.UserID);
            if (author == null || author.Joined > item.Posted)
                return 0;
            return (int) (item.Posted - author.Joined).TotalDays;
        }

        [Feature("AuthorProfession")]
        public int GetAuthorProfession(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            int userId = item.UserID;
            UserPersonal author = buhonlineDataProvider.ReadUserPersonal(userId);
            if (author == null) return 0;
            string profession = author.Profession;
            if (profession == null) return 0;
            if (!professions.ContainsKey(profession))
                professions.Add(profession, professions.Keys.Count + 1);
            return professions[profession];
        }

        [Feature("AuthorPostsCount")]
        public int GetAuthorPostsCount(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            yaf_User author = GetAuthor(item.UserID);
            if (author == null)
                return 0;
            return author.NumPosts;
        }

        [Feature("DistanceToThanks")]
        public int GetDistanceToThanks(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            int tId = item.TopicID;
            if (!Preprocessing.ThanksPositionsInTopics.ContainsKey(tId))
                return int.MaxValue;
            int[] validatedPositions =
                Preprocessing.ThanksPositionsInTopics[tId].Where(pos => pos >= item.Position).ToArray();
            if (!validatedPositions.Any()) return int.MaxValue;
            return validatedPositions.Min();
        }

        [Feature("QuestionMarksCount")]
        public int GetQuestionMarksFeature(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return item.Message.Split('?').Length - 1;
        }

        [Feature("PostLength")]
        public int GetPostLengthFeature(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return item.Message.Length;
        }

        [Feature("PunctuationsCount")]
        public int GetPunctuationsCount(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return item.Message.Count(char.IsPunctuation);
        }

        [Feature("UniquePunctuationsCount")]
        public int GetDistinctPunctuationsCount(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return item.Message.Where(char.IsPunctuation).Distinct().Count();
        }

        [Feature("QuoteMarksCount")]
        public int GetQuoteMarksCount(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return FeatureRegex.QuoteMarks.Matches(item.Message).Count;
        }

        [Feature("QuotedWordsCount")]
        public int GetQuotedWordsCount(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            int sum = 0;
            MatchCollection quotedStrings = FeatureRegex.QuotedWords.Matches(item.Message);
            for (int i = 0; i < quotedStrings.Count; i++)
            {
                sum += FeatureRegex.Tokens.Split(quotedStrings[i].Groups[1].Value).Length;
            }
            return sum;
        }

        [Feature("QuotedWordsRatio")]
        public double GetQuotedWordsToAllCount(FeatureParameters parameters)
        {
            return (double) GetQuotedWordsCount(parameters)/GetWordsCount(parameters);
        }

        [Feature("WordsCount")]
        public int GetWordsCount(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return FeatureRegex.Tokens.Split(item.Message).Length;
        }

        [Feature("DigitsCount")]
        public int GetDigitsCount(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return item.Message.Count(char.IsDigit);
        }

        [Feature("NumbersCount")]
        public int GetNumbersCount(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return FeatureRegex.Numbers.Split(item.Message).Length - 1;
        }

        [Feature("IsSmileContained")]
        public bool IsSmileContained(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return smiles.Any(item.Message.Contains);
        }

        [Feature("UpperLettersCount")]
        public int GetUpperLettersCount(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return item.Message.Count(char.IsUpper);
        }

        [Feature("GreetingWordContained")]
        public bool IsHelloContained(FeatureParameters parameters)
        {
            HashSet<string> helloWords = Preprocessing.HelloWords;
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return helloWords.Any(item.Message.ToLower().StartsWith);
        }

        [Feature("StopWordsRatio")]
        public double GetStopWordsToAllWordsCount(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            HashSet<string> stopwords = Preprocessing.StopWords;
            return (double) FeatureRegex.Words.Split(item.Message.ToLower()).Count(stopwords.Contains)/
                   GetWordsCount(parameters);
        }

        [Feature("ThankWordsContained")]
        public bool IsThankwordsContained(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            HashSet<string> thankwords = Preprocessing.ThankWords;
            return FeatureRegex.Words.Split(item.Message.ToLower()).Any(thankwords.Contains);
        }

        [Feature("UserQuotesCount")]
        public int GetUserQuotesCount(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return FeatureRegex.QuoteExpression.Matches(item.Message).Count;
        }

        [Feature("HasQuotes")]
        public bool IsQuoteContained(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return FeatureRegex.QuoteExpression.IsMatch(item.Message);
        }

        [Feature("IsUrlContained")]
        public bool IsUrlContained(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return FeatureRegex.Url.IsMatch(item.Message);
        }

        [Feature("IsBuhWordsContained")]
        public bool IsBuhWordsContained(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return IsWordsFromCollectionContained(item.Message, buhWords);
        }

        [Feature("IsTaxWordsContained")]
        public bool IsTaxWordsContained(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return IsWordsFromCollectionContained(item.Message, taxWords);
        }

        [Feature("IsControlSystemWordsContained")]
        public bool IsControlSystemWordsContained(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return IsWordsFromCollectionContained(item.Message, controlSystemWords);
        }

        [Feature("IsFormWordsContained")]
        public bool IsFormWordsContained(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return IsWordsFromCollectionContained(item.Message, formWords);
        }

        [Feature("HtmlTagsCount")]
        public int GetHtmlTagsCount(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return FeatureRegex.HtmlTags.Matches(item.Message).Count;
        }

        [Feature("IsTextHighlighted")]
        public bool IsTextHighlighted(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return FeatureRegex.HighlightBBTag.IsMatch(item.Message);
        }

        [Feature("AccentedWordsCount")]
        public int GetAccentedWordsCount(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return GetTaggedWordsCount(item.Message, new List<string> {"b", "s", "color", "colour"});
        }

        [Feature("IsFormulaContained")]
        public bool IsFormulaContained(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            string[] parts = item.Message.Split(new[] {']', '?'});
            return parts.Any(FeatureRegex.Formula.IsMatch);
        }

        [Feature("IsDateContained")]
        public bool IsDateContained(FeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return FeatureRegex.Date.IsMatch(item.Message);
        }

        private bool IsWordsFromCollectionContained(string text, IEnumerable<string> words)
        {
            string buhText = string.Join(" ", words);
            var lemmaInfos = lemmatizer.GetLemmaInfos(buhText);
            IEnumerable<string> textWords = text.SplitIntoWordsLower();
            return lemmaInfos.Select(li => li.Lemma).Any(textWords.Contains) || words.Any(textWords.Contains);
        }

        private int GetTaggedWordsCount(string text, IEnumerable<string> tagNames)
        {
            return tagNames.Sum(tn => GetTaggedWordsCount(text, tn));
        }

        private int GetTaggedWordsCount(string text, string tagName)
        {
            int quoteWordsCount = 0;
            Regex exp = FeatureRegex.TaggedWords(tagName);
            foreach (Match match in exp.Matches(text))
            {
                MatchCollection wordMatches = FeatureRegex.Words.Matches(match.Groups[1].Value);
                quoteWordsCount += wordMatches.Count;
            }
            return quoteWordsCount;
        }

        private yaf_Message GetNthPost(int topicId, int n)
        {
            return buhonlineDataProvider.ReadMessageInPosition(topicId, n);
        }

        private yaf_Message GetTopicStarter(int topicId)
        {
            return GetNthPost(topicId, 0);
        }

        private yaf_Message GetPreviousPost(int topicId, int messageId)
        {
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            yaf_Message previous = null;
            int position = item.Position - 1;
            while (previous == null && position >= 0)
            {
                previous = GetNthPost(topicId, position);
                position -= 1;
            }
            return previous;
        }

        private yaf_User GetAuthor(int userId)
        {
            return buhonlineDataProvider.ReadUser(userId);
        }
    }
}