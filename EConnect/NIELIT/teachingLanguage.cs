using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect
{
    public class teachingLanguage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Language")]
        [Required(ErrorMessage = "Language Name is required")]
        [MaxLength(250)]
        [Display(Name = "Language Name")]
        public String Name { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        [Required(ErrorMessage = "Enter By is required")]
        public Int64 enterByID { get; set; }

        [Column("enterDate", TypeName = "DateTime")]
        public DateTime enterDate { get; set; }
       
    }
}
