using System ; 
using System.Web ; 
using System.Web.UI ; 
using System.Web.UI.WebControls ; 
using EConnect ; 
using EConnect.URM ; 
using EConnect.DAL ; 
using EConnect.Utils.Data ; 
using System.Data ;
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

public partial class NielitCentreBatchwiseFinancialRep : BasePage
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

            //currentRoleId = 6 ;

            if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/Rpt/NielitCentreBatchwiseFinancialRepFilter.aspx"))
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

                ShowData() ; 
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
            int centreID = Convert.ToInt32(Request.QueryString["InstId"]);
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
                                  where s.ID == centreID
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

            int CourseCatId = Convert.ToInt32(Request.QueryString["CourseCatId"]) ;  

            Int64 CourseId = Convert.ToInt64(Request.QueryString["CourseId"]);
            Int64 BatchId = Convert.ToInt64(Request.QueryString["BatchId"]);
 
                    TableHeaderCell tc1 = new TableHeaderCell();
                    tc1.Width = Unit.Percentage(100);

                    TableHeaderRow th1 = new TableHeaderRow() ; 
                    tc1.ColumnSpan = 16 ; 
                    tc1.Text = " Batchwise Financial report" ; 
                    tc1.HorizontalAlign = HorizontalAlign.Center ; 
                    th1.Cells.Add(tc1);

                    tbl.Rows.Add(th1) ; 

                    TableHeaderRow th2 = new TableHeaderRow();
                    TableHeaderCell tc2 = new TableHeaderCell();
                    tc2.Width = Unit.Percentage(100);
                    tc2.ColumnSpan = 16;
                    tc2.Text = " For " + centreName;
                    tc2.HorizontalAlign = HorizontalAlign.Center;
                    th2.Cells.Add(tc2);

                    tbl.Rows.Add(th2) ;  

                    TableHeaderRow th3 = new TableHeaderRow();
                    TableHeaderCell tc3 = new TableHeaderCell();
                    tc3.Width = Unit.Percentage(100);
                    tc3.ColumnSpan = 16;
                    tc3.Text = " Report generated on " + System.DateTime .Now.ToLongDateString ();
                    tc3.HorizontalAlign = HorizontalAlign.Right;
                    th3.Cells.Add(tc3);

                    tbl.Rows.Add(th2);
                    TableHeaderRow th = new TableHeaderRow() ; 
                    th.CssClass = "head1" ; 

                    TableHeaderCell tc = new TableHeaderCell() ; 
                    tc.Width = Unit.Percentage(1) ; 
                    tc.Text = "<b>#</b>" ; 
                    tc.HorizontalAlign = HorizontalAlign.Right ; 
                    th.Cells.Add(tc) ; 

                    TableHeaderCell tcCol1 = new TableHeaderCell();
                    tcCol1.Width = Unit.Percentage(1);
                    tcCol1.Text = "<b>Online Reference Number</b>" ; 
                    tcCol1.HorizontalAlign = HorizontalAlign.Center; 
                    th.Cells.Add(tcCol1);

                    TableHeaderCell tcCol2 = new TableHeaderCell();
                    tcCol2.Width = Unit.Percentage(1);
                    tcCol2.Text = "<b>Name of student</b>";
                    tcCol2.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol2);

                    TableHeaderCell tcCol3 = new TableHeaderCell();
                    tcCol3.Width = Unit.Percentage(1);
                    tcCol3.Text = "<b>Father/Guardian Name</b>";
                    tcCol3.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol3);

                    TableHeaderCell tcCol4 = new TableHeaderCell();
                    tcCol4.Width = Unit.Percentage(5);
                    tcCol4.Text = "<b>Mother Name</b>";
                    tcCol4.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol4);

                    TableHeaderCell tcCol5 = new TableHeaderCell();
                    tcCol5.Width = Unit.Percentage(8);
                    tcCol5.Text = "<b>Gender</b>";   
                    tcCol5.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol5);

                    TableHeaderCell tcCol6 = new TableHeaderCell() ;    
                    tcCol6.Width = Unit.Percentage(9);
                    tcCol6.Text = "<b>Date of Birth</b>";
                    tcCol6.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol6);

                    TableHeaderCell tcCol7 = new TableHeaderCell() ;  
                    tcCol7.Width = Unit.Percentage(8);
                    tcCol7.Text = "<b>Course Start Date</b>";
                    tcCol7.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol7);

                    TableHeaderCell tcCol8 = new TableHeaderCell();
                    tcCol8.Width = Unit.Percentage(8);
                    tcCol8.Text = "<b>Course End Date</b>";
                    tcCol8.HorizontalAlign = HorizontalAlign.Center;
                    tcCol8.Wrap = false;
                    th.Cells.Add(tcCol8);

                    TableHeaderCell tcCol9 = new TableHeaderCell();
                    tcCol9.Width = Unit.Percentage(15);
                    tcCol9.Text = "<b>Cast Category</b>";
                    tcCol9.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol9);


                    TableHeaderCell tcCol10 = new TableHeaderCell();
                    tcCol10.Width = Unit.Percentage(8);
                    tcCol10.Text = "<b>Registration No.</b>";
                    tcCol10.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol10);


                    TableHeaderCell tcCol11 = new TableHeaderCell();
                    tcCol11.Width = Unit.Percentage(8);
                    tcCol11.Text = "<b>Mobile No.</b>";
                    tcCol11.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol11);


                    TableHeaderCell tcCol16 = new TableHeaderCell();
                    tcCol16.Width = Unit.Percentage(8);
                    tcCol16.Text = "<b>Admission Date </b>";
                    tcCol16.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol16);

                    TableHeaderCell tcCol17 = new TableHeaderCell();
                    tcCol17.Width = Unit.Percentage(8);
                    tcCol17.Text = "<b>Total Tution Fees </b>";
                    tcCol17.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol17);

                    TableHeaderCell tcCol18 = new TableHeaderCell();
                    tcCol18.Width = Unit.Percentage(8);
                    tcCol18.Text = "<b>Total Tution Fees Paid </b>";
                    tcCol18.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol18);

                    TableHeaderCell tcCol19 = new TableHeaderCell();
                    tcCol19.Width = Unit.Percentage(8);
                    tcCol19.Text = "<b>Balance Amount </b>";
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
            int centreID = Convert.ToInt32(Request.QueryString["InstId"]);
            string centreType = Request.QueryString["centreType"].ToString();
            int CourseCatId = Convert.ToInt32(Request.QueryString["CourseCatId"]);

            Int32 CourseId = Convert.ToInt32(Request.QueryString["CourseId"]); 
            Int32 BatchId = Convert.ToInt32(Request.QueryString["BatchId"]);

            StringBuilder mySql = new StringBuilder();

            string strHead = "";
            NIELITMISContext context = new NIELITMISContext() ; 

        using (DataTable dt = BatchwiseFinacialReport(centreID, centreType, CourseCatId, CourseId, BatchId))
            { 
                if (dt.Rows.Count > 0)
                {
                    var application = (from a in dt.AsEnumerable()
                                       select new
                                       {
                                           OnlineRefNo = a.Field<string>("OnlineRefNo"),
                                           courseName = a.Field<string>("courseName"),
                                           courseCategory = a.Field<string>("courseCategory"),
                                           batchCode = a.Field<string>("batchCode"),
                                           batchName = a.Field<string>("batchName"),
                                           StudentName = a.Field<string>("stuName"),
                                           Fathername = a.Field<string>("fathName"),
                                           Mothername = a.Field<string>("mothName"),
                                           Guardianname=a.Field <string>("GuardianName"),
                                           gender = a.Field<string>("gender"),
                                           dob = a.Field<DateTime>("dob"),
                                           courseStart = a.Field<DateTime>("courseStart"),
                                           courseEnd = a.Field<DateTime>("courseEnd"),
                                           castCategory = a.Field<string>("castCategory"),
                                           regnNo = a.Field<string>("regnNo"),
                                           mobNo = a.Field<Int64>("mobNo"),
                                           admDate = a.Field<DateTime>("admDate"),
                                           totalFees = a.Field<Int32>("totalFees"),
                                           feesPaid = a.Field<Int32>("feesPaid"),
                                           feesBal = a.Field<Int32>("feesBal"), 
                                        }).ToList() ;
                     
                    if (application.Count() > 0) 
                    {
                        ShowTableHeader() ; 
                        int i = 1;
                        foreach (var app in application) 
                        {
                            TableRow tr = new TableRow() ; 
                            if (i % 2 == 0)
                                tr.CssClass = "gdalternate1" ; 
                            else
                                tr.CssClass = "gdrow1"  ;

                            TableCell tdRow = new TableCell() ; 
                            tdRow.Width = Unit.Percentage(1); 
                            tdRow.Text = i.ToString(); 
                            tdRow.HorizontalAlign = HorizontalAlign.Right; 
                            tr.Cells.Add(tdRow);

                            TableCell tdRow2 = new TableCell(); 
                            tdRow2.Width = Unit.Percentage(1); 

                            NielitCentreStudent NielitCentreStudentRefNo = context.NielitCentreStudent.Where(s => s.Number == app.OnlineRefNo ).FirstOrDefault();

                            if (NielitCentreStudentRefNo != null)
                                tdRow2.Text = NielitCentreStudentRefNo.Number.ToString(); 
                            else
                                tdRow2.Text = "";

                            tdRow2.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow2);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(1);
                            tdRow3.Text = app.StudentName.ToString();
                            tdRow3.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow3);

                            if (string.IsNullOrEmpty(app.Guardianname) == true && string.IsNullOrWhiteSpace(app.Guardianname) == true)
                            {
                                TableCell tdRow45 = new TableCell();
                                tdRow45.Width = Unit.Percentage(8);
                                if (app.Fathername != null)
                                    tdRow45.Text = app.Fathername.ToUpper();
                                tdRow45.HorizontalAlign = HorizontalAlign.Left;
                                tdRow45.Wrap = false;
                                tr.Cells.Add(tdRow45);

                                TableCell tdRow35 = new TableCell() ; 
                                tdRow35.Width = Unit.Percentage(8) ; 
                                if (app.Mothername != null)
                                    tdRow35.Text = app.Mothername.ToUpper() ; 
                                tdRow35.HorizontalAlign = HorizontalAlign.Left ; 
                                tdRow35.Wrap = false ; 
                                tr.Cells.Add(tdRow35) ; 
                            }
                            else
                            { 
                                TableCell tdRow45 = new TableCell();
                                tdRow45.Width = Unit.Percentage(8);
                                if (app.Guardianname != null)
                                    tdRow45.Text = app.Guardianname.ToUpper();
                                tdRow45.HorizontalAlign = HorizontalAlign.Left;
                                tdRow45.Wrap = false;
                                tr.Cells.Add(tdRow45);

                                TableCell tdRow46 = new TableCell();
                                tdRow46.Width = Unit.Percentage(8);
                                tdRow46.Text = "NA";
                                tdRow46.HorizontalAlign = HorizontalAlign.Left;
                                tdRow46.Wrap = false;
                                tr.Cells.Add(tdRow46);
                            }

                            TableCell tdRow5 = new TableCell();
                            tdRow5.Width = Unit.Percentage(1);
                            if (app.gender != null)
                                tdRow5.Text = app.gender.ToString().ToUpper();
                            tdRow5.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow5);

                            TableCell tdRow6 = new TableCell();
                            tdRow6.Width = Unit.Percentage(8);
                            if (app.dob != null)
                                tdRow6.Text = app.dob.ToString("dd-MMM-yyyy"); 
                            tdRow6.HorizontalAlign = HorizontalAlign.Left;
                            tdRow6.Wrap = false;
                            tr.Cells.Add(tdRow6); 

                            TableCell tdRow7 = new TableCell();
                            tdRow7.Width = Unit.Percentage(10);
                            if (app.courseStart  != null) 
                                tdRow7.Text = app.courseStart.ToString("dd-MMM-yyyy"); 
                            tdRow7.HorizontalAlign = HorizontalAlign.Center;
                            tdRow7.Wrap = false;
                            tr.Cells.Add(tdRow7);

                            TableCell tdRow8 = new TableCell();
                            tdRow8.Width = Unit.Percentage(1);
                            if (app.courseEnd != null)
                                tdRow8.Text = app.courseEnd.ToString("dd-MMM-yyyy");
                            tdRow8.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow8);
                            
                            TableCell tdRow10 = new TableCell();
                            tdRow10.Width = Unit.Percentage(15);
                            tdRow10.Text = app.castCategory.ToString();
                            tdRow10.HorizontalAlign = HorizontalAlign.Left;
                            tdRow10.Wrap = false;
                            tr.Cells.Add(tdRow10);                           

                            TableCell tdRow11 = new TableCell();
                            tdRow11.Width = Unit.Percentage(2);
                            if (app.regnNo != null)
                                tdRow11.Text = app.regnNo;
                            tdRow11.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow11);

                            TableCell tdRow12 = new TableCell() ; 
                            tdRow12.Width = Unit.Percentage(4);
                            if (app.mobNo != null)
                                tdRow12.Text = app.mobNo.ToString();
                            tdRow12.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow12);

                            TableCell tdRow13 = new TableCell();
                            tdRow13.Width = Unit.Percentage(4);
                            if (app.admDate != null)
                                tdRow13.Text = app.admDate.ToString("dd-MMM-yyyy");
                            tdRow13.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow13);

                            TableCell tdRow14 = new TableCell();
                            tdRow14.Width = Unit.Percentage(4);
                            if (app.totalFees != null)
                                tdRow14.Text = app.totalFees.ToString();
                            tdRow14.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow14);

                            TableCell tdRow15 = new TableCell();
                            tdRow15.Width = Unit.Percentage(4);
                            if (app.feesPaid  != null)
                                tdRow15.Text = app.feesPaid.ToString();
                            tdRow15.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow15);

                            TableCell tdRow16 = new TableCell();
                            tdRow16.Width = Unit.Percentage(4);
                            if (app.feesBal  != null)
                                tdRow16.Text = app.feesBal.ToString() ; 
                            tdRow16.HorizontalAlign = HorizontalAlign.Left ; 
                            tr.Cells.Add(tdRow16) ; 

                            tbl.Rows.Add(tr) ; 
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
        finally
        {
        }
    }


    public DataTable BatchwiseFinacialReport(int centreID,string centreType,int CourseCatId,Int32 CourseId,Int32 BatchId)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString ; 
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("RepBatchwiseFinancial", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@centreID", SqlDbType.BigInt));
                cmd.Parameters["@centreID"].Value = centreID;
                cmd.Parameters.Add(new SqlParameter("@centreType", SqlDbType.VarChar ,1));
                cmd.Parameters["@centreType"].Value = centreType ;
                cmd.Parameters.Add(new SqlParameter("@courseCategoryID", SqlDbType.BigInt)) ; 
                cmd.Parameters["@courseCategoryID"].Value = CourseCatId ; 
                cmd.Parameters.Add(new SqlParameter("@courseID", SqlDbType.BigInt)) ; 
                cmd.Parameters["@courseID"].Value = CourseId ; 
                cmd.Parameters.Add(new SqlParameter("@batchID", SqlDbType.BigInt)) ; 
                cmd.Parameters["@batchID"].Value = BatchId ; 

                con.Open() ; 
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd)) 
                { 
                    sda.Fill(myDt) ; 
                } 
            }
        }
        return myDt ; 
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
            divReportData.Visible = true ; 
            ShowData() ; 
            divReportData.Controls.Add(tbl) ; 
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