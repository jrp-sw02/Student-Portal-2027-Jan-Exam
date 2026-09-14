using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class OnlineProtsahanExamModules
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("OnlineRefNo")]        
        [MaxLength(50)]
        public String OnlineRefNo { get; set; }

        [Column("Candidate_ID", TypeName = "bigint")]
        public Int64 Candidate_ID { get; set; }
       
        [Column("RegnNo", TypeName = "bigint")]
        public Int64 RegnNo { get; set; }

        [Column("ExamID", TypeName = "bigint")]
        public Int64 ExamID { get; set; }

        [Column("ModuleID", TypeName = "bigint")]
        public Int64 ModuleID { get; set; }

        [Column("isVerifiedByExam", TypeName = "bit")]
        public Boolean? isVerifiedByExam { get; set; }

        [Column("isProcessed", TypeName = "bit")]
        public Boolean? isProcessed { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        public Int64? enterBy { get; set; }

        [Column("enterDate")]
        public DateTime? enterDate { get; set; }
       
    }
}
