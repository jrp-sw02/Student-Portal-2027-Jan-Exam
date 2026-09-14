using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Transactions;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.IO.Compression;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Data.Objects;
using System.Text.RegularExpressions;
using EConnect;


public partial class Admin_NielitCentreStudentMapping : BasePage
{
    int stateid = 0;

    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 entityID = 0;
    Int32 lnkID = 0;
    Int32 UserRefNumber = 0;
    Int32 UserTypeid = 0;
    Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
    Int64 NielitCentrelinkedToCentreId = 0;
    //Added 6 Sep 2021
    Int32 SubcetreIdByApplication = 0;
    String NameCenter = "";
    protected void Page_Load(object sender, EventArgs e)
    {


        if (Request.UrlReferrer != null && Request.QueryString["OnlineRefNo"] != null)
        {
            string previousPageUrl = Request.UrlReferrer.AbsoluteUri;
            string Refno = Request.QueryString["OnlineRefNo"];
            txtrefno.Text = Refno.Trim();
            btnShow_Click(btnShow, null);
            // string previousPageName = System.IO.Path.GetFileName(Request.UrlReferrer.AbsolutePath);
        }
        else
        {

            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            try
            {
               // if (IsSessionAlive() == false)
                //    Response.Redirect("../Index.aspx");
                currentRoleId = Convert.ToInt32(Session["RoleID"]); //25
                loginUserNo = Convert.ToInt32(Session["UserID"]); //443910
                entityID = Convert.ToInt32(Session["EntityID"]);
                UserTypeid = Convert.ToInt32(Session["UserType"]); //10
                //loginUserNo=443910;
                //UserTypeid =10;

              //  if (!UserManager.HasRight(currentRoleId, enmRight.View))
               // {
              //      Response.Write("Sorry! You don't have rights  to view this page");
              //      Response.End();
              //  }
                User objUser1;
                using (EConnectContext context = new EConnectContext())
                {
                    objUser1 = new EConnect.URM.User();

                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);


                }

                Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                if (!Page.IsPostBack)
                {
                    User objUser;

                    using (EConnectContext context = new EConnectContext())
                    {
                        objUser = new EConnect.URM.User();

                        User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                        UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);

                        using (NIELITMISContext context1 = new NIELITMISContext())
                        {

                            if (UserTypeid == 10)
                            {
                                ListItem lst = new ListItem("--Select One--", "0");
                                var Center = from t in context1.NielitCentres
                                             where t.ID == UserRefNumber
                                             orderby (t.Name)
                                             select new { ValueField = t.ID, TextField = t.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, Center, lst);

                                var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                                lnkID = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                                if (lnkID != 0)
                                {
                                    NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                                    txtInstitute.Text = intitutesName.Name;
                                    Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                    RdoAffInstOrNonAffInst.SelectedValue = "2";
                                    // ddlSubcentreName.Enabled = false;
                                }
                                else
                                {
                                    NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                                    txtInstitute.Text = intitutesName.Name;
                                    Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                    RdoAffInstOrNonAffInst.SelectedValue = "2";
                                    ddlCenter.SelectedValue = NielitCentreId.ToString();
                                    ddlCenter.Enabled = false;
                                    ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                                }
                            }
                            else if (UserTypeid == 11)
                            {
                                var intituteslinkedToCentre = context1.NonAffInstitutes.Find(loginUser.UserRefNumber);
                                lnkID = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                                NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                                if (institutesName != null)
                                {
                                    txtInstitute.Text = institutesName.Name;
                                    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                }
                                RdoAffInstOrNonAffInst.Items.RemoveAt(2);
                                FillddlcentreName();
                                RdoAffInstOrNonAffInst.Items.RemoveAt(0);
                                ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                            }
                            else if (UserTypeid == 4)
                            {
                                var intituteslinkedToCentre = from s in context1.AffInstitutes
                                                              where s.instituteID == loginUser.UserRefNumber
                                                              select new { ID = s.ID, linkedToCentre = s.linkedToCentre };

                                if (intituteslinkedToCentre.Count() == 0)
                                {
                                    ShowAlert("Menu is not available for the institute");
                                    return;
                                }
                                NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().ID);
                                NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
                                lnkID = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
                                NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                                if (institutesName != null)
                                {
                                    txtInstitute.Text = institutesName.Name;
                                    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                }
                                RdoAffInstOrNonAffInst.Items.RemoveAt(2);
                                FillddlcentreName();
                                RdoAffInstOrNonAffInst.Items.RemoveAt(1);

                            }
                        }
                        //Added
                        if (ddlWhetherPlaced.SelectedValue.ToString() == "1")
                            btn.Visible = true;
                        if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                        {
                            BindGender();
                            BindEditNewModeData();

                        }
                        else
                        {
                            BindGender();
                            BindEditNewModeData();
                            ViewState["SortField"] = "";
                            ViewState["SortOrder"] = "";

                            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                            {
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Nielit Centre Student", "Admin/NielitCentreStudentEdit.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                            }
                            else
                            {
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Nielit Centre Student", "Admin/NielitCentreStudentEdit.aspx", ""));
                            }

                        }
                        if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                            ShowAlert(Request.QueryString["msg"].ToString());
                    }
                }
                BreadCrumb1.Render();

            }

            catch (Exception ex)
            {
                ShowAlert(ex.Message, true);
            }
        }
    }

    protected void FillddlcentreName()
    {
        try
        {
            User objUser;
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                ListItem lst1 = new ListItem("--Select One--", "99");
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);


                using (NIELITMISContext context = new NIELITMISContext())
                {
                    NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                    txtInstitute.Text = institutesName.Name;
                    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                    ddlCenter.ClearSelection();
                    if (UserTypeid == 11)
                    {
                        var centreName = from s in context.NonAffInstitutes
                                         where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                         select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                        ddlCenter.Enabled = false;
                        var NonAfflcentre = (from p in context.NonAffInstitutes
                                             where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                             select p).FirstOrDefault();
                        ddlCenter.SelectedValue = NonAfflcentre.ID.ToString();
                        RdoAffInstOrNonAffInst.SelectedValue = "0";
                        ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                    }
                    else
                    {
                        var centreName = from s in context.AffInstitutes
                                         where s.linkedToCentre == NelitCentreLinkId && s.instituteID == loginUser.UserRefNumber
                                         select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                        ddlCenter.Enabled = false;
                        var NonAfflcentre = (from p in context.AffInstitutes
                                             where p.linkedToCentre == NelitCentreLinkId && p.instituteID == loginUser.UserRefNumber
                                             select p).FirstOrDefault();
                        ddlCenter.SelectedValue = NonAfflcentre.instituteID.ToString();
                        RdoAffInstOrNonAffInst.SelectedValue = "1";
                        ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void RdoAffInstOrNonAffInst_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlCourse.ClearSelection();
            ddlBatch.ClearSelection();
            ddlCourse.Items.Clear();
            ddlBatch.Items.Clear();
            User objUser;
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context2 = new NIELITMISContext())
                {
                    if (UserTypeid == 10)
                    {
                        var instituteslinkedToCentre = context2.NielitCentres.Find(loginUser.UserRefNumber);
                        lnkID = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (UserTypeid == 11)
                    {
                        var instituteslinkedToCentre = context2.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        lnkID = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (lnkID != 0)
                    {
                        NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                        txtInstitute.Text = institutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        using (NIELITMISContext context = new NIELITMISContext())
                        {
                            ListItem lst1 = new ListItem("--Select One--", "99");
                            if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                            {
                                ddlCenter.ClearSelection();
                                var centreName = from s in context.AffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                ddlCenter.Enabled = true;
                            }
                            else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                            {
                                if (UserTypeid == 11)//Non AffInstitutes by user refNumber
                                {
                                    ddlCenter.ClearSelection();
                                    var centreName = from s in context.NonAffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                    var NonAfflcentre = (from p in context.NonAffInstitutes
                                                         where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                                         select p).FirstOrDefault();
                                    ddlCenter.SelectedValue = NonAfflcentre.ID.ToString();
                                }
                                else //NonAffInstitutes for Nielit Centres
                                {
                                    ddlCenter.ClearSelection();
                                    var centreName = from s in context.NonAffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                    ddlCenter.Enabled = true;
                                }
                                if (UserTypeid == 4)//AffInstitutes by user refNumber
                                {
                                    ddlCenter.ClearSelection();
                                    var centreName = from s in context.AffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId && s.instituteID == loginUser.UserRefNumber
                                                     select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                    //ddlCenter.Enabled = false;
                                    var Afflcentre = (from p in context.AffInstitutes
                                                      where p.linkedToCentre == NelitCentreLinkId && p.instituteID == loginUser.UserRefNumber
                                                      select p).FirstOrDefault();
                                    ddlCenter.SelectedValue = Afflcentre.instituteID.ToString();
                                }
                                else // AffInstitutes for Nielit Centres
                                {
                                    ddlCenter.ClearSelection();
                                    var centreName = from s in context.AffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId
                                                     select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                    ddlCenter.Enabled = true;
                                }
                            }
                            else
                            {
                                ddlCenter.Items.Add(new ListItem("--Select One--", "99"));
                                ddlCenter.SelectedValue = "99";
                                //ddlCenter.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                        txtInstitute.Text = institutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        using (NIELITMISContext context = new NIELITMISContext())
                        {
                            ListItem lst1 = new ListItem("--Select One--", "99");
                            if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                            {
                                ddlCenter.ClearSelection();
                                var centreName = from s in context.AffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                ddlCenter.Enabled = true;
                            }
                            else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                            {
                                ddlCenter.ClearSelection();
                                ddlCenter.Items.Clear();
                                var centreName = from s in context.NonAffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.ID, TextField = s.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                                ddlCenter.Enabled = true;
                            }

                            else
                            {
                                ddlCenter.ClearSelection();

                                var Center = from t in context.NielitCentres
                                             where t.ID == UserRefNumber
                                             orderby (t.Name)
                                             select new { ValueField = t.ID, TextField = t.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, Center, lst1);

                                ddlCenter.SelectedValue = NielitCentreId.ToString();
                                ddlCenter.Enabled = false;
                                ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);

                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }
    }
    protected void BindEditNewModeData()
    {
        try
        {
            bindMaritalStatus();
            bindCastCategory();
            bindReligion();
            bindState();
            //bindCourses();
            //bindBatches();
            //bindProject();
            //bindCenter();
            bindCompanyName();
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");

                //var Courses = from s in context.NielitCentreCourses
                //              where s.ShowOnWeb == true && s.IsActive == true
                //              orderby (s.Name)
                //              select new { ValueField = s.ID, TextField = s.Name };
                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, Courses, lst);

                //var Batch = from s in context.NielitCentreBatchs
                //            where s.Show_On_Web == true && s.IsActive == true
                //            orderby (s.Name)
                //            select new { ValueField = s.ID, TextField = s.Name };
                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);

                var CorState = (from s in context.Locations
                                orderby (s.Name)
                                where s.LocationTypeID == 2
                                && s.ParentLocationID == 1
                                select new { ValueField = s.ID, TextField = s.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCmpState, CorState, lst);

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillCourses(DropDownList ddl, Int32 courseCategoryID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ddl.Items.Clear();
                ListItem lst = new ListItem("--All--", "0");
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == courseCategoryID
                                 orderby p.DisplayOrder
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, CourseList, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        NIELITMISContext context = new NIELITMISContext();
        try
        {
            if (count <= 0)
                count = 10;
            //Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();

            var Examcentre = from s in context.NielitCentreStudent
                             select new { Name = s.Name };

            if (!String.IsNullOrEmpty(searchString))
            {
                Examcentre = Examcentre.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            Examcentre = Examcentre.OrderBy(s => s.Name);

            foreach (var c in Examcentre)
            {
                items.Add(c.Name);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("NielitCentreStudentEdit.aspx", true);

    }

    protected void bindPaymentOption()
    {
        using (EConnectContext context = new EConnectContext())
        {
            Course currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["id"]));
            string CourseCatg = currentCourse.CourseCategory.Name.ToString();
            //if (CourseCatg == "IRDA" && RdoUndergngDOEACC.SelectedValue == "I")
            //{ ddlPaymentOption.Items.RemoveAt(0); }
            //if (CourseCatg == "Digital Literacy Course" && RdoUndergngDOEACC.SelectedValue == "I")
            //{ ddlPaymentOption.Items.RemoveAt(0); }
        }
    }

    protected void bindExamName(Int32 ApplicantTypeId)
    {
        try
        {
            Int32 courseID = 0;
            Int32 NormalactivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
            Int32 LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee);
            using (EConnectContext context = new EConnectContext())
            {
                Int32 examID = 0;
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    courseID = Convert.ToInt32(Request.QueryString["id"]);
                }
                else if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                {
                    Int64 appID = Convert.ToInt64(Request.QueryString["Appid"]);
                    var app = context.CourseRegistrationApplications.Find(appID);
                    courseID = app.CourseID;

                }
                var LateFeeExam = (from e in context.CutOffDates
                                   join i in context.Exams on e.ExamID equals i.ID
                                   where e.CourseID == courseID
                                   && e.ApplicantTypeID == ApplicantTypeId
                                   && e.ActivityID == LateFeeActivityId
                                   && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                   orderby e.EfferctiveDate ascending
                                   select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
                if (LateFeeExam.Count() > 0)//If  applicable for late fee ?
                {

                    if (LateFeeExam != null)
                    {
                        //LblExamName.Text = LateFeeExam.FirstOrDefault().ExamName;
                        examID = LateFeeExam.FirstOrDefault().ExamID;
                        btnSave.Visible = true;
                    }
                }
                else if (LateFeeExam.Count() <= 0)//If  not applicable for late fee ?
                {

                    var NormalFeeExam = (from e in context.CutOffDates
                                         join i in context.Exams on e.ExamID equals i.ID
                                         where e.CourseID == courseID
                                         && e.ApplicantTypeID == ApplicantTypeId
                                         && e.ActivityID == NormalactivityId
                                         && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                         orderby e.EfferctiveDate ascending
                                         select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
                    if (NormalFeeExam.Count() > 0)
                    {
                        //LblExamName.Text = NormalFeeExam.FirstOrDefault().ExamName;
                        examID = NormalFeeExam.FirstOrDefault().ExamID;
                        btnSave.Visible = true;
                    }

                }
                ViewState["ExamID"] = examID.ToString();
                //ShowFeeDetail(examID);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ShowData()
    {
        try
        {
            if (!String.IsNullOrEmpty(Request.QueryString["key"]))
            {
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    Int64 applID = Convert.ToInt64(Request.QueryString["key"]);
                    //This Query will get all the information of Applied Canditate by generated Application id                                     
                    var application = context.NielitCentreStudent.Find(applID);


                    //var instituteDetail = (from a in context.AccreditationDetails
                    //                       join i in context.Institutes
                    //                           on a.InstituteID equals i.ID
                    //                       where i.ID == application.InstituteID
                    //                       select new
                    //                       {
                    //                           Name = i.Name,
                    //                           CentreID = i.ID,
                    //                           StateId = i.StateID,
                    //                           DistrictId = i.DistrictID,
                    //                           StateName = i.State.Name,
                    //                           DistrictName = i.District.Name,
                    //                           AccNo = a.AccreditationNumber
                    //                       }).FirstOrDefault();

                    //RdoUndergngDOEACC.SelectedValue = "I";


                    //DdlAccState.SelectedValue = Convert.ToString(instituteDetail.StateId);
                    //int id = Convert.ToInt32(DdlAccState.SelectedValue);
                    //DdlAccState_SelectedIndexChanged(DdlAccState, EventArgs.Empty);
                    //DdlAccCentre.SelectedValue = Convert.ToString(instituteDetail.CentreID);
                    //}
                    //else
                    //{
                    //RdoUndergngDOEACC.SelectedValue = "D";
                    //TxtExperienceInYears.Text = application.ExperienceInYears.ToString();
                    //}
                    // ddlPaymentOption.SelectedValue = application.PaymentSourceID.HasValue ? application.PaymentSourceID.Value.ToString() : (RdoUndergngDOEACC.SelectedValue == "D" ? "1" : "2");
                    //Candidate Personal Detail...

                    ddlSalutaionName.SelectedValue = application.Salutation.ToString();
                    txtAppName.Text = GetInitCap(application.Name.ToString());
                    lblname.Text = txtAppName.Text;

                    ddlCenter.SelectedValue = application.InstituteID.ToString();

                    if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName) == true)
                    {
                        trfather.Visible = true;
                        trmother.Visible = true;
                        trguardian.Visible = false;
                        Rdoownertype.SelectedValue = "P";
                        Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                        txtFatherName.Text = string.IsNullOrEmpty(application.FatherName) == false && !string.IsNullOrWhiteSpace(application.FatherName) ? GetInitCap(application.FatherName) : "";
                        txtMotherName.Text = string.IsNullOrEmpty(application.MotherName) == false && !string.IsNullOrWhiteSpace(application.MotherName) ? GetInitCap(application.MotherName) : "";
                    }
                    else
                    {
                        trfather.Visible = false;
                        trmother.Visible = false;
                        trguardian.Visible = true;
                        Rdoownertype.SelectedValue = "G";
                        Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                        TxtGuardianName.Text = GetInitCap(application.GuardianName);
                    }

                    ddl_gender.SelectedValue = application.Gender;
                    txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");
                    //txtBodyMark.Text = string.IsNullOrEmpty(application.BodyMark) == false && !string.IsNullOrWhiteSpace(application.BodyMark) ? application.BodyMark : "";
                    ddlCategory.SelectedValue = application.CastCategoryID.ToString();
                    ddlMStatus.SelectedValue = application.MaritalStatusID.ToString();
                    if (application.IsHandicaped.ToString() == "True")
                    {
                        Rdhandicapped.SelectedValue = "Y";
                    }
                    else
                    {
                        Rdhandicapped.SelectedValue = "N";
                    }
                    if (application.IsExServicemane.ToString() == "True")
                    {
                        Rdexserviceman.SelectedValue = "Y";
                    }
                    else
                    {
                        Rdexserviceman.SelectedValue = "N";
                    }
                    ddlReligion.SelectedValue = application.ReligionID.ToString();
                    //Candidate Contact Detail...

                    TxtSTDcode.Text = application.PhoneNumber.HasValue && application.PhoneNumber != 0 ? "0" + application.StdNumber.ToString() : "";
                    txtCorPhoneNo.Text = application.PhoneNumber.ToString();
                    txtCorMobileNo.Text = application.MobileNumber.ToString();
                    txtEmailId.Text = application.EmailAddress.ToString();
                    RdisEWS.SelectedValue = application.Is_EWS.ToString();

                    //aadhar details
                    //if (application.AadharNumber.HasValue)
                    //    txtaadhar.Text = application.AadharNumber.Value.ToString();
                    //else
                    //    txtaadhar.Text = "";


                    //Candidate Address Detail...
                    TxtPerAddressLine1.Text = string.IsNullOrEmpty(application.PerAddressLine1) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine1) ? GetInitCap(application.PerAddressLine1) : "";
                    TxtPerAddressLine2.Text = string.IsNullOrEmpty(application.PerAddressLine2) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine2) ? GetInitCap(application.PerAddressLine2) : "";
                    TxtPerAddressLine3.Text = string.IsNullOrEmpty(application.PerAddressLine3) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine3) ? GetInitCap(application.PerAddressLine3) : "";
                    TxtPerCity.Text = string.IsNullOrEmpty(application.PerCityName) == false && !string.IsNullOrWhiteSpace(application.PerCityName) ? GetInitCap(application.PerCityName) : "";
                    ddlPState.SelectedValue = application.PerStateID != null && application.PerStateID != 0 ? application.PerStateID.ToString() : "0";
                    int id1 = Convert.ToInt32(ddlPState.SelectedValue);
                    ddlPState_SelectedIndexChanged(ddlPState, EventArgs.Empty);
                    ddlPdistrict.SelectedValue = application.PerDistrictID.ToString();
                    TxtPpincode.Text = application.PerPinCode.ToString();

                    TxtCorAddressLine1.Text = string.IsNullOrEmpty(application.CorAddressLine1) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine1) ? GetInitCap(application.CorAddressLine1) : "";
                    TxtCorAddressLine2.Text = string.IsNullOrEmpty(application.CorAddressLine2) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine2) ? GetInitCap(application.CorAddressLine2) : "";
                    TxtCorAddressLine3.Text = string.IsNullOrEmpty(application.CorAddressLine3) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine3) ? GetInitCap(application.CorAddressLine3) : "";
                    TxtCorCity.Text = string.IsNullOrEmpty(application.CorCityName) == false && !string.IsNullOrWhiteSpace(application.CorCityName) ? GetInitCap(application.CorCityName) : "";
                    ddlCorState.SelectedValue = application.CorStateID.ToString();
                    int id2 = Convert.ToInt32(ddlCorState.SelectedValue);
                    ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                    ddlcordistrict.SelectedValue = application.CorDistrictID.ToString();
                    txtCorPinCode.Text = application.CorPinCode.ToString();

                    //btnback.Visible = false;
                };

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void UpdateData()
    {
        try
        {
            // UpdateData1();
            DataTable dt = new DataTable();
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString);
            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            cnn.Open();
            string name = txtAppName.Text;
            string fathername = txtFatherName.Text;
            string gaurdian = TxtGuardianName.Text;
            DateTime dob = Convert.ToDateTime(txtDob.Text);
            Int64 courseid = Convert.ToInt64(ddlCourse.SelectedValue.Trim());
            EConnectContext context1 = new EConnectContext();

            using (NIELITMISContext context = new NIELITMISContext())
            {
                string Number = txtrefno.Text;
                using (SqlCommand cmd = new SqlCommand("select ID  FROM [NIELITMIS].[dbo].[NielitCentreStudent] where Number= @Number"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.AddWithValue("@Number", Number);
                        sda.SelectCommand = cmd;

                        sda.Fill(dt);


                    }
                }
                if (rdbCourseType.SelectedValue.Trim() == "R")
                {
                    Int64 courseregno = Convert.ToInt64(txtRegNo.Text);
                    var checkRegno = (from i in context1.RegistrationDetails
                                      where i.RegistrationNo == courseregno
                                      select new
                                      {
                                          Id = i.ID
                                      }).FirstOrDefault();
                    if (checkRegno != null)
                    {
                        var checkRegdetails = (from a in context1.CourseRegistrationApplications
                                               join i in context1.RegistrationDetails
                                                   on a.CandidateID equals i.CandidateID
                                               where
                                               a.Name == name &&
                                                 a.GuardianName==null ? a.FatherName == fathername : a.GuardianName == gaurdian
                                               // (a.GuardianName == gaurdian || a.FatherName == fathername)
                                               && a.DateOfBirth == dob
                                               && a.CourseID == courseid

                                               select new
                                               {
                                                   Id = a.ID
                                               }).FirstOrDefault();
                        if (checkRegdetails == null)
                        {
                            lblerror.Text = "Registration/Application details mismatch with candidate data entered in MIS Portal.";
                            return;
                        }
                    }
                    else
                    {
                        lblerror.Text = "Registration/Application No. does not exist.";
                        return;
                    }
                }

                else if (rdbCourseType.SelectedValue.Trim() == "A")
                {
                    string appno = (txtRegNo.Text);
                    var checkRegno = (from a in context1.CertificateExamApplications
                                      where a.Number == appno

                                      select new
                                      {
                                          Id = a.ID

                                      }).FirstOrDefault();
                    if (checkRegno != null)
                    {
                        var checkRegdetails = (from a in context1.CertificateExamApplications

                                               where a.Name == name && (a.GuardianName == gaurdian || a.FatherName == fathername) && a.DateOfBirth == dob
                                               && a.CourseID == courseid

                                               select new
                                               {
                                                   Id = a.ID

                                               }).FirstOrDefault();
                        if (checkRegdetails == null)
                        {
                            lblerror.Text = "Registration/Application details mismatch with candidate data entered in MIS Portal.";
                            return;
                        }
                    }
                    else
                    {
                        lblerror.Text = "Registration/Application No. does not exist.";
                        return;
                    }
                }
                Int64 applID = Convert.ToInt64(dt.Rows[0]["ID"].ToString());

                var objRegistration = context.NielitCentreStudent.Find(applID);


                if (rdbCourseType.SelectedValue.Trim() == "R" && objRegistration.RegisteredCourseRegistrationNo == null) // added on 29-01-2026
                {
                    objRegistration.AlreadyRegistered = true; 
                    objRegistration.RegisteredCourseID = Convert.ToInt32(ddlCourse.SelectedValue);
                    objRegistration.RegisteredCourseRegistrationNo = Convert.ToInt32(txtRegNo.Text);
                }
                else if (rdbCourseType.SelectedValue.Trim() == "A" && objRegistration.RegisteredCourseRegistrationNo == null) // added on 29-01-2026
                {
                    objRegistration.AlreadyRegistered = true;
                    objRegistration.RegisteredCourseID = Convert.ToInt32(ddlCourse.SelectedValue);
                }

                context.Entry(objRegistration).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                long Appid = objRegistration.ID;
                UpdateOnlineRefId();
                NIELITStudentCentreHistory();
                strMessage = "Record Updated Successfully.";

                Response.Redirect("NielitCentreStudentMapping.aspx?msg=" + strMessage, true);
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void SaveData()
    {
        try
        {
            Boolean success = false;
            string AfflinstituteLinkCentreName = "";
            //Added for refNo
            EConnectContext context1 = new EConnectContext();
            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();

            if (UserTypeid == 4)
            {
                NIELITMISContext context2 = new NIELITMISContext();
                var intituteslinkedToCentre = from s in context2.AffInstitutes
                                              where s.instituteID == loginUser.UserRefNumber
                                              select new { ID = s.ID, linkedToCentre = s.linkedToCentre };

                NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().ID);
                //HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
                lnkID = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
                NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                if (institutesName != null)
                {
                    txtInstitute.Text = institutesName.Name;
                    AfflinstituteLinkCentreName = institutesName.Name;
                    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);

                }
            }

            //CheckDuplicate AAdhaar for Fee reimbursement scheme
            if (ddlProcname.SelectedValue.ToString() == "2")
            {
                bool duplicateAAdhaar = false;
                duplicateAAdhaar = checkDuplicateAAdhaar();
                if (duplicateAAdhaar)
                {
                    ShowAlert("Aadhar already enrolled in some other running batch, Cannot register");
                    return;
                }
            }
            //
            //
            //else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (NIELITMISContext context = new NIELITMISContext())
                    {
                        //int courseid = Convert.ToInt32(Request.QueryString["id"]);
                        if (UserTypeid == 4)
                        {
                            NameCenter = AfflinstituteLinkCentreName;
                        }
                        else
                        {
                            NielitCentres nielitcentre = context.NielitCentres.Find(loginUser.UserRefNumber);
                            NameCenter = nielitcentre.Name;
                        }
                        NielitCentreStudent objRegistration;
                        string aff = RdoAffInstOrNonAffInst.SelectedValue.ToString();
                        string affVal = "";
                        if (aff == "0")
                            affVal = "N";
                        if (aff == "1")
                            affVal = "Y";
                        if (aff == "2")
                            affVal = "O";
                        //NielitCentreStudent objRegistration = new EConnect.NIELIT.NielitCentreStudent();
                        objRegistration = new NielitCentreStudent();
                        string CheckAffOrNot = RdoAffInstOrNonAffInst.SelectedValue.ToString();

                        objRegistration.whetherAffiliated = affVal;
                        objRegistration.CourseID = Convert.ToInt64(ddlCourse.SelectedValue);
                        objRegistration.batch_ID = Convert.ToInt64(ddlBatch.SelectedValue);
                        objRegistration.InstituteID = Convert.ToInt64(ddlCenter.SelectedValue);
                        //User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                        //objRegistration.InstituteID = loginUser.UserRefNumber;




                        objRegistration.ApplicationDate = DateTime.Now;


                        objRegistration.AlreadyRegistered = false;

                        // objRegistration.AlreadyRegistered = false;
                        objRegistration.AlreadyQualified = false;
                        objRegistration.Roll_No = null;

                        string salutation = ddlSalutaionName.SelectedItem.Text;
                        salutation = salutation.Substring(0, salutation.IndexOf('/'));

                        if (ddlWheatherPojectStu.SelectedValue == "1")
                        {

                            objRegistration.whetherProjectStudent = true;
                            objRegistration.projectId = Convert.ToInt64(ddlProcname.SelectedValue);
                        }
                        else
                        {
                            objRegistration.whetherProjectStudent = false;
                            //objRegistration.projectId = Convert.ToInt64(ddlProcname.SelectedValue);
                        }

                        objRegistration.Salutation = salutation.Trim();
                        objRegistration.Name = txtAppName.Text.Trim().ToString();
                        if (Rdoownertype.SelectedValue == "P")
                        {
                            objRegistration.FatherName = txtFatherName.Text.Trim();
                            objRegistration.MotherName = txtMotherName.Text.Trim();
                            objRegistration.GuardianName = null;
                        }
                        else if (Rdoownertype.SelectedValue == "G")
                        {
                            objRegistration.GuardianName = TxtGuardianName.Text.Trim();
                            objRegistration.FatherName = null;
                            objRegistration.MotherName = null;
                        }

                        string gender = ddl_gender.SelectedValue;
                        objRegistration.Gender = gender.Trim();
                        //gender = gender.Substring(0, gender.IndexOf('/'));
                        //objRegistration.Gender = gender.Trim();
                        if (RdisEWS.SelectedValue == "Y")
                            objRegistration.Is_EWS = true;
                        else
                            objRegistration.Is_EWS = false;
                        objRegistration.MaritalStatusID = Convert.ToInt32(ddlMStatus.SelectedValue);
                        objRegistration.DateOfBirth = Convert.ToDateTime(txtDob.Text.ToString());
                        objRegistration.CastCategoryID = Convert.ToInt32(ddlCategory.SelectedValue);
                        if (Rdhandicapped.SelectedValue == "Y")
                        {
                            objRegistration.IsHandicaped = true;
                        }
                        else
                        {
                            objRegistration.IsHandicaped = false;
                        }
                        if (Rdexserviceman.SelectedValue == "Y")
                        {
                            objRegistration.IsExServicemane = true;
                        }
                        else
                        {
                            objRegistration.IsExServicemane = false;
                        }


                        //objRegistration.BodyMark = txtBodyMark.Text;

                        //Contact Details
                        if (String.IsNullOrEmpty(TxtSTDcode.Text) == false && string.IsNullOrEmpty(txtCorPhoneNo.Text) == false)
                        {
                            objRegistration.StdNumber = Convert.ToInt32(TxtSTDcode.Text);
                            objRegistration.PhoneNumber = Convert.ToInt32(txtCorPhoneNo.Text);
                        }
                        objRegistration.MobileNumber = Convert.ToInt64(txtCorMobileNo.Text);
                        objRegistration.EmailAddress = txtEmailId.Text;


                        //Permanent Address Details
                        objRegistration.PerAddressLine1 = TxtPerAddressLine1.Text;
                        objRegistration.PerAddressLine2 = TxtPerAddressLine2.Text;
                        objRegistration.PerAddressLine3 = TxtPerAddressLine3.Text;
                        objRegistration.PerCountryID = 0;
                        objRegistration.PerCityName = TxtPerCity.Text;
                        objRegistration.PerStateID = Convert.ToInt32(ddlPState.SelectedValue);
                        objRegistration.PerDistrictID = Convert.ToInt32(ddlPdistrict.SelectedValue);
                        objRegistration.PerPinCode = Convert.ToInt32(TxtPpincode.Text);


                        //Correspondence Address Details
                        if (chkSame.Checked)
                        {
                            objRegistration.CorAddressLine1 = TxtPerAddressLine1.Text;
                            objRegistration.CorAddressLine2 = TxtPerAddressLine2.Text;
                            objRegistration.CorAddressLine3 = TxtPerAddressLine3.Text;
                            objRegistration.CorCountryID = 0;
                            objRegistration.CorCityName = TxtPerCity.Text;
                            objRegistration.CorStateID = Convert.ToInt32(ddlPState.SelectedValue);
                            objRegistration.CorDistrictID = Convert.ToInt32(ddlPdistrict.SelectedValue);
                            objRegistration.CorPinCode = Convert.ToInt32(TxtPpincode.Text);
                        }
                        else
                        {
                            objRegistration.CorAddressLine1 = Server.HtmlEncode(TxtCorAddressLine1.Text);
                            objRegistration.CorAddressLine2 = TxtCorAddressLine2.Text;
                            objRegistration.CorAddressLine3 = TxtCorAddressLine3.Text;
                            objRegistration.CorCountryID = 0;
                            objRegistration.CorCityName = TxtCorCity.Text;
                            objRegistration.CorStateID = Convert.ToInt32(ddlCorState.SelectedValue);
                            objRegistration.CorDistrictID = Convert.ToInt32(ddlcordistrict.SelectedValue);
                            objRegistration.CorPinCode = Convert.ToInt32(txtCorPinCode.Text);
                        }

                        objRegistration.IsVerifiedByInstitute = false;
                        DateTime Dob = Convert.ToDateTime(txtDob.Text);
                        Int32 ReligionId = Convert.ToInt32(ddlReligion.SelectedValue);
                        Int32 castCategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                        Boolean IsHandicapped = Rdhandicapped.SelectedValue == "Y" ? true : false;

                        objRegistration.ReligionID = ReligionId;

                        //aadhar details
                        //if (!String.IsNullOrEmpty(txtaadhar.Text))
                        //    objRegistration.AadharNumber = Convert.ToInt64(txtaadhar.Text);
                        //else
                        //    objRegistration.AadharNumber = null;

                        objRegistration.UIDType = Convert.ToInt32(UidTypeDdl.SelectedValue);
                        //Added for Aadhar Encryption
                        if (UidTypeDdl.SelectedValue.ToString() == "1")
                            objRegistration.UIDNumber = EConnect.Utils.Security.Encryption.Encrypt(UidNumberTxt.Text.Trim().ToUpper());
                        else
                            objRegistration.UIDNumber = UidNumberTxt.Text.Trim().ToUpper();

                        objRegistration.AadharVerfied = false;

                        objRegistration.whether_Course_Complete = false;
                        objRegistration.whether_Certificate_Issued = false;
                        objRegistration.certificate_Issue_Date = null;
                        objRegistration.whetherPlaced = false;
                        objRegistration.company_Name = 0;
                        objRegistration.comapny_Address = "";
                        objRegistration.comapny_Address2 = "";
                        objRegistration.comapny_Address3 = "";
                        objRegistration.CompnyCityName = "";
                        objRegistration.CompnyCountry_ID = 0;
                        objRegistration.compnyState_ID = 0;
                        objRegistration.compnyDistrict_ID = 0;
                        objRegistration.CompnyPinCode = 0;
                        objRegistration.affidavit_Date = null;
                        objRegistration.affidavit_No = "";
                        objRegistration.affidavit_Verified = false;

                        objRegistration.FinalSubmissionDate = DateTime.Now;
                        objRegistration.FinalSubmitted = true;
                        objRegistration.enter_By = Convert.ToInt32(Session["UserID"]);
                        objRegistration.enter_Date = DateTime.Now;

                        context.NielitCentreStudent.Add(objRegistration);
                        context.SaveChanges();

                        long Appid = objRegistration.ID;
                        success = true;
                        if (success == true)
                        {

                            objRegistration = context.NielitCentreStudent.Find(Convert.ToInt32(Appid));
                            objRegistration.Number = NameCenter + "-" + Appid.ToString();
                            context.SaveChanges();
                            scope.Complete();
                            // strMessage = "New record saved. Reference No. for Student Registered : " + NameCenter + "-" + Appid.ToString();
                        }

                        Response.Redirect("NielitCentreStudentEdit.aspx?msg=" + strMessage, true);
                    };

                    //scope.Complete();
                };
            }
        }
        catch (DbEntityValidationException e)
        {
            foreach (var eve in e.EntityValidationErrors)
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

            throw e;
        }
    }

    protected void GenerateNewCaptchaImage()
    {
        try
        {
            ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateRandomNumber(6);
            EConnect.CaptchaImage captcha = new CaptchaImage(ViewState["CaptchCode"].ToString(), 200, 50, "Century Schoolbook");
            imgcap.Src = captcha.ImageSource;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    public void UpdateOnlineRefId()
    {
        try
        {
            string Number = Convert.ToString(txtRegNo.Text);
            string OnlineRefNo = txtrefno.Text;
            string type = rdbCourseType.SelectedValue.Trim();
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection Conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Usp_UpdateOnlineRefId", Conn))
                {
                    Conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@number", Number);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@pOnlineRefNo", OnlineRefNo);
                    cmd.ExecuteNonQuery();
                    Conn.Close();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected bool isSelected(DropDownList Dropdown)
    {
        try
        {
            if (Dropdown.SelectedValue == "0")
            {
                Dropdown.Focus();
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool isBlank(TextBox txtBox)
    {
        try
        {
            if (txtBox.Text.Trim() == "")
            {
                txtBox.Focus();
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool isBlankNumber(TextBox txtBox)
    {
        try
        {
            int zero = 0;

            if (txtBox.Text.Trim() == "" || txtBox.Text.Trim() == zero.ToString() || txtBox.Text.Trim() == ".")
            {
                txtBox.Text = "";
                txtBox.Focus();
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected bool isNumber(TextBox txtBox)
    {
        try
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            if (txtBox.Text.Trim() != "")
            {
                if (!regex.IsMatch(txtBox.Text.Trim()))
                {
                    txtBox.Text = "";
                    txtBox.Focus();
                    return false;
                }
                else
                    return true;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool isValidPassingYear(TextBox txtPassingYear, TextBox txtDob)
    {
        try
        {
            Int64 PassingYear = Convert.ToInt64(txtPassingYear.Text);
            string Dobdate = txtDob.Text;
            string[] dobYear = Dobdate.Split('-');


            if (DateTime.Now.Year < PassingYear || PassingYear <= (Convert.ToInt64(dobYear[2]) + 10))
            {
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected bool isValidDob(TextBox txtBox)
    {
        try
        {
            DateTime todaydate = DateTime.Now;
            DateTime Inputdate = Convert.ToDateTime(txtBox.Text);

            int result1 = DateTime.Compare(todaydate, Inputdate);
            int result2 = DateTime.Compare(todaydate.AddYears(-10), Inputdate);

            if (result2 == -1)
            {
                lblerror.Text = "Invalid date of birth";
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindCourse()
    {
        try
        {
            int CourseId = Convert.ToInt32(Convert.ToString(Request.QueryString["id"]));
            int courseCategoryId = 0;
            using (var context = new EConnectContext())
            {
                courseCategoryId = context.Courses.Find(CourseId).CourseCategoryID;
            }
            if (courseCategoryId == 8)
            {
                UIDtr.Visible = true;
                //Aadhaartr.Visible = false;
            }
            else
            {
                UIDtr.Visible = false;
                //Aadhaartr.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void bindMaritalStatus()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var maritalStatus = from p in context.MaritalStatus
                                    orderby (p.DisplayOrder)
                                    select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlMStatus, maritalStatus, lst);
            };

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindCourses()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");

                var Courses = from s in context.NielitCentreCourses
                              where s.IsVerified == true
                              orderby (s.Name)
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, Courses, lst);
            };

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindCenter()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {

                if (UserTypeid == 10)
                {
                    ListItem lst = new ListItem("--Select One--", "0");
                    var Center = from t in context.NielitCentres
                                 where t.ID == UserRefNumber
                                 orderby (t.Name)
                                 select new { ValueField = t.ID, TextField = t.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, Center, lst);
                    //RdoAffInstOrNonAffInst.SelectedValue = "2";
                }
                //else if (UserTypeid == 4)
                //{
                //    ddlCenter.ClearSelection();
                //    var centreName = from s in context.AffInstitutes
                //                     select new { ValueField = s.ID, TextField = s.Name };
                //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst);
                //    //RdoAffInstOrNonAffInst.SelectedValue = "1";


                //}
                //else if (UserTypeid == 11)
                //{
                //    ddlCenter.ClearSelection();
                //    var centreName = from s in context.NonAffInstitutes
                //                     select new { ValueField = s.ID, TextField = s.Name };
                //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst);

                //}
            };

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindCompanyName()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");

                var companyMas = from t in context.companyMasters
                                 orderby (t.company_Name)
                                 select new { ValueField = t.ID, TextField = t.company_Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCompanyName, companyMas, lst);
            };

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void bindBatches()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");

                var Batch = from s in context.NielitCentreBatchs
                            where s.IsVerified == true && //s.enterBy == loginUserNo
                            (s.centreID.ToString().Trim() == UserRefNumber.ToString().Trim() || s.subCentreID.ToString().Trim() == UserRefNumber.ToString().Trim())
                            orderby (s.Name)
                            select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
            };

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    //protected void bindBatches()
    //{
    //    try
    //    {
    //        using (NIELITMISContext context = new NIELITMISContext())
    //        {
    //            ListItem lst = new ListItem("--Select One--", "0");

    //            var Batch = from s in context.NielitCentreBatchs
    //                        where s.IsVerified == true && s.enterBy == loginUserNo
    //                        orderby (s.Name)
    //                        select new { ValueField = s.ID, TextField = s.Name };
    //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
    //        };

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }

    //}

    //protected void bindProject()
    //{
    //    try
    //    {
    //        Int64 cID = Convert.ToInt64(ddlCourse.SelectedValue);
    //        using (NIELITMISContext context = new NIELITMISContext())
    //        {
    //            ListItem lst = new ListItem("--Select One--", "0");

    //            var Proc = from t in context.NielitProjectss
    //                       join k in context.NielitProjCoursess on t.ID equals k.projID
    //                       where k.courseID == cID
    //                       orderby (t.ProjectName)
    //                       select new { ValueField = t.ID, TextField = t.ProjectName };

    //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlProcname, Proc, lst);

    //        };

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }

    //}
    protected void bindProject()
    {
        try
        {
            Int64 cID = Convert.ToInt64(ddlCourse.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");

                if (cID.ToString().Length > 3)
                {
                    var Proc = from t in context.NielitProjectss
                               join k in context.NielitProjCoursess on t.ID equals k.projID
                               join d in context.NielitCourseDurations on k.courseID equals d.ID
                               where d.ID == cID
                               && k.IsActive
                               orderby (t.ProjectName)
                               select new { ValueField = t.ID, TextField = t.ProjectName };

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlProcname, Proc, lst);
                }
                else
                {
                    var Proc = from t in context.NielitProjectss
                               join k in context.NielitProjCoursess on t.ID equals k.projID
                               join d in context.CourseFeetypes on k.courseID equals d.courseID
                               where d.courseID == cID
                               && k.IsActive
                               && d.isActive
                               orderby (t.ProjectName)
                               select new { ValueField = t.ID, TextField = t.ProjectName };

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlProcname, Proc.Distinct(), lst);
                }

            };

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindCastCategory()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ddlCategory.Items.Clear();
                ListItem lst = new ListItem("--Select One--", "0");
                if (ddlProcname.SelectedValue.ToString() == "2")
                {
                    var castcategory = from p in context.CastCategories
                                       where (p.Code.Equals("SC") || p.Code.Equals("ST"))
                                       orderby (p.DisplayOrder)
                                       select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCategory, castcategory, lst);
                }
                else
                {
                    var castcategory = from p in context.CastCategories
                                       orderby (p.DisplayOrder)
                                       select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCategory, castcategory, lst);
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void bindReligion()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var religion = from p in context.Religions
                               orderby (p.DisplayOrder)
                               select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlReligion, religion, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindEducational()
    {
        try
        {
            //Int32 ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
            int courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
            using (var context = new EConnectContext())
            {
                //List<Int32> qualificationLevels = context.QualificationEligibility.Where(s => s.CourseID == courseID && s.ApplicantTypeID == ApplicantTypeId && s.EffectiveDateFrom <= DateTime.Now).OrderByDescending(c => c.EffectiveDateFrom).Select(c => c.QualificationLevelID).ToList();
                ListItem lst = new ListItem("--Select One--", "0");

                //var education = from p in context.EducationalQualifications
                //                join q in context.QualificationEligibility on p.QualificationLevelID equals q.QualificationLevelID
                //                where q.CourseID == courseID
                //                && q.ApplicantTypeID == ApplicantTypeId
                //                select new { ValueField = p.ID, TextField = p.Name };
                //EConnect.Utils.Common.ControlUtility.BindListObject(DDLeducode, education, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindState()
    {
        Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
        Int32 withdrawlid = Convert.ToInt32(enmAccreditationStatus.Withdrawal);
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CorState = (from s in context.Locations
                                orderby (s.Name)
                                where s.LocationTypeID == 2
                                && s.ParentLocationID == 1
                                select new { ValueField = s.ID, TextField = s.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCorState, CorState, lst);
                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlCmpState, CorState, lst);
                //var AccState = (from s in context.Locations
                //                join i in context.Institutes on s.ID equals i.StateID
                //                join a in context.AccreditationDetails on i.ID equals a.InstituteID
                //                orderby (s.Name)
                //                where s.LocationTypeID == 2
                //                && s.ParentLocationID == 1
                //                && a.CourseID == courseID
                //                && a.AccreditationStatusID != withdrawlid
                //                select new { ValueField = s.ID, TextField = s.Name }).Distinct().ToList();
                //EConnect.Utils.Common.ControlUtility.BindListObject(DdlAccState, AccState, lst);

                var PState = (from s in context.Locations
                              orderby (s.Name)
                              where s.LocationTypeID == 2
                              && s.ParentLocationID == 1
                              select new { ValueField = s.ID, TextField = s.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlPState, PState, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindDistrict(long stateID, ref DropDownList ddl)
    {
        try
        {
            int locationTypeID = Convert.ToInt32(enmLocationType.District);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var district = from s in context.Locations
                               orderby (s.Name)
                               where s.LocationTypeID == locationTypeID
                               && s.ParentLocationID == stateID
                               select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, district.ToList(), lst);

                if (ddl.Items.Count == 0)
                    ddl.Items.Add(lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void bindCorespondDistrict(int id)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (id != null & id != 0)
                {
                    var district = from s in context.Locations
                                   orderby (s.Name)
                                   where s.LocationTypeID == 4
                                   && s.ParentLocationID == id
                                   select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlcordistrict, district, lst);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void BindAccCentre(int stateid, int courseID)
    {
        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
            Int32 withdrawlid = Convert.ToInt32(enmAccreditationStatus.Withdrawal);
            //using (var context = new EConnectContext())
            //{
            //    var AccCentre = (from i in context.Institutes
            //                     join d in context.AccreditationDetails on i.ID equals d.InstituteID
            //                     orderby i.Name
            //                     where i.StateID == stateid
            //                     && d.CourseID == courseID
            //                     && d.AccreditationStatusID != withdrawlid
            //                     select new { ValueField = i.ID, TextField = d.AccreditationNumber + " - " + i.Name + ", " + (!string.IsNullOrEmpty(i.CityName) ? i.CityName : "") }).ToList();
            //    EConnect.Utils.Common.ControlUtility.BindListObject(DdlAccCentre, AccCentre.Distinct(), lst);
            //};
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    private void bindDeclaration(string applicantType, int agecount)
    {

        if (agecount == -1)
        {
            LblGuard.Text = txtFatherName.Text;
            LblHGuard.Text = txtFatherName.Text;
            lblname.Text = txtAppName.Text;
            LblHName.Text = txtAppName.Text;
            tddeclaration1.Style.Add("display", "none");
            tddeclaration2.Style.Add("display", "block");
        }
        else
        {
            tddeclaration1.Style.Add("display", "block");
            tddeclaration2.Style.Add("display", "none");
        }
    }

    protected void GetApplicantType()
    {
        try
        {
            if (Request.QueryString["id"] == null)
                return;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
                var course = context.Courses.Find(courseID);
                if (course != null)
                {
                    //if (!String.IsNullOrEmpty(course.ApplicantTypeID.ToString()))
                    //{
                    //    if (course.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                    //        RdoUndergngDOEACC.SelectedValue = "D";
                    //    else
                    //        RdoUndergngDOEACC.SelectedValue = "I";
                    //    lblApplicantType.Text = RdoUndergngDOEACC.SelectedItem.Text;
                    //    RdoUndergngDOEACC.Visible = false;
                    //    lblApplicantType.Visible = true;
                    //    RdoUndergngDOEACC_SelectedIndexChanged(this, null);
                    //}
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }

    protected void activeInactivePrsnlAndAddressDetail(Boolean isActive)
    {
        try
        {
            ddlSalutaionName.Enabled = isActive;
            txtAppName.Enabled = isActive;
            txtFatherName.Enabled = isActive;
            txtMotherName.Enabled = isActive;
            TxtGuardianName.Enabled = isActive;
            ddl_gender.Enabled = isActive;
            txtDob.Enabled = isActive;
            ddlCategory.Enabled = isActive;
            //imgDob.Visible = isActive;

            TxtPerAddressLine1.Enabled = isActive;
            TxtPerAddressLine2.Enabled = isActive;
            TxtPerAddressLine3.Enabled = isActive;
            TxtPerCity.Enabled = isActive;
            ddlPState.Enabled = isActive;
            TxtPpincode.Enabled = isActive;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void ResetAll()
    {
        try
        {

            txtAppName.Text = "";
            txtFatherName.Text = "";
            txtMotherName.Text = "";
            TxtGuardianName.Text = "";
            TxtCorAddressLine1.Text = "";
            TxtCorAddressLine2.Text = "";
            TxtCorAddressLine3.Text = "";
            TxtCorCity.Text = "";
            TxtPerAddressLine1.Text = "";
            TxtPerAddressLine2.Text = "";
            TxtPerAddressLine3.Text = "";
            TxtPerCity.Text = "";
            TxtSTDcode.Text = "";
            txtRegNo.Text = "";
            //TxtYearOfPassing2.Text = "";
            //txtBodyMark.Text = "";
            txtCorMobileNo.Text = "";
            txtCorPhoneNo.Text = "";
            txtCorPinCode.Text = "";
            TxtPpincode.Text = "";
            txtCorPhoneNo.Text = "";
            txtCorPinCode.Text = "";
            txtcode.Text = "";
            txtCorPhoneNo.Text = "";
            txtDob.Text = "";
            txtEmailId.Text = "";
            GenerateNewCaptchaImage();
            ddlCategory.SelectedValue = "0";
            ddlCorState.SelectedValue = "0";
            //DDLeducode.SelectedValue = "0";
            ddlSalutaionName.SelectedValue = "0";

            //txtCompanyName.Text = "";
            ddlCompanyName.SelectedValue = "0";
            txtCmpAdd1.Text = "";
            txtCmpAdd2.Text = "";
            txtCmpAdd3.Text = "";
            txtCmyCityName.Text = "";
            ddlCmpState.SelectedValue = "0";
            ddlCmpDistrict.SelectedValue = "0";
            ddlwhetherCertificateIssued.SelectedValue = "0";
            ddlWhetherPlaced.SelectedValue = "0";
            ddlCourseComplete.SelectedValue = "0";
            txtcertificateIssueDate.Text = "";
            txtCmpPinCode.Text = "";
            UidNumberTxt.Text = "";
            rdbCourseType.Enabled = true;
            txtRegNo.Enabled = true;
            rdbCourseType.ClearSelection();
            UidTypeDdl.SelectedValue = "0";
            lblerror.Text = "";

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool isDuplicate()
    {
        try
        {
            DateTime dob = Convert.ToDateTime(txtDob.Text);
            Int32 currentCourseID = Convert.ToInt32(Request.QueryString["id"].ToString());
            using (EConnectContext context = new EConnectContext())
            {
                Int32 count;
                Int32 ShortermCoursesCategory = context.CourseCategories.Where(s => s.Code.ToUpper() == "STC").FirstOrDefault().ID;
                Int32 IRDACategory = context.CourseCategories.Where(s => s.Code.ToUpper() == "IRDA").FirstOrDefault().ID;
                Course currentCourse = context.Courses.Find(currentCourseID);
                if (currentCourse.CourseCategoryID == ShortermCoursesCategory)
                {
                    if (Rdoownertype.SelectedValue == "G")
                    {
                        count = (from c in context.Candidates
                                 join r in context.RegistrationDetails
                                     on c.ID equals r.CandidateID
                                 where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                 && (c.GuardianName.ToUpper() == TxtGuardianName.Text.ToUpper().Trim() || c.FatherName.ToUpper() == txtFatherName.Text.ToUpper().Trim())
                                 && c.Gender == ddl_gender.SelectedValue
                                 && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                 && r.CourseID == currentCourseID
                                 select c).Count();
                    }
                    else
                    {
                        count = (from c in context.Candidates
                                 join r in context.RegistrationDetails
                                     on c.ID equals r.CandidateID
                                 where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                 && c.FatherName.Equals(txtFatherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                 && c.Gender == ddl_gender.SelectedValue
                                 && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                 && r.CourseID == currentCourseID
                                 select c).Count();
                    }
                    if (count > 0)
                        return true;
                    else
                        return false;
                }
                else if (currentCourse.CourseCategoryID == IRDACategory)
                {
                    Int32 ExamId = Convert.ToInt32(ViewState["ExamID"]);
                    if (Rdoownertype.SelectedValue == "G")
                    {
                        count = (from c in context.CourseRegistrationApplications
                                 where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                  && c.GuardianName.Equals(TxtGuardianName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                  && c.ApplicableExamID == ExamId
                                  && c.Gender == ddl_gender.SelectedValue
                                  && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                  && c.CourseID == currentCourseID
                                 select c).Count();
                    }
                    else
                    {
                        count = (from c in context.CourseRegistrationApplications
                                 where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                 && c.FatherName.Equals(txtFatherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                 && c.MotherName.Equals(txtMotherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                 && c.ApplicableExamID == ExamId
                                 && c.Gender == ddl_gender.SelectedValue
                                 && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                 && c.CourseID == currentCourseID
                                 select c).Count();
                    }
                    if (count > 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (Rdoownertype.SelectedValue == "G")
                    {
                        count = (from c in context.Candidates
                                 join r in context.RegistrationDetails
                                     on c.ID equals r.CandidateID
                                 where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                  && (c.GuardianName.ToUpper() == TxtGuardianName.Text.ToUpper().Trim() || c.FatherName.ToUpper() == txtFatherName.Text.ToUpper().Trim())
                                  && c.Gender == ddl_gender.SelectedValue
                                  && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                  && r.CourseCategoryID == currentCourse.CourseCategoryID
                                 select c).Count();
                    }
                    else
                    {
                        count = (from c in context.Candidates
                                 join r in context.RegistrationDetails
                                     on c.ID equals r.CandidateID
                                 where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                 && c.FatherName.Equals(txtFatherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                 && c.Gender == ddl_gender.SelectedValue
                                 && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                 && r.CourseCategoryID == currentCourse.CourseCategoryID
                                 select c).Count();
                    }
                    if (count > 0)
                        return true;
                    else
                        return false;
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void RenderPage(Int32 currentCourseID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                int id = Convert.ToInt32(Request.QueryString["id"].ToString());
                Course currentCourse = context.Courses.Find(currentCourseID);
                Int32 ShortermCoursesCategory = context.CourseCategories.Where(s => s.Code.ToUpper() == "STC").FirstOrDefault().ID;
                ListItem lst = new ListItem("--Select One--", "0");
                Lblhead.Text = currentCourse.Name;
                var cname = from c in context.Courses
                            where c.ID == id
                            select new { ValueField = c.ID, TextField = c.Name };

                //DDLRegForCourse.DataSource = cname.ToList();
                //DDLRegForCourse.DataValueField = "ValueField";
                //DDLRegForCourse.DataTextField = "TextField";
                //DDLRegForCourse.DataBind();
                //DDLRegForCourse.Enabled = false;


                //showing declaration
                if (currentCourse.CourseCategoryID == ShortermCoursesCategory)
                {
                    //Label90.Text = "Registration Cycle / पंजीकरण चक्र";
                    tddeclaration1.Style.Add("display", "none");
                    tddeclaration2.Style.Add("display", "block");
                    trnote.Visible = true;
                    lbldeccoursecode.Text = GetInitCap(currentCourse.Name);
                    lbldeccoursecode.Text = GetInitCap(currentCourse.Name);
                    LblHCourseCode.Text = GetInitCap(currentCourse.Name);
                    //ddlPaymentOption.Items.RemoveAt(0);
                }
                else if (currentCourse.Code == "ACC")
                {
                    //Label90.Text = "Registration Cycle / पंजीकरण चक्र";
                }
                else
                {
                    tddeclaration1.Style.Add("display", "block");
                    tddeclaration2.Style.Add("display", "none");
                    trnote.Visible = false;
                }

                //DDLRegForCourse.Enabled = false;

            };

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool IsValidForm()
    {
        try
        {

            // Int64 regno = Convert.ToInt64(txtregno.Text);
            if (txtrefno.Text == "")
            {
                lblerror.Text = "Please Enter Reference Number.";
                return false;
            }

            if (txtRemarks.Text == "")
            {
                lblerror.Text = "Please Enter Reason for Editing.";
                return false;
            }

            //DDLeducode_SelectedIndexChanged(DDLeducode, EventArgs.Empty);
            if (ddlCenter.SelectedValue == "0")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Center";
                ddlCenter.Focus();
                return false;
            }

            if (ddlCourse.SelectedValue == "0")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Course";
                ddlCourse.Focus();
                return false;
            }

            if (ddlBatch.SelectedValue == "0")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Batch";
                ddlBatch.Focus();
                return false;
            }

            if (ddlWheatherPojectStu.SelectedValue == "0")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Project Student";
                ddlBatch.Focus();
                return false;
            }

            if (ddlWheatherPojectStu.SelectedValue == "1")
            {
                if (ddlProcname.SelectedValue == "0")
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Please Select Project Name";
                    ddlBatch.Focus();
                    return false;
                }
            }

            if (ddlSalutaionName.SelectedValue == "0")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Salutation";
                ddlSalutaionName.Focus();
                return false;
            }

            if (!isBlank(txtAppName))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Applicant Name can not be left blank";
                return false;
            }

            if (!Char.IsLetter(txtAppName.Text, 0))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Applicant Name should start with an alphabet.");
            }

            if (!Char.IsLetter(txtAppName.Text, txtAppName.Text.Length - 1))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Applicant Name should end with an alphabet.");
            }

            if (txtAppName.Text.Length == 1)
            {
                txtAppName.Text = "";
                txtAppName.Focus();
                throw new Exception("Applicant Name should be single Character.");
            }

            if (TxtSTDcode.Text == "")
            {
                if (txtCorPhoneNo.Text != "")
                {
                    TxtSTDcode.Text = "";
                    TxtSTDcode.Focus();
                    throw new Exception("Please enter STD Code.");
                }
            }

            if (txtCorPhoneNo.Text == "")
            {
                if (TxtSTDcode.Text != "")
                {
                    txtCorPhoneNo.Text = "";
                    txtCorPhoneNo.Focus();
                    throw new Exception("Please enter Phone No.");
                }
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtAppName.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
            {
                txtAppName.Text = "";
                txtAppName.Focus();
                throw new Exception("Applicant Name should be with an English Alphabets(e.g - a-zA-Z)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtPerAddressLine1.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtPerAddressLine1.Text = "";
                TxtPerAddressLine1.Focus();
                throw new Exception("Address Line1 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtPerAddressLine2.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtPerAddressLine2.Text = "";
                TxtPerAddressLine2.Focus();
                throw new Exception("Address Line2 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtPerAddressLine3.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtPerAddressLine3.Text = "";
                TxtPerAddressLine3.Focus();
                throw new Exception("Address Line3 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtPerCity.Text, "^[a-zA-Z\u00FC\u00DC ]"))
            {
                TxtPerCity.Text = "";
                TxtPerCity.Focus();

                throw new Exception("City Name should be with an English Alphabets(e.g - a-zA-Z)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorAddressLine1.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtCorAddressLine1.Text = "";
                TxtCorAddressLine1.Focus();
                throw new Exception("Correspondence Address Line1 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorAddressLine2.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtCorAddressLine2.Text = "";
                TxtCorAddressLine2.Focus();
                throw new Exception("Correspondence Address Line2 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorAddressLine3.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtCorAddressLine3.Text = "";
                TxtCorAddressLine3.Focus();
                throw new Exception("Correspondence Address Line3 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorCity.Text, "^[a-zA-Z\u00FC\u00DC ]"))
            {
                TxtCorCity.Text = "";
                TxtCorCity.Focus();
                throw new Exception("Correspondence City Name should be with an English Alphabets(e.g - a-zA-Z)");
            }



            if (Rdoownertype.SelectedValue == "P")//Parents
            {
                if (String.IsNullOrWhiteSpace(txtFatherName.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Please enter Father's Name.");
                }

                if (txtFatherName.Text.Length == 1)
                {
                    txtFatherName.Text = "";
                    txtFatherName.Focus();
                    throw new Exception("Father Name should be single Character.");
                }

                if (txtMotherName.Text.Length == 1)
                {
                    txtMotherName.Text = "";
                    txtMotherName.Focus();
                    throw new Exception("Mother Name should be single Character.");
                }



                if (!System.Text.RegularExpressions.Regex.IsMatch(txtFatherName.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
                {
                    txtFatherName.Text = "";
                    txtFatherName.Focus();
                    throw new Exception("Father Name should be with an English Alphabets(e.g - a-zA-Z)");
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtMotherName.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
                {
                    txtMotherName.Text = "";
                    txtMotherName.Focus();
                    throw new Exception("Mother Name should be with an English Alphabets(e.g - a-zA-Z)");
                }

                if (String.IsNullOrWhiteSpace(txtMotherName.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Please enter Mother's Name");
                }

                if (!String.IsNullOrWhiteSpace(txtFatherName.Text))
                {
                    if (!Char.IsLetter(txtFatherName.Text, 0))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Father Name should start with an alphabet.");
                    }
                    if (!Char.IsLetter(txtFatherName.Text, txtFatherName.Text.Length - 1))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Father Name should end with an alphabet.");
                    }
                }

                if (!String.IsNullOrWhiteSpace(txtMotherName.Text))
                {
                    if (!Char.IsLetter(txtMotherName.Text, 0))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Mother Name should start with an alphabet.");
                    }
                    if (!Char.IsLetter(txtMotherName.Text, txtMotherName.Text.Length - 1))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Mother Name should end with an alphabet.");
                    }
                }
            }
            else if (Rdoownertype.SelectedValue == "G")
            {
                if (String.IsNullOrWhiteSpace(TxtGuardianName.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Please enter Guardian Name.");
                }

                if (!String.IsNullOrWhiteSpace(TxtGuardianName.Text))
                {
                    if (!Char.IsLetter(TxtGuardianName.Text, 0))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Guardian Name should start with an alphabet.");
                    }

                    if (TxtGuardianName.Text.Length == 1)
                    {
                        TxtGuardianName.Text = "";
                        TxtGuardianName.Focus();
                        throw new Exception("Guardian Name should be single Character.");
                    }

                    if (!System.Text.RegularExpressions.Regex.IsMatch(TxtGuardianName.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
                    {
                        TxtGuardianName.Text = "";
                        TxtGuardianName.Focus();
                        throw new Exception("Guardian Name should be with an English Alphabets(e.g - a-zA-Z)");
                    }

                    if (!Char.IsLetter(TxtGuardianName.Text, TxtGuardianName.Text.Length - 1))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Guardian Name should end with an alphabet.");
                    }
                }
            }

            if (!isBlank(txtDob))
            {
                lblerror.Visible = true;
                lblerror.Text = "Date Of Birth can not be left blank";
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                return false;
            }
            if (!isValidDob(txtDob))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Date of Birth";
                return false;
            }
            if (!isSelected(ddlMStatus))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Marital Status";
                return false;
            }
            if (!isSelected(ddlCategory))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Cast Category";
                return false;
            }
            if (!isSelected(ddlReligion))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Religion";
                return false;
            }
            if (String.IsNullOrEmpty(TxtSTDcode.Text) == false && string.IsNullOrEmpty(txtCorPhoneNo.Text) == false)
            {

                if (!isNumber(TxtSTDcode))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Invalid STD Code";
                    return false;
                }

                if (!isNumber(txtCorPhoneNo))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Invalid Phone Number";
                    return false;
                }
            }
            if (!isBlank(txtCorMobileNo))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Mobile Number can not be left blank";
                return false;
            }
            if (!isNumber(txtCorMobileNo))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Mobile Number";
                return false;
            }
            if (!isBlank(txtEmailId))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Email Id can not be left blank";
                return false;
            }
            if (!isBlank(TxtPerAddressLine1))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Permanent Address Line1 can not be left blank";
                return false;
            }
            if (!isBlank(TxtPerAddressLine2))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Permanent Address Line2 can not be left blank";
                return false;
            }
            if (!isBlank(TxtPerCity))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Permanent City can not be left blank";
                return false;
            }

            if (!isSelected(ddlPState))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select Permanent State";
                return false;
            }
            if (!isSelected(ddlPdistrict))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Permanent District";
                return false;
            }
            if (!isBlank(TxtPpincode))
            {
                lblerror.Visible = true;
                lblerror.Text = "Pin Code can not be left blank";
                return false;
            }
            if (!isNumber(TxtPpincode))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Pin Code Number";
                return false;
            }
            if (chkSame.Checked == false)
            {
                if (!isBlank(TxtCorAddressLine1))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "correspondence Address Line1 can not be left blank";
                    return false;
                }
                if (!isBlank(TxtCorAddressLine2))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "correspondence Address Line2 can not be left blank";
                    return false;
                }
                if (!isBlank(TxtCorCity))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "correspondence  City can not be left blank";
                    return false;
                }

                if (!isSelected(ddlCorState))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Please Select correspondence State";
                    return false;
                }
                if (!isSelected(ddlcordistrict))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Please Select correspondence District";
                    return false;
                }
                if (!isBlank(txtCorPinCode))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Pin Code can not be left blank";
                    return false;
                }
                if (!isNumber(txtCorPinCode))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Invalid  Pin Code Number";
                    return false;
                }
            }
            //if (!isSelected(DDLeducode))
            //{
            //    lblerror.Visible = true;
            //    GenerateNewCaptchaImage();
            //    txtcode.Text = "";
            //    lblerror.Text = "Please Select Highest Education";
            //    return false;
            //}

            //if (!isBlank(TxtYearOfPassing2))
            //{
            //    lblerror.Visible = true;
            //    GenerateNewCaptchaImage();
            //    txtcode.Text = "";
            //    lblerror.Text = "Passing Year can not be left blank";
            //    return false;
            //}
            //if (!isNumber(TxtYearOfPassing2))
            //{
            //    lblerror.Visible = true;
            //    GenerateNewCaptchaImage();
            //    txtcode.Text = "";
            //    lblerror.Text = "Invalid Highest Education Passing Year ";
            //    return false;
            //}

            //if (!isValidPassingYear(TxtYearOfPassing2, txtDob))
            //{
            //    lblerror.Visible = true;
            //    GenerateNewCaptchaImage();
            //    txtcode.Text = "";
            //    lblerror.Text = "Invalid Highest Education Passing Year ";
            //    return false;
            //}



            //if (!String.IsNullOrEmpty(txtaadhar.Text))
            //{
            //    if (!IsNumeric(txtaadhar.Text))
            //    {
            //        GenerateNewCaptchaImage();
            //        txtcode.Text = "";
            //        throw new Exception("Not valid Aadhaar Number.");
            //    }
            //}





            if (UIDtr.Visible == true)
            {

                if (isSelected(UidTypeDdl))
                {
                    if (UidTypeDdl.SelectedValue == "1")  // ----Aadhaar
                    {
                        //Added for update mode 
                        if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                        {
                        }
                        else
                        {
                            if (!IsNumeric(UidNumberTxt.Text))
                            {
                                GenerateNewCaptchaImage();
                                txtcode.Text = "";
                                throw new Exception("Not valid Aadhaar Number.");
                            }
                            if (UidNumberTxt.Text.Length != 12)
                            {
                                GenerateNewCaptchaImage();
                                txtcode.Text = "";
                                throw new Exception("Aadhaar Number should of 12 digits.");
                            }
                        }
                    }
                    else if (UidTypeDdl.SelectedValue == "2")  // ----PAN
                    {
                        if (!Char.IsLetter(UidNumberTxt.Text, 0))
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            throw new Exception("PAN should start with an alphabet.");
                        }
                        if (UidNumberTxt.Text.Length != 10)
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            throw new Exception("PAN should of 10 digits.");
                        }
                    }
                }

            }

            if (ddlwhetherCertificateIssued.SelectedValue == "1")//Parents
            {
                if (!isBlank(txtcertificateIssueDate))
                {
                    GenerateNewCaptchaImage();
                    txtcertificateIssueDate.Text = "";
                    throw new Exception("Certificate Issue Date can not be left blank");
                }
            }
            if (!isBlank(txtcode))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Captcha Code can not be left blank";
                return false;
            }
            if (txtcode.Text != ViewState["CaptchCode"].ToString())
            {
                lblerror.Visible = true;
                lblerror.Text = "Invalid Captcha Code";
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            GenerateNewCaptchaImage();
            txtcode.Text = "";
            throw ex;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidForm())
            {
                //if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                // {
                btnSave.Text = "Update";
                btnback.Visible = true;
                UpdateData();
                //  }
                // else
                // {
                //   btnSave.Text = "Submit";
                //  btnback.Visible = true;
                //  SaveData();
                //}
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void chkSame_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkSame.Checked)
            {
                bool isComplete = true;
                if (!isBlank(TxtPerAddressLine1))
                {
                    lblerror.Text = "Permanent Address Line1 can not be left blank";
                    isComplete = false;
                }
                else if (!isBlank(TxtPerAddressLine2))
                {
                    lblerror.Text = "Permanent Address Line2 can not be left blank";
                    isComplete = false;
                }
                else if (!isBlank(TxtPerCity))
                {
                    lblerror.Text = "Permanent City can not be left blank";
                    isComplete = false;
                }

                else if (!isSelected(ddlPState))
                {
                    lblerror.Text = "Please Select Permanent State";
                    isComplete = false;
                }
                else if (!isSelected(ddlPdistrict))
                {
                    lblerror.Text = "Please Select Permanent District";
                    isComplete = false;
                }
                else if (!isBlank(TxtPpincode))
                {
                    lblerror.Text = "Pin Code can not be left blank";
                    isComplete = false;
                }
                else if (!isNumber(TxtPpincode))
                {
                    lblerror.Text = "Invalid Pin Code Number";
                    isComplete = false;
                }
                else if (TxtPpincode.Text.Length != 6)
                {
                    lblerror.Text = "Invalid Pin Code Number";
                    isComplete = false;
                }
                if (isComplete == false)
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    chkSame.Checked = false;
                    return;
                }
                TxtCorAddressLine1.Enabled = false;
                TxtCorAddressLine2.Enabled = false;
                TxtCorAddressLine3.Enabled = false;
                TxtCorCity.Enabled = false;
                txtCorPinCode.Enabled = false;
                ddlCorState.Enabled = false;
                ddlcordistrict.Enabled = false;
                TxtCorAddressLine1.Text = TxtPerAddressLine1.Text;
                TxtCorAddressLine2.Text = TxtPerAddressLine2.Text;
                TxtCorAddressLine3.Text = TxtPerAddressLine3.Text;
                TxtCorCity.Text = TxtPerCity.Text;
                txtCorPinCode.Text = TxtPpincode.Text;
                ddlCorState.SelectedValue = ddlPState.SelectedValue;
                ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                ddlcordistrict.SelectedValue = ddlPdistrict.SelectedValue;
            }
            else
            {
                TxtCorAddressLine1.Enabled = true;
                TxtCorAddressLine2.Enabled = true;
                TxtCorAddressLine3.Enabled = true;
                TxtCorCity.Enabled = true;
                txtCorPinCode.Enabled = true;
                ddlCorState.Enabled = true;
                ddlcordistrict.Enabled = true;
                TxtCorAddressLine1.Text = "";
                TxtCorAddressLine2.Text = "";
                TxtCorAddressLine3.Text = "";
                TxtCorCity.Text = "";
                txtCorPinCode.Text = "";
                ddlCorState.SelectedValue = "0";
                ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                ddlcordistrict.SelectedValue = "0";
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void DdlAccState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            //stateid = Convert.ToInt32(DdlAccState.SelectedValue);
            Int32 courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
            BindAccCentre(stateid, courseID);


        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("NielitCentreStudentEdit.aspx?msg=" + strMessage, true);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }

    }

    protected void RdoUndergngDOEACC_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //LblExamName.Text = "";
            //if (RdoUndergngDOEACC.SelectedValue == "I")
            //{
            //    TrLastCenterAccno.Visible = true;
            //    TrAccCentre.Visible = true;
            //    TrExperience.Visible = false;
            //    ddlPaymentOption.SelectedValue = "2";
            //    ddlPaymentOption.Enabled = true;
            //    //DdlAccState.SelectedValue = "0";
            //    DdlAccState_SelectedIndexChanged(DdlAccState, EventArgs.Empty);
            //    bindDeclaration("I", 0);

            //}
            //else
            //{
            //    TrExperience.Visible = true;
            //    TrLastCenterAccno.Visible = false;
            //    TrAccCentre.Visible = false;
            //    ddlPaymentOption.SelectedValue = "1";
            //    ddlPaymentOption.Enabled = false;
            //    txtDob.Text = "";
            //    bindDeclaration("D", 0);
            //}
            //Int32 ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
            bindEducational();
            //bindExamName(ApplicantTypeId);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }
    }

    protected void ddlCorState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id1 = Convert.ToInt32(ddlCorState.SelectedValue);
            ddlcordistrict.Items.Clear();
            bindDistrict(id1, ref ddlcordistrict);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void ddlPState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id2 = Convert.ToInt32(ddlPState.SelectedValue);
            bindDistrict(id2, ref ddlPdistrict);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
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
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }



    protected void ddlSalutaionName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSalutaionName.SelectedValue != "0")
            {
                if (ddlSalutaionName.SelectedValue.Trim().ToUpper() == "MS.")
                {
                    ddl_gender.SelectedValue = "Female";
                    ddl_gender.Enabled = false;
                }
                else if (ddlSalutaionName.SelectedValue == "Mr.")
                {
                    ddl_gender.SelectedValue = "Male";
                    ddl_gender.Enabled = false;
                }
                //Added for transgender
                else if (ddlSalutaionName.SelectedValue == "X")
                {
                    ddl_gender.SelectedValue = "Trans";
                    ddl_gender.Enabled = false;
                }

            }

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void Rdoownertype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Rdoownertype.SelectedValue == "P")//Parents
            {
                trfather.Visible = true;
                trmother.Visible = true;
                trguardian.Visible = false;
                trfather.Attributes.Add("class", "gdrow1");
                trmother.Attributes.Add("class", "gdalternate1");
                trgender.Attributes.Add("class", "gdrow1");
                trdob.Attributes.Add("class", "trgdalternate1calendar");
                trmaritalstatus.Attributes.Add("class", "gdrow1");
                trcategory.Attributes.Add("class", "gdalternate1");
                trhandicapped.Attributes.Add("class", "gdrow1");
                trexserviceman.Attributes.Add("class", "gdalternate1");
                trreligion.Attributes.Add("class", "gdrow1");
            }
            if (Rdoownertype.SelectedValue == "G")//Guardian
            {
                trguardian.Visible = true;
                trfather.Visible = false;
                trmother.Visible = false;
                trgender.Attributes.Add("class", "gdalternate1");
                trdob.Attributes.Add("class", "trgdrow1calendar");
                trmaritalstatus.Attributes.Add("class", "gdalternate1");
                trcategory.Attributes.Add("class", "gdrow1");
                trhandicapped.Attributes.Add("class", "gdalternate1");
                trexserviceman.Attributes.Add("class", "gdrow1");
                trreligion.Attributes.Add("class", "gdalternate1");
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }
    }

    protected void txtDob_TextChanged(object sender, EventArgs e)
    {
        DateTime todaydate = DateTime.Now;
        DateTime Inputdate = Convert.ToDateTime(txtDob.Text);
        int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);
        //string applicantType = RdoUndergngDOEACC.SelectedValue.ToString();
        //if (courseType < 5)
        //    bindDeclaration(applicantType, countAge);

    }

    protected void DdlAccCentre_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (DdlAccCentre.SelectedValue != "0")
        //    AlertModalPopUp.Show();
    }

    protected void ddlCmpState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id1 = Convert.ToInt32(ddlCmpState.SelectedValue);
            //ddlcordistrict.Items.Clear();
            bindDistrict(id1, ref ddlCmpDistrict);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void ddlWheatherPojectStu_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlWheatherPojectStu.SelectedValue == "1")
        {

            ddlProcname.Enabled = true;
        }
        else
        {
            ddlProcname.SelectedValue = "0";
            ddlProcname.Enabled = false;
        }
        // bindCastCategory();
    }

    protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCompanyName.SelectedValue != "0")
            {
                Int64 iID = Convert.ToInt64(ddlCompanyName.SelectedValue);
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    var application = context.companyMasters.Find(iID);

                    //ddlCompanyName.SelectedItem = application.company_Name;
                    txtCmpAdd1.Text = application.comapny_Address.ToString();

                    if (application.comapny_Address2 != null)
                    {
                        txtCmpAdd2.Text = application.comapny_Address2.ToString();
                    }

                    if (application.comapny_Address3 != null)
                    {
                        txtCmpAdd3.Text = application.comapny_Address3.ToString();
                    }

                    if (application.CompnyCityName != null)
                    {
                        txtCmyCityName.Text = application.CompnyCityName.ToString();
                    }

                    if (application.compnyState_ID != null)
                    {
                        ddlCmpState.SelectedValue = application.compnyState_ID.ToString();
                    }
                    ddlCmpState_SelectedIndexChanged(ddlCmpState, EventArgs.Empty);
                    if (application.compnyDistrict_ID != null)
                    {
                        ddlCmpDistrict.SelectedValue = application.compnyDistrict_ID.ToString();
                    }

                    if (application.CompnyPinCode != null)
                    {
                        txtCmpPinCode.Text = application.CompnyPinCode.ToString();
                    }

                    if (application.contactPersonName != null)
                    {
                        txtPersonName.Text = application.contactPersonName.ToString();
                    }

                    if (application.contactPersonMobile != null)
                    {
                        txtMob.Text = application.contactPersonMobile.ToString();
                    }

                    //ddlCorState_SelectedIndexChanged(ddlCmpState, EventArgs.Empty);
                    //ddlCmpDistrict.SelectedValue = application.compnyDistrict_ID.ToString();
                    //txtCmpPinCode.Text = application.CompnyPinCode.ToString();

                    //txtPersonName.Text = application.contactPersonName.ToString();
                    //txtMob.Text = application.contactPersonMobile.ToString();

                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlWhetherPlaced_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlWhetherPlaced.SelectedValue == "1")
        {
            btn.Visible = true;
            //trPersonDetails.Visible = true;
            //tr2.Visible = true;
            //trCompanyHead.Visible = true;
            //trCompanyName.Visible = true;
            //trCmpAdd1.Visible = true;
            //trCmpAdd2.Visible = true;
            //trCmpAdd3.Visible = true;
            //trCmyCityName.Visible = true;
            //trCmpState.Visible = true;
            //trCmpDistrict.Visible = true;
            //trCmpPinCode.Visible = true;

        }

        else
        {
            btn.Visible = false;
            //trPersonDetails.Visible = false;
            //tr2.Visible = false;
            //trCompanyHead.Visible = false;
            //trCompanyName.Visible = false;
            //trCmpAdd1.Visible = false;
            //trCmpAdd2.Visible = false;
            //trCmpAdd3.Visible = false;
            //trCmyCityName.Visible = false;
            //trCmpState.Visible = false;
            //trCmpDistrict.Visible = false;
            //trCmpPinCode.Visible = false;

        }
    }
    protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem lst = new ListItem("--Select One--", "0");
        Int64 NelitCentreLinkId = 0;
        Int64 NielitCentrelinkedToCentreId = 0;
        Int64 subcentreid = 0;
        string Seleted = "";
        User objUser;

        ddlBatch.ClearSelection();
        ddlCourse.ClearSelection();

        ddlBatch.Items.Clear();
        ddlCourse.Items.Clear();
        try
        {
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    Seleted = RdoAffInstOrNonAffInst.SelectedValue;
                    subcentreid = Convert.ToInt64(ddlCenter.SelectedValue);
                    if (UserTypeid == 10)
                    {
                        var instituteslinkedToCentre = context.NielitCentres.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (UserTypeid == 11)
                    {
                        var instituteslinkedToCentre = context.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }

                    if (NielitCentrelinkedToCentreId != 0)
                    {
                        NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        txtInstitute.Text = institutesName.Name;
                        Int32 NelitCentreLinkId1 = Convert.ToInt32(institutesName.ID);

                    }

                    if (Seleted == "2")
                        //Added 6 Sep 2021
                        if (NielitCentreId.ToString().Length <= 4 && SubcetreIdByApplication != 0)
                        {
                            BindCourses(SubcetreIdByApplication, Convert.ToInt16(Seleted));


                        }
                        else
                        {
                            BindCourses(Convert.ToInt32(NielitCentreId), Convert.ToInt16(Seleted));
                        }
                    // BindCourses(Convert.ToInt32(NielitCentreId), Convert.ToInt16(Seleted));
                    else
                        BindCourses(Convert.ToInt32(subcentreid), Convert.ToInt16(Seleted));

                    ddlCourse.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Error:" + ex.Message);
        }

    }


    //protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Int64 NelitCentreLinkId = 0;
    //    Int64 subcentreid = 0;
    //    Int64 Courseid = 0;

    //    ddlBatch.ClearSelection();

    //    ddlBatch.Items.Clear();

    //    try
    //    {
    //        subcentreid = Convert.ToInt64(ddlCenter.SelectedValue);
    //        Courseid = Convert.ToInt64(ddlCourse.SelectedValue);

    //        using (NIELITMISContext context = new NIELITMISContext())
    //        {
    //            ListItem lst = new ListItem("--Select One--", "0");

    //            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
    //            {
    //                if (RdoAffInstOrNonAffInst.SelectedValue == "2")
    //                {

    //                    var Batch = from s in context.NielitCentreBatchs
    //                                where s.IsVerified == true && s.centreID == subcentreid && s.subCentreID == 0
    //                                        && s.CourseDurationID == Courseid // && (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
    //                                orderby (s.Name)
    //                                select new { ValueField = s.ID, TextField = s.Name };
    //                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
    //                    bindProject();
    //                }
    //                else
    //                {
    //                    var Batch = from s in context.NielitCentreBatchs
    //                                where s.IsVerified == true && s.subCentreID == subcentreid
    //                                        && s.CourseDurationID == Courseid //&& (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
    //                                orderby (s.Name)
    //                                select new { ValueField = s.ID, TextField = s.Name };
    //                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
    //                    bindProject();

    //                }
    //            }
    //            else
    //            {
    //                if (RdoAffInstOrNonAffInst.SelectedValue == "2")
    //                {

    //                    var Batch = from s in context.NielitCentreBatchs
    //                                where s.IsVerified == true && s.centreID == subcentreid && s.subCentreID == 0
    //                                        && s.CourseDurationID == Courseid
    //                                //(s.startDate <= System.DateTime.Now && 
    //                                // && (s.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
    //                                orderby (s.Name)
    //                                select new { ValueField = s.ID, TextField = s.Name };
    //                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
    //                    bindProject();
    //                }
    //                else
    //                {
    //                    var Batch = from s in context.NielitCentreBatchs
    //                                where s.IsVerified == true && s.subCentreID == subcentreid
    //                                        && s.CourseDurationID == Courseid
    //                                //(s.startDate <= System.DateTime.Now && 

    //                                // && (s.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
    //                                orderby (s.Name)
    //                                select new { ValueField = s.ID, TextField = s.Name };
    //                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
    //                    bindProject();

    //                }
    //            }
    //        };

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 NelitCentreLinkId = 0;
        Int64 subcentreid = 0;
        Int64 Courseid = 0;

        ddlBatch.ClearSelection();

        ddlBatch.Items.Clear();

        try
        {
            subcentreid = Convert.ToInt64(ddlCenter.SelectedValue);
            Courseid = Convert.ToInt64(ddlCourse.SelectedValue);

            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");

                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    if (RdoAffInstOrNonAffInst.SelectedValue == "2")
                    {

                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.centreID == subcentreid && s.subCentreID == 0
                                            && s.CourseDurationID == Courseid // && (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
                        bindProject();
                    }
                    else
                    {
                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.subCentreID == subcentreid
                                            && s.CourseDurationID == Courseid //&& (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
                        bindProject();

                    }
                }
                else
                {
                    if (RdoAffInstOrNonAffInst.SelectedValue == "2")
                    {

                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.centreID == subcentreid && s.subCentreID == 0
                                            && s.CourseDurationID == Courseid
                                    //(s.startDate <= System.DateTime.Now && 
                                    // && (s.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
                        bindProject();
                    }
                    else
                    {
                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.subCentreID == subcentreid
                                            && s.CourseDurationID == Courseid
                                    //(s.startDate <= System.DateTime.Now && 

                                    // && (s.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
                        bindProject();

                    }
                }
            };

            ddlWheatherPojectStu.SelectedValue = "0";
            ddlWheatherPojectStu_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void BindBatch()
    {
        try
        {
            //Added 6 Sep2021
            ddlBatch.Items.Clear();
            User objUser;
            using (EConnectContext context = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                using (NIELITMISContext context1 = new NIELITMISContext())
                {
                    ListItem lst = new ListItem("--Select One--", "0");
                    if (UserTypeid == 10)
                    {
                        var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                        Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        if (NielitCentrelinkedToCentreId != 0)
                        {
                            NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            RdoAffInstOrNonAffInst.SelectedValue = "2";
                            ddlCenter.Enabled = false;
                            //     ListItem lst = new ListItem("--Select One--", "0");
                            var BatchName = from p in context1.NielitCentreBatchs
                                            where p.IsVerified == true && (p.startDate <= System.DateTime.Now && p.endDate >= System.DateTime.Now)
                                            && p.subCentreID == NelitCentreLinkId
                                            orderby (p.Name)
                                            select new { ValueField = p.ID, TextField = p.Name };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                            //EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                        }
                        else
                        {
                            NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            RdoAffInstOrNonAffInst.SelectedValue = "2";
                            ddlCenter.Enabled = false;
                            // ListItem lst = new ListItem("--Select One--", "0");
                            var BatchName = from p in context1.NielitCentreBatchs
                                            where p.IsVerified == true 
//&& (p.startDate <= System.DateTime.Now && p.endDate >= System.DateTime.Now)
                                                //&& p.centreID == NelitCentreLinkId  -- Changed on 6 Sep 2021
                         && (p.centreID == NelitCentreLinkId || p.centreID == SubcetreIdByApplication)
                                            orderby (p.Name)
                                            select new { ValueField = p.ID, TextField = p.Name };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                        }
                    }
                    else if (UserTypeid == 11)
                    {
                        var intituteslinkedToCentre = context1.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        Int32 subcentreId = Convert.ToInt32(intituteslinkedToCentre.ID);
                        NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        }
                        // FillddlSubcentreName();
                        //ListItem lst = new ListItem("--Select One--", "0");
                        var BatchName = from p in context1.NielitCentreBatchs
                                        where p.IsVerified == true && (p.startDate <= System.DateTime.Now && p.endDate >= System.DateTime.Now)
                                        && p.subCentreID == subcentreId
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                    }
                    else if (UserTypeid == 4)
                    {
                        var intituteslinkedToCentre = context1.AffInstitutes.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        Int32 subcentreId = Convert.ToInt32(intituteslinkedToCentre.ID);

                        NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        }
                        // FillddlSubcentreName();
                        //   ListItem lst = new ListItem("--Select One--", "0");
                        var BatchName = from p in context1.NielitCentreBatchs
                                        where p.IsVerified == true && (p.startDate <= System.DateTime.Now && p.endDate >= System.DateTime.Now)
                                        && p.subCentreID == subcentreId
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);

                    }
                }
                //  ListItem lst = new ListItem("--Select One--", "0");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void certificateIssueDate_Calender_SelectionChanged(object sender, System.EventArgs e)
    {

        NIELITMISContext context = new NIELITMISContext();
        Int64 applID = Convert.ToInt64(Request.QueryString["Key"]);

        var objRegistration = context.NielitCentreStudent.Find(applID);

        if (objRegistration.whether_Certificate_Issued.ToString() == "1")
        {
            //txtcertificateIssueDate.Text = Convert.ToDateTime(certificateIssueDate_Calender.SelectedDate, CultureInfo.GetCultureInfo("en-US")).ToString("dd-MMM-yyyy");
        }
        else
        {
            lblerror.Visible = true;
            lblerror.Text = "Not allowed to change date!!.";
            txtcertificateIssueDate.Focus();
            return;
        }
    }

    protected void CalendarPlacementDate_SelectionChanged(object sender, System.EventArgs e)
    {
        //txtPlacementDate.Text = Convert.ToDateTime(CalendarPlacementDate.SelectedDate, CultureInfo.GetCultureInfo("en-US")).ToString("dd-MMM-yyyy");
    }

    protected void TextBox1_TextChanged(object sender, System.EventArgs e)
    {
        DateTime todaydate = DateTime.Now;
        DateTime Inputdate = Convert.ToDateTime(txtDob.Text);
        int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);
        //string applicantType = RdoUndergngDOEACC.SelectedValue.ToString();
        //if (courseType < 5)
        //    bindDeclaration(applicantType, countAge);
    }

    protected void btn_Click(object sender, System.EventArgs e)
    {
        using (NIELITMISContext context = new NIELITMISContext())
        {

            Int64 applID = Convert.ToInt64(Request.QueryString["Key"]);
            var objRegistration = context.NielitCentreStudent.Find(applID);

            Session["StudentID"] = objRegistration.Number;
            Session["batch_ID"] = objRegistration.batch_ID;
        }

        Response.Redirect("../Admin/StudentPlacementDetails.aspx", true);
    }
    protected void ddlProcname_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindCastCategory();

    }
    protected bool checkDuplicateAAdhaar()
    {
        try
        {
            DateTime dob = Convert.ToDateTime(txtDob.Text);
            Int32 currentCourseID = Convert.ToInt32(ddlCourse.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int32 count;

                string EncAadhaar = EConnect.Utils.Security.Encryption.Encrypt(UidNumberTxt.Text.Trim().ToUpper());

                // if (Rdoownertype.SelectedValue == "G")
                // {

                var stuBatch = (from a in context.NielitCentreBatchs
                                where a.ID.ToString() != ddlBatch.SelectedValue.ToString()
                                && (System.Data.Entity.DbFunctions.TruncateTime(System.DateTime.Today) >= System.Data.Entity.DbFunctions.TruncateTime(a.startDate) && System.Data.Entity.DbFunctions.TruncateTime(System.DateTime.Today) <= System.Data.Entity.DbFunctions.TruncateTime(a.endDate))
                                select new
                                {
                                    batchID = a.ID
                                }).ToList();


                var studentList = (from c in context.NielitCentreStudent

                                   where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)

                                   && (c.GuardianName.ToUpper() == TxtGuardianName.Text.ToUpper().Trim() || c.FatherName.ToUpper() == txtFatherName.Text.ToUpper().Trim())
                                   && c.Gender == ddl_gender.SelectedValue
                                   && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                       // && c.CourseID != currentCourseID
                                   && c.UIDNumber == EncAadhaar
                                   && c.UIDType.ToString() == "1"

                                   select new
                                   {
                                       batch = c.batch_ID
                                   }).Distinct().ToList();

                if (studentList.Count() == 0)
                    return false;

                foreach (var batch1 in stuBatch)
                {
                    foreach (var studentList1 in studentList)
                    {
                        if (batch1.batchID == studentList1.batch)
                        {
                            return true;
                        }
                    }
                }
                return false;
                // }


            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindGender()
    {
        try
        {
            using (EConnectContext vContext = new EConnectContext())
            {
                var Gender = from s in
                                 vContext.tblGender
                             select new { ValueField = s.genderCode, TextField = s.name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddl_gender, Gender, new ListItem("--Select Gender--", "0"));

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindCourses(Int32 CentreId, int CentreType)
    {
        // 2- Centre, 0--Non Aff Instt 1--Aff Instt

        using (NIELITMISContext context = new NIELITMISContext())
        {
            ListItem lst = new ListItem("--Select One--", "0");
            using (DataTable dt = GetBatchCourseRecord(CentreId, CentreType))
            {
                //    //var Course = from p in context.NielitCentreCourses
                //    //             join k in context.NielitCourseDurations on p.ID equals k.courseID
                //    //             where p.IsVerified == true && k.isVerified==true
                //    //             orderby (p.Name)
                //    //             select new { ValueField = k.ID, TextField = p.Name+ " ( " + k.courseDurationDays +" Days )" };
                //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, Course, lst);

                if (dt.Rows.Count > 0)
                {
                    ddlCourse.DataSource = dt;
                    ddlCourse.DataTextField = "Name";
                    ddlCourse.DataValueField = "ID";
                    ddlCourse.DataBind();
                    ddlCourse.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
            }
        }
    }
    protected DataTable GetBatchCourseRecord(Int32 CentreId, int CentreType)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetBatchCourseRecord", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pCentreId", SqlDbType.BigInt).Value = CentreId;
                    cmd.Parameters.Add("@pCentreType", SqlDbType.Int).Value = CentreType;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    public void NIELITStudentCentreHistory()
    {
        try
        {
            //string constr = ConfigurationManager.ConnectionStrings["NIELITMISContextt"].ConnectionString;
            string refno = txtrefno.Text;
            string remarks = txtRemarks.Text;
            int EditBy = Convert.ToInt32(Session["UserID"]);
            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection Conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("InsertNIELITStudentCenterHistory", Conn))
                {
                    Conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@refnumber", refno);
                    cmd.Parameters.AddWithValue("@remarks", remarks);
                    cmd.Parameters.AddWithValue("@editby", EditBy);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void RadioButtonListCentre_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        txtRegNo.Text = "";
        txtRegNo.Enabled = true;
        rdbCourseType.Enabled = true;
        if (RadioButtonListCentre.SelectedValue.Trim() == "Y")
        {
            Trappno.Visible = true;
            tr4.Visible = true;
            rdbCourseType.ClearSelection();
        }
        else
        {
            Trappno.Visible = false;
            tr4.Visible = false;
            // ResetAll();
        }
    }

    protected void rdbCourseType_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        txtRegNo.Text = "";
        txtRegNo.Enabled = true;
        if (rdbCourseType.SelectedValue.Trim() == "R")
        {
            lblregistrationNo.Text = "Registration No.";
            using (NIELITMISContext context = new NIELITMISContext())
            {
                string Number = txtrefno.Text;
                var regno = (from a in context.NielitCentreStudent

                             where a.Number == Number
                             select new
                             {
                                 CourseRegistrationNo = a.RegisteredCourseRegistrationNo
                             }).FirstOrDefault();

                if (regno.CourseRegistrationNo == null)
                {
                    txtRegNo.Enabled = true;
                    rdbCourseType.Enabled = true;
                }
                else
                {
                    txtRegNo.Text = Convert.ToString(regno.CourseRegistrationNo);
                    txtRegNo.Enabled = false;
                    rdbCourseType.Enabled = false;
                }
            }
        }
        else if (rdbCourseType.SelectedValue.Trim() == "A")
        {
            lblregistrationNo.Text = "Application No.";
            using (EConnectContext context1 = new EConnectContext())
            {
                string Number = txtrefno.Text;
                var regno = (from a in context1.CertificateExamApplications

                             where a.OnlineRefID == Number
                             select new
                             {
                                 ApplicationNumber = a.Number
                             }).FirstOrDefault();

                if (regno != null)
                {
                    txtRegNo.Text = Convert.ToString(regno.ApplicationNumber);
                    txtRegNo.Enabled = false;
                    rdbCourseType.Enabled = false;
                }
                else
                {
                    txtRegNo.Enabled = true;
                    rdbCourseType.Enabled = true;
                }
            }
            //txtRegNo.Enabled = true;
        }


    }

    protected void btnShow_Click(object sender, System.EventArgs e)
    {
        try
        {
           

            BindBatch();
            lblerror.Text = "t1";
            lblerror.Visible = true;
            tralready.Visible = true;
            if (txtrefno.Text.Trim() == "")
            {
                lblerror.Visible = true;
                txtrefno.Text = "";
                lblerror.Text = "Please Enter Reference Number.";
            }
            else
            {
                ResetAll();
                string Number = txtrefno.Text;
                User objUser;
                Int32 NielitCentrelinkedToCentreIdForStudentRecordEdit = 0;

                using (EConnectContext context1 = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();

                    User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);

                    using (NIELITMISContext context = new NIELITMISContext())
                    {


                        //  var applicationcentre = context.RegistrationDetails.Where(p => p.RegistrationNo == Number).FirstOrDefault();
                        /*
                        var Center = (from t in context.NielitCentres
                                      where t.ID == UserRefNumber
                                      select new
                                      {
                                          Id = t.ID


                                      }).FirstOrDefault();

                        var Centre = context.NielitCentres.Find(loginUser.UserRefNumber);

*/



                        var application = (from a in context.NielitCentreStudent
                                           // join i in context.NielitCentreBatch
                                           // on a.centreID equals i.centreID
                                           where a.Number == Number
                                           && a.whetherPlaced == false
                                           && a.whether_Course_Complete == false
                                           && a.whether_Certificate_Issued == false
                                           //&& a.whetherProjectStudent == false
                                           select new
                                           {

                                               CourseId = a.CourseID,
                                               BatchID = a.batch_ID,
                                               Salutation = a.Salutation,
                                               project = a.projectId,
                                               Name = a.Name,
                                               GuardianName = a.GuardianName,
                                               FatherName = a.FatherName,
                                               MotherName = a.MotherName,
                                               Gender = a.Gender,
                                               DateOfBirth = a.DateOfBirth,
                                               CastCategoryID = a.CastCategoryID,
                                               MaritalStatusID = a.MaritalStatusID,
                                               IsHandicaped = a.IsHandicaped ? 1 : 0,
                                               IsExServicemane = a.IsExServicemane ? 1 : 0,
                                               Is_EWS = a.Is_EWS ? 1 : 0,
                                               ReligionID = a.ReligionID,
                                               PhoneNumber = a.PhoneNumber,
                                               StdNumber = a.StdNumber,
                                               MobileNumber = a.MobileNumber,
                                               EmailAddress = a.EmailAddress,
                                               PerAddressLine1 = a.PerAddressLine1,
                                               PerAddressLine2 = a.PerAddressLine2,
                                               PerAddressLine3 = a.PerAddressLine3,
                                               PerCityName = a.PerCityName,
                                               PerStateID = a.PerStateID,
                                               PerDistrictID = a.PerDistrictID,
                                               PerPinCode = a.PerPinCode,
                                               CorAddressLine1 = a.CorAddressLine1,
                                               CorAddressLine2 = a.CorAddressLine2,
                                               CorAddressLine3 = a.CorAddressLine3,
                                               CorCityName = a.PerCityName,
                                               CorStateID = a.CorStateID,
                                               CorDistrictID = a.CorDistrictID,
                                               CorPinCode = a.CorPinCode,
                                               UIDType = a.UIDType,
                                               UIDNumber = a.UIDNumber,
                                               CenterId = a.InstituteID,
                                               whetherProject = a.whetherProjectStudent ? 1 : 0
                                               //  affidavit_No=a.affidavitNo,
                                               // affidavit_Date=a.affidavitDate,
                                               //affidavit_Verified=a.affidavitVerified

                                           }).FirstOrDefault();



                        //Candidate Personal Detail...
                        if (application != null)
                        {
                            // deep add code on 22 june 2021                           
                            string AccessRecord = "No";
                            NIELITMISContext context2 = new NIELITMISContext();
                            if (UserTypeid == 10)
                            {
                                var intituteslinkedToCentre = (from s in context2.NonAffInstitutes
                                                               join r in context2.NielitCentreStudent on s.ID equals r.InstituteID
                                                               where s.linkedToCentre == UserRefNumber && r.Number == Number
                                                               select new { Id = s.ID, LinktoCentreID = s.linkedToCentre }).ToList();
                                if (intituteslinkedToCentre.Count > 0)
                                {
                                    AccessRecord = "Yes";
                                }
                                else
                                {
                                    var intituteslinkedToCentreAffl = (from s in context2.AffInstitutes
                                                                       join r in context2.NielitCentreStudent on s.instituteID equals r.InstituteID
                                                                       where s.linkedToCentre == UserRefNumber && r.Number == Number
                                                                       select new { Id = s.ID, LinktoCentreID = s.linkedToCentre }).ToList();
                                    if (intituteslinkedToCentreAffl.Count > 0)
                                    {
                                        AccessRecord = "Yes";
                                    }
                                }
                            }
                            // deep add code end on 22 june 2021

                            if (UserRefNumber == application.CenterId || AccessRecord == "Yes")
                            {
                                RadioButtonListCentre.SelectedValue = "Y";
                                RadioButtonListCentre_SelectedIndexChanged(sender, e);

                                //  ShowAlert("Please enter correct data and ensure to cross check the data before saving.");
                                RdoAffInstOrNonAffInst.Enabled = false;
                                lblerror.Visible = true;
                                lblerror.Text = "Warning !! Please enter correct data and ensure to cross check the data before saving.";
                                TblFormDetail.Visible = true;
                                trremarks.Visible = true;
                                trcenter.Visible = true;
                                trcourse.Visible = true;
                                trproject.Visible = true;
                                ddlCourse.Enabled = false;

                                //Added 6 Sep 2021
                                SubcetreIdByApplication = Convert.ToInt32(application.CenterId);

                                ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);


                                ddlCourse.SelectedValue = application.CourseId != null && application.CourseId != 0 ? application.CourseId.ToString() : "0";
                                // int id3 = Convert.ToInt32(ddlCourse.SelectedValue);

                                ddlCourse_SelectedIndexChanged(ddlCourse, EventArgs.Empty);
                                ddlCourse.Enabled = false;
                                //  ddlBatch.Enabled = true;
                                BindBatch();
                                //ListItem item = new ListItem();
                                //item = ddlBatch.Items.FindByValue(application.BatchID.ToString());
                                //if (item == null)
                                //{
                                //    ShowAlert("Batch has already ended, Editing not allowed");
                                //    return;
                                //}





                                /*ddlCourse.SelectedValue = application.CourseId != null && application.CourseId != 0 ? application.CourseId.ToString() : "0";
                                // int id3 = Convert.ToInt32(ddlCourse.SelectedValue);

                                ddlCourse_SelectedIndexChanged(ddlCourse, EventArgs.Empty);*/
                                //  ddlBatch.Enabled = true;
                                ddlBatch.SelectedValue = application.BatchID.ToString();
                                if (application.whetherProject.ToString() == "1")
                                {
                                    ddlWheatherPojectStu.SelectedValue = "1";
                                    ddlProcname.SelectedValue = application.project.ToString();
                                }
                                else if (application.whetherProject.ToString() == "0")
                                {
                                    ddlWheatherPojectStu.SelectedValue = "2";
                                }

                                //ddlWheatherPojectStu.SelectedValue = application.whetherProject;
                                ddlSalutaionName.SelectedValue = application.Salutation.ToString();
                                txtAppName.Text = GetInitCap(application.Name.ToString());
                                lblname.Text = txtAppName.Text;
                                if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName) == true)
                                {
                                    trfather.Visible = true;
                                    trmother.Visible = true;
                                    trguardian.Visible = false;
                                    Rdoownertype.SelectedValue = "P";
                                    Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                                    txtFatherName.Text = string.IsNullOrEmpty(application.FatherName) == false && !string.IsNullOrWhiteSpace(application.FatherName) ? GetInitCap(application.FatherName) : "";
                                    txtMotherName.Text = string.IsNullOrEmpty(application.MotherName) == false && !string.IsNullOrWhiteSpace(application.MotherName) ? GetInitCap(application.MotherName) : "";
                                }
                                else
                                {
                                    trfather.Visible = false;
                                    trmother.Visible = false;
                                    trguardian.Visible = true;
                                    Rdoownertype.SelectedValue = "G";
                                    Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                                    TxtGuardianName.Text = GetInitCap(application.GuardianName);
                                }

                                ddl_gender.SelectedValue = application.Gender;
                                txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");

                                ddlCategory.SelectedValue = application.CastCategoryID.ToString();
                                ddlMStatus.SelectedValue = application.MaritalStatusID.ToString();
                                if (application.IsHandicaped.ToString() == "1")
                                {
                                    Rdhandicapped.SelectedValue = "Y";
                                }
                                else
                                {
                                    Rdhandicapped.SelectedValue = "N";
                                }
                                if (application.IsExServicemane.ToString() == "1")
                                {
                                    Rdexserviceman.SelectedValue = "Y";
                                }
                                else
                                {
                                    Rdexserviceman.SelectedValue = "N";
                                }

                                if (application.Is_EWS.ToString() == "1")
                                {
                                    RdisEWS.SelectedValue = "Y";
                                }
                                else
                                {
                                    RdisEWS.SelectedValue = "N";
                                }

                                ddlReligion.SelectedValue = application.ReligionID.ToString();


                                TxtSTDcode.Text = application.PhoneNumber.HasValue && application.PhoneNumber != 0 ? "0" + application.StdNumber.ToString() : "";
                                txtCorPhoneNo.Text = application.PhoneNumber.ToString();
                                txtCorMobileNo.Text = application.MobileNumber.ToString();
                                txtEmailId.Text = application.EmailAddress.ToString();


                                //Candidate Address Detail...
                                TxtPerAddressLine1.Text = string.IsNullOrEmpty(application.PerAddressLine1) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine1) ? GetInitCap(application.PerAddressLine1) : "";
                                TxtPerAddressLine2.Text = string.IsNullOrEmpty(application.PerAddressLine2) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine2) ? GetInitCap(application.PerAddressLine2) : "";
                                TxtPerAddressLine3.Text = string.IsNullOrEmpty(application.PerAddressLine3) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine3) ? GetInitCap(application.PerAddressLine3) : "";
                                TxtPerCity.Text = string.IsNullOrEmpty(application.PerCityName) == false && !string.IsNullOrWhiteSpace(application.PerCityName) ? GetInitCap(application.PerCityName) : "";
                                ddlPState.SelectedValue = application.PerStateID != null && application.PerStateID != 0 ? application.PerStateID.ToString() : "0";
                                int id1 = Convert.ToInt32(ddlPState.SelectedValue);
                                ddlPState_SelectedIndexChanged(ddlPState, EventArgs.Empty);
                                ddlPdistrict.SelectedValue = application.PerDistrictID.ToString();
                                TxtPpincode.Text = application.PerPinCode.ToString();

                                TxtCorAddressLine1.Text = string.IsNullOrEmpty(application.CorAddressLine1) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine1) ? GetInitCap(application.CorAddressLine1) : "";
                                TxtCorAddressLine2.Text = string.IsNullOrEmpty(application.CorAddressLine2) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine2) ? GetInitCap(application.CorAddressLine2) : "";
                                TxtCorAddressLine3.Text = string.IsNullOrEmpty(application.CorAddressLine3) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine3) ? GetInitCap(application.CorAddressLine3) : "";
                                TxtCorCity.Text = string.IsNullOrEmpty(application.CorCityName) == false && !string.IsNullOrWhiteSpace(application.CorCityName) ? GetInitCap(application.CorCityName) : "";
                                ddlCorState.SelectedValue = application.CorStateID.ToString();
                                int id2 = Convert.ToInt32(ddlCorState.SelectedValue);
                                ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                                ddlcordistrict.SelectedValue = application.CorDistrictID.ToString();
                                txtCorPinCode.Text = application.CorPinCode.ToString();
                                UidTypeDdl.SelectedValue = application.UIDType.ToString();

                                if (UidTypeDdl.SelectedValue.ToString() == "1")
                                {
                                    UidNumberTxt.Text = application.UIDNumber.ToString();
                                    string decr = EConnect.Utils.Security.Decryption.Decrypt(UidNumberTxt.Text);
                                    UidNumberTxt.Text = decr;
                                }
                                else
                                    UidNumberTxt.Text = application.UIDNumber.ToString();
                                // UidNumberTxt.Text = application.UIDNumber.ToString();




                            }
                            else if (UserRefNumber != application.CenterId || AccessRecord == "Yes")
                            {
                                TblFormDetail.Visible = false;
                                trremarks.Visible = false;
                                trcenter.Visible = false;
                                trcourse.Visible = false;
                                trproject.Visible = false;
                                lblerror.Visible = true;
                                lblerror.Text = "Student does not belong to this center,please check!!.";

                            }
                        }
                        if (application == null)
                        {
                            TblFormDetail.Visible = false;
                            trremarks.Visible = false;
                            trcenter.Visible = false;
                            trcourse.Visible = false;
                            trproject.Visible = false;
                            lblerror.Visible = true;
                            tralready.Visible = false;
                            lblerror.Text = "Reference Number does not exist for this centre!!.";
                        }
                    }


                }
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
}