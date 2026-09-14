using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CourseExamSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Category is required")]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course is required")]
        public Int32 CourseID { get; set; }

        [Column("Exam_Session_ID", TypeName = "int")]
        [Required(ErrorMessage = "Exam Session is required")]
        public Int32 ExamSessionID { get; set; }

        [ForeignKey("ExamSessionID")]
        public virtual ExamSession ExamSession { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }
    }
}
