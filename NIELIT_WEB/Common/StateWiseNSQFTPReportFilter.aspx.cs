using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Collections.Generic;
using System.Web.UI;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

public partial class StateWiseNSQFTPReportFilter : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int32 UserTypeId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }

            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);

            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                //Testing
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            if (!Page.IsPostBack)
            {
                txtAsOnDate.Text = "";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("StateWise NSQF TP Stats", "HO/Rpt/StateWiseNSQFTPs.aspx", ""));
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    #region vCode

    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("~/Common/StateWiseNSQFTPReportFilter.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }


    #endregion
}

    