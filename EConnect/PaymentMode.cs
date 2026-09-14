using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect
{
    public class PaymentMode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public int ID { get; set; }

        [Column("NAME")]
        [Required(ErrorMessage = "Payment Mode Name is required")]
        [MaxLength(50)]
        [Display(Name = "Payment Mode Name")]
        public String Name { get; set; }

        [Column("NAME_REGIONAL")]
        [MaxLength(100)]
        [Display(Name = "Payment Mode Regional Name")]
        public String NameRegional { get; set; }

        [Column("Display_Order", TypeName = "int")]
        [Required(ErrorMessage = "Display Order is required")]
        [Display(Name = "Display Order")]
        public Int32 DisplayOrder { get; set; }

        [Column("Show", TypeName = "bit")]
        public Boolean Visible { get; set; }
    }
}
