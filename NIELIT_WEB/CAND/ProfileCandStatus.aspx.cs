using System;

public partial class CAND_ProfileCandStatus : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request.QueryString["type"]))
        {
            Label1.Text = Request.QueryString["type"];
            Label2.Text = Request.QueryString["type"];
        }
    }
}