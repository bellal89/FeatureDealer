using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FeatureDealer
{
    internal static class FileProcessing
    {
        private const string dir = "Dictionaries";
        private static readonly string closeThankWordsFile = Path.Combine(dir, "closeThankWords2.txt"); // слова благодарности нагерерированные по левенштейну от 'спасибо' и 'спс'
        private static readonly string thankPosts2000File = Path.Combine(dir, "postsUnityDistrib.txt"); // оцененные посты (0/1 - пост благодарности/не благодарности)
        private static readonly string gratitudeAppliedFile = Path.Combine(dir, "labeledGratitudes.txt"); // файл с результатом

        public static IEnumerable<string> CloseWordsToThank = GetWordsFromFile(closeThankWordsFile);
        public static readonly IEnumerable<KeyValuePair<int, int>> ThankPostWithScores = ParseThankWordsFile(thankPosts2000File);
        public static readonly IEnumerable<KeyValuePair<int, int>> MessageIdAndIsThankPost = GetGratitudeResultsFromCSV(gratitudeAppliedFile);

        private static IEnumerable<string> GetWordsFromFile(string fileName)
        {
            var lines = File.ReadAllLines(fileName);
            return lines.Distinct().Where(s => !String.IsNullOrWhiteSpace(s));
        }
        
        private static IEnumerable<KeyValuePair<int, int>> ParseThankWordsFile(string fileName)
        {
            var lines = File.ReadLines(fileName, Encoding.GetEncoding(1251));
            foreach (var line in lines)
            {
                var items = line.Split('\t');
                var id = int.Parse(items[0]);
                var isThankPost = int.Parse(items[2]);
                yield return new KeyValuePair<int, int>(id, isThankPost);
            }
        }
        private static IEnumerable<KeyValuePair<int, int>> GetGratitudeResultsFromCSV(string filename)
        {
            var lines = File.ReadLines(filename);
            foreach (var line in lines.Skip(1))
            {
                var lineWithoutQuotes = line.Replace("\"", "");
                var items = lineWithoutQuotes.Split(';');
                var messageId = int.Parse(items[0]);
                var prediction = int.Parse(items[3]);
                yield return new KeyValuePair<int, int>(messageId, prediction);
            }
        }
    }
}
