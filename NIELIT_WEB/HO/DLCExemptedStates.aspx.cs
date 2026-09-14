using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.Utils.Common;
using EConnect.NIELIT;

public partial class HO_DLCExemptedStates : BasePage
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
            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Index.aspx");
            if (!Page.IsPostBack)
            {

                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    FillState();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillState();

                    FilterFillState();
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("DLC Exempted States", "HO/DLCExemptedStates.aspx?" + Request.QueryString.ToString(), ""));
                }
                if (Request.QueryString["msg"] != null)
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
            BreadCrumb1.Render();
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
            context = new EConnectContext();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            
            lblHeading.Text = "DLC Exempted State";
            //Get last modified date of current record and save it in ViewState object.
            //ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.
            DLCExemptedStates objAddress = context.DLCExemptedStatess.Find(Convert.ToInt32(Request.QueryString["Key"]));

            ddlState.SelectedValue = objAddress.StateID.ToString();
            ddlState.Enabled = false;
            txtEffectiveDtFrom.Text = objAddress.fromDate.ToString("dd-MMM-yyyy");
            txtEffectiveDtFrom.Enabled = false;
            DateTime dt = Convert.ToDateTime(objAddress.toDate);

            if (objAddress.toDate != null)
            {
                txtEffectiveDtTo.Text = dt.ToString("dd-MMM-yyyy");
            }
            else
            {
                txtEffectiveDtTo.Text = "";
            }
            

            txtEffectiveDtTo.Enabled = true;
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("", "HO/DLCExemptedStates.aspx?" + Request.QueryString.ToString(), ""));
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
            int StateID = 0;
            if (ddlAddresType.SelectedValue != "0")
                StateID = Convert.ToInt32(ddlAddresType.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();

            var addr = from s in context.DLCExemptedStatess
                       select new
                       {
                           ID = s.ID,
                           stateid = s.StateID,
                           Fromdate = s.fromDate,
                           Todate = s.toDate,
                           state = s.State.Name         
                       };

            if (!String.IsNullOrEmpty(searchString))
            {
                addr = addr.Where(s => s.state.ToUpper().Contains(searchString));

            }
            if (StateID != 0)
            {
                addr = addr.Where(s => s.stateid == StateID);
            }
            
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "AddressType":
                        if (sortOrder == "DESC")
                            addr = addr.OrderByDescending(s => s.state);
                        else
                            addr = addr.OrderBy(s => s.state);
                        break;
                    case "Fromdate":
                        if (sortOrder == "DESC")
                            addr = addr.OrderByDescending(s => s.Fromdate);
                        else
                            addr = addr.OrderBy(s => s.Fromdate);
                        break;
                    case "Todate":
                        if (sortOrder == "DESC")
                            addr = addr.OrderByDescending(s => s.Todate);
                        else
                            addr = addr.OrderBy(s => s.Todate);
                        break;
                    
                    default:
                        addr = addr.OrderBy(s => s.state);
                        break;
                }
            }
            PagingBar1.Bind(addr, ref gvMain);
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
        try
        {
            if (btnMode.ViewMode == ToggleView.Mode.New)
            {
                FillState();
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                //Change the heading text as required
                lblHeading.Text = "New DLC Exempted State";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New DLC Exempted State", "#", ""));
                //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New DLC Exempted", "HO/DLCExemptedStates.aspx?" + Request.QueryString.ToString(), ""));
            }
            else
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("DLCExemptedStates=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=?ID=" + Request.QueryString["ID"].ToString()), true);
                }
                else
                {
                    Response.Redirect("DLCExemptedStates.aspx", true);
                }
                //Response.Redirect("DLCExemptedStates.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
            DLCExemptedStates objAddress;
            Nullable<DateTime> Todate = null;
            Int32 userid = Convert.ToInt32(Session["UserTypeId"]);
            DateTime fromdate = Convert.ToDateTime(txtEffectiveDtFrom.Text);
            Int64 Stateid = Convert.ToInt64(ddlState.SelectedValue);

            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                if (context.DLCExemptedStatess.Count(reg => reg.StateID == Stateid && reg.toDate == null) == 0)
                {
                    if (context.DLCExemptedStatess.Count(reg => reg.StateID == Stateid && reg.toDate >= fromdate) == 0)
                    {
                        if (DateTime.Now < fromdate || DateTime.Now.ToLongDateString() == fromdate.ToLongDateString())
                        {
                            objAddress = new DLCExemptedStates();
                            objAddress.StateID = Convert.ToInt64(ddlState.SelectedValue);
                            objAddress.fromDate = Convert.ToDateTime(txtEffectiveDtFrom.Text);
                            objAddress.toDate = Todate;
                            objAddress.enterDate = DateTime.Now;
                            objAddress.enterBy = userid;
                            context.DLCExemptedStatess.Add(objAddress);
                            context.SaveChanges();
                            strMessage = "New record saved.";
                        }
                        else
                        {
                            txtEffectiveDtFrom.Text = "";
                            txtEffectiveDtFrom.Focus();
                            lblerror.Visible = true;
                            lblerror.Text = "Effective From Date should be greater than or equal to current date. ";
                            return;
                        }
                    }
                    else
                    {
                        txtEffectiveDtFrom.Focus();
                        lblerror.Visible = true;
                        lblerror.Text = ddlState.SelectedItem.ToString() + " State is already exempted. ";
                        return;
                    }
                    
                }
                else
                {
                    //txtEffectiveDtFrom.Text = "";
                    txtEffectiveDtFrom.Focus();
                    lblerror.Visible = true;
                    lblerror.Text = ddlState.SelectedItem.ToString() + " State is already exempted. ";
                    return;
                }
            }
            else
            {
                ////Initialize current object by loading it and get its current modified date
                DateTime todate1;
                if (txtEffectiveDtTo.Text != "")
                {
                    todate1 = Convert.ToDateTime(txtEffectiveDtTo.Text);
                }
                else
                {
                    txtEffectiveDtTo.Focus();
                    lblerror.Visible = true;
                    lblerror.Text = "Please enter Effective To Date. ";
                    return;
                }
                Int32 id = Convert.ToInt32(Request.QueryString["Key"]);
                if (context.DLCExemptedStatess.Count(reg => reg.ID == id && reg.toDate != null) == 0)
                {
                    if (fromdate < todate1)
                    {
                        objAddress = context.DLCExemptedStatess.Find(Convert.ToInt32(Request.QueryString["Key"]));

                        objAddress.toDate = Convert.ToDateTime(txtEffectiveDtTo.Text);
                        objAddress.enterDate = DateTime.Now;
                        objAddress.enterBy = userid;

                        context.SaveChanges();
                        strMessage = "Record updated.";
                    }
                    else
                    {
                        txtEffectiveDtTo.Text = "";
                        txtEffectiveDtTo.Focus();
                        lblerror.Visible = true;
                        lblerror.Text = "Effective To Date should be greater than Effective From Date. ";
                        return;
                    }
                }
                else
                {
                    //txtEffectiveDtFrom.Text = "";
                    txtEffectiveDtFrom.Focus();
                    lblerror.Visible = true;
                    lblerror.Text = "This State is already exempted ";
                    return;
                }

            }

            Response.Redirect("DLCExemptedStates.aspx?msg=" + strMessage, true);
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
            ddlAddresType.SelectedValue = "0";
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
            context = new EConnectContext();
            if (hfActionID.Value != "")
            {
                String recordID = hfActionID.Value.Split('$')[0].ToString();
                LinkButton btnAction = (LinkButton)sender;
                if (btnAction.CommandName == "Delete")
                {
                    //Load the object and apply validateion if required
                    //call delete function
                    //bind the grid again
                    BindGridView();
                    ShowAlert("Record deleted successfully.", true);
                    hfActionID.Value = "";
                }
                else if (btnAction.CommandName == "Action")
                {
                    //Load the object and apply validateion if required
                    //call function to perform required action
                    //bind the grid again
                    BindGridView();
                    ShowAlert("Record Action1 successfully.", true);
                    hfActionID.Value = "";
                }
                uPnlGrid.Update();
            }
        }
        catch (Exception ex)
        {
            hfActionID.Value = "";
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }
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
            var fillState = (from p in context.Locations
                                 join q in context.DLCExemptedStatess on p.ID equals q.StateID
                                 where p.LocationTypeID == 2
                                // && p.effectiveToDate == null
                                 orderby p.Name ascending
                                 select new { Name = p.Name });
            if (!String.IsNullOrEmpty(searchString))
            {
                fillState = fillState.Where(s => s.Name.ToUpper().Contains(searchString));
            }

            foreach (var user in fillState)
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
        try
        {
            
            Response.Redirect("DLCExemptedStates.aspx", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    public void FillState()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var fillState = (from p in context.Locations
                                    where p.LocationTypeID == 2
                                 //   && p.effectiveToDate == null
                                    orderby p.Name ascending
                                    select new { ValueField = p.ID, TextField = p.Name });
                ListItem lst = new ListItem("--Select One--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlState, fillState, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public void FilterFillState()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var fillState = (from p in context.Locations
                                 join q in context.DLCExemptedStatess on p.ID equals q.StateID
                                 where p.LocationTypeID == 2
                               //  && p.effectiveToDate == null
                                 orderby p.Name ascending
                                 select new { ValueField = p.ID, TextField = p.Name });
                ListItem lst = new ListItem("--Select One--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlAddresType, fillState.Distinct (), lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

}