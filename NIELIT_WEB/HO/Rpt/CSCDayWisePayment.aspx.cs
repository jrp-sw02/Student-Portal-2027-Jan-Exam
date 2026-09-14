using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.URM;

public partial class HO_Rpt_CSCDayWisePayment : BasePage
{
    Table tbl = new Table();
    String PayStatusId = "0"; 
    Int32 currentRoleId = 0;
    int PayModeId = 0;


    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Visible = false;
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/CSCDayWisePaymentReport.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                newShowData();
                tbl.CssClass = "sample3";
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(100); ;
                divReportData.Controls.Add(tbl);
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
            

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(2);
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            tcCol1.Text = "#";
            th.Cells.Add(tcCol1);

          

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(10);
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            tcCol.Text = "Payment Date";
            th.Cells.Add(tcCol);
          

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(5);
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            tcCol2.Text = "Demand Note No";
            th.Cells.Add(tcCol2);

           
            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(10);
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            tcCol5.Text = "Total Fee Amount (Rs.)";
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(12);
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            tcCol9.Text = "Payment Status";
            th.Cells.Add(tcCol9);

            tbl.Rows.Add(th);
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
            newShowData();
            divReportData.Controls.Add(tbl);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Day_Wise_Payment_Report.xls");
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
    protected void newShowData()
    {
        EConnectContext context = new EConnectContext(); ;
        try
        {
            int i = 0;
            Decimal totamt = 0;                
            Decimal TotalAmount = 0;           
            PayModeId = Convert.ToInt32(Request.QueryString["PayModeId"]);
            enmPaymentMode paymentMode = (enmPaymentMode)PayModeId;
            PayStatusId = Request.QueryString["PayStatusId"];
            DateTime PayFromDate = Convert.ToDateTime(Request.QueryString["PayFromDate"]);
            DateTime PayToDate = Convert.ToDateTime(Request.QueryString["PayToDate"]);
            string strHead = "";
            string strHead1 = "";
            StringBuilder mySql = new StringBuilder();         

            strHead += "</br><b> Payment Date From :</b>" + PayFromDate.ToString("dd-MMM-yyyy") + " to " + PayToDate.ToString("dd-MMM-yyyy");

            strHead += "<br/> <b>Payment Mode :  </b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.enmPaymentMode)(PayModeId)).ToString();

            if (PayStatusId == "0")
                strHead += "<br/> <b>Payment Status :</b> All";
            else if (PayStatusId.Trim().ToUpper() == "F".ToUpper().Trim())
                strHead += "<br/> <b>Payment Status :</b> Failed";
            else if (PayStatusId.Trim().ToUpper() == "S".ToUpper().Trim())
                strHead += "<br/> <b>Payment Status :</b> Success";

            strHead += "<br/><b>Course Category :</b> All";

            strHead += "<br/><b>Course Name :</b> All";

            if (paymentMode == enmPaymentMode.CSCSPV)
            {
                var CSCPayment = context.CSCTransactions
                    .Where(p => p.Date >= PayFromDate && p.Date <= PayToDate)
                    .Select(p =>p);

                if (PayStatusId != "0")
                {
                    if (PayStatusId == "S")
                        CSCPayment = CSCPayment.Where(p => p.ResponseStatus == 0 || p.ResponseStatus == 100);
                    if (PayStatusId == "F")
                        CSCPayment = CSCPayment.Where(p => p.ResponseStatus == 1 || p.ResponseStatus == null);
                }
              
                if (CSCPayment.Count() > 0)
                {
                    ShowTableHeader();
                    foreach (var item in CSCPayment)
                    {
                        TableRow tr = new TableRow();
                        if (i % 2 == 0)
                            tr.CssClass = "gdalternate1";
                        else
                            tr.CssClass = "gdrow1";
                        i++;
                        TableHeaderCell tcCol1 = new TableHeaderCell();
                        tcCol1.Width = Unit.Percentage(2);
                        tcCol1.HorizontalAlign = HorizontalAlign.Left;
                        tcCol1.Text = i.ToString();
                        tr.Cells.Add(tcCol1);

                        TableCell tcCol = new TableCell();
                        tcCol.Width = Unit.Percentage(5);
                        tcCol.HorizontalAlign = HorizontalAlign.Center;
                        tcCol.Text = item.Date.ToString("dd-MMM-yyyy");
                        tr.Cells.Add(tcCol);

                        TableCell tcCol2 = new TableCell();
                        tcCol2.Width = Unit.Percentage(8);
                        tcCol2.HorizontalAlign = HorizontalAlign.Right;
                        tcCol2.Text =item.DemandNoteID.ToString();                      
                        tr.Cells.Add(tcCol2);


                        TableCell tcCol8 = new TableCell();
                        tcCol8.Width = Unit.Percentage(8);
                        tcCol8.HorizontalAlign = HorizontalAlign.Right;
                        tcCol8.Text = item.Amount.ToString("F");
                        TotalAmount += Convert.ToDecimal(tcCol8.Text);
                        tr.Cells.Add(tcCol8);

                        TableCell tcCol9 = new TableCell();
                        tcCol9.Width = Unit.Percentage(20);
                        tcCol9.HorizontalAlign = HorizontalAlign.Left;
                        tcCol9.Text = (String.IsNullOrEmpty(item.ResponseMessage) ? "Failed" : item.ResponseMessage.ToString());
                        tr.Cells.Add(tcCol9);


                        tbl.Rows.Add(tr);
                        if (item.ResponseMessage == "Success")
                        {
                            totamt += Convert.ToDecimal(item.Amount);
                        }
                    }

                    //showing total
                    TableRow trNew = new TableRow();
                    if (i % 2 == 0)
                        trNew.CssClass = "gdalternate1";
                    else
                        trNew.CssClass = "gdrow1";

                    TableCell tdNewRow1 = new TableCell();
                    tdNewRow1.Width = Unit.Percentage(1);
                    tdNewRow1.Text = "<b>Total</b>";
                    tdNewRow1.ColumnSpan = 3;
                    tdNewRow1.HorizontalAlign = HorizontalAlign.Center;
                    trNew.Cells.Add(tdNewRow1);

                   

                    TableCell tdNewRow12 = new TableCell();
                    tdNewRow12.Width = Unit.Percentage(1);
                    tdNewRow12.Text = "<b>" + TotalAmount.ToString() + "</b>";
                    tdNewRow12.HorizontalAlign = HorizontalAlign.Right;
                    trNew.Cells.Add(tdNewRow12);

                    TableCell tdNewRow13 = new TableCell();
                    tdNewRow13.Width = Unit.Percentage(1);
                    tdNewRow13.Text = "";
                    tdNewRow13.HorizontalAlign = HorizontalAlign.Right;
                    trNew.Cells.Add(tdNewRow13);

                    tbl.Rows.Add(trNew);
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "No Record Found !";
                }
                strHead1 += "<br/><b>Total Amount Of Successful Transactions :</b>" + totamt.ToString("F");
            }
            LblRptSubHeader.Text = strHead;
            LblRptSubHeader1.Text = strHead1;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }
        finally { context.Dispose(); }
    }
}