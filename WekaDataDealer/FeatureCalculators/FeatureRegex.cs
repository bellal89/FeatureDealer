using System.Text.RegularExpressions;

namespace FeatureDealer.FeatureCalculators
{
    static class FeatureRegex
    {
        private const RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase;
        public static readonly Regex Date = new Regex(@"(0[1-9]|[12][0-9]|3[01])[- /.]((0[1-9]|1[012])|января|февраля|марта|апреля|мая|июня|июля|августа|сентября|октября|ноября|декабря)[- /.](19|20)\d\d", regexOptions);
        public static readonly Regex QuotedWords = new Regex("[«\"'`]([^\\s]+(\\s+[^\\s\"'`]+)*)[\"'`»]", regexOptions);
        public static readonly Regex QuoteExpression = new Regex(@"\[quote(?:=[^\]]+)?\](.+)\[/quote\]", regexOptions);
        public static readonly Regex MessageNum = new Regex("buhonline.ru.*?[?&]m=(\\d+)", regexOptions);
        public static readonly Regex TopicNum = new Regex("buhonline.ru.*?[?&]t=(\\d+)", regexOptions);
        public static readonly Regex Formula = new Regex(@"^[^\[&]*\d+\s*=\s*\d+", regexOptions);
        public static readonly Regex HighlightBBTag = new Regex("\\[(b|i\\]|u\\]|s\\]|color|colour|size)", regexOptions);
        public static readonly Regex QuoteTag = new Regex("\\[quote|\\[/quote", regexOptions);
        public static readonly Regex Url = new Regex("https?://.+/.*", regexOptions);
        public static readonly Regex QuoteMarks = new Regex("[\"'`]", regexOptions);
        public static readonly Regex Numbers = new Regex("\\d+", regexOptions);
        public static readonly Regex Tokens = new Regex("\\s+", regexOptions);
        public static readonly Regex Words = new Regex("\\W+", regexOptions);

        public static readonly Regex BBQuoteCodes = new Regex(@"\[quote(?:=.+?)?\].+\[/quote\]", regexOptions);
        public static readonly Regex BBCodeTags = new Regex(@"\[[^\]]+?\]", regexOptions);
        public static readonly Regex HtmlTags = new Regex(@"<[^>]*?>", regexOptions);

        public static Regex TaggedWords(string tagName)
        {
            return new Regex(string.Format(@"\[{0}(?:=[^\]]+)?\](.*?)\[/{0}\]", tagName), regexOptions);
        }
    }
}
