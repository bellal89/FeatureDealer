using System;

namespace FeatureDealer.Models.MappedClasses
{
    public class UserPersonal : IDataItem
    {
        public int UserPersonalId { get; set; }
        public int? BuhSpecializationId { get; set; }
        public int? OrganizationTypeId { get; set; }
        public int? TaxingTypeId { get; set; }
        public int? TransferTypeId { get; set; }
        public int? BankId { get; set; }
        public int? InspectionId { get; set; }
        public int? InspectorJobPositionId { get; set; }
        public int? InspectionTypeId { get; set; }
        public int? PfrId { get; set; }
        public int? FssId { get; set; }
        public int? FsgsId { get; set; }
        public byte? Standing { get; set; }
        public byte? UserType { get; set; }
        public string Profession { get; set; }
        public string SecondName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string Birthday { get; set; }
        public int? GMCityId { get; set; }
        public string Email { get; set; }
        public string PhoneHome { get; set; }
        public string PhoneMobile { get; set; }
        public string PhoneOffice { get; set; }
        public string ICQ { get; set; }
        public string Skype { get; set; }
        public string ProfileVKontakte { get; set; }
        public string ProfileMoiKrug { get; set; }
        public string Competence { get; set; }
        public string UserTypeText { get; set; }
        public string Acl { get; set; }
        public DateTime ModifiedDate { get; set; }

        #region IDataItem Members

        public int Id
        {
            get { return UserPersonalId; }
        }

        #endregion
    }
}