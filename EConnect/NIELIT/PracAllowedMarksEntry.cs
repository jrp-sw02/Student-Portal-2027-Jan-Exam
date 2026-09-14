using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class PracAllowedMarksEntry
    {

        [Key]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Center_Code")]
        [MaxLength(10)]
        public String CenterCode { get; set; }

        [Column("Exam_date")]
        public DateTime? ExamDate { get; set; }

        [Column("Exam_Session")]
        [MaxLength(50)]
        public String ExamSession { get; set; }

        [Column("Batch")]
        [MaxLength(10)]
        public String Batch { get; set; }

    }
}
