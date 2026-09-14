using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect
{
    public class tblGender
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("salutation")]
        [MaxLength(10)]
        public String salutation { get; set; }

        [Column("name")]
        [MaxLength(50)]
        public String name { get; set; }

        [Column("name_regional")]
        [MaxLength(50)]
        public String name_regional { get; set; }

        [Column("genderCode")]
        public String genderCode { get; set; }

        [Column("enterDate", TypeName = "datetime")]
        public DateTime enterDate { get; set; }

        [Column("enterBy", TypeName = "int")]
        public Int32 enterBy { get; set; }
    }
}
