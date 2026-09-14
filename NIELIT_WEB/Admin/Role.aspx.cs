using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;

public partial class Admin_Role : BasePage
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
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Roles", "", ""));
                
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
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
            var query = from s in context.Roles
                        select new
                        {
                            ID = s.ID,
                            name= s.Name,
                            usertype = s.Usertype.Name
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
                     case "usertype":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.usertype);
                        else
                            query = query.OrderBy(s => s.usertype);
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
                if (gvMain.EditIndex == e.Row.RowIndex)
                {
                        DropDownList ddlusertype = (DropDownList)e.Row.FindControl("ddlusertype2");
                        using (EConnectContext context = new EConnectContext())
                        {
                                var filluserstype = (from p in context.UserTypes
                                                     orderby p.Name
                                                     select new { ValueField = p.ID, TextField = p.Name });
                                ListItem lst = new ListItem("--Select One--", "0");
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlusertype, filluserstype, lst);
                                ddlusertype.Items.FindByText((e.Row.FindControl("lbQLevel") as Label).Text).Selected = true;
                        };
                        
                }
              
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                TextBox rolenamefooter = (TextBox)e.Row.FindControl("Txtrolename");
                DropDownList ddluser = (DropDownList)e.Row.FindControl("ddlusertype1");
                using (EConnectContext context = new EConnectContext())
                {
                    var filluserstype = (from p in context.UserTypes
                                             orderby p.Name
                                             select new { ValueField = p.ID, TextField = p.Name });
                    ListItem lst = new ListItem("--Select One--", "0");
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddluser, filluserstype, lst);
                };
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Add")
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.New))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to add new record.", true);
                    return;
                }
                TextBox rolename = (TextBox)gvMain.FooterRow.FindControl("Txtrolename");
                DropDownList ddlusertype = (DropDownList)gvMain.FooterRow.FindControl("ddlusertype1");
                string rolname= rolename.Text;
                Int32 usertypeId = Convert.ToInt32(ddlusertype.SelectedValue);
                using (EConnectContext context = new EConnectContext())
                {
                    Role role = new Role();
                    if (context.Roles.Any(s=>s.Name.ToUpper() == rolname.ToUpper() && s.UserTypeID == usertypeId))
                    {
                        throw new Exception("Data already exixts");
                    }
                    else
                    {
                        role.Name = rolname;
                        role.UserTypeID = usertypeId;
                        context.Roles.Add(role);
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
    protected void gvMain_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            Int32[] ids = { 1, 2, 3, 4, 5, 6 };
            Int32 id = Int32.Parse(gvMain.DataKeys[e.NewEditIndex].Value.ToString());
            if (ids.Contains(id))
            {
                string msg1 = "You cannot edit this record";
                ShowAlert(msg1, true);
                e.Cancel = true;
            }
            else if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to edit this record.", true);
                return;
            }
            else
            {
                gvMain.Columns[3].ItemStyle.Width = Unit.Percentage(10);
                gvMain.EditIndex = e.NewEditIndex;
                gvMain.Columns[gvMain.Columns.Count - 1].Visible = false;
                BindGridView();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true); ;
        }

    }
    protected void gvMain_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            gvMain.EditIndex = -1;
            gvMain.Columns[gvMain.Columns.Count - 1].Visible = true;
            BindGridView();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvMain_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            gvMain.Columns[gvMain.Columns.Count - 1].Visible = true;
            using (EConnectContext context = new EConnectContext())
            {
                GridViewRow row = (GridViewRow)gvMain.Rows[e.RowIndex];
                Int32 id = Int32.Parse(gvMain.DataKeys[e.RowIndex].Value.ToString());
                Int32[] ids = { 1, 2, 3, 4, 5, 6 };
                if (ids.Contains(id))
                {
                    string msg1 = "You cannot edit this record";
                    ShowAlert(msg1, true);
                }
                else if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to edit this record.", true);
                    return;
                }
                else
                {
                    TextBox txtEditTemplate = (TextBox)row.FindControl("Txtrolename1");
                    string rolename = txtEditTemplate.Text;
                    DropDownList dApplicant = (DropDownList)row.FindControl("ddlusertype2");
                    Role updaterole = new Role();
                    updaterole = context.Roles.Find(id);
                    updaterole.UserTypeID = Convert.ToInt32(dApplicant.SelectedValue);
                    updaterole.Name = rolename;
                    context.SaveChanges();
                }
            };

            String msg = "Record updated.";
            ShowAlert(msg, true);
            //Reset the edit index.
            gvMain.EditIndex = -1;
            BindGridView();
            BreadCrumb1.Render();
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
            
            Int32 ID =  Convert.ToInt32(gvMain.DataKeys[e.RowIndex].Values[0].ToString());
            Int32[] ids = { 1, 2, 3, 4, 5, 6 };
            if (ids.Contains(ID))
            {
                String msg = "You cannot delete this record";
                ShowAlert(msg, true);
            }
            else if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to delete record.", true);
                return;
            }
            else
            {
                context = new EConnectContext();
                Role Q = context.Roles.Find(ID);
                context.Roles.Remove(Q);
                context.SaveChanges();
                String msg = "Record deleted successfully";
                ShowAlert(msg, true);
                BindGridView();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    //[System.Web.Services.WebMethod(EnableSession = true)]
    //public static String[] GetSearchText(String prefixText, Int32 count)
    //{
    //    EConnectContext context = new EConnectContext();
    //    try
    //    {
    //        if (count <= 0)
    //            count = 10;
    //        List<String> items = new List<String>();
    //        string searchString = prefixText.Trim().ToUpper();
    //        var users = from s in context.Roles
    //                    select new { Name = s.Name };
    //        if (!String.IsNullOrEmpty(searchString))
    //        {
    //            users = users.Where(s => s.Name.ToUpper().Contains(searchString));
    //        }
    //        users = users.OrderBy(s => s.Name).Distinct();
    //        foreach (var user in users)
    //        {
    //            items.Add(user.Name);
    //        }
    //        return items.ToArray();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally { context.Dispose(); }
    //}
}  
   
   