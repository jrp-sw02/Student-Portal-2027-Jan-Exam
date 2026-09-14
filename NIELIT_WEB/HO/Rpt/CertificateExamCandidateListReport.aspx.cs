using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class CAND_CertificateExamCandidateListReport : BasePage
{
    Table tbl = new Table();
    //EConnectContext context = new EConnectContext();
    //Int32 StatusId = 0;
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

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(1);
            tcCol1.Text = "#";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(14);
            tcCol2.Text = "Application Number";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);


            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(17);
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Text = "Name of the Candidate";
            th.Cells.Add(tcCol3);


            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(13);
            tcCol4.Text = "Mother's Name";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(18);
            tcCol5.Text = "Father's/Guardian Name";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);


            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(9);
            tcCol6.Text = "Date of Birth";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(5);
            tcCol7.Text = "Sex";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);

            TableHeaderCell tcCol8 = new TableHeaderCell();
            tcCol8.Width = Unit.Percentage(23);
            tcCol8.Text = "Sign of the Candidate";
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
                //tdRow.Width = Unit.Percentage(1);
                tdRow.Text = i.ToString();
                tdRow.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(tdRow);

                HyperLink link = new HyperLink();
                link.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/CertificatePreview.aspx?ID=" + app.CourseID + "&Appid=" + app.ID + "&Dob=" + app.DateOfBirth + "&Type=Print");
                link.Target = "_blank";
                link.Style.Add("text-decoration", "none");
                link.ForeColor = System.Drawing.Color.Black;
                TableCell tdRow1 = new TableCell();
                //tdRow1.Width = Unit.Percentage(8);
                link.Text = app.Number.ToString();
                tdRow1.HorizontalAlign = HorizontalAlign.Left;
                tdRow1.Controls.Add(link);
                tr.Cells.Add(tdRow1);
                //app.Course.Code.ToString();

                TableCell tdRow2 = new TableCell();
                //tdRow2.Width = Unit.Percentage(8);
                if (app.Name != null)
                    tdRow2.Text = app.Name.ToString();
                tdRow2.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow2);

                TableCell tdRow3 = new TableCell();
                //tdRow3.Width = Unit.Percentage(8);
                if (app.MotherName != null)
                    tdRow3.Text = app.MotherName;
                tdRow3.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow3);

                if (string.IsNullOrEmpty(app.GuardianName) == true && string.IsNullOrWhiteSpace(app.GuardianName) == true)
                {
                    TableCell tdRow4 = new TableCell();
                    //tdRow4.Width = Unit.Percentage(13);
                    if (app.FatherName != null)
                        tdRow4.Text = app.FatherName.ToString();
                    tdRow4.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow4);
                }
                else
                {
                    TableCell tdRow4 = new TableCell();
                    //tdRow4.Width = Unit.Percentage(13);
                    if (app.GuardianName != null)
                        tdRow4.Text = app.GuardianName.ToString();
                    tdRow4.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow4);
                }

                TableCell tdRow5 = new TableCell();
                //tdRow5.Width = Unit.Percentage(12);
                if (app.DateOfBirth != null)
                    tdRow5.Text = app.DateOfBirth.ToString("dd-MMM-yyyy");
                tdRow5.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow5);

                TableCell tdRow6 = new TableCell();
                //tdRow6.Width = Unit.Percentage(13);
                if (app.Gender != null)
                    tdRow6.Text = app.Gender.ToString();
                tdRow6.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow6);

                TableCell tdRow7 = new TableCell();
                //tdRow7.Width = Unit.Percentage(13);
                tdRow7.Text = "";
                tdRow7.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow7);
                lblHeader.Text = "Candidate Details Report for the Examination " + app.Exam.Name;
                LblRptSubHeader.Text = "<font size='4'><strong><b><u>Candidate's Details for the " + app.Course.Code + " Examination</u></b></strong></font><br/><br/>(To be sent along with the Examination Application forms of " + app.Course.Code + ")";
                lblInstituteName.Text = app.Institute.Name.ToString();
                lblBccnoLable.Text = app.Course.Code + "_NO";
                lblBccno.Text = app.Institute.AccreditationDetails.Where(s => s.CourseID == app.CourseID).Select(t => t.AccreditationNumber).FirstOrDefault();

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
                        //};
                    }
                }
            };

        }          //LblRptSubHeader.Text = strHead;

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