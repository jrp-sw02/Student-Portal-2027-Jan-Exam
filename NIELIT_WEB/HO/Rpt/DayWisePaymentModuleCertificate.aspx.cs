using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data.Objects;

public partial class HO_Rpt_DayWisePaymentModuleCertificate : BasePage
{
    Table tbl = new Table();   
    String PayStatusId = "0";
    String DateType = "0";
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/DayWisePaymentReport.aspx"))
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
    protected void ShowTableHeader()
    {
        try
        {
            int AppTypeId = Convert.ToInt32(Request.QueryString["AppTypeId"]);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(2);
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            tcCol1.Text = "#";
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol15 = new TableHeaderCell();
            tcCol15.Width = Unit.Percentage(5);
            tcCol15.HorizontalAlign = HorizontalAlign.Center;
            tcCol15.Text = "Course";
            th.Cells.Add(tcCol15);

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(10);
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            tcCol.Text = "Payment Date";
            th.Cells.Add(tcCol);

            if (PayModeId == Convert.ToInt32(enmPaymentMode.Online))
            {
                TableHeaderCell tcCol16 = new TableHeaderCell();
                tcCol16.Width = Unit.Percentage(12);
                tcCol16.HorizontalAlign = HorizontalAlign.Center;
                tcCol16.Text = "Settled Date";
                th.Cells.Add(tcCol16);
            }

            if (PayModeId == Convert.ToInt32(enmPaymentMode.NEFTRTGS))
            {
                TableHeaderCell tcCol16 = new TableHeaderCell();
                tcCol16.Width = Unit.Percentage(12);
                tcCol16.HorizontalAlign = HorizontalAlign.Center;
                tcCol16.Text = "Verified Date";
                th.Cells.Add(tcCol16);
            }

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(5);
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            tcCol2.Text = "No of Candidates";
            th.Cells.Add(tcCol2);


            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(10);
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            tcCol5.Text = "Total Fee Amount (Rs.)";
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(6); // change it to 6
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            tcCol9.Text = "Payment Status";
            th.Cells.Add(tcCol9);



            // added by amit start
            TableHeaderCell tcColn1 = new TableHeaderCell();
            tcColn1.Width = Unit.Percentage(6);
            tcColn1.HorizontalAlign = HorizontalAlign.Center;
            tcColn1.Text = "Payment Gateway Used";
            th.Cells.Add(tcColn1);
            // added by amit end


            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void newShowData()
    {
        EConnectContext context = new EConnectContext();
        try
        {
            int i = 0;
            Decimal totamt = 0;
            Int64 noofCandidates = 0;
            Decimal TotalAmount = 0;
            PayModeId = Convert.ToInt32(Request.QueryString["PayModeId"]);
            enmPaymentMode paymentMode = (enmPaymentMode)PayModeId;
            int TransTypeId = Convert.ToInt32(Request.QueryString["TransTypeId"]);
            int CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            int AppTypeId = Convert.ToInt32(Request.QueryString["AppTypeId"]);
            enmApplicationType applicationType = (enmApplicationType)AppTypeId;
            PayStatusId = Request.QueryString["PayStatusId"];
            DateType = Request.QueryString["Datetype"];
            DateTime PayFromDate = Convert.ToDateTime(Request.QueryString["PayFromDate"]);
            DateTime PayToDate = Convert.ToDateTime(Request.QueryString["PayToDate"]);
            string strHead = ""; string strHead1 = "";
            // added by amit start
            String gateway = Convert.ToString(Request.QueryString["Gateway"]);
            // added by amit end
            StringBuilder mySql = new StringBuilder();

            if (AppTypeId == 4)
            {

                if (DateType != "0")
                {
                    if (DateType == "SV")
                    {
                        if (PayModeId == Convert.ToInt32(enmPaymentMode.Online))
                            strHead += "</br><b> Settled Date From :</b>" + PayFromDate.ToString("dd-MMM-yyyy") + " to " + PayToDate.ToString("dd-MMM-yyyy");
                        else if (PayModeId == Convert.ToInt32(enmPaymentMode.NEFTRTGS))
                            strHead += "</br><b> Verified Date From :</b>" + PayFromDate.ToString("dd-MMM-yyyy") + " to " + PayToDate.ToString("dd-MMM-yyyy");
                    }
                    else
                        strHead += "</br><b> Payment Date From :</b>" + PayFromDate.ToString("dd-MMM-yyyy") + " to " + PayToDate.ToString("dd-MMM-yyyy");
                }
                else
                {
                    strHead += "</br><b> Payment Date From :</b>" + PayFromDate.ToString("dd-MMM-yyyy") + " to " + PayToDate.ToString("dd-MMM-yyyy");
                }
                strHead += "<br/> <b>Payment Mode :  </b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.enmPaymentMode)(PayModeId)).ToString();
                strHead += "<br/> <b>Application Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicationType)(AppTypeId)).ToString();
                if (PayStatusId == "0")
                    strHead += "<br/> <b>Payment Status :</b> All";
                else if (PayStatusId.Trim().ToUpper() == "F".ToUpper().Trim())
                    strHead += "<br/> <b>Payment Status :</b> Failed";
                else if (PayStatusId.Trim().ToUpper() == "S".ToUpper().Trim())
                    strHead += "<br/> <b>Payment Status :</b> Success";
                if (CourseId != 0)
                {
                    var courses = context.Courses.Find(CourseId);
                    if (courses != null)
                    {
                        strHead += "<br/><b>Course Name :</b>" + courses.Name;
                    }
                }
                else
                {
                    strHead += "<br/><b>Course Name :</b> All";
                }
                if (TransTypeId == 0)
                    strHead += "<br/><b>Transaction Type :</b>  All";
                else if (TransTypeId == 1)
                    strHead += "<br/><b>Transaction Type :</b> Single";
                else if (TransTypeId == 2)
                    strHead += "<br/><b>Transaction Type :</b> Multiple";


                #region ONLINE

                /* added by amit start */
                if (gateway == "1")
                    strHead += "<br/><b>Gateway Used : </b>BillDesk ";
                else if (gateway == "2")
                    strHead += "<br/><b>Gateway Used : </b>ICIC ";
                else
                    strHead += "<br/><b>Gateway Used : </b>ALL";
                /* added by amit end */


                if (paymentMode == enmPaymentMode.Online)
                {
                    var Transaction = from o in context.OnlineTransaction
                                      join d in context.DemandNotes on o.DemandNoteID equals d.ID
                                      join m in context.ModuleCertificateRequests on d.ID equals m.DemandNoteID
                                      join pg in context.Paymentgateways on o.PGCode equals pg.ID.ToString() into pgjoin // added by amit 
                                      from pg in pgjoin.DefaultIfEmpty()

                                      where d.ApplicationTypeID == 4 && m.CourseID == CourseId
                                      group new { o, d, m, pg }
                                      by new
                                      {
                                          TransactionDate = System.Data.Entity.DbFunctions.TruncateTime(o.RequestDate),
                                          SettledDate = System.Data.Entity.DbFunctions.TruncateTime(o.SettledOn),
                                          o.PGCode,
                                          o.ResponseStatusMessage,
                                          o.ResponseStatusCode,
                                          m.Course.Code,
                                          PaymentGateway = pg == null ? "EFT" : pg.Description // added by amit
                                      } into gp
                                      select new
                                      {
                                          TransactionDate = gp.Key.TransactionDate,
                                          CourseCode = gp.Key.Code,
                                          SettledDate = gp.Key.SettledDate, 
                                          PGCode = gp.Key.PGCode, //added by amit
                                          Paymentgateway = gp.Key.PaymentGateway, //added by amit
                                          FeeAmount = gp.Sum(m => m.d.Amount),
                                          StatusMessage = string.IsNullOrEmpty(gp.Key.ResponseStatusMessage) ? "Failed" : gp.Key.ResponseStatusMessage,
                                          StatusCode = gp.Key.ResponseStatusCode,
                                          Count = gp.Count()
                                      };

                    // gateway based filtering
                    // amit start
                    if (gateway == "2")
                        Transaction = Transaction.Where(s => s.Paymentgateway == "2");
                    else if (gateway == "1")
                        Transaction = Transaction.Where(s => s.Paymentgateway == "1" || s.Paymentgateway == null); // Billdesk 
                    // amit end


                    if (PayStatusId != "0")
                    {
                        if (PayStatusId == "S")
                        { Transaction = Transaction.Where(s => s.StatusCode == "0300" || s.StatusCode == "E000"); } // added s.StatusCode == "E000"
                        if (PayStatusId == "F")
                        { Transaction = Transaction.Where(s => s.StatusCode != "0300" || s.StatusCode == null); }
                    }
                    if (DateType != "0")
                    {
                        if (DateType == "P")
                        {
                            Transaction = Transaction.Where(o => o.TransactionDate >= PayFromDate && o.TransactionDate <= PayToDate);
                        }
                        else if (DateType == "SV")
                        {
                            if (gateway == "2")
                                Transaction = Transaction.Where(o => o.TransactionDate >= PayFromDate && o.TransactionDate <= PayToDate);
                            else if (gateway == "1")
                                Transaction = Transaction.Where(o => o.SettledDate >= PayFromDate && o.SettledDate <= PayToDate);
                            else
                                Transaction = Transaction.Where(o => (o.PGCode == "2" && o.TransactionDate >= PayFromDate && o.TransactionDate <= PayToDate) || ((o.PGCode == "1" || o.PGCode == null) && o.SettledDate >= PayFromDate && o.SettledDate <= PayToDate ));
                        }
                    }



                    Transaction = Transaction.OrderBy(s => s.TransactionDate);
                    if (Transaction.Count() > 0)
                    {
                        ShowTableHeader();
                        foreach (var item in Transaction)
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

                            TableCell tcCol25 = new TableCell();
                            tcCol25.Width = Unit.Percentage(5);
                            tcCol25.HorizontalAlign = HorizontalAlign.Center;
                            tcCol25.Text = item.CourseCode;
                            tr.Cells.Add(tcCol25);


                            TableCell tcCol = new TableCell();
                            tcCol.Width = Unit.Percentage(8);
                            tcCol.HorizontalAlign = HorizontalAlign.Center;
                            tcCol.Text = item.TransactionDate.Value.ToString("dd-MMM-yyyy");
                            tr.Cells.Add(tcCol);

                            TableCell tcCol16 = new TableCell();
                            tcCol16.Width = Unit.Percentage(8);
                            tcCol16.HorizontalAlign = HorizontalAlign.Center;
                            tcCol16.Text = String.IsNullOrEmpty(item.SettledDate.ToString()) ? "NA" : item.SettledDate.Value.ToString("dd-MMM-yyyy");
                            tr.Cells.Add(tcCol16);

                            TableCell tcCol2 = new TableCell();
                            tcCol2.Width = Unit.Percentage(8);
                            tcCol2.HorizontalAlign = HorizontalAlign.Right;
                            tcCol2.Text = item.Count.ToString();
                            noofCandidates += Convert.ToInt64(item.Count);
                            tr.Cells.Add(tcCol2);

                            TableCell tcCol8 = new TableCell();
                            tcCol8.Width = Unit.Percentage(4); // made half by amit
                            tcCol8.HorizontalAlign = HorizontalAlign.Right;
                            tcCol8.Text = item.FeeAmount.ToString("F");
                            TotalAmount += Convert.ToDecimal(tcCol8.Text);
                            tr.Cells.Add(tcCol8);

                            // added by amit start

                            TableCell tdRow30 = new TableCell();
                            tdRow30.Width = Unit.Percentage(4); 
                            if (!String.IsNullOrEmpty(item.Paymentgateway.ToString()))
                                tdRow30.Text = item.Paymentgateway.ToString();
                            else
                                tdRow30.Text = "NA";
                            tr.Cells.Add(tdRow30);

                            // added by amit end


                            TableCell tcCol9 = new TableCell();
                            tcCol9.Width = Unit.Percentage(20);
                            tcCol9.HorizontalAlign = HorizontalAlign.Left;
                            tcCol9.Text = item.StatusMessage;
                            tr.Cells.Add(tcCol9);




                            tbl.Rows.Add(tr);

                            if (item.StatusMessage.ToUpper().Trim() == "Success".ToUpper().Trim())
                            {
                                totamt += Convert.ToDecimal(item.FeeAmount);
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
                        tdNewRow1.ColumnSpan = 4;
                        tdNewRow1.HorizontalAlign = HorizontalAlign.Center;
                        trNew.Cells.Add(tdNewRow1);

                        TableCell tdNewRow5 = new TableCell();
                        tdNewRow5.Width = Unit.Percentage(1);
                        tdNewRow5.Text = "<b>" + noofCandidates.ToString() + "</b>";
                        tdNewRow5.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow5);

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

                        //added by amit start
                        TableCell tdNewRown1 = new TableCell();
                        tdNewRown1.Width = Unit.Percentage(1);
                        tdNewRown1.Text = "";
                        tdNewRown1.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRown1);
                        //added by amit end

                        tbl.Rows.Add(trNew);
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found !";
                    }
                    strHead1 += "<br/><b>Total Amount Of Successful Transactions :</b>" + totamt.ToString("F");
                }
                #endregion

                #region NEFT / RTGS
                else if (paymentMode == enmPaymentMode.NEFTRTGS)
                {
                    if (DateType == "P")
                    {
                        strHead += "<br/><b> Date Type :</b> Payment Date";
                    }
                    if (DateType == "SV")
                    {
                        strHead += "<br/><b> Date Type :</b> Verified Date";
                    }

                    var Transaction = from o in context.NEFTTransactions
                                      join d in context.DemandNotes on o.DemandNoteID equals d.ID
                                      join m in context.ModuleCertificateRequests on d.ID equals m.DemandNoteID
                                      where d.ApplicationTypeID == 4 && m.CourseID == CourseId
                                      group new { o, d, m }
                                      by new
                                      {
                                          TransactionDate = System.Data.Entity.DbFunctions.TruncateTime(o.TransactionDate),
                                          SettledDate = System.Data.Entity.DbFunctions.TruncateTime(o.Date),
                                          d.PaymentStatusID,
                                          m.Course.Code
                                      } into gp
                                      select new
                                      {
                                          TransactionDate = gp.Key.TransactionDate,
                                          CourseCode = gp.Key.Code,
                                          SettledDate = gp.Key.SettledDate,
                                          FeeAmount = gp.Sum(m => m.d.Amount),
                                          StatusMessage = gp.Key.PaymentStatusID,
                                          Count = gp.Count()
                                      };
                    if (PayStatusId != "0")
                    {
                        if (PayStatusId == "S")
                        { Transaction = Transaction.Where(s => s.StatusMessage == Convert.ToInt32(enmPaymentStatus.Paid)); }
                        if (PayStatusId == "F")
                        { Transaction = Transaction.Where(s => s.StatusMessage == Convert.ToInt32(enmPaymentStatus.Failed)); }
                    }
                    if (DateType != "0")
                    {
                        if (DateType == "P")
                        {
                            Transaction = Transaction.Where(o => o.TransactionDate >= PayFromDate && o.TransactionDate <= PayToDate);
                        }
                        else if (DateType == "SV")
                        {
                            Transaction = Transaction.Where(o => o.SettledDate >= PayFromDate && o.SettledDate <= PayToDate);
                        }
                    }
                    Transaction = Transaction.OrderBy(s => s.SettledDate);
                    if (Transaction.Count() > 0)
                    {
                        ShowTableHeader();
                        foreach (var item in Transaction)
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

                            TableCell tcCol25 = new TableCell();
                            tcCol25.Width = Unit.Percentage(5);
                            tcCol25.HorizontalAlign = HorizontalAlign.Center;
                            tcCol25.Text = item.CourseCode;
                            tr.Cells.Add(tcCol25);


                            TableCell tcCol = new TableCell();
                            tcCol.Width = Unit.Percentage(8);
                            tcCol.HorizontalAlign = HorizontalAlign.Center;
                            tcCol.Text = item.TransactionDate.Value.ToString("dd-MMM-yyyy");
                            tr.Cells.Add(tcCol);

                            TableCell tcCol16 = new TableCell();
                            tcCol16.Width = Unit.Percentage(8);
                            tcCol16.HorizontalAlign = HorizontalAlign.Center;
                            tcCol16.Text = String.IsNullOrEmpty(item.SettledDate.ToString()) ? "NA" : item.SettledDate.Value.ToString("dd-MMM-yyyy");
                            tr.Cells.Add(tcCol16);

                            TableCell tcCol2 = new TableCell();
                            tcCol2.Width = Unit.Percentage(8);
                            tcCol2.HorizontalAlign = HorizontalAlign.Right;
                            tcCol2.Text = item.Count.ToString();
                            noofCandidates += Convert.ToInt64(item.Count);
                            tr.Cells.Add(tcCol2);

                            TableCell tcCol8 = new TableCell();
                            tcCol8.Width = Unit.Percentage(8);
                            tcCol8.HorizontalAlign = HorizontalAlign.Right;
                            tcCol8.Text = Convert.ToInt64(item.FeeAmount).ToString("F");
                            TotalAmount += Convert.ToDecimal(tcCol8.Text);
                            tr.Cells.Add(tcCol8);


                            TableCell tcCol9 = new TableCell();
                            tcCol9.Width = Unit.Percentage(20);
                            tcCol9.HorizontalAlign = HorizontalAlign.Left;
                            tcCol9.Text = (String.IsNullOrEmpty(item.StatusMessage.ToString()) ? "Failed" : EConnect.Utils.Common.EnumUtility.GetDescription((enmPaymentStatus)(Convert.ToInt32(item.StatusMessage))).ToString());
                            tr.Cells.Add(tcCol9);

                            tbl.Rows.Add(tr);
                            totamt += Convert.ToDecimal(item.FeeAmount);

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
                        tdNewRow1.ColumnSpan = 4;
                        tdNewRow1.HorizontalAlign = HorizontalAlign.Center;
                        trNew.Cells.Add(tdNewRow1);

                        TableCell tdNewRow5 = new TableCell();
                        tdNewRow5.Width = Unit.Percentage(1);
                        tdNewRow5.Text = "<b>" + noofCandidates.ToString() + "</b>";
                        tdNewRow5.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow5);

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

                #endregion
            }

            if (AppTypeId == 5)
            {
                if (DateType != "0")
                {
                    if (DateType == "SV")
                    {
                        if (PayModeId == Convert.ToInt32(enmPaymentMode.Online))
                            strHead += "</br><b> Settled Date From :</b>" + PayFromDate.ToString("dd-MMM-yyyy") + " to " + PayToDate.ToString("dd-MMM-yyyy");
                        else if (PayModeId == Convert.ToInt32(enmPaymentMode.NEFTRTGS))
                            strHead += "</br><b> Verified Date From :</b>" + PayFromDate.ToString("dd-MMM-yyyy") + " to " + PayToDate.ToString("dd-MMM-yyyy");
                    }
                    else
                        strHead += "</br><b> Payment Date From :</b>" + PayFromDate.ToString("dd-MMM-yyyy") + " to " + PayToDate.ToString("dd-MMM-yyyy");
                }
                else
                {
                    strHead += "</br><b> Payment Date From :</b>" + PayFromDate.ToString("dd-MMM-yyyy") + " to " + PayToDate.ToString("dd-MMM-yyyy");
                }
                strHead += "<br/> <b>Payment Mode :  </b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.enmPaymentMode)(PayModeId)).ToString();
                strHead += "<br/> <b>Application Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicationType)(AppTypeId)).ToString();
                if (PayStatusId == "0")
                    strHead += "<br/> <b>Payment Status :</b> All";
                else if (PayStatusId.Trim().ToUpper() == "F".ToUpper().Trim())
                    strHead += "<br/> <b>Payment Status :</b> Failed";
                else if (PayStatusId.Trim().ToUpper() == "S".ToUpper().Trim())
                    strHead += "<br/> <b>Payment Status :</b> Success";
                if (CourseId != 0)
                {
                    var courses = context.Courses.Find(CourseId);
                    if (courses != null)
                    {
                        strHead += "<br/><b>Course Name :</b>" + courses.Name;
                    }
                }
                else
                {
                    strHead += "<br/><b>Course Name :</b> All";
                }
                if (TransTypeId == 0)
                    strHead += "<br/><b>Transaction Type :</b>  All";
                else if (TransTypeId == 1)
                    strHead += "<br/><b>Transaction Type :</b> Single";
                else if (TransTypeId == 2)
                    strHead += "<br/><b>Transaction Type :</b> Multiple";


                #region ONLINE
                    
                /* added by amit start */
                if (gateway == "1")
                    strHead += "<br/><b>Gateway Used : </b>BillDesk ";
                else if (gateway == "2")
                    strHead += "<br/><b>Gateway Used : </b>ICIC ";
                else
                    strHead += "<br/><b>Gateway Used : </b>ALL";
                /* added by amit end */	


                if (paymentMode == enmPaymentMode.Online)
                {
                    var Transaction = from o in context.OnlineTransaction
                                      join d in context.DemandNotes on o.DemandNoteID equals d.ID
                                      join m in context.CourseProjectApplications on d.ID equals m.DemandNoteID
                                      where d.ApplicationTypeID == 5 && m.CourseID == CourseId
                                      join pg in context.Paymentgateways on o.PGCode equals pg.ID.ToString() into pgjoin // added by amit 
                                      from pg in pgjoin.DefaultIfEmpty()
                                      group new { o, d, m, pg }
                                      by new
                                      {
                                          TransactionDate = System.Data.Entity.DbFunctions.TruncateTime(o.RequestDate),
                                          SettledDate = System.Data.Entity.DbFunctions.TruncateTime(o.SettledOn),
                                          o.PGCode, // added by amit
                                          o.ResponseStatusMessage,
                                          o.ResponseStatusCode,
                                          m.Course.Code,
                                          PaymentGateway = pg == null ? "EFT" : pg.Description // added by amit
                                      } into gp
                                      select new
                                      {
                                          TransactionDate = gp.Key.TransactionDate,
                                          CourseCode = gp.Key.Code,
                                          SettledDate = gp.Key.SettledDate,
                                          PGCode = gp.Key.PGCode, //added by amit
                                          Paymentgateway = gp.Key.PaymentGateway, //added by amit
                                          FeeAmount = gp.Sum(m => m.d.Amount),
                                          StatusMessage = string.IsNullOrEmpty(gp.Key.ResponseStatusMessage) ? "Failed" : gp.Key.ResponseStatusMessage,
                                          StatusCode = gp.Key.ResponseStatusCode,
                                          Count = gp.Count()
                                      };


                    // amit start
                    if (gateway == "2")
                        Transaction = Transaction.Where(s => s.Paymentgateway == "2");
                    else if (gateway == "1")
                        Transaction = Transaction.Where(s => s.Paymentgateway == "1" || s.Paymentgateway == null); // Billdesk 
                    // amit end


                    if (PayStatusId != "0")
                    {
                        if (PayStatusId == "S")
                        { Transaction = Transaction.Where(s => s.StatusCode == "0300" || s.StatusCode=="E000"); } // added by amit
                        if (PayStatusId == "F")
                        { Transaction = Transaction.Where(s => s.StatusCode != "0300" || s.StatusCode == null); }
                    }
                    if (DateType != "0")
                    {
                        if (DateType == "P")
                        {
                            Transaction = Transaction.Where(o => o.TransactionDate >= PayFromDate && o.TransactionDate <= PayToDate);
                        }
                        else if (DateType == "SV")
                        {
                            if (gateway == "2")
                                Transaction = Transaction.Where(o => o.TransactionDate >= PayFromDate && o.TransactionDate <= PayToDate);
                            else if (gateway == "1")
                                Transaction = Transaction.Where(o => o.SettledDate >= PayFromDate && o.SettledDate <= PayToDate);
                            else
                                Transaction = Transaction.Where(o => (o.PGCode == "2" && o.TransactionDate >= PayFromDate && o.TransactionDate <= PayToDate) || ((o.PGCode == "1" || o.PGCode == null) && o.SettledDate >= PayFromDate && o.SettledDate <= PayToDate));
                        }
                        //else if (DateType == "SV")
                        //{
                        //    Transaction = Transaction.Where(o => o.SettledDate >= PayFromDate && o.SettledDate <= PayToDate);
                        //}
                    }
                    Transaction = Transaction.OrderBy(s => s.TransactionDate);
                    if (Transaction.Count() > 0)
                    {
                        ShowTableHeader();
                        foreach (var item in Transaction)
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

                            TableCell tcCol25 = new TableCell();
                            tcCol25.Width = Unit.Percentage(5);
                            tcCol25.HorizontalAlign = HorizontalAlign.Center;
                            tcCol25.Text = item.CourseCode;
                            tr.Cells.Add(tcCol25);


                            TableCell tcCol = new TableCell();
                            tcCol.Width = Unit.Percentage(8);
                            tcCol.HorizontalAlign = HorizontalAlign.Center;
                            tcCol.Text = item.TransactionDate.Value.ToString("dd-MMM-yyyy");
                            tr.Cells.Add(tcCol);

                            TableCell tcCol16 = new TableCell();
                            tcCol16.Width = Unit.Percentage(8);
                            tcCol16.HorizontalAlign = HorizontalAlign.Center;
                            tcCol16.Text = String.IsNullOrEmpty(item.SettledDate.ToString()) ? "NA" : item.SettledDate.Value.ToString("dd-MMM-yyyy");
                            tr.Cells.Add(tcCol16);

                            TableCell tcCol2 = new TableCell();
                            tcCol2.Width = Unit.Percentage(8);
                            tcCol2.HorizontalAlign = HorizontalAlign.Right;
                            tcCol2.Text = item.Count.ToString();
                            noofCandidates += Convert.ToInt64(item.Count);
                            tr.Cells.Add(tcCol2);

                            TableCell tcCol8 = new TableCell();
                            tcCol8.Width = Unit.Percentage(4);
                            tcCol8.HorizontalAlign = HorizontalAlign.Right;
                            tcCol8.Text = item.FeeAmount.ToString("F");
                            TotalAmount += Convert.ToDecimal(tcCol8.Text);
                            tr.Cells.Add(tcCol8);

                            TableCell tcCol9 = new TableCell();
                            tcCol9.Width = Unit.Percentage(20);
                            tcCol9.HorizontalAlign = HorizontalAlign.Left;
                            tcCol9.Text = item.StatusMessage;
                            tr.Cells.Add(tcCol9);

                            // added by amit start
                            TableCell tdRow30 = new TableCell();
                            tdRow30.Width = Unit.Percentage(4);
                            if (!String.IsNullOrEmpty(item.Paymentgateway.ToString()))
                                tdRow30.Text = item.Paymentgateway.ToString();
                            else
                                tdRow30.Text = "NA";
                            tr.Cells.Add(tdRow30);
                            // added by amit end

                            tbl.Rows.Add(tr);

                            if (item.StatusMessage.ToUpper().Trim() == "Success".ToUpper().Trim())
                            {
                                totamt += Convert.ToDecimal(item.FeeAmount);
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
                        tdNewRow1.ColumnSpan = 4;
                        tdNewRow1.HorizontalAlign = HorizontalAlign.Center;
                        trNew.Cells.Add(tdNewRow1);

                        TableCell tdNewRow5 = new TableCell();
                        tdNewRow5.Width = Unit.Percentage(1);
                        tdNewRow5.Text = "<b>" + noofCandidates.ToString() + "</b>";
                        tdNewRow5.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow5);

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

                        TableCell tdNewRow14 = new TableCell();
                        tdNewRow14.Width = Unit.Percentage(1);
                        tdNewRow14.Text = "";
                        tdNewRow14.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow14);


                        tbl.Rows.Add(trNew);
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found !";
                    }
                    strHead1 += "<br/><b>Total Amount Of Successful Transactions :</b>" + totamt.ToString("F");
                }
                #endregion

                #region NEFT / RTGS
                else if (paymentMode == enmPaymentMode.NEFTRTGS)
                {
                    if (DateType == "P")
                    {
                        strHead += "<br/><b> Date Type :</b> Payment Date";
                    }
                    if (DateType == "SV")
                    {
                        strHead += "<br/><b> Date Type :</b> Verified Date";
                    }

                    var Transaction = from o in context.NEFTTransactions
                                      join d in context.DemandNotes on o.DemandNoteID equals d.ID
                                      join m in context.CourseProjectApplications on d.ID equals m.DemandNoteID
                                      where d.ApplicationTypeID == 5 && m.CourseID == CourseId
                                      group new { o, d, m }
                                      by new
                                      {
                                          TransactionDate = System.Data.Entity.DbFunctions.TruncateTime(o.TransactionDate),
                                          SettledDate = System.Data.Entity.DbFunctions.TruncateTime(o.Date),
                                          d.PaymentStatusID,
                                          m.Course.Code
                                      } into gp
                                      select new
                                      {
                                          TransactionDate = gp.Key.TransactionDate,
                                          CourseCode = gp.Key.Code,
                                          SettledDate = gp.Key.SettledDate,
                                          FeeAmount = gp.Sum(m => m.d.Amount),
                                          StatusMessage = gp.Key.PaymentStatusID,
                                          Count = gp.Count()
                                      };
                    if (PayStatusId != "0")
                    {
                        if (PayStatusId == "S")
                        { Transaction = Transaction.Where(s => s.StatusMessage == Convert.ToInt32(enmPaymentStatus.Paid)); }
                        if (PayStatusId == "F")
                        { Transaction = Transaction.Where(s => s.StatusMessage == Convert.ToInt32(enmPaymentStatus.Failed)); }
                    }
                    if (DateType != "0")
                    {
                        if (DateType == "P")
                        {
                            Transaction = Transaction.Where(o => o.TransactionDate >= PayFromDate && o.TransactionDate <= PayToDate);
                        }
                        else if (DateType == "SV")
                        {
                            Transaction = Transaction.Where(o => o.SettledDate >= PayFromDate && o.SettledDate <= PayToDate);
                        }
                    }
                    Transaction = Transaction.OrderBy(s => s.SettledDate);
                    if (Transaction.Count() > 0)
                    {
                        ShowTableHeader();
                        foreach (var item in Transaction)
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

                            TableCell tcCol25 = new TableCell();
                            tcCol25.Width = Unit.Percentage(5);
                            tcCol25.HorizontalAlign = HorizontalAlign.Center;
                            tcCol25.Text = item.CourseCode;
                            tr.Cells.Add(tcCol25);


                            TableCell tcCol = new TableCell();
                            tcCol.Width = Unit.Percentage(8);
                            tcCol.HorizontalAlign = HorizontalAlign.Center;
                            tcCol.Text = item.TransactionDate.Value.ToString("dd-MMM-yyyy");
                            tr.Cells.Add(tcCol);

                            TableCell tcCol16 = new TableCell();
                            tcCol16.Width = Unit.Percentage(8);
                            tcCol16.HorizontalAlign = HorizontalAlign.Center;
                            tcCol16.Text = String.IsNullOrEmpty(item.SettledDate.ToString()) ? "NA" : item.SettledDate.Value.ToString("dd-MMM-yyyy");
                            tr.Cells.Add(tcCol16);

                            TableCell tcCol2 = new TableCell();
                            tcCol2.Width = Unit.Percentage(8);
                            tcCol2.HorizontalAlign = HorizontalAlign.Right;
                            tcCol2.Text = item.Count.ToString();
                            noofCandidates += Convert.ToInt64(item.Count);
                            tr.Cells.Add(tcCol2);

                            TableCell tcCol8 = new TableCell();
                            tcCol8.Width = Unit.Percentage(8);
                            tcCol8.HorizontalAlign = HorizontalAlign.Right;
                            tcCol8.Text = Convert.ToInt64(item.FeeAmount).ToString("F");
                            TotalAmount += Convert.ToDecimal(tcCol8.Text);
                            tr.Cells.Add(tcCol8);

                            TableCell tcCol9 = new TableCell();
                            tcCol9.Width = Unit.Percentage(20);
                            tcCol9.HorizontalAlign = HorizontalAlign.Left;
                            tcCol9.Text = (String.IsNullOrEmpty(item.StatusMessage.ToString()) ? "Failed" : EConnect.Utils.Common.EnumUtility.GetDescription((enmPaymentStatus)(Convert.ToInt32(item.StatusMessage))).ToString());
                            tr.Cells.Add(tcCol9);

                            tbl.Rows.Add(tr);
                            totamt += Convert.ToDecimal(item.FeeAmount);

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
                        tdNewRow1.ColumnSpan = 4;
                        tdNewRow1.HorizontalAlign = HorizontalAlign.Center;
                        trNew.Cells.Add(tdNewRow1);

                        TableCell tdNewRow5 = new TableCell();
                        tdNewRow5.Width = Unit.Percentage(1);
                        tdNewRow5.Text = "<b>" + noofCandidates.ToString() + "</b>";
                        tdNewRow5.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow5);

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
                #endregion
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