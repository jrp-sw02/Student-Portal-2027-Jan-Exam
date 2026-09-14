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

public partial class VANielitCentrePaymentReportForGstRep : BasePage
{
    Table tbl = new Table();
    StringBuilder str = new StringBuilder();
    Int32 currentRoleId = 0;
    DataTable dt;
    Int32 cSharePercent = 0, ccSharePercent = 0, HqSharePercent = 0;
    Int64 gTotalCand = 0, gTotalFee, gTotalAmtWithGst=0, gTotalGst = 0, gTotalFeeCshare = 0, gTotalFeeCCshare = 0, gTotalFeeHqShare = 0, gTotalGstCshare = 0, gTotalGstCCshare = 0, gTotalGstHqShare = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }

            currentRoleId = Convert.ToInt32(Session["RoleID"]);

            if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/Rpt/VANielitCentrePaymentReportForGst.aspx"))
            {
                //Testing
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            if (!Page.IsPostBack)
            {
                if ((!string.IsNullOrEmpty(Request.QueryString["centreId"]))
                && (!string.IsNullOrEmpty(Request.QueryString["dateFrom"]))
                && (!string.IsNullOrEmpty(Request.QueryString["dateTo"])))
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
            if ((!string.IsNullOrEmpty(Request.QueryString["centreId"]))
            && (!string.IsNullOrEmpty(Request.QueryString["dateFrom"]))
            && (!string.IsNullOrEmpty(Request.QueryString["dateTo"])))
            {
                Int32 centreID = 0;

                DateTime FromDate = Convert.ToDateTime(Request.QueryString["dateFrom"]);
                DateTime ToDate = Convert.ToDateTime(Request.QueryString["dateTo"]);
                centreID = Convert.ToInt32(Request.QueryString["centreId"]);

                if (centreID == 0)
                {
                    ShowAlert("Centre Name Not Found");
                    return;
                }

                if (FromDate.Day.ToString() != "1")
                {
                    ShowAlert("Invalid settled date from");
                    return;
                }

                DateTime OneM;
                OneM = FromDate.AddMonths(1).AddDays(-1);
                if (ToDate != OneM)
                {
                    ShowAlert("Invalid Report Period");
                    return;
                }


                using (DataTable dt = RepVAR_CentreShareReport(centreID, FromDate, ToDate))
                {
                    if (dt.Rows.Count > 0)
                    {
                        var application = (from a in dt.AsEnumerable()
                                           select new
                                           {
                                               CentreNa = a.Field<string>("CentreName"),
                                               CoCatName = a.Field<string>("CourseCategory"),
                                               CourseName = a.Field<string>("CourseName"),
                                               batchId = a.Field<Int64>("BatchId"),
                                               batchCode = a.Field<string>("BatchCode"),
                                               CandidateCount = a.Field<int>("CandidateCount"),
                                               TotalFeeAmount = a.Field<decimal>("TotalFeeAmount"),
                                               CShare = a.Field<decimal>("CShare"),
                                               ccShare = a.Field<decimal>("ccShare"),
                                               HqShare = a.Field<decimal>("HqShare"),
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

                                Int32 Batch_id = 0;
                                Int64 fee = 0, Tfee = 0, cand = 0, gst = 0, tGstAmt = 0, TotalFeeAmt = 0;

                                Batch_id = Convert.ToInt32(app.batchId);
                                
                                // Total Fee Amount
                                fee = GetFeeAmount(Batch_id, FromDate, ToDate);
                                cand = Convert.ToInt64(app.CandidateCount);
                                Tfee = fee * cand;
                                // Total Gst Amount
                                gst = GetGstAmount(Batch_id, FromDate, ToDate);
                                tGstAmt = gst * cand;

                                //decimal TotalFeeAmt = 0;
                                TotalFeeAmt = Tfee + tGstAmt;

                                TableCell tdRow = new TableCell();
                                tdRow.Width = Unit.Percentage(1);
                                tdRow.Text = i.ToString();
                                tdRow.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow);

                                TableCell tdRow3 = new TableCell();
                                tdRow3.Width = Unit.Percentage(8);
                                tdRow3.Text = app.CoCatName.ToString();
                                tdRow3.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow3);

                                TableCell tdRow5 = new TableCell();
                                tdRow5.Width = Unit.Percentage(8);
                                tdRow5.Text = app.CourseName.ToString();
                                tdRow5.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow5);

                                TableCell tdRow13 = new TableCell();
                                tdRow13.Width = Unit.Percentage(6);
                                tdRow13.Text = app.batchCode.ToString();
                                tdRow13.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(tdRow13);

                                //Total Candidates
                                TableCell tdRow14 = new TableCell();
                                tdRow14.Width = Unit.Percentage(6);
                                tdRow14.Text = app.CandidateCount.ToString();
                                gTotalCand += Convert.ToInt64(app.CandidateCount);
                                tdRow14.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(tdRow14);
                                
                                //Total fee Amount
                                TableCell tdRow16 = new TableCell();
                                tdRow16.Width = Unit.Percentage(8);
                                if (Tfee != 0)
                                { 
                                    tdRow16.Text = Convert.ToString(Tfee) + ".00";
                                    gTotalFee += Tfee;
                                }
                                else { tdRow16.Text = "NA"; }
                                tdRow16.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow16);

                                //Total GST Amount
                                TableCell tdRow17 = new TableCell();
                                tdRow17.Width = Unit.Percentage(8);
                                if (tGstAmt != 0)
                                { 
                                    tdRow17.Text = Convert.ToString(tGstAmt) + ".00";
                                    gTotalGst += tGstAmt;
                                }
                                else { tdRow17.Text = "NA"; }
                                tdRow17.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow17);

                                //TotalAmt=  Fee + GST
                                TableCell tdRow18 = new TableCell();
                                tdRow18.Width = Unit.Percentage(6);
                                //if (app.TotalFeeAmount != 0)
                                //{ TotalFeeAmt = Convert.ToDecimal(app.TotalFeeAmount); }
                                tdRow18.Text = Convert.ToString(TotalFeeAmt) + ".00";
                                gTotalAmtWithGst += Convert.ToInt64(TotalFeeAmt);
                                tdRow18.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow18);
                                //TableCell tdRow18 = new TableCell();
                                //tdRow18.Width = Unit.Percentage(6);
                                //decimal TotalFeeAmt = 0;
                                //if (app.TotalFeeAmount != 0)
                                //{ TotalFeeAmt = Convert.ToDecimal(app.TotalFeeAmount); }
                                //tdRow18.Text = Convert.ToString(TotalFeeAmt) + ".00";
                                //tdRow18.HorizontalAlign = HorizontalAlign.Right;
                                //tr.Cells.Add(tdRow18);

                                //Centre Share Amount (60%)
                                TableCell tdRow19 = new TableCell();
                                tdRow19.Width = Unit.Percentage(6);
                                Int64 cShareAmt = 0;
                                cShareAmt=(Tfee*cSharePercent)/100;
                                //if (app.CShare != 0)
                                //{ cShareAmt = Convert.ToInt32(app.CShare); }
                                tdRow19.Text = Convert.ToString(cShareAmt) + ".00";
                                gTotalFeeCshare += cShareAmt;
                                tdRow19.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow19);
                                //

                                TableCell tdRow19G = new TableCell();
                                tdRow19G.Width = Unit.Percentage(6);
                                Int64 cShareAmtGst = 0;
                                cShareAmtGst = (tGstAmt * cSharePercent) / 100;
                                //if (app.CShare != 0)
                                //{ cShareAmtGst = Convert.ToInt32(app.CShare); }
                                tdRow19G.Text = Convert.ToString(cShareAmtGst) + ".00";
                                gTotalGstCshare += cShareAmtGst;
                                tdRow19G.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow19G);
                                //

                                //Controlling Centre Share Amount (30%)
                                TableCell tdRow22 = new TableCell();
                                tdRow22.Width = Unit.Percentage(6);
                                Int64 CcShareAmt = 0;
                                CcShareAmt = (Tfee * ccSharePercent) / 100;
                                //if (app.ccShare != 0)
                                //{ CcShareAmt = Convert.ToInt32(app.ccShare); }
                                tdRow22.Text = Convert.ToString(CcShareAmt) + ".00";
                                gTotalFeeCCshare += CcShareAmt;
                                tdRow22.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow22);
                                //
                                TableCell tdRow22G = new TableCell();
                                tdRow22G.Width = Unit.Percentage(6);
                                Int64 CcShareAmtGst = 0;
                                CcShareAmtGst = (tGstAmt * ccSharePercent) / 100;
                                //if (app.ccShare != 0)
                                //{ CcShareAmtGst = Convert.ToInt32(app.ccShare); }
                                tdRow22G.Text = Convert.ToString(CcShareAmtGst) + ".00";
                                gTotalGstCCshare += CcShareAmtGst;
                                tdRow22G.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow22G);
                                
                                
                                // For Round figure of Hq Amount

                                //Hq Share Amount (10%)
                                TableCell tdRow23 = new TableCell();
                                tdRow23.Width = Unit.Percentage(6);
                                Int64 HqShareAmt = 0;
                                HqShareAmt = Tfee - (cShareAmt + CcShareAmt);
                                //HqShareAmt = (Tfee * HqSharePercent) / 100;
                                //if (app.HqShare != 0)
                                //{ HqShareAmt = Convert.ToInt32(app.HqShare); }
                                tdRow23.Text = Convert.ToString(HqShareAmt) + ".00";
                                gTotalFeeHqShare += HqShareAmt;
                                tdRow23.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow23);
                                //
                                TableCell tdRow23G = new TableCell();
                                tdRow23G.Width = Unit.Percentage(6);
                                Int64 HqShareAmtGst = 0;
                                HqShareAmtGst = tGstAmt - (cShareAmtGst + CcShareAmtGst);
                                //HqShareAmtGst = (tGstAmt * HqSharePercent) / 100;
                                //if (app.HqShare != 0)
                                //{ HqShareAmtGst = Convert.ToInt32(app.HqShare); }
                                tdRow23G.Text = Convert.ToString(HqShareAmtGst) + ".00";
                                gTotalGstHqShare += HqShareAmtGst;
                                tdRow23G.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow23G);



                                ////Hq Share Amount (10%)
                                //TableCell tdRow23 = new TableCell();
                                //tdRow23.Width = Unit.Percentage(6);
                                //Int64 HqShareAmt = 0;
                                //HqShareAmt = (Tfee * HqSharePercent) / 100;
                                ////if (app.HqShare != 0)
                                ////{ HqShareAmt = Convert.ToInt32(app.HqShare); }
                                //tdRow23.Text = Convert.ToString(HqShareAmt) + ".00";
                                //gTotalFeeHqShare += HqShareAmt;
                                //tdRow23.HorizontalAlign = HorizontalAlign.Right;
                                //tr.Cells.Add(tdRow23);
                                ////
                                //TableCell tdRow23G = new TableCell();
                                //tdRow23G.Width = Unit.Percentage(6);
                                //Int64 HqShareAmtGst = 0;
                                //HqShareAmtGst = (tGstAmt * HqSharePercent) / 100;
                                ////if (app.HqShare != 0)
                                ////{ HqShareAmtGst = Convert.ToInt32(app.HqShare); }
                                //tdRow23G.Text = Convert.ToString(HqShareAmtGst) + ".00";
                                //gTotalGstHqShare += HqShareAmtGst;
                                //tdRow23G.HorizontalAlign = HorizontalAlign.Right;
                                //tr.Cells.Add(tdRow23G);

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
                            tdNewRow1.ColumnSpan = 4;
                            tdNewRow1.HorizontalAlign = HorizontalAlign.Center;
                            trTotal.Cells.Add(tdNewRow1);
                            
                            TableCell tdNewRow12 = new TableCell();
                            tdNewRow12.Width = Unit.Percentage(1);
                            tdNewRow12.Text = "<b>" + gTotalCand + "</b>";
                            tdNewRow12.HorizontalAlign = HorizontalAlign.Center;
                            trTotal.Cells.Add(tdNewRow12);

                            TableCell tdNewRow13 = new TableCell();
                            tdNewRow13.Width = Unit.Percentage(1);
                            tdNewRow13.Text = "<b>" + gTotalFee + ".00" + "</b>";
                            tdNewRow13.HorizontalAlign = HorizontalAlign.Right;
                            trTotal.Cells.Add(tdNewRow13);

                            TableCell tdNewRow14 = new TableCell();
                            tdNewRow14.Width = Unit.Percentage(1);
                            tdNewRow14.Text = "<b>" + gTotalGst + ".00" + "</b>";
                            tdNewRow14.HorizontalAlign = HorizontalAlign.Right;
                            trTotal.Cells.Add(tdNewRow14);                            
                            
                            TableCell tdNewRow15 = new TableCell();
                            tdNewRow15.Width = Unit.Percentage(1);
                            tdNewRow15.Text = "<b>" + gTotalAmtWithGst + ".00" + "</b>";
                            tdNewRow15.HorizontalAlign = HorizontalAlign.Right;
                            trTotal.Cells.Add(tdNewRow15);
                            
                            TableCell tdNewRow16 = new TableCell();
                            tdNewRow16.Width = Unit.Percentage(1);
                            tdNewRow16.Text = "<b>" + gTotalFeeCshare + ".00" + "</b>";
                            tdNewRow16.HorizontalAlign = HorizontalAlign.Right;
                            trTotal.Cells.Add(tdNewRow16);

                            TableCell tdNewRow17 = new TableCell();
                            tdNewRow17.Width = Unit.Percentage(1);
                            tdNewRow17.Text = "<b>" + gTotalGstCshare + ".00" + "</b>";
                            tdNewRow17.HorizontalAlign = HorizontalAlign.Right;
                            trTotal.Cells.Add(tdNewRow17);

                            TableCell tdNewRow18 = new TableCell();
                            tdNewRow18.Width = Unit.Percentage(1);
                            tdNewRow18.Text = "<b>" + gTotalFeeCCshare + ".00" + "</b>";
                            tdNewRow18.HorizontalAlign = HorizontalAlign.Right;
                            trTotal.Cells.Add(tdNewRow18);

                            TableCell tdNewRow19 = new TableCell();
                            tdNewRow19.Width = Unit.Percentage(1);
                            tdNewRow19.Text = "<b>" + gTotalGstCCshare + ".00" + "</b>";
                            tdNewRow19.HorizontalAlign = HorizontalAlign.Right;
                            trTotal.Cells.Add(tdNewRow19);
                            
                            TableCell tdNewRow20 = new TableCell();
                            tdNewRow20.Width = Unit.Percentage(1);
                            tdNewRow20.Text = "<b>" + gTotalFeeHqShare + ".00" + "</b>";
                            tdNewRow20.HorizontalAlign = HorizontalAlign.Right;
                            trTotal.Cells.Add(tdNewRow20);

                            TableCell tdNewRow21 = new TableCell();
                            tdNewRow21.Width = Unit.Percentage(1);
                            tdNewRow21.Text = "<b>" + gTotalGstHqShare + ".00" + "</b>";
                            tdNewRow21.HorizontalAlign = HorizontalAlign.Right;
                            trTotal.Cells.Add(tdNewRow21);

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
            DateTime FromDate = Convert.ToDateTime(Request.QueryString["dateFrom"]);
            DateTime ToDate = Convert.ToDateTime(Request.QueryString["dateTo"]);
            Int32 centreID = Convert.ToInt32(Request.QueryString["centreId"]);

            string centreName = "", cShare="", ccShare="", HqShare="";

            //For get Centre name
            if (centreID != 0)
            {
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    var centre = (from s in context.NielitCentres
                                  where s.ID == centreID
                                  select new { name = s.Name }).FirstOrDefault();
                    if (centre != null)
                    {
                        centreName = centre.name;
                    }

                    //For get share percentage
                    StringBuilder mySql = new StringBuilder();
                    mySql.Append(" select sh.centreShare cShare, sh.controllerShare ccShare, sh.HQShare HqShare from [NIELITMIS].[dbo].[virtualAcademyShares] sh where sh.effectiveTo is null");
                    dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dtRow in dt.Rows)
                        {
                            cSharePercent = Convert.ToInt32(dtRow["cShare"]);
                            ccSharePercent = Convert.ToInt32(dtRow["ccShare"]);
                            HqSharePercent = Convert.ToInt32(dtRow["HqShare"]);

                            cShare = cSharePercent.ToString() + "%";
                            ccShare = ccSharePercent.ToString() + "%";
                            HqShare = HqSharePercent.ToString() + "%";
                        }
                    }
                }
            }

            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);

            //Int32 NoOfColumnSpan = 11;
            Int32 NoOfColumnSpan = 14;

            TableHeaderRow th = new TableHeaderRow();
            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.ColumnSpan = NoOfColumnSpan;
            tcCol1.Width = Unit.Percentage(18);
            tcCol1.Text = "Centre Name : " + centreName;
            tcCol1.HorizontalAlign = HorizontalAlign.Left;
            th.Cells.Add(tcCol1);
            tbl.Rows.Add(th);

            //
            TableHeaderRow th01 = new TableHeaderRow();
            TableHeaderCell tcCol02 = new TableHeaderCell();
            tcCol02.ColumnSpan = NoOfColumnSpan;
            tcCol02.Width = Unit.Percentage(18);
            tcCol02.Text = " Payment Settled Date From : " + FromDate.ToString("dd-MMM-yyyy");
            tcCol02.HorizontalAlign = HorizontalAlign.Left;
            th01.Cells.Add(tcCol02);
            tbl.Rows.Add(th01);

            TableHeaderRow th02 = new TableHeaderRow();
            TableHeaderCell tcCol03 = new TableHeaderCell();
            tcCol03.ColumnSpan = NoOfColumnSpan;
            tcCol03.Width = Unit.Percentage(18);
            tcCol03.Text = " Payment Settled Date To : " + ToDate.ToString("dd-MMM-yyyy") + "<br/>";
            tcCol03.HorizontalAlign = HorizontalAlign.Left;
            th02.Cells.Add(tcCol03);
            tbl.Rows.Add(th02);

            TableHeaderRow th2B = new TableHeaderRow();

            #region Blank row
            TableHeaderCell tcB = new TableHeaderCell();
            tcB.Width = Unit.Percentage(1);
            tcB.Text = "";
            th2B.Cells.Add(tcB);

            TableHeaderCell tcCol2B = new TableHeaderCell();
            tcCol2B.Width = Unit.Percentage(1);
            tcCol2B.Text = "";
            th2B.Cells.Add(tcCol2B);

            TableHeaderCell tcCol3B = new TableHeaderCell();
            tcCol3B.Width = Unit.Percentage(1);
            tcCol3B.Text = "";
            th2B.Cells.Add(tcCol3B);

            TableHeaderCell tcCol4B = new TableHeaderCell();
            tcCol4B.Width = Unit.Percentage(1);
            tcCol4B.Text = "";
            th2B.Cells.Add(tcCol4B);

            TableHeaderCell tcCol5B = new TableHeaderCell();
            tcCol5B.Width = Unit.Percentage(1);
            tcCol5B.Text = "";
            th2B.Cells.Add(tcCol5B);

            TableHeaderCell tcCol6B = new TableHeaderCell();
            tcCol6B.Width = Unit.Percentage(1);
            tcCol6B.Text = "";
            th2B.Cells.Add(tcCol6B);

            TableHeaderCell tcCol7B = new TableHeaderCell();
            tcCol7B.Width = Unit.Percentage(1);
            tcCol7B.Text = "";
            th2B.Cells.Add(tcCol7B);

            TableHeaderCell tcCol8B = new TableHeaderCell();
            tcCol8B.Width = Unit.Percentage(6);
            tcCol8B.Text = "";
            th2B.Cells.Add(tcCol8B);
            
            TableHeaderCell tcCol9B = new TableHeaderCell();
            tcCol9B.Width = Unit.Percentage(6);
            tcCol9B.Text = "";
            th2B.Cells.Add(tcCol9B);

            TableHeaderCell tcCol91B = new TableHeaderCell();
            tcCol91B.Width = Unit.Percentage(6);
            tcCol91B.Text = "";
            th2B.Cells.Add(tcCol91B);


            TableHeaderCell tcCol10B = new TableHeaderCell();
            tcCol10B.Width = Unit.Percentage(1);
            tcCol10B.Text = "";
            th2B.Cells.Add(tcCol10B);
            
            TableHeaderCell tcCol101B = new TableHeaderCell();
            tcCol101B.Width = Unit.Percentage(6);
            tcCol101B.Text = "";
            th2B.Cells.Add(tcCol101B);
           
            TableHeaderCell tcCol11B = new TableHeaderCell();
            tcCol11B.Width = Unit.Percentage(6);
            tcCol11B.Text = "";
            th2B.Cells.Add(tcCol11B);

            TableHeaderCell tcCol111B = new TableHeaderCell();
            tcCol111B.Width = Unit.Percentage(6);
            tcCol111B.Text = "";
            th2B.Cells.Add(tcCol111B);

            tbl.Rows.Add(th2B);
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
            tcCol2.Text = "<b>Course Category</b>";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th2.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(8);
            tcCol3.Text = "<b>Course</b>";
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th2.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(6);
            tcCol4.Text = "<b>Batch Code</b>";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th2.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(8);
            tcCol5.Text = "<b>No Of Candidates</b>";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th2.Cells.Add(tcCol5);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(8);
            tcCol6.Text = "<b>Fee Amount (Rs.)</b>";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th2.Cells.Add(tcCol6);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(8);
            tcCol7.Text = "<b>GST Amount (Rs.)</b>";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th2.Cells.Add(tcCol7);

            TableHeaderCell tcCol8 = new TableHeaderCell();
            tcCol8.Width = Unit.Percentage(8);
            tcCol8.Text = "<b>Total Amount (Rs.)</b>";
            tcCol8.HorizontalAlign = HorizontalAlign.Center;
            th2.Cells.Add(tcCol8);
            
            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(12);
            tcCol9.ColumnSpan = 2;
            tcCol9.Text = "<b>Centre Share ("+cShare+") (Rs.)</b>";
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            th2.Cells.Add(tcCol9);

            TableHeaderCell tcCol10 = new TableHeaderCell();
            tcCol10.Width = Unit.Percentage(12);
            tcCol10.ColumnSpan = 2;
            tcCol10.Text = "<b>Controlling Centre (" + ccShare + ") (Rs.)</b>";
            tcCol10.HorizontalAlign = HorizontalAlign.Center;
            th2.Cells.Add(tcCol10);

            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.Width = Unit.Percentage(12);
            tcCol11.ColumnSpan = 2;
            tcCol11.Text = "<b>HQ Share (" + HqShare + ") (Rs.)</b>";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th2.Cells.Add(tcCol11);

            tbl.Rows.Add(th2);
            //

            TableHeaderRow th0211 = new TableHeaderRow();
            th0211.CssClass = "head1";

            TableHeaderCell tcCol0300 = new TableHeaderCell();
            tcCol0300.ColumnSpan = 8;
            tcCol0300.Width = Unit.Percentage(1);
            tcCol0300.Text = "";
            tcCol0300.HorizontalAlign = HorizontalAlign.Left;
            th0211.Cells.Add(tcCol0300);

            TableHeaderCell tcCol0301 = new TableHeaderCell();
            tcCol0301.Width = Unit.Percentage(6);
            tcCol0301.Text = "Fee";
            tcCol0301.HorizontalAlign = HorizontalAlign.Center;
            th0211.Cells.Add(tcCol0301);

            TableHeaderCell tcCol0302 = new TableHeaderCell();
            tcCol0302.Width = Unit.Percentage(6);
            tcCol0302.Text = " GST ";
            tcCol0302.HorizontalAlign = HorizontalAlign.Center;
            th0211.Cells.Add(tcCol0302);

            TableHeaderCell tcCol03001 = new TableHeaderCell();
            tcCol03001.Width = Unit.Percentage(6);
            tcCol03001.Text = "Fee";
            tcCol03001.HorizontalAlign = HorizontalAlign.Center;
            th0211.Cells.Add(tcCol03001);

            TableHeaderCell tcCol03002 = new TableHeaderCell();
            tcCol03002.Width = Unit.Percentage(6);
            tcCol03002.Text = " GST ";
            tcCol03002.HorizontalAlign = HorizontalAlign.Center;
            th0211.Cells.Add(tcCol03002);

            TableHeaderCell tcCol03011 = new TableHeaderCell();
            tcCol03011.Width = Unit.Percentage(6);
            tcCol03011.Text = "Fee";
            tcCol03011.HorizontalAlign = HorizontalAlign.Center;
            th0211.Cells.Add(tcCol03011);

            TableHeaderCell tcCol03021 = new TableHeaderCell();
            tcCol03021.Width = Unit.Percentage(6);
            tcCol03021.Text = " GST ";
            tcCol03021.HorizontalAlign = HorizontalAlign.Center;
            th0211.Cells.Add(tcCol03021);
            
            tbl.Rows.Add(th0211);
            #endregion
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public DataTable RepVAR_CentreShareReport(Int32 centreID, DateTime FromDate, DateTime ToDate)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand scCommand = new SqlCommand("VA_RepCentreShareReport", con))
            {
                scCommand.CommandType = CommandType.StoredProcedure;

                scCommand.Parameters.Add(new SqlParameter("@CentreId", SqlDbType.BigInt));
                scCommand.Parameters["@CentreId"].Value = centreID;
                scCommand.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.Date));
                scCommand.Parameters["@FromDate"].Value = FromDate;
                scCommand.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.Date));
                scCommand.Parameters["@ToDate"].Value = ToDate;

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
            Response.AddHeader("content-disposition", "attachment;filename=VANielitCentrePaymentReportForGstRep.pdf");
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
            Response.AddHeader("content-disposition", "attachment;filename=VANielitCentrePaymentReportForGstRep.xls");
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
    protected Int64 GetFeeAmount(Int32 batchID, DateTime FromDate, DateTime ToDate)
    {
        try
        {
            Int64 feeAmt = 0, FeeTypeID = 0;
            using (NIELITMISContext context = new NIELITMISContext())
            {
                FeeTypeID = (from f in context.feeTypeMas
                             where f.feeType == "Registration Fee"
                             select new { FeeTypeID = f.ID }).FirstOrDefault().FeeTypeID;

                var FeeRegEXists = (from s in context.NielitCentreBatchFees
                                    where s.batchID == batchID && s.feeTypeID == FeeTypeID
                                    //&& s.effectiveFromDate <= FromDate && (s.effectiveToDate >= ToDate || s.effectiveToDate == null)
                                    select new
                                    {
                                        ID = s.ID,
                                        CourseId = s.batchID
                                    }).ToList();

                if (FeeRegEXists.Count > 0)
                {
                    feeAmt = (from f in context.NielitCentreBatchFees
                              where f.batchID == batchID && f.feeTypeID == FeeTypeID
                                    && f.effectiveFromDate <= FromDate
                                    && (f.effectiveToDate >= ToDate || f.effectiveToDate == null)
                              select new { FeeAmount = f.feeAmount }).FirstOrDefault().FeeAmount;
                }
            };
            return feeAmt;
        }
        catch (Exception ex) { throw ex; }
    }
    protected Int64 GetGstAmount(Int32 batchID, DateTime FromDate, DateTime ToDate)
    {
        try
        {


            Int64 FeeTypeIDGST = 0, feeAmountGST = 0;
            using (NIELITMISContext context = new NIELITMISContext())
            {
                FeeTypeIDGST = (from f in context.feeTypeMas
                                where f.feeType == "GST on Registration Fee"
                                select new { FeeTypeID = f.ID }).FirstOrDefault().FeeTypeID;

                var FeeGSTEXists = (from s in context.NielitCentreBatchFees
                                    where s.batchID == batchID && s.feeTypeID == FeeTypeIDGST
                                    //&& s.effectiveFromDate <= FromDate && (s.effectiveToDate >= ToDate || s.effectiveToDate == null)
                                    select new
                                    {
                                        ID = s.ID,
                                        CourseId = s.batchID
                                    }).ToList();

                if (FeeGSTEXists.Count > 0)
                {
                    feeAmountGST = (from f in context.NielitCentreBatchFees
                                    where f.batchID == batchID && f.feeTypeID == FeeTypeIDGST
                                    && f.effectiveFromDate <= FromDate && (f.effectiveToDate >= ToDate || f.effectiveToDate == null)
                                    select new { FeeAmount = f.feeAmount }).FirstOrDefault().FeeAmount;
                }
            };
            return feeAmountGST;
        }
        catch (Exception ex) { throw ex; }
    }
    #endregion


}