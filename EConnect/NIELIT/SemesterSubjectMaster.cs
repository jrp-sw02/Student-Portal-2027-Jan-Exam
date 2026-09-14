using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class SemesterSubjectMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", TypeName = "int")]
        public Int32 Id { get; set; }

        [Column("SubId", TypeName = "int")]
        [Required(ErrorMessage = "Subject ID is required")]
        [Display(Name = "SubId")]
        public Int32 SubId { get; set; }

        [Column("BatchId", TypeName = "int")]
        [Required(ErrorMessage = "BatchId is required")]
        [Display(Name = "BatchId")]
        public Int32 BatchId { get; set; }

        [Column("SemNo", TypeName = "int")]
        [Required(ErrorMessage = "Semester No is required")]
        [Display(Name = "SemNo")]
        public Int32 SemNo { get; set; }

        [Column("NoOfCredits", TypeName = "int")]
        [Required(ErrorMessage = "NoOfCredits is required")]
        [Display(Name = "NoOfCredits")]
        public Int32 NoOfCredits { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        public Int64 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }
    }
}
