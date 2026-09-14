using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CandidateQualificationDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Candidate_ID", TypeName = "bigint")]
        public Int64 CandidateID { get; set; }

        [Column("Educational_Qualification_ID", TypeName = "int")]
        public Int32 EducationalQualificationID { get; set; }

        [Column("Passing_Year", TypeName = "int")]
        public Int32? PassingYear { get; set; }

        [Column("Effective_From_Date")]
        public DateTime EffectiveFromDate { get; set; }

        public virtual Candidate Candidate { get; set; }

        public virtual EducationalQualification EducationalQualification { get; set; }
    }
}
