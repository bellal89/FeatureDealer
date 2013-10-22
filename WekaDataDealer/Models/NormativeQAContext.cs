using System.Data.Entity;
using FeatureDealer.Models.MappedClasses;
using FeatureDealer.Models.Mapping;

namespace FeatureDealer.Models
{
    public class NormativeQAContext : DbContext
    {
        static NormativeQAContext()
        {
            Database.SetInitializer<BuhonlineContext>(null);
        }

        public NormativeQAContext()
            : base("Name=NormativeQAContext")
        {
        }

        public DbSet<yaf_Message> yaf_Message { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new yaf_MessageMap());
        }
    }
}