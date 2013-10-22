using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.Models.Mapping
{
    public class yaf_TopicMap : EntityTypeConfiguration<yaf_Topic>
    {
        public yaf_TopicMap()
        {
            // Primary Key
            this.HasKey(t => new { t.TopicID, t.ForumID, t.UserID, t.Posted, t.Topic, t.Views, t.Priority, t.NumPosts, t.Flags, t.HasAnswers });

            // Properties
            this.Property(t => t.TopicID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.ForumID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.Topic)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Views)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Priority)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.LastUserName)
                .HasMaxLength(50);

            this.Property(t => t.NumPosts)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Flags)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("yaf_Topic");
            this.Property(t => t.TopicID).HasColumnName("TopicID");
            this.Property(t => t.ForumID).HasColumnName("ForumID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.CommonTagId).HasColumnName("CommonTagId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Posted).HasColumnName("Posted");
            this.Property(t => t.Topic).HasColumnName("Topic");
            this.Property(t => t.Views).HasColumnName("Views");
            this.Property(t => t.Priority).HasColumnName("Priority");
            this.Property(t => t.PollID).HasColumnName("PollID");
            this.Property(t => t.TopicMovedID).HasColumnName("TopicMovedID");
            this.Property(t => t.LastPosted).HasColumnName("LastPosted");
            this.Property(t => t.LastMessageID).HasColumnName("LastMessageID");
            this.Property(t => t.LastUserID).HasColumnName("LastUserID");
            this.Property(t => t.LastUserName).HasColumnName("LastUserName");
            this.Property(t => t.NumPosts).HasColumnName("NumPosts");
            this.Property(t => t.Flags).HasColumnName("Flags");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.HasAnswers).HasColumnName("HasAnswers");
        }
    }
}
