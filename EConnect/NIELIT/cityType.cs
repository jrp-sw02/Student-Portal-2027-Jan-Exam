using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
   public class cityType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("cityType")]
        [Required(ErrorMessage = "City Type is required")]
        [MaxLength(1)]
        [Display(Name = "City Type")]
        public String cityTypeName { get; set; }

        [Column("Description")]
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(50)]
        [Display(Name = "Description")]
        public String description { get; set; }

        [Column("enterBy", TypeName = "int")]
        public Int32? enterBy { get; set; }

        [Column("enterDate", TypeName = "DateTime")]
        public DateTime? enterDate { get; set; }
        //Added 8 Feb 2019 for performance criterai
        public virtual ICollection<perfoCriteriaMas> perfoCriteriaDetails { get; set; }
        public virtual ICollection<Institute > institutes { get; set; }
      //  public virtual ICollection<InstituteHistory > institutesHistory { get; set; }
        public virtual ICollection<perfoReport > perfoReports { get; set; }
    }
}
