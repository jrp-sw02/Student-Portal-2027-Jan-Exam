using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class NSQFModuleCandidateMarks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Registration_no", TypeName = "int")]
        public Int32? RegistrationNo { get; set; }

        [Column("Candidate_id", TypeName = "int")]
        public Int32? CandidateId { get; set; }

        [Column("Course_id", TypeName = "int")]
        public Int32 CourseId { get; set; }

        [Column("Module_id", TypeName = "int")]
        public Int32? ModuleId { get; set; }

        [Column("Attempt_no_for_module_in_same_exam_month_and_year", TypeName = "int")]
        public Int32? AttemptNoForModule { get; set; }

        [Column("Module_code", TypeName = "int")]
        public Int32? ModuleCode { get; set; }

        [Column("Module_type", TypeName = "int")]
        public Int32? ModuleType { get; set; }

        [Column("Exam_month", TypeName = "int")]
        public Int32? ExamMonth { get; set; }

        [Column("Exam_year", TypeName = "int")]
        public Int32? ExamYear { get; set; }

        [Column("NSQF_roll_no")]      
        [MaxLength(60)]
        [Display(Name = "NSQF Roll No")]
        public String NSQFRollNo { get; set; }

        [Column("Exam_Date_Tentative")]
        [Display(Name = "Exam Date Tentative")]
        public DateTime? ExamDateTentative { get; set; }

        [Column("Exam_date")]
        [Display(Name = "Exam Date")]
        public DateTime? ExamDate { get; set; }

        [Column("BatchID", TypeName = "int")]
        public Int32? BatchId { get; set; }

        [Column("Total_Marks", TypeName = "int")]
        public Int32? TotalMarks { get; set; }

        [Column("Marks_upload_date")]
        [Display(Name = "Marks Upload Date")]
        public DateTime? MarksUploadDate { get; set; }

        [Column("Marks_upload_by")]
        [MaxLength(60)]
        [Display(Name = "Marks Upload By")]
        public String MarksUploadBy { get; set; }

        [Column("Result")]
        [MaxLength(60)]
        [Display(Name = "Result")]
        public String Result { get; set; }

        [Column("Absent_flag")]
        [MaxLength(60)]
        [Display(Name = "Absent Flag")]
        public String AbsentFlag { get; set; }

        [Column("UnfairMeansFlag")]
        [MaxLength(60)]
        [Display(Name = "UnfairMeansFlag")]
        public String UnfairMeansFlag { get; set; }

        [Column("ShowResultFlag")]
        [MaxLength(60)]
        [Display(Name = "ShowResultFlag")]
        public String ShowResultFlag { get; set; }

        [Column("Result_upload_date")]
        [Display(Name = "Result Upload Date")]
        public DateTime? ResultUploadDate { get; set; }

        [Column("Reslut_Declaration_Date")]
        [Display(Name = "Result Declaration Date")]
        public DateTime? ResultDeclarationDate { get; set; }


        [Column("Result_upload_by")]
        [MaxLength(60)]
        [Display(Name = "Result Upload By")]
        public String ResultUploadBy { get; set; }

        [Column("Remarks")]
        [MaxLength(60)]
        [Display(Name = "Remarks")]
        public String Remarks { get; set; }
        
        //Added 9 Apr 2026
        [Column("Grade_Code")]
        [MaxLength(1)]
        [Display(Name = "Grade Code")]
        public String GradeCode { get; set; }

    }
}
