using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class virtualAcademyRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("centreID", TypeName = "bigint")]
        public Int64 centreID { get; set; }

        [Column("CourseDurationID", TypeName = "bigint")]
        public Int64 CourseDurationID { get; set; }

        [Column("batchID", TypeName = "bigint")]
        public Int64 batchID { get; set; }
        
        [Column("Date")]
        [Required]
        public DateTime ApplicationDate { get; set; }

        [Column("Number")]
        [MaxLength(50)]
        public String Number { get; set; }       

        [Column("Already_Registered", TypeName = "bit")] 
        public Boolean Already_Registered { get; set; }

        [Column("Registered_Course_ID", TypeName = "int")]
        public Int32? Registered_Course_ID { get; set; }

        [Column("Registered_Course_Registration_No", TypeName = "int")]
        public Int32? Registered_Course_Registration_No { get; set; }

        [Column("Registration_Type_ID", TypeName = "int")]
        public Int32? RegistrationTypeID { get; set; }

        [Column("Applicant_Type_Id", TypeName = "int")]
        public Int32? Applicant_Type_Id { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        public Int32 CourseCategoryID { get; set; }

        [Column("Experience_In_Years", TypeName = "numeric")]
        public Decimal? ExperienceInYears { get; set; }

        [Column("Already_Qualified", TypeName = "bit")]
        public Boolean AlreadyQualified { get; set; }

        [Column("Qualified_Course_ID", TypeName = "int")]
        public Int32? QualifiedCourseID { get; set; }

        [Column("Qualified_Course_Registration_No", TypeName = "int")]
        public Int32? QualifiedCourseRegistrationNo { get; set; }

        [Column("Qualified_Course_Passing_Year", TypeName = "int")]
        public Int32? QualifiedCoursePassingYear { get; set; }

        //Processing Details
        [Column("Final_Submitted", TypeName = "bit")]
        public Boolean FinalSubmitted { get; set; }

        [Column("Final_Submission_Date")]
        public DateTime? FinalSubmissionDate { get; set; }


        //[Column("Registered_Course_ID", TypeName = "int")]
        //public Int32? RegisteredCourseID { get; set; }

        //[Column("Registered_Course_Registration_No", TypeName = "int")]
        //public Int32? RegisteredCourseRegistrationNo { get; set; }

     
        //Personal Details 

        [Column("Candidate_ID", TypeName = "bigint")]
        public Int64? Candidate_ID { get; set; }

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
        
        [Column("Body_Mark")]
        [MaxLength(50)]
        [Display(Name = "Body Mark")]
        public String BodyMark { get; set; }
        // Added jksah             

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
               
        //[Column("OnlineRefID")]
        //[MaxLength(30)]
        //public String OnlineRefID { get; set; }       
        

        //Qualification Detail

        [Column("Educational_Qualification_ID", TypeName = "int")]
        public Int32 EducationalQualificationID { get; set; }

        [Column("Passing_Year", TypeName = "int")]
        public Int32 PassingYear { get; set; }       

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

        [Column("IsMobileNumberVerified", TypeName = "bit")]
        public Boolean IsMobileNumberVerified { get; set; }

        [Column("IsEmailVerified", TypeName = "bit")]
        public Boolean IsEmailVerified { get; set; }

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

        [Column("Cor_Country_ID", TypeName = "bigint")]
        public Int64? Cor_Country_ID { get; set; }

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

        [Column("Per_Country_ID", TypeName = "bigint")]
        public Int64? PerCountryID { get; set; }

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

        [Column("Demand_Note_ID", TypeName = "bigint")]
        public Int64? DemandNoteID { get; set; }

        [Column("Fee_Type_ID", TypeName = "int")]
        public Int32? FeeTypeID { get; set; }

        [Column("Fee_Amt", TypeName = "decimal")]
        public Decimal? FeeAmount { get; set; }

        [Column("Payment_Status_ID", TypeName = "int")]
        public Int32? PaymentStatusID { get; set; }

        [Column("Application_Status_ID", TypeName = "int")]
        public Int32 ApplicationStatusID { get; set; }

        [Column("Is_Verified_By_Institute", TypeName = "bit")]
        public Boolean? IsVerifiedByInstitute { get; set; }

        [Column("Verified_On_By_Institute")]
        public DateTime? DateOfVerificationByInstitute { get; set; }

        [Column("Aadhar_Number")]
        public string AadharNumber { get; set; }

        [Column("Aadhar_Verfied", TypeName = "bit")]
        public Boolean AadharVerfied { get; set; }

        [Column("UID_Type", TypeName = "int")]
        public Int32? UIDType { get; set; }

        [Column("UID_Number")]
        [MaxLength(50)]
        public String UIDNumber { get; set; }

        [Column("whetherProjectStudent", TypeName = "bit")]
        public Boolean whetherProjectStudent { get; set; }

        //[Column("Batch_Item_ID", TypeName = "bigint")]
        //public Int64? BatchItemID { get; set; }

        [Column("projectId", TypeName = "bigint")]
        public Int64? projectId { get; set; }

        [Column("whetherCourseComplete", TypeName = "bit")]
        public Boolean whetherCourseComplete { get; set; }

        [Column("whetherCertificateIssued", TypeName = "bit")]
        public Boolean whetherCertificateIssued { get; set; }

        [Column("certificateIssueDate")]
        public DateTime? certificateIssueDate { get; set; }

        [Column("whetherPlaced", TypeName = "bit")]
        public Boolean whetherPlaced { get; set; }

        [Column("placementDate")]
        public DateTime? placementDate { get; set; }


        [Column("companyNameID", TypeName = "bigint")]
        public Int64? companyNameID { get; set; }


        [Column("comapnyAddress")]
        //[Required(ErrorMessage = "Address Line1 required")]
        [MaxLength(100)]
        [Display(Name = "Address Line1")]
        public String comapnyAddress { get; set; }

        [Column("comapnyAddress2")]
        //[Required(ErrorMessage = "Address Line2 required")]
        [MaxLength(100)]
        [Display(Name = "Address Line2")]
        public String comapnyAddress2 { get; set; }

        [Column("comapnyAddress3")]
        [MaxLength(100)]
        [Display(Name = "Address Line3")]
        public String comapnyAddress3 { get; set; }

        [Column("comapnyCountry_ID", TypeName = "bigint")]
        public Int64? comapnyCountry_ID { get; set; }

        [Column("comapnyState_ID", TypeName = "bigint")]
       // [Required(ErrorMessage = "State required")]
        [Display(Name = "State")]
        public Int64? comapnyState_ID { get; set; }

        [Column("comapnyDistrict_ID", TypeName = "bigint")]
        [Display(Name = "District")]
        public Int64? comapnyDistrict_ID { get; set; }

        [Column("comapnyCity_Name")]
        [MaxLength(50)]
        [Display(Name = "City Name")]
        public String comapnyCity_Name { get; set; }

        [Column("comapnyPin_Code", TypeName = "int")]
        //[Required(ErrorMessage = "Pin Code required")]
        [Display(Name = "Pin Code")]
        public Int32? comapnyPin_Code { get; set; }

        [Column("enterBy", TypeName = "int")]
        public Int32? enterBy { get; set; }

        [Column("enterDate")]
        public DateTime? enterDate { get; set; }


        //Added 22 Feb 2019
        [Column("affidavitNo")]
        [MaxLength(100)]
        public string affidavitNo { get; set; }

        [Column("affidavitDate")]
        public DateTime? affidavitDate { get; set; }

        [Column("affidavitVerified", TypeName = "bit")]
        public Boolean affidavitVerified { get; set; }


        [Column("GuardianFlagStatusID")]
        public Int64 GuardianFlagStatusID { get; set; }


        [Column("is_EWS", TypeName = "bit")]
        public Boolean Is_EWS { get; set; }
       
        [NotMapped]
        public enmApplicantType enmApplicantType
        {
            get
            {
                return (enmApplicantType)this.Applicant_Type_Id;
            }
            set
            {
                this.Applicant_Type_Id = (int)value;
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
        //[Column("affidavitFile")]
        //public string affidavitFile { get; set; }

        //[Column("affidavitUpload", TypeName = "varbinary(Max)")]
        //public Byte[] affidavitUpload { get; set; }

        

        //[Column("affidavitUploadedOn")]
        //public DateTime? affidavitUploadedOn { get; set; }

        //[Column("affidavitUploadedBy")]
        //public Int64? affidavitUploadedBy { get; set; }

        //[Column("affidavitVerifiedOn")]
        //public DateTime? affidavitVerifiedOn { get; set; }

        //[Column("affidavitVerifiedBy")]
        //public Int64? affidavitVerifiedBy { get; set; }

       


        //
    }
}
