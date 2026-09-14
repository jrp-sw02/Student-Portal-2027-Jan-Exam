using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.URM;
using EConnect.DAL;
using System.Web.Security;

public partial class Admin_ExternalEntity :BasePage
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
                    ShowEditMode();
                }
                else
                {
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("External Entity", "Admin/ExternalEntity.aspx", ""));
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BindGridView();
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
    protected void ShowEditMode()
    {
        try
        {
            context= new EConnectContext();
            Int32 userId = Convert.ToInt32(Request.QueryString["Key"]);
            var users = (from p in context.ExternalEntities
                                 where p.ID == userId
                                 select p).FirstOrDefault();
            txtusername.Text = users.Name;
            txtmobileno.Text = users.MobileNumber.ToString();
            txtemail.Text = users.Email;
            txtaddress.Text = users.Address;
            txtcomments.Text = users.Description;
            btnMode.ViewMode = ToggleView.Mode.List;
            btnSave.Text = "Update";
            //tblNavLinks.Visible = true;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            lblHeading.Text = "External Entity Details";
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(users.Name, "Admin/ExternalEntity.aspx?key=" + Request.QueryString["key"].ToString(),""));
            //hlUserLog.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("UserLog.aspx?UID=" +users.ID.ToString());
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
            context = new EConnectContext();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var users = (from s in context.ExternalEntities
			//Added 3 Oct 2019 for login access
                        join p in context.Users  on s.ID equals p.UserRefNumber 
                        //
                        select new 
                        {
							 //Added 3 Oct 2019 for login access
                            HasLoginAccess = p.HasLoginAccess ? "Enabled" : "Disabled",
                            //
                            UserID = s.ID,
                            UserName = s.Name.ToUpper(),
                            MobileNo = s.MobileNumber,
                            Emailaddress = s.Email.ToLower(),
                       }).Distinct ();
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where((s => s.UserName.ToUpper().Contains(searchString)));
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "UserID":
                        if (sortOrder == "DESC")
                            users = users.OrderByDescending(s => s.UserID);
                        else
                            users = users.OrderBy(s => s.UserID);
                        break;
                    case "UserName":
                        if (sortOrder == "DESC")
                            users = users.OrderByDescending(s => s.UserName);
                        else
                            users = users.OrderBy(s => s.UserName);
                        break;
                    case "MobileNo":
                        if (sortOrder == "DESC")
                            users = users.OrderByDescending(s => s.MobileNo);
                        else
                            users = users.OrderBy(s => s.MobileNo);
                        break;
                    case "Emailaddress":
                        if (sortOrder == "DESC")
                            users = users.OrderByDescending(s => s.Emailaddress);
                        else
                            users = users.OrderBy(s => s.Emailaddress);
                        break;
                    default:
                        users = users.OrderBy(s => s.UserID);
                        break;
                }
            }
            PagingBar1.Bind(users, ref gvMain);
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
                ShowAlert("Sorry! You don't have rights to add new record.");
                return;
            }
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            lblHeading.Text = "New External Entity";
            //Updating Breadscrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New External Entity", "#", ""));
        }
        else
        {
            Response.Redirect("ExternalEntity.aspx", true);
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
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
            ExternalEntity exuser;
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                exuser = new EConnect.URM.ExternalEntity();
                exuser.CreatedBy = Convert.ToInt32(Session["UserID"]);
                exuser.CreatedOn = DateTime.Now;
                exuser.Name = txtusername.Text;
                exuser.MobileNumber = Convert.ToInt64(txtmobileno.Text);
                exuser.Address = txtaddress.Text;
                exuser.Email = txtemail.Text;
                exuser.Description = txtcomments.Text;
                context.ExternalEntities.Add(exuser);
                context.SaveChanges();
                strMessage = "New record saved.";
            }
            else
            {
                exuser = context.ExternalEntities.Find(Convert.ToInt32(Request.QueryString["Key"]));
                exuser.Name = txtusername.Text;
                exuser.MobileNumber = Convert.ToInt64(txtmobileno.Text);
                exuser.Address = txtaddress.Text;
                exuser.Email = txtemail.Text;
                exuser.Description = txtcomments.Text;
                context.SaveChanges();
                strMessage = "Record updated.";
            }
         Response.Redirect("ExternalEntity.aspx?msg="+strMessage);
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
            ddlSearchUserType.SelectedValue = "0";
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
            if (hfActionID.Value != "")
            {
                String recordID = hfActionID.Value.Split('$')[0].ToString();
                LinkButton btnAction = (LinkButton)sender;
                if (btnAction.CommandName == "Reset")
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        User selectedUser = UserManager.GetUserByUserID(Convert.ToInt32(hfActionID.Value), context);
                        selectedUser.Password = UserManager.ComputeSha256Hash(selectedUser.LoginID.ToLower()).ToUpper(); // FormsAuthentication.HashPasswordForStoringInConfigFile(selectedUser.LoginID.ToLower(), System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
                        selectedUser.LastPasswordChangedOn = DateTime.Now;
                        context.Entry(selectedUser).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
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
                        if (selectedUser.HasLoginAccess == true)
                            selectedUser.HasLoginAccess = false;
                        else
                            selectedUser.HasLoginAccess = true;
                        context.Entry(selectedUser).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
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
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl);
                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);
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
            var users = from s in context.ExternalEntities
                        select new { Name = s.Name};
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            users = users.OrderBy(s => s.Name);
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
        Response.Redirect("ExternalEntity.aspx", true);
    }
}
