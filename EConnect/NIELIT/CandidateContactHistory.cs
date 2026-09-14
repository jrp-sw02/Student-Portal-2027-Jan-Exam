using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CandidateContactHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Candidate_ID", TypeName = "bigint")]
        public Int64 CandidateID { get; set; }

        [Column("Mobile", TypeName = "bigint")]
        public Int64? MobileNumber { get; set; }

        [Column("Std", TypeName = "int")]
        public Int32? StdNumber { get; set; }

        [Column("Phone", TypeName = "int")]
        public Int32? PhoneNumber { get; set; }

        [Column("Email")]
        [MaxLength(100)]
        public String EmailAddress { get; set; }

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
