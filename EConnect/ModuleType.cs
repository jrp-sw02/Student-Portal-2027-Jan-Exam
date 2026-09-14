using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect
{
    public class ModuleType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Module Type Name is required")]
        [MaxLength(50)]
        [Display(Name = "Module Type Name")]
        public String Name { get; set; }

        [Column("NAME_REGIONAL")]
        [MaxLength(100)]
        [Display(Name = "Module Type Regional Name")]
        public String NameRegional { get; set; }

        [Column("Code")]
        [Required(ErrorMessage = "Module Type Code is required")]
        [MaxLength(10)]
        [Display(Name = "Module Type Code")]
        public String Code { get; set; }

        [Column("Display_Order", TypeName = "int")]
        [Required(ErrorMessage = "Display Order is required")]
        [Display(Name = "Display Order")]
        public Int32 DisplayOrder { get; set; }
    }
}
