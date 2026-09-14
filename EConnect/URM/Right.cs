using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.URM
{
   public  class Right
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Role_ID", TypeName="int")]
        public Int32 RoleID { get; set; }

        [Column("Menu_Object_ID", TypeName = "int")]
        public Int32 MenuObjectID { get; set; }

        [Column("Is_New", TypeName = "bit")]
        [Display(Name = "Is New")]
        public Boolean HasNew { get; set; }

        [Column("Is_View", TypeName = "bit")]
        [Display(Name = "Is View")]
        public Boolean HasView { get; set; }

        [Column("Is_Edit", TypeName = "bit")]
        [Display(Name = "Is Edit")]
        public Boolean HasEdit { get; set; }

        [Column("Is_Delete", TypeName = "bit")]
        [Display(Name = "Is Delete")]
        public Boolean HasDelete { get; set; }

        [Column("Is_Full", TypeName = "bit")]
        [Display(Name = "Is Full")]
        public Boolean HasFullControl { get; set; }

        [Column("Created_On")]
        public DateTime CreatedOn { get; set; }

        [Column("Created_By",  TypeName="int")]
        public Int32 CreatedBy { get; set; }

        [Column("Modified_On")]
        public DateTime? ModifiedOn { get; set; }

        [Column("Modified_By", TypeName = "int")]
        public Int32? ModifiedBy { get; set; }

        [ForeignKey("MenuObjectID")]
        public virtual MenuObject MenuObject { get; set; }

        [ForeignKey("RoleID")]
        public virtual UserRole Role { get; set; }

    }
}
