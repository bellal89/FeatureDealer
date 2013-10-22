using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.Models.Mapping
{
    public class PostFeatureMap : EntityTypeConfiguration<PostFeature>
    {
        public PostFeatureMap()
        {
            // Primary Key
            this.HasKey(t => t.MessageId);

            // Properties
            this.Property(t => t.MessageId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("PostFeatures");
            this.Property(t => t.MessageId).HasColumnName("MessageId");
            this.Property(t => t.isThanks).HasColumnName("isThanks");
        }
    }
}
