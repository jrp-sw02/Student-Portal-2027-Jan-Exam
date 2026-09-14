using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class AccreditationDetailHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        
        
        [Column("InsttAccrDetailID", TypeName = "bigint")]
        public Int64 InsttAccrDetailID { get; set; }

        [Column("Institute_ID", TypeName = "bigint")]
        [Required(ErrorMessage = "Istitute ID is required")]
        [Display(Name = "Institute")]
        public Int64 InstituteID { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Category is required")]
        [Display(Name = "Course Category")]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public Int32 CourseID { get; set; }

        [Column("Accreditation_Status_ID", TypeName = "int")]
        [Required(ErrorMessage = "Accreditaion Status is required")]
        [Display(Name = "Accreditaion Status")]
        public Int32 AccreditationStatusID { get; set; }

        [Column("Accreditation_Number")]
        [Required(ErrorMessage = "Accreditation Number is required")]
        [MaxLength(25)]
        [Display(Name = "Accreditation Number")]
        public String AccreditationNumber { get; set; }

        [Column("Effective_From_Date")]
        [Display(Name = "Effective From Date")]
        public DateTime? EffectiveFromDate { get; set; }

        [Column("Effective_To_Date")]
        [Display(Name = "Effective To Date")]
        public DateTime? EffectiveToDate { get; set; }
        //Added 14 May 2019
        [Column("Withdrawl_Date")]
        [Display(Name = "Withdrawl_Date")]
        public DateTime? WithdrawlDate { get; set; }
        // 
        //Added 19 May 2020 for blocking institute
        [Column("Temp_Blocked")]
        [Display(Name = "Temp_Blocked")]
        public bool tempBlocked { get; set; }

        [Column("BlockedFromDate")]
        [Display(Name = "BlockedFromDate")]
        public DateTime? BlockedFromDate { get; set; }

        [Column("BlockedToDate")]
        [Display(Name = "BlockedToDate")]
        public DateTime? BlockedToDate { get; set; }

        [Column("updatedBy")]
        [Display(Name = "updatedBy")]
        public Int32  updatedBy { get; set; }
        // 

        [NotMapped]
        public enmAccreditationStatus enmAccreditationStatus
        {
            get
            {
                return (enmAccreditationStatus)this.AccreditationStatusID;
            }
            set
            {
                this.AccreditationStatusID = (int)value;
            }
        }

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [ForeignKey("AccreditationStatusID")]
        public virtual AccreditationStatus AccreditationStatus { get; set; }
    }
}
