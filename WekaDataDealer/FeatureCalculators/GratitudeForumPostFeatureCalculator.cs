using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.FeatureCalculators
{
    internal class GratitudeForumPostFeatureCalculator : IFeatureCalculator
    {
        private static readonly Regex thankWordRegex = Utilites.AlternativeRegex(FileProcessing.CloseWordsToThank);

        public List<List<object>> GetFeatureVectors(IEnumerable<IDataItem> dataItems)
        {
            return dataItems.Select(item => CalculateFeatures(item as yaf_Message)).ToList();
        }

        public List<object> CalculateFeatures(yaf_Message item)
        {
            var strippedMessage = item.Message.StripOutBBCodeQuotes().StripBBCodeTags().ToLowerInvariant();
            return new List<object>
                       {
                           WordThanksCount(strippedMessage),
                           LengthWithoutQuotes(strippedMessage),
                           SentencesCount(strippedMessage),
                           EndPointsCount(strippedMessage),
                           ExclamationsCount(strippedMessage),
                           QuotientExclamationsToSymbols(strippedMessage),
                           //FeatureCalculator.PositionInThread(item),
                           //FeatureCalculator.DistanceToEnd(item)
                       };
        }

        private static int WordThanksCount(string strippedMessage)
        {
            var matches = thankWordRegex.Match(strippedMessage);
            return matches.Captures.Count;
        }
        private static int LengthWithoutQuotes(string strippedMessage)
        {
            return strippedMessage.Length;
        }
        private static int SentencesCount(string strippedMessage)
        {
            return strippedMessage.Split('!', '?', '.').DistinctAndNotWhitespace().Count();
        }
        private static int EndPointsCount(string strippedMessage)
        {
            return strippedMessage.Count(c => c == '!' || c == '.');
        }
        private static int ExclamationsCount(string strippedMessage)
        {
            return strippedMessage.Count(c => c == '!');
        }
        private static double QuotientExclamationsToSymbols(string strippedMessage)
        {
            return (double)ExclamationsCount(strippedMessage) / (strippedMessage).Length;
        }

        public IEnumerable<Feature> Calculate(IFeatureParameters data)
        {
            // todo
            return null;
        }
    }
}
