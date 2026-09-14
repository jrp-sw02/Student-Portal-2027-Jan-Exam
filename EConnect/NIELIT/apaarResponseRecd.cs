using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class apaarResponseRecd
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("apaarReqID ", TypeName = "bigint")]
        public long apaarReqID { get; set; }

        [Column("abc_account_id")]
        [Required(ErrorMessage = "Apaar ID is required")]
        public string abc_account_id { get; set; }

        [Column("cname")]
        public string cname { get; set; }

        [Column("genderID", TypeName = "int")]
       
        public Int32? genderID { get; set; }

        [Column("dob", TypeName = "datetime")]
       
        public DateTime? dob { get; set; }

        [Column("status", TypeName = "bit")]
        [Required(ErrorMessage = "Status is required")]
        public bool  status { get; set; }

        [Column("statuscode")]
        public string statuscode { get; set; }


        [Column("status_code")]
        
        public string status_code { get; set; }

        [Column("messageCode")]
        public string messageCode { get; set; }


        [Column("nameMatch ", TypeName = "bit")]
        [Required(ErrorMessage = "Status is required")]
        public bool nameMatch { get; set; }

        [Column("birthYearMatch ", TypeName = "bit")]
        [Required(ErrorMessage = "Status is required")]
        public bool birthYearMatch { get; set; }

        [Column("genderMatch ", TypeName = "bit")]
        [Required(ErrorMessage = "Status is required")]
        public bool genderMatch { get; set; }


        [Column("message")]

        public string message { get; set; }

        [Column("responseContent")]
        public string responseContent { get; set; }

        [Column("enterBy", TypeName = "int")]
        [Required(ErrorMessage = "Enter By is required")]
        public Int32 enterByID { get; set; }


        [Column("enterDate")]
        public DateTime enterDate { get; set; }

    }
}
