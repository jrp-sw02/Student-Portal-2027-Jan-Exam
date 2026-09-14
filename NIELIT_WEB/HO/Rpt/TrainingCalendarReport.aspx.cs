using EConnect.DAL;
using EConnect.URM;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HO_Rpt_TrainingCalendarReport : BasePage
{
    Table tbl = new Table();
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 UserTypeId = 0;
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/TrainingCalendarReportFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!Page.IsPostBack)
            {

                tbl.CssClass = "sample3";
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(100);
                ShowData();
                divReportData.Controls.Add(tbl);


            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void ShowTableHeader()
    {
        try
        {
            int centreId = Convert.ToInt32(Request.QueryString["centreId"]);
            string reportId = Convert.ToString(Request.QueryString["itemvalue"]);
            int courseId = Convert.ToInt32(Request.QueryString["courseId"]);
            string centrname = Convert.ToString(Request.QueryString["centrname"]);
            string courseName = Convert.ToString(Request.QueryString["courseName"]);
            DateTime periodFromDate = Convert.ToDateTime(Request.QueryString["periodFromDate"]);
            DateTime periodToDate = Convert.ToDateTime(Request.QueryString["periodToDate"]);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "SrNo.";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(8);
            tcCol1.Text = "Centre Name";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);


            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(8);
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            tcCol2.Text = "Course Name";
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(8);
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Text = "Whether NSQF";
            th.Cells.Add(tcCol3);


            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(8);
            tcCol4.Text = "NSQF Level";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(10);
            tcCol5.Text = "Participant Eligibility";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);


            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(5);
            tcCol6.Text = "Duration (in hrs/months)";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);


            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(3);
            tcCol7.Text = "Start Date";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);


            TableHeaderCell tcCol8 = new TableHeaderCell();
            tcCol8.Width = Unit.Percentage(3);
            tcCol8.Text = "End Date";
            tcCol8.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol8);

            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(3);
            tcCol9.Text = "Admission Status";
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol9);

            TableHeaderCell tcCol10 = new TableHeaderCell();
            tcCol10.Width = Unit.Percentage(5);
            tcCol10.Text = "Course Incharge";
            tcCol10.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol10);

            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.Width = Unit.Percentage(9);
            tcCol11.Text = "Contact No.";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol11);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }

    protected void ShowData()
    {
        try
        {
            lblError.Visible = false;
            NIELITMISContext context = new NIELITMISContext();
            string reportId = Convert.ToString(Request.QueryString["itemvalue"]);
            int centreId = Convert.ToInt32(Request.QueryString["centreId"]);
            int courseId = Convert.ToInt32(Request.QueryString["courseId"]);
            string centrname = Convert.ToString(Request.QueryString["centrname"]);
            string courseName = Convert.ToString(Request.QueryString["courseName"]);
            DateTime periodFromDate = Convert.ToDateTime(Request.QueryString["periodFromDate"]);
            DateTime periodToDate = Convert.ToDateTime(Request.QueryString["periodToDate"]);

            StringBuilder mySql = new StringBuilder();


            using (ds = GetTrainingData(centreId, reportId, courseId, periodFromDate, periodToDate))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ShowTableHeader();
                    lblCentreName.Text = "Report of Training Calendar for : " + Convert.ToString(centrname) + Convert.ToString(courseName);
                    if (reportId == "A")
                    {
                        lblCentreName.Visible = false;
                    }
                    lblfromdate.Text = "From: " + periodFromDate.ToLongDateString ();
                    lbltodate.Text = "To: " +periodToDate.ToLongDateString ();
                    int i;
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        TableRow tr = new TableRow();
                        //  int i = 1;
                        if (i % 2 == 0)
                            tr.CssClass = "gdalternate1";
                        else
                            tr.CssClass = "gdrow1";
                        ds.Tables[0].NewRow();
                        TableCell tdRow = new TableCell();
                        tdRow.Width = Unit.Percentage(1);
                        tdRow.Text = (i + 1).ToString();
                        tdRow.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow);


                        TableCell tdRow1 = new TableCell();
                        tdRow1.Width = Unit.Percentage(10);
                        tdRow1.Text = ds.Tables[0].Rows[i]["CentreName"].ToString();
                        tdRow1.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow1);

                        TableCell tdRow2 = new TableCell();
                        tdRow2.Width = Unit.Percentage(10);
                        tdRow2.Text = ds.Tables[0].Rows[i]["CourseName"].ToString();
                        tdRow2.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow2);

                        TableCell tdRow3 = new TableCell();
                        tdRow3.Width = Unit.Percentage(12);
                        tdRow3.Text = ds.Tables[0].Rows[i]["whetherNSQFAligned"].ToString();
                        tdRow3.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow3);

                        TableCell tdRow13 = new TableCell();
                        tdRow13.Width = Unit.Percentage(10);
                        tdRow13.Text = ds.Tables[0].Rows[i]["NSQFLevel"].ToString();
                        tdRow13.HorizontalAlign = HorizontalAlign.Center;
                        tdRow13.Wrap = false;
                        tr.Cells.Add(tdRow13);

                        TableCell tdRow14 = new TableCell();
                        tdRow14.Width = Unit.Percentage(5);
                        tdRow14.Text = ds.Tables[0].Rows[i]["participantEligibility"].ToString();
                        tdRow14.HorizontalAlign = HorizontalAlign.Center;
                        tdRow14.Wrap = false;
                        tr.Cells.Add(tdRow14);

                        TableCell tdRow6 = new TableCell();
                        tdRow6.Width = Unit.Percentage(5);
                        tdRow6.Text = ds.Tables[0].Rows[i]["duration"].ToString();
                        tdRow6.HorizontalAlign = HorizontalAlign.Center;
                        tdRow6.Wrap = false;
                        tr.Cells.Add(tdRow6);

                        TableCell tdRow4 = new TableCell();
                        tdRow4.Width = Unit.Percentage(8);
                        if (ds.Tables[0].Rows[i]["startDate"] != null)
                        {
                            DateTime dt1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["startDate"]);
                            tdRow4.Text = dt1.ToString("dd-MMM-yyyy");
                        }

                        //  tdRow4.Text = ds.Tables[0].Rows[i]["startDate"].ToString();
                        tdRow4.HorizontalAlign = HorizontalAlign.Center;
                        tdRow4.Wrap = false;
                        tr.Cells.Add(tdRow4);

                        TableCell tdRow5 = new TableCell();
                        tdRow5.Width = Unit.Percentage(10);
                        if (ds.Tables[0].Rows[i]["endDate"] != null)
                        {
                            DateTime dt1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["endDate"]);
                            tdRow5.Text = dt1.ToString("dd-MMM-yyyy");
                        }
                        //  tdRow5.Text = ds.Tables[0].Rows[i]["endDate"].ToString();
                        tdRow5.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow5);

                        TableCell tdRow7 = new TableCell();
                        tdRow7.Width = Unit.Percentage(3);
                        tdRow7.Text = ds.Tables[0].Rows[i]["admissionStatus"].ToString();
                        tdRow7.HorizontalAlign = HorizontalAlign.Center;
                        tdRow7.Wrap = false;
                        tr.Cells.Add(tdRow7);

                        TableCell tdRow8 = new TableCell();
                        tdRow8.Width = Unit.Percentage(8);
                        tdRow8.Text = ds.Tables[0].Rows[i]["courseInchargeName"].ToString();
                        tdRow8.HorizontalAlign = HorizontalAlign.Center;
                        tdRow8.Wrap = false;
                        tr.Cells.Add(tdRow8);

                        TableCell tdRow9 = new TableCell();
                        tdRow9.Width = Unit.Percentage(8);
                        if (ds.Tables[0].Rows[i]["courseInchargeMobile"] != null)
                        {
                            tdRow9.Text = ds.Tables[0].Rows[i]["courseInchargeMobile"].ToString() + "</br> &nbsp;&nbsp;" + ds.Tables[0].Rows[i]["courseInchargePhone1"].ToString();
                        }

                        tdRow9.HorizontalAlign = HorizontalAlign.Center;
                        tdRow9.Wrap = false;
                        tr.Cells.Add(tdRow9);



                        tbl.Rows.Add(tr);
                        // i++;
                        lblCount.Visible = true;
                        lblCount.Text = "Total Records : " + ds.Tables[0].Rows.Count.ToString();
                    }
                }

                else
                {
                    lblCount.Visible = false;
                    lblError.Visible = true;
                    lblError.Text = "No Record Found";
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

    public DataSet GetTrainingData(Int64 centreId, string reportId, Int64 courseId, DateTime periodFromDate, DateTime periodToDate)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetTrainingCalendarReport", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@startDate", SqlDbType.Date));
                cmd.Parameters["@startDate"].Value = periodFromDate;

                cmd.Parameters.Add(new SqlParameter("@endDate", SqlDbType.Date));
                cmd.Parameters["@endDate"].Value = periodToDate;

                cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar));
                cmd.Parameters["@type"].Value = reportId;
                if (reportId == "C")
                {
                    cmd.Parameters.Add(new SqlParameter("@centreId", SqlDbType.BigInt));
                    cmd.Parameters["@centreId"].Value = centreId;
                }
                else if (reportId == "Co")
                {
                    cmd.Parameters.Add(new SqlParameter("@courseId", SqlDbType.BigInt));
                    cmd.Parameters["@courseId"].Value = courseId;
                }
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(ds);
                }
            }
        }
        return ds;
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

            divpdf.Visible = true;
            tbl.BorderStyle = BorderStyle.Solid;
            tbl.CssClass = "sample3";
            tbl.CellPadding = 2;
            tbl.CellSpacing = 1;
            tbl.Width = Unit.Percentage(200);

            ShowData();
            divpdf.Controls.Add(tbl);

            divpdf.RenderControl(hw);
            hw.RenderEndTag();
            Response.Clear();
            StringReader sr = new StringReader(sw.ToString());
            // Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=TrainingCalendarReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}

