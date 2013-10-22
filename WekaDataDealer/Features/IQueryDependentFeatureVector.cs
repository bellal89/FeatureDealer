using System.Collections.Generic;
using FeatureDealer.FeatureCalculators;
using FeatureDealer.FeatureSettings;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.Features
{
    internal interface IQueryDependentFeatureVector
    {
        [GetValue]
        IEnumerable<Feature> GetValues(yaf_Message message, string query, yaf_Message topicStarter);
    }
}