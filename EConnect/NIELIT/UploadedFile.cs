using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class UploadedFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "File Name is required")]
        [MaxLength(50)]
        [Display(Name = "File Name")]
        public String Name { get; set; }

        [Column("Original_File_Name")]
        [MaxLength(100)]
        [Display(Name = "Course Regional Name")]
        public String OriginalName { get; set; }

        [Column("Extension")]
        [MaxLength(5)]
        public String Extension { get; set; }

        [Column("Uploaded_File",  TypeName="varbinary(Max)")]
        public Byte[] BlobFile { get; set; }

        [Column("Uploaded_On")]
        public DateTime UploadedOn { get; set; }
    }
}
