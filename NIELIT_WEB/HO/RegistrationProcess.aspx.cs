using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Common;

public partial class RegistrationProcess : BasePage
{
    String strMessage = string.Empty;
    Table tbl = new Table();
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int16 verifiedNotCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            //if (IsSessionAlive() == false)
            //{ Response.Redirect("../Index.aspx"); }
            //currentRoleId = Convert.ToInt32(Session["RoleID"]);
            //loginUserNo = Convert.ToInt32(Session["UserID"]);
            //if (!UserManager.HasRight(currentRoleId, enmRight.View))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {
                lblError.Visible = false;

                if (!String.IsNullOrEmpty(Request.QueryString["batchItemID"]))
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        var batch = context.BatchItems.Find(Convert.ToInt32(Request.QueryString["batchItemID"]));
                        int BatchId = batch.BatchID;
                        CourseRegistrationApplication cr = new CourseRegistrationApplication();
                        cr = context.CourseRegistrationApplications.Find(batch.CourseRegistrationApplicationID);
                        int courseId = cr.CourseID;
                        int CandidateTypeId = cr.ApplicantTypeID;
                        Int32 examYear = Convert.ToInt32(cr.ApplicableExam.ExamYear);
                        Int32 ApplicationStatusId = Convert.ToInt32(cr.ApplicationStatusID);
                        BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Registration Process", "/HO/RegistrationProcess.aspx?BatchId=" + BatchId + "&courseId=" + courseId + "&CandidateTypeId=" + CandidateTypeId + "&ExamYear=" + examYear + "&ApplicationStatusID=" + ApplicationStatusId, ""));
                        ShowEditMode();
                    };
                }
                else
                {
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registration Process", "#", ""));
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    btnMode.Visible = false;
                    bindCourse();
                    bindCandidateType();
                    BindGridView();
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                { ShowAlert(Request.QueryString["msg"].ToString()); }
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    public void bindCourse()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseTypeId = Convert.ToInt32(enmCourseType.CertificationCourse);
                ddlCourse.Items.Clear();
                ListItem lst = new ListItem("--Select One--", "0");
                //Modified 18 Jul 2019
                var courses = from s in context.Courses
                              where s.CourseTypeID == CourseTypeId
                               && s.IsActive 
			//  	Updated 13 Dec 2022 from s.ShowOnWeb to s.IsActive
                                 select new { ValueField = s.ID, TextField = s.Name+" ("+s.Code +")" };
                              //select new { ValueField = s.ID, TextField = s.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, courses, lst);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void bindCandidateType()
    {
        try
        {
            EnumUtility.BindListObject(ref ddlCandidateType, typeof(EConnect.NIELIT.enmApplicantType), new ListItem("--Select One--", "0"));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void bindBatchNumber(int CourseId, int CandidateTypeId, Int32 examYear)
    {
        try
        {
            int BatchStatusId = Convert.ToInt32(enmBatchStatus.Completed);
            Int32 ApplicationStatus = Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var courses = from s in context.Batchs
                              where (s.CourseID == CourseId && s.ApplicantTypeID == CandidateTypeId
                                     && s.StatusID == BatchStatusId && s.Exam.ExamYear == examYear)
                              select new { ValueField = s.ID, TextField = s.Number };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatchNumber, courses, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowEditMode()
    {

        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                int id = Convert.ToInt32(Request.QueryString["batchItemID"]);
                Int32 ApplicationStatus = Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing);
                Int32 ApplicationStatus1 = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                BatchItem obj = context.BatchItems.Find(id);
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 2;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                lblHeading.Text = "Candidate Detail";
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;

                CourseRegistrationApplication cr = new CourseRegistrationApplication();
                cr = context.CourseRegistrationApplications.Find(obj.CourseRegistrationApplicationID);
                if (cr.ApplicationStatusID == ApplicationStatus1) // for Processed Applications
                {
                    BtnProcess2.Visible = false;
                    DivRegdetail.Visible = false;
                    var registrationDetail = (from r in context.RegistrationDetails
                                              where r.CourseRegistrationApplicationID == cr.ID
                                              orderby r.CommencementFromDate descending
                                              select r).FirstOrDefault();
                    if (registrationDetail != null)
                    {
                        divTbl.Visible = true;
                        Int64 regno = Convert.ToInt64(registrationDetail.RegistrationNo);
                        lblAppno.Text = cr.Number;
                        lblAppDate.Text = cr.ApplicationDate.ToString("dd-MMM-yyyy");
                        lblExamName.Text = cr.ApplicableExam.Name;
                        lblCurrntRegNo.Text = registrationDetail.RegistrationNo.ToString();
                        lblCurrntRegDate.Text = registrationDetail.RegistrationDate.ToString("dd-MMM-yyyy");
                        lblCurrntRegCourse.Text = cr.Course.Name;
                        lblCommncmntFrmDate.Text = registrationDetail.CommencementFromDate.ToString("dd-MMM-yyyy");
                        lblValidUpToDate.Text = registrationDetail.ValidUptoDate.ToString("dd-MMM-yyyy");
                    }
                }
                else //  for Not Processed Applications
                {
                    BtnProcess2.Visible = true;
                    divTbl.Visible = false;
                    enmCurrentRegistrationStatus regStatus = GetCurrentRegistrationStatus(Convert.ToInt32(Request.QueryString["batchItemID"]), context);
                    if (regStatus != enmCurrentRegistrationStatus.None)
                        lblRegStatus.Text = EConnect.Utils.Common.EnumUtility.GetDescription(regStatus);
                    if (regStatus == enmCurrentRegistrationStatus.ReRegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndReRegistrationValidityPeriodNotLapsed)//NewOnCancellationRequest
                    {
                        trddlConfirm.Visible = true;
                    }
                    else
                    {
                        trddlConfirm.Visible = false;
                    }
                }
                if (!String.IsNullOrEmpty(Request.QueryString["ApplTypeID"]))
                {
                    candidateDetail.ApplicationTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(obj.CourseRegistrationApplication.Name + "(" + obj.CourseRegistrationApplicationID.ToString() + ")", "/HO/RegistrationProcess.aspx?batchItemID=" + Request.QueryString["batchItemID"].ToString(), ""));
                }
                else
                {
                    candidateDetail.ApplicationTypeID = cr.ApplicantTypeID;
                    BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem(obj.CourseRegistrationApplication.Name + "(" + obj.CourseRegistrationApplicationID.ToString() + ")", "/HO/RegistrationProcess.aspx?batchItemID=" + Request.QueryString["batchItemID"].ToString(), ""));
                }
                candidateDetail.BatchItemID = Convert.ToInt32(Request.QueryString["batchItemID"]);
                candidateDetail.Bind();
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected enmCurrentRegistrationStatus GetCurrentRegistrationStatus(Int32 batchItemID, EConnectContext context) //checking Registration status
    {
        try
        {
            CourseRegistrationApplication cr = new CourseRegistrationApplication();
            enmCurrentRegistrationStatus reasons = enmCurrentRegistrationStatus.None;
            if (batchItemID != 0)
            {
                BatchItem batchItem = context.BatchItems.Find(batchItemID);
                cr = context.CourseRegistrationApplications.Find(batchItem.CourseRegistrationApplicationID);
                if (cr.AlreadyRegistered == true)
                {
                    var currentRegistration = (from c in context.RegistrationDetails
                                               where c.CandidateID == cr.CandidateID
                                               orderby c.CommencementFromDate descending
                                               select c).FirstOrDefault();
                    lblRegNo.Text = cr.RegisteredCourseRegistrationNo.ToString();
                    lblRegDate.Text = currentRegistration.RegistrationDate.ToString("dd-MMM-yyyy");
                    lblRegCourse.Text = cr.RegisteredCourse.Name;
                    var reRegistrationPolicy = context.CourseRegistrationPolicies.Where(a => a.CourseID == currentRegistration.CourseID && a.EffectiveFromDate <= DateTime.Now).OrderByDescending(c => c.EffectiveFromDate).FirstOrDefault();
                  
                    if (currentRegistration.enmRegistrationStatus == enmRegistrationStatus.Completed)
                    {
                        //Current Level Registration status is completed
                        //Valdate whether eligible for auto upgradation or not
                        reasons = enmCurrentRegistrationStatus.CompletedAsAllModulesOfCurrentLevelPassedCompletelyWithinRegistrationPeriod;
                    }
                    else if (currentRegistration.enmRegistrationStatus == enmRegistrationStatus.Cancelled)
                    {
                        reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
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
                                    reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                                }
                                else
                                {
                                    //Registration validity period is over but eligible for re-registration
                                    reasons = enmCurrentRegistrationStatus.ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired;
                                }
                            }
                            else
                            {
                                //Registration validity period is over and no chances for reregistration
                                reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                            }
                        }
                        else
                        {
                            reasons = enmCurrentRegistrationStatus.RegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndValidityPeriodNotLapsed;
                        }
                    }
                    else if (currentRegistration.enmRegistrationStatus == enmRegistrationStatus.Registered)
                    {
                        //If valid upto date is less  than current date: Registration expired
                        if (DateTime.Now > currentRegistration.ValidUptoDate)
                        {
                            if (reRegistrationPolicy.ReRegistrationChance == true)
                            {
                                //if (DateTime.Now > currentRegistration.ValidUptoDate.AddMonths(reRegistrationPolicy.ReRegistrationGapInMonths.Value))
                                //compare application date with valid upto date rather than current date.

                                if (cr.ApplicationDate.Date > currentRegistration.ValidUptoDate.AddMonths(reRegistrationPolicy.ReRegistrationGapInMonths.Value).Date)
                                {
                                    //Registration validity period is over and no chances for reregistration
                                    reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                                }
                                else
                                {
                                    //On 29 April,2014 changes in the code if the candidate is selected the registration type as 'New on cancellation request' even if he is available for re-registration then set the status applicable for Cancellation
                                    if (cr.RegistrationTypeID == Convert.ToInt32(enmRegistrationType.NewOnCancellationRequest))
                                    {
                                        reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                                    }
                                    else
                                    {
                                        //Registration validity period is over but eligible for re-registration
                                        reasons = enmCurrentRegistrationStatus.ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired;
                                    }
                                }
                            }
                            else
                            {
                                //Registration validity period is over and no chances for reregistration
                                reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                            }
                        }
                        else
                        {
                            int currentRevisionNo = CourseManager.GetCourseRevisionNumberAtRegistrationCompleted(context, currentRegistration.CourseID, currentRegistration.RegistrationNo, currentRegistration.CandidateID);
                            if (ShowModuleSummary(currentRegistration.CourseID, currentRegistration.RegistrationNo, currentRevisionNo, currentRegistration.CandidateID) <= 0)
                            {
                                //Current Level Registration status is completed
                                //Valdate whether eligible for auto upgradation or not
                                reasons = enmCurrentRegistrationStatus.CompletedAsAllModulesOfCurrentLevelPassedCompletelyWithinRegistrationPeriod;
                            }
                            else
                            {
                                //Registration validity period is over but eligible for re-registration
                                reasons = enmCurrentRegistrationStatus.RegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndValidityPeriodNotLapsed;
                            }
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
                                    reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                                }
                                else
                                {
                                    //Registration validity period is over but eligible for re-registration
                                    reasons = enmCurrentRegistrationStatus.ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired;
                                }
                            }
                            else
                            {
                                //Registration validity period is over and no chances for reregistration
                                reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
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
                            reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                        }
                        else
                        {
                            //Under re-registration validity period
                            reasons = enmCurrentRegistrationStatus.ReRegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndReRegistrationValidityPeriodNotLapsed;
                        }
                    }
                    //if (cr.RegistrationTypeID == Convert.ToInt16(enmRegistrationType.RegistrationMercy))
                    //{
                    //    reasons = enmCurrentRegistrationStatus.EligibleForNewRegistrationAsPreviousRegistrationNotFound;
                    //    //  UpdateRegistrationForMercyCase(batchItemID, context);
                    //}
                }
                else if (cr.AlreadyRegistered == false)
                {
                    reasons = enmCurrentRegistrationStatus.EligibleForNewRegistrationAsPreviousRegistrationNotFound;
                }

            }
            return reasons;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void BtnProcess2_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int64 batchItemID = Convert.ToInt32(Request.QueryString["batchItemID"]);
                List<StudentList> stList = new List<StudentList>();
                stList.Add(RegistrationProcessing(batchItemID));
                // sending sms and email
                var evnt = context.NotificationEvent.Find(Convert.ToInt32(enmNotificationEvents.AfterSuccessfullRegistration));
                if (evnt.SendEmail == true)
                {
                    Thread threademail = new Thread(() => SentBulkEmail(stList, Convert.ToInt32(Session["UserID"])));
                    threademail.Start();
                }
                if (evnt.SendSms == true)
                {
                    Thread threadsms = new Thread(() => SentBulkSMS(stList, Convert.ToInt32(Session["UserID"])));
                    threadsms.Start();
                }
                BatchItem batchitem = context.BatchItems.Find(batchItemID);
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem(batchitem.CourseRegistrationApplication.Name + "(" + batchitem.CourseRegistrationApplicationID.ToString() + ")", "/HO/RegistrationProcess.aspx?batchItemID=" + Request.QueryString["batchItemID"].ToString(), ""));
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }
    }
    private Int64 SaveCandidateRecord(Int64 courseRegAppID, EConnectContext context)
    {
        try
        {
            Int64 candidateID = 0;
            var application = (from a in context.CourseRegistrationApplications
                               where a.ID == courseRegAppID
                               select a).FirstOrDefault();
            Candidate objCandidate;
            objCandidate = new EConnect.NIELIT.Candidate();

            objCandidate.Salutation = application.Salutation;
            objCandidate.Name = application.Name;

            objCandidate.FatherName = application.FatherName;
            objCandidate.MotherName = application.MotherName;
            objCandidate.GuardianName = application.GuardianName;

            objCandidate.Gender = application.Gender;
            objCandidate.MaritalStatusID = application.MaritalStatusID;
            objCandidate.DateOfBirth = application.DateOfBirth;
            objCandidate.CastCategoryID = application.CastCategoryID;
            objCandidate.IsHandicaped = application.IsHandicaped;
            objCandidate.IsExServicemane = application.IsExServicemane;
            objCandidate.ReligionID = application.ReligionID;
            objCandidate.BodyMark = application.BodyMark;
            objCandidate.EffectiveFromDate = DateTime.Now;
            context.Candidates.Add(objCandidate);
            context.SaveChanges();

            UploadedFile objPhoto = new EConnect.NIELIT.UploadedFile();
            objPhoto.Name = "LR-P-" + objCandidate.ID;
            objPhoto.OriginalName = application.PhotoFileName;
            objPhoto.BlobFile = application.Photo;
            objPhoto.Extension = System.IO.Path.GetExtension(application.PhotoFileName).ToLower();
            objPhoto.UploadedOn = DateTime.Now;
            context.UploadedFiles.Add(objPhoto);
            context.SaveChanges();

            UploadedFile objSignature = new EConnect.NIELIT.UploadedFile();
            objSignature.Name = "LR-S-" + objCandidate.ID;
            objSignature.OriginalName = application.SignatureFileName;
            objSignature.BlobFile = application.Signature;
            objSignature.Extension = System.IO.Path.GetExtension(application.SignatureFileName).ToLower();
            objSignature.UploadedOn = DateTime.Now;
            context.UploadedFiles.Add(objSignature);
            context.SaveChanges();

            UploadedFile objThumb = new EConnect.NIELIT.UploadedFile();
            objThumb.Name = "LR-T-" + objCandidate.ID;
            objThumb.OriginalName = application.LeftThumbFileName;
            objThumb.BlobFile = application.LeftThumb;
            objThumb.Extension = System.IO.Path.GetExtension(application.LeftThumbFileName).ToLower();
            objThumb.UploadedOn = DateTime.Now;
            context.UploadedFiles.Add(objThumb);
            context.SaveChanges();

            objCandidate.PhotoFileID = objPhoto.ID;
            objCandidate.SignatureFileID = objSignature.ID;
            objCandidate.LeftThumbImpressionFileID = objThumb.ID;
            context.Entry(objCandidate).State = System.Data.Entity.EntityState.Modified;

            CandidateContactDetail objContact = new EConnect.NIELIT.CandidateContactDetail();
            objContact.CandidateID = objCandidate.ID;
            if (application.StdNumber.HasValue)
            { objContact.StdNumber = application.StdNumber.Value; }
            if (application.PhoneNumber.HasValue)
            { objContact.PhoneNumber = application.PhoneNumber.Value; }
            objContact.MobileNumber = application.MobileNumber;
            objContact.EmailAddress = application.EmailAddress;
            objContact.EffectiveFromDate = DateTime.Now;
            context.CandidateContactDetails.Add(objContact);
            context.SaveChanges();

            //permanant Address detail
            Address objAddress = new EConnect.Address();
            objAddress.AddressLine1 = application.PerAddressLine1;
            objAddress.AddressLine2 = application.PerAddressLine2;
            objAddress.AddressLine3 = application.PerAddressLine3;
            objAddress.StateID = application.PerStateID;
            if (application.PerDistrictID.HasValue)
            { objAddress.DistrictID = application.PerDistrictID.Value; }
            objAddress.PinCode = application.PerPinCode;
            objAddress.AddressTypeID = Convert.ToInt32(enmAddressType.PermanentAddress);
            objAddress.CandidateID = objCandidate.ID;
            objAddress.CountryID = 1; ///need to change in future
			//Added 23 Oct 2019
			objAddress.CityName=application.PerCityName;
            objAddress.EffectiveDateFrom = DateTime.Now;
            objAddress.CreatedOn = DateTime.Now;
            context.Addresses.Add(objAddress);
            context.SaveChanges();

            //Correspondance Address detail
            Address objAddress2 = new EConnect.Address();
            objAddress2.AddressLine1 = application.CorAddressLine1;
            objAddress2.AddressLine2 = application.CorAddressLine2;
            objAddress2.AddressLine3 = application.CorAddressLine3;
            objAddress2.StateID = application.CorStateID;
            if (application.CorDistrictID.HasValue)
            { objAddress2.DistrictID = application.CorDistrictID.Value; }
            objAddress2.PinCode = application.CorPinCode;
            objAddress2.AddressTypeID = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
            objAddress2.CandidateID = objCandidate.ID;
            objAddress2.CountryID = 1;
			 //Added 23 Oct 2019
           objAddress2.CityName = application.CorCityName;
            objAddress2.EffectiveDateFrom = DateTime.Now;
            objAddress2.CreatedOn = DateTime.Now;
            context.Addresses.Add(objAddress2);
            context.SaveChanges();

            CandidateQualificationDetail objQualification = new CandidateQualificationDetail();
            objQualification.CandidateID = objCandidate.ID;
            objQualification.EducationalQualificationID = application.EducationalQualificationID;
            objQualification.PassingYear = application.PassingYear;
            objQualification.EffectiveFromDate = DateTime.Now;
            context.CandidateQualificationDetails.Add(objQualification);
            context.SaveChanges();

            //update candidate-Id in course Registration Application
            application.CandidateID = objCandidate.ID;
            context.Entry(application).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            candidateID = objCandidate.ID;

            return candidateID;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private Int64 SaveNewRegistrationRecord(Int64 courseRegAppID, Int64 batchItemID, Int64 candidateID, EConnectContext context)
    {
        try
        {
            Int64 regno = 0;
            Int64 newRegno = 0;
            var application = (from a in context.CourseRegistrationApplications
                               where a.ID == courseRegAppID
                               select a).FirstOrDefault();
            if (application != null)
            {
                var batch = context.BatchItems.Find(batchItemID);
                batch.StatusID = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                context.Entry(batch).State = System.Data.Entity.EntityState.Modified;

                int BatchId = (Int32)batch.BatchID;
                RegistrationDetail objRegistration = new EConnect.NIELIT.RegistrationDetail();
                objRegistration.CandidateID = candidateID;
                objRegistration.CourseCategoryID = application.CourseCategoryID;
                objRegistration.CourseID = application.CourseID;
                objRegistration.ApplicantTypeID = application.ApplicantTypeID;
                var app = GetRegistrationData(application.CourseID, BatchId, application.ApplicantTypeID, courseRegAppID, context);
                string commencementDetail = GetCommencementDetail(application.CourseID, BatchId, application.ApplicantTypeID, courseRegAppID, context);
                string[] commencement = commencementDetail.Split(',');
                DateTime CommencementFromDate = Convert.ToDateTime(commencement[0]);
                DateTime CommencementToDate = Convert.ToDateTime(commencement[1]);

                objRegistration.CommencementFromDate = CommencementFromDate;
                objRegistration.ValidUptoDate = CommencementToDate;
                objRegistration.WhetherExtension = false;

                objRegistration.RegistrationStatusID = Convert.ToInt32(enmRegistrationStatus.Registered);
                if (application.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute))
                {
                    objRegistration.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Institute);
                    objRegistration.InstituteID = application.InstituteID;
                }
                else
                {
                    objRegistration.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Direct);
                    objRegistration.ExperienceInYears = Convert.ToDecimal(application.ExperienceInYears);
                }
                objRegistration.CourseRegistrationApplicationID = courseRegAppID;

                //New code 
                var status = context.RegistrationStatus.Find(Convert.ToInt32(enmRegistrationStatus.Registered));
                if (application.RegistrationTypeID == Convert.ToInt32(enmRegistrationType.New))
                {
                    //To obtain new registration no from registration_Counter table
                    newRegno = Convert.ToInt64(EConnect.Utils.Data.DbUtility.ExecuteScaller("Update Registration_No_Counter set No_of_Reg = No_of_Reg + 1 OUTPUT INSERTED.No_of_Reg as regno ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                    objRegistration.RegistrationNo = newRegno;
                    objRegistration.RegistrationMonth = DateTime.Now.Month;
                    objRegistration.RegistrationYear = DateTime.Now.Year;
                    objRegistration.RegistrationDate = DateTime.Now;
                    objRegistration.RegistrationStatusCode = status.Code;
                    objRegistration.RegistrationTypeID = Convert.ToInt32(application.RegistrationTypeID);//New
                }
                else
                {
                    var registration = (from s in context.RegistrationDetails
                                        where s.CandidateID == candidateID
                                        orderby s.CommencementFromDate descending
                                        select s).FirstOrDefault();

                    if (application.RegistrationTypeID == Convert.ToInt32(enmRegistrationType.NewOnCancellationRequest) ||
                        application.RegistrationTypeID == Convert.ToInt32(enmRegistrationType.NewOnExpired))
                    {
                        //To obtain new registration no from registration_Counter table
                        //modification on 8 Nov 2014 to generate same registration number in next level even if the candidate submit the project after filling of online registration form.
                        if (registration.enmRegistrationStatus == enmRegistrationStatus.Completed)
                        {
                            objRegistration.RegistrationNo = registration.RegistrationNo;
                            enmCurrentRegistrationStatus regStatus = GetCurrentRegistrationStatus(Convert.ToInt32(application.BatchItemID.Value), context);

                            if (regStatus != enmCurrentRegistrationStatus.None)
                                objRegistration.CurrentRegistrationStatusID = Convert.ToInt32(regStatus);
                        }
                        else
                        {
                            newRegno = Convert.ToInt64(EConnect.Utils.Data.DbUtility.ExecuteScaller("Update Registration_No_Counter set No_of_Reg = No_of_Reg + 1 OUTPUT INSERTED.No_of_Reg as regno ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                            objRegistration.RegistrationNo = newRegno;
                            objRegistration.CurrentRegistrationStatusID = registration.CurrentRegistrationStatusID;
                        }
                    }
                    else
                    {
                        objRegistration.RegistrationNo = registration.RegistrationNo;
                        enmCurrentRegistrationStatus regStatus = GetCurrentRegistrationStatus(Convert.ToInt32(application.BatchItemID.Value), context);

                        if (regStatus != enmCurrentRegistrationStatus.None)
                            objRegistration.CurrentRegistrationStatusID = Convert.ToInt32(regStatus);
                    }
                    objRegistration.RegistrationMonth = DateTime.Now.Month;
                    objRegistration.RegistrationYear = DateTime.Now.Year;
                    objRegistration.RegistrationDate = DateTime.Now;
                    objRegistration.RegistrationStatusCode = status.Code;
                    objRegistration.RegistrationTypeID = Convert.ToInt32(application.RegistrationTypeID);
                }
                context.RegistrationDetails.Add(objRegistration);
                context.SaveChanges();
                regno = objRegistration.RegistrationNo;
            }
            return regno;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private Int64 UpdateRe_RegistrationRecord(Int64 courseRegAppID, int BatchId, EConnectContext context)
    {
        try
        {

            Int64 regno = 0;

            var application = (from a in context.CourseRegistrationApplications
                               where a.ID == courseRegAppID
                               select a).FirstOrDefault();

            RegistrationDetail objRegistration = context.RegistrationDetails.Where(s => s.CandidateID == application.CandidateID && s.CourseID == application.CourseID).OrderByDescending(s => s.RegistrationDate).FirstOrDefault();

            if (objRegistration != null && application != null)
            {
                var policy = context.CourseRegistrationPolicies.Where(s => s.CourseID == application.CourseID && s.CourseCategoryID == application.CourseCategoryID).OrderByDescending(s => s.EffectiveFromDate).FirstOrDefault();
                int reRegistrationMonths = policy.ReRegistrationGapInMonths.Value;
                objRegistration.ReRegistrationNumber = (int)objRegistration.RegistrationNo;

                enmCurrentRegistrationStatus regStatus = GetCurrentRegistrationStatus(Convert.ToInt32(application.BatchItemID.Value), context);
                if (regStatus != enmCurrentRegistrationStatus.None)
                    objRegistration.CurrentRegistrationStatusID = Convert.ToInt32(regStatus);

                //Set Re-Registration date as date on which processing is done previous it was coming from previous registration valid upto date plus reg Monts
                objRegistration.ReRegistrationDate = DateTime.Now;
                int examID = GetExamID(application.ApplicantTypeID, application.CourseID, application.ApplicationDate, context);
                var exams = context.Exams.Find(examID);
                int papers = 0;
                if (exams.ExaminationCycle.enmExamSchedule == enmExamSchedule.HalfYearly)
                    papers = 2;
                else if (exams.ExaminationCycle.enmExamSchedule == enmExamSchedule.Monthly)
                    papers = 12;
                else if (exams.ExaminationCycle.enmExamSchedule == enmExamSchedule.Periodic)
                    papers = 3;
                else if (exams.ExaminationCycle.enmExamSchedule == enmExamSchedule.Quarterly)
                    papers = 4;
                else if (exams.ExaminationCycle.enmExamSchedule == enmExamSchedule.Weekly)
                    papers = 52;
                else if (exams.ExaminationCycle.enmExamSchedule == enmExamSchedule.Fortnightly)
                    papers = 24;
                else if (exams.ExaminationCycle.enmExamSchedule == enmExamSchedule.Yearly)
                    papers = 1;

                int totalMonths = 12 / papers;

                DateTime NewValidUptoDate = new DateTime();
                Int64 year = (exams.ExamYear);
                NewValidUptoDate = DateTime.Parse(year.ToString() + "-" + exams.ExamMonth.ToString() + "-01");
                objRegistration.ValidUptoDate = NewValidUptoDate.AddMonths(policy.ReRegistrationValidity.GetValueOrDefault()).AddDays(-1);
                objRegistration.WhetherExtension = true;

                objRegistration.RegistrationStatusID = Convert.ToInt32(enmRegistrationStatus.ReRegistered);
                objRegistration.RegistrationStatusCode = context.RegistrationStatus.Find(Convert.ToInt32(enmRegistrationStatus.ReRegistered)).Code;
                if (application.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute))
                {
                    objRegistration.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Institute);
                    objRegistration.InstituteID = application.InstituteID;
                    objRegistration.ExperienceInYears = null;
                }
                else
                {
                    objRegistration.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Direct);
                    objRegistration.InstituteID = null;
                    objRegistration.ExperienceInYears = Convert.ToDecimal(application.ExperienceInYears);
                }
                objRegistration.CourseRegistrationApplicationID = courseRegAppID;
                objRegistration.RegistrationTypeID = Convert.ToInt32(enmRegistrationType.ReRegistration);
                regno = objRegistration.RegistrationNo;
                context.Entry(objRegistration).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }

            return regno;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void UpdateRegistrationCancelRecord(Int64 courseRegAppID, int BatchId, EConnectContext context)
    {
        try
        {
            var application = (from a in context.CourseRegistrationApplications
                               where a.ID == courseRegAppID
                               select a).FirstOrDefault();

            RegistrationDetail objRegistration = context.RegistrationDetails.Where(s => s.CandidateID == application.CandidateID).OrderByDescending(s => s.RegistrationDate).FirstOrDefault();


            objRegistration.CancelledOn = DateTime.Now;
            objRegistration.CancelledBy = (int?)entityID;
            enmCurrentRegistrationStatus regStatus = GetCurrentRegistrationStatus(Convert.ToInt32(application.BatchItemID.Value), context);
            if (regStatus != enmCurrentRegistrationStatus.None)
                objRegistration.CurrentRegistrationStatusID = Convert.ToInt32(regStatus);
            objRegistration.RegistrationStatusID = Convert.ToInt32(enmRegistrationStatus.Cancelled);
            objRegistration.RegistrationStatusCode = context.RegistrationStatus.Find(Convert.ToInt32(enmRegistrationStatus.Cancelled)).Code;

            context.Entry(objRegistration).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            //Result grading cancelled for previous registration
            var cexamApplDetail = (from cm in context.CourseExamApplicationDetails
                                   where cm.CourseID == objRegistration.CourseID && cm.RegistrationNumber == objRegistration.RegistrationNo
                                   && cm.CandidateID == objRegistration.CandidateID
                                   && cm.ResultGradeID !=12
                                   select cm).ToList();
            if (cexamApplDetail.Count() > 0)
            {
                foreach (var result in cexamApplDetail)
                {
                    CourseExamApplicationDetail objExam = context.CourseExamApplicationDetails.Find(Convert.ToInt64(result.ID));
                    objExam.IsCanceled = true;
                    objExam.CanceledOn = Convert.ToDateTime(DateTime.Now);
                    if (objExam.ResultGradeID.HasValue)
                    {
                        objExam.OldResultGradeID = objExam.ResultGradeID.Value;
                    }
                    objExam.UpdatedByID = Convert.ToInt32(Session["UserID"]);
                    objExam.UpdatedOn = Convert.ToDateTime(DateTime.Now);
                    objExam.ResultGradeID = context.ResultGrades.Where(s => s.CourseCategoryID == 1
                                                              && s.Code == "Z").FirstOrDefault().ID;

                    context.Entry(objExam).State = System.Data.Entity.EntityState.Modified;
                }
                context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void UpdateRegistrationExpiredRecord(Int64 courseRegAppID, int BatchId)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var application = (from a in context.CourseRegistrationApplications
                                   where a.ID == courseRegAppID
                                   select a).FirstOrDefault();

                RegistrationDetail objRegistration = context.RegistrationDetails.Where(s => s.CandidateID == application.CandidateID).OrderByDescending(s => s.RegistrationDate).FirstOrDefault();

                objRegistration.ExpiryDate = DateTime.Now;
                objRegistration.RegistrationStatusID = Convert.ToInt32(enmRegistrationStatus.Expired);
                objRegistration.RegistrationStatusCode = context.RegistrationStatus.Find(Convert.ToInt32(enmRegistrationStatus.Expired)).Code;

                enmCurrentRegistrationStatus regStatus = GetCurrentRegistrationStatus(Convert.ToInt32(application.BatchItemID.Value), context);
                if (regStatus != enmCurrentRegistrationStatus.None)
                    objRegistration.CurrentRegistrationStatusID = Convert.ToInt32(regStatus);

                context.Entry(objRegistration).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            };

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private CourseRegistrationApplication GetRegistrationData(int CourseId, int BatchId, int CandidateTypeId, Int64 CourseRegistrationApplicationID, EConnectContext context)
    {
        try
        {
            int BatchComplete = Convert.ToInt32(enmBatchStatus.Completed);
            int ApplicationVerifiedByNIELIT = Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing);

            var app = (from s in context.CourseRegistrationApplications
                       join b in context.BatchItems on s.ID equals b.CourseRegistrationApplicationID
                       where s.ID == CourseRegistrationApplicationID
                       select s).FirstOrDefault();

            if (app != null)
            {
                return app;
            }
            else
            { return null; }
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }
    private string GetCommencementDetail(int CourseId, int BatchId, int CandidateTypeId, Int64 CourseRegistrationApplicationID, EConnectContext context)
    {
        try
        {
            var app = GetRegistrationData(CourseId, BatchId, CandidateTypeId, CourseRegistrationApplicationID, context);
            if (app != null)
            {
                int examID = GetExamID(app.ApplicantTypeID, app.CourseID, app.ApplicationDate, context);
                var exams = context.Exams.Find(examID);
                var course = context.Courses.Find(CourseId);
                CourseRegistrationPolicy currentPolicy = CourseManager.GetCurrentRegistrationPolicy(context, CourseId);
                int papers = 0;
                if (exams.ExaminationCycle.enmExamSchedule == enmExamSchedule.HalfYearly)
                    papers = 2;
                else if (exams.ExaminationCycle.enmExamSchedule == enmExamSchedule.Monthly)
                    papers = 12;
                else if (exams.ExaminationCycle.enmExamSchedule == enmExamSchedule.Periodic)
                    papers = 3;
                else if (exams.ExaminationCycle.enmExamSchedule == enmExamSchedule.Quarterly)
                    papers = 4;
                else if (exams.ExaminationCycle.enmExamSchedule == enmExamSchedule.Weekly)
                    papers = 52;
                else if (exams.ExaminationCycle.enmExamSchedule == enmExamSchedule.Fortnightly)
                    papers = 24;
                else if (exams.ExaminationCycle.enmExamSchedule == enmExamSchedule.Yearly)
                    papers = 1;

                int totalMonths = 12 / papers;
                DateTime CommencementFromDate = new DateTime();
                var CommencementWithGreaterThanCurrentExamMonth = (from s in context.Exams
                                                                   where s.CourseID == app.CourseID && s.ExamYear == exams.ExamYear && s.ExamMonth > exams.ExamMonth
                                                                   orderby s.ExamMonth descending
                                                                   select s).Take(1).FirstOrDefault();

                if (CommencementWithGreaterThanCurrentExamMonth != null)
                {
                    Int64 year = (exams.ExamYear);
                    CommencementFromDate = DateTime.Parse(exams.ExamYear.ToString() + "-" + exams.ExamMonth.ToString() + "-01");

                }
                else
                {
                    Int64 year = (exams.ExamYear);
                    CommencementFromDate = DateTime.Parse(exams.ExamYear.ToString() + "-" + exams.ExamMonth.ToString() + "-01");

                }

                DateTime CommencementToDate = new DateTime();
                CommencementToDate = CommencementFromDate.AddMonths(currentPolicy.RegistrationValidity).AddDays(-1);
                return CommencementFromDate + "," + CommencementToDate;
            }
            else
                return "";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void ProcessDetail(int CourseId, int BatchId, int CandidateTypeId, Int64 CourseRegistrationApplicationID, Int64 regno, EConnectContext context)
    {
        try
        {
            var app = GetRegistrationData(CourseId, BatchId, CandidateTypeId, CourseRegistrationApplicationID, context);
            string commencementDetail = GetCommencementDetail(CourseId, BatchId, CandidateTypeId, CourseRegistrationApplicationID, context);
            string[] commencement = commencementDetail.Split(',');
            DateTime CommencementFromDate = Convert.ToDateTime(commencement[0]);
            DateTime CommencementToDate = Convert.ToDateTime(commencement[1]);
            if (app != null)
            {
                int examID = GetExamID(app.ApplicantTypeID, app.CourseID, app.ApplicationDate, context);
                var exams = context.Exams.Find(examID);
                lblAppno.Text = app.Number;
                lblAppDate.Text = app.ApplicationDate.ToString("dd-MMM-yyyy");
                lblExamName.Text = exams != null ? exams.Name : "";
                var reg = context.RegistrationDetails.Where(r => r.RegistrationNo == regno && r.CourseID == CourseId && r.CourseRegistrationApplicationID == CourseRegistrationApplicationID).OrderByDescending(r => r.CommencementFromDate).FirstOrDefault();
                lblCurrntRegNo.Text = regno.ToString();
                lblCurrntRegDate.Text = reg.RegistrationDate.ToString("dd-MMM-yyyy");
                lblCurrntRegCourse.Text = context.Courses.Find(CourseId).Name;
                lblCommncmntFrmDate.Text = reg.CommencementFromDate.ToString("dd-MMM-yyyy");
                lblValidUpToDate.Text = reg.ValidUptoDate.ToString("dd-MMM-yyyy");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindGridView()
    {
        try
        {
            lblError.Visible = false;
            tblShowDetail1.Visible = false;
            int courseId = 0;
            int BatchId = 0;
            int CandidateTypeId = 0;
            Int32 ExamYear = 0;
            Int32 ApplicationStatus = 0;
            Int32 applStatusID = 0;
            divGrid.Visible = true;
            PagingBar1.Visible = true;

            if (Convert.ToInt32(ddlCourse.SelectedValue) != 0 && Convert.ToInt32(ddlBatchNumber.SelectedValue) != 0 && Convert.ToInt32(ddlCandidateType.SelectedValue) != 0 && Convert.ToInt32(ddlflExamYear.SelectedValue) != 0)
            {
                courseId = Convert.ToInt32(ddlCourse.SelectedValue);
                BatchId = Convert.ToInt32(ddlBatchNumber.SelectedValue);
                CandidateTypeId = Convert.ToInt32(ddlCandidateType.SelectedValue);
                ExamYear = Convert.ToInt32(ddlflExamYear.SelectedValue);
                BreadCrumb1.RemoveLastBreadCrumbItem();
                BreadCrumb1.RemoveLastBreadCrumbItem();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registration Process", "#", ""));
            }
            else
            {
                courseId = Convert.ToInt32(Request.QueryString["courseId"]);
                BatchId = Convert.ToInt32(Request.QueryString["BatchId"]);
                CandidateTypeId = Convert.ToInt32(Request.QueryString["CandidateTypeId"]);
                ExamYear = Convert.ToInt32(Request.QueryString["ExamYear"]);
                applStatusID = Convert.ToInt32(Request.QueryString["ApplicationStatusID"]);
                bindCandidateType();
                bindCourse();
                FillFilterExamYear(courseId, CandidateTypeId);
                bindBatchNumber(courseId, CandidateTypeId, ExamYear);
                ddlCandidateType.SelectedValue = Convert.ToString(Request.QueryString["CandidateTypeId"]);
                ddlCourse.SelectedValue = Convert.ToString(Request.QueryString["courseId"]);
                ddlflExamYear.SelectedValue = Convert.ToString(Request.QueryString["ExamYear"]);
                ddlBatchNumber.SelectedValue = Convert.ToString(Request.QueryString["BatchId"]);
                if (applStatusID == Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing))
                    ddlApplStatus.SelectedValue = "N";
                else if (applStatusID == Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                    ddlApplStatus.SelectedValue = "P";
            }
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();

            if (courseId != 0 && CandidateTypeId != 0 && BatchId != 0 && ExamYear != 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    if (ddlApplStatus.SelectedValue == "N")
                        ApplicationStatus = Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing);
                    else
                        ApplicationStatus = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);

                    Int32 applTypeID = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                    var registration = from s in context.CourseRegistrationApplications
                                       join b in context.BatchItems on s.ID equals b.CourseRegistrationApplicationID
                                       where s.ApplicationStatusID == ApplicationStatus &&
                                       s.CourseID == courseId && s.ApplicantTypeID == CandidateTypeId
                                       && b.BatchID == BatchId && s.ApplicableExam.ExamYear == ExamYear
                                       select new
                                       {
                                           AppNo = s.Number,
                                           AppDate = s.ApplicationDate,
                                           CandidateName = s.Name,
                                           CandidateType = s.ApplicantType.Name,
                                           Course = s.Course.Name,
                                           DateOfReg = DateTime.Now,
                                           RegNo = "",
                                           commencedFromDate = "",
                                           CourseId = s.CourseID,
                                           CandidateTypeId = s.ApplicantTypeID,
                                           ID = b.ID,
                                           BatchId = b.Batch.ID,
                                           ApplTypeID = applTypeID,
                                           // Add these
                                           RegistrationTypeID = s.RegistrationTypeID,                                       

                                           // Optional but useful
                                           IsMercyCase = s.RegistrationTypeID == 7
                      
                                       };

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        if (IsNumeric(searchString))
                        {
                            long SearchStr = Convert.ToInt64(searchString);
                            registration = registration.Where(s => s.AppNo == SearchStr.ToString());
                        }
                        else
                            registration = registration.Where(s => s.CandidateName.ToUpper().Contains(searchString));
                    }
                    registration = registration.OrderBy(s => s.AppNo);
                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortField)
                        {
                            case "AppNo":
                                if (sortOrder == "DESC")
                                    registration = registration.OrderByDescending(s => s.AppNo);
                                else
                                    registration = registration.OrderBy(s => s.AppNo);
                                break;
                            case "AppDate":
                                if (sortOrder == "DESC")
                                    registration = registration.OrderByDescending(s => s.AppDate);
                                else
                                    registration = registration.OrderBy(s => s.AppDate);
                                break;
                            case "CandidateName":
                                if (sortOrder == "DESC")
                                    registration = registration.OrderByDescending(s => s.CandidateName);
                                else
                                    registration = registration.OrderBy(s => s.CandidateName);
                                break;
                            case "CandidateType":
                                if (sortOrder == "DESC")
                                    registration = registration.OrderByDescending(s => s.CandidateType);
                                else
                                    registration = registration.OrderBy(s => s.CandidateType);
                                break;

                            case "Course":
                                if (sortOrder == "DESC")
                                    registration = registration.OrderByDescending(s => s.Course);
                                else
                                    registration = registration.OrderBy(s => s.Course);
                                break;
                            case "DateOfReg":
                                if (sortOrder == "DESC")
                                    registration = registration.OrderByDescending(s => s.DateOfReg);
                                else
                                    registration = registration.OrderBy(s => s.DateOfReg);
                                break;

                            default:
                                registration = registration.OrderBy(s => s.CandidateName);
                                break;
                        }
                    }
                    PagingBar1.Bind(registration, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    UpnlShow.Update();
                    if (gvMain.Rows.Count > 0)
                    {
                        if (ddlApplStatus.SelectedValue == "N")
                        {
                            btnProcess.Visible = true;
                        }
                        else
                        {
                            btnProcess.Visible = false;
                        }
                        lblError.Visible = false;
                    }
                    else
                    {
                        lblError.Visible = true;
                        btnProcess.Visible = false;
                        lblError.Text = "Sorry ! No Record Found ";
                    }
                };
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "Sorry ! No Record.Please filter data to show records.";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New User";
        }
        else
        {
            using (EConnectContext context = new EConnectContext())
            {
                var batch = context.BatchItems.Find(Convert.ToInt32(Request.QueryString["batchItemID"]));
                int BatchId = batch.BatchID;
                CourseRegistrationApplication cr = new CourseRegistrationApplication();
                cr = context.CourseRegistrationApplications.Find(batch.CourseRegistrationApplicationID);
                int courseId = cr.CourseID;
                int CandidateTypeId = cr.ApplicantTypeID;
                Int32 examYear = Convert.ToInt32(cr.ApplicableExam.ExamYear);
                Int32 ApplicationStatusId = Convert.ToInt32(cr.ApplicationStatusID);
                Response.Redirect("~/HO/RegistrationProcess.aspx?BatchId=" + BatchId + "&courseId=" + courseId + "&CandidateTypeId=" + CandidateTypeId + "&ExamYear=" + examYear + "&ApplicationStatusID=" + ApplicationStatusId);
            };
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SearchBar_Reset(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            ddlBatchNumber.SelectedValue = "0";
            ddlCourse.SelectedValue = "0";
            ddlCandidateType.SelectedValue = "0";
            ddlflExamYear.SelectedValue = "0";
            lblError.Text = "Sorry ! No Record.Please filter data to show records.";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
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
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = hl.NavigateUrl;
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = hl.NavigateUrl;
                HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                h4.NavigateUrl = hl.NavigateUrl;
                HyperLink h5 = (HyperLink)e.Row.Cells[5].Controls[0];
                h5.NavigateUrl = hl.NavigateUrl;
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                Regex regex = new Regex(@"^[0-9]+$");
                if (count <= 0)
                    count = 10;
                List<String> items = new List<String>();
                string searchString = prefixText.Trim().ToUpper();
                int AppStatusId = Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing);
                var reg = from s in context.CourseRegistrationApplications
                          where s.ApplicationStatusID == AppStatusId
                          select new { AppId = s.ID, AppName = s.Name };
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (regex.IsMatch(searchString))
                    {
                        long SearchStr = Convert.ToInt64(searchString);
                        reg = reg.Where(s => s.AppId == SearchStr);

                    }
                    else
                        reg = reg.Where(s => s.AppName.ToUpper().Contains(searchString));
                }
                foreach (var user in reg)
                {

                    if (regex.IsMatch(searchString))
                        items.Add(user.AppId.ToString());
                    else
                        items.Add(user.AppName);

                }
                return items.ToArray();
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int CourseId = Convert.ToInt32(ddlCourse.SelectedValue);
            int CandidateTypeId = Convert.ToInt32(ddlCandidateType.SelectedValue);
            FillFilterExamYear(CourseId, CandidateTypeId);
            ddlflExamYear_SelectedIndexChanged("", System.EventArgs.Empty);
        }
        catch (Exception ex)
        {

            ShowAlert(ex.Message);
        }
    }
    protected void FillFilterExamYear(int CourseId, int CandidateTypeId)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                int BatchStatusId = Convert.ToInt32(enmBatchStatus.Completed);
                Int32 ApplicationStatus = Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing);
                Int32 applstatus = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                ListItem lst = new ListItem("--Select One--", "0");
                //var ExamYear = (from b in context.Batchs
                //                join bi in context.BatchItems on b.ID equals bi.BatchID
                //                where (bi.StatusID == ApplicationStatus || bi.StatusID == applstatus) &&
                //                       b.StatusID == BatchStatusId && bi.CourseRegistrationApplicationID != null
                //                       && b.ApplicantTypeID == CandidateTypeId && b.CourseID == CourseId
                //                orderby (b.Exam.ExamYear)
                //                select new { ValueField = b.Exam.ExamYear, TextField = b.Exam.ExamYear }).Distinct();
                var ExamYear = context.Exams.Where(E => E.CourseID == CourseId).Select(s => new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                ExamYear = ExamYear.OrderByDescending(s => s.ValueField).Take(6);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlflExamYear, ExamYear, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlBatchNumber_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ShowTableHeader()
    {
        TableHeaderRow th = new TableHeaderRow();
        th.CssClass = "head1";

        TableHeaderCell tcCol1 = new TableHeaderCell();
        tcCol1.HorizontalAlign = HorizontalAlign.Left;
        tcCol1.Text = "Application No.";
        th.Cells.Add(tcCol1);

        TableHeaderCell tcCol2 = new TableHeaderCell();
        tcCol2.HorizontalAlign = HorizontalAlign.Right;
        tcCol2.Text = "Application Date";
        th.Cells.Add(tcCol2);

        TableHeaderCell tcCol3 = new TableHeaderCell();
        tcCol3.HorizontalAlign = HorizontalAlign.Left;

        tcCol3.Text = "Exam Name";
        th.Cells.Add(tcCol3);

        TableHeaderCell tcCol4 = new TableHeaderCell();
        tcCol4.HorizontalAlign = HorizontalAlign.Right;
        tcCol4.Text = "Registration Date";
        th.Cells.Add(tcCol4);

        TableHeaderCell tcCol5 = new TableHeaderCell();
        tcCol5.HorizontalAlign = HorizontalAlign.Right;
        tcCol5.Text = "Commencement From Date";
        th.Cells.Add(tcCol5);

        TableHeaderCell tcCol6 = new TableHeaderCell();
        tcCol6.HorizontalAlign = HorizontalAlign.Right;
        tcCol6.Text = "Valid Upto Date";
        th.Cells.Add(tcCol6);

        tbl.Rows.Add(th);

    }
    protected Int32 GetExamID(Int32 ApplicantTypeId, Int32 courseID, DateTime applicationDate, EConnectContext context)
    {
        try
        {
            Int32 NormalactivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
            Int32 LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee);

            Int32 examID = 0;


            var LateFeeExam = (from e in context.CutOffDates
                               join i in context.Exams on e.ExamID equals i.ID
                               where e.CourseID == courseID
                               && e.ApplicantTypeID == ApplicantTypeId
                               && e.ActivityID == LateFeeActivityId
                               && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(applicationDate)
                               orderby e.EfferctiveDate ascending
                               select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
            if (LateFeeExam.Count() > 0)//If  applicable for late fee ?
            {

                if (LateFeeExam != null)
                    examID = LateFeeExam.FirstOrDefault().ExamID;
            }
            else if (LateFeeExam.Count() <= 0)//If  not applicable for late fee ?
            {

                var NormalFeeExam = (from e in context.CutOffDates
                                     join i in context.Exams on e.ExamID equals i.ID
                                     where e.CourseID == courseID
                                     && e.ApplicantTypeID == ApplicantTypeId
                                     && e.ActivityID == NormalactivityId
                                     && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(applicationDate)
                                     orderby e.EfferctiveDate ascending
                                     select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
                if (NormalFeeExam.Count() > 0)
                    examID = NormalFeeExam.FirstOrDefault().ExamID;
            }
            return examID;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void LnkBtnBack_Click(object sender, EventArgs e)
    {
        int batchItemID = Convert.ToInt32(Request.QueryString["batchItemID"]);
        Response.Redirect("~/HO/RegistrationProcess.aspx?batchItemID=" + batchItemID);
    }
    protected void ddlCandidateType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCandidateType.SelectedValue == "0")
            {
                ddlflExamYear.Items.Clear();
                ddlflExamYear.Items.Insert(0, "--Select One--");
                ddlBatchNumber.Items.Clear();
                ddlBatchNumber.Items.Insert(0, "--Select One--");
                ddlCourse.SelectedValue = "0";
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlflExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int CourseId = Convert.ToInt32(ddlCourse.SelectedValue);
            int CandidateTypeId = Convert.ToInt32(ddlCandidateType.SelectedValue);
            Int32 ExamYear = Convert.ToInt32(ddlflExamYear.SelectedValue);
            bindBatchNumber(CourseId, CandidateTypeId, ExamYear);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnProcess_Click(object sender, EventArgs e)
    {
        Int16 verifiedCount = 0;
        EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
        try
        {
           
            using (EConnectContext context = new EConnectContext())
            {

                string sql = "";
                Int64 batchItemID = 0;

                string batchItemIDs = "";
                String styleClass = "";
                int counter = 0;

                List<StudentList> stList = new List<StudentList>();
                for (int i = 0; i < gvMain.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)gvMain.Rows[i].FindControl("chk");
                    if (cbx.Checked == true)
                    {
                        batchItemID = Convert.ToInt64(gvMain.DataKeys[i].Values[0]);
                        stList.Add(RegistrationProcessing(batchItemID));
                        verifiedCount += 1;
                        batchItemIDs += batchItemID + ",";
                    }
                }

                // sending sms and email  
                var evnt = context.NotificationEvent.Find(Convert.ToInt32(enmNotificationEvents.AfterSuccessfullRegistration));
                if (evnt.SendEmail == true)
                {
                    Thread threademail = new Thread(() => SentBulkEmail(stList, Convert.ToInt32(Session["UserID"])));
                    threademail.Start();
                }
                if (evnt.SendSms == true)
                {
                    Thread threadsms = new Thread(() => SentBulkSMS(stList, Convert.ToInt32(Session["UserID"])));
                    threadsms.Start();
                }

                //deep add message display for 28 may 2018
                if (verifiedNotCount > 0)
                {
                    ShowAlert(verifiedCount.ToString() + " applications Processed successfully. while It has been observed by system that same batch was already running by another user/systems", true);
                }
                else
                {
                    ShowAlert(verifiedCount.ToString() + " applications Processed successfully.", true);
                }
                //deep add message display for 28 may 2018

               // ShowAlert(verifiedCount.ToString() + " applications Processed successfully.", true);
                BindGridView();
                BatchItem batchitem = context.BatchItems.Find(batchItemID);
                Int32 ApplicationStatus = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                Int32 courseID = batchitem.Batch.CourseID;
                if (batchitem != null)
                {
                    //EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
                    con.Open();

                    batchItemIDs = batchItemIDs.TrimEnd(',');

                    sql = " select cr.Number as ApplNumber ,c.Name + '('+ e.Name + ')' as examName , r.Registration_No as RegistrationNum, " +
                          " REPLACE(CONVERT(VARCHAR,r.Registration_Date,106),' ','-') as Registration_Date , " +
                          " case r.Reg_Type_ID when '4' then REPLACE(CONVERT(VARCHAR,r.Re_Registration_Date,106),' ','-')  " +
                          "  else REPLACE(CONVERT(VARCHAR,r.Registration_Date,106),' ','-')  end as RegistrationDate ," +
                          " REPLACE(CONVERT(VARCHAR,r.Valid_Upto_Date ,106),' ','-') as validUptoDt " +
                          " from Course_Registration_Application cr,Registration_Detail r,Course c,Exam e where cr.ID= r.Courser_Reg_Appl_ID " +
                          " and cr.Course_ID =c.ID and cr.Application_Status_ID = " + ApplicationStatus + " and cr.Course_ID = " + courseID +
                          " and cr.Exam_ID =e.ID  " +
                          " and cr.Batch_Item_ID in (" + batchItemIDs + ")  order by cr.ID  ";
                    DataTable dt = EConnect.Utils.Data.DbUtility.GetDataTable(sql, con, null, CommandType.Text, true);
                    if (dt.Rows.Count > 0)
                    {
                        tblShowDetail1.Visible = true;
                        StringBuilder tablestring = new StringBuilder();
                        tablestring.Append("<table width='100%' class='sample3' cellspacing='0' cellpadding='3' border='1' id='tblShowDetail'><tr class='head1'><th>#</th><th>Application No.</th><th>Exam Name</th><th>Registration No.</th><th>Registration Date</th><th>Valid Upto</th></tr>");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            counter++;
                            if (counter % 2 == 0)
                                styleClass = "gdalternate1";
                            else
                                styleClass = "gdrow1";
                            tablestring.Append("<tr class=" + styleClass + " ><td>" + (i + 1).ToString() + "</td><td>" + dt.Rows[i]["ApplNumber"].ToString() +
                                 "</td><td>" + dt.Rows[i]["examName"].ToString() + "</td><td>" + dt.Rows[i]["RegistrationNum"].ToString() + "</td><td>"
                                 + dt.Rows[i]["RegistrationDate"].ToString() + "</td><td>" + dt.Rows[i]["validUptoDt"].ToString() + "</td></tr>");
                        }
                        tablestring.Append("</table>");
                        divShowdetail.InnerHtml = tablestring.ToString();
                        UpnlShow.Update();
                        divGrid.Visible = false;
                        PagingBar1.Visible = false;
                        btnProcess.Visible = false;
                    }
                    else
                    {
                        tblShowDetail1.Visible = false;
                        PagingBar1.Visible = true;
                    }
                    con.Close();
                    dt.Dispose();
                }
            };



        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
            con.Close(); 
           
        }
    }
    private StudentList RegistrationProcessing(Int64 batchItemID)
    {
        StudentList st = new StudentList();
        try
        {
            Int64 mobileNumber = 0; string mobileMsg = ""; string msg = ""; string emailAddress = "";
            //Added for SMS template
            string templateId="";
            using (EConnectContext context = new EConnectContext())
            {
                var batchitem = context.BatchItems.Find(batchItemID);
                CourseRegistrationApplication cr = new CourseRegistrationApplication();

                if (batchItemID != 0)
                {
                    cr = context.CourseRegistrationApplications.Find(batchitem.CourseRegistrationApplicationID.Value);

                    int CandidateTypeId = cr.ApplicantTypeID;
                    int BatchId = batchitem.BatchID;
                    int CourseId = cr.CourseID;
                    Int64 regno = 0;
                    Int64 candidateID = 0;
                    mobileNumber = cr.MobileNumber;
                    st.Email = cr.EmailAddress;
                    st.MobileNo = cr.MobileNumber;
                    emailAddress = cr.EmailAddress;
                    btnMode.Visible = false;
                    enmCurrentRegistrationStatus regStatus = GetCurrentRegistrationStatus(Convert.ToInt32(batchItemID), context);
                    switch (regStatus)
                    {

                        case enmCurrentRegistrationStatus.EligibleForNewRegistrationAsPreviousRegistrationNotFound: //1
                            //deep add code on 27 may 2018 for check value process or under processing specific candidates
                            //deep testing pupose of 29 may 2018
                                var checkstatus = (from a in context.CourseRegistrationApplications
                                                   where a.BatchItemID == batchItemID
                                                   select a).FirstOrDefault();
                                if (checkstatus.Registration_Process_Flag.ToString() == "N" )
                                {
                                checkstatus.Registration_Process_Flag = "Y";
                                checkstatus.Registration_Process_Flag_Y_DT = Convert.ToDateTime(DateTime.Now);
                                context.Entry(checkstatus).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                                //deep testing pupose of 29 may 2018
                                if (cr.ApplicationStatusID != Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                                {
                                    candidateID = SaveCandidateRecord(cr.ID, context);
                                    regno = SaveNewRegistrationRecord(cr.ID, batchItemID, candidateID, context);
                                    divTbl.Visible = true;
                                    ProcessDetail(CourseId, BatchId, CandidateTypeId, cr.ID, regno, context);
                                    lblMsg.Visible = true;
                                    lblMsg.Text = "Congratulation!You are successfully registered..";

                                    //deep add on 28 may2018
                                    cr.Registration_Process_Flag = "C";
                                    cr.Registration_Process_Flag_C_DT = Convert.ToDateTime(DateTime.Now);
                                    //deep end add on 28 may2018

                                    cr.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                                    context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                    DivRegdetail.Visible = false;
                                }
                            }
                            else
                            {
                                verifiedNotCount += 1;
                            }
                            break;
                        case enmCurrentRegistrationStatus.RegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndValidityPeriodNotLapsed: //2
                            if (cr.ApplicationStatusID != Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                            {
                                UpdateRegistrationCancelRecord(cr.ID, BatchId, context);
                                regno = SaveNewRegistrationRecord(cr.ID, batchItemID, (Int64)cr.CandidateID, context);
                                divTbl.Visible = true;
                                ProcessDetail(CourseId, BatchId, CandidateTypeId, cr.ID, regno, context); //Show Processed Detail

                                cr.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                                context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                                DivRegdetail.Visible = false;
                                lblMsg.Visible = true;
                                lblMsg.Text = "You are registered as a fresh registration as Previous Registration has been cancelled...";
                            }
                            break;
                        case enmCurrentRegistrationStatus.CompletedAsAllModulesOfCurrentLevelPassedCompletelyWithinRegistrationPeriod: //3
                            if (cr.ApplicationStatusID != Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                            {
                                regno = SaveNewRegistrationRecord(cr.ID, batchItemID, (Int64)cr.CandidateID, context);
                                divTbl.Visible = true;
                                ProcessDetail(CourseId, BatchId, CandidateTypeId, cr.ID, regno, context);
                                lblMsg.Visible = true;
                                lblMsg.Text = "Congratulation!You are successfully registered..";
                                cr.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                                context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                                DivRegdetail.Visible = false;
                            }
                            break;
                        case enmCurrentRegistrationStatus.ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired: //4
                            if (cr.ApplicationStatusID != Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                            {
                                regno = UpdateRe_RegistrationRecord(cr.ID, BatchId, context);
                                divTbl.Visible = true;
                                ProcessDetail(CourseId, BatchId, CandidateTypeId, cr.ID, regno, context);
                                lblMsg.Visible = true;
                                lblMsg.Text = "You are successfully Re-Registered..!";
                                cr.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                                context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                                DivRegdetail.Visible = false;
                            }
                            break;
                        case enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod: //5
                            if (cr.ApplicationStatusID != Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                            {
                                UpdateRegistrationCancelRecord(cr.ID, BatchId, context);
                                regno = SaveNewRegistrationRecord(cr.ID, batchItemID, (Int64)cr.CandidateID, context);
                                divTbl.Visible = true;
                                ProcessDetail(CourseId, BatchId, CandidateTypeId, cr.ID, regno, context);
                                lblMsg.Visible = true;
                                lblMsg.Text = "Congratulation!You are successfully registered as a fresh registration and your previous registration has been cancelled..";
                                cr.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                                context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                                DivRegdetail.Visible = false;
                            }
                            break;
                        case enmCurrentRegistrationStatus.CompletedAllModulesPassedInPreviousExamAndEligibleForAutoUpgradation: //6
                            if (cr.ApplicationStatusID != Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                            {
                                regno = SaveNewRegistrationRecord(cr.ID, batchItemID, (Int64)cr.CandidateID, context);
                                divTbl.Visible = true;
                                ProcessDetail(CourseId, BatchId, CandidateTypeId, cr.ID, regno, context);
                                lblMsg.Visible = true;
                                lblMsg.Text = "Congratulation!You are successfully registered..";
                                cr.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                                context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                                DivRegdetail.Visible = false;
                            }
                            break;
                        case enmCurrentRegistrationStatus.ReRegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndReRegistrationValidityPeriodNotLapsed: //8
                            if (cr.ApplicationStatusID != Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                            {
                                UpdateRegistrationCancelRecord(cr.ID, BatchId, context);
                                regno = SaveNewRegistrationRecord(cr.ID, batchItemID, (Int64)cr.CandidateID, context);
                                divTbl.Visible = true;
                                ProcessDetail(CourseId, BatchId, CandidateTypeId, cr.ID, regno, context);
                                lblMsg.Visible = true;
                                lblMsg.Text = "Congratulation!You are successfully registered as a fresh registration and your previous registration has been cancelled..";
                                cr.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                                context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();

                                DivRegdetail.Visible = false;
                            }
                            break;
                    }
                    if (regno > 0)
                   {
                        var registration = context.RegistrationDetails.Where(s => s.RegistrationNo == regno && s.CourseRegistrationApplicationID == cr.ID).OrderByDescending(s => s.RegistrationDate).FirstOrDefault();
                        mobileMsg = "You are successfully registered with NIELIT on ";
                         
                            
                        if (registration.RegistrationStatusID == Convert.ToInt32(enmRegistrationStatus.ReRegistered))
                        {
                            mobileMsg += registration.ReRegistrationDate.Value.ToString("dd-MMM-yyyy hh:mm") + "  with Registration Number " + registration.RegistrationNo.ToString() + " . Your current course level " +
                                            "is " + cr.Course.Name + ".";
                        }
                        else
                        {
                            mobileMsg += registration.RegistrationDate.ToString("dd-MMM-yyyy hh:mm") + "  with Registration Number " + registration.RegistrationNo.ToString() + " . Your current course level " +
                                           "is " + cr.Course.Name + ".";
                        }

                        if (cr.AlreadyRegistered == false)
                        {
                            mobileMsg += "You can now register through the New User option given at Home Page by providing Registration Number and " +
                                         "other details and can get Login ID and Password from the NIELIT-NIELIT";
                            templateId = "1307160931597520711";
                        }

                        else
                        {
                            mobileMsg += "You can use your existing Login ID and Password for further details.-NIELIT";
                            templateId = "1307160931602371982";
                        }

                        st.MobileMsg = mobileMsg;
                        //Added for SMS template
                        st.templateId = templateId;

                        msg = "Dear " + GetInitCap(cr.Salutation + " " + cr.Name) + ",<br/><br/>" + "You are successfully registered with NIELIT on "
                              + registration.RegistrationDate.ToString("dd-MMM-yyyy hh:mm") + " with Registration Number " + registration.RegistrationNo.ToString() +
                              ". Your current course level is " + cr.Course.Name + ".";
                        if (cr.AlreadyRegistered == false)
                            msg += "You can now register through the <u>New User</u> option given at Home Page by providing Registration Number and " +
                                         " other details and can get Login ID and Password from the NIELIT";
                        else
                            msg += "You can use your existing Login ID and Password for further details.";
                        st.EmailMsg = msg;
                    }

                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return st;
    }
    protected Int32 ShowModuleSummary(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber, Int64 candID)
    {
        try
        {
            Int32 moduleTypeTheory = Convert.ToInt32(enmModuleType.Theory);
            Int32 moduleTypePractical = Convert.ToInt32(enmModuleType.Practical);
            Int32 moduleTypeProject = Convert.ToInt32(enmModuleType.Project);
            using (EConnectContext context = new EConnectContext())
            {
                var listOfPassedModules = (from d in context.CourseExamApplicationDetails
                                           join m in context.Modules on d.ModuleID equals m.ID
                                           where d.CourseID == currentCourseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candID &&
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
                string tdTotalTheoryModules = (theoryCompModules + theoryElectiveModules + bridgeModules).ToString() + " (" + theoryCompModules.ToString() + " + " + theoryElectiveModules.ToString() + " + " + bridgeModules.ToString() + ")";
                string tdTotalPracticalModules = CourseManager.GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Practical, null).ToString();
                string tdTotalProjectModules = CourseManager.GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Project, null).ToString();

                Int32 moduleTypeBridge = Convert.ToInt32(enmModuleType.Bridge);
                int attempted = CourseManager.GetCountOfAttempteddModulesOfAnyRevision(context, currentCourseID, registrationNumber, candID, enmModuleType.Theory);
                string tdAttemptedTheoryModules = (attempted + CourseManager.GetCountOfAttempteddModulesOfAnyRevision(context, currentCourseID, registrationNumber, candID, enmModuleType.Bridge)).ToString();

                string tdAttemptedPracticalModules = CourseManager.GetCountOfAttempteddModulesOfAnyRevision(context, currentCourseID, registrationNumber, candID, enmModuleType.Practical).ToString();
                string tdAttemptedProjectModules = CourseManager.GetCountOfAttempteddModulesOfAnyRevision(context, currentCourseID, registrationNumber, candID, enmModuleType.Project).ToString();



                string tdPassedTheoryModules = listOfPassedModules.Where(d => (d.ModuleTypeID == moduleTypeTheory || d.ModuleTypeID == moduleTypeBridge)).Count().ToString();
                string tdPassedPracticalModules = listOfPassedModules.Where(d => d.ModuleTypeID == moduleTypePractical).Count().ToString();
                string tdPassedProjectModules = listOfPassedModules.Where(d => d.ModuleTypeID == moduleTypeProject).Count().ToString();


                string tdRemainingTheorygModules = ((theoryCompModules + theoryElectiveModules + bridgeModules) - Convert.ToInt32(tdPassedTheoryModules)).ToString();
                string tdRemainingPracticalModules = (Convert.ToInt32(tdTotalPracticalModules) - Convert.ToInt32(tdPassedPracticalModules)).ToString();
                string tdRemainingProjectModules = (Convert.ToInt32(tdTotalProjectModules) - Convert.ToInt32(tdPassedProjectModules)).ToString();

                ICollection<Module> lstRemainingModules = CourseManager.GetRemainingModules(context, currentCourseID, registrationNumber, currentevisionNumber, candID);
                if (tdRemainingTheorygModules == "0")
                    lstRemainingModules = lstRemainingModules.Where(d => d.ModuleTypeID != (Int32)enmModuleType.Theory).ToList();

                return lstRemainingModules.Count;

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void SentBulkSMS(List<StudentList> candidateList, Int32 UserID)
    {

        for (int i = 0; i < candidateList.Count(); i++)
        {
            try
            {
                //sending SMS 
                if (candidateList[i].MobileNo != 0 && !String.IsNullOrEmpty(candidateList[i].MobileMsg))
                {
                    EConnect.NIELIT.SMS sms = new SMS(candidateList[i].MobileMsg, candidateList[i].MobileNo.ToString(), candidateList[i].templateId ,SmsServiceType.BulkSMS, false);
                    int sentMessageCount;
                    sms.sendSingleSMS(out sentMessageCount);
                }
            }
            catch (Exception) { }

        }
    }
    protected void SentBulkEmail(List<StudentList> candidateList, Int32 UserID)
    {

        for (int i = 0; i < candidateList.Count(); i++)
        {
            try
            {
                //sending SMS 
                if (candidateList[i].Email.Trim().Length > 0 && !String.IsNullOrEmpty(candidateList[i].EmailMsg))
                {
                    EConnect.NIELIT.Email mail = new Email("Online Registration Form:NIELIT", candidateList[i].EmailMsg, candidateList[i].Email, false);
                    mail.SendInThread(UserID);
                }
            }
            catch (Exception) { }
        }

    }
}
public class StudentList
{

    public Int64 MobileNo
    {
        get;
        set;
    }
    public String Email
    {
        get;
        set;
    }
    public String MobileMsg
    {
        get;
        set;
    }
    public String EmailMsg
    {
        get;
        set;
    }
    //Added for SMS templates
    public string templateId
    {
        get;
        set;
    }
}