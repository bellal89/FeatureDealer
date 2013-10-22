using System;

namespace FeatureDealer.Models.MappedClasses
{
    public partial class yaf_Forum
    {
        public int ForumID { get; set; }
        public int CategoryID { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public short SortOrder { get; set; }
        public Nullable<System.DateTime> LastPosted { get; set; }
        public Nullable<int> LastTopicID { get; set; }
        public Nullable<int> LastMessageID { get; set; }
        public Nullable<int> LastUserID { get; set; }
        public string LastUserName { get; set; }
        public int NumTopics { get; set; }
        public int NumPosts { get; set; }
        public string RemoteURL { get; set; }
        public int Flags { get; set; }
        public string ThemeURL { get; set; }
    }
}
