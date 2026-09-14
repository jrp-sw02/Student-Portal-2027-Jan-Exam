using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class MISQualification_Eligibility
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        [Required]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        [Required]
        public Int32 CourseID { get; set; }

        [Column("Qualification_Level_ID", TypeName = "int")]
        [Required]
        public Int32 QualificationLevelID { get; set; }

        [Column("Experience", TypeName = "numeric")]
        [Required]
        public Decimal Experience { get; set; }

        [Column("Effective_From_Date")]
        public DateTime EffectiveDateFrom { get; set; }

        [Column("Effective_To_Date")]
        public DateTime? EffectiveDateTo { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        [Required(ErrorMessage = "Enter By is required")]
        public Int64 enterByID { get; set; }

        [Column("enterDate", TypeName = "DateTime")]
        public DateTime enterDate { get; set; }
       
    }
}
