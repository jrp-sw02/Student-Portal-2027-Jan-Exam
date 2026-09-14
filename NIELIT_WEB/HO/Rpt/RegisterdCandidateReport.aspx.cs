using System;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class HO_Rpt_RegisterdCandidateReport : BasePage
{
    Table tbl = new Table();
    EConnectContext context;
    Int32 StatusId = 0;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 applicantTypeID = 0;
    StringBuilder str = new StringBuilder();
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/RegisterdCandidateFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            //if (Request.UrlReferrer == null)
            //{
            //    Response.Write(GeInvalidRequestMessage("Goto Home Page", "../MainPage.aspx"));
            //    Response.End();
            //    return;
            //}
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

            TableHeaderCell tc = new TableHeaderCell();
            tc.Width = Unit.Percentage(1);
            tc.Text = "#";
            tc.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tc);


            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(1);
            tcCol1.Text = "App.No.";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(1);
            tcCol2.Text = "Regn.No";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(1);
            tcCol3.Text = "Level";
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(5);
            tcCol4.Text = "Name";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol14 = new TableHeaderCell();
            tcCol14.Width = Unit.Percentage(5);
            tcCol14.Text = "Father/Guardian Name";
            tcCol14.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol14);


            TableHeaderCell tcCol25 = new TableHeaderCell();
            tcCol25.Width = Unit.Percentage(5);
            tcCol25.Text = "Mother Name";
            tcCol25.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol25);


            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(8);
            tcCol6.Text = "Date of Birth";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);


            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(5);
            tcCol7.Text = "Commenced From";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);


            TableHeaderCell tcCol8 = new TableHeaderCell();
            tcCol8.Width = Unit.Percentage(3);
            tcCol8.Text = "Expiry Date";
            tcCol8.HorizontalAlign = HorizontalAlign.Center;
            tcCol8.Wrap = false;
            th.Cells.Add(tcCol8);


            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(15);
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
            int ExamId = Convert.ToInt32(Request.QueryString["ExamId"]);
            int CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            int TypeId = Convert.ToInt32(Request.QueryString["TypeId"]);
            Int32 applStatusId = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
            Int32 applicantTypeID = Convert.ToInt32(enmApplicantType.Institute);
            Int32 RegtypeID = Convert.ToInt32(Request.QueryString["RegTypeID"]);
            Int64 InstituteID = entityID;
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
                strHead += "<br/> <b>Registration Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmRegistrationType)(RegtypeID)).ToString();
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "No Record Found";
            }
            if (ExamId != 0 && CourseId != 0 && TypeId != 0 && RegtypeID != 0)
            {
                if (TypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    //using (context = new EConnectContext())
                    //{
                    var application = (from a in context.RegistrationDetails
                                       join c in context.CourseRegistrationApplications
                                       on a.CourseRegistrationApplicationID equals c.ID
                                       where c.ApplicableExamID == ExamId && c.CourseID == CourseId && c.FinalSubmitted == true
                                       orderby c.Number, a.RegistrationNo
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
                                           ApplicationStatus = c.ApplicationStatusID,
                                           appno = a.CourseRegistrationApplicationID.HasValue ? a.CourseRegistrationApplicationID : 0
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

                    if (RegtypeID == 4)
                    {
                        application = application.Where(s => s.Regtypeid == RegtypeID).ToList();
                    }
                    else
                    {
                        application = application.Where(s => s.Regtypeid == RegtypeID).ToList();
                    }

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


                            TableCell tdRow2 = new TableCell();
                            tdRow2.Width = Unit.Percentage(1);
                            CourseRegistrationApplication courreg = context.CourseRegistrationApplications.Where(s => s.ID == app.appno).FirstOrDefault();
                            if (courreg != null)
                                tdRow2.Text = courreg.Number.ToString();
                            else
                                tdRow2.Text = "";
                            tdRow2.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow2);

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
                            tdRow6.Width = Unit.Percentage(8);
                            if (app.Name != null)
                                tdRow6.Text = app.Name.ToUpper();
                            tdRow6.HorizontalAlign = HorizontalAlign.Left;
                            tdRow6.Wrap = false;
                            tr.Cells.Add(tdRow6);


                            if (string.IsNullOrEmpty(app.guardianname) == true && string.IsNullOrWhiteSpace(app.guardianname) == true)
                            {
                                TableCell tdRow4 = new TableCell();
                                tdRow4.Width = Unit.Percentage(8);
                                if (app.fathername != null)
                                    tdRow4.Text = app.fathername.ToUpper();
                                tdRow4.HorizontalAlign = HorizontalAlign.Left;
                                tdRow4.Wrap = false;
                                tr.Cells.Add(tdRow4);


                                TableCell tdRow35 = new TableCell();
                                tdRow35.Width = Unit.Percentage(8);
                                if (app.mothername != null)
                                    tdRow35.Text = app.mothername.ToUpper();
                                tdRow35.HorizontalAlign = HorizontalAlign.Left;
                                tdRow35.Wrap = false;
                                tr.Cells.Add(tdRow35);

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


                                TableCell tdRow45 = new TableCell();
                                tdRow45.Width = Unit.Percentage(8);
                                tdRow45.Text = "NA";
                                tdRow45.HorizontalAlign = HorizontalAlign.Left;
                                tdRow45.Wrap = false;
                                tr.Cells.Add(tdRow45);
                            }

                            TableCell tdRow7 = new TableCell();
                            tdRow7.Width = Unit.Percentage(10);
                            if (app.Dob != null)
                                tdRow7.Text = app.Dob.ToString("dd-MMM-yyyy");
                            tdRow7.HorizontalAlign = HorizontalAlign.Center;
                            tdRow7.Wrap = false;
                            tr.Cells.Add(tdRow7);


                            TableCell tdRow8 = new TableCell();
                            tdRow8.Width = Unit.Percentage(1);
                            if (app.CommencementDate != null)
                                tdRow8.Text = app.CommencementDate.ToString("MMM-yyyy");
                            tdRow8.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow8);


                            TableCell tdRow9 = new TableCell();
                            tdRow9.Width = Unit.Percentage(1);
                            if (app.Validdate != null)
                                tdRow9.Text = app.Validdate.ToString("MMM-yyyy");
                            tdRow9.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow9);

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
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }
                    //};
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
            Response.AddHeader("content-disposition", "attachment;filename=RegisteredCandidate.xls");
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