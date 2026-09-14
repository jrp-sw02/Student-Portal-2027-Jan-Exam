using System;
using System.Data;
using System.Linq;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class CAND_OTPprocess : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    string verify = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try 
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(1);
            lblError.Visible = false;
            if (IsSessionAlive() == false)
                Response.Redirect("../Home.aspx");
            loginUserType = (UserType)Session["UserType"];
            entityID =  Convert.ToInt64(Session["EntityID"]);
            
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["verify"])))
                {
                    verify = Convert.ToString(Request.QueryString["verify"]);
                    
                }
                else
                {
                    ShowAlert("Invalid request");
                    BtnValidate.Visible = false;
                    BtnResend.Visible = false;
                }
                ShowMessage();
            }       
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ShowMessage()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                MultiView1.ActiveViewIndex = 0;
                var mobile = context.CandidateContactDetails.Where(s => s.CandidateID == entityID).FirstOrDefault();
                if (verify == "email")
                {
                    lblHeading.Text = "Email Address Verification";
                    LblOTP.Text = "Email Address: " + mobile.EmailAddress.ToString();
                    LblOTP.Text += " on " + mobile.EmailOTPValidUptoDate.Value.ToString("dd-MMM-yyyy hh:mm tt");
                    lblRefNo.Text = "(OTP Reference Number: " + mobile.MobileOTPValidUptoDate.Value.ToString("hhmmss") + ")";
                    trmessage.Visible = false;
                }
                else
                {
                    string str = "";
                    if (mobile.MobileNumber.HasValue && mobile.MobileNumber.Value.ToString().Length == 10)
                    {
                        string suffix = mobile.MobileNumber.ToString().Substring(mobile.MobileNumber.ToString().Length - 4, 4);
                        str = string.Format("XXXXXX{0}", suffix);
                    }
                    lblHeading.Text = "Mobile Number Verification";
                    LblOTP.Text = "mobile number : " + str;
                    LblOTP.Text += " on " + mobile.MobileOTPValidUptoDate.Value.ToString("dd-MMM-yyyy hh:mm tt");
                    lblRefNo.Text = "(OTP Reference Number: " + mobile.MobileOTPValidUptoDate.Value.ToString("hhmmss") + ")";
                    trmessage.Visible = true;
                    var reg = context.RegistrationDetails.Where(q => q.CandidateID == entityID).OrderByDescending(q => q.CourseID).FirstOrDefault();
                    if (reg != null)
                        lbmessage.Text = "<b>Note:-</b> If you have not recieved any OTP through sms on your mobile number or sms service is blocked in your region due to some security reasons, you can verify your mobile number by sending sms <br/> <b>NIELIT VM " + reg.RegistrationNo + " to 51969 </b>";
                }
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(lblHeading.Text, "CAND/OTPprocess.aspx?verify=" + Convert.ToString(Request.QueryString["verify"]), ""));

            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BtnValidate_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(TxtOTP.Text.Trim()) == false)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["verify"])))
                    verify = Convert.ToString(Request.QueryString["verify"]);
                else
                    verify = "0";

                if (verify == "email")
                    VerifyEmail();
                else if (verify == "mobile")
                    VerifyMobile();
            }
            else
                lblError.Text = "OTP cannot be Blank..!";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void VerifyMobile()
    {
        try 
        {
            using (EConnectContext context = new EConnectContext())
            {
                var  mobile = context.CandidateContactDetails.Where( s => s.CandidateID == entityID).FirstOrDefault();
                if (Convert.ToInt32(TxtOTP.Text.Trim()) == mobile.MobileOTP)
                {
                    TxtOTP.Text = "";
                    mobile.IsMobileNumberVerified = true;
                    mobile.MobileNumberVerifiedOn = DateTime.Now;
                    mobile.MobileOTPValidUptoDate = DateTime.Now;
                    context.Entry(mobile).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    LblOTPMsg.Text = "Mobile Number";
                    MultiView1.ActiveViewIndex = 1;
                    BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Mobile Number Verification", "#", ""));          
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Entered OTP does not match with the OTP sent to your Registered Mobile Number. Please check and re-enter OTP.";
                    MultiView1.ActiveViewIndex = 0;                   
                }
                
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void VerifyEmail()
    {
        try
        {
            BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Email Address Verification", "#", ""));
            using (EConnectContext context = new EConnectContext())
            {
                var email = context.CandidateContactDetails.Where(s => s.CandidateID == entityID).FirstOrDefault();
                
                    if (Convert.ToInt32(TxtOTP.Text.Trim()) == email.EmailOTP)
                    {
                        TxtOTP.Text = "";
                        email.IsEmailVerified = true;
                        email.EmailVerifiedOn = DateTime.Now;
                        email.EmailOTPValidUptoDate = DateTime.Now;
                        context.Entry(email).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        LblOTPMsg.Text = "Email Address";
                        LblOTPCaption.Text = "Email Address";
                        MultiView1.ActiveViewIndex = 1;
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Entered OTP does not match with the OTP sent to your Registered Email Address. Please check and re-enter OTP.";
                        MultiView1.ActiveViewIndex = 0;
                    }
                    
                    
            };

        }
        catch (Exception ex)
        {
            throw ex;

        }

    }
    protected void BtnValidateCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/CAND/myprofile.aspx");
    }
    protected void BtnResend_Click(object sender, EventArgs e)
    {
        try
        {
            string str="";
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["verify"])))            
                verify = Convert.ToString(Request.QueryString["verify"]);
            if (verify == "email")
            {
                CommonFunctions.GenerateEmailOTP(UserType.Candidate, entityID, true);
            }
            else if (verify == "mobile")
            {
                CommonFunctions.GenerateMobileOTP(UserType.Candidate, entityID, true);
            }
            using (EConnectContext context = new EConnectContext())
            {
                CandidateContactDetail contact = context.CandidateContactDetails.Where(c => c.CandidateID == entityID).FirstOrDefault();
                if (verify == "email")
                {
                    str = "email adddress " + contact.EmailAddress;
                }
                else if (verify == "mobile")
                {
                    str = "mobile number " + contact.MobileNumber.ToString();
                }
            }
            lblError.Visible = true;
            lblError.Text = "New OTP has been sent to your  " + str;
            ShowMessage();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}