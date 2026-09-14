﻿using System;
using System.Data.Objects;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Web;
using System.Collections.Generic;

public partial class DownloadAdmitCard : BasePage
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Lblerror.Text = "";
            if (Request.UrlReferrer == null)
                if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in") && (Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "https://nielit.gov.in"))
                {
                    Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
                    Response.End();
                    return;
                }
            if (!Page.IsPostBack)
            {               
             
                Int32 courseID = 0;
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    courseID = Convert.ToInt32(Request.QueryString["id"]);
                    RenderPage(courseID);
                    showsidelink(courseID);
                }
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                Lblerror.Visible = false;
                Lberror1.Visible = false;
                lbmessage.Visible = false;                
                bindExamYear(courseID);
            }
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

                if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    if (Request.UrlReferrer.ToString().ToLower().Contains("aboutcourse.aspx"))
                    { BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Admit Card", "WEB/DownloadAdmitCard.aspx?ID=" + Request.QueryString["id"], "")); }

                    else
                    {  BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Download Admit Card", "WEB/DownloadAdmitCard.aspx?ID=" + Request.QueryString["id"], ""));}

                    Sidelink.Items.Add(new SideLinkItem("Apply Online", "RulesForOnlineRegistration.aspx?" + Request.QueryString, "../images/Apply_Online.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Filled Application", "FilledForm.aspx?ID=" + Request.QueryString["id"].ToString(), "../images/Get_Filled_Form.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Check Application Status", "ApplicationStatus.aspx?ID=" + Request.QueryString["id"].ToString(), "../images/View_Application_Status.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Result", "Result.aspx?ID=" + Request.QueryString["id"].ToString(), "../images/View_Result.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Search Accredited Centre", "FrmAccredetedCentre.aspx?ID=" + Request.QueryString["id"].ToString(), "../images/View_Certificate_Status.jpg"));
                    Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                    Sidelink.Render();
                }
                else
                {
                    Rdserachby.Items.RemoveAt(1);
                    if (Request.UrlReferrer.ToString().ToLower().Contains("aboutcourse.aspx"))
                    { 
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Admit Card", "WEB/DownloadAdmitCard.aspx?" + Request.QueryString, ""));
                    }
                    else
                    { 
                        BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Download Admit Card", "WEB/DownloadAdmitCard.aspx?" + Request.QueryString, "")); 
                    }

                    Sidelink.Items.Add(new SideLinkItem("Apply Online", "RulesForOnlineRegistration.aspx?" + Request.QueryString, "../images/Apply_Online.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Filled Form", "FilledForm.aspx?" + Request.QueryString, "../images/Get_Filled_Form.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Check Form Status", "ApplicationStatus.aspx?" + Request.QueryString, "../images/View_Application_Status.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("View Result", "Result.aspx?" + Request.QueryString, "../images/View_Result.jpg"));
                    Sidelink.Items.Add(new SideLinkItem("Search Centre", "FrmAccredetedCentre.aspx?" + Request.QueryString, "../images/View_Certificate_Status.jpg"));
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
    protected void GenerateNewCaptchaImage()
    {
        try
        {
            ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateCaptchaCode(6);
            EConnect.CaptchaImage captcha = new CaptchaImage(ViewState["CaptchCode"].ToString(), 200, 50, "Century Schoolbook");
            imgcap.Src = captcha.ImageSource;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void bindExamYear(int courseID)
    {
        using (var context = new EConnectContext())
        {
            if (courseID == 1 ||  courseID == 2|| courseID == 3|| courseID == 4)
            {
               // ListItem lst = new ListItem("--Select One--", "0");
                int currentYear = DateTime.Now.Year;
                int PreviousYear = DateTime.Now.AddYears(-1).Year;
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var ExamYear = context.Exams.Where(s => s.CourseID == courseID && (s.ExamYear == currentYear || s.ExamYear==PreviousYear))
                               .Select(s => new { ValueField = s.ExamYear, TextField = s.ExamYear });

                ExamYear = ExamYear.Distinct().OrderByDescending(p => p.ValueField);
                EConnect.Utils.Common.ControlUtility.BindListObject(Ddlexamyear, ExamYear, lst);
            }
            else
            {
               // ListItem lst = new ListItem("--Select One--", "0");
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var ExamYear = context.Exams.Where(s => s.CourseID == courseID && s.ExamYear > 2018)
                               .Select(s => new { ValueField = s.ExamYear, TextField = s.ExamYear });

                ExamYear = ExamYear.Distinct().OrderByDescending(p => p.ValueField);
                EConnect.Utils.Common.ControlUtility.BindListObject(Ddlexamyear, ExamYear, lst);
            }

        }
    }
    protected void bindexams()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
                int examyear = Convert.ToInt32(Ddlexamyear.SelectedValue);
                Course currentCourse = context.Courses.Find(courseID);
                //ListItem lst = new ListItem("--Select One--", "0");
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");

                IQueryable<Exam> Exam = context.Exams.Where(s => s.CourseID == courseID);
                if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                {
                    Exam = Exam.Where(p => p.DateOfPublishingOfRollNumber != null && p.DateOfPublishingOfRollNumber <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && p.ExamYear == examyear);
                }
                else if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    Exam = Exam.Where(p => (p.DateOfPublishingOfRollNumber != null || p.DateofPublishingPracticalAdmitCard != null || p.Online_Admit_Card_Publish_Date != null) && (p.DateOfPublishingOfRollNumber <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) || p.DateofPublishingPracticalAdmitCard <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) || p.Online_Admit_Card_Publish_Date <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)) && p.ExamYear == examyear && p.DateOfPublishingOfResult == null);
                  //  Exam = Exam.Where(p =>  p.ExamYear == examyear);                
                }
                 //var examname = Exam.Select(s => new { ValueField = s.ID, TextField = "July 2020 - Jan 2021" }).OrderByDescending(s => s.ValueField);
                var examname = Exam.Select(s => new { ValueField = s.ID, TextField = s.Name }).OrderByDescending(s => s.ValueField);
               // var examname = Exam.Where(s => (s.ExamYear == examyear && s.CourseID == courseID) || (s.ExamYear == 2018 && s.CourseID == courseID)).Select(s => new { ValueField = s.ID, TextField = s.Name }).OrderByDescending(s => s.ValueField);

                EConnect.Utils.Common.ControlUtility.BindListObject(Ddlexamcycle, examname, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
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
                var downloadables =   files.Where(s => s.CourseID == courseID).Select(s => new { FileID = s.DownloadableFileID.Value, LinkName = s.LinkName })
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
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Txtappno.Text = "";
            TxtDOB.Text = "";
            Lblerror.Visible = false;
            lbmessage.Visible = false;
            txtcode.Text = "";
            GenerateNewCaptchaImage();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BtnView_Click(object sender, EventArgs e)
    {
        var context = new EConnectContext();
        try
        {
            int  courseID = Convert.ToInt32(Request.QueryString["id"]);
            BreadCrumb1.Render();
            Int32 searchBy = 0;
            if (Ddlexamcycle.SelectedValue == "0")
                throw new Exception("Please select Examination Name");
            if (Rdserachby.SelectedValue == "1" && (courseID == 1 || courseID == 2 || courseID == 3 || courseID == 4))
            {
                if (String.IsNullOrWhiteSpace(Txtappno.Text))
                    throw new Exception("Please Enter Application No.");               
                searchBy = 1;//Search by Application No.
                //throw new Exception("Please select Registration Number Radio Button .");
                //return;                
            }
            if (Rdserachby.SelectedValue == "1")
            {
                if (String.IsNullOrWhiteSpace(Txtappno.Text))
                    throw new Exception("Please Enter Application No.");
                searchBy = 1;//Search by Application No.
                //throw new Exception("Please select Registration Number Radio Button .");
                //return;
            }
            else if (Rdserachby.SelectedValue == "2")
            {
                if (String.IsNullOrWhiteSpace(Txtappno.Text))
                    throw new Exception("Please enter Registration No.");
                else if (!IsNumeric(Txtappno.Text))
                    throw new Exception("Please enter Valid Registration No.");
                searchBy = 2;//Search by Registration No.
            }
            if (String.IsNullOrWhiteSpace(TxtDOB.Text.Trim()))
                throw new Exception("Please enter Candidate's Date of Birth.");
            if (!IsDate(TxtDOB.Text))
                throw new Exception("Invalid Date of Birth of the candidate");
            if (String.IsNullOrWhiteSpace(txtcode.Text.Trim()))
                throw new Exception("Please enter Captcha Code shown in image below.");
            if (txtcode.Text == ViewState["CaptchCode"].ToString())
            {
                string appnumber = Txtappno.Text.ToString();
                Int32 CourseID = Convert.ToInt32(Request.QueryString["id"].ToString());
                Int32 examID = Convert.ToInt32(Ddlexamcycle.SelectedValue);
                DateTime DOB = Convert.ToDateTime(TxtDOB.Text.ToString());
                Course currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["id"].ToString()));
                if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                {
                    Lblcourse.Text = GetInitCap("ADMIT CARD FOR") + " " + GetInitCap(currentCourse.Name);
                    Lblccname.Text = currentCourse.CourseCategory.Name + "-" + GetInitCap("(" + currentCourse.Name + ")");

                    var download =  context.CertificateExamApplications.
                                    Where(a => a.Number.Equals(appnumber, StringComparison.OrdinalIgnoreCase)
                                    && a.ExamID == examID && a.CourseID == CourseID
                                    && a.DateOfBirth == DOB && a.RollNumber != null
                                    && a.FinalSubmitted == true).
                                    Select(a => new
                                    {
                                        name = a.Salutation + a.Name,
                                        Appid = a.ID,
                                        fname = a.FatherName,
                                        mname = a.MotherName,
                                        gname = a.GuardianName,
                                        dob = a.DateOfBirth,
                                        doe = a.Exam.ExamStartDate,
                                        examname = a.Exam.Name,
                                        examcentre = a.ExamCentreName,
                                    }).FirstOrDefault();

                    if (download != null)
                    {
                        divfilter.Visible = false;
                        divresult.Visible = true;
                        divfooter.Visible = true;
                        Btndownload.Visible = true;
                        Btndownload.Text = "Print Admit Card";
                        trpractical.Visible = false;
                        trtheory.Visible = true;
                        Lblname.Text = GetInitCap(download.name);
                        if (string.IsNullOrEmpty(download.gname) == true && string.IsNullOrWhiteSpace(download.gname) == true)
                        {
                            trfathername.Visible = true;
                            trmothername.Visible = true;
                            trgname.Visible = false;
                            if (string.IsNullOrEmpty(download.fname) == false && !string.IsNullOrWhiteSpace(download.fname))
                                Lbfname.Text = "Mr. " + GetInitCap(download.fname);
                            else
                                Lbfname.Text = "NA";
                            if (string.IsNullOrEmpty(download.mname) == false && string.IsNullOrWhiteSpace(download.mname) == false)
                                Lbmname.Text = "Mrs. " + GetInitCap(download.mname);
                            else
                                Lbmname.Text = "NA";
                        }
                        else
                        {
                            Lgname.Text = string.IsNullOrEmpty(download.gname) == false && string.IsNullOrWhiteSpace(download.gname) == false ? GetInitCap(download.gname) : "NA";
                            trfathername.Visible = false;
                            trmothername.Visible = false;
                            trgname.Visible = true;
                            trdob.Attributes.Add("class", "gdalternate1");
                        }
                        Lbldob.Text = download.dob.ToString("dd-MMM-yyyy");
                        Lblexam.Text = (!string.IsNullOrEmpty(download.examname)) ? GetInitCap(download.examname) : "";
                        Lblexamcentre.Text = (!string.IsNullOrEmpty(download.examcentre)) ? download.examcentre.ToUpper() : "";

                        //if ((examID >= 3778 && CourseID == 7) || (examID >= 3790 && CourseID == 5) || (examID >= 3802 && CourseID == 98) || (examID >= 3814 && CourseID == 99))
                        ////if ((examID >= 3778 && CourseID == 7) || (examID >= 3790 && CourseID == 5) || (examID >= 3802 && CourseID == 98) || (examID >= 3519 && CourseID == 99))                            
                        //{ Btndownload.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCardVersion5.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + download.Appid) + "','Form'); return false;"); }                                                                      

                        if ((examID >= 5055 && CourseID == 7) || (examID >= 5067 && CourseID == 5) || (examID >= 5079 && CourseID == 98) || (examID >= 5091 && CourseID == 99) || (examID >= 5103 && CourseID == 174) || (examID >= 5115 && CourseID == 175) || (examID >= 7783 && CourseID == 1226)) //|| (examID >= 5107 && CourseID == 175))
                        { Btndownload.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCardVersion7.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + download.Appid) + "','Form'); return false;"); }
                      
                        
                        else if ((examID >= 4371 && CourseID == 7) || (examID >= 4383 && CourseID == 5) || (examID >= 4395 && CourseID == 98) || (examID >= 4407 && CourseID == 99) || (examID >= 4431 && CourseID == 174) || (examID >= 4455 && CourseID == 175)) //|| (examID >= 5107 && CourseID == 175))
                        { Btndownload.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCardVersion6.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + download.Appid) + "','Form'); return false;"); }
                        else if ((examID >= 3778 && examID < 4371 && CourseID == 7) || (examID >= 3790 && examID < 4383 && CourseID == 5) || (examID >= 3802 && examID < 4395 && CourseID == 98) || (examID >= 3814 && examID < 4407 && CourseID == 99))
                        //if ((examID >= 3778 && CourseID == 7) || (examID >= 3790 && CourseID == 5) || (examID >= 3802 && CourseID == 98) || (examID >= 3519 && CourseID == 99))
                        { Btndownload.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCardVersion5.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + download.Appid) + "','Form'); return false;"); }
                        else if ((examID >= 2708 && CourseID == 7) || (examID >= 2696 && CourseID == 5) || (examID >= 2720 && CourseID == 98) || (examID >= 2732 && CourseID == 99))
                        { Btndownload.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCardVersion4.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + download.Appid) + "','Form'); return false;"); }
                        else if ((examID >= 1944 && CourseID == 7) || (examID >= 1932 && CourseID == 5) || (examID >= 1968 && CourseID == 99) || (examID >= 1956 && CourseID == 98) || (examID >= 2640 && CourseID == 101) || (examID >= 2628 && CourseID == 100))
                        { Btndownload.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCardVersion3.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + download.Appid) + "','Form'); return false;"); }
                        else if ((examID >= 1177 && CourseID == 7) || (examID >= 1189 && CourseID == 5) || (examID >= 1201 && CourseID == 99) || (examID >= 1213 && CourseID == 98) || (examID >= 1225 && CourseID == 101) || (examID >= 1237 && CourseID == 100))
                        { Btndownload.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCardVersion2.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + download.Appid) + "','Form'); return false;"); }
                        else if ((examID >= 1004 && examID < 1011) || (examID >= 992 && examID < 999) || (examID >= 1089 && examID < 1124) || examID >= 1170)
                        { Btndownload.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCardVersion1.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + download.Appid) + "','Form'); return false;"); }
                        else
                        { Btndownload.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificateAdmitCard.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + download.Appid) + "','Form'); return false;"); }
                        Lblerror.Visible = false;
                        lbmessage.Visible = true;
                        tbsearch.Visible = false;
                    }
                    else
                    {
                        Lblerror.Text = "Admit Card not declared for this exam / Invalid Application No. or Date of Birth or Invalid Exam Name.";
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        lbmessage.Visible = false;
                    }
                }
                else if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    string Appno = "";
                    Int64 registrationNumber = 0;
                    if (searchBy == 1)
                    {
                        searchBy = 1;//Search by Application Number.
                        Appno = Txtappno.Text.Trim().ToUpper();

                    }
                    else if (searchBy == 2)
                    {
                        searchBy = 2;//Search by Registration Number.
                        if (!IsNumeric(Txtappno.Text.Trim()))
                        {
                            throw new Exception("Invalid Registration Number. Please enter numeric value only");
                        }
                        registrationNumber = Convert.ToInt64(Txtappno.Text);
                    }
                    Lblcourse.Text = GetInitCap("ADMIT CARD FOR") + " " + GetInitCap(currentCourse.Name);
                    Lblccname.Text = currentCourse.CourseCategory.Name + "-" + "(" + currentCourse.Name + ")";

               //-- below code Commented on date 07082023
//                    var download = (from a in context.CourseExamApplications
//                                    //where (searchBy == 1 ? a.Number.ToUpper() == Appno.ToUpper() : a.RegistrationNumber == registrationNumber) && (a.ExamID == examID || a.ExamID == previousExamId) && a.CourseID == CourseID && a.Candidate.DateOfBirth == DOB && a.FinalSubmitted == true 
//                                    where (searchBy == 1 ? a.Number.ToUpper() == Appno.ToUpper() : a.RegistrationNumber == registrationNumber) && a.ExamID == examID 
//// && a.CourseID == CourseID
// && a.CourseID == CourseID 
// //&& a.AllottedExamStateID == 28  && a.AllottedExamCentreID == 2402 
// && a.Candidate.DateOfBirth == DOB && a.FinalSubmitted == true
//                                    && (a.RollNumber != null || a.PracticalOfficeRefNumber != null)
//                                    && a.ApplicationStatusID == 10 && (a.PaymentStatusID == 2 || a.PaymentStatusID == 4)
//                                    orderby a.ExamID descending
//                                    //where (searchBy == 1 ? a.Number.ToUpper() == Appno.ToUpper() : a.ID == registrationNumber) && a.ExamID == examID && a.CourseID == CourseID && a.Candidate.DateOfBirth == DOB && a.FinalSubmitted == true && a.RollNumber != null
//                                    select new
//                                    {
//                                        name = a.Candidate.Salutation + a.Candidate.Name,
//                                        Appid = a.ID,
//                                        fname = a.Candidate.FatherName,
//                                        mname = a.Candidate.MotherName,
//                                        gname = a.Candidate.GuardianName,
//                                        doe = a.Exam.ExamStartDate,
//                                        dob = a.Candidate.DateOfBirth,
//                                        rollno = a.RollNumber,
//                                        examname = a.Exam.Name,
//                                        examcentre = a.AllottedExamCentre.Name,
//                                        pracexamcentre = a.PracticalInstituteName,
//                                        noofpractmodule = a.NumberOfPracticalModulesApplied,
//                                        nooftheorymodule = a.NumberOfTheoryModulesApplied,
//                                        rollnumber = a.RollNumber.HasValue ? a.RollNumber.Value : 0,
//                                        profficerefno = a.PracticalOfficeRefNumber,
//                                        examId = a.ExamID
//                                    }).FirstOrDefault();

                    var download = (from a in context.CourseExamApplications
                                    //where (searchBy == 1 ? a.Number.ToUpper() == Appno.ToUpper() : a.RegistrationNumber == registrationNumber) && (a.ExamID == examID || a.ExamID == previousExamId) && a.CourseID == CourseID && a.Candidate.DateOfBirth == DOB && a.FinalSubmitted == true 
                                    where (searchBy == 1 ? a.Number.ToUpper() == Appno.ToUpper() : a.RegistrationNumber == registrationNumber) && a.ExamID == examID
                                        // && a.CourseID == CourseID
 && a.CourseID == CourseID
                                        //&& a.AllottedExamStateID == 28  && a.AllottedExamCentreID == 2402 
 && a.Candidate.DateOfBirth == DOB && a.FinalSubmitted == true
                                    && (a.RollNumber != null)
                                    && (a.ApplicationStatusID == 10 || a.ApplicationStatusID == 8 ) && (a.PaymentStatusID == 2 || a.PaymentStatusID == 4)
                                    orderby a.ExamID descending
                                    //where (searchBy == 1 ? a.Number.ToUpper() == Appno.ToUpper() : a.ID == registrationNumber) && a.ExamID == examID && a.CourseID == CourseID && a.Candidate.DateOfBirth == DOB && a.FinalSubmitted == true && a.RollNumber != null
                                    select new
                                    {
                                        name = a.Candidate.Salutation + a.Candidate.Name,
                                        Appid = a.ID,
                                        fname = a.Candidate.FatherName,
                                        mname = a.Candidate.MotherName,
                                        gname = a.Candidate.GuardianName,
                                        doe = a.Exam.ExamStartDate,
                                        dob = a.Candidate.DateOfBirth,
                                        rollno = a.RollNumber,
                                        examname = a.Exam.Name,
                                        examcentre = a.AllottedExamCentre.Name,
                                        pracexamcentre = a.PracticalInstituteName,
                                        noofpractmodule = a.NumberOfPracticalModulesApplied,
                                        nooftheorymodule = a.NumberOfTheoryModulesApplied,
                                        rollnumber = a.RollNumber.HasValue ? a.RollNumber.Value : 0,
                                        profficerefno = a.PracticalOfficeRefNumber,
                                        examId = a.ExamID
                                    }).FirstOrDefault();
                  
                    if (download != null)
                    {
                        divfilter.Visible = false;
                        divresult.Visible = true;
                        divfooter.Visible = true;
                        Lblname.Text = GetInitCap(download.name);
                        if (string.IsNullOrEmpty(download.gname) == true && string.IsNullOrWhiteSpace(download.gname) == true)
                        {
                            trfathername.Visible = true;
                            trmothername.Visible = true;
                            trgname.Visible = false;
                            if (string.IsNullOrEmpty(download.fname) == false && !string.IsNullOrWhiteSpace(download.fname))
                                Lbfname.Text = "Mr. " + GetInitCap(download.fname);
                            else
                                Lbfname.Text = "NA";
                            if (string.IsNullOrEmpty(download.mname) == false && string.IsNullOrWhiteSpace(download.mname) == false)
                                Lbmname.Text = "Mrs. " + GetInitCap(download.mname);
                            else
                                Lbmname.Text = "NA";
                        }
                        else
                        {
                            Lgname.Text = string.IsNullOrEmpty(download.gname) == false && string.IsNullOrWhiteSpace(download.gname) == false ? GetInitCap(download.gname) : "NA";
                            trfathername.Visible = false;
                            trmothername.Visible = false;
                            trgname.Visible = true;
                            trdob.Attributes.Add("class", "gdalternate1");
                        }
                        Lbldob.Text = download.dob.ToString("dd-MMM-yyyy");
                        Lblexam.Text = (!string.IsNullOrEmpty(download.examname)) ? GetInitCap(download.examname) : "";
                        
                        var checkPublishDatePrac = (from b in context.Exams
                                                    where b.ID == download.examId && b.DateofPublishingPracticalAdmitCard <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                                    select b).FirstOrDefault();

                        var checkPublishDateTh_offline = (from b in context.Exams
                                                  where b.ID == download.examId && b.DateOfPublishingOfRollNumber <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                                  select b).FirstOrDefault();

                        var checkPublishDateTh_Online = (from b in context.Exams
                                                  where b.ID == download.examId && b.Online_Admit_Card_Publish_Date <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                                  select b).FirstOrDefault();

                        //-- New Code added as require dated on 07082023, checking eligibility for offline admit card 
                        //List<int> data = new List<int> { 938, 939, 940, 941 };
                        List<int> data = new List<int> { 942, 943, 944, 945, 946, 947, 948, 949, 950, 951, 952, 953, 954, 955 };
                        List<int> data1 = (from m in context.CourseExamApplicationDetails where m.CourseID == 2 && m.ExamID == examID && m.RegistrationNumber==registrationNumber select m.ModuleID).ToList();
                        int count = data1.Intersect(data).Count();
                        //--

                        // to check if candidate has applied only for theory modules, for OffLine Admit card.
                        if (download.nooftheorymodule != 0 && checkPublishDateTh_offline != null && CourseID != 1 && ((CourseID == 2 && count != 0) || CourseID != 2))
                        {
                            
                            if (download.rollnumber != null && download.rollnumber != 0 )
                            {
                                Btndownload1.Visible = false;
                                Btndownload.Visible = true;
                                Btndownload.Text = courseID != 2 ? "Print Theory Exam Admit Card" : "Print Theory OffLine Exam Admit Card";
                                trpractical.Visible = false;
                                trtheory.Visible = true;
                                Lblexamcentre.Text = (!string.IsNullOrEmpty(download.examcentre)) ? download.examcentre.ToUpper() : "";
                                Btndownload.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CourseAdmitCard_Ver8.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + download.Appid) + "','Form'); return false;");
                                lbmessage.Visible = true;
                                Lberror1.Visible = false;
                            }
                            else
                            {
                                Lberror1.Visible = true;
                                Lberror1.Text = "Your OffLine Theory Exam Admit Card has not been uploaded till date.Please contact NIELIT office regarding this.";
                            }
                        }

                       

                        //-- New Code added as require dated on 07082023, checking eligibility for Online admit card 
                         List<int> data2 = new List<int> { 938, 939, 940, 941 };
                        //List<int> data2 = new List<int> { 942, 943, 944, 945, 946, 947, 948, 949, 950, 951, 952, 953, 954, 955 };
                        List<int> data3 = (from m in context.CourseExamApplicationDetails where m.CourseID == 2 && m.ExamID == examID && m.RegistrationNumber == registrationNumber select m.ModuleID).ToList();
                        int count1 = data3.Intersect(data2).Count();
                        //--

                        // to check if candidate has applied only for theory modules, for OnLine Admit card.
                        if (download.nooftheorymodule != 0 && checkPublishDateTh_Online != null && (CourseID == 1 || (CourseID == 2 && count1 != 0)))
                        {
                            if (download.rollnumber != null && download.rollnumber != 0)
                            {
                                Btndownload1.Visible = false;
                                Btndownload2.Visible = true;
                                Btndownload2.Text = "Print OnLine Theory Exam Admit Card";
                                trpractical.Visible = false;
                                trtheory.Visible = true;
                                Lblexamcentre.Text = (!string.IsNullOrEmpty(download.examcentre)) ? download.examcentre.ToUpper() : "";
                                Btndownload2.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/OnlineCourseAdmitCard_Ver1.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + download.Appid) + "','Form'); return false;");
                                lbmessage.Visible = true;
                                Lberror1.Visible = false;
                            }
                            else
                            {
                                Lberror1.Visible = true;
                                Lberror1.Text = "Your OnLine Theory Exam Admit Card has not been uploaded till date.Please contact NIELIT office regarding this.";
                            }
                        }

                        // to check if candidate has applied only for practical modules.
                        if (download.noofpractmodule != 0 && checkPublishDatePrac != null)
                        {
                            if (download.rollnumber != null && download.rollnumber != 0)
                            {
                                Btndownload1.Visible = true;
                                //Btndownload.Visible = false;
                                trpractical.Visible = true;
                                trtheory.Visible = false;
                                trpractical.Attributes.Add("class", "gdalternate1");
                                Lblpracexamcentre.Text = (!string.IsNullOrEmpty(download.pracexamcentre)) ? download.pracexamcentre.ToUpper() : "";
                                Btndownload1.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CoursePracticalAdmitCard_Ver5.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + download.Appid) + "','Form'); return false;");
                                lbmessage.Visible = true;
                                Lberror1.Visible = false;
                            }
                            else
                            {
                                Lberror1.Visible = true;
                                Lberror1.Text = "Your Practical Examination Admit Card has not been uploaded till date.Please contact NIELIT office regarding this.";
                            }
                        }
                        // to check if candidate has applied both for theory modules and practical modules.
                        //if (download.noofpractmodule != 0 && download.nooftheorymodule != 0 && download.profficerefno != null && checkPublishDatePrac != null && checkPublishDateTh_offline == null)
                        //if (download.noofpractmodule != 0 && download.nooftheorymodule != 0 && checkPublishDatePrac != null)
                        //{
                        //    if (download.rollnumber != 0 && download.rollnumber != null)
                        //    {
                                
                        //        Btndownload1.Visible = true;
                        //       // Btndownload.Visible = true;
                        //        //Btndownload.Text = "Print Theory Examination Admit Card";
                        //        trpractical.Visible = true;
                        //        trtheory.Visible = true;
                        //        Lblexamcentre.Text = (!string.IsNullOrEmpty(download.examcentre)) ? download.examcentre.ToUpper() : "";
                        //        Btndownload.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CourseAdmitCard_Ver8.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + download.Appid) + "','Form'); return false;");

                        //        Lblpracexamcentre.Text = (!string.IsNullOrEmpty(download.pracexamcentre)) ? download.pracexamcentre.ToUpper() : "";
                        //        Btndownload1.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CoursePracticalAdmitCard_Ver4.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + download.Appid) + "','Form'); return false;");
                        //        lbmessage.Visible = true;
                        //    }
                        //    else
                        //    {
                        //        Btndownload.Visible = false;
                        //       // Lberror1.Visible = true;
                        //       // Lberror1.Text = "Your Theory Examination Admit Card has not been uploaded till date.Please contact NIELIT office regarding this.";
                        //        Btndownload1.Visible = true;
                        //        trpractical.Visible = true;
                        //        Lblpracexamcentre.Text = (!string.IsNullOrEmpty(download.pracexamcentre)) ? download.pracexamcentre.ToUpper() : "";
                        //        Btndownload1.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CoursePracticalAdmitCard_Ver4.aspx?ID=" + Request.QueryString["id"].ToString() + "&Appid=" + download.Appid) + "','Form'); return false;");

                        //    }
                        //}
                        Lblerror.Visible = false;
                        tbsearch.Visible = false;
                    }
                    else
                    {
                        Lblerror.Text = "Admit Card not declared for this exam / Invalid Application No. or Date of Birth or Invalid Exam Name.";
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        lbmessage.Visible = false;
                    }
                }
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
        finally 
        { 
            context.Dispose(); 
        }
    }
    protected void Btnback_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            GenerateNewCaptchaImage();
            txtcode.Text = "";
           // Response.Redirect("../WEB/DownloadAdmitCard.aspx?" + Request.QueryString);
		    Response.Redirect("../WEB/allCourses.aspx?" + Request.QueryString);
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
    protected void Rdserachby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (Rdserachby.SelectedValue == "1")
            {
                tdfilter.InnerText = "Enter Application No.";
                spfilter.InnerText = "(Application Number of Exam Form)";
                Txtappno.Text = "";

                TxtDOB.Text = "";
                GenerateNewCaptchaImage();
            }
            else if (Rdserachby.SelectedValue == "2")
            {
                tdfilter.InnerText = "Enter Registration No.";
                spfilter.InnerText = "(Registration Number of the Course)";
                Txtappno.Text = "";

                TxtDOB.Text = "";
                GenerateNewCaptchaImage();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void Ddlexamyear_SelectedIndexChanged(object sender, EventArgs e)
    { 
        bindexams(); 
    }
    
}