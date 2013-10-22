namespace FeatureDealer.FeatureCalculators
{
    public class QueryDependentFeatureParameters: IFeatureParameters
    {
        public int MessageId { get; set; }
        public string Query { get; set; }
    }
}