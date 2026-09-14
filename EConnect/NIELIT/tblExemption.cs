using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class tblExemption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Course_Category_ID")]
        [Required(ErrorMessage = "Course_Category_ID is Required")]
        [Display(Name = "Course_Category_ID")]
        public Int32 Course_Category_ID { get; set; }

        [Column("Candidate_ID")]
        [Required(ErrorMessage = "Candidate_ID is Required")]
        [Display(Name = "Candidate_ID")]
        public Int64 Candidate_ID { get; set; }

        [Column("Registration_Number")]
        [Required(ErrorMessage = "Registration_Number is Required")]
        [Display(Name = "Registration_Number")]
        public Int64 Registration_Number { get; set; }

        [Column("Base_Course_ID")]
        [Required(ErrorMessage = "Base_Course_ID is Required")]
        [Display(Name = "Base_Course_ID")]
        public int Base_Course_ID { get; set; }

        [Column("Base_Revision_Number")]
        [Required(ErrorMessage = "Base_Revision_Number is Required")]
        [Display(Name = "Base_Revision_Number")]
        public int Base_Revision_Number { get; set; }

        [Column("Base_Module_ID")]
        [Required(ErrorMessage = "Base_Module_ID is Required")]
        [Display(Name = "Base_Module_ID")]
        public int Base_Module_ID { get; set; }

        [Column("Exempted_Course_ID")]
        [Required(ErrorMessage = "Exempted_Course_ID is Required")]
        [Display(Name = "Exempted_Course_ID")]
        public int Exempted_Course_ID { get; set; }

        [Column("Exempted_Revision_Number")]
        [Required(ErrorMessage = "Exempted_Revision_Number is Required")]
        [Display(Name = "Exempted_Revision_Number")]
        public int Exempted_Revision_Number { get; set; }

        [Column("Exempted_Module_ID")]
        [Required(ErrorMessage = "Exempted_Module_ID is Required")]
        [Display(Name = "Exempted_Module_ID")]
        public int Exempted_Module_ID { get; set; }

        [Column("Exam_ID")]
        [Display(Name = "Exam_ID")]
        public Int32 Exam_ID { get; set; }

        [Column("Exempted_On")]
        [Display(Name = "Exempted_On")]
        public DateTime Exempted_On { get; set; }
    }
}
