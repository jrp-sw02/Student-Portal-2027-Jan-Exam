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

public partial class HO_Rpt_ExamCentreListReport : BasePage
{
    Table tbl = new Table();
    EConnectContext context;
    //Int32 StatusId = 0;
    UserType loginUserType;
    //Int64 entityID = 0;
    //Int32 applicantTypeID = 0;
    Int32 ListModeID = 0;
    Int64 StateId = 0;
    Int32 CourseCategoryID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }

            loginUserType = (UserType)Session["UserType"];
            if (!Page.IsPostBack)
            {
                if ((!string.IsNullOrEmpty(Request.QueryString["StateId"])) && (!string.IsNullOrEmpty(Request.QueryString["CourseCategoryId"])) && (!string.IsNullOrEmpty(Request.QueryString["ListModeId"])))
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
                    Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Home.aspx")));
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
            if (ListModeID == 0)
            {

                TableHeaderCell tcCol1 = new TableHeaderCell();
                tcCol1.Width = Unit.Percentage(60);
                tcCol1.Text = "State Name";
                tcCol1.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol1);

                TableHeaderCell tcCol2 = new TableHeaderCell();
                tcCol2.Width = Unit.Percentage(39);
                tcCol2.Text = "No of Exam Centres";
                tcCol2.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol2);
            }
            else
            {
                TableHeaderCell tcCol1 = new TableHeaderCell();
                tcCol1.Width = Unit.Percentage(30);
                tcCol1.Text = "State Name";
                tcCol1.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol1);

                TableHeaderCell tcCol2 = new TableHeaderCell();
                tcCol2.Width = Unit.Percentage(30);
                tcCol2.Text = "Centre Name";
                tcCol2.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol2);

                TableHeaderCell tcCol3 = new TableHeaderCell();
                tcCol3.Width = Unit.Percentage(20);
                tcCol3.HorizontalAlign = HorizontalAlign.Center;
                tcCol3.Text = "Centre Code";
                th.Cells.Add(tcCol3);

                //TableHeaderCell tcCol4 = new TableHeaderCell();
                //tcCol4.Width = Unit.Percentage(19);
                //tcCol4.Text = "Centre Type";
                //tcCol4.HorizontalAlign = HorizontalAlign.Center;
                //th.Cells.Add(tcCol4);
            }
            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ShowExamCentreData(IEnumerable<ExamCenter> examcentre)
    {
        try
        {

            ShowTableHeader();
            int i = 1;
            foreach (ExamCenter app in examcentre)
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
                tdRow1.Width = Unit.Percentage(20);
                tdRow1.Text = app.State.Name.ToString();
                tdRow1.HorizontalAlign = HorizontalAlign.Left;
                tr.Cells.Add(tdRow1);

                if (ListModeID == 0)
                {
                    //ExamCentre=ExamCentre
                    context = new EConnectContext();
                    HyperLink link = new HyperLink();
                    //ListModeID = 1;
                    //StateId = app.StateID;
                    link.NavigateUrl = "ExamCentreListReport.aspx?StateId=" + app.StateID + "&CourseCategoryId=" + CourseCategoryID + "&ListModeId=1";
                    TableCell tdRow2 = new TableCell();
                    tdRow2.Width = Unit.Percentage(30);
                    if (app.StateID != null)
                        link.Text = getcount(context, Convert.ToInt64(app.StateID)).ToString();
                    tdRow2.Controls.Add(link);
                    tdRow2.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow2);
                }
                else
                {
                    TableCell tdRow2 = new TableCell();
                    tdRow2.Width = Unit.Percentage(30);
                    if (app.Name != null)
                        tdRow2.Text = app.Name.ToString();
                    tdRow2.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow2);

                    TableCell tdRow3 = new TableCell();
                    tdRow3.Width = Unit.Percentage(20);
                    if (app.Code != null)
                        tdRow3.Text = app.Code.ToString();
                    tdRow3.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow3);


                    //TableCell tdRow4 = new TableCell();
                    //tdRow4.Width = Unit.Percentage(19);
                    //if (app.ExamCentreTypeID != null)
                    //    tdRow4.Text = EConnect.Utils.Common.EnumUtility.GetDescription((enmExamCenterType)(app.ExamCentreTypeID));
                    //tdRow4.HorizontalAlign = HorizontalAlign.Left;
                    //tr.Cells.Add(tdRow4);
                }
                tbl.Rows.Add(tr);
                i++;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally { context.Dispose(); }

    }
    protected Int32 getcount(EConnectContext context, Int64 StateID)
    {
        try
        {
            var totalrecord = (from c in context.ExamCenters
                               where c.StateID == StateID && c.CourseCategoryID == CourseCategoryID
                               select c).Count();

            if (totalrecord != 0)
                return totalrecord;
            else
                return 0;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected string getName(EConnectContext context, Int64 StateID)
    {
        try
        {

            var stname = (from c in context.Locations
                          where c.ID == StateID && c.LocationTypeID == 2
                          select c).FirstOrDefault().Name;

            if (stname != null)
                return stname;
            else
                return "";

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
            using (EConnectContext context = new EConnectContext())
            {
                CourseCategoryID = Convert.ToInt32(Request.QueryString["CourseCategoryId"]);
                StateId = Convert.ToInt64(Request.QueryString["StateId"]);
                ListModeID = Convert.ToInt32(Request.QueryString["ListModeId"]);
                string strHead = "";

                CourseCategory courseCat = context.CourseCategories.Find(CourseCategoryID);
                if (courseCat != null)
                {
                    strHead += "</br> <b>Course Category :</b> " + GetInitCap(courseCat.Name.ToString());
                }
                if (StateId != 0)
                {

                    var state = (from s in context.Locations
                                 where s.LocationTypeID == 2 && s.ID == StateId
                                 select s).FirstOrDefault();
                    if (state != null)
                    {
                        strHead += ", <b>State :</b> " + GetInitCap(state.Name.ToString());
                    }


                }
                else if (StateId == 0)
                {
                    strHead += ", <b>State :</b>All";
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "No Record Found";
                }
                if (ListModeID == 1)
                {

                    var ExamCentre = (from s in context.ExamCenters
                                      where s.CourseCategoryID == CourseCategoryID
                                      orderby s.State.Name
                                      select s).ToList();
                    if (StateId != 0)
                    {
                        //&& s.StateID == StateId
                        ExamCentre = ExamCentre.Where(s => s.StateID == StateId).ToList();
                    }

                    if (ExamCentre.Count > 0)
                    {
                        ShowExamCentreData(ExamCentre);
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }
                }
                else
                {
                    int i = 1;

                        var stateidlist = (from s in context.ExamCenters
                                           where s.CourseCategoryID == CourseCategoryID
                                           orderby s.State.Name
                                           select s.StateID).Distinct().ToList();

                        if (stateidlist != null)
                        {
                            ShowTableHeader();
                            if (StateId == 0)
                            {
                                foreach (var stid in stateidlist)
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
                                    tdRow1.Width = Unit.Percentage(60);
                                    tdRow1.Text = getName(context, stid);
                                    tdRow1.HorizontalAlign = HorizontalAlign.Left;
                                    tr.Cells.Add(tdRow1);

                                    HyperLink link = new HyperLink();
                                    link.Target = "new";
                                    link.NavigateUrl = "ExamCentreListReport.aspx?StateId=" + stid + "&CourseCategoryId=" + CourseCategoryID + "&ListModeId=1";
                                    TableCell tdRow2 = new TableCell();
                                    tdRow2.Width = Unit.Percentage(39);
                                    if (stid != null)
                                        link.Text = getcount(context, Convert.ToInt64(stid)).ToString();
                                    tdRow2.Controls.Add(link);
                                    tdRow2.HorizontalAlign = HorizontalAlign.Center;
                                    tr.Cells.Add(tdRow2);

                                    tbl.Rows.Add(tr);
                                    i++;
                                }
                            }
                            else
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
                                tdRow1.Width = Unit.Percentage(60);
                                tdRow1.Text = getName(context, StateId);
                                tdRow1.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow1);

                                HyperLink link = new HyperLink();
                                link.Target = "new";
                                link.NavigateUrl = "ExamCentreListReport.aspx?StateId=" + StateId + "&CourseCategoryId=" + CourseCategoryID + "&ListModeId=1";
                                TableCell tdRow2 = new TableCell();
                                tdRow2.Width = Unit.Percentage(39);
                                if (StateId != null)
                                    link.Text = getcount(context, Convert.ToInt64(StateId)).ToString();
                                tdRow2.Controls.Add(link);
                                tdRow2.HorizontalAlign = HorizontalAlign.Right;
                                tr.Cells.Add(tdRow2);

                                tbl.Rows.Add(tr);
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
            };
        }
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
            using (EConnectContext context = new EConnectContext())
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
                    //command = new OleDbCommand("delete from  [MS Access;Database=" + Access + "].[Exam_Database]", connection);
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
                                                                    new SqlParameter("@cent_allot", dtrow["cent_allot"].ToString().Trim() ),
                                                                        new SqlParameter("@cent_add", dtrow["cent_add"].ToString().Trim()),
                                                                            new SqlParameter("@batch", dtrow["batch"].ToString().Trim()),
                                                                                 new SqlParameter("@rep_time", dtrow["rep_time"].ToString().Trim()),
                                                                                     new SqlParameter("@Application_Status_ID", Convert.ToInt32(enmCertificateExamApplicationStatus.ExamCentreAndRollNumberAlloted)),
                                                                                         new SqlParameter("@currentDate", Convert.ToDateTime(DateTime.Now)),
                                                                                            new SqlParameter("@examDate", Convert.ToDateTime(dtrow["examDate"].ToString())),
                                                                                                new SqlParameter("@UserID", Convert.ToInt32(Session["UserID"])),
                                                                                                    new SqlParameter("@sr_no", dtrow["sr_no"].ToString())    };
                                    context.Database.ExecuteSqlCommand("update Certificate_Exam_Application set  " +
                                                                       "Roll_Number =@Rollno," +
                                                                       "Exam_Centre_Name=@cent_allot," +
                                                                       "Exam_Centre_Address=@cent_add," +
                                                                       "Exam_Batch_Number=@batch," +
                                                                       "Reporting_Time=@rep_time," +
                                                                       "Application_Status_ID= @Application_Status_ID ," +
                                                                       "Updated_On=@currentDate," +
                                                                       "Date_of_Exam=@examDate," +
                                                                       "Updated_By=@UserID where ID=@sr_no");

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