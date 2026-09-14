using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CourseExamApplicationDetail
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

        [Column("Online_Exam_Login_ID", TypeName = "varchar"),MaxLength(20)]
        public string OnlineExamLoginID { get; set; }

        [Column("Venue_Code", TypeName = "varchar"), MaxLength(10)]
        public string VenueCode { get; set; }

        [Column("Venue_Name", TypeName = "varchar"), MaxLength(100)]
        public string VenueName { get; set; }

        [Column("Venue_City_Name", TypeName = "varchar"), MaxLength(100)]
        public string VenueCityName { get; set; }

        [Column("Venue_Address", TypeName = "varchar"), MaxLength(300)]
        public string VenueAddress { get; set; }

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



        [Column("Marks_Objective", TypeName = "decimal")]
        public Decimal? MarksObjective { get; set; }

        [Column("Marks_Descriptive", TypeName = "decimal")]
        public Decimal? MarksDescriptive { get; set; }

        [Column("Marks_Grace", TypeName = "decimal")]
        public Decimal? MarksGrace { get; set; }

        [Column("Marks_Total", TypeName = "decimal")]
        public Decimal? MarksTotal { get; set; }




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

        [Column("Canceled_On")]
        public DateTime? CanceledOn { get; set; }

        [Column("Institute_ID", TypeName = "bigint")]
        public Int64? InstituteID { get; set; }

        [ForeignKey("InstituteID")]
        public virtual Institute Institute { get; set; }

        [Column("Roll_Number", TypeName = "bigint")]
        public Int64? RollNumber { get; set; }

        [Column("Exam_Centre_ID", TypeName = "int")]
        public Int32? ExamCentreID { get; set; }

        [ForeignKey("ExamCentreID")]
        public virtual ExamCenter ExamCenter { get; set; }

        [Column("Exam_Venue_ID", TypeName = "int")]
        public Int32? ExamVenueID { get; set; }

        [ForeignKey("ExamVenueID")]
        public virtual ExamVenue ExamVenue { get; set; }

        [Column("Exam_Month", TypeName = "int")]      
        public Int32? ExamMonth { get; set; }

        [Column("Exam_Year", TypeName = "int")]       
        public Int32? ExamYear { get; set; }

        [Column("Exam_ID", TypeName = "int")]
        public Int32 ExamID { get; set; }

        [ForeignKey("ExamID")]
        public virtual Exam Exam { get; set; }




        [Column("Old_Marks_Objective", TypeName = "decimal")]
        public Decimal? OldMarksObjective { get; set; }

        [Column("Old_Marks_Descriptive", TypeName = "decimal")]
        public Decimal? OldMarksDescriptive { get; set; }

        [Column("Old_Marks_Grace", TypeName = "decimal")]
        public Decimal? OldMarksGrace { get; set; }

        [Column("Old_Marks_Total", TypeName = "decimal")]
        public Decimal? OldMarksTotal { get; set; }




        [Column("Old_Result_Grade_ID", TypeName = "int")]
        public Int32? OldResultGradeID { get; set; }

        [Column("Pr_Batch_No")]
        public String PracticalExamBatchNumber { get; set; }

        [Column("Pr_Date")]
        public DateTime? PracticalExamDate { get; set; }

        [Column("Pr_Rept_Time")]
        public String PracticalExamReportingTime { get; set; }

        [Column("carry_forward_remarks")]
        public String carryforwardremarks { get; set; }


        [Column("practical_marks_out_of_100_new_pattern", TypeName = "decimal")]
        public Decimal? practicalmarksoutof100newpattern { get; set; }

        [Column("Theory_marks_out_of_100_new_pattern", TypeName = "decimal")]
        public Decimal? Theorymarksoutof100newpattern { get; set; }

        [Column("question_paper_booklet_series", TypeName = "decimal")]
        public Decimal? questionpaperbookletseries { get; set; }




    }
}
