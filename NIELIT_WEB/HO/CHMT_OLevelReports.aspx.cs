using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.IO;
using EConnect.URM;
//using OfficeOpenXml;
using iTextSharp.text.html.simpleparser;

public partial class HO_CHMT_OLevelReports : BasePage
{
    Table tbl = new Table();
    string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 loginUserNo = 0;
    Int32 currentRoleId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Visible = false;
        lblError.Text = "";
        if (IsSessionAlive() == false)
            Response.Redirect("../Index.aspx");
        currentRoleId = Convert.ToInt32(Session["RoleID"]);
        loginUserNo = Convert.ToInt32(Session["UserID"]);
        if (!UserManager.HasRight(currentRoleId, enmRight.View))
        {
            Response.Write("Sorry! You don't have rights  to view this page");
            Response.End();
        }
        else
        {
                if (!IsPostBack)
                {
                    //BindExamDropdown();

                    //lblError.Visible = false;
                    //lblError.Text = "";
                   
                }
        }
    }

    private void BindExamDropdown()
    {

        try
        {
           
            string query = "  select distinct(e.name) as examName,e.id  as id  from Course_Exam_Application_Detail cead, exam e where e.Course_ID =1213 and cead.Course_ID = e.Course_ID and e.Result_Publish_Date is  null"+
                           " and  exists ( select   1  from   Course_Exam_Application_Detail cead  where   cead.Exam_ID = e.ID)";

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    // Execute the query and load data into a DataTable
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    // Bind the DataTable to the DropDownList
                    ddlExam.DataSource = dt;
                    ddlExam.DataTextField = "examName"; // Displayed text
                    ddlExam.DataValueField = "id";  // Value associated with the item
                    ddlExam.DataBind();

                    // Add the "--Select One--" item at the beginning
                    ddlExam.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select One--", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    private DataTable GetCompiledResult()
    {
         DataTable dt = new DataTable();
         Int64 examId = Convert.ToInt32(ddlExam.SelectedValue);
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("CHMT_OLevel_CompiledResult_Rep", con))
            {             
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@examId", examId);

                con.Open();

                using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                {
                    adpt.Fill(dt);
                }
            }
        }

        return dt;
    }
    private DataTable GetFinalizedResult()
    {
        DataTable dt = new DataTable();
        Int64 examId = Convert.ToInt32(ddlExam.SelectedValue);
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("CHMT_OLevel_FinalizedResult_Rep", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@examId", examId);

                con.Open();

                using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                {
                    adpt.Fill(dt);
                }
            }
        }

        return dt;
    }
    private DataTable GetCompiledResultMW()
    {
        DataTable dt = new DataTable();
        Int64 examId = Convert.ToInt32(ddlExam.SelectedValue);
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("CHMT_OLevel_Finalised_Result_ModuleWise_SessionWise_Rep", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id", examId);

                con.Open();

                using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                {
                    adpt.Fill(dt);
                }
            }
        }

        return dt;
    }
    private DataTable GetCompiledResultCW()
    {
        DataTable dt = new DataTable();
        Int64 examId = Convert.ToInt32(ddlExam.SelectedValue);
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("CHMT_OLevel_Finalised_Result_CenterWise_SessionWise_Rep", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id", examId);

                con.Open();

                using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                {
                    adpt.Fill(dt);
                }
            }
        }

        return dt;
    }
    private DataTable GetESResult( out  string message)
    {
        DataTable dt = new DataTable();
        message = string.Empty;
        Int64 examId = Convert.ToInt32(ddlExam.SelectedValue);
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("CHMT_OLevel_ES_Rep", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id", examId);

                SqlParameter outP = new SqlParameter("@MSG_OUT", SqlDbType.NVarChar,500);
                outP.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outP);

                con.Open();

                using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                {
                    adpt.Fill(dt);
                }

                message = cmd.Parameters["@MSG_OUT"].Value.ToString();
            }
        }

        return dt;
    }
    private DataTable GetSummarReportES( out  string  message)
    {
        DataTable dt = new DataTable();
        message = string.Empty;
        Int64 examId = Convert.ToInt32(ddlExam.SelectedValue);
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("CHMT_ES_Summary_Rep", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@exam_id", examId);

                SqlParameter outP = new SqlParameter("@MSG_OUT", SqlDbType.NVarChar, 500);
                outP.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outP);

                con.Open();

                using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                {
                    adpt.Fill(dt);
                }

                message = cmd.Parameters["@MSG_OUT"].Value.ToString();
            }
        }

        return dt;
    }

    protected void ShowTableHeader()
    {
        try
        {
         
            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);


            TableHeaderRow th1 = new TableHeaderRow();
            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.ColumnSpan = 11;
            tcCol11.Width = Unit.Percentage(18);
            tcCol11.Text = "CHM-T O LEVEL Compiled Result Report ";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tcCol11);
            tbl.Rows.Add(th1);


            TableHeaderRow t1 = new TableHeaderRow();
            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.ColumnSpan = 11;
            tcCol1.Width = Unit.Percentage(18);
            tcCol1.Text = "Exam Session: " + ddlExam.SelectedItem.Text;
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            t1.Cells.Add(tcCol1);
            tbl.Rows.Add(t1);

            TableHeaderRow t2 = new TableHeaderRow();
            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.ColumnSpan = 11;
            tcCol2.Width = Unit.Percentage(18);
            tcCol2.Text = "Report Date: " + DateTime.Now.ToString("dd MMM yyyy");
            tcCol2.HorizontalAlign = HorizontalAlign.Right;
            t2.Cells.Add(tcCol2);
            tbl.Rows.Add(t2);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell thc = new TableHeaderCell();
            thc.Width = Unit.Pixel(10);
            thc.HorizontalAlign = HorizontalAlign.Center;
            thc.Text = "SrNo.";
            th.Cells.Add(thc);

            TableHeaderCell thc1 = new TableHeaderCell();
            thc1.Width = Unit.Percentage(3);
            thc1.HorizontalAlign = HorizontalAlign.Center;
            thc1.Text = "Registration No";
            th.Cells.Add(thc1);

            TableHeaderCell thc2 = new TableHeaderCell();
            thc2.Width = Unit.Percentage(3);
            thc2.HorizontalAlign = HorizontalAlign.Center;
            thc2.Text = "Candidate Name";
            th.Cells.Add(thc2);

            TableHeaderCell thc3 = new TableHeaderCell();
            thc3.Width = Unit.Percentage(3);
            thc3.HorizontalAlign = HorizontalAlign.Center;
            thc3.Text = "Roll No";
            th.Cells.Add(thc3);

            TableHeaderCell thc4 = new TableHeaderCell();
            thc4.Width = Unit.Percentage(3);
            thc4.HorizontalAlign = HorizontalAlign.Center;
            thc4.Text = "Module Code";
            th.Cells.Add(thc4);
           

            TableHeaderCell thc7 = new TableHeaderCell();
            thc7.Width = Unit.Percentage(3);
            thc7.HorizontalAlign = HorizontalAlign.Center;
            thc7.Text = "Th. Wt.(60%)";
            th.Cells.Add(thc7);
           
            TableHeaderCell thc8 = new TableHeaderCell();
            thc8.Width = Unit.Percentage(3);
            thc8.HorizontalAlign = HorizontalAlign.Center;
            thc8.Text = "Pr Wt. (40%)";
            th.Cells.Add(thc8);
           

            TableHeaderCell thc10 = new TableHeaderCell();
            thc10.Width = Unit.Percentage(3);
            thc10.HorizontalAlign = HorizontalAlign.Center;
            thc10.Text = "Practical Marks(out of 100)";
            th.Cells.Add(thc10);

            TableHeaderCell thc11 = new TableHeaderCell();
            thc11.Width = Unit.Percentage(3);
            thc11.HorizontalAlign = HorizontalAlign.Center;
            thc11.Text = "Theory Marks(out of 100)";
            th.Cells.Add(thc11);
           

            TableHeaderCell thc13 = new TableHeaderCell();
            thc13.Width = Unit.Percentage(3);
            thc13.HorizontalAlign = HorizontalAlign.Center;
            thc13.Text = "Total Marks (Wt.)";
            th.Cells.Add(thc13);

            TableHeaderCell thc14 = new TableHeaderCell();
            thc14.Width = Unit.Percentage(3);
            thc14.HorizontalAlign = HorizontalAlign.Center;
            thc14.Text = "Centre Code";
            th.Cells.Add(thc14);


            tbl.Rows.Add(th);
            divReportData.Controls.Add(tbl);
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowTableHeaderFP()
    {
        try
        {

            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);


            TableHeaderRow th1 = new TableHeaderRow();
            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.ColumnSpan = 13;
            tcCol11.Width = Unit.Percentage(18);
            tcCol11.Text = "CHM-T O LEVEL Finalized Result Report ";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tcCol11);
            tbl.Rows.Add(th1);


            TableHeaderRow t1 = new TableHeaderRow();
            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.ColumnSpan = 13;
            tcCol1.Width = Unit.Percentage(18);
            tcCol1.Text = "Exam Session: " + ddlExam.SelectedItem.Text;
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            t1.Cells.Add(tcCol1);
            tbl.Rows.Add(t1);

            TableHeaderRow t2 = new TableHeaderRow();
            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.ColumnSpan = 13;
            tcCol2.Width = Unit.Percentage(18);
            tcCol2.Text = "Report Date: " + DateTime.Now.ToString("dd MMM yyyy");
            tcCol2.HorizontalAlign = HorizontalAlign.Right;
            t2.Cells.Add(tcCol2);
            tbl.Rows.Add(t2);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell thc = new TableHeaderCell();
            thc.Width = Unit.Pixel(10);
            thc.HorizontalAlign = HorizontalAlign.Center;
            thc.Text = "SrNo.";
            th.Cells.Add(thc);

            TableHeaderCell thc1 = new TableHeaderCell();
            thc1.Width = Unit.Percentage(3);
            thc1.HorizontalAlign = HorizontalAlign.Center;
            thc1.Text = "Registration No";
            th.Cells.Add(thc1);

            TableHeaderCell thc2 = new TableHeaderCell();
            thc2.Width = Unit.Percentage(3);
            thc2.HorizontalAlign = HorizontalAlign.Center;
            thc2.Text = "Candidate Name";
            th.Cells.Add(thc2);

            TableHeaderCell thc3 = new TableHeaderCell();
            thc3.Width = Unit.Percentage(3);
            thc3.HorizontalAlign = HorizontalAlign.Center;
            thc3.Text = "Roll No";
            th.Cells.Add(thc3);

            TableHeaderCell thc4 = new TableHeaderCell();
            thc4.Width = Unit.Percentage(3);
            thc4.HorizontalAlign = HorizontalAlign.Center;
            thc4.Text = "Module Code";
            th.Cells.Add(thc4);


            TableHeaderCell thc7 = new TableHeaderCell();
            thc7.Width = Unit.Percentage(3);
            thc7.HorizontalAlign = HorizontalAlign.Center;
            thc7.Text = "Th. Wt.(60%)";
            th.Cells.Add(thc7);

            TableHeaderCell thc8 = new TableHeaderCell();
            thc8.Width = Unit.Percentage(3);
            thc8.HorizontalAlign = HorizontalAlign.Center;
            thc8.Text = "Pr Wt. (40%)";
            th.Cells.Add(thc8);


            TableHeaderCell thc10 = new TableHeaderCell();
            thc10.Width = Unit.Percentage(3);
            thc10.HorizontalAlign = HorizontalAlign.Center;
            thc10.Text = "Practical Marks(out of 100)";
            th.Cells.Add(thc10);

            TableHeaderCell thc11 = new TableHeaderCell();
            thc11.Width = Unit.Percentage(3);
            thc11.HorizontalAlign = HorizontalAlign.Center;
            thc11.Text = "Theory Marks(out of 100)";
            th.Cells.Add(thc11);


            TableHeaderCell thc13 = new TableHeaderCell();
            thc13.Width = Unit.Percentage(3);
            thc13.HorizontalAlign = HorizontalAlign.Center;
            thc13.Text = "Total Marks (Wt.)";
            th.Cells.Add(thc13);

            TableHeaderCell thc15 = new TableHeaderCell();
            thc15.Width = Unit.Percentage(3);
            thc15.HorizontalAlign = HorizontalAlign.Center;
            thc15.Text = "Grade";
            th.Cells.Add(thc15);

            TableHeaderCell thc16 = new TableHeaderCell();
            thc16.Width = Unit.Percentage(3);
            thc16.HorizontalAlign = HorizontalAlign.Center;
            thc16.Text = "Result";
            th.Cells.Add(thc16);

            TableHeaderCell thc14 = new TableHeaderCell();
            thc14.Width = Unit.Percentage(3);
            thc14.HorizontalAlign = HorizontalAlign.Center;
            thc14.Text = "Centre Code";
            th.Cells.Add(thc14);


            tbl.Rows.Add(th);
            divReportData.Controls.Add(tbl);
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowTableHeaderMW()
    {
        try
        {

            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);


            TableHeaderRow th1 = new TableHeaderRow();
            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.ColumnSpan = 13;
            tcCol11.Width = Unit.Percentage(18);
            tcCol11.Text = "CHM-T O LEVEL Finalized Result Report Module Wise ";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tcCol11);
            tbl.Rows.Add(th1);


            TableHeaderRow t1 = new TableHeaderRow();
            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.ColumnSpan = 13;
            tcCol1.Width = Unit.Percentage(18);
            tcCol1.Text = "Exam Session : " + ddlExam.SelectedItem.Text;
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            t1.Cells.Add(tcCol1);
            tbl.Rows.Add(t1);


            TableHeaderRow t2 = new TableHeaderRow();
            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.ColumnSpan = 13;
            tcCol2.Width = Unit.Percentage(18);
            tcCol2.Text = "Report Date: " + DateTime.Now.ToString("dd MMM yyyy");
            tcCol2.HorizontalAlign = HorizontalAlign.Right;
            t2.Cells.Add(tcCol2);
            tbl.Rows.Add(t2);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell thc = new TableHeaderCell();
            thc.Width = Unit.Percentage(1);
            thc.HorizontalAlign = HorizontalAlign.Center;
            thc.Text = "SrNo.";
            th.Cells.Add(thc);

            TableHeaderCell thc1 = new TableHeaderCell();
            thc1.Width = Unit.Percentage(3);
            thc1.HorizontalAlign = HorizontalAlign.Center;
            thc1.Text = "Module Code";
            th.Cells.Add(thc1);

            TableHeaderCell tc11 = new TableHeaderCell();
            tc11.Width = Unit.Percentage(3);
            tc11.HorizontalAlign = HorizontalAlign.Center;
            tc11.Text = "S";
            th.Cells.Add(tc11);

            TableHeaderCell tc2 = new TableHeaderCell();
            tc2.Width = Unit.Percentage(3);
            tc2.HorizontalAlign = HorizontalAlign.Center;
            tc2.Text = "A";
            th.Cells.Add(tc2);


            TableHeaderCell tc3 = new TableHeaderCell();
            tc3.Width = Unit.Percentage(3);
            tc3.HorizontalAlign = HorizontalAlign.Center;
            tc3.Text = "B";
            th.Cells.Add(tc3);

            TableHeaderCell tc4 = new TableHeaderCell();
            tc4.Width = Unit.Percentage(3);
            tc4.HorizontalAlign = HorizontalAlign.Center;
            tc4.Text = "C";
            th.Cells.Add(tc4);

            TableHeaderCell tc5 = new TableHeaderCell();
            tc5.Width = Unit.Percentage(3);
            tc5.HorizontalAlign = HorizontalAlign.Center;
            tc5.Text = "D";
            th.Cells.Add(tc5);

            TableHeaderCell tc6 = new TableHeaderCell();
            tc6.Width = Unit.Percentage(3);
            tc6.HorizontalAlign = HorizontalAlign.Center;
            tc6.Text = "F";
            th.Cells.Add(tc6);

            TableHeaderCell tc7 = new TableHeaderCell();
            tc7.Width = Unit.Percentage(3);
            tc7.HorizontalAlign = HorizontalAlign.Center;
            tc7.Text = "ABS";
            th.Cells.Add(tc7);
           
            TableHeaderCell thc2 = new TableHeaderCell();
            thc2.Width = Unit.Percentage(3);
            thc2.HorizontalAlign = HorizontalAlign.Center;
            thc2.Text = "Applied";
            th.Cells.Add(thc2);

            TableHeaderCell thc3 = new TableHeaderCell();
            thc3.Width = Unit.Percentage(3);
            thc3.HorizontalAlign = HorizontalAlign.Center;
            thc3.Text = "Appeared";
            th.Cells.Add(thc3);

            TableHeaderCell thc4 = new TableHeaderCell();
            thc4.Width = Unit.Percentage(3);
            thc4.HorizontalAlign = HorizontalAlign.Center;
            thc4.Text = "Passed";
            th.Cells.Add(thc4);

            TableHeaderCell thc5 = new TableHeaderCell();
            thc5.Width = Unit.Percentage(3);
            thc5.HorizontalAlign = HorizontalAlign.Center;
            thc5.Text = "Pass(%)";
            th.Cells.Add(thc5);

            tbl.Rows.Add(th);
            divReportData.Controls.Add(tbl);
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowTableHeaderCW()
    {
        try
        {

            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);

            TableHeaderRow th1 = new TableHeaderRow();
            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.ColumnSpan = 8;
            tcCol11.Width = Unit.Percentage(18);
            tcCol11.Text = "CHM-T O LEVEL Finalized Result Report Centre Wise ";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tcCol11);
            tbl.Rows.Add(th1);

            TableHeaderRow t1 = new TableHeaderRow();
            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.ColumnSpan = 8;
            tcCol1.Width = Unit.Percentage(18);
            tcCol1.Text = "Exam Session : " + ddlExam.SelectedItem.Text;
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            t1.Cells.Add(tcCol1);
            tbl.Rows.Add(t1);

            TableHeaderRow t2 = new TableHeaderRow();
            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.ColumnSpan = 11;
            tcCol2.Width = Unit.Percentage(18);
            tcCol2.Text = "Report Date: " + DateTime.Now.ToString("dd MMM yyyy");
            tcCol2.HorizontalAlign = HorizontalAlign.Right;
            t2.Cells.Add(tcCol2);
            tbl.Rows.Add(t2);


            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell thc = new TableHeaderCell();
            thc.Width = Unit.Percentage(1);
            thc.HorizontalAlign = HorizontalAlign.Center;
            thc.Text = "SrNo.";
            th.Cells.Add(thc);

            TableHeaderCell thc1 = new TableHeaderCell();
            thc1.Width = Unit.Percentage(3);
            thc1.HorizontalAlign = HorizontalAlign.Center;
            thc1.Text = "Centre Code";
            th.Cells.Add(thc1);

            TableHeaderCell thc2 = new TableHeaderCell();
            thc2.Width = Unit.Percentage(3);
            thc2.HorizontalAlign = HorizontalAlign.Center;
            thc2.Text = "Applied";
            th.Cells.Add(thc2);

            TableHeaderCell thc3 = new TableHeaderCell();
            thc3.Width = Unit.Percentage(3);
            thc3.HorizontalAlign = HorizontalAlign.Center;
            thc3.Text = "Appeared";
            th.Cells.Add(thc3);

            TableHeaderCell thc4 = new TableHeaderCell();
            thc4.Width = Unit.Percentage(3);
            thc4.HorizontalAlign = HorizontalAlign.Center;
            thc4.Text = "Passed";
            th.Cells.Add(thc4);

            TableHeaderCell thc5 = new TableHeaderCell();
            thc5.Width = Unit.Percentage(3);
            thc5.HorizontalAlign = HorizontalAlign.Center;
            thc5.Text = "Failed";
            th.Cells.Add(thc5);

            TableHeaderCell thc6 = new TableHeaderCell();
            thc6.Width = Unit.Percentage(3);
            thc6.HorizontalAlign = HorizontalAlign.Center;
            thc6.Text = "Absent";
            th.Cells.Add(thc6);

            TableHeaderCell thc7 = new TableHeaderCell();
            thc7.Width = Unit.Percentage(3);
            thc7.HorizontalAlign = HorizontalAlign.Center;
            thc7.Text = "Pass(%)";
            th.Cells.Add(thc7);

            tbl.Rows.Add(th);
            //divReportData.Controls.Add(tbl);
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowTableHeaderES()
    {
        try
        {


             string  message;
             DataTable getResult = GetESResult (out message);
            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);

            TableHeaderRow th1 = new TableHeaderRow();
            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.ColumnSpan = 7;
            tcCol11.Width = Unit.Percentage(18);
            tcCol11.Text = "CHM-T O LEVEL Employability Skill Result Report ";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tcCol11);
            tbl.Rows.Add(th1);

            TableHeaderRow t1 = new TableHeaderRow();
            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.ColumnSpan = 7;
            tcCol1.Width = Unit.Percentage(18);
            tcCol1.Text = "Exam Session : " + message;
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            t1.Cells.Add(tcCol1);
            tbl.Rows.Add(t1);

            TableHeaderRow t2 = new TableHeaderRow();
            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.ColumnSpan = 11;
            tcCol2.Width = Unit.Percentage(18);
            tcCol2.Text = "Report Date: " + DateTime.Now.ToString("dd MMM yyyy");
            tcCol2.HorizontalAlign = HorizontalAlign.Right;
            t2.Cells.Add(tcCol2);
            tbl.Rows.Add(t2);


            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell thc = new TableHeaderCell();
            thc.Width = Unit.Percentage(1);
            thc.HorizontalAlign = HorizontalAlign.Center;
            thc.Text = "SrNo.";
            th.Cells.Add(thc);

            TableHeaderCell thc1 = new TableHeaderCell();
            thc1.Width = Unit.Percentage(3);
            thc1.HorizontalAlign = HorizontalAlign.Center;
            thc1.Text = "Registration No";
            th.Cells.Add(thc1);

            TableHeaderCell thc2 = new TableHeaderCell();
            thc2.Width = Unit.Percentage(3);
            thc2.HorizontalAlign = HorizontalAlign.Center;
            thc2.Text = "Candidate Name";
            th.Cells.Add(thc2);

            TableHeaderCell thc3 = new TableHeaderCell();
            thc3.Width = Unit.Percentage(3);
            thc3.HorizontalAlign = HorizontalAlign.Center;
            thc3.Text = "Module Code ";
            th.Cells.Add(thc3);

            TableHeaderCell thc4 = new TableHeaderCell();
            thc4.Width = Unit.Percentage(3);
            thc4.HorizontalAlign = HorizontalAlign.Center;
            thc4.Text = "Exam Date";
            th.Cells.Add(thc4);

            TableHeaderCell thc5 = new TableHeaderCell();
            thc5.Width = Unit.Percentage(3);
            thc5.HorizontalAlign = HorizontalAlign.Center;
            thc5.Text = "Total Marks";
            th.Cells.Add(thc5);

            TableHeaderCell thc6 = new TableHeaderCell();
            thc6.Width = Unit.Percentage(3);
            thc6.HorizontalAlign = HorizontalAlign.Center;
            thc6.Text = "Result";
            th.Cells.Add(thc6);

            //TableHeaderCell thc7 = new TableHeaderCell();
            //thc7.Width = Unit.Percentage(3);
            //thc7.HorizontalAlign = HorizontalAlign.Center;
            //thc7.Text = "Pass(%)";
            //th.Cells.Add(thc7);

            tbl.Rows.Add(th);
            divReportData.Controls.Add(tbl);
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ShowTableHeaderSummaryES()
    {
        try
        {


            string message = string.Empty;
            DataTable getSummaryRep = GetSummarReportES( out message);
            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);


            TableHeaderRow th1 = new TableHeaderRow();
            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.ColumnSpan = 13;
            tcCol11.Width = Unit.Percentage(18);
            tcCol11.Text = "CHM-T O LEVEL Employablity Skill Summary Report ";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tcCol11);
            tbl.Rows.Add(th1);


            TableHeaderRow t1 = new TableHeaderRow();
            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.ColumnSpan = 13;
            tcCol1.Width = Unit.Percentage(18);
            tcCol1.Text = "Exam Session: " + message;
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            t1.Cells.Add(tcCol1);
            tbl.Rows.Add(t1);

            TableHeaderRow t2 = new TableHeaderRow();
            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.ColumnSpan = 13;
            tcCol2.Width = Unit.Percentage(18);
            tcCol2.Text = "Report Date: " + DateTime.Now.ToString("dd MMM yyyy");
            tcCol2.HorizontalAlign = HorizontalAlign.Right;
            t2.Cells.Add(tcCol2);
            tbl.Rows.Add(t2);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell thc = new TableHeaderCell();
            thc.Width = Unit.Pixel(10);
            thc.HorizontalAlign = HorizontalAlign.Center;
            thc.Text = "SrNo.";
            th.Cells.Add(thc);

            TableHeaderCell thc1 = new TableHeaderCell();
            thc1.Width = Unit.Percentage(3);
            thc1.HorizontalAlign = HorizontalAlign.Center;
            thc1.Text = "Module Code";
            th.Cells.Add(thc1);

            TableHeaderCell thc2 = new TableHeaderCell();
            thc2.Width = Unit.Percentage(3);
            thc2.HorizontalAlign = HorizontalAlign.Center;
            thc2.Text = "Grade S";
            th.Cells.Add(thc2);

            TableHeaderCell thc3 = new TableHeaderCell();
            thc3.Width = Unit.Percentage(3);
            thc3.HorizontalAlign = HorizontalAlign.Center;
            thc3.Text = "Grade A";
            th.Cells.Add(thc3);

            TableHeaderCell thc4 = new TableHeaderCell();
            thc4.Width = Unit.Percentage(3);
            thc4.HorizontalAlign = HorizontalAlign.Center;
            thc4.Text = "Grade B";
            th.Cells.Add(thc4);


            TableHeaderCell thc7 = new TableHeaderCell();
            thc7.Width = Unit.Percentage(3);
            thc7.HorizontalAlign = HorizontalAlign.Center;
            thc7.Text = "Grade C";
            th.Cells.Add(thc7);

            TableHeaderCell thc8 = new TableHeaderCell();
            thc8.Width = Unit.Percentage(3);
            thc8.HorizontalAlign = HorizontalAlign.Center;
            thc8.Text = "Grade D";
            th.Cells.Add(thc8);


            TableHeaderCell thc10 = new TableHeaderCell();
            thc10.Width = Unit.Percentage(3);
            thc10.HorizontalAlign = HorizontalAlign.Center;
            thc10.Text = "Grade F";
            th.Cells.Add(thc10);

            TableHeaderCell thc11 = new TableHeaderCell();
            thc11.Width = Unit.Percentage(3);
            thc11.HorizontalAlign = HorizontalAlign.Center;
            thc11.Text = "Absent";
            th.Cells.Add(thc11);


            TableHeaderCell thc13 = new TableHeaderCell();
            thc13.Width = Unit.Percentage(3);
            thc13.HorizontalAlign = HorizontalAlign.Center;
            thc13.Text = "Applied";
            th.Cells.Add(thc13);

            TableHeaderCell thc15 = new TableHeaderCell();
            thc15.Width = Unit.Percentage(3);
            thc15.HorizontalAlign = HorizontalAlign.Center;
            thc15.Text = "Appeared";
            th.Cells.Add(thc15);

            TableHeaderCell thc16 = new TableHeaderCell();
            thc16.Width = Unit.Percentage(3);
            thc16.HorizontalAlign = HorizontalAlign.Center;
            thc16.Text = "Pass";
            th.Cells.Add(thc16);

            TableHeaderCell thc14 = new TableHeaderCell();
            thc14.Width = Unit.Percentage(3);
            thc14.HorizontalAlign = HorizontalAlign.Center;
            thc14.Text = "Pass(%)";
            th.Cells.Add(thc14);


            tbl.Rows.Add(th);
            divReportData.Controls.Add(tbl);
        }
       
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowData()
    {
       // int i = 0;

       // DataTable dt1 = GetCompiledResult();
        try
        {
            if (ddlReport.SelectedValue == "1")
            {

                using (DataTable dt1 = GetCompiledResult())
                {
                    if (dt1.Rows.Count > 0)
                    {
                        var application = (from a in dt1.AsEnumerable()
                                           select new
                                           {
                                               Registration_Number = a.Field<Int64?>("Registration_Number") ?? 0,
                                               Candidate_Name = a.Field<string>("Candidate_Name") ?? "",
                                               Roll_Number = a.Field<Int64?>("Roll_Number") ?? 0,
                                               Short_Name = a.Field<string>("Short_Name") ?? "",
                                               marks_objective = a.Field<Decimal?>("marks_objective") ?? 0,
                                               marks_descriptive = a.Field<Decimal?>("marks_descriptive") ?? 0,
                                               practical_marks_out_of_100 = a.Field<Decimal?>("practical_marks_out_of_100") ?? 0,
                                               theory_marks_out_of_100 = a.Field<Decimal?>("theory_marks_out_of_100") ?? 0,
                                               total_marks = a.Field<Decimal?>("total_marks") ?? 0,
                                               Venue_Code = a.Field<string>("Venue_Code") ?? "",

                                           }).ToList();

                        if (application.Count() > 0)
                        {

                            ShowTableHeader();

                            int i = 0;

                            //  if (dt1.Rows.Count > 0)
                            // {
                            foreach (var app in application)
                            {
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;

                                TableHeaderCell thc = new TableHeaderCell();
                                thc.Width = Unit.Percentage(1);
                                thc.HorizontalAlign = HorizontalAlign.Center;
                                thc.Text = i.ToString();
                                tr.Cells.Add(thc);

                                TableCell thc1 = new TableCell();
                                thc1.Width = Unit.Percentage(3);
                                thc1.HorizontalAlign = HorizontalAlign.Center;
                                thc1.Text = app.Registration_Number.ToString();
                                tr.Cells.Add(thc1);

                                TableCell thc2 = new TableCell();
                                thc2.Width = Unit.Percentage(3);
                                thc2.HorizontalAlign = HorizontalAlign.Center;
                                thc2.Text = app.Candidate_Name.ToString();
                                tr.Cells.Add(thc2);

                                TableCell thc3 = new TableCell();
                                thc3.Width = Unit.Percentage(3);
                                thc3.HorizontalAlign = HorizontalAlign.Center;
                                thc3.Text = app.Roll_Number.ToString();
                                tr.Cells.Add(thc3);

                                TableCell thc4 = new TableCell();
                                thc4.Width = Unit.Percentage(3);
                                thc4.HorizontalAlign = HorizontalAlign.Center;
                                thc4.Text = app.Short_Name.ToString();
                                tr.Cells.Add(thc4);

                                TableCell thc6 = new TableCell();
                                thc6.Width = Unit.Percentage(3);
                                thc6.HorizontalAlign = HorizontalAlign.Center;
                                thc6.Text = app.marks_objective.ToString();
                                tr.Cells.Add(thc6);

                                TableCell thc7 = new TableCell();
                                thc7.Width = Unit.Percentage(3);
                                thc7.HorizontalAlign = HorizontalAlign.Center;
                                thc7.Text = app.marks_descriptive.ToString();
                                tr.Cells.Add(thc7);

                                TableCell thc9 = new TableCell();
                                thc9.Width = Unit.Percentage(3);
                                thc9.HorizontalAlign = HorizontalAlign.Center;
                                thc9.Text = app.practical_marks_out_of_100.ToString();
                                tr.Cells.Add(thc9);
                                ////total_marks
                                TableCell thc10 = new TableCell();
                                thc10.Width = Unit.Percentage(3);
                                thc10.HorizontalAlign = HorizontalAlign.Center;
                                thc10.Text = app.theory_marks_out_of_100.ToString();
                                tr.Cells.Add(thc10);

                                TableCell thc12 = new TableCell();
                                thc12.Width = Unit.Percentage(3);
                                thc12.HorizontalAlign = HorizontalAlign.Center;
                                thc12.Text = app.total_marks.ToString();
                                tr.Cells.Add(thc12);

                                TableCell thc14 = new TableCell();
                                thc14.Width = Unit.Percentage(3);
                                thc14.HorizontalAlign = HorizontalAlign.Center;
                                thc14.Text = app.Venue_Code.ToString();
                                tr.Cells.Add(thc14);

                                tbl.Rows.Add(tr);
                                divReportData.Controls.Add(tbl);
                               // lblError.Visible = false;
                            }
                        }
                        //else
                        //{
                        //    lblError.Visible = true;
                        //    lblError.Text = "No Record Found";
                        //}
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }
                }

            }

            if (ddlReport.SelectedValue == "2")
            {
                using (DataTable dt1 = GetCompiledResultMW())
                {
                    if (dt1.Rows.Count > 0)
                    {
                        var application = (from a in dt1.AsEnumerable()
                                           select new
                                           {
                                               Short_Name = a.Field<String>("Short_Name") ?? "",

                                               S = a.Field<Int32?>("S") ?? 0,
                                               A = a.Field<Int32?>("A") ?? 0,
                                               B = a.Field<Int32?>("B") ?? 0,
                                               C = a.Field<Int32?>("C") ?? 0,
                                               D = a.Field<Int32?>("D") ?? 0,
                                               F = a.Field<Int32?>("F") ?? 0,
                                               absent = a.Field<Int32?>("absent_") ?? 0,
                                               Applied = a.Field<Int32?>("Applied") ?? 0,
                                               Apeared = a.Field<Int32?>("Apeared") ?? 0,
                                               passed = a.Field<Int32?>("passed") ?? 0,
                                               pass_percent = a.Field<Decimal?>("pass_percent") ?? 0,

                                           }).ToList();

                        if (application.Count() > 0)
                        {

                            ShowTableHeaderMW();

                            int i = 0;

                            //  if (dt1.Rows.Count > 0)
                            // {
                            foreach (var app in application)
                            {
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;

                                TableHeaderCell thc = new TableHeaderCell();
                                thc.Width = Unit.Percentage(1);
                                thc.HorizontalAlign = HorizontalAlign.Center;
                                thc.Text = i.ToString();
                                tr.Cells.Add(thc);

                                TableCell thc1 = new TableCell();
                                thc1.Width = Unit.Percentage(3);
                                thc1.HorizontalAlign = HorizontalAlign.Center;
                                thc1.Text = app.Short_Name.ToString();
                                tr.Cells.Add(thc1);

                                TableCell thr1 = new TableCell();
                                thr1.Width = Unit.Percentage(3);
                                thr1.HorizontalAlign = HorizontalAlign.Center;
                                thr1.Text = app.S.ToString();
                                tr.Cells.Add(thr1);

                                TableCell thr2 = new TableCell();
                                thr2.Width = Unit.Percentage(3);
                                thr2.HorizontalAlign = HorizontalAlign.Center;
                                thr2.Text = app.A.ToString();
                                tr.Cells.Add(thr2);

                                TableCell thr3 = new TableCell();
                                thr3.Width = Unit.Percentage(3);
                                thr3.HorizontalAlign = HorizontalAlign.Center;
                                thr3.Text = app.B.ToString();
                                tr.Cells.Add(thr3);

                                TableCell thr4 = new TableCell();
                                thr4.Width = Unit.Percentage(3);
                                thr4.HorizontalAlign = HorizontalAlign.Center;
                                thr4.Text = app.C.ToString();
                                tr.Cells.Add(thr4);

                                TableCell thr5 = new TableCell();
                                thr5.Width = Unit.Percentage(3);
                                thr5.HorizontalAlign = HorizontalAlign.Center;
                                thr5.Text = app.D.ToString();
                                tr.Cells.Add(thr5);

                                TableCell thr6 = new TableCell();
                                thr6.Width = Unit.Percentage(3);
                                thr6.HorizontalAlign = HorizontalAlign.Center;
                                thr6.Text = app.F.ToString();
                                tr.Cells.Add(thr6);

                                TableCell thr7 = new TableCell();
                                thr7.Width = Unit.Percentage(3);
                                thr7.HorizontalAlign = HorizontalAlign.Center;
                                thr7.Text = app.absent.ToString();
                                tr.Cells.Add(thr7);

                                TableCell thc2 = new TableCell();
                                thc2.Width = Unit.Percentage(3);
                                thc2.HorizontalAlign = HorizontalAlign.Center;
                                thc2.Text = app.Applied.ToString();
                                tr.Cells.Add(thc2);

                                TableCell thc3 = new TableCell();
                                thc3.Width = Unit.Percentage(3);
                                thc3.HorizontalAlign = HorizontalAlign.Center;
                                thc3.Text = app.Apeared.ToString();
                                tr.Cells.Add(thc3);

                                TableCell thc4 = new TableCell();
                                thc4.Width = Unit.Percentage(3);
                                thc4.HorizontalAlign = HorizontalAlign.Center;
                                thc4.Text = app.passed.ToString();
                                tr.Cells.Add(thc4);

                                TableCell thc5 = new TableCell();
                                thc5.Width = Unit.Percentage(3);
                                thc5.HorizontalAlign = HorizontalAlign.Center;
                                thc5.Text = app.pass_percent.ToString();
                                tr.Cells.Add(thc5);    

                                tbl.Rows.Add(tr);
                                divReportData.Controls.Add(tbl);
                               // lblError.Visible = false;
                            }
                        }
                        //else
                        //{
                        //    lblError.Visible = true;
                        //    lblError.Text = "No Record Found";
                        //}
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }
                }
            }




            if (ddlReport.SelectedValue == "3")
            {
                using (DataTable dt1 = GetCompiledResultCW())
                {
                    if (dt1.Rows.Count > 0)
                    {
                        var application = (from a in dt1.AsEnumerable()
                                           select new
                                           {
                                               Venue_Code = a.Field<String>("Venue_Code") ?? "",
                                               Applied = a.Field<Int32?>("Applied") ?? 0,
                                               Apeared = a.Field<Int32?>("Apeared") ?? 0,
                                               passed = a.Field<Int32?>("passed") ?? 0,
                                               failed = a.Field<Int32?>("Failed") ?? 0,
                                               absent = a.Field<Int32?>("absent_") ?? 0,
                                               pass_percent = a.Field<Decimal?>("pass_percent") ?? 0,

                                           }).ToList();

                        if (application.Count() > 0)
                        {

                            ShowTableHeaderCW();

                            int i = 0;

                            //  if (dt1.Rows.Count > 0)
                            // {
                            foreach (var app in application)
                            {
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;

                                TableHeaderCell thc = new TableHeaderCell();
                                thc.Width = Unit.Percentage(1);
                                thc.HorizontalAlign = HorizontalAlign.Center;
                                thc.Text = i.ToString();
                                tr.Cells.Add(thc);

                                TableCell thc1 = new TableCell();
                                thc1.Width = Unit.Percentage(3);
                                thc1.HorizontalAlign = HorizontalAlign.Center;
                                thc1.Text = app.Venue_Code.ToString();
                                tr.Cells.Add(thc1);

                                TableCell thc2 = new TableCell();
                                thc2.Width = Unit.Percentage(3);
                                thc2.HorizontalAlign = HorizontalAlign.Center;
                                thc2.Text = app.Applied.ToString();
                                tr.Cells.Add(thc2);

                                TableCell thc3 = new TableCell();
                                thc3.Width = Unit.Percentage(3);
                                thc3.HorizontalAlign = HorizontalAlign.Center;
                                thc3.Text = app.Apeared.ToString();
                                tr.Cells.Add(thc3);

                                TableCell thc4 = new TableCell();
                                thc4.Width = Unit.Percentage(3);
                                thc4.HorizontalAlign = HorizontalAlign.Center;
                                thc4.Text = app.passed.ToString();
                                tr.Cells.Add(thc4);

                                TableCell thc5 = new TableCell();
                                thc5.Width = Unit.Percentage(3);
                                thc5.HorizontalAlign = HorizontalAlign.Center;
                                thc5.Text = app.failed.ToString();
                                tr.Cells.Add(thc5);

                                TableCell thc6 = new TableCell();
                                thc6.Width = Unit.Percentage(3);
                                thc6.HorizontalAlign = HorizontalAlign.Center;
                                thc6.Text = app.absent.ToString();
                                tr.Cells.Add(thc6);

                                TableCell thc7 = new TableCell();
                                thc7.Width = Unit.Percentage(3);
                                thc7.HorizontalAlign = HorizontalAlign.Center;
                                thc7.Text = app.pass_percent.ToString();
                                tr.Cells.Add(thc7);

                                tbl.Rows.Add(tr);
                                divReportData.Controls.Add(tbl);
                                //lblError.Visible = false;
                            }
                        }
                        //else
                        //{
                        //    lblError.Visible = true;
                        //    lblError.Text = "No Record Found";
                        //}
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }
                }
            }



            if (ddlReport.SelectedValue == "4")
            {

                using (DataTable dt1 = GetFinalizedResult())
                {
                    if (dt1.Rows.Count > 0)
                    {
                        var application = (from a in dt1.AsEnumerable()
                                           select new
                                           {
                                               Registration_Number = a.Field<Int64?>("Registration_Number") ?? 0,
                                               Candidate_Name = a.Field<string>("Candidate_Name") ?? "",
                                               Roll_Number = a.Field<Int64?>("Roll_Number") ?? 0,
                                               Short_Name = a.Field<string>("Short_Name") ?? "",
                                               marks_objective = a.Field<Decimal?>("marks_objective") ,
                                               marks_descriptive = a.Field<Decimal?>("marks_descriptive") ,
                                               practical_marks_out_of_100 = a.Field<Decimal?>("practical_marks_out_of_100") ,
                                               theory_marks_out_of_100 = a.Field<Decimal?>("theory_marks_out_of_100") ,
                                               total_marks = a.Field<Decimal?>("total_marks") ,
                                               grade_code = a.Field<string>("grade_code") ?? "",
                                               result = a.Field<string>("result") ?? "",
                                               Venue_Code = a.Field<string>("Venue_Code") ?? "",

                                           }).ToList();

                        if (application.Count() > 0)
                        {

                            ShowTableHeaderFP();

                            int i = 0;

                            //  if (dt1.Rows.Count > 0)
                            // {
                            foreach (var app in application)
                            {
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;

                                TableHeaderCell thc = new TableHeaderCell();
                                thc.Width = Unit.Percentage(1);
                                thc.HorizontalAlign = HorizontalAlign.Center;
                                thc.Text = i.ToString();
                                tr.Cells.Add(thc);

                                TableCell thc1 = new TableCell();
                                thc1.Width = Unit.Percentage(3);
                                thc1.HorizontalAlign = HorizontalAlign.Center;
                                thc1.Text = app.Registration_Number.ToString();
                                tr.Cells.Add(thc1);

                                TableCell thc2 = new TableCell();
                                thc2.Width = Unit.Percentage(3);
                                thc2.HorizontalAlign = HorizontalAlign.Center;
                                thc2.Text = app.Candidate_Name.ToString();
                                tr.Cells.Add(thc2);

                                TableCell thc3 = new TableCell();
                                thc3.Width = Unit.Percentage(3);
                                thc3.HorizontalAlign = HorizontalAlign.Center;
                                thc3.Text = app.Roll_Number.ToString();
                                tr.Cells.Add(thc3);

                                TableCell thc4 = new TableCell();
                                thc4.Width = Unit.Percentage(3);
                                thc4.HorizontalAlign = HorizontalAlign.Center;
                                thc4.Text = app.Short_Name.ToString();
                                tr.Cells.Add(thc4);

                                TableCell thc6 = new TableCell();
                                thc6.Width = Unit.Percentage(3);
                                thc6.HorizontalAlign = HorizontalAlign.Center;
                                thc6.Text = app.marks_objective.ToString();
                                tr.Cells.Add(thc6);

                                TableCell thc7 = new TableCell();
                                thc7.Width = Unit.Percentage(3);
                                thc7.HorizontalAlign = HorizontalAlign.Center;
                                thc7.Text = app.marks_descriptive.ToString();
                                tr.Cells.Add(thc7);

                                TableCell thc9 = new TableCell();
                                thc9.Width = Unit.Percentage(3);
                                thc9.HorizontalAlign = HorizontalAlign.Center;
                                thc9.Text = app.practical_marks_out_of_100.ToString();
                                tr.Cells.Add(thc9);
                                ////total_marks
                                TableCell thc10 = new TableCell();
                                thc10.Width = Unit.Percentage(3);
                                thc10.HorizontalAlign = HorizontalAlign.Center;
                                thc10.Text = app.theory_marks_out_of_100.ToString();
                                tr.Cells.Add(thc10);

                                TableCell thc12 = new TableCell();
                                thc12.Width = Unit.Percentage(3);
                                thc12.HorizontalAlign = HorizontalAlign.Center;
                                thc12.Text = app.total_marks.ToString();
                                tr.Cells.Add(thc12);

                                TableCell thc13 = new TableCell();
                                thc13.Width = Unit.Percentage(3);
                                thc13.HorizontalAlign = HorizontalAlign.Center;
                                thc13.Text = app.grade_code.ToString();
                                tr.Cells.Add(thc13);

                                TableCell thc15 = new TableCell();
                                thc15.Width = Unit.Percentage(3);
                                thc15.HorizontalAlign = HorizontalAlign.Center;
                                thc15.Text = app.result.ToString();
                                tr.Cells.Add(thc15);

                                TableCell thc14 = new TableCell();
                                thc14.Width = Unit.Percentage(3);
                                thc14.HorizontalAlign = HorizontalAlign.Center;
                                thc14.Text = app.Venue_Code.ToString();
                                tr.Cells.Add(thc14);

                                tbl.Rows.Add(tr);
                                divReportData.Controls.Add(tbl);
                                //lblError.Visible = false;
                            }
                        }
                        //else
                        //{
                        //    lblError.Visible = true;
                        //    lblError.Text = "No Record Found";
                        //}
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }
                }

            }

            if (ddlReport.SelectedValue == "5")
            {

                string message;
                using (DataTable dt1 = GetESResult(out message))
                {
                    if (dt1.Rows.Count > 0)
                    {
                        var application = (from a in dt1.AsEnumerable()
                                           select new
                                           {
                                               Registration_Number = a.Field<Decimal?>("Registration_Number"),
                                               candidate_name = a.Field<string>("candidate_name") ?? "",
                                               module_code = a.Field<string>("module_code") ?? "",
                                               exam_date = a.Field<string>("exam_date")?? "",
                                               total_marks = a.Field<Decimal?>("total_marks"),
                                               result = a.Field<string>("result")?? "",                                              

                                           }).ToList();

                        if (application.Count() > 0)
                        {

                            ShowTableHeaderES();

                            int i = 0;

                            //  if (dt1.Rows.Count > 0)
                            // {
                            foreach (var app in application)
                            {
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;

                                TableHeaderCell thc = new TableHeaderCell();
                                thc.Width = Unit.Percentage(1);
                                thc.HorizontalAlign = HorizontalAlign.Center;
                                thc.Text = i.ToString();
                                tr.Cells.Add(thc);

                                TableCell thc1 = new TableCell();
                                thc1.Width = Unit.Percentage(3);
                                thc1.HorizontalAlign = HorizontalAlign.Center;
                                thc1.Text = app.Registration_Number.ToString();
                                tr.Cells.Add(thc1);

                                TableCell thc2 = new TableCell();
                                thc2.Width = Unit.Percentage(3);
                                thc2.HorizontalAlign = HorizontalAlign.Center;
                                thc2.Text = app.candidate_name.ToString();
                                tr.Cells.Add(thc2);

                                TableCell thc3 = new TableCell();
                                thc3.Width = Unit.Percentage(3);
                                thc3.HorizontalAlign = HorizontalAlign.Center;
                                thc3.Text = app.module_code.ToString();
                                tr.Cells.Add(thc3);

                                TableCell thc4 = new TableCell();
                                thc4.Width = Unit.Percentage(3);
                                thc4.HorizontalAlign = HorizontalAlign.Center;
                                thc4.Text = app.exam_date.ToString();
                                tr.Cells.Add(thc4);

                                TableCell thc6 = new TableCell();
                                thc6.Width = Unit.Percentage(3);
                                thc6.HorizontalAlign = HorizontalAlign.Center;
                                thc6.Text = app.total_marks.ToString();
                                tr.Cells.Add(thc6);

                                TableCell thc7 = new TableCell();
                                thc7.Width = Unit.Percentage(3);
                                thc7.HorizontalAlign = HorizontalAlign.Center;
                                thc7.Text = app.result.ToString();
                                tr.Cells.Add(thc7);

                               
                                tbl.Rows.Add(tr);
                                divReportData.Controls.Add(tbl);
                                //lblError.Visible = false;
                            }
                        }                        
                        
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }
                }
            }
            if (ddlReport.SelectedValue == "6")
            {
                string message;
                using (DataTable dt1 = GetSummarReportES(out  message))
                {
                    if (dt1.Rows.Count > 0)
                    {

                        //lblError.Visible = false;
                        lblError.Text = "";

                        var application = (from a in dt1.AsEnumerable()
                                           select new
                                           {
                                               SNo = a.Field<Int64>("SN"),
                                               module_code = a.Field<string>("module_code") ?? "",
                                               gradeS = a.Field<Int32?>("Grade S") ?? 0,
                                               gradeA = a.Field<Int32?>("Grade A") ?? 0,
                                               gradeB = a.Field<Int32?>("Grade B") ?? 0,
                                               gradeC = a.Field<Int32?>("Grade C") ?? 0,
                                               gradeD = a.Field<Int32?>("Grade D") ?? 0,
                                               gradeF = a.Field<Int32?>("Grade F") ?? 0,
                                               Absent = a.Field<Int32?>("Absent") ?? 0,
                                               Applied = a.Field<Int32?>("Applied") ?? 0,
                                               Appeared = a.Field<Int32?>("Appeared") ?? 0,
                                               Pass = a.Field<Int32?>("Pass") ?? 0,
                                               PassPercent = a.Field<Decimal?>("Pass(%)"),                                               

                                           }).ToList();

                        if (application.Count() > 0)
                        {

                            ShowTableHeaderSummaryES();

                            int i = 0;

                            //  if (dt1.Rows.Count > 0)
                            // {
                            foreach (var app in application)
                            {
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;

                                //TableHeaderCell thc = new TableHeaderCell();
                                //thc.Width = Unit.Percentage(1);
                                //thc.HorizontalAlign = HorizontalAlign.Center;
                                //thc.Text = i.ToString();
                                //tr.Cells.Add(thc);

                                TableCell thc1 = new TableCell();
                                thc1.Width = Unit.Percentage(3);
                                thc1.HorizontalAlign = HorizontalAlign.Center;
                                thc1.Text = app.SNo.ToString();
                                tr.Cells.Add(thc1);

                                TableCell thc2 = new TableCell();
                                thc2.Width = Unit.Percentage(3);
                                thc2.HorizontalAlign = HorizontalAlign.Center;
                                thc2.Text = app.module_code.ToString();
                                tr.Cells.Add(thc2);

                                TableCell thc3 = new TableCell();
                                thc3.Width = Unit.Percentage(3);
                                thc3.HorizontalAlign = HorizontalAlign.Center;
                                thc3.Text = app.gradeS.ToString();
                                tr.Cells.Add(thc3);

                                TableCell thc4 = new TableCell();
                                thc4.Width = Unit.Percentage(3);
                                thc4.HorizontalAlign = HorizontalAlign.Center;
                                thc4.Text = app.gradeA.ToString();
                                tr.Cells.Add(thc4);

                                TableCell thc6 = new TableCell();
                                thc6.Width = Unit.Percentage(3);
                                thc6.HorizontalAlign = HorizontalAlign.Center;
                                thc6.Text = app.gradeB.ToString();
                                tr.Cells.Add(thc6);

                                TableCell thc7 = new TableCell();
                                thc7.Width = Unit.Percentage(3);
                                thc7.HorizontalAlign = HorizontalAlign.Center;
                                thc7.Text = app.gradeC.ToString();
                                tr.Cells.Add(thc7);

                                TableCell thc8 = new TableCell();
                                thc8.Width = Unit.Percentage(3);
                                thc8.HorizontalAlign = HorizontalAlign.Center;
                                thc8.Text = app.gradeD.ToString();
                                tr.Cells.Add(thc8);

                                TableCell thc14 = new TableCell();
                                thc14.Width = Unit.Percentage(3);
                                thc14.HorizontalAlign = HorizontalAlign.Center;
                                thc14.Text = app.gradeF.ToString();
                                tr.Cells.Add(thc14);

                                TableCell thc9 = new TableCell();
                                thc9.Width = Unit.Percentage(3);
                                thc9.HorizontalAlign = HorizontalAlign.Center;
                                thc9.Text = app.Absent.ToString();
                                tr.Cells.Add(thc9);

                                TableCell thc10 = new TableCell();
                                thc10.Width = Unit.Percentage(3);
                                thc10.HorizontalAlign = HorizontalAlign.Center;
                                thc10.Text = app.Applied.ToString();
                                tr.Cells.Add(thc10);

                                TableCell thc11 = new TableCell();
                                thc11.Width = Unit.Percentage(3);
                                thc11.HorizontalAlign = HorizontalAlign.Center;
                                thc11.Text = app.Appeared.ToString();
                                tr.Cells.Add(thc11);

                                TableCell thc12 = new TableCell();
                                thc12.Width = Unit.Percentage(3);
                                thc12.HorizontalAlign = HorizontalAlign.Center;
                                thc12.Text = app.Pass.ToString();
                                tr.Cells.Add(thc12);

                                TableCell thc13 = new TableCell();
                                thc13.Width = Unit.Percentage(3);
                                thc13.HorizontalAlign = HorizontalAlign.Center;
                                thc13.Text = app.PassPercent.ToString();
                                tr.Cells.Add(thc13);

                                tbl.Rows.Add(tr);
                                divReportData.Controls.Add(tbl);


                            }
                        }
                        //else
                        //{
                        //    //lblError.Visible = true;
                        //    lblError.Text = "No Record Found";
                        //}
                    }

                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
                   
    }
    protected void btnView_Click(object sender, EventArgs e)
    {        
        try
        {
           
            //Int32 examID = Convert.ToInt32(ddlExam.SelectedValue);
            //if(Convert.ToInt32(ddlReport.SelectedValue) == 1 )
            //{
            //    string isCompiled = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select  whether_compiled  from  CHMT_OLevel_ResultFreeze  where exam_id = " + examID, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

            //    if (isCompiled == "N")
            //    {
            //        lblError.Visible = true;
            //        lblError.Text = "CHM-T  O Level  Result  is  not  compiled yet. Please try again once the result is compiled.";
            //    }
            //    else
            //    {
            //         ShowData();
            //    }
            //}

            //else if (Convert.ToInt32(ddlReport.SelectedValue) == 4 || Convert.ToInt32(ddlReport.SelectedValue) == 2 || Convert.ToInt32(ddlReport.SelectedValue) == 3)
            //{
            //    string isFinalized = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select  whether_finalized  from  CHMT_OLevel_ResultFreeze  where exam_id = " + examID, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

            //    if (isFinalized == "N")
            //    {
            //        lblError.Visible = true;
            //        lblError.Text = "CHM-T  O Level  Result  is  not  finailzed yet. Please try again, once the result is finalized.";
            //    }
            //    else
            //    {
            //        ShowData();
            //    }
            //}

            //else
            //{
                ShowData();
           // }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void imgPDF_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            hw.AddAttribute("border", "1");
            hw.RenderBeginTag(HtmlTextWriterTag.Font);
            hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "9");
          

            divReportData.Visible = true;
            tbl.BorderStyle = BorderStyle.Solid;
            tbl.CssClass = "sample3";
            tbl.CellPadding = 2;
            tbl.CellSpacing = 1;
            tbl.Width = Unit.Percentage(200);

            ShowData();
            divReportData.Controls.Add(tbl);

            divReportData.RenderControl(hw);
            hw.RenderEndTag();
            Response.Clear();
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();

            if (ddlReport.SelectedValue == "1")
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=CHMT_CompiledResult_Report.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
            }

            else if (ddlReport.SelectedValue == "2")
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=CHMT_Modulewise_FinalizedReport.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
            }
            else if (ddlReport.SelectedValue == "3")
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=CHMT_CentreWise_FinalizedReport.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
            }
            else if (ddlReport.SelectedValue == "4")
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=CHMT_FinalizedResult_Report.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
            }
            else if (ddlReport.SelectedValue == "5")
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=CHMT_ESResult_Report.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
            }
            else
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=CHMT_ESSummary_Report.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
            }

            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
            divReportData.Visible = true;
            //ShowTableHeader();
            ShowData();
            divReportData.Controls.Add(tbl);
            Response.Clear();
            if (ddlReport.SelectedValue == "1")
            {
                Response.AddHeader("content-disposition", "attachment;filename=CHMT_CompiledResult_Report.xls");
            }
            else if (ddlReport.SelectedValue == "2")
            {
                Response.AddHeader("content-disposition", "attachment;filename=CHMT_Modulewise_FinalizedReport.xls");
            }
            else if (ddlReport.SelectedValue == "3")
            {
                Response.AddHeader("content-disposition", "attachment;filename=CHMT_CentreWise_FinalizedReport.xls");
            }
            else if (ddlReport.SelectedValue == "4")
            {
                Response.AddHeader("content-disposition", "attachment;filename=CHMT_FinalizedResult_Report.xls");
            }
            else if (ddlReport.SelectedValue == "5")
            {
                Response.AddHeader("content-disposition", "attachment;filename=CHMT_ESResult_Report.xls");
            }
            else
            {
                Response.AddHeader("content-disposition", "attachment;filename=CHMT_ESSummary_Report.xls");
            }
                        
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            htmlWrite = new Html32TextWriter(StringWrite);
            divReportData.RenderControl(htmlWrite);
            Response.Write(StringWrite.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlReport_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            // Assume connString is your SQL connection string
            Int16 inputOption = 0;
         //   ddlExam.SelectedIndex = 0;
            string reportContent = ddlReport.SelectedItem.Text;
            if (reportContent.Contains("Compiled"))
            {
                inputOption = 1;
            }
            else if (reportContent.Contains("Finalized") || reportContent.Contains("Employability") )
            {
                inputOption = 2;
            }
            else
            {
                inputOption = 0;
            }           

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("CHM_Get_Reprot_ExamSession_DropDown", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Input_option", inputOption);
                    con.Open();

                    // Execute the query and load data into a DataTable
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    // Bind the DataTable to the DropDownList
                    if (dt.Rows.Count > 0)
                    {
                        ddlExam.DataSource = dt;
                        ddlExam.DataTextField = "Exam_session_name"; // Displayed text
                        ddlExam.DataValueField = "exam_id";  // Value associated with the item
                        ddlExam.DataBind();

                        // Add the "--Select One--" item at the beginning
                        ddlExam.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select One--", "0"));
                        
                    }
                    else
                    {
                        //lblError.Visible = true;
                        //lblError.Text = "No  exam  for  the  selected report.";
                        //ddlExam.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select One--", "0"));
                        ddlExam.Items.Clear();
                        ddlExam.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select One--", "0"));
                        //ddlExam.SelectedIndex = 0;
                        //ddlExam.Items[0].Text = "--Select One--";
                        //for (int i = 1; i < ddlExam.Items.Count; i++)
                        //{
                        //   ddlExam.Items.Remove( new System.Web.UI.WebControls.ListItem("--Select One--",Convert.ToString(i) ));
                        //}
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }


    }
   
}