using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class SubjectMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int32 ID { get; set; }
        [Column("SubjectName")]
        [Required(ErrorMessage = "Subject Name is required")]
        [MaxLength(300)]
        [Display(Name = "Subject Name")]
        public String Name { get; set; }
        [Column("enterDate")]
        public DateTime CreatedOn { get; set; }
        [Column("enterby", TypeName = "bigint")]
        public Int64 CreatedByID { get; set; }
    }
}
