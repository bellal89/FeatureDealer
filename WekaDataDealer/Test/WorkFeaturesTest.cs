using System.Collections.Generic;
using System.IO;
using System.Linq;
using FeatureDealer.DataProviders;
using FeatureDealer.FeatureCalculators;
using FeatureDealer.FeatureSettings;
using FeatureDealer.Format;
using FeatureDealer.Models.MappedClasses;
using NUnit.Framework;

namespace FeatureDealer.Test
{
    [TestFixture]
    internal class WorkFeaturesTest
    {
        [Test]
        public void TestCalculateEvaluationFeatures()
        {
            const string evaluationFilepath = @"Test\Evaluation.txt";
            if (File.Exists(evaluationFilepath))
                File.Delete(evaluationFilepath);
            Program.PrepareData();
            using (var evaluationDataReader = new EvaluationDataReader())
            using (var sparseFormatWriter = new SparseFormatWriter(evaluationFilepath))
            {
                const string featuresFilepath = @"Test\Features.xml";
                var featuresList = FeatureListBuilder.GetFeaturesList(featuresFilepath);
                var requests = evaluationDataReader.ReadRequests();
                var featureCalculator = new FeatureCalculator(featuresList);
                var qdFeatureCalculator = new QueryDependentFeatureCalculator(featuresList);
                foreach (var request in requests)
                {
                    IEnumerable<Evaluation> evaluations = evaluationDataReader.ReadEvaluation(request.Id).AsEnumerable();
                    foreach (var evaluation in evaluations)
                    {
                        var messageId = evaluation.PostId;
                        IEnumerable<Feature> qiFeatures = featureCalculator.Calculate(new FeatureParameters { MessageId = messageId });
                        IEnumerable<Feature> qdFeatures = qdFeatureCalculator.Calculate(new QueryDependentFeatureParameters { MessageId = messageId, Query = request.Text });
                        var features = qiFeatures.Concat(qdFeatures);
                        int relevance = evaluation.Relevance ?? 0;
                        sparseFormatWriter.Append(new List<PostData> { new PostData { PostId = messageId, Features = features, RequestId = evaluation.RequestId, Label = relevance } });
                    }
                }
            }
        }

        [Test]
        public void TestCalculateIndexFeatures()
        {
            const string indexFilepath = @"Test\Index.txt";
            if (File.Exists(indexFilepath))
                File.Delete(indexFilepath);
            Program.PrepareData();
            using (var buhonlineDataReader = new BuhonlineDataReader())
            using (var sparseFormatWriter = new SparseFormatWriter(indexFilepath))
            {
                const string featuresFilepath = @"Test\IndexFeatures.xml";
                var featuresList = FeatureListBuilder.GetFeaturesList(featuresFilepath);
                var messageIds = buhonlineDataReader.ReadAnswerIds();
                var featureCalculator = new FeatureCalculator(featuresList);
                foreach (int messageId in messageIds)
                {
                    var featureVectors = new List<PostData>();
                    IEnumerable<Feature> messageFeatures = featureCalculator.Calculate(new FeatureParameters { MessageId = messageId });
                    featureVectors.Add(new PostData { PostId = messageId, Features = messageFeatures });
                    sparseFormatWriter.Append(featureVectors);
                }
            }
        }
    }
}