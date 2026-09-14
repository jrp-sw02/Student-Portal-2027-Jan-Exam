using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class perfoReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

       
        [Column("instituteID", TypeName = "bigint")]
        public Int64 instituteID { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("cityTypeID")]
        [Required(ErrorMessage = "City Type is required")]
        [Display(Name = "City Type")]
        public int cityTypeID { get; set; }


        [Column("accrNo")]
        [Required(ErrorMessage = "Accredirtation no. is required")]
        [Display(Name = "Accreditation No.")]
        public string accrNo { get; set; }

        [Column("validFrom")]
        [Required(ErrorMessage = "Valid From is required")]
        [Display(Name = "Valid From")]
        public DateTime  validFrom { get; set; }

        [Column("validUpto")]
        [Required(ErrorMessage = "Valid Upto is required")]
        [Display(Name = "Valid Upto")]
        public DateTime validUpto { get; set; }

        [Column("examsCheckedCount")]
        [Required(ErrorMessage = "Exams Checked Count is required")]
        [Display(Name = "Exams Checked Count")]
        public int examsCheckedCount { get; set; }

        [Column("candFielded")]
        [Display(Name = "Candidates Fielded")]
        public int candFielded { get; set; }

        [Column("candAppeared")]
        [Display(Name = "Candidates Appeared")]
        public int candAppeared { get; set; }

        [Column("candPassed")]
        [Display(Name = "Candidates Passed")]
        public int candPassed { get; set; }

        [Column("passPercent")]
        [Display(Name = "Pass percentage")]
        public decimal passPercent { get; set; }

        [Column("whetherEligible")]
        [Display(Name = "Whether Eligible")]
        public string whetherEligible { get; set; }

        [Column("reportGroupID")]
        [Display(Name = "Report Group")]
        public Int64  reportGroupID { get; set; }

        [Column("expiryGenUptoDate")]
        public DateTime expiryGenUptoDate { get; set; }

        [Column("perfoCriteriaID")]
        [Display(Name = "Criteria Applied")]
        public Int64 perfoCriteriaID { get; set; }

       [Column("enterBy", TypeName = "int")]
        public Int32? enterBy { get; set; }

        [Column("enterDate", TypeName = "DateTime")]
        public DateTime? enterDate { get; set; }

        [ForeignKey("cityTypeID")]
        public virtual cityType parentCityType { get; set; }

        [ForeignKey("instituteID")]
        public virtual Institute  parentInstitute { get; set; }

        [ForeignKey("perfoCriteriaID")]
        public virtual perfoCriteriaMas  parentCriteria { get; set; }
    }
}
