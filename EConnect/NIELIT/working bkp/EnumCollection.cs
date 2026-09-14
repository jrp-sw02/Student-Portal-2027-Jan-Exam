using System.ComponentModel;

namespace EConnect.NIELIT
{
	public enum enmCourseType
	{
		[Description("Certification Course")]
		CertificationCourse = 1,
		[Description("Certification Exam")]
		CertificationExam = 2,
		 [Description("IRDA Category")]
		IRDACat = 8
		
	}
	public enum enmApplicationSource
	{
		[Description("Candidate")]
		Candidate = 1,
		[Description("CSC")]
		CSC = 2
	}
    //public enum enmExamCenterType
    //{
    //    [Description("Both")]
    //    Both = 3,
    //    [Description("Primary Center")]
    //    Primary = 1,
    //    [Description("Secondary Center")]
    //    Secondary = 2
    //}
    public enum enmExamCenterType
    {
        [Description("Both")]
        Both = 3,
        [Description("Online Center")]
        Primary = 1,
        [Description("Offline Center")]
        Secondary = 2
    }
	public enum enmRegistrationType
	{
		[Description("New")]
		New = 1,
		[Description("Transfer")]
		Transfer = 2,
		[Description("Auto Upgradation")]
		AutoUpgradation = 3,
		[Description("ReRegistration")]
		ReRegistration = 4,
		[Description("New Registration on Previous Registration Cancellation Request")]
		NewOnCancellationRequest = 5,
		[Description("New Registration as  Previous Registration Period is Expired ")]
		NewOnExpired = 6,
		        [Description("Special Registration")]
        SpecialRegistration = 7,
        [Description("Special Extension")]
        SpecialExtension = 8
	}
	public enum enmApplicantType
	{
		[Description("Direct")]
		Direct = 1,
		[Description("Institute")]
		Institute = 2
	}
	public enum enmSelectionType
	{
		[Description("Compulsory")]
		Compulsory = 1,
		[Description("Elective")]
		Elective = 2
	}
	public enum enmAccreditationStatus
	{ 
		[Description("Full Extended")]
		FullExtended = 1,
		[Description("Full")]
		Full = 2,
		[Description("Extended")]
		Extended = 3,
		[Description("Provisional")]
		Provisional = 4,
		[Description("Withdrawal")]
		Withdrawal = 5,
		[Description("Acknowledged")]
		Acknowledged = 6,
		[Description("Rejected")]
		Rejected = 7,
		[Description("Deferred")]
		Deferred = 8

	}
	public enum enmRegistrationStatus
	{
		[Description("Registered")]
		Registered = 1,
		[Description("Re-Registered")]
		ReRegistered = 2,
		[Description("Project Pending")]
		ProjectPending = 3,
		[Description("Completed")]
		Completed = 4,
		[Description("Expired")]
		Expired = 5,
		[Description("Cancelled")]
		Cancelled = 6
	}
	public enum enmExamSchedule
	{ 
		[Description("Every Month Exam Cycle")] Monthly = 1,
		[Description("Periodic Exam Cycle")]
		Periodic = 2,
		[Description("Half Yearly Exam Cycle")]
		HalfYearly = 3,
		[Description("Yearly Exam Cycle")]
		Yearly = 4,
		[Description("Quarterly Exam Cycle")]
		Quarterly = 5,
		[Description("Fortnightly Exam Cycle")]
		Fortnightly = 6,
		[Description("Weekly Exam Cycle")]
		Weekly = 7,
		[Description("Daily Exam Cycle")]
		Daily = 8
	}
	public enum enmBatchStatus
	{
		[Description("Created")]
		Created = 1,
		[Description("Under Processing")]
		UnderProcessing = 2,
		[Description("Completed")]
		Completed = 3
	}    
	public enum enmApplicationType
	{
		[Description("Course Registration Application")]
		CourseRegistrationApplication = 1,
		[Description("Certificate Exam Application")]
		CertificateExamApplication = 2,
		[Description("Course Exam Application")]
		CourseExamApplication = 3,
		[Description("Module Certificate Request")]
		ModuleCertificateRequest = 4,
        [Description("Course Project Application")]
        CourseProjectApplication = 5,
        [Description("Course VAF ACF Application")]
        CourseVafAcfApplication = 6,
        [Description("Mercy Case Registration")]
        MercyCaseRegistration = 9,
        [Description("Special Extension")]
        SpecialExtension = 10

	}
	public enum enmRequestStatus
	{
		[Description("Applied")]
		Applied = 1,
		[Description("Received")]
		Received = 2,
		[Description("Under Processing")]
		UnderProcessing = 3,
		[Description("Rejected")]
		Rejected = 4,
		[Description("Kept in Abeyance")]
		KeptInAbeyance = 5,
		[Description("Completed")]
		Completed = 6
	}
	public enum enmUmcDecisionType
	{
		[Description("No Action")]
		Applied = 1,
		[Description("Cancel candidature for Particular Paper")]
		CancelCandidatureForParticularPaper = 2,
		[Description("Debar for a period of 3 chances")]
		DebarForAperiodofThreeChances = 3,
		[Description("Debar for a period of 5 chances")]
		DebarForAperiodofFiveChances = 4,
		[Description("Cancel candidature fo all papers")]
		CancelCandidatureForAllPaper = 5,
		[Description("Debar for a period of 2 subsequent exams")]
		DebarForAperiodofTowSubsequentExams =6
	}
    public enum enmVaf_AcfApplicationStatus
    {
        [Description("Verified By Institute But VAF ACF Not Deposited")]
        VerifiedButVafAcfNotPaidByInstitute = 1,
        [Description("Verified By Institute And VAF ACF Deposited")]
        VerifiedAndVafAcfPaidByInstitute = 2
    }
	public enum enmDownloadableType
	{
		[Description("Instructions")]
		Instructions=1,
		[Description("Notification")]
		Notification=2,
		[Description("Brochure")]
		Brochure=3,
		[Description("Syllabus")]
		Syllabus=4,
		[Description("FAQ")]
		FAQ = 5,
		[Description("Step By Step Process For Online Application Form")]
		StepByStepProcessForOnlineApplicationForm = 6,
		[Description("About Course")]  AboutCourse = 7,
		[Description("Notice")]
		Notice = 8,
		[Description("Project Guideline")]
		ProjectGuideline = 9,
		[Description("Performa (Sample)")]
		PerformaSample = 10
	}
	public enum enmCourseApplicationStatus
	{
		[Description("Applied But Fee Not Deposited By Candidate")]
		AppliedButFeeNotDepositedByCandidate = 1,
		[Description("Fee Deposited By Candidate But Application Not Received By NIELIT")]
		FeeDepositedByCandidateButApplicationNotReceivedByNIELIT=2,
		[Description("Applied But Pending For Institute Verification")]
		AppliedButPendingForInstituteVerification=4,
		[Description("Aplication Verified By Institute")]
		ApplicationVerifiedByInstitute=5,
		[Description("Fee Paid By Institute But Application Pending To Dispatch To NIELIT")]
		FeePaidByInstituteButApplicationPendingToDispatchToNIELIT = 6,
		[Description("Application Dispatched By The Institute To NIELIT")]
		ApplicationDispatchedByTheInstituteToNIELIT=8,
		[Description("Application Received By NIELIT")]
		ApplicationReceivedByNIELIT=10,
		[Description("Payment Verification Pending")]
		PaymentVerificationPending = 12,
		[Description("Aplication Found Dublicate")]
		ApplicationFoundDublicate = 13,
		[Description("Kept in Abeyance")]
		KeptInAbeyance=14,
		[Description("Application Rejected With Reason")]
		ApplicationRejectedWithReason=15,
		[Description("Application Verified By NIELIT And Forwarded To Registration Wing")]
		ApplicationVerifiedByNIELITAndForwardedToRegistrationWing=16,
		[Description("Registration Number Alloted")]
		RegistrationNumberAlloted=18,
		[Description("Application Rejected by Institute")]
		ApplicationRejectedbyInstitute = 20
        
	}
	public enum enmCertificateExamApplicationStatus
	{
		[Description("Applied But Fee Not Deposited By Candidate")]
		AppliedButFeeNotDepositedByCandidate = 1,
		[Description("Fee Deposited By Candidate But Application Not Received By Regional Centre")]
		FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre=3,
		[Description("Applied But Pending For Institute Verification")]
		AppliedButPendingForInstituteVerification = 4,
		[Description("Aplication Verified By Institute")]
		ApplicationVerifiedByInstitute = 5,
		[Description("Fee Paid By Institute But Application Pending To Dispatch To Regional Centre")]
		FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre = 7,
		[Description("Application Dispatched By The Institute To Regional Centre")]
		ApplicationDispatchedByTheInstituteToRegionalCentre = 9,
		[Description("Application Received By Regional Centre")]
		ApplicationReceivedByRegionalCentre = 11,
		[Description("Payment Verification Pending")]
		PaymentVerificationPending = 12,
		[Description("Aplication Found Dublicate")]
		ApplicationFoundDublicate = 13,
		[Description("Kept in Abeyance")]
		KeptInAbeyance = 14,
		[Description("Application Rejected With Reason")]
		ApplicationRejectedWithReason = 15,
		[Description("Application Verified By Regional Centre And Forwarded To Examination Wing")]
		ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing=17,
		[Description("Exam Centre And Roll Number Alloted")]
		ExamCentreAndRollNumberAlloted=19,
		[Description("Application Rejected by Institute")]
		ApplicationRejectedbyInstitute = 20
	}
	public enum enmCourseExamApplicationStatus
	{
		[Description("Applied But Fee Not Deposited By Candidate")]
		AppliedButFeeNotDepositedByCandidate = 1,
		[Description("Fee Deposited By Candidate But Application Not Received By NIELIT")]
		FeeDepositedByCandidateButApplicationNotReceivedByNIELIT = 2,
		[Description("Applied But Pending For Institute Verification")]
		AppliedButPendingForInstituteVerification = 4,
		[Description("Application Verified By Institute")]
		ApplicationVerifiedByInstitute = 5,
		[Description("Fee Paid By Institute But Application Pending To Dispatch To NIELIT")]
		FeePaidByInstituteButApplicationPendingToDispatchToNIELIT = 6,
		[Description("Application Dispatched By The Institute To NIELIT")]
		ApplicationDispatchedByTheInstituteToNIELIT = 8,
		[Description("Application Received By NIELIT")]
		ApplicationReceivedByNIELIT = 10,
		[Description("Payment Verification Pending")]
		PaymentVerificationPending = 12,
		[Description("Application Found Duplicate")]
		ApplicationFoundDuplicate = 13,
		[Description("Kept in Abeyance")]
		KeptInAbeyance = 14,
		[Description("Application Rejected With Reason")]
		ApplicationRejectedWithReason = 15,
		[Description("Application Verified By NIELIT And Forwarded To Examination Wing")]
		ApplicationVerifiedByNIELITAndForwardedToExaminationWing = 21,
		[Description("Exam Centre And Roll Number Alloted")]
		ExamCentreAndRollNumberAlloted = 19,
		[Description("Application Rejected by Institute")]
		ApplicationRejectedbyInstitute=20,
        //Added for CHM(T)-O level project 13 May 2024
        [Description("CHM Project finalized by NIELIT")]
        CHMTProjectfinalizedbyNIELIT = 22
	}
	public enum enmFeeType
	{
		[Description("Registration Fee")]
		RegistrationFee = 1,
		[Description("Registration Cum Examination Fee")]
		RegistrationCumExaminationFee = 2,
		[Description("Re-Registration Fee")]
		ReRegistrationFee = 3,
		[Description("Examination Fee")]
		ExaminationFee = 4,
		[Description("Postage Fee Charged Toward Examination Correspondence")]
		PostageFeeChargedTowardExaminationCorrespondence = 5,
		[Description("Late Fee - Exam")]
		LateFeeExam = 6,
		[Description("Practical Fee")]
		PracticalFee = 7,
		[Description("Project Evaluation Fee")]
		ProjectEvaluationFee = 8,
		[Description("Improvement Of Paper Fee")]
		ImprovementOfPaperFee = 9,
		[Description("Project Resubmission Fee")]
		ProjectResubmissionFee = 10,
		[Description("Issue of Duplicate I-Card Fee")]
		IssueOfDuplicateICardFee = 11,
		[Description("Re-Totaling of Answer Script Fee")]
		ReTotalingOfAnswerScriptFee = 12,
		[Description("Disclosuse of Answer Script Fee")]
		DisclosuseOfAnswerScriptFee = 13 ,
		[Description("Final Project Evaluation Fee")]
		FinalProjectEvaluationFee = 14,
		[Description("Final Project Resubmission Fee")]
		FinalProjectResubmissionFee = 15,
		[Description("Certificate re-issuance, in case of corrections")]
		CertificateReIssuanceInCaseOfCorrections = 16,
		[Description("Certificate re-issuance, in case of loss of certificate")]
		CertificateReIssuanceInCaseOfLossOfCertificate= 17,
		[Description("Certificate re-issuance, in case of mutilated certificate")]
		CertificateReIssuanceInCaseOfMutilatedCertificate = 18,
		[Description("Late Fee - Registration")]
		LateFeeRegistration = 19,
		[Description("Module Certificate")]
		ModuleCertificate = 20,
        [Description("Theory with Practical Fee")]
        TheorywithPracticalFee = 22,
        [Description("Practical Fee with Theory")]
        PracticalFeewithTheory = 23,
              [Description("Mercy Registration Fee")]
        MercyRegistrationFee = 28,
        [Description("Special Extension Registration Fee")]
        SpecialExtensionRegistrationFee = 29,
        [Description("ACF VAF Fee")]
        ACF_VAF_fee = 24
	}
	public enum enmActivity
	{
		[Description("Date of Commencement of Online Fill in Examination Application Form")]
		DateFfCommencementOfOnlineFillInExaminationApplicationForm =1 ,
		[Description("Last Date of Online Submission of Examination Application Form")]
		LastDateOfOnlineSubmissionOfExaminationApplicationForm = 2,
		[Description("Last Date of Online Submission of Examination Application Form - With Late Fee")]
		LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee = 3,
		[Description("Last Date Of Submission Of Online Registration Application")]
		LastDateOfSubmissionOfOnlineRegistrationApplication = 4,
		 [Description("Last Date Of Submission Of Online Registration Application With Late Fee")]
		LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee = 5
	}
	public enum enmCSCActivity
	{
		[Description("Fill Application Form")]
		FillApplicationForm = 1,
		[Description("Fill Form and Deposit Fee")]
		FillFormandDepositFee = 2,
		[Description("Deposit Fee")]
		DepositFee = 3
	}
	public enum enmCurrentRegistrationStatus
	{
		[Description("None")]
		None = 0,
		[Description("Fresh registration as your previous registration number is not found,")]
		EligibleForNewRegistrationAsPreviousRegistrationNotFound = 1, // NEW (New Registration No)


		[Description("You have not yet completed your current level and  validity period not expired yet. If you apply for registration in same or other level, your current level registration would be cancelled and a fresh registration would be allotted and the credits obtained by you against  the current cancelled registration will lapse for the purpose of completing the examination at the new level.")]
		RegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndValidityPeriodNotLapsed = 2, //NewOnCancellationRequest (New Registration No)

		[Description("You have completed your current level. If you apply for registration in same or other level, your current level registration would be transferred to next level and same registration number would be allotted and the credits obtained by you against  the current passed registration will be given to you for the purpose of completing the examination at the new level.")]
		CompletedAsAllModulesOfCurrentLevelPassedCompletelyWithinRegistrationPeriod = 3,//Transfer (Same Registration No)

		[Description("Validity period of current level registration has expired and you are now in grace period of re-registration and eligible for re-registration in same level with same registration number")]
		ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired = 4,//ReRegistration (Same Registration No)

		[Description("You have not passed all modules/papers/practicals/projects in given validity period. If you apply for registration in same or other level, your current level registration would be cancelled and a fresh registration would be allotted and the credits obtained by you against  the current cancelled registration (due to expiry of re-registration period) will lapse for the purpose of completing the examination at the new level.")]
		ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod = 5,//NewOnExpired (New Registration No)

		[Description("You have passed all modules/papers/practicals/projects in last exam attempted by you. If you apply for registration in same or other level, your current level registration wolud be transferred to next higher level and same registration would be allotted and the credits obtained by you against  the current passed registration will be given to you for the purpose of completing the examination at the new level.")]
		CompletedAllModulesPassedInPreviousExamAndEligibleForAutoUpgradation = 6,//AutoUpgradation (Same Registration No)

		[Description("You have completed highest level and you can not apply for new registration as we have not new level.")]
		CompletedEligibleForNextLevelRegistrationButNoNextLevelAvailable = 7,

		[Description("You have not passed all modules/papers/practicals/projects in re-registration validity period but re-registration validity period not expired yet. If you apply for registration in same or other level, your current level re-registration would be cancelled and a fresh registration would be allotted and the credits obtained by you against  the current cancelled registration will lapse for the purpose of completing the examination at the new level.")]
		ReRegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndReRegistrationValidityPeriodNotLapsed = 8,//NewOnCancellationRequest (New Registration No)
	}
	public enum enmUpdateFields
	{
		[Description("Name")]
		Name = 1,
		[Description("DOB")]
		DOB = 2,
		[Description("Gender")]
		Gender = 3,
		[Description("Occupation")]
		Occupation = 4,
		[Description("Caste Category")]
		CasteCategory = 5,
		[Description("Father Name")]
		FatherName = 6,
		[Description("Mother Name")]
		MotherName = 7,
		[Description("Guardian")]
		Guardian = 8,
		[Description("Salutation")]
		Salutation = 9
	}
    public enum enmPuraskarApplicationStatus
    {
        [Description("Applied By Candidate")]
        AppliedByCandidate = 1,
        [Description("Verified By Institute But NIELIT Verification Pending")]
        VerifiedByInstituteButNIELITVerificationPending = 2,
        [Description("Rejected By Institute")]
        RejectedByInstitute = 3,
        [Description("Verified By Exam Wing But Finance Wing Verification Pending")]
        VerifiedByExamWingButFinanceWingVerificationPending = 4,
        [Description("Rejected By Exam Wing")]
        RejectedByExamWing = 5,
        [Description("Verified By Finance Wing But Payment To Be Processed")]
        VerifiedByFinanceWingButPaymentToBeProcessed = 6,
        [Description("Rejected By Finance Wing")]
        RejectedByFinanceWing = 7,
        [Description("Payment Released To Bank")]
        PaymentReleasedToBank = 8,
        [Description("Payment Transferred To Bank Account")]
        PaymentTransferredToBankAccount = 9,
        [Description("Payment Rejected By Bank")]
        PaymentRejectedByBank = 10,

    }
    public enum enmActivityVirtualAcademy
    {
        [Description("Date of Commencement of Online Fill in Examination Application Form")]
        DateFfCommencementOfOnlineFillInExaminationApplicationForm = 1,
        [Description("Last Date of Online Submission of Examination Application Form")]
        LastDateOfOnlineSubmissionOfExaminationApplicationForm = 2,
        [Description("Last Date of Online Submission of Examination Application Form - With Late Fee")]
        LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee = 3,
        [Description("Last Date Of Submission Of Online Registration Application")]
        LastDateOfSubmissionOfOnlineVirtualAcademyRegistrationApplication = 4,
        //[Description("Last Date Of Submission Of Online Registration Application With Late Fee")]
        //LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee = 5
    }
    public enum enmFeeTypeVirtualAcademy
    {
        [Description("Virtual Academy Registration Fee")]
        VirtualAcademyRegistrationFee = 1,
        [Description("Virtual Academy Re-Registration Fee")]
        VirtualAcademyReRegistrationFee = 2, //3 to 2
        [Description("Virtual Academy Late Fee - Registration")]
        VirtualAcademyLateFeeRegistration = 3 // 19 to 3 change number

    }
    public enum enmApplicationTypeVirtualAcademy
    {
        [Description("Virtual Academy Course Registration Application")]
        VirtualAcademyCourseRegistrationApplication = 1

    }
#region[Reena added on 17-01-2023]
    public enum enmCandidateUpdateRequestFields //added by Reena
    {
        [Description("Handicapped(Disability)")]
        Handicapped = 1,
        [Description("Marital Status")]
        MaritalStatus = 2,
        [Description("Caste")]
        CasteCategory = 3,
        [Description("Mobile Number")]
        MobileNumber = 4,
        [Description("Email Address")]
        EmailAddress = 5,
        [Description("Correspondence Address")]
        CorrespondenceAddress = 6,
        [Description("Educational Qualification")]
        EducationalQualification = 7,
        [Description("Photo")]
        Photo = 9,
        [Description("Thumb")]
        Thumb = 10,
        [Description("Signature")]
        Signature = 11,
        [Description("Name")]
        Name = 12,
        [Description("Father Name")]
        FatherName = 13,
        [Description("Mother Name")]
        MotherName = 14,
        [Description("Permanent Address")]
        PermanentAddress = 15,
        [Description("Date of Birth")]
        DOB = 16,
        [Description("Gender")]
        Gender = 17,
        [Description("Registration Type")]
        RegistrationType = 18,
        [Description("Candidate Name")]
        CandidateName_FinalCert = 19,
        [Description("Father's Name")]
        FatherName_FinalCert = 20,
        [Description("Mother's Name")]
        MotherName_FinalCert = 21,

    }
    public enum enmCandidateUpdateRequestStatus 
    {
        [Description("Request Submitted")]
        RequestReceived = 1,
        [Description("Documents Uploaded")]
        DocumentsUploaded = 2,
        [Description("Form final submitted but fees not paid")]
        Formfinalsubmittedbutfeesnotpaid = 3,
        [Description("Fees paid but not verified")]
        Feespaidbutnotverified = 4,
        [Description("Fees verified by accounts")]
        Feesverifiedbyaccounts = 5,
        [Description("Application under process")]
        Applicationunderprocess = 6,
        [Description("Application Rejected")]
        ApplicationRejected = 13,
        [Description("Kept in Abeyance")]
        KeptinAbeyance = 10,
        [Description("Updation Complete")]
        UpdationComplete = 11,
        [Description("Form final submitted")]
        Formfinalsubmittedforfreecases = 12,
        [Description("Application Verified")]
        ApplicationVerified = 14,

    }
    #endregion


}