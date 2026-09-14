using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class VirtualAcademyDemandNote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Date")]
        [Required]
        public DateTime ApplicationDate { get; set; }

        [Column("Fee_Type_ID", TypeName = "int")]
        public Int32 FeeTypeID { get; set; }

        [Column("Payment_Mode_ID", TypeName = "int")]
        public Int32 PaymentModeID { get; set; }

        [Column("Demand_Note_Type_ID", TypeName = "int")]
        public Int32 DemandNoteTypeID { get; set; }

        [Column("Amount", TypeName ="decimal")]
        public Decimal Amount { get; set; }

        [Column("Application_Type_ID", TypeName = "int")]
        public Int32 ApplicationTypeID { get; set; }

        [Column("Online_Transaction_ID", TypeName = "int")]
        public Int32? OnlineTransactionID { get; set; }

        //[Column("CSC_Transaction_ID", TypeName = "int")]
        //public Int32? CSCTransaction_ID { get; set; }

        //[Column("DD_Transaction_ID", TypeName = "int")]
        //public Int32? DDTransactionID { get; set; }

        //[Column("NEFT_Transaction_ID", TypeName = "int")]
        //public Int32? NEFTTransactionID { get; set; }

        [Column("Created_By", TypeName = "int")]
        public Int32? CreatedBy { get; set; }

        [Column("Status_ID", TypeName = "int")]
        public Int32 PaymentStatusID { get; set; }

        [Column ("Course_Category_ID", TypeName = "int")]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        public Int32 CourseID { get; set; }

        [Column("ServiceID")]
        [MaxLength(20)]
        public String ServiceID { get; set; }

        [NotMapped]
        public enmPaymentStatus enmPaymentStatus
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

        [ForeignKey("PaymentStatusID")]
        public virtual PaymentStatus PaymentStatus { get; set; }

        [NotMapped]
        public enmPaymentMode enmPaymentMode
        {
            get
            {
                return (enmPaymentMode)this.PaymentModeID;
            }
            set
            {
                this.PaymentModeID = (int)value;
            }
        }
        [NotMapped]
        public enmApplicationTypeVirtualAcademy enmApplicationTypeVirtualAcademy
        {
            get
            {
                return (enmApplicationTypeVirtualAcademy)this.ApplicationTypeID;
            }
            set
            {
                this.ApplicationTypeID = (int)value;
            }
        }

        [ForeignKey("PaymentModeID")]
        public virtual PaymentMode PaymentMode { get; set; }

        [ForeignKey("FeeTypeID")]
        public virtual FeeType FeeType { get; set; }

        [NotMapped]
        public enmDemandNoteType enmDemandNoteType
        {
            get
            {
                return (enmDemandNoteType)this.DemandNoteTypeID;
            }
            set
            {
                this.DemandNoteTypeID = (int)value;
            }
        }

        [ForeignKey("OnlineTransactionID")]
        public virtual OnlineTransaction OnlineTransaction { get; set; }

        //[ForeignKey("DDTransactionID")]
        //public virtual DemandDraftTransaction DemandDraftTransaction { get; set; }

        //[ForeignKey("CSCTransaction_ID")]
        //public virtual CSCTransaction CSCTransaction { get; set; }

        //[ForeignKey("NEFTTransactionID")]
        //public virtual NEFTTransaction NEFTTransaction { get; set; }
    }
}
