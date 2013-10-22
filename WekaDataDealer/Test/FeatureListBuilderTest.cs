using System.Collections.Generic;
using FeatureDealer.FeatureSettings;
using NUnit.Framework;
using System.Linq;

namespace FeatureDealer.Test
{
    [TestFixture]
    public class FeatureListBuilderTest
    {
        [Test]
        public void Test()
        {
            const string settingFilepath = @".\Test\Features.xml";
            FeatureList featuresList = FeatureListBuilder.GetFeaturesList(settingFilepath);
            IEnumerable<FeatureContent> features = featuresList.Features;
            var notIgnoredFeatures = features.Where(f => !f.Ignore).Select(f=>f.FeatureName);
            var expectedNotInoredFeatures = new List<string> { "idf", "DigitsCount", "SolrQueryDependent" };
            Assert.That(notIgnoredFeatures, Is.EquivalentTo(expectedNotInoredFeatures));
        }
    }
}