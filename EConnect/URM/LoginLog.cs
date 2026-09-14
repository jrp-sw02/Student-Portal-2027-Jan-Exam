using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EConnect.HRMS;
namespace EConnect.URM
{
    public class LoginLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("ORG_ID", TypeName = "int")]
        [Required(ErrorMessage = "Organization ID is required")]
        public int OrganizationID { get; set; }

        [Column("LOGIN_ID")]
        [Required(ErrorMessage = "Login ID is required")]
        [MaxLength(50)]
        [Display(Name = "Login ID")]
        public String LoginID { get; set; }

        [Column("LOGIN_TIME")]
        [Required(ErrorMessage = "Login Time is required")]
        public DateTime LoginTime { get; set; }

        [Column("SESSION_ID")]
        [Required(ErrorMessage = "Session ID is required")]
        [MaxLength(50)]
        [Display(Name = "Session ID")]
        public String SessionID { get; set; }

        [Column("BROWSER")]
        [Required(ErrorMessage = "Browser Name is required")]
        [MaxLength(50)]
        [Display(Name = "Browser Name")]
        public String BrowserName { get; set; }

        [Column("HOST_NAME")]
        [Required(ErrorMessage = "Host Name is required")]
        [MaxLength(100)]
        [Display(Name = "Host Name")]
        public String HostName { get; set; }

        [Column("CLIENT_IP")]
        [Required(ErrorMessage = "Client IP is required")]
        [MaxLength(50)]
        [Display(Name = "Client IP")]
        public String ClientIP { get; set; }

        [Column("SOURCE_IP")]
        [Required(ErrorMessage = "Source IP is required")]
        [MaxLength(50)]
        [Display(Name = "Source IP")]
        public String SourceIP { get; set; }

        [Column("SERVER_VARIABLES")]
        [Required(ErrorMessage = "Server Variables are required")]
        [MaxLength(4000)]
        [Display(Name = "Server Variables")]
        public String ServerVariables { get; set; }

        [Column("LOGOUT_TIME")]
        public DateTime? LogoutTime { get; set; }

        [Column("LOGIN_RESPONSE", TypeName = "int")]
        public int LoginResponseValue { get; set; }

        [Column("IS_ADMIN_LOGIN")]
        public Boolean IsAdminLogin { get; set; }

        [Column("USER_TYPE")]
        public int UserTypeId { get; set; }

        [NotMapped]
        public AuthenticationResponse LoginResponse
        {
            get
            {
                return (AuthenticationResponse)this.LoginResponseValue;
            }
            set
            {
                this.LoginResponseValue = (int)value;
            }
        }

        public virtual Organization Organization { get; set; }
    }
}
