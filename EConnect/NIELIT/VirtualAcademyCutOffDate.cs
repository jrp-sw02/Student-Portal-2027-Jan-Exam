using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class VirtualAcademyCutOffDate
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Category is required")]
        public Int32 CourseCategoryID { get; set; }
        

        [Column("CourseDurationID", TypeName = "int")]
        [Required(ErrorMessage = "Course is required")]
        public Int32 CourseDurationID { get; set; }

        [Column("batchID", TypeName = "bigint")]
        [Required(ErrorMessage = "Batch is required")]
        public Int64 batchID { get; set; }

        [Column("Activity_ID", TypeName = "int")]
        [Required(ErrorMessage = "Activity Type is required")]
        public Int32 ActivityID { get; set; }

        [Column("Applicant_Type_ID", TypeName = "int")]
        [Required(ErrorMessage = "Applicant Type is required")]
        public Int32 ApplicantTypeID { get; set; }

        [Column("regnStartDate")]
        public DateTime regnStartDate { get; set; }

        [Column("Efferctive_Date")]
        public DateTime Efferctive_Date { get; set; }

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }
               

        [ForeignKey("ActivityID")]
        public virtual Activity Activity { get; set; }
       

        [Column("enterBy", TypeName = "bigint")]
        [Required(ErrorMessage = "Enter By is required")]
        public Int64 enterByID { get; set; }


        [Column("enterDate", TypeName = "DateTime")]
        public DateTime enterDate { get; set; }      
    }
}
