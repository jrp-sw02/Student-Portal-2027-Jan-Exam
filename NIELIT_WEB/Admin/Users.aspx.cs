using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Script.Serialization;
using EConnect.URM;
using EConnect.DAL;
using System.Web.Security;
using EConnect.Utils.Common;
using AjaxControlToolkit;
using System.Transactions;
using EConnect.NIELIT;
public partial class Admin_Users : BasePage
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
                fillusertype();
                BindCoursesForMapping();
                //EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlUserType, typeof(EConnect.URM.UserType), new ListItem("--Select One--", "0"));
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    ShowEditMode();
                }
                else
                {
                    BreadCrumb1.RemoveLastBreadCrumbItem();

                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    EnumUtility.BindListObject(ref ddlSearchUserType, typeof(EConnect.URM.UserType), new ListItem("--Select One--", "0"));
                    if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                    {
                        ddlSearchUserType.SelectedValue = "4";
                        ddlSearchUserType.Enabled = false;
                    }
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "Please Select Filter Criteria For View Users Records";
                        lblError.Visible = true;
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["UserType"]))
                    {
                        ddlSearchUserType.SelectedValue = Request.QueryString["UserType"].ToString();
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["status"]))
                    {
                        ddlSearchLoginStatus.SelectedValue = Request.QueryString["status"].ToString();
                    }
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Users", "Admin/Users.aspx?UserType=" + ddlSearchUserType.SelectedValue + "&status=" + ddlSearchLoginStatus.SelectedValue, ""));
                    if (ddlSearchUserType.SelectedValue != "0" || ddlSearchLoginStatus.SelectedValue != "0")
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
    protected void fillusertype()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                Int32 candidatetype = Convert.ToInt32(UserType.Candidate);
                Int32 institutetype = Convert.ToInt32(UserType.Institute);
                ListItem lst = new ListItem("--Select One--", "0");
                var roles = from s in context.UserTypes
                            where s.ID != candidatetype
                            orderby s.Name
                            select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlUserType, roles, lst);

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ShowEditMode()
    {
        try
        {
            //deep add on 23 may 2018
            txtMobileNumberRegCentre.Visible = false;
            txtEmailRegCentre.Visible = false;
            Label8.Visible = false;
            Label9.Visible = false;
            // deep end on 23 may 2018
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "User Details";
            tblNavLinks.Visible = true;
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            using (EConnectContext context = new EConnectContext())
            {
                User objUser = UserManager.GetUserByUserID(Convert.ToInt32(Request.QueryString["Key"]), context);
                if (objUser != null)
                {
                    EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlUserType, typeof(EConnect.URM.UserType), new ListItem("--Select One--", "0"));
                    ddlUserType.SelectedValue = objUser.UserTypeID.ToString();
                    //ddlUserType_SelectedIndexChanged(ddlUserType, EventArgs.Empty);
                    // ddlEntity.SelectedValue = objUser.UserRefNumber.ToString();
                    // ddlEntity_SelectedIndexChanged(ddlEntity, EventArgs.Empty);
                    Bindrole(objUser.UserTypeID);
                    hfEntityID.Value = objUser.UserRefNumber.ToString();
                    UserType userType = (EConnect.URM.UserType)Convert.ToInt32(ddlUserType.SelectedValue);
                    if (userType == EConnect.URM.UserType.Admin || userType == EConnect.URM.UserType.ExternalAdmin || userType == EConnect.URM.UserType.HeadOffice)
                    {
                        var externalUser = context.ExternalEntities.Find(objUser.UserRefNumber);
                        txtEntity.Text = externalUser.Name.ToLower();
                        txtEmail.Text = externalUser.Email.ToString().ToLower();
                        txtMobileNumber.Text = externalUser.MobileNumber.ToString();
                    }
                    else if (userType == EConnect.URM.UserType.Candidate)
                    {
                        var candidate = context.Candidates.Find(objUser.UserRefNumber);
                        txtEntity.Text = candidate.Name + (candidate.FatherName == null ? "" : " S/o " + candidate.FatherName);
                        EConnect.NIELIT.CandidateContactDetail contact = candidate.ContactDetails.FirstOrDefault();
                        txtEmail.Text = contact.EmailAddress.ToString().ToLower();
                        txtMobileNumber.Text = contact.MobileNumber.HasValue ? contact.MobileNumber.Value.ToString() : "";
                    }
                    else if (userType == EConnect.URM.UserType.Institute)
                    {
                        var Institute = context.Institutes.Find(objUser.UserRefNumber);
                        txtEntity.Text = (Institute.Name + ", " + Institute.CityName + ", " + Institute.State.Name).ToLower();
                        if (Institute.EmailAddress1 != null)
                            txtEmail.Text = Institute.EmailAddress1.ToString().ToLower();
                        if (Institute.MobileNumber.HasValue)
                            txtMobileNumber.Text = Institute.MobileNumber.Value.ToString();
                    }
                    else if (userType == EConnect.URM.UserType.RegionalCenter)
                    {
                        var RegionalCentre = context.RegionalCenters.Find(objUser.UserRefNumber);
                        txtEntity.Text = RegionalCentre.Name.ToLower();
                        if (RegionalCentre.RegisteredEmailAddress != null)
                            txtEmail.Text = RegionalCentre.RegisteredEmailAddress.ToString().ToLower();
                        if (RegionalCentre.RegisteredMobileNumber.HasValue)
                            txtMobileNumber.Text = RegionalCentre.RegisteredMobileNumber.Value.ToString();
                        //deep add on 23 may 2018
                        if (objUser.UserTypeID == 5)
                        {
                            txtMobileNumberRegCentre.Visible = true;
                            txtEmailRegCentre.Visible = true;
                            Label8.Visible = true;
                            Label9.Visible = true;
                            txtMobileNumberRegCentre.Text = "0";
                            var RegionalCentreUser = context.Users.Find(objUser.UserID);
                            if (RegionalCentreUser.EmailID != null)
                                txtEmailRegCentre.Text = RegionalCentreUser.EmailID.ToString().ToLower();

                            txtMobileNumberRegCentre.Text = RegionalCentreUser.MobileNumber.ToString();

                        }
                        //deep  end code on 23 may 2018
                    }
					  else if (userType == EConnect.URM.UserType.projectNIELITCentre)
                        {
                            NIELITMISContext context1 = new NIELITMISContext();
                            var NIELITCentre = context1.NielitCentres.Find(objUser.UserRefNumber);
                            txtEntity.Text = NIELITCentre.Name.ToLower();
                            if (NIELITCentre.email1 != null)
                                txtEmail.Text = NIELITCentre.email1.ToString().ToLower();
                            if (NIELITCentre.mobile != null)
                                txtMobileNumber.Text = NIELITCentre.mobile.ToString();

                            txtMobileNumberRegCentre.Visible = true;
                            txtEmailRegCentre.Visible = true;
                            Label8.Visible = true;
                            Label9.Visible = true;
                            txtMobileNumberRegCentre.Text = objUser.MobileNumber.ToString ();
                            txtEmailRegCentre.Text = objUser.EmailID;
                        }

                    txtEntity.Enabled = false;
                    txtUserId.Text = objUser.LoginID.ToString().ToLower();
                    txtUserName.Text = objUser.UserName;
                    ddlExpiryDays.SelectedValue = objUser.PasswordExpiryDays.ToString();
                    ddlStatus.SelectedValue = Convert.ToInt16(objUser.HasLoginAccess).ToString();
                    ddlUserType.Enabled = false;
                    Ddlrolename.SelectedValue = Convert.ToInt32(objUser.DefaultRoleID).ToString();
                    //ddlEntity.Enabled = false;
                    txtUserId.ReadOnly = true;
                    //rac.InnerText = objUser.LoginID.ToString().ToLower();
                    hfLoginID.Value = Request.QueryString["Key"];
                    hfName.Value = objUser.LoginID.ToString().ToLower();
                    // liEdit.Visible = true;

                    //HO_Course_Mapping
                    if ((Convert.ToInt32(Ddlrolename.SelectedValue) == Convert.ToInt32(enmRole.CourseWiseHeadOffice)) || (Convert.ToInt32(Ddlrolename.SelectedValue) == Convert.ToInt32(enmRole.CourseWiseAdmin)))
                    {
                        tblShow.Visible = true;

                        var filldata = (from f in context.CourseWiseUserMappings
                                        where f.UserNo == objUser.UserID
                                        select new { CourseID = f.CourseID }).ToList();

                        if (filldata.Count > 0)
                        {
                            foreach (var s in filldata)
                            {
                                foreach (ListItem sli in chkcourselist.Items)
                                {
                                    if (s.CourseID == Convert.ToInt32(sli.Value))
                                    {
                                        sli.Selected = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        tblShow.Visible = false;
                    }

                    
                }
                string url = "Admin/Users.aspx?" + Request.QueryString.ToString();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(objUser.UserName, url, ""));
                hlUserLog.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("UserLog.aspx?UID=" + objUser.UserID.ToString());
                hlUserRole.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("UserRole.aspx?UID=" + objUser.UserID.ToString());
            };
            if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            {
                btnSave.Visible = false;
                truserRole.Visible = false;
            }
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
            BreadCrumb1.Render();
            context = new EConnectContext();
            lblError.Visible = false;
            int userType = 0;
            string LoginStatus = "";
            if (ddlSearchLoginStatus.SelectedValue != "0")
                LoginStatus = Convert.ToString(ddlSearchLoginStatus.SelectedItem.Text);
            if (ddlSearchUserType.SelectedValue != "0")
                userType = Convert.ToInt32(ddlSearchUserType.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            if (userType != 0)
            {
                var users = from s in context.Users
                            select new
                            {
                                HasLoginAccess = s.HasLoginAccess ? "Enabled" : "Disabled",
                                LoginID = s.LoginID,
                                UserTypeID = s.UserTypeID,
                                UserName = s.UserName,
                                LastPasswordChangedOn = s.LastPasswordChangedOn,
                                UserID = s.UserID,
                                OrganizationID = s.OrganizationID,
                                //enmUserType=EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.URM.UserType)s.UserTypeID)
                                enmUserType = s.UserType.Name,
                                LastLoginDateTime = s.LastLoginDateTime
                            };
                if (!String.IsNullOrEmpty(searchString))
                {
                    users = users.Where(s => s.LoginID.ToUpper().Contains(searchString)
                                           || s.UserName.ToUpper().Contains(searchString));
                }
                if (userType != 0)
                    users = users.Where(s => s.UserTypeID == userType);
                users = users.OrderBy(s => s.UserName);
                if (LoginStatus.ToString() != "")
                {
                    users = users.Where(s => s.HasLoginAccess == LoginStatus);
                }
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "LoginID":
                            if (sortOrder == "DESC")
                                users = users.OrderByDescending(s => s.LoginID);
                            else
                                users = users.OrderBy(s => s.LoginID);
                            break;
                        case "UserName":
                            if (sortOrder == "DESC")
                                users = users.OrderByDescending(s => s.UserName);
                            else
                                users = users.OrderBy(s => s.UserName);
                            break;
                        case "UserType":
                            if (sortOrder == "DESC")
                                users = users.OrderByDescending(s => s.UserTypeID);
                            else
                                users = users.OrderBy(s => s.UserTypeID);
                            break;
                        case "LastLoginDateTime":
                            if (sortOrder == "DESC")
                                users = users.OrderByDescending(s => s.LastLoginDateTime);
                            else
                                users = users.OrderBy(s => s.LastLoginDateTime);
                            break;
                        case "HasLoginAccess":
                            if (sortOrder == "DESC")
                                users = users.OrderByDescending(s => s.HasLoginAccess);
                            else
                                users = users.OrderBy(s => s.HasLoginAccess);
                            break;
                        default:
                            users = users.OrderBy(s => s.LoginID);
                            break;
                    }
                }
                PagingBar1.Bind(users, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();

                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    gvMain.Columns[6].Visible = false;
                }

                if (gvMain.Rows.Count <= 0)
                {
                    lblError.Text = "No record found.";
                    lblError.Visible = true;
                }
            }
            else
            {
                Response.Redirect("Users.aspx", true);
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
            lblHeading.Text = "New User";
            //Updating Breadscrumbh
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New User", "#", ""));
        }
        else
        {
            string url = "Users.aspx?";
            if (!String.IsNullOrEmpty(Request.QueryString["UserType"]))
            {
                url += "UserType=" + Request.QueryString["UserType"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.QueryString["status"]))
            {
                if (url.Contains("?"))
                    url += "&status=" + Request.QueryString["status"].ToString();
                else
                    url += "?status=" + Request.QueryString["status"].ToString();
            }
            Response.Redirect(url, true);
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
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    User objUser;
                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        if (context.Users.Any(u => u.LoginID == txtUserId.Text.ToUpper()) == true)
                            throw new Exception("Given UserID is alreay exist. Please give another UserID");
                        objUser = new EConnect.URM.User();
                        objUser.CreatedBy = Convert.ToInt32(Session["UserID"]);
                        objUser.CreatedOn = DateTime.Now;
                        objUser.HasLoginAccess = Convert.ToBoolean(Convert.ToInt16(ddlStatus.SelectedValue));
                        objUser.LoginID = txtUserId.Text.ToUpper();
                        objUser.OrganizationID = Convert.ToInt32(Session["OrgID"]);
                        string password = EConnect.Utils.Security.RandomPassword.Generate(6, 8);
                        objUser.Password = UserManager.ComputeSha256Hash(password).ToUpper();  //FormsAuthentication.HashPasswordForStoringInConfigFile(password, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
                        objUser.PasswordExpiryDays = Convert.ToInt32(ddlExpiryDays.SelectedValue);
                        objUser.LastPasswordChangedOn = DateTime.Now;
                        objUser.FailedLoginAttempts = 0;
                        objUser.UserName = txtUserName.Text.ToUpper();
                        //objUser.EmailID = txtEmail.Text;
                        //deep add this line of code on 22 may 2018
                        Int32 userType = Convert.ToInt32(ddlUserType.SelectedValue);
                        if (userType  == 5)
                        {
                            objUser.EmailID = txtEmailRegCentre.Text;
                            objUser.MobileNumber = Convert.ToInt64(txtMobileNumberRegCentre.Text);
                        }

                        //deep end code this line of code on 22 may 2018
						//Added 26 June 2020
                        if (userType == 10)
                        {
                            objUser.EmailID = txtEmail.Text;
                            objUser.MobileNumber = Convert.ToInt64(txtMobileNumber.Text);
                        }
                      //
                        objUser.UserTypeID = Convert.ToInt32(ddlUserType.SelectedValue);
                        objUser.UserRefNumber = Convert.ToInt64(hfEntityID.Value);
                        objUser.DefaultRoleID = Convert.ToInt32(Ddlrolename.SelectedValue);
                        context.Users.Add(objUser);
                        context.SaveChanges();
                        //User Roles
                        EConnect.URM.UserRole userRole = new EConnect.URM.UserRole();
                        userRole.UserID = objUser.UserID;
                        userRole.RoleID = Convert.ToInt32(Ddlrolename.SelectedValue);
                        userRole.CreatedOn = DateTime.Now;
                        userRole.CreatedBy = Convert.ToInt32(Session["UserID"]);
                        context.UserRoles.Add(userRole);
                        context.SaveChanges();

                        //HO_User_Course Mapping
                        CourseWiseUserMapping CourseUser;
                        foreach (ListItem l in chkcourselist.Items)
                        {
                            if (l.Selected)
                            {
                                CourseUser = new EConnect.NIELIT.CourseWiseUserMapping();
                                CourseUser.CourseID = Convert.ToInt32(l.Value);
                                CourseUser.CourseCategoryID = context.Courses.Find(Convert.ToInt32(l.Value)).CourseCategoryID;
                                CourseUser.UserNo = objUser.UserID;
                                context.CourseWiseUserMappings.Add(CourseUser);
                                context.SaveChanges();
                            }
                        }
                        

                        CommonFunctions.SendAccountActivationEmail(objUser.UserID, false, password);
                        strMessage = "New record saved.";
                    }
                    else
                    {
                        objUser = context.Users.Find(Convert.ToInt32(Request.QueryString["key"]));
                        objUser.HasLoginAccess = Convert.ToBoolean(Convert.ToInt16(ddlStatus.SelectedValue));
                        objUser.PasswordExpiryDays = Convert.ToInt32(ddlExpiryDays.SelectedValue);
                        objUser.UserName = txtUserName.Text.ToUpper();
                        //deep add this line of code on 22 may 2018
                        if (objUser.UserTypeID == 5)
                        {
                            objUser.EmailID = txtEmailRegCentre.Text;
                            objUser.MobileNumber = Convert.ToInt64(txtMobileNumberRegCentre.Text);
                        }
                        //deep end code this line of code on 22 may 2018
						 //Added 26 June 2020
                        if (objUser.UserTypeID == 10)
                        {
                            objUser.EmailID = txtEmail.Text;
                            objUser.MobileNumber = Convert.ToInt64(txtMobileNumber.Text);
                        }
                        //
                        objUser.DefaultRoleID = Convert.ToInt32(Ddlrolename.SelectedValue);
                        context.SaveChanges();
                        var userrole = context.UserRoles.Where(s => s.UserID == objUser.UserID).OrderByDescending(s => s.CreatedOn).FirstOrDefault();
                        userrole.RoleID = objUser.DefaultRoleID;
                        context.Entry(userrole).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        //HO_User_Course Mapping
                        context.Database.ExecuteSqlCommand("Delete from Course_Wise_User_Mapping Where User_No =" + objUser.UserID);
                        CourseWiseUserMapping CourseUser;
                        foreach (ListItem l in chkcourselist.Items)
                        {
                            if (l.Selected)
                            {
                                CourseUser = new EConnect.NIELIT.CourseWiseUserMapping();
                                CourseUser.CourseID = Convert.ToInt32(l.Value);
                                CourseUser.CourseCategoryID = context.Courses.Find(Convert.ToInt32(l.Value)).CourseCategoryID;
                                CourseUser.UserNo = objUser.UserID;
                                context.CourseWiseUserMappings.Add(CourseUser);
                                context.SaveChanges();
                            }
                        }
                    }
                };
                scope.Complete();
                if (!String.IsNullOrEmpty(Request.QueryString["UserType"]) && !String.IsNullOrEmpty(Request.QueryString["status"]))
                    Response.Redirect("Users.aspx?UserType=" + Request.QueryString["UserType"].ToString() + "&status=" + Request.QueryString["status"].ToString(), true);
                else
                    Response.Redirect("Users.aspx", true);
            };
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
            BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Users", "Admin/Users.aspx?UserType=" + ddlSearchUserType.SelectedValue + "&status=" + ddlSearchLoginStatus.SelectedValue, ""));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    //protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ddlEntity.Items.Clear();
    //        UserType userType = (EConnect.URM.UserType)Convert.ToInt32(ddlUserType.SelectedValue);
    //        lblUserNameCaption.Text = userType.ToString() + " Name <b class='mandatory'>*</b>";
    //        ListItem lst = new ListItem("--Select One--", "0");
    //        using(EConnectContext  context = new EConnectContext())
    //        {
    //            if (userType == EConnect.URM.UserType.Admin || userType == EConnect.URM.UserType.External || userType == EConnect.URM.UserType.HeadOffice)
    //            {
    //                lblUserNameCaption.Text = userType.ToString() + " Entity Name <b class='mandatory'>*</b>";
    //                var empList = from p in context.ExternalEntities
    //                              orderby p.Name
    //                              select new { ValueField = p.ID, TextField = p.Name };
    //                EConnect.Utils.Common.ControlUtility.BindListObject(ddlEntity, empList, lst);
    //            }
    //            else if (userType == EConnect.URM.UserType.Candidate)
    //            {
    //                var empList = from p in context.Candidates
    //                              orderby p.Name
    //                              select new { ValueField = p.ID, TextField = p.Name };
    //                EConnect.Utils.Common.ControlUtility.BindListObject(ddlEntity, empList, lst);
    //            }
    //            else if (userType == EConnect.URM.UserType.Institute)
    //            {
    //                var empList = from p in context.Institutes
    //                              orderby p.Name
    //                              select new { ValueField = p.ID, TextField = p.Name };
    //                EConnect.Utils.Common.ControlUtility.BindListObject(ddlEntity, empList, lst);
    //            }
    //            else if (userType == EConnect.URM.UserType.RegionalCenter)
    //            {
    //                var empList = from p in context.RegionalCenters
    //                              orderby p.Name
    //                              select new { ValueField = p.ID, TextField = p.Name };
    //                EConnect.Utils.Common.ControlUtility.BindListObject(ddlEntity, empList, lst);
    //            }
    //            else
    //            {
    //                lblUserNameCaption.Text = "Entity Name <b class='mandatory'>*</b>";
    //                ddlEntity.Items.Add(lst);
    //            }
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message, true);
    //    }
    //}
    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("Users.aspx");
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
            BreadCrumb1.Render();
            if (hfActionID.Value != "")
            {
                String recordID = hfActionID.Value.Split('$')[0].ToString();
                LinkButton btnAction = (LinkButton)sender;
                if (btnAction.CommandName == "Reset")
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        User selectedUser = UserManager.GetUserByUserID(Convert.ToInt32(hfActionID.Value), context);
                        string resetpassword = EConnect.Utils.Security.RandomPassword.Generate(6, 8);
                        selectedUser.Password = UserManager.ComputeSha256Hash(resetpassword).ToUpper(); //FormsAuthentication.HashPasswordForStoringInConfigFile(resetpassword, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
                        selectedUser.LastPasswordChangedOn = DateTime.Now;
                        selectedUser.FailedLoginAttempts = 0;
                        context.Entry(selectedUser).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        string emailAddress = "";
                        if (selectedUser.enmUserType == EConnect.URM.UserType.Candidate)
                        {
                            emailAddress = (from c in context.CandidateContactDetails
                                            where c.CandidateID == selectedUser.UserRefNumber
                                            select new
                                            {
                                                email = c.EmailAddress
                                            }).FirstOrDefault().email;
                        }
                        else if (selectedUser.enmUserType == EConnect.URM.UserType.Admin || selectedUser.enmUserType == EConnect.URM.UserType.ExternalAdmin || selectedUser.enmUserType == EConnect.URM.UserType.HeadOffice)
                        {
                            emailAddress = (from c in context.ExternalEntities
                                            where c.ID == selectedUser.UserRefNumber
                                            select new
                                            {
                                                email = c.Email
                                            }).FirstOrDefault().email;
                        }
                        else if (selectedUser.enmUserType == EConnect.URM.UserType.Institute)
                        {
                            emailAddress = (from c in context.Institutes
                                            where c.ID == selectedUser.UserRefNumber
                                            select new
                                            {
                                                email = c.EmailAddress1
                                            }).FirstOrDefault().email;
                        }
                        else if (selectedUser.enmUserType == EConnect.URM.UserType.RegionalCenter)
                        {
                           // emailAddress = (from c in context.RegionalCenters
                                          //  where c.ID == selectedUser.UserRefNumber
                                          //  select new
                                          //  {
                                              //  email = c.RegisteredEmailAddress
                                           // }).FirstOrDefault().email;
                            //deep add on 23 may 2018
                            emailAddress = (from c in context.Users
                                            where c.UserID == selectedUser.UserID
                                            select new
                                            {
                                                email = c.EmailID
                                            }).FirstOrDefault().email;
                            //deep end on 23 may 2018
                        }
						 else if ( selectedUser.enmUserType == EConnect.URM.UserType.projectNIELITCentre|| selectedUser.enmUserType == EConnect.URM.UserType.NonAffiliatedInstitute)
                        {
                            // emailAddress = (from c in context.RegionalCenters
                            //  where c.ID == selectedUser.UserRefNumber
                            //  select new
                            //  {
                            //  email = c.RegisteredEmailAddress
                            // }).FirstOrDefault().email;
                            //deep add on 23 may 2018
                            emailAddress = (from c in context.Users
                                            where c.UserID == selectedUser.UserID
                                            select new
                                            {
                                                email = c.EmailID
                                            }).FirstOrDefault().email;
                            //deep end on 23 may 2018
                        }
                        //if (emailAddress != "")
						if (emailAddress != null)
                        {
                            string msg = "";
                            msg = "Dear " + GetInitCap(selectedUser.UserName) + ",<br/><br/>" + "You have successfully reset your password and your reset password  and login-Id to login into  Online Student Information and Enrollment System of NIELIT  are as followed :- <br/><br/>User ID  &nbsp;&nbsp;&nbsp;&nbsp;: " + selectedUser.LoginID + "<br/>Password &nbsp;:  " + resetpassword +
                              "<br/><br/> " +
                              "Thanks<br />" +
                              "NIELIT Team";
                            EConnect.NIELIT.Email mail = new EConnect.NIELIT.Email("Reset Password:NIELIT", msg, emailAddress);
                            try
                            {
                                mail.Send();
                            }
                            catch (Exception ex)
                            {
                                CommonFunctions.SendSmsToAdminOnEmailFailed("Dear Admin,<Br>A Reset Password Email could not be sent to email address " + emailAddress + ".Reason: " + ex.ToString());
                            }
                        }
                    };
                    BindGridView();
                    ShowAlert("Password reset successfully.", true);
                    hfActionID.Value = "";
                }
                else if (btnAction.CommandName == "ChangeStatus")
                {
                    //Load the object and apply validateion if required
                    //call function to perform required action
                    //bind the grid again
                    using (EConnectContext context = new EConnectContext())
                    {
                        User selectedUser = UserManager.GetUserByUserID(Convert.ToInt32(hfActionID.Value), context);
                        if (selectedUser.UserID == 1)
                        {
                            String msg = "You cannot change the login status of selected user!";
                            ShowAlert(msg, true);
                        }
                        else
                        {
                            if (selectedUser.HasLoginAccess == true)
                                selectedUser.HasLoginAccess = false;
                            else
                                selectedUser.HasLoginAccess = true;
                            context.Entry(selectedUser).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                    };
                    BindGridView();
                    ShowAlert("Login status changed successfully successfully.", true);
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
                HyperLink hl0 = (HyperLink)e.Row.Cells[1].Controls[0];
                hl0.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl0.NavigateUrl + "&UserType=" + ddlSearchUserType.SelectedValue + "&status=" + ddlSearchLoginStatus.SelectedValue);
                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = hl0.NavigateUrl;
                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = hl0.NavigateUrl;
                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = hl0.NavigateUrl;
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

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetUserType(String prefixText, Int32 count, string contextKey)
    {
        // EConnectContext context = new EConnectContext();
        //Added 30 Aug 2019
       NIELITMISContext context1 = new NIELITMISContext();
        //
        try
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string searchString = prefixText.Trim().ToUpper();
            Int32 UserTypeID = Convert.ToInt32(contextKey);
            List<String> items = new List<String>();
            UserType userType = (EConnect.URM.UserType)Convert.ToInt32(contextKey);
            using (EConnectContext context = new EConnectContext())
            {
                if (userType == EConnect.URM.UserType.Admin || userType == EConnect.URM.UserType.ExternalAdmin || userType == EConnect.URM.UserType.HeadOffice || userType == EConnect.URM.UserType.ExternalRegionalCenter)
                {
                    var Users = from s in context.ExternalEntities
                                select new { ID = s.ID, Name = s.Name };
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        Users = Users.Where(s => s.Name.ToUpper().Contains(searchString)).Distinct().Take(count);
                    }
                    Users = Users.OrderBy(s => s.Name);
                    if (Users.Count() == 0)
                    {
                        items.Add(AutoCompleteExtender.CreateAutoCompleteItem("Not match found", js.Serialize(new { ID = 0, Name = "Not match found" })));
                    }
                    else
                    {
                        foreach (var c in Users)
                        {
                            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Name, js.Serialize(c)));
                        }
                    }

                    //return items.ToArray();
                }
                else if (userType == EConnect.URM.UserType.Candidate)
                {
                    var empList = from p in context.Candidates
                                  orderby p.Name
                                  select new { ID = p.ID, Name = p.Name + (p.FatherName == null ? "" : " S/o " + p.FatherName) };
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        empList = empList.Where(s => s.Name.ToUpper().Contains(searchString)).Distinct().Take(count);
                    }
                    empList = empList.OrderBy(s => s.Name);
                    if (empList.Count() == 0)
                    {
                        items.Add(AutoCompleteExtender.CreateAutoCompleteItem("Not match found", js.Serialize(new { ID = 0, Name = "Not match found" })));
                    }
                    else
                    {
                        foreach (var c in empList)
                        {
                            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Name, js.Serialize(c)));
                        }
                    }

                    // return items.ToArray();

                }
                else if (userType == EConnect.URM.UserType.Institute)
                {
                    var empList = from p in context.Institutes
                                  orderby p.Name
                                  select new { ID = p.ID, Name = (p.Name + ", " + p.CityName + ", " + p.State.Name).ToLower() };
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        empList = empList.Where(s => s.Name.ToUpper().Contains(searchString)).Distinct().Take(count);
                    }
                    empList = empList.OrderBy(s => s.Name);
                    if (empList.Count() == 0)
                    {
                        items.Add(AutoCompleteExtender.CreateAutoCompleteItem("Not match found", js.Serialize(new { ID = 0, Name = "Not match found" })));
                    }
                    else
                    {
                        foreach (var c in empList)
                        {
                            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Name, js.Serialize(c)));
                        }
                    }
                    //return items.ToArray();

                }
                else if (userType == EConnect.URM.UserType.RegionalCenter)
                {
                    var empList = from p in context.RegionalCenters
                                  orderby p.Name
                                  select new { ID = p.ID, Name = p.Name };
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        empList = empList.Where(s => s.Name.ToUpper().Contains(searchString)).Distinct().Take(count);
                    }
                    empList = empList.OrderBy(s => s.Name);
                    if (empList.Count() == 0)
                    {
                        items.Add(AutoCompleteExtender.CreateAutoCompleteItem("Not match found", js.Serialize(new { ID = 0, Name = "Not match found" })));
                    }
                    else
                    {
                        foreach (var c in empList)
                        {
                            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Name, js.Serialize(c)));
                        }
                    }

                }
                 //Added 30-Aug-2019
                else if (userType == EConnect.URM.UserType.projectNIELITCentre)
                {
                    var centreList = from p in context1.NielitCentres
                                     orderby p.Name
                                     select new { ID = p.ID, Name = p.Name };
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        centreList = centreList.Where(s => s.Name.ToUpper().Contains(searchString)).Distinct().Take(count);
                    }
                    centreList = centreList.OrderBy(s => s.Name);
                    if (centreList.Count() == 0)
                    {
                        items.Add(AutoCompleteExtender.CreateAutoCompleteItem("Not match found", js.Serialize(new { ID = 0, Name = "Not match found" })));
                    }
                    else
                    {
                        foreach (var c in centreList)
                        {
                            items.Add(AutoCompleteExtender.CreateAutoCompleteItem(c.Name, js.Serialize(c)));
                        }
                    }
                    //
                }
                return items.ToArray();
            };

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BreadCrumb1.RemoveLastBreadCrumbItem();
        if (!String.IsNullOrEmpty(Request.QueryString["UserType"]) && !String.IsNullOrEmpty(Request.QueryString["status"]))
            Response.Redirect("Users.aspx?UserType=" + Request.QueryString["UserType"].ToString() + "&status=" + Request.QueryString["status"].ToString(), true);
        else
            Response.Redirect("Users.aspx", true);
    }
    protected void hfEntityID_ValueChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (hfEntityID.Value != "0")
            {
                UserType userType = (EConnect.URM.UserType)Convert.ToInt32(ddlUserType.SelectedValue);
                Int64 entityID = Convert.ToInt64(hfEntityID.Value);
                using (EConnectContext context = new EConnectContext())
                {
                    if (userType == EConnect.URM.UserType.Admin || userType == EConnect.URM.UserType.ExternalAdmin || userType == EConnect.URM.UserType.HeadOffice || userType == EConnect.URM.UserType.ExternalRegionalCenter || userType == EConnect.URM.UserType.ExternalInstitute || userType == EConnect.URM.UserType.ExternalHeadOffice)
                    {

                        var userDetail = (from p in context.ExternalEntities
                                          where p.ID == (int)entityID
                                          select new { UserID = p.Name.ToLower().Replace(" ", ""), UserName = p.Name.ToUpper(), MobileNumber = p.MobileNumber, EmailID = p.Email }).FirstOrDefault();

                        txtEmail.Text = userDetail.EmailID;
                        txtMobileNumber.Text = userDetail.MobileNumber.ToString();
                        if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                        {
                            txtUserId.Text = userDetail.UserID.ToString();
                            txtUserName.Text = userDetail.UserName.ToString();
                        }
                    }
                    else if (userType == EConnect.URM.UserType.Candidate)
                    {
                        var userDetail = (from p in context.Candidates
                                          where p.ID == entityID
                                          select new { UserID = p.Name.ToLower().Replace(" ", ""), UserName = p.Name.ToUpper(), MobileNumber = p.ContactDetails.FirstOrDefault().MobileNumber.HasValue ? p.ContactDetails.FirstOrDefault().MobileNumber.Value : 0, EmailID = p.ContactDetails.FirstOrDefault().EmailAddress }).FirstOrDefault();
                        if (userDetail.EmailID != null && userDetail.EmailID != "")
                        {
                            txtEmail.Enabled = false;
                            txtEmail.Text = userDetail.EmailID;
                        }
                        else
                        {
                            txtEmail.Enabled = true;
                        }
                        if (userDetail.MobileNumber != null && userDetail.MobileNumber != 0)
                        {
                            txtMobileNumber.Enabled = false;
                            txtMobileNumber.Text = userDetail.MobileNumber.ToString();
                        }
                        else
                        {
                            txtMobileNumber.Enabled = true;
                        }
                        if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                        {
                            txtUserId.Text = userDetail.UserID.ToString();
                            txtUserName.Text = userDetail.UserName.ToString();
                        }
                    }
                    else if (userType == EConnect.URM.UserType.Institute)
                    {
                        var userDetail = (from p in context.Institutes
                                          where p.ID == entityID
                                          select new { UserID = p.Name.ToLower().Replace(" ", ""), UserName = p.Name.ToUpper(), MobileNumber = p.MobileNumber, EmailID = p.EmailAddress1 }).FirstOrDefault();
                        txtEmail.Text = userDetail.EmailID;
                        txtMobileNumber.Text = userDetail.MobileNumber.ToString();
                        if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                        {
                            txtUserId.Text = userDetail.UserID.ToString();
                            txtUserName.Text = userDetail.UserName.ToString();
                        }
                    }
                    else if (userType == EConnect.URM.UserType.RegionalCenter)
                    {
                        var userDetail = (from p in context.RegionalCenters
                                          where p.ID == (int)entityID
                                          select new { UserID = p.Name.ToLower().Replace(" ", ""), UserName = p.Name.ToUpper(), MobileNumber = p.RegisteredMobileNumber, EmailID = p.RegisteredEmailAddress }).FirstOrDefault();
                        txtEmail.Text = userDetail.EmailID;
                        txtMobileNumber.Text = userDetail.MobileNumber.ToString();
                        if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                        {
                            txtUserId.Text = userDetail.UserID.ToString();
                            txtUserName.Text = userDetail.UserName.ToString();
                        }
                    }
                    //Added 30 Aug 2019
                    else if (userType == EConnect.URM.UserType.projectNIELITCentre)
                    {
                        NIELITMISContext context1 = new NIELITMISContext();
                        var userDetail = (from p in context1.NielitCentres
                                          where p.ID == (int)entityID
                                          select new
                                          {
                                              UserID = p.Name.ToLower().Replace(" ", ""),
                                              UserName = p.Name.ToUpper(),
                                              MobileNumber = p.mobile,
                                              EmailID = p.email1
                                          }).FirstOrDefault();
                        txtEmail.Text = userDetail.EmailID;
                        txtMobileNumber.Text = userDetail.MobileNumber.ToString();
						 //Added 26 June 2020
                        txtEmail.Enabled = true ;
                        txtMobileNumber.Enabled = true;
                        //
                        if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                        {
                            txtUserId.Text = userDetail.UserID.ToString();
                            txtUserName.Text = userDetail.UserName.ToString();
                        }
                    }
                    if (txtEmail.Text == "" || txtMobileNumber.Text == "")
                    {
                        ShowAlert("Either email adress or mobile number of " + ddlUserType.SelectedItem.Text + " not found.", true);
                    }
                };
            }
            else
            {
                txtUserId.Text = "";
                txtUserName.Text = "";
                txtEmail.Text = "";
                txtMobileNumber.Text = "";
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void Bindrole(Int32 usertypeID)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var roles = from s in context.Roles
                            where s.UserTypeID == usertypeID 
                            orderby s.Name
                            select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(Ddlrolename, roles, lst);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BreadCrumb1.Render();
        hfEntityID.Value = "0";
        txtEntity.Text = "";
        txtUserId.Text = "";
        txtUserName.Text = "";
        ddlExpiryDays.SelectedValue = "10";
        ddlStatus.SelectedValue = "1";
        txtEmail.Text = "";
        txtMobileNumber.Text = "";
        Int32 UserTypeId = Convert.ToInt32(ddlUserType.SelectedValue);
        //deep add code on 23 may 2018
        if (UserTypeId == 5)
        {
            txtMobileNumberRegCentre.Visible = true;
            txtEmailRegCentre.Visible = true;
            Label8.Visible = true;
            Label9.Visible = true;
        }
        else
        {
            txtMobileNumberRegCentre.Visible = false;
            txtEmailRegCentre.Visible = false;
            Label8.Visible = false;
            Label9.Visible = false;
        }
        //deep end code on 23 may 2018
        acUserType.ContextKey = UserTypeId.ToString();
        Int32 usertypeID = Convert.ToInt32(ddlUserType.SelectedValue);
        Bindrole(usertypeID);
    }


    protected void BindCoursesForMapping()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var courses = from s in context.Courses
                              orderby s.CourseTypeID , s.DisplayOrder
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(chkcourselist, courses);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }
    }

    protected void Ddlrolename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if ((Convert.ToInt32(Ddlrolename.SelectedValue) == Convert.ToInt32(enmRole.CourseWiseHeadOffice)) || (Convert.ToInt32(Ddlrolename.SelectedValue) == Convert.ToInt32(enmRole.CourseWiseAdmin)))
                tblShow.Visible = true;
            else
                tblShow.Visible = false;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }
    }
}