using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class NielitCentreBatchFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int32 ID { get; set; }

        [Column("batchID")]
        [Required(ErrorMessage = "Batch is required")]
        [Display(Name = "Batch")]
        public Int64 batchID { get; set; }

        [Column("feeTypeID")]
        [Required(ErrorMessage = "Fee type is required")]
        [Display(Name = "Fee type")]
        public Int64 feeTypeID { get; set; }

        [Column("feeAmount", TypeName = "int")]
        [Display(Name = "Fee Amount")]
        public Int32 feeAmount { get; set; }

        [Column("effectiveFromDate")]
        public DateTime effectiveFromDate { get; set; }

        [Column("effectiveToDate")]
        public DateTime? effectiveToDate { get; set; }

        [Column("enterBy", TypeName = "int")]
        [Display(Name = "User Name")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }
    }
}
