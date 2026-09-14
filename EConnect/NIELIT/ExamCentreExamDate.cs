using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class ExamCentreExamDate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Exam_id", TypeName = "int")]
        public Int32 ExamId { get; set; }

        [Column("Regional_Centre_Id", TypeName = "int")]
        public Int32 RegionalCentreId { get; set; }

        [Column("Exam_City_Code", TypeName = "varchar")]
        public String CityCode { get; set; }

        [Column("Exam_Centre_Code", TypeName = "varchar")]
        public String CentreCode { get; set; }

        [Column("Exam_Date")]
        public DateTime ExamDate { get; set; }

        [Column("Created_On")]
        public DateTime CreatedOn { get; set; }

        [Column("Created_By", TypeName = "bigint")]
        public Int64 CreatedByID { get; set; }

        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; }

        [ForeignKey("RegionalCentreId")]
        public virtual RegionalCenter RegionalCenter { get; set; }
    }
}

