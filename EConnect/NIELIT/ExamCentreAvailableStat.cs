using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class ExamCentreAvailableStat
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

        [Column("Exam_Centre_CCC_Id", TypeName = "bigint")]
        public Int64 CentreCCCId { get; set; }

        [Column("Exam_Centre_BatchSize", TypeName = "smallint")]
        public Int16 CentreBatchSize { get; set; }

        [Column("Exam_Centre_BatchBufferSize", TypeName = "smallint")]
        public Int16 CentreBatchBufferSize { get; set; }

        [Column("Exam_Centre_Address", TypeName = "varchar")]
        public String CentreAddress { get; set; }

        [Column("Exam_Centre_Contact_Person", TypeName = "varchar")]
        public String CentreContactPerson { get; set; }

        [Column("Exam_Centre_Contact_Email", TypeName = "varchar")]
        public String CentreContactEmail { get; set; }

        [Column("Exam_Centre_Contact_Mobile", TypeName = "bigint")]
        public Int64 CentreContactMobile { get; set; }

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
