using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect
{
    public class AddressUpdateTemp
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("SL", TypeName = "int")]
        public Int32 SL { get; set; }

        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Address_Type_ID", TypeName = "int")]
        public Int32 Address_Type_ID { get; set; }

        [Column("Address1")]
        [Required]
        [MaxLength(100)]
        public String Address1 { get; set; }

        [Column("Address2")]
        [Required]
        [MaxLength(100)]
        public String Address2 { get; set; }

        [Column("Address3")]
        [Required]
        [MaxLength(100)]
        public String Address3 { get; set; }

        [Column("Country_ID", TypeName = "bigint")]
        [Required]
        public Int64 Country_ID { get; set; }

        [Column("State_ID", TypeName = "bigint")]
        public Int64 State_ID { get; set; }

        [Column("District_ID", TypeName = "bigint")]
        public Int64 District_ID { get; set; }

        [Column("Tehsil_ID", TypeName = "bigint")]
        public Int64 Tehsil_ID { get; set; }

        [Column("City_Name")]
        [MaxLength(50)]
        public String City_Name { get; set; }

        [Column("Pin_Code", TypeName = "int")]
        [Required]
        public Int32 Pin_Code { get; set; }

        [Column("Candidate_ID", TypeName = "bigint")]
        public Int64 Candidate_ID { get; set; }

        [Column("Effective_From_Date")]
        public DateTime Effective_From_Date { get; set; }

        [Column("Created_On")]
        [Required()]
        public DateTime Created_On { get; set; }

        [Column("Created_By", TypeName = "int")]
        [Required()]
        public Int32 Created_By { get; set; }

        [Column("Is_Verified", TypeName = "bit")]
        [Required()]
        public Boolean IsVerified { get; set; }

        [Column("Verified_By", TypeName = "int")]
        public Int32 Verified_By { get; set; }

        [Column("State_Code")]
        [MaxLength(20)]
        public String State_Code { get; set; }

        [Column("Update_DateTime")]
        public DateTime Update_DateTime { get; set; }

        [Column("Client_IPAddress")]
        [MaxLength(50)]
        public String Client_IPAddress { get; set; }

        [Column("Client_UserId")]
        [MaxLength(50)]
        public String Client_UserId { get; set; }

        [Column("Client_HostName")]
        [MaxLength(50)]
        public String Client_HostName { get; set; }
    }
}
