using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class NonAffInstitute
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Institute Name is required")]
        [MaxLength(100)]
        [Display(Name = "Course Name")]
        public String Name { get; set; }

        [Column("NAME_REGIONAL")]
        [MaxLength(200)]
        [Display(Name = "Institute Regional Name")]
        public String NameRegional { get; set; }

        [Column("Contact_Person_Name")]
        [MaxLength(100)]
        [Display(Name = "Contact Person Name")]
        public String ContactPersonName { get; set; }

        [Column("Contact_Person_Post")]
        [MaxLength(50)]
        [Display(Name = "Contact Person Post")]
        public String ContactPersonPost { get; set; }

        [Column("Std_No", TypeName = "int")]
        [Display(Name = "Std Number")]
        public Int32? StdNumber { get; set; }

        [Column("Phone1", TypeName = "int")]
        [Display(Name = "Phone Number1")]
        public Int32? PhoneNumber1 { get; set; }

        [Column("Phone2", TypeName = "int")]
        [Display(Name = "Phone Number2")]
        public Int32? PhoneNumber2 { get; set; }

        [Column("Mobile", TypeName = "bigint")]
        [Display(Name = "Mobile Number")]
        public Int64? MobileNumber { get; set; }

        [Column("Fax", TypeName = "int")]
        [Display(Name = "Fax Number2")]
        public Int32? FaxNumber { get; set; }

        [Column("email1")]
        [MaxLength(70)]
        [Display(Name = "Email Address1")]
        public String EmailAddress1 { get; set; }

        [Column("email2")]
        [MaxLength(70)]
        [Display(Name = "Email Address2")]
        public String EmailAddress2 { get; set; }

        [Column("website")]
        [MaxLength(50)]
        [Display(Name = "Web Address")]
        public String WebAddress { get; set; }

        [Column("Address1")]
        [MaxLength(250)]
        [Display(Name = "Address Line1")]
        public String AddressLine1 { get; set; }

        [Column("Address2")]
        [MaxLength(250)]
        [Display(Name = "Address Line2")]
        public String AddressLine2 { get; set; }

        [Column("Address3")]
        [MaxLength(100)]
        [Display(Name = "Address Line3")]
        public String AddressLine3 { get; set; }

        [Column("State_ID", TypeName = "bigint")]
        [Display(Name = "State")]
        public Int64 StateID { get; set; }

        [Column("District_ID", TypeName = "bigint")]
        [Display(Name = "District")]
        public Int64? DistrictID { get; set; }

        [Column("City_Name")]
        [MaxLength(50)]
        [Display(Name = "City Name")]
        public String CityName { get; set; }

        [Column("Pin_Code", TypeName = "int")]
        [Display(Name = "Pin Code")]
        public Int32? PinCode { get; set; }

        //Added 13 feb 2019
        [Column("cityTypeID", TypeName = "int")]
        [Display(Name = "City Type")]
        public Int32 cityTypeID { get; set; }

        public virtual ICollection<AccreditationDetail> AccreditationDetails { get; set; }

        [ForeignKey("StateID")]
        public virtual Location State { get; set; }

        [ForeignKey("DistrictID")]
        public virtual Location District { get; set; }

        // Added 27 May 2019
        [Column("GSTNo")]
        public String GSTNo { get; set; }
        //

        //Added 13 feb 2019
        [ForeignKey("cityTypeID")]
        public virtual cityType cityType { get; set; }

        [Column("linkedToCentre", TypeName = "bigint")]
        [Display(Name = "linked To Centre")]
        public Int32 linkedToCentre { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        [Display(Name = "User Name")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }

        public virtual ICollection<RegistrationDetail> RegistrationDetails { get; set; }

    }
}
