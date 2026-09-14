using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
  public class NEFTRefund
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Date")]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Column("Refund_Date")]
        [Display(Name = "Refund Date")]
        public DateTime RefundDate { get; set; }

        [Column("Refund_Amount", TypeName = "bigint")]
        public Int64 RefundAmount { get; set; }

        [Column("UTR_Number")]
        [MaxLength(50)]
        public String UTRNumber { get; set; }

        [Column("Account_Number")]
        [MaxLength(100)]
        public String AccountNumber { get; set; }

        [Column("Account_Holder_Name")]
        [MaxLength(200)]
        public String AccountHolderName { get; set; }

        [Column("Account_Type")]
        [MaxLength(100)]
        public String AccountType { get; set; }

        [Column("IFSC_Code")]
        [MaxLength(150)]
        public String IFSCCode { get; set; }

        [Column("Refund_Reason")]
        [MaxLength(250)]
        public String RefundReason { get; set; }

        [Column("Cheque_Number", TypeName = "bigint")]
        public Int64? ChequeNumber { get; set; }

        [Column("Created_By", TypeName = "int")]
        public Int32 CreatedBy { get; set; }

        [Column("Refund_Mode", TypeName = "int")]
        public Int32 RefundMode { get; set; }

        [Column("Neft_Bank_Transaction_ID", TypeName = "int")]
        public Int32 NeftBankTransactionID { get; set; }

        [Column("Cheque_Issuer_Name")]
        [MaxLength(100)]
        public String ChequeIssuerName { get; set; }

        [ForeignKey("NeftBankTransactionID")]
        public virtual NEFTBankTransaction NeftBankTransaction { get; set; }
    }
}
