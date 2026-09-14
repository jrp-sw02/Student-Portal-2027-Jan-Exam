using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class PuraskarFeeReimbursementMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("courseID", TypeName = "bigint")]
        public Int64 courseID { get; set; }

        [Column("TotalInstalments", TypeName = "int")]
        public Int32 TotalInstalments { get; set; }

        [Column("TotalAmt", TypeName = "bigint")]
        public Int64 TotalAmt { get; set; }

        [Column("effectiveFrom")]
        [Display(Name = "effectiveFrom")]
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
