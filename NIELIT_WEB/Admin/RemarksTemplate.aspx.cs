using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
public partial class Admin_RemarksTemplate : BasePage
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
                    BindRemarks();
                    BindGridView();  
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Remarks", "Admin/RemarksTemplate.aspx", ""));
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
    protected void BindRemarks()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var remarks = from p in context.Remarks
                               orderby (p.DisplayOrder)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlremarks, remarks, lst);
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
            Int32 RemarksID = 0;
            if (ddlremarks.SelectedValue != "0")
                RemarksID = Convert.ToInt32(ddlremarks.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var remarks = from s in context.Remarks
                             orderby s.DisplayOrder
                             select new
                             {
                                 ID = s.ID,
                                 remarkname = s.Name,
                             };


            if (RemarksID != 0)
            {
                remarks = remarks.Where(s => s.ID == RemarksID);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                remarks = remarks.Where(s => s.remarkname.ToUpper().Contains(searchString));
            }

            remarks = remarks.OrderBy(s => s.remarkname);
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "categoryname":
                        if (sortOrder == "DESC")
                            remarks = remarks.OrderByDescending(s => s.remarkname);
                        else
                            remarks = remarks.OrderBy(s => s.remarkname);
                        break;
                    default:
                        remarks = remarks.OrderBy(s => s.ID);
                        break;
                }
            }
            PagingBar1.Bind(remarks, ref gvMain);
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
            lblHeading.Text = "Remark Details";
            var remarks = context.Remarks.Find(Convert.ToInt32(Request.QueryString["Key"]));
            Txtremarks.Text = remarks.Name;
            Txtdisplayorder.Text = remarks.DisplayOrder.ToString();
            Txtdisplayorder.Enabled = false;
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Remarks Detail" , "Admin/RemarksTemplate.aspx?" + Request.QueryString.ToString(), ""));
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
            lblHeading.Text = "New Remarks";
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Remarks", "", ""));
        }
        else
        {
            Response.Redirect("RemarksTemplate.aspx", true);
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
            Int32 displayorder = Convert.ToInt32(Txtdisplayorder.Text);
            string remarkname = Txtremarks.Text;
            Remarks  remark;
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                if (context.Remarks.Any(s => s.Name.ToUpper() == remarkname.ToUpper()))
                {
                    throw new Exception("This Remark name already exists.");
                }
                else if (context.Remarks.Any(s => s.DisplayOrder == displayorder))
                {
                    throw new Exception("Remark name with this display order already exists.");
                }
                else
                {
                    remark = new EConnect.NIELIT.Remarks();
                    remark.Name = Txtremarks.Text.Trim();
                    remark.DisplayOrder = Convert.ToInt32(Txtdisplayorder.Text.Trim());
                    context.Remarks.Add(remark);
                    context.SaveChanges();
                    strMessage = "New record saved.";
                }
            }
            else
            {
                Int32 KeyID = Convert.ToInt32(Request.QueryString["key"]);
                if (!(context.Remarks.Any(s => s.Name.ToUpper() == remarkname.ToUpper() && s.ID != KeyID)))
                {
                    remark = context.Remarks.Find(Convert.ToInt32(Request.QueryString["Key"]));
                    remark.Name = Txtremarks.Text.Trim();
                    remark.DisplayOrder = Convert.ToInt32(Txtdisplayorder.Text.Trim());
                    context.SaveChanges();
                    strMessage = "Record updated.";
                }
                else
                {
                    throw new Exception(" This Remark already exists.");
                }
            }
            Response.Redirect("RemarksTemplate.aspx?msg="+strMessage);
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
            ddlremarks.SelectedValue = "0";
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
                Remarks remark = context.Remarks.Find(Convert.ToInt32(hfActionID.Value));
                context.Remarks.Remove(remark);
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
            ShowAlert("Record can not be delted!", true);
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
            var remark = from s in context.Remarks
                           select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                remark = remark.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            remark = remark.OrderBy(s => s.Name);
            foreach (var linkName in remark)
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("RemarksTemplate.aspx", true);
    }
}