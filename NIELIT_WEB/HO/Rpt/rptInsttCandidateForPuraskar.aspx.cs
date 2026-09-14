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
using System.Data .SqlClient ;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web;
using System.Configuration;

public partial class rptInsttCandidateForPuraskar : BasePage
{
    Table tbl = new Table();
    
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;   

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                FillCategories();
                FillCourses();
                FillExamCycle();
                FillSessionExam();               
                //FillVerificationStatus();
                tbl.CssClass = "sample3";
                tbl.BorderStyle  = BorderStyle .Solid ;                
                
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(100);
                //ShowData();
              //  divReportData.Controls.Add(tbl);
                
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowTableHeader()
    {
        if (ddlcourse.SelectedValue.ToString().Equals("0"))
            return;

        if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
            return;

        if (ddlExamCycle.SelectedValue.ToString().Equals("0"))
            return;

        if (ddlSessionExam.SelectedValue.ToString().Equals("0"))
            return;
        try
        {
            TableHeaderRow th1 = new TableHeaderRow();
            th1.CssClass = "head1";

            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);
        
            
            tc1.ColumnSpan = 14;
            tc1.Text = "Report of Students Applied for Online Protsahan Puraskar in " + ddlcourse.SelectedItem.Text + " Course for " + ddlSessionExam.SelectedItem.Text + " Session";
               
            tc1.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tc1);

            tbl.Rows.Add(th1);
            
            TableHeaderRow th2 = new TableHeaderRow();
            th2.CssClass = "head1";

            TableHeaderCell tc2 = new TableHeaderCell();
            tc2.Width = Unit.Percentage(100);
                     
            tc2.ColumnSpan = 14;
            tc2.Text = "Report Generated on: " + System .DateTime .Now.ToLongDateString ();
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

        
            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(4);
            tcCol2.Text = "Exam Cycle";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

           
            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(5);
            tcCol3.Text = "Registration No.";
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(25);
            tcCol4.Text = "Name";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(25);
            tcCol5.Text = "Parent / Guardian Name";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(5);
            tcCol6.Text = "Date of Birth";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(5);
            tcCol7.Text = "Gender";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);

            TableHeaderCell tcCol8 = new TableHeaderCell();
            tcCol8.Width = Unit.Percentage(5);
            tcCol8.Text = "Caste";
            tcCol8.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol8);

            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(5);
            tcCol9.Text = "Physically Handicapped";
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol9);

            TableHeaderCell tcCol10 = new TableHeaderCell();
            tcCol10.Width = Unit.Percentage(2);
            tcCol10.Text = "Modules Appeared";
            tcCol10.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol10);

            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.Width = Unit.Percentage(2);
            tcCol11.Text = "Modules Passed";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol11);

            TableHeaderCell tcCol12 = new TableHeaderCell();
            tcCol12.Width = Unit.Percentage(5);
            tcCol12.Text = "Application Verification Status";
            tcCol12.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol12);


            TableHeaderCell tcCol13 = new TableHeaderCell();
            tcCol13.Width = Unit.Percentage(5);
            tcCol13.Text = "Rejection Reason(if rejected)";
            tcCol13.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol13);

            TableHeaderCell tcCol14 = new TableHeaderCell();
            tcCol14.Width = Unit.Percentage(5);
            tcCol14.Text = "Current Application Status";
            tcCol14.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol14);


            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowData()
    {       
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
       
        try
        {
            //ShowTableHeader();
            if (ddlcourse.SelectedValue.ToString().Equals("0"))
            {
                ShowAlert("Select Course");
                return;
            }
            if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
            {
                ShowAlert("Select Course Type");
                return;
            }
            if (ddlExamCycle.SelectedValue.ToString().Equals("0"))
            {
                ShowAlert("Select Exam Cycle");
                return;
            }
            if (ddlSessionExam.SelectedValue.ToString().Equals("0"))
                return;

            SqlCommand scCommand = new SqlCommand("getInstCandForPuraskar", new SqlConnection(con.ConnectionString));         
            scCommand.CommandType = CommandType.StoredProcedure;
            scCommand.Parameters.Add("@pExamID", SqlDbType.Int).Value = Convert.ToInt32(ddlSessionExam.SelectedValue);
            scCommand.Parameters.Add("@pInstID", SqlDbType.Int).Value = 1337;// Convert.ToInt64(Session["EntityID"]);
            scCommand.Parameters.Add("@pVerificationStatus", SqlDbType.Int).Value = Convert.ToInt32(ddlVerificationStatus.SelectedValue );
          
            scCommand.CommandTimeout = 50000;

            if (scCommand.Connection.State == ConnectionState.Closed)
            {
                scCommand.Connection.Open();
            }

            SqlDataAdapter da = new SqlDataAdapter(scCommand);
            DataSet ds = new DataSet();

            da.Fill(ds);

            if (ds.Tables.Count > 0)
            {
                lblError.Visible = false;
                ShowTableHeader();
                if (ds.Tables[0].Rows.Count > 0)
                {
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


                        TableCell tdRow2 = new TableCell();
                        tdRow2.Width = Unit.Percentage(4);
                        tdRow2.Text = ds.Tables[0].Rows[i]["ExamCycle"].ToString();
                        tdRow2.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow2);


                        TableCell tdRow3 = new TableCell();
                        tdRow3.Width = Unit.Percentage(5);
                        tdRow3.Text = ds.Tables[0].Rows[i]["RegnNo"].ToString();
                        tdRow3.HorizontalAlign = HorizontalAlign.Center;

                        tr.Cells.Add(tdRow3);


                        TableCell tdRow4 = new TableCell();
                        tdRow4.Width = Unit.Percentage(25);
                        tdRow4.Text = ds.Tables[0].Rows[i]["CName"].ToString();
                        tdRow4.HorizontalAlign = HorizontalAlign.Center;

                        tr.Cells.Add(tdRow4);

                        TableCell tdRow5 = new TableCell();
                        tdRow5.Width = Unit.Percentage(25);
                        tdRow5.Text = ds.Tables[0].Rows[i]["ParentGuardianName"].ToString();
                        tdRow5.HorizontalAlign = HorizontalAlign.Center;

                        tr.Cells.Add(tdRow5);

                        TableCell tdRow6 = new TableCell();
                        tdRow6.Width = Unit.Percentage(10);
                        tdRow6.Text = Convert.ToDateTime(ds.Tables[0].Rows[i]["DateOfBirth"].ToString()).ToString("dd-MMM-yyyy");
                        tdRow6.HorizontalAlign = HorizontalAlign.Center;

                        tr.Cells.Add(tdRow6);

                        TableCell tdRow7 = new TableCell();
                        tdRow7.Width = Unit.Percentage(5);
                        tdRow7.Text = ds.Tables[0].Rows[i]["Gender"].ToString();
                        tdRow7.HorizontalAlign = HorizontalAlign.Center;

                        tr.Cells.Add(tdRow7);

                        TableCell tdRow8 = new TableCell();
                        tdRow8.Width = Unit.Percentage(5);
                        tdRow8.Text = ds.Tables[0].Rows[i]["Caste"].ToString();
                        tdRow8.HorizontalAlign = HorizontalAlign.Center;

                        tr.Cells.Add(tdRow8);

                        TableCell tdRow9 = new TableCell();
                        tdRow9.Width = Unit.Percentage(5);
                        tdRow9.Text = ds.Tables[0].Rows[i]["isHandicapped"].ToString();
                        tdRow9.HorizontalAlign = HorizontalAlign.Center;

                        tr.Cells.Add(tdRow9);

                        TableCell tdRow10 = new TableCell();
                        tdRow10.Width = Unit.Percentage(20);
                        tdRow10.Text = ds.Tables[0].Rows[i]["ExamAppeared"].ToString();
                        tdRow10.Text = tdRow10.Text.Replace(", ", ",").Replace(",", ", ");
                        tdRow10.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow10);

                        TableCell tdRow11 = new TableCell();
                        tdRow11.Width = Unit.Percentage(20);
                        tdRow11.Text = ds.Tables[0].Rows[i]["ExamPassed"].ToString();
                        tdRow11.Text = tdRow11.Text.Replace(", ", ",").Replace(",", ", ");
                        tdRow11.HorizontalAlign = HorizontalAlign.Center;

                        tr.Cells.Add(tdRow11);

                        TableCell tdRow12 = new TableCell();
                        tdRow12.Width = Unit.Percentage(5);
                        tdRow12.Text = ds.Tables[0].Rows[i]["VerificationStatus"].ToString();
                        tdRow12.HorizontalAlign = HorizontalAlign.Center;

                        tr.Cells.Add(tdRow12);


                        TableCell tdRow13 = new TableCell();
                        tdRow13.Width = Unit.Percentage(5);
                        tdRow13.Text = ds.Tables[0].Rows[i]["rejectionReason"].ToString();
                        tdRow13.HorizontalAlign = HorizontalAlign.Center;

                        tr.Cells.Add(tdRow13);


                        TableCell tdRow14 = new TableCell();
                        tdRow14.Width = Unit.Percentage(5);
                        tdRow14.Text = ds.Tables[0].Rows[i]["ApplicationStatus"].ToString();
                        tdRow14.HorizontalAlign = HorizontalAlign.Center;

                        tr.Cells.Add(tdRow14);

                        tbl.Rows.Add(tr);
                    }

                }
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "No Record Found.";
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }                      
   }
       
     protected void btnGenerate_Click(object sender, EventArgs e)
    {
   
        tbl.CssClass = "sample3";
        tbl.CellPadding = 2;
        tbl.CellSpacing = 1;
        tbl.Width = Unit.Percentage(100);      
        ShowData();
        divReportData.Controls.Add(tbl);
        divReportData.Visible = true;
    }

   
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        if (ddlcourse.SelectedValue.ToString().Equals("0"))
            return;
        if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
            return;
        if (ddlExamCycle.SelectedValue.ToString().Equals("0"))
            return;
        if (ddlSessionExam.SelectedValue.ToString().Equals("0"))
            return;              

        try
        {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;           
                divReportData.Visible = true;                
                ShowData();
                divReportData.Controls.Add(tbl);
                Response.Clear();
            
            Response.AddHeader("content-disposition", "attachment;filename=PuraskarAppStudentReport.xls");
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
    protected void imgPDF_Click(object sender, ImageClickEventArgs e)
    {
        if (ddlcourse .SelectedValue.ToString ().Equals ("0"))
            return;
        if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
            return;
        if (ddlExamCycle.SelectedValue.ToString().Equals("0"))
            return;
        if (ddlSessionExam.SelectedValue.ToString().Equals("0"))
            return;
      
          try
        {           
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            hw.AddAttribute("border", "2");
              hw.RenderBeginTag(HtmlTextWriterTag.Font );
              hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize , "10");

            divReportData.Visible = true;
            tbl.BorderStyle = BorderStyle .Solid ;
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
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=PuraskarAppStudentReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);            
            Response.Write(pdfDoc);
            Response.End();
            
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillExamName()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                Int32 ExamYear = Convert.ToInt32(ddlSessionExam .SelectedValue);
                Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
                var ExamName = (from p in context.CourseExamApplications
                                join c in context.Exams on
                                p.ExamID equals c.ID
                                where p.Exam.ExamYear == ExamYear && p.CourseID == CourseID && c.DateOfPublishingOfResult != null && c.ExaminationCycleID == ExamCycleID
                                orderby (c.Name) ascending
                                select new { ValueField = p.Exam.ID, TextField = p.Exam.Name }).Distinct().ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSessionExam, ExamName, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillExamYear()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
                Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                Int32 CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                Int32 currentyear = Convert.ToInt32(DateTime.Now.Year);
                var examYear = (from p in context.Exams
                                join q in context.CourseExamApplications
                                on p.ID equals q.ExamID
                                where (p.CourseID == CourseID && p.CourseCategoryID == CourseCategoryID && p.ExaminationCycleID == ExamCycleID && p.ExamYear >= currentyear && p.DateOfPublishingOfResult == null)
                                orderby (p.ExamYear) descending
                                select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSessionExam, examYear, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillExamCycle()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                int CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                var ExamCycleList = from p in context.ExaminationCycles
                                    where p.CourseID == CourseID
                                    select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, ExamCycleList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillCategories()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationCourse);
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");

                var Category = (from s in context.CourseCategories
                                join c in context.Courses on s.ID equals c.CourseCategoryID
                                where c.CourseTypeID == CourseType && s.ID == 1
                                orderby (s.Name)
                                select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillCourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationExam);
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                int id = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == id
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourse, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillSessionExam()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
                Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                Int32 CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                Int32 currentyear = Convert.ToInt32(DateTime.Now.Year);
                var examYear = (from p in context.Exams
                                where (p.CourseID == CourseID && p.CourseCategoryID == CourseCategoryID && p.ExaminationCycleID == ExamCycleID)
                                orderby (p.ExamYear) descending
                                select new { ValueField = p.ID, TextField = p.Name });

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSessionExam, examYear, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }  

    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcourse.Items.Clear();
        ddlExamCycle.Items.Clear();
        ddlSessionExam.Items.Clear(); 
        ddlExamCycle.Items.Insert(0, "--Select One--");
       
        FillCourses();
        FillExamCycle();
        FillSessionExam();       
        divReportData.Visible = false;      
    }
    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExamCycle.Items.Clear();
        ddlSessionExam.Items.Clear();
        FillExamCycle();
        FillSessionExam();     
        divReportData.Visible = false;
    }
        
    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
       // ddlExamYear.Items.Clear();
      ddlSessionExam.Items.Clear();
      ddlSessionExam.Items.Insert(0, "--Select One--");   
        FillSessionExam();     
        divReportData.Visible = false;
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void ddlSessionExam_SelectedIndexChanged(object sender, EventArgs e)
    {     
        divReportData.Visible = false;
    }
}