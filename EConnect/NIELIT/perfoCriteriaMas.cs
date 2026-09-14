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
    public class perfoCriteriaMas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("courseCatID")]
        [Required(ErrorMessage = "Course Category is required")]
        [Display(Name = "Course Category")]
        public int courseCatID { get; set; }

        [Column("cityTypeID")]
        [Required(ErrorMessage = "City Type is required")]
        [Display(Name = "City Type")]
        public int cityTypeID { get; set; }

        [Column("minCandidates")]
        [Required(ErrorMessage = "Minimum Candidates is required")]
        [Display(Name = "Minimum Candidates")]
        public int minCand { get; set; }

        [Column("passPercent")]
        [Required(ErrorMessage = "Pass percentage is required")]
        [Display(Name = "Pass percentage")]
        public int passPercent { get; set; }

        [Column("examCount")]
        [Required(ErrorMessage = "Exam Count is required")]
        [Display(Name = "Exam Count")]
        public int examCount { get; set; }

        [Column("effectiveFrom")]
        public DateTime effectiveFromDate { get; set; }

        [Column("effectiveTo")]
        public DateTime? effectiveToDate { get; set; }


        [Column("enterBy", TypeName = "int")]
        public Int32? enterBy { get; set; }

        [Column("enterDate", TypeName = "DateTime")]
        public DateTime? enterDate { get; set; }

        [ForeignKey("cityTypeID")]
        public virtual cityType parentCityType { get; set; }

        [ForeignKey("courseCatID")]
        public virtual CourseCategory parentCourseCat { get; set; }
    }
}
