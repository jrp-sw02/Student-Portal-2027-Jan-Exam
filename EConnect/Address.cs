using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Address_Type_ID", TypeName="int")]
        [Required(ErrorMessage = "Address TypeIDe required")]
        [Display(Name = "Address Type")]
        public Int32 AddressTypeID { get; set; }

        [Column("Address1")]
        [Required(ErrorMessage = "Address Line1 required")]
        [MaxLength(100)]
        [Display(Name = "Address Line1")]
        public String AddressLine1 { get; set; }

        [Column("Address2")]       
        [MaxLength(100)]
        [Display(Name = "Address Line2")]
        public String AddressLine2 { get; set; }

        [Column("Address3")]
        [MaxLength(100)]
        [Display(Name = "Address Line3")]
        public String AddressLine3 { get; set; }

        [Column("Country_ID", TypeName = "bigint")]
        [Required(ErrorMessage = "Country required")]
        [Display(Name = "Country")]
        public Int64 CountryID { get; set; }

        [Column("State_ID", TypeName = "bigint")]
        [Display(Name = "State")]
        public Int64? StateID { get; set; }

        [Column("District_ID", TypeName = "bigint")]
        [Display(Name = "District")]
        public Int64? DistrictID { get; set; }

        [Column("City_Name")]
        [MaxLength(50)]
        [Display(Name = "City Name")]
        public String CityName { get; set; }

        [Column("Pin_Code", TypeName = "int")]
        [Required(ErrorMessage = "Pin Code required")]
        [Display(Name = "Pin Code")]
        public Int32? PinCode { get; set; }

        [Column("Candidate_ID", TypeName = "bigint")]
        [Display(Name = "Candidate Name")]
        public Int64? CandidateID { get; set; }

        [Column("Effective_From_Date")]
        public DateTime EffectiveDateFrom { get; set; }

        [Column("Created_On")]
        [Required()]
        public DateTime CreatedOn { get; set; }

        [Column("Created_By", TypeName = "int")]
        [Required()]
        public Int32 CreatedByID { get; set; }

        [ForeignKey("CreatedByID")]
        public virtual URM.User CreatedByUser { get; set; }

        [Column("Is_Verified", TypeName = "bit")]
        [Required()]
        public Boolean  IsVerified { get; set; }

        [Column("Verified_By", TypeName = "int")]        
        public Int32? VerifiedByID { get; set; }

        [ForeignKey("VerifiedByID")]
        public virtual URM.User VerifiedByUser { get; set; }

        [ForeignKey("AddressTypeID")]
        public virtual AddressType AddressType { get; set; }

        [NotMapped]
        public enmAddressType enmAddressType
        {
            get
            {
                return (enmAddressType)this.AddressTypeID;
            }
            set
            {
                this.AddressTypeID = (int)value;
            }
        }

        [ForeignKey("CountryID")]
        public virtual Location  Country { get; set; }

        [ForeignKey("StateID")]
        public virtual Location State { get; set; }

        [ForeignKey("DistrictID")]
        public virtual Location District { get; set; }
    }
}
