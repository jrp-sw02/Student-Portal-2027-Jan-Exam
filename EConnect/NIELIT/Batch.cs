using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class Batch
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Batch_No")]
        [Required(ErrorMessage = "Batch Name is required")]
        [MaxLength(50)]
        public String Number { get; set; }

        [Column("Application_Type_ID", TypeName = "int")]
        [Required(ErrorMessage = "Application Type is required")]
        public Int32 ApplicationTypeID { get; set; }

        [Column("Applicant_Type_ID", TypeName = "int")]
        [Required(ErrorMessage = "Applicant Type is required")]
        public Int32 ApplicantTypeID { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Category is required")]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course is required")]
        public Int32 CourseID { get; set; }

        [Column("Status_ID", TypeName = "int")]
        [Required(ErrorMessage = "Status is required")]
        public Int32 StatusID { get; set; }

        [Column("Exam_ID", TypeName = "int")]
        public Int32? ExamID { get; set; }

        [ForeignKey("ExamID")]
        public virtual Exam Exam { get; set; }

        [Column("Created_By", TypeName = "int")]
        [Required(ErrorMessage = "Created By is required")]
        public Int32 CreatedByID { get; set; }

        [Column("Regional_Center_ID", TypeName = "int")]
        public Int32? RegionalCenterID { get; set; }

        [Column("Created_On")]
        public DateTime CreatedOn { get; set; }

        [NotMapped]
        public virtual enmBatchStatus Status
        {
            get
            {
                return (enmBatchStatus)this.StatusID;
            }
            set
            {
                this.StatusID = Convert.ToInt32(value);
            }
        }
        [NotMapped]
        public virtual enmApplicationType enmApplicationType
        {
            get
            {
                return (enmApplicationType)this.ApplicationTypeID;
            }
            set
            {
                this.ApplicationTypeID = Convert.ToInt32(value);
            }
        }
        [NotMapped]
        public virtual enmApplicantType enmApplicantType
        {
            get
            {
                return (enmApplicantType)this.ApplicantTypeID;
            }
            set
            {
                this.ApplicantTypeID = Convert.ToInt32(value);
            }
        }

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }

        [ForeignKey("ApplicationTypeID")]
        public virtual ApplicationType ApplicationType { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        public virtual ICollection<BatchItem> BatchItems { get; set; }

        [ForeignKey("RegionalCenterID")]
        public virtual RegionalCenter RegionalCenter { get; set; }

        [ForeignKey("ApplicantTypeID")]
        public virtual ApplicantType ApplicantType { get; set; }
    }
}
