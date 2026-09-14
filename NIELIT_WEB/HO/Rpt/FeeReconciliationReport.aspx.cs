using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Linq;
using RestSharp;
using System.Data.SqlClient;

public partial class HO_Rpt_FeeReconciliationReport : BasePage
{
    Table tbl = new Table();
    public static int j;   
    Int32 currentRoleId = 0;
    Int32 practicalfeetype = Convert.ToInt32(enmFeeType.PracticalFee);
    Int32 theoryfeetype = Convert.ToInt32(enmFeeType.ExaminationFee);
    Int32 improvementfeetype = Convert.ToInt32(enmFeeType.ImprovementOfPaperFee);
    Int32 processingfeetype = Convert.ToInt32(enmFeeType.PostageFeeChargedTowardExaminationCorrespondence);

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
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/FeeReconciliation.aspx"))
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
            int AppTypeId = Convert.ToInt32(Request.QueryString["AppTypeId"]);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(2);
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            tcCol1.Text = "#";
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(9);
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            tcCol2.Text = "Regional Center";
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(9);
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Text = "Fee-CSC Wallet";
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(9);
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            tcCol4.Text = "Fee-NEFT Transaction";
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(9);
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            tcCol5.Text = "Fee-Online Transaction";
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(9);
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            tcCol6.Text = "Total Fee Received";
            th.Cells.Add(tcCol6);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(9);
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            tcCol7.Text = "Total Candidates(Fee Received)";
            th.Cells.Add(tcCol7);

            TableHeaderCell tcCol8 = new TableHeaderCell();
            tcCol8.Width = Unit.Percentage(9);
            tcCol8.HorizontalAlign = HorizontalAlign.Center;
            tcCol8.Text = "Total Candidates(Fresh)";
            th.Cells.Add(tcCol8);

            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(9);
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            tcCol9.Text = "CarryForward(by RC)";
            th.Cells.Add(tcCol9);

            TableHeaderCell tcCol10 = new TableHeaderCell();
            tcCol10.Width = Unit.Percentage(9);
            tcCol10.HorizontalAlign = HorizontalAlign.Center;
            tcCol10.Text = "CarryForward(by Candidate)";
            th.Cells.Add(tcCol10);
            tbl.Rows.Add(th);

            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.Width = Unit.Percentage(9);
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            tcCol11.Text = "Total Candidate(Exam-Scheduled)";
            th.Cells.Add(tcCol11);
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
            DateTime AppStartDate;            
            DataTable dt;

            int ExamId = Convert.ToInt32(Request.QueryString["ExamId"]);
            int CourseCatId = Convert.ToInt32(Request.QueryString["CourseCatId"]);
            int CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            int AppTypeId = Convert.ToInt32(Request.QueryString["AppTypeId"]);
            enmApplicationType applicationType = (enmApplicationType)AppTypeId;

            Int64 OnlineTotal = 0, NeftTotal = 0, CSCTotal = 0, FeeReceivedTotal = 0;
            decimal FeeReceivedCandTotal = 0;
            Int32 FreshCandTotal = 0, ForwardByRcTotal = 0, ForwardByCandTotal = 0, ExamScheduledCandTotal = 0;

            string strHead = "";
            StringBuilder mySql = new StringBuilder();
            StringBuilder mySql1 = new StringBuilder();
            //December_2024
            SqlParameter[] param1 = new SqlParameter[3];

            if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
            {
                Int32 feeAmount = 0;
                Int32 feeTypeID = Convert.ToInt32(enmFeeType.RegistrationCumExaminationFee);
                feeAmount = (from f in context.FeeDetails
                             where f.CourseID == CourseId && f.FeeTypeID == feeTypeID &&
                                f.EffectiveFromDate == (from c in context.FeeDetails
                                                        where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                                                        select c.EffectiveFromDate).Max()
                             select new { FeeAmount = f.FeeAmount }).FirstOrDefault().FeeAmount;
                var courses = context.Courses.Find(CourseId);
                var Exams = context.Exams.Find(ExamId);
                strHead += "<br/><b>Course Category :</b>" + courses.CourseCategory.Name;
                strHead += "<br/><b>Course Name :</b>" + courses.Name;
                strHead += "<br/><b>Exam Name :</b>" + Exams.Name;

                AppStartDate = (from p in context.CutOffDates
                                where p.ExamID == ExamId && p.ActivityID == 1  // Commencement Date
                                select p).FirstOrDefault().EfferctiveDate;

                //mySql.Append("with base1 as (select 'Online' mode, d.Name, sum(c.Fee_Amt) as Total_amt, COUNT(distinct c.Number) Total_Cand " +
                //             "from Online_Transaction  a, Demand_Note b,Certificate_Exam_Application c, Regional_Center d " +
                //             "where  convert(date, a.[Request_Date]) >=convert(Date, '" + AppStartDate + "') and  a.Demand_Note_ID =b.ID  and a.Settled_On is not null " +
                //             "and c.Exam_ID ='" + ExamId + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID group by d.Name) " +
                //             ",base2 as(select 'NEFT' mode ,d.Name, sum(c.Fee_Amt) as Total_amt, COUNT(distinct c.Number) Total_Cand " +
                //             "from NEFT_Transaction  a, Demand_Note b,Certificate_Exam_Application c,Regional_Center d " +
                //             "where convert(date, a.[Transaction_Date]) >=convert(Date, '" + AppStartDate + "') and  a.Demand_Note_ID =b.ID " +
                //             "and  c.Exam_ID ='" + ExamId + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID group by d.Name) " +
                //             ",base3 as(select 'CSC' mode,d.Name, sum(c.Fee_Amt) as Total_amt, COUNT(distinct c.Number) Total_Cand " +
                //             "from CSC_Transaction a, Demand_Note b,Certificate_Exam_Application c,Regional_Center d " +
                //             "where  convert(date, a.[Date])>=convert(Date, '" + AppStartDate + "') and a.Response_Number is not null and a.Response_Message='Success' and  a.Demand_Note_ID =b.ID " +
                //             "and c.Exam_ID ='" + ExamId + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID group by d.Name) " +
                //             ",base5 as(select t.Name,COUNT(*) Total_Count, SUM(fee_amt) Fee_Amt from (select d.Name, c.[Number], c.Demand_Note_ID, c.Fee_Amt " +
                //             "from Online_Transaction a, Demand_Note b,Certificate_Exam_Application c, Regional_Center d " +
                //             "where  convert(date, a.[Request_Date]) < convert(Date, '" + AppStartDate + "') and  a.Demand_Note_ID =b.ID and a.Settled_On is not null  and  c.Exam_ID ='" + ExamId + "'	and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID " +
                //             "union " +
                //             "select d.Name, c.[Number], c.Demand_Note_ID, c.Fee_Amt from NEFT_Transaction  a, Demand_Note b,Certificate_Exam_Application c,Regional_Center d " +
                //             "where convert(date, a.[Transaction_Date]) <convert(Date, '" + AppStartDate + "')	and  a.Demand_Note_ID =b.ID  and  c.Exam_ID ='" + ExamId + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID  " +
                //             "union " +
                //             "select d.Name, c.Number, c.Demand_Note_ID, c.Fee_Amt from CSC_Transaction a, Demand_Note b,Certificate_Exam_Application c,Regional_Center d " +
                //             "where convert(date, a.[Date])<convert(Date, '" + AppStartDate + "') and a.Response_Number is not null and a.Response_Message='Success' and  a.Demand_Note_ID =b.ID   and  c.Exam_ID ='" + ExamId + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID ) " +
                //             "as t group by t.Name) " +
                //             ",base6 as (select d.Name, count(c.Number) Total_Count " +
                //             "from Certificate_Exam_Application c, Regional_Center d where Exam_ID = '" + ExamId + "' and Final_Submitted = 1 and Is_Exempted= 1 and Payment_Status_ID = 2 " +
                //             "and convert(date, c.[Date])>=convert(Date, '" + AppStartDate + "') and c.Regional_Center_ID = d.ID  group by  d.Name) " +
                //             ",base as ( select Name from Regional_Center ) " +
                //             "select base.Name RcName, isnull(base1.Total_amt, 0) OnlineAmount, isnull(base2.Total_amt, 0) NeftAmount, isnull(base3.Total_amt, 0) CscAmount " +
                //             ",(isnull(base1.Total_amt, 0) + isnull(base2.Total_amt, 0) + isnull(base3.Total_amt, 0)) TotalAmount " +
                //             ",(isnull(base1.Total_amt, 0) + isnull(base2.Total_amt, 0) + isnull(base3.Total_amt, 0))/'" + feeAmount + "' TotalFeeReceivedForCand " +
                //             ",(isnull(base1.Total_Cand, 0) + ISNULL(base2.Total_Cand, 0) + ISNULL(base3.Total_Cand, 0)) TotalFreshCand " +
                //             ",isnull(base5.Total_Count, 0) ForwardByRC ,isnull(base6.Total_Count, 0) ForwardByCandidate  " +
                //             "from base left join base1 on base.Name = base1.Name left join base2 on base.Name = base2.Name left join base3 on base.Name = base3.Name left join base5 on base.Name = base5.Name  left join base6 on base.Name = base6.Name " +
                //             "order by base.name ");

                //December_2024
                param1[0] = new SqlParameter("@AppStartDate", AppStartDate);
                param1[1] = new SqlParameter("@ExamId", ExamId);
                param1[2] = new SqlParameter("@feeAmount", feeAmount);
                                                                       
                mySql.Append("with base1 as (select 'Online' mode, d.Name, sum(c.Fee_Amt) as Total_amt, COUNT(distinct c.Number) Total_Cand " +
                             "from Online_Transaction  a, Demand_Note b,Certificate_Exam_Application c, Regional_Center d " +
                             "where  convert(date, a.[Request_Date]) >=convert(Date, @AppStartDate) and  a.Demand_Note_ID =b.ID  and a.Settled_On is not null " +
                             "and c.Exam_ID =@ExamId and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID group by d.Name) " +
                             ",base2 as(select 'NEFT' mode ,d.Name, sum(c.Fee_Amt) as Total_amt, COUNT(distinct c.Number) Total_Cand " +
                             "from NEFT_Transaction  a, Demand_Note b,Certificate_Exam_Application c,Regional_Center d " +
                             "where convert(date, a.[Transaction_Date]) >=convert(Date, @AppStartDate) and  a.Demand_Note_ID =b.ID " +
                             "and  c.Exam_ID =@ExamId and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID group by d.Name) " +
                             ",base3 as(select 'CSC' mode,d.Name, sum(c.Fee_Amt) as Total_amt, COUNT(distinct c.Number) Total_Cand " +
                             "from CSC_Transaction a, Demand_Note b,Certificate_Exam_Application c,Regional_Center d " +
                             "where  convert(date, a.[Date])>=convert(Date, @AppStartDate) and a.Response_Number is not null and a.Response_Message='Success' and  a.Demand_Note_ID =b.ID " +
                             "and c.Exam_ID =@ExamId and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID group by d.Name) " +
                             ",base5 as(select t.Name,COUNT(*) Total_Count, SUM(fee_amt) Fee_Amt from (select d.Name, c.[Number], c.Demand_Note_ID, c.Fee_Amt " +
                             "from Online_Transaction a, Demand_Note b,Certificate_Exam_Application c, Regional_Center d " +
                             "where  convert(date, a.[Request_Date]) < convert(Date, @AppStartDate) and  a.Demand_Note_ID =b.ID and a.Settled_On is not null  and  c.Exam_ID =@ExamId	and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID " +
                             "union " +
                             "select d.Name, c.[Number], c.Demand_Note_ID, c.Fee_Amt from NEFT_Transaction  a, Demand_Note b,Certificate_Exam_Application c,Regional_Center d " +
                             "where convert(date, a.[Transaction_Date]) <convert(Date, @AppStartDate)	and  a.Demand_Note_ID =b.ID  and  c.Exam_ID =@ExamId and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID  " +
                             "union " +
                             "select d.Name, c.Number, c.Demand_Note_ID, c.Fee_Amt from CSC_Transaction a, Demand_Note b,Certificate_Exam_Application c,Regional_Center d " +
                             "where convert(date, a.[Date])<convert(Date, @AppStartDate) and a.Response_Number is not null and a.Response_Message='Success' and  a.Demand_Note_ID =b.ID   and  c.Exam_ID =@ExamId and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID ) " +
                             "as t group by t.Name) " +
                             ",base6 as (select d.Name, count(c.Number) Total_Count " +
                             "from Certificate_Exam_Application c, Regional_Center d where Exam_ID = @ExamId and Final_Submitted = 1 and Is_Exempted= 1 and Payment_Status_ID = 2 " +
                             "and convert(date, c.[Date])>=convert(Date, @AppStartDate) and c.Regional_Center_ID = d.ID  group by  d.Name) " +
                             ",base as ( select Name from Regional_Center ) " +
                             "select base.Name RcName, isnull(base1.Total_amt, 0) OnlineAmount, isnull(base2.Total_amt, 0) NeftAmount, isnull(base3.Total_amt, 0) CscAmount " +
                             ",(isnull(base1.Total_amt, 0) + isnull(base2.Total_amt, 0) + isnull(base3.Total_amt, 0)) TotalAmount " +
                             ",(isnull(base1.Total_amt, 0) + isnull(base2.Total_amt, 0) + isnull(base3.Total_amt, 0))/@feeAmount TotalFeeReceivedForCand " +
                             ",(isnull(base1.Total_Cand, 0) + ISNULL(base2.Total_Cand, 0) + ISNULL(base3.Total_Cand, 0)) TotalFreshCand " +
                             ",isnull(base5.Total_Count, 0) ForwardByRC ,isnull(base6.Total_Count, 0) ForwardByCandidate  " +
                             "from base left join base1 on base.Name = base1.Name left join base2 on base.Name = base2.Name left join base3 on base.Name = base3.Name left join base5 on base.Name = base5.Name  left join base6 on base.Name = base6.Name " +
                             "order by base.name ");

            }
            else if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
            { ShowAlert("No service available for your query !!"); }
            else if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            { ShowAlert("No service available for your query !!"); }
            else { ShowAlert("Something went wrong !! Try Again Please."); }

            //dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
            //December_2024
            dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), param1, CommandType.Text, false);
            if (dt.Rows.Count > 0)
            {
                ShowTableHeader();
                foreach (DataRow dtRow in dt.Rows)
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

                    TableCell tcCol2 = new TableCell();
                    tcCol2.Width = Unit.Percentage(5);
                    tcCol2.HorizontalAlign = HorizontalAlign.Left;
                    tcCol2.Text = dtRow["RcName"].ToString();
                    tr.Cells.Add(tcCol2);


                    TableCell tcCol3 = new TableCell();
                    tcCol3.Width = Unit.Percentage(10);
                    tcCol3.HorizontalAlign = HorizontalAlign.Right;
                    tcCol3.Text = dtRow["OnlineAmount"].ToString();
                    OnlineTotal += Convert.ToInt64(tcCol3.Text);
                    tr.Cells.Add(tcCol3);

                    TableCell tcCol4 = new TableCell();
                    tcCol4.Width = Unit.Percentage(10);
                    tcCol4.HorizontalAlign = HorizontalAlign.Right;
                    tcCol4.Text = dtRow["NeftAmount"].ToString();
                    NeftTotal += Convert.ToInt64(tcCol4.Text);
                    tr.Cells.Add(tcCol4);

                    TableCell tcCol5 = new TableCell();
                    tcCol5.Width = Unit.Percentage(10);
                    tcCol5.HorizontalAlign = HorizontalAlign.Right;
                    tcCol5.Text = dtRow["CscAmount"].ToString();
                    CSCTotal += Convert.ToInt64(tcCol5.Text);
                    tr.Cells.Add(tcCol5);

                    TableCell tcCol6 = new TableCell();
                    tcCol6.HorizontalAlign = HorizontalAlign.Right;
                    tcCol6.Width = Unit.Percentage(10);
                    tcCol6.Text = dtRow["TotalAmount"].ToString();
                    FeeReceivedTotal += Convert.ToInt64(tcCol6.Text);
                    tr.Cells.Add(tcCol6);

                    TableCell tcCol7 = new TableCell();
                    tcCol7.HorizontalAlign = HorizontalAlign.Right;
                    tcCol7.Width = Unit.Percentage(10);
                    tcCol7.Text = dtRow["TotalFeeReceivedForCand"].ToString();
                    FeeReceivedCandTotal += Convert.ToDecimal(tcCol6.Text);
                    tr.Cells.Add(tcCol7);

                    TableCell tcCol8 = new TableCell();
                    tcCol8.HorizontalAlign = HorizontalAlign.Right;
                    tcCol8.Width = Unit.Percentage(10);
                    tcCol8.Text = Convert.ToInt32(dtRow["TotalFreshCand"]).ToString();
                    FreshCandTotal += Convert.ToInt32(tcCol8.Text);
                    tr.Cells.Add(tcCol8);

                    TableCell tcCol9 = new TableCell();
                    tcCol9.HorizontalAlign = HorizontalAlign.Right;
                    tcCol9.Width = Unit.Percentage(10);
                    tcCol9.Text = dtRow["ForwardByRC"].ToString();
                    ForwardByRcTotal += Convert.ToInt32(tcCol9.Text);
                    tr.Cells.Add(tcCol9);

                    TableCell tcCol10 = new TableCell();
                    tcCol10.HorizontalAlign = HorizontalAlign.Right;
                    tcCol10.Width = Unit.Percentage(10);
                    tcCol10.Text = dtRow["ForwardByCandidate"].ToString();
                    ForwardByCandTotal += Convert.ToInt32(tcCol10.Text);
                    tr.Cells.Add(tcCol10);

                    TableCell tcCol11 = new TableCell();
                    tcCol11.HorizontalAlign = HorizontalAlign.Right;
                    tcCol11.Width = Unit.Percentage(10);
                    tcCol11.Text = "0";
                    ExamScheduledCandTotal += Convert.ToInt32(tcCol11.Text);
                    tr.Cells.Add(tcCol11);

                    tbl.Rows.Add(tr);
                }

                //showing total
                TableRow trNew = new TableRow();
                if (i % 2 == 0)
                    trNew.CssClass = "gdalternate1";
                else
                    trNew.CssClass = "gdrow1";

                TableCell tdNewRow1 = new TableCell();
                tdNewRow1.Width = Unit.Percentage(7);
                tdNewRow1.Text = "<b>Total</b>";
                tdNewRow1.ColumnSpan = 2;
                tdNewRow1.HorizontalAlign = HorizontalAlign.Center;
                trNew.Cells.Add(tdNewRow1);

                TableCell tdNewRow3 = new TableCell();
                tdNewRow3.Width = Unit.Percentage(10);
                tdNewRow3.Text = "<b>" + OnlineTotal.ToString() + "</b>";
                tdNewRow3.HorizontalAlign = HorizontalAlign.Right;
                trNew.Cells.Add(tdNewRow3);

                TableCell tdNewRow4 = new TableCell();
                tdNewRow4.Width = Unit.Percentage(10);
                tdNewRow4.Text = "<b>" + NeftTotal.ToString() + "</b>";
                tdNewRow4.HorizontalAlign = HorizontalAlign.Right;
                trNew.Cells.Add(tdNewRow4);

                TableCell tdNewRow5 = new TableCell();
                tdNewRow5.Width = Unit.Percentage(10);
                tdNewRow5.Text = "<b>" + CSCTotal.ToString() + "</b>";
                tdNewRow5.HorizontalAlign = HorizontalAlign.Right;
                trNew.Cells.Add(tdNewRow5);

                TableCell tdNewRow6 = new TableCell();
                tdNewRow6.Width = Unit.Percentage(10);
                tdNewRow6.Text = "<b>" + FeeReceivedTotal.ToString() + "</b>";
                tdNewRow6.HorizontalAlign = HorizontalAlign.Right;
                trNew.Cells.Add(tdNewRow6);

                TableCell tdNewRow7 = new TableCell();
                tdNewRow7.Width = Unit.Percentage(10);
                tdNewRow7.Text = "<b>" + FeeReceivedCandTotal.ToString() + "</b>";
                tdNewRow7.HorizontalAlign = HorizontalAlign.Right;
                trNew.Cells.Add(tdNewRow7);

                TableCell tdNewRow8 = new TableCell();
                tdNewRow8.Width = Unit.Percentage(10);
                tdNewRow8.Text = "<b>" + FreshCandTotal.ToString() + "</b>";
                tdNewRow8.HorizontalAlign = HorizontalAlign.Right;
                trNew.Cells.Add(tdNewRow8);

                TableCell tdNewRow9 = new TableCell();
                tdNewRow9.Width = Unit.Percentage(10);
                tdNewRow9.Text = "<b>" + ForwardByRcTotal.ToString() + "</b>";
                tdNewRow9.HorizontalAlign = HorizontalAlign.Right;
                trNew.Cells.Add(tdNewRow9);

                TableCell tdNewRow10 = new TableCell();
                tdNewRow10.Width = Unit.Percentage(10);
                tdNewRow10.Text = "<b>" + ForwardByCandTotal.ToString() + "</b>";
                tdNewRow10.HorizontalAlign = HorizontalAlign.Right;
                trNew.Cells.Add(tdNewRow10);

                TableCell tdNewRow11 = new TableCell();
                tdNewRow11.Width = Unit.Percentage(10);
                tdNewRow11.Text = "";
                tdNewRow11.HorizontalAlign = HorizontalAlign.Right;
                trNew.Cells.Add(tdNewRow11);

                tbl.Rows.Add(trNew);
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "No Record Found !";
            }
            LblRptSubHeader.Text = strHead;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }
        finally { context.Dispose(); }
    }
}