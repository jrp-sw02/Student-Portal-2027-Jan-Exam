using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class DownloadableHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Downloadable_ID", TypeName = "int")]
        [Required]
        public Int32 DownloadableID { get; set; }

        [Column("Link_Name")]
        [Required]
        [MaxLength(30)]
        public String LinkName { get; set; }

        [Column("Effective_From_Date")]
        public DateTime EffectiveFromDate { get; set; }

        [Column("Effective_To_Date")]
        public DateTime EffectiveToDate { get; set; }

        [Column("File_ID", TypeName = "bigint")]
        public Int64? DownloadableFileID { get; set; }

        [ForeignKey("DownloadableFileID")]
        public virtual UploadedFile DownloadableFile { get; set; }

        public virtual Downloadable Downloadable { get; set; }
    }
}
