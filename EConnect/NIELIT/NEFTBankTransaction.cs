using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
   public  class NEFTBankTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("UTR_NUMBER")]
        [MaxLength(50)]
        public String UTRNUMBER { get; set; }

        [Column("Sender_Ifsc")]
        [MaxLength(50)]
        public String SenderIfsc { get; set; }

        [Column("Sender_Name")]
        [MaxLength(50)]
        public String SenderName { get; set; }

        [Column("Transaction_ID")]
        [MaxLength(50)]
        public String TransactionID { get; set; }

        [Column("Transaction_Date")]
        public DateTime? TransactionDate { get; set; }

        [Column("Transaction_Amt", TypeName = "int")]
        public Int32 TransactionAmt { get; set; }

        [Column("Demand_Note_ID", TypeName = "bigint")]
        public Int64? DemandNoteID { get; set; }

        [Column("Uploaded_Date")]
        public DateTime? UploadedDate { get; set; }

        [ForeignKey("DemandNoteID")]
        public virtual DemandNote DemandNote { get; set; }

    }
}
