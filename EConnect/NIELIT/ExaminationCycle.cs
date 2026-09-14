using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class ExaminationCycle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Exam Name is Required")]
        [MaxLength(50)]
        [Display(Name = "Exam Name")]
        public String Name { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Category is required")]
        [Display(Name = "Course Category")]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public Int32 CourseID { get; set; }

        [Column("Schedule_ID", TypeName = "int")]
        [Required(ErrorMessage = "Schedule is required")]
        [Display(Name = "Schedule")]
        public Int32 ExamScheduleID { get; set; }

        [Column("Starting_Month", TypeName = "int")]
        [Required(ErrorMessage = "Starting Month Name is required")]
        [Display(Name = "Starting Month Name")]
        public Int32 StartingMonth { get; set; }

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [NotMapped]
        public enmExamSchedule enmExamSchedule
        {
            get
            { return (enmExamSchedule)this.ExamScheduleID; }
            set { this.ExamScheduleID = Convert.ToInt32(value); }
        }

        public virtual ICollection<Exam> Exams { get; set; }
    }
}
