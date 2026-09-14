using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class Admin_RegStudSummary : BasePage
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

                ViewState["SortField"] = "";
                ViewState["SortOrder"] = "";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registered Candidates", "Admin/RegStudSummary.aspx", ""));
                BindGridView();
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                ShowAlert(Request.QueryString["msg"].ToString());
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindGridView()
    {
        try
        {
            context = new EConnectContext();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            Int32 Pursuing = Convert.ToInt32(enmRegistrationStatus.Registered);
            Int32 Completed = Convert.ToInt32(enmRegistrationStatus.Completed);
            Int32 Expired = Convert.ToInt32(enmRegistrationStatus.Expired);
            Int32 Cancelled = Convert.ToInt32(enmRegistrationStatus.Cancelled);
            Int32 Reregistered = Convert.ToInt32(enmRegistrationStatus.ReRegistered);
            Int32 Practpending = Convert.ToInt32(enmRegistrationStatus.ProjectPending);
            var registration = (from c in context.RegistrationDetails.AsNoTracking()
                                    group c by new { c.CourseID, c.Course.Name,c.CourseCategory.Code} into s
                                    orderby s.Key.CourseID
                                    select new
                                    {
                                        ID = s.Key.CourseID,
                                        courseCategory = s.FirstOrDefault().CourseCategory.Name,
                                        Name = ((!string.IsNullOrEmpty(s.Key.Code)) ? s.Key.Code : "All") + "-" + ((!string.IsNullOrEmpty(s.Key.Name)) ? s.Key.Name : "All"),
                                        PPursuing = s.Count(q => q.RegistrationStatusID == Pursuing),
                                        PCompleted = s.Count(q => q.RegistrationStatusID == Completed),
                                        PExpired = s.Count(q => q.RegistrationStatusID == Expired),
                                        PCancelled = s.Count(q=>q.RegistrationStatusID ==Cancelled),
                                        PreRegistered = s.Count(q=>q.RegistrationStatusID==Reregistered),
                                        Ppractpending = s.Count(q=>q.RegistrationStatusID==Practpending)
                                    });

         
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ID":
                            if (sortOrder == "DESC")
                                registration = registration.OrderByDescending(s => s.ID);
                            else
                                registration = registration.OrderBy(s => s.ID);
                            break;
                        case "courseCategory":
                            if (sortOrder == "DESC")
                                registration = registration.OrderByDescending(s => s.courseCategory);
                            else
                                registration = registration.OrderBy(s => s.courseCategory);
                            break;
                        case "Name":
                            if (sortOrder == "DESC")
                                registration = registration.OrderByDescending(s => s.Name);
                            else
                                registration = registration.OrderBy(s => s.Name);
                            break;
                        case "PPursuing":
                            if (sortOrder == "DESC")
                                registration = registration.OrderByDescending(s => s.PPursuing);
                            else
                                registration = registration.OrderBy(s => s.PPursuing);
                            break;
                        case "PCompleted":
                            if (sortOrder == "DESC")
                                registration = registration.OrderByDescending(s => s.PCompleted);
                            else
                                registration = registration.OrderBy(s => s.PCompleted);
                            break;
                        case "PExpired":
                            if (sortOrder == "DESC")
                                registration = registration.OrderByDescending(s => s.PExpired);
                            else
                                registration = registration.OrderBy(s => s.PExpired);
                            break;

                        case "PCancelled":
                            if (sortOrder == "DESC")
                                registration = registration.OrderByDescending(s => s.PCancelled);
                            else
                                registration = registration.OrderBy(s => s.PCancelled);
                            break;
                        case "PreRegistered":
                            if (sortOrder == "DESC")
                                registration = registration.OrderByDescending(s => s.PreRegistered);
                            else
                                registration = registration.OrderBy(s => s.PreRegistered);
                            break;
                        case "Ppractpending":
                            if (sortOrder == "DESC")
                                registration = registration.OrderByDescending(s => s.Ppractpending);
                            else
                                registration = registration.OrderBy(s => s.Ppractpending);
                            break;
                        default:
                            registration = registration.OrderBy(s => s.ID);
                            break;
                    }
                }
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    registration = registration.Where(a => roleCourses.Contains(a.ID));
                }
                PagingBar1.Bind(registration, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                BreadCrumb1.Render();
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
            lblHeading.Text = "Registered Students";
        }
        else
        {
            Response.Redirect("Admin_RegStudSummary", true);
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
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl1 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl);
                HyperLink hl2 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);
                HyperLink hl3 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);
                HyperLink hl4 = (HyperLink)e.Row.Cells[6].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl);
                HyperLink hl5 = (HyperLink)e.Row.Cells[7].Controls[0];
                hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}