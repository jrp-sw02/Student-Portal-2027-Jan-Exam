using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class NielitCentreBatch
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

        [Column("Name")]
        [Required(ErrorMessage = "Name is Required")]
        [MaxLength(150)]
        [Display(Name = "Name")]
        public String Name { get; set; }

        [Column("BatchCode")]
        [Required(ErrorMessage = "Batch Code is Required")]
        [MaxLength(50)]
        [Display(Name = "BatchCode")]
        public String BatchCode { get; set; }

        [Column("startDate")]
        public DateTime startDate { get; set; }

        [Column("endDate")]
        public DateTime endDate { get; set; }

        //[Column("batchSession")]
        //[Required(ErrorMessage = "Batch Session Code is Required")]
        //[MaxLength(1)]
        //[Display(Name = "Batch Session")]
        //public Int32 batchSession { get; set; }

        [Column("whetherAffiliated")]
        [MaxLength(1)]
        [Display(Name = "whetherAffiliated")]
        public String whetherAffiliated { get; set; }

        [Column("batchSession", TypeName = "int")]
        public Int32 batchSession { get; set; }



        [Column("batchDurationTheoryHours", TypeName = "numeric")]
        public Decimal batchDurationTheoryHours { get; set; }

        [Column("batchDurationPracticalHours", TypeName = "numeric")]
        public Decimal batchDurationPracticalHours { get; set; }

        [Column("whetherCorporate", TypeName = "bit")]
        public Boolean whetherCorporate { get; set; }

        [Column("OrgTrained")]
        // [Required(ErrorMessage = "Org Trained is Required")]
        [MaxLength(100)]
        [Display(Name = "Org Trained")]
        public String OrgTrained { get; set; }

        [Column("facultyName")]
        [Required(ErrorMessage = "Faculty Name is Required")]
        [MaxLength(50)]
        [Display(Name = "Faculty Name")]
        public String facultyName { get; set; }

        [Column("facultyEmail")]
        [Required(ErrorMessage = "Faculty Email is Required")]
        [MaxLength(50)]
        [Display(Name = "Faculty Email")]
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

	   //Add for formal course

        [Column("IsSemBased", TypeName = "bit")]
        [Display(Name = "Is Sem Based")]
        public Boolean IsSemBased { get; set; }

        [Column("SemYearBased", TypeName = "int")]
        [Display(Name = "SemYearBased")]
        public Int32? SemYearBased { get; set; }

        [Column("AffUniversity")]
        [MaxLength(100)]
        [Display(Name = "AffUniversity")]
        public String AffUniversity { get; set; }

        [Column("RegistrationDurationYears", TypeName = "int")]
        [Display(Name = "RegistrationDurationYears")]
        public Int32? RegistrationDurationYears { get; set; }
    }
}
