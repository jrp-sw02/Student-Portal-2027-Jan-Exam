using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class MercyDocumentStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("StatusName")]
        [Required(ErrorMessage = "Status Name is required")]
        [MaxLength(50)]
        [Display(Name = "Status Name")]
        public String StatusName { get; set; }

        [Column("IsActive", TypeName = "bit")]
        public bool IsActive { get; set; }

        // Navigation property
        public virtual ICollection<MercyCase_UploadedDocs> MercyCaseUploadedDocs { get; set; }
    }
}
