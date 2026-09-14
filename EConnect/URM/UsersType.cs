using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.URM
{
    public class UsersType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public int ID { get; set; }

        [Column("NAME")]
        [MaxLength(50)]
        public String Name { get; set; }

        [Column("Module_ID", TypeName = "int")]
        public Int32 ModuleID { get; set; }

        [ForeignKey("ModuleID")]
        public virtual MenuObject DefaultModule { get; set; }
    }
}
