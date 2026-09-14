using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class PuraskarFeeReimbursementInstalment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("feeReimbMasterId", TypeName = "bigint")]
        public Int64 feeReimbMasterId { get; set; }

        [Column("courseID", TypeName = "bigint")]
        public Int64 courseID { get; set; }

        [Column("instalmentNo", TypeName = "int")]
        public Int32 instalmentNo { get; set; }

        [Column("paperCount", TypeName = "int")]
        public Int32 paperCount { get; set; }

        [Column("instalmentAmt", TypeName = "bigint")]
        public Int64 instalmentAmt { get; set; }

        [Column("effectiveFrom")]
        [Display(Name = "effective From")]
        public DateTime effectiveFrom { get; set; }

        [Column("effectiveTo")]
        [Display(Name = "effective End Date")]
        public DateTime? effectiveTo { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        public Int64 enterBy { get; set; }

        [Column("enterDate")]
        [Display(Name = "enterDate")]
        public DateTime enterDate { get; set; }

    }
}
