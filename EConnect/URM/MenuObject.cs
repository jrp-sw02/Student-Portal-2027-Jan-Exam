using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EConnect.HRMS;
namespace EConnect.URM
{
    public class MenuObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }

        [Column("ORG_ID", TypeName = "int")]
        [Required(ErrorMessage = "Organization ID is required")]
        public int OrganizationID { get; set; }

        [Column("NAME")]
        [Required(ErrorMessage = "Object Name is required")]
        [MaxLength(50)]
        [Display(Name = "Object Name")]
        public String Name { get; set; }

        [Column("TYPE_ID", TypeName = "int")]
        [Required(ErrorMessage = "Menu Object Type is required")]
        public int MenuObjectTypeID { get;  set; }

        [Column("PARENT_ID", TypeName = "int")]
        public int? ParentMenuObjectID { get; set; }

        [Column("ABRIVIATION")]
        [MaxLength(10)]
        public String Abbreviation { get; set; }

        [Column("ICON")]
        public byte[] Icon  { get; set; }

        [Column("THUMBNAIL")]
        public byte[] ThumbNail { get; set; }

        [Column("ORDER_NO", TypeName = "int")]
        public int DisplayOrder { get; set; }

        [Column("NEW_WINDOW")]
        public Boolean OpenInNewWindow { get; set; }

        [Column("WINDOW_PARAMS")]
        public String NewWindowParamenters { get; set; }

        [Column("URL")]
        public String FormURL { get; set; }

        [Column("CREATED_BY", TypeName = "int")]
        [Required(ErrorMessage = "Created By is required")]
        public int CreatedBy { get; set; }

        [Column("CREATED_ON")]
        public DateTime CreatedOn { get; set; }

        [Column("MODIFIED_BY", TypeName = "int")]
        public int? ModifiedBy { get; set; }

        [Column("MODIFIED_ON")]
        public DateTime? ModifiedOn { get; set; }

        [Column("ICON_PATH")]
        public String IconPath { get; set; }

        [Column("THUMBNAIL_PATH")]
        public String ThumbNailPath { get; set; }

        [Column("DELETED")]
        public Boolean Deleted { get; set; }

        [Column("IS_PARENT")]
        public Boolean IsParent { get; set; }
        
        [NotMapped]
        public enmMenuObjectType enmMenuObjectType
        { 
            get 
            {
                return (enmMenuObjectType)this.MenuObjectTypeID; 
            } 
            set 
            { 
                this.MenuObjectTypeID = (int)value;
            }
        }

        public virtual MenuObjectType MenuObjectType { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual MenuObject  ParentMenuObject { get; set; }

        public virtual ICollection<MenuObject> ChildMenuObjects { set; get; }
    }
}
