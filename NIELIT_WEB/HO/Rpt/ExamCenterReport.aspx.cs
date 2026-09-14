using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Data;

public partial class ExamCenterReportPage : BasePage
{
    Table tbl = new Table();
    EConnectContext context;
    //Int32 StatusId = 0;
    Int32 applicantTypeID = 0;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 stateID = 0;
    Int32 ExamId = 0;
    Int32 CourseId = 0;
    Int32 TypeId = 0;
    Int32 examCenter = 0;
    Int32 examVenue = 0;
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/ExamCenterFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);

            if (!Page.IsPostBack)
            {

                tbl.CssClass = "sample3";
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(100);
                ShowData();
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

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "#";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(10);
            tcCol1.Text = "Centre Code";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(15);
            tcCol2.Text = "Centre Address ";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(15);
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Text = "Centre State";
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(15);
            tcCol4.Text = "Candidates Applied";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(15);
            tcCol5.Text = "Candidates Appeared";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(15);
            tcCol6.Text = "Candidates Passed";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(10);
            tcCol7.Text = "Pass %";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);

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
            int j = 1;
            var app = application.FirstOrDefault();

                TableRow tr = new TableRow();
                tr.CssClass = "gdalternate1";

                TableCell tdRow = new TableCell();
                tdRow.Width = Unit.Percentage(2);
                tdRow.Text = j.ToString();
                tdRow.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow);

                TableCell tdRow1 = new TableCell();
                tdRow1.Width = Unit.Percentage(15);
                if (app.ExamCentreName != null)
                    tdRow1.Text = app.ExamCentreName.ToString();
                else
                    tdRow1.Text = "NA";
                tdRow1.Wrap = false;
                tdRow1.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow1);

                TableCell tdRow2 = new TableCell();
                tdRow2.Width = Unit.Percentage(15);
                if (app.ExamCentreName != null)
                    tdRow2.Text = context.ExamCenters.Where(s => s.Code.ToUpper().Trim() == app.ExamCentreName.ToUpper().Trim()).Select(t => t.Name).FirstOrDefault();
                else
                    tdRow2.Text = "NA";
                tdRow2.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow2);

                TableCell tdRow3 = new TableCell();
                tdRow3.Width = Unit.Percentage(15);
                if (app.ExamCentreName != null)
                    tdRow3.Text = context.ExamCenters.Where(s => s.Code.ToUpper().Trim() == app.ExamCentreName.ToUpper().Trim()).Select(t => t.State.Name).FirstOrDefault();
                else
                    tdRow3.Text = "NA";
                tdRow3.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow3);

                TableCell tdRow4 = new TableCell();
                tdRow4.Width = Unit.Percentage(15);
                if (application != null)
                    tdRow4.Text = application.Count().ToString();
                else
                    tdRow4.Text = "0";
                tdRow4.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow4);

                Int32 absent = (from r in context.ResultGrades
                                where r.CourseCategoryID == 2
                                && r.Code.ToUpper().Trim() == "ABS".Trim()
                                select r.ID).FirstOrDefault();

                var total2 = application.Where(s => s.ResultGradeID.Value != absent);
                TableCell tdRow5 = new TableCell();
                tdRow5.Width = Unit.Percentage(15);
                if (total2 != null)
                    tdRow5.Text = total2.Count().ToString();
                else
                    tdRow5.Text = "0";
                tdRow5.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow5);

                var paasedcodes = (from r in context.ResultGrades
                                   where r.CourseCategoryID == 2
                                   && r.Description.ToUpper().Trim() == "PASS".Trim()
                                   select r.ID).ToArray();

                Int32 totalpassedstudent = 0;
                for (int i = 0; i < paasedcodes.Length; i++)
                {
                    var total1 = application.Where(s => s.ResultGradeID.Value == paasedcodes[i]).Count();
                    totalpassedstudent += total1;
                }

                TableCell tdRow6 = new TableCell();
                tdRow6.Width = Unit.Percentage(15);
                if (totalpassedstudent != 0)
                    tdRow6.Text = totalpassedstudent.ToString();
                else
                    tdRow6.Text = "0";
                tdRow6.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow6);

                float per = 0;
                if (totalpassedstudent != 0 && total2 != null)
                {
                    float passedstudent = Convert.ToSingle(totalpassedstudent.ToString());
                    float totalstudent = Convert.ToSingle(total2.Count().ToString());
                    if (passedstudent == 0 && totalstudent == 0)
                    {
                        per = 0;
                    }
                    else
                    {
                        float ratio = ((passedstudent) / (totalstudent));
                        per = ratio * 100;
                    }
                }
                TableCell tdRow7 = new TableCell();
                tdRow7.Width = Unit.Percentage(10);
                tdRow7.Text = per.ToString("F");
                tdRow7.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow7);

                tbl.Rows.Add(tr);
                j++;
            //}
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ShowCourseRegistrationData(IEnumerable<CourseRegistrationApplication> application)
    {
        try
        {
            ShowTableHeader();
            int i = 1;

            TableRow tr = new TableRow();
            tr.CssClass = "gdalternate1";

            TableCell tdRow = new TableCell();
            tdRow.Width = Unit.Percentage(1);
            tdRow.Text = i.ToString();
            tdRow.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(tdRow);

            TableCell tdRow1 = new TableCell();
            tdRow1.Width = Unit.Percentage(1);
            tdRow1.Text = "NA";
            tdRow1.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(tdRow1);

            TableCell tdRow2 = new TableCell();
            tdRow2.Width = Unit.Percentage(13);
            //if(app.ApplicationDate!=null)
            tdRow2.Text = "";
            tdRow2.HorizontalAlign = HorizontalAlign.Center;
            tr.Cells.Add(tdRow2);

            TableCell tdRow3 = new TableCell();
            tdRow3.Width = Unit.Percentage(14);
            //if(app.Name!=null)
            tdRow3.Text = "";
            tdRow3.HorizontalAlign = HorizontalAlign.Left;
            tr.Cells.Add(tdRow3);






            tbl.Rows.Add(tr);


        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ShowCourseExamData(IEnumerable<CourseExamApplication> application)
    {
        try
        {
            ShowTableHeader();
            int j = 1;
            var app = application.FirstOrDefault();

                TableRow tr = new TableRow();
                tr.CssClass = "gdalternate1";

                TableCell tdRow = new TableCell();
                tdRow.Width = Unit.Percentage(1);
                tdRow.Text = j.ToString();
                tdRow.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow);

                TableCell tdRow1 = new TableCell();
                tdRow1.Width = Unit.Percentage(1);
                if (app != null)
                    tdRow1.Text = app.AllottedExamCentre.Code.ToString();
                else
                    tdRow1.Text = "NA";
                tdRow1.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow1);

                TableCell tdRow2 = new TableCell();
                tdRow2.Width = Unit.Percentage(15);
                if (app != null)
                    tdRow2.Text = app.AllottedExamCentre.Name.ToString();
                else
                    tdRow2.Text = "NA";
                tdRow2.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow2);

                TableCell tdRow3 = new TableCell();
                tdRow3.Width = Unit.Percentage(15);
                if (app != null)
                    tdRow3.Text = app.AllottedExamCentre.State.Name.ToString();
                else
                    tdRow3.Text = "NA";
                tdRow3.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow3);

                var application1 = (from a in context.CourseExamApplications
                                    where (a.ExamID == ExamId && a.CourseID == CourseId && a.AllottedExamCentreID == app.AllottedExamCentreID
                                          )
                                    select a.ID).ToArray();

                var query = (from p in context.CourseExamApplicationDetails
                             where p.ResultGradeID != null && application1.Contains(p.CourseExamApplicationID.Value)
                             select p).Distinct().ToList();


                TableCell tdRow4 = new TableCell();
                tdRow4.Width = Unit.Percentage(15);
                if (query != null)
                    tdRow4.Text = query.Select(t => t.CandidateID).Distinct().Count().ToString();
                else
                    tdRow4.Text = "0";
                tdRow4.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow4);

                Int32 absent = (from r in context.ResultGrades
                                where r.CourseCategoryID == 1
                                && r.Code.ToUpper().Trim() == "ABS".Trim()
                                select r.ID).FirstOrDefault();

                var total2 = query.Where(s => s.ResultGradeID.Value != absent).Select(t => t.CandidateID).Distinct();
                TableCell tdRow5 = new TableCell();
                tdRow5.Width = Unit.Percentage(15);
                if (total2 != null)
                    tdRow5.Text = total2.Count().ToString();
                else
                    tdRow5.Text = "0";
                tdRow5.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow5);

                var paasedcodes = (from r in context.ResultGrades
                                   where r.CourseCategoryID == 1
                                   && r.Description.ToUpper().Trim() == "PASS".Trim()
                                   select r.ID).ToArray();

                var totalpassedstudent = query.Where(s => paasedcodes.Contains(s.ResultGradeID.Value)).Select(t => t.CandidateID).Distinct().Count();

                TableCell tdRow6 = new TableCell();
                tdRow6.Width = Unit.Percentage(15);
                if (totalpassedstudent != null)
                    tdRow6.Text = totalpassedstudent.ToString();
                else
                    tdRow6.Text = "0";
                tdRow6.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow6);

                float per = 0;
                if (totalpassedstudent != null && total2 != null)
                {
                    float passedstudent = Convert.ToSingle(totalpassedstudent.ToString());
                    float totalstudent = Convert.ToSingle(total2.Count().ToString());
                    if (passedstudent == 0 && totalstudent == 0)
                    {
                        per = 0;
                    }
                    else
                    {
                        float ratio = ((passedstudent) / (totalstudent));
                        per = ratio * 100;
                    }
                }
                TableCell tdRow7 = new TableCell();
                tdRow7.Width = Unit.Percentage(15);
                tdRow7.Text = per.ToString("F");
                tdRow7.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow7);

                tbl.Rows.Add(tr);
                j++;
          
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
            stateID = Convert.ToInt32(Request.QueryString["stateID"]);
            ExamId = Convert.ToInt32(Request.QueryString["ExamId"]);
            CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            TypeId = Convert.ToInt32(Request.QueryString["TypeId"]);
            examCenter = Convert.ToInt32(Request.QueryString["ExamCenter"]);
            examVenue = Convert.ToInt32(Request.QueryString["examVenue"]);
            Exam exam = context.Exams.Find(ExamId);
            string strHead = "";
            if (exam != null)
            {
                strHead += "</br> <b>Application Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicationType)(TypeId)).ToString();
                strHead += ", <b>Course Category :</b> " + exam.CourseCategory.Name;
                strHead += " , <b>Course : </b>" + exam.Course.Name;
                strHead += "<br/> <b> Exam Cycle : </b>" + exam.ExaminationCycle.Name;
                strHead += ", <b> Exam Name : </b>" + exam.Name;
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "No Record Found";
            }
            if (ExamId != 0 && CourseId != 0 && TypeId != 0)
            {
                #region ------Certificate-Exam-Application-------
                if (TypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    using (context = new EConnectContext())
                    {
                        var application = (from a in context.CertificateExamApplications.AsNoTracking()
                                           where a.ExamID == ExamId && a.CourseID == CourseId && a.ResultGradeID != null
                                           select new
                                           {
                                               ID = a.ID,
                                               Number = a.Number,
                                               ExamCentreName = a.ExamCentreName,
                                               InstituteID  = a.InstituteID,
                                               RegionalCenterID = a.RegionalCenterID,
                                               ResultGradeID = a.ResultGradeID,
                                               ExamCntreAddress = a.ExamCentreAddress
                                           }).ToList();

                        if (examCenter > 0 && examCenter != 99999999)
                            {
                                var ExamCentreName = (from k in context.ExamCenters
                                                      where k.ID == examCenter
                                                      select k.Code).FirstOrDefault();
                                //application = application.Where(s => s.ExamCentreName.ToUpper().Trim() == ExamCentreName.ToUpper().Trim()).ToList();
                                application = application.Where(s => s.ExamCentreName.Contains(ExamCentreName.ToUpper().Trim())).ToList();
                            }
                            else if(examCenter == 99999999)
                            {
                                //var examcentrelist = context.ExamCenters.Where(a => a.StateID == stateID).Select(t => t.Code.ToUpper().Trim()).ToList();

                                application = (from p in context.CertificateExamApplications
                                                      join q in context.ExamCenters on p.ExamCentreName.Trim().Substring(0, 3).Trim() equals q.Code.Trim()
                                                      where p.CourseID == CourseId
                                                      && q.CourseCategoryID == 2
                                                      && q.StateID == stateID
                                                      && p.ExamID == ExamId
                                                      && p.ExamCentreName != null
                                                      orderby p.ExamCentreName 
                                                      select new {
                                                        ID = p.ID,
                                                        Number = p.Number,
                                                        ExamCentreName = p.ExamCentreName,
                                                        InstituteID  = p.InstituteID,
                                                        RegionalCenterID = p.RegionalCenterID,
                                                        ResultGradeID = p.ResultGradeID,
                                                        ExamCntreAddress = p.ExamCentreAddress
                                                      }).ToList();

                                //application = application.Where(s => examcentrelist.Contains(s.ExamCentreName.ToUpper().Trim())).ToList();
                            }
                            else
                            {

                                    var examcentrelist = context.ExamCenters.Where(a => a.StateID == stateID).Select(t => t.Code.ToUpper().Trim()).ToList();
                                    application = application.Where(s => examcentrelist.Contains(s.ExamCentreName.ToUpper().Trim())).ToList();

                            }
                        
                        if (loginUserType == UserType.Institute)
                            application = application.Where(s => s.InstituteID == entityID).ToList();
                        if (loginUserType == UserType.RegionalCenter)
                        {
                            application = application.Where(s => s.RegionalCenterID == entityID).ToList();
                        }
                        if (application.Count() > 0)
                        {
                            //ShowCertificateExamData(application);
                            
                            ShowTableHeader();
                            int j = 1;

                            var appCount = application.Select(l => l.ExamCentreName).Distinct().ToList();

                            foreach (var CentreName in appCount)
                            {
                                var app = application.Where(s => s.ExamCentreName == CentreName).FirstOrDefault();

                                TableRow tr = new TableRow();
                                tr.CssClass = "gdalternate1";

                                TableCell tdRow = new TableCell();
                                tdRow.Width = Unit.Percentage(2);
                                tdRow.Text = j.ToString();
                                tdRow.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow);

                                TableCell tdRow1 = new TableCell();
                                tdRow1.Width = Unit.Percentage(15);
                                if (app.ExamCentreName != null)
                                    tdRow1.Text = app.ExamCentreName.ToString();
                                else
                                    tdRow1.Text = "NA";
                                tdRow1.Wrap = false;
                                tdRow1.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow1);

                                TableCell tdRow2 = new TableCell();
                                tdRow2.Width = Unit.Percentage(15);
                                if (app.ExamCentreName != null)
                                    //tdRow2.Text = context.ExamCenters.Where(s => s.Code.StartsWith(app.ExamCentreName.Substring(0, 3).ToUpper().Trim())).Select(t => t.Name).FirstOrDefault();
                                    tdRow2.Text = app.ExamCntreAddress.ToString();//context.ExamCenters.Where(s => s.Code.StartsWith(app.ExamCentreName.Substring(0, 3).ToUpper().Trim())).Select(t => t.Name).FirstOrDefault();
                                else
                                    tdRow2.Text = "NA";
                                tdRow2.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow2);

                                TableCell tdRow3 = new TableCell();
                                tdRow3.Width = Unit.Percentage(15);
                                if (app.ExamCentreName != null)
                                    tdRow3.Text = context.ExamCenters.Where(s => s.Code.StartsWith(app.ExamCentreName.Substring(0, 3).ToUpper().Trim())).Select(t => t.State.Name).FirstOrDefault();
                                else
                                    tdRow3.Text = "NA";
                                tdRow3.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow3);

                                TableCell tdRow4 = new TableCell();
                                tdRow4.Width = Unit.Percentage(15);
                                if (application != null)
                                    //tdRow4.Text = application.Count().ToString();
                                    tdRow4.Text = application.Where(s => s.ExamCentreName == CentreName).Count().ToString();
                                else
                                    tdRow4.Text = "0";
                                tdRow4.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow4);

                            //    var absent = (from r in context.ResultGrades
                            //                    where r.CourseCategoryID == 2
                            //                    && r.Code.ToUpper().Trim() == "ABS".Trim()
                            //                    select 
                            //                    new ID = r.ID
                            //}).Distinct().ToList();

                             var absent = (from a in context.ResultGrades
                                                where a.CourseCategoryID == 2
                                                && a.Description.ToUpper().Trim() == "Pass".Trim()
                                                || a.Description.ToUpper().Trim() == "Fail".Trim()
                                                select a.ID).ToArray();
                             Int32 totalabsstudent = 0; 
                             for (int i = 0; i < absent.Length; i++)
                             {
                                 var total1 = application.Where(s => s.ExamCentreName == CentreName && s.ResultGradeID.Value == absent[i]).Count();
                                 totalabsstudent += total1;
                             }
                             
                                 TableCell tdRow5 = new TableCell();
                                 tdRow5.Width = Unit.Percentage(15);
                                 if (totalabsstudent != 0)
                                     tdRow5.Text = totalabsstudent.ToString();
                                 else
                                     tdRow5.Text = "0";
                                tdRow5.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow5);
                            
                                var paasedcodes = (from r in context.ResultGrades
                                                   where r.CourseCategoryID == 2
                                                   && r.Description.ToUpper().Trim() == "PASS".Trim()
                                                   select r.ID).ToArray();

                                Int32 totalpassedstudent = 0;
                                for (int i = 0; i < paasedcodes.Length; i++)
                                {
                                    var total1 = application.Where(s => s.ExamCentreName == CentreName &&  s.ResultGradeID.Value == paasedcodes[i]).Count();
                                    totalpassedstudent += total1;
                                }

                                TableCell tdRow6 = new TableCell();
                                tdRow6.Width = Unit.Percentage(15);
                                if (totalpassedstudent != 0)
                                    tdRow6.Text = totalpassedstudent.ToString();
                                else
                                    tdRow6.Text = "0";
                                tdRow6.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow6);

                                float per = 0;
                                if (totalpassedstudent != 0 && totalabsstudent != null)
                                {
                                    float passedstudent = Convert.ToSingle(totalpassedstudent.ToString());
                                    float totalstudent = Convert.ToSingle(totalabsstudent.ToString());
                                    if (passedstudent == 0 && totalstudent == 0)
                                    {
                                        per = 0;
                                    }
                                    else
                                    {
                                        float ratio = ((passedstudent) / (totalstudent));
                                        per = ratio * 100;
                                    }
                                }
                                TableCell tdRow7 = new TableCell();
                                tdRow7.Width = Unit.Percentage(10);
                                tdRow7.Text = per.ToString("F");
                                tdRow7.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow7);

                                tbl.Rows.Add(tr);
                                j++;
                            }
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No Record Found";
                        }
                    };
                }
                #endregion----------------

                #region------Courese-Exam-Application----------
                else if (TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    using (context = new EConnectContext())
                    {
                        var application = (from a in context.CourseExamApplications
                                           join t in context.ExamWiseExamCenters on a.AllottedExamCentreID equals t.ExamCenterID
                                           where a.ExamID == ExamId && a.CourseID == CourseId
                                           select new 
                                           {
                                               ID = a.ID,
                                               AllottedExamCentreID = a.AllottedExamCentreID,
                                               AllottedExamStateID = a.AllottedExamStateID,
                                               InstituteID = a.InstituteID,
                                               ApplicantTypeID = a.ApplicantTypeID,
                                               AllottedExamCentreCode = a.AllottedExamCentre.Code,
                                               AllottedExamCentreName = a.AllottedExamCentre.Name,
                                               AllottedExamCentreStateName = a.AllottedExamCentre.State.Name

                                           }).ToList();
                        if (examCenter != 0)
                            application = application.Where(s => s.AllottedExamCentreID == examCenter).ToList();
                        if (stateID != 0)
                            application = application.Where(s => s.AllottedExamStateID == stateID).ToList();
                        if (loginUserType == UserType.Institute)
                            application = application.Where(s => s.InstituteID == entityID).ToList();
                        if (loginUserType == UserType.HeadOffice)
                        {
                            if (applicantTypeID == 1)
                            {
                                application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)).ToList();
                            }
                            else if (applicantTypeID == 2)
                            {
                                application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                            }
                        }
                        if (loginUserType == UserType.RegionalCenter)
                        {
                            if (applicantTypeID == 1)
                            {
                                application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)).ToList();
                            }
                            else if (applicantTypeID == 2)
                            {
                                application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                            }
                        }
                        if (loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
                        {
                            if (applicantTypeID == 1)
                            {
                                application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)).ToList();
                            }
                            else if (applicantTypeID == 2)
                            {
                                application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                            }
                        }
                        if (application.Count() > 0)
                        {
                            //ShowCourseExamData(application);
                            ShowTableHeader();
                            int j = 1;
                            var app = application.FirstOrDefault();

                            TableRow tr = new TableRow();
                            tr.CssClass = "gdalternate1";

                            TableCell tdRow = new TableCell();
                            tdRow.Width = Unit.Percentage(1);
                            tdRow.Text = j.ToString();
                            tdRow.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow);

                            TableCell tdRow1 = new TableCell();
                            tdRow1.Width = Unit.Percentage(1);
                            if (app != null)
                                tdRow1.Text = app.AllottedExamCentreCode.ToString();
                            else
                                tdRow1.Text = "NA";
                            tdRow1.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow1);

                            TableCell tdRow2 = new TableCell();
                            tdRow2.Width = Unit.Percentage(15);
                            if (app != null)
                                tdRow2.Text = app.AllottedExamCentreName.ToString();
                            else
                                tdRow2.Text = "NA";
                            tdRow2.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow2);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(15);
                            if (app != null)
                                tdRow3.Text = app.AllottedExamCentreStateName.ToString();
                            else
                                tdRow3.Text = "NA";
                            tdRow3.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow3);

                            var application1 = (from a in context.CourseExamApplications
                                                where (a.ExamID == ExamId && a.CourseID == CourseId && a.AllottedExamCentreID == app.AllottedExamCentreID
                                                      )
                                                select a.ID).ToArray();

                            var query = (from p in context.CourseExamApplicationDetails
                                         where p.ResultGradeID != null && application1.Contains(p.CourseExamApplicationID.Value)
                                         select p).Distinct().ToList();


                            TableCell tdRow4 = new TableCell();
                            tdRow4.Width = Unit.Percentage(15);
                            if (query != null)
                                tdRow4.Text = query.Select(t => t.CandidateID).Distinct().Count().ToString();
                            else
                                tdRow4.Text = "0";
                            tdRow4.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow4);

                            Int32 absent = (from r in context.ResultGrades
                                            where r.CourseCategoryID == 1
                                            && r.Code.ToUpper().Trim() == "ABS".Trim()
                                            select r.ID).FirstOrDefault();

                            var total2 = query.Where(s => s.ResultGradeID.Value != absent).Select(t => t.CandidateID).Distinct();
                            TableCell tdRow5 = new TableCell();
                            tdRow5.Width = Unit.Percentage(15);
                            if (total2 != null)
                                tdRow5.Text = total2.Count().ToString();
                            else
                                tdRow5.Text = "0";
                            tdRow5.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow5);

                            var paasedcodes = (from r in context.ResultGrades
                                               where r.CourseCategoryID == 1
                                               && r.Description.ToUpper().Trim() == "PASS".Trim()
                                               select r.ID).ToArray();

                            var totalpassedstudent = query.Where(s => paasedcodes.Contains(s.ResultGradeID.Value)).Select(t => t.CandidateID).Distinct().Count();

                            TableCell tdRow6 = new TableCell();
                            tdRow6.Width = Unit.Percentage(15);
                            if (totalpassedstudent != 0)
                                tdRow6.Text = totalpassedstudent.ToString();
                            else
                                tdRow6.Text = "0";
                            tdRow6.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow6);

                            float per = 0;
                            if (totalpassedstudent != 0 && total2 != null)
                            {
                                float passedstudent = Convert.ToSingle(totalpassedstudent.ToString());
                                float totalstudent = Convert.ToSingle(total2.Count().ToString());
                                if (passedstudent == 0 && totalstudent == 0)
                                {
                                    per = 0;
                                }
                                else
                                {
                                    float ratio = ((passedstudent) / (totalstudent));
                                    per = ratio * 100;
                                }
                            }
                            TableCell tdRow7 = new TableCell();
                            tdRow7.Width = Unit.Percentage(15);
                            tdRow7.Text = per.ToString("F");
                            tdRow7.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow7);

                            tbl.Rows.Add(tr);
                            j++;





                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No Record Found";
                        }
                    };
                }
                #endregion----------
            }
            LblRptSubHeader.Text = strHead;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally{ context.Dispose();}

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
    protected void imgAccess_Click(object sender, ImageClickEventArgs e)
    {

        OleDbConnection connection = new OleDbConnection(); ;
        OleDbCommand command = new OleDbCommand();
        int CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
        string Access = "";
        var filename = "Applications_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".mdb";
        //RC-CHA_Apllications_CCC_Jan2013.mdb
        try
        {

            Course currentCourse = context.Courses.Find(CourseId);
            if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
            {
                var result = (from c in context.CertificateExamApplications
                              select new
                              {
                                  Folder_no = "",
                                  batch_no = "",
                                  sr_no = c.ID,
                                  exam_year = c.Exam.Name,
                                  NAME = c.Name,
                                  F_NAME = c.FatherName,
                                  M_NAME = c.MotherName,
                                  D_O_B = c.DateOfBirth,
                                  SEX = c.Gender,
                                  H_Qual = c.EducationalQualification.Code,
                                  ADD1 = c.CorAddressLine1,
                                  ADD2 = c.CorAddressLine2,
                                  ADD3 = c.CorAddressLine3,
                                  CITY = c.CorDistrict.Name,
                                  STATE = c.CorState.Name,
                                  PINCODE = c.CorPinCode,
                                  PH_NO_C = (c.PhoneNumber.HasValue ? c.PhoneNumber.Value : 0),
                                  EMAIL_C = c.EmailAddress,
                                  CCC_NO = (c.InstituteID.HasValue ? c.InstituteID.Value : 0),
                                  INST_NAME = c.Institute.Name,
                                  courseID = c.CourseID,
                                  INST_ADD = c.Institute.AddressLine1 + "  " + c.Institute.AddressLine2 + "   " + c.Institute.State.Name,
                                  INST_STAT = c.Institute.AccreditationDetails.FirstOrDefault().AccreditationStatus.Code,
                                  TH_CENT_CH = c.ExamCenter1.Code,
                                  SEC_TH_CEN = c.ExamCenter2.Code,
                                  OCCUPATION = c.Occupation.Name,
                                  CATEGORY = c.CastCategory.Code,
                                  PREV_APP = (c.AlreadyApplied == true) ? "Y" : "N",
                                  PREV_M = c.PreviousExam.Name,
                                  PREV_YEAR = "",//need to work on this
                                  PREV_ROLL = (!string.IsNullOrEmpty(c.PreviousRollNumber)) ? c.PreviousRollNumber : "NA",
                                  Rollno = "",
                                  cent_allot = "",
                                  cent_add = "",
                                  examDate = c.Exam.ExamStartDate,
                                  batch = "",
                                  rep_time = "",
                              });
                Access = Server.MapPath("~/Download/" + filename);
                //Response.Write(Access);
                //Response.End();
                result = result.Where(p => p.courseID == CourseId);
                if (System.IO.File.Exists(Access))
                    System.IO.File.Delete(Access);
                System.IO.File.Copy(Server.MapPath("~/Download/Database_format_BCC.mdb"), Access);
                string connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Access + ";Persist Security Info=False;";
                connection.ConnectionString = connect;
                connection.Open();
                //command = new OleDbCommand("delete from  [MS Access;Database="+ Access + "].[Exam_Database]", connection);
                //December_2024
                command = new OleDbCommand("delete from  [MS Access;Database=@access].[Exam_Database]", connection);
                command.Parameters.AddWithValue("@access", Access);
                command.ExecuteNonQuery();
                foreach (var certificate in result)
                {
                    //command = new OleDbCommand("insert into [MS Access;Database=" + Access + "].[Exam_Database](Folder_no, batch_no,sr_no,exam_year,NAME,F_NAME,M_NAME,D_O_B,SEX,H_Qual,ADD1,ADD2,ADD3,CITY,STATE,PINCODE,PH_NO_C,EMAIL_C,CCC_NO,INST_NAME,INST_ADD,INST_STAT,TH_CENT_CH,SEC_TH_CEN,OCCUPATION,CATEGORY,PREV_APP,PREV_M,PREV_YEAR,PREV_ROLL,Rollno,cent_allot,cent_add,examDate,batch,rep_time)values('" + certificate.Folder_no + "','" + certificate.batch_no + "','" + certificate.sr_no + "','" + certificate.exam_year + "','" + certificate.NAME + "','" + certificate.F_NAME + "','" + certificate.M_NAME + "','" + certificate.D_O_B + "','" + certificate.SEX + "','" + certificate.H_Qual + "','" + certificate.ADD1 + "','" + certificate.ADD2 + "','" + certificate.ADD3 + "','" + certificate.CITY + "','" + certificate.STATE + "','" + certificate.PINCODE + "','" + certificate.PH_NO_C + "','" + certificate.EMAIL_C + "','" + certificate.CCC_NO + "','" + certificate.INST_NAME + "','" + certificate.INST_ADD + "','" + certificate.INST_STAT + "','" + certificate.TH_CENT_CH + "','" + certificate.SEC_TH_CEN + "','" + certificate.OCCUPATION + "','" + certificate.CATEGORY + "','" + certificate.PREV_APP + "','" + certificate.PREV_M + "','" + certificate.PREV_YEAR + "','" + certificate.PREV_ROLL + "','" + certificate.Rollno + "','" + certificate.cent_allot + "','" + certificate.cent_add + "','" + certificate.examDate + "','" + certificate.batch + "','" + certificate.rep_time + "')", connection);
                    //December_2024
                    command = new OleDbCommand("insert into [MS Access;Database=@access].[Exam_Database](Folder_no, batch_no,sr_no,exam_year,NAME,F_NAME,M_NAME,D_O_B,SEX,H_Qual,ADD1,ADD2,ADD3,CITY,STATE,PINCODE,PH_NO_C,EMAIL_C,CCC_NO,INST_NAME,INST_ADD,INST_STAT,TH_CENT_CH,SEC_TH_CEN,OCCUPATION,CATEGORY,PREV_APP,PREV_M,PREV_YEAR,PREV_ROLL,Rollno,cent_allot,cent_add,examDate,batch,rep_time)values(@Folder_no,@batch_no,@sr_no,@exam_year,@NAME,@F_NAME,@M_NAME,@D_O_B,@SEX,@H_Qual,@ADD1,@ADD2,@ADD3,@CITY,@STATE,@PINCODE,@PH_NO_C,@EMAIL_C,@CCC_NO,@INST_NAME,@INST_ADD,@INST_STAT,@TH_CENT_CH,@SEC_TH_CEN,@OCCUPATION,@CATEGORY,@PREV_APP,@PREV_M,@PREV_YEAR,@PREV_ROLL,@Rollno,@cent_allot,@cent_add,@examDate,@batch,@rep_time)", connection);
                    command.Parameters.AddWithValue("@access", Access);
                    command.Parameters.AddWithValue("@Folder_no", certificate.Folder_no);
                    command.Parameters.AddWithValue("@batch_no", certificate.batch_no);
                    command.Parameters.AddWithValue("@sr_no", certificate.sr_no);
                    command.Parameters.AddWithValue("@exam_year", certificate.exam_year);
                    command.Parameters.AddWithValue("@NAME", certificate.NAME);
                    command.Parameters.AddWithValue("@F_NAME", certificate.F_NAME);
                    command.Parameters.AddWithValue("@M_NAME", certificate.M_NAME);
                    command.Parameters.AddWithValue("@D_O_B", certificate.D_O_B);
                    command.Parameters.AddWithValue("@SEX", certificate.SEX);
                    command.Parameters.AddWithValue("@H_Qual", certificate.H_Qual);
                    command.Parameters.AddWithValue("@ADD1", certificate.ADD1);
                    command.Parameters.AddWithValue("@ADD2", certificate.ADD2);
                    command.Parameters.AddWithValue("@ADD3", certificate.ADD3);
                    command.Parameters.AddWithValue("@CITY", certificate.CITY);
                    command.Parameters.AddWithValue("@STATE", certificate.STATE);
                    command.Parameters.AddWithValue("@PINCODE", certificate.PINCODE);
                    command.Parameters.AddWithValue("@PH_NO_C", certificate.PH_NO_C);
                    command.Parameters.AddWithValue("@EMAIL_C", certificate.EMAIL_C);
                    command.Parameters.AddWithValue("@CCC_NO", certificate.CCC_NO);
                    command.Parameters.AddWithValue("@INST_NAME", certificate.INST_NAME);
                    command.Parameters.AddWithValue("@INST_ADD", certificate.INST_ADD);
                    command.Parameters.AddWithValue("@INST_STAT", certificate.INST_STAT);
                    command.Parameters.AddWithValue("@TH_CENT_CH", certificate.TH_CENT_CH);
                    command.Parameters.AddWithValue("@SEC_TH_CEN", certificate.SEC_TH_CEN);
                    command.Parameters.AddWithValue("@OCCUPATION", certificate.OCCUPATION);
                    command.Parameters.AddWithValue("@CATEGORY", certificate.CATEGORY);
                    command.Parameters.AddWithValue("@PREV_APP", certificate.PREV_APP);
                    command.Parameters.AddWithValue("@PREV_M", certificate.PREV_M);
                    command.Parameters.AddWithValue("@PREV_YEAR", certificate.PREV_YEAR);
                    command.Parameters.AddWithValue("@PREV_ROLL", certificate.PREV_ROLL);
                    command.Parameters.AddWithValue("@Rollno", certificate.Rollno);
                    command.Parameters.AddWithValue("@cent_allot", certificate.cent_allot);
                    command.Parameters.AddWithValue("@cent_add", certificate.cent_add);
                    command.Parameters.AddWithValue("@examDate", certificate.examDate);
                    command.Parameters.AddWithValue("@batch", certificate.batch);
                    command.Parameters.AddWithValue("@rep_time", certificate.rep_time);
                    command.ExecuteNonQuery();
                }
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Charset = "";
                Response.ContentType = "Application/vnd.mdb";
                //Response.TransmitFile(Access);
                Response.WriteFile(Access);
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);

        }
        finally
        {
            context.Dispose();
            command.Dispose();
            connection.Close();
            connection.Dispose();
            //context.Database.ExecuteSqlCommand("insert into [MS Access;Database=" + Access + "].[Temp] values('" + certificate + "')");
            //context.Database.ExecuteSqlCommand("SELECT * INTO [MS Access;Database=" + Access + "].[Temp][ C_Name] FROM ['" + certificate.C_Name + "'] ");
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        OleDbCommand cmdAccess = new OleDbCommand();
        OleDbDataAdapter oda = new OleDbDataAdapter();
        OleDbConnection conAccess = new OleDbConnection();
        String msg = "";
        string FilePath = "";
        String conStr = "";
        try
        {
            if (FileUpload1.HasFile)
            {

                String FileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                FileName += "_Application_" + DateTime.Now.ToString("ddMMyyyyhhmmss");
                var filename = "Applications_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".mdb";
                String Extension = Path.GetExtension(FileUpload1.PostedFile.FileName.ToLower());
                FilePath = Server.MapPath("~/UploadedFiles/" + FileName);
                FileUpload1.SaveAs(FilePath);
                if (Extension.Trim() == ".mdb")
                {
                    conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Persist Security Info=False"; ;
                }
                else
                {
                    ShowAlert("Please Choose .mdb  Extension File", true);
                    return;
                }
                conAccess = new OleDbConnection(conStr);
                cmdAccess.Connection = conAccess;
                conAccess.Open();
                DataTable dtAccess = new DataTable();
                DataTable dt = new DataTable();
                dtAccess = conAccess.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                String SheetName = dtAccess.Rows[0]["TABLE_NAME"].ToString();
                //cmdAccess.CommandText = "select sr_no,Rollno,cent_allot,cent_add,batch,rep_time,examDate from ["+ SheetName + "]";
                //December_2024
                cmdAccess.CommandText = "select sr_no,Rollno,cent_allot,cent_add,batch,rep_time,examDate from [@SheetName]";
                cmdAccess.Parameters.AddWithValue("@SheetName", SheetName);
                oda.SelectCommand = cmdAccess;
                oda.Fill(dt);
                //int i = 0;
                int succeedCounter = 0;
                int failedCounter = 0;
                //int flag = 0;
                if (dt.Rows.Count > 0)
                {

                    using (EConnectContext context = new EConnectContext())
                    {
                        foreach (DataRow dtrow in dt.Rows)
                        {
                            if (String.IsNullOrEmpty(dtrow[1].ToString()) || String.IsNullOrEmpty(dtrow[2].ToString()) || String.IsNullOrEmpty(dtrow[3].ToString()) || String.IsNullOrEmpty(dtrow[4].ToString()) || String.IsNullOrEmpty(dtrow[5].ToString()) || String.IsNullOrEmpty(dtrow[6].ToString()))
                            {
                                failedCounter += 1;
                            }
                            else
                            {
                                //String sql = "select Roll_Number from Certificate_Exam_Application where ID='" + dtrow["sr_no"].ToString() + "'";
                                //DataTable dtTbl = DbUtility.GetDataTable(sql, new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
                                //December_2024
                                SqlParameter[] param1 = { new SqlParameter("@sr_no", dtrow["sr_no"].ToString()) };
                                String sql = "select Roll_Number from Certificate_Exam_Application where ID=@sr_no";
                                DataTable dtTbl = DbUtility.GetDataTable(sql, new EConnect.Connections.SqlCon(), param1, CommandType.Text, false);
                                if (dtTbl.Rows[0][0] is DBNull)
                                {

                                    //context.Database.ExecuteSqlCommand("update Certificate_Exam_Application set  " +
                                    //                                   "Roll_Number =" + Convert.ToInt64((dtrow["Rollno"].ToString().Trim())) + "," +
                                    //                                   "Exam_Centre_Name='" + dtrow["cent_allot"].ToString().Trim() + "'," +
                                    //                                   "Exam_Centre_Address='" + dtrow["cent_add"].ToString().Trim() + "'," +
                                    //                                   "Exam_Batch_Number='" + dtrow["batch"].ToString().Trim() + "'," +
                                    //                                   "Reporting_Time='" + dtrow["rep_time"].ToString().Trim() + "'," +
                                    //                                   "Application_Status_ID= " + Convert.ToInt32(enmCertificateExamApplicationStatus.ExamCentreAndRollNumberAlloted) + "," +
                                    //                                   "Updated_On='" + Convert.ToDateTime(DateTime.Now) + "'," +
                                    //                                   "Date_of_Exam='" + Convert.ToDateTime(dtrow["examDate"].ToString()) + "'," +
                                    //                                   "Updated_By=" + Convert.ToInt32(Session["UserID"]) + " where ID='" + dtrow["sr_no"].ToString() + "'");

                                    //December_2024
                                    SqlParameter[] param2 = { new SqlParameter("@Rollno", Convert.ToInt64((dtrow["Rollno"].ToString().Trim()))),
                                                                    new SqlParameter("@cent_allot",dtrow["cent_allot"].ToString().Trim()),
                                                                        new SqlParameter("@cent_add", dtrow["cent_add"].ToString().Trim()),
                                                                            new SqlParameter("@batch", dtrow["batch"].ToString().Trim() ),
                                                                                new SqlParameter("@rep_time", dtrow["rep_time"].ToString().Trim()),
                                                                                    new SqlParameter("@Application_Status_ID", Convert.ToInt32(enmCertificateExamApplicationStatus.ExamCentreAndRollNumberAlloted)),
                                                                                        new SqlParameter("@currentDate", Convert.ToDateTime(DateTime.Now)),
                                                                                            new SqlParameter("@examDate", Convert.ToDateTime(dtrow["examDate"].ToString())),
                                                                                                new SqlParameter("@UserID", Convert.ToInt32(Session["UserID"])),
                                                                                                    new SqlParameter("@sr_no", dtrow["sr_no"].ToString())
                                                                                                            };
                                    context.Database.ExecuteSqlCommand("update Certificate_Exam_Application set  " +
                                                                       "Roll_Number =@Rollno," +
                                                                       "Exam_Centre_Name=@cent_allot," +
                                                                       "Exam_Centre_Address=@cent_add," +
                                                                       "Exam_Batch_Number=@batch," +
                                                                       "Reporting_Time=@rep_time," +
                                                                       "Application_Status_ID= @Application_Status_ID," +
                                                                       "Updated_On=@currentDate," +
                                                                       "Date_of_Exam=@examDate," +
                                                                       "Updated_By=@UserID where ID=@sr_no", param2);

                                    succeedCounter += 1;
                                }
                            }

                        }

                        context.SaveChanges();
                        if (succeedCounter == 0 && failedCounter == 0)
                        {
                            msg = "This Data Is Already Updated";
                        }
                        else
                        {
                            msg = succeedCounter.ToString() + " records  are updated";
                            if (failedCounter > 0)
                                msg += " , " + failedCounter.ToString() + " record could not be updated";
                        }
                        ShowAlert(msg, true);
                    };
                }
            }
            else
            {
                throw new Exception("Please select document to be uploaded");
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally
        {
            cmdAccess.Dispose();
            conAccess.Close();
            conAccess.Dispose();
            oda.Dispose();
            File.Delete(FilePath);
        }
    }
}