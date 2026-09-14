using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class ResultGrade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Code")]
        [MaxLength(10)]
        [Required]
        public String Code { get; set; }

        [Column("Description")]
        [MaxLength(500)]
        [Required]
        public String Description { get; set; }

        [Column("Percentage_From",  TypeName="int")]
        [Required]
        public Int32 PercentageFrom { get; set; }

        [Column("Percentage_To", TypeName = "int")]
        [Required]
        public Int32 PercentageTo { get; set; }

        [Column("Is_Passed", TypeName = "bit")]
        [Required]
        public Boolean  IsPassed { get; set; }

        [Column("Effective_From_Date")]
        [Required]
        public DateTime EffectiveFromDate { get; set; }

        [Column("Course_Category_ID", TypeName="int")]
        [Required]
        public Int32 CourseCategoryID { get; set; }

        [Column("Version_ID", TypeName = "int")]
        public Int32? VersionID { get; set; }

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }
    }
}
