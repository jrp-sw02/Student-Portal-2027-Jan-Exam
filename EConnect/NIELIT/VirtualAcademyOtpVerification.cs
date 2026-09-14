using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class VirtualAcademyOtpVerification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", TypeName = "bigint")]
        public Int64 Id { get; set; }       

        [Column("virtualAcademyRegistrationID", TypeName = "bigint")]
        public Int64? virtualAcademyRegistrationID { get; set; }       

        [Column("Mobile", TypeName = "bigint")]
        public Int64? MobileNumber { get; set; }

        [Column("Mobile_OTP", TypeName = "int")]
        public Int32? Mobile_OTP { get; set; }

        [Column("MobileOTPCreatedOn")]
        public DateTime? MobileOTPCreatedOn { get; set; }

        [Column("Is_Mobile_Verified", TypeName = "bit")]
        public Boolean IsMobileNumberVerified { get; set; }

        [Column("Mobile_Verified_On")]
        public DateTime? MobileNumberVerifiedOn { get; set; }

        [Column("Email")]
        [MaxLength(100)]
        public String EmailAddress { get; set; }

        [Column("Email_OTP", TypeName = "int")]
        public Int32? Email_OTP { get; set; }

        [Column("EmailOTPCreatedOn")]
        public DateTime? EmailOTPCreatedOn { get; set; }

        [Column("Is_Email_Verified", TypeName = "bit")]
        public Boolean IsEmailVerified { get; set; }

        [Column("Email_Verified_On")]
        public DateTime? EmailVerifiedOn { get; set; }
       
    }
}
