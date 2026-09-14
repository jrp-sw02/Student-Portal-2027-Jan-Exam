using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class CAND_CurrentRegistrationStatus : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentevisionNumber = 0;
    Int32 currentCourseID = 0;
    Int32 applicanttypeID = 0;
    Int64 registrationNumber = 0;
    Int32 currentAppliedExamID = 0;
    String currentCourseName = "";
    int stateid;
    int districtid;
    Int64 appID = 0;
    Int32 moduleTypeTheory = Convert.ToInt32(enmModuleType.Theory);
    Int32 moduleTypePractical = Convert.ToInt32(enmModuleType.Practical);
    Int32 moduleTypeProject = Convert.ToInt32(enmModuleType.Project);
    Int32 registrationStatusCompleted = Convert.ToInt32(enmRegistrationStatus.Completed);
    string requesturl = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            lblError.Text = "";
            lblError.Visible = false;
            lblErrorMsg.Text = "";
            lblErrorMsg.Visible = false;
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);

            lblMessage.Visible = true;
            if (!Page.IsPostBack)
            {
                ddlConsentRelation.Enabled = false;
                //added by ashutosh start
                txtConsentDate.Text = DateTime.Today.ToString("dd-MM-yyyy");
                txtConsentTime.Text = DateTime.Now.ToString("HH:mm");
                //added by ashutosh end

                bindState();
                DdlAccCentre.Items.Clear();
                DdlAccCentre.Items.Add(new ListItem("--Select One--", "0"));

                ShowCurretnRegistrationDetails();
                ShowModuleSummary();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Current Course Status: " + currentCourseName, "", ""));
                BreadCrumb1.Render();
                showsidelink();
                ShowCandidateDetails();
                ImgBtnPopupFee.ToolTip = "";
                ImgBtnPopupFee.Enabled = false;
                ImgBtnPopupFee.ImageUrl = "~/images/DisablePopup.PNG";
                lblFeeDetail.Text = "";
                //check for new registration
                using (EConnectContext context = new EConnectContext())
                {

                    if (!context.Candidates.Any(c => (c.IsLocked == true && c.ID == entityID)))
                    {
                        lblError.Text = "Dear Candidate,<br><br>You can not apply for new registration.<br>Your profile details are not completed/locked yet. Please first complete your profile details and lock it. <br> <a href=" + EConnect.Utils.Security.QuertStringModule.Encrypt("myprofile.aspx") + "> Click here to view profile</a>";
                        lblError.Visible = true;
                        btnSave.Visible = false;
                        tblRegistration.Visible = false;
                        divSave.Visible = false;
                        return;
                    }
                    else if (IsValidProfileDetails())
                    {
                        lblError.Text = "Dear Candidate,<br><br>You can not apply for new registration.<br>Your profile details are not completed/locked yet.Please contact NIELIT Centre for updating your Profile Details.";
                        lblError.Visible = true;
                        tblRegistration.Visible = false;
                        tblRegProcess.Visible = false;
                        btnSave.Visible = false;
                    }
                    else
                    {
                        // to check whether the candidate has applied for the current exam of the course ie course registration application.
                        var appliedApplication = (from c in context.CourseRegistrationApplications
                                                  where c.CandidateID == entityID
                                                  orderby c.ApplicationDate descending
                                                  select c).FirstOrDefault();
                        if (appliedApplication != null)
                        {
                            currentAppliedExamID = GetCurrentExam(appliedApplication.ApplicantTypeID, appliedApplication.CourseID);
                        }
                        var application = (from a in context.CourseRegistrationApplications
                                           where a.CandidateID == entityID && a.FinalSubmitted == true && a.ApplicableExamID == currentAppliedExamID
                                           select a).FirstOrDefault();

                        if (application != null)
                        {
                            enmCourseApplicationStatus applStatus = (enmCourseApplicationStatus)application.ApplicationStatusID;
                            lblErrorMsg.Visible = false;
                            tblRegistration.Visible = false;
                            divSave.Visible = false;
                            lblMessage.Visible = true;
                            lblMessage.Text = "Dear Candidate ,";
                            lblMessage.Text += "<br/> You have already applied for Course Registration Application. Your details are as following:-";
                            lblMessage.Text += "<br/> Application No     : " + application.Number;
                            lblMessage.Text += "<br/> Application Date   : " + application.ApplicationDate.ToString("dd-MMM-yyyy");
                            lblMessage.Text += "<br/> Course Name        : " + application.Course.Name;
                            lblMessage.Text += "<br/> Exam Name          : " + application.ApplicableExam.Name;
                            lblMessage.Text += "<br/> Application Status : " + EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);

                            if (applStatus == enmCourseApplicationStatus.AppliedButFeeNotDepositedByCandidate)
                            {
                                Lblnote.Visible = true;
                                requesturl = Request.Url.ToString();
                                Lblnote.InnerHtml = "Note:-Please pay your fee either through CSC/E-Mitra Center or Online or by Demand Draft by clicking here <a href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CourseRegistrationApplication) + "&Appid=" + application.ID.ToString() + "&DemandID=" + application.DemandNoteID.Value.ToString() + "&RU=" + requesturl) + "'>Pay Fee </a>";
                            }
                        }
                        else
                        {
                            var application1 = (from a in context.CourseRegistrationApplications
                                                where a.CandidateID == entityID && a.FinalSubmitted != true
                                                    && a.RegisteredCourseRegistrationNo == registrationNumber
                                                orderby a.ApplicationDate descending
                                                select a).FirstOrDefault();
                            if (application1 != null)
                            {
                                appID = application1.ID;
                                Int32 ApplicantTypeId = Convert.ToInt32(enmApplicantType.Direct);

                                int couID = Convert.ToInt32(application1.CourseID);

                                var experience = context.QualificationEligibility.Where(s => s.CourseID == couID && s.ApplicantTypeID == ApplicantTypeId && s.EffectiveDateFrom <= DateTime.Now).FirstOrDefault();
                                if (experience != null)
                                {
                                    HfExperience.Value = TxtExperienceInYears.Text.Trim() != "" ? TxtExperienceInYears.Text : "0";
                                }
                                ShowData();
                                Int32 ApplicantTypeId1 = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
                                bindExamName(ApplicantTypeId1, couID);
                                btnSave.Text = "Update";
                            }
                            Lblnote.Visible = false;
                            divSave.Visible = true;
                            tblRegistration.Visible = true;
                            lblMessage.Text = "Dear Candidate,<br><br>" + EConnect.Utils.Common.EnumUtility.GetDescription(EConnect.NIELIT.CourseManager.GetCurrentRegistrationStatus(currentCourseID, entityID));
                            enmCurrentRegistrationStatus registrationStatus = EConnect.NIELIT.CourseManager.GetCurrentRegistrationStatus(currentCourseID, entityID);
                            if (registrationStatus == enmCurrentRegistrationStatus.RegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndValidityPeriodNotLapsed
                                || registrationStatus == enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod
                                || registrationStatus == enmCurrentRegistrationStatus.ReRegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndReRegistrationValidityPeriodNotLapsed)//NewOnCancellationRequest
                            {
                                ICollection<Module> lstRemainingModules = CourseManager.GetRemainingModules(context, currentCourseID, registrationNumber, currentevisionNumber, entityID);
                                if (tdRemainingTheorygModules.InnerText == "0")
                                    lstRemainingModules = lstRemainingModules.Where(d => d.ModuleTypeID != (Int32)enmModuleType.Theory).ToList();
                                if (lstRemainingModules.Count <= 0)
                                {
                                    tblRegProcess.Visible = false;
                                    lblMessage.Text = "Dear Candidate,<br><br>" + EConnect.Utils.Common.EnumUtility.GetDescription(enmCurrentRegistrationStatus.CompletedAsAllModulesOfCurrentLevelPassedCompletelyWithinRegistrationPeriod);
                                }
                                else
                                {
                                    tblRegProcess.Visible = true;
                                    ddlNewRegistration.SelectedValue = "C";
                                    ddlNewRegistration.Enabled = false;
                                    trConfirm.Visible = true;
                                    ChkConfirmCancel.Enabled = true;
                                    //ShowAlert("Details");
                                    ShowCandidateDetails();
                                }
                            }
                            else if (registrationStatus == enmCurrentRegistrationStatus.ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired)//ReRegistration
                            {
                                tblRegProcess.Visible = true;
                                trConfirm.Visible = true;
                                // Deep add code on 04 Feb 2022
                                ddlNewRegistration.SelectedValue = "R";
                                // ddlNewRegistration.Enabled = false;
                                // ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                                ddlNewRegistration_SelectedIndexChanged(ddlNewRegistration, EventArgs.Empty);
                                // Deep add code end on 04 Feb 2022

                                // DEEP add code on 15 July 2021
                                if (application1 != null)
                                {
                                    int RegistrationTypeID = Convert.ToInt32(application1.RegistrationTypeID);
                                    if (RegistrationTypeID == 4)
                                    {
                                        trConfirm.Visible = false;
                                    }
                                }
                                // DEEP end code on 15 July 2021


                            }
                            else
                            {
                                tblRegProcess.Visible = false;
                            }
                        }
                    }
                };

                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    Int32 ApplicantTypeId = Convert.ToInt32(enmApplicantType.Direct);
                    int courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
                    bindExamName(ApplicantTypeId, courseID);
                    using (EConnectContext context = new EConnectContext())
                    {
                        var experience = context.QualificationEligibility.Where(s => s.CourseID == courseID && s.ApplicantTypeID == ApplicantTypeId && s.EffectiveDateFrom <= DateTime.Now).FirstOrDefault();
                        if (experience != null)
                        {
                            HfExperience.Value = TxtExperienceInYears.Text.Trim() != "" ? TxtExperienceInYears.Text : "0";
                        }
                    };
                }

                if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                {
                    ShowData();
                    Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
                    Int32 ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
                    bindExamName(ApplicantTypeId, courseID);
                    btnSave.Text = "Update";
                }
                else
                {
                    if (appID == 0)
                        btnSave.Text = "Submit";
                    else
                        btnSave.Text = "Update";
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message.ToString();
            lblError.Visible = true;
        }
    }
    public bool IsValidProfileDetails()
    {
        Boolean flag = false;
        //Verifying Profile Details of Candidate
        using (EConnectContext context = new EConnectContext())
        {
            Candidate cand = context.Candidates.Find(entityID);
            if (cand != null)
            {
                if (string.IsNullOrEmpty(cand.Gender) == true)
                    flag = true;
                else if (cand.MaritalStatusID == null)
                    flag = true;
                else if (cand.CastCategoryID == null)
                    flag = true;


                //Contact Details
                var contact = context.CandidateContactDetails.Where(a => a.CandidateID == entityID).OrderByDescending(a => a.EffectiveFromDate).FirstOrDefault();
                if (contact != null)
                {
                    if (string.IsNullOrEmpty(contact.EmailAddress) == true)
                        flag = true;
                    else if (contact.MobileNumber == null)
                        flag = true;
                }

                //Permanent Address Details
                int PerAddTypeId = Convert.ToInt32(enmAddressType.PermanentAddress);
                var PerAdd = context.Addresses.OrderByDescending(a => a.EffectiveDateFrom).Where(a => a.CandidateID == entityID && a.AddressTypeID == PerAddTypeId).FirstOrDefault();
                if (string.IsNullOrEmpty(PerAdd.AddressLine1) == true)
                    flag = true;
                if (PerAdd.StateID == null)
                    flag = true;
                if (PerAdd.PinCode == null)
                    flag = true;

                //Correspondence Address Details
                int CorAddTypeId = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                var CorAdd = (from cad in context.Addresses
                              where cad.CandidateID == entityID && cad.AddressTypeID == CorAddTypeId
                              orderby cad.EffectiveDateFrom descending
                              select cad).FirstOrDefault();
                if (CorAdd != null)
                {
                    if (string.IsNullOrEmpty(CorAdd.AddressLine1) == true)
                        flag = true;
                    if (CorAdd.StateID == null)
                        flag = true;
                    if (CorAdd.PinCode == null)
                        flag = true;
                }

                if (flag == true)
                {

                }
            }
        };
        return flag;
    }
    protected void ShowData()
    {

        try
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int64 applID = Convert.ToInt64(Request.QueryString["Appid"]);
                    //This Query will get all the information of Applied Canditate by generated Application id 
                    if (!String.IsNullOrEmpty(Request.QueryString["Status"]))
                    {
                        ddlNewRegistration.SelectedValue = Request.QueryString["Status"].ToString();
                        ddlNewRegistration.Enabled = false;
                        if (ddlNewRegistration.SelectedValue == "R")
                        {
                            DDLRegForCourse.Enabled = false;
                            trConfirm.Visible = false;
                            ChkConfirmCancel.Enabled = false;
                        }
                        else
                        {
                            DDLRegForCourse.Enabled = true;
                            TxtExperienceInYears.Enabled = true;
                            RdoUndergngDOEACC.Enabled = true;
                            ImgBtnPopupFee.Enabled = true;
                            ChkConfirmCancel.Enabled = true;
                            DdlAccCentre.Enabled = true;
                            DdlAccState.Enabled = true;
                        }
                    }
                    var application = (from a in context.CourseRegistrationApplications
                                       where a.ID == applID
                                       select a).FirstOrDefault();
                    DDLRegForCourse.SelectedValue = application.CourseID.ToString();
                    if (application.enmApplicantType == enmApplicantType.Institute)
                    {
                        Institute ins = application.Institute;

                        RdoUndergngDOEACC.SelectedValue = "I";
                        ShowApplicatantType();

                        DdlAccState.SelectedValue = Convert.ToString(ins.StateID);
                        int id = Convert.ToInt32(DdlAccState.SelectedValue);
                        DdlAccState_SelectedIndexChanged(DdlAccState, EventArgs.Empty);
                        BindAccCentre(id, application.CourseID);
                        DdlAccCentre.SelectedValue = Convert.ToString(ins.ID);
                        ddlPaymentOption.Enabled = true;
                        ddlPaymentOption.SelectedValue = application.PaymentSourceID.Value.ToString();
                    }
                    else
                    {
                        RdoUndergngDOEACC.SelectedValue = "D";
                        ddlPaymentOption.SelectedValue = application.PaymentSourceID.Value.ToString();
                        ddlPaymentOption.Enabled = false;
                        ShowApplicatantType();
                        TxtExperienceInYears.Text = application.ExperienceInYears.ToString();
                    }
                    ddlReligion.SelectedValue = application.ReligionID.ToString();
                    txtBodyMark.Text = string.IsNullOrEmpty(application.BodyMark) == false && !string.IsNullOrWhiteSpace(application.BodyMark) ? application.BodyMark : "";

                    bindEducational();
                    DDLeducode.SelectedValue = application.EducationalQualification.ID.ToString();
                    TxtYearOfPassing2.Text = application.PassingYear.ToString();
                    //Added for Apaar
                    txtAppName.Text = application.Name.ToString();
                    txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");
                    txtGender.Text = application.Gender.ToString();
                    //Apaar End
                    //code for binding showdata apaar block added by ashutosh start
                    if (application.ApaarRequestID != null && application.ApaarRequestID != 0)
                    {
                        txtAppName.Enabled = false;
                        //Rdoownertype.Enabled = false;
                        txtDob.Enabled = false;
                        txtGender.Enabled = false;

                        Int64 apaarRequestID = Convert.ToInt64(application.ApaarRequestID);

                        var apaarData = context.ApaarRequest
                            .Where(a => a.ID == apaarRequestID)
                            .FirstOrDefault();

                        if (apaarData != null)
                        {
                            // Apaar ID
                            if (!String.IsNullOrEmpty(application.apaarID))
                            {
                                txtapaar.Text = EncryptDecrypt.DecryptString(application.apaarID);
                            }

                            txtapaar.Enabled = false;
                            //txtapaar.Style["pointer-events"] = "none";

                            // Provider Present
                            txtIsProviderPresent.Text = "True";
                            txtIsProviderPresent.Enabled = false;

                            // Consent Relation
                            ddlConsentRelation.Items.FindByText(apaarData.consentRelation).Selected = true;
                            ddlConsentRelation.Enabled = false;

                            // Provider Name
                            txtproviderName.Text =
                                !String.IsNullOrEmpty(apaarData.providerName)
                                ? apaarData.providerName
                                : "";

                            txtproviderName.Enabled = false;

                            // Auth Mode
                            // logic to populate 
                            DateTime todaydate = DateTime.Now;
                            DateTime Inputdate = Convert.ToDateTime(txtDob.Text);
                            int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);
                            BindAuthMode(countAge);

                            lblApaarDeclaration.Text = apaarData.undertakingTextChecked;
                            lblApaarDeclaration.Visible = true;
                            tdApaarDeclaration.Visible = true;

                            ddlAuthMode.SelectedValue =
                                apaarData.authModeID.HasValue
                                ? apaarData.authModeID.Value.ToString()
                                : "0";

                            ddlAuthMode.Enabled = false;

                            // Authentication ID No
                            txtAuthenticationIdNo.Text =
                                !String.IsNullOrEmpty(apaarData.authModeIDNo)
                                ? apaarData.authModeIDNo
                                : "";

                            txtAuthenticationIdNo.Enabled = false;

                            // Consent Date
                            txtConsentDate.Text =
                                apaarData.consentDate.HasValue
                                ? apaarData.consentDate.Value.ToString("dd/MM/yyyy")
                                : "";

                            txtConsentDate.Enabled = false;

                            // Consent Time
                            txtConsentTime.Text =
                                apaarData.consentTime.HasValue
                                ? apaarData.consentTime.Value.ToString(@"hh\:mm")
                                : "";

                            txtConsentTime.Enabled = false;

                            // Consent Place
                            txtConsentPlace.Text =
                                !String.IsNullOrEmpty(apaarData.consentPlace)
                                ? apaarData.consentPlace
                                : "";

                            txtConsentPlace.Enabled = false;
                            //bindDeclaration(application.ApplicantTypeID.ToString(), countAge);

                        }
                    }

                    //code for binding showdata apaar block added bby ashutosh end


                }
                ;
            }
            else
            {
                using (EConnectContext context = new EConnectContext())
                {
                    var application1 = (from a in context.CourseRegistrationApplications
                                        where a.CandidateID == entityID && a.FinalSubmitted != true
                                            && a.RegisteredCourseRegistrationNo == registrationNumber
                                        orderby a.ApplicationDate descending
                                        select a).FirstOrDefault();
                    if (application1 != null)
                    {
                        Int64 applID = Convert.ToInt64(application1.ID);
                        //This Query will get all the information of Applied Canditate by generated Application id 
                        DDLRegForCourse.SelectedValue = application1.CourseID.ToString();
                        if (application1.enmApplicantType == enmApplicantType.Institute)
                        {
                            Institute ins = application1.Institute;

                            RdoUndergngDOEACC.SelectedValue = "I";
                            ShowApplicatantType();

                            DdlAccState.SelectedValue = Convert.ToString(ins.StateID);
                            int id = Convert.ToInt32(DdlAccState.SelectedValue);
                            DdlAccState_SelectedIndexChanged(DdlAccState, EventArgs.Empty);
                            BindAccCentre(id, application1.CourseID);
                            DdlAccCentre.SelectedValue = Convert.ToString(ins.ID);
                            ddlPaymentOption.Enabled = true;
                            ddlPaymentOption.SelectedValue = application1.PaymentSourceID.Value.ToString();
                        }
                        else
                        {
                            RdoUndergngDOEACC.SelectedValue = "D";
                            ddlPaymentOption.SelectedValue = application1.PaymentSourceID.Value.ToString();
                            ddlPaymentOption.Enabled = false;
                            ShowApplicatantType();
                            TxtExperienceInYears.Text = application1.ExperienceInYears.ToString();
                        }

                        //Deep Add on 15 July 2021 for selected registration type applied by candidates

                        string RegistrationTypeId = application1.RegistrationTypeID.ToString();
                        if (application1.RegistrationTypeID.ToString() == "5")
                        {
                            ddlNewRegistration.SelectedValue = "C";
                            DDLRegForCourse.Enabled = true;
                            TxtExperienceInYears.Enabled = true;
                            RdoUndergngDOEACC.Enabled = true;
                            ImgBtnPopupFee.Enabled = true;
                            ChkConfirmCancel.Enabled = true;
                            DdlAccCentre.Enabled = true;
                            DdlAccState.Enabled = true;
                            ShowCandidateDetails();
                        }
                        if (application1.RegistrationTypeID.ToString() == "4")
                        {
                            ddlNewRegistration.SelectedValue = "R";
                            DDLRegForCourse.Enabled = false;
                            trConfirm.Visible = false;
                            ChkConfirmCancel.Enabled = false;
                        }
                        //Deep Add END on 15 July 2021 for selected registration type applied by candidates

                        ddlReligion.SelectedValue = application1.ReligionID.ToString();
                        txtBodyMark.Text = string.IsNullOrEmpty(application1.BodyMark) == false && !string.IsNullOrWhiteSpace(application1.BodyMark) ? application1.BodyMark : "";

                        bindEducational();
                        DDLeducode.SelectedValue = application1.EducationalQualification.ID.ToString();
                        TxtYearOfPassing2.Text = application1.PassingYear.ToString();
                        //Added for Apaar
                        if (txtAppName.Text == "")
                        {
                            txtAppName.Text = application1.Name.ToString();
                            txtDob.Text = application1.DateOfBirth.ToString("dd-MMM-yyyy");
                            txtGender.Text = application1.Gender.ToString();
                        }
                        //Apaar End

                        //code for binding showdata apaar block added bby ashutosh start
                        if (application1.ApaarRequestID != null && application1.ApaarRequestID != 0)
                        {
                            txtAppName.Enabled = false;
                            //Rdoownertype.Enabled = false;
                            txtDob.Enabled = false;
                            txtGender.Enabled = false;

                            Int64 apaarRequestID = Convert.ToInt64(application1.ApaarRequestID);

                            var apaarData = context.ApaarRequest
                                .Where(a => a.ID == apaarRequestID)
                                .FirstOrDefault();

                            if (apaarData != null)
                            {
                                // Apaar ID
                                if (!String.IsNullOrEmpty(application1.apaarID))
                                {
                                    txtapaar.Text = EncryptDecrypt.DecryptString(application1.apaarID);
                                }

                                txtapaar.Enabled = false;
                                //txtapaar.Style["pointer-events"] = "none";

                                // Provider Present
                                txtIsProviderPresent.Text = "True";
                                txtIsProviderPresent.Enabled = false;

                                // Consent Relation
                                ddlConsentRelation.Items.FindByText(apaarData.consentRelation).Selected = true;
                                ddlConsentRelation.Enabled = false;

                                // Provider Name
                                txtproviderName.Text =
                                    !String.IsNullOrEmpty(apaarData.providerName)
                                    ? apaarData.providerName
                                    : "";

                                txtproviderName.Enabled = false;

                                // Auth Mode
                                // logic to populate 
                                DateTime todaydate = DateTime.Now;
                                DateTime Inputdate = Convert.ToDateTime(txtDob.Text);
                                int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);
                                BindAuthMode(countAge);

                                lblApaarDeclaration.Text = apaarData.undertakingTextChecked;
                                lblApaarDeclaration.Visible = true;
                                tdApaarDeclaration.Visible = true;

                                ddlAuthMode.SelectedValue =
                                    apaarData.authModeID.HasValue
                                    ? apaarData.authModeID.Value.ToString()
                                    : "0";

                                ddlAuthMode.Enabled = false;

                                // Authentication ID No
                                txtAuthenticationIdNo.Text =
                                    !String.IsNullOrEmpty(apaarData.authModeIDNo)
                                    ? apaarData.authModeIDNo
                                    : "";

                                txtAuthenticationIdNo.Enabled = false;

                                // Consent Date
                                txtConsentDate.Text =
                                    apaarData.consentDate.HasValue
                                    ? apaarData.consentDate.Value.ToString("dd/MM/yyyy")
                                    : "";

                                txtConsentDate.Enabled = false;

                                // Consent Time
                                txtConsentTime.Text =
                                    apaarData.consentTime.HasValue
                                    ? apaarData.consentTime.Value.ToString(@"hh\:mm")
                                    : "";

                                txtConsentTime.Enabled = false;

                                // Consent Place
                                txtConsentPlace.Text =
                                    !String.IsNullOrEmpty(apaarData.consentPlace)
                                    ? apaarData.consentPlace
                                    : "";

                                txtConsentPlace.Enabled = false;
                                //bindDeclaration(application.ApplicantTypeID.ToString(), countAge);

                            }
                        }

                        //code for binding showdata apaar block added bby ashutosh end
                    }
                }
                ;

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowApplicatantType()
    {
        try
        {
            if (RdoUndergngDOEACC.SelectedValue == "I")//Institute
            {
                TrLastCenterAccno.Visible = true;
                TrAccCentre.Visible = true;
                TrExperience.Visible = false;
            }
            if (RdoUndergngDOEACC.SelectedValue == "D")//Direct
            {
                TrExperience.Visible = true;
                TrLastCenterAccno.Visible = false;
                TrLastCenterInstiName.Visible = false;
                TrAccCentre.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
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
                    applicanttypeID = registration.ApplicantTypeID;
                    currentevisionNumber = CourseManager.GetCourseRevisionNumberAtRegistrationCompleted(context, currentCourseID, registrationNumber, entityID);
                    currentCourseName = registration.Course.Name;
                    lblCourseNameSt.InnerText = "Current Course Status: " + currentCourseName;
                    lblRegNumber.InnerText = registration.RegistrationNo.ToString();
                    lblRegStatus.InnerText = registration.RegistrationStatus.Name;
                    lblRegDate.InnerText = registration.RegistrationDate.ToString("dd-MMM-yyyy");
                    lblRegCommencementDate.InnerText = registration.CommencementFromDate.ToString("dd-MMM-yyyy");
                    lblRegValidUptoDate.InnerText = registration.ValidUptoDate.ToString("dd-MMM-yyyy");
                    lblCandidateType.InnerText = registration.ApplicantType.Name;
                    lblPreviousCourse.Text = "Course Name : " + currentCourseName + " , Registration No : " + registrationNumber;

                    var prevQualification = (from r in context.RegistrationDetails
                                             where r.RegistrationNo == registrationNumber
                                                  && r.RegistrationStatusID == registrationStatusCompleted
                                                  && r.CompletionYear != null
                                             orderby r.CourseID descending
                                             select r);
                    if (prevQualification.Count() > 0)
                    {
                        var p = prevQualification.FirstOrDefault();
                        lblPrevQual.Text = "Course : " + p.Course.Name + " , Registration No : " + p.RegistrationNo + " , Passing Year: " + p.CompletionYear;
                    }
                    else
                    {
                        lblPrevQual.Text = "NA";
                    }

                }

                ListItem lst = new ListItem("--Select One--", "0");
                Course currentCourse = context.Courses.Find(currentCourseID);
                var courses = from s in context.Courses
                              where s.CourseTypeID == currentCourse.CourseTypeID && s.DisplayOrder >= currentCourseID
                              && s.CourseCategoryID == currentCourse.CourseCategoryID
                              select new { ValueField = s.ID, TextField = s.Name };

                Int32 RegStatusProjectPending = Convert.ToInt32(enmRegistrationStatus.ProjectPending);
                Int32 RegStatusCompleted = Convert.ToInt32(enmRegistrationStatus.Completed);
                //string sql = "select distinct  c.ID as ValueField ,c.Name  as TextField   from Course c where c.Course_Category_ID ='" + currentCourse.CourseCategoryID + "' and  c.ID NOT IN  " +
                //             " (Select  Course_ID from Registration_Detail where  Candidate_ID  ='" + registration.CandidateID + "' and " +
                //             " Registration_Status_ID In( " + RegStatusProjectPending + "," + RegStatusCompleted + ")) order by c.ID ";
                //DataTable dtTbl = DbUtility.GetDataTable(sql, new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
                //December_2024
                SqlParameter[] param1 = { new SqlParameter("@currentCourseCourseCategoryID", currentCourse.CourseCategoryID),
                                            new SqlParameter("@registrationCandidateID", registration.CandidateID),
                                                new SqlParameter("@regStatusProjectPending", RegStatusProjectPending),
                                                    new SqlParameter("@regStatusCompleted", RegStatusCompleted),
                                                        };
                string sql = "select distinct  c.ID as ValueField ,c.Name  as TextField   from Course c where c.Course_Category_ID =@currentCourseCourseCategoryID and  c.ID NOT IN  " +
                           " (Select  Course_ID from Registration_Detail where  Candidate_ID  =@registrationCandidateID and " +
                           " Registration_Status_ID In( @regStatusProjectPending,@regStatusCompleted)) order by c.ID ";
                DataTable dtTbl = DbUtility.GetDataTable(sql, new EConnect.Connections.SqlCon(), param1, CommandType.Text, false);

                EConnect.Utils.Common.ControlUtility.BindListObject(DDLRegForCourse, dtTbl, lst);
                bindReligion();
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //Added for Apaar
    protected void ShowCandidateDetails()
    {
        try
        {
            if (ddlNewRegistration.SelectedValue == "C")
            {
                using (EConnectContext context = new EConnectContext())
                {
                    var candidate = (from c in context.Candidates
                                     join r in context.RegistrationDetails on c.ID equals r.CandidateID
                                     where c.ID == entityID
                                         // && c.CourseID ==r.CourseID
                                     && (r.RegistrationStatusCode.ToString() == "R" || r.RegistrationStatusCode.ToString() == "G")
                                     orderby c.ID descending
                                     select c).FirstOrDefault();
                    if (candidate != null)
                    {
                        txtAppName.Text = candidate.Name.ToString();
                        txtDob.Text = candidate.DateOfBirth.ToString("dd-MMM-yyyy");
                        txtGender.Text = candidate.Gender.ToString();

                    }
                }
            }

            else
            {
                using (EConnectContext context = new EConnectContext())
                {
                    var candidate = (from c in context.Candidates
                                     join r in context.RegistrationDetails on c.ID equals r.CandidateID
                                     where c.ID == entityID
                                         // && c.CourseID ==r.CourseID
                                     && r.RegistrationStatusCode.ToString() == "P"
                                     orderby c.ID descending
                                     select c).FirstOrDefault();
                    if (candidate != null)
                    {
                        txtAppName.Text = candidate.Name.ToString();
                        txtDob.Text = candidate.DateOfBirth.ToString("dd-MMM-yyyy");
                        txtGender.Text = candidate.Gender.ToString();

                    }
                };
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }//Apaar End
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
                tdPassedPracticalModules.InnerText = listOfPassedModules.Where(d => d.ModuleTypeID == moduleTypePractical).Count().ToString();
                tdPassedProjectModules.InnerText = listOfPassedModules.Where(d => d.ModuleTypeID == moduleTypeProject).Count().ToString();
                tdRemainingTheorygModules.InnerText = ((theoryCompModules + theoryElectiveModules + bridgeModules) - Convert.ToInt32(tdPassedTheoryModules.InnerText)).ToString();
                tdRemainingPracticalModules.InnerText = (Convert.ToInt32(tdTotalPracticalModules.InnerText) - Convert.ToInt32(tdPassedPracticalModules.InnerText)).ToString();
                tdRemainingProjectModules.InnerText = (Convert.ToInt32(tdTotalProjectModules.InnerText) - Convert.ToInt32(tdPassedProjectModules.InnerText)).ToString();

                ICollection<Module> lstRemainingModules = CourseManager.GetRemainingModules(context, currentCourseID, registrationNumber, currentevisionNumber, entityID);
                int count = DDLRegForCourse.Items.Count;
                if (tdRemainingTheorygModules.InnerText == "0")
                    lstRemainingModules = lstRemainingModules.Where(d => d.ModuleTypeID != (Int32)enmModuleType.Theory).ToList();
                if (lstRemainingModules.Count <= 0)
                {
                    for (int i = 1; i <= count; i++)
                    {
                        if (i <= currentCourseID)
                            DDLRegForCourse.Items.Remove(DDLRegForCourse.Items.FindByValue(i.ToString()));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void bindState()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var state = (from s in context.Locations
                             orderby (s.Name)
                             where s.LocationTypeID == 2 && s.ParentLocationID == 1
                             select new { ValueField = s.ID, TextField = s.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlAccState, state, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void DdlAccState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int stateID = Convert.ToInt32(DdlAccState.SelectedValue);
            Int32 courseID = Convert.ToInt32(DDLRegForCourse.SelectedValue);
            BindAccCentre((Int32)stateID, courseID);
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }
    public void bindDistrict(long stateID, ref DropDownList ddl)
    {
        try
        {
            ddl.Items.Clear();
            int locationTypeID = Convert.ToInt32(enmLocationType.District);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (stateID != 0)
                {
                    var district = from s in context.Locations
                                   orderby (s.Name)
                                   where s.LocationTypeID == locationTypeID && s.ParentLocationID == stateID
                                   select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddl, district.ToList(), lst);
                }

            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void DdlAccDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            districtid = Convert.ToInt32(DdlAccDistrict.SelectedValue);
            stateid = Convert.ToInt32(DdlAccState.SelectedValue);
            Int32 courseID = Convert.ToInt32(DDLRegForCourse.SelectedValue);
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }
    protected void BindAccCentre(int stateid, int courseID)
    {
        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                var AccCentre = (from i in context.Institutes
                                 join d in context.AccreditationDetails on i.ID equals d.InstituteID
                                 orderby i.Name
                                 where i.StateID == stateid && d.CourseID == courseID
                                 select new { ValueField = i.ID, TextField = d.AccreditationNumber + " - " + i.Name + ", " + i.CityName }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlAccCentre, AccCentre, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void bindEducational()
    {
        try
        {
            Int32 ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
            int courseID = Convert.ToInt32(DDLRegForCourse.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                List<Int32> qualificationLevels = context.QualificationEligibility.Where(s => s.CourseID == courseID && s.ApplicantTypeID == ApplicantTypeId && s.EffectiveDateFrom <= DateTime.Now && (s.EffectiveDateTo >= DateTime.Now || s.EffectiveDateTo == null)).OrderByDescending(c => c.EffectiveDateFrom).Select(c => c.QualificationLevelID).ToList();
                //&& s.EffectiveDateFrom <= DateTime.Now).OrderByDescending(c => c.EffectiveDateFrom).Select(c => c.QualificationLevelID).ToList();
                if (qualificationLevels.Count() > 0)
                {
                    Int32 displayOrder = (from t in context.QualificationEligibility
                                          where t.CourseID == courseID && t.ApplicantTypeID == ApplicantTypeId
                                          orderby t.EffectiveDateFrom descending
                                          select t.QualificationLevel.DisplayOrder.Value).Min();
                    ListItem lst = new ListItem("--Select One--", "0");
                    //var education = from p in context.EducationalQualifications
                    //                where p.QualificationLevel.DisplayOrder >= displayOrder
                    //                orderby (p.DisplayOrder)
                    //                select new { ValueField = p.ID, TextField = p.Name };
                    //EConnect.Utils.Common.ControlUtility.BindListObject(DDLeducode, education, lst);


                    var education = from p in context.EducationalQualifications
                                    join q in context.QualificationEligibility on p.QualificationLevelID equals q.QualificationLevelID
                                    where q.CourseID == courseID
                                    && q.ApplicantTypeID == ApplicantTypeId
                 && q.EffectiveDateFrom <= System.DateTime.Now
                                         && (q.EffectiveDateTo >= System.DateTime.Now || q.EffectiveDateTo == null)
                                    select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(DDLeducode, education, lst);

                }
            };
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
    protected void RdoUndergngDOEACC_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (RdoUndergngDOEACC.SelectedValue == "I")
            {
                TrLastCenterAccno.Visible = true;
                //TrLastCenterInstiName.Visible = true;
                TrAccCentre.Visible = true;
                TrExperience.Visible = false;
                ddlPaymentOption.Enabled = true;
            }
            else
            {
                TrExperience.Visible = true;
                TrLastCenterAccno.Visible = false;
                TrLastCenterInstiName.Visible = false;
                TrAccCentre.Visible = false;
                ddlPaymentOption.SelectedValue = "1";
                ddlPaymentOption.Enabled = false;
            }
            if (ddlNewRegistration.SelectedValue == "R")
            {
                ddlPaymentOption.SelectedValue = "1";
                ddlPaymentOption.Enabled = false;
            }
            Int32 ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
            Int32 courseID = Convert.ToInt32(DDLRegForCourse.SelectedValue);
            bindExamName(ApplicantTypeId, courseID);
            bindEducational();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }
    }
    protected void bindExamName(Int32 ApplicantTypeId, Int32 courseID)
    {
        try
        {
            Int32 NormalactivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
            Int32 LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee);
            DateTime dateallowed = new DateTime(2017, 4, 25);
            using (EConnectContext context = new EConnectContext())
            {
                Int32 examID = 0;

                if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                {
                    Int64 appID = Convert.ToInt64(Request.QueryString["Appid"]);
                    var app = context.CourseRegistrationApplications.Find(appID);
                    courseID = app.CourseID;

                }
                var LateFeeExam = (from e in context.CutOffDates
                                   join i in context.Exams on e.ExamID equals i.ID
                                   where e.CourseID == courseID
                                   && e.ApplicantTypeID == ApplicantTypeId
                                   && e.ActivityID == LateFeeActivityId
                                   && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                   orderby e.EfferctiveDate ascending
                                   select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
                if (LateFeeExam.Count() > 0)//If  applicable for late fee ?
                {

                    if (LateFeeExam != null)
                    {
                        LblExamName.Text = LateFeeExam.FirstOrDefault().ExamName;
                        examID = LateFeeExam.FirstOrDefault().ExamID;
                    }
                }
                else if (LateFeeExam.Count() <= 0)//If  not applicable for late fee ?
                {

                    var NormalFeeExam = (from e in context.CutOffDates
                                         join i in context.Exams on e.ExamID equals i.ID
                                         where e.CourseID == courseID
                                         && e.ApplicantTypeID == ApplicantTypeId
                                         && e.ActivityID == NormalactivityId
                                         && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                         orderby e.EfferctiveDate ascending
                                         select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
                    if (NormalFeeExam.Count() > 0)
                    {
                        LblExamName.Text = NormalFeeExam.FirstOrDefault().ExamName;
                        examID = NormalFeeExam.FirstOrDefault().ExamID;
                    }

                }
                ViewState["LoginUserExamID"] = examID.ToString();
                ShowFeeDetail(examID, courseID);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowFeeDetail(Int32 examID, Int32 courseID)
    {
        try
        {
            //int courseID = Convert.ToInt32(DDLRegForCourse.SelectedValue);

            if (examID != 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 cscProcessingFee = 0;
                    if (Request.QueryString["Src"] != null)
                    {
                        if (Request.QueryString["Src"] == "CSC")
                        {
                            Int32 applicationtypeid = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                            Int32 activityid = Convert.ToInt32(enmCSCActivity.FillFormandDepositFee);
                            cscProcessingFee = CourseManager.GetCSCProcessingCharge(context, applicationtypeid, activityid);
                        }
                    }
                    var course = context.Courses.Find(courseID);
                    //LblFeeCourseName.Text = course.Name;
                    ImgBtnPopupFee.Enabled = true;
                    ImgBtnPopupFee.ImageUrl = "~/images/popup1.jpg";
                    ImgBtnPopupFee.ToolTip = "Click here to view Fee Detail.";
                    //int FeeTypeID = Convert.ToInt32(enmFeeType.RegistrationFee);

                    // Deep  add code on 15 July 2021 and comment above line FeeTypeID only for new registration
                    Int32 FeeTypeID = 0;
                    if (ddlNewRegistration.SelectedValue == "R")              // Re-Registration 
                    { FeeTypeID = Convert.ToInt32(enmFeeType.ReRegistrationFee); }
                    else { FeeTypeID = Convert.ToInt32(enmFeeType.RegistrationFee); }

                    // Deep end code on 15 July 2021

                    LblFeeTypeName.Text = Convert.ToString(enmFeeType.RegistrationFee);
                    int ExamId = examID;
                    //int ExamCycleId = Convert.ToInt32(DdlExamCycle.SelectedValue);
                    Int32 feeAmount = 0;

                    feeAmount = (from f in context.FeeDetails
                                 where f.CourseID == courseID && f.FeeTypeID == FeeTypeID &&
                                  f.EffectiveFromDate == (from c in context.FeeDetails
                                                          where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                                                          select c.EffectiveFromDate).Max()
                                 select new { FeeAmount = f.FeeAmount }).FirstOrDefault().FeeAmount;
                    LblNormalFee.Text = feeAmount.ToString("F");
                    LblTotalFee.Text = (feeAmount + cscProcessingFee).ToString("F");
                    lblProcessingFee.Text = cscProcessingFee.ToString("F");
                    lblFeeDetail.Text = "Fee : Rs/- " + (feeAmount + cscProcessingFee) + ".00";

                    int LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee);

                    int NormalFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);

                    int ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
                    int LateFeeTypeId = Convert.ToInt32(enmFeeType.LateFeeRegistration);

                    var Fee = (from f in context.CutOffDates
                               where f.CourseID == courseID &&
                               f.ExamID == ExamId
                               //f.ApplicantTypeID == ApplicantTypeId
                               select new { EffectiveDate = f.EfferctiveDate, f.ActivityID, f.ApplicantTypeID }).ToList();

                    var NormalFee = Fee.Where(l => l.ActivityID == NormalFeeActivityId);
                    var lateFee = Fee.Where(l => l.ActivityID == LateFeeActivityId && l.ApplicantTypeID == ApplicantTypeId);
                    if (lateFee.Count() > 0 && lateFee != null)
                    {
                        if (NormalFee.FirstOrDefault().EffectiveDate <= DateTime.Now)
                        {
                            Int64 LatefeeAmount = (from f in context.FeeDetails
                                                   where f.CourseID == courseID && f.FeeTypeID == LateFeeTypeId &&
                                                   f.EffectiveFromDate == (from c in context.FeeDetails
                                                                           where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                                                                           select c.EffectiveFromDate).Max()
                                                   select new { FeeAmount = f.FeeAmount }).FirstOrDefault().FeeAmount;
                            if (LatefeeAmount != null)
                            {
                                if (LatefeeAmount > 0)
                                {
                                    lblFeeDetail.Text = "Fee : Rs/- " + (feeAmount + LatefeeAmount + cscProcessingFee) + ".00";
                                    LblNormalFee.Text = feeAmount.ToString("F");
                                    LblLateFee.Text = LatefeeAmount.ToString("F");
                                    lblProcessingFee.Text = cscProcessingFee.ToString("F");
                                    LblTotalFee.Text = (feeAmount + LatefeeAmount + cscProcessingFee).ToString("F");

                                }
                            }
                        }
                    }


                };
                LblAmountInWords.Text = " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(LblTotalFee.Text.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
            }
            else
            {
                ImgBtnPopupFee.ToolTip = "";
                ImgBtnPopupFee.Enabled = false;
                ImgBtnPopupFee.ImageUrl = "~/images/DisablePopup.PNG";
                lblFeeDetail.Text = "";
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }
    }
    protected void DDLRegForCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LblExamName.Text = "";
            BreadCrumb1.Render();
            Int32 ApplicantTypeId = Convert.ToInt32(enmApplicantType.Direct);
            Int32 courseID = Convert.ToInt32(DDLRegForCourse.SelectedValue);
            bindExamName(ApplicantTypeId, courseID);

            using (EConnectContext context = new EConnectContext())
            {
                var experience = context.QualificationEligibility.Where(s => s.CourseID == courseID && s.ApplicantTypeID == ApplicantTypeId && s.EffectiveDateFrom <= DateTime.Now).FirstOrDefault();
                if (experience != null)
                {
                    HfExperience.Value = TxtExperienceInYears.Text.Trim() != "" ? TxtExperienceInYears.Text : "0";
                }
            };
            bindState();
            DdlAccCentre.Items.Clear();
            DdlAccCentre.Items.Add(new ListItem("--Select One--", "0"));
            bindEducational();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidForm())
            {

                //Validation of apaar start
               
                string gender = txtGender.Text.Substring(0, 1).ToUpper();
                long apaarRequestId;
                string validatedApaarData = validateApaar.ConvertApaarDatatoJSONandEncrypt(txtapaar.Text, txtAppName.Text, txtDob.Text, gender, txtproviderName.Text, ddlAuthMode.SelectedItem.Text, txtAuthenticationIdNo.Text, ddlConsentRelation.SelectedItem.Text, txtConsentPlace.Text, lblApaarDeclaration.Text, out apaarRequestId);


                if (String.IsNullOrWhiteSpace(validatedApaarData))
                {
                    throw new Exception("Apaar could not be validated.");
                }

                JObject apaarObj = JObject.Parse(validatedApaarData);

                string status =
                  apaarObj["status"] != null
                   ? apaarObj["status"].ToString().Trim().ToLower()
                     : "";

                string statusCode =
                    apaarObj["status_code"] != null
                    ? apaarObj["status_code"].ToString().Trim()
                    : "";

                string messageCode =
                    apaarObj["message_code"] != null
                    ? apaarObj["message_code"].ToString().Trim()
                    : "";

                string message =
                    apaarObj["message"] != null
                    ? apaarObj["message"].ToString().Trim()
                    : "";


                using (EConnectContext db = new EConnectContext())
                {
                    apaarResponseRecd responseObj = new apaarResponseRecd();

                    // ApaarID
                    responseObj.abc_account_id = txtapaar.Text;
                    if (apaarObj["message"].ToString() != "Records not found")
                    {
                        responseObj.abc_account_id = txtapaar.Text;
                        // cname
                        responseObj.cname =
                            apaarObj["CNAME"] != null
                            ? apaarObj["CNAME"].ToString()
                            : "";

                        // gender
                        if (apaarObj["GENDER"] != null)
                        {
                            string g =
                                apaarObj["GENDER"]
                                .ToString()
                                .Trim()
                                .ToUpper();

                            if (g == "M")
                                responseObj.genderID = 1;

                            else if (g == "F")
                                responseObj.genderID = 2;

                            else if (g == "T")
                                responseObj.genderID = 3;
                        }

                        //// dob
                        //if (apaarObj["DOB"] != null)
                        //{
                        //    DateTime parsedDob;

                        //    if (
                        //        DateTime.TryParse(
                        //            apaarObj["DOB"].ToString(),
                        //            out parsedDob
                        //        )
                        //    )
                        //    {
                        //        responseObj.dob = parsedDob;
                        //    }
                        //}
                        if (apaarObj["DOB"] != null)
                        {
                            DateTime parsedDob;
                           // DateTime enteredDob;

                            if ( DateTime.TryParseExact(apaarObj["DOB"].ToString(),"dd/MM/yyyy",CultureInfo.InvariantCulture ,DateTimeStyles.None, out parsedDob))
                              
                            {
                                // Compare complete date (day/month/year)
                               /* if (parsedDob.Date == enteredDob.Date)
                                {
                                    responseObj.dob = parsedDob;

                                }
                                else
                                {
                                    throw new Exception("DOB does not match with APAAR record.");
                                }*/
				responseObj.dob = parsedDob;
                            }
                            else
                            {
                                throw new Exception("Invalid DOB format.");
                            }

                        }
                    }
                    else
                    {
                        responseObj.status = false;
                        //bit values insert data according to api rsponse
                        responseObj.nameMatch = false;
                        responseObj.birthYearMatch = false;
                        responseObj.genderMatch = false;
                    }
                    // status insert , db takes bit value
                    string apiStatus =
                        apaarObj["status"] != null
                        ? apaarObj["status"].ToString().Trim().ToLower()
                        : "";

                    if (apiStatus == "success")
                    {
                        responseObj.status = true;
                        //bit values insert data according to api rsponse
                        responseObj.nameMatch = true;
                        responseObj.birthYearMatch = true;
                        responseObj.genderMatch = true;
                    }
                    else
                    {
                        responseObj.status = false;
                        if (apaarObj["message"].ToString() == "Records not found")
                        {
                            responseObj.status = false;
                            //bit values insert data according to api rsponse
                            responseObj.nameMatch = false;
                            responseObj.birthYearMatch = false;
                            responseObj.genderMatch = false;
                        }
                        else
                        {
                            //bit values insert data according to api rsponse
                            if (apaarObj["match_data_status"] != null)
                            {
                                JObject matchObj =
                                    (JObject)apaarObj["match_data_status"];

                                //if (matchObj["student_name_match"] != null)
                                //{
                                //    responseObj.nameMatch =
                                //        Convert.ToBoolean(
                                //            matchObj["student_name_match"]
                                //        );
                                //}
                                if (matchObj["student_name_match"] != null)
                                {
                                    responseObj.nameMatch =
                                        Convert.ToBoolean(
                                            matchObj["student_name_match"].ToString()
                                        );
                                }

                                if (matchObj["year_of_birth"] != null)
                                {
                                    responseObj.birthYearMatch =
                                        Convert.ToBoolean(
                                            matchObj["year_of_birth"].ToString()
                                        );
                                }

                                if (matchObj["gender_match"] != null)
                                {
                                    responseObj.genderMatch =
                                        Convert.ToBoolean(
                                            matchObj["gender_match"].ToString()
                                        );
                                }
                            }
                        }
                    }

                    // status code 
                    responseObj.statuscode =
                        apaarObj["status_code"] != null
                        ? apaarObj["status_code"].ToString()
                        : "";

                    // status code 
                    responseObj.status_code =
                        apaarObj["status_code"] != null
                        ? apaarObj["status_code"].ToString()
                        : "";

                    // message
                    responseObj.message =
                        apaarObj["message"] != null
                        ? apaarObj["message"].ToString()
                        : "";

                    // full json response
                    responseObj.responseContent =
                        validatedApaarData;

                    // ENTER BY
                    responseObj.enterByID = 99;

                    // enter date
                    responseObj.enterDate =
                        DateTime.Now;


                    // messageCode
                    responseObj.messageCode =
                        apaarObj["message_code"] != null
                        ? apaarObj["message_code"].ToString()
                        : "";


                    // apaar requestID
                    responseObj.apaarReqID =
                        apaarRequestId;

                    db.apaarResponseRecd.Add(responseObj);
                    db.SaveChanges();
                }

                if (message == "Records not found")
                    throw new Exception(message);

                if (status == "fail")
                {
                    // mismatch validations

                    JObject matchObj =
                        (JObject)apaarObj["match_data_status"];

                    bool nameMatch =
                        matchObj["student_name_match"] != null
                        ? Convert.ToBoolean(
                            matchObj["student_name_match"].ToString()
                          )
                        : false;

                    bool dobMatch =
                        matchObj["year_of_birth"] != null
                        ? Convert.ToBoolean(
                            matchObj["year_of_birth"].ToString()
                          )
                        : false;

                    bool genderMatch =
                        matchObj["gender_match"] != null
                        ? Convert.ToBoolean(
                            matchObj["gender_match"].ToString()
                          )
                        : false;

                    if (!nameMatch)
                    {
                        throw new Exception(
                            "APAAR validation failed : Name does not match."
                        );
                    }

                    if (!dobMatch)
                    {
                        throw new Exception(
                            "APAAR validation failed : Date of Birth does not match."
                        );
                    }

                    if (!genderMatch)
                    {
                        throw new Exception(
                            "APAAR validation failed : Gender does not match."
                        );
                    }

                    throw new Exception(message);
                }

                //Check for duplicate Apaar for Exam and Course
                string apaarEncryptedCheck = EncryptDecrypt.EncryptString(txtapaar.Text);
                Int64 applId = Convert.ToInt64(Request.QueryString["Appid"]);
                EConnectContext context1 = new EConnectContext();
               // Course currentCourse = context1.Courses.Find(Convert.ToInt32(Request.QueryString["id"]));
                //Int32 CourseId = currentCourse.ID;
		Int32 CourseId = Convert.ToInt32(DDLRegForCourse.SelectedValue); 
                Int32 ExamId= Convert.ToInt32(ViewState["LoginUserExamID"]);
		string duplicate = checkDuplicateApaar(apaarEncryptedCheck, CourseId, ExamId);

                    if(duplicate!="0")
                    {
                        if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                        {
                          Int64   applId1 = Convert.ToInt64(Request.QueryString["Appid"]);
                            //This Query will get all the information of Applied Canditate by generated Application id
                            CourseRegistrationApplication oldApplication = context1.CourseRegistrationApplications.Find(applId1);
                            if(oldApplication .Number!=duplicate)
                                throw new Exception("Duplicate application for the exam cycle not allowed, Check status for " + duplicate.ToString() + " Application");
                        }
                        else
                        throw new Exception("Duplicate application for the exam cycle not allowed, Check status for "+ duplicate.ToString()+" Application");
                        
                    }

                //End Apaar Code
                using (EConnectContext context = new EConnectContext())
                {
                    Int64 applID = 0;
                    Int32 courseid = 0;
                    var registration = (from c in context.RegistrationDetails
                                        where c.CandidateID == entityID
                                        orderby c.CommencementFromDate descending
                                        select c).FirstOrDefault();
                    if (registration != null)
                    {
                        registrationNumber = registration.RegistrationNo;
                    }
                    var application1 = (from a in context.CourseRegistrationApplications
                                        where a.CandidateID == entityID && a.FinalSubmitted != true
                                            && a.RegisteredCourseRegistrationNo == registrationNumber
                                        orderby a.ApplicationDate descending
                                        select a).FirstOrDefault();
                    if (application1 != null)
                    {
                        applID = Convert.ToInt64(application1.ID);
                        courseid = Convert.ToInt32(application1.CourseID);
                        UpdateData(applID, courseid, apaarRequestId);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                        {
                            btnSave.Text = "Update";
                            applID = Convert.ToInt64(Request.QueryString["Appid"]);
                            courseid = Convert.ToInt32(Request.QueryString["id"]);
                            UpdateData(applID, courseid, apaarRequestId);
                        }
                        else
                        {
                            btnSave.Text = "Submit";
                            SaveData(apaarRequestId);
                        }
                    }
                };
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public bool IsValidForm()
    {
        try
        {
            if (DDLRegForCourse.SelectedValue == "0")
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Please Select Course Name";
                return false;
            }
            if (RdoUndergngDOEACC.SelectedValue == "I")
            {
                if (DdlAccState.SelectedValue == "0")
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "Please Select State of Accredited Institite";
                    return false;
                }

                if (DdlAccCentre.SelectedValue == "0")
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "Please Select Centre of Accredited Institite";
                    return false;
                }
            }
            else if (RdoUndergngDOEACC.SelectedValue == "D")
            {
                if (HfExperience.Value != "0")
                {
                    if (!isBlank(TxtExperienceInYears))
                    {
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.Text = "Candidate Experience can not be left blank";
                        return false;
                    }
                    if (Convert.ToDecimal(TxtExperienceInYears.Text) >= 10)
                    {
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.Text = "Experience should be less than 10 years.";
                        return false;
                    }
                    if (Convert.ToDecimal(TxtExperienceInYears.Text) < Convert.ToDecimal(HfExperience.Value))
                    {
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.Text = "Minimum " + HfExperience.Value + " years of experience required";
                        return false;
                    }
                }
                else
                {
                    if (!isBlank(TxtExperienceInYears))
                    {
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.Text = "Candidate Experience can not be left blank";
                        return false;
                    }
                    if (Convert.ToDecimal(TxtExperienceInYears.Text) >= 10)
                    {
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.Text = "Experience should be less than 10 years.";
                        return false;
                    }
                }
            }
            if (string.IsNullOrEmpty(LblExamName.Text))
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "You cannot apply as no Exam Name found";
                return false;
            }

            if (DDLeducode.SelectedValue == "0")
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Please Select Highest Education";
                return false;
            }
            if (!isBlank(TxtYearOfPassing2))
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Passing Year can not be left blank";
                return false;
            }
            if (!IsNumeric(TxtYearOfPassing2.Text))
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Invalid Highest Education Passing Year ";
                return false;
            }
            if (ddlReligion.SelectedValue == "0")
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Please Select Religion";
                return false;
            }
            if (!isBlank(txtBodyMark))
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Body Mark can not be left blank";
                return false;
            }
            if (chk1.Checked == false)
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Please Select Declaration";
                return false;
            }
            //added validation for new fields start
            if (TrConsentRelation.Visible)
            {
                if (string.IsNullOrEmpty(ddlConsentRelation.SelectedValue))
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "Please select Consent Relation";
                    return false;
                }
            }
            if (TrAuthMode.Visible)
            {
                if (string.IsNullOrEmpty(ddlAuthMode.SelectedValue) || ddlAuthMode.SelectedValue == "0")
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "Please select Authentication Mode";
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(txtAuthenticationIdNo.Text))
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Authentication ID cannot be blank";
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtConsentDate.Text))
            {
                lblError.Text = "Consent Date cannot be blank";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtConsentTime.Text))
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Consent Time cannot be blank";
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtConsentPlace.Text))
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Consent Place cannot be blank";
                return false;
            }
		if (!System.Text.RegularExpressions.Regex.IsMatch(txtConsentPlace.Text, @"^[A-Za-z-\s']+$"))
		{
   		 lblErrorMsg.Visible = true;
   		 lblErrorMsg.Text = "Consent Place can contain only alphabets, one space, and apostrophe.";
   		 return false;
		}

            if (chkApaarDeclaration.Checked != true)
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Please check the Apaar Declaration Statement";
                return false;
                //throw new Exception("Please check the Apaar Declaration Statement");
            }
            //Apaar validation added by ashutosh end 

            //added validation for new fields end
            if (ddlNewRegistration.SelectedValue == "C")
            {
                if (tblRegProcess.Visible == true)
                {
                    if (ChkConfirmCancel.Checked == false)
                    {
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.Text = "Please Select Declaration";
                        return false;
                    }
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isBlank(TextBox txtBox)
    {
        try
        {
            if (txtBox.Text.Trim() == "")
            {
                txtBox.Focus();
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void SaveData(Int64 apaarRequestId)
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    int courseid = 0;
                    var registration = context.RegistrationDetails.Where(c => c.CandidateID == entityID).OrderByDescending(c => c.CommencementFromDate).FirstOrDefault();
                    if (DDLRegForCourse.SelectedValue != "0")
                    {
                        courseid = Convert.ToInt32(DDLRegForCourse.SelectedValue);
                    }
                    else
                    {
                        if (registration != null)
                        {
                            courseid = registration.CourseID;
                        }
                    }

                    Course currentCourse = context.Courses.Find(courseid);
                    CourseRegistrationApplication objRegistration = new EConnect.NIELIT.CourseRegistrationApplication();
                    enmCurrentRegistrationStatus registrationStatus = EConnect.NIELIT.CourseManager.GetCurrentRegistrationStatus(registration.CourseID, entityID);

                    var candidate = context.Candidates.Find(entityID);

                    objRegistration.CourseCategoryID = currentCourse.CourseCategoryID;
                    objRegistration.CourseID = currentCourse.ID;
                    objRegistration.ApplicationDate = DateTime.Now;
                    objRegistration.CandidateID = candidate.ID;
                    objRegistration.ApplicableExamID = Convert.ToInt32(ViewState["LoginUserExamID"]);
                    if (tblRegProcess.Visible == true)
                    {
                        if (RdoUndergngDOEACC.SelectedValue == "I")
                        {
                            objRegistration.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Institute);
                            objRegistration.InstituteID = Convert.ToInt32(DdlAccCentre.SelectedValue);
                            objRegistration.ExperienceInYears = null;
                        }
                        else
                        {
                            objRegistration.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Direct);
                            objRegistration.InstituteID = null;
                            objRegistration.ExperienceInYears = !string.IsNullOrEmpty(TxtExperienceInYears.Text) && !string.IsNullOrWhiteSpace(TxtExperienceInYears.Text) ? Convert.ToDecimal(TxtExperienceInYears.Text) : 0;
                        }
                        if (ddlNewRegistration.SelectedValue == "C")
                        {
                            objRegistration.RegistrationTypeID = Convert.ToInt32(enmRegistrationType.NewOnCancellationRequest);
                        }
                        else
                        {
                            objRegistration.RegistrationTypeID = Convert.ToInt32(enmRegistrationType.ReRegistration);
                        }
                    }
                    else
                    {
                        if (RdoUndergngDOEACC.SelectedValue == "I")
                        {
                            objRegistration.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Institute);
                            objRegistration.InstituteID = Convert.ToInt32(DdlAccCentre.SelectedValue);
                            objRegistration.ExperienceInYears = null;
                        }
                        else
                        {
                            objRegistration.InstituteID = null;
                            objRegistration.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Direct);
                            objRegistration.ExperienceInYears = !string.IsNullOrEmpty(TxtExperienceInYears.Text) && !string.IsNullOrWhiteSpace(TxtExperienceInYears.Text) ? Convert.ToDecimal(TxtExperienceInYears.Text) : 0;
                        }
                        enmCurrentRegistrationStatus registrationStatusID = EConnect.NIELIT.CourseManager.GetCurrentRegistrationStatus(courseid, entityID);
                        if (registrationStatus == enmCurrentRegistrationStatus.RegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndValidityPeriodNotLapsed)
                        {
                            ICollection<Module> lstRemainingModules = CourseManager.GetRemainingModules(context, currentCourseID, registrationNumber, currentevisionNumber, entityID);
                            if (tdRemainingTheorygModules.InnerText == "0")
                                lstRemainingModules = lstRemainingModules.Where(d => d.ModuleTypeID != (Int32)enmModuleType.Theory).ToList();
                            if (lstRemainingModules.Count <= 0)
                            {
                                objRegistration.RegistrationTypeID = Convert.ToInt32(enmRegistrationType.Transfer);
                            }
                            else
                            {
                                objRegistration.RegistrationTypeID = Convert.ToInt32(enmRegistrationType.NewOnCancellationRequest);
                            }
                        }
                        else if (registrationStatusID == enmCurrentRegistrationStatus.CompletedAsAllModulesOfCurrentLevelPassedCompletelyWithinRegistrationPeriod)
                        {
                            objRegistration.RegistrationTypeID = Convert.ToInt32(enmRegistrationType.Transfer);
                        }
                        else if (registrationStatusID == enmCurrentRegistrationStatus.ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired)
                        {
                            objRegistration.RegistrationTypeID = Convert.ToInt32(enmRegistrationType.ReRegistration);
                        }
                        else if (registrationStatusID == enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod)
                        {
                            objRegistration.RegistrationTypeID = Convert.ToInt32(enmRegistrationType.NewOnExpired);
                        }
                        else if (registrationStatusID == enmCurrentRegistrationStatus.CompletedAllModulesPassedInPreviousExamAndEligibleForAutoUpgradation)
                        {
                            objRegistration.RegistrationTypeID = Convert.ToInt32(enmRegistrationType.AutoUpgradation);
                        }
                        else if (registrationStatusID == enmCurrentRegistrationStatus.ReRegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndReRegistrationValidityPeriodNotLapsed)
                        {
                            objRegistration.RegistrationTypeID = Convert.ToInt32(enmRegistrationType.NewOnCancellationRequest);
                        }
                    }
                    //Previous Educational Qualification Details
                    if (registration != null)
                    {
                        objRegistration.AlreadyRegistered = true;
                        objRegistration.RegisteredCourseID = registration.CourseID;
                        objRegistration.RegisteredCourseRegistrationNo = Convert.ToInt32(registration.RegistrationNo);
                    }
                    else
                    {
                        objRegistration.AlreadyRegistered = false;
                    }
                    var prevQualification = (from r in context.RegistrationDetails
                                             where r.RegistrationNo == registration.RegistrationNo
                                                  && r.RegistrationStatusID == registrationStatusCompleted
                                                  && r.CompletionYear != null
                                             orderby r.CourseID descending
                                             select r).FirstOrDefault();
                    if (prevQualification != null)
                    {
                        objRegistration.AlreadyQualified = true;
                        objRegistration.QualifiedCourseID = prevQualification.CourseID;
                        objRegistration.QualifiedCourseRegistrationNo = Convert.ToInt32(prevQualification.RegistrationNo);
                        objRegistration.QualifiedCoursePassingYear = Convert.ToInt32(prevQualification.CompletionYear.Value);
                    }
                    else
                    {
                        objRegistration.AlreadyQualified = false;
                    }

                    //Saving Personal Details
                    string salutation = candidate.Salutation;
                    objRegistration.Salutation = salutation.Trim();
                    objRegistration.Name = candidate.Name;
                    if (string.IsNullOrEmpty(candidate.GuardianName) == true && string.IsNullOrWhiteSpace(candidate.GuardianName) == true)
                    {
                        objRegistration.FatherName = candidate.FatherName;
                        objRegistration.MotherName = candidate.MotherName;
                        objRegistration.GuardianName = "";
                    }
                    else
                    {
                        objRegistration.GuardianName = candidate.GuardianName;
                        objRegistration.FatherName = "";
                        objRegistration.MotherName = "";
                    }

                    if (!string.IsNullOrEmpty(candidate.Gender) == true)
                        objRegistration.Gender = candidate.Gender;
                    else
                        throw new Exception("You cannot apply for new Registration as your Profile Details has not yet updated.Please contact NIELIT Centre for updating your Profile Details.");

                    if (candidate.MaritalStatusID != null)
                        objRegistration.MaritalStatusID = candidate.MaritalStatusID.Value;
                    else
                        throw new Exception("You cannot apply for new Registration as your Marital Status has not yet updated.Please contact NIELIT Centre for updating your Marital Status.");
                    objRegistration.DateOfBirth = candidate.DateOfBirth;
                    if (candidate.CastCategoryID != null)
                        objRegistration.CastCategoryID = candidate.CastCategoryID.Value;
                    else
                        throw new Exception("You cannot apply for new Registration as your Cast Category has not yet updated.Please contact NIELIT Centre for updating your Cast Category.");
                    objRegistration.IsHandicaped = candidate.IsHandicaped;
                    objRegistration.IsExServicemane = candidate.IsExServicemane;

                    objRegistration.ReligionID = Convert.ToInt32(ddlReligion.SelectedValue);
                    objRegistration.BodyMark = txtBodyMark.Text;

                    if (candidate.Photo != null)
                    {
                        objRegistration.PhotoFileName = candidate.Photo.Name.ToString();
                        objRegistration.Photo = candidate.Photo.BlobFile;
                    }
                    if (candidate.Signature != null)
                    {
                        objRegistration.SignatureFileName = candidate.Signature.Name.ToString();
                        objRegistration.Signature = candidate.Signature.BlobFile;
                    }
                    if (candidate.LeftThumbImpression != null)
                    {
                        objRegistration.LeftThumbFileName = candidate.LeftThumbImpression.Name.ToString();
                        objRegistration.LeftThumb = candidate.LeftThumbImpression.BlobFile;
                    }

                    //Contact Details
                    var contact = context.CandidateContactDetails.Where(a => a.CandidateID == candidate.ID).FirstOrDefault();
                    if (contact != null)
                    {
                        if (contact.StdNumber != null)
                            objRegistration.StdNumber = contact.StdNumber.Value;
                        if (contact.PhoneNumber != null)
                            objRegistration.PhoneNumber = contact.PhoneNumber.Value;
                        objRegistration.MobileNumber = contact.MobileNumber.Value;
                        objRegistration.EmailAddress = contact.EmailAddress;
                    }

                    //Permanent Address Details
                    int PerAddTypeId = Convert.ToInt32(enmAddressType.PermanentAddress);
                    var PerAdd = context.Addresses.OrderByDescending(a => a.EffectiveDateFrom).Where(a => a.CandidateID == candidate.ID && a.AddressTypeID == PerAddTypeId).FirstOrDefault();

                    if (string.IsNullOrEmpty(PerAdd.AddressLine1) == true)
                        throw new Exception("You cannot apply for new Registration as your Permanent Address Details has not yet updated.Please contact NIELIT Centre for updating your Permanent Address.");
                    else
                        objRegistration.PerAddressLine1 = PerAdd.AddressLine1;
                    if (string.IsNullOrEmpty(PerAdd.AddressLine2) == true && string.IsNullOrWhiteSpace(PerAdd.AddressLine2) == true)
                        objRegistration.PerAddressLine2 = "NA";
                    else
                        objRegistration.PerAddressLine2 = PerAdd.AddressLine2;
                    objRegistration.PerAddressLine3 = PerAdd.AddressLine3;
                    objRegistration.PerCityName = PerAdd.CityName;
                    if (PerAdd.StateID != null)
                        objRegistration.PerStateID = PerAdd.StateID.Value;
                    else
                        throw new Exception("You cannot apply for new Registration as your Permanent Address Details has not yet updated.Please contact NIELIT Centre for updating your Permanent Address.");
                    if (PerAdd.DistrictID != null)
                        objRegistration.PerDistrictID = PerAdd.DistrictID.Value;
                    if (PerAdd.PinCode != null)
                        objRegistration.PerPinCode = PerAdd.PinCode.Value;
                    else
                        throw new Exception("You cannot apply for new Registration as your Permanent Address Details has not yet updated.Please contact NIELIT Centre for updating your Permanent Address.");


                    //Correspondence Address Details
                    int CorAddTypeId = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                    var CorAdd = context.Addresses.Where(cad => cad.CandidateID == entityID && cad.AddressTypeID == CorAddTypeId).OrderByDescending(cad => cad.EffectiveDateFrom).FirstOrDefault();

                    if (CorAdd != null)
                    {
                        if (string.IsNullOrEmpty(CorAdd.AddressLine1) == true)
                            throw new Exception("You cannot apply for new Registration as your Correspondence Address Details has not yet updated.Please contact NIELIT Centre for updating your Correspondence Address.");
                        else
                            objRegistration.CorAddressLine1 = CorAdd.AddressLine1;

                        if (string.IsNullOrEmpty(CorAdd.AddressLine2) == true && string.IsNullOrWhiteSpace(CorAdd.AddressLine2) == true)
                            objRegistration.CorAddressLine2 = "NA";
                        else
                            objRegistration.CorAddressLine2 = CorAdd.AddressLine2;
                        objRegistration.CorAddressLine3 = CorAdd.AddressLine3;

                        objRegistration.CorCityName = CorAdd.CityName.ToString();
                        if (CorAdd.StateID != null)
                            objRegistration.CorStateID = CorAdd.StateID.Value;
                        else
                            throw new Exception("You cannot apply for new Registration as your Correspondence Address Details has not yet updated.Please contact NIELIT Centre for updating your Correspondence Address.");
                        if (CorAdd.DistrictID != null)
                            objRegistration.CorDistrictID = CorAdd.DistrictID.Value;
                        if (CorAdd.PinCode != null)
                            objRegistration.CorPinCode = CorAdd.PinCode.Value;
                        else
                            throw new Exception("You cannot apply for new Registration as your Correspondence Address Details has not yet updated.Please contact NIELIT Centre for updating your Correspondence Address.");
                    }

                    //Qualification Details
                    objRegistration.EducationalQualificationID = Convert.ToInt32(DDLeducode.SelectedValue);
                    objRegistration.PassingYear = Convert.ToInt32(TxtYearOfPassing2.Text);
                    objRegistration.IsVerifiedByInstitute = false;
                    objRegistration.ApplicationSourceID = Convert.ToInt32(enmApplicationSource.Candidate);

                    //Payment Source
                    objRegistration.PaymentSourceID = Convert.ToInt32(ddlPaymentOption.SelectedValue);

                    //Added 17 Dec 2019
                    var courseRegn = (from a in context.CourseRegistrationApplications
                                      where a.CandidateID == candidate.ID
                                      orderby a.FinalSubmissionDate descending
                                      select a).FirstOrDefault();
                    if (courseRegn != null)
                    {
                        objRegistration.GuardianFlagStatusID = courseRegn.GuardianFlagStatusID;
                    }
                    //
                    //Added for apaar addition
                    if (!String.IsNullOrEmpty(txtapaar.Text))
                    {
                        //added on 12 Sept 2024
                        string apaarEncrypted = EncryptDecrypt.EncryptString(txtapaar.Text);
                        objRegistration.apaarID = apaarEncrypted;
                        objRegistration.ApaarRequestID = apaarRequestId;
                        //added on 12 Sept 2024
                    }
                    else
                    {
                        ShowAlert("Enter Apaar");
                        return;
                    }
                    //Apaar end
                    context.CourseRegistrationApplications.Add(objRegistration);
                    context.SaveChanges(); //testing for 4 feb 2022

                    long Appid = objRegistration.ID;
                    scope.Complete();
                    lblErrorMsg.Text = "New record saved.";
                    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/FrmPreview.aspx?Appid=" + Appid + "&Status=" + ddlNewRegistration.SelectedValue.ToString()));
                };
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public void bindReligion()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var religion = from p in context.Religions
                               orderby (p.DisplayOrder)
                               select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlReligion, religion, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void UpdateData(Int64 applID, Int32 courseid, Int64 apaarRequestId)
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Course currentCourse = context.Courses.Find(courseid);
                    //This Query will get all the information of Applied Candidate by generated Application id                  
                    var application = context.CourseRegistrationApplications.Find(applID);

                    application.CourseCategoryID = currentCourse.CourseCategoryID;
                    application.CourseID = currentCourse.ID;
                    application.ApplicationDate = DateTime.Now;
                    application.ApplicableExamID = Convert.ToInt32(ViewState["LoginUserExamID"]);

                    if (RdoUndergngDOEACC.SelectedValue == "I")
                    {
                        application.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Institute);
                        application.InstituteID = Convert.ToInt32(DdlAccCentre.SelectedValue);
                        application.ExperienceInYears = null;
                    }
                    else
                    {
                        application.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Direct);
                        application.InstituteID = null;
                        application.ExperienceInYears = !string.IsNullOrEmpty(TxtExperienceInYears.Text) && !string.IsNullOrWhiteSpace(TxtExperienceInYears.Text) ? Convert.ToDecimal(TxtExperienceInYears.Text) : 0;
                    }

                    // Deep add code on 15 July 2021 For ReRegistration save after submit form (another pages )
                    if (ddlNewRegistration.SelectedValue == "C")
                    {
                        application.RegistrationTypeID = Convert.ToInt32(enmRegistrationType.NewOnCancellationRequest);
                    }
                    else if (ddlNewRegistration.SelectedValue == "R")
                    {
                        application.RegistrationTypeID = Convert.ToInt32(enmRegistrationType.ReRegistration);
                    }

                    // Deep End code on 15 July 2021 For ReRegistration save after submit form (comes another pages )

                    //Personal Details
                    application.ReligionID = Convert.ToInt32(ddlReligion.SelectedValue);
                    application.BodyMark = txtBodyMark.Text;

                    //Qualification Details
                    application.EducationalQualificationID = Convert.ToInt32(DDLeducode.SelectedValue);
                    application.PassingYear = Convert.ToInt32(TxtYearOfPassing2.Text);

                    //Payment Source Details
                    application.PaymentSourceID = Convert.ToInt32(ddlPaymentOption.SelectedValue);
                    //Added for apaar addition
                    if (!String.IsNullOrEmpty(txtapaar.Text))
                    {
                        //added on 12 Sept 2024
                        string apaarEncrypted = EncryptDecrypt.EncryptString(txtapaar.Text);
                        application.apaarID = apaarEncrypted;
                        //added on 12 Sept 2024
                    }
                    else
                    {
                        ShowAlert("Enter Apaar");
                        return;
                    }
                    application.ApaarRequestID = apaarRequestId;
                    //Apaar end
                    context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    long Appid = application.ID;
                    scope.Complete();
                    lblErrorMsg.Text = "Record Updated.";
                    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/FrmPreview.aspx?Appid=" + Appid + "&Status=" + ddlNewRegistration.SelectedValue.ToString()));
                };
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlNewRegistration_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LblExamName.Text = "";
            BreadCrumb1.Render();
            if (ddlNewRegistration.SelectedValue == "R")
            {
                ddlPaymentOption.SelectedValue = "1";
                ddlPaymentOption.Enabled = false;
                DDLRegForCourse.Enabled = false;
                trConfirm.Visible = false;
                ChkConfirmCancel.Enabled = false;

                using (EConnectContext context = new EConnectContext())
                {
                    var registration = (from c in context.RegistrationDetails
                                        where c.CandidateID == entityID
                                        orderby c.CommencementFromDate descending
                                        select c).FirstOrDefault();
                    if (registration != null)
                    {
                        DDLRegForCourse.SelectedValue = registration.CourseID.ToString();
                        DDLRegForCourse_SelectedIndexChanged("", System.EventArgs.Empty);
                        //Added for Apaar
                        Int64 applID = Convert.ToInt64(Request.QueryString["Appid"]);
                        var application = (from a in context.CourseRegistrationApplications
                                           where a.CandidateID == entityID && a.FinalSubmitted == true
                                           select a).FirstOrDefault();
                        txtAppName.Text = application.Name.ToString();
                        txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");
                        txtGender.Text = application.Gender.ToString();
                        //Apaar End
                    }
                };
            }
            else
            {
                //ddlPaymentOption.SelectedValue = "1";
                ddlPaymentOption.Enabled = true;
                DDLRegForCourse.Enabled = true;
                TxtExperienceInYears.Enabled = true;
                RdoUndergngDOEACC.Enabled = true;
                ImgBtnPopupFee.Enabled = true;
                trConfirm.Visible = true;
                ChkConfirmCancel.Enabled = true;
                DdlAccCentre.Enabled = true;
                DdlAccState.Enabled = true;
                //Added for Apaar
                EConnectContext context = new EConnectContext();
                Int64 applID = Convert.ToInt64(Request.QueryString["Appid"]);
                var application = (from a in context.CourseRegistrationApplications
                                   where a.CandidateID == entityID && a.FinalSubmitted == true
                                   select a).FirstOrDefault();
                txtAppName.Text = application.Name.ToString();
                txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");
                txtGender.Text = application.Gender.ToString();
                //Apaar End
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void DDLeducode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HfExperience.Value = "0";
            lblExperienceInYears.Text = "";
            int courseID = Convert.ToInt32(DDLRegForCourse.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                Int32 qualificationID = Convert.ToInt32(DDLeducode.SelectedValue);
                Int32 ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);

                var qual = context.EducationalQualifications.Find(qualificationID);
                if (qual != null)
                {
                    decimal experience = (from k in context.QualificationEligibility
                                          where k.CourseID == courseID && k.ApplicantTypeID == ApplicantTypeId && k.QualificationLevelID == qual.QualificationLevelID
                                          orderby k.EffectiveDateFrom descending
                                          select new { k.Experience, k.EffectiveDateFrom }).Select(k => k.Experience).FirstOrDefault();
                    if (experience != null)
                    {
                        HfExperience.Value = experience > 0 ? experience.ToString() : "0";
                        if (HfExperience.Value != "0")
                        {
                            lblExperienceInYears.Visible = true;
                            lblExperienceInYears.Text = "<font color='red'>* </font>(Minimum " + HfExperience.Value + " year(s) of experience required)";
                        }
                    }
                    else
                    {
                        HfExperience.Value = "0";
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected Int32 GetCurrentExam(Int32 ApplicantTypeId, Int32 courseID)
    {
        try
        {
            Int32 NormalactivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
            Int32 LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee);
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CurrentexamID = 0;
                var LateFeeExam = (from e in context.CutOffDates
                                   join i in context.Exams on e.ExamID equals i.ID
                                   where e.CourseID == courseID
                                   && e.ApplicantTypeID == ApplicantTypeId
                                   && e.ActivityID == LateFeeActivityId
                                   && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                   orderby e.EfferctiveDate ascending
                                   select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
                if (LateFeeExam.Count() > 0)//If  applicable for late fee ?
                {

                    if (LateFeeExam != null)
                    {
                        CurrentexamID = LateFeeExam.FirstOrDefault().ExamID;
                    }
                }
                else if (LateFeeExam.Count() <= 0)//If  not applicable for late fee ?
                {

                    var NormalFeeExam = (from e in context.CutOffDates
                                         join i in context.Exams on e.ExamID equals i.ID
                                         where e.CourseID == courseID
                                         && e.ApplicantTypeID == ApplicantTypeId
                                         && e.ActivityID == NormalactivityId
                                         && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                         orderby e.EfferctiveDate ascending
                                         select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
                    if (NormalFeeExam.Count() > 0)
                    {
                        CurrentexamID = NormalFeeExam.FirstOrDefault().ExamID;
                    }
                }
                return CurrentexamID;
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected string checkDuplicateApaar(string apaarEncryptedCheck, int CourseId, int ExamId)
    {
        string duplicate = "0";
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var candRecord = (from c in context.CourseRegistrationApplications
                                  where c.CourseID == CourseId && c.ApplicableExamID == ExamId
                                        && c.apaarID == apaarEncryptedCheck
                                  select new { Number = c.Number }).FirstOrDefault();
                if (candRecord == null)
                    return duplicate;
                if (candRecord.Number != null)
                    duplicate = Convert.ToString(candRecord.Number);
                return duplicate;
            }
        }

        catch (Exception ex)
        {
            return "-99";
        }
    }

    protected void BindAuthMode(int whether18)
    {
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);

            SqlCommand cmd = new SqlCommand("BIND_AUTH_MODE", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@whether18", whether18);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            da.Fill(dt);

            ddlAuthMode.DataSource = dt;
            ddlAuthMode.DataTextField = "AuthModeName";
            ddlAuthMode.DataValueField = "AuthModeValue";
            ddlAuthMode.DataBind();

            ddlAuthMode.Items.Insert(0, new ListItem("--Select--", "0"));

        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.Visible = true;
            ShowAlert(ex.Message);
        }
    }
    protected void txtapaar_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(txtDob.Text))
            {
                throw new Exception("Invalid Date of Birth. Fill in correct format example : 01-Jan-2000");

            }
            DateTime todaydate = DateTime.Now;
            DateTime Inputdate = Convert.ToDateTime(txtDob.Text);
            int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);

            txtAuthenticationIdNo.Text = "";
            ddlAuthMode.SelectedIndex = 0;
            ddlConsentRelation.SelectedIndex = 0;
            txtproviderName.Text = "";
            txtConsentDate.Text = "";
            txtConsentTime.Text = "";
            txtConsentPlace.Text = "";

            if (countAge >= 0)
            {
                txtIsProviderPresent.Text = "True";
                txtIsProviderPresent.Enabled = false;

                ddlConsentRelation.Items.Clear();
                ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
                ddlConsentRelation.Items.Add(new ListItem("Self", "1"));

                ddlConsentRelation.SelectedIndex = 1;
                ddlConsentRelation.Enabled = false;
                ddlConsentRelation_SelectedIndexChanged(sender, e);

                txtproviderName.Text = txtAppName.Text;
                txtproviderName.Enabled = false;

                BindAuthMode(countAge);
                ddlAuthMode.SelectedValue = "1";

                ddlAuthMode.Enabled = false;
                //ddlAuthMode.Enabled = false;
                txtAuthenticationIdNo.Text = txtapaar.Text;
                txtAuthenticationIdNo.Enabled = false;


            }
            else
            {
                txtIsProviderPresent.Text = "True";
                txtIsProviderPresent.Enabled = false;

                ddlConsentRelation.Enabled = true;
                txtproviderName.Enabled = true;
                ddlAuthMode.Enabled = true;
                txtAuthenticationIdNo.Enabled = true;
                //if (Rdoownertype.SelectedValue == "P")
                //{
                //    ddlConsentRelation.Items.Clear();
                //    ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
                //    ddlConsentRelation.Items.Add(new ListItem("Father", "3"));
                //    ddlConsentRelation.Items.Add(new ListItem("Mother", "4"));
                //}
                //else
                //{
                //    ddlConsentRelation.Items.Clear();
                //    ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
                //    ddlConsentRelation.Items.Add(new ListItem("Guardian", "2"));
                //}
                //ddlConsentRelation.SelectedIndex = 0;
                //txtproviderName.Text = "";
                ddlConsentRelation.Items.Clear();

                ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
                ddlConsentRelation.Items.Add(new ListItem("Guardian", "2"));
                ddlConsentRelation.Items.Add(new ListItem("Father", "3"));
                ddlConsentRelation.Items.Add(new ListItem("Mother", "4"));

                ddlConsentRelation.SelectedIndex = 0;

                txtproviderName.Text = "";
                // txtproviderName.Text = txtproviderName.Text;
                // txtproviderName.ReadOnly = true;

                BindAuthMode(countAge);
                ddlAuthMode.SelectedIndex = 0;

            }
            txtConsentDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtConsentDate.Enabled = false;

            txtConsentTime.Text = DateTime.Now.ToString("HH:mm");
            txtConsentTime.Enabled = false;

            txtConsentPlace.Text = txtConsentPlace.Text;
            BindApaarDeclaration(countAge);
        }

        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.Visible = true;
            ShowAlert(ex.Message);
        }

    }
    protected void ddlAuthMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (ddlAuthMode.SelectedItem.Text.ToLower() == "self")
            {
                txtAuthenticationIdNo.Text = txtapaar.Text.ToString();
                txtAuthenticationIdNo.ReadOnly = true;
            }
            else
            {
                txtAuthenticationIdNo.Text = "";
                txtAuthenticationIdNo.ReadOnly = false;
            }

            DateTime todaydate = DateTime.Now;
            DateTime Inputdate = Convert.ToDateTime(txtDob.Text);
            int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);
            BindApaarDeclaration(countAge);
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.Visible = true;
            ShowAlert(ex.Message);
        }
    }
    private void BindApaarDeclaration(int countAge)
    {
        string relationText = "";
        string wardText = "";
        string authDoc = "";

        if (countAge >= 0)
        {
            relationText = txtproviderName.Text + "(" + ddlConsentRelation.SelectedItem.Text + ")";
            wardText = "self";
        }
        else
        {
            relationText = txtproviderName.Text + "(" + ddlConsentRelation.SelectedItem.Text + ")";
            wardText = "ward";
        }

        authDoc = txtAuthenticationIdNo.Text;

        lblApaarDeclaration.Text =
                                  "I "
                                  + relationText +
                                  ", hereby voluntarily give my consent to NIELIT to use APAAR ID of "
                                  + txtAppName.Text +
                                  " (" + wardText + ") with APAAR ID as "
                                  + txtapaar.Text +
                                  " for validation of personal details."

                                  + "I understand that the APAAR ID may be used and shared only for limited, authorized purposes, "
                                  + "and that the information provided by me shall be kept confidential."

                                  + "The information w.r.t authentication document no. "
                                  + authDoc +
                                  " provided by me, is correct and valid to the best of my knowledge.";
    }
    protected void ddlConsentRelation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string fNAME = "";
            string mNAME = "";
            string gNAME = "";


            using (EConnectContext context = new EConnectContext())
            {
                var candidate = (from c in context.Candidates
                                 join r in context.RegistrationDetails on c.ID equals r.CandidateID
                                 where c.ID == entityID
                                     // && c.CourseID ==r.CourseID
                                 && (r.RegistrationStatusCode.ToString() == "R" || r.RegistrationStatusCode.ToString() == "G" || r.RegistrationStatusCode.ToString() == "P")
                                 orderby c.ID descending
                                 select c).FirstOrDefault();
                if (candidate != null)
                {
                    fNAME = candidate.FatherName ?? "";
                    mNAME = candidate.MotherName ?? "";
                    gNAME = candidate.GuardianName ?? "";

                }
            }

            txtproviderName.Text = "";

            switch (ddlConsentRelation.SelectedValue)
            {
                case "1":
                    txtproviderName.Text = txtAppName.Text;
                    break;

                case "2":

                    if (string.IsNullOrWhiteSpace(gNAME))
                    {
                        ddlConsentRelation.SelectedIndex = 0;
                        txtproviderName.Text = "";

                        ShowAlert("Guardian not mentioned at the time of registration.", true);
                        return;
                    }

                    txtproviderName.Text = gNAME;
                    break;

                case "3":
                    txtproviderName.Text = fNAME;
                    break;

                case "4":
                    txtproviderName.Text = mNAME;
                    break;

                default:
                    txtproviderName.Text = "";
                    break;
            }
            txtproviderName.Enabled = false;
            DateTime todaydate = DateTime.Now;
            DateTime inputdate = Convert.ToDateTime(txtDob.Text);

            int countAge = DateTime.Compare(todaydate.AddYears(-18), inputdate);

            BindApaarDeclaration(countAge);
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.Visible = true;
            ShowAlert(ex.Message);
        }
    }
    protected void txtAuthenticationIdNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(txtDob.Text))
            {
                throw new Exception("Invalid Date of Birth. Fill in correct format example : 01-Jan-2000");

            }
            int countAge = 0;

            if (ViewState["CountAge"] != null)
            {
                countAge = Convert.ToInt32(ViewState["CountAge"]);
            }

            BindApaarDeclaration(countAge);
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.Visible = true;
            ShowAlert(ex.Message);
        }
    }
}