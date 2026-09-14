using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class PuraskarApplicationForm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        //[Column("OnlineRefNo", TypeName = "bigint")]
        //public string OnlineRefNo { get; set; }

        [Column("OnlineRefNo")]
        public string OnlineRefNo { get; set; }

        [Column("CandidateID", TypeName = "bigint")]
        public Int64 CandidateID { get; set; }

        [Column("RegnNo", TypeName = "bigint")]
        public Int64 RegnNo { get; set; }


        [Column("InstituteID", TypeName = "bigint")]
        public Int64 InstituteID { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("AadharNumber", TypeName = "bigint")]
        public Int64 AadharNumber { get; set; }

        [Column("AccountNo")]
        public string AccountNo { get; set; }

        [Column("AccountHolderName")]
        public string AccountHolderName { get; set; }

        [Column("AccountType")]
        public string AccountType { get; set; }

        [Column("BankName")]
        public string BankName { get; set; }

        [Column("BankAddress")]
        public string BankAddress { get; set; }

        [Column("BankIFSC")]
        public string BankIFSC { get; set; }

        [Column("AnnualIncome", TypeName = "bigint")]
        public Int64 AnnualIncome { get; set; }

        [Column("ExamID", TypeName = "bigint")]
        public Int64 ExamID { get; set; }

        [Column("PapersAppeared")]
        public int? PapersAppeared { get; set; }

        [Column("PapersPassed")]
        public int? PapersPassed { get; set; }

        [Column("CasteCertNo")]
        public string CasteCertNo { get; set; }

        [Column("CasteCertDate")]
        public DateTime? CasteCertDate { get; set; }

        [Column("CasteCertUpload")]
        public byte[] CasteCertUpload { get; set; }

        [Column("CasteCertFile")]
        public string CasteCertFile { get; set; }

        [Column("CasteCertUploadedOn")]
        public DateTime? CasteCertUploadedOn { get; set; }

        [Column("IncomeCertNo")]
        public string IncomeCertNo { get; set; }

        [Column("IncomeCertDate")]
        public DateTime? IncomeCertDate { get; set; }

        [Column("IncomeCertUpload")]
        public byte[] IncomeCertUpload { get; set; }

        [Column("IncomeCertFile")]
        public string IncomeCertFile { get; set; }

        [Column("IncomeCertUploadedOn")]
        public DateTime? IncomeCertUploadedOn { get; set; }

        [Column("PHCertNo")]
        public string PHCertNo { get; set; }

        [Column("PHCertDate")]
        public DateTime? PHCertDate { get; set; }

        [Column("PHCertUpload")]
        public byte[] PHCertUpload { get; set; }

        [Column("PHCertFile")]
        public string PHCertFile { get; set; }

        [Column("PHCertUploadedOn")]
        public DateTime? PHCertUploadedOn { get; set; }

        [Column("finalSubmit", TypeName = "bit")]
        public Boolean? finalSubmit { get; set; }

        [Column("Final_Submission_Date")]
        public DateTime? Final_Submission_Date { get; set; }

        [Column("verifiedByInstt", TypeName = "bit")]
        public Boolean? verifiedByInstt { get; set; }

        [Column("verifiedByInsttUser", TypeName = "bigint")]
        public Int64? verifiedByInsttUser { get; set; }

        [Column("VerifiedInsttOn")]
        public DateTime? VerifiedInsttOn { get; set; }

        [Column("InsttRejectionReason")]
        public string InsttRejectionReason{get; set;}

        [Column("verifiedByExam", TypeName = "bit")]
        public Boolean? verifiedByExam { get; set; }

        [Column("verifiedByExamUser", TypeName = "bigint")]
        public Int64? verifiedByExamUser { get; set; }

        [Column("VerifiedExamOn")]
        public DateTime? VerifiedExamOn { get; set; }

         [Column("examRejectionReason")]
        public string examRejectionReason{get; set;}

        [Column("verifiedByFinance", TypeName = "bit")]
        public Boolean? verifiedByFinance { get; set; }

        [Column("verifiedByFinanceUser", TypeName = "bigint")]
        public Int64? verifiedByFinanceUser { get; set; }

        [Column("VerifiedByFinanceOn", TypeName = "date")]
        public DateTime? VerifiedByFinanceOn { get; set; }

         [Column("financeRejectionReason")]
        public string financeRejectionReason{get; set;}


         [Column("isWithHeldFinance", TypeName = "bit")]
         public Boolean? isWithHeldFinance { get; set; }

         [Column("withHeldDate", TypeName = "date")]
         public DateTime? withHeldDate { get; set; }

         [Column("withHeldReason")]
         public string withHeldReason { get; set; }

         [Column("withHeldResolvedDate", TypeName = "date")]
         public DateTime? withHeldResolvedDate { get; set; }
        
        [Column("SentForBankTransferOn", TypeName = "date")]
        public DateTime? SentForBankTransferOn{get; set;}

        [Column("InstallmentsNumber", TypeName = "int")]
        public Int32? InstallmentsNumber { get; set; }

        [Column("AmountReleased", TypeName = "bigint")]
        public Int64? AmountReleased { get; set; }

        [Column("AmountReleaseDate")]
        public DateTime? AmountReleaseDate { get; set; }

        [Column("TransactionId")]
        public string TransactionId { get; set; }

        [Column("TransactionStatus")]
        public string TransactionStatus { get; set; }

         [Column("isSettled", TypeName = "bit")]
        public Boolean? isSettled { get; set; }

        [Column("settledBy", TypeName = "int")]
        public Int32? settledBy { get; set; }

        [Column("settledOn",TypeName = "date")]
        public DateTime? settledOn { get; set; }

        [Column("settledFileName")]
        public string settledFileName { get; set; }

        [Column("Remarks")]
        public string Remarks { get; set; }

        [Column("applicationStatusID", TypeName = "bigint")]
        public Int64? applicationStatusID { get; set; }

        [Column("moduleCountProcessed", TypeName = "bigint")]
        public Int64? moduleCountProcessed { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        public Int64? enterBy { get; set; }

        [Column("enterDate")]
        public DateTime? enterDate { get; set; }
    }
}