using System.Collections.Generic;
using System.Linq;
using FeatureDealer.FeatureCalculators;
using FeatureDealer.FeatureSettings;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.Features
{
    public class QueryDependentFeatures
    {
        private readonly BuhOnlineDataProvider buhonlineDataProvider;

        public QueryDependentFeatures()
        {
            buhonlineDataProvider = new BuhOnlineDataProvider();
        }

        private double TextsWindowRatio(string text1, string text2)
        {
            // note отношение расстояния от первого слова из text2 до последнего в text1 к длине text1 (соотношение считается в символах)
            int minWordIndex = int.MaxValue;
            int maxWordIndex = int.MinValue;
            List<string> text1Words = text1.SplitIntoWordsLower().ToList();
            IEnumerable<string> text2Words = text2.SplitIntoWordsLower();
            foreach (string word in text2Words)
            {
                int firstWordIndex = text1Words.IndexOf(word);
                if (firstWordIndex >= 0 && firstWordIndex < minWordIndex) minWordIndex = firstWordIndex;
                int lastWordIndex = text1Words.LastIndexOf(word);
                if (lastWordIndex >= 0 && lastWordIndex > maxWordIndex) maxWordIndex = lastWordIndex;
            }
            int distance = maxWordIndex - minWordIndex;
            float postLength = text1.Length;
            if (postLength == 0) return 0;
            return distance/postLength;
        }

        [QueryDependentFeature("answerQueryWindowRatio")]
        public double AnswerQueryWindowRatio(QueryDependentFeatureParameters parameters)
        {
            string anwerText = GetAnswer(parameters.MessageId);
            return TextsWindowRatio(anwerText, parameters.Query);
        }

        [QueryDependentFeature("questionQueryWindowRatio")]
        public double QuestionQueryWindowRatio(QueryDependentFeatureParameters parameters)
        {
            string questionText = GetQuestion(parameters.MessageId);
            return TextsWindowRatio(questionText, parameters.Query);
        }

        private static int CoveredQueryTermNumber(string text1, string text2)
        {
            // note число одинаковых термов в text1 и text2
            IEnumerable<string> text2Words = text2.SplitIntoWordsLower();
            return text2Words.Count(text1.Contains);
        }

        private string GetAnswer(int messageId)
        {
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            return item.Message;
        }

        private string GetQuestion(int messageId)
        {
            yaf_Message item = buhonlineDataProvider.ReadMessage(messageId);
            int topicId = item.TopicID;
            yaf_Message topicStarter = buhonlineDataProvider.ReadTopicStarter(topicId);
            return topicStarter.Message;
        }

        [QueryDependentFeature("coveredAnswerQueryTermNumber")]
        public int CoveredAnswerQueryTermNumber(QueryDependentFeatureParameters parameters)
        {
            string answerText = GetAnswer(parameters.MessageId);
            return CoveredQueryTermNumber(answerText, parameters.Query);
        }

        [QueryDependentFeature("coveredQuestionQueryTermNumber")]
        public int CoveredQuestionQueryTermNumber(QueryDependentFeatureParameters parameters)
        {
            string questionText = GetQuestion(parameters.MessageId);
            return CoveredQueryTermNumber(questionText, parameters.Query);
        }

        private bool HasQueryWordsSequence(string answer, string query)
        {
            // note точное совпадение последовательности слов запроса в ответе (посте)
            return answer.Contains(query);
        }

        [QueryDependentFeature("answerHasQueryWordsSequence")]
        public bool AnswerHasQueryWordsSequence(QueryDependentFeatureParameters parameters)
        {
            string answerText = GetAnswer(parameters.MessageId);
            return HasQueryWordsSequence(answerText, parameters.Query);
        }

        [QueryDependentFeature("questionHasQueryWordsSequence")]
        public bool QuestionHasQueryWordsSequence(QueryDependentFeatureParameters parameters)
        {
            string questionText = GetQuestion(parameters.MessageId);
            return HasQueryWordsSequence(questionText, parameters.Query);
        }

        private static double QueryWordsRatioInMetadocument(string text1, string text2, string query)
        {
            // note metadocument = question + answer
            // note отношение количества слов из query в text1Words к длине text1Words + text2Words
            IEnumerable<string> queryWords = query.SplitIntoWordsLower();
            IEnumerable<string> text1Words = text1.SplitIntoWordsLower();
            IEnumerable<string> text2Words = text2.SplitIntoWordsLower();
            float count = queryWords.Count(text1Words.Contains);
            float documentLength = text1Words.Count() + text2Words.Count();
            return count/documentLength;
        }

        [QueryDependentFeature("queryWordsInQuestionMetadocumentRatio")]
        public double QueryWordsInQuestionMetadocumentRatio(QueryDependentFeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            string answerText = GetAnswer(messageId);
            string questionText = GetQuestion(messageId);
            return QueryWordsRatioInMetadocument(questionText, answerText, parameters.Query);
        }

        [QueryDependentFeature("queryWordsInAnswerMetadocumentRatio")]
        public double QueryWordsInAnswerMetadocumentRatio(QueryDependentFeatureParameters parameters)
        {
            int messageId = parameters.MessageId;
            string answerText = GetAnswer(messageId);
            string questionText = GetQuestion(messageId);
            return QueryWordsRatioInMetadocument(answerText, questionText, parameters.Query);
        }

        private static double QueryWordsRatio(string text, string query)
        {
            IDictionary<string, int> counts = new Dictionary<string, int>();
            IEnumerable<string> textWords = text.SplitIntoWordsLower();
            IEnumerable<string> queryWords = query.SplitIntoWordsLower();
            float count = queryWords.Count(textWords.Contains);
            float textLength = textWords.Count();
            return count/textLength;
        }

        [QueryDependentFeature("answerQueryWordsRatio")]
        public double AnswerQueryWordsRatio(QueryDependentFeatureParameters parameters)
        {
            string answerText = GetAnswer(parameters.MessageId);
            return QueryWordsRatio(answerText, parameters.Query);
        }

        [QueryDependentFeature("questionQueryWordsRatio")]
        public double QuestionQueryWordsRatio(QueryDependentFeatureParameters parameters)
        {
            string questionText = GetQuestion(parameters.MessageId);
            return QueryWordsRatio(questionText, parameters.Query);
        }

        private static int QueryWordsMaxSequenceLength(string text, string query)
        {
            // note самая длинная последовательность слов запроса (без учета порядка слов запроса)
            IEnumerable<string> textWords = text.SplitIntoWordsLower();
            var counts = new List<int>();
            int counter = 0;
            foreach (string textWord in textWords)
            {
                if (query.Contains(textWord))
                    counter += 1;
                else
                {
                    counts.Add(counter);
                    counter = 0;
                }
            }
            if (counts.Count == 0)
                return 0;
            return counts.Max();
        }

        [QueryDependentFeature("queryWordsMaxSequenceInAnswerLength")]
        public int QueryWordsMaxSequenceInAnswerLength(QueryDependentFeatureParameters parameters)
        {
            string answerText = GetAnswer(parameters.MessageId);
            return QueryWordsMaxSequenceLength(answerText, parameters.Query);
        }

        [QueryDependentFeature("queryWordsMaxSequenceInQuestionLength")]
        public int QueryWordsMaxSequenceInQuestionLength(QueryDependentFeatureParameters parameters)
        {
            string questionText = GetQuestion(parameters.MessageId);
            return QueryWordsMaxSequenceLength(questionText, parameters.Query);
        }

        private double TextQueryLengthDifference(string text, string query)
        {
            // note разница считается в количестве слов
            IEnumerable<string> textWords = text.SplitIntoWordsLower();
            IEnumerable<string> queryWords = query.SplitIntoWordsLower();
            return textWords.Count() - queryWords.Count();
        }

        [QueryDependentFeature("answerQueryLengthDifference")]
        public double AnswerQueryLengthDifference(QueryDependentFeatureParameters parameters)
        {
            string answerText = GetAnswer(parameters.MessageId);
            return TextQueryLengthDifference(answerText, parameters.Query);
        }

        [QueryDependentFeature("questionQueryLengthDifference")]
        public double QuestionQueryLengthDifference(QueryDependentFeatureParameters parameters)
        {
            string questionText = GetQuestion(parameters.MessageId);
            return TextQueryLengthDifference(questionText, parameters.Query);
        }

        private double TextQueryLengthRatio(string text, string query)
        {
            IEnumerable<string> textWords = text.SplitIntoWordsLower();
            IEnumerable<string> queryWords = query.SplitIntoWordsLower();
            return textWords.Count()/queryWords.Count();
        }

        [QueryDependentFeature("answerQueryLengthRatio")]
        public double AnswerQueryLengthRatio(QueryDependentFeatureParameters parameters)
        {
            string answerText = GetAnswer(parameters.MessageId);
            return TextQueryLengthRatio(answerText, parameters.Query);
        }

        [QueryDependentFeature("questionQueryLengthRatio")]
        public double QuestionQueryLengthRatio(QueryDependentFeatureParameters parameters)
        {
            string questionText = GetQuestion(parameters.MessageId);
            return TextQueryLengthRatio(questionText, parameters.Query);
        }
    }
}