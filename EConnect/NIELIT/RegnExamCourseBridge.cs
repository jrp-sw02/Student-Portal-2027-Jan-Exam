using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class RegnExamCourseBridge
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Category is required")]
        [Display(Name = "Course Category")]
        public Int32 CourseCategoryID { get; set; }

        [Column("Regn_CourseID", TypeName = "int")]
        [Required(ErrorMessage = "Registration Course Name is required")]
        [Display(Name = "Registration Course Name")]
        public Int32 RegnCourseID { get; set; }

        [Column("Exam_CourseID", TypeName = "int")]
        [Required(ErrorMessage = "Exam Course name is required")]
        [Display(Name = "Exam Course Name")]
        public Int32 ExamCourseID { get; set; }
    }
}
