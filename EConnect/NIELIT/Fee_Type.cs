using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class FeeType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public int ID { get; set; }

        [Column("NAME")]
        [MaxLength(50)]
        public String Name { get; set; }

        [Column("NAME_REGIONAL")]
        [MaxLength(200)]
        public String NameRegional { get; set; }

        [Column("Related_To", TypeName = "int")]
        public Int32 RelatedTo { get; set; }
    }
}
