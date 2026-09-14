using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
   public class CHMTProjectManualEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }
 
        [Column("Registration_No", TypeName = "bigint")]
        public Int64 RegistrationNo { get; set; }

        [Column("Name")]
        public String Name { get; set; }

        [Column("Father_Name")]
        [MaxLength(60)]
        [Display(Name = "Father's Name Name")]
        public String FatherName { get; set; }

        [Column("Dob")]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Column("Project_Receipt_Date")]
        [Display(Name = "Project Receipt Date")]
        public DateTime ProjectReceiptDate { get; set; }

        [Column("Whether_Finalized", TypeName = "bit")]
        [Display(Name = "Whether Finalized")]
        public Boolean? WhetherFinalized { get; set; }

        [Column("Finalized_Date")]
        [Display(Name = "Finalized Date")]
        public DateTime? FinalizedDate { get; set; }   

        [Column("Entered_By")]
        public String EnteredBy { get; set; }

        [Column("Entry_Date")]
        [Display(Name = "Entry Date")]
        public DateTime EntryDate { get; set; }

      





    }
}
