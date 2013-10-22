using System;

namespace FeatureDealer.Models.MappedClasses
{
    public class yaf_User : IDataItem
    {
        public int UserID { get; set; }
        public int BoardID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime Joined { get; set; }
        public DateTime LastVisit { get; set; }
        public string IP { get; set; }
        public int NumPosts { get; set; }
        public string Location { get; set; }
        public string HomePage { get; set; }
        public int TimeZone { get; set; }
        public string Avatar { get; set; }
        public string Signature { get; set; }
        public byte[] AvatarImage { get; set; }
        public int RankID { get; set; }
        public DateTime? Suspended { get; set; }
        public string LanguageFile { get; set; }
        public string ThemeFile { get; set; }
        public string MSN { get; set; }
        public string YIM { get; set; }
        public string AIM { get; set; }
        public int? ICQ { get; set; }
        public string RealName { get; set; }
        public string Occupation { get; set; }
        public string Interests { get; set; }
        public byte Gender { get; set; }
        public string Weblog { get; set; }
        public bool PMNotification { get; set; }
        public int Flags { get; set; }
        public bool? IsApproved { get; set; }
        public int Points { get; set; }
        public int? OldUserId { get; set; }
        public string ValidationHash { get; set; }
        public string LoweredName { get; set; }
        public int? CmsModuleId { get; set; }
        public bool IsOperator { get; set; }
        public string Avatar2 { get; set; }
        public string Avatar3 { get; set; }
        public bool IsLookingForAJob { get; set; }
        public string ProfileCompletion { get; set; }
        public bool ReceiveNotificationOfPersonalMessages { get; set; }
        public bool ReceiveNotificationOfInvitationToColleagues { get; set; }
        public bool ReceiveNotificationOfJobOffers { get; set; }
        public bool ReceiveNotificationOfProposalResume { get; set; }
        public bool ReceiveMessagesOnlyFromColleagues { get; set; }
        public bool IsOpenForThanksDisabled { get; set; }

        #region IDataItem Members

        public int Id
        {
            get { return UserID; }
        }

        #endregion
    }
}