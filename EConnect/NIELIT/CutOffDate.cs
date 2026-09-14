using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CutOffDate
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

        [Column("Activity_ID", TypeName = "int")]
        [Required(ErrorMessage = "Activity Type is required")]
        public Int32 ActivityID { get; set; }

        [Column("Applicant_Type_ID", TypeName = "int")]
        [Required(ErrorMessage = "Applicant Type is required")]
        public Int32 ApplicantTypeID { get; set; }

        [Column("Exam_ID", TypeName = "int")]
        [Required(ErrorMessage = "Exam Name is required")]
        public Int32 ExamID { get; set; }

        [Column("Efferctive_Date")]
        public DateTime EfferctiveDate { get; set; }

        [NotMapped]
        public virtual enmActivity enmActivity
        {
            get
            {
                return (enmActivity)this.ApplicantTypeID;
            }
            set
            {
                this.ApplicantTypeID = Convert.ToInt32(value);
            }
        }
        [NotMapped]
        public virtual enmApplicantType enmApplicantType
        {
            get
            {
                return (enmApplicantType)this.ApplicantTypeID;
            }
            set
            {
                this.ApplicantTypeID = Convert.ToInt32(value);
            }
        }
        
        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }

        [ForeignKey("ActivityID")]
        public virtual Activity Activity { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [ForeignKey("ApplicantTypeID")]
        public virtual ApplicantType ApplicantType { get; set; }

        [ForeignKey("ExamID")]
        public virtual Exam Exam { get; set; }
    }
}
