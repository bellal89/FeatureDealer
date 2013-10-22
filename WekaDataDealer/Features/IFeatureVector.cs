using System.Collections.Generic;
using FeatureDealer.FeatureCalculators;
using FeatureDealer.FeatureSettings;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.Features
{
    interface IFeatureVector
    {
        [GetValue]
        IEnumerable<Feature> GetValues(yaf_Message message);
    }
}