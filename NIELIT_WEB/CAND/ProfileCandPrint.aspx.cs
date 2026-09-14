using System;

public partial class CAND_ProfileCandPrint_ : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request.QueryString["type"]))
        {
            if (Request.QueryString["type"] == "Name Change")
            {
                m1.ActiveViewIndex = 0;
            }
            else
            {
                m1.ActiveViewIndex = 1;
            }
        }
    }
}