using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class aadhaarResponse
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("applicationNo")]
        [Required(ErrorMessage = "Application No is required")]
        public string applicationNo { get; set; }

        [Column("epramaanid")]
        [Required(ErrorMessage = "epramaan ID is required")]
        public string epramaanid { get; set; }

        [Column("courseCategoryID", TypeName = "int")]
        [Required(ErrorMessage = "Course Category is required")]
        public Int32 courseCategoryID { get; set; }

        [Column("courseID", TypeName = "bigint")]
        [Required(ErrorMessage = "Course is required")]
        public Int64 courseID { get; set; }

        [Column("status", TypeName = "bit")]
        [Required(ErrorMessage = "Status is required")]
        public bool  status { get; set; }

        [Column("transactionID")]
        [Required(ErrorMessage = "Transaction ID is required")]
        public string transactionID { get; set; }

        [Column("errorCode")]
       
        public string errorCode { get; set; }

        [Column("Remarks")]
        public string Remarks { get; set; }

       [Column("bioResponseCode")]
        public string bioResponseCode { get; set; }

	 [Column("enterBy", TypeName = "int")]
        [Required(ErrorMessage = "Enter By is required")]
        public Int32 enterByID { get; set; }


        [Column("enterDate")]
        public DateTime enterDate { get; set; }

    }
}
