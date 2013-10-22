namespace FeatureDealer.Models.MappedClasses
{
    public class Request: IDataItem
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string TextClarification { get; set; }
    }
}