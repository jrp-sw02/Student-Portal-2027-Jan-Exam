using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EConnect.NIELIT
{
    [Serializable]
    public class NielitCourseDuration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int32 ID { get; set; }

        [Column("courseID", TypeName = "int")]
        [Required(ErrorMessage = "courseID is required")]
        [Display(Name = "course_ID")]
        public Int32 courseID { get; set; }

        [Column("courseDurationHrs", TypeName = "int")]
        public Int32 courseDurationHrs { get; set; }

        [Column("courseDurationDays", TypeName = "int")]
        public Int32 courseDurationDays { get; set; }

        //[Column("courseCompletionDurationHrs", TypeName = "int")]
        //public Int32 courseCompletionDurationHrs { get; set; }

        //[Column("courseRunningMonths", TypeName = "int")]
        //public Int32 courseRunningMonths { get; set; }

        [Column("isVerified", TypeName = "bit")]
        public bool? isVerified { get; set; }

        [Column("verifiedStatus", TypeName = "int")]
        public Int32? verifiedStatus { get; set; }

        [Column("verificationMessage")]
        [MaxLength(500)]
        public String verificationMessage { get; set; }

        [Column("verificationStatusDate")]
        public DateTime? verificationStatusDate { get; set; }

        [Column("verifiedOn")]
        public DateTime? verifiedOn { get; set; }

        [Column("verifiedBy", TypeName = "bigint")]
        public Int32? verifiedBy { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        public int enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }
 		
 		//Add for Year for formal course

        [Column("courseDurationYears", TypeName = "int")]
        public Int32? courseDurationYears { get; set; }

    }
}
