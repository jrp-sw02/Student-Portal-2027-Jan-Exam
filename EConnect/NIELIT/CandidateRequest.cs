using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CandidateRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Candidate_ID", TypeName = "bigint")]
        [Required(ErrorMessage = "Course is required")]
        public Int64 CandidateID { get; set; }

        [Column("Request_Type_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course is required")]
        public Int32 CandidateRequestTypeID { get; set; }

        [Column("Request_Date")]
        public DateTime DateOfRequest { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Category is required")]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course is required")]
        public Int32 CourseID { get; set; }

        [Column("Mobile_Number", TypeName = "bigint")]
        public Int64 MobileNumber { get; set; }

        [Column("OTP", TypeName = "int")]
        public Int32 OTP { get; set; }

        [Column("OTP_Last_Issue_Date")]
        public DateTime OTPLastIssueDate { get; set; }

        [Column("Is_OTP_Verified", TypeName = "bit")]
        public Boolean IsOtpVerified { get; set; }

        [Column("Otp_Verified_On")]
        public DateTime? DateOfOtpVerification { get; set; }

        [Column("Status_ID", TypeName = "int")]
        public Int32 StatusID { get; set; }

        [Column("Status_Remarks")]
        [MaxLength(100)]
        public String StatusRemarks { get; set; }

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }

        [ForeignKey("CandidateRequestTypeID")]
        public virtual CandidateRequestType CandidateRequestType { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [ForeignKey("CandidateID")]
        public virtual Candidate Candidate { get; set; }

    }
}
