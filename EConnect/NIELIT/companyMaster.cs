using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class companyMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("companyName"),]
        [MaxLength(100)]
        [Display(Name = "companyName")]
        public String company_Name { get; set; }

        [Column("comapnyAddress")]

        [MaxLength(100)]
        [Display(Name = "Comapny Address")]
        public String comapny_Address { get; set; }

        [Column("comapnyAddress2")]

        [MaxLength(100)]
        [Display(Name = "Comapny Address2")]
        public String comapny_Address2 { get; set; }

        [Column("comapnyAddress3")]

        [MaxLength(100)]
        [Display(Name = "Comapny Address3")]
        public String comapny_Address3 { get; set; }


        [Column("comapnyCountry_ID", TypeName = "bigint")]

        [Display(Name = "CountryID")]
        public Int64? CompnyCountry_ID { get; set; }

        [Column("comapnyState_ID", TypeName = "bigint")]
        [Display(Name = "State")]
        public Int64? compnyState_ID { get; set; }

        [Column("comapnyDistrict_ID", TypeName = "bigint")]
        [Display(Name = "District")]
        public Int64? compnyDistrict_ID { get; set; }

        [Column("comapnyCity_Name")]
        [MaxLength(50)]
        [Display(Name = "City Name")]
        public String CompnyCityName { get; set; }

        [Column("comapnyPin_Code1", TypeName = "int")]

        [Display(Name = "Pin Code")]
        public Int32? CompnyPinCode { get; set; }

        [Column("contactPersonName")]
        [MaxLength(100)]
        [Display(Name = "Person Name")]
        public String contactPersonName { get; set; }

        [Column("contactPersonMobile", TypeName = "bigint")]
        public Int64? contactPersonMobile { get; set; }

        [Column("Email")]
        [MaxLength(100)]
        [Display(Name = "Contact Person Email")]
        public String PersonEmail { get; set; }

        [Column("enterBy", TypeName = "int")]
        public Int32 enter_By { get; set; }

        [Column("enterDate")]
        public DateTime? enter_Date { get; set; }

    }
}
