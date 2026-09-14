using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class RegistrationStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [MaxLength(30)]
        public String Name { get; set; }

        [Column("Code")]
        [MaxLength(1)]
        public String Code { get; set; }

        [Column("Description")]
        [MaxLength(100)]
        public String Description { get; set; }
    }
}
