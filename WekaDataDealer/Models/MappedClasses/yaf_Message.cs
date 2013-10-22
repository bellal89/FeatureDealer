using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FeatureDealer.Models.MappedClasses
{
    public class yaf_Message : IDataItem
    {
        public int MessageID { get; set; }
        public int TopicID { get; set; }
        public int? ReplyTo { get; set; }
        public int Position { get; set; }
        public int Indent { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public DateTime Posted { get; set; }
        public string Message { get; set; }
        public string IP { get; set; }
        public DateTime? Edited { get; set; }
        public int Flags { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsApproved { get; set; }
        public bool OpenForThanks { get; set; }
        public int Points { get; set; }

        [NotMapped]
        public IsQuestion IsQuestion { get; set; }

        #region IDataItem Members

        public int Id
        {
            get { return MessageID; }
        }

        #endregion

        public override string ToString()
        {
            return String.Join("\t",
                               new[]
                                   {
                                       MessageID.ToString(CultureInfo.InvariantCulture),
                                       Position.ToString(CultureInfo.InvariantCulture),
                                       ReplyTo.ToString(),
                                       TopicID.ToString(CultureInfo.InvariantCulture),
                                       UserID.ToString(CultureInfo.InvariantCulture),
                                       Message
                                   }) + (IsQuestion == IsQuestion.Unknown ? "" : "\t" + (int) IsQuestion);
        }

        public static yaf_Message CreateFromLine(string line)
        {
            string[] input = line.Split('\t');
            return new yaf_Message
                       {
                           MessageID = int.Parse(input[0]),
                           Position = int.Parse(input[1]),
                           ReplyTo = int.Parse(input[2]),
                           TopicID = int.Parse(input[3]),
                           UserID = int.Parse(input[4]),
                           Message = input[5],
                           IsQuestion = input.Length > 6 ? (IsQuestion) int.Parse(input[6]) : IsQuestion.Unknown
                       };
        }
    }

    public enum IsQuestion
    {
        No,
        Yes,
        Unknown
    }
}