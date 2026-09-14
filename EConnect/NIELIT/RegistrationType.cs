using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class RegistrationType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public int ID { get; set; }

        [Column("NAME")]
        [MaxLength(50)]
        [Required]
        public String Name { get; set; }

        [Column("CODE")]
        [MaxLength(5)]
        public String Code { get; set; }
    }
}
