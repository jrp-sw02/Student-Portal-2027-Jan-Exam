using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class ExamSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage="Session Name is required")]
        [MaxLength(20)]
        [Display(Name="Exam Session Name")]
        public String Name { get; set; }

        [Column("Name_Regional")]
        [MaxLength(30)]
        [Display(Name = "Exam Session Regional Name")]
        public String NameRegional { get; set; }

        [Column("Code")]
        [MaxLength(2)]
        [Display(Name="Exam Session Code")]
        public String  Code { get; set; }

        [Column("Start_Time")]
        [Display(Name = "Session Start Time")]
        public DateTime StartTime { get; set; }

        [Column("End_Time")]
        [Display(Name = "Session End Time")]
        public DateTime EndTime { get; set; }
    }
}
