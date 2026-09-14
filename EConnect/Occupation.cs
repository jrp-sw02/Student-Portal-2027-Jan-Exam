using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect
{
    public class Occupation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Occupation Name is required")]
        [MaxLength(100)]
        [Display(Name = "Occupation Name")]
        public String Name { get; set; }

        [Column("Code")]
        [Required(ErrorMessage = "Code is required")]
        [MaxLength(3)]
        public String Code { get; set; }

        [Column("Name_Regional")]
        [MaxLength(150)]
        [Display(Name = "Occupation Regional Name")]
        public String NameRegional { get; set; }

        [Column("Display_Order", TypeName = "int")]
        [Required(ErrorMessage = " Display Order is required")] 
        public Int32 DisplayOrder { get; set; }

        [Column("Is_Enabled", TypeName = "bit")]
        public Boolean IsEnabled { get; set; }

        [Column("Course_ID", TypeName = "int")]
        public Int32 ? CourseID { get; set; }
    }
}
