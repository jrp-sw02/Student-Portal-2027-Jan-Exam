using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect
{
   public class Candidate_Qualifications_UpdateTemp
    {
       [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       [Column("SL", TypeName = "bigint")]
       public Int64 SL { get; set; }

       [Column("Candidate_ID", TypeName = "bigint")]
       public Int64 Candidate_ID { get; set; }

       [Column("Educational_Qualification_ID", TypeName = "int")]
       public Int32 Educational_Qualification_ID { get; set; }

       [Column("Passing_Year", TypeName = "int")]
       public Int32 Passing_Year { get; set; }

       [Column("Other_Qualification")]
       [MaxLength(50)]
       public String Other_Qualification { get; set; }

       [Column("Effective_From_Date")]
       public DateTime Effective_From_Date { get; set; }
      
       [Column("Client_IPAddress")]
       [MaxLength(50)]
       public String Client_IPAddress { get; set; }

       [Column("Client_UserId")]
       [MaxLength(50)]
       public String Client_UserId { get; set; }

       [Column("Client_HostName")]
       [MaxLength(50)]
       public String Client_HostName { get; set; }

       [Column("Update_DateTime")]
       public DateTime Update_DateTime { get; set; }
    }
}
