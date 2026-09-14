using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Text;
using System.Data.Objects;

public partial class HO_Rpt_RegistrationProcessingReport : BasePage
{
    Table tbl = new Table();
   
    Int32 StatusId = 0;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 applicantTypeID = 0;
    StringBuilder str = new StringBuilder();
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/RegistrationBatchProcessingFilter.aspx"))
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
    protected void ShowTableHeader(Int32 ApplicationstatusID)
    {
        try
        {
            int TypeId = Convert.ToInt32(Request.QueryString["TypeId"]);
            int reporttype = Convert.ToInt32(Request.QueryString["ReptFormat"]);
            if (reporttype == 2)
            {
                TableHeaderRow th = new TableHeaderRow();
                th.CssClass = "head1";

                //TableHeaderCell tc = new TableHeaderCell();
                //tc.Width = Unit.Percentage(1);
                //tc.Text = "#";
                //tc.HorizontalAlign = HorizontalAlign.Right;
                //th.Cells.Add(tc);

                TableHeaderCell tcCol = new TableHeaderCell();
                tcCol.Width = Unit.Percentage(1);
                tcCol.Text = "BatchNo.";
                tcCol.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol);

                str.Append("BatchNo." + "#");

                TableHeaderCell tcCol1 = new TableHeaderCell();
                tcCol1.Width = Unit.Percentage(1);
                tcCol1.Text = "App.No.";
                tcCol1.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol1);

                str.Append("SrNo." + "#");

                if (ApplicationstatusID == Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                {
                    TableHeaderCell tcCol2 = new TableHeaderCell();
                    tcCol2.Width = Unit.Percentage(1);
                    tcCol2.Text = "Regn.No";
                    tcCol2.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol2);
                    str.Append("REGN.NO" + "#");
                }

                TableHeaderCell tcCol3 = new TableHeaderCell();
                tcCol3.Width = Unit.Percentage(1);
                tcCol3.Text = "Level";
                tcCol3.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol3);
                str.Append("LEVEL" + "#");

                TableHeaderCell tcCol4 = new TableHeaderCell();
                tcCol4.Width = Unit.Percentage(5);
                tcCol4.Text = "Name";
                tcCol4.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol4);
                str.Append("NAME" + "#");

                TableHeaderCell tcCol14 = new TableHeaderCell();
                tcCol14.Width = Unit.Percentage(5);
                tcCol14.Text = "Father/Guardian Name";
                tcCol14.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol14);
                str.Append("FATHER_NAME" + "#");

                TableHeaderCell tcCol25 = new TableHeaderCell();
                tcCol25.Width = Unit.Percentage(5);
                tcCol25.Text = "Mother Name";
                tcCol25.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol25);
                str.Append("MOTHER_NAME" + "#");

                TableHeaderCell tcCol6 = new TableHeaderCell();
                tcCol6.Width = Unit.Percentage(8);
                tcCol6.Text = "Date of Birth";
                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol6);
                str.Append("D_O_B" + "#");

                if (ApplicationstatusID == Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                {
                    TableHeaderCell tcCol7 = new TableHeaderCell();
                    tcCol7.Width = Unit.Percentage(5);
                    tcCol7.Text = "Commenced From";
                    tcCol7.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol7);
                    str.Append("COMMENCING_FROM" + "#");

                    TableHeaderCell tcCol8 = new TableHeaderCell();
                    tcCol8.Width = Unit.Percentage(3);
                    tcCol8.Text = "Expiry Date";
                    tcCol8.HorizontalAlign = HorizontalAlign.Center;
                    tcCol8.Wrap = false;
                    th.Cells.Add(tcCol8);
                    str.Append("EXPIRY_DT" + "#");
                }

                TableHeaderCell tcCol9 = new TableHeaderCell();
                tcCol9.Width = Unit.Percentage(1);
                tcCol9.Text = "Address1";
                tcCol9.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol9);
                str.Append("ADD1" + "#");

                TableHeaderCell tcCol12 = new TableHeaderCell();
                tcCol12.Width = Unit.Percentage(1);
                tcCol12.Text = "Address2";
                tcCol12.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol12);
                str.Append("ADD2" + "#");

                TableHeaderCell tcCol13 = new TableHeaderCell();
                tcCol13.Width = Unit.Percentage(1);
                tcCol13.Text = "Address3";
                tcCol13.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol13);
                str.Append("ADD3" + "#");

                TableHeaderCell tcCol10 = new TableHeaderCell();
                tcCol10.Width = Unit.Percentage(2);
                tcCol10.Text = "City";
                tcCol10.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol10);
                str.Append("CITY" + "#");

                TableHeaderCell tcCol11 = new TableHeaderCell();
                tcCol11.Width = Unit.Percentage(2);
                tcCol11.Text = "State";
                tcCol11.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol11);
                str.Append("STATE" + "#");

                TableHeaderCell tcCol16 = new TableHeaderCell();
                tcCol16.Width = Unit.Percentage(2);
                tcCol16.Text = "PinCode";
                tcCol16.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol16);
                str.Append("PINCODE");

                tbl.Rows.Add(th);
                str.AppendLine();
            }
            else if (reporttype == 1)
            {

                TableHeaderRow th = new TableHeaderRow();
                th.CssClass = "head1";

                TableHeaderCell tc = new TableHeaderCell();
                tc.Width = Unit.Percentage(1);
                tc.Text = "S.No";
                tc.HorizontalAlign = HorizontalAlign.Right;
                th.Cells.Add(tc);

                if (ApplicationstatusID == Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                {
                    TableHeaderCell tcCol2 = new TableHeaderCell();
                    tcCol2.Width = Unit.Percentage(1);
                    tcCol2.Text = "Regn.No";
                    tcCol2.HorizontalAlign = HorizontalAlign.Center;
                    th.Cells.Add(tcCol2);
                }

                TableHeaderCell tcCol3 = new TableHeaderCell();
                tcCol3.Width = Unit.Percentage(1);
                tcCol3.Text = "Lvl";
                tcCol3.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol3);

                TableHeaderCell tcCol4 = new TableHeaderCell();
                tcCol4.Width = Unit.Percentage(5);
                tcCol4.Text = "Name";
                tcCol4.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol4);

                TableHeaderCell tcCol9 = new TableHeaderCell();
                tcCol9.Width = Unit.Percentage(1);
                tcCol9.Text = "Address";
                tcCol9.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol9);

                TableHeaderCell tcCol10 = new TableHeaderCell();
                tcCol10.Width = Unit.Percentage(2);
                tcCol10.Text = "City";
                tcCol10.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol10);

                TableHeaderCell tcCol11 = new TableHeaderCell();
                tcCol11.Width = Unit.Percentage(2);
                tcCol11.Text = "State";
                tcCol11.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol11);

                TableHeaderCell tcCol16 = new TableHeaderCell();
                tcCol16.Width = Unit.Percentage(2);
                tcCol16.Text = "PinCode";
                tcCol16.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol16);

                tbl.Rows.Add(th);
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
            applicantTypeID = Convert.ToInt32(Request.QueryString["applicantTypeID"]);
            int ExamId = Convert.ToInt32(Request.QueryString["ExamId"]);
            int CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            int TypeId = Convert.ToInt32(Request.QueryString["TypeId"]);
            Int32 Gender = Convert.ToInt32(Request.QueryString["Gender"]);
            Int32 Category = Convert.ToInt32(Request.QueryString["Category"]);
            Int32 CandidateState = Convert.ToInt32(Request.QueryString["CStateID"]);
            Int32 RegtypeID = Convert.ToInt32(Request.QueryString["RegTypeID"]);
            Int32 InstituteID = Convert.ToInt32(Request.QueryString["InstituteID"]);
            Int32 reportFormat = Convert.ToInt32(Request.QueryString["ReptFormat"]);
            DateTime regFromDate = Convert.ToDateTime(Request.QueryString["regFromDate"]);
            DateTime regToDate = Convert.ToDateTime(Request.QueryString["regToDate"]);
            Int32 applStatusId = Convert.ToInt32(Request.QueryString["ApplicationStatusId"]);
            Int32 batchno = Convert.ToInt32(Request.QueryString["batchno"]);
            StringBuilder mySql = new StringBuilder();
            Exam exam = context.Exams.Find(ExamId);
            string strHead = "";
            if (exam != null)
            {
                strHead += "<br/> <b>Application Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicationType)(TypeId)).ToString();
                strHead += "<br/> <b>Course Category :</b> " + exam.CourseCategory.Name;
                strHead += "<br/> <b>Course : </b>" + exam.Course.Name;
                strHead += "<br/> <b> Exam Cycle : </b>" + exam.ExaminationCycle.Name;
                strHead += "<br/> <b> Exam Name : </b>" + exam.Name;
                if (RegtypeID != 0)
                    strHead += "<br/> <b>Registration Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmRegistrationType)(RegtypeID)).ToString();
                else
                    strHead += "<br/> <b>Registration Type :</b>  All";
                if (reportFormat == 1)
                    strHead += "<br/> <b>Report Format :</b> " + " Registration Card Dispatch File Format";
                else
                    strHead += "<br/> <b>Report Format :</b> " + " Registration Card Data File Format";

            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "No Record Found";
            }
            if (ExamId != 0 && CourseId != 0 && TypeId != 0)
            {
                if (TypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    using (context = new EConnectContext())
                    {
                        if (applStatusId != 0)
                        {
                            if (applStatusId == Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                            {
                                var application = (from a in context.RegistrationDetails
                                                   join c in context.CourseRegistrationApplications
                                                   on a.CourseRegistrationApplicationID equals c.ID
                                                   join b in context.BatchItems
                                                       on c.BatchItemID equals b.ID
                                                   where c.ApplicableExamID == ExamId && c.CourseID == CourseId && c.FinalSubmitted == true
                                                   orderby b.Batch.Number, a.RegistrationNo
                                                   select new
                                                   {
                                                       ApplicantTypeID = c.ApplicantTypeID,
                                                       InstituteID = c.ApplicantTypeID == 2 ? c.InstituteID : 0,
                                                       Gender = c.Gender,
                                                       CastCategoryID = c.CastCategoryID,
                                                       CorStateID = c.CorStateID,
                                                       CommencementDate = a.CommencementFromDate,
                                                       Validdate = a.ValidUptoDate,
                                                       Name = c.Name,
                                                       Pincode = c.CorPinCode,
                                                       City = c.CorCityName,
                                                       Add1 = c.CorAddressLine1,
                                                       Add2 = c.CorAddressLine2,
                                                       Add3 = c.CorAddressLine3,
                                                       Regtypeid = a.RegistrationTypeID != null ? a.RegistrationTypeID.Value : 0,
                                                       Regdate = System.Data.Entity.DbFunctions.TruncateTime(a.RegistrationDate),
                                                       ReRegdate = a.RegistrationTypeID == 4 ? System.Data.Entity.DbFunctions.TruncateTime(a.ReRegistrationDate) : null,
                                                       Regno = a.RegistrationNo,
                                                       Level = c.Course.Code,
                                                       Dob = c.DateOfBirth,
                                                       State = c.CorStateID != 0 ? c.CorState.Name : "",
                                                       fathername = c.FatherName,
                                                       guardianname = c.GuardianName,
                                                       mothername = c.MotherName,
                                                       Batchno = b.Batch.Number,
                                                       BatchId = b.BatchID,
                                                       ApplicationStatus = c.ApplicationStatusID,
                                                       appno = b.CourseRegistrationApplicationID != null ? b.CourseRegistrationApplicationID.Value : 0
                                                   }).ToList();

                                if (applicantTypeID == 0)
                                    strHead += "<br/> <b>Applicant Type :</b>Both";
                                else
                                    strHead += "<br/> <b>Applicant Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicantType)(applicantTypeID)).ToString();
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
                                        strHead += "<br/> <b> Institute Name :</b> " + GetInitCap(inst.Name).ToString();
                                    }
                                }

                                if (applStatusId != 0)
                                {
                                    application = application.Where(s => s.ApplicationStatus == applStatusId).ToList();
                                    if (applStatusId == Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                                        strHead += "<br/> <b>Application Status:</b>Registration Number Alloted";
                                }

                                if (Gender == 0)
                                    strHead += "<br/> <b>Gender :</b>All";

                                else if (Gender == 1)
                                {
                                    string gender = "Female";
                                    application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                                    strHead += "<br/> <b>Gender :</b>Female";
                                }
                                else if (Gender == 2)
                                {
                                    string gender = "Male";
                                    application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                                    strHead += "<br/> <b>Gender :</b>Male";
                                }
                                if (Category == 0)
                                    strHead += "<br/> <b>Category :</b>All";

                                else if (Category != 0)
                                {
                                    application = application.Where(s => s.CastCategoryID == Category).ToList();
                                    CastCategory ct = context.CastCategories.Where(s => s.ID == Category).FirstOrDefault();
                                    strHead += "<br/> <b>Category:</b>" + GetInitCap(ct.Name.ToString());
                                }
                                if (CandidateState == 0)
                                    strHead += "<br/> <b>Candidate State :</b>All";

                                else if (CandidateState != 0)
                                {
                                    application = application.Where(s => s.CorStateID == CandidateState).ToList();
                                    Location ct = context.Locations.Where(s => s.ID == CandidateState && s.LocationTypeID == 2).FirstOrDefault();
                                    strHead += "<br/> <b>Candidate State:</b>" + GetInitCap(ct.Name.ToString());
                                }

                                if (RegtypeID != 0)
                                {
                                    if (RegtypeID == 4)
                                    {
                                        application = application.Where(s => s.ReRegdate >= regFromDate && s.ReRegdate <= regToDate && s.Regtypeid == RegtypeID).ToList();
                                        strHead += "<br/> <b>Re-Registration Date From :</b>" + regFromDate.ToString("dd-MMM-yyyy") + " to " + regToDate.ToString("dd-MMM-yyyy");
                                    }
                                    else
                                    {
                                        application = application.Where(s => s.Regdate >= regFromDate && s.Regdate <= regToDate && s.Regtypeid == RegtypeID).ToList();
                                        strHead += "<br/> <b>Registration Date From :</b>" + regFromDate.ToString("dd-MMM-yyyy") + " to " + regToDate.ToString("dd-MMM-yyyy");
                                    }
                                }
                                else
                                {
                                    application = application.Where(s => s.Regdate >= regFromDate && s.Regdate <= regToDate).ToList();
                                    strHead += "<br/> <b>Registration Date From :</b>" + regFromDate.ToString("dd-MMM-yyyy") + " to " + regToDate.ToString("dd-MMM-yyyy");
                                }
                                if (batchno != 0)
                                {
                                    application = application.Where(s => s.BatchId == batchno).ToList();
                                    Batch b = context.Batchs.Where(s => s.ID == batchno).FirstOrDefault();
                                    strHead += "<br/> <b>Batch Number:</b>" + b.Number.ToString().ToUpper();
                                }
                                if (application.Count() > 0)
                                {
                                    str = new StringBuilder();
                                    if (reportFormat == 2)
                                    {
                                        ibtext.Visible = true;
                                        ShowTableHeader(applStatusId);
                                        int i = 1;
                                        foreach (var app in application)
                                        {
                                            TableRow tr = new TableRow();
                                            if (i % 2 == 0)
                                                tr.CssClass = "gdalternate1";
                                            else
                                                tr.CssClass = "gdrow1";

                                            //TableCell tdRow = new TableCell();
                                            //tdRow.Width = Unit.Percentage(1);
                                            //tdRow.Text = i.ToString();
                                            //tdRow.HorizontalAlign = HorizontalAlign.Right;
                                            //tr.Cells.Add(tdRow);

                                            TableCell tdRow1 = new TableCell();
                                            tdRow1.Width = Unit.Percentage(1);
                                            tdRow1.Text = app.Batchno.ToString();
                                            tdRow1.HorizontalAlign = HorizontalAlign.Center;
                                            tr.Cells.Add(tdRow1);
                                            str.Append(app.Batchno.ToString() + "#");

                                            TableCell tdRow2 = new TableCell();
                                            tdRow2.Width = Unit.Percentage(1);
                                            CourseRegistrationApplication courreg = context.CourseRegistrationApplications.Where(s => s.ID == app.appno).FirstOrDefault();
                                            if (courreg != null)
                                                tdRow2.Text = courreg.Number.ToString();
                                            else
                                                tdRow2.Text = "";
                                            tdRow2.HorizontalAlign = HorizontalAlign.Right;
                                            tr.Cells.Add(tdRow2);
                                            str.Append(app.appno.ToString() + "#");

                                            TableCell tdRow3 = new TableCell();
                                            tdRow3.Width = Unit.Percentage(1);
                                            tdRow3.Text = app.Regno.ToString();
                                            tdRow3.HorizontalAlign = HorizontalAlign.Right;
                                            tr.Cells.Add(tdRow3);
                                            str.Append(app.Regno.ToString() + "#");

                                            TableCell tdRow5 = new TableCell();
                                            tdRow5.Width = Unit.Percentage(1);
                                            if (app.Level != null)
                                                tdRow5.Text = app.Level.ToString().ToUpper();
                                            tdRow5.HorizontalAlign = HorizontalAlign.Center;
                                            tr.Cells.Add(tdRow5);
                                            str.Append(app.Level.ToString().ToUpper() + "#");

                                            TableCell tdRow6 = new TableCell();
                                            tdRow6.Width = Unit.Percentage(8);
                                            if (app.Name != null)
                                                tdRow6.Text = app.Name.ToUpper();
                                            tdRow6.HorizontalAlign = HorizontalAlign.Left;
                                            tdRow6.Wrap = false;
                                            tr.Cells.Add(tdRow6);
                                            str.Append(app.Name.ToUpper() + "#");

                                            if (string.IsNullOrEmpty(app.guardianname) == true && string.IsNullOrWhiteSpace(app.guardianname) == true)
                                            {
                                                TableCell tdRow4 = new TableCell();
                                                tdRow4.Width = Unit.Percentage(8);
                                                if (app.fathername != null)
                                                    tdRow4.Text = app.fathername.ToUpper();
                                                tdRow4.HorizontalAlign = HorizontalAlign.Left;
                                                tdRow4.Wrap = false;
                                                tr.Cells.Add(tdRow4);
                                                str.Append(app.fathername.ToUpper() + "#");

                                                TableCell tdRow35 = new TableCell();
                                                tdRow35.Width = Unit.Percentage(8);
                                                if (app.mothername != null)
                                                    tdRow35.Text = app.mothername.ToUpper();
                                                tdRow35.HorizontalAlign = HorizontalAlign.Left;
                                                tdRow35.Wrap = false;
                                                tr.Cells.Add(tdRow35);
                                                str.Append(app.mothername.ToUpper() + "#");
                                            }
                                            else
                                            {
                                                TableCell tdRow4 = new TableCell();
                                                tdRow4.Width = Unit.Percentage(8);
                                                if (app.guardianname != null)
                                                    tdRow4.Text = app.guardianname.ToUpper();
                                                tdRow4.HorizontalAlign = HorizontalAlign.Left;
                                                tdRow4.Wrap = false;
                                                tr.Cells.Add(tdRow4);
                                                str.Append(app.guardianname.ToUpper() + "#");

                                                TableCell tdRow45 = new TableCell();
                                                tdRow45.Width = Unit.Percentage(8);
                                                tdRow45.Text = "NA";
                                                tdRow45.HorizontalAlign = HorizontalAlign.Left;
                                                tdRow45.Wrap = false;
                                                tr.Cells.Add(tdRow45);
                                                str.Append("NA" + "#");
                                            }

                                            TableCell tdRow7 = new TableCell();
                                            tdRow7.Width = Unit.Percentage(10);
                                            if (app.Dob != null)
                                                tdRow7.Text = app.Dob.ToString("dd-MMM-yyyy");
                                            tdRow7.HorizontalAlign = HorizontalAlign.Center;
                                            tdRow7.Wrap = false;
                                            tr.Cells.Add(tdRow7);
                                            str.Append(app.Dob.ToString("dd-MMM-yyyy") + "#");

                                            TableCell tdRow8 = new TableCell();
                                            tdRow8.Width = Unit.Percentage(1);
                                            if (app.CommencementDate != null)
                                                tdRow8.Text = app.CommencementDate.ToString("MMM-yyyy");
                                            tdRow8.HorizontalAlign = HorizontalAlign.Center;
                                            tr.Cells.Add(tdRow8);
                                            str.Append(app.CommencementDate.ToString("MMM-yyyy") + "#");

                                            TableCell tdRow9 = new TableCell();
                                            tdRow9.Width = Unit.Percentage(1);
                                            if (app.Validdate != null)
                                                tdRow9.Text = app.Validdate.ToString("MMM-yyyy");
                                            tdRow9.HorizontalAlign = HorizontalAlign.Center;
                                            tr.Cells.Add(tdRow9);
                                            str.Append(app.Validdate.ToString("MMM-yyyy") + "#");

                                            TableCell tdRow10 = new TableCell();
                                            tdRow10.Width = Unit.Percentage(2);
                                            if (!String.IsNullOrEmpty(app.Add1) && !String.IsNullOrWhiteSpace(app.Add1))
                                            {
                                                tdRow10.Text = app.Add1.ToString().ToUpper();
                                                str.Append(app.Add1.ToString().ToUpper().Replace("#", "") + "#");
                                            }
                                            else
                                            {
                                                str.Append("#");
                                            }
                                            tdRow10.HorizontalAlign = HorizontalAlign.Left;
                                            tr.Cells.Add(tdRow10);


                                            TableCell tdRow14 = new TableCell();
                                            tdRow14.Width = Unit.Percentage(2);
                                            if (!String.IsNullOrEmpty(app.Add2) && !String.IsNullOrWhiteSpace(app.Add2))
                                            {
                                                tdRow14.Text = app.Add2.ToString().ToUpper();
                                                str.Append(app.Add2.ToString().ToUpper().Replace("#", "") + "#");
                                            }
                                            else
                                            {
                                                str.Append("#");
                                            }
                                            tdRow14.HorizontalAlign = HorizontalAlign.Left;
                                            tr.Cells.Add(tdRow14);


                                            TableCell tdRow15 = new TableCell();
                                            tdRow15.Width = Unit.Percentage(2);
                                            if (!String.IsNullOrEmpty(app.Add3) && !String.IsNullOrWhiteSpace(app.Add3))
                                            {
                                                tdRow15.Text = app.Add3.ToString().ToUpper();
                                                str.Append(app.Add3.ToString().ToUpper().Replace("#", "") + "#");
                                            }
                                            else
                                            {
                                                str.Append("#");
                                            }
                                            tdRow15.HorizontalAlign = HorizontalAlign.Left;
                                            tr.Cells.Add(tdRow15);
                                            //str.Append(app.Add3.ToString().ToUpper().Replace("#", "") + "#");

                                            TableCell tdRow11 = new TableCell();
                                            tdRow11.Width = Unit.Percentage(2);
                                            if (app.City != null)
                                                tdRow11.Text = app.City.ToString().ToUpper();
                                            tdRow11.HorizontalAlign = HorizontalAlign.Left;
                                            tr.Cells.Add(tdRow11);
                                            str.Append(app.City.ToString().ToUpper() + "#");

                                            TableCell tdRow12 = new TableCell();
                                            tdRow12.Width = Unit.Percentage(4);
                                            if (app.CorStateID != null)
                                                tdRow12.Text = app.State.ToString().ToUpper();
                                            tdRow12.HorizontalAlign = HorizontalAlign.Left;
                                            tr.Cells.Add(tdRow12);
                                            str.Append(app.State.ToString().ToUpper() + "#");

                                            TableCell tdRow13 = new TableCell();
                                            tdRow13.Width = Unit.Percentage(4);
                                            if (app.Pincode != null)
                                                tdRow13.Text = app.Pincode.ToString();
                                            tdRow13.HorizontalAlign = HorizontalAlign.Right;
                                            tr.Cells.Add(tdRow13);
                                            str.Append(app.Pincode.ToString());

                                            tbl.Rows.Add(tr);
                                            str.AppendLine();
                                            i++;
                                        }
                                        lblCount.Visible = true;
                                        lblCount.Text = "Total Records : " + application.Count().ToString();
                                    }
                                    else if (reportFormat == 1)
                                    {
                                        ibtext.Visible = false;
                                        ShowTableHeader(applStatusId);
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

                                            TableCell tdRow3 = new TableCell();
                                            tdRow3.Width = Unit.Percentage(1);
                                            tdRow3.Text = app.Regno.ToString();
                                            tdRow3.HorizontalAlign = HorizontalAlign.Right;
                                            tr.Cells.Add(tdRow3);


                                            TableCell tdRow5 = new TableCell();
                                            tdRow5.Width = Unit.Percentage(1);
                                            if (app.Level != null)
                                                tdRow5.Text = app.Level.ToString().ToUpper();
                                            tdRow5.HorizontalAlign = HorizontalAlign.Center;
                                            tr.Cells.Add(tdRow5);


                                            TableCell tdRow6 = new TableCell();
                                            tdRow6.Width = Unit.Percentage(10);
                                            if (app.Name != null)
                                                tdRow6.Text = app.Name.ToUpper();
                                            tdRow6.HorizontalAlign = HorizontalAlign.Left;
                                            tdRow6.Wrap = false;
                                            tr.Cells.Add(tdRow6);


                                            TableCell tdRow10 = new TableCell();
                                            tdRow10.Width = Unit.Percentage(15);
                                            if (!String.IsNullOrEmpty(app.Add1) && !String.IsNullOrWhiteSpace(app.Add1))
                                                tdRow10.Text = app.Add1.ToString().Replace("#", "").ToUpper();
                                            if (!String.IsNullOrEmpty(app.Add2) && !String.IsNullOrWhiteSpace(app.Add2))
                                                tdRow10.Text += " " + app.Add2.ToString().Replace("#", "").ToUpper();
                                            if (!String.IsNullOrEmpty(app.Add3) && !String.IsNullOrWhiteSpace(app.Add3))
                                                tdRow10.Text += " " + app.Add3.ToString().Replace("#", "").ToUpper();
                                            tdRow10.HorizontalAlign = HorizontalAlign.Left;
                                            tdRow10.Wrap = false;
                                            tr.Cells.Add(tdRow10);


                                            TableCell tdRow11 = new TableCell();
                                            tdRow11.Width = Unit.Percentage(4);
                                            if (app.City != null)
                                                tdRow11.Text = app.City.ToString().ToUpper();
                                            tdRow11.HorizontalAlign = HorizontalAlign.Left;
                                            tr.Cells.Add(tdRow11);

                                            TableCell tdRow12 = new TableCell();
                                            tdRow12.Width = Unit.Percentage(4);
                                            if (app.CorStateID != null)
                                                tdRow12.Text = app.State.ToString().ToUpper();
                                            tdRow12.HorizontalAlign = HorizontalAlign.Left;
                                            tdRow12.Wrap = false;
                                            tr.Cells.Add(tdRow12);

                                            TableCell tdRow13 = new TableCell();
                                            tdRow13.Width = Unit.Percentage(4);
                                            if (app.Pincode != null)
                                                tdRow13.Text = app.Pincode.ToString();
                                            tdRow13.HorizontalAlign = HorizontalAlign.Right;
                                            tr.Cells.Add(tdRow13);

                                            tbl.Rows.Add(tr);
                                            i++;
                                        }
                                        lblCount.Visible = true;
                                        lblCount.Text = "Total Records : " + application.Count().ToString();
                                    }
                                }
                                else
                                {
                                    lblError.Visible = true;
                                    lblError.Text = "No Record Found";
                                }
                            }
                            else if (applStatusId == Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance) || applStatusId == Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedWithReason))
                            {
                                var application = (from c in context.CourseRegistrationApplications
                                                   join b in context.BatchItems
                                                       on c.BatchItemID equals b.ID
                                                   where c.ApplicableExamID == ExamId && c.CourseID == CourseId && c.FinalSubmitted == true
                                                   orderby b.Batch.Number
                                                   select new
                                                   {
                                                       ApplicantTypeID = c.ApplicantTypeID,
                                                       InstituteID = c.ApplicantTypeID == 2 ? c.InstituteID : 0,
                                                       Gender = c.Gender,
                                                       CastCategoryID = c.CastCategoryID,
                                                       CorStateID = c.CorStateID,
                                                       Name = c.Name,
                                                       ApplicationDate = System.Data.Entity.DbFunctions.TruncateTime(c.ApplicationDate),
                                                       Pincode = c.CorPinCode,
                                                       City = c.CorCityName,
                                                       Add1 = c.CorAddressLine1,
                                                       Add2 = c.CorAddressLine2,
                                                       Add3 = c.CorAddressLine3,
                                                       Regtypeid = c.RegistrationTypeID,
                                                       Level = c.Course.Code,
                                                       Dob = c.DateOfBirth,
                                                       State = c.CorStateID != 0 ? c.CorState.Name : "",
                                                       fathername = c.FatherName,
                                                       guardianname = c.GuardianName,
                                                       Batchno = b.Batch.Number,
                                                       BatchId = b.BatchID,
                                                       ApplicationStatus = c.ApplicationStatusID,
                                                       appno = b.CourseRegistrationApplicationID != null ? b.CourseRegistrationApplicationID.Value : 0
                                                   }).ToList();

                                if (applicantTypeID == 0)
                                    strHead += "<br/> <b>Applicant Type :</b>Both";
                                else
                                    strHead += "<br/> <b>Applicant Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicantType)(applicantTypeID)).ToString();
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
                                        strHead += "<br/> <b> Institute Name :</b> " + GetInitCap(inst.Name).ToString();
                                    }
                                }

                                if (applStatusId != 0)
                                {
                                    application = application.Where(s => s.ApplicationStatus == applStatusId).ToList();
                                    if (applStatusId == Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedWithReason))
                                        strHead += "<br/> <b>Application Status:</b>Application Rejected";
                                    else if (applStatusId == Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance))
                                        strHead += "<br/> <b>Application Status:</b>Kept In Abeyance";
                                }

                                if (Gender == 0)
                                    strHead += "<br/> <b>Gender :</b>All";

                                else if (Gender == 1)
                                {
                                    string gender = "Female";
                                    application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                                    strHead += "<br/> <b>Gender :</b>Female";
                                }
                                else if (Gender == 2)
                                {
                                    string gender = "Male";
                                    application = application.Where(s => s.Gender.ToUpper().Trim() == gender.ToUpper().Trim()).ToList();
                                    strHead += "<br/> <b>Gender :</b>Male";
                                }
                                if (Category == 0)
                                    strHead += "<br/> <b>Category :</b>All";

                                else if (Category != 0)
                                {
                                    application = application.Where(s => s.CastCategoryID == Category).ToList();
                                    CastCategory ct = context.CastCategories.Where(s => s.ID == Category).FirstOrDefault();
                                    strHead += "<br/> <b>Category:</b>" + GetInitCap(ct.Name.ToString());
                                }
                                if (CandidateState == 0)
                                    strHead += "<br/> <b>Candidate State :</b>All";

                                else if (CandidateState != 0)
                                {
                                    application = application.Where(s => s.CorStateID == CandidateState).ToList();
                                    Location ct = context.Locations.Where(s => s.ID == CandidateState && s.LocationTypeID == 2).FirstOrDefault();
                                    strHead += "<br/> <b>Candidate State:</b>" + GetInitCap(ct.Name.ToString());
                                }
                                if (batchno != 0)
                                {
                                    application = application.Where(s => s.BatchId == batchno).ToList();
                                    Batch b = context.Batchs.Where(s => s.ID == batchno).FirstOrDefault();
                                    strHead += "<br/> <b>Batch Number:</b>" + b.Number.ToString().ToUpper();
                                }

                                if (RegtypeID != 0)
                                {
                                    application = application.Where(s => s.ApplicationDate >= regFromDate && s.ApplicationDate <= regToDate && s.Regtypeid == RegtypeID).ToList();
                                    strHead += "<br/> <b>Application Date From :</b>" + regFromDate.ToString("dd-MMM-yyyy") + " to " + regToDate.ToString("dd-MMM-yyyy");
                                }
                                else
                                {
                                    application = application.Where(s => s.ApplicationDate >= regFromDate && s.ApplicationDate <= regToDate).ToList();
                                    strHead += "<br/> <b>Application Date From :</b>" + regFromDate.ToString("dd-MMM-yyyy") + " to " + regToDate.ToString("dd-MMM-yyyy");
                                }
                                if (application.Count() > 0)
                                {
                                    str = new StringBuilder();
                                    if (reportFormat == 2)
                                    {
                                        ibtext.Visible = true;
                                        ShowTableHeader(applStatusId);
                                        int i = 1;
                                        foreach (var app in application)
                                        {
                                            TableRow tr = new TableRow();
                                            if (i % 2 == 0)
                                                tr.CssClass = "gdalternate1";
                                            else
                                                tr.CssClass = "gdrow1";

                                            //TableCell tdRow = new TableCell();
                                            //tdRow.Width = Unit.Percentage(1);
                                            //tdRow.Text = i.ToString();
                                            //tdRow.HorizontalAlign = HorizontalAlign.Right;
                                            //tr.Cells.Add(tdRow);

                                            TableCell tdRow1 = new TableCell();
                                            tdRow1.Width = Unit.Percentage(1);
                                            tdRow1.Text = app.Batchno.ToString().ToUpper();
                                            tdRow1.HorizontalAlign = HorizontalAlign.Center;
                                            tr.Cells.Add(tdRow1);
                                            str.Append(app.Batchno.ToString().ToUpper() + "#");

                                            TableCell tdRow2 = new TableCell();
                                            tdRow2.Width = Unit.Percentage(1);
                                            CourseRegistrationApplication courreg = context.CourseRegistrationApplications.Where(s => s.ID == app.appno).FirstOrDefault();
                                            if (courreg != null)
                                                tdRow2.Text = courreg.Number.ToString();
                                            else
                                                tdRow2.Text = "";
                                            tdRow2.HorizontalAlign = HorizontalAlign.Right;
                                            tr.Cells.Add(tdRow2);
                                            str.Append(app.appno.ToString() + "#");

                                            TableCell tdRow5 = new TableCell();
                                            tdRow5.Width = Unit.Percentage(1);
                                            if (app.Level != null)
                                                tdRow5.Text = app.Level.ToString().ToUpper();
                                            tdRow5.HorizontalAlign = HorizontalAlign.Center;
                                            tr.Cells.Add(tdRow5);
                                            str.Append(app.Level.ToString() + "#");

                                            TableCell tdRow6 = new TableCell();
                                            tdRow6.Width = Unit.Percentage(10);
                                            if (app.Name != null)
                                                tdRow6.Text = app.Name.ToUpper();
                                            tdRow6.HorizontalAlign = HorizontalAlign.Left;
                                            tdRow6.Wrap = false;
                                            tr.Cells.Add(tdRow6);
                                            str.Append(app.Name.ToUpper() + "#");


                                            if (string.IsNullOrEmpty(app.guardianname) == true && string.IsNullOrWhiteSpace(app.guardianname) == true)
                                            {
                                                TableCell tdRow4 = new TableCell();
                                                tdRow4.Width = Unit.Percentage(10);
                                                if (app.fathername != null)
                                                    tdRow4.Text = app.fathername.ToUpper();
                                                tdRow4.HorizontalAlign = HorizontalAlign.Left;
                                                tdRow4.Wrap = false;
                                                tr.Cells.Add(tdRow4);
                                                str.Append(app.fathername.ToUpper() + "#");
                                            }
                                            else
                                            {
                                                TableCell tdRow4 = new TableCell();
                                                tdRow4.Width = Unit.Percentage(10);
                                                if (app.guardianname != null)
                                                    tdRow4.Text = app.guardianname.ToUpper();
                                                tdRow4.HorizontalAlign = HorizontalAlign.Left;
                                                tdRow4.Wrap = false;
                                                tr.Cells.Add(tdRow4);
                                                str.Append(app.guardianname.ToUpper() + "#");
                                            }

                                            TableCell tdRow7 = new TableCell();
                                            tdRow7.Width = Unit.Percentage(10);
                                            if (app.Dob != null)
                                                tdRow7.Text = app.Dob.ToString("dd-MMM-yyyy");
                                            tdRow7.HorizontalAlign = HorizontalAlign.Center;
                                            tdRow7.Wrap = false;
                                            tr.Cells.Add(tdRow7);
                                            str.Append(app.Dob.ToString("dd-MMM-yyyy") + "#");

                                            TableCell tdRow10 = new TableCell();
                                            tdRow10.Width = Unit.Percentage(2);
                                            if (!String.IsNullOrEmpty(app.Add1) && !String.IsNullOrWhiteSpace(app.Add1))
                                            {
                                                tdRow10.Text = app.Add1.ToString().ToUpper();
                                                str.Append(app.Add1.ToString().Replace("#", "").ToUpper() + "#");
                                            }
                                            else
                                            {
                                                str.Append("#");
                                            }
                                            tdRow10.HorizontalAlign = HorizontalAlign.Left;
                                            tr.Cells.Add(tdRow10);


                                            TableCell tdRow14 = new TableCell();
                                            tdRow14.Width = Unit.Percentage(2);
                                            if (!String.IsNullOrEmpty(app.Add2) && !String.IsNullOrWhiteSpace(app.Add2))
                                            {
                                                tdRow14.Text = app.Add2.ToString().ToUpper();
                                                str.Append(app.Add2.ToString().Replace("#", "").ToUpper() + "#");
                                            }
                                            else
                                            {
                                                str.Append("#");
                                            }
                                            tdRow14.HorizontalAlign = HorizontalAlign.Left;
                                            tr.Cells.Add(tdRow14);


                                            TableCell tdRow15 = new TableCell();
                                            tdRow15.Width = Unit.Percentage(2);
                                            if (!String.IsNullOrEmpty(app.Add3) && !String.IsNullOrWhiteSpace(app.Add3))
                                            {
                                                tdRow15.Text = app.Add3.ToString().ToUpper();
                                                str.Append(app.Add3.ToString().Replace("#", "").ToUpper() + "#");
                                            }
                                            else
                                            {
                                                str.Append("#");
                                            }
                                            tdRow15.HorizontalAlign = HorizontalAlign.Left;
                                            tr.Cells.Add(tdRow15);
                                            //str.Append(app.Add3.ToString().Replace("#", "").ToUpper() + "#");

                                            TableCell tdRow11 = new TableCell();
                                            tdRow11.Width = Unit.Percentage(2);
                                            if (app.City != null)
                                                tdRow11.Text = app.City.ToUpper();
                                            tdRow11.HorizontalAlign = HorizontalAlign.Left;
                                            tr.Cells.Add(tdRow11);
                                            str.Append(app.City.ToUpper() + "#");

                                            TableCell tdRow12 = new TableCell();
                                            tdRow12.Width = Unit.Percentage(4);
                                            if (app.CorStateID != null)
                                                tdRow12.Text = app.State.ToUpper();
                                            tdRow12.HorizontalAlign = HorizontalAlign.Left;
                                            tr.Cells.Add(tdRow12);
                                            str.Append(app.State.ToUpper() + "#");

                                            TableCell tdRow13 = new TableCell();
                                            tdRow13.Width = Unit.Percentage(4);
                                            if (app.Pincode != null)
                                                tdRow13.Text = app.Pincode.ToString();
                                            tdRow13.HorizontalAlign = HorizontalAlign.Right;
                                            tr.Cells.Add(tdRow13);
                                            str.Append(app.Pincode.ToString());

                                            tbl.Rows.Add(tr);
                                            str.AppendLine();
                                            i++;
                                        }
                                        lblCount.Visible = true;
                                        lblCount.Text = "Total Records : " + application.Count().ToString();
                                    }
                                    else if (reportFormat == 1)
                                    {
                                        ibtext.Visible = false;
                                        ShowTableHeader(applStatusId);
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

                                            TableCell tdRow5 = new TableCell();
                                            tdRow5.Width = Unit.Percentage(1);
                                            if (app.Level != null)
                                                tdRow5.Text = app.Level.ToString().ToUpper();
                                            tdRow5.HorizontalAlign = HorizontalAlign.Center;
                                            tr.Cells.Add(tdRow5);

                                            TableCell tdRow6 = new TableCell();
                                            tdRow6.Width = Unit.Percentage(10);
                                            if (app.Name != null)
                                                tdRow6.Text = app.Name.ToUpper();
                                            tdRow6.HorizontalAlign = HorizontalAlign.Left;
                                            tdRow6.Wrap = false;
                                            tr.Cells.Add(tdRow6);

                                            TableCell tdRow10 = new TableCell();
                                            tdRow10.Width = Unit.Percentage(2);
                                            if (!String.IsNullOrEmpty(app.Add1) && !String.IsNullOrWhiteSpace(app.Add1))
                                                tdRow10.Text = app.Add1.ToString().Replace("#", "").ToUpper();
                                            if (!String.IsNullOrEmpty(app.Add2) && !String.IsNullOrWhiteSpace(app.Add2))
                                                tdRow10.Text += " " + app.Add2.ToString().Replace("#", "").ToUpper();
                                            if (!String.IsNullOrEmpty(app.Add3) && !String.IsNullOrWhiteSpace(app.Add3))
                                                tdRow10.Text += " " + app.Add3.ToString().Replace("#", "").ToUpper();
                                            tdRow10.HorizontalAlign = HorizontalAlign.Left;
                                            tdRow10.Wrap = false;
                                            tr.Cells.Add(tdRow10);

                                            TableCell tdRow11 = new TableCell();
                                            tdRow11.Width = Unit.Percentage(2);
                                            if (app.City != null)
                                                tdRow11.Text = app.City.ToString().ToUpper();
                                            tdRow11.HorizontalAlign = HorizontalAlign.Left;
                                            tr.Cells.Add(tdRow11);

                                            TableCell tdRow12 = new TableCell();
                                            tdRow12.Width = Unit.Percentage(4);
                                            if (app.CorStateID != null)
                                                tdRow12.Text = app.State.ToString().ToUpper();
                                            tdRow12.HorizontalAlign = HorizontalAlign.Left;
                                            tdRow12.Wrap = false;
                                            tr.Cells.Add(tdRow12);

                                            TableCell tdRow13 = new TableCell();
                                            tdRow13.Width = Unit.Percentage(4);
                                            if (app.Pincode != null)
                                                tdRow13.Text = app.Pincode.ToString();
                                            tdRow13.HorizontalAlign = HorizontalAlign.Right;
                                            tr.Cells.Add(tdRow13);
                                            str.Append(app.Pincode.ToString());

                                            tbl.Rows.Add(tr);
                                            i++;

                                        }
                                        lblCount.Visible = true;
                                        lblCount.Text = "Total Records : " + application.Count().ToString();
                                    }
                                }
                                else
                                {
                                    lblError.Visible = true;
                                    lblError.Text = "No Record Found";
                                }
                            }
                        }
                    };
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
    protected void ibtext_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            divReportData.Visible = true;
            ShowData();
            divReportData.Controls.Add(tbl);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Applications.txt");
            Response.Charset = "";
            Response.ContentType = "text/plain";
            Response.Charset = "UTF-8";
            Response.Write(str.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}