using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class OnlineRefund
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Transation_ID", TypeName = "int")]
        public Int32 TransactionID { get; set; }

        [Column("Bank_ID")]
        [MaxLength(50)]
        public String BankID { get; set; }

        [Column("Txt_Ref_No")]
        [MaxLength(50)]
        public String Txt_Ref_No { get; set; }

        [Column("Bank_Ref_No")]
        [MaxLength(50)]
        public String Bank_Ref_No { get; set; }

        [Column("Transaction_Date")]
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [Column("Transaction_Amount", TypeName = "bigint")]
        public Int64 TransactionAmount { get; set; }

        [Column("Product_ID")]
        [MaxLength(50)]
        public String ProductID { get; set; }

        [Column("Refund_Date")]
        [Display(Name = "Refund Date")]
        public DateTime RefundDate { get; set; }

        [Column("Refund_Amount", TypeName = "bigint")]
        public Int64 Refund_Amount { get; set; }

        [Column("BillDesk_Refund_Date")]
        [Display(Name = "Bill Desk Refund Date")]
        public DateTime? BillDeskRefundDate { get; set; }

        [Column("BillDesk_Refund_ID", TypeName = "bigint")]
        public Int64? BillDeskRefundID { get; set; }

        [ForeignKey("TransactionID")]
        public virtual OnlineTransaction onlineTransaction { get; set; }

    }
}
