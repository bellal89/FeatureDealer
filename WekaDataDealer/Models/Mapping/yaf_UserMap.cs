using System.Data.Entity.ModelConfiguration;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.Models.Mapping
{
    public class yaf_UserMap : EntityTypeConfiguration<yaf_User>
    {
        public yaf_UserMap()
        {
            // Primary Key
            this.HasKey(t => t.UserID);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.Email)
                .HasMaxLength(50);

            this.Property(t => t.IP)
                .HasMaxLength(15);

            this.Property(t => t.Location)
                .HasMaxLength(50);

            this.Property(t => t.HomePage)
                .HasMaxLength(50);

            this.Property(t => t.Avatar)
                .HasMaxLength(255);

            this.Property(t => t.LanguageFile)
                .HasMaxLength(50);

            this.Property(t => t.ThemeFile)
                .HasMaxLength(50);

            this.Property(t => t.MSN)
                .HasMaxLength(50);

            this.Property(t => t.YIM)
                .HasMaxLength(30);

            this.Property(t => t.AIM)
                .HasMaxLength(30);

            this.Property(t => t.RealName)
                .HasMaxLength(50);

            this.Property(t => t.Occupation)
                .HasMaxLength(50);

            this.Property(t => t.Interests)
                .HasMaxLength(100);

            this.Property(t => t.Weblog)
                .HasMaxLength(100);

            this.Property(t => t.ValidationHash)
                .HasMaxLength(50);

            this.Property(t => t.LoweredName)
                .HasMaxLength(50);

            this.Property(t => t.Avatar2)
                .HasMaxLength(255);

            this.Property(t => t.Avatar3)
                .HasMaxLength(255);

            this.Property(t => t.ProfileCompletion)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("yaf_User");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.BoardID).HasColumnName("BoardID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Joined).HasColumnName("Joined");
            this.Property(t => t.LastVisit).HasColumnName("LastVisit");
            this.Property(t => t.IP).HasColumnName("IP");
            this.Property(t => t.NumPosts).HasColumnName("NumPosts");
            this.Property(t => t.Location).HasColumnName("Location");
            this.Property(t => t.HomePage).HasColumnName("HomePage");
            this.Property(t => t.TimeZone).HasColumnName("TimeZone");
            this.Property(t => t.Avatar).HasColumnName("Avatar");
            this.Property(t => t.Signature).HasColumnName("Signature");
            this.Property(t => t.AvatarImage).HasColumnName("AvatarImage");
            this.Property(t => t.RankID).HasColumnName("RankID");
            this.Property(t => t.Suspended).HasColumnName("Suspended");
            this.Property(t => t.LanguageFile).HasColumnName("LanguageFile");
            this.Property(t => t.ThemeFile).HasColumnName("ThemeFile");
            this.Property(t => t.MSN).HasColumnName("MSN");
            this.Property(t => t.YIM).HasColumnName("YIM");
            this.Property(t => t.AIM).HasColumnName("AIM");
            this.Property(t => t.ICQ).HasColumnName("ICQ");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.Occupation).HasColumnName("Occupation");
            this.Property(t => t.Interests).HasColumnName("Interests");
            this.Property(t => t.Gender).HasColumnName("Gender");
            this.Property(t => t.Weblog).HasColumnName("Weblog");
            this.Property(t => t.PMNotification).HasColumnName("PMNotification");
            this.Property(t => t.Flags).HasColumnName("Flags");
            this.Property(t => t.IsApproved).HasColumnName("IsApproved");
            this.Property(t => t.Points).HasColumnName("Points");
            this.Property(t => t.OldUserId).HasColumnName("OldUserId");
            this.Property(t => t.ValidationHash).HasColumnName("ValidationHash");
            this.Property(t => t.LoweredName).HasColumnName("LoweredName");
            this.Property(t => t.CmsModuleId).HasColumnName("CmsModuleId");
            this.Property(t => t.IsOperator).HasColumnName("IsOperator");
            this.Property(t => t.Avatar2).HasColumnName("Avatar2");
            this.Property(t => t.Avatar3).HasColumnName("Avatar3");
            this.Property(t => t.IsLookingForAJob).HasColumnName("IsLookingForAJob");
            this.Property(t => t.ProfileCompletion).HasColumnName("ProfileCompletion");
            this.Property(t => t.ReceiveNotificationOfPersonalMessages).HasColumnName("ReceiveNotificationOfPersonalMessages");
            this.Property(t => t.ReceiveNotificationOfInvitationToColleagues).HasColumnName("ReceiveNotificationOfInvitationToColleagues");
            this.Property(t => t.ReceiveNotificationOfJobOffers).HasColumnName("ReceiveNotificationOfJobOffers");
            this.Property(t => t.ReceiveNotificationOfProposalResume).HasColumnName("ReceiveNotificationOfProposalResume");
            this.Property(t => t.ReceiveMessagesOnlyFromColleagues).HasColumnName("ReceiveMessagesOnlyFromColleagues");
            this.Property(t => t.IsOpenForThanksDisabled).HasColumnName("IsOpenForThanksDisabled");
        }
    }
}
