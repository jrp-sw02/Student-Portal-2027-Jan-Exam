using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CourseWiseOccupationMapping
    {
        [Column("Course_Category_ID", TypeName = "int")]
        public Int32 CourseCategoryID { get; set; }

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }

        [Column("Course_ID", TypeName = "int")]
        public Int32 CourseID { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [Column("Occupation_ID", TypeName = "int")]
        public Int32 OccupationID { get; set; }

        [ForeignKey("OccupationID")]
        public virtual Occupation Occupation { get; set; }
    }
}
