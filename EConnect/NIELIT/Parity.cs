using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class Parity
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

        [Column("Course_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course is required")]
        public Int32 CourseID { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [Column("Old_Revision_Number", TypeName = "int")]
        [Required]
        public Int32 OldRevisionNumber { get; set; }

        [Column("Old_Module_ID", TypeName = "int")]
        [Required]
        public Int32 OldModuleID { get; set; }

        [ForeignKey("OldModuleID")]
        public virtual Module OldModule { get; set; }

        [Column("New_Revision_Number", TypeName = "int")]
        [Required]
        public Int32 NewRevisionNumber { get; set; }

        [Column("New_Module_ID", TypeName = "int")]
        [Required]
        public Int32 NewModuleID { get; set; }

        [ForeignKey("NewModuleID")]
        public virtual Module NewModule { get; set; }
    }
}
