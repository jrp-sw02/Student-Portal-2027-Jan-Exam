using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Common;
using System.Text.RegularExpressions;
using EConnect.NIELIT;
using System.Web;
using System.Transactions;
using System.Data.Objects;
using EConnect;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO.Compression;

public partial class Admin_NielitCourseDuration : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 UserTypeId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                User objUser;
                using (EConnectContext context = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();
                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    RegionalCenter RegName = context.RegionalCenters.Find(loginUser.UserRefNumber);
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        FillCourseName();
                        ShowEditMode();
                    }
                    else
                    {
                        using (NIELITMISContext context1 = new NIELITMISContext())
                        {
                            ViewState["SortField"] = "";
                            ViewState["SortOrder"] = "";
                            FillCourseName();
                            FillFilterCourseName();
                            FillCoursesDurationForFilterV();
                            BindGridView();
                            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                            {
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Course  Durations List", "Admin/NielitCourseDuration.aspx?Id=" + Request.QueryString["Id"].ToString(), ""));
                            }
                            else
                            {
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Course  Durations List", "Admin/NielitCourseDuration.aspx", ""));
                            }
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
    protected void FillCourseName()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.NielitCentreCourses
                                 where p.IsVerified == true
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourseName, CourseList, lst);
                ListItem lst1 = new ListItem("Not Available", "99");
                var verifyStatus = from p in context.verifyStatusMass
                                   orderby (p.verifyDescription)
                                   select new { ValueField = p.ID, TextField = p.verifyDescription };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlverifiedStatus, verifyStatus, lst1);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillFilterCourseName()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                if (UserTypeId == 10)
                {
                    var CourseList = from p in context.NielitCentreCourses
                                     join c in context.NielitCourseDurations on p.ID equals c.courseID
                                     where c.enterBy == loginUserNo
                                     orderby (p.Name)
                                     select new { ValueField = c.ID, TextField = p.Name + " ( " + c.courseDurationDays + " Days )" };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseNameF, CourseList, lst);
                }
                else
                {
                    var CourseList = from p in context.NielitCentreCourses
                                     join c in context.NielitCourseDurations on p.ID equals c.courseID
                                     orderby (p.Name)
                                     select new { ValueField = c.ID, TextField = p.Name + " ( " + c.courseDurationDays + " Days )" };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseNameF, CourseList, lst);
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ShowEditMode()
    {
        try
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            this.Rview.Visible = true;
            this.Rview1.Visible = true;
            btnSave.Text = "Update";
            lblHeading.Text = "NIELIT  Course Durations";
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int32 NielitCourseDurationId = 0;
                NielitCourseDurationId = Convert.ToInt32(Request.QueryString["Key"]);
                NielitCourseDuration editCourse = context.NielitCourseDurations.Find(NielitCourseDurationId);
                var course = (from p in context.NielitCourseDurations
                              join c in context.NielitCentreCourses on p.courseID equals c.ID
                              where p.ID == NielitCourseDurationId
                              select new
                              {
                                  ID = p.ID,
                                  Name = c.Name,
                                  CourseID = p.courseID,
                                  CourseCatID = c.CourseCategoryID,
                                  courseDurationHrs = p.courseDurationHrs,
                                  courseDurationDays = p.courseDurationDays,
                                  courseDurationYears = p.courseDurationYears,
                                  Isverified = p.isVerified == true ? "YES" : "NO",
                                  VerifiedStatus = p.verifiedStatus,
                                  VerificationMessage = p.verificationMessage ?? "Not Available"
                              }).FirstOrDefault();
                txtcourseDurationHrs.Text = course.courseDurationHrs.ToString();
                txtcourseDurationDays.Text = course.courseDurationDays.ToString();
                if (course.CourseCatID == 101)
                {
                    rvYear.Visible = true;
                    rvYear1.Visible = true;
                    txtcourseDurationYears.Text = course.courseDurationYears.ToString();
                }
                else
                {
                    rvYear.Visible = false;
                    rvYear1.Visible = false;
                }
                ddlcourseName.SelectedValue = course.CourseID.ToString();
                ddlcourseName.Enabled = false;
                TxtverificationMessage.Text = course.VerificationMessage;
                if (course.VerifiedStatus.ToString() != "")
                {
                    ddlverifiedStatus.SelectedValue = course.VerifiedStatus.ToString();
                }
                else
                {

                    ddlverifiedStatus.SelectedValue = "99";
                }
                if (course.Isverified.ToString() != "")
                {
                    ddlIsVerified.SelectedValue = course.Isverified.ToString();
                }
                if (course.Isverified == "YES")
                {
                    ddlIsVerified.SelectedValue = "1";
                    txtcourseDurationHrs.Enabled = false;
                    txtcourseDurationDays.Enabled = false;
                    txtcourseDurationYears.Enabled = false;
                }
                else if (course.Isverified == "NO")
                {
                    ddlIsVerified.SelectedValue = "2";
                    txtcourseDurationHrs.Enabled = false;
                    txtcourseDurationDays.Enabled = false;
                    txtcourseDurationYears.Enabled = false;
                }
                else
                {
                    ddlIsVerified.SelectedValue = "3";
                }
                ddlIsVerified.Enabled = false;

                if (UserTypeId == 10)
                {
                    ddlIsVerified.Enabled = false;
                    this.Rview.Visible = true;
                    this.Rview1.Visible = true;
                }
                else
                {
                    if (course.Isverified == "YES")
                    {
                        ddlIsVerified.SelectedValue = "1";
                    }
                    else if (course.Isverified == "NO")
                    {
                        ddlIsVerified.SelectedValue = "2";
                    }
                    else
                    {
                        ddlIsVerified.SelectedValue = "3";
                    }
                    ddlIsVerified.Enabled = true;
                   
                }
            };
            if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            {
                btnSave.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("NielitCourseDuration.aspx", true);
    }
    protected void BindGridView()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                int courseNameF = 0, courseDurationMonths = 0;
                Int32 courseverifiedStatus = 99;
                if (ddlCourseNameF.SelectedValue != "0")
                    courseNameF = Convert.ToInt32(ddlCourseNameF.SelectedValue);
                if (ddlCourseVerifiedStatus.SelectedValue != "99")
                    courseverifiedStatus = Convert.ToInt32(ddlCourseVerifiedStatus.SelectedValue);
                if (ddlCourseDuration.SelectedValue != "0")
                    courseDurationMonths = Convert.ToInt32(ddlCourseDuration.SelectedValue);
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();

                var courses = (from s in context.NielitCourseDurations
                               join c in context.NielitCentreCourses on s.courseID equals c.ID
                               join w in context.verifyStatusMass on s.verifiedStatus equals w.ID
                                        into k
                               from verifyStatusMass in k.DefaultIfEmpty()
                               //where s.enterBy == loginUserNo
                               select new
                               {
                                   ID = s.ID,
                                   Name = c.Name,
                                   CourseId = s.courseID,
                                   courseDurationHrs = s.courseDurationHrs,
                                   courseDurationDays = s.courseDurationDays,
                                   enterbyy = s.enterBy,
                                   Verifiedd = s.isVerified == true ? "YES" : "NO",
                                   verifiedstatusId = s.verifiedStatus,
                                   VerifiedStatus = verifyStatusMass.verifyDescription,
                                   VerifiedOn = s.verifiedOn,
                               });
                if (currentRoleId == 1 || currentRoleId == 6 || currentRoleId == 27) // check users role  admin NOT  THEN RECORD FILTER
                {
                    //courses = courses.Where(s => s.enterby == loginUserNo);
                }
                else
                {
                    courses = courses.Where(s => s.enterbyy == loginUserNo);
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
                }
                if (courseNameF != 0)
                    courses = courses.Where(s => s.ID == courseNameF);
                if (courseverifiedStatus != 99)
                    courses = courses.Where(s => s.verifiedstatusId == courseverifiedStatus);

                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "Name":
                            if (sortOrder == "DESC")
                                courses = courses.OrderByDescending(s => s.ID);
                            else
                                courses = courses.OrderBy(s => s.ID);
                            break;

                        case "Code":
                            if (sortOrder == "DESC")
                                courses = courses.OrderByDescending(s => s.ID);
                            else
                                courses = courses.OrderBy(s => s.ID);
                            break;

                        default:
                            courses = courses.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(courses, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                lblError.Visible = false;
                gvMain.Visible = true;
                if (gvMain.Rows.Count <= 0)
                {
                    lblError.Text = "No record found.";
                    lblError.Visible = true;
                    gvMain.Visible = false;
                }
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    gvMain.Columns[7].Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to add new record.");
                return;
            }
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;

            if (UserTypeId == 10)
            {
                ddlIsVerified.Enabled = false;
                this.Rview.Visible = false;
                this.Rview1.Visible = false;
            }
            //Change the heading text as required
            lblHeading.Text = "New Course Durations";
            //Updating Breadcrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New NIELIT Course Durations", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitCourseDuration.aspx?ID=" + Request.QueryString["ID"].ToString()), true);
            }
            else
            {
                Response.Redirect("NielitCourseDuration.aspx", true);
            }
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SearchBar_Reset(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlCourseNameF.SelectedValue = "0";
            FillCoursesDurationForFilterV();
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SaveRecordNielitCourseDuration(object sender, EventArgs e) //SaveRecordNielitCourse
    {
        try
        {
            BreadCrumb1.Render();
            using (NIELITMISContext context = new NIELITMISContext())
            {
                //create and object 
                NielitCourseDuration currentCourseDuration;
                Boolean isverified = false;
                 Int32 CourseId = Convert.ToInt32(ddlcourseName.SelectedValue);
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    currentCourseDuration = new NielitCourseDuration();
                    currentCourseDuration.courseID = Convert.ToInt32(ddlcourseName.SelectedValue);//course  id                   
                    currentCourseDuration.courseDurationHrs = Convert.ToInt32(txtcourseDurationHrs.Text);// course Duration Hrs
                    currentCourseDuration.courseDurationDays = Convert.ToInt32(txtcourseDurationDays.Text);// course completion duration Hrs
                    var application = (from p in context.NielitCentreCourses
                                       where p.ID == CourseId && p.IsVerified == true && p.CourseCategoryID == 101
                                       select new
                                       {
                                           ID = p.ID,
                                           Name = p.Name,
                                           CourseCatId = p.CourseCategoryID

                                       }).FirstOrDefault();

                    if (application != null)
                    {
                        if (txtcourseDurationYears.Text == "")
                        {
                            strMessage = "Please enter Course Duration Years";
                            lblyearerror.Text = "Please enter Course Duration Years";
                            txtcourseDurationYears.Focus();
                            return;
                        }
                        else
                        {
                            currentCourseDuration.courseDurationYears = Convert.ToInt32(txtcourseDurationYears.Text);// course completion duration Years
                        }
                }
                    Int32 courseId = 0, courseDurationHrs = 0, courseDurationDays = 0;
                    courseId = Convert.ToInt32(ddlcourseName.SelectedValue);
                    courseDurationHrs = Convert.ToInt32(txtcourseDurationHrs.Text);
                    courseDurationDays = Convert.ToInt32(txtcourseDurationDays.Text);
                    string courseName = ddlcourseName.SelectedItem.Text;
                    var courseDurationList1 = (from p in context.NielitCourseDurations
                                               where p.courseID == courseId && p.courseDurationHrs == courseDurationHrs && p.courseDurationDays == courseDurationDays
                                               select p).ToList();
                    if (courseDurationList1.Count == 0)
                    {
                        if (ddlIsVerified.SelectedValue == "1")
                        {
                            isverified = true;
                        }
                        if (ddlIsVerified.SelectedValue == "2")
                        {
                            isverified = false; ;
                        }
                        currentCourseDuration.enterDate = DateTime.Now; // entered course date by which
                        currentCourseDuration.enterBy = Convert.ToInt32(Session["UserID"]);
                        currentCourseDuration.isVerified = true;
                        currentCourseDuration.verificationMessage = "verified";
                        currentCourseDuration.verifiedStatus = 1;
                        currentCourseDuration.verifiedOn = DateTime.Now;
                        currentCourseDuration.verifiedBy = Convert.ToInt32(Session["UserID"]);
                        context.NielitCourseDurations.Add(currentCourseDuration);//save
                        context.SaveChanges();
                        strMessage = "New Record Saved";
                        SendEmailNielitCentreAuto(courseId, courseName, "Verified", Convert.ToInt32(Session["UserID"]));
                    }
                    else
                    {
                        if (ddlIsVerified.SelectedValue == "1")
                        {
                            isverified = true;
                        }
                        if (ddlIsVerified.SelectedValue == "2")
                        {
                            isverified = false; ;
                        }
                        currentCourseDuration.enterDate = DateTime.Now; // entered course date by which
                        currentCourseDuration.enterBy = Convert.ToInt32(Session["UserID"]);
                        currentCourseDuration.isVerified = false;
                        currentCourseDuration.verificationMessage = "Not verified on  " + DateTime.Now;
                        currentCourseDuration.verifiedStatus = 2;
                        currentCourseDuration.verifiedOn = DateTime.Now;
                        currentCourseDuration.verifiedBy = Convert.ToInt32(Session["UserID"]);
                        context.NielitCourseDurations.Add(currentCourseDuration);//save
                        context.SaveChanges();
                        strMessage = "New Record Saved";
                        SendEmailNielitCentreAuto(courseId, courseName, "Not Verified", Convert.ToInt32(Session["UserID"]));
                    }
                }
                else
                {
                    currentCourseDuration = new NielitCourseDuration();
                    currentCourseDuration = context.NielitCourseDurations.Find(Convert.ToInt32(Request.QueryString["key"]));
                    currentCourseDuration.courseDurationHrs = Convert.ToInt32(txtcourseDurationHrs.Text);// course Duration Hrs
                    currentCourseDuration.courseDurationDays = Convert.ToInt32(txtcourseDurationDays.Text);// course completion duration Hrs                   

                    if (ddlIsVerified.SelectedValue == "1")
                    {
                        isverified = true;
                    }
                    if (ddlIsVerified.SelectedValue == "2")
                    {
                        isverified = false; ;
                    }
                    context.SaveChanges();

                    strMessage = "Record Updated";
                }
            }
            Response.Redirect("NielitCourseDuration.aspx?msg=" + strMessage, true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SendEmailNielitCentreAuto(Int32 ID, string courseName, string verifiedStatuss, Int32 enterBy)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 enterByRefId = enterBy;

                var contactreg = (from s in context.Users
                                  // join p in context.ExternalEntities on s.UserRefNumber equals p.ID
                                  where s.UserID == enterByRefId
                                  select new { s.LoginID, s.UserTypeID, s.EmailID, s.MobileNumber }).FirstOrDefault();
                if (contactreg.EmailID != null)
                {
                    String EmailMsg = "Dear " + contactreg.LoginID + ", " + "This is regarding Course (" + courseName + ")"
                        + " for duration creation requested by you. The status of the course with specified duration is " + verifiedStatuss + ".";
                    //sending Email
                    if (contactreg.EmailID.Length > 0)
                    {
                        try
                        {
                            EConnect.NIELIT.Email mail = new Email("COURSE STATUS :NIELIT", EmailMsg, contactreg.EmailID);
                            mail.Send();
                        }
                        catch { ShowAlert("OTP is not sent on E-mail,"); }
                    }
                }
                else
                {
                    ShowAlert("Status is not sent on E-mail,");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            ddlCourseNameF.SelectedValue = "0";
            ddlCourseVerifiedStatus.SelectedValue = "99";
            FillCoursesDurationForFilterV();
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void PerformPopupAction(object sender, EventArgs e)
    {
        try
        {
            if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
            {
                BindGridView();
                uPnlGrid.Update();
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to delete the records.", true);
                return;
            }
            using (TransactionScope scope = new TransactionScope())
            {
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    Int32 courseid = Convert.ToInt32(hfActionID.Value);
                    EConnect.NIELIT.NielitCourseDuration course = context.NielitCourseDurations.Find(courseid);

                    context.NielitCourseDurations.Remove(course);
                    context.SaveChanges();
                    scope.Complete();
                    ShowAlert("Record deleted successfully.", true);
                    hfActionID.Value = "";
                };
            }
            BindGridView();
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be deleted!", true);
        }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                string href = hl.NavigateUrl;
                if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    href += "&Id=" + Request.QueryString["Id"].ToString();
                }
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
                HyperLink hl2 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

                Image imgAction = (Image)e.Row.FindControl("imgAction");
                imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();

                CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("NielitCourseDuration.aspx", true);
    }

    protected void FillCoursesDurationForFilterV()
    {
        try
        {
            ListItem lst = new ListItem("--All--", "0");
            ListItem lst1 = new ListItem("--All--", "0");
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ddlCourseDuration.Items.Clear();
                if (UserTypeId == 10)
                {
                    var CourseDuration = from p in context.NielitCourseDurations
                                         where p.enterBy == loginUserNo
                                         orderby (p.courseDurationDays)
                                         select new { ValueField = p.courseDurationDays, TextField = p.courseDurationDays };
                    CourseDuration = CourseDuration.Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseDuration, CourseDuration, lst);
                }
                else
                {
                    var CourseDuration = from p in context.NielitCourseDurations
                                         orderby (p.courseDurationDays)
                                         select new { ValueField = p.courseDurationDays, TextField = p.courseDurationDays };
                    CourseDuration = CourseDuration.Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseDuration, CourseDuration, lst1);
                }
                ListItem lst2 = new ListItem("--Select One--", "0");
                var verifyStatus = from p in context.verifyStatusMass
                                   orderby (p.verifyDescription)
                                   select new { ValueField = p.ID, TextField = p.verifyDescription };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlverifiedStatus, verifyStatus, lst2);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void FillCourseNames(int catID)
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                if (catID != null || catID != 0)
                {
                    var CourseName = from p in context.NielitCentreCourses
                                     where p.CourseCategoryID == catID

                                     orderby (p.Name)
                                     select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourseName, CourseName, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void ddlCourseNameF_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 coursecatID = Convert.ToInt32(ddlCourseNameF.SelectedValue);
            ListItem lst = new ListItem("--All--", "0");
            using (NIELITMISContext context = new NIELITMISContext())
            {
                if (UserTypeId == 10)
                {
                    var CourseDuration = from p in context.NielitCourseDurations
                                         where p.ID == coursecatID && p.enterBy == loginUserNo
                                         orderby (p.courseDurationDays)
                                         select new { ValueField = p.courseDurationDays, TextField = p.courseDurationDays };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseDuration, CourseDuration, lst);
                }
                else
                {
                    var CourseDuration = from p in context.NielitCourseDurations
                                         where p.ID == coursecatID
                                         orderby (p.courseDurationDays)
                                         select new { ValueField = p.courseDurationDays, TextField = p.courseDurationDays };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseDuration, CourseDuration, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void ddlcourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 CourseId = Convert.ToInt32(ddlcourseName.SelectedValue);
            ListItem lst = new ListItem("--All--", "0");
            using (NIELITMISContext context = new NIELITMISContext())
            {
                var application = (from p in context.NielitCentreCourses
                                   where p.ID == CourseId && p.IsVerified == true && p.CourseCategoryID == 101
                                   select new
                                   {
                                       ID = p.ID,
                                       Name = p.Name,
                                       CourseCatId = p.CourseCategoryID

                                   }).FirstOrDefault();

                if (application != null)
                {
                    rvYear.Visible = true;
                    rvYear1.Visible = true;
                }
                else if (application == null)
                {
                    rvYear.Visible = false;
                    rvYear1.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }


    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        Int32 loginUserNo = 0, UserTypeId = 0;
        NIELITMISContext context = new NIELITMISContext();
        try
        {
            loginUserNo = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            UserTypeId = Convert.ToInt32(HttpContext.Current.Session["UserTypeId"]);
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();

            string searchString = prefixText.Trim().ToUpper();
            var courses = from c in context.NielitCourseDurations
                          join s in context.NielitCentreCourses on c.courseID equals s.ID
                          select new { Name = s.Name, c.enterBy };
            courses = courses.Distinct();
            if (UserTypeId == 10)
            {
                courses = courses.Where(s => s.enterBy == loginUserNo);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            courses = courses.OrderBy(s => s.Name).Take(count);
            foreach (var course in courses)
            {
                items.Add(course.Name);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
}