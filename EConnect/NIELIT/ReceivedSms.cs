using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class ReceivedSms
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Mobile_Number", TypeName = "bigint")]
        public Int64 MobileNumber { get; set; }

        [Column("Registration_Number", TypeName = "bigint")]
        public Int64? RegistrationNumber { get; set; }

        [Column("Sms_Data")]
        [MaxLength(500)]
        public String RequestData { get; set; }

        [Column("Message")]
        [MaxLength(160)]
        public String Message { get; set; }

        [Column("Is_Valid", TypeName = "bit")]
        public Boolean IsValid { get; set; }

        [Column("Created_On")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Column("Sent_Email_Id", TypeName = "bigint")]
        public Int64? SentEmailId { get; set; }

        [ForeignKey("SentEmailId")]
        public virtual EConnect.NIELIT.SentEmail SentEmail { get; set; }

    }
}
