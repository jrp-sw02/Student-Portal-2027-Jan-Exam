using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;

public partial class ChangeHistory : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

        Sidelink.Items.Add(new SideLinkItem("Apply For Changes", "FrmEditCandidateDetail.aspx?SrcType=0", "", ""));
        Sidelink.SideLinkType = SideLinkItem.SideLinkType.Hyperlink;
        Sidelink.Render();
        Sidelink.Visible = true;
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("History Detail", "CAND/ChangeHistory.aspx", ""));  
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    //l1.Visible = true;
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
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
    protected void ShowEditMode()
    {

        try
        {
            //context = new EConnectContext();
            //btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            //ucSearchBar.Visible = false;
            //btnSave.Text = "Update";
            lblHeading.Text = "History Detail";
            //Lbreadscum.Text = "Change History Detail";
            Sidelink1.Items.Add(new SideLinkItem("Print", "ProfileCandPrint.aspx?type="+ Request.QueryString["t"].ToString() + "","",""));
            Sidelink1.Items.Add(new SideLinkItem("View Status", "ProfileCandStatus.aspx?type=" + Request.QueryString["t"].ToString() + "", "", ""));
            Sidelink1.SideLinkType = SideLinkItem.SideLinkType.Hyperlink;
            Sidelink1.Render();
            Sidelink1.Visible = true;
            Sidelink.Visible = false;
            if ((!String.IsNullOrEmpty(Request.QueryString["t"])))
            {
                if (Request.QueryString["t"] == "Name Change")
                {
                    //l2.Visible = true;
                    t2.Visible = true;
                }
                else
                {
                    //l2.Visible = true;
                    d1.Visible = true;
                }
            }
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.
             
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //finally
        //{
        //    context.Dispose();
        //}
    }
    protected void BindGridView()
    {
        try
        {
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            Int32 RequestID = 0;
            if (ddlFilter1.SelectedValue != "0")
                RequestID = Convert.ToInt32(ddlFilter1.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var request = (from r in context.CandidateRequests
                         join g in context.CandidateRequestTypes on r.CandidateRequestTypeID equals g.ID
                         orderby (g.Name)
                           where r.CandidateID == entityID
                           select new { RequestType = g.Name, RequestNo = r.ID, RequestDate = r.DateOfRequest, Status =  r.StatusID,RequestTypeID =g.ID });

            if (RequestID != 0)
                request = request.Where(p => p.RequestTypeID == RequestID);
            if (!String.IsNullOrEmpty(searchString))
            {
                request = request.Where(s => s.RequestType.ToUpper().Contains(searchString));
                                     
            }
            //if (userType != 0)
            //    users = users.Where(s => s.UserTypeID == userType);
            //users = users.OrderBy(s => s.UserName);
            //if (!string.IsNullOrEmpty(sortOrder))
            //{
            //    switch (sortField)
            //    {
            //        case "LoginID":
            //            if (sortOrder == "DESC")
            //                users = users.OrderByDescending(s => s.LoginID);
            //            else
            //                users = users.OrderBy(s => s.LoginID);
            //            break;
            //        case "UserName":
            //            if (sortOrder == "DESC")
            //                users = users.OrderByDescending(s => s.UserName);
            //            else
            //                users = users.OrderBy(s => s.UserName);
            //            break;
            //        case "UserType":
            //            if (sortOrder == "DESC")
            //                users = users.OrderByDescending(s => s.UserTypeID);
            //            else
            //                users = users.OrderBy(s => s.UserTypeID);
            //            break;
            //        case "HasLoginAccess":
            //            if (sortOrder == "DESC")
            //                users = users.OrderByDescending(s => s.HasLoginAccess);
            //            else
            //                users = users.OrderBy(s => s.HasLoginAccess);
            //            break;
            //        case "PasswordExpiryDate":
            //            if (sortOrder == "DESC")
            //                users = users.OrderByDescending(s => s.LastPasswordChangedOn);
            //            else
            //                users = users.OrderBy(s => s.LastPasswordChangedOn);
            //            break;
            //        default:
            //            users = users.OrderBy(s => s.LoginID);
            //            break;
            //    }

            //}
            PagingBar1.Bind(request, ref gvMain);
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
    //protected void ToggleViewMode_Changed(object sender, EventArgs e)
    //{
    //    if (btnMode.ViewMode == ToggleView.Mode.New)
    //    {
    //        btnMode.ViewMode = ToggleView.Mode.List;
    //        mltvTab.ActiveViewIndex = 1;
    //        pnlFilter.Visible = false;
    //        ucSearchBar.Visible = false;
    //        //Change the heading text as required
    //        lblHeading.Text = "View Change History";
    //    }
    //    else
    //    {
    //        Response.Redirect("ChangeHistory.aspx", true);
    //    }
    //}
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
            //context = new EConnectContext();
            //create and object 
            //User objUser;
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
            Response.Redirect("ChangeHistory.aspx?msg=" + strMessage);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
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
            ddlFilter1.SelectedValue = "0";
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
                //HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                //hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);

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
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var request = from s in context.CandidateRequestTypes
                          select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                request = request.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            request = request.OrderBy(s => s.Name);

            foreach (var request1 in request)
            {
                items.Add(request1.Name);
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
        Response.Redirect("ChangeHistory.aspx", true);
    }
}