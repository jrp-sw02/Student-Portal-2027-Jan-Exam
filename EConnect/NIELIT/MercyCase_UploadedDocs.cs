using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class MercyCase_UploadedDocs
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public long ID { get; set; }

        [Column("candidate_id", TypeName = "bigint")]
        public long? Candidate_Id { get; set; }

        [Column("Original_File_Name", TypeName = "varchar")]
        [MaxLength(255)]
        public string Original_File_Name { get; set; }

        [Column("Extension", TypeName = "varchar")]
        [Required]
        [MaxLength(5)]
        public string Extension { get; set; }

        [Column("Uploaded_File", TypeName = "varbinary")]
        [Required]
        public byte[] Uploaded_File { get; set; }

        [Column("Uploaded_On", TypeName = "datetime")]
        [Required]
        public DateTime Uploaded_On { get; set; }

        [Column("Doc_Type", TypeName = "varchar")]
        [MaxLength(50)]
        public string Doc_Type { get; set; }

        [Column("Request_Status_Id", TypeName = "int")]
        public int? Request_Status_Id { get; set; }

        [Column("Verified_By", TypeName = "bigint")]
        public long? Verified_By { get; set; }

        [Column("Remarks", TypeName = "varchar")]
        [MaxLength(255)]
        public string Remarks { get; set; }

        [Column("Verified_on", TypeName = "datetime")]
        public DateTime? Verified_on { get; set; }

        // Navigation property
        [ForeignKey("Request_Status_Id")]
        public virtual MercyDocumentStatus MercyDocumentStatus { get; set; }
    }
}
