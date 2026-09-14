using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class RegionalCenter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("Name")]
        public String Name { get; set; }

        [MaxLength(10)]
        [Column("Code")]
        public String Code { get; set; }

        [Column("Registered_Mobile_Number")]
        public Int64? RegisteredMobileNumber { get; set; }

        [MaxLength(100)]
        [Column("Registered_EmailID")]
        public String RegisteredEmailAddress { get; set; }

        [MaxLength(100)]
        [Column("Contact_Numbers")]
        public String ContactNumbers { get; set; }

        [MaxLength(200)]
        [Column("Email_Addresses")]
        public String EmailAddresses { get; set; }

        [MaxLength(100)]
        [Column("Website")]
        public String Website { get; set; }

        [MaxLength(200)]
        [Column("Address")]
        public String Address { get; set; }

        [MaxLength(200)]
        [Column("Bank_Account_Name")]
        public String BankAccountName { get; set; }

        [MaxLength(100)]
        [Column("Bank_Branch")]
        public String BankBranchName { get; set; }
		
		 //Added 18 May 2021
        public bool isActive { get; set; }
    }
}
