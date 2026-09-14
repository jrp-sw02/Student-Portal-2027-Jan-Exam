using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class HO_Rpt_DuplicateCandidateReport : BasePage
{
    Table tbl = new Table();
    public Int32 CourseCatID;
    public Int32 ApptypeID;
    public Int32 BatchID;
    public Int32 ExamId;
    public Int32 Courseid;
    Int32 currentRoleId = 0;
    EConnectContext context;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/DuplicateRecordsFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

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
        { ShowAlert(ex.Message); }
    }
    protected void ShowTableHeader()
    {
        try
        {

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(1);
            tcCol1.HorizontalAlign = HorizontalAlign.Left;
            tcCol1.Text = "#";
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.HorizontalAlign = HorizontalAlign.Left;
            tcCol.Text = "Application No.";
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(2);
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            tcCol2.Text = "Level";
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Width = Unit.Percentage(10);
            tcCol3.Text = "Candidate Name";
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            tcCol4.Width = Unit.Percentage(20);
            tcCol4.Text = "Father / Mother Name";
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(12);
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            tcCol5.Text = "Date of Birth";
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(12);
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            tcCol6.Text = "Duplicate Record Details";
            th.Cells.Add(tcCol6);

            tbl.Rows.Add(th);
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
            LiteralControl link = new LiteralControl();
            CourseCatID = Convert.ToInt32(Request.QueryString["CourseCatId"]);
            ApptypeID = Convert.ToInt32(Request.QueryString["AppTypeId"]);
            BatchID = Convert.ToInt32(Request.QueryString["BatchId"]);
            ExamId = Convert.ToInt32(Request.QueryString["Examid"]);
            Courseid = Convert.ToInt32(Request.QueryString["CourseID"]);
            string strHead = "";
            if (CourseCatID != 0)
            {
                var CourseCategory = context.CourseCategories.Find(CourseCatID);
                if (CourseCategory != null)
                {
                    strHead += "<br/><b>Category Name :</b>" + CourseCategory.Name;
                }
            }
            else
            {
                strHead += "<br/><b>Category Name :</b> All";
            }
            if (Courseid != 0)
            {
                var course = context.Courses.Find(Courseid);
                if (course != null)
                {
                    strHead += "<br/><b>Course Name :</b>" + course.Name;
                }
            }
            else
            {
                strHead += "<br/><b>Course Name :</b> All";
            }
            if (ExamId != 0)
            {
                var exams = context.Exams.Find(ExamId);
                if (exams != null)
                {
                    strHead += "<br/><b>Exam Name :</b>" + exams.Name;
                }
            }
            else
            {
                strHead += "<br/><b>Exam Name :</b> All";
            }
            strHead += "</br><b> Application Type :</b>" + EConnect.Utils.Common.EnumUtility.GetDescription((enmApplicationType)(ApptypeID));

            if (ApptypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
            {

                var application = (from c in context.BatchItems
                                   where c.BatchID == BatchID
                                   orderby c.CourseRegistrationApplicationID
                                   select new
                                   {

                                       Level = c.CourseRegistrationApplication.Course.Code,
                                       Dob = c.CourseRegistrationApplication.DateOfBirth,
                                       fathername = c.CourseRegistrationApplication.FatherName,
                                       mothername = c.CourseRegistrationApplication.MotherName,
                                       guardianname = c.CourseRegistrationApplication.GuardianName,
                                       Number = c.CourseRegistrationApplication.Number,
                                       batchitemid = c.ID,
                                       name = c.CourseRegistrationApplication.Name
                                   }).ToList();

                if (application.Count() > 0)
                {

                    ShowTableHeader();
                    int i = 1;
                    foreach (var app in application)
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
                        tdRow1.Width = Unit.Percentage(2);
                        tdRow1.Text = app.Number.ToString();
                        tdRow1.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow1);

                        TableCell tdRow5 = new TableCell();
                        tdRow5.Width = Unit.Percentage(3);
                        if (app.Level != null)
                            tdRow5.Text = app.Level.ToString().ToUpper();
                        tdRow5.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow5);


                        TableCell tdRow6 = new TableCell();
                        tdRow6.Width = Unit.Percentage(22);
                        if (app.name != null)
                            tdRow6.Text = GetInitCap(app.name.ToString());
                        tdRow6.HorizontalAlign = HorizontalAlign.Left;
                        tdRow6.Wrap = false;
                        tr.Cells.Add(tdRow6);


                        if (string.IsNullOrEmpty(app.guardianname) == true && string.IsNullOrWhiteSpace(app.guardianname) == true)
                        {
                            TableCell tdRow4 = new TableCell();
                            tdRow4.Width = Unit.Percentage(25);
                            if (app.fathername != null && app.mothername != null)
                                tdRow4.Text = GetInitCap(app.fathername.ToString()) + " / " + GetInitCap(app.mothername.ToString());
                            tdRow4.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow4);
                        }
                        else
                        {
                            TableCell tdRow4 = new TableCell();
                            tdRow4.Width = Unit.Percentage(25);
                            if (app.guardianname != null)
                                tdRow4.Text = GetInitCap(app.guardianname.ToString());
                            tdRow4.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow4);

                        }

                        TableCell tdRow7 = new TableCell();
                        tdRow7.Width = Unit.Percentage(10);
                        if (app.Dob != null)
                            tdRow7.Text = app.Dob.ToString("dd-MMM-yyyy");
                        tdRow7.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow7);

                        var duplicate = (from a in context.CourseRegistrationApplications
                                         where a.BatchItemID == app.batchitemid && a.FinalSubmitted == true
                                         select a).FirstOrDefault();
                        if (duplicate != null)
                        {
                            if (String.IsNullOrEmpty(duplicate.GuardianName) || String.IsNullOrWhiteSpace(duplicate.GuardianName))
                            {

                                var candidatelist = (from s in context.Candidates
                                                     where ((s.Name.ToUpper() == duplicate.Name.ToUpper() && s.DateOfBirth == duplicate.DateOfBirth)
                                                         || (s.Name.ToUpper() == duplicate.Name.ToUpper() && s.FatherName.ToUpper() == duplicate.FatherName.ToUpper())
                                                         || (s.Name.ToUpper() == duplicate.Name.ToUpper() && s.MotherName.ToUpper() == duplicate.MotherName.ToUpper())
                                                         || (s.FatherName.ToUpper() == duplicate.FatherName.ToUpper() && s.DateOfBirth == duplicate.DateOfBirth)
                                                         || (s.FatherName.ToUpper() == duplicate.FatherName.ToUpper() && s.MotherName.ToUpper() == duplicate.MotherName.ToUpper())
                                                         || (s.DateOfBirth == duplicate.DateOfBirth && s.MotherName.ToUpper() == duplicate.MotherName.ToUpper())
                                                         )
                                                     select s).ToList();

                                if (duplicate.CandidateID != 0 && duplicate.CandidateID != null)
                                {
                                    candidatelist = candidatelist.Where(s => s.ID != duplicate.CandidateID).ToList();
                                }
                                if (candidatelist.Count() > 0)
                                {
                                    foreach (var candidate in candidatelist)
                                    {
                                        if (candidate != null)
                                        {
                                            var registration = (from c in context.RegistrationDetails
                                                                where c.CandidateID == candidate.ID
                                                                select new
                                                                {
                                                                    regno = c.RegistrationNo,
                                                                }).FirstOrDefault();
                                            if (registration != null)
                                            {
                                                link.Text += "<a onclick=\"window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../../CAND/DuplicateCandidateList.aspx?candidateID=" + candidate.ID) + "');\" target='_blank' style=\"cursor:pointer;\">" + CommonFunctions.GetInitCap(candidate.Name) + " ( " + registration.regno + " ) " + "</a>, ";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    link.Text = " No, Duplicate Record Found.";
                                }
                            }
                            else
                            {

                                var candidatelist = (from s in context.Candidates
                                                     where ((s.Name.ToUpper() == duplicate.Name.ToUpper() && s.DateOfBirth == duplicate.DateOfBirth)
                                                         || (s.Name.ToUpper() == duplicate.Name.ToUpper() && s.GuardianName.ToUpper() == duplicate.GuardianName.ToUpper())
                                                         || (s.DateOfBirth == duplicate.DateOfBirth && s.GuardianName.ToUpper() == duplicate.GuardianName.ToUpper())
                                                     )
                                                     select s).ToList();
                                if (duplicate.CandidateID != 0 && duplicate.CandidateID != null)
                                {
                                    candidatelist = candidatelist.Where(s => s.ID != duplicate.CandidateID).ToList();
                                }
                                if (candidatelist.Count() > 0)
                                {
                                    foreach (var candidate in candidatelist)
                                    {
                                        if (candidate != null)
                                        {
                                            var registration = (from c in context.RegistrationDetails
                                                                where c.CandidateID == candidate.ID
                                                                select new
                                                                {
                                                                    regno = c.RegistrationNo,
                                                                }).FirstOrDefault();
                                            if (registration != null)
                                            {
                                                link.Text += "<a onclick=\"window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("../../CAND/DuplicateCandidateList.aspx?candidateID=" + candidate.ID) + "');\" target='_blank' style=\"cursor:pointer;\">" + CommonFunctions.GetInitCap(candidate.Name) + " ( " + registration.regno + " ) " + "</a>, ";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    link.Text = " No, Duplicate Record Found.";
                                }
                            }
                        }

                        TableCell tdRow10 = new TableCell();
                        tdRow10.Width = Unit.Percentage(35);
                        tdRow10.Text = link.Text.Trim().TrimEnd(',').ToString();
                        tdRow10.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(tdRow10);


                        tbl.Rows.Add(tr);
                        link.Text = "";
                        i++;

                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "No Record Found";
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