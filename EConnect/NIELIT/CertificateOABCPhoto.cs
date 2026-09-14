using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CertificateOABCPhoto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Candidate_ID", TypeName = "bigint")]
        public Int64 CandidateID { get; set; }

        [Column("Registration_No", TypeName = "bigint")]
        public Int64 RegistrationNo { get; set; }

        [Column("Level_Code")]     
        public String LevelCode { get; set; }

        [Column("Certificate_Phase_No", TypeName = "int")]
        public Int32 CertificatePhaseNo { get; set; }

        [Column("Certificate_No", TypeName = "bigint")]
        public Int64? CertificateNo { get; set; }

        [Column("Photo_uploaded_file_Id", TypeName = "bigint")]
        public Int64? PhotofileId { get; set; }

        [Column("Sign_uploaded_file_Id", TypeName = "bigint")]
        public Int64? SignfileId { get; set; }
    }
}
