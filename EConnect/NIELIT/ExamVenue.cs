using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class ExamVenue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Exam Venue Name is required")]
        [MaxLength(250)]
        public String Name { get; set; }

        [Column("Exam_Centre_ID", TypeName = "int")]
        [Required(ErrorMessage = "Exam Centre is required")]
        public Int32 ExamCentreID { get; set; }

        [ForeignKey("ExamCentreID")]
        public virtual ExamCenter ExamCenter { get; set; }

        [Column("Address1")]
        [MaxLength(200)]
        [Required]
        public String AddressLine1 { get; set; }

        [Column("Address2")]
        [MaxLength(200)]
        public String AddressLine2 { get; set; }

        [Column("City")]
        [Required]
        [MaxLength(50)]
        public String City { get; set; }

        [Column("Pin_Code", TypeName = "int")]
        public Int32? PinCode { get; set; }

        [Column("State_ID", TypeName = "bigint")]
        [Required(ErrorMessage = "State ID is required")]
        public Int64 StateID { get; set; }

        [ForeignKey("StateID")]
        public virtual Location State { get; set; }

        [Column("Std_Code", TypeName = "int")]
        public Int32? StdCode { get; set; }

        [Column("Phone_Nunber", TypeName = "int")]
        public Int32? PhoneNumber { get; set; }

        [Column("Fax_Number", TypeName = "int")]
        public Int32? FaxNumber { get; set; }

        [Column("Mobile", TypeName = "bigint")]
        [Display(Name = "Mobile Number")]
        public Int64? MobileNumber { get; set; }

        [Column("Email")]
        [MaxLength(100)]
        public String EmailAddress { get; set; }

        [Column("Is_Active")]
        public Boolean IsActive { get; set; }

        [Column("Created_By", TypeName = "int")]
        public Int32 CreatedByID { get; set; }

        [ForeignKey("CreatedByID")]
        public virtual URM.User CreatedByUser { get; set; }

        [Column("Created_On")]
        public DateTime CreatedOn { get; set; }

        [Column("Course_ID", TypeName = "int")]
        public Int32 CourseID { get; set; }

        [Column("Code")]
        [Required]
        [MaxLength(50)]
        public String Code { get; set; }
    }
}
