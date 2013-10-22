using System;

namespace FeatureDealer.Models.MappedClasses
{
    public class Evaluation : IDataItem
    {
        public int EvaluationId { get; set; }
        public int RequestId { get; set; }
        public int PostId { get; set; }
        public int? Relevance { get; set; }
        public string Accessor { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public Post Post { get; set; }
        public Request Request { get; set; }

        #region IDataItem Members

        public int Id
        {
            get { return EvaluationId; }
        }

        #endregion
    }
}