using System.Data.Entity.ModelConfiguration;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.Models.Mapping
{
    public class EvaluationMap : EntityTypeConfiguration<Evaluation>
    {
        public EvaluationMap()
        {
            // Primary Key
            this.HasKey(t => t.EvaluationId);

            // Properties
            // Table & Column Mappings
            this.ToTable("Evaluations");
            this.Property(t => t.EvaluationId).HasColumnName("EvaluationId");
            this.Property(t => t.RequestId).HasColumnName("RequestId");
            this.Property(t => t.PostId).HasColumnName("PostId");
            this.Property(t => t.Relevance).HasColumnName("Relevance");
            this.Property(t => t.Accessor).HasColumnName("Accessor");
            this.Property(t => t.Start).HasColumnName("Start");
            this.Property(t => t.End).HasColumnName("End");

            // Relationships
            this.HasRequired(t => t.Post)
                .WithMany(t => t.Evaluations)
                .HasForeignKey(d => d.PostId);
        }
    }
}
