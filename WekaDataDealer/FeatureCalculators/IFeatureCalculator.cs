using System.Collections.Generic;

namespace FeatureDealer.FeatureCalculators
{
    interface IFeatureCalculator
    {
        IEnumerable<Feature> Calculate(IFeatureParameters data);
    }
}
