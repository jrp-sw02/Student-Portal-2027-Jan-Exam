using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Data;
using System.Data.SqlClient;

public partial class ReportPgae : BasePage
{
    Table tbl = new Table();
    EConnectContext context;
    Int32 StatusId = 0;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 applicantTypeID = 0;
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
        //    if (!IsSessionAlive())
        //    {
        //        Response.Redirect("~/index.aspx");
        //    }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);

            //if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/ApplicationReceiptFilter.aspx"))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {

                tbl.CssClass = "sample3";
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(100);
                ShowData();
                //divReportData.Controls.Add(tbl);
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
            int TypeId = Convert.ToInt32(Request.QueryString["TypeId"]);
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

            if (TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                TableHeaderCell tcCol20 = new TableHeaderCell();
                tcCol20.Width = Unit.Percentage(19);
                tcCol20.HorizontalAlign = HorizontalAlign.Center;
                tcCol20.Text = "Registration Number";
                th.Cells.Add(tcCol20);
            }

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
            tcCol5.Width = Unit.Percentage(5);
            tcCol5.Text = "Mobile";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol30 = new TableHeaderCell();
            tcCol30.Width = Unit.Percentage(7);
            tcCol30.Text = "Email";
            tcCol30.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol30);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(3);
            tcCol6.Text = "Date of Birth";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);
            if (TypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication) || TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                TableHeaderCell tcCol14 = new TableHeaderCell();
                tcCol14.Width = Unit.Percentage(3);
                tcCol14.Text = "First Centre Choice-State";
                tcCol14.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol14);

                TableHeaderCell tcCol15 = new TableHeaderCell();
                tcCol15.Width = Unit.Percentage(3);
                tcCol15.Text = "Second Centre Choice-State";
                tcCol15.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol15);

            }
            if (StatusId == 0)
            {
                TableHeaderCell tcCol7 = new TableHeaderCell();
                tcCol7.Width = Unit.Percentage(16);
                tcCol7.Text = "Application status";
                tcCol7.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol7);
            }

            TableHeaderCell tcCol8 = new TableHeaderCell();
            tcCol8.Width = Unit.Percentage(8);
            tcCol8.Text = "Payment status";
            tcCol8.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol8);

            if (loginUserType == UserType.HeadOffice || loginUserType == UserType.RegionalCenter || loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
            {
                TableHeaderCell tcCol9 = new TableHeaderCell();
                tcCol9.Width = Unit.Percentage(16);
                tcCol9.Text = "Applicant Type";
                tcCol9.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol9);
            }

            TableHeaderCell tcCol17 = new TableHeaderCell();
            tcCol17.Width = Unit.Percentage(8);
            tcCol17.Text = "Fee Amount";
            tcCol17.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol17);

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
            tcCol0.Text = "#";
            th.Cells.Add(tcCol0);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Text = "Name";
            tcCol1.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Text = "Father Name";
            tcCol2.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.HorizontalAlign = HorizontalAlign.Left;
            tcCol3.Text = "Mother Name";
            th.Cells.Add(tcCol3);

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
            applicantTypeID = Convert.ToInt32(Request.QueryString["applicantTypeID"]);
            StatusId = Convert.ToInt32(Request.QueryString["StatusId"]);
            Int32 paymentStatusId = Convert.ToInt32(Request.QueryString["paymentStatusId"]);
            int ExamId = Convert.ToInt32(Request.QueryString["ExamId"]);
            int CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            int TypeId = Convert.ToInt32(Request.QueryString["TypeId"]);
            Int32 Gender = Convert.ToInt32(Request.QueryString["Gender"]);
            Int32 Category = Convert.ToInt32(Request.QueryString["Category"]);
            Int32 Occupation = Convert.ToInt32(Request.QueryString["Occupation"]);
            Int32 CandidateState = Convert.ToInt32(Request.QueryString["CStateID"]);
            Int64 ExamCentre1 = Convert.ToInt64(Request.QueryString["ExamCentre1"]);
            Int32 ExamCentreState1 = Convert.ToInt32(Request.QueryString["Exam1StateID"]);
            Int64 ExamCentre2 = Convert.ToInt64(Request.QueryString["ExamCentre2"]);
            Int32 ExamCentreState2 = Convert.ToInt32(Request.QueryString["Exam2StateID"]);
            Int32 RegionalcentreID = 0;
            Int32 paymentpending = Convert.ToInt32(enmPaymentStatus.Pending);
            Int32[] paymentstatus = { Convert.ToInt32(enmPaymentStatus.Paid), Convert.ToInt32(enmPaymentStatus.PaidButNotVerified), Convert.ToInt32(enmPaymentStatus.Failed) };

            int InstituteID = 0;
            if (loginUserType == UserType.Institute)
                InstituteID = Convert.ToInt32(entityID);
            else
                InstituteID = Convert.ToInt32(Request.QueryString["InstituteID"]);
            if (loginUserType == UserType.RegionalCenter)
                RegionalcentreID = Convert.ToInt32(entityID);
            else
                RegionalcentreID = Convert.ToInt32(Request.QueryString["RegID"]);
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
                # region------Certificate Exam Application
                if (TypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    if (StatusId == 0)
                        strHead += "</br> <b>Application Status :</b> All ";
                    else
                    {
                        if (StatusId == 100)
                        {
                            strHead += "</br> <b>Application Status :</b> " + "Fee Pending to be Paid By Institute";
                        }
                        else
                        {
                            strHead += "</br> <b>Application Status :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmCertificateExamApplicationStatus)(StatusId)).ToString();
                        }
                    }

                    var application = (from a in context.CertificateExamApplications.AsNoTracking()
                                       where a.ExamID == ExamId && a.CourseID == CourseId && a.FinalSubmitted == true
                                       select new
                                       {
                                           ID = a.ID,
                                           ApplicationDate = a.ApplicationDate,
                                           Number = a.Number,
                                           CourseCategoryID = a.CourseCategoryID,
                                           CourseID = a.CourseID,
                                           ExamID = a.ExamID,
                                           ApplicantTypeID = a.ApplicantTypeID,
                                           InstituteID = a.InstituteID,
                                           ExamCenter1ID = a.ExamCenter1ID,
                                           ExamCenter2ID = a.ExamCenter2ID,
                                           RegionalCenterID = a.RegionalCenterID,
                                           Name = a.Name,
                                           FatherName = a.FatherName,
                                           GuardianName = a.GuardianName,
                                           Gender = a.Gender,
                                           DateOfBirth = a.DateOfBirth,
                                           CastCategoryID = a.CastCategoryID,
                                           OccupationID = a.OccupationID,
                                           MobileNumber = a.MobileNumber,
                                           EmailAddress = a.EmailAddress,
                                           FinalSubmitted = a.FinalSubmitted,
                                           PaymentStatusID = a.PaymentStatusID,
                                           ApplicationStatusID = a.ApplicationStatusID,
                                           CorStateID = a.CorStateID,
                                           TotalFeeAmount = a.TotalFeeAmount,
                                           Corstate = a.CorState.Name,
                                           InstituteName = a.Institute.Name,
                                           ExamCenter1Name = a.ExamCenter1.Name,
                                           ExamCenter2Name = a.ExamCenter2.Name,
                                           OccupationName = a.Occupation.Name,
                                           CastCategoryName = a.CastCategory.Name
                                       }).ToList();
                    if (StatusId != 0)
                    {
                        if (StatusId == Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre))
                        {
                            application = application.Where(s => paymentstatus.Contains(s.PaymentStatusID) && s.ApplicationStatusID == StatusId).ToList();
                        }
                        else if (StatusId == 100)
                        {
                            application = application.Where(s => s.PaymentStatusID == paymentpending && s.ApplicationStatusID == Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre)).ToList();
                        }
                        else
                        {
                            application = application.Where(s => s.ApplicationStatusID == StatusId).ToList();
                        }
                    }
                    if (loginUserType == UserType.Institute)
                        application = application.Where(s => s.InstituteID == entityID).ToList();
                    if (loginUserType == UserType.HeadOffice)
                    {
                        if (applicantTypeID == 0)
                            strHead += "</br> <b>Applicant Type :</b>Both";
                        else
                            strHead += "</br> <b>Applicant Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicantType)(applicantTypeID)).ToString();
                        if (applicantTypeID == 1)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)).ToList();
                        }
                        else if (applicantTypeID == 2)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                            if (InstituteID != 0)
                            {
                                application = application.Where(s => s.InstituteID == InstituteID).ToList();
                                Institute inst = context.Institutes.Find(InstituteID);
                                strHead += "</br> <b> Institute Name :</b> " + GetInitCap(inst.Name).ToString();
                            }
                        }
                        if (RegionalcentreID == 0)
                        {
                            strHead += "</br> <b>Regional Centre Name :</b>All";
                        }
                        else
                        {
                            if (RegionalcentreID != 0)
                            {
                                application = application.Where(s => s.RegionalCenterID == RegionalcentreID).ToList();
                                RegionalCenter rc = context.RegionalCenters.Find(RegionalcentreID);
                                strHead += "</br> <b> Regional Centre Name :</b> " + GetInitCap(rc.Name).ToString();
                            }
                        }
                        if (Gender == 0)
                            strHead += "</br> <b>Gender :</b>All";

                        else if (Gender == 1)
                        {
                            string gender = "Female";
                            application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Female";
                        }
                        else if (Gender == 2)
                        {
                            string gender = "Male";
                            application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Male";
                        }
                        if (Category == 0)
                            strHead += "</br> <b>Category :</b>All";

                        else if (Category != 0)
                        {
                            application = application.Where(s => s.CastCategoryID == Category).ToList();
                            CastCategory ct = context.CastCategories.Where(s => s.ID == Category).FirstOrDefault();
                            strHead += "</br> <b>Cateogry:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (Occupation == 0)
                            strHead += "</br> <b>Occupation :</b>All";

                        else if (Occupation != 0)
                        {
                            application = application.Where(s => s.OccupationID == Occupation).ToList();
                            Occupation ct = context.Occupations.Where(s => s.ID == Occupation).FirstOrDefault();
                            strHead += "</br> <b>Occupation:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (CandidateState == 0)
                            strHead += "</br> <b>Candidate State :</b>All";

                        else if (CandidateState != 0)
                        {
                            application = application.Where(s => s.CorStateID == CandidateState).ToList();
                            Location ct = context.Locations.Where(s => s.ID == CandidateState && s.LocationTypeID == 2).FirstOrDefault();
                            strHead += "</br> <b>Candidate State:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (ExamCentre1 == 0)
                            strHead += "</br> <b>Exam Centre Choice 1 :</b>All";

                        else if (ExamCentre1 != 0)
                        {
                            application = application.Where(s => s.ExamCenter1ID == ExamCentre1).ToList();
                            ExamCenter ct = context.ExamCenters.Where(s => s.ID == ExamCentre1).FirstOrDefault();
                            strHead += "</br> <b>Exam Centre Choice 1 :</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (ExamCentre2 == 0)
                            strHead += "</br> <b>Exam Centre Choice 2 :</b>All";

                        else if (ExamCentre2 != 0)
                        {
                            application = application.Where(s => s.ExamCenter2ID == ExamCentre2).ToList();
                            ExamCenter ct = context.ExamCenters.Where(s => s.ID == ExamCentre2).FirstOrDefault();
                            strHead += "</br> <b>Exam Centre Choice 2 :</b>" + GetInitCap(ct.Name.ToString());
                        }

                    }
                    if (loginUserType == UserType.RegionalCenter)
                    {
                        RegionalCenter regcenter = context.RegionalCenters.Where(s => s.ID == entityID).FirstOrDefault();
                        strHead += "</br> <b>Regional Centre Name :</b>" + GetInitCap(regcenter.Name);
                        if (applicantTypeID == 0)
                            strHead += "</br> <b>Applicant Type :</b>Both";
                        else
                            strHead += "</br> <b>Applicant Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicantType)(applicantTypeID)).ToString();
                        if (applicantTypeID == 1)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)).ToList();
                        }
                        else if (applicantTypeID == 2)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                            if (InstituteID != 0)
                            {
                                application = application.Where(s => s.InstituteID == InstituteID).ToList();
                                Institute inst = context.Institutes.Find(InstituteID);
                                strHead += "</br> <b> Institute Name :</b> " + GetInitCap(inst.Name).ToString();
                            }
                        }
                        application = application.Where(s => s.RegionalCenterID == entityID).ToList();
                        if (Gender == 0)
                            strHead += "</br> <b>Gender :</b>All";

                        else if (Gender == 1)
                        {
                            string gender = "Female";
                            application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Female";
                        }
                        else if (Gender == 2)
                        {
                            string gender = "Male";
                            application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Male";
                        }
                        if (Category == 0)
                            strHead += "</br> <b>Category :</b>All";

                        else if (Category != 0)
                        {
                            application = application.Where(s => s.CastCategoryID == Category).ToList();
                            CastCategory ct = context.CastCategories.Where(s => s.ID == Category).FirstOrDefault();
                            strHead += "</br> <b>Cateogry:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (Occupation == 0)
                            strHead += "</br> <b>Occupation :</b>All";

                        else if (Occupation != 0)
                        {
                            application = application.Where(s => s.OccupationID == Occupation).ToList();
                            Occupation ct = context.Occupations.Where(s => s.ID == Occupation).FirstOrDefault();
                            strHead += "</br> <b>Occupation:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (CandidateState == 0)
                            strHead += "</br> <b>Candidate State :</b>All";

                        else if (CandidateState != 0)
                        {
                            application = application.Where(s => s.CorStateID == CandidateState).ToList();
                            Location ct = context.Locations.Where(s => s.ID == CandidateState && s.LocationTypeID == 2).FirstOrDefault();
                            strHead += "</br> <b>Candidate State:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (ExamCentre1 == 0)
                            strHead += "</br> <b>Exam Centre Choice 1 :</b>All";

                        else if (ExamCentre1 != 0)
                        {
                            application = application.Where(s => s.ExamCenter1ID == ExamCentre1).ToList();
                            ExamCenter ct = context.ExamCenters.Where(s => s.ID == ExamCentre1).FirstOrDefault();
                            strHead += "</br> <b>Exam Centre Choice 1 :</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (ExamCentre2 == 0)
                            strHead += "</br> <b>Exam Centre Choice 2 :</b>All";

                        else if (ExamCentre2 != 0)
                        {
                            application = application.Where(s => s.ExamCenter2ID == ExamCentre2).ToList();
                            ExamCenter ct = context.ExamCenters.Where(s => s.ID == ExamCentre2).FirstOrDefault();
                            strHead += "</br> <b>Exam Centre Choice 2 :</b>" + GetInitCap(ct.Name.ToString());
                        }
                    }
                    if (loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
                    {
                        if (applicantTypeID == 0)
                            strHead += "</br> <b>Applicant Type :</b>Both";
                        else
                            strHead += "</br> <b>Applicant Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicantType)(applicantTypeID)).ToString();
                        if (applicantTypeID == 1)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)).ToList();
                        }
                        else if (applicantTypeID == 2)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                            if (InstituteID != 0)
                            {
                                application = application.Where(s => s.InstituteID == InstituteID).ToList();
                                Institute inst = context.Institutes.Find(InstituteID);
                                strHead += "</br> <b> Institute Name :</b> " + GetInitCap(inst.Name).ToString();
                            }
                        }
                        if (RegionalcentreID == 0)
                        {
                            strHead += "</br> <b>Regional Centre Name :</b>All";
                        }
                        else
                        {
                            if (RegionalcentreID != 0)
                            {
                                application = application.Where(s => s.RegionalCenterID == RegionalcentreID).ToList();
                                RegionalCenter rc = context.RegionalCenters.Find(RegionalcentreID);
                                strHead += "</br> <b> Regional Centre Name :</b> " + GetInitCap(rc.Name).ToString();
                            }
                        }
                        if (Gender == 0)
                            strHead += "</br> <b>Gender :</b>All";

                        else if (Gender == 1)
                        {
                            string gender = "Female";
                            application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Female";
                        }
                        else if (Gender == 2)
                        {
                            string gender = "Male";
                            application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Male";
                        }
                        if (Category == 0)
                            strHead += "</br> <b>Category :</b>All";

                        else if (Category != 0)
                        {
                            application = application.Where(s => s.CastCategoryID == Category).ToList();
                            CastCategory ct = context.CastCategories.Where(s => s.ID == Category).FirstOrDefault();
                            strHead += "</br> <b>Cateogry:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (Occupation == 0)
                            strHead += "</br> <b>Occupation :</b>All";

                        else if (Occupation != 0)
                        {
                            application = application.Where(s => s.OccupationID == Occupation).ToList();
                            Occupation ct = context.Occupations.Where(s => s.ID == Occupation).FirstOrDefault();
                            strHead += "</br> <b>Occupation:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (CandidateState == 0)
                            strHead += "</br> <b>Candidate State :</b>All";

                        else if (CandidateState != 0)
                        {
                            application = application.Where(s => s.CorStateID == CandidateState).ToList();
                            Location ct = context.Locations.Where(s => s.ID == CandidateState && s.LocationTypeID == 2).FirstOrDefault();
                            strHead += "</br> <b>Candidate State:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (ExamCentre1 == 0)
                            strHead += "</br> <b>Exam Centre Choice 1 :</b>All";

                        else if (ExamCentre1 != 0)
                        {
                            application = application.Where(s => s.ExamCenter1ID == ExamCentre1).ToList();
                            ExamCenter ct = context.ExamCenters.Where(s => s.ID == ExamCentre1).FirstOrDefault();
                            strHead += "</br> <b>Exam Centre Choice 1 :</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (ExamCentre2 == 0)
                            strHead += "</br> <b>Exam Centre Choice 2 :</b>All";

                        else if (ExamCentre2 != 0)
                        {
                            application = application.Where(s => s.ExamCenter2ID == ExamCentre2).ToList();
                            ExamCenter ct = context.ExamCenters.Where(s => s.ID == ExamCentre2).FirstOrDefault();
                            strHead += "</br> <b>Exam Centre Choice 2 :</b>" + GetInitCap(ct.Name.ToString());
                        }
                    }
                    if (loginUserType == UserType.Institute)
                    {
                        if (applicantTypeID == 0)
                            strHead += "</br> <b>Applicant Type :</b>Both";
                        else
                            strHead += "</br> <b>Applicant Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicantType)(applicantTypeID)).ToString();
                        if (applicantTypeID == 1)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)).ToList();
                        }
                        else if (applicantTypeID == 2)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                            if (InstituteID != 0)
                            {
                                application = application.Where(s => s.InstituteID == InstituteID).ToList();
                                Institute inst = context.Institutes.Find(InstituteID);
                                strHead += "</br> <b> Institute Name :</b> " + GetInitCap(inst.Name).ToString();
                            }
                        }
                        if (RegionalcentreID == 0)
                        {
                            strHead += "</br> <b>Regional Centre Name :</b>All";
                        }
                        else
                        {
                            if (RegionalcentreID != 0)
                            {
                                application = application.Where(s => s.RegionalCenterID == RegionalcentreID).ToList();
                                RegionalCenter rc = context.RegionalCenters.Find(RegionalcentreID);
                                strHead += "</br> <b> Regional Centre Name :</b> " + GetInitCap(rc.Name).ToString();
                            }
                        }
                        if (Gender == 0)
                            strHead += "</br> <b>Gender :</b>All";

                        else if (Gender == 1)
                        {
                            string gender = "Female";
                            application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Female";
                        }
                        else if (Gender == 2)
                        {
                            string gender = "Male";
                            application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Male";
                        }
                        if (Category == 0)
                            strHead += "</br> <b>Category :</b>All";

                        else if (Category != 0)
                        {
                            application = application.Where(s => s.CastCategoryID == Category).ToList();
                            CastCategory ct = context.CastCategories.Where(s => s.ID == Category).FirstOrDefault();
                            strHead += "</br> <b>Cateogry:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (Occupation == 0)
                            strHead += "</br> <b>Occupation :</b>All";

                        else if (Occupation != 0)
                        {
                            application = application.Where(s => s.OccupationID == Occupation).ToList();
                            Occupation ct = context.Occupations.Where(s => s.ID == Occupation).FirstOrDefault();
                            strHead += "</br> <b>Occupation:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (CandidateState == 0)
                            strHead += "</br> <b>Candidate State :</b>All";

                        else if (CandidateState != 0)
                        {
                            application = application.Where(s => s.CorStateID == CandidateState).ToList();
                            Location ct = context.Locations.Where(s => s.ID == CandidateState && s.LocationTypeID == 2).FirstOrDefault();
                            strHead += "</br> <b>Candidate State:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (ExamCentre1 == 0)
                            strHead += "</br> <b>Exam Centre Choice 1 :</b>All";

                        else if (ExamCentre1 != 0)
                        {
                            application = application.Where(s => s.ExamCenter1ID == ExamCentre1).ToList();
                            ExamCenter ct = context.ExamCenters.Where(s => s.ID == ExamCentre1).FirstOrDefault();
                            strHead += "</br> <b>Exam Centre Choice 1 :</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (ExamCentre2 == 0)
                            strHead += "</br> <b>Exam Centre Choice 2 :</b>All";

                        else if (ExamCentre2 != 0)
                        {
                            application = application.Where(s => s.ExamCenter2ID == ExamCentre2).ToList();
                            ExamCenter ct = context.ExamCenters.Where(s => s.ID == ExamCentre2).FirstOrDefault();
                            strHead += "</br> <b>Exam Centre Choice 2 :</b>" + GetInitCap(ct.Name.ToString());
                        }
                    }
                    if (application.Count() > 0)
                    {
                        ShowTableHeader();
                        for (int i = 0; i < application.Count(); i++)
                        {
                            TableRow tr = new TableRow();
                            if (i % 2 == 0)
                                tr.CssClass = "gdalternate1";
                            else
                                tr.CssClass = "gdrow1";

                            TableCell tdRow = new TableCell();
                            tdRow.Width = Unit.Percentage(1);
                            tdRow.Text = (i + 1).ToString();
                            tdRow.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow);

                            TableCell tdRow1 = new TableCell();
                            tdRow1.Width = Unit.Percentage(1);
                            tdRow1.Text = application[i].Number.ToString();
                            tdRow1.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow1);

                            TableCell tdRow2 = new TableCell();
                            tdRow2.Width = Unit.Percentage(10);
                            if (application[i].ApplicationDate != null)
                                tdRow2.Text = application[i].ApplicationDate.ToString("dd-MMM-yyyy");
                            tdRow2.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow2);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(14);
                            if (application[i].Name != null)
                                tdRow3.Text = GetInitCap(application[i].Name.ToString());
                            tdRow3.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow3);
                            if (string.IsNullOrEmpty(application[i].GuardianName) == true && string.IsNullOrWhiteSpace(application[i].GuardianName) == true)
                            {
                                TableCell tdRow4 = new TableCell();
                                tdRow4.Width = Unit.Percentage(12);
                                if (application[i].FatherName != null)
                                    tdRow4.Text = GetInitCap(application[i].FatherName.ToString());
                                tdRow4.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow4);
                            }
                            else
                            {
                                TableCell tdRow4 = new TableCell();
                                tdRow4.Width = Unit.Percentage(12);
                                if (application[i].GuardianName != null)
                                    tdRow4.Text = GetInitCap(application[i].GuardianName.ToString());
                                tdRow4.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow4);
                            }

                            TableCell tdRow5 = new TableCell();
                            tdRow5.Width = Unit.Percentage(5);
                            if (application[i].MobileNumber != 0 && application[i].MobileNumber != 0)
                                tdRow5.Text = application[i].MobileNumber.ToString();
                            tdRow5.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow5);

                            TableCell tdrow30 = new TableCell();
                            tdrow30.Width = Unit.Percentage(7);
                            if (!string.IsNullOrEmpty(application[i].EmailAddress))
                                tdrow30.Text = application[i].EmailAddress.ToString().ToLower();
                            tdrow30.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdrow30);

                            TableCell tdRow6 = new TableCell();
                            tdRow6.Width = Unit.Percentage(10);
                            if (application[i].DateOfBirth != null)
                                tdRow6.Text = application[i].DateOfBirth.ToString("dd-MMM-yyyy");
                            tdRow6.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow6);

                            if (TypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                            {
                                TableCell tdRow15 = new TableCell();
                                tdRow15.Width = Unit.Percentage(10);
                                if (application[i].ExamCenter1ID != 0 && application[i].ExamCenter1ID != 0)
                                    tdRow15.Text = application[i].ExamCenter1Name;
                                else
                                    tdRow15.Text = "NA";
                                tdRow15.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(tdRow15);

                                TableCell tdRow16 = new TableCell();
                                tdRow16.Width = Unit.Percentage(10);
                                if (application[i].ExamCenter2ID != 0 && application[i].ExamCenter2ID != 0)
                                    tdRow16.Text = application[i].ExamCenter2Name;
                                else
                                    tdRow16.Text = "NA";
                                tdRow16.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(tdRow16);
                            }

                            if (StatusId == 0)
                            {
                                TableCell tdRow7 = new TableCell();
                                tdRow7.Width = Unit.Percentage(16);
                                if (application[i].ApplicationStatusID != 0)
                                {
                                    if ((application[i].ApplicationStatusID == Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre)) && (application[i].PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending)))
                                    {
                                        tdRow7.Text = "Fee Pending to be Paid by Institute";
                                    }
                                    else
                                    {
                                        tdRow7.Text = EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmCertificateExamApplicationStatus)(application[i].ApplicationStatusID)).ToString();
                                    }
                                }
                                tdRow7.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow7);
                            }
                            TableCell tdRow8 = new TableCell();
                            tdRow8.Width = Unit.Percentage(14);
                            tdRow8.Text = EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.enmPaymentStatus)(application[i].PaymentStatusID)).ToString();
                            tr.Cells.Add(tdRow8);

                            if (loginUserType == UserType.RegionalCenter || loginUserType == UserType.HeadOffice || loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
                            {
                                TableCell tdRow7 = new TableCell();
                                tdRow7.Width = Unit.Percentage(15);
                                tdRow7.Text = "Direct";
                                tdRow7.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow7);
                                if (application[i].ApplicantTypeID == 2)
                                {
                                    if (application[i].InstituteID != null)
                                        tdRow7.Text = application[i].InstituteName;
                                }
                            }

                            TableCell tdRow17 = new TableCell();
                            tdRow17.Width = Unit.Percentage(10);
                            if (application[i].TotalFeeAmount.HasValue)
                                tdRow17.Text = application[i].TotalFeeAmount.Value.ToString("F");
                            else
                                tdRow17.Text = "NA";
                            tr.Cells.Add(tdRow17);

                            tbl.Rows.Add(tr);
                        }
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }
                }
                #endregion---------------

                #region------Course Registration Application------------

                else if (TypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    if (StatusId == 0)
                        strHead += "</br> <b>Application Status :</b> All ";
                    else
                    {
                        if (StatusId == 100)
                        {
                            strHead += "</br> <b>Application Status :</b> " + "Fee Pending to be Paid By Institute";
                        }
                        else
                        {
                            strHead += "</br> <b>Application Status :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmCourseApplicationStatus)(StatusId)).ToString();
                        }
                    }
                    var application = (from a in context.CourseRegistrationApplications.AsNoTracking()
                                       where a.ApplicableExamID == ExamId && a.CourseID == CourseId && a.FinalSubmitted == true
                                       select new
                                       {
                                           ID = a.ID,
                                           ApplicationDate = a.ApplicationDate,
                                           Number = a.Number,
                                           CourseCategoryID = a.CourseCategoryID,
                                           CourseID = a.CourseID,
                                           ExamID = a.ApplicableExamID,
                                           ApplicantTypeID = a.ApplicantTypeID,
                                           InstituteID = a.InstituteID,
                                           Name = a.Name,
                                           FatherName = a.FatherName,
                                           GuardianName = a.GuardianName,
                                           Gender = a.Gender,
                                           DateOfBirth = a.DateOfBirth,
                                           CastCategoryID = a.CastCategoryID,
                                           MobileNumber = a.MobileNumber,
                                           EmailAddress = a.EmailAddress,
                                           FinalSubmitted = a.FinalSubmitted,
                                           PaymentStatusID = a.PaymentStatusID,
                                           ApplicationStatusID = a.ApplicationStatusID,
                                           CorStateID = a.CorStateID,
                                           FeeAmount = a.FeeAmount,
                                           CorStateName = a.CorState.Name,
                                           InstituteName = a.Institute.Name,
                                           CastCategoryName = a.CastCategory.Name

                                       }).ToList();
                    if (StatusId != 0)
                    {
                        if (StatusId == Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT))
                        {
                            application = application.Where(s => paymentstatus.Contains(s.PaymentStatusID.Value) && s.ApplicationStatusID == StatusId).ToList();
                        }
                        else if (StatusId == 100)
                        {
                            application = application.Where(s => s.PaymentStatusID == paymentpending && s.ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT)).ToList();
                        }
                        else
                        {
                            application = application.Where(s => s.ApplicationStatusID == StatusId).ToList();
                        }
                    }
                    if (loginUserType == UserType.Institute)
                        application = application.Where(s => s.InstituteID == entityID).ToList();
                    if (loginUserType == UserType.HeadOffice)
                    {
                        if (applicantTypeID == 0)
                            strHead += "</br> <b>Applicant Type :</b>Both";
                        else
                            strHead += "</br> <b>Applicant Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicantType)(applicantTypeID)).ToString();
                        if (applicantTypeID == 1)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)).ToList();
                        }
                        else if (applicantTypeID == 2)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                            if (InstituteID != 0)
                            {
                                application = application.Where(s => s.InstituteID == InstituteID).ToList();
                                Institute inst = context.Institutes.Find(InstituteID);
                                strHead += "</br> <b> Institute Name :</b> " + GetInitCap(inst.Name).ToString();
                            }
                        }
                        if (Gender == 0)
                            strHead += "</br> <b>Gender :</b>All";

                        else if (Gender == 1)
                        {
                            string gender = "Female";
                            application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Female";
                        }
                        else if (Gender == 2)
                        {
                            string gender = "Male";
                            application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Male";
                        }
                        if (Category == 0)
                            strHead += "</br> <b>Category :</b>All";

                        else if (Category != 0)
                        {
                            application = application.Where(s => s.CastCategoryID == Category).ToList();
                            CastCategory ct = context.CastCategories.Where(s => s.ID == Category).FirstOrDefault();
                            strHead += "</br> <b>Cateogry:</b>" + GetInitCap(ct.Name.ToString());
                        }

                        if (CandidateState == 0)
                            strHead += "</br> <b>Candidate State :</b>All";

                        else if (CandidateState != 0)
                        {
                            application = application.Where(s => s.CorStateID == CandidateState).ToList();
                            Location ct = context.Locations.Where(s => s.ID == CandidateState && s.LocationTypeID == 2).FirstOrDefault();
                            strHead += "</br> <b>Candidate State:</b>" + GetInitCap(ct.Name.ToString());
                        }

                    }
                    if (loginUserType == UserType.RegionalCenter)
                    {
                        if (applicantTypeID == 0)
                            strHead += "</br> <b>Applicant Type :</b>Both";
                        else
                            strHead += "</br> <b>Applicant Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicantType)(applicantTypeID)).ToString();
                        if (applicantTypeID == 1)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)).ToList();
                        }
                        else if (applicantTypeID == 2)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                            if (InstituteID != 0)
                            {
                                application = application.Where(s => s.InstituteID == InstituteID).ToList();
                                Institute inst = context.Institutes.Find(InstituteID);
                                strHead += "</br> <b> Institute Name :</b> " + GetInitCap(inst.Name).ToString();
                            }
                        }
                        if (Gender == 0)
                            strHead += "</br> <b>Gender :</b>All";

                        else if (Gender == 1)
                        {
                            string gender = "Female";
                            application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Female";
                        }
                        else if (Gender == 2)
                        {
                            string gender = "Male";
                            application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Male";
                        }
                        if (Category == 0)
                            strHead += "</br> <b>Category :</b>All";

                        else if (Category != 0)
                        {
                            application = application.Where(s => s.CastCategoryID == Category).ToList();
                            CastCategory ct = context.CastCategories.Where(s => s.ID == Category).FirstOrDefault();
                            strHead += "</br> <b>Cateogry:</b>" + GetInitCap(ct.Name.ToString());
                        }

                        if (CandidateState == 0)
                            strHead += "</br> <b>Candidate State :</b>All";

                        else if (CandidateState != 0)
                        {
                            application = application.Where(s => s.CorStateID == CandidateState).ToList();
                            Location ct = context.Locations.Where(s => s.ID == CandidateState && s.LocationTypeID == 2).FirstOrDefault();
                            strHead += "</br> <b>Candidate State:</b>" + GetInitCap(ct.Name.ToString());
                        }

                    }
                    if (loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
                    {
                        if (applicantTypeID == 0)
                            strHead += "</br> <b>Applicant Type :</b>Both";
                        else
                            strHead += "</br> <b>Applicant Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicantType)(applicantTypeID)).ToString();
                        if (applicantTypeID == 1)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)).ToList();
                        }
                        else if (applicantTypeID == 2)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                            if (InstituteID != 0)
                            {
                                application = application.Where(s => s.InstituteID == InstituteID).ToList();
                                Institute inst = context.Institutes.Find(InstituteID);
                                strHead += "</br> <b> Institute Name :</b> " + GetInitCap(inst.Name).ToString();
                            }
                        }
                        if (Gender == 0)
                            strHead += "</br> <b>Gender :</b>All";

                        else if (Gender == 1)
                        {
                            string gender = "Female";
                            application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Female";
                        }
                        else if (Gender == 2)
                        {
                            string gender = "Male";
                            application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Male";
                        }
                        if (Category == 0)
                            strHead += "</br> <b>Category :</b>All";

                        else if (Category != 0)
                        {
                            application = application.Where(s => s.CastCategoryID == Category).ToList();
                            CastCategory ct = context.CastCategories.Where(s => s.ID == Category).FirstOrDefault();
                            strHead += "</br> <b>Cateogry:</b>" + GetInitCap(ct.Name.ToString());
                        }

                        if (CandidateState == 0)
                            strHead += "</br> <b>Candidate State :</b>All";

                        else if (CandidateState != 0)
                        {
                            application = application.Where(s => s.CorStateID == CandidateState).ToList();
                            Location ct = context.Locations.Where(s => s.ID == CandidateState && s.LocationTypeID == 2).FirstOrDefault();
                            strHead += "</br> <b>Candidate State:</b>" + GetInitCap(ct.Name.ToString());
                        }

                    }
                    if (loginUserType == UserType.Institute)
                    {
                        if (applicantTypeID == 0)
                            strHead += "</br> <b>Applicant Type :</b>Both";
                        else
                            strHead += "</br> <b>Applicant Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicantType)(applicantTypeID)).ToString();
                        if (applicantTypeID == 1)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)).ToList();
                        }
                        else if (applicantTypeID == 2)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                            if (InstituteID != 0)
                            {
                                application = application.Where(s => s.InstituteID == InstituteID).ToList();
                                Institute inst = context.Institutes.Find(InstituteID);
                                strHead += "</br> <b> Institute Name :</b> " + GetInitCap(inst.Name).ToString();
                            }
                        }
                        if (Gender == 0)
                            strHead += "</br> <b>Gender :</b>All";

                        else if (Gender == 1)
                        {
                            string gender = "Female";
                            application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Female";
                        }
                        else if (Gender == 2)
                        {
                            string gender = "Male";
                            application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Male";
                        }
                        if (Category == 0)
                            strHead += "</br> <b>Category :</b>All";

                        else if (Category != 0)
                        {
                            application = application.Where(s => s.CastCategoryID == Category).ToList();
                            CastCategory ct = context.CastCategories.Where(s => s.ID == Category).FirstOrDefault();
                            strHead += "</br> <b>Cateogry:</b>" + GetInitCap(ct.Name.ToString());
                        }

                        if (CandidateState == 0)
                            strHead += "</br> <b>Candidate State :</b>All";

                        else if (CandidateState != 0)
                        {
                            application = application.Where(s => s.CorStateID == CandidateState).ToList();
                            Location ct = context.Locations.Where(s => s.ID == CandidateState && s.LocationTypeID == 2).FirstOrDefault();
                            strHead += "</br> <b>Candidate State:</b>" + GetInitCap(ct.Name.ToString());
                        }

                    }
                    if (application.Count() > 0)
                    {
                        ShowTableHeader();
                        for (int i = 0; i < application.Count(); i++)
                        {
                            TableRow tr = new TableRow();
                            if (i % 2 == 0)
                                tr.CssClass = "gdalternate1";
                            else
                                tr.CssClass = "gdrow1";

                            TableCell tdRow = new TableCell();
                            tdRow.Width = Unit.Percentage(1);
                            tdRow.Text = (i + 1).ToString();
                            tdRow.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow);

                            TableCell tdRow1 = new TableCell();
                            tdRow1.Width = Unit.Percentage(1);
                            tdRow1.Text = application[i].Number.ToString();
                            tdRow1.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow1);

                            TableCell tdRow2 = new TableCell();
                            tdRow2.Width = Unit.Percentage(13);
                            if (application[i].ApplicationDate != null)
                                tdRow2.Text = application[i].ApplicationDate.ToString("dd-MMM-yyyy");
                            tdRow2.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow2);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(14);
                            if (application[i].Name != null)
                                tdRow3.Text = GetInitCap(application[i].Name.Trim().ToString());
                            tdRow3.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow3);
                            if (string.IsNullOrEmpty(application[i].GuardianName) == true && string.IsNullOrWhiteSpace(application[i].GuardianName) == true)
                            {
                                TableCell tdRow4 = new TableCell();
                                tdRow4.Width = Unit.Percentage(12);
                                if (application[i].FatherName != null)
                                    tdRow4.Text = GetInitCap(application[i].FatherName.ToString());
                                tdRow4.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow4);
                            }
                            else
                            {
                                TableCell tdRow4 = new TableCell();
                                tdRow4.Width = Unit.Percentage(12);
                                if (application[i].GuardianName != null)
                                    tdRow4.Text = GetInitCap(application[i].GuardianName.ToString());
                                tdRow4.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow4);
                            }
                            TableCell tdRow5 = new TableCell();
                            tdRow5.Width = Unit.Percentage(5);
                            if (application[i].MobileNumber != 0)
                                tdRow5.Text = application[i].MobileNumber.ToString();
                            tdRow5.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow5);

                            TableCell tdrow30 = new TableCell();
                            tdrow30.Width = Unit.Percentage(7);
                            if (!string.IsNullOrEmpty(application[i].EmailAddress))
                                tdrow30.Text = application[i].EmailAddress.ToString().ToLower();
                            tdrow30.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdrow30);

                            TableCell tdRow6 = new TableCell();
                            tdRow6.Width = Unit.Percentage(13);
                            if (application[i].DateOfBirth != null)
                                tdRow6.Text = application[i].DateOfBirth.ToString("dd-MMM-yyyy");
                            tdRow6.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow6);

                            if (TypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication) || TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                            {
                                TableCell tdRow13 = new TableCell();
                                tdRow13.Width = Unit.Percentage(10);
                                tdRow13.Text = "";
                                tdRow13.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(tdRow13);
                            }

                            if (TypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication) || TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                            {
                                TableCell tdRow15 = new TableCell();
                                tdRow15.Width = Unit.Percentage(10);
                                tdRow15.Text = "";
                                tdRow15.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(tdRow15);

                                TableCell tdRow16 = new TableCell();
                                tdRow16.Width = Unit.Percentage(10);
                                tdRow16.Text = "";
                                tdRow16.HorizontalAlign = HorizontalAlign.Center;
                                tr.Cells.Add(tdRow16);
                            }
                            if (StatusId == 0)
                            {
                                TableCell tdRow7 = new TableCell();
                                tdRow7.Width = Unit.Percentage(16);
                                if (application[i].ApplicationStatusID != 0)
                                {
                                    if ((application[i].ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT)) && (application[i].PaymentStatusID.Value == Convert.ToInt32(enmPaymentStatus.Pending)))
                                    {
                                        tdRow7.Text = "Fee Pending to be Paid by Institute";
                                    }
                                    else
                                    {
                                        tdRow7.Text = EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmCourseApplicationStatus)(application[i].ApplicationStatusID)).ToString();
                                    }
                                }
                                tdRow7.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow7);
                            }

                            TableCell tdRow8 = new TableCell();
                            tdRow8.Width = Unit.Percentage(14);
                            tdRow8.HorizontalAlign = HorizontalAlign.Left;
                            tdRow8.Text = EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.enmPaymentStatus)(application[i].PaymentStatusID)).ToString();
                            tr.Cells.Add(tdRow8);

                            if (loginUserType == UserType.RegionalCenter || loginUserType == UserType.HeadOffice || loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
                            {
                                TableCell tdRow7 = new TableCell();
                                tdRow7.Width = Unit.Percentage(15);
                                tdRow7.Text = "Direct";
                                tdRow7.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow7);
                                if (application[i].ApplicantTypeID == 2)
                                {
                                    if (application[i].InstituteID != null)
                                        tdRow7.Text = application[i].InstituteName;
                                }
                            }

                            TableCell tdRow17 = new TableCell();
                            tdRow17.Width = Unit.Percentage(10);
                            if (application[i].FeeAmount.HasValue)
                                tdRow17.Text = application[i].FeeAmount.Value.ToString("F");
                            else
                                tdRow17.Text = "NA";
                            tr.Cells.Add(tdRow17);
                            tbl.Rows.Add(tr);
                        }
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }

                }
                #endregion--------------

                #region-----Coursee exam-----------
                else if (TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    if (StatusId == 0)
                        strHead += "</br> <b>Application Status :</b> All ";
                    else
                    {
                        if (paymentStatusId != 0)
                        {
                            if ((StatusId == Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT)) && (paymentStatusId == Convert.ToInt32(enmPaymentStatus.Pending)))
                            {
                                strHead += "</br> <b>Application Status :</b> " + "Fee Pending to be Paid By Institute";
                            }
                            else
                            {
                                strHead += "</br> <b>Application Status :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmCourseExamApplicationStatus)(StatusId)).ToString();
                            }
                        }
                        else
                        {
                            if (StatusId == 100)
                            {
                                strHead += "</br> <b>Application Status :</b> " + "Fee Pending to be Paid By Institute";
                            }
                            else
                            {
                                strHead += "</br> <b>Application Status :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmCourseExamApplicationStatus)(StatusId)).ToString();
                            }
                        }
                    }
                                 
                        var application = (from a in context.CourseExamApplications.Include("Candidates").AsNoTracking()
                                           where a.ExamID == ExamId && a.CourseID == CourseId && a.FinalSubmitted == true 
										   //&&  (a.CourseCategoryID == 6 ? a.ApplicationStatusID == 8 :   a.ApplicationStatusID == 10 )
                                           select new
                                           {
                                               ID = a.ID,
                                               Number = a.Number,
                                               ApplicationDate = a.ApplicationDate,
                                               RegistrationNumber = a.RegistrationNumber,
                                               CandidateName = a.Candidate.Name,
                                               CourseID = a.CourseID,
                                               CandidateDateOfBirth = a.Candidate.DateOfBirth,
                                               PaymentStatusID = a.PaymentStatusID,
                                               FeeAmount = a.FeeAmount,
                                               ApplicationStatusID = a.ApplicationStatusID,
                                               InstituteID = a.InstituteID,
                                               InstituteName = a.Institute.Name,
                                               ApplicantTypeID = a.ApplicantTypeID,
                                               CandidateGender = a.Candidate.Gender,
                                               CandidateCastCategoryID = a.Candidate.CastCategoryID,
                                               CandidateAddresses = a.Candidate.Addresses.Where(s => s.AddressTypeID == 1).OrderByDescending(t => t.EffectiveDateFrom).FirstOrDefault(),
                                               CandidateOccupationID = a.Candidate.OccupationID,
                                               ExamCenter1ID = a.ExamCenter1ID,
                                               ExamCenter2ID = a.ExamCenter2ID,
                                               DemandNoteID = a.DemandNoteID,
                                               CandidateFatherName = a.Candidate.FatherName,
                                               CandidateGuardianName = a.Candidate.GuardianName,
                                               MobileNumber = a.Candidate.ContactDetails.FirstOrDefault().MobileNumber,
                                               EmailAddress = a.Candidate.ContactDetails.FirstOrDefault().EmailAddress,
                                               ExamCenter1Name = a.ExamCenter1.Name + "(" + a.ExamCenter1.State.Name + ")",
                                               ExamCenter2Name = a.ExamCenter2.Name + "(" + a.ExamCenter2.State.Name + ")",
                                               FormForwardedByInstituteOn = a.FormForwardedByInstituteOn
                                           }).ToList();

                        if (StatusId != 0)
                        {
                            if (paymentStatusId != 0)
                            {
                                if ((StatusId == Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT)) && (paymentStatusId == Convert.ToInt32(enmPaymentStatus.Pending)))
                                {
                                    application = application.Where(s => s.PaymentStatusID == paymentStatusId && s.ApplicationStatusID == StatusId).ToList();
                                }
                                else if ((StatusId == Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT)) && (paymentStatusId == Convert.ToInt32(enmPaymentStatus.Paid) || paymentStatusId == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified)))
                                {
                                    application = application.Where(s => paymentstatus.Contains(s.PaymentStatusID) && s.ApplicationStatusID == StatusId).ToList();
                                }
                                else
                                {
                                    application = application.Where(s => s.ApplicationStatusID == StatusId).ToList();
                                }
                            }
                            else
                            {
                                if (StatusId == Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT))
                                {
                                    application = application.Where(s => paymentstatus.Contains(s.PaymentStatusID) && s.ApplicationStatusID == StatusId).ToList();
                                }
                                else if (StatusId == 100)
                                {
                                    application = application.Where(s => s.PaymentStatusID == paymentpending && s.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT)).ToList();
                                }
                                else
                                {
                                    application = application.Where(s => s.ApplicationStatusID == StatusId).ToList();
                                }
                            }
                        }

                        if (loginUserType == UserType.Institute)
                            application = application.Where(s => (s.InstituteID == entityID && s.ApplicantTypeID == 2)).ToList();
                        if (loginUserType == UserType.HeadOffice)
                        {
                            if (applicantTypeID == 0)
                                strHead += "</br> <b>Applicant Type :</b>Both";
                            else
                                strHead += "</br> <b>Applicant Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicantType)(applicantTypeID)).ToString();
                            if (applicantTypeID == 1)
                            {
                                application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)).ToList();
                            }
                            else if (applicantTypeID == 2)
                            {
                                application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                                if (InstituteID != 0)
                                {
                                    application = application.Where(s => s.InstituteID == InstituteID).ToList();
                                    Institute inst = context.Institutes.Find(InstituteID);
                                    strHead += "</br> <b> Institute Name :</b> " + GetInitCap(inst.Name).ToString();
                                }
                            }
                            if (Gender == 0)
                                strHead += "</br> <b>Gender :</b>All";

                            else if (Gender == 1)
                            {
                                string gender = "Female";
                                application = application.Where(s => s.CandidateGender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                                strHead += "</br> <b>Gender :</b>Female";
                            }
                            else if (Gender == 2)
                            {
                                string gender = "Male";
                                application = application.Where(s => s.CandidateGender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                                strHead += "</br> <b>Gender :</b>Male";
                            }
                            if (Category == 0)
                                strHead += "</br> <b>Category :</b>All";

                            else if (Category != 0)
                            {
                                application = application.Where(s => s.CandidateCastCategoryID == Category).ToList();
                                CastCategory ct = context.CastCategories.Where(s => s.ID == Category).FirstOrDefault();
                                strHead += "</br> <b>Cateogry:</b>" + GetInitCap(ct.Name.ToString());
                            }
                            if (Occupation == 0)
                                strHead += "</br> <b>Occupation :</b>All";

                            else if (Occupation != 0)
                            {
                                application = application.Where(s => s.CandidateOccupationID == Occupation).ToList();
                                Occupation ct = context.Occupations.Where(s => s.ID == Occupation).FirstOrDefault();
                                strHead += "</br> <b>Occupation:</b>" + GetInitCap(ct.Name.ToString());
                            }
                            if (CandidateState == 0)
                                strHead += "</br> <b>Candidate State :</b>All";

                            else if (CandidateState != 0)
                            {
                                application = application.Where(s => s.CandidateAddresses.StateID == CandidateState).ToList();
                                Location ct = context.Locations.Where(s => s.ID == CandidateState && s.LocationTypeID == 2).FirstOrDefault();
                                strHead += "</br> <b>Candidate State:</b>" + GetInitCap(ct.Name.ToString());
                            }
                            if (ExamCentre1 == 0)
                                strHead += "</br> <b>Exam Centre Choice 1 :</b>All";

                            else if (ExamCentre1 != 0)
                            {
                                application = application.Where(s => s.ExamCenter1ID == ExamCentre1).ToList();
                                ExamCenter ct = context.ExamCenters.Where(s => s.ID == ExamCentre1).FirstOrDefault();
                                strHead += "</br> <b>Exam Centre Choice 1 :</b>" + GetInitCap(ct.Name.ToString());
                            }
                            if (ExamCentre2 == 0)
                                strHead += "</br> <b>Exam Centre Choice 2 :</b>All";

                            else if (ExamCentre2 != 0)
                            {
                                application = application.Where(s => s.ExamCenter2ID == ExamCentre2).ToList();
                                ExamCenter ct = context.ExamCenters.Where(s => s.ID == ExamCentre2).FirstOrDefault();
                                strHead += "</br> <b>Exam Centre Choice 2 :</b>" + GetInitCap(ct.Name.ToString());
                            }
                        }
                    
                    if (loginUserType == UserType.RegionalCenter)
                    {
                        if (applicantTypeID == 0)
                            strHead += "</br> <b>Applicant Type :</b>Both";
                        else
                            strHead += "</br> <b>Applicant Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicantType)(applicantTypeID)).ToString();
                        if (applicantTypeID == 1)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)).ToList();
                        }
                        else if (applicantTypeID == 2)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                            if (InstituteID != 0)
                            {
                                application = application.Where(s => s.InstituteID == InstituteID).ToList();
                                Institute inst = context.Institutes.Find(InstituteID);
                                strHead += "</br> <b> Institute Name :</b> " + GetInitCap(inst.Name).ToString();
                            }
                        }
                        if (Gender == 0)
                            strHead += "</br> <b>Gender :</b>All";

                        else if (Gender == 1)
                        {
                            string gender = "Female";
                            application = application.Where(s => s.CandidateGender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Female";
                        }
                        else if (Gender == 2)
                        {
                            string gender = "Male";
                            application = application.Where(s => s.CandidateGender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Male";
                        }
                        if (Category == 0)
                            strHead += "</br> <b>Category :</b>All";

                        else if (Category != 0)
                        {
                            application = application.Where(s => s.CandidateCastCategoryID == Category).ToList();
                            CastCategory ct = context.CastCategories.Where(s => s.ID == Category).FirstOrDefault();
                            strHead += "</br> <b>Cateogry:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (Occupation == 0)
                            strHead += "</br> <b>Occupation :</b>All";

                        else if (Occupation != 0)
                        {
                            application = application.Where(s => s.CandidateOccupationID == Occupation).ToList();
                            Occupation ct = context.Occupations.Where(s => s.ID == Occupation).FirstOrDefault();
                            strHead += "</br> <b>Occupation:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (CandidateState == 0)
                            strHead += "</br> <b>Candidate State :</b>All";

                        else if (CandidateState != 0)
                        {
                            application = application.Where(s => s.CandidateAddresses.StateID == CandidateState).ToList();
                            Location ct = context.Locations.Where(s => s.ID == CandidateState && s.LocationTypeID == 2).FirstOrDefault();
                            strHead += "</br> <b>Candidate State:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (ExamCentre1 == 0)
                            strHead += "</br> <b>Exam Centre Choice 1 :</b>All";

                        else if (ExamCentre1 != 0)
                        {
                            application = application.Where(s => s.ExamCenter1ID == ExamCentre1).ToList();
                            ExamCenter ct = context.ExamCenters.Where(s => s.ID == ExamCentre1).FirstOrDefault();
                            strHead += "</br> <b>Exam Centre Choice 1 :</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (ExamCentre2 == 0)
                            strHead += "</br> <b>Exam Centre Choice 2 :</b>All";

                        else if (ExamCentre2 != 0)
                        {
                            application = application.Where(s => s.ExamCenter2ID == ExamCentre2).ToList();
                            ExamCenter ct = context.ExamCenters.Where(s => s.ID == ExamCentre2).FirstOrDefault();
                            strHead += "</br> <b>Exam Centre Choice 2 :</b>" + GetInitCap(ct.Name.ToString());
                        }
                    }
                    if (loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
                    {
                        if (applicantTypeID == 0)
                            strHead += "</br> <b>Applicant Type :</b>Both";
                        else
                            strHead += "</br> <b>Applicant Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicantType)(applicantTypeID)).ToString();
                        if (applicantTypeID == 1)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)).ToList();
                        }
                        else if (applicantTypeID == 2)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                            if (InstituteID != 0)
                            {
                                application = application.Where(s => s.InstituteID == InstituteID).ToList();
                                Institute inst = context.Institutes.Find(InstituteID);
                                strHead += "</br> <b> Institute Name :</b> " + GetInitCap(inst.Name).ToString();
                            }
                        }
                        if (Gender == 0)
                            strHead += "</br> <b>Gender :</b>All";

                        else if (Gender == 1)
                        {
                            string gender = "Female";
                            application = application.Where(s => s.CandidateGender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Female";
                        }
                        else if (Gender == 2)
                        {
                            string gender = "Male";
                            application = application.Where(s => s.CandidateGender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Male";
                        }
                        if (Category == 0)
                            strHead += "</br> <b>Category :</b>All";

                        else if (Category != 0)
                        {
                            application = application.Where(s => s.CandidateCastCategoryID == Category).ToList();
                            CastCategory ct = context.CastCategories.Where(s => s.ID == Category).FirstOrDefault();
                            strHead += "</br> <b>Cateogry:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (Occupation == 0)
                            strHead += "</br> <b>Occupation :</b>All";

                        else if (Occupation != 0)
                        {
                            application = application.Where(s => s.CandidateOccupationID == Occupation).ToList();
                            Occupation ct = context.Occupations.Where(s => s.ID == Occupation).FirstOrDefault();
                            strHead += "</br> <b>Occupation:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (CandidateState == 0)
                            strHead += "</br> <b>Candidate State :</b>All";

                        else if (CandidateState != 0)
                        {
                            application = application.Where(s => s.CandidateAddresses.StateID == CandidateState).ToList();
                            Location ct = context.Locations.Where(s => s.ID == CandidateState && s.LocationTypeID == 2).FirstOrDefault();
                            strHead += "</br> <b>Candidate State:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (ExamCentre1 == 0)
                            strHead += "</br> <b>Exam Centre Choice 1 :</b>All";

                        else if (ExamCentre1 != 0)
                        {
                            application = application.Where(s => s.ExamCenter1ID == ExamCentre1).ToList();
                            ExamCenter ct = context.ExamCenters.Where(s => s.ID == ExamCentre1).FirstOrDefault();
                            strHead += "</br> <b>Exam Centre Choice 1 :</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (ExamCentre2 == 0)
                            strHead += "</br> <b>Exam Centre Choice 2 :</b>All";

                        else if (ExamCentre2 != 0)
                        {
                            application = application.Where(s => s.ExamCenter2ID == ExamCentre2).ToList();
                            ExamCenter ct = context.ExamCenters.Where(s => s.ID == ExamCentre2).FirstOrDefault();
                            strHead += "</br> <b>Exam Centre Choice 2 :</b>" + GetInitCap(ct.Name.ToString());
                        }
                    }
                    if (loginUserType == UserType.Institute)
                    {
                        if (applicantTypeID == 0)
                            strHead += "</br> <b>Applicant Type :</b>Both";
                        else
                            strHead += "</br> <b>Applicant Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicantType)(applicantTypeID)).ToString();
                        if (applicantTypeID == 1)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)).ToList();
                        }
                        else if (applicantTypeID == 2)
                        {
                            application = application.Where(s => s.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute)).ToList();
                            if (InstituteID != 0)
                            {
                                application = application.Where(s => s.InstituteID == InstituteID).ToList();
                                Institute inst = context.Institutes.Find(InstituteID);
                                strHead += "</br> <b> Institute Name :</b> " + GetInitCap(inst.Name).ToString();
                            }
                        }
                        if (Gender == 0)
                            strHead += "</br> <b>Gender :</b>All";

                        else if (Gender == 1)
                        {
                            string gender = "Female";
                            application = application.Where(s => s.CandidateGender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Female";
                        }
                        else if (Gender == 2)
                        {
                            string gender = "Male";
                            application = application.Where(s => s.CandidateGender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                            strHead += "</br> <b>Gender :</b>Male";
                        }
                        if (Category == 0)
                            strHead += "</br> <b>Category :</b>All";

                        else if (Category != 0)
                        {
                            application = application.Where(s => s.CandidateCastCategoryID == Category).ToList();
                            CastCategory ct = context.CastCategories.Where(s => s.ID == Category).FirstOrDefault();
                            strHead += "</br> <b>Cateogry:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (Occupation == 0)
                            strHead += "</br> <b>Occupation :</b>All";

                        else if (Occupation != 0)
                        {
                            application = application.Where(s => s.CandidateOccupationID == Occupation).ToList();
                            Occupation ct = context.Occupations.Where(s => s.ID == Occupation).FirstOrDefault();
                            strHead += "</br> <b>Occupation:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (CandidateState == 0)
                            strHead += "</br> <b>Candidate State :</b>All";

                        else if (CandidateState != 0)
                        {
                            application = application.Where(s => s.CandidateAddresses.StateID == CandidateState).ToList();
                            Location ct = context.Locations.Where(s => s.ID == CandidateState && s.LocationTypeID == 2).FirstOrDefault();
                            strHead += "</br> <b>Candidate State:</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (ExamCentre1 == 0)
                            strHead += "</br> <b>Exam Centre Choice 1 :</b>All";

                        else if (ExamCentre1 != 0)
                        {
                            application = application.Where(s => s.ExamCenter1ID == ExamCentre1).ToList();
                            ExamCenter ct = context.ExamCenters.Where(s => s.ID == ExamCentre1).FirstOrDefault();
                            strHead += "</br> <b>Exam Centre Choice 1 :</b>" + GetInitCap(ct.Name.ToString());
                        }
                        if (ExamCentre2 == 0)
                            strHead += "</br> <b>Exam Centre Choice 2 :</b>All";

                        else if (ExamCentre2 != 0)
                        {
                            application = application.Where(s => s.ExamCenter2ID == ExamCentre2).ToList();
                            ExamCenter ct = context.ExamCenters.Where(s => s.ID == ExamCentre2).FirstOrDefault();
                            strHead += "</br> <b>Exam Centre Choice 2 :</b>" + GetInitCap(ct.Name.ToString());
                        }
                    }
                    if (application.Count() > 0)
                    {
                        ShowTableHeader();
                        var app = application;
                        for (int i = 0; i < app.Count(); i++)
                        {
                            TableRow tr = new TableRow();
                            if (i % 2 == 0)
                                tr.CssClass = "gdalternate1";
                            else
                                tr.CssClass = "gdrow1";

                            TableCell tdRow = new TableCell();
                            tdRow.Width = Unit.Percentage(1);
                            tdRow.Text = (i + 1).ToString();
                            tdRow.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow);

                            TableCell tdRow1 = new TableCell();
                            tdRow1.Width = Unit.Percentage(3);
                            tdRow1.Text = app[i].Number;
                            if (app[i].DemandNoteID.HasValue)
                                tdRow1.Text += " ( Demand Note No: -  " + app[i].DemandNoteID.Value + " ) ";
                            else
                                tdRow1.Text += " ( Demand Note Not Generated ) ";
                            tdRow1.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow1);

                            TableCell tdRow2 = new TableCell();
                            tdRow2.Width = Unit.Percentage(10);
                            if (app[i].ApplicationDate != null)
                                tdRow2.Text = app[i].ApplicationDate.ToString("dd-MMM-yyyy");
                            tdRow2.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow2);

                            TableCell tdRow20 = new TableCell();
                            tdRow20.Width = Unit.Percentage(14);
                            tdRow20.Text = app[i].RegistrationNumber.ToString();
                            tdRow20.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow20);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(14);
                            tdRow3.Text = GetInitCap(app[i].CandidateName);
                            tdRow3.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow3);

                            if (string.IsNullOrEmpty(app[i].CandidateGuardianName) == true && string.IsNullOrWhiteSpace(app[i].CandidateGuardianName) == true)
                            {
                                TableCell tdRow4 = new TableCell();
                                tdRow4.Width = Unit.Percentage(12);
                                tdRow4.Text = GetInitCap(app[i].CandidateFatherName);
                                tdRow4.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow4);
                            }
                            else
                            {
                                TableCell tdRow4 = new TableCell();
                                tdRow4.Width = Unit.Percentage(12);
                                tdRow4.Text = GetInitCap(app[i].CandidateGuardianName);
                                tdRow4.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow4);
                            }

                            TableCell tdRow5 = new TableCell();
                            tdRow5.Width = Unit.Percentage(5);
                            if (app[i].MobileNumber != 0 && app[i].MobileNumber != null)
                                tdRow5.Text = app[i].MobileNumber.ToString();
                            tdRow5.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow5);

                            TableCell tdrow30 = new TableCell();
                            tdrow30.Width = Unit.Percentage(7);
                            if (!string.IsNullOrEmpty(app[i].EmailAddress))
                                tdrow30.Text = app[i].EmailAddress.ToString().ToLower();
                            tdrow30.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdrow30);

                            TableCell tdRow6 = new TableCell();
                            tdRow6.Width = Unit.Percentage(12);
                            tdRow6.Text = app[i].CandidateDateOfBirth.ToString("dd-MMM-yyyy");
                            tdRow6.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow6);

                            TableCell tdRow15 = new TableCell();
                            tdRow15.Width = Unit.Percentage(10);
                            tdRow15.Text = app[i].ExamCenter1Name;
                            tdRow15.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow15);

                            TableCell tdRow16 = new TableCell();
                            tdRow16.Width = Unit.Percentage(10);
                            tdRow16.Text = app[i].ExamCenter2Name;
                            tdRow16.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow16);

                            if (StatusId == 0)
                            {
                                TableCell tdRow7 = new TableCell();
                                tdRow7.Width = Unit.Percentage(16);
                                if ((app[i].ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT)) && (app[i].PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending)))
                                {
                                    tdRow7.Text = "Fee Pending to be Paid by Institute";
                                }
                                else if (app[i].ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT))
                                {
                                    tdRow7.Text = CommonFunctions.GetInitCap(EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmCourseExamApplicationStatus)(app[i].ApplicationStatusID)));
                                    if (app[i].FormForwardedByInstituteOn.HasValue)
                                        tdRow7.Text += "<br/> On :- " + app[i].FormForwardedByInstituteOn.Value.ToString("dd-MMM-yyyy");
                                }
                                else
                                {
                                    tdRow7.Text = EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmCourseExamApplicationStatus)(app[i].ApplicationStatusID)).ToString();
                                }

                                tdRow7.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow7);
                            }
                            TableCell tdRow8 = new TableCell();
                            tdRow8.Width = Unit.Percentage(14);
                            tdRow8.HorizontalAlign = HorizontalAlign.Left;
                            tdRow8.Text = EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.enmPaymentStatus)(app[i].PaymentStatusID)).ToString();
                            tr.Cells.Add(tdRow8);
                            if (loginUserType == UserType.RegionalCenter || loginUserType == UserType.HeadOffice || loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
                            {
                                TableCell tdRow7 = new TableCell();
                                tdRow7.Width = Unit.Percentage(15);
                                tdRow7.Text = "Direct";
                                tdRow7.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow7);
                                if (app[i].ApplicantTypeID == 2)
                                {
                                    if (app[i].InstituteID != null)
                                    {
                                        Int64 InstId = app[i].InstituteID.GetValueOrDefault();
                                        Int32 crsId = app[i].CourseID;
                                        tdRow7.Text = app[i].InstituteName;
                                        if (context.AccreditationDetails.Where(s => s.InstituteID == InstId && s.CourseID == crsId).Count() > 0)
                                        {
                                            tdRow7.Text += " ( " + context.AccreditationDetails.Where(s => s.InstituteID == InstId && s.CourseID == crsId).FirstOrDefault().AccreditationNumber + " ) ";
                                        }
                                        else
                                        {
                                            tdRow7.Text += " ( Accrediation Details Not Available ) ";
                                        }

                                        //for showing forwarded date
                                        if (app[i].FormForwardedByInstituteOn.HasValue && app[i].ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT))
                                        {
                                            tdRow7.Text += "<br/><b><i><font style='font-size:12px; color:green;'>Dispatched to Nielit On:- " + app[i].FormForwardedByInstituteOn.Value.ToString("dd-MMM-yyyy") + "<font/><i/><b/>";
                                        }
                                    }
                                }
                            }

                            TableCell tdRow17 = new TableCell();
                            tdRow17.Width = Unit.Percentage(10);
                            if (app[i].FeeAmount != 0)
                                tdRow17.Text = app[i].FeeAmount.ToString("F");
                            else
                                tdRow17.Text = "NA";
                            tr.Cells.Add(tdRow17);
                            tbl.Rows.Add(tr);
                        }
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }
                }
                #endregion---------------
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
    protected void imgAccess_Click(object sender, ImageClickEventArgs e)
    {
        OleDbConnection connection = new OleDbConnection(); ;
        OleDbCommand command = new OleDbCommand();
        int CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
        string Access = "";
        var filename = "Applications_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".mdb";
        try
        {
            context = new EConnectContext();
            Course currentCourse = context.Courses.Find(CourseId);
            if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
            {
                var result = (from c in context.CertificateExamApplications.AsNoTracking()
                              where c.CourseID == CourseId
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
                if (System.IO.File.Exists(Access))
                    System.IO.File.Delete(Access);
                System.IO.File.Copy(Server.MapPath("~/Download/Database_format_BCC.mdb"), Access);
                string connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Access + ";Persist Security Info=False;";
                connection.ConnectionString = connect;
                connection.Open();
                //command = new OleDbCommand("delete from  [MS Access;Database=" + Access + "].[Exam_Database]", connection);
                command = new OleDbCommand("delete from  [MS Access;Database=@Access].[Exam_Database]", connection);
                command.Parameters.Add("@Access", OleDbType.VarChar, 300);
                command.Parameters["@Access"].Value = Access;
                command.ExecuteNonQuery();
                foreach (var certificate in result)
                {
                    command = new OleDbCommand("insert into [MS Access;Database=@Access].[Exam_Database](Folder_no, batch_no,sr_no,exam_year,NAME,F_NAME,M_NAME,D_O_B,SEX,H_Qual,ADD1,ADD2,ADD3,CITY,STATE,PINCODE,PH_NO_C,EMAIL_C,CCC_NO,INST_NAME,INST_ADD,INST_STAT,TH_CENT_CH,SEC_TH_CEN,OCCUPATION,CATEGORY,PREV_APP,PREV_M,PREV_YEAR,PREV_ROLL,Rollno,cent_allot,cent_add,examDate,batch,rep_time)values('" + certificate.Folder_no + "','" + certificate.batch_no + "','" + certificate.sr_no + "','" + certificate.exam_year + "','" + certificate.NAME + "','" + certificate.F_NAME + "','" + certificate.M_NAME + "','" + certificate.D_O_B + "','" + certificate.SEX + "','" + certificate.H_Qual + "','" + certificate.ADD1 + "','" + certificate.ADD2 + "','" + certificate.ADD3 + "','" + certificate.CITY + "','" + certificate.STATE + "','" + certificate.PINCODE + "','" + certificate.PH_NO_C + "','" + certificate.EMAIL_C + "','" + certificate.CCC_NO + "','" + certificate.INST_NAME + "','" + certificate.INST_ADD + "','" + certificate.INST_STAT + "','" + certificate.TH_CENT_CH + "','" + certificate.SEC_TH_CEN + "','" + certificate.OCCUPATION + "','" + certificate.CATEGORY + "','" + certificate.PREV_APP + "','" + certificate.PREV_M + "','" + certificate.PREV_YEAR + "','" + certificate.PREV_ROLL + "','" + certificate.Rollno + "','" + certificate.cent_allot + "','" + certificate.cent_add + "','" + certificate.examDate + "','" + certificate.batch + "','" + certificate.rep_time + "')", connection);
                    command.Parameters.Clear();
                    command.Parameters.Add("@Access", OleDbType.VarChar, 300);
                    command.Parameters["@Access"].Value = Access;
                    command.ExecuteNonQuery();
                }
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Charset = "";
                Response.ContentType = "Application/vnd.mdb";

                Response.WriteFile(Access);
                Response.End();
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
        finally
        {
            context.Dispose();
            command.Dispose();
            connection.Close();
            connection.Dispose();
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
                //cmdAccess.CommandText = "select sr_no,Rollno,cent_allot,cent_add,batch,rep_time,examDate from [" + SheetName + "]";
                cmdAccess.CommandText = "select sr_no,Rollno,cent_allot,cent_add,batch,rep_time,examDate from [@SheetName]";
                cmdAccess.Parameters.Add("@SheetName", OleDbType.VarChar, 300);
                cmdAccess.Parameters["@SheetName"].Value = SheetName;
                oda.SelectCommand = cmdAccess;
                oda.Fill(dt);
                int succeedCounter = 0;
                int failedCounter = 0;
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
                               // String sql = "select Roll_Number from Certificate_Exam_Application where ID='" + dtrow["sr_no"].ToString() + "'";
                                String sql = "select Roll_Number from Certificate_Exam_Application where ID=@ID";
                                SqlParameter[] p ={
				                                new SqlParameter("@ID",dtrow["sr_no"].ToString())
                                                };
                                DataTable dtTbl = DbUtility.GetDataTable(sql, new EConnect.Connections.SqlCon(), p, CommandType.Text, false);
                                if (dtTbl.Rows[0][0] is DBNull)
                                {
                                    SqlParameter[] para1 ={
				                                new SqlParameter("@RollNo",Convert.ToInt64((dtrow["Rollno"].ToString().Trim()))),
                                                new SqlParameter("@ExamCentreName",dtrow["cent_allot"].ToString().Trim()),
                                                new SqlParameter("@ExamCentreAdd",dtrow["cent_add"].ToString().Trim()),
                                                new SqlParameter("@ExamBatchNo", dtrow["batch"].ToString().Trim() ),
                                                new SqlParameter("@ReportingTime",dtrow["rep_time"].ToString().Trim()),
                                                new SqlParameter("@ApplnStatusID",Convert.ToInt32(enmCertificateExamApplicationStatus.ExamCentreAndRollNumberAlloted)),
                                               new SqlParameter("@UpdatedOn", Convert.ToDateTime(DateTime.Now)),
                                                new SqlParameter("@ExamDate",Convert.ToDateTime(dtrow["examDate"].ToString())),
                                                new SqlParameter("@UpdatedBy",Convert.ToInt32(Session["UserID"])),
                                                new SqlParameter("@ID",dtrow["sr_no"].ToString())
                                                };
                                    context.Database.ExecuteSqlCommand("update Certificate_Exam_Application set  " +
                                                                       "Roll_Number =@RollNo," +
                                                                       "Exam_Centre_Name='@ExamCentreName'," +
                                                                       "Exam_Centre_Address='@ExamCentreAdd'," +
                                                                       "Exam_Batch_Number='@ExamBatchNo'," +
                                                                       "Reporting_Time='@ReportingTime'," +
                                                                       "Application_Status_ID= @ApplnStatusID," +
                                                                       "Updated_On=@UpdatedOn," +
                                                                       "Date_of_Exam=@ExamDate," +
                                                                       "Updated_By=@UpdatedBy where ID='@ID'",para1);

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