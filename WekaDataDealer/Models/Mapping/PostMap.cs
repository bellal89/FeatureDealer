using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.Models.Mapping
{
    public class PostMap : EntityTypeConfiguration<Post>
    {
        public PostMap()
        {
            // Primary Key
            this.HasKey(t => t.PostId);

            // Properties
            this.Property(t => t.PostId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Posts");
            this.Property(t => t.PostId).HasColumnName("PostId");
            this.Property(t => t.Text).HasColumnName("Text");
            this.Property(t => t.TopicStarter_PostId).HasColumnName("TopicStarter_PostId");
            this.Property(t => t.Request_Id).HasColumnName("Request_Id");

            // Relationships
            this.HasOptional(t => t.Post1)
                .WithMany(t => t.Posts1)
                .HasForeignKey(d => d.TopicStarter_PostId);
        }
    }
}
