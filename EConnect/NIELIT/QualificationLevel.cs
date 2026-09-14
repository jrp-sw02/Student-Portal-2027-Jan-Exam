using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
   public class QualificationLevel
    {
       [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       [Column("ID", TypeName = "int")]
       public Int32 ID { get; set; }

       [Column("Name")]
       [Required]
       [MaxLength(50)]
       public String Name { get; set; }

       [Column("Code")]
       [MaxLength(10)]
       [Display(Name = "Code")]
       public String Code { get; set; }

       [Column("Display_Order", TypeName = "int")]
       [Display(Name = "Display Order")]
       public Int32? DisplayOrder { get; set; }

       ICollection<EducationalQualification> EducationalQualifications { get; set; }
    }
}
