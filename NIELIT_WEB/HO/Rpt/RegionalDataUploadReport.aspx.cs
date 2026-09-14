using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class ReportPgae : BasePage
{
    Table tbl = new Table(); 
    UserType loginUserType;    
    Int32 applicantTypeID = 0;
    Int32 CourseID = 0;
    Int64 ExamId = 0;
    Int32 CourseCategoryID = 0;
    Int32 currentRoleId = 0;    
    Int32 RegionalCentreID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/RegionalDataUpload.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            if (!Page.IsPostBack)
            {
                if ((!string.IsNullOrEmpty(Request.QueryString["RegionalCentreID"])) && (!string.IsNullOrEmpty(Request.QueryString["CourseCategory"])) && (!string.IsNullOrEmpty(Request.QueryString["CourseId"])) && (!string.IsNullOrEmpty(Request.QueryString["ExamId"])) && (!string.IsNullOrEmpty(Request.QueryString["TypeId"])))
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
        TableHeaderRow th = new TableHeaderRow();
        th.CssClass = "head1";

        TableHeaderCell tcCol1 = new TableHeaderCell();
        tcCol1.Width = Unit.Percentage(3);
        tcCol1.Text = "#";
        th.Cells.Add(tcCol1);

        TableHeaderCell tcCol2 = new TableHeaderCell();
        tcCol2.Width = Unit.Percentage(10);
        tcCol2.Text = "Regional Centre Name";
        th.Cells.Add(tcCol2);

        TableHeaderCell tcCol3 = new TableHeaderCell();
        tcCol3.Width = Unit.Percentage(15);
        tcCol3.Text = "Total Candidates Applied";
        th.Cells.Add(tcCol3);

        TableHeaderCell tcCol4 = new TableHeaderCell();
        tcCol4.Width = Unit.Percentage(18);
        tcCol4.Text = "Admit Card Data Uploaded for Candidates";
        th.Cells.Add(tcCol4);

        TableHeaderCell tcCol5 = new TableHeaderCell();
        tcCol5.Width = Unit.Percentage(15);
        tcCol5.Text = "Result Declared for Candidates";
        th.Cells.Add(tcCol5);

        tbl.Rows.Add(th);
        
    }
    protected void ShowTableData()
    {
        try
        {
            Int32[] paymentstatus = { Convert.ToInt32(enmPaymentStatus.Paid), Convert.ToInt32(enmPaymentStatus.PaidButNotVerified) };
            Int32 applicationstatusId = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing);
            CourseCategoryID = Convert.ToInt32(Request.QueryString["CourseCategory"]);
            CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
            ExamId = Convert.ToInt32(Request.QueryString["ExamId"]);
            applicantTypeID = Convert.ToInt32(Request.QueryString["TypeId"]);
            RegionalCentreID = Convert.ToInt32(Request.QueryString["RegionalCentreID"]);
            StringBuilder mySql = new StringBuilder();
            string strHead = "";
            int i = 0;
            int totcount = 0; int uploadedcount = 0; int resultcount = 0;
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
                        strHead += "<br/><b>Exam Name :</b>" + Exam.Name;
                    }
                }
                if (ExamId != 0 && CourseCategoryID != 0 && CourseID != 0)
                {
                    totcount = uploadedcount = resultcount = 0;
                    var Exam = (from c in context.CertificateExamApplications
                                join r in context.RegionalCenters on c.RegionalCenterID equals r.ID
                                where c.ExamID == ExamId && c.CourseID == CourseID && c.CourseCategoryID == CourseCategoryID && c.FinalSubmitted == true 
                                && (c.PaymentStatusID == (int)(enmPaymentStatus.Paid) || c.PaymentStatusID == (int)(enmPaymentStatus.PaidButNotVerified ))
                                select new
                                {
                                    RegID=c.RegionalCenterID,
                                    RegCentreName = c.RegionalCenter.Name
                                }).Distinct();
                    if(RegionalCentreID !=0)
                    {
                        Exam = Exam.Where(a => a.RegID == RegionalCentreID);
                    }
                   
                    if (Exam.Count() > 0 && Exam != null)
                    {
                        ShowTableHeader();
                        foreach (var e in Exam)
                        {
                            var count = context.CertificateExamApplications.Where(a => a.RegionalCenterID == e.RegID && a.ExamID == ExamId && a.CourseCategoryID == CourseCategoryID && a.CourseID == CourseID && a.FinalSubmitted == true && (a.PaymentStatusID == (int)(enmPaymentStatus.Paid) || a.PaymentStatusID == (int)(enmPaymentStatus.PaidButNotVerified)) //&& paymentstatus.Contains(a.PaymentStatusID)  
                                && context.DemandNotes.Any(s => s.ID == a.DemandNoteID && (s.PaymentStatusID == 2 || s.PaymentStatusID == 4) && (s.OnlineTransactionID != null || s.NEFTTransactionID != null || s.CSCTransaction_ID != null))).Count();

                            var AdmitCardIssued = context.CertificateExamApplications.Where(a => a.RollNumber != null && a.RegionalCenterID == e.RegID && a.ExamID == ExamId && a.CourseCategoryID == CourseCategoryID && a.CourseID == CourseID && a.FinalSubmitted == true && (a.PaymentStatusID == (int)(enmPaymentStatus.Paid) || a.PaymentStatusID == (int)(enmPaymentStatus.PaidButNotVerified)) //&& paymentstatus.Contains(a.PaymentStatusID)
                                ).Select(a => a.RollNumber).Count();

                            var Result = context.CertificateExamApplications.Where(a => a.ResultGradeID != null && a.RegionalCenterID == e.RegID && a.ExamID == ExamId && a.CourseCategoryID == CourseCategoryID && a.CourseID == CourseID && a.FinalSubmitted == true && (a.PaymentStatusID == (int)(enmPaymentStatus.Paid) || a.PaymentStatusID == (int)(enmPaymentStatus.PaidButNotVerified)) //&& paymentstatus.Contains(a.PaymentStatusID)
                                ).Select(a => a.ResultGradeID).Count();
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
                            tdRow1.Text =e.RegCentreName.ToString();
                            tdRow1.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow1);

                            TableCell tdRow2 = new TableCell();
                            tdRow2.Width = Unit.Percentage(8);
                            tdRow2.Text = count.ToString();
                            tdRow2.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow2);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(8);
                            tdRow3.Text = AdmitCardIssued.ToString();
                            tdRow3.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow3);

                            TableCell tdRow4 = new TableCell();
                            tdRow4.Width = Unit.Percentage(8);
                            tdRow4.Text = Result.ToString();
                            tdRow4.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow4);
                            totcount += count;
                            uploadedcount += AdmitCardIssued;
                            resultcount += Result;
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
                        tdNewRow2.HorizontalAlign = HorizontalAlign.Left;
                        trNew.Cells.Add(tdNewRow2);

                        TableCell tdNewRow3 = new TableCell();
                        tdNewRow3.Width = Unit.Percentage(1);
                        tdNewRow3.Text = "<b>Total :</b> &nbsp;&nbsp;" + totcount.ToString();
                        tdNewRow3.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow3);

                        TableCell tdNewRow4 = new TableCell();
                        tdNewRow4.Width = Unit.Percentage(1);
                        tdNewRow4.Text = "<b>Total :</b> &nbsp;&nbsp;" + uploadedcount.ToString();
                        tdNewRow4.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow4);

                        TableCell tdNewRow5 = new TableCell();
                        tdNewRow5.Width = Unit.Percentage(1);
                        tdNewRow5.Text = "<b>Total :</b> &nbsp;&nbsp;" + resultcount.ToString();
                        tdNewRow5.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow5);
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
            Response.AddHeader("content-disposition", "attachment;filename=RegionalDataUpload.xls");
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