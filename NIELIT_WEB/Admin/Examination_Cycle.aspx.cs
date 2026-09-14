using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Common;
using EConnect.NIELIT;
public partial class Examination_Cycle : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    FillCourseCategory();                
                    EnumUtility.BindListObject(ref ddlMonthCycle, typeof(EConnect.NIELIT.enmExamSchedule), new ListItem("--Select One--", "0"));
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillCourseCategory();
                    FillCategoryCourse();
                    EnumUtility.BindListObject(ref ddlMonthCycle, typeof(EConnect.NIELIT.enmExamSchedule), new ListItem("--Select One--", "0"));
                    EnumUtility.BindListObject(ref ddlExamSchedule, typeof(EConnect.NIELIT.enmExamSchedule), new ListItem("--All--", "0"));
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Examination Cycles", "Admin/Examination_Cycle.aspx", ""));
                 
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void FillCourseCategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.CourseCategories
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategory, CourseList.Distinct(), lst);


            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillCategoryCourse()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                var CourseList = from p in context.CourseCategories
                                 orderby (p.DisplayOrder)
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCategory, CourseList.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillCourseName(int catID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (catID != 0)
                {
                    var CourseList = from p in context.Courses
                                     where p.CourseCategoryID == catID
                                     orderby (p.DisplayOrder)
                                     select new { ValueField = p.ID, TextField = p.Name };

                    if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                    {
                        var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                        CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                    }
                    CourseList = CourseList.Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, CourseList, lst);
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillCourseNames(int catID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                if (catID != 0)
                {
                    var CourseName = from p in context.Courses
                                     where p.CourseCategoryID == catID
                                     orderby (p.DisplayOrder)
                                     select new { ValueField = p.ID, TextField = p.Name };

                    if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                    {
                        var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                        CourseName = CourseName.Where(a => roleCourses.Contains(a.ValueField));
                    }
                    CourseName = CourseName.Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, CourseName, lst);
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
            context = new EConnectContext();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Examination Cycle";
            tblNavLinks.Visible = true;
            ExaminationCycle objExam = context.ExaminationCycles.Find(Convert.ToInt32(Request.QueryString["key"]));

            ddlCourseCategory.SelectedValue = objExam.CourseCategoryID.ToString();
            ddlCourseCategory_SelectedIndexChanged(ddlCourseCategory, EventArgs.Empty);
            ddlCourseName.SelectedValue = objExam.CourseID.ToString();
            txtExamName.Text = objExam.Name.ToString();
            ddlMonthCycle.SelectedValue = objExam.ExamScheduleID.ToString();
            Int32 CycleID = Convert.ToInt32(objExam.ID.ToString());
            Int32 CatID = Convert.ToInt32(objExam.CourseCategoryID);
            Int32 CourseID = Convert.ToInt32(objExam.CourseID);
            ddlStartMonth.SelectedValue = objExam.StartingMonth.ToString();
            //ddlOccurance.SelectedValue = objExam.Occurance.ToString();
            //ddlWeek.SelectedValue = objExam.WeekNumber.ToString();
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(objExam.Name, "Admin/Examination_Cycle.aspx?Key=" + (Request.QueryString["key"]), ""));

            h2Exams.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("admcertiexamdetail.aspx?CycleID=" + CycleID + "&CatID=" + CatID + "&CourseID=" + CourseID );
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.
            if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            {
                btnSave.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            context.Dispose();
        }
    }
    protected void BindGridView()
    {
        try
        {
                //this is the sample code how to bind the grid control
                context = new EConnectContext();
                int CourseCategory = 0;
                int CourseName = 0;
                int ExamSch = 0;
                if (ddlCategory.SelectedValue != "0")
                    CourseCategory = Convert.ToInt32(ddlCategory.SelectedValue);
                if (ddlCourse.SelectedValue != "0")
                    CourseName = Convert.ToInt32(ddlCourse.SelectedValue);
                if (ddlExamSchedule.SelectedValue != "0")
                    ExamSch = Convert.ToInt32(ddlExamSchedule.SelectedValue);
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                var examCycle = from s in context.ExaminationCycles
                                select new
                                {
                                    ID = s.ID,
                                    name = s.Name,
                                    CourseCategoryID=s.CourseCategoryID,
                                    CategoryName = s.CourseCategory.Name,
                                    CourseID=s.CourseID,
                                    CourseName = (!string.IsNullOrEmpty(s.CourseCategory.Code) ? s.CourseCategory.Code : "All") + "-" + (!string.IsNullOrEmpty(s.Course.Code) ? s.Course.Code : "All"),
                                    MonthCycle = s.ExamScheduleID

                                };
                if (!String.IsNullOrEmpty(searchString))
                {
                    examCycle = examCycle.Where(s => s.name.ToUpper().Contains(searchString));
                }
                if (CourseCategory != 0)
                    examCycle = examCycle.Where(s =>s.CourseCategoryID == CourseCategory);
                if (CourseName != 0)
                    examCycle = examCycle.Where(s => s.CourseID == CourseName);
                if (ExamSch != 0)
                    examCycle = examCycle.Where(s => s.MonthCycle == ExamSch);
                examCycle = examCycle.OrderBy(s => s.CategoryName);
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "CategoryName":
                            if (sortOrder == "DESC")
                                examCycle = examCycle.OrderByDescending(s => s.CategoryName);
                            else
                                examCycle = examCycle.OrderBy(s => s.CategoryName);
                            break;
                        case "CourseName":
                            if (sortOrder == "DESC")
                                examCycle = examCycle.OrderByDescending(s => s.CourseName);
                            else
                                examCycle = examCycle.OrderBy(s => s.CourseName);
                            break;
                        case "name":
                            if (sortOrder == "DESC")
                                examCycle = examCycle.OrderByDescending(s => s.name);
                            else
                                examCycle = examCycle.OrderBy(s => s.name);
                            break;
                        case "MonthCycle":
                            if (sortOrder == "DESC")
                                examCycle = examCycle.OrderByDescending(s => s.MonthCycle);
                            else
                                examCycle = examCycle.OrderBy(s => s.MonthCycle);
                            break;
                        default:
                            examCycle = examCycle.OrderBy(s => s.CategoryName);
                            break;
                    }
                }
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    examCycle = examCycle.Where(a => roleCourses.Contains(a.CourseCategoryID));
                }

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    examCycle = examCycle.Where(a => roleCourses.Contains(a.CourseID));
                }
                PagingBar1.Bind(examCycle, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            context.Dispose();
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
                ShowAlert("Sorry! You don't have rights to add new record.",true);
                return;
            }
            FillCourseCategory();
            //FillCourseName();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New Examination Cycle";
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Examination Cycle", "", ""));
         
        }
        else
        {
            Response.Redirect("Examination_Cycle.aspx", true);
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
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
    protected void SearchBar_Reset(object sender, EventArgs e)
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
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            context = new EConnectContext();
            //create and object 
            ExaminationCycle objExam;
            if (String.IsNullOrEmpty(Request.QueryString["key"]))
            {
                objExam = new EConnect.NIELIT.ExaminationCycle();
                objExam.CourseCategoryID = Convert.ToInt32(ddlCourseCategory.SelectedValue);
                objExam.CourseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                objExam.Name = txtExamName.Text;
                objExam.ExamScheduleID = Convert.ToInt32(ddlMonthCycle.SelectedValue);
                objExam.StartingMonth= Convert.ToInt32(ddlStartMonth.SelectedValue);
                context.ExaminationCycles.Add(objExam);
                context.SaveChanges();
                strMessage = "New record saved.";
            }
            else
            {
                ////Initialize current object by loading it and get its current modified date
                objExam = context.ExaminationCycles.Find(Convert.ToInt32(Request.QueryString["key"]));
                objExam.CourseCategoryID = Convert.ToInt32(ddlCourseCategory.SelectedValue);
                objExam.CourseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                objExam.Name = txtExamName.Text;
                objExam.ExamScheduleID = Convert.ToInt32(ddlMonthCycle.SelectedValue);
                objExam.StartingMonth = Convert.ToInt32(ddlStartMonth.SelectedValue);
                if (context.ExaminationCycles.Any(c => c.Name.ToUpper() == objExam.Name.ToUpper() && c.CourseCategoryID == objExam.CourseCategoryID &&
                    c.CourseID == objExam.CourseID && c.ExamScheduleID == objExam.ExamScheduleID && c.ID != objExam.ID))
                {
                    throw new Exception("Duplicate record not allowed");
                }
                context.SaveChanges();
                strMessage = "Record updated.";
            }

            //Call save method
            //Redirect it to list mode
            Response.Redirect("Examination_Cycle.aspx?msg=" + strMessage);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }

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
            ddlCategory.SelectedValue = "0";
            ddlCourse.SelectedValue = "0";
            ddlExamSchedule.SelectedValue = "0";
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
            using (EConnectContext context = new EConnectContext())
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to delete the records.", true);
                    return;
                }
                ExaminationCycle examcycle = context.ExaminationCycles.Find(Convert.ToInt32(hfActionID.Value));
                context.ExaminationCycles.Remove(examcycle);
                context.SaveChanges();
                BindGridView();
                ShowAlert("Record deleted successfully.", true);
                hfActionID.Value = "";
            };
            uPnlGrid.Update();
        }
        catch (Exception)
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
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                Image imgAction = (Image)e.Row.FindControl("imgAction");
                imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();

                CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
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
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var exmCycle = from s in context.ExaminationCycles
                        select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                exmCycle = exmCycle.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            exmCycle = exmCycle.OrderBy(s => s.Name);
            foreach (var cycles in exmCycle)
            {
                items.Add(cycles.Name);
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
        Response.Redirect("Examination_Cycle.aspx", true);
    }
    protected void ddlCourseCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id2 = Convert.ToInt32(ddlCourseCategory.SelectedValue);
        ddlCourseName.Items.Clear();
        FillCourseName(id2);
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id2 = Convert.ToInt32(ddlCategory.SelectedValue);
        ddlCourseName.Items.Clear();
        FillCourseNames(id2);
    }
}