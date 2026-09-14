using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class UpdateFields
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public int ID { get; set; }

        [Column("Field_Name")]
        [Required(ErrorMessage = "Filed Name is required")]
        [MaxLength(200)]
        [Display(Name = "Field Name")]
        public String FieldName { get; set; }

        [Column("Is_Update", TypeName = "bit")]
        public Boolean IsUpdate { get; set; }
    }
}
