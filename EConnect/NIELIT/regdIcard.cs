using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class regdIcard
    {
         [Column("Candidate_Id")]
         [Display(Name = "Candidate ID")]
        public int Candidate_Id { get; set; }
        

       [ Column("Registration_No")]
        [Display(Name = "Registration No")]
        public int Registration_No { get; set; }
       
	  [ Column("Photo_uploaded_file_Id")]
        [Display(Name = "Photo No")]
        public Int64 Photo_uploaded_file_Id { get; set; }
	



    }
}
