using System;
using System.Data;
using System.Data.SqlClient;                                            //November_2024
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class HO_Rpt_RegionalCentrePaymentReport : BasePage
{
    Table tbl = new Table();
    //EConnectContext context = new EConnectContext();
   
    UserType loginUserType;   
    Int32 applicantTypeID = 0;
    Int32 CourseID = 0;
    Int64 ExamId = 0;
    Int32 CourseCategoryID = 0;
    Int32 currentRoleId = 0;    
    Int32 RegionalCentreID = 0;
    Int32 ReportTypeID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/RegionalCentrePaymentReportFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            if (!Page.IsPostBack)
            {
                if ((!string.IsNullOrEmpty(Request.QueryString["RegionalCentreID"])) && (!string.IsNullOrEmpty(Request.QueryString["CourseCategory"])) && (!string.IsNullOrEmpty(Request.QueryString["ExamId"])) && (!string.IsNullOrEmpty(Request.QueryString["TypeId"])))
                {
                    tbl.CssClass = "sample3";
                    tbl.CellPadding = 2;
                    tbl.CellSpacing = 1;
                    tbl.Width = Unit.Percentage(100);
                    ShowTableData();
                    divReportData.Controls.Add(tbl);
                }
                else
                {
                    Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Home.aspx")));
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
    protected void ShowTableHeader()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                TableHeaderRow th = new TableHeaderRow();
                th.CssClass = "head1";

                TableHeaderCell tcCol1 = new TableHeaderCell();
                tcCol1.Width = Unit.Percentage(3);
                tcCol1.Text = "#";
                th.Cells.Add(tcCol1);

                TableHeaderCell tcCol2 = new TableHeaderCell();
                tcCol2.Width = Unit.Percentage(15);
                tcCol2.ColumnSpan = 3;
                tcCol2.Text = "Regional Centre Name";
                th.Cells.Add(tcCol2);

                TableHeaderCell tcCol3 = new TableHeaderCell();
                tcCol3.Width = Unit.Percentage(15);
                tcCol3.Text = "No. of " + context.Exams.Find(ExamId).Course.Code + " Candidates " + "<br/> (Excluding Exempted Cases) ";
                th.Cells.Add(tcCol3);

                TableHeaderCell tcCol6 = new TableHeaderCell();
                tcCol6.Width = Unit.Percentage(8);
                tcCol6.Text = "By Online Payment Mode";
                th.Cells.Add(tcCol6);

                TableHeaderCell tcCol7 = new TableHeaderCell();
                tcCol7.Width = Unit.Percentage(8);
                tcCol7.Text = "By NEFT/RTGS Mode";
                th.Cells.Add(tcCol7);

                TableHeaderCell tcCol9 = new TableHeaderCell();
                tcCol9.Width = Unit.Percentage(8);
                tcCol9.Text = "By CSC-SPV Mode";
                th.Cells.Add(tcCol9);

                TableHeaderCell tcCol10 = new TableHeaderCell();
                tcCol10.Width = Unit.Percentage(12);
                tcCol10.Text = "By Demand Draft Mode (Exempted Cases)";
                th.Cells.Add(tcCol10);

                TableHeaderCell tcCol5 = new TableHeaderCell();
                tcCol5.Width = Unit.Percentage(11);
                tcCol5.Text = "Total " + context.Exams.Find(ExamId).Course.Code + " Fee Amount ";
                th.Cells.Add(tcCol5);

                tbl.Rows.Add(th);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ShowTableHeader1()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                TableHeaderRow th = new TableHeaderRow();
                th.CssClass = "head1";

                TableHeaderCell tcCol1 = new TableHeaderCell();
                tcCol1.Width = Unit.Percentage(3);
                tcCol1.Text = "#";
                th.Cells.Add(tcCol1);

                TableHeaderCell tcCol2 = new TableHeaderCell();
                tcCol2.Width = Unit.Percentage(12);
                tcCol2.Text = "Regional Centre Name";
                th.Cells.Add(tcCol2);


                TableHeaderCell tcCol32 = new TableHeaderCell();
                tcCol32.Width = Unit.Percentage(12);
                tcCol32.Text = "Forwarded From Exam";
                th.Cells.Add(tcCol32);


                TableHeaderCell tcCol22 = new TableHeaderCell();
                tcCol22.Width = Unit.Percentage(12);
                tcCol22.Text = "Forwarded to Exam";
                th.Cells.Add(tcCol22);

                TableHeaderCell tcCol3 = new TableHeaderCell();
                tcCol3.Width = Unit.Percentage(15);
                tcCol3.Text = "No. of " + context.Exams.Find(ExamId).Course.Code + " Candidates " + "<br/> (Excluding Exempted Cases) ";
                th.Cells.Add(tcCol3);

                TableHeaderCell tcCol6 = new TableHeaderCell();
                tcCol6.Width = Unit.Percentage(8);
                tcCol6.Text = "By Online Payment Mode";
                th.Cells.Add(tcCol6);

                TableHeaderCell tcCol7 = new TableHeaderCell();
                tcCol7.Width = Unit.Percentage(8);
                tcCol7.Text = "By NEFT/RTGS Mode";
                th.Cells.Add(tcCol7);

                TableHeaderCell tcCol9 = new TableHeaderCell();
                tcCol9.Width = Unit.Percentage(8);
                tcCol9.Text = "By CSC-SPV Mode";
                th.Cells.Add(tcCol9);

                TableHeaderCell tcCol10 = new TableHeaderCell();
                tcCol10.Width = Unit.Percentage(12);
                tcCol10.Text = "By Demand Draft Mode (Exempted Cases)";
                th.Cells.Add(tcCol10);

                TableHeaderCell tcCol5 = new TableHeaderCell();
                tcCol5.Width = Unit.Percentage(11);
                tcCol5.Text = "Total " + context.Exams.Find(ExamId).Course.Code + " Fee Amount ";
                th.Cells.Add(tcCol5);

                tbl.Rows.Add(th);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ShowTableHeaderForwardedCandidateHeading()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                TableHeaderRow th = new TableHeaderRow();
                //th.CssClass = "head1";

                TableHeaderCell tcCol1 = new TableHeaderCell();
                tcCol1.Width = Unit.Percentage(15);
                tcCol1.ColumnSpan = 10;
                tcCol1.Text = "<br/>Forwarded Candidates Payment Report";
                th.Cells.Add(tcCol1);



                tbl.Rows.Add(th);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ShowTableData()
    {
        try
        {
            Int32 paymentStatus = Convert.ToInt32(enmPaymentStatus.Paid);
            Int32 onlinemode = Convert.ToInt32(enmPaymentMode.Online);
            Int32 cscmode = Convert.ToInt32(enmPaymentMode.CSCSPV);
            Int32 neftmode = Convert.ToInt32(enmPaymentMode.NEFTRTGS);
            Int32 demandmode = Convert.ToInt32(enmPaymentMode.DemandDraft);
            DataTable dt = new DataTable();
            CourseCategoryID = Convert.ToInt32(Request.QueryString["CourseCategory"]);
            ReportTypeID = Convert.ToInt32(Request.QueryString["ReportTypeId"]);
            ExamId = Convert.ToInt32(Request.QueryString["ExamId"]);
            applicantTypeID = Convert.ToInt32(Request.QueryString["TypeId"]);
            RegionalCentreID = Convert.ToInt32(Request.QueryString["RegionalCentreID"]);
            StringBuilder mySql = new StringBuilder();
            Int32 count = 0;// count1 = 0;
            string strHead = "";
            int i = 0;
            int totcount = 0; int feeamount = 0;
            int onlinecount = 0; int csccount = 0;
            int demandcount = 0; int neftcount = 0;
            //forwarded
            Int32 fcount = 0;
            int totfcount = 0; int feefamount = 0;
            int onlinefcount = 0; int cscfcount = 0;
            int demandfcount = 0; int neftfcount = 0;
            using (EConnectContext context = new EConnectContext())
            {
                if (RegionalCentreID != 0)
                {
                    var regionalcentre = context.RegionalCenters.Find(RegionalCentreID);
                    if (regionalcentre != null)
                    {
                        strHead += "</br> <b>Regional Centre :</b> " + GetInitCap(regionalcentre.Name.ToString());
                    }
                    else
                    {
                        strHead += "</br> <b>Regional Centre :</b> " + GetInitCap("All");
                    }
                }
                if (CourseCategoryID != 0)
                {
                    CourseCategory courseCat = context.CourseCategories.Find(CourseCategoryID);
                    if (courseCat != null)
                    {
                        strHead += "</br> <b>Course Cagtegory :</b> " + GetInitCap(courseCat.Name.ToString());
                    }
                }
                if (CourseID != 0)
                {
                    var courses = context.Courses.Find(CourseID);
                    if (courses != null)
                    {
                        strHead += "<br/><b>Course Name :</b>" + courses.Name.ToString();
                    }
                }
                if (applicantTypeID != 0)
                {
                    var application = context.ApplicationTypes.Find(applicantTypeID);
                    if (application != null)
                    {
                        strHead += "<br/><b>Application Type :</b>" + application.Name.ToString();
                    }
                }
                if (ExamId != 0)
                {
                    var Exam = context.Exams.Find(ExamId);
                    if (Exam != null)
                    {
                        strHead += "<br/><b>Exam Name :</b>" + Exam.Name + "(" + Exam.Course.Code + ")";
                    }
                }

                if (ReportTypeID == 1)
                {
                    if (ExamId != 0 && applicantTypeID != 0 && CourseCategoryID != 0)
                    {
                        var Exam = (from c in context.CertificateExamApplications
                                    join r in context.RegionalCenters on c.RegionalCenterID equals r.ID
                                    where c.ExamID == ExamId && c.CourseCategoryID == CourseCategoryID && c.FinalSubmitted == true && c.PaymentStatusID == paymentStatus
                                    group c by new { c.CourseID, c.RegionalCenterID } into g
                                    select new
                                    {
                                        RegId = g.Key.RegionalCenterID,
                                        CourseID = g.Key.CourseID,
                                    }).Distinct();
                        if (RegionalCentreID != 0)
                        {
                            Exam = Exam.Where(a => a.RegId == RegionalCentreID);
                        }
                        if (Exam.Count() > 0 && Exam != null)
                        {
                            ShowTableHeader();
                            foreach (var e in Exam)
                            {
                                //Int32 count1 = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT COUNT(Distinct Number) from Certificate_Exam_Application c where c.Regional_Center_ID = " + e.RegId + " and  c.Exam_ID = " + ExamId + " and c.Course_ID= " + e.CourseID + " and c.Payment_Status_ID = " + paymentStatus + " and c.Final_Submitted = 'true' and  c.Is_Exempted <> 1 ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //Int32 count2 = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT COUNT(Distinct Number) from Certificate_Exam_Application c where c.Regional_Center_ID = " + e.RegId + " and  c.Exam_ID = " + ExamId + " and c.Course_ID= " + e.CourseID + " and c.Payment_Status_ID = " + paymentStatus + " and c.Final_Submitted = 'true' and  c.Is_Exempted <> 1  and c.Number IN(SELECT distinct Number from Certificate_Exam_Application_History h where h.Course_ID= " + e.CourseID + " and h.Regional_Center_ID = " + e.RegId + ")", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //Int32 count3 = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("Select COUNT(distinct Number) FROM Certificate_Exam_Application_History h where h.Regional_Center_ID = " + e.RegId + " and h.Exam_ID = " + ExamId + " and h.Course_ID= " + e.CourseID + " and h.Number not in(SELECT distinct Number FROM Certificate_Exam_Application c where c.Exam_ID = " + ExamId + " and c.Course_ID= " + e.CourseID + " ) and h.Number not in(SELECT distinct Number FROM Certificate_Exam_Application_History h1 where h1.Exam_ID != " + ExamId + " and h1.Course_ID= " + e.CourseID + ")", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //count = (count1 + count3) - count2;
                                // ----------------------------------------------------------------------------------
                                //----at present with current exam_ID = 302

                                //Int32 count1 = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT COUNT(Distinct Number) from Certificate_Exam_Application c where c.Regional_Center_ID = " + e.RegId + " and  c.Exam_ID = " + ExamId + " and c.Course_ID= " + e.CourseID + " and c.Payment_Status_ID = " + paymentStatus + " and c.Final_Submitted = 'true' and  c.Is_Exempted <> 1  ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //November_2024
                                SqlParameter[] para1 ={
                                                                        new SqlParameter("@regId", e.RegId),
                                                                        new SqlParameter("@examId", ExamId),
                                                                        new SqlParameter("@courseID", e.CourseID),
                                                                        new SqlParameter("@paymentStatus", paymentStatus)
                                                                };
                                Int32 count1 = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT COUNT(Distinct Number) from Certificate_Exam_Application c where c.Regional_Center_ID = @regId and  c.Exam_ID = @examId and c.Course_ID= @courseID and c.Payment_Status_ID = @paymentStatus and c.Final_Submitted = 'true' and  c.Is_Exempted <> 1  ", new EConnect.Connections.SqlCon(),para1, CommandType.Text, false));

                                //-------------------------------------------------------------------------------------
                                //----at present forwarded to other exam_id( != 302 ) from current exam_id = 302

                                //Int32 count2 = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("Select COUNT(Distinct Number) FROM Certificate_Exam_Application_History h where h.Regional_Center_ID = " + e.RegId + " and h.Exam_ID = " + ExamId + " and h.Course_ID= " + e.CourseID + " and h.Payment_Status_ID = " + paymentStatus + " and h.Number in(SELECT Distinct Number FROM Certificate_Exam_Application c where c.Regional_Center_ID = " + e.RegId + " and  c.Exam_ID != " + ExamId + " and c.Course_ID= " + e.CourseID + " and c.Payment_Status_ID = " + paymentStatus + " ) ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //November_2024
                                Int32 count2 = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("Select COUNT(Distinct Number) FROM Certificate_Exam_Application_History h where h.Regional_Center_ID = @regId and h.Exam_ID = @examId and h.Course_ID= @courseID and h.Payment_Status_ID = @paymentStatus and h.Number in(SELECT Distinct Number FROM Certificate_Exam_Application c where c.Regional_Center_ID = @regId and  c.Exam_ID != @examId and c.Course_ID= @courseID and c.Payment_Status_ID = @paymentStatus ) ", new EConnect.Connections.SqlCon(),para1, CommandType.Text, false));


                                //------------------------------------------------------------------------------------------
                                //--- at present forwarded to current exam_id = 302 from other exam_id
                                //Int32 count3 = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT COUNT(Distinct Number) from Certificate_Exam_Application c where c.Regional_Center_ID = " + e.RegId + " and  c.Exam_ID = " + ExamId + " and c.Course_ID= " + e.CourseID + " and c.Payment_Status_ID = " + paymentStatus + " and c.Final_Submitted = 'true' and  c.Is_Exempted <> 1 and c.Number IN(SELECT Distinct Number from Certificate_Exam_Application_History h where h.Regional_Center_ID = " + e.RegId + " and  h.Exam_ID != " + ExamId + " and h.Course_ID= " + e.CourseID + " and h.Payment_Status_ID = " + paymentStatus + ")  ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //November_2024
                                Int32 count3 = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT COUNT(Distinct Number) from Certificate_Exam_Application c where c.Regional_Center_ID = @regId and  c.Exam_ID = @examId and c.Course_ID= @courseID and c.Payment_Status_ID = @paymentStatus and c.Final_Submitted = 'true' and  c.Is_Exempted <> 1 and c.Number IN(SELECT Distinct Number from Certificate_Exam_Application_History h where h.Regional_Center_ID = @regId and  h.Exam_ID != @examId and h.Course_ID= @courseID and h.Payment_Status_ID = @paymentStatus)  ", new EConnect.Connections.SqlCon(),para1, CommandType.Text, false));
                                //-------------------------------------------------------------------------------------------
                                count = (count1 + count2) - count3;
                                //count = context.CertificateExamApplications.Where(a => a.RegionalCenterID == e.RegId && a.ExamID == ExamId && a.CourseCategoryID == CourseCategoryID && a.CourseID == e.CourseID && a.FinalSubmitted == true && a.PaymentStatusID > paymentStatus && a.DemandNote.PaymentModeID != demandmode).Count();
                                totcount += count;
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;
                                TableCell tdRow = new TableCell();
                                tdRow.Width = Unit.Percentage(1);
                                tdRow.Text = i.ToString();
                                tdRow.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow);

                                TableCell tdRow1 = new TableCell();
                                tdRow1.Width = Unit.Percentage(8);
                                tdRow1.ColumnSpan = 3;
                                tdRow1.Text = context.RegionalCenters.Find(e.RegId).Name.ToString();
                                tdRow1.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow1);

                                TableCell tdRow2 = new TableCell();
                                tdRow2.Width = Unit.Percentage(8);
                                tdRow2.Text = count.ToString();
                                tdRow2.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow2);

                                TableCell tdRow3 = new TableCell();
                                tdRow3.Width = Unit.Percentage(8);
                                //tdRow3.Text = (from a in context.CertificateExamApplications
                                //               join d in context.DemandNotes on a.DemandNoteID equals d.ID
                                //               where a.RegionalCenterID == e.RegId && a.ExamID == ExamId && a.CourseCategoryID == CourseCategoryID && a.CourseID == e.CourseID && a.FinalSubmitted == true && a.PaymentStatusID == paymentStatus
                                //               && d.PaymentModeID == onlinemode && d.OnlineTransactionID != null && d.PaymentStatusID > paymentStatus
                                //               select a).Count().ToString();
                                //onlinecount += Convert.ToInt32(tdRow3.Text);
                                tdRow3.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow3);


                                TableCell tdRow15 = new TableCell();
                                tdRow15.Width = Unit.Percentage(8);
                                //tdRow15.Text = (from a in context.CertificateExamApplications
                                //                join d in context.DemandNotes on a.DemandNoteID equals d.ID
                                //                where d.PaymentModeID == neftmode && d.NEFTTransactionID != null && d.PaymentStatusID > paymentStatus && a.RegionalCenterID == e.RegId && a.ExamID == ExamId && a.CourseCategoryID == CourseCategoryID && a.CourseID == e.CourseID && a.FinalSubmitted == true && a.PaymentStatusID == paymentStatus
                                //                select a).Count().ToString();
                                //neftcount += Convert.ToInt32(tdRow15.Text);
                                tdRow15.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow15);

                                TableCell tdRow14 = new TableCell();
                                tdRow14.Width = Unit.Percentage(8);
                                //tdRow14.Text = (from a in context.CertificateExamApplications
                                //                join d in context.DemandNotes on a.DemandNoteID equals d.ID
                                //                where d.PaymentModeID == cscmode && d.CSCTransaction_ID != null && d.PaymentStatusID > paymentStatus && a.RegionalCenterID == e.RegId && a.ExamID == ExamId && a.CourseCategoryID == CourseCategoryID && a.CourseID == e.CourseID && a.FinalSubmitted == true && a.PaymentStatusID == paymentStatus
                                //                select a).Count().ToString();
                                //csccount += Convert.ToInt32(tdRow14.Text);
                                tdRow14.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow14);

                                TableCell tdRow16 = new TableCell();
                                tdRow16.Width = Unit.Percentage(8);
                                //tdRow16.Text = (from a in context.CertificateExamApplications
                                //                join d in context.DemandNotes on a.DemandNoteID equals d.ID
                                //                where d.PaymentModeID == demandmode && d.PaymentStatusID > paymentStatus && a.RegionalCenterID == e.RegId && a.ExamID == ExamId && a.CourseCategoryID == CourseCategoryID && a.CourseID == e.CourseID && a.FinalSubmitted == true && a.IsExempted == true
                                //                select a).Count().ToString();
                                //demandcount += Convert.ToInt32(tdRow16.Text);
                                tdRow16.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow16);

                                TableCell tdRow4 = new TableCell();
                                tdRow4.Width = Unit.Percentage(8);
                                tdRow4.Text = "" + (count * context.FeeDetails.Where(a => a.CourseID == e.CourseID).Select(k => k.FeeAmount).FirstOrDefault()).ToString();
                                feeamount += Convert.ToInt32(tdRow4.Text);
                                tdRow4.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow4);

                                tbl.Rows.Add(tr);
                            }

                            TableRow trNew = new TableRow();
                            if (i % 2 == 0)
                                trNew.CssClass = "gdalternate1";
                            else
                                trNew.CssClass = "gdrow1";
                            TableCell tdNewRow1 = new TableCell();
                            tdNewRow1.Width = Unit.Percentage(1);
                            tdNewRow1.Text = "";
                            tdNewRow1.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow1);

                            TableCell tdNewRow2 = new TableCell();
                            tdNewRow2.Width = Unit.Percentage(1);
                            tdNewRow2.Text = "";
                            tdNewRow2.ColumnSpan = 3;
                            tdNewRow2.HorizontalAlign = HorizontalAlign.Left;
                            trNew.Cells.Add(tdNewRow2);

                            TableCell tdNewRow3 = new TableCell();
                            tdNewRow3.Width = Unit.Percentage(1);
                            tdNewRow3.Text = "<b>Total :</b> &nbsp;&nbsp;" + totcount.ToString();
                            tdNewRow3.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow3);

                            TableCell tdNewRow5 = new TableCell();
                            tdNewRow5.Width = Unit.Percentage(1);
                            tdNewRow5.Text = "<b>Total :</b> &nbsp;&nbsp;" + onlinecount.ToString();
                            tdNewRow5.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow5);

                            TableCell tdNewRow6 = new TableCell();
                            tdNewRow6.Width = Unit.Percentage(1);
                            tdNewRow6.Text = "<b>Total :</b> &nbsp;&nbsp;" + neftcount.ToString();
                            tdNewRow6.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow6);

                            TableCell tdNewRow7 = new TableCell();
                            tdNewRow7.Width = Unit.Percentage(1);
                            tdNewRow7.Text = "<b>Total :</b> &nbsp;&nbsp;" + csccount.ToString();
                            tdNewRow7.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow7);

                            TableCell tdNewRow8 = new TableCell();
                            tdNewRow8.Width = Unit.Percentage(1);
                            tdNewRow8.Text = "<b>Total :</b> &nbsp;&nbsp;" + demandcount.ToString();
                            tdNewRow8.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow8);

                            TableCell tdNewRow4 = new TableCell();
                            tdNewRow4.Width = Unit.Percentage(1);
                            tdNewRow4.Text = "<b>Total :</b> &nbsp;&nbsp;" + feeamount.ToString();
                            tdNewRow4.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow4);

                            tbl.Rows.Add(trNew);
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
                else if (ReportTypeID == 2)
                {
                    //showing data  from main table
                    if (ExamId != 0 && applicantTypeID != 0 && CourseCategoryID != 0)
                    {
                        var Exam = (from c in context.CertificateExamApplications
                                    join r in context.RegionalCenters on c.RegionalCenterID equals r.ID
                                    where c.ExamID == ExamId && c.CourseCategoryID == CourseCategoryID && c.FinalSubmitted == true && c.PaymentStatusID == paymentStatus
                                    group c by new { c.CourseID, c.RegionalCenterID } into g
                                    select new
                                    {
                                        RegId = g.Key.RegionalCenterID,
                                        CourseID = g.Key.CourseID,
                                    }).Distinct();
                        if (RegionalCentreID != 0)
                        {
                            Exam = Exam.Where(a => a.RegId == RegionalCentreID);
                        }
                        if (Exam.Count() > 0 && Exam != null)
                        {
                            ShowTableHeader();
                            foreach (var e in Exam)
                            {
                                //November_2024
                                SqlParameter[] para2 = {   new SqlParameter("@regId", e.RegId),
                                                           new SqlParameter("@examId", ExamId),
                                                           new SqlParameter("@courseID", e.CourseID),
                                                           new SqlParameter("@paymentStatus", paymentStatus) };

                                SqlParameter[] para3 = {   new SqlParameter("@regId", e.RegId),
                                                           new SqlParameter("@examId", ExamId),
                                                           new SqlParameter("@courseID", e.CourseID) };

                                //Int32 count1 = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT COUNT(Distinct Number) from Certificate_Exam_Application c where c.Regional_Center_ID = " + e.RegId + " and  c.Exam_ID = " + ExamId + " and c.Course_ID= " + e.CourseID + " and c.Payment_Status_ID = " + paymentStatus + " and c.Final_Submitted = 'true' and  c.Is_Exempted <> 1 ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //November_2024
                                Int32 count1 = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT COUNT(Distinct Number) from Certificate_Exam_Application c where c.Regional_Center_ID =@regId and  c.Exam_ID = @examId and c.Course_ID= @courseID and c.Payment_Status_ID = @paymentStatus and c.Final_Submitted = 'true' and  c.Is_Exempted <> 1 ", new EConnect.Connections.SqlCon(),para2, CommandType.Text, false));

                                //Int32 count2 = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT COUNT(Distinct Number) from Certificate_Exam_Application c where c.Regional_Center_ID = " + e.RegId + " and  c.Exam_ID = " + ExamId + " and c.Course_ID= " + e.CourseID + " and c.Payment_Status_ID = " + paymentStatus + " and c.Final_Submitted = 'true' and  c.Is_Exempted <> 1  and c.Number IN(SELECT distinct Number from Certificate_Exam_Application_History)", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //November_2024
                                Int32 count2 = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT COUNT(Distinct Number) from Certificate_Exam_Application c where c.Regional_Center_ID = @regId and  c.Exam_ID = @examId and c.Course_ID= @courseID and c.Payment_Status_ID = @paymentStatus and c.Final_Submitted = 'true' and  c.Is_Exempted <> 1  and c.Number IN(SELECT distinct Number from Certificate_Exam_Application_History)", new EConnect.Connections.SqlCon(),para2, CommandType.Text, false));

                                //Int32 count3 = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("Select COUNT(distinct Number) FROM Certificate_Exam_Application_History h where h.Regional_Center_ID = " + e.RegId + " and h.Exam_ID = " + ExamId + " and h.Course_ID= " + e.CourseID + " and h.Number not in(SELECT Number FROM Certificate_Exam_Application c where c.Exam_ID = " + ExamId + " and c.Course_ID= " + e.CourseID + " ) and h.Number not in(SELECT distinct Number FROM Certificate_Exam_Application_History h1 where h1.Exam_ID = " + ExamId + " and h1.Course_ID= " + e.CourseID + ")", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //November_2024
                                Int32 count3 = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("Select COUNT(distinct Number) FROM Certificate_Exam_Application_History h where h.Regional_Center_ID = @regId and h.Exam_ID = @examId and h.Course_ID= @courseID and h.Number not in(SELECT Number FROM Certificate_Exam_Application c where c.Exam_ID = @examId and c.Course_ID= @courseID) and h.Number not in(SELECT distinct Number FROM Certificate_Exam_Application_History h1 where h1.Exam_ID = @examId and h1.Course_ID= @courseID)", new EConnect.Connections.SqlCon(),para3, CommandType.Text, false));

                                count = (count1 + count3) - count2;

                                //count = context.CertificateExamApplications.Where(a => a.RegionalCenterID == e.RegId && a.ExamID == ExamId && a.CourseCategoryID == CourseCategoryID && a.CourseID == e.CourseID && a.FinalSubmitted == true && a.PaymentStatusID > paymentStatus && a.DemandNote.PaymentModeID != demandmode).Count();
                                totcount += count;
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;
                                TableCell tdRow = new TableCell();
                                tdRow.Width = Unit.Percentage(1);
                                tdRow.Text = i.ToString();
                                tdRow.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow);

                                TableCell tdRow1 = new TableCell();
                                tdRow1.Width = Unit.Percentage(8);
                                tdRow1.ColumnSpan = 3;
                                tdRow1.Text = context.RegionalCenters.Find(e.RegId).Name.ToString();
                                tdRow1.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow1);

                                TableCell tdRow2 = new TableCell();
                                tdRow2.Width = Unit.Percentage(8);
                                tdRow2.Text = count.ToString();
                                tdRow2.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow2);

                                TableCell tdRow3 = new TableCell();
                                tdRow3.Width = Unit.Percentage(8);
                                //tdRow3.Text = (from a in context.CertificateExamApplications
                                //               join d in context.DemandNotes on a.DemandNoteID equals d.ID
                                //               where a.RegionalCenterID == e.RegId && a.ExamID == ExamId && a.CourseCategoryID == CourseCategoryID && a.CourseID == e.CourseID && a.FinalSubmitted == true && a.PaymentStatusID > paymentStatus
                                //               && d.PaymentModeID == onlinemode && d.OnlineTransactionID != null && d.PaymentStatusID > paymentStatus
                                //               select a).Count().ToString();
                                //onlinecount += Convert.ToInt32(tdRow3.Text);
                                tdRow3.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow3);


                                TableCell tdRow15 = new TableCell();
                                tdRow15.Width = Unit.Percentage(8);
                                //tdRow15.Text = (from a in context.CertificateExamApplications
                                //                join d in context.DemandNotes on a.DemandNoteID equals d.ID
                                //                where d.PaymentModeID == neftmode && d.NEFTTransactionID != null && d.PaymentStatusID > paymentStatus && a.RegionalCenterID == e.RegId && a.ExamID == ExamId && a.CourseCategoryID == CourseCategoryID && a.CourseID == e.CourseID && a.FinalSubmitted == true && a.PaymentStatusID > paymentStatus
                                //                select a).Count().ToString();
                                //neftcount += Convert.ToInt32(tdRow15.Text);
                                tdRow15.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow15);

                                TableCell tdRow14 = new TableCell();
                                tdRow14.Width = Unit.Percentage(8);
                                //tdRow14.Text = (from a in context.CertificateExamApplications
                                //                join d in context.DemandNotes on a.DemandNoteID equals d.ID
                                //                where d.PaymentModeID == cscmode && d.CSCTransaction_ID != null && d.PaymentStatusID > paymentStatus && a.RegionalCenterID == e.RegId && a.ExamID == ExamId && a.CourseCategoryID == CourseCategoryID && a.CourseID == e.CourseID && a.FinalSubmitted == true && a.PaymentStatusID > paymentStatus
                                //                select a).Count().ToString();
                                //csccount += Convert.ToInt32(tdRow14.Text);
                                tdRow14.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow14);

                                TableCell tdRow16 = new TableCell();
                                tdRow16.Width = Unit.Percentage(8);
                                //tdRow16.Text = (from a in context.CertificateExamApplications
                                //                join d in context.DemandNotes on a.DemandNoteID equals d.ID
                                //                where d.PaymentModeID == demandmode && d.PaymentStatusID > paymentStatus && a.RegionalCenterID == e.RegId && a.ExamID == ExamId && a.CourseCategoryID == CourseCategoryID && a.CourseID == e.CourseID && a.FinalSubmitted == true && a.IsExempted == true
                                //                select a).Count().ToString();
                                //demandcount += Convert.ToInt32(tdRow16.Text);
                                tdRow16.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow16);

                                TableCell tdRow4 = new TableCell();
                                tdRow4.Width = Unit.Percentage(8);
                                tdRow4.Text = "" + (count * context.FeeDetails.Where(a => a.CourseID == e.CourseID).Select(k => k.FeeAmount).FirstOrDefault()).ToString();
                                feeamount += Convert.ToInt32(tdRow4.Text);
                                tdRow4.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow4);

                                tbl.Rows.Add(tr);
                            }
                            TableRow trNew = new TableRow();
                            if (i % 2 == 0)
                                trNew.CssClass = "gdalternate1";
                            else
                                trNew.CssClass = "gdrow1";
                            TableCell tdNewRow1 = new TableCell();
                            tdNewRow1.Width = Unit.Percentage(1);
                            tdNewRow1.Text = "";
                            tdNewRow1.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow1);

                            TableCell tdNewRow2 = new TableCell();
                            tdNewRow2.Width = Unit.Percentage(1);
                            tdNewRow2.Text = "";
                            tdNewRow2.ColumnSpan = 3;
                            tdNewRow2.HorizontalAlign = HorizontalAlign.Left;
                            trNew.Cells.Add(tdNewRow2);

                            TableCell tdNewRow3 = new TableCell();
                            tdNewRow3.Width = Unit.Percentage(1);
                            tdNewRow3.Text = "<b>Total :</b> &nbsp;&nbsp;" + totcount.ToString();
                            tdNewRow3.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow3);

                            TableCell tdNewRow5 = new TableCell();
                            tdNewRow5.Width = Unit.Percentage(1);
                            tdNewRow5.Text = "<b>Total :</b> &nbsp;&nbsp;" + onlinecount.ToString();
                            tdNewRow5.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow5);

                            TableCell tdNewRow6 = new TableCell();
                            tdNewRow6.Width = Unit.Percentage(1);
                            tdNewRow6.Text = "<b>Total :</b> &nbsp;&nbsp;" + neftcount.ToString();
                            tdNewRow6.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow6);

                            TableCell tdNewRow7 = new TableCell();
                            tdNewRow7.Width = Unit.Percentage(1);
                            tdNewRow7.Text = "<b>Total :</b> &nbsp;&nbsp;" + csccount.ToString();
                            tdNewRow7.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow7);

                            TableCell tdNewRow8 = new TableCell();
                            tdNewRow8.Width = Unit.Percentage(1);
                            tdNewRow8.Text = "<b>Total :</b> &nbsp;&nbsp;" + demandcount.ToString();
                            tdNewRow8.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow8);

                            TableCell tdNewRow4 = new TableCell();
                            tdNewRow4.Width = Unit.Percentage(1);
                            tdNewRow4.Text = "<b>Total :</b> &nbsp;&nbsp;" + feeamount.ToString();
                            tdNewRow4.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow4);

                            tbl.Rows.Add(trNew);
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

                    //showing data from history table
                    if (ExamId != 0 && applicantTypeID != 0 && CourseCategoryID != 0)
                    {

                        if (RegionalCentreID != 0)
                        {
                            //November_2024
                            SqlParameter[] para4 = {  new SqlParameter("@examId", ExamId),
                                                     new SqlParameter("@courseCategoryID", CourseCategoryID),
                                                    new SqlParameter("@regionalCentreID",RegionalCentreID),
                                                    new SqlParameter("@paymentStatus", paymentStatus) };
                            //dt = EConnect.Utils.Data.DbUtility.GetDataTable("select c.Regional_Center_ID, c.Course_ID,c.New_Exam_ID , c.Exam_ID from  Certificate_Exam_Application_history c where c.Exam_ID = " + ExamId + " and c.Course_Category_ID = " + CourseCategoryID + " and c.Final_Submitted = 'true' and c.Regional_Center_ID=" + RegionalCentreID + " and c.Remarks_ID is not null and c.Exam_ID is not null and c.New_Exam_ID is not null and c.Payment_Status_ID = " + paymentStatus + " group by c.Regional_Center_ID, c.Course_ID, c.New_Exam_ID, c.Exam_ID ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
                            //November_2024
                            dt = EConnect.Utils.Data.DbUtility.GetDataTable("select c.Regional_Center_ID, c.Course_ID,c.New_Exam_ID , c.Exam_ID from  Certificate_Exam_Application_history c where c.Exam_ID = @examId and c.Course_Category_ID = @courseCategoryID and c.Final_Submitted = 'true' and c.Regional_Center_ID=@regionalCentreID and c.Remarks_ID is not null and c.Exam_ID is not null and c.New_Exam_ID is not null and c.Payment_Status_ID = @paymentStatus group by c.Regional_Center_ID, c.Course_ID, c.New_Exam_ID, c.Exam_ID ", new EConnect.Connections.SqlCon(),para4, CommandType.Text, false);
                        }
                        else
                        {
                            //November_2024
                            SqlParameter[] para5 = {  new SqlParameter("@examId", ExamId),
                                                      new SqlParameter("@courseCategoryID", CourseCategoryID),
                                                      new SqlParameter("@paymentStatus", paymentStatus) };
                            //dt = EConnect.Utils.Data.DbUtility.GetDataTable("select c.Regional_Center_ID, c.Course_ID,c.New_Exam_ID, c.Exam_ID from  Certificate_Exam_Application_history c where c.Exam_ID = " + ExamId + " and c.Course_Category_ID = " + CourseCategoryID + " and c.Final_Submitted = 'true' and c.Remarks_ID is not null and c.Exam_ID is not null and c.New_Exam_ID is not null and c.Payment_Status_ID = " + paymentStatus + " group by c.Regional_Center_ID, c.Course_ID,c.New_Exam_ID, c.Exam_ID ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
                            //November_2024
                            dt = EConnect.Utils.Data.DbUtility.GetDataTable("select c.Regional_Center_ID, c.Course_ID,c.New_Exam_ID, c.Exam_ID from  Certificate_Exam_Application_history c where c.Exam_ID = @examId and c.Course_Category_ID = @courseCategoryID and c.Final_Submitted = 'true' and c.Remarks_ID is not null and c.Exam_ID is not null and c.New_Exam_ID is not null and c.Payment_Status_ID = @paymentStatus group by c.Regional_Center_ID, c.Course_ID,c.New_Exam_ID, c.Exam_ID ", new EConnect.Connections.SqlCon(),para5, CommandType.Text, false);
                        }

                        if (dt.Rows.Count > 0)
                        {
                            ShowTableHeaderForwardedCandidateHeading();
                            ShowTableHeader1();
                            foreach (DataRow dr in dt.Rows)
                            {
                                //November_2024
                                SqlParameter[] para6 = {   new SqlParameter("@regionalCenterID", dr[0]),
                                                                                new SqlParameter("@examId", ExamId),
                                                                                new SqlParameter("@courseCategoryID", CourseCategoryID),
                                                                                new SqlParameter("@courseID", dr[1]),
                                                                                new SqlParameter("@newExamID", dr[2]),
                                                                                new SqlParameter("@paymentStatus", paymentStatus),
                                                                                new SqlParameter("@demandmode", demandmode) };
                                //fcount = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from Certificate_Exam_Application_history c , Demand_note d where c.Demand_Note_Id = d.ID and c.Regional_Center_ID = " + dr[0] + "  and c.Exam_ID = " + ExamId + " and c.Course_Category_ID = " + CourseCategoryID + " and c.Course_ID = " + dr[1] + " and c.Remarks_ID is not null and c.New_Exam_ID = " + dr[2] + " and c.Final_Submitted = 'true' and c.Payment_Status_ID = " + paymentStatus + " and d.Payment_Mode_ID != " + demandmode, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //November_2024
                                fcount = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from Certificate_Exam_Application_history c , Demand_note d where c.Demand_Note_Id = d.ID and c.Regional_Center_ID = @regionalCenterID  and c.Exam_ID = @examId and c.Course_Category_ID = @courseCategoryID and c.Course_ID = @courseID and c.Remarks_ID is not null and c.New_Exam_ID = @newExamID and c.Final_Submitted = 'true' and c.Payment_Status_ID = @paymentStatus and d.Payment_Mode_ID != @demandmode", new EConnect.Connections.SqlCon(),para6, CommandType.Text, false));

                                totfcount += fcount;
                                TableRow tr = new TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;
                                TableCell tdRow = new TableCell();
                                tdRow.Width = Unit.Percentage(1);
                                tdRow.Text = i.ToString();
                                tdRow.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow);

                                TableCell tdRow1 = new TableCell();
                                tdRow1.Width = Unit.Percentage(8);

                                //November_2024
                                SqlParameter[] para7 = { new SqlParameter("@id", dr[0]) };

                                //tdRow1.Text = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Name from Regional_Center d where d.ID = " + dr[0], new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //November_2024
                                tdRow1.Text = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Name from Regional_Center d where d.ID = @id ", new EConnect.Connections.SqlCon(),para7, CommandType.Text, false));

                                tdRow1.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow1);

                                TableCell tdRow61 = new TableCell();
                                tdRow61.Width = Unit.Percentage(8);

                                //November_2024
                                SqlParameter[] para8 = { new SqlParameter("@id", dr[3]) };

                                //tdRow61.Text = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Name from Exam d where d.ID= " + dr[3], new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //November_2024
                                tdRow61.Text = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Name from Exam d where d.ID= @id ", new EConnect.Connections.SqlCon(),para8, CommandType.Text, false));
                                
                                tdRow61.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow61);

                                TableCell tdRow51 = new TableCell();
                                tdRow51.Width = Unit.Percentage(8);


                                //November_2024
                                SqlParameter[] para9 = { new SqlParameter("@id", dr[2]) };

                                //tdRow51.Text = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Name from Exam d where d.ID= " + dr[2], new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //November_2024
                                tdRow51.Text = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Name from Exam d where d.ID= @id", new EConnect.Connections.SqlCon(),para9, CommandType.Text, false));
                                
                                tdRow51.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow51);

                                TableCell tdRow2 = new TableCell();
                                tdRow2.Width = Unit.Percentage(8);
                                tdRow2.Text = fcount.ToString();
                                tdRow2.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow2);

                                TableCell tdRow3 = new TableCell();
                                tdRow3.Width = Unit.Percentage(8);



                                //November_2024
                                SqlParameter[] para10 = {  new SqlParameter("@regionalCenterID",dr[0]),
                                                                                        new SqlParameter("@examId", ExamId),
                                                                                        new SqlParameter("@courseCategoryID", CourseCategoryID),
                                                                                        new SqlParameter("@courseID", dr[1]),
                                                                                        new SqlParameter("@newExamID", dr[2]),
                                                                                        new SqlParameter("@paymentStatus", paymentStatus),
                                                                                        new SqlParameter("@onlineMode", onlinemode) };


                                //tdRow3.Text = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from Certificate_Exam_Application_history c , Demand_note d where c.Demand_Note_Id = d.ID and c.Regional_Center_ID = " + dr[0] + "  and c.Exam_ID = " + ExamId + " and c.Course_Category_ID = " + CourseCategoryID + " and c.Course_ID = " + dr[1] + " and c.Remarks_ID is not null and c.New_Exam_ID = " + dr[2] + " and c.Final_Submitted = 'true' and c.Payment_Status_ID = " + paymentStatus + " and d.Payment_Mode_ID = " + onlinemode + " and d.Online_Transaction_ID is not null and d.Status_ID = " + paymentStatus, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //November_2024
                                tdRow3.Text = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from Certificate_Exam_Application_history c , Demand_note d where c.Demand_Note_Id = d.ID and c.Regional_Center_ID = @regionalCenterID  and c.Exam_ID = @examId and c.Course_Category_ID = @courseCategoryID and c.Course_ID = @courseID and c.Remarks_ID is not null and c.New_Exam_ID = @newExamID and c.Final_Submitted = 'true' and c.Payment_Status_ID = @paymentStatus and d.Payment_Mode_ID = @onlineMode and d.Online_Transaction_ID is not null and d.Status_ID =@paymentStatus ", new EConnect.Connections.SqlCon(),para10, CommandType.Text, false));

                                onlinefcount += Convert.ToInt32(tdRow3.Text);
                                tdRow3.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow3);


                                TableCell tdRow15 = new TableCell();
                                tdRow15.Width = Unit.Percentage(8);


                                //November_2024
                                SqlParameter[] para11 = {   new SqlParameter("@regionalCenterID",dr[0]),
                                                            new SqlParameter("@examId", ExamId),
                                                            new SqlParameter("@courseCategoryID", CourseCategoryID),
                                                            new SqlParameter("@courseID", dr[1]),
                                                             new SqlParameter("@newExamID", dr[2]),
                                                             new SqlParameter("@paymentStatus", paymentStatus),
                                                             new SqlParameter("@neftMode", neftmode) };

                                //tdRow15.Text = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from Certificate_Exam_Application_history c , Demand_note d where c.Demand_Note_Id = d.ID and c.Regional_Center_ID = " + dr[0] + "  and c.Exam_ID = " + ExamId + " and c.Course_Category_ID = " + CourseCategoryID + " and c.Course_ID = " + dr[1] + "and c.Remarks_ID is not null and c.New_Exam_ID = " + dr[2] + " and c.Final_Submitted = 'true' and c.Payment_Status_ID = " + paymentStatus + " and d.Payment_Mode_ID = " + neftmode + " and d.NEFT_Transaction_ID is not null and d.Status_ID = " + paymentStatus, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //November_2024
                                tdRow15.Text = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from Certificate_Exam_Application_history c , Demand_note d where c.Demand_Note_Id = d.ID and c.Regional_Center_ID = @regionalCenterID  and c.Exam_ID = @examId and c.Course_Category_ID = @courseCategoryID and c.Course_ID = @courseID and c.Remarks_ID is not null and c.New_Exam_ID = @newExamID and c.Final_Submitted = 'true' and c.Payment_Status_ID = @paymentStatus and d.Payment_Mode_ID = @neftMode and d.NEFT_Transaction_ID is not null and d.Status_ID = @paymentStatus", new EConnect.Connections.SqlCon(),para11, CommandType.Text, false));

                                neftfcount += Convert.ToInt32(tdRow15.Text);
                                tdRow15.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow15);

                                TableCell tdRow14 = new TableCell();
                                tdRow14.Width = Unit.Percentage(8);


                                //November_2024
                                SqlParameter[] para12 = {   new SqlParameter("@regionalCenterID",dr[0]),
                                                            new SqlParameter("@examId", ExamId),
                                                            new SqlParameter("@courseCategoryID", CourseCategoryID),
                                                            new SqlParameter("@courseID", dr[1]),
                                                            new SqlParameter("@newExamID", dr[2]),
                                                            new SqlParameter("@paymentStatus", paymentStatus),
                                                            new SqlParameter("@cscMode", cscmode) };

                                //tdRow14.Text = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from Certificate_Exam_Application_history c , Demand_note d where c.Demand_Note_Id = d.ID and c.Regional_Center_ID = " + dr[0] + "  and c.Exam_ID = " + ExamId + " and c.Course_Category_ID = " + CourseCategoryID + " and c.Course_ID = " + dr[1] + "and c.Remarks_ID is not null and c.New_Exam_ID = " + dr[2] + " and c.Final_Submitted = 'true' and c.Payment_Status_ID = " + paymentStatus + " and d.Payment_Mode_ID = " + cscmode + " and d.CSC_Transaction_ID is not null and d.Status_ID = " + paymentStatus, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //November_2024
                                tdRow14.Text = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from Certificate_Exam_Application_history c , Demand_note d where c.Demand_Note_Id = d.ID and c.Regional_Center_ID = @regionalCenterID  and c.Exam_ID = @examId and c.Course_Category_ID = @courseCategoryID and c.Course_ID = @courseID and c.Remarks_ID is not null and c.New_Exam_ID = @newExamID and c.Final_Submitted = 'true' and c.Payment_Status_ID = @paymentStatus and d.Payment_Mode_ID = @cscMode and d.CSC_Transaction_ID is not null and d.Status_ID = @paymentStatus", new EConnect.Connections.SqlCon(),para12, CommandType.Text, false));

                                cscfcount += Convert.ToInt32(tdRow14.Text);
                                tdRow14.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow14);

                                TableCell tdRow16 = new TableCell();
                                tdRow16.Width = Unit.Percentage(8);


                                //November_2024
                                SqlParameter[] para13 = {    new SqlParameter("@regionalCenterID",dr[0]),
                                                             new SqlParameter("@examId", ExamId),
                                                              new SqlParameter("@courseCategoryID", CourseCategoryID),
                                                              new SqlParameter("@courseID", dr[1]),
                                                             new SqlParameter("@newExamID", dr[2]),
                                                             new SqlParameter("@paymentStatus", paymentStatus),
                                                             new SqlParameter("@demandMode", demandmode) };

                                //tdRow16.Text = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from Certificate_Exam_Application_history c , Demand_note d where c.Demand_Note_Id = d.ID and c.Regional_Center_ID = " + dr[0] + "  and c.Exam_ID = " + ExamId + " and c.Course_Category_ID = " + CourseCategoryID + " and c.Course_ID = " + dr[1] + "and c.Remarks_ID is not null and c.New_Exam_ID = " + dr[2] + " and c.Final_Submitted = 'true' and c.Payment_Status_ID = " + paymentStatus + " and d.Payment_Mode_ID = " + demandmode + " and d.DD_Transaction_ID is not null and d.Status_ID = " + paymentStatus, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //November_2024
                                tdRow16.Text = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from Certificate_Exam_Application_history c , Demand_note d where c.Demand_Note_Id = d.ID and c.Regional_Center_ID = @regionalCenterID  and c.Exam_ID = @examId and c.Course_Category_ID = @courseCategoryID and c.Course_ID = @courseID and c.Remarks_ID is not null and c.New_Exam_ID = @newExamID and c.Final_Submitted = 'true' and c.Payment_Status_ID = @paymentStatus and d.Payment_Mode_ID = @demandMode and d.DD_Transaction_ID is not null and d.Status_ID = @paymentStatus", new EConnect.Connections.SqlCon(),para13, CommandType.Text, false));

                                demandfcount += Convert.ToInt32(tdRow16.Text);
                                tdRow16.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow16);

                                TableCell tdRow4 = new TableCell();
                                tdRow4.Width = Unit.Percentage(8);


                                //November_2024
                                SqlParameter[] para14 = { new SqlParameter("@courseID", dr[1]) };

                                //tdRow4.Text = "" + (fcount * Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail d where d.Course_ID = " + dr[1], new EConnect.Connections.SqlCon(), null, CommandType.Text, false))).ToString();
                                //November_2024
                                tdRow4.Text = "" + (fcount * Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail d where d.Course_ID = @courseID ", new EConnect.Connections.SqlCon(),para14, CommandType.Text, false))).ToString();

                                feefamount += Convert.ToInt32(tdRow4.Text);
                                tdRow4.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow4);

                                tbl.Rows.Add(tr);
                            }

                            //forwarded count
                            TableRow trNew = new TableRow();
                            if (i % 2 == 0)
                                trNew.CssClass = "gdalternate1";
                            else
                                trNew.CssClass = "gdrow1";
                            TableCell tdNewRow1 = new TableCell();
                            tdNewRow1.Width = Unit.Percentage(1);
                            tdNewRow1.Text = "";
                            tdNewRow1.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow1);

                            TableCell tdNewRow52 = new TableCell();
                            tdNewRow52.Width = Unit.Percentage(1);
                            tdNewRow52.Text = "";
                            tdNewRow52.HorizontalAlign = HorizontalAlign.Left;
                            trNew.Cells.Add(tdNewRow52);

                            TableCell tdNewRow2 = new TableCell();
                            tdNewRow2.Width = Unit.Percentage(1);
                            tdNewRow2.Text = "";
                            tdNewRow2.HorizontalAlign = HorizontalAlign.Left;
                            trNew.Cells.Add(tdNewRow2);

                            TableCell tdNewRow42 = new TableCell();
                            tdNewRow42.Width = Unit.Percentage(1);
                            tdNewRow42.Text = "";
                            tdNewRow42.HorizontalAlign = HorizontalAlign.Left;
                            trNew.Cells.Add(tdNewRow42);

                            TableCell tdNewRow3 = new TableCell();
                            tdNewRow3.Width = Unit.Percentage(1);
                            tdNewRow3.Text = "<b>Total :</b> &nbsp;&nbsp;" + totfcount.ToString();
                            tdNewRow3.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow3);

                            TableCell tdNewRow5 = new TableCell();
                            tdNewRow5.Width = Unit.Percentage(1);
                            tdNewRow5.Text = "<b>Total :</b> &nbsp;&nbsp;" + onlinefcount.ToString();
                            tdNewRow5.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow5);

                            TableCell tdNewRow6 = new TableCell();
                            tdNewRow6.Width = Unit.Percentage(1);
                            tdNewRow6.Text = "<b>Total :</b> &nbsp;&nbsp;" + neftfcount.ToString();
                            tdNewRow6.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow6);

                            TableCell tdNewRow7 = new TableCell();
                            tdNewRow7.Width = Unit.Percentage(1);
                            tdNewRow7.Text = "<b>Total :</b> &nbsp;&nbsp;" + cscfcount.ToString();
                            tdNewRow7.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow7);

                            TableCell tdNewRow8 = new TableCell();
                            tdNewRow8.Width = Unit.Percentage(1);
                            tdNewRow8.Text = "<b>Total :</b> &nbsp;&nbsp;" + demandfcount.ToString();
                            tdNewRow8.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow8);

                            TableCell tdNewRow94 = new TableCell();
                            tdNewRow94.Width = Unit.Percentage(1);
                            tdNewRow94.Text = "<b>Total :</b> &nbsp;&nbsp;" + feefamount.ToString();
                            tdNewRow94.HorizontalAlign = HorizontalAlign.Right;
                            trNew.Cells.Add(tdNewRow94);

                            tbl.Rows.Add(trNew);



                            TableRow trNew11 = new TableRow();
                            TableCell tdNewRow44 = new TableCell();
                            tdNewRow44.Width = Unit.Percentage(1);
                            tdNewRow44.Text = "";
                            tdNewRow44.ColumnSpan = 9;
                            tdNewRow44.Height = Unit.Pixel(10);
                            tdNewRow44.HorizontalAlign = HorizontalAlign.Right;
                            trNew11.Cells.Add(tdNewRow44);

                            tbl.Rows.Add(trNew11);


                            //Grand total
                            TableRow trNew1 = new TableRow();
                            if (i % 2 == 0)
                                trNew1.CssClass = "gdalternate1";
                            else
                                trNew1.CssClass = "gdrow1";

                            TableCell tdNewRow11 = new TableCell();
                            tdNewRow11.Width = Unit.Percentage(1);
                            tdNewRow11.Text = "<b>Grand Total</b>";
                            tdNewRow11.ColumnSpan = 4;
                            tdNewRow11.HorizontalAlign = HorizontalAlign.Center;
                            trNew1.Cells.Add(tdNewRow11);



                            TableCell tdNewRow31 = new TableCell();
                            tdNewRow31.Width = Unit.Percentage(1);
                            tdNewRow31.Text = "<b>Total :</b> &nbsp;&nbsp;" + (totcount + totfcount).ToString();
                            tdNewRow31.HorizontalAlign = HorizontalAlign.Right;
                            trNew1.Cells.Add(tdNewRow31);

                            TableCell tdNewRow51 = new TableCell();
                            tdNewRow51.Width = Unit.Percentage(1);
                            tdNewRow51.Text = "<b>Total :</b> &nbsp;&nbsp;" + (onlinecount + onlinefcount).ToString();
                            tdNewRow51.HorizontalAlign = HorizontalAlign.Right;
                            trNew1.Cells.Add(tdNewRow51);

                            TableCell tdNewRow61 = new TableCell();
                            tdNewRow61.Width = Unit.Percentage(1);
                            tdNewRow61.Text = "<b>Total :</b> &nbsp;&nbsp;" + (neftcount + neftfcount).ToString();
                            tdNewRow61.HorizontalAlign = HorizontalAlign.Right;
                            trNew1.Cells.Add(tdNewRow61);

                            TableCell tdNewRow71 = new TableCell();
                            tdNewRow71.Width = Unit.Percentage(1);
                            tdNewRow71.Text = "<b>Total :</b> &nbsp;&nbsp;" + (csccount + cscfcount).ToString();
                            tdNewRow71.HorizontalAlign = HorizontalAlign.Right;
                            trNew1.Cells.Add(tdNewRow71);

                            TableCell tdNewRow81 = new TableCell();
                            tdNewRow81.Width = Unit.Percentage(1);
                            tdNewRow81.Text = "<b>Total :</b> &nbsp;&nbsp;" + (demandcount + demandfcount).ToString();
                            tdNewRow81.HorizontalAlign = HorizontalAlign.Right;
                            trNew1.Cells.Add(tdNewRow81);

                            TableCell tdNewRow41 = new TableCell();
                            tdNewRow41.Width = Unit.Percentage(1);
                            tdNewRow41.Text = "<b>Total :</b> &nbsp;&nbsp;" + (feeamount + feefamount).ToString();
                            tdNewRow41.HorizontalAlign = HorizontalAlign.Right;
                            trNew1.Cells.Add(tdNewRow41);
                            tbl.Rows.Add(trNew1);
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No Record Found for Forwarded Candidates.";
                        }
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found for Forwarded Candidates.";
                    }

                }
            };
            LblRptSubHeader.Text = strHead;
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
            ShowTableData();
            divReportData.Controls.Add(tbl);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=RegionalCenterPaymentReport.xls");
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