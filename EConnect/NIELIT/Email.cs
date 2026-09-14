using System;
using System.Net.Mail;
//Added 14 Jan 2023 for email
using System.Net;
//
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using EConnect.DAL;

namespace EConnect.NIELIT
{
	public class Email
	{
		private String _emailUrl = "";
		String strSaveFrom = "Do not reply <donotreply@nielit.gov.in>";
		private String _senderEmailId;
		private String _password;
		private String _subject;
		private String _mailBody;
		private String _recepientEmailAddress;
		private Boolean _isManuallySent = false;
		private String _host = "202.41.97.145";
		private String _crUserName = "";
		private String _crPassword = "";
		String contentData;
		/// <summary>
		/// New Email
		/// </summary>
		/// <param name="subject">Subject of email</param>
		/// <param name="mailMessage">Email body</param>
		/// <param name="recepientEmailID">Email addresses of the recepient</param>
		public Email(String subject, String body, String emailAddressTo, Boolean isManuallySent = false)
		{
			//_senderEmailId = "donotreply@nielit.gov.in";
			//_password = "P@55w0rd";
			//_mailServer = "202.157.74.20";
			try
			{
				_senderEmailId = System.Web.Configuration.WebConfigurationManager.AppSettings["senderEmailId"].ToString();
				_password = System.Web.Configuration.WebConfigurationManager.AppSettings["senderEmailPassword"].ToString();
				_emailUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["emailUrl"].ToString();
				_host = System.Web.Configuration.WebConfigurationManager.AppSettings["host"].ToString();
				_crUserName = System.Web.Configuration.WebConfigurationManager.AppSettings["userName"].ToString();
				_crPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["password"].ToString();
				_isManuallySent = isManuallySent;
			}
			catch (Exception) { }
			if (string.IsNullOrEmpty(_senderEmailId))
			{
				_senderEmailId = "donotreply@nielit.gov.in";
				_password = "P@55w0rd";
				_emailUrl = "https://mail.nielit.in/web-request.jsp";
				_host = "202.41.97.145";
				_crUserName = "";
				_crPassword = "";
			}
			_subject = subject;
			_mailBody = body;
			_recepientEmailAddress = emailAddressTo;
			strSaveFrom = "DoNotReply<" + _senderEmailId + ">";
			#region "Post Data Creation"
			StringBuilder values = new StringBuilder();

			values.AppendFormat("{0}={1}&", "from", _senderEmailId);
			values.AppendFormat("{0}={1}&", "to", _recepientEmailAddress);
			values.AppendFormat("{0}={1}&", "sub", _subject);
			values.AppendFormat("{0}={1}&", "pass", _password);
			values.AppendFormat("{0}={1}&", "data", HttpUtility.UrlEncode(_mailBody));
			values.AppendFormat("{0}={1}&", "cc", "");
			values.AppendFormat("{0}={1}&", "bcc", "");
			values.AppendFormat("{0}={1}", "savefrom", strSaveFrom);
			contentData = values.ToString();
			#endregion
		}
		private void ValidateEmailParameters()
		{
			try
			{
				if (String.IsNullOrEmpty(_subject.Trim()))
					throw new ArgumentException("Subject can not be blank");
				if (String.IsNullOrEmpty(_mailBody.Trim()))
					throw new ArgumentException("Email message/body can not be blank");
				if (String.IsNullOrEmpty(_recepientEmailAddress.Trim()))
					throw new ArgumentException("Email address of recepient can not be blank");
				if (!IsValidEmailAddress(_recepientEmailAddress))
					throw new ArgumentException("Invalid email address of the recepient");
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		private bool IsValidEmailAddress(string emailAddress)
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
		public void Send()
		{
			try
			{
				String responseData = "";
				ValidateEmailParameters();
				try
				{
					System.Net.NetworkCredential cred = new System.Net.NetworkCredential(_crUserName, _crPassword);
					SmtpClient client = new SmtpClient();
					client.Host = _host;
					client.Credentials = cred;
					//Added code for email 14 Jan 2023
					// Email changed 16 nov 2023 
					//client.Port = 25;
                    client.Port = 465;
					client.EnableSsl = true;
					client.DeliveryMethod = SmtpDeliveryMethod.Network;
					ServicePointManager.ServerCertificateValidationCallback += (s, ce, ca, p) => true;
					ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;// | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
																					  //



					MailMessage mail = new MailMessage();
					mail.Subject = _subject;
					mail.From = new MailAddress("donotreply@nielit.gov.in", "DoNotReply");
					mail.IsBodyHtml = true;
					mail.To.Add(_recepientEmailAddress);
					mail.Body = _mailBody;
					client.Send(mail);
					responseData = "Success.";
					mail.Dispose();

					SentEmail sentMail;
					using (EConnectContext context = new EConnectContext())
					{
						sentMail = new SentEmail();
						if (_subject.Length > 100)
							_subject = _subject.Substring(0, 100);
						sentMail.Subject = _subject;
						sentMail.Message = _mailBody;
						sentMail.IsManuallySent = _isManuallySent;
						if (_recepientEmailAddress.Length > 70)
							_recepientEmailAddress = _recepientEmailAddress.Substring(1, 70);
						sentMail.EmailAddress = _recepientEmailAddress;
						sentMail.ResponseMessage = responseData;
						sentMail.SentOn = DateTime.Now;
						if (HttpContext.Current.Session["UserID"] != null)
							sentMail.SentByUserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
						context.SentEmails.Add(sentMail);
						context.SaveChanges();
					};
				}
				catch (Exception){}				
			}
			catch (Exception ex)
			{
				using (EConnect.DAL.EConnectContext context = new EConnect.DAL.EConnectContext())
				{
					var mobileNumber = context.Organizations.Find(1).MobileNumberTechnicalPerson;
					if (mobileNumber.HasValue)
					{
                        EConnect.NIELIT.SMS message = new SMS("Email sending failure: " + ex.Message, mobileNumber.Value.ToString(), "1307161052903404302", SmsServiceType.SignleSMS);
						int sentMessageCount;
						message.sendSingleSMS(out sentMessageCount);
					}
				};
				throw ex;
			}
		}
		public void SendInThread(Int32 userID)
		{
			try
			{
				String responseData = "";
				ValidateEmailParameters();
				try
				{
					System.Net.NetworkCredential cred = new System.Net.NetworkCredential(_crUserName, _crPassword);
					SmtpClient client = new SmtpClient();
					client.Host = _host;
					client.Credentials = cred;
					MailMessage mail = new MailMessage();
					mail.Subject = _subject;
					mail.From = new MailAddress("donotreply@nielit.gov.in", "DoNotReply");
					mail.IsBodyHtml = true;
					mail.To.Add(_recepientEmailAddress);
					mail.Body = _mailBody;
					client.Send(mail);
					responseData = "Success.";
					mail.Dispose();

					SentEmail sentMail;
					using (EConnectContext context = new EConnectContext())
					{
						sentMail = new SentEmail();
						if (_subject.Length > 100)
							_subject = _subject.Substring(0, 100);
						sentMail.Subject = _subject;
						sentMail.Message = _mailBody;
						sentMail.IsManuallySent = _isManuallySent;
						if (_recepientEmailAddress.Length > 70)
							_recepientEmailAddress = _recepientEmailAddress.Substring(1, 70);
						sentMail.EmailAddress = _recepientEmailAddress;
						sentMail.ResponseMessage = responseData;
						sentMail.SentOn = DateTime.Now;
						if (HttpContext.Current.Session["UserID"] != null)
							sentMail.SentByUserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
						context.SentEmails.Add(sentMail);
						context.SaveChanges();
					};
				}
				catch (Exception) { }			
			}
			catch (Exception ex)
			{
				using (EConnect.DAL.EConnectContext context = new EConnect.DAL.EConnectContext())
				{
					var mobileNumber = context.Organizations.Find(1).MobileNumberTechnicalPerson;
					if (mobileNumber.HasValue)
					{
                        EConnect.NIELIT.SMS message = new SMS("Email sending failure: " + ex.Message, mobileNumber.Value.ToString(), "1307161052903404302", SmsServiceType.SignleSMS);
						int sentMessageCount;
						message.sendSingleSMS(out sentMessageCount);
					}
				};
				throw ex;
			}
		}
	}
}
