using System.Collections.Generic;
using System.IO;
using System.Linq;
using FeatureDealer.FeatureCalculators;
using NUnit.Framework;

namespace FeatureDealer.Format
{
    [TestFixture]
    public class SparseFormatTest
    {
        private const string sparseFeaturesFilepath = @"Test\BuhFeatureVectors.txt";
        private const string expectedUpdatedFeaturesFilepath = @"Test\ExpectedUpdatedFeatures.sparse";

        [Test]
        public void TestAddFeatureIsCorrect()
        {
            const string updatedFeaturesFilepath = @"Test\test.sparse";
            var postData = new List<PostData>
                               {
                                   new PostData
                                       {
                                           RequestId = 1,
                                           PostId = 328571,
                                           Features = new[]
                                                          {
                                                              new Feature {Number = 111, Value = 111},
                                                              new Feature {Number = 222, Value = 222}
                                                          }
                                       },
                                   new PostData
                                       {
                                           RequestId = 42,
                                           PostId = 271594,
                                           Features = new[]
                                                          {
                                                              new Feature {Number = 333, Value = 333},
                                                              new Feature {Number = 444, Value = 444}
                                                          }
                                       }
                               };
            SparseFormatWriter.AddFeaturesToExisting(postData, sparseFeaturesFilepath, updatedFeaturesFilepath, true);
            var updatedTemplateFileInfo = new FileInfo(expectedUpdatedFeaturesFilepath);
            var updatedFeaturesFileInfo = new FileInfo(updatedFeaturesFilepath);
            FileAssert.AreEqual(updatedTemplateFileInfo, updatedFeaturesFileInfo);
        }

        [Test]
        public void TestSaveFeaturesIsCorrect()
        {
            var featureData = new List<PostData>
                                  {
                                      new PostData
                                          {
                                              PostId = 201,
                                              RequestId = 2,
                                              Features = new[]
                                                             {
                                                                 new Feature
                                                                     {
                                                                         Name = "queryWindowRatio",
                                                                         Value = 0.45454546809196472,
                                                                         Number = 51
                                                                     },
                                                                 new Feature
                                                                     {
                                                                         Name = "coveredQueryTermNumber",
                                                                         Value = 2.0,
                                                                         Number = 52
                                                                     },
                                                                 new Feature
                                                                     {
                                                                         Name = "hasQueryWordsSequence",
                                                                         Value = 0.0,
                                                                         Number = 53
                                                                     },
                                                                 new Feature
                                                                     {
                                                                         Name = "queryWordsTopicStarterRatio",
                                                                         Value = 0.0,
                                                                         Number = 54
                                                                     },
                                                                 new Feature
                                                                     {
                                                                         Name = "answerWordsTopicStarterRatio",
                                                                         Value = 1.0,
                                                                         Number = 55
                                                                     }
                                                             }
                                          },
                                      new PostData
                                          {
                                              PostId = 102,
                                              RequestId = 3,
                                              Features = new[]
                                                             {
                                                                 new Feature
                                                                     {
                                                                         Name = "queryWindowRatio",
                                                                         Value = 0.44117647409439087,
                                                                         Number = 51
                                                                     },
                                                                 new Feature
                                                                     {
                                                                         Name = "coveredQueryTermNumber",
                                                                         Value = 3.0,
                                                                         Number = 52
                                                                     },
                                                                 new Feature
                                                                     {
                                                                         Name = "hasQueryWordsSequence",
                                                                         Value = 0.0,
                                                                         Number = 53
                                                                     },
                                                                 new Feature
                                                                     {
                                                                         Name = "queryWordsTopicStarterRatio",
                                                                         Value = 0.3333333432674408,
                                                                         Number = 54
                                                                     },
                                                                 new Feature
                                                                     {
                                                                         Name = "answerWordsTopicStarterRatio",
                                                                         Value = 1.0,
                                                                         Number = 55
                                                                     }
                                                             }
                                          }
                                  };
            const string savedFeaturesFilepath = @"Test\SavedFeatures.sparse";
            const string expectedSavedFeaturesFilepath = @"Test\ExpectedSavedFeatures.sparse";
            SparseFormatWriter.Save(savedFeaturesFilepath,
                              featureData.Select(
                                  f =>
                                  new SparseFormatWriter.SparseData
                                      {Features = f.Features, PostId = f.PostId, Qid = f.RequestId}));
            var savedFeaturesFileInfo = new FileInfo(savedFeaturesFilepath);
            var expectedFeaturesFileInfo = new FileInfo(expectedSavedFeaturesFilepath);
            FileAssert.AreEqual(savedFeaturesFileInfo, expectedFeaturesFileInfo);
        }

        [Test]
        public void TestRemoveFeaturesByNumber()
        {
            SparseFormatWriter.RemoveFeaturesWithNumbers(new[] { 45, 46 }, @"Test\BuhFeatureVectors.txt", @"Test\RemovedBuhFeatureVectors.txt");
            var removedBuhFeaturesFileInfo = new FileInfo(@"Test\RemovedBuhFeatureVectors.txt");
            var expectedRemovedBuhFeaturesFileInfo = new FileInfo(@"Test\ExpectedRemovedBuhFeatureVectors.txt");
            FileAssert.AreEqual(removedBuhFeaturesFileInfo, expectedRemovedBuhFeaturesFileInfo);
        }

        [Test]
        public void TestSortFeatures()
        {
            const string sortedFeaturesFilepath = @"Test\SortedFeatureVectors.txt";
            SparseFormatWriter.SortFeatures(@"Test\UnsortedFeatureVectors.txt", sortedFeaturesFilepath);
            var sortedFeaturesFileInfo = new FileInfo(sortedFeaturesFilepath);
            var expectedSortedFeatureVectors = new FileInfo(@"Test\ExpectedSortedFeatureVectors.txt");
            FileAssert.AreEqual(sortedFeaturesFileInfo, expectedSortedFeatureVectors);
        }

        [Test]
        public void TestSaveMetadata()
        {
            IEnumerable<Feature> features = new []{new Feature{Name = "TestFeature1", Number = 1}, new Feature{Name = "TestFeature2", Number = 2} };
            const string savedFeaturesMetadataFilepath = @"Test\FeaturesMetadata.txt";
            SparseFormatWriter.SaveMetadata(features, savedFeaturesMetadataFilepath);
            var savedFeaturesFileInfo = new FileInfo(savedFeaturesMetadataFilepath);
            var expectedSavedFeaturesFileInfo = new FileInfo(@"Test\ExpectedSavedFeaturesMetadata.txt");
            FileAssert.AreEqual(savedFeaturesFileInfo, expectedSavedFeaturesFileInfo);
        }
    }
}