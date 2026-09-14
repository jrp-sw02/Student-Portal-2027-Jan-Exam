using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Date")]
        [Required]
        public DateTime Date { get; set; }

        [Column("User_ID", TypeName = "int")]
        [Required]
        public Int32 UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual EConnect.URM.User User { get; set; }

        [Column("User_Type", TypeName = "int")]
        [Required]
        public Int32 UserTypeID { get; set; }

        [ForeignKey("UserTypeID")]
        public virtual EConnect.URM.UsersType UsersType { get; set; }

        [NotMapped]
        public EConnect.URM.UserType enmUserType
        {
            get
            {
                return (EConnect.URM.UserType)this.UserTypeID;
            }
            set
            {
                this.UserTypeID = (int)value;
            }
        }

        [Column("Feedback")]
        [MaxLength(1000)]
        public String FeedbackDescription { get; set; }

        [Column("suggestion")]
        [MaxLength(1000)]
        public String Suggestions { get; set; }
        
        [Column("Marked_Read", TypeName = "bit")]
        [Required]
        public Boolean IsMarkedRead { get; set; }

        [Column("Read_ON")]
        public DateTime? MrkedAsReadOn { get; set; }
    }
}
