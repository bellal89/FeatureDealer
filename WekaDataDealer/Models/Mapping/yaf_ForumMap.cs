using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.Models.Mapping
{
    public class yaf_ForumMap : EntityTypeConfiguration<yaf_Forum>
    {
        public yaf_ForumMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ForumID, t.CategoryID, t.Name, t.Description, t.SortOrder, t.NumTopics, t.NumPosts, t.Flags });

            // Properties
            this.Property(t => t.ForumID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CategoryID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.SortOrder)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.LastUserName)
                .HasMaxLength(50);

            this.Property(t => t.NumTopics)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NumPosts)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RemoteURL)
                .HasMaxLength(100);

            this.Property(t => t.Flags)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ThemeURL)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("yaf_Forum");
            this.Property(t => t.ForumID).HasColumnName("ForumID");
            this.Property(t => t.CategoryID).HasColumnName("CategoryID");
            this.Property(t => t.ParentID).HasColumnName("ParentID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
            this.Property(t => t.LastPosted).HasColumnName("LastPosted");
            this.Property(t => t.LastTopicID).HasColumnName("LastTopicID");
            this.Property(t => t.LastMessageID).HasColumnName("LastMessageID");
            this.Property(t => t.LastUserID).HasColumnName("LastUserID");
            this.Property(t => t.LastUserName).HasColumnName("LastUserName");
            this.Property(t => t.NumTopics).HasColumnName("NumTopics");
            this.Property(t => t.NumPosts).HasColumnName("NumPosts");
            this.Property(t => t.RemoteURL).HasColumnName("RemoteURL");
            this.Property(t => t.Flags).HasColumnName("Flags");
            this.Property(t => t.ThemeURL).HasColumnName("ThemeURL");
        }
    }
}
