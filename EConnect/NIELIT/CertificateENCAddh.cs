using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{


    public class CertificateENCAddh
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("encAddh")]
        [MaxLength]
        public String encAddh { get; set; }

        [Column("CertificateRegID", TypeName = "bigint")]
        public Int64? CertificateRegID { get; set; }


        [Column("enterBy", TypeName = "bigint")]
        public Int64? enterBy { get; set; }

        [Column("enterDate")]
        public DateTime? enterDate { get; set; }

    }

}
