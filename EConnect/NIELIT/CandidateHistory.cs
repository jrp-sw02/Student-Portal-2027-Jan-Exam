using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CandidateHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Candidate_ID", TypeName = "bigint")]
        public Int64 CandidateID { get; set; }

        [Column("Father_Name")]
        [MaxLength(60)]        
        [Display(Name = "Father's Name Name")]
        public String FatherName { get; set; }

        [Column("Mother_Name")]
        [MaxLength(60)]        
        [Display(Name = "Mother's Name Name")]
        public String MotherName { get; set; }

        [Column("Guardian_Name")]
        [MaxLength(60)]
        public String GuardianName { get; set; }

        [Column("Gender")]       
        [MaxLength(6)]
        [Display(Name = "Gender")]
        public String Gender { get; set; }

        [Column("Marital_Status_ID", TypeName = "int")]
        [Display(Name = "Marital Status")]
        public Int32? MaritalStatusID { get; set; }

        [ForeignKey("MaritalStatusID")]
        public virtual MaritalStatus MaritalStatus { get; set; }

        [Column("Created_On")]
        public DateTime CreatedOn { get; set; }

        [Column("Created_By", TypeName = "int")]
        public Int32 CreatedByID { get; set; }

        [ForeignKey("CandidateID")]
        public virtual Candidate Candidate { get; set; }

        [ForeignKey("CreatedByID")]
        public virtual URM.User CreatedByUSer { get; set; }
    }
}
