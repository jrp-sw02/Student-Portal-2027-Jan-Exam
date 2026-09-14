using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class projectMainCentre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("projectID", TypeName = "bigint")]
        public Int64 projectID { get; set; }

        [Column("centreID", TypeName = "bigint")]
        public Int64 centreID { get; set; }

        [Column("allocatedFromDate")]
        public DateTime allocatedFromDate { get; set; }

        [Column("allocatedTodate")]
        public DateTime allocatedTodate { get; set; }

        [Column("budgetAllocated", TypeName = "bigint")]
        public Int64 budgetAllocated { get; set; }

        [Column("isAadharAuthenticationReqd", TypeName = "bit")]
        public Boolean isAadharAuthenticationReqd  { get; set; }

        [Column("enterBy", TypeName = "int")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }

        //public virtual ICollection<NielitProjects> NielitProjects { get; set; }
        //public virtual ICollection<NielitCentres> NielitCentres { get; set; }

    }
}
