using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Data;

public partial class Admin_DownloadCertificateData : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
    Int32 currentRoleId = 0;
    Int32 paymentPendingStatusID = Convert.ToInt32(enmPaymentStatus.Paid);

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (loginUserType == UserType.RegionalCenter || loginUserType == UserType.HeadOffice || loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
            {
                if (!IsPostBack)
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BindCourseCategory();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Certificate Data", "#", ""));
                }
            }
            else
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Certificate Data", "#", ""));                
                btnReset.Visible = false;
                Lblerror.Text = "You can not Download Certificate Data.";
                Lblerror.Visible = true;
            }
            BreadCrumb1.Render();

        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message;
            Lblerror.Visible = true;
        }
    }
    protected void BindCourseCategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.CourseCategories
                              join c in context.Courses
                                  on s.ID equals c.CourseCategoryID
                              where c.CourseTypeID == courseTypeCertificateExam
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses.Distinct(), lst);

                ListItem lst2 = new ListItem("--All--", "%");
                var rc = from s in context.RegionalCenters
                         select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlRc, rc.OrderBy(c => c.TextField), lst2);
                ddlRc.Items.Insert(0, new ListItem("--Select--", "0"));
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.Courses
                              where s.CourseCategoryID == courseCatId && s.CourseTypeID == 2
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
            }
            ddlCourseName.SelectedValue = "0";
            ddlCourseName_SelectedIndexChanged(ddlCourseName, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 cid = Convert.ToInt32(ddlCourseName.SelectedValue);
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var ApplicationList = from p in context.ApplicationTypes
                                          where p.CourseTypeID == cr.CourseTypeID
                                          select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, ApplicationList, lst);
                }
                else
                {
                    ddlAppType.Items.Clear();
                    ddlAppType.Items.Insert(0, lst);
                }
            };
            ddlAppType.SelectedValue = "0";
            ddlAppType_SelectedIndexChanged(ddlAppType, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlAppType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
            Int32 AppTypeId = Convert.ToInt32(ddlAppType.SelectedValue);
            if (AppTypeId > 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
                    ListItem lst1 = new ListItem("--Select One--", "0");
                    var courses = from s in context.ExaminationCycles
                                  join a in context.ApplicationTypes on s.Course.CourseTypeID equals a.CourseTypeID
                                  where s.CourseID == CourseId
                                  select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, courses.Distinct(), lst1);

                };
            }
            else
            {
                ddlExamCycle.Items.Clear();
                ddlExamCycle.Items.Insert(0, lst);
                ddlExamCycle.SelectedValue = "0";
                ddlExamCycle_SelectedIndexChanged(ddlExamCycle, EventArgs.Empty);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseId > 0)
                {
                    Course currentcourse = context.Courses.Find(courseId);
                    var courses = (from s in context.Exams
                                   where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId
                                   select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                    courses = courses.OrderByDescending(s => s.ValueField);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, courses, lst);
                    ddlExamYear_SelectedIndexChanged(ddlExamYear, EventArgs.Empty);
                }
                else
                {
                    ddlExamYear.Items.Clear();
                    ddlExamYear.Items.Insert(0, lst);
                    ddlExamYear.SelectedValue = "0";
                    ddlExamYear_SelectedIndexChanged(ddlExamName, EventArgs.Empty);
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            int ExamCycleId = Convert.ToInt32(ddlExamCycle.SelectedValue);
            int examYear = Convert.ToInt32(ddlExamYear.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseId > 0)
                {
                    var courses = (from s in context.Exams
                                   where s.CourseID == courseId && s.ExaminationCycleID == ExamCycleId && s.ExamYear == examYear
                                   && s.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                   select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                    courses = courses.OrderByDescending(s => s.ValueField);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, courses, lst);
                    ddlExamName_SelectedIndexChanged(ddlExamName, EventArgs.Empty);
                }
                else
                {
                    ddlExamName.Items.Clear();
                    ddlExamName.Items.Insert(0, lst);
                    ddlExamName.SelectedValue = "0";
                    ddlExamName_SelectedIndexChanged(ddlExamName, EventArgs.Empty);
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlExamName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (ddlExamName.SelectedValue != "0")
            {
                using (EConnectContext context = new EConnectContext())
                {
                    //Int32 applicationStatus = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing);
                    Int32 applType = Convert.ToInt32(ddlAppType.SelectedValue);
                    Int32 examID = Convert.ToInt32(ddlExamName.SelectedValue);
                    Int32 CourseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
                    divBatchDetails.Visible = true;
                    if (context.Exams.Find(examID).IsBatchProcessable)
                    {
                        //BindGridView();
                        lblApplicationsDetails.Visible = false;
                        //tblGrid.Visible = true;
                    }
                    else
                    {
                        //tblGrid.Visible = false;
                        var Grade = (from s in context.ResultGrades
                                     where s.CourseCategoryID == CourseCatId && s.Description == "Pass"
                                     select new { PassNo = s.ID }).ToList();
                        List<int> PassRefernce = new List<int>();
                        foreach (var p in Grade)
                            PassRefernce.Add(p.PassNo);
                        var appls = context.CertificateExamApplications.Where(a => a.ExamID == examID && a.RollNumber != null && a.ResultGradeID != null && PassRefernce.Contains(a.ResultGradeID.Value)).Select(p => new { p.Number, p.RegionalCenterID });
                        if (ddlRc.SelectedValue != "0" && ddlRc.SelectedValue != "%")
                        {
                            Int32 regionalCentreID = Convert.ToInt32(ddlRc.SelectedValue);
                            appls = appls.Where(b => b.RegionalCenterID == regionalCentreID);
                        }
                        if (appls != null)
                        {
                            lblApplicationsDetails.Text = "Total Number of Applications: " + appls.Count();
                            lblApplicationsDetails.Visible = true;
                        }


                    }
                };
            }
            else
                divBatchDetails.Visible = false;

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {

        OleDbConnection connection = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();
        string mdbFilePath = "";
        String filename = "";
        try
        {
            context = new EConnectContext();
            DataTable dtTbl = new DataTable();
            Int32 completed = Convert.ToInt32(enmBatchStatus.Completed);
            Int32 CourseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            String examCycle = ddlExamCycle.SelectedItem.Text;
            Int32 examYear = Convert.ToInt32(ddlExamYear.SelectedValue);
            Course currentCourse = context.Courses.Find(CourseId);
            Int32 examId = Convert.ToInt32(ddlExamName.SelectedValue);
            string regCentreId = ddlRc.SelectedValue;
            if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
            {
                string regCentreCode = "";
                if (ddlRc.SelectedValue != "%")
                    regCentreCode = context.RegionalCenters.Find(Convert.ToInt32(regCentreId)).Code;
                else
                    regCentreCode = "All";
                string courseCode = currentCourse.Code.ToString();
                filename = regCentreCode + "_" + courseCode + "_" + ddlExamName.SelectedItem.Text.Replace(" ", "").Replace(",", "_") + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".mdb";
            }
            mdbFilePath = Server.MapPath("~/Download/" + filename);
            if (System.IO.File.Exists(mdbFilePath))
                System.IO.File.Delete(mdbFilePath);

            System.IO.File.Copy(Server.MapPath("~/Download/Certificate_file_format.mdb"), mdbFilePath);
            string connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mdbFilePath + ";Persist Security Info=False;";
            //string connect = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbFilePath + ";Persist Security Info=False;";

            connection.ConnectionString = connect;
            connection.Open();
            command = new OleDbCommand("delete from  [MS Access;Database=" + mdbFilePath + "].[CERT]", connection);
            command.ExecuteNonQuery();
            String sqlStr = "";
            String sql = "";
            sql = " select  Number application_no, Roll_Number rollno, c.name, Father_Name f_name, Mother_Name m_name, Guardian_Name g_name, Dob d_o_b," +
                    " I.Name inst_name, rg.code grade,(Select DateName( month , DateAdd( month , exam_month , -1 )) exam_month from Exam where ID = " + examId + ") as exam_month, " +
                    " " + examYear + " as exam_year, '" + examCycle + "' exam_type," +
                    " (Select name from Course where ID=" + CourseId + ") coursename, case UID_Type when 1 then  UID_Number else '' end  Aadhaar, " +
                    " '' Aadhaar_Enrl, '' grade_legends_desc, '' Remarks, (select Code  from Regional_Center where ID = c.Regional_Center_ID)  regional_centre, " +
                    " '' affidavit_srno, '' affidavit_date, case UID_Type when 2 then  UID_Number else '' end  pan_no, (select Name  from Institute where ID =c.institute_id ) sponsor, " +
                    " Cor_Address1  address1, Cor_Address2  address2, Cor_Address3  address3, Cor_City_Name address_city, upper(loc.Name) address_state, Cor_Pin_Code  address_pin, '' registration_no, '' course_start_on, '' course_end_on  " +
                    " from Certificate_Exam_Application c, Location loc, Result_Grading rg, Institute I " +
                    " where Roll_Number is not null and Result_Grade_ID is not null and c.Institute_Id = I.ID " +
                    " and rg.Course_Category_ID =c.Course_Category_ID and rg.[Description] ='PASS' and rg.ID=c.Result_Grade_ID " +
                    " and c.Cor_State_ID =loc.ID and Exam_ID= " + examId + " and c.Course_Category_ID = " + CourseCatId + "  and c.Course_ID = " + CourseId + " and Regional_Center_ID like '" + regCentreId + "' ";


            dtTbl = DbUtility.GetDataTable(sql, new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
            if (dtTbl.Rows.Count > 0)
            {
                for (int i = 0; i < dtTbl.Rows.Count; i++)
                {
                    sqlStr = " insert into [MS Access;Database=" + mdbFilePath + "].[CERT] (application_no, rollno, name, f_name, m_name, g_name, d_o_b," +
                                    " inst_name, grade, exam_month, exam_year, exam_type, coursename, Aadhaar, Aadhaar_Enrl, grade_legends_desc," +
                                    " Remarks, regional_centre, affidavit_srno,  pan_no, sponsor, address1, address2, address3, address_city," +
                                    " address_state, address_pin, TP_Name, registration_no, course_start_on, course_end_on ";
                    sqlStr += "      ) " +
                                    " values('" + dtTbl.Rows[i]["application_no"] + "', '" + dtTbl.Rows[i]["rollno"] + "', '" + dtTbl.Rows[i]["name"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["f_name"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["m_name"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["g_name"].ToString().Replace("'", "''") + "', #" + dtTbl.Rows[i]["d_o_b"].ToString() + "#, '" + dtTbl.Rows[i]["inst_name"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["grade"] + "','"
                                    + dtTbl.Rows[i]["exam_month"].ToString() + "', '" + dtTbl.Rows[i]["exam_year"].ToString() + "', '" + dtTbl.Rows[i]["exam_type"].ToString() + "', '" + dtTbl.Rows[i]["coursename"].ToString() + "', '" + dtTbl.Rows[i]["Aadhaar"].ToString() + "','" + dtTbl.Rows[i]["Aadhaar_Enrl"].ToString() + "',  '"
                                    + dtTbl.Rows[i]["grade_legends_desc"].ToString() + "', '" + dtTbl.Rows[i]["Remarks"].ToString() + "', '" + dtTbl.Rows[i]["regional_centre"].ToString() + "', '" + dtTbl.Rows[i]["affidavit_srno"].ToString() + "' , '" + dtTbl.Rows[i]["pan_no"].ToString() + "', '" + dtTbl.Rows[i]["sponsor"].ToString() + "', '" + dtTbl.Rows[i]["address1"].ToString().Replace("'", "''")
                                    + "', '" + dtTbl.Rows[i]["address2"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["address3"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["address_city"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["address_state"]
                                    + "', '" + dtTbl.Rows[i]["address_pin"] + "', ' ', '" + dtTbl.Rows[i]["registration_no"] + "', '" + dtTbl.Rows[i]["course_start_on"] + "', '" + dtTbl.Rows[i]["course_end_on"] + "'";
                    sqlStr += ")";

                    command = new OleDbCommand(sqlStr, connection);
                    command.ExecuteNonQuery();
                }
                command.Dispose();
                connection.Close();
                connection.Dispose();
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.ContentType = "application/octet-stream";
                Response.Charset = "UTF-8";
                Response.WriteFile(mdbFilePath);
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
        finally { context.Dispose(); }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlCourseCategry.SelectedValue = "0";
            ddlCourseName.SelectedValue = "0";
            ddlAppType.SelectedValue = "0";
            ddlExamCycle.SelectedValue = "0";
            ddlExamYear.SelectedValue = "0";
            ddlExamName.SelectedValue = "0";
            ddlRc.SelectedValue = "0";
            divBatchDetails.Visible = false;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}