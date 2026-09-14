using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PracObserverDashboard : System.Web.UI.Page
{

    string CentreCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        CentreCode = Request.QueryString["CentreCode"];
        Session["CentreCode"] = CentreCode;

    }
}