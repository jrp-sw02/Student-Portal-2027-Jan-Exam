using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect
{
    public class LocationType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public int ID { get; set; }

        [Column("NAME")]
        [Required(ErrorMessage = "Location Type Name is required")]
        [MaxLength(50)]
        [Display(Name = "Location Type Name")]
        public String Name { get; set; }

        [Column("NAME_REGIONAL")]
        [MaxLength(100)]
        [Display(Name = "Location Type Regional Name")]
        public String NameRegional { get; set; }

        [Column("PARENTID", TypeName = "int")]
        public int? ParentID { get; set; }

        [Column("TYPE")]
        [MaxLength(50)]
        [Display(Name = "Type")]
        public String Type { get; set; }

        [Column("TYPE_REGIONAL")]
        [MaxLength(100)]
        [Display(Name = "Type Regional Name")]
        public String TypeRegional { get; set; }

        [ForeignKey("ParentID")]
        public virtual LocationType ParentLocationType { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
    }
}
