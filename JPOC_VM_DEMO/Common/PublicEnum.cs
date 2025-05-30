using System.ComponentModel;
namespace JPOC_VM_DEMO.Common
{

    /// <summary>
    /// Contains all public enumerations for the JPOC system
    /// </summary>
    public static class PublicEnum
    {
        public enum eApplicationType
        {
            Undefined,
            WindowForm,
            WebApplication,
            WebService
        }

        /// <summary>
        /// User type enumeration
        /// </summary>
        public enum eUserType
        {
            /// <summary>
            /// Undefined
            /// </summary>
            Undefined = 0,

            /// <summary>
            /// Internal user (Elsevier)
            /// </summary>
            INT = 1,

            /// <summary>
            /// Individual user
            /// </summary>
            IND = 2,

            /// <summary>
            /// Institution user
            /// </summary>
            INS = 3
        }

        /// <summary>
        /// Runtime environment
        /// </summary>
        /// <remarks>
        /// ENV_[XXXX] part of AppSettings
        /// </remarks>
        public enum eRuntimeEnvironment
        {
            /// <summary>
            /// LOCAL (Development) environment
            /// </summary>
            LOCAL,

            /// <summary>
            /// IT environment
            /// </summary>
            IT,

            /// <summary>
            /// DEMO environment
            /// </summary>
            DEMO,

            /// <summary>
            /// Preview environment
            /// </summary>
            PREV,

            /// <summary>
            /// Production environment
            /// </summary>
            PROD,

            /// <summary>
            /// VM environment
            /// </summary>
            VM,

            /// <summary>
            /// Development environment
            /// </summary>
            DEV,

            /// <summary>
            /// Pre-production environment
            /// </summary>
            PRE,

            /// <summary>
            /// Test environment
            /// </summary>
            TEST,

            /// <summary>
            /// Foundation UAT environment
            /// </summary>
            FOUND_UAT,

            /// <summary>
            /// Foundation local environment
            /// </summary>
            FOUND_LOCAL,

            /// <summary>
            /// Foundation production environment
            /// </summary>
            FOUND_PROD,

            /// <summary>
            /// E-Library environment
            /// </summary>
            ELIB,

            /// <summary>
            /// E-Nurse environment
            /// </summary>
            ENURSE
        }

        public enum eRtnCD
        {
            /// <summary>
            /// Operation completed successfully
            /// </summary>
            Normal = 0,

            /// <summary>
            /// Business logic error occurred
            /// </summary>
            LogicalError = 1,

            /// <summary>
            /// Critical system error occurred
            /// </summary>
            FatalError = 9  // Fixed typo from 'Faital' to 'Fatal'
        }

        /// <summary>
        /// Page access rights
        /// </summary>
        /// <remarks>
        /// None = No access rights
        /// ReferenceOnly = Read-only
        /// Editable = Read/Write enabled
        /// </remarks>
        public enum AccessRight
        {
            /// <summary>
            /// No access rights
            /// </summary>
            None = 0,

            /// <summary>
            /// Read-only access
            /// </summary>
            ReferenceOnly = 1,

            /// <summary>
            /// Read/Write access
            /// </summary>
            Editable = 2
        }

        /// <summary>
        /// Target page
        /// </summary>
        /// <remarks>Tab menu index</remarks>
        public enum TabTargetPage
        {
            /// <summary>
            /// Disease/Symptoms
            /// </summary>
            Disease = 0,

            /// <summary>
            /// Evaluation/Treatment examples
            /// </summary>
            Situation = 1,

            /// <summary>
            /// Images
            /// </summary>
            ImageList = 2,

            /// <summary>
            /// Evidence/Description
            /// </summary>
            Evidence = 3,

            /// <summary>
            /// Detailed information
            /// </summary>
            Actions = 4
        }

        /// <summary>
        /// Key item types used in DAO
        /// </summary>
        /// <remarks>Indicates search key items</remarks>
        public enum KeyValueType
        {
            DiseaseID = 0,
            SituationID = 1,
            SituationOrderSetID = 2,
            SituationOrderSetParentID = 3,
            SituationOrderSetSampleID = 4,
            SituationOrderSetPatientProfileID = 5,
            SrlID = 6,
            Name = 7,
            Code = 8
        }

        /// <summary>
        /// Upload file edit mode
        /// </summary>
        /// <remarks>Indicates new or update</remarks>
        public enum UploadFileMode
        {
            /// <summary>
            /// New
            /// </summary>
            New = 0,

            /// <summary>
            /// Update
            /// </summary>
            Edit = 1
        }

        /// <summary>
        /// Disease action type and action items data type
        /// </summary>
        /// <remarks>Indicates header or body</remarks>
        public enum ActionDataType
        {
            /// <summary>
            /// Undefined
            /// </summary>
            Undefined = -1,

            /// <summary>
            /// Body part
            /// </summary>
            Body = 0,

            /// <summary>
            /// Header row
            /// </summary>
            Header = 1
        }

        /// <summary>
        /// Page type
        /// </summary>
        /// <remarks>Indicates display content</remarks>
        public enum PageType
        {
            Undefined = 0,
            Disease = 1,
            Situation = 2,
            ImageList = 3,
            Evidence = 4,
            Actions = 5,
            LabTest = 6,
            Drug = 7,
            Shirobon = 8,
            Handout = 9
        }

        /// <summary>
        /// Journal (Reference) source category
        /// </summary>
        /// <remarks>Indicates what the journal is linked to</remarks>
        public enum JournalType
        {
            All = 0,
            Image = 1,
            Evidence = 2,
            Step1 = 3
        }

        /// <summary>
        /// User role values
        /// </summary>
        /// <remarks>
        /// User role numbers
        /// Must match T_JP_Role table
        /// </remarks>
        public enum eRole
        {
            [Description("Undefined")]
            Undefined = 0,

            [Description("ElsevierAdmin")]
            ElsevierAdmin = 1,

            [Description("InstitutionAdmin")]
            InstitutionAdmin = 3,

            [Description("InstitutionIP")]
            InstitutionIP = 4,

            [Description("InstitutionUser")]
            InstitutionUser = 5,

            [Description("Individual")]
            Individual = 6,

            [Description("Trial")]
            Trial = 7,

            [Description("ElsevierEditor")]
            ElsevierEditor = 8,

            [Description("ElsevierManager")]
            ElsevierManager = 9,

            [Description("ElsevierUser")]
            ElsevierUser = 10,

            [Description("VMAdmin")]
            VMAdmin = 11,

            [Description("VMUser")]
            VMUser = 12,

            [Description("ElsevierSales")]
            ElsevierSales = 13,

            [Description("TechnicalSupport")]
            TechnicalSupport = 14,

            [Description("DvdUser")]
            DvdUser = 15,

            [Description("Agency")]
            Agency = 16,

            [Description("VMContents")]
            VMContents = 17,

            [Description("InstitutionSelfUser")]
            InstitutionSelfUser = 19,

            [Description("Tentative")]
            Tentative = 20,

            [Description("RegistrationID")]
            RegistrationID = 21,

            [Description("Subscriber")]
            Subscriber = 22,

            [Description("Entitlement Audit")]
            EntitlementAudit = 23,

            [Description("Entitlement Support")]
            EntitlementSupport = 24,

            [Description("JP Support")]
            JPSupport = 25,

            [Description("Elsevier Marketing")]
            ElsevierMarketing = 26,

            [Description("Individual Management")]
            IndividualManagement = 27,

            [Description("Support Admin")]
            SupportAdmin = 28,

            [Description("RegistrationIDForM3")]
            RegistrationIDForM3 = 29,

            [Description("RegistrationIDForTrialM3")]
            RegistrationIDForTrialM3 = 30,

            [Description("TrialSubscriber")]
            TrialSubscriber = 31
        }

        /// <summary>
        /// Login type
        /// </summary>
        public enum eLoginType
        {
            /// <summary>
            /// Undefined (Not logged in)
            /// </summary>
            Undefined = 0,

            /// <summary>
            /// Password
            /// </summary>
            Password = 1,

            /// <summary>
            /// IP Address
            /// </summary>
            IpAddress = 2
        }

        /// <summary>
        /// Message type
        /// </summary>
        public enum MessageType
        {
            Undefined = 0,
            Question = 1,
            Information = 2,
            Warning = 3,
            Error = 4,
            Fatal = 5
        }

        /// <summary>
        /// Image link behavior
        /// </summary>
        public enum ImageLinkBehavior
        {
            /// <summary>
            /// Show icon and set link
            /// </summary>
            IconAndLink = 0,

            /// <summary>
            /// Show icon but no link
            /// </summary>
            IconOnly = 1,

            /// <summary>
            /// Hide icon
            /// </summary>
            HideIcon = 2
        }

        public enum ReturnCode
        {
            Normal = 0,
            LogicalError = 1,
            FatalError = 9
        }

        public enum JobResult
        {
            Success = 0,
            Warning = 1,
            Error = 9
        }

        public enum ImageType
        {
            Undefined = 0,
            RawImage = 1,
            TbImage = 2,
            AdImage = 3,
            DpImage = 4
        }

        public enum StepEventType
        {
            Message = 0,
            StartEvent = 1,
            EndEvent = 2
        }

        /// <summary>
        /// Demo type
        /// </summary>
        public enum DemoType
        {
            /// <summary>
            /// No demo
            /// </summary>
            None = 0,

            /// <summary>
            /// Demo 1
            /// </summary>
            DEMO = 1
        }

        public enum ButtonAction
        {
            Undefined = 0,
            ShowForm = 1,
            ShowFormAsDialog = 2,
            CloseForm = 3,
            UpdateDatabase = 4,
            SelectDatabase = 5,
            AnyTransaction = 6
        }

        public enum KeyDataType
        {
            Integer,
            String,
            CsvString
        }

        public enum InputType
        {
            Undefined,
            Entry,
            SearchCriteria,
            MultiPurpose
        }

        public enum ContentType
        {
            Text,
            Image
        }

        public enum MinMax
        {
            MIN,
            MAX
        }

        /// <summary>
        /// Tries to parse an enum value by name
        /// </summary>
        public static object TryParseEnumByName(Type enumType, string enumName)
        {
            if (Enum.IsDefined(enumType, enumName))
            {
                try
                {
                    return Enum.Parse(enumType, enumName, false);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }
}