using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
     public class OnlineExamCenter
     {
         [Key]
         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         [Column("ID", TypeName = "int")]
         public Int32 ID { get; set; }

         [Column("Course_Category_ID", TypeName = "int")]
         [Required(ErrorMessage = "Course Category is required")]
         public Int32 CourseCategoryID { get; set; }

         [Column("Course_ID", TypeName = "int")]
         public Int32? CourseID { get; set; }

         [Column("Online_Exam_Center_Type_ID", TypeName = "int")]
         [Required(ErrorMessage = "Exam Centre Type is required")]
         public Int32 ExamCentreTypeID { get; set; }

         [Column("Name")]
         [Required(ErrorMessage = "Exam Center Name is required")]
         [MaxLength(50)]
         [Display(Name = "Exam Center Name")]
         public String Name { get; set; }

         [Column("NAME_REGIONAL")]
         [MaxLength(100)]
         [Display(Name = "Exam Center Regional Name")]
         public String NameRegional { get; set; }

         [Column("Code")]
         [Required(ErrorMessage = "Exam Center Code is required")]
         [MaxLength(10)]
         [Display(Name = "Exam Center Code")]
         public String Code { get; set; }

         [Column("State_ID", TypeName = "bigint")]
         [Required(ErrorMessage = "State ID is required")]
         [Display(Name = "State ID")]
         public Int64 StateID { get; set; }

         [Column("District_ID", TypeName = "bigint")]
         [Display(Name = "District ID")]
         public Int64? DistrictID { get; set; }

         [Column("Is_Enabled", TypeName = "bit")]
         public Boolean IsEnabled { get; set; }

         [ForeignKey("StateID")]
         public virtual Location State { get; set; }

         [ForeignKey("DistrictID")]
         public virtual Location District { get; set; }

         [NotMapped]
         public enmExamCenterType enmExamCenterType
         {
             get
             {
                 return (enmExamCenterType)this.ExamCentreTypeID;
             }
             set
             {
                 this.ExamCentreTypeID = (int)value;
             }
         }

         [ForeignKey("CourseCategoryID")]
         public virtual CourseCategory CourseCategory { get; set; }

         [ForeignKey("CourseID")]
         public virtual Course Course { get; set; }

         public virtual ICollection<ExamVenue> ExamVenues { get; set; }
     }
}
