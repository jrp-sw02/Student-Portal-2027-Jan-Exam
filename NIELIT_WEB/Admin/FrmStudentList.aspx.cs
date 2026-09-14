using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrmStudentList : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string status =Convert.ToString(Request.QueryString["status"]);
        lblHeading.Text = "List Of Candidates  " + status;

        switch(status)
        {
            case "for Pending Varification":
                 btnReject.Visible = true;
                 btnVerify.Visible = true;
                 btnVerify.Text = "Verify";
                 btnvermark.Visible = true;
                 Div_Default.Visible = true;
                 DivForword.Visible = false;
                 DivTotReg.Visible = false;
                break;
            case "for Pending Payment":
                 btnReject.Visible = false;
                 btnVerify.Visible = true;
                 btnVerify.Text = "Pay Now";
                 Div_Default.Visible = true;
                 DivForword.Visible = false;
                 DivTotReg.Visible = false;
                break;
            case "for Pending For Forward":
                 Div_Default.Visible = false;
                 DivForword.Visible = true;
                 DivTotReg.Visible = false;
                 btnReject.Visible = false;
                 btnVerify.Visible = true;
                 btnVerify.Text = "Submit";
                
                break;
            case "for Total Registration":
                 Div_Default.Visible = false;
                 DivForword.Visible = false;
                 DivTotReg.Visible = true;
                 btnReject.Visible = false;
                 btnVerify.Visible = false;
                break;

        }

     
        a1.HRef = "acc_reg_info.aspx";
        a1.InnerText = "Course Registration Status";
      
    }
    protected void btnVerify_Click(object sender, EventArgs e)
    {
         string status = Request.QueryString["status"].ToString();
         if (status  == "for Pending Payment")
        Response.Redirect("~/FrmConfirm.aspx");
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/acc_reg_info.aspx");
    }
}