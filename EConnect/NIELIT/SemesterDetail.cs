using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class SemesterDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("centreID", TypeName = "bigint")]
        public Int64 centreID { get; set; }

        //[Column("CourseID", TypeName = "bigint")]
        //public Int64 CourseID { get; set; }

        [Column("CourseDurationID", TypeName = "bigint")]
        public Int64 CourseDurationID { get; set; }

        [Column("subCentreID", TypeName = "bigint")]
        public Int64 subCentreID { get; set; }

        [Column("batchID", TypeName = "bigint")]
        public Int64 batchID { get; set; }

        [Column("semesterNo", TypeName = "int")]
        public int semesterNo { get; set; }

        [Column("semStartDate")]
        public DateTime semStartDate { get; set; }

        [Column("semEndDate")]
        public DateTime semEndDate { get; set; }
        
        [Column("semSession", TypeName = "int")]
        public Int32 semSession { get; set; }
        
        [Column("semDurationTheoryHours", TypeName = "numeric")]
        public Decimal batchDurationTheoryHours { get; set; }

        [Column("semDurationPracticalHours", TypeName = "numeric")]
        public Decimal batchDurationPracticalHours { get; set; }

        [Column("facultyName")]
        [Required(ErrorMessage = "Coordinator Name is Required")]
        [MaxLength(50)]
        [Display(Name = "Coordinator Name")]
        public String facultyName { get; set; }

        [Column("facultyEmail")]
        [Required(ErrorMessage = "Coordinator Email is Required")]
        [MaxLength(50)]
        [Display(Name = "Coordinator Email")]
        public String facultyEmail { get; set; }

        [Column("learningModeID", TypeName = "bigint")]
        public Int64? learningModeID { get; set; }

        [Column("Remarks")]
        [MaxLength(1000)]
        [Display(Name = "Remarks")]
        public String Remarks { get; set; }

        [Column("Show_On_Web", TypeName = "bit")]
        [Display(Name = "Show On Web")]
        public Boolean Show_On_Web { get; set; }

        [Column("IsActive", TypeName = "bit")]
        [Display(Name = "Is Active")]
        public Boolean IsActive { get; set; }

        [Column("IsVerified", TypeName = "bit")]
        [Display(Name = "Is Verified")]
        public Boolean IsVerified { get; set; }

        [Column("enterBy", TypeName = "int")]
        [Display(Name = "User Name")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }

      }
}
