using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.URM
{
   public class UserRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Role_ID", TypeName = "int")]
        public Int32 RoleID { get; set; }

        [Column("User_ID", TypeName = "int")]
        public Int32 UserID { get; set; }

        [Column("Created_By", TypeName = "int")]
        public Int32 CreatedBy { get; set; }

        [Column("Created_ON")]
        public DateTime CreatedOn { get; set; }

        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }
      
    }
}
