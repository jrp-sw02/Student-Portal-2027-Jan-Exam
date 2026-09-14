using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class Admin_Studentstatistics : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblerror.Text = "";
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!IsPostBack)
            {
                BindCourseCategory();
                //FillAppType();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Student Statistics in Exam", "#", ""));
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }

    #region-------Private Methods----------------
    private void BindCourseCategory()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.CourseCategories
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, courses, lst);
            };
        }
        catch (Exception ex)
        { throw ex; }

    }
    #endregion--------------------------------

    #region-------Events-------------------------

    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                //if (courseCatId > 1)
                //{
                var courses = from s in context.Courses
                              where s.CourseCategoryID == courseCatId
                              select new { ValueField = s.ID, TextField = s.Code };
                courses = courses.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
                if (courseCatId == 1)
                {
                    ddlCourseName.Items.Clear();
                    ddlCourseName.Items.Add(new ListItem("---Select One--", "0"));
                    ddlCourseName.Items.Add(new ListItem("---All--", "00"));
                }

                if (courseCatId == 2)
                {
                    ddlCourseName.Items.Clear();
                    //ddlCourseName.Items.Add(new ListItem("---Select One--", "0"));
                    ListItem lst1 = new ListItem("-- All --", "1"); 
                    //ddlCourseName.Items.Add(new ListItem("---Select One--", "0"));
                    //ddlCourseName.Items.Add(new ListItem("All", "1"));
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst1);
                   // EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst1);                   
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
        String courseName = ddlCourseName.SelectedItem.Text;
        if (courseName == "ACC")
        {
            DDlApplicationStatus.Items.Remove(DDlApplicationStatus.Items.FindByText("Appeared"));
            DDlApplicationStatus.Items.Remove(DDlApplicationStatus.Items.FindByText("Passed"));
        }
        if (courseCatId == 1)
        {
            DDlApplicationStatus.Items.Clear();
            DDlApplicationStatus.Items.Add(new ListItem("---Select One--", "0"));
            DDlApplicationStatus.Items.Add(new ListItem("Registered", "1"));
            DDlApplicationStatus.Items.Add(new ListItem("Certified", "2"));
        }

    }
    #endregion--------------------------------
    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt;
            int CourseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            int CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            String CourseCode = ddlCourseName.SelectedItem.Text;

            DateTime FromDate = Convert.ToDateTime(txttDateFrom.Text.Trim());
            DateTime ToDate = Convert.ToDateTime(txtDateto.Text.Trim());

            StringBuilder mySql = new StringBuilder();
            StringBuilder mySql1 = new StringBuilder();

            if (CourseCatId == 1) ///O/A/B/C Courses
            {
                // State Wise
                if (DdlReportType.SelectedValue == "1")
                {
                    //Registered
                    if (DDlApplicationStatus.SelectedValue == "1")
                    {
                          mySql.Append("select state_name, " +
                                       "SUM(Reg_cand_count) 'TotalReg'," +
                                       "sum(case when level_code='A' then Reg_cand_count else 0 end) 'ALevel'," +
                                       "sum(case when level_code='O' then Reg_cand_count else 0 end) 'OLevel'," +
                                       "sum(case when level_code='B' then Reg_cand_count else 0 end) 'BLevel'," +
                                       "sum(case when level_code='C' then Reg_cand_count else 0 end) 'CLevel'," +
                                       "sum(case when level_code='O' and sex_desc='Female' then Reg_cand_count else 0 end) 'OFemale', " +
                                       "sum(case when level_code='O' and sex_desc='Male' then Reg_cand_count else 0 end) 'OMale'," +
                                       "sum(case when level_code='O' and category_desc='General' then Reg_cand_count else 0 end) 'OGeneral', " +
                                       "sum(case when level_code='O' and category_desc='Scheduled Caste' then Reg_cand_count else 0 end) 'OSC', " +
                                       "sum(case when level_code='O' and category_desc='Scheduled Tribe' then Reg_cand_count else 0 end) 'OST', " +
                                       "sum(case when level_code='O' and category_desc='OBC' then Reg_cand_count else 0 end) 'OOBC', " +
                                       "sum(case when level_code='A' and sex_desc='Female' then Reg_cand_count else 0 end) 'AFemale', " +
                                       "sum(case when level_code='A' and sex_desc='Male' then Reg_cand_count else 0 end) 'AMale', " +
                                       "sum(case when level_code='A' and category_desc='General' then Reg_cand_count else 0 end) 'AGeneral', " +
                                       "sum(case when level_code='A' and category_desc='Scheduled Caste' then Reg_cand_count else 0 end) 'ASC', " +
                                       "sum(case when level_code='A' and category_desc='Scheduled Tribe' then Reg_cand_count else 0 end) 'AST', " +
                                       "sum(case when level_code='A' and category_desc='OBC' then Reg_cand_count else 0 end) 'AOBC', " +
                                       "sum(case when level_code='B' and sex_desc='Female' then Reg_cand_count else 0 end) 'BFemale', " +
                                       "sum(case when level_code='B' and sex_desc='Male' then Reg_cand_count else 0 end) 'BMale', " +
                                       "sum(case when level_code='B' and category_desc='General' then Reg_cand_count else 0 end) 'BGeneral', " +
                                       "sum(case when level_code='B' and category_desc='Scheduled Caste' then Reg_cand_count else 0 end) 'BSC', " +
                                       "sum(case when level_code='B' and category_desc='Scheduled Tribe' then Reg_cand_count else 0 end) 'BST', " +
                                       "sum(case when level_code='B' and category_desc='OBC' then Reg_cand_count else 0 end) 'BOBC', " +
                                       "sum(case when level_code='C' and sex_desc='Female' then Reg_cand_count else 0 end) 'CFemale', " +
                                       "sum(case when level_code='C' and sex_desc='Male' then Reg_cand_count else 0 end) 'CMale', " +
                                       "sum(case when level_code='C' and category_desc='General' then Reg_cand_count else 0 end) 'CGeneral', " +
                                       "sum(case when level_code='C' and category_desc='Scheduled Caste' then Reg_cand_count else 0 end) 'CSC', " +
                                       "sum(case when level_code='C' and category_desc='Scheduled Tribe' then Reg_cand_count else 0 end) 'CST', " +
                                       "sum(case when level_code='C' and category_desc='OBC' then Reg_cand_count else 0 end) 'COBC' " +
                                       "from(" +
                                        "select COUNT(*) as Reg_cand_count, level_code , c.state_name ,d.sex_desc ,f.category_desc " +
                                        "from doeacc.dbo.r_candidate_registration_details a, doeacc.dbo.r_candidate_corr_address b," +
                                        "doeacc.dbo.g_state_codes c , doeacc.dbo.g_sex_codes d ,doeacc.dbo.r_candidate_personal_details e, doeacc.dbo.g_category_codes f " +
                                        "where registration_status <>'C' and registration_date >=CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) and registration_date < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                        "and a.candidate_code =b.candidate_code and b.to_date is null and c.state_code =b.corr_state_code " +
                                        "and e.sex_code =d.sex_code and e.candidate_code =a.candidate_code and f.category_code =e.category_code " +
                                        "group by  level_code , c.state_name ,d.sex_desc  ,f.category_desc " +
                                       ") STAT	group by state_name	order by state_name");
                    }
                    //Certified
                    else if (DDlApplicationStatus.SelectedValue == "2")
                    {
                              mySql.Append("select State_Name, " +
                                           "SUM(Reg_cand_count) 'Total_Certified'," +
                                           "sum(case when level_code='A' then Reg_cand_count else 0 end) 'ALevel'," +
                                           "sum(case when level_code='O' then Reg_cand_count else 0 end) 'OLevel'," +
                                           "sum(case when level_code='B' then Reg_cand_count else 0 end) 'BLevel'," +
                                           "sum(case when level_code='C' then Reg_cand_count else 0 end) 'CLevel'," +
                                           "sum(case when level_code='O' and sex_desc='Female' then Reg_cand_count else 0 end) 'OFemale', " +
                                           "sum(case when level_code='O' and sex_desc='Male' then Reg_cand_count else 0 end) 'OMale'," +
                                           "sum(case when level_code='O' and category_desc='General' then Reg_cand_count else 0 end) 'OGeneral', " +
                                           "sum(case when level_code='O' and category_desc='Scheduled Caste' then Reg_cand_count else 0 end) 'OSC', " +
                                           "sum(case when level_code='O' and category_desc='Scheduled Tribe' then Reg_cand_count else 0 end) 'OST', " +
                                           "sum(case when level_code='O' and category_desc='OBC' then Reg_cand_count else 0 end) 'OOBC', " +
                                           "sum(case when level_code='A' and sex_desc='Female' then Reg_cand_count else 0 end) 'AFemale', " +
                                           "sum(case when level_code='A' and sex_desc='Male' then Reg_cand_count else 0 end) 'AMale', " +
                                           "sum(case when level_code='A' and category_desc='General' then Reg_cand_count else 0 end) 'AGeneral', " +
                                           "sum(case when level_code='A' and category_desc='Scheduled Caste' then Reg_cand_count else 0 end) 'ASC', " +
                                           "sum(case when level_code='A' and category_desc='Scheduled Tribe' then Reg_cand_count else 0 end) 'AST', " +
                                           "sum(case when level_code='A' and category_desc='OBC' then Reg_cand_count else 0 end) 'AOBC', " +
                                           "sum(case when level_code='B' and sex_desc='Female' then Reg_cand_count else 0 end) 'BFemale', " +
                                           "sum(case when level_code='B' and sex_desc='Male' then Reg_cand_count else 0 end) 'BMale', " +
                                           "sum(case when level_code='B' and category_desc='General' then Reg_cand_count else 0 end) 'BGeneral', " +
                                           "sum(case when level_code='B' and category_desc='Scheduled Caste' then Reg_cand_count else 0 end) 'BSC', " +
                                           "sum(case when level_code='B' and category_desc='Scheduled Tribe' then Reg_cand_count else 0 end) 'BST', " +
                                           "sum(case when level_code='B' and category_desc='OBC' then Reg_cand_count else 0 end) 'BOBC', " +
                                           "sum(case when level_code='C' and sex_desc='Female' then Reg_cand_count else 0 end) 'CFemale', " +
                                           "sum(case when level_code='C' and sex_desc='Male' then Reg_cand_count else 0 end) 'CMale', " +
                                           "sum(case when level_code='C' and category_desc='General' then Reg_cand_count else 0 end) 'CGeneral', " +
                                           "sum(case when level_code='C' and category_desc='Scheduled Caste' then Reg_cand_count else 0 end) 'CSC', " +
                                           "sum(case when level_code='C' and category_desc='Scheduled Tribe' then Reg_cand_count else 0 end) 'CST', " +
                                           "sum(case when level_code='C' and category_desc='OBC' then Reg_cand_count else 0 end) 'COBC' " +
                                           "from(" +
                                           "select COUNT(*) as Reg_cand_count, level_code , c.state_name ,d.sex_desc ,f.category_desc " +
                                           "from doeacc.dbo.r_candidate_registration_details a, doeacc.dbo.r_candidate_corr_address b," +
                                           "doeacc.dbo.g_state_codes c , doeacc.dbo.g_sex_codes d ,doeacc.dbo.r_candidate_personal_details e, doeacc.dbo.g_category_codes f " +
                                           "where registration_status <>'C' and certificate_issued = 'Y' and certificate_issue_date >=CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) and certificate_issue_date < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                           "and a.candidate_code =b.candidate_code and b.to_date is null and c.state_code =b.corr_state_code " +
                                           "and e.sex_code =d.sex_code and e.candidate_code =a.candidate_code and f.category_code =e.category_code " +
                                           "group by  level_code , c.state_name ,d.sex_desc  ,f.category_desc " +
                                           ") STAT	group by state_name	order by state_name");
                    }
                }
                // Month Wise
                else if (DdlReportType.SelectedValue == "2")
                {
                    ShowAlert("Your selected service is not available at present");
                    //mySql.Append("select CONVERT(varchar, registration_year) +' - '+ CONVERT(varchar, DateName( month , DateAdd( month , registration_month, -1 ) )) 'Year-Month', "+
                    //                "COUNT(*) 'OABC-Regn' from doeacc.dbo.r_candidate_registration_details "+
                    //                "where registration_status <> 'C' and (registration_year >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE)) " +
                    //                "group by registration_year, registration_month");

                }
            }
            else if (CourseCatId == 2 || CourseCatId == 9) // DLC Courses
            {
                // State Wise
                if (DdlReportType.SelectedValue == "1" && CourseId != 1)
                {
                    // Applied
                    if (DDlApplicationStatus.SelectedValue == "1")
                    {
                        if (CourseCode == "CCC" || CourseCode == "BCC" || CourseCode == "CCCP" || CourseCode == "ECC" || CourseCode == "MoPR-BCC" || CourseCode == "DVP-CCC" || CourseCode == "DVP-BCC")
                        {
                            mySql.Append("select State, sum(total) 'Total', " +
                                         "sum(case when CatID=1 then total else 0 end) 'General'," +
                                         "sum(case when CatID=2 then total else 0 end) 'SC'," +
                                         "sum(case when CatID=3 then total else 0 end) 'ST'," +
                                         "sum(case when CatID=4 then total else 0 end) 'OBC'," +
                                         "sum(case when Gender='Male' then total else 0 end) 'Male'," +
                                         "sum(case when Gender='Female' then total else 0 end) 'Female'" +
                                         "from ( select B.Name 'State', a.Cast_Category_ID CatID, C.Name Category, Gender," +
                                         "COUNT(distinct Number) 'Total'" +
                                         "from NIELIT.DBO.Certificate_Exam_Application a, nielit.dbo.Location B, nielit.dbo.Cast_Category C " +
                                         "where Final_Submission_Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) and Final_Submission_Date < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE) and Course_ID = '" + CourseId + "' " +
                                         "and Payment_Status_ID > 1   " +
                                         "and Course_Category_ID = '" + CourseCatId + "' AND A.Cor_State_ID = B.ID AND A.Cast_Category_ID=C.ID " +
                                         "group by B.Name, a.Cast_Category_ID, C.Name, Gender, A.course_id ) STAT group by State order by State");
                        }
                        else if (CourseCode == "ACC")
                        {
                              mySql.Append("select State, sum(total) 'Total'," +
                                           "sum(case when CatID=1 then total else 0 end) 'General'," +
                                           "sum(case when CatID=2 then total else 0 end) 'SC'," +
                                           "sum(case when CatID=3 then total else 0 end) 'ST'," +
                                           "sum(case when CatID=4 then total else 0 end) 'OBC'," +
                                           "sum(case when Gender='Male' then total else 0 end) 'Male'," +
                                           "sum(case when Gender='Female' then total else 0 end) 'Female'" +
                                           "from ( select B.Name 'State', a.Cast_Category_ID CatID, C.Name Category, Gender," +
                                           "COUNT(distinct Candidate_ID) 'Total'" +
                                           "from  NIELIT.DBO.Course_Registration_Application A, nielit.dbo.Location B, nielit.dbo.Cast_Category C " +
                                           "where  Final_Submission_Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) and Final_Submission_Date < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                           "and Course_ID = '" + CourseId + "' and Payment_Status_ID > 1 and Candidate_ID is not null  " +
                                           "and Course_Category_ID = '" + CourseCatId + "' AND A.Cor_State_ID = B.ID AND A.Cast_Category_ID=C.ID " +
                                           "group by B.Name, a.Cast_Category_ID, C.Name, Gender, A.course_id ) STAT group by State order by State");
                        }
                    }
                    // Appeared
                    else if (DDlApplicationStatus.SelectedValue == "2")
                    {
                        mySql.Append("select State, sum(total) 'Total', " +
                                     "sum(case when CatID=1 then total else 0 end) 'General'," +
                                     "sum(case when CatID=2 then total else 0 end) 'SC'," +
                                     "sum(case when CatID=3 then total else 0 end) 'ST'," +
                                     "sum(case when CatID=4 then total else 0 end) 'OBC'," +
                                     "sum(case when Gender='Male' then total else 0 end) 'Male'," +
                                     "sum(case when Gender='Female' then total else 0 end) 'Female'" +
                                     "from ( select B.Name 'State', a.Cast_Category_ID CatID, C.Name Category, Gender," +
                                     "COUNT(distinct Roll_Number) 'Total'" +
                                     "from NIELIT.DBO.Certificate_Exam_Application a, nielit.dbo.Location B, nielit.dbo.Cast_Category C " +
                                     "where Date_of_Exam >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) and Date_of_Exam < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE) and Course_ID = '" + CourseId + "' " +
                                     "and Result_Grade_ID in (Select ID from Result_Grading where Description in ('Pass','Fail') " +
                                     "and Course_Category_ID = '" + CourseCatId + "') AND A.Cor_State_ID = B.ID AND A.Cast_Category_ID=C.ID " +
                                     "group by B.Name, a.Cast_Category_ID, C.Name, Gender, A.course_id ) STAT group by State order by State");
                    }
                    // Passed
                    else if (DDlApplicationStatus.SelectedValue == "3")
                    {
                        mySql.Append("select State, sum(total) 'Total', " +
                                     "sum(case when CatID=1 then total else 0 end) 'General'," +
                                     "sum(case when CatID=2 then total else 0 end) 'SC'," +
                                     "sum(case when CatID=3 then total else 0 end) 'ST'," +
                                     "sum(case when CatID=4 then total else 0 end) 'OBC'," +
                                     "sum(case when Gender='Male' then total else 0 end) 'Male'," +
                                     "sum(case when Gender='Female' then total else 0 end) 'Female'" +
                                     "from ( select B.Name 'State', a.Cast_Category_ID CatID, C.Name Category, Gender," +
                                     "COUNT(distinct Roll_Number) 'Total' from NIELIT.DBO.Certificate_Exam_Application a, nielit.dbo.Location B, nielit.dbo.Cast_Category C " +
                                     "where Date_of_Exam >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) and Date_of_Exam < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE) and Course_ID = '" + CourseId + "' " +
                                     "and Result_Grade_ID in (Select ID from Result_Grading where Description = 'Pass'  " +
                                     "and Course_Category_ID = '" + CourseCatId + "') AND A.Cor_State_ID = B.ID AND A.Cast_Category_ID=C.ID " +
                                     "group by B.Name, a.Cast_Category_ID, C.Name, Gender, A.course_id ) STAT group by State order by State");
                    }
                }
                //All Statewise
                if (DdlReportType.SelectedValue == "1" && CourseId == 1)
                {
                    // Applied
                    if (DDlApplicationStatus.SelectedValue == "1")
                    {
                        //if (CourseCode == "CCC" || CourseCode == "BCC" || CourseCode == "CCCP" || CourseCode == "ECC" || CourseCode == "MoPR-BCC" || CourseCode == "DVP-CCC" || CourseCode == "DVP-BCC")
                        //{
                            mySql.Append("select State, sum(total) 'Total', " +
                                         "sum(case when CatID=1 then total else 0 end) 'General'," +
                                         "sum(case when CatID=2 then total else 0 end) 'SC'," +
                                         "sum(case when CatID=3 then total else 0 end) 'ST'," +
                                         "sum(case when CatID=4 then total else 0 end) 'OBC'," +
                                         "sum(case when Gender='Male' then total else 0 end) 'Male'," +
                                         "sum(case when Gender='Female' then total else 0 end) 'Female'" +
                                         "from ( select B.Name 'State', a.Cast_Category_ID CatID, C.Name Category, Gender," +
                                         "COUNT(distinct Number) 'Total'" +
                                         "from NIELIT.DBO.Certificate_Exam_Application a, nielit.dbo.Location B, nielit.dbo.Cast_Category C " +
                                         "where Final_Submission_Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) and Final_Submission_Date < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE)  " +
                                         "and Payment_Status_ID > 1   " +
                                         "and Course_Category_ID = '" + CourseCatId + "' AND A.Cor_State_ID = B.ID AND A.Cast_Category_ID=C.ID " +
                                         "group by B.Name, a.Cast_Category_ID, C.Name, Gender, A.course_id ) STAT group by State order by State");
                       // }
                        //else if (CourseCode == "ACC")
                        //{
                        //    mySql.Append("select State, sum(total) 'Total'," +
                        //                 "sum(case when CatID=1 then total else 0 end) 'General'," +
                        //                 "sum(case when CatID=2 then total else 0 end) 'SC'," +
                        //                 "sum(case when CatID=3 then total else 0 end) 'ST'," +
                        //                 "sum(case when CatID=4 then total else 0 end) 'OBC'," +
                        //                 "sum(case when Gender='Male' then total else 0 end) 'Male'," +
                        //                 "sum(case when Gender='Female' then total else 0 end) 'Female'" +
                        //                 "from ( select B.Name 'State', a.Cast_Category_ID CatID, C.Name Category, Gender," +
                        //                 "COUNT(distinct Candidate_ID) 'Total'" +
                        //                 "from  NIELIT.DBO.Course_Registration_Application A, nielit.dbo.Location B, nielit.dbo.Cast_Category C " +
                        //                 "where  Final_Submission_Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) and Final_Submission_Date < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                        //                 "and Course_ID = '" + CourseId + "' and Payment_Status_ID > 1 and Candidate_ID is not null  " +
                        //                 "and Course_Category_ID = '" + CourseCatId + "' AND A.Cor_State_ID = B.ID AND A.Cast_Category_ID=C.ID " +
                        //                 "group by B.Name, a.Cast_Category_ID, C.Name, Gender, A.course_id ) STAT group by State order by State");
                        //}
                    }
                    // Appeared
                    else if (DDlApplicationStatus.SelectedValue == "2")
                    {
                        mySql.Append("select State, sum(total) 'Total', " +
                                     "sum(case when CatID=1 then total else 0 end) 'General'," +
                                     "sum(case when CatID=2 then total else 0 end) 'SC'," +
                                     "sum(case when CatID=3 then total else 0 end) 'ST'," +
                                     "sum(case when CatID=4 then total else 0 end) 'OBC'," +
                                     "sum(case when Gender='Male' then total else 0 end) 'Male'," +
                                     "sum(case when Gender='Female' then total else 0 end) 'Female'" +
                                     "from ( select B.Name 'State', a.Cast_Category_ID CatID, C.Name Category, Gender," +
                                     "COUNT(distinct Roll_Number) 'Total'" +
                                     "from NIELIT.DBO.Certificate_Exam_Application a, nielit.dbo.Location B, nielit.dbo.Cast_Category C " +
                                     "where Date_of_Exam >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) and Date_of_Exam < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE)  " +
                                     "and Result_Grade_ID in (Select ID from Result_Grading where Description in ('Pass','Fail') " +
                                     "and Course_Category_ID = '" + CourseCatId + "') AND A.Cor_State_ID = B.ID AND A.Cast_Category_ID=C.ID " +
                                     "group by B.Name, a.Cast_Category_ID, C.Name, Gender, A.course_id ) STAT group by State order by State");
                    }
                    // Passed
                    else if (DDlApplicationStatus.SelectedValue == "3")
                    {
                        mySql.Append("select State, sum(total) 'Total', " +
                                     "sum(case when CatID=1 then total else 0 end) 'General'," +
                                     "sum(case when CatID=2 then total else 0 end) 'SC'," +
                                     "sum(case when CatID=3 then total else 0 end) 'ST'," +
                                     "sum(case when CatID=4 then total else 0 end) 'OBC'," +
                                     "sum(case when Gender='Male' then total else 0 end) 'Male'," +
                                     "sum(case when Gender='Female' then total else 0 end) 'Female'" +
                                     "from ( select B.Name 'State', a.Cast_Category_ID CatID, C.Name Category, Gender," +
                                     "COUNT(distinct Roll_Number) 'Total' from NIELIT.DBO.Certificate_Exam_Application a, nielit.dbo.Location B, nielit.dbo.Cast_Category C " +
                                     "where Date_of_Exam >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) and Date_of_Exam < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE)  " +
                                     "and Result_Grade_ID in (Select ID from Result_Grading where Description = 'Pass'  " +
                                     "and Course_Category_ID = '" + CourseCatId + "') AND A.Cor_State_ID = B.ID AND A.Cast_Category_ID=C.ID " +
                                     "group by B.Name, a.Cast_Category_ID, C.Name, Gender, A.course_id ) STAT group by State order by State");
                    }
                }

                // Month Wise
                else if (DdlReportType.SelectedValue == "2" && CourseId  != 1)
                {
                    // Applied
                    if (DDlApplicationStatus.SelectedValue == "1")
                    {
                        if (CourseCode == "CCC" || CourseCode == "BCC" || CourseCode == "CCCP" || CourseCode == "ECC" || CourseCode == "MoPR-BCC")
                        {
                              mySql.Append( " select CONVERT(varchar, datepart(year,[date]))+' - '+ CONVERT(varchar, DateName( month , DateAdd( month , datepart(MONTH,[date]), -1 ) ))'Year-Month'," +
                                            " count(*) 'Total-applied'" +
                                            " from nielit.dbo.Certificate_Exam_Application " +
                                            " where Final_Submitted =1 and Payment_Status_ID =2 " +
                                            " and Final_Submission_Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                            " and Date < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                            " and Course_ID = '" + CourseId + "' group by datepart(year,[date]), datepart(MONTH,[date])" +
                                            " order by datepart(year,[date]), datepart(MONTH,[date])");
                        }
                        else if (CourseCode == "ACC")
                        {
                            //string year = FromDate.Year.ToString();
                               mySql.Append("select Registration_Year 'Year', DateName( month , DateAdd( month , Registration_Month, -1 ) ) 'Month', COUNT(*) 'Total-applied' " +
                                            "from nielit.dbo.Registration_Detail " +
                                            "where registration_status <> 'C' " +
                                            "and Course_ID ='" + CourseId + "'  and Registration_Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                            "group by Registration_Year, Registration_Month " +
                                            "order by Registration_Year, Registration_Month");
                        }
                    }
                    // Appeared
                    else if (DDlApplicationStatus.SelectedValue == "2")
                    {
                           mySql.Append("select CONVERT(varchar, datepart(year,[date]))+' - '+ CONVERT(varchar, DateName( month , DateAdd( month , datepart(MONTH,[date]), -1 ) ))'Year-Month'," +
                                        "count(*) 'Total-appeared'" +
                                        "from nielit.dbo.Certificate_Exam_Application " +
                                        "where Final_Submitted =1 and Payment_Status_ID  =2 " +
                                        "and Date_of_Exam >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) and Course_ID = '" + CourseId + "'" +
                                        "and Result_Grade_ID in (Select ID from Result_Grading where Description in ('Pass', 'Fail')  " +
                                        "and Course_Category_ID = '" + CourseCatId + "') " +
                                        "group by datepart(year,[date]), datepart(MONTH,[date])" +
                                        "order by datepart(year,[date]), datepart(MONTH,[date])");
                    }
                    // Passed
                    else if (DDlApplicationStatus.SelectedValue == "3")
                    {
                          mySql.Append("select CONVERT(varchar, datepart(year,[date]))+' - '+ CONVERT(varchar, DateName( month , DateAdd( month , datepart(MONTH,[date]), -1 ) ))'Year-Month'," +
                                       "count(*) 'Total-passed'" +
                                       "from nielit.dbo.Certificate_Exam_Application " +
                                       "where Final_Submitted =1 and Payment_Status_ID  =2 " +
                                       "and Date_of_Exam >=CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) and Course_ID = '" + CourseId + "'" +
                                       "and Result_Grade_ID in (Select ID from Result_Grading where Description = 'Pass' " +
                                       "and Course_Category_ID = '" + CourseCatId + "') " +
                                       "group by datepart(year,[date]), datepart(MONTH,[date])" +
                                       "order by datepart(year,[date]), datepart(MONTH,[date])");
                    }
                }

                else if (DdlReportType.SelectedValue == "2" && CourseId == 1)
                {
                    // Applied
                    if (DDlApplicationStatus.SelectedValue == "1")
                    {
                      //  if (CourseCode == "CCC" || CourseCode == "BCC" || CourseCode == "CCCP" || CourseCode == "ECC" || CourseCode == "MoPR-BCC")
                       // {
                            mySql.Append(" select CONVERT(varchar, datepart(year,[date]))+' - '+ CONVERT(varchar, DateName( month , DateAdd( month , datepart(MONTH,[date]), -1 ) ))'Year-Month'," +
                                          " count(*) 'Total-applied'" +
                                          " from nielit.dbo.Certificate_Exam_Application " +
                                          " where Final_Submitted =1 and Payment_Status_ID =2 " +
                                          " and Final_Submission_Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                          " and Date < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                          "  group by datepart(year,[date]), datepart(MONTH,[date])" +
                                          " order by datepart(year,[date]), datepart(MONTH,[date])");
                       // }
                        //else if (CourseCode == "ACC")
                        //{
                        //    //string year = FromDate.Year.ToString();
                        //    mySql.Append("select Registration_Year 'Year', DateName( month , DateAdd( month , Registration_Month, -1 ) ) 'Month', COUNT(*) 'Total-applied' " +
                        //                 "from nielit.dbo.Registration_Detail " +
                        //                 "where registration_status <> 'C' " +
                        //                 "and Course_ID ='" + CourseId + "'  and Registration_Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                        //                 "group by Registration_Year, Registration_Month " +
                        //                 "order by Registration_Year, Registration_Month");
                        //}
                    }
                    // Appeared
                    else if (DDlApplicationStatus.SelectedValue == "2")
                    {
                        mySql.Append("select CONVERT(varchar, datepart(year,[date]))+' - '+ CONVERT(varchar, DateName( month , DateAdd( month , datepart(MONTH,[date]), -1 ) ))'Year-Month'," +
                                     "count(*) 'Total-appeared'" +
                                     "from nielit.dbo.Certificate_Exam_Application " +
                                     "where Final_Submitted =1 and Payment_Status_ID  =2 " +
                                     "and Date_of_Exam >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE)  " +
                                     "and Result_Grade_ID in (Select ID from Result_Grading where Description in ('Pass', 'Fail')  " +
                                     "and Course_Category_ID = '" + CourseCatId + "') " +
                                     "group by datepart(year,[date]), datepart(MONTH,[date])" +
                                     "order by datepart(year,[date]), datepart(MONTH,[date])");
                    }
                    // Passed
                    else if (DDlApplicationStatus.SelectedValue == "3")
                    {
                        mySql.Append("select CONVERT(varchar, datepart(year,[date]))+' - '+ CONVERT(varchar, DateName( month , DateAdd( month , datepart(MONTH,[date]), -1 ) ))'Year-Month'," +
                                     "count(*) 'Total-passed'" +
                                     "from nielit.dbo.Certificate_Exam_Application " +
                                     "where Final_Submitted =1 and Payment_Status_ID  =2 " +
                                     "and Date_of_Exam >=CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                     "and Result_Grade_ID in (Select ID from Result_Grading where Description = 'Pass' " +
                                     "and Course_Category_ID = '" + CourseCatId + "') " +
                                     "group by datepart(year,[date]), datepart(MONTH,[date])" +
                                     "order by datepart(year,[date]), datepart(MONTH,[date])");
                    }
                }
                    else if (DdlReportType.SelectedValue == "3" && CourseId != 1)
                    {
                        //Applied 
                        if (DDlApplicationStatus.SelectedValue == "1")
                        {
                            if (CourseCode == "CCC" || CourseCode == "BCC" || CourseCode == "CCCP" || CourseCode == "ECC" || CourseCode == "MoPR-BCC" || CourseCode == "DVP-CCC" || CourseCode == "DVP-BCC")
                            {
                                mySql.Append(" select  Exam_Year,State, sum(total) 'Total', " +
                                             " sum(case when CatID=1 then total else 0 end) 'General', " +
                                             " sum(case when CatID=2 then total else 0 end) 'SC', " +
                                             " sum(case when CatID=3 then total else 0 end) 'ST', " +
                                             " sum(case when CatID=4 then total else 0 end) 'OBC',  " +
                                             " sum(case when Gender='Male' then total else 0 end) 'Male', " +
                                             " sum(case when Gender='Female' then total else 0 end) 'Female' " +
                                             " from ( select   DATEPART(year,a.Date) Exam_Year,B.Name 'State', a.Cast_Category_ID CatID, C.Name Category, " +
                                             " Gender, COUNT(distinct Number) 'Total' from NIELIT.DBO.Certificate_Exam_Application a, nielit.dbo.Location B, " +
                                             " nielit.dbo.Cast_Category C where Final_Submission_Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) and " +
                                             " Final_Submission_Date < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE) and Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                             " and Date < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE) and Course_ID = '" + CourseId + "' and Payment_Status_ID > 1  " +
                                             " and Course_Category_ID ='" + CourseCatId + "' AND A.Cor_State_ID = B.ID AND A.Cast_Category_ID=C.ID " +
                                             " group by B.Name, a.Cast_Category_ID, C.Name, Gender, A.course_id " +
                                             " ,DATEPART(year,a.Date)) STAT group by State,Exam_Year order by  Exam_Year, State");
                            }
                            else if (CourseCode == "ACC")
                            {

                            }                            
                        }
                        //Appeared 
                        else if (DDlApplicationStatus.SelectedValue == "2")
                        {
                               mySql.Append("select Exam_Year,State, sum(total) 'Total', " +
                                            " sum(case when CatID=1 then total else 0 end) 'General'," +
                                            " sum(case when CatID=2 then total else 0 end) 'SC'," +
                                            " sum(case when CatID=3 then total else 0 end) 'ST', " +
                                            " sum(case when CatID=4 then total else 0 end) 'OBC', " +
                                            " sum(case when Gender='Male' then total else 0 end) 'Male'," +
                                            " sum(case when Gender='Female' then total else 0 end) 'Female'" +
                                            " from (select DATEPART(year,a.Date_of_Exam) Exam_Year,  B.Name 'State', a.Cast_Category_ID CatID, C.Name Category, Gender,COUNT(distinct Roll_Number) 'Total'" +
                                            " from NIELIT.DBO.Certificate_Exam_Application a, nielit.dbo.Location B, nielit.dbo.Cast_Category C " +
                                            " where Date_of_Exam >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) and Date_of_Exam < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE)" +                                 
                                            " and Result_Grade_ID in (Select ID from Result_Grading where Description in ('Pass','Fail') and Course_Category_ID = '" + CourseCatId + "') AND A.Cor_State_ID = B.ID " +
                                            " AND A.Cast_Category_ID=C.ID group by B.Name, a.Cast_Category_ID, C.Name, Gender, " +
                                            " DATEPART(year,a.Date_of_Exam)) STAT group by State , Exam_Year order by Exam_Year,State");
                        }
                        //Passed
                        else if (DDlApplicationStatus.SelectedValue == "3")
                        {
                            mySql.Append("select Exam_Year,State, sum(total) 'Total', sum(case when CatID=1 then total else 0 end) 'General',sum(case when CatID=2 then total else 0 end) 'SC'," +
                                        " sum(case when CatID=3 then total else 0 end) 'ST',sum(case when CatID=4 then total else 0 end) 'OBC', sum(case when Gender='Male' then total else 0 end) 'Male'"+
                                        ",sum(case when Gender='Female' then total else 0 end) 'Female' from ( select   DATEPART(year,a.Date_of_Exam) Exam_Year,B.Name 'State', a.Cast_Category_ID CatID," + 
                                        " C.Name Category, Gender,COUNT(distinct Roll_Number) 'Total' from NIELIT.DBO.Certificate_Exam_Application a, nielit.dbo.Location B, nielit.dbo.Cast_Category C " +
                                        " where Date_of_Exam >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) and Date_of_Exam < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE)"+
                                        " and Course_ID = '" + CourseId + "' and Result_Grade_ID in (Select ID from Result_Grading where Description = 'Pass'  and Course_Category_ID = '" + CourseCatId + "')"+
                                        " AND A.Cor_State_ID = B.ID AND A.Cast_Category_ID=C.ID group by B.Name, a.Cast_Category_ID, C.Name, Gender, A.course_id ,DATEPART(year,a.Date_of_Exam)) " +
                                        " STAT group by State,Exam_Year order by  Exam_Year, State");
                        }
                    }

                else if (DdlReportType.SelectedValue == "3" &&  CourseId == 1)
                    {
                        //Applied 
                        if (DDlApplicationStatus.SelectedValue == "1")
                        {                           
                                mySql.Append(" select  Exam_Year,State, sum(total) 'Total', "+
                                             " sum(case when CatID=1 then total else 0 end) 'General', " + 
                                             " sum(case when CatID=2 then total else 0 end) 'SC', " +
                                             " sum(case when CatID=3 then total else 0 end) 'ST', " +
                                             " sum(case when CatID=4 then total else 0 end) 'OBC', " +
                                             " sum(case when Gender='Male' then total else 0 end) 'Male', " +
                                             " sum(case when Gender='Female' then total else 0 end) 'Female' " +
                                             " from ( select   DATEPART(year,a.Date) Exam_Year,B.Name 'State', a.Cast_Category_ID CatID, C.Name Category, Gender," +
                                             " COUNT(distinct Number) 'Total' from NIELIT.DBO.Certificate_Exam_Application a, nielit.dbo.Location B, nielit.dbo.Cast_Category C " +
                                             " where Final_Submission_Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                             " and Final_Submission_Date < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                             " and Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                             " and Date < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                             " and Payment_Status_ID > 1  and Course_Category_ID = '" + CourseCatId + "' AND " +
                                             " A.Cor_State_ID = B.ID AND A.Cast_Category_ID=C.ID group by B.Name, a.Cast_Category_ID, C.Name, Gender "+
                                             ",DATEPART(year,a.Date)) STAT group by State,Exam_Year order by  Exam_Year, State");                                                                
                            
                            //else if (CourseCode == "ACC")
                            //{

                            //}
                        }
                        //Appeared 
                        else if (DDlApplicationStatus.SelectedValue == "2")
                        {
                            mySql.Append(" select Exam_Year,State, sum(total) 'Total', " +
                                         " sum(case when CatID=1 then total else 0 end) 'General', " +
                                         " sum(case when CatID=2 then total else 0 end) 'SC', " +
                                         " sum(case when CatID=3 then total else 0 end) 'ST', " +
                                         " sum(case when CatID=4 then total else 0 end) 'OBC', " +
                                         " sum(case when Gender='Male' then total else 0 end) 'Male'," +
                                         " sum(case when Gender='Female' then total else 0 end) 'Female' " +
                                         " from ( select DATEPART(year,a.Date_of_Exam) Exam_Year,  B.Name 'State', a.Cast_Category_ID CatID, " +
                                         " C.Name Category, Gender,COUNT(distinct Roll_Number) 'Total' from NIELIT.DBO.Certificate_Exam_Application a," +
                                         " nielit.dbo.Location B, nielit.dbo.Cast_Category C where  Date_of_Exam >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                         " and Date_of_Exam < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE) and Result_Grade_ID in " +
                                         " (Select ID from Result_Grading where Description in ('Pass','Fail') and Course_Category_ID = '" + CourseCatId + "') AND A.Cor_State_ID = B.ID " +
                                         " AND A.Cast_Category_ID=C.ID group by B.Name, a.Cast_Category_ID, C.Name, Gender, " +
                                         " DATEPART(year,a.Date_of_Exam)) STAT group by State , Exam_Year order by Exam_Year,State");
                        }
                        //Passed
                        else if (DDlApplicationStatus.SelectedValue == "3")
                        {
                            mySql.Append(" select Exam_Year,State, sum(total) 'Total', sum(case when CatID=1 then total else 0 end) 'General',sum(case when CatID=2 then total else 0 end) 'SC',"+
                                         " sum(case when CatID=3 then total else 0 end) 'ST',sum(case when CatID=4 then total else 0 end) 'OBC', " +
                                         " sum(case when Gender='Male' then total else 0 end) 'Male',sum(case when Gender='Female' then total else 0 end) 'Female' " +
                                         " from ( select   DATEPART(year,a.Date_of_Exam) Exam_Year,B.Name 'State', a.Cast_Category_ID CatID, C.Name Category, Gender,COUNT(distinct Roll_Number) 'Total' " +
                                         " from NIELIT.DBO.Certificate_Exam_Application a, nielit.dbo.Location B, nielit.dbo.Cast_Category C " +
                                         " where Date_of_Exam >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                         " and Date_of_Exam < CAST('" + ToDate.ToString("dd-MMM-yyyy") + "' as DATE) " +
                                         " and Result_Grade_ID in (Select ID from Result_Grading where Description = 'Pass'  and Course_Category_ID = '" + CourseCatId + "') " +
                                         " AND A.Cor_State_ID = B.ID AND A.Cast_Category_ID=C.ID group by B.Name, a.Cast_Category_ID, C.Name, Gender," +
                                         " DATEPART(year,a.Date_of_Exam)) STAT group by State,Exam_Year order by  Exam_Year, State");
                        }
                     }
                }
            
            else { ShowAlert("Service not available !! Contact Administrator."); }

            dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
            string sheetname = ddlCourseCategry.SelectedItem.Text.Replace("/", " ") + "-" + ddlCourseName.SelectedItem.Text.Replace("/", "_") + "-" + DdlReportType.SelectedItem.Text + "-" + DDlApplicationStatus.SelectedItem.Text; ;
            if (dt.Rows.Count > 0)
            {
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + sheetname + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);
                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            else
            {
                ShowAlert("No record found.");
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }

    }

}


