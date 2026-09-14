using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;

public partial class UserRole : BasePage
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
                ViewState["SortField"] = "";
                ViewState["SortOrder"] = "";
                BindGridView();
                if (!string.IsNullOrEmpty(Request.QueryString["UID"]))
                {
                    Int32 UID = Convert.ToInt32(Request.QueryString["UID"]);
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("User Roles", "Admin/UserRole.aspx?UID=" + UID, ""));
                }
                else
                {
                    //BreadCrumb1.RemoveLastBreadCrumbItem();
                    //BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("User Roles", "#", ""));
                    BreadCrumb1.Render();
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
                if (gvMain.Rows.Count <= 0)
                {
                    btnMode.Visible = true;
                    lblError.Visible = true;
                    lblError.Text = "No Record Found";
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindGridView()
    {
        try
        {
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            Int32 UID = Convert.ToInt32(Request.QueryString["UID"]);
            lblError.Visible = false;
            if (UID != 0)
            {
                Int32 usertypeID = (from c in context.Users
                                    where c.UserID == UID
                                    select new
                                    {
                                        usertypeID = c.UserTypeID
                                    }).FirstOrDefault().usertypeID;

                Int32 userID = (from c in context.UserRoles
                                where c.UserID == UID
                                select new
                                {
                                    UserID1 = c.UserID
                                }).FirstOrDefault().UserID1;


                Int32 RoleId = (from c in context.UserRoles
                                where c.UserID == UID
                                select new
                                {
                                    RoleID = c.RoleID
                                }).FirstOrDefault().RoleID;

                Int32 DefaultRoleID = (from c in context.Users
                                       where c.UserID == userID
                                       select new
                                       {
                                           DefaultRoleID = c.DefaultRoleID
                                       }).FirstOrDefault().DefaultRoleID;

                if (RoleId == DefaultRoleID)
                {

                    var query = from u in context.UserRoles
                                join c in context.Roles on u.RoleID equals c.ID
                                where c.UserTypeID == usertypeID && u.UserID == UID
                                select new
                                {
                                    ID = u.ID,
                                    name = c.Name + " (Default Role)",
                                    CreatedOn = u.CreatedOn
                                };

                    if (!string.IsNullOrEmpty(searchString))
                    {
                        query = query.Where((s => s.name.ToUpper().Contains(searchString)));
                    }
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
                            case "CreatedOn":
                                if (sortOrder == "DESC")
                                    query = query.OrderByDescending(s => s.CreatedOn);
                                else
                                    query = query.OrderBy(s => s.CreatedOn);
                                break;
                            default:
                                query = query.OrderBy(s => s.ID);
                                break;
                        }
                    }
                    PagingBar1.Bind(query, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                }

                else
                {
                    var query = from u in context.UserRoles
                                join c in context.Roles on u.RoleID equals c.ID
                                where c.UserTypeID == usertypeID && u.UserID == UID
                                select new
                                {
                                    ID = u.ID,
                                    name = c.Name,
                                    CreatedOn = u.CreatedOn
                                };
                    if (!string.IsNullOrEmpty(searchString))
                    {
                        query = query.Where((s => s.name.ToUpper().Contains(searchString)));
                    }
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
                            case "CreatedOn":
                                if (sortOrder == "DESC")
                                    query = query.OrderByDescending(s => s.CreatedOn);
                                else
                                    query = query.OrderBy(s => s.CreatedOn);
                                break;
                            default:
                                query = query.OrderBy(s => s.ID);
                                break;
                        }
                    }
                    PagingBar1.Bind(query, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                }
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
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList rolenamefooter = (DropDownList)e.Row.FindControl("ddlRole");
                Label txtDate = (Label)e.Row.FindControl("lblFCreatedOnDate");
                Int32 UID = Convert.ToInt32(Request.QueryString["UID"]);
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 usertypeID = (from c in context.Users
                                        where c.UserID == UID
                                        select new
                                        {
                                            usertypeID = c.UserTypeID
                                        }).FirstOrDefault().usertypeID;
                    Int32 RoleId = (from c in context.UserRoles
                                    where c.UserID == UID
                                    select new
                                    {
                                        RoleID = c.RoleID
                                    }).FirstOrDefault().RoleID;

                    var filluserstype = (from u in context.Roles
                                         where u.UserTypeID == usertypeID && u.ID != RoleId
                                         select new { ValueField = u.ID, TextField = u.Name });
                    ListItem lst = new ListItem("--Select One--", "0");
                    EConnect.Utils.Common.ControlUtility.BindListObject(rolenamefooter, filluserstype, lst);
                };
                txtDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Add")
            {
                DropDownList rolename = (DropDownList)gvMain.FooterRow.FindControl("ddlRole");
                Label txtcreateddate = (Label)gvMain.FooterRow.FindControl("lblFCreatedOnDate");
                DateTime createddate = Convert.ToDateTime(txtcreateddate.Text);
                Int32 roleid = Convert.ToInt32(rolename.SelectedValue);
                Int32 UID = Convert.ToInt32(Request.QueryString["UID"]);
                using (EConnectContext context = new EConnectContext())
                {
                    EConnect.URM.UserRole usr = new EConnect.URM.UserRole();
                    if (context.UserRoles.Any(s => s.RoleID == roleid))
                    {
                        throw new Exception("Data already exixts");
                    }
                    else
                    {
                        usr.RoleID = roleid;
                        usr.UserID = UID;
                        usr.CreatedOn = createddate;
                        usr.CreatedBy = Convert.ToInt32(Session["UserID"]);
                        context.UserRoles.Add(usr);
                        context.SaveChanges();
                        String msg = "Record Saved.";
                        ShowAlert(msg, true);
                    }

                };
                gvMain.EditIndex = -1;
                BindGridView();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gvMain_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

            Int32 ID = Convert.ToInt32(gvMain.DataKeys[e.RowIndex].Values[0].ToString());
            Int32 UID = Convert.ToInt32(Request.QueryString["UID"]);
            using (EConnectContext context = new EConnectContext())
            {
                Int32 userID = (from c in context.UserRoles
                                where c.ID == ID
                                select new
                                {
                                    UserID1 = c.UserID
                                }).FirstOrDefault().UserID1;

                Int32 RoleId = (from c in context.UserRoles
                                where c.UserID == UID
                                select new
                                {
                                    RoleID = c.RoleID
                                }).FirstOrDefault().RoleID;

                Int32 DefaultRoleID = (from c in context.Users
                                       where c.UserID == userID
                                       select new
                                       {
                                           DefaultRoleID = c.DefaultRoleID
                                       }).FirstOrDefault().DefaultRoleID;

                if (userID == 1)
                {
                    String msg = "You can not delete this record";
                    ShowAlert(msg, true);
                }
                else if (RoleId == DefaultRoleID)
                {
                    String msg = "You can not delete this record";
                    ShowAlert(msg, true);
                }
                else
                {

                    EConnect.URM.UserRole Q = context.UserRoles.Find(ID);
                    context.UserRoles.Remove(Q);
                    context.SaveChanges();
                    String msg = "Record deleted successfully";
                    ShowAlert(msg, true);
                    BindGridView();
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
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
            var users = from s in context.Roles
                        select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            users = users.OrderBy(s => s.Name).Distinct();
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
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            FillRole();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            txtCreatedOn.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            //btnMode.Visible = true;
            //Change the heading text as required
            lblHeading.Text = "New UserRole";
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New UserRole", "#", ""));
        }
        else
        {
            Response.Redirect("UserRole.aspx", true);
        }

    }
    protected void FillRole()
    {
        try
        {
            Int32 UID = Convert.ToInt32(Request.QueryString["UID"]);

            if (UID != 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 usertypeID = (from c in context.Users
                                        where c.UserID == UID
                                        select new
                                        {
                                            usertypeID = c.UserTypeID
                                        }).FirstOrDefault().usertypeID;
                    //Int32 RoleId = (from c in context.UserRoles
                    //                where c.UserID == UID
                    //                select new
                    //                {
                    //                    RoleID = c.RoleID
                    //                }).FirstOrDefault().RoleID;

                    //&& u.ID != RoleId
                    var filluserstype = (from u in context.Roles
                                         where u.UserTypeID == usertypeID
                                         select new { ValueField = u.ID, TextField = u.Name });
                    ListItem lst = new ListItem("--Select One--", "0");
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlRoleName, filluserstype, lst);
                };
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        using (EConnectContext context = new EConnectContext())
        {
            EConnect.URM.UserRole usr = new EConnect.URM.UserRole();
            Int32 roleid = Convert.ToInt32(ddlRoleName.SelectedValue);
            Int32 UID = Convert.ToInt32(Request.QueryString["UID"]);
            usr.RoleID = roleid;
            usr.UserID = UID;
            usr.CreatedOn = DateTime.Now;
            usr.CreatedBy = Convert.ToInt32(Session["UserID"]);
            context.UserRoles.Add(usr);
            context.SaveChanges();
            strMessage = "Record Saved";
            Response.Redirect("UserRole.aspx?msg=" + strMessage + "&UID=" + UID);
        };
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Int32 UID = Convert.ToInt32(Request.QueryString["UID"]);
        Response.Redirect("UserRole.aspx?UID=" + UID, true);
    }
}