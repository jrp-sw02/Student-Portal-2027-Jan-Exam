using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class CourseProjectApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Application_Date")]
        [Required]
        public DateTime ApplicationDate { get; set; }

        [Column("DemandNote_Validity_Date_Upto")]
        public DateTime? DemandNoteValiditydateupto { get; set; }

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

        [Column("Registration_Number", TypeName = "bigint")]
        public Int64 RegistrationNumber { get; set; }  

        [Column("Is_Resubmission", TypeName = "bit")]
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

        [Column("Number_of_Project_Modules", TypeName = "int")]
        public Int32 NumberOfProjectModulesApplied { get; set; }       

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

        [Column("Whether_DemandNote_Canceled", TypeName = "bit")]
        public Boolean WhetherDemandNoteCanceled { get; set; }

        //Added 13 May 2024 for CHM(T)-O Leevel Project
        [Column("Commencement_exam_id", TypeName = "int")]
        public Int32? ExamID { get; set; }

        [ForeignKey("ExamID")]
        public virtual Exam Exam { get; set; }
        //
        [Column("Project_Receipt_Entered_Date")]
        public DateTime? ProjectReceiptEnteredDate { get; set; }
        //

    }
}
