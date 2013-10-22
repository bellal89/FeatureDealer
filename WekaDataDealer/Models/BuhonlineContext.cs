using System.Data.Entity;
using FeatureDealer.Models.MappedClasses;
using FeatureDealer.Models.Mapping;

namespace FeatureDealer.Models
{
    public class BuhonlineContext : DbContext
    {
        static BuhonlineContext()
        {
            Database.SetInitializer<BuhonlineContext>(null);
        }

        public BuhonlineContext()
            : base("Name=BuhonlineContext")
        {
        }

        public DbSet<UserPersonal> UserPersonals { get; set; }
        public DbSet<yaf_Forum> yaf_Forum { get; set; }
        public DbSet<yaf_Message> yaf_Message { get; set; }
        public DbSet<yaf_Topic> yaf_Topic { get; set; }
        public DbSet<yaf_User> yaf_User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserPersonalMap());
            modelBuilder.Configurations.Add(new yaf_ForumMap());
            modelBuilder.Configurations.Add(new yaf_MessageMap());
            modelBuilder.Configurations.Add(new yaf_TopicMap());
            modelBuilder.Configurations.Add(new yaf_UserMap());
        }
    }
}