using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class BatchItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Batch_ID",TypeName = "int")]
        [Required(ErrorMessage = "Batch Name is required")]
        public Int32 BatchID { get; set; }

        [Column("Courser_Reg_Appl_ID", TypeName = "bigint")]
        public Int64? CourseRegistrationApplicationID { get; set; }

        [Column("Cert_Exam_Appl_ID", TypeName = "bigint")]
        public Int64? CertificateExamApplicationID { get; set; }

        [Column("Course_Exam_Appl_ID", TypeName = "bigint")]
        public Int64? CourseExamApplicationID { get; set; }

        [Column("Status_ID", TypeName = "int")]
        [Required(ErrorMessage = "Status is required")]
        public Int32 StatusID { get; set; }

        [Column("Created_By", TypeName = "int")]
        [Required(ErrorMessage = "Created By is required")]
        public Int32 CreatedByID { get; set; }

        [Column("Created_On")]
        public DateTime CreatedOn { get; set; }

        [Column("Updated_By", TypeName = "int")]
        public Int32? UpdatedByID { get; set; }

        [Column("Updated_On")]
        public DateTime? UpdatedOn { get; set; }

        [Column("Rejected_By", TypeName = "int")]
        public Int32? RejectedByID { get; set; }

        [Column("Rejected_On")]
        public DateTime? RejectedOn { get; set; }

        [Column("Is_Rejected", TypeName = "bit")]
        [Display(Name = "Is Rejected")]
        public Boolean IsRejected { get; set; }

        [Column("Remarks")]
        public String Remarks { get; set; }

        [Column("Rejected_Reason")]
        [MaxLength(500)]
        [Display(Name = "Rejected Reason")]
        public String RejectedReason { get; set; }

        [Column("Kept_In_Abeyance_On")]
        public DateTime? KeptInAbeyanceOn{ get; set; }

        [Column("Is_Kept_In_Abeyance", TypeName = "bit")]
        [Display(Name = "Is Kept In Abeyance")]
        public Boolean IsKeptInAbeyance { get; set; }

        [Column("Kept_In_Abeyance_Reason")]
        [MaxLength(500)]
        [Display(Name = "Kept In Abeyance Reason")]
        public String KeptInAbeyanceReason { get; set; }

        [ForeignKey("BatchID")]
        public virtual Batch Batch { get; set; }

        [NotMapped]
        public virtual enmCourseApplicationStatus Status
        {
            get
            {
                return (enmCourseApplicationStatus)this.StatusID;
            }
            set
            {
                this.StatusID = Convert.ToInt32(value);
            }
        }

        [ForeignKey("CourseRegistrationApplicationID")]
        public virtual CourseRegistrationApplication CourseRegistrationApplication { get; set; }

        [ForeignKey("CertificateExamApplicationID")]
        public virtual CertificateExamApplication CertificateExamApplication { get; set; }

        [ForeignKey("CourseExamApplicationID")]
        public virtual CourseExamApplication CourseExamApplication { get; set; }
    }
}
