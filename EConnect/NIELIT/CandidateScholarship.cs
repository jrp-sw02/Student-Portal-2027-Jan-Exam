using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CandidateScholarship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Candidate_ID", TypeName = "bigint")]
        public Int64 CandidateID { get; set; }

        [Column("Aadhar_Number", TypeName = "bigint")]
        public Int64 ? AadharNumber { get; set; }

        [Column("Account_Number")]
        [MaxLength(100)]
        public String AccountNumber { get; set; }

        [Column("Account_Holder_Name")]
        [MaxLength(100)]
        public String AccountHolderName { get; set; }

        [Column("Type_Of_Account")]
        [MaxLength(100)]
        public String TypeOfAccount { get; set; }

        [Column("Caste_Category", TypeName = "int")]
        public Int32 CasteCategory { get; set; }

        [Column("Annual_Income", TypeName = "int")]
        public Int32 AnnualIncome { get; set; }

        [Column("IFSC_Code")]
        [MaxLength(100)]
        public String IFSCCode { get; set; }

        [Column("Bank_Name")]
        [MaxLength(100)]
        public String BankName { get; set; }

        [Column("Bank_Address")]
        [MaxLength(200)]
        public String BankAddress { get; set; }

        [Column("Date")]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Column("Is_Handicaped", TypeName = "bit")]
        [Display(Name = "Is Handicapped")]
        public Boolean IsHandicaped { get; set; }

        [Column("Aaadhar_File_ID", TypeName = "bigint")]
        public Int64 ? AaadharFileID { get; set; }

        [Column("Income_Proof_File_ID", TypeName = "bigint")]
        public Int64 ? IncomeProofFileID { get; set; }

        [Column("Caste_Certificate_File_ID", TypeName = "bigint")]
        public Int64 ? CasteCertificateFileID { get; set; }

        [Column("Physical_Handicap_File_ID", TypeName = "bigint")]
        public Int64? PhysicalHandicapFileID { get; set; }

        [Column("Bank_Account_File_ID", TypeName = "bigint")]
        public Int64? BankAccountFileID { get; set; }

        [ForeignKey("CandidateID")]
        public virtual Candidate Candidate { get; set; }

        [ForeignKey("AaadharFileID")]
        public virtual UploadedFile Aaadhar { get; set; }

        [ForeignKey("IncomeProofFileID")]
        public virtual UploadedFile IncomeProof { get; set; }

        [ForeignKey("CasteCertificateFileID")]
        public virtual UploadedFile CasteCertificate { get; set; }

        [ForeignKey("PhysicalHandicapFileID")]
        public virtual UploadedFile PhysicalHandicap { get; set; }

        [ForeignKey("BankAccountFileID")]
        public virtual UploadedFile BankAccount { get; set; }

        [ForeignKey("AnnualIncome")]
        public virtual IncomeCategory IncomeCategory { get; set; }

    }
}
