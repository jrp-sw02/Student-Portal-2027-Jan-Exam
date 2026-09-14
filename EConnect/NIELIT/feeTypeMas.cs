using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class feeTypeMas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("feeType")]
        [Required(ErrorMessage = "Fee Type is required")]
        [MaxLength(150)]
        [Display(Name = "Fee Type")]
        public String feeType { get; set; }

        [Column("Description")]
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500)]
        [Display(Name = "Description Type")]
        public String Description { get; set; }

        [Column("enterBy", TypeName = "int")]
        [Display(Name = "User Name")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }
    }
}
