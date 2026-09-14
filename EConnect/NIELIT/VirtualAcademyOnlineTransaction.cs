using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class VirtualAcademyOnlineTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Request_Date")]
        public DateTime RequestDate { get; set; }

        [Column("Demand_Note_ID", TypeName = "bigint")]
        public Int64 DemandNoteID { get; set; }

        [Column("Amount", TypeName = "decimal")]
        public Decimal Amount { get; set; }

        [Column("Request_Parameters")]
        [MaxLength(300)]
        public String RequestParameters { get; set; }

        [Column("Response_Parameters")]
        [MaxLength(400)]
        public String ResponseParameters { get; set; }

        [Column("Response_Date")]
        public DateTime? ResponseDate { get; set; }

        [Column("Reference_Number")]
        [MaxLength(20)]
        public String ReferenceNumber { get; set; }

        [Column("Response_Status_Code")]
        [MaxLength(5)]
        public String ResponseStatusCode { get; set; }

        [Column("Response_Status_Message")]
        [MaxLength(50)]
        public String ResponseStatusMessage { get; set; }

        [Column("Is_Settled", TypeName = "bit")]
        public Boolean IsSettled { get; set; }

        [Column("Settled_By", TypeName = "int")]
        public Int32? SettledBy { get; set; }

        [Column("Settled_On")]
        public DateTime? SettledOn { get; set; }

        [Column("Settled_FileName")]
        [MaxLength(300)]
        public String SettledFileName { get; set; }
    }
}
