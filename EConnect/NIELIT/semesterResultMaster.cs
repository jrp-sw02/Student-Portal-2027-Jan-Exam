using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class semesterResultMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", TypeName = "int")]
        public Int32 Id { get; set; }    

        [Column("Result")]
        [Required(ErrorMessage = "Result Name is required")]
        [MaxLength(50)]
        [Display(Name = "Result")]
        public String Result { get; set; }

        [Column("Code")]
        [Required(ErrorMessage = "Result Code is required")]
        [MaxLength(10)]
        [Display(Name = "Code")]
        public String Code { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        public Int64 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }
    }
}
