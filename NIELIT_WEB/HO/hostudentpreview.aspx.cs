using System;

public partial class HO_hostudentpreview : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("LoadsProcess.aspx?key=R/BTH/101");
    }
}