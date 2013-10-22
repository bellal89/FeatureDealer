namespace FeatureDealer.RuLemmatizer
{
    public class LemmaInfo
    {
        public LemmaInfo(string initialWord, string lemma, string partOfSpeech)
        {
            InitialWord = initialWord;
            Lemma = lemma;
            PartOfSpeech = partOfSpeech;
        }

        public string InitialWord { get; set; }

        public string Lemma { get; set; }

        public string PartOfSpeech { get; set; }
    }
}