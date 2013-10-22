using FeatureDealer.FeatureSettings;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.Features
{
    internal interface IFeature
    {
        [GetValue]
        double GetValue(yaf_Message message);
    }
}