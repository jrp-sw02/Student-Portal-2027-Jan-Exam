using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect
{
    public class Language
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public int ID { get; set; }

        [Column("NAME")]
        [Required(ErrorMessage = "Language Name is required")]
        [MaxLength(50)]
        [Display(Name = "Language Name")]
        public String Name { get; set; }

        [Column("NAME_REGIONAL")]
        [MaxLength(100)]
        [Display(Name = "Applicant Type Regional Name")]
        public String NameRegional { get; set; }

        [Column("Language_Culture")]
        [MaxLength(10)]
        [Display(Name = "Language Culture")]
        public String LanguageCulture { get; set; }

        [Column("Unique_Seo_Code")]
        [MaxLength(10)]
        [Display(Name = "Unique Seo Code")]
        public String UniqueSeoCode { get; set; }

        [Column("Is_RTL")]
        public Boolean IsRTL { get; set; }

        [Column("Display_Order", TypeName = "int")]
        [Required(ErrorMessage = "Display Order is required")]
        [Display(Name = "Display Order")]
        public Int32 DisplayOrder { get; set; }
    }
}
