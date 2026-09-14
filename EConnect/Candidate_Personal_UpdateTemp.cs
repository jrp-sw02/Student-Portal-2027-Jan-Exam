using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect
{
  public  class Candidate_Personal_UpdateTemp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("SL", TypeName = "bigint")]        
        public Int64 SL { get; set; }

        [Column("Details_Type_ID", TypeName = "bigint")]        
        public Int64 Details_Type_ID { get; set; }

        [Column("Candidate_ID", TypeName = "bigint")]       
        public Int64 Candidate_ID { get; set; }

        [Column("Name")]        
        [MaxLength(60)]
        public String Name { get; set; }

        [Column("Father_Name")]       
        [MaxLength(60)]
        public String Father_Name { get; set; }

        [Column("Mother_Name")]        
        [MaxLength(60)]
        public String Mother_Name { get; set; }

        [Column("Gender")]        
        [MaxLength(6)]
        public String Gender { get; set; }

        [Column("Marital_Status_ID", TypeName = "int")]       
        public Int32? Marital_Status_ID { get; set; }


        [Column("Dob")]
        [Display(Name = "Date of Birth")]
        public DateTime Dob { get; set; }

        [Column("Cast_Category_ID", TypeName = "int")]      
        [Display(Name = "Cast Category")]
        public Int32 Cast_Category_ID { get; set; }

        [Column("Is_Handicaped", TypeName = "bit")]        
        public Boolean Is_Handicaped { get; set; }

        [Column("Is_Ex_Servicemane", TypeName = "bit")]       
        public Boolean Is_Ex_Servicemane { get; set; }
              
        [Column("Is_Verified", TypeName = "bit")]        
        [Display(Name = "Is Verified")]
        public Boolean Is_Verified { get; set; }

        [Column("Verified_By", TypeName = "bigint")]        
        public Int32 Verified_By { get; set; }

        [Column("Mobile", TypeName = "bigint")]
        public Int64 Mobile { get; set; }

        //[Column("Std", TypeName = "int")]
        //public Int32 StdNumber { get; set; }

        [Column("Phone", TypeName = "int")]
        public Int32 Phone { get; set; }

        [Column("Email")]
        [MaxLength(100)]
        public String Email { get; set; }

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
