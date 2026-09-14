using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class CourseWiseAmtPerPaperForOnlinePuraskarApp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int32 ID { get; set; }

        [Column("courseCode")]
        [Required(ErrorMessage = "Course Name is required")]
        [MaxLength(1)]
        [Display(Name = "Course  Name")]
        public String courseCode { get; set; }

        [Column("AmountPerPaper", TypeName = "int")]
        [Required(ErrorMessage = "Fee type is Required")]
        [Display(Name = "Fee type")]
        public Int32 AmountPerPaper { get; set; }

        [Column("EffectiveFromDate")]
        public DateTime EffectiveFromDate { get; set; }

        [Column("EffectiveToDate")]
        public DateTime? EffectiveToDate { get; set; }

        [Column("EnterBy", TypeName = "bigint")]
        [Display(Name = "User Name")]
        public Int32 EnterBy { get; set; }

        [Column("EnterDate")]
        public DateTime EnterDate { get; set; }

       

    }
}
