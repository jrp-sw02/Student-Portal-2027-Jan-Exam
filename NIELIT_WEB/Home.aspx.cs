using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.Objects;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

public partial class Home : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // amit rate limit start
        /* if (IsRateLimited())
         {
             Response.StatusCode = 429;
             Response.AddHeader("Retry-After", "10");
             Response.Write("Too Many Requests");
             Response.End();
             return;
         }*/

        // amit rate limit end

        lblError.Text = "";// FormsAuthentication.HashPasswordForStoringInConfigFile("nitesh", System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (!Page.IsPostBack)
            {
                Session["OtpCount"] = 0;
                AllNotification();
                GeneralNotifications();
                GenerateNewCaptchaImage();
                trOtpCode.Visible = false;
                trOtp.Visible = false;
                Session["Bread"] = null;
                /* Sidelink.Items.Add(new SideLinkItem("Certification/Courses", "WEB/allCourses.aspx", "images/Get_Filled_Form.jpg"));
                 Sidelink.Items.Add(new SideLinkItem("Apply Online", "WEB/allCourses.aspx?query=apply", "images/Apply_Online.jpg"));
                 Sidelink.Items.Add(new SideLinkItem("Download Admit Card", "WEB/allCourses.aspx?query=admit", "images/Print_Admit_Card.jpg"));
                 Sidelink.Items.Add(new SideLinkItem("View Result", "WEB/allCourses.aspx?query=result", "images/View_Result.jpg"));
                 Sidelink.Items.Add(new SideLinkItem("Regional Centres", "abt_centers.aspx", "images/View_Application_Status.jpg"));
                 Sidelink.Items.Add(new SideLinkItem("Search Institutes", "WEB/FrmAccredetedCentre.aspx", "images/Get_Filled_Form.jpg"));
                 //Sidelink.Items.Add(new SideLinkItem("Pay for NIELIT Services", "UPI/PayforNielitServices.aspx", "images/Get_Filled_Form.jpg"));
                 Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;*/

                Sidelink.Items.Add(new SideLinkItem("Regional Centres", "abt_centers.aspx", "images/View_Application_Status.jpg"));
                Sidelink.Items.Add(new SideLinkItem("Search Institutes", "WEB/FrmAccredetedCentre.aspx", "images/Get_Filled_Form.jpg"));

                Sidelink.Items.Add(new SideLinkItem("Search Application Number", "Admin/RetrieveApplicationNumber.aspx", "images/Get_Filled_Form.jpg"));

                Sidelink.Items.Add(new SideLinkItem("Certification/Courses", "WEB/allCourses.aspx", "images/Get_Filled_Form.jpg"));
                //Added 7 Apr 2021 for NSQF Courses
                //Sidelink.Items.Add(new SideLinkItem("List of Active NSQF Courses", "Handlers/UploadedFileHandler.ashx?ID=1598721", "images/Get_Filled_Form.jpg"));
                //Modified on 20 June 2022 for NSQF link redirection		
                Sidelink.Items.Add(new SideLinkItem("List of Active NSQF Courses", "https://www.nielit.gov.in/content/nsqf", "_blank"));
                Sidelink.Items.Add(new SideLinkItem("Apply Online", "WEB/allCourses.aspx?query=apply"));
                Sidelink.Items.Add(new SideLinkItem("Training Econtent", "https://econtent.nielit.gov.in/", "images/View_Result.jpg", "_blank"));
                //Commented 20 Feb 2023 Sidelink.Items.Add(new SideLinkItem("Practical AdmitCard O/A/B/C", "https://admitcard.nielit.in", "images/Print_Admit_Card.jpg","IE"));
                Sidelink.Items.Add(new SideLinkItem("Download Admit Card", "WEB/allCourses.aspx?query=admit", "images/Print_Admit_Card.jpg"));
                Sidelink.Items.Add(new SideLinkItem("View Result", "WEB/allCourses.aspx?query=result", "images/View_Result.jpg"));
                Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                Sidelink.Render();
                //Sidelink.Render();

                Dictionary<Int32, Int32> LoginUsersList = new Dictionary<Int32, Int32>();
                LoginUsersList = (Dictionary<Int32, Int32>)Application["LoginUsersList"];
                LblActiveLoggedIn.Text = LoginUsersList.Count().ToString();
            }
        }
        catch (Exception)
        { }
    }

    private bool IsValidForm()
    {
        try
        {
            if (txtcode.Text != ViewState["CaptchCode"].ToString())
            {
                lblError.Visible = true;
                lblError.Text = "Invalid Captcha Code";
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                return false;
            }
            return true;
        }
        catch (Exception ex)
        { throw ex; }
    }
    private bool IsVerifyForm()
    {
        try
        {
            //	lblError.Text="hi";
            if (Session["OtpCount"] != null)
            {
                if (Convert.ToInt32(Session["OtpCount"]) > 3)
                {
                    Response.Clear();
                    Response.StatusCode = 429;
                    Response.TrySkipIisCustomErrors = true;
                    Response.StatusDescription = "Too many OTP verfication attempts";
                    Response.Write("Too many OTP verfication attempts");
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return false;
                    //Response.Clear();
                    // Response.Redirect("Index.aspx");
                }
            }
            else
                Session["OtpCount"] = 0;

            int usertypeid = int.Parse(Session["UserTypeId"].ToString());
            if (usertypeid == 1 || usertypeid == 2 || usertypeid == 6 || usertypeid == 9 || usertypeid == 5 || usertypeid == 10)
            {
                Int64 OtpRefNo = int.Parse(ViewState["OtpRefNumber"].ToString());
                using (EConnectContext context = new EConnectContext())
                {
                    int otpconde = context.UserLoginOtpDetails.Find(OtpRefNo).OtpNumber;
                    if (txtOtp.Text != otpconde.ToString())
                    {
                        Session["OtpCount"] = Convert.ToInt32(Session["OtpCount"]) + 1;
                        lblError.Visible = true;
                        lblError.Text = "Invalid OTP Code";
                        txtOtp.Text = "";
                        return false;
                    }
                }
            }
            return true;
        }
        catch (Exception ex)
        { throw ex; }
    }
    protected void Lnkemail_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";
            Lnkemail.Visible = false;
            if (Session["UserID"] != null)
            {
                CommonFunctions.SendAccountActivationEmail(Convert.ToInt32(Session["UserID"]), true);
                Session["UserID"] = null;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    private void GeneralNotifications()
    {

        string filePath = Server.MapPath("~/App_Data/GeneralNotificationCache.txt");

        // Read from cache if file is less than 6 hours old
        if (File.Exists(filePath))
        {
            FileInfo fi = new FileInfo(filePath);

            if ((DateTime.Now - fi.LastWriteTime).TotalHours < 6)
            {
                otherNotification.InnerHtml = File.ReadAllText(filePath);
                //SetVisitorCounter();
                return;
            }
        }


        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                #region General Notification
                StringBuilder sbgeneral = new StringBuilder();

                var description = (from r in context.NoticeEvents.AsNoTracking()
                                   where System.Data.Entity.DbFunctions.TruncateTime(r.StartDate) <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && System.Data.Entity.DbFunctions.TruncateTime(r.EndDate) >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                   select new
                                   {
                                       ID = r.ID,
                                       desc = r.ActivityDescription,
                                       name = r.ActivityName
                                   });
                description = description.OrderBy(s => s.ID);
                if (description.Count() > 0)
                {
                    foreach (var activity in description.ToList())
                    {
                        sbgeneral.Append("<span class='lblNormal' style='font-weight: bold;text-decoration: underline; padding-left: 4px;'>" + activity.name.TrimEnd('.') + "</span>");
                        sbgeneral.Append("<li>" + GetInitCap(activity.desc.ToString()));
                        sbgeneral.Append("</li>");
                    }
                }
                else
                {
                    sbgeneral.Append("<li> No Other Notification declared </li>");
                }

                string html = sbgeneral.ToString();

                // Save HTML to cache file
                File.WriteAllText(filePath, html);

                otherNotification.InnerHtml = html;


                // otherNotification.InnerHtml = sbgeneral.ToString();
                #endregion




                #region Visitor Counter
                var org = context.Organizations.Find(1);
                if (Session["counter"] != null)
                    lblCounter.InnerText = Session["counter"].ToString() + " out of " + org.VisitorCounter.ToString();
                #endregion
            }
            ;
        }
        catch (Exception ex)
        { ShowAlert(ex.Message); }
    }
    private void AllNotification()
    {
        try
        {

            string examFile = Server.MapPath("~/App_Data/ExamNotificationCache.txt");
            string admitFile = Server.MapPath("~/App_Data/AdmitCardNotificationCache.txt");
            string resultFile = Server.MapPath("~/App_Data/ResultNotificationCache.txt");
            string pracFile = Server.MapPath("~/App_Data/PracticalAdmitCardNotificationCache.txt");

            bool examCached = File.Exists(examFile) && (DateTime.Now - new FileInfo(examFile).LastWriteTime).TotalHours < 6;
            bool admitCached = File.Exists(admitFile) && (DateTime.Now - new FileInfo(admitFile).LastWriteTime).TotalHours < 6;
            bool resultCached = File.Exists(resultFile) && (DateTime.Now - new FileInfo(resultFile).LastWriteTime).TotalHours < 6;
            bool pracCached = File.Exists(pracFile) && (DateTime.Now - new FileInfo(pracFile).LastWriteTime).TotalHours < 6;

            if (examCached) examNotification.InnerHtml = File.ReadAllText(examFile);
            if (admitCached) admitCardNotification.InnerHtml = File.ReadAllText(admitFile);
            if (resultCached) resultNotification.InnerHtml = File.ReadAllText(resultFile);
            if (pracCached) practicaladmitCardNotification.InnerHtml = File.ReadAllText(pracFile);

            if (examCached && admitCached && resultCached && pracCached)
                return;


            using (var context = new EConnectContext())
            {
                var Allexams = (from r in context.Exams.AsNoTracking()
                                where r.DateOfPublishingOfTimeTable != null && r.ExamYear >= 2017
                                select new
                                {
                                    Name = r.Name,
                                    Course = r.Course.Name,
                                    Code = r.Course.Code,
                                    Category = r.CourseCategory.Code,
                                    ID = r.ID,
                                    Emonth = r.ExamMonth,
                                    Eyear = r.ExamYear,
                                    DateOfPublishingOfTimeTable = r.DateOfPublishingOfTimeTable,
                                    DateOfPublishingOfRollNumber = r.DateOfPublishingOfRollNumber,
                                    DateofPublishingPracticalAdmitCard = r.DateofPublishingPracticalAdmitCard,
                                    DateOfPublishingOfResult = r.DateOfPublishingOfResult,
                                    dateofadmitcard = r.DateOfPublishingOfRollNumber,
                                    dateofpractadmitcard = r.DateofPublishingPracticalAdmitCard,
                                    dateofresult = r.DateOfPublishingOfResult
                                });



                #region Exam-Block------

                StringBuilder sbexam = new StringBuilder(); StringBuilder sbtemp = new StringBuilder();
                var exams = Allexams.Where(s => System.Data.Entity.DbFunctions.TruncateTime(s.DateOfPublishingOfTimeTable) <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && s.DateOfPublishingOfRollNumber == null);
                exams = exams.OrderBy(s => s.Eyear).ThenBy(s => s.Emonth);

                DataTable DTexams = new DataTable();
                DataTable DTFilter = new DataTable();

                //Implement Exam Notification on Home Page thru proc 
                //string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                //SqlConnection con = new SqlConnection(constr);
                //con.Open();

                //using (SqlCommand Cmm = new SqlCommand("Exam_Notification", con))
                //{
                //    Cmm.CommandType = CommandType.StoredProcedure;
                //    SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                //    Sda.Fill(DTexams);
                //}

                //DataTable copyDataTable;
                //copyDataTable = DTexams.Copy();
                ////copyDataTable.Columns.Remove("examName");
                ////copyDataTable.Columns.Remove("examCategory");
                ////copyDataTable.Columns.Remove("examCourse");
                //copyDataTable.Columns.Remove("ActivityName");
                //copyDataTable.Columns.Remove("ApplicantType");
                //copyDataTable.Columns.Remove("Efferctive_Date");



                //if (DTexams.Rows.Count > 0)
                //{
                //    //DTexams = DTexams.AsEnumerable()
                //    //.GroupBy(r => new { Col1 = r["examName"], Col2 = r["examCategory"], Col3 = r["examCourse"] })
                //    //.Select(g => g.OrderBy(r => r["Efferctive_Date"]).First())
                //    //.CopyToDataTable();
                //    string x = "", y = "";
                //    foreach (DataRow dtRow in DTexams.Rows)
                //    {
                //        x = dtRow["Filter"].ToString();
                //        //DTexams.Select("CName like '" + txtCName.Text.Trim() + "%' OR CId = "+txtCId.Text.Trim ())
                //        var cutoffDates = (from c in context.CutOffDates where c.ExamID == 1 orderby c.EfferctiveDate, c.ApplicantType.Name select c).ToList();
                //        if (x != y)
                //        {
                //            y = x;
                //            sbexam.Append("<li><b>" + dtRow["examName"].ToString() + " " + dtRow["examCategory"].ToString() + ", " + dtRow["examCourse"].ToString() + "</b>");
                //            sbtemp.Append(" </br><i><span style='font-size:9pt;padding:0 0 0 0; margin:0;'>");
                //        }

                //        //if (cutoffDates.Count() > 0)
                //        //{

                //            //foreach (DataRow dtRow2 in DTexams.Rows)
                //            //{
                //            if (x == y)
                //            {
                //                if (dtRow["examName"].ToString() + dtRow["examCategory"].ToString() + dtRow["examCourse"].ToString() == dtRow["Filter"].ToString())
                //                    sbtemp.Append(dtRow["ActivityName"] + " for " + dtRow["ApplicantType"] + " Candidate: " + Convert.ToDateTime(dtRow["Efferctive_Date"]).ToString("dd-MMM-yyyy") + "; ");
                //                //}
                //            }
                //            sbtemp.Replace(";", "</br>");
                //            sbexam.Append(sbtemp + "</span></i></li>");
                //        //} 
                //    sbtemp.Clear();
                //    }
                //}
                //else
                //{
                //    sbexam.Append("<li>No exam declared</li>");
                //}
                //examNotification.InnerHtml = sbexam.ToString();


                if (exams.Count() > 0)
                {
                    foreach (var exam in exams.ToList())
                    {
                        IQueryable<CutOffDate> CuttOffDates;
                        var cutoffDates = (from c in context.CutOffDates where c.ExamID == exam.ID orderby c.EfferctiveDate, c.ApplicantType.Name select c).ToList();
                        CuttOffDates = (from c in context.CutOffDates where c.ExamID == exam.ID orderby c.EfferctiveDate, c.ApplicantType.Name select c);
                        if (cutoffDates.Count() > 0)
                        {
                            sbexam.Append("<li><b>" + exam.Name + " " + exam.Category + ", " + exam.Course + "</b>");
                            sbtemp.Append(" </br><i><span style='font-size:9pt;padding:0 0 0 0; margin:0;'>");
                            foreach (var cutoffDate in cutoffDates)
                            {
                                sbtemp.Append(cutoffDate.Activity.Name + " for " + cutoffDate.ApplicantType.Name + " Candidate: " + cutoffDate.EfferctiveDate.ToString("dd-MMM-yyyy") + "; ");
                            }
                            sbtemp.Replace(";", "</br>");
                            sbexam.Append(sbtemp + "</span></i></li>");
                        }
                        sbtemp.Clear();
                    }
                }
                else
                {
                    sbexam.Append("<li>No exam declared</li>");
                }
                //  examNotification.InnerHtml = sbexam.ToString();

                string examHtml = sbexam.ToString();
                File.WriteAllText(examFile, examHtml);
                examNotification.InnerHtml = examHtml;

                #endregion

                #region AdmitCard-Block------

                StringBuilder sbadmitcard = new StringBuilder();
                var excludedIdsForAug2025Cid2 = new List<int> { 8768, 8756, 8744, 8732, 8720 };         //to hide aug 2025 for course Cat 2     --06082025
                var admitcard = Allexams.Where(s => s.DateOfPublishingOfRollNumber != null && System.Data.Entity.DbFunctions.TruncateTime(s.DateOfPublishingOfRollNumber) <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && s.DateOfPublishingOfResult == null && !excludedIdsForAug2025Cid2.Contains(s.ID)); //to hide aug 2025 for course Cat 2 (excludedIdsForAug2025Cid2)    --06082025);
                admitcard = admitcard.OrderBy(s => s.Eyear).ThenBy(s => s.Emonth);

                if (admitcard.Count() > 0)
                {
                    foreach (var exam in admitcard.ToList())
                    {
                        sbadmitcard.Append("<li><b>" + exam.Name + " " + exam.Category + "</b>, " + exam.Course + " admit card can be downloaded/printed from " + exam.dateofadmitcard.GetValueOrDefault().ToString("dd-MMM-yyyy"));
                        sbadmitcard.Append("</li>");
                    }
                }
                else
                {
                    sbadmitcard.Append("<li>No admit card declared</li>");
                }
                //  admitCardNotification.InnerHtml = sbadmitcard.ToString();
                string admitHtml = sbadmitcard.ToString();
                File.WriteAllText(admitFile, admitHtml);
                admitCardNotification.InnerHtml = admitHtml;

                #endregion

                #region Result-Block------

                DateTime AddMonths = DateTime.Now.AddMonths(-3);
                StringBuilder sbresult = new StringBuilder();
                var results = Allexams.Where(s => s.DateOfPublishingOfResult != null && s.DateOfPublishingOfResult >= AddMonths);
                results = results.OrderBy(s => s.Eyear).ThenBy(s => s.Emonth);


                if (results.Count() > 0)
                {
                    foreach (var exam in results.ToList())
                    {


                        sbresult.Append("<li><b>" + exam.Name + " " + exam.Category + "</b>, " + exam.Course + " result can be viewed/printed from " + exam.dateofresult.GetValueOrDefault().ToString("dd-MMM-yyyy"));
                        //Added for centres

                        var centres = (from c in context.tempExamResultPubs.AsNoTracking()
                                           /* join r in context.Exams.AsNoTracking()
                                            on c.examId equals r.ID
                                            where c.DateOfPublishing != null && r.DateOfPublishingOfTimeTable != null && r.ExamYear >= 2017
                                            && r.DateOfPublishingOfResult != null && r.DateOfPublishingOfResult >= AddMonths*/
                                       where c.examId == exam.ID && c.DateOfPublishing != null
                                       select new
                                       {
                                           dateofresult = c.DateOfPublishing,
                                           centre = c.RegionalCenter.Code,
                                           centreCode = c.centreCode,
                                           examCentreCount = c.examCentreCount
                                       });

                        //  var totCentres= from  cent in context.RegionalCenters 
                        //  select cent.ID ;

                        //Shivesh 14/11/2018
                        //var totCentres=(from cent in context.CertificateExamApplications 
                        //               where cent.ExamID  == exam.ID
                        //               select new
                        //               {
                        //                   examCentre=cent.Number.Substring (0,2)
                        //               }).Distinct();
                        //string ex="";
                        //foreach (var i in totCentres.ToList())
                        //{
                        //    ex = ex + ' ' + i.examCentre;
                        //}
                        //if (centres.ToList().Count() < totCentres.ToList().Count() && centres .ToList() .Count() >0)
                        //{
                        //    sbresult.Append(" for roll nos. starting from ");
                        //    foreach (var cent in centres.ToList())
                        //    {
                        //        sbresult.Append(cent.centre + ", ");
                        //    }
                        //    sbresult.Remove(sbresult.Length-2, 1);
                        //}
                        int cCount = 0;
                        string ex = "";
                        foreach (var i in centres.ToList())
                        {
                            ex = ex + ' ' + i.centreCode;
                            cCount = i.examCentreCount;
                        }
                        if (centres.ToList().Count() < cCount && cCount > 0)
                        {
                            sbresult.Append(" for roll nos. starting from ");
                            foreach (var cent in centres.ToList())
                            {
                                sbresult.Append(cent.centre + ", ");
                            }
                            sbresult.Remove(sbresult.Length - 2, 1);
                        }
                        sbresult.Append("</li>");
                    }

                    //Shivesh 14/11/2018
                    // sbresult.Append("</li>");
                    // }
                }
                else
                {
                    sbresult.Append("<li>No result declared</li>");
                }
                //      resultNotification.InnerHtml = sbresult.ToString();
                string resultHtml = sbresult.ToString();
                File.WriteAllText(resultFile, resultHtml);
                resultNotification.InnerHtml = resultHtml;

                #endregion

                #region Practical-AdmitCard-Block----

                StringBuilder sbpracadmitcard = new StringBuilder();
                var practicalAdmitcard = Allexams.Where(s => s.DateofPublishingPracticalAdmitCard != null && System.Data.Entity.DbFunctions.TruncateTime(s.DateofPublishingPracticalAdmitCard) <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && s.DateOfPublishingOfResult == null);
                practicalAdmitcard = practicalAdmitcard.OrderBy(s => s.Eyear).ThenBy(s => s.Emonth);

                if (practicalAdmitcard.Count() > 0)
                {
                    foreach (var exam in practicalAdmitcard.ToList())
                    {
                        sbpracadmitcard.Append("<li><b>" + exam.Name + " " + exam.Category + "</b>, " + exam.Course + " Practical Examination admit card can be downloaded/printed from " + exam.dateofpractadmitcard.GetValueOrDefault().ToString("dd-MMM-yyyy"));
                        sbpracadmitcard.Append("</li>");
                    }
                }
                else
                {
                    sbpracadmitcard.Append("<li>No Practical Examination admit card declared</li>");
                }
                //   practicaladmitCardNotification.InnerHtml = sbpracadmitcard.ToString();
                string pracHtml = sbpracadmitcard.ToString();
                File.WriteAllText(pracFile, pracHtml);
                practicaladmitCardNotification.InnerHtml = pracHtml;

                #endregion
            }
        }
        catch (Exception ex)
        { ShowAlert(ex.Message); }
        finally { }
    }
    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GenerateNewCaptchaImage();
            txtcode.Text = "";
        }
        catch (Exception) { }
    }
    private void GenerateNewCaptchaImage()
    {
        try
        {
            //ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateRandomNumber(6);

            ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateCaptchaCode(6);
            EConnect.CaptchaImage captcha = new CaptchaImage(ViewState["CaptchCode"].ToString(), 200, 50, "Arial");
            imgcap.Src = captcha.ImageSource;
        }
        catch (Exception ex)
        { ShowAlert(ex.Message); }

    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            // if (Session["UserID"] != null)
            //    Session.Abandon();
            if (IsValidForm())
            {
                btnLogin.Visible = true;
                using (EConnectContext context = new EConnectContext())
                {
                    User loginUser = new EConnect.URM.User();
                    AuthenticationResponse loginResponse = UserManager.ValidateUser(txtUserName.Text, txtPwd.Text, hfKey.Value, int.Parse(ddlUserType.SelectedValue), out loginUser, context, 1);


                    // Utility to movedd all users frm MD5 to SHA256 Encryption
                    var userQuery = from u in context.Users
                                    where u.LoginID.ToUpper() == txtUserName.Text.Trim().ToUpper()
                                   //Added for error in institute login 3 June 2020
                                   && (u.UserTypeID.ToString() == ddlUserType.SelectedValue.ToString() || ddlUserType.SelectedValue.ToString() == "9")
                                    select u;
                    if (userQuery.Count() == 0)
                    {
                        ShowAlert("User doesnot exist");
                        return;
                    }
                    User loginUsers = userQuery.Single();

                    if (loginUsers.Password.Length == 32)
                    {
                        Lnkemail.Visible = false;
                        Session["UserID"] = loginUser.UserID.ToString();
                        //Added for session 9 June 2020
                        Session["UserType"] = loginUser.enmUserType;
                        Session["UserTypeId"] = loginUser.UserTypeID;
                        //
                        lblError.Text = EConnect.Utils.Common.EnumUtility.GetDescription(loginResponse).ToString();
                        Response.Redirect("Admin/AdminChangePasswd.aspx?src=expire");
                    }
                    txtUserName.Text = "";
                    if (loginResponse == AuthenticationResponse.PasswordExpired)
                    {
                        Lnkemail.Visible = false;
                        Session["UserID"] = loginUser.UserID.ToString();
                        //Added for session 9 June 2020
                        Session["UserType"] = loginUser.enmUserType;
                        Session["UserTypeId"] = loginUser.UserTypeID;
                        //
                        lblError.Text = EConnect.Utils.Common.EnumUtility.GetDescription(loginResponse).ToString();
                        Response.Redirect("Admin/AdminChangePasswd.aspx?src=expire");
                    }
                    else if (loginResponse == AuthenticationResponse.FailedLoginAttemptsLimitCrossed)
                    {
                        Lnkemail.Visible = false;
                        // lblError.Text = "You have entered wrong password more than 3 times, please change your password using forgot password wizard.";
                        lblError.Text = "You have entered wrong password more than 3 times, Login Disabled, Please contact Helpdesk.";
                        btnLogin.Visible = false;
                        // Response.Clear();
                        /* Response.StatusCode = 429;
                         Response.TrySkipIisCustomErrors = true;
                         Response.StatusDescription = "Too many OTP verfication attempts";
                         Response.Write("Too many OTP verfication attempts");*/

                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        // HttpContext.Current.ApplicationInstance.CompleteRequest();

                        return;

                    }
                    else if (loginResponse == AuthenticationResponse.LoginDisabled)
                    {
                        Lnkemail.Visible = true;
                        Session["UserID"] = loginUser.UserID.ToString();
                        lblError.Text = "Your account is not activated yet, Please activate you account by clicking on the link provided in the mail sent to you at the time of registration.If you did not receive the mail please click on this link to resend the mail";
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                    }
                    else if (loginResponse == AuthenticationResponse.SuccessfullWithChangeInPasswordRequiredOnFirstLogin)
                    {
                        Lnkemail.Visible = false;
                        Session["UserID"] = loginUser.UserID.ToString();
                        Session["UserType"] = loginUser.enmUserType;
                        Session["UserTypeId"] = loginUser.UserTypeID;
                        lblError.Text = EConnect.Utils.Common.EnumUtility.GetDescription(loginResponse).ToString();
                        Response.Redirect("Admin/AdminChangePasswd.aspx?src=login");
                    }
                    else if (loginResponse == AuthenticationResponse.Successfull || 1==1)
                    {

                        Lnkemail.Visible = false;
                        //if (loginUser.UserTypeID != 3 && loginUser.UserTypeID != 4 && loginUser.UserTypeID != 5 && loginUser.UserTypeID != 7 && loginUser.UserTypeID != 8)

                        if (loginUser.UserTypeID != 3 && loginUser.UserTypeID != 4 && loginUser.UserTypeID != 7 && loginUser.UserTypeID != 8 && loginUser.UserTypeID != 11)
                        {
                            LoginPnl.Visible = false;
                            VeriifyPnl.Visible = true;

                            Session["OrgId"] = loginUser.OrganizationID.ToString();
                            Session["XUserID"] = loginUser.UserID.ToString();
                            Session["UserName"] = loginUser.UserName.ToString();
                            Session["LogID"] = UserManager.GetCerrentLogID(loginUser.LoginID, context, 1);
                            Session["ModuleId"] = loginUser.UserType.ModuleID.ToString();
                            Session["RoleID"] = loginUser.DefaultRoleID;
                            Session["ProjectID"] = "2";
                            hfVal.Value = "1";
                            btnLogin.Enabled = false;
                            Session["UserType"] = loginUser.enmUserType;
                            Session["UserTypeId"] = loginUser.UserTypeID;
                            Session["EntityID"] = loginUser.UserRefNumber;
                            Session["studentReg"] = loginUser.LoginID.ToString();
                            var UserRole = context.Roles.Find(Session["RoleID"]);
                            Session["RoleName"] = UserRole.Name.ToString();

                            //deep add code on 15 May2018
                            //deep add for only regional center
                            if (loginUser.UserTypeID == 5)
                            {
                                // Creating OTP Message
                                Int32 OTP = Convert.ToInt32(EConnect.CommonFunctions.GenerateRandomNumber(6));
                                var contactreg = (from s in context.Users
                                                  join p in context.ExternalEntities on s.UserRefNumber equals p.ID
                                                  where s.UserTypeID == loginUser.UserTypeID && s.LoginID == loginUser.LoginID
                                                  select new { s.LoginID, s.UserTypeID, s.EmailID, s.MobileNumber }).FirstOrDefault();


                                Int64 OtpRefNumber = GenerateLoginOTP(contactreg.LoginID, contactreg.UserTypeID, OTP);
                                ViewState["OtpRefNumber"] = OtpRefNumber;
                                lblOtpRefNo.Text = "OTP Reference Number: " + ViewState["OtpRefNumber"].ToString();

                                String EmailMsg = "Dear " + contactreg.LoginID + ", " + OTP + " is One Time Password for login in student.nielit.gov.in. OTP Reference Number: " + OtpRefNumber;
                                String MobileMsg = "Dear " + contactreg.LoginID + ", " + OTP + " is One Time Password for login in https:\\student.nielit.gov.in. OTP Reference Number: " + OtpRefNumber;

                                //sending Email 
                                if (contactreg.EmailID.Length > 0)
                                {
                                    try
                                    {
                                        EConnect.NIELIT.Email mail = new Email("Online LogIn:NIELIT", EmailMsg, contactreg.EmailID);
                                        mail.Send();
                                    }
                                    catch { ShowAlert("OTP is not sent on E-mail, check on Mobile"); }
                                }

                                // Sending OTP Message
                                if (contactreg.MobileNumber != 0)
                                {
                                    try
                                    {
                                        EConnect.NIELIT.SMS message = new SMS(MobileMsg, contactreg.MobileNumber.ToString(), "1007885424031416341", SmsServiceType.SignleSMS, false);
                                        //EConnect.NIELIT.SMS message = new SMS(MobileMsg, contactreg.MobileNumber.ToString(),"1307159090377248363", SmsServiceType.SignleSMS, false);
                                        int sentMessageCount;
                                        //message.Send(out sentMessageCount);
                                        message.sendOTPMSG(out sentMessageCount);
                                        if (sentMessageCount <= 0)
                                        { throw new Exception("OTP message has not been sent. Try Again"); }
                                    }
                                    catch { ShowAlert("OTP is not sent on Mobile, check on E-mail"); }
                                }


                                trOtp.Visible = true;
                                trOtpCode.Visible = true;
                            }
                            ////deep end code for only regional center on 15 may 2018
                            else
                            {
                                //Added 1 Sep 2019 for NIELIT Projects
                                if (loginUser.UserTypeID == 10)
                                {
                                    // Creating OTP Message
                                    Int32 OTP = Convert.ToInt32(EConnect.CommonFunctions.GenerateRandomNumber(6));
                                    NIELITMISContext context1 = new NIELITMISContext();
                                    var contactreg = (from s in context.Users
                                                          // join p in context.ExternalEntities on s.UserRefNumber equals p.ID
                                                      where s.UserTypeID == loginUser.UserTypeID && s.LoginID == loginUser.LoginID
                                                      select new { s.LoginID, s.UserTypeID, s.EmailID, s.MobileNumber, s.UserRefNumber }).FirstOrDefault();

                                    Int64 NielitCentreid = contactreg.UserRefNumber;
                                    var nielitcentre = (from s in context1.NielitCentres
                                                        where s.ID == NielitCentreid
                                                        select new { s.ID, s.email1, s.mobile }).FirstOrDefault();
                                    //Added 26 jun 2020
                                    if (nielitcentre == null)
                                    {

                                        ShowAlert("Invalid User, Please contact Administrator");
                                        return;
                                    }


                                    Int64 OtpRefNumber = GenerateLoginOTP(contactreg.LoginID, contactreg.UserTypeID, OTP);
                                    ViewState["OtpRefNumber"] = OtpRefNumber;
                                    lblOtpRefNo.Text = "OTP Reference Number: " + ViewState["OtpRefNumber"].ToString();

                                    String EmailMsg = "Dear " + contactreg.LoginID + ", " + OTP + " is One Time Password for login in student.nielit.gov.in. OTP Reference Number: " + OtpRefNumber;
                                    String MobileMsg = "Dear " + contactreg.LoginID + ", " + OTP + " is One Time Password for login in https:\\student.nielit.gov.in. OTP Reference Number: " + OtpRefNumber;

                                    //sending Email 
                                    //  if (nielitcentre.email1.Length > 0)
                                    if (contactreg.EmailID.Length > 0)

                                    {
                                        try
                                        {
                                            EConnect.NIELIT.Email mail = new Email("Online LogIn:NIELIT", EmailMsg, contactreg.EmailID);
                                            mail.Send();
                                        }
                                        catch { ShowAlert("OTP is not sent on E-mail, check on Mobile"); }
                                    }

                                    // Sending OTP Message
                                    //  if (nielitcentre.mobile != 0)
                                    if (contactreg.MobileNumber != 0)

                                    {
                                        try
                                        {
                                            EConnect.NIELIT.SMS message = new SMS(MobileMsg, contactreg.MobileNumber.ToString(), "1007885424031416341", SmsServiceType.SignleSMS, false);
                                            int sentMessageCount;
                                            //message.Send(out sentMessageCount);
                                            message.sendOTPMSG(out sentMessageCount);
                                            if (sentMessageCount <= 0)
                                            { throw new Exception("OTP message has not been sent. Try Again"); }
                                        }
                                        catch { ShowAlert("OTP is not sent on Mobile, check on E-mail"); }
                                    }


                                    trOtp.Visible = true;
                                    trOtpCode.Visible = true;
                                }


                                ///End Code 2 Sep 2019
                                else
                                {
                                    //deep end code on 15 May2018

                                    // Creating OTP Message
                                    Int32 OTP = Convert.ToInt32(EConnect.CommonFunctions.GenerateRandomNumber(6));
                                    var contact = (from s in context.Users
                                                   join p in context.ExternalEntities on s.UserRefNumber equals p.ID
                                                   where s.UserTypeID == loginUser.UserTypeID && s.LoginID == loginUser.LoginID
                                                   select new { s.LoginID, s.UserTypeID, p.Email, p.MobileNumber }).FirstOrDefault();

                                    Int64 OtpRefNumber = GenerateLoginOTP(contact.LoginID, contact.UserTypeID, OTP);
                                    ViewState["OtpRefNumber"] = OtpRefNumber;
                                    lblOtpRefNo.Text = "OTP Reference Number: " + ViewState["OtpRefNumber"].ToString();

                                    String EmailMsg = "Dear " + contact.LoginID + ", " + OTP + " is One Time Password for login in https:\\student.nielit.gov.in. OTP Reference Number: " + OtpRefNumber;
                                    String MobileMsg = "Dear " + contact.LoginID + ", " + OTP + " is One Time Password for login in https:\\student.nielit.gov.in. OTP Reference Number: " + OtpRefNumber;

                                    //sending Email 
                                    if (contact.Email.Length > 0)
                                    {
                                        try
                                        {
                                            EConnect.NIELIT.Email mail = new Email("Online LogIn:NIELIT", EmailMsg, contact.Email);
                                            mail.Send();
                                        }
                                        catch { ShowAlert("OTP is not sent on E-mail, check on Mobile"); }
                                    }

                                    // Sending OTP Message
                                    if (contact.MobileNumber != 0)
                                    {
                                        try
                                        {
                                            EConnect.NIELIT.SMS message = new SMS(MobileMsg, contact.MobileNumber.ToString(), "1007885424031416341", SmsServiceType.SignleSMS, false);
                                            int sentMessageCount;
                                            message.sendOTPMSG(out sentMessageCount);
                                            if (sentMessageCount <= 0)
                                            { throw new Exception("OTP message has not been sent. Try Again"); }
                                        }
                                        catch { ShowAlert("OTP is not sent on Mobile, check on E-mail"); }
                                    }
                                    trOtp.Visible = true;
                                    trOtpCode.Visible = true;
                                } // For NIELIT Centres
                            }//deep add this braces on 15 May 2018
                        }
                        else
                        {
                            Dictionary<Int32, Int32> LoginUsersList = new Dictionary<Int32, Int32>();
                            LoginUsersList = (Dictionary<Int32, Int32>)Application["LoginUsersList"];

                            Session["OrgId"] = loginUser.OrganizationID.ToString();
                            Session["UserID"] = loginUser.UserID.ToString();
                            Session["UserName"] = loginUser.UserName.ToString();
                            Session["LogID"] = UserManager.GetCerrentLogID(loginUser.LoginID, context, 1);
                            Session["ModuleId"] = loginUser.UserType.ModuleID.ToString();
                            Session["RoleID"] = loginUser.DefaultRoleID;
                            Session["ProjectID"] = "2";
                            hfVal.Value = "1";
                            btnLogin.Enabled = false;
                            Session["UserType"] = loginUser.enmUserType;
                            Session["UserTypeId"] = loginUser.UserTypeID;
                            Session["EntityID"] = loginUser.UserRefNumber;
                            Session["studentReg"] = loginUser.LoginID.ToString();

                            //-----------------------Find Role Name ----------Added by vivek-----14/05/2015---
                            var UserRole = context.Roles.Find(Session["RoleID"]);
                            Session["RoleName"] = UserRole.Name.ToString();
                            //-----------------------End------------------------------------------------------
                            Application.Lock();

                            if (LoginUsersList.ContainsKey(loginUser.UserID))
                            {
                                LoginUsersList.Remove(loginUser.UserID);
                                this.ShowAlert("Duplicate login not allowed. You have been logged out from another location");
                            }
                            LoginUsersList.Add(loginUser.UserID, Convert.ToInt32(Session["LogID"]));
                            Application["LoginUsersList"] = LoginUsersList;
                            Application.UnLock();
                            string script = "  if (document.getElementById('" + hfVal.ClientID + "')) {" +
                                "if (document.getElementById('" + hfVal.ClientID + "').value == '1') { " +
                                    " document.getElementById('" + hfVal.ClientID + "').value = '0';" +
                                    "window.top.location = 'Mainpage.aspx';" +
                                "}" +
                                "else { " +
                                    "if (window.top.location.href.indexOf('Home.aspx') >= 0) " +
                                    "{" +
                                        "window.top.location.href = 'Index.aspx';" +
                                    "} " +
                                "} " +
                            "}";
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "", script, true);
                        }
                    }
                    else
                    {
                        Lnkemail.Visible = false;
                        lblError.Text = EConnect.Utils.Common.EnumUtility.GetDescription(loginResponse).ToString();
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";

                    }

                }
                ;
            }
        }
        //catch (Exception)
        //{ lblError.Text = "Login Details are not Correct"; }

        catch (DbEntityValidationException Exception)
        {
            foreach (var eve in Exception.EntityValidationErrors)
            {
                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        ve.PropertyName, ve.ErrorMessage);
                }
            }
            //throw;

            throw Exception;

        }
    }
    protected void btnLVerify_Click(object sender, EventArgs e)
    {
        if (IsVerifyForm())
        {
            Session["UserID"] = Session["XUserID"];

            Dictionary<Int32, Int32> LoginUsersList = new Dictionary<Int32, Int32>();
            LoginUsersList = (Dictionary<Int32, Int32>)Application["LoginUsersList"];

            Application.Lock();

            if (LoginUsersList.ContainsKey(Int32.Parse(Session["UserID"].ToString())))
            {
                LoginUsersList.Remove(Int32.Parse(Session["UserID"].ToString()));
                this.ShowAlert("Duplicate login not allowed. You have been logged out from another location");
            }
            LoginUsersList.Add(Int32.Parse(Session["UserID"].ToString()), Convert.ToInt32(Session["LogID"]));
            Application["LoginUsersList"] = LoginUsersList;
            Application.UnLock();

            if (Session["UserID"] != null)
            {
                string script = "  if (document.getElementById('" + hfVal.ClientID + "')) {" +
                    "if (document.getElementById('" + hfVal.ClientID + "').value == '1') { " +
                        " document.getElementById('" + hfVal.ClientID + "').value = '0';" +
                        "window.top.location = 'Mainpage.aspx';" +
                    "}" +
                    "else { " +
                        "if (window.top.location.href.indexOf('Home.aspx') >= 0) " +
                        "{" +
                            "window.top.location.href = 'Index.aspx';" +
                        "} " +
                    "} " +
                "}";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "", script, true);
            }
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Session["UserID"] != null)
        { Session.Abandon(); }
        Response.Redirect("Index.aspx", true);
    }
    public static Int64 GenerateLoginOTP(string UserId, int UserTypeId, int OTP)
    {
        using (EConnectContext context = new EConnectContext())
        {
            UserLoginOtpDetail UserOtp = new UserLoginOtpDetail();
            UserOtp.LoginID = UserId;
            UserOtp.UserTypeID = UserTypeId;
            UserOtp.OtpNumber = OTP;
            UserOtp.CreatedOn = DateTime.Now;
            context.UserLoginOtpDetails.Add(UserOtp);
            context.SaveChanges();

            return UserOtp.Id;
        }
    }
    private string GetClientIP()
    {
        string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (string.IsNullOrEmpty(ip))
            ip = Request.ServerVariables["REMOTE_ADDR"];

        if (string.IsNullOrEmpty(ip))
            ip = Request.UserHostAddress;

        // Handle multiple proxies
        if (!string.IsNullOrEmpty(ip) && ip.Contains(","))
            ip = ip.Split(',')[0].Trim();

        return ip ?? "unknown";
    }


    private bool IsRateLimited()
    {
        string ip = GetClientIP(); // More reliable IP detection
        string key = "RL_" + ip;

        Application.Lock();
        try
        {
            RateLimitInfo info = Application[key] as RateLimitInfo;

            if (info == null)
            {
                info = new RateLimitInfo
                {
                    Count = 1,
                    WindowStart = DateTime.UtcNow
                };
                Application[key] = info;
                return false;
            }

            // === FIXED WINDOW: 60 seconds window, max 30 requests ===
            const int MAX_REQUESTS = 30;
            const int WINDOW_SECONDS = 60;

            TimeSpan elapsed = DateTime.UtcNow - info.WindowStart;

            if (elapsed.TotalSeconds > WINDOW_SECONDS)
            {
                // Reset window
                info.Count = 1;
                info.WindowStart = DateTime.UtcNow;
                return false;
            }

            info.Count++;

            if (info.Count > MAX_REQUESTS)
            {
                // Optional: Log for audit visibility

                return true;
            }

            return false;
        }
        finally
        {
            Application.UnLock();
        }
    }
}
public class RateLimitInfo
{
    public int Count { get; set; }
    public DateTime WindowStart { get; set; }
}
