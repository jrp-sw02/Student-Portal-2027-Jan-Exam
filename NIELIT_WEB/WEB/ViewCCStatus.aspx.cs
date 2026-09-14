using System;
using System.Web.UI;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class ViewCCStatus : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                RenderPage(Convert.ToInt32(Request.QueryString["id"].ToString()));
            }
        }
        base.ReWriteAction(this.Form);
    }

    protected void RenderPage(Int32 currentCourseID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Course currentCourse = context.Courses.Find(currentCourseID);
                if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    Label1.Text = "Course";
                    Label2.Text = "Course";
                    Label3.Text = "Course";
                    Label4.Text = currentCourse.Name;
                }
                else if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                {

                    Label1.Text = "Certificate";
                    Label2.Text = "Certificate";
                    Label3.Text = "Certificate";
                    Label4.Text = currentCourse.Name;
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}