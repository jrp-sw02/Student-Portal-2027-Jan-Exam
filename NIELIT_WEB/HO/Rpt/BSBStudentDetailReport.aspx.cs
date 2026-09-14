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
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class HO_Rpt_BSBStudentDetailReport : BasePage
{

    string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    Table tbl = new Table(); 
    protected void Page_Load(object sender, EventArgs e)
    {
        //try
        //{
        //    if (IsSessionAlive() == false)
        //        Response.Redirect("~/index.aspx");
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message);
        //}

    }
    private DataTable GetStudentReport()
    {
        DataTable dt = new DataTable();

        DateTime FromDate = Convert.ToDateTime(txttDateFrom.Text);
        DateTime ToDate = Convert.ToDateTime(txtDateto.Text);
        string BSBCode = txtCode.Text;

        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("BSBStudentDetailRep", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SubmittedFrom", FromDate);
                cmd.Parameters.AddWithValue("@SubmittedTo", ToDate);
                cmd.Parameters.AddWithValue("@BSBSchoolCode", BSBCode);

                con.Open();

                using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                {
                    adpt.Fill(dt);
                }
            }
        }

        return dt;
    }
    protected void ShowTableHeader()
    {
        try
        {

            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(10);


            TableHeaderRow th1 = new TableHeaderRow();
            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.ColumnSpan = 17;
            tcCol11.Width = Unit.Percentage(1);
            tcCol11.Text = "BSB Student Detail Report ";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tcCol11);
            tbl.Rows.Add(th1);


            //TableHeaderRow t1 = new TableHeaderRow();
            //TableHeaderCell tcCol1 = new TableHeaderCell();
            //tcCol1.ColumnSpan = 11;
            //tcCol1.Width = Unit.Percentage(18);
            //tcCol1.Text = "Exam Session: ";
            //tcCol1.HorizontalAlign = HorizontalAlign.Center;
            //t1.Cells.Add(tcCol1);
            //tbl.Rows.Add(t1);

            TableHeaderRow t2 = new TableHeaderRow();
            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.ColumnSpan = 17;
            tcCol2.Width = Unit.Percentage(1);
            tcCol2.Text = "Report Date: " + DateTime.Now.ToString("dd MMM yyyy");
            tcCol2.HorizontalAlign = HorizontalAlign.Right;
            t2.Cells.Add(tcCol2);
            tbl.Rows.Add(t2);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell thc = new TableHeaderCell();
            thc.Width = Unit.Percentage(1);
            thc.HorizontalAlign = HorizontalAlign.Center;
            thc.Text = "SrNo.";
            th.Cells.Add(thc);

            TableHeaderCell thc1 = new TableHeaderCell();
            thc1.Width = Unit.Percentage(1);
            thc1.HorizontalAlign = HorizontalAlign.Center;
            thc1.Text = "Application Number";
            th.Cells.Add(thc1);

            TableHeaderCell thc2 = new TableHeaderCell();
            thc2.Width = Unit.Percentage(1);
            thc2.HorizontalAlign = HorizontalAlign.Center;
            thc2.Text = "Registration Number";
            th.Cells.Add(thc2);

       
            TableHeaderCell thc3 = new TableHeaderCell();
            thc3.Width = Unit.Percentage(1);
            thc3.HorizontalAlign = HorizontalAlign.Center;
            thc3.Text = "Candidate Name";
            th.Cells.Add(thc3);

            TableHeaderCell thc4 = new TableHeaderCell();
            thc4.Width = Unit.Percentage(1);
            thc4.HorizontalAlign = HorizontalAlign.Center;
            thc4.Text = "Father Name";
            th.Cells.Add(thc4);


            TableHeaderCell thc7 = new TableHeaderCell();
            thc7.Width = Unit.Percentage(1);
            thc7.HorizontalAlign = HorizontalAlign.Center;
            thc7.Text = "Mother Name";
            th.Cells.Add(thc7);

            TableHeaderCell thc8 = new TableHeaderCell();
            thc8.Width = Unit.Percentage(1);
            thc8.HorizontalAlign = HorizontalAlign.Center;
            thc8.Text = "Date  of  Birth";
            th.Cells.Add(thc8);


            TableHeaderCell thc10 = new TableHeaderCell();
            thc10.Width = Unit.Percentage(1);
            thc10.HorizontalAlign = HorizontalAlign.Center;
            thc10.Text = "Gender";
            th.Cells.Add(thc10);

            TableHeaderCell thc10a = new TableHeaderCell();
            thc10a.Width = Unit.Percentage(1);
            thc10a.HorizontalAlign = HorizontalAlign.Center;
            thc10a.Text = "Email";
            th.Cells.Add(thc10a);

            TableHeaderCell thc10b = new TableHeaderCell();
            thc10b.Width = Unit.Percentage(1);
            thc10b.HorizontalAlign = HorizontalAlign.Center;
            thc10b.Text = "Mobile";
            th.Cells.Add(thc10b);


            TableHeaderCell thc11 = new TableHeaderCell();
            thc11.Width = Unit.Percentage(1);
            thc11.HorizontalAlign = HorizontalAlign.Center;
            thc11.Text = "Education Qualification";
            th.Cells.Add(thc11);


            TableHeaderCell thc13 = new TableHeaderCell();
            thc13.Width = Unit.Percentage(1);
            thc13.HorizontalAlign = HorizontalAlign.Center;
            thc13.Text = "Address";
            th.Cells.Add(thc13);

            TableHeaderCell thc14 = new TableHeaderCell();
            thc14.Width = Unit.Percentage(1);
            thc14.HorizontalAlign = HorizontalAlign.Center;
            thc14.Text = "City";
            th.Cells.Add(thc14);


             TableHeaderCell thc15 = new TableHeaderCell();
            thc15.Width = Unit.Percentage(1);
            thc15.HorizontalAlign = HorizontalAlign.Center;
            thc15.Text = "State";
            th.Cells.Add(thc15);


            //TableHeaderCell thc19 = new TableHeaderCell();
            //thc19.Width = Unit.Percentage(3);
            //thc19.HorizontalAlign = HorizontalAlign.Center;
            //thc19.Text = "Pincode";
            //th.Cells.Add(thc19);




             TableHeaderCell thc16 = new TableHeaderCell();
            thc16.Width = Unit.Percentage(1);
            thc16.HorizontalAlign = HorizontalAlign.Center;
            thc16.Text = "Form Submission Date";
            th.Cells.Add(thc16);

             TableHeaderCell thc17 = new TableHeaderCell();
            thc17.Width = Unit.Percentage(1);
            thc17.HorizontalAlign = HorizontalAlign.Center;
            thc17.Text = "Payment Status";
            th.Cells.Add(thc17);

            TableHeaderCell thc18 = new TableHeaderCell();
            thc18.Width = Unit.Percentage(1);
            thc18.HorizontalAlign = HorizontalAlign.Center;
            thc18.Text = "BSB Code";
            th.Cells.Add(thc18);

          
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
        // int i = 0;

        // DataTable dt1 = GetCompiledResult();
        try
        {
            using (DataTable dt1 = GetStudentReport())
                {
                    if (dt1.Rows.Count > 0)
                    {
                        var application = (from a in dt1.AsEnumerable()
                                           select new
                                           {
                                               Form_No = a.Field<string>("Number") ?? "",
                                               Registration_Number = a.Field<string>("Registration_Number"),
                                               Candidate_Name = a.Field<string>("Candidate_Name") ?? "",
                                               Father_Name = a.Field<string>("Father_Name") ?? "",
                                               Mother_Name = a.Field<string>("Mother_Name") ?? "",
                                               Dob = a.Field<String>("Dob"),
                                               Gender = a.Field<string>("Gender") ?? "",
                                               email = a.Field<string>("email") ?? "",
                                               mobile = a.Field<string>("mobile") ?? "",
                                               Education_Qualification = a.Field<string>("Education_Qualification") ?? "",
                                               Candidate_Address = a.Field<string>("Candidate_Address") ?? "",
                                               City = a.Field<string>("City") ?? "",
                                               State = a.Field<string>("State") ?? "",
                                              // Pincode = a.Field<Int64?>("Pincode") ?? 0,
                                               Final_Submission_Date = a.Field<String>("Final_Submission_Date"),
                                               Payment_Status = a.Field<string>("Payment_Status") ?? "",
                                               BSB_U_DISECode = a.Field<string>("BSB_U_DISECode") ?? "",                                              
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
                                thc1.Width = Unit.Percentage(1);
                                thc1.HorizontalAlign = HorizontalAlign.Center;
                                thc1.Text = app.Form_No.ToString();
                                tr.Cells.Add(thc1);

                                TableCell thc2 = new TableCell();
                                thc2.Width = Unit.Percentage(1);
                                thc2.HorizontalAlign = HorizontalAlign.Center;
                                thc2.Text = app.Registration_Number.ToString();
                                tr.Cells.Add(thc2);

                                TableCell thc3 = new TableCell();
                                thc3.Width = Unit.Percentage(1);
                                thc3.HorizontalAlign = HorizontalAlign.Center;
                                thc3.Text = app.Candidate_Name.ToString();
                                tr.Cells.Add(thc3);

                                TableCell thc4 = new TableCell();
                                thc4.Width = Unit.Percentage(1);
                                thc4.HorizontalAlign = HorizontalAlign.Center;
                                thc4.Text = app.Father_Name.ToString();
                                tr.Cells.Add(thc4);

                                TableCell thc6 = new TableCell();
                                thc6.Width = Unit.Percentage(1);
                                thc6.HorizontalAlign = HorizontalAlign.Center;
                                thc6.Text = app.Mother_Name.ToString();
                                tr.Cells.Add(thc6);

                                TableCell thc7 = new TableCell();
                                thc7.Width = Unit.Percentage(1);
                                thc7.HorizontalAlign = HorizontalAlign.Center;
                                thc7.Text = app.Dob.ToString();
                                tr.Cells.Add(thc7);

                                TableCell thc9 = new TableCell();
                                thc9.Width = Unit.Percentage(1);
                                thc9.HorizontalAlign = HorizontalAlign.Center;
                                thc9.Text = app.Gender.ToString();
                                tr.Cells.Add(thc9);

                                TableCell thc9a = new TableCell();
                                thc9a.Width = Unit.Percentage(1);
                                thc9a.HorizontalAlign = HorizontalAlign.Center;
                                thc9a.Text = app.email.ToString();
                                tr.Cells.Add(thc9a);

                                TableCell thc9b = new TableCell();
                                thc9b.Width = Unit.Percentage(1);
                                thc9b.HorizontalAlign = HorizontalAlign.Center;
                                thc9b.Text = app.mobile.ToString();
                                tr.Cells.Add(thc9b);

                               
                                TableCell thc10 = new TableCell();
                                thc10.Width = Unit.Percentage(1);
                                thc10.HorizontalAlign = HorizontalAlign.Center;
                                thc10.Text = app.Education_Qualification.ToString();
                                tr.Cells.Add(thc10);

                                TableCell thc12 = new TableCell();
                                thc12.Width = Unit.Percentage(1);
                                thc12.HorizontalAlign = HorizontalAlign.Center;
                                thc12.Text = app.Candidate_Address.ToString();
                                tr.Cells.Add(thc12);

                                TableCell thc14 = new TableCell();
                                thc14.Width = Unit.Percentage(1);
                                thc14.HorizontalAlign = HorizontalAlign.Center;
                                thc14.Text = app.City.ToString();
                                tr.Cells.Add(thc14);

                                TableCell thc15 = new TableCell();
                                thc15.Width = Unit.Percentage(1);
                                thc15.HorizontalAlign = HorizontalAlign.Center;
                                thc15.Text = app.State.ToString();
                                tr.Cells.Add(thc15);

                                //TableCell thc19 = new TableCell();
                                //thc19.Width = Unit.Percentage(3);
                                //thc19.HorizontalAlign = HorizontalAlign.Center;
                                //thc19.Text = app.Pincode.ToString();
                                //tr.Cells.Add(thc19);

                                TableCell thc16 = new TableCell();
                                thc16.Width = Unit.Percentage(1);
                                thc16.HorizontalAlign = HorizontalAlign.Center;
                                thc16.Text = app.Final_Submission_Date.ToString();
                                tr.Cells.Add(thc16);

                                TableCell thc17 = new TableCell();
                                thc17.Width = Unit.Percentage(1);
                                thc17.HorizontalAlign = HorizontalAlign.Center;
                                thc17.Text = app.Payment_Status.ToString();
                                tr.Cells.Add(thc17);

                                TableCell thc18 = new TableCell();
                                thc18.Width = Unit.Percentage(1);
                                thc18.HorizontalAlign = HorizontalAlign.Center;
                                thc18.Text = app.BSB_U_DISECode.ToString();
                                tr.Cells.Add(thc18);

                                tbl.Rows.Add(tr);
                                divReportData.Controls.Add(tbl);
                                // lblError.Visible = false;
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

            }

     
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        ShowData();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtCode.Text = "";
        txtDateto.Text = "";
        txttDateFrom .Text ="";
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
            tbl.Width = Unit.Percentage(100);

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
            Response.AddHeader("content-disposition", "attachment;filename=BSBStudentDetail.pdf");
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
            //ShowTableHeader();
            ShowData();
            divReportData.Controls.Add(tbl);
            Response.Clear();         
            Response.AddHeader("content-disposition", "attachment;filename=BSBStudentDetail.xls");         
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