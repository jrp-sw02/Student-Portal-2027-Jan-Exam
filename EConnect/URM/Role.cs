using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.URM
{
   public  class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("User_Type_ID", TypeName = "int")]
        public Int32 UserTypeID { get; set; }
        
        [Column("Name")]
        [Required(ErrorMessage = "Role Name is required")]
        [MaxLength(50)]
        public String Name { get; set; }

        [Column("NAME_REGIONAL")]
        [MaxLength(100)]
        [Display(Name = "Role Regional Name")]
        public String NameRegional { get; set; }

        [ForeignKey("UserTypeID")]
        public virtual UsersType Usertype { get; set; }

        public virtual ICollection<Right> Rights { get; set; }
    }
}
