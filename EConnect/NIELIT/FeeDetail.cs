using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public  class FeeDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        public Int32 CourseID { get; set; }

        [Column("Fee_Type_ID", TypeName = "int")]
        public Int32 FeeTypeID { get; set; }

        [Column("Fee_Amount", TypeName = "int")]
        public Int32 FeeAmount { get; set; }

        [Column("Effective_From_Date")]
        public DateTime EffectiveFromDate { get; set; }

        [Column("Effective_To_Date")]
        public DateTime? EffectiveToDate { get; set; }

        public virtual CourseCategory CourseCategory { get; set; }
        public virtual Course Course { get; set; }

        [ForeignKey("FeeTypeID")]
        public virtual FeeType FeeType { get; set; }

        // Added for UP Project start 
        //Added_17_12_2024
        [Column("Project_ID", TypeName = "bigint")]
        public Int64? projectid { get; set; }

        // Added for UP Project end 

    }
}
