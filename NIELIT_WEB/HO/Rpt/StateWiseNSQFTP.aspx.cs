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
using EConnect.Utils.Common;
using System.Configuration;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Data.OleDb;
using System.IO;

public partial class StateWiseNSQFTP : BasePage
{
    Table tbl = new Table();
    StringBuilder str = new StringBuilder();
    Int32 currentRoleId = 0;
    DataTable dt;
    int gTotal = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }

            currentRoleId = Convert.ToInt32(Session["RoleID"]);

            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/StateWiseNSQFTPReportFilter.aspx"))
            {
                //Testing
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            if (!Page.IsPostBack)
            {
                if ( (!string.IsNullOrEmpty(Request.QueryString["asOnDate"])))
                
                {
                    tbl.CssClass = "sample3";
                    tbl.CellPadding = 2;
                    tbl.CellSpacing = 1;
                    tbl.Width = Unit.Percentage(100);
                    ShowData();
                    divReportData.Controls.Add(tbl);
                }
                else
                {
                    Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../../Home.aspx")));
                    Response.End();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    #region vCode
    protected void ShowData()
    {
        try
        {
            if(!string.IsNullOrEmpty(Request.QueryString["asOnDate"]))
            
            {
              DateTime asOnDate = Convert.ToDateTime(Request.QueryString["asOnDate"]);

                int gTotal = 0;
                
                using (DataTable dt = StateWiseNSQFTPStat( asOnDate))
                {
                    if (dt.Rows.Count > 0)
                    {
                        var application = (from a in dt.AsEnumerable()
                                           select new
                                           {
                                               StateName = a.Field<string>("StateName"),
                                               TPCount = a.Field<int>("TPCount"),
                                               
                                           }).ToList();

                        if (application.Count() > 0)
                        {

                            ShowTableHeader();
                           
                            int i = 0;
                            foreach (var app in application)
                            {
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";

                                
                                TableCell tdRow = new TableCell();
                                tdRow.Width = Unit.Percentage(1);
                                tdRow.Text = (i+1).ToString();
                                tdRow.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow);

                                TableCell tdRow3 = new TableCell();
                                tdRow3.Width = Unit.Percentage(8);
                                tdRow3.Text = app.StateName.ToString();
                                tdRow3.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow3);

                                TableCell tdRow5 = new TableCell();
                                tdRow5.Width = Unit.Percentage(8);
                                tdRow5.Text = app.TPCount.ToString();
                                tdRow5.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow5);

                                gTotal = gTotal + Convert.ToInt32 (app.TPCount);
                                
                                tbl.Rows.Add(tr);
                                i++;
                            }

                            #region GrandTotal
                            //showing total
                            TableRow trTotal = new TableRow();
                            //trTotal.CssClass = "head1";

                            if (i % 2 == 0)
                                trTotal.CssClass = "gdalternate1";
                            else
                                trTotal.CssClass = "gdrow1";

                            TableCell tdNewRow1 = new TableCell();
                            tdNewRow1.Width = Unit.Percentage(1);
                            tdNewRow1.Text = "<b>Total</b>";
                            tdNewRow1.ColumnSpan = 2;
                            tdNewRow1.HorizontalAlign = HorizontalAlign.Right;
                            trTotal.Cells.Add(tdNewRow1);
                            
                            TableCell tdNewRow12 = new TableCell();
                            tdNewRow12.Width = Unit.Percentage(1);
                            tdNewRow12.Text = "<b>" + gTotal + "</b>";
                            tdNewRow12.HorizontalAlign = HorizontalAlign.Left;
                            trTotal.Cells.Add(tdNewRow12);
                        tbl.Rows.Add(trTotal);
                            #endregion
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
            DateTime asOnDate = Convert.ToDateTime(Request.QueryString["asOnDate"]);
                      
            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);


            TableHeaderRow th1 = new TableHeaderRow();
            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.ColumnSpan = 3;
            tcCol11.Width = Unit.Percentage(18);
            tcCol11.Text = "State Wise Short Term NSQF Courses TP Report ";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tcCol11);
            tbl.Rows.Add(th1);


            TableHeaderRow th = new TableHeaderRow();
            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.ColumnSpan = 3;
            tcCol1.Width = Unit.Percentage(18);
            tcCol1.Text = "As On Date : " + asOnDate .ToString ();
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);
            tbl.Rows.Add(th);

            
            #endregion

            #region MainHeader
            TableHeaderRow th2 = new TableHeaderRow();
            th2.CssClass = "head1";

            TableHeaderCell tc = new TableHeaderCell();
            tc.Width = Unit.Percentage(1);
            tc.Text = "<b>#</b>";
            tc.HorizontalAlign = HorizontalAlign.Right;
            th2.Cells.Add(tc);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(8);
            tcCol2.Text = "<b>State Name</b>";
            tcCol2.HorizontalAlign = HorizontalAlign.Left;
            th2.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(8);
            tcCol3.Text = "<b>TP Count</b>";
            tcCol3.HorizontalAlign = HorizontalAlign.Left;
            th2.Cells.Add(tcCol3);

            
            tbl.Rows.Add(th2);
            //

            
            #endregion
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public DataTable StateWiseNSQFTPStat(DateTime asOnDate)
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand scCommand = new SqlCommand("getStateWiseNSQFTPStat", con))
            {
                scCommand.CommandType = CommandType.StoredProcedure;

                
                scCommand.Parameters.Add(new SqlParameter("@pAsOnDate", SqlDbType.Date));
                scCommand.Parameters["@pAsOnDate"].Value = asOnDate;
                
                 con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(scCommand))
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
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=StateWiseNSQFTPStat.pdf");
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
            Response.AddHeader("content-disposition", "attachment;filename=StateWiseNSQFTPStat.xls");
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