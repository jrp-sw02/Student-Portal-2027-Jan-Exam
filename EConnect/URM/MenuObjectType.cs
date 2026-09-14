using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.URM
{
    public class MenuObjectType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public int ID { get; set; }

        [Column("NAME")]
        [Required(ErrorMessage = "Object Type Name is required")]
        [MaxLength(20)]
        [Display(Name = "Object Type Name")]
        public String Name { get; set; }

        [Column("PARENT_ID", TypeName = "int")]
        public int? ParentID { get; set; }

        [Column("HAS_SAME_PARENT")]
        public Boolean HasSameParent { get; set; }

        [Column("IS_WINDOW_TYPE")]
        public Boolean IsWindowType { get; set; }

        [Column("DEFAULT_WINDOW_PARAMS")]
        [MaxLength(300)]
        public String DefaultWindowParameters { get; set; }

        [Column("DEFAULT_ICON_PATH")]
        [MaxLength(200)]
        public String DefaultIconPath { get; set; }

        [Column("DEFUALT_THUMBNAIL_PATH")]
        [MaxLength(200)]
        public String DefaultThumbNailPath { get; set; }

        public virtual ICollection<MenuObject> MenuOjects { get; set; }
    }
}
