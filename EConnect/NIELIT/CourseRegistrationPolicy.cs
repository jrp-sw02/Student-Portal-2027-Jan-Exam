using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
   public class CourseRegistrationPolicy
    {
       [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       [Column("ID", TypeName = "int")]
       public Int32 ID { get; set; }

       [Column("Course_Category_ID", TypeName = "int")]
       public Int32 CourseCategoryID { get; set; }

       [Column("Course_ID", TypeName = "int")]
       public Int32 CourseID { get; set; }

       [Column("Reg_Validity", TypeName = "int")]
       public Int32 RegistrationValidity { get; set; }

       [Column("Re_Reg_Chance", TypeName = "bit")]
       public Boolean ReRegistrationChance { get; set; }

       [Column("Re_Reg_Validity", TypeName = "int")]
       public Int32? ReRegistrationValidity { get; set; }

       [Column("Re_Reg_Gape", TypeName = "int")]
       public Int32? ReRegistrationGapInMonths { get; set; }

       [Column("Allowed_Language", TypeName = "int")]
       public Int32? AllowedLanguage { get; set; }

       [Column("Effective_From_Date")]
       public DateTime EffectiveFromDate { get; set; }

       [ForeignKey("CourseCategoryID")]
       public virtual CourseCategory CourseCategory { get; set; }

       [ForeignKey("CourseID")]
       public virtual Course Course { get; set; }

    }
}
