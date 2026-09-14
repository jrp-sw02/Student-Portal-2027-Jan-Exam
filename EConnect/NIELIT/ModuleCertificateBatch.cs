using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class ModuleCertificateBatch
    {       
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        public Int32 CourseID { get; set; }

        [Column("Batch_Size", TypeName = "int")]
        public Int32 Size { get; set; }

        [Required, Column("Created_On")]       
        public DateTime CreatedDate { get; set; }
    }
}
