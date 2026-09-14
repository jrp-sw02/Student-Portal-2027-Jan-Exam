using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class Exam
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [Required]
        [MaxLength(50)]
        public String Name { get; set; }

        [Column("Examn_Cycle_ID", TypeName = "int")]
        [Required]
        public Int32 ExaminationCycleID { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        [Required]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        [Required]
        public Int32 CourseID { get; set; }

        [Column("Exam_Year", TypeName = "int")]
        [Required]
        public Int32 ExamYear { get; set; }

        [Column("Exam_Month", TypeName = "int")]
        [Required(ErrorMessage = "Exam Month Name is required")]
        public Int32 ExamMonth { get; set; }

        [Column("Day_Occurance", TypeName = "int")]
        [Required(ErrorMessage = "Day Occurance is required")]
        public Int32 DayOccurance { get; set; }

        [Column("Weak_Number", TypeName = "int")]
        [Required(ErrorMessage = "Week Name is required")]
        public Int32 WeekNumber { get; set; }

        [Column("Exam_Start_Date")]
        [Required]
        public DateTime ExamStartDate { get; set; }

        [Column("Created_By", TypeName = "int")]
        [Required]
        public Int32 CreatedByID { get; set; }

        [Column("Created_On")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Column("Date_Of_Publishing_On_Web")]
        public DateTime? DateOfPublishingOfTimeTable { get; set; }

        [Column("Roll_Number_Publish_Date")]
        public DateTime? DateOfPublishingOfRollNumber { get; set; }

        [Column("Result_Publish_Date")]
        public DateTime? DateOfPublishingOfResult { get; set; }

        [Column("Practical_Admit_Card_Publish_Date")]
        public DateTime? DateofPublishingPracticalAdmitCard { get; set; }

        [Column("Online_Admit_Card_Publish_Date")]
        public DateTime? Online_Admit_Card_Publish_Date { get; set; }

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [ForeignKey("ExaminationCycleID")]
        public virtual ExaminationCycle ExaminationCycle { get; set; }

        [Column("Is_Dispatchable")]
        public Boolean IsDispatchable { get; set; }

        [Column("Is_Batch_Processable")]
        public Boolean IsBatchProcessable { get; set; }

        [Column("Result_Grade_Version_ID", TypeName = "int")]
        public Int32? ResultGradeVersionID { get; set; }

        [Column("Neft_Ext_Period")]
        public Int32 NeftExtPeriod { get; set; }

        [Column("Fee_Submission_Institute_Ext_Period")]
        public Int32 FeeSubmissioInstituteExtPeriod { get; set; }
        
        public virtual ICollection<CutOffDate> CutOffDates { get; set; }

        public virtual ICollection<ExamTimeTable> ExamTimeTableDetail { get; set; }

        public virtual ICollection<tempExamResultPub> tempExamResultPub { get; set; }

    }
}
