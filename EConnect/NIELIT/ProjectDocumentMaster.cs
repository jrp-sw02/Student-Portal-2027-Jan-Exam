using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class ProjectDocumentMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int32 ID { get; set; }

        [Column("project")]
        [Required(ErrorMessage = "Project is required")]
        [Display(Name = "Project")]
        public Int64 projectID { get; set; }

        [Column("documents")]
        [Required(ErrorMessage = "Documents is required")]
        [Display(Name = "Documents")]
        public String documentName { get; set; }

     
        [Column("effectiveFromDate")]
        public DateTime effectiveFrom { get; set; }

        [Column("effectiveToDate")]
        public DateTime? effectiveTo { get; set; }

        //[Column("enterBy", TypeName = "int")]
        //[Display(Name = "User Name")]
        //public Int32 enterBy { get; set; }

        //[Column("enterDate")]
        //public DateTime enterDate { get; set; }
    }
}
