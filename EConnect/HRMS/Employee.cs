using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.HRMS
{
    public class Employee  
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName ="int")]
        public int ID { get; set; }

        [Column("ORG_ID", TypeName = "int")]
        [Required(ErrorMessage = "Organization ID is required")]
        public int OrganizationID { get; set; }

        [Column("INTERNAL_TOKEN_NUMBER")]
        [MaxLength(15)]
        [Display(Name = "Employee NUmber")]
        public String EmoloyeeNumber { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [Column("TITLE")]
        [MaxLength(10)]
        [Display(Name = "Title")]
        public String Title { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Column("FIRST_NAME")]
        [MaxLength(20)]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }

        [Column("MIDDLE_NAME")]
        [MaxLength(20)]
        [Display(Name = "Middle Name")]
        public String MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Column("LAST_NAME")]
        [MaxLength(20)]
        [Display(Name = "Last Name")]
        public String LastName { get; set; }

        [Column("GENDER")]
        [MaxLength(10)]
        [Display(Name = "Gender")]
        public String Gender { get; set; }

        public string FullName
        {
            get { return (Title + " " + FirstName + " " + MiddleName + " " + LastName).Replace("  ", " ").Trim(); }
        }

        [Column("FATHER_NAME")]
        [MaxLength(50)]
        [Display(Name = "Father's Name")]
        public String FatherName { get; set; }

        [Column("MOTHER_NAME")]
        [MaxLength(50)]
        [Display(Name = "Mother's Name")]
        public String MotherName { get; set; }

        [Column("SPOUSE_NAME")]
        [MaxLength(50)]
        [Display(Name = "Spouse Name")]
        public String SpouseName { get; set; }

        [Column("DATE_OF_BIRTH")]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("MARITAL_STATUS_ID", TypeName = "int")]
        public int? MaritalStatusID { get; set; }

        [Column("HEIGHT_IN_INCH", TypeName = "int")]
        public int HeightInInch { get; set; }

        [Column("IDENTIFCATION_MARK")]
        [MaxLength(100)]
        [Display(Name = "Identification Mark")]
        public String IdentificationMark { get; set; }

        [Column("RESERVATION_CATEGORY_ID", TypeName = "int")]
        public int? ReservationCategoryID { get; set; }

        [Column("SUB_CATEGORY_ID", TypeName = "int")]
        public int? SubCategoryID { get; set; }

        [Column("RELIGION_ID", TypeName = "int")]
        public int? ReligionID { get; set; }

        [Column("CATEGORY_TYPE_ID", TypeName = "int")]
        public int? CategoryTypeID { get; set; }

        [Column("INJOB", TypeName = "int")]
        public int InJobValue { get; set; }
        
        [NotMapped]
        [Display(Name = "In Job")]
        public bool InJob { get { return InJobValue == 1; } set { InJobValue = value == true ? 1 : 0; } }
       
        public virtual Organization Organization { get; set; }
    }
}