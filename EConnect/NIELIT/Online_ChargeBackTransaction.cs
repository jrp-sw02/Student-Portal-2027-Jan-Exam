using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
   public class Online_ChargeBackTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Date")]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Column("Biller_Name")]
        [MaxLength(100)]
        public String BillerName { get; set; }

        [Column("Debit_Type")]
        [MaxLength(70)]
        public String DebitType { get; set; }

        [Column("Pay_Mode")]
        [MaxLength(100)]
        public String PayMode { get; set; }

        [Column("Product_code")]
        [MaxLength(100)]
        public String Productcode { get; set; }

        [Column("BD_Reference_No")]
        [MaxLength(100)]
        public String BDReferenceNo { get; set; }

        [Column("BillDesk_ID")]
        [MaxLength(70)]
        public String BillDeskID { get; set; }

        [Column("Ref_1", TypeName = "int")]
        public Int32 Ref1 { get; set; }

        [Column("Ref_2", TypeName = "int")]
        public Int32 Ref2 { get; set; }

        [Column("Ref_3")]
        [MaxLength(70)]
        public String Ref3 { get; set; }

        [Column("Ref_4")]
        [MaxLength(70)]
        public String Ref4 { get; set; }

        [Column("Created_On")]
        public DateTime CreatedOn { get; set; }

        [Column("Transaction_Amount", TypeName = "bigint")]
        public Int64 TransactionAmount { get; set; }

        [Column("Refund_ID", TypeName = "bigint")]
        public Int64 RefundID { get; set; }

        [Column("Refund_Date")]
        public DateTime RefundDate { get; set; }

        [Column("Refund_Amount", TypeName = "bigint")]
        public Int64 RefundAmount { get; set; }
    }
}
