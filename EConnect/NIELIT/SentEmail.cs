using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class SentEmail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Is_Manual", TypeName = "bit")]
        public Boolean IsManuallySent { get; set; }

        [Column("Subject")]
        [MaxLength(100)]
        public String Subject { get; set; }

        [Column("Email_Address")]
        [MaxLength(70)]
        public String EmailAddress { get; set; }

        [Column("Message")]
        public String Message { get; set; }

        [Column("Response_Message")]
        [MaxLength(100)]
        public String ResponseMessage { get; set; }

        [Column("Created_By", TypeName = "int")]
        public Int32? SentByUserID { get; set; }

        [ForeignKey("SentByUserID")]
        public virtual EConnect.URM.User SentByUser { get; set; }

        [Column("Created_On")]
        [Required]
        public DateTime SentOn { get; set; }
    }
}
