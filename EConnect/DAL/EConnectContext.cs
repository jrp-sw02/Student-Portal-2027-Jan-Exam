using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using EConnect.HRMS;
using EConnect.NIELIT;
using EConnect.URM;

/// <summary>
/// Summary description for EConnectContext
/// </summary>
namespace EConnect.DAL
{
    public class EConnectContext : DbContext
    {


        public DbSet<ExamCycleExceptionalFeature> ExamCycleExceptionalFeatures { get; set; }
        public DbSet<MenuObject> MenuObjects { get; set; }
        public DbSet<MenuObjectType> MenuObjectTypes { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UsersType> UserTypes { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<ExternalEntity> ExternalEntities { get; set; }
        public DbSet<UserLoginOtpDetail> UserLoginOtpDetails { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<MaritalStatus> MaritalStatus { get; set; }
        public DbSet<CastCategory> CastCategories { get; set; }
        public DbSet<ApplicantType> ApplicantTypes { get; set; }
        public DbSet<DaakReceiptDispatchMode> DaakReceiptDispatchModes { get; set; }
        public DbSet<EducationalQualification> EducationalQualifications { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<ModuleType> ModuleTypes { get; set; }
        public DbSet<PaymentMode> PaymentModes { get; set; }
        public DbSet<NotificationEvent> NotificationEvent { get; set; }
        public DbSet<Religion> Religions { get; set; }

        public DbSet<ExamCenter> ExamCenters { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseType> CourseTypes { get; set; }
        public DbSet<CourseRevision> CourseRevisions { get; set; }
        public DbSet<RegionalCenter> RegionalCenters { get; set; }
        public DbSet<CourseWiseState> CourseWiseStates { get; set; }
        public DbSet<Batch> Batchs { get; set; }
        public DbSet<BatchItem> BatchItems { get; set; }
        public DbSet<ApplicationType> ApplicationTypes { get; set; }
        public DbSet<ApplicationStatus> ApplicationStatuses { get; set; }
        public DbSet<ApplicationStatusMapping> ApplicationStatusMappings { get; set; }
        public DbSet<Downloadable> Downloadables { get; set; }
        public DbSet<DownloadableHistory> DownloadableHistory { get; set; }
        public DbSet<CertificateExamResult> CertificateExamResults { get; set; }
        public DbSet<AccreditationDetail> AccreditationDetails { get; set; }
        public DbSet<AccreditationStatus> AccreditationStatus { get; set; }
        public DbSet<Institute> Institutes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<ExamCentreChoiceRefilling> ExamCentreChoiceRefillings { get; set; }

        public DbSet<AddressUpdateTemp> AddressUpdateTemps { get; set; }

        public DbSet<Candidate_Personal_UpdateTemp> Candidate_Personal_UpdateTemps { get; set; }
        public DbSet<Candidate_Qualifications_UpdateTemp> Candidate_Qualifications_UpdateTemps { get; set; }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateContactDetail> CandidateContactDetails { get; set; }
        public DbSet<CandidateContactHistory> CandidateContactHistoryDetails { get; set; }
        public DbSet<CandidateHistory> CandidateHistory { get; set; }
        public DbSet<CandidateQualificationDetail> CandidateQualificationDetails { get; set; }
        public DbSet<CertificateExamApplication> CertificateExamApplications { get; set; }

        public DbSet<CertificateExamApplicationHistory> CertificateExamApplicationHistory { get; set; }


        public DbSet<CourseRegistrationApplication> CourseRegistrationApplications { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<SelectionType> SelectionTypes { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<CandidateRequest> CandidateRequests { get; set; }
        public DbSet<CandidateRequestType> CandidateRequestTypes { get; set; }
        public DbSet<CutOffDate> CutOffDates { get; set; }
        public DbSet<ExamTimeTable> ExamTimeTables { get; set; }
        public DbSet<ModulePracticalEligibility> ModulePracticalEligibilities { get; set; }
        public DbSet<CourseExamSession> CourseExamSessions { get; set; }
        public DbSet<CourseExamApplication> CourseExamApplications { get; set; }
        public DbSet<CourseExamApplicationDetail> CourseExamApplicationDetails { get; set; }
        public DbSet<ResultGrade> ResultGrades { get; set; }
        public DbSet<UnfairMeansCase> UnfairMeansCases { get; set; }
        public DbSet<ExamVenue> ExamVenues { get; set; }
        public DbSet<Right> UserRights { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Parity> Parities { get; set; }
        public DbSet<ExemptionRule> ExemptionRules { get; set; }
        public DbSet<CourseRegistrationPolicy> CourseRegistrationPolicies { get; set; }
        public DbSet<RegistrationType> RegistrationTypes { get; set; }
        public DbSet<SentEmail> SentEmails { get; set; }
        public DbSet<SentSMS> SentSMS { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<OnlineRefund> OnlineRefunds { get; set; }
        public DbSet<ReceivedSms> ReceivedSmss { get; set; }

        public DbSet<CSCTransaction> CSCTransactions { get; set; }
        public DbSet<PaymentStatus> PaymentStatuss { get; set; }
        public DbSet<DemandDraftTransaction> DemandDraftTransactions { get; set; }
        public DbSet<OnlineTransaction> OnlineTransaction { get; set; }

        public DbSet<DemandNote> DemandNotes { get; set; }
        public DbSet<FeeType> FeeTypes { get; set; }
        public DbSet<FeeDetail> FeeDetails { get; set; }
        public DbSet<CSCCharges> CSCCharges { get; set; }
        public DbSet<UmcDecisionType> UmcDecisionTypes { get; set; }

        public DbSet<ExamSession> ExamSessions { get; set; }
        public DbSet<Activity> ExamActivities { get; set; }
        public DbSet<Occupation> Occupations { get; set; }
        public DbSet<ExaminationCycle> ExaminationCycles { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<QualificationEligibility> QualificationEligibility { get; set; }
        public DbSet<QualificationLevel> QualificationLevels { get; set; }
        public DbSet<RegistrationDetail> RegistrationDetails { get; set; }
        public DbSet<RegistrationStatus> RegistrationStatus { get; set; }
        public DbSet<ExamWiseExamCenter> ExamWiseExamCenters { get; set; }
        public DbSet<TimeTablePattern> TimeTablePatterns { get; set; }
        public DbSet<ExamSuperintendent> ExamSuperintendents { get; set; }
        public DbSet<NEFTTransaction> NEFTTransactions { get; set; }
        public DbSet<NEFTBankTransaction> NEFTBankTransactions { get; set; }
        public DbSet<DeficiencyCode> DeficiencyCodes { get; set; }
        public DbSet<BatchItemDeficiencyDetail> BatchItemDeficiencyDetails { get; set; }
        public DbSet<NoticeEvent> NoticeEvents { get; set; }
        public DbSet<RevisionChoice> RevisionChoices { get; set; }
        public DbSet<DebarredCandidate> DebarredCandidates { get; set; }


        //Added on 25 Nov 2021

        public DbSet<ProjectEligibility> ProjectEligibilities { get; set; }
        public DbSet<ProjectFee> ProjectFees { get; set; }
        public DbSet<CourseProjectApplication> CourseProjectApplications { get; set; }
        public DbSet<CourseProjectApplicationDetail> CourseProjectApplicationDetails { get; set; }


        public DbSet<CourseWiseUserMapping> CourseWiseUserMappings { get; set; }
        public DbSet<NEFTRefund> NEFTRefund { get; set; }
        public DbSet<Online_ChargeBackTransaction> Online_ChargeBackTransactions { get; set; }
        public DbSet<CandidateScholarship> CandidateScholarships { get; set; }

        public DbSet<IncomeCategory> IncomeCategories { get; set; }
        public DbSet<UpdateFields> UpdateFields { get; set; }
        public DbSet<Remarks> Remarks { get; set; }
        public DbSet<CourseWiseOccupationMapping> CourseWiseOccupationMappings { get; set; }

        public DbSet<RegnExamCourseBridge> RegnExamCourseBridges { get; set; }

        public DbSet<CertificateOABCPhoto> CertificateOABCPhotos { get; set; }
		
		public DbSet<ExamCentreChoiceStat> ExamCentreChoiceStats { get; set; }
        public DbSet<ExamCentreAvailableStat> ExamCentreAvailableStats { get; set; }
        public DbSet<ExamCentreExamDate> ExamCentreExamDates { get; set; }

        public DbSet<ModuleCertificateBatch> ModuleCertificateBatchs { get; set; }
        public DbSet<ModuleCertificateRequest> ModuleCertificateRequests { get; set; }

        public DbSet<AccCourseCompletionDate> AccCourseCompletionDates { get; set; }
        public DbSet<DisabilityType> DisabilityTypes { get; set; }

        //Added
        public DbSet<tempExamResultPub> tempExamResultPubs { get; set; }
	//Added 1 feb 2019
        public DbSet<cityType> cityTypeMas { get; set; }
        public DbSet<perfoCriteriaMas> perfoCriteriaMas { get; set; }
        //Added 13 feb 2019
        public DbSet<perfoReport> perfoReport { get; set; }
//Added 5 Sep 2020
        public DbSet<InstituteWithdrawal> insttWithdrawal { get; set; }


        public DbSet<TblSmsEmailBulk> tblSMSs { get; set; }
       // public DbSet<regdIcard> tbRegdCardPhoto { get; set; }
	   //Added 17 Feb 2020
	   public DbSet<DLCExemptedStates> DLCExemptedStatess { get; set; }
		public DbSet<perfoCriteriaMasNewSOP> perfoCriteriaMasNewSOP { get; set; }
        public DbSet<AccreditationDetailHistory> AccreditationDetailHistory { get; set; }
		//Added 15 June 2020 for NSQF Download images
		 public DbSet<CourseMappingWithNSQFCoursecode> CourseMappingWithNSQFCoursecodes { get; set; }
			public DbSet<NSQFCertPhasePrintDetail> NSQFCertPhasePrintDetails { get; set; }
            public DbSet<NSQFModuleCandidateMarks> NSQFModuleCandidateMarks { get; set; }
			public DbSet<CourseLevelDurations> CourseLevelDurationss { get; set; }

            //Added jkSah
            public DbSet<tblGender> tblGender { get; set; }
            //Added for protsahan
            public DbSet<PuraskarApplicationForm> PuraskarApplicationForms { get; set; }
            public DbSet<OnlineProtsahanExamModules> OnlineProtsahanExamModuless { get; set; }

            public DbSet<PuraskarAppMobileEmailOtpVerificationDetail> PuraskarAppMobileEmailOtpVerificationDetails { get; set; }
            public DbSet<PuraskarAppDocsVerificationAndDeclarationByInstt> PuraskarAppDocsVerificationAndDeclarationByInstts { get; set; }

            public DbSet<CourseWiseAmtPerPaperForOnlinePuraskarApp> CourseWiseAmtPerPaperForOnlinePuraskarApps { get; set; }
            public DbSet<PuraskarFeeReimbursementMaster> PuraskarFeeReimbursementMasters { get; set; }
            public DbSet<PuraskarFeeReimbursementInstalment> PuraskarFeeReimbursementInstalments { get; set; }

	    public DbSet<NSQFFreeCourseMapping> NSQFFreeCourseMapping { get; set; }
            public DbSet<NSQFFreeCourseGrant> NSQFFreeCourseGrants { get; set; }
            public DbSet<RegnICardCandidatePhotos> RegnICardCandidatePhotos { get; set; }

        //
            //Added 14 Jan 2023 DVP BCC Daily exam cycle
            public DbSet<NonWorkingDays> nonWorkingDays { get; set; }
            
            //Added 2 Feb 2023 Practical Changes
            public DbSet<PracticalCandidateMarks> PracticalCandidateMarks { get; set; }
            public DbSet<PracAllowedMarksEntry> PracAllowedMarksEntry { get; set; }

        //Added for Aadhaar Encryption
            public DbSet<CourseENCAaddhar> CourseENCAaddhar { get; set; }
           public DbSet<CertificateENCAddh> CertificateENCAddh { get; set; }
            public DbSet<aadhaarResponse> aadhaarResponse { get; set; }
           public DbSet<tblExemption> tblModuleExemption { get; set; }
        //Added for apaarValidation 22 Sep 2024
        public DbSet<apaarResponseRecd> apaarResponseRecd { get; set; }

		
        //Added for CHM(T) Change 13 June2024
           public DbSet<CHMTProjectManualEntry> CHMTProjectManualEntry { get; set; }
            #region[Added on 30/04/2023 by Reena]
            public DbSet<PracLocationType> PracLocationTypes { get; set; }
            public DbSet<PracLocation> PracLocations { get; set; }
            public DbSet<PracExamCenter> PracExamCenters { get; set; }

            public DbSet<OnlineLocationType> OnlineLocationTypes { get; set; }
            public DbSet<OnlineLocation> OnlineLocations { get; set; }
            public DbSet<OnlineExamCenter> OnlineExamCenters { get; set; }
            //Added_Master_Certificate_Course_Type_28_10_2024
            public DbSet<CertificateCourseType> CertificateCourseTypes { get; set; }

            public DbSet<CourseExamVafAcf> CourseExamVafAcfs { get; set; }
            // Added by  AMIT  Start
            public DbSet<projCriteriaMaster> projCriteriaMaster { get; set; }
            public DbSet<projStudentMaster> projStudentMaster { get; set; }
        //  public DbSet<BSBSchools> BSBSchools { get; set; }

        // Added by  AMIT End
        public DbSet<PaymentGateways> Paymentgateways { get; set; }
        public DbSet<PGErrorCodes> PGErrorCodes { get; set; }
        public DbSet<BankHashMismatch> BankHashMismatch { get; set; }
        #endregion
        // added by amit start
        public DbSet<projEmailMobExempt> projEmailMobExempts { get; set; }
        // added by amit end

        public DbSet<ApaarRequest> ApaarRequest { get; set; }

        public DbSet<AuthMode> AuthMode { get; set; }


        // Added by Komal
        public DbSet<MercyCase_UploadedDocs> MercyCase_UploadedDocs { get; set; }
        public DbSet<MercyDocumentStatus> MercyDocumentStatus { get; set; }

        public EConnectContext()
            : base()
        {
            // Get the ObjectContext related to this DbContext
            var objectContext = (this as IObjectContextAdapter).ObjectContext;

            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 600;
        }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Conventions.Remove<ColumnTypeCasingConvention>();

            //Mapping of class with table and schema
            //EConnect
            modelBuilder.Entity<ExamCycleExceptionalFeature>().ToTable("tblExamCycle_ExceptionalFeature");
            modelBuilder.Entity<LocationType>().ToTable("LocationType");
            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<MaritalStatus>().ToTable("Marital_Status");
            modelBuilder.Entity<CastCategory>().ToTable("Cast_Category");
            modelBuilder.Entity<ApplicantType>().ToTable("Applicant_Type");
            modelBuilder.Entity<DaakReceiptDispatchMode>().ToTable("Daak_Receipt_Dispatch_Mode");
            modelBuilder.Entity<EducationalQualification>().ToTable("Educational_Qualification");
            modelBuilder.Entity<Language>().ToTable("Language");
            modelBuilder.Entity<ModuleType>().ToTable("Module_Type");
            modelBuilder.Entity<PaymentMode>().ToTable("Payment_Mode");
            modelBuilder.Entity<NotificationEvent>().ToTable("Notification_Event");
            modelBuilder.Entity<Religion>().ToTable("Religion");
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<AddressType>().ToTable("AddressType");
            modelBuilder.Entity<Occupation>().ToTable("Occupation");
            modelBuilder.Entity<RegionalCenter>().ToTable("Regional_Center");
            modelBuilder.Entity<CourseWiseState>().ToTable("Course_Wise_State");
            modelBuilder.Entity<FeeType>().ToTable("Fee_Type");
            modelBuilder.Entity<FeeDetail>().ToTable("Fee_Detail");
            modelBuilder.Entity<CutOffDate>().ToTable("Cut_Off_Date");
            modelBuilder.Entity<ExamTimeTable>().ToTable("Exam_Time_Table");
            modelBuilder.Entity<ModulePracticalEligibility>().ToTable("Module_Practical_Eligibility");
            modelBuilder.Entity<CourseExamSession>().ToTable("Course_Exam_Session");
            modelBuilder.Entity<CourseExamApplication>().ToTable("Course_Exam_Application");
               modelBuilder.Entity<CourseExamApplicationDetail>().ToTable("Course_Exam_Application_Detail");
            modelBuilder.Entity<ResultGrade>().ToTable("Result_Grading");
            modelBuilder.Entity<UnfairMeansCase>().ToTable("UnfairMeansCase");
            modelBuilder.Entity<ExamVenue>().ToTable("Exam_Venue");
            modelBuilder.Entity<Parity>().ToTable("Parity");
            modelBuilder.Entity<ExemptionRule>().ToTable("Exemption_Rule");
            modelBuilder.Entity<CourseRegistrationPolicy>().ToTable("Course_Registraton_Policy");
            modelBuilder.Entity<RegistrationType>().ToTable("Registration_Type");
            modelBuilder.Entity<SentEmail>().ToTable("Sent_Email");
            modelBuilder.Entity<SentSMS>().ToTable("Sent_SMS");
            modelBuilder.Entity<Feedback>().ToTable("Feedback");
            modelBuilder.Entity<UmcDecisionType>().ToTable("UMC_DECISION_TYPE");

            modelBuilder.Entity<AddressUpdateTemp>().ToTable("Address_update_temp_sync");
            modelBuilder.Entity<Candidate_Personal_UpdateTemp>().ToTable("Candidate_update_temp_sync");
            modelBuilder.Entity<Candidate_Qualifications_UpdateTemp>().ToTable("Candidate_qualifications_temp_sync");
            modelBuilder.Entity<RevisionChoice>().ToTable("tblRevisionChoice");
            modelBuilder.Entity<DebarredCandidate>().ToTable("tblDebarredCandidate");


            //Added on 25 Nov 

            modelBuilder.Entity<ProjectEligibility>().ToTable("tblProjectEligibility");
            modelBuilder.Entity<ProjectFee>().ToTable("tblProjectFee");
            modelBuilder.Entity<CourseProjectApplication>().ToTable("Course_Project_Application");
            modelBuilder.Entity<CourseProjectApplicationDetail>().ToTable("Course_Project_Application_Detail");

            


            //Mapping of Parent Child record in Menu object table
            modelBuilder.Entity<Location>()
                .HasMany(c => c.ChildLocations)
                .WithOptional(p => p.ParentLocation);


            //EConnect.NIELIT
            modelBuilder.Entity<ExamCenter>().ToTable("Exam_Center");
            modelBuilder.Entity<CourseCategory>().ToTable("Course_Category");
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<CourseType>().ToTable("Course_Type");
            modelBuilder.Entity<CourseRevision>().ToTable("Course_Revision");
            modelBuilder.Entity<AccreditationDetail>().ToTable("Intitute_Accreditation_Detail");
            modelBuilder.Entity<AccreditationStatus>().ToTable("Accreditation_Status");
            modelBuilder.Entity<Institute>().ToTable("Institute");
            modelBuilder.Entity<ExamSession>().ToTable("Exam_Session");
            modelBuilder.Entity<Activity>().ToTable("Activity");
            modelBuilder.Entity<Candidate>().ToTable("Candidate");
            modelBuilder.Entity<SelectionType>().ToTable("Selection_Type");
            modelBuilder.Entity<Module>().ToTable("Module");
            modelBuilder.Entity<Batch>().ToTable("Batch");
            modelBuilder.Entity<BatchItem>().ToTable("Batch_Item");
            modelBuilder.Entity<ApplicationType>().ToTable("Application_Type");
            modelBuilder.Entity<ApplicationStatus>().ToTable("Application_Status");
            modelBuilder.Entity<ApplicationStatusMapping>().ToTable("Application_Status_Mapping");
            modelBuilder.Entity<CandidateRequest>().ToTable("Candidate_Request");
            modelBuilder.Entity<CandidateRequestType>().ToTable("Candidate_Request_Type");
            modelBuilder.Entity<Downloadable>().ToTable("Downloadable");
            modelBuilder.Entity<DownloadableHistory>().ToTable("Downloadable_History");
            modelBuilder.Entity<CertificateExamResult>().ToTable("Certificate_Exam_Result");
            modelBuilder.Entity<ExamWiseExamCenter>().ToTable("Exam_Wise_Exam_Center");
            modelBuilder.Entity<TimeTablePattern>().ToTable("Time_Table_Pattern");
            modelBuilder.Entity<OnlineRefund>().ToTable("OnlineRefund");
            modelBuilder.Entity<ExamSuperintendent>().ToTable("Exam_Sup_Data");
            modelBuilder.Entity<ReceivedSms>().ToTable("Received_Sms");
            modelBuilder.Entity<DeficiencyCode>().ToTable("Deficiency_Code");
            modelBuilder.Entity<BatchItemDeficiencyDetail>().ToTable("Batch_Item_Deficiency_Detail");
            modelBuilder.Entity<NoticeEvent>().ToTable("Notice_Event");
            modelBuilder.Entity<ExamCentreChoiceRefilling>().ToTable("New_ExamCentre_Choice");
            //Added
            modelBuilder.Entity<tempExamResultPub>().ToTable("tempExamResultPub");
	   
	    modelBuilder.Entity<cityType>().ToTable("cityTypeMas");
            modelBuilder.Entity<perfoCriteriaMas>().ToTable("perfoCriteria");
            //Added 13 Feb 2019
            modelBuilder.Entity<perfoReport>().ToTable("perfoReport");
			//Added 17 Feb 2020
			
			modelBuilder.Entity<perfoCriteriaMasNewSOP>().ToTable("perfoCriteriaNewSOP");

            //Added 5 Sep 2020
            modelBuilder.Entity<InstituteWithdrawal>().ToTable("instituteWithdrawalUploads");
            //Added 19 May 2020
            modelBuilder.Entity<AccreditationDetailHistory>().ToTable("Intitute_Accreditation_Detail_history");

            //composite Primary Key in ExamWiseExamCenter
            modelBuilder.Entity<ExamWiseExamCenter>().HasKey(p => new { p.ExamID, p.ExamCenterID });

            //composite Primary Key in TimeTablePattern
            modelBuilder.Entity<TimeTablePattern>().HasKey(p => new { p.CourseID, p.RevisionNumber, p.ModuleID });

            modelBuilder.Entity<CSCTransaction>().ToTable("CSC_Transaction");
            //modelBuilder.Entity<CSCTransaction>().HasRequired(o => o.DemandNote).WithOptional(d => d.CSCTransaction);
            modelBuilder.Entity<PaymentStatus>().ToTable("Payment_Status");
            modelBuilder.Entity<DemandDraftTransaction>().ToTable("DemandDraftTransaction");
            //modelBuilder.Entity<DemandDraftTransaction>().HasRequired(o => o.DemandNote).WithOptional(d => d.DemandDraftTransaction);
            modelBuilder.Entity<OnlineTransaction>().ToTable("Online_Transaction");
            //modelBuilder.Entity<OnlineTransaction>().HasRequired(o => o.DemandNote).WithOptional(d => d.OnlineTransaction);
            modelBuilder.Entity<DemandNote>().ToTable("Demand_Note");
            modelBuilder.Entity<ExaminationCycle>().ToTable("Exam_Cycle");
            modelBuilder.Entity<Exam>().ToTable("Exam");
            modelBuilder.Entity<QualificationEligibility>().ToTable("Qualification_Eligibility");
            modelBuilder.Entity<QualificationLevel>().ToTable("Qualification_Level");
            modelBuilder.Entity<RegistrationDetail>().ToTable("Registration_Detail");
            modelBuilder.Entity<RegistrationStatus>().ToTable("Registration_Status");
            modelBuilder.Entity<CSCCharges>().ToTable("CSC_Charges");
            modelBuilder.Entity<NEFTTransaction>().ToTable("NEFT_Transaction");
            modelBuilder.Entity<NEFTBankTransaction>().ToTable("Neft_Bank_Transaction");
            modelBuilder.Entity<NEFTRefund>().ToTable("NEFT_Refund");
            modelBuilder.Entity<Online_ChargeBackTransaction>().ToTable("Online_Charge_Back_Transaction");
            modelBuilder.Entity<CandidateScholarship>().ToTable("Candidate_Scholarship");
            modelBuilder.Entity<IncomeCategory>().ToTable("Income_Category");

            //modelBuilder.Entity<Candidate>().HasOptional(c => c.ContactDetail).WithRequired(p => p.Candidate);
            modelBuilder.Entity<CandidateContactDetail>().ToTable("Candidate_Contact");
            modelBuilder.Entity<CandidateContactHistory>().ToTable("Candidate_Contact_History");
            modelBuilder.Entity<CandidateHistory>().ToTable("Candidate_History");
            modelBuilder.Entity<CandidateQualificationDetail>().ToTable("Candidate_Qualification");
            modelBuilder.Entity<CertificateExamApplication>().ToTable("Certificate_Exam_Application");

            modelBuilder.Entity<CertificateExamApplicationHistory>().ToTable("Certificate_Exam_Application_History");

            modelBuilder.Entity<CourseRegistrationApplication>().ToTable("Course_Registration_Application");
            modelBuilder.Entity<UploadedFile>().ToTable("Uploaded_File");
            modelBuilder.Entity<UpdateFields>().ToTable("Updatable_Fields");
            modelBuilder.Entity<Remarks>().ToTable("Remarks");
            //Added jksah
            modelBuilder.Entity<tblGender>().ToTable("tblGender");


            //EConnect.URM
            modelBuilder.Entity<MenuObject>().ToTable("URM_MENU_OBJECT");
            modelBuilder.Entity<ExternalEntity>().ToTable("URM_EXTERNAL_ENTITY");
            modelBuilder.Entity<LoginLog>().ToTable("URM_LOGIN_LOG");
            modelBuilder.Entity<MenuObjectType>().ToTable("URM_MENU_OBJECT_TYPE");
            modelBuilder.Entity<User>().ToTable("URM_USER_MASTER");
            modelBuilder.Entity<UsersType>().ToTable("URM_USER_TYPE");
            modelBuilder.Entity<Role>().ToTable("URM_Role");
            modelBuilder.Entity<Right>().ToTable("URM_Right");
            modelBuilder.Entity<UserRole>().ToTable("Urm_User_Role");
            modelBuilder.Entity<UserLoginOtpDetail>().ToTable("UserLoginOtpDetail");

            modelBuilder.Entity<CourseWiseUserMapping>().ToTable("Course_Wise_User_Mapping");
            modelBuilder.Entity<CourseWiseUserMapping>().HasKey(p => new { p.CourseID, p.CourseCategoryID, p.UserNo });


            modelBuilder.Entity<CourseWiseOccupationMapping>().ToTable("CourseWiseOccupation");
            modelBuilder.Entity<CourseWiseOccupationMapping>().HasKey(p => new { p.CourseID, p.CourseCategoryID, p.OccupationID });

            //EConnect.HRMS
            modelBuilder.Entity<Employee>().ToTable("EMPLOYEE");
            modelBuilder.Entity<Organization>().ToTable("ORGANIZATION");

            modelBuilder.Entity<RegnExamCourseBridge>().ToTable("Regn-Cum-Exam-Course-Bridge");

            modelBuilder.Entity<CertificateOABCPhoto>().ToTable("Certificate_OABC_Photo");
			
			 modelBuilder.Entity<ExamCentreChoiceStat>().ToTable("Exam_Centre_Choice_Stat");
            modelBuilder.Entity<ExamCentreAvailableStat>().ToTable("Exam_Centre_Available_Stat");
            modelBuilder.Entity<ExamCentreExamDate>().ToTable("Exam_Centre_Exam_Date");

            modelBuilder.Entity<ModuleCertificateBatch>().ToTable("ModuleCertificateBatch");
            modelBuilder.Entity<ModuleCertificateRequest>().ToTable("ModuleCertificateRequest");

            modelBuilder.Entity<AccCourseCompletionDate>().ToTable("Acc_Course_Completion_Dates");
            modelBuilder.Entity<DisabilityType>().ToTable("Disability_Type");
            modelBuilder.Entity<RegnICardCandidatePhotos>().ToTable("Regn_I_Card_Candidate_Photos");

			//Added 15 june 2020 for NSQF Download images
			modelBuilder.Entity<CourseMappingWithNSQFCoursecode>().ToTable("CourseMappingWithNSQFCoursecode");
				modelBuilder.Entity<NSQFCertPhasePrintDetail>().ToTable("NSQF_Cert_Phase_Print_Detail");
                modelBuilder.Entity<NSQFModuleCandidateMarks>().ToTable("NSQF_Module_Candidate_marks");
                modelBuilder.Entity<NSQFModuleCandidateMarks>().HasKey(p => new { p.RegistrationNo, p.CandidateId, p.CourseId, p.AttemptNoForModule, p.ModuleId, p.ExamMonth, p.ExamYear });
            modelBuilder.Entity<CourseLevelDurations>().ToTable("CourseLevelDuration");

                modelBuilder.Entity<PuraskarApplicationForm>().ToTable("OnlinePuraskarApplication");

                modelBuilder.Entity<CourseWiseAmtPerPaperForOnlinePuraskarApp>().ToTable("CourseWiseAmtPerPaperForOnlinePuraskarApp");
                modelBuilder.Entity<OnlineProtsahanExamModules>().ToTable("onlineProtsahanExamModules");

                modelBuilder.Entity<PuraskarAppMobileEmailOtpVerificationDetail>().ToTable("PuraskarAppMobileEmailOtpVerificationDetail");
                modelBuilder.Entity<ProjectEligibility>().ToTable("tblProjectEligibility"); modelBuilder.Entity<PuraskarAppDocsVerificationAndDeclarationByInstt>().ToTable("PuraskarAppDocsVerificationAndDeclarationByInstt");
                modelBuilder.Entity<PuraskarFeeReimbursementMaster>().ToTable("PuraskarFeeReimbursementMaster");
                modelBuilder.Entity<PuraskarFeeReimbursementInstalment>().ToTable("PuraskarFeeReimbursementInstalment");
//Added 2 Feb 2023 Practical Changes
 modelBuilder.Entity<PracticalCandidateMarks>().ToTable("Practical_candidate_marks");
                modelBuilder.Entity<PracAllowedMarksEntry>().ToTable("Prac_Allowed_Marks_Entry");
            //Added for Apaar Validation 22 Sep 2024
                modelBuilder.Entity<apaarResponseRecd>().ToTable("apaarResponseRecd");
            //Added for CHM(T) Change 13 June 2024
                modelBuilder.Entity<CHMTProjectManualEntry>().ToTable("CHMT_Project_Manual_Entry");

                #region[Added by Reena(30/04/2023)]
                modelBuilder.Entity<PracLocationType>().ToTable("Prac_LocationType");
                modelBuilder.Entity<PracLocation>().ToTable("Prac_Location");
                modelBuilder.Entity<PracExamCenter>().ToTable("Prac_Exam_Center");

                modelBuilder.Entity<OnlineLocationType>().ToTable("Online_LocationType");
                modelBuilder.Entity<OnlineLocation>().ToTable("Online_Location");
                modelBuilder.Entity<OnlineExamCenter>().ToTable("Online_Exam_Center");
                modelBuilder.Entity<aadhaarResponse>().ToTable("aadhaarResponse");
                //Added_Master_Certificate_Course_Type_28_10_2024
                modelBuilder.Entity<CertificateCourseType>().ToTable("certCourseType");
               
    // Added by Komal
            modelBuilder.Entity<MercyCase_UploadedDocs>().ToTable("MercyCase_UploadedDocs");

            modelBuilder.Entity<MercyDocumentStatus>().ToTable("MercyDocumentStatus");
            // End


                #endregion  
                // Added by  Amit Start
                modelBuilder.Entity<projCriteriaMaster>().ToTable("projCriteriaMaster");
                modelBuilder.Entity<projStudentMaster>().ToTable("projStudentMaster");
            // Added by  Amit End 
            modelBuilder.Entity<PaymentGateways>().ToTable("PaymentGateways");
            modelBuilder.Entity<PGErrorCodes>().ToTable("PGErrorCodes");
            modelBuilder.Entity<BankHashMismatch>().ToTable("BankHashMismatch");
            // added by amit start
            modelBuilder.Entity<projEmailMobExempt>().ToTable("projEmailMobExempt");
            // added by amit end
            modelBuilder.Entity<ApaarRequest>().ToTable("ApaarRequest");
            modelBuilder.Entity<AuthMode>().ToTable("AuthModes");

            //Added for AadhaarEncryption
            /* modelBuilder.Entity<CourseENCAaddhar>().ToTable("Course_ENC_Addh");
              modelBuilder.Entity<CertificateENCAddh>().ToTable("Certificate_ENC_Addh");*/


            // modelBuilder.Entity <regdIcard>().ToTable ("Regn_I_Card_Candidate_Photos");
            //Mapping of Parent Child record in Menu object table
            modelBuilder.Entity<MenuObject>()
                .HasMany(c => c.ChildMenuObjects)
                .WithOptional(p => p.ParentMenuObject);
        }

    }

}