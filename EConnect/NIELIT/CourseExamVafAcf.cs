using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    [Table("Course_Exam_VAF_ACF")]
    public class CourseExamVafAcf
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Appl_Number")]
        public String Number { get; set; }


        [Column("Appl_Date")]        
        public DateTime? ApplicationDate { get; set; }

       

        [Column("Course_Category_ID", TypeName = "int")]
        [Required]
        public Int32 CourseCategoryID { get; set; }

       
        [Column("Course_ID", TypeName = "int")]
        [Required]  
        public Int32 CourseID { get; set; }
        public virtual Course Course { get; set; }


        [Column("Candidate_ID", TypeName = "bigint")]
        [Required]
        public Int64 CandidateID { get; set; }

        public virtual Candidate Candidate { get; set; }



        [Column("Exam_ID", TypeName = "int")]
        [Required]
        public Int32 ExamID { get; set; }
        public virtual Exam Exam { get; set; }


        [Column("Registration_Number", TypeName = "bigint")]
        [Required]
        public Int64 RegistrationNumber { get; set; }

       

        [Column("Institute_ID", TypeName = "bigint")]
        public Int64 InstituteID { get; set; }
        public virtual Institute Institute { get; set; }

        [Column("Institute_Name")]
        public String InstituteName { get; set; }

        [Column("Institute_Address")]
        public String InstituteAddress { get; set; }
       

        [Column("Fee_Type_ID", TypeName = "int")]
        [Required]
        public Int32 FeeTypeID { get; set; }

      

        [Column("TotalFee_Amount", TypeName = "decimal")]
        [Required]
        public Decimal FeeAmount { get; set; }


        [Column("VAF_Amount", TypeName = "decimal")]
        [Required]
        public Decimal VafAmount { get; set; }


        [Column("ACF_Amount", TypeName = "decimal")]
        [Required]
        public Decimal AcfAmount { get; set; }



        [Column("Final_Submitted", TypeName = "bit")]
        [Required]
        public Boolean FinalSubmitted { get; set; }

        [Column("Final_Submission_Date")]
        public DateTime FinalSubmissionDate { get; set; }


       

      

        [Column("Application_Status_ID", TypeName = "int")]
        [Required]
        public Int32 ApplicationStatusID { get; set; }


        [Column("Payment_Status_ID", TypeName = "int")]
        [Required]
        public Int32 PaymentStatusID { get; set; }

        public virtual PaymentStatus PaymentStatus { get; set; }



        [Column("Demand_Note_ID", TypeName = "bigint")]
        public Int64? DemandNoteID { get; set; }

        public virtual DemandNote DemandNote { get; set; }




    }
}
