using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EConnect.URM;
namespace EConnect.NIELIT
{
    public class ExemptionRule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Category is required")]
        public Int32 CourseCategoryID { get; set; }

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }

        [Column("Current_Course_ID", TypeName = "int")]
        [Required(ErrorMessage = "Current Course is required")]
        public Int32 CurrentCourseID { get; set; }

        [ForeignKey("CurrentCourseID")]
        public virtual Course CurrentCourse { get; set; }

        [Column("Current_Course_Revision", TypeName = "int")]
        [Required]
        public Int32 CurrentCourseRevisionNumber { get; set; }

        [Column("Current_Module_ID", TypeName = "int")]
        public Int32? CurrentModuleID { get; set; }

        [ForeignKey("CurrentModuleID")]
        public virtual Module CurrentModule { get; set; }

        [Column("Lower_Course_ID", TypeName = "int")]
        [Required(ErrorMessage = "Lower Course is required")]
        public Int32 LowerCourseID { get; set; }

        [ForeignKey("LowerCourseID")]
        public virtual Course LowerCourse { get; set; }

        [Column("Lower_Course_Revision", TypeName = "int")]
        [Required]
        public Int32 LowerCourseRevisionNumber { get; set; }

        [Column("Lower_Module_ID", TypeName = "int")]
        [Required]
        public Int32 LowerModuleID { get; set; }

        [ForeignKey("LowerModuleID")]
        public virtual Module LowerModule { get; set; }

        [Column("Allow_Parity", TypeName = "bit")]
        public Boolean AllowParity { get; set; }

        [Column("Created_By", TypeName = "int")]
        [Required]
        public Int32 CreatedByID { get; set; }

        [ForeignKey("CreatedByID")]
        public virtual User user { get; set; }

        [Column("Created_On")]
        [Required]
        public DateTime CreatedOn { get; set; }
    }
}
