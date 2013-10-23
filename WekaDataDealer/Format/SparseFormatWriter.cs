using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FeatureDealer.FeatureCalculators;

namespace FeatureDealer.Format
{
    internal class SparseFormatWriter : IFormatWriter, IDisposable
    {
        private static FileStream fileStream;
        private readonly StreamWriter streamWriter;

        public SparseFormatWriter(string featuresFilepath)
        {
            fileStream = File.Open(featuresFilepath, FileMode.Append);
            streamWriter = new StreamWriter(fileStream);
        }

        #region IDisposable Members

        public void Dispose()
        {
            streamWriter.Close();
            fileStream.Close();
        }

        #endregion

        public static void Save(string featuresFilepath, IEnumerable<SparseData> vectors)
        {
            using (FileStream fileStream = File.Create(featuresFilepath))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    foreach (SparseData vector in vectors)
                        streamWriter.WriteLine(vector);
                }
            }
        }

        public static void Save(string featuresFilepath, IEnumerable<PostData> vectors)
        {
            using (FileStream fileStream = File.Create(featuresFilepath))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    foreach (PostData vector in vectors)
                        streamWriter.WriteLine(new SparseData {PostId = vector.PostId, Features = vector.Features});
                }
            }
        }

        public void Append(IEnumerable<PostData> vectors)
        {
            foreach (PostData vector in vectors)
                streamWriter.WriteLine(new SparseData
                                           {
                                               PostId = vector.PostId,
                                               Features = vector.Features,
                                               Label = vector.Label,
                                               Qid = vector.RequestId
                                           });
        }

        public static void GroupByIds(string featuresFilepath, string resultFilepath)
        {
            // note загружает все в память
            IEnumerable<string> lines = File.ReadLines(featuresFilepath);
            var samples = new List<SparseData>();
            using (FileStream fileStream = File.Create(resultFilepath))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    samples.AddRange(lines.Select(SparseData.Parse));
                    IOrderedEnumerable<SparseData> orderedSamples = samples.OrderBy(s => s.Qid);
                    foreach (SparseData sample in orderedSamples)
                    {
                        string sparseLine = sample.ToString();
                        streamWriter.WriteLine(sparseLine);
                    }
                }
            }
        }

        public static void SaveMetadata(IEnumerable<Feature> features, string metaFilepath)
        {
            using (FileStream metaStream = File.Create(metaFilepath))
            {
                using (var metaWriter = new StreamWriter(metaStream))
                {
                    foreach (Feature feature in features)
                    {
                        metaWriter.WriteLine(string.Format("{0} {1}", feature.Name, feature.Number));
                    }
                }
            }
        }

        public static void SortFeatures(string filepath, string sortedFilepath)
        {
            IEnumerable<string> lines = File.ReadLines(filepath);
            using (FileStream fileStream = File.Create(sortedFilepath))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    foreach (string line in lines)
                    {
                        SparseData sparseData = SparseData.Parse(line);
                        List<Feature> oldFeatures = sparseData.Features.ToList();
                        oldFeatures.Sort(new FeatureComparer());
                        sparseData.Features = oldFeatures;
                        string sparseLine = sparseData.ToString();
                        streamWriter.WriteLine(sparseLine);
                    }
                }
            }
        }

        public static void ReplaceScore(string filepath, string resultFilepath, IDictionary<int, int> scoreReplacements)
        {
            IEnumerable<string> lines = File.ReadLines(filepath);
            using (FileStream fileStream = File.Create(resultFilepath))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    foreach (string line in lines)
                    {
                        SparseData sparseData = SparseData.Parse(line);
                        int label = sparseData.Label;
                        if (scoreReplacements.ContainsKey(label))
                            label = scoreReplacements[label];
                        sparseData.Label = label;
                        string sparseLine = sparseData.ToString();
                        streamWriter.WriteLine(sparseLine);
                    }
                }
            }
        }

        public static IEnumerable<int> ReadPostIds(string filepath)
        {
            IEnumerable<string> readLines = File.ReadLines(filepath);
            return readLines.Select(SparseData.Parse).Select(sparseData => sparseData.PostId);
        }

        public static IEnumerable<KeyValuePair<int, int>> ReadQids(string filepath)
        {
            IEnumerable<string> readLines = File.ReadLines(filepath);
            return readLines.Select(SparseData.Parse).Select(s => new KeyValuePair<int, int>(s.Qid, s.PostId));
        }

        public static IEnumerable<int> ReadUniqueQids(string filepath)
        {
            IEnumerable<string> readLines = File.ReadLines(filepath);
            return readLines.Select(SparseData.Parse).Select(s => s.Qid).Distinct();
        }

        public void RemovePosts(IEnumerable<int> postIds)
        {
        }

        public static void AddFeaturesToExisting(IEnumerable<PostData> featureSets,
                                                 string existingFeaturesFilepath, string updatedFeaturesFilepath,
                                                 bool queryDependent)
        {
            // note не предпоалагает передачу в списке параметров новых векторов со значениями, множество пар qid:postId в файле и параметре должны полностью совпадать, иначе вектор, не попадающий в пересечение не попадет в результирующий файл
            IEnumerable<string> lines = File.ReadLines(existingFeaturesFilepath);
            using (FileStream fileStream = File.Create(updatedFeaturesFilepath))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    foreach (string line in lines)
                    {
                        SparseData sparseData = SparseData.Parse(line);
                        int qid = sparseData.Qid;
                        int postId = sparseData.PostId;
                        IEnumerable<Feature> features = sparseData.Features.ToList();
                        IEnumerable<PostData> posts = featureSets;
                        if (queryDependent)
                            posts = featureSets.Where(f => f.RequestId == qid);
                        if (!posts.Any())
                            continue;
                        PostData postData = posts.FirstOrDefault(p => p.PostId == postId);
                        if (postData != null)
                        {
                            List<Feature> oldFeatures = postData.Features.ToList();
                            // todo при добавлении отслеживать, что признака с такми номером/именем нет в коллекции
                            oldFeatures.AddRange(features);
                            sparseData.Features = oldFeatures;
                        }
                        string sparseLine = sparseData.ToString();
                        streamWriter.WriteLine(sparseLine);
                    }
                }
            }
        }

        public static void RemoveFeaturesWithNumbers(IEnumerable<int> numbersToRemove, string existingFeaturesFilepath,
                                                     string resultFeaturesFilepath)
        {
            IEnumerable<string> lines = File.ReadLines(existingFeaturesFilepath);
            using (FileStream fileStream = File.Create(resultFeaturesFilepath))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    foreach (string line in lines)
                    {
                        SparseData sparseData = SparseData.Parse(line);
                        List<Feature> oldFeatures = sparseData.Features.ToList();
                        IEnumerable<Feature> featuresToRemove =
                            oldFeatures.Where(f => numbersToRemove.Contains(f.Number));
                        IEnumerable<Feature> newFeatures = oldFeatures.Where(f => !featuresToRemove.Contains(f));
                        sparseData.Features = newFeatures;
                        string sparseLine = sparseData.ToString();
                        streamWriter.WriteLine(sparseLine);
                    }
                }
            }
        }

        public static void RemoveEnties(IEnumerable<int> ids, string filepath, string resultFeaturesFilepath)
        {
            IEnumerable<string> lines = File.ReadLines(filepath);
            using (FileStream fileStream = File.Create(resultFeaturesFilepath))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    foreach (string line in lines)
                    {
                        SparseData sparseData = SparseData.Parse(line);
                        int postId = sparseData.PostId;
                        if (ids.Contains(postId))
                            continue;
                        string sparseLine = sparseData.ToString();
                        streamWriter.WriteLine(sparseLine);
                    }
                }
            }
        }

        #region Nested type: FeatureComparer

        private class FeatureComparer : IComparer<Feature>
        {
            #region IComparer<Feature> Members

            public int Compare(Feature x, Feature y)
            {
                int xn = x.Number;
                int yn = y.Number;
                if (xn > yn) return 1;
                if (xn < yn) return -1;
                return 0;
            }

            #endregion
        }

        #endregion

        #region Nested type: SparseData

        internal class SparseData
        {
            public int Qid { get; set; }
            public int Label { get; set; }
            public IEnumerable<Feature> Features { get; set; }
            public int PostId { get; set; }

            public override string ToString()
            {
                string line = string.Format("{0} qid:{1}", Label, Qid);
                IEnumerable<Feature> features = Features;
                line = features.Aggregate(line,
                                          (current, feature) =>
                                          string.Format("{0} {1}:{2}", current, feature.Number,
                                                        feature.Value.ToString(CultureInfo.InvariantCulture)));
                line = string.Format("{0} # {1}", line, PostId);
                return line;
            }

            public static SparseData Parse(string line)
            {
                var oldFeatures = new List<Feature>();
                string[] parts = line.Split(' ');
                int label = 0;
                int qid = 0;
                int postId = int.Parse(parts[parts.Length - 1]);
                foreach (string part in parts)
                {
                    if (part.Equals("#"))
                        break;
                    if (!part.Contains(':'))
                    {
                        label = int.Parse(part);
                        continue;
                    }
                    if (part.Contains("qid"))
                    {
                        qid = int.Parse(part.Split(':')[1]);
                        continue;
                    }

                    string[] feature = part.Split(':');
                    float value = Single.Parse(feature[1], CultureInfo.InvariantCulture);
                    int number = int.Parse(feature[0]);
                    string name = string.Empty;
                    oldFeatures.Add(new Feature {Name = name, Number = number, Value = value});
                }
                return new SparseData {Qid = qid, Label = label, Features = oldFeatures, PostId = postId};
            }
        }

        #endregion
    }
}