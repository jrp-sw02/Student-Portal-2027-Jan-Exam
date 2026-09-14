using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class NielitTrgSpecialization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("NielitSectorID", TypeName = "int")]
        [Required]
        public Int32 NielitSectorID { get; set; }

        [Column("specializationCode")]
        [Required(ErrorMessage = "Specialization Code is required")]
        [MaxLength(10)]
        [Display(Name = "Specialization Code")]
        public String specializationCode { get; set; }

        [Column("specializationName")]
        [Required(ErrorMessage = "Specialization Name is required")]
        [MaxLength(50)]
        [Display(Name = "Specialization Name")]
        public String specializationName { get; set; }

        [Column("specializationDetails")]
        [MaxLength(100)]
        [Display(Name = "Specialization Details")]
        public String specializationDetails { get; set; }

        [Column("enterBy", TypeName = "int")]
        [Display(Name = "User Name")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }

        [ForeignKey("NielitSectorID")]
        public virtual Batch Batch { get; set; }

        [ForeignKey("NielitSectorID")]
        public virtual NielitSectors NielitSectors { get; set; }

    }
}
