using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FeatureDealer.Models.MappedClasses;

namespace FeatureDealer.Models.Mapping
{
    public class UserPersonalMap : EntityTypeConfiguration<UserPersonal>
    {
        public UserPersonalMap()
        {
            // Primary Key
            this.HasKey(t => t.UserPersonalId);

            // Properties
            this.Property(t => t.UserPersonalId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Profession)
                .HasMaxLength(30);

            this.Property(t => t.SecondName)
                .HasMaxLength(50);

            this.Property(t => t.FirstName)
                .HasMaxLength(50);

            this.Property(t => t.Patronymic)
                .HasMaxLength(50);

            this.Property(t => t.Birthday)
                .HasMaxLength(10);

            this.Property(t => t.Email)
                .HasMaxLength(50);

            this.Property(t => t.PhoneHome)
                .HasMaxLength(50);

            this.Property(t => t.PhoneMobile)
                .HasMaxLength(50);

            this.Property(t => t.PhoneOffice)
                .HasMaxLength(50);

            this.Property(t => t.ICQ)
                .HasMaxLength(50);

            this.Property(t => t.Skype)
                .HasMaxLength(50);

            this.Property(t => t.ProfileVKontakte)
                .HasMaxLength(255);

            this.Property(t => t.ProfileMoiKrug)
                .HasMaxLength(255);

            this.Property(t => t.Competence)
                .HasMaxLength(4000);

            this.Property(t => t.UserTypeText)
                .HasMaxLength(50);

            this.Property(t => t.Acl)
                .HasMaxLength(4000);

            // Table & Column Mappings
            this.ToTable("UserPersonal");
            this.Property(t => t.UserPersonalId).HasColumnName("UserPersonalId");
            this.Property(t => t.BuhSpecializationId).HasColumnName("BuhSpecializationId");
            this.Property(t => t.OrganizationTypeId).HasColumnName("OrganizationTypeId");
            this.Property(t => t.TaxingTypeId).HasColumnName("TaxingTypeId");
            this.Property(t => t.TransferTypeId).HasColumnName("TransferTypeId");
            this.Property(t => t.BankId).HasColumnName("BankId");
            this.Property(t => t.InspectionId).HasColumnName("InspectionId");
            this.Property(t => t.InspectorJobPositionId).HasColumnName("InspectorJobPositionId");
            this.Property(t => t.InspectionTypeId).HasColumnName("InspectionTypeId");
            this.Property(t => t.PfrId).HasColumnName("PfrId");
            this.Property(t => t.FssId).HasColumnName("FssId");
            this.Property(t => t.FsgsId).HasColumnName("FsgsId");
            this.Property(t => t.Standing).HasColumnName("Standing");
            this.Property(t => t.UserType).HasColumnName("UserType");
            this.Property(t => t.Profession).HasColumnName("Profession");
            this.Property(t => t.SecondName).HasColumnName("SecondName");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.Patronymic).HasColumnName("Patronymic");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.GMCityId).HasColumnName("GMCityId");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.PhoneHome).HasColumnName("PhoneHome");
            this.Property(t => t.PhoneMobile).HasColumnName("PhoneMobile");
            this.Property(t => t.PhoneOffice).HasColumnName("PhoneOffice");
            this.Property(t => t.ICQ).HasColumnName("ICQ");
            this.Property(t => t.Skype).HasColumnName("Skype");
            this.Property(t => t.ProfileVKontakte).HasColumnName("ProfileVKontakte");
            this.Property(t => t.ProfileMoiKrug).HasColumnName("ProfileMoiKrug");
            this.Property(t => t.Competence).HasColumnName("Competence");
            this.Property(t => t.UserTypeText).HasColumnName("UserTypeText");
            this.Property(t => t.Acl).HasColumnName("Acl");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
        }
    }
}
