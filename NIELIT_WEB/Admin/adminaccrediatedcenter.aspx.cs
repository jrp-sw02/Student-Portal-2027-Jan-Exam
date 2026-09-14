using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
public partial class Admin_adminaccrediatedcenter : BasePage
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
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    BindState();
					//Added 13 feb 2019
                    BindCityType();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillFilter();
                    //BindCity();
					
                    //Added 13 feb 2019
                    BindCityType();
                    BindState();
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Accredited Centre", "Admin/adminaccrediatedcenter.aspx", ""));
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
	
	 //Added 13 feb 2019
    public void BindCityType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                ListItem lst = new ListItem("--Select One--", "0");
                var cityType = from p in context.cityTypeMas
                               orderby (p.ID)
                               select new { ValueField = p.ID, TextField = p.description };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCityType, cityType, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
	
    public void BindState()
    {
        using (var context = new EConnectContext())
        {
            ListItem lst = new ListItem("--Select One--", "0");
            var state = from s in context.Locations
                        where s.LocationTypeID == 2
                        select new { ValueField = s.ID, TextField = s.Name };
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlstate, state, lst);

        }
    }
    public void BindDistrict(int id)
    {
        using (var context = new EConnectContext())
        {
            ListItem lst = new ListItem("--Select All--", "0");
            if (id != null || id != 0)
            {
                var district = from s in context.Locations
                               where s.LocationTypeID == 4 && s.ParentLocationID == id
                               select new { ValueField = s.ID, TextField = s.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddldistrict, district, lst);
            }

        }
    }
    protected void FillFilter()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                var statelist = from p in context.Locations
                                where p.LocationTypeID == 2
                                select new { ValueField = p.ID, TextField = p.Name };


                //var mylist = string.Concat(statelist,CourseList);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlAccentre, statelist, lst);
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
            lblHeading.Text = "Accredited Centres";
            tblNavLinks.Visible = true;

            using (EConnectContext context = new EConnectContext())
            {
                Int32 centreId = Convert.ToInt32(Request.QueryString["Key"]);
                txtInstituteID.Text = centreId.ToString();
                txtInstituteID.Enabled = false;
                var centre = (from p in context.Institutes
                              where p.ID == centreId
                              select p).FirstOrDefault();

                txtaccentre.Text = centre.Name.ToString();
                if (centre.ContactPersonName != null)
                    txtcontactperson.Text = centre.ContactPersonName;
                if (centre.ContactPersonPost != null)
                    txtdesignation.Text = centre.ContactPersonPost;
                if (centre.MobileNumber != null)
                    txtmobile.Text = centre.MobileNumber.ToString();
                if (centre.FaxNumber != null)
                    txtfaxno.Text = centre.FaxNumber.ToString();
                if (centre.StdNumber != null)
                    txtstdno.Text = "0" + centre.StdNumber.ToString();
                if (centre.PhoneNumber1 != null)
                    txtphone1.Text = centre.PhoneNumber1.ToString();
                if (centre.PhoneNumber2 != null)
                    txtphone2.Text = centre.PhoneNumber2.ToString();
                if (centre.EmailAddress1 != null)
                    txtemail1.Text = centre.EmailAddress1.ToString();
                if (centre.EmailAddress2 != null)
                    txtemail2.Text = centre.EmailAddress2.ToString();
                if (centre.WebAddress != null)
                    txtwebaddress.Text = centre.WebAddress.ToString();
                hfAccID.Value = Request.QueryString["Key"];
                hfName.Value = Request.QueryString["Name"];

                txtadd1.Text = centre.AddressLine1;
                if (centre.AddressLine2 != null)
                    txtadd2.Text = centre.AddressLine2;
                if (centre.AddressLine3 != null)
                    txtadd3.Text = centre.AddressLine3;
                if (centre.CityName != null)
                    txtcity.Text = centre.CityName;
				
				 //Added 13 Feb 2019
                if (centre.cityTypeID != null)
                    ddlCityType.SelectedValue = centre.cityTypeID.ToString();
				
                if (centre.StateID != null)
                {
                    ddlstate.SelectedValue = centre.StateID.ToString();
                    ddlstate_SelectedIndexChanged(ddlstate, EventArgs.Empty);
                }
                if (centre.DistrictID.HasValue)
                    ddldistrict.SelectedValue = centre.DistrictID.ToString();
                if (centre.PinCode != null)
                    txtpinno.Text = centre.PinCode.ToString();


                //Updating breadscrumb
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(centre.Name, "Admin/adminaccrediatedcenter.aspx?" + Request.QueryString.ToString(), ""));
                //hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("accrediationdetails.aspx?key1=" + Request.QueryString["Key"] + "&name=" + Request.QueryString["Name"]);
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("accrediationdetails.aspx?key1=" + Request.QueryString["Key"]);
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                //Create an object of record to be modified and assign properties to relevant fields.
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    btnSave.Visible = false;
                }
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
            lblError.Visible = false;
            context = new EConnectContext();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            Int64 stateID = 0;
            if (ddlAccentre.SelectedValue != "0")
                stateID = Convert.ToInt64(ddlAccentre.SelectedValue);
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var centre = (from s in context.Institutes
                          where s.StateID != null
                          select new
                           {
                               ID = s.ID,
                               Location = s.CityName.ToUpper() + ", " + s.State.Name.ToUpper(),
                               City1 = s.CityName,
                               StateID = s.StateID,
                               Name = s.Name,
                               ContactPersonName = s.ContactPersonName,
                               //  MobileNumber = s.MobileNumber.HasValue ? s.MobileNumber : 0
                               AccreditationNumber = s.AccreditationDetails.Select(a => a.AccreditationNumber).FirstOrDefault() //s.AccreditationDetails.Select(a => a.AccreditationNumber).Aggregate((a, x) => a + ", " + x)
                           }).Distinct();
            if (!String.IsNullOrEmpty(searchString))
            {
                centre = centre.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            if (stateID != 0)
            {
                centre = centre.Where(s => s.StateID == stateID);
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "ID":
                        if (sortOrder == "DESC")
                            centre = centre.OrderByDescending(s => s.ID);
                        else
                            centre = centre.OrderBy(s => s.ID);
                        break;
                    case "Name":
                        if (sortOrder == "DESC")
                            centre = centre.OrderByDescending(s => s.Name);
                        else
                            centre = centre.OrderBy(s => s.Name);
                        break;
                    case "Location":
                        if (sortOrder == "DESC")
                            centre = centre.OrderByDescending(s => s.Location);
                        else
                            centre = centre.OrderBy(s => s.Location);
                        break;
                    case "MobileNumber":
                        if (sortOrder == "DESC")
                            centre = centre.OrderByDescending(s => s.AccreditationNumber);
                        else
                            centre = centre.OrderBy(s => s.AccreditationNumber);
                        break;

                    default:
                        centre = centre.OrderBy(s => s.ID);
                        break;
                }

            }
            if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
            {
                var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                centre = (from c in centre join r in context.AccreditationDetails
                          on c.ID equals r.InstituteID 
                          where roleCourses.Contains(r.CourseID)
                          select new
                           {
                               ID = c.ID,
                               Location = c.Location,
                               City1 = c.City1,
                               StateID = c.StateID,
                               Name = c.Name,
                               ContactPersonName = c.ContactPersonName,
                               AccreditationNumber = c.AccreditationNumber
                           }).Distinct();

                var centre1 = (from s in context.Institutes
                               where s.AccreditationDetails.Count <= 0
                               select new
                               {
                                   ID = s.ID,
                                   Location = s.CityName.ToUpper() + ", " + s.State.Name.ToUpper(),
                                   City1 = s.CityName,
                                   StateID = s.StateID,
                                   Name = s.Name,
                                   ContactPersonName = s.ContactPersonName,
                                   AccreditationNumber = s.AccreditationDetails.Select(a => a.AccreditationNumber).FirstOrDefault()
                               }).Distinct();
                centre = centre.Union(centre1);
            }
            PagingBar1.Bind(centre, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
            if (gvMain.Rows.Count <= 0)
            {
                lblError.Text = "No record found.";
                lblError.Visible = true;
            }

            if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
            {
                gvMain.Columns[4].Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
            txtInstituteID.Enabled = true;
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Accredited Centre";
            //Updating Breadcrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Accredited Centre", "#", ""));
        }
        else
        {
            txtInstituteID.Enabled = false;
            Response.Redirect("adminaccrediatedcenter.aspx", true);
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
            using (EConnectContext context = new EConnectContext())
            {
                Institute objCentre;
                Int64 instituteid = Convert.ToInt64(txtInstituteID.Text);
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    if (!context.Institutes.Any(s => s.ID == instituteid))
                    {
                        objCentre = new Institute();
                        objCentre.ID = Convert.ToInt64(txtInstituteID.Text);
                        objCentre.Name = txtaccentre.Text.ToUpper();
                        objCentre.ContactPersonName = txtcontactperson.Text.ToUpper();
                        objCentre.ContactPersonPost = txtdesignation.Text.ToUpper();
                        objCentre.MobileNumber = Convert.ToInt64(txtmobile.Text);
                        if (txtfaxno.Text.Trim() != "")
                            objCentre.FaxNumber = Convert.ToInt32(txtfaxno.Text);
                        else
                            objCentre.FaxNumber = null;
                        if (txtstdno.Text.Trim() != "")
                            objCentre.StdNumber = Convert.ToInt32(txtstdno.Text);
                        else
                            objCentre.StdNumber = null;
                        if (txtphone1.Text.Trim() != "")
                            objCentre.PhoneNumber1 = Convert.ToInt32(txtphone1.Text);
                        else
                            objCentre.PhoneNumber1 = null;
                        if (txtphone2.Text.Trim() != "")
                            objCentre.PhoneNumber2 = Convert.ToInt32(txtphone2.Text);
                        else
                            objCentre.PhoneNumber2 = null;
                        if (txtemail1.Text.Trim() != "")
                            objCentre.EmailAddress1 = txtemail1.Text.ToString();
                        else
                            objCentre.EmailAddress1 = null;
                        if (txtemail2.Text.Trim() != "")
                            objCentre.EmailAddress2 = txtemail2.Text.ToString();
                        else
                            objCentre.EmailAddress2 = null;
                        if (txtwebaddress.Text.Trim() != "")
                            objCentre.WebAddress = txtwebaddress.Text.ToString();
                        else
                            objCentre.WebAddress = null;
                        objCentre.WebAddress = txtwebaddress.Text;

                        objCentre.AddressLine1 = txtadd1.Text;
                        objCentre.AddressLine2 = txtadd2.Text;
                        if (txtadd3.Text.Trim() != "")
                            objCentre.AddressLine3 = txtadd3.Text.ToString();
                        else
                            objCentre.AddressLine3 = null;
                        if (txtcity.Text.Trim() != "")
                            objCentre.CityName = txtcity.Text.ToString();
                        else
                            objCentre.CityName = null;
						
						 //Added 13 Feb 2019
                        objCentre.cityTypeID = Convert.ToInt32(ddlCityType.SelectedValue);
                        objCentre.StateID = Convert.ToInt64(ddlstate.SelectedValue);
                        objCentre.DistrictID = Convert.ToInt64(ddldistrict.SelectedValue);
                        objCentre.PinCode = Convert.ToInt32(txtpinno.Text);
                        context.Institutes.Add(objCentre);
                        context.SaveChanges();
                        strMessage = "New record saved.";
                    }
                    else
                    {
                        ShowAlert("Institute Already exists with this Instiute-ID:-" + Convert.ToInt64(txtInstituteID.Text));
                        return;
                    }
                }
                else
                {
                    objCentre = context.Institutes.Find(Convert.ToInt32(Request.QueryString["key"]));

                    objCentre.Name = txtaccentre.Text.ToUpper();
                    objCentre.ContactPersonName = txtcontactperson.Text.ToUpper();
                    objCentre.ContactPersonPost = txtdesignation.Text.ToUpper();
                    objCentre.MobileNumber = Convert.ToInt64(txtmobile.Text);
                    if (txtfaxno.Text.Trim() != "")
                        objCentre.FaxNumber = Convert.ToInt32(txtfaxno.Text);
                    else
                        objCentre.FaxNumber = null;
                    if (txtstdno.Text.Trim() != "")
                        objCentre.StdNumber = Convert.ToInt32(txtstdno.Text);
                    else
                        objCentre.StdNumber = null;
                    if (txtphone1.Text.Trim() != "")
                        objCentre.PhoneNumber1 = Convert.ToInt32(txtphone1.Text);
                    else
                        objCentre.PhoneNumber1 = null;
                    if (txtphone2.Text.Trim() != "")
                        objCentre.PhoneNumber2 = Convert.ToInt32(txtphone2.Text);
                    else
                        objCentre.PhoneNumber2 = null;
                    if (txtemail1.Text.Trim() != "")
                        objCentre.EmailAddress1 = txtemail1.Text.ToString();
                    else
                        objCentre.EmailAddress1 = null;
                    if (txtemail2.Text.Trim() != "")
                        objCentre.EmailAddress2 = txtemail2.Text.ToString();
                    else
                        objCentre.EmailAddress2 = null;
                    if (txtwebaddress.Text.Trim() != "")
                        objCentre.WebAddress = txtwebaddress.Text.ToString();
                    else
                        objCentre.WebAddress = null;
                    objCentre.AddressLine1 = txtadd1.Text;
                    objCentre.AddressLine2 = txtadd2.Text;
                    if (txtadd3.Text.Trim() != "")
                        objCentre.AddressLine3 = txtadd3.Text.ToString();
                    else
                        objCentre.AddressLine3 = null;
                    if (txtcity.Text.Trim() != "")
                        objCentre.CityName = txtcity.Text.ToString();
                    else
                        objCentre.CityName = null;
					
					 //Added 13 Feb 2019
                        objCentre.cityTypeID = Convert.ToInt32(ddlCityType.SelectedValue);
                    objCentre.StateID = Convert.ToInt64(ddlstate.SelectedValue);
                    objCentre.DistrictID = Convert.ToInt64(ddldistrict.SelectedValue);
                    objCentre.PinCode = Convert.ToInt32(txtpinno.Text);
                    strMessage = "Record updated.";
                    context.SaveChanges();
                }
            };
            //Call save method
            //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
            //Redirect it to list mode
            Response.Redirect("adminaccrediatedcenter.aspx?msg=" + strMessage);

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
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
            ddlAccentre.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
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
            using (EConnectContext context = new EConnectContext())
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to delete the records.", true);
                    return;
                }
                Institute institute = context.Institutes.Find(Convert.ToInt32(hfActionID.Value));
                context.Institutes.Remove(institute);
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
            ShowAlert("Record can not be delted!");
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
                hl1.NavigateUrl = hl.NavigateUrl;

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
            var centre = from s in context.Institutes
                         select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                centre = centre.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            centre = centre.OrderBy(s => s.Name).Distinct();
            foreach (var course in centre)
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
        Response.Redirect("adminaccrediatedcenter.aspx", true);
    }
    protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id1 = Convert.ToInt32(ddlstate.SelectedValue);
        ddldistrict.Items.Clear();
        BindDistrict(id1);
    }

}