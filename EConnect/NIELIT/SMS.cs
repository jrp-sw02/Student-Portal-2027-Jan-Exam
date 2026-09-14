using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using EConnect.DAL;

namespace EConnect.NIELIT
{
    /// <summary>
    /// SMS service type for single and bulk SMS
    /// </summary>
    public enum SmsServiceType
    {
        [Description("singlemsg")]
        SignleSMS = 1,
        [Description("bulkmsg")]
        BulkSMS = 2,
        [Description("otpmsg")]
        OtpSMS = 3,
        [Description("unicodemsg")]
        UnicodeSMS = 4
    }
    /// <summary>
    /// Response Codes sent by sms server
    /// </summary>
    public enum SmsResponse
    {
        [Description("Credentials Error, may be invalid username or password")]
        CredentialsError = 401,
        [Description("messages submitted successfully")]
        MessagesSubmittedSuccessfully = 402,
        [Description("Credits not available")]
        CreditsNotAvailable = 403,
        [Description("Internal Database Error")]
        InternalDatabaseError = 404,
        [Description("Internal Networking Error")]
        InternalNetworkingError = 405,
        [Description("Invalid or Duplicate numbers")]
        InvalidOrDuplicateNumbers = 406,
        [Description("Network Error on SMSC")]
        NetworkErrorOnSMSC = 407,
        [Description("Network Error on SMSC")]
        NetworkErrorOnSMSC1 = 408,
        [Description("SMSC response timed out, message will be submitted")]
        SmsCResponseTimedOutMessageWillBeSubmitted = 409,
        [Description("Internal Limit Exceeded, Contact support")]
        InternalLimitExceededContactsupport = 410,
        [Description("Sender ID not approved")]
        SenderIdNotApproved = 411,
        [Description("Sender ID not approved")]
        SenderIdNotApproved1 = 412,
        [Description("Suspect Spam, we do not accept these messages")]
        SuspectSpamWeDoNotAcceptTheseMessages = 413,
        [Description("Rejected by various reasons by the operator such as DND, SPAM etc")]
        RejectedByVariousReasonsByTheOperatorSuchAsDndSpamEtc = 414,
        [Description("Secure Key not available")]
        SecureKetNotAvailable = 415,
        [Description("Hash does not match")]
        HashDoesNotMatch = 416
    }
    /// <summary>
    /// How to send message using this class
    /// SMS message = new SMS("Message to be sent upto 160 characters", "9460854860,9413318916", EConnect.NIELIT.SmsServiceType.BulkSMS);
    /// Int32 numberOfSentMessages;
    /// SmsResponse response = message.Send(out numberOfSentMessages);
    /// Response.Write(numberOfSentMessages.ToString() + " " + EConnect.Utils.Common.EnumUtility.GetDescription(response));
    /// </summary>
    public class SMS
    {
        private String _userName = "NIELITCHD-Student";
        private String _password = "OnlineStud@123$";
        private String _senderID = "NIELIT";
        private String _SecureKey = "f1656e34-cd2b-4118-be81-80796338f086";
        private String _url = "https://msdgweb.mgov.gov.in/esms/sendsmsrequestDLT";
        //Changed as copied 8 Mar 2021
        //private String _url = "https://msdgweb.mgov.gov.in/esms/sendsmsrequest";        
        private SmsServiceType _smsServiceType = SmsServiceType.SignleSMS;
        private String _message = "";
        private String _mobileNumbers = "";
        private Boolean _isManuallySent = false;
        private String MsgId = "";
        //Added Templateid for SMS
        private string _templateId = "";
        /// <summary>
        /// New Text Message
        /// </summary>
        /// <param name="message">Text Message upto 160 character length</param>
        /// <param name="mobileNumbers">10 digit Mobile Numbers(Quoma separated in case of ultiple SMS recipients) of SMS recipient(s)</param>
        /// <param name="serviceType">Select SignleSMS in case of 1 SMS recipient or select BulkSMS in case of multiple SMS recipients.</param>
        public SMS(String message, String mobileNumbers, string templateId, SmsServiceType serviceType = SmsServiceType.SignleSMS, Boolean isManuallySent = false)
        {
            _smsServiceType = serviceType;
            _message = message.Replace("&", "").Replace("?", "");
            _mobileNumbers = mobileNumbers.Trim().Trim(',');
            _isManuallySent = isManuallySent;
            //Added template id for SMS
            _templateId = templateId;
        }
        private void ValidateSMSParameters()
        {
            try
            {
                Int32 numberlength = 0;
                if (String.IsNullOrEmpty(_message.Trim()))
                    throw new ArgumentException("Message can not be blank");
                if (String.IsNullOrEmpty(_mobileNumbers.Trim()))
                    throw new ArgumentException("Mobile Number can not be blank");
                // Added for template id for SMS
                if (String.IsNullOrEmpty(_templateId.Trim()))
                    throw new ArgumentException("Template Id can not be blank");
                if (_smsServiceType == SmsServiceType.SignleSMS)
                {
                    numberlength = _mobileNumbers.Length;
                    if (numberlength != 10)
                        throw new Exception("Invalid mobile number. Mobile number should be of 10 digits only");
                    else if (_mobileNumbers.StartsWith("0"))
                        throw new Exception("Invalid mobile number. Mobile number can not start with '0'");
                }
                else if (_smsServiceType == SmsServiceType.BulkSMS)
                {
                    if (_mobileNumbers.Contains(','))
                    {
                        string[] numbers = _mobileNumbers.Split(',');
                        StringBuilder sb = new StringBuilder();
                        foreach (String number in numbers)
                        {
                            numberlength = number.Trim().Length;
                            if (numberlength != 10)
                                throw new Exception("Invalid mobile number(" + number.ToString() + "). Mobile number should be of 10 digits only");
                            else if (number.StartsWith("0"))
                                throw new Exception("Invalid mobile number(" + number.ToString() + "). Mobile number can not start with '0'");
                            sb.Append(number.Trim() + ",");
                        }
                        _mobileNumbers = sb.ToString().Trim().Trim(',');
                    }
                    else
                    {
                        numberlength = _mobileNumbers.Trim().Length;
                        if (numberlength != 10)
                            throw new Exception("Invalid mobile number(" + _mobileNumbers.ToString() + "). Mobile number should be of 10 digits only");
                        else if (_mobileNumbers.StartsWith("0"))
                            throw new Exception("Invalid mobile number(" + _mobileNumbers.ToString() + "). Mobile number can not start with '0'");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Old Methoda, Now not in use
        /// <summary>
        /// Send the Text Message
        /// </summary>
        /// <returns>Response Status</returns>
        //public SmsResponse Send(out Int32 numberOfSentSMS)
        //{
        //    try
        //    {
        //        NIELIT.SmsResponse enmSmsResponse = SmsResponse.MessagesSubmittedSuccessfully;
        //        numberOfSentSMS = 1;
        //        SentSMS sentSms;
        //        ValidateSMSParameters();

        //        Stream dataStream;
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
        //        request.Method = "POST";
        //        String smsservicetype = EConnect.Utils.Common.EnumUtility.GetDescription(_smsServiceType).ToLower();// "bulkmsg"; // for bulk msg
        //        String query = "";
        //        if (_smsServiceType == SmsServiceType.BulkSMS)
        //        {
        //            query = "username=" + HttpUtility.UrlEncode(_userName) +
        //                    "&password=" + HttpUtility.UrlEncode(_password) +
        //                    "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
        //                    "&content=" + HttpUtility.UrlEncode(_message) +
        //                    "&bulkmobno=" + HttpUtility.UrlEncode(_mobileNumbers) +
        //                    "&senderid=" + HttpUtility.UrlEncode(_senderID);
        //        }
        //        else
        //        {
        //            query = "username=" + HttpUtility.UrlEncode(_userName) +
        //                    "&password=" + HttpUtility.UrlEncode(_password) +
        //                    "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
        //                    "&content=" + HttpUtility.UrlEncode(_message) +
        //                    "&mobileno=" + HttpUtility.UrlEncode(_mobileNumbers) +
        //                    "&senderid=" + HttpUtility.UrlEncode(_senderID);
        //        }
        //        byte[] byteArray = Encoding.ASCII.GetBytes(query);
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.ContentLength = byteArray.Length;
        //        dataStream = request.GetRequestStream();
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //        dataStream.Close();
        //        WebResponse response = request.GetResponse();
        //        String Status = ((HttpWebResponse)response).StatusDescription;
        //        dataStream = response.GetResponseStream();
        //        StreamReader reader = new StreamReader(dataStream);

        //        string responseFromServer = reader.ReadToEnd();

        //        numberOfSentSMS = 0;
        //        if (responseFromServer.Contains(","))
        //        {
        //            String[] arrResponse = responseFromServer.Split(',');
        //            if (arrResponse[0].Length > 3)
        //                enmSmsResponse = (SmsResponse)Convert.ToInt32(arrResponse[0].Substring(0, 3));
        //            else
        //                enmSmsResponse = (SmsResponse)Convert.ToInt32(arrResponse[0].Trim());
        //            //enmSmsResponse = (SmsResponse)Convert.ToInt32(arrResponse[0].Trim());
        //            if (enmSmsResponse == SmsResponse.MessagesSubmittedSuccessfully)
        //            {
        //                //numberOfSentSMS = Convert.ToInt32(arrResponse[1].Trim());
        //                MsgId = arrResponse[1].Trim();
        //                numberOfSentSMS = 1;
        //                try
        //                {
        //                    using (EConnectContext context = new EConnectContext())
        //                    {
        //                        if (_smsServiceType == SmsServiceType.SignleSMS)
        //                        {
        //                            sentSms = new SentSMS();
        //                            if (_message.Length > 500)
        //                                _message = _message.Substring(0, 500);
        //                            sentSms.Message = _message;
        //                            sentSms.IsManuallySent = _isManuallySent;
        //                            sentSms.MobileNumber = Convert.ToInt64(_mobileNumbers);
        //                            sentSms.Response = enmSmsResponse;
        //                            sentSms.SentOn = DateTime.Now;
        //                            sentSms.MsgId = MsgId;
        //                            if (HttpContext.Current.Session["UserID"] != null)
        //                                sentSms.SentByUserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        //                            context.SentSMS.Add(sentSms);

        //                        }
        //                        else if (_smsServiceType == SmsServiceType.BulkSMS)
        //                        {
        //                            string[] numbers = _mobileNumbers.Split(',');
        //                            StringBuilder sb = new StringBuilder();
        //                            foreach (String number in numbers)
        //                            {
        //                                sentSms = new SentSMS();
        //                                if (_message.Length > 500)
        //                                    _message = _message.Substring(0, 500);
        //                                sentSms.IsManuallySent = _isManuallySent;
        //                                sentSms.Message = _message;
        //                                sentSms.MobileNumber = Convert.ToInt64(number);
        //                                sentSms.Response = enmSmsResponse;
        //                                sentSms.SentOn = DateTime.Now;
        //                                sentSms.MsgId = MsgId;
        //                                if (HttpContext.Current.Session["UserID"] != null)
        //                                    sentSms.SentByUserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        //                                context.SentSMS.Add(sentSms);
        //                            }
        //                        }
        //                        context.SaveChanges();
        //                    };
        //                }
        //                catch (Exception ex)
        //                {

        //                }
        //            }
        //        }
        //        else
        //        {
        //            enmSmsResponse = (SmsResponse)Convert.ToInt32(responseFromServer.Trim());
        //        }
        //        reader.Close();
        //        dataStream.Close();
        //        dataStream.Dispose();
        //        response.Close();
        //        return enmSmsResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        using (EConnectContext context = new EConnectContext())
        //        {
        //            var emailAddress = context.Organizations.Find(1).EmailTechnicalPerson;
        //            if (emailAddress != null)
        //            {
        //                EConnect.NIELIT.Email mail = new Email("SMS sending failure", "Dear Admin,SMS could not be sent to " + _mobileNumbers.ToString() + " on " + DateTime.Now.ToString() + ".<BR>Reason:" + ex.Message, emailAddress);
        //                mail.Send();
        //            }
        //        };
        //        throw ex;
        //    }
        //}
        //public SmsResponse Send(out String numberOfSentSMS)
        //{
        //    try
        //    {
        //        NIELIT.SmsResponse enmSmsResponse = SmsResponse.MessagesSubmittedSuccessfully;
        //        numberOfSentSMS = "1";
        //        SentSMS sentSms;
        //        ValidateSMSParameters();
        //        try
        //        {
        //            Stream dataStream1;
        //            HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(_url1 + "&dest=" + _mobileNumbers + "&msg=" + _message + "&prty=3&vp=30");
        //            request1.Method = "GET";
        //            WebResponse response1 = request1.GetResponse();
        //            String Status1 = ((HttpWebResponse)response1).StatusDescription;
        //            dataStream1 = response1.GetResponseStream();
        //            StreamReader reader1 = new StreamReader(dataStream1);
        //            string responseFromServer1 = reader1.ReadToEnd();
        //            reader1.Close();
        //            dataStream1.Close();
        //            dataStream1.Dispose();
        //            response1.Close();
        //            if (!responseFromServer1.ToUpper().StartsWith("NH"))
        //                enmSmsResponse = SmsResponse.InvalidOrDuplicateNumbers;
        //            else
        //                enmSmsResponse = SmsResponse.MessagesSubmittedSuccessfully;
        //            using (EConnectContext context = new EConnectContext())
        //            {
        //                string[] numbers = _mobileNumbers.Split(',');
        //                StringBuilder sb = new StringBuilder();
        //                foreach (String number in numbers)
        //                {
        //                    sentSms = new SentSMS();
        //                    if (_message.Length > 500)
        //                        _message = _message.Substring(0, 500);
        //                    sentSms.IsManuallySent = _isManuallySent;
        //                    sentSms.Message = _message;
        //                    sentSms.MobileNumber = Convert.ToInt64(number);
        //                    sentSms.Response = enmSmsResponse;
        //                    sentSms.SentOn = DateTime.Now;
        //                    sentSms.MsgId = MsgId;
        //                    if (HttpContext.Current.Session["UserID"] != null)
        //                        sentSms.SentByUserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        //                    context.SentSMS.Add(sentSms);
        //                }
        //                context.SaveChanges();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //        enmSmsResponse = SmsResponse.MessagesSubmittedSuccessfully;
        //        Stream dataStream;
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
        //        request.Method = "POST";
        //        String smsservicetype = EConnect.Utils.Common.EnumUtility.GetDescription(_smsServiceType).ToLower();// "bulkmsg"; // for bulk msg
        //        String query = "";
        //        if (_smsServiceType == SmsServiceType.BulkSMS)
        //        {
        //            query = "username=" + HttpUtility.UrlEncode(_userName) +
        //                    "&password=" + HttpUtility.UrlEncode(_password) +
        //                    "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
        //                    "&content=" + HttpUtility.UrlEncode(_message) +
        //                    "&bulkmobno=" + HttpUtility.UrlEncode(_mobileNumbers) +
        //                    "&senderid=" + HttpUtility.UrlEncode(_senderID);
        //        }
        //        else
        //        {
        //            query = "username=" + HttpUtility.UrlEncode(_userName) +
        //                    "&password=" + HttpUtility.UrlEncode(_password) +
        //                    "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
        //                    "&content=" + HttpUtility.UrlEncode(_message) +
        //                    "&mobileno=" + HttpUtility.UrlEncode(_mobileNumbers) +
        //                    "&senderid=" + HttpUtility.UrlEncode(_senderID);
        //        }
        //        byte[] byteArray = Encoding.ASCII.GetBytes(query);
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.ContentLength = byteArray.Length;
        //        dataStream = request.GetRequestStream();
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //        dataStream.Close();
        //        WebResponse response = request.GetResponse();
        //        String Status = ((HttpWebResponse)response).StatusDescription;
        //        dataStream = response.GetResponseStream();
        //        StreamReader reader = new StreamReader(dataStream);

        //        string responseFromServer = reader.ReadToEnd();

        //        numberOfSentSMS = "0";
        //        if (responseFromServer.Contains(","))
        //        {
        //            String[] arrResponse = responseFromServer.Split(',');
        //            if (arrResponse[0].Length > 3)
        //                enmSmsResponse = (SmsResponse)Convert.ToInt32(arrResponse[0].Substring(0, 3));
        //            else
        //                enmSmsResponse = (SmsResponse)Convert.ToInt32(arrResponse[0].Trim());
        //            if (enmSmsResponse == SmsResponse.MessagesSubmittedSuccessfully)
        //            {
        //                //numberOfSentSMS = responseFromServer;
        //                //numberOfSentSMS = arrResponse[1].Trim();
        //                MsgId = arrResponse[1].Trim();
        //                numberOfSentSMS = "1";
        //                try
        //                {

        //                    using (EConnectContext context = new EConnectContext())
        //                    {
        //                        if (_smsServiceType == SmsServiceType.SignleSMS)
        //                        {
        //                            sentSms = new SentSMS();
        //                            if (_message.Length > 500)
        //                                _message = _message.Substring(0, 500);
        //                            sentSms.Message = _message;
        //                            sentSms.IsManuallySent = _isManuallySent;
        //                            sentSms.MobileNumber = Convert.ToInt64(_mobileNumbers);
        //                            sentSms.Response = enmSmsResponse;
        //                            sentSms.SentOn = DateTime.Now;
        //                            sentSms.MsgId = MsgId;
        //                            if (HttpContext.Current.Session["UserID"] != null)
        //                                sentSms.SentByUserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        //                            context.SentSMS.Add(sentSms);

        //                        }
        //                        else if (_smsServiceType == SmsServiceType.BulkSMS)
        //                        {
        //                            string[] numbers = _mobileNumbers.Split(',');
        //                            StringBuilder sb = new StringBuilder();
        //                            foreach (String number in numbers)
        //                            {
        //                                sentSms = new SentSMS();
        //                                if (_message.Length > 500)
        //                                    _message = _message.Substring(0, 500);
        //                                sentSms.IsManuallySent = _isManuallySent;
        //                                sentSms.Message = _message;
        //                                sentSms.MobileNumber = Convert.ToInt64(number);
        //                                sentSms.Response = enmSmsResponse;
        //                                sentSms.SentOn = DateTime.Now;
        //                                sentSms.MsgId = MsgId;
        //                                if (HttpContext.Current.Session["UserID"] != null)
        //                                    sentSms.SentByUserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
        //                                context.SentSMS.Add(sentSms);
        //                            }
        //                        }
        //                        context.SaveChanges();
        //                    };
        //                }
        //                catch (Exception ex)
        //                {

        //                }
        //            }
        //        }
        //        else
        //        {
        //            enmSmsResponse = (SmsResponse)Convert.ToInt32(responseFromServer.Trim());
        //        }
        //        reader.Close();
        //        dataStream.Close();
        //        dataStream.Dispose();
        //        response.Close();
        //        return enmSmsResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        using (EConnectContext context = new EConnectContext())
        //        {
        //            var emailAddress = context.Organizations.Find(1).EmailTechnicalPerson;
        //            if (emailAddress != null)
        //            {
        //                EConnect.NIELIT.Email mail = new Email("SMS sending failure", "Dear Admin,SMS could not be sent to " + _mobileNumbers.ToString() + " on " + DateTime.Now.ToString() + ".<BR>Reason:" + ex.Message, emailAddress);
        //                mail.Send();
        //            }
        //        };
        //        throw ex;
        //    }
        //}
        //public SmsResponse SendInThread(out Int32 numberOfSentSMS, Int32 userID)
        //{
        //    try
        //    {
        //        NIELIT.SmsResponse enmSmsResponse = SmsResponse.MessagesSubmittedSuccessfully;
        //        numberOfSentSMS = 1;
        //        SentSMS sentSms;
        //        ValidateSMSParameters();

        //        Stream dataStream;
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
        //        request.Method = "POST";
        //        String smsservicetype = EConnect.Utils.Common.EnumUtility.GetDescription(_smsServiceType).ToLower();// "bulkmsg"; // for bulk msg
        //        String query = "";
        //        if (_smsServiceType == SmsServiceType.BulkSMS)
        //        {
        //            query = "username=" + HttpUtility.UrlEncode(_userName) +
        //                    "&password=" + HttpUtility.UrlEncode(_password) +
        //                    "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
        //                    "&content=" + HttpUtility.UrlEncode(_message) +
        //                    "&bulkmobno=" + HttpUtility.UrlEncode(_mobileNumbers) +
        //                    "&senderid=" + HttpUtility.UrlEncode(_senderID);
        //        }
        //        else
        //        {
        //            query = "username=" + HttpUtility.UrlEncode(_userName) +
        //                    "&password=" + HttpUtility.UrlEncode(_password) +
        //                    "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
        //                    "&content=" + HttpUtility.UrlEncode(_message) +
        //                    "&mobileno=" + HttpUtility.UrlEncode(_mobileNumbers) +
        //                    "&senderid=" + HttpUtility.UrlEncode(_senderID);
        //        }
        //        byte[] byteArray = Encoding.ASCII.GetBytes(query);
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.ContentLength = byteArray.Length;
        //        dataStream = request.GetRequestStream();
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //        dataStream.Close();
        //        WebResponse response = request.GetResponse();
        //        String Status = ((HttpWebResponse)response).StatusDescription;
        //        dataStream = response.GetResponseStream();
        //        StreamReader reader = new StreamReader(dataStream);

        //        string responseFromServer = reader.ReadToEnd();

        //        numberOfSentSMS = 0;
        //        if (responseFromServer.Contains(","))
        //        {
        //            String[] arrResponse = responseFromServer.Split(',');
        //            if (arrResponse[0].Length > 3)
        //                enmSmsResponse = (SmsResponse)Convert.ToInt32(arrResponse[0].Substring(0, 3));
        //            else
        //                enmSmsResponse = (SmsResponse)Convert.ToInt32(arrResponse[0].Trim());
        //            //enmSmsResponse = (SmsResponse)Convert.ToInt32(arrResponse[0].Trim());
        //            if (enmSmsResponse == SmsResponse.MessagesSubmittedSuccessfully)
        //            {
        //                //numberOfSentSMS = Convert.ToInt32(arrResponse[1].Trim());
        //                MsgId = arrResponse[1].Trim();
        //                numberOfSentSMS = 1;
        //                try
        //                {
        //                    using (EConnectContext context = new EConnectContext())
        //                    {
        //                        if (_smsServiceType == SmsServiceType.SignleSMS)
        //                        {
        //                            sentSms = new SentSMS();
        //                            if (_message.Length > 500)
        //                                _message = _message.Substring(0, 500);
        //                            sentSms.Message = _message;
        //                            sentSms.IsManuallySent = _isManuallySent;
        //                            sentSms.MobileNumber = Convert.ToInt64(_mobileNumbers);
        //                            sentSms.Response = enmSmsResponse;
        //                            sentSms.SentOn = DateTime.Now;
        //                            sentSms.MsgId = MsgId;
        //                            if (userID != 0)
        //                                sentSms.SentByUserID = userID;
        //                            context.SentSMS.Add(sentSms);

        //                        }
        //                        else if (_smsServiceType == SmsServiceType.BulkSMS)
        //                        {
        //                            string[] numbers = _mobileNumbers.Split(',');
        //                            StringBuilder sb = new StringBuilder();
        //                            foreach (String number in numbers)
        //                            {
        //                                sentSms = new SentSMS();
        //                                if (_message.Length > 500)
        //                                    _message = _message.Substring(0, 500);
        //                                sentSms.IsManuallySent = _isManuallySent;
        //                                sentSms.Message = _message;
        //                                sentSms.MobileNumber = Convert.ToInt64(number);
        //                                sentSms.Response = enmSmsResponse;
        //                                sentSms.SentOn = DateTime.Now;
        //                                sentSms.MsgId = MsgId;
        //                                if (userID != 0)
        //                                    sentSms.SentByUserID = userID;
        //                                context.SentSMS.Add(sentSms);
        //                            }
        //                        }
        //                        context.SaveChanges();
        //                    };
        //                }
        //                catch (Exception ex)
        //                {
        //                }
        //            }
        //        }
        //        else
        //        {
        //            enmSmsResponse = (SmsResponse)Convert.ToInt32(responseFromServer.Trim());
        //        }
        //        reader.Close();
        //        dataStream.Close();
        //        dataStream.Dispose();
        //        response.Close();
        //        return enmSmsResponse;
        //    }
        //    catch (Exception ex)
        //    {
        //        using (EConnectContext context = new EConnectContext())
        //        {
        //            var emailAddress = context.Organizations.Find(1).EmailTechnicalPerson;
        //            if (emailAddress != null)
        //            {
        //                EConnect.NIELIT.Email mail = new Email("SMS sending failure", "Dear Admin,SMS could not be sent to " + _mobileNumbers.ToString() + " on " + DateTime.Now.ToString() + ".<BR>Reason:" + ex.Message, emailAddress);
        //                mail.Send();
        //            }
        //        };
        //        throw ex;
        //    }
        //} 
        #endregion

        /// <summary>
        /// Method for sending single SMS.
        /// </summary>
        /// <param name="username"> Registered user name</param>
        /// <param name="password"> Valid login password</param>
        /// <param name="senderid">Sender ID </param>
        /// <param name="mobileNo"> valid Single Mobile Number </param>
        /// <param name="message">Message Content </param>
        /// <param name="secureKey">Department generate key by login to services portal</param>
        public void sendSingleSMS(out Int32 numberOfSentSMS)
        {
            NIELIT.SmsResponse enmSmsResponse = SmsResponse.MessagesSubmittedSuccessfully;
            numberOfSentSMS = 0;
            SentSMS sentSms;
            ValidateSMSParameters();
            Stream dataStream;
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //modified as per doc
            //| SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            
           // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
            request.ProtocolVersion = HttpVersion.Version10;
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 1;            
            ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
            request.Method = "POST";
            System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();

            String encryptedPassword = encryptedPasswod(_password);
            String NewsecureKey = hashGenerator(_userName, _senderID, _message, _SecureKey);
            String smsservicetype = "singlemsg"; //For single message.

            String query = "username=" + HttpUtility.UrlEncode(_userName) +
            "&password=" + HttpUtility.UrlEncode(encryptedPassword) +
            "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
            "&content=" + HttpUtility.UrlEncode(_message) +
            "&mobileno=" + HttpUtility.UrlEncode(_mobileNumbers) +
            "&senderid=" + HttpUtility.UrlEncode(_senderID) +
            "&key=" + HttpUtility.UrlEncode(NewsecureKey)+
             "&templateid=" + HttpUtility.UrlEncode(_templateId.Trim());

            //Added templateid for SMS

            byte[] byteArray = Encoding.ASCII.GetBytes(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            String Status = ((HttpWebResponse)response).StatusDescription;
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            String responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            
            if (responseFromServer.Contains(","))
            {
                String[] arrResponse = responseFromServer.Split(',');
                if (arrResponse[0].Length > 3)
                { enmSmsResponse = (SmsResponse)Convert.ToInt32(arrResponse[0].Substring(0, 3)); }
                else
                { enmSmsResponse = (SmsResponse)Convert.ToInt32(arrResponse[0].Trim()); }

                if (enmSmsResponse == SmsResponse.MessagesSubmittedSuccessfully)
                {
                    MsgId = arrResponse[1].Trim();
                    numberOfSentSMS = 1;
                    try
                    {
                        using (EConnectContext context = new EConnectContext())
                        {
                            if (_smsServiceType == SmsServiceType.SignleSMS)
                            {
                                sentSms = new SentSMS();
                                if (_message.Length > 500)
                                    _message = _message.Substring(0, 500);
                                sentSms.Message = _message;
                                sentSms.IsManuallySent = _isManuallySent;
                                sentSms.MobileNumber = Convert.ToInt64(_mobileNumbers);
                                sentSms.Response = enmSmsResponse;
                                sentSms.SentOn = DateTime.Now;
                                sentSms.MsgId = MsgId;
                                if (HttpContext.Current.Session["UserID"] != null)
                                { sentSms.SentByUserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]); }
                                context.SentSMS.Add(sentSms);
                            }
                            context.SaveChanges();
                        };
                    }
                    catch (Exception){}
                }
            }
            else
            {
                enmSmsResponse = (SmsResponse)Convert.ToInt32(responseFromServer.Trim());
            }
        }

        /// <summary>
        /// Method for sending OTP MSG.
        /// </summary>
        /// <param name="username"> Registered user name</param>
        /// <param name="password"> Valid login password</param>
        /// <param name="senderid">Sender ID </param>
        /// <param name="mobileNo"> valid single Mobile Number </param>
        /// <param name="message">Message Content </param>
        /// <param name="secureKey">Department generate key by login to services portal</param>
        public void sendOTPMSG(out Int32 numberOfSentSMS)
        {
            NIELIT.SmsResponse enmSmsResponse = SmsResponse.MessagesSubmittedSuccessfully;
            numberOfSentSMS = 0;
            SentSMS sentSms;
            ValidateSMSParameters();
            try
            {
                Stream dataStream;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
                request.ProtocolVersion = HttpVersion.Version10;
                request.KeepAlive = false;
                request.ServicePoint.ConnectionLimit = 1;                
                ((HttpWebRequest)request).UserAgent = "Mozilla/4.0(compatible; MSIE 5.0; Windows 98; DigExt)";
                request.Method = "POST";
                System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();

                String encryptedPassword = encryptedPasswod(_password);
                String key = hashGenerator(_userName, _senderID, _message, _SecureKey);
                String smsservicetype = "otpmsg"; //For OTP message.
                String query = "username=" + HttpUtility.UrlEncode(_userName) +
                "&password=" + HttpUtility.UrlEncode(encryptedPassword) +
                "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
                "&content=" + HttpUtility.UrlEncode(_message) +
                "&mobileno=" + HttpUtility.UrlEncode(_mobileNumbers) +
                "&senderid=" + HttpUtility.UrlEncode(_senderID) +
                "&key=" + HttpUtility.UrlEncode(key)+
                 "&templateid=" + HttpUtility.UrlEncode(_templateId.Trim());

                //Added templateid for SMS
                byte[] byteArray = Encoding.ASCII.GetBytes(query);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;                
                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                String Status = ((HttpWebResponse)response).StatusDescription;
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                String responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                
                if (responseFromServer.Contains(","))
                {
                    String[] arrResponse = responseFromServer.Split(',');
                    if (arrResponse[0].Length > 3)
                    { enmSmsResponse = (SmsResponse)Convert.ToInt32(arrResponse[0].Substring(0, 3)); }
                    else
                    { enmSmsResponse = (SmsResponse)Convert.ToInt32(arrResponse[0].Trim()); }

                    if (enmSmsResponse == SmsResponse.MessagesSubmittedSuccessfully)
                    {
                        MsgId = arrResponse[1].Trim();
                        numberOfSentSMS = 1;
                        try
                        {
                            using (EConnectContext context = new EConnectContext())
                            {
                                sentSms = new SentSMS();
                                if (_message.Length > 500)
                                    _message = _message.Substring(0, 500);
                                sentSms.Message = _message;
                                sentSms.IsManuallySent = _isManuallySent;
                                sentSms.MobileNumber = Convert.ToInt64(_mobileNumbers);
                                sentSms.Response = enmSmsResponse;
                                sentSms.SentOn = DateTime.Now;
                                sentSms.MsgId = MsgId;
                                if (HttpContext.Current.Session["UserID"] != null)
                                { sentSms.SentByUserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]); }
                                context.SentSMS.Add(sentSms);

                                context.SaveChanges();
                            };
                        }
                        catch (Exception ex) { }
                    }
                }
                else
                {
                    enmSmsResponse = (SmsResponse)Convert.ToInt32(responseFromServer.Trim());
                }
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// Method for sending bulk SMS.
        /// </summary>
        /// <param name="username"> Registered user name</param>
        /// <param name="password"> Valid login password</param>
        /// <param name="senderid">Sender ID </param>
        /// <param name="mobileNo"> valid Mobile Numbers </param>
        /// <param name="message">Message Content </param>
        /// <param name="secureKey">Department generate key by login to services portal</param>
        public void sendBulkSMS(out Int32 numberOfSentSMS)
        {
            NIELIT.SmsResponse enmSmsResponse = SmsResponse.MessagesSubmittedSuccessfully;
            numberOfSentSMS = 0;
            //SentSMS sentSms;
            ValidateSMSParameters();

            Stream dataStream;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
            request.ProtocolVersion = HttpVersion.Version10;
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 1;
            //((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
            ((HttpWebRequest)request).UserAgent = "Mozilla/4.0(compatible; MSIE 5.0; Windows 98; DigExt)";
            request.Method = "POST";
            System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();
            String encryptedPassword = encryptedPasswod(_password);
            String NewsecureKey = hashGenerator(_userName, _senderID, _message, _SecureKey);
            Console.Write(NewsecureKey);
            Console.Write(encryptedPassword);
            String smsservicetype = "bulkmsg"; // for bulk msg
            String query = "username=" + HttpUtility.UrlEncode(_userName) +
            "&password=" + HttpUtility.UrlEncode(encryptedPassword) +
            "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
            "&content=" + HttpUtility.UrlEncode(_message) +
            "&bulkmobno=" + HttpUtility.UrlEncode(_mobileNumbers) +
            "&senderid=" + HttpUtility.UrlEncode(_senderID) +
            "&key=" + HttpUtility.UrlEncode(NewsecureKey)+
             "&templateid=" + HttpUtility.UrlEncode(_templateId.Trim());

            //Added templateid for SMS
            Console.Write(query);
            byte[] byteArray = Encoding.ASCII.GetBytes(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            String Status = ((HttpWebResponse)response).StatusDescription;
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            String responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            //return responseFromServer;
            if (responseFromServer.Contains(","))
            {
                String[] arrResponse = responseFromServer.Split(',');
                if (arrResponse[0].Length > 3)
                { enmSmsResponse = (SmsResponse)Convert.ToInt32(arrResponse[0].Substring(0, 3)); }
                else
                { enmSmsResponse = (SmsResponse)Convert.ToInt32(arrResponse[0].Trim()); }

                if (enmSmsResponse == SmsResponse.MessagesSubmittedSuccessfully)
                {
                    MsgId = arrResponse[1].Trim();
                    numberOfSentSMS = 1;
                    //try
                    //{
                    //    using (EConnectContext context = new EConnectContext())
                    //    {
                    //        sentSms = new SentSMS();
                    //        if (_message.Length > 500)
                    //            _message = _message.Substring(0, 500);
                    //        sentSms.Message = _message;
                    //        sentSms.IsManuallySent = _isManuallySent;
                    //        sentSms.MobileNumber = Convert.ToInt64(_mobileNumbers);
                    //        sentSms.Response = enmSmsResponse;
                    //        sentSms.SentOn = DateTime.Now;
                    //        sentSms.MsgId = MsgId;
                    //        if (HttpContext.Current.Session["UserID"] != null)
                    //            sentSms.SentByUserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                    //        context.SentSMS.Add(sentSms);

                    //        context.SaveChanges();
                    //    };
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                }
            }
            else
            {
                enmSmsResponse = (SmsResponse)Convert.ToInt32(responseFromServer.Trim());
            }
        }

        /// <summary>
        /// method for Sending unicode..
        /// </summary>
        /// <param name="username"> Registered user name</param>
        /// <param name="password"> Valid login password</param>
        /// <param name="senderid">Sender ID </param>
        /// <param name="mobileNo"> valid Mobile Numbers </param>
        /// <param name="Unicodemessage">Unicodemessage Message Content</param>
        /// <param name="secureKey">Department generate key by login to services portal</param>
        public void sendUnicodeSMS(out Int32 numberOfSentSMS)
        {
            NIELIT.SmsResponse enmSmsResponse = SmsResponse.MessagesSubmittedSuccessfully;
            numberOfSentSMS = 0;
            SentSMS sentSms;
            ValidateSMSParameters();

            Stream dataStream;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);
            request.ProtocolVersion = HttpVersion.Version10;
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 1;            
            ((HttpWebRequest)request).UserAgent = "Mozilla/4.0(compatible; MSIE 5.0; Windows 98; DigExt)";
            request.Method = "POST";
            System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();
            String U_Convertedmessage = "";
            foreach (char c in _message)
            {
                int j = (int)c;
                String sss = "&#" + j + ";";
                U_Convertedmessage = U_Convertedmessage + sss;
            }
            String encryptedPassword = encryptedPasswod(_password);
            String NewsecureKey = hashGenerator(_userName, _senderID, U_Convertedmessage, _SecureKey);
            String smsservicetype = "unicodemsg"; // for unicode msg
            String query = "username=" + HttpUtility.UrlEncode(_userName) +
            "&password=" + HttpUtility.UrlEncode(encryptedPassword) +
            "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
            "&content=" + HttpUtility.UrlEncode(U_Convertedmessage) +
            "&bulkmobno=" + HttpUtility.UrlEncode(_mobileNumbers) +
            "&senderid=" + HttpUtility.UrlEncode(_senderID) +
            "&key=" + HttpUtility.UrlEncode(NewsecureKey)+
             "&templateid=" + HttpUtility.UrlEncode(_templateId.Trim());

            //Added templateid for SMS
            byte[] byteArray = Encoding.ASCII.GetBytes(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            String Status = ((HttpWebResponse)response).StatusDescription;
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            String responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            
            if (responseFromServer.Contains(","))
            {
                String[] arrResponse = responseFromServer.Split(',');
                if (arrResponse[0].Length > 3)
                { enmSmsResponse = (SmsResponse)Convert.ToInt32(arrResponse[0].Substring(0, 3)); }
                else
                { enmSmsResponse = (SmsResponse)Convert.ToInt32(arrResponse[0].Trim()); }

                if (enmSmsResponse == SmsResponse.MessagesSubmittedSuccessfully)
                {
                    MsgId = arrResponse[1].Trim();
                    numberOfSentSMS = 1;
                    try
                    {
                        using (EConnectContext context = new EConnectContext())
                        {
                            sentSms = new SentSMS();
                            if (_message.Length > 500)
                                _message = _message.Substring(0, 500);
                            sentSms.Message = _message;
                            sentSms.IsManuallySent = _isManuallySent;
                            sentSms.MobileNumber = Convert.ToInt64(_mobileNumbers);
                            sentSms.Response = enmSmsResponse;
                            sentSms.SentOn = DateTime.Now;
                            sentSms.MsgId = MsgId;
                            if (HttpContext.Current.Session["UserID"] != null)
                                sentSms.SentByUserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                            context.SentSMS.Add(sentSms);

                            context.SaveChanges();
                        };
                    }
                    catch (Exception) { }	
                }
            }
            else
            {
                enmSmsResponse = (SmsResponse)Convert.ToInt32(responseFromServer.Trim());
            }
        }

        /// <summary>
        /// Method to get Encrypted the password
        /// </summary>
        /// <param name="password"> password as String"</param>
        protected String encryptedPasswod(String password)
        {
            byte[] encPwd = Encoding.UTF8.GetBytes(password);
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA1");
            byte[] pp = sha1.ComputeHash(encPwd);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in pp)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Method to Generate hash code
        /// </summary>
        /// <param name= "secure_key">your last generated Secure_key </param>
        protected String hashGenerator(String Username, String sender_id, String message, String secure_key)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Username).Append(sender_id).Append(message).Append(secure_key);
            byte[] genkey = Encoding.UTF8.GetBytes(sb.ToString());
            HashAlgorithm sha1 = HashAlgorithm.Create("SHA512");
            byte[] sec_key = sha1.ComputeHash(genkey);
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < sec_key.Length; i++)
            {
                sb1.Append(sec_key[i].ToString("x2"));
            }
            return sb1.ToString();
        }
    }
    class MyPolicy : ICertificatePolicy
    {
        public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem)
        {
            return true;
        }
    }

}
