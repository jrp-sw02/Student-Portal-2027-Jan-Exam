using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class Admin_DLC_AddReason : BasePage 
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;


    protected void Page_Load(object sender, EventArgs e)
    {

         //if (IsSessionAlive() == false)
         //       Response.Redirect("../Index.aspx");
         //   currentRoleId = Convert.ToInt32(Session["RoleID"]);
         //   loginUserNo = Convert.ToInt32(Session["UserID"]);
         //   if (!UserManager.HasRight(currentRoleId, enmRight.View))
         //   {
         //       Response.Write("Sorry! You don't have rights  to view this page");
         //       Response.End();
         //   }
         //   loginUserType = (UserType)Session["UserType"];
         //   entityID = Convert.ToInt64(Session["EntityID"]);
         //   if (loginUserType == UserType.RegionalCenter || loginUserType == UserType.HeadOffice || loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
         //   {
                if (!IsPostBack)
                {
                    BindGridView();
                    txtStatusName.Text = "Application Rejected by Examination Wing";
                }
           // }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string statusName = "Application Rejected by Examination Wing";
        string statusDescription = txtStatusDescription.Text;

        using (var context = new EConnectContext())        
        {
            var reasonCheck = (from a in context.ApplicationStatuses
                               where a.Description == statusDescription
                               select new { Desc = a.Description }).FirstOrDefault();
            if (reasonCheck == null)
            {

                context.Database.ExecuteSqlCommand(" INSERT INTO [dbo].[Application_Status]  ([Name],[Description]) VALUES ('" + statusName + "','" + statusDescription + "')");
            }
            else
            {
                Lblerror.Visible = true;
                Lblerror.Text = " This ";
            }
        }
    }

    protected void BindGridView()
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        try
        {
            DataTable DT = new DataTable();
            con.Open();
           
            using (SqlCommand Cmm = new SqlCommand("select id,Description from Application_Status where id > 21  ", con))
            {
                Cmm.CommandType = CommandType.Text;
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                Sda.Fill(DT);
            }
         
            grdReason.DataSource = DT;
            grdReason.DataBind();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }               

    }
}