using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FeatureDealer.FeatureCalculators;

namespace FeatureDealer
{
    public static class  Utilites
    {
        public static string StripOutBBCodeQuotes(this string s)
        {
            return FeatureRegex.BBQuoteCodes.Replace(s, string.Empty);
        }

        public static string StripBBCodeTags(this string s)
        {
            FeatureRegex.BBCodeTags.Replace(s, string.Empty);
            return Regex.Replace(s, @"\[[^\]]+?\]", String.Empty, RegexOptions.IgnoreCase);
        }

        public static IEnumerable<string> StripQuotesStripBBcodeSplitInWordsLower(this string s)
        {
            return s.StripOutBBCodeQuotes().StripBBCodeTags().SplitIntoWordsLower();
        }

        public static string StripHTMLTags(this String s)
        {
            return FeatureRegex.HtmlTags.Replace(s, string.Empty);
        }

        public static IEnumerable<string> SplitIntoWordsLower(this string s)
        {
            return FeatureRegex.Words.Split(s).Select(w => w.ToLower()).Where(w => w != string.Empty);
        }

        public static IEnumerable<string> SplitInWordsAndStripHTML(this string s)
        {
            return s.StripHTMLTags().SplitIntoWordsLower();
        }
        public static  IEnumerable<string> DistinctAndNotWhitespace(this IEnumerable<string> ss)
        {
            return ss.Distinct().Where(s => !String.IsNullOrWhiteSpace(s));
        }

        // todo что-то тут пошло не так...
        public static Regex AlternativeRegex(IEnumerable<string> words)
        {
            var altString = String.Join("|", words);
            return new Regex(altString);
        }

        // todo удалить эти преобразования в dictionary - там где они использутся, нужно сразу строить dictionary - непонятно зачем лишний раз гонять данные из одной структуры в другую
        public static Dictionary<TK,TV> ToDictionary<TK,TV>(this IEnumerable<KeyValuePair<TK,TV>> items)
        {
            return items.ToDictionary(kv => kv.Key, kv => kv.Value);
        }
        public static Dictionary<TK,TV> ToDictionary<TK,TV>(this IEnumerable<Tuple<TK,TV>> items)
        {
            return items.ToDictionary(i => i.Item1, i => i.Item2);
        }
    }
}
