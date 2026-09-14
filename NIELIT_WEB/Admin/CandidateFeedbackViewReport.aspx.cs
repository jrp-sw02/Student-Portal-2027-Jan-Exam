using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data.OleDb;
using System.Data.Objects;
public partial class CandidateFeedbackViewReport : BasePage
{
    Table tbl = new Table();
    EConnectContext context;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            if (String.IsNullOrEmpty(Request.QueryString["UserTypeID"]) && String.IsNullOrEmpty(Request.QueryString["Datefrom"]) && String.IsNullOrEmpty(Request.QueryString["Dateto"]))
                GeInvalidRequestMessage("Go to home page", "../mainpage.aspx");
            if (!Page.IsPostBack)
            {
                tbl.CssClass = "sample3";
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(98);
                tbl.HorizontalAlign = HorizontalAlign.Center;
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
        TableHeaderRow th = new TableHeaderRow();
        th.CssClass = "head1";

        TableHeaderCell tcCol0 = new TableHeaderCell();
        tcCol0.Width = Unit.Percentage(2);
        tcCol0.Text = "#";
        tcCol0.HorizontalAlign = HorizontalAlign.Center;
        th.Cells.Add(tcCol0);

        TableHeaderCell tcCol1 = new TableHeaderCell();
        tcCol1.Width = Unit.Percentage(10);
        tcCol1.Text = "Date";
        tcCol1.HorizontalAlign = HorizontalAlign.Center;
        th.Cells.Add(tcCol1);

        TableHeaderCell tcCol2 = new TableHeaderCell();
        tcCol2.Width = Unit.Percentage(13);
        tcCol2.Text = "Registration Detail";
        tcCol2.HorizontalAlign = HorizontalAlign.Center;
        th.Cells.Add(tcCol2);

        TableHeaderCell tcCol3 = new TableHeaderCell();
        tcCol3.Width = Unit.Percentage(20);
        tcCol3.Text = "User Name";
        tcCol3.HorizontalAlign = HorizontalAlign.Center;
        th.Cells.Add(tcCol3);

        TableHeaderCell tcCol4 = new TableHeaderCell();
        tcCol4.Width = Unit.Percentage(35);
        tcCol4.HorizontalAlign = HorizontalAlign.Center;
        tcCol4.Text = "Feedback Received";
        th.Cells.Add(tcCol4);

        TableHeaderCell tcCol5 = new TableHeaderCell();
        tcCol5.Width = Unit.Percentage(30);
        tcCol5.Text = "Suggestion Reveived";
        tcCol5.HorizontalAlign = HorizontalAlign.Center;
        th.Cells.Add(tcCol5);
        tbl.Rows.Add(th);

    }
    protected void ShowCandidateFeebBack(IEnumerable<Feedback> feedbacklist)
    {
        try
        {
            context = new EConnectContext();
            int i = 0;
            foreach (Feedback fb in feedbacklist)
            {
                TableRow tr = new TableRow();
                if (i % 2 == 0)
                    tr.CssClass = "gdalternate1";
                else
                    tr.CssClass = "gdrow1";
                TableCell tdRow0 = new TableCell();
                tdRow0.Width = Unit.Percentage(2);
                tdRow0.Text = (i + 1).ToString();
                tdRow0.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow0);

                TableCell tdRow1 = new TableCell();
                tdRow1.Width = Unit.Percentage(10);
                tdRow1.Text = fb.Date.ToString("dd-MMM-yyyy");
                tdRow1.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow1);

                var registration = (from c in context.RegistrationDetails
                                    where c.CandidateID == fb.UserID
                                    orderby c.CommencementFromDate descending
                                    select c).FirstOrDefault();

                TableCell tdRow2 = new TableCell();
                tdRow2.Width = Unit.Percentage(13);
                if (registration != null)
                    tdRow2.Text = registration.Course.Name + "-" + registration.RegistrationNo;
                tdRow2.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow2);

                TableCell tdRow3 = new TableCell();
                tdRow3.Width = Unit.Percentage(20);
                var uname = (from u in context.Users
                             where u.UserRefNumber == fb.UserID
                             select u.UserName).FirstOrDefault();
                if (uname != null)
                    tdRow3.Text = GetInitCap(uname.ToString());
                else
                    tdRow3.Text = "NA";
                tdRow3.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow3);

                //TableCell tdRow1 = new TableCell();
                //tdRow1.Width = Unit.Percentage(5);
                //tdRow1.Text = exam.ExamFomDate.ToString("dd-MMM-yyyy");
                //if (exam.ExamToDates.HasValue)
                //{
                //    if (exam.ExamToDates.Value != exam.ExamFomDate)
                //        tdRow1.Text += "<br>To<br>" + exam.ExamToDates.Value.ToString("dd-MMM-yyyy");
                //}
                //tdRow1.HorizontalAlign = HorizontalAlign.Center;
                //tr.Cells.Add(tdRow1);



                TableCell tdRow4 = new TableCell();
                tdRow4.Width = Unit.Percentage(35);
                if (fb.FeedbackDescription != null)
                    tdRow4.Text = fb.FeedbackDescription;
                tdRow4.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow4);

                TableCell tdRow5 = new TableCell();
                tdRow5.Width = Unit.Percentage(30);
                if (fb.Suggestions != null)
                    tdRow5.Text = fb.Suggestions.ToString();
                tdRow5.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow5);
                tbl.Rows.Add(tr);
                i++;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
    protected void ShowData()
    {
        try
        {
            Int32 UserTypeID = Convert.ToInt32(Request.QueryString["UserTypeID"]);
            DateTime DateFrom = Convert.ToDateTime(Request.QueryString["Datefrom"]);
            DateTime DateTo = Convert.ToDateTime(Request.QueryString["DateTo"]);

            if (UserTypeID != 0 && DateFrom != null && DateTo !=null )
            {

                lblUserType.Text = EConnect.Utils.Common.EnumUtility.GetDescription((UserType)(UserTypeID));
                lblSessions.Text = "";
                lblSessions.Text += "<br/><b>Date</b>:-" + DateFrom.ToString("dd-MMM-yyyy") + "<b> to </b>" + DateTo.ToString("dd-MMM-yyyy");
                using (EConnectContext context = new EConnectContext())
                {
                    ShowTableHeader();
                    var feedbacks = (from s in context.Feedbacks
                                    where System.Data.Entity.DbFunctions.TruncateTime(s.Date) >= System.Data.Entity.DbFunctions.TruncateTime(DateFrom) && System.Data.Entity.DbFunctions.TruncateTime(s.Date) <= System.Data.Entity.DbFunctions.TruncateTime(DateTo)
                                    && s.UserTypeID == UserTypeID 
                                    orderby s.Date
                                   select s).ToList();
                    ShowCandidateFeebBack(feedbacks);
                }; 
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
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
    protected void imgAccess_Click(object sender, ImageClickEventArgs e)
    {

        OleDbConnection connection = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();
        int CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
        //RC-CHA_Apllications_CCC_Jan2013.mdb
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Course currentCourse = context.Courses.Find(CourseId);
                if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                {
                    var result = (from c in context.CertificateExamApplications
                                  select new
                                  {
                                      courseid = c.CourseID,
                                      exam_year = c.Exam.Name,
                                      sr_no = c.ID,
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
                                      PH_NO_C = c.PhoneNumber.Value,
                                      EMAIL_C = c.EmailAddress,
                                      CCC_NO = c.InstituteID.Value,
                                      INST_NAME = c.Institute.Name,
                                      INST_ADD = c.Institute.AddressLine1 + "  " + c.Institute.AddressLine2 + "   " + c.Institute.State.Name,
                                      INST_STAT = c.Institute.AccreditationDetails.FirstOrDefault().AccreditationStatus.Code,
                                      TH_CENT_CH = c.ExamCenter1.Code,
                                      SEC_TH_CEN = c.ExamCenter2.Code,
                                      OCCUPATION = c.Occupation.Name,
                                      CATEGORY = c.CastCategory.Code,
                                      PREV_APP = (c.AlreadyApplied == true) ? "Y" : "N",
                                      PREV_M = c.PreviousExam.Name,
                                      PREV_YEAR = "",//need to work on this
                                      PREV_ROLL = (c.PreviousRollNumber.Trim() != "" && c.PreviousRollNumber != null) ? c.PreviousRollNumber.ToUpper() : "NA",
                                      Rollno = "",
                                      cent_allot = "",
                                      cent_add = "",
                                      examDate = c.Exam.ExamStartDate,
                                      batch = "",
                                      rep_time = ""
                                  });
                    result = result.Where(q => q.courseid == CourseId);
                    var certificate = result.FirstOrDefault();//need to use tolist here
                    string Access = Server.MapPath("../../Download/Database_formatCCC_EMEC.mdb");
                    string connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Access + ";Persist Security Info=False";
                    connection.ConnectionString = connect;
                    connection.Open();
                    command = new OleDbCommand("insert into [MS Access;Database=" + Access + "].[Exam_Database](sr_no,exam_year,NAME,F_NAME,M_NAME,D_O_B,SEX,H_Qual,ADD1,ADD2,ADD3,CITY,STATE,PINCODE,PH_NO_C,EMAIL_C,CCC_NO,INST_NAME,INST_ADD,INST_STAT,TH_CENT_CH,SEC_TH_CEN,OCCUPATION,CATEGORY,PREV_APP,PREV_M,PREV_YEAR,PREV_ROLL,Rollno,cent_allot,cent_add,examDate,batch,rep_time) values('" + certificate.sr_no + "','" + certificate.exam_year + "','" + certificate.NAME + "','" + certificate.F_NAME + "','" + certificate.M_NAME + "','" + certificate.D_O_B + "','" + certificate.SEX + "','" + certificate.H_Qual + "','" + certificate.ADD1 + "','" + certificate.ADD2 + "','" + certificate.ADD3 + "','" + certificate.CITY + "','" + certificate.STATE + "','" + certificate.PINCODE + "','" + certificate.PH_NO_C + "','" + certificate.EMAIL_C + "','" + certificate.CCC_NO + "','" + certificate.INST_NAME + "','" + certificate.INST_ADD + "','" + certificate.INST_STAT + "','" + certificate.TH_CENT_CH + "','" + certificate.SEC_TH_CEN + "','" + certificate.OCCUPATION + "','" + certificate.CATEGORY + "','" + certificate.PREV_APP + "','" + certificate.PREV_M + "','" + certificate.PREV_YEAR + "','" + certificate.PREV_ROLL + "','" + certificate.Rollno + "','" + certificate.cent_allot + "','" + certificate.cent_add + "','" + certificate.examDate + "','" + certificate.batch + "','" + certificate.rep_time + "')", connection);
                    command.ExecuteNonQuery();
                    // Response.Clear();
                    // Response.AddHeader("content-disposition", "attachment;filename=Applications.mdb");
                    // Response.Charset = "";
                    // Response.ContentType = "application/vnd.mdb";
                    //Response.TransmitFile(Access);
                    //  Response.TransmitFile("../../Download/Database_formatCCC_EMEC.mdb");
                    //  Response.End();
                }
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);

        }
        finally
        {
            command.Dispose();
            connection.Close();
            connection.Dispose();
            //context.Database.ExecuteSqlCommand("insert into [MS Access;Database=" + Access + "].[Temp] values('" + certificate + "')");
            //context.Database.ExecuteSqlCommand("SELECT * INTO [MS Access;Database=" + Access + "].[Temp][ C_Name] FROM ['" + certificate.C_Name + "'] ");
        }
    }
}