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

public partial class CandidateDetailReportPage : BasePage
{
    Table tbl = new Table();
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 applicantTypeID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
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
            Int32 DemandNoteID = Convert.ToInt32(Request.QueryString["DemandNoteId"]);
            DemandNote demandNote = context.DemandNotes.Find(DemandNoteID);
            int TypeId = Convert.ToInt32(demandNote.ApplicationTypeID);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "#";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(1);
            tcCol1.Text = "Course";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(10);
            tcCol2.Text = "Application No.";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(3);
            tcCol3.Text = "Date";
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol3);

            if (TypeId != Convert.ToInt32(enmApplicationType.CertificateExamApplication))
            {
                TableHeaderCell tcCol4 = new TableHeaderCell();
                tcCol4.Width = Unit.Percentage(8);
                tcCol4.HorizontalAlign = HorizontalAlign.Center;
                tcCol4.Text = "Registration No.";
                th.Cells.Add(tcCol4);
            }

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(12);
            tcCol5.Text = "Candidate Name";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(13);
            tcCol6.Text = "Father/Guardian Name";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(8);
            tcCol7.Text = "Fee Amount";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);

            TableHeaderCell tcCol8 = new TableHeaderCell();
            tcCol8.Width = Unit.Percentage(15);
            tcCol8.Text = "Payment status";
            tcCol8.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol8);

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
                tdRow1.Width = Unit.Percentage(8);
                tdRow1.Text = app.Course.Code.ToString();
                tdRow1.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow1);

                HyperLink link = new HyperLink();
                link.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/CertificatePreview.aspx?ID=" + app.CourseID + "&Appid=" + app.ID + "&Dob=" + app.DateOfBirth + "&Type=Print");
                link.Target = "_blank";
                link.Style.Add("text-decoration", "none");
                link.ForeColor = System.Drawing.Color.Black;
                TableCell tdRow2 = new TableCell();
                tdRow2.Width = Unit.Percentage(8);
                link.Text = app.Number.ToString();
                tdRow2.HorizontalAlign = HorizontalAlign.Left;
                tdRow2.Controls.Add(link);
                tr.Cells.Add(tdRow2);

                TableCell tdRow3 = new TableCell();
                tdRow3.Width = Unit.Percentage(8);
                if (app.ApplicationDate != null)
                    tdRow3.Text = app.ApplicationDate.ToString("dd-MMM-yyyy");
                tdRow3.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow3);

                TableCell tdRow5 = new TableCell();
                tdRow5.Width = Unit.Percentage(12);
                if (app.Name != null)
                    tdRow5.Text = app.Name.ToString();
                tdRow5.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow5);

                if (string.IsNullOrEmpty(app.GuardianName) == true && string.IsNullOrWhiteSpace(app.GuardianName) == true)
                {
                    TableCell tdRow6 = new TableCell();
                    tdRow6.Width = Unit.Percentage(13);
                    if (app.FatherName != null)
                        tdRow6.Text = app.FatherName.ToString();
                    tdRow6.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow6);
                }
                else
                {
                    TableCell tdRow6 = new TableCell();
                    tdRow6.Width = Unit.Percentage(13);
                    if (app.GuardianName != null)
                        tdRow6.Text = app.GuardianName.ToString();
                    tdRow6.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow6);
                }

                TableCell tdRow7 = new TableCell();
                tdRow7.Width = Unit.Percentage(8);
                if (app.DemandNote.DemandNoteTypeID == Convert.ToInt32(enmDemandNoteType.Single))
                {
                    if (app.DemandNote.Amount != null)
                        tdRow7.Text = app.DemandNote.Amount.ToString("F");
                }
                else if (app.DemandNote.DemandNoteTypeID == Convert.ToInt32(enmDemandNoteType.Multiple))
                {
                    if (app.TotalFeeAmount.HasValue)
                        tdRow7.Text = app.TotalFeeAmount.Value.ToString("F");
                }
                tdRow7.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow7);

                Int32 PaymentStatusPending = Convert.ToInt32(enmPaymentStatus.Pending);
                TableCell tdRow8 = new TableCell();
                tdRow8.Width = Unit.Percentage(15);
                if (app.DemandNote.PaymentStatusID == PaymentStatusPending)
                    tdRow8.Text = "Not Paid";
                else
                {
                    if (app.DemandNote.DDTransactionID != null)
                        tdRow8.Text = app.DemandNote.PaymentMode.Name + "-" + app.DemandNote.DemandDraftTransaction.DemandDraftNumber.ToString();
                    else if (app.DemandNote.CSCTransaction_ID != null)
                        tdRow8.Text = app.DemandNote.PaymentMode.Name + "-" + app.DemandNote.CSCTransaction.ResponseTransactionNumber.ToString();
                    else if (app.DemandNote.OnlineTransactionID != null)
                        tdRow8.Text = app.DemandNote.PaymentMode.Name + "-" + app.DemandNote.OnlineTransaction.ReferenceNumber.ToString();
                    else if (app.DemandNote.NEFTTransactionID != null)
                        tdRow8.Text = app.DemandNote.PaymentMode.Name + "-" + app.DemandNote.NEFTTransaction.TransactionNumber.ToString();
                }
                tdRow8.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow8);

                tbl.Rows.Add(tr);
                i++;
            }
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
            using (EConnectContext context = new EConnectContext())
            {
                foreach (CourseRegistrationApplication app in application)
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
                    tdRow1.Width = Unit.Percentage(8);
                    tdRow1.Text = app.Course.Name.ToString();
                    tdRow1.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow1);

                    HyperLink link = new HyperLink();
                    link.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/FrmPreview.aspx?ID=" + app.CourseID + "&Appid=" + app.ID + "&Dob=" + app.DateOfBirth + "&Type=Print");
                    link.Target = "_blank";
                    link.Style.Add("text-decoration", "none");
                    link.ForeColor = System.Drawing.Color.Black;
                    TableCell tdRow2 = new TableCell();
                    tdRow2.Width = Unit.Percentage(8);
                    link.Text = app.Number.ToString();
                    tdRow2.HorizontalAlign = HorizontalAlign.Left;
                    tdRow2.Controls.Add(link);
                    tr.Cells.Add(tdRow2);

                    TableCell tdRow3 = new TableCell();
                    tdRow3.Width = Unit.Percentage(8);
                    if (app.ApplicationDate != null)
                        tdRow3.Text = app.ApplicationDate.ToString("dd-MMM-yyyy");
                    tdRow3.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow3);

                    TableCell tdRow4 = new TableCell();
                    tdRow4.Width = Unit.Percentage(12);
                    if (app.RegisteredCourseRegistrationNo != null)
                        tdRow4.Text = app.RegisteredCourseRegistrationNo.ToString();
                    else
                        tdRow4.Text = "NA";
                    tdRow4.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow4);

                    TableCell tdRow5 = new TableCell();
                    tdRow5.Width = Unit.Percentage(12);
                    if (app.Name != null)
                        tdRow5.Text = app.Name.ToString();
                    tdRow5.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow5);

                    if (string.IsNullOrEmpty(app.GuardianName) == true && string.IsNullOrWhiteSpace(app.GuardianName) == true)
                    {
                        TableCell tdRow6 = new TableCell();
                        tdRow6.Width = Unit.Percentage(13);
                        if (app.FatherName != null)
                            tdRow6.Text = app.FatherName.ToString();
                        tdRow6.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(tdRow6);
                    }
                    else
                    {
                        TableCell tdRow6 = new TableCell();
                        tdRow6.Width = Unit.Percentage(12);
                        if (app.GuardianName != null)
                            tdRow6.Text = app.GuardianName.ToString();
                        tdRow6.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(tdRow6);
                    }

                    TableCell tdRow7 = new TableCell();
                    tdRow7.Width = Unit.Percentage(8);
                    if (app.DemandNote.DemandNoteTypeID == Convert.ToInt32(enmDemandNoteType.Single))
                    {
                        if (app.DemandNote.Amount != null)
                            tdRow7.Text = app.DemandNote.Amount.ToString("F");
                    }
                    else if (app.DemandNote.DemandNoteTypeID == Convert.ToInt32(enmDemandNoteType.Multiple))
                    {
                        if (app.FeeAmount.HasValue)
                            tdRow7.Text = app.FeeAmount.Value.ToString("F");
                    }
                    tdRow7.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow7);



                    Int32 PaymentStatusPending = Convert.ToInt32(enmPaymentStatus.Pending);
                    TableCell tdRow8 = new TableCell();
                    tdRow8.Width = Unit.Percentage(15);
                    if (app.DemandNote.enmPaymentStatus == enmPaymentStatus.Paid || app.DemandNote.enmPaymentStatus == enmPaymentStatus.PaidButNotVerified)
                    {
                        if (app.DemandNote.enmPaymentMode == enmPaymentMode.DemandDraft)
                            tdRow8.Text = app.DemandNote.PaymentMode.Name + "-" + app.DemandNote.DemandDraftTransaction.DemandDraftNumber.ToString();
                        else if (app.DemandNote.enmPaymentMode == enmPaymentMode.CSCSPV)
                            tdRow8.Text = app.DemandNote.PaymentMode.Name + "-" + app.DemandNote.CSCTransaction.ResponseTransactionNumber.ToString();
                        else if (app.DemandNote.enmPaymentMode == enmPaymentMode.Online)
                            tdRow8.Text = app.DemandNote.PaymentMode.Name + "-" + app.DemandNote.OnlineTransaction.ReferenceNumber.ToString();
                        else if (app.DemandNote.enmPaymentMode == enmPaymentMode.NEFTRTGS)
                            tdRow8.Text = app.DemandNote.PaymentMode.Name + "-" + app.DemandNote.NEFTTransaction.TransactionNumber.ToString();
                    }
                    else
                    {
                        tdRow8.Text = "Not Paid";
                    }
                    tdRow8.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow8);

                    tbl.Rows.Add(tr);
                    i++;
                }
            }
            ;
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
            int i = 1;
            foreach (CourseExamApplication app in application)
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
                tdRow1.Width = Unit.Percentage(8);
                tdRow1.Text = app.Course.Name.ToString();
                tdRow1.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow1);

                HyperLink link = new HyperLink();
                link.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/ExamFormPreview.aspx?ID=" + app.CourseID + "&Appid=" + app.ID + "&Dob=" + app.Candidate.DateOfBirth + "&candidateID=" + app.CandidateID + "&Type=Print");
                link.Target = "_blank";
                link.Style.Add("text-decoration", "none");
                link.ForeColor = System.Drawing.Color.Black;
                TableCell tdRow2 = new TableCell();
                tdRow2.Width = Unit.Percentage(8);
                link.Text = app.Number.ToString();
                tdRow2.Controls.Add(link);
                tdRow2.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow2);

                TableCell tdRow3 = new TableCell();
                tdRow3.Width = Unit.Percentage(8);
                if (app.ApplicationDate != null)
                    tdRow3.Text = app.ApplicationDate.ToString("dd-MMM-yyyy");
                tdRow3.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow3);

                TableCell tdRow4 = new TableCell();
                tdRow4.Width = Unit.Percentage(12);
                if (app.RegistrationNumber != null)
                    tdRow4.Text = app.RegistrationNumber.ToString();
                tdRow4.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow4);

                TableCell tdRow5 = new TableCell();
                tdRow5.Width = Unit.Percentage(12);
                if (app.Candidate.Name != null)
                    tdRow5.Text = app.Candidate.Name.ToString();
                tdRow5.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow5);
                if (string.IsNullOrEmpty(app.Candidate.GuardianName) == true && string.IsNullOrWhiteSpace(app.Candidate.GuardianName) == true)
                {
                    TableCell tdRow6 = new TableCell();
                    tdRow6.Width = Unit.Percentage(13);
                    if (app.Candidate.FatherName != null)
                        tdRow6.Text = app.Candidate.FatherName.ToString();
                    tdRow6.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow6);
                }
                else
                {
                    TableCell tdRow6 = new TableCell();
                    tdRow6.Width = Unit.Percentage(13);
                    if (app.Candidate.GuardianName != null)
                        tdRow6.Text = app.Candidate.GuardianName.ToString();
                    tdRow6.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow6);
                }

                TableCell tdRow7 = new TableCell();
                tdRow7.Width = Unit.Percentage(8);
                if (app.DemandNote.DemandNoteTypeID == Convert.ToInt32(enmDemandNoteType.Single))
                {
                    if (app.DemandNote.Amount != null)
                        tdRow7.Text = app.DemandNote.Amount.ToString("F");
                }
                else if (app.DemandNote.DemandNoteTypeID == Convert.ToInt32(enmDemandNoteType.Multiple))
                {
                    if (app.FeeAmount != null)
                        tdRow7.Text = app.FeeAmount.ToString("F");
                }

                tdRow7.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow7);

                Int32 PaymentStatusPending = Convert.ToInt32(enmPaymentStatus.Pending);
                TableCell tdRow8 = new TableCell();
                tdRow8.Width = Unit.Percentage(15);
                if (app.DemandNote.PaymentStatusID == PaymentStatusPending)
                    tdRow8.Text = "Not Paid";
                else
                {
                    if (app.DemandNote.DDTransactionID != null)
                        tdRow8.Text = app.DemandNote.PaymentMode.Name + "-" + app.DemandNote.DemandDraftTransaction.DemandDraftNumber.ToString();
                    else if (app.DemandNote.CSCTransaction_ID != null)
                        tdRow8.Text = app.DemandNote.PaymentMode.Name + "-" + app.DemandNote.CSCTransaction.ResponseTransactionNumber.ToString();
                    else if (app.DemandNote.OnlineTransactionID != null)
                        tdRow8.Text = app.DemandNote.PaymentMode.Name + "-" + app.DemandNote.OnlineTransaction.ReferenceNumber.ToString();
                    else if (app.DemandNote.NEFTTransactionID != null)
                        tdRow8.Text = app.DemandNote.PaymentMode.Name + "-" + app.DemandNote.NEFTTransaction.TransactionNumber.ToString();
                }
                tdRow8.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow8);

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
            context = new EConnectContext();
            Int32 DemandNoteID = 0;
            String[] DemandNoteIDs = new String[] { };
            if (Request.QueryString["DemandNoteId"].ToString().Contains(","))
            {
                DemandNoteIDs = Request.QueryString["DemandNoteId"].ToString().Split(',');
            }
            else
                DemandNoteID = Convert.ToInt32(Request.QueryString["DemandNoteId"]);

            DemandNote demandNote = context.DemandNotes.Find(DemandNoteID);
            int TypeId = Convert.ToInt32(demandNote.ApplicationTypeID);
            string strHead = "";
            if (demandNote != null)
            {
                strHead += "</br> <b>Demand Note Number :</b> " + demandNote.ID.ToString();
                strHead += ", <b>Demand Note Date :</b> " + demandNote.ApplicationDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "No Record Found";
            }
            if (demandNote != null)
            {
                #region------Certificate-Exam-Application----------
                if (TypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    var application = (from a in context.CertificateExamApplications
                                       where a.DemandNoteID == demandNote.ID
                                       select a).ToList();
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
                        }
                        application = application.Where(s => s.RegionalCenterID == entityID).ToList();
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
                        }
                        application = application.Where(s => s.RegionalCenterID == entityID).ToList();
                    }
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
                #endregion--------------

                #region-------Course-Registration-Application
                else if (TypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    var application = (from a in context.CourseRegistrationApplications
                                       where a.DemandNoteID == demandNote.ID
                                       select a).ToList();
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
                        }
                    }
                    if (application.Count() > 0)
                    {
                        ShowCourseRegistrationData(application);
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }
                }
                #endregion-----------

                #region--------Course-Exam-Application-----
                else if (TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    var application = (from a in context.CourseExamApplications
                                       where a.DemandNoteID == demandNote.ID
                                       select a).ToList();
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
                        }
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
                #endregion----------------------
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
            Response.AddHeader("content-disposition", "attachment;filename=CandidateDetail.xls");
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

                result = result.Where(p => p.courseID == CourseId);
                if (System.IO.File.Exists(Access))
                    System.IO.File.Delete(Access);
                System.IO.File.Copy(Server.MapPath("~/Download/Database_format_BCC.mdb"), Access);
                string connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Access + ";Persist Security Info=False;";
                connection.ConnectionString = connect;
                connection.Open();
                // command = new OleDbCommand("delete from  [MS Access;Database=" + Access + "].[Exam_Database]", connection);
                command = new OleDbCommand("delete from  [MS Access;Database=@Access].[Exam_Database]", connection);
                command.Parameters.AddWithValue("@Access", Access);
                command.ExecuteNonQuery();
                foreach (var certificate in result)
                {
                    command = new OleDbCommand("insert into [MS Access;Database=@Access].[Exam_Database](Folder_no, batch_no,sr_no,exam_year,NAME,F_NAME,M_NAME,D_O_B,SEX,H_Qual,ADD1,ADD2,ADD3,CITY,STATE,PINCODE,PH_NO_C,EMAIL_C,CCC_NO,INST_NAME,INST_ADD,INST_STAT,TH_CENT_CH,SEC_TH_CEN,OCCUPATION,CATEGORY,PREV_APP,PREV_M,PREV_YEAR,PREV_ROLL,Rollno,cent_allot,cent_add,examDate,batch,rep_time)values('" + certificate.Folder_no + "','" + certificate.batch_no + "','" + certificate.sr_no + "','" + certificate.exam_year + "','" + certificate.NAME + "','" + certificate.F_NAME + "','" + certificate.M_NAME + "','" + certificate.D_O_B + "','" + certificate.SEX + "','" + certificate.H_Qual + "','" + certificate.ADD1 + "','" + certificate.ADD2 + "','" + certificate.ADD3 + "','" + certificate.CITY + "','" + certificate.STATE + "','" + certificate.PINCODE + "','" + certificate.PH_NO_C + "','" + certificate.EMAIL_C + "','" + certificate.CCC_NO + "','" + certificate.INST_NAME + "','" + certificate.INST_ADD + "','" + certificate.INST_STAT + "','" + certificate.TH_CENT_CH + "','" + certificate.SEC_TH_CEN + "','" + certificate.OCCUPATION + "','" + certificate.CATEGORY + "','" + certificate.PREV_APP + "','" + certificate.PREV_M + "','" + certificate.PREV_YEAR + "','" + certificate.PREV_ROLL + "','" + certificate.Rollno + "','" + certificate.cent_allot + "','" + certificate.cent_add + "','" + certificate.examDate + "','" + certificate.batch + "','" + certificate.rep_time + "')", connection);
                    command.Parameters.AddWithValue("@Access", Access);
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
                cmdAccess.CommandText = "select sr_no,Rollno,cent_allot,cent_add,batch,rep_time,examDate from @SheetName";
                cmdAccess.Parameters.AddWithValue("@SheetName", SheetName);
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
                                //String sql = "select Roll_Number from Certificate_Exam_Application where ID='" + dtrow["sr_no"].ToString() + "'";
                                String sql = "select Roll_Number from Certificate_Exam_Application where ID='@ID'";
                                SqlParameter[] p = {
                                                       new SqlParameter("@ID", dtrow["sr_no"].ToString())
                                                   };
                                DataTable dtTbl = DbUtility.GetDataTable(sql, new EConnect.Connections.SqlCon(), p, CommandType.Text, false);
                                if (dtTbl.Rows[0][0] is DBNull)
                                {
                                    SqlParameter[] p1 = {
                                                       new SqlParameter("@Rollno", Convert.ToInt64((dtrow["Rollno"].ToString().Trim()))),
                                                       new SqlParameter("@CentName", dtrow["cent_allot"].ToString().Trim()),
                                                       new SqlParameter("@CentAddr", dtrow["cent_add"].ToString().Trim()),
                                                       new SqlParameter("@ExamBatch", dtrow["batch"].ToString().Trim()),
                                                       new SqlParameter("@ReportTime",dtrow["rep_time"].ToString().Trim()),
                                                       new SqlParameter("@ApplStatus", Convert.ToInt32(enmCertificateExamApplicationStatus.ExamCentreAndRollNumberAlloted)),
                                                       new SqlParameter("@UpdatedOn", Convert.ToDateTime(DateTime.Now)),
                                                       new SqlParameter("@Examdate", Convert.ToDateTime(dtrow["examDate"].ToString())),
                                                       new SqlParameter("@UpdatedBy",  Convert.ToInt32(Session["UserID"])),
                                                       new SqlParameter("@ID",   dtrow["sr_no"].ToString())
                                                   };
                                    context.Database.ExecuteSqlCommand("update Certificate_Exam_Application set  " +
                                                                      "Roll_Number =@Rollno," +
                                                                      "Exam_Centre_Name='@CentName'," +
                                                                      "Exam_Centre_Address='@CentAddr'," +
                                                                      "Exam_Batch_Number='@ExamBatch'," +
                                                                      "Reporting_Time='@ReportTime'," +
                                                                      "Application_Status_ID= @ApplStatus," +
                                                                      "Updated_On=@UpdatedOn," +
                                                                      "Date_of_Exam=@Examdate," +
                                                                      "Updated_By=@UpdatedBy where ID='@ID'");

                                    /* context.Database.ExecuteSqlCommand("update Certificate_Exam_Application set  " +
                                                                        "Roll_Number =" + Convert.ToInt64((dtrow["Rollno"].ToString().Trim())) + "," +
                                                                        "Exam_Centre_Name='" + dtrow["cent_allot"].ToString().Trim() + "'," +
                                                                        "Exam_Centre_Address='" + dtrow["cent_add"].ToString().Trim() + "'," +
                                                                        "Exam_Batch_Number='" + dtrow["batch"].ToString().Trim() + "'," +
                                                                        "Reporting_Time='" + dtrow["rep_time"].ToString().Trim() + "'," +
                                                                        "Application_Status_ID= " + Convert.ToInt32(enmCertificateExamApplicationStatus.ExamCentreAndRollNumberAlloted) + "," +
                                                                        "Updated_On='" + Convert.ToDateTime(DateTime.Now) + "'," +
                                                                        "Date_of_Exam='" + Convert.ToDateTime(dtrow["examDate"].ToString()) + "'," +
                                                                        "Updated_By=" + Convert.ToInt32(Session["UserID"]) + " where ID='" + dtrow["sr_no"].ToString() + "'");
                                     */
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
                    }
                    ;
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