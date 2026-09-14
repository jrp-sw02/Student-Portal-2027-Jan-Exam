using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class TblSmsEmailBulk
    {
        [Key]
        [Column("NAME")]
        [Display(Name = "Activity Name")]
        public String Name { get; set; }

        [Column("AppNum")]
        [Display(Name = "AppNum")]
        public String AppNum { get; set; }

        [Column("MobileNumber")]
        [Display(Name = "Mobile Number")]
        public Double MobileNumber { get; set; }

        [Column("Message")]
        [Display(Name = "Message")]
        public String Message { get; set; }

        [Column("Email")]
        [Display(Name = "Email")]
        public String Email { get; set; }

        [Column("EmailMsg")]
        [Display(Name = "EmailMsg")]
        public String EmailMsg { get; set; }

        [Column("SMS_Response")]
        [Display(Name = "SMS_Response")]
        public String SMS_Response { get; set; }

        [Column("SMS_sent_flag")]
        [Display(Name = "SMS_sent_flag")]
        public bool SmsSent { get; set; }

        [Column("SentDate")]
        [Display(Name = "SentDate")]
        public DateTime ? SMSSentDate { get; set; }

        [Column("SentEmailDate")]
        [Display(Name = "SentEmailDate")]
        public DateTime ? SentEmailDate { get; set; }


    }
}
