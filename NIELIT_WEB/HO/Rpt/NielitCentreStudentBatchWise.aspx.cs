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
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/Rpt/NielitCentreStudentBatchFilter.aspx"))
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
            Int64 BatchId = Convert.ToInt64(Request.QueryString["BatchId"]);
            // String CourseName = Request.QueryString["CName"].ToUpper().ToString();
            String CourseName = "", BatchName="", BatchCode="";
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
			 NIELITMISContext context1 = new NIELITMISContext();
                        NielitCentreBatch BatchNameC = context1.NielitCentreBatchs.Where(s => s.ID == BatchId).FirstOrDefault();
                        BatchName = BatchNameC.Name;
                        BatchCode = BatchNameC.BatchCode;
                    }
                }
                else
                {
                    NIELITMISContext context1 = new NIELITMISContext();

                    if (CourseCatId != 0)
                    {
                        NielitCentreCourse cName = context1.NielitCentreCourses.Where(s => s.ID == CourseId).FirstOrDefault();
                        CourseName = cName.Name;
                        NielitCentreBatch BatchNameC = context1.NielitCentreBatchs.Where(s => s.ID == BatchId).FirstOrDefault();
                        BatchName = BatchNameC.Name;
                        BatchCode = BatchNameC.BatchCode;
                    }

                }

            }
            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);

            TableHeaderRow th1 = new TableHeaderRow();
            tc1.ColumnSpan = 16;
            tc1.Text = " Report of Course ( " + CourseName + "  ) Batch Name: " + BatchName + "  <br/>  Batch Code : " + BatchCode + " ";
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
            tcCol1.Text = "<b>Online Reference Number</b>";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(1);
            tcCol2.Text = "<b>Name</b>";
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
            
            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(8);
            tcCol6.Text = "<b>Cast Category</b>";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);


            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(5);
            tcCol7.Text = "<b>Mobile</b>";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);


            TableHeaderCell tcCol8 = new TableHeaderCell();
            tcCol8.Width = Unit.Percentage(3);
            tcCol8.Text = "<b>Email</b>";
            tcCol8.HorizontalAlign = HorizontalAlign.Center;
            tcCol8.Wrap = false;
            th.Cells.Add(tcCol8);


            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(15);
            tcCol9.Text = "<b>Date of Birth</b>";
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol9);


            TableHeaderCell tcCol10 = new TableHeaderCell();
            tcCol10.Width = Unit.Percentage(2);
            tcCol10.Text = "<b>Address</b>";
            tcCol10.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol10);


            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.Width = Unit.Percentage(2);
            tcCol11.Text = "<b>Application Status</b>";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol11);


            TableHeaderCell tcCol16 = new TableHeaderCell();
            tcCol16.Width = Unit.Percentage(2);
            tcCol16.Text = "<b>Project Student </b>";
            tcCol16.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol16);

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
            Int64 BatchId = Convert.ToInt64(Request.QueryString["BatchId"]);
            int CastCatid = Convert.ToInt32(Request.QueryString["Category"]);
            string Gender = Request.QueryString["Gender"].ToString();
            int PhyHandicapedId = Convert.ToInt32(Request.QueryString["PhyH"]);
            int WhetherProjStudent = Convert.ToInt32(Request.QueryString["WhProStu"]);
            string ProjectName = Request.QueryString["ProjName"];
            Int64 ProjectId = Convert.ToInt64(ProjectName);            

            StringBuilder mySql = new StringBuilder();
           
            string strHead = "";

            using (DataTable dt = GetStudentData(InstituteID, BatchId))
            {
                if (dt.Rows.Count > 0)
                {
                    var application = (from a in dt.AsEnumerable()
                                       select new
                                       {
                                           OnlineReferenceNumber = a.Field<string>("OnlineReferenceNumber"),
                                           StudentName = a.Field<string>("StudentName"),
                                           Fathername = a.Field<string>("Fathername"),
                                           Guardianname = a.Field<string>("Guardianname"),
                                           Mothername = a.Field<string>("Mothername"),
                                           CastCategoryID = a.Field<string>("CastName"),
                                           CastCatID = a.Field<int>("CastCategoryID"),
                                           Is_Handicaped = a.Field<string>("Is_Handicaped"),
                                           whetherProjectStudent = a.Field<string>("whetherProjectStudent"),
                                           Mobile = a.Field<Int64>("Mobile"),
                                           EmailId = a.Field<string>("EmailId"),
                                           Dob = a.Field<DateTime>("Dob"),
                                           Add1 = a.Field<string>("Add1"),
                                           Add2 = a.Field<string>("Add2"),
                                           Add3 = a.Field<string>("Add3"),
                                           Gender = a.Field<string>("Gender"),
                                           ProjectID = a.Field<string>("ProjectID"),
                                           ProjectIDD = a.Field<Int64>("ProjectIDD"),
                                           ApplicationStatus = a.Field<string>("ApplicationStatus")                                          
                                       }).ToList();
                   
                    if (Gender != "0")
                    {
                        application = application.Where(s => s.Gender == Gender).ToList();

                    }
                    if (CastCatid != 0)
                    {
                        application = application.Where(s => s.CastCatID == CastCatid).ToList();

                    }

                    if (PhyHandicapedId != 0)
                    {
                        if (PhyHandicapedId == 2)
                        {
                            PhyHandicapedId = 0;
                        }
                        application = application.Where(s => s.Is_Handicaped == PhyHandicapedId.ToString()).ToList();
                    }

                    if (WhetherProjStudent != 0)
                    {
                        if (WhetherProjStudent == 2)
                        {
                            WhetherProjStudent = 0;
                        }
                        application = application.Where(s => s.whetherProjectStudent == WhetherProjStudent.ToString()).ToList();
                    }
                    if (ProjectId != 0)
                    {
                        application = application.Where(s => s.ProjectIDD == ProjectId).ToList();

                    }
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
                            NielitCentreStudent NielitCentreStudentRefNo = context.NielitCentreStudent.Where(s => s.Number == app.OnlineReferenceNumber).FirstOrDefault();
                            if (NielitCentreStudentRefNo != null)
                                tdRow2.Text = NielitCentreStudentRefNo.Number.ToString();
                            else
                                tdRow2.Text = "";
                            tdRow2.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow2);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(1);
                            tdRow3.Text = app.StudentName.ToString();
                            tdRow3.HorizontalAlign = HorizontalAlign.Right;
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


                                TableCell tdRow35 = new TableCell();
                                tdRow35.Width = Unit.Percentage(8);
                                if (app.Mothername != null)
                                    tdRow35.Text = app.Mothername.ToUpper();
                                tdRow35.HorizontalAlign = HorizontalAlign.Left;
                                tdRow35.Wrap = false;
                                tr.Cells.Add(tdRow35);
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
                            if (app.CastCategoryID != null)
                                tdRow5.Text = app.CastCategoryID.ToString().ToUpper();
                            tdRow5.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow5);

                            TableCell tdRow6 = new TableCell();
                            tdRow6.Width = Unit.Percentage(8);
                            if (app.Mobile != null)
                                tdRow6.Text = app.Mobile.ToString();
                            tdRow6.HorizontalAlign = HorizontalAlign.Left;
                            tdRow6.Wrap = false;
                            tr.Cells.Add(tdRow6);

                            TableCell tdRow7 = new TableCell();
                            tdRow7.Width = Unit.Percentage(10);
                            if (app.EmailId != null)
                                tdRow7.Text = app.EmailId.ToUpper();
                            tdRow7.HorizontalAlign = HorizontalAlign.Center;
                            tdRow7.Wrap = false;
                            tr.Cells.Add(tdRow7);

                            TableCell tdRow8 = new TableCell();
                            tdRow8.Width = Unit.Percentage(1);
                            if (app.Dob != null)
                                tdRow8.Text = app.Dob.ToString("dd-MMM-yyyy");
                            tdRow8.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow8);
                            
                            TableCell tdRow10 = new TableCell();
                            tdRow10.Width = Unit.Percentage(15);
                            if (!String.IsNullOrEmpty(app.Add1) && !String.IsNullOrWhiteSpace(app.Add1))
                                tdRow10.Text = app.Add1.ToString().Replace("#", "").ToUpper();
                            if (!String.IsNullOrEmpty(app.Add2) && !String.IsNullOrWhiteSpace(app.Add2))
                                tdRow10.Text += " " + app.Add2.ToString().Replace("#", "").ToUpper();
                            if (!String.IsNullOrEmpty(app.Add3) && !String.IsNullOrWhiteSpace(app.Add3))
                                tdRow10.Text += " " + app.Add3.ToString().Replace("#", "").ToUpper();
                            tdRow10.HorizontalAlign = HorizontalAlign.Left;
                            tdRow10.Wrap = false;
                            tr.Cells.Add(tdRow10);                           

                            TableCell tdRow11 = new TableCell();
                            tdRow11.Width = Unit.Percentage(2);
                            if (app.ApplicationStatus != null)
                                tdRow11.Text = app.ApplicationStatus.ToString().ToUpper();
                            tdRow11.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow11);

                            TableCell tdRow12 = new TableCell();
                            tdRow12.Width = Unit.Percentage(4);
                            if (app.ProjectID != null)
                                tdRow12.Text = app.ProjectID.ToString().ToUpper();
                            tdRow12.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow12);                          

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
    public DataTable GetStudentData(Int64 InstituteID, Int64 BatchId)
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetStudentDataBatchWise", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;                
                cmd.Parameters.Add(new SqlParameter("@InstituteID", SqlDbType.BigInt));
                cmd.Parameters["@InstituteID"].Value = InstituteID;
                cmd.Parameters.Add(new SqlParameter("@BatchId", SqlDbType.BigInt));
                cmd.Parameters["@BatchId"].Value = BatchId;
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