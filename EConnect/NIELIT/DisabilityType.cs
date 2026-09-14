using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class DisabilityType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [MaxLength(100)]
        public String Name { get; set; }

        [Column("Code")]
        [MaxLength(5)]
        public String Code { get; set; }
    }
}
