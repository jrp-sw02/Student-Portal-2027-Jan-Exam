using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class ExamCentreChoiceRefilling
    {
        [Key, Column("id", TypeName = "int", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Column("id", TypeName = "int")]
        public Int32 id { get; set; }

        [Key, Column("Application_Id", Order = 1)]
        //[Column("project_name")]        
        [Required(ErrorMessage = "Application Id is required")]
        [MaxLength(60)]
        [Display(Name = "Application Id")]
        public String ApplicationId { get; set; }

        [Column("Exam_Id", TypeName = "int")]
        public Int32 ExamId { get; set; }

        [Column("Exam_Centre1_ID", TypeName = "int")]
        public Int32 ExamCentre1_ID { get; set; }

        [Column("Exam_Centre2_ID", TypeName = "int")]
        public Int32 ExamCentre2_ID { get; set; }

        [Column("New_ExamCentre_ID", TypeName = "int")]
        public Int32 NewExamCentre_ID { get; set; }

        [Column("New_Exam_Id", TypeName = "int")]
        public Int32 New_Exam_Id { get; set; }

        //[Key, Column("Agreed_Disagreed_Flag", Order = 2)]
        [Column("Agreed_Disagreed_Flag")]        
        //[Required(ErrorMessage = "agreed_disagreed_flag is required")]
        [MaxLength(1)]
        [Display(Name = "Agreed Disagreed Flag")]
        public String Agreed_Disagreed_Flag { get; set; }

        [Column("Agreed_Disagreed_Timestamp")]
        [Required]
        public DateTime Agreed_Disagreed_Timestamp { get; set; }

        [Column("Otp_Number", TypeName = "int")]
        public Int32 Otp_Number { get; set; }

        [Column("Validation_Flag")]        
        [MaxLength(1)]
        [Display(Name = "Validation Flag")]
        public String Validation_Flag { get; set; }

        [Column("Validation_Timestamp")]        
        public DateTime? Validation_Timestamp { get; set; }

        [Column("Centrechange_Optout")]
        //[Required(ErrorMessage = "agreed_disagreed_flag is required")]
        [MaxLength(1)]
        [Display(Name = "Centrechange Optout")]
        public String Centrechange_Optout { get; set; }
        
    }
}
