using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect
{
    public class CastCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Cast Category Name is required")]
        [MaxLength(50)]
        [Display(Name = "Cast Category Name")]
        public String Name { get; set; }

        [Column("NAME_REGIONAL")]
        [MaxLength(100)]
        [Display(Name = "Cast Category Regional Name")]
        public String NameRegional { get; set; }

        [Column("Code")]
        [Required(ErrorMessage = "Cast Category Code is required")]
        [MaxLength(10)]
        [Display(Name = "Cast Category Code")]
        public String Code { get; set; }

        [Column("Display_Order", TypeName = "int")]
        [Required(ErrorMessage = "Display Order is required")]
        [Display(Name = "Display Order")]
        public Int32 DisplayOrder { get; set; }
    }
}
