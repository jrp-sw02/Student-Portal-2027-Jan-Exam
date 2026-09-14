using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CSCTransaction
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

        [Column("Amount", TypeName = "decimal")]
        public Decimal Amount { get; set; }

        [Column("Response_Number")]
        [MaxLength(30)]
        public String ResponseTransactionNumber { get; set; }

        [Column("Response_Message")]
        [MaxLength(200)]
        public String ResponseMessage { get; set; }

        [Column("Response_Status", TypeName="int")]
        public Int32?  ResponseStatus { get; set; }

        [Column("Created_By", TypeName = "int")]
        public Int32? CreatedByID { get; set; }
    }
}
