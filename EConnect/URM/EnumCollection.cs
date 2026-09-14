using System.ComponentModel;
namespace EConnect.URM
{
    public enum AuthenticationResponse
    {
        [Description("Login Successfull.")]
        Successfull = 0,
        [Description("Invalid UserId or Password!")]
        InvalidUserIdOrPassword = 1,
        [Description("Your password has been expired!")]
        PasswordExpired = 2,
        [Description("Login disabled by the administrator!")]
        LoginDisabled = 4,
        [Description("Your failed login attempts limit crossed!")]
        FailedLoginAttemptsLimitCrossed = 5,
        [Description("Login successfull. But Change in password required on first login!")]
        SuccessfullWithChangeInPasswordRequiredOnFirstLogin = 6
    }
    public enum UserType
    {
        [Description("Admin")]
        Admin = 1,
        [Description("External Admin")]
        ExternalAdmin = 2,
        [Description("Candidate")]
        Candidate = 3,
        [Description("Institute")]
        Institute = 4,
        [Description("Regional Center")]
        RegionalCenter = 5,
        [Description("Head Office")]
        HeadOffice = 6,
        [Description("External Institute")]
        ExternalInstitute = 7,
        [Description("External Regional Center")]
        ExternalRegionalCenter = 8,
        [Description("External Head Office")]
        ExternalHeadOffice = 9,
                //Added 30-Aug-2019
        [Description("ProjectNIELITCentre")]
        projectNIELITCentre = 10,
	    [Description("Non Affiliated Institute")]
        NonAffiliatedInstitute = 11,
        [Description("Observer")]
        Observer = 12,
        [Description("Examiner")]
        Examiner = 13
    }
    public enum enmRole
    {
        [Description("Super Admin")]
        SuperAdmin = 1,
        [Description("External")]
        External = 2,
        [Description("Candidate")]
        Candidate = 3,
        [Description("Admin Institute")]
        AdminInstitute = 4,
        [Description("Admin Regional Center")]
        AdminRegionalCenter = 5,
        [Description("Admin Head Office")]
        AdminHeadOffice = 6,
        [Description("Course Wise Head Office")]
        CourseWiseHeadOffice = 12,
        [Description("Course Wise Admin")]
        CourseWiseAdmin = 13
    }
    public enum enmRight
    {
        [Description("View")]
        View = 1,
        [Description("Edit")]
        Edit = 2,
        [Description("Delete")]
        Delete = 3,
        [Description("New")]
        New = 4,
        [Description("Full Control")]
        FullControl = 5 
    }
    public enum enmMenuObjectType
    {
        [Description("Project")]
        Project = 1,
        [Description("Module")]
        Module = 2,
        [Description("Menu")]
        Menu = 3,
        [Description("Form")]
        Form = 4
    }
}
