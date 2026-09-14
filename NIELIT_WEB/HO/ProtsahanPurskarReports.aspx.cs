using System;
using System.IO;
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
using System.Data.OleDb;
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



public partial class ProtsahanPurskarReports : BasePage
{
    String strMessage = string.Empty;  
    Int32 currentRoleId = 0;
   
   System.Web.UI.WebControls.Table tbl = new System.Web.UI.WebControls.Table();
    protected void Page_Load(object sender, EventArgs e)
    {
       
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);         
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                ExamName();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Protsahan Puruskar Reports", "HO/ProtsahanPurskarReports.aspx", ""));
            }
          
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProtsahanPurskarReports.aspx", true);
    }
   
    #region -----DBT Bank Report ------------
   
    protected void btnDownload3_Click(object sender, EventArgs e)
    {
        getDataForPDF();
       
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        hw.AddAttribute("border", "1");
        hw.RenderBeginTag(HtmlTextWriterTag.Font);
        hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "9");

        divReportData.Visible = true;
        tbl.BorderStyle = System.Web.UI.WebControls.BorderStyle.Ridge;
        tbl.CssClass = "sample3";
        tbl.CellPadding = 1;
        tbl.CellSpacing = 1;
        tbl.Width = System.Web.UI.WebControls.Unit.Percentage(100);


        divReportData.Controls.Add(tbl);

        divReportData.RenderControl(hw);
        hw.RenderEndTag();
        Response.Clear();

        StringReader sr = new StringReader(sw.ToString());
        //iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5f, 5f, 5f, 0f);

        iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);
        iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        htmlparser.Parse(sr);
        pdfDoc.Close();

        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=PuruskarDBTBankReport.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        Response.Write(pdfDoc);
        Response.End();
    }
      
    protected void getDataForPDF()
    {
        try
        {
            ShowTableHeaderForPDF();

            Int64 totAmountReleased = 0;
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand scCommand = new SqlCommand("GetDataForPurskarApplicationDBTBankReport", con);
            scCommand.CommandType = CommandType.StoredProcedure;
            //scCommand.Parameters.Add("@CodeForViewRecord", SqlDbType.Int).Value = 14;
            //scCommand.Parameters.Add("@InstituteID", SqlDbType.Int).Value = 0;
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

                int i;
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {                  
                        TableRow tr = new TableRow();
                        if (i % 2 == 0)
                            tr.CssClass = "gdalternate1";
                        else
                            tr.CssClass = "gdrow1";
                     string datacheck = ds.Tables[0].Rows[i]["RECOMMENDED"].ToString();
                     if (datacheck == "RECOMMENDED")
                     {
                         TableCell tdRow = new TableCell();
                         tdRow.Width = System.Web.UI.WebControls.Unit.Percentage(1);
                         tdRow.Text = (i + 1).ToString();
                         tdRow.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                         tr.Cells.Add(tdRow);

                         TableCell tdRow22 = new TableCell();
                         tdRow22.Width = Unit.Percentage(5);
                         tdRow22.Text = ds.Tables[0].Rows[i]["REFERENCE_NO"].ToString();
                         tdRow22.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Justify;
                         tr.Cells.Add(tdRow22);

                         TableCell tdRow2 = new TableCell();
                         tdRow2.Width = Unit.Percentage(5);
                         tdRow2.Text = ds.Tables[0].Rows[i]["NAME_OF_CANDIDATE"].ToString().ToUpper();
                         tdRow2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                         tr.Cells.Add(tdRow2);

                         TableCell tdRow2e = new TableCell();
                         tdRow2e.Width = Unit.Percentage(5);
                         // tdRow2e.Text = EncryptDecrypt.DecryptString(ds.Tables[0].Rows[i]["AADHAAR_NO"].ToString()).ToString();
                         tdRow2e.Text = ds.Tables[0].Rows[i]["AADHAAR_NO"].ToString().ToString();
                         tdRow2e.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                         tr.Cells.Add(tdRow2e);

                         TableCell tdRow4 = new TableCell();
                         tdRow4.Width = Unit.Percentage(5);
                         tdRow4.Text = ds.Tables[0].Rows[i]["REGN_NO"].ToString();
                         tdRow4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                         tr.Cells.Add(tdRow4);

                         TableCell tdRow2b = new TableCell();
                         tdRow2b.Width = Unit.Percentage(5);
                         tdRow2b.Text = ds.Tables[0].Rows[i]["moduleCountProcessed"].ToString();
                         tdRow2b.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                         tr.Cells.Add(tdRow2b);

                         TableCell tdRow23 = new TableCell();
                         tdRow23.Width = Unit.Percentage(5);
                         string a = ds.Tables[0].Rows[i]["AmountReleased"].ToString();
                         if (a == "0")
                         {
                             tdRow23.Text = "--";
                         }
                         else
                         {
                             tdRow23.Text = ds.Tables[0].Rows[i]["AmountReleased"].ToString() + "/-";
                         }
                         tdRow23.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                         tr.Cells.Add(tdRow23);


                         totAmountReleased = totAmountReleased + Convert.ToInt64(ds.Tables[0].Rows[i]["AmountReleased"].ToString());

                         tbl.Rows.Add(tr);
                     }
                }

                TableRow trNew = new TableRow();
                if (i % 2 == 0)
                    trNew.CssClass = "gdalternate1";
                else
                    trNew.CssClass = "gdrow1";
                TableCell tdNewRow1 = new TableCell();
                tdNewRow1.Width = System.Web.UI.WebControls.Unit.Percentage(1);
                tdNewRow1.Height = System.Web.UI.WebControls.Unit.Percentage(15);
                tdNewRow1.Text = "";
                tdNewRow1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                tdNewRow1.BorderWidth = 0;
                tdNewRow1.ColumnSpan = 4;
                tdNewRow1.BorderColor = System.Drawing.Color.White;
                trNew.Cells.Add(tdNewRow1);

                TableCell tdNewRow21 = new TableCell();
                tdNewRow21.Width = System.Web.UI.WebControls.Unit.Percentage(8);
                tdNewRow21.Height = System.Web.UI.WebControls.Unit.Percentage(15);
                tdNewRow21.ColumnSpan = 1;
                tdNewRow21.Text = "<b>Total :</b> &nbsp;&nbsp;" + totAmountReleased.ToString();
                tdNewRow21.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                tdNewRow21.BorderWidth = 0;
                tdNewRow21.BorderColor = System.Drawing.Color.White;
                trNew.Cells.Add(tdNewRow21);

                tbl.Rows.Add(trNew);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
   
    protected void ShowTableHeaderForPDF()
    {
        try
        {           
            TableHeaderRow th1 = new TableHeaderRow();
            th1.CssClass = "head1";
            TableHeaderCell tc1 = new TableHeaderCell();
            TableHeaderCell tc11 = new TableHeaderCell();
            tc1.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            tc1.ColumnSpan = 7;
            tc1.Text = "PROTSAHAN PURUSKAR <br> DBT Bank Report <br>List of Candidates Recommended for the session of " + ddlflExam.SelectedItem.Text.ToUpper() + " Examination ";
            tc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th1.Cells.Add(tc1);

            tbl.Rows.Add(th1);

            TableHeaderRow th2 = new TableHeaderRow();
            th2.CssClass = "head1";

            TableHeaderCell tc2 = new TableHeaderCell();
            tc2.Width = Unit.Percentage(100);
            tc2.ColumnSpan = 7;
            tc2.Text = "Report Generated on: " + System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString();
            tc2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            th2.Cells.Add(tc2);

            tbl.Rows.Add(th2);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";
            th.BorderColor = Color.LightGray;

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "#";
            tcCol.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(5);
            tcCol5.Text = "Ref No.";
            tcCol5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol5);
           

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(5);
            tcCol2.Text = "NAME OF CANDIDATE";
            tcCol2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol2e = new TableHeaderCell();
            tcCol2e.Width = Unit.Percentage(5);
            tcCol2e.Text = "AADHAAR NUMBER";
            tcCol2e.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2e);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(5);
            tcCol4.Text = "REGN_NO";
            tcCol4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol2b = new TableHeaderCell();
            tcCol2b.Width = Unit.Percentage(5);
            tcCol2b.Text = "FINAL PAPER";
            tcCol2b.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2b);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(5);
            tcCol6.Text = "SCHOLARSHIP_AMT";
            tcCol6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

          

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    #endregion -----DBT Bank Report ------------

    #region -----Master List of All Report ------------
   
    protected void btnDownload1_Click(object sender, EventArgs e)
    {
        getDataForPDF1();

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        hw.AddAttribute("border", "1");
        hw.RenderBeginTag(HtmlTextWriterTag.Font);
        hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "9");

        divReportData.Visible = true;
        tbl.BorderStyle = System.Web.UI.WebControls.BorderStyle.Ridge;
        tbl.CssClass = "sample3";
        tbl.CellPadding = 1;
        tbl.CellSpacing = 1;
        tbl.Width = System.Web.UI.WebControls.Unit.Percentage(100);


        divReportData.Controls.Add(tbl);

        divReportData.RenderControl(hw);
        hw.RenderEndTag();
        Response.Clear();

        StringReader sr = new StringReader(sw.ToString());
        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
        //iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5f, 5f, 5f, 0f);

        iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);
        iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        htmlparser.Parse(sr);
        pdfDoc.Close();

        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=PuruskarMasterListOfAllReport.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        Response.Write(pdfDoc);
        Response.End();
    }
      
    protected void getDataForPDF1()
    {
        try
        {
            ShowTableHeaderForPDF1();

            Int64 totAmountReleased = 0;
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand scCommand = new SqlCommand("GetDataForPurskarApplicationDBTBankReport", con);
            scCommand.CommandType = CommandType.StoredProcedure;
            //scCommand.Parameters.Add("@CodeForViewRecord", SqlDbType.Int).Value = 14;
            //scCommand.Parameters.Add("@InstituteID", SqlDbType.Int).Value = 0;
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

                int i;
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    TableRow tr = new TableRow();
                    if (i % 2 == 0)
                        tr.CssClass = "gdalternate1";
                    else
                        tr.CssClass = "gdrow1";

                    TableCell tdRow = new TableCell();
                    tdRow.Width = System.Web.UI.WebControls.Unit.Percentage(1);
                    tdRow.Text = (i + 1).ToString();
                    tdRow.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tr.Cells.Add(tdRow);

                    //TableCell tdRow22 = new TableCell();
                    //tdRow22.Width = Unit.Percentage(5);
                    //tdRow22.Text = ds.Tables[0].Rows[i]["REFERENCE_NO"].ToString();
                    //tdRow22.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Justify;
                    //tr.Cells.Add(tdRow22);

                    TableCell tdRow2 = new TableCell();
                    tdRow2.Width = Unit.Percentage(5);
                    tdRow2.Text = ds.Tables[0].Rows[i]["NAME_OF_CANDIDATE"].ToString().ToUpper();
                    tdRow2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    tr.Cells.Add(tdRow2);

                    TableCell tdRow2e = new TableCell();
                    tdRow2e.Width = Unit.Percentage(5);
                    // tdRow2e.Text = EncryptDecrypt.DecryptString(ds.Tables[0].Rows[i]["AADHAAR_NO"].ToString()).ToString();
                    tdRow2e.Text = ds.Tables[0].Rows[i]["AADHAAR_NO"].ToString().ToString();
                    tdRow2e.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tr.Cells.Add(tdRow2e);

                    TableCell tdRow2l = new TableCell();
                    tdRow2l.Width = Unit.Percentage(5);
                    tdRow2l.Text = ds.Tables[0].Rows[i]["LEVEL"].ToString();
                    tdRow2l.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tr.Cells.Add(tdRow2l);

                    TableCell tdRow4 = new TableCell();
                    tdRow4.Width = Unit.Percentage(5);
                    tdRow4.Text = ds.Tables[0].Rows[i]["REGN_NO"].ToString();
                    tdRow4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tr.Cells.Add(tdRow4);

                    TableCell tdRow2s = new TableCell();
                    tdRow2s.Width = Unit.Percentage(5);
                    tdRow2s.Text = ds.Tables[0].Rows[i]["SEX_CAT"].ToString();
                    tdRow2s.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tr.Cells.Add(tdRow2s);

                    TableCell tdRow2b = new TableCell();
                    tdRow2b.Width = Unit.Percentage(5);
                    tdRow2b.Text = ds.Tables[0].Rows[i]["moduleCountProcessed"].ToString();
                    tdRow2b.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tr.Cells.Add(tdRow2b);

                    
                    TableCell tdRow2p = new TableCell();
                    tdRow2p.Width = Unit.Percentage(5);
                    tdRow2p.Text = ds.Tables[0].Rows[i]["DESC_OF_PAPERS"].ToString();
                    tdRow2p.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tr.Cells.Add(tdRow2p);

                    TableCell tdRow2r = new TableCell();
                    tdRow2r.Width = Unit.Percentage(5);
                    tdRow2r.Text = ds.Tables[0].Rows[i]["RECOMMENDED"].ToString();
                    tdRow2r.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tr.Cells.Add(tdRow2r);


                    TableCell tdRow23 = new TableCell();
                    tdRow23.Width = Unit.Percentage(5);
                    string a = ds.Tables[0].Rows[i]["AmountReleased"].ToString();
                    if (a == "0")
                    {
                        tdRow23.Text = "--";
                    }
                    else
                    {
                        tdRow23.Text = ds.Tables[0].Rows[i]["AmountReleased"].ToString() + "/-";
                    }
                    tdRow23.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tr.Cells.Add(tdRow23);

                    TableCell tdRow2rR = new TableCell();
                    tdRow2rR.Width = Unit.Percentage(5);
                    tdRow2rR.Text = "";
                    tdRow2rR.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tr.Cells.Add(tdRow2rR);

                    totAmountReleased = totAmountReleased + Convert.ToInt64(ds.Tables[0].Rows[i]["AmountReleased"].ToString());

                    tbl.Rows.Add(tr);

                }

                TableRow trNew = new TableRow();
                if (i % 2 == 0)
                    trNew.CssClass = "gdalternate1";
                else
                    trNew.CssClass = "gdrow1";
                TableCell tdNewRow1 = new TableCell();
                tdNewRow1.Width = System.Web.UI.WebControls.Unit.Percentage(1);
                tdNewRow1.Height = System.Web.UI.WebControls.Unit.Percentage(15);
                tdNewRow1.Text = "";
                tdNewRow1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                tdNewRow1.BorderWidth = 0;
                tdNewRow1.ColumnSpan = 4;
                tdNewRow1.BorderColor = System.Drawing.Color.White;
                trNew.Cells.Add(tdNewRow1);

                TableCell tdNewRow21 = new TableCell();
                tdNewRow21.Width = System.Web.UI.WebControls.Unit.Percentage(8);
                tdNewRow21.Height = System.Web.UI.WebControls.Unit.Percentage(15);
                tdNewRow21.ColumnSpan = 1;
                tdNewRow21.Text = "<b>Total :</b> &nbsp;&nbsp;" + totAmountReleased.ToString();
                tdNewRow21.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                tdNewRow21.BorderWidth = 0;
                tdNewRow21.BorderColor = System.Drawing.Color.White;
                trNew.Cells.Add(tdNewRow21);

                tbl.Rows.Add(trNew);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    
    protected void ShowTableHeaderForPDF1()
    {
        try
        {
            TableHeaderRow th1 = new TableHeaderRow();
            th1.CssClass = "head1";
            TableHeaderCell tc1 = new TableHeaderCell();
            TableHeaderCell tc11 = new TableHeaderCell();
            tc1.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            tc1.ColumnSpan = 11;
            tc1.Text = "PROTSAHAN PURUSKAR MASTER LIST OF ALL FOR - " + ddlflExam.SelectedItem.Text.ToUpper() + " REPORT ";
            tc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th1.Cells.Add(tc1);

            tbl.Rows.Add(th1);

            TableHeaderRow th2 = new TableHeaderRow();
            th2.CssClass = "head1";

            TableHeaderCell tc2 = new TableHeaderCell();
            tc2.Width = Unit.Percentage(100);
            tc2.ColumnSpan = 11;
            tc2.Text = "Report Generated on: " + System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString();
            tc2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            th2.Cells.Add(tc2);

            tbl.Rows.Add(th2);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";
            th.BorderColor = Color.LightGray;

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "SL";
            tcCol.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol);

           
            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(5);
            tcCol2.Text = "NAME OF CANDIDATE";
            tcCol2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol2e = new TableHeaderCell();
            tcCol2e.Width = Unit.Percentage(5);
            tcCol2e.Text = "AADHAAR NUMBER";
            tcCol2e.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2e);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(5);
            tcCol5.Text = "LEVEL";
            tcCol5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(5);
            tcCol4.Text = "REGN_NO";
            tcCol4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol2S = new TableHeaderCell();
            tcCol2S.Width = Unit.Percentage(5);
            tcCol2S.Text = "SEX_CAT";
            tcCol2S.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2S);

            TableHeaderCell tcCol2b = new TableHeaderCell();
            tcCol2b.Width = Unit.Percentage(5);
            tcCol2b.Text = "FINAL PAPER";
            tcCol2b.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2b);

            TableHeaderCell tcCol2P = new TableHeaderCell();
            tcCol2P.Width = Unit.Percentage(5);
            tcCol2P.Text = "DESC OF PAPERS";
            tcCol2P.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2P);

            TableHeaderCell tcCol2R = new TableHeaderCell();
            tcCol2R.Width = Unit.Percentage(5);
            tcCol2R.Text = "RECOMMENDED";
            tcCol2R.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2R);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(5);
            tcCol6.Text = "SCHOLARSHIP_AMT";
            tcCol6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

            TableHeaderCell tcCol2RR = new TableHeaderCell();
            tcCol2RR.Width = Unit.Percentage(5);
            tcCol2RR.Text = "REMARKS";
            tcCol2RR.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2RR);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    #endregion -----Master List of All Report ------------

    #region -----Recommended Report ------------
   
    protected void btnDownload2_Click(object sender, EventArgs e)
    {
        getDataForPDF2();

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        hw.AddAttribute("border", "1");
        hw.RenderBeginTag(HtmlTextWriterTag.Font);
        hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "9");

        divReportData.Visible = true;
        tbl.BorderStyle = System.Web.UI.WebControls.BorderStyle.Ridge;
        tbl.CssClass = "sample3";
        tbl.CellPadding = 1;
        tbl.CellSpacing = 1;
        tbl.Width = System.Web.UI.WebControls.Unit.Percentage(100);


        divReportData.Controls.Add(tbl);

        divReportData.RenderControl(hw);
        hw.RenderEndTag();
        Response.Clear();

        StringReader sr = new StringReader(sw.ToString());
        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
        //iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 5f, 5f, 5f, 0f);

        iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);
        iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        htmlparser.Parse(sr);
        pdfDoc.Close();

        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=PuruskarRecommendedReport.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        Response.Write(pdfDoc);
        Response.End();
    }
    
    protected void getDataForPDF2()
    {
        try
        {
            ShowTableHeaderForPDF2();

            Int64 totAmountReleased = 0;
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand scCommand = new SqlCommand("GetDataForPurskarApplicationDBTBankReport", con);
            scCommand.CommandType = CommandType.StoredProcedure;
            //scCommand.Parameters.Add("@CodeForViewRecord", SqlDbType.Int).Value = 14;
            //scCommand.Parameters.Add("@InstituteID", SqlDbType.Int).Value = 0;
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

                int i;
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    TableRow tr = new TableRow();
                    if (i % 2 == 0)
                        tr.CssClass = "gdalternate1";
                    else
                        tr.CssClass = "gdrow1";
                    string datacheck = ds.Tables[0].Rows[i]["RECOMMENDED"].ToString();
                    if (datacheck == "RECOMMENDED")
                    {
                        TableCell tdRow = new TableCell();
                        tdRow.Width = System.Web.UI.WebControls.Unit.Percentage(1);
                        tdRow.Text = (i + 1).ToString();
                        tdRow.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tr.Cells.Add(tdRow);

                        TableCell tdRow22 = new TableCell();
                        tdRow22.Width = Unit.Percentage(5);
                        tdRow22.Text = ds.Tables[0].Rows[i]["REFERENCE_NO"].ToString();
                        tdRow22.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Justify;
                        tr.Cells.Add(tdRow22);

                        TableCell tdRow2 = new TableCell();
                        tdRow2.Width = Unit.Percentage(5);
                        tdRow2.Text = ds.Tables[0].Rows[i]["NAME_OF_CANDIDATE"].ToString().ToUpper();
                        tdRow2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                        tr.Cells.Add(tdRow2);

                        TableCell tdRow2e = new TableCell();
                        tdRow2e.Width = Unit.Percentage(5);
                        // tdRow2e.Text = EncryptDecrypt.DecryptString(ds.Tables[0].Rows[i]["AADHAAR_NO"].ToString()).ToString();
                        tdRow2e.Text = ds.Tables[0].Rows[i]["AADHAAR_NO"].ToString().ToString();
                        tdRow2e.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tr.Cells.Add(tdRow2e);

                        TableCell tdRow2l = new TableCell();
                        tdRow2l.Width = Unit.Percentage(5);
                        tdRow2l.Text = ds.Tables[0].Rows[i]["LEVEL"].ToString();
                        tdRow2l.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tr.Cells.Add(tdRow2l);

                        TableCell tdRow4 = new TableCell();
                        tdRow4.Width = Unit.Percentage(5);
                        tdRow4.Text = ds.Tables[0].Rows[i]["REGN_NO"].ToString();
                        tdRow4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tr.Cells.Add(tdRow4);

                        TableCell tdRow2s = new TableCell();
                        tdRow2s.Width = Unit.Percentage(5);
                        tdRow2s.Text = ds.Tables[0].Rows[i]["SEX_CAT"].ToString();
                        tdRow2s.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tr.Cells.Add(tdRow2s);

                        TableCell tdRow2b = new TableCell();
                        tdRow2b.Width = Unit.Percentage(5);
                        tdRow2b.Text = ds.Tables[0].Rows[i]["moduleCountProcessed"].ToString();
                        tdRow2b.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tr.Cells.Add(tdRow2b);


                        TableCell tdRow2p = new TableCell();
                        tdRow2p.Width = Unit.Percentage(5);
                        tdRow2p.Text = ds.Tables[0].Rows[i]["DESC_OF_PAPERS"].ToString();
                        tdRow2p.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tr.Cells.Add(tdRow2p);

                        TableCell tdRow2r = new TableCell();
                        tdRow2r.Width = Unit.Percentage(5);
                        tdRow2r.Text = ds.Tables[0].Rows[i]["RECOMMENDED"].ToString();
                        tdRow2r.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tr.Cells.Add(tdRow2r);


                        TableCell tdRow23 = new TableCell();
                        tdRow23.Width = Unit.Percentage(5);
                        string a = ds.Tables[0].Rows[i]["AmountReleased"].ToString();
                        if (a == "0")
                        {
                            tdRow23.Text = "--";
                        }
                        else
                        {
                            tdRow23.Text = ds.Tables[0].Rows[i]["AmountReleased"].ToString() + "/-";
                        }
                        tdRow23.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tr.Cells.Add(tdRow23);

                        TableCell tdRow2rR = new TableCell();
                        tdRow2rR.Width = Unit.Percentage(5);
                        tdRow2rR.Text = "";
                        tdRow2rR.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        tr.Cells.Add(tdRow2rR);

                        totAmountReleased = totAmountReleased + Convert.ToInt64(ds.Tables[0].Rows[i]["AmountReleased"].ToString());

                        tbl.Rows.Add(tr);
                    }

                }

                TableRow trNew = new TableRow();
                if (i % 2 == 0)
                    trNew.CssClass = "gdalternate1";
                else
                    trNew.CssClass = "gdrow1";
                TableCell tdNewRow1 = new TableCell();
                tdNewRow1.Width = System.Web.UI.WebControls.Unit.Percentage(1);
                tdNewRow1.Height = System.Web.UI.WebControls.Unit.Percentage(15);
                tdNewRow1.Text = "";
                tdNewRow1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                tdNewRow1.BorderWidth = 0;
                tdNewRow1.ColumnSpan = 4;
                tdNewRow1.BorderColor = System.Drawing.Color.White;
                trNew.Cells.Add(tdNewRow1);

                TableCell tdNewRow21 = new TableCell();
                tdNewRow21.Width = System.Web.UI.WebControls.Unit.Percentage(8);
                tdNewRow21.Height = System.Web.UI.WebControls.Unit.Percentage(15);
                tdNewRow21.ColumnSpan = 1;
                tdNewRow21.Text = "<b>Total :</b> &nbsp;&nbsp;" + totAmountReleased.ToString();
                tdNewRow21.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                tdNewRow21.BorderWidth = 0;
                tdNewRow21.BorderColor = System.Drawing.Color.White;
                trNew.Cells.Add(tdNewRow21);

                tbl.Rows.Add(trNew);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
   
    protected void ShowTableHeaderForPDF2()
    {
        try
        {
            TableHeaderRow th1 = new TableHeaderRow();
            th1.CssClass = "head1";
            TableHeaderCell tc1 = new TableHeaderCell();
            TableHeaderCell tc11 = new TableHeaderCell();
            tc1.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            tc1.ColumnSpan = 12;
            tc1.Text = "PROTSAHAN PURUSKAR <br> List of Candidates Recommended for the session of - " + ddlflExam.SelectedItem.Text.ToUpper() + " Examination ";
            tc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th1.Cells.Add(tc1);

            tbl.Rows.Add(th1);

            TableHeaderRow th2 = new TableHeaderRow();
            th2.CssClass = "head1";

            TableHeaderCell tc2 = new TableHeaderCell();
            tc2.Width = Unit.Percentage(100);
            tc2.ColumnSpan = 12;
            tc2.Text = "Report Generated on: " + System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString();
            tc2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            th2.Cells.Add(tc2);

            tbl.Rows.Add(th2);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";
            th.BorderColor = Color.LightGray;

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "SL";
            tcCol.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol55 = new TableHeaderCell();
            tcCol55.Width = Unit.Percentage(5);
            tcCol55.Text = "Ref No.";
            tcCol55.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol55);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(5);
            tcCol2.Text = "NAME OF CANDIDATE";
            tcCol2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol2e = new TableHeaderCell();
            tcCol2e.Width = Unit.Percentage(5);
            tcCol2e.Text = "AADHAAR NUMBER";
            tcCol2e.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2e);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(5);
            tcCol5.Text = "LEVEL";
            tcCol5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(5);
            tcCol4.Text = "REGN_NO";
            tcCol4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol2S = new TableHeaderCell();
            tcCol2S.Width = Unit.Percentage(5);
            tcCol2S.Text = "SEX_CAT";
            tcCol2S.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2S);

            TableHeaderCell tcCol2b = new TableHeaderCell();
            tcCol2b.Width = Unit.Percentage(5);
            tcCol2b.Text = "FINAL PAPER";
            tcCol2b.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2b);

            TableHeaderCell tcCol2P = new TableHeaderCell();
            tcCol2P.Width = Unit.Percentage(5);
            tcCol2P.Text = "DESC OF PAPERS";
            tcCol2P.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2P);

            TableHeaderCell tcCol2R = new TableHeaderCell();
            tcCol2R.Width = Unit.Percentage(5);
            tcCol2R.Text = "RECOMMENDED";
            tcCol2R.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2R);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(5);
            tcCol6.Text = "SCHOLARSHIP_AMT";
            tcCol6.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

            TableHeaderCell tcCol2RR = new TableHeaderCell();
            tcCol2RR.Width = Unit.Percentage(5);
            tcCol2RR.Text = "REMARKS";
            tcCol2RR.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2RR);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    #endregion -----Recommended Report ------------
   
   
    public void ExamName()
    {
        System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--All--", "0");
        using (System.Data.DataTable dt = GetDataForPurskarApplicationCommitteeReport(1))
        {
            if (dt.Rows.Count > 0)
            {
                int k;
                for (k = 0; k < 1; k++)
                {
                    ddlflExam.DataSource = dt;
                    ddlflExam.DataTextField = "ExamName";
                    ddlflExam.DataValueField = "ExamID";
                    ddlflExam.DataBind();
                    
                }
            }
            else
            {
                ddlflExam.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select One--", "0"));
            }
        }

    }

    public System.Data.DataTable GetDataForPurskarApplicationCommitteeReport(int ViewCode)
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        System.Data.DataTable myDt = new System.Data.DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetDataForPurskarApplicationCommitteeReport", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ViewRecord", SqlDbType.Int));
                cmd.Parameters["@ViewRecord"].Value = ViewCode;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }   
   
   
}  
