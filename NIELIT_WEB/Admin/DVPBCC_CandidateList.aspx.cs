using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class Admin_DVPBCC_CandidateList : BasePage
{
    DataSet ds = new DataSet();
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Table tbl = new Table();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsSessionAlive() == false)
            Response.Redirect("../Index.aspx");
        currentRoleId = Convert.ToInt32(Session["RoleID"]);
        loginUserNo = Convert.ToInt32(Session["UserID"]);
        
        if (!UserManager.HasRight(currentRoleId, enmRight.View))
        {
            Response.Write("Sorry! You don't have rights  to view this page");
            Response.End();
        }
        loginUserType = (UserType)Session["UserType"];
        entityID = Convert.ToInt64(Session["EntityID"]);

        if (!IsPostBack)
        {
            BindCourse();
            BindInstitute();
        }
        //  }
    }

    protected void BindCourse()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("-- Select One --", "0");

                var examName = (from a in context.Exams
                                where (a.ExamMonth == DateTime.Now.Month && a.ExamYear == DateTime.Now.Year && a.CourseID ==175)
                                select new { ValueField = a.ID, TextField = a.Name });
                examName = examName.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, examName, lst);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void BindInstitute()
    {
        try
        {

            using (var context = new EConnectContext())
            {
                string field, val;
                if (loginUserType != UserType.Institute)
                {
                    field = "-- Select All --";
                    val = "0";
                    System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem(field, val);

                    var institute = (from c in context.Institutes
                                     join a in context.AccreditationDetails on c.ID equals a.InstituteID
                                     where a.CourseID ==175 && a.AccreditationStatusID !=5
                                    // where c.ID == entityID
                                     select new { ValueField = c.ID, TextField = c.Name });
                    institute = institute.Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlInstitute, institute, lst);
                }
                else
                {
                    field = "-- Select  --";
                    val = "-9";
                    System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem(field, val);

                    var institute = (from c in context.Institutes
                                     where c.ID == entityID
                                     select new { ValueField = c.ID, TextField = c.Name });
                    institute = institute.Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlInstitute, institute, lst);
                }

               
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }


    protected void btnShow_Click(object sender, EventArgs e)
    {
        GetExamData(Convert.ToInt32(ddlExamName.SelectedValue), Convert.ToInt32(ddlInstitute.SelectedValue));
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvMain.DataSource = ds;
            gvMain.DataBind();
            gvMain.Visible = true;
        }
        else
        {
            ShowAlert("No data available for selected criteria.");
            gvMain.Visible = false;
        }
    }
    
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        GetExamData(Convert.ToInt32(ddlExamName.SelectedValue), Convert.ToInt32(ddlInstitute.SelectedValue));
        if (ds.Tables[0].Rows.Count > 0)
        {
            downloadReport();
        }
        else
            ShowAlert("No data available for selected criteria.");
    }

    public DataSet GetExamData(Int32 examId, Int32 instituteId)
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("get_DVPBCC_CandidateList", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@instituteId", SqlDbType.Int));
                cmd.Parameters["@instituteId"].Value = instituteId;

                cmd.Parameters.Add(new SqlParameter("@examId", SqlDbType.Int));
                cmd.Parameters["@examId"].Value = examId;

                
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(ds); 
                }
            }
        }
        return ds;
    }

    void downloadReport()
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

            ShowData(ds.Tables[0]);
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
            Response.AddHeader("content-disposition", "attachment;filename=DVPBCC_CandidateList.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void ShowTableHeader(DataTable tab)
    {
        try
        { 
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(5);
            tcCol.Text = "SrNo.";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(10);
            tcCol1.Text = "Accreditation Number";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);


            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(10);
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            tcCol2.Text = "Exam Centre Name";
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(20);
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Text = "Exam Centre Address";
            th.Cells.Add(tcCol3);


            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(10);
            tcCol4.Text = "Roll Number";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(10);
            tcCol5.Text = "Candidate Name";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);


            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(10);
            tcCol6.Text = "Father Name";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);


            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(10);
            tcCol7.Text = "Date of Birth";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);
            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }

    protected void ShowData(DataTable tab)
    {
        try
        { 
            StringBuilder mySql = new StringBuilder(); 
            
                if (tab.Rows.Count > 0)
                {
                    ShowTableHeader(tab);                    
                    
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        TableRow tr = new TableRow();
                        //  int i = 1;
                        if (i % 2 == 0)
                            tr.CssClass = "gdalternate1";
                        else
                            tr.CssClass = "gdrow1";
                        ds.Tables[0].NewRow();
                        TableCell tdRow = new TableCell();
                        tdRow.Width = Unit.Percentage(5);
                        tdRow.Text = (i + 1).ToString();
                        tdRow.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow);


                        TableCell tdRow1 = new TableCell();
                        tdRow1.Width = Unit.Percentage(10);
                        tdRow1.Text = ds.Tables[0].Rows[i]["Accreditation_Number"].ToString();
                        tdRow1.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow1);

                        TableCell tdRow2 = new TableCell();
                        tdRow2.Width = Unit.Percentage(10);
                        tdRow2.Text = ds.Tables[0].Rows[i]["Exam_Centre_Name"].ToString();
                        tdRow2.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow2);

                        TableCell tdRow3 = new TableCell();
                        tdRow3.Width = Unit.Percentage(20);
                        tdRow3.Text = ds.Tables[0].Rows[i]["Exam_Centre_Address"].ToString();
                        tdRow3.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow3);

                        TableCell tdRow4 = new TableCell();
                        tdRow4.Width = Unit.Percentage(10);
                        tdRow4.Text = ds.Tables[0].Rows[i]["Roll_Number"].ToString();
                        tdRow4.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow4);

                        TableCell tdRow5 = new TableCell();
                        tdRow5.Width = Unit.Percentage(5);
                        tdRow5.Text = ds.Tables[0].Rows[i]["Name"].ToString();
                        tdRow5.HorizontalAlign = HorizontalAlign.Center; 
                        tr.Cells.Add(tdRow5);

                        TableCell tdRow6 = new TableCell();
                        tdRow6.Width = Unit.Percentage(5);
                        tdRow6.Text = ds.Tables[0].Rows[i]["Father_Name"].ToString();
                        tdRow6.HorizontalAlign = HorizontalAlign.Center; 
                        tr.Cells.Add(tdRow6);

                        TableCell tdRow7 = new TableCell();
                        tdRow7.Width = Unit.Percentage(10);
                        tdRow7.Text = ds.Tables[0].Rows[i]["Dob"].ToString();
                        tdRow7.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow7);
                        
                        

                        lblExamDate.Text = "Exam Date:" + ds.Tables[0].Rows[i]["Date_of_Exam"].ToString();

                        lblExamTime.Text = "Exam Time:" + ds.Tables[0].Rows[i]["Reporting_Time"].ToString();


                    tbl.Rows.Add(tr); 
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
}



    
 