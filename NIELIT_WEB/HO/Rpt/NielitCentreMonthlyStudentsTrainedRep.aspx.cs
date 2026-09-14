using EConnect.DAL;
using EConnect.URM;
//using System.Drawing;
using iTextSharp.text;
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

public partial class NielitCentreMonthlyStudentsTrainedRep : BasePage
{

    Table tbl = new Table();
    StringBuilder str = new StringBuilder();
    Int32 currentRoleId = 0;
    TableHeaderRow th2 = new TableHeaderRow();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/Rpt/NielitCentreMonthlyStudentTrainedFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
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
            ShowAlert(ex.Message);
        }
    }

    protected void ShowTableHeader()
    {
        try
        {

            DateTime FromDate = Convert.ToDateTime(Request.QueryString["DateFrom"]);
            DateTime ToDate = Convert.ToDateTime(Request.QueryString["DateTo"]);
            int centreID = Convert.ToInt32(Request.QueryString["centreId"]);
            string centreType = Request.QueryString["centreType"].ToString();
            string centreName = "";
            if (centreType == "C")
            {
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    var centre = (from s in context.NielitCentres
                                  where s.ID == centreID
                                  select new { name = s.Name }).FirstOrDefault();
                    if (centre != null)
                        centreName = centre.name;
                }
            }

            if (centreType == "S")
            {
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    var centre = (from s in context.AffInstitutes
                                  where s.instituteID == centreID
                                  select new { name = s.Name }).FirstOrDefault();
                    if (centre != null)
                        centreName = centre.name;

                    var centre1 = (from s in context.NonAffInstitutes
                                   where s.ID == centreID
                                   select new { name = s.Name }).FirstOrDefault();
                    if (centre1 != null)
                        centreName = centre1.name;
                }
            }

            int cols = GetMonthDifference(FromDate, ToDate);
            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(500);

            TableHeaderRow th1 = new TableHeaderRow();
            tc1.ColumnSpan = cols;
            tc1.Text = "Report of " + centreName + "<br/> for students trained <br/> in batches ending from " + FromDate.ToString("dd-MMM-yyyy") + "  To " + ToDate.ToString("dd-MMM-yyyy") + "";
            tc1.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tc1);

            tbl.Rows.Add(th1);

            TableHeaderRow th = new TableHeaderRow();
            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.ColumnSpan = cols;
            tcCol1.Width = Unit.Percentage(100);
            tcCol1.Text = "Report As on " + System.DateTime.Now.ToString ();
            tcCol1.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tcCol1);
            tbl.Rows.Add(th);

            

           
            th2.CssClass = "head1";
            TableHeaderCell tc = new TableHeaderCell();
            tc.Width = Unit.Percentage(1);
            tc.Text = "<b>#</b>";
            tc.HorizontalAlign = HorizontalAlign.Right;
            th2.Cells.Add(tc);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(1);
            tcCol2.Text = "<b>Course Name</b>";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th2.Cells.Add(tcCol2);


            GenerateTable(cols, FromDate);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(1);
            tcCol3.Text = "<b>Total</b>";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th2.Cells.Add(tcCol3);
           
            
            tbl.Rows.Add(th2);

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    private void GenerateTable(int colscount,DateTime FromDate)
    {
        

       // TableHeaderRow header = new TableHeaderRow();
       // tbl.Rows.Add(header);
        //These two lines in iteir own loop
       // TableHeaderCell headerTableCell1 = new TableHeaderCell();
       // header.Cells.Add(headerTableCell1);
        for (int i = 0; i < 1; i++)
        {
           // TableRow row = new TableRow();
            for (int j = 0; j <= colscount; j++)
            {
                TableCell tc = new TableCell();
                tc.Width = Unit.Percentage(5);
                tc.Text = FromDate.ToString("MMM") + "-"+FromDate.Year .ToString ();
                tc.HorizontalAlign = HorizontalAlign.Center;
                th2.Cells.Add(tc);
                FromDate=FromDate.AddMonths(1);
            }
           
        }
       
       
    }

    private void GenerateData(int colscount, DateTime FromDate)
    {


        // TableHeaderRow header = new TableHeaderRow();
        // tbl.Rows.Add(header);
        //These two lines in iteir own loop
        // TableHeaderCell headerTableCell1 = new TableHeaderCell();
        // header.Cells.Add(headerTableCell1);
        for (int i = 0; i < 1; i++)
        {
            // TableRow row = new TableRow();
            for (int j = 0; j <= colscount; j++)
            {
                TableCell tc = new TableCell();
                tc.Width = Unit.Percentage(5);
                tc.Text = FromDate.ToString("MMM") + "-" + FromDate.Year.ToString();
                tc.HorizontalAlign = HorizontalAlign.Center;
                th2.Cells.Add(tc);
                FromDate = FromDate.AddMonths(1);
            }

        }


    }
    //public List<Item> monthNameList(int colscount, DateTime FromDate)
    //{
    //    List<Item> monthNames=new List<Item> ();
    //    for (int i = 0; i < 1; i++)
    //    {
    //        // TableRow row = new TableRow();
    //        for (int j = 0; j <= colscount; j++)
    //        {
    //            Item i1 = new Item();
    //            i1.monthName = FromDate.ToString("MMM") + "-" + FromDate.Year.ToString();
    //            monthNames.Add( i1);
               
    //        }

    //    }
    //    return (monthNames);
    //}
    public static int GetMonthDifference(DateTime startDate, DateTime endDate)
    {
        int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
        return Math.Abs(monthsApart);
    }
    protected void ShowData()
    {
        try
        {
            User objUser;
            Int32 loginUserNo = 0;
            Int64 InstituteID = 0;
            lblError.Visible = false;
            string strHead = "";

            NIELITMISContext context = new NIELITMISContext();


            Int64 centreId = Convert.ToInt64(Request.QueryString["centreId"]);
            DateTime FromDate = Convert.ToDateTime(Request.QueryString["DateFrom"]);
            DateTime ToDate = Convert.ToDateTime(Request.QueryString["DateTo"]);
            string centreType = Request.QueryString["centreType"];

            StringBuilder mySql = new StringBuilder();

           

            using (DataTable dt = GetStudentTrained(centreId, centreType, FromDate, ToDate))
            {



                if (dt.Rows.Count > 0)
                {
                    //var application = (from a in dt.AsEnumerable()
                    //                   select new
                    //                   {
                    //                       courseName = a.Field<string>("Courses"),
                    //                       //courseCategory = a.Field<string>("courseCategory"),
                    //                       //courseType = a.Field<string>("courseType"),

                    //                       //NumberOfStudentsTrained = a.Field<int>("NumberOfStudentsTrained"),

                    //                   }).ToList();
                   
                    //if (dt.Rows .Count >0)
                    //{
                        ShowTableHeader();

                        int i = 1;
                     //   foreach (var app in application)
                        foreach(DataRow row in dt.Rows)
                        {
                            TableRow tr = new TableRow();
                            if (i % 2 == 0)
                                tr.CssClass = "gdalternate1";
                            else
                                tr.CssClass = "gdrow1";

                            TableCell tdRow = new TableCell();
                            tdRow.Width = Unit.Percentage(1);
                            tdRow.Text = i.ToString();
                            tdRow.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow);


                            TableCell tdRow2 = new TableCell();
                            tdRow2.Width = Unit.Percentage(8);
                            string dddd = row["Courses"].ToString ();
                            tdRow2.Text = dddd;
                            tdRow2.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow2);

                            FromDate = Convert.ToDateTime(Request.QueryString["DateFrom"]);
                            int colscount = GetMonthDifference(FromDate, ToDate);

                            for (int x = 0; x < 1; x++)
                            {
                                // TableRow row = new TableRow();
                                for (int j = 0; j < colscount; j++)
                                {
                                    TableCell tc = new TableCell();
                                    tc.Width = Unit.Percentage(5);
                                    string data = FromDate.ToString("MMM") + "-" + FromDate.Year.ToString();
                                    tc.Text = row[data].ToString();
                                    tc.HorizontalAlign = HorizontalAlign.Center;
                                    tr.Cells.Add(tc);
                                    FromDate = FromDate.AddMonths(1);
                                }
                                TableCell tc1 = new TableCell();
                                tc1.Width = Unit.Percentage(5);
                                string data1 = FromDate.ToString("MMM") + "-" + FromDate.Year.ToString();
                                tc1.Text = row[data1].ToString();
                                tc1.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(tc1);
                            }

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(4);
                             tdRow3.Text = row["Total"].ToString();
                            tdRow3.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow3);



                            //TableCell tdRow5 = new TableCell();
                            //tdRow5.Width = Unit.Percentage(7);
                            //// tdRow5.Text = app.courseCategory.ToString();
                            //tdRow5.HorizontalAlign = HorizontalAlign.Left;
                            //tr.Cells.Add(tdRow5);

                            //TableCell tdRow13 = new TableCell();
                            //tdRow13.Width = Unit.Percentage(1);
                            //// tdRow13.Text = app.NumberOfStudentsTrained.ToString();
                            //tdRow13.HorizontalAlign = HorizontalAlign.Left;
                            //tdRow13.Wrap = false;
                            //tr.Cells.Add(tdRow13);


                            tbl.Rows.Add(tr);
                            i++;
                        }
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }
               // }
              //  else
              //  {
              //      lblError.Visible = true;
              //    lblError.Text = "No Record Found";
              //}
            }
            LblRptSubHeader.Text = strHead;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
        }
    }
    public IEnumerable<string> GetColumn(List<Item> items, string columnName)
    {
        var values = items.Select(x => x.GetType().GetProperty(columnName).GetValue(x).ToString());
        return values;
    }
    public DataTable GetStudentTrained(Int64 centreId, string centreType, DateTime FromDate, DateTime ToDate)
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("[repMonthlyStudentsTrained]", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@pCentreID", SqlDbType.BigInt));
                cmd.Parameters["@pCentreID"].Value = centreId;
                //cmd.Parameters.Add(new SqlParameter("@centreType", SqlDbType.VarChar, 1));
                //cmd.Parameters["@centreType"].Value = centreType;
                //cmd.Parameters.Add(new SqlParameter("@pStartDt", SqlDbType.VarChar,20));
                //cmd.Parameters["@pStartDt"].Value = FromDate.ToShortDateString();
                //cmd.Parameters.Add(new SqlParameter("@pEndDt", SqlDbType.VarChar ,20));
                //cmd.Parameters["@pEndDt"].Value = ToDate.ToShortDateString ();
                cmd.Parameters.Add(new SqlParameter("@pStartDt", SqlDbType.Date));
                cmd.Parameters["@pStartDt"].Value = FromDate;
                cmd.Parameters.Add(new SqlParameter("@pEndDt", SqlDbType.Date));
                cmd.Parameters["@pEndDt"].Value = ToDate;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }

    //protected void imgPDF_Click(object sender, ImageClickEventArgs e)
    //{

    //    try
    //    {
    //        StringWriter sw = new StringWriter();
    //        HtmlTextWriter hw = new HtmlTextWriter(sw);
    //        hw.AddAttribute("border", "1");
    //        hw.RenderBeginTag(HtmlTextWriterTag.Font);
    //        hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "9");

    //        divReportData.Visible = true;
    //        tbl.BorderStyle = BorderStyle.Solid;
    //        tbl.CssClass = "sample3";
    //        tbl.CellPadding = 2;
    //        tbl.CellSpacing = 1;
    //        tbl.Width = Unit.Percentage(200);

    //        ShowData();
    //        divReportData.Controls.Add(tbl);

    //        divReportData.RenderControl(hw);
    //        hw.RenderEndTag();
    //        Response.Clear();
    //        StringReader sr = new StringReader(sw.ToString());
    //        Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);
    //        // Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);

    //        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
    //        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
    //        pdfDoc.Open();
    //        htmlparser.Parse(sr);
    //        pdfDoc.Close();
    //        Response.ContentType = "application/pdf";
    //        Response.AddHeader("content-disposition", "attachment;filename=StudentTrainedReport.pdf");
    //        Response.Cache.SetCacheability(HttpCacheability.NoCache);

    //        // Response.Write(pdfDoc);
    //        Response.Write(HttpUtility.HtmlEncode(pdfDoc));
    //        Response.End();
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}




    //Added by amit start

    protected void imgPDF_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Int64 centreId = Convert.ToInt64(Request.QueryString["centreId"]);
            DateTime fromDate = Convert.ToDateTime(Request.QueryString["DateFrom"]);
            DateTime toDate = Convert.ToDateTime(Request.QueryString["DateTo"]);
            string centreType = Request.QueryString["centreType"];

            DataTable dt = GetStudentTrained(centreId, centreType, fromDate, toDate);
            if (dt.Rows.Count == 0)
            {
                ShowAlert("No records found for PDF generation.");
                return;
            }

            int monthCount = GetMonthDifference(fromDate, toDate) + 1;

            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
            MemoryStream ms = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, ms);
            pdfDoc.Open();

            // Fonts 
            Font headerFont = new Font(Font.FontFamily.HELVETICA, 10f, Font.BOLD, BaseColor.WHITE);
            Font titleFont = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD, BaseColor.BLACK);
            Font cellFont = new Font(Font.FontFamily.HELVETICA, 9f, Font.NORMAL, BaseColor.BLACK);
            Font boldCellFont = new Font(Font.FontFamily.HELVETICA, 9f, Font.BOLD, BaseColor.BLACK);

            BaseColor headerBg = new BaseColor(0, 102, 204);
            BaseColor oddRowBg = new BaseColor(240, 248, 255);
            BaseColor evenRowBg = BaseColor.WHITE;

            // Title
            Paragraph title = new Paragraph("Student Trained Report (Monthly)", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 10f;
            pdfDoc.Add(title);

            // Period line - string concatenation only
            string periodText = "Period: " + fromDate.ToString("dd-MMM-yyyy") +
                               " to " + toDate.ToString("dd-MMM-yyyy") +
                               " | Generated on: " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
            Paragraph period = new Paragraph(periodText, cellFont);
            period.Alignment = Element.ALIGN_CENTER;
            pdfDoc.Add(period);
            pdfDoc.Add(Chunk.NEWLINE);

            // Table setup
            int totalColumns = 3 + monthCount;
            PdfPTable table = new PdfPTable(totalColumns);
            table.WidthPercentage = 100;

            float[] widths = new float[totalColumns];
            widths[0] = 5f;   // Sr#
            widths[1] = 25f;  // Course
            for (int i = 2; i < totalColumns - 1; i++)
                widths[i] = 6f;
            widths[totalColumns - 1] = 8f; // Total
            table.SetWidths(widths);

            // Header cells
            AddCell(table, "Sr#", headerFont, headerBg, Element.ALIGN_CENTER);
            AddCell(table, "Course Name", headerFont, headerBg, Element.ALIGN_CENTER);

            DateTime monthCursor = fromDate;
            for (int m = 0; m < monthCount; m++)
            {
                AddCell(table, monthCursor.ToString("MMM-yyyy"), headerFont, headerBg, Element.ALIGN_CENTER);
                monthCursor = monthCursor.AddMonths(1);
            }
            AddCell(table, "Total", headerFont, headerBg, Element.ALIGN_CENTER);

            // Data rows
            int sr = 1;
            foreach (DataRow row in dt.Rows)
            {
                BaseColor rowBg = (sr % 2 == 0) ? evenRowBg : oddRowBg;

                AddCell(table, sr.ToString(), cellFont, rowBg, Element.ALIGN_RIGHT);
                AddCell(table, row["Courses"].ToString(), boldCellFont, rowBg, Element.ALIGN_LEFT);

                monthCursor = fromDate;
                for (int m = 0; m < monthCount; m++)
                {
                    string colKey = monthCursor.ToString("MMM") + "-" + monthCursor.Year.ToString();
                    string val = "0";
                    if (row.Table.Columns.Contains(colKey))
                        val = row[colKey].ToString();

                    AddCell(table, val, cellFont, rowBg, Element.ALIGN_CENTER);
                    monthCursor = monthCursor.AddMonths(1);
                }

                AddCell(table, row["Total"].ToString(), boldCellFont, rowBg, Element.ALIGN_CENTER);
                sr++;
            }

            pdfDoc.Add(table);
            pdfDoc.Close();
            writer.Close();

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=StudentTrainedReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(ms.ToArray());
            Response.Flush();
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert("PDF Error: " + ex.Message);
        }
    }

    private void AddCell(PdfPTable table, string text, Font font, BaseColor bg, int alignment)
    {
        PdfPCell cell = new PdfPCell(new Phrase(text, font));
        cell.BackgroundColor = bg;
        cell.HorizontalAlignment = alignment;
        cell.Padding = 4f;
        cell.UseAscender = true;
        cell.UseDescender = true;
        table.AddCell(cell);
    }

    //Added by amit end

    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
            divReportData.Visible = true;
            ShowData();
            divReportData.Controls.Add(tbl);
//Added for Audit
            foreach (TableRow row in tbl.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    cell.Text = HttpUtility.HtmlEncode(cell.Text); // Sanitize cell content
                }
            }
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=StudentTrainedReport.xls");
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
}
public class Item
{
   public string monthName;
}