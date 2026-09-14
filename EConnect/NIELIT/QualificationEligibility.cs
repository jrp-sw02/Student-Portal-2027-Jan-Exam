using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class QualificationEligibility
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

        [Column("Applicant_Type_ID", TypeName = "int")]
        [Required]
        public Int32 ApplicantTypeID{ get; set; }

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

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [ForeignKey("ApplicantTypeID")]
        public virtual ApplicantType ApplicantType { get; set; }

        [ForeignKey("QualificationLevelID")]
        public virtual QualificationLevel QualificationLevel { get; set; }

    }
}
