using System;
using System.Collections.Generic;

namespace FeatureDealer.Converters
{
    class VectorToArffConverter
    {
        public readonly FeatureInfo[]
            FeatureInfos = {
                               new FeatureInfo {Name = "IsDateContained", Type = FeatureType.Bool},
                               new FeatureInfo {Name = "IsQuoteContained", Type = FeatureType.Bool},
                               new FeatureInfo {Name = "IsTopicStarterCited", Type = FeatureType.Bool},
                               new FeatureInfo {Name = "IsUrlContained", Type = FeatureType.Bool},
                               new FeatureInfo {Name = "IsFormulaContained", Type = FeatureType.Bool},
                               new FeatureInfo {Name = "IsSmileContained", Type = FeatureType.Bool},
                               new FeatureInfo {Name = "IsThankwordsContained", Type = FeatureType.Bool},
                               new FeatureInfo {Name = "IsSameAuthorAsTopicStarter", Type = FeatureType.Bool},
                               new FeatureInfo {Name = "IsUrlContained", Type = FeatureType.Bool},
                               new FeatureInfo {Name = "IsBuhWordsContained", Type = FeatureType.Bool},
                               new FeatureInfo {Name = "IsTaxWordsContained", Type = FeatureType.Bool},
                               new FeatureInfo {Name = "IsControlSystemWordsContained", Type = FeatureType.Bool},
                               new FeatureInfo {Name = "IsFormWordsContained", Type = FeatureType.Bool},
                               new FeatureInfo {Name = "IsEdited", Type = FeatureType.Bool},
                               new FeatureInfo {Name = "IsTextHighlighted", Type = FeatureType.Bool},

                               new FeatureInfo {Name = "QuestionMarksFeature"},
                               new FeatureInfo {Name = "PostPositionFeature"},
                               new FeatureInfo {Name = "PostLengthFeature"},
                               new FeatureInfo {Name = "PunctuationsCount"},
                               new FeatureInfo {Name = "DistinctPunctuationsCount"},
                               new FeatureInfo {Name = "QuoteMarksCount"},
                               new FeatureInfo {Name = "QuotedWordsCount"},
                               new FeatureInfo {Name = "QuotedWordsToAllCount"},
                               new FeatureInfo {Name = "UserQuotesCount"},
                               new FeatureInfo {Name = "QuoteWordsToAllWordsCount"},
                               new FeatureInfo {Name = "MaxQuoteDeepness"},
                               new FeatureInfo {Name = "BBTagsCount"},
                               new FeatureInfo {Name = "AccentedWordsCount"},
                               new FeatureInfo {Name = "AuthorRating"},
                               new FeatureInfo {Name = "AuthorForumAge"},
                               new FeatureInfo {Name = "AuthorProfession", Type = FeatureType.String},
                               new FeatureInfo {Name = "AuthorPostsCount"},
                               new FeatureInfo {Name = "DigitsCount"},
                               new FeatureInfo {Name = "NumbersCount"},
                               new FeatureInfo {Name = "UpperLettersCount"},
                               new FeatureInfo {Name = "StopWordsToAllWordsCount"},
                               new FeatureInfo {Name = "PositionInThread"},
                               new FeatureInfo {Name = "TimeFromTopicStarter"},
                               new FeatureInfo {Name = "TimeFromPreviousPost"},
                               new FeatureInfo {Name = "ThreadLength"},
                               new FeatureInfo {Name = "QuotientPostLengthToAvarageInTopic"},
                               new FeatureInfo {Name = "Points"},
                               new FeatureInfo {Name = "BBTagsCount"},
                               new FeatureInfo {Name = "TopicCitedCount"},
                               new FeatureInfo {Name = "MessageCitedCount"}
                           };

        public VectorToArffConverter(List<object> vector)
        {
            if (vector.Count != FeatureInfos.Length)
                throw new Exception("Vector size does not fit to size of Infos array.");
        }
    }

    internal class FeatureInfo
    {
        public string Name;
        public string Type = FeatureType.Numeric;
    }

    internal static class FeatureType
    {
        public const string Bool = "{True, False}";
        public const string Numeric = "numeric";
        public const string String = "string";
    }

}
