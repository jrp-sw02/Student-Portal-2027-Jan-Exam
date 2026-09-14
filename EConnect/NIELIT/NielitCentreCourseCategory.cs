using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
   public class NielitCentreCourseCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Course Category Name is required")]
        [MaxLength(100)]
        [Display(Name = "Course Category Name")]
        public String Name { get; set; }

        [Column("NAME_REGIONAL")]
        [MaxLength(150)]
        [Display(Name = "Course Category Regional Name")]
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

        [DefaultValue(true)]
        public bool IsActive { get; set; }
        
        //public virtual ICollection<NielitCentreCourse> NielitCentreCourses { get; set; }


    }
}
