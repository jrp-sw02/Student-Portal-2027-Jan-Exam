using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EConnect.NIELIT
{
    public class StudentSemesterAcademicDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", TypeName = "int")]
        public Int32 Id { get; set; }

        [Column("StudentId", TypeName = "bigint")]
        [Required(ErrorMessage = "Student ID is required")]
        [Display(Name = "StudentId")]
        public Int64 StudentId { get; set; }

        [Column("SemSubId", TypeName = "bigint")]
        [Required(ErrorMessage = "SemSubId is required")]
        [Display(Name = "SemSubId")]
        public Int64 SemSubId { get; set; }

        [Column("Result")]
        [Required(ErrorMessage = "Result is required")]
        [MaxLength(10)]
        [Display(Name = "Result")]
        public String Result { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        public Int64 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }
    }
}
