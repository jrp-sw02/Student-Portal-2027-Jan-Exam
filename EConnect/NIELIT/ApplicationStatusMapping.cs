using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
  public class ApplicationStatusMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Application_Status_ID", TypeName = "int")]
        [Required]
        public Int32 ApplicationStatusID { get; set; }

        [Column("Application_Type_ID", TypeName = "int")]
        [Required]
        public Int32 ApplicationTypeID { get; set; }

        [Column("Is_HO", TypeName = "bit")]
        [Required]
        public Boolean IsRelatedToHeadOffice { get; set; }

        [Column("Is_RC", TypeName = "bit")]
        [Required]
        public Boolean IsRelatedToRegionalCentre { get; set; }

        [Column("Is_AC", TypeName = "bit")]
        [Required]
        public Boolean IsRelatedToAccreditedCentre { get; set; }

        [Column("Is_Admin", TypeName = "bit")]
        [Required]
        public Boolean IsRelatedToAdmin { get; set; }

        [Column("Is_External", TypeName = "bit")]
        [Required]
        public Boolean IsRelatedToExternal { get; set; }

        [NotMapped]
        public enmApplicationType enmApplicationType
        {
            get
            {
                return (enmApplicationType)this.ApplicationTypeID;
            }
            set
            {
                this.ApplicationTypeID = (int)value;
            }
        }

        [ForeignKey("ApplicationStatusID")]
        public virtual ApplicationStatus ApplicationStatus{get;set;}

        [ForeignKey("ApplicationTypeID")]
        public virtual ApplicationType ApplicationType{get;set;}
    }
}
