using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect
{
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("NAME")]
        [Required(ErrorMessage = "Location Name is required")]
        [MaxLength(100)]
        [Display(Name = "Location Name")]
        public String Name { get; set; }

        [Column("NAME_REGIONAL")]
        [MaxLength(100)]
        [Display(Name = "Location Name Regional Name")]
        public String NameRegional { get; set; }

        [Column("Code")]
        [Required(ErrorMessage = "Location Code is required")]
        [MaxLength(10)]
        [Display(Name = "Location Code")]
        public String Code { get; set; }

        [Column("Location_Type_ID", TypeName = "int")]
        [Required(ErrorMessage = "Location Type ID is required")]
        public int LocationTypeID { get; set; }

        [Column("PARENT_ID", TypeName = "bigint")]
        public Int64? ParentLocationID { get; set; }

        [Column("TYPENAME")]
        [MaxLength(100)]
        [Display(Name = "Type Name")]
        public String TypeName { get; set; }

        [Column("Type_Name_Regional")]
        [MaxLength(150)]
        [Display(Name = "Type Name Regional Name")]
        public String TypeNameRegional { get; set; }

        //Added for Lucknow Gorakhpur bifurcation 21 Dec 2022
        [Column("isActive", TypeName = "bit")]
        public bool isActive { get; set; }

        [NotMapped]
        public enmLocationType enmLocationType
        {
            get
            {
                return (enmLocationType)this.LocationTypeID;
            }
            set
            {
                this.LocationTypeID = (int)value;
            }
        }

        public virtual LocationType LocationType { get; set; }

        [ForeignKey("ParentLocationID")]
        public virtual Location ParentLocation { get; set; }

        public virtual ICollection<Location> ChildLocations { set; get; }
    }
}
