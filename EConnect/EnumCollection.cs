using System.ComponentModel;
namespace EConnect
{
    public enum enmLanguage
    {
        [Description("Default-English")]
        DefaultEnglish = 0,
        [Description("English")]
        English = 1,
        [Description("Hindi")]
        Hindi = 2,
        [Description("Punjabi")]
        Punjabi = 3 
    }

    public enum Gender
    {
        [Description("Female")]
        Female = 1,
        [Description("Male")]
        Male = 2 
    }


    public enum enmPaymentSource
    {
        [Description("Yes, Examination Fee Will be Paid by Me")]
        Candidate = 1,
        [Description("No, Accredited Institute Will Pay Examination Fee")]
        Institute = 2
    }
    public enum ConfigurationType
    {
        [Description("Payment Mode Configuration")]
        PaymentMode = 1,
        [Description("Notification Message Configuration")]
        NotificationMessage = 2
    }

    public enum enmLocationType
    {
        [Description("Country")]
        Country = 1,
        [Description("State")]
        State = 2,
        [Description("Zone")]
        Zone = 3,
        [Description("District")]
        District =4,
        [Description("Tehsil")]
        Tehsil = 5,
        [Description("City")]
        City = 6
    }

    public enum enmMaritalStatus
    {
        [Description("Single")]
        Single = 1,
        [Description("Married")]
        Married = 2,
        [Description("Divorced")]
        Divorced = 3,
        [Description("Widowed")]
        Widowed = 4 
    }

    public enum enmCastCategory
    {
        [Description("General")]
        General = 1,
        [Description("Scheduled Caste")]
        ScheduledCaste = 2,
        [Description("Scheduled Tribe")]
        ScheduledTribe = 3,
        [Description("Other Backward Class")]
        OtherBackwardClass = 4
    }

   
    public enum enmModuleType
    {
        [Description("Theory")]
        Theory = 1,
        [Description("Term Paper")]
        TermPaper = 2,
        [Description("Practical")]
        Practical = 3,
        [Description("Project")]
        Project = 4,
        [Description("Bridge")]
        Bridge = 5
    }

    public enum enmDaakReceiptDispatchMode
    {
        [Description("By Hand")]
        ByHand = 1,
        [Description("Speed Post")]
        SpeedPost = 2,
        [Description("Registered Post")]
        RegisteredPost = 3,
        [Description("Under Postal Certificate")]
        UPC = 4,
        [Description("Courier")] 
        Courier = 5,
        [Description("Ordinary Post")]
        OrdinaryPost = 6
    }

    public enum enmPaymentMode
    {
        [Description("Demand Draft")]
        DemandDraft = 1,
        [Description("Online")]
        Online = 2,
        [Description("Multi City Cheque")]
        MultiCityCheque = 3,
        [Description("Cash")]
        Cash = 4,
        [Description("CSC SPV")]
        CSCSPV = 5,
        [Description("NEFT RTGS")]
        NEFTRTGS = 6
    }

    public enum enmPaymentStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Paid")]
        Paid = 2,
        [Description("Failed")]
        Failed = 3,
        [Description("Paid-Not Verified")]
        PaidButNotVerified = 4
    }

    public enum enmDemandNoteType
    {
        [Description("Single")]
        Single = 1,
        [Description("Multiple")]
        Multiple = 2
    }

    public enum enmAddressType
    {
        [Description("Correspondence Address")]
        CorrespondenceAddress = 1,
        [Description("Permanent Address")]
        PermanentAddress = 2
    }

   public enum enmNotificationEvents
   {
       [Description("After submitting the course exam application form")]
       AfterSubmittingTheCourseExamApplicationForm = 5,
       [Description("After Submitting the certificate exam application form")]
       AfterSubmittingTheCertificateExamApplicationForm = 6,
       [Description("After submitting the course registration application form")]
       AfterSubmittingTheCourseRegistrationApplicationForm = 7,
       [Description("After verification of course exam application by institute")]
       AfterVerificationOfCourseExamApplicationByInstitute = 8,
       [Description("After verification of certificate exam application by institute")]
       AfterVerificationOfCertificateExamApplicationByInstitute = 9,
       [Description("After verification of course registration application by institute")]
       AfterVerificationOfCourseRegistrationApplicationByInstitute = 10,
       [Description("After making payment")]
       AfterMakingPayment = 11,
       [Description("After verification of the payment")]
       AfterVerificationOfThePayment = 12,
       [Description("After receiving and verifying of form data at Head Office")]
       AfterReceivingAndVerifyingOfFormDataAtHeadOffice = 13,
       [Description("After Successfull Registration")]
       AfterSuccessfullRegistration = 14,
       [Description("After Marking the Course Registration Application as Kept in Abeyance")]
       AfterMarkingtheCourseRegistrationApplicationasKeptinAbeyance = 15,
       [Description("After Marking the Course Registration Application as RejectedWithReason")]
       AfterMarkingtheCourseRegistrationApplicationasRejectedWithReason = 16,
       [Description("After Marking the Course Registration Application as Verified")]
       AfterMarkingtheCourseRegistrationApplicationasVerified = 17,
       [Description("After Sending the Special Registration or Special Extension for approval as Verified")]
       AfterSendingtheSpecialRegistrationorSpecialExtensionforapprovalasVerified = 19
   }

    public enum enmExamSession
    {
        [Description("Forenoon")]
        Forenoon = 1,
        [Description("Afternoon")]
        Afternoon = 2
    }
    }
