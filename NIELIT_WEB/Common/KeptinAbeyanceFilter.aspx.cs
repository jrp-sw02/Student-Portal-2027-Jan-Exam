using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class Common_KeptinAbeyanceFilter : BasePage
{
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try 
        {
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Kept in Abeyance Report", "Common/KeptinAbeyanceFilter.aspx", ""));
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!IsPostBack)
            {
                bindCourse();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void bindCourse()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseTypeID = Convert.ToInt32(enmCourseType.CertificationCourse);
                ListItem lst = new ListItem("--ALL--", "0");
                var courses = from s in context.Courses
                              where s.CourseTypeID == CourseTypeID
                              select new { ValueField = s.ID, TextField = s.Name + " ( " + s.Code + " )" };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                }

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            ddlCourseName.SelectedValue = "0";
            txtDateFrom.Text = "";
            txtToDate.Text = "";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}