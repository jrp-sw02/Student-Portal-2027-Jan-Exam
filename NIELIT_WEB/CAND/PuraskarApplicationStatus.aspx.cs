using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class PuraskarApplicationStatus : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Lblerror.Text = "";
            if (Request.UrlReferrer == null)
			//	if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in" ) && (Request.UrlReferrer == null  || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "http://nielit.gov.in"))
            {
                Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
                Response.End();
                return;
            }
            if (!Page.IsPostBack)
            {               
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                Lblerror.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GenerateNewCaptchaImage();
            txtcode.Text = "";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void GenerateNewCaptchaImage()
    {
        try
        {
            ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateRandomNumber(6);
            HfCaptcha.Value = ViewState["CaptchCode"].ToString();
            EConnect.CaptchaImage captcha = new CaptchaImage(HfCaptcha.Value, 200, 50, "Century Schoolbook");
            imgcap.Src = captcha.ImageSource;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void Btnview_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();          
            Int32 paymentPending = Convert.ToInt32(enmPaymentStatus.Pending);
            StringBuilder Paymentname = new StringBuilder();
            StringBuilder deficiencyname = new StringBuilder();
            if (txtcode.Text == ViewState["CaptchCode"].ToString())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    string applID = TxtAppno.Text.ToString();
                    DateTime DOB = Convert.ToDateTime(TxtDOB.Text.ToString());
                    PuraskarApplicationForm app = new PuraskarApplicationForm();
                    var application = (from a in context.PuraskarApplicationForms
                                       join b in context.Exams on a.ExamID equals b.ID
                                       join c in context.CourseRegistrationApplications on a.CandidateID equals c.CandidateID
                                       where a.OnlineRefNo.Equals(applID, StringComparison.OrdinalIgnoreCase) && c.DateOfBirth == DOB && a.finalSubmit == true
                                       select new
                                       {
                                           appdate = a.Final_Submission_Date,
                                           appno = a.ID,
                                           Appnumber = a.OnlineRefNo,
                                           name = a.Name,
                                           fname = c.FatherName,
                                           mname = c.MotherName,
                                           gname = c.GuardianName,
                                           photo = c.Photo,
                                           salutation = c.Salutation,
                                           status = a.applicationStatusID,
                                           institutename = c.Institute.Name + "(" + c.Institute.AccreditationDetails.FirstOrDefault().AccreditationNumber + ")",
                                           instituteVerifiedOn = a.VerifiedInsttOn,
                                           examVerifiedOn = a.VerifiedExamOn,
                                           financeVerifiedOn = a.VerifiedByFinanceOn,
                                           insttRejectionReason = a.InsttRejectionReason,
                                           examRejectionReason = a.examRejectionReason,
                                           financeRejectionreason = a.financeRejectionReason,
                                           course = c.CourseID,
                                           examCycle = b.Name


                                       }).FirstOrDefault();

                    if (application != null)
                    {
                        divfilter.Visible = false;
                        divStatus.Visible = true;

                        Course coursename = context.Courses.Find(Convert.ToInt32(application.course));
                        if (coursename != null)
                            Lblcourse.Text = GetInitCap("STATUS OF PURASKAR APPLICATION FOR") + " " + GetInitCap(coursename.Name);

                        LblAppdate.Text = application.appdate.Value.ToString("dd/MMM/yyyy");
                        LblAppno.Text = application.Appnumber.ToString();
                        LblDOB.Text = TxtDOB.Text;
                        Lblname.Text = application.salutation + " " + GetInitCap(application.name);
                        if (string.IsNullOrEmpty(application.gname) == true && string.IsNullOrWhiteSpace(application.gname) == true)
                        {
                            trfathername.Visible = true;
                            trmothername.Visible = true;
                            trgname.Visible = false;
                            if (string.IsNullOrEmpty(application.fname) == false && !string.IsNullOrWhiteSpace(application.fname))
                                Lbfname.Text = "Mr. " + GetInitCap(application.fname);
                            else
                                Lbfname.Text = "NA";
                            if (string.IsNullOrEmpty(application.mname) == false && string.IsNullOrWhiteSpace(application.mname) == false)
                                Lbmname.Text = "Mrs. " + GetInitCap(application.mname);
                            else
                                Lbmname.Text = "NA";
                        }
                        else
                        {
                            Lgname.Text = string.IsNullOrEmpty(application.gname) == false && string.IsNullOrWhiteSpace(application.gname) == false ? GetInitCap(application.gname) : "NA";
                            trfathername.Visible = false;
                            trmothername.Visible = false;
                            trgname.Visible = true;
                        }
                        imgcandphoto.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])application.photo);
                        Lblecycle.Text = application.examCycle;
                        enmPuraskarApplicationStatus applStatus = (enmPuraskarApplicationStatus)application.status;
                        if (applStatus == enmPuraskarApplicationStatus.AppliedByCandidate || applStatus == enmPuraskarApplicationStatus.PaymentTransferredToBankAccount || applStatus == enmPuraskarApplicationStatus.PaymentReleasedToBank || applStatus == enmPuraskarApplicationStatus.PaymentRejectedByBank)
                        {
                            Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);

                        }
                        else if (applStatus == enmPuraskarApplicationStatus.VerifiedByInstituteButNIELITVerificationPending)
                        {
                            Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                            Lblss.Text += ". Verfication Date :" + application.instituteVerifiedOn.Value.ToString("dd-MMM-yyyy");

                        }
                        else if (applStatus == enmPuraskarApplicationStatus.VerifiedByExamWingButFinanceWingVerificationPending)
                        {
                            Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                            Lblss.Text += ". Verfication Date :" + application.examVerifiedOn.Value.ToString("dd-MMM-yyyy");

                        }
                        else if (applStatus == enmPuraskarApplicationStatus.VerifiedByFinanceWingButPaymentToBeProcessed)
                        {
                            Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                            Lblss.Text += ". Verfication Date :" + application.financeVerifiedOn.Value.ToString("dd-MMM-yyyy");

                        }

                        else if (applStatus == enmPuraskarApplicationStatus.RejectedByInstitute || applStatus == enmPuraskarApplicationStatus.RejectedByExamWing || applStatus == enmPuraskarApplicationStatus.RejectedByFinanceWing)
                        {
                            Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                            if (application.examRejectionReason != null)
                                Lblss.Text += " Reason :" + application.examRejectionReason;
                            if (application.insttRejectionReason != null)
                                Lblss.Text += " Reason :" + application.insttRejectionReason;
                            if (application.financeRejectionreason != null)
                                Lblss.Text += " Reason :" + application.financeRejectionreason;
                        }

                        trexcycle.Visible = true;
                        trcstatus.BgColor = "#E6F0F0";
                        Lblerror.Visible = false;
                    }
                    else
                    {
                        Lblerror.Text = "Invalid Application No. or Date of Birth or form not final submitted";
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                    }


                    if (!String.IsNullOrEmpty(Lblerror.Text))
                        Lblerror.Visible = true;
                }
            }
            else
            {
                Lblerror.Text = "Invalid Captcha Code";
                GenerateNewCaptchaImage();
                txtcode.Text = "";
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            GenerateNewCaptchaImage();
            txtcode.Text = "";
            Lblerror.Visible = false;
            TxtAppno.Text = "";
            TxtDOB.Text = "";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void Btnpback_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Response.Redirect("../CAND/PuraskarApplicationStatus.aspx?" + Request.QueryString);
            GenerateNewCaptchaImage();
            txtcode.Text = "";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
   
}