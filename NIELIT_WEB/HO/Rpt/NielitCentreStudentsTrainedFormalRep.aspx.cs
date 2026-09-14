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

public partial class NielitCentreStudentsTrainedFormalRep : BasePage
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/Rpt/NielitCentreStudentTrainedFormalFilter.aspx"))
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
            string centreType=Request.QueryString["centreType"].ToString();
            string centreName="";
            if(centreType =="C")
            {
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    var centre = (from s in context.NielitCentres
                                  where s.ID == centreID
                                  select new { name = s.Name }).FirstOrDefault();
                    if(centre!=null)
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


                TableHeaderCell tc1 = new TableHeaderCell();
                tc1.Width = Unit.Percentage(100);

                TableHeaderRow th1 = new TableHeaderRow();
                tc1.ColumnSpan = 6;
                tc1.Text = "Report for Students Trained by " + centreName + "<br/> Date :  From " + FromDate.ToString("dd-MMM-yyyy") + "  To " + ToDate.ToString("dd-MMM-yyyy") + "";
                tc1.HorizontalAlign = HorizontalAlign.Center;
                th1.Cells.Add(tc1);

                tbl.Rows.Add(th1);

                TableHeaderRow th = new TableHeaderRow();
                TableHeaderCell tcCol1 = new TableHeaderCell();
                tcCol1.ColumnSpan = 6;
                tcCol1.Width = Unit.Percentage(1);
                tcCol1.Text = "Report Generated on " + System.DateTime.Today.ToLongDateString();
                tcCol1.HorizontalAlign = HorizontalAlign.Right;
                th.Cells.Add(tcCol1);
                tbl.Rows.Add(th);

                TableHeaderRow th2 = new TableHeaderRow();
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

                TableHeaderCell tcCol3 = new TableHeaderCell();
                tcCol3.Width = Unit.Percentage(1);
                tcCol3.Text = "<b>Course Type</b>";
                tcCol3.HorizontalAlign = HorizontalAlign.Center;
                th2.Cells.Add(tcCol3);

                TableHeaderCell tcCol4 = new TableHeaderCell();
                tcCol4.Width = Unit.Percentage(5);
                tcCol4.Text = "<b>Course Category</b>";
                tcCol4.HorizontalAlign = HorizontalAlign.Center;
                th2.Cells.Add(tcCol4);

                TableHeaderCell tcCol44 = new TableHeaderCell();
                tcCol44.Width = Unit.Percentage(5);
                tcCol44.Text = "<b>Semester</b>";
                tcCol44.HorizontalAlign = HorizontalAlign.Center;
                th2.Cells.Add(tcCol44);


                TableHeaderCell tcCol6 = new TableHeaderCell();
                tcCol6.Width = Unit.Percentage(8);
                tcCol6.Text = "<b>Total students trained</b>";
                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                th2.Cells.Add(tcCol6);



                tbl.Rows.Add(th2);
            
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
          
          
            Int64 centreId = Convert.ToInt64(Request.QueryString["centreId"]);
            DateTime FromDate = Convert.ToDateTime(Request.QueryString["DateFrom"]);
            DateTime ToDate = Convert.ToDateTime(Request.QueryString["DateTo"]);
            string centreType = Request.QueryString["centreType"];

            StringBuilder mySql = new StringBuilder();



            using (DataTable dt = GetStudentTrained(centreId,centreType,FromDate, ToDate))
            {
                if (dt.Rows.Count > 0)
                {
                    var application = (from a in dt.AsEnumerable()
                                       select new
                                       {
                                           courseName = a.Field<string>("courseName"),
                                           courseCategory = a.Field<string>("courseCategory"),
                                           courseType = a.Field<string>("courseType"),
                                                                                    
                                           NumberOfStudentsTrained = a.Field<int>("NumberOfStudentsTrained"),
                                           semesterid = a.Field<int>("semesterid"),
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
                            tdRow2.Width = Unit.Percentage(8);
                            string dddd = app.courseName;
                            tdRow2.Text = app.courseName.ToString();
                            tdRow2.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow2);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(4);
                            tdRow3.Text = app.courseType .ToString();
                            tdRow3.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow3);

                               

                            TableCell tdRow5 = new TableCell();
                            tdRow5.Width = Unit.Percentage(7);                           
                            tdRow5.Text = app.courseCategory .ToString();
                            tdRow5.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow5);

                            TableCell tdRow55 = new TableCell();
                            tdRow55.Width = Unit.Percentage(7);
                            tdRow55.Text = app.semesterid.ToString();
                            tdRow55.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow55);

                            TableCell tdRow13 = new TableCell();
                            tdRow13.Width = Unit.Percentage(1);
                            tdRow13.Text = app.NumberOfStudentsTrained.ToString();
                            tdRow13.HorizontalAlign = HorizontalAlign.Left;
                            tdRow13.Wrap = false;
                            tr.Cells.Add(tdRow13);

                           
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
    public DataTable GetStudentTrained(Int64 centreId,string centreType, DateTime FromDate, DateTime ToDate)
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetStudentsTrainedCentreForFormal", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@centreID", SqlDbType.BigInt));
                cmd.Parameters["@centreID"].Value = centreId;
                cmd.Parameters.Add(new SqlParameter("@centreType", SqlDbType.VarChar ,1));
                cmd.Parameters["@centreType"].Value = centreType;
                cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.Date));
                cmd.Parameters["@FromDate"].Value = FromDate;
                cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.Date));
                cmd.Parameters["@ToDate"].Value = ToDate;
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
            Response.AddHeader("content-disposition", "attachment;filename=StudentTrainedReport.pdf");
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