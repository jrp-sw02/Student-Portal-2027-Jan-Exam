using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class PuraskarAppDocsVerificationAndDeclarationByInstt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", TypeName = "bigint")]
        public Int64 Id { get; set; }

        [Column("RegnNo", TypeName = "bigint")]
        public Int64? RegnNo { get; set; }

        [Column("ExamID", TypeName = "bigint")]
        public Int64? ExamID { get; set; }

        [Column("DocsVerifiedByInstt", TypeName = "bit")]
        public Boolean DocsVerifiedByInstt { get; set; }

        [Column("DocsVerifiedByInsttUser", TypeName = "bigint")]
        public Int64? DocsVerifiedByInsttUser { get; set; }

        [Column("DocsVerifiedByInsttOn")]
        public DateTime? DocsVerifiedByInsttOn { get; set; }

        [Column("DeclarationByInstt", TypeName = "bit")]
        public Boolean DeclarationByInstt { get; set; }

        [Column("DeclarationByInsttUser", TypeName = "bigint")]
        public Int64? DeclarationByInsttUser { get; set; }

        [Column("DeclarationByInsttOn")]
        public DateTime? DeclarationByInsttOn { get; set; }

       
       
    }
}
