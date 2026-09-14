using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class UmcDecisionType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [Required]
        [MaxLength(100)]
        public String Name { get; set; }

        [Column("Description")]
        [MaxLength(200)]
        public String Description { get; set; }

        [Column("Debar_Chances", TypeName = "int")]
        [Required]
        public Int32 DebarChances { get; set; }
    }
}
