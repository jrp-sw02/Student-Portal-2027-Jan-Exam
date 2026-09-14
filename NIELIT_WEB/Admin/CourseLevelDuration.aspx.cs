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

public partial class Admin_CourseLevelDuration : BasePage
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

            //deeptest on 10 may21 delete between and uncomment
            //UserTypeId = 10;
            //currentRoleId = 21;
            //loginUserNo = 285716;

            //deeptest on 10 may21 delete between and uncomment
            if (!Page.IsPostBack)
            {
                User objUser;
                using (EConnectContext context = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();
                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                   // RegionalCenter RegName = context.RegionalCenters.Find(loginUser.UserRefNumber);
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        FillCategories();
                        FillCategoriesF();
                        ShowEditMode();
                    }
                    else
                    {
                        using (NIELITMISContext context1 = new NIELITMISContext())
                        {
                            ViewState["SortField"] = "";
                            ViewState["SortOrder"] = "";
                            FillCategories();
                            FillCategoriesF();
                            BindGridView();
                            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                            {
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Course Level Durations List", "Admin/CourseLevelDuration.aspx?Id=" + Request.QueryString["Id"].ToString(), ""));
                            }
                            else
                            {
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Course Level Durations List", "Admin/CourseLevelDuration.aspx", ""));
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
    protected void FillCategories()
    {
        try
        {
            ddlcoursecategory.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.CourseCategories
                               where p.IsActive == true
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillCategoriesF()
    {
        try
        {

            ddlcoursecategoryF.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.CourseCategories
                               where p.IsActive == true
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategoryF, Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 coursecatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == coursecatID && p.ShowOnWeb == true && p.IsActive == true
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourseName, CourseList, lst);

            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void ShowEditMode()
    {
        try
        {
           
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseLevelDurationId = 0;
                CourseLevelDurationId = Convert.ToInt32(Request.QueryString["Key"]);
                CourseLevelDurations editCourse = context.CourseLevelDurationss.Find(CourseLevelDurationId);
                var courseCnt = from c in context.CourseLevelDurationss
                                where c.Effective_To_Date == null
                                && c.ID == CourseLevelDurationId
                                select c;
                if (courseCnt != null)
                {
                    int cnt = courseCnt.Count();
                    if (cnt == 0)
                    {
                        btnMode.ViewMode = ToggleView.Mode.List;

                      
                        throw new Exception("Record cannot be modified, Already effective To date is available");
                      
                        return;
                    }
                }
                else
                    {
                        btnMode.ViewMode = ToggleView.Mode.List;
                        throw new Exception("Record cannot be modified, Already effective To date is available");
                        return;
                    }

                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;

                Label10.Visible = true;
                txtEffectiveToDate.Visible = true;


                btnSave.Text = "Update";
                lblHeading.Text = "Course Level Durations";

                var course = (from p in context.CourseLevelDurationss
                              where p.ID == CourseLevelDurationId
                              select new
                              {
                                  ID = p.ID,
                                  //Name = c.Name,
                                  CourseCatID = p.CourseCategoryID,
                                  CourseID = p.CourseID,
                                  courseLevelDurationHrs = p.Course_Level_DurationHrs,
                                  courseLevelNo = p.Course_Level_no,
                                  courseLevelRevisionNo = p.Course_Level_Revision_no,
                                  EffectiveFromDate = p.Effective_From_Date,
                                  EffectrivetoDate = p.Effective_To_Date
                              }).FirstOrDefault();
                txtcourseLevelDurationHrs.Text = course.courseLevelDurationHrs.ToString();
                txtCourseLevelNo.Text = course.courseLevelNo.ToString();
                txtcourseLevelRevisionNo.Text = course.courseLevelRevisionNo.ToString();
                txtEffectiveFromDate.Text = Convert.ToDateTime(course.EffectiveFromDate).ToString("dd-MMM-yyyy");
                if (course.EffectrivetoDate.ToString() != "")
                {
                    txtEffectiveToDate.Text = Convert.ToDateTime(course.EffectrivetoDate).ToString("dd-MMM-yyyy");
                }
                ddlcoursecategory.SelectedValue = course.CourseCatID.ToString();
                

                Int32 coursecatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);

                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == coursecatID && p.ShowOnWeb == true && p.IsActive == true
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourseName, CourseList, lst);



                ddlcourseName.SelectedValue = course.CourseID.ToString();
                ddlcoursecategory.Enabled = false;
                ddlcourseName.Enabled = false;
                txtcourseLevelRevisionNo.Enabled = false;
                txtCourseLevelNo.Enabled = false;
                txtcourseLevelDurationHrs.Enabled = false;
                txtEffectiveFromDate.Enabled = false;

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
        Response.Redirect("CourseLevelDuration.aspx", true);
    }
    protected void BindGridView()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                int courseNameF = 0, courseCatF = 0;
                Int32 courseLevelNo = 0;
                if (ddlCourseNameF.SelectedValue != "0")
                    courseNameF = Convert.ToInt32(ddlCourseNameF.SelectedValue);
                if (ddlCourseLevelNo.SelectedValue != "0")
                    courseLevelNo = Convert.ToInt32(ddlCourseLevelNo.SelectedValue);
                if (ddlcoursecategoryF.SelectedValue != "0")
                    courseCatF = Convert.ToInt32(ddlcoursecategoryF.SelectedValue);
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();

                var courses = (from s in context.CourseLevelDurationss
                               join c in context.Courses on s.CourseID equals c.ID
                               join w in context.CourseCategories on s.CourseCategoryID equals w.ID
                               orderby c.ID,s.Course_Level_Revision_no descending,s.ID descending
                               select new
                               {
                                   ID = s.ID,
                                   coursecatName = w.Name,
                                   Name = c.Name,
                                   CourseId = s.CourseID,
                                   CourseCatId = s.CourseCategoryID,
                                   courseLevelDurationHrs = s.Course_Level_DurationHrs,
                                   courseLevelNo = s.Course_Level_no,
                                   courseLevelRevisionNo = s.Course_Level_Revision_no,
                                   enterbyy = s.enterBy,
                                   Effective_From_Date = s.Effective_From_Date,
                                   //Effective_To_Date = s.Effective_To_Date.Value==null ? s.Effective_To_Date.Value : s.Effective_To_Date.Value.Date
                                   Effective_To_Date = s.Effective_To_Date == null ? (DateTime?)null : s.Effective_To_Date
                               }
                               
                               );

                if (!String.IsNullOrEmpty(searchString))
                {
                    courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
                }
                if (courseCatF != 0)
                    courses = courses.Where(s => s.CourseCatId == courseCatF);
                if (courseNameF != 0)
                    courses = courses.Where(s => s.CourseId == courseNameF);
                if (courseLevelNo != 0)
                    courses = courses.Where(s => s.courseLevelNo == courseLevelNo);

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
                    gvMain.Columns[8].Visible = false;
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


            //Change the heading text as required
            lblHeading.Text = "New Course Level Durations";
            //Updating Breadcrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Course Level Durations", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("CourseLevelDuration.aspx?ID=" + Request.QueryString["ID"].ToString()), true);
            }
            else
            {
                Response.Redirect("CourseLevelDuration.aspx", true);
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
            FillCategories();
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SaveRecordCourseLevelDuration(object sender, EventArgs e) //SaveRecordNielitCourse
    {
        try
        {
            BreadCrumb1.Render();
            if (txtCourseLevelNo.Text == "")
            {
                txtCourseLevelNo.Text = "0";
            }
            using (EConnectContext context = new EConnectContext())
            {

                //create and object 
                CourseLevelDurations currentCourseLevelDuration;
              
                Int32 courseId = 0, courseLevelDurationHrs = 0, courseLevelNo = 0, coursecatid = 0, CourseLevelRevisionno = 0;
                DateTime effectivefromDateS,effectiveToDateS;
                effectivefromDateS = Convert.ToDateTime(txtEffectiveFromDate.Text);
                

                if (txtEffectiveToDate.Text.Trim().Length != 0)
                {
                    effectiveToDateS = Convert.ToDateTime(txtEffectiveToDate.Text);
                    if (effectivefromDateS > effectiveToDateS)
                    {
                        throw new Exception("Effective To Date should be greater than effective From date");
                        return;
                    }
                }
                coursecatid = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                courseId = Convert.ToInt32(ddlcourseName.SelectedValue);
                courseLevelDurationHrs = Convert.ToInt32(txtcourseLevelDurationHrs.Text);
                courseLevelNo = Convert.ToInt32(txtCourseLevelNo.Text);
                if (coursecatid == 6 && courseLevelNo == 0)
                {
                    throw new Exception("Course Level No should not be blank or 0.");
                    return;
                }

                CourseLevelRevisionno = Convert.ToInt32(txtcourseLevelRevisionNo.Text);
                string courseName = ddlcourseName.SelectedItem.Text;
                var courseDurationList1 = (from p in context.CourseLevelDurationss
                                           where p.CourseCategoryID == coursecatid && p.CourseID == courseId && p.Course_Level_DurationHrs == courseLevelDurationHrs
                                           && p.Course_Level_Revision_no == CourseLevelRevisionno && p.Effective_From_Date == effectivefromDateS
                                           select p).ToList();
                //var courseDurationRecordExists = (from p in context.CourseLevelDurationss
                //                                  where p.CourseCategoryID == coursecatid && p.CourseID == courseId 
                //                                  //&& p.Effective_From_Date == effectivefromDateS
                //                                  select p).ToList();
                var courseDurationCheck = (from p in context.CourseLevelDurationss
                                           where p.CourseCategoryID == coursecatid && p.CourseID == courseId
                                          && p.Effective_To_Date == null
                                           select p).ToList();

                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                   
                    if (courseDurationCheck.Count != 0)
                    {
                        throw new Exception("Course already exists with null effective to date, Please use update for revision changes");
                        return;
                    }
                    //if (courseDurationRecordExists.Count != 0)
                    //{
                    //    throw new Exception("Course Category, Course Name should not be same, Please use update for revision changes");
                    //    return;
                    //}
                    if (courseDurationList1.Count == 0)
                    {

                        currentCourseLevelDuration = new CourseLevelDurations();
                        currentCourseLevelDuration.CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                        currentCourseLevelDuration.CourseID = Convert.ToInt32(ddlcourseName.SelectedValue);//course  id                   
                        currentCourseLevelDuration.Course_Level_DurationHrs = Convert.ToInt32(txtcourseLevelDurationHrs.Text);// course Level Duration Hrs
                        if (courseLevelNo == 0)
                        {
                            currentCourseLevelDuration.Course_Level_no = null;
                        }
                        else
                        {
                            currentCourseLevelDuration.Course_Level_no = Convert.ToInt32(txtCourseLevelNo.Text);// course Level No
                        }
                        currentCourseLevelDuration.Course_Level_Revision_no = Convert.ToInt32(txtcourseLevelRevisionNo.Text);// course Level Revision No
                        currentCourseLevelDuration.Effective_From_Date = Convert.ToDateTime(txtEffectiveFromDate.Text);
                        if (txtEffectiveToDate.Text != "")
                        {
                            currentCourseLevelDuration.Effective_To_Date = Convert.ToDateTime(txtEffectiveToDate.Text);
                        }

                        currentCourseLevelDuration.enterDate = DateTime.Now; // entered course date by which
                        currentCourseLevelDuration.enterBy = Convert.ToInt32(Session["UserID"]);

                        context.CourseLevelDurationss.Add(currentCourseLevelDuration);//save
                        context.SaveChanges();
                        strMessage = "New Record Saved";

                    }
                }
                else
                {
                    /*
                    if (txtEffectiveToDate.Text.Trim().Length == 0)
                    {
                        throw new Exception("Please enter Effective to date");
                        return;
                    }
                     * */
                  //  effectiveToDateS = Convert.ToDateTime(txtEffectiveToDate.Text);
                    /*
                    var courseDurationDate = (from p in context.CourseLevelDurationss
                                               where p.CourseCategoryID == coursecatid && p.CourseID == courseId 
                                             // && (p.Effective_From_Date >= effectivefromDateS || p.Effective_From_Date <=effectiveToDateS ) 
                                              && p.Effective_To_Date == null
                                               select p).ToList();
                    if (courseDurationDate.Count != 0)
                    {
                        throw new Exception("Please check Effective from and effective To dates , they cannot be before already entered effective date.");
                        return;
                    }
                     * */
                    using (TransactionScope scope = new TransactionScope())
                    {
                        currentCourseLevelDuration = new CourseLevelDurations();
                       
                        var courseDurationUpdate = (from p in context.CourseLevelDurationss
                                                    where p.CourseCategoryID == coursecatid && p.CourseID == courseId
                                                   && p.Effective_To_Date == null
                                                    select p).FirstOrDefault();
                        currentCourseLevelDuration = context.CourseLevelDurationss.Find(courseDurationUpdate.ID);
                        if (txtEffectiveToDate.Text != "")
                        {
                            currentCourseLevelDuration.Effective_To_Date = Convert.ToDateTime(txtEffectiveToDate.Text);
                        }
                        currentCourseLevelDuration.enterDate = DateTime.Now; // entered course date by which
                        currentCourseLevelDuration.enterBy = Convert.ToInt32(Session["UserID"]);


                        context.SaveChanges();
                       
                        scope.Complete();
                        strMessage = "Revision changes Saved";
                    }
                }
            }
            Response.Redirect("CourseLevelDuration.aspx?msg=" + strMessage, true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
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
            ddlCourseLevelNo.SelectedValue = "0";
            FillCategories();
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
        Response.Redirect("CourseLevelDuration.aspx", true);
    }

    protected void ddlcoursecategoryF_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 coursecatID = Convert.ToInt32(ddlcoursecategoryF.SelectedValue);

            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                if (coursecatID != null || coursecatID != 0)
                {
                    var CourseName = from p in context.Courses
                                     where p.CourseCategoryID == coursecatID && p.ShowOnWeb == true && p.IsActive == true
                                     orderby (p.Name)
                                     select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseNameF, CourseName, lst);
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
            Int32 coursecatID = Convert.ToInt32(ddlcoursecategoryF.SelectedValue);
            Int32 courseID = Convert.ToInt32(ddlCourseNameF.SelectedValue);
            ListItem lst = new ListItem("--All--", "0");
            using (EConnectContext context = new EConnectContext())
            {

                var CourseLevelNo = from p in context.CourseLevelDurationss
                                    where p.CourseCategoryID == coursecatID && p.CourseID == courseID
                                    orderby (p.Course_Level_no)
                                    select new { ValueField = p.Course_Level_no, TextField = p.Course_Level_no };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseLevelNo, CourseLevelNo, lst);

            };
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
        EConnectContext context = new EConnectContext();
        try
        {
            loginUserNo = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            UserTypeId = Convert.ToInt32(HttpContext.Current.Session["UserTypeId"]);
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();



            string searchString = prefixText.Trim().ToUpper();
            var courses = from s in context.CourseLevelDurationss
                          join c in context.Courses on s.CourseID equals c.ID
                          join w in context.CourseCategories on s.CourseCategoryID equals w.ID
                          select new { Name = c.Name, CatName = w.Name, courselevelNo = s.Course_Level_no };
            courses = courses.Distinct();

            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString) || s.CatName.ToUpper().Contains(searchString) || s.courselevelNo.ToString().Contains(searchString));
            }
            courses = courses.OrderBy(s => s.Name);
            //courses = courses.OrderBy(s => s.CatName).Take(count);
            //courses = courses.OrderBy(s => s.courselevelNo).Take(count);
            foreach (var course in courses)
            {
                items.Add(course.Name);
                //items.Add(course.CatName);
                //items.Add(course.courselevelNo.ToString().Distinct());
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