using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;

public partial class Admin_AdminExamHistory : BasePage
{
    String strMessage = string.Empty;
   
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
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());

                if (!String.IsNullOrEmpty(Request.QueryString["exam"]))
                {
                    mltvTab.ActiveViewIndex = 0;                   
                    btnMode.Visible = false;
                    pnlFilter.Visible = false;
                    ucSearchBar.Visible = false;
                    btnMode.ViewMode = ToggleView.Mode.List;
                    
                    if (string.IsNullOrWhiteSpace(Convert.ToString(Session["level"])))
                    {
                        licourse.Visible = false;
                        liLevel.Visible = false;
                        spn1.InnerText = "Exam Detail Of " +  Request.QueryString["lvl"].ToString();
                    }
                    else
                    {
                        liLevel.Visible = true;
                        licourse.Visible = true;
                        Label2.Text = Convert.ToString(Session["level"]);
                        spn1.InnerText = "Exam Detail Of " + Convert.ToString(Session["level"]);
                    }
                }
                else
                {                    
                    btnMode.Visible = false;                    
                }



            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ShowEditMode()
    {

        //try
        //{
        //    context = new EConnectContext();
        //    btnMode.ViewMode = ToggleView.Mode.List;
        //    mltvTab.ActiveViewIndex = 1;
        //    pnlFilter.Visible = false;
        //    ucSearchBar.Visible = false;
        //    lblHeading.Text = "Courses Details";
        //    //Get last modified date of current record and save it in ViewState object.
        //    ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
        //    User objUser = context.Users.Find(Convert.ToInt32(Request.QueryString["Key"]));
        //    txtDept.Text = objUser.Organization.Name;
        //    hfDeptId.Value = objUser.OrganizationID.ToString() + "$" + txtDept.Text;
        //    EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlUserType, typeof(EConnect.URM.UserType), new ListItem("--Select One--", "0"));
        //    ddlUserType.SelectedValue = ((Int32)objUser.UserType).ToString();
        //    ddlUserType_SelectedIndexChanged(ddlUserType, EventArgs.Empty);
        //    ddlEntity.SelectedValue = objUser.UserRefNumber.ToString();
        //    txtUserId.Text = objUser.LoginID.ToString().ToLower();
        //    txtUserName.Text = objUser.UserName;
        //    txtEmail.Text = objUser.EmailID;
        //    ddlExpiryDays.SelectedValue = objUser.PasswordExpiryDays.ToString();
        //    ddlStatus.SelectedValue = Convert.ToInt16(objUser.HasLoginAccess).ToString();
        //    txtDept.ReadOnly = true;
        //    ddlUserType.Enabled = false;
        //    ddlEntity.Enabled = false;
        //    txtUserId.ReadOnly = true;
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
        //finally
        //{
        //    context.Dispose();
        //}
    }
    protected void BindGridView()
    {
        //try
        //{
        //context = new EConnectContext();
        //int userType = 0;
        //if (ddlSearchUserType.SelectedValue != "0")
        //    userType = Convert.ToInt32(ddlSearchUserType.SelectedValue);
        //string searchString = ucSearchBar.SearchText.Trim().ToUpper();
        //string sortOrder = ViewState["SortOrder"].ToString();
        //string sortField = ViewState["SortField"].ToString();
        //var users = from s in context.Users
        //            select s;
        //if (!String.IsNullOrEmpty(searchString))
        //{
        //    users = users.Where(s => s.LoginID.ToUpper().Contains(searchString)
        //                           || s.UserName.ToUpper().Contains(searchString));
        //}
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
        //PagingBar1.Bind(users, ref gvMain);
        //uPnlGrid.Update();
        //uPnlNavigation.Update();
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
        //finally
        //{
        //    context.Dispose();
        //}
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        //try
        //{
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //    BindGridView();
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;            
        }
        else
        {
            Response.Redirect("AdminExamHistory.aspx?key1=" + Request.QueryString["key1"].ToString() + "&exam=" + Request.QueryString["exam"].ToString() + "&level=" + Request.QueryString["level"].ToString(), true);
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
        //try
        //{
        //    context = new EConnectContext();
        //    User objUser;
        //    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
        //    {
        //        objUser = new EConnect.URM.User();
        //        objUser.CreatedBy = Convert.ToInt32(Session["UserID"]);
        //        objUser.CreatedOn = DateTime.Now;
        //        objUser.EmailID = txtEmail.Text;
        //        objUser.HasLoginAccess = Convert.ToBoolean(Convert.ToInt16(ddlStatus.SelectedValue));
        //        objUser.LoginID = txtUserId.Text.ToUpper();
        //        objUser.OrganizationID = Convert.ToInt32(hfDeptId.Value.Split('$')[0]);
        //        objUser.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtUserId.Text.ToLower(), System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
        //        objUser.DefaultModuleID = 5;
        //        objUser.PasswordExpiryDays = Convert.ToInt32(ddlExpiryDays.SelectedValue);
        //        objUser.LastPasswordChangedOn = DateTime.Now;
        //        objUser.FailedLoginAttempts = 0;
        //        objUser.UserName = txtUserName.Text.ToUpper();
        //        objUser.UserTypeID = Convert.ToInt32(ddlUserType.SelectedValue);
        //        objUser.UserRefNumber = Convert.ToInt32(ddlEntity.SelectedValue);

        //        context.Users.Add(objUser);
        //        context.SaveChanges();
        //        strMessage = "New record saved.";
        //    }
        //    else
        //    {
        //        ////Initialize current object by loading it and get its current modified date
        //        //DateTime modifiedOn = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
        //        ////Caompare last modified date saved in viewstate object with current modified date.
        //        //if (Convert.ToDateTime(ViewState["LastModifiedOn"]) == modifiedOn)
        //        //{
        //        //Set modifiedBy and ModifiedOn property of current record
        //        //objMenuObject.ModifiedBy = Convert.ToInt64(Session["UserNo"]);
        //        //objMenuObject.ModifiedOn = DateTime.Now;

        //        objUser = context.Users.Find(Convert.ToInt32(Request.QueryString["key"]));
        //        objUser.EmailID = txtEmail.Text;
        //        objUser.HasLoginAccess = Convert.ToBoolean(Convert.ToInt16(ddlStatus.SelectedValue));
        //        objUser.PasswordExpiryDays = Convert.ToInt32(ddlExpiryDays.SelectedValue);
        //        objUser.UserName = txtUserName.Text.ToUpper();
        //        strMessage = "Record updated.";
        //        context.SaveChanges();
        //        //}
        //        //else
        //        //    throw new Exception("Record changed by some other person. Please reopen the record in edit mode and update again.");

        //    }

        //    //Call save method
        //    //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
        //    //Redirect it to list mode
        //    Response.Redirect("AdminCourses.aspx?msg=" + strMessage);
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
        //finally { context.Dispose(); }

    }
    protected void AllyFilter(object sender, EventArgs e)
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
    protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    context = new EConnectContext();
        //    if (hfDeptId.Value == "" && ddlUserType.SelectedValue != "0")
        //    {
        //        ddlUserType.SelectedValue = "0";
        //        UpdatePanel3.Update();
        //        throw new Exception("Please select department");
        //    }
        //    String[] dept = hfDeptId.Value.Split('$');
        //    ddlEntity.Items.Clear();
        //    ListItem lst = new ListItem("--Select One--", "0");
        //    if ((EConnect.URM.UserType)Convert.ToInt32(ddlUserType.SelectedValue) == EConnect.URM.UserType.InternalUser)
        //    {
        //        lblUserNameCaption.Text = "Employee Name <b class='mandatory'>*</b>";
        //        var empList = from p in context.Employees
        //                      orderby (p.FirstName)
        //                      select new { ValueField = p.ID, TextField = p.FirstName + " " + p.MiddleName + " " + p.LastName };
        //        EConnect.Utils.Common.ControlUtility.BindListObject(ddlEntity, empList, lst);
        //    }
        //    else if ((EConnect.URM.UserType)Convert.ToInt16(ddlUserType.SelectedValue) == EConnect.URM.UserType.ExternalUser)
        //    {
        //        lblUserNameCaption.Text = "External Entity Name <b class='mandatory'>*</b>";
        //        var empList = from p in context.ExternalEntities
        //                      orderby p.Name
        //                      select new { ValueField = p.ID, TextField = p.Name };
        //        EConnect.Utils.Common.ControlUtility.BindListObject(ddlEntity, empList, lst);
        //    }
        //    else
        //    {
        //        lblUserNameCaption.Text = "Employee Name";
        //        ddlEntity.Items.Add(lst);
        //    }
        //    ddlEntity.Focus();
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
        //finally { context.Dispose(); }
    }
    protected void hfDeptId_ValueChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    ddlEntity.Items.Clear();
        //    ddlEntity.Items.Add(new ListItem("--Select One--", "0"));
        //    ddlStatus.SelectedValue = "1";
        //    ddlExpiryDays.SelectedValue = "10";
        //    if (hfDeptId.Value == "")
        //    {
        //        ddlUserType.Items.Clear();
        //        ddlUserType.Items.Add(new ListItem("--Select One--", "0"));
        //    }
        //    else
        //    {
        //        EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlUserType, typeof(EConnect.URM.UserType), new ListItem("--Select One--", "0"));
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }
    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        //try
        //{
        //    ddlSearchUserType.SelectedValue = "0";
        //    PagingBar1.CurrentPageIndex = 0;
        //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
    }
    protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
    {
        //try
        //{
        //    ViewState["SortField"] = e.SortExpression;
        //    if (ViewState["SortOrder"].ToString() == "DESC")
        //        ViewState["SortOrder"] = "ASC";
        //    else
        //        ViewState["SortOrder"] = "DESC";
        //    BindGridView();
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
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
        //        if (btnAction.CommandName == "Reset")
        //        {
        //            //Load the object and apply validateion if required
        //            //call delete function
        //            //bind the grid again
        //            User selectedUser = context.Users.Find(Convert.ToInt32(hfActionID.Value));
        //            selectedUser.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(selectedUser.LoginID.ToLower(), System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
        //            selectedUser.LastPasswordChangedOn = DateTime.Now;
        //            context.Entry(selectedUser).State = System.Data.Entity.EntityState.Modified;
        //            context.SaveChanges();
        //            BindGridView();
        //            ShowAlert("Password reset successfully.", true);
        //            hfActionID.Value = "";
        //        }
        //        else if (btnAction.CommandName == "ChangeStatus")
        //        {
        //            //Load the object and apply validateion if required
        //            //call function to perform required action
        //            //bind the grid again
        //            User selectedUser = context.Users.Find(Convert.ToInt32(hfActionID.Value));
        //            if (selectedUser.HasLoginAccess == true)
        //                selectedUser.HasLoginAccess = false;
        //            else
        //                selectedUser.HasLoginAccess = true;
        //            context.Entry(selectedUser).State = System.Data.Entity.EntityState.Modified;
        //            context.SaveChanges();
        //            BindGridView();
        //            ShowAlert("Login status changed successfully successfully.", true);
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
        Response.Redirect("AdminExamHistory.aspx?key1=" + Request.QueryString["key1"].ToString() + "&exam=" + Request.QueryString["exam"].ToString() + "&level=" + Request.QueryString["level"].ToString(), true);
    }
}