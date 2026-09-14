using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public  class PracticalCandidateMarks
    {
        [Key]
        [Column("ID", TypeName = "bigint")]
        public Int32 ID { get; set; }

        //[Column("Exam_Month", TypeName = "tinyint")]
        //public Int64 ExamMonth { get; set; }

        [Column("Exam_Month")]
        [Display(Name = "Exam_Month")]
        public byte ExamMonth { get; set; }

        [Column("Exam_Year", TypeName = "smallint")]
        public Int16 ExamYear { get; set; }

        [Column("Center_Code")]
        [MaxLength(50)]
        public String CenterCode { get; set; }

        [Column("Registration_no", TypeName = "bigint")]
        public Int32 RegistrationNo { get; set; }

        [Column("Level_code", TypeName = "char")]
        [MaxLength(1)]
        public string LevelCode { get; set; }

        //[Column("Level_code")]
        //[MaxLength(1)]
        //public String LevelCode { get; set; }

        [Column("module_id", TypeName = "smallint")]
        public Int16 ModuleId { get; set; }

        [Column("Batch_code")]
        [MaxLength(10)]
        public String BatchCode { get; set; }

        [Column("Exam_date")]
        public DateTime? ExamDate { get; set; }

        [Column("Exam_report_time")]
        [MaxLength(50)]
        public String ExamReportTime { get; set; }

        [Column("Examiner_marks_40", TypeName = "Decimal")]
        public  Decimal ? ExaminerMarks40 { get; set; }

        [Column("Examiner_marks_40_entry_date")]
        public DateTime? ExaminerMarks40EntryDate { get; set; }

        [Column("Observer_marks_40", TypeName = "Decimal")]
        public Decimal? ObserverMarks40 { get; set; }

        [Column("Observer_marks_40_entry_date")]
        public DateTime? ObserverMarks40EntryDate { get; set; }

        [Column("Observer_marks_20", TypeName = "Decimal")]
        public Decimal? ObserverMarks20 { get; set; }


        [Column("Observer_marks_20_entry_date")]
        public DateTime? ObserverMarks20EntryDate { get; set; }

        [Column("Remarks")]
        [MaxLength(500)]
        public String Remarks { get; set; }

    }
}
