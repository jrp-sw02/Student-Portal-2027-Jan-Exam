using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
  public  class projectSubCentre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int32 ID { get; set; }

        [Column("projectID", TypeName = "bigint")]
        [Required(ErrorMessage = "projectID is Required")]
        public Int32 projectID { get; set; }

        [Column("centreID", TypeName = "bigint")]
        [Required(ErrorMessage = "centreID is Required")]
        public Int32 centreID { get; set; }

        [Column("allocatedFromDate")]
        [Required(ErrorMessage = "allocatedFromDate is Required")]
        public DateTime allocatedFromDate { get; set; }

        [Column("allocatedTodate")]
        [Required(ErrorMessage = "allocatedTodate is Required")]
        public DateTime allocatedTodate { get; set; }

        [Column("budgetAllocated", TypeName = "bigint")]
        public Int32? budgetAllocated { get; set; }

        [Column("whetherAffiliated", TypeName = "bit")]
        public bool whetherAffiliated { get; set; }

        [Column("enterBy", TypeName = "int")]
        public int enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }

        //public virtual ICollection<NielitProjects> NielitProjectss { get; set; }
    }
}
