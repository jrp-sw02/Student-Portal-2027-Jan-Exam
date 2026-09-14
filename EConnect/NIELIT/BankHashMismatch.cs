using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class BankHashMismatch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public int ID { get; set; }

        [Column("transID", TypeName = "int")]
        public int transID { get; set; }

        [Column("transDate")]
        public DateTime transDate { get; set; }

        [Column("amount", TypeName = "decimal")]
        public decimal? amount { get; set; }

        [Column("responseParameters")]
        [MaxLength(500)]
        public String responseParameters { get; set; }

        [Column("hashGenerated")]
        [MaxLength(500)]
        public String hashGenerated { get; set; }

        [Column("hashRecd")]
        [MaxLength(500)]
        public String hashRecd { get; set; }
                
        [Column("enterBy", TypeName = "int")]
        public Int32? enterBy { get; set; }

        [Column("enterDate")]
        public DateTime? enterDate { get; set; }

        
    }
}
