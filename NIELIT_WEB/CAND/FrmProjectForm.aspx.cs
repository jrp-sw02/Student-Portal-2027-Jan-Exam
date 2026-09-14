using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data.Entity.SqlServer;
using System.Transactions;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text;
using System.Configuration;

public partial class CAND_FrmProjectForm : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentevisionNumber = 0;
    Int32 preevisionNumber = 0;
    Int32 currentCourseID = 0;
    Int64 registrationNumber = 0;
    Int32 applicantTypeID = 0;
    Int32[] regStatusID ={Convert.ToInt32(enmRegistrationStatus.Registered),
                                     Convert.ToInt32(enmRegistrationStatus.ReRegistered)                                     
                                     };
    Course currentCourse;
    //Exam attemptedLastExams;
    //IQueryable<Exam> lastExams;

    protected void Page_Load(object sender, EventArgs e)
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
            ////if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in") && (Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "http://nielit.gov.in"))
            ////{
            ////    Response.Write(GeInvalidRequestMessage("Goto Home Page", Server.MapPath("../Index.aspx")));
            ////    Response.End();
            ////    return;
            ////}
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            currentCourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            registrationNumber = Convert.ToInt64(Request.QueryString["RegNo"]);

            if (!IsPostBack)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    if (!context.Candidates.Any(c => (c.IsLocked == true && c.ID == entityID)))
                    {
                        lblerror.Text = "You can not apply for Project.<br>Your profile details are not completed/locked yet. Please first complete your profile details and lock it. <br> <a href=" + EConnect.Utils.Security.QuertStringModule.Encrypt("myprofile.aspx") + "> Click here to view profile</a>";
                        lblerror.Visible = true;
                        btnSave.Visible = false;
                        tblMain.Visible = false;
                        return;
                    }

                    currentCourse = context.Courses.Find(currentCourseID);
                    Int32 ShortermCoursesCategory = context.CourseCategories.Where(s => s.Code.ToUpper() == "STC").FirstOrDefault().ID;
                    lbllevel.Text = currentCourse.Name;
                    var registrationDetail = (from s in context.RegistrationDetails
                                              where s.CourseID == currentCourseID && s.CandidateID == entityID && s.RegistrationNo == registrationNumber
                                              select s).FirstOrDefault();

                    //  EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlCandidateType, typeof(enmApplicantType), null);
                    // if (ddlCandidateType.Items[0].Value == Convert.ToInt32(enmApplicantType.Direct).ToString())
                    //  ddlCandidateType.Items[0].Text = "As Direct Candidate";
                    // if (ddlCandidateType.Items[1].Value == Convert.ToInt32(enmApplicantType.Institute).ToString())
                    // ddlCandidateType.Items[1].Text = "Through Accredited Institute";

                    ddlCandidateType.Items.Clear();

                    // Create a new ListItem for "Through Accredited Institute" and add it to the dropdown list
                    ListItem instituteItem = new ListItem("Through Accredited Institute", Convert.ToInt32(enmApplicantType.Institute).ToString());
                    ddlCandidateType.Items.Add(instituteItem);

                    if (currentCourseID == 1213)
                    {
                        if (registrationDetail != null)
                        {
                            //Declaration

                            chmExam.Visible = true;
                            chmPay.Visible = true;

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
                            BindGridView_modules_fill();
                        }
                        else
                        {
                            lblerror.Text = "You are not eligible to apply for Project this time.";
                            lblerror.Visible = true;
                            btnSave.Visible = false;
                        }

                        // StringBuilder applicationname = new StringBuilder();
                        //var examstatus = (from r in context.CourseProjectApplications
                        //                  join d in context.DemandNotes
                        //                  on r.DemandNoteID equals d.ID
                        //                  where r.RegistrationNumber == registrationNumber && r.CandidateID == entityID && r.CourseID == currentCourseID
                        //                  && r.FinalSubmitted == true
                        //                  select new
                        //                  {
                        //                      appno = r.ID,
                        //                      applicationnumber = r.Number,
                        //                      coursename = r.Course.Name,
                        //                      courseid = r.CourseID,
                        //                      RegistrationNumber = r.RegistrationNumber,
                        //                      candidateid = r.CandidateID,
                        //                  }).ToList();

                        //if (examstatus.Count > 0)
                        //{
                        //    foreach (var names in examstatus)
                        //    {
                        //        applicationname.Append("<a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("ProjectFormPreview.aspx?candidateID=" + names.candidateid + "&Appid=" + names.appno.ToString()) + "' target='_blank' >" + names.applicationnumber + "</a>" + ",");
                        //        tdformstatus.InnerHtml = applicationname.ToString().TrimEnd(',');
                        //    }
                        //}
                        //else
                        //{
                        //    tdformstatus.InnerHtml = "Not Available";
                        //}
                    }




                   //if (registrationDetail != null)
                    else if (registrationDetail != null && currentCourseID != 1213)
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
                        //BindGridView_passedmodules();
                        BindGridView_modules_fill();
                        if (regStatusID.Contains(registrationDetail.RegistrationStatusID.Value) && registrationDetail.ValidUptoDate >= DateTime.Now.Date)
                        {
                            //exam.ID
                            if (CourseManager.IsAlreadyApplied(context, 3637, currentCourseID, registrationNumber, entityID) == max_revision)
                            {
                                currentevisionNumber = max_revision; ;
                                preevisionNumber = currentevisionNumber - 1;
                            }

                            else if (CourseManager.IsAlreadyApplied(context, 3637, currentCourseID, registrationNumber, entityID) != max_revision)
                            {
                                if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                {
                                    if (CourseManager.IsAlreadyApplied(context, 3637, currentCourseID, registrationNumber, entityID) == 0)
                                    {
                                        currentevisionNumber = max_revision;
                                        preevisionNumber = currentevisionNumber - 1;
                                    }
                                    else if (CourseManager.IsAlreadyApplied(context, 3637, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                    {
                                        currentevisionNumber = CourseManager.IsAlreadyApplied(context, 3637, currentCourseID, registrationNumber, entityID);
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
                            //-------------------------------To show previous project details------------------------------
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
                                }
                            }
                            else
                            {
                                tdformstatus.InnerHtml = "Not Available";
                            }
                            //-------------------------------------------------------    
                            //Exam exam = CourseManager.GetNextExam(context, currentCourseID, applicantTypeID);
                            //if (exam != null)
                            //{
                            //    if (context.ExamTimeTables.Any(t => t.CourseID == currentCourseID && t.ExamID == exam.ID))
                            //    {
                            //lblExamName.Text = exam.Name.ToUpper();
                            //lblExamName1.Text = exam.Name.ToUpper();
                            //lastExams = CourseManager.GetListOfAttemptedExams(context, currentCourseID, registrationNumber, entityID).Where(c => c.ID != exam.ID);
                            //attemptedLastExams = lastExams.OrderByDescending(d => new { d.ExamYear, d.ExamMonth }).FirstOrDefault();
                            //if (attemptedLastExams != null)
                            //    lblPreviousExam.Text = attemptedLastExams.Name.ToUpper();                                    

                            //Exam Center
                            //var examcentre = (from p in context.ExamCenters
                            //                  join c in context.ExamWiseExamCenters on p.ID equals c.ExamCenterID
                            //                  join s in context.Locations on p.StateID equals s.ID
                            //                  where (p.CourseCategoryID == currentCourse.CourseCategoryID || p.CourseID == currentCourseID) &&
                            //                  c.ExamID == exam.ID
                            //                  select new { ValueField = s.ID, TextField = s.Name.ToUpper() }).Distinct().OrderBy(s => s.TextField);                     

                            //Fee Dataisl
                            //lblProjectFee.Text = GetProjectFee(currentCourseID, enmFeeType.ExaminationFee).ToString("F");                                   
                            //lblProcessingFee.Text = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PostageFeeChargedTowardExaminationCorrespondence).ToString("F");
                            //lblTotalProcessingFee.Text = lblProcessingFee.Text;                                   

                            //FillRemianingModules(currentCourseID, registrationNumber, entityID, applicantTypeID);

                            //if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                            //{
                            //    if (CourseManager.IsCourseReviesd(currentCourseID))
                            //    {
                            //        if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == max_revision ||
                            //            CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                            //        {                                                
                            //            //trModuleOption.Visible = true;
                            //            //trModuleHead.Visible = true;
                            //            //trModuleOption1.Visible = true;
                            //        }

                            //        else
                            //        {                                               
                            //            //trModuleOption.Visible = false;
                            //            //trModuleHead.Visible = false;
                            //            //trModuleOption1.Visible = false;
                            //            if (CourseManager.IsAlreadyApplied(context, exam.ID, currentCourseID, registrationNumber, entityID) == 0)
                            //            {

                            //            }
                            //        }
                            //    }
                            //    else
                            //    {
                            //        if (lblerror.Text == "You can not apply for exam as all theory/bridge/practical modules have been passed."
                            //            || lblerror.Text == "You have already applied for this exam.")
                            //        {                                                
                            //            //trModuleOption.Visible = false;
                            //            //trModuleHead.Visible = false;
                            //            //trModuleOption1.Visible = false;
                            //        }
                            //        else
                            //        {                                               
                            //            //trModuleOption.Visible = true;
                            //            //trModuleHead.Visible = true;
                            //            //trModuleOption1.Visible = true;
                            //        }
                            //    }
                            //}
                            //else
                            //{                                        
                            //    //trModuleOption.Visible = true;
                            //    //trModuleHead.Visible = true;
                            //    //trModuleOption1.Visible = true;
                            //}

                            if (context.CourseProjectApplications.Any(c => (c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber)))
                            {
                                Int64 applId = 0;
                                if (context.CourseProjectApplications.Any(c => (c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber && c.FinalSubmitted == false)) == true)
                                {
                                    if (Request.QueryString["Appid"] == null)
                                    {

                                        lblerror.Text = "You have already applied for this Project but not submitted finally.<br>Please change your exam details (if you want to change ) and click on Update button to view the preview. ";
                                        chkdisclamier.Checked = true;

                                        if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                        {
                                            if (CourseManager.IsCourseReviesd(currentCourseID))
                                            {
                                                if (CourseManager.IsAlreadyApplied(context, 3637, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                    CourseManager.IsAlreadyApplied(context, 3637, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                {
                                                    //trModuleOption.Visible = true;
                                                    //trModuleHead.Visible = true;
                                                    //trModuleOption1.Visible = true;
                                                }
                                                else
                                                {
                                                    //trModuleOption.Visible = false;
                                                    //trModuleHead.Visible = false;
                                                    //trModuleOption1.Visible = false;
                                                }
                                            }
                                            else
                                            {
                                                // trModuleOption.Visible = true;
                                                //trModuleHead.Visible = true;
                                                //trModuleOption1.Visible = true;
                                            }
                                        }
                                        else
                                        {
                                            //trModuleOption.Visible = true;
                                            //trModuleHead.Visible = true;
                                            //trModuleOption1.Visible = true;
                                        }

                                    }
                                    else
                                    {
                                        btnback.Visible = false;
                                        lblerror.Text = "You can change your Project details. Please click on Update button after making changes.";

                                        if (!CourseManager.IsNewRegisteredCandidate(currentCourseID, registrationNumber))
                                        {
                                            if (CourseManager.IsCourseReviesd(currentCourseID))
                                            {
                                                if (CourseManager.IsAlreadyApplied(context, 3637, currentCourseID, registrationNumber, entityID) == max_revision ||
                                                     CourseManager.IsAlreadyApplied(context, 3637, currentCourseID, registrationNumber, entityID) == (max_revision - 1))
                                                {
                                                    //trModuleOption.Visible = true;
                                                    //trModuleHead.Visible = true;
                                                    //trModuleOption1.Visible = true;
                                                }
                                                else
                                                {
                                                    //trModuleOption.Visible = false;
                                                    //trModuleHead.Visible = false;
                                                    //trModuleOption1.Visible = false;
                                                }
                                            }
                                            else
                                            {
                                                //trModuleOption.Visible = true;
                                                //trModuleHead.Visible = true;
                                                //trModuleOption1.Visible = true;
                                            }
                                        }
                                        else
                                        {
                                            // trModuleOption.Visible = true;
                                            //trModuleHead.Visible = true;
                                            // trModuleOption1.Visible = true;
                                        }
                                    }
                                    lblerror.Visible = true;
                                    applId = context.CourseProjectApplications.Where(c => (c.CourseID == currentCourseID && c.CandidateID == entityID
                                    && c.RegistrationNumber == registrationNumber && c.FinalSubmitted == false)).FirstOrDefault().ID;
                                    ShowApplicationData(applId);
                                }
                                else if (context.CourseProjectApplications.Any(c => (c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber && c.FinalSubmitted == true && c.PaymentStatusID == 1 && c.WhetherDemandNoteCanceled == false)) == true)
                                {
                                    lblerror.Visible = true;
                                    applId = context.CourseProjectApplications.Where(c => (c.CourseID == currentCourseID && c.CandidateID == entityID
                                    && c.RegistrationNumber == registrationNumber && c.FinalSubmitted == true && c.PaymentStatusID == 1 && c.WhetherDemandNoteCanceled == false)).FirstOrDefault().ID;
                                    ShowApplicationData(applId);
                                }
                            }
                            //}
                            //else
                            //{
                            //    lblerror.Text = "Time table not found for " + exam.Name + " exam. You can not apply for exam at this time.";
                            //    lblerror.Visible = true;
                            //    btnSave.Visible = false;
                            //}
                            //}
                            //else
                            //{
                            //    lblerror.Text = "No next exam found. You can not apply for exam at this time.";
                            //    lblerror.Visible = true;
                            //    btnSave.Visible = false;
                            //}
                        }
                        else
                        {
                            lblerror.Text = "You are not eligible to apply for Project as your course status is completed/expired/project pending/cancelled.";
                            lblerror.Visible = true;
                            btnSave.Visible = false;
                            tblMain.Visible = false;
                        }
                    }
                    else
                    {
                        lblerror.Text = "You are not eligible to apply for Project this time.";
                        lblerror.Visible = true;
                        btnSave.Visible = false;
                    }
                };
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //protected void FillRemianingModules(Int32 courseID, Int64 registrationNumber, Int64 candidateID, Int32 applicantTypeID)
    //{
    //    using (EConnectContext context = new EConnectContext())
    //    {
    //        Exam exam = CourseManager.GetNextExam(context, currentCourseID, applicantTypeID);       

    //            ICollection<Module> modules = CourseManager.GetRemainingModules(context, courseID, registrationNumber, currentevisionNumber, entityID);
    //            if (modules.Count() >= 0)
    //            {
    //                Int32 project = Convert.ToInt32(enmModuleType.Project);

    //                Int32 max_revision = (from p in context.Modules
    //                                      where p.CourseID == currentCourseID
    //                                      select p.RevisionNumber).Distinct().Max();

    //                Int32 mod = currentevisionNumber % 10;
    //                if (mod == 1 || (mod > 3 && mod < 11))
    //                    lblRevision.Text = currentevisionNumber.ToString() + "<sup>th</sup> Revision) ";
    //                else if (mod == 2)
    //                    lblRevision.Text = currentevisionNumber.ToString() + "<sup>nd</sup> Revision) ";
    //                else if (mod == 3)
    //                    lblRevision.Text = currentevisionNumber.ToString() + "<sup>rd</sup> Revision) ";                   

    //                Int32 compulsory = Convert.ToInt32(enmSelectionType.Compulsory);                   
    //                    modules = modules.Where(c => c.ModuleTypeID == (Int32)enmModuleType.Project).ToList();

    //                var ModuleListProject = modules.Where(m => m.ModuleTypeID == project ).Select(m => new { ValueField = m.ID, TextField = (m.ShortName + " &nbsp;&nbsp;" + m.Name)}).ToList();
    //                EConnect.Utils.Common.ControlUtility.BindListObject(chklistProject, ModuleListProject);                     

    //            }           
    //    };

    //}

    protected void btnSave_Click(object sender, EventArgs e)
    {
        CourseProjectApplication appl = new CourseProjectApplication();
        try
        {
            //ValidateTheoryModules();
            ////ValidatePracticalModules();
            //Int32 TotalTheoryFee = 0;            
            //Int32 theoryCount = 0;
            //Int32 practicalCount = 0;
            //Int32 feeProcessing = 0;

            //validating exam centre selection     
            using (TransactionScope scope = new TransactionScope())
            {

                using (EConnectContext context = new EConnectContext())
                {
                    RegistrationDetail registrationDetail = (from s in context.RegistrationDetails
                                                             where s.CourseID == currentCourseID && s.CandidateID == entityID && s.RegistrationNo == registrationNumber
                                                             select s).FirstOrDefault();
                    //Exam currentExam = CourseManager.GetNextExam(context, currentCourseID, registrationDetail.ApplicantTypeID);
                    //Int32 feeTheory = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee);

                    //feeProcessing = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PostageFeeChargedTowardExaminationCorrespondence);
                    //if (Request.QueryString["Appid"] == null && context.CourseExamApplications.Any(c => (c.ExamID == currentExam.ID && c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber)) == false)
                    if (Request.QueryString["Appid"] == null)
                    {
                        if (context.CourseProjectApplications.Any(c => (c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber && c.FinalSubmitted == false)) == true)
                        {
                            Int64 applID = 0;
                            if (Request.QueryString["Appid"] != null)
                            {
                                applID = Convert.ToInt64(Request.QueryString["Appid"]);
                            }
                            else if (Request.QueryString["Appid"] == null)
                            {
                                applID = context.CourseProjectApplications.Where(c => (c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber && c.FinalSubmitted == false)).FirstOrDefault().ID;
                            }

                            appl = context.CourseProjectApplications.Find(applID);
                            appl.ApplicationDate = DateTime.Now;
                            appl.FeeAmount = Convert.ToDecimal(lblTotalFee.Text);
                            appl.NumberOfProjectModulesApplied = Convert.ToInt32(lblProjectCount.Text.Trim());
                            if (registrationDetail.enmApplicantType == enmApplicantType.Institute)
                                appl.InstituteID = registrationDetail.InstituteID;

                            // added on 05-04-2024
                           /* Commented on 21 May 2024 if ((Convert.ToInt32(ddlCandidateType.SelectedValue) == 1 && Convert.ToInt32(ddlPaymentOption.SelectedValue) == 1) || (Convert.ToInt32(ddlCandidateType.SelectedValue) == 2 && Convert.ToInt32(ddlPaymentOption.SelectedValue) == 1))
                            {
                                appl.ApplicationStatusID = 1;
                            }
                            if ((Convert.ToInt32(ddlCandidateType.SelectedValue) == 1 && Convert.ToInt32(ddlPaymentOption.SelectedValue) == 2) || (Convert.ToInt32(ddlCandidateType.SelectedValue) == 2 && Convert.ToInt32(ddlPaymentOption.SelectedValue) == 2))
                            {
                                appl.ApplicationStatusID = 4;
                                appl.PaymentSourceID = 2;
                            }*/

				            //Added on 21-05-2024
		                    if (Convert.ToInt32(ddlCandidateType.SelectedValue) == 1)
                            {
                                appl.ApplicationStatusID = 1;
                            }
                            if (Convert.ToInt32(ddlCandidateType.SelectedValue) == 2 && currentCourseID == 1213)
                            {
                                appl.ApplicationStatusID = 4;
                            }

				            //till here
                            context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();

                            //context.Database.ExecuteSqlCommand("Delete from Course_Project_Application_Detail where Course_Exam_Appl_ID = " + applID);

                            //November_2024
                            SqlParameter[] para1 = { new SqlParameter("@applID", applID) 
                                                        //new SqlParameter("@applID", SqlDbType.BigInt) { Value = applID }
                                                             };
                            context.Database.ExecuteSqlCommand("Delete from Course_Project_Application_Detail where Course_Exam_Appl_ID = @applID ", para1);
                            context.SaveChanges();
                        }
                        else if (context.CourseProjectApplications.Any(c => (c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber)) == false
                                || context.CourseProjectApplications.Any(c => (c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber && c.FinalSubmitted == true)) == true)
                        {
                            appl.ApplicantTypeID = registrationDetail.ApplicantTypeID;
                            appl.ApplicationDate = DateTime.Now;
                            appl.CandidateID = entityID;
                            appl.CourseCategoryID = registrationDetail.CourseCategoryID;
                            appl.CourseID = registrationDetail.CourseID;

                            //appl.ExamID = currentExam.ID;
                            appl.FeeAmount = Convert.ToDecimal(lblTotalFee.Text);
                            appl.FeeTypeID = Convert.ToInt32(enmFeeType.ProjectEvaluationFee);
                            appl.RegistrationNumber = registrationNumber;
                            appl.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);


                            //Fee Detail
                            appl.NumberOfProjectModulesApplied = Convert.ToInt32(lblProjectCount.Text.Trim());
                            if (registrationDetail.enmApplicantType == enmApplicantType.Institute)
                                appl.InstituteID = registrationDetail.InstituteID;
                            //if (Convert.ToInt32(ddlCandidateType.SelectedValue) == 1 && Convert.ToInt32(ddlPaymentOption.SelectedValue)== 2)
                            //{
                            //    appl.ApplicationStatusID = 1;
                            //}
                            //if (Convert.ToInt32(ddlCandidateType.SelectedValue) == 2)
                            //{
                            //    appl.ApplicationStatusID = 4;
                            //    appl.PaymentSourceID = 2;
                            //}
                            /* Commented on 14 May 2024 for CHM Project if ((Convert.ToInt32(ddlCandidateType.SelectedValue) == 1 && Convert.ToInt32(ddlPaymentOption.SelectedValue) == 1) || (Convert.ToInt32(ddlCandidateType.SelectedValue) == 2 && Convert.ToInt32(ddlPaymentOption.SelectedValue) == 1))
                             {
                                 appl.ApplicationStatusID = 1;
                             }
                             if ((Convert.ToInt32(ddlCandidateType.SelectedValue) == 1 && Convert.ToInt32(ddlPaymentOption.SelectedValue) == 2) || (Convert.ToInt32(ddlCandidateType.SelectedValue) == 2 && Convert.ToInt32(ddlPaymentOption.SelectedValue) == 2))
                             {
                                 appl.ApplicationStatusID = 4;
                                 appl.PaymentSourceID = 2;
                             }*/
                            //Added for CHM(T) Project 14 May 2024
                            if (currentCourseID == 1213)
                            {
                                appl.ApplicationStatusID = 4;
                                appl.PaymentSourceID = 2;
                            }
                            else
                            {
                                appl.ApplicationStatusID = 1;
                            }

                            //appl.NumberOfPracticalModulesApplied = practicalCount;
                            //attemptedLastExams = CourseManager.GetListOfAttemptedExams(context, currentCourseID, registrationNumber, entityID).OrderByDescending(d => new { d.ExamYear, d.ExamMonth }).FirstOrDefault();
                            //if (attemptedLastExams != null)
                            //    appl.PreviousExamID = attemptedLastExams.ID;
                            context.CourseProjectApplications.Add(appl);
                            context.SaveChanges();
                        }
                        else
                        {
                            Int64 applID = 0;
                            if (Request.QueryString["Appid"] != null)
                            {
                                applID = Convert.ToInt64(Request.QueryString["Appid"]);
                            }
                            else if (Request.QueryString["Appid"] == null)
                            {
                                applID = context.CourseProjectApplications.Where(c => (c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber && c.FinalSubmitted == false)).FirstOrDefault().ID;
                            }

                            appl = context.CourseProjectApplications.Find(applID);
                            appl.ApplicationDate = DateTime.Now;
                            appl.FeeAmount = Convert.ToDecimal(lblTotalFee.Text);
                            appl.NumberOfProjectModulesApplied = Convert.ToInt32(lblProjectCount.Text.Trim());
                            context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                            if (registrationDetail.enmApplicantType == enmApplicantType.Institute)
                                appl.InstituteID = registrationDetail.InstituteID;
                            if (Convert.ToInt32(ddlCandidateType.SelectedValue) == 1)
                            {
                                appl.ApplicationStatusID = 1;
                            }
                            if (Convert.ToInt32(ddlCandidateType.SelectedValue) == 2 && currentCourseID ==1213)
                            {
                                appl.ApplicationStatusID = 4;
                            }

                            context.SaveChanges();

                            //context.Database.ExecuteSqlCommand("Delete from Course_Project_Application_Detail where Course_Exam_Appl_ID = " + applID);

                            //November_2024
                            SqlParameter[] para2 = { new SqlParameter("@applID", applID)
                                                        //new SqlParameter("@applID", SqlDbType.BigInt) { Value = applID }
                                                             };
                            context.Database.ExecuteSqlCommand("Delete from Course_Project_Application_Detail where Course_Exam_Appl_ID = @applID ", para2);
                            context.SaveChanges();
                        }

                        context.SaveChanges();
                    }
                    else
                    {
                        Int64 applID = 0;
                        if (Request.QueryString["Appid"] != null)
                        {
                            applID = Convert.ToInt64(Request.QueryString["Appid"]);
                        }
                        else if (Request.QueryString["Appid"] == null)
                        {
                            applID = context.CourseProjectApplications.Where(c => (c.CourseID == currentCourseID && c.CandidateID == entityID && c.RegistrationNumber == registrationNumber)).FirstOrDefault().ID;
                        }

                        appl = context.CourseProjectApplications.Find(applID);
                        appl.ApplicationDate = DateTime.Now;
                        appl.FeeAmount = Convert.ToDecimal(lblTotalFee.Text);
                        appl.NumberOfProjectModulesApplied = Convert.ToInt32(lblProjectCount.Text.Trim());
                        context.Entry(appl).State = System.Data.Entity.EntityState.Modified;

                        // added on 05-04-2024 
                        if (registrationDetail.enmApplicantType == enmApplicantType.Institute)
                            appl.InstituteID = registrationDetail.InstituteID;
                        if (Convert.ToInt32(ddlCandidateType.SelectedValue) == 1)
                        {
                            appl.ApplicationStatusID = 1;
                        }
                        if (Convert.ToInt32(ddlCandidateType.SelectedValue) == 2 && currentCourseID ==1213)
                        {
                            appl.ApplicationStatusID = 4;
                        }
                        // added on 05-04-2024 
                        context.SaveChanges();

                        //context.Database.ExecuteSqlCommand("Delete from Course_Project_Application_Detail where Course_Exam_Appl_ID = " + applID);

                        //November_2024
                        SqlParameter[] para3 = { /*new SqlParameter("@applID", SqlDbType.BigInt) { Value = applID } */
                                                        new SqlParameter("@applID", applID)
                                                             };
                        context.Database.ExecuteSqlCommand("Delete from Course_Project_Application_Detail where Course_Exam_Appl_ID = @applID ", para3);
                        context.SaveChanges();
                    }
                    foreach (GridViewRow gv in GridView_project.Rows)
                    {
                        int rowIndex = gv.RowIndex;
                        System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gv.FindControl("cb_selection");
                        if (chk.Checked == true)
                        {
                            CourseProjectApplicationDetail module = new CourseProjectApplicationDetail();
                            module.CandidateID = entityID;
                            module.CourseExamApplicationID = appl.ID;
                            module.CourseID = currentCourseID;
                            //module.ExamID = appl.ExamID;
                            module.ExamMonth = DateTime.Now.Month;
                            module.ExamYear = DateTime.Now.Year;
                            module.FeeAmount = Convert.ToDecimal(gv.Cells[3].Text);
                            //TotalTheoryFee = lblTotalFee.Text;
                            if (registrationDetail.enmApplicantType == enmApplicantType.Institute)
                                module.InstituteID = registrationDetail.InstituteID;
                            //Added 17 May 2024 for CHMTOlevelproject

                            System.Web.UI.WebControls.TextBox txtProjectTitle = (System.Web.UI.WebControls.TextBox)gv.FindControl("txtProjectTitle");
                            string projectTitle = txtProjectTitle.Text;
                            if (string.IsNullOrEmpty(projectTitle))
                            {
                                ShowAlert("Please fill the Project Title");
                                return;
                            }

                            module.ProjectTitle = projectTitle;

                            //module.ModuleID = Convert.ToInt32(theHiddenField.Value);
                            module.ModuleID = Convert.ToInt32(GridView_project.DataKeys[rowIndex].Values[0]);
                            module.RegistrationNumber = registrationDetail.RegistrationNo;
                            context.CourseProjectApplicationDetails.Add(module);
                        }
                    }

                    context.SaveChanges();
                };
                if (lblProjectCount.Text == "0")
                {
                    throw new Exception("Please select at least one Project module to apply.");
                }
                scope.Complete();
            };
            using (EConnectContext context = new EConnectContext())
            {
                appl = context.CourseProjectApplications.Find(appl.ID);

                appl.FeeAmount = Convert.ToDecimal(lblTotalFee.Text);
                appl.NumberOfProjectModulesApplied = Convert.ToInt32(lblProjectCount.Text.Trim());
                context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("ProjectFormPreview.aspx?AppID=" + appl.ID.ToString()), true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    //protected void UpdateFeeDetails()
    //{
    //    try
    //    {            
    //        Int32 ProjectCount = 0;
    //        foreach (ListItem item in chklistProject.Items)
    //        {
    //            if (item.Selected == true)
    //                ProjectCount++;
    //        }
    //        //selectedModules.Value = ProjectCount.ToString();
    //        lblProjectCount.Text = ProjectCount.ToString();
    //        lblTotalProjectFee.Text = (Convert.ToSingle(lblProjectFee.Text) * ProjectCount).ToString("F");

    //        lblTotalFee.Text = Convert.ToSingle(lblTotalProjectFee.Text).ToString("F");            

    //        lblAmountHindi.Text = EConnect.Utils.Conversion.ConversionUtility.NumberToText(lblTotalFee.Text, EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}        

    //protected void ValidateTheoryModules()
    //{
    //    Int32 remainingTheoryModuleCount = 0; ;
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            Exam exam = CourseManager.GetNextExam(context, currentCourseID, 1);
    //            Int32 project = Convert.ToInt32(enmModuleType.Project);
    //            Int32 elective = Convert.ToInt32(enmSelectionType.Elective);
    //            Int32 electiveGroupID = 0;
    //            Int32 compulsory = Convert.ToInt32(enmSelectionType.Compulsory);
    //            Int64 firstRevisionNumber = CourseManager.GetCourseRevisionNumberAtRegistrationCommenced(context, currentCourseID, registrationNumber, entityID);
    //            currentevisionNumber = CourseManager.GetCourseRevisionNumberAtRegistrationCompleted(context, currentCourseID, registrationNumber, entityID);
    //            preevisionNumber = currentevisionNumber - 1;

    //                remainingTheoryModuleCount = GetRemainingAllTheoryModuleCount(currentCourseID, registrationNumber, currentevisionNumber, entityID);



    //            List<Int32> attemptedAndSelectedModules = new List<Int32>();
    //            List<Int32> passedAdnSelectedModules = new List<Int32>();

    //                passedAdnSelectedModules = CourseManager.GetPassedModulesOfCurrentRevision(context, currentCourseID, registrationNumber, currentevisionNumber, entityID).Where(c => c.ModuleTypeID != project).Select(c => c.ID).Distinct().ToList();
    //                attemptedAndSelectedModules = CourseManager.GetListtOfAttempteddModulesOfCurrentRevision(context, exam.ID, currentCourseID, registrationNumber, currentevisionNumber, entityID, enmModuleType.Theory).Where(c => c.ModuleTypeID != project).Select(c => c.ID).Distinct().ToList();

    //            Int32 selectedModuleID = 0;
    //            Int32 selectedModuleCount = 0;
    //            List<Int32> selectedModules = new List<Int32>();
    //            //Selected Module in this exam
    //            foreach (ListItem item in chklistProject.Items)
    //            {
    //                if (item.Selected == true)
    //                {
    //                    if (selectedModuleCount >= remainingTheoryModuleCount)
    //                    {
    //                        item.Selected = false;
    //                        throw new Exception("You can select only " + remainingTheoryModuleCount.ToString() + " remaining modules in this exam. If you want to select this module deselect any selected module first.");
    //                    }
    //                    selectedModuleID = Convert.ToInt32(item.Value);
    //                    Module selectedModule = context.Modules.Find(selectedModuleID);

    //                    if (selectedModule.enmSelectionType == enmSelectionType.Elective)
    //                    {
    //                        electiveGroupID = selectedModule.ElectiveGroup.Value;
    //                        Int32 electivegroupsubnumber = selectedModule.ModuleSubNUmber;


    //                        var pendingResultCount = (from r in context.CourseExamApplicationDetails
    //                                                  where r.Module.ElectiveGroup == electiveGroupID && r.ResultGradeID.HasValue == false &&
    //                                                  r.ExamID != exam.ID && r.CandidateID == entityID && r.RegistrationNumber == registrationNumber
    //                                                  && r.CourseID == currentCourseID && r.Module.SelectionTypeID == elective && r.IsUnfairMeansCase == false
    //                                                  select r);
    //                        if (pendingResultCount.Count() > 0)
    //                        {
    //                            //item.Selected = false;
    //                            if ((pendingResultCount.FirstOrDefault().CourseExamApplication.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)) && (pendingResultCount.FirstOrDefault().CourseExamApplication.IsExported))
    //                            {
    //                                item.Selected = false;
    //                                throw new Exception("You can not select this module - " + selectedModule.ShortName + " as you have already attempted this module or module of this elective group in the last exam and result is pending.");
    //                            }
    //                        }                     

    //                    }
    //                    else
    //                    {
    //                        var pendingResultCount = (from r in context.CourseExamApplicationDetails
    //                                                  where r.ModuleID == selectedModuleID && r.ResultGradeID.HasValue == false &&
    //                                                  r.ExamID != exam.ID && r.CandidateID == entityID && r.RegistrationNumber == registrationNumber
    //                                                  && r.CourseID == currentCourseID && r.Module.SelectionTypeID == compulsory && r.IsUnfairMeansCase == false
    //                                                  select r);
    //                        if (pendingResultCount.Count() > 0)
    //                        {
    //                            if ((pendingResultCount.FirstOrDefault().CourseExamApplication.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)) && (pendingResultCount.FirstOrDefault().CourseExamApplication.IsExported))
    //                            {
    //                                item.Selected = false;
    //                                throw new Exception("You can not select this module - " + selectedModule.ShortName + " as you have already attempted this module in the last exam and result is pending.");
    //                            }
    //                        }
    //                        if (!passedAdnSelectedModules.Contains(selectedModuleID))
    //                        {
    //                            passedAdnSelectedModules.Add(selectedModuleID);
    //                            selectedModules.Add(selectedModuleID);
    //                        }
    //                    }
    //                    selectedModuleCount++;
    //                    lblCoutMessage.Text = "Theory Papers  &nbsp;&nbsp;&nbsp;&nbsp; <b style='color:red'>You have selected " + selectedModuleCount.ToString() + " modules out of " + remainingTheoryModuleCount.ToString() + " modules (remaining to pass) in the list below.</b>";
    //                }
    //            }

    //            if (selectedModuleCount == 0)
    //                lblCoutMessage.Text = "Theory Papers  &nbsp;&nbsp;&nbsp;&nbsp; <b style='color:red'>You can select upto  " + remainingTheoryModuleCount.ToString() + " modules (remaining to pass) in the list below.</b>";
    //            else
    //            {
    //                //Time table validation

    //                var lstTimeTable = (from t in context.ExamTimeTables
    //                                    where t.ExamID == exam.ID && selectedModules.Contains(t.ModuleID)
    //                                    group t by new { t.ExamFomDate, t.ExamSession.Name } into g
    //                                    select new { Group = g.Key, cnt = g.Count() }).ToList();
    //                foreach (var timetable in lstTimeTable)
    //                {
    //                    if (timetable.cnt > 1)
    //                    {
    //                        btnSave.Visible = false;
    //                        throw new Exception("You can not select more than 1 module on same date and exam session(" + timetable.Group.ExamFomDate.ToString("dd-MMM-yyyy") + " " + timetable.Group.Name + "). Please refer time table for details.");
    //                    }
    //                }
    //            }
    //            attemptedAndSelectedModules.AddRange(selectedModules);
    //            if (!(currentCourseID == 4 && firstRevisionNumber < currentevisionNumber && currentevisionNumber == 3) || currentCourseID != 4)
    //            {
    //                if (remainingTheoryModuleCount > 0)
    //                {
    //                    if (selectedModuleCount >= remainingTheoryModuleCount)
    //                    {
    //                        UpdateFeeDetails();
    //                        return;
    //                    }
    //                    List<Int32> attemptedCompulsoryModules = (from p in context.Modules
    //                                                              where attemptedAndSelectedModules.Contains(p.ID) && p.SelectionTypeID == compulsory
    //                                                              select p.ID).Distinct().ToList();
    //                    List<Int32> attemptedElectiveModules = attemptedAndSelectedModules.FindAll(c => !attemptedCompulsoryModules.Contains(c));
    //                    Int32 traverceFlagCompulsory = 0;
    //                    Int32 traverceFlagElective = 0;
    //                    foreach (ListItem item in chklistProject.Items)
    //                    {
    //                        if (item.Selected)
    //                        {
    //                            Boolean isEligible = true;
    //                            Int32 practicalModulesID = Convert.ToInt32(item.Value);
    //                            var theoryCompulsoryModules = (from p in context.ModulePracticalEligibilities
    //                                                           where p.PracticalModuleID == practicalModulesID
    //                                                           select p.TheoryModule).Distinct().ToList();
    //                            if (theoryCompulsoryModules.Where(c => c.enmSelectionType == enmSelectionType.Elective).Count() == 0)
    //                                traverceFlagElective = 1;

    //                            //for MAT-O Level since in this there is no practical eligibility
    //                            if (theoryCompulsoryModules.Where(c => c.enmSelectionType == enmSelectionType.Compulsory).Count() == 0)
    //                                traverceFlagCompulsory = 1;

    //                            var theoryElectiveModulesID = (from p in context.ModulePracticalEligibilities
    //                                                           where p.PracticalModuleID == practicalModulesID && p.SelectionTypeID != compulsory
    //                                                           select p.ElectiveGroup).Distinct().ToList();

    //                            var attemptedelectivegroup = (from m in context.Modules
    //                                                          where attemptedElectiveModules.Contains(m.ID) && m.SelectionTypeID != compulsory
    //                                                          select m.ElectiveGroup).Distinct().ToList();

    //                            foreach (Module thModule in theoryCompulsoryModules)
    //                            {
    //                                if (thModule.enmSelectionType == enmSelectionType.Compulsory)
    //                                {
    //                                    if (!attemptedCompulsoryModules.Contains(thModule.ID))
    //                                    {
    //                                        var listOfPassedModules = (from d in context.CourseExamApplicationDetails
    //                                                                   join m in context.Modules on d.ModuleID equals m.ID
    //                                                                   where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
    //                                                                   d.Grade.IsPassed == true
    //                                                                   orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
    //                                                                   select m.ID).ToList();

    //                                        string moduleCode = context.Modules.Find(thModule.ID).ShortName;
    //                                        moduleCode = moduleCode.Substring(0, moduleCode.IndexOf("-"));
    //                                        var modulesAtt = context.Modules.Where(a => listOfPassedModules.Contains(a.ID));
    //                                        modulesAtt = modulesAtt.Where(a => a.ShortName.Contains(moduleCode));
    //                                        var clearedModCount = modulesAtt.Count();
    //                                        if (clearedModCount <= 0)
    //                                        {
    //                                            isEligible = false;
    //                                            break;
    //                                        }
    //                                    }
    //                                    traverceFlagCompulsory = 1;
    //                                }
    //                            }
    //                            foreach (Int32 egId in theoryElectiveModulesID)
    //                            {
    //                                //if (attemptedElectiveModules.Contains(thModule.ID))
    //                                //{
    //                                //    isEligible = true;
    //                                //    traverceFlagElective = 1;
    //                                //}
    //                                if (!attemptedelectivegroup.Contains(egId))
    //                                {
    //                                    var listOfPassedModules = (from d in context.CourseExamApplicationDetails
    //                                                               join m in context.Modules on d.ModuleID equals m.ID
    //                                                               where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
    //                                                               d.Grade.IsPassed == true && m.ElectiveGroup.HasValue && m.ElectiveGroup.Value == egId
    //                                                               orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
    //                                                               select m.ID).ToList();

    //                                    var moduleCode = from d in context.Modules
    //                                                     where d.ElectiveGroup == egId
    //                                                     select new
    //                                                     {
    //                                                         ShortName = d.ShortName,
    //                                                         NumberOfPapersToPass = d.NumberOfElectiveModulesAllowed
    //                                                     };
    //                                    var modulesAtt = context.Modules.Where(a => listOfPassedModules.Contains(a.ID));
    //                                    int match = 0;
    //                                    int required = 0;
    //                                    foreach (var aa in moduleCode)
    //                                    {
    //                                        var code = aa.ShortName.IndexOf("-") > 0 ? aa.ShortName.Substring(0, aa.ShortName.IndexOf("-")) : aa.ShortName;
    //                                        modulesAtt = modulesAtt.Where(a => a.ShortName.Contains(code));
    //                                        if (modulesAtt.Count() > 0)
    //                                            match++;
    //                                        required = aa.NumberOfPapersToPass.Value;
    //                                    }
    //                                    if (match < required)
    //                                    {
    //                                        isEligible = false;
    //                                        break;
    //                                    }
    //                                }
    //                                traverceFlagElective = 1;
    //                            }
    //                            if (traverceFlagElective == 1 && traverceFlagCompulsory == 1)
    //                            {
    //                                item.Selected = isEligible;
    //                            }
    //                            else
    //                            {
    //                                item.Selected = false;
    //                                throw new Exception("You are not eligible to select this practical module. See practical eligibility criteria.");
    //                            }
    //                        }
    //                    }
    //                }
    //            }

    //            UpdateFeeDetails();
    //            btnSave.Visible = true;
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        Int32 selectedModuleCount = 0;
    //        foreach (ListItem item in chklistProject.Items)
    //        {
    //            if (item.Selected == true)
    //            {
    //                selectedModuleCount++;
    //            }
    //        }
    //        lblCoutMessage.Text = "Theory Papers  &nbsp;&nbsp;&nbsp;&nbsp; <b style='color:red'>You have selected " + selectedModuleCount.ToString() + " modules out of " + remainingTheoryModuleCount.ToString() + " modules (remaining to pass) in the list below.</b>";
    //        UpdateFeeDetails();
    //        throw ex;
    //    }
    //}              

    protected void ShowApplicationData(Int64 applID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                CourseProjectApplication appl = context.CourseProjectApplications.Find(applID);
                Candidate candidate = appl.Candidate;
                var modules = (from a in context.CourseProjectApplicationDetails
                               join m in context.Modules on a.ModuleID equals m.ID
                               where a.CourseExamApplicationID == applID
                               orderby m.Code
                               select new { m.ShortName, m.Name, m.ModuleTypeID, a.FeeAmount, m.ID });
                if (appl != null)
                {
                    var registrationDetail = (from s in context.RegistrationDetails
                                              where s.CourseID == appl.CourseID && s.CandidateID == entityID && s.RegistrationNo == appl.RegistrationNumber
                                              select s).FirstOrDefault();

                    Int32 project = Convert.ToInt32(enmModuleType.Project);
                    //Int32 projectFee = 0;                   
                    if (GridView_project.Rows.Count > 0)
                    {
                        foreach (var module in modules.ToList())
                        {
                            foreach (GridViewRow gv in GridView_project.Rows)
                            {
                                int rowIndex = gv.RowIndex;
                                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gv.FindControl("cb_selection");
                                if (module.ID == Convert.ToInt64(GridView_project.DataKeys[rowIndex].Values[0]))
                                {
                                    chk.Checked = true;
                                    amount_calculation();
                                }
                            }

                        }
                    }

                }

                if (appl.FinalSubmitted)
                {
                    Int32 project = Convert.ToInt32(enmModuleType.Project);
                    //Int32 projectFee = 0;
                    //lblCoutMessage.Text = "Theory Papers ";
                    LnkBtnPrintForm.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("ProjectFormPreview.aspx?Appid=" + appl.ID.ToString()) + "');");
                    LnkBtnPrintForm.Visible = true;
                    btnSave.Visible = false;
                    btnback.Visible = true;

                    //lblTotalFee.Text = appl.FeeAmount.ToString("F");

                    //List<ListItem> lstProject = new List<ListItem>();                        
                    //foreach (var module in modules.ToList())
                    //{
                    //    if (module.ModuleTypeID == project)
                    //    {
                    //        lstProject.Add(new ListItem(module.ShortName + " " + module.Name, module.ID.ToString()));
                    //    }                            
                    //}
                    //chklistProject.DataSource = lstProject.Select(c => new { ValueField = c.Value, TextField = c.Text });
                    //chklistProject.DataBind();    

                    foreach (GridViewRow gv in GridView_project.Rows)
                    {
                        int rowIndex = gv.RowIndex;
                        System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gv.FindControl("cb_selection");
                        chk.Enabled = false;
                    }
                    var registered = (from r in context.CourseProjectApplications
                                      where r.CandidateID == entityID && r.FinalSubmitted == true
                                      orderby r.ApplicationDate descending
                                      select new
                                      {
                                          status = r.ApplicationStatusID,
                                          coursename = r.Course.Name,
                                          applicationID = r.ID,
                                          demandnoteID = r.DemandNoteID,
                                          Batchitemid = r.BatchItemID.HasValue ? r.BatchItemID.Value : 0
                                      }).FirstOrDefault();
                    if (registered != null)
                    {
                        enmCourseApplicationStatus applStatus = (enmCourseApplicationStatus)registered.status;
                        if (applStatus == enmCourseApplicationStatus.AppliedButFeeNotDepositedByCandidate)
                        {
                            btnpayment.Visible = true;
                            chkdisclamier.Enabled = false;
                            chkdisclamier.Checked = true;
                            lblerror.Text = "You have already applied for the selected Project but fees not paid yet.Please make the payment. ";
                        }
                        else
                        {
                            chkdisclamier.Enabled = false;
                            lblerror.Text = "You have already applied for this Project.";
                        }
                    }
                    //chkdisclamier.Enabled = false;
                    //lblerror.Text = "You have already applied for this exam.";
                }
                else
                {
                    btnSave.Text = "Update";
                }
                lblAmountHindi.Text = EConnect.Utils.Conversion.ConversionUtility.NumberToText(lblTotalFee.Text, EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English);

            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //protected Int32 GetRemainingAllTheoryModuleCount(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber, Int64 candidateID)
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            Int32 projectmodulecount = 0;
    //            Int32 max_revision = (from p in context.Modules
    //                                  where p.CourseID == currentCourseID
    //                                  select p.RevisionNumber).Distinct().Max();
    //            Int32 registrationRevisionNumber = EConnect.NIELIT.CourseManager.GetCourseRevisionNumberAtRegistrationCommenced(context, currentCourseID, registrationNumber, candidateID);                
    //            Int32 project = Convert.ToInt32(enmModuleType.Project);
    //            Int32 allProjectModules = CourseManager.GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Project, null);                
    //            Int32 allProjectModulesPassed = (from d in context.CourseExamApplicationDetails
    //                                            join m in context.Modules on d.ModuleID equals m.ID
    //                                            where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
    //                                            d.Grade.IsPassed == true && m.ModuleTypeID == project
    //                                            orderby m.ModuleTypeID, m.SelectionTypeID, m.Code                                               
    //                                            select m).Count();


    //            projectmodulecount = allProjectModules - allProjectModulesPassed;

    //            return projectmodulecount;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}   

    void cleartextamount()
    {
        lblProjectCount.Text = "";
        //lblTotalProjectFee.Text = "";  
        lblTotalFee.Text = "";
        lblAmountHindi.Text = "Zero Rupees Only";
    }

    //protected void chklistProject_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {                
    //                lblerror.Visible = lblerror.Visible;
    //                ValidateTheoryModules();               
    //        }
    //    }

    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message, true);
    //    }
    //}

    //protected void BindGridView_passedmodules()
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            var listOfPassedModules = (from d in context.CourseExamApplicationDetails
    //                                       join m in context.Modules on d.ModuleID equals m.ID
    //                                       where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
    //                                       d.Grade.IsPassed == true && m.ModuleTypeID==4
    //                                       orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
    //                                       select new
    //                                       {                                               
    //                                           ID = m.ID,
    //                                           name = m.Name,
    //                                           Code = m.ShortName,                                              
    //                                           Result = d.Grade.Description,
    //                                           Grade = d.Grade.Code
    //                                       });
    //            GridView_Passedproject.DataSource = listOfPassedModules.ToList();
    //            GridView_Passedproject.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    protected void BindGridView_modules_fill()
    {
        try
        {

            using (EConnectContext context = new EConnectContext())
            {
                Int32 RevisionNumber = 0;
                int theorymodule = Convert.ToInt32(enmModuleType.Theory);
                int projectmodule = Convert.ToInt32(enmModuleType.Project);
                var attempted_theory_modules = CourseManager.GetListOfAttempteddModulesOfAnyRevision(context, currentCourseID, registrationNumber, entityID);

                var project_eligilbilities = (from e in context.ProjectEligibilities
                                              where e.course_level_id == currentCourseID && e.whether_effective == true
                                              select e).Distinct().FirstOrDefault();

                if (project_eligilbilities != null)
                {
                    if (project_eligilbilities.whether_max_revision_to_show == false)
                    {
                        RevisionNumber = attempted_theory_modules.Where(s => s.ModuleTypeID == theorymodule).Select(s => s.RevisionNumber).Distinct().Max();
                    }
                    else
                    {
                        RevisionNumber = CourseManager.GetCurrentCourseRevisionNumber(context, currentCourseID);
                    }
                }

                //Int32 max_revision = (from p in context.Modules
                //                      where p.CourseID == currentCourseID
                //                    select p.RevisionNumber).Distinct().Max();

                //var appliedproject = (from r in context.CourseProjectApplications
                //                      join s in context.CourseProjectApplicationDetails
                //                      on r.ID equals s.CourseExamApplicationID
                //                      join d in context.DemandNotes
                //                      on r.DemandNoteID equals d.ID
                //                      join m in context.Modules
                //                      on s.ModuleID equals m.ID
                //                      where r.RegistrationNumber == registrationNumber && r.CandidateID == entityID && r.CourseID == currentCourseID
                //                      && r.FinalSubmitted == true && d.PaymentStatusID == 2 && r.PaymentStatusID == 2 && r.DemandNoteID != null
                //                      select new
                //                      {
                //                          moduleID = m.ID,
                //                      }).ToList(); 
              /*  var listOfmodules = (from m in context.Modules
                                     join pf in context.ProjectFees on m.ShortName equals pf.project_short_name
                                     where pf.whether_effective == "Y" &&
                                      m.CourseID == pf.course_level_id &&
                                      pf.course_level_id == currentCourseID &&
                                      !(
                                          (from pd in context.CourseProjectApplicationDetails
                                           join m2 in context.Modules on pd.ModuleID equals m2.ID
                                           join pf2 in context.ProjectFees on m2.ShortName equals pf2.project_short_name
                                           where pd.RegistrationNumber == registrationNumber &&
                                                 m2.CourseID == pf2.course_level_id &&
                                                 pd.CourseID == currentCourseID &&
                               (from a in context.CourseProjectApplications
                                where a.ID == pd.CourseExamApplicationID && a.FinalSubmitted == true
                                select 1).Any() &&
                               !(from cead in context.CourseExamApplicationDetails
                                 join rg in context.ResultGrades on cead.ResultGradeID equals rg.ID
                                 where cead.RegistrationNumber == registrationNumber &&
                                       cead.CourseID == currentCourseID &&
                                       rg.IsPassed == false &&
                                       m2.ProjectNumber != null &&
                                       m2.ID == cead.ModuleID
                                 select 1).Any()
                                           select m2.ProjectNumber)
                                          .Union(
                                              from c in context.CourseExamApplicationDetails
                                              join m3 in context.Modules on c.ModuleID equals m3.ID
                                              join rg in context.ResultGrades on c.ResultGradeID equals rg.ID
                                              where c.RegistrationNumber == registrationNumber && c.CourseID == currentCourseID
                                                    && rg.IsPassed == true
						 && pf.project_short_name ==m.ShortName
                                              select m3.ProjectNumber
                                          )
                                      ).Contains(m.ProjectNumber) &&
                                      m.RevisionNumber == (from mm in context.Modules
                                                           where m.CourseID == mm.CourseID
                                                           select mm.RevisionNumber).Max()
                                     select new
                                     {
                                         ID = m.ID,
                                         name = m.ShortName + " - " + "(" + m.Name + ")",
                                         fees = pf.fee_amount
                                     }).Distinct();*/

                /*var listOfmodules = (from m in context.Modules
                                         join pf in context.ProjectFees on m.ShortName equals pf.project_short_name
                                         where pf.whether_effective == "Y" &&
                                          m.CourseID == pf.course_level_id &&
                                          pf.course_level_id == currentCourseID &&
                                          !(
                                              (from pd in context.CourseProjectApplicationDetails
                                               join m2 in context.Modules on pd.ModuleID equals m2.ID
                                               join pf2 in context.ProjectFees on m2.ShortName equals pf2.project_short_name
                                               where pd.RegistrationNumber == registrationNumber &&
                                                     m2.CourseID == pf2.course_level_id &&
                                                     pd.CourseID == currentCourseID
                                               select m2.ProjectNumber)
                                              .Union(
                                                  from c in context.CourseExamApplicationDetails
                                                  join m3 in context.Modules on c.ModuleID equals m3.ID
                                                  join rg in context.ResultGrades on c.ResultGradeID equals rg.ID
                                                  where c.RegistrationNumber == registrationNumber && c.CourseID == currentCourseID
                                                        && rg.IsPassed == true
                                                  select m3.ProjectNumber
                                              )
                                          ).Contains(m.ProjectNumber) &&
                                          m.RevisionNumber == (from mm in context.Modules
                                                               where m.CourseID == mm.CourseID
                                                               select mm.RevisionNumber).Max()
                                         select new
                                         {
                                             ID = m.ID,
                                             name = m.ShortName + " - " + "(" + m.Name + ")",
                                             fees = pf.fee_amount
                                         });

                    var listOfmodules = (from m in context.Modules
                                     join pf in context.ProjectFees on m.ShortName equals pf.project_short_name
                                     where pf.whether_effective == "Y" &&
                                      m.CourseID == pf.course_level_id &&
                                      pf.course_level_id == currentCourseID &&
                                      !(
                                          from pd in context.CourseProjectApplicationDetails
                                          join m2 in context.Modules on pd.ModuleID equals m2.ID
                                          join pf2 in context.ProjectFees on m2.ShortName equals pf2.project_short_name
                                          where pd.RegistrationNumber == registrationNumber &&
                                                m2.CourseID == pf2.course_level_id &&
                                                pd.CourseID == currentCourseID
                                          select m2.ProjectNumber
                                      ).Contains(m.ProjectNumber) &&
                                      m.RevisionNumber == (from mm in context.Modules
                                                           where m.CourseID == mm.CourseID
                                                           select mm.RevisionNumber).Max()
                select new
                                     {
                                         ID = m.ID,
                                         name = m.ShortName + " - " + "(" + m.Name + ")",
                                         fees = pf.fee_amount                                       
                                     });*/

                /* select new
                 {
                     m.ID,
                     m.ShortName,
                     pf.fee_amount
                 });*/
                /*Commented on 1 May2024 var listOfmodules = (from m in context.Modules
                                      join f in context.ProjectFees on m.ShortName equals f.project_short_name
                                      where m.CourseID == currentCourseID && m.ModuleTypeID == projectmodule && m.RevisionNumber == RevisionNumber
                                      && f.whether_effective == "Y" && f.course_level_id == currentCourseID
                                      && (!((from d in context.CourseExamApplicationDetails
                                             where d.Grade.IsPassed == true && d.CourseID == currentCourseID &&
                                             d.RegistrationNumber == registrationNumber && d.CandidateID == entityID
                                             select d.ModuleID).Contains(m.ID))
                                      || !((from r in context.CourseProjectApplications
                                            join s in context.CourseProjectApplicationDetails on r.ID equals s.CourseExamApplicationID
                                            join d in context.DemandNotes on r.DemandNoteID equals d.ID
                                            join mo in context.Modules on s.ModuleID equals mo.ID
                                            where r.RegistrationNumber == registrationNumber && r.CandidateID == entityID && r.CourseID == currentCourseID
                                            && r.FinalSubmitted == true && d.PaymentStatusID == 2 && r.PaymentStatusID == 2
                                            select mo.ID).Contains(m.ID)) )                                                    
                                      orderby f.project_sequence_number
                                      select new
                                      {
                                          ID = m.ID,
                                          name = m.ShortName + " - " + "(" + m.Name + ")",
                                          fees = f.fee_amount
                                      }).Distinct().ToList();*/

                //var listOfmodules1 = (from m in context.Modules
                //                     join f in context.ProjectFees on m.ShortName equals f.project_short_name
                //                     where m.CourseID == currentCourseID && m.ModuleTypeID == projectmodule && m.RevisionNumber == RevisionNumber
                //                     && f.whether_effective == "Y" && f.course_level_id == currentCourseID
                //                     && (!((from d in context.CourseExamApplicationDetails
                //                            where d.Grade.IsPassed == true && d.CourseID == currentCourseID &&
                //                            d.RegistrationNumber == registrationNumber && d.CandidateID == entityID
                //                            select d.ModuleID).Contains(m.ID))
                //                     || !((from r in context.CourseProjectApplications
                //                           join s in context.CourseProjectApplicationDetails on r.ID equals s.CourseExamApplicationID
                //                           join d in context.DemandNotes on r.DemandNoteID equals d.ID
                //                           join mo in context.Modules on s.ModuleID equals mo.ID
                //                           where r.RegistrationNumber == registrationNumber && r.CandidateID == entityID && r.CourseID == currentCourseID
                //                           && r.FinalSubmitted == true && d.PaymentStatusID == 2 && r.PaymentStatusID == 2
                //                           select mo.ID).Contains(m.ID)))
                //                     orderby f.project_sequence_number
                //                     select new
                //                     {
                //                         ID = m.ID,
                //                         name = m.ShortName + " - " + "(" + m.Name + ")",
                //                         fees = f.fee_amount
                //                     }); 

                //var moduleswithcanceledDemandnote = (from r in context.CourseProjectApplications
                //                                      join s in context.CourseProjectApplicationDetails on r.ID equals s.CourseExamApplicationID
                //                                      join d in context.DemandNotes on r.DemandNoteID equals d.ID
                //                                      join mo in context.Modules on s.ModuleID equals mo.ID
                //                                      join f in context.ProjectFees on mo.ShortName equals f.project_short_name
                //                                      where r.RegistrationNumber == registrationNumber && r.CandidateID == entityID && r.CourseID == currentCourseID
                //                                      && r.WhetherDemandNoteCanceled == true && f.whether_effective == "Y" && f.course_level_id == currentCourseID
                //                                      && !((from dd in context.CourseExamApplicationDetails
                //                                            where dd.Grade.IsPassed == true && dd.CourseID == currentCourseID &&
                //                                            dd.RegistrationNumber == registrationNumber && dd.CandidateID == entityID
                //                                            select dd.ModuleID).Contains(mo.ID))
                //                                     && !((from rr in context.CourseProjectApplications
                //                                           join ss in context.CourseProjectApplicationDetails on rr.ID equals ss.CourseExamApplicationID
                //                                           join de in context.DemandNotes on rr.DemandNoteID equals de.ID
                //                                           join mmo in context.Modules on ss.ModuleID equals mmo.ID
                //                                           where rr.RegistrationNumber == registrationNumber && rr.CandidateID == entityID && rr.CourseID == currentCourseID
                //                                           && rr.FinalSubmitted == true && de.PaymentStatusID == 2 && rr.PaymentStatusID == 2
                //                                           select mmo.ID).Contains(mo.ID))
                //                                     orderby f.project_sequence_number
                //                                      select new
                //                                      {
                //                                          ID = mo.ID,
                //                                          name = mo.ShortName + " - " + "(" + mo.Name + ")",
                //                                          fees = f.fee_amount
                //                                      }).Distinct().ToList();


                //var totalmodulestoshow = listOfmodules.Union(moduleswithcanceledDemandnote).Distinct().ToList();
		DataTable dataTable = listofModules();

                     if (dataTable.Rows.Count > 0)
                     {                       
                         //grdMismatch.DataSource = dataTable;
                         //grdMismatch.DataBind();
                         GridView_project.DataSource = dataTable;
                         GridView_project.DataBind();
                     }
                     else if (dataTable.Rows.Count== 0 && currentCourseID  == 1213)
                     {
                        
                          btnSave.Visible = false;
                          btnback.Visible = false;
                     }
                     else if (dataTable.Rows.Count == 0 && currentCourseID != 1213)
                     {
                         btnSave.Enabled = false;
                         btnback.Disabled = true;

                     }

                //var totalmodulestoshow = listOfmodules.Distinct().ToList();





                //var listOfmodules = (from d in context.CourseExamApplicationDetails
                //                           join m in context.Modules on d.ModuleID equals m.ID
                //                           join f in context.ProjectFees on m.CourseID equals f.course_level_id
                //                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                //                           m.ModuleTypeID == 4
                //                           orderby m.ModuleTypeID, m.SelectionTypeID, m.Code
                //                           select new
                //                           {
                //                               ID = m.ID,
                //                               name = m.Name + "(" + m.ShortName+")",                                               
                //                               fees = f.fee_amount                                               
                //                           }).ToList();

              /*  GridView_project.DataSource = totalmodulestoshow;
                GridView_project.DataBind();

                if (GridView_project.Rows.Count == 0)
                {
                    btnSave.Visible = false;
                    btnback.Visible = false;
                }*/
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //public static Int32 GetProjectFee(Int32 courseID, enmFeeType feeType)
    //{
    //    try
    //    {
    //        Int32 feeTypeID = Convert.ToInt32(feeType);
    //        int fee = (from r in FeeDetails
    //                   where r.CourseID == courseID &&
    //                   r.FeeTypeID == feeTypeID && r.EffectiveFromDate <= System.Data.Entity.DbFunctions.CreateDateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
    //                   orderby r.EffectiveFromDate descending
    //                   select r.FeeAmount).FirstOrDefault();
    //        if (fee > 0)
    //            return fee;
    //        else
    //            return 0;
    //    }
    //    catch (Exception) { return 0; }
    //}

    protected void GridView_project_RowDataBound(object sender, GridViewRowEventArgs e)
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

    //protected void GridView_Passedproject_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            e.Row.Cells[0].Text = ((e.Row.RowIndex + 1)).ToString();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    protected void amount_calculation_CHM()
    {
        List<Exception> loopExceptions = new List<Exception>();
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                int count = 0;
                float amount = (float)0.00;
                float finalamount = (float)0.00;

                if (GridView_project.Rows.Count > 0)
                {
                    foreach (GridViewRow gv in GridView_project.Rows)
                    {
                        try
                        {
                            System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gv.FindControl("cb_selection");
                            if (chk.Checked == true)
                            {
                                int rowIndex = gv.RowIndex;
                                long projectmoduleId = Convert.ToInt64(GridView_project.DataKeys[rowIndex].Values[0]);
                                int projectmodule = Convert.ToInt32(enmModuleType.Project);
                                string projectShortName = context.Modules.Where(s => s.ModuleTypeID == projectmodule && s.ID == projectmoduleId).Select(s => s.ShortName).Distinct().FirstOrDefault();

                                //Commented on  28-03-2024 
                                if (CourseManager.IsEligibleForProject(currentCourseID, registrationNumber, entityID, projectShortName) == false)
                                {
                                    chk.Checked = false;
                                    throw new Exception("Dear candidate You are not eligible for this project.Kindly read carefully the project eligibility criteria and then apply for the project accordingly.");

                                }
                                //Commented on  28-03-2024 
                                amount = (float)Convert.ToSingle(gv.Cells[3].Text);
                                //lblTotalFee.Text = GridView_project.DataKeys[gv.RowIndex].Values["fees"].ToString();
                                count++;
                                finalamount = finalamount + amount;
                            }
                        }
                        catch (Exception ex)
                        {
                            //loopExceptions.Add(ex);
                            //ShowAlert(ex.Message); 
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message + "')", true);
                            // MessageBox.Show(ex.Message);
                        }
                    }
                    lblTotalFee.Text = Convert.ToSingle(finalamount).ToString("F");
                    lblAmountHindi.Text = EConnect.Utils.Conversion.ConversionUtility.NumberToText(lblTotalFee.Text, EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English);
                    lblProjectCount.Text = count.ToString();
                }
            }
            if (loopExceptions.Count > 0)
            {
                foreach (var exe in loopExceptions)
                {
                    //ShowAlert(exe.Message);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + exe.Message + "')", true);
                    //--lblerror.Visible = true;
                    //--lblerror.Text = exe.Message;

                }
            }
        }
        catch (Exception ex)
        {
            // ShowAlert(ex.Message);  
            //loopExceptions.Add(ex);   
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message + "')", true);
        }

    }



    protected void amount_calculation()
    {
        List<Exception> loopExceptions = new List<Exception>();
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                int count = 0;
                float amount = (float)0.00;
                float finalamount = (float)0.00;

                if (GridView_project.Rows.Count > 0)
                {
                    foreach (GridViewRow gv in GridView_project.Rows)
                    {
                        try
                        {
                            System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gv.FindControl("cb_selection");
                            if (chk.Checked == true)
                            {
                                int rowIndex = gv.RowIndex;
                                long projectmoduleId = Convert.ToInt64(GridView_project.DataKeys[rowIndex].Values[0]);
                                int projectmodule = Convert.ToInt32(enmModuleType.Project);
                                string projectShortName = context.Modules.Where(s => s.ModuleTypeID == projectmodule && s.ID == projectmoduleId).Select(s => s.ShortName).Distinct().FirstOrDefault();
                                if (CourseManager.IsEligibleForProject(currentCourseID, registrationNumber, entityID, projectShortName) == false)
                                {
                                    chk.Checked = false;
                                    throw new Exception("Dear candidate You are not eligible for this project.Kindly read carefully the project eligibility criteria and then apply for the project accordingly.");

                                }
                                amount = (float)Convert.ToSingle(gv.Cells[3].Text);
                                //lblTotalFee.Text = GridView_project.DataKeys[gv.RowIndex].Values["fees"].ToString();
                                count++;
                                finalamount = finalamount + amount;
                            }
                        }
                        catch (Exception ex)
                        {
                            //loopExceptions.Add(ex);
                            //ShowAlert(ex.Message); 
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message + "')", true);
                            // MessageBox.Show(ex.Message);
                        }
                    }
                    lblTotalFee.Text = Convert.ToSingle(finalamount).ToString("F");
                    lblAmountHindi.Text = EConnect.Utils.Conversion.ConversionUtility.NumberToText(lblTotalFee.Text, EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English);
                    lblProjectCount.Text = count.ToString();
                }
            }
            if (loopExceptions.Count > 0)
            {
                foreach (var exe in loopExceptions)
                {
                    //ShowAlert(exe.Message);
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + exe.Message + "')", true);
                    //--lblerror.Visible = true;
                    //--lblerror.Text = exe.Message;

                }
            }
        }
        catch (Exception ex)
        {
            // ShowAlert(ex.Message);  
            //loopExceptions.Add(ex);   
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message + "')", true);
        }

    }

    protected void cb_selection_CheckedChanged(object sender, EventArgs e)
    {
        if (currentCourseID == 1213)
        {

            amount_calculation_CHM();
        }
        else
        {
            amount_calculation();
        }

    }

    protected void btnpayment_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var registered = (from r in context.CourseProjectApplications
                                  where r.CandidateID == entityID && r.FinalSubmitted == true
                                  orderby r.ApplicationDate descending
                                  select new
                                  {
                                      status = r.ApplicationStatusID,
                                      coursename = r.Course.Name,
                                      applicationID = r.ID,
                                      demandnoteID = r.DemandNoteID,
                                      Batchitemid = r.BatchItemID.HasValue ? r.BatchItemID.Value : 0
                                  }).FirstOrDefault();
                if (registered != null)
                {
                    enmCourseApplicationStatus applStatus = (enmCourseApplicationStatus)registered.status;
                    if (applStatus == enmCourseApplicationStatus.AppliedButFeeNotDepositedByCandidate)
                    {
                        //string requesturl = "";
                        //requesturl = Request.Url.ToString();                        
                        //Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/FrmConfirmProject.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CourseProjectApplication) + "&Appid=" + registered.applicationID.ToString() + "&DemandID=" + registered.demandnoteID.Value.ToString()), true);
                        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CourseProjectApplication) + "&Appid=" + registered.applicationID.ToString() + "&DemandID=" + registered.demandnoteID.Value.ToString()), true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DataTable listofModules()
    {

        string conString = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection sqlCon = new SqlConnection(conString);
        try
        {
           // Int16 registrationNumber;
           // currentCourseID,registrationNumber
            DataTable dt = new DataTable();

            // Assuming you have a SqlConnection named sqlCon
            using (SqlCommand cmd = new SqlCommand("OABCProjectModulesDisplay", sqlCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@currentCourse_ID", currentCourseID);
                cmd.Parameters.AddWithValue("@registrationNumber", registrationNumber);


                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            //DataColumn srNoColumn = new DataColumn("SrNo", typeof(int));
            //dt.Columns.Add(srNoColumn);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i]["SrNo"] = i + 1;
            //}

            return dt;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
            return null;
        }
    }
  


}