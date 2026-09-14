using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class ProjectCategoryMaster
    {
        [Key]
        [Column("ID", TypeName = "bigint")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ID { get; set; }

        [Column("ProjectCategoryName")]
        [Required(ErrorMessage = "Project Category Name is required")]
        [MaxLength(300)]
        [Display(Name = "Project Category Name")]
        public string ProjectCategoryName { get; set; }

        [Column("ProjectCategoryCode")]
        [Required(ErrorMessage = "Project Category Code is required")]
        [MaxLength(100)]
        [Display(Name = "Project Category Code")]
        public string ProjectCategoryCode { get; set; }

        [Column("Description")]
        [MaxLength(300)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Column("EnterBy", TypeName = "bigint")]
        [Required(ErrorMessage = "Enter By is required")]
        [Display(Name = "Enter By")]
        public Int64 EnterBy { get; set; }

        [Column("EnterDate")]
        [Required(ErrorMessage = "Enter Date is required")]
        [Display(Name = "Enter Date")]
        public DateTime EnterDate { get; set; }
    }
}
