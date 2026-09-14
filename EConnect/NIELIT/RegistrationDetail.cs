using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class RegistrationDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Candidate_ID", TypeName = "bigint")]
        public Int64 CandidateID { get; set; }

        [Column("Registration_No", TypeName = "bigint")]
        public Int64 RegistrationNo { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Category is required")]
        [Display(Name = "Course Category")]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public Int32 CourseID { get; set; }

        [Column("Registration_Month",TypeName = "int")]
        public Int32 RegistrationMonth { get; set; }

        [Column("Registration_Year", TypeName = "int")]
        public Int32 RegistrationYear { get; set; }

        [Column("Registration_Date")]
        public DateTime RegistrationDate { get; set; }

        [Column("Commencement_From_Date")]
        public DateTime CommencementFromDate { get; set; }

        [Column("Valid_Upto_Date")]
        public DateTime ValidUptoDate { get; set; }

        [Column("whether_extension",TypeName = "bit")]
        public Boolean WhetherExtension { get; set; }

        [Column("Reg_Type_ID", TypeName = "int")]
        public Int32? RegistrationTypeID { get; set; }

        [Column("Registration_Status_ID", TypeName = "int")]
        public Int32? RegistrationStatusID { get; set; }

        [Column("Registration_Status")]
        [MaxLength(1)]
        public string RegistrationStatusCode { get; set; }

        [Column("Applicant_Type_ID", TypeName = "int")]
        public Int32 ApplicantTypeID { get; set; }

        [Column("Institute_ID", TypeName = "bigint")]
        public Int64? InstituteID { get; set; }

        [Column("Experience_In_Years", TypeName = "decimal")]
        public Decimal? ExperienceInYears { get; set; }

        [Column("Re_Registration_No", TypeName = "int")]
        public Int32? ReRegistrationNumber { get; set; }

        [Column("Re_Registration_Date")]
        public DateTime? ReRegistrationDate { get; set; }

        [Column("Expiry_Date")]
        public DateTime? ExpiryDate { get; set; }

        [Column("Completion_Month", TypeName="int")]
        public Int32? CompletionMonth { get; set; }

        [Column("Completion_Year", TypeName="int")]
        public Int32? CompletionYear { get; set; }

        [Column("Cancelled_On")]
        public DateTime? CancelledOn { get; set; }

        [Column("Cancelled_By", TypeName = "int")]
        public Int32? CancelledBy { get; set; }

        [Column("Courser_Reg_Appl_ID", TypeName = "bigint")]
        public Int64? CourseRegistrationApplicationID { get; set; }

        [Column("Current_Reg_Status_ID", TypeName = "int")]
        public Int32? CurrentRegistrationStatusID { get; set; }

        [ForeignKey("CourseRegistrationApplicationID")]
        public virtual CourseRegistrationApplication CourseRegistrationApplication { get; set; }

        [NotMapped]
        public virtual enmCurrentRegistrationStatus CurrentRegistrationStatus
        {
            get
            {
                return (enmCurrentRegistrationStatus)this.CurrentRegistrationStatusID;
            }
            set
            {
                this.CurrentRegistrationStatusID = Convert.ToInt32(value);
            }
        }

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [ForeignKey("InstituteID")]
        public virtual Institute Institute { get; set; }

        [ForeignKey("ApplicantTypeID")]
        public virtual ApplicantType ApplicantType { get; set; }

        [ForeignKey("CandidateID")]
        public virtual Candidate Candidate { get; set; }

        [ForeignKey("RegistrationStatusID")]
        public virtual RegistrationStatus RegistrationStatus { get; set; }

        [NotMapped]
        public virtual enmApplicantType enmApplicantType
        {
            get { return (enmApplicantType)this.ApplicantTypeID; }
            set { this.ApplicantTypeID = Convert.ToInt32(value); }
        }
        [NotMapped]
        public virtual enmRegistrationStatus enmRegistrationStatus
        {
            get { return (enmRegistrationStatus)this.RegistrationStatusID; }
            set { this.RegistrationStatusID = Convert.ToInt32(value); }
        }
    }
}
