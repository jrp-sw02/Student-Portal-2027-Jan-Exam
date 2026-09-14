using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
public partial class PatternOfTimeTable : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;
    IEnumerable<Object> modules;
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View,"Admin/CertificateCourse.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    fillCourseRevNo();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Time Table Pattern", "Admin/TimeTablePattern.aspx", ""));
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void fillCourseRevNo()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
                Course course = context.Courses.Find(couID);
                ListItem lst = new ListItem("--Select One--", "-1");
                var query = (from p in context.CourseRevisions
                            where (p.CourseCategoryID== course.CourseCategoryID && p.CourseID == couID)
                            orderby (p.RevisionNumber)
                            select new { ValueField = p.RevisionNumber, TextField = p.RevisionNumber}).Distinct();

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseRevNo, query, lst);
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
            lblHeading.Text = "Form Header Detail";
            tblNavLinks.Visible = true;
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.
             
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
            Int32 CourseID = 0;
            Int32 couCatID = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
            }
            Course course = context.Courses.Find(CourseID);
            couCatID = course.CourseCategoryID;
            int[] moduleTypes = { Convert.ToInt32(enmModuleType.Theory), Convert.ToInt32(enmModuleType.Bridge) };
            Int32 revNo = Convert.ToInt32(ddlCourseRevNo.SelectedValue);
            var query = from s in context.Modules
                        where (s.CourseCategoryID == couCatID && s.CourseID == CourseID && 
                               s.RevisionNumber == revNo && moduleTypes.Contains(s.ModuleTypeID))
                        orderby s.Code
                        select new
                        {
                            ID=s.ID,
                            ModuleName=s.ShortName + "-" + s.Name
                        };
            gvMain.Columns[1].HeaderText = "Module Name:Revision " + ddlCourseRevNo.SelectedValue.ToString();
              
                if (query.Count() != 0)
                {
                    PagingBar1.Bind(query, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                }
                else
                {
                    lberror.Visible = true;
                    lberror.Text = "No Record Found";
                    btnReset.Visible = false;
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
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New User";
        }
        else
        {
            Response.Redirect("ModuleParity.aspx", true);
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
            context = new EConnectContext();
            //create and object 
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
               
                strMessage = "New record saved.";
            }
            else
            {
                ////Initialize current object by loading it and get its current modified date
                strMessage = "Record updated.";
            }

            //Call save method
            //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
            //Redirect it to list mode
            Response.Redirect("ModuleParity.aspx?msg=" + strMessage);
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
            //ddlSearchUserType.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
                    Int32 moduleID = Convert.ToInt32(gvMain.DataKeys[e.Row.RowIndex].Values[0]);
                    Int32 CourseRevNo = Convert.ToInt32(ddlCourseRevNo.SelectedValue);

                    DropDownList ddlExamSession = (DropDownList)e.Row.FindControl("ddlExamSession");
                    TextBox txtExmDay = (TextBox)e.Row.FindControl("txtExamDay");
                    var fillExamSession = (from p in context.CourseExamSessions
                                           where p.CourseID == couID
                                           select new { ValueField = p.ExamSession.ID, TextField = p.ExamSession.Name });

                    ListItem lst = new ListItem("--Select One--", "0");
                    if (fillExamSession.Count() > 0)
                    {
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamSession, fillExamSession, lst);
                    }
                    else
                    {
                        var fillExamSessionNew = (from p in context.ExamSessions
                                               select new { ValueField = p.ID, TextField = p.Name });

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamSession, fillExamSessionNew, lst);
                    }

                    var query = (from s in context.TimeTablePatterns
                                 where (s.CourseID == couID && s.RevisionNumber == CourseRevNo && s.ModuleID == moduleID)
                                 select new { ExamSessionID = s.ExamSessionID,ExamDay=s.ExamDay }).FirstOrDefault();
                    if (query != null)
                    {
                        ddlExamSession.SelectedValue = query.ExamSessionID.ToString();
                        txtExmDay.Text = query.ExamDay.ToString();
                        btnSave.Text = "Update";
                    }
                };
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
            var users = from s in context.Users
                        select new { Name = s.UserName };
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            users = users.OrderBy(s => s.Name);

            var users1 = from s in context.Users
                         select new { Name = s.LoginID };
            if (!String.IsNullOrEmpty(searchString))
            {
                users1 = users1.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            users = users.Union(users1).Take(count);
            foreach (var user in users)
            {
                items.Add(user.Name);
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
       
       if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
       {
           Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("TimeTablePattern.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() ), true);
       }
       else
       {
           Response.Redirect("TimeTablePattern.aspx", true);
       }
    }
  
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            btnReset.Visible = true;
            if (ddlCourseRevNo.SelectedValue == "-1")
            {
                throw new Exception("Please select Course Revision No.");
            }
            using (EConnectContext context = new EConnectContext())
            {
                Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
                Course course = context.Courses.Find(couID);
                var query = (from p in context.CourseRevisions
                             where (p.CourseCategoryID == course.CourseCategoryID && p.CourseID == couID)
                             orderby p.RevisionNumber descending 
                             select new { revNo = p.RevisionNumber}).Take(1);
                if (query.Count() > 0)
                {
                    Int32 currentRevNumber = Convert.ToInt32(query.FirstOrDefault().revNo.ToString());
                    if (currentRevNumber != Convert.ToInt32(ddlCourseRevNo.SelectedValue))
                    {
                        btnSave.Visible = false;
                    }
                    else
                    {
                        btnSave.Visible = true;
                    }
                }
            };
            divGrid.Visible = true;
            divNavigation.Visible = true;
           
            PagingBar1.CurrentPageSize = 0; 
            BindGridView();

            PagingBar1.Visible = true;
            divSave.Visible = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlCourseRevNo.SelectedValue = "-1";
           
            divGrid.Visible = false;
            divNavigation.Visible = false;
            divSave.Visible = false;
            btnReset.Visible = false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
  
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            context = new EConnectContext();
            Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
            Int32 CourseRevNo = Convert.ToInt32(ddlCourseRevNo.SelectedValue);
            foreach (GridViewRow row in gvMain.Rows)
            {
                Int32 examSession = Convert.ToInt32(((DropDownList)row.FindControl("ddlExamSession")).SelectedValue);
                String txtexmDay = Convert.ToString(((TextBox)row.FindControl("txtExamDay")).Text);
                if (examSession == 0)
                {
                    throw new Exception("Please Select Exam Session");
                }
                if (!IsNumeric(txtexmDay))
                {
                    throw new Exception("Exam Day must be in digits");
                }
            }
            context.Database.ExecuteSqlCommand("delete from Time_Table_Pattern where Course_ID='" + couID.ToString() + "'" +
                                               " and Revision_Number='" + CourseRevNo.ToString() + "'");
            foreach (GridViewRow row in gvMain.Rows)
            {
                TimeTablePattern objTimeTblPattern = new TimeTablePattern();
                   
                objTimeTblPattern.CourseID = couID;
                objTimeTblPattern.RevisionNumber = CourseRevNo;
                Int32 moduleID = Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Values[0]);
                objTimeTblPattern.ModuleID = moduleID;
                Int32 examSessionID = Convert.ToInt32(((DropDownList)row.FindControl("ddlExamSession")).SelectedValue);
                objTimeTblPattern.ExamSessionID = examSessionID;
                Int32 examDay = Convert.ToInt32(((TextBox)row.FindControl("txtExamDay")).Text);
                objTimeTblPattern.ExamDay = examDay;
                objTimeTblPattern.CreatedByID = Convert.ToInt32(Session["UserID"]);
                objTimeTblPattern.CreatedOn = DateTime.Now;
               
                if (examSessionID != 0)
                {
                    context.TimeTablePatterns.Add(objTimeTblPattern);
                }
             }
             context.SaveChanges();
             if(btnSave.Text == "Save")
                strMessage = "Record Saved";
             else
                 strMessage = "Record Updated";
             Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("TimeTablePattern.aspx?" + Request.QueryString.ToString() + "&msg=" + strMessage),true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally
        {
            context.Dispose();
        }
    }
}