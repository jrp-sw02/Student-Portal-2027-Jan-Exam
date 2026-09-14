using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CourseRegistrationApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Date")]
        [Required]
        public DateTime ApplicationDate { get; set; }

        [Column("Number")]
        [MaxLength(30)]
        public String Number { get; set; }

        [Column("Registration_Type_ID", TypeName = "int")]
        public Int32? RegistrationTypeID { get; set; }

        [Column("Exam_ID", TypeName = "int")]
        public Int32? ApplicableExamID { get; set; }

        [ForeignKey("RegistrationTypeID")]
        public virtual RegistrationType RegistrationType { get; set; }

        [ForeignKey("ApplicableExamID")]
        public virtual Exam ApplicableExam { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        public Int32 CourseID { get; set; }

        [Column("Already_Registered", TypeName = "bit")]
        public Boolean AlreadyRegistered { get; set; }

        [Column("Registered_Course_ID", TypeName = "int")]
        public Int32? RegisteredCourseID { get; set; }

        [Column("Registered_Course_Registration_No", TypeName = "int")]
        public Int32? RegisteredCourseRegistrationNo { get; set; }

        [Column("Applicant_Type_ID", TypeName = "int")]
        public Int32 ApplicantTypeID { get; set; }

        [Column("Experience_In_Years", TypeName = "numeric")]
        public Decimal? ExperienceInYears { get; set; }

        [Column("Institute_ID", TypeName = "bigint")]
        public Int64? InstituteID { get; set; }

        [Column("Already_Qualified", TypeName = "bit")]
        public Boolean AlreadyQualified { get; set; }

        [Column("Qualified_Course_ID", TypeName = "int")]
        public Int32? QualifiedCourseID { get; set; }

        [Column("Qualified_Course_Registration_No", TypeName = "int")]
        public Int32? QualifiedCourseRegistrationNo { get; set; }

        [Column("Qualified_Course_Passing_Year", TypeName = "int")]
        public Int32? QualifiedCoursePassingYear { get; set; }

        //Personal Details 

        [Column("Candidate_ID", TypeName = "bigint")]
        public Int64? CandidateID { get; set; }

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

        [Column("Marital_Status_ID", TypeName = "int")]
        [Display(Name = "Marital Status")]
        public Int32 MaritalStatusID { get; set; }

        [Column("Dob")]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Column("Cast_Category_ID", TypeName = "int")]
        [Display(Name = "Cast Category")]
        public Int32 CastCategoryID { get; set; }

        [Column("Religion_ID", TypeName = "int")]
        [Display(Name = "Religion")]
        public Int32 ReligionID { get; set; }

        [Column("Is_Handicaped", TypeName = "bit")]
        [Display(Name = "Is Handicapped")]
        public Boolean IsHandicaped { get; set; }

        [Column("Is_Ex_Servicemane", TypeName = "bit")]
        [Display(Name = "Is Ex Servicemane")]
        public Boolean IsExServicemane { get; set; }

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

        [Column("Body_Mark")]
        [MaxLength(50)]
        [Display(Name = "Body Mark")]
        public String BodyMark { get; set; }

        ////DEEP AdD LINE OF CODE on 27 may 2018
        [Column("Registration_Process_Flag")]
        [MaxLength(1)]
        [Display(Name = "Registration_Process_Flag")]
        public String Registration_Process_Flag { get; set; }

        [Column("Registration_Process_Flag_N_DT")]
        public DateTime? Registration_Process_Flag_N_DT { get; set; }

        [Column("Registration_Process_Flag_Y_DT")]
        public DateTime? Registration_Process_Flag_Y_DT { get; set; }

        [Column("Registration_Process_Flag_C_DT")]
        public DateTime? Registration_Process_Flag_C_DT { get; set; }

        //jk sah on 14-1-2021
        [Column("Is_EWS", TypeName = "bit")]
        [Display(Name = "Is EWS")]
        public Boolean Is_EWS { get; set; }

        [Column("OnlineRefID")]
        [MaxLength(30)]
        public String OnlineRefID { get; set; }

	//Added for Aadhaar Encryption

	    [Column("u_encID", TypeName = "bigint")]
        public Int64? uencID { get; set; }

        [Column("a_encID", TypeName = "bigint")]
        public Int64? aencID { get; set; }
         
        [Column("isBSB")]
        public Boolean isBSB { get; set; }

        [Column("BSB_U_DISECode")]
        public String BSB_U_DISECode { get; set; }

        [Column("Apaar_ID")]
        public string apaarID { get; set; }

        ////DEEP end of Add LINE OF CODE on 27 may 2018

        [ForeignKey("RegisteredCourseID")]
        public virtual Course RegisteredCourse { get; set; }
        [ForeignKey("QualifiedCourseID")]
        public virtual Course QualifiedCourse { get; set; }
        public virtual ApplicantType ApplicantType { get; set; }
        public virtual Institute Institute { get; set; }
        public virtual CourseCategory CourseCategory { get; set; }
        public virtual Course Course { get; set; }
        public virtual MaritalStatus MaritalStatus { get; set; }
        public virtual CastCategory CastCategory { get; set; }
        public virtual Religion Religion { get; set; }
        public virtual Candidate Candidate { get; set; }

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
        public enmMaritalStatus enmMaritalStatus
        {
            get
            {
                return (enmMaritalStatus)this.MaritalStatusID;
            }
            set
            {
                this.MaritalStatusID = (int)value;
            }
        }
        [NotMapped]
        public enmCastCategory enmCastCategory
        {
            get
            {
                return (enmCastCategory)this.CastCategoryID;
            }
            set
            {
                this.MaritalStatusID = (int)value;
            }
        }

        //Qualification Detail

        [Column("Educational_Qualification_ID", TypeName = "int")]
        public Int32 EducationalQualificationID { get; set; }

        [Column("Passing_Year", TypeName = "int")]
        public Int32 PassingYear { get; set; }

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

        //Correspondence Address Detail

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
        //Permanent Address Details

        [Column("Per_Address1")]
        [Required(ErrorMessage = "Address Line1 required")]
        [MaxLength(100)]
        [Display(Name = "Address Line1")]
        public String PerAddressLine1 { get; set; }

        [Column("Per_Address2")]
        [Required(ErrorMessage = "Address Line2 required")]
        [MaxLength(100)]
        [Display(Name = "Address Line2")]
        public String PerAddressLine2 { get; set; }

        [Column("Per_Address3")]
        [MaxLength(100)]
        [Display(Name = "Address Line3")]
        public String PerAddressLine3 { get; set; }

        [Column("Per_State_ID", TypeName = "bigint")]
        [Required(ErrorMessage = "State required")]
        [Display(Name = "State")]
        public Int64 PerStateID { get; set; }

        [Column("Per_District_ID", TypeName = "bigint")]
        [Display(Name = "District")]
        public Int64? PerDistrictID { get; set; }

        [Column("Per_City_Name")]
        [MaxLength(50)]
        [Display(Name = "City Name")]
        public String PerCityName { get; set; }

        [Column("Per_Pin_Code", TypeName = "int")]
        [Required(ErrorMessage = "Pin Code required")]
        [Display(Name = "Pin Code")]
        public Int32 PerPinCode { get; set; }

        [ForeignKey("PerStateID")]
        public virtual Location PerState { get; set; }

        [ForeignKey("PerDistrictID")]
        public virtual Location PerDistrict { get; set; }

        //Processing Details
        [Column("Final_Submitted", TypeName = "bit")]
        public Boolean FinalSubmitted { get; set; }

        [Column("Final_Submission_Date")]
        public DateTime? FinalSubmissionDate { get; set; }

        [Column("Demand_Note_ID", TypeName = "bigint")]
        public Int64? DemandNoteID { get; set; }

        [Column("Fee_Type_ID", TypeName = "int")]
        public Int32? FeeTypeID { get; set; }

        [Column("Fee_Amt", TypeName = "decimal")]
        public Decimal? FeeAmount { get; set; }

        [Column("Is_Verified_By_Institute", TypeName = "bit")]
        public Boolean? IsVerifiedByInstitute { get; set; }

        [Column("Verified_On_By_Institute")]
        public DateTime? DateOfVerificationByInstitute { get; set; }

        [Column("Payment_Status_ID", TypeName = "int")]
        public Int32? PaymentStatusID { get; set; }

        [Column("Application_Status_ID", TypeName = "int")]
        public Int32 ApplicationStatusID { get; set; }

        [Column("Batch_Item_ID", TypeName = "bigint")]
        public Int64? BatchItemID { get; set; }

        [ForeignKey("DemandNoteID")]
        public virtual DemandNote DemandNote { get; set; }

        [ForeignKey("PaymentStatusID")]
        public virtual PaymentStatus PaymentStatus { get; set; }

        [ForeignKey("FeeTypeID")]
        public virtual FeeType FeeType { get; set; }

        [Column("App_Source", TypeName = "int")]
        public Int32 ApplicationSourceID { get; set; }

        [Column("Is_Synced", TypeName = "bit")]
        [Display(Name = "Is Synced")]
        public Boolean IsSynced { get; set; }

        [Column("Synced_On")]
        public DateTime? SyncedOn { get; set; }

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
        [NotMapped]
        public enmCourseApplicationStatus enmCourseApplicationStatus
        {
            get
            {
                return (enmCourseApplicationStatus)this.ApplicationStatusID;
            }
            set
            {
                this.ApplicationStatusID = (int)value;
            }
        }

        [Column("Payment_Source", TypeName = "int")]
        public Int32? PaymentSourceID { get; set; }

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

        [Column("Is_Linked", TypeName = "bit")]
        public Boolean IsLinked { get; set; }        

        [Column("Aadhar_Number", TypeName = "bigint")]
        public Int64? AadharNumber { get; set; }

        [Column("Aadhar_Verfied", TypeName = "bit")]
        public Boolean AadharVerfied { get; set; }

        [Column("UID_Type", TypeName = "int")]
        public Int32? UIDType { get; set; }

        [Column("UID_Number")]
        [MaxLength(50)]
        public String UIDNumber { get; set; }



        //Added 22 Feb 2019
        [Column("affidavitNo")]
        [MaxLength(100)]
        public string affidavitNo { get; set; }

        [Column("affidavitDate")]
        public DateTime? affidavitDate { get; set; }

        [Column("affidavitFile")]
        public string affidavitFile { get; set; }

        [Column("affidavitUpload", TypeName = "varbinary(Max)")]
        public Byte[] affidavitUpload { get; set; }

        [Column("affidavitVerified", TypeName = "bit")]
        public Boolean affidavitVerified { get; set; }

        [Column("affidavitUploadedOn")]
        public DateTime? affidavitUploadedOn { get; set; }

        [Column("affidavitUploadedBy")]
        public Int64? affidavitUploadedBy { get; set; }

        [Column("affidavitVerifiedOn")]
        public DateTime? affidavitVerifiedOn { get; set; }

        [Column("affidavitVerifiedBy")]
        public Int64? affidavitVerifiedBy { get; set; }

        [Column("GuardianFlagStatusID")]
        public Int64 GuardianFlagStatusID { get; set; }


        //


        // Added for UP Project  Start 

        //New_Columns_28_08_2024
        [Column("Project_ID", TypeName = "bigint")]
        public Int64? projectID { get; set; }


        //New_Columns_12_12_2024
        [Column("batchid", TypeName = "bigint")]
        public Int64? batchID { get; set; }

        //Added_30_12_2024_Start
        [Column("field1")]
        [MaxLength(400)]
        [Display(Name = "Field 1")]
        public string field1 { get; set; }

        [Column("field2")]
        [MaxLength(400)]
        [Display(Name = "Field 2")]
        public string field2 { get; set; }


        [Column("field3")]
        [MaxLength(400)]
        [Display(Name = "Field 3")]
        public string field3 { get; set; }


        [Column("field4")]
        [MaxLength(400)]
        [Display(Name = "Field 4")]
        public string field4 { get; set; }


        [Column("field5")]
        [MaxLength(400)]
        [Display(Name = "Field 5")]
        public string field5 { get; set; }


        // Added for UP Project  End  
        [Column("ApaarRequestID ", TypeName = "bigint")]
        public Int64 ApaarRequestID { get; set; }

    }
}
