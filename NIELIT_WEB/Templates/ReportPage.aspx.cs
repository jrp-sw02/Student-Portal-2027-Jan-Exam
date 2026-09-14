using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ReportPgae : BasePage
{
    Table tbl = new Table();
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsSessionAlive())
        //{
        //    Response.Redirect("~/index.aspx");
        //}
        if (!Page.IsPostBack)
        {
            tbl.CssClass = "sample3";
            tbl.CellPadding = 2;
            tbl.CellSpacing = 0;
            tbl.Width = Unit.Percentage(100);
            ShowTableHeader();
            ShowTableData();
            divReportData.Controls.Add(tbl);
        }
    }
    protected void ShowTableHeader()
    {
        TableHeaderRow th = new TableHeaderRow();
        th.CssClass = "head1";

        TableHeaderCell tcCol1 = new TableHeaderCell();
        tcCol1.Width = Unit.Percentage(3);
        tcCol1.Text = "Col1";
        th.Cells.Add(tcCol1);

        TableHeaderCell tcCol2 = new TableHeaderCell();
        tcCol2.Width = Unit.Percentage(10);
        tcCol2.Text = "Col2";
        th.Cells.Add(tcCol2);

        TableHeaderCell tcCol3 = new TableHeaderCell();
        tcCol3.Width = Unit.Percentage(10);
        tcCol3.Text = "Col3";
        th.Cells.Add(tcCol3);

        TableHeaderCell tcCol4 = new TableHeaderCell();
        tcCol4.Width = Unit.Percentage(10);
        tcCol4.Text = "Col4";
        th.Cells.Add(tcCol4);

        TableHeaderCell tcCol5 = new TableHeaderCell();
        tcCol5.Width = Unit.Percentage(10);
        tcCol5.Text = "Col5";
        th.Cells.Add(tcCol5);

        tbl.Rows.Add(th);
        
    }

    protected void ShowTableData()
    {
        TableRow tr = new TableRow();
        tr.CssClass = "gdalternate1";

        TableCell tdRow1 = new TableCell();
        tdRow1.Width = Unit.Percentage(3);
        tdRow1.Text = "Row1";
        tr.Cells.Add(tdRow1);

        TableCell tdRow2 = new TableCell();
        tdRow2.Width = Unit.Percentage(10);
        tdRow2.Text = "Row2";
        tr.Cells.Add(tdRow2);

        TableCell tdRow3 = new TableCell();
        tdRow3.Width = Unit.Percentage(10);
        tdRow3.Text = "Row3";
        tr.Cells.Add(tdRow3);

        TableCell tdRow4 = new TableCell();
        tdRow4.Width = Unit.Percentage(10);
        tdRow4.Text = "Row4";
        tr.Cells.Add(tdRow4);

        TableCell tdRow5 = new TableCell();
        tdRow5.Width = Unit.Percentage(10);
        tdRow5.Text = "Row5";
        tr.Cells.Add(tdRow5);

        tbl.Rows.Add(tr);


        TableRow trAlt = new TableRow();
        trAlt.CssClass = "gdrow1";

        TableCell tdAltRow1 = new TableCell();
        tdAltRow1.Width = Unit.Percentage(3);
        tdAltRow1.Text = "Row1";
        trAlt.Cells.Add(tdAltRow1);

        TableCell tdAltRow2 = new TableCell();
        tdAltRow2.Width = Unit.Percentage(10);
        tdAltRow2.Text = "Row2";
        trAlt.Cells.Add(tdAltRow2);

        TableCell tdAltRow3 = new TableCell();
        tdAltRow3.Width = Unit.Percentage(10);
        tdAltRow3.Text = "Row3";
        trAlt.Cells.Add(tdAltRow3);

        TableCell tdAltRow4 = new TableCell();
        tdAltRow4.Width = Unit.Percentage(10);
        tdAltRow4.Text = "Row4";
        trAlt.Cells.Add(tdAltRow4);

        TableCell tdAltRow5 = new TableCell();
        tdAltRow5.Width = Unit.Percentage(10);
        tdAltRow5.Text = "Row5";
        trAlt.Cells.Add(tdAltRow5);

        tbl.Rows.Add(trAlt);

    }
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {  
        System.IO.StringWriter StringWrite =new System.IO.StringWriter();
        Html32TextWriter htmlWrite;
        divReportData.Visible = true;
        ShowTableHeader();
        ShowTableData();
        divReportData.Controls.Add(tbl);
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=Applications.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.xls";
        htmlWrite = new Html32TextWriter(StringWrite);
        divReportData.RenderControl(htmlWrite);
        Response.Write(StringWrite.ToString());
        Response.End();
    }
}