using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class ExamCycleExceptionalFeature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "int")]
        public Int32 id { get; set; }

        [Column("exam_id")]
        [Required(ErrorMessage = "exam  id is required")]
        [Display(Name = "exam  id")]
        public Int32 exam_id { get; set; }

        [Column("course_id")]
        [Required(ErrorMessage = "course id is required")]
        [Display(Name = "course_id")]
        public Int32 course_id { get; set; }
        
        [Column("whether_effective")]
        [Required(ErrorMessage = "whether effective is required")]
        [MaxLength(1)]
        [Display(Name = "whether effective")]
        public String whether_effective { get; set; }

        [Column("effective_from_date")]
        [Required(ErrorMessage = "effective from date is required")]
        public DateTime effective_from_date { get; set; }

        [Column("effective_upto_date")]
        [Required(ErrorMessage = "effective upto date is required")]
        public DateTime effective_upto_date { get; set; }

        [Column("remarks")]
        [Required(ErrorMessage = "remarks is required")]
        [MaxLength(1000)]
        [Display(Name = "remarks")]
        public String remarks { get; set; }
    }
}
