using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class NielitProjects
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int32 ID { get; set; }

        [Column("schemeCode")]
        [MaxLength(5)]
        [Display(Name = "SchemeCode")]
        public String schemeCode { get; set; }

        [Column("projectName")]
        [Required(ErrorMessage = "Project Name is Required")]
        [MaxLength(200)]
        [Display(Name = "ProjectName")]
        public String ProjectName { get; set; }

        [Column("projectDescription")]
        [Required(ErrorMessage = "projectDescription is Required")]
        [MaxLength(1000)]
        [Display(Name = "projectDescription")]
        public String projectDescription { get; set; }

        [Column("projectDurationMonths", TypeName = "int")]
        public Int32 projectDurationMonths { get; set; }

        [Column("projectFromDate")]
        public DateTime projectFromDate { get; set; }

        [Column("projectTodate")]
        public DateTime projectTodate { get; set; }

        [Column("fundedByOrg")]
        [MaxLength(200)]
        [Display(Name = "Funded By Org")]
        public String fundedByOrg { get; set; }

        [Column("budgetAllocated", TypeName = "bigint")]
        [Required(ErrorMessage = "BudgetAllocated is Required")]
        public Int32 budgetAllocated { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        [Display(Name = "User Name")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }

        [Column("isAadharAuthenticationReqd", TypeName = "bit")]
        public bool isAadharAuthenticationReqd { get; set; }

        [Column("aadharOrderNo")]
        [MaxLength(50)]
        [Display(Name = "Aadhar OrderNo")]
        public String aadharOrderNo { get; set; }

        [Column("aadharOrderDate")]
        public DateTime? aadharOrderDate { get; set; }

    }
}

