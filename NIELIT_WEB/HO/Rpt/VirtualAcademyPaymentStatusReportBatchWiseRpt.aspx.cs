using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Linq;
using EConnect.Utils.Common;
using System.Collections.Generic;
using System.Web;
using System.Data.Objects;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

public partial class HO_Rpt_VirtualAcademyPaymentStatusReportBatchWiseRpt : BasePage
    {
    Table tbl = new Table();    
    public static int j;
    Int32 currentRoleId = 0;

    Int32 CentreID = 0;
    Int32 CourseCatId = 0;
    Int32 CourseId = 0;
    Int32 BatchId = 0;

    string CentreName = "";
    string courseCatName = "";
    string courseName = "";
    string BatchName = "";
    

    protected void Page_Load(object sender, EventArgs e)
        {
        lblError.Visible = false;
        try
            {
            if (!IsSessionAlive())
                {
                Response.Redirect("~/index.aspx");
                }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/VirtualAcademyPaymentStatusReportBatchWise.aspx"))
                {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
                }
            if (!Page.IsPostBack)
                {
                newShowData();
                tbl.CssClass = "sample3";
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(100); ;
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
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(2);
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            tcCol1.Text = "#";
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol14 = new TableHeaderCell();
            tcCol14.Width = Unit.Percentage(7);
            tcCol14.HorizontalAlign = HorizontalAlign.Center;
            tcCol14.Text = "Number";
            th.Cells.Add(tcCol14);

            TableHeaderCell tcCol15 = new TableHeaderCell();
            tcCol15.Width = Unit.Percentage(12);
            tcCol15.HorizontalAlign = HorizontalAlign.Center;
            tcCol15.Text = "Name";
            th.Cells.Add(tcCol15);

            //if (FatherName != "")
            //    {
                TableHeaderCell tcCol16 = new TableHeaderCell();
                tcCol16.Width = Unit.Percentage(12);
                tcCol16.HorizontalAlign = HorizontalAlign.Center;
                tcCol16.Text = "Father Name";
                th.Cells.Add(tcCol16);
                //}

            //else (FatherName == "")
            //    {
            //    TableHeaderCell tcCol18 = new TableHeaderCell();
            //    tcCol18.Width = Unit.Percentage(6);
            //    tcCol18.HorizontalAlign = HorizontalAlign.Center;
            //    tcCol18.Text = "Guardin Name";
            //    th.Cells.Add(tcCol18);
            //    }

            //
                TableHeaderCell tcCol20 = new TableHeaderCell();
                tcCol20.Width = Unit.Percentage(7);
                tcCol20.HorizontalAlign = HorizontalAlign.Center;
                tcCol20.Text = "Mobile No";
                th.Cells.Add(tcCol20);

                TableHeaderCell tcCol21 = new TableHeaderCell();
                tcCol21.Width = Unit.Percentage(12);
                tcCol21.HorizontalAlign = HorizontalAlign.Center;
                tcCol21.Text = "Email ID";
                th.Cells.Add(tcCol21);
            //
            TableHeaderCell tcCol17 = new TableHeaderCell();
            tcCol17.Width = Unit.Percentage(8);
            tcCol17.HorizontalAlign = HorizontalAlign.Center;
            tcCol17.Text = "Paid Amt (Rs.)";
            th.Cells.Add(tcCol17);

            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(12);
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            tcCol9.Text = "Payment Status";
            th.Cells.Add(tcCol9);

            tbl.Rows.Add(th);
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }

        }
    protected void newShowData()
        {
        EConnectContext context = new EConnectContext(); ;
        NIELITMISContext contextmis = new NIELITMISContext();
        try
            {
            CentreID = Convert.ToInt32(Request.QueryString["CentreID"]);
            CourseCatId = Convert.ToInt32(Request.QueryString["CourseCategory"]);
            CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            BatchId = Convert.ToInt32(Request.QueryString["BatchId"]);
            currentRoleId = Convert.ToInt32(Session["RoleID"]);

            string strHead = "";
            string strHead1 = "";

            #region Names
            // Centre Name
            if (CentreID != 0)
                {
                var Centre = contextmis.NielitCentres.Find(CentreID);
                if (Centre != null)
                    {
                    CentreName = Centre.Name.ToString();
                    strHead += "<br/><b>Centre Name : </b>" + CentreName;
                    TopHeading.Text = "Virtual Academy Batch Wise Payment Status Report - " + CentreName.ToString();
                    }
                }

            // Course Category
            if (CourseCatId != 0)
                {
                var category = context.CourseCategories.Find(CourseCatId);
                if (category != null)
                    {
                    courseCatName = category.Name.ToString();
                    strHead += "<br/><b>Course Category : </b>" + courseCatName;
                    }
                else
                    {
                    var category1 = contextmis.NielitCentreCourseCategorys.Find(CourseCatId);
                    if (category1 != null)
                        {
                        courseCatName = category1.Name.ToString();
                        strHead += "<br/><b>Course Category : </b>" + courseCatName;
                        }
                    }
                }

            // Course
            if (CourseId != 0)
                {
                var course = contextmis.NielitCourseDurations.Find(CourseId);
                if (course != null)
                    {
                    Int32 courses1 = course.courseID;
                    var courses = context.Courses.Find(courses1);
                    if (courses != null)
                        {
                        courseName = courses.Name.ToString();
                        strHead += "<br/><b>Course Name : </b>" + courseName;
                        }
                    else
                        {
                        var mm = contextmis.NielitCentreCourses.Find(courses1);
                        if (mm != null)
                            {
                            courseName = mm.Name.ToString();
                            strHead += "<br/><b>Course Name : </b>" + courseName;
                            }
                        }
                    }
                }
            //batch name
            if (BatchId != 0)
                {
                var Batch = contextmis.NielitCentreBatchs.Find(BatchId);
                if (Batch != null)
                    {
                    BatchName = Batch.Name.ToString();
                    strHead += "<br/><b>Batch Name : </b>" + BatchName;
                    }
                }
            #endregion

            if (CentreID != 0 && CourseCatId != 0 && CourseId != 0 && BatchId != 0)
                {
                string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("GetVirtualAcademyRecordsForBatchWisePaymentStatusReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pUserId", SqlDbType.BigInt);
                cmd.Parameters["@pUserId"].Value = currentRoleId;
                cmd.Parameters.Add("@CentreID", SqlDbType.BigInt);
                cmd.Parameters["@CentreID"].Value = CentreID;
                cmd.Parameters.Add("@CourseCategoryId", SqlDbType.BigInt);
                cmd.Parameters["@CourseCategoryId"].Value = CourseCatId;
                cmd.Parameters.Add("@CourseId", SqlDbType.BigInt);
                cmd.Parameters["@CourseId"].Value = CourseId;
                cmd.Parameters.Add("@BatchId", SqlDbType.BigInt);
                cmd.Parameters["@BatchId"].Value = BatchId;
                //scCommand.Parameters.Add("@ExamName", SqlDbType.VarChar).Value = Convert.ToString(ddlSessionExam.SelectedValue);

                cmd.CommandTimeout = 50000;
                if (cmd.Connection.State == ConnectionState.Closed)
                    {
                    cmd.Connection.Open();
                    }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                    {
                    ShowTableHeader();
                    int i;
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                        TableRow tr = new TableRow();
                        if (i % 2 == 0)
                            tr.CssClass = "gdalternate1";
                        else
                            tr.CssClass = "gdrow1";

                        TableCell tdRow = new TableCell();
                        tdRow.Width = Unit.Percentage(1);
                        tdRow.Text = (i + 1).ToString();
                        tdRow.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow);

                        TableCell tdRow1 = new TableCell();
                        tdRow1.Width = Unit.Percentage(7);
                        tdRow1.Text = ds.Tables[0].Rows[i]["Number"].ToString();
                        tdRow1.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow1);

                        TableCell tdRow2 = new TableCell();
                        tdRow2.Width = Unit.Percentage(12);
                        tdRow2.Text = ds.Tables[0].Rows[i]["Name"].ToString();
                        tdRow2.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(tdRow2);

                        TableCell tdRow3 = new TableCell();
                        tdRow3.Width = Unit.Percentage(12);
                        tdRow3.Text = ds.Tables[0].Rows[i]["FatherName"].ToString();
                        tdRow3.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(tdRow3);

                        TableCell tdRow5 = new TableCell();
                        tdRow5.Width = Unit.Percentage(7);
                        tdRow5.Text = ds.Tables[0].Rows[i]["Mobile"].ToString();
                        tdRow5.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow5);

                        TableCell tdRow6 = new TableCell();
                        tdRow6.Width = Unit.Percentage(12);
                        tdRow6.Text = ds.Tables[0].Rows[i]["Email"].ToString();
                        tdRow6.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(tdRow6);

                        TableCell tdRow4 = new TableCell();
                        tdRow4.Width = Unit.Percentage(7);
                        tdRow4.Text = ds.Tables[0].Rows[i]["AmtPaid"].ToString();
                        tdRow4.HorizontalAlign = HorizontalAlign.Right;
                        tr.Cells.Add(tdRow4);

                        TableCell tdRow11 = new TableCell();
                        tdRow11.Width = Unit.Percentage(12);
                        tdRow11.Text = ds.Tables[0].Rows[i]["PaymentStatus"].ToString();
                        tdRow11.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(tdRow11);

                        tbl.Rows.Add(tr);
                        }

                    lblError.Visible = false;
                    //lblheading.Visible = false;
                    }
                else
                    {
                    LblRptSubHeader.Text = strHead;
                    lblError.Visible = true;
                    lblError.Text = "No Record Found !";
                    //lblheading.Visible = false;
                    throw new Exception("No Record Found !");
                    }

                LblRptSubHeader.Text = strHead;
                //LblRptSubHeader1.Text = strHead1;
                }
            }
        catch (Exception ex)
            {
            throw ex;
            //ShowAlert(ex.Message.ToString());
            }
        }
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
        {
        try
            {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
            divReportData.Visible = true;
            newShowData();
            divReportData.Controls.Add(tbl);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=VirtualAcademyPaymentStatusReportBatchWise.xls");
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
    protected void imgPDF_Click(object sender, ImageClickEventArgs e)
        {

        try
            {
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //hw.AddAttribute("border", "1");
            //hw.RenderBeginTag(HtmlTextWriterTag.Font);
            //hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "9");

            //divReportData.Visible = true;
            //tbl.BorderStyle = BorderStyle.Solid;
            //tbl.CssClass = "sample3";
            //tbl.CellPadding = 2;
            //tbl.CellSpacing = 1;
            //tbl.Width = Unit.Percentage(200);

            //newShowData();
            //divReportData.Controls.Add(tbl);

            //divReportData.RenderControl(hw);
            //hw.RenderEndTag();
            //Response.Clear();
            //StringReader sr = new StringReader(sw.ToString());
            //// Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);
            //Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);

            //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //pdfDoc.Open();
            //htmlparser.Parse(sr);
            //pdfDoc.Close();
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=VirtualAcademyPaymentStatusReportBatchWise.pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //Response.Write(pdfDoc);
            //Response.End();


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

            newShowData();

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
            Response.AddHeader("content-disposition", "attachment;filename=VirtualAcademyPaymentStatusReportBatchWise.pdf");
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