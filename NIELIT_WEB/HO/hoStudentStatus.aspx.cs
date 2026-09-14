using System;
using EConnect.DAL;

public partial class HO_hoStudentStatus : BasePage
{
    String strMessage = string.Empty;
    //EConnectContext context = new EConnectContext();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                //using (EConnectContext context = new EConnectContext())
                //{
                //    Int64 cid = Convert.ToInt64(Request.QueryString["Key"]);
                //    Candidate cr = context.Candidates.Find(cid);
                    
                //};
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(Request.QueryString["Name"], "", ""));
                //HO/hoCoursesRegStatus.aspx?" + Request.QueryString.ToString()
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }
    }
}