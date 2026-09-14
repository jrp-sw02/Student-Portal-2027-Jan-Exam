using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class ExamWiseExamCenter
    {
       
      
        [Column("Exam_ID", TypeName = "int")]
        [Required]
        public Int32 ExamID{ get; set; }
       
        [Column("Exam_Center_ID", TypeName = "int")]
        [Required]
        public Int32 ExamCenterID { get; set; }

        [Column("Exam_Centre_Type_ID", TypeName = "int")]
        [Required]
        public Int32 ExamCentreTypeID { get; set; }

        [Column("Created_By", TypeName = "int")]
        [Required]
        public Int32 CreatedByID { get; set; }

        [Column("Created_On")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [ForeignKey("ExamID")]
        public virtual Exam Exam { get; set; }

        [ForeignKey("ExamCenterID")]
        public virtual ExamCenter ExamCenter { get; set; }

        [NotMapped]
        public enmExamCenterType enmExamCenterType
        {
            get
            {
                return (enmExamCenterType)this.ExamCentreTypeID;
            }
            set
            {
                this.ExamCentreTypeID = (int)value;
            }
        }
    }
}
