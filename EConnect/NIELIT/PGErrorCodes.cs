using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class PGErrorCodes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public int ID { get; set; }

        [Column("PGId", TypeName = "int")]
        public int PGId { get; set; }

        [Column("PGErrorCode ")]
        [MaxLength(10)]
        public String PGErrorCode { get; set; }

        [Column("Description")]
        [MaxLength(500)]
        public String Description { get; set; }

        [Column("enterBy", TypeName = "int")]
        public Int32? enterBy { get; set; }

        [Column("enterDate")]
        public DateTime? enterDate { get; set; }

        
    }
}
