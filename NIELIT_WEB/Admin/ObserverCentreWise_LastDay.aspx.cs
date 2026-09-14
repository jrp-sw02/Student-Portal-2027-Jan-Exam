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

public partial class Admin_ObserverCentreWise : BasePage
{

    String CentreCode;
    protected void Page_Load(object sender, EventArgs e)
    {
       CentreCode = (string)Session["CentreCode"];
       // CentreCode = "UTTADEH01";
        if (IsSessionAlive() == false)
            Response.Redirect("../Index.aspx");
        if (!IsPostBack)
        {
            BindBatch();
        }     
    }
    protected void BindBatch()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                DateTime myDateTime = DateTime.Now;
              // DateTime myDateTime = Convert.ToDateTime("2022-08-06");

                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--  Select Batch  --", "0");
                var batchList = from p in context.PracticalCandidateMarks
                                where p.CenterCode.Replace(" ","") == CentreCode
                                && p.ExamDate.Value.Year == myDateTime.Year
                                && p.ExamDate.Value.Month == myDateTime.Month
                             // Commented on 08022023 - RLakshmi-Vikas   && p.ExamDate.Value.Day == myDateTime.Day - 1
                              // Commented on 08022023 - RLakshmi-Vikas  select new { ValueField = p.BatchCode, TextField = p.BatchCode };
							  && p.ExamDate.Value.Day < myDateTime.Day
                              select new { ValueField = p.BatchCode, TextField = p.BatchCode + " " + p.ExamDate.Value.Day + "-" + p.ExamDate.Value.Month + "-" + p.ExamDate.Value.Year };
                batchList = batchList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, batchList, lst);                            
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void OnUpdate(object sender, EventArgs e)
    {

        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
        string observerMarks = (row.FindControl("txtObserver40") as TextBox).Text;
        Int64 registrationNumber = Convert.ToInt64(row.Cells[2].Text);
        Int64 moduleType = Convert.ToInt64(row.Cells[5].Text);
        try
        {
            using (var context = new EConnectContext())
            {
                var updateMarks1 = (from x in context.PracticalCandidateMarks
                                    where x.RegistrationNo == registrationNumber && x.ModuleId == moduleType
                                    select x).FirstOrDefault();
                updateMarks1.ObserverMarks40 = Int32.Parse(observerMarks);
                updateMarks1.ObserverMarks40EntryDate = DateTime.Now;
                context.SaveChanges();
            }

            BindGridView();         

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            con.Close();
            con.Dispose();

        }       
    }

    private DataTable GetData()
    {
        DataTable dt = new DataTable();

        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
    
        string batchCode = ddlBatch.SelectedValue;
        string sql = "PracticalObserverMarksEntry_BatchWise";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@centreCode", SqlDbType.VarChar).Value = CentreCode;
                cmd.Parameters.Add("@batchCode", SqlDbType.VarChar).Value = batchCode;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
        }

        return dt;
    }

    protected void BindGridView()
    {
        try
        {
             DataTable dt_result = GetData();

             if (dt_result.Rows.Count == 0 )
                 //|| dt_result.Rows.Count != Convert.ToInt32(txtCount.Text))
             {
                 lblInfo.Visible = true;
                 lblInfo.Text = "Marks are not enterd  by the the  examiner for this  batch.";
                 grdCandidateData.Visible = false;
             }
             else
             {
                 lblInfo.Visible = false;
                 grdCandidateData.DataSource = dt_result;
                 grdCandidateData.DataBind();
                 grdCandidateData.Visible = true;
             }            
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void grdCandidateData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdCandidateData.PageIndex = e.NewPageIndex;
        grdCandidateData.SelectedIndex = -1;
        BindGridView();
    }
    protected void grdCandidateData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            String registrationNumber = e.Row.Cells[2].Text;
            DataRowView data = (DataRowView)e.Row.DataItem;
            TextBox txtObserver40 = (TextBox)e.Row.FindControl("txtObserver40");



            //Button btnSubmit = (Button)e.Row.FindControl("lnkOUpdate");

            //var value = EConnect.Utils.Data.DbUtility.ExecuteScaller("select  Observer_marks_40 from  Practical_candidate_marks where Registration_no = '" + registrationNumber + "'and   Batch_code = '" + ddlBatch.SelectedValue + "'", new EConnect.Connections.SqlCon(), null, CommandType.Text, false);

            //November_2024
            SqlParameter[] param1 = { new SqlParameter("@registrationNumber", registrationNumber),
                                      new SqlParameter("@batchCode", ddlBatch.SelectedValue)
                                        };
            var value = EConnect.Utils.Data.DbUtility.ExecuteScaller("select  Observer_marks_40 from  Practical_candidate_marks where Registration_no = @registrationNumber and   Batch_code = @batchCode ", new EConnect.Connections.SqlCon(), param1, CommandType.Text, false);

            if (value != DBNull.Value)
            {
                //meters += Convert.ToDecimal(value);
                txtObserver40.Enabled = false;
                txtObserver40.Text = Convert.ToString(value);
              //  btnSubmit.Enabled = false;

            }
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                string batchCode = ddlBatch.SelectedValue;
                //Int64 candidatesCount = Convert.ToInt64(EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from Practical_candidate_marks   where Center_Code = 'UTTADEH01'  and Batch_code = '" + ddlBatch.SelectedValue + "'", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                Int64 candidatesCount = (from c in context.PracticalCandidateMarks
                                         where c.CenterCode == CentreCode && c.BatchCode == batchCode 
                                         //&& c.ExamMonth == 1 && c.ExamYear == 2023
                                         select c).Count();

                trCandidatesCount.Visible = true;
                tdlbl.Visible = true;
                tdCandidatesCount.Visible = true;
                txtCount.Text = Convert.ToString(candidatesCount);
                BindGridView();
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

}