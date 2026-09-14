using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class projDataUploadAllowed
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("projID", TypeName = "bigint")]
        public long projID { get; set; }

        [Column("effectiveFrom", TypeName = "date")]
        public DateTime effectiveFrom { get; set; }

        [Column("effectiveTo", TypeName = "date")]
        public DateTime effectiveTo { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        public long enterBy { get; set; }

        [Column("enterdate", TypeName = "date")]
        public DateTime enterdate { get; set; }

    }

}