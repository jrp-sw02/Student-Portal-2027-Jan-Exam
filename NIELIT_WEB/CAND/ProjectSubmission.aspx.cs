using System;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class CAND_ProjectSubmission : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentevisionNumber = 0;
    Int32 currentCourseID = 0;
    Int64 registrationNumber = 0;
    String currentCourseName = "";
    Int32 moduleTypeTheory = Convert.ToInt32(enmModuleType.Theory);
    Int32 moduleTypePractical = Convert.ToInt32(enmModuleType.Practical);
    Int32 moduleTypeProject = Convert.ToInt32(enmModuleType.Project);

    protected void Page_Load(object sender, EventArgs e)
    {
         try
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            lblError.Text = "";
            lblError.Visible = false;
            if (IsSessionAlive() == false)
                Response.Redirect("../Home.aspx");
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            ShowCurretnRegistrationDetails();
            ShowModuleSummary();
             GetCurrentRegistrationStatus(currentCourseID);
            lblMessage.Visible = true;
            if (!Page.IsPostBack)
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Current Course Status: " + currentCourseName, "", ""));
                BreadCrumb1.Render();
                showsidelink();
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message.ToString();
            lblError.Visible = true;
        }
    }
    protected void ShowCurretnRegistrationDetails()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                
                var registration = (from c in context.RegistrationDetails
                                    where c.CandidateID == entityID
                                    orderby c.CommencementFromDate descending
                                    select c).FirstOrDefault();
                if (registration != null)
                {
                    registrationNumber = registration.RegistrationNo;
                    currentCourseID = registration.CourseID;
                    currentevisionNumber = CourseManager.GetCourseRevisionNumberAtRegistrationCompleted(context, currentCourseID, registrationNumber, entityID);
                    currentCourseName = registration.Course.Name;
                    lblCourseNameSt.InnerText = "Current Course Status: " + currentCourseName;
                    lblRegNumber.InnerText = registration.RegistrationNo.ToString();
                    lblRegStatus.InnerText = registration.RegistrationStatus.Name;
                    lblRegDate.InnerText = registration.RegistrationDate.ToString("dd-MMM-yyyy");
                    lblRegCommencementDate.InnerText = registration.CommencementFromDate.ToString("dd-MMM-yyyy");
                    lblRegValidUptoDate.InnerText = registration.ValidUptoDate.ToString("dd-MMM-yyyy");
                    lblCandidateType.InnerText = registration.ApplicantType.Name;
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowModuleSummary()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var listOfPassedModules = (from d in context.CourseExamApplicationDetails
                                           join m in context.Modules on d.ModuleID equals m.ID
                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == entityID &&
                                           d.Grade.IsPassed == true
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
                                               Result = d.Grade.Description,
                                               Grade = d.Grade.Code
                                           });

                Int32 theoryCompModules = CourseManager.GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Theory, enmSelectionType.Compulsory);
                Int32 theoryElectiveModules = CourseManager.GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Theory, enmSelectionType.Elective);
                Int32 bridgeModules = CourseManager.GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Bridge, null);
                tdTotalTheoryModules.InnerText = (theoryCompModules + theoryElectiveModules + bridgeModules).ToString() + " (" + theoryCompModules.ToString() + " + " + theoryElectiveModules.ToString() + " + " + bridgeModules.ToString() + ")";
                tdTotalPracticalModules.InnerText = CourseManager.GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Practical, null).ToString();
                tdTotalProjectModules.InnerText = CourseManager.GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Project, null).ToString();

                Int32 moduleTypeBridge = Convert.ToInt32(enmModuleType.Bridge);
                int attempted = CourseManager.GetCountOfAttempteddModulesOfAnyRevision(context, currentCourseID, registrationNumber, entityID, enmModuleType.Theory);
                tdAttemptedTheoryModules.InnerText = (attempted + CourseManager.GetCountOfAttempteddModulesOfAnyRevision(context, currentCourseID, registrationNumber, entityID, enmModuleType.Bridge)).ToString();

                tdAttemptedPracticalModules.InnerText = CourseManager.GetCountOfAttempteddModulesOfAnyRevision(context, currentCourseID, registrationNumber, entityID, enmModuleType.Practical).ToString();
                tdAttemptedProjectModules.InnerText = CourseManager.GetCountOfAttempteddModulesOfAnyRevision(context, currentCourseID, registrationNumber, entityID, enmModuleType.Project).ToString();



                tdPassedTheoryModules.InnerText = listOfPassedModules.Where(d => (d.ModuleTypeID == moduleTypeTheory || d.ModuleTypeID == moduleTypeBridge)).Count().ToString();
                //tdPassedElectiveModules.InnerText = listOfPassedModules.Where(d => (d.ModuleTypeID == moduleTypeTheory && d.SelectionTypeID == 2)).Select(m=>m.ElectiveGroup).Distinct().Count().ToString();
                tdPassedPracticalModules.InnerText = listOfPassedModules.Where(d => d.ModuleTypeID == moduleTypePractical).Count().ToString();
                tdPassedProjectModules.InnerText = listOfPassedModules.Where(d => d.ModuleTypeID == moduleTypeProject).Count().ToString();


                tdRemainingTheorygModules.InnerText = ((theoryCompModules + theoryElectiveModules + bridgeModules) - Convert.ToInt32(tdPassedTheoryModules.InnerText)).ToString();
                tdRemainingPracticalModules.InnerText = (Convert.ToInt32(tdTotalPracticalModules.InnerText) - Convert.ToInt32(tdPassedPracticalModules.InnerText)).ToString();
                tdRemainingProjectModules.InnerText = (Convert.ToInt32(tdTotalProjectModules.InnerText) - Convert.ToInt32(tdPassedProjectModules.InnerText)).ToString();
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
            SideLink1.SideLinkType = SideLinkItem.SideLinkType.DownloadLink;
            using (EConnectContext context = new EConnectContext())
            {
                var courselist = (from rg in context.RegistrationDetails
                                  where rg.CandidateID == entityID
                                  orderby rg.CourseID ascending
                                  select rg.CourseID).ToArray();
                for (int i = 0; i < courselist.Length; i++)
                {
                    Int32 courseId = Convert.ToInt32(courselist[i]);
                    var dl = from d in context.Downloadables
                             where d.CourseID == courseId && d.ShowOnWeb == true
                             select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName };
                    foreach (var dnbl in dl.Distinct())
                    {
                        SideLink1.Items.Add(new SideLinkItem(dnbl.LinkName, "../Handlers/UploadedFileHandler.ashx?ID=" + dnbl.FileID.ToString(), "", "_blank"));
                    }
                    SideLink1.Render();
                }
                Int32 ccatId = (from c in context.RegistrationDetails
                                where c.CandidateID == entityID
                                select new
                                {
                                    courcatID = c.CourseCategoryID
                                }).FirstOrDefault().courcatID;
                var d2 = (from d in context.Downloadables
                          where d.CourseID == null && d.CourseCategoryID == ccatId && d.ShowOnWeb == true
                          select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName });
                var d3 = (from d in context.Downloadables
                          where d.CourseID == null && d.CourseCategoryID == null && d.ShowOnWeb == true
                          select new { FileID = d.DownloadableFileID.Value, LinkName = d.LinkName }).Union(d2);
                foreach (var dnbl in d3.Distinct())
                {
                    SideLink1.Items.Add(new SideLinkItem(dnbl.LinkName, "../Handlers/UploadedFileHandler.ashx?ID=" + dnbl.FileID.ToString(), "", "_blank"));
                }
                SideLink1.Render();
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void GetCurrentRegistrationStatus(Int32 CurrentCourseID) //checking Registration status
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                var currentRegistration = (from c in context.RegistrationDetails
                                        where c.CandidateID == entityID
                                        orderby c.CommencementFromDate descending
                                        select c).FirstOrDefault();
                var reRegistrationPolicy = context.CourseRegistrationPolicies.Where(a => a.CourseID == currentRegistration.CourseID && a.EffectiveFromDate <= DateTime.Now).OrderByDescending(c => c.EffectiveFromDate).FirstOrDefault();
                enmCurrentRegistrationStatus reasons = enmCurrentRegistrationStatus.None;
                if (currentRegistration.enmRegistrationStatus == enmRegistrationStatus.Completed)
                {
                    //Current Level Registration status is completed
                    //Valdate whether eligible for auto upgradation or not
                    reasons = enmCurrentRegistrationStatus.CompletedAsAllModulesOfCurrentLevelPassedCompletelyWithinRegistrationPeriod;
                }
                else if (currentRegistration.enmRegistrationStatus == enmRegistrationStatus.Cancelled)
                {
                    reasons = enmCurrentRegistrationStatus.CancelledAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                    lblMessage.Text = "Dear Candidate,<br><br>";
                }
                else if (currentRegistration.enmRegistrationStatus == enmRegistrationStatus.ProjectPending)
                {
                    //If valid upto date is less  than current date: Registration expired
                    if (DateTime.Now > currentRegistration.ValidUptoDate)
                    {
                        if (reRegistrationPolicy.ReRegistrationChance == true)
                        {
                            if (DateTime.Now > currentRegistration.ValidUptoDate.AddMonths(reRegistrationPolicy.ReRegistrationGapInMonths.Value))
                            {
                                //Registration validity period is over and no chances for reregistration
                                reasons = enmCurrentRegistrationStatus.CancelledAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                                lblMessage.Text = "Dear Candidate,<br><br>";
                            }
                            else
                            {
                                //Registration validity period is over but eligible for re-registration
                                reasons = enmCurrentRegistrationStatus.ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired;
                                lblMessage.Text = "Dear Candidate,<br><br>";
                            }
                        }
                        else
                        {
                            //Registration validity period is over and no chances for reregistration
                            reasons = enmCurrentRegistrationStatus.CancelledAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                            lblMessage.Text = "Dear Candidate,<br><br>";
                        }
                    }
                    else
                    {
                        reasons = enmCurrentRegistrationStatus.RegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndValidityPeriodNotLapsed;
                        lblMessage.Text = "Dear Candidate,<br><br>";
                    }
                    
                }
                else if (currentRegistration.enmRegistrationStatus == enmRegistrationStatus.Registered)
                {
                    //If valid upto date is less  than current date: Registration expired
                    if (DateTime.Now > currentRegistration.ValidUptoDate)
                    {
                        if (reRegistrationPolicy.ReRegistrationChance == true)
                        {
                            if (DateTime.Now > currentRegistration.ValidUptoDate.AddMonths(reRegistrationPolicy.ReRegistrationGapInMonths.Value))
                            {
                                //Registration validity period is over and no chances for reregistration
                                reasons = enmCurrentRegistrationStatus.CancelledAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                                lblMessage.Text = "Dear Candidate,<br><br>";
                            }
                            else
                            {
                                //Registration validity period is over but eligible for re-registration
                                reasons = enmCurrentRegistrationStatus.ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired;
                                lblMessage.Text = "Dear Candidate,<br><br>";
                            }
                        }
                        else
                        {
                            //Registration validity period is over and no chances for reregistration
                            reasons = enmCurrentRegistrationStatus.CancelledAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                            lblMessage.Text = "Dear Candidate,<br><br>";
                        }
                    }
                    else
                    {
                        //Registration validity period is over but eligible for re-registration
                        reasons = enmCurrentRegistrationStatus.RegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndValidityPeriodNotLapsed;
                        lblMessage.Text = "Dear Candidate,<br><br>";
                    }
                }
                else if (currentRegistration.enmRegistrationStatus == enmRegistrationStatus.Expired)
                {
                    //If valid upto date is less  than current date: Registration expired
                    if (DateTime.Now > currentRegistration.ValidUptoDate)
                    {
                        if (reRegistrationPolicy.ReRegistrationChance == true)
                        {
                            if (DateTime.Now > currentRegistration.ValidUptoDate.AddMonths(reRegistrationPolicy.ReRegistrationGapInMonths.Value))
                            {
                                //Registration validity period is over and no chances for reregistration
                                reasons = enmCurrentRegistrationStatus.CancelledAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                                lblMessage.Text = "Dear Candidate,<br><br>";
                            }
                            else
                            {
                                //Registration validity period is over but eligible for re-registration
                                reasons = enmCurrentRegistrationStatus.ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired;
                                lblMessage.Text = "Dear Candidate,<br><br>";
                            }
                        }
                        else
                        {
                            //Registration validity period is over and no chances for reregistration
                            reasons = enmCurrentRegistrationStatus.CancelledAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                        }
                    }
                    else
                    {
                        //Registration validity period is over but eligible for re-registration
                        reasons = enmCurrentRegistrationStatus.ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired;
                    }

                }
                else if (currentRegistration.enmRegistrationStatus == enmRegistrationStatus.ReRegistered)
                {
                    //If valid upto date is less  than current date: Registration expired
                    if (DateTime.Now > currentRegistration.ValidUptoDate)
                    {
                        //ReRegistration validity period is over
                        reasons = enmCurrentRegistrationStatus.CancelledAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                    }
                    else
                    {
                        //Under re-registration validity period
                        reasons = enmCurrentRegistrationStatus.ReRegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndReRegistrationValidityPeriodNotLapsed;
                    }
                }
                else
                { }
                //return reasons;

            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public enum enmCurrentRegistrationStatus
    {
        [Description("None")]
        None = 0,
        [Description("Validity period of current level registration has expired and you are now eligible for re-registration in save level with same registration number")]
        EligibleForNewRegistrationAsPreviousRegistrationNotFound = 1,

        [Description("Your current level registration would be cancelled and a fresh registration would be allotted and the credits obtained by you against  the current cancelled registration will lapse for the purpose of completing the examination at the new level.")]
        RegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndValidityPeriodNotLapsed = 2,

        [Description("Your current level registration wolud be transferred to next level and same registration would be allotted and the credits obtained by you against  the current passed registration will be given to you for the purpose of completing the examination at the new level.")]
        CompletedAsAllModulesOfCurrentLevelPassedCompletelyWithinRegistrationPeriod = 3,

        [Description("Validity period of current level registration has expired and you are now in grace period of re-registration and eligible for re-registration in same level with same registration number")]
        ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired = 4,

        [Description("Your current level registration would be cancelled and a fresh registration would be allotted and the credits obtained by you against  the current cancelled registration (due to expiry of re-registration period) will lapse for the purpose of completing the examination at the new level.")]
        CancelledAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod = 5,

        [Description("Your current level registration wolud be transferred to next higher level and same registration would be allotted and the credits obtained by you against  the current passed registration will be given to you for the purpose of completing the examination at the new level.")]
        CompletedAllModulesPassedInPreviousExamAndEligibleForAutoUpgradation = 6,
         
        [Description("You have completed highest level and you can not apply for new registration as we have not new level.")]
        CompletedEligibleForNextLevelRegistrationButNoNextLevelAvailable = 7,

        [Description("Your current level re-registration would be cancelled and a fresh registration would be allotted and the credits obtained by you against  the current cancelled registration will lapse for the purpose of completing the examination at the new level.")]
        ReRegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndReRegistrationValidityPeriodNotLapsed = 2,
    }
}
