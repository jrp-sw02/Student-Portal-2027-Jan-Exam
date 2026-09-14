using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Transactions;


public partial class Admin_Default : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Int32 courseID = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                courseID = Convert.ToInt32(Request.QueryString["id"]);
            }
        }
    }
    protected void btn_disagree_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Index.aspx");
    }
    protected void btn_agree_Click(object sender, EventArgs e)
    {
        try
        {
            if (chk_declaration.Checked == false)
            {
                ShowAlert("Please check the Declaration");
                return;
            }

            Response.Redirect("~/WEB/DownloadAdmitCard.aspx?" + Request.QueryString);

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }


    }

}
