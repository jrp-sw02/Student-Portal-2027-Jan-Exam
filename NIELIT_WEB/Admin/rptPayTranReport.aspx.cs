using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HO_Rpt_PaymentRecieptReport : BasePage
{
    Table tbl = new Table();
    public static int j;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {


            if (!String.IsNullOrEmpty(Request.QueryString["ctgry"]))
            {
                lblcategory.Text = Request.QueryString["Catg"];
                lblcourse.Text = Request.QueryString["Course"];
                lblfromdate.Text = Request.QueryString["dfrom"];
                lbldateto.Text = Request.QueryString["dto"];
                lblfeetype.Text = Request.QueryString["ftype"];
            }
            tbl.CssClass = "sample3";
            tbl.CellPadding = 2;
            tbl.CellSpacing = 0;
            tbl.Width = Unit.Percentage(100);
            ShowTableHeader();
            j = 1;
            for (int i = 0; i < 10; i++)
            {
                
                ShowTableData();
            }
            divReportData.Controls.Add(tbl);
        }
    }
    protected void ShowTableHeader()
    {
        TableHeaderRow th = new TableHeaderRow();
        th.CssClass = "head1";

        TableHeaderCell tcCol1 = new TableHeaderCell();
        tcCol1.Width = Unit.Percentage(3);
        tcCol1.HorizontalAlign = HorizontalAlign.Left;
        tcCol1.Text = "#";
        th.Cells.Add(tcCol1);

        TableHeaderCell tcCol2 = new TableHeaderCell();
        tcCol2.Width = Unit.Percentage(10);
        tcCol2.HorizontalAlign = HorizontalAlign.Left;
        tcCol2.Text = "Payment Mode";
        th.Cells.Add(tcCol2);

        TableHeaderCell tcCol3 = new TableHeaderCell();
        tcCol3.HorizontalAlign = HorizontalAlign.Left;
        tcCol3.Width = Unit.Percentage(10);
        tcCol3.Text = "Date";
        th.Cells.Add(tcCol3);

        TableHeaderCell tcCol4 = new TableHeaderCell();
        tcCol4.Width = Unit.Percentage(10);
        tcCol4.HorizontalAlign = HorizontalAlign.Left;
        tcCol4.Text = "Student Registration No";
        th.Cells.Add(tcCol4);

        TableHeaderCell tcCol5 = new TableHeaderCell();
        tcCol5.Width = Unit.Percentage(10);
        tcCol5.HorizontalAlign = HorizontalAlign.Right;
        tcCol5.Text = "Amount";
        th.Cells.Add(tcCol5);

        //TableHeaderCell tcCol6 = new TableHeaderCell();
        //tcCol6.Width = Unit.Percentage(10);
        //tcCol6.HorizontalAlign = HorizontalAlign.Right;
        //tcCol6.Text = "Transaction Date";
        //th.Cells.Add(tcCol6);

        //TableHeaderCell tcCol7 = new TableHeaderCell();
        //tcCol7.Width = Unit.Percentage(10);
        //tcCol7.HorizontalAlign = HorizontalAlign.Right;
        //tcCol7.Text = "Amount";
        //th.Cells.Add(tcCol7);

        tbl.Rows.Add(th);
        
    }

    protected void ShowTableData()
    {
        
        TableRow tr = new TableRow();
        tr.CssClass = "gdalternate1";
        tr.HorizontalAlign = HorizontalAlign.Left;
        TableCell tdRow1 = new TableCell();
        tdRow1.Width = Unit.Percentage(3);
        tdRow1.Text = j.ToString();
        tr.Cells.Add(tdRow1);

        TableCell tdRow2 = new TableCell();
        tdRow2.Width = Unit.Percentage(10);
        tdRow2.Text = "Direct";
        tr.Cells.Add(tdRow2);

        TableCell tdRow3 = new TableCell();
        tdRow3.Width = Unit.Percentage(10);
        tdRow3.Text = "Nitesh Garg";
        tr.Cells.Add(tdRow3);

        TableCell tdRow4 = new TableCell();
        tdRow4.Width = Unit.Percentage(10);
        tdRow4.Text = "Online";
        tr.Cells.Add(tdRow4);

        TableCell tdRow5 = new TableCell();
        
        tdRow5.Width = Unit.Percentage(10);
        tdRow5.HorizontalAlign = HorizontalAlign.Right;
        tdRow5.Text = "23244";
        tr.Cells.Add(tdRow5);

        //TableCell tdRow6 = new TableCell();
        //tdRow6.Width = Unit.Percentage(10);
        //tdRow6.HorizontalAlign = HorizontalAlign.Right;
        //tdRow6.Text = "09-Jul-2008";
        //tr.Cells.Add(tdRow6);

        //TableCell tdRow7 = new TableCell();
        //tdRow7.HorizontalAlign = HorizontalAlign.Right;
        //tdRow7.Width = Unit.Percentage(10);
        //tdRow7.Text = "500.00";
        //tr.Cells.Add(tdRow7);
        //tbl.Rows.Add(tr);

        j++;
        TableRow trAlt = new TableRow();
        trAlt.CssClass = "gdrow1";

        TableCell tdAltRow1 = new TableCell();
        //tdAltRow1.HorizontalAlign = HorizontalAlign.Left;
        tdAltRow1.Width = Unit.Percentage(3);
        tdAltRow1.Text = j.ToString();
        trAlt.Cells.Add(tdAltRow1);

        TableCell tdAltRow2 = new TableCell();
        tdAltRow2.Width = Unit.Percentage(10);
        tdAltRow2.Text = "Accredited Centre";
        trAlt.Cells.Add(tdAltRow2);

        TableCell tdAltRow3 = new TableCell();
        tdAltRow3.Width = Unit.Percentage(10);
        tdAltRow3.Text = "Ashwariya College";
        trAlt.Cells.Add(tdAltRow3);

        TableCell tdAltRow4 = new TableCell();
        tdAltRow4.Width = Unit.Percentage(10);
        tdAltRow4.Text = "CSC";
        trAlt.Cells.Add(tdAltRow4);

        TableCell tdAltRow5 = new TableCell();
        tdAltRow5.Width = Unit.Percentage(10);
        tdAltRow5.HorizontalAlign = HorizontalAlign.Right;
        tdAltRow5.Text = "45322";
        trAlt.Cells.Add(tdAltRow5);

        //TableCell tdAltRow6 = new TableCell();
        //tdAltRow6.Width = Unit.Percentage(10);
        //tdAltRow6.HorizontalAlign = HorizontalAlign.Right;
        //tdAltRow6.Text = "10-Mar-2009";
        //trAlt.Cells.Add(tdAltRow6);

        //TableCell tdAltRow7 = new TableCell();
        //tdAltRow7.HorizontalAlign = HorizontalAlign.Right;
        //tdAltRow7.Width = Unit.Percentage(10);
        //tdAltRow7.Text = "500.00";
        //trAlt.Cells.Add(tdAltRow7);
        j++;
        tbl.Rows.Add(trAlt);

    }
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {

    }
}