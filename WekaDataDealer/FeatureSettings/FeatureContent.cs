using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FeatureDealer.FeatureSettings
{
    [XmlRoot("features")]
    public class FeatureList
    {
        [Obsolete]
        public FeatureList()
        {
        }

        [XmlElement("feature")]
        public List<FeatureContent> Features { get; set; }
    }

    [Serializable]
    public class FeatureContent
    {
        [Obsolete]
        public FeatureContent()
        {
        }

        public FeatureContent(string featureName)
        {
            FeatureName = featureName;
        }

        [XmlAttribute("name")]
        public string FeatureName { get; set; }

        [XmlAttribute("ignore")]
        public bool Ignore { get; set; }

        [XmlAttribute("queryDependent")]
        public bool QueryDependent { get; set; }

        [XmlAttribute("group")]
        public bool Group { get; set; }

        [XmlAttribute("number")]
        public int Number { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as FeatureContent;
            if (other == null) return false;
            return Equals(other.FeatureName, FeatureName);
        }

        public override int GetHashCode()
        {
            return FeatureName.GetHashCode();
        }
    }
}