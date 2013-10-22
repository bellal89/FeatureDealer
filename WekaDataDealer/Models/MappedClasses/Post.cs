using System.Collections.Generic;

namespace FeatureDealer.Models.MappedClasses
{
    public class Post : IDataItem
    {
        public Post()
        {
            Evaluations = new List<Evaluation>();
            Posts1 = new List<Post>();
        }

        public int PostId { get; set; }
        public string Text { get; set; }
        public int? TopicStarter_PostId { get; set; }
        public int? Request_Id { get; set; }
        public ICollection<Evaluation> Evaluations { get; set; }
        public ICollection<Post> Posts1 { get; set; }
        public Post Post1 { get; set; }
        public Request Request { get; set; }

        #region IDataItem Members

        public int Id
        {
            get { return PostId; }
        }

        #endregion
    }
}