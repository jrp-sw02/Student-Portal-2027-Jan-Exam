using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class ExamTimeTable
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

        [Column("Exam_ID", TypeName = "int")]
        [Required(ErrorMessage = "Exam Name is required")]
        public Int32 ExamID { get; set; }

        [Column("Module_ID", TypeName = "int")]
        [Required(ErrorMessage = "Modulee is required")]
        public Int32 ModuleID { get; set; }

        [Column("Exam_Sessiion_ID", TypeName = "int")]
        public Int32? ExamSessiionID { get; set; }

        [Column("Exam_From_Date")]
        public DateTime ExamFomDate { get; set; }

        [Column("Exam_To_Date")]
        public DateTime? ExamToDates { get; set; }
       
        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }
        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }
       
        [ForeignKey("ModuleID")]
        public virtual Module Module { get; set; }
        [ForeignKey("ExamID")]
        public virtual Exam Exam { get; set; }
        [ForeignKey("ExamSessiionID")]
        public virtual ExamSession ExamSession { get; set; }
    }
}
