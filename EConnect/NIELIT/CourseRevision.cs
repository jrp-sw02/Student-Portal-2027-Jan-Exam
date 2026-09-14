using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CourseRevision
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Category is required")]
        [Display(Name = "Course Category")]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course required")]
        [Display(Name = "Course")]
        public Int32 CourseID { get; set; }

        [Column("Revision_Number", TypeName = "int")]
        [Required(ErrorMessage = "Revision Number required")]
        [Display(Name = "Revision Number")]
        public Int32 RevisionNumber { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Course Name is required")]
        [MaxLength(150)]
        [Display(Name = "Course Name")]
        public String Name { get; set; }

        [Column("Code")]
        [Required(ErrorMessage = "Course Code is required")]
        [MaxLength(20)]
        [Display(Name = "Course Code")]
        public String Code { get; set; }

        [Column("Effective_From_Date")]
        [Required(ErrorMessage = "Effective From Date is required")]
        [Display(Name = "Effective From Date")]
        public DateTime EffectiveFromDate { get; set; }

        [Column("Effective_To_Date")]
        [Display(Name = "Effective To Date")]
        public DateTime? EffectiveToDate { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }
    }
}
