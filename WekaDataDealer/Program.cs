using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FeatureDealer.DataProviders;
using FeatureDealer.FeatureCalculators;
using FeatureDealer.FeatureSettings;
using FeatureDealer.Format;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: FeatureDealer.exe [E] [C] \r\n E = 'eval' or 'index'\r\n C is path to features configuration file like Test\\Features.xml");
                return;
            }
            string configuration = string.Empty;
            if (args.Length > 1) configuration = args[1];
            string evaluation = args[0];
            if (!(evaluation.Equals("eval") || evaluation.Equals("index")))
            {
                Console.WriteLine("Unknown E option {0}. Possible values are 'eval' or 'index'");
            }
            if (evaluation.Equals("eval"))
            {
                if (string.IsNullOrEmpty(configuration))
                    configuration = @"Test\Features.xml";
                CalculateEvaluationFeatures("./Evaluation.txt", configuration);
                return;
            }
            if (evaluation.Equals("index"))
            {
                if (string.IsNullOrEmpty(configuration))
                    configuration = @"Test\IndexFeatures.xml";
                CalculateIndexFeatures("./Index.txt", configuration);
                return;
            }
        }

        public static void PrepareData()
        {
            const string thankFeaturesFilename = @".\ThankFeatures.txt";
            if (!File.Exists(thankFeaturesFilename))
                Preprocessing.ImportThanksDataFromDB(thankFeaturesFilename);
            const string thankCloseWords = @".\thankCloseWords.txt";
            if (!File.Exists(thankCloseWords))
                Preprocessing.GenerateCloseWordsFor("спасибо", 2, thankCloseWords);
            Preprocessing.GetThanksPositionsInTopics();
            Preprocessing.FillCitationCounts();
        }

        public static void CalculateIndexFeatures(string indexFilepath, string featuresFilepath)
        {
            if (File.Exists(indexFilepath))
                File.Delete(indexFilepath);
            PrepareData();
            using (var buhonlineDataReader = new BuhonlineDataReader())
            using (var sparseFormatWriter = new SparseFormatWriter(indexFilepath))
            {
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

        public static void CalculateEvaluationFeatures(string evaluationFilepath, string featuresFilepath)
        {
            if (File.Exists(evaluationFilepath))
                File.Delete(evaluationFilepath);
            PrepareData();
            using (var evaluationDataReader = new EvaluationDataReader())
            using (var sparseFormatWriter = new SparseFormatWriter(evaluationFilepath))
            {
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
    }

    public class PostData
    {
        public int RequestId { get; set; }
        public int PostId { get; set; }
        public int Label { get; set; }
        public IEnumerable<Feature> Features { get; set; }
    }
}