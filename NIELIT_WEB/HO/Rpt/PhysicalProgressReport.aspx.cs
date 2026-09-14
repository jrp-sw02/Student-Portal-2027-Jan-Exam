using EConnect.DAL;
using EConnect.URM;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HO_Rpt_PhysicalProgressReport : System.Web.UI.Page
{
    Table tbl = new Table();
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/PhysicalProgressReportFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);

            //loginUserType = 10;
            //currentRoleId = 21;
            //entityID = 285716;

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

        }
    }
    protected void ShowTableHeader()
    {
        try
        {
            int centreId = Convert.ToInt32(Request.QueryString["centreId"]);

            TableHeaderRow th1 = new TableHeaderRow();
            th1.CssClass = "head1";


            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(1);
            tcCol2.Text = "  ";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.ColumnSpan = 6;
            tcCol3.Width = Unit.Percentage(1);
            tcCol3.Text = "FY " + (System.DateTime.Today.Year - 1).ToString() + "-" + (System.DateTime.Today.Year).ToString();
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            //tcCol1.Width = Unit.Percentage(8);
            tcCol4.ColumnSpan = 8;
            tcCol4.Text = "FY " + (System.DateTime.Today.Year).ToString() + "-" + (System.DateTime.Today.Year + 1).ToString(); ;
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tcCol4);

            tbl.Rows.Add(th1);


            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "Course Category ";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(8);
            tcCol1.Text = "Yearly Target assigned by HQ";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);


            TableHeaderCell tcCol2a = new TableHeaderCell();
            tcCol2a.Width = Unit.Percentage(8);
            tcCol2a.HorizontalAlign = HorizontalAlign.Center;
            tcCol2a.Text = "(Qtr-1) Apr to June";
            th.Cells.Add(tcCol2a);

            TableHeaderCell tcCol3a = new TableHeaderCell();
            tcCol3a.Width = Unit.Percentage(8);
            tcCol3a.HorizontalAlign = HorizontalAlign.Center;
            tcCol3a.Text = "(Qtr-2) July to Sept";
            th.Cells.Add(tcCol3a);

            TableHeaderCell tcCol3b = new TableHeaderCell();
            tcCol3b.Width = Unit.Percentage(8);
            tcCol3b.HorizontalAlign = HorizontalAlign.Center;
            tcCol3b.Text = "(Qtr-3) Oct to Dec";
            th.Cells.Add(tcCol3b);

            TableHeaderCell tcCol4a = new TableHeaderCell();
            tcCol4a.Width = Unit.Percentage(8);
            tcCol4a.Text = "(Qtr-4) Jan to Mar";
            tcCol4a.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4a);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(10);
            tcCol5.Text = "Total";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);


            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.Width = Unit.Percentage(8);
            tcCol11.Text = "Yearly Target assigned by HQ";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol11);


            TableHeaderCell tcCol12 = new TableHeaderCell();
            tcCol12.Width = Unit.Percentage(8);
            tcCol12.HorizontalAlign = HorizontalAlign.Center;
            tcCol12.Text = "(Qtr-1) Apr to June";
            th.Cells.Add(tcCol12);

            TableHeaderCell tcCol13 = new TableHeaderCell();
            tcCol13.Width = Unit.Percentage(8);
            tcCol13.HorizontalAlign = HorizontalAlign.Center;
            tcCol13.Text = "(Qtr-2) July to Sept";
            th.Cells.Add(tcCol13);

            TableHeaderCell tcCol13b = new TableHeaderCell();
            tcCol13b.Width = Unit.Percentage(8);
            tcCol13b.HorizontalAlign = HorizontalAlign.Center;
            tcCol13b.Text = "(Qtr-3) Oct to Dec";
            th.Cells.Add(tcCol13b);

            TableHeaderCell tcCol14 = new TableHeaderCell();
            tcCol14.Width = Unit.Percentage(8);
            tcCol14.Text = "(Qtr-4) Jan to Mar";
            tcCol14.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol14);

            TableHeaderCell tcCol15 = new TableHeaderCell();
            tcCol15.Width = Unit.Percentage(10);
            tcCol15.Text = "Total";
            tcCol15.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol15);


            TableHeaderCell tcCol16 = new TableHeaderCell();
            tcCol16.Width = Unit.Percentage(8);
            tcCol16.Text = "Percentage of Achievement w.r.t Target (%)"; 
            tcCol16.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol16);

            TableHeaderCell tcCol17 = new TableHeaderCell();
            tcCol17.Width = Unit.Percentage(10);
            tcCol17.Text = "Overall Percentage (%)";
            tcCol17.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol17);


            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            //ShowAlert(ex.Message);
        }

    }
    protected void ShowTableHeader2(string centrename)
    {
        try
        {
            int vQuarter = Convert.ToInt32(Request.QueryString["quarter"]);
            int vYear = Convert.ToInt32(Request.QueryString["year"]);


            TableRow tr = new TableRow();
            TableCell tc = new TableCell();

            tc.ColumnSpan = 15;
            tr.Cells.Add(tc);
            tbl.Rows.Add(tr);

            TableRow tr1 = new TableRow();
            TableCell tc1 = new TableCell();

            tc1.ColumnSpan = 15;
            tr1.Cells.Add(tc1);
            tbl.Rows.Add(tr1);

            TableRow tr2 = new TableRow();
            TableCell tc2 = new TableCell();

            tc2.ColumnSpan = 15;
            tc2.Text = "Physical Progress Report ";
            tr2.Cells.Add(tc2);
            tbl.Rows.Add(tr2);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "Centre: ";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            //tcCol1.Width = Unit.Percentage(8);
            tcCol1.ColumnSpan = 14;
            tcCol1.Text = centrename;
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            tbl.Rows.Add(th);

        }
        catch (Exception ex)
        {
            //ShowAlert(ex.Message);
        }

    }
    protected void ShowData()
    {
        try
        {

            lblError.Visible = false;

            NIELITMISContext context = new NIELITMISContext();

            int centreId = Convert.ToInt32(Request.QueryString["centreId"]);

            StringBuilder mySql = new StringBuilder();


            using (ds= GetData(centreId))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //ShowTableHeader();
                    string oldCentreName = "x";


                    int i, x;
                    x = 0;
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string centreName = ds.Tables[0].Rows[i]["centreName"].ToString();

                        if (oldCentreName != centreName)
                        {
                            x = 1;
                            ShowTableHeader2(centreName);
                            ShowTableHeader();
                            oldCentreName = centreName;
                        }
                        else
                            x++;	

                        TableRow tr = new TableRow();
                        //  int i = 1;
                        if (i % 2 == 0)
                            tr.CssClass = "gdalternate1";
                        else
                            tr.CssClass = "gdrow1";
                        ds.Tables[0].NewRow();
                        //TableCell tdRow = new TableCell();
                        //tdRow.Width = Unit.Percentage(1);
                        //tdRow.Text = (i+1).ToString();
                        //tdRow.HorizontalAlign = HorizontalAlign.Center;
                        //tr.Cells.Add(tdRow);


                        TableCell tdRow1 = new TableCell();
                        tdRow1.Width = Unit.Percentage(10);
                        tdRow1.Text = ds.Tables[0].Rows[i]["coursecategory"].ToString();
                        tdRow1.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow1);

                        TableCell tdRow2 = new TableCell();
                        tdRow2.Width = Unit.Percentage(10);
                        tdRow2.Text = ds.Tables[0].Rows[i]["y1Target"].ToString();
                        tdRow2.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow2);

                        TableCell tdRow3 = new TableCell();
                        tdRow3.Width = Unit.Percentage(12);
                        tdRow3.Text = ds.Tables[0].Rows[i]["y1Q1"].ToString();
                        tdRow3.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow3);
                        
                        TableCell tdRow13 = new TableCell();
                        tdRow13.Width = Unit.Percentage(10);
                        tdRow13.Text = ds.Tables[0].Rows[i]["y1Q2"].ToString();
                        tdRow13.HorizontalAlign = HorizontalAlign.Center;
                        tdRow13.Wrap = false;
                        tr.Cells.Add(tdRow13);

                        TableCell tdRow14 = new TableCell();
                        tdRow14.Width = Unit.Percentage(5);
                        tdRow14.Text = ds.Tables[0].Rows[i]["y1Q3"].ToString();
                        tdRow14.HorizontalAlign = HorizontalAlign.Center;
                        tdRow14.Wrap = false;
                        tr.Cells.Add(tdRow14);

                        TableCell tdRow6 = new TableCell();
                        tdRow6.Width = Unit.Percentage(5);
                        tdRow6.Text = ds.Tables[0].Rows[i]["y1Q4"].ToString();
                        tdRow6.HorizontalAlign = HorizontalAlign.Center;
                        tdRow6.Wrap = false;
                        tr.Cells.Add(tdRow6);

                        TableCell tdRow7 = new TableCell();
                        tdRow7.Width = Unit.Percentage(5);
                        tdRow7.Text = ds.Tables[0].Rows[i]["y1Total"].ToString();
                        tdRow7.HorizontalAlign = HorizontalAlign.Center;
                        tdRow7.Wrap = false;
                        tr.Cells.Add(tdRow7);


                        TableCell tdRow7a = new TableCell();
                        tdRow7a.Width = Unit.Percentage(10);
                        tdRow7a.Text = ds.Tables[0].Rows[i]["y2Target"].ToString();
                        tdRow7a.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow7a);


                        TableCell tdRow8 = new TableCell();
                        tdRow8.Width = Unit.Percentage(12);
                        tdRow8.Text = ds.Tables[0].Rows[i]["y2Q1"].ToString();
                        tdRow8.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow8);

                        TableCell tdRow9 = new TableCell();
                        tdRow9.Width = Unit.Percentage(10);
                        tdRow9.Text = ds.Tables[0].Rows[i]["y2Q2"].ToString();
                        tdRow9.HorizontalAlign = HorizontalAlign.Center;
                        tdRow9.Wrap = false;
                        tr.Cells.Add(tdRow9);

                        TableCell tdRow15 = new TableCell();
                        tdRow15.Width = Unit.Percentage(5);
                        tdRow15.Text = ds.Tables[0].Rows[i]["y2Q3"].ToString();
                        tdRow15.HorizontalAlign = HorizontalAlign.Center;
                        tdRow15.Wrap = false;
                        tr.Cells.Add(tdRow15);

                        TableCell tdRow16 = new TableCell();
                        tdRow16.Width = Unit.Percentage(5);
                        tdRow16.Text = ds.Tables[0].Rows[i]["y2Q4"].ToString();
                        tdRow16.HorizontalAlign = HorizontalAlign.Center;
                        tdRow16.Wrap = false;
                        tr.Cells.Add(tdRow16);

                        TableCell tdRow17 = new TableCell();
                        tdRow17.Width = Unit.Percentage(5);
                        tdRow17.Text = ds.Tables[0].Rows[i]["y2Total"].ToString();
                        tdRow17.HorizontalAlign = HorizontalAlign.Center;
                        tdRow17.Wrap = false;
                        tr.Cells.Add(tdRow17);

                        TableCell tdRow18 = new TableCell();
                        tdRow18.Width = Unit.Percentage(5);
                        tdRow18.Text = ds.Tables[0].Rows[i]["percentAchieved"].ToString();
                        tdRow18.HorizontalAlign = HorizontalAlign.Center;
                        tdRow18.Wrap = false;
                        tr.Cells.Add(tdRow18);

                        TableCell tdRow19 = new TableCell();
                        tdRow19.Width = Unit.Percentage(5);
                        tdRow19.Text = ds.Tables[0].Rows[i]["overallPercent"].ToString();
                        tdRow19.HorizontalAlign = HorizontalAlign.Center;
                        tdRow19.Wrap = false;
                        tr.Cells.Add(tdRow19);
                        
                        tbl.Rows.Add(tr);
                        // i++;
                       // lblCount.Visible = true;
                      //  lblCount.Text = "Total Records : " + ds.Tables[0].Rows.Count.ToString();
                    }
                      
                      
                    

                }

                else
                {
                    lblError.Visible = true;
                    lblError.Text = "No Record Found";
                }

                    // LblRptSubHeader.Text = strHead;
                }
            }
        catch (Exception ex)
        {
            // ShowAlert(ex.Message);
        }
        finally
        {
        }
    }
    public DataSet GetData(Int64 centreId)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("repPhysicalProgress", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@pCentreID", SqlDbType.BigInt));
                cmd.Parameters["@pCentreID"].Value = centreId;

                cmd.CommandTimeout = 240;

                
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    

                    sda.Fill(ds);
                }
            }
        }
        return ds;
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

            divpdf.Visible = true;
            tbl.BorderStyle = BorderStyle.Solid;
            tbl.CssClass = "sample3";
            tbl.CellPadding = 2;
            tbl.CellSpacing = 1;
            tbl.Width = Unit.Percentage(200);

            ShowData();
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
            Response.AddHeader("content-disposition", "attachment;filename=PhysicalProgress.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            //  ShowAlert(ex.Message);
        }
    }
    protected void imgXL_Click(object sender, ImageClickEventArgs e)
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

        ShowData();
        divpdf.Controls.Add(tbl);

        divpdf.RenderControl(hw);
        hw.RenderEndTag();
        Response.Clear();
        // This actually makes your HTML output to be downloaded as .xls file
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("Content-Disposition", "attachment;filename=PhysicalProgress.xls");
        Response.ContentType = "application/vnd.ms-excel";
        Response.Write(sw.ToString());
        Response.End();
    }
    protected void imgDOC_Click(object sender, ImageClickEventArgs e)
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

        ShowData();
        divpdf.Controls.Add(tbl);

        divpdf.RenderControl(hw);
        hw.RenderEndTag();
        Response.Clear();
        // This actually makes your HTML output to be downloaded as .xls file
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("Content-Disposition", "attachment;filename=PhysicalProgress.doc");
        Response.ContentType = "application/vnd.ms-word";
        Response.Write(sw.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
}