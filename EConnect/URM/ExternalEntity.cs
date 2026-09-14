using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.URM
{
    public class ExternalEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Entity Name is required")]
        [Column("NAME")]
        [MaxLength(100)]
        [Display(Name = "Name")]
        public String Name { get; set; }

        [Column("MOBILE_NO", TypeName="bigint")]
        [Display(Name = "Mobile Number")]
        public Int64 MobileNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [Column("EMAIL")]
        [MaxLength(100)]
        [Display(Name = "Email Address")]
        public String Email { get; set; }

        [Column("ADDRESS")]
        [MaxLength(300)]
        [Display(Name = "Address")]
        public String Address { get; set; }

        [Column("COMMENTS")]
        [MaxLength(200)]
        [Display(Name = "Description")]
        public String Description { get; set; }

        [Required(ErrorMessage = "Created By Id is required")]
        [Column("CREATED_BY", TypeName = "int")]
        public int CreatedBy { get; set; }

        [Column("CREATED_ON")]
        public DateTime CreatedOn { get; set; }
    }
}
