using System.Data.Entity;
using FeatureDealer.Models.MappedClasses;
using FeatureDealer.Models.Mapping;

namespace FeatureDealer.Models
{
    public class FeaturesContext : DbContext
    {
        static FeaturesContext()
        {
            Database.SetInitializer<FeaturesContext>(null);
        }

        public FeaturesContext()
            : base("Name=FeaturesContext")
        {
        }

        public DbSet<PostFeature> PostFeatures { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new PostFeatureMap());
        }
    }
}