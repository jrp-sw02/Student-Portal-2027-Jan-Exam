using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
  public class ApplicationStatus
    {       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(200)]
        public String Name { get; set; }

        [Column("Description")]
        [MaxLength(300)]
        public String Description { get; set; }
    }
}
