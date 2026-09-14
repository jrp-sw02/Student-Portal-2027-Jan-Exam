using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CourseLevelDurations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Category is required")]
        [Display(Name = "Course Category")]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course ID is required")]
        [Display(Name = "Course ID")]
        public Int32 CourseID { get; set; }

        [Column("Course_Level_Revision_no", TypeName = "int")]
        [Required(ErrorMessage = "Course Level Revision no is required")]
        [Display(Name = "Course Level Revision no")]
        public Int32 Course_Level_Revision_no { get; set; }

        //Commented due to decimals [Column("Course_Level_no", TypeName = "int")]       
        [Column("Course_Level_no", TypeName = "decimal")]       
        [Display(Name = "Course Level no")]
        public decimal? Course_Level_no { get; set; }

        //Changed to decimal on 1 Apr 2025  [Column("Course_Level_DurationHrs", TypeName = "int")]
        [Column("Course_Level_DurationHrs", TypeName = "decimal")]
        [Required(ErrorMessage = "Course Level DurationHrs is required")]
        [Display(Name = "Course Level DurationHrs")]
        public decimal? Course_Level_DurationHrs { get; set; }

        [Column("theoryHrs", TypeName = "numeric")]
        public decimal? theoryHrs { get; set; }

        [Column("practicalHrs", TypeName = "numeric")]
        public decimal? practicalHrs { get; set; }

        [Column("esHrs", TypeName = "numeric")]
        public decimal? esHrs { get; set; }

        [Column("OJT_ProjectHrs", TypeName = "numeric")]
        public decimal? OJT_ProjectHrs { get; set; }

        [Column("domainSkillsHrs", TypeName = "numeric")]
        public decimal? domainSkillsHrs { get; set; }

        //Added 15 Jul 2024 for credits
        [Column("Credits", TypeName = "decimal")]
        [Display(Name = "Credits")]
        public Decimal? Credits { get; set; }

        //Added 18 Oct 2024
        [Column("certCourseType")]
        [Display(Name = "Course Type for Certificate")]
        public string certCourseType { get; set; }

        //Added 28-07-2023
        [Column("QualFileCourseName")]
        [Display(Name = "Qualification File Course Name")]
        public string QualFileCourseName { get; set; }

        [DefaultValue(false)]
        public bool isGOINascomm { get; set; }


        [Column("NCVETQualCode")]
        [Display(Name = "NCVET Qualification Code")]
        public string NCVETQualCode { get; set; }

        [Column("NIELITQualCode")]
        [Display(Name = "NIELIT Qualification Code")]
        public string NIELITQualCode { get; set; }

        //Added_For_MinAge_25_10_2024
        [Column("minAge", TypeName = "int")]
        public Int32? minAge { get; set; }

        //Added_For_MaxAge_25_10_2024
        [Column("maxAge", TypeName = "int")]
        public Int32? maxAge { get; set; }
        
        [Column("awardingBodyID", TypeName = "varchar")]
        public string awardingBodyID { get; set; }

        [Column("awardingBodyName", TypeName = "varchar")]
        [MaxLength(100)]
        public string awardingBodyName { get; set; }

        [Column("claasroomCount", TypeName = "int")]
        public Int32? claasroomCount { get; set; }

        [Column("labCount", TypeName = "int")]
        public Int32? labCount { get; set; }


        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Column("Effective_From_Date")]
        public DateTime? Effective_From_Date { get; set; }

        [Column("Effective_To_Date")]
        public DateTime? Effective_To_Date { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }

        [Column("enterBy", TypeName = "int")]
        public Int32 enterBy { get; set; }
       
    }
}
