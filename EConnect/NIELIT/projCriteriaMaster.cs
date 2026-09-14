using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class projCriteriaMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int32 ID { get; set; }

        [Column("projID", TypeName = "bigint")]
        public long? projID { get; set; }

        [Column("field1")]
        [MaxLength(400)]
        public string field1 { get; set; }

        [Column("field2")]
        [MaxLength(400)]
        public string field2 { get; set; }

        [Column("field3")]
        [MaxLength(400)]
        public string field3 { get; set; }

        [Column("field4")]
        [MaxLength(400)]
        public string field4 { get; set; }

        [Column("field5")]
        [MaxLength(400)]
        public string field5 { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        public long? enterby { get; set; }

        [Column("enterDate", TypeName = "date")]
        public DateTime? enterDate { get; set; }   
    }
}
