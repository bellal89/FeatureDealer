using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.Models.Mapping
{
    public class yaf_MessageMap : EntityTypeConfiguration<yaf_Message>
    {
        public yaf_MessageMap()
        {
            // Primary Key
            HasKey(
                t =>
                new
                    {
                        t.MessageID,
                        t.TopicID,
                        t.Position,
                        t.Indent,
                        t.UserID,
                        t.Posted,
                        t.Message,
                        t.IP,
                        t.Flags,
                        t.OpenForThanks,
                        t.Points
                    });

            // Properties
            Property(t => t.MessageID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.TopicID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Position)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Indent)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.UserID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.UserName)
                .HasMaxLength(50);

            Property(t => t.Message)
                .IsRequired();

            Property(t => t.IP)
                .IsRequired()
                .HasMaxLength(15);

            Property(t => t.Flags)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Points)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            ToTable("yaf_Message");
            Property(t => t.MessageID).HasColumnName("MessageID");
            Property(t => t.TopicID).HasColumnName("TopicID");
            Property(t => t.ReplyTo).HasColumnName("ReplyTo");
            Property(t => t.Position).HasColumnName("Position");
            Property(t => t.Indent).HasColumnName("Indent");
            Property(t => t.UserID).HasColumnName("UserID");
            Property(t => t.UserName).HasColumnName("UserName");
            Property(t => t.Posted).HasColumnName("Posted");
            Property(t => t.Message).HasColumnName("Message");
            Property(t => t.IP).HasColumnName("IP");
            Property(t => t.Edited).HasColumnName("Edited");
            Property(t => t.Flags).HasColumnName("Flags");
            Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            Property(t => t.IsApproved).HasColumnName("IsApproved");
            Property(t => t.OpenForThanks).HasColumnName("OpenForThanks");
            Property(t => t.Points).HasColumnName("Points");
        }
    }
}