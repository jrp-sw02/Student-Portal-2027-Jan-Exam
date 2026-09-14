using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

		[Column("NielitSectorID", TypeName = "int")]
        public Int32 NielitSectorID { get; set; }

        [Column("NIELITrgSplnID", TypeName = "int")]
        public Int32 NIELITrgSplnID { get; set; }
        [Column("Name")]
        [Required(ErrorMessage = "Course Name is required")]
        [MaxLength(150)]
        [Display(Name = "Course Name")]
        public String Name { get; set; }

        [Column("NAME_REGIONAL")]
        [MaxLength(150)]
        [Display(Name = "Course Regional Name")]
        public String NameRegional { get; set; }

        [Column("Code")]
        [Required(ErrorMessage = "Course Code is required")]
        [MaxLength(20)]
        [Display(Name = "Course Code")]
        public String Code { get; set; }

        [Column("Display_Order", TypeName = "int")]
        [Required(ErrorMessage = "Display Order is required")]
        [Display(Name = "Display Order")]
        public Int32 DisplayOrder { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Category is required")]
        [Display(Name = "Course Category")]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_Type_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Type is required")]
        [Display(Name = "Course Type")]
        public Int32 CourseTypeID { get; set; }

        [Column("Show_On_Web", TypeName = "bit")]
        public Boolean ShowOnWeb { get; set; }

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }

        [ForeignKey("CourseTypeID")]
        public virtual CourseType CourseType { get; set; }

        [Column("Previous_Course_ID", TypeName = "int")]
        public Int32? LowerCourseID { get; set; }

        [ForeignKey("LowerCourseID")]
        public virtual Course LowerCourse { get; set; }

        [Column("Applicant_Type_ID", TypeName = "int")]
        public Int32? ApplicantTypeID { get; set; }

        [Column("Registration_ServiceID")]
        [MaxLength(20)]
        public String RegistrationServiceID { get; set; }

        [Column("Examination_ServiceID")]
        [MaxLength(20)]
        public String ExaminationServiceID { get; set; }

        [Column("Certificate_ServiceID")]
        [MaxLength(20)]
        public String CertificateServiceID { get; set; }
        //ProjectServiceID

        [Column("Project_ServiceID")]
        [MaxLength(20)]
        public String ProjectServiceID { get; set; }

        [Column("VAFACF_ServiceID")]
        [MaxLength(20)]
        public String VAFACFServiceID { get; set; }

        [Column("Roll_Number_Code")]
        [MaxLength(1)]
        [Display(Name = "Roll Number Code")]
        public String RollNumberCode { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
//Added for future skill
        [Column("whetherFutureSkill")]
        [DefaultValue(false)]
        public bool IsFuture { get; set; }

        [ForeignKey("ApplicantTypeID")]
        public virtual ApplicantType ApplicantType { get; set; }

        public virtual ICollection<CourseRevision> CourseRevisions { get; set; }

        public virtual ICollection<CourseRegistrationPolicy> CourseRegistrationPolicies { get; set; }

        public virtual ICollection<FeeDetail> FeeDetails { get; set; }

        [NotMapped]
        public enmCourseType enmCourseType
        {
            get
            {
                return (enmCourseType)this.CourseTypeID;
            }
            set
            {
                this.CourseTypeID = (int)value;
            }
        }

        public virtual ICollection<RegistrationDetail> RegistrationDetails { get; set; }
    }
}
