using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class NonAffiliatedInstituteCoursesDetailHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("instituteID", TypeName = "bigint")]
        [Required(ErrorMessage = "Istitute ID is required")]
        [Display(Name = "Institute")]
        public Int64 InstituteID { get; set; }

        [Column("courseCategoryID", TypeName = "int")]
        [Required(ErrorMessage = "Course Category is required")]
        [Display(Name = "Course Category")]
        public Int32 CourseCategoryID { get; set; }

        [Column("courseID", TypeName = "int")]
        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public Int32 CourseID { get; set; }

        [Column("linkedStatusID", TypeName = "int")]
        [Required(ErrorMessage = "linked Status ID is required")]
        [Display(Name = "linkedStatusID")]
        public Int32 linkedStatusID { get; set; }

        [Column("courseLinkedNumber")]
        [Required(ErrorMessage = "course Linked Number is required")]
        [MaxLength(25)]
        [Display(Name = "courseLinkedNumber")]
        public String courseLinkedNumber { get; set; }

        [Column("effectiveFromDate")]
        [Display(Name = "Effective From Date")]
        public DateTime? effectiveFromDate { get; set; }

        [Column("effectiveToDate")]
        [Display(Name = "Effective To Date")]
        public DateTime? effectiveToDate { get; set; }
	
        [Column("withdrawlDate")]
        [Display(Name = "Withdrawl_Date")]
        public DateTime? withdrawlDate { get; set; }

        [Column("tempBlocked", TypeName = "bit")]
        [Display(Name = "Temp_Blocked")]
        public bool tempBlocked { get; set; }

        [Column("BlockedFromDate")]
        [Display(Name = "BlockedFromDate")]
        public DateTime? BlockedFromDate { get; set; }

        [Column("BlockedToDate")]
        [Display(Name = "BlockedFromDate")]
        public DateTime? BlockedToDate { get; set; }

        [Column("projectId", TypeName = "bigint")]       
        [Display(Name = "Project")]
        public Int64 projectId { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        [Display(Name = "User Name")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }
       
       
    }
}
