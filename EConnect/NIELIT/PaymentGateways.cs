using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class PaymentGateways
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public int ID { get; set; }

        [Column("Code")]
        [MaxLength(10)]
        public String Code { get; set; }

        [Column("Description")]
        [MaxLength(500)]
        public String Description { get; set; }

        [Column("Request_Date")]
        public DateTime RequestDate { get; set; }
               

        [Column("requestUrl")]
        [MaxLength(500)]
        public String requestUrl { get; set; }

        [Column("merchantID")]
        [MaxLength(10)]
        public String merchantID { get; set; }

        
        [Column("enterBy", TypeName = "int")]
        public Int32? enterBy { get; set; }

        [Column("enterDate")]
        public DateTime? enterDate { get; set; }

        
    }
}
