using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using System.Globalization;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
//using iTextSharp.tool.xml;
using EConnect.URM;

public partial class HO_CHMT_Project_Report : BasePage
{

    string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    Table tbl = new Table();
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 loginUserNo = 0;
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {

        lblError.Visible = false;
        lblError.Text = "";
        if (IsSessionAlive() == false)
            Response.Redirect("../Index.aspx");
        currentRoleId = Convert.ToInt32(Session["RoleID"]);
        loginUserNo = Convert.ToInt32(Session["UserID"]);
        if (!UserManager.HasRight(currentRoleId, enmRight.View))
        {
            Response.Write("Sorry! You don't have rights  to view this page");
            Response.End();
        }
        else
        {
            if (!IsPostBack)
            {
                BindInstitute();
            }
        }
    
    }

    protected void BindInstitute()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                var instituteList = from p in context.CourseProjectApplications
                                    join i in context.Institutes on p.InstituteID equals i.ID
                                    where p.ApplicationStatusID == 10  && p.CourseID ==  1213 
                                    orderby (i.Name)
                                    select new { ValueField = i.ID, TextField = i.Name };
                instituteList = instituteList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlInstitute, instituteList, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataTable GetReport()
    {
        DataTable dt = new DataTable();
        Int64 instituteId = Convert.ToInt32(ddlInstitute.SelectedValue);
        DateTime fromDate =   Convert.ToDateTime(txtFromDate.Text);
        DateTime toDate =  Convert.ToDateTime( txtToDate.Text);
               
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("CHMT_ProjectSubmission_Rep", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@instituteId", instituteId);
                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);

                con.Open();

                using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                {
                    adpt.Fill(dt);
                }
            }
        }

        return dt;
    }

    protected void ShowData()
    {
        try
        {
            using (DataTable dt1 = GetReport())
                {
                    if (dt1.Rows.Count > 0)
                    {
                        var application = (from a in dt1.AsEnumerable()
                                           select new
                                           {
                                               Registration_Number = a.Field<Int64?>("RegistrationNo") ?? 0,
                                               Candidate_Name = a.Field<string>("CandidateName") ?? "",
                                               Father_Name = a.Field<string>("FatherName") ?? "",
                                               Project_Receipt_Date = a.Field<string>("ProjectReceiptDate"),
                                               Project_Title = a.Field<string>("ProjectTitle"), 
                                           }).ToList();

                        if (application.Count() > 0)
                        {

                            ShowTableHeader();

                            int i = 0;

                            //  if (dt1.Rows.Count > 0)
                            // {
                            foreach (var app in application)
                            {
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;

                                TableHeaderCell thc = new TableHeaderCell();
                                thc.Width = Unit.Percentage(1);
                                thc.HorizontalAlign = HorizontalAlign.Center;
                                thc.Text = i.ToString();
                                tr.Cells.Add(thc);

                                TableCell thc1 = new TableCell();
                                thc1.Width = Unit.Percentage(3);
                                thc1.HorizontalAlign = HorizontalAlign.Center;
                                thc1.Text = app.Registration_Number.ToString();
                                tr.Cells.Add(thc1);

                                TableCell thc2 = new TableCell();
                                thc2.Width = Unit.Percentage(3);
                                thc2.HorizontalAlign = HorizontalAlign.Center;
                                thc2.Text = app.Candidate_Name.ToString();
                                tr.Cells.Add(thc2);

                                TableCell thc3 = new TableCell();
                                thc3.Width = Unit.Percentage(3);
                                thc3.HorizontalAlign = HorizontalAlign.Center;
                                thc3.Text = app.Father_Name.ToString();
                                tr.Cells.Add(thc3);

                                TableCell thc4 = new TableCell();
                                thc4.Width = Unit.Percentage(3);
                                thc4.HorizontalAlign = HorizontalAlign.Center;
                                thc4.Text = app.Project_Receipt_Date.ToString();
                                tr.Cells.Add(thc4);


                                TableCell thc5 = new TableCell();
                                thc5.Width = Unit.Percentage(3);
                                thc5.HorizontalAlign = HorizontalAlign.Center;
                                thc5.Text = app.Project_Title.ToString();
                                tr.Cells.Add(thc5);
                             

                                tbl.Rows.Add(tr);
                                divReportData.Controls.Add(tbl);
                               // lblError.Visible = false;
                            }
                        }
                        //else
                        //{
                        //    lblError.Visible = true;
                        //    lblError.Text = "No Record Found";
                        //}
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }
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

            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);


            TableHeaderRow th1 = new TableHeaderRow();
            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.ColumnSpan = 11;
            tcCol11.Width = Unit.Percentage(18);
            tcCol11.Text = "CHMT O Level Verified Candidates Project Report";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tcCol11);
            tbl.Rows.Add(th1);


            //TableHeaderRow t1 = new TableHeaderRow();
            //TableHeaderCell tcCol1 = new TableHeaderCell();
            //tcCol1.ColumnSpan = 11;
            //tcCol1.Width = Unit.Percentage(18);
            //tcCol1.Text = "Exam Session: " ;
            //tcCol1.HorizontalAlign = HorizontalAlign.Center;
            //t1.Cells.Add(tcCol1);
            //tbl.Rows.Add(t1);

            TableHeaderRow t2 = new TableHeaderRow();
            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.ColumnSpan = 11;
            tcCol2.Width = Unit.Percentage(18);
            tcCol2.Text = "Report Date: " + DateTime.Now.ToString("dd-MM-yyyy");
            tcCol2.HorizontalAlign = HorizontalAlign.Right;
            t2.Cells.Add(tcCol2);
            tbl.Rows.Add(t2);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell thc = new TableHeaderCell();
            thc.Width = Unit.Pixel(10);
            thc.HorizontalAlign = HorizontalAlign.Center;
            thc.Text = "SrNo.";
            th.Cells.Add(thc);

            TableHeaderCell thc1 = new TableHeaderCell();
            thc1.Width = Unit.Percentage(3);
            thc1.HorizontalAlign = HorizontalAlign.Center;
            thc1.Text = "Registration No";
            th.Cells.Add(thc1);

            TableHeaderCell thc2 = new TableHeaderCell();
            thc2.Width = Unit.Percentage(3);
            thc2.HorizontalAlign = HorizontalAlign.Center;
            thc2.Text = "Candidate Name";
            th.Cells.Add(thc2);

            TableHeaderCell thc3 = new TableHeaderCell();
            thc3.Width = Unit.Percentage(3);
            thc3.HorizontalAlign = HorizontalAlign.Center;
            thc3.Text = "Father Name";
            th.Cells.Add(thc3);

            TableHeaderCell thc4 = new TableHeaderCell();
            thc4.Width = Unit.Percentage(3);
            thc4.HorizontalAlign = HorizontalAlign.Center;
            thc4.Text = "Project Receipt Date";
            th.Cells.Add(thc4);

            TableHeaderCell thc5 = new TableHeaderCell();
            thc5.Width = Unit.Percentage(3);
            thc5.HorizontalAlign = HorizontalAlign.Center;
            thc5.Text = "Project Title";
            th.Cells.Add(thc5);
          
            tbl.Rows.Add(th);
            divReportData.Controls.Add(tbl);
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void GenerateReport()
    {
        try
        {
            Document doc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);

            int instituteID = Convert.ToInt32(ddlInstitute.SelectedValue);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);
                doc.Open();

                if (instituteID != 0)
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        //Paragraph p5 = new Paragraph("* This is electronically generated Marksheet, hence does not require signature. ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7));
                        //p5.Alignment = Element.ALIGN_CENTER;
                        //doc.Add(p5);

                        Paragraph sp = new Paragraph("\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                        sp.Alignment = Element.ALIGN_CENTER;
                        doc.Add(sp);

                        string imageURL = Server.MapPath("~/images/NielitLogoforPdf.jpg");
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageURL);
                        img.ScaleToFit(140f, 120f);
                        img.SpacingBefore = 100f;
                        img.Alignment = Element.ALIGN_CENTER;
                        doc.Add(img);

                        Paragraph para = new Paragraph("NATIONAL INSTITUTE OF ELECTRONICS AND INFORMATION TECHNOLOGY", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13, iTextSharp.text.Font.BOLD));
                        para.Alignment = Element.ALIGN_CENTER;
                        doc.Add(para);

                        Paragraph para1 = new Paragraph("(NIELIT Bhawan, Plot No 3, PSP Pocket, Institutional Area, Sector 8 Dwarka, South West Delhi, Delhi 110077) \n Website: nielit.gov.in", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLDITALIC));
                        para1.Alignment = Element.ALIGN_CENTER;
                        doc.Add(para1);

                        Paragraph para2 = new Paragraph("CHMT HQ Verified Candidates Project Report", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13, iTextSharp.text.Font.BOLD));
                        para2.Alignment = Element.ALIGN_CENTER;
                        doc.Add(para2);

                        Paragraph p = new Paragraph("Institute: " + ddlInstitute.SelectedItem.Text + "                                                                                                  Report Date: " + DateTime.Now.ToString("dd-MM-yyyy"), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD));
                      //  p.Alignment = Element.ALIGN_CENTER;                      
                        doc.Add(p);
                 
                        doc.Add(Chunk.NEWLINE);

                        PdfPTable table = new PdfPTable(6);
                        table.WidthPercentage = 100;
                        table.SetWidths(new float[] { 0.3f,1, 1, 1, 1 ,1});

                        table.AddCell("Sr No");
                        table.AddCell("Registration No");
                        table.AddCell("Candidate Name");
                        table.AddCell("Father Name");
                        table.AddCell("Project Receipt Date");
                        table.AddCell("Project Title");

                        DataTable dt2 = GetReport();
                        if (dt2.Rows.Count > 0)
                        {
                            int srNo = 1;
                            foreach (DataRow mod_row in dt2.Rows)
                            {
                                table.AddCell(new Phrase(new Chunk(srNo.ToString(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                srNo++;

                                table.AddCell(new Phrase(new Chunk(mod_row["RegistrationNo"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                table.AddCell(new Phrase(new Chunk(mod_row["CandidateName"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                table.AddCell(new Phrase(new Chunk(mod_row["FatherName"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                table.AddCell(new Phrase(new Chunk(mod_row["ProjectReceiptDate"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                table.AddCell(new Phrase(new Chunk(mod_row["ProjectTitle"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            }
                        }

                        doc.Add(table);
                        doc.Add(Chunk.NEWLINE);
                    }
                }

                doc.Close();
                writer.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();

                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=CHMTHQVerifiedCandidateProjectReport.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            ShowData();
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
            GenerateReport();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    
}