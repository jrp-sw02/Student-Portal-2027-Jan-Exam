using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class Downloadable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Link_Name")]
        [Required]       
        public String LinkName { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        public Int32? CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        public Int32? CourseID { get; set; }

        [Column("Downloadable_Type_ID", TypeName = "int")]
        [Required]
        public Int32 DownloadableTypeID { get; set; }

        [Column("File_ID", TypeName = "bigint")]
        public Int64? DownloadableFileID { get; set; }

        [Column("Show_On_Web", TypeName = "bit")]
        public Boolean ShowOnWeb { get; set; }

        [Column("Effective_From_Date")]
        public DateTime EffectiveFromDate { get; set; }

        [NotMapped]
        public virtual enmDownloadableType DownloadableType
        {
            get
            {
                return (enmDownloadableType)this.DownloadableTypeID;
            }
            set
            {
                this.DownloadableTypeID = Convert.ToInt32(value);
            }
        }

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [ForeignKey("DownloadableFileID")]
        public virtual UploadedFile DownloadableFile { get; set; }
    }
}
