
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class RegnICardCandidatePhotos
    {
        [Key]
        [Column("Candidate_Id", Order = 0, TypeName = "int")]
        public int CandidateId { get; set; }

        [Key]
        [Column("Registration_No", Order = 1, TypeName = "int")]
        public int RegistrationNo { get; set; }


        [Column("Photo_uploaded_file_Id", TypeName = "bigint")]
        public Int64 PhotoUploadedFileId { get; set; }


        [Column("Sign_uploaded_file_Id", TypeName = "bigint")]
        public Int64 SignUploadedFileId { get; set; }


        [Column("Data_Insert_date")]
        public DateTime DataInsertDate { get; set; }


        [Column("I_Card_Download_Count", TypeName = "int")]
        public Int32 ICardDownloadCount { get; set; }


        [Column("Last_Download_Date")]
        public DateTime LastDownloadDate { get; set; }


        [Column("Data_Update_Date")]
        public DateTime DataUpdateDate { get; set; }

    }
}
