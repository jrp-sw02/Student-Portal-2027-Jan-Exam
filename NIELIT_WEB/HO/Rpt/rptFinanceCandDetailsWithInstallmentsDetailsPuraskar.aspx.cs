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
using System.Collections.Generic;
using System.Web;
using System.Transactions;
using EConnect.Utils.Common;
using System.Drawing;

public partial class rptFinanceCandDetailsWithInstallmentsDetailsPuraskar : BasePage
{
    Table tbl = new Table();
    
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 applicantTypeID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //if (!IsSessionAlive())
            //{
            //    Response.Redirect("~/index.aspx");
            //}
            //loginUserType = (UserType)Session["UserType"];
            //entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                FillCategories();
                ddlcoursecategory.SelectedValue = "1";
                FillCourses();
                FillExamCycle();
                FillSessionExam();                            
            }
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

            Int64 gtot = 0;
            
            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            SqlCommand scCommand = new SqlCommand("getFinanceInstalmentsDetailsReport", new SqlConnection(con.ConnectionString));
            scCommand.CommandType = CommandType.StoredProcedure;
            scCommand.Parameters.Add("@View", SqlDbType.Int).Value = 1;
            scCommand.Parameters.Add("@CourseID", SqlDbType.Int).Value = Convert.ToInt32(ddlcourse.SelectedValue);
            scCommand.Parameters.Add("@ExamID", SqlDbType.Int).Value = Convert.ToInt32(ddlSessionExam.SelectedValue);
            scCommand.Parameters.Add(new SqlParameter("@Regno", SqlDbType.BigInt));
            scCommand.Parameters.Add(new SqlParameter("@Lvlcode", SqlDbType.VarChar));
            scCommand.Parameters["@Lvlcode"].Value = ddlcourse.SelectedItem.Text;
            scCommand.Parameters["@Regno"].Value = 1;
            
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

                    TableCell tdRow2 = new TableCell();
                    tdRow2.Width = Unit.Percentage(4);
                    tdRow2.Text = ds.Tables[0].Rows[i]["Name"].ToString();
                    tdRow2.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow2);

                    TableCell tdRow3 = new TableCell();
                    tdRow3.Width = Unit.Percentage(5);
                    tdRow3.Text = ds.Tables[0].Rows[i]["FathersName"].ToString();
                    tdRow3.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow3);

                    TableCell tdRow16 = new TableCell();
                    tdRow16.Width = Unit.Percentage(5);
                    tdRow16.Text = ds.Tables[0].Rows[i]["RegnNo"].ToString();
                    tdRow16.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow16);                   

                    TableCell tdRow12 = new TableCell();
                    tdRow12.Width = Unit.Percentage(4);
                    tdRow12.Text = ds.Tables[0].Rows[i]["Level"].ToString();
                    tdRow12.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow12);

                    TableCell tdRow13 = new TableCell();
                    tdRow13.Width = Unit.Percentage(5);
                    tdRow13.Text = Convert.ToDateTime(ds.Tables[0].Rows[i]["Dob"]).ToString("dd/MMM/yyyy");
                    tdRow13.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow13);

                    TableCell tdRow4 = new TableCell();
                    tdRow4.Width = Unit.Percentage(20);
                    tdRow4.Text = ds.Tables[0].Rows[i]["InstallmentsNumber"].ToString();
                    tdRow4.HorizontalAlign = HorizontalAlign.Center;

                    TableCell tdRow5 = new TableCell();
                    tdRow5.Width = Unit.Percentage(15);
                    tdRow5.Text = ds.Tables[0].Rows[i]["ExamCycle"].ToString();
                    tdRow5.HorizontalAlign = HorizontalAlign.Center;

                    TableCell tdRow7 = new TableCell();
                    tdRow7.Width = Unit.Percentage(5);
                    tdRow7.Text = ds.Tables[0].Rows[i]["Amountpaid"].ToString();
                    tdRow7.HorizontalAlign = HorizontalAlign.Center;

                    TableCell tdRow18 = new TableCell();
                    tdRow18.Width = Unit.Percentage(5);
                    tdRow18.Text = ds.Tables[0].Rows[i]["AccountNo"].ToString();
                    tdRow18.HorizontalAlign = HorizontalAlign.Center;

                    TableCell tdRow17 = new TableCell();
                    tdRow17.Width = Unit.Percentage(5);
                    tdRow17.Text = ds.Tables[0].Rows[i]["BankName"].ToString();
                    tdRow17.HorizontalAlign = HorizontalAlign.Center;

                    TableCell tdRow8 = new TableCell();
                    tdRow8.Width = Unit.Percentage(5);
                    tdRow8.Text = Convert.ToDateTime(ds.Tables[0].Rows[i]["PaymentDate"]).ToString("dd/MMM/yyyy");
                    tdRow8.HorizontalAlign = HorizontalAlign.Center;

                    Int64 regno = 0, candidateid = 0;
                    regno = Convert.ToInt64(ds.Tables[0].Rows[i]["RegnNo"].ToString());
                    // candidateid = Convert.ToInt64(ds.Tables[0].Rows[i]["CandidateID"].ToString());
                    using (DataTable dt = PreviousInstallmentsRecordView(regno)) //PreviousInstallmentsRecordView(regno, candidateid))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            int j;
                            for (j = 0; j < dt.Rows.Count; j++)
                            {
                                tdRow4.Text = tdRow4.Text + " <p>" + "-----------" + " <p>" + dt.Rows[j].Field<int>("InstallmentsNumber").ToString() + "</p>" + "</p>";
                                tdRow5.Text = tdRow5.Text + " <p>" + "-----------" + " <p>" + dt.Rows[j].Field<string>("ExamCycle").ToString() + "</p>" + "</p>";
                                tdRow7.Text = tdRow7.Text + " <p>" + "------------" + " <p>" + dt.Rows[j].Field<Int64>("Amountpaid").ToString() + "</p>" + "</p>";
                                tdRow18.Text = tdRow18.Text + " <p>" + "-----------" + " <p>" + dt.Rows[j].Field<string>("AccountNo").ToString() + "</p>" + "</p>";
                                tdRow17.Text = tdRow17.Text + "<p>" + "-----------" + " <p>" + dt.Rows[j].Field<string>("BankName").ToString() + "</p>" + "</p>";
                                tdRow8.Text = tdRow8.Text + " <p>" + "-----------" + " <p>" + Convert.ToDateTime(dt.Rows[j]["PaymentDate"]).ToString("dd/MMM/yyyy") + "</p>" + "</p>";

                                //tdRow4.Text = tdRow4.Text +  " <p>" + dt.Rows[j].Field<int>("InstallmentsNumber").ToString()  + "</p>";
                                //tdRow5.Text = tdRow5.Text +  " <p>" + dt.Rows[j].Field<string>("ExamCycle").ToString()  + "</p>";
                                //tdRow7.Text = tdRow7.Text +  " <p>" + dt.Rows[j].Field<Int64>("Amountpaid").ToString()  + "</p>";
                                //tdRow18.Text = tdRow18.Text + " <p>" + dt.Rows[j].Field<string>("AccountNo").ToString() + "</p>";
                                //tdRow17.Text = tdRow17.Text  + " <p>" + dt.Rows[j].Field<string>("BankName").ToString()  + "</p>";
                                //tdRow8.Text = tdRow8.Text  + " <p>" + Convert.ToDateTime(dt.Rows[j]["PaymentDate"]).ToString("dd/MMM/yyyy")  + "</p>";
                                  
                            }
                        }
                    }
                    
                    //TableCell tdRow4 = new TableCell();
                    //tdRow4.Width = Unit.Percentage(20);
                    //tdRow4.Text = ds.Tables[0].Rows[i]["InstallmentsNumber"].ToString();
                    //tdRow4.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow4);

                    //TableCell tdRow5 = new TableCell();
                    //tdRow5.Width = Unit.Percentage(15);
                    //tdRow5.Text = ds.Tables[0].Rows[i]["ExamCycle"].ToString();
                    //tdRow5.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow5);

                    //TableCell tdRow7 = new TableCell();
                    //tdRow7.Width = Unit.Percentage(5);
                    //tdRow7.Text = ds.Tables[0].Rows[i]["Amountpaid"].ToString();
                    //tdRow7.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow7);

                    //TableCell tdRow18 = new TableCell();
                    //tdRow18.Width = Unit.Percentage(5);
                    //tdRow18.Text = ds.Tables[0].Rows[i]["AccountNo"].ToString();
                    //tdRow18.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow18);

                    //TableCell tdRow17 = new TableCell();
                    //tdRow17.Width = Unit.Percentage(5);
                    //tdRow17.Text = ds.Tables[0].Rows[i]["BankName"].ToString();
                    //tdRow17.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow17);

                   

                    //TableCell tdRow8 = new TableCell();
                    //tdRow8.Width = Unit.Percentage(5);
                    //tdRow8.Text = Convert.ToDateTime(ds.Tables[0].Rows[i]["PaymentDate"]).ToString("dd/MMM/yyyy");
                    //tdRow8.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow8);

                    tbl.Rows.Add(tr);

                }
               
                lblError.Visible = false;
                lblheading.Visible = true; 
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "No Record Found !";
                lblheading.Visible = true;               
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
                      
   }
    public DataTable PreviousInstallmentsRecordView(Int64 regno) //PreviousInstallmentsRecordView(Int64 regno, Int64 CandidateID)
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("getFinanceInstalmentsDetailsReport", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Regno", SqlDbType.BigInt));
                cmd.Parameters["@Regno"].Value = regno;
                cmd.Parameters.Add("@CourseID", SqlDbType.Int).Value = Convert.ToInt32(ddlcourse.SelectedValue);
                cmd.Parameters.Add("@ExamID", SqlDbType.Int).Value = Convert.ToInt32(ddlSessionExam.SelectedValue);
                //cmd.Parameters.Add(new SqlParameter("@CandidateID", SqlDbType.BigInt));
                //cmd.Parameters["@CandidateID"].Value = CandidateID;
                //cmd.Parameters.Add(new SqlParameter("@pExamID", SqlDbType.BigInt));
                //cmd.Parameters["@pExamID"].Value = ExamID;
                cmd.Parameters.Add(new SqlParameter("@Lvlcode", SqlDbType.VarChar));
                cmd.Parameters["@Lvlcode"].Value = ddlcourse.SelectedItem.Text;
                cmd.Parameters.Add(new SqlParameter("@View", SqlDbType.Int));
                cmd.Parameters["@View"].Value = 2;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
   protected void ShowTableHeader()
   {
       if (ddlcourse.SelectedValue.ToString().Equals("0"))
           return;

       if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
           return;

       try
       {
           TableHeaderRow th1 = new TableHeaderRow();
           th1.CssClass = "head1";

           TableHeaderCell tc1 = new TableHeaderCell();
           tc1.Width = Unit.Percentage(100);

           string examcycle = ddlSessionExam.SelectedItem.Text;

           tc1.ColumnSpan = 12;
           tc1.Text = "Report of Protsahan Puraskar Installments Details (Finance)  of " + ddlSessionExam.SelectedItem.Text + "";

           tc1.HorizontalAlign = HorizontalAlign.Center;
           th1.Cells.Add(tc1);

           tbl.Rows.Add(th1);

           TableHeaderRow th2 = new TableHeaderRow();
           th2.CssClass = "head1";

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


           TableHeaderCell tcCol2 = new TableHeaderCell();
           tcCol2.Width = Unit.Percentage(10);
           tcCol2.Text = "Name";
           tcCol2.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol2);

           TableCell tdRow113 = new TableCell();
           tdRow113.Width = Unit.Percentage(15);
           tdRow113.Text = "Registration No.";
           tdRow113.HorizontalAlign = HorizontalAlign.Center;

           TableHeaderCell tcCol6 = new TableHeaderCell();
           tcCol6.Width = Unit.Percentage(15);
           tcCol6.Text = "Father's Name";
           tcCol6.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol6);

           th.Cells.Add(tdRow113);
           TableHeaderCell tcCol3 = new TableHeaderCell();
           tcCol3.Width = Unit.Percentage(15);
           tcCol3.Text = "Level ";
           tcCol3.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol3);

           TableHeaderCell tcCol4 = new TableHeaderCell();
           tcCol4.Width = Unit.Percentage(15);
           tcCol4.Text = "DoB";
           tcCol4.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol4);

           TableHeaderCell tcCol5 = new TableHeaderCell();
           tcCol5.Width = Unit.Percentage(20);
           tcCol5.Text = "Instalment No ";
           tcCol5.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol5);

           TableHeaderCell tcCol15 = new TableHeaderCell();
           tcCol15.Width = Unit.Percentage(20);
           tcCol15.Text = "Exam Cycle  ";
           tcCol15.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol15);

           TableHeaderCell tcCol7 = new TableHeaderCell();
           tcCol7.Width = Unit.Percentage(5);
           tcCol7.Text = "Amount paid ";
           tcCol7.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol7);

           TableHeaderCell tcCol8 = new TableHeaderCell();
           tcCol8.Width = Unit.Percentage(5);
           tcCol8.Text = "Account No  ";
           tcCol8.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol8);

           TableHeaderCell tcCol9 = new TableHeaderCell();
           tcCol9.Width = Unit.Percentage(5);
           tcCol9.Text = "Bank ";
           tcCol9.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol9);

           TableHeaderCell tcCol10 = new TableHeaderCell();
           tcCol10.Width = Unit.Percentage(5);
           tcCol10.Text = "Payment Date  ";
           tcCol10.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol10);

           tbl.Rows.Add(th);
       }
       catch (Exception ex)
       {
           ShowAlert(ex.Message);
       }
   }  

   protected void btnShowReport_Click(object sender, EventArgs e)
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

        try
        {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
           
                divReportData.Visible = true;
                //getData();
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
    protected void imgPDF_Click(object sender, ImageClickEventArgs e)
    {
        if (ddlcourse .SelectedValue.ToString ().Equals ("0"))
            return;

        if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
            return;
            
          try
        {        
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            hw.AddAttribute("border", "1");
              hw.RenderBeginTag(HtmlTextWriterTag.Font );
              hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize , "9");

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
           // Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
              
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=StudentsRegdReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            
            Response.Write(pdfDoc);
            Response.End();           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
   
    protected void FillCategories()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationCourse );
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");

                var Category = (from s in context.CourseCategories
                                join c in context.Courses on s.ID equals c.CourseCategoryID
                                where c.CourseTypeID == CourseType && s.ID==1
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
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcourse.Items.Clear();
             
        FillCourses();
        divReportData.Visible = false;       
    }
    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        divReportData.Visible = false;
        lblheading.Visible = false;      
        lblError.Text = "";
        lblError.Visible = false;
        ddlExamCycle.Items.Clear();
        ddlSessionExam.Items.Clear();
        ddlSessionExam.Items.Insert(0, "--Select One--");
        FillExamCycle();
        divReportData.Visible = false;
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
    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSessionExam.Items.Clear();
        ddlSessionExam.Items.Insert(0, "--Select One--");
        FillSessionExam();

        divReportData.Visible = false;
    }
    protected void ddlSessionExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        divReportData.Visible = false;
        lblheading.Visible = false;
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }    
}