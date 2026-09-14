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

public partial class HO_CHMTProjectEntryManual :BasePage
{
    Int32 loginUserNo = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        //loginUserNo = Convert.ToInt32(Session["UserID"]);
        //if (IsSessionAlive() == false)
        //    Response.Redirect("../Index.aspx");
       // loginUserNo = 9999;
        if (!IsPostBack)
        {
            // Initialization code here

          //
            //Int64 registrationNo = Convert.ToInt64(txtRegNo.Text);
        }
    }
        protected void btnView_Click(object sender, EventArgs e)
        {

            try
            {
               
           
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message);
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
               
               // string registrationNo = txtRegNo.Text;
               // Int64 regNo = Convert.ToInt64(registrationNo);              
               // DateTime PRD= Convert.ToDateTime(txtPRD.Text );

               // string conString = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
               // SqlConnection sqlCon = new SqlConnection(conString);

               // SqlCommand cmd = new SqlCommand("CHMTProjectManualEntry", sqlCon);

               // cmd.CommandType = CommandType.StoredProcedure;
               // cmd.Parameters.AddWithValue("@regNo", regNo);            
               // cmd.Parameters.AddWithValue("@ProjectReceiptDate", PRD);
               // cmd.Parameters.AddWithValue("@user", loginUserNo);

               // SqlParameter msgOut = new SqlParameter("@msgOut", SqlDbType.VarChar,1000);
               // msgOut.Direction = ParameterDirection.Output;
               // cmd.Parameters.Add(msgOut);

               // sqlCon.Open();
               // cmd.ExecuteNonQuery();

               // string msg = cmd.Parameters["@msgOut"].Value as string;

               // if (string.IsNullOrEmpty(msg))
               // {
               //     ShowAlert("Project Receipt Date  for CHM(T) O  Level Project has been entered successfully.");
               // }
               // else
               // {
               //     lblError.Visible = true;
               //     lblError.Text = msg;
               // }
               //// txtPRD.Text = string.Empty;
               // sqlCon.Close();
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message);
            }

        }
}