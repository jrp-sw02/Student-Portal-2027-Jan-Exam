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

public partial class Admin_ObserverCentreWise : BasePage
{

    String CentreCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        CentreCode = (string)Session["CentreCode"];

        if (!IsPostBack)
        {
            BindGridView();

             
        }
       
    }


    protected void BindExamDate()
    {
        try
        {
           // int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            //BreadCrumb1.Render();
          //  lblNoRecord.Text = "";
            using (var context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
               // if (courseCatId > 0)
                //{

                // string  centreCode = "ARUNITA01";
                //var pracExamDate = from p in context.PracticalCandidateMarks
                //                   where p.CenterCode == centreCode
                //                   select new { ValueField = p.i, TextField = p.data_downloaded_sequence };

                    ////DataDownloadedSequence = DataDownloadedSequence.Distinct().Take(20);
                    //DataDownloadedSequence = DataDownloadedSequence.Distinct().OrderByDescending(a => a.TextField);
                    //EConnect.Utils.Common.ControlUtility.BindListObject(ddlDataDownloadedSequence, DataDownloadedSequence, lst);
              //  }
                //else
                //{
                //    ddlDataDownloadedSequence.Items.Clear();
                //    ddlDataDownloadedSequence.Items.Insert(0, lst);
                //}
                //ddlDataDownloadedSequence.SelectedValue = "0";
               // ddlDataDownloadedSequence_SelectedIndexChanged(ddlDataDownloadedSequence, EventArgs.Empty);
            };
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
        
        //int rowIndex = Convert.ToInt32(e.CommandArgument);

        //Reference the GridView Row.
      //  GridViewRow row = grdCandidateData.Rows[rowIndex];

        //Fetch value of Name.
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
        string observerMarks20 = (row.FindControl("txtObserver20") as TextBox).Text;



        //Fetch value of Country
          string registrationNumber = row.Cells[1].Text;
          string moduleType = row.Cells[4].Text;

          string sql = " update Practical_candidate_marks  set  Observer_marks_20 = '" + observerMarks20 + "'  where Registration_no = '" + registrationNumber + "' and module_id = '" + moduleType + "' ";
          SqlConnection conn = new SqlConnection(constr);
          SqlCommand cmd = new SqlCommand(sql);
          cmd.Connection = conn;
          conn.Open();
          cmd.ExecuteNonQuery();
          cmd.Dispose();

        //  SqlDataAdapter sda = new SqlDataAdapter(cmd);

       
      //  string name = (row.Cells[3].Controls[0] as TextBox).Text;
        // string country = (row.Cells[1].Controls[0] as TextBox).Text;
        DataTable dt = ViewState["dt"] as DataTable;
      //  dt.Rows[row.RowIndex]["txExaminertMarks"] = name;
        //dt.Rows[row.RowIndex]["Country"] = country;
        ViewState["dt"] = dt;
        grdCandidateData.EditIndex = -1;
        BindGridView();
    }

    private DataTable GetData()
    {
        DataTable dt = new DataTable();

        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
    //    string sql = "select distinct  d.Registration_Number Registration_Number ," +
    //" (select name from Candidate where id =d.Candidate_ID ) Candidate_Name, " +
    //" (select Short_Name  from Module where id=d.Module_ID ) Module_Short_Name	" +
    //" from  Course_Exam_Application_Detail  d where  Exam_ID  in (5745,5749,5751,5747)  and ( Fee_Amount =0 or Fee_Amount =500)" +
    //" and exists (select * from  Course_Exam_Application a where a.id=d.Course_Exam_Appl_ID and Pr_Office_Ref_No ='DELHNEW04' and a.Exam_ID =d.Exam_ID and " +
    //" a.Registration_Number =d.Registration_Number )";

        string sql = "select pm.Center_Code,Registration_no, upper( (select name from Candidate where id = (select top 1 Candidate_id from Registration_Detail r  "+ 
" where r.Registration_No =  pm.Registration_no order by Candidate_id desc  ))) Candidate_Name ," +
" Level_code,( select m.Short_Name  from Module m  where m.id =pm.module_id ) module_Short_Name  ,  module_id " +
//",Examiner_marks_40 ,Observer_marks_40 ,Observer_marks_20"+
       " from Practical_candidate_marks pm   inner  join  Prac_allowed_marks_entry pa  on    pm.Exam_date = pa.Exam_date and pm.Exam_report_time = pa.Exam_Session and pm.Center_Code=pa.Center_code where     " +
" pm.center_Code ='" + CentreCode + "'   and Examiner_marks_40 is not null   and  Observer_marks_40  is not  null   and Observer_marks_20  is null order by    pm.Exam_date, Exam_report_time, Registration_no ";


        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
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

             grdCandidateData.DataSource = dt_result;
             grdCandidateData.DataBind();
             grdCandidateData.Visible = true;

        }
        catch (Exception ex)
        {

            ShowAlert(ex.Message);
        }

    }
}