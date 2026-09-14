using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class Admin_Notice : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
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
                    FillActivites();
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Notice Activities", "Admin/Notice.aspx", ""));
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
    protected void FillActivites()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var activites = from p in context.NoticeEvents
                               orderby (p.ActivityName)
                               select new { ValueField = p.ID, TextField = p.ActivityName };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlfilteractivityname, activites, lst);
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
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            Int64 ActivityID = 0;
            if (ddlfilteractivityname.SelectedValue != "0")
                ActivityID = Convert.ToInt64(ddlfilteractivityname.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var activity = from s in context.NoticeEvents
                             orderby s.ActivityName
                             select new
                             {
                                 ID = s.ID,
                                 activityname = s.ActivityName,
                                 activitydesc = s.ActivityDescription,
                                 activityid = s.ID
                             };
            if (ActivityID != 0)
            {
                activity = activity.Where(s => s.activityid == ActivityID);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                activity = activity.Where(s => s.activityname.ToUpper().Contains(searchString));
            }
            activity = activity.OrderBy(s => s.activityname);
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "activityname":
                        if (sortOrder == "DESC")
                            activity = activity.OrderByDescending(s => s.activityname);
                        else
                            activity = activity.OrderBy(s => s.activityname);
                        break;
                    case "activitydesc":
                        if (sortOrder == "DESC")
                            activity = activity.OrderByDescending(s => s.activitydesc);
                        else
                            activity = activity.OrderBy(s => s.activitydesc);
                        break;
                    default:
                        activity = activity.OrderBy(s => s.ID);
                        break;
                }
            }
            PagingBar1.Bind(activity, ref gvMain);
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
            lblHeading.Text = "Activity Details";
            NoticeEvent notice = context.NoticeEvents.Find(Convert.ToInt64(Request.QueryString["Key"]));
            txtactname.Text = GetInitCap(notice.ActivityName.ToString());
            txactstartdate.Text = notice.StartDate.ToString("dd-MMM-yyyy");
            txactenddate.Text = notice.EndDate.ToString("dd-MMM-yyyy");
            txtactivityDescription.Text = GetInitCap(notice.ActivityDescription.ToString());
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(notice.ActivityName.ToString(), "", ""));

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
                ShowAlert("Sorry! You don't have rights to add new record.", true);
                return;
            }
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New Activity";
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Activity", "", ""));
        }
        else
        {
            Response.Redirect("Notice.aspx", true);
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
            DateTime startdate = Convert.ToDateTime(txactstartdate.Text);
            DateTime enddate = Convert.ToDateTime(txactenddate.Text);
            string actname = txtactname.Text.Trim();
            string actdesc = txtactivityDescription.Text.Trim();
            Int32 length = actdesc.Length;
            NoticeEvent notice;
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                if (context.NoticeEvents.Any(s => s.ActivityName.ToUpper() == actname.ToUpper()))
                {
                   throw new Exception(" Notice with this activity name already exists.");
                }
                else if(context.NoticeEvents.Any(s => s.ActivityDescription.ToUpper() == actdesc.ToUpper()))
                {
                    throw new Exception("Notice with this activity description already exists.");
                }
                else
                {
                    notice = new EConnect.NIELIT.NoticeEvent();
                    notice.ActivityName = txtactname.Text.Trim();
                    if (length > 300)
                        txtactivityDescription.Text = txtactivityDescription.Text.Trim().ToString().Substring(0, 300);
                    notice.ActivityDescription = txtactivityDescription.Text.Trim();
                    notice.StartDate = Convert.ToDateTime(txactstartdate.Text.Trim());
                    notice.EndDate = Convert.ToDateTime(txactenddate.Text.Trim());
                    context.NoticeEvents.Add(notice);
                    context.SaveChanges();
                    strMessage = "New record saved";
                }
            }
            else
            {
                Int64 KeyID = Convert.ToInt64(Request.QueryString["key"]);
                if (!(context.NoticeEvents.Any(s => s.ActivityName.ToUpper() == actname.ToUpper() && s.ActivityDescription.ToUpper() == actdesc.ToUpper() && s.ID!=KeyID)))
                {
                    notice = context.NoticeEvents.Find(Convert.ToInt64(Request.QueryString["Key"]));
                    notice.ActivityName = txtactname.Text.Trim();
                    if (length > 300)
                        txtactivityDescription.Text = txtactivityDescription.Text.Trim().ToString().Substring(0, 300);
                    notice.ActivityDescription = txtactivityDescription.Text.Trim();
                    notice.StartDate = Convert.ToDateTime(txactstartdate.Text.Trim());
                    notice.EndDate = Convert.ToDateTime(txactenddate.Text.Trim());
                    context.SaveChanges();
                    strMessage = "Record updated";
                }
                else
                {
                    throw new Exception(" This Activity already exists.");
                }
            }
            Response.Redirect("Notice.aspx?msg="+strMessage);
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
            ddlfilteractivityname.SelectedValue = "0";
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
            var category = from s in context.NoticeEvents
                           select new { Name = s.ActivityName };
            if (!String.IsNullOrEmpty(searchString))
            {
                category = category.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            category = category.OrderBy(s => s.Name);
            foreach (var linkName in category)
            {
                items.Add(linkName.Name);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
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
                NoticeEvent noticeevent = context.NoticeEvents.Find(Convert.ToInt64(hfActionID.Value));
                context.NoticeEvents.Remove(noticeevent);
                context.SaveChanges();
                BindGridView();
                ShowAlert("Record deleted successfully.", true);
                hfActionID.Value = "";
            };
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be deleted!", true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("Notice.aspx", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}