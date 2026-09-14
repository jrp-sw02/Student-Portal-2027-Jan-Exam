using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class virtualAcademyCourseImages
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("courseCategoryID")]
        [Required(ErrorMessage = "Course Category is required")]
        [Display(Name = "Course Category")]
        public Int64 courseCategoryID { get; set; }

        [Column("courseID")]
        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public Int64 courseID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "File Name is required")]
        [MaxLength(50)]
        [Display(Name = "File Name")]
        public String Name { get; set; }

        [Column("fileName")]
        [MaxLength(100)]
        [Display(Name = "fileName")]
        public String fileName { get; set; }

        [Column("extension")]
        [MaxLength(5)]
        public String Extension { get; set; }

        [Column("uploadedFile",  TypeName="varbinary(Max)")]
        public Byte[] BlobFile { get; set; }

        [Column("uploadedOn")]
        public DateTime UploadedOn { get; set; }

        [Column("uploadedBy")]
        public Int64 UploadedBy { get; set; }
    }
}
