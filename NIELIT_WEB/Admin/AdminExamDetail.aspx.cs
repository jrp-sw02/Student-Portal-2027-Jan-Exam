using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
public partial class Admin_AdminExamDetail : BasePage
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
                    Filltype();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    Bindcourses();
                    BindGridView();
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
    protected void Filltype()
    {

        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //Populating Courses
                ListItem lst = new ListItem("--Select One--", "0");
                var app = from p in context.ApplicantTypes
                          orderby (p.Name)
                          select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlapptype, app, lst);
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
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Detail:Update", "", ""));
            BreadCrumb1.Render();
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            string couresname = Request.QueryString["Exam"];
            var student = (from c in context.Candidates//need to change query
                           join rg in context.RegistrationDetails
                           on c.ID equals rg.CandidateID
                           where c.ID == Appid && rg.Course.Name == couresname
                           select new
                           {
                               Course = rg.Course.Name.ToUpper(),
                               regdate = rg.RegistrationDate,
                               validdate = rg.ValidUptoDate,
                               status = (rg.RegistrationStatusID.HasValue ? rg.RegistrationStatus.Name : "NA"),
                               CandtypeID = (rg.ApplicantTypeID != 0 ? rg.ApplicantTypeID : 0)
                           }).FirstOrDefault();
            btnMode.Visible = true;
            btnSave.Text = "Update";
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            lblHeading.Text = "Exam Details";
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            txtcname.Text = student.Course;
            txtregdate.Text = student.regdate.ToString("dd-MMM-yyyy");
            txtvaliddate.Text = student.validdate.ToString("dd-MMM-yyyy");
            Txtstatus.Text = student.status;
            ddlapptype.SelectedValue = student.CandtypeID.ToString();

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
    protected void Bindcourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //Populating Courses
                ListItem lst = new ListItem("--Select One--", "0");
                var Course = from p in context.Courses
                             where p.CourseCategoryID == 1
                             orderby (p.DisplayOrder)
                             select new { ValueField = p.ID, TextField = p.Name.ToUpper() };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcname, Course, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindGridView()
    {
        try
        {
            context = new EConnectContext();
            int courseid = 0;
            if (ddlcname.SelectedValue != "0")
                courseid = Convert.ToInt32(ddlcname.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            var student = from c in context.Candidates // need to change query
                          join rg in context.RegistrationDetails
                          on c.ID equals rg.CandidateID
                          where c.ID == Appid
                          orderby rg.Course.DisplayOrder
                          select new
                          {
                              ID = c.ID,
                              Exam = rg.Course.Name,
                              doexam=rg.RegistrationDate,
                              modattempted=rg.RegistrationDate,
                              modcleared=rg.RegistrationDate,
                              result=rg.RegistrationDate,
                              appid = Appid,
                              CourseID = rg.CourseID
                          };
            if (courseid != 0)
                student = student.Where(p => p.CourseID == courseid);
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "ID":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.ID);
                        else
                            student = student.OrderBy(s => s.ID);
                        break;
                    case "Exam":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.Exam);
                        else
                            student = student.OrderBy(s => s.Exam);
                        break;
                    case "doexam":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.doexam);
                        else
                            student = student.OrderBy(s => s.doexam);
                        break;
                    case "modattempted":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.modattempted);
                        else
                            student = student.OrderBy(s => s.modattempted);
                        break;
                    case "modcleared":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.modcleared);
                        else
                            student = student.OrderBy(s => s.modcleared);
                        break;
                     case "result":
                        if (sortOrder == "DESC")
                            student = student.OrderByDescending(s => s.result);
                        else
                            student = student.OrderBy(s => s.result);
                        break;
                    default:
                        student = student.OrderBy(s => s.ID);
                        break;
                }
            }
            PagingBar1.Bind(student, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
            if (!String.IsNullOrEmpty(Request.QueryString["msg"]))
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Details", "Admin/AdminExamDetail.aspx?key1=" + Request.QueryString["key1"], ""));
            }
            else
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Details", "Admin/AdminExamDetail.aspx?" + Request.QueryString.ToString(), ""));
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
            //lblHeading.Text = "New Course";

        }
        else
        {
            if (!string.IsNullOrEmpty(Request.QueryString["key1"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminExamDetail.aspx?key1=" + Request.QueryString["key1"]));
            }
            else
            {
                Response.Redirect("AdminExamDetail.aspx", true);
            }
        }

    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        //try
        //{
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }
    protected void SearchBar_Reset(object sender, EventArgs e)
    {
        //try
        //{
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            context = new EConnectContext();
            Int32 Appid = Convert.ToInt32(Request.QueryString["key1"]);
            string couresname = Request.QueryString["Exam"];
            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                var student = (from c in context.Candidates
                               join rg in context.RegistrationDetails
                               on c.ID equals rg.CandidateID
                               where c.ID == Appid && rg.Course.Name == couresname
                               orderby rg.RegistrationStatus.Name ascending
                               select rg).FirstOrDefault();

                student.Course.Name = txtcname.Text;
                student.RegistrationDate = Convert.ToDateTime(txtregdate.Text);
                student.ValidUptoDate = Convert.ToDateTime(txtvaliddate.Text);
                student.RegistrationStatus.Name = Txtstatus.Text;
                student.ApplicantTypeID = Convert.ToInt32(ddlapptype.SelectedValue);
                context.Entry(student).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                strMessage = "Record updated.";
            }
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminExamDetail.aspx?key1=" + Request.QueryString["key1"]+"&msg="+strMessage));
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
            ddlcname.SelectedValue = "0";
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
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl);
                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);
                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);
                HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

            }
        }
        catch (Exception ex)
        {
            throw ex;
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
    [System.Web.Services.WebMethod(EnableSession = true)]
    public void GetSearchText(String prefixText, Int32 count)
    {
        //EConnectContext context = new EConnectContext();
        //try
        //{
        //    if (count <= 0)
        //        count = 10;
        //    List<String> items = new List<String>();
        //    string searchString = prefixText.Trim().ToUpper();
        //    var users = from s in context.Users
        //                select new { Name = s.UserName };
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        users = users.Where(s => s.Name.ToUpper().Contains(searchString));
        //    }
        //    users = users.OrderBy(s => s.Name);

        //    var users1 = from s in context.Users
        //                 select new { Name = s.LoginID };
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        users1 = users1.Where(s => s.Name.ToUpper().Contains(searchString));
        //    }
        //    users = users.Union(users1).Take(count);
        //    foreach (var user in users)
        //    {
        //        items.Add(user.Name);
        //    }
        //    return items.ToArray();
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
        //finally { context.Dispose(); }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BreadCrumb1.Render();
        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("AdminExamDetail.aspx?key1=" + Request.QueryString["key1"]));
    }
}