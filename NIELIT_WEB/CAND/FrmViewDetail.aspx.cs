using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.NIELIT;
using EConnect.DAL;
using System.Text;
using EConnect.URM;
using EConnect.HRMS;
using System.Web.Security;
using EConnect.Utils.Common;
using System.Data.Objects;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

public partial class FrmViewDetail : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentevisionNumber = 0;
    Int32 currentCourseID = 0;
    Int64 registrationNumber = 0;
    enmRegistrationStatus registrationStatus;
    Int32 moduleTypeTheory = Convert.ToInt32(enmModuleType.Theory);
    Int32 moduleTypePractical = Convert.ToInt32(enmModuleType.Practical);
    Int32 moduleTypeProject = Convert.ToInt32(enmModuleType.Project);
    Table tbl = new Table();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Home.aspx");
            loginUserType = (UserType)Session["UserType"];
            currentCourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            registrationNumber = Convert.ToInt64(Request.QueryString["RegNo"]);
            if (!String.IsNullOrEmpty(Request.QueryString["CandidateID"]))
                entityID = Convert.ToInt64(Request.QueryString["CandidateID"]);
            else
                entityID = Convert.ToInt64(Session["EntityID"]);

            using (EConnectContext context = new EConnectContext())
            {
				var nextExam = CourseManager.GetNextExam(context, currentCourseID , 1);
                String currentCourseName = context.Courses.Find(currentCourseID).Name;
                //--currentevisionNumber = CourseManager.GetCourseRevisionNumberAtRegistrationCompleted(context, currentCourseID, registrationNumber, entityID);
                currentevisionNumber = GetCourseRevisionNumberAtRegistrationCompleted(context, currentCourseID, registrationNumber, entityID);
                string sup = "<sup>th</sup> Revision";
                //string sup1 = "5.1";
                if (currentevisionNumber == 1)
                    sup = "<sup>st</sup> Revision";
                else if (currentevisionNumber == 2)
                    sup = "<sup>nd</sup> Revision";
                if (currentevisionNumber == 3)
                    sup = "<sup>rd</sup> Revision";

                if (currentevisionNumber == 6)
                {
                    string currentevisionNumber1 = "5.1";
                    summeryHeading.InnerHtml += "(<a title='Click here to view/print list of modules' href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CourseModuleList.aspx?CourseId=" + currentCourseID.ToString() + "&RevisionId=" + currentevisionNumber.ToString()) + "' target='_blank'>" + currentevisionNumber1.ToString() + sup + "</a>)";
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Module Status: " + currentCourseName, "CAND/FrmViewDetail.aspx?" + Request.QueryString.ToString(), ""));
                }
                else
                {
                    summeryHeading.InnerHtml += " (<a title='Click here to view/print list of modules' href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CourseModuleList.aspx?CourseId=" + currentCourseID.ToString() + "&RevisionId=" + currentevisionNumber.ToString()) + "' target='_blank'>" + currentevisionNumber.ToString() + sup + "</a>)";
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Module Status: " + currentCourseName, "CAND/FrmViewDetail.aspx?" + Request.QueryString.ToString(), ""));
                }

                var latestcourse = (from c in context.RegistrationDetails
                                    where c.CandidateID == entityID && (c.RegistrationStatusID == 1 || c.RegistrationStatusID == 2 || c.RegistrationStatusID == 3 || c.RegistrationStatusID == 4)
                                    orderby c.CommencementFromDate descending, c.CourseID descending
                                    select new
                                    {
                                        //courseid = c.CourseID,
                                        //coursecatId = c.CourseCategoryID,
                                        //courseCatName = c.CourseCategory.Name,
                                        //apptypeid = c.ApplicantTypeID,
                                        //regno = c.RegistrationNo,
                                        regstatusid = c.RegistrationStatusID,
                                        validuptodate = c.ValidUptoDate,
                                        CommencementFromDate = c.CommencementFromDate
                                    }).FirstOrDefault();
                    // };
            if (loginUserType == UserType.Candidate && nextExam!=null)
            {
                Int32 registeredid = Convert.ToInt32(enmRegistrationStatus.Registered);
                Int32 reregisteredid = Convert.ToInt32(enmRegistrationStatus.ReRegistered);
                if ((latestcourse.regstatusid == registeredid || latestcourse.regstatusid == reregisteredid) && (latestcourse.validuptodate > DateTime.Today)
                    && latestcourse.CommencementFromDate <= nextExam.ExamStartDate.AddDays(-(double)(nextExam.ExamStartDate.Day - 1)))
                {
                    showsidelink();
                    // string tt = Convert.ToString(Session["ModuleID"]);
                    // DateTime  examStartDate = nextExam.ExamStartDate.AddDays(-(double)(nextExam.ExamStartDate.Day - 1));
                    //Sidelink.Items.Add(new SideLinkItem("Apply Online For Exam", "../CAND/FrmExamForm.aspx?CourseID=" + currentCourseID + "&RegNo=" + registrationNumber, "../images/Apply_Online.jpg"));
                     Sidelink.Items.Add(new SideLinkItem("Apply Online For Exam", "../CAND/FrmExamForm.aspx?CourseID=" + currentCourseID + "&RegNo=" + registrationNumber + "&examStartDate=" + nextExam.ExamStartDate, "../images/Apply_Online.jpg"));
                    //Sidelink.Items.Add(new SideLinkItem("Print Modules List", "../CAND/CourseModuleList.aspx?CourseId=" + currentCourseID.ToString() + "&RevisionId=" + currentevisionNumber.ToString(), "../images/Print_Admit_Card.jpg", "_blank"));

                    Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                    Sidelink.Render();
                }
            }

            StringBuilder applicationname = new StringBuilder();
            var examstatus = (from r in context.CourseProjectApplications
                              join d in context.DemandNotes
                              on r.DemandNoteID equals d.ID
                              where r.RegistrationNumber == registrationNumber && r.CandidateID == entityID && r.CourseID == currentCourseID
                              && r.FinalSubmitted == true
                              select new
                              {
                                  appno = r.ID,
                                  applicationnumber = r.Number,
                                  coursename = r.Course.Name,
                                  courseid = r.CourseID,
                                  RegistrationNumber = r.RegistrationNumber,
                                  candidateid = r.CandidateID,
                              }).ToList();

            if (examstatus.Count > 0)
            {
                foreach (var names in examstatus)
                {
                    applicationname.Append("<a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("ProjectFormPreview.aspx?candidateID=" + names.candidateid + "&Appid=" + names.appno.ToString()) + "' target='_blank' >" + names.applicationnumber + "</a>" + ",");
                    tdformstatus.InnerHtml = applicationname.ToString().TrimEnd(',');
                   //-- Nsqftdformstatus.InnerHtml = applicationname.ToString().TrimEnd(',');
                }
            }
            else
            {
                tdformstatus.InnerHtml = "Not Available";
               //-- Nsqftdformstatus.InnerHtml = "Not Available";
            }
		  };
            if (!Page.IsPostBack)
            {

                tbl.CssClass = "sample3";
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(100);
                Popup();
                divreport.Controls.Add(tbl);
            }
            BreadCrumb1.Render();
            BindGridView(currentevisionNumber);
            BindRemainingModules();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    public static Int32 GetCourseRevisionNumberAtRegistrationCompleted(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int64 candidateID)
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

    //public string MonthName(int month)
    //{
    //     return CultureInfo.CurrentCulture.
    //        DateTimeFormat.GetMonthName
    //        (month);
    //}

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

    protected void BindGridView(int RevisionNoOfCandiadte)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //var listOfPassedModules = (from d in context.CourseExamApplicationDetails
                //                           join m in context.Modules on d.ModuleID equals m.ID                                           
                //                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                //                           d.Grade.IsPassed == true                                          
                //                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                //                           select new
                //                           {
                //                               CourseID = m.CourseID,
                //                               ID = m.ID,
                //                               name = m.Name,
                //                               Code = m.ShortName,
                //                               ModuleTypeID = m.ModuleTypeID,
                //                               SelectionTypeID = m.SelectionTypeID,
                //                               ElectiveGroup = m.ElectiveGroup,
                //                               MType = m.ModuleType.Name + (m.ModuleTypeID == 1 ? (m.SelectionTypeID == 1 ? " (Comp.)" : " (Elect.)") : ""),
                //                               doexam = d.Exam.Name == null ? "NA" : d.Exam.Name,
                //                               Result = d.Grade.Description,
                //                               Grade = d.Grade.Code                                              
                //                           });
                  
                var dataCourseStatus = (from m in context.RegistrationDetails where m.CourseID == currentCourseID && m.RegistrationNo == registrationNumber select m.RegistrationStatus).FirstOrDefault();
                
                int tdTotalTheoryModule = 0; int tdTotalPactModules = 0; int tdTotalProjectModule = 0; int tdTotalpassedThryModules = 0; int tdTotalpassedPractModules = 0; int tdTotalpassedProjectModules = 0; int tdTotalExemptThryModules = 0; int tdTotalExemptPractModules = 0; int tdTotalexemptProjectModules = 0;
                var listOfPassedModules = GetData(currentCourseID, registrationNumber, entityID, RevisionNoOfCandiadte);
                //listOfPassedModules = listOfPassedModules.Union(listofProjectModules).Union(listofPassesPracticalModules);
                gvMain.DataSource = listOfPassedModules;
                gvMain.DataBind();
                Int32 moduleTypeBridge = Convert.ToInt32(enmModuleType.Bridge);
                // code Added on dated 26062023 by Abhi Singh Purpose - Add New GridView for display Intra-Level Exemption
                string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                SqlConnection con = new SqlConnection(constr);
                //string Sql = "select (case when Base_Module_ID!='' then (select (Short_Name+' - '+ Name) from Module where Id=Base_Module_ID) End) as ExempttedModule, (case when Exempted_Module_ID!='' then (select (Short_Name+' - '+ Name) from Module where Id=Exempted_Module_ID) End) as ExemptionModule from Course_Exam_Exemption where Candidate_ID="+entityID+" and Registration_Number="+registrationNumber+" and base_course_id="+currentCourseID+" and Exempted_Course_ID="+currentCourseID+"";
                
                //November_2024
                string Sql = "select (case when Base_Module_ID!='' then (select (Short_Name+' - '+ Name) from Module where Id=Base_Module_ID) End) as ExempttedModule, (case when Exempted_Module_ID!='' then (select (Short_Name+' - '+ Name) from Module where Id=Exempted_Module_ID) End) as ExemptionModule from Course_Exam_Exemption where Candidate_ID=@entityID and Registration_Number=@registrationNumber and base_course_id=@currentCourseID and Exempted_Course_ID=@currentCourseID ";
                SqlDataAdapter da = new SqlDataAdapter(Sql,con);
                da.SelectCommand.Parameters.AddWithValue("@entityID", entityID);
                da.SelectCommand.Parameters.AddWithValue("@registrationNumber", registrationNumber);
                da.SelectCommand.Parameters.AddWithValue("@currentCourseID", currentCourseID);
                //da.SelectCommand.Parameters.Add("@entityID", SqlDbType.BigInt).Value= entityID;
                //da.SelectCommand.Parameters.Add("@registrationNumber", SqlDbType.BigInt).Value = registrationNumber; ;
                //da.SelectCommand.Parameters.Add("@currentCourseID", SqlDbType.Int).Value = currentCourseID; 
               

                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    intralblExemptDiv.Visible = true;
                    LblexempNote.Text = currentCourseID == 2 ? " Note:- Papers A9-R4,A10.1-R4/A10.2-R4 having no direct exemption in the latest syllabus revision, hence will not be counted as passed modules in new revision instead the modules exempted against these modules(if applicable & availed) will be counted to qualify the level. Allthough candidate can avail Module-Wise certificate for the papers of old revision and the same old revision modules will be counted in final certification" : " Note:- Papers BE2-R4, B4.5-R4, B5.2-R4, BE10-R4 and BE12-R4 having no direct exemption in the latest syllabus revision, hence will not be counted as passed modules in new revision instead the modules exempted against these modules(if applicable & availed) will be counted to qualify the level. Allthough candidate can avail Module-Wise certificate for the papers of old revision and the same old revision modules will be counted in final certification";
                    gvIntraLevelExemption.DataSource = dt;
                    gvIntraLevelExemption.DataBind();
                }

                //int currentevisionNumber23 = (from c in context.CourseExamApplicationDetails
                //                            join m in context.Modules on c.ModuleID equals m.ID
                //                              where c.CourseID == currentCourseID && c.RegistrationNumber == registrationNumber && c.CandidateID == entityID && m.ModuleTypeID != 3 && m.ModuleTypeID != 4
                //                            select m.RevisionNumber).Max();

                int currentevisionNumber23 = RevisionNoOfCandiadte;
              
                if (currentevisionNumber23 > 5 && (currentCourseID == 2||currentCourseID == 1))
                {
                var  listOfPassedModules1 = (from d in context.CourseExamApplicationDetails
                                                join m in context.Modules on d.ModuleID equals m.ID
                                                where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                                d.Grade.IsPassed == true && ((int?)d.ExamID == null || (int?)d.ExamID != null) && d.ResultGradeID != 12 && d.ResultGradeID != 191 && m.ID != 354 && m.ID != 355 && m.ID != 356
                                                orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                select new
                                                {
                                                    CourseID = m.CourseID,
                                                    ID = m.ID,
                                                    name = m.Name,
                                                    Code = m.ShortName,
                                                    ModuleTypeID = m.ModuleTypeID,
                                                    SelectionTypeID = m.SelectionTypeID,
                                                    ElectiveGroup = m.ElectiveGroup,
                                                    MType = m.ModuleType.Name + (m.ModuleTypeID == 1 ? (m.SelectionTypeID == 1 ? " (Comp.)" : " (Elect.)") : ""),
                                                    doexam = d.Exam.Name == null ? "NA" : d.Exam.Name,
                                                    //doexam =  e.Name == null ? "NA" : e.Name,
                                                    Result = d.Grade.Description,
                                                    Grade = d.Grade.Code
                                                });

                //List<int> listOfPassedModules2 = (from d in context.CourseExamApplicationDetails
                //                            join m in context.Modules on d.ModuleID equals m.ID
                //                            where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                //                            d.Grade.IsPassed == true && ((int?)d.ExamID == null || (int?)d.ExamID != null) && d.ResultGradeID != 12 && d.ResultGradeID != 191 && m.ID != 354 && m.ID != 355 && m.ID != 356
                //                            orderby m.ModuleTypeID, m.SelectionTypeID, m.Code select m.ID).ToList();
                //List<int> data1 = new List<int> { 938, 939, 940, 941, 717, 718, 719, 720, 346, 347, 348, 349, 22, 16, 23, 24, 133, 134, 136, 137 };
                     
                //     var dta= listOfPassedModules2.Intersect(data1);

               if (dataCourseStatus.Code == "P")
                     {
                         exemptdetlswindow.Visible = false;
                         remaingdetlswindow.Visible = false;
                         tdPassedTheoryModules.InnerText = listOfPassedModules1.Where(d => (d.ModuleTypeID == moduleTypeTheory || d.ModuleTypeID == moduleTypeBridge)).Count().ToString();
                        
                         //added coded by Abhi Singh on Dated 11092023 ---start---
                         if (currentCourseID == 2 || currentCourseID == 1)
                         {
                             List<int> listOfPassedModules2 = (from d in context.CourseExamApplicationDetails
                                                               join m in context.Modules on d.ModuleID equals m.ID
                                                               where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                                               d.Grade.IsPassed == true && ((int?)d.ExamID == null || (int?)d.ExamID != null) && d.ResultGradeID != 12 && d.ResultGradeID != 191 && (m.ID == 929 || m.ID == 930 || m.ID == 931
                                                               || m.ID == 932 || m.ID == 938 || m.ID == 939 || m.ID == 940 || m.ID == 941 || m.ID == 717 || m.ID == 718 || m.ID == 719 || m.ID == 720 || m.ID == 346 || m.ID == 348 || m.ID == 949
                                                               || m.ID == 22 || m.ID == 16 || m.ID == 24 || m.ID == 23 || m.ID == 133 || m.ID == 134 || m.ID == 136 || m.ID == 137 || m.ID == 711 || m.ID == 712 || m.ID == 713 || m.ID == 338 || m.ID == 339 || m.ID == 340 || m.ID == 19 || m.ID == 20 || m.ID == 21
                                                               || m.ID == 17 || m.ID == 100 || m.ID == 103 || m.ID == 104 || m.ID == 107 || m.ID == 108 || m.ID == 109)
                                                               orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                               select m.ID).ToList();
                             //List<int> data1 = new List<int> { 938, 939, 940, 941, 717, 718, 719, 720, 346, 347, 348, 349, 22, 16, 23, 24, 133, 134, 136, 137 };

                             //var dta = listOfPassedModules2.Intersect(data1);
                             tdPassedPracticalModules.InnerText = listOfPassedModules2.Count().ToString();
                         }
                         else
                         tdPassedPracticalModules.InnerText = listOfPassedModules1.Where(d => d.ModuleTypeID == moduleTypePractical).Count().ToString();
                         //End

                         tdPassedProjectModules.InnerText = listOfPassedModules1.Where(d => d.ModuleTypeID == moduleTypeProject).Count().ToString();
                     }
                     else
                     {
                         if(currentCourseID!=1)
                         exemptdetlswindow.Visible = true;

                         List<int> listOfPassedModules2 = GetTotalPassedforExemptofPract(currentCourseID, registrationNumber, currentevisionNumber23, entityID);
                         List<int> data1 = new List<int> { 929, 930, 931, 932, 938, 939, 940, 941 };

                         var dta = listOfPassedModules2.Intersect(data1);

                         remaingdetlswindow.Visible = true;
                         //int countmodule = (listOfPassedModules1.Where(d => d.ModuleTypeID == moduleTypePractical).Count()-dta.Count());
                         //tdPassedTheoryModules.InnerText = listOfPassedModules1.Where(d => (d.ModuleTypeID == moduleTypeTheory || d.ModuleTypeID == moduleTypeBridge)).Count().ToString();
                         tdPassedTheoryModules.InnerText = GetTotalPassedAllTheoryModuleCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID).ToString();
                         tdTotalpassedThryModules = GetTotalPassedAllTheoryModuleCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID); //--
                         //tdPassedElectiveModules.InnerText = listOfPassedModules.Where(d => (d.ModuleTypeID == moduleTypeTheory && d.SelectionTypeID == 2)).Select(m=>m.ElectiveGroup).Distinct().Count().ToString();
                         tdPassedPracticalModules.InnerText = (GetTotalPractPassedThorughParityCount(currentCourseID,registrationNumber,currentevisionNumber23,entityID) + dta.Count()).ToString();
                         tdTotalpassedPractModules = (GetTotalPractPassedThorughParityCount(currentCourseID,registrationNumber,currentevisionNumber23,entityID) + dta.Count()); //--
                         tdPassedProjectModules.InnerText = GetTotalProjectpassedThorughParityCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID).ToString();
                         tdTotalpassedProjectModules = GetTotalProjectpassedThorughParityCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID); //--
                     }
                }
                else
                {
                    var listOfPassedModules1 = (from d in context.CourseExamApplicationDetails
                                                join m in context.Modules on d.ModuleID equals m.ID
                                                where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                                d.Grade.IsPassed == true && ((int?)d.ExamID == null || (int?)d.ExamID != null) && d.ResultGradeID != 12 && d.ResultGradeID != 191 
                                                orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                select new
                                                {
                                                    CourseID = m.CourseID,
                                                    ID = m.ID,
                                                    name = m.Name,
                                                    Code = m.ShortName,
                                                    ModuleTypeID = m.ModuleTypeID,
                                                    SelectionTypeID = m.SelectionTypeID,
                                                    ElectiveGroup = m.ElectiveGroup,
                                                    MType = m.ModuleType.Name + (m.ModuleTypeID == 1 ? (m.SelectionTypeID == 1 ? " (Comp.)" : " (Elect.)") : ""),
                                                    doexam = d.Exam.Name == null ? "NA" : d.Exam.Name,
                                                    //doexam =  e.Name == null ? "NA" : e.Name,
                                                    Result = d.Grade.Description,
                                                    Grade = d.Grade.Code
                                                });

                    if (dataCourseStatus.Code == "P")
                    {
                        
                        //added coded by Abhi Singh on Dated 11092023 --- Start--
 
                        var listOfPassedModules2 = (from d in context.CourseExamApplicationDetails
                                                    join m in context.Modules on d.ModuleID equals m.ID
                                                    where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                                    d.Grade.IsPassed == true && ((int?)d.ExamID == null || (int?)d.ExamID != null) 
                                                    orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                    select new
                                                    {
                                                        CourseID = m.CourseID,
                                                        ID = m.ID,
                                                        name = m.Name,
                                                        Code = m.ShortName,
                                                        ModuleTypeID = m.ModuleTypeID,
                                                        SelectionTypeID = m.SelectionTypeID,
                                                        ElectiveGroup = m.ElectiveGroup,
                                                        MType = m.ModuleType.Name + (m.ModuleTypeID == 1 ? (m.SelectionTypeID == 1 ? " (Comp.)" : " (Elect.)") : ""),
                                                        doexam = d.Exam.Name == null ? "NA" : d.Exam.Name,
                                                        //doexam =  e.Name == null ? "NA" : e.Name,
                                                        Result = d.Grade.Description,
                                                        Grade = d.Grade.Code
                                                    });
                        //End
                        exemptdetlswindow.Visible = false;
                        remaingdetlswindow.Visible = false;
                        tdPassedTheoryModules.InnerText = listOfPassedModules2.Where(d => (d.ModuleTypeID == moduleTypeTheory || d.ModuleTypeID == moduleTypeBridge)).Count().ToString();
                        tdPassedPracticalModules.InnerText = listOfPassedModules2.Where(d => d.ModuleTypeID == moduleTypePractical).Count().ToString();
                        tdPassedProjectModules.InnerText = listOfPassedModules2.Where(d => d.ModuleTypeID == moduleTypeProject).Count().ToString();
                    }
                    else
                    {
                        if (currentCourseID != 1)
                         exemptdetlswindow.Visible = true;
                          
                         remaingdetlswindow.Visible = true;
                        //tdPassedTheoryModules.InnerText = listOfPassedModules1.Where(d => (d.ModuleTypeID == moduleTypeTheory || d.ModuleTypeID == moduleTypeBridge)).Count().ToString();
                        tdPassedTheoryModules.InnerText = GetTotalPassedAllTheoryModuleCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID).ToString();
                        tdTotalpassedThryModules = GetTotalPassedAllTheoryModuleCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID); //--
                        //tdPassedElectiveModules.InnerText = listOfPassedModules.Where(d => (d.ModuleTypeID == moduleTypeTheory && d.SelectionTypeID == 2)).Select(m=>m.ElectiveGroup).Distinct().Count().ToString();
                        tdPassedPracticalModules.InnerText = GetTotalPractPassedThorughParityCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID).ToString();
                        tdTotalpassedPractModules = GetTotalPractPassedThorughParityCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID); //--
                        tdPassedProjectModules.InnerText = GetTotalProjectpassedThorughParityCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID).ToString();
                        tdTotalpassedProjectModules = GetTotalProjectpassedThorughParityCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID); //--
                      }
                }

                var listOfPassedModules21 = (from d in context.CourseExamApplicationDetails
                                            join m in context.Modules on d.ModuleID equals m.ID
                                            where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                            d.Grade.IsPassed == true && ((int?)d.ExamID == null || (int?)d.ExamID != null) && (d.ResultGradeID == 12 || d.ResultGradeID == 191)
                                            orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                            select new
                                            {
                                                CourseID = m.CourseID,
                                                ID = m.ID,
                                                name = m.Name,
                                                Code = m.ShortName,
                                                ModuleTypeID = m.ModuleTypeID,
                                                SelectionTypeID = m.SelectionTypeID,
                                                ElectiveGroup = m.ElectiveGroup,
                                                MType = m.ModuleType.Name + (m.ModuleTypeID == 1 ? (m.SelectionTypeID == 1 ? " (Comp.)" : " (Elect.)") : ""),
                                                doexam = d.Exam.Name == null ? "NA" : d.Exam.Name,
                                                //doexam =  e.Name == null ? "NA" : e.Name,
                                                Result = d.Grade.Description,
                                                Grade = d.Grade.Code
                                            });
           

                Int32 theoryCompModules = CourseManager.GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Theory, enmSelectionType.Compulsory);
                Int32 theoryElectiveModules = CourseManager.GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Theory, enmSelectionType.Elective);
                Int32 bridgeModules = CourseManager.GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Bridge, null);
                tdTotalTheoryModules.InnerText = (theoryCompModules + theoryElectiveModules + bridgeModules).ToString() + " (" + theoryCompModules.ToString() + " + " + theoryElectiveModules.ToString() + " + " + bridgeModules.ToString() + ")";
                tdTotalTheoryModule = (theoryCompModules + theoryElectiveModules + bridgeModules); //--
                tdTotalPracticalModules.InnerText = CourseManager.GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Practical, null).ToString();
                tdTotalPactModules = CourseManager.GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Practical, null); //--
                tdTotalProjectModules.InnerText = CourseManager.GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Project, null).ToString();
                tdTotalProjectModule = CourseManager.GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Project, null); //--
           
                int attempted = CourseManager.GetCountOfAttempteddModulesOfAnyRevision(context, currentCourseID, registrationNumber, entityID, enmModuleType.Theory);
                tdAttemptedTheoryModules.InnerText = (attempted + CourseManager.GetCountOfAttempteddModulesOfAnyRevision(context, currentCourseID, registrationNumber, entityID, enmModuleType.Bridge)).ToString();
                tdAttemptedPracticalModules.InnerText = CourseManager.GetCountOfAttempteddModulesOfAnyRevision(context, currentCourseID, registrationNumber, entityID, enmModuleType.Practical).ToString();
                tdAttemptedProjectModules.InnerText = CourseManager.GetCountOfAttempteddModulesOfAnyRevision(context, currentCourseID, registrationNumber, entityID, enmModuleType.Project).ToString();

                //--

                //List<int> listOfPassedModules2 = (from d in context.CourseExamApplicationDetails
                //                                  join m in context.Modules on d.ModuleID equals m.ID
                //                                  where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                //                                  d.Grade.IsPassed == true && ((int?)d.ExamID == null || (int?)d.ExamID != null) && m.ID != 354 && m.ID != 355 && m.ID != 356
                //                                  orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                //                                  select m.ID).ToList();

                List<int> listOfPassedModules12 = GetTotalPassedforExemptofPractwithExemptedModule(currentCourseID, registrationNumber, currentevisionNumber23, entityID);
                List<int> data21 = new List<int> { 929, 930, 931, 932, 938, 939, 940, 941 };

                var dta1 = listOfPassedModules12.Intersect(data21);
                //--

                if (currentCourseID != 1)
                {
                    //exemptdetlswindow.Visible = true;
                    //--tdtotalexemptthory.InnerText = listOfPassedModules21.Where(d => (d.ModuleTypeID == moduleTypeTheory || d.ModuleTypeID == moduleTypeBridge)).Count().ToString();
                    tdtotalexemptthory.InnerText = GetTotalExemptedAllTheoryModuleCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID).ToString();
                    tdTotalExemptThryModules = GetTotalExemptedAllTheoryModuleCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID); //--
                    //tdPassedElectiveModules.InnerText = listOfPassedModules.Where(d => (d.ModuleTypeID == moduleTypeTheory && d.SelectionTypeID == 2)).Select(m=>m.ElectiveGroup).Distinct().Count().ToString();
                    if (currentevisionNumber23 > 5 && currentCourseID==2)
                    {
                        tdtotalexemptpract.InnerText = (GetTotalPractExemptThorughParityCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID)+dta1.Count()).ToString();
                        tdTotalExemptPractModules = (GetTotalPractExemptThorughParityCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID) + dta1.Count()); //--
                    }
                    else
                    {
                        tdtotalexemptpract.InnerText = GetTotalPractExemptThorughParityCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID).ToString();
                        tdTotalExemptPractModules = GetTotalPractExemptThorughParityCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID); //--
                    }

                    tdtotalexemptproject.InnerText = GetTotalProjectExemptThorughParityCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID).ToString();
                    tdTotalexemptProjectModules = GetTotalProjectExemptThorughParityCount(currentCourseID, registrationNumber, currentevisionNumber23, entityID); //--

                    tdtotalremaingthory.InnerText = (tdTotalTheoryModule - (tdTotalpassedThryModules + tdTotalExemptThryModules)).ToString();
                    tdtotalremaingpract.InnerText = (tdTotalPactModules - (tdTotalpassedPractModules + tdTotalExemptPractModules)).ToString();
                    tdtotalremaingproject.InnerText = (tdTotalProjectModule - (tdTotalpassedProjectModules + tdTotalexemptProjectModules)).ToString();
                }
                else
                {
                    tdtotalremaingthory.InnerText = (tdTotalTheoryModule - tdTotalpassedThryModules).ToString();
                    tdtotalremaingpract.InnerText = (tdTotalPactModules - tdTotalpassedPractModules ).ToString();
                    tdtotalremaingproject.InnerText = (tdTotalProjectModule - tdTotalpassedProjectModules).ToString();
                }

               /* tdRemainingTheorygModules.InnerText = ((theoryCompModules + theoryElectiveModules + bridgeModules) - Convert.ToInt32(tdPassedTheoryModules.InnerText)).ToString();
                tdRemainingPracticalModules.InnerText = (Convert.ToInt32(tdTotalPracticalModules.InnerText) - Convert.ToInt32(tdPassedPracticalModules.InnerText)).ToString();
                tdRemainingProjectModules.InnerText = (Convert.ToInt32(tdTotalProjectModules.InnerText) - Convert.ToInt32(tdPassedProjectModules.InnerText)).ToString();*/

                IQueryable<Exam> attemptedExams = CourseManager.GetListOfAttemptedExams(context, currentCourseID, registrationNumber, entityID);
                foreach (Exam exam in attemptedExams.OrderByDescending(d => new { d.ExamYear, d.ExamMonth }).ToList())
                {
                    if (exam != null)
                    {
                        if (!String.IsNullOrEmpty(Request.QueryString["CandidateID"]))
                        {
                            tdAttemptedExams.InnerHtml += "<a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("frmexamhistory.aspx?CourseID=" + exam.CourseID.ToString() + " &ExamId=" + exam.ID.ToString() + "&CandidateID=" + Request.QueryString["CandidateID"]) + "' target='_self'>" + exam.Name + "</a>; ";
                            //--NsqftdAttemptedExams.InnerHtml += "<a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("frmexamhistory.aspx?CourseID=" + exam.CourseID.ToString() + " &ExamId=" + exam.ID.ToString() + "&CandidateID=" + Request.QueryString["CandidateID"]) + "' target='_self'>" + exam.Name + "</a>; ";
                        }
                        else
                        {
                            tdAttemptedExams.InnerHtml += "<a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("frmexamhistory.aspx?CourseID=" + exam.CourseID.ToString() + " &ExamId=" + exam.ID.ToString()) + "' target='_self'>" + exam.Name + "</a>; ";
                           //-- NsqftdAttemptedExams.InnerHtml += "<a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("frmexamhistory.aspx?CourseID=" + exam.CourseID.ToString() + " &ExamId=" + exam.ID.ToString()) + "' target='_self'>" + exam.Name + "</a>; ";
                        }
                    }
                }
                tdAttemptedExams.InnerHtml = tdAttemptedExams.InnerHtml.Trim().Trim(';');
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

   

    protected Int32 GetTotalPassedAllTheoryModuleCount(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber, Int64 candidateID)
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


                //Int32 allTheoryModulesPassed = (from d in context.CourseExamApplicationDetails
                //                                join m in context.Modules on d.ModuleID equals m.ID
                //                                where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                //                                d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && d.ResultGradeID != 12 && d.ResultGradeID != 191
                //                                orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                //                                select m).Count();



               
                if (currentCourseID == 1)
                {

                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 1 || m.ModuleTypeID == 5) && d.ResultGradeID != 12 && d.ResultGradeID != 191
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();



                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 1 select m.ID).Distinct().ToList();
                        int TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).Count();

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 1 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                         List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR1.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                         List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 1 select m.ID).ToList();
                         List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                         List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                       if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        if (currentevisionNumber == 5)
                            return TotalPassedModuleR4toR5.Count();

                        //List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
                        var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        //List<int> PassedModuleInR52 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();
                        int countTotalPassedInOlevel = (TotalpasedinR6 + passedModuleinparityR5ToR6.Count());
                        //theorymodulecount = ((allTheoryModules + allBridgeModules) - countTotalPassedInOlevel);
                        theorymodulecount = countTotalPassedInOlevel;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (currentCourseID == 2)
                {

                    try{

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 1 || m.ModuleTypeID == 5) && d.ResultGradeID != 12 && d.ResultGradeID != 191
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                        //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                        //--List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 2 select m.ID).Distinct().ToList();
                        int TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).Count();

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 1 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();
                        
                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();
                        
                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        if (currentevisionNumber == 5)
                            return TotalPassedModuleR4toR5.Count();

                        List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
                        var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> PassedModuleInR52 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();
                        int countTotalPassedInAlevel = (TotalpasedinR6 + PassedModuleInR52.Count());
                        //theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel));
                        theorymodulecount = countTotalPassedInAlevel;
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            else if (currentCourseID == 3 )
                {
                 
                        
                        List<int> OmmitedModuleR2toR4 = new List<int> { 390, 389, 388, 387, 386, 385, 384, 383, 382, 391, 392, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 208, 227, 209, 210, 211, 212, 213, 215, 229, 230, 231, 202, 170, 171, 172, 173, 196, 174, 198 };

                        //Total passed modules in B-R4 
                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && d.ResultGradeID != 12 && d.ResultGradeID != 191
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                        //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                        List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 3 select m.ID).Distinct().ToList();
                        int TotalpasedinR5 = allModulR5.Intersect(passedTotalanyrevisonafterommiteR2toR4).Count();

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 1 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR1.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR1.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR2toR3 = passedModuleinparityR2ToR3.Concat(PassedModuleInR3).ToList();
                        if (currentevisionNumber == 3)
                            return TotalPassedModuleR2toR3.Count();

                        var passedModuleinparityR3ToR4 = (from s in context.Parities where TotalPassedModuleR2toR3.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR41 = PassedModuleInR4.Concat(passedModuleinparityR3ToR4).ToList();
                        if (currentevisionNumber == 4)
                            return PassedModuleInR41.Count();

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

                        
                        theorymodulecount = (TotalpasedinR5 + passedModuleinparityR4ToR5.Count() - countConditionalpassedmoduleinBR2toBER4 - countConditionalpassedmoduleinBR2toBER5);
                    
                }
                else if (currentCourseID == 4)
                {

                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 1 || m.ModuleTypeID == 5) && d.ResultGradeID != 12 && d.ResultGradeID != 191
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();



                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 1 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                     
                    }
                    catch { }
                }
               
                return theorymodulecount;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //change on dated 17072023
    protected List<int> GetTotalPassedforExemptofPract(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber, Int64 candidateID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                List<int> theorymodulecount =new List<int>();
                Int32 max_revision = (from p in context.Modules
                                      where p.CourseID == currentCourseID
                                      select p.RevisionNumber).Distinct().Max();
                Int32 registrationRevisionNumber = EConnect.NIELIT.CourseManager.GetCourseRevisionNumberAtRegistrationCommenced(context, currentCourseID, registrationNumber, candidateID);
                Int32 theory = Convert.ToInt32(enmModuleType.Theory);
                Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);


                //Int32 allTheoryModulesPassed = (from d in context.CourseExamApplicationDetails
                //                                join m in context.Modules on d.ModuleID equals m.ID
                //                                where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                //                                d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && d.ResultGradeID!=12 && d.ResultGradeID!=191
                //                                orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                //                                select m).Count();




                if (currentCourseID == 1)
                {

                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 1 || m.ModuleTypeID == 5) && d.ResultGradeID != 12 && d.ResultGradeID != 191
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();



                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 1 select m.ID).Distinct().ToList();
                        List<int> TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).ToList();

                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR1.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
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
                        //int countTotalPassedInOlevel = TotalpasedinR6 + passedModuleinparityR5ToR6;
                        //theorymodulecount = ((allTheoryModules + allBridgeModules) - countTotalPassedInOlevel);
                        theorymodulecount = passedModuleinparityR5ToR6.Concat(TotalpasedinR6).ToList();
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
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 1 || m.ModuleTypeID == 5) && d.ResultGradeID != 12 && d.ResultGradeID != 191
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                        //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                        //--List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 2 select m.ID).Distinct().ToList();
                        List<int> TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).ToList();

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                       

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 1 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                      

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                       

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
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
                       //-- int countTotalPassedInAlevel = (TotalpasedinR6 + PassedModuleInR52.Count());
                        //theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel));
                        theorymodulecount = (TotalpasedinR6.Concat( PassedModuleInR52)).ToList();
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

              
                return theorymodulecount;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //change on dated 18072023
    protected List<int> GetTotalPassedforExemptofPractwithExemptedModule(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber, Int64 candidateID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                List<int> theorymodulecount = new List<int>();
                Int32 max_revision = (from p in context.Modules
                                      where p.CourseID == currentCourseID
                                      select p.RevisionNumber).Distinct().Max();
                Int32 registrationRevisionNumber = EConnect.NIELIT.CourseManager.GetCourseRevisionNumberAtRegistrationCommenced(context, currentCourseID, registrationNumber, candidateID);
                Int32 theory = Convert.ToInt32(enmModuleType.Theory);
                Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);


                //Int32 allTheoryModulesPassed = (from d in context.CourseExamApplicationDetails
                //                                join m in context.Modules on d.ModuleID equals m.ID
                //                                where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                //                                d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && d.ResultGradeID!=12 && d.ResultGradeID!=191
                //                                orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                //                                select m).Count();




                if (currentCourseID == 1)
                {

                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 1 || m.ModuleTypeID == 5) && (d.ResultGradeID == 12 || d.ResultGradeID == 191)
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();



                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 1 select m.ID).Distinct().ToList();
                        List<int> TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).ToList();

                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();


                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR1.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();


                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
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
                        //int countTotalPassedInOlevel = TotalpasedinR6 + passedModuleinparityR5ToR6;
                        //theorymodulecount = ((allTheoryModules + allBridgeModules) - countTotalPassedInOlevel);
                        theorymodulecount = passedModuleinparityR5ToR6.Concat(TotalpasedinR6).ToList();
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
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 1 || m.ModuleTypeID == 5) && (d.ResultGradeID == 12 || d.ResultGradeID == 191)
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                        //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                        //--List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 2 select m.ID).Distinct().ToList();
                        List<int> TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).ToList();

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();


                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 1 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();


                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();


                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
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
                        //-- int countTotalPassedInAlevel = (TotalpasedinR6 + PassedModuleInR52.Count());
                        //theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel));
                        theorymodulecount = (TotalpasedinR6.Concat(PassedModuleInR52)).ToList();
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }


                return theorymodulecount;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected Int32 GetTotalExemptedAllTheoryModuleCount(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber, Int64 candidateID)
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

              if (currentCourseID == 2)
                 {

                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 1 || m.ModuleTypeID == 5) && (d.ResultGradeID == 12 || d.ResultGradeID == 191)
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                        //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                        //--List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 2 select m.ID).Distinct().ToList();
                        int TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).Count();

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();
                        
                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();
                        
                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        if (currentevisionNumber == 5)
                            return TotalPassedModuleR4toR5.Count();

                        List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
                        var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> PassedModuleInR52 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();
                        int countTotalPassedInAlevel = (TotalpasedinR6 + PassedModuleInR52.Count());
                        //theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel));
                        theorymodulecount = countTotalPassedInAlevel;
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (currentCourseID == 3)
                {

                    
                    List<int> OmmitedModuleR2toR4 = new List<int> { 390, 389, 388, 387, 386, 385, 384, 383, 382, 391, 392, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 208, 227, 209, 210, 211, 212, 213, 215, 229, 230, 231, 202, 170, 171, 172, 173, 196, 174, 198 };

                    //Total passed modules in B-R4 
                    List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                       join m in context.Modules on d.ModuleID equals m.ID
                                                       where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                       d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && (d.ResultGradeID == 12 || d.ResultGradeID == 191)
                                                       orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                       select d.ModuleID).ToList();


                    //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                    List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
                    List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 3 select m.ID).Distinct().ToList();
                    int TotalpasedinR5 = allModulR5.Intersect(passedTotalanyrevisonafterommiteR2toR4).Count();

                    List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 3 select m.ID).ToList();
                    List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                    if (currentevisionNumber == 0)
                        return PassedModuleInR0.Count();

                    List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 1 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                    List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 3 select m.ID).ToList();
                    List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                    List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                    if (currentevisionNumber == 1)
                        return PassedModuleInR11.Count();

                    List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                    List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 3 select m.ID).ToList();
                    List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                    List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                    if (currentevisionNumber == 2)
                        return PassedModuleInR21.Count();

                    List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                    List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 3 select m.ID).ToList();
                    List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                    List<int> TotalPassedModuleR2toR3 = passedModuleinparityR2ToR3.Concat(PassedModuleInR3).ToList();
                    if (currentevisionNumber == 3)
                        return TotalPassedModuleR2toR3.Count();

                    var passedModuleinparityR3ToR4 = (from s in context.Parities where TotalPassedModuleR2toR3.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                    List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 3 select m.ID).ToList();
                    List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                    List<int> PassedModuleInR41 = PassedModuleInR4.Concat(passedModuleinparityR3ToR4).ToList();
                    if (currentevisionNumber == 4)
                        return PassedModuleInR41.Count();

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

                   
                    theorymodulecount = (TotalpasedinR5 + passedModuleinparityR4ToR5.Count() - countConditionalpassedmoduleinBR2toBER4 - countConditionalpassedmoduleinBR2toBER5);

                }

                return theorymodulecount;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // this code added dated on 18072023

    protected Int32 GetTotalPractPassedThorughParityCount(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber, Int64 candidateID)
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

                if (currentCourseID == 1)
                {

                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 3) && d.ResultGradeID != 12
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                        //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                        //--List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 1 select m.ID).Distinct().ToList();
                        int TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).Count();

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        if (currentevisionNumber == 5)
                            return TotalPassedModuleR4toR5.Count();

                        //List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
                        var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        //List<int> PassedModuleInR52 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();
                        int countTotalPassedInAlevel = (TotalpasedinR6 + passedModuleinparityR5ToR6.Count());
                        //theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel));
                        theorymodulecount = countTotalPassedInAlevel;
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
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
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 3) && d.ResultGradeID != 12
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                        //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                        //--List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 2 select m.ID).Distinct().ToList();
                        int TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).Count();

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        if (currentevisionNumber == 5)
                            return TotalPassedModuleR4toR5.Count();

                        //List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
                        var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        //List<int> PassedModuleInR52 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();
                        int countTotalPassedInAlevel = (TotalpasedinR6 + passedModuleinparityR5ToR6.Count());
                        //theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel));
                        theorymodulecount = countTotalPassedInAlevel;
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (currentCourseID == 3)
                {


                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 3) && d.ResultGradeID != 12
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                      

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        theorymodulecount = TotalPassedModuleR4toR5.Count();
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (currentCourseID == 4)
                {


                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 3) && d.ResultGradeID != 12
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                        //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                        //--List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 4 select m.ID).Distinct().ToList();
                        int TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).Count();

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        if (currentevisionNumber == 5)
                            return TotalPassedModuleR4toR5.Count();

                        //List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
                        var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        //List<int> PassedModuleInR52 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();
                        int countTotalPassedInAlevel = (TotalpasedinR6 + passedModuleinparityR5ToR6.Count());
                        //theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel));
                        theorymodulecount = countTotalPassedInAlevel;
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                return theorymodulecount;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // this code added dated on 18072023

    protected Int32 GetTotalPractExemptThorughParityCount(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber, Int64 candidateID)
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

                if (currentCourseID == 1)
                {

                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 3) && d.ResultGradeID == 12
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                        //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                        //--List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 1 select m.ID).Distinct().ToList();
                        int TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).Count();

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        if (currentevisionNumber == 5)
                            return TotalPassedModuleR4toR5.Count();

                        //List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
                        var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        //List<int> PassedModuleInR52 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();
                        int countTotalPassedInAlevel = (TotalpasedinR6 + passedModuleinparityR5ToR6.Count());
                        //theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel));
                        theorymodulecount = countTotalPassedInAlevel;
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
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
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 3) && d.ResultGradeID == 12
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                        //--Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                        //--List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();

                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 2 select m.ID).Distinct().ToList();
                        int TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).Count();

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        if (currentevisionNumber == 5)
                            return TotalPassedModuleR4toR5.Count();

                        //List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
                        var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        //List<int> PassedModuleInR52 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();
                        int countTotalPassedInAlevel = (TotalpasedinR6 + passedModuleinparityR5ToR6.Count());
                        //theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel));
                        theorymodulecount = countTotalPassedInAlevel;
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (currentCourseID == 3)
                {


                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 3) && d.ResultGradeID == 12
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();




                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        theorymodulecount = TotalPassedModuleR4toR5.Count();
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (currentCourseID == 4)
                {


                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 3) && d.ResultGradeID == 12
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                        //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                        //--List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 4 select m.ID).Distinct().ToList();
                        int TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).Count();

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        if (currentevisionNumber == 5)
                            return TotalPassedModuleR4toR5.Count();

                        //List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
                        var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        //List<int> PassedModuleInR52 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();
                        int countTotalPassedInAlevel = (TotalpasedinR6 + passedModuleinparityR5ToR6.Count());
                        //theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel));
                        theorymodulecount = countTotalPassedInAlevel;
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                return theorymodulecount;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // this code added dated on 18072023

    protected Int32 GetTotalProjectpassedThorughParityCount(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber, Int64 candidateID)
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

                if (currentCourseID == 1)
                {

                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 4) && d.ResultGradeID != 12
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                        //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                        //--List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 1 select m.ID).Distinct().ToList();
                        int TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).Count();

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        if (currentevisionNumber == 5)
                            return TotalPassedModuleR4toR5.Count();

                        //List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
                        var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        //List<int> PassedModuleInR52 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();
                        int countTotalPassedInAlevel = (TotalpasedinR6 + passedModuleinparityR5ToR6.Count());
                        //theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel));
                        theorymodulecount = countTotalPassedInAlevel;
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
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
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 4) && d.ResultGradeID != 12
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                        //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                        //--List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 2 select m.ID).Distinct().ToList();
                        int TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).Count();

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        if (currentevisionNumber == 5)
                            return TotalPassedModuleR4toR5.Count();

                        //List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
                        var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        //List<int> PassedModuleInR52 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();
                        int countTotalPassedInAlevel = (TotalpasedinR6 + passedModuleinparityR5ToR6.Count());
                        //theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel));
                        theorymodulecount = countTotalPassedInAlevel;
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (currentCourseID == 3)
                {


                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 4) && d.ResultGradeID != 12
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();




                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        theorymodulecount = TotalPassedModuleR4toR5.Count();
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (currentCourseID == 4)
                {


                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 4) && d.ResultGradeID != 12
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                        

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();

                        theorymodulecount = TotalPassedModuleR3toR4.Count();
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                return theorymodulecount;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // this code added dated on 18072023

    protected Int32 GetTotalProjectExemptThorughParityCount(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber, Int64 candidateID)
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

                if (currentCourseID == 1)
                {

                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 4) && d.ResultGradeID == 12
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                        //Calcualt total Passed In Theroy B-R4 after remove module B-R4 omitted from B-R5(Max Passed Module count 10(Compulsory + elective))
                        //--List<int> passedTotalanyrevisonafterommiteR2toR4 = passedTotalanyrevison.Except(OmmitedModuleR2toR4).ToList();
                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 1 select m.ID).Distinct().ToList();
                        int TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).Count();

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 1 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        if (currentevisionNumber == 5)
                            return TotalPassedModuleR4toR5.Count();

                        //List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
                        var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        //List<int> PassedModuleInR52 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();
                        int countTotalPassedInAlevel = (TotalpasedinR6 + passedModuleinparityR5ToR6.Count());
                        //theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel));
                        theorymodulecount = countTotalPassedInAlevel;
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
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
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 4) && d.ResultGradeID == 12
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();


                    
                        List<int> allModulR6 = (from m in context.Modules where m.RevisionNumber == 6 && m.CourseID == 2 select m.ID).Distinct().ToList();
                        int TotalpasedinR6 = allModulR6.Intersect(passedTotalanyrevison).Count();

                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 2 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        if (currentevisionNumber == 5)
                            return TotalPassedModuleR4toR5.Count();

                        //List<int> praticalModuleListAR6 = new List<int> { 956, 957, 958, 959 };
                        var passedModuleinparityR5ToR6 = (from s in context.Parities where TotalPassedModuleR4toR5.Contains(s.OldModuleID) && s.NewRevisionNumber == 6 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        //List<int> PassedModuleInR52 = passedModuleinparityR5ToR6.Except(praticalModuleListAR6).ToList();
                        int countTotalPassedInAlevel = (TotalpasedinR6 + passedModuleinparityR5ToR6.Count());
                        //theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel));
                        theorymodulecount = countTotalPassedInAlevel;
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (currentCourseID == 3)
                {


                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 4) && d.ResultGradeID == 12
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();




                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();
                        if (currentevisionNumber == 4)
                            return TotalPassedModuleR3toR4.Count();

                        var passedModuleinparityR4ToR5 = (from s in context.Parities where TotalPassedModuleR3toR4.Contains(s.OldModuleID) && s.NewRevisionNumber == 5 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR5 = (from m in context.Modules where m.RevisionNumber == 5 && m.CourseID == 3 select m.ID).ToList();
                        List<int> PassedModuleInR5 = allModulR5.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR4toR5 = PassedModuleInR5.Concat(passedModuleinparityR4ToR5).ToList();
                        theorymodulecount = TotalPassedModuleR4toR5.Count();
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else if (currentCourseID == 4)
                {


                    try
                    {

                        List<int> passedTotalanyrevison = (from d in context.CourseExamApplicationDetails
                                                           join m in context.Modules on d.ModuleID equals m.ID
                                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                                           d.Grade.IsPassed == true && (m.ModuleTypeID == 4) && d.ResultGradeID == 12
                                                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                                                           select d.ModuleID).ToList();




                        List<int> allModulR0 = (from m in context.Modules where m.RevisionNumber == 0 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR0 = allModulR0.Intersect(passedTotalanyrevison).ToList();
                        if (currentevisionNumber == 0)
                            return PassedModuleInR0.Count();

                        List<int> passedModuleinparityR0ToR1 = (from s in context.Parities where PassedModuleInR0.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR1 = (from m in context.Modules where m.RevisionNumber == 1 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR1 = allModulR1.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR11 = PassedModuleInR1.Concat(passedModuleinparityR0ToR1).ToList();
                        if (currentevisionNumber == 1)
                            return PassedModuleInR11.Count();

                        List<int> passedModuleinparityR1ToR2 = (from s in context.Parities where PassedModuleInR11.Contains(s.OldModuleID) && s.NewRevisionNumber == 2 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR2 = (from m in context.Modules where m.RevisionNumber == 2 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR2 = allModulR2.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR21 = PassedModuleInR2.Concat(passedModuleinparityR1ToR2).ToList();
                        if (currentevisionNumber == 2)
                            return PassedModuleInR21.Count();

                        List<int> passedModuleinparityR2ToR3 = (from s in context.Parities where PassedModuleInR21.Contains(s.OldModuleID) && s.NewRevisionNumber == 3 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR3 = (from m in context.Modules where m.RevisionNumber == 3 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR3 = allModulR3.Intersect(passedTotalanyrevison).ToList();
                        List<int> PassedModuleInR31 = PassedModuleInR3.Concat(passedModuleinparityR2ToR3).ToList();
                        if (currentevisionNumber == 3)
                            return PassedModuleInR31.Count();

                        List<int> passedModuleinparityR3ToR4 = (from s in context.Parities where PassedModuleInR31.Contains(s.OldModuleID) && s.NewRevisionNumber == 4 orderby s.NewModuleID select s.NewModuleID).Distinct().ToList();
                        List<int> allModulR4 = (from m in context.Modules where m.RevisionNumber == 4 && m.CourseID == 4 select m.ID).ToList();
                        List<int> PassedModuleInR4 = allModulR4.Intersect(passedTotalanyrevison).ToList();
                        List<int> TotalPassedModuleR3toR4 = passedModuleinparityR3ToR4.Concat(PassedModuleInR4).ToList();

                        theorymodulecount = TotalPassedModuleR3toR4.Count();
                        // theorymodulecount = (((allTheoryModules + allBridgeModules) - countTotalPassedInAlevel - moduleno_nine_ten_TheoryModulesPassed_inrevision4));
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                return theorymodulecount;
            }
        }
        catch (Exception ex)
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

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                if (!String.IsNullOrEmpty(Request.QueryString["CandidateID"]))
                    hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl + "&CandidateID=" + Request.QueryString["CandidateID"]);
                else
                    hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);

                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = hl.NavigateUrl;

                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = hl.NavigateUrl;

                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = hl.NavigateUrl;

                HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl4.NavigateUrl = hl.NavigateUrl;

                HyperLink hl5 = (HyperLink)e.Row.Cells[6].Controls[0];
                hl5.NavigateUrl = hl.NavigateUrl;

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void BindRemainingModules()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var dataCourseStatus = (from m in context.RegistrationDetails where m.CourseID == currentCourseID && m.RegistrationNo == registrationNumber select m.RegistrationStatus).FirstOrDefault();
                //ICollection<Module> lstRemainingModules = CourseManager.GetRemainingModules(context, currentCourseID, registrationNumber, currentevisionNumber, entityID);
                //int currentevisionNumber = Convert.ToInt16((from m in context.Courses where m.ID == currentCourseID orderby m.CourseRevisions descending select m.CourseRevisions).FirstOrDefault());
                Int32 currentevisionNumber_new = currentCourseID == 1 ? 6 : currentCourseID == 2 ? 6 : currentCourseID == 3 ? 5 : currentCourseID == 4 ? 4 : currentevisionNumber;
                int course_catId = (from c in context.Courses where c.ID == currentCourseID select c.CourseCategoryID).FirstOrDefault();
                if (course_catId == 6)  // coded adde by abhi singh dated on 13052024 
                {
                    ICollection<Module> lstRemainingModules = GetRemainingModules_NsqFcandidate(context, currentCourseID, registrationNumber, currentevisionNumber_new, entityID, 0);
                    //  if (tdRemainingTheorygModules.InnerText == "0")
                    //    lstRemainingModules = lstRemainingModules.Where(d => d.ModuleTypeID != (Int32)enmModuleType.Theory).ToList();
                    //currentevisionNumber = GetCourseRevisionNumberAtRegistrationCompleted(context, currentCourseID, registrationNumber, entityID);
                   // div_O.Visible = false;
                   // notnsqfisummery.Visible = false;
                    exemptdetlswindow.Visible = false;
                  //  nsqfisummery.Visible = true;
                    uPnlNavigation.Visible = true;
                    string sup = "<sup>th</sup> Revision";
                    //string sup1 = "5.1";
                    if (currentevisionNumber_new == 1)
                        sup = "<sup>st</sup> Revision";
                    else if (currentevisionNumber_new == 2)
                        sup = "<sup>nd</sup> Revision";
                    if (currentevisionNumber_new == 3)
                        sup = "<sup>rd</sup> Revision";
                    if (currentevisionNumber_new == 6)
                    {
                        string currentevisionNumber1 = "5.1";
                        summeryHeading1.InnerHtml += " <a title='Click here to view/print list of modules' href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CourseModuleList.aspx?CourseId=" + currentCourseID.ToString() + "&RevisionId=" + currentevisionNumber_new.ToString()) + "' target='_blank'>" + currentevisionNumber1.ToString() + sup + "</a>)";

                    }
                    else
                    {
                        summeryHeading1.InnerHtml += " <a title='Click here to view/print list of modules' href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CourseModuleList.aspx?CourseId=" + currentCourseID.ToString() + "&RevisionId=" + currentevisionNumber_new.ToString()) + "' target='_blank'>" + currentevisionNumber_new.ToString() + sup + "</a>)";

                    }
                    if (lstRemainingModules != null && lstRemainingModules.Count != 0 && dataCourseStatus.Code != "P")
                    {
                        gvModules.Visible = true;
                        divgvModules.Visible = true;
                        var lst = lstRemainingModules.Select(m => new
                        {
                            CourseID = m.CourseID,
                            ID = m.ID,
                            name = m.Name,
                            Code = m.ShortName,
                            ModuleTypeID = m.ModuleTypeID,
                            MType = m.ModuleType.Name + (m.ModuleTypeID == 1 ? (m.SelectionTypeID == 1 ? " (Comp.)" : " (Elect.)") : ""),
                            doexam = "NA"
                        }).ToList();
                        //if (lst != null & lst.Count > 0)
                        //{
                        //    Sidelink.Items.Add(new SideLinkItem("Make Mercy Appeal", "../CAND/MercyAppeal.aspx?" + Request.QueryString.ToString(), "../images/Apply_Online.jpg"));
                        //    Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                        //    Sidelink.Render();
                        //}
                        gvModules.DataSource = lst;
                        gvModules.DataBind();
                    }
                    NSQFResultView();
                }
                else
                {
                    ICollection<Module> lstRemainingModules = GetRemainingModules(context, currentCourseID, registrationNumber, currentevisionNumber_new, entityID, 0);
                    //  if (tdRemainingTheorygModules.InnerText == "0")
                    //    lstRemainingModules = lstRemainingModules.Where(d => d.ModuleTypeID != (Int32)enmModuleType.Theory).ToList();
                    //currentevisionNumber = GetCourseRevisionNumberAtRegistrationCompleted(context, currentCourseID, registrationNumber, entityID);
                  //  div_O.Visible = true;
                   // notnsqfisummery.Visible = true;
                  //  nsqfisummery.Visible = false;
                    uPnlNavigation.Visible = false;
                    string sup = "<sup>th</sup> Revision";
                    //string sup1 = "5.1";
                    if (currentevisionNumber_new == 1)
                        sup = "<sup>st</sup> Revision";
                    else if (currentevisionNumber_new == 2)
                        sup = "<sup>nd</sup> Revision";
                    if (currentevisionNumber_new == 3)
                        sup = "<sup>rd</sup> Revision";
                    if (currentevisionNumber_new == 6)
                    {
                        string currentevisionNumber1 = "5.1";
                        summeryHeading1.InnerHtml += " <a title='Click here to view/print list of modules' href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CourseModuleList.aspx?CourseId=" + currentCourseID.ToString() + "&RevisionId=" + currentevisionNumber_new.ToString()) + "' target='_blank'>" + currentevisionNumber1.ToString() + sup + "</a>)";

                    }
                    else
                    {
                        summeryHeading1.InnerHtml += " <a title='Click here to view/print list of modules' href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CourseModuleList.aspx?CourseId=" + currentCourseID.ToString() + "&RevisionId=" + currentevisionNumber_new.ToString()) + "' target='_blank'>" + currentevisionNumber_new.ToString() + sup + "</a>)";

                    }
                    if (lstRemainingModules != null && lstRemainingModules.Count != 0 && dataCourseStatus.Code != "P")
                    {
                        gvModules.Visible = true;
                        divgvModules.Visible = true;
                        var lst = lstRemainingModules.Select(m => new
                        {
                            CourseID = m.CourseID,
                            ID = m.ID,
                            name = m.Name,
                            Code = m.ShortName,
                            ModuleTypeID = m.ModuleTypeID,
                            MType = m.ModuleType.Name + (m.ModuleTypeID == 1 ? (m.SelectionTypeID == 1 ? " (Comp.)" : " (Elect.)") : ""),
                            doexam = "NA"
                        }).ToList();
                        //if (lst != null & lst.Count > 0)
                        //{
                        //    Sidelink.Items.Add(new SideLinkItem("Make Mercy Appeal", "../CAND/MercyAppeal.aspx?" + Request.QueryString.ToString(), "../images/Apply_Online.jpg"));
                        //    Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
                        //    Sidelink.Render();
                        //}
                        gvModules.DataSource = lst;
                        gvModules.DataBind();
                    }
                }
            };
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
                string sql = "Select ID FROM Course_Exam_Exemption where Candidate_ID=@candidateID and Registration_Number=@registrationNumber and Exempted_Course_ID=@currentCourseID and Exempted_Revision_Number=@currentevisionNumber";

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

    Int32 UserTypeId = 0;  //--start-- added new code by abhi singh dated on 05062024 for nsqf result.

    protected void NSQFResultView()
    {
        try
        {
            using (DataTable dt = FillGridViewNSQFResultViewRecord())
            {
                if (dt.Rows.Count > 0)
                {
                    var CentreBatchs = (from p in dt.AsEnumerable()
                                        select new
                                        {
                                            Registration_no = p.Field<Int32>("Registration number"),
                                            NSQF_roll_no = p.Field<string>("Roll Number"),
                                            Module_Name = p.Field<string>("Module Name"),
                                            Result = p.Field<string>("Result"),
                                            Exam_Cycle = p.Field<string>("Exam Cycle"),
                                            Reslut_Declaration_Date = p.Field<string>("Result Declaration Date"),
                                            // Result_upload_date = p.Field<string>("Result_upload_date"),                                   
                                        });

                    PagingBar1.Bind(CentreBatchs, ref gvMainNSQResultView);
                    uPnlNavigation.Update();
                    PagingBar1.Visible = true;
                    gvMainNSQResultView.Visible = true;
                    //lblnsqfResult.Visible = false;  // not require
                }
                else
                {
                    //lblnsqfResult.Text = "NSQF Result not found."; // not require
                    PagingBar1.Visible = false;
                    gvMainNSQResultView.Visible = false;
                    // lblnsqfResult.Visible = true; // not require
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public DataTable FillGridViewNSQFResultViewRecord()
    {
        UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
        Int32 RegistrationNumber = 0;
        if (UserTypeId == 3)
        {
            string UserID;
            UserID = (Session["studentReg"]).ToString();
            RegistrationNumber = Convert.ToInt32(UserID);
        
        }
        
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("Show_NSQF_Result", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PReg_no", SqlDbType.Int).Value = RegistrationNumber;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMainNSQResultView.PageIndex = PagingBar1.CurrentPageIndex;
            NSQFResultView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void gvMainNSQResultView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                string href = hl.NavigateUrl;
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    // -- End --
    public static ICollection<Module> GetRemainingModules(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int32 currentRevisionNumber1, Int64 candidateID, int afterExmpt)
    {
        try
        {
            Int32 countPassedCodModule = 0;
            countPassedCodModule = PassedConditionalTheoryModuleCount1(courseID, registrationNumber, currentRevisionNumber1);
            Int32 countPassedCodModule_Alevel = 0;
            countPassedCodModule_Alevel = PassedConditionalTheoryModuleCount_Alevel(courseID, registrationNumber, currentRevisionNumber1, candidateID);
            Int32 moduleTypeTheory = Convert.ToInt32(enmModuleType.Theory);
            Int32 moduleTypeBridge = Convert.ToInt32(enmModuleType.Bridge);
            Int32 moduleTypePractical = Convert.ToInt32(enmModuleType.Practical);
            Int32 moduleTypeProject = Convert.ToInt32(enmModuleType.Project);
            Int32 registrationRevisionNumber = EConnect.NIELIT.CourseManager.GetCourseRevisionNumberAtRegistrationCommenced(context, courseID, registrationNumber, candidateID);
            ICollection<Module> listOfModulesPassed = EConnect.NIELIT.CourseManager.GetPassedModulesListOfAnyRevision(context, courseID, registrationNumber, candidateID).ToList();
            
           

            ICollection<Module> listOfModulesOfCurrentRevision = EConnect.NIELIT.CourseManager.GetModulesList(context, courseID, currentRevisionNumber1).ToList();

            List<Module> passedModulesOfCurrentRevision = new List<Module>();
            // Int32 PassedConditionalTheoryModuleCount1 = PassedConditionalTheoryModuleCount(courseID, registrationNumber, currentRevisionNumber);
            if (registrationRevisionNumber == currentRevisionNumber1)
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
                if (currentRevisionNumber1 == 6 && courseID == 1)
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

                else if (currentRevisionNumber1 == 6 && courseID == 2)
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
            }
            else
            {
                foreach (var mod in listOfModulesPassed.ToList())
                {
                    //Implement Elctive/Selective Rule
                    Int32 passedRevisionNumber = mod.RevisionNumber;
                    if (passedRevisionNumber == currentRevisionNumber1)
                    {
                        passedModulesOfCurrentRevision.Add(mod);
                    }
                    else
                    {
                        Int32 index = 0;
                        Int32 oldModuleID = mod.ID;
                        for (index = passedRevisionNumber; index < currentRevisionNumber1; index++)
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


                if (currentRevisionNumber1 == 6 && courseID == 1)
                {
                    if (listOfModulesPassed.Any(p => p.ID == 929) || listOfModulesPassed.Any(p => p.ID == 930) || listOfModulesPassed.Any(p => p.ID == 931) || listOfModulesPassed.Any(p => p.ID == 932))
                    {
                        Module moduleF = context.Modules.Find(715);
                        listOfModulesOfCurrentRevision.Remove(moduleF);
                    }
                }

                if (currentRevisionNumber1 == 6 && courseID == 2)
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


                if (currentRevisionNumber1 == 6 && courseID == 1)
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

                else if (currentRevisionNumber1== 6 && courseID == 2)
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
                                                  where el.ElectiveGroup == groupID && el.RevisionNumber == currentRevisionNumber1 && el.CourseID == courseID
                                                  select el.NumberOfElectiveModulesAllowed).Distinct().Sum();
                        if (modulesToBeCleared.HasValue)
                        {
                            Int32 moduleToBePassed = (Int32)modulesToBeCleared;
                            //Int32? modulesCleared = passedModulesOfCurrentRevision.Where(d => (d.ElectiveGroup == groupID && d.RevisionNumber == currentRevisionNumber && d.CourseID == courseID)).Select(d => d.NumberOfElectiveModulesAllowed).Distinct().Count();                            
                            Int32? modulesCleared = passedModulesOfCurrentRevision.Where(d => (d.ElectiveGroup == groupID && d.RevisionNumber == currentRevisionNumber1 && d.CourseID == courseID)).Select(d => d.ID).Distinct().Count();
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
            Int32 theoryModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber1, enmModuleType.Theory, enmSelectionType.Compulsory);
            //theoryModulesTobePassed += GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Bridge, null);
            Int32 theoryModulesPassed = passedModulesOfCurrentRevision.Where(d => (d.ModuleTypeID == moduleTypeTheory && d.SelectionTypeID == compulsory)).Count();
            if (theoryModulesPassed >= theoryModulesTobePassed)
                if (registrationRevisionNumber == currentRevisionNumber1)
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
            //if (courseID == 3 && currentRevisionNumber1 == 5 && countPassedCodModule > 0 && CheckExemptioDoneOrNot(courseID, registrationNumber, currentRevisionNumber1, candidateID) < 1)
            //{
            //    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 1143).ToList();
            //    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 1144).ToList();
            //}
            //--End--

            //--added code dated 26042023 for A Level (Removing specialized modeule from exemption list for those candidate who is eleigible for exempetion

            //if (courseID == 2 && countPassedCodModule_Alevel > 0 && CheckExemptioDoneOrNot(courseID, registrationNumber, currentRevisionNumber1, candidateID) < 1 && A1toA8passedmoduleCount(registrationNumber, candidateID) < 8)
            //{
            //    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 946).ToList();
            //    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 947).ToList();
            //    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 948).ToList();
            //    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 949).ToList();
            //    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 950).ToList();
            //    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 951).ToList();
            //    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 952).ToList();
            //    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 953).ToList();
            //    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 954).ToList();
            //    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 955).ToList();
            //}

            string BlevelCompltedted = (from m in context.RegistrationDetails where m.RegistrationNo == registrationNumber && m.CourseID == 3 select m.RegistrationStatusCode).FirstOrDefault();
            if (courseID == 4 && BlevelCompltedted == "P")
            {
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 425).ToList();
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 426).ToList();
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 427).ToList();
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ID != 428).ToList();
            }
            // --End--

            Int32 bridgeModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber1, enmModuleType.Bridge, null);
            Int32 bridgeModulesPassed = passedModulesOfCurrentRevision.Where(d => d.ModuleTypeID == moduleTypeBridge).Count();
            if (bridgeModulesPassed >= bridgeModulesTobePassed)
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ModuleTypeID != moduleTypeBridge).ToList();

            Int32 practicalModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber1, enmModuleType.Practical, null);
            Int32 practicalModulesPassed = passedModulesOfCurrentRevision.Where(d => d.ModuleTypeID == moduleTypePractical).Count();
            if (currentRevisionNumber1 != 5 && courseID != 3)
            {
                if (practicalModulesPassed >= practicalModulesTobePassed)
                    if (registrationRevisionNumber == currentRevisionNumber1)
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


            Int32 projectModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber1, enmModuleType.Project, null);
            Int32 projectModulesPassed = passedModulesOfCurrentRevision.Where(d => d.ModuleTypeID == moduleTypeProject).Count();
            //if (projectModulesPassed >= projectModulesTobePassed)
            //    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ModuleTypeID != moduleTypeProject).ToList();

            return listOfModulesOfCurrentRevision.Distinct().ToList();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public static ICollection<Module> GetRemainingModules_NsqFcandidate(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int32 currentRevisionNumber1, Int64 candidateID, int afterExmpt)
    {
        try
        {
            Int32 countPassedCodModule = 0;
            countPassedCodModule = PassedConditionalTheoryModuleCount1(courseID, registrationNumber, currentRevisionNumber1);
            Int32 countPassedCodModule_Alevel = 0;
            countPassedCodModule_Alevel = PassedConditionalTheoryModuleCount_Alevel(courseID, registrationNumber, currentRevisionNumber1, candidateID);
            Int32 moduleTypeTheory = Convert.ToInt32(enmModuleType.Theory);
            Int32 moduleTypeBridge = Convert.ToInt32(enmModuleType.Bridge);
            Int32 moduleTypePractical = Convert.ToInt32(enmModuleType.Practical);
            Int32 moduleTypeProject = Convert.ToInt32(enmModuleType.Project);
            Int32 registrationRevisionNumber = EConnect.NIELIT.CourseManager.GetCourseRevisionNumberAtRegistrationCommenced(context, courseID, registrationNumber, candidateID);
            ICollection<Module> listOfModulesPassed = GetPassedModulesListOfAnyRevisionInNSQF(context, courseID, registrationNumber, candidateID).ToList();



            ICollection<Module> listOfModulesOfCurrentRevision = EConnect.NIELIT.CourseManager.GetModulesList(context, courseID, currentRevisionNumber1).ToList();

            List<Module> passedModulesOfCurrentRevision = new List<Module>();
            // Int32 PassedConditionalTheoryModuleCount1 = PassedConditionalTheoryModuleCount(courseID, registrationNumber, currentRevisionNumber);
            if (registrationRevisionNumber == currentRevisionNumber1)
            {
                listOfModulesPassed.ToList().ForEach(s => listOfModulesOfCurrentRevision.Remove(s));
                passedModulesOfCurrentRevision = (List<Module>)listOfModulesPassed;
                
            }
            else
            {
                foreach (var mod in listOfModulesPassed.ToList())
                {
                    //Implement Elctive/Selective Rule
                    Int32 passedRevisionNumber = mod.RevisionNumber;
                    if (passedRevisionNumber == currentRevisionNumber1)
                    {
                        passedModulesOfCurrentRevision.Add(mod);
                    }
                    else
                    {
                        Int32 index = 0;
                        Int32 oldModuleID = mod.ID;
                        for (index = passedRevisionNumber; index < currentRevisionNumber1; index++)
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




                passedModulesOfCurrentRevision.ForEach(s => listOfModulesOfCurrentRevision.Remove(s));
            }
            Int32 elective = Convert.ToInt32(enmSelectionType.Elective);

            //this code will remove all modules of elective group whome any of the module has been passed by the candidate


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
                                              where el.ElectiveGroup == groupID && el.RevisionNumber == currentRevisionNumber1 && el.CourseID == courseID
                                              select el.NumberOfElectiveModulesAllowed).Distinct().Sum();
                    if (modulesToBeCleared.HasValue)
                    {
                        Int32 moduleToBePassed = (Int32)modulesToBeCleared;
                        //Int32? modulesCleared = passedModulesOfCurrentRevision.Where(d => (d.ElectiveGroup == groupID && d.RevisionNumber == currentRevisionNumber && d.CourseID == courseID)).Select(d => d.NumberOfElectiveModulesAllowed).Distinct().Count();                            
                        Int32? modulesCleared = passedModulesOfCurrentRevision.Where(d => (d.ElectiveGroup == groupID && d.RevisionNumber == currentRevisionNumber1 && d.CourseID == courseID)).Select(d => d.ID).Distinct().Count();
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


            Int32 compulsory = Convert.ToInt32(enmSelectionType.Compulsory);
            Int32 theoryModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber1, enmModuleType.Theory, enmSelectionType.Compulsory);
          
            Int32 theoryModulesPassed = passedModulesOfCurrentRevision.Where(d => (d.ModuleTypeID == moduleTypeTheory && d.SelectionTypeID == compulsory)).Count();
            if (theoryModulesPassed >= theoryModulesTobePassed)
                if (registrationRevisionNumber == currentRevisionNumber1)
                    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => ((d.ModuleTypeID == moduleTypeTheory ? d.SelectionTypeID != compulsory : 1 == 1))).ToList();

          

            Int32 bridgeModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber1, enmModuleType.Bridge, null);
            Int32 bridgeModulesPassed = passedModulesOfCurrentRevision.Where(d => d.ModuleTypeID == moduleTypeBridge).Count();
            if (bridgeModulesPassed >= bridgeModulesTobePassed)
                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ModuleTypeID != moduleTypeBridge).ToList();

            Int32 practicalModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber1, enmModuleType.Practical, null);
            Int32 practicalModulesPassed = passedModulesOfCurrentRevision.Where(d => d.ModuleTypeID == moduleTypePractical).Count();
            if (currentRevisionNumber1 != 5 && courseID != 3)
            {
                if (practicalModulesPassed >= practicalModulesTobePassed)
                    if (registrationRevisionNumber == currentRevisionNumber1)
                        listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ModuleTypeID != moduleTypePractical).ToList();
            }


            Int32 projectModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber1, enmModuleType.Project, null);
            Int32 projectModulesPassed = passedModulesOfCurrentRevision.Where(d => d.ModuleTypeID == moduleTypeProject).Count();
          

            return listOfModulesOfCurrentRevision.Distinct().ToList();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static IQueryable<Module> GetPassedModulesListOfAnyRevisionInNSQF(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int64 candidateID)
    {
        try
        {
            IQueryable<Module> passedModulesOfAllRevisions = (from ns in context.NSQFModuleCandidateMarks join m in context.Modules on ns.ModuleId equals m.ID where ns.RegistrationNo == registrationNumber && ns.CourseId == courseID && (ns.Result == "Pass" || ns.Result == "PASS") orderby m.RevisionNumber, m.Code select m);
            return passedModulesOfAllRevisions;
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
                                                         d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && (m.ID == 402 || m.ID == 404 || m.ID == 415 || m.ID == 417 || m.ID == 407 || m.ID == 54 || m.ID == 58 || m.ID == 66 || m.ID == 222 || m.ID == 225)
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

                List<int> A1toA8 = new List<int> { 946, 947, 948, 949, 950, 951, 952, 953, 954, 955 };
                List<int> TotalpasedinA1toA8 = TotalpasedinR6.Except(A1toA8).ToList();
                int countTotalPassedInA1toA8 = TotalpasedinA1toA8.Count();
                return countTotalPassedInA1toA8;
                //if (countTotalPassedInAlevel>=8)
                //    return 0;
                //else
                //    return 1;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void showsidelink()
    {
        try
        {
            Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            Sidelink1.SideLinkType = SideLinkItem.SideLinkType.DownloadLink;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 ccatId = (from c in context.Courses
                                where c.ID == courseID && c.ShowOnWeb == true
                                select new
                               {
                                 courcatID = c.CourseCategoryID
                               }).FirstOrDefault().courcatID;
                var dl = from d in context.Downloadables
                         where d.CourseID == courseID && d.ShowOnWeb == true
                         select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName };
                var d2 = (from d in context.Downloadables
                          where d.CourseID == null && d.CourseCategoryID == ccatId && d.ShowOnWeb == true
                          select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName }).Union(dl);
                var d3 = (from d in context.Downloadables
                          where d.CourseID == null && d.CourseCategoryID == null && d.ShowOnWeb == true
                          select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName }).Union(d2);
                foreach (var dnbl in d3.Distinct())
                {
                    Sidelink1.Items.Add(new SideLinkItem(dnbl.LinkName, "../Handlers/UploadedFileHandler.ashx?ID=" + dnbl.FileID.ToString(), "", "_blank"));
                }
                Sidelink1.Render();
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void gvModules_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void Popup()
    {
        try
        {
            tbl.Width = Unit.Percentage(100);
            tbl.CellPadding = 0;
            showTableheader();
            int i = 1;
            ImgBtnPopupFee.Enabled = true;
            ImgBtnPopupFee.ImageUrl = "~/images/popup1.jpg";
            ImgBtnPopupFee.ToolTip = "Click here to view Grade Legends.";
            using (EConnectContext context = new EConnectContext())
            {

                Int32[] notInGrades = { 8, 10};
                var grade = (from g in context.ResultGrades
                             where g.CourseCategoryID == 1 && !notInGrades.Contains(g.ID)
                             select new
                             {
                                 grade = g.Code,
                                 description = g.Description,
                                 legend1 = g.PercentageFrom,
                                 legend2 = g.PercentageTo
                             }).ToList();
                if (grade.Count() >= 0)
                {
                    foreach (var result in grade)
                    {
                        TableRow tr = new TableRow();
                        if (i % 2 == 0)
                            tr.CssClass = "gdalternate1";
                        else
                            tr.CssClass = "gdrow1";

                        TableCell tdRow = new TableCell();

                        tdRow.Text = result.grade.ToUpper();
                        tdRow.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow);

                        TableCell tdRow2 = new TableCell();

                        if (result.legend1 != null && result.legend1 > 0 && result.legend2 != null && result.legend2 > 0)
                            tdRow2.Text = result.legend1 + " to " + result.legend2;
                        else
                            tdRow2.Text = "-";
                        tdRow2.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow2);

                        TableCell tdRow1 = new TableCell();

                        tdRow1.Text = GetInitCap(result.description).ToString();
                        if (result.description.Length >= 18)
                        {
                            tdRow1.Text = result.description.ToString().Substring(0, 18) + "...";
                            tdRow1.ToolTip = result.description;
                        }
                        tdRow1.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(tdRow1);

                        tbl.Rows.Add(tr);
                        i++;
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void showTableheader()
    {
        try
        {
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(18);
            tcCol.Text = "Grade";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(44);
            tcCol2.Text = "Marks Range (in %)";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(38);
            tcCol1.Text = "Remarks";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }

}