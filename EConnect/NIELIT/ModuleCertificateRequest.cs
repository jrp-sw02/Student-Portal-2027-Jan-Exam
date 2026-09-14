using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class ModuleCertificateRequest
    {       
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Registration_No", TypeName = "bigint")]
        public Int64 RegistrationNo { get; set; }

        [Column("Course_ID", TypeName = "int")]       
        public Int32 CourseID { get; set; }

        [Column("Module_ID", TypeName = "int")]
        public Int32 ModuleID { get; set; }

        [Column("Batch_ID", TypeName = "int")]
        public Int32? BatchID { get; set; }

        [Column("Certificate_No", TypeName = "varchar"), MaxLength(25)]
        public String CertificateNumber { get; set; }

        [Column("Demand_Note_ID", TypeName = "bigint")]
        public Int64? DemandNoteID { get; set; }

        [Column("Payment_Status_ID", TypeName = "int")]
        public Int32 PaymentStatusID { get; set; }

        [Column("Is_Downloaded", TypeName = "bit")]
        public Boolean IsDownloaded { get; set; }

        [Column("Is_Duplicate", TypeName = "bit")]
        public Boolean IsDuplicate { get; set; }

        [Column("Is_Blocked", TypeName = "bit")]
        public Boolean IsBlocked { get; set; }

        [Required, Column("Request_Date")]      
        public DateTime RequestDate { get; set; }

        [Column("Type", TypeName = "varchar"), MaxLength(10)]
        public String RequestType { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [ForeignKey("DemandNoteID")]
        public virtual DemandNote DemandNote { get; set; }

        [ForeignKey("ModuleID")]
        public virtual Module Module { get; set; }

        [ForeignKey("BatchID")]
        public virtual ModuleCertificateBatch Batch { get; set; }

        [ForeignKey("PaymentStatusID")]
        public virtual PaymentStatus PaymentStatus { get; set; }

        [NotMapped]
        public enmPaymentStatus enumPaymentStatus
        {
            get
            {
                return (enmPaymentStatus)this.PaymentStatusID;
            }
            set
            {
                this.PaymentStatusID = (int)value;
            }
        }
    }
}
