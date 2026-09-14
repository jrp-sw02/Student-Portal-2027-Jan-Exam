using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class NSQFCertPhasePrintDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("rollno", TypeName = "int")]
        public Int32? rollno { get; set; }

        [Column("name")]      
        [MaxLength(60)]
        [Display(Name = "Candidate Name")]
        public String Name { get; set; }

        [Column("f_name")]
        [MaxLength(60)]
        [Display(Name = "Father Name")]
        public String f_name { get; set; }

        [Column("m_name")]
        [MaxLength(60)]
        [Display(Name = "Mother Name")]
        public String m_name { get; set; }

        [Column("G_NAME")]
        [MaxLength(60)]
        public String G_NAME { get; set; }

        [Column("affidavit_srno", TypeName = "int")]
        public Int32? affidavit_srno { get; set; }

        [Column("affidavit_date")]      
        [MaxLength(60)]
        [Display(Name = "affidavit date")]
        public String affidavit_date { get; set; }

      
        //[Column("affidavit_date")]
        //[Display(Name = "Affidavit Date")]
        //public DateTime? affidavit_date { get; set; }

        [Column("affidavit_enter_date")]      
        [MaxLength(60)]
        [Display(Name = "affidavit enter date")]
        public String affidavit_enter_date { get; set; }

        //[Column("affidavit_enter_date", TypeName = "int")]
        //public Int32? affidavit_enter_date { get; set; }

        [Column("affidavit_enter_by", TypeName = "int")]
        public Int32? affidavit_enter_by { get; set; }

        [Column("dob")]
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        [Column("inst_name")]
        [MaxLength(177)]
        public String inst_name { get; set; }

        [Column("tp_name")]
        [MaxLength(150)]
        public String tp_name { get; set; }

        [Column("grade_code")]
        [MaxLength(10)]
        public String grade_code { get; set; }


        [Column("exam_month", TypeName = "int")]
        public Int32? exam_month { get; set; }

        [Column("exam_year", TypeName = "int")]
        public Int32? exam_year { get; set; }

        [Column("exam_type")]
        [MaxLength(4)]
        public String exam_type { get; set; }

        [Column("course_code")]
        [MaxLength(4)]
        public String course_code { get; set; }

        [Column("data_file_name")]
        [MaxLength(7)]
        public String data_file_name { get; set; }

        [Column("pan_no", TypeName = "int")]
        public Int32? pan_no { get; set; }

        [Column("Aadhaar", TypeName = "int")]
        public Int32? Aadhaar { get; set; }

        [Column("Aadhaar_Enrl", TypeName = "int")]
        public Int32? Aadhaar_Enrl { get; set; }

        [Column("AadhaarVerified", TypeName = "int")]
        public Int32? AadhaarVerified { get; set; }

        [Column("AadhaarVerifiedManual", TypeName = "int")]
        public Int32? AadhaarVerifiedManual { get; set; }

        [Column("AadhaarVerifiedBy", TypeName = "int")]
        public Int32? AadhaarVerifiedBy { get; set; }

        [Column("grade_legends_desc")]
        [MaxLength(150)]
        public String grade_legends_desc { get; set; }


        [Column("Remarks", TypeName = "int")]
        public Int32? Remarks { get; set; }

        [Column("regional_centre")]
        [MaxLength(57)]
        public String regional_centre { get; set; }

        [Column("bacth", TypeName = "int")]
        public Int32? bacth { get; set; }

        [Column("phase", TypeName = "int")]
        public Int32? phase { get; set; }


        [Column("application_no", TypeName = "bigint")]
        public Int64? application_no { get; set; }

        [Column("address1")]       
        [MaxLength(100)]
        public String address1 { get; set; }

        [Column("address2")]
        [MaxLength(100)]
        public String address2 { get; set; }

        [Column("address3")]
        [MaxLength(100)]
        public String address3 { get; set; }

        [Column("address_city")]
        [MaxLength(50)]
        public String address_city { get; set; }

        [Column("address_state")]
        [MaxLength(100)]
        public String address_state { get; set; }

        [Column("address_pin", TypeName = "int")]
        public Int32? address_pin { get; set; }

        //[Column("registration_no", TypeName = "int")]
        //public Int32? registration_no { get; set; }

        [Column("registration_no", TypeName = "bigint")]
        public Int64? registration_no { get; set; }

        [Column("percentage", TypeName = "decimal")]
        public Decimal? percentage { get; set; }

        [Column("phase_date")]
        public DateTime? phase_date { get; set; }

        [Column("course_start_on", TypeName = "int")]
        public Int32? course_start_on { get; set; }

        [Column("course_end_on")]
        public DateTime? course_end_on { get; set; }

        [Column("Last_Th_Exam_passed_year", TypeName = "int")]
        public Int32? Last_Th_Exam_passed_year { get; set; }

        [Column("Last_Th_Exam_passed_Month", TypeName = "int")]
        public Int32? Last_Th_Exam_passed_Month { get; set; }

        [Column("Last_Th_Exam_NSQF_ROLL_NO")]
        [MaxLength(16)]
        public String Last_Th_Exam_NSQF_ROLL_NO { get; set; }

        [Column("data_downloaded_sequence", TypeName = "int")]
        public Int32? data_downloaded_sequence { get; set; }

        [Column("data_downloaded_date")]
        public DateTime? data_downloaded_date { get; set; }

        [Column("Credits", TypeName = "int")]
        public Int32? Credits { get; set; }

    }
}
