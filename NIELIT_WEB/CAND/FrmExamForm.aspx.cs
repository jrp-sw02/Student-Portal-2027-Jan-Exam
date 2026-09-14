using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Transactions;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

public partial class FrmExamForm : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentevisionNumber = 0;
    public static Int32 remainingTheoryModuleCountspeclGroupAlevel = 0;
    Int32 preevisionNumber = 0;
    Int32 currentCourseID = 0;
    Int64 registrationNumber = 0;
    Int32 applicantTypeID = 0;
    Int16 moduleCount_CourceId_3 = 0;
    Int16 splOLevelCondition = 0;
    public static Int32 ALevel_OffLineExamCenterOption_Theory;
    public static Int32 ALevel_OffLineExamCenterOption_Pract;
    Int32[] regStatusID ={Convert.ToInt32(enmRegistrationStatus.Registered),
                          Convert.ToInt32(enmRegistrationStatus.ReRegistered)};
    Course currentCourse;
    Exam attemptedLastExams;
    IQueryable<Exam> lastExams;
    Int32 exceptional_exam_id;
    Int32 lastexam;
    DateTime examStartDate;
    Int32 candidate_lastexam;
    string Rv;
   public static Int16 Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL = 0;
   public static Int16 Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL = 0;
   public static Int16 Eligibility_Method_For_Apply_In_Old_Exam_Pattren_B_LVL = -1;

   readonly int[] offlineModules_A = { 942,943,944,945,946,947,948,949,950,951,952,953,954,955};
   readonly int[] offlineModules_B = { 382, 383, 384, 385, 386, 387, 388, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398, 399, 400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 1117, 1118, 1119, 1120, 1121, 1122, 1123, 1124, 1125, 1126, 1127, 1128, 1129, 1130, 1131, 1132, 1133, 1134, 1135, 1136, 1137, 1138, 1143 };
   readonly int[] offlineModules_C = { 429, 430, 431, 432, 433, 434, 435, 436, 437, 438, 439, 440, 441, 442, 443 };
   readonly int[] onlineModules_O = { 929, 930, 931, 932 };
   readonly int[] onlineModules_A = { 938, 939, 940, 941 };
   readonly int[] practicalModules_O = { 933, 934, 935, 936 };
   readonly int[] practicalModules_A = { 956, 957, 958, 959, 960 };
   readonly int[] practicalModules_B = { 418, 419,420,421,1139,1140,1141 };
   readonly int[] practicalModules_C = { 444,445,446,447};

    //Int32 k = 0;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        
        {

            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Home.aspx");
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(1);

            //if (Request.UrlReferrer == null)
            ////if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in") && (Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "https://nielit.gov.in"))
            //{
            //    Response.Write(GeInvalidRequestMessage("Goto Home Page", Server.MapPath("../Index.aspx")));
            //    Response.End();
            //    return;
            //}

            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            examStartDate = Convert.ToDateTime(Request.QueryString["examStartDate"]);
           // examStartDate ='08/07/2023';
            //entityID = 1413670;

            currentCourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            //currentCourseID = 115;

            registrationNumber = Convert.ToInt64(Request.QueryString["RegNo"]);
            //  registrationNumber = 1393908;

           
            if (currentCourseID == 1)
            {
                Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL = 0;
                if (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 0)
                {
                    chklistPracticals.Enabled = false;
                }
            }
            //if (currentCourseID == 1)///Reena
            //{
            //    tr_select_module.Visible = false;//Added by kismat on 10May23
            //    rbtn_module_selection.Visible = false;
            //   // TrOnlTheoryExamCenter1.Visible = true;
            //   // TrOnlTheoryExamCenter2.Visible = true;

            //   // TrPracExamCenter1.Visible = true;
            //   // TrPracExamCenter2.Visible = true;f

            //    TrExamCentre1.Visible = false;
            //    TrExamCentre2.Visible = false;

            //    LblOnlSI1.Text = "2.1";
            //    LblOnlSI2.Text = "2.2";
            //    LblPracSI1.Text = "2.3";
            //    LblPracSI2.Text = "2.4"; 
            //    //

            //    //Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL = CourseManager.Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL(registrationNumber);
            //    Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL = 0;
            //    if (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 0)
            //    {
            //        chklistPracticals.Enabled = false;
            //    }
            //}
           if (currentCourseID == 1213)
            {
                chklistPracticals.Enabled = false;
                TrExamCentre1.Visible = true;
                TrExamCentre2.Visible = true;
                LblOnlSI1.Text = "2.1";
                LblOnlSI2.Text = "2.2";
                LblPracSI1.Text = "2.3";
                LblPracSI2.Text = "2.4"; 
            }
           else if (currentCourseID == 2)
           {
               Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL = 0;
           }
           //else if (currentCourseID == 2)///Reena
           //{
           //    tr_select_module.Visible = false;//Added by kismat on 10May23
           //    rbtn_module_selection.Visible = false;
           //    ///Reena
           //   // TrOnlTheoryExamCenter1.Visible = true;
           //   // TrOnlTheoryExamCenter2.Visible = true;

            //    //TrPracExamCenter1.Visible = true;
           //   // TrPracExamCenter2.Visible = true;

            //    //TrExamCentre1.Visible = true;
           //   // TrExamCentre2.Visible = true;

            //    LblSiOff1.Text = "2.1";
           //    LblSiOff2.Text = "2.2";
           //    LblOnlSI1.Text = "2.3";
           //    LblOnlSI2.Text = "2.4";
           //    LblPracSI1.Text = "2.5";
           //    LblPracSI2.Text = "2.6"; 
           //    //
           //    //Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL = CourseManager.Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL(registrationNumber);
           //    Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL = 0;
           //}
           else if (currentCourseID == 3)
           {
               Eligibility_Method_For_Apply_In_Old_Exam_Pattren_B_LVL = Eligibility_For_Apply_In_Old_Exam_Pattren_B_LVL(registrationNumber, currentCourseID, entityID, currentevisionNumber);
           }
           //else if (currentCourseID == 3) ///Reena
           //{

            //   // TrExamCentre1.Visible = true;
           //    //TrExamCentre2.Visible = true;

            //   // TrPracExamCenter1.Visible = true;
           //   // TrPracExamCenter2.Visible = true;


            //    LblSiOff1.Text = "2.1";
           //    LblSiOff2.Text = "2.2";
           //    LblPracSI1.Text = "2.3";
           //    LblPracSI2.Text = "2.4";

            //    Eligibility_Method_For_Apply_In_Old_Exam_Pattren_B_LVL = Eligibility_For_Apply_In_Old_Exam_Pattren_B_LVL(registrationNumber, currentCourseID, entityID, currentevisionNumber);
           //}

            //else if (currentCourseID == 4) ///Reena
           //{

            //    //TrExamCentre1.Visible = true;
           //    //TrExamCentre2.Visible = true;

            //    //TrPracExamCenter1.Visible = true;
           //    //TrPracExamCenter2.Visible = true;

            //    LblSiOff1.Text = "2.1";
           //    LblSiOff2.Text = "2.2";
           //    LblPracSI1.Text = "2.3";
           //    LblPracSI2.Text = "2.4";

            //}

           else  //***********Added by Reena on 09/05/2023 for other courses handling*************//
           {
               TrExamCentre1.Visible = true;
               TrExamCentre2.Visible = true;
               LblSiOff1.Text = "3.1";
               LblSiOff2.Text = "3.2";
               LblPracSI1.Text = "2.3";
               LblPracSI2.Text = "2.4";
           }
           
            if (!IsPostBack)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    //Added to let candiadte to fill the exam form of July . should remove  from  line 70  to 87 

                    var CDate = (from c in context.RegistrationDetails
                                 where c.RegistrationNo == registrationNumber && c.CourseID == currentCourseID
                                 select new { CommencementFromDate = c.CommencementFromDate }).FirstOrDefault();

                    if (CDate.CommencementFromDate > examStartDate.AddDays(-(double)(examStartDate.Day - 1)) && currentCourseID!=1213)
                    {
                        lblerror.Visible = true;
                        lblerror.Text = " You are not allowed to fill the Examination Form. Please check your Commencement Date  or Registration Validity in Registration Detail.";
                        tblMain.Visible = false;
                        btnSave.Visible = false;
                        trModuleOption1.Visible = false;
                        trModuleOption.Visible = false;
                        trModuleHead.Visible = false;
                        tr_select_module.Visible = false;
                        rbtn_module_selection.Visible = false;
                        //ShowAlert("You are not allowed to fill the Examination Form");
                        return;
                    }
                    else
                    {

                        if (!CourseManager.IsCandidateDebarred(currentCourseID, registrationNumber))
                        {
                            lblerror.Text = "You can not apply for this Exam because your candidature is debarred from appearing in subsequent two examinations of NIELIT.";
                            lblerror.Visible = true;
                            btnSave.Visible = false;
                            trModuleOption1.Visible = false;
                            trModuleOption.Visible = false;
                            trModuleHead.Visible = false;
                            tr_select_module.Visible = false;
                            rbtn_module_selection.Visible = false;
                            return;
                        }

                        if (!context.Candidates.Any(c => (c.IsLocked == true && c.ID == entityID)))
                        {
                            lblerror.Text = "You can not apply for exam.<br>Your profile details are not completed/locked yet. Please first complete your profile details and lock it. <br> <a href=" + EConnect.Utils.Security.QuertStringModule.Encrypt("myprofile.aspx") + "> Click here to view profile</a>";
                            lblerror.Visible = true;
                            btnSave.Visible = false;
                            tblMain.Visible = false;
                            return;
                        }

                        ////-------Start-----------for fetching previous and curret revision number from database automatically----------------------------------------

                        var revision_text = (from s in context.RevisionChoices
                                             where s.course_id == currentCourseID && s.whether_show_revision_choice == "Y" && s.show_revision_choice_till_date >= DateTime.Now
                                             orderby (s.revision_choice_effective_date) descending
                                             select new { previous_revision_number = s.previous_revision_number, new_revision_number = s.new_revision_number }).FirstOrDefault();


                        if (CourseManager.IsCourseReviesd(currentCourseID))
                        {
                            //lblRevision.Text = currentevisionNumber.ToString() + "<sup>th</sup> Revision) ";
                            //rbtn_module_selection.Items.FindByValue("pre").Text = "Revision " + revision_text.previous_revision_number.ToString();
                            //rbtn_module_selection.Items.FindByValue("current").Text = "Revision " + revision_text.new_revision_number.ToString();

                            rbtn_module_selection.Items.FindByValue("pre").Text = "Old Pattern";
                            rbtn_module_selection.Items.FindByValue("current").Text = "New Pattern";
                        }
                        //------------------------------------------------------------------------------------------------------------------------------------------END

                        currentCourse = context.Courses.Find(currentCourseID);
                        Int32 ShortermCoursesCategory = context.CourseCategories.Where(s => s.Code.ToUpper() == "STC").FirstOrDefault().ID;
                        EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlCandidateType, typeof(enmApplicantType), null);
                        if (ddlCandidateType.Items[0].Value == Convert.ToInt32(enmApplicantType.Direct).ToString())
                            ddlCandidateType.Items[0].Text = "As Direct Candidate";
                        if (ddlCandidateType.Items[1].Value == Convert.ToInt32(enmApplicantType.Institute).ToString())
                            ddlCandidateType.Items[1].Text = "Through Accredited Institute";
                        lbllevel.Text = currentCourse.Name;
                        var registrationDetail = (from s in context.RegistrationDetails
                                                  where s.CourseID == currentCourseID && s.CandidateID == entityID && s.RegistrationNo == registrationNumber && (s.RegistrationStatus.ID == 1 || s.RegistrationStatus.ID == 2 || s.RegistrationStatus.ID == 4)
                                                  select s).FirstOrDefault();
                        if (registrationDetail != null)
                        {
                            //Declaration
                            Candidate candidate = registrationDetail.Candidate;
                            lblCandidateName.Text = candidate.Salutation + " " + GetInitCap(candidate.Name);
                            lblRegNumber.Text = registrationNumber.ToString();

                            lblName.Text = GetInitCap(candidate.Name);

                            if (candidate.Gender == "Male")
                                lblName.Text += " S/o ";
                            else
                                lblName.Text += " D/o ";
                            if (!string.IsNullOrEmpty(candidate.FatherName))
                                lblName.Text += GetInitCap(candidate.FatherName);
                            else if (!string.IsNullOrEmpty(candidate.GuardianName))
                                lblName.Text += GetInitCap(candidate.GuardianName);
                            else
                                lblName.Text += " NA ";
                            if (registrationDetail.enmApplicantType == enmApplicantType.Direct)
                                lblCandidateType.Text = " as a Direct candidate ";
                            else
                            {
                                //Institute institute = registrationDetail.Institute;
                                Institute institute = registrationDetail.Institute;
                                if (institute != null)
                                {
                                    lblCandidateType.Text = " through Accredited Institute namely " + institute.Name + " ACCR No. " + currentCourse.Code + "-" + institute.AccreditationDetails.Where(d => d.CourseID == currentCourseID).FirstOrDefault().AccreditationNumber;
                                    lblCandidateType1.Text = lblCandidateType.Text;
                                }
                            }

                            //modification in declaration
                            if (currentCourse.CourseCategoryID == ShortermCoursesCategory)
                            {
                                header1.Visible = false;
                                header2.Visible = true;
                            }
                            else
                            {
                                header1.Visible = true;
                                header2.Visible = false;
                            }
                            applicantTypeID = registrationDetail.ApplicantTypeID;
                            Exam exam = CourseManager.GetNextExam(context, currentCourseID, applicantTypeID);

                            Int32 max_revision = (from p in context.Modules
                                                  where p.CourseID == currentCourseID
                                                  select p.RevisionNumber).Distinct().Max();
                            //  calculate max Exam Pattern

                            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<=============================For July2020 revision change====================================================
                            Int32 application_status = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                            exceptional_exam_id = context.ExamCycleExceptionalFeatures.Where(s => s.whether_effective == "Y" && s.course_id == currentCourseID && System.Data.Entity.DbFunctions.TruncateTime(s.effective_upto_date) > System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)).Select(s => s.exam_id).Distinct().FirstOrDefault();
                            //Exam lastExam_attempted_exceptionalcase = CourseManager.GeLastExam(context, currentCourseID, applicantTypeID);
                            int candidate_attempted_last_exam = (from s in context.CourseExamApplicationDetails
                                                                 join ee in context.Exams on s.ExamID equals ee.ID
                                                                 join ce in context.CourseExamApplications on s.CourseExamApplicationID equals ce.ID
                                                                 where s.CourseID == currentCourseID && s.RegistrationNumber == registrationNumber && s.CandidateID == entityID
                                                                     //&& s.ExamID.HasValue == true
                                                                 && ce.ApplicationStatusID == application_status
                                                                 select ce.ExamID).DefaultIfEmpty(0).Distinct().Max();

                            int revisionNumber_with_nochoice = (from s in context.CourseExamApplicationDetails
                                                                join m in context.Modules on s.ModuleID equals m.ID
                                                                where (s.CourseID == currentCourseID && s.CandidateID == entityID &&
                                                                        s.RegistrationNumber == registrationNumber && s.ExamID == exceptional_exam_id)
                                                                orderby m.RevisionNumber, m.Code
                                                                select m.RevisionNumber).DefaultIfEmpty(0).Distinct().Max();
                            //lastexam = lastExam_attempted_exceptionalcase == null ? 0 : lastExam_attempted_exceptionalcase.ID;
                            candidate_lastexam = candidate_attempted_last_exam == 0 ? 0 : candidate_attempted_last_exam;

                            //=====================================================================For July2020==========================>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                            if ((regStatusID.Contains(registrationDetail.RegistrationStatusID.Value) || (registrationDetail.RegistrationStatusID.Value == 4 && registrationDetail.CourseID == 2)) && registrationDetail.ValidUptoDate >= DateTime.Now.Date) // coded on dated 02052024 by abhi singh 
                            {
                                //<<<<<<<<<<<<<<<<<<<<<<=============================For July2020 revision change=======================================
                                if (exceptional_exam_id == candidate_lastexam && exceptional_exam_id != 0)
                                {
                                    currentevisionNumber = revisionNumber_with_nochoice;
                                }

                                //=====================================================For July2020==========================>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == max_revision)
                                //if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == max_revision)
                                {
                                    currentevisionNumber = max_revision; ;
                                    preevisionNumber = currentevisionNumber - 1;
                                }
                                else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) != max_revision)
                                {
                                    if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                    {
                                        if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0)
                                        {
                                            currentevisionNumber = max_revision;
                                            preevisionNumber = currentevisionNumber - 1;
                                        }
                                        else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                        {
                                            currentevisionNumber = CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID);
                                            preevisionNumber = currentevisionNumber;
                                        }
                                    }
                                    else
                                    {
                                        currentevisionNumber = max_revision;
                                        preevisionNumber = currentevisionNumber - 1;
                                    }
                                }
                                else
                                {
                                    currentevisionNumber = max_revision;
                                    preevisionNumber = currentevisionNumber - 1;
                                }
                                //Exam exam = CourseManager.GetNextExam(context, currentCourseID, applicantTypeID);
                                if (exam != null)
                                {
                                    if (context.ExamTimeTables.Any(t => t.CourseID == currentCourseID && t.ExamID == exam.ID))
                                    {
                                        lblExamName.Text = exam.Name.ToUpper();
                                        lblExamName1.Text = exam.Name.ToUpper();
                                        //lastExams = CourseManager.GetListOfAttemptedExams(context, currentCourseID, registrationNumber, entityID).Where(c => c.ID != exam.ID );
                                        lastExams = CourseManager.GetListOfTheoryPassed(context, currentCourseID, registrationNumber, entityID).Where(c => c.ID != exam.ID);
                                        attemptedLastExams = lastExams.OrderByDescending(d => new { d.ExamYear, d.ExamMonth }).FirstOrDefault();
                                        if (attemptedLastExams != null)
                                            lblPreviousExam.Text = attemptedLastExams.Name.ToUpper();

                                        ddlCandidateType.SelectedValue = registrationDetail.ApplicantTypeID.ToString();
                                        if (registrationDetail.enmApplicantType == enmApplicantType.Direct)
                                        {
                                            ddlCandidateType.Enabled = false;
                                            ddlPaymentOption.SelectedValue = "1";
                                            ddlPaymentOption.Enabled = false;
                                        }
                                        else
                                        {
                                            ddlPaymentOption.SelectedValue = "2";
                                            ddlPaymentOption.Enabled = true;
                                            Int32 totalExamsAttempted = lastExams.Count();
                                            if (currentCourseID == 1 || currentCourseID == 2)
                                            {
                                                if (totalExamsAttempted >= 2)
                                                    ddlCandidateType.Enabled = true;
                                                else
                                                    ddlCandidateType.Enabled = false;
                                            }
                                            else
                                            {
                                                if (totalExamsAttempted >= 6)
                                                    ddlCandidateType.Enabled = true;
                                                else
                                                    ddlCandidateType.Enabled = false;
                                            }
                                        }

                                        //Exam Center

                                        if (registrationDetail.CourseCategoryID == 6)
                                        {
                                            Institute institute = registrationDetail.Institute;

                                            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
                                            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();

                                            cnn.Open();

                                            //using (SqlCommand cmd = new SqlCommand("select  id, name from Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = '" + institute.StateID + "'))"))
                                            
                                            //November_2024
                                            using (SqlCommand cmd = new SqlCommand("select  id, name from Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = @instituteStateID ))"))
                                            {
                                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                                {
                                                    cmd.Connection = cnn;
                                                    //November_2024
                                                    cmd.Parameters.AddWithValue("@instituteStateID", institute.StateID);
                                                    sda.SelectCommand = cmd;
                                                    using (DataTable dTable = new DataTable())
                                                    {
                                                        sda.Fill(dTable);

                                                        //ddlStateFirst.DataSource = dTable;
                                                        //ddlStateFirst.DataBind();

                                                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlStateFirst, dTable, new ListItem("--Select Location--", "0"));
                                                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlStateSecond, dTable, new ListItem("--Select Location--", "0"));

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var examcentre = (from p in context.ExamCenters
                                                              join c in context.ExamWiseExamCenters on p.ID equals c.ExamCenterID
                                                              join s in context.Locations on p.StateID equals s.ID
                                                              where (p.CourseCategoryID == currentCourse.CourseCategoryID || p.CourseID == currentCourseID)
                                                              && c.ExamID == exam.ID
                                                              select new { ValueField = s.ID, TextField = s.Name.ToUpper() }).Distinct().OrderBy(s => s.TextField);
                                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlStateFirst, examcentre, new ListItem("--Select Location--", "0"));
                                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlStateSecond, examcentre, new ListItem("--Select Location--", "0"));

                                        }
                                        //Added By Reena
                                        if (registrationDetail.CourseCategoryID == 6 || registrationDetail.CourseCategoryID == 3)
                                        {
                                            Institute institute = registrationDetail.Institute;

                                            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
                                            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();

                                            cnn.Open();

                                            //using (SqlCommand cmd = new SqlCommand("select id, name from Prac_Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = '" + institute.StateID + "'))"))
                                            
                                            //November_2024
                                            using (SqlCommand cmd = new SqlCommand("select id, name from Prac_Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = @instituteStateID ))"))
                                            {
                                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                                {
                                                    cmd.Connection = cnn;
                                                    //November_2024
                                                    cmd.Parameters.AddWithValue("@instituteStateID", institute.StateID);
                                                    sda.SelectCommand = cmd;
                                                    using (DataTable dTable = new DataTable())
                                                    {
                                                        sda.Fill(dTable); 

                                                        //ddlStateFirst.DataSource = dTable;
                                                        //ddlStateFirst.DataBind();

                                                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlPracStateFirst, dTable, new ListItem("--Select Location--", "0"));
                                                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlPracStateSecond, dTable, new ListItem("--Select Location--", "0"));

                                                    }
                                                }
                                            }
                                        }
                                       else
                                        {
                                            var examcentre = (from p in context.PracExamCenters
                                                              //join c in context.ExamWiseExamCenters on p.ID equals c.ExamCenterID
                                                              join s in context.PracLocations on p.StateID equals s.ID
                                                              where (p.CourseCategoryID == currentCourse.CourseCategoryID )
                                                              //&& c.ExamID == exam.ID
                                                              select new { ValueField = s.ID, TextField = s.Name.ToUpper() }).Distinct().OrderBy(s => s.TextField);
                                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlPracStateFirst, examcentre, new ListItem("--Select Location--", "0"));
                                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlPracStateSecond, examcentre, new ListItem("--Select Location--", "0"));

                                        }
                                        //Added by Reena on (03/05/2023)
                                        if (registrationDetail.CourseCategoryID == 6)
                                        {
                                            Institute institute = registrationDetail.Institute;

                                            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
                                            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();

                                            cnn.Open();

                                            //using (SqlCommand cmd = new SqlCommand("select  id, name from Online_Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = '" + institute.StateID + "'))"))

                                            //Novermber_2024
                                            using (SqlCommand cmd = new SqlCommand("select  id, name from Online_Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = @instituteStateID ))"))
                                            {
                                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                                {
                                                    cmd.Connection = cnn;
                                                    //November_2024
                                                    cmd.Parameters.AddWithValue("@instituteStateID", institute.StateID);
                                                    sda.SelectCommand = cmd;
                                                    using (DataTable dTable = new DataTable())
                                                    {
                                                        sda.Fill(dTable); 

                                                        //ddlStateFirst.DataSource = dTable;
                                                        //ddlStateFirst.DataBind();

                                                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlOnlTheoryExamStateFirst, dTable, new ListItem("--Select Location--", "0"));
                                                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlOnlTheoryExamStateSecond, dTable, new ListItem("--Select Location--", "0"));

                                                    }
                                                }
                                            }
                                        }
                                        else if (registrationDetail.CourseCategoryID == 3) // added by abhi singh dated on 
                                        {
                                            Institute institute = registrationDetail.Institute;

                                            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
                                            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();

                                            cnn.Open();

                                            //using (SqlCommand cmd = new SqlCommand("select  id, name from Online_Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = '" + institute.StateID + "'))"))

                                            //November_2024
                                            using (SqlCommand cmd = new SqlCommand("select id, name from Online_Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = @instituteStateID ))"))
                                            {
                                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                                {
                                                    cmd.Connection = cnn;
                                                    //November_2024
                                                    cmd.Parameters.AddWithValue("@instituteStateID", institute.StateID);
                                                    sda.SelectCommand = cmd;
                                                    using (DataTable dTable = new DataTable())
                                                    {
                                                        sda.Fill(dTable);

                                                        //ddlStateFirst.DataSource = dTable;
                                                        //ddlStateFirst.DataBind();

                                                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlOnlTheoryExamStateFirst, dTable, new ListItem("--Select Location--", "0"));
                                                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlOnlTheoryExamStateSecond, dTable, new ListItem("--Select Location--", "0"));

                                                    }
                                                }
                                            }
                                        }
                                        else
                                       {
                                            var examcentre = (from p in context.OnlineExamCenters
                                                              //join c in context.ExamWiseExamCenters on p.ID equals c.ExamCenterID
                                                              join s in context.OnlineLocations on p.StateID equals s.ID
                                                              where (p.CourseCategoryID == currentCourse.CourseCategoryID)
                                                              //&& c.ExamID == exam.ID
                                                              select new { ValueField = s.ID, TextField = s.Name.ToUpper() }).Distinct().OrderBy(s => s.TextField);
                                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlOnlTheoryExamStateFirst, examcentre, new ListItem("--Select Location--", "0"));
                                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlOnlTheoryExamStateSecond, examcentre, new ListItem("--Select Location--", "0"));

                                        }
                                        //Language option
                                        CourseRegistrationPolicy currentPolicy = CourseManager.GetCurrentRegistrationPolicy(context, currentCourseID);
                                        if (currentPolicy.AllowedLanguage.HasValue)
                                        {
                                            rblMedium.SelectedValue = currentPolicy.AllowedLanguage.Value.ToString();
                                            rblMedium.Enabled = false;
                                        }

                                        //Fee Details
                                        lblTheoryFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee).ToString("F");
                                        lblPracticalFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee).ToString("F");
                                        lblLateFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.LateFeeExam).ToString("F");
                                        lblProcessingFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PostageFeeChargedTowardExaminationCorrespondence).ToString("F");
                                        lblTotalProcessingFee.Text = lblProcessingFee.Text;
                                        if (CourseManager.IsLateFeeApplicable(context, exam.ID, applicantTypeID))
                                            lblTotalLateFee.Text = lblLateFee.Text;

                                        FillRemianingModules(currentCourseID, registrationNumber, entityID, applicantTypeID, exceptional_exam_id, candidate_lastexam, 0);

                                        //<<<<<<<<<<<<<<<<<<<<<<=============================For July2020 revision change=======================================
                                        if (exceptional_exam_id == candidate_lastexam && exceptional_exam_id != 0)
                                        {
                                            currentevisionNumber = revisionNumber_with_nochoice;
                                            tr_select_module.Visible = false;
                                            rbtn_module_selection.Visible = false;
                                        }

                                        //===================================================== For July2020 ==========================>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                        else if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                        //if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                        {
                                            if (CourseManager.IsCourseReviesd(currentCourseID))
                                            {
                                                tr_select_module.Visible = true;//*****
                                                rbtn_module_selection.Visible = true;
                                                 
                                                if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0 ||
                                                    IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == 0)
                                                {
                                                    //tr_select_module.Visible = false;//*****
                                                    trModuleOption.Visible = false;
                                                    trModuleHead.Visible = false;
                                                    trModuleOption1.Visible = false;
                                                }
                                                else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                    CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                {
                                                    //tr_select_module.Visible = false;//*****
                                                    trModuleOption.Visible = true;
                                                    trModuleHead.Visible = true;
                                                    trModuleOption1.Visible = true;
                                                }
                                                else if (IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                        IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                {
                                                    //tr_select_module.Visible = false;//*****
                                                    trModuleOption.Visible = true;
                                                    trModuleHead.Visible = true;
                                                    trModuleOption1.Visible = true;
                                                }
                                                else
                                                {
                                                    //tr_select_module.Visible = false;//*****
                                                    trModuleOption.Visible = false;
                                                    trModuleHead.Visible = false;
                                                    trModuleOption1.Visible = false;
                                                    if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0)
                                                    {
                                                        rbtn_module_selection.SelectedIndex = -1;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (lblerror.Text == "You can not apply for exam as all theory/bridge/practical modules have been passed."
                                                    || lblerror.Text == "You have already applied for this exam.")
                                                {
                                                    tr_select_module.Visible = false;
                                                    rbtn_module_selection.Visible = false;
                                                    trModuleOption.Visible = false;
                                                    trModuleHead.Visible = false;
                                                    trModuleOption1.Visible = false;
                                                }
                                                else
                                                {
                                                    int countPassedCodModule = 0;
                                                    int countPassedCodModule_alevel = 0;
                                                    countPassedCodModule = PassedConditionalTheoryModuleCount(currentCourseID, registrationNumber,currentevisionNumber, entityID);
                                                    countPassedCodModule_alevel = PassedConditionalTheoryModuleCount_Alevel(currentCourseID, registrationNumber, currentevisionNumber, entityID);
                                                    if ((countPassedCodModule != 0 && CheckExemptioDoneOrNot(currentCourseID, registrationNumber, currentevisionNumber, entityID) < 1) || (countPassedCodModule_alevel != 0 && CheckExemptioDoneOrNot(currentCourseID, registrationNumber, currentevisionNumber, entityID) < 1 && A1toA8passedmoduleCount(registrationNumber, entityID) < 8))
                                                    {
                                                        tr_select_module.Visible = false;
                                                        rbtn_module_selection.Visible = false;
                                                        trModuleOption.Visible = true;
                                                        trModuleHead.Visible = true;
                                                        trModuleOption1.Visible = true;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //commented by me as on 02022023
                                            //if ((currentCourseID == 1 || currentCourseID == 2 || currentCourseID == 3) && (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 1 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL == 1 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_B_LVL == 1))
                                            if ((currentCourseID == 1 && Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 1) || (currentCourseID == 2 && Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL == 1) || (currentCourseID == 3 && Eligibility_Method_For_Apply_In_Old_Exam_Pattren_B_LVL == 1)) 
                                            //--if ((currentCourseID == 1 || currentCourseID == 2 || currentCourseID == 3))
                                            {
                                                //tr_select_module.Visible = true;//*****
                                               // rbtn_module_selection.Visible = true;
                                            }
                                            else
                                            {
                                                tr_select_module.Visible = false;
                                                rbtn_module_selection.Visible = false;
                                            }
                                            trModuleOption.Visible = true;
                                            trModuleHead.Visible = true;
                                            trModuleOption1.Visible = true;
                                        }

                                        if (context.CourseExamApplications.Any(c => (c.ExamID == exam.ID && c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber)))
                                        {
                                            if (Request.QueryString["Appid"] == null)
                                            {
                                                lblerror.Text = "You have already applied for this exam but not submitted finally.<br>Please change your exam details (if you want to change ) and click on Update button to view the preview. ";
                                                chkdisclamier.Checked = true;
                                                //<<<<<<<<<<<<<<<<<<<<<<=============================For July2020 revision change=======================================
                                                if (exceptional_exam_id == candidate_lastexam && exceptional_exam_id != 0)
                                                {
                                                    //currentevisionNumber = revisionNumber_jul20;
                                                    tr_select_module.Visible = false;
                                                    rbtn_module_selection.Visible = false;
                                                }
                                                //=====================================================For July2020==========================>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                                else if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                //if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                {
                                                    if (CourseManager.IsCourseReviesd(currentCourseID))
                                                    {
                                                       //-- tr_select_module.Visible = true;//*****
                                                        //--rbtn_module_selection.Visible = true;

                                                        if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0 ||
                                                            IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == 0)
                                                        {
                                                            //tr_select_module.Visible = false;//*****
                                                            trModuleOption.Visible = false;
                                                            trModuleHead.Visible = false;
                                                            trModuleOption1.Visible = false;
                                                        }
                                                        else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                            CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                        {
                                                            //tr_select_module.Visible = false;//*****
                                                            trModuleOption.Visible = true;
                                                            trModuleHead.Visible = true;
                                                            trModuleOption1.Visible = true;
                                                        }
                                                        else if (IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                                IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                        {
                                                            //tr_select_module.Visible = false;//*****
                                                            trModuleOption.Visible = true;
                                                            trModuleHead.Visible = true;
                                                            trModuleOption1.Visible = true;
                                                        }
                                                        else
                                                        {
                                                            //tr_select_module.Visible = false;//*****
                                                            trModuleOption.Visible = false;
                                                            trModuleHead.Visible = false;
                                                            trModuleOption1.Visible = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        tr_select_module.Visible = false;
                                                        rbtn_module_selection.Visible = false;
                                                        trModuleOption.Visible = true;
                                                        trModuleHead.Visible = true;
                                                        trModuleOption1.Visible = true;
                                                    }
                                                }
                                                else
                                                {
                                                    tr_select_module.Visible = false;
                                                    rbtn_module_selection.Visible = false;
                                                    trModuleOption.Visible = true;
                                                    trModuleHead.Visible = true;
                                                    trModuleOption1.Visible = true;
                                                }
                                            }
                                            else
                                            {
                                                btnback.Visible = false;
                                                lblerror.Text = "You can change your exam details. Please click on Update button after making changes.";
                                                //<<<<<<<<<<<<<<<<<<<<<<=============================For July2020 revision change=======================================
                                                if (exceptional_exam_id == candidate_lastexam && exceptional_exam_id != 0)
                                                {
                                                    //currentevisionNumber = revisionNumber_jul20;
                                                    tr_select_module.Visible = false;
                                                    rbtn_module_selection.Visible = false;
                                                }
                                                //=====================================================For July2020==========================>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                                else if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                //if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                {
                                                    if (CourseManager.IsCourseReviesd(currentCourseID))
                                                    {
                                                        //--tr_select_module.Visible = true;//*****
                                                        //--rbtn_module_selection.Visible = true;

                                                        if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0 ||
                                                            IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == 0)
                                                        {
                                                            //tr_select_module.Visible = false;//*****
                                                            trModuleOption.Visible = false;
                                                            trModuleHead.Visible = false;
                                                            trModuleOption1.Visible = false;
                                                        }
                                                        else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                             CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                        {
                                                            //tr_select_module.Visible = false;//*****
                                                            trModuleOption.Visible = true;
                                                            trModuleHead.Visible = true;
                                                            trModuleOption1.Visible = true;
                                                        }
                                                        else if (IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                                IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                        {
                                                            //tr_select_module.Visible = false;//*****
                                                            trModuleOption.Visible = true;
                                                            trModuleHead.Visible = true;
                                                            trModuleOption1.Visible = true;
                                                        }
                                                        else
                                                        {
                                                            //tr_select_module.Visible = false;//*****
                                                            trModuleOption.Visible = false;
                                                            trModuleHead.Visible = false;
                                                            trModuleOption1.Visible = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        tr_select_module.Visible = false;
                                                        rbtn_module_selection.Visible = false;
                                                        trModuleOption.Visible = true;
                                                        trModuleHead.Visible = true;
                                                        trModuleOption1.Visible = true;
                                                    }
                                                }
                                                else
                                                {
                                                    tr_select_module.Visible = false;
                                                    rbtn_module_selection.Visible = false;
                                                    trModuleOption.Visible = true;
                                                    trModuleHead.Visible = true;
                                                    trModuleOption1.Visible = true;
                                                }
                                            }
                                            lblerror.Visible = true;
                                            Int64 applId = context.CourseExamApplications.Where(c => (c.ExamID == exam.ID && c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber)).FirstOrDefault().ID;
                                            ShowApplicationData(applId);
                                        }
                                    }
                                    else
                                    {
                                        lblerror.Text = "Time table not found for " + exam.Name + " exam. You can not apply for exam at this time.";
                                        lblerror.Visible = true;
                                        btnSave.Visible = false;
                                    }
                                }
                                else
                                {
                                    lblerror.Text = "No next exam found. You can not apply for exam at this time.";
                                    lblerror.Visible = true;
                                    btnSave.Visible = false;
                                }
                            }
                            else
                            {
                                lblerror.Text = "You are not eligible to apply for exam as your course status is completed/expired/project pending/cancelled.";
                                lblerror.Visible = true;
                                btnSave.Visible = false;
                                tblMain.Visible = false;
                            }
                        }
                        else
                        {
                            lblerror.Text = "You are not eligible to apply for exam this time.";
                            lblerror.Visible = true;
                            btnSave.Visible = false;
                        }
                    };
                }
            }
            if (splOLevelCondition == 1)
            {
               // lblCoutMessageOlevelSpecl.Text = "2.1";
                panelfeedetails.Visible = false;
                disclmrPanel.Visible = false;
                btnSave.Visible = false;
                btnExmpSubmit.Visible = false;
                FeedtlsId.Visible = false;
                disclamrdivid.Visible = false;
                trModuleOptionExmpt.Visible = false;
                trModuleOptionExmpt1.Visible = false;
                TrExamCentre1.Visible = false;
                TrExamCentre2.Visible = false;
                trImprovementHead.Visible = false;
                trModuleOption.Visible = false;
                trModuleOption1.Visible = false;
                trModuleOptionOlevelSpecl.Visible = true;
                trModuleOptionOlevelSpecl1.Visible = true;
                frty.Visible = true;
                TrOptionOlevelSpecl.Visible = true;
                ExamCenterChh.Visible = false;
                //TrExamCentre1.Visible = false;
                //TrExamCentre2.Visible = false;
            }
            
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    

    public static Int16 Eligibility_For_Apply_In_Old_Exam_Pattren_B_LVL(Int64 registrationNumber,int currentCourseID,Int64 Candidate_id,int currentevisionNumber)
    {
     
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                
                DateTime RegistrationDate = (from d in context.RegistrationDetails where d.RegistrationNo == registrationNumber && d.CourseID==3 select d.RegistrationDate).FirstOrDefault();
                DateTime CommencementFromDate = (from d in context.RegistrationDetails where d.RegistrationNo == registrationNumber && d.CourseID == 3 select d.CommencementFromDate).FirstOrDefault();
                if ((Convert.ToDateTime("2023-01-01 00:00:00.000") <= RegistrationDate && Convert.ToDateTime("2023-07-01 00:00:00.000") <= CommencementFromDate))
                {
                    return 0;
                }
                else if (CheckExemptioDoneOrNot(currentCourseID, registrationNumber, 5, Candidate_id) < 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
               
            }
          
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected Boolean IsProfileLocked(Int64 candidateID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                if (context.Candidates.Any(c => (c.IsLocked == true && c.ID == entityID)))
                    return true;
                else
                    return false;

                //int CorAddTypeId = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                //int PerAddTypeId = Convert.ToInt32(enmAddressType.PermanentAddress);
                //var IsNotNullCand = (from a in context.Candidates
                //                     where a.ID == candidateID && (
                //                     (a.FatherName != null && a.FatherName.Trim() != "" &&
                //                     a.MotherName != null && a.MotherName.Trim() != "") || (a.GuardianName != null && a.GuardianName.Trim() != "")) &&
                //                     a.Gender != null && a.Gender.Trim() != ""
                //                     select a);

                //var IsNotNullCorAddress = (from a in context.Addresses
                //                           where a.CandidateID == candidateID && a.AddressTypeID == CorAddTypeId &&
                //                           a.AddressLine1 != null && a.AddressLine1.Trim() != "" &&
                //                           a.CityName != null && a.CityName.Trim() != "" &&
                //                           a.DistrictID != null && a.DistrictID != 0 &&
                //                           a.StateID != null && a.StateID != 0 &&
                //                           a.PinCode != null && a.PinCode != 0 &&
                //                           a.EffectiveDateFrom >= a.EffectiveDateFrom
                //                           select a).Take(1);

                //var IsNotNullContact = (from a in context.CandidateContactDetails
                //                        where a.CandidateID == candidateID &&
                //                        a.EmailAddress != null && a.EmailAddress.Trim() != "" && a.IsEmailVerified == true &&
                //                        a.MobileNumber != null && a.MobileNumber != 0 && a.IsMobileNumberVerified == true
                //                        select a);
                //if (IsNotNullCand.Count() > 0 && IsNotNullContact.Count() > 0 && IsNotNullCorAddress.Count() > 0)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}

            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

   
  
    protected void FillRemianingModules(Int32 courseID, Int64 registrationNumber, Int64 candidateID, Int32 applicantTypeID, Int32? examID, Int32? lastExam_attempted_exceptionalcase, int AfterExempt)
    {
        using (EConnectContext context = new EConnectContext())
        {
            Exam exam = CourseManager.GetNextExam(context, currentCourseID, applicantTypeID);
            if (CourseManager.IsLateFeeApplicable(context, exam.ID, applicantTypeID))
                lblTotalLateFee.Text = lblLateFee.Text;          
            int revisionNumber ;
            if (rbtn_module_selection.SelectedValue == "")
            {
                if ((courseID == 1 || courseID == 2 || courseID == 3) && (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 0 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL == 0) || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_B_LVL == 0)
                {
                    revisionNumber = currentevisionNumber;
                }
                else if ((courseID == 1 || courseID == 2 || courseID == 3) && (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 1 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL == 1 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_B_LVL == 1))
                {
                    revisionNumber = preevisionNumber;
                }
                else
                {
                    revisionNumber = currentevisionNumber;
                }
            }
            else 
            {
                if (AfterExempt != 1)
                {
                    if ((courseID == 3) || courseID==2)
                    {
                        revisionNumber = currentevisionNumber;
                        int countPassedCodModule = 0;
                        int countPassedCodModule_alevel = 0;
                        countPassedCodModule = PassedConditionalTheoryModuleCount(currentCourseID, registrationNumber, revisionNumber, entityID);
                        countPassedCodModule_alevel = PassedConditionalTheoryModuleCount_Alevel(currentCourseID, registrationNumber, revisionNumber, entityID);

                        //trModuleOptionExmpt.Visible = true;
                        //trModuleOptionExmpt1.Visible = true;
                        //trModuleOption.Visible = false;
                        //trModuleOption1.Visible = false;

                        if ((countPassedCodModule != 0 && CheckExemptioDoneOrNot(courseID, registrationNumber, revisionNumber, candidateID) < 1) || (countPassedCodModule_alevel != 0 && CheckExemptioDoneOrNot(courseID, registrationNumber, revisionNumber, candidateID) < 1 && A1toA8passedmoduleCount(registrationNumber,entityID)<8))
                        {
                            lblserveExpt.Text = "2.1";
                            panelfeedetails.Visible = false;
                            disclmrPanel.Visible = false;
                            btnSave.Visible = false;
                            btnExmpSubmit.Visible = true;
                            FeedtlsId.Visible = false;
                            disclamrdivid.Visible = false;
                            trModuleOptionExmpt.Visible = true;
                            trModuleOptionExmpt1.Visible = true;
                            TrExamCentre1.Visible = false;
                            TrExamCentre2.Visible = false;
                            trImprovementHead.Visible = false;
                            trModuleOption.Visible = false;
                            trModuleOption1.Visible = false;
                          
                        }
                        else
                        {
                            //trModuleOptionExmpt.Visible = true;
                            //trModuleOptionExmpt1.Visible = true;
                            //trModuleOption.Visible = true;
                            //trModuleOption1.Visible = true;

                            lblserve1.Text = "2.1";
                            panelfeedetails.Visible = true;
                            disclmrPanel.Visible = true;
                            btnExmpSubmit.Visible = false;
                            btnSave.Visible = true;
                            FeedtlsId.Visible = true;
                            disclamrdivid.Visible = true;
                            trImprovementHead.Visible = true;
                            trModuleOptionExmpt.Visible = false;
                            trModuleOptionExmpt1.Visible = false;
                            TrExamCentre1.Visible = true;
                            TrExamCentre2.Visible = true;
                            trModuleOption.Visible = true;
                            trModuleOption1.Visible = true;
                        }
                    }
                    else
                    {
                        revisionNumber = preevisionNumber;
                        lblserve1.Text = "2.1";
                        panelfeedetails.Visible = true;
                        disclmrPanel.Visible = true;
                        btnSave.Visible = true;
                        btnExmpSubmit.Visible = false;
                        FeedtlsId.Visible = true;
                        disclamrdivid.Visible = true;
                        trImprovementHead.Visible = true;
                        trModuleOptionExmpt.Visible = false;
                        trModuleOptionExmpt1.Visible = false;
                        TrExamCentre1.Visible = true;
                        TrExamCentre2.Visible = true;
                        trModuleOption.Visible = true;
                        trModuleOption1.Visible = true;
                    }
                }
           else {
                    revisionNumber = currentevisionNumber;
                }     
                
            }
           
        
             //ICollection<Module> modules  = CourseManager.GetRemainingModules(context, courseID, registrationNumber, revisionNumber, entityID);
             ICollection<Module> modules = GetRemainingModules(context, courseID, registrationNumber, revisionNumber, entityID, AfterExempt);
             Int32 theory = Convert.ToInt32(enmModuleType.Theory);
             Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
             Int32 practical = Convert.ToInt32(enmModuleType.Practical);
             Int32 compulsory = Convert.ToInt32(enmSelectionType.Compulsory);
             var ModuleListTheory12 = modules.Where(m => m.ModuleTypeID == theory).ToList();
             int ModuleListPractCount = GetSpOLevelPassedPractical(registrationNumber, entityID);
             Int32 currentRevision = GetCourseRevisionNumberAtRegistrationCompleted1(context, courseID, registrationNumber, entityID);
             if (ModuleListTheory12.Count == 0 && courseID == 1 && currentRevision <= 5 && ModuleListPractCount==0)
             {
                 TrOptionOlevelSpecl.Visible = true;
                 Btncancelmodule.Visible = true;
                 btnback.Visible = false;
                 lblRevision.Text = currentRevision.ToString()+" Revision)";
                
                // var modulesd = (from m in context.Modules where m.CourseID == 1 && m.RevisionNumber == 5 && (m.ModuleTypeID==theory || m.ModuleTypeID==bridge) select m).Select(m => new { ValueField = m.ID, TextField = m.SelectionTypeID == compulsory ? ((m.ShortName + " &nbsp;&nbsp;" + m.Name)) + " (Compulsory)" : ((m.ShortName + " &nbsp;&nbsp;" + m.Name)) + " (<font style='color:red'>Elective</font>)" }).ToList();
                // var data1 =(from m in GetSpOLevelPassedModule(registrationNumber,currentevisionNumber,entityID) select new {Id=Convert.ToInt16( m.ToString())} ).ToList();
                 //var Modules = (from m in context.Modules join dm in data1 on m.ID equals dm.Id select m).ToList();
                 var moduleIds = GetSpOLevelPassedModule(registrationNumber, currentRevision, entityID);
                // var moduleIds = GetData(currentCourseID, registrationNumber, entityID, currentRevision);
                 var Modules = context.Modules.Where(m => moduleIds.Contains(m.ID)).Select(m => new { ValueField = m.ID, TextField = m.SelectionTypeID == compulsory ? ((m.ShortName + " &nbsp;&nbsp;" + m.Name)) + " (Compulsory)" : ((m.ShortName + " &nbsp;&nbsp;" + m.Name)) + " (<font style='color:red'>Elective</font>)" }).ToList();

                  EConnect.Utils.Common.ControlUtility.BindListObject(chklistModulesOlevelSpecl, Modules);
                 //newspclOlevelCond = 1;
                 splOLevelCondition = 1;
             }
            

             //Added to conditional visibility of offline centre selection based on module remianing

            // if (courseID == 1213)
            // {
            //     TrPracExamCenter1.Visible = true;
            //     TrPracExamCenter2.Visible = true;
            //     LblSiOff1.Text = "2.1";
            //     LblSiOff2.Text = "2.2";
            //     LblPracSI1.Text = "2.3";
            //     LblPracSI2.Text = "2.4";
            // }
            // if (courseID == 1)
            // {
            //     //Added to conditional visibility of online centre selection based on module remianing
            //     foreach (int on_O in onlineModules_O)
            //     {
            //         if (modules.Any(p => p.ID == on_O))
            //         {
            //             TrOnlTheoryExamCenter1.Visible = true;
            //             TrOnlTheoryExamCenter2.Visible = true;
            //             break;
            //         }
            //     }
            //     //Added to conditional visibility of practical centre selection based on module remianing
            //     foreach (int prac_O in practicalModules_O)
            //     {
            //         if (modules.Any(p => p.ID == prac_O))
            //         {
            //             TrPracExamCenter1.Visible = true;
            //             TrPracExamCenter2.Visible = true;
            //             break;
            //         }
            //     }
            // }
            // else if (courseID == 2)
            // {

            //     //Added to conditional visibility of offline centre selection based on module remianing
            //     foreach (int off_A in offlineModules_A)
            // {
            //     if (modules.Any(p => p.ID == off_A))
            //     {
            //         TrExamCentre1.Visible = true;
            //         TrExamCentre2.Visible = true;

            //         break;
            //     }
            // }
             
            //     //Added to conditional visibility of online centre selection based on module remianing
            //     foreach (int on_A in onlineModules_A)
            //     {
            //         if (modules.Any(p => p.ID == on_A))
            //         {
            //             TrOnlTheoryExamCenter1.Visible = true;
            //             TrOnlTheoryExamCenter2.Visible = true;
            //             break;
            //         }
            //     }
            //     //Added to conditional visibility of practical centre selection based on module remianing
            //     foreach (int prac_A in practicalModules_A)
            //     {
            //         if (modules.Any(p => p.ID == prac_A))
            //         {
            //             TrPracExamCenter1.Visible = true;
            //             TrPracExamCenter2.Visible = true;
            //             break;
            //         }
            //     }
            
            // }
            // else if (courseID == 3)
            // {

            //     //Added to conditional visibility of offline centre selection based on module remianing
            //     foreach (int off_B in offlineModules_B)
            //     {
            //         if (modules.Any(p => p.ID == off_B))
            //         {
            //             TrExamCentre1.Visible = true;
            //             TrExamCentre2.Visible = true;

            //             break;
            //         }
            //     }
            //     //Added to conditional visibility of practical centre selection based on module remianing
            //     foreach (int prac_B in practicalModules_B)
            //     {
            //         if (modules.Any(p => p.ID == prac_B))
            //         {
            //             TrPracExamCenter1.Visible = true;
            //             TrPracExamCenter2.Visible = true;
            //             break;
            //         }
            //     }
            // }
            // else if (courseID == 4)
            //{
            //    //Added to conditional visibility of offline centre selection based on module remianing
            //    foreach (int off_C in offlineModules_C)
            //    {
            //        if (modules.Any(p => p.ID == off_C))
            //        {
            //            TrExamCentre1.Visible = true;
            //            TrExamCentre2.Visible = true;

            //            break;
            //        }
            //    }
            //    //Added to conditional visibility of practical centre selection based on module remianing
            //    foreach (int prac_C in practicalModules_C)
            //    {
            //        if (modules.Any(p => p.ID == prac_C))
            //        {
            //            TrPracExamCenter1.Visible = true;
            //            TrPracExamCenter2.Visible = true;
            //            break;
            //        }
            //    }
            //}
            
             

			if (currentevisionNumber == 6 && courseID == 1)
            {

                bool b748 = modules.Any(p => p.ID == 929);
                if (b748 == false)
                {
                    Module moduleF = context.Modules.Find(933);
                    modules.Remove(moduleF);
                }

                bool b749 = modules.Any(p => p.ID == 930);
                if (b749 == false)
                {
                    Module moduleF = context.Modules.Find(934);
                    modules.Remove(moduleF);
                }
                bool b750 = modules.Any(p => p.ID == 931);
                if (b750 == false)
                {
                    Module moduleF = context.Modules.Find(935);
                    modules.Remove(moduleF);
                }
                bool b751 = modules.Any(p => p.ID == 932);
                if (b751 == false)
                {
                    Module moduleF = context.Modules.Find(936);
                    modules.Remove(moduleF);
                }

                //passedModulesOfCurrentRevision.ForEach(p => listOfModulesOfCurrentRevision.Remove(p));
            }

            else if (currentevisionNumber == 6 && courseID == 2)
            {
                bool b757 = modules.Any(p => p.ID == 938);
                if (b757 == false)
                {
                    Module moduleF = context.Modules.Find(956);
                    modules.Remove(moduleF);
                }

                bool b758 = modules.Any(p => p.ID == 939);
                if (b758 == false)
                {
                    Module moduleF = context.Modules.Find(957);
                    modules.Remove(moduleF);
                }
                bool b759 = modules.Any(p => p.ID == 940);
                if (b759 == false)
                {
                    Module moduleF = context.Modules.Find(958);
                    modules.Remove(moduleF);
                }
                bool b760 = modules.Any(p => p.ID == 941);
                if (b760 == false)
                {
                    Module moduleF = context.Modules.Find(959);
                    modules.Remove(moduleF);
                }

            }

	        //--moduleCount_CourceId_3=Convert.ToInt16( modules.Count());

            if (modules.Count() >= 0)
            {
                //Int32 theory = Convert.ToInt32(enmModuleType.Theory);
                //Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
                //Int32 practical = Convert.ToInt32(enmModuleType.Practical);

                Int32 max_revision = (from p in context.Modules
                                      where p.CourseID == currentCourseID
                                      select p.RevisionNumber).Distinct().Max();
                if ((CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0 ||
                    IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == 0) && rbtn_module_selection.SelectedValue == "")
                { rbtn_module_selection.SelectedIndex = -1; }
                else if (revisionNumber == max_revision)
                { rbtn_module_selection.SelectedIndex = 1; }
                else
                { rbtn_module_selection.SelectedIndex = 0; }
                Int32 mod = currentevisionNumber % 10;

                if (splOLevelCondition != 1)
                {
                    if (mod == 1)
                    {
                        lblRevision.Text = revisionNumber.ToString() + "<sup>st</sup> Revision) ";
                    }
                    else if (mod == 2)
                        lblRevision.Text = revisionNumber.ToString() + "<sup>nd</sup> Revision) ";
                    else if (mod == 3)
                        lblRevision.Text = revisionNumber.ToString() + "<sup>rd</sup> Revision) ";

                    else if (mod == 6)
                    {
                        Rv = "5.1";
                        lblRevision.Text = Rv + "<sup>th</sup> Revision) ";
                        revisionNumber = 6;
                    }
                    else
                    {
                        lblRevision.Text = revisionNumber.ToString() + "<sup>th</sup> Revision) ";
                    }

                }
               
               

                
                Int32 remainingTheoryModuleCount = 0;
                int countPassedCodModule = 0;
                int countPassedCodModule_alevel = 0;
                int ModuleListTheorsdExmptthoryCount = modules.Where(m => m.ModuleTypeID == theory || m.ModuleTypeID == bridge).ToList().Count();
                countPassedCodModule = PassedConditionalTheoryModuleCount(currentCourseID, registrationNumber, revisionNumber, entityID);
                countPassedCodModule_alevel = PassedConditionalTheoryModuleCount_Alevel(currentCourseID, registrationNumber, revisionNumber, entityID);
                if (courseID == 3)
                {
                   
                    if (countPassedCodModule != 0 && AfterExempt == 0 && CheckExemptioDoneOrNot(courseID, registrationNumber, revisionNumber, candidateID) < 1 )
                    {
                      remainingTheoryModuleCount = PassedConditionalTheoryModuleCount(currentCourseID, registrationNumber, revisionNumber, entityID);
                      if (remainingTheoryModuleCount > ModuleListTheorsdExmptthoryCount)
                          remainingTheoryModuleCount = ModuleListTheorsdExmptthoryCount;
                        lblCoutMessageExmpt.Text = "<b style='color:red'>You can avail Exemption upto " + remainingTheoryModuleCount.ToString() + " Modules among below Modules Listed in left panel.</b>";
                        lblCoutMessageExmpthidden.Value =remainingTheoryModuleCount.ToString();
                    }
                    else
                    {
                        remainingTheoryModuleCount = PassedConditionalTheoryModuleCount(currentCourseID, registrationNumber, revisionNumber, entityID);
                        lblCoutMessageExmpt.Text = "<b style='color:red'>You can avail Exemption upto " + remainingTheoryModuleCount.ToString() + " Modules among below Modules Listed in left panel.</b>";
                        remainingTheoryModuleCount = GetRemainingAllTheoryModuleCount(currentCourseID, registrationNumber, revisionNumber, entityID);
                        lblCoutMessage.Text = "Theory Papers  &nbsp;&nbsp;&nbsp;&nbsp; <b style='color:red'>You can select upto  " + remainingTheoryModuleCount.ToString() + " modules (remaining to pass) in the list below.</b>";
                       // lblCoutMessageExmpthidden.Value = remainingTheoryModuleCount.ToString();
                    }
                   
                }
                else if (courseID == 2)
                {
                    if (countPassedCodModule_alevel != 0 && CheckExemptioDoneOrNot(courseID, registrationNumber, revisionNumber, candidateID) < 1 && A1toA8passedmoduleCount(registrationNumber, candidateID) < 8)
                    {
                     
                         remainingTheoryModuleCount = PassedConditionalTheoryModuleCount_Alevel(currentCourseID, registrationNumber, revisionNumber, entityID);
                         lblCoutMessageExmpt.Text = "<b style='color:red'>You can avail Exemption upto " + remainingTheoryModuleCount.ToString() + " Modules among below Modules Listed in left panel.</b>";
                       //  lblCoutMessageExmpthidden.Value = remainingTheoryModuleCount.ToString();
                    }
                    else
                    {
                        remainingTheoryModuleCount = PassedConditionalTheoryModuleCount_Alevel(currentCourseID, registrationNumber, revisionNumber, entityID);
                        lblCoutMessageExmpt.Text = "<b style='color:red'>You can avail Exemption upto " + remainingTheoryModuleCount.ToString() + " Modules among below Modules Listed in left panel.</b>";
                        remainingTheoryModuleCount = GetRemainingAllTheoryModuleCount(currentCourseID, registrationNumber, revisionNumber, entityID);
                        lblCoutMessage.Text = "Theory Papers  &nbsp;&nbsp;&nbsp;&nbsp; <b style='color:red'>You can select upto  " + remainingTheoryModuleCount.ToString() + " modules (remaining to pass) in the list below.</b>";
                       // lblCoutMessageExmpthidden.Value = remainingTheoryModuleCount.ToString();
                    }
                }
                else
                {
                        remainingTheoryModuleCount = GetRemainingAllTheoryModuleCount(currentCourseID, registrationNumber, revisionNumber, entityID);
                        lblCoutMessage.Text = "Theory Papers  &nbsp;&nbsp;&nbsp;&nbsp; <b style='color:red'>You can select upto  " + remainingTheoryModuleCount.ToString() + " modules (remaining to pass) in the list below.</b>";
                       // lblCoutMessageExmpthidden.Value = remainingTheoryModuleCount.ToString();
                }

                string AlevelCompltedted = (from m in context.RegistrationDetails where m.RegistrationNo == registrationNumber && m.CourseID == 2 select m.RegistrationStatusCode).FirstOrDefault(); // code added by abhi singh on dated 03052024 check  alevel completed or not
                if (courseID == 2 && AlevelCompltedted == "P")
                {
                    lblCoutMessage.Text = "&nbsp; <b style='color:red'>You have already cleared the required specialization module. You can futher select any of the below specialization if you are intrested in getting certificate of additional specialized area.</b> <a href='https://nielit.gov.in/sites/default/files/headquarter/pdf/20220905_5th_revised_A_Level_IT.pdf' target='_blank'>Click here to see the syllabus </a>";
                    divpract.Visible = false;
                
                }
                else
                {
                    divpract.Visible = true;
                }

                if (remainingTheoryModuleCount == 0 && ModuleListTheorsdExmptthoryCount == 0) // added code dated on 24042024
                    modules = modules.Where(c => c.ModuleTypeID == (Int32)enmModuleType.Practical).ToList();
                else
                    remainingTheoryModuleCountspeclGroupAlevel = ModuleListTheorsdExmptthoryCount;
                // added for old and new pattern
                string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                DataTable myDt = new DataTable();
                SqlConnection con = new SqlConnection(constr);


                if ((courseID == 1 || courseID == 2 ))
                {
                    lblerror.Visible = true;
                   // lblerror.Text = "You are eligible to apply only for new Exam Pattern.";
                     lblerror.Text = "Online Exam Application Form is as per the latest revision(R5.1) Please read the syllabus carefully";
                    trModuleOption1.Visible = false;
                    trModuleOption.Visible = false;
                    trModuleHead.Visible = false;

                    lblTheoryFee.Text = "1000.00/750.00";
                    lblPracticalFee.Text = "500.00";

                    if (rbtn_module_selection.SelectedValue == "pre")
                    {
                        ShowAlert("You are not eligible to apply for Old pattern");
                        return;
                    }
                }
                else if ((courseID == 1 || courseID == 2 ))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "You are eligible to apply only for Old Exam Pattern.";
                    trModuleOption1.Visible = false;
                    trModuleOption.Visible = false;
                    trModuleHead.Visible = false;

                    lblTheoryFee.Text = "750.00";
                    lblPracticalFee.Text = "500.00";
                    //if (rbtn_module_selection.SelectedValue == "current")
                    //{
                    //    ShowAlert("You are not eligible to apply for New pattern");
                    //    return;

                    //}
                }
                if (courseID == 3 )
                {
                    lblerror.Visible = true;
                    lblerror.Text = "You are eligible to apply only for new Exam Pattern.";
                    trModuleOption1.Visible = false;
                    trModuleOption.Visible = false;
                    trModuleHead.Visible = false;

                    lblTheoryFee.Text = "1000.00/750.00";
                    lblPracticalFee.Text = "500.00";

                    //if (rbtn_module_selection.SelectedValue == "pre")
                    //{
                    //    ShowAlert("You are not eligible to apply for Old pattern");
                    //    return;
                    //}
                }
                con.Close();
                if (countPassedCodModule != 0 && AfterExempt==0 && CheckExemptioDoneOrNot(courseID,registrationNumber,revisionNumber,candidateID)<1)
                {
                    var ModuleListTheory = modules.Where(m => m.ModuleTypeID == theory || m.ModuleTypeID == bridge).Select(m => new { ValueField = m.ID, TextField = m.SelectionTypeID == compulsory ? ((m.ShortName + " &nbsp;&nbsp;" + m.Name)) + " (Compulsory)" : ((m.ShortName + " &nbsp;&nbsp;" + m.Name)) + " (<font style='color:red'>Elective</font>)" }).ToList();
                    EConnect.Utils.Common.ControlUtility.BindListObject(chklistModulesExmpt, ModuleListTheory);
                    ICollection<Module> modules1 = PassedConditionalTheoryModuleCount2(courseID, registrationNumber, revisionNumber);
                    var ModuleListPractical = modules1.Where(m => m.ModuleTypeID == theory).Select(m => new { TextField = m.ShortName + " - " + m.Name }).ToList();
                    listboxexmpt.Visible = true;
                    Label5.Visible = true;
                    //--EConnect.Utils.Common.ControlUtility.BindListObject(listboxexmpt, ModuleListPractical);
                    listboxexmpt.DataSource = ModuleListPractical.Select(m => m.TextField.ToString());
                    listboxexmpt.DataBind();
                }
                else if (countPassedCodModule_alevel != 0 && AfterExempt == 0 && CheckExemptioDoneOrNot(courseID, registrationNumber, revisionNumber, candidateID) < 1 && A1toA8passedmoduleCount(registrationNumber, candidateID) < 8)
                {
                    var ModuleListTheory = modules.Where(m => m.ModuleTypeID == theory || m.ModuleTypeID == bridge).Select(m => new { ValueField = m.ID, TextField = m.SelectionTypeID == compulsory ? ((m.ShortName + " &nbsp;&nbsp;" + m.Name)) + " (Compulsory)" : ((m.ShortName + " &nbsp;&nbsp;" + m.Name)) + " (<font style='color:red'>Elective</font>)" }).ToList();
                    EConnect.Utils.Common.ControlUtility.BindListObject(chklistModulesExmpt, ModuleListTheory);
                    ICollection<Module> modules1 = PassedConditionalTheoryModuleCount2_Alevel(courseID, registrationNumber, revisionNumber);
                    var ModuleListPractical = modules1.Where(m => m.ModuleTypeID == theory).Select(m => new { TextField = m.ShortName + " - " + m.Name }).ToList();
                    listboxexmpt.Visible = true;
                    Label5.Visible = true;
                    //--EConnect.Utils.Common.ControlUtility.BindListObject(listboxexmpt, ModuleListPractical);
                    listboxexmpt.DataSource = ModuleListPractical.Select(m => m.TextField.ToString());
                    listboxexmpt.DataBind();
                }
                else
                {
                    var ModuleListTheory = modules.Where(m => m.ModuleTypeID == theory || m.ModuleTypeID == bridge).Select(m => new { ValueField = m.ID, TextField = m.SelectionTypeID == compulsory ? ((m.ShortName + " &nbsp;&nbsp;" + m.Name)) + " (Compulsory)" : ((m.ShortName + " &nbsp;&nbsp;" + m.Name)) + " (<font style='color:red'>Elective</font>)" }).ToList();
                    var ModuleListPractical = modules.Where(m => m.ModuleTypeID == practical).Select(m => new { ValueField = m.ID, TextField = m.ShortName + " &nbsp;&nbsp;" + m.Name }).ToList();
                   
                    // int CountTotalPassedModule =(ModuleListTheory.Count() + ModuleListPractical.Count());
                    //if (CountTotalPassedModule != 0)
                    //{
                        EConnect.Utils.Common.ControlUtility.BindListObject(chklistModules, ModuleListTheory);
                        listboxexmpt.Visible = false;
                        Label5.Visible = false;
                        EConnect.Utils.Common.ControlUtility.BindListObject(chklistPracticals, ModuleListPractical);
                    //}
                    //else
                    //{
                    //    ShowAlert("You Have Already Cleared All Modules(Theroy and Practical)!");
                    //    Response.Write(GeInvalidRequestMessage("Goto Home Page", Server.MapPath("../Index.aspx")));
                    //    Response.End();
                    //    return;
                    //}
                }
                
                if (attemptedLastExams != null && attemptedLastExams.ID != examID && remainingTheoryModuleCount == 0)
                {
                    Exam lastExam = CourseManager.GeLastExam(context, currentCourseID, applicantTypeID);
                    if (attemptedLastExams.ExamMonth == lastExam.ExamMonth && attemptedLastExams.ExamYear == lastExam.ExamYear)
                    {
                        lblerror.Text = "You can apply for exam for improvement purpose only..";
                        lblTheoryFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ImprovementOfPaperFee).ToString("F");
                        lblerror.Visible = true;
                        tr_select_module.Visible = false;
                        rbtn_module_selection.Visible = false;
                        trImprovementHead.Visible = true;
                        trImprovementOption.Visible = true;
                        trModuleHead.Visible = false;
                        trModuleOption.Visible = false;
                        var passedModulesOfCurrentRevision = CourseManager.GetModulesForImprovement(context, currentCourseID, registrationNumber, revisionNumber, candidateID);
                        var allModules = (from m in passedModulesOfCurrentRevision
                                          orderby m.Code
                                          select new { ValueField = m.ID, TextField = m.ShortName + " " + m.Name }).Distinct().ToList();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlModules, allModules, new ListItem("--Select One--", "0"));
                        lblDeclarationNumber.Text = "5.";
                    }                  
                }                
                foreach (ListItem item in chklistModules.Items)
                {
                    Int32 moduleID = Convert.ToInt32(item.Value);
                    ExamTimeTable tt = context.ExamTimeTables.Where(t => (t.ExamID == exam.ID && t.ModuleID == moduleID)).FirstOrDefault();
                    if (tt != null && tt.ExamSession != null)
                        item.Text += " <b>" + tt.ExamFomDate.ToString("dd-MMM-yyyy") + ": " + tt.ExamSession.Code + "</b>";
                    else
                        item.Text += " <b>from " + tt.ExamFomDate.ToString("dd-MMM-yyyy") + " to " + Convert.ToDateTime(tt.ExamToDates).ToString("dd-MMM-yyyy") + "</b>";
                }
                if (lastExam_attempted_exceptionalcase == examID && examID != 0)
                {
                    tr_select_module.Visible = false;
                    rbtn_module_selection.Visible = false;
                    trModuleHead.Visible = true;
                  if (AfterExempt != 1)
                   {
                    if ((courseID == 3)||courseID==2)
                    {

                        if ((countPassedCodModule != 0 && CheckExemptioDoneOrNot(courseID, registrationNumber, revisionNumber, candidateID) < 1) || (countPassedCodModule_alevel != 0 && CheckExemptioDoneOrNot(courseID, registrationNumber, revisionNumber, candidateID) < 1 && A1toA8passedmoduleCount(registrationNumber, entityID) < 8) && !string.IsNullOrEmpty(ModuleListTheorsdExmptthoryCount.ToString()))
                        {
                            lblserveExpt.Text = "2.1";
                            panelfeedetails.Visible = false;
                            disclmrPanel.Visible = false;
                            btnSave.Visible = false;
                            btnExmpSubmit.Visible = true;
                            trImprovementHead.Visible = false;
                            FeedtlsId.Visible = false;
                            disclamrdivid.Visible = false;
                            trModuleOptionExmpt.Visible = true;
                            trModuleOptionExmpt1.Visible = true;
                            TrExamCentre1.Visible = false;
                            TrExamCentre2.Visible = false;
                            trModuleOption.Visible = false;
                            trModuleOption1.Visible = false;
                        }
                        else
                        {
                            lblserve1.Text = "2.1";
                            panelfeedetails.Visible = true;
                            btnSave.Visible = true;
                            disclmrPanel.Visible = true;
                            btnExmpSubmit.Visible = false;
                            trImprovementHead.Visible = true;
                            FeedtlsId.Visible = true;
                            disclamrdivid.Visible = true;
                            trModuleOptionExmpt.Visible = false;
                            trModuleOptionExmpt1.Visible = false;
                            TrExamCentre1.Visible = true;
                            TrExamCentre2.Visible = true;
                            trModuleOption.Visible = true;
                            trModuleOption1.Visible = true;
                        }
                    }
                    else
                    {
                        lblserve1.Text = "2.1";
                        panelfeedetails.Visible = true;
                        btnSave.Visible = true;
                        btnExmpSubmit.Visible = false;
                        disclmrPanel.Visible = true;
                        FeedtlsId.Visible = true;
                        trImprovementHead.Visible = true;
                        disclamrdivid.Visible = true;
                        trModuleOptionExmpt.Visible = false;
                        trModuleOptionExmpt1.Visible = false;
                        TrExamCentre1.Visible = true;
                        TrExamCentre2.Visible = true;
                        trModuleOption.Visible = true;
                        trModuleOption1.Visible = true;
                    }
                 }
                  else
                  {
                      revisionNumber = currentevisionNumber;
                  }  
                }
                else
                {
                    //tr_select_module.Visible = true; //commented by kismat
                    trModuleHead.Visible = true;
                    if (AfterExempt != 1)
                    {
                        if ((courseID == 3)|| courseID==2)
                        {
                            //trModuleOptionExmpt.Visible = true;
                            //trModuleOptionExmpt1.Visible = true;
                            //trModuleOption.Visible = false;
                            //trModuleOption1.Visible = false;

                            if ((countPassedCodModule != 0 && CheckExemptioDoneOrNot(courseID, registrationNumber, revisionNumber, candidateID) < 1) || (countPassedCodModule_alevel != 0 && CheckExemptioDoneOrNot(courseID, registrationNumber, revisionNumber, candidateID) < 1 && A1toA8passedmoduleCount(registrationNumber,entityID)<8) && !string.IsNullOrEmpty(ModuleListTheorsdExmptthoryCount.ToString()))
                               
                            {
                                lblserveExpt.Text = "2.1";
                                panelfeedetails.Visible = false;
                                btnSave.Visible = false;
                                btnExmpSubmit.Visible = true;
                                disclmrPanel.Visible = false;
                                FeedtlsId.Visible = false;
                                disclamrdivid.Visible = false;
                                trImprovementHead.Visible = false;
                                trModuleOptionExmpt.Visible = true;
                                trModuleOptionExmpt1.Visible = true;
                                TrExamCentre1.Visible = false;
                                TrExamCentre2.Visible = false;
                                trModuleOption.Visible = false;
                                trModuleOption1.Visible = false;
                            }
                            else
                            {
                                lblserve1.Text = "2.1";
                                panelfeedetails.Visible = true;
                                btnSave.Visible = true;
                                btnExmpSubmit.Visible = false;
                                disclmrPanel.Visible = true;
                                FeedtlsId.Visible = true;
                                disclamrdivid.Visible = true;
                                trImprovementHead.Visible = true;
                                trModuleOptionExmpt.Visible = false;
                                trModuleOptionExmpt1.Visible = false;
                                TrExamCentre1.Visible = true;
                                TrExamCentre2.Visible = true;
                                trModuleOption.Visible = true;
                                trModuleOption1.Visible = true;
                            }
                        }
                        else
                        {
                            lblserve1.Text = "2.1";
                            panelfeedetails.Visible = true;
                            btnSave.Visible = true;
                            btnExmpSubmit.Visible = false;
                            FeedtlsId.Visible = true;
                            disclmrPanel.Visible = true;
                            disclamrdivid.Visible = true;
                            trImprovementHead.Visible = true;
                            trModuleOptionExmpt.Visible = false;
                            trModuleOptionExmpt1.Visible = false;
                            TrExamCentre1.Visible = true;
                            TrExamCentre2.Visible = true;
                            trModuleOption.Visible = true;
                            trModuleOption1.Visible = true;
                        }
                    }
                    else
                    {
                        revisionNumber = currentevisionNumber;
                    }  
                }
            }
            else
            {
                lblerror.Text = "You can not apply for exam as all theory/bridge/practical modules have been passed.";
                lblerror.Visible = true;
                btnSave.Visible = false;
            }
        };
    }

    public static ICollection<Module> GetRemainingModules(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int32 currentRevisionNumber, Int64 candidateID, int afterExmpt)
    {
        try
        {
            Int32 countPassedCodModule = 0;
            countPassedCodModule = PassedConditionalTheoryModuleCount1(courseID, registrationNumber, currentRevisionNumber);
            Int32 countPassedCodModule_Alevel = 0;
            countPassedCodModule_Alevel = PassedConditionalTheoryModuleCount_Alevel(courseID, registrationNumber, currentRevisionNumber,candidateID);
            Int32 moduleTypeTheory = Convert.ToInt32(enmModuleType.Theory);
            Int32 moduleTypeBridge = Convert.ToInt32(enmModuleType.Bridge);
            Int32 moduleTypePractical = Convert.ToInt32(enmModuleType.Practical);
            Int32 moduleTypeProject = Convert.ToInt32(enmModuleType.Project);
            Int32 registrationRevisionNumber = EConnect.NIELIT.CourseManager.GetCourseRevisionNumberAtRegistrationCommenced(context, courseID, registrationNumber, candidateID);
            ICollection<Module> listOfModulesPassed = EConnect.NIELIT.CourseManager.GetPassedModulesListOfAnyRevision(context, courseID, registrationNumber, candidateID).ToList();

            ICollection<Module> listOfModulesOfCurrentRevision = EConnect.NIELIT.CourseManager.GetModulesList(context, courseID, currentRevisionNumber).ToList();

            List<Module> passedModulesOfCurrentRevision = new List<Module>();
            // Int32 PassedConditionalTheoryModuleCount1 = PassedConditionalTheoryModuleCount(courseID, registrationNumber, currentRevisionNumber);
            if (registrationRevisionNumber == currentRevisionNumber)
            {
                listOfModulesPassed.ToList().ForEach(s => listOfModulesOfCurrentRevision.Remove(s));
                passedModulesOfCurrentRevision = (List<Module>)listOfModulesPassed;
                if (courseID == 1213)
                {

                    bool b748 = passedModulesOfCurrentRevision.Any(p => p.ID == 1151);
                    if (b748 == true)
                    {
                        Module moduleF = context.Modules.Find(1155);
                        listOfModulesOfCurrentRevision.Remove(moduleF);
                    }

                    bool b749 = passedModulesOfCurrentRevision.Any(p => p.ID == 1152);
                    if (b749 == true)
                    {
                        Module moduleF = context.Modules.Find(1156);
                        listOfModulesOfCurrentRevision.Remove(moduleF);
                    }
                    bool b750 = passedModulesOfCurrentRevision.Any(p => p.ID == 1153);
                    if (b750 == true)
                    {
                        Module moduleF = context.Modules.Find(1157);
                        listOfModulesOfCurrentRevision.Remove(moduleF);
                    }
                    bool b751 = passedModulesOfCurrentRevision.Any(p => p.ID == 1154);
                    if (b751 == true)
                    {
                        Module moduleF = context.Modules.Find(1158);
                        listOfModulesOfCurrentRevision.Remove(moduleF);
                    }

                    //passedModulesOfCurrentRevision.ForEach(p => listOfModulesOfCurrentRevision.Remove(p));
                }
            }
            else
            {
                foreach (var mod in listOfModulesPassed.ToList())
                {
                    //Implement Elctive/Selective Rule
                    Int32 passedRevisionNumber = mod.RevisionNumber;
                    if (passedRevisionNumber == currentRevisionNumber)
                    {
                        passedModulesOfCurrentRevision.Add(mod);
                    }
                    else
                    {
                        Int32 index = 0;
                        Int32 oldModuleID = mod.ID;
                        for (index = passedRevisionNumber; index < currentRevisionNumber; index++)
                        {
                            int newModule = (from s in context.Parities
                                             where s.CourseID == courseID && s.OldRevisionNumber == index
                                            && s.OldModuleID == oldModuleID
                                             select s.NewModuleID).FirstOrDefault();
                            if (newModule > 0)
                            {
                                oldModuleID = (Int32)newModule;
                            }
                            else
                                break;
                        }

                        Module newMod = context.Modules.Find(oldModuleID);
                        if (newMod != null)
                        {
                            passedModulesOfCurrentRevision.Add(newMod);
                        }
                    }
                }


                if (currentRevisionNumber == 6 && courseID == 1)
                {
                    if (listOfModulesPassed.Any(p => p.ID == 929) || listOfModulesPassed.Any(p => p.ID == 930) || listOfModulesPassed.Any(p => p.ID == 931) || listOfModulesPassed.Any(p => p.ID == 932))
                    {
                        Module moduleF = context.Modules.Find(715);
                        listOfModulesOfCurrentRevision.Remove(moduleF);
                    }
                }

                if (currentRevisionNumber == 6 && courseID == 2)
                {
                    if (listOfModulesPassed.Any(p => p.ID == 938) || listOfModulesPassed.Any(p => p.ID == 939) || listOfModulesPassed.Any(p => p.ID == 940) || listOfModulesPassed.Any(p => p.ID == 941))
                    {
                        Module moduleF = context.Modules.Find(735);
                        listOfModulesOfCurrentRevision.Remove(moduleF);
                    }
                }

                //if (currentRevisionNumber == 5 && courseID == 3)
                //{
                //    if (listOfModulesPassed.Any(p => p.ID == 929) || listOfModulesPassed.Any(p => p.ID == 930) || listOfModulesPassed.Any(p => p.ID == 931) || listOfModulesPassed.Any(p => p.ID == 932))
                //    {
                //        Module moduleF = context.Modules.Find(715);
                //        listOfModulesOfCurrentRevision.Remove(moduleF);
                //    }
                //}


                if (currentRevisionNumber == 6 && courseID == 1)
                {

                    bool b748 = passedModulesOfCurrentRevision.Any(p => p.ID == 929);
                    if (b748 == true)
                    {
                        Module moduleF = context.Modules.Find(933);
                        listOfModulesOfCurrentRevision.Remove(moduleF);
                    }

                    bool b749 = passedModulesOfCurrentRevision.Any(p => p.ID == 930);
                    if (b749 == true)
                    {
                        Module moduleF = context.Modules.Find(934);
                        listOfModulesOfCurrentRevision.Remove(moduleF);
                    }
                    bool b750 = passedModulesOfCurrentRevision.Any(p => p.ID == 931);
                    if (b750 == true)
                    {
                        Module moduleF = context.Modules.Find(935);
                        listOfModulesOfCurrentRevision.Remove(moduleF);
                    }
                    bool b751 = passedModulesOfCurrentRevision.Any(p => p.ID == 932);
                    if (b751 == true)
                    {
                        Module moduleF = context.Modules.Find(936);
                        listOfModulesOfCurrentRevision.Remove(moduleF);
                    }

                    //passedModulesOfCurrentRevision.ForEach(p => listOfModulesOfCurrentRevision.Remove(p));
                }
               
                else if (currentRevisionNumber == 6 && courseID == 2)
                {
                    bool b757 = passedModulesOfCurrentRevision.Any(p => p.ID == 938);
                    if (b757 == true)
                    {
                        Module moduleF = context.Modules.Find(956);
                        listOfModulesOfCurrentRevision.Remove(moduleF);
                    }

                    bool b758 = passedModulesOfCurrentRevision.Any(p => p.ID == 939);
                    if (b758 == true)
                    {
                        Module moduleF = context.Modules.Find(957);
                        listOfModulesOfCurrentRevision.Remove(moduleF);
                    }
                    bool b759 = passedModulesOfCurrentRevision.Any(p => p.ID == 940);
                    if (b759 == true)
                    {
                        Module moduleF = context.Modules.Find(958);
                        listOfModulesOfCurrentRevision.Remove(moduleF);
                    }
                    bool b760 = passedModulesOfCurrentRevision.Any(p => p.ID == 941);
                    if (b760 == true)
                    {
                        Module moduleF = context.Modules.Find(959);
                        listOfModulesOfCurrentRevision.Remove(moduleF);
                    }

                }
                passedModulesOfCurrentRevision.ForEach(s => listOfModulesOfCurrentRevision.Remove(s));
            }
            Int32 elective = Convert.ToInt32(enmSelectionType.Elective);

            //this code will remove all modules of elective group whome any of the module has been passed by the candidate
             string AlevelCompltedted=(from m in context.RegistrationDetails where m.RegistrationNo==registrationNumber && m.CourseID==2 select m.RegistrationStatusCode).FirstOrDefault();
             if (courseID != 2 || (courseID == 2 && AlevelCompltedted != "P"))
             {
                List<Int32> arrElectiveGroup = new List<int>();
                foreach (Module module in passedModulesOfCurrentRevision)
                {
                    if (module != null)
                    {
                        if (module.SelectionTypeID == elective)
                        {
                            if (!arrElectiveGroup.Contains(module.ElectiveGroup.Value))
                                arrElectiveGroup.Add(module.ElectiveGroup.Value);
                        }
                    }
                }
                if (arrElectiveGroup.Count > 0)
                {
                    foreach (Int32? groupID in arrElectiveGroup)
                    {
                        var modulesToBeCleared = (from el in context.Modules
                                                  where el.ElectiveGroup == groupID && el.RevisionNumber == currentRevisionNumber && el.CourseID == courseID
                                                  select el.NumberOfElectiveModulesAllowed).Distinct().Sum();
                        if (modulesToBeCleared.HasValue)
                        {
                            Int32 moduleToBePassed = (Int32)modulesToBeCleared;
                            //Int32? modulesCleared = passedModulesOfCurrentRevision.Where(d => (d.ElectiveGroup == groupID && d.RevisionNumber == currentRevisionNumber && d.CourseID == courseID)).Select(d => d.NumberOfElectiveModulesAllowed).Distinct().Count();                            
                            Int32? modulesCleared = passedModulesOfCurrentRevision.Where(d => (d.ElectiveGroup == groupID && d.RevisionNumber == currentRevisionNumber && d.CourseID == courseID)).Select(d => d.ID).Distinct().Count();
                            if (modulesCleared.HasValue)
                            {
                                if (modulesCleared.Value >= moduleToBePassed)
                                {
                                    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => (d.ElectiveGroup.HasValue ? d.ElectiveGroup.Value != groupID : 1 == 1)).ToList();
                                }
                            }
                        }
                    }

                }
            }
            //---Start----------Special Case for BE7-R4 & B252-R4 (Sw Testing & Quality Management) in 'B'-Level on dated 14May2020-------------------------

            if (courseID == 3)
            {
                Int32 theory = Convert.ToInt32(enmModuleType.Theory);
                Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
                Int32 moduleno_392 = (from d in context.CourseExamApplicationDetails
                                      join m in context.Modules on d.ModuleID equals m.ID
                                      where d.CourseID == courseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                      d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && m.ID == 392
                                      select m).Distinct().Count();
                Int32 moduleno_412 = (from d in context.CourseExamApplicationDetails
                                      join m in context.Modules on d.ModuleID equals m.ID
                                      where d.CourseID == courseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                      d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && m.ID == 412
                                      select m).Distinct().Count();

                if (moduleno_392 != 0)
                {
                    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => (d.ID != 412)).ToList();
                }
                else if (moduleno_412 != 0)
                {
                    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => (d.ID != 392)).ToList();
                }
            }

            //---------------------------------------------------------------END-----------------------------------------------------------

            //
            Int32 compulsory = Convert.ToInt32(enmSelectionType.Compulsory);
            Int32 theoryModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber, enmModuleType.Theory, enmSelectionType.Compulsory);
            //theoryModulesTobePassed += GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Bridge, null);
            Int32 theoryModulesPassed = passedModulesOfCurrentRevision.Where(d => (d.ModuleTypeID == moduleTypeTheory && d.SelectionTypeID == compulsory)).Count();
            if (theoryModulesPassed >= theoryModulesTobePassed)
                if (registrationRevisionNumber == currentRevisionNumber)
                    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => ((d.ModuleTypeID == moduleTypeTheory ? d.SelectionTypeID != compulsory : 1 == 1))).ToList();

            // --start-- commented by abhi singh dated on 23042024 for testing remove specilization group.
            //if (listOfModulesOfCurrentRevision.Contains(context.Modules.Find(946)) && !listOfModulesOfCurrentRevision.Contains(context.Modules.Find(951)))
            //{

            //    Module moduleXi = context.Modules.Find(951);
            //    listOfModulesOfCurrentRevision.Add(moduleXi);

            //    Module moduleXii = context.Modules.Find(952);
            //    listOfModulesOfCurrentRevision.Add(moduleXii);

            //    Module moduleXiii = context.Modules.Find(953);
            //    listOfModulesOfCurrentRevision.Add(moduleXiii);

            //    Module moduleXiv = context.Modules.Find(954);
            //    listOfModulesOfCurrentRevision.Add(moduleXiv);

            //    Module moduleXv = context.Modules.Find(955);
            //    listOfModulesOfCurrentRevision.Add(moduleXv);

            //}
            //else if (!listOfModulesOfCurrentRevision.Contains(context.Modules.Find(946)) && listOfModulesOfCurrentRevision.Contains(context.Modules.Find(951)))
            //{
            //    Module moduleXi = context.Modules.Find(946);
            //    listOfModulesOfCurrentRevision.Add(moduleXi);

            //    Module moduleXii = context.Modules.Find(947);
            //    listOfModulesOfCurrentRevision.Add(moduleXii);

            //    Module moduleXiii = context.Modules.Find(948);
            //    listOfModulesOfCurrentRevision.Add(moduleXiii);

            //    Module moduleXiv = context.Modules.Find(949);
            //    listOfModulesOfCurrentRevision.Add(moduleXiv);

            //    Module moduleXv = context.Modules.Find(950);
            //    listOfModulesOfCurrentRevision.Add(moduleXv);
            //}

            // --End--
            //--added code dated 28032023 for B Level
            if (courseID == 3 && currentRevisionNumber == 5 && countPassedCodModule > 0 && CheckExemptioDoneOrNot(courseID, registrationNumber, currentRevisionNumber, candidateID) < 1)
            {
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 1143).ToList();
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 1144).ToList();
            }
            //--End--

            //--added code dated 26042023 for A Level (Removing specialized modeule from exemption list for those candidate who is eleigible for exempetion

            if (courseID == 2 && countPassedCodModule_Alevel > 0 && CheckExemptioDoneOrNot(courseID, registrationNumber, currentRevisionNumber, candidateID) < 1 && A1toA8passedmoduleCount( registrationNumber,  candidateID) <8)
            {
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 946).ToList();
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 947).ToList();
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 948).ToList();
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 949).ToList();
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 950).ToList();
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 951).ToList();
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 952).ToList();
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 953).ToList();
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 954).ToList();
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 955).ToList();
            }
            string BlevelCompltedted=(from m in context.RegistrationDetails where m.RegistrationNo==registrationNumber && m.CourseID==3 select m.RegistrationStatusCode).FirstOrDefault();
            if (courseID == 4 && BlevelCompltedted == "P")
            {
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 425).ToList();
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 426).ToList();
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 427).ToList();
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 428).ToList();
            }
           // --End--

            Int32 bridgeModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber, enmModuleType.Bridge, null);
            Int32 bridgeModulesPassed = passedModulesOfCurrentRevision.Where(d => d.ModuleTypeID == moduleTypeBridge).Count();
            if (bridgeModulesPassed >= bridgeModulesTobePassed)
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ModuleTypeID != moduleTypeBridge).ToList();

            Int32 practicalModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber, enmModuleType.Practical, null);
            Int32 practicalModulesPassed = passedModulesOfCurrentRevision.Where(d => d.ModuleTypeID == moduleTypePractical).Count();
            if (currentRevisionNumber != 5 && courseID != 3)
            {
                if (practicalModulesPassed >= practicalModulesTobePassed)
                    if (registrationRevisionNumber == currentRevisionNumber)
                        listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ModuleTypeID != moduleTypePractical).ToList();
            }
            //else
            //{
            //    //For PR1 
            //    int pr1count = listOfModulesOfCurrentRevision.Where(d => d.ID == 1043 || d.ID == 1044 || d.ID == 1045 || d.ID == 1046 || d.ID == 1047).Count();
            //    if (pr1count == 0)
            //    {
            //        listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 1065).ToList();
            //    }

            //    //For PR2 
            
            //    int pr2count = listOfModulesOfCurrentRevision.Where(d => d.ID == 1048 || d.ID == 1049 || d.ID == 1050 || d.ID == 1051 || d.ID == 1052).Count();
            //    if (pr2count == 0)
            //    {
            //        listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 1066).ToList();
            //    }

            //    //PR3

            //    int pr3count = listOfModulesOfCurrentRevision.Where(d =>  d.ID == 1053 || d.ID == 1054 || d.ID == 1055 || d.ID == 1056 || d.ID == 1057 || d.ID == 1058 || d.ID == 1059 || d.ID == 1060 || d.ID == 1061 || d.ID == 1062 || d.ID == 1063 || d.ID == 1064).Count();
            //    if (pr3count == 0)
            //    {
            //        listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 1067).ToList();
            //    }
            //}

            Int32 projectModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber, enmModuleType.Project, null);
            Int32 projectModulesPassed = passedModulesOfCurrentRevision.Where(d => d.ModuleTypeID == moduleTypeProject).Count();
            if (projectModulesPassed >= projectModulesTobePassed)
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ModuleTypeID != moduleTypeProject).ToList();

            return listOfModulesOfCurrentRevision.Distinct().ToList();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static int A1toA8passedmoduleCount(Int64 registrationNumber, Int64 candidateID)
    {

        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                   join m in context.Modules on d.ModuleID equals m.ID
                                                   where d.CourseID == 2 && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                   d.Grade.IsPassed == true && (m.ModuleTypeID == 1 || m.ModuleTypeID == 5)
                                                   orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                   select d.ModuleID).ToList();


                //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                //--List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
               

                List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 2 select m.ID).ToList();
                List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR2.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();

                List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 2 select m.ID).ToList();
                List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();

                List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 2 select m.ID).ToList();
                List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();

                var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 2 select m.ID).ToList();
                List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
                List<int> PassedTotalModuleInR5toR6 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();

                List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 2 select m.ID).Distinct().ToList();
                List<int> PassedModuleInR6 = allModulR6.Intersect(passedTotalanyrevison).ToList();
                List<int> TotalpasedinR6 = PassedModuleInR6.Concat(PassedTotalModuleInR5toR6).ToList();

                List<int> A1toA8 = new List<int> { 946,947,948,949,950,951,952,953,954,955 };
                List<int> TotalpasedinA1toA8 = TotalpasedinR6.Except(A1toA8).ToList();
                int countTotalPassedInA1toA8 = TotalpasedinA1toA8.Count();
                return countTotalPassedInA1toA8 ;
                //if (countTotalPassedInAlevel>=8)
                //    return 0;
                //else
                //    return 1;
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
      }

    public static Int32 GetTotalModules(Int32 courseID, Int32 revisionNumber, enmModuleType? moduleType, enmSelectionType? selectionType)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 compulsoryType = Convert.ToInt32(enmSelectionType.Compulsory);
                Int32? selectionTypeID = null;
                if (selectionType.HasValue)
                    selectionTypeID = Convert.ToInt32(selectionType);
                Int32 count = 0;
                if (moduleType.HasValue)
                {

                    Int32 moduleTypeID = Convert.ToInt32(moduleType);
                    if (moduleType.Value == enmModuleType.Theory)
                    {
                        Int32 bridgeTypeID = Convert.ToInt32(enmModuleType.Bridge);
                        if (selectionTypeID.HasValue)
                        {
                            if (selectionType.Value == enmSelectionType.Compulsory)
                            {
                                count = (from m in context.Modules
                                         where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID == selectionTypeID.Value && m.ModuleTypeID == moduleTypeID
                                         select m).Count();
                            }
                            else
                            {
                                var electiveGroup = (from m in context.Modules
                                                     where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID != compulsoryType && m.ModuleTypeID == moduleTypeID
                                                     select m.ElectiveGroup).Distinct();
                                if (electiveGroup != null)
                                {
                                    ////-------Start----------Added on 15052020 due to count mismatch of elective group-id during the switching of revision choice and total module count
                                    foreach (var groupid in electiveGroup.ToList())
                                    {
                                        var cnt = (from m in context.Modules
                                                   where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.ElectiveGroup == groupid
                                                   select m.NumberOfElectiveModulesAllowed).Distinct().Sum();
                                        if (cnt.HasValue)
                                        {
                                            count += (Int32)cnt.Value;
                                        }
                                    }
                                    ////------------------------------------------------------------End    

                                    ////-------Start----------comment on 05052020 due to count mismatch of elective group-id during the switching of revision choice
                                    ////var cnt = (from m in context.Modules
                                    ////           where electiveGroup.Contains(m.ElectiveGroup) && m.CourseID == courseID && m.RevisionNumber == revisionNumber
                                    ////           select m.NumberOfElectiveModulesAllowed).Distinct().Sum();
                                    ////if (cnt.HasValue)
                                    ////    //Start----------comment on 05052020 due to count mismatch of elective group-id during the switching of revision choice
                                    ////    //count = (Int32)cnt.Value;
                                    ////    //----------------------------------------------------------------End

                                    ////    //Start-------Added on 05052020 for rectify count mismatch of elective group-id during the switching of revision choice-----------
                                    ////    for (Int32 totalelectivegroup = 0; totalelectivegroup < electiveGroup.Count(); totalelectivegroup++)
                                    ////    {
                                    ////        count += (Int32)cnt.Value;
                                    ////    }
                                    ////    //------------------------------------------------------------End    
                                }
                            }

                        }
                        else
                        {
                            count = (from m in context.Modules
                                     where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID == compulsoryType && m.ModuleTypeID == moduleTypeID
                                     select m).Count();
                            var electiveGroup = (from m in context.Modules
                                                 where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID != compulsoryType && m.ModuleTypeID == moduleTypeID
                                                 select m.ElectiveGroup).Distinct();
                            if (electiveGroup != null)
                            {

                                ////-------Start----------Added on 15052020 due to count mismatch of elective group-id during the switching of revision choice and total module count
                                foreach (var groupid in electiveGroup.ToList())
                                {
                                    var cnt = (from m in context.Modules
                                               where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.ElectiveGroup == groupid
                                               select m.NumberOfElectiveModulesAllowed).Distinct().Sum();
                                    if (cnt.HasValue)
                                    {
                                        count += (Int32)cnt.Value;
                                    }
                                }
                                ////------------------------------------------------------------End    

                                ////-------Start----------comment on 05052020 due to count mismatch of elective group-id during the switching of revision choice
                                //var cnt = (from m in context.Modules
                                //           where electiveGroup.Contains(m.ElectiveGroup) && m.CourseID == courseID && m.RevisionNumber == revisionNumber
                                //           select m.NumberOfElectiveModulesAllowed).Distinct().Sum();
                                //if (cnt.HasValue)
                                // //Start----------comment on 24042020 due to count mismatch of elective group-id during the switching of revision choice
                                ////count += (Int32)cnt.Value;                                    
                                ////----------------------------------------------------------------End

                                ////Start-------Added on 24042020 for rectify count mismatch of elective group-id during the switching of revision choice-----------
                                //for (Int32 totalelectivegroup = 0; totalelectivegroup < electiveGroup.Count(); totalelectivegroup++)
                                //{
                                //    count += (Int32)cnt.Value;
                                //}
                                ////------------------------------------------------------------End    
                            }
                            //count += (from m in context.Modules
                            //          where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID != compulsoryType && (m.ModuleTypeID == moduleTypeID || m.ModuleTypeID == bridgeTypeID)
                            //          select m.ElectiveGroup).Distinct().Count();
                        }
                    }
                    else
                    {
                        count = (from m in context.Modules
                                 where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.ModuleTypeID == moduleTypeID
                                 select m).Count();
                    }
                }
                else
                {
                    if (selectionTypeID.HasValue)
                    {
                        if (selectionType.Value == enmSelectionType.Compulsory)
                        {
                            count = (from m in context.Modules
                                     where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID == selectionTypeID.Value
                                     select m).Count();
                        }
                        else
                        {
                            var electiveGroup = (from m in context.Modules
                                                 where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID != compulsoryType
                                                 select m.ElectiveGroup).Distinct();
                            if (electiveGroup != null)
                            {

                                ////-------Start----------Added on 15052020 due to count mismatch of elective group-id during the switching of revision choice and total module count
                                foreach (var groupid in electiveGroup.ToList())
                                {
                                    var cnt = (from m in context.Modules
                                               where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.ElectiveGroup == groupid
                                               select m.NumberOfElectiveModulesAllowed).Distinct().Sum();
                                    if (cnt.HasValue)
                                    {
                                        count += (Int32)cnt.Value;
                                    }
                                }
                                ////------------------------------------------------------------End   

                                ////-------Start----------comment on 05052020 due to count mismatch of elective group-id during the switching of revision choice
                                //var cnt = (from m in context.Modules
                                //           where electiveGroup.Contains(m.ElectiveGroup) && m.CourseID == courseID && m.RevisionNumber == revisionNumber
                                //           select m.NumberOfElectiveModulesAllowed).Distinct().Sum();
                                //if (cnt.HasValue)
                                //    //count = (Int32)cnt.Value;

                                //    //Start-------Added on 05052020 for rectify count mismatch of elective group-id during the switching of revision choice-----------
                                //    for (Int32 totalelectivegroup = 0; totalelectivegroup < electiveGroup.Count(); totalelectivegroup++)
                                //    {
                                //        count += (Int32)cnt.Value;
                                //    }
                                //    //------------------------------------------------------------End    
                            }
                        }

                    }
                    else
                    {
                        count = (from m in context.Modules
                                 where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID == compulsoryType
                                 select m).Count();
                        var electiveGroup = (from m in context.Modules
                                             where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID != compulsoryType
                                             select m.ElectiveGroup).Distinct();
                        if (electiveGroup != null)
                        {
                            ////-------Start----------Added on 15052020 due to count mismatch of elective group-id during the switching of revision choice and total module count                                
                            foreach (var groupid in electiveGroup.ToList())
                            {
                                var cnt = (from m in context.Modules
                                           where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.ElectiveGroup == groupid
                                           select m.NumberOfElectiveModulesAllowed).Distinct().Sum();
                                if (cnt.HasValue)
                                {
                                    count += (Int32)cnt.Value;
                                }
                            }
                            ////------------------------------------------------------------End    

                            ////-------Start----------comment on 05052020 due to count mismatch of elective group-id during the switching of revision choice
                            //var cnt = (from m in context.Modules
                            //           where electiveGroup.Contains(m.ElectiveGroup) && m.CourseID == courseID && m.RevisionNumber == revisionNumber
                            //           select m.NumberOfElectiveModulesAllowed).Distinct().Sum();
                            //if (cnt.HasValue)
                            //    //count += (Int32)cnt.Value;

                            //    //Start-------Added on 05052020 for rectify count mismatch of elective group-id during the switching of revision choice-----------
                            //    for (Int32 totalelectivegroup = 0; totalelectivegroup < electiveGroup.Count(); totalelectivegroup++)
                            //    {
                            //        count += (Int32)cnt.Value;
                            //    }
                            ////------------------------------------------------------------End    
                        }
                        //count += (from m in context.Modules
                        //          where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID != compulsoryType
                        //          select m.ElectiveGroup).Distinct().Count();
                    }
                }
                return count;
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        CourseExamApplication appl = new CourseExamApplication();
              
        EConnectContext econtext = new EConnectContext();
        try
        {
            ValidateTheoryModules();
            ValidatePracticalModules(0);
            Int32 TotalTheoryFee = 0;
            Int32 totalPracticalFee = 0;
            Int32 theoryCount = 0;
            Int32 practicalCount = 0;
            Int32 feeProcessing = 0;

            //validating revision choice selection  
       
            if (CourseManager.IsCourseReviesd(currentCourseID) && econtext.RevisionChoices.Any(s => s.course_id == currentCourseID) && tr_select_module.Visible == true)
            {
                if (rbtn_module_selection.SelectedIndex < 0)
                {
                    throw new Exception("Please Select module revision");
                }
            }
            #region[validating Online_Offline_Prac_exam centre selection(Add By Reena)]
            if (currentCourseID == 1)  //added by Reena
            {
                
                //added by Reena

              

                if (TrPracExamCenter1.Visible == true && DdlPracExamCentre1.SelectedValue == "0")
                {
                    throw new Exception("Please select Practical Exam Centre 1");
                }

                if (TrPracExamCenter2.Visible == true && DdlPracExamCentre2.SelectedValue == "0")
                {
                    throw new Exception("Please select Practical Exam Centre 2");
                }
                if (DdlPracExamCentre1.SelectedValue == DdlPracExamCentre2.SelectedValue && DdlPracExamCentre1.SelectedValue != "0")
                {
                    throw new Exception("Both Exam Practical Centre Choices cannot be same.");
                }

                if (TrOnlTheoryExamCenter1.Visible == true && DdlOnlTheoryExamCentre1.SelectedValue == "0")
                {
                    throw new Exception("Please select Online Theory Exam Centre 1");
                }

                if (TrOnlTheoryExamCenter2.Visible == true && DdlOnlTheoryExamCentre2.SelectedValue == "0")
                {
                    throw new Exception("Please select Online Theory Exam Centre 2");
                }
                if (DdlOnlTheoryExamCentre1.SelectedValue == DdlOnlTheoryExamCentre2.SelectedValue && DdlOnlTheoryExamCentre1.SelectedValue != "0")
                {
                    throw new Exception("Both Online Exam Centre Choices cannot be same.");
                }
               
            }
            if (currentCourseID == 2)  //added by Reena
            {
                if (TrExamCentre1.Visible == true && DdlExamCentre1.SelectedValue == "0")
                {
                    throw new Exception("Please select Exam Centre 1");
                }

                if (TrExamCentre2.Visible == true && DdlExamCentre2.SelectedValue == "0")
                {
                    throw new Exception("Please select Exam Centre 2");
                }
                if (DdlExamCentre1.SelectedValue == DdlExamCentre2.SelectedValue && DdlExamCentre1.SelectedValue != "0")
                {
                    throw new Exception("Both Exam Centre Choices cannot be same.");
                }

                if (TrPracExamCenter1.Visible == true && DdlPracExamCentre1.SelectedValue == "0")
                {
                    throw new Exception("Please select Practical Exam Centre 1");
                }

                if (TrPracExamCenter2.Visible == true && DdlPracExamCentre2.SelectedValue == "0")
                {
                    throw new Exception("Please select Practical Exam Centre 2");
                }
                if (DdlPracExamCentre1.SelectedValue == DdlPracExamCentre2.SelectedValue && DdlPracExamCentre1.SelectedValue != "0")
                {
                    throw new Exception("Both Exam Practical Centre Choices cannot be same.");
                }

                if (TrOnlTheoryExamCenter1.Visible == true && DdlOnlTheoryExamCentre1.SelectedValue == "0")
                {
                    throw new Exception("Please select Online Theory Exam Centre 1");
                }

                if (TrOnlTheoryExamCenter2.Visible == true && DdlOnlTheoryExamCentre2.SelectedValue == "0")
                {
                    throw new Exception("Please select Online Theory Exam Centre 2");
                }
                if ( DdlOnlTheoryExamCentre1.SelectedValue == DdlOnlTheoryExamCentre2.SelectedValue && DdlOnlTheoryExamCentre1.SelectedValue != "0")
                {
                    throw new Exception("Both Online Exam  Centre Choices cannot be same.");
                }
              
            }
            if (currentCourseID == 3 || currentCourseID == 4) //added by Reena
            {
                if (DdlExamCentre1.SelectedValue == "0")
                {
                    throw new Exception("Please select Exam Centre 1");
                }
                if (DdlExamCentre1.SelectedValue == "0")
                {
                    throw new Exception("Please select Exam Centre 1");
                }
                if (DdlExamCentre2.SelectedValue == "0")
                {
                    throw new Exception("Please select Exam Centre 2");
                }
                if (DdlExamCentre1.SelectedValue == DdlExamCentre2.SelectedValue && DdlExamCentre1.SelectedValue != "0")
                {
                    throw new Exception("Both Exam Centre Choices cannot be same.");
                }
               
                if (TrPracExamCenter2.Visible == true &&  DdlPracExamCentre1.SelectedValue == "0")
                {
                    throw new Exception("Please select Practical Exam Centre 1");
                }

                if (TrPracExamCenter2.Visible == true && DdlPracExamCentre2.SelectedValue == "0")
                {
                    throw new Exception("Please select Practical Exam Centre 2");
                }
                if (DdlPracExamCentre1.SelectedValue == DdlPracExamCentre2.SelectedValue && DdlPracExamCentre1.SelectedValue != "0")
                {
                    throw new Exception("Both Exam Practical Centre Choices cannot be same.");
                }

            }
            #endregion

            if (ddlModules.Visible == true)
            {
                if (chklistPracticals.Enabled == true)
                {
                    if (chklistPracticals.SelectedIndex < 0 && ddlModules.SelectedValue == "0")
                    {
                        throw new Exception("Please select either module for improvement or module for practical exam.");
                    }
                }
            }
            if (chkdisclamier.Checked == false)
            {
                throw new Exception("Please select the declaration.");
            }

            using (TransactionScope scope = new TransactionScope())
            {

                using (EConnectContext context = new EConnectContext())
                {
                    RegistrationDetail registrationDetail = (from s in context.RegistrationDetails
                                                             where s.CourseID == currentCourseID && s.CandidateID == entityID && s.RegistrationNo == registrationNumber
                                                             select s).FirstOrDefault();
                                       
                    Exam currentExam = CourseManager.GetNextExam(context, currentCourseID, registrationDetail.ApplicantTypeID);
                    Int32 feeTheory = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee)   ;                 
                    Int32 feePractical = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee);                    
                    Int32 theoryFee = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.TheorywithPracticalFee);
                    Int32 practicalFee = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFeewithTheory);
                         
                  
                    Int32 feeImprovement = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ImprovementOfPaperFee);
                    feeProcessing = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PostageFeeChargedTowardExaminationCorrespondence);
                    if (Request.QueryString["Appid"] == null && context.CourseExamApplications.Any(c => (c.ExamID == currentExam.ID && c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber)) == false)
                    {
                        if (context.CourseExamApplications.Any(c => (c.ExamID == currentExam.ID && c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber)))
                        {
                            lblerror.Text = "You have already applied for the this exam.";
                            lblerror.Visible = true;
                            btnSave.Visible = false;
                            tblMain.Visible = false;
                            return;
                        }
                        appl.ApplicantTypeID = Convert.ToInt32(ddlCandidateType.SelectedValue); // registrationDetail.ApplicantTypeID;
                        appl.ApplicationDate = DateTime.Now;
                        appl.PaymentSourceID = Convert.ToInt32(ddlPaymentOption.SelectedValue);
                        enmPaymentSource paymentSource = (enmPaymentSource)Convert.ToInt32(ddlPaymentOption.SelectedValue);
                        if (paymentSource == enmPaymentSource.Candidate)
                            appl.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate);
                        else
                            appl.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification);
                        appl.CandidateID = entityID;
                        appl.CourseCategoryID = registrationDetail.CourseCategoryID;
                        appl.CourseID = registrationDetail.CourseID;

                        if (currentCourseID == 1) //Reena

                        {
                            if (TrPracExamCenter1.Visible == true)
                            {
                                appl.PracExamCenter1ID = Convert.ToInt32(DdlPracExamCentre1.SelectedValue);
                                appl.PracExamCenter2ID = Convert.ToInt32(DdlPracExamCentre2.SelectedValue);
                            }

                            //Added By Reena(03/05/2023)
                            if (TrOnlTheoryExamCenter1.Visible == true)
                            {
                                appl.OnlineExamCenter1ID = Convert.ToInt32(DdlOnlTheoryExamCentre1.SelectedValue);
                                appl.OnlineExamCenter2ID = Convert.ToInt32(DdlOnlTheoryExamCentre2.SelectedValue);
                            }
                        }
                        if (currentCourseID == 2) //Reena
                        {
                            if (TrExamCentre1.Visible == true)
                            {
                                appl.ExamCenter1ID = Convert.ToInt32(DdlExamCentre1.SelectedValue);
                                appl.ExamCenter2ID = Convert.ToInt32(DdlExamCentre2.SelectedValue);
                            }
                            if (TrPracExamCenter1.Visible == true)
                            {
                                appl.PracExamCenter1ID = Convert.ToInt32(DdlPracExamCentre1.SelectedValue);
                                appl.PracExamCenter2ID = Convert.ToInt32(DdlPracExamCentre2.SelectedValue);
                            }

                            //Added By Reena(03/05/2023)
                            if (TrOnlTheoryExamCenter1.Visible == true)
                            {
                                appl.OnlineExamCenter1ID = Convert.ToInt32(DdlOnlTheoryExamCentre1.SelectedValue);
                                appl.OnlineExamCenter2ID = Convert.ToInt32(DdlOnlTheoryExamCentre2.SelectedValue);
                            }
                           

                        }
                        if (currentCourseID == 3 || currentCourseID == 4)//Reena
                        {
                            if (TrExamCentre1.Visible == true)
                            {
                                appl.ExamCenter1ID = Convert.ToInt32(DdlExamCentre1.SelectedValue);
                                appl.ExamCenter2ID = Convert.ToInt32(DdlExamCentre2.SelectedValue);
                            }

                            //Added By Reena
                            if (TrPracExamCenter1.Visible == true)
                            {
                                appl.PracExamCenter1ID = Convert.ToInt32(DdlPracExamCentre1.SelectedValue);
                                appl.PracExamCenter2ID = Convert.ToInt32(DdlPracExamCentre2.SelectedValue);
                            }

                            
                        }
                        else if (currentCourseID == 1213)
                        {
                            if (TrPracExamCenter1.Visible == true)
                            {
                                appl.PracExamCenter1ID = Convert.ToInt32(DdlPracExamCentre1.SelectedValue);
                                appl.PracExamCenter2ID = Convert.ToInt32(DdlPracExamCentre2.SelectedValue);
                            }

                            if (TrExamCentre1.Visible == true)
                            {
                                appl.ExamCenter1ID = Convert.ToInt32(DdlExamCentre1.SelectedValue);
                                appl.ExamCenter2ID = Convert.ToInt32(DdlExamCentre2.SelectedValue);
                            }
                        }
                        else
                        {
                            if (TrExamCentre1.Visible == true)
                            {
                                appl.ExamCenter1ID = Convert.ToInt32(DdlExamCentre1.SelectedValue);
                                appl.ExamCenter2ID = Convert.ToInt32(DdlExamCentre2.SelectedValue);
                            }
                        }
                      

                        appl.ExamID = currentExam.ID;
                        appl.FeeAmount = Convert.ToDecimal(lblTotalFee.Text);
                        appl.FeeTypeID = Convert.ToInt32(enmFeeType.ExaminationFee);
                        if (registrationDetail.enmApplicantType == enmApplicantType.Institute)
                            appl.InstituteID = registrationDetail.InstituteID;
                        if (ddlModules.SelectedValue != "0" && ddlModules.Visible == true)
                        {
                            appl.IsImprovementApplication = true;
                        }
                        else if (ddlModules.SelectedValue == "0" && ddlModules.Visible == true)
                        {
                            appl.IsImprovementApplication = false;
                        }
                        if (CourseManager.IsLateFeeApplicable(context, appl.ExamID, appl.ApplicantTypeID))
                        {
                            appl.LateFeeImposed = true;
                            appl.LateFeeAmount = Convert.ToDecimal(CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.LateFeeExam));
                        }
                        if (rblMedium.SelectedValue == "1")
                            appl.MediumOfExamID = Convert.ToInt32(enmLanguage.English);
                        else
                            appl.MediumOfExamID = Convert.ToInt32(enmLanguage.Hindi);
                        appl.RegistrationNumber = registrationNumber;
                        appl.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);


                        //Fee Detail
                        appl.NumberOfTheoryModulesApplied = theoryCount;
                        appl.NumberOfPracticalModulesApplied = practicalCount;
                       // attemptedLastExams = CourseManager.GetListOfAttemptedExams(context, currentCourseID, registrationNumber, entityID).OrderByDescending(d => new { d.ExamYear, d.ExamMonth }).FirstOrDefault();
                        attemptedLastExams = CourseManager.GetListOfTheoryPassed(context, currentCourseID, registrationNumber, entityID).OrderByDescending(d => new { d.ExamYear, d.ExamMonth }).FirstOrDefault();
                        if (attemptedLastExams != null)
                            appl.PreviousExamID = attemptedLastExams.ID;
                        context.CourseExamApplications.Add(appl);
                        context.SaveChanges();
                    }
                    else
                    {
                        Int64 applID = 0;
                        if (Request.QueryString["Appid"] != null)
                            applID = Convert.ToInt64(Request.QueryString["Appid"]);
                        else
                            applID = context.CourseExamApplications.Where(c => (c.ExamID == currentExam.ID && c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber)).FirstOrDefault().ID;
                        appl = context.CourseExamApplications.Find(applID);
                        appl.ApplicantTypeID = Convert.ToInt32(ddlCandidateType.SelectedValue);// registrationDetail.ApplicantTypeID;
                        appl.ApplicationDate = DateTime.Now;
                        appl.PaymentSourceID = Convert.ToInt32(ddlPaymentOption.SelectedValue);

                        //appl.FeeAmount = Convert.ToDecimal(lblTotalFee.Text);
                        //appl.NumberOfTheoryModulesApplied = Convert.ToInt32(lblTheoryCount.Text.Trim());
                        //appl.NumberOfPracticalModulesApplied = Convert.ToInt32(lblPracticalCount.Text.Trim()); 

                        if (appl.enmApplicantType == enmApplicantType.Direct)
                            appl.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate);
                        enmPaymentSource paymentSource = (enmPaymentSource)Convert.ToInt32(ddlPaymentOption.SelectedValue);
                        if (paymentSource == enmPaymentSource.Candidate)
                            appl.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate);
                        else
                            appl.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification);

                        if (currentCourseID == 1) //Reena
                        {
                            if (TrPracExamCenter1.Visible == true)
                            {
                                appl.PracExamCenter1ID = Convert.ToInt32(DdlPracExamCentre1.SelectedValue);
                                appl.PracExamCenter2ID = Convert.ToInt32(DdlPracExamCentre2.SelectedValue);
                            }

                            //Added By Reena(03/05/2023)
                            if (TrOnlTheoryExamCenter1.Visible == true)
                            {
                                appl.OnlineExamCenter1ID = Convert.ToInt32(DdlOnlTheoryExamCentre1.SelectedValue);
                                appl.OnlineExamCenter2ID = Convert.ToInt32(DdlOnlTheoryExamCentre2.SelectedValue);
                            }
                        }
                        if (currentCourseID == 2) //Reena
                        {
                            if (TrExamCentre1.Visible == true)
                            {
                                appl.ExamCenter1ID = Convert.ToInt32(DdlExamCentre1.SelectedValue);
                                appl.ExamCenter2ID = Convert.ToInt32(DdlExamCentre2.SelectedValue);
                            }
                            if (TrPracExamCenter1.Visible == true)
                            {
                                appl.PracExamCenter1ID = Convert.ToInt32(DdlPracExamCentre1.SelectedValue);
                                appl.PracExamCenter2ID = Convert.ToInt32(DdlPracExamCentre2.SelectedValue);
                            }

                            //Added By Reena(03/05/2023)
                            if (TrOnlTheoryExamCenter1.Visible == true)
                            {
                                appl.OnlineExamCenter1ID = Convert.ToInt32(DdlOnlTheoryExamCentre1.SelectedValue);
                                appl.OnlineExamCenter2ID = Convert.ToInt32(DdlOnlTheoryExamCentre2.SelectedValue);
                            }


                        }
                        if (currentCourseID == 3 || currentCourseID == 4)//Reena
                        {
                            if (TrExamCentre1.Visible == true)
                            {
                                appl.ExamCenter1ID = Convert.ToInt32(DdlExamCentre1.SelectedValue);
                                appl.ExamCenter2ID = Convert.ToInt32(DdlExamCentre2.SelectedValue);
                            }

                            //Added By Reena
                            if (TrPracExamCenter1.Visible == true)
                            {
                                appl.PracExamCenter1ID = Convert.ToInt32(DdlPracExamCentre1.SelectedValue);
                                appl.PracExamCenter2ID = Convert.ToInt32(DdlPracExamCentre2.SelectedValue);
                            }


                        }
                        else if (currentCourseID == 1213)
                        {
                            if (TrPracExamCenter1.Visible == true)
                            {
                                appl.PracExamCenter1ID = Convert.ToInt32(DdlPracExamCentre1.SelectedValue);
                                appl.PracExamCenter2ID = Convert.ToInt32(DdlPracExamCentre2.SelectedValue);
                            }
                            if (TrExamCentre1.Visible == true)
                            {
                                appl.ExamCenter1ID = Convert.ToInt32(DdlExamCentre1.SelectedValue);
                                appl.ExamCenter2ID = Convert.ToInt32(DdlExamCentre2.SelectedValue);
                            }
                        }
                        else
                        {
                            if (TrExamCentre1.Visible == true)
                            {
                                appl.ExamCenter1ID = Convert.ToInt32(DdlExamCentre1.SelectedValue);
                                appl.ExamCenter2ID = Convert.ToInt32(DdlExamCentre2.SelectedValue);
                            }
                        }
                       

                        if (CourseManager.IsLateFeeApplicable(context, appl.ExamID, appl.ApplicantTypeID))
                        {
                            appl.LateFeeImposed = true;
                            appl.LateFeeAmount = Convert.ToDecimal(CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.LateFeeExam));
                        }
                        if (rblMedium.SelectedValue == "1")
                            appl.MediumOfExamID = Convert.ToInt32(enmLanguage.English);
                        else
                            appl.MediumOfExamID = Convert.ToInt32(enmLanguage.Hindi);
                        //appl.ApplicationDate = DateTime.Now;
                        if (ddlModules.SelectedValue != "0" && ddlModules.Visible == true)
                        {
                            appl.IsImprovementApplication = true;
                        }
                        else if (ddlModules.SelectedValue == "0" && ddlModules.Visible == true)
                        {
                            appl.IsImprovementApplication = false;
                        }
                        context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        //context.Database.ExecuteSqlCommand("Delete from Course_Exam_Application_Detail where Course_Exam_Appl_ID = " + applID);

                        //November_2024
                        SqlParameter[] para1 = { /*new SqlParameter("@applID", applID) ,*/
                                                    new SqlParameter("@applID", SqlDbType.BigInt) { Value = applID }
                                                                            };
                        
                        context.Database.ExecuteSqlCommand("Delete from Course_Exam_Application_Detail where Course_Exam_Appl_ID =@applID ", para1);

                        context.SaveChanges();
                    }

                    if (ddlModules.Visible == false)
                    {
                       
                            foreach (ListItem item in chklistModules.Items)
                            {
                                if (item.Selected == true)
                                {
                                            
                                   Module th_with_prac = context.Modules.Find( Convert.ToInt32 (item.Value));
                                   Int32 TheoryWithPractical =   Convert.ToInt32(th_with_prac.TheoryWithPractical) ;
                                     

                                    theoryCount++;
                                    CourseExamApplicationDetail module = new CourseExamApplicationDetail();
                                    module.CandidateID = entityID;
                                    module.CourseExamApplicationID = appl.ID;
                                    module.CourseID = currentCourseID;
                                    module.ExamID = appl.ExamID;
                                    module.ExamMonth = currentExam.ExamMonth;
                                    module.ExamYear = currentExam.ExamYear;
                                 //   module.FeeAmount = feeTheory;

                                    if (currentCourseID == 1)
                                    {
                                        module.FeeAmount = (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 1) ? feeTheory : theoryFee;
                                        // TotalTheoryFee += feeTheory;
                                        TotalTheoryFee += (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 1) ? feeTheory : theoryFee;
                                    }
                                    else if (currentCourseID == 2)
                                    {
                                        module.FeeAmount = TheoryWithPractical == 0  ? feeTheory :  theoryFee  ;
                                        // TotalTheoryFee += feeTheory;
                                        TotalTheoryFee += TheoryWithPractical == 0 ? feeTheory : theoryFee ;
                                    }
                                    else if (currentCourseID == 1213)
                                    {
                                        //module.FeeAmount = theoryFee;
                                        //// TotalTheoryFee += feeTheory;
                                        //TotalTheoryFee +=  theoryFee;

                                        module.FeeAmount = TheoryWithPractical == 0 ? feeTheory : theoryFee;
                                        // TotalTheoryFee += feeTheory;
                                        TotalTheoryFee += TheoryWithPractical == 0 ? feeTheory : theoryFee;
                                    }
                                    else
                                    {
                                        module.FeeAmount = feeTheory;
                                        TotalTheoryFee += feeTheory;
                                    }
                                    if (registrationDetail.enmApplicantType == enmApplicantType.Institute)
                                        module.InstituteID = registrationDetail.InstituteID;
                                    module.ModuleID = Convert.ToInt32(item.Value);
                                    module.RegistrationNumber = registrationDetail.RegistrationNo;
                                    context.CourseExamApplicationDetails.Add(module);
                                }

                            }
                            foreach (ListItem item in chklistPracticals.Items)
                            {
                                if (item.Selected == true)
                                {

                                    Module th_with_prac = context.Modules.Find(Convert.ToInt32(item.Value));
                                    Int32 TheoryWithPractical = Convert.ToInt32(th_with_prac.TheoryWithPractical);

                                    practicalCount++;
                                    CourseExamApplicationDetail module = new CourseExamApplicationDetail();
                                    module.CandidateID = entityID;
                                    module.CourseExamApplicationID = appl.ID;
                                    module.CourseID = currentCourseID;
                                    module.ExamID = appl.ExamID;
                                    module.ExamMonth = currentExam.ExamMonth;
                                    module.ExamYear = currentExam.ExamYear;
                                    if (currentCourseID == 1)
                                    {
                                        module.FeeAmount = (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 1) ? feePractical : practicalFee;
                                        // module.FeeAmount =  feePractical ;
                                        // totalPracticalFee += feePractical;
                                        totalPracticalFee += (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 1) ? feePractical : practicalFee;
                                    }
                                    else if (currentCourseID == 2)
                                    {
                                        module.FeeAmount = TheoryWithPractical == 0 ? feePractical : practicalFee;
                                      //  module.FeeAmount = TheoryWithPractical == false ? feePractical : practicalFee;
                                        // module.FeeAmount =  feePractical ;
                                        // totalPracticalFee += feePractical;
                                        totalPracticalFee += TheoryWithPractical == 0 ? feePractical : practicalFee;
                                    //    totalPracticalFee += TheoryWithPractical == false ? feePractical : practicalFee;


                                    }
                                    else
                                    {
                                        module.FeeAmount = feePractical;
                                        totalPracticalFee += feePractical;
                                    }

                                    if (registrationDetail.enmApplicantType == enmApplicantType.Institute)
                                        module.InstituteID = registrationDetail.InstituteID;
                                    module.ModuleID = Convert.ToInt32(item.Value);
                                    module.RegistrationNumber = registrationDetail.RegistrationNo;
                                    context.CourseExamApplicationDetails.Add(module);
                                }
                            }

                        }
               
                    else if (ddlModules.Visible == true)
                    {
                        foreach (ListItem item in chklistPracticals.Items)
                        {
                            if (item.Selected == true)
                            {
                                practicalCount++;
                                CourseExamApplicationDetail module = new CourseExamApplicationDetail();
                                module.CandidateID = entityID;
                                module.CourseExamApplicationID = appl.ID;
                                module.CourseID = currentCourseID;
                                module.ExamID = appl.ExamID;
                                module.ExamMonth = currentExam.ExamMonth;
                                module.ExamYear = currentExam.ExamYear;
                                module.FeeAmount = feePractical;
                                totalPracticalFee += feePractical;
                                if (registrationDetail.enmApplicantType == enmApplicantType.Institute)
                                    module.InstituteID = registrationDetail.InstituteID;
                                module.ModuleID = Convert.ToInt32(item.Value);
                                module.RegistrationNumber = registrationDetail.RegistrationNo;
                                context.CourseExamApplicationDetails.Add(module);
                            }
                        }
                        if (ddlModules.SelectedValue != "0")
                        {
                            theoryCount++;
                            CourseExamApplicationDetail moduleimprov = new CourseExamApplicationDetail();
                            moduleimprov.CandidateID = entityID;
                            moduleimprov.CourseExamApplicationID = appl.ID;
                            moduleimprov.CourseID = currentCourseID;
                            moduleimprov.ExamID = appl.ExamID;
                            moduleimprov.ExamMonth = currentExam.ExamMonth;
                            moduleimprov.ExamYear = currentExam.ExamYear;
                            moduleimprov.FeeAmount = feeImprovement;
                            TotalTheoryFee = feeImprovement;
                            if (registrationDetail.enmApplicantType == enmApplicantType.Institute)
                                moduleimprov.InstituteID = registrationDetail.InstituteID;
                            moduleimprov.ModuleID = Convert.ToInt32(ddlModules.SelectedValue);
                            moduleimprov.RegistrationNumber = registrationDetail.RegistrationNo;
                            context.CourseExamApplicationDetails.Add(moduleimprov);
                        }
                    }
                    else
	             	{
                        theoryCount++;
                        CourseExamApplicationDetail module = new CourseExamApplicationDetail();
                        module.CandidateID = entityID;
                        module.CourseExamApplicationID = appl.ID;
                        module.CourseID = currentCourseID;
                        module.ExamID = appl.ExamID;
                        module.ExamMonth = currentExam.ExamMonth;
                        module.ExamYear = currentExam.ExamYear;
                        module.FeeAmount = feeImprovement;
                        TotalTheoryFee = feeImprovement;
                        if (registrationDetail.enmApplicantType == enmApplicantType.Institute)
                            module.InstituteID = registrationDetail.InstituteID;
                        module.ModuleID = Convert.ToInt32(ddlModules.SelectedValue);
                        module.RegistrationNumber = registrationDetail.RegistrationNo;
                        context.CourseExamApplicationDetails.Add(module);

                        if (Convert.ToInt16(ddlModules.SelectedValue)== 929 || Convert.ToInt16(ddlModules.SelectedValue)== 930 || Convert.ToInt16(ddlModules.SelectedValue)== 931 || Convert.ToInt16(ddlModules.SelectedValue)== 932 || Convert.ToInt16(ddlModules.SelectedValue)== 938 || Convert.ToInt16(ddlModules.SelectedValue)== 939 || Convert.ToInt16(ddlModules.SelectedValue)== 940 || Convert.ToInt16(ddlModules.SelectedValue)== 941 )
                       
                        {
                            practicalCount++;
                            CourseExamApplicationDetail module1 = new CourseExamApplicationDetail();
                            module1.CandidateID = entityID;
                            module1.CourseExamApplicationID = appl.ID;
                            module1.CourseID = currentCourseID;
                            module1.ExamID = appl.ExamID;
                            module1.ExamMonth = currentExam.ExamMonth;
                            module1.ExamYear = currentExam.ExamYear;
                            module1.FeeAmount = feePractical;
                            totalPracticalFee += feePractical;
                            if (registrationDetail.enmApplicantType == enmApplicantType.Institute)
                                module1.InstituteID = registrationDetail.InstituteID;
                            module1.ModuleID = (
                                Convert.ToInt32(ddlModules.SelectedValue) == 929 ? 933 : Convert.ToInt32(ddlModules.SelectedValue) == 930 ? 934 : Convert.ToInt32(ddlModules.SelectedValue) == 931 ? 935 : Convert.ToInt32(ddlModules.SelectedValue) == 932 ? 936 : Convert.ToInt32(ddlModules.SelectedValue) == 938 ? 956 : Convert.ToInt32(ddlModules.SelectedValue) == 939 ? 957  : Convert.ToInt32(ddlModules.SelectedValue) == 940 ? 958  : Convert.ToInt32(ddlModules.SelectedValue) == 940 ? 959  :0)
                                ;
                            module1.RegistrationNumber = registrationDetail.RegistrationNo;
                            context.CourseExamApplicationDetails.Add(module1);
                        }

                    }

                    /*Commented 21 Nov 2022{
                        //Improvement Case;
                        theoryCount++;
                        CourseExamApplicationDetail module = new CourseExamApplicationDetail();
                        module.CandidateID = entityID;
                        module.CourseExamApplicationID = appl.ID;
                        module.CourseID = currentCourseID;
                        module.ExamID = appl.ExamID;
                        module.ExamMonth = currentExam.ExamMonth;
                        module.ExamYear = currentExam.ExamYear;
                        module.FeeAmount = feeImprovement;
                        TotalTheoryFee = feeImprovement;
                        if (registrationDetail.enmApplicantType == enmApplicantType.Institute)
                            module.InstituteID = registrationDetail.InstituteID;
                        module.ModuleID = Convert.ToInt32(ddlModules.SelectedValue);
                        module.RegistrationNumber = registrationDetail.RegistrationNo;
                        context.CourseExamApplicationDetails.Add(module);
                    }*/
                    context.SaveChanges();
                    //Invalid column name 'Present_Status'.
                    //Invalid column name 'UpdatedByUser_UserID'.
                };

                if (theoryCount == 0 && practicalCount == 0)
                {
                    throw new Exception("Please select at least one theory/practical module to apply.");
                }
                scope.Complete();
            };
            using (EConnectContext context = new EConnectContext())
            {
                if (ddlModules.Visible == true && ddlModules.SelectedValue == "0")
                {
                    appl.IsImprovementApplication = false;
                }
                else if (ddlModules.Visible == true && ddlModules.SelectedValue != "0")
                {
                    appl.IsImprovementApplication = true;
                }
                appl = context.CourseExamApplications.Find(appl.ID);

                appl.FeeAmount = Convert.ToDecimal((totalPracticalFee + TotalTheoryFee + feeProcessing));
                if (appl.LateFeeAmount.HasValue)
                    appl.FeeAmount += appl.LateFeeAmount.Value;
                appl.NumberOfTheoryModulesApplied = theoryCount;
                appl.NumberOfPracticalModulesApplied = practicalCount;
                context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();   //  examStartDate = Convert.ToDateTime(Request.QueryString["examStartDate"]);
            }
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("ExamFormPreview.aspx?AppID=" + appl.ID.ToString() + "&examStartDate=" + examStartDate.ToString()), false);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void UpdateFeeDetails()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 theoryCount = 0;
                Int32 practicalCount = 0;
                Int32 theoryWithPracticalCount = 0;

                if ((currentCourseID == 1 || currentCourseID == 2 || currentCourseID == 3) && (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 0 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL == 0 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_B_LVL==0))
                {
                   Int32 j = 0;
                    foreach (ListItem item in chklistModules.Items)
                    {
                        Module th_with_prac = context.Modules.Find(Convert.ToInt32(item.Value));
                        Int32 i = th_with_prac.ModuleNUmber;
                        i = i - 1;

                        if (item.Selected == true)
                        {
                            theoryCount++;                           
                            if (i < 4)
                            {
                                theoryWithPracticalCount++;                               
                                chklistPracticals.Items[j].Selected = true;
                                chklistPracticals.Items[j].Enabled = false;
                            }
                        }
                        if (item.Selected == false && i < 4)
                        {                          
                            chklistPracticals.Items[j].Selected = false;
                            chklistPracticals.Items[j].Enabled = false;
                        }
                        j++;
                    }
                }
                else if (currentCourseID == 1213)
                {
                    Int32 j = 0;
                    foreach (ListItem item in chklistModules.Items)
                    {
                        //if (item.Selected == true)
                        //    theoryCount++;
                        

                        Module th_with_prac = context.Modules.Find(Convert.ToInt32(item.Value));
                        Int32 i = th_with_prac.ModuleNUmber;
                        i = i - 1;

                        if (item.Selected == true)
                        {
                            theoryCount++;
                            if (i < 4)
                            {
                                theoryWithPracticalCount++;
                                chklistPracticals.Items[j].Selected = true;
                                chklistPracticals.Items[j].Enabled = false;
                            }
                        }
                        if (item.Selected == false && i < 4)
                        {
                            chklistPracticals.Items[j].Selected = false;
                            chklistPracticals.Items[j].Enabled = false;
                        }
                        j++;

                    }
                }
                else
                {
                    foreach (ListItem item in chklistModules.Items)
                    {
                        if (item.Selected == true)
                            theoryCount++;
                    }
                }
                 foreach (ListItem item in chklistPracticals.Items)
                 {
                     if (item.Selected == true)
                         practicalCount++;
                 }
                 if (theoryWithPracticalCount != 0)
                 {
                     theoryCount = theoryCount - theoryWithPracticalCount;
                     practicalCount = practicalCount - theoryWithPracticalCount;
                 }
                 else if (theoryWithPracticalCount != 0 && currentCourseID == 1213)
                 {
                     theoryCount = theoryCount - theoryWithPracticalCount;
                     practicalCount = 0;
                 }
                if (ddlModules.SelectedValue != "0")
                    theoryCount = 1;
            
                selectedModules.Value = (theoryCount + practicalCount).ToString();
                lblTheoryCount.Text = "TC : "+ theoryCount.ToString() + " TPC :" + theoryWithPracticalCount.ToString();               

                 if (theoryWithPracticalCount == 0 && theoryCount == 0  && practicalCount != 0)
                 {
                    // lblTheoryFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.TheorywithPracticalFee).ToString("F") + "*" + theoryWithPracticalCount.ToString() + "+" + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee).ToString("F") + "*" + theoryCount.ToString();
                     lblPracticalFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee).ToString("F") + "*" + practicalCount.ToString();
                     lblTotalPracticalFee.Text = (CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee) * practicalCount).ToString("F");
                     //lblTotalTheoryFee.Text = (CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.TheorywithPracticalFee) * theoryWithPracticalCount + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee) * theoryCount).ToString("F");
                 }
                 if (theoryWithPracticalCount == 0 && theoryCount != 0 && practicalCount == 0)
                 {
                     lblTheoryFee.Text =  CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee).ToString("F") + "*" + theoryCount.ToString();
                     lblPracticalFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFeewithTheory).ToString("F") + "*" + practicalCount.ToString();
                     lblTotalPracticalFee.Text = (CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFeewithTheory) * practicalCount).ToString("F");
                     lblTotalTheoryFee.Text = (CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee) * theoryCount).ToString("F");
                 }
                 if (theoryWithPracticalCount == 0 && theoryCount != 0 && practicalCount != 0)
                 {
                     lblTheoryFee.Text =  CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee).ToString("F") + "*" + theoryCount.ToString();
                     lblPracticalFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee).ToString("F") + "*" + practicalCount.ToString();
                     lblTotalPracticalFee.Text = (CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee) * practicalCount).ToString("F");
                     lblTotalTheoryFee.Text = (CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee) * theoryCount).ToString("F");
                 }
                 if (theoryWithPracticalCount != 0 && theoryCount == 0 && practicalCount == 0)
                 {
                     lblTheoryFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.TheorywithPracticalFee).ToString("F") + "*" + theoryWithPracticalCount.ToString() ;
                     lblPracticalFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFeewithTheory).ToString("F") + "*" + practicalCount.ToString();
                     lblTotalPracticalFee.Text = (CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFeewithTheory) * practicalCount).ToString("F");
                     lblTotalTheoryFee.Text = (CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.TheorywithPracticalFee) * theoryWithPracticalCount ).ToString("F");
                 }
                 if (theoryWithPracticalCount != 0 && theoryCount == 0 && practicalCount != 0)
                 {
                     lblTheoryFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.TheorywithPracticalFee).ToString("F") + "*" + theoryWithPracticalCount.ToString() ;
                     lblPracticalFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee).ToString("F") + "*" + practicalCount.ToString();
                     lblTotalPracticalFee.Text = (CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee) * practicalCount).ToString("F");
                     lblTotalTheoryFee.Text = (CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.TheorywithPracticalFee) * theoryWithPracticalCount).ToString("F");
                 }
                 if (theoryWithPracticalCount != 0 && theoryCount != 0 && practicalCount == 0)
                 {
                     lblTheoryFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.TheorywithPracticalFee).ToString("F") + "*" + theoryWithPracticalCount.ToString() + "+" + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee).ToString("F") + "*" + theoryCount.ToString();
                     lblPracticalFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFeewithTheory).ToString("F") + "*" + practicalCount.ToString();
                     lblTotalPracticalFee.Text = (CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFeewithTheory) * practicalCount).ToString("F");
                     lblTotalTheoryFee.Text = (CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.TheorywithPracticalFee) * theoryWithPracticalCount + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee) * theoryCount).ToString("F");
                 }
                 if (theoryWithPracticalCount != 0 && theoryCount != 0 && practicalCount != 0)
                 {
                     lblTheoryFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.TheorywithPracticalFee).ToString("F") + "*" + theoryWithPracticalCount.ToString() + "+" + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee).ToString("F") + "*" + theoryCount.ToString();
                     lblPracticalFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee).ToString("F") + "*" + practicalCount.ToString();
                     lblTotalPracticalFee.Text = (CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee) * practicalCount).ToString("F");
                     lblTotalTheoryFee.Text = (CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.TheorywithPracticalFee) * theoryWithPracticalCount + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee) * theoryCount).ToString("F");
                 }
                 if (theoryWithPracticalCount == 0 && theoryCount == 0 && practicalCount == 0)
                 {

                     if ((currentCourseID == 1 || currentCourseID == 2 || currentCourseID == 3) && (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 1 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL == 1 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_B_LVL==1))
                     {                        
                         lblTheoryFee.Text = "750.00";
                         lblPracticalFee.Text = "500.00";
                     }
                     else if ((currentCourseID == 1 || currentCourseID == 2 || currentCourseID == 3) && (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 0 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL == 0 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_B_LVL==0))
                     {
                         lblTheoryFee.Text = "1000.00/750.00";
                         lblPracticalFee.Text = "500.00";
                     }
                     //lblTheoryFee.Text = "0.00";
                     //lblPracticalFee.Text = "0.00";
                     lblTotalPracticalFee.Text = "0.00";
                     lblTotalTheoryFee.Text = "0.00";
                     lblTotalFee.Text ="0.00";
                 //  lblTotalProcessingFee.Text = "0.00";
                 }
                 lblPracticalCount.Text = practicalCount.ToString();
                 //--lblPracticalCount.Text = "0";
                
                //selectedModules.Value = (theoryCount + practicalCount).ToString();
                //lblTheoryCount.Text = theoryCount.ToString();
                //lblTotalTheoryFee.Text = (Convert.ToSingle(lblTheoryFee.Text) * theoryCount).ToString("F");
                //lblPracticalCount.Text = practicalCount.ToString();
                //lblTotalPracticalFee.Text = (Convert.ToSingle(lblPracticalFee.Text) * practicalCount).ToString("F");

                 if (lblTotalLateFee.Text == "")
                 {
                     lblTotalLateFee.Text = "0.00";
                 }
              
                 lblTotalFee.Text = (Convert.ToSingle(lblTotalTheoryFee.Text) + Convert.ToSingle(lblTotalLateFee.Text) + Convert.ToSingle(lblTotalProcessingFee.Text) + Convert.ToSingle(lblTotalPracticalFee.Text)).ToString("F");        

                if (Convert.ToSingle(lblTotalLateFee.Text) == Convert.ToSingle(lblTotalFee.Text))
                {
                    lblTotalFee.Text = "0.0";
                }

                lblAmountHindi.Text = EConnect.Utils.Conversion.ConversionUtility.NumberToText(lblTotalFee.Text, EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English);            
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

  

    protected void chklistModules_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlStateFirst.SelectedValue = "0";
        ddlStateFirst_SelectedIndexChanged(ddlStateFirst, EventArgs.Empty);
        ALevel_OffLineExamCenterOption_Pract = 0;
        ALevel_OffLineExamCenterOption_Theory = 0;
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //================Start(1)===========Added on 06052020 for 'A'-Level 1 is to 1 selection from different elective grouID i.e.(9 and 10)===================== 
                if (currentCourseID == 2)
                {
                    //Int32 revision = Convert.ToInt32(rbtn_module_selection.SelectedItem.ToString().Substring(9));

                   
                    Int32 revision = Convert.ToInt32(lblRevision.Text.ToString().Substring(0, 1));
                    foreach (ListItem item in chklistModules.Items)
                    {
                        if (item.Selected == true)
                        {
                            Int32 compulsory = Convert.ToInt32(enmSelectionType.Compulsory);
                            Int32 selectedModuleID = Convert.ToInt32(item.Value);
                            Module selectedModule = context.Modules.Find(selectedModuleID);

                            //int valueexamModelid1 = 0;
                            //int valueexamModelid2 = 0;
                            //if (selectedModule.ExamModeId == 1)
                            //{
                            //    valueexamModelid1 = 1;
                            //    valueexamModelid2 = 0;
                            //}
                            //if (selectedModule.ExamModeId == 2)
                            //{
                            //    valueexamModelid2 = 2;
                            //    valueexamModelid1 = 0;
                            //}
                            //if (selectedModule.ExamModeId != valueexamModelid1)
                            //{
                            //    ddlStateFirst.SelectedValue = "0";
                            //    ddlStateFirst_SelectedIndexChanged(ddlStateFirst, EventArgs.Empty);
                            //}

                           
                            // --stsrt-- commented by abhi singh dated on 23042024 for testing unfeeze speical group selection in A Level Revision 6
                            if (selectedModule.enmSelectionType == enmSelectionType.Elective)
                            {
                                Int32 electiveGroupID = selectedModule.ElectiveGroup.Value;
                                Int32 selectedmodulesubnumber = selectedModule.ModuleSubNUmber;
                                var allelectivegroupid = (from m in context.Modules
                                                          where m.SelectionTypeID != compulsory && m.ModuleSubNUmber != selectedmodulesubnumber &&
                                                          m.CourseID == currentCourseID && (m.RevisionNumber == revision || m.RevisionNumber == 6)  // revision == 6 is added to enable A Level 1 to 1 selection after new pattern 
                                                          select m.ID).Distinct().ToList();

                                foreach (var electivesubnumber in allelectivegroupid.ToList())
                                {
                                    foreach (ListItem items in chklistModules.Items)
                                    {
                                        if (items.Value == electivesubnumber.ToString())
                                        {
                                            items.Enabled = false;
                                        }
                                    }
                                }
                            }
                            // -- End --
                        }
                    }

                    UpdateFeeDetails();
                    ValidateTheoryModules();
                }

                //======================================================================End(1)=======================================================================

                //================Start(2)===========Added on 15052020 for 'B'-Level ModuleId i.e. B252-R4 & BE7-R4 (Sw Testing and quality Management) Single selection from both ModuleId only==== 
                else if (currentCourseID == 3)
                {

                    //Int32 revision = Convert.ToInt32(rbtn_module_selection.SelectedItem.ToString().Substring(9));   

                    Int32 revision = Convert.ToInt32(lblRevision.Text.ToString().Substring(0, 1));

                    List<String> module_value = new List<String>();
                    foreach (ListItem item in chklistModules.Items)
                    {
                        if (item.Selected == true)
                        {
                            if (item.Value == "392")
                            {
                                module_value.Add("412");
                            }
                            else if (item.Value == "412")
                            {
                                module_value.Add("392");
                            }
                            if (module_value.Count != 0)
                            {
                                foreach (ListItem items in chklistModules.Items)
                                {
                                    if (items.Value.ToString() == module_value.FirstOrDefault().ToString())
                                    {
                                        items.Selected = false;
                                        items.Enabled = false;
                                        break;
                                    }
                                }
                            }
                        }

                    }
                    lblerror.Visible = lblerror.Visible;
                    ValidateTheoryModules();
                }

                //======================================================================End(2)========================================================================
                else
                {
                    lblerror.Visible = lblerror.Visible;

                    if (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 0)
                    {

                        UpdateFeeDetails();
                        ValidateTheoryModules();
                    }
                    else
                    {
                        ValidateTheoryModules();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
   
    protected void chklistModulesExmpt_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //================Start(1)===========Added on 06052020 for 'A'-Level 1 is to 1 selection from different elective grouID i.e.(9 and 10)===================== 
                if (currentCourseID == 2)
                {
                    //Int32 revision = Convert.ToInt32(rbtn_module_selection.SelectedItem.ToString().Substring(9));
                    Int32 revision = Convert.ToInt32(lblRevision.Text.ToString().Substring(0, 1));
                    foreach (ListItem item in chklistModulesExmpt.Items)
                    {
                        if (item.Selected == true)
                        {
                            Int32 compulsory = Convert.ToInt32(enmSelectionType.Compulsory);
                            Int32 selectedModuleID = Convert.ToInt32(item.Value);
                            Module selectedModule = context.Modules.Find(selectedModuleID);
                            if (selectedModule.enmSelectionType == enmSelectionType.Elective)
                            {
                                Int32 electiveGroupID = selectedModule.ElectiveGroup.Value;
                                Int32 selectedmodulesubnumber = selectedModule.ModuleSubNUmber;
                                var allelectivegroupid = (from m in context.Modules
                                                          where m.SelectionTypeID != compulsory && m.ModuleSubNUmber != selectedmodulesubnumber &&
                                                          m.CourseID == currentCourseID && (m.RevisionNumber == revision || m.RevisionNumber == 6)  // revision == 6 is added to enable A Level 1 to 1 selection after new pattern 
                                                          select m.ID).Distinct().ToList();

                                foreach (var electivesubnumber in allelectivegroupid.ToList())
                                {
                                    foreach (ListItem items in chklistModulesExmpt.Items)
                                    {
                                        if (items.Value == electivesubnumber.ToString())
                                        {
                                            items.Enabled = false;
                                        }
                                    }
                                }
                            }

                        }
                    }

                    UpdateFeeDetails();                                   
                    ValidateTheoryModules();
                }
               
                //======================================================================End(1)=======================================================================

                //================Start(2)===========Added on 15052020 for 'B'-Level ModuleId i.e. B252-R4 & BE7-R4 (Sw Testing and quality Management) Single selection from both ModuleId only==== 
                else if (currentCourseID == 3)
                {
                    
                    //Int32 revision = Convert.ToInt32(rbtn_module_selection.SelectedItem.ToString().Substring(9));   
               
                    Int32 revision = Convert.ToInt32(lblRevision.Text.ToString().Substring(0, 1));

                    List<String> module_value = new List<String>();
                    foreach (ListItem item in chklistModulesExmpt.Items)
                    {
                        if (item.Selected == true)
                        {
                            if (item.Value == "392")
                            {
                                module_value.Add("412");
                            }
                            else if (item.Value == "412")
                            {
                                module_value.Add("392");
                            }
                            if (module_value.Count != 0)
                            {
                                foreach (ListItem items in chklistModulesExmpt.Items)
                                {
                                    if (items.Value.ToString() == module_value.FirstOrDefault().ToString())
                                    {
                                        items.Selected = false;
                                        items.Enabled = false;
                                        break;
                                    }
                                }
                            }
                        }

                    }
                    lblerror.Visible = lblerror.Visible;
                    ValidateTheoryModules();
                }
                  
                //======================================================================End(2)========================================================================
                else
                {
                    lblerror.Visible = lblerror.Visible;

                    if (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 0)
                    {
                       
                        UpdateFeeDetails();
                        ValidateTheoryModules();
                    }
                    else
                    {
                        ValidateTheoryModules();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void chklistModulesOlevelSpecl_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Uncheck all other items
        //for (int i = 0; i < chklistModulesOlevelSpecl.Items.Count; i++)
        //{
        //    if (i != chklistModulesOlevelSpecl.SelectedIndex)
        //    {
        //        chklistModulesOlevelSpecl.(i, false);
        //    }
        //}
       
    }

    protected void chklistPracticals_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ValidatePracticalModules(1);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    //protected void chklistPracticalsExmpt_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ValidatePracticalModules();             
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message, true);
    //    }
    //}

    protected void ValidatePracticalModules( int sts)
    {
        if (currentCourseID == 2 && sts == 1)
        {
            ddlStateFirst.SelectedValue = "0";
            ddlStateFirst_SelectedIndexChanged(ddlStateFirst, EventArgs.Empty);
            ALevel_OffLineExamCenterOption_Pract = 0;
            ALevel_OffLineExamCenterOption_Theory = 0;
        }
        Int32 remainingTheoryModuleCount = 0; ;
        List<Exception> loopExceptions = new List<Exception>();
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                entityID = Convert.ToInt64(Session["EntityID"]);
                Exam exam = CourseManager.GetNextExam(context, currentCourseID, Convert.ToInt32(ddlCandidateType.SelectedValue));
                Int32 selectedModuleCountInThisExam = 0;
                Int32 project = Convert.ToInt32(enmModuleType.Project);
                Int64 firstRevisionNumber = CourseManager.GetCourseRevisionNumberAtRegistrationCommenced(context, currentCourseID, registrationNumber, entityID);

                currentevisionNumber = CourseManager.GetCourseRevisionNumberAtRegistrationCompleted(context, currentCourseID, registrationNumber, entityID);
                preevisionNumber = currentevisionNumber - 1;
                //Int32 remainingTheoryModuleCount = GetRemainingAllTheoryModuleCount(currentCourseID, registrationNumber, currentevisionNumber, entityID);
                if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == currentevisionNumber)
                {
                    remainingTheoryModuleCount = GetRemainingAllTheoryModuleCount(currentCourseID, registrationNumber, currentevisionNumber, entityID);
                }
                //if ( CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == preevisionNumber)  // commented by abhi singh dated on 16042024
                //{
                //    remainingTheoryModuleCount = GetRemainingAllTheoryModuleCount(currentCourseID, registrationNumber, preevisionNumber, entityID);
                //}
                else
                {
                    remainingTheoryModuleCount = GetRemainingAllTheoryModuleCount(currentCourseID, registrationNumber, currentevisionNumber, entityID);
                }
                if (remainingTheoryModuleCount > 0)
                {
                    // course_id 149 condition is temporary to apply only practical module for backlog cases dated 05-Sep-2019
                    if (currentCourseID == 149)
                    {
                    }
                    else
                    {

                        if (!(currentCourseID == 4 && firstRevisionNumber < currentevisionNumber && currentevisionNumber == 3) || currentCourseID != 4)
                        {
                            //Retreiving already passed modules
                            List<Int32> attemptedModules = new List<Int32>();
                            //Exam exam = CourseManager.GetNextExam(context, currentCourseID, Convert.ToInt32(ddlCandidateType.SelectedValue));
                            //attemptedModules = CourseManager.GetListtOfAttempteddModulesOfCurrentRevision(context, exam.ID, currentCourseID, registrationNumber, currentevisionNumber, entityID, enmModuleType.Theory).Where(c => c.ModuleTypeID != project).Select(c => c.ID).Distinct().ToList();
                            if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == currentevisionNumber)
                            {
                                attemptedModules = CourseManager.GetListtOfAttempteddModulesOfCurrentRevision(context, exam.ID, currentCourseID, registrationNumber, currentevisionNumber, entityID, enmModuleType.Theory).Where(c => c.ModuleTypeID != project).Select(c => c.ID).Distinct().ToList();
                            }
                            //if (rbtn_module_selection.SelectedValue == "pre" || (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == preevisionNumber && currentevisionNumber!=5)) // commented by abhi singh dated on 16042024
                            //{
                            
                            //    attemptedModules = CourseManager.GetListtOfAttempteddModulesOfCurrentRevision(context, exam.ID, currentCourseID, registrationNumber, preevisionNumber, entityID, enmModuleType.Theory).Where(c => c.ModuleTypeID != project).Select(c => c.ID).Distinct().ToList();
                            //}
                            else
                            {

                                attemptedModules = CourseManager.GetListtOfAttempteddModulesOfCurrentRevision(context, exam.ID, currentCourseID, registrationNumber, currentevisionNumber, entityID, enmModuleType.Theory).Where(c => c.ModuleTypeID != project).Select(c => c.ID).Distinct().ToList();
                            }
                            var attemptedmodules_anyrevision = (from s in context.CourseExamApplicationDetails
                                                                join m in context.Modules on s.ModuleID equals m.ID
                                                                where (s.CourseID == currentCourseID &&
                                                                        s.CandidateID == entityID &&
                                                                        s.RegistrationNumber == registrationNumber && s.ExamID != exam.ID &&
                                                                        m.ModuleTypeID != project)
                                                                orderby m.RevisionNumber, m.Code
                                                                select m.ID).Distinct().ToList();
                            //Selected Module in this exam
                            foreach (ListItem item in chklistModules.Items)
                            {
                                if (item.Selected == true)
                                {
                                    attemptedmodules_anyrevision.Add(Convert.ToInt32(item.Value));
                                    attemptedModules.Add(Convert.ToInt32(item.Value));
                                    selectedModuleCountInThisExam++;
                                }
                            }
                            if (attemptedModules.Count > 0 && attemptedmodules_anyrevision.Count > 0)
                            {
                                if (selectedModuleCountInThisExam >= remainingTheoryModuleCount)
                                {
                                    UpdateFeeDetails();
                                    return;
                                }
                                Int32 practicalModulesID = 0;
                                foreach (ListItem item in chklistPracticals.Items)
                                {
                                    if (item.Selected)
                                    {
                                        practicalModulesID = Convert.ToInt32(item.Value);
                                        Int32 compulsory = Convert.ToInt32(enmSelectionType.Compulsory);

                                        //<<<<<<<<<<<<<===========check whether apply in July2020 exam cycle due to non conducting of exam ===================
                                        Module selectedModule = context.Modules.Find(practicalModulesID);
                                        int? selectedModulexamMode = selectedModule.ExamModeId;
                                        //if (selectedModulexamMode == 2 || selectedModulexamMode == 3 )
                                        //{
                                        //    ddlStateFirst.SelectedValue = "0";
                                        //    ddlStateFirst_SelectedIndexChanged(ddlStateFirst, EventArgs.Empty);
                                        //}
                                        //Newly Added 
                                        //Int32 oldModuleID = 0;

                                        //if ((currentCourseID == 1 || currentCourseID == 2) && (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 0 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL == 0))
                                        //{
                                        //    Int32 ParityF = context.Parities.Where(x => x.NewModuleID == practicalModulesID).FirstOrDefault().ID;

                                        //    Parity parityS = context.Parities.Find(ParityF);

                                        //    if (parityS.NewRevisionNumber.Equals(6))
                                        //    {
                                        //        oldModuleID = parityS.OldModuleID;
                                        //    }
                                        //    else
                                        //    {
                                        //        oldModuleID = practicalModulesID;
                                        //    }
                                        //}

                                        var pendingResultCount = (from r in context.CourseExamApplicationDetails
                                                                  where r.ModuleID == practicalModulesID && (r.ResultGradeID.HasValue == false || r.ResultGradeID == 11) &&
                                                                  r.ExamID != exam.ID && r.CandidateID == entityID && r.RegistrationNumber == registrationNumber
                                                                  && r.CourseID == currentCourseID && r.Module.SelectionTypeID == compulsory && r.IsUnfairMeansCase == false
                                                                  select r);
                                        if (pendingResultCount.Count() > 0)
                                        {
                                            if ((pendingResultCount.FirstOrDefault().CourseExamApplication.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)) && (pendingResultCount.FirstOrDefault().CourseExamApplication.IsExported))
                                            {
                                                item.Selected = false;
                                                throw new Exception("You can not select this module - " + selectedModule.ShortName + " as you have already attempted this module in the last exam and result is pending.");
                                            }
                                        }
                                        else
                                        {
                                            var pendingresult_new_module_id = (from p in context.Parities
                                                                               where p.CourseID == currentCourseID && p.NewModuleID == practicalModulesID
                                                                               && p.OldRevisionNumber == preevisionNumber && p.NewRevisionNumber == currentevisionNumber
                                                                               select p.OldModuleID).Distinct().FirstOrDefault();
                                            var pendingResult_old_module_id = (from r in context.CourseExamApplicationDetails
                                                                               where r.ResultGradeID.HasValue == false && r.ExamID != exam.ID
                                                                               && r.CandidateID == entityID && r.RegistrationNumber == registrationNumber
                                                                               && r.CourseID == currentCourseID && r.IsUnfairMeansCase == false
                                                                               select r);

                                            foreach (var pending_old_mod in pendingResult_old_module_id.ToList())
                                            {
                                                try
                                                {
                                                    if (pending_old_mod.ModuleID == pendingresult_new_module_id && (pending_old_mod.CourseExamApplication.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)) && (pending_old_mod.CourseExamApplication.IsExported))
                                                    {
                                                        item.Selected = false;
                                                        throw new Exception("You can not select this module - " + selectedModule.ShortName + " as you have already attempted this module in the last exam in revision-" + preevisionNumber.ToString() + " and result is pending.");
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    loopExceptions.Add(ex);
                                                }
                                            }
                                        }
                                        //=================================================================================================>>>>>>>>>>>>>>>>>>
                                        //Eligibility for theory compulsory Modules
                                        var theoryCompulsoryModulesID = (from p in context.ModulePracticalEligibilities
                                                                         where p.PracticalModuleID == practicalModulesID && p.SelectionTypeID == compulsory
                                                                         select p.TheoryModuleID).Distinct().ToList();
                                        Boolean isEligible = true;
                                        foreach (Int32 moduleID in theoryCompulsoryModulesID.ToList())
                                        {
                                            if (!(attemptedModules.Contains(moduleID) || attemptedmodules_anyrevision.Contains(moduleID)))
                                            {
                                                isEligible = false;
                                                break;
                                                //    var listOfPassedModules = (from d in context.CourseExamApplicationDetails
                                                //                               join m in context.Modules on d.ModuleID equals m.ID
                                                //                               where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                                //                               d.Grade.IsPassed == true
                                                //                               orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                //                               select m.ID).ToList();

                                                //    string moduleCode = context.Modules.Find(moduleID).ShortName;
                                                //    moduleCode = moduleCode.Substring(0, moduleCode.IndexOf("-"));
                                                //    var modulesAtt = context.Modules.Where(a => listOfPassedModules.Contains(a.ID));
                                                //    modulesAtt = modulesAtt.Where(a => a.ShortName.Contains(moduleCode));
                                                //    var clearedModCount = modulesAtt.Count();
                                                //    if (clearedModCount <= 0)
                                                //    {
                                                //        isEligible = false;
                                                //        break;
                                                //    }
                                            }
                                            //else
                                            //{
                                            //    isEligible = false;
                                            //    break;
                                            //}

                                        }
                                        //Eligibility for theory elective Modules
                                        //var theoryElectiveModulesID = (from p in context.ModulePracticalEligibilities
                                        //                               where p.PracticalModuleID == practicalModulesID && p.SelectionTypeID != compulsory
                                        //                               select p.ElectiveGroup).Distinct().ToList();
                                        //var attemptedElctiveGroups = (from m in context.Modules
                                        //                              where attemptedModules.Contains(m.ID) && m.SelectionTypeID != compulsory
                                        //                              select m.ElectiveGroup).Distinct().ToList();
                                        //foreach (Int32 electiveGroupID in theoryElectiveModulesID.ToList())
                                        //{
                                        //    if (!attemptedElctiveGroups.Contains(electiveGroupID))
                                        //    {
                                        //        var listOfPassedModules = (from d in context.CourseExamApplicationDetails
                                        //                                   join m in context.Modules on d.ModuleID equals m.ID
                                        //                                   where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                        //                                   d.Grade.IsPassed == true && m.ElectiveGroup.HasValue && m.ElectiveGroup.Value == electiveGroupID
                                        //                                   orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                        //                                   select m.ID).ToList();

                                        //        var moduleCode = from d in context.Modules
                                        //                         where d.ElectiveGroup == electiveGroupID
                                        //                         select new
                                        //                         {
                                        //                             ShortName = d.ShortName,
                                        //                             NumberOfPapersToPass = d.NumberOfElectiveModulesAllowed
                                        //                         };
                                        //        var modulesAtt = context.Modules.Where(a => listOfPassedModules.Contains(a.ID));
                                        //        int match = 0;
                                        //        int required = 0;
                                        //        foreach (var aa in moduleCode)
                                        //        {
                                        //            var code = aa.ShortName.IndexOf("-") > 0 ? aa.ShortName.Substring(0, aa.ShortName.IndexOf("-")) : aa.ShortName;
                                        //            modulesAtt = modulesAtt.Where(a => a.ShortName.Contains(code));
                                        //            if (modulesAtt.Count() > 0)
                                        //                match++;
                                        //            required = aa.NumberOfPapersToPass.Value;
                                        //        }
                                        //        if (match < required)
                                        //        {
                                        //            isEligible = false;
                                        //            break;
                                        //        }
                                        //    }
                                        //}

                                        var theoryElectiveModulesID = (from p in context.ModulePracticalEligibilities
                                                                       where p.PracticalModuleID == practicalModulesID && p.SelectionTypeID != compulsory
                                                                       select p.ElectiveGroup).Distinct().ToList();
                                        var attemptedElctiveGroups = (from m in context.Modules
                                                                      where attemptedModules.Contains(m.ID) && m.SelectionTypeID != compulsory
                                                                      select m.ElectiveGroup).Distinct().ToList();
                                        var attemptedElctiveGroupsCount = (from m in context.Modules
                                                                           where attemptedModules.Contains(m.ID) && m.SelectionTypeID == 2
                                                                           select m.ElectiveGroup).ToList();

                                        foreach (Int32 electiveGroupID in theoryElectiveModulesID.ToList())
                                        {
                                            if (!attemptedElctiveGroups.Contains(electiveGroupID))
                                            {
                                                isEligible = false;
                                                break;
                                            }
                                            else
                                            {
                                                var NumberOfElectiveModulesAllowed = (from p in context.Modules
                                                                                      where p.ElectiveGroup == electiveGroupID
                                                                                      select p.NumberOfElectiveModulesAllowed).FirstOrDefault();
                                                if (attemptedElctiveGroupsCount.Count() < NumberOfElectiveModulesAllowed)
                                                {
                                                    isEligible = false;
                                                    break;
                                                }
                                            }
                                        }

                                        if (isEligible == false)
                                        {
                                            item.Selected = false;
                                            throw new Exception("You are not eligible to select this practical module. See practical eligibility criteria.");
                                        }
                                        if ((selectedModulexamMode == 2 || selectedModulexamMode == 3) && currentCourseID == 2)
                                        {
                                            ALevel_OffLineExamCenterOption_Pract = 2;
                                            //ALevel_OffLineExamCenterOption_Theory = 2;
                                        }
                                        //else
                                        //{
                                        //    ALevel_OffLineExamCenterOption_Theory = 2;
                                        //    ALevel_OffLineExamCenterOption_Pract = 0;
                                        //}

                                    }
                                    else
                                    {
                                        ALevel_OffLineExamCenterOption_Pract = 0;
                                    }

                                }

                            }
                            else
                            {
                                chklistPracticals.SelectedItem.Selected = false;
                                throw new Exception("You are not eligible to select this practical module. See practical eligibility criteria.");
                            }
                        }
                    }
                }
                else
                {
                    Int32 practicalModulesID = 0;
                    foreach (ListItem item in chklistPracticals.Items)
                    {
                        if (item.Selected)
                        {
                            practicalModulesID = Convert.ToInt32(item.Value);
                            Int32 compulsory = Convert.ToInt32(enmSelectionType.Compulsory);

                            //<<<<<<<<<<<<<===========check whether apply in July2020 exam cycle due to non conducting of exam ===================
                            Module selectedModule = context.Modules.Find(practicalModulesID);
                            int? selectedModulexamMode = selectedModule.ExamModeId;
                            if ((selectedModulexamMode == 2 || selectedModulexamMode == 3) && currentCourseID == 2)
                            {
                                ALevel_OffLineExamCenterOption_Pract = 2;
                                //ALevel_OffLineExamCenterOption_Theory = 2;
                            }
                        }
                        else
                        {
                            ALevel_OffLineExamCenterOption_Pract = 0;
                        }
                    }
                }
            };
            UpdateFeeDetails();
            if (loopExceptions.Count > 0)
            {
                foreach (var exe in loopExceptions)
                {
                    //ShowAlert(exe.Message);
                    throw exe;
                }
            }
        }
        catch (Exception ex)
        {
            UpdateFeeDetails();
            throw ex;
        }
    }
    public static int selectedModuleCount1 = 0;
               
    protected void ValidateTheoryModules()
    {
        Int32 remainingTheoryModuleCount = 0;
        //DataTable dt = new DataTable();
        //dt.Columns.Add("electiveGroupID", typeof(int));
        //dt.Columns.Add("electivegroupsubnumber", typeof(int));
        //dt.Columns.Add("electiveSelectionTypeId", typeof(int));
        //DataRow _dr = dt.NewRow();
        List<Exception> loopExceptions = new List<Exception>();
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Exam exam = CourseManager.GetNextExam(context, currentCourseID, Convert.ToInt32(ddlCandidateType.SelectedValue));
                Int32 project = Convert.ToInt32(enmModuleType.Project);
                Int32 elective = Convert.ToInt32(enmSelectionType.Elective);
                Int32 electiveGroupID = 0;
                Int32 compulsory = Convert.ToInt32(enmSelectionType.Compulsory);
                Int64 firstRevisionNumber = CourseManager.GetCourseRevisionNumberAtRegistrationCommenced(context, currentCourseID, registrationNumber, entityID);
                currentevisionNumber = CourseManager.GetCourseRevisionNumberAtRegistrationCompleted(context, currentCourseID, registrationNumber, entityID);
                if (currentevisionNumber != 1)
                    preevisionNumber = currentevisionNumber - 1;
                else
                    preevisionNumber = currentevisionNumber;
                Int32 applicationstatus = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);

                //-----remainingTheoryModuleCount = GetRemainingAllTheoryModuleCount(currentCourseID, registrationNumber, currentevisionNumber, entityID);-----comented on 250220
                //---------------------------
                //<<<<<<<<<<<<<<<====================added on 08/10/2020,only for NSQF and MAT-olevel case=========================

                currentCourse = context.Courses.Find(currentCourseID);
                if ((context.Modules.Any(c => c.CourseCategoryID == currentCourse.CourseCategoryID && c.CourseCategoryID == 6 && currentCourseID > 102)) ||
                    (context.Modules.Any(c => c.CourseCategoryID == currentCourse.CourseCategoryID && c.CourseCategoryID == 4 && currentCourseID == 9)))
                {
                    currentevisionNumber = CourseManager.GetCourseRevisionNumberAtRegistrationCompleted(context, currentCourseID, registrationNumber, entityID);
                    preevisionNumber = currentevisionNumber;
                }
                //======================================================================>>>>>>>>>>>>>>>>>>>>>>>
                int countExemModule = CheckExemptioDoneOrNot(currentCourseID,registrationNumber,currentevisionNumber,entityID);
                if ( CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == currentevisionNumber)
                {
                    if (currentCourseID == 3 && PassedConditionalTheoryModuleCount(currentCourseID, registrationNumber, currentevisionNumber, entityID) != 0 && countExemModule<1)
                       remainingTheoryModuleCount = PassedConditionalTheoryModuleCount(currentCourseID, registrationNumber, currentevisionNumber, entityID);
                       else
                    remainingTheoryModuleCount = GetRemainingAllTheoryModuleCount(currentCourseID, registrationNumber, currentevisionNumber, entityID);
                }
                //else if (rbtn_module_selection.SelectedValue == "pre" || CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == preevisionNumber) // commented by abhi singh dated on 16042024
                //{
                //    remainingTheoryModuleCount = GetRemainingAllTheoryModuleCount(currentCourseID, registrationNumber, preevisionNumber, entityID);
                //}
                else
                {
                    remainingTheoryModuleCount = GetRemainingAllTheoryModuleCount(currentCourseID, registrationNumber, currentevisionNumber, entityID);
                }

                //---------------------------
                //Retreiving already passed modules
                List<Int32> attemptedAndSelectedModules = new List<Int32>();
                List<Int32> passedAdnSelectedModules = new List<Int32>();
                //Exam exam = CourseManager.GetNextExam(context, currentCourseID, Convert.ToInt32(ddlCandidateType.SelectedValue));
                //passedAdnSelectedModules = CourseManager.GetPassedModulesOfCurrentRevision(context, currentCourseID, registrationNumber, currentevisionNumber, entityID).Where(c => c.ModuleTypeID != project).Select(c => c.ID).Distinct().ToList(); ---------commented on 250220
                //attemptedAndSelectedModules = CourseManager.GetListtOfAttempteddModulesOfCurrentRevision(context, exam.ID, currentCourseID, registrationNumber, currentevisionNumber, entityID, enmModuleType.Theory).Where(c => c.ModuleTypeID != project).Select(c => c.ID).Distinct().ToList(); ---------commented on 250220
                if ( CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == currentevisionNumber)
                {
                    passedAdnSelectedModules = CourseManager.GetPassedModulesOfCurrentRevision(context, currentCourseID, registrationNumber, currentevisionNumber, entityID).Where(c => c.ModuleTypeID != project).Select(c => c.ID).Distinct().ToList();
                    attemptedAndSelectedModules = CourseManager.GetListtOfAttempteddModulesOfCurrentRevision(context, exam.ID, currentCourseID, registrationNumber, currentevisionNumber, entityID, enmModuleType.Theory).Where(c => c.ModuleTypeID != project).Select(c => c.ID).Distinct().ToList();
                }
                //else if (rbtn_module_selection.SelectedValue == "pre" || CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == preevisionNumber)  // commented by abhi singh dated on 16042024
                //{
                //    passedAdnSelectedModules = CourseManager.GetPassedModulesOfCurrentRevision(context, currentCourseID, registrationNumber, preevisionNumber, entityID).Where(c => c.ModuleTypeID != project).Select(c => c.ID).Distinct().ToList();
                //    attemptedAndSelectedModules = CourseManager.GetListtOfAttempteddModulesOfCurrentRevision(context, exam.ID, currentCourseID, registrationNumber, preevisionNumber, entityID, enmModuleType.Theory).Where(c => c.ModuleTypeID != project).Select(c => c.ID).Distinct().ToList();
                //}
                else
                {
                    passedAdnSelectedModules = CourseManager.GetPassedModulesOfCurrentRevision(context, currentCourseID, registrationNumber, currentevisionNumber, entityID).Where(c => c.ModuleTypeID != project).Select(c => c.ID).Distinct().ToList();
                    attemptedAndSelectedModules = CourseManager.GetListtOfAttempteddModulesOfCurrentRevision(context, exam.ID, currentCourseID, registrationNumber, currentevisionNumber, entityID, enmModuleType.Theory).Where(c => c.ModuleTypeID != project).Select(c => c.ID).Distinct().ToList();
                }

                Int32 selectedModuleID = 0;
                Int32 selectedModuleCount = 0;
                int selectElectModelCount_25 = 1;
                int selectElectModelCount_54 = 1;
                int selectElectModelCount_34 = 1;
                //Int32 selectedElectiveCount = 0;
                int? selecctedModuleExamMode = 0;
                List<Int32> selectedModules = new List<Int32>();

                //Selected Module in this exam

                if (trModuleOptionExmpt.Visible==true)
                {
                    foreach (ListItem item in chklistModulesExmpt.Items)
                    {
                        try
                        {
                            if (item.Selected == true)
                            {
                                if (selectedModuleCount >= remainingTheoryModuleCount)
                                {
                                    item.Selected = false;
                                    throw new Exception("You can select only " + remainingTheoryModuleCount.ToString() + " remaining modules in this selection. If you want to select this module deselect any selected module first.");
                                }
                                selectedModuleID = Convert.ToInt32(item.Value);
                                Module selectedModule = context.Modules.Find(selectedModuleID);
                                selecctedModuleExamMode = selectedModule.ExamModeId;
                                if (selectedModule.enmSelectionType == enmSelectionType.Elective)
                                {
                                    electiveGroupID = selectedModule.ElectiveGroup.Value;
                                    Int32 electivegroupsubnumber = selectedModule.ModuleSubNUmber;
                                    Int32 electiveSelectionTypeId = Convert.ToInt32(selectedModule.NumberOfElectiveModulesAllowed);

                                    //Int32 oldModuleID = 0;

                                    //if ((currentCourseID == 1 || currentCourseID == 2) && (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 0 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL == 0))
                                    //{
                                    //    Int32 ParityF = context.Parities.Where(x => x.NewModuleID == electiveGroupID).FirstOrDefault().ID;

                                    //    Parity parityS = context.Parities.Find(ParityF);

                                    //    if (parityS.NewRevisionNumber.Equals(6))
                                    //    {
                                    //        oldModuleID = parityS.OldModuleID;
                                    //    }
                                    //    else
                                    //    {
                                    //        oldModuleID = electiveGroupID;
                                    //    }
                                    //}

                                    var pendingResultCount = (from r in context.CourseExamApplicationDetails
                                                              where r.Module.ElectiveGroup == electiveGroupID && (r.ResultGradeID.HasValue == false || r.ResultGradeID == 11) &&
                                                              r.ExamID != exam.ID && r.CandidateID == entityID && r.RegistrationNumber == registrationNumber
                                                              && r.CourseID == currentCourseID && r.Module.SelectionTypeID == elective && r.IsUnfairMeansCase == false
                                                              && (r.CourseExamApplication.PaymentStatusID == 2 || r.CourseExamApplication.PaymentStatusID == 4)
                                                              && r.CourseExamApplication.ID == r.CourseExamApplicationID
                                                              && r.CourseExamApplication.ApplicationStatusID == applicationstatus
                                                              select r);

                                    var pendingResultModuleSelected = (from r in context.CourseExamApplicationDetails
                                                                       where r.Module.ElectiveGroup == electiveGroupID && r.ResultGradeID.HasValue == false && r.ModuleID == selectedModuleID &&
                                                                       r.ExamID != exam.ID && r.CandidateID == entityID && r.RegistrationNumber == registrationNumber
                                                                       && r.CourseID == currentCourseID && r.Module.SelectionTypeID == elective && r.IsUnfairMeansCase == false
                                                                        && (r.CourseExamApplication.PaymentStatusID == 2 || r.CourseExamApplication.PaymentStatusID == 4)
                                                                       && r.CourseExamApplication.ID == r.CourseExamApplicationID
                                                                       && r.CourseExamApplication.ApplicationStatusID == applicationstatus
                                                                       select r);

                                    if (pendingResultModuleSelected.Count() > 0) //|| pendingResultCount.Count() > (electiveSelectionTypeId-1))
                                    {
                                        //item.Selected = false;
                                        //if ((pendingResultCount.FirstOrDefault().CourseExamApplication.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)) && (pendingResultCount.FirstOrDefault().CourseExamApplication.IsExported))
                                        //{
                                        item.Selected = false;
                                        throw new Exception("You can not select this module - " + selectedModule.ShortName + " as you have already attempted this module or module of this elective group in the last exam and result is pending.");
                                        //}

                                    }
                                    //<<<<<<<<<<<<<==================================================================================
                                    else
                                    {
                                        var pendingresult_new_module_id = (from p in context.Parities
                                                                           where p.CourseID == currentCourseID && p.NewModuleID == selectedModuleID
                                                                           && p.OldRevisionNumber == preevisionNumber && p.NewRevisionNumber == currentevisionNumber
                                                                           select p.OldModuleID).Distinct().FirstOrDefault();

                                        var pendingResult_old_module_id = (from r in context.CourseExamApplicationDetails
                                                                           where r.ResultGradeID.HasValue == false && r.Module.ElectiveGroup == electiveGroupID &&
                                                                           r.ExamID != exam.ID && r.CandidateID == entityID && r.RegistrationNumber == registrationNumber
                                                                           && r.CourseID == currentCourseID && r.IsUnfairMeansCase == false
                                                                           && (r.CourseExamApplication.PaymentStatusID == 2 || r.CourseExamApplication.PaymentStatusID == 4)
                                                                            && r.CourseExamApplication.ID == r.CourseExamApplicationID
                                                                            && r.CourseExamApplication.ApplicationStatusID == applicationstatus
                                                                           select r);

                                        foreach (var pending_old_mod in pendingResult_old_module_id.ToList())
                                        {
                                            try
                                            {
                                                if (pending_old_mod.ModuleID == pendingresult_new_module_id && (pending_old_mod.CourseExamApplication.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)) && (pending_old_mod.CourseExamApplication.IsExported))
                                                {
                                                    item.Selected = false;
                                                    throw new Exception("You can not select this module - " + selectedModule.ShortName + " as you have already attempted this module or module of this elective group in the last exam in revision-" + preevisionNumber.ToString() + " and result is pending.");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                loopExceptions.Add(ex);
                                            }
                                        }
                                    }
                                    //================================================================>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                    //--------commented on 28042020---------------
                                    //Int32 electiveModuleCount = (from m in context.Modules
                                    //                             where passedAdnSelectedModules.Contains(m.ID) && m.SelectionTypeID == elective && m.ElectiveGroup.Value == electiveGroupID
                                    //                             select m.ID).Distinct().Count();
                                    //if (electiveModuleCount >= selectedModule.NumberOfElectiveModulesAllowed)
                                    //{
                                    //    item.Selected = false;
                                    //    throw new Exception("You can select only " + selectedModule.NumberOfElectiveModulesAllowed.ToString() + " elective module in this elective group.");
                                    //}
                                    //else
                                    //{

                                    //    passedAdnSelectedModules.Add(selectedModuleID);
                                    //    selectedModules.Add(selectedModuleID);
                                    //}
                                    //---------------------------------------------

                                    //==============Start==============Added_28042020 and then 15May2020============For Special case of 'A' level Candidates who are filling thier choices from 3-4 to 5 revision and also for 'B' Level where to check one module from elective group is already passed
                                    Int32 theory = Convert.ToInt32(enmModuleType.Theory);
                                    Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
                                    Int32 electiveModuleCount = (from m in context.Modules
                                                                 where passedAdnSelectedModules.Contains(m.ID) && m.SelectionTypeID == elective && m.ElectiveGroup.Value == electiveGroupID
                                                                 select m.ID).Distinct().Count();
                                    electiveModuleCount = electiveModuleCount + pendingResultCount.Count();

                                    //string x = (from d in context.CourseExamApplicationDetails
                                    //            join m in context.Modules on d.ModuleID equals m.ID
                                    //            where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                    //            d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && (m.ID == 354 || m.ID == 355 || m.ID == 356)
                                    //             && (d.CourseExamApplication.PaymentStatusID == 2 || d.CourseExamApplication.PaymentStatusID == 4)
                                    //               && d.CourseExamApplication.ID == d.CourseExamApplicationID
                                    //               && d.CourseExamApplication.ApplicationStatusID == applicationstatus
                                    //            orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                    //            select m).ToString();

                                    Int32 moduleno_nine_ten_TheoryModulesPassed_inrevision4 = (from d in context.CourseExamApplicationDetails
                                                                                               join m in context.Modules on d.ModuleID equals m.ID
                                                                                               where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                                                                               d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && (m.ID == 354 || m.ID == 355 || m.ID == 356)
                                                                                                && (d.CourseExamApplication.PaymentStatusID == 2 || d.CourseExamApplication.PaymentStatusID == 4)
                                                                                                  && d.CourseExamApplication.ID == d.CourseExamApplicationID
                                                                                                  && d.CourseExamApplication.ApplicationStatusID == applicationstatus
                                                                                               orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                                                               select m).Count();

                                    Int32 moduleno_passed_in_electiveGroupId = (from d in context.CourseExamApplicationDetails
                                                                                join m in context.Modules on d.ModuleID equals m.ID
                                                                                where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                                                                d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && m.ElectiveGroup.Value == electiveGroupID
                                                                                  && (d.CourseExamApplication.PaymentStatusID == 2 || d.CourseExamApplication.PaymentStatusID == 4)
                                                                                  && d.CourseExamApplication.ID == d.CourseExamApplicationID
                                                                                  && d.CourseExamApplication.ApplicationStatusID == applicationstatus
                                                                                orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                                                select m).Count();

                                    //=============================================================================================================================


                                    if (electiveModuleCount != 0)
                                    {
                                        if (electiveModuleCount < selectedModule.NumberOfElectiveModulesAllowed)
                                        {
                                            if (currentCourseID != 3 && currentevisionNumber != 5)
                                            {
                                                if (passedAdnSelectedModules.Contains(391))
                                                {
                                                    if (!passedAdnSelectedModules.Contains(412))
                                                    {
                                                        if (item.Selected == true && item.Value == "412")
                                                        {
                                                            if (!passedAdnSelectedModules.Contains(selectedModuleID))
                                                            {
                                                                passedAdnSelectedModules.Add(selectedModuleID);
                                                                selectedModules.Add(selectedModuleID);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            item.Selected = false;
                                                            throw new Exception("Your current selection is Invalid. Kindly refer to the Syllabus for valid selection.");
                                                        }
                                                    }

                                                }
                                            }
                                         }
                                     }

                                  

                                    //==============================================================================================================================
                                    //==============================================================================================================================
                                    if (electiveModuleCount >= selectedModule.NumberOfElectiveModulesAllowed)
                                    {
                                        if (electiveModuleCount > moduleno_passed_in_electiveGroupId && moduleno_passed_in_electiveGroupId != 0)
                                        {
                                            Int32 difference = selectedModule.NumberOfElectiveModulesAllowed.Value - moduleno_passed_in_electiveGroupId;
                                            item.Selected = false;
                                            throw new Exception("You can select only " + difference + " elective module, because " + moduleno_passed_in_electiveGroupId + " Module from this elective group is already passed.");
                                        }
                                        else if (currentCourseID == 3 && currentevisionNumber == 5)
                                        {
                                            List<int> data1 = new List<int> { 408, 409, 410, 411, 412, 413, 414, 416 };

                                            //Total passed modules in B-R4 
                                            List<int> data2 = (from d in context.CourseExamApplicationDetails
                                                               join m in context.Modules on d.ModuleID equals m.ID
                                                               where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                                               d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge)
                                                               orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                               select d.ModuleID).ToList();

                                           
                                            List<int> data3 = data2.Intersect(data1).ToList();

                                            // calculate total theroy in B-R5
                                            List<int> data4 = (from s in context.Parities where s.NewRevisionNumber == 5 && s.CourseID == 3 select s.OldModuleID).ToList();

                                            //calculate total remaining module in B-R5 after Choose New Version(B-R4 to B-R5)
                                            int data5 = data4.Intersect(data3).Count();
                                            if (data5 <= electiveModuleCount)
                                            {
                                                item.Selected = false;
                                                throw new Exception("You can select only 1 elective module, because " + data5 + " Module from this elective group is already passed/Exempt.");
                                            }
                                        }
                                        else if (moduleno_nine_ten_TheoryModulesPassed_inrevision4 != 2)
                                        {
                                            if (!passedAdnSelectedModules.Contains(selectedModuleID))
                                            {
                                                passedAdnSelectedModules.Add(selectedModuleID);
                                                selectedModules.Add(selectedModuleID);
                                            }
                                        }
                                        else
                                        {
                                            item.Selected = false;
                                            throw new Exception("You can select only " + selectedModule.NumberOfElectiveModulesAllowed.ToString() + " elective module in this elective group.");
                                        }
                                    }
                                    else
                                    {
                                        if (!passedAdnSelectedModules.Contains(selectedModuleID))
                                        {
                                            passedAdnSelectedModules.Add(selectedModuleID);
                                            selectedModules.Add(selectedModuleID);
                                            electiveModuleCount = electiveModuleCount + pendingResultCount.Count();
                                        }
                                    }
                                    //===========================================================================================================================END
                                    
                                    //---added code date 23042023---------------------for Handle max selection elective model in partircular elective group
                                    //if (electiveSelectionTypeId < selectElectModelCount_25)
                                    //{
                                    //    item.Selected = false;
                                    //    throw new Exception("You can select only " + selectedModule.NumberOfElectiveModulesAllowed.ToString() + " elective module in this elective group.");
                                    //}
                                    if (electiveSelectionTypeId < selectElectModelCount_34)
                                    {
                                        item.Selected = false;
                                        throw new Exception("You can select only " + selectedModule.NumberOfElectiveModulesAllowed.ToString() + " elective module in this elective group.");
                                    }
                                    //if (electiveSelectionTypeId < selectElectModelCount_54)
                                    //{
                                    //    item.Selected = false;
                                    //    throw new Exception("You can select only " + selectedModule.NumberOfElectiveModulesAllowed.ToString() + " elective module in this elective group.");
                                    //}
                                    //---------------------------------
                                    //if (electiveGroupID == 25)
                                    //    selectElectModelCount_25++;
                                    //if (electiveGroupID == 54)
                                    //    selectElectModelCount_54++;
                                    if (electiveGroupID == 34)
                                        selectElectModelCount_34++;
                                }
                                else
                                {
                                    //var pendingResultCount = (from r in context.CourseExamApplicationDetails
                                    //                          where r.ModuleID == selectedModuleID && r.ResultGradeID.HasValue == false &&
                                    //                          r.ExamID != exam.ID && r.CandidateID == entityID && r.RegistrationNumber == registrationNumber
                                    //                          && r.CourseID == currentCourseID && r.Module.SelectionTypeID == compulsory && r.IsUnfairMeansCase == false
                                    //                          select r);
                                    //if (pendingResultCount.Count() > 0)
                                    //{
                                    //    if ((pendingResultCount.FirstOrDefault().CourseExamApplication.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)) && (pendingResultCount.FirstOrDefault().CourseExamApplication.IsExported))
                                    //    {
                                    //        item.Selected = false;
                                    //        throw new Exception("You can not select this module - " + selectedModule.ShortName + " as you have already attempted this module in the last exam and result is pending.");
                                    //    }
                                    //}

                                    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<============================================================================================   

                                    Int32 oldModuleID = 0;

                                    if ((currentCourseID == 1 || currentCourseID == 2 ) && (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 0 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL == 0))
                                    {
                                        Int32 ParityF = context.Parities.Where(x => x.NewModuleID == selectedModuleID).FirstOrDefault().ID;

                                        Parity parityS = context.Parities.Find(ParityF);

                                        if (parityS.NewRevisionNumber.Equals(6))
                                        {
                                            oldModuleID = parityS.OldModuleID;
                                        }
                                        else
                                        {
                                            oldModuleID = selectedModuleID;
                                        }
                                    }
                                    var pendingResultCount = (from r in context.CourseExamApplicationDetails
                                                              where (r.ModuleID == selectedModuleID || r.ModuleID == oldModuleID) && (r.ResultGradeID.HasValue == false || r.ResultGradeID == 11) &&
                                                              r.ExamID != exam.ID && r.CandidateID == entityID && r.RegistrationNumber == registrationNumber
                                                              && r.CourseID == currentCourseID && r.Module.SelectionTypeID == compulsory && r.IsUnfairMeansCase == false
                                                              && (r.CourseExamApplication.PaymentStatusID == 2 || r.CourseExamApplication.PaymentStatusID == 4)
                                                              && r.CourseExamApplication.ID == r.CourseExamApplicationID
                                                              && r.CourseExamApplication.ApplicationStatusID == applicationstatus
                                                              select r);

                                    if (pendingResultCount.Count() > 0)
                                    {
                                        //if ((pendingResultCount.FirstOrDefault().CourseExamApplication.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)) && (pendingResultCount.FirstOrDefault().CourseExamApplication.IsExported))
                                        //{

                                        item.Selected = false;
                                        throw new Exception("You can not select this module - " + selectedModule.ShortName + " as you have already attempted this module in the last exam and result is pending.");
                                        //}
                                    }
                                    else
                                    {
                                        var pendingresult_new_module_id = (from p in context.Parities
                                                                           where p.CourseID == currentCourseID && p.NewModuleID == selectedModuleID
                                                                           && p.OldRevisionNumber == preevisionNumber && p.NewRevisionNumber == currentevisionNumber
                                                                           select p.OldModuleID).Distinct().FirstOrDefault();
                                        var pendingResult_old_module_id = (from r in context.CourseExamApplicationDetails
                                                                           where r.ResultGradeID.HasValue == false && r.ExamID != exam.ID
                                                                           && r.CandidateID == entityID && r.RegistrationNumber == registrationNumber
                                                                           && r.CourseID == currentCourseID && r.IsUnfairMeansCase == false
                                                                           && (r.CourseExamApplication.PaymentStatusID == 2 || r.CourseExamApplication.PaymentStatusID == 4)
                                                                           && r.CourseExamApplication.ID == r.CourseExamApplicationID
                                                                           && r.CourseExamApplication.ApplicationStatusID == applicationstatus
                                                                           select r);

                                        foreach (var pending_old_mod in pendingResult_old_module_id.ToList())
                                        {
                                            try
                                            {
                                                if (pending_old_mod.ModuleID == pendingresult_new_module_id && (pending_old_mod.CourseExamApplication.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)) && (pending_old_mod.CourseExamApplication.IsExported))
                                                {
                                                    item.Selected = false;
                                                    throw new Exception("You can not select this module - " + selectedModule.ShortName + " as you have already attempted this module in the last exam in revision-" + preevisionNumber.ToString() + " and result is pending.");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                loopExceptions.Add(ex);
                                            }
                                        }
                                    }
                                    //=====================================================================================================>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                    if (!passedAdnSelectedModules.Contains(selectedModuleID))
                                    {
                                        passedAdnSelectedModules.Add(selectedModuleID);
                                        selectedModules.Add(selectedModuleID);
                                    }
                                }
                                selectedModuleCount++;
                                selectedModuleCount1++;
                                lblCoutMessage.Text = "Theory Papers  &nbsp;&nbsp;&nbsp;&nbsp; <b style='color:red'>You have selected " + selectedModuleCount.ToString() + " modules out of " + remainingTheoryModuleCount.ToString() + " modules (remaining to pass) in the list below.</b>";
                            }
                        }
                        catch (Exception ex)
                        {
                            loopExceptions.Add(ex);
                        }
                    }
                }
                else
                {
                    foreach (ListItem item in chklistModules.Items)
                    {
                        try
                        {
                            if (item.Selected == true)
                            {
                                
                                //if (selectedModuleCount >= remainingTheoryModuleCount) // changes by abhi singh on dated 24042024 for give a selection module of multiple specilization group

                                if (selectedModuleCount >= remainingTheoryModuleCount && remainingTheoryModuleCountspeclGroupAlevel==0)
                                {
                                    item.Selected = false;
                                    throw new Exception("You can select only " + remainingTheoryModuleCount.ToString() + " remaining modules in this exam. If you want to select this module deselect any selected module first.");
                                }

                                selectedModuleID = Convert.ToInt32(item.Value);
                                Module selectedModule = context.Modules.Find(selectedModuleID);
                                selecctedModuleExamMode = selectedModule.ExamModeId;
                                if (selectedModule.enmSelectionType == enmSelectionType.Elective)
                                {
                                    electiveGroupID = selectedModule.ElectiveGroup.Value;
                                    Int32 electivegroupsubnumber = selectedModule.ModuleSubNUmber;
                                    Int32 electiveSelectionTypeId = Convert.ToInt32(selectedModule.NumberOfElectiveModulesAllowed);

                                    
                                    //Int32 oldModuleID = 0;

                                    //if ((currentCourseID == 1 || currentCourseID == 2) && (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 0 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL == 0))
                                    //{
                                    //    Int32 ParityF = context.Parities.Where(x => x.NewModuleID == electiveGroupID).FirstOrDefault().ID;

                                    //    Parity parityS = context.Parities.Find(ParityF);

                                    //    if (parityS.NewRevisionNumber.Equals(6))
                                    //    {
                                    //        oldModuleID = parityS.OldModuleID;
                                    //    }
                                    //    else
                                    //    {
                                    //        oldModuleID = electiveGroupID;
                                    //    }
                                    //}

                                    var pendingResultCount = (from r in context.CourseExamApplicationDetails
                                                              where r.Module.ElectiveGroup == electiveGroupID && (r.ResultGradeID.HasValue == false || r.ResultGradeID == 11) &&
                                                              r.ExamID != exam.ID && r.CandidateID == entityID && r.RegistrationNumber == registrationNumber
                                                              && r.CourseID == currentCourseID && r.Module.SelectionTypeID == elective && r.IsUnfairMeansCase == false
                                                              && (r.CourseExamApplication.PaymentStatusID == 2 || r.CourseExamApplication.PaymentStatusID == 4)
                                                              && r.CourseExamApplication.ID == r.CourseExamApplicationID
                                                              && r.CourseExamApplication.ApplicationStatusID == applicationstatus
                                                              select r);

                                    var pendingResultModuleSelected = (from r in context.CourseExamApplicationDetails
                                                                       where r.Module.ElectiveGroup == electiveGroupID && r.ResultGradeID.HasValue == false && r.ModuleID == selectedModuleID &&
                                                                       r.ExamID != exam.ID && r.CandidateID == entityID && r.RegistrationNumber == registrationNumber
                                                                       && r.CourseID == currentCourseID && r.Module.SelectionTypeID == elective && r.IsUnfairMeansCase == false
                                                                        && (r.CourseExamApplication.PaymentStatusID == 2 || r.CourseExamApplication.PaymentStatusID == 4)
                                                                       && r.CourseExamApplication.ID == r.CourseExamApplicationID
                                                                       && r.CourseExamApplication.ApplicationStatusID == applicationstatus
                                                                       select r);

                                    if (pendingResultModuleSelected.Count() > 0) //|| pendingResultCount.Count() > (electiveSelectionTypeId-1))
                                    {
                                        //item.Selected = false;
                                        //if ((pendingResultCount.FirstOrDefault().CourseExamApplication.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)) && (pendingResultCount.FirstOrDefault().CourseExamApplication.IsExported))
                                        //{
                                        item.Selected = false;
                                        throw new Exception("You can not select this module - " + selectedModule.ShortName + " as you have already attempted this module or module of this elective group in the last exam and result is pending.");
                                        //}

                                    }
                                    //<<<<<<<<<<<<<==================================================================================
                                    else
                                    {
                                        var pendingresult_new_module_id = (from p in context.Parities
                                                                           where p.CourseID == currentCourseID && p.NewModuleID == selectedModuleID
                                                                           && p.OldRevisionNumber == preevisionNumber && p.NewRevisionNumber == currentevisionNumber
                                                                           select p.OldModuleID).Distinct().FirstOrDefault();

                                        var pendingResult_old_module_id = (from r in context.CourseExamApplicationDetails
                                                                           where r.ResultGradeID.HasValue == false && r.Module.ElectiveGroup == electiveGroupID &&
                                                                           r.ExamID != exam.ID && r.CandidateID == entityID && r.RegistrationNumber == registrationNumber
                                                                           && r.CourseID == currentCourseID && r.IsUnfairMeansCase == false
                                                                           && (r.CourseExamApplication.PaymentStatusID == 2 || r.CourseExamApplication.PaymentStatusID == 4)
                                                                            && r.CourseExamApplication.ID == r.CourseExamApplicationID
                                                                            && r.CourseExamApplication.ApplicationStatusID == applicationstatus
                                                                           select r);

                                        foreach (var pending_old_mod in pendingResult_old_module_id.ToList())
                                        {
                                            try
                                            {
                                                if (pending_old_mod.ModuleID == pendingresult_new_module_id && (pending_old_mod.CourseExamApplication.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)) && (pending_old_mod.CourseExamApplication.IsExported))
                                                {
                                                    item.Selected = false;
                                                    throw new Exception("You can not select this module - " + selectedModule.ShortName + " as you have already attempted this module or module of this elective group in the last exam in revision-" + preevisionNumber.ToString() + " and result is pending.");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                loopExceptions.Add(ex);
                                            }
                                        }
                                    }
                                    //================================================================>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                    //--------commented on 28042020---------------
                                    //Int32 electiveModuleCount = (from m in context.Modules
                                    //                             where passedAdnSelectedModules.Contains(m.ID) && m.SelectionTypeID == elective && m.ElectiveGroup.Value == electiveGroupID
                                    //                             select m.ID).Distinct().Count();
                                    //if (electiveModuleCount >= selectedModule.NumberOfElectiveModulesAllowed)
                                    //{
                                    //    item.Selected = false;
                                    //    throw new Exception("You can select only " + selectedModule.NumberOfElectiveModulesAllowed.ToString() + " elective module in this elective group.");
                                    //}
                                    //else
                                    //{

                                    //    passedAdnSelectedModules.Add(selectedModuleID);
                                    //    selectedModules.Add(selectedModuleID);
                                    //}
                                    //---------------------------------------------

                                    //==============Start==============Added_28042020 and then 15May2020============For Special case of 'A' level Candidates who are filling thier choices from 3-4 to 5 revision and also for 'B' Level where to check one module from elective group is already passed
                                    Int32 theory = Convert.ToInt32(enmModuleType.Theory);
                                    Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
                                    Int32 electiveModuleCount = (from m in context.Modules
                                                                 where passedAdnSelectedModules.Contains(m.ID) && m.SelectionTypeID == elective && m.ElectiveGroup.Value == electiveGroupID
                                                                 select m.ID).Distinct().Count();
                                    electiveModuleCount = electiveModuleCount + pendingResultCount.Count();

                                    //string x = (from d in context.CourseExamApplicationDetails
                                    //            join m in context.Modules on d.ModuleID equals m.ID
                                    //            where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                    //            d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && (m.ID == 354 || m.ID == 355 || m.ID == 356)
                                    //             && (d.CourseExamApplication.PaymentStatusID == 2 || d.CourseExamApplication.PaymentStatusID == 4)
                                    //               && d.CourseExamApplication.ID == d.CourseExamApplicationID
                                    //               && d.CourseExamApplication.ApplicationStatusID == applicationstatus
                                    //            orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                    //            select m).ToString();

                                    Int32 moduleno_nine_ten_TheoryModulesPassed_inrevision4 = (from d in context.CourseExamApplicationDetails
                                                                                               join m in context.Modules on d.ModuleID equals m.ID
                                                                                               where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                                                                               d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && (m.ID == 354 || m.ID == 355 || m.ID == 356)
                                                                                                && (d.CourseExamApplication.PaymentStatusID == 2 || d.CourseExamApplication.PaymentStatusID == 4)
                                                                                                  && d.CourseExamApplication.ID == d.CourseExamApplicationID
                                                                                                  && d.CourseExamApplication.ApplicationStatusID == applicationstatus
                                                                                               orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                                                               select m).Count();

                                    Int32 moduleno_passed_in_electiveGroupId = (from d in context.CourseExamApplicationDetails
                                                                                join m in context.Modules on d.ModuleID equals m.ID
                                                                                where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                                                                d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && m.ElectiveGroup.Value == electiveGroupID
                                                                                  //&& (d.CourseExamApplication.PaymentStatusID == 2 || d.CourseExamApplication.PaymentStatusID == 4)
                                                                                  && d.CourseExamApplication.ID == d.CourseExamApplicationID
                                                                                  && d.CourseExamApplication.ApplicationStatusID == applicationstatus
                                                                                orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                                                select m).Count();

                                    //=============================================================================================================================


                                    //if (electiveModuleCount != 0)
                                    //{
                                    //    if (electiveModuleCount < selectedModule.NumberOfElectiveModulesAllowed)
                                    //    {
                                    //        if (currentCourseID == 3 && currentevisionNumber == 5)
                                    //        { 

                                    //        }
                                    //        else
                                    //        {
                                    //        if (passedAdnSelectedModules.Contains(391))
                                    //        {
                                    //            if (!passedAdnSelectedModules.Contains(412))
                                    //            {
                                    //                if (item.Selected == true && item.Value == "412")
                                    //                {
                                    //                    if (!passedAdnSelectedModules.Contains(selectedModuleID))
                                    //                    {
                                    //                        passedAdnSelectedModules.Add(selectedModuleID);
                                    //                        selectedModules.Add(selectedModuleID);
                                    //                    }
                                    //                }
                                    //                else
                                    //                {
                                    //                    item.Selected = false;
                                    //                    throw new Exception("Your current selection is Invalid. Kindly refer to the Syllabus for valid selection.");
                                    //                }
                                    //            }

                                    //        }
                                    //      }
                                    //   }
                                    //}



                                    if (electiveModuleCount != 0)
                                    {
                                        if (electiveModuleCount < selectedModule.NumberOfElectiveModulesAllowed)
                                        {
                                           if (currentCourseID != 3 && currentevisionNumber != 5)
                                            {
                                                if (passedAdnSelectedModules.Contains(391))
                                                {
                                                    if (!passedAdnSelectedModules.Contains(412))
                                                    {
                                                        if (item.Selected == true && item.Value == "412")
                                                        {
                                                            if (!passedAdnSelectedModules.Contains(selectedModuleID))
                                                            {
                                                                passedAdnSelectedModules.Add(selectedModuleID);
                                                                selectedModules.Add(selectedModuleID);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            item.Selected = false;
                                                            throw new Exception("Your current selection is Invalid. Kindly refer to the Syllabus for valid selection.");
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }

                                    //==============================================================================================================================
                                    //==============================================================================================================================
                                    if (electiveModuleCount >= selectedModule.NumberOfElectiveModulesAllowed)
                                    {
                                        if (electiveModuleCount > moduleno_passed_in_electiveGroupId && moduleno_passed_in_electiveGroupId != 0)
                                        {
                                            Int32 difference = selectedModule.NumberOfElectiveModulesAllowed.Value - moduleno_passed_in_electiveGroupId;
                                            item.Selected = false;
                                            throw new Exception("You can select only " + difference + " elective module, because " + moduleno_passed_in_electiveGroupId + " Module from this elective group is already passed.");
                                        }
                                        else if (currentCourseID == 3 && currentevisionNumber == 5 && countExemModule>0)
                                        {
                                            List<int> data1 = new List<int> { 408, 409, 410, 411, 412, 413, 414, 416 };

                                            //Total passed modules in B-R4 
                                            List<int> data2 = (from d in context.CourseExamApplicationDetails
                                                               join m in context.Modules on d.ModuleID equals m.ID
                                                               where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                                               d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge)
                                                               orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                               select d.ModuleID).ToList();


                                            List<int> data3 = data2.Intersect(data1).ToList();

                                            // calculate total theroy in B-R5
                                            List<int> data4 = (from s in context.Parities where s.NewRevisionNumber == 5 && s.CourseID == 3 select s.OldModuleID).ToList();

                                            //calculate total remaining module in B-R5 after Choose New Version(B-R4 to B-R5)
                                            int data5 = data4.Intersect(data3).Count();
                                            if (data5 <= electiveModuleCount)
                                            {
                                                item.Selected = false;
                                                throw new Exception("You can select only 1 elective module, because " + data5 + " Module from this elective group is already passed/Exempt.");
                                            }
                                        }
                                        
                                        //else if (moduleno_nine_ten_TheoryModulesPassed_inrevision4 != 2)
                                        //{
                                        //    if (!passedAdnSelectedModules.Contains(selectedModuleID))
                                        //    {
                                        //        passedAdnSelectedModules.Add(selectedModuleID);
                                        //        selectedModules.Add(selectedModuleID);
                                        //    }
                                        //}
                                        else if(currentCourseID!=2)
                                        {
                                            item.Selected = false;
                                            throw new Exception("You can select only " + selectedModule.NumberOfElectiveModulesAllowed.ToString() + " elective module in this elective group.");
                                        }
                                    }
                                    else
                                    {
                                        if (!passedAdnSelectedModules.Contains(selectedModuleID))
                                        {
                                            passedAdnSelectedModules.Add(selectedModuleID);
                                            selectedModules.Add(selectedModuleID);
                                            electiveModuleCount = electiveModuleCount + pendingResultCount.Count();
                                        }
                                    }
                                    //===========================================================================================================================END


                               
                                    //---added code date 23042023---------------------for Handle max selection elective model in partircular elective group
                                    //if (electiveSelectionTypeId < selectElectModelCount)
                                    //{
                                    //    item.Selected = false;
                                    //    throw new Exception("You can select only " + selectedModule.NumberOfElectiveModulesAllowed.ToString() + " elective module in this elective group.");
                                    //}

                                    if (electiveSelectionTypeId < selectElectModelCount_25)
                                    {
                                        item.Selected = false;
                                        throw new Exception("You can select only " + selectedModule.NumberOfElectiveModulesAllowed.ToString() + " elective module in this elective group.");
                                    }
                                    if (electiveSelectionTypeId < selectElectModelCount_34)
                                    {
                                        item.Selected = false;
                                        throw new Exception("You can select only " + selectedModule.NumberOfElectiveModulesAllowed.ToString() + " elective module in this elective group.");
                                    }
                                    if (electiveSelectionTypeId < selectElectModelCount_54)
                                    {
                                        item.Selected = false;
                                        throw new Exception("You can select only " + selectedModule.NumberOfElectiveModulesAllowed.ToString() + " elective module in this elective group.");
                                    }
                                    //---------------------------------
                                    if (electiveGroupID == 25)
                                        selectElectModelCount_25++;
                                    if (electiveGroupID == 54)
                                        selectElectModelCount_54++;
                                    if (electiveGroupID == 34)
                                        selectElectModelCount_34++;
                                    //---------------------------------
                                   
                                    
                                }
                                else
                                {
                                    //var pendingResultCount = (from r in context.CourseExamApplicationDetails
                                    //                          where r.ModuleID == selectedModuleID && r.ResultGradeID.HasValue == false &&
                                    //                          r.ExamID != exam.ID && r.CandidateID == entityID && r.RegistrationNumber == registrationNumber
                                    //                          && r.CourseID == currentCourseID && r.Module.SelectionTypeID == compulsory && r.IsUnfairMeansCase == false
                                    //                          select r);
                                    //if (pendingResultCount.Count() > 0)
                                    //{
                                    //    if ((pendingResultCount.FirstOrDefault().CourseExamApplication.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)) && (pendingResultCount.FirstOrDefault().CourseExamApplication.IsExported))
                                    //    {
                                    //        item.Selected = false;
                                    //        throw new Exception("You can not select this module - " + selectedModule.ShortName + " as you have already attempted this module in the last exam and result is pending.");
                                    //    }
                                    //}

                                    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<============================================================================================   

                                    Int32 oldModuleID = 0;

                                    if ((currentCourseID == 1 || currentCourseID == 2 ) && (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 0 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL == 0 ))
                                    {
                                        Int32 ParityF = context.Parities.Where(x => x.NewModuleID == selectedModuleID).FirstOrDefault().ID;

                                        Parity parityS = context.Parities.Find(ParityF);

                                        if (parityS.NewRevisionNumber.Equals(6))
                                        {
                                            oldModuleID = parityS.OldModuleID;
                                        }
                                        else
                                        {
                                            oldModuleID = selectedModuleID;
                                        }
                                    }
                                    var pendingResultCount = (from r in context.CourseExamApplicationDetails
                                                              where (r.ModuleID == selectedModuleID || r.ModuleID == oldModuleID) && (r.ResultGradeID.HasValue == false || r.ResultGradeID == 11) &&
                                                              r.ExamID != exam.ID && r.CandidateID == entityID && r.RegistrationNumber == registrationNumber
                                                              && r.CourseID == currentCourseID && r.Module.SelectionTypeID == compulsory && r.IsUnfairMeansCase == false
                                                              && (r.CourseExamApplication.PaymentStatusID == 2 || r.CourseExamApplication.PaymentStatusID == 4)
                                                              && r.CourseExamApplication.ID == r.CourseExamApplicationID
                                                              && r.CourseExamApplication.ApplicationStatusID == applicationstatus
                                                              select r);

                                    if (pendingResultCount.Count() > 0)
                                    {
                                        //if ((pendingResultCount.FirstOrDefault().CourseExamApplication.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)) && (pendingResultCount.FirstOrDefault().CourseExamApplication.IsExported))
                                        //{

                                        item.Selected = false;
                                        throw new Exception("You can not select this module - " + selectedModule.ShortName + " as you have already attempted this module in the last exam and result is pending.");
                                        //}
                                    }
                                    else
                                    {
                                        var pendingresult_new_module_id = (from p in context.Parities
                                                                           where p.CourseID == currentCourseID && p.NewModuleID == selectedModuleID
                                                                           && p.OldRevisionNumber == preevisionNumber && p.NewRevisionNumber == currentevisionNumber
                                                                           select p.OldModuleID).Distinct().FirstOrDefault();
                                        var pendingResult_old_module_id = (from r in context.CourseExamApplicationDetails
                                                                           where r.ResultGradeID.HasValue == false && r.ExamID != exam.ID
                                                                           && r.CandidateID == entityID && r.RegistrationNumber == registrationNumber
                                                                           && r.CourseID == currentCourseID && r.IsUnfairMeansCase == false
                                                                           && (r.CourseExamApplication.PaymentStatusID == 2 || r.CourseExamApplication.PaymentStatusID == 4)
                                                                           && r.CourseExamApplication.ID == r.CourseExamApplicationID
                                                                           && r.CourseExamApplication.ApplicationStatusID == applicationstatus
                                                                           select r);

                                        foreach (var pending_old_mod in pendingResult_old_module_id.ToList())
                                        {
                                            try
                                            {
                                                if (pending_old_mod.ModuleID == pendingresult_new_module_id && (pending_old_mod.CourseExamApplication.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)) && (pending_old_mod.CourseExamApplication.IsExported))
                                                {
                                                    item.Selected = false;
                                                    throw new Exception("You can not select this module - " + selectedModule.ShortName + " as you have already attempted this module in the last exam in revision-" + preevisionNumber.ToString() + " and result is pending.");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                loopExceptions.Add(ex);
                                            }
                                        }
                                    }
                                    //=====================================================================================================>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                    if (!passedAdnSelectedModules.Contains(selectedModuleID))
                                    {
                                        passedAdnSelectedModules.Add(selectedModuleID);
                                        selectedModules.Add(selectedModuleID);
                                    }
                                }
                                selectedModuleCount++;
                                //--selectedModuleCount1++;
                                lblCoutMessage.Text = "Theory Papers  &nbsp;&nbsp;&nbsp;&nbsp; <b style='color:red'> You have selected " + selectedModuleCount.ToString() + " modules out of " + remainingTheoryModuleCount.ToString() + " modules (remaining to pass) in the list below.</b>";
                                if ((selectedModule.ExamModeId == 2 || selectedModule.ExamModeId == 3) && currentCourseID == 2)
                                    ALevel_OffLineExamCenterOption_Theory = 2;
                                else
                                    ALevel_OffLineExamCenterOption_Theory = 1;
                            }
                        }
                        catch (Exception ex)
                        {
                            loopExceptions.Add(ex);
                        }
                    }
                }
               

                if (selectedModuleCount == 0)
                    lblCoutMessage.Text = "Theory Papers  &nbsp;&nbsp;&nbsp;&nbsp; <b style='color:red'>You can select upto  " + remainingTheoryModuleCount.ToString() + " modules (remaining to pass) in the list below.</b>";
                else
                {
                    //Time table validation
                  
                    var lstTimeTable = (from t in context.ExamTimeTables
                                        where t.ExamID == exam.ID && selectedModules.Contains(t.ModuleID) && t.Module.ExamModeId!=1
                                        group t by new { t.ExamFomDate, t.ExamSession.Name } into g
                                        select new { Group = g.Key, cnt = g.Count() }).ToList();
                    foreach (var timetable in lstTimeTable)
                    {
                        try
                        {
                            if (timetable.cnt > 1 && selecctedModuleExamMode!=1)
                            {
                                btnSave.Visible = false;
                                throw new Exception("You can not select more than 1 module on same date and exam session(" + timetable.Group.ExamFomDate.ToString("dd-MMM-yyyy") + " " + timetable.Group.Name + "). Please refer time table for details.");
                            }
                        }
                        catch (Exception ex)
                        {
                            loopExceptions.Add(ex);
                        }
                    }
                }
                attemptedAndSelectedModules.AddRange(selectedModules);
                if (!(currentCourseID == 4 && firstRevisionNumber < currentevisionNumber && currentevisionNumber == 3) || currentCourseID != 4)
                {
                    if (remainingTheoryModuleCount > 0)
                    {
                        if (selectedModuleCount >= remainingTheoryModuleCount)
                        {
                            UpdateFeeDetails();
                            if (loopExceptions.Count > 0)
                            {
                                foreach (var exe in loopExceptions)
                                {
                                    //ShowAlert(exe.Message);
                                    throw exe;
                                }
                            }
                            return;
                        }
                        List<Int32> attemptedCompulsoryModules = (from p in context.Modules
                                                                  where attemptedAndSelectedModules.Contains(p.ID) && p.SelectionTypeID == compulsory
                                                                  select p.ID).Distinct().ToList();
                        List<Int32> attemptedElectiveModules = attemptedAndSelectedModules.FindAll(c => !attemptedCompulsoryModules.Contains(c));

                        //<<<<<<<<<<<<<<<<<<==========================commented on 07-10-2020====================================
                        ////Int32 traverceFlagCompulsory = 0;
                        ////Int32 traverceFlagElective = 0;
                        ////foreach (ListItem item in chklistPracticalsExmpt.Items)
                        ////{
                        ////    try
                        ////    {
                        ////        if (item.Selected)
                        ////        {
                        ////            Boolean isEligible = true;
                        ////            Int32 practicalModulesID = Convert.ToInt32(item.Value);
                        ////            var theoryCompulsoryModules = (from p in context.ModulePracticalEligibilities
                        ////                                           where p.PracticalModuleID == practicalModulesID
                        ////                                           select p.TheoryModule).Distinct().ToList();
                        ////            if (theoryCompulsoryModules.Where(c => c.enmSelectionType == enmSelectionType.Elective).Count() == 0)
                        ////                traverceFlagElective = 1;

                        ////            //for MAT-O Level since in this there is no practical eligibility
                        ////            if (theoryCompulsoryModules.Where(c => c.enmSelectionType == enmSelectionType.Compulsory).Count() == 0)
                        ////                traverceFlagCompulsory = 1;

                        ////            var theoryElectiveModulesID = (from p in context.ModulePracticalEligibilities
                        ////                                           where p.PracticalModuleID == practicalModulesID && p.SelectionTypeID != compulsory
                        ////                                           select p.ElectiveGroup).Distinct().ToList();

                        ////            var attemptedelectivegroup = (from m in context.Modules
                        ////                                          where attemptedElectiveModules.Contains(m.ID) && m.SelectionTypeID != compulsory
                        ////                                          select m.ElectiveGroup).Distinct().ToList();

                        ////            foreach (Module thModule in theoryCompulsoryModules)
                        ////            {
                        ////                if (thModule.enmSelectionType == enmSelectionType.Compulsory)
                        ////                {
                        ////                    if (!attemptedCompulsoryModules.Contains(thModule.ID))
                        ////                    {
                        ////                        var listOfPassedModules = (from d in context.CourseExamApplicationDetails
                        ////                                                   join m in context.Modules on d.ModuleID equals m.ID
                        ////                                                   where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                        ////                                                   d.Grade.IsPassed == true
                        ////                                                   orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                        ////                                                   select m.ID).ToList();

                        ////                        string moduleCode = context.Modules.Find(thModule.ID).ShortName;
                        ////                        moduleCode = moduleCode.Substring(0, moduleCode.IndexOf("-"));
                        ////                        var modulesAtt = context.Modules.Where(a => listOfPassedModules.Contains(a.ID));
                        ////                        modulesAtt = modulesAtt.Where(a => a.ShortName.Contains(moduleCode));
                        ////                        var clearedModCount = modulesAtt.Count();
                        ////                        if (clearedModCount <= 0)
                        ////                        {
                        ////                            isEligible = false;
                        ////                            break;
                        ////                        }
                        ////                    }
                        ////                    traverceFlagCompulsory = 1;
                        ////                }
                        ////            }
                        ////            foreach (Int32 egId in theoryElectiveModulesID)
                        ////            {
                        ////                //if (attemptedElectiveModules.Contains(thModule.ID))
                        ////                //{
                        ////                //    isEligible = true;
                        ////                //    traverceFlagElective = 1;
                        ////                //}
                        ////                if (!attemptedelectivegroup.Contains(egId))
                        ////                {
                        ////                    var listOfPassedModules = (from d in context.CourseExamApplicationDetails
                        ////                                               join m in context.Modules on d.ModuleID equals m.ID
                        ////                                               where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                        ////                                               d.Grade.IsPassed == true && m.ElectiveGroup.HasValue && m.ElectiveGroup.Value == egId
                        ////                                               orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                        ////                                               select m.ID).ToList();

                        ////                    var moduleCode = from d in context.Modules
                        ////                                     where d.ElectiveGroup == egId
                        ////                                     select new
                        ////                                     {
                        ////                                         ShortName = d.ShortName,
                        ////                                         NumberOfPapersToPass = d.NumberOfElectiveModulesAllowed
                        ////                                     };
                        ////                    var modulesAtt = context.Modules.Where(a => listOfPassedModules.Contains(a.ID));
                        ////                    int match = 0;
                        ////                    int required = 0;
                        ////                    foreach (var aa in moduleCode)
                        ////                    {
                        ////                        var code = aa.ShortName.IndexOf("-") > 0 ? aa.ShortName.Substring(0, aa.ShortName.IndexOf("-")) : aa.ShortName;
                        ////                        modulesAtt = modulesAtt.Where(a => a.ShortName.Contains(code));
                        ////                        if (modulesAtt.Count() > 0)
                        ////                            match++;
                        ////                        required = aa.NumberOfPapersToPass.Value;
                        ////                    }
                        ////                    if (match < required)
                        ////                    {
                        ////                        isEligible = false;
                        ////                        break;
                        ////                    }
                        ////                }
                        ////                traverceFlagElective = 1;
                        ////            }
                        ////            if (traverceFlagElective == 1 && traverceFlagCompulsory == 1)
                        ////            {
                        ////                item.Selected = isEligible;
                        ////            }
                        ////            else
                        ////            {
                        ////                item.Selected = false;
                        ////                throw new Exception("You are not eligible to select this practical module. See practical eligibility criteria.");
                        ////            }
                        ////        }
                        ////    }
                        ////    catch (Exception ex)
                        ////    {
                        ////        loopExceptions.Add(ex);
                        ////    }
                        ////}
                        ////==========================commented on 07-10-2020====================================>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    }
                }

                UpdateFeeDetails();
                btnSave.Visible = true;
            };
            //=========throw exceptions from foreach loop============
            if (loopExceptions.Count > 0)
            {
                foreach (var exe in loopExceptions)
                {
                    //ShowAlert(exe.Message);
                    throw exe;
                }
            }
        }
        catch (Exception ex)
        {
            Int32 selectedModuleCount = 0;
            foreach (ListItem item in chklistModules.Items)
            {
                if (item.Selected == true)
                {
                    selectedModuleCount++;
                }
            }
            lblCoutMessage.Text = "Theory Papers  &nbsp;&nbsp;&nbsp;&nbsp; <b style='color:red'>You have selected " + selectedModuleCount.ToString() + " modules out of " + remainingTheoryModuleCount.ToString() + " modules (remaining to pass) in the list below.</b>";
            UpdateFeeDetails();
            throw ex;
        }
        if (loopExceptions.Count > 0)
        {
            foreach (var exe in loopExceptions)
            {
                //ShowAlert(exe.Message);
                throw exe;
            }
        }
    }

    protected void ddlStateFirst_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlStateSecond.SelectedValue = "0";
            ddlStateSecond_SelectedIndexChanged(ddlStateSecond, EventArgs.Empty);
            Int32 ApplicantType = 1;

            //int ALevel_OffLineExamCenterOption_Theory1 = 0;
            //int ALevel_OffLineExamCenterOption_Pract1 = 0;
            if(currentCourseID==1)
                ALevel_OffLineExamCenterOption_Theory = 1;
            //if(currentCourseID==2)
            //    ALevel_OffLineExamCenterOption = 1;
            if(currentCourseID==3)
                ALevel_OffLineExamCenterOption_Theory = 2;
            if (currentCourseID == 4)
                ALevel_OffLineExamCenterOption_Theory = 2;
            using (EConnectContext context = new EConnectContext())
            {
                var course = context.Courses.Find(currentCourseID);
                if (course != null)
                    if (!String.IsNullOrEmpty(course.ApplicantTypeID.ToString()))
                    {
                        ApplicantType = course.ApplicantTypeID.Value;
                    }

                Exam exam = CourseManager.GetNextExam(context, currentCourseID, ApplicantType);

                Int32 stateID = Convert.ToInt32(ddlStateFirst.SelectedValue);

                var registrationDetail = (from s in context.RegistrationDetails
                                          where s.CourseID == currentCourseID && s.CandidateID == entityID && s.RegistrationNo == registrationNumber
                                          select s).FirstOrDefault();
               
                    //var examcentre = from p in context.ExamCenters
                    //                 join s in context.ExamWiseExamCenters on p.ID equals s.ExamCenterID
                    //                 orderby p.Name
                    //                 where p.StateID == stateID && (p.CourseCategoryID == exam.CourseCategoryID || p.CourseID == currentCourseID) &&
                    //                 s.ExamID == exam.ID && (s.ExamCentreTypeID == ALevel_OffLineExamCenterOption_Theory || s.ExamCentreTypeID == 3)
                    //                 select new
                    //                 {
                    //                     ValueField = p.ID,
                    //                   //TextField = SqlFunctions.StringConvert((double)s.ExamCentreTypeID, 1) == "1" ? p.Code.ToUpper() + " - " + p.Name + " (P)" : p.Code.ToUpper() + " - " + p.Name + " (S)",
                    //                     TextField = p.Code.ToUpper() + " - " + p.Name ,
                    //                     examCentreTypeID = s.ExamCentreTypeID
                    //                 };
                    //EConnect.Utils.Common.ControlUtility.BindListObject(DdlExamCentre1, examcentre, new ListItem("--Select Exam Centre--", "0"));
                if (currentCourseID == 2)
                {
                    foreach (ListItem item in chklistModules.Items)
                    {
                        if (item.Selected == true)
                        {
                            int selectedModuleID = Convert.ToInt32(item.Value);
                            Module selectedModule = context.Modules.Find(selectedModuleID);
                            int? selecctedModuleExamMode = selectedModule.ExamModeId;
                            if (selecctedModuleExamMode == 2)
                                ALevel_OffLineExamCenterOption_Theory = 2;
                            else
                                ALevel_OffLineExamCenterOption_Theory = 1;
                        }
                    }
                    foreach (ListItem item in chklistPracticals.Items)
                    {
                        if (item.Selected == true)
                        {
                            int selectedModuleID = Convert.ToInt32(item.Value);
                            Module selectedModule = context.Modules.Find(selectedModuleID);
                            int? selecctedModuleExamMode = selectedModule.ExamModeId;
                            if (selecctedModuleExamMode == 2)
                                ALevel_OffLineExamCenterOption_Pract = 2;
                        }
                    }
                    
                    if (ALevel_OffLineExamCenterOption_Pract == 0 && ALevel_OffLineExamCenterOption_Theory!=0)
                    {
                        var examcentre = from p in context.ExamCenters
                                         join s in context.ExamWiseExamCenters on p.ID equals s.ExamCenterID
                                         orderby p.Name
                                         where p.StateID == stateID && (p.CourseCategoryID == exam.CourseCategoryID || p.CourseID == currentCourseID) &&
                                         s.ExamID == exam.ID && (s.ExamCentreTypeID == ALevel_OffLineExamCenterOption_Theory || s.ExamCentreTypeID == 3)
                                         select new
                                         {
                                             ValueField = p.ID,
                                             //TextField = SqlFunctions.StringConvert((double)s.ExamCentreTypeID, 1) == "1" ? p.Code.ToUpper() + " - " + p.Name + " (P)" : p.Code.ToUpper() + " - " + p.Name + " (S)",
                                             TextField = p.Code.ToUpper() + " - " + p.Name,
                                             examCentreTypeID = s.ExamCentreTypeID
                                         };
                        //if (firstCentreID != 0)
                        //{
                        //    if (context.ExamWiseExamCenters.Where(c => (c.ExamCenterID == firstCentreID && c.ExamID == exam.ID)).FirstOrDefault().enmExamCenterType == enmExamCenterType.Secondary)
                        //        examcentre = examcentre.Where(c => c.examCentreTypeID == primary);

                        //}
                        EConnect.Utils.Common.ControlUtility.BindListObject(DdlExamCentre1, examcentre, new ListItem("--Select Exam Centre--", "0"));
                    }
                    else
                    {
                        var examcentre = from p in context.ExamCenters
                                         join s in context.ExamWiseExamCenters on p.ID equals s.ExamCenterID
                                         orderby p.Name
                                         where p.StateID == stateID && (p.CourseCategoryID == exam.CourseCategoryID || p.CourseID == currentCourseID) &&
                                         s.ExamID == exam.ID && (s.ExamCentreTypeID == ALevel_OffLineExamCenterOption_Pract || s.ExamCentreTypeID == 3)
                                         select new
                                         {
                                             ValueField = p.ID,
                                             //TextField = SqlFunctions.StringConvert((double)s.ExamCentreTypeID, 1) == "1" ? p.Code.ToUpper() + " - " + p.Name + " (P)" : p.Code.ToUpper() + " - " + p.Name + " (S)",
                                             TextField = p.Code.ToUpper() + " - " + p.Name,
                                             examCentreTypeID = s.ExamCentreTypeID
                                         };
                        //if (firstCentreID != 0)
                        //{
                        //    if (context.ExamWiseExamCenters.Where(c => (c.ExamCenterID == firstCentreID && c.ExamID == exam.ID)).FirstOrDefault().enmExamCenterType == enmExamCenterType.Secondary)
                        //        examcentre = examcentre.Where(c => c.examCentreTypeID == primary);

                        //}
                        EConnect.Utils.Common.ControlUtility.BindListObject(DdlExamCentre1, examcentre, new ListItem("--Select Exam Centre--", "0"));
                    }
                }
                else
                {
                    var examcentre = from p in context.ExamCenters
                                     join s in context.ExamWiseExamCenters on p.ID equals s.ExamCenterID
                                     orderby p.Name
                                     where p.StateID == stateID && (p.CourseCategoryID == exam.CourseCategoryID || p.CourseID == currentCourseID) &&
                                     //s.ExamID == exam.ID && (s.ExamCentreTypeID == ALevel_OffLineExamCenterOption_Theory || s.ExamCentreTypeID == 3)
                                     s.ExamID == exam.ID 
                                     select new
                                     {
                                         ValueField = p.ID,
                                         //TextField = SqlFunctions.StringConvert((double)s.ExamCentreTypeID, 1) == "1" ? p.Code.ToUpper() + " - " + p.Name + " (P)" : p.Code.ToUpper() + " - " + p.Name + " (S)",
                                         TextField = p.Code.ToUpper() + " - " + p.Name,
                                         examCentreTypeID = s.ExamCentreTypeID
                                     };
                    //if (firstCentreID != 0)
                    //{
                    //    if (context.ExamWiseExamCenters.Where(c => (c.ExamCenterID == firstCentreID && c.ExamID == exam.ID)).FirstOrDefault().enmExamCenterType == enmExamCenterType.Secondary)
                    //        examcentre = examcentre.Where(c => c.examCentreTypeID == primary);

                    //}
                    EConnect.Utils.Common.ControlUtility.BindListObject(DdlExamCentre1, examcentre, new ListItem("--Select Exam Centre--", "0"));
                }
              };           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void ddlStateSecond_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 ApplicantType = 1;
            Int32 firstCentreID = Convert.ToInt32(DdlExamCentre1.SelectedValue);
            Int32 primary = Convert.ToInt32(enmExamCenterType.Primary);
            Int32 secondary = Convert.ToInt32(enmExamCenterType.Secondary);
            if (currentCourseID == 1)
                ALevel_OffLineExamCenterOption_Theory = 1;
            //if (currentCourseID == 2)
            //    ALevel_OffLineExamCenterOption = 1;
            if (currentCourseID == 3)
                ALevel_OffLineExamCenterOption_Theory = 2;
            if (currentCourseID == 4)
                ALevel_OffLineExamCenterOption_Theory = 2;
            using (EConnectContext context = new EConnectContext())
            {
                var course = context.Courses.Find(currentCourseID);
                if (course != null)
                    if (!String.IsNullOrEmpty(course.ApplicantTypeID.ToString()))
                    {
                        ApplicantType = course.ApplicantTypeID.Value;
                    }
                Exam exam = CourseManager.GetNextExam(context, currentCourseID, ApplicantType);
                Int32 stateID = Convert.ToInt32(ddlStateSecond.SelectedValue);

                var registrationDetail = (from s in context.RegistrationDetails
                                          where s.CourseID == currentCourseID && s.CandidateID == entityID && s.RegistrationNo == registrationNumber
                                          select s).FirstOrDefault();
                if (currentCourseID == 2)
                {
                    foreach (ListItem item in chklistModules.Items)
                    {
                        if (item.Selected == true)
                        {
                            int selectedModuleID = Convert.ToInt32(item.Value);
                            Module selectedModule = context.Modules.Find(selectedModuleID);
                            int? selecctedModuleExamMode = selectedModule.ExamModeId;
                            if (selecctedModuleExamMode == 2)
                                ALevel_OffLineExamCenterOption_Theory = 2;
                            else
                                ALevel_OffLineExamCenterOption_Theory = 1;
                        }
                    }
                    foreach (ListItem item in chklistPracticals.Items)
                    {
                        if (item.Selected == true)
                        {
                            int selectedModuleID = Convert.ToInt32(item.Value);
                            Module selectedModule = context.Modules.Find(selectedModuleID);
                            int? selecctedModuleExamMode = selectedModule.ExamModeId;
                            if (selecctedModuleExamMode == 2)
                                ALevel_OffLineExamCenterOption_Pract = 2;
                        }
                    }

                    if (ALevel_OffLineExamCenterOption_Pract == 0 && ALevel_OffLineExamCenterOption_Theory != 0)
                    {
                        var examcentre = from p in context.ExamCenters
                                         join s in context.ExamWiseExamCenters on p.ID equals s.ExamCenterID
                                         orderby p.Name
                                         where p.StateID == stateID && (p.CourseCategoryID == exam.CourseCategoryID || p.CourseID == currentCourseID) &&
                                             //where p.StateID == stateID && (p.CourseCategoryID == exam.CourseCategoryID || p.CourseID == currentCourseID) &&
                                         s.ExamID == exam.ID && (s.ExamCentreTypeID == ALevel_OffLineExamCenterOption_Theory || s.ExamCentreTypeID == 3) && p.ID != firstCentreID
                                         select new
                                         {
                                             ValueField = p.ID,
                                             //TextField = SqlFunctions.StringConvert((double)s.ExamCentreTypeID, 1) == "1" ? p.Code.ToUpper() + " - " + p.Name + " (P)" : p.Code.ToUpper() + " - " + p.Name + " (S)",
                                             TextField = p.Code.ToUpper() + " - " + p.Name,
                                             examCentreTypeID = s.ExamCentreTypeID
                                         };
                        //if (firstCentreID != 0)
                        //{
                        //    if (context.ExamWiseExamCenters.Where(c => (c.ExamCenterID == firstCentreID && c.ExamID == exam.ID)).FirstOrDefault().enmExamCenterType == enmExamCenterType.Secondary)
                        //        examcentre = examcentre.Where(c => c.examCentreTypeID == primary);

                        //}
                        EConnect.Utils.Common.ControlUtility.BindListObject(DdlExamCentre2, examcentre, new ListItem("--Select Exam Centre--", "0"));
                    }
                    else
                    {
                        var examcentre = from p in context.ExamCenters
                                         join s in context.ExamWiseExamCenters on p.ID equals s.ExamCenterID
                                         orderby p.Name
                                         where p.StateID == stateID && (p.CourseCategoryID == exam.CourseCategoryID || p.CourseID == currentCourseID) &&
                                             //where p.StateID == stateID && (p.CourseCategoryID == exam.CourseCategoryID || p.CourseID == currentCourseID) &&
                                         s.ExamID == exam.ID && (s.ExamCentreTypeID == ALevel_OffLineExamCenterOption_Pract || s.ExamCentreTypeID == 3) && p.ID != firstCentreID
                                         select new
                                         {
                                             ValueField = p.ID,
                                             //TextField = SqlFunctions.StringConvert((double)s.ExamCentreTypeID, 1) == "1" ? p.Code.ToUpper() + " - " + p.Name + " (P)" : p.Code.ToUpper() + " - " + p.Name + " (S)",
                                             TextField = p.Code.ToUpper() + " - " + p.Name,
                                             examCentreTypeID = s.ExamCentreTypeID
                                         };
                        //if (firstCentreID != 0)
                        //{
                        //    if (context.ExamWiseExamCenters.Where(c => (c.ExamCenterID == firstCentreID && c.ExamID == exam.ID)).FirstOrDefault().enmExamCenterType == enmExamCenterType.Secondary)
                        //        examcentre = examcentre.Where(c => c.examCentreTypeID == primary);

                        //}
                        EConnect.Utils.Common.ControlUtility.BindListObject(DdlExamCentre2, examcentre, new ListItem("--Select Exam Centre--", "0"));
                    }
                }
                else
                {
                    var examcentre = from p in context.ExamCenters
                                     join s in context.ExamWiseExamCenters on p.ID equals s.ExamCenterID
                                     orderby p.Name
                                     where p.StateID == stateID && (p.CourseCategoryID == exam.CourseCategoryID || p.CourseID == currentCourseID) &&
                                         //where p.StateID == stateID && (p.CourseCategoryID == exam.CourseCategoryID || p.CourseID == currentCourseID) &&
                                     s.ExamID == exam.ID && (s.ExamCentreTypeID == ALevel_OffLineExamCenterOption_Theory || s.ExamCentreTypeID == 3) && p.ID != firstCentreID
                                     select new
                                     {
                                         ValueField = p.ID,
                                         //TextField = SqlFunctions.StringConvert((double)s.ExamCentreTypeID, 1) == "1" ? p.Code.ToUpper() + " - " + p.Name + " (P)" : p.Code.ToUpper() + " - " + p.Name + " (S)",
                                         TextField = p.Code.ToUpper() + " - " + p.Name,
                                         examCentreTypeID = s.ExamCentreTypeID
                                     };
                    //if (firstCentreID != 0)
                    //{
                    //    if (context.ExamWiseExamCenters.Where(c => (c.ExamCenterID == firstCentreID && c.ExamID == exam.ID)).FirstOrDefault().enmExamCenterType == enmExamCenterType.Secondary)
                    //        examcentre = examcentre.Where(c => c.examCentreTypeID == primary);

                    //}
                    EConnect.Utils.Common.ControlUtility.BindListObject(DdlExamCentre2, examcentre, new ListItem("--Select Exam Centre--", "0"));
                }
                };            
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void DdlExamCentre1_SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            ddlStateSecond.SelectedValue = "0";
            ddlStateSecond_SelectedIndexChanged(ddlStateSecond, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    #region[Practical_Exam_Center(Added by Reena)]

    protected void ddlPracStateFirst_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlPracStateSecond.SelectedValue = "0";
            ddlPracStateSecond_SelectedIndexChanged(ddlPracStateSecond, EventArgs.Empty);
            Int32 ApplicantType = 1;
            using (EConnectContext context = new EConnectContext())
            {
                var course = context.Courses.Find(currentCourseID);
                if (course != null)
                    if (!String.IsNullOrEmpty(course.ApplicantTypeID.ToString()))
                    {
                        ApplicantType = course.ApplicantTypeID.Value;
                    }

                Exam exam = CourseManager.GetNextExam(context, currentCourseID, ApplicantType);

                Int32 stateID = Convert.ToInt32(ddlPracStateFirst.SelectedValue);

                var registrationDetail = (from s in context.RegistrationDetails
                                          where s.CourseID == currentCourseID && s.CandidateID == entityID && s.RegistrationNo == registrationNumber
                                          select s).FirstOrDefault();
               
                var examcentre = from p in context.PracExamCenters
                                 //join s in context.ExamWiseExamCenters on p.ID equals s.ExamCenterID
                                 orderby p.Name
                                 where p.StateID == stateID && (p.CourseCategoryID == exam.CourseCategoryID )
                                // && s.ExamID == exam.ID
                                 select new
                                 {
                                     ValueField = p.ID,
                                     TextField = SqlFunctions.StringConvert((double)p.ExamCentreTypeID, 1) == "1" ? p.Code.ToUpper() + " - " + p.Name + " (P)" : p.Code.ToUpper() + " - " + p.Name + " (S)",
                                     examCentreTypeID = p.ExamCentreTypeID
                                 };
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlPracExamCentre1, examcentre, new ListItem("--Select Exam Centre--", "0"));
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void ddlPracStateSecond_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 ApplicantType = 1;
            Int32 firstCentreID = Convert.ToInt32(DdlPracExamCentre1.SelectedValue);
            Int32 primary = Convert.ToInt32(enmExamCenterType.Primary);
            Int32 secondary = Convert.ToInt32(enmExamCenterType.Secondary);
            using (EConnectContext context = new EConnectContext())
            {
                var course = context.Courses.Find(currentCourseID);
                if (course != null)
                    if (!String.IsNullOrEmpty(course.ApplicantTypeID.ToString()))
                    {
                        ApplicantType = course.ApplicantTypeID.Value;
                    }
                Exam exam = CourseManager.GetNextExam(context, currentCourseID, ApplicantType);
                Int32 stateID = Convert.ToInt32(ddlPracStateSecond.SelectedValue);

                var registrationDetail = (from s in context.RegistrationDetails
                                          where s.CourseID == currentCourseID && s.CandidateID == entityID && s.RegistrationNo == registrationNumber
                                          select s).FirstOrDefault();
               
                var examcentre = from p in context.PracExamCenters
                                 //join s in context.ExamWiseExamCenters on p.ID equals s.ExamCenterID
                                 orderby p.Name
                                 where p.StateID == stateID && (p.CourseCategoryID == exam.CourseCategoryID) && p.ID != firstCentreID
                                 // && s.ExamID == exam.ID 
                                 select new
                                 {
                                     ValueField = p.ID,
                                     TextField = SqlFunctions.StringConvert((double)p.ExamCentreTypeID, 1) == "1" ? p.Code.ToUpper() + " - " + p.Name + " (P)" : p.Code.ToUpper() + " - " + p.Name + " (S)",
                                     examCentreTypeID = p.ExamCentreTypeID
                                 };

                if (firstCentreID != 0)
                {
                    //if (context.ExamWiseExamCenters.Where(c => (c.ExamCenterID == firstCentreID && c.ExamID == exam.ID)).FirstOrDefault().enmExamCenterType == enmExamCenterType.Secondary)
                    //    examcentre = examcentre.Where(c => c.examCentreTypeID == primary);

                }
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlPracExamCentre2, examcentre, new ListItem("--Select Exam Centre--", "0"));
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void DdlPracExamCentre1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlPracStateSecond.SelectedValue = "0";
            ddlPracStateSecond_SelectedIndexChanged(ddlPracStateSecond, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    #endregion

    #region [Online_Theory_Exam_Center(Add by Reena)]
    protected void ddlOnlTheoryExamStateFirst_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlOnlTheoryExamStateSecond.SelectedValue = "0";
            ddlOnlTheoryExamStateSecond_SelectedIndexChanged(ddlOnlTheoryExamStateSecond, EventArgs.Empty);
            Int32 ApplicantType = 1;
            using (EConnectContext context = new EConnectContext())
            {
                var course = context.Courses.Find(currentCourseID);
                if (course != null)
                    if (!String.IsNullOrEmpty(course.ApplicantTypeID.ToString()))
                    {
                        ApplicantType = course.ApplicantTypeID.Value;
                    }

                Exam exam = CourseManager.GetNextExam(context, currentCourseID, ApplicantType);

                Int32 stateID = Convert.ToInt32(ddlOnlTheoryExamStateFirst.SelectedValue);

                var registrationDetail = (from s in context.RegistrationDetails
                                          where s.CourseID == currentCourseID && s.CandidateID == entityID && s.RegistrationNo == registrationNumber
                                          select s).FirstOrDefault();

                var examcentre = from p in context.OnlineExamCenters
                                 //join s in context.ExamWiseExamCenters on p.ID equals s.ExamCenterID
                                 orderby p.Name
                                 where p.StateID == stateID && (p.CourseCategoryID == exam.CourseCategoryID)
                                 // && s.ExamID == exam.ID
                                 select new
                                 {
                                     ValueField = p.ID,
                                     TextField = SqlFunctions.StringConvert((double)p.ExamCentreTypeID, 1) == "1" ? p.Code.ToUpper() + " - " + p.Name + " (P)" : p.Code.ToUpper() + " - " + p.Name + " (S)",
                                     examCentreTypeID = p.ExamCentreTypeID
                                 };
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlOnlTheoryExamCentre1, examcentre, new ListItem("--Select Exam Centre--", "0"));
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }
    protected void ddlOnlTheoryExamStateSecond_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 ApplicantType = 1;
            Int32 firstCentreID = Convert.ToInt32(DdlOnlTheoryExamCentre1.SelectedValue);
            Int32 primary = Convert.ToInt32(enmExamCenterType.Primary);
            Int32 secondary = Convert.ToInt32(enmExamCenterType.Secondary);
            using (EConnectContext context = new EConnectContext())
            {
                var course = context.Courses.Find(currentCourseID);
                if (course != null)
                    if (!String.IsNullOrEmpty(course.ApplicantTypeID.ToString()))
                    {
                        ApplicantType = course.ApplicantTypeID.Value;
                    }
                Exam exam = CourseManager.GetNextExam(context, currentCourseID, ApplicantType);
                Int32 stateID = Convert.ToInt32(ddlOnlTheoryExamStateSecond.SelectedValue);

                var registrationDetail = (from s in context.RegistrationDetails
                                          where s.CourseID == currentCourseID && s.CandidateID == entityID && s.RegistrationNo == registrationNumber
                                          select s).FirstOrDefault();

                var examcentre = from p in context.OnlineExamCenters
                                 //join s in context.ExamWiseExamCenters on p.ID equals s.ExamCenterID
                                 orderby p.Name
                                 where p.StateID == stateID && (p.CourseCategoryID == exam.CourseCategoryID) && p.ID != firstCentreID
                                 // && s.ExamID == exam.ID 
                                 select new
                                 {
                                     ValueField = p.ID,
                                     TextField = SqlFunctions.StringConvert((double)p.ExamCentreTypeID, 1) == "1" ? p.Code.ToUpper() + " - " + p.Name + " (P)" : p.Code.ToUpper() + " - " + p.Name + " (S)",
                                     examCentreTypeID = p.ExamCentreTypeID
                                 };

                if (firstCentreID != 0)
                {
                    //if (context.ExamWiseExamCenters.Where(c => (c.ExamCenterID == firstCentreID && c.ExamID == exam.ID)).FirstOrDefault().enmExamCenterType == enmExamCenterType.Secondary)
                    //    examcentre = examcentre.Where(c => c.examCentreTypeID == primary);

                }
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlOnlTheoryExamCentre2, examcentre, new ListItem("--Select Exam Centre--", "0"));
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void DdlOnlTheoryExamCentre1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlOnlTheoryExamStateSecond.SelectedValue = "0";
            ddlOnlTheoryExamStateSecond_SelectedIndexChanged(ddlOnlTheoryExamStateSecond, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    
    #endregion

    protected void ddlModules_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 theoryCount = 0;
            Int32 practicalCount = 0;

            if (ddlModules.SelectedValue != "0")
                theoryCount = 1;

            // Int32 x = Convert.ToInt32(chklistPracticalsExmpt.SelectedValue);

            foreach (ListItem item in chklistPracticals.Items)
            {
                if (item.Selected == true)
                    practicalCount++;
            }

            ddlStateFirst.SelectedValue = "0";
            ddlStateFirst_SelectedIndexChanged(ddlStateFirst, EventArgs.Empty);
            ALevel_OffLineExamCenterOption_Theory = 0;
            ALevel_OffLineExamCenterOption_Pract = 0;
            //string x = chklistPracticalsExmpt.SelectedValue;
            //if (chklistPracticalsExmpt.SelectedValue != "0" || chklistPracticalsExmpt.SelectedValue != "")
            //    practicalCount = 1;
            //if (chklistPracticalsExmpt.SelectedValue == "") 
            //    practicalCount = 0;

            int modulid =Convert.ToInt16(ddlModules.SelectedValue);
            EConnectContext context = new EConnectContext();
            Module selectedModule = context.Modules.Find(modulid);
            if (currentCourseID == 2 && selectedModule.ExamModeId == 1)
                ALevel_OffLineExamCenterOption_Theory = 1;
            else
                ALevel_OffLineExamCenterOption_Theory = 2;

            selectedModules.Value = theoryCount.ToString();
            lblTheoryCount.Text = theoryCount.ToString();
            lblTotalTheoryFee.Text = (Convert.ToSingle(lblTheoryFee.Text) * theoryCount).ToString("F");
            // lblPracticalCount.Text = "0";
            lblPracticalCount.Text = practicalCount.ToString();
            //lblTotalPracticalFee.Text = (Convert.ToSingle(lblPracticalFee.Text) * 0).ToString("F");
            lblTotalPracticalFee.Text = (Convert.ToSingle(lblPracticalFee.Text) * practicalCount).ToString("F");
            if (lblTotalLateFee.Text == "")
            {
                lblTotalLateFee.Text = "0.00";
            }
            lblTotalFee.Text = (Convert.ToSingle(lblTotalTheoryFee.Text) + Convert.ToSingle(lblTotalLateFee.Text) + Convert.ToSingle(lblTotalPracticalFee.Text) + Convert.ToSingle(lblTotalProcessingFee.Text)).ToString("F");
            if (Convert.ToSingle(lblTotalLateFee.Text) == Convert.ToSingle(lblTotalFee.Text))
            {
                lblTotalFee.Text = "0.0";
            }
            lblAmountHindi.Text = EConnect.Utils.Conversion.ConversionUtility.NumberToText(lblTotalFee.Text, EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void ShowApplicationData(Int64 applID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                CourseExamApplication appl = context.CourseExamApplications.Find(applID);
                Candidate candidate = appl.Candidate;
                if (appl != null)
                {
                    var registrationDetail = (from s in context.RegistrationDetails
                                              where s.CourseID == appl.CourseID && s.CandidateID == entityID && s.RegistrationNo == appl.RegistrationNumber
                                              select s).FirstOrDefault();

                    if (ddlCandidateType.Enabled)
                        ddlCandidateType.SelectedValue = appl.ApplicantTypeID.ToString();
                    else
                        ddlCandidateType.SelectedValue = registrationDetail.ApplicantTypeID.ToString();

                    //if (appl.enmApplicantType == enmApplicantType.Direct)
                    //{
                    //    ddlCandidateType.Enabled = false;
                    //    ddlPaymentOption.SelectedValue = "1";
                    //    ddlPaymentOption.Enabled = false;
                    //}
                    //else
                    //{
                    //    ddlPaymentOption.SelectedValue = "2";
                    //    ddlPaymentOption.Enabled = true;
                    //    Int32 totalExamsAttempted = lastExams.Count();
                    //    if (currentCourseID == 1 || currentCourseID == 2)
                    //    {
                    //        if (totalExamsAttempted > 2)
                    //            ddlCandidateType.Enabled = true;
                    //        else
                    //            ddlCandidateType.Enabled = false;
                    //    }
                    //    else
                    //    {
                    //        if (totalExamsAttempted > 6)
                    //            ddlCandidateType.Enabled = true;
                    //        else
                    //            ddlCandidateType.Enabled = false;
                    //    }
                    //}
                     ddlPaymentOption.SelectedValue = appl.PaymentSourceID.ToString();

                     #region[Fill_Online_Offline_Prac_Exam_Center_Value_On_Edit_Mode (Add By Reena)]
                     if (currentCourseID == 1) //Reena
                     {

                         if (TrExamCentre1.Visible == true)
                         {
                             ddlStateFirst.SelectedValue = appl.ExamCenter1.StateID.ToString();
                             ddlStateFirst_SelectedIndexChanged(ddlStateFirst, EventArgs.Empty);
                             DdlExamCentre1.SelectedValue = appl.ExamCenter1ID.ToString();

                             ddlStateSecond.SelectedValue = appl.ExamCenter2.StateID.ToString();
                             ddlStateSecond_SelectedIndexChanged(ddlStateSecond, EventArgs.Empty);
                             DdlExamCentre2.SelectedValue = appl.ExamCenter2ID.ToString();
                         }
                         //Added by Reena as on (03/05/2023)
                         if (TrOnlTheoryExamCenter1.Visible == true)
                         {
                             ddlOnlTheoryExamStateFirst.SelectedValue = appl.OnlineExamCenter1.StateID.ToString();
                             ddlOnlTheoryExamStateFirst_SelectedIndexChanged(ddlOnlTheoryExamStateFirst, EventArgs.Empty);
                             DdlOnlTheoryExamCentre1.SelectedValue = appl.OnlineExamCenter1ID.ToString();

                             ddlOnlTheoryExamStateSecond.SelectedValue = appl.OnlineExamCenter2.StateID.ToString();
                             ddlOnlTheoryExamStateSecond_SelectedIndexChanged(ddlOnlTheoryExamStateSecond, EventArgs.Empty);
                             DdlOnlTheoryExamCentre2.SelectedValue = appl.OnlineExamCenter2ID.ToString();
                         }

                         if (TrPracExamCenter1.Visible == true)
                         {
                             ddlPracStateFirst.SelectedValue = appl.PracExamCenter1.StateID.ToString();
                             ddlPracStateFirst_SelectedIndexChanged(ddlPracStateFirst, EventArgs.Empty);
                             DdlPracExamCentre1.SelectedValue = appl.PracExamCenter1ID.ToString();

                             ddlPracStateSecond.SelectedValue = appl.PracExamCenter2.StateID.ToString();
                             ddlPracStateSecond_SelectedIndexChanged(ddlPracStateSecond, EventArgs.Empty);
                             DdlPracExamCentre2.SelectedValue = appl.PracExamCenter2ID.ToString();
                         }
                        //TrOnlTheoryExamCenter1.Visible = true;
                        //TrOnlTheoryExamCenter2.Visible = true;
                        //TrPracExamCenter1.Visible = true;
                        //TrPracExamCenter2.Visible = true;

                        LblOnlSI1.Text = "2.1";
                        LblOnlSI2.Text = "2.2";
                        LblPracSI1.Text = "2.3";
                        LblPracSI2.Text = "2.4";
                         //end
                    }
                     if (currentCourseID == 1213)
                     {
                         if (TrExamCentre1.Visible == true)
                         {
                             ddlStateFirst.SelectedValue = appl.ExamCenter1.StateID.ToString();
                             ddlStateFirst_SelectedIndexChanged(ddlStateFirst, EventArgs.Empty);
                             DdlExamCentre1.SelectedValue = appl.ExamCenter1ID.ToString();

                             ddlStateSecond.SelectedValue = appl.ExamCenter2.StateID.ToString();
                             ddlStateSecond_SelectedIndexChanged(ddlStateSecond, EventArgs.Empty);
                             DdlExamCentre2.SelectedValue = appl.ExamCenter2ID.ToString();
                             LblOnlSI1.Text = "2.1";
                             LblOnlSI2.Text = "2.2";
                         }
                     }
                    if (currentCourseID == 2) //Reena
                    {
                        if (TrExamCentre1.Visible == true)
                        {
                            ddlStateFirst.SelectedValue = appl.ExamCenter1.StateID.ToString();
                            ddlStateFirst_SelectedIndexChanged(ddlStateFirst, EventArgs.Empty);
                            DdlExamCentre1.SelectedValue = appl.ExamCenter1ID.ToString();

                            ddlStateSecond.SelectedValue = appl.ExamCenter2.StateID.ToString();
                            ddlStateSecond_SelectedIndexChanged(ddlStateSecond, EventArgs.Empty);
                            DdlExamCentre2.SelectedValue = appl.ExamCenter2ID.ToString();
                        }
                        //Added by Reena as on (30/04/2023)
                        if (TrPracExamCenter1.Visible == true)
                        {
                            ddlPracStateFirst.SelectedValue = appl.PracExamCenter1.StateID.ToString();
                            ddlPracStateFirst_SelectedIndexChanged(ddlPracStateFirst, EventArgs.Empty);
                            DdlPracExamCentre1.SelectedValue = appl.PracExamCenter1ID.ToString();

                            ddlPracStateSecond.SelectedValue = appl.PracExamCenter2.StateID.ToString();
                            ddlPracStateSecond_SelectedIndexChanged(ddlPracStateSecond, EventArgs.Empty);
                            DdlPracExamCentre2.SelectedValue = appl.PracExamCenter2ID.ToString();
                        }
                        //end

                        //Added by Reena as on (03/05/2023)
                        if (TrOnlTheoryExamCenter1.Visible == true)
                        {
                            ddlOnlTheoryExamStateFirst.SelectedValue = appl.OnlineExamCenter1.StateID.ToString();
                            ddlOnlTheoryExamStateFirst_SelectedIndexChanged(ddlOnlTheoryExamStateFirst, EventArgs.Empty);
                            DdlOnlTheoryExamCentre1.SelectedValue = appl.OnlineExamCenter1ID.ToString();

                            ddlOnlTheoryExamStateSecond.SelectedValue = appl.OnlineExamCenter2.StateID.ToString();
                            ddlOnlTheoryExamStateSecond_SelectedIndexChanged(ddlOnlTheoryExamStateSecond, EventArgs.Empty);
                            DdlOnlTheoryExamCentre2.SelectedValue = appl.OnlineExamCenter2ID.ToString();
                        }
                        //end
                        //TrOnlTheoryExamCenter1.Visible = true;
                        //TrOnlTheoryExamCenter2.Visible = true;
                        //TrPracExamCenter1.Visible = true;
                        //TrPracExamCenter2.Visible = true;
                        //TrExamCentre1.Visible = true;
                        //TrExamCentre2.Visible = true;
                        LblSiOff1.Text = "3.1";
                        LblSiOff2.Text = "3.2";
                        LblOnlSI1.Text = "2.3";
                        LblOnlSI2.Text = "2.4";
                        LblPracSI1.Text = "2.5";
                        LblPracSI2.Text = "2.6";
                        //end
                    }
                    if (currentCourseID == 3 || currentCourseID == 4)//Reena
                    {
                        if (TrExamCentre1.Visible == true)
                        {
                            ddlStateFirst.SelectedValue = appl.ExamCenter1.StateID.ToString();
                            ddlStateFirst_SelectedIndexChanged(ddlStateFirst, EventArgs.Empty);
                            DdlExamCentre1.SelectedValue = appl.ExamCenter1ID.ToString();

                            ddlStateSecond.SelectedValue = appl.ExamCenter2.StateID.ToString();
                            ddlStateSecond_SelectedIndexChanged(ddlStateSecond, EventArgs.Empty);
                            DdlExamCentre2.SelectedValue = appl.ExamCenter2ID.ToString();
                        }
                        if (TrPracExamCenter1.Visible == true)
                        {
                            ddlPracStateFirst.SelectedValue = appl.PracExamCenter1.StateID.ToString();
                            ddlPracStateFirst_SelectedIndexChanged(ddlPracStateFirst, EventArgs.Empty);
                            DdlPracExamCentre1.SelectedValue = appl.PracExamCenter1ID.ToString();

                            ddlPracStateSecond.SelectedValue = appl.PracExamCenter2.StateID.ToString();
                            ddlPracStateSecond_SelectedIndexChanged(ddlPracStateSecond, EventArgs.Empty);
                            DdlPracExamCentre2.SelectedValue = appl.PracExamCenter2ID.ToString();
                        }
                        //TrPracExamCenter1.Visible = true;
                        //TrPracExamCenter2.Visible = true;
                        //TrExamCentre1.Visible = true;
                        //TrExamCentre2.Visible = true;
                        LblSiOff1.Text = "3.1";
                        LblSiOff2.Text = "3.2";                        
                        LblPracSI1.Text = "2.3";
                        LblPracSI2.Text = "2.4";

                    }
                     #endregion

                    var modules = (from a in context.CourseExamApplicationDetails
                                   join m in context.Modules on a.ModuleID equals m.ID
                                   where a.CourseExamApplicationID == applID
                                   orderby m.Code
                                   select new { m.ShortName, m.Name, m.ModuleTypeID, a.FeeAmount, m.ID });
                    Int32 practical = Convert.ToInt32(enmModuleType.Practical);
                    Int32 practicalFee = 0;
                    Int32 theoryFee = 0;
                    if (appl.IsImprovementApplication == false)
                    {
                        foreach (var module in modules.ToList())
                        {
                            if (module.ModuleTypeID != practical)
                            {
                                foreach (ListItem item in chklistModules.Items)
                                {
                                    if (module.ID == Convert.ToInt64(item.Value))
                                    {
                                        item.Selected = true;
                                        theoryFee += (Int32)module.FeeAmount;
                                    }
                                }
                            }
                            else
                            {
                                foreach (ListItem item in chklistPracticals.Items)
                                {
                                    if (module.ID == Convert.ToInt64(item.Value))
                                    {
                                        item.Selected = true;
                                        practicalFee += (Int32)module.FeeAmount;
                                    }
                                }
                            }
                        }
                        UpdateFeeDetails();
                    }
                    else
                    {
                        ddlModules.SelectedValue = modules.FirstOrDefault().ID.ToString();
                        ddlModules_SelectedIndexChanged(ddlModules, EventArgs.Empty);
                    }

                    if (appl.FinalSubmitted)
                    {
                        lblCoutMessage.Text = "Theory Papers ";
                        LnkBtnPrintForm.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("ExamFormPreview.aspx?Appid=" + appl.ID.ToString()) + "');");
                        LnkBtnPrintForm.Visible = true;
                        btnSave.Visible = false;
                        btnback.Visible = true;
                        ddlStateFirst.Enabled = false;
                        ddlStateSecond.Enabled = false;
                        DdlExamCentre1.Enabled = false;
                        DdlExamCentre2.Enabled = false;
                        ddlModules.Enabled = false;
                        chklistModules.Enabled = false;
                        chklistPracticals.Enabled = false;
                        lblTotalFee.Text = appl.FeeAmount.ToString("F");

                        List<ListItem> lstTheory = new List<ListItem>();
                        List<ListItem> lstPactical = new List<ListItem>();
                        foreach (var module in modules.ToList())
                        {
                            if (module.ModuleTypeID != practical)
                            {
                                lstTheory.Add(new ListItem(module.ShortName + " " + module.Name, module.ID.ToString()));
                            }
                            else
                            {
                                lstPactical.Add(new ListItem(module.ShortName + " " + module.Name, module.ID.ToString()));
                            }
                        }
                        chklistModules.DataSource = lstTheory.Select(c => new { ValueField = c.Value, TextField = c.Text });
                        chklistModules.DataBind();
                        chklistPracticals.DataSource = lstPactical.Select(c => new { ValueField = c.Value, TextField = c.Text });
                        chklistPracticals.DataBind();
                        if (appl.LateFeeImposed)
                        {
                            lblLateFee.Text = appl.LateFeeAmount.Value.ToString("F");
                        }
                        chkdisclamier.Enabled = false;
                        lblerror.Text = "You have already applied for this exam.";
                    }
                    else
                    {
                        btnSave.Text = "Update";
                    }
                    lblAmountHindi.Text = EConnect.Utils.Conversion.ConversionUtility.NumberToText(lblTotalFee.Text, EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English);
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlCandidateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            enmApplicantType selectedType = (enmApplicantType)Convert.ToInt32(ddlCandidateType.SelectedValue);
            if (selectedType == enmApplicantType.Direct)
            {
                ddlPaymentOption.SelectedValue = "1";
                ddlPaymentOption.Enabled = false;
            }
            else
            {
                ddlPaymentOption.SelectedValue = "2";
                ddlPaymentOption.Enabled = true; ;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected Int32 PassedConditionalTheoryModuleCount(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber, Int64 candidateID)
    { 
     try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 theory = Convert.ToInt32(enmModuleType.Theory);
                Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
                Int32 PassesdConditionalTheoryModules = (from d in context.CourseExamApplicationDetails
                                                         join m in context.Modules on d.ModuleID equals m.ID
                                                         where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                         d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && (m.ID == 402 || m.ID == 404 || m.ID == 415 || m.ID == 417 || m.ID == 407 || m.ID == 54 || m.ID == 58 || m.ID == 66 || m.ID == 222  || m.ID == 225)
                                                         orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                         select d.ModuleID).Count();
                return PassesdConditionalTheoryModules;
            }
           
        }
     catch (Exception ex)
     {
         throw ex;
     }
    }

    protected static Int32 PassedConditionalTheoryModuleCount_Alevel(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber, Int64 candidateID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 theory = Convert.ToInt32(enmModuleType.Theory);
                Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
                Int32 PassesdConditionalTheoryModules = (from d in context.CourseExamApplicationDetails
                                                         join m in context.Modules on d.ModuleID equals m.ID
                                                         where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                         d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && (m.ID == 354 || m.ID == 355 || m.ID == 356 || m.ID == 29 || m.ID == 30 || m.ID == 31 || m.ID == 142 || m.ID == 144 || m.ID == 145)
                                                         orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                         select d.ModuleID).Count();
                if (PassesdConditionalTheoryModules > 0)
                {

                    
                        int datacnt = (8 - A1toA8passedmoduleCount(registrationNumber, candidateID));
                        if (datacnt >= PassesdConditionalTheoryModules)
                            return PassesdConditionalTheoryModules;
                        else
                            return datacnt;
                 }
                
                else
                {
                    return 0;
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
     }

    protected static int CheckExemptioDoneOrNot(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber, Int64 candidateID)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                //SqlConnection con = new SqlConnection("Data Source=10.246.112.177;Initial Catalog=NIELIT;Persist Security Info=True;User ID=shaukat; Password=Db4PareekshaUAT@9211$; MultipleActiveResultSets=True;Timeout = 120; Max Pool Size=1000");

                //string sql = "Select ID FROM Course_Exam_Exemption where Candidate_ID='" + candidateID + "' and Registration_Number='" + registrationNumber + "' and Exempted_Course_ID='" + currentCourseID + "' and Exempted_Revision_Number='" + currentevisionNumber + "'";

                //November_2024
                string sql = "Select ID FROM Course_Exam_Exemption where Candidate_ID=@candidateID and Registration_Number=@registrationNumber and Exempted_Course_ID=@currentCourseID and Exempted_Revision_Number=@currentevisionNumber ";
                SqlDataAdapter ad = new SqlDataAdapter(sql, con);

                //November_2024
                ad.SelectCommand.Parameters.AddWithValue("@candidateID", candidateID);
                ad.SelectCommand.Parameters.AddWithValue("@registrationNumber", registrationNumber);
                ad.SelectCommand.Parameters.AddWithValue("@currentCourseID", currentCourseID);
                ad.SelectCommand.Parameters.AddWithValue("@currentevisionNumber", currentevisionNumber);

                con.Open();
                DataTable dt = new DataTable();
                ad.Fill(dt);
                int count = 0;
                count = dt.Rows.Count;
                con.Close();
                return count;

            }

            //using (EConnectContext context = new EConnectContext())
            //{
            //    int count = (from s in context.tblModuleExemption where s.Exempted_Course_ID == currentCourseID && s.Registration_Number == registrationNumber && s.Candidate_ID == candidateID select s.ID).Count();
            //    return count;
            //}
         
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected static Int32 PassedConditionalTheoryModuleCount1(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 theory = Convert.ToInt32(enmModuleType.Theory);
                Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
                Int32 PassesdConditionalTheoryModules = (from d in context.CourseExamApplicationDetails
                                                         join m in context.Modules on d.ModuleID equals m.ID
                                                         where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber &&
                                                         d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && (m.ID == 402 || m.ID == 404 || m.ID == 415 || m.ID == 417 || m.ID == 407 || m.ID == 54 || m.ID == 58 || m.ID == 66 || m.ID == 222  || m.ID == 225)
                                                         orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                         select d.ModuleID).Count();
                return PassesdConditionalTheoryModules;
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //protected static Int32 PassedConditionalTheoryModuleCount1_Alevel(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber)
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            Int32 theory = Convert.ToInt32(enmModuleType.Theory);
    //            Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
    //            Int32 PassesdConditionalTheoryModules = (from d in context.CourseExamApplicationDetails
    //                                                     join m in context.Modules on d.ModuleID equals m.ID
    //                                                     where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber &&
    //                                                     d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && (m.ID == 354 || m.ID == 355 || m.ID == 356 || m.ID == 29 || m.ID == 30 || m.ID == 31 || m.ID == 142 || m.ID == 144 || m.ID == 145)
    //                                                     orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
    //                                                     select d.ModuleID).Count();
    //            return PassesdConditionalTheoryModules;
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    protected static ICollection<Module> PassedConditionalTheoryModuleCount2(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 theory = Convert.ToInt32(enmModuleType.Theory);
                Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
                IQueryable<Module> PassesdConditionalTheoryModules = (from d in context.CourseExamApplicationDetails
                                                         join m in context.Modules on d.ModuleID equals m.ID
                                                         where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber &&
                                                         d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && (m.ID == 402 || m.ID == 404 || m.ID == 415 || m.ID == 417 || m.ID == 407 || m.ID == 54 || m.ID == 58 || m.ID == 66 || m.ID == 222  || m.ID == 225)
                                                         orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                         select d.Module);
                return PassesdConditionalTheoryModules.ToList();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected static ICollection<Module> PassedConditionalTheoryModuleCount2_Alevel(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 theory = Convert.ToInt32(enmModuleType.Theory);
                Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
                IQueryable<Module> PassesdConditionalTheoryModules = (from d in context.CourseExamApplicationDetails
                                                                      join m in context.Modules on d.ModuleID equals m.ID
                                                                      where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber &&
                                                                      d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && (m.ID == 354 || m.ID == 355 || m.ID == 356 || m.ID == 29 || m.ID == 30 || m.ID == 31 || m.ID == 142 || m.ID == 144 || m.ID == 145)
                                                                      orderby d.MarksTotal
                                                                      select d.Module);
                return PassesdConditionalTheoryModules.ToList();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected int GetSpOLevelPassedPractical(Int64 registrationNumber, Int64 candidateID)
    {
        using (EConnectContext context = new EConnectContext())
        {


            List<int> frt =new List<Int32> { 284,344,715};

          
            List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                               join m in context.Modules on d.ModuleID equals m.ID
                                               where d.CourseID == 1 && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                               d.Grade.IsPassed == true && (m.ModuleTypeID == 3)
                                               orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                               select d.ModuleID).ToList();
            List<int> data1 = passedTotalanyrevison.Intersect(frt).ToList();
            return data1.Count;

        }
    }

    protected List<Int32> GetSpOLevelPassedModule(Int64 registrationNumber, Int32 currentrevision, Int64 candidateID)
    {
        List<Int32> data = new List<Int32>();
        //try
        //{
        using (EConnectContext context = new EConnectContext())
        {




            List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                               join m in context.Modules on d.ModuleID equals m.ID
                                               where d.CourseID == 1 && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                               d.Grade.IsPassed == true && (m.ModuleTypeID == 1)
                                               orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                               select d.ModuleID).ToList();



            //List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 1 select m.ID).Distinct().ToList();
            //int TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).Count();
            if (currentrevision == 3)
            {
                List<Int32> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 1 select m.ID).ToList();
                List<Int32> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();

                List<Int32> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 1 select m.ID).ToList();
                List<Int32> PassedModuleInR3 = allModulR2.Intersect(passedTotalanyrevison).ToList();

                List<int> R2ToR3 = PassedModuleInR2.Concat(PassedModuleInR3).ToList();
                data = R2ToR3;
            }


            if (currentrevision == 4)
            {
                List<Int32> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 1 select m.ID).ToList();
                List<Int32> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();

                List<Int32> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 1 select m.ID).ToList();
                List<Int32> PassedModuleInR3 = allModulR2.Intersect(passedTotalanyrevison).ToList();


                List<Int32> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 1 select m.ID).ToList();
                List<Int32> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();

                List<int> R2ToR3 = PassedModuleInR2.Concat(PassedModuleInR3).ToList();


                List<Int32> TotalPassedModuletillR4 = R2ToR3.Concat(PassedModuleInR4).ToList();
                data= TotalPassedModuletillR4;
            }

            if (currentrevision == 5)
            {
                List<Int32> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 1 select m.ID).ToList();
                List<Int32> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();

                List<Int32> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 1 select m.ID).ToList();
                List<Int32> PassedModuleInR3 = allModulR2.Intersect(passedTotalanyrevison).ToList();


                List<Int32> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 1 select m.ID).ToList();
                List<Int32> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();

                List<Int32> R2ToR3 = PassedModuleInR2.Concat(PassedModuleInR3).ToList();


                List<Int32> TotalPassedModuletillR4 = R2ToR3.Concat(PassedModuleInR4).ToList();
               // return TotalPassedModuletillR4;

                List<Int32> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 1 select m.ID).ToList();
                List<Int32> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();

                List<Int32> TotalPassedModuletillR5 = PassedModuleInR5.Concat(TotalPassedModuletillR4).ToList();
                data= TotalPassedModuletillR5;
            }
            //var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
            //List<Int32> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 1 select m.ID).ToList();
            //List<Int32> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
            //List<Int32> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
            ////if(currentrevision==5)
            //return TotalPassedModuleR4toR5;
            ////List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
            //var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
            ////List<int> PassedModuleInR52 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();
            //int countTotalPassedInOlevel = (TotalpasedinR6 + passedModuleinparityR5ToR6.Count());
            //theorymodulecount = ((allTheoryModules + allBridgeModules) - countTotalPassedInOlevel);

            //return TotalPassedModuleR4toR5;
            return data;
        }
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
    }

    private DataTable GetData(Int64 currentCourseID, Int64 registrationNumber, Int64 entityID, int RevisionNoOfCandiadte)
    {
        DataTable dt1 = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

        // commented by abhi on dated 31052023 and commented code in implemented code for hide passed conditional module against exemption
        string sql = "";
        if (currentCourseID == 2 && RevisionNoOfCandiadte > 4)
        {
           // sql = "select cad.Course_ID CourseID, m.id as ID , upper(m.Name)  as name , m.Short_Name as Code , m.Module_Type_ID  as ModuleTypeID, " +
           //" m.Selection_Type_ID as SelectionTypeID, m.Elective_Group as ElectiveGroup,mt.Name +  ( " +
           //" CASE WHEN m.Module_Type_ID =1 and m.Selection_Type_ID =1 and Sub_Number =0 then '(Comp.)' " +
           //" WHEN m.Module_Type_ID =1 and m.Selection_Type_ID > 1 and Sub_Number >0 then '(Elect.)' else  '' END) as MType,  " +
           //" isnull(e.Name ,'NA') as doexam,rg.Description as Result , rg.Code as Grade from " +
           //" Module_Type mt,  module m , Result_Grading rg,Course_Exam_Application_Detail cad LEFT OUTER JOIN exam e on cad.Exam_ID =e.id " +
           //" where cad.Module_ID =m.id and cad.Result_Grade_ID =rg.id and m.Module_Type_ID =mt.ID " +
           //" and rg.Code <>'$'  and rg.Is_Passed = 1  and cad.Module_ID not in (933,934,935,936,956,957,958,959,354,355,356) and cad.Registration_Number ='" + registrationNumber + "' and cad.Candidate_ID ='" + entityID + "' and cad.Course_ID ='" + currentCourseID + "' and m.id not in (select base_module_id from Course_Exam_Exemption where Registration_Number='" + registrationNumber + "' and Exempted_Course_ID= '" + currentCourseID + "' and Candidate_ID='" + entityID + "')" +
           //" order by m.Module_Type_ID , m.Selection_Type_ID , m.ID ";

            //November_2024
            sql = "select cad.Course_ID CourseID, m.id as ID , upper(m.Name)  as name , m.Short_Name as Code , m.Module_Type_ID  as ModuleTypeID, " +
            " m.Selection_Type_ID as SelectionTypeID, m.Elective_Group as ElectiveGroup,mt.Name +  ( " +
            " CASE WHEN m.Module_Type_ID =1 and m.Selection_Type_ID =1 and Sub_Number =0 then '(Comp.)' " +
            " WHEN m.Module_Type_ID =1 and m.Selection_Type_ID > 1 and Sub_Number >0 then '(Elect.)' else  '' END) as MType,  " +
            " isnull(e.Name ,'NA') as doexam,rg.Description as Result , rg.Code as Grade from " +
            " Module_Type mt,  module m , Result_Grading rg,Course_Exam_Application_Detail cad LEFT OUTER JOIN exam e on cad.Exam_ID =e.id " +
            " where cad.Module_ID =m.id and cad.Result_Grade_ID =rg.id and m.Module_Type_ID =mt.ID " +
            " and rg.Code <>'$'  and rg.Is_Passed = 1  and cad.Module_ID not in (933,934,935,936,956,957,958,959,354,355,356) and cad.Registration_Number =@registrationNumber and cad.Candidate_ID =@entityID and cad.Course_ID =@currentCourseID and m.id not in (select base_module_id from Course_Exam_Exemption where Registration_Number=@registrationNumber and Exempted_Course_ID= @currentCourseID and Candidate_ID=@entityID)" +
            " order by m.Module_Type_ID , m.Selection_Type_ID , m.ID ";

        }
        else
        {
            //sql = "select cad.Course_ID CourseID, m.id as ID , upper(m.Name)  as name , m.Short_Name as Code , m.Module_Type_ID  as ModuleTypeID, " +
            //  " m.Selection_Type_ID as SelectionTypeID, m.Elective_Group as ElectiveGroup,mt.Name +  ( " +
            //  " CASE WHEN m.Module_Type_ID =1 and m.Selection_Type_ID =1 and Sub_Number =0 then '(Comp.)' " +
            //  " WHEN m.Module_Type_ID =1 and m.Selection_Type_ID >1 and Sub_Number >0 then '(Elect.)' else  '' END) as MType,  " +
            //  " isnull(e.Name ,'NA') as doexam,rg.Description as Result , rg.Code as Grade from " +
            //  " Module_Type mt,  module m , Result_Grading rg,Course_Exam_Application_Detail cad LEFT OUTER JOIN exam e on cad.Exam_ID =e.id " +
            //  " where cad.Module_ID =m.id and cad.Result_Grade_ID =rg.id and m.Module_Type_ID =mt.ID " +
            //  " and rg.Code <>'$'  and rg.Is_Passed = 1  and cad.Module_ID not in (933,934,935,936,956,957,958,959) and cad.Registration_Number ='" + registrationNumber + "' and cad.Candidate_ID ='" + entityID + "' and cad.Course_ID ='" + currentCourseID + "' and m.id not in (select base_module_id from Course_Exam_Exemption where Registration_Number='" + registrationNumber + "' and Exempted_Course_ID= '" + currentCourseID + "' and Candidate_ID='" + entityID + "')" +
            //  " order by m.Module_Type_ID , m.Selection_Type_ID , m.ID ";

            //November_2024
            sql = "select cad.Course_ID CourseID, m.id as ID , upper(m.Name)  as name , m.Short_Name as Code , m.Module_Type_ID  as ModuleTypeID, " +
              " m.Selection_Type_ID as SelectionTypeID, m.Elective_Group as ElectiveGroup,mt.Name +  ( " +
              " CASE WHEN m.Module_Type_ID =1 and m.Selection_Type_ID =1 and Sub_Number =0 then '(Comp.)' " +
              " WHEN m.Module_Type_ID =1 and m.Selection_Type_ID >1 and Sub_Number >0 then '(Elect.)' else  '' END) as MType,  " +
              " isnull(e.Name ,'NA') as doexam,rg.Description as Result , rg.Code as Grade from " +
              " Module_Type mt,  module m , Result_Grading rg,Course_Exam_Application_Detail cad LEFT OUTER JOIN exam e on cad.Exam_ID =e.id " +
              " where cad.Module_ID =m.id and cad.Result_Grade_ID =rg.id and m.Module_Type_ID =mt.ID " +
              " and rg.Code <>'$'  and rg.Is_Passed = 1  and cad.Module_ID not in (933,934,935,936,956,957,958,959) and cad.Registration_Number =@registrationNumber and cad.Candidate_ID =@entityID and cad.Course_ID =@currentCourseID and m.id not in (select base_module_id from Course_Exam_Exemption where Registration_Number=@registrationNumber and Exempted_Course_ID= @currentCourseID and Candidate_ID=@entityID)" +
              " order by m.Module_Type_ID , m.Selection_Type_ID , m.ID ";
        }

        //string sql = "select cad.Course_ID CourseID, m.id as ID , upper(m.Name)  as name , m.Short_Name as Code , m.Module_Type_ID  as ModuleTypeID, " +
        //            " m.Selection_Type_ID as SelectionTypeID, m.Elective_Group as ElectiveGroup,mt.Name +  ( " +
        //            " CASE WHEN m.Module_Type_ID =1 and m.Selection_Type_ID =1 and Sub_Number =0 then '(Comp.)' " +
        //            " WHEN m.Module_Type_ID =1 and m.Selection_Type_ID >1 and Sub_Number >0 then '(Elect.)' else  '' END) as MType,  " +
        //            " isnull(e.Name ,'NA') as doexam,rg.Description as Result , rg.Code as Grade from " +
        //            " Module_Type mt,  module m , Result_Grading rg,Course_Exam_Application_Detail cad LEFT OUTER JOIN exam e on cad.Exam_ID =e.id " +
        //            " where cad.Module_ID =m.id and cad.Result_Grade_ID =rg.id and m.Module_Type_ID =mt.ID " +
        //            " and rg.Code <>'$'  and rg.Is_Passed = 1  and cad.Module_ID not in (933,934,935,936,956,957,958,959) and cad.Registration_Number ='" + registrationNumber + "' and cad.Candidate_ID ='" + entityID + "' and cad.Course_ID ='" + currentCourseID + "' " +
        //            " order by m.Module_Type_ID , m.Selection_Type_ID , m.ID ";



        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;

                //November_2024
                cmd.Parameters.AddWithValue("@registrationNumber", registrationNumber);
                cmd.Parameters.AddWithValue("@entityID", entityID);
                cmd.Parameters.AddWithValue("@currentCourseID", currentCourseID);
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt1);
                }
            }
        }

        return dt1;
    }


    protected Int32 GetRemainingAllTheoryModuleCount(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber, Int64 candidateID)
    {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 theorymodulecount = 0;
                Int32 max_revision = (from p in context.Modules
                                      where p.CourseID == currentCourseID
                                      select p.RevisionNumber).Distinct().Max();
                Int32 registrationRevisionNumber = EConnect.NIELIT.CourseManager.GetCourseRevisionNumberAtRegistrationCommenced(context, currentCourseID, registrationNumber, candidateID);
                Int32 theory = Convert.ToInt32(enmModuleType.Theory);
                Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
                Int32 allTheoryModules = GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Theory, null);
                Int32 allBridgeModules = GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Bridge, null);
                
                Int32 allTheoryModulesPassed = (from d in context.CourseExamApplicationDetails
                                                join m in context.Modules on d.ModuleID equals m.ID
                                                where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge)
                                                orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                select m).Count();

          

                Int32 moduleno_nine_ten_TheoryModulesPassed_inrevision4 = (from d in context.CourseExamApplicationDetails
                                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && (m.ID == 354 || m.ID == 355 || m.ID == 356 || m.ID == 29 || m.ID == 30 || m.ID == 31 || m.ID == 142 || m.ID == 144 || m.ID == 145)
                                                                           select m).Distinct().Count();
                if (currentCourseID == 1)
                {

                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == 1 && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 1 || m.ModuleTypeID == 5)
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();



                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 1 select m.ID).Distinct().ToList();
                        int TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).Count();

                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR2.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();

                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        //List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
                        var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        //List<int> PassedModuleInR52 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();
                        int countTotalPassedInOlevel = (TotalpasedinR6 + passedModuleinparityR5ToR6.Count());
                        theorymodulecount = ((allTheoryModules + allBridgeModules) - countTotalPassedInOlevel );
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (currentCourseID == 2)
                {
                    
                    try
                    {
                       
                            List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                               join m in context.Modules on d.ModuleID equals m.ID
                                                               where d.CourseID == 2 && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                               d.Grade.IsPassed == true && (m.ModuleTypeID == 1 || m.ModuleTypeID == 5)
                                                               orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                               select d.ModuleID).ToList();


                            //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                            //--List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
                            List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 2 select m.ID).Distinct().ToList();
                            int TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).Count();

                            List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 2 select m.ID).ToList();
                            List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                            List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR2.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();

                            List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 2 select m.ID).ToList();
                            List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                            List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();

                            List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                            List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 2 select m.ID).ToList();
                            List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                            List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();

                            var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                            List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 2 select m.ID).ToList();
                            List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                            List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                            List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
                            var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                            List<int> PassedModuleInR52 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();
                            int countTotalPassedInAlevel = (TotalpasedinR6 + PassedModuleInR52.Count());
                            theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel ));
                           // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                else if (currentCourseID == 3 && currentevisionNumber == 4)
                {

                    List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                       join m in context.Modules on d.ModuleID equals m.ID
                                       where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                       d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge)
                                       orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                       select d.ModuleID).ToList();

                    List<int> allModulR4= (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 3 select m.ID).ToList();
                    List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();

                    List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 3 select m.ID).ToList();
                    List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                    List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR1.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).ToList();
                   
                    List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 3 select m.ID).ToList();
                    List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                    List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();

                    List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).ToList();
                    List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 3 select m.ID).ToList();
                    List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                    List<int> TotalPassedModuleR2toR3 = passedModuleinparityR2ToR3.Concat(PassedModuleInR3).ToList();

                    var passedModuleinparityR3ToR4 = (from s in context.Parities where TotalPassedModuleR2toR3.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                    int totalpassedModuleinR3afterParitytoR4 = allModulR4.Intersect(passedModuleinparityR3ToR4).Count();

                    List<int> datafR3 = (from m in context.Modules where (m.ShortName == "B35-R3" || m.ShortName == "B43-R3" || m.ShortName == "BE9-R3" || m.ShortName == "BE2-R3" || m.ShortName == "BE10-R3" || m.ShortName == "BE1-R3" || m.ShortName == "BE3-R3" || m.ShortName == "BE8-R3" || m.ShortName == "BE4-R3" || m.ShortName == "BE5-R3") select m.ID).ToList();
                    int dlfR3 = PassedModuleInR3.Intersect(datafR3).Count();
                    int countConditionalpassedmoduleinBR3toBER4 = 0;
                    if (dlfR3 == 3)
                    {
                        countConditionalpassedmoduleinBR3toBER4 = 1;
                    }
                    if (dlfR3 == 4)
                    {
                        countConditionalpassedmoduleinBR3toBER4 = 2;
                    }


                    //List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 3 select m.ID).ToList();
                    //List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                    //var passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR2.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).ToList();
                    //List<int> totalpassedModuleinR2afterParitytoR3List = allModulR3.Intersect(passedModuleinparityR2ToR3).ToList();
                    //var passedModuleinparityR2ToR4 = (from s in context.Parities where totalpassedModuleinR2afterParitytoR3List.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).ToList();
                    //int totalpassedModuleinR2afterParitytoR4 = allModulR4.Intersect(passedModuleinparityR2ToR4).Count();
                    List<int> datafR2 = (from m in context.Modules where (m.ShortName == "B41" || m.ShortName == "B43" || m.ShortName == "B51" || m.ShortName == "BE4" || m.ShortName == "BE5" || m.ShortName == "BE6") select m.ID).ToList();
                    int dlfR2 = PassedModuleInR2.Intersect(datafR2).Count();
                    int countConditionalpassedmoduleinBR2toBER4 = 0;
                    if (dlfR2 == 3)
                    {
                        countConditionalpassedmoduleinBR2toBER4 = 1;
                    }
                    if (dlfR2 == 4)
                    {
                        countConditionalpassedmoduleinBR2toBER4 = 2;
                    }
                    if (dlfR2 == 5)
                    {
                        countConditionalpassedmoduleinBR2toBER4 = 3;
                    }

                    theorymodulecount = (allTheoryModules + allBridgeModules) - (PassedModuleInR4.Count() + totalpassedModuleinR3afterParitytoR4 - countConditionalpassedmoduleinBR3toBER4 - countConditionalpassedmoduleinBR2toBER4);
                    //--theorymodulecount = (allTheoryModules + allBridgeModules) - (PassedModuleInR4.Count() + totalpassedModuleinR3afterParitytoR4 - countConditionalpassedmoduleinBR3toBER4);
                }
                else if (currentCourseID == 3 && currentevisionNumber == 5)
                {
                    if (allTheoryModulesPassed == 0)
                        theorymodulecount = (allTheoryModules + allBridgeModules);
                    else
                    {
                        //Count Exempted Module From BR4 to BR5 
                        //--int countExeModule = CheckExemptioDoneOrNot(currentCourseID,registrationNumber,currentevisionNumber,candidateID);
                        //total module B-R4 omitted from B-R5
                        //--List<int> data1 = new List<int> { 390, 389, 388, 387, 386, 385, 384, 383, 382, 391, 392, };
                        List<int> OmmitedModuleR2toR4 = new List<int> { 390, 389, 388, 387, 386, 385, 384, 383, 382, 391, 392, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 208, 227, 209, 210, 211, 212, 213, 215, 229, 230, 231, 202, 170, 171, 172, 173, 196, 174, 198 };

                        //Total passed modules in B-R4 
                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge)
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                        //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                        List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 3 select m.ID).Distinct().ToList();
                        int TotalpasedinR5 = allModulR5.Intersect(passedTotalanyrevisonafterommiteR2toR4).Count();

                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR1.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).ToList();

                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList(); 
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR2toR3 = passedModuleinparityR2ToR3.Concat(PassedModuleInR3).ToList();

                        var passedModuleinparityR3ToR4 = (from s in context.Parities where TotalPassedModuleR2toR3.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR41 = PassedModuleInR4.Concat(passedModuleinparityR3ToR4).ToList();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where PassedModuleInR41.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> datafR3 = (from m in context.Modules where (m.ShortName == "B35-R3" || m.ShortName == "B43-R3" || m.ShortName == "BE9-R3" || m.ShortName == "BE2-R3" || m.ShortName == "BE10-R3" || m.ShortName == "BE1-R3" || m.ShortName == "BE3-R3" || m.ShortName == "BE8-R3" || m.ShortName == "BE4-R3" || m.ShortName == "BE5-R3") select m.ID).ToList();
                        int dlfR3 = PassedModuleInR3.Intersect(datafR3).Count();
                        int countConditionalpassedmoduleinBR2toBER4 = 0;
                        if (dlfR3 == 3)
                        {
                            countConditionalpassedmoduleinBR2toBER4 = 1;
                        }
                        if (dlfR3 == 4)
                        {
                            countConditionalpassedmoduleinBR2toBER4 = 2;
                        }

                        List<int> datafR2 = (from m in context.Modules where (m.ShortName == "B41" || m.ShortName == "B43" || m.ShortName == "B51" || m.ShortName == "BE4" || m.ShortName == "BE5" || m.ShortName == "BE6") select m.ID).ToList();
                        int dlfR2 = PassedModuleInR2.Intersect(datafR2).Count();
                        int countConditionalpassedmoduleinBR2toBER5 = 0;
                        if (dlfR2 == 3)
                        {
                            countConditionalpassedmoduleinBR2toBER5 = 1;
                        }
                        if (dlfR2 == 4)
                        {
                            countConditionalpassedmoduleinBR2toBER5 = 2;
                        }
                        if (dlfR2 == 5)
                        {
                            countConditionalpassedmoduleinBR2toBER5 = 3;
                        }

                        //--theorymodulecount = ((allTheoryModules + allBridgeModules) - (data5 + countExeModule + data51 + data52 + data53));
                        //--theorymodulecount = ((allTheoryModules + allBridgeModules) - (TotalPassedmoduelInR4 + countExeModule + totalpassedModuleinafterParityR3 - countConditionalpassedmoduleinBR2toBER4));
                        theorymodulecount = (allTheoryModules + allBridgeModules) - (TotalpasedinR5 + passedModuleinparityR4ToR5.Count() - countConditionalpassedmoduleinBR2toBER4 - countConditionalpassedmoduleinBR2toBER5);
                    }
                }
                else { 
                    //*****************Added on 09/05/2023 by Reena -for handling other than OABC************//
                    //string BlevelComplted=(from m in context.RegistrationDetails where m.RegistrationNo==registrationNumber && m.CourseID==3 select m.RegistrationStatusCode).FirstOrDefault();
                    //if (currentCourseID == 4 && BlevelComplted=="P")
                    //theorymodulecount = (allTheoryModules + allBridgeModules) - allTheoryModulesPassed - 4;
                    //else
                        theorymodulecount = (allTheoryModules + allBridgeModules) - allTheoryModulesPassed;

                } // ************************************************************************************************//
                return theorymodulecount;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    //protected void rbtn_module_selection_SelectedIndexChanged(object sender, EventArgs e)   // commented by abhi singh dated on 16042024
    //{

    //    if (rbtn_module_selection.SelectedValue == "pre" || rbtn_module_selection.SelectedValue == "current")
    //    {
    //        cleartextamount();
    //        using (EConnectContext context = new EConnectContext())
    //        {

    //            loginUserType = (UserType)Session["UserType"];
    //            entityID = Convert.ToInt64(Session["EntityID"]);
    //            currentCourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
    //            registrationNumber = Convert.ToInt64(Request.QueryString["RegNo"]);
    //            var registrationDetail = (from s in context.RegistrationDetails
    //                                      where s.CourseID == currentCourseID && s.CandidateID == entityID && s.RegistrationNo == registrationNumber
    //                                      select s).FirstOrDefault();
    //            applicantTypeID = registrationDetail.ApplicantTypeID;
    //            //currentevisionNumber = CourseManager.GetCourseRevisionNumberAtRegistrationCompleted(context, currentCourseID, registrationNumber, entityID);
    //            //preevisionNumber = currentevisionNumber - 1;
    //            Int32 application_status = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
    //            exceptional_exam_id = context.ExamCycleExceptionalFeatures.Where(s => s.whether_effective == "Y" && s.course_id == currentCourseID && System.Data.Entity.DbFunctions.TruncateTime(s.effective_upto_date) > System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)).Select(s => s.exam_id).Distinct().FirstOrDefault();
    //            //Exam lastExam_attempted_exceptionalcase = CourseManager.GeLastExam(context, currentCourseID, applicantTypeID);
    //            int candidate_attempted_last_exam = (from s in context.CourseExamApplicationDetails
    //                                                 join ee in context.Exams on s.ExamID equals ee.ID
    //                                                 join ce in context.CourseExamApplications on s.CourseExamApplicationID equals ce.ID
    //                                                 where s.CourseID == currentCourseID && s.RegistrationNumber == registrationNumber && s.CandidateID == entityID
    //                                                     //&& s.ExamID.HasValue == true
    //                                                 && ce.ApplicationStatusID == application_status
    //                                                 select ce.ExamID).DefaultIfEmpty(0).Distinct().Max();

    //            int revisionNumber_with_nochoice = (from s in context.CourseExamApplicationDetails
    //                                                join m in context.Modules on s.ModuleID equals m.ID
    //                                                where (s.CourseID == currentCourseID && s.CandidateID == entityID &&
    //                                                        s.RegistrationNumber == registrationNumber && s.ExamID == exceptional_exam_id)
    //                                                orderby m.RevisionNumber, m.Code
    //                                                select m.RevisionNumber).DefaultIfEmpty(0).Distinct().Max();
    //            //lastexam = lastExam_attempted_exceptionalcase == null ? 0 : lastExam_attempted_exceptionalcase.ID;
    //            candidate_lastexam = candidate_attempted_last_exam == 0 ? 0 : candidate_attempted_last_exam;

    //            Exam exam = CourseManager.GetNextExam(context, currentCourseID, applicantTypeID);
    //            Int32 max_revision = (from p in context.Modules
    //                                  where p.CourseID == currentCourseID
    //                                  select p.RevisionNumber).Distinct().Max();
    //            {
    //                currentevisionNumber = max_revision;
    //                preevisionNumber = currentevisionNumber - 1;
    //            }
             
    //            FillRemianingModules(currentCourseID, registrationNumber, entityID, applicantTypeID, exceptional_exam_id, candidate_lastexam, 0);

              
    //        }

    //    }
    //}
    
    void cleartextamount()
    {
        lblTheoryCount.Text = "0.00";
        lblPracticalCount.Text = "0.00";
        lblTotalTheoryFee.Text = "0.00";
        lblTotalPracticalFee.Text = "0.00";
        //lblTotalProcessingFee.Text="";
        lblTotalLateFee.Text = "0.00";
        lblTotalFee.Text = "0.00";
    }

    protected Int32 IsAlreadyApplied_inlastexam(EConnectContext context, Int32 currentExamID, Int32 courseID, Int64 registrationNumber, Int64 candidateID)//find revision number of non final submit-----02032020
    {
        try
        {
            Int32 application_status = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
            int revision_number = (from d in context.CourseExamApplicationDetails
                                   join c in context.CourseExamApplications
                                   on d.ExamID equals c.ExamID
                                   join m in context.Modules on d.ModuleID equals m.ID
                                   where (c.CourseID == courseID &&
                                           c.CandidateID == candidateID &&
                                           c.RegistrationNumber == d.RegistrationNumber &&
                                           c.RegistrationNumber == registrationNumber && c.ExamID == currentExamID
                                           && c.FinalSubmitted == true && c.ApplicationStatusID == application_status)
                                   select m.RevisionNumber).FirstOrDefault();
            if (revision_number > 0)
            {
                return revision_number;
            }
            else
                return 0;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // added by Abhi singh dated on 12042024 for find current revision number of candidate 
    public static Int32 GetCourseRevisionNumberAtRegistrationCompleted1(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int64 candidateID)
    {
        try
        {
            enmRegistrationStatus registrationStatus;
            Int32 currentevisionNumber = 0;
            RegistrationDetail regDetail = context.RegistrationDetails.Where(r => (r.CourseID == courseID && r.RegistrationNo == registrationNumber && r.CandidateID == candidateID)).FirstOrDefault();
            registrationStatus = regDetail.enmRegistrationStatus;
            if (registrationStatus == enmRegistrationStatus.Cancelled || registrationStatus == enmRegistrationStatus.Completed || registrationStatus == enmRegistrationStatus.Expired || regDetail.ValidUptoDate <= DateTime.Now.Date)
            {
                try
                {
                    int count = (from c in context.CourseExamApplicationDetails
                                 join m in context.Modules on c.ModuleID equals m.ID
                                 where c.CourseID == courseID && c.RegistrationNumber == registrationNumber && c.CandidateID == candidateID && m.ModuleTypeID != 3 && m.ModuleTypeID != 4
                                 select m.RevisionNumber).Count();
                    if (count != 0)
                    {
                        currentevisionNumber = (from c in context.CourseExamApplicationDetails
                                                join m in context.Modules on c.ModuleID equals m.ID
                                                where c.CourseID == courseID && c.RegistrationNumber == registrationNumber && c.CandidateID == candidateID && m.ModuleTypeID != 3 && m.ModuleTypeID != 4
                                                select m.RevisionNumber).Max();
                    }
                    else
                    {
                        currentevisionNumber = (from m in context.CourseRevisions where m.CourseID == courseID select m.RevisionNumber).Max();
                    }
                }
                catch (Exception) { }
            }
            else
            {
                int count = (from c in context.CourseExamApplicationDetails
                             join m in context.Modules on c.ModuleID equals m.ID
                             where c.CourseID == courseID && c.RegistrationNumber == registrationNumber && c.CandidateID == candidateID && m.ModuleTypeID != 3 && m.ModuleTypeID != 4
                             select m.RevisionNumber).Count();
                if (count != 0)
                {
                    currentevisionNumber = (from c in context.CourseExamApplicationDetails
                                            join m in context.Modules on c.ModuleID equals m.ID
                                            where c.CourseID == courseID && c.RegistrationNumber == registrationNumber && c.CandidateID == candidateID && m.ModuleTypeID != 3 && m.ModuleTypeID != 4
                                            select m.RevisionNumber).Max();
                }
                else
                {
                    currentevisionNumber = (from m in context.CourseRevisions where m.CourseID == courseID select m.RevisionNumber).Max();

                }
                //-- currentevisionNumber = CourseManager.GetCurrentCourseRevisionNumber(context, courseID);

            }
            return currentevisionNumber;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnCaneclModule_Click(object sender, EventArgs e)
    {
        //List<Exception> loopExceptions = new List<Exception>();
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                string module_id = string.Empty;
                int count = 0;
                foreach (ListItem item in chklistModulesOlevelSpecl.Items)
                {
                    if (item.Selected == true)
                    {
                        count++;
                        module_id=item.Value;
                    }
                }
                if (count > 1)
                {
                    throw new Exception("Please select only one module for Module Cancellation");
                }
                if (count==0)
                {
                    throw new Exception("Please select module for Cancellation");
                }
                string datenow = System.DateTime.Now.ToString("yyyy-MM-dd");
                Exam exam = CourseManager.GetNextExam(context, currentCourseID, Convert.ToInt32(ddlCandidateType.SelectedValue));
                string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                SqlConnection con = new SqlConnection(constr);
                
                //string sqlQuery = "Update Course_Exam_Application_Detail set Is_Canceled='1', Result_Grade_ID='7', Cancel_Remarks='Cancellation opted by candidate for R5.1', Canceled_On='" + datenow + "' where  Registration_Number='" + registrationNumber + "' and Module_ID='" + module_id + "' and Result_Grade_ID in (1,2,3,4,6)";

                //November_2024
                string sqlQuery = "Update Course_Exam_Application_Detail set Is_Canceled='1', Result_Grade_ID='7', Cancel_Remarks='Cancellation opted by candidate for R5.1', Canceled_On=@dateNow where  Registration_Number=@registrationNumber and Module_ID=@moduleId and Result_Grade_ID in (1,2,3,4,6)";
                con.Open();
                SqlCommand cmd1 = new SqlCommand(sqlQuery, con);

                //November_2024
                cmd1.Parameters.AddWithValue("@dateNow", datenow);
                cmd1.Parameters.AddWithValue("@registrationNumber", registrationNumber);
                cmd1.Parameters.AddWithValue("@moduleId", module_id);

                int rowEffect1 = cmd1.ExecuteNonQuery();
                if (rowEffect1 > 0)
                {
                    splOLevelCondition = 0;
                    lblserve1.Text = "2.1";
                    panelfeedetails.Visible = true;
                    disclmrPanel.Visible = true;
                    btnExmpSubmit.Visible = false;
                    btnSave.Visible = true;
                    FeedtlsId.Visible = true;
                    disclamrdivid.Visible = true;
                    trImprovementHead.Visible = true;
                    trModuleOptionExmpt.Visible = false;
                    trModuleOptionExmpt1.Visible = false;
                    trModuleOption.Visible = true;
                    trModuleOption1.Visible = true;
                    trModuleOptionOlevelSpecl.Visible=false;
                    trModuleOptionOlevelSpecl1.Visible=false;
                    lblRevision.Visible = false;
                    //frty.Visible = false;
                    TrOptionOlevelSpecl.Visible = false;
                    Btncancelmodule.Visible = false;
                    //cleartextamount();
                    //if ((rbtn_module_selection.SelectedValue == "pre" || rbtn_module_selection.SelectedValue == "current") && currentCourseID==3)
                    //   {
                    
                      

                            //if (IsSessionAlive() == false)
                            //    Response.Redirect("../Home.aspx");
                            Response.CacheControl = "no-cache";
                            Response.AddHeader("Progra", "no-cache");
                            Response.Expires = -1500;
                            Response.ExpiresAbsolute = DateTime.Now.AddDays(1);

                            //if (Request.UrlReferrer == null)
                            ////if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in") && (Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "https://nielit.gov.in"))
                            //{
                            //    Response.Write(GeInvalidRequestMessage("Goto Home Page", Server.MapPath("../Index.aspx")));
                            //    Response.End();
                            //    return;
                            //}

                            loginUserType = (UserType)Session["UserType"];
                            entityID = Convert.ToInt64(Session["EntityID"]);
                            examStartDate = Convert.ToDateTime(Request.QueryString["examStartDate"]);
                            //entityID = 1413670;

                            currentCourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                            //currentCourseID = 115;

                            registrationNumber = Convert.ToInt64(Request.QueryString["RegNo"]);
                            //  registrationNumber = 1393908;

                            if (currentCourseID == 1)
                            {
                                // Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL = CourseManager.Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL(registrationNumber);
                                if (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 0)
                                {
                                    chklistPracticals.Enabled = false;
                                }
                            }
                            else if (currentCourseID == 1213)
                            {
                                chklistPracticals.Enabled = false;
                            }
                            else if (currentCourseID == 2)
                            {
                                //Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL = CourseManager.Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL(registrationNumber);
                            }
                            else if (currentCourseID == 3)
                            {
                                Eligibility_Method_For_Apply_In_Old_Exam_Pattren_B_LVL = Eligibility_For_Apply_In_Old_Exam_Pattren_B_LVL(registrationNumber, currentCourseID, entityID, currentevisionNumber);
                            }

                            //Added to let candiadte to fill the exam form of July . should remove  from  line 70  to 87 

                            var CDate = (from c in context.RegistrationDetails
                                         where c.RegistrationNo == registrationNumber && c.CourseID == currentCourseID
                                         select new { CommencementFromDate = c.CommencementFromDate }).FirstOrDefault();

                            if (CDate.CommencementFromDate > examStartDate.AddDays(-(double)(examStartDate.Day - 1)) && currentCourseID != 1213)
                            {
                                lblerror.Visible = true;
                                lblerror.Text = " You are not allowed to fill the Examination Form. Please check your Commencement Date  or Registration Validity in Registration Detail.";
                                tblMain.Visible = false;
                                btnSave.Visible = false;
                                trModuleOption1.Visible = false;
                                trModuleOption.Visible = false;
                                trModuleHead.Visible = false;
                                tr_select_module.Visible = false;
                                rbtn_module_selection.Visible = false;
                                //ShowAlert("You are not allowed to fill the Examination Form");
                                return;
                            }
                            else
                            {

                                if (!CourseManager.IsCandidateDebarred(currentCourseID, registrationNumber))
                                {
                                    lblerror.Text = "You can not apply for this Exam because your candidature is debarred from appearing in subsequent two examinations of NIELIT.";
                                    lblerror.Visible = true;
                                    btnSave.Visible = false;
                                    trModuleOption1.Visible = false;
                                    trModuleOption.Visible = false;
                                    trModuleHead.Visible = false;
                                    tr_select_module.Visible = false;
                                    rbtn_module_selection.Visible = false;
                                    return;
                                }

                                if (!context.Candidates.Any(c => (c.IsLocked == true && c.ID == entityID)))
                                {
                                    lblerror.Text = "You can not apply for exam.<br>Your profile details are not completed/locked yet. Please first complete your profile details and lock it. <br> <a href=" + EConnect.Utils.Security.QuertStringModule.Encrypt("myprofile.aspx") + "> Click here to view profile</a>";
                                    lblerror.Visible = true;
                                    btnSave.Visible = false;
                                    tblMain.Visible = false;
                                    return;
                                }

                                ////-------Start-----------for fetching previous and curret revision number from database automatically----------------------------------------

                                var revision_text = (from s in context.RevisionChoices
                                                     where s.course_id == currentCourseID && s.whether_show_revision_choice == "Y" && s.show_revision_choice_till_date >= DateTime.Now
                                                     orderby (s.revision_choice_effective_date) descending
                                                     select new { previous_revision_number = s.previous_revision_number, new_revision_number = s.new_revision_number }).FirstOrDefault();


                                if (CourseManager.IsCourseReviesd(currentCourseID))
                                {
                                    //lblRevision.Text = currentevisionNumber.ToString() + "<sup>th</sup> Revision) ";
                                    //rbtn_module_selection.Items.FindByValue("pre").Text = "Revision " + revision_text.previous_revision_number.ToString();
                                    //rbtn_module_selection.Items.FindByValue("current").Text = "Revision " + revision_text.new_revision_number.ToString();

                                    rbtn_module_selection.Items.FindByValue("pre").Text = "Old Pattern";
                                    rbtn_module_selection.Items.FindByValue("current").Text = "New Pattern";
                                }
                                //------------------------------------------------------------------------------------------------------------------------------------------END

                                currentCourse = context.Courses.Find(currentCourseID);
                                Int32 ShortermCoursesCategory = context.CourseCategories.Where(s => s.Code.ToUpper() == "STC").FirstOrDefault().ID;
                                EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlCandidateType, typeof(enmApplicantType), null);
                                if (ddlCandidateType.Items[0].Value == Convert.ToInt32(enmApplicantType.Direct).ToString())
                                    ddlCandidateType.Items[0].Text = "As Direct Candidate";
                                if (ddlCandidateType.Items[1].Value == Convert.ToInt32(enmApplicantType.Institute).ToString())
                                    ddlCandidateType.Items[1].Text = "Through Accredited Institute";
                                lbllevel.Text = currentCourse.Name;
                                var registrationDetail = (from s in context.RegistrationDetails
                                                          where s.CourseID == currentCourseID && s.CandidateID == entityID && s.RegistrationNo == registrationNumber
                                                          select s).FirstOrDefault();
                                if (registrationDetail != null)
                                {
                                    //Declaration
                                    Candidate candidate = registrationDetail.Candidate;
                                    lblCandidateName.Text = candidate.Salutation + " " + GetInitCap(candidate.Name);
                                    lblRegNumber.Text = registrationNumber.ToString();

                                    lblName.Text = GetInitCap(candidate.Name);

                                    if (candidate.Gender == "Male")
                                        lblName.Text += " S/o ";
                                    else
                                        lblName.Text += " D/o ";
                                    if (!string.IsNullOrEmpty(candidate.FatherName))
                                        lblName.Text += GetInitCap(candidate.FatherName);
                                    else if (!string.IsNullOrEmpty(candidate.GuardianName))
                                        lblName.Text += GetInitCap(candidate.GuardianName);
                                    else
                                        lblName.Text += " NA ";
                                    if (registrationDetail.enmApplicantType == enmApplicantType.Direct)
                                        lblCandidateType.Text = " as a Direct candidate ";
                                    else
                                    {
                                        //Institute institute = registrationDetail.Institute;
                                        Institute institute = registrationDetail.Institute;
                                        if (institute != null)
                                        {
                                            lblCandidateType.Text = " through Accredited Institute namely " + institute.Name + " ACCR No. " + currentCourse.Code + "-" + institute.AccreditationDetails.Where(d => d.CourseID == currentCourseID).FirstOrDefault().AccreditationNumber;
                                            lblCandidateType1.Text = lblCandidateType.Text;
                                        }
                                    }

                                    //modification in declaration
                                    if (currentCourse.CourseCategoryID == ShortermCoursesCategory)
                                    {
                                        header1.Visible = false;
                                        header2.Visible = true;
                                    }
                                    else
                                    {
                                        header1.Visible = true;
                                        header2.Visible = false;
                                    }
                                    applicantTypeID = registrationDetail.ApplicantTypeID;
                                    //Exam exam = CourseManager.GetNextExam(context, currentCourseID, applicantTypeID);

                                    Int32 max_revision = (from p in context.Modules
                                                          where p.CourseID == currentCourseID
                                                          select p.RevisionNumber).Distinct().Max();
                                    //  calculate max Exam Pattern

                                    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<=============================For July2020 revision change====================================================
                                    Int32 application_status = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                                    exceptional_exam_id = context.ExamCycleExceptionalFeatures.Where(s => s.whether_effective == "Y" && s.course_id == currentCourseID && System.Data.Entity.DbFunctions.TruncateTime(s.effective_upto_date) > System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)).Select(s => s.exam_id).Distinct().FirstOrDefault();
                                    //Exam lastExam_attempted_exceptionalcase = CourseManager.GeLastExam(context, currentCourseID, applicantTypeID);
                                    int candidate_attempted_last_exam = (from s in context.CourseExamApplicationDetails
                                                                         join ee in context.Exams on s.ExamID equals ee.ID
                                                                         join ce in context.CourseExamApplications on s.CourseExamApplicationID equals ce.ID
                                                                         where s.CourseID == currentCourseID && s.RegistrationNumber == registrationNumber && s.CandidateID == entityID
                                                                             //&& s.ExamID.HasValue == true
                                                                         && ce.ApplicationStatusID == application_status
                                                                         select ce.ExamID).DefaultIfEmpty(0).Distinct().Max();

                                    int revisionNumber_with_nochoice = (from s in context.CourseExamApplicationDetails
                                                                        join m in context.Modules on s.ModuleID equals m.ID
                                                                        where (s.CourseID == currentCourseID && s.CandidateID == entityID &&
                                                                                s.RegistrationNumber == registrationNumber && s.ExamID == exceptional_exam_id)
                                                                        orderby m.RevisionNumber, m.Code
                                                                        select m.RevisionNumber).DefaultIfEmpty(0).Distinct().Max();
                                    //lastexam = lastExam_attempted_exceptionalcase == null ? 0 : lastExam_attempted_exceptionalcase.ID;
                                    candidate_lastexam = candidate_attempted_last_exam == 0 ? 0 : candidate_attempted_last_exam;

                                    //=====================================================================For July2020==========================>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                    if ((regStatusID.Contains(registrationDetail.RegistrationStatusID.Value) || (registrationDetail.RegistrationStatusID.Value == 4 && registrationDetail.CourseID==2)) && registrationDetail.ValidUptoDate >= DateTime.Now.Date)
                                    {
                                        //<<<<<<<<<<<<<<<<<<<<<<=============================For July2020 revision change=======================================
                                        if (exceptional_exam_id == candidate_lastexam && exceptional_exam_id != 0)
                                        {
                                            currentevisionNumber = revisionNumber_with_nochoice;
                                        }

                                        //=====================================================For July2020==========================>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                        else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == max_revision)
                                        //if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == max_revision)
                                        {
                                            currentevisionNumber = max_revision; ;
                                            preevisionNumber = currentevisionNumber - 1;
                                        }
                                        else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) != max_revision)
                                        {
                                            if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                            {
                                                if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0)
                                                {
                                                    currentevisionNumber = max_revision;
                                                    preevisionNumber = currentevisionNumber - 1;
                                                }
                                                else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                {
                                                    currentevisionNumber = CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID);
                                                    preevisionNumber = currentevisionNumber;
                                                }
                                            }
                                            else
                                            {
                                                currentevisionNumber = max_revision;
                                                preevisionNumber = currentevisionNumber - 1;
                                            }
                                        }
                                        else
                                        {
                                            currentevisionNumber = max_revision;
                                            preevisionNumber = currentevisionNumber - 1;
                                        }
                                        //Exam exam = CourseManager.GetNextExam(context, currentCourseID, applicantTypeID);
                                        if (exam != null)
                                        {
                                            if (context.ExamTimeTables.Any(t => t.CourseID == currentCourseID && t.ExamID == exam.ID))
                                            {
                                                lblExamName.Text = exam.Name.ToUpper();
                                                lblExamName1.Text = exam.Name.ToUpper();
                                                //lastExams = CourseManager.GetListOfAttemptedExams(context, currentCourseID, registrationNumber, entityID).Where(c => c.ID != exam.ID );
                                                lastExams = CourseManager.GetListOfTheoryPassed(context, currentCourseID, registrationNumber, entityID).Where(c => c.ID != exam.ID);
                                                attemptedLastExams = lastExams.OrderByDescending(d => new { d.ExamYear, d.ExamMonth }).FirstOrDefault();
                                                if (attemptedLastExams != null)
                                                    lblPreviousExam.Text = attemptedLastExams.Name.ToUpper();

                                                ddlCandidateType.SelectedValue = registrationDetail.ApplicantTypeID.ToString();
                                                if (registrationDetail.enmApplicantType == enmApplicantType.Direct)
                                                {
                                                    ddlCandidateType.Enabled = false;
                                                    ddlPaymentOption.SelectedValue = "1";
                                                    ddlPaymentOption.Enabled = false;
                                                }
                                                else
                                                {
                                                    ddlPaymentOption.SelectedValue = "2";
                                                    ddlPaymentOption.Enabled = true;
                                                    Int32 totalExamsAttempted = lastExams.Count();
                                                    if (currentCourseID == 1 || currentCourseID == 2)
                                                    {
                                                        if (totalExamsAttempted >= 2)
                                                            ddlCandidateType.Enabled = true;
                                                        else
                                                            ddlCandidateType.Enabled = false;
                                                    }
                                                    else
                                                    {
                                                        if (totalExamsAttempted >= 6)
                                                            ddlCandidateType.Enabled = true;
                                                        else
                                                            ddlCandidateType.Enabled = false;
                                                    }
                                                }

                                                //Exam Center

                                                if (registrationDetail.CourseCategoryID == 6)
                                                {
                                                    Institute institute = registrationDetail.Institute;

                                                   // SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
                                                  //  EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();

                                                    con.Open();

                                                    //using (SqlCommand cmd = new SqlCommand("select  id, name from Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = '" + institute.StateID + "'))"))
                                                    
                                                    //November_2024
                                                    using (SqlCommand cmd = new SqlCommand("select  id, name from Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = @instituteStateID))"))
                                                    {
                                                        using (SqlDataAdapter sda = new SqlDataAdapter())
                                                        {
                                                            cmd.Connection = con;
                                                            sda.SelectCommand = cmd;
                                                            //November_2024
                                                            cmd.Parameters.AddWithValue("@instituteStateID", institute.StateID);
                                                            using (DataTable dTable = new DataTable())
                                                            {
                                                                sda.Fill(dTable);

                                                                //ddlStateFirst.DataSource = dTable;
                                                                //ddlStateFirst.DataBind();

                                                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlStateFirst, dTable, new ListItem("--Select Location--", "0"));
                                                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlStateSecond, dTable, new ListItem("--Select Location--", "0"));

                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    var examcentre = (from p in context.ExamCenters
                                                                      join c in context.ExamWiseExamCenters on p.ID equals c.ExamCenterID
                                                                      join s in context.Locations on p.StateID equals s.ID
                                                                      where (p.CourseCategoryID == currentCourse.CourseCategoryID || p.CourseID == currentCourseID)
                                                                      && c.ExamID == exam.ID
                                                                      select new { ValueField = s.ID, TextField = s.Name.ToUpper() }).Distinct().OrderBy(s => s.TextField);
                                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlStateFirst, examcentre, new ListItem("--Select Location--", "0"));
                                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlStateSecond, examcentre, new ListItem("--Select Location--", "0"));

                                                }
                                                #region[Prac_Center Added By Reena]

                                                //Exam Center

                                                if (registrationDetail.CourseCategoryID == 6)
                                                {
                                                    Institute institute = registrationDetail.Institute;

                                                   // SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
                                                   // EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();

                                                    con.Open();

                                                    //using (SqlCommand cmd = new SqlCommand("select  id, name from Prac_Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = '" + institute.StateID + "'))"))

                                                    //November_2024
                                                    using (SqlCommand cmd = new SqlCommand("select  id, name from Prac_Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = @instituteStateID ))"))
                                                    {
                                                        using (SqlDataAdapter sda = new SqlDataAdapter())
                                                        {
                                                            cmd.Connection = con;
                                                            //November_2024
                                                            cmd.Parameters.AddWithValue("@instituteStateID", institute.StateID);
                                                            sda.SelectCommand = cmd;
                                                            using (DataTable dTable = new DataTable())
                                                            {
                                                                sda.Fill(dTable);

                                                                //ddlStateFirst.DataSource = dTable;
                                                                //ddlStateFirst.DataBind();


                                                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlPracStateFirst, dTable, new ListItem("--Select Location--", "0"));
                                                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlPracStateSecond, dTable, new ListItem("--Select Location--", "0"));
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    var examcentre = (from p in context.PracExamCenters
                                                                      //join c in context.ExamWiseExamCenters on p.ID equals c.ExamCenterID
                                                                      join s in context.PracLocations on p.StateID equals s.ID
                                                                      where (p.CourseCategoryID == currentCourse.CourseCategoryID)
                                                                      //&& c.ExamID == exam.ID
                                                                      select new { ValueField = s.ID, TextField = s.Name.ToUpper() }).Distinct().OrderBy(s => s.TextField);

                                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlPracStateFirst, examcentre, new ListItem("--Select Location--", "0"));
                                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlPracStateSecond, examcentre, new ListItem("--Select Location--", "0"));
                                                }
                                                #endregion

                                                #region[Online_Theory_Center Added By Reena]

                                                //Exam Center

                                                if (registrationDetail.CourseCategoryID == 6)
                                                {
                                                    Institute institute = registrationDetail.Institute;

                                                   // SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
                                                   // EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();

                                                    con.Open();

                                                    //using (SqlCommand cmd = new SqlCommand("select  id, name from Online_Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = '" + institute.StateID + "'))"))
                                                    
                                                    //November_2024
                                                    using (SqlCommand cmd = new SqlCommand("select  id, name from Online_Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = @instituteStateID ))"))
                                                    {
                                                        using (SqlDataAdapter sda = new SqlDataAdapter())
                                                        {
                                                            cmd.Connection = con;
                                                            //November_2024
                                                            cmd.Parameters.AddWithValue("@instituteStateID", institute.StateID);
                                                            sda.SelectCommand = cmd;
                                                            using (DataTable dTable = new DataTable())
                                                            {
                                                                sda.Fill(dTable);

                                                                //ddlStateFirst.DataSource = dTable;
                                                                //ddlStateFirst.DataBind();  


                                                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlOnlTheoryExamStateFirst, dTable, new ListItem("--Select Location--", "0"));
                                                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlOnlTheoryExamStateSecond, dTable, new ListItem("--Select Location--", "0"));
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    var examcentre = (from p in context.OnlineExamCenters
                                                                      //join c in context.ExamWiseExamCenters on p.ID equals c.ExamCenterID
                                                                      join s in context.OnlineLocations on p.StateID equals s.ID
                                                                      where (p.CourseCategoryID == currentCourse.CourseCategoryID)
                                                                      //&& c.ExamID == exam.ID
                                                                      select new { ValueField = s.ID, TextField = s.Name.ToUpper() }).Distinct().OrderBy(s => s.TextField);

                                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlOnlTheoryExamStateFirst, examcentre, new ListItem("--Select Location--", "0"));//Reena
                                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlOnlTheoryExamStateSecond, examcentre, new ListItem("--Select Location--", "0")); //Reena
                                                }
                                                #endregion

                                                //Language option
                                                CourseRegistrationPolicy currentPolicy = CourseManager.GetCurrentRegistrationPolicy(context, currentCourseID);
                                                if (currentPolicy.AllowedLanguage.HasValue)
                                                {
                                                    rblMedium.SelectedValue = currentPolicy.AllowedLanguage.Value.ToString();
                                                    rblMedium.Enabled = false;
                                                }

                                                //Fee Details
                                                lblTheoryFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee).ToString("F");
                                                lblPracticalFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee).ToString("F");
                                                lblLateFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.LateFeeExam).ToString("F");
                                                lblProcessingFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PostageFeeChargedTowardExaminationCorrespondence).ToString("F");
                                                lblTotalProcessingFee.Text = lblProcessingFee.Text;
                                                if (CourseManager.IsLateFeeApplicable(context, exam.ID, applicantTypeID))
                                                    lblTotalLateFee.Text = lblLateFee.Text;

                                                FillRemianingModules(currentCourseID, registrationNumber, entityID, applicantTypeID, exceptional_exam_id, candidate_lastexam, 0);

                                                //<<<<<<<<<<<<<<<<<<<<<<=============================For July2020 revision change=======================================
                                                if (exceptional_exam_id == candidate_lastexam && exceptional_exam_id != 0)
                                                {
                                                    currentevisionNumber = revisionNumber_with_nochoice;
                                                    tr_select_module.Visible = false;
                                                    rbtn_module_selection.Visible = false;
                                                }

                                                //=====================================================For July2020==========================>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                                else if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                //if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                {
                                                    if (CourseManager.IsCourseReviesd(currentCourseID))
                                                    {
                                                        tr_select_module.Visible = true;
                                                        rbtn_module_selection.Visible = true;

                                                        if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0 ||
                                                            IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == 0)
                                                        {
                                                            //tr_select_module.Visible = false;//*****
                                                            trModuleOption.Visible = false;
                                                            trModuleHead.Visible = false;
                                                            trModuleOption1.Visible = false;
                                                        }
                                                        else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                            CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                        {
                                                            //tr_select_module.Visible = false;//*****
                                                            trModuleOption.Visible = true;
                                                            trModuleHead.Visible = true;
                                                            trModuleOption1.Visible = true;
                                                        }
                                                        else if (IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                                IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                        {
                                                            //tr_select_module.Visible = false;//*****
                                                            trModuleOption.Visible = true;
                                                            trModuleHead.Visible = true;
                                                            trModuleOption1.Visible = true;
                                                        }
                                                        else
                                                        {
                                                            //tr_select_module.Visible = false;//*****
                                                            trModuleOption.Visible = false;
                                                            trModuleHead.Visible = false;
                                                            trModuleOption1.Visible = false;
                                                            if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0)
                                                            {
                                                                rbtn_module_selection.SelectedIndex = -1;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (lblerror.Text == "You can not apply for exam as all theory/bridge/practical modules have been passed."
                                                            || lblerror.Text == "You have already applied for this exam.")
                                                        {
                                                            tr_select_module.Visible = false;
                                                            rbtn_module_selection.Visible = false;
                                                            trModuleOption.Visible = false;
                                                            trModuleHead.Visible = false;
                                                            trModuleOption1.Visible = false;
                                                        }
                                                        else
                                                        {
                                                            tr_select_module.Visible = false;
                                                            rbtn_module_selection.Visible = false;
                                                            trModuleOption.Visible = true;
                                                            trModuleHead.Visible = true;
                                                            trModuleOption1.Visible = true;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    //commented by me as on 02022023
                                                    //if ((currentCourseID == 1 || currentCourseID == 2 || currentCourseID == 3) && (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 1 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL == 1 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_B_LVL == 1))
                                                    if ((currentCourseID == 1 && Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 1) || (currentCourseID == 2 && Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL == 1) || (currentCourseID == 3 && Eligibility_Method_For_Apply_In_Old_Exam_Pattren_B_LVL == 1))
                                                    //--if ((currentCourseID == 1 || currentCourseID == 2 || currentCourseID == 3))
                                                    {
                                                        tr_select_module.Visible = true;//*****
                                                        rbtn_module_selection.Visible = true;
                                                    }
                                                    else
                                                    {
                                                        tr_select_module.Visible = false;
                                                        rbtn_module_selection.Visible = false;
                                                    }
                                                    trModuleOption.Visible = true;
                                                    trModuleHead.Visible = true;
                                                    trModuleOption1.Visible = true;
                                                }

                                                if (context.CourseExamApplications.Any(c => (c.ExamID == exam.ID && c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber)))
                                                {
                                                    if (Request.QueryString["Appid"] == null)
                                                    {
                                                        lblerror.Text = "You have already applied for this exam but not submitted finally.<br>Please change your exam details (if you want to change ) and click on Update button to view the preview. ";
                                                        chkdisclamier.Checked = true;
                                                        //<<<<<<<<<<<<<<<<<<<<<<=============================For July2020 revision change=======================================
                                                        if (exceptional_exam_id == candidate_lastexam && exceptional_exam_id != 0)
                                                        {
                                                            //currentevisionNumber = revisionNumber_jul20;
                                                            tr_select_module.Visible = false;
                                                            rbtn_module_selection.Visible = false;
                                                        }
                                                        //=====================================================For July2020==========================>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                                        else if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                        //if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                        {
                                                            if (CourseManager.IsCourseReviesd(currentCourseID))
                                                            {
                                                                tr_select_module.Visible = true;//*****
                                                                rbtn_module_selection.Visible = true;

                                                                if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0 ||
                                                                    IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == 0)
                                                                {
                                                                    //tr_select_module.Visible = false;//*****
                                                                    trModuleOption.Visible = false;
                                                                    trModuleHead.Visible = false;
                                                                    trModuleOption1.Visible = false;
                                                                }
                                                                else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                                    CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                                {
                                                                    //tr_select_module.Visible = false;//*****
                                                                    trModuleOption.Visible = true;
                                                                    trModuleHead.Visible = true;
                                                                    trModuleOption1.Visible = true;
                                                                }
                                                                else if (IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                                        IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                                {
                                                                    //tr_select_module.Visible = false;//*****
                                                                    trModuleOption.Visible = true;
                                                                    trModuleHead.Visible = true;
                                                                    trModuleOption1.Visible = true;
                                                                }
                                                                else
                                                                {
                                                                    //tr_select_module.Visible = false;//*****
                                                                    trModuleOption.Visible = false;
                                                                    trModuleHead.Visible = false;
                                                                    trModuleOption1.Visible = false;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                tr_select_module.Visible = false;
                                                                rbtn_module_selection.Visible = false;
                                                                trModuleOption.Visible = true;
                                                                trModuleHead.Visible = true;
                                                                trModuleOption1.Visible = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            tr_select_module.Visible = false;
                                                            rbtn_module_selection.Visible = false;
                                                            trModuleOption.Visible = true;
                                                            trModuleHead.Visible = true;
                                                            trModuleOption1.Visible = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        btnback.Visible = false;
                                                        lblerror.Text = "You can change your exam details. Please click on Update button after making changes.";
                                                        //<<<<<<<<<<<<<<<<<<<<<<=============================For July2020 revision change=======================================
                                                        if (exceptional_exam_id == candidate_lastexam && exceptional_exam_id != 0)
                                                        {
                                                            //currentevisionNumber = revisionNumber_jul20;
                                                            tr_select_module.Visible = false;
                                                            rbtn_module_selection.Visible = false;
                                                        }
                                                        //=====================================================For July2020==========================>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                                        else if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                        //if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                        {
                                                            if (CourseManager.IsCourseReviesd(currentCourseID))
                                                            {
                                                                tr_select_module.Visible = true;//*****
                                                                rbtn_module_selection.Visible = true;

                                                                if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0 ||
                                                                    IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == 0)
                                                                {
                                                                    //tr_select_module.Visible = false;//*****
                                                                    trModuleOption.Visible = false;
                                                                    trModuleHead.Visible = false;
                                                                    trModuleOption1.Visible = false;
                                                                }
                                                                else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                                     CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                                {
                                                                    //tr_select_module.Visible = false;//*****
                                                                    trModuleOption.Visible = true;
                                                                    trModuleHead.Visible = true;
                                                                    trModuleOption1.Visible = true;
                                                                }
                                                                else if (IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                                        IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                                {
                                                                    //tr_select_module.Visible = false;//*****
                                                                    trModuleOption.Visible = true;
                                                                    trModuleHead.Visible = true;
                                                                    trModuleOption1.Visible = true;
                                                                }
                                                                else
                                                                {
                                                                    //tr_select_module.Visible = false;//*****
                                                                    trModuleOption.Visible = false;
                                                                    trModuleHead.Visible = false;
                                                                    trModuleOption1.Visible = false;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                tr_select_module.Visible = false;
                                                                rbtn_module_selection.Visible = false;
                                                                trModuleOption.Visible = true;
                                                                trModuleHead.Visible = true;
                                                                trModuleOption1.Visible = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            tr_select_module.Visible = false;
                                                            rbtn_module_selection.Visible = false;
                                                            trModuleOption.Visible = true;
                                                            trModuleHead.Visible = true;
                                                            trModuleOption1.Visible = true;
                                                        }
                                                    }
                                                    lblerror.Visible = true;
                                                    Int64 applId = context.CourseExamApplications.Where(c => (c.ExamID == exam.ID && c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber)).FirstOrDefault().ID;
                                                    ShowApplicationData(applId);
                                                }
                                            }
                                            else
                                            {
                                                lblerror.Text = "Time table not found for " + exam.Name + " exam. You can not apply for exam at this time.";
                                                lblerror.Visible = true;
                                                btnSave.Visible = false;
                                            }
                                        }
                                        else
                                        {
                                            lblerror.Text = "No next exam found. You can not apply for exam at this time.";
                                            lblerror.Visible = true;
                                            btnSave.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        lblerror.Text = "You are not eligible to apply for exam as your course status is completed/expired/project pending/cancelled.";
                                        lblerror.Visible = true;
                                        btnSave.Visible = false;
                                        tblMain.Visible = false;
                                    }
                                }
                                else
                                {
                                    lblerror.Text = "You are not eligible to apply for exam this time.";
                                    lblerror.Visible = true;
                                    btnSave.Visible = false;
                                }
                            };

                            //if (splOLevelCondition == 1)
                            //{
                            //    lblserveExpt.Text = "2.1";
                            //    panelfeedetails.Visible = false;
                            //    disclmrPanel.Visible = false;
                            //    btnSave.Visible = false;
                            //    btnExmpSubmit.Visible = false;
                            //    FeedtlsId.Visible = false;
                            //    disclamrdivid.Visible = false;
                            //    trModuleOptionExmpt.Visible = false;
                            //    trModuleOptionExmpt1.Visible = false;
                            //    TrExamCentre1.Visible = false;
                            //    TrExamCentre2.Visible = false;
                            //    trImprovementHead.Visible = false;
                            //    trModuleOption.Visible = false;
                            //    trModuleOption1.Visible = false;
                            //    trModuleOptionOlevelSpecl.Visible = true;
                            //    trModuleOptionOlevelSpecl1.Visible = true;
                            //    TrOptionOlevelSpecl.Visible = true;
                            //}

                 
                }
            }
        }
        catch(Exception ex)
        {
          //  loopExceptions.Add(ex);
            TrExamCentre1.Visible = false;
            TrExamCentre2.Visible = false;
            ShowAlert(ex.Message);
        }
    }

    protected void btnExmpSubmit_Click(object sender, EventArgs e)
    {
        //btnExmpSubmit.Visible = false;
        List<Exception> loopExceptions = new List<Exception>();
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
              
                Int32 selectedModuleID = 0;
                int successcount = 1;
                List<Int32> PassesdConditionalTheoryModules_B = (from d in context.CourseExamApplicationDetails
                                                                 join m in context.Modules on d.ModuleID equals m.ID
                                                                 where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                                                 d.Grade.IsPassed == true && (m.ModuleTypeID == 1 || m.ModuleTypeID == 5) && (m.ID == 402 || m.ID == 404 || m.ID == 415 || m.ID == 417 || m.ID == 407 || m.ID == 54 || m.ID == 58 || m.ID == 66 || m.ID == 222 || m.ID == 225)
                                                                 orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                                 select d.ModuleID).ToList();

                List<Int32> PassesdConditionalTheoryModules_A = (from d in context.CourseExamApplicationDetails
                                                                 join m in context.Modules on d.ModuleID equals m.ID
                                                                 where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                                                 d.Grade.IsPassed == true && (m.ModuleTypeID == 1 || m.ModuleTypeID == 5) && (m.ID == 354 || m.ID == 355 || m.ID == 356 || m.ID == 29 || m.ID == 30 || m.ID == 31 || m.ID == 142 || m.ID == 144 || m.ID == 145)
                                                                 orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                                 select d.ModuleID).ToList();

               currentevisionNumber = CourseManager.GetCourseRevisionNumberAtRegistrationCompleted(context, currentCourseID, registrationNumber, entityID);
              int selectedModuleCount2 = 0;
             
               foreach (ListItem item in chklistModulesExmpt.Items)
               {
                   if (item.Selected == true)
                   {
                       selectedModuleCount2++;
                   }
               }
             
                int count = 0;
                int count1 = 0;
                //int count1 = PassesdConditionalTheoryModules_B.Count();
                if (currentCourseID==3)
                 count1 =Convert.ToInt16( lblCoutMessageExmpthidden.Value);
                int count2 = PassedConditionalTheoryModuleCount_Alevel(currentCourseID,registrationNumber,currentevisionNumber,entityID);
                // int count1 = PassedConditionalTheoryModuleCount(currentCourseID,registrationNumber,currentevisionNumber,entityID);
                if (selectedModuleCount2 != count1 && currentCourseID==3)
                {
                    //Console.WriteLine("<script>alert('You Can Not Proceed Exemption Process Because You Not Selected Complete Exemption Module '" + count1 + "' ')</script>");
                    //return;
                    try
                    {
                        btnSave.Visible = false;
                       // btnExmpSubmit.Visible = true;
                        trModuleOption.Visible = false;
                        trModuleOption1.Visible = false;
                       throw new Exception("Please select the '" + count1 + "' module(s) to avail exemption as it is mandatory to proceed further");
                    }
                    catch (Exception ex)
                    {
                       // loopExceptions.Add(ex);

                        ShowAlert(ex.Message);
                    }

                }
                else if (selectedModuleCount2 != count2 && currentCourseID == 2)
                {
                    try
                    {
                        btnSave.Visible = false;
                        trModuleOption.Visible = false;
                        trModuleOption1.Visible = false;
                        //btnExmpSubmit.Visible = true;
                        throw new Exception("Please select the '" + count2 + "' module(s) to avail exemption as it is mandatory to proceed further");
                    }
                    catch (Exception ex)
                    {
                        // loopExceptions.Add(ex);
                        ShowAlert(ex.Message);
                    }
                }
                else
                {
                   
                    foreach (ListItem item in chklistModulesExmpt.Items)
                    {
                        if (item.Selected == true)
                        {
                            Exam exam = CourseManager.GetNextExam(context, currentCourseID, Convert.ToInt32(ddlCandidateType.SelectedValue));
                            selectedModuleID = Convert.ToInt32(item.Value);
                            Module selectedModule = context.Modules.Find(selectedModuleID);

                            //string NewModuleName = selectedModule.ShortName + " " + selectedModule.Name;
                            int? ModuleCode = selectedModule.Code;
                            int moduleid = selectedModule.ID;
                            Int32 Exam_ID = exam.ID;
                            Int32 Course_Category_ID = 1;
                            Int64 Candidate_ID = entityID;
                            Int64 Registration_Number = registrationNumber;
                            Int32 Base_Course_ID = currentCourseID;
                            int Base_Revision_Number = (currentevisionNumber - 1);
                            Int32 Base_Module_ID = count1 != 0 ? PassesdConditionalTheoryModules_B[count] : PassesdConditionalTheoryModules_A[count];
                            Int32 Exempted_Course_ID = currentCourseID;
                            int Exempted_Revision_Number = currentevisionNumber;
                            int Exempted_Module_ID = moduleid;
                            DateTime Exempted_On = System.DateTime.Now;
                            Int64 IdGradeY = (from d in context.ResultGrades where d.Code == "Y" select d.ID).FirstOrDefault();
                            //Int64 Id = (maxId + 1);
                            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                            SqlConnection con = new SqlConnection(constr);
                            //SqlConnection con = new SqlConnection("Data Source=10.246.112.177;Initial Catalog=NIELIT;Persist Security Info=True;User ID=shaukat; Password=Db4PareekshaUAT@9211$; MultipleActiveResultSets=True;Timeout = 120; Max Pool Size=1000");

                            //string sqlQuery = "INSERT INTO Course_Exam_Exemption(Course_Category_ID ,Candidate_ID ,Registration_Number ,Base_Course_ID ,Base_Revision_Number ,Base_Module_ID,Exempted_Course_ID,Exempted_Revision_Number,Exempted_Module_ID,Exam_ID,Exempted_On) VALUES ('" + Course_Category_ID + "','" + Candidate_ID + "','" + Registration_Number + "','" + Base_Course_ID + "','" + Base_Revision_Number + "','" + Base_Module_ID + "','" + Exempted_Course_ID + "','" + Exempted_Revision_Number + "','" + Exempted_Module_ID + "','" + Exam_ID + "','" + Exempted_On + "')";

                            //Novermber_2024
                            string sqlQuery = "INSERT INTO Course_Exam_Exemption(Course_Category_ID ,Candidate_ID ,Registration_Number ,Base_Course_ID ,Base_Revision_Number ,Base_Module_ID,Exempted_Course_ID,Exempted_Revision_Number,Exempted_Module_ID,Exam_ID,Exempted_On) VALUES (@courseCategoryId,@candidateID,@registrationNumber,@baseCourseID,@baseRevisionNumber,@baseModuleID,@exemptedCourseID,@exemptedRevisionNumber,@exemptedModuleID,@examID,@exemptedOn)";

                            //string sqlQuery1 = "INSERT INTO Course_Exam_Application_Detail(Candidate_ID ,Registration_Number ,Module_Id,Fee_Amount,Is_UMC,Result_Grade_Id,Is_Canceled, Module_Ccde,Exam_Month,Exam_Year, Course_Id, Exam_Id,Absent_flag) VALUES ('" + Candidate_ID + "','" + Registration_Number + "','" + moduleid + "','0','0','" + IdGradeY + "','0','" + ModuleCode + "','" + exam.ExamMonth + "','" + exam.ExamYear + "','" + currentCourseID + "','" + exam.ID + "','N')";

                            //Novermber_2024
                            string sqlQuery1 = "INSERT INTO Course_Exam_Application_Detail(Candidate_ID ,Registration_Number ,Module_Id,Fee_Amount,Is_UMC,Result_Grade_Id,Is_Canceled, Module_Ccde,Exam_Month,Exam_Year, Course_Id, Exam_Id,Absent_flag) VALUES (@candidateID,@registrationNumber,@moduleId,'0','0',@idGradeY,'0',@moduleCode,@examMonth,@examYear,@currentCourseID,@examID,'N')";

                            con.Open();
                            SqlCommand cmd1 = new SqlCommand(sqlQuery, con);
                            
                            //Novermber_2024
                            cmd1.Parameters.AddWithValue("@courseCategoryId", Course_Category_ID);
                            cmd1.Parameters.AddWithValue("@candidateID", Candidate_ID);
                            cmd1.Parameters.AddWithValue("@registrationNumber", Registration_Number);
                            cmd1.Parameters.AddWithValue("@baseCourseID", Base_Course_ID);
                            cmd1.Parameters.AddWithValue("@baseRevisionNumber", Base_Revision_Number);
                            cmd1.Parameters.AddWithValue("@baseModuleID", Base_Module_ID);
                            cmd1.Parameters.AddWithValue("@exemptedCourseID", Exempted_Course_ID);
                            cmd1.Parameters.AddWithValue("@exemptedRevisionNumber", Exempted_Revision_Number);
                            cmd1.Parameters.AddWithValue("@exemptedModuleID", Exempted_Module_ID);
                            cmd1.Parameters.AddWithValue("@examID", Exam_ID);
                            cmd1.Parameters.AddWithValue("@exemptedOn", Exempted_On);
                            int rowEffect1 = cmd1.ExecuteNonQuery();

                            SqlCommand cmd2 = new SqlCommand(sqlQuery1, con);

                            //Novermber_2024
                            cmd2.Parameters.AddWithValue("@candidateID", Candidate_ID);
                            cmd2.Parameters.AddWithValue("@registrationNumber", Registration_Number);
                            cmd2.Parameters.AddWithValue("@moduleId", moduleid);
                            cmd2.Parameters.AddWithValue("@idGradeY", IdGradeY);
                            cmd2.Parameters.AddWithValue("@moduleCode", ModuleCode);
                            cmd2.Parameters.AddWithValue("@examMonth", exam.ExamMonth);
                            cmd2.Parameters.AddWithValue("@examYear", exam.ExamYear);
                            cmd2.Parameters.AddWithValue("@currentCourseID", currentCourseID);
                            cmd2.Parameters.AddWithValue("@examID", exam.ID);

                            int rowEffect2 = cmd2.ExecuteNonQuery();
                            if (rowEffect1 > 0 && rowEffect2 > 0)
                            {
                                successcount++;
                            }
                            count++;
                        }

                    }
                }
               
               
                if (successcount > 1)
                {
                    //--Console.WriteLine("<script>alert('Exemption Process has been Completed and Now your Exam Window open for submit Exam Application')</script>");
                    lblserve1.Text = "2.1";
                    panelfeedetails.Visible = true;
                    disclmrPanel.Visible = true;
                    btnExmpSubmit.Visible = false;
                    btnSave.Visible = true;
                    FeedtlsId.Visible = true;
                    disclamrdivid.Visible = true;
                    trImprovementHead.Visible = true;
                    trModuleOptionExmpt.Visible = false;
                    trModuleOptionExmpt1.Visible = false;
                    trModuleOption.Visible = true;
                    trModuleOption1.Visible = true;
                    //cleartextamount();
                     //if ((rbtn_module_selection.SelectedValue == "pre" || rbtn_module_selection.SelectedValue == "current") && currentCourseID==3)
                     //   {
                    if (currentCourseID == 3)
                     {        
                    cleartextamount();
                            loginUserType = (UserType)Session["UserType"];
                                entityID = Convert.ToInt64(Session["EntityID"]);
                                currentCourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                                registrationNumber = Convert.ToInt64(Request.QueryString["RegNo"]);
                                var registrationDetail = (from s in context.RegistrationDetails
                                                          where s.CourseID == currentCourseID && s.CandidateID == entityID && s.RegistrationNo == registrationNumber
                                                          select s).FirstOrDefault();
                                applicantTypeID = registrationDetail.ApplicantTypeID;
                                //currentevisionNumber = CourseManager.GetCourseRevisionNumberAtRegistrationCompleted(context, currentCourseID, registrationNumber, entityID);
                                //preevisionNumber = currentevisionNumber - 1;
                                Int32 application_status = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                                exceptional_exam_id = context.ExamCycleExceptionalFeatures.Where(s => s.whether_effective == "Y" && s.course_id == currentCourseID && System.Data.Entity.DbFunctions.TruncateTime(s.effective_upto_date) > System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)).Select(s => s.exam_id).Distinct().FirstOrDefault();
                                //Exam lastExam_attempted_exceptionalcase = CourseManager.GeLastExam(context, currentCourseID, applicantTypeID);
                                int candidate_attempted_last_exam = (from s in context.CourseExamApplicationDetails
                                                                     join ee in context.Exams on s.ExamID equals ee.ID
                                                                     join ce in context.CourseExamApplications on s.CourseExamApplicationID equals ce.ID
                                                                     where s.CourseID == currentCourseID && s.RegistrationNumber == registrationNumber && s.CandidateID == entityID
                                                                         //&& s.ExamID.HasValue == true
                                                                     && ce.ApplicationStatusID == application_status
                                                                     select ce.ExamID).DefaultIfEmpty(0).Distinct().Max();

                                int revisionNumber_with_nochoice = (from s in context.CourseExamApplicationDetails
                                                                    join m in context.Modules on s.ModuleID equals m.ID
                                                                    where (s.CourseID == currentCourseID && s.CandidateID == entityID &&
                                                                            s.RegistrationNumber == registrationNumber && s.ExamID == exceptional_exam_id)
                                                                    orderby m.RevisionNumber, m.Code
                                                                    select m.RevisionNumber).DefaultIfEmpty(0).Distinct().Max();
                                //lastexam = lastExam_attempted_exceptionalcase == null ? 0 : lastExam_attempted_exceptionalcase.ID;
                                candidate_lastexam = candidate_attempted_last_exam == 0 ? 0 : candidate_attempted_last_exam;

                                Exam exam = CourseManager.GetNextExam(context, currentCourseID, applicantTypeID);
                                Int32 max_revision = (from p in context.Modules
                                                      where p.CourseID == currentCourseID
                                                      select p.RevisionNumber).Distinct().Max();
                                {
                                    currentevisionNumber = max_revision;
                                    preevisionNumber = currentevisionNumber - 1;
                                }
                                FillRemianingModules(currentCourseID, registrationNumber, entityID, applicantTypeID, exceptional_exam_id, candidate_lastexam, 0);


                          

                        }

                     else
                     {
                         try
                         {

                             if (IsSessionAlive() == false)
                                 Response.Redirect("../Home.aspx");
                             Response.CacheControl = "no-cache";
                             Response.AddHeader("Progra", "no-cache");
                             Response.Expires = -1500;
                             Response.ExpiresAbsolute = DateTime.Now.AddDays(1);

                             //if (Request.UrlReferrer == null)
                             ////if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in") && (Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "https://nielit.gov.in"))
                             //{
                             //    Response.Write(GeInvalidRequestMessage("Goto Home Page", Server.MapPath("../Index.aspx")));
                             //    Response.End();
                             //    return;
                             //}

                             loginUserType = (UserType)Session["UserType"];
                             entityID = Convert.ToInt64(Session["EntityID"]);
                             examStartDate = Convert.ToDateTime(Request.QueryString["examStartDate"]);
                             //entityID = 1413670;

                             currentCourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                             //currentCourseID = 115;

                             registrationNumber = Convert.ToInt64(Request.QueryString["RegNo"]);
                             //  registrationNumber = 1393908;

                             if (currentCourseID == 1)
                             {
                                // Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL = CourseManager.Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL(registrationNumber);
                                 if (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 0)
                                 {
                                     chklistPracticals.Enabled = false;
                                 }
                             }
                             else if (currentCourseID == 1213)
                             {
                                 chklistPracticals.Enabled = false;
                             }
                             else if (currentCourseID == 2)
                             {
                                 //Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL = CourseManager.Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL(registrationNumber);
                             }
                             else if (currentCourseID == 3)
                             {
                                 Eligibility_Method_For_Apply_In_Old_Exam_Pattren_B_LVL = Eligibility_For_Apply_In_Old_Exam_Pattren_B_LVL(registrationNumber, currentCourseID, entityID, currentevisionNumber);
                             }
                          
                                     //Added to let candiadte to fill the exam form of July . should remove  from  line 70  to 87 

                                     var CDate = (from c in context.RegistrationDetails
                                                  where c.RegistrationNo == registrationNumber && c.CourseID == currentCourseID
                                                  select new { CommencementFromDate = c.CommencementFromDate }).FirstOrDefault();

                                     if (CDate.CommencementFromDate > examStartDate.AddDays(-(double)(examStartDate.Day - 1)) && currentCourseID!=1213)
                                     {
                                         lblerror.Visible = true;
                                         lblerror.Text = " You are not allowed to fill the Examination Form. Please check your Commencement Date  or Registration Validity in Registration Detail.";
                                         tblMain.Visible = false;
                                         btnSave.Visible = false;
                                         trModuleOption1.Visible = false;
                                         trModuleOption.Visible = false;
                                         trModuleHead.Visible = false;
                                         tr_select_module.Visible = false;
                                         rbtn_module_selection.Visible = false;
                                         //ShowAlert("You are not allowed to fill the Examination Form");
                                         return;
                                     }
                                     else
                                     {

                                         if (!CourseManager.IsCandidateDebarred(currentCourseID, registrationNumber))
                                         {
                                             lblerror.Text = "You can not apply for this Exam because your candidature is debarred from appearing in subsequent two examinations of NIELIT.";
                                             lblerror.Visible = true;
                                             btnSave.Visible = false;
                                             trModuleOption1.Visible = false;
                                             trModuleOption.Visible = false;
                                             trModuleHead.Visible = false;
                                             tr_select_module.Visible = false;
                                             rbtn_module_selection.Visible = false;
                                             return;
                                         }

                                         if (!context.Candidates.Any(c => (c.IsLocked == true && c.ID == entityID)))
                                         {
                                             lblerror.Text = "You can not apply for exam.<br>Your profile details are not completed/locked yet. Please first complete your profile details and lock it. <br> <a href=" + EConnect.Utils.Security.QuertStringModule.Encrypt("myprofile.aspx") + "> Click here to view profile</a>";
                                             lblerror.Visible = true;
                                             btnSave.Visible = false;
                                             tblMain.Visible = false;
                                             return;
                                         }

                                         ////-------Start-----------for fetching previous and curret revision number from database automatically----------------------------------------

                                         var revision_text = (from s in context.RevisionChoices
                                                              where s.course_id == currentCourseID && s.whether_show_revision_choice == "Y" && s.show_revision_choice_till_date >= DateTime.Now
                                                              orderby (s.revision_choice_effective_date) descending
                                                              select new { previous_revision_number = s.previous_revision_number, new_revision_number = s.new_revision_number }).FirstOrDefault();


                                         if (CourseManager.IsCourseReviesd(currentCourseID))
                                         {
                                             //lblRevision.Text = currentevisionNumber.ToString() + "<sup>th</sup> Revision) ";
                                             //rbtn_module_selection.Items.FindByValue("pre").Text = "Revision " + revision_text.previous_revision_number.ToString();
                                             //rbtn_module_selection.Items.FindByValue("current").Text = "Revision " + revision_text.new_revision_number.ToString();

                                             rbtn_module_selection.Items.FindByValue("pre").Text = "Old Pattern";
                                             rbtn_module_selection.Items.FindByValue("current").Text = "New Pattern";
                                         }
                                         //------------------------------------------------------------------------------------------------------------------------------------------END

                                         currentCourse = context.Courses.Find(currentCourseID);
                                         Int32 ShortermCoursesCategory = context.CourseCategories.Where(s => s.Code.ToUpper() == "STC").FirstOrDefault().ID;
                                         EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlCandidateType, typeof(enmApplicantType), null);
                                         if (ddlCandidateType.Items[0].Value == Convert.ToInt32(enmApplicantType.Direct).ToString())
                                             ddlCandidateType.Items[0].Text = "As Direct Candidate";
                                         if (ddlCandidateType.Items[1].Value == Convert.ToInt32(enmApplicantType.Institute).ToString())
                                             ddlCandidateType.Items[1].Text = "Through Accredited Institute";
                                         lbllevel.Text = currentCourse.Name;
                                         var registrationDetail = (from s in context.RegistrationDetails
                                                                   where s.CourseID == currentCourseID && s.CandidateID == entityID && s.RegistrationNo == registrationNumber
                                                                   select s).FirstOrDefault();
                                         if (registrationDetail != null)
                                         {
                                             //Declaration
                                             Candidate candidate = registrationDetail.Candidate;
                                             lblCandidateName.Text = candidate.Salutation + " " + GetInitCap(candidate.Name);
                                             lblRegNumber.Text = registrationNumber.ToString();

                                             lblName.Text = GetInitCap(candidate.Name);

                                             if (candidate.Gender == "Male")
                                                 lblName.Text += " S/o ";
                                             else
                                                 lblName.Text += " D/o ";
                                             if (!string.IsNullOrEmpty(candidate.FatherName))
                                                 lblName.Text += GetInitCap(candidate.FatherName);
                                             else if (!string.IsNullOrEmpty(candidate.GuardianName))
                                                 lblName.Text += GetInitCap(candidate.GuardianName);
                                             else
                                                 lblName.Text += " NA ";
                                             if (registrationDetail.enmApplicantType == enmApplicantType.Direct)
                                                 lblCandidateType.Text = " as a Direct candidate ";
                                             else
                                             {
                                                 //Institute institute = registrationDetail.Institute;
                                                 Institute institute = registrationDetail.Institute;
                                                 if (institute != null)
                                                 {
                                                     lblCandidateType.Text = " through Accredited Institute namely " + institute.Name + " ACCR No. " + currentCourse.Code + "-" + institute.AccreditationDetails.Where(d => d.CourseID == currentCourseID).FirstOrDefault().AccreditationNumber;
                                                     lblCandidateType1.Text = lblCandidateType.Text;
                                                 }
                                             }

                                             //modification in declaration
                                             if (currentCourse.CourseCategoryID == ShortermCoursesCategory)
                                             {
                                                 header1.Visible = false;
                                                 header2.Visible = true;
                                             }
                                             else
                                             {
                                                 header1.Visible = true;
                                                 header2.Visible = false;
                                             }
                                             applicantTypeID = registrationDetail.ApplicantTypeID;
                                             Exam exam = CourseManager.GetNextExam(context, currentCourseID, applicantTypeID);

                                             Int32 max_revision = (from p in context.Modules
                                                                   where p.CourseID == currentCourseID
                                                                   select p.RevisionNumber).Distinct().Max();
                                             //  calculate max Exam Pattern

                                             //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<=============================For July2020 revision change====================================================
                                             Int32 application_status = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                                             exceptional_exam_id = context.ExamCycleExceptionalFeatures.Where(s => s.whether_effective == "Y" && s.course_id == currentCourseID && System.Data.Entity.DbFunctions.TruncateTime(s.effective_upto_date) > System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)).Select(s => s.exam_id).Distinct().FirstOrDefault();
                                             //Exam lastExam_attempted_exceptionalcase = CourseManager.GeLastExam(context, currentCourseID, applicantTypeID);
                                             int candidate_attempted_last_exam = (from s in context.CourseExamApplicationDetails
                                                                                  join ee in context.Exams on s.ExamID equals ee.ID
                                                                                  join ce in context.CourseExamApplications on s.CourseExamApplicationID equals ce.ID
                                                                                  where s.CourseID == currentCourseID && s.RegistrationNumber == registrationNumber && s.CandidateID == entityID
                                                                                      //&& s.ExamID.HasValue == true
                                                                                  && ce.ApplicationStatusID == application_status
                                                                                  select ce.ExamID).DefaultIfEmpty(0).Distinct().Max();

                                             int revisionNumber_with_nochoice = (from s in context.CourseExamApplicationDetails
                                                                                 join m in context.Modules on s.ModuleID equals m.ID
                                                                                 where (s.CourseID == currentCourseID && s.CandidateID == entityID &&
                                                                                         s.RegistrationNumber == registrationNumber && s.ExamID == exceptional_exam_id)
                                                                                 orderby m.RevisionNumber, m.Code
                                                                                 select m.RevisionNumber).DefaultIfEmpty(0).Distinct().Max();
                                             //lastexam = lastExam_attempted_exceptionalcase == null ? 0 : lastExam_attempted_exceptionalcase.ID;
                                             candidate_lastexam = candidate_attempted_last_exam == 0 ? 0 : candidate_attempted_last_exam;

                                             //=====================================================================For July2020==========================>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                             if ((regStatusID.Contains(registrationDetail.RegistrationStatusID.Value) || (registrationDetail.RegistrationStatusID.Value == 4 && registrationDetail.CourseID == 2)) && registrationDetail.ValidUptoDate >= DateTime.Now.Date)  // coded on dated 02052024 by abhi singh 
                                             {
                                                 //<<<<<<<<<<<<<<<<<<<<<<=============================For July2020 revision change=======================================
                                                 if (exceptional_exam_id == candidate_lastexam && exceptional_exam_id != 0)
                                                 {
                                                     currentevisionNumber = revisionNumber_with_nochoice;
                                                 }

                                                 //=====================================================For July2020==========================>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                                                 else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == max_revision)
                                                 //if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == max_revision)
                                                 {
                                                     currentevisionNumber = max_revision; ;
                                                     preevisionNumber = currentevisionNumber - 1;
                                                 }
                                                 else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) != max_revision)
                                                 {
                                                     if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                     {
                                                         if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0)
                                                         {
                                                             currentevisionNumber = max_revision;
                                                             preevisionNumber = currentevisionNumber - 1;
                                                         }
                                                         else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                         {
                                                             currentevisionNumber = CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID);
                                                             preevisionNumber = currentevisionNumber;
                                                         }
                                                     }
                                                     else
                                                     {
                                                         currentevisionNumber = max_revision;
                                                         preevisionNumber = currentevisionNumber - 1;
                                                     }
                                                 }
                                                 else
                                                 {
                                                     currentevisionNumber = max_revision;
                                                     preevisionNumber = currentevisionNumber - 1;
                                                 }
                                                 //Exam exam = CourseManager.GetNextExam(context, currentCourseID, applicantTypeID);
                                                 if (exam != null)
                                                 {
                                                     if (context.ExamTimeTables.Any(t => t.CourseID == currentCourseID && t.ExamID == exam.ID))
                                                     {
                                                         lblExamName.Text = exam.Name.ToUpper();
                                                         lblExamName1.Text = exam.Name.ToUpper();
                                                         //lastExams = CourseManager.GetListOfAttemptedExams(context, currentCourseID, registrationNumber, entityID).Where(c => c.ID != exam.ID );
                                                         lastExams = CourseManager.GetListOfTheoryPassed(context, currentCourseID, registrationNumber, entityID).Where(c => c.ID != exam.ID);
                                                         attemptedLastExams = lastExams.OrderByDescending(d => new { d.ExamYear, d.ExamMonth }).FirstOrDefault();
                                                         if (attemptedLastExams != null)
                                                             lblPreviousExam.Text = attemptedLastExams.Name.ToUpper();

                                                         ddlCandidateType.SelectedValue = registrationDetail.ApplicantTypeID.ToString();
                                                         if (registrationDetail.enmApplicantType == enmApplicantType.Direct)
                                                         {
                                                             ddlCandidateType.Enabled = false;
                                                             ddlPaymentOption.SelectedValue = "1";
                                                             ddlPaymentOption.Enabled = false;
                                                         }
                                                         else
                                                         {
                                                             ddlPaymentOption.SelectedValue = "2";
                                                             ddlPaymentOption.Enabled = true;
                                                             Int32 totalExamsAttempted = lastExams.Count();
                                                             if (currentCourseID == 1 || currentCourseID == 2)
                                                             {
                                                                 if (totalExamsAttempted >= 2)
                                                                     ddlCandidateType.Enabled = true;
                                                                 else
                                                                     ddlCandidateType.Enabled = false;
                                                             }
                                                             else
                                                             {
                                                                 if (totalExamsAttempted >= 6)
                                                                     ddlCandidateType.Enabled = true;
                                                                 else
                                                                     ddlCandidateType.Enabled = false;
                                                             }
                                                         }

                                                         //Exam Center

                                                         if (registrationDetail.CourseCategoryID == 6)
                                                         {
                                                             Institute institute = registrationDetail.Institute;

                                                             SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
                                                             EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();

                                                             cnn.Open();

                                                             //using (SqlCommand cmd = new SqlCommand("select  id, name from Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = '" + institute.StateID + "'))"))

                                                             //November_2024
                                                             using (SqlCommand cmd = new SqlCommand("select  id, name from Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = @instituteStateID ))"))
                                                             {
                                                                 using (SqlDataAdapter sda = new SqlDataAdapter())
                                                                 {
                                                                     cmd.Connection = cnn;
                                                                     //November_2024
                                                                     cmd.Parameters.AddWithValue("@instituteStateID", institute.StateID);
                                                                     sda.SelectCommand = cmd;
                                                                     using (DataTable dTable = new DataTable())
                                                                     {
                                                                         sda.Fill(dTable);

                                                                         //ddlStateFirst.DataSource = dTable;
                                                                         //ddlStateFirst.DataBind();

                                                                         EConnect.Utils.Common.ControlUtility.BindListObject(ddlStateFirst, dTable, new ListItem("--Select Location--", "0"));
                                                                         EConnect.Utils.Common.ControlUtility.BindListObject(ddlStateSecond, dTable, new ListItem("--Select Location--", "0"));

                                                                     }
                                                                 }
                                                             }
                                                         }
                                                         else
                                                         {
                                                             var examcentre = (from p in context.ExamCenters
                                                                               join c in context.ExamWiseExamCenters on p.ID equals c.ExamCenterID
                                                                               join s in context.Locations on p.StateID equals s.ID
                                                                               where (p.CourseCategoryID == currentCourse.CourseCategoryID || p.CourseID == currentCourseID)
                                                                               && c.ExamID == exam.ID
                                                                               select new { ValueField = s.ID, TextField = s.Name.ToUpper() }).Distinct().OrderBy(s => s.TextField);
                                                             EConnect.Utils.Common.ControlUtility.BindListObject(ddlStateFirst, examcentre, new ListItem("--Select Location--", "0"));
                                                             EConnect.Utils.Common.ControlUtility.BindListObject(ddlStateSecond, examcentre, new ListItem("--Select Location--", "0"));

                                                         }
                                                         #region[Prac_Center Added By Reena]

                                                         //Exam Center

                                                         if (registrationDetail.CourseCategoryID == 6)
                                                         {
                                                             Institute institute = registrationDetail.Institute;

                                                             SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
                                                             EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();

                                                             cnn.Open();

                                                             //using (SqlCommand cmd = new SqlCommand("select  id, name from Prac_Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = '" + institute.StateID + "'))"))
                                                             
                                                             //November_2024
                                                             using (SqlCommand cmd = new SqlCommand("select  id, name from Prac_Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = @instituteStateID ))"))
                                                             {
                                                                 using (SqlDataAdapter sda = new SqlDataAdapter())
                                                                 {
                                                                     cmd.Connection = cnn;
                                                                      //November_2024
                                                                     cmd.Parameters.AddWithValue("@instituteStateID", institute.StateID);
                                                                     sda.SelectCommand = cmd;
                                                                     using (DataTable dTable = new DataTable())
                                                                     {
                                                                         sda.Fill(dTable);

                                                                         //ddlStateFirst.DataSource = dTable;
                                                                         //ddlStateFirst.DataBind();


                                                                         EConnect.Utils.Common.ControlUtility.BindListObject(ddlPracStateFirst, dTable, new ListItem("--Select Location--", "0"));
                                                                         EConnect.Utils.Common.ControlUtility.BindListObject(ddlPracStateSecond, dTable, new ListItem("--Select Location--", "0")); 
                                                                     }
                                                                 }
                                                             }
                                                         }
                                                         else
                                                         {
                                                             var examcentre = (from p in context.PracExamCenters
                                                                               //join c in context.ExamWiseExamCenters on p.ID equals c.ExamCenterID
                                                                               join s in context.PracLocations on p.StateID equals s.ID
                                                                               where (p.CourseCategoryID == currentCourse.CourseCategoryID )
                                                                               //&& c.ExamID == exam.ID
                                                                               select new { ValueField = s.ID, TextField = s.Name.ToUpper() }).Distinct().OrderBy(s => s.TextField);

                                                             EConnect.Utils.Common.ControlUtility.BindListObject(ddlPracStateFirst, examcentre, new ListItem("--Select Location--", "0"));
                                                             EConnect.Utils.Common.ControlUtility.BindListObject(ddlPracStateSecond, examcentre, new ListItem("--Select Location--", "0")); 
                                                         }
                                                         #endregion

                                                         #region[Online_Theory_Center Added By Reena]

                                                         //Exam Center

                                                         if (registrationDetail.CourseCategoryID == 6)
                                                         {
                                                             Institute institute = registrationDetail.Institute;

                                                             SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
                                                             EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();

                                                             cnn.Open();

                                                             //using (SqlCommand cmd = new SqlCommand("select  id, name from Online_Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = '" + institute.StateID + "'))"))
                                                             
                                                             //November_2024
                                                             using (SqlCommand cmd = new SqlCommand("select  id, name from Online_Location where id in ( select distinct State_ID   from Course_Wise_State where Regional_Center_ID = ( select distinct top 1 Regional_Center_ID  from Course_Wise_State where course_id =7 and State_ID = @instituteStateID ))"))
                                                             {
                                                                 using (SqlDataAdapter sda = new SqlDataAdapter())
                                                                 {
                                                                     cmd.Connection = cnn;
                                                                     //November_2024
                                                                     cmd.Parameters.AddWithValue("@instituteStateID", institute.StateID);
                                                                     sda.SelectCommand = cmd;
                                                                     using (DataTable dTable = new DataTable())
                                                                     {
                                                                         sda.Fill(dTable); 

                                                                         //ddlStateFirst.DataSource = dTable;
                                                                         //ddlStateFirst.DataBind();  


                                                                         EConnect.Utils.Common.ControlUtility.BindListObject(ddlOnlTheoryExamStateFirst, dTable, new ListItem("--Select Location--", "0"));
                                                                         EConnect.Utils.Common.ControlUtility.BindListObject(ddlOnlTheoryExamStateSecond, dTable, new ListItem("--Select Location--", "0")); 
                                                                     }
                                                                 }
                                                             }
                                                         }
                                                         else
                                                         {
                                                             var examcentre = (from p in context.OnlineExamCenters
                                                                               //join c in context.ExamWiseExamCenters on p.ID equals c.ExamCenterID
                                                                               join s in context.OnlineLocations on p.StateID equals s.ID
                                                                               where (p.CourseCategoryID == currentCourse.CourseCategoryID)
                                                                               //&& c.ExamID == exam.ID
                                                                               select new { ValueField = s.ID, TextField = s.Name.ToUpper() }).Distinct().OrderBy(s => s.TextField);

                                                             EConnect.Utils.Common.ControlUtility.BindListObject(ddlOnlTheoryExamStateFirst, examcentre, new ListItem("--Select Location--", "0"));//Reena
                                                             EConnect.Utils.Common.ControlUtility.BindListObject(ddlOnlTheoryExamStateSecond, examcentre, new ListItem("--Select Location--", "0")); //Reena
                                                         }
                                                         #endregion

                                                         //Language option
                                                         CourseRegistrationPolicy currentPolicy = CourseManager.GetCurrentRegistrationPolicy(context, currentCourseID);
                                                         if (currentPolicy.AllowedLanguage.HasValue)
                                                         {
                                                             rblMedium.SelectedValue = currentPolicy.AllowedLanguage.Value.ToString();
                                                             rblMedium.Enabled = false;
                                                         }

                                                         //Fee Details
                                                         lblTheoryFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee).ToString("F");
                                                         lblPracticalFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee).ToString("F");
                                                         lblLateFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.LateFeeExam).ToString("F");
                                                         lblProcessingFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PostageFeeChargedTowardExaminationCorrespondence).ToString("F");
                                                         lblTotalProcessingFee.Text = lblProcessingFee.Text;
                                                         if (CourseManager.IsLateFeeApplicable(context, exam.ID, applicantTypeID))
                                                             lblTotalLateFee.Text = lblLateFee.Text;

                                                         FillRemianingModules(currentCourseID, registrationNumber, entityID, applicantTypeID, exceptional_exam_id, candidate_lastexam, 0);

                                                         //<<<<<<<<<<<<<<<<<<<<<<=============================For July2020 revision change=======================================
                                                         if (exceptional_exam_id == candidate_lastexam && exceptional_exam_id != 0)
                                                         {
                                                             currentevisionNumber = revisionNumber_with_nochoice;
                                                             tr_select_module.Visible = false;
                                                             rbtn_module_selection.Visible = false;
                                                         }

                                                         //=====================================================For July2020==========================>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                                         else if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                         //if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                         {
                                                             if (CourseManager.IsCourseReviesd(currentCourseID))
                                                             {
                                                                 tr_select_module.Visible = true;
                                                                 rbtn_module_selection.Visible = true;

                                                                 if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0 ||
                                                                     IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == 0)
                                                                 {
                                                                     //tr_select_module.Visible = false;//*****
                                                                     trModuleOption.Visible = false;
                                                                     trModuleHead.Visible = false;
                                                                     trModuleOption1.Visible = false;
                                                                 }
                                                                 else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                                     CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                                 {
                                                                     //tr_select_module.Visible = false;//*****
                                                                     trModuleOption.Visible = true;
                                                                     trModuleHead.Visible = true;
                                                                     trModuleOption1.Visible = true;
                                                                 }
                                                                 else if (IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                                         IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                                 {
                                                                     //tr_select_module.Visible = false;//*****
                                                                     trModuleOption.Visible = true;
                                                                     trModuleHead.Visible = true;
                                                                     trModuleOption1.Visible = true;
                                                                 }
                                                                 else
                                                                 {
                                                                     //tr_select_module.Visible = false;//*****
                                                                     trModuleOption.Visible = false;
                                                                     trModuleHead.Visible = false;
                                                                     trModuleOption1.Visible = false;
                                                                     if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0)
                                                                     {
                                                                         rbtn_module_selection.SelectedIndex = -1;
                                                                     }
                                                                 }
                                                             }
                                                             else
                                                             {
                                                                 if (lblerror.Text == "You can not apply for exam as all theory/bridge/practical modules have been passed."
                                                                     || lblerror.Text == "You have already applied for this exam.")
                                                                 {
                                                                     tr_select_module.Visible = false;
                                                                     rbtn_module_selection.Visible = false;
                                                                     trModuleOption.Visible = false;
                                                                     trModuleHead.Visible = false;
                                                                     trModuleOption1.Visible = false;
                                                                 }
                                                                 else
                                                                 {
                                                                     tr_select_module.Visible = false;
                                                                     rbtn_module_selection.Visible = false;
                                                                     trModuleOption.Visible = true;
                                                                     trModuleHead.Visible = true;
                                                                     trModuleOption1.Visible = true;
                                                                 }
                                                             }
                                                         }
                                                         else
                                                         {
                                                             //commented by me as on 02022023
                                                             //if ((currentCourseID == 1 || currentCourseID == 2 || currentCourseID == 3) && (Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 1 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL == 1 || Eligibility_Method_For_Apply_In_Old_Exam_Pattren_B_LVL == 1))
                                                             if ((currentCourseID == 1 && Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL == 1) || (currentCourseID == 2 && Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL == 1) || (currentCourseID == 3 && Eligibility_Method_For_Apply_In_Old_Exam_Pattren_B_LVL == 1)) 
                                                             //--if ((currentCourseID == 1 || currentCourseID == 2 || currentCourseID == 3))
                                                             {
                                                                 tr_select_module.Visible = true;//*****
                                                                 rbtn_module_selection.Visible = true;
                                                             }
                                                             else
                                                             {
                                                                 tr_select_module.Visible = false;
                                                                 rbtn_module_selection.Visible = false;
                                                             }
                                                             trModuleOption.Visible = true;
                                                             trModuleHead.Visible = true;
                                                             trModuleOption1.Visible = true;
                                                         }

                                                         if (context.CourseExamApplications.Any(c => (c.ExamID == exam.ID && c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber)))
                                                         {
                                                             if (Request.QueryString["Appid"] == null)
                                                             {
                                                                 lblerror.Text = "You have already applied for this exam but not submitted finally.<br>Please change your exam details (if you want to change ) and click on Update button to view the preview. ";
                                                                 chkdisclamier.Checked = true;
                                                                 //<<<<<<<<<<<<<<<<<<<<<<=============================For July2020 revision change=======================================
                                                                 if (exceptional_exam_id == candidate_lastexam && exceptional_exam_id != 0)
                                                                 {
                                                                     //currentevisionNumber = revisionNumber_jul20;
                                                                     tr_select_module.Visible = false;
                                                                     rbtn_module_selection.Visible = false;
                                                                 }
                                                                 //=====================================================For July2020==========================>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                                                 else if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                                 //if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                                 {
                                                                     if (CourseManager.IsCourseReviesd(currentCourseID))
                                                                     {
                                                                         tr_select_module.Visible = true;//*****
                                                                         rbtn_module_selection.Visible = true;

                                                                         if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0 ||
                                                                             IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == 0)
                                                                         {
                                                                             //tr_select_module.Visible = false;//*****
                                                                             trModuleOption.Visible = false;
                                                                             trModuleHead.Visible = false;
                                                                             trModuleOption1.Visible = false;
                                                                         }
                                                                         else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                                             CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                                         {
                                                                             //tr_select_module.Visible = false;//*****
                                                                             trModuleOption.Visible = true;
                                                                             trModuleHead.Visible = true;
                                                                             trModuleOption1.Visible = true;
                                                                         }
                                                                         else if (IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                                                 IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                                         {
                                                                             //tr_select_module.Visible = false;//*****
                                                                             trModuleOption.Visible = true;
                                                                             trModuleHead.Visible = true;
                                                                             trModuleOption1.Visible = true;
                                                                         }
                                                                         else
                                                                         {
                                                                             //tr_select_module.Visible = false;//*****
                                                                             trModuleOption.Visible = false;
                                                                             trModuleHead.Visible = false;
                                                                             trModuleOption1.Visible = false;
                                                                         }
                                                                     }
                                                                     else
                                                                     {
                                                                         tr_select_module.Visible = false;
                                                                         rbtn_module_selection.Visible = false;
                                                                         trModuleOption.Visible = true;
                                                                         trModuleHead.Visible = true;
                                                                         trModuleOption1.Visible = true;
                                                                     }
                                                                 }
                                                                 else
                                                                 {
                                                                     tr_select_module.Visible = false;
                                                                     rbtn_module_selection.Visible = false;
                                                                     trModuleOption.Visible = true;
                                                                     trModuleHead.Visible = true;
                                                                     trModuleOption1.Visible = true;
                                                                 }
                                                             }
                                                             else
                                                             {
                                                                 btnback.Visible = false;
                                                                 lblerror.Text = "You can change your exam details. Please click on Update button after making changes.";
                                                                 //<<<<<<<<<<<<<<<<<<<<<<=============================For July2020 revision change=======================================
                                                                 if (exceptional_exam_id == candidate_lastexam && exceptional_exam_id != 0)
                                                                 {
                                                                     //currentevisionNumber = revisionNumber_jul20;
                                                                     tr_select_module.Visible = false;
                                                                     rbtn_module_selection.Visible = false;
                                                                 }
                                                                 //=====================================================For July2020==========================>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                                                                 else if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                                 //if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                                                 {
                                                                     if (CourseManager.IsCourseReviesd(currentCourseID))
                                                                     {
                                                                         tr_select_module.Visible = true;//*****
                                                                         rbtn_module_selection.Visible = true;

                                                                         if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0 ||
                                                                             IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == 0)
                                                                         {
                                                                             //tr_select_module.Visible = false;//*****
                                                                             trModuleOption.Visible = false;
                                                                             trModuleHead.Visible = false;
                                                                             trModuleOption1.Visible = false;
                                                                         }
                                                                         else if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                                              CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                                         {
                                                                             //tr_select_module.Visible = false;//*****
                                                                             trModuleOption.Visible = true;
                                                                             trModuleHead.Visible = true;
                                                                             trModuleOption1.Visible = true;
                                                                         }
                                                                         else if (IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                                                 IsAlreadyApplied_inlastexam(context, candidate_lastexam, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                                         {
                                                                             //tr_select_module.Visible = false;//*****
                                                                             trModuleOption.Visible = true;
                                                                             trModuleHead.Visible = true;
                                                                             trModuleOption1.Visible = true;
                                                                         }
                                                                         else
                                                                         {
                                                                             //tr_select_module.Visible = false;//*****
                                                                             trModuleOption.Visible = false;
                                                                             trModuleHead.Visible = false;
                                                                             trModuleOption1.Visible = false;
                                                                         }
                                                                     }
                                                                     else
                                                                     {
                                                                         tr_select_module.Visible = false;
                                                                         rbtn_module_selection.Visible = false;
                                                                         trModuleOption.Visible = true;
                                                                         trModuleHead.Visible = true;
                                                                         trModuleOption1.Visible = true;
                                                                     }
                                                                 }
                                                                 else
                                                                 {
                                                                     tr_select_module.Visible = false;
                                                                     rbtn_module_selection.Visible = false;
                                                                     trModuleOption.Visible = true;
                                                                     trModuleHead.Visible = true;
                                                                     trModuleOption1.Visible = true;
                                                                 }
                                                             }
                                                             lblerror.Visible = true;
                                                             Int64 applId = context.CourseExamApplications.Where(c => (c.ExamID == exam.ID && c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber)).FirstOrDefault().ID;
                                                             ShowApplicationData(applId);
                                                         }
                                                     }
                                                     else
                                                     {
                                                         lblerror.Text = "Time table not found for " + exam.Name + " exam. You can not apply for exam at this time.";
                                                         lblerror.Visible = true;
                                                         btnSave.Visible = false;
                                                     }
                                                 }
                                                 else
                                                 {
                                                     lblerror.Text = "No next exam found. You can not apply for exam at this time.";
                                                     lblerror.Visible = true;
                                                     btnSave.Visible = false;
                                                 }
                                             }
                                             else
                                             {
                                                 lblerror.Text = "You are not eligible to apply for exam as your course status is completed/expired/project pending/cancelled.";
                                                 lblerror.Visible = true;
                                                 btnSave.Visible = false;
                                                 tblMain.Visible = false;
                                             }
                                         }
                                         else
                                         {
                                             lblerror.Text = "You are not eligible to apply for exam this time.";
                                             lblerror.Visible = true;
                                             btnSave.Visible = false;
                                         }
                                     };
                                     if (splOLevelCondition == 1)
                                     {
                                         lblserveExpt.Text = "2.1";
                                         panelfeedetails.Visible = false;
                                         disclmrPanel.Visible = false;
                                         btnSave.Visible = false;
                                         btnExmpSubmit.Visible = false;
                                         FeedtlsId.Visible = false;
                                         disclamrdivid.Visible = false;
                                         trModuleOptionExmpt.Visible = false;
                                         trModuleOptionExmpt1.Visible = false;
                                         TrExamCentre1.Visible = false;
                                         TrExamCentre2.Visible = false;
                                         trImprovementHead.Visible = false;
                                         trModuleOption.Visible = false;
                                         trModuleOption1.Visible = false;
                                         trModuleOptionOlevelSpecl.Visible=true;
                                         trModuleOptionOlevelSpecl1.Visible=true;
                                         lblRevision.Visible = true;
                                         TrOptionOlevelSpecl.Visible = true;
                                     }
                         
                         }
                         catch (Exception ex)
                         {
                            // btnExmpSubmit.Visible = true;
                             ShowAlert(ex.Message);
                         }
                     }
                }
                else
                {
                    //btnExmpSubmit.Visible = true;
                    Console.WriteLine("<script>alert('Something went wrong!! Please Contact to Nielit HQ IT Team !')</script>");
                }
            }
        }
        catch(Exception ex) 
        {
            //btnExmpSubmit.Visible = true;
            loopExceptions.Add(ex);
        }
        
       
    }

    
   
}