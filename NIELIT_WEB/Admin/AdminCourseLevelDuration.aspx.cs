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
using DocumentFormat.OpenXml.Presentation;

public partial class AdminCourseLevelDuration : BasePage
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
            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
            //Commented for admin 
            //if (!UserManager.HasRight(currentRoleId, enmRight.View))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}

            if (!Page.IsPostBack)
            {
                User objUser;
                using (EConnectContext context = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();
                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    //RegionalCenter RegName = context.RegionalCenters.Find(loginUser.UserRefNumber);
                    Int32? prevCourse = null;
                    lblHdnCourse.Value = Request.QueryString["CourseId"];


                    if (Request.QueryString["filterCourse"] != null)
                    {
                        prevCourse = Convert.ToInt32(Request.QueryString["filterCourse"]);
                        lblHdnCourse.Value = Request.QueryString["filterCourse"];
                        ViewState["filterCourse"] = lblHdnCourse.Value;
                    }

                    /*  if (prevCourse == null && Request.QueryString["Key"]==null)
                      {
                          lblHdnCourse.Value  = ViewState["filterCourse"].ToString ();
                          prevCourse = Convert.ToInt32(lblHdnCourse.Value);
                      }  Commented for check*/

                    prevCourse = Convert.ToInt32(lblHdnCourse.Value);
                    var coursecategory = (from p in context.Courses
                                          where p.ID == prevCourse
                                          select new { catID = p.CourseCategoryID }).FirstOrDefault();


                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        FillCategories();
                        ddlcoursecategory.SelectedValue = coursecategory.catID.ToString();
                        ddlcoursecategory_SelectedIndexChanged(sender, e);
                        ddlcourseName.SelectedValue = prevCourse.ToString();
                        //ddlcoursecategory.SelectedValue = coursecategory.catID.ToString();
                        //ddlcoursecategory_SelectedIndexChanged(sender, e);
                        //ddlcourseName.SelectedValue = prevCourse.ToString();
                        ddlcoursecategory.Enabled = false;
                        ddlcourseName.Enabled = false;
                        FillCategoriesF();
                        //FillCertCourseType(); // added by amit new
                        //ddlcoursecategoryF.SelectedValue = coursecategory.catID.ToString();
                        //ddlcoursecategoryF_SelectedIndexChanged(sender, e);
                        //ddlCourseNameF.SelectedValue = prevCourse.ToString();
                        ddlcoursecategoryF.Enabled = false;
                        ddlcoursecategoryF.Enabled = false;
                        ShowEditMode();
                    }
                    else
                    {
                        using (NIELITMISContext context1 = new NIELITMISContext())
                        {
                            ViewState["SortField"] = "";
                            ViewState["SortOrder"] = "";
                            FillCategories();
                            ddlcoursecategory.SelectedValue = coursecategory.catID.ToString();
                            ddlcoursecategory_SelectedIndexChanged(sender, e);
                            ddlcourseName.SelectedValue = prevCourse.ToString();
                            ddlcoursecategory.Enabled = false;
                            ddlcourseName.Enabled = false;
                            FillCategoriesF();
                            ddlcoursecategoryF.SelectedValue = coursecategory.catID.ToString();
                            ddlcoursecategoryF_SelectedIndexChanged(sender, e);
                            ddlCourseNameF.SelectedValue = prevCourse.ToString();
                            ddlcoursecategoryF.Enabled = false;
                            ddlcoursecategoryF.Enabled = false;
                            // FillCertCourseType(); // added by amit new
                            //  btnMode.ViewMode = ToggleView.Mode.New;
                            //   ToggleViewMode_Changed(sender, e);
                            BindGridView();
                            /* if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                             {
                                 BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Course Level Durations List", "Admin/AdminCourseLevelDuration.aspx?Id=" + Request.QueryString["Id"].ToString() , ""));
                             }
                             else
                             {
                                 BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Course Level Durations List", "Admin/AdminCourseLevelDuration.aspx?CourseId=" + Request.QueryString["CourseId"].ToString(), ""));
                             }*/
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
            // Response.Redirect("AdminCourseLevelDuration.aspx?filterCourse=" + lblHdnCourse.Value);
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
            }
            ;
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
            }
            ;
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
                                 where p.CourseCategoryID == coursecatID
                                 && p.ShowOnWeb == true
                                 && p.IsActive == true
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourseName, CourseList, lst);
            }
            ;
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


                        ShowAlert("Record cannot be modified, Already effective To date is available");
                        //throw new Exception("Record cannot be modified, Already effective To date is available");

                        return;
                    }
                }
                else
                {
                    btnMode.ViewMode = ToggleView.Mode.List;
                    ShowAlert("Record cannot be modified, Already effective To date is available");
                    //throw new Exception("Record cannot be modified, Already effective To date is available");

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
                                  // added by amit start
                                  theoryHrs = p.theoryHrs,
                                  practicalHrs = p.practicalHrs,
                                  esHrs = p.esHrs,
                                  OJT_ProjectHrs = p.OJT_ProjectHrs,
                                  domainSkillsHrs = p.domainSkillsHrs,
                                  awardingBodyID = p.awardingBodyID,
                                  awardingBodyName = p.awardingBodyName,
                                  claasroomCount = p.claasroomCount,
                                  labCount = p.labCount,
                                  // added by amit end
                                  NCVETQualCode = p.NCVETQualCode,
                                  NIELITQualCode = p.NIELITQualCode,
                                  EffectiveFromDate = p.Effective_From_Date,
                                  EffectrivetoDate = p.Effective_To_Date
                              }).FirstOrDefault();

                // populating the fields on update
                txtcourseLevelDurationHrs.Text = course.courseLevelDurationHrs.ToString();
                txtCourseLevelNo.Text = course.courseLevelNo.ToString();
                txtcourseLevelRevisionNo.Text = course.courseLevelRevisionNo.ToString();
                txtEffectiveFromDate.Text = Convert.ToDateTime(course.EffectiveFromDate).ToString("dd-MMM-yyyy");
                txtNCVETQualCode.Text = course.NCVETQualCode;
                txtNIELITQualCode.Text = course.NIELITQualCode;

                //added by amit start
                txtthrs.Text = course.theoryHrs.HasValue ? course.theoryHrs.Value.ToString() : "";
                txtphrs.Text = course.practicalHrs.HasValue ? course.practicalHrs.Value.ToString() : "";
                txteshrs.Text = course.esHrs.HasValue ? course.esHrs.Value.ToString() : "";
                txtOJThrs.Text = course.OJT_ProjectHrs.HasValue ? course.OJT_ProjectHrs.Value.ToString() : "";
                txtdomainskill.Text = course.domainSkillsHrs.HasValue ? course.domainSkillsHrs.Value.ToString() : "";
                txtawardingbodyid.Text = string.IsNullOrEmpty(course.awardingBodyID) ? course.awardingBodyID.ToString() : "";
                txtawardname.Text = string.IsNullOrEmpty(course.awardingBodyName) ? "" : course.awardingBodyName;
                txtclasscount.Text = course.claasroomCount.HasValue ? course.claasroomCount.Value.ToString() : "";
                txtlabcnt.Text = course.labCount.HasValue ? course.labCount.Value.ToString() : "";
                //addded  by amit end


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

                //txtEffectiveFromDate.Enabled = false; testing
                if (txtNIELITQualCode.Text != "")
                    txtNIELITQualCode.Enabled = false;
                if (txtNCVETQualCode.Text != "")
                    txtNCVETQualCode.Enabled = false;

            }
            ;

            //amit for testing
            //if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            //{
            //    btnSave.Visible = false;
            //}
        }
        catch (Exception ex)
        {
            BreadCrumb1.Render();
            throw ex;
        }
    }
    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdminCourseLevelDuration.aspx?filterCourse=" + ddlcourseName.SelectedValue, true);
    }
    protected void BindGridView()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                Int32 prevCourse = Convert.ToInt32(lblHdnCourse.Value);
                int courseNameF = 0, courseCatF = 0;
                Int32 courseLevelNo = 0;


                if (ddlCourseNameF.SelectedValue != "0")
                    courseNameF = Convert.ToInt32(ddlCourseNameF.SelectedValue);
                if (ddlCourseLevelNo.SelectedValue != "0")
                    courseLevelNo = Convert.ToInt32(ddlCourseLevelNo.SelectedValue);
                if (ddlcoursecategoryF.SelectedValue != "0")
                    courseCatF = Convert.ToInt32(ddlcoursecategoryF.SelectedValue);

                //string sortOrder = ViewState["SortOrder"].ToString();
                //string sortField = ViewState["SortField"].ToString();

                var courses = (from s in context.CourseLevelDurationss
                               join c in context.Courses on s.CourseID equals c.ID
                               join w in context.CourseCategories on s.CourseCategoryID equals w.ID
                               orderby c.ID, s.Course_Level_Revision_no descending, s.ID descending
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

                                   // added by amit start
                                   theoryHrs = s.theoryHrs,
                                   practicalHrs = s.practicalHrs,
                                   esHrs = s.esHrs,
                                   OJT_ProjectHrs = s.OJT_ProjectHrs,
                                   domainSkillsHrs = s.domainSkillsHrs,
                                   awardingBodyID = s.awardingBodyID,
                                   awardingBodyName = s.awardingBodyName,
                                   claasroomCount = s.claasroomCount,
                                   labCount = s.labCount,
                                   // added by amit end

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

                courses = courses.Where(s => s.CourseId == prevCourse);

                //if (!string.IsNullOrEmpty(sortOrder))
                //{
                //    switch (sortField)
                //    {
                //        case "Name":
                //            if (sortOrder == "DESC")
                //                courses = courses.OrderByDescending(s => s.ID);
                //            else
                //                courses = courses.OrderBy(s => s.ID);
                //            break;

                //        case "Code":
                //            if (sortOrder == "DESC")
                //                courses = courses.OrderByDescending(s => s.ID);
                //            else
                //                courses = courses.OrderBy(s => s.ID);
                //            break;

                //        default:
                //            courses = courses.OrderBy(s => s.ID);
                //            break;
                //    }
                //}


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
            //if (!UserManager.HasRight(currentRoleId, enmRight.New))
            //{
            //    BreadCrumb1.Render();
            //    ShowAlert("Sorry! You don't have rights to add new record.");
            //    return;
            //}
            EConnectContext context = new EConnectContext();
            Int32 prevCourse = Convert.ToInt32(lblHdnCourse.Value);
            var coursecategory = (from p in context.Courses
                                  where p.ID == prevCourse
                                  select new { catID = p.CourseCategoryID }).FirstOrDefault();

            FillCategories();
            ddlcoursecategory.SelectedValue = coursecategory.catID.ToString();
            ddlcoursecategory_SelectedIndexChanged(sender, e);
            ddlcourseName.SelectedValue = prevCourse.ToString();


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
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminCourseLevelDuration.aspx?ID=" + Request.QueryString["ID"].ToString() + "&filterCourse=" + ddlcourseName.SelectedValue + ""), true);
            }
            else
            {
                Response.Redirect("AdminCourseLevelDuration.aspx?filterCourse=" + ddlcourseName.SelectedValue, true);
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
    // amit start
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
    // amit end
    protected void SaveRecordCourseLevelDuration(object sender, EventArgs e)
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

                Int32 courseId = 0, courseLevelDurationHrs = 0, coursecatid = 0, CourseLevelRevisionno = 0;
                DateTime effectivefromDateS, effectiveToDateS;
                Decimal courseLevelNo = 0;

                effectivefromDateS = Convert.ToDateTime(txtEffectiveFromDate.Text);

                courseId = Convert.ToInt32(ddlcourseName.SelectedValue);

                int oldCount = (from p in context.CourseLevelDurationss
                                where p.CourseID == courseId
                                orderby p.Effective_To_Date descending
                                select p
                                ).Count();


                // old record found 
                if (oldCount > 0)
                {
                    var effectiveDateOld = (from p in context.CourseLevelDurationss
                                            where p.CourseID == courseId
                                            orderby p.Effective_To_Date descending
                                            select p
                                            ).FirstOrDefault();

                    if (effectivefromDateS < effectiveDateOld.Effective_From_Date || effectivefromDateS < effectiveDateOld.Effective_To_Date)
                    {
                        ShowAlert("Effective from Date invalid,Check earlier entered dates");
                        return;
                    }
                }


                if (txtNCVETQualCode.Enabled == true)
                {
                    int NCVETQualOld = (from p in context.CourseLevelDurationss
                                        where p.CourseID == courseId &&
                                        p.NCVETQualCode == txtNCVETQualCode.Text
                                        orderby p.Effective_To_Date descending
                                        select p
                                             ).Count();
                    if (NCVETQualOld > 0)
                    {
                        ShowAlert("NCVET Qualification Code already exists");
                        return;
                    }
                }
                if (txtNIELITQualCode.Enabled == true)
                {
                    int NIELITQualOld = (from p in context.CourseLevelDurationss
                                         where p.CourseID == courseId &&
                                         p.NIELITQualCode == txtNIELITQualCode.Text
                                         orderby p.Effective_To_Date descending
                                         select p
                                           ).Count();
                    if (NIELITQualOld > 0)
                    {
                        ShowAlert("NIELIT Qualification Code already exists");
                        return;
                    }
                }
                //Comment by amit
                 if (txtEffectiveToDate.Text.Trim().Length != 0)
                {
                    effectiveToDateS = Convert.ToDateTime(txtEffectiveToDate.Text);
                    if (effectivefromDateS > effectiveToDateS)
                    {
                        throw new Exception("Effective To Date should be greater than effective From date");
                    }
                }

                coursecatid = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                courseId = Convert.ToInt32(ddlcourseName.SelectedValue);
                courseLevelDurationHrs = Convert.ToInt32(txtcourseLevelDurationHrs.Text);
                courseLevelNo = Convert.ToDecimal(txtCourseLevelNo.Text);


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


                // SAVING INFO
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
                            currentCourseLevelDuration.Course_Level_no = Convert.ToDecimal(txtCourseLevelNo.Text);// course Level No
                        }

                        // added by amit start

                        if (string.IsNullOrWhiteSpace(txtphrs.Text) ||
                            string.IsNullOrWhiteSpace(txtthrs.Text) ||
                            string.IsNullOrWhiteSpace(txteshrs.Text) ||
                            string.IsNullOrWhiteSpace(txtOJThrs.Text) ||
                            string.IsNullOrWhiteSpace(txtdomainskill.Text) ||
                            string.IsNullOrWhiteSpace(txtawardingbodyid.Text) ||
                            string.IsNullOrWhiteSpace(txtawardname.Text) ||
                            string.IsNullOrWhiteSpace(txtlabcnt.Text) ||
                            string.IsNullOrWhiteSpace(txtclasscount.Text)
                        )
                        {
                            ShowAlert("Mandatory fields cannot be left blank");
                            return;
                        }

                        currentCourseLevelDuration.practicalHrs = Convert.ToDecimal(txtphrs.Text);
                        currentCourseLevelDuration.theoryHrs = Convert.ToDecimal(txtthrs.Text);
                        currentCourseLevelDuration.esHrs = Convert.ToDecimal(txteshrs.Text);
                        currentCourseLevelDuration.OJT_ProjectHrs = Convert.ToDecimal(txtOJThrs.Text);
                        currentCourseLevelDuration.domainSkillsHrs = Convert.ToDecimal(txtdomainskill.Text);
                        currentCourseLevelDuration.awardingBodyID = Convert.ToString(txtawardingbodyid.Text);
                        currentCourseLevelDuration.awardingBodyName = Convert.ToString(txtawardname.Text);
                        currentCourseLevelDuration.labCount = Convert.ToInt32(txtlabcnt.Text);
                        currentCourseLevelDuration.claasroomCount = Convert.ToInt32(txtclasscount.Text);

                        // added by amit end

                        currentCourseLevelDuration.Course_Level_Revision_no = Convert.ToInt32(txtcourseLevelRevisionNo.Text);
                        currentCourseLevelDuration.Effective_From_Date = Convert.ToDateTime(txtEffectiveFromDate.Text);


                        if (txtEffectiveToDate.Text != "")
                        {
                            currentCourseLevelDuration.Effective_To_Date = Convert.ToDateTime(txtEffectiveToDate.Text);
                        }

                        if (txtNCVETQualCode.Text != "")
                            currentCourseLevelDuration.NCVETQualCode = txtNCVETQualCode.Text;

                        if (txtNIELITQualCode.Text != "")
                            currentCourseLevelDuration.NIELITQualCode = txtNIELITQualCode.Text;

                        currentCourseLevelDuration.enterDate = DateTime.Now; // entered course date by which
                        currentCourseLevelDuration.enterBy = Convert.ToInt32(Session["UserID"]);

                        context.CourseLevelDurationss.Add(currentCourseLevelDuration);//save
                        context.SaveChanges();
                        strMessage = "New Record Saved";
                    }
                    else
                    {
                        throw new Exception("Already existing revision or effective dates, not saved");
                        return;
                        strMessage ="ALready existing revision or effective dates, not saved";
                    }
                }
                else
                {
                    
                    if (txtEffectiveToDate.Text.Trim().Length == 0)
                    {
                        throw new Exception("Please enter Effective to date");
                        return;
                    }
                    
                     effectiveToDateS = Convert.ToDateTime(txtEffectiveToDate.Text);
                    
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

                        if (txtNCVETQualCode.Text != "")
                            currentCourseLevelDuration.NCVETQualCode = txtNCVETQualCode.Text;

                        if (txtNIELITQualCode.Text != "")
                            currentCourseLevelDuration.NIELITQualCode = txtNIELITQualCode.Text;

                        //added by amit start addition for new blocks


                        currentCourseLevelDuration.enterDate = DateTime.Now; // entered course date by which
                        currentCourseLevelDuration.enterBy = Convert.ToInt32(Session["UserID"]);
                        context.SaveChanges();
                        scope.Complete();
                        strMessage = "Revision changes Saved";
                    }
                }
            }
            Response.Redirect("AdminCourseLevelDuration.aspx?msg=" + strMessage + "&filterCourse=" + ddlcourseName.SelectedValue, true);
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


    //protected void PerformPopupAction(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
    //        {
    //            BindGridView();
    //            uPnlGrid.Update();
    //            BreadCrumb1.Render();
    //            ShowAlert("Sorry! You don't have rights to delete the records.", true);
    //            return;
    //        }
    //        using (TransactionScope scope = new TransactionScope())
    //        {
    //            using (NIELITMISContext context = new NIELITMISContext())
    //            {
    //                Int32 courseid = Convert.ToInt32(hfActionID.Value);
    //                EConnect.NIELIT.NielitCourseDuration course = context.NielitCourseDurations.Find(courseid);

    //                context.NielitCourseDurations.Remove(course);
    //                context.SaveChanges();
    //                scope.Complete();
    //                ShowAlert("Record deleted successfully.", true);
    //                hfActionID.Value = "";
    //            }
    //            ;
    //        }
    //        BindGridView();
    //        uPnlGrid.Update();
    //    }
    //    catch (Exception ex)
    //    {
    //        BindGridView();
    //        uPnlGrid.Update();
    //        ShowAlert("Record can not be deleted!", true);
    //    }
    //}
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
                    href += "&Id=" + Request.QueryString["Id"].ToString() + "&filterCourse=" + Request.QueryString["filterCourse"];
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

        Response.Redirect("AdminCourseLevelDuration.aspx?filterCourse=" + ddlcourseName.SelectedValue, true);
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
            }
            ;
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

            }
            ;
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

    //Added_For_Course_Type_of_Certificate_Bound_28_10_2024_Start
    //protected void FillCertCourseType()
    //{
    //    try
    //    {
    //        //using (EConnectContext context = new EConnectContext())
    //        //{
    //        //    ListItem lst = new ListItem("--Select One--", "0");
    //        //    var certCourseTypesList = from p in context.CertificateCourseTypes
    //        //                     orderby (p.certCourseType)
    //        //                     select new { ValueField = p.ID, TextField = p.certCourseTypeDesc };


    //        //    certCourseTypesList = certCourseTypesList.Distinct();
    //        //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCertCourseType, certCourseTypesList, lst);
    //        //};

    //        using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
    //        {
    //            DataTable dt = new DataTable();
    //            ListItem lst = new ListItem("--Select One--", "0");
    //            using (SqlCommand sqlCommand = new SqlCommand("select certCourseType,certCourseTypeDesc+'('+certCourseType+')' as CourseType from certCourseType", sqlConnection))
    //            {
    //                //sqlCommand.CommandType = CommandType.StoredProcedure;
    //                sqlConnection.Open();
    //                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
    //                {
    //                    sqlDataAdapter.Fill(dt);

    //                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCertCourseType, dt, lst);
    //                    //ddlCertCourseType.DataSource = dt;
    //                    //ddlCertCourseType.DataValueField = "ID";
    //                    //ddlCertCourseType.DataTextField = "certCourseTypeDesc";
    //                    //ddlCertCourseType.DataBind();
    //                    //ddlCertCourseType.Enabled = true;
    //                }
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //Added_For_Course_Type_of_Certificate_Bound_28_10_2024_End

}