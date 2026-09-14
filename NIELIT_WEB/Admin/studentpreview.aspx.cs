using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class studentpreview : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sf =Convert.ToString(Request.QueryString["v"]);
        if (sf == "s")
        {
            
            btnverify.Visible = false;           
            btnskip.Visible = false;
            btnreject.Visible = false;
            a3.HRef = "FrmStudentList.aspx?status=for Pending Varification&course=O level(Computer S/w)&St=0&sts=f";

        }
        if (sf == "f")
        {
            btnprevious.Visible = true;
            btnverify.Visible = true;
            btnskip.Visible = true;
            btnreject.Visible = true;
            btnnext.Visible = true;
            a3.HRef = "FrmStudentList.aspx?status=for Pending Payment&course=O level(Computer S/w)&St=0&sts=s";

        }
    }
  
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        string sf = Convert.ToString(Request.QueryString["v"]);
        if (sf == "f")
        {

            Response.Redirect("~/FrmStudentList.aspx?status=for Pending Varification&course=O level(Computer S/w)&St=0&sts=f");
        }
        if (sf == "s")
        {
            Response.Redirect("~/FrmStudentList.aspx?status=for Pending Payment&course=O level(Computer S/w)&St=0&sts=s");
        }
    }
}