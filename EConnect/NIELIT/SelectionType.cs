using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class SelectionType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [MaxLength(50)]
        [Required]
        public String Name { get; set; }

        [Column("Code")]
        [MaxLength(1)]
        [Required]
        public String Code { get; set; }
    }
}
