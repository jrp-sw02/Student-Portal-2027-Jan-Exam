using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CourseType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Course Type Name is required")]
        [MaxLength(100)]
        [Display(Name = "Course Type Name")]
        public String Name { get; set; }

        [Column("NAME_REGIONAL")]
        [MaxLength(150)]
        [Display(Name = "Course Type Regional Name")]
        public String NameRegional { get; set; }

        [Column("Display_Order", TypeName = "int")]
        [Required(ErrorMessage = "Display Order is required")]
        [Display(Name = "Display Order")]
        public Int32 DisplayOrder { get; set; }

        public virtual ICollection<Course> courses { get; set; }

        public virtual ICollection<ApplicationType> ApplicationTypes { get; set; }
    }
}
