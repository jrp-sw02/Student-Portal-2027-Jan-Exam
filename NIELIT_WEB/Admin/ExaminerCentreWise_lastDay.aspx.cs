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
using RestSharp;

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
               // DateTime myDateTime =  Convert.ToDateTime("2022-08-06");
              
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--  Select Batch  --", "0");
                var batchList = from p in context.PracticalCandidateMarks
                                where p.CenterCode.Replace(" ", "") == CentreCode.Replace(" ", "")
                                && p.ExamDate.Value.Year == myDateTime.Year
                                && p.ExamDate.Value.Month == myDateTime.Month
                              // Commented on 08022023 - RLakshmi-Vikas  && p.ExamDate.Value.Day == myDateTime.Day - 1
							 // Commented on 08022023 - RLakshmi-Vikasselect new {   ValueField = p.BatchCode,TextField = p.BatchCode };
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

        /* int rowIndex = Convert.ToInt32(e.CommandArgument);       
         GridViewRow row = grdCandidateData.Rows[rowIndex];     
         GridViewRow r1 = grdCandidateData.Rows[i];
        Label lblModuleId = null;
        lblModuleId = (Label)grdCandidateData.Rows.cells[]

         for (int i = 0; i <= grdCandidateData.Rows.Count - 1; i++)
         {
             int Id = (int)grdCandidateData.DataKeys[i].Values["INT_ID"];
         }

         int index = grdCandidateData.SelectedIndex;
         int rowIndex = ((sender as LinkButton).NamingContainer as GridViewRow).RowIndex;
         int id = Convert.ToInt32(grdCandidateData.DataKeys[rowIndex].Value);

         string id = grdCandidateData.DataKeys[5].Values[0].ToString();
         string ModuleId = ((HiddenField)row.FindControl("lblModuleId")).Value;

         int ModuleId = (int)grdCandidateData.DataKeys[5].Value;*/


          GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
          string  examinerMarks =  (row.FindControl("txExaminertMarks") as TextBox).Text;        
          Int64  registrationNumber =  Convert.ToInt64(row.Cells[2].Text);       
          Int64 moduleType = Convert.ToInt64(row.Cells[5].Text);
          try
          {
              using (var context = new EConnectContext())
              {                
                  var updateMarks =( from x in context.PracticalCandidateMarks
                                     where x.RegistrationNo == registrationNumber && x.ModuleId == moduleType
                                    select x).FirstOrDefault();
                  if (updateMarks.ExaminerMarks40 > 40)
                  {
                      
                  }
                  else
                  {
                      updateMarks.ExaminerMarks40 = Int32.Parse(examinerMarks);
                      updateMarks.ExaminerMarks40EntryDate = DateTime.Now;
                  }
                  

                  context.SaveChanges();                   
              }              
          }
          catch (Exception ex)
          {
              throw ex;
          }

        BindGridView();      
    }

    //protected void OnEdit(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
    //        (row.FindControl("txExaminertMarks") as TextBox).Enabled == true;
           
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    private DataTable GetData()
    {
        DataTable dt = new DataTable();

        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);  

        string batchCode = ddlBatch.SelectedValue;
        string sql = "PracticalExaminerMarksEntry_BatchWise";
    
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

    protected void grdCandidateData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {            
            String registrationNumber =   e.Row.Cells[2].Text;           
            DataRowView data = (DataRowView)e.Row.DataItem;           
            TextBox txtExaminerMarks = (TextBox)e.Row.FindControl("txExaminertMarks");
            //var value = EConnect.Utils.Data.DbUtility.ExecuteScaller("select  Examiner_marks_40 from  Practical_candidate_marks   where   Registration_no = '" + registrationNumber  + "'and   Batch_code = '"+ ddlBatch.SelectedValue +"'", new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
            //December_2024
            SqlParameter[] param1 = { new SqlParameter("@registrationNumber", registrationNumber),
                                        new SqlParameter("@batch", ddlBatch.SelectedValue)
                                                    };
            var value = EConnect.Utils.Data.DbUtility.ExecuteScaller("select  Examiner_marks_40 from  Practical_candidate_marks   where   Registration_no = @registrationNumber and   Batch_code = @batch", new EConnect.Connections.SqlCon(), param1, CommandType.Text, false);

            if (value != DBNull.Value)
            {
               //meters += Convert.ToDecimal(value);
                txtExaminerMarks.Enabled = false;
                txtExaminerMarks.Text = Convert.ToString(value);                                
            }                                   
        }
        //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Edit)
        //{
        //    TextBox txtExaminer = e.Row.FindControl("txExaminertMarks") as TextBox;
        //    txtExaminer.Enabled = true;
        //}    
    }


    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState == DataControlRowState.Edit)
        //{

        //   // GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
        //    grdCandidateData.EditIndex = e.NewEditIndex;
        //    TextBox txtExaminerMarks = (TextBox)e.Row.FindControl("txExaminertMarks");
        //    txtExaminerMarks.Enabled = false;
        //    BindGridView();
        //}
        ////this.BindGrid();    
        grdCandidateData.EditIndex = e.NewEditIndex;
        
    }

    protected void BindGridView()
    {       
        try
        {
             DataTable dt_result = GetData();
             grdCandidateData.DataSource = dt_result;
             grdCandidateData.DataBind();
             grdCandidateData.Visible = true;

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

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                string batchCode =  ddlBatch.SelectedValue ;                
                //Int64 candidatesCount = Convert.ToInt64(EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from Practical_candidate_marks   where Center_Code = 'UTTADEH01'  and Batch_code = '" + ddlBatch.SelectedValue + "'", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                Int64 candidatesCount = (from c in context.PracticalCandidateMarks
                                         where c.CenterCode.Replace(" ", "") == CentreCode && c.BatchCode == batchCode
                                         //&&  c.ExamMonth == 1  &&  c.ExamYear == 2023 
                                         select c).Count();
                
                trCandidatesCount.Visible = true;
                tdlbl.Visible = true;
                tdCandidatesCount.Visible = true;
                txtCount.Text = Convert.ToString(candidatesCount);
                BindGridView();
            }            
        }
        catch(Exception ex )
        {
            ShowAlert(ex.Message); 
        }
    }
}