using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class CourseProjectApplicationDetail
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Course_Exam_Appl_ID", TypeName = "bigint")]
        public Int64? CourseExamApplicationID { get; set; }

        public virtual CourseExamApplication CourseExamApplication { get; set; }

        [Column("Candidate_ID", TypeName = "bigint")]
        public Int64 CandidateID { get; set; }

        public virtual Candidate Candidate { get; set; }

        [Column("Registration_Number", TypeName = "bigint")]
        public Int64 RegistrationNumber { get; set; }

        [Column("Module_ID", TypeName = "int")]
        public Int32 ModuleID { get; set; }

        [ForeignKey("ModuleID")]
        public virtual Module Module { get; set; }

        [Column("Course_ID", TypeName = "int")]
        public Int32? CourseID { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [Column("Fee_Amount", TypeName = "decimal")]
        public Decimal FeeAmount { get; set; }

        [Column("In_Lieu_Module_ID", TypeName = "int")]
        public Int32? InLieuModuleID { get; set; }

        [ForeignKey("InLieuModuleID")]
        public virtual Module InLieuModule { get; set; }

        [Column("Is_UMC", TypeName = "bit")]
        public Boolean IsUnfairMeansCase { get; set; }

        [Column("UMC_ID", TypeName = "bigint")]
        public Int64? UnfairMeansCaseID { get; set; }

        [ForeignKey("UnfairMeansCaseID")]
        public virtual UnfairMeansCase UnfairMeansCase { get; set; }

        [Column("Umc_Decision_ID", TypeName = "int")]
        public Int32? UmcDecisionID { get; set; }

        [ForeignKey("UmcDecisionID")]
        public UmcDecisionType UmcDecisionType { get; set; }

        [NotMapped]
        public enmUmcDecisionType enmUmcDecisionType
        {
            get
            {
                return (enmUmcDecisionType)this.UmcDecisionID;
            }
            set
            {
                this.UmcDecisionID = (int)value;
            }
        }

        [Column("Marks_Objective", TypeName = "int")]
        public Int32? MarksObjective { get; set; }

        [Column("Marks_Descriptive", TypeName = "int")]
        public Int32? MarksDescriptive { get; set; }

        [Column("Marks_Grace", TypeName = "int")]
        public Int32? MarksGrace { get; set; }

        [Column("Marks_Total", TypeName = "int")]
        public Int32? MarksTotal { get; set; }

        [Column("Result_Grade_ID", TypeName = "int")]
        public Int32? ResultGradeID { get; set; }

        [ForeignKey("ResultGradeID")]
        public virtual ResultGrade Grade { get; set; }

        [Column("Updated_By", TypeName = "int")]
        public Int32? UpdatedByID { get; set; }

        [ForeignKey("UpdatedByID")]
        public virtual URM.User UpdatedByUser { get; set; }

        [Column("Updated_On")]
        public DateTime? UpdatedOn { get; set; }

        [Column("Is_Canceled", TypeName = "bit")]
        public Boolean? IsCanceled { get; set; }

        [Column("Cancel_Remarks")]
        [MaxLength(50)]
        public String CancelRemarks { get; set; }

        //added on  16  amy  2024  for CHM(T) O  Level Project
        [Column("Project_Title")]
        [MaxLength(1000)]
        public String ProjectTitle { get; set; }
        //
        //added 
        [Column("Project_Receipt_Date")]
        public DateTime? ProjectReceiptDate { get; set; }
        //added

        [Column("Canceled_On")]
        public DateTime? CanceledOn { get; set; }

        [Column("Institute_ID", TypeName = "bigint")]
        public Int64? InstituteID { get; set; }

        [ForeignKey("InstituteID")]
        public virtual Institute Institute { get; set; }     
        
        [Column("Exam_Month", TypeName = "int")]
        public Int32? ExamMonth { get; set; }

        [Column("Exam_Year", TypeName = "int")]
        public Int32? ExamYear { get; set; }

        //[Column("Exam_ID", TypeName = "int")]
        //public Int32? ExamID { get; set; }

        //[ForeignKey("ExamID")]
        //public virtual Exam Exam { get; set; }

        [Column("Old_Marks_Objective", TypeName = "int")]
        public Int32? OldMarksObjective { get; set; }

        [Column("Old_Marks_Descriptive", TypeName = "int")]
        public Int32? OldMarksDescriptive { get; set; }

        [Column("Old_Marks_Grace", TypeName = "int")]
        public Int32? OldMarksGrace { get; set; }

        [Column("Old_Marks_Total", TypeName = "int")]
        public Int32? OldMarksTotal { get; set; }

        [Column("Old_Result_Grade_ID", TypeName = "int")]
        public Int32? OldResultGradeID { get; set; }

        [Column("Pr_Batch_No")]
        public String PracticalExamBatchNumber { get; set; }

        [Column("Pr_Date")]
        public DateTime? PracticalExamDate { get; set; }

        [Column("Pr_Rept_Time")]
        public String PracticalExamReportingTime { get; set; }

        [Column("Commencement_exam_id", TypeName = "int")]
        public Int32? ExamID { get; set; }
        
    }
}
