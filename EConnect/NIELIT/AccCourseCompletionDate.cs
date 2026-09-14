using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class AccCourseCompletionDate
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int32 ID { get; set; }

        [Key]
        [Column("Registration_No", Order = 0, TypeName = "bigint")]
        public Int64 RegistrationNo { get; set; }

        [Column("Name")]      
        public String Name { get; set; }

        [Key]
        [Column("Course_Id", Order = 1, TypeName = "int")]
        public Int32 CourseId { get; set; }

        [Column("Exam_Id", TypeName = "int")]
        public Int32? RegnCycleId { get; set; }

        [Column("Institute_Id", TypeName = "bigint")]
        public Int32 InstituteId { get; set; }

        [Column("StartDate")]
        public DateTime StartDate { get; set; }

        [Column("EndDate")]
        public DateTime EndDate { get; set; }

        [Column("Is_Verified", TypeName = "bit")]
        public Boolean? IsVerified { get; set; }

        [Column("Phase_No", TypeName = "int")]
        public Int32? PhaseNo { get; set; }

        [Column("PhaseDate")]
        public DateTime? PhaseDate { get; set; }

        [Column("Certificate_Issued", TypeName = "bit")]      
        public Boolean? CertificateIssued { get; set; }

        [Column("Last_Modificatation_Date")]
        public DateTime? LastModificatationDate { get; set; }



    }
}
