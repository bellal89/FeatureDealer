using System;

namespace FeatureDealer.FeatureSettings
{
    public class QueryDependentFeatureAttribute:Attribute
    {
        private readonly string featureName;

        public QueryDependentFeatureAttribute(string featureName)
        {
            this.featureName = featureName;
        }

        public bool Ignore { get; set; }

        public string FeatureName
        {
            get { return featureName; }
        }
    }
}