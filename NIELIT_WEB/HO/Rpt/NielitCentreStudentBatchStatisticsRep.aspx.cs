using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Data;
using System.Data;
using EConnect.NIELIT;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Transactions;
using EConnect.Utils.Common;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Data.OleDb;
using System.IO;
using EConnect.Utils.Common;

public partial class ReportPgae : BasePage
{   
    Table tbl = new Table();  
    StringBuilder str = new StringBuilder();
    Int32 currentRoleId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/Rpt/NielitCentreStudentBatchStatisticsRepFilter.aspx"))
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
           int CourseCatId = Convert.ToInt32(Request.QueryString["CourseCatId"]);
           Int32 CourseType = CourseCatId.ToString().Length;
           Int64 CourseId = Convert.ToInt64(Request.QueryString["CourseId"]);          
            String CourseName ="";
            DateTime BatchFromDate = Convert.ToDateTime(Request.QueryString["DateFrom"]);
            DateTime BatchToDate = Convert.ToDateTime(Request.QueryString["DateTo"]);
            using (EConnectContext context = new EConnectContext())
            {
                if (CourseType <= 2)
                {
                    if (CourseCatId != 0)
                    {
                        Course cName = context.Courses.Where(s => s.ID == CourseId).FirstOrDefault();
                        CourseName = cName.Name; 
                    }
                }
                else
                {
                    NIELITMISContext context1 = new NIELITMISContext();
                    if (CourseCatId != 0)
                    {
						 var courseId = (from p in context1.NielitCourseDurations 
                                       where p.ID == CourseId
                                       select new { cId = p.courseID  }).FirstOrDefault ();
                        NielitCentreCourse cName = context1.NielitCentreCourses.Where(s => s.ID == courseId.cId).FirstOrDefault();
                        CourseName = cName.Name; 
                    }
                }
            }
            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);

            TableHeaderRow th1 = new TableHeaderRow();
            tc1.ColumnSpan = 16;
            tc1.Text = "Statistics Report of Course ( " + CourseName + "  ) Batch Wise  <br/>  Date :  From " + BatchFromDate.ToString("dd-MMM-yyyy") + "  To " + BatchToDate.ToString("dd-MMM-yyyy") + "";
            tc1.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tc1);

            tbl.Rows.Add(th1);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";
            TableHeaderCell tc = new TableHeaderCell();
            tc.Width = Unit.Percentage(1);
            tc.Text = "<b>#</b>";
            tc.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tc);


            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(1);
            tcCol1.Text = "<b>Batch Code</b>";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(1);
            tcCol2.Text = "<b>Batch Number</b>";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(1);
            tcCol3.Text = "<b>Start Date</b>";
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(5);
            tcCol4.Text = "<b>End date</b>";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);           


            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(8);
            tcCol6.Text = "<b>No. of students enrolled</b>";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);


            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(5);
            tcCol7.Text = "<b>Project students Count</b>";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);


            TableHeaderCell tcCol8 = new TableHeaderCell();
            tcCol8.Width = Unit.Percentage(3);
            tcCol8.Text = "<b>SC Count</b>";
            tcCol8.HorizontalAlign = HorizontalAlign.Center;
            tcCol8.Wrap = false;
            th.Cells.Add(tcCol8);


            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(15);
            tcCol9.Text = "<b>ST Count</b>";
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol9);


            TableHeaderCell tcCol10 = new TableHeaderCell();
            tcCol10.Width = Unit.Percentage(2);
            tcCol10.Text = "<b>OBC Count</b>";
            tcCol10.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol10);


            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.Width = Unit.Percentage(2);
            tcCol11.Text = "<b>General Count</b>";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol11);


            TableHeaderCell tcCol16 = new TableHeaderCell();
            tcCol16.Width = Unit.Percentage(2);
            tcCol16.Text = "<b>PH Count</b>";
            tcCol16.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol16);

            TableHeaderCell tcCol17 = new TableHeaderCell();
            tcCol17.Width = Unit.Percentage(2);
            tcCol17.Text = "<b>Women Count</b>";
            tcCol17.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol17);

            TableHeaderCell tcCol18 = new TableHeaderCell();
            tcCol18.Width = Unit.Percentage(2);
            tcCol18.Text = "<b>Certified Students</b>";
            tcCol18.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol18);

            TableHeaderCell tcCol19 = new TableHeaderCell();
            tcCol19.Width = Unit.Percentage(2);
            tcCol19.Text = "<b>Placed Students as on (</b>"+System.DateTime.Now.ToString("dd-MMM-yyyy")+ "<b> ) </b>";
            tcCol19.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol19);

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
            User objUser;
            Int32 loginUserNo = 0;
            Int64 InstituteID = 0;
            lblError.Visible = false;
            string strHead = "";
            
            NIELITMISContext context = new NIELITMISContext();
            string InstituteIDd = Request.QueryString["InstId"];
            if (InstituteIDd == "undefined")
            {
                using (EConnectContext context1 = new EConnectContext())
                {
                    loginUserNo = Convert.ToInt32(Session["UserID"]);
                    objUser = new EConnect.URM.User();
                    User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    Int64 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                    InstituteID = NielitCentreId;
                }
            }
            else
            {
                 InstituteID = Convert.ToInt64(Request.QueryString["InstId"]);
            }
            Int64 CourseId = Convert.ToInt64(Request.QueryString["CourseId"]);
            int CourseCatId = Convert.ToInt32(Request.QueryString["CourseCatId"]);
            DateTime BatchFromDate = Convert.ToDateTime(Request.QueryString["DateFrom"]);
            DateTime BatchToDate = Convert.ToDateTime(Request.QueryString["DateTo"]);          

            StringBuilder mySql = new StringBuilder();



            using (DataTable dt = GetStudentData(InstituteID, CourseId, BatchFromDate, BatchToDate))
            {
                if (dt.Rows.Count > 0)
                {
                    var application = (from a in dt.AsEnumerable()
                                       select new
                                       {
                                          
                                           BatchCode = a.Field<string>("BatchCode"),
                                           BatchNumber = a.Field<int>("BatchNumber"),
                                           BatchStartDate = a.Field<DateTime>("startDate").ToString("dd-MMM-yyyy"),
                                           BatchEndDate = a.Field<DateTime>("endDate").ToString("dd-MMM-yyyy"),
                                           NumberOfStudentsEnrolled = a.Field<int>("NumberOfStudentsEnrolled"),
                                           ProjectStudentsCount = a.Field<int>("ProjectStudentsCount"),
                                           SCCount = a.Field<int>("SCCount"),
                                           STCount = a.Field<int>("STCount"),
                                           OBCCount = a.Field<int>("OBCCount"),
                                           GeneralCount = a.Field<int>("GeneralCount"),
                                           PHCount = a.Field<int>("PHCount"),
                                           WomenCount = a.Field<int>("WomenCount"),
                                           CertifiedStudents = a.Field<int>("CertifiedStudents"),
                                           whetherPlaced = a.Field<int>("whetherPlaced")
                                       }).ToList();
                   
                    if (application.Count() > 0)
                    {
                        ShowTableHeader();
                      
                        int i = 1;
                        foreach (var app in application)
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
                            tdRow2.Width = Unit.Percentage(1);
                            tdRow2.Text = app.BatchCode.ToString();
                            tdRow2.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow2);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(1);
                            tdRow3.Text = app.BatchNumber.ToString();
                            tdRow3.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow3);

                                TableCell tdRow45 = new TableCell();
                                tdRow45.Width = Unit.Percentage(8);                                
                                tdRow45.Text = app.BatchStartDate;
                                tdRow45.HorizontalAlign = HorizontalAlign.Left;
                                tdRow45.Wrap = false;
                                tr.Cells.Add(tdRow45);

                            TableCell tdRow5 = new TableCell();
                            tdRow5.Width = Unit.Percentage(1);                           
                            tdRow5.Text = app.BatchEndDate.ToString();
                            tdRow5.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow5);

                            TableCell tdRow13 = new TableCell();
                            tdRow13.Width = Unit.Percentage(8);
                            tdRow13.Text = app.NumberOfStudentsEnrolled.ToString();
                            tdRow13.HorizontalAlign = HorizontalAlign.Left;
                            tdRow13.Wrap = false;
                            tr.Cells.Add(tdRow13);

                            TableCell tdRow14 = new TableCell();
                            tdRow14.Width = Unit.Percentage(8);
                            tdRow14.Text = app.ProjectStudentsCount.ToString();
                            tdRow14.HorizontalAlign = HorizontalAlign.Left;
                            tdRow14.Wrap = false;
                            tr.Cells.Add(tdRow14);

                            TableCell tdRow6 = new TableCell();
                            tdRow6.Width = Unit.Percentage(8);                           
                            tdRow6.Text = app.SCCount.ToString();
                            tdRow6.HorizontalAlign = HorizontalAlign.Left;
                            tdRow6.Wrap = false;
                            tr.Cells.Add(tdRow6);

                            TableCell tdRow7 = new TableCell();
                            tdRow7.Width = Unit.Percentage(10);                            
                            tdRow7.Text = app.STCount.ToString();
                            tdRow7.HorizontalAlign = HorizontalAlign.Center;
                            tdRow7.Wrap = false;
                            tr.Cells.Add(tdRow7);

                            TableCell tdRow8 = new TableCell();
                            tdRow8.Width = Unit.Percentage(1);
                            tdRow8.Text = app.OBCCount.ToString();
                            tdRow8.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow8);
                            
                            TableCell tdRow10 = new TableCell();
                            tdRow10.Width = Unit.Percentage(15);
                            tdRow10.Text = app.GeneralCount.ToString();
                            tdRow10.HorizontalAlign = HorizontalAlign.Left;
                            tdRow10.Wrap = false;
                            tr.Cells.Add(tdRow10);                           

                            TableCell tdRow11 = new TableCell();
                            tdRow11.Width = Unit.Percentage(2);                           
                            tdRow11.Text = app.PHCount.ToString();
                            tdRow11.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow11);

                            TableCell tdRow12 = new TableCell();
                            tdRow12.Width = Unit.Percentage(4);                            
                            tdRow12.Text = app.WomenCount.ToString().ToUpper();
                            tdRow12.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow12);

                            TableCell tdRow15 = new TableCell();
                            tdRow15.Width = Unit.Percentage(8);
                            tdRow15.Text = app.CertifiedStudents.ToString();
                            tdRow15.HorizontalAlign = HorizontalAlign.Left;
                            tdRow15.Wrap = false;
                            tr.Cells.Add(tdRow15);

                            TableCell tdRow16 = new TableCell();
                            tdRow16.Width = Unit.Percentage(8);
                            tdRow16.Text = app.whetherPlaced.ToString();
                            tdRow16.HorizontalAlign = HorizontalAlign.Left;
                            tdRow16.Wrap = false;
                            tr.Cells.Add(tdRow16);                           

                            tbl.Rows.Add(tr);
                            i++;
                        } 
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }                    
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "No Record Found";
                }
            } 
            LblRptSubHeader.Text = strHead;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally { 
        }
    }
    public DataTable GetStudentData(Int64 InstituteID, Int64 CourseId ,DateTime BatchFromDate, DateTime BatchToDate)
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetStudentDataCourseBatchWise", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;                
                cmd.Parameters.Add(new SqlParameter("@InstituteID", SqlDbType.BigInt));
                cmd.Parameters["@InstituteID"].Value = InstituteID;
                cmd.Parameters.Add(new SqlParameter("@CourseId", SqlDbType.BigInt));
                cmd.Parameters["@CourseId"].Value = CourseId;
                cmd.Parameters.Add(new SqlParameter("@BatchFromDate", SqlDbType.Date));
                cmd.Parameters["@BatchFromDate"].Value = BatchFromDate;
                cmd.Parameters.Add(new SqlParameter("@BatchToDate", SqlDbType.Date));
                cmd.Parameters["@BatchToDate"].Value = BatchToDate;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
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
            // Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=StudentBatchWiseReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

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
            ShowData();
            divReportData.Controls.Add(tbl);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=StudentBatchWiseReport.xls");
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