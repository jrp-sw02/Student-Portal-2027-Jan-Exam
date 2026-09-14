using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
  public class CSCCharges
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("ApplicationTypeID", TypeName = "int")]
        [Required]
        public Int32 ApplicationTypeID { get; set; }

        [Column("Activity_ID", TypeName = "int")]
        [Required]
        public Int32 ActivityID { get; set; }

        [Column("Amount", TypeName = "int")]
        public Int32 Amount { get; set; }

        [Column("Effective_Date")]
        public DateTime EffectiveDate { get; set; }

        [NotMapped]
        public enmCSCActivity enmCSCActivity
        {
            get
            {
                return (enmCSCActivity)this.ActivityID;
            }
            set
            {
                this.ActivityID = (int)value;
            }
        }

        [ForeignKey("ApplicationTypeID")]
        public virtual ApplicationType ApplicationType { get; set; }

    }
}
