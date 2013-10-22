namespace FeatureDealer.FeatureCalculators
{
    public class Feature
    {
        public double Value { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var feature = obj as Feature;
            if (feature == null) return false;
            return Value == feature.Value && Number == feature.Number && Name == feature.Name &&
                   Description == feature.Description;
        }
    }
}