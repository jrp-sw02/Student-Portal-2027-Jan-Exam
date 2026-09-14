/////////////////////////////////////////////////////////////////////////////////
//   File Name		: RegStatusMas.aspx.cs
//   Namespace		: EConnect
//   Class Name(s)	: HO_RegStatusMas
//   Version #	    : 1.0
/////////////////////////////////////////////////////////////////////////////////

#region[Assemblies]
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.Utils.Common;
using EConnect.URM;
using EConnect.NIELIT;
#endregion


public partial class HO_RegStatusMas : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;

        DateTime dateTime = DateTime.UtcNow.Date;
        txtEntDt.Text = dateTime.ToString();

        try
        {
            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Index.aspx");
            if (!Page.IsPostBack)
            {

                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    //FillState();
                   // EnumUtility.BindListObject(ref ddlRegName, typeof(EConnect.NIELIT.RegStatusMas), new ListItem("--Select One--", "0"));
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    //FillState();

                    //EnumUtility.BindListObject(ref ddlAddrType, typeof(EConnect.enmAddressType), new ListItem("--Select One--", "0"));
                    //EnumUtility.BindListObject(ref ddlRegName, typeof(EConnect.enmAddressType), new ListItem("--Select One--", "0"));
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Reg Status Master", "RegStatusMas.aspx?" + Request.QueryString.ToString(), ""));
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
            txtRegDesc.Enabled = true;
            txtRegSt.Enabled = true;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            tblNavLinks.Visible = true;
            lblHeading.Text = "Reg Status Details";
            
            RegStatusMas objRegstatus = context.reqStatusMass.Find(Convert.ToInt32(Request.QueryString["Key"]));
            //objRegstatus.reqStatusName = txtRegSt.Text.Trim();
            //objRegstatus.reqStatusDesc = txtRegDesc.Text.Trim();
            txtRegSt.Text = objRegstatus.reqStatusName.ToString();
            txtRegDesc.Text = objRegstatus.reqStatusDesc.ToString();
            txtEntDt.Enabled = false;
            txtEntDt.Text = objRegstatus.enterDate.ToString("dd-MMM-yyyy");


            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(objRegstatus.reqStatusName, "RegStatusMas.aspx?" + Request.QueryString.ToString(), ""));
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
            //int TypeAddr = 0;
            //if (ddlRegName.SelectedValue != "0")
            //    TypeAddr = Convert.ToInt32(ddlRegName.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();

            var reg = from s in context.reqStatusMass
                      select new
                      {
                          ID = s.ID,
                          reqStatusName = s.reqStatusName,
                          reqStatusDesc = s.reqStatusDesc,
                           enterDate = s.enterDate
                          //enterBy = s.enterBy,
                         

                      };

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    addr = addr.Where(s => s.addr1.ToUpper().Contains(searchString) ||
            //                           s.addr2.ToUpper().Contains(searchString) ||
            //                           s.addr3.ToUpper().Contains(searchString) ||
            //                           s.AddressType.ToUpper().Contains(searchString));

            //}
            //if (TypeAddr != 0)
            //    addr = addr.Where(s => s.AddressTypeID == TypeAddr);
            //addr = addr.OrderBy(s => s.AddressType);
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "reqStatusName":
                        if (sortOrder == "DESC")
                            reg = reg.OrderByDescending(s => s.reqStatusName);
                        else
                            reg = reg.OrderBy(s => s.reqStatusName);
                        break;
                    case "reqStatusDesc":
                        if (sortOrder == "DESC")
                            reg = reg.OrderByDescending(s => s.reqStatusDesc);
                        else
                            reg = reg.OrderBy(s => s.reqStatusDesc);
                        break;
                    //case "enterBy":
                    //    if (sortOrder == "DESC")
                    //        reg = reg.OrderByDescending(s => s.enterBy);
                    //    else
                    //        reg = reg.OrderBy(s => s.enterBy);
                    //    break;
                    case "enterDate":
                        if (sortOrder == "DESC")
                            reg = reg.OrderByDescending(s => s.enterDate);
                        else
                            reg = reg.OrderBy(s => s.enterDate);
                        break;
                    default:
                        reg = reg.OrderBy(s => s.reqStatusName);
                        break;
                }
            }
            PagingBar1.Bind(reg, ref gvMain);
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
    //public void FillState()
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            var fillState = (from p in context.Locations
    //                             where p.LocationTypeID == 2
    //                             orderby p.Name ascending
    //                             select new { ValueField = p.ID, TextField = p.Name });
    //            ListItem lst = new ListItem("--Select One--", "0");
    //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlState, fillState, lst);
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }

    //}
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
                //FillState();
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                txtRegSt.Enabled = true;
                //Change the heading text as required
                lblHeading.Text = "New Reg Status";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Reg Status Name", "RegStatusMas.aspx?" + Request.QueryString.ToString(), ""));
            }
            else
            {
                Response.Redirect("RegStatusMas.aspx", true);
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
            RegStatusMas objRegstatus;
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                objRegstatus = new EConnect.NIELIT.RegStatusMas();
                objRegstatus.reqStatusName = txtRegSt.Text.Trim();
                objRegstatus.reqStatusDesc = txtRegDesc.Text.Trim();
                objRegstatus.enterDate = Convert.ToDateTime(txtEntDt.Text);
                objRegstatus.enterBy = Convert.ToInt32("1");

               
                context.reqStatusMass.Add(objRegstatus);
                context.SaveChanges();
                strMessage = "Record saved Successfully..";
            }
            else
            {
                ////Initialize current object by loading it and get its current modified date
                objRegstatus = context.reqStatusMass.Find(Convert.ToInt32(Request.QueryString["Key"]));

                objRegstatus.reqStatusName = txtRegSt.Text;
                objRegstatus.reqStatusDesc = txtRegDesc.Text;
                objRegstatus.enterDate = Convert.ToDateTime(txtEntDt.Text);
                              

                context.SaveChanges();
                strMessage = "Record updated.";

            }

            //Call save method
            //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
            //Redirect it to list mode
            Response.Redirect("RegStatusMas.aspx?msg=" + strMessage);
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

            ddlRegName.SelectedValue = "0";
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

         ////   //Added by Reena
         //   var reg = from s in context.reqStatusMass
         //             select new
         //             {
         //                reqStatusName = s.reqStatusName
         //              };
         //   if (!String.IsNullOrEmpty(searchString))
         //   {
         //       reg = reg.Where(s => s.reqStatusName.ToUpper().Contains(searchString));
         //   }
         //   reg = reg.OrderBy(s => s.reqStatusName);
         //////
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
            Response.Redirect("RegStatusMas.aspx", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    //protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int id2 = Convert.ToInt32(ddlState.SelectedValue);
    //        ddlDistrict.Items.Clear();
    //        fillDistrict(id2);
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}
    //public void fillDistrict(int sid)
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            ListItem lst = new ListItem("--Select One--", "0");
    //            if (sid != null || sid != 0)
    //            {
    //                var district2 = from s in context.Locations
    //                                orderby (s.Name)
    //                                where s.LocationTypeID == 4 && s.ParentLocationID == sid
    //                                select new { ValueField = s.ID, TextField = s.Name };

    //                EConnect.Utils.Common.ControlUtility.BindListObject(ddlDistrict, district2, lst);
    //            }

    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }


    //}
}
