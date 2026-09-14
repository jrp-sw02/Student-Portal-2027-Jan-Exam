using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CandidateRequestType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        public String Name { get; set; }

        [Column("Description")]
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(200)]
        public String Description { get; set; }

        [Column("Is_Exam_Request", TypeName = "bit")]
        public Boolean IsExamRequest { get; set; }

        [Column("Is_Registration_Request", TypeName = "bit")]
        public Boolean IsRegistrationRequest { get; set; }

        [Column("Is_Change_Request", TypeName = "bit")]
        public Boolean IsChangeRequest { get; set; }

        [Column("Is_Chargable", TypeName = "bit")]
        public Boolean IsChargable { get; set; }

        [Column("Fee_Type_ID", TypeName = "int")]
        public Int32? FeeTypeID { get; set; }
    }
}
