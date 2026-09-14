using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class DeficiencyCode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Code", TypeName = "int")]
        [Required(ErrorMessage = "Code is required")]
        public Int32 Code { get; set; }

        [Column("Description")]
        [MaxLength(1500)]
        [Display(Name = "Description")]
        public String Description { get; set; }

        [Column("Description_Regional")]
        [MaxLength(1500)]
        [Display(Name = "Description Regional")]
        public String DescriptionRegional { get; set; }

        [Column("Application_Type_ID", TypeName = "int")]
        [Required(ErrorMessage = "Application Type ID is required")]
        [Display(Name = "Application Type ID")]
        public Int32 ApplicationTypeID { get; set; }

        [ForeignKey("ApplicationTypeID")]
        public virtual ApplicationType ApplicationType { get; set; }

    }
}
