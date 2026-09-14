using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Web;

public partial class Admin_ExamTimeTableReport : BasePage
{
    Table tbl = new Table();
    //EConnectContext context = new EConnectContext();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(Request.QueryString["ExamId"]) && String.IsNullOrEmpty(Request.QueryString["CourseId"]))
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
        TableHeaderCell tcCol1 = new TableHeaderCell();
        tcCol1.Width = Unit.Percentage(10);
        tcCol1.Text = "Day(s)";
        tcCol1.HorizontalAlign = HorizontalAlign.Center;
        th.Cells.Add(tcCol1);


        TableHeaderCell tcCol2 = new TableHeaderCell();
        tcCol2.Width = Unit.Percentage(10);
        tcCol2.Text = "Exam Session";
        tcCol2.HorizontalAlign = HorizontalAlign.Center;
        th.Cells.Add(tcCol2);

        TableHeaderCell tcCol3 = new TableHeaderCell();
        tcCol3.Width = Unit.Percentage(10);
        tcCol3.HorizontalAlign = HorizontalAlign.Center;
        tcCol3.Text = "Module Code";
        th.Cells.Add(tcCol3);

        TableHeaderCell tcCol4 = new TableHeaderCell();
        tcCol4.Width = Unit.Percentage(70);
        tcCol4.Text = "Module Name";
        tcCol4.HorizontalAlign = HorizontalAlign.Center;
        th.Cells.Add(tcCol4);
        tbl.Rows.Add(th);

    }
    protected void ShowTimeTable(IEnumerable<ExamTimeTable> ExamTimeTableList)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                int i = 0;
                int practical = Convert.ToInt32(enmModuleType.Practical);
                foreach (ExamTimeTable exam in ExamTimeTableList)
                {
                    int revisionNumber_practical = context.Modules.Where(s => s.ID == exam.ModuleID && s.ModuleTypeID == practical).Select(s => s.RevisionNumber).Distinct().FirstOrDefault();
                    TableRow tr = new TableRow();
                    if (i % 2 == 0)
                        tr.CssClass = "gdalternate1";
                    else
                        tr.CssClass = "gdrow1";
                    TableCell tdRow1 = new TableCell();
                    tdRow1.Width = Unit.Percentage(5);
                    tdRow1.Text = exam.ExamFomDate.ToString("dd-MMM-yyyy");
                    if (exam.ExamToDates.HasValue)
                    {
                        if (exam.ExamToDates.Value != exam.ExamFomDate)
                            tdRow1.Text += "<br>To<br>" + exam.ExamToDates.Value.ToString("dd-MMM-yyyy");
                    }
                    tdRow1.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow1);

                    TableCell tdRow2 = new TableCell();
                    tdRow2.Width = Unit.Percentage(3);
                    tdRow2.Text = "-";
                    if (exam.ExamSessiionID.HasValue && exam.ExamSessiionID > 0)
                        tdRow2.Text = exam.ExamSession.Name.ToString();
                    tdRow2.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow2);

                    TableCell tdRow3 = new TableCell();
                    tdRow3.Width = Unit.Percentage(5);
                    tdRow3.Text = exam.Module.ShortName.ToString();
                    if (revisionNumber_practical != 0)
                    {
                        bool forcefullyChange = false;
                        if (revisionNumber_practical == 6)
                        {
                            revisionNumber_practical = 5;
                            forcefullyChange = true;

                        }
                        tdRow3.Text = exam.Module.ShortName.ToString() + " - " + revisionNumber_practical;
                        if (forcefullyChange)
                        {
                            revisionNumber_practical = 6;
                        }
                    }
                    tdRow3.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow3);

                    TableCell tdRow4 = new TableCell();
                    tdRow4.Width = Unit.Percentage(20);
                    tdRow4.Text = exam.Module.Name.ToString();
                    tdRow4.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow4);
                    tbl.Rows.Add(tr);
                    i++;
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowData()
    {
        try
        {
            Int32 ExamId = Convert.ToInt32(Request.QueryString["ExamId"]);
            Int32 CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            Int32 practical = Convert.ToInt32(enmModuleType.Practical);
            Int32 theory = Convert.ToInt32(enmModuleType.Theory);
            Int32 Bridge = Convert.ToInt32(enmModuleType.Bridge);
            if (ExamId != 0 && CourseId != 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    //-------------------Exam Session-----------------------------------------------------------------
                    var examSessions = context.ExamSessions.ToList();
                    lblSessions.Text = "";
                    foreach (ExamSession session in examSessions)
                    {
                        lblSessions.Text += session.Code + ": " + session.Name.ToUpper() + ": " + session.StartTime.ToShortTimeString() + " To " + session.EndTime.ToShortTimeString() + " Hrs. IST  :  ";
                    }
                    lblSessions.Text = lblSessions.Text.Trim().TrimEnd(':');


                    ShowTableHeader();
                    var theoryTimeTable = (from a in context.ExamTimeTables
                                           where a.CourseID == CourseId && a.ExamID == ExamId && a.Module.ModuleTypeID != practical
                                           orderby a.ModuleID
                                           select a).ToList().Distinct();

                    var examName = context.Exams.Where(e => e.ID == ExamId).Select(e => new { Name = e.Name, CoueseName = e.Course.Name }).FirstOrDefault();
                    lblCourse.Text = "-" + examName.Name.ToString().ToUpper() + " EXAMINATION : " + examName.CoueseName;

                    TableRow trTh = new TableRow();
                    trTh.CssClass = "sample2";
                    TableCell tcTh = new TableCell();
                    tcTh.Width = Unit.Percentage(35);
                    tcTh.ColumnSpan = 4;
                    tcTh.Font.Bold = true;
                    tcTh.Text = "Theory Papers" + "<font style='color:red'>(Exact date and time of the examination will be communicated separately)</font>";
                    trTh.Cells.Add(tcTh);
                    tbl.Rows.Add(trTh);
                    ShowTimeTable(theoryTimeTable);

                    var practicalTimeTable = (from a in context.ExamTimeTables
                                              where a.CourseID == CourseId && a.ExamID == ExamId && a.Module.ModuleTypeID == practical
                                              orderby a.ModuleID
                                              select a).ToList();

                    TableRow trPr = new TableRow();
                    trPr.CssClass = "sample2";
                    TableCell tcPr = new TableCell();
                    tcPr.Width = Unit.Percentage(35);
                    tcPr.ColumnSpan = 4;
                    tcPr.Font.Bold = true;
                    tcPr.Text = "Practical Papers" + "<font style='color:red'>(Exact date and time of the examination will be communicated separately)</font>";
                    trPr.Cells.Add(tcPr);
                    tbl.Rows.Add(trPr);
                    ShowTimeTable(practicalTimeTable);
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
		//Added for Audit
            foreach (TableRow row in tbl.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    cell.Text = HttpUtility.HtmlEncode(cell.Text); // Sanitize cell content
                }
            }
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

        OleDbConnection connection = new OleDbConnection(); ;
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
                   // command = new OleDbCommand("insert into [MS Access;Database=" + Access + "].[Exam_Database](sr_no,exam_year,NAME,F_NAME,M_NAME,D_O_B,SEX,H_Qual,ADD1,ADD2,ADD3,CITY,STATE,PINCODE,PH_NO_C,EMAIL_C,CCC_NO,INST_NAME,INST_ADD,INST_STAT,TH_CENT_CH,SEC_TH_CEN,OCCUPATION,CATEGORY,PREV_APP,PREV_M,PREV_YEAR,PREV_ROLL,Rollno,cent_allot,cent_add,examDate,batch,rep_time) values('" + certificate.sr_no + "','" + certificate.exam_year + "','" + certificate.NAME + "','" + certificate.F_NAME + "','" + certificate.M_NAME + "','" + certificate.D_O_B + "','" + certificate.SEX + "','" + certificate.H_Qual + "','" + certificate.ADD1 + "','" + certificate.ADD2 + "','" + certificate.ADD3 + "','" + certificate.CITY + "','" + certificate.STATE + "','" + certificate.PINCODE + "','" + certificate.PH_NO_C + "','" + certificate.EMAIL_C + "','" + certificate.CCC_NO + "','" + certificate.INST_NAME + "','" + certificate.INST_ADD + "','" + certificate.INST_STAT + "','" + certificate.TH_CENT_CH + "','" + certificate.SEC_TH_CEN + "','" + certificate.OCCUPATION + "','" + certificate.CATEGORY + "','" + certificate.PREV_APP + "','" + certificate.PREV_M + "','" + certificate.PREV_YEAR + "','" + certificate.PREV_ROLL + "','" + certificate.Rollno + "','" + certificate.cent_allot + "','" + certificate.cent_add + "','" + certificate.examDate + "','" + certificate.batch + "','" + certificate.rep_time + "')", connection);
                    //December_2024
                    command = new OleDbCommand("insert into [Exam_Database](sr_no,exam_year,NAME,F_NAME,M_NAME,D_O_B,SEX,H_Qual,ADD1,ADD2,ADD3,CITY,STATE,PINCODE,PH_NO_C,EMAIL_C,CCC_NO,INST_NAME,INST_ADD,INST_STAT,TH_CENT_CH,SEC_TH_CEN,OCCUPATION,CATEGORY,PREV_APP,PREV_M,PREV_YEAR,PREV_ROLL,Rollno,cent_allot,cent_add,examDate,batch,rep_time) values(@sr_no,@exam_year,@NAME,@F_NAME,@M_NAME,@D_O_B,@SEX,@H_Qual,@ADD1,@ADD2,@ADD3,@CITY,@STATE,@PINCODE,@PH_NO_C,@EMAIL_C,@CCC_NO,@INST_NAME,@INST_ADD,@INST_STAT,@TH_CENT_CH,@SEC_TH_CEN,@OCCUPATION,@CATEGORY,@PREV_APP,@PREV_M,@PREV_YEAR,@PREV_ROLL,@Rollno,@cent_allot,@cent_add,@examDate,@batch,@rep_time)", connection);
                    //command.Parameters.AddWithValue("@access", Access);
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

                  //  command.Parameters.AddWithValue("", certificate.CITY);

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