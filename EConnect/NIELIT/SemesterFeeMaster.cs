using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
   public class SemesterFeeMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int32 ID { get; set; }

        [Column("courseid")]
        [Required(ErrorMessage = "courseid is required")]
        [Display(Name = "courseid")]
        public Int64 courseid { get; set; }

        [Column("batchID")]
        [Required(ErrorMessage = "Batch is required")]
        [Display(Name = "Batch")]
        public Int64 batchID { get; set; }

        [Column("SemId")]
        [Required(ErrorMessage = "SemId is required")]
        [Display(Name = "SemId")]
        public Int64 SemId { get; set; }

        [Column("feeTypeID")]
        [Required(ErrorMessage = "Fee type is required")]
        [Display(Name = "Fee type")]
        public Int64 feeTypeID { get; set; }

        [Column("feeAmount", TypeName = "bigint")]
        [Display(Name = "Fee Amount")]
        public Int64 feeAmount { get; set; }

        [Column("effectiveFromDate")]
        public DateTime effectiveFromDate { get; set; }

        [Column("effectiveToDate")]
        public DateTime effectiveToDate { get; set; }

        [Column("enterBy", TypeName = "int")]
        [Display(Name = "User Name")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }

    }
}
