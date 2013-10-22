using System.IO;
using System.Xml.Serialization;

namespace FeatureDealer.FeatureSettings
{
    public class FeatureListBuilder
    {
        public static FeatureList GetFeaturesList(string settingFilepath)
        {
            using (var fileStream = File.OpenRead(settingFilepath))
            {
                var deserializedXml = new XmlSerializer(typeof (FeatureList)).Deserialize(fileStream);
                var featureList = deserializedXml as FeatureList;
                return featureList;
            }
        }
    }
}