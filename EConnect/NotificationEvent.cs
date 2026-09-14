using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect
{
    public class NotificationEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public int ID { get; set; }

        [Column("EVENT_NAME")]
        [Required(ErrorMessage = "Event Name is required")]
        [MaxLength(200)]
        [Display(Name = "Event Name")]
        public String EventName { get; set; }

        [Column("Send_Sms", TypeName = "bit")]
        public Boolean SendSms { get; set; }

        [Column("Send_Email", TypeName = "bit")]
        public Boolean SendEmail { get; set; }
    }
}
