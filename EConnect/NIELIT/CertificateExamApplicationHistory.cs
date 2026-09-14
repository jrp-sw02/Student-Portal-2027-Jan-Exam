using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class CertificateExamApplicationHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Certificate_Exam_Application_ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Date")]
        [Required]
        public DateTime ApplicationDate { get; set; }

        [Column("Number")]
        [MaxLength(30)]
        public String Number { get; set; }

        [Column("Candidate_ID", TypeName = "bigint")]
        public Int64? CandidateID { get; set; }

        [Column("Already_Applied", TypeName = "bit")]
        public Boolean AlreadyApplied { get; set; }

        [Column("Previous_Exam_ID", TypeName = "int")]
        public Int32? PreviousExamID { get; set; }

        [Column("Previous_Roll_Number")]
        [MaxLength(15)]
        public String PreviousRollNumber { get; set; }

        [Column("Previous_Application_ID", TypeName = "bigint")]
        public Int64? PreviousApplicationID { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        public Int32 CourseID { get; set; }

        [Column("Exam_ID", TypeName = "int")]
        public Int32 ExamID { get; set; }

        [Column("Applicant_Type_ID", TypeName = "int")]
        public Int32 ApplicantTypeID { get; set; }

        [Column("Institute_ID", TypeName = "bigint")]
        public Int64? InstituteID { get; set; }

        [Column("Exam_Center1_ID", TypeName = "int")]
        public Int32 ExamCenter1ID { get; set; }

        [Column("Exam_Center2_ID", TypeName = "int")]
        public Int32 ExamCenter2ID { get; set; }

        [Column("Regional_Center_ID", TypeName = "int")]
        public Int32? RegionalCenterID { get; set; }


        //Personal Details 
        [Column("Salutaion")]
        [Required(ErrorMessage = "Salutaion is required")]
        [MaxLength(10)]
        [Display(Name = "Salutaion")]
        public String Salutation { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Candidate Name is required")]
        [MaxLength(60)]
        [Display(Name = "Candidate Name")]
        public String Name { get; set; }

        [Column("Father_Name")]
        [MaxLength(60)]        
        [Display(Name = "Father's Name Name")]
        public String FatherName { get; set; }

        [Column("Mother_Name")]
        [MaxLength(60)]       
        [Display(Name = "Mother's Name Name")]
        public String MotherName { get; set; }

        [Column("Guardian_Name")]
        [MaxLength(60)]
        public String GuardianName { get; set; }

        [Column("Gender")]
        [Required(ErrorMessage = "Gender is required")]
        [MaxLength(6)]
        [Display(Name = "Gender")]
        public String Gender { get; set; }

        [Column("Dob")]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Column("Cast_Category_ID", TypeName = "int")]
        [Display(Name = "Cast Category")]
        public Int32 CastCategoryID { get; set; }

        [Column("Is_Disability", TypeName = "bit")]
        [Display(Name = "Is Handicapped")]
        public Boolean? IsDisability { get; set; }

        [Column("Disability_Type_ID", TypeName = "int")]
        public Int32? DisabilityTypeID { get; set; }

        [Column("Disability_Percentage", TypeName = "int")]
        public Int32? DisabilityPercentage { get; set; }

        [Column("Occupation_ID", TypeName = "int")]
        [Display(Name = "Occupation")]
        public Int32 OccupationID { get; set; }

        

        [Column("Photo_File_Name")]
        [MaxLength(50)]
        public String PhotoFileName { get; set; }

        [Column("Photo", TypeName = "varbinary(Max)")]
        public Byte[] Photo { get; set; }

        [Column("Signature_File_Name")]
        [MaxLength(50)]
        public String SignatureFileName { get; set; }

        [Column("Signature", TypeName = "varbinary(Max)")]
        public Byte[] Signature { get; set; }

        [Column("Left_Thumb_File_Name")]
        [MaxLength(50)]
        public String LeftThumbFileName { get; set; }

        [Column("Left_Thumb", TypeName = "varbinary(Max)")]
        public Byte[] LeftThumb { get; set; }

        public virtual CastCategory CastCategory { get; set; }
        public virtual Occupation Occupation { get; set; }


        //Correspondence Address Details
        [Column("Cor_Address1")]
        [Required(ErrorMessage = "Address Line1 required")]
        [MaxLength(100)]
        [Display(Name = "Address Line1")]
        public String CorAddressLine1 { get; set; }

        [Column("Cor_Address2")]
        [Required(ErrorMessage = "Address Line2 required")]
        [MaxLength(100)]
        [Display(Name = "Address Line2")]
        public String CorAddressLine2 { get; set; }

        [Column("Cor_Address3")]
        [MaxLength(100)]
        [Display(Name = "Address Line3")]
        public String CorAddressLine3 { get; set; }

        [Column("Cor_State_ID", TypeName = "bigint")]
        [Required(ErrorMessage = "State required")]
        [Display(Name = "State")]
        public Int64 CorStateID { get; set; }

        [Column("Cor_District_ID", TypeName = "bigint")]
        [Display(Name = "District")]
        public Int64? CorDistrictID { get; set; }

        [Column("Cor_City_Name")]
        [MaxLength(50)]
        [Display(Name = "City Name")]
        public String CorCityName { get; set; }

        [Column("Cor_Pin_Code", TypeName = "int")]
        [Required(ErrorMessage = "Pin Code required")]
        [Display(Name = "Pin Code")]
        public Int32 CorPinCode { get; set; }

        [ForeignKey("CorStateID")]
        public virtual Location CorState { get; set; }

        [ForeignKey("CorDistrictID")]
        public virtual Location CorDistrict { get; set; }

        //Qualification Detail

        [Column("Educational_Qualification_ID", TypeName = "int")]
        public Int32 EducationalQualificationID { get; set; }

        [Column("Passing_Year", TypeName = "int")]
        public Int32? PassingYear { get; set; }

        public virtual EducationalQualification EducationalQualification { get; set; }


        //Contact Details
        [Column("Mobile", TypeName = "bigint")]
        public Int64 MobileNumber { get; set; }

        [Column("Std", TypeName = "int")]
        public Int32? StdNumber { get; set; }

        [Column("Phone", TypeName = "int")]
        public Int32? PhoneNumber { get; set; }

        [Column("Email")]
        [MaxLength(100)]
        public String EmailAddress { get; set; }

        [Column("Email_sent_flag")]
        public Boolean? EmailSent { get; set; }

        [Column("SMS_sent_flag")]
        public Boolean? SMSSent { get; set; }

        //Correspondence Address Details
        [Column("Final_Submitted", TypeName = "bit")]
        public Boolean FinalSubmitted { get; set; }

        [Column("Final_Submission_Date")]
        public DateTime? FinalSubmissionDate { get; set; }

        [Column("Demand_Note_ID", TypeName = "bigint")]
        public Int64? DemandNoteID { get; set; }

        [Column("Course_Duration_From")]
        public String CourseDurationFrom { get; set; }

        [Column("Course_Duration_To")]
        public String CourseDurationTo { get; set; }

        [Column("Fee_Type_ID", TypeName = "int")]
        public Int32? FeeTypeID { get; set; }

        [Column("Fee_Amt", TypeName = "decimal")]
        public Decimal? FeeAmount { get; set; }

        [Column("Late_Fee_Amt", TypeName = "decimal")]
        public Decimal? LateFeeAmount { get; set; }

        [Column("Total_Fee_Amt", TypeName = "decimal")]
        public Decimal? TotalFeeAmount { get; set; }

        [Column("Is_Verified_By_Institute", TypeName = "bit")]
        public Boolean IsVerifiedByInstitute { get; set; }

        [Column("Verified_On_By_Institute")]
        public DateTime? DateOfVerificationByInstitute { get; set; }

        [Column("Payment_Status_ID", TypeName = "int")]
        public Int32 PaymentStatusID { get; set; }

        [Column("Application_Status_ID", TypeName = "int")]
        public Int32 ApplicationStatusID { get; set; }

        [Column("Batch_Item_ID", TypeName = "bigint")]
        public Int64? BatchItemID { get; set; }

        [Column("Roll_Number")]
        [MaxLength(15)]
        public String RollNumber { get; set; }

        [Column("Exam_Centre_Name")]
        public String ExamCentreName { get; set; }

        [Column("Exam_Centre_Address")]
        public String ExamCentreAddress { get; set; }

        [Column("Date_of_Exam")]
        public DateTime? DateOfExam { get; set; }

        [Column("Exam_Batch_Number")]
        public String ExamBatchNumber { get; set; }

        [Column("Reporting_Time")]
        public String ReportingTime { get; set; }

        [Column("Updated_On")]
        public DateTime? UpdatedOn { get; set; }

        [Column("Updated_By", TypeName = "int")]
        public Int32? UpdatedByID { get; set; }

        [Column("Result_Grade_ID", TypeName = "int")]
        public Int32? ResultGradeID { get; set; }

        [ForeignKey("ResultGradeID")]
        public virtual ResultGrade ResultGrade { get; set; }

        [Column("Result_Updated_On")]
        public DateTime? ResultUpdatedOn { get; set; }

        [Column("Result_Updated_By", TypeName = "int")]
        public Int32? ResultUpdatedBy { get; set; }

        [Column("Exempted_Application_ID", TypeName = "bigint")]
        public Int64? ExemptedApplicationID { get; set; }

        [Column("Is_Exempted", TypeName = "bit")]
        public Boolean IsExempted { get; set; }

        [Column("Aadhar_Number", TypeName = "bigint")]
        public Int64? AadharNumber { get; set; }

        [Column("Aadhar_Verfied", TypeName = "bit")]
        public Boolean AadharVerfied { get; set; }

        [Column("Department")]
        [MaxLength(100)]
        public String Department { get; set; }

        [Column("Employee_Code")]
        [MaxLength(100)]
        public String EmployeeCode { get; set; }

        [Column("Designation")]
        [MaxLength(100)]
        public String Designation { get; set; }

        [Column("Posting_City")]
        [MaxLength(100)]
        public String PostingCity { get; set; }

        [Column("Date_of_Joining")]
        public DateTime? DateofJoining { get; set; }

        [Column("Date_of_Retirement")]
        public DateTime? DateofRetirement { get; set; }

        public virtual Candidate Candidate { get; set; }

        [ForeignKey("PreviousApplicationID")]
        public virtual CertificateExamApplication PreviousExamApplication { get; set; }

        public virtual CourseCategory CourseCategory { get; set; }

        public virtual Course Course { get; set; }

        public virtual Exam Exam { get; set; }

        [ForeignKey("FeeTypeID")]
        public virtual FeeType FeeType { get; set; }

        [ForeignKey("PreviousExamID")]
        public virtual Exam PreviousExam { get; set; }

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
        [NotMapped]
        public enmCertificateExamApplicationStatus enmCertificateExamApplicationStatus
        {
            get
            {
                return (enmCertificateExamApplicationStatus)this.ApplicationStatusID;
            }
            set
            {
                this.ApplicationStatusID = (int)value;
            }
        }

        [ForeignKey("DemandNoteID")]
        public virtual DemandNote DemandNote { get; set; }

        [ForeignKey("PaymentStatusID")]
        public virtual PaymentStatus PaymentStatus { get; set; }

        public virtual Institute Institute { get; set; }

        public virtual ExamCenter ExamCenter1 { get; set; }

        public virtual ExamCenter ExamCenter2 { get; set; }

        [ForeignKey("RegionalCenterID")]
        public virtual RegionalCenter RegionalCenter { get; set; }

        [Column("App_Source", TypeName = "int")]
        public Int32 ApplicationSourceID { get; set; }

        [Column("Is_Synced", TypeName = "bit")]
        [Display(Name = "Is Synced")]
        public Boolean IsSynced { get; set; }

        [Column("Synced_On")]
        public DateTime? SyncedOn { get; set; }

        [Column("Is_Downloaded", TypeName = "bit")]
        [Display(Name = "Is Downloaded")]
        public Boolean IsDownloaded { get; set; }

        [Column("Downloaded_On")]
        public DateTime? DownloadedOn { get; set; }

        [Column("UID_Type", TypeName = "int")]
        public Int32? UIDType { get; set; }

        [Column("UID_Number")]
        [MaxLength(50)]
        public String UIDNumber { get; set; }

        //jk sah on 14-1-2021
        [Column("Is_EWS", TypeName = "bit")]
        [Display(Name = "Is EWS")]
        public Boolean Is_EWS { get; set; }

        [Column("OnlineRefID")]
        [MaxLength(30)]
        public String OnlineRefID { get; set; }

        [NotMapped]
        public enmApplicationSource enmApplicationSource
        {
            get
            {
                return (enmApplicationSource)this.ApplicationSourceID;
            }
            set
            {
                this.ApplicationSourceID = (int)value;
            }
        }

        public virtual DisabilityType DisabilityType { get; set; }
    }
}
