using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class AffInstitute
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("instituteID", TypeName = "bigint")]
        public Int64 instituteID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Institute Name is required")]
        [MaxLength(150)]
        [Display(Name = "Course Name")]
        public String Name { get; set; }

        [Column("Accr_No")]
        [Required(ErrorMessage = "Accredation number")]
        [MaxLength(150)]
        [Display(Name = "Accredation Name")]
        public String Accr_No  { get; set; }

        [Column("linkedToCentre", TypeName = "bigint")]
        [Display(Name = "linked To Centre")]
        public Int32 linkedToCentre { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        [Display(Name = "User Name")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }


    }
}
