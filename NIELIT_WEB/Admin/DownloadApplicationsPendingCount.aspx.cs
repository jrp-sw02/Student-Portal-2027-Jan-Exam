using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;


public partial class Admin_DownloadApplicationsPendingCount : BasePage
{

    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (IsSessionAlive() == false)
            Response.Redirect("../Index.aspx");
        //currentRoleId = Convert.ToInt32(Session["RoleID"]);
        //if (!UserManager.HasRight(currentRoleId, enmRight.View))
        //{
        //    Response.Write("Sorry! You don't have rights  to view this page");
        //    Response.End();
        //}
        //loginUserType = (UserType)Session["UserType"];
        //entityID = Convert.ToInt64(Session["EntityID"]);
        if (!IsPostBack)
        {
            Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 examID = Convert.ToInt32(Request.QueryString["examID"]);
            Int32 regionalCentreID = Convert.ToInt32(Request.QueryString["regionalCentreID"]);
        }
    }

    protected void btnPendingCount_Click(object sender, EventArgs e)
    {
        BindGridView();
    }
    protected void BindGridView()
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        try
        {
            DataTable DT = new DataTable();

            Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 examID = Convert.ToInt32(Request.QueryString["examID"]);
            Int32 regionalCentreID = Convert.ToInt32(Request.QueryString["regionalCentreID"]);                                         

            //Int64 pendingCount = Convert.ToInt64(EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) Pending_Count from Certificate_Exam_Application cea where Course_ID ='" + courseID + "'and exam_id < (select id from exam e where e.Course_ID = cea.Course_ID and e.Exam_Month = 12 and e.Exam_Year = 2021) and Roll_Number is null and Payment_Status_ID in (2,4) and cea.Demand_Note_ID is not null and exists (select id from Demand_Note d where d.ID = cea.Demand_Note_ID and Status_ID in (2,4) and (Online_Transaction_ID is not null or NEFT_Transaction_ID is not null or CSC_Transaction_ID  is not null )) and ((Application_Status_ID  = 17 ) or (Application_Status_ID in(7,9)) and cea.Regional_Center_ID ='" + regionalCentreID + "') ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
            //if ( pendingCount == 0 )
            //{
            //    Lblerror.Visible = true;
            //    Lblerror.Text = "There is no Pending Count for this Exam.";
            //}
            //else 
            //{

            using (SqlCommand Cmm = new SqlCommand( "  select Number , UPPER(Name) as Name ,  (select Name  from exam where id =Exam_ID) Applied_Exam_Name  " +
                                                    " from Certificate_Exam_Application cea  where Course_ID = '"+ courseID +"' and exam_id < ( select id from exam e  " + 
                                                    " where e.Course_ID = cea.Course_ID and e.Exam_Month = 12 and e.Exam_Year = 2021 ) and Roll_Number is null and Payment_Status_ID in (2,4) " +
                                                    " and cea.Demand_Note_ID is not null and exists ( select id from Demand_Note d where d.ID = cea.Demand_Note_ID  and  Status_ID in (2,4) and  " +
                                                    " (Online_Transaction_ID is not null or NEFT_Transaction_ID is not null or CSC_Transaction_ID  is not null  ) " +
                                                    " ) and ( (Application_Status_ID  = 17 ) or ( Application_Status_ID in(7,9)) and cea.Regional_Center_ID = '"+ regionalCentreID +"' )", con))


            {
                Cmm.CommandType = CommandType.Text;
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                Sda.Fill(DT);
            }
             
            PagingBar1.Bind(DT, ref gvPendingCount);
            Updatepanel1.Update();
            uPnlNavigation.Update();
            gvPendingCount.Visible = true;
            
            }
       // }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {            
            gvPendingCount.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
          
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}