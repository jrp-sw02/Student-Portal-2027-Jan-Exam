using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class ApplicationStatus : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Lblerror.Text = "";
            //if (Request.UrlReferrer == null)
		    //if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in" ) && (Request.UrlReferrer == null  || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "https://nielit.gov.in"))
      //      {
      //          Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
      //          Response.End();
      //          return;
      //      }
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
                    RenderPage();
                    showsidelink(courseID);
                }
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
            ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateCaptchaCode(6);
            HfCaptcha.Value = ViewState["CaptchCode"].ToString();
            EConnect.CaptchaImage captcha = new CaptchaImage(HfCaptcha.Value, 200, 50, "Arial");
            imgcap.Src = captcha.ImageSource;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void RenderPage()
    {
        try
        {
            string tt = Convert.ToString(Session["ModuleID"]);
            using (EConnectContext context = new EConnectContext())
            {
                Course currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["id"]));
                if (currentCourse.enmCourseType == enmCourseType.CertificationCourse && (Request.QueryString["type"] == Convert.ToString(3)))
                {
                    if (Request.UrlReferrer.ToString().ToLower().Contains("aboutcourse.aspx"))
                    { BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Check Application Status", "WEB/ApplicationStatus.aspx?" + Request.QueryString, "")); }
                    else
                    { BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Check Application Status", "WEB/ApplicationStatus.aspx?" + Request.QueryString, "")); }
                    lblheading.Text = "Check Application Status";
                    Lblsubheading.Text = "Course Name";
                    Lblcname.Text = currentCourse.Name;

                    Sidelink.Items.Add(new SideLinkItem("View Filled Application", "FilledForm.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/Get_Filled_Form.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Download Admit Card", "DownloadAdmitCard.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/Print_Admit_Card.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Result", "Result.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/View_Result.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Course Status", "CCStatus.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/View_Certificate_Status.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Search Accredited Centre", "FrmAccredetedCentre.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/View_Certificate_Status.jpg"));
                    Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                    Sidelink.Render();


                }
                else if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    if (Request.UrlReferrer.ToString().ToLower().Contains("aboutcourse.aspx"))
                    { BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Check Application Status", "WEB/ApplicationStatus.aspx?ID=" + Request.QueryString["id"], "")); }
                    else
                    { BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Check Application Status", "WEB/ApplicationStatus.aspx?ID=" + Request.QueryString["id"], "")); }
                    lblheading.Text = "Check Application Status";
                    Lblsubheading.Text = "Course Name";
                    Lblcname.Text = currentCourse.Name;
                    Sidelink.Items.Add(new SideLinkItem("Apply Online", "RulesForOnlineRegistration.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/Apply_Online.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Filled Application", "FilledForm.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/Get_Filled_Form.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Result", "Result.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/View_Result.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Search Accredited Centre", "FrmAccredetedCentre.aspx?ID=" + Request.QueryString["id"].ToString() + "&type=" + tt + "", "../images/View_Certificate_Status.jpg"));
                    Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                    Sidelink.Render();
                }
                else
                {
                    if (Request.UrlReferrer.ToString().ToLower().Contains("aboutcourse.aspx"))
                    { BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Check Form Status", "WEB/ApplicationStatus.aspx?" + Request.QueryString, "")); }
                    else
                    { BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Check Form Status", "WEB/ApplicationStatus.aspx?" + Request.QueryString, "")); }

                    lblheading.Text = "Check Form Status";
                    Lblsubheading.Text = "Certificate Name";
                    Lblcname.Text = currentCourse.Name;
                    Sidelink.Items.Add(new SideLinkItem("Apply Online", "RulesForOnlineRegistration.aspx?" + Request.QueryString + "&type=" + tt + "", "../images/Apply_Online.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Filled Form", "FilledForm.aspx?" + Request.QueryString + "&type=" + tt + "", "../images/Get_Filled_Form.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Download Admit Card", "DownloadAdmitCard.aspx?" + Request.QueryString + "&type=" + tt + "", "../images/Print_Admit_Card.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Result", "Result.aspx?" + Request.QueryString + "&type=" + tt + "", "../images/View_Result.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Search Centre", "FrmAccredetedCentre.aspx?" + Request.QueryString + "&type=" + tt + "", "../images/View_Certificate_Status.jpg"));
                    Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                    Sidelink.Render();

                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void Btnview_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            string requesturl = "";
            Int32 paymentPending = Convert.ToInt32(enmPaymentStatus.Pending);
            StringBuilder Paymentname = new StringBuilder();
            StringBuilder deficiencyname = new StringBuilder();
            Int32 counter = 0;
            if (txtcode.Text == ViewState["CaptchCode"].ToString())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    string applID = TxtAppno.Text.ToString();
                    Int32 CourseID = Convert.ToInt32(Request.QueryString["id"].ToString());
                    DateTime DOB = Convert.ToDateTime(TxtDOB.Text.ToString());
                    Course currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["id"]));
                    if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                    {

                        Lblcourse.Text = GetInitCap("STATUS OF ONLINE REGISTRATION APPLICATION FOR") + " " + GetInitCap(currentCourse.Name);
                        Lblccname.Text = currentCourse.CourseCategory.Name + "-" + "(" + currentCourse.Name + ")";
                        CourseRegistrationApplication app = new CourseRegistrationApplication();
                        var application = (from a in context.CourseRegistrationApplications
                                           where a.Number.Equals(applID, StringComparison.OrdinalIgnoreCase) && a.DateOfBirth == DOB && a.CourseID == CourseID && a.FinalSubmitted == true
                                           select new
                                           {
                                               appdate = a.ApplicationDate,
                                               appno = a.ID,
                                               Appnumber = a.Number,
                                               dob = a.DateOfBirth,
                                               name = a.Name,
                                               fname = a.FatherName,
                                               mname = a.MotherName,
                                               gname = a.GuardianName,
                                               photo = a.Photo,
                                               salutation = a.Salutation,
                                               status = a.ApplicationStatusID,
                                               verifiedon = a.DateOfVerificationByInstitute,
                                               Demandid = a.DemandNoteID,
                                               apptypeID = a.ApplicantTypeID,
                                               apptypename = a.ApplicantType.Name,
                                               paymentstatus = a.PaymentStatusID,
                                               institutename = a.Institute.Name + "(" + a.Institute.AccreditationDetails.FirstOrDefault().AccreditationNumber + ")",
                                               Batchitemid = a.BatchItemID.HasValue ? a.BatchItemID.Value : 0
                                           }).FirstOrDefault();

                        if (application != null)
                        {
                            divfilter.Visible = false;
                            divStatus.Visible = true;
                            LblAppdate.Text = application.appdate.ToString("dd-MMM-yyyy");
                            LblAppno.Text = application.Appnumber.ToString();
                            LblDOB.Text = application.dob.ToString("dd-MMM-yyyy");
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
                            if (application.apptypeID == Convert.ToInt32(enmApplicantType.Direct))
                                Lblapptype.Text = application.apptypename;
                            else
                                Lblapptype.Text = application.apptypename + ":-" + application.institutename;
                            enmCourseApplicationStatus applStatus = (enmCourseApplicationStatus)application.status;
                            if (applStatus == enmCourseApplicationStatus.ApplicationVerifiedByInstitute)
                            {
                                Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                                Lblss.Text += " On :" + application.verifiedon.Value.ToString("dd-MMM-yyyy");
                            }
                            else if (applStatus == enmCourseApplicationStatus.AppliedButFeeNotDepositedByCandidate)
                            {
                                Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                                Lblnote.Visible = true;
                                requesturl = Request.Url.ToString();
                                var paymentTypes = (from p in context.PaymentModes
                                                    where p.Visible == true
                                                    orderby p.Name
                                                    select p).ToList();

                                foreach (var names in paymentTypes)
                                {
                                    Paymentname.Append(names.Name + ",");
                                }
                                Lblnote.InnerHtml = "Note:-Please pay your fee either through <b> " + Paymentname.ToString().TrimEnd(',') + " <b> by clicking here <a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CourseRegistrationApplication) + "&Appid=" + application.appno.ToString() + "&DemandID=" + application.Demandid.Value.ToString() + "&RU=" + requesturl) + "'>Pay Fee </a>";
                            }
                            else if (applStatus == enmCourseApplicationStatus.KeptInAbeyance && application.Batchitemid != 0)
                            {
                                Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                                Lblnote.Visible = true;
                                requesturl = Request.Url.ToString();
                                var deficiencydetails = (from p in context.BatchItemDeficiencyDetails
                                                         join t in context.DeficiencyCodes
                                                             on p.DeficiencyID equals t.ID
                                                         join c in context.BatchItems
                                                             on p.BatchItemID equals c.ID
                                                         where p.BatchItemID == application.Batchitemid && p.IsFullFilled == false
                                                         orderby p.DeficiencyID
                                                         select new
                                                         {
                                                             deficiencydesc = c.KeptInAbeyanceReason,
                                                             defficiencydetail = t.Description
                                                         }).ToList();
                                if (deficiencydetails.Count() > 0)
                                {
                                    foreach (var deficiency in deficiencydetails)
                                    {
                                        counter = counter + 1;
                                        deficiencyname.Append(counter + ". " + deficiency.defficiencydetail + "<br/>");
                                    }
                                    Lblnote.InnerHtml = "Note:-Your Application has been Kept In Abeyance due to the following deficiencies found in your application form :- <br/> " + deficiencyname.ToString() + " Remarks:- <br/>" + GetInitCap(deficiencydetails.FirstOrDefault().deficiencydesc.ToString()) + " <br/> Please fulfill the deficiencies found in your application and resend your application form back to the NIELIT alongwith the suitable documents in support of this within the 15 days , failing which your application will be rejected without prior notice.";
                                }
                            }
                            else if (applStatus == enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT && application.paymentstatus == paymentPending)
                            {
                                Lblss.Text = "Fee Pending to be Paid by Institute";
                            }
                            else
                            {
                                Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                            }
                            trexcycle.Visible = false;
                            trcstatus.BgColor = "#E6F0F0";
                            Lblerror.Visible = false;
                        }
                        else
                        {
                            Lblerror.Text = "Invalid Application No. or Date of Birth";
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                        }
                    }
                    else if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                    {

                        var application = (from a in context.CertificateExamApplications
                                           where a.Number.Equals(applID, StringComparison.OrdinalIgnoreCase) && a.CourseID == CourseID && a.FinalSubmitted == true && a.DateOfBirth == DOB
                                           select new
                                           {
                                               appdate = a.ApplicationDate,
                                               appno = a.ID,
                                               Appnumber = a.Number,
                                               dob = a.DateOfBirth,
                                               name = a.Name,
                                               fname = a.FatherName,
                                               mname = a.MotherName,
                                               gname = a.GuardianName,
                                               photo = a.Photo,
                                               salutation = a.Salutation,
                                               exam = a.Exam.Name,
                                               examcyclename = a.Exam.ExaminationCycle.Name,
                                               status = a.ApplicationStatusID,
                                               cid = a.CourseID,
                                               verifiedon = a.DateOfVerificationByInstitute,
                                               Demandid = a.DemandNoteID,
                                               apptypeID = a.ApplicantTypeID,
                                               apptypename = a.ApplicantType.Name,
                                               paymentstatus = a.PaymentStatusID,
                                               institutename = a.Institute.Name + "(" + a.Institute.AccreditationDetails.FirstOrDefault().AccreditationNumber + ")",
                                               examID = a.ExamID
                                           }).FirstOrDefault();

                        if (application != null)
                        {
                            divfilter.Visible = false;
                            divStatus.Visible = true;
                            Lblcourse.Text = GetInitCap("STATUS OF ONLINE EXAMINATION APPLICATION FOR") + " " + currentCourse.Code;
                            Lblccname.Text = currentCourse.CourseCategory.Name + ":-" + currentCourse.Name;
                            LblAppdate.Text = application.appdate.ToString("dd-MMM-yyyy");
                            LblAppno.Text = application.Appnumber.ToString();
                            LblDOB.Text = application.dob.ToString("dd-MMM-yyyy");
                            Lblname.Text = application.salutation + " " + application.name.ToUpper();
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
                            if (application.apptypeID == Convert.ToInt32(enmApplicantType.Direct))
                                Lblapptype.Text = application.apptypename;
                            else
                                Lblapptype.Text = application.apptypename + ":-" + application.institutename;
                            enmCertificateExamApplicationStatus applStatus = (enmCertificateExamApplicationStatus)application.status;
                            if (applStatus == enmCertificateExamApplicationStatus.ApplicationVerifiedByInstitute)
                            {
                                Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                                Lblss.Text += " On :" + application.verifiedon.Value.ToString("dd-MMM-yyyy");
                            }
                            else if (applStatus == enmCertificateExamApplicationStatus.AppliedButFeeNotDepositedByCandidate)
                            {
                                Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                                Lblnote.Visible = true;
                                requesturl = Request.Url.ToString();
                                var paymentTypes = (from p in context.PaymentModes
                                                    where p.Visible == true
                                                    orderby p.Name
                                                    select p).ToList();

                                foreach (var names in paymentTypes)
                                {
                                    Paymentname.Append(names.Name + ",");
                                }
                                Lblnote.InnerHtml = "Note:-Please pay your fee either through <b>" + Paymentname.ToString().TrimEnd(',') + " </b> by clicking here <a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CertificateExamApplication) + "&Appid=" + application.appno.ToString() + "&DemandID=" + application.Demandid.Value.ToString() + "&RU=" + requesturl) + "'>Pay Fee </a>";
                            }
                            else if (applStatus == enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre && application.paymentstatus == paymentPending)
                            {
                                Lblss.Text = "Fee Pending to be Paid by Institute.";
                            }
                            else if (applStatus == enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre)
                            {
                                if (context.Exams.Where(s => s.ID == application.examID).FirstOrDefault().IsBatchProcessable)
                                    Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                                else
                                    Lblss.Text = "Fee Deposited By Candidate and now application is under process.";
                            }
                            else
                            {
                                Lblss.Text = EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                            }
                            trexcycle.Visible = true;
                            trexcycle.BgColor = "#E6F0F0";
                            trcstatus.BgColor = "#c9d7e2";
                            Lblecycle.Text = GetInitCap(application.examcyclename) + " :- " + GetInitCap(application.exam);
                            Lblerror.Visible = false;

                        }
                        else
                        {
                            Lblerror.Text = "Invalid Application No. or Date of Birth";
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                        }
                    }
                };
            }
            else
            {
                Lblerror.Text = "Invalid Captcha Code";
                GenerateNewCaptchaImage();
                txtcode.Text = "";
            }
            if (!String.IsNullOrEmpty(Lblerror.Text))
                Lblerror.Visible = true;
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
            Response.Redirect("../WEB/ApplicationStatus.aspx?" + Request.QueryString);
            GenerateNewCaptchaImage();
            txtcode.Text = "";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void showsidelink(int courseID)
    {
        try
        {
            Sidelink1.SideLinkType = SideLinkItem.SideLinkType.DownloadLink;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 ccatId = context.Courses.Find(courseID).CourseCategoryID;

                IQueryable<Downloadable> files = context.Downloadables;
                var downloadables = files.Where(s => s.CourseID == courseID).Select(s => new { FileID = s.DownloadableFileID.Value, LinkName = s.LinkName })
                                     .Union(files.Where(s => s.CourseCategoryID == ccatId && s.CourseID == null).Select(s => new { FileID = s.DownloadableFileID.Value, LinkName = s.LinkName }))
                                     .Union(files.Where(s => s.CourseID == null && s.CourseCategoryID == null).Select(s => new { FileID = s.DownloadableFileID.Value, LinkName = s.LinkName }))
                                     .Distinct();

                foreach (var file in downloadables)
                {
                    Sidelink1.Items.Add(new SideLinkItem(file.LinkName, "../Handlers/UploadedFileHandler.ashx?ID=" + file.FileID.ToString(), "", "_blank"));
                }
                Sidelink1.Render();
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}