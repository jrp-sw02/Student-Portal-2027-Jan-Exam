using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.HRMS
{
    public class Organization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public int ID { get; set; }

        [Column("NAME")]
        [Required(ErrorMessage = "Organization Name is required")]
        [MaxLength(100)]
        [Display(Name = "Organization Name")]
        public String Name { get; set; }

        [Column("NAME_REGIONAL")]
        [MaxLength(200)]
        [Display(Name = "Organization Regional Name")]
        public String NameRegional { get; set; }

        [Column("Main_Heading")]
        [Display(Name = "Main Heading")]
        public String MainHeading { get; set; }

        [Column("Main_Heading_Regional")]
        [Display(Name = "Main Heading Regional")]
        public String MainHeadingRegional { get; set; }

        [Column("Sub_Heading")]
        [Display(Name = "Sub Heading")]
        public String SubHeading { get; set; }

        [Column("Sub_Heading_Regional")]
        [Display(Name = "Sub Heading Regional")]
        public String SubHeadingRegional { get; set; }

        [Column("Address1")]
        [MaxLength(100)]
        [Display(Name = "Address Line 1")]
        public String AddressLine1 { get; set; }

        [Column("Address1_Regional")]
        [MaxLength(150)]
        [Display(Name = "Address Line 1 Regional")]
        public String AddressLine1Regional { get; set; }

        [Column("Address2")]
        [MaxLength(100)]
        [Display(Name = "Address Line 2")]
        public String AddressLine2 { get; set; }

        [Column("Address2_Regional")]
        [MaxLength(150)]
        [Display(Name = "Address Line 2 Regional")]
        public String AddressLine2Regional { get; set; }

        [Column("State_ID", TypeName="bigint")]
        [Display(Name = "State ID")]
        public Int64 StateID { get; set; }

        [Column("City")]
        [MaxLength(100)]
        [Display(Name = "AddressCity Name")]
        public String CityName { get; set; }

        [Column("City_Regional")]
        [MaxLength(150)]
        [Display(Name = "City Name Regional")]
        public String CityNameRegional { get; set; }

        [Column("Pin")]
        [MaxLength(6)]
        [Display(Name = "Pin Code")]
        public String PinCode { get; set; }

        [Column("Phone1")]
        [MaxLength(15)]
        [Display(Name = "Phone Number 1")]
        public String PhoneNumber1 { get; set; }

        [Column("Phone2")]
        [MaxLength(15)]
        [Display(Name = "Phone Number 2")]
        public String PhoneNumber2 { get; set; }

        [Column("Phone3")]
        [MaxLength(15)]
        [Display(Name = "Phone Number 3")]
        public String PhoneNumber3 { get; set; }

        [Column("Phone4")]
        [MaxLength(15)]
        [Display(Name = "Phone Number 4")]
        public String PhoneNumber4 { get; set; }

        [Column("Fax")]
        [MaxLength(15)]
        [Display(Name = "Fax Number 1")]
        public String FaxNumber { get; set; }

        [Column("Email")]
        [MaxLength(100)]
        [Display(Name = "Email Address")]
        public String EmailAddress { get; set; }

        [Column("WebSite")]
        [MaxLength(100)]
        [Display(Name = "Web Site")]
        public String WebSite { get; set; }

        [Column("ICON")]
        [Display(Name = "Icon Image")]
        public byte[] Icon { get; set; }

        [Column("Logo")]
        [Display(Name = "Logo Image")]
        public byte[] Logo { get; set; }

        [Column("ICON_PATH")]
        [MaxLength(100)]
        [Display(Name = "Icon Path")]
        public String IconPath { get; set; }

        [Column("LOGO_PATH")]
        [MaxLength(100)]
        [Display(Name = "Logo Path")]
        public String LogoPath { get; set; }

        [MaxLength(200)]
        [Column("Bank_Account_Name")]
        public String BankAccountName { get; set; }

        [MaxLength(100)]
        [Column("Bank_Branch")]
        public String BankBranchName { get; set; }

        [Column("Tech_Email")]
        [MaxLength(70)]
        public String EmailTechnicalPerson { get; set; }

        [Column("Tech_Mobile", TypeName = "bigint")]
        public Int64? MobileNumberTechnicalPerson { get; set; }

        [Column("Visitor_Counter", TypeName = "bigint")]
        public Int64 VisitorCounter { get; set; }
    }
}
