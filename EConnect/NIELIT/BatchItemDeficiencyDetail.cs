using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class BatchItemDeficiencyDetail
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Batch_Item_ID", TypeName = "bigint")]
        [Required]
        public Int64 BatchItemID { get; set; }

        [Column("Deficiency_ID", TypeName = "int")]
        [Required]
        public Int32 DeficiencyID { get; set; }

        [Column("Deficiency_Desc")]
        [MaxLength(500)]
        [Display(Name = "Deficiency Description")]
        public String DeficiencyDesc { get; set; }

        [Column("Created_By", TypeName = "int")]
        [Required]
        public Int32 CreatedByID { get; set; }

        [Column("Created_On")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Column("Is_Full_Filled", TypeName = "bit")]
        [Display(Name = "Is Full Filled")]
        public Boolean IsFullFilled { get; set; }

        [Column("Updated_By", TypeName = "int")]
        public Int32? UpdatedByID { get; set; }

        [Column("Updated_On")]
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey("DeficiencyID")]
        public virtual DeficiencyCode DeficiencyCode { get; set; }
      
        [ForeignKey("BatchItemID")]
        public virtual BatchItem  BatchItem { get; set; }

     

    }
}
