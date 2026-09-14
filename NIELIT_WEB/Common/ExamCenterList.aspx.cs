using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class Common_ExamCenterList :BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);

    protected void Page_Load(object sender, EventArgs e)
    {
        lblerror.Text = "";
        try
        {
            
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!IsPostBack)
            {
                BindListData();
                ddlCourseCategry.SelectedValue = "2";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Centre List Report", "#", ""));
            }
            BreadCrumb1.Render();
            
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }
    protected void BindListData()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                Int32 CourseType;
                CourseType = Convert.ToInt32(enmCourseType.CertificationExam);
                var Category = (from p in context.Courses
                               where p.CourseTypeID==CourseType
                               orderby (p.Name)
                               select new { ValueField = p.CourseCategory.ID, TextField = p.CourseCategory.Name }).Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, Category, lst);

                ListItem lst1 = new ListItem("--All--", "0");
                var state = from s in context.Locations
                            where s.LocationTypeID == 2
                            select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlState, state, lst1);               
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
            BreadCrumb1.Render();
            ddlCourseCategry.SelectedValue = "0";
            ddlState.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlState.SelectedValue == "0")
        {
            ddlListMode.SelectedValue = "0";
            ddlListMode.Enabled = false;
            
        }
        else
        {
            ddlListMode.SelectedValue = "1";
            ddlListMode.Enabled = true;
            
        }
    }
   
}