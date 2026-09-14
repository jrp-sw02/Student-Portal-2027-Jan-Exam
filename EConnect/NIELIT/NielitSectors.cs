using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class NielitSectors
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("sectorShortCode")]
        [Required(ErrorMessage = "Sector Short Code is required")]
        [MaxLength(10)]
        [Display(Name = "Sector Short Code")]
        public String sectorShortCode { get; set; }

        [Column("sectorName")]
        [Required(ErrorMessage = "Sector Name is required")]
        [MaxLength(50)]
        [Display(Name = "Sector Name Code")]
        public String sectorName { get; set; }

        [Column("sectorDescription")]
        [MaxLength(100)]
        [Display(Name = "Sector Description")]
        public String sectorDescription { get; set; }

        [Column("enterBy", TypeName = "int")]
        [Display(Name = "User Name")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }

    }
}
