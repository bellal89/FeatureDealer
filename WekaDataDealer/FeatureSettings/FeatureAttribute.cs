using System;

namespace FeatureDealer.FeatureSettings
{
    public class FeatureAttribute : Attribute
    {
        private readonly string featureName;

        public FeatureAttribute(string featureName)
        {
            this.featureName = featureName;
        }

        public bool Ignore { get; set; }

        public string FeatureName
        {
            get { return featureName; }
        }
    }

    public class GetValueAttribute:Attribute{}
    public class GetDescriptionAttribute:Attribute{}
    public class GetNumberAttribute:Attribute{}
    public class GetNameAttribute:Attribute{}
}