using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
   public class tempExamResultPub
    {
         [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("examResultPubId", TypeName = "bigint")]
        public Int64 examResultPubId { get; set; }

        [Column("examId", TypeName = "int")]
        [Required(ErrorMessage = "Exam is required")]
        [Display(Name = "Exam")]
        public Int32 examId { get; set; }

        [Column("RegionalCentreId",TypeName = "int")]
        [Required(ErrorMessage = "Regional Centre is required")]
        [Display(Name = "Regional Centre")]
        public Int32 RegionalCentreId { get; set; } 

        [Column("DateOfPublishing")]
        [Display(Name = "Date of Publishing")]
        public DateTime DateOfPublishing { get; set; }

        [Column("examCentreCount")]
        [Display(Name = "Exam Centre Count")]
        public int examCentreCount { get; set; }

        [Column("centreCode")]
        [Display(Name = "Centre Code")]
        public string centreCode { get; set; }


       [ForeignKey("examId")]
        public virtual Exam Exam{ get; set; }

      [ForeignKey("RegionalCentreId")]
      public virtual RegionalCenter  RegionalCenter { get; set; }

      
        
    }


   
    
}
