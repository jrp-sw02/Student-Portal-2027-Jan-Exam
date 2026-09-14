using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class SemesterMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", TypeName = "int")]
        public Int32 Id { get; set; }

        [Column("CourseId", TypeName = "bigint")]
        [Required(ErrorMessage = "Course ID is required")]
        [Display(Name = "CourseId")]
        public Int64 CourseId { get; set; }

        [Column("BatchId", TypeName = "bigint")]
        [Required(ErrorMessage = "BatchId is required")]
        [Display(Name = "BatchId")]
        public Int64 BatchId { get; set; }

        [Column("NoOfSems", TypeName = "int")]
        [Required(ErrorMessage = "Semester No is required")]
        [Display(Name = "NoOfSems")]
        public Int32 NoOfSems { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        public Int64 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }
    }
}
