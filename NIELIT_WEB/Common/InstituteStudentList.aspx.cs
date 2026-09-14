using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class ReportPgae : BasePage
{
    Table tbl = new Table();
    //EConnectContext context = new EConnectContext();
    UserType loginUserType;
    Int64 entityID = 0;
    Int64 instituteId = 0;
    Int32 ExamId = 0;
    Int32 CourseId = 0;
    Int32 courseCatId = 0;
    Int32 TypeId = 0;
    String source = "";
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/InstituteCentreFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["instituteId"]) && !String.IsNullOrEmpty(Request.QueryString["ExamId"]) && !String.IsNullOrEmpty(Request.QueryString["CourseId"]) && !String.IsNullOrEmpty(Request.QueryString["TypeId"]))
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
                    Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
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
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";


            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "#";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(1);
            tcCol1.Text = "ApplicationNo.";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(3);
            tcCol2.Text = "Date";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(19);
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Text = "Candidate Name";
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(19);
            tcCol4.Text = "Father/Guardian Name";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(12);
            tcCol5.Text = "Mother Name";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(3);
            tcCol6.Text = "Date of Birth";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

            if (TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                TableHeaderCell tcCol7 = new TableHeaderCell();
                tcCol7.Width = Unit.Percentage(3);
                tcCol7.Text = "Module Name";
                tcCol7.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol7);
            }

            tbl.Rows.Add(th);
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
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol0 = new TableHeaderCell();
            //tcCol0.Width = Unit.Percentage(3);
            tcCol0.Text = "#";
            th.Cells.Add(tcCol0);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            //tcCol1.Width = Unit.Percentage(30);
            tcCol1.Text = "Name";
            tcCol1.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            //tcCol2.Width = Unit.Percentage(30);
            tcCol2.Text = "Father Name";
            tcCol2.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            //tcCol3.Width = Unit.Percentage(30);
            tcCol3.HorizontalAlign = HorizontalAlign.Left;
            tcCol3.Text = "Mother Name";
            th.Cells.Add(tcCol3);

            //TableHeaderCell tcCol4 = new TableHeaderCell();
            //tcCol4.Width = Unit.Percentage(15);
            //tcCol4.Text = "Father Name";
            //tcCol4.HorizontalAlign = HorizontalAlign.Left;
            //th.Cells.Add(tcCol4);

            //TableHeaderCell tcCol5 = new TableHeaderCell();
            //tcCol5.Width = Unit.Percentage(15);
            //tcCol5.Text = "Mother Name";
            //tcCol5.HorizontalAlign = HorizontalAlign.Left;
            //th.Cells.Add(tcCol5);

            //TableHeaderCell tcCol6 = new TableHeaderCell();
            //tcCol6.Width = Unit.Percentage(10);
            //tcCol6.Text = "Date of Birth";
            //tcCol6.HorizontalAlign = HorizontalAlign.Right;
            //th.Cells.Add(tcCol6);

            //TableHeaderCell tcCol7 = new TableHeaderCell();
            //tcCol7.Width = Unit.Percentage(10);
            //tcCol7.Text = "Payment Mode";
            //tcCol7.HorizontalAlign = HorizontalAlign.Left;
            //th.Cells.Add(tcCol7);

            //TableHeaderCell tcCol8 = new TableHeaderCell();
            //tcCol8.Width = Unit.Percentage(10);
            //tcCol8.Text = "Payment status";
            //th.Cells.Add(tcCol8);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowCertificateExamData(IEnumerable<CertificateExamApplication> application)
    {
        try
        {
            ShowTableHeader();
            int i = 1;
            foreach (CertificateExamApplication app in application)
            {
                TableRow tr = new TableRow();
                if (i % 2 == 0)
                    tr.CssClass = "gdalternate1";
                else
                    tr.CssClass = "gdrow1";

                TableCell tdRow = new TableCell();
                tdRow.Width = Unit.Percentage(1);
                tdRow.Text = i.ToString();
                tdRow.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow);

                TableCell tdRow1 = new TableCell();
                tdRow1.Width = Unit.Percentage(1);
                tdRow1.Text = app.Number.ToString();
                tdRow1.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow1);

                TableCell tdRow2 = new TableCell();
                tdRow2.Width = Unit.Percentage(10);
                if (app.ApplicationDate != null)
                    tdRow2.Text = app.ApplicationDate.ToString("dd-MMM-yyyy");
                tdRow2.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow2);

                TableCell tdRow3 = new TableCell();
                tdRow3.Width = Unit.Percentage(14);
                if (app.Name != null)
                    tdRow3.Text = GetInitCap(app.Name).ToString();
                tdRow3.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow3);

                if (string.IsNullOrEmpty(app.GuardianName) == true && string.IsNullOrWhiteSpace(app.GuardianName) == true)
                {
                    TableCell tdRow4 = new TableCell();
                    tdRow4.Width = Unit.Percentage(12);
                    if (app.FatherName != null)
                        tdRow4.Text = GetInitCap(app.FatherName).ToString();
                    tdRow4.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow4);

                    TableCell tdRow5 = new TableCell();
                    tdRow5.Width = Unit.Percentage(8);
                    if (app.MotherName != null)
                        tdRow5.Text = GetInitCap(app.MotherName).ToString();
                    tdRow5.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow5);
                }
                else
                {
                    TableCell tdRow4 = new TableCell();
                    tdRow4.Width = Unit.Percentage(12);
                    if (app.GuardianName != null)
                        tdRow4.Text = GetInitCap(app.GuardianName).ToString();
                    tdRow4.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow4);

                    TableCell tdRow5 = new TableCell();
                    tdRow5.Width = Unit.Percentage(8);
                    if (app.MotherName != null)
                        tdRow5.Text = GetInitCap(app.MotherName).ToString();
                    tdRow5.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow5);
                }
                TableCell tdRow6 = new TableCell();
                tdRow6.Width = Unit.Percentage(10);
                if (app.DateOfBirth != null)
                    tdRow6.Text = app.DateOfBirth.ToString("dd-MMM-yyyy");
                tdRow6.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow6);

                tbl.Rows.Add(tr);
                i++;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowCourseExamData(IEnumerable<CourseExamApplicationDetail> application)
    {
        try
        {
            ShowTableHeader();
            int i = 1;
            foreach (CourseExamApplicationDetail app in application)
            {
                TableRow tr = new TableRow();
                if (i % 2 == 0)
                    tr.CssClass = "gdalternate1";
                else
                    tr.CssClass = "gdrow1";

                TableCell tdRow = new TableCell();
                tdRow.Width = Unit.Percentage(1);
                tdRow.Text = i.ToString();
                tdRow.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow);

                TableCell tdRow1 = new TableCell();
                tdRow1.Width = Unit.Percentage(1);
                if (app.CourseExamApplication != null)
                    tdRow1.Text = app.CourseExamApplication.Number.ToString();
                else
                    tdRow1.Text = "NA";
                tdRow1.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow1);

                TableCell tdRow2 = new TableCell();
                tdRow2.Width = Unit.Percentage(10);
                if (app.CourseExamApplication != null)
                    tdRow2.Text = app.CourseExamApplication.ApplicationDate.ToString("dd-MMM-yyyy");
                else
                    tdRow2.Text = "NA";
                tdRow2.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow2);

                TableCell tdRow3 = new TableCell();
                tdRow3.Width = Unit.Percentage(22);
                if (app.Candidate.Name != null)
                    tdRow3.Text = GetInitCap(app.Candidate.Name).ToString();
                tdRow3.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow3);

                if (string.IsNullOrEmpty(app.Candidate.GuardianName) == true && string.IsNullOrWhiteSpace(app.Candidate.GuardianName) == true)
                {
                    TableCell tdRow4 = new TableCell();
                    tdRow4.Width = Unit.Percentage(12);
                    if (app.Candidate.FatherName != null)
                        tdRow4.Text = GetInitCap(app.Candidate.FatherName).ToString();
                    tdRow4.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow4);

                    TableCell tdRow5 = new TableCell();
                    tdRow5.Width = Unit.Percentage(8);
                    if (app.Candidate.MotherName != null)
                        tdRow5.Text = GetInitCap(app.Candidate.MotherName).ToString();
                    tdRow5.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow5);
                }
                else
                {
                    TableCell tdRow4 = new TableCell();
                    tdRow4.Width = Unit.Percentage(12);
                    if (app.Candidate.GuardianName != null)
                        tdRow4.Text = GetInitCap(app.Candidate.GuardianName).ToString();
                    tdRow4.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow4);

                    TableCell tdRow5 = new TableCell();
                    tdRow5.Width = Unit.Percentage(8);
                    if (app.Candidate.MotherName != null)
                        tdRow5.Text = GetInitCap(app.Candidate.MotherName).ToString();
                    tdRow5.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow5);
                }


                TableCell tdRow6 = new TableCell();
                tdRow6.Width = Unit.Percentage(10);
                if (app.Candidate.DateOfBirth != null)
                    tdRow6.Text = app.Candidate.DateOfBirth.ToString("dd-MMM-yyyy");
                tdRow6.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow6);

                TableCell tdRow7 = new TableCell();
                tdRow7.Width = Unit.Percentage(30);
                if (app.Module.Name != null)
                    tdRow7.Text = app.Module.Name;
                tdRow7.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow7);

                tbl.Rows.Add(tr);
                i++;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }

    protected void ShowData()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                instituteId = Convert.ToInt64(Request.QueryString["instituteId"]);
                ExamId = Convert.ToInt32(Request.QueryString["ExamId"]);
                CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
                courseCatId = Convert.ToInt32(Request.QueryString["courseCatId"]);
                TypeId = Convert.ToInt32(Request.QueryString["TypeId"]);
                source = (Request.QueryString["src"]).ToString();
                Exam exam = context.Exams.Find(ExamId);
                Institute institute = context.Institutes.Find(instituteId);
                string strHead = "";
                //deep add 28 july 2018
                DateTime? DateFrom = Convert.ToDateTime(Request.QueryString["FromDate"]);
                DateTime? DateTo = Convert.ToDateTime(Request.QueryString["ToDate"]);
                DateTime NullDate = Convert.ToDateTime("1/1/0001");
                // deep add end july 2018   DateFrom.GetValueOrDefault().ToString("dd-MMM-yyyy")

                if (exam != null && institute != null)
                {
                    strHead += "</br> <b>Application Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicationType)(TypeId)).ToString();
                    strHead += ", <b>Course Category :</b> " + exam.CourseCategory.Name;
                    strHead += " , <b>Course : </b>" + exam.Course.Name;
                    strHead += "<br/> <b> Exam Cycle : </b>" + exam.ExaminationCycle.Name;
                    strHead += ", <b> Exam Name : </b>" + exam.Name;
                    if (source.ToUpper().Trim() == "candapplied".ToUpper().Trim())
                        strHead += "<br/> <b> List of Applied Candidates For ";
                    else if (source.ToUpper().Trim() == "candappeared".ToUpper().Trim())
                        strHead += "<br/> <b> List of Appeared Candidates For ";
                    else if (source.ToUpper().Trim() == "candpassed".ToUpper().Trim())
                        strHead += "<br/> <b> List of Passed Candidates For ";

                    //deep add code on 27 july 2018
                    else if (source.ToUpper().Trim() == "candabsent".ToUpper().Trim())
                        strHead += "<br/> <b> List of Absent Candidates For ";
                    else if (source.ToUpper().Trim() == "candfail".ToUpper().Trim())
                        strHead += "<br/> <b> List of Failed Candidates For ";
                    //deep end code on 27 july 2018
                    strHead += "<b>Institute : </b>" + institute.Name;
                }
                //else
                //{
                //    lblError.Visible = true;
                //    lblError.Text = "No Record Found";
                //}


                //deep add code on 29 july 2018
                else if (ExamId == 0 && DateFrom.GetValueOrDefault() != NullDate)
                {
                    strHead += "</br> <b>Application Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicationType)(TypeId)).ToString();
                    String AccrNo = institute.AccreditationDetails.Where(s => s.CourseID == CourseId).FirstOrDefault().AccreditationNumber;
                    var courseDetail = context.Courses.Find(CourseId);
                    if (source.ToUpper().Trim() == "candapplied".ToUpper().Trim())
                        strHead += "<br/> <b> List of Applied Candidates For ";
                    else if (source.ToUpper().Trim() == "candappeared".ToUpper().Trim())
                        strHead += "<br/> <b> List of Appeared Candidates For ";
                    else if (source.ToUpper().Trim() == "candpassed".ToUpper().Trim())
                        strHead += "<br/> <b> List of Passed Candidates For ";
                    else if (source.ToUpper().Trim() == "candabsent".ToUpper().Trim())
                        strHead += "<br/> <b> List of Absent Candidates For ";
                    else if (source.ToUpper().Trim() == "candfail".ToUpper().Trim())
                        strHead += "<br/> <b> List of Failed Candidates For ";
                    strHead += "<b>Institute : </b> (" + AccrNo + ") " + institute.Name;
                    strHead += "<br/><b>Course : </b>" + courseDetail.Name;
                    strHead += "<br/><b>From : </b>" + DateFrom.GetValueOrDefault().ToString("dd-MMM-yyyy") + " <b>To : </b>" + DateTo.GetValueOrDefault().ToString("dd-MMM-yyyy");
                    // }                 
                    var application1 = (from a in context.CertificateExamApplications
                                        where a.CourseID == CourseId && a.InstituteID == instituteId && a.ResultGradeID != null
                                        && a.DateOfExam >= DateFrom && a.DateOfExam < DateTo
                                        select a).ToList();
                    var resultGrade1 = context.ResultGrades.Where(s => s.CourseCategoryID == courseCatId);
                    if (source.ToUpper().Trim() == "candappeared".ToUpper().Trim())
                    {
                        var absent = resultGrade1.Where(s => s.Description.Equals("Absent", StringComparison.OrdinalIgnoreCase)).Select(s => s.ID).ToArray();
                        application1 = application1.Where(s => !absent.Contains(s.ResultGradeID.Value)).Distinct().ToList();
                    }
                    if (source.ToUpper().Trim() == "candpassed".ToUpper().Trim())
                    {
                        var passedcodes = resultGrade1.Where(s => s.Description.Equals("PASS", StringComparison.OrdinalIgnoreCase)).Select(s => s.ID).ToArray();
                        application1 = application1.Where(s => passedcodes.Contains(s.ResultGradeID.Value)).Distinct().ToList();
                    }
                    if (source.ToUpper().Trim() == "candabsent".ToUpper().Trim())
                    {
                        var absentcodes = resultGrade1.Where(s => s.Description.Equals("Absent", StringComparison.OrdinalIgnoreCase)).Select(s => s.ID).ToArray();
                        application1 = application1.Where(s => absentcodes.Contains(s.ResultGradeID.Value)).Distinct().ToList();
                    }

                    if (source.ToUpper().Trim() == "candfail".ToUpper().Trim())
                    {
                        var failcodes = resultGrade1.Where(s => s.Description.Equals("Fail", StringComparison.OrdinalIgnoreCase)).Select(s => s.ID).ToArray();
                        application1 = application1.Where(s => failcodes.Contains(s.ResultGradeID.Value)).Distinct().ToList();
                    }
                    if (application1.Count() > 0)
                    {
                        ShowCertificateExamData(application1);
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
                     
                //deep  end on add code on 29 july 2018


                if (ExamId != 0 && CourseId != 0 && TypeId != 0)
                {
                    if (TypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                    {
                        var application = (from a in context.CertificateExamApplications
                                           where a.ExamID == ExamId && a.CourseID == CourseId && a.InstituteID == instituteId && a.ResultGradeID != null
                                           select a).ToList();

                        var resultGrade = context.ResultGrades.Where(s => s.CourseCategoryID == courseCatId);
                        if (source.ToUpper().Trim() == "candappeared".ToUpper().Trim())
                        {                           
                            var absent = resultGrade.Where(s => s.Description.Equals("Absent", StringComparison.OrdinalIgnoreCase)).Select(s => s.ID).ToArray();                            
                            application = application.Where(s => !absent.Contains(s.ResultGradeID.Value)).Distinct().ToList();
                        }
                        if (source.ToUpper().Trim() == "candpassed".ToUpper().Trim())
                        {                          
                            var passedcodes = resultGrade.Where(s => s.Description.Equals("PASS", StringComparison.OrdinalIgnoreCase)).Select(s => s.ID).ToArray();
                            application = application.Where(s => passedcodes.Contains(s.ResultGradeID.Value)).Distinct().ToList();
                        }

                        //deep add code on 27 july 2018
                        if (source.ToUpper().Trim() == "candabsent".ToUpper().Trim())
                        {
                            var absentcodes = resultGrade.Where(s => s.Description.Equals("Absent", StringComparison.OrdinalIgnoreCase)).Select(s => s.ID).ToArray();
                            application = application.Where(s => absentcodes.Contains(s.ResultGradeID.Value)).Distinct().ToList();
                        }

                        if (source.ToUpper().Trim() == "candfail".ToUpper().Trim())
                        {
                            var failcodes = resultGrade.Where(s => s.Description.Equals("Fail", StringComparison.OrdinalIgnoreCase)).Select(s => s.ID).ToArray();
                            application = application.Where(s => failcodes.Contains(s.ResultGradeID.Value)).Distinct().ToList();
                        }
                        //deep end code on 27 july 2018
                        if (application.Count() > 0)
                        {
                            ShowCertificateExamData(application);
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No Record Found";
                        }
                    }
                    else if (TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    {
                        var application = (from a in context.CourseExamApplicationDetails
                                           where a.ExamID == ExamId && a.CourseID == CourseId && a.InstituteID == instituteId && a.ResultGradeID != null
                                           orderby a.Candidate.Name ascending
                                           select a).ToList();
                        if (source.ToUpper().Trim() == "candappeared".ToUpper().Trim())
                        {
                            Int32 absent = (from r in context.ResultGrades
                                            where r.CourseCategoryID == 1
                                            && r.Code.ToUpper().Trim() == "ABS".Trim()
                                            select r.ID).FirstOrDefault();
                            application = application.Where(s => s.ResultGradeID.Value != absent).Distinct().ToList();
                        }
                        else if (source.ToUpper().Trim() == "candpassed".ToUpper().Trim())
                        {
                            var paasedcodes = (from r in context.ResultGrades
                                               where r.CourseCategoryID == 1
                                               && r.Description.ToUpper().Trim() == "PASS".Trim()
                                               select r.ID).ToArray();
                            application = application.Where(s => paasedcodes.Contains(s.ResultGradeID.Value)).Distinct().ToList();
                        }
                        if (application.Count() > 0)
                        {
                            ShowCourseExamData(application);
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No Record Found";
                        }
                    }
                }
                LblRptSubHeader.Text = strHead;
            };
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
            Response.AddHeader("content-disposition", "attachment;filename=Applications.xls");
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