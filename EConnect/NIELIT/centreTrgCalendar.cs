using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class centreTrgCalendar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("CentreId", TypeName = "bigint")]
        public Int64 CentreId { get; set; }

        [Column("courseCategoryID", TypeName = "bigint")]
        public Int64 courseCategoryID { get; set; }

        [Column("courseId", TypeName = "bigint")]
        public Int64 courseId { get; set; }

        [Column("whetherNSQFAligned", TypeName = "bit")]
        public bool whetherNSQFAligned { get; set; }

        [Column("NSQFLevel", TypeName = "int")]
        [Required(ErrorMessage = "NSQF Level is required")]
        [Display(Name = "NSQF Level")]
        public int NSQFLevel { get; set; }
        
        [Column("participantEligibility")]
        [Required(ErrorMessage = "Participant Eligibility is required")]
        [MaxLength(250)]
        [Display(Name = "Participant Eligibility")]
        public String participantEligibility { get; set; }

        [Column("durationHrs", TypeName = "int")]
        [Display(Name = "durationHrs")]
        public int durationHrs { get; set; }

        [Column("durationMths", TypeName = "int")]
        [Display(Name = "durationMths")]
        public int durationMths { get; set; }

        [Column("startDate", TypeName = "date")]
        [Display(Name = "Start Date")]
        public DateTime  startDate { get; set; }

        [Column("endDate", TypeName = "date")]
        [Display(Name = "End Date")]
        public DateTime endDate { get; set; }

        [Column("admissionStatus")]
        [Required(ErrorMessage = "Admission Status is required")]
        [MaxLength(1)]
        [Display(Name = "Admission Status")]
        public String admissionStatus { get; set; }
        
        [Column("courseInchargeName")]
        [MaxLength(50)]
        public String courseInchargeName { get; set; }

        [Column("courseInchargeDesig")]
        [MaxLength(50)]
        public String courseInchargeDesig { get; set; }

        [Column("courseInchargeSTDCode")]
        [MaxLength(6)]
        public String courseInchargeSTDCode { get; set; }

        [Column("courseInchargePhone1")]
        [MaxLength(10)]
        public String courseInchargePhone1 { get; set; }

        [Column("courseInchargePhone2")]
        [MaxLength(10)]
        public String courseInchargePhone2 { get; set; }

        [Column("courseInchargeMobile")]
        [MaxLength(12)]
        public String courseInchargeMobile { get; set; }

        [Column("courseInchargeEmail")]
        [MaxLength(100)]
        public String courseInchargeEmail { get; set; }

        [Column("enterBy", TypeName = "int")]
        [Required(ErrorMessage = "Enter By is required")]
        public Int32 enterByID { get; set; }


        [Column("enterDate",TypeName ="DateTime")]
        public DateTime enterDate { get; set; }
    }
}
