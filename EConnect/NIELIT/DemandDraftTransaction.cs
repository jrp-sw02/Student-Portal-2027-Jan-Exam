using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class DemandDraftTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Date")]
        public DateTime Date { get; set; }

        [Column("Demand_Note_ID", TypeName = "bigint")]
        [Required]
        public Int64 DemandNoteID { get; set; }

        [Column("Dd_Number")]
        [Required]
        [MaxLength(20)]
        public String DemandDraftNumber { get; set; }

        [Column("DD_Date")]
        [Required]
        public DateTime DemandDraftDate { get; set; }

        [Column("DD_Amt", TypeName = "decimal")]
        public Decimal DemandDraftAmount { get; set; }

        [Column("DD_Bank")]
        [Required]
        [MaxLength(50)]
        public String IssuingBankName { get; set; }

        [Column("Is_Verified", TypeName = "bit")]
        public Boolean  IsVerified { get; set; }

        [Column("Verified_By", TypeName = "int")]
        public Int32? VerifiedByID { get; set; }

        [Column("Verified_On")]
        public DateTime? VerificatonDate { get; set; }

        [Column("Created_By", TypeName = "int")]
        public Int32? CreatedByID { get; set; }
    }
}
