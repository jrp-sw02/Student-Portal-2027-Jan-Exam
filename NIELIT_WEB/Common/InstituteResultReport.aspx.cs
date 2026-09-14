using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class Common_InstituteResultReport : BasePage
{
    Table tbl = new Table();
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;
    Int64 instituteId = 0;
    Int32 ExamId = 0;
    Int32 CourseId = 0;
    Int32 TypeId = 0;
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/InstituteResultFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["ExamId"]) && !String.IsNullOrEmpty(Request.QueryString["CourseId"]) && !String.IsNullOrEmpty(Request.QueryString["TypeId"]))
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
    protected void ShowData()
    {
        try
        {
            context = new EConnectContext();
            instituteId = entityID;
            ExamId = Convert.ToInt32(Request.QueryString["ExamId"]);
            CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            TypeId = Convert.ToInt32(Request.QueryString["TypeId"]);
            Exam exam = context.Exams.Find(ExamId);
            Institute institute = context.Institutes.Find(instituteId);
            string strHead = "";
            if (exam != null && institute != null)
            {
                strHead += "</br> <b>Institute No :</b> " + context.AccreditationDetails.Where(s => s.InstituteID == instituteId).FirstOrDefault().AccreditationNumber;
                strHead += "</br> <b>Institute Name :</b> " + GetInitCap(institute.Name);
                strHead += "</br> <b> Institute Address :</b> " + GetInitCap(institute.AddressLine1 + "  ,  " + institute.AddressLine2 + institute.AddressLine3 +
                                                                "<br/>" + institute.CityName + " ( " + institute.State.Name + " ) ,  " + " Pin Code : " + 
                                                                (institute.PinCode.HasValue ? institute.PinCode.Value.ToString() : "NA") + ", Phone Number  :");
                if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                {
                    strHead += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber1.Value;
                    if (institute.PhoneNumber2.HasValue)
                        strHead += ", " + institute.PhoneNumber2.Value;
                }
                else
                    strHead += "NA";

                strHead += "</br> <b> Course :</b> "  + context.Courses.Find(CourseId).Name;
                strHead += "</br> <b> Exam :</b> " + exam.Name;
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "No Record Found";
            }
            if (ExamId != 0 && CourseId != 0 && TypeId != 0)
            {
                if (TypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    var application = (from a in context.CertificateExamApplications.Include("ResultGrade")
                                       where a.ExamID == ExamId && a.CourseID == CourseId && a.InstituteID == instituteId && a.ResultGradeID != null
                                       select new
                                       {
                                           ID = a.ID,
                                           Number = a.Number,
                                           RollNumber = a.RollNumber,
                                           Name = a.Name,
                                           FatherName = a.FatherName,
                                           ResultGradeID = a.ResultGradeID,
                                           ApplicantTypeID = a.ApplicantTypeID,
                                           ExamID = a.ExamID,
                                           ResultGradeCode = a.ResultGrade.Code,
                                           CourseID = a.CourseID,
                                           ResultGradeDescription = a.ResultGrade.Description
                                       }).ToList();
                    if (application.Count() > 0)
                    {
                        ShowTableHeader();                    
                        for (int i = 0; i < application.Count(); i++ )
                        {
                            TableRow tr = new TableRow();
                            if (i % 2 == 0)
                                tr.CssClass = "gdalternate1";
                            else
                                tr.CssClass = "gdrow1";

                            TableCell tdRow = new TableCell();
                            tdRow.Width = Unit.Percentage(1);
                            tdRow.Text = (i+1).ToString();
                            tdRow.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(8);
                            if (application[i].Number != null)
                                tdRow3.Text = application[i].Number;
                            tdRow3.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow3);

                            TableCell tdRow1 = new TableCell();
                            tdRow1.Width = Unit.Percentage(8);
                            string rollno = application[i].RollNumber;
                            if (rollno != null)
                                tdRow1.Text = rollno.ToString();
                            else
                                tdRow1.Text = "NA";
                            tdRow1.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow1);

                            TableCell tdRow4 = new TableCell();
                            tdRow4.Width = Unit.Percentage(25);
                            if (application[i].Name != null)
                                tdRow4.Text = GetInitCap(application[i].Name);
                            tdRow4.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow4);

                            TableCell tdRow14 = new TableCell();
                            tdRow14.Width = Unit.Percentage(15);
                            if (application[i].FatherName != null)
                                tdRow14.Text = "Mr." + GetInitCap(application[i].FatherName);
                            tdRow14.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow14);

                            TableCell tdRow5 = new TableCell();
                            tdRow5.Width = Unit.Percentage(40);
                            if (application[i].ResultGradeID.HasValue)
                            {
                                if (application[i].ResultGradeCode.ToString() == "*")
                                {
                                    Int32 ExamID = 0;
                                    String examname = "";
                                    ExamID = GetCurrentExamID(application[i].ApplicantTypeID, application[i].CourseID);
                                    if (ExamID != 0)
                                    {
                                        examname = context.Exams.Find(ExamID).Name.ToString();
                                    }
                                    else
                                    {
                                        Int32 examcycleId = context.ExaminationCycles.Where(s => s.CourseID == application[i].CourseID && s.ID != 7).FirstOrDefault().ID;
                                        ExamID = GetNextExamID(application[i].ApplicantTypeID, application[i].CourseID, examcycleId);
                                        if (ExamID != 0)
                                            examname = context.Exams.Find(ExamID).Name.ToString();
                                    }
                                    tdRow5.Text = application[i].ResultGradeCode + " ( " + application[i].ResultGradeDescription.Trim('.') + " for " + context.Courses.Find(application[i].CourseID).Code + " Exam cycle " + examname + " . The examination fee of such candidates is exempted." + " ) ";
                                }
                                else
                                {
                                    tdRow5.Text = application[i].ResultGradeCode + " ( " + application[i].ResultGradeDescription + " ) ";
                                }
                            }
                            else
                                tdRow5.Text = "NA";
                            tdRow5.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow5);

                            tbl.Rows.Add(tr);                           
                        }
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
                                       && a.ModuleID != 933 && a.ModuleID != 934 && a.ModuleID != 935 && a.ModuleID != 936 && a.ModuleID != 956 && a.ModuleID != 957 && a.ModuleID != 958 && a.ModuleID != 959
                                       select new
                                       {
                                           ID = a.ID,
                                           RegistrationNumber = a.RegistrationNumber,
                                           RollNumber =  a.RollNumber,
                                           CandidateName = a.Candidate.Name,
                                           CandidateFatherName = a.Candidate.FatherName,
                                           ResultGradeID = a.ResultGradeID,
                                           ModuleShortName = a.Module.ShortName
                                       }).ToList();

                    if (application.Count() > 0)
                    {
                        int applicationCount = application.Count();                       
                        ShowTableHeader();                      
                        
                        for (int i = 0; i < applicationCount; i++)
                        {
                            TableRow tr = new TableRow();
                            if (i % 2 == 0)
                                tr.CssClass = "gdalternate1";
                            else
                                tr.CssClass = "gdrow1";

                            TableCell tdRow = new TableCell();
                            tdRow.Width = Unit.Percentage(1);
                            tdRow.Text = (i+1).ToString();
                            tdRow.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(8);
                            if (application[i].RegistrationNumber != 0)
                                tdRow3.Text = application[i].RegistrationNumber.ToString();
                            tdRow3.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow3);

                            TableCell tdRow1 = new TableCell();
                            tdRow1.Width = Unit.Percentage(8);
                            if (application[i].RollNumber.HasValue)
                                tdRow1.Text = application[i].RollNumber.Value.ToString();
                            else
                                tdRow1.Text = "NA";
                            tdRow1.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow1);

                            TableCell tdRow4 = new TableCell();
                            tdRow4.Width = Unit.Percentage(20);
                            if (application[i].CandidateName != null)
                                tdRow4.Text = GetInitCap(application[i].CandidateName);
                            tdRow4.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow4);

                            TableCell tdRow14 = new TableCell();
                            tdRow14.Width = Unit.Percentage(20);
                            if (application[i].CandidateFatherName != null)
                                tdRow14.Text = "Mr." + GetInitCap(application[i].CandidateFatherName);
                            tdRow14.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow14);

                            TableCell tdRow5 = new TableCell();
                            tdRow5.Width = Unit.Percentage(10);
                            if (application[i].ResultGradeID.HasValue)
                            {
                                int ResultGradeID = application[i].ResultGradeID.GetValueOrDefault();
                                tdRow5.Text = application[i].ModuleShortName + " ( " + context.ResultGrades.Where(s => s.ID == ResultGradeID).FirstOrDefault().Code + " ) ";
                            }
                            else
                                tdRow5.Text = "NA";
                            tdRow5.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow5);
                            tbl.Rows.Add(tr);                           
                        }
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }                  
                }
            }
            LblRptSubHeader.Text = strHead;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally { context.Dispose(); }

    }
    protected void ShowTableHeader()
    {
        try
        {
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(1);
            tcCol1.Text = "#";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            if (TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                TableHeaderCell tcCol4 = new TableHeaderCell();
                tcCol4.Width = Unit.Percentage(8);
                tcCol4.Text = "Regn.No.";
                tcCol4.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol4);
            }
            else
            {
                TableHeaderCell tcCol4 = new TableHeaderCell();
                tcCol4.Width = Unit.Percentage(8);
                tcCol4.Text = "App.No.";
                tcCol4.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol4);
            }

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(8);
            tcCol2.Text = "Roll No.";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(20);
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Text = "Candidate Name";
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(20);
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            tcCol9.Text = "Father's Name";
            th.Cells.Add(tcCol9);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(10);
            tcCol7.Text = "Result";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected Int32 GetNextExamID(Int32 applicantTypeID, Int32 courseID, Int32 examCycleID)
    {
        try
        {
            Int32 examID = 0;
            using (EConnectContext context = new EConnectContext())
            {
                int StartDateofFormFilling = Convert.ToInt32(enmActivity.DateFfCommencementOfOnlineFillInExaminationApplicationForm);
                var examsAll = (from f in context.Exams
                                where f.CourseID == courseID && f.ExaminationCycleID == examCycleID &&
                                f.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && f.DateOfPublishingOfRollNumber == null
                                select f);

                examsAll = examsAll.Where(a => a.CutOffDates.Where(k => k.ActivityID == StartDateofFormFilling && k.ApplicantTypeID == applicantTypeID).FirstOrDefault().EfferctiveDate <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now));

                if (examsAll.Count() > 0)
                {
                    int LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                    int NormalFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                    var exmasWithNormalLastDate = (from t in context.CutOffDates
                                                   where examsAll.Select(d => d.ID).Contains(t.ExamID) && t.ApplicantTypeID == applicantTypeID &&
                                                   t.ActivityID == NormalFeeActivityId && t.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                                   orderby t.Exam.ExamStartDate
                                                   select t.Exam).Distinct();
                    if (exmasWithNormalLastDate.Count() == 0)
                    {
                        var exmasWithLateFeeLastDate = (from t in context.CutOffDates
                                                        where examsAll.Select(d => d.ID).Contains(t.ExamID) && t.ApplicantTypeID == applicantTypeID &&
                                                        t.ActivityID == LateFeeActivityId && t.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                                        orderby t.Exam.ExamStartDate
                                                        select t.Exam).Distinct();
                        examID = exmasWithLateFeeLastDate.Select(c => c.ID).FirstOrDefault();
                    }
                    else
                    {
                        examID = exmasWithNormalLastDate.Select(c => c.ID).FirstOrDefault();
                    }
                }
                else
                    examID = examsAll.Select(c => c.ID).FirstOrDefault();
            };
            return examID;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected Int32 GetCurrentExamID(Int32 applicantTypeID, Int32 courseID)
    {
        try
        {
            Int32 ExamID = 0;
            Int32 NormalactivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
            Int32 LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
            if (courseID != 0)
            {
                using (EConnectContext context = new EConnectContext())
                {

                    var LateFeeExam = (from e in context.CutOffDates
                                       join i in context.Exams on e.ExamID equals i.ID
                                       where e.CourseID == courseID
                                       && e.ApplicantTypeID == applicantTypeID
                                       && e.ActivityID == LateFeeActivityId
                                       && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                       orderby e.EfferctiveDate ascending
                                       select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
                    if (LateFeeExam.Count() > 0)//If  applicable for late fee ?
                    {

                        if (LateFeeExam != null)
                        {
                            ExamID = LateFeeExam.FirstOrDefault().ExamID;
                        }
                    }
                    else if (LateFeeExam.Count() <= 0)//If  not applicable for late fee ?
                    {

                        var NormalFeeExam = (from e in context.CutOffDates
                                             join i in context.Exams on e.ExamID equals i.ID
                                             where e.CourseID == courseID
                                             && e.ApplicantTypeID == applicantTypeID
                                             && e.ActivityID == NormalactivityId
                                             && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                             orderby e.EfferctiveDate ascending
                                             select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
                        if (NormalFeeExam.Count() > 0)
                        {
                            ExamID = NormalFeeExam.FirstOrDefault().ExamID;
                        }
                    }

                };
            }
            return ExamID;
        }
        catch (Exception ex)
        {
            throw ex;
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
            Response.AddHeader("content-disposition", "attachment;filename=ResultSheet.xls");
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