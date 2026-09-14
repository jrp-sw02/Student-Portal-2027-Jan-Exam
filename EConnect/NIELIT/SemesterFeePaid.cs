using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class SemesterFeePaid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("feeTypeID", TypeName = "bigint")]
        public Int64 feeTypeID { get; set; }

        [Column("studentID", TypeName = "bigint")]
        public Int64 studentID { get; set; }

        [Column("paymentDate")]
        public DateTime paymentDate { get; set; }

        [Column("AmtPaid", TypeName = "int")]
        public Int32 AmtPaid { get; set; }

        [Column("Remarks")]       
        [MaxLength(500)]
        [Display(Name = "Remarks")]
        public String Remarks { get; set; }

        [Column("enterBy", TypeName = "int")]
        [Display(Name = "User Name")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }
    }
}
