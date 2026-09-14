using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EConnect.HRMS;
namespace EConnect.URM
{
    public class User
    {
        [Required(ErrorMessage = "Organization ID is required")]
        [Column("ORG_ID", TypeName = "int")]
        public int OrganizationID { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("USER_NO", TypeName = "int")]
        public int UserID { get; set; }

        [Required(ErrorMessage="Login Id is required")]
        [MaxLength(50)]
        [Column("USER_ID")]
        public String LoginID { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(100)]
        [Column("PWD")]
        public String Password { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        [MaxLength(100)]
        [Column("USER_NAME")]
        public String UserName { get; set; }

        [Required(ErrorMessage = "User Type is required")]
        [Column("USER_TYPE", TypeName = "int")]
        public int UserTypeID { get; set; }

        [Column("USER_REF_NO", TypeName = "bigint")]
        public Int64 UserRefNumber { get; set; }

        [MaxLength(100)]
        [Column("EMAIL_ID")]
        public String EmailID { get; set; }

        [Column("MOBILE_NO", TypeName = "bigint")]
        [Display(Name = "Mobile Number")]
        public Int64 MobileNumber { get; set; }

        [MaxLength(200)]
        [Column("SECURITY_QUESTION")]
        public String SecurityQuestion { get; set; }

        [MaxLength(100)]
        [Column("SECURITY_ANSWER")]
        public String SecurityAnswer { get; set; }

        [Column("PWD_EXPIRY_DAYS", TypeName = "int")]
        public int PasswordExpiryDays { get; set; }

        [Column("PWD_CHANGED_ON")]
        public DateTime LastPasswordChangedOn { get; set; }

        [Column("CAN_LOGIN")]
        public Boolean HasLoginAccess { get; set; }

        [Column("LAST_LOGIN_ON")]
        public DateTime? LastLoginDateTime { get; set; }

        [Required(ErrorMessage = "Created By Id is required")]
        [Column("CREATED_BY", TypeName = "int")]
        public int CreatedBy { get; set; }

        [Column("CREATED_ON")]
        public DateTime CreatedOn { get; set; }

        [Column("FAILED_LOGIN_ATTEMPT", TypeName = "int")]
        public int FailedLoginAttempts { get; set; }

        [Required(ErrorMessage = "Default Role ID is required")]
        [Column("Default_Role_ID", TypeName = "int")]
        public int DefaultRoleID { get; set; }

        [ForeignKey("DefaultRoleID")]
        public virtual Role DefaultRole { get; set; }

        public virtual Organization Organization { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual User CreatedByUser { get; set; }

        [NotMapped]
        public virtual UserType enmUserType
        {
            get
            {
                return (UserType)this.UserTypeID;
            }
            set
            {
                this.UserTypeID = (int)value;
            }
        }
        
        [NotMapped]
        public virtual DateTime PasswordExpiryDate
        {
            get
            {
                return this.LastPasswordChangedOn.AddDays(PasswordExpiryDays);
            }
        }
       
        [ForeignKey("UserTypeID")]
        public virtual UsersType UserType { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
