using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class sms_vm : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;

    protected void Page_Load(object sender, EventArgs e)
    {
        //https://student.nielit.gov.in/sms/vm.aspx?mobileNumber=9560974252&timeStamp=434&operatorName=sfssf&areaCode=fsadfd&message=sfisdfjsffjsfjk
        //mobileNumber
        //timeStamp
        //operatorName
        //message = NIELIT VM 123456
        try
        {
            context = new EConnectContext();
            if (Request.QueryString.Keys.Count > 0)
            {
                if (!Page.IsPostBack)
                {
                    //Response.Write(Request.QueryString.ToString());
                    //Response.End();
                    //string PagePath = "Activation.aspx?p1=" + 10 + "&p2=" + "kuldeep" + "&p3=" + 34;
                    // Response.Write(Request.Url.ToString().Substring(0, Request.Url.ToString().LastIndexOf('/')) + "/" + PagePath);
                    if (!String.IsNullOrEmpty(Request.QueryString["mobileNumber"].ToString()) && !String.IsNullOrEmpty(Request.QueryString["message"]))
                    {
                        string senddata = Request.QueryString.ToString();
                        ReceivedSms rs;
                        Int64 mobNumber = 0;
                        if (IsNumeric(Request.QueryString["mobileNumber"].ToString()))
                        {
                            mobNumber = Convert.ToInt64(Request.QueryString["mobileNumber"]);
                        }
                        else
                        {
                            throw new Exception("Invalid Mobile Number");
                        }
                        string timeStamp = Request.QueryString["timeStamp"];
                        string opName = Request.QueryString["operatorName"];
                        string AreaCode = Request.QueryString["areacode"];
                        string msg = Request.QueryString["message"].ToString().ToUpper();
                        string[] msgdata = msg.Split(' ');
                        if (msgdata.Count() == 3)
                        {
                            string msgNIELIT = msgdata[0].ToString();
                            string msgVM = msgdata[1].ToString();
                            Int64 Regno = 0;
                            if (IsNumeric(msgdata[2]))
                                Regno = Convert.ToInt64(msgdata[2]);
                            else
                            {
                                rs = new ReceivedSms();
                                rs.MobileNumber = mobNumber;
                                rs.Message = msg;
                                rs.IsValid = false;
                                rs.RegistrationNumber = Regno;
                                rs.RequestData = senddata;
                                //rs.SentEmailId = SendEmailID.ID;
                                rs.CreatedOn = DateTime.Now;
                                context.ReceivedSmss.Add(rs);
                                context.SaveChanges();
                                throw new Exception("Invalid Registration Number");
                            }
                            if (msgNIELIT == "NIELIT" && msgVM == "VM" && Regno > 0)
                            {
                                //using (EConnectContext context = new EConnectContext())
                                //{
                                    Int64 CandidateID = (from c in context.RegistrationDetails
                                                         where c.RegistrationNo == Regno
                                                         select c.CandidateID).FirstOrDefault();
                                    if (CandidateID != null)
                                    {
                                        var candidateContact = (from cc in context.CandidateContactDetails
                                                                where cc.CandidateID == CandidateID
                                                                select cc).FirstOrDefault();
                                        if (candidateContact != null)
                                        {
                                            if (candidateContact.MobileNumber.HasValue)
                                            {
                                                if (candidateContact.MobileNumber.Value == mobNumber)
                                                {
                                                    if (candidateContact.IsMobileNumberVerified == false)
                                                    {
                                                        candidateContact.IsMobileNumberVerified = true;
                                                        candidateContact.MobileNumberVerifiedOn = DateTime.Now;
                                                        if (!string.IsNullOrWhiteSpace(candidateContact.EmailAddress))
                                                        {
                                                            string emailAddress = null;
                                                            string Emailmsg = "Dear Candidate, Your mobile number " + mobNumber.ToString() + " has been verified successfully on " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm") + ". <br><br>Regards<Br>NIELIT Team";
                                                            emailAddress = candidateContact.EmailAddress;
                                                            EConnect.NIELIT.Email mail = new Email("NIELIT: Mobile Number Verification", Emailmsg, emailAddress.ToString().Trim(), true);
                                                            try
                                                            {
                                                                mail.Send();
                                                                var SendEmailID = (from s in context.SentEmails
                                                                                   where s.EmailAddress.ToUpper() == emailAddress.ToUpper()
                                                                                   orderby s.ID descending
                                                                                   select s).FirstOrDefault();
                                                                if (SendEmailID != null)
                                                                {
                                                                    rs = new ReceivedSms();
                                                                    rs.MobileNumber = mobNumber;
                                                                    rs.Message = msg;
                                                                    rs.IsValid = true;
                                                                    rs.RegistrationNumber = Regno;
                                                                    rs.RequestData = senddata;
                                                                    rs.SentEmailId = SendEmailID.ID;
                                                                    rs.CreatedOn = DateTime.Now;
                                                                    context.ReceivedSmss.Add(rs);
                                                                    context.SaveChanges();
                                                                }
                                                                else
                                                                {
                                                                    rs = new ReceivedSms();
                                                                    rs.MobileNumber = mobNumber;
                                                                    rs.Message = msg;
                                                                    rs.IsValid = false;
                                                                    rs.RegistrationNumber = Regno;
                                                                    rs.RequestData = senddata;
                                                                    rs.SentEmailId = SendEmailID.ID;
                                                                    rs.CreatedOn = DateTime.Now;
                                                                    context.ReceivedSmss.Add(rs);
                                                                    context.SaveChanges();
                                                                    throw new Exception("Invalid Mobile Number");
                                                                }
                                                            }
                                                            catch (Exception ex)
                                                            {

                                                            }
                                                        }
                                                        context.Entry(candidateContact).State = System.Data.Entity.EntityState.Modified;
                                                        context.SaveChanges();
                                                        throw new Exception("Invalid Mobile Number");
                                                    }
                                                    else
                                                    {
                                                        rs = new ReceivedSms();
                                                        rs.MobileNumber = mobNumber;
                                                        rs.Message = msg;
                                                        rs.IsValid = false;
                                                        rs.RegistrationNumber = Regno;
                                                        rs.RequestData = senddata;
                                                        //rs.SentEmailId = SendEmailID.ID;
                                                        rs.CreatedOn = DateTime.Now;
                                                        context.ReceivedSmss.Add(rs);
                                                        context.SaveChanges();
                                                        throw new Exception("Mobile Number already verified.");
                                                    }
                                                }
                                                else
                                                {
                                                    rs = new ReceivedSms();
                                                    rs.MobileNumber = mobNumber;
                                                    rs.Message = msg;
                                                    rs.IsValid = false;
                                                    rs.RegistrationNumber = Regno;
                                                    rs.RequestData = senddata;
                                                    //rs.SentEmailId = SendEmailID.ID;
                                                    rs.CreatedOn = DateTime.Now;
                                                    context.ReceivedSmss.Add(rs);
                                                    context.SaveChanges();
                                                    throw new Exception("Invalid Mobile Number");
                                                }
                                            }
                                            else
                                            {
                                                rs = new ReceivedSms();
                                                rs.MobileNumber = mobNumber;
                                                rs.Message = msg;
                                                rs.IsValid = false;
                                                rs.RegistrationNumber = Regno;
                                                rs.RequestData = senddata;
                                                //rs.SentEmailId = SendEmailID.ID;
                                                rs.CreatedOn = DateTime.Now;
                                                context.ReceivedSmss.Add(rs);
                                                context.SaveChanges();
                                                throw new Exception("Invalid Mobile Number");
                                            }
                                        }
                                        else
                                        {
                                            rs = new ReceivedSms();
                                            rs.MobileNumber = mobNumber;
                                            rs.Message = msg;
                                            rs.IsValid = false;
                                            rs.RegistrationNumber = Regno;
                                            rs.RequestData = senddata;
                                            //rs.SentEmailId = SendEmailID.ID;
                                            rs.CreatedOn = DateTime.Now;
                                            context.ReceivedSmss.Add(rs);
                                            context.SaveChanges();
                                            throw new Exception("Invalid Registration Number");
                                        }
                                    }
                                    else
                                    {
                                        rs = new ReceivedSms();
                                        rs.MobileNumber = mobNumber;
                                        rs.Message = msg;
                                        rs.IsValid = false;
                                        rs.RegistrationNumber = Regno;
                                        rs.RequestData = senddata;
                                        //rs.SentEmailId = SendEmailID.ID;
                                        rs.CreatedOn = DateTime.Now;
                                        context.ReceivedSmss.Add(rs);
                                        context.SaveChanges();
                                        throw new Exception("Invalid Registration Number");
                                    }
                                //};
                            }
                            else
                            {
                                rs = new ReceivedSms();
                                rs.MobileNumber = mobNumber;
                                rs.Message = msg;
                                rs.IsValid = false;
                                rs.RegistrationNumber = Regno;
                                rs.RequestData = senddata;
                                //rs.SentEmailId = SendEmailID.ID;
                                rs.CreatedOn = DateTime.Now;
                                context.ReceivedSmss.Add(rs);
                                context.SaveChanges();
                                throw new Exception("Invalid Request Parameter In Message");
                            }
                        }
                        else
                        {
                            rs = new ReceivedSms();
                            rs.MobileNumber = mobNumber;
                            rs.Message = msg;
                            rs.IsValid = false;
                            //rs.RegistrationNumber = Regno;
                            rs.RequestData = senddata;
                            //rs.SentEmailId = SendEmailID.ID;
                            rs.CreatedOn = DateTime.Now;
                            context.ReceivedSmss.Add(rs);
                            context.SaveChanges();
                            throw new Exception("Invalid keyword. (NIELIT VM RegistrationNumber)");
                        }

                    }
                    else
                    {
                        throw new Exception("Invalid Request Parameters");
                    }
                }
            }
            else
            {
                throw new Exception("Invalid Request Parameters");
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            Response.End();
        }
        finally { context.Dispose(); }

    }
}