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

public partial class HO_CHMTResultPublish : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 loginUserNo = 0;
    Int32 currentRoleId = 0;
    string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsSessionAlive() == false)
            Response.Redirect("../Index.aspx");
        currentRoleId = Convert.ToInt32(Session["RoleID"]);
        loginUserNo = Convert.ToInt32(Session["UserID"]);
       // loginUserNo = 1111;
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            else
            { 
                if (!IsPostBack)
                {
                    BindExamDropdown();
                }
      }
    }

    private void BindExamDropdown()
    {

        try
        {

            string query = "SELECT  EXAM_ID id , ( SELECT  NAME FROM EXAM E  WHERE  E.ID=F.exam_id ) AS examName  FROM   CHMT_OLevel_ResultFreeze  F WHERE whether_compiled='Y' AND whether_finalized='Y'AND whether_published='N' ";
                          

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                 
                    ddlExam.DataSource = dt;
                    ddlExam.DataTextField = "examName"; // Displayed text
                    ddlExam.DataValueField = "id";  // Value associated with the item
                    ddlExam.DataBind();
                   
                    ddlExam.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }


    protected void btnFreeze_Click(object sender, EventArgs e)
    {
        int examId = Convert.ToInt32(ddlExam.SelectedValue);
        string conString = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        using (SqlConnection con = new SqlConnection(conString))
        {
            using (SqlCommand cmd = new SqlCommand("CHMTResult_Publish", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@examID", examId);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", loginUserNo);

                SqlParameter outputParam = new SqlParameter("@msg", SqlDbType.NVarChar, 255);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);


                con.Open();
                cmd.ExecuteNonQuery();
               

                String message = Convert.ToString(cmd.Parameters["@msg"].Value);

                if (!string.IsNullOrEmpty(message))
                {
                    lblError.Visible = true;
                    lblError.Text = "Result is either not finalized or already published.";
                }

                else
                 {

                     lblError.Visible = true;
                     lblError.Text = "Data has been published successfully.";
                    //ShowAlert("Data has been published successfully.");
                 }

            }
        }
    }
}