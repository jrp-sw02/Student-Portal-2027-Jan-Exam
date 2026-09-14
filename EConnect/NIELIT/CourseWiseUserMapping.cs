using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CourseWiseUserMapping
    {
        [Column("Course_Category_ID", TypeName = "int")]
        public Int32 CourseCategoryID { get; set; }

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }

        [Column("Course_ID", TypeName = "int")]
        public Int32 CourseID { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }


        [Column("User_No", TypeName = "int")]
        public Int32 UserNo { get; set; }

        [ForeignKey("UserNo")]
        public virtual URM.User User { get; set; }
    }
}
