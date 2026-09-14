using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CertificateExamResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Exam_Cycle_ID", TypeName = "int")]
        [Required]
        public Int32 ExaminationCycleID { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        [Required]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        [Required]
        public Int32 CourseID { get; set; }

        [Column("Exam_ID", TypeName = "int")]
        [Required]
        public Int32 ExamID { get; set; }

        [Column("Regional_Centre_Name")]
        [Required(ErrorMessage = "Regional Centre Name is required")]
        [MaxLength(50)]
        [Display(Name = "Regional Centre Name")]
        public String RegionalCentreName { get; set; }

        [Column("Roll_Number")]
        [MaxLength(15)]
        public String RollNumber { get; set; }

        [Column("Candidate_Name")]
        [Required(ErrorMessage = "Candidate Name is required")]
        [MaxLength(30)]
        [Display(Name = "Candidate Name")]
        public String CandidateName { get; set; }

        [Column("Father_Name")]
        [MaxLength(30)]
        [Required(ErrorMessage = "Fathe's Name is required")]
        [Display(Name = "Father's Name Name")]
        public String FatherName { get; set; }

        [Column("Mother_Name")]
        [MaxLength(30)]
        [Required(ErrorMessage = "Mother's Name is required")]
        [Display(Name = "Mother's Name Name")]
        public String MotherName { get; set; }

        [Column("Institute_Acc_Number")]
        [MaxLength(30)]
        [Required(ErrorMessage = "Institute Accredition Number is required")]
        [Display(Name = "Institute Accredition Number")]
        public String InstituteAccreditationNumber { get; set; }

        [Column("Institute_Name")]
        [MaxLength(50)]
        [Required(ErrorMessage = "Institute Name is required")]
        [Display(Name = "Institute Name")]
        public String InstituteName { get; set; }

        [Column("Result")]
        [MaxLength(50)]
        [Required(ErrorMessage = "Result Is required")]
        [Display(Name = "Result")]
        public String Result { get; set; }

        [Column("Centre_Code")]
        [MaxLength(50)]
        [Required(ErrorMessage = "Centre Code Is required")]
        [Display(Name = "Centre Code")]
        public String ExamCentreCode { get; set; }

        [Column("Result_Wh")]
        [MaxLength(50)]
        [Required(ErrorMessage = "Result_Wh Is required")]
        [Display(Name = "Result Wh")]
        public String ResultWh { get; set; }

        [Column("Date_of_Exam")]
        public DateTime DateofExam { get; set; }

        [Column("Created_By", TypeName = "int")]
        [Required]
        public Int32 CreatedByID { get; set; }

        [Column("Created_On")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [ForeignKey("ExaminationCycleID")]
        public virtual ExaminationCycle ExaminationCycle { get; set; }

        [ForeignKey("ExamID")]
        public virtual Exam Exam { get; set; }

   }
}
