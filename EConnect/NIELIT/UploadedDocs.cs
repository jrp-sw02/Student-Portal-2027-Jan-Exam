using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class UploadedDocs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id", TypeName = "bigint")]
        public Int64 ID { get; set; }


      
        [Column("Student_Id", TypeName = "bigint")]
        public Int64 studentID { get; set; }


        

        [Column("DocumentTypeID", TypeName = "bigint")]
        public Int64 documentTypeID { get; set; }


        [Column("Name")]
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50)]
        [Display(Name = "Name")]
        public String Name { get; set; }



        [Column("Original_File_Name")]
        [Required(ErrorMessage = "Original_File_Name is required")]
        [MaxLength(1000)]
        [Display(Name = "Name")]
        public String originalFileName { get; set; }

        [Column("Extension")]
        [Required(ErrorMessage = "Extension is required")]
        [MaxLength(5)]
        [Display(Name = "Extension")]
        public String extension { get; set; }


        [Column("Uploaded_File",  TypeName="varbinary(Max)")]
        public Byte[] uploadedFile { get; set; }

        [Column("Uploaded_On")]
        public DateTime uploadedOn { get; set; }

    }
}
