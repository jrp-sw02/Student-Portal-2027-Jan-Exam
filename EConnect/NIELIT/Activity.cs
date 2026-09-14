using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class Activity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage="Activity Name is Required")]
        [MaxLength(150)]
        [Display(Name = "Activity Name")]
        public String Name { get; set; }

        [Column("Name_Regional")]
        [MaxLength(30)]
        [Display(Name = "Exam Session Regional Name")]
        public String NameRegional { get; set; }

        [Column("Code")]
        [MaxLength(10)]
        [Display(Name = "Exam Session Code")]
        public String Code { get; set; }
    }
}
