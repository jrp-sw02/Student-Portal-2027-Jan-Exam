using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class ApaarRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public long ID { get; set; }

        [Column("apaarId")]
        public string apaarId { get; set; }

        [Column("cname")]
        public string cname { get; set; }

        [Column("genderID", TypeName = "int")]
        public int? genderID { get; set; }

        [Column("dob", TypeName = "date")]
        public DateTime? dob { get; set; }

        [Column("transactionID")]
        public string transactionID { get; set; }

        [Column("providerName")]
        public string providerName { get; set; }

        [Column("authModeID", TypeName = "bigint")]
        public Int64? authModeID { get; set; }

        [Column("authModeIDNo")]
        public string authModeIDNo { get; set; }

        [Column("consentRelation")]
        public string consentRelation { get; set; }

        [Column("consentDate", TypeName = "date")]
        public DateTime? consentDate { get; set; }

        [Column("consentTime", TypeName = "time")]
        public TimeSpan? consentTime { get; set; }

        [Column("consentPlace")]
        public string consentPlace { get; set; }

        [Column("undertakingTextChecked")]
        public string undertakingTextChecked { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        public Int64? enterBy { get; set; }

        [Column("enterDate", TypeName = "date")]
        public DateTime? enterDate { get; set; }
    }
}