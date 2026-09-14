using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EConnect.NIELIT;
//Added 21 Dec 2022 for defaultvalue attribute
using System.Collections.Generic;
using System.ComponentModel;
//
namespace EConnect
{
    public class EducationalQualification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public int ID { get; set; }

        [Column("NAME")]
        [Required(ErrorMessage = "Educational Qualification Name is required")]
        [MaxLength(150)]
        [Display(Name = "Educational Qualification Name")]
        public String Name { get; set; }

        [Column("NAME_REGIONAL")]
        [MaxLength(200)]
        [Display(Name = "Educational Qualification Regional Name")]
        public String NameRegional { get; set; }

        [Column("Code")]
        [MaxLength(10)]
        [Display(Name = "Code")]
        public String Code { get; set; }

        [Column("Qulification_Level_ID", TypeName = "int")]
        public Int32? QualificationLevelID { get; set; }

        [Column("Display_Order", TypeName = "int")]
        [Required(ErrorMessage = "Display Order is required")]
        [Display(Name = "Display Order")]
        public Int32 DisplayOrder { get; set; }

        //Added 21 Dec 2022  for centre pursuing issue
        [Column("allowFutureYear", TypeName = "bit")]
        [DefaultValue(false)]
        public bool allowFutureYear { get; set; }
        //

        [ForeignKey("QualificationLevelID")]
        public virtual QualificationLevel QualificationLevel{get;set;}

    }
       
}
