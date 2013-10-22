using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FeatureDealer.DataProviders;
using FeatureDealer.FeatureCalculators;
using FeatureDealer.Models;
using FeatureDealer.Models.MappedClasses;
using cqa_medical.UtilitsNamespace;

namespace FeatureDealer
{
    public static class Preprocessing
    {
        static Preprocessing()
        {
            // toto galina использовать provider
            buhonlineDataReader = new BuhonlineDataReader();
        }

        public static void GetThanksPositionsInTopics()
        {
            var topics = new List<yaf_Topic>();
            var thankDict = FileProcessing.MessageIdAndIsThankPost.ToDictionary();
            foreach (var topic in topics)
            {
                var topicId = topic.TopicID;
                var messages = new List<yaf_Message>();
                var posts = messages.Where(m => thankDict[m.MessageID] == 1).Select(m => m.Position);
                ThanksPositionsInTopics.Add(topicId, new HashSet<int>(posts));
            }
        }

        public static void GenerateCloseWordsFor(string expectedWord, int maxDistance, string fileToSave)
        {
            var closeWords = new HashSet<string>();
            var len = expectedWord.Length;
            var context = new BuhonlineContext();
            var messages = context.yaf_Message.Select(p => p.Message);
            foreach (var message in messages)
            {
                foreach (var s in message.StripQuotesStripBBcodeSplitInWordsLower())
                {
                    if (s.Length < len - 1 || s.Length > len + 1) continue;
                    var distance = expectedWord.LevensteinDistance(s);
                    if (distance <= maxDistance)
                        closeWords.Add(s);
                }
            }
            var distinctCloseWords = closeWords.SelectMany(s => s.SplitIntoWordsLower()).DistinctAndNotWhitespace();
            File.WriteAllLines(fileToSave, distinctCloseWords.OrderBy(s => s));
        }

        private static void ReadStopwords()
        {
            stopWords = new HashSet<string>(File.ReadAllLines("Dictionaries/stopwords.txt").Select(w => w.Trim()));
        }

        private static void ReadHelloWords()
        {
            helloWords = new HashSet<string>(File.ReadAllLines("Dictionaries/HelloWords.txt").Select(w => w.Trim()));
        }

        private static void ReadThankWords()
        {
            thankwords = new HashSet<string>(File.ReadAllLines("Dictionaries/thankwords.txt").Select(w => w.Trim()));
        }

        public static void ImportThanksDataFromDB(string fileToSave)
        {
            // todo импорт данных никакого отношения непосредственно к предобработке не имеет
            var reader = new BuhonlineDataReader();
            List<IDataItem> posts = reader.Read().Where(post => !(((yaf_Message)post).Message.ToLower().StartsWith("спасибо") || ((yaf_Message)post).Message.ToLower().StartsWith("благодар"))).ToList();
            const int batch = 2000;
            if (posts.Count > batch)
            {
                var randomPosts = new List<IDataItem>();
                var rand = new Random();
                for (var i = 0; i < batch; i++)
                {
                    var id = rand.Next(posts.Count - i);
                    randomPosts.Add(posts[id]);
                    var post = posts[posts.Count - 1 - i];
                    posts[posts.Count - 1 - i] = posts[id];
                    posts[id] = post;
                }

                File.WriteAllLines(fileToSave, randomPosts.Select(item => item.ToString()));
            }
        }

        private static readonly Dictionary<int, int> topicCitedCounts = new Dictionary<int, int>();
        private static readonly Dictionary<int, int> messageCitedCounts = new Dictionary<int, int>();
        private static readonly Dictionary<int, HashSet<int>> thanksPositionsInTopics = new Dictionary<int, HashSet<int>>();
        private static HashSet<string> stopWords;
        private static HashSet<string> helloWords;
        private static HashSet<string> thankwords;
        private static readonly BuhonlineDataReader buhonlineDataReader;

        public static Dictionary<int, int> TopicCitedCounts
        {
            get { return topicCitedCounts; }
        }

        public static Dictionary<int, int> MessageCitedCounts
        {
            get { return messageCitedCounts; }
        }

        public static Dictionary<int, HashSet<int>> ThanksPositionsInTopics
        {
            get { return thanksPositionsInTopics; }
        }

        public static HashSet<string> StopWords
        {
            get
            {
                if (stopWords == null) ReadStopwords();
                return stopWords;
            }
        }

        public static HashSet<string> HelloWords
        {
            get
            {
                if (helloWords == null) ReadHelloWords();
                return helloWords;
            }
        }

        public static HashSet<string> ThankWords
        {
            get
            {
                if (thankwords == null) ReadThankWords();
                return thankwords;
            }
        }

        public static void FillCitationCounts()
        {
            var messages = buhonlineDataReader.ReadMessageTexts();
            // note считает сколько раз в сообщениях отсылают на ответ или вопрос с некоторым идентификатором
            Regex topicEx = FeatureRegex.TopicNum;
            Regex messageEx = FeatureRegex.MessageNum;
            foreach (string message in messages)
            {
                foreach (Match match in topicEx.Matches(message))
                {
                    int topicId = int.Parse(match.Groups[1].Value);
                    if (!topicCitedCounts.ContainsKey(topicId))
                        topicCitedCounts[topicId] = 0;
                    topicCitedCounts[topicId]++;
                }

                foreach (Match match in messageEx.Matches(message))
                {
                    int messageId = int.Parse(match.Groups[1].Value);
                    if (!messageCitedCounts.ContainsKey(messageId))
                        messageCitedCounts[messageId] = 0;
                    messageCitedCounts[messageId]++;
                }
            }
        }
    }
}