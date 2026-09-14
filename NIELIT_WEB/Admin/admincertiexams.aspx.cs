using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
public partial class admcertiexams : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    FillCourseCategory();
                    FillCourseType();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";

                    FillCourseCategory();
                    FillCourseType();
                    FillFilter();
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Certification Exams", "Admin/admincertiexams.aspx", ""));
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
    protected void FillFilter()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.CourseCategories
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourses, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
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
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillCourseType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.CourseTypes
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursetype, CourseList, lst);
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
            btnSave.Text = "Update";
            lblHeading.Text = "Certification Exam";
            tblNavLinks.Visible = true;
            ddlEntity.SelectedIndex = 4;
           using (EConnectContext context = new EConnectContext())
            {
                Int32 courseId = Convert.ToInt32(Request.QueryString["Key"]);
                var course = (from p in context.Courses
                              join c in context.CourseRevisions
                               on p.ID equals c.CourseID
                              where p.ID == courseId
                              orderby c.EffectiveFromDate descending
                              select new
                              {
                                  ID = p.ID,
                                  Name = p.Name,
                                  Code = p.Code,
                                  CourseCategoryID = p.CourseCategoryID,
                                  CourseTypeID = p.CourseTypeID,
                                  CurrentRevisionNumber = c.RevisionNumber,
                                  EffectiveFromDate = c.EffectiveFromDate
                              }).FirstOrDefault();
                txtcoursename.Text = course.Name;
                txtcoursecode.Text = course.Code;
                ddlcoursecategory.SelectedValue = course.CourseCategoryID.ToString();
                ddlcoursecategory.Enabled = false;
                ddlcoursetype.SelectedValue = course.CourseTypeID.ToString();
                ddlcoursetype.Enabled = false;
                txteffectivedate.Text = course.EffectiveFromDate.ToString("dd-MMM-yyyy");
                hfAccID.Value = Request.QueryString["Key"];
                hfName.Value = Request.QueryString["Name"];
                h1exam.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("ExamCentres.aspx?CourseId=" + Request.QueryString["Key"] );
                h2Exams.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("admcertiexamdetail.aspx?CourseId=" + hfAccID.Value +"&CatID=" + course.CourseCategoryID);
                //Updating breadscrumb

                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(course.Name, "Admin/admincertiexams.aspx?Key=" + (Request.QueryString["key"]), ""));

                

                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                //Create an object of record to be modified and assign properties to relevant fields.
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            // context.Dispose();
        }
    }
    protected void BindGridView()
    {
        try
        {
            Int32 courseType = Convert.ToInt32(enmCourseType.CertificationExam);
            context = new EConnectContext();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            int courseCategory = 0;
            if (ddlCourses.SelectedValue != "0")
                courseCategory = Convert.ToInt32(ddlCourses.SelectedValue);
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var courses = from s in context.Courses
                          where s.CourseTypeID == courseType
                          select new { ID = s.ID, Name = s.Name, CategoryName = s.CourseCategory.Name, CourseCategoryID = s.CourseCategoryID };
            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            if (courseCategory != 0)
                courses = courses.Where(s => s.CourseCategoryID == courseCategory);
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "Name":
                        if (sortOrder == "DESC")
                            courses = courses.OrderByDescending(s => s.Name);
                        else
                            courses = courses.OrderBy(s => s.Name);
                        break;
                    case "CategoryName":
                        if (sortOrder == "DESC")
                            courses = courses.OrderByDescending(s => s.CategoryName);
                        else
                            courses = courses.OrderBy(s => s.CategoryName);
                        break;

                    default:
                        courses = courses.OrderBy(s => s.Name);
                        break;
                }
            }
            PagingBar1.Bind(courses, ref gvMain);
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
            FillCourseCategory();
            FillCourseType();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Certification Exams";
            //Updating Breadcrumb

            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Course", "", ""));
           
       
          
         }

        else
        {
            Response.Redirect("admincertiexams.aspx", true);
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
        //try
        //{
        //    context = new EConnectContext();
        //    //create and object 
        //    User objUser;
        //    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
        //    {

        //        strMessage = "New record saved.";
        //    }
        //    else
        //    {
        //        ////Initialize current object by loading it and get its current modified date
        //        strMessage = "Record updated.";
        //    }

        //    //Call save method
        //    //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
        //    //Redirect it to list mode
        //    Response.Redirect("CertificateCourse.aspx?msg=" + strMessage);
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
        //finally { context.Dispose(); }

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
            ddlCourses.SelectedValue = "0";
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
        //try
        //{
        //    context = new EConnectContext();
        //    if (hfActionID.Value != "")
        //    {
        //        String recordID = hfActionID.Value.Split('$')[0].ToString();
        //        LinkButton btnAction = (LinkButton)sender;
        //        if (btnAction.CommandName == "Delete")
        //        {
        //            //Load the object and apply validateion if required
        //            //call delete function
        //            //bind the grid again
        //            BindGridView();
        //            ShowAlert("Record deleted successfully.", true);
        //            hfActionID.Value = "";
        //        }
        //        else if (btnAction.CommandName == "Action")
        //        {
        //            //Load the object and apply validateion if required
        //            //call function to perform required action
        //            //bind the grid again
        //            BindGridView();
        //            ShowAlert("Record Action1 successfully.", true);
        //            hfActionID.Value = "";
        //        }
        //        uPnlGrid.Update();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    hfActionID.Value = "";
        //    ShowAlert(ex.Message, true);
        //}
        //finally { context.Dispose(); }
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
                hl1.NavigateUrl = hl.NavigateUrl;

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                //Image imgAction = (Image)e.Row.FindControl("imgAction");
                //imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();

                //CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                //imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
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
            Int32 courseType = Convert.ToInt32(enmCourseType.CertificationExam);
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var courses = from s in context.Courses
                          where s.CourseTypeID == courseType
                          select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            courses = courses.OrderBy(s => s.Name);
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("admincertiexams.aspx", true);
    }
}