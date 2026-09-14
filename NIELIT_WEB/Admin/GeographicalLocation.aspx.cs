using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
public partial class GeographicalLocation : BasePage
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
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    FillLocationType();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillLocationType();
                    FillFilterLocType();
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Geographical Location", "Admin/GeographicalLocation.aspx", ""));
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
    protected void FillFilterLocType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 city = Convert.ToInt32(enmLocationType.City);
                Int32 tehsil = Convert.ToInt32(enmLocationType.Tehsil);
                Int32 zone = Convert.ToInt32(enmLocationType.Zone);
                ListItem lst = new ListItem("--All--", "0");
                //var locType = from p in context.LocationTypes
                //              where (p.ID != city  && p.ID!= zone && p.ID != tehsil )
                //              orderby (p.ID)
                //              select new { ValueField = p.ID, TextField = p.Name };
                var locType=(from p in context.LocationTypes 
                             where (p.ID != city)
                            join q in context.Locations 
                            on p.ID equals q.LocationTypeID
                            orderby p.ID 
                            select new { ValueField = p.ID, TextField = p.Name }).Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlFLocType, locType, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillLocationType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 city = Convert.ToInt32(enmLocationType.City);
                Int32 tehsil = Convert.ToInt32(enmLocationType.Tehsil);
                Int32 zone = Convert.ToInt32(enmLocationType.Zone);
                ListItem lst = new ListItem("--Select One--", "0");
                var locType = from p in context.LocationTypes
                              where (p.ID != city && p.ID != zone && p.ID != tehsil)
                              orderby (p.ID)
                              select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlLocationType, locType, lst);
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
            lblHeading.Text = "Geographical Location Details";
            //tblNavLinks.Visible = true;
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.

            Location objLocation = context.Locations.Find(Convert.ToInt32(Request.QueryString["Key"]));

            LocationType objLocType = context.LocationTypes.Find(Convert.ToInt32(objLocation.LocationTypeID.ToString()));
            if ((enmLocationType)objLocType.ID == enmLocationType.Country)
                lblTypeName.Text = "Capital Name";
            else if ((enmLocationType)objLocType.ID == enmLocationType.State)
                lblTypeName.Text = "Capital Name";
            else if ((enmLocationType)objLocType.ID == enmLocationType.District)
                lblTypeName.Text = "Headquarter Name";

            ddlLocationType.SelectedValue = objLocation.LocationTypeID.ToString();
            txtName.Text = objLocation.Name;
            txtCode.Text = objLocation.Code;
           
            if (objLocation.ParentLocationID.HasValue)
            {
                txtParent.Text = objLocation.ParentLocation.Name;
                hfParentId.Value = objLocation.ParentLocationID.Value.ToString();
            }
            txtTypeName.Text = objLocation.TypeName;
          
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(objLocation.Name, "Admin/GeographicalLocation.aspx?Key=" + Request.QueryString["Key"], ""));
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
    protected void BindGridView()
    {
        try
        {
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            int locationType = 0;
            if (ddlFLocType.SelectedValue != "0")
                locationType = Convert.ToInt32(ddlFLocType.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            Int32 city = Convert.ToInt32(enmLocationType.City);
            Int32 tehsil = Convert.ToInt32(enmLocationType.Tehsil);
            Int32 zone = Convert.ToInt32(enmLocationType.Zone);
            var query = from s in context.Locations
                        where (s.LocationTypeID != city && s.LocationTypeID != zone && s.LocationTypeID != tehsil)
                        select new
                        {
                            ID = s.ID,
                            name=s.Name,
                            locTypeID=s.LocationTypeID,
                            locationType=s.LocationType.Name ,
                            parentLocId=s.ParentLocationID,
                            parentTypeName=s.ParentLocation.Name,
                            parentType = (from p in context.LocationTypes 
                                          where p.ID == s.LocationTypeID
                                          select p.ParentLocationType.Name).FirstOrDefault()
                                     
                        };
            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.name.ToUpper().Contains(searchString)
                                       || s.locationType.ToUpper().Contains(searchString));
            }
            if (locationType != 0)
                query = query.Where(s => s.locTypeID == locationType);
            query = query.OrderBy(s => s.name);
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "name":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.name);
                        else
                            query = query.OrderBy(s => s.name);
                        break;
                    case "locationType":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.locationType);
                        else
                            query = query.OrderBy(s => s.locationType);
                        break;
                    case "parentTypeName":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.parentTypeName);
                        else
                            query = query.OrderBy(s => s.parentTypeName);
                        break;
                    case "parentType":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.parentType);
                        else
                            query = query.OrderBy(s => s.parentType);
                        break;
                    default:
                        query = query.OrderBy(s => s.name);
                        break;
                }
            }
            PagingBar1.Bind(query, ref gvMain);
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
            lblHeading.Text = "New Geographical Location";
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Geographical Location", "#", ""));
        }
        else
        {
            Response.Redirect("GeographicalLocation.aspx", true);
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
            //create and object 
            Location objLocation;
            string name=txtName.Text.ToUpper().Trim();
            string typename = txtTypeName.Text.ToUpper().Trim();
            Int32 LocationTypeId=Convert.ToInt16(ddlLocationType.SelectedValue);
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                    objLocation = new Location();
                    if (txtCode.Text != "")
                    {
                        string code = txtCode.Text.ToUpper();
                        if (context.Locations.Any(s => s.Code.ToUpper() == code))
                        {
                            throw new Exception("This code already exists.");
                        }
                    }
                    if (context.Locations.Any(s => s.Name.ToUpper() == name && s.LocationTypeID == LocationTypeId))
                    {
                        throw new Exception("Location with this name already exists.");
                    }
                    else
                    {
                        if (Convert.ToInt32(ddlLocationType.SelectedValue) != Convert.ToInt32(enmLocationType.Country))
                        {
                            if (txtParent.Text == "")
                            {
                                throw new Exception("Please select parent location name");
                            }
                        }
                        objLocation = new Location();
                        objLocation.Name = txtName.Text.Trim();
                        objLocation.Code = txtCode.Text.ToString().Trim();
                        objLocation.TypeName = txtTypeName.Text.ToString().Trim();
                        objLocation.LocationTypeID = Convert.ToInt32(ddlLocationType.SelectedValue);
                        if (!String.IsNullOrWhiteSpace(hfParentId.Value))
                        {
                            objLocation.ParentLocationID = Convert.ToInt32(hfParentId.Value);
                        }
                        context.Locations.Add(objLocation);
                        context.SaveChanges();
                        strMessage = "New record saved.";
                    }
             }
            else
            {
                ////Initialize current object by loading it and get its current modified date
                Int32 KeyID = Convert.ToInt32(Request.QueryString["key"]);
                if (!(context.Locations.Any(s => s.Name.ToUpper() == name.ToUpper() && s.LocationTypeID == LocationTypeId && s.ID!=KeyID)))
                {

                    objLocation = context.Locations.Find(Convert.ToInt32(Request.QueryString["Key"]));
                    objLocation.Name = txtName.Text.Trim();
                    objLocation.Code = txtCode.Text.Trim();
                    objLocation.TypeName = txtTypeName.Text.ToString().Trim();
                    objLocation.LocationTypeID = Convert.ToInt32(ddlLocationType.SelectedValue);
                    if (Convert.ToInt32(ddlLocationType.SelectedValue) != Convert.ToInt32(enmLocationType.Country))
                    {
                        if (txtParent.Text == "")
                        {
                           throw new Exception("Please select parent location name");
                        }
                    }
                    if (!String.IsNullOrWhiteSpace(hfParentId.Value))
                    {
                        objLocation.ParentLocationID = Convert.ToInt32(hfParentId.Value);
                    }
                    context.SaveChanges();
                    strMessage = "Record updated.";
                }
                else
                {
                      throw new Exception("Record with this name already exist.");
                }
            }
            //Call save method
            //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
            //Redirect it to list mode
            Response.Redirect("GeographicalLocation.aspx?msg="+strMessage);
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
            ddlFLocType.SelectedValue = "0";
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
                if (btnAction.CommandName == "Action")
                {
                    //Load the object and apply validateion if required
                    //call function to perform required action
                    //bind the grid again
                    BindGridView();
                    ShowAlert("Record Action1 successfully.", true);
                    hfActionID.Value = "";
                }
                else if (btnAction.CommandName == "Delete")
                {
                    //Load the object and apply validateion if required
                    //call function to perform required action
                    //bind the grid again
                    if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                    {
                        BreadCrumb1.Render();
                        ShowAlert("Sorry! You don't have rights to delete the records.", true);
                        return;
                    }
                    Location  location = context.Locations.Find(Convert.ToInt32(hfActionID.Value));
                    context.Locations.Remove(location);
                    context.SaveChanges();
                    BindGridView();
                    ShowAlert("Record deleted successfully.", true);
                    hfActionID.Value = "";
                }
                uPnlGrid.Update();
            }
        }
        catch (Exception ex)
        {
            BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be deleted!", true);
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
                //HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                //hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                Image imgAction = (Image)e.Row.FindControl("imgAction");
                imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
                CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
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
            var query  = from s in context.Locations
                        select new { Name = s.Name};
            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            query = query.OrderBy(s => s.Name);

            var query1 = from s in context.Locations
                         select new { Name = s.LocationType.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                query1 = query1.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            query = query.Union(query1).Take(count);
            foreach (var user in query)
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
            Response.Redirect("GeographicalLocation.aspx", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlLocationType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 locTypeId = Convert.ToInt32(ddlLocationType.SelectedValue);
            txtParent.Text = "";
            hfParentId.Value = "";
            context = new EConnectContext();
            LocationType objLocType = context.LocationTypes.Find(locTypeId);
            if ((enmLocationType)objLocType.ID == enmLocationType.Country)
                lblTypeName.Text = "Capital Name";
            else if((enmLocationType)objLocType.ID == enmLocationType.State)
                lblTypeName.Text = "Capital Name";
            else if((enmLocationType)objLocType.ID == enmLocationType.District)
                lblTypeName.Text = "Headquarter Name";
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
    protected void fillParentLocName(Int32 locationTypeID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (locationTypeID != null || locationTypeID != 0)
                {
                    var parentName = from s in context.Locations
                                    orderby (s.Name)
                                    where s.LocationTypeID == locationTypeID
                                    select new { ValueField = s.ID, TextField = s.Name };

                    //EConnect.Utils.Common.ControlUtility.BindListObject(ddlParentName, parentName, lst);
                }

            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void imgPopup_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            context = new EConnectContext();
            tvParents.Nodes.Clear();
            //Location objLocation = context.Locations.Find(Convert.ToInt32(ddlLocationType.SelectedValue));
            Int32 locationTypeId = Convert.ToInt32(ddlLocationType.SelectedValue);
            LocationType objLocType = context.LocationTypes.Find(locationTypeId);

            if (objLocType.ParentID.HasValue)
            {
                var parents = from p in context.Locations
                              where p.LocationTypeID == (int)enmLocationType.Country 
                              select p;
                foreach (Location location in parents)
                {
                    TreeNode tn = new TreeNode(location.Name, location.ID.ToString());

                    if ((enmLocationType)objLocType.ParentID.Value == location.enmLocationType)
                        tn.SelectAction = TreeNodeSelectAction.Select;
                    else
                        tn.SelectAction = TreeNodeSelectAction.Expand;

                    tvParents.Nodes.Add(tn);
                    tvParents.ExpandAll();
                    if(locationTypeId != Convert.ToInt32(enmLocationType.State) && locationTypeId != Convert.ToInt32(enmLocationType.Country))
                    {
                        BindTreeView(ref tn, (enmLocationType)objLocType.ParentID.Value, location.enmLocationType, location.ChildLocations);
                       
                    }
                }


                ModalPopupExtender1.Show();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tt", "alert('Location Type Country does not have any parent location name')", true);

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }
    }
    private void BindTreeView(ref TreeNode ParentMenuItem, enmLocationType pParentOfSelectedObjectType, enmLocationType pCurrentParentType, ICollection<Location> childLocations)
    {
        try
        {
           
            foreach (Location locations in childLocations)
            {
                TreeNode tn = new TreeNode(locations.Name, locations.ID.ToString());
                ParentMenuItem.ChildNodes.Add(tn);
                if (pParentOfSelectedObjectType == locations.enmLocationType)
                {
                    tn.SelectAction = TreeNodeSelectAction.Select;   
                }
                else
                {
                    tn.SelectAction = TreeNodeSelectAction.Expand;
                    BindTreeView(ref tn, pParentOfSelectedObjectType, locations.enmLocationType, locations.ChildLocations);
                }
               
                  tvParents.ExpandAll();             
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void tvParents_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            txtParent.Text = tvParents.SelectedNode.Text;
            hfParentId.Value = tvParents.SelectedNode.Value;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}