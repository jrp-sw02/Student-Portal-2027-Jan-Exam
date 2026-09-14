using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using EConnect.DAL;
using System.Security.Cryptography;
using System.Data.Entity.Validation;
namespace EConnect.URM
{
    public class UserManager
    {
        public static void LogLoginDetails(String loginID, int userTypeId, AuthenticationResponse loginResponse, EConnectContext context, Int32 orgID = 1)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (string var in HttpContext.Current.Request.ServerVariables)
                {
                    sb.Append(var + " --> " + HttpContext.Current.Request.ServerVariables[var] + System.Environment.NewLine);
                }
                LoginLog log = new LoginLog();
                log.IsAdminLogin = false;
                log.BrowserName = HttpContext.Current.Request.Browser.Type.ToString();
                log.HostName = System.Net.Dns.GetHostName();
                log.ClientIP = System.Net.Dns.GetHostEntry(log.HostName).AddressList.GetValue(0).ToString();
                log.LoginID = loginID.ToUpper();
                log.IsAdminLogin = false;
                log.LoginResponse = loginResponse;
                log.LoginTime = DateTime.Now;
                log.OrganizationID = orgID;
                log.UserTypeId = userTypeId;
                if (sb.Length > 4000)
                    log.ServerVariables = sb.ToString().Substring(0, 3999).Trim();
                else
                    log.ServerVariables = sb.ToString();
                log.SessionID = HttpContext.Current.Session.SessionID.ToString();
                log.SourceIP = HttpContext.Current.Request.UserHostAddress.ToString();
                context.LoginLogs.Add(log);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Int64 GetCerrentLogID(String loginID, EConnectContext context, Int32 orgID = 1)
        {
            try
            {
                Int64 logQuery = (from g in context.LoginLogs
                                  where g.LoginID.ToUpper() == loginID.ToUpper() && g.SessionID == HttpContext.Current.Session.SessionID
                                  select g.ID).Max();
                return logQuery;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static AuthenticationResponse ValidateUser(String loginID, String loginPassword, int userTypeId, out User outLoginUser, EConnectContext context, Int32 orgID = 1)
        {
            Int32 _intLoginAttempts = 3;
            User loginUser = new User();
            
            try
            {
                _intLoginAttempts = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["LoginAttempts"]);
            }
            catch (Exception) { }	
            AuthenticationResponse loginResponse;
            Int32 _orgID = orgID;
            string pwd = loginPassword.ToUpper();// FormsAuthentication.HashPasswordForStoringInConfigFile(loginPassword, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
            //For the User as Others
            if (userTypeId != 0)
            {
                int[] notinUserType = { 3, 4 };
               var userQuery = from u in context.Users
                                    where u.LoginID.ToUpper() == loginID.Trim().ToUpper() && !notinUserType.Contains(u.UserTypeID) &&
                                    u.OrganizationID == 1
                                    select u;
                //For the User as Student or Institutes
                if (userTypeId == 3 || userTypeId == 4)
                {
                   userQuery = from u in context.Users
                                where u.LoginID.ToUpper() == loginID.Trim().ToUpper() && u.UserTypeID == userTypeId &&
                                u.OrganizationID == 1
                                select u;
                }
                if (userQuery.Count() > 0)
                {
                    loginUser = userQuery.Single();
                    //Check if the user has login access or not
                    if (loginUser.Password == pwd)
                    {
                        if (loginUser.FailedLoginAttempts >= _intLoginAttempts)
                        {
                            loginResponse = AuthenticationResponse.FailedLoginAttemptsLimitCrossed;
                        }
                        else
                        {
                            if (loginUser.HasLoginAccess == true)
                            {
                                //Check if the user logging in first time
                                if (loginUser.LastLoginDateTime.HasValue)
                                {
                                    //Check if the user password has been expired or not
                                    if (loginUser.LastPasswordChangedOn.AddDays(loginUser.PasswordExpiryDays) >= DateTime.Now || loginUser.PasswordExpiryDays == 0)
                                    {
                                        loginResponse = AuthenticationResponse.Successfull;
                                        loginUser.LastLoginDateTime = DateTime.Now;
                                        loginUser.FailedLoginAttempts = (int)0;
                                        context.Entry(loginUser).State = EntityState.Modified;
                                        LogLoginDetails(loginUser.LoginID, loginUser.UserTypeID, loginResponse, context, _orgID);
                                        context.SaveChanges();
                                    }
                                    else
                                    {
                                        loginResponse = AuthenticationResponse.PasswordExpired;
                                    }
                                }
                                else
                                {
                                    loginResponse = AuthenticationResponse.SuccessfullWithChangeInPasswordRequiredOnFirstLogin;
                                }
                            }
                            else
                            {
                                loginResponse = AuthenticationResponse.LoginDisabled;
                            }
                        }
                    }
                    else
                    {
                        loginResponse = AuthenticationResponse.InvalidUserIdOrPassword;
                        if (loginUser.FailedLoginAttempts < _intLoginAttempts)
                        {
                            loginUser.FailedLoginAttempts += 1;
                            context.Entry(loginUser).State = EntityState.Modified;
                            LogLoginDetails(loginUser.LoginID, loginUser.UserTypeID, loginResponse, context, _orgID);
                            context.SaveChanges();
                        }
                        else
                        {
                            loginResponse = AuthenticationResponse.FailedLoginAttemptsLimitCrossed;
                        }
                    }
                }
                else
                {
                    loginResponse = AuthenticationResponse.InvalidUserIdOrPassword;
                }
            }
            else 
            {
                loginResponse = AuthenticationResponse.InvalidUserIdOrPassword;
            }
            outLoginUser = loginUser;
            return loginResponse;
        }
        public static User GetUserByLoginID(String loginID, int userTypeId, EConnectContext context, Int32 orgID = 1)
        {
            try
            {
                int[] notinUserType = { 3, 4 };
                var user = (from u in context.Users
                            where u.OrganizationID == orgID && u.LoginID.ToUpper() == loginID.ToUpper() && !notinUserType.Contains(u.UserTypeID)
                            select u).FirstOrDefault();                
                if (userTypeId == 3 || userTypeId == 4)
                {
                    user = (from u in context.Users
                            where u.OrganizationID == orgID && u.LoginID.ToUpper() == loginID.ToUpper() && u.UserTypeID == userTypeId
                            select u).FirstOrDefault();
                }
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static User GetUserByUserID(Int32 loginID, EConnectContext context)
        {
            try
            {
                var user = context.Users.Find(loginID);
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Boolean HasRight(Int32 roleID, enmRight right, string menuObjectName = "")
        {
            try
            {
                if (string.IsNullOrEmpty(menuObjectName.Trim()))
                    menuObjectName = GetCurrentPageName();
                else
                    menuObjectName = menuObjectName.Trim().ToUpper();
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 moduleID = Convert.ToInt32(HttpContext.Current.Session["ModuleID"]);
                    Int32 menuObjectID = (from m in context.MenuObjects join p in context.MenuObjects on m.ParentMenuObjectID equals p.ID
                                          where m.FormURL.Trim().ToUpper() == menuObjectName && p.ParentMenuObjectID == moduleID
                                          select m.ID).FirstOrDefault();
                    var rights = (from r in context.UserRights
                                  where r.RoleID == roleID && r.MenuObjectID == menuObjectID
                                  select r).FirstOrDefault();
                    if (rights != null)
                    {
                        if (rights.HasFullControl)
                            return true;
                        else if (rights.HasView && right == enmRight.View)
                            return true;
                        else if (rights.HasEdit && right == enmRight.Edit)
                            return true;
                        else if (rights.HasNew && right == enmRight.New)
                            return true;
                        else if (rights.HasDelete && right == enmRight.Delete)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static String GetCurrentPageName()
        {
            try
            {
                //string menuObjectName = HttpContext.Current.Request.Url.LocalPath.ToString().ToUpper().Replace("/NIELIT_WEB", "").Trim();
                //return menuObjectName.Substring(menuObjectName.ToString().IndexOf('/') + 1, menuObjectName.ToString().Length - menuObjectName.ToString().IndexOf('/') - 1);

                string appPath = HttpContext.Current.Request.ApplicationPath;
                string path = HttpContext.Current.Request.Url.AbsolutePath;

                if (path.StartsWith(appPath, StringComparison.OrdinalIgnoreCase))
                {
                    path = path.Substring(appPath.Length);
                }

                return path.TrimStart('/');
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static AuthenticationResponse ValidateUser(String loginID, String loginPassword, String RandomKey, int userTypeId, out User outLoginUser, EConnectContext context, Int32 orgID = 1)
        {

            Int32 _intLoginAttempts = 3;
            String UserLoginId = loginID.Trim().ToUpper();
            User loginUser = new User();

            try
            {
                _intLoginAttempts = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["LoginAttempts"]);
            }
            catch (Exception) { }
            AuthenticationResponse loginResponse;
            Int32 _orgID = orgID;
            try
            {
                //For the User as Others
                if (userTypeId != 0)
                {
                    var userQuery = context.Users.Where(u => u.LoginID.ToUpper() == UserLoginId && u.OrganizationID == 1).Select(u => u);

                    //For the User as Student or Institutes
                    if (userTypeId == 3 || userTypeId == 4)
                    { userQuery = userQuery.Where(s => s.UserTypeID == userTypeId); }

                    if (userQuery.Count() > 0)
                    {
                        loginUser = userQuery.Single();

                        //<<<<<<<<<<<<=======================added on 08/10/2020 for master key password against the registration number of any candidate=========================
                        var masteruser = context.Users.Where(u => u.UserID == 4 && u.UserTypeID ==1 && u.OrganizationID == 1).Select(u => u).Single();
                        string createdMastersaltedPwd = ComputeSha256Hash(RandomKey + masteruser.Password);
                        //===============================================================================================================================>>>>>>>>>>>>>>>>>>>>>>>

                        //string createSaltedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile((RandomKey + loginUser.Password), System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
                        string createSaltedPwd = ComputeSha256Hash(RandomKey + loginUser.Password);
                        //Check if the user has login access or not
                        if (createSaltedPwd.ToUpper() == loginPassword.ToUpper() || createdMastersaltedPwd.ToUpper() == loginPassword.ToUpper() )
                        {
                            if (loginUser.FailedLoginAttempts >= _intLoginAttempts)
                            {
                                loginResponse = AuthenticationResponse.FailedLoginAttemptsLimitCrossed;
                            }
                            else
                            {
                                if (loginUser.HasLoginAccess == true)
                                {
                                    //// Start //try to block candidate login for some time 
                                    //if (userTypeId == 3)
                                    //{
                                    //    loginResponse = AuthenticationResponse.InvalidUserIdOrPassword;
                                    //    // do nothing 
                                    //}
                                    ////end //try to block candidate login for some time 

                                    ////Check if the user logging in first time including below else 
                                    //else 
                                    if (loginUser.LastLoginDateTime.HasValue)
                                    {
                                        //Check if the user password has been expired or not
                                        if (loginUser.LastPasswordChangedOn.AddDays(loginUser.PasswordExpiryDays) >= DateTime.Now || loginUser.PasswordExpiryDays == 0)
                                        {
                                            loginResponse = AuthenticationResponse.Successfull;
                                            loginUser.LastLoginDateTime = DateTime.Now;
                                            loginUser.FailedLoginAttempts = (int)0;
                                            context.Entry(loginUser).State = EntityState.Modified;
                                            LogLoginDetails(loginUser.LoginID, loginUser.UserTypeID, loginResponse, context, _orgID);
                                            context.SaveChanges();
                                        }
                                        else
                                        {
                                            loginResponse = AuthenticationResponse.PasswordExpired;
                                        }
                                    }
                                    else
                                    {
                                        loginResponse = AuthenticationResponse.SuccessfullWithChangeInPasswordRequiredOnFirstLogin;
                                    }
                                }
                                else
                                {
                                    loginResponse = AuthenticationResponse.LoginDisabled;
                                }
                            }
                        }
                        else
                        {
                            loginResponse = AuthenticationResponse.InvalidUserIdOrPassword;
                            if (loginUser.FailedLoginAttempts < _intLoginAttempts)
                            {
                                loginUser.FailedLoginAttempts += 1;
                                context.Entry(loginUser).State = EntityState.Modified;
                                LogLoginDetails(loginUser.LoginID, loginUser.UserTypeID, loginResponse, context, _orgID);
                                context.SaveChanges();
                            }
                            else
                            {
                                loginResponse = AuthenticationResponse.FailedLoginAttemptsLimitCrossed;
                            }
                        }
                    }
                    else
                    {
                        loginResponse = AuthenticationResponse.InvalidUserIdOrPassword;
                    }
                }
                else
                {
                    loginResponse = AuthenticationResponse.InvalidUserIdOrPassword;
                }
                outLoginUser = loginUser;
                return loginResponse;
            }
            catch (DbEntityValidationException Exception)
            {
                string Error = "";
                foreach (var eve in Exception.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Error = Error + ve.PropertyName + ve.ErrorMessage ;
                        //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        //    ve.PropertyName, ve.ErrorMessage);
                    }
                }
                //throw;

                throw Exception;

            }
        }
        
    }
}
