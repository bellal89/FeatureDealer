using System.Data.Entity;
using FeatureDealer.Models.MappedClasses;
using FeatureDealer.Models.Mapping;

namespace FeatureDealer.Models
{
    public class EvaluationContext : DbContext
    {
        static EvaluationContext()
        {
            Database.SetInitializer<EvaluationContext>(null);
        }

        public EvaluationContext()
            : base("Name=EvaluationDBContext")
        {
        }

        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Request> Requests { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new EvaluationMap());
            modelBuilder.Configurations.Add(new PostMap());
            modelBuilder.Configurations.Add(new RequestMap());
        }
    }
}