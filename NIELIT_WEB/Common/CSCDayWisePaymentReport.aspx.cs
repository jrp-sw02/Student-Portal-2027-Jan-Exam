using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.URM;
using EConnect.Utils.Common;

public partial class Common_CSCDayWisePaymentReport : BasePage
{
    String strMessage = string.Empty;
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
                //txttDateFrom.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                //txtDateto.Text = DateTime.Now.ToString("dd-MMM-yyyy");             
                //ViewState["SortField"] = "";
                //ViewState["SortOrder"] = "";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("CSC Day Wise Payment Report ", "#", ""));
                //if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                //    ShowAlert(Request.QueryString["msg"].ToString());
            }
            BreadCrumb1.Render();

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }
}