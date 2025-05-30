using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JPOC_VM_DEMO.Services.JpocData.T_JP_Disease.Model
{
    [Serializable]
    [Table("T_JP_Disease")]
    public class Disease
    {
        #region Properties

        [Key]
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Synonyms { get; set; }
        public int Importance { get; set; }
        public int Frequency { get; set; }
        public int Urgency { get; set; }
        public double Sequence { get; set; }
        public string Information { get; set; }
        public string Status { get; set; }
        public bool IsCurrentVersion { get; set; }
        public int Version { get; set; }
        public string VersionString { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public int CheckoutBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool Defunct { get; set; }
        public string ExternalLink { get; set; }
        public string AuthorName { get; set; }
        public string AuthorInstitution { get; set; }
        public string CoauthorName { get; set; }
        public string CoauthorInstitution { get; set; }
        public string ExternalLink2 { get; set; }
        public string ExternalLink3 { get; set; }
        public int SubcategoryId { get; set; }
        public int SortId { get; set; }
        public bool ProhibitedImport { get; set; }
        public string LatestEdittingDate { get; set; }
        public int WideTitle { get; set; }
        public string AuthorIntroducation { get; set; }
        public string InactiveMessage { get; set; }
        public bool PublishProd { get; set; }
        public string HistoryText { get; set; }
        public bool IsWip { get; set; }
        public string IntroductionAuthor { get; set; }
        public string IntroductionAuthorInstitution { get; set; }
        public string IntroductionExpertise { get; set; }
        public string IntroductionSpecialist { get; set; }
        public string IntroductionAcademy { get; set; }
        public string IntroductionResume { get; set; }
        public string IntroductionAdvice { get; set; }
        public string IntroductionExternal { get; set; }
        public string IntroductionAuthorMessage { get; set; }
        public string IntroductionAuthorPhoto { get; set; }
        public string MedicalSafety { get; set; }

        #endregion

        #region Constructors

        public Disease()
        {
        }

        public Disease(int id, Guid guid, string title, string type, string synonyms,
            int importance, int frequency, int urgency, double sequence, string information,
            string status, bool isCurrentVersion, int version, string versionString,
            int createdBy, int modifiedBy, int checkoutBy, DateTime createdDate,
            DateTime modifiedDate, bool defunct, string externalLink, string authorName,
            string authorInstitution, string coauthorName, string coauthorInstitution,
            string externalLink2, string externalLink3, int subcategoryId, int sortId,
            bool prohibitedImport, string latestEdittingDate, int wideTitle,
            string authorIntroducation, string inactiveMessage, bool publishProd,
            string historyText, bool isWip, string introductionAuthor,
            string introductionAuthorInstitution, string introductionExpertise,
            string introductionSpecialist, string introductionAcademy,
            string introductionResume, string introductionAdvice,
            string introductionExternal, string introductionAuthorMessage,
            string introductionAuthorPhoto, string medicalSafety)
        {
            Id = id;
            Guid = guid;
            Title = title;
            Type = type;
            Synonyms = synonyms;
            Importance = importance;
            Frequency = frequency;
            Urgency = urgency;
            Sequence = sequence;
            Information = information;
            Status = status;
            IsCurrentVersion = isCurrentVersion;
            Version = version;
            VersionString = versionString;
            CreatedBy = createdBy;
            ModifiedBy = modifiedBy;
            CheckoutBy = checkoutBy;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
            Defunct = defunct;
            ExternalLink = externalLink;
            AuthorName = authorName;
            AuthorInstitution = authorInstitution;
            CoauthorName = coauthorName;
            CoauthorInstitution = coauthorInstitution;
            ExternalLink2 = externalLink2;
            ExternalLink3 = externalLink3;
            SubcategoryId = subcategoryId;
            SortId = sortId;
            ProhibitedImport = prohibitedImport;
            LatestEdittingDate = latestEdittingDate;
            WideTitle = wideTitle;
            AuthorIntroducation = authorIntroducation;
            InactiveMessage = inactiveMessage;
            PublishProd = publishProd;
            HistoryText = historyText;
            IsWip = isWip;
            IntroductionAuthor = introductionAuthor;
            IntroductionAuthorInstitution = introductionAuthorInstitution;
            IntroductionExpertise = introductionExpertise;
            IntroductionSpecialist = introductionSpecialist;
            IntroductionAcademy = introductionAcademy;
            IntroductionResume = introductionResume;
            IntroductionAdvice = introductionAdvice;
            IntroductionExternal = introductionExternal;
            IntroductionAuthorMessage = introductionAuthorMessage;
            IntroductionAuthorPhoto = introductionAuthorPhoto;
            MedicalSafety = medicalSafety;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns the Primary Key of the object.
        /// </summary>
        public override string ToString()
        {
            return $"[T_JP_Disease] {Id}";
        }

        /// <summary>
        /// Returns true if the Ids of the two instances are equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Disease)obj;
            return Id == other.Id;
        }

        /// <summary>
        /// Returns the GetHashCode() method of the Primary Key member.
        /// </summary>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #endregion
    }
}
