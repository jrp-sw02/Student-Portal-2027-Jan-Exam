using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CandidateContactDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Candidate_ID", TypeName = "bigint")]
        public Int64 CandidateID { get; set; }

        [Column("Mobile", TypeName = "bigint")]
        public Int64? MobileNumber { get; set; }

        [Column("Std", TypeName = "int")]
        public Int32? StdNumber { get; set; }

        [Column("Phone", TypeName = "int")]
        public Int32? PhoneNumber { get; set; }

        [Column("Email")]
        [MaxLength(100)]
        public String EmailAddress { get; set; }

        [Column("Effective_From_Date")]
        public DateTime EffectiveFromDate { get; set; }

        public virtual Candidate Candidate { get; set; }

        [Column("Is_Mobile_Verified", TypeName = "bit")]
        public Boolean IsMobileNumberVerified { get; set; }

        [Column("Mobile_Verified_On")]
        public DateTime? MobileNumberVerifiedOn { get; set; }

        [Column("Mobile_OTP", TypeName = "int")]
        public Int32? MobileOTP { get; set; }

        [Column("Mobile_OTP_Valid_Upto")]
        public DateTime? MobileOTPValidUptoDate { get; set; }

        [Column("Is_Email_Verified", TypeName = "bit")]
        public Boolean IsEmailVerified { get; set; }

        [Column("Email_Verified_On")]
        public DateTime? EmailVerifiedOn { get; set; }

        [Column("Email_OTP", TypeName = "int")]
        public Int32? EmailOTP { get; set; }

        [Column("Email_OTP_Valid_Upto")]
        public DateTime? EmailOTPValidUptoDate { get; set; }
    }
}