using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class TimeTablePattern
    {
        [Column("Course_ID", TypeName = "int")]
        [Required]
        public Int32 CourseID { get; set; }

        [Column("Revision_Number", TypeName = "int")]
        [Required]
        public Int32 RevisionNumber { get; set; }

        [Column("Module_ID", TypeName = "int")]
        [Required]
        public Int32 ModuleID { get; set; }

        [Column("Exam_Day", TypeName = "int")]
        [Required]
        public Int32 ExamDay { get; set; }

        [Column("Exam_Session_ID", TypeName = "int")]
        [Required]
        public Int32 ExamSessionID { get; set; }

        [Column("Created_By", TypeName = "int")]
        [Required]
        public Int32 CreatedByID { get; set; }

        [Column("Created_On")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [ForeignKey("ExamSessionID")]
        public virtual ExamSession ExamSession { get; set; }

        [ForeignKey("ModuleID")]
        public virtual Module Module { get; set; }
    }
}
