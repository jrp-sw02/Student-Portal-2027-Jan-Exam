using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class virtualAcademyCourseProspectus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("courseCategoryID")]
        [Required(ErrorMessage = "Course Category is required")]
        [Display(Name = "Course Category")]
        public Int64 courseCategoryID { get; set; }

        [Column("courseDurationID")]
        [Required(ErrorMessage = "Course Duration is required")]
        [Display(Name = "Course")]
        public Int64 courseDurationID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "File Name is required")]
        [MaxLength(50)]
        [Display(Name = "File Name")]
        public String Name { get; set; }

        [Column("fileName")]
        [MaxLength(100)]
        [Display(Name = "fileName")]
        public String fileName { get; set; }

        [Column("extension")]
        [MaxLength(5)]
        public String Extension { get; set; }

        //Add by vishal on 28.02.2022
        [Column("heading1")]
        [MaxLength(1000)]
        [Display(Name = "heading1")]
        public String Heading1 { get; set; }

        [Column("heading2")]
        [MaxLength(1000)]
        [Display(Name = "heading2")]
        public String Heading2 { get; set; }

        [Column("objective")]
        [MaxLength(2000)]
        [Display(Name = "objective")]
        public String Objective { get; set; }

        [Column("eligibility")]
        [MaxLength(2000)]
        [Display(Name = "eligibility")]
        public String Eligibility { get; set; }

        [Column("prerequisite")]
        [MaxLength(2000)]
        [Display(Name = "prerequisite")]
        public String Prerequisite { get; set; }

        [Column("courseFees")]
        [MaxLength(1000)]
        [Display(Name = "courseFees")]
        public String CourseFees { get; set; }

        [Column("methodology")]
        [MaxLength(2000)]
        [Display(Name = "methodology")]
        public String Methodology { get; set; }

        [Column("intendedUsers")]
        [MaxLength(2000)]
        [Display(Name = "intendedUsers")]
        public String IntendedUsers { get; set; }

        [Column("outcome")]
        [MaxLength(2000)]
        [Display(Name = "outcome")]
        public String Outcome { get; set; }

        [Column("courseContent")]
        [MaxLength(2000)]
        [Display(Name = "courseContent")]
        public String CourseContent { get; set; }
        
        [Column("faculty1Name")]
        [MaxLength(100)]
        [Display(Name = "faculty1Name")]
        public String Faculty1Name { get; set; }

        [Column("faculty1Qual")]
        [MaxLength(200)]
        [Display(Name = "faculty1Qual")]
        public String Faculty1Qual { get; set; }

        [Column("faculty1Mob")]
        [MaxLength(10)]
        [Display(Name = "faculty1Mob")]
        public String Faculty1Mob { get; set; }

        [Column("faculty1Email")]
        [MaxLength(100)]
        [Display(Name = "faculty1Email")]
        public String Faculty1Email { get; set; }

        [Column("faculty2Name")]
        [MaxLength(100)]
        [Display(Name = "faculty2Name")]
        public String Faculty2Name { get; set; }

        [Column("faculty2Qual")]
        [MaxLength(200)]
        [Display(Name = "faculty2Qual")]
        public String Faculty2Qual { get; set; }

        [Column("faculty2Mob")]
        [MaxLength(10)]
        [Display(Name = "faculty2Mob")]
        public String Faculty2Mob { get; set; }

        [Column("faculty2Email")]
        [MaxLength(100)]
        [Display(Name = "faculty2Email")]
        public String Faculty2Email { get; set; }

        [Column("faculty3Name")]
        [MaxLength(100)]
        [Display(Name = "faculty3Name")]
        public String Faculty3Name { get; set; }

        [Column("faculty3Qual")]
        [MaxLength(200)]
        [Display(Name = "faculty3Qual")]
        public String Faculty3Qual { get; set; }

        [Column("faculty3Mob")]
        [MaxLength(10)]
        [Display(Name = "faculty3Mob")]
        public String Faculty3Mob { get; set; }

        [Column("faculty3Email")]
        [MaxLength(100)]
        [Display(Name = "faculty3Email")]
        public String Faculty3Email { get; set; }
        //Add by vishal on 28.02.2022



        [Column("uploadedFile",  TypeName="varbinary(Max)")]
        public Byte[] BlobFile { get; set; }

        [Column("uploadedOn")]
        public DateTime UploadedOn { get; set; }

        [Column("uploadedBy")]
        public Int64 UploadedBy { get; set; }

    }
}
