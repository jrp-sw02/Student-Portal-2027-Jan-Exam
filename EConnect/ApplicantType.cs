using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect
{
    public class ApplicantType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public int ID { get; set; }

        [Column("NAME")]
        [Required(ErrorMessage = "Applicant Type Name is required")]
        [MaxLength(50)]
        [Display(Name = "Applicant Type Name")]
        public String Name { get; set; }

        [Column("NAME_REGIONAL")]
        [MaxLength(100)]
        [Display(Name = "Applicant Type Regional Name")]
        public String NameRegional { get; set; }

        [Column("Code")]
        [Required(ErrorMessage = "Code is required")]
        [MaxLength(10)]
        [Display(Name = "Code")]
        public String Code { get; set; }

        [Column("Display_Order", TypeName="int")]
        [Required(ErrorMessage = "Display Order is required")]
        [Display(Name = "Display Order")]
        public Int32 DisplayOrder { get; set; }
    }
}
