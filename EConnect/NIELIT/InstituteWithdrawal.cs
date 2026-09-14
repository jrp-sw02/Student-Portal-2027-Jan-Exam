using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class InstituteWithdrawal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("SrNO", TypeName = "int")]
        [Display(Name = "Sr Number")]
        public Int32? SrNO { get; set; }

        [Column("ccc_no")]
        [Required(ErrorMessage = "CCC No is required")]
        [MaxLength(25)]
        [Display(Name = "CCC No")]
        public String ccc_no { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Institute Name is required")]
        [MaxLength(150)]
        [Display(Name = "Institute Name")]
        public String Name { get; set; }

        [Column("Address")]
        [MaxLength(750)]
        [Display(Name = "Address")]
        public String Address { get; set; }

        [Column("City_Name")]
        [MaxLength(50)]
        [Display(Name = "City Name")]
        public String CityName { get; set; }


        [Column("StateName")]
        [MaxLength(100)]
        [Display(Name = "State Name")]
        public String StateName { get; set; }
                   

        [Column("Pin_Code", TypeName = "int")]
        [Display(Name = "Pin Code")]
        public Int32? PinCode { get; set; }

        [Column("withdrawalDate")]
        public DateTime? withdrawalDate { get; set; }

        [Column("uploadFileName")]
        [MaxLength(250)]
        public string uploadFileName { get; set; }


        [Column("enterBy", TypeName = "int")]
        [Required(ErrorMessage = "Enter By is required")]
        public Int32 enterByID { get; set; }


        [Column("enterDate")]
        public DateTime enterDate { get; set; }

        [Column("uploadStatus")]
        [MaxLength(25)]
        public string uploadStatus { get; set; }

        [Column("Remarks")]
        [MaxLength(100)]
        public string Remarks { get; set; }
     
    }
}
