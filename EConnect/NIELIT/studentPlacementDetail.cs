using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class studentPlacementDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("studentID", TypeName = "bigint")]
        [Required(ErrorMessage = "Student ID is required")]
        [Display(Name = "studentID")]
        public Int64 studentID { get; set; }

        [Column("companyID", TypeName = "bigint")]
        [Required(ErrorMessage = "Company ID is required")]
        [Display(Name = "Company")]
        public Int64 companyID { get; set; }

        [Column("designation")]
        [Required(ErrorMessage = "Designation is required")]
        [MaxLength(1000)]
        [Display(Name = "Designation")]
        public String designation { get; set; }

        [Column("dateFrom")]
        [Display(Name = "Effective From Date")]
        public DateTime? EffectiveFromDate { get; set; }

        [Column("dateTo")]
        [Display(Name = "Effective To Date")]
        public DateTime? EffectiveToDate { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        public Int64 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }

    }
}
