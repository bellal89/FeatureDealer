using System.Data.Entity.ModelConfiguration;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.Models.Mapping
{
    public class RequestMap : EntityTypeConfiguration<Request>
    {
        public RequestMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Requests");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Text).HasColumnName("Text");
            this.Property(t => t.TextClarification).HasColumnName("TextClarification");
        }
    }
}
