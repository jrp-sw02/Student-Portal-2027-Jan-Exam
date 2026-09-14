using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class batchSize
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("projectID", TypeName = "bigint")]
        public Int64 ProjectID { get; set; }

        [Column("courseID", TypeName = "bigint")]
        public Int64 CourseID { get; set; }

        [Column("centreID", TypeName = "bigint")]
        public Int64 CentreID { get; set; }

        [Column("maxCandInBatch", TypeName = "bigint")]
        public Int64 MaxCandInBatch { get; set; }

        [Column("effectiveFrom", TypeName = "date")]
        public DateTime? EffectiveFrom { get; set; }

        [Column("effectiveTo", TypeName = "date")]
        public DateTime? EffectiveTo { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        public Int64 EnterBy { get; set; }

        [Column("enterdate", TypeName = "datetime")]
        public DateTime? EnterDate { get; set; }


    }
}
