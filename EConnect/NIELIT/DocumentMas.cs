using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class DocumentMas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }


        [Column("DocumentName")]
        [Required(ErrorMessage = "Document Name is required")]
        [Display(Name = "Documents Name")]
        public String documentName { get; set; }

        [Column("SizeInKb")]
        public int SizeInKb { get; set; }

        [Column("Type")]
        [Required(ErrorMessage = "Document Type is required")]
        [Display(Name = "Documents Type")]
        public String documentType { get; set; }


        [Column("docDownloadUrl")]
        public String docDownloadUrl { get; set; }

        [Column("createdBy", TypeName = "int")]
        [Display(Name = "User Name")]
        public Int32 createdBy { get; set; }

        [Column("createdDate")]
        public DateTime createdDate { get; set; }
    }
}
