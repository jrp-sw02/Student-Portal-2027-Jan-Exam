using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.URM
{
    public class UserLoginOtpDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", TypeName = "bigint")]
        public Int64 Id { get; set; }

        [Column("UserId")]
        public String LoginID { get; set; }

        [Column("UserTypeId", TypeName = "int")]
        public int UserTypeID { get; set; }

        [Column("OtpNumber", TypeName = "int")]
        public int OtpNumber { get; set; }

        [Column("CreatedOn")]
        public DateTime CreatedOn { get; set; }
    }
}
