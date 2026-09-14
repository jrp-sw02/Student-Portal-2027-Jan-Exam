using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class HO_Rpt_PracticalMarksReportBatchWise : BasePage
{
    Int64 entityID = 0;
    Table tbl = new Table();
 
  
   // string CentreCode = Session["CentreCode"].ToString();
     //string CentreCode = "UTTADEH01";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Index.aspx");

            if (!IsPostBack)
            {
                tbl.CssClass = "sample3";
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(100);
                divReportData.Controls.Add(tbl);
                BindCentre();
                imgPDF.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }

    protected void BindCentre()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                //entityID = Convert.ToInt64(Session["EntityID"]);  Session["CentreCode"] 
                 string CentreCode = Session["CentreCode"].ToString();
                //string CentreCode = "UTTADEH01";
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--  Select Centre  --", "0");
                var centreList = from p in context.PracAllowedMarksEntry
                                 //&& p.ExamDate == DateTime.Now 
                                 where p.CenterCode.Replace(" ", "") == CentreCode
                                 select new { ValueField = p.CenterCode, TextField = p.CenterCode };
                centreList = centreList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentreCode, centreList, lst);
                //ddlBatch.Items.Insert(0, new ListItem("--  Select Batch  --", "0"));               
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private DataTable GetData()
    {

        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

        string sql = "onlinePracticalMarksReport";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@centreCode", SqlDbType.VarChar).Value = ddlCentreCode.SelectedValue;
                cmd.Parameters.Add("@batchCode", SqlDbType.VarChar).Value = ddlBatch.SelectedValue;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
        }

        return dt;
    }

    protected void ShowTableHeader()
    {

        try
        {
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell thc = new TableHeaderCell();
            thc.Width = Unit.Percentage(1);
            thc.HorizontalAlign = HorizontalAlign.Center;
            thc.Text = "#";
            th.Cells.Add(thc);

            TableHeaderCell thc1 = new TableHeaderCell();
            thc1.Width = Unit.Percentage(3);
            thc1.HorizontalAlign = HorizontalAlign.Center;
            thc1.Text = "Registration No.";
            th.Cells.Add(thc1);

            TableHeaderCell thc2 = new TableHeaderCell();
            thc2.Width = Unit.Percentage(3);
            thc2.HorizontalAlign = HorizontalAlign.Center;
            thc2.Text = "Candidate Name";
            th.Cells.Add(thc2);

            TableHeaderCell thc3 = new TableHeaderCell();
            thc3.Width = Unit.Percentage(3);
            thc3.HorizontalAlign = HorizontalAlign.Center;
            thc3.Text = "Module Name";
            th.Cells.Add(thc3);

            TableHeaderCell thc4 = new TableHeaderCell();
            thc4.Width = Unit.Percentage(3);
            thc4.HorizontalAlign = HorizontalAlign.Center;
            thc4.Text = "Examiner Marks (Max 40)";
            th.Cells.Add(thc4);

            TableHeaderCell thc7 = new TableHeaderCell();
            thc7.Width = Unit.Percentage(3);
            thc7.HorizontalAlign = HorizontalAlign.Center;
            thc7.Text = "Observer Marks(Max 40)";
            th.Cells.Add(thc7);

            TableHeaderCell thc8 = new TableHeaderCell();
            thc8.Width = Unit.Percentage(3);
            thc8.HorizontalAlign = HorizontalAlign.Center;
            thc8.Text = "Viva Marks(Max 20)";
            th.Cells.Add(thc8);

            TableHeaderCell thc9 = new TableHeaderCell();
            thc9.Width = Unit.Percentage(3);
            thc9.HorizontalAlign = HorizontalAlign.Center;
            thc9.Text = "Total Marks(out of 100)";
            th.Cells.Add(thc9);

            tbl.Rows.Add(th);
            divReportData.Controls.Add(tbl);
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void ShowData()
    {

        int i = 0;
        Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
        PdfWriter pdfWriter1 = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        DataTable dt1 = GetData();

        if (dt1.Rows.Count > 0)
        {
            foreach (DataRow dtrow in dt1.Rows)
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
                thc1.Text = dtrow["Registration_no"].ToString();
                tr.Cells.Add(thc1);

                TableCell thc2 = new TableCell();
                thc2.Width = Unit.Percentage(3);
                thc2.HorizontalAlign = HorizontalAlign.Center;
                thc2.Text = dtrow["Candidate_Name"].ToString();
                tr.Cells.Add(thc2);

                TableCell thc3 = new TableCell();
                thc3.Width = Unit.Percentage(3);
                thc3.HorizontalAlign = HorizontalAlign.Center;
                thc3.Text = dtrow["module_Short_Name"].ToString();
                tr.Cells.Add(thc3);

                TableCell thc4 = new TableCell();
                thc4.Width = Unit.Percentage(3);
                thc4.HorizontalAlign = HorizontalAlign.Center;
                thc4.Text = dtrow["ExaminerMarks"].ToString();
                tr.Cells.Add(thc4);

                TableCell thc6 = new TableCell();
                thc6.Width = Unit.Percentage(3);
                thc6.HorizontalAlign = HorizontalAlign.Center;
                thc6.Text = dtrow["ObserverMarks"].ToString();
                tr.Cells.Add(thc6);

                TableCell thc7 = new TableCell();
                thc7.Width = Unit.Percentage(3);
                thc7.HorizontalAlign = HorizontalAlign.Center;
                thc7.Text = dtrow["VivaMarks"].ToString();
                tr.Cells.Add(thc7);

                TableCell thc8 = new TableCell();
                thc8.Width = Unit.Percentage(3);
                thc8.HorizontalAlign = HorizontalAlign.Center;
                thc8.Text = dtrow["TotalMarks"].ToString();
                tr.Cells.Add(thc8);
                tbl.Rows.Add(tr);
                divReportData.Controls.Add(tbl);
                //i++;                
            }
        }
        else
        {
            lblError.Visible = true;
            lblError.Text = "No Record Found!";
        }
    }

    protected void imgPDF_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            int i = 1;
            string centreCode = ddlCentreCode.SelectedValue;
            string batchCode = ddlBatch.SelectedValue;
            using (var context = new EConnectContext())
            {
                var info = (from p in context.PracAllowedMarksEntry
                            where p.CenterCode == centreCode && p.Batch == batchCode
                            select new { examSession = p.ExamSession, examDate = p.ExamDate }).FirstOrDefault();

                Document doc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
                PdfWriter pdfWriter1 = PdfWriter.GetInstance(doc, Response.OutputStream);
                doc.Open();

                Paragraph para = new Paragraph(" NATIONAL INSTITUTE OF ELECTRONICS AND INFORMATION TECHNOLOGY  \n An Autonomous Scientific Society of Ministry of Electronics and Information Technology , Govt of  India \n  Details of Marks Entered ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13, iTextSharp.text.Font.BOLD));
                para.Alignment = Element.ALIGN_CENTER;
                doc.Add(para);

                doc.Add(Chunk.NEWLINE);
                doc.Add(Chunk.NEWLINE);

                Paragraph p1 = new Paragraph(" Centre Code  :  " + centreCode + " \n  Date / Time of Examination :  " + info.examDate + " / " + info.examSession + " \n  Batch : " + batchCode + " \n ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11));
                p1.Alignment = Element.ALIGN_LEFT;
                doc.Add(p1);

                PdfPTable table = new PdfPTable(8);
                table.WidthPercentage = 100;
                float[] width = { 0.2F, 0.7F, 0.8F, 0.7F, 0.8F, 0.8F, 0.8F, 0.8F };
                table.SetWidths(width);
                //table.SetWidths(new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1 });
                table.AddCell("#");
                table.AddCell("Registration No.");
                table.AddCell("Candidate Name");
                table.AddCell("Module Name");
                table.AddCell("Examiner Marks (Max 40)");
                table.AddCell("Observer Marks(Max 40)");
                table.AddCell("Viva Marks(Max 20)");
                table.AddCell("Total Marks(out of 100)");

                DataTable dt2 = GetData();
                if (dt2.Rows.Count > 0)
                {
                    foreach (DataRow mod_row in dt2.Rows)
                    {
                        table.AddCell(new Phrase(new Chunk((i++).ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                        table.AddCell(new Phrase(new Chunk(mod_row["REGISTRATION_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                        table.AddCell(new Phrase(new Chunk(mod_row["Candidate_Name"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                        table.AddCell(new Phrase(new Chunk(mod_row["module_Short_Name"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                        table.AddCell(new Phrase(new Chunk(mod_row["ExaminerMarks"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                        table.AddCell(new Phrase(new Chunk(mod_row["ObserverMarks"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                        table.AddCell(new Phrase(new Chunk(mod_row["VivaMarks"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                        table.AddCell(new Phrase(new Chunk(mod_row["TotalMarks"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));

                        if (table.Rows.Count % 11 == 0)
                        {
                            doc.Add(table);
                            table.FlushContent();

                            Paragraph p2 = new Paragraph(" \n\n                 Signature: \b                                                                                                                                               Signature: \n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11));
                            p2.Alignment = Element.ALIGN_LEFT;
                            doc.Add(p2);

                            Paragraph pi = new Paragraph(" \n                  Name: \b                                                                                                                                                     Name: \n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11));
                            pi.Alignment = Element.ALIGN_LEFT;
                            doc.Add(pi);

                            Paragraph p3 = new Paragraph("                 OBSERVER CUM  CHEIF PRACTICAL EXAMINER                     \b                                                  PRACTICAL EXAMINER \n\n\n\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11));
                            p3.Alignment = Element.ALIGN_LEFT;
                            doc.Add(p3);


                            Paragraph p4 = new Paragraph("Note: i) Marks should not be disclosed to the candidates under any circumstances.\n          ii) Over writing, if any  should  be counter signed by  both, the Practical Examiner and the Observer.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11));
                            p4.Alignment = Element.ALIGN_LEFT;
                            doc.Add(p4);

                            doc.NewPage();

                            Paragraph p11 = new Paragraph(" NATIONAL INSTITUTE OF ELECTRONICS AND INFORMATION TECHNOLOGY  \n An Autonomous Scientific Society of Ministry of Electronics and Information Technology , Govt of  India \n  Details of Marks Entered ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13, iTextSharp.text.Font.BOLD));
                            p11.Alignment = Element.ALIGN_CENTER;
                            doc.Add(p11);


                            doc.Add(Chunk.NEWLINE);
                            doc.Add(Chunk.NEWLINE);


                            Paragraph p12 = new Paragraph(" Centre Code  :  " + centreCode + " \n  Date / Time of Examination :  " + info.examDate + " / " + info.examSession + " \n  Batch : " + batchCode + " \n ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11));
                            p12.Alignment = Element.ALIGN_LEFT;
                            doc.Add(p12);

                            table.AddCell("#");
                            table.AddCell("Registration No.");
                            table.AddCell("Candidate Name");
                            table.AddCell("Module Name");
                            table.AddCell("Examiner Marks (Max 40)");
                            table.AddCell("Observer Marks(Max 40)");
                            table.AddCell("Viva Marks(Max 20)");
                            table.AddCell("Total Marks(out of 100)");

                        }
                    }
                    doc.Add(table);

                    Paragraph p21 = new Paragraph(" \n\n                 Signature: \b                                                                                                                                               Signature: \n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11));
                    p21.Alignment = Element.ALIGN_LEFT;
                    doc.Add(p21);

                    Paragraph p22 = new Paragraph(" \n                  Name: \b                                                                                                                                                     Name: \n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11));
                    p22.Alignment = Element.ALIGN_LEFT;
                    doc.Add(p22);

                    Paragraph p23 = new Paragraph("                 OBSERVER CUM  CHEIF PRACTICAL EXAMINER                     \b                                                  PRACTICAL EXAMINER \n\n\n\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11));
                    p23.Alignment = Element.ALIGN_LEFT;
                    doc.Add(p23);


                    Paragraph p24 = new Paragraph("Note: i) Marks should not be disclosed to the candidates under any circumstances.\n          ii) Over writing, if any  should  be counter signed by  both, the Practical Examiner and the Observer.", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11));
                    p24.Alignment = Element.ALIGN_LEFT;
                    doc.Add(p24);

                }

                doc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=ExaminerObserverReport.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(doc);
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

        string batchCode = ddlBatch.SelectedValue;
        string CentreCode = ddlCentreCode.SelectedValue;

        using (var context = new EConnectContext())
        {

            Int64 candidatesCount = (from c in context.PracticalCandidateMarks
                                     where c.CenterCode == CentreCode && c.BatchCode == batchCode
                                     //&& c.ExamMonth == 1 && c.ExamYear == 2023
                                     select c).Count();

            //&& p.ExamMonth == 1 && p.ExamYear == 2023
            Int32 examinerCount = context.PracticalCandidateMarks.Where(p => p.BatchCode == batchCode && p.CenterCode == CentreCode && p.ExaminerMarks40 != null).Select(s => s.ExaminerMarks40).Count();
            Int32 observerCount = context.PracticalCandidateMarks.Where(p => p.BatchCode == batchCode && p.CenterCode == CentreCode && p.ObserverMarks40 != null).Select(s => s.ObserverMarks40).Count();
            Int32 vivaCount = context.PracticalCandidateMarks.Where(p => p.BatchCode == batchCode && p.CenterCode == CentreCode && p.ObserverMarks20 != null).Select(s => s.ObserverMarks20).Count();
//Changed by message 6 Feb 2023
// UnCommented on 07.02.2023 Lakshmi-Vikas 
            if (examinerCount == candidatesCount && observerCount == candidatesCount && vivaCount == candidatesCount)
// Commented on 07.02.2023 Lakshmi-Vikas 
//if (examinerCount !=0 && observerCount !=0 && vivaCount!=0)
            {
                ShowTableHeader();
                ShowData();
                lblReport.Visible = false;
                imgPDF.Visible = true;

            }
            else
            {
                lblReport.Visible = true;
                lblReport.Text = "Marks for all the candidates in the batch are not entered by the Examiner and Observer.";
                //imgPDF.Enabled = false;
                imgPDF.Visible = false;
            }

        }
    }
    protected void ddlCentreCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                String centreCode = ddlCentreCode.SelectedValue;

                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--  Select Batch  --", "0");
                var batchList = from b in context.PracAllowedMarksEntry
                                where b.CenterCode == centreCode
                                select new { ValueField = b.Batch, TextField = b.Batch };
                batchList = batchList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, batchList, lst);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
}