using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CertificateCourseType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }


        [Column("certCourseType")]
        [Required(ErrorMessage = "Course Type of Certificate is required")]
        [MaxLength(3)]
        [Display(Name = "Course Type of Certificate")]
        public String certCourseType { get; set; }

       

        [Column("Remarks")]
        [Required(ErrorMessage = "Remarks is required")]
        [MaxLength(100)]
        [Display(Name = "Remarks")]
        public String remarks { get; set; }


        [Column("enterDate")]
        public DateTime enterDate { get; set; }



        [Column("enterBy", TypeName = "bigint")]
        public Int64? enterBy { get; set; }


        [Column("certCourseTypeDesc")]
        [Required(ErrorMessage = "Description of Course Type Certificate is required")]
        [MaxLength(100)]
        [Display(Name = "Description of Course Type Certificate")]
        public String certCourseTypeDesc { get; set; }


    }
}
