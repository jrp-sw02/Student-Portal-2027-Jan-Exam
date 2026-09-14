using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class verifyStatusMas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("verifyCode")]        
        [MaxLength(1)]        
        public String verifyCode { get; set; }

        [Column("verifyDescription")]
        [MaxLength(50)]        
        public String verifyDescription { get; set; }

        [Column("enterBy", TypeName = "int")]
        public int enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }
      
    }
}
