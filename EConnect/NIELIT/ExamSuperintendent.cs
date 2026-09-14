using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
   public class ExamSuperintendent
    {
       [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       [Column("ID", TypeName = "int")]
       public Int32 ID { get; set; }

       [Column("Regional_Centre_ID", TypeName = "int")]
       public Int32 RegionalCentreID { get; set; }

       [Column("Exam_ID", TypeName = "int")]
       public Int32 ExamID { get; set; }

       [Column("Uploadded_File_ID", TypeName = "bigint")]
       public Int64 UploaddedFileID { get; set; }

       [Column("Created_By", TypeName = "int")]
       public Int32 CreatedBy { get; set; }

       [Column("Created_On")]
       public DateTime CreatedOn { get; set; }

       [ForeignKey("UploaddedFileID")]
       public virtual UploadedFile UploadFile { get; set; }

    }
}
