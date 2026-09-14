using System;
using System.Linq;
using EConnect.DAL;

public partial class UserControl_NormaltHeader : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["OrgID"] = "1";
        showheader();
    }
    protected void showheader()
    {
        using (EConnectContext ctx = new EConnectContext())
        {
            Int32 orgID = Convert.ToInt32(Session["OrgID"]);
            var users1 = (from u in ctx.Organizations
                          where u.ID == orgID
                          select u).Single();
            tdHeaderBig.InnerText = users1.Name;
            string m = users1.MainHeading;
            string s = users1.SubHeading;
            tdHeaderSmall.InnerText = string.Concat(m, s);
        }
    }
}