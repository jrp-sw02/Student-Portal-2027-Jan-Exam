using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class Admin_DVPBCC_CandPaymentNotVerifiedList : BasePage
{
    DataSet ds = new DataSet();
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Table tbl = new Table();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsSessionAlive() == false)
            Response.Redirect("../Index.aspx");
        currentRoleId = Convert.ToInt32(Session["RoleID"]);
        loginUserNo = Convert.ToInt32(Session["UserID"]);
        
        if (!UserManager.HasRight(currentRoleId, enmRight.View))
        {
            Response.Write("Sorry! You don't have rights  to view this page");
            Response.End();
        }
        loginUserType = (UserType)Session["UserType"];
        entityID = Convert.ToInt64(Session["EntityID"]);

        if (!IsPostBack)
        {
            BindExamYear();           
        }        
    }

    protected void BindExamYear()
    {
        var currentYear = DateTime.Today.Year +1;
        for (int i = 1; i <= 5; i++)
        {
            ddlYear.Items.Add((currentYear - i).ToString());
        }
              
    }
   
    protected void btnShow_Click(object sender, EventArgs e)
    {
        var currentYear = DateTime.Today.Year;
        var currentMonth = DateTime.Today.Month;
        int selectedMonth = Convert.ToInt32(ddlMonth.SelectedValue);
        int selectedYear = Convert.ToInt32(ddlYear.SelectedValue);
        if (selectedMonth > currentMonth && selectedYear == currentYear)
        {
            ShowAlert("Please select Current Month and Year or Before !!");
            return;
        }
        GetExamData(Convert.ToInt32(ddlMonth.SelectedValue), Convert.ToInt32(ddlYear.SelectedValue));
        if (ds.Tables[0].Rows.Count > 0)
        {
            lblReportHeading.Text = "<b> Report for Candidates whose  Payment is Not Verified in  " + ddlMonth.SelectedItem.Text + "- " + ddlYear.SelectedItem.Text + "</b> ";
            
            gvMain.DataSource = ds;
            gvMain.DataBind();
            gvMain.Visible = true;
        }
        else
        {
            lblReportHeading.Text = "";
            ShowAlert("No data available for selected criteria.");
            gvMain.Visible = false;
        }
    }
   
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        GetExamData(Convert.ToInt32(ddlMonth.SelectedValue), Convert.ToInt32(ddlYear.SelectedValue));
        if (ds.Tables[0].Rows.Count > 0)
        {
            downloadReport();
        }
        else
            ShowAlert("No data available for selected criteria.");
    }

    public DataSet GetExamData(Int32 ExamMonth, Int32 ExamYear)
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("get_DVPBCC_CandidateListForPaymentNotVerified", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ExamMonth", SqlDbType.Int));
                cmd.Parameters["@ExamMonth"].Value = ExamMonth;
                cmd.Parameters.Add(new SqlParameter("@ExamYear", SqlDbType.Int));
                cmd.Parameters["@ExamYear"].Value = ExamYear;                
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(ds); 
                }
            }
        }
        return ds;
    }

    void downloadReport()
    {
        try
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            hw.AddAttribute("border", "1");
            hw.RenderBeginTag(HtmlTextWriterTag.Font);
            hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "10");

            divpdf.Visible = true;
            tbl.BorderStyle = BorderStyle.Solid;
            tbl.CssClass = "sample3";
            tbl.CellPadding = 1;
            tbl.CellSpacing = 1;
            tbl.Width = Unit.Percentage(200);

            ShowData(ds.Tables[0]);
            divpdf.Controls.Add(tbl);

            divpdf.RenderControl(hw);
            hw.RenderEndTag();
            Response.Clear();
            StringReader sr = new StringReader(sw.ToString());          
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=DVPBCC_CandPaymentNotVerifiedList.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void ShowTableHeader(DataTable tab)
    {
        try
        {
            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);

            TableHeaderRow th1 = new TableHeaderRow();
            tc1.ColumnSpan = 8;
            tc1.Text = "<b> Report for Candidates whose  Payment is Not Verified in  " + ddlMonth.SelectedItem.Text + "- " + ddlYear.SelectedItem.Text + "</b> ";          
            tc1.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tc1);
            tbl.Rows.Add(th1);

            TableHeaderRow th2 = new TableHeaderRow();
            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.ColumnSpan = 8;
            tcCol1.Width = Unit.Percentage(1);
            tcCol1.Text = "Report Generated on " + System.DateTime.Today.ToLongDateString();
            tcCol1.HorizontalAlign = HorizontalAlign.Right;
            th2.Cells.Add(tcCol1);
            tbl.Rows.Add(th2);

            TableHeaderRow th = new TableHeaderRow();
            th2.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(5);
            tcCol.Text = "SrNo.";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.Width = Unit.Percentage(10);
            tcCol11.Text = "Roll Number";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol11);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(10);
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            tcCol2.Text = "Candidate Name";
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(20);
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Text = "Father Name";
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(10);
            tcCol4.Text = "Date of Birth";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(10);
            tcCol5.Text = "DemandNote No.";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(10);
            tcCol6.Text = "Exam Centre";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(10);
            tcCol7.Text = "Exam Date";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);
            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void ShowData(DataTable tab)
    {
        try
        { 
            StringBuilder mySql = new StringBuilder(); 
            
                if (tab.Rows.Count > 0)
                {
                    ShowTableHeader(tab);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        TableRow tr = new TableRow();                      
                        if (i % 2 == 0)
                            tr.CssClass = "gdalternate1";
                        else
                            tr.CssClass = "gdrow1";
                        ds.Tables[0].NewRow();
                        TableCell tdRow = new TableCell();
                        tdRow.Width = Unit.Percentage(5);
                        tdRow.Text = (i + 1).ToString();
                        tdRow.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow);

                        TableCell tdRow1 = new TableCell();
                        tdRow1.Width = Unit.Percentage(10);
                        tdRow1.Text = ds.Tables[0].Rows[i]["Roll_Number"].ToString();
                        tdRow1.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow1);

                        TableCell tdRow2 = new TableCell();
                        tdRow2.Width = Unit.Percentage(10);
                        tdRow2.Text = ds.Tables[0].Rows[i]["Name"].ToString();
                        tdRow2.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow2);

                        TableCell tdRow3 = new TableCell();
                        tdRow3.Width = Unit.Percentage(20);
                        tdRow3.Text = ds.Tables[0].Rows[i]["Father_Name"].ToString();
                        tdRow3.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow3);

                        TableCell tdRow4 = new TableCell();
                        tdRow4.Width = Unit.Percentage(10);
                        tdRow4.Text = ds.Tables[0].Rows[i]["Dob"].ToString();
                        tdRow4.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow4);

                        TableCell tdRow5 = new TableCell();
                        tdRow5.Width = Unit.Percentage(5);
                        tdRow5.Text = ds.Tables[0].Rows[i]["Demand_Note_ID"].ToString();
                        tdRow5.HorizontalAlign = HorizontalAlign.Center; 
                        tr.Cells.Add(tdRow5);

                        TableCell tdRow6 = new TableCell();
                        tdRow6.Width = Unit.Percentage(5);
                        tdRow6.Text = ds.Tables[0].Rows[i]["Exam_Centre_Name"].ToString();
                        tdRow6.HorizontalAlign = HorizontalAlign.Center; 
                        tr.Cells.Add(tdRow6);

                        TableCell tdRow7 = new TableCell();
                        tdRow7.Width = Unit.Percentage(10);
                        tdRow7.Text = ds.Tables[0].Rows[i]["Date_of_Exam"].ToString();
                        tdRow7.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow7);
                    tbl.Rows.Add(tr); 
                    }
                }             
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
        }
    }
protected void btnDownload1_Click(object sender, EventArgs e)
    {
        gvMain.Visible = true;
        GetExamData(Convert.ToInt32(ddlMonth.SelectedValue), Convert.ToInt32(ddlYear.SelectedValue));
        if (ds.Tables[0].Rows.Count > 0)
        {
            downloadReportExcel();
        }
        else
            ShowAlert("No data available for selected criteria.");
    }
    void downloadReportExcel()
    {
        try
        {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            hw.AddAttribute("border", "1");
            hw.RenderBeginTag(HtmlTextWriterTag.Font);
            hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "9");

            divpdf.Visible = true;
            tbl.BorderStyle = BorderStyle.Solid;
            tbl.CssClass = "sample3";
            tbl.CellPadding = 2;
            tbl.CellSpacing = 1;
            tbl.Width = Unit.Percentage(200);

            ShowData(ds.Tables[0]);
            divpdf.Controls.Add(tbl);

            //  divpdf.RenderControl(hw);
            //  hw.RenderEndTag();
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=PaymentNotVerified.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";

            htmlWrite = new Html32TextWriter(StringWrite);
            htmlWrite.AddAttribute("border", "2");
            divpdf.RenderControl(htmlWrite);
            Response.Write(StringWrite.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}