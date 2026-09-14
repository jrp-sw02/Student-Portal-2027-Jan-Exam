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

public partial class HO_Rpt_RunningProjStatus : System.Web.UI.Page
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/RunningProjStatusFilter.aspx"))
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
            tcCol.Text = "SrNo.";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(8);
            tcCol1.Text = "Project name";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);


            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(8);
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            tcCol2.Text = "Duration";
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(8);
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Text = "Budget Outlay";
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol3a = new TableHeaderCell();
            tcCol3a.Width = Unit.Percentage(8);
            tcCol3a.HorizontalAlign = HorizontalAlign.Center;
            tcCol3a.Text = "Centre Budget Outlay";
            th.Cells.Add(tcCol3a);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(8);
            tcCol4.Text = "Total Target";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(10);
            tcCol5.Text = "Centre Target";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);


            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(5);
            tcCol6.Text = "Target Achieved";
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

            // added DateTime variable on 10-03-2025

            DateTime AsOnDate = Convert.ToDateTime(Request.QueryString["AsOnDate"]);

            TableRow tr = new TableRow();
            TableCell tc=new TableCell() ;
            
            tc.ColumnSpan =8;
            tr.Cells .Add (tc);
             tbl.Rows.Add(tr);
            
            TableRow tr1 = new TableRow();
            TableCell tc1 = new TableCell();

            tc1.ColumnSpan = 8;
            tr1.Cells .Add(tc1);
            tbl.Rows.Add(tr1);

		TableRow tr2 = new TableRow();
            TableCell tc2 = new TableCell();

            tc2.ColumnSpan = 8;
            tc2.Text = "Report of running projects status As On " + AsOnDate.ToString("dd-MMM-yyyy");
            tr2.Cells .Add(tc2);
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
            tcCol1.ColumnSpan = 7;
            tcCol1.Text = centrename ;
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
          
            DateTime AsOnDate = Convert.ToDateTime(Request.QueryString["AsOnDate"]);
            
            StringBuilder mySql = new StringBuilder();

           using (ds= GetData(centreId, AsOnDate))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //ShowTableHeader();
                    string oldCentreName = "x";
                    
                   
                    int i,x;
                    x = 0;
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string centreName = ds.Tables[0].Rows[i]["Centre"].ToString();

                        if(oldCentreName !=centreName )
                        {
                            x = 1;
                            ShowTableHeader2(centreName);
                            ShowTableHeader() ;
                            oldCentreName =centreName;
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
                        tdRow.Text = (x).ToString();
                        tdRow.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow);


                       /* TableCell tdRow1 = new TableCell();
                        tdRow1.Width = Unit.Percentage(10);
                        tdRow1.Text = ds.Tables[0].Rows[i]["Centre"].ToString();
                        tdRow1.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow1);*/

                        TableCell tdRow2 = new TableCell();
                        tdRow2.Width = Unit.Percentage(10);
                        tdRow2.Text = ds.Tables[0].Rows[i]["Project"].ToString();
                        tdRow2.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow2);

                        TableCell tdRow3 = new TableCell();
                        tdRow3.Width = Unit.Percentage(12);
                        tdRow3.Text = ds.Tables[0].Rows[i]["Durationyrs"].ToString();
                        tdRow3.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow3);


                       

                       
                        TableCell tdRow13 = new TableCell();
                        tdRow13.Width = Unit.Percentage(10);
                        tdRow13.Text = ds.Tables[0].Rows[i]["TotalBudget"].ToString();
                        tdRow13.HorizontalAlign = HorizontalAlign.Center;
                        tdRow13.Wrap = false;
                        tr.Cells.Add(tdRow13);

                        TableCell tdRow14 = new TableCell();
                        tdRow14.Width = Unit.Percentage(5);
                        tdRow14.Text = ds.Tables[0].Rows[i]["CentreBudget"].ToString();
                        tdRow14.HorizontalAlign = HorizontalAlign.Center;
                        tdRow14.Wrap = false;
                        tr.Cells.Add(tdRow14);

                        TableCell tdRow6 = new TableCell();
                        tdRow6.Width = Unit.Percentage(5);
                        tdRow6.Text = ds.Tables[0].Rows[i]["TotalTarget"].ToString();
                        tdRow6.HorizontalAlign = HorizontalAlign.Center;
                        tdRow6.Wrap = false;
                        tr.Cells.Add(tdRow6);


                        TableCell tdRow6a = new TableCell();
                        tdRow6a.Width = Unit.Percentage(5);
                        tdRow6a.Text = ds.Tables[0].Rows[i]["CentreTarget"].ToString();
                        tdRow6a.HorizontalAlign = HorizontalAlign.Center;
                        tdRow6a.Wrap = false;
                        tr.Cells.Add(tdRow6a);

                        TableCell tdRow7 = new TableCell();
                        tdRow7.Width = Unit.Percentage(3);
                        tdRow7.Text = ds.Tables[0].Rows[i]["TotalAchieved"].ToString();
                        tdRow7.HorizontalAlign = HorizontalAlign.Center;
                        tdRow7.Wrap = false;
                        tr.Cells.Add(tdRow7);

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
    public DataSet GetData(Int64 centreId, DateTime AsOnDate)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("RepRunningProjStatus", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@pCentreID", SqlDbType.BigInt));
                cmd.Parameters["@pCentreID"].Value = centreId;
                
                cmd.Parameters.Add(new SqlParameter("@pAsOnDate", SqlDbType.Date));
                cmd.Parameters["@pAsOnDate"].Value = AsOnDate;
                   
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
           
            // Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            StringReader sr = new StringReader(sw.ToString());
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=RunningProjectStatus.pdf");
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
        Response.AddHeader("Content-Disposition", "attachment;filename=RunningProjectStatus.xls");
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
        Response.AddHeader("Content-Disposition", "attachment;filename=RunningProjectStatus.doc");
        Response.ContentType = "application/vnd.ms-word";
        Response.Write(sw.ToString());
        Response.End();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //base.VerifyRenderingInServerForm(control);
    }
}