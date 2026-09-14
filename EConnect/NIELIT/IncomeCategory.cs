using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
  public class IncomeCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Income Category Name is required")]
        [MaxLength(150)]
        [Display(Name = "Income Category Name")]
        public String Name { get; set; }

        [Column("Name_Regional")]
        [MaxLength(150)]
        [Display(Name = "Income Category Regional Name")]
        public String NameRegional { get; set; }

        [Column("Display_Order", TypeName = "int")]
        [Required(ErrorMessage = "Display Order is required")]
        [Display(Name = "Display Order")]
        public Int32 DisplayOrder { get; set; }
    }
}
