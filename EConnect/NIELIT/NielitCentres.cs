using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class NielitCentres
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }


        [Column("instituteID", TypeName = "bigint")]
        public Int64 instituteID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(150)]
        [Display(Name = "Name")]
        public String Name { get; set; }

        [Column("Address1")]
        [Required(ErrorMessage = "Address1 is required")]
        [MaxLength(250)]
        [Display(Name = "Address1")]
        public String Address1 { get; set; }

        [Column("Address2")]
        [Required(ErrorMessage = "Address2 is required")]
        [MaxLength(250)]
        [Display(Name = "Address2")]
        public String Address2 { get; set; }

        [Column("Address3")]
        [Required(ErrorMessage = "Address3 is required")]
        [MaxLength(250)]
        [Display(Name = "Address3")]
        public String Address3 { get; set; }

        [Column("City_Name")]
        [MaxLength(50)]
        [Display(Name = "City Name")]
        public String cityName { get; set; }

        [Column("mobile", TypeName = "bigint")]
        public Int64 mobile { get; set; }

        [Column("linkedToCentre", TypeName = "bigint")]
        public Int64 linkedToCentre { get; set; }


        [Column("email1")]
        [MaxLength(70)]
        [Display(Name = "Email1")]
        public String email1 { get; set; }

        [Column("enterBy", TypeName = "int")]
        [Display(Name = "User Name")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }

    }
}
