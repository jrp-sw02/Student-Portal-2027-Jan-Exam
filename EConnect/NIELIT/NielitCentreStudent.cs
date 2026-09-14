using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class NielitCentreStudent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("centreID", TypeName = "bigint")]
        public Int64 InstituteID { get; set; }

        [Column("whetherAffiliated", TypeName = "char")]
        [MaxLength(1)]
        public string whetherAffiliated { get; set; }

        [Column("CourseID", TypeName = "bigint")]
        public Int64 CourseID { get; set; }

        [Column("batchID", TypeName = "bigint")]
        public Int64? batch_ID { get; set; }

        [Column("RollNo", TypeName = "int")]
        public Int32? Roll_No { get; set; }

        [Column("Date")]
        [Required]
        public DateTime ApplicationDate { get; set; }

        [Column("Number")]
        [MaxLength(30)]
        public String Number { get; set; }

        [Column("Candidate_ID", TypeName = "bigint")]
        public Int64? CandidateID { get; set; }

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

        [Column("Already_Qualified", TypeName = "bit")]
        public Boolean AlreadyQualified { get; set; }

        [Column("Qualified_Course_ID", TypeName = "int")]
        public Int32? QualifiedCourseID { get; set; }

        [Column("Qualified_Course_Registration_No", TypeName = "int")]
        public Int32? QualifiedCourseRegistrationNo { get; set; }

        [Column("Qualified_Course_Passing_Year", TypeName = "int")]
        public Int32? QualifiedCoursePassingYear { get; set; }

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
        
        // Added jksah

        [Column("Is_EWS", TypeName = "bit")]
        public Boolean Is_EWS { get; set; }

        [Column("whetherProjectStudent", TypeName = "bit")]
        [Display(Name = "whetherProjectStudent")]
        public Boolean whetherProjectStudent { get; set; }


        [Column("projectId", TypeName = "bigint")]
        public Int64 projectId { get; set; }

        [Column("Body_Mark")]
        [MaxLength(50)]
        [Display(Name = "Body Mark")]
        public String BodyMark { get; set; }

        //[ForeignKey("RegisteredCourseID")]
        //public virtual Course RegisteredCourse { get; set; }
        //[ForeignKey("QualifiedCourseID")]
        //public virtual Course QualifiedCourse { get; set; }
        //public virtual ApplicantType ApplicantType { get; set; }
        //public virtual Institute Institute { get; set; }
        //public virtual CourseCategory CourseCategory { get; set; }
        //public virtual Course Course { get; set; }
        //public virtual MaritalStatus MaritalStatus { get; set; }
        //public virtual CastCategory CastCategory { get; set; }
        //public virtual Religion Religion { get; set; }
        //public virtual Candidate Candidate { get; set; }

        //[NotMapped]
        //public enmApplicantType enmApplicantType
        //{
        //    get
        //    {
        //        return (enmApplicantType)this.ApplicantTypeID;
        //    }
        //    set
        //    {
        //        this.ApplicantTypeID = (int)value;
        //    }
        //}

        //[NotMapped]
        //public enmMaritalStatus enmMaritalStatus
        //{
        //    get
        //    {
        //        return (enmMaritalStatus)this.MaritalStatusID;
        //    }
        //    set
        //    {
        //        this.MaritalStatusID = (int)value;
        //    }
        //}
        //[NotMapped]
        //public enmCastCategory enmCastCategory
        //{
        //    get
        //    {
        //        return (enmCastCategory)this.CastCategoryID;
        //    }
        //    set
        //    {
        //        this.MaritalStatusID = (int)value;
        //    }
        //}

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
        
        [Column("Cor_Country_ID", TypeName = "bigint")]
        public Int64? CorCountryID { get; set; }

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

        //[ForeignKey("CorStateID")]
        //public virtual Location CorState { get; set; }

        //[ForeignKey("CorDistrictID")]
        //public virtual Location CorDistrict { get; set; }
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
        [Display(Name = "Country")]
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

        //[ForeignKey("PerStateID")]
        //public virtual Location PerState { get; set; }

        //[ForeignKey("PerDistrictID")]
        //public virtual Location PerDistrict { get; set; }

        //Processing Details
        [Column("Final_Submitted", TypeName = "bit")]
        public Boolean FinalSubmitted { get; set; }

        [Column("Final_Submission_Date")]
        public DateTime? FinalSubmissionDate { get; set; }



        [Column("Is_Verified_By_Institute", TypeName = "bit")]
        public Boolean? IsVerifiedByInstitute { get; set; }

        [Column("Verified_On_By_Institute")]
        public DateTime? DateOfVerificationByInstitute { get; set; }

        [Column("Aadhar_Number", TypeName = "bigint")]
        public Int64? AadharNumber { get; set; }

        [Column("Aadhar_Verfied", TypeName = "bit")]
        public Boolean? AadharVerfied { get; set; }

        [Column("UID_Type", TypeName = "int")]
        public Int32? UIDType { get; set; }

        [Column("UID_Number")]
        [MaxLength(50)]
        public String UIDNumber { get; set; }

        //Add xtra
        [Column("whetherCourseComplete", TypeName = "bit")]
        public Boolean? whether_Course_Complete { get; set; }

        [Column("whetherCertificateIssued", TypeName = "bit")]
        public Boolean? whether_Certificate_Issued { get; set; }

        [Column("certificateIssueDate")]
        public DateTime? certificate_Issue_Date { get; set; }

        [Column("whetherPlaced", TypeName = "bit")]
        public Boolean? whetherPlaced { get; set; }
        
        [Column("placementDate")]
        public DateTime? placementDate { get; set; }

        [Column("companyNameID", TypeName = "bigint")]
        [Display(Name = "companyNameID")]
        public Int64? company_Name { get; set; }

        [Column("comapnyAddress")]
        
        [MaxLength(100)]
        [Display(Name = "Comapny Address")]
        public String comapny_Address { get; set; }

        [Column("comapnyAddress2")]
        
        [MaxLength(100)]
        [Display(Name = "Comapny Address2")]
        public String comapny_Address2 { get; set; }

        [Column("comapnyAddress3")]
        
        [MaxLength(100)]
        [Display(Name = "Comapny Address3")]
        public String comapny_Address3 { get; set; }


        [Column("comapnyCountry_ID", TypeName = "bigint")]
        [Display(Name = "CountryID")]
        public Int64? CompnyCountry_ID { get; set; }

        [Column("comapnyState_ID", TypeName = "bigint")]
        [Display(Name = "State")]
        public Int64? compnyState_ID { get; set; }

        [Column("comapnyDistrict_ID", TypeName = "bigint")]
        [Display(Name = "District")]
        public Int64? compnyDistrict_ID { get; set; }

        [Column("comapnyCity_Name")]
        [MaxLength(50)]
        [Display(Name = "City Name")]
        public String CompnyCityName { get; set; }

        [Column("comapnyPin_Code1", TypeName = "int")]
        
        [Display(Name = "Pin Code")]
        public Int32? CompnyPinCode { get; set; }

        [Column("enterBy", TypeName = "int")]
        public Int32 enter_By { get; set; }

        [Column("enterDate")]
        public DateTime? enter_Date { get; set; }

        [Column("affidavitNo")]
        [MaxLength(100)]
        [Display(Name = "Affidavit No")]
        public String affidavit_No { get; set; }

        [Column("affidavitDate")]
        public DateTime? affidavit_Date { get; set; }

        [Column("affidavitVerified", TypeName = "bit")]
        public Boolean? affidavit_Verified { get; set; }
	 	//Added for Formal Course

        [Column("semesterid", TypeName = "int")]
        public Int32? semesterid { get; set; }

        [Column("University_RegistrationNo")]
        public string University_RegistrationNo { get; set; }

        [Column("WhetherLateralEntry", TypeName = "int")]
        public Int32? WhetherLateralEntry { get; set; }

        [Column("whetherDropOut", TypeName = "bit")]
        public Boolean? whetherDropOut { get; set; }

    }
}
