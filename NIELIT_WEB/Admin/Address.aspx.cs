using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.Utils.Common;
public partial class AddressForm : BasePage
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
                    FillState();
                    EnumUtility.BindListObject(ref ddlAddrType, typeof(EConnect.enmAddressType), new ListItem("--Select One--", "0"));
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillState();
                  
                    EnumUtility.BindListObject(ref ddlAddrType, typeof(EConnect.enmAddressType), new ListItem("--Select One--", "0"));
                    EnumUtility.BindListObject(ref ddlAddresType , typeof(EConnect.enmAddressType), new ListItem("--Select One--", "0"));
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Address", "Address.aspx?" + Request.QueryString.ToString(), ""));
                }
                if (Request.QueryString["msg"]!=null)
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
            tblNavLinks.Visible = true;
            lblHeading.Text = "Address Details";
            //Get last modified date of current record and save it in ViewState object.
            //ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.
            Address objAddress = context.Addresses.Find(Convert.ToInt32(Request.QueryString["Key"]));
           
            ddlAddrType.SelectedValue = objAddress.AddressTypeID.ToString();
            ddlAddrType.Enabled = false;
            txtEffectiveDtFrom.Text = objAddress.EffectiveDateFrom.ToString("dd-MMM-yyyy");
            txtAddress1.Text = objAddress.AddressLine1.ToString();
            txtAddress2.Text = objAddress.AddressLine2.ToString();
            txtCity.Text = objAddress.CityName.ToString();
            txtPin.Text = objAddress.PinCode.ToString();
            ddlCountry.SelectedValue = objAddress.CountryID.ToString();
            if (objAddress.AddressLine3 != null )
            { txtAddress3.Text = objAddress.AddressLine3.ToString(); }
            ddlState.SelectedValue = objAddress.StateID.ToString();
            ddlState_SelectedIndexChanged(ddlState, EventArgs.Empty);
            ddlDistrict.SelectedValue = objAddress.DistrictID.ToString();
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(objAddress.AddressLine1, "Address.aspx?" + Request.QueryString.ToString(), ""));
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
            int TypeAddr = 0;
            if (ddlAddresType.SelectedValue != "0")
                TypeAddr = Convert.ToInt32(ddlAddresType.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
           
            var addr = from s in context.Addresses
                       select new
                       {
                           ID = s.ID,
                           AddressTypeID=s.AddressTypeID,
                           AddressType = s.AddressType.Name,
                           addr1 = (s.AddressLine1 + " " + s.AddressLine2 + ""),
                           addr2 = (s.AddressLine3 ?? "" + " "),
                           addr3=(s.CityName),
                           date = s.EffectiveDateFrom,
                           state=s.State.Name                      
                       };

            if (!String.IsNullOrEmpty(searchString))
            {
                addr = addr.Where(s => s.addr1.ToUpper().Contains(searchString) || 
                                       s.addr2.ToUpper().Contains(searchString) || 
                                       s.addr3.ToUpper().Contains(searchString) || 
                                       s.AddressType.ToUpper().Contains(searchString));
                                        
            }
            if (TypeAddr != 0)
                addr = addr.Where(s => s.AddressTypeID == TypeAddr);
             addr = addr.OrderBy(s => s.AddressType);
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "AddressType":
                        if (sortOrder == "DESC")
                            addr = addr.OrderByDescending(s => s.AddressType);
                        else
                            addr = addr.OrderBy(s => s.AddressType);
                        break;
                    case "date":
                        if (sortOrder == "DESC")
                            addr = addr.OrderByDescending(s => s.date);
                        else
                            addr = addr.OrderBy(s => s.date);
                        break;
                    case "state":
                        if (sortOrder == "DESC")
                            addr = addr.OrderByDescending(s => s.state);
                        else
                            addr = addr.OrderBy(s => s.state);
                        break;
                    case "addr1":
                         if (sortOrder == "DESC")
                             addr = addr.OrderByDescending(s => s.addr1);
                        else
                             addr = addr.OrderBy(s => s.addr1);
                        break;
                    default:
                        addr = addr.OrderBy(s => s.AddressType);
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
    public void FillState()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var fillState = (from p in context.Locations
                                 where p.LocationTypeID == 2
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
                lblHeading.Text = "New Address";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Address", "Address.aspx?" + Request.QueryString.ToString(), ""));
            }
            else
            {
                Response.Redirect("Address.aspx", true);
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
            Address objAddress;
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                objAddress = new EConnect.Address();
                objAddress.AddressTypeID = Convert.ToInt32(ddlAddrType.SelectedValue);
                objAddress.EffectiveDateFrom = Convert.ToDateTime(txtEffectiveDtFrom.Text);
                objAddress.AddressLine1 = txtAddress1.Text;
                objAddress.AddressLine2 = txtAddress2.Text;
                objAddress.AddressLine3 = txtAddress3.Text;
                objAddress.CityName = txtCity.Text;
                objAddress.PinCode = Convert.ToInt32(txtPin.Text);
                objAddress.CountryID = Convert.ToInt32(ddlCountry.SelectedValue);
                objAddress.StateID = Convert.ToInt32(ddlState.SelectedValue);
                objAddress.DistrictID = Convert.ToInt32(ddlDistrict.SelectedValue);

                context.Addresses.Add(objAddress);
                context.SaveChanges();
                strMessage = "New record saved.";
            }
            else
            {
                ////Initialize current object by loading it and get its current modified date
                objAddress= context.Addresses.Find(Convert.ToInt32(Request.QueryString["Key"]));
                objAddress.AddressTypeID = Convert.ToInt32(ddlAddrType.SelectedValue);
                objAddress.EffectiveDateFrom = Convert.ToDateTime(txtEffectiveDtFrom.Text);
                //objAddress.EffectiveDateTo = Convert.ToDateTime(txtEffectiveDtTo.Text);
                objAddress.AddressLine1 = txtAddress1.Text;
                objAddress.AddressLine2 = txtAddress2.Text;
                objAddress.AddressLine3 = txtAddress3.Text;
                objAddress.CityName = txtCity.Text;
                objAddress.PinCode = Convert.ToInt32(txtPin.Text);
                objAddress.CountryID = Convert.ToInt32(ddlCountry.SelectedValue);
                objAddress.StateID = Convert.ToInt32(ddlState.SelectedValue);
                objAddress.DistrictID = Convert.ToInt32(ddlDistrict.SelectedValue);

                context.SaveChanges();
                strMessage = "Record updated.";

            }

            //Call save method
            //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
            //Redirect it to list mode
            Response.Redirect("Address.aspx?msg=" + strMessage);
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
        try
        {
            Response.Redirect("Address.aspx", true);
        }
        catch(Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id2 = Convert.ToInt32(ddlState.SelectedValue);
            ddlDistrict.Items.Clear();
            fillDistrict(id2);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void fillDistrict(int sid)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (sid != null || sid != 0)
                {
                    var district2 = from s in context.Locations
                                    orderby (s.Name)
                                    where s.LocationTypeID == 4 && s.ParentLocationID == sid
                                    select new { ValueField = s.ID, TextField = s.Name };

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlDistrict, district2, lst);
                }

            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

      
    }
}