using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using EConnect.DAL;
using EConnect.NIELIT;
using System.IO;

public class CommonFunctions
{
	public CommonFunctions()
	{
		//
		// TODO: Add constructor logic here
		//
	}
	public static  Boolean IsNumeric(String value)
	{
		try
		{
			Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
			return regex.IsMatch(value.Trim());
		}
		catch (Exception)
		{
			return false;
		}
	}
	public static  Boolean IsDate(String value)
	{
		try
		{
			DateTime dt = Convert.ToDateTime(value);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
	public static  String GeInvalidRequestMessage(String redirectText, String redirectPage)
	{
		return "<B>Invalid Request Parameters</B><br><a href='" + redirectPage + "'>" + redirectText + "</a>";
	}
	public static  string GetInitCap(string str)
	{
		try
		{
			return new CultureInfo("en").TextInfo.ToTitleCase(str.ToLower());
			//return System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(str.ToLower());
			//return str.Substring(0, 1).ToUpper() + str.Substring(1, str.Length).ToLower();
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
	public static  bool IsValidEmailAddress(string emailAddress)
	{
		try
		{
			MailAddress m = new MailAddress(emailAddress, emailAddress);
			string MatchEmailPattern =
						@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
				 + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
							[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
				 + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
							[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
				 + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
			if (emailAddress != null)
				return Regex.IsMatch(emailAddress, MatchEmailPattern);
			else
				return false;
		}
		catch (FormatException)
		{
			return false;
		}
	}
	public static  bool IsValidMobileNumber(string mobileNumber)
	{
		try
		{
			if (mobileNumber.Length == 10)
			{
				if (!mobileNumber.StartsWith("0"))
				{
					return true;
				}
				else
					return false;
			}
			else
				return false;
		}
		catch (FormatException)
		{
			return false;
		}
	}
	public static void SendAccountActivationEmail(Int32 UserID, Boolean Isroot,string password = "")
	{ 
		string emailAddress = "";
		try
		{
			if (UserID > 0)
			{
				string  link = HttpContext.Current.Request.UrlReferrer.ToString().Substring(0, HttpContext.Current.Request.UrlReferrer.ToString().LastIndexOf('/'));
				using (EConnectContext context = new EConnectContext())
				{
					EConnect.URM.User objUser = context.Users.Find(UserID);
					if (objUser.enmUserType == EConnect.URM.UserType.Candidate)
					{
						emailAddress = (from c in context.CandidateContactDetails
										where c.CandidateID == objUser.UserRefNumber
										select new
										{
											email = c.EmailAddress
										}).FirstOrDefault().email;
					}
					else if (objUser.enmUserType == EConnect.URM.UserType.Admin || objUser.enmUserType == EConnect.URM.UserType.ExternalAdmin || objUser.enmUserType == EConnect.URM.UserType.HeadOffice)
					{
						emailAddress = (from c in context.ExternalEntities
										where c.ID == objUser.UserRefNumber
										select new
										{
											email = c.Email
										}).FirstOrDefault().email;
					}
					else if (objUser.enmUserType == EConnect.URM.UserType.Institute)
					{
						emailAddress = (from c in context.Institutes
										where c.ID == objUser.UserRefNumber
										select new
										{
											email = c.EmailAddress1
										}).FirstOrDefault().email;
					}
					 else if (objUser.enmUserType == EConnect.URM.UserType.NonAffiliatedInstitute)
                    {
                        NIELITMISContext context1 = new NIELITMISContext();
                        emailAddress = (from c in context1.NonAffInstitutes 
                                        where c.ID == objUser.UserRefNumber
                                        select new
                                        {
                                            email = c.EmailAddress1
                                        }).FirstOrDefault().email;
                    }
					else if (objUser.enmUserType == EConnect.URM.UserType.RegionalCenter || objUser.enmUserType ==EConnect .URM.UserType.projectNIELITCentre)
					{
                        //emailAddress = (from c in context.RegionalCenters
                        //                where c.ID == objUser.UserRefNumber
                        //                select new
                        //                {
                        //                    email = c.RegisteredEmailAddress
                        //                }).FirstOrDefault().email;
                        //deep add on 23 may 2018
                        emailAddress = (from c in context.Users
                                        where c.UserID == objUser.UserID
                                        select new
                                        {
                                            email = c.EmailID
                                        }).FirstOrDefault().email;
                        //deep end on 23 may 2018
					}
					if (emailAddress != "")
					{
						string PagePath = EConnect.Utils.Security.QuertStringModule.Encrypt("Activation.aspx?p1=" + objUser.UserID + "&p2=" + objUser.LoginID + "&p3=" + objUser.UserRefNumber);
						string msg = "";
						if (Isroot == false)
						{
							link = link.Substring(0, link.LastIndexOf('/'));
						}
						link = link + "/" + PagePath;
						if (password == "")
						{
							msg = "Dear " + GetInitCap(objUser.UserName) + ",<br/><br/>" +
							   "You can not login into Online Student Information and Enrollment System of NIELIT until your email address is verified.<BR> " +
							   "Please click on the following link to verify your email address. It will activate your user account.<br/><br/> <a href=" + link + ">Verify email address and activate your user account.</a>";
							EConnect.NIELIT.Email mail = new Email("Account Activation:NIELIT", msg, emailAddress);
							mail.Send();
						}
						else
						{
							msg = "Dear " + GetInitCap(objUser.UserName) + ",<br/><br/>" + "You have successfully registered with Online Student Information and Enrollment System of NIELIT. <br/> Your login details are as following: <br/><br/>User ID  &nbsp;&nbsp;&nbsp;&nbsp;: " + objUser.LoginID + "<br/>Password &nbsp;:  " + password + "<br/><br/><br/>" +
							   "You can not login into Online Student Information and Enrollment System of NIELIT until your email address is verified.<BR> " +
							   "Please click on the following link to verify your email address. It will activate your user account.<br/><br/> <a href=" + link+ ">Verify email address and activate your user account.</a>";
							EConnect.NIELIT.Email mail = new Email("New User Registration:NIELIT", msg, emailAddress);
							mail.Send();
						}
					}
				};
			}
			else
				throw new Exception("User ID can not be null or zero");
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
	public static void ForgotPasswordMail(Int32 UserID, string password)
	{
		string emailAddress = "";
		try
		{
			if (UserID >0 )
			{
				using (EConnectContext context = new EConnectContext())
				{
					EConnect.URM.User objUser = context.Users.Find(UserID);
					if (objUser.enmUserType == EConnect.URM.UserType.Candidate)
					{
						emailAddress = (from c in context.CandidateContactDetails
										where c.CandidateID == objUser.UserRefNumber
										select new
										{
											email = c.EmailAddress
										}).FirstOrDefault().email;
					}
					else if (objUser.enmUserType == EConnect.URM.UserType.Admin || objUser.enmUserType == EConnect.URM.UserType.ExternalAdmin || objUser.enmUserType == EConnect.URM.UserType.HeadOffice)
					{
						emailAddress = (from c in context.ExternalEntities
										where c.ID == objUser.UserRefNumber
										select new
										{
											email = c.Email
										}).FirstOrDefault().email;
					}
					else if (objUser.enmUserType == EConnect.URM.UserType.Institute)
					{
						emailAddress = (from c in context.Institutes
										where c.ID == objUser.UserRefNumber
										select new
										{
											email = c.EmailAddress1
										}).FirstOrDefault().email;
					}
					else if (objUser.enmUserType == EConnect.URM.UserType.RegionalCenter || objUser.enmUserType == EConnect.URM.UserType.projectNIELITCentre)
					{					
                        //emailAddress = (from c in context.RegionalCenters
                        //                where c.ID == objUser.UserRefNumber
                        //                select new
                        //                {
                        //                    email = c.RegisteredEmailAddress
                        //                }).FirstOrDefault().email;

                        //deep add on 24 may 2018
                        emailAddress = (from c in context.Users
                                        where c.UserID == objUser.UserID
                                        select new
                                        {
                                            email = c.EmailID
                                        }).FirstOrDefault().email;
                        //deep end on 24 may 2018
					}
					if (emailAddress != "")
					{
						string msg = "";
						msg = "Dear " + GetInitCap(objUser.UserName) + ",<br/><br/>" + "Your temporary password  and login-Id to login into  Online Student Information and Enrollment System of NIELIT  are as followed :- <br/><br/>User ID  &nbsp;&nbsp;&nbsp;&nbsp;: " + objUser.LoginID + "<br/> Temporary Password &nbsp;:  " + password + "<br/><br/><br/>" +
							   "Note:- Please change your Password after login.<br/><br/> " +
							   "Thanks<br />" +
							   "NIELIT Team";
						EConnect.NIELIT.Email mail = new Email("Forgot Password:NIELIT", msg, emailAddress);
						mail.Send();
					}
				};
			}
			else
				throw new Exception("User ID can not be null or zero");
		}
		catch (Exception ex)
		{
			try
			{
				SendSmsToAdminOnEmailFailed("Dear Admin,<Br>A Forgot Password Email could not be sent to email address " + emailAddress + ".Reason: " + ex.ToString());
			}
			catch (Exception exec)
			{
			}
			throw ex;
		}
	}
	public static void GenerateMobileOTP(EConnect.URM.UserType enmUserType, Int64 entityID, Boolean resendOTP = false)
	{
		try
		{

			Int32 OTP = Convert.ToInt32(EConnect.CommonFunctions.GenerateRandomNumber(6));
			using (EConnectContext context = new EConnectContext())
			{
				if (enmUserType == EConnect.URM.UserType.Candidate)
				{
					var mobile = context.CandidateContactDetails.Where(s => s.CandidateID == entityID).FirstOrDefault();
					if (mobile.MobileOTP == null || resendOTP ==true)
					{
						mobile.MobileOTP = OTP;
						mobile.MobileOTPValidUptoDate = DateTime.Now;
						context.Entry(mobile).State = System.Data.Entity.EntityState.Modified;
						//EConnect.NIELIT.SMS message = new SMS("OTP for mobile number verification is " + OTP.ToString() + ". Reference Number:" + mobile.MobileOTPValidUptoDate.Value.ToString("hhmmss") + ". Use this OTP to complete verification process", mobile.MobileNumber.ToString(), SmsServiceType.SignleSMS);
						EConnect.NIELIT.SMS message = new SMS("OTP for mobile number verification is " + OTP.ToString() + ". Reference Number: " + mobile.MobileOTPValidUptoDate.Value.ToString("hhmmss") + ". Use this OTP to complete verification process-NIELIT", mobile.MobileNumber.ToString(), "1307161052940277785", SmsServiceType.SignleSMS);
						int sentMessageCount;
						message.sendOTPMSG(out sentMessageCount);
						if (sentMessageCount == 1)
							context.SaveChanges();
						else
						{
							String msg = "Dear Admin<SMS could not be sent to the mobile number " + mobile.MobileNumber.ToString() + " on " + DateTime.Now.ToString() + ".<BR>Reaon: 0 message sent.";
							SendEMailToAdminOnSmsFailed(msg);
							throw new Exception("OTP could not be sent due to some technical problem. Please try again later");
						}
					}
				}
			};

		}
		catch (Exception ex)
		{ throw ex; }
	}
	public static void SendEMailToAdminOnSmsFailed(String msg)
	{
		try
		{
			using (EConnectContext context = new EConnectContext())
			{
				var emailAddress = context.Organizations.Find(1).EmailTechnicalPerson;
				if(emailAddress !=null)
				{
					EConnect.NIELIT.Email mail = new Email("New User Registration:NIELIT", msg,emailAddress);
					mail.Send();
				}
			};
		}
		catch (Exception)
		{ 
		}
	}
	public static void SendSmsToAdminOnEmailFailed(String msg)
	{
		try
		{
			using (EConnectContext context = new EConnectContext())
			{
				var mobileNumber = context.Organizations.Find(1).MobileNumberTechnicalPerson;
				if (mobileNumber.HasValue)
				{
					//EConnect.NIELIT.SMS message = new SMS(msg,mobileNumber.Value.ToString(), SmsServiceType.SignleSMS);
					EConnect.NIELIT.SMS message = new SMS(msg,mobileNumber.Value.ToString(),"test", SmsServiceType.SignleSMS);
					int sentMessageCount;
					message.sendSingleSMS(out sentMessageCount);
				}
			};
		}
		catch (Exception){}
	}
	public static void GenerateEmailOTP(EConnect.URM.UserType enmUserType, Int64 entityID, Boolean resendOTP = false)
	{
		try
		{
			Int32 OTP = Convert.ToInt32(EConnect.CommonFunctions.GenerateRandomNumber(6));
			using (EConnectContext context = new EConnectContext())
			{
				if (enmUserType == EConnect.URM.UserType.Candidate)
				{
					var email = context.CandidateContactDetails.Where(s => s.CandidateID == entityID).FirstOrDefault();
					if (email.EmailOTP == null || resendOTP == true)
					{
						email.EmailOTP = OTP;
						email.EmailOTPValidUptoDate = DateTime.Now;
						context.Entry(email).State =  EntityState.Modified;
						email.EmailOTPValidUptoDate = DateTime.Now.AddDays(1);
						context.SaveChanges();
						string msg = "";
						var obj = context.Candidates.Find(entityID);
						msg = "Dear " + GetInitCap(obj.Name) + ",<br/><br/>" + "OTP for Email Address verification is " + OTP.ToString() + ". Reference Number:" + email.EmailOTPValidUptoDate.Value.ToString("hhmmss") + ". Use this OTP to complete Email Address verification process<br/><br/><br/><br/><br/>Thank You,<br/> NIELIT";
						EConnect.NIELIT.Email mail = new Email("OTP for Email Address Verification :NIELIT", msg, email.EmailAddress);

						try
						{
							mail.Send();
						}
						catch(Exception)
						{
						  throw new Exception("OTP has been sent to your registered E-mail ID. Please check junk/spam mail if you not received this mail in your inbox");
						  //throw new Exception("OTP could not be sent due to some technical problem. Please try again later");
						}
					}
				}
			}
		}
		catch (Exception ex)
		{ throw ex; }
	}
	public static bool IsDemandNoteCancellable(Int64 DemandNoteID)
	{
		try
		{
			using(EConnectContext context= new EConnectContext())
			{
				var demand = context.DemandNotes.Where(s => s.ID == DemandNoteID).ToList();
				if (demand.Count() >= 0)
				{
					if ((context.OnlineTransaction.Where(s => s.DemandNoteID == DemandNoteID).Count() == 0) && (context.NEFTTransactions.Where(s => s.DemandNoteID == DemandNoteID).Count() == 0) && (context.CSCTransactions.Where(s => s.DemandNoteID == DemandNoteID).Count() == 0))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				return false;
			};
		}
		catch(Exception)
		{
			return false;
		}
	}
	public static string RemoveSpaces(string str)
	{
		try
		{
			while (str.Contains("  "))
			{
				str = str.Replace("  ", " ");
			}
			return str;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}
//Added 10 Sep 2020
    public static void createLog(string logFile, string text)
    {
       // string logFile = Convert.ToString(dd + "_1.txt");
        logFile = HttpContext.Current.Server.MapPath("~/UploadedFiles/" + logFile);
        // Open the log file for append and write the log
        StreamWriter sw = new StreamWriter(logFile, true);
        sw.WriteLine("Log File");
        sw.WriteLine(" ");
        sw.WriteLine("********** {0} **********", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        sw.WriteLine(text);
        sw.WriteLine(" ");
        sw.Close();
    }
	    //Added for email  display 3 feb 2021
    public static string ChangeEmailDisplay(string email)
    {
        string newEmail = "";
        try
        {
            newEmail = email.Replace("@", "[at]");
            newEmail = newEmail.Replace(".", "[dot]");
            newEmail = newEmail.Replace("-", "[hyphen]");

            return newEmail;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
// added on 17 November 2021 for check the demand note id payment status try 
    public static bool IsDemandNoteCancellableVirtualAcademy(Int64 DemandNoteID)
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                var demand = context.VirtualAcademyDemandNotes.Where(s => s.ID == DemandNoteID).ToList();
                if (demand.Count() >= 0)
                {
                    if ((context.VirtualAcademyOnlineTransaction.Where(s => s.DemandNoteID == DemandNoteID).Count() == 0) )
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            };
        }
        catch (Exception)
        {
            return false;
        }
    }

    // ended on 17  

}