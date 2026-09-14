using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class NonWorkingDays
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64? ID { get; set; }

        [Column("nonWorkingDate")]
        public DateTime NonWorkingDate { get; set; }

        [Column("nonWorkingDay")]
        [MaxLength(20)]
        public String NonWorkingDay { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        public Int64? EnterBy { get; set; }

        [Column("enterDate")]
        public DateTime EnterDate { get; set; }
    }
}
