using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Data;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Transactions;
using EConnect.Utils.Common;
using System.Drawing;

//rptFinanceCandDetailsWithInstallmentsDetailsPuraskar.aspx  Protsahan Puraskar Candidates Report  (Finance)  Puraskar
public partial class PuraskarCandReportFin : BasePage
    {
    Table tbl = new Table();

    //EConnectContext context;
    Int32 currentRoleId = 0;
    protected void Page_Load(object sender, EventArgs e)
        {
        try
            {

            if (IsSessionAlive() == false)
                Response.Redirect("~/Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
                {
                //Response.Write("Sorry! You don't have rights  to view this page");
                //Response.End();
                }

            if (!Page.IsPostBack)
                {   
                ddlSessionExam.Items.Clear();
                FillSessionExam();
                }
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }

    #region Code
    protected void FillCategories()
        {
    //    try
    //        {
    //        using (EConnectContext context = new EConnectContext())
    //            {
    //            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
    //            var Category = (from s in context.CourseCategories
    //                            where s.ID == 1 && s.IsActive == true
    //                            orderby (s.Name)
    //                            select new { ValueField = s.ID, TextField = s.Name }).Distinct();
    //            if (Category != null)
    //                {
    //                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category, lst);
    //                }
    //            ddlcoursecategory.SelectedValue = "1";
    //            };
    //        }
    //    catch (Exception ex)
    //        {
    //        throw ex;
    //        }
        }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
        {
        //divReportData.Visible = false;
        }

    protected void FillCourses()
        {
    //    try
    //        {
    //        int CouCatid = 0;
    //        using (EConnectContext context = new EConnectContext())
    //            {
    //            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
    //            CouCatid = Convert.ToInt32(ddlcoursecategory.SelectedValue);
    //            var CourseList = from p in context.Courses
    //                             where p.CourseCategoryID == CouCatid && p.IsActive == true && p.ShowOnWeb == true
    //                             select new { ValueField = p.ID, TextField = p.Name };
    //            if (CourseList != null)
    //                {
    //                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourse, CourseList, lst);
    //                }
    //            };
    //        }
    //    catch (Exception ex)
    //        {
    //        throw ex;
    //        }
        }
    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
        {

        //divReportData.Visible = false;
        //lblheading.Visible = false;
        //lblError.Text = "";
        //lblError.Visible = false;
        //ddlExamCycle.Items.Clear();
        //ddlExamCycle.Items.Insert(0, "--Select One--");
        //ddlSessionExam.Items.Clear();
        //ddlSessionExam.Items.Insert(0, "--Select One--");
        //FillExamCycle();
        //divReportData.Visible = false;
        }

    protected void FillExamCycle()
        {
    //    try
    //        {
    //        int CourseCatID = 0, CourseID = 0;
    //        using (EConnectContext context = new EConnectContext())
    //            {
    //            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
    //            CourseCatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
    //            CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
    //            var ExamCycleList = from p in context.ExaminationCycles
    //                                where p.CourseCategoryID == CourseCatID && p.CourseID == CourseID
    //                                select new { ValueField = p.ID, TextField = p.Name };
    //            if (ExamCycleList != null)
    //                {
    //                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, ExamCycleList, lst);
    //                }
    //            };
    //        }
    //    catch (Exception ex)
    //        {
    //        throw ex;
    //        }
        }
    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
        {
        //ddlSessionExam.Items.Clear();
        //ddlSessionExam.Items.Insert(0, "--Select One--");
        //FillSessionExam();
        //divReportData.Visible = false;
        }

    protected void FillSessionExam()
        {
        try
            {
            Int32 CourseCategoryID = 0;//, CourseID = 0, ExamCycleID = 0;

            using (EConnectContext context = new EConnectContext())
                {
                CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                //CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                //ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);

                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var examCycleName = (from p in context.Exams
                                     where (p.CourseCategoryID == CourseCategoryID && p.DateOfPublishingOfResult != null)
                                     select new { ValueField = p.Name, TextField = p.Name, year=p.ExamYear }).Distinct();
                if (examCycleName.Count() != 0)
                    {
                    var examCycleName1 = examCycleName.OrderByDescending(c => c.year);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlSessionExam, examCycleName1, lst);
                    }
                };
            }
            //{
            //Int32 CourseCategoryID = 0, CourseID = 0, ExamCycleID = 0;
            
            //using (EConnectContext context = new EConnectContext())
            //    {
            //    CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            //    CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
            //    ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);

            //    System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
            //    Int32 currentyear = Convert.ToInt32(DateTime.Now.Year);
            //    var examYear = (from p in context.Exams
            //                    where (p.CourseID == CourseID && p.CourseCategoryID == CourseCategoryID && p.ExaminationCycleID == ExamCycleID && p.DateOfPublishingOfResult != null)
            //                    orderby (p.ExamYear) descending
            //                    select new { ValueField = p.ID, TextField = p.Name });
            //    if (examYear != null)
            //        {
            //        EConnect.Utils.Common.ControlUtility.BindListObject(ddlSessionExam, examYear, lst);
            //        }
            //    };
            //}
        catch (Exception ex)
            {
            throw ex;
            }
        }
    protected void ddlSessionExam_SelectedIndexChanged(object sender, EventArgs e)
        {
        divReportData.Visible = false;
        lblheading.Visible = false;
        lblError.Visible = false;
        }

    protected void btnShowReport_Click(object sender, EventArgs e)
        {
        try
            {
            tbl.CssClass = "sample3";
            tbl.CellPadding = 2;
            tbl.CellSpacing = 1;
            tbl.Width = Unit.Percentage(100);
            ShowData();
            divReportData.Controls.Add(tbl);
            divReportData.Visible = true;
            }
        catch (Exception ex)
            {
            //throw ex;
            ShowAlert(ex.Message);
            }
        }
    protected void imgPDF_Click(object sender, ImageClickEventArgs e)
        {
        if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
            {
            ShowAlert("Select Course Category");
            return;
            }

        if (ddlcourse.SelectedValue.ToString().Equals("0"))
            {
            ShowAlert("Select Course");
            return;
            }

        if (ddlExamCycle.SelectedValue.ToString().Equals("0"))
            {
            ShowAlert("Select Exam Cycle");
            return;
            }
        if (ddlSessionExam.SelectedValue.ToString().Equals("0"))
            {
            ShowAlert("Select Exam Session");
            return;
            }

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
            Response.AddHeader("content-disposition", "attachment;filename=ProtsahanStudentsReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }

    protected void ShowTableHeader()
        {
        if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
            {
            ShowAlert("Select Course Category");
            return;
            }

        if (ddlcourse.SelectedValue.ToString().Equals("0"))
            {
            ShowAlert("Select Course");
            return;
            }

        if (ddlExamCycle.SelectedValue.ToString().Equals("0"))
            {
            ShowAlert("Select Exam Cycle");
            return;
            }
        if (ddlSessionExam.SelectedValue.ToString().Equals("0"))
            {
            ShowAlert("Select Exam Session");
            return;
            }

        try
            {
            TableHeaderRow th1 = new TableHeaderRow();
            th1.CssClass = "head1";

            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);

            //string examcycle = ddlSessionExam.SelectedItem.Text;

            tc1.ColumnSpan = 12;
            tc1.Text = "Protsahan Puraskar Candidates Report  (Finance)  of " + ddlSessionExam.SelectedItem.Text + "";

            tc1.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tc1);

            tbl.Rows.Add(th1);

            TableHeaderRow th2 = new TableHeaderRow();
            //th2.CssClass = "head1";

            TableHeaderCell tc2 = new TableHeaderCell();
            tc2.Width = Unit.Percentage(100);

            tc2.ColumnSpan = 12;
            tc2.Text = "Report Generated on: " + System.DateTime.Now.ToLongDateString();
            tc2.HorizontalAlign = HorizontalAlign.Right;
            th2.Cells.Add(tc2);

            tbl.Rows.Add(th2);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "#";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(3);
            tcCol1.Text = "Level";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(9);
            tcCol2.Text = "Ref. Number";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(6);
            tcCol3.Text = "Regn. Number";
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(14);
            tcCol4.Text = "Name";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(14);
            tcCol5.Text = "Father's Name";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(5);
            tcCol6.Text = "Gender ";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(14);
            tcCol7.Text = "Cast Category";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);

            TableHeaderCell tcCol8 = new TableHeaderCell();
            tcCol8.Width = Unit.Percentage(3);
            tcCol8.Text = "Physically Handicapped";
            tcCol8.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol8);

            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(30);
            tcCol9.Text = "Module Name(s)";
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol9);

            TableHeaderCell tcCol10 = new TableHeaderCell();
            tcCol10.Width = Unit.Percentage(8);
            tcCol10.Text = "Amount (Rs.)";
            tcCol10.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol10);

            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.Width = Unit.Percentage(25);
            tcCol11.Text = "Application Status";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol11);

            //TableHeaderCell tcCol12 = new TableHeaderCell();
            //tcCol12.Width = Unit.Percentage(20);
            //tcCol12.Text = "Module Name";
            //tcCol12.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol12);

            tbl.Rows.Add(th);
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }
    protected void btnReset_Click(object sender, EventArgs e)
        {
        Response.Redirect("PuraskarCandReportFin.aspx", false);
        //divReportData.Visible = false;
        //lblheading.Visible = false;
        //lblError.Text = "";
        //lblError.Visible = false;
        //divReportData.Visible = false;

        //ddlcoursecategory.Items.Clear();
        //ddlExamCycle.Items.Insert(1, "Information Technology");
        ////FillCategories();
        //ddlcourse.Items.Clear();
        ////FillCourses();
        //ddlExamCycle.Items.Clear();
        //ddlExamCycle.Items.Insert(0, "Select All");
        //ddlSessionExam.Items.Clear();
        //ddlSessionExam.Items.Insert(1, "Half Yearly");
        }

    protected void ibExport_Click(object sender, ImageClickEventArgs e)
        {
        if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
            {
            ShowAlert("Select Course Category");
            return;
            }

        if (ddlcourse.SelectedValue.ToString().Equals("0"))
            {
            ShowAlert("Select Course");
            return;
            }

        if (ddlExamCycle.SelectedValue.ToString().Equals("0"))
            {
            ShowAlert("Select Exam Cycle");
            return;
            }
        if (ddlSessionExam.SelectedValue.ToString().Equals("0"))
            {
            ShowAlert("Select Exam Session");
            return;
            }

        try
            {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;

            divReportData.Visible = true;
            //getData();
            ShowData();
            divReportData.Controls.Add(tbl);
            Response.Clear();

            Response.AddHeader("content-disposition", "attachment;filename=StudentRegdReport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            htmlWrite = new Html32TextWriter(StringWrite);
            htmlWrite.AddAttribute("border", "2");
            divReportData.RenderControl(htmlWrite);
            Response.Write(StringWrite.ToString());
            Response.End();
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
            if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
                {
                ShowAlert("Select Course Category");
                return;
                }

            if (ddlcourse.SelectedValue.ToString().Equals("0"))
                {
                ShowAlert("Select Course");
                return;
                }

            if (ddlExamCycle.SelectedValue.ToString().Equals("0"))
                {
                ShowAlert("Select Exam Cycle");
                return;
                }
            if (ddlSessionExam.SelectedValue.ToString().Equals("0"))
                {
                ShowAlert("Select Exam Session");
                return;
                }
            
            //Int64 gtot = 0;
            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            SqlCommand scCommand = new SqlCommand("getPuraskarCandReportFin", new SqlConnection(con.ConnectionString));
            scCommand.CommandType = CommandType.StoredProcedure;
            //scCommand.Parameters.Add("@View", SqlDbType.Int).Value = 1;
            scCommand.Parameters.Add("@ExamName", SqlDbType.VarChar).Value = Convert.ToString(ddlSessionExam.SelectedValue);

            //scCommand.Parameters.Add(new SqlParameter("@Regno", SqlDbType.BigInt));
            //scCommand.Parameters.Add(new SqlParameter("@Lvlcode", SqlDbType.VarChar));

            scCommand.CommandTimeout = 50000;
            if (scCommand.Connection.State == ConnectionState.Closed)
                {
                scCommand.Connection.Open();
                }

            SqlDataAdapter da = new SqlDataAdapter(scCommand);
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
                    tdRow1.Width = Unit.Percentage(3);
                    tdRow1.Text = ds.Tables[0].Rows[i]["Level"].ToString();
                    tdRow1.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow1);

                    TableCell tdRow2 = new TableCell();
                    tdRow2.Width = Unit.Percentage(9);
                    tdRow2.Text = ds.Tables[0].Rows[i]["RefNumber"].ToString();
                    tdRow2.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow2);

                    TableCell tdRow3 = new TableCell();
                    tdRow3.Width = Unit.Percentage(6);
                    tdRow3.Text = ds.Tables[0].Rows[i]["RegnNo"].ToString();
                    tdRow3.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow3);

                    TableCell tdRow4 = new TableCell();
                    tdRow4.Width = Unit.Percentage(14);
                    tdRow4.Text = ds.Tables[0].Rows[i]["Name"].ToString();
                    tdRow4.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow4);

                    TableCell tdRow5 = new TableCell();
                    tdRow5.Width = Unit.Percentage(14);
                    tdRow5.Text = ds.Tables[0].Rows[i]["FatherName"].ToString();
                    tdRow5.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow5);

                    TableCell tdRow6 = new TableCell();
                    tdRow6.Width = Unit.Percentage(5);
                    tdRow6.Text = ds.Tables[0].Rows[i]["Gender"].ToString();  //Convert.ToDateTime(ds.Tables[0].Rows[i]["Dob"]).ToString("dd/MMM/yyyy");
                    tdRow6.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow6);

                    TableCell tdRow7 = new TableCell();
                    tdRow7.Width = Unit.Percentage(14);
                    tdRow7.Text = ds.Tables[0].Rows[i]["CastCategoryID"].ToString();
                    tdRow7.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow7);

                    TableCell tdRow8 = new TableCell();
                    tdRow8.Width = Unit.Percentage(3);
                    tdRow8.Text = ds.Tables[0].Rows[i]["PWD"].ToString();
                    tdRow8.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow8);

                    TableCell tdRow9 = new TableCell();
                    tdRow9.Width = Unit.Percentage(30);
                    tdRow9.Text = ds.Tables[0].Rows[i]["ModuleName"].ToString();
                    tdRow9.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow9);

                    TableCell tdRow10 = new TableCell();
                    tdRow10.Width = Unit.Percentage(8);
                    tdRow10.Text = ds.Tables[0].Rows[i]["Amount"].ToString();
                    tdRow10.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow10);

                    TableCell tdRow11 = new TableCell();
                    tdRow11.Width = Unit.Percentage(25);
                    tdRow11.Text = ds.Tables[0].Rows[i]["AppStatus"].ToString();
                    tdRow11.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow11);

                    tbl.Rows.Add(tr);
                    }

                lblError.Visible = false;
                lblheading.Visible = false;
                }
            else
                {
                lblError.Visible = true;
                lblError.Text = "No Record Found !";
                lblheading.Visible = false;
                throw new Exception("No Record Found !");
                //throw new Exception("No record found.");
                }
            }
        catch (Exception ex)
            {
            throw ex;
            //ShowAlert(ex.Message);
            }
        }

    public override void VerifyRenderingInServerForm(Control control)
        {
        /* Verifies that the control is rendered */
        }
    #endregion

    }