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

public partial class HO_Rpt_QuarterYearTrained : System.Web.UI.Page
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/QuarterYearTrainedFilter.aspx"))
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
           
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = " ";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);
		
	    TableHeaderCell tcCol1a = new TableHeaderCell();
            tcCol1a.Width = Unit.Percentage(8);
            tcCol1a.Text = "Training Type";
            tcCol1a.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1a);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(8);
            tcCol1.Text = "Formal Courses";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);


            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(8);
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            tcCol2.Text = "Skill Based Long Term >500 hrs";
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(8);
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Text = "Skill based between 91 hrs to 500 hrs";
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol3a = new TableHeaderCell();
            tcCol3a.Width = Unit.Percentage(8);
            tcCol3a.HorizontalAlign = HorizontalAlign.Center;
            tcCol3a.Text = "Skill Based <=90 hrs";
            th.Cells.Add(tcCol3a);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(8);
            tcCol4.Text = "Digital Literacy Courses > 90 hrs";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(10);
            tcCol5.Text = "Digital Literacy courses <=90 hrs";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(10);
            tcCol6.Text = "Total";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);


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

            tc.ColumnSpan = 9;
            tr.Cells.Add(tc);
            tbl.Rows.Add(tr);

            TableRow tr1 = new TableRow();
            TableCell tc1 = new TableCell();

            tc1.ColumnSpan = 9;
            tr1.Cells.Add(tc1);
            tbl.Rows.Add(tr1);

            TableRow tr2 = new TableRow();
            TableCell tc2 = new TableCell();

            tc2.ColumnSpan = 9;
            tc2.Text = "Report of Trained On-Campus/Off-Campus students for batches ending in quarter " + vQuarter.ToString() + " year " + vYear.ToString();
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
            tcCol1.ColumnSpan = 8;
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
                       

            int vQuarter = Convert.ToInt32(Request.QueryString["quarter"]);
            int vYear = Convert.ToInt32(Request.QueryString["year"]);

            StringBuilder mySql = new StringBuilder();


            using (ds= GetBatchData(centreId,vQuarter,vYear))
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
                        TableCell tdRow = new TableCell();
                        tdRow.Width = Unit.Percentage(1);
                        tdRow.Text = (i+1).ToString();
                        tdRow.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow);

			TableCell tdRow1 = new TableCell();
                        tdRow1.Width = Unit.Percentage(10);
                        tdRow1.Text = ds.Tables[0].Rows[i]["trainingType"].ToString();
                        tdRow1.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow1);

                        TableCell tdRow1a = new TableCell();
                        tdRow1a.Width = Unit.Percentage(10);
                        tdRow1a.Text = ds.Tables[0].Rows[i]["Formal"].ToString();
                        tdRow1a.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow1a);

                        TableCell tdRow2 = new TableCell();
                        tdRow2.Width = Unit.Percentage(10);
                        tdRow2.Text = ds.Tables[0].Rows[i]["skillMore500"].ToString();
                        tdRow2.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow2);

                        TableCell tdRow3 = new TableCell();
                        tdRow3.Width = Unit.Percentage(12);
                        tdRow3.Text = ds.Tables[0].Rows[i]["skill91To500"].ToString();
                        tdRow3.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow3);
                        
                        TableCell tdRow13 = new TableCell();
                        tdRow13.Width = Unit.Percentage(10);
                        tdRow13.Text = ds.Tables[0].Rows[i]["skillLess90"].ToString();
                        tdRow13.HorizontalAlign = HorizontalAlign.Center;
                        tdRow13.Wrap = false;
                        tr.Cells.Add(tdRow13);

                        TableCell tdRow14 = new TableCell();
                        tdRow14.Width = Unit.Percentage(5);
                        tdRow14.Text = ds.Tables[0].Rows[i]["DLCMore90"].ToString();
                        tdRow14.HorizontalAlign = HorizontalAlign.Center;
                        tdRow14.Wrap = false;
                        tr.Cells.Add(tdRow14);

                        TableCell tdRow6 = new TableCell();
                        tdRow6.Width = Unit.Percentage(5);
                        tdRow6.Text = ds.Tables[0].Rows[i]["DLCLess90"].ToString();
                        tdRow6.HorizontalAlign = HorizontalAlign.Center;
                        tdRow6.Wrap = false;
                        tr.Cells.Add(tdRow6);

                        TableCell tdRow6a = new TableCell();
                        tdRow6a.Width = Unit.Percentage(5);
                        tdRow6a.Text = ds.Tables[0].Rows[i]["total"].ToString();
                        tdRow6a.HorizontalAlign = HorizontalAlign.Center;
                        tdRow6a.Wrap = false;
                        tr.Cells.Add(tdRow6a);
                        
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
    public DataSet GetBatchData(Int64 centreId,  int vQuarter, int vYear)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("repQuarterYearOnCampusTrained", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@pCentreID", SqlDbType.BigInt));
                cmd.Parameters["@pCentreID"].Value = centreId;


                cmd.Parameters.Add(new SqlParameter("@pQuarter", SqlDbType.Int));
                cmd.Parameters["@pQuarter"].Value = vQuarter ;
                cmd.Parameters.Add(new SqlParameter("@pYear", SqlDbType.Int));
                cmd.Parameters["@pYear"].Value = vYear;
                
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
            Response.AddHeader("content-disposition", "attachment;filename=QuarterYearTrained.pdf");
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
        Response.AddHeader("Content-Disposition", "attachment;filename=QuarterYearTrained.xls");
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
        Response.AddHeader("Content-Disposition", "attachment;filename=QuarterYearTrained.doc");
        Response.ContentType = "application/vnd.ms-word";
        Response.Write(sw.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
}