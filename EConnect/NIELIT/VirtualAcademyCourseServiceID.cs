using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class VirtualAcademyCourseServiceID
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Course Name is required")]
        [MaxLength(150)]
        [Display(Name = "Course Name")]
        public String Name { get; set; }             

        [Column("Code")]      
        [MaxLength(20)]       
        public String Code { get; set; }               

        [Column("Course_Category_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Category is required")]
        [Display(Name = "Course Category")]
        public Int32 CourseCategoryIID { get; set; }

        [Column("Course_Type_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Type is required")]
        [Display(Name = "Course Type")]
        public Int32 CourseTypeIID { get; set; }        

        [Column("Registration_ServiceID")]
        [MaxLength(20)]
        public String RegistrationServiceID { get; set; }

        [Column("Examination_ServiceID")]
        [MaxLength(20)]
        public String ExaminationServiceID { get; set; }

        [Column("Certificate_ServiceID")]
        [MaxLength(20)]
        public String CertificateServiceID { get; set; }       

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [Column("enterBy", TypeName = "int")]
        [Display(Name = "User Name")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }
    }
}
