using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class SentSMS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Is_Manual", TypeName = "bit")]
        public Boolean IsManuallySent { get; set; }

        [Column("Mobile_Number", TypeName = "bigint")]
        public Int64 MobileNumber { get; set; }

        [Column("Message")]
        [MaxLength(500)]
        public String Message { get; set; }

        [Column("Response_ID")]
        public Int32 ResponseID { get; set; }

        [NotMapped]
        public NIELIT.SmsResponse Response
        {
            get
            {
                return (NIELIT.SmsResponse)this.ResponseID;
            }
            set
            {
                this.ResponseID = (int)value;
            }
        }

        [Column("Created_By", TypeName = "int")]
        public Int32? SentByUserID { get; set; }

        [ForeignKey("SentByUserID")]
        public virtual EConnect.URM.User SentByUser { get; set; }

        [Column("Created_On")]
        [Required]
        public DateTime SentOn { get; set; }

        [Column("MsgId")]
        public String MsgId { get; set; }
    }
}
