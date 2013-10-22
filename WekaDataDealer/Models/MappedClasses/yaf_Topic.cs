using System;

namespace FeatureDealer.Models.MappedClasses
{
    public class yaf_Topic: IDataItem
    {
        public int TopicID { get; set; }
        public int ForumID { get; set; }
        public int UserID { get; set; }
        public int? CommonTagId { get; set; }
        public string UserName { get; set; }
        public DateTime Posted { get; set; }
        public string Topic { get; set; }
        public int Views { get; set; }
        public short Priority { get; set; }
        public int? PollID { get; set; }
        public int? TopicMovedID { get; set; }
        public DateTime? LastPosted { get; set; }
        public int? LastMessageID { get; set; }
        public int? LastUserID { get; set; }
        public string LastUserName { get; set; }
        public int NumPosts { get; set; }
        public int Flags { get; set; }
        public bool? IsDeleted { get; set; }
        public bool HasAnswers { get; set; }

        public int Id
        {
            get { return TopicID; }
        }
    }
}