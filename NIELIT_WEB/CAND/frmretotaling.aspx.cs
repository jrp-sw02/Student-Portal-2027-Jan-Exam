using System;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class CAND_frmretotaling :BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Int32 id = Convert.ToInt32(Request.QueryString["CourseID"]);
        using (EConnectContext context = new EConnectContext())
        {
            Course currentcourse = context.Courses.Find(id);
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Retotalling", "", ""));
            BreadCrumb1.Render();
        };
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/frmconfirm.aspx");
    }
}