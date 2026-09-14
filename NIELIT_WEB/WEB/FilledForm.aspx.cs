using System;
using System.Linq;
using System.Web.UI;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class FilledForm : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Lblerror.Text = "";
            //if (Request.UrlReferrer == null)
            //if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in" ) && (Request.UrlReferrer == null  || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "https://nielit.gov.in"))
            //        {
            //            Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
            //            Response.End();
            //            return;
            //        }

            String [] allowedreferrers = System.Web.Configuration.WebConfigurationManager.AppSettings["AllowedReferrers"].Split(',').Select(x => x.Trim().ToLower()).ToArray();
            string referrerHost = Request.UrlReferrer == null ? null : Request.UrlReferrer.Host.ToLower();

            if(referrerHost== null || !allowedreferrers.Contains(referrerHost)){
                string urldecoded = System.Web.HttpUtility.HtmlDecode(
        GeInvalidRequestMessage("Goto Home Page", "../Index.aspx"));

                Response.Write(urldecoded);
                Response.End();
                return;
            }

            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
                    showsidelink(courseID);
                    RenderPage(courseID);
                }
                GenerateNewCaptchaImage();
                txtcode.Text = "";
            }
            base.ReWriteAction(this.Form);
            if (!String.IsNullOrEmpty(Lblerror.Text))
                Lblerror.Visible = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void RenderPage(int courseID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Course currentCourse = context.Courses.Find(courseID);
                string tt = Convert.ToString(Session["ModuleID"]);
                if (currentCourse.enmCourseType == enmCourseType.CertificationCourse && (Request.QueryString["type"] == Convert.ToString(3)))
                {
                    if (Request.UrlReferrer.ToString().ToLower().Contains("aboutcourse.aspx"))
                    { BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("View Filled Application", "WEB/FilledForm.aspx?ID=" + Request.QueryString["id"], "")); }
                    else
                    { BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("View Filled Application", "WEB/FilledForm.aspx?ID=" + Request.QueryString["id"], "")); }
                    Lblheading.Text = "View Filled Application";
                    Lblsubheading.Text = "Course Name";
                    Lblcname.Text = currentCourse.Name;
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
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("View Filled Application", "WEB/FilledForm.aspx?ID=" + Request.QueryString["id"], ""));
                    else
                        BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("View Filled Application", "WEB/FilledForm.aspx?ID=" + Request.QueryString["id"], ""));
                    Lblheading.Text = "View Filled Application";
                    Lblsubheading.Text = "Course Name";
                    Lblcname.Text = currentCourse.Name;
                    Sidelink.Items.Add(new SideLinkItem("Apply Online", "RulesForOnlineRegistration.aspx?ID=" + Request.QueryString["id"] + "&type=" + tt + "", "../images/Apply_Online.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Check Application Status", "ApplicationStatus.aspx?ID=" + Request.QueryString["id"] + "&type=" + tt + "", "../images/View_Application_Status.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Result", "Result.aspx?ID=" + Request.QueryString["id"] + "&type=" + tt + "", "../images/View_Result.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Search Accredited Centre", "FrmAccredetedCentre.aspx?ID=" + Request.QueryString["id"] + "&type=" + tt + "", "../images/View_Certificate_Status.jpg"));
                    Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                    Sidelink.Render();
                }
                else
                {
                    if (Request.UrlReferrer.ToString().ToLower().Contains("aboutcourse.aspx"))
                    { BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("View Filled Form", "WEB/FilledForm.aspx?" + Request.QueryString, "")); }
                    else
                    { BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("View Filled Form", "WEB/FilledForm.aspx?" + Request.QueryString, "")); }

                    Lblheading.Text = "View Filled Form";
                    Lblsubheading.Text = "Certificate Name";
                    Lblcname.Text = currentCourse.Name;
                    Sidelink.Items.Add(new SideLinkItem("Apply Online", "RulesForOnlineRegistration.aspx?" + Request.QueryString + "&type=" + tt + "", "../images/Apply_Online.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Check Form Status", "ApplicationStatus.aspx?" + Request.QueryString + "&type=" + tt + "", "../images/View_Application_Status.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Download Admit Card", "DownloadAdmitCard.aspx?" + Request.QueryString + "&type=" + tt + "", "../images/Print_Admit_Card.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Result", "Result.aspx?" + Request.QueryString + "&type=" + tt + "", "../images/View_Result.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Search Centre", "FrmAccredetedCentre.aspx?" + Request.QueryString + "&type=" + tt + "", "../images/View_Certificate_Status.jpg"));
                    Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                    Sidelink.Render();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
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
    protected void BtnView_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (txtcode.Text == ViewState["CaptchCode"].ToString())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    string applID = TxtAppno.Text.ToString();
                    Int32 CourseID = Convert.ToInt32(Request.QueryString["id"].ToString());
                    DateTime DOB = Convert.ToDateTime(TxtDOB.Text.ToString());
                    Course currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["id"].ToString()));
                    if (currentCourse.enmCourseType == enmCourseType.CertificationCourse && (Request.QueryString["type"] == Convert.ToString(3)))
                    {
                        var application = (from a in context.CourseRegistrationApplications//need to change it
                                           where a.Number.ToUpper() == applID.ToUpper() && a.DateOfBirth == DOB && a.CourseID == CourseID
                                           select new
                                           {
                                               Category = a.CourseCategory.Name,
                                               Course = a.Course.Name,
                                               Name = a.Salutation + " " + a.Name,
                                               fname = a.FatherName,
                                               mname = a.MotherName,
                                               gname = a.GuardianName,
                                               ApplicationNumber = a.Number,
                                               Appid = a.ID,
                                               ApplicationDate = a.ApplicationDate,
                                               FinalSubmitted = a.FinalSubmitted
                                           }).FirstOrDefault();

                        if (application != null)
                        {
                            if (application.FinalSubmitted)
                            {
                                divFilter.Visible = false;
                                divpreview.Visible = true;
                                Lblhead.Text = GetInitCap("APPLICATION DETAILS OF ONLINE EXAMINATION APPLICATION  FOR") + " " + application.Course;
                                Lbcname.Text = application.Category.ToUpper() + ":-" + application.Course.ToUpper();
                                Lblappname.Text = GetInitCap(application.Name);
                                if (string.IsNullOrEmpty(application.gname) == true && string.IsNullOrWhiteSpace(application.gname) == true)
                                {
                                    trfathername.Visible = true;
                                    trmothername.Visible = true;
                                    trgname.Visible = false;
                                    if (string.IsNullOrEmpty(application.fname) == false && !string.IsNullOrWhiteSpace(application.fname))
                                        Lblfname.Text = "Mr. " + GetInitCap(application.fname);
                                    else
                                        Lblfname.Text = "NA";
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
                                LblAppno.Text = application.ApplicationNumber.ToString();
                                Lblappdate.Text = application.ApplicationDate.ToString("dd-MMM-yyyy");
                                Lblerror.Text = "Please click on view button to print filled form details";
                                BtnPrint.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/ExamFormPreview.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + application.Appid + "&Dob=" + TxtDOB.Text.ToString() + "&Type=Print") + "');");
                            }
                            else
                            {
                                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/ExamFormPreview.aspx?Appid=" + application.Appid));
                            }

                        }
                        else
                        {
                            Lblerror.Text = "Invalid Application No. or Date of Birth";
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                        }
                    }
                    else if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                    {
                        var application = (from a in context.CourseRegistrationApplications
                                           where a.Number.ToUpper() == applID.ToUpper() && a.DateOfBirth == DOB && a.CourseID == CourseID
                                           select new
                                           {
                                               Category = a.CourseCategory.Name,
                                               Course = a.Course.Name,
                                               Name = a.Salutation + " " + a.Name,
                                               fname = a.FatherName,
                                               mname = a.MotherName,
                                               gname = a.GuardianName,
                                               ApplicationNumber = a.Number,
                                               Appid = a.ID,
                                               ApplicationDate = a.ApplicationDate,
                                               FinalSubmitted = a.FinalSubmitted
                                           }).FirstOrDefault();


                        if (application != null)
                        {
                            if (application.FinalSubmitted)
                            {
                                divFilter.Visible = false;
                                divpreview.Visible = true;
                                Lblhead.Text = GetInitCap("APPLICATION DETAILS  OF ONLINE REGISTRATION APPLICATION  FOR") + " " + application.Course;
                                Lbcname.Text = application.Category.ToUpper() + ":-" + application.Course.ToUpper();
                                Lblappname.Text = GetInitCap(application.Name);
                                if (string.IsNullOrEmpty(application.gname) == true && string.IsNullOrWhiteSpace(application.gname) == true)
                                {
                                    trfathername.Visible = true;
                                    trmothername.Visible = true;
                                    trgname.Visible = false;
                                    if (string.IsNullOrEmpty(application.fname) == false && !string.IsNullOrWhiteSpace(application.fname))
                                        Lblfname.Text = "Mr. " + GetInitCap(application.fname);
                                    else
                                        Lblfname.Text = "NA";
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
                                LblAppno.Text = application.ApplicationNumber.ToString();
                                Lblappdate.Text = application.ApplicationDate.ToString("dd-MMM-yyyy");
                                Lblerror.Text = "Please click on view button to print filled form details";
                                BtnPrint.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/FrmPreview.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + application.Appid + "&Dob=" + TxtDOB.Text.ToString() + "&Type=Print") + "');");
                            }
                            else
                            {
                                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/FrmPreview.aspx?Appid=" + application.Appid));
                            }
                        }
                        else
                        {
                            Lblerror.Text = "Invalid Application No. or Date of Birth";
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                        }

                    }
                    else
                    {
                        var application = (from a in context.CertificateExamApplications
                                           where a.Number.ToUpper() == applID.ToUpper() && a.DateOfBirth == DOB && a.CourseID == CourseID
                                           select new
                                           {
                                               Category = a.CourseCategory.Name,
                                               Course = a.Course.Name,
                                               Name = a.Salutation + " " + a.Name,
                                               fname = a.FatherName,
                                               mname = a.MotherName,
                                               gname = a.GuardianName,
                                               ApplicationNumber = a.Number,
                                               Appid = a.ID,
                                               ApplicationDate = a.ApplicationDate,
                                               FinalSubmit = a.FinalSubmitted
                                           }).FirstOrDefault();


                        if (application != null)
                        {
                            if (application.FinalSubmit)
                            {
                                divFilter.Visible = false;
                                divpreview.Visible = true;
                                Lblhead.Text = GetInitCap("APPLICATION DETAILS OF ONLINE EXAMINATION APPLICATION  FOR") + " " + application.Course;
                                Lbcname.Text = application.Category.ToUpper() + ":-" + application.Course.ToUpper();
                                Lblappname.Text = GetInitCap(application.Name);
                                if (string.IsNullOrEmpty(application.gname) == true && string.IsNullOrWhiteSpace(application.gname) == true)
                                {
                                    trfathername.Visible = true;
                                    trmothername.Visible = true;
                                    trgname.Visible = false;
                                    if (string.IsNullOrEmpty(application.fname) == false && !string.IsNullOrWhiteSpace(application.fname))
                                        Lblfname.Text = "Mr. " + GetInitCap(application.fname);
                                    else
                                        Lblfname.Text = "NA";
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
                                LblAppno.Text = application.ApplicationNumber.ToString();
                                Lblappdate.Text = application.ApplicationDate.ToString("dd-MMM-yyyy");
                                Lblerror.Text = "Please click on view button to print  filled form details";
                                BtnPrint.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificatePreview.aspx?ID=" + Request.QueryString["id"] + "&Appid=" + application.Appid + "&candtype=" + Request.QueryString["candtype"] + "&Type=Print") + "');");
                            }
                            else
                            {
                                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificatePreview.aspx?ID=" + Request.QueryString["id"] + "&Appid=" + application.Appid + "&candtype=" + Request.QueryString["candtype"]));
                            }
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
    protected void Btnpback_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Response.Redirect("../WEB/FilledForm.aspx?" + Request.QueryString);
            GenerateNewCaptchaImage();
            txtcode.Text = "";
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
            Lblerror.Visible = false;
            GenerateNewCaptchaImage();
            TxtAppno.Text = "";
            TxtDOB.Text = "";
            txtcode.Text = "";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BtnBack_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
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