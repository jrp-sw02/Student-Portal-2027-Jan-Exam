using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Common;
public partial class UserLog : BasePage
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View,"Admin/Users.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["UID"]))
            {
                hfUserID.Value = Request.QueryString["UID"];
            }
    
            if (!Page.IsPostBack)
            {
                    context=new EConnectContext();
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    var fillddl = (from p in context.LoginLogs
                                   orderby p.BrowserName ascending
                                   select new { ValueField = p.BrowserName, TextField = p.BrowserName }).Distinct();
                    ListItem lst = new ListItem("--All--", "0");
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlBrowser, fillddl, lst);

                    EnumUtility.BindListObject(ref ddlLog,typeof(EConnect.URM.AuthenticationResponse),new ListItem("--All--","3"));
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Login Log", "Admin/UserLog.aspx?UID=" + Request.QueryString["UID"], ""));
                    BindGridView();                
                   
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
            context=new EConnectContext();
            string userName = context.Users.Find(Convert.ToInt64(Request.QueryString["UID"].ToString())).LoginID;
            
            int loginStatus = -1;
            string browserType = "0";


            if (ddlLog.SelectedValue != "-1")
                loginStatus = Convert.ToInt32(ddlLog.SelectedValue);
            if (ddlBrowser.SelectedValue != "0")
                browserType = ddlBrowser.SelectedValue;
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var userLog = from s in context.LoginLogs
                          where (s.LoginID.ToUpper() == userName.ToUpper())
                          select s;
            DateTime? dateFrom = null ;
            DateTime? dateTo = null ;
            if(!String.IsNullOrEmpty(txtDateFrom.Text))
                dateFrom = Convert.ToDateTime(txtDateFrom.Text);
            if(!String.IsNullOrEmpty(txtToDate.Text))
                dateTo = Convert.ToDateTime(txtToDate.Text);
            
            if (loginStatus != 3)
                userLog = userLog.Where(s => s.LoginResponseValue == loginStatus);
            if (browserType != "0")
                userLog = userLog.Where(s => s.BrowserName == browserType);
            if(dateTo.HasValue && dateFrom.HasValue)
            {
                userLog = userLog.Where(s => s.LoginTime >= dateFrom.Value  
                            && s.LoginTime <=dateTo.Value);
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "LoginTime":
                        if (sortOrder == "DESC")
                            userLog = userLog.OrderByDescending(s => s.LoginTime);
                        else
                            userLog = userLog.OrderBy(s => s.LoginTime);
                        break;
                    case "BrowserName":
                        if (sortOrder == "DESC")
                            userLog = userLog.OrderByDescending(s => s.BrowserName);
                        else
                            userLog = userLog.OrderBy(s => s.BrowserName);
                        break;
                    case "ClientIP":
                        if (sortOrder == "DESC")
                            userLog = userLog.OrderByDescending(s => s.ClientIP);
                        else
                            userLog = userLog.OrderBy(s => s.ClientIP);
                        break;
                    case "SourceIP":
                        if (sortOrder == "DESC")
                            userLog = userLog.OrderByDescending(s => s.SourceIP);
                        else
                            userLog = userLog.OrderBy(s => s.SourceIP);
                        break;
                    case "LogoutTime":
                        if (sortOrder == "DESC")
                            userLog = userLog.OrderByDescending(s => s.LogoutTime);
                        else
                            userLog = userLog.OrderBy(s => s.LogoutTime);
                        break;
                    case "LoginResponseValue":
                        if (sortOrder == "DESC")
                            userLog = userLog.OrderByDescending(s => s.LoginResponseValue);
                        else
                            userLog = userLog.OrderBy(s => s.LoginResponseValue);
                        break;
                    default:
                        userLog = userLog.OrderBy(s => s.LoginTime);
                        break;
                }
            }
            PagingBar1.Bind(userLog, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();

            if (!UserManager.HasRight(currentRoleId, enmRight.Delete, "Admin/Users.aspx"))
            {
               gvMain.Columns[7].Visible = false ;
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
            ddlLog.SelectedValue = "3";
            ddlBrowser.SelectedValue = "0";
            txtDateFrom.Text = "";
            txtToDate.Text = "";
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
    protected void lbDelteteOne_Click(object sender, EventArgs e)
    {
        try
        {
            context = new EConnectContext();
          
          
            context.LoginLogs.Remove(context.LoginLogs.Find(Convert.ToInt32(hfActionID.Value.Split('$')[0].ToString())));
            context.SaveChanges();
            BindGridView();
            ShowAlert("Record deleted successfully.");
            hfActionID.Value = "";
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
                Image imgAction = (Image)e.Row.FindControl("imgAction");
                imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString() + "$" + gvMain.DataKeys[e.Row.RowIndex].Values[1].ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
 }