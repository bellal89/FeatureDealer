using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FeatureDealer.FeatureCalculators;

namespace FeatureDealer.Converters
{
    class GratitudeVectorToArffConverter
    {
        public readonly FeatureInfo[]
            FeatureInfos = {
                               new FeatureInfo {Name = "WordThanksCount"},
                               new FeatureInfo {Name = "LengthWithoutQuotes"},
                               new FeatureInfo {Name = "SentencesCount"},
                               new FeatureInfo {Name = "EndPointsCount"},
                               new FeatureInfo {Name = "ExclamationsCount"},
                               new FeatureInfo {Name = "QuotientExclamationsToSymbols"},
                               new FeatureInfo {Name = "DistanceToTopicStarter"},
                               new FeatureInfo {Name = "DistanceToEnd"},
//                               new FeatureInfo {Name = "label"},
                               new FeatureInfo {Name = "MessageId"},
                           };

//        public GratitudeVectorToArffConverter(List<object> vector)
//        {
//            if (vector.Count != FeatureInfos.Length)
//                throw new Exception("Vector size does not fit to size of Infos array.");
//        }
    }
}
