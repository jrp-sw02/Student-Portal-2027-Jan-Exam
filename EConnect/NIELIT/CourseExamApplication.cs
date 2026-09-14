using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CourseExamApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Application_Date")]
        [Required]
        public DateTime ApplicationDate { get; set; }

        [Column("Appl_Number")]
        public String Number { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        public Int32 CourseCategoryID { get; set; }

        public virtual CourseCategory CourseCategory { get; set; }

        [Column("Course_ID", TypeName = "int")]
        public Int32 CourseID { get; set; }
        
        public virtual Course Course { get; set; }

        [Column("Candidate_ID", TypeName = "bigint")]
        public Int64 CandidateID { get; set; }

        public virtual Candidate Candidate { get; set; }

        [Column("Previous_Exam_ID", TypeName = "int")]
        public Int32? PreviousExamID { get; set; }

        [ForeignKey("PreviousExamID")]
        public virtual Exam PreviousExam { get; set; }

        [Column("Exam_ID", TypeName = "int")]
        public Int32 ExamID { get; set; }

        [ForeignKey("ExamID")]
        public virtual Exam Exam { get; set; }

        [Column("Registration_Number", TypeName = "bigint")]
        public Int64 RegistrationNumber { get; set; }

        [Column("Medium_of_Exam", TypeName = "int")]
        public Int32 MediumOfExamID { get; set; }

        [ForeignKey("MediumOfExamID")]
        public virtual Language  MediumOfExam { get; set; }

        [NotMapped]
        public enmLanguage enmMediumOfExam
        {
            get
            {
                return (enmLanguage)this.MediumOfExamID;
            }
            set
            {
                this.MediumOfExamID = (int)value;
            }
        }

        [Column("Is_Improvement", TypeName = "bit")]
        public Boolean IsImprovementApplication { get; set; }

        [Column("Applicant_Type_ID", TypeName = "int")]
        public Int32 ApplicantTypeID { get; set; }

        public virtual ApplicantType ApplicantType { get; set; }

        [NotMapped]
        public enmApplicantType enmApplicantType
        {
            get
            {
                return (enmApplicantType)this.ApplicantTypeID;
            }
            set
            {
                this.ApplicantTypeID = (int)value;
            }
        }

        [Column("Institute_ID", TypeName = "bigint")]
        public Int64? InstituteID { get; set; }

        public virtual Institute Institute { get; set; }

        [Column("Exam_Center1_ID", TypeName = "int")]
        public Int32? ExamCenter1ID { get; set; }

        [ForeignKey("ExamCenter1ID")]
        public virtual ExamCenter ExamCenter1 { get; set; }

        [Column("Exam_Center2_ID", TypeName = "int")]
        public Int32? ExamCenter2ID { get; set; }

        [ForeignKey("ExamCenter2ID")]
        public virtual ExamCenter ExamCenter2 { get; set; }

        //Added by Reena
        [Column("Prac_Exam_Center1_ID", TypeName = "int")]
        public Int32? PracExamCenter1ID { get; set; }

        [ForeignKey("PracExamCenter1ID")]
        public virtual PracExamCenter PracExamCenter1 { get; set; }

        [Column("Prac_Exam_Center2_ID", TypeName = "int")]
        public Int32? PracExamCenter2ID { get; set; }

        [ForeignKey("PracExamCenter2ID")]
        public virtual PracExamCenter PracExamCenter2 { get; set; }

        //
        [Column("Online_Exam_Center1_ID", TypeName = "int")]
        public Int32? OnlineExamCenter1ID { get; set; }

        [ForeignKey("OnlineExamCenter1ID")]
        public virtual OnlineExamCenter OnlineExamCenter1 { get; set; }

        [Column("Online_Exam_Center2_ID", TypeName = "int")]
        public Int32? OnlineExamCenter2ID { get; set; }

        [ForeignKey("OnlineExamCenter2ID")]
        public virtual OnlineExamCenter OnlineExamCenter2 { get; set; }
        
        //End


        [Column("Late_Fee_Imposed", TypeName = "bit")]
        public Boolean LateFeeImposed { get; set; }

        [Column("Late_Fee_Amount", TypeName = "decimal")]
        public Decimal? LateFeeAmount { get; set; }

        [Column("Qualified_Previous_Level", TypeName = "bit")]
        public Boolean PreviousLevelQualified { get; set; }

        [Column("Project_Submission_Date")]
        public DateTime? DateOfProjectSubmissionOfPreviousLevel { get; set; }

        [Column("Fee_Type_ID", TypeName = "int")]
        public Int32? FeeTypeID { get; set; }

        [ForeignKey("FeeTypeID")]
        public virtual FeeType FeeType { get; set; }

        [Column("Fee_Amount", TypeName = "decimal")]
        public Decimal FeeAmount { get; set; }

        [Column("Number_of_th_Modules", TypeName = "int")]
        public Int32 NumberOfTheoryModulesApplied { get; set; }

        [Column("Number_of_Pr_Modules", TypeName = "int")]
        public Int32 NumberOfPracticalModulesApplied { get; set; }

        [Column("Final_Submitted", TypeName = "bit")]
        public Boolean FinalSubmitted { get; set; }

        [Column("Final_Submission_Date")]
        public DateTime? FinalSubmissionDate { get; set; }

        [Column("Is_Verified_By_Institute", TypeName = "bit")]
        public Boolean IsVerifiedByInstitute { get; set; }

        [Column("Verified_On_By_Institute")]
        public DateTime? DateOfVerificationByInstitute { get; set; }

        [Column("Course_Duration_From")]
        public String CourseDurationFrom { get; set; }

        [Column("Course_Duration_To")]
        public String CourseDurationTo { get; set; }

        [Column("Application_Status_ID", TypeName = "int")]
        public Int32 ApplicationStatusID { get; set; }

        public virtual ApplicationStatus ApplicationStatus { get; set; }

        [NotMapped]
        public enmCourseExamApplicationStatus enmCourseExamApplicationStatus
        {
            get
            {
                return (enmCourseExamApplicationStatus)this.ApplicationStatusID;
            }
            set
            {
                this.ApplicationStatusID = (int)value;
            }
        }

        [Column("Payment_Status_ID", TypeName = "int")]
        public Int32 PaymentStatusID { get; set; }

        [ForeignKey("PaymentStatusID")]
        public virtual PaymentStatus PaymentStatus { get; set; }

        [NotMapped]
        public enmPaymentStatus enmPaymentStatus
        {
            get
            {
                return (enmPaymentStatus)this.PaymentStatusID;
            }
            set
            {
                this.PaymentStatusID = (int)value;
            }
        }

        [Column("Batch_Item_ID", TypeName = "bigint")]
        public Int64? BatchItemID { get; set; }

        [Column("Demand_Note_ID", TypeName = "bigint")]
        public Int64? DemandNoteID { get; set; }

        [ForeignKey("DemandNoteID")]
        public virtual DemandNote DemandNote { get; set; }

        [Column("Roll_Number", TypeName = "bigint")]
        public Int64? RollNumber { get; set; }

        [Column("Allotted_Exam_State_ID", TypeName = "bigint")]
        public Int64? AllottedExamStateID { get; set; }

        [Column("Allotted_Exam_Centre_ID", TypeName = "int")]
        public Int32? AllottedExamCentreID { get; set; }

        [ForeignKey("AllottedExamCentreID")]
        public virtual ExamCenter AllottedExamCentre { get; set; }

        [Column("Allotted_Exam_Venue_ID", TypeName = "int")]
        public Int32? AllottedExamVenueID { get; set; }

        [Column("Allotted_Practical_State_ID", TypeName = "bigint")]
        public Int64? AllottedPracticalStateID { get; set; }

        [Column("Allotted_Practical_Centre_ID", TypeName = "int")]
        public Int32? AllottedPracticalCentreID { get; set; }

        [ForeignKey("AllottedPracticalCentreID")]
        public virtual ExamCenter AllottedPracticalCentre { get; set; }

        [Column("Allotted_Practical_Venue_ID", TypeName = "int")]
        public Int32? AllottedPracticalVenueID { get; set; }

        [Column("Payment_Source", TypeName = "int")]
        public Int32 PaymentSourceID { get; set; }

        [Column("Is_Synced", TypeName = "bit")]
        [Display(Name = "Is Synced")]
        public Boolean IsSynced { get; set; }

        [Column("Synced_On")]
        public DateTime? SyncedOn { get; set; }

        [NotMapped]
        public enmPaymentSource enmPaymentSource
        {
            get
            {
                return (enmPaymentSource)this.PaymentSourceID;
            }
            set
            {
                this.PaymentSourceID = (int)value;
            }
        }

        [Column("Payment_Source_Changed", TypeName = "bit")]
        public Boolean PaymentSourceChanged { get; set; }

        [Column("Payment_Source_Changed_On")]
        public DateTime? PaymentSourceChangedOn { get; set; }

        [Column("Payment_Source_Changed_By", TypeName = "int")]
        public Int32? PaymentSourceChangedBy { get; set; }

        [Column("Applicant_Type_Changed", TypeName = "bit")]
        public Boolean ApplicantTypeChanged { get; set; }

        [Column("Applicant_Type_Changed_On")]
        public DateTime? ApplicantTypeChangedOn { get; set; }

        [Column("Applicant_Type_Changed_By", TypeName = "int")]
        public Int32? ApplicantTypeChangedBy { get; set; }

        [Column("Is_Exported", TypeName = "bit")]
        public Boolean IsExported { get; set; }

        [Column("Exported_On")]
        public DateTime? ExportedOn { get; set; }

        [Column("Exported_By", TypeName = "int")]
        public Int32? ExportedByID { get; set; }

        [Column("Admit_Card_Changed_On")]
        public DateTime? AdmitCardChangedOn { get; set; }

        [Column("Admit_Card_Changed_By", TypeName = "int")]
        public Int32? AdmitCardChangedBy { get; set; }

        [Column("Pr_Office_Ref_No")]
        public String PracticalOfficeRefNumber { get; set; }

        [Column("Pr_Institute_Name")]
        public String PracticalInstituteName { get; set; }

        [Column("Pr_Institute_Address")]
        public String PracticalInstituteAddress { get; set; }

        [Column("Form_Forwarded_By_Institute_On")]
        public DateTime? FormForwardedByInstituteOn { get; set; }

        [ForeignKey("ExportedByID")]
        public virtual EConnect.URM.User ExportedByUser { get; set; }

        [ForeignKey("AdmitCardChangedBy")]
        public virtual EConnect.URM.User AdmitCardChangedByUser { get; set; }

        [Column("NSQF_Roll_no")]
        public String NSQFRollNo { get; set; }
    }
}
