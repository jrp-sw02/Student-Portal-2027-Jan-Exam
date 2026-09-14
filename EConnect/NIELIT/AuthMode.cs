using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class AuthMode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public long ID { get; set; }

        [Column("authCode")]
        public string authCode { get; set; }

        [Column("authDesc")]
        public string authDesc { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        public long? enterBy { get; set; }

        [Column("enterDate", TypeName = "datetime")]
        public DateTime? enterDate { get; set; }
    }
}