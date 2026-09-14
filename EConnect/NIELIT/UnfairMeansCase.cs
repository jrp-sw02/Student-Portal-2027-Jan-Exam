using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class UnfairMeansCase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Course_Exam_Appl_ID", TypeName = "bigint")]
        public Int64 CourseExamApplicationID { get; set; }

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

        [Column("Incident")]
        [Required]
        [MaxLength(500)]
        public String Incident { get; set; }

        [Column("Decision_ID", TypeName = "int")]
        public Int32? DecisionID { get; set; }

        [Column("Decision_Remarks")]
        [MaxLength(250)]
        public String DecisionRemarks { get; set; }

        [Column("Decision_Ref_No")]
        [MaxLength(30)]
        public String DecisionReferenceNumber { get; set; }

        [Column("Created_By", TypeName = "int")]
        public Int32 CreatedByID { get; set; }

        [Column("CreatedByID", TypeName = "int")]
        public virtual URM.User CreatedByUser { get; set; }

        [Column("Created_On")]
        public DateTime CreatedOn { get; set; }

        [Column("Updated_By", TypeName = "int")]
        public Int32? UpdatedByID { get; set; }

        [Column("UpdatedByID", TypeName = "int")]
        public virtual URM.User UpdatedByUser { get; set; }

        [Column("Updated_On")]
        public DateTime? UpdatedOn { get; set; }
    }
}
