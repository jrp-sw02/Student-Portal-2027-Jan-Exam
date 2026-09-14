using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data.Objects;


public class STCregistration
{
    
    public STCregistration()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static Int64 STCPaymentProcess(Int64 applID)
    {
        using (var context = new EConnectContext())
        {
            Int32 feeAmount = 0;
            var application = context.CourseRegistrationApplications.Find(applID);
            if (application.enmApplicantType == enmApplicantType.Institute && feeAmount == 0)
            {
                DemandNote demand = new DemandNote();
                demand.ApplicationDate = DateTime.Now;
                demand.FeeTypeID = application.FeeTypeID.Value;    //RegistrationFee;
                demand.ApplicationTypeID = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.DemandDraft);
                demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Multiple);
                demand.Amount = application.FeeAmount.Value;    // STC feeamount = 0
                demand.CreatedBy = 1;
                demand.CourseCategoryID = application.CourseCategoryID;
                demand.CourseID = application.CourseID;
                demand.ServiceID = application.Course.RegistrationServiceID;

                context.DemandNotes.Add(demand);
                context.SaveChanges();

                return demand.ID;
            }
            return 0;

        };
    }
    public static void STCregistrationNumberAllocation2(List<Int64> applist, Int32 courseId, Int32 courseCatId, Int32 examId)
    {

        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //var appl = context.CourseRegistrationApplications.Find(applId);
                var courseCatg = context.CourseCategories.Find(courseCatId);
                var course = context.Courses.Find(courseId);
                var exam = context.Exams.Find(examId);

                #region Generate Batch Number---------
                StringBuilder batchNumber = new StringBuilder();
                //-----for HO only---Course_Registration_Application only------Institute type only---
                batchNumber.Append("HO/" + courseCatg.Code + "/" + course.Code + "/LR/I/");
                #endregion

                #region Save Batch--------------------
                Batch batch = new Batch();
                batch.Number = batchNumber.ToString();
                batch.ApplicationTypeID = 1;                      // CourseRegistrationApplication
                batch.CourseCategoryID = courseCatId;            // STC = 6
                batch.CourseID = courseId;
                batch.StatusID = 1;                     // Created
                batch.CreatedOn = DateTime.Now;
                batch.CreatedByID = 193;                      // Dummy for HO
                batch.ApplicantTypeID = 2;                      //  Institute
                batch.ExamID = examId;
                context.Batchs.Add(batch);
                context.SaveChanges();

                batch.Number += batch.ID.ToString();
                context.Entry(batch).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                int batchId = batch.ID;
                string FinalbatchNumber = batch.Number;
                #endregion

                #region Form Scanning-----------------
                var applicant = (from p in context.CourseRegistrationApplications
                                where p.ApplicableExamID == examId
                                && p.ApplicationStatusID == 8
                                && p.PaymentStatusID == 2
                                && applist.Contains(p.ID)
                                && p.BatchItemID == null
                                select new { Appno = p.Number }).ToList();
                foreach (var application in applicant)
                {
                    var appl = context.CourseRegistrationApplications.Where(p => p.Number == application.Appno).FirstOrDefault();

                    BatchItem batchItem = new BatchItem();
                    batchItem.BatchID = batchId;
                    batchItem.CourseRegistrationApplicationID = appl.ID;
                    batchItem.StatusID = 16;               // ApplicationVerifiedbyNielit&ForwardedtoRegistrationWing
                    batchItem.CreatedByID = 193;                // Dummy for HO
                    batchItem.CreatedOn = DateTime.Now;
                    batchItem.Remarks = "Scanned on" + DateTime.Now;
                    batchItem.IsRejected = false;
                    batchItem.IsKeptInAbeyance = false;
                    context.BatchItems.Add(batchItem);
                    context.SaveChanges();

                    appl.BatchItemID = batchItem.ID;
                    appl.ApplicationStatusID = 16;
                    context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
                batch.StatusID = 3;            // batch completed
                context.Entry(batch).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                #endregion

                #region Registration Number Generate-------
                int verifiedCount = 0;
                var registration = from s in context.CourseRegistrationApplications
                                   join b in context.BatchItems on s.ID equals b.CourseRegistrationApplicationID
                                   where s.ApplicationStatusID == 16 &&
                                   s.CourseID == courseId && s.ApplicantTypeID == 2
                                   && b.BatchID == batchId && s.ApplicableExamID == examId
                                   && applist.Contains(s.ID)
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
                                       batchItemID = b.ID,
                                       BatchId = b.Batch.ID,
                                       ApplTypeID = 1
                                   };

                List<STCStudentList> stList = new List<STCStudentList>();
                foreach (var cand in registration)
                {
                    stList.Add(RegistrationProcessing(cand.batchItemID));
                    verifiedCount += 1;
                }

                // sending sms and email  
                var evnt = context.NotificationEvent.Find(Convert.ToInt32(enmNotificationEvents.AfterSuccessfullRegistration));
                if (evnt.SendEmail == true)
                {
                    Thread threademail = new Thread(() => SentBulkEmail(stList));
                    threademail.Start();
                }
                if (evnt.SendSms == true)
                {
                    Thread threadsms = new Thread(() => SentBulkSMS(stList));
                    threadsms.Start();
                }
                #endregion

            }
        }
        catch (Exception ex)
        { throw ex; }
    }

    public static void STCregistrationNumberAllocation(Int64 demandNoteId, Int32 courseId, Int32 courseCatId, Int32 examId)
    {

        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var demandnote = context.DemandNotes.Find(demandNoteId);
                var courseCatg = context.CourseCategories.Find(courseCatId);
                var course = context.Courses.Find(courseId);
                var exam = context.Exams.Find(examId);

                #region Generate Batch Number---------
                StringBuilder batchNumber = new StringBuilder();
                //-----for HO only---Course_Registration_Application only------Institute type only---
                batchNumber.Append("HO/" + courseCatg.Code + "/" + course.Code + "/LR/I/");
                #endregion

                #region Save Batch--------------------
                Batch batch = new Batch();
                batch.Number = batchNumber.ToString();
                batch.ApplicationTypeID  = 1;                      // CourseRegistrationApplication
                batch.CourseCategoryID = courseCatId;            // STC = 6
                batch.CourseID = courseId;
                batch.StatusID = 1;                      // Created
                batch.CreatedOn = DateTime.Now;
                batch.CreatedByID = 193;                      // Dummy for HO
                batch.ApplicantTypeID = 2;                      //  Institute
                batch.ExamID = examId;
                context.Batchs.Add(batch);
                context.SaveChanges();
                batch.Number += batch.ID.ToString();
                context.Entry(batch).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                int batchId = batch.ID;
                string FinalbatchNumber = batch.Number;
                #endregion

                #region Form Scanning-----------------
                var applicant = (from p in context.CourseRegistrationApplications
                                where p.ApplicableExamID == examId
                                && p.ApplicationStatusID == 8
                                || p.ApplicationStatusID == 2
                                && p.PaymentStatusID == 2
                                && p.DemandNoteID == demandNoteId
                                && p.BatchItemID == null
                                select new { Appno = p.Number }).ToList();
                foreach (var application in applicant)
                {
                    var appl = context.CourseRegistrationApplications.Where(p => p.Number == application.Appno).FirstOrDefault();
                    BatchItem batchItem = new BatchItem();
                    batchItem.BatchID = batchId;
                    batchItem.CourseRegistrationApplicationID = appl.ID;
                    batchItem.StatusID = 16;               // ApplicationVerifiedbyNielit&ForwardedtoRegistrationWing
                    batchItem.CreatedByID = 193;                // Dummy for vinodHO
                    batchItem.CreatedOn = DateTime.Now;
                    batchItem.Remarks = "Scanned on" + DateTime.Now;
                    context.BatchItems.Add(batchItem);
                    context.SaveChanges();

                    appl.BatchItemID = batchItem.ID;
                    appl.ApplicationStatusID = 16;
                    context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
                batch.StatusID = 3;            // batch completed
                context.Entry(batch).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                #endregion

                #region Registration Number Generate-------
                int verifiedCount = 0;
                var registration = from s in context.CourseRegistrationApplications
                                   join b in context.BatchItems on s.ID equals b.CourseRegistrationApplicationID
                                   where s.ApplicationStatusID == 16 &&
                                   s.CourseID == courseId && s.ApplicantTypeID == 2
                                   && b.BatchID == batchId && s.ApplicableExamID == examId
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
                                       batchItemID = b.ID,
                                       BatchId = b.Batch.ID,
                                       ApplTypeID = 1
                                   };

                List<STCStudentList> stList = new List<STCStudentList>();
                foreach (var cand in registration)
                {
                    stList.Add(RegistrationProcessing(cand.batchItemID));
                    verifiedCount += 1;
                }

                // sending sms and email  
                var evnt = context.NotificationEvent.Find(Convert.ToInt32(enmNotificationEvents.AfterSuccessfullRegistration));
                if (evnt.SendEmail == true)
                {
                    Thread threademail = new Thread(() => SentBulkEmail(stList));
                    threademail.Start();
                }
                if (evnt.SendSms == true)
                {
                    Thread threadsms = new Thread(() => SentBulkSMS(stList));
                    threadsms.Start();
                }
                #endregion

            }
        }
        catch (Exception ex)
        { throw ex; }
    }   
    private static STCStudentList RegistrationProcessing(Int64 batchItemID)
    {
        STCStudentList st = new STCStudentList();
        try
        {
            Int64 mobileNumber = 0; string mobileMsg = ""; string msg = ""; string emailAddress = "";
            //Added for SMS
            string templateId = "";
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

                    enmCurrentRegistrationStatus regStatus = GetCurrentRegistrationStatus(Convert.ToInt32(batchItemID), context);
                    switch (regStatus)
                    {

                        case enmCurrentRegistrationStatus.EligibleForNewRegistrationAsPreviousRegistrationNotFound: //1
                            if (cr.ApplicationStatusID != Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                            {
                                candidateID = SaveCandidateRecord(cr.ID, context);
                                regno = SaveNewRegistrationRecord(cr.ID, batchItemID, candidateID, context);

                                ProcessDetail(CourseId, BatchId, CandidateTypeId, cr.ID, regno, context);
                                cr.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                                context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                            }
                            break;
                        case enmCurrentRegistrationStatus.RegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndValidityPeriodNotLapsed: //2
                            if (cr.ApplicationStatusID != Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                            {
                                //using (TransactionScope scope = new TransactionScope())
                                //{
                                UpdateRegistrationCancelRecord(cr.ID, BatchId, context);
                                regno = SaveNewRegistrationRecord(cr.ID, batchItemID, (Int64)cr.CandidateID, context);
                                //divTbl.Visible = true;
                                ProcessDetail(CourseId, BatchId, CandidateTypeId, cr.ID, regno, context); //Show Processed Detail

                                cr.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                                context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                                //DivRegdetail.Visible = false;
                                //lblMsg.Visible = true;
                                //lblMsg.Text = "You are registered as a fresh registration as Previous Registration has been cancelled...";
                                //scope.Complete();
                                //};
                            }
                            break;
                        case enmCurrentRegistrationStatus.CompletedAsAllModulesOfCurrentLevelPassedCompletelyWithinRegistrationPeriod: //3
                            if (cr.ApplicationStatusID != Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                            {
                                //using (TransactionScope scope = new TransactionScope())
                                //{
                                regno = SaveNewRegistrationRecord(cr.ID, batchItemID, (Int64)cr.CandidateID, context);
                                //divTbl.Visible = true;
                                ProcessDetail(CourseId, BatchId, CandidateTypeId, cr.ID, regno, context);
                                //lblMsg.Visible = true;
                                //lblMsg.Text = "Congratulation!You are successfully registered..";
                                cr.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                                context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                                //DivRegdetail.Visible = false;
                                //    scope.Complete();
                                //};
                            }
                            break;
                        case enmCurrentRegistrationStatus.ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired: //4
                            if (cr.ApplicationStatusID != Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                            {
                                //using (TransactionScope scope = new TransactionScope())
                                //{
                                regno = UpdateRe_RegistrationRecord(cr.ID, BatchId, context);
                                //divTbl.Visible = true;
                                ProcessDetail(CourseId, BatchId, CandidateTypeId, cr.ID, regno, context);
                                //lblMsg.Visible = true;
                                //lblMsg.Text = "You are successfully Re-Registered..!";
                                cr.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                                context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                                //DivRegdetail.Visible = false;
                                //    scope.Complete();
                                //};
                            }
                            break;
                        case enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod: //5
                            if (cr.ApplicationStatusID != Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                            {
                                //using (TransactionScope scope = new TransactionScope())
                                //{
                                UpdateRegistrationCancelRecord(cr.ID, BatchId, context);
                                regno = SaveNewRegistrationRecord(cr.ID, batchItemID, (Int64)cr.CandidateID, context);
                                //divTbl.Visible = true;
                                ProcessDetail(CourseId, BatchId, CandidateTypeId, cr.ID, regno, context);
                                //lblMsg.Visible = true;
                                //lblMsg.Text = "Congratulation!You are successfully registered as a fresh registration and your previous registration has been cancelled..";
                                cr.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                                context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                                //DivRegdetail.Visible = false;
                                //    scope.Complete();
                                //};
                            }
                            break;
                        case enmCurrentRegistrationStatus.CompletedAllModulesPassedInPreviousExamAndEligibleForAutoUpgradation: //6
                            if (cr.ApplicationStatusID != Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                            {
                                //using (TransactionScope scope = new TransactionScope())
                                //{
                                regno = SaveNewRegistrationRecord(cr.ID, batchItemID, (Int64)cr.CandidateID, context);
                                //divTbl.Visible = true;
                                ProcessDetail(CourseId, BatchId, CandidateTypeId, cr.ID, regno, context);
                                //lblMsg.Visible = true;
                                //lblMsg.Text = "Congratulation!You are successfully registered..";
                                cr.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                                context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                                //DivRegdetail.Visible = false;
                                //    scope.Complete();
                                //};
                            }
                            break;
                        case enmCurrentRegistrationStatus.ReRegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndReRegistrationValidityPeriodNotLapsed: //8
                            if (cr.ApplicationStatusID != Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                            {
                                //using (TransactionScope scope = new TransactionScope())
                                //{
                                UpdateRegistrationCancelRecord(cr.ID, BatchId, context);

                                regno = SaveNewRegistrationRecord(cr.ID, batchItemID, (Int64)cr.CandidateID, context);
                                //divTbl.Visible = true;
                                ProcessDetail(CourseId, BatchId, CandidateTypeId, cr.ID, regno, context);
                                //lblMsg.Visible = true;
                                //lblMsg.Text = "Congratulation!You are successfully registered as a fresh registration and your previous registration has been cancelled..";
                                cr.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                                context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();

                                //DivRegdetail.Visible = false;
                                //    scope.Complete();
                                //};
                            }
                            break;

                    }
                    if (regno > 0)
                    {
                        var registration = context.RegistrationDetails.Where(s => s.RegistrationNo == regno && s.CourseRegistrationApplicationID == cr.ID).OrderByDescending(s => s.RegistrationDate).FirstOrDefault();
                        mobileMsg = "You are successfully registered with NIELIT on ";
                        if (registration.RegistrationStatusID == Convert.ToInt32(enmRegistrationStatus.ReRegistered))
                        {
                            mobileMsg += registration.ReRegistrationDate.Value.ToString("dd-MMM-yyyy hh:mm") + " with Registration Number " + registration.RegistrationNo.ToString() + ". Your current course level " +
                                            " is " + cr.Course.Name + ".";
                        }
                        else
                        {
                            mobileMsg += registration.RegistrationDate.ToString("dd-MMM-yyyy hh:mm") + " with Registration Number " + registration.RegistrationNo.ToString() + ". Your current course level " +
                                           " is " + cr.Course.Name + ".";
                        }

                        if (cr.AlreadyRegistered == false)
                        {
                            mobileMsg += "You can now register through the New User option given at Home Page by providing Registration Number and " +
                                         " other details and can get Login ID and Password from the NIELIT";
                            //Added templateid for SMS
                            templateId = "test1";
                        }
                        else
                        {
                            mobileMsg += "You can use your existing Login ID and Password for further details.";
                         //Added templateid for SMS
                            templateId = "test2";
                        }

                        st.MobileMsg = mobileMsg;
                        //Added for templateid
                        st.templateid = templateId;
                        

                        msg = "Dear " + cr.Salutation + " " + cr.Name + ",<br/><br/>" + "You are successfully registered with NIELIT on "
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
    private static Int64 UpdateRe_RegistrationRecord(Int64 courseRegAppID, int BatchId, EConnectContext context)
    {
        try
        {

            Int64 regno = 0;
            //using (EConnectContext context = new EConnectContext())
            //{
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

                //DateTime ReRegistrationDate = objRegistration.ValidUptoDate.AddMonths(reRegistrationMonths).AddDays(-1);
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
                //NewValidUptoDate = NewValidUptoDate.AddMonths(1).AddDays(-1);

                //objRegistration.ValidUptoDate = ReRegistrationDate.AddMonths(-1).AddDays(1).AddYears(policy.RegistrationValidity / papers).AddMonths(-(totalMonths - 1)).AddDays(-1);

                //objRegistration.ValidUptoDate = NewValidUptoDate.AddMonths(-1).AddDays(1).AddYears(policy.RegistrationValidity / papers).AddMonths(-(totalMonths - 1)).AddDays(-1);
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
            //};
            return regno;

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private static void UpdateRegistrationCancelRecord(Int64 courseRegAppID, int BatchId, EConnectContext context)
    {
        try
        {
            var application = (from a in context.CourseRegistrationApplications
                               where a.ID == courseRegAppID
                               select a).FirstOrDefault();

            RegistrationDetail objRegistration = context.RegistrationDetails.Where(s => s.CandidateID == application.CandidateID).OrderByDescending(s => s.RegistrationDate).FirstOrDefault();


            objRegistration.CancelledOn = DateTime.Now;
            objRegistration.CancelledBy = 193;
            enmCurrentRegistrationStatus regStatus = GetCurrentRegistrationStatus(Convert.ToInt32(application.BatchItemID.Value), context);
            if (regStatus != enmCurrentRegistrationStatus.None)
                objRegistration.CurrentRegistrationStatusID = Convert.ToInt32(regStatus);
            objRegistration.RegistrationStatusID = Convert.ToInt32(enmRegistrationStatus.Cancelled);
            objRegistration.RegistrationStatusCode = context.RegistrationStatus.Find(Convert.ToInt32(enmRegistrationStatus.Cancelled)).Code;
            //objRegistration.CourseRegistrationApplicationID = courseRegAppID;
            context.Entry(objRegistration).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            //Result grading cancelled for previous registration
            var cexamApplDetail = (from cm in context.CourseExamApplicationDetails
                                   where cm.CourseID == objRegistration.CourseID && cm.RegistrationNumber == objRegistration.RegistrationNo
                                   && cm.CandidateID == objRegistration.CandidateID
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
                    objExam.UpdatedByID = 193;
                    objExam.UpdatedOn = Convert.ToDateTime(DateTime.Now);
                    objExam.ResultGradeID = context.ResultGrades.Where(s => s.CourseCategoryID == 1
                                                              && s.Code == "Z").FirstOrDefault().ID;
                    //string remarks = EConnect.Utils.Common.EnumUtility.GetDescription((enmRegistrationType)application.RegistrationTypeID.Value);
                    //objExam.CancelRemarks = remarks.ToString();
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
    protected static enmCurrentRegistrationStatus GetCurrentRegistrationStatus(Int32 batchItemID, EConnectContext context) //checking Registration status
    {
        try
        {
            //using (EConnectContext context = new EConnectContext())
            //{

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
                    //lblRegNo.Text = cr.RegisteredCourseRegistrationNo.ToString();
                    //lblRegDate.Text = currentRegistration.RegistrationDate.ToString("dd-MMM-yyyy");
                    //lblRegCourse.Text = cr.RegisteredCourse.Name;
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
                        //lblMessage.Text = "Dear Candidate,<br><br>";
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
                                    //lblMessage.Text = "Dear Candidate,<br><br>";
                                }
                            }
                            else
                            {
                                //Registration validity period is over and no chances for reregistration
                                reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                                //lblMessage.Text = "Dear Candidate,<br><br>";
                            }
                        }
                        else
                        {
                            reasons = enmCurrentRegistrationStatus.RegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndValidityPeriodNotLapsed;
                            // lblMessage.Text = "Dear Candidate,<br><br>";
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

                                if (cr.ApplicationDate >= currentRegistration.ValidUptoDate.AddMonths(reRegistrationPolicy.ReRegistrationGapInMonths.Value))
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


                }
                else if (cr.AlreadyRegistered == false)
                {

                    reasons = enmCurrentRegistrationStatus.EligibleForNewRegistrationAsPreviousRegistrationNotFound;
                }

            }
            return reasons;
            //};
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    private static Int64 SaveCandidateRecord(Int64 courseRegAppID, EConnectContext context)
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
                objContact.StdNumber = application.StdNumber.Value;
            if (application.PhoneNumber.HasValue)
                objContact.PhoneNumber = application.PhoneNumber.Value;
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
                objAddress.DistrictID = application.PerDistrictID.Value;
            objAddress.PinCode = application.PerPinCode;
            objAddress.AddressTypeID = Convert.ToInt32(enmAddressType.PermanentAddress);
            objAddress.CandidateID = objCandidate.ID;
            objAddress.CountryID = 1; ///need to change in future
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
                objAddress2.DistrictID = application.CorDistrictID.Value;
            objAddress2.PinCode = application.CorPinCode;
            objAddress2.AddressTypeID = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
            objAddress2.CandidateID = objCandidate.ID;
            objAddress2.CountryID = 1;
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
    private static Int64 SaveNewRegistrationRecord(Int64 courseRegAppID, Int64 batchItemID, Int64 candidateID, EConnectContext context)
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

                //var reg = (from s in context.RegistrationDetails
                //           orderby s.RegistrationNo descending
                //           select s).FirstOrDefault();



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
                    //objRegistration.RegistrationNo = reg.RegistrationNo + 1;
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
                        //objRegistration.RegistrationNo = reg.RegistrationNo + 1;
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
    private static void ProcessDetail(int CourseId, int BatchId, int CandidateTypeId, Int64 CourseRegistrationApplicationID, Int64 regno, EConnectContext context)
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
                //lblAppno.Text = app.Number;
                //lblAppDate.Text = app.ApplicationDate.ToString("dd-MMM-yyyy");
                //lblExamName.Text = exams != null ? exams.Name : "";
                var reg = context.RegistrationDetails.Where(r => r.RegistrationNo == regno && r.CourseID == CourseId && r.CourseRegistrationApplicationID == CourseRegistrationApplicationID).OrderByDescending(r => r.CommencementFromDate).FirstOrDefault();
                //lblCurrntRegNo.Text = regno.ToString();
                //lblCurrntRegDate.Text = reg.RegistrationDate.ToString("dd-MMM-yyyy");
                //lblCurrntRegCourse.Text = context.Courses.Find(CourseId).Name;
                //lblCommncmntFrmDate.Text = CommencementFromDate.ToString("dd-MMM-yyyy");
                //lblValidUpToDate.Text = CommencementToDate.ToString("dd-MMM-yyyy");
                //lblCommncmntFrmDate.Text = reg.CommencementFromDate.ToString("dd-MMM-yyyy");
                //lblValidUpToDate.Text = reg.ValidUptoDate.ToString("dd-MMM-yyyy");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected static Int32 ShowModuleSummary(Int32 currentCourseID, Int64 registrationNumber, Int32 currentevisionNumber, Int64 candID)
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
                //tdPassedElectiveModules.InnerText = listOfPassedModules.Where(d => (d.ModuleTypeID == moduleTypeTheory && d.SelectionTypeID == 2)).Select(m=>m.ElectiveGroup).Distinct().Count().ToString();
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
    protected static Int32 GetExamID(Int32 ApplicantTypeId, Int32 courseID, DateTime applicationDate, EConnectContext context)
    {
        try
        {
            Int32 NormalactivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
            Int32 LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee);
            //using (EConnectContext context = new EConnectContext())
            //{
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

            //};
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private static CourseRegistrationApplication GetRegistrationData(int CourseId, int BatchId, int CandidateTypeId, Int64 CourseRegistrationApplicationID, EConnectContext context)
    {
        try
        {
            //using (EConnectContext context = new EConnectContext())
            //{
            int BatchComplete = Convert.ToInt32(enmBatchStatus.Completed);
            int ApplicationVerifiedByNIELIT = Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing);
            //var app = (from s in context.CourseRegistrationApplications
            //           join b in context.BatchItems on s.ID equals b.CourseRegistrationApplicationID
            //           //join r in context.RegistrationDetails on s.CandidateID equals r.CandidateID
            //           where s.ApplicationStatusID == ApplicationVerifiedByNIELIT && b.Batch.StatusID == BatchComplete
            //           && s.CourseID == CourseId && s.ApplicantTypeID == CandidateTypeId
            //           && b.BatchID == BatchId && s.ID == CourseRegistrationApplicationID
            //           select s).FirstOrDefault();
            var app = (from s in context.CourseRegistrationApplications
                       join b in context.BatchItems on s.ID equals b.CourseRegistrationApplicationID
                       where s.ID == CourseRegistrationApplicationID
                       select s).FirstOrDefault();

            if (app != null)
            {
                return app;
            }
            else
                return null;
            //};
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }
    private static string GetCommencementDetail(int CourseId, int BatchId, int CandidateTypeId, Int64 CourseRegistrationApplicationID, EConnectContext context)
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
                    //CommencementFromDate = CommencementFromDate.AddMonths(1).AddDays(-1);
                }
                else
                {
                    Int64 year = (exams.ExamYear);
                    CommencementFromDate = DateTime.Parse(exams.ExamYear.ToString() + "-" + exams.ExamMonth.ToString() + "-01");
                    //CommencementFromDate = CommencementFromDate.AddMonths(1).AddDays(-1);
                }

                DateTime CommencementToDate = new DateTime();
                //CommencementToDate = CommencementFromDate.AddMonths(-1).AddDays(1).AddYears(currentPolicy.RegistrationValidity / papers).AddMonths(-(totalMonths - 1)).AddDays(-1);
                CommencementToDate = CommencementFromDate.AddMonths(currentPolicy.RegistrationValidity).AddDays(-1);
                //return CommencementFromDate.AddDays(-30) + "," + CommencementToDate;
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
    protected static void SentBulkSMS(List<STCStudentList> candidateList)
    {

        for (int i = 0; i < candidateList.Count(); i++)
        {
            try
            {
                //sending SMS 
                if (candidateList[i].MobileNo != null && candidateList[i].MobileNo != 0 && !String.IsNullOrEmpty(candidateList[i].MobileMsg))
                {
                    //Modified templateid for SMS
                    EConnect.NIELIT.SMS sms = new SMS(candidateList[i].MobileMsg, candidateList[i].MobileNo.ToString(),candidateList [i].templateid , SmsServiceType.BulkSMS, false);
                    int sentMessageCount;
                    sms.sendSingleSMS(out sentMessageCount);
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
    protected static void SentBulkEmail(List<STCStudentList> candidateList)
    {

        for (int i = 0; i < candidateList.Count(); i++)
        {
            try
            {
                //sending SMS 
                if (candidateList[i].Email.Trim().Length > 0 && !String.IsNullOrEmpty(candidateList[i].EmailMsg))
                {
                    EConnect.NIELIT.Email mail = new Email("Online Registration Form:NIELIT", candidateList[i].EmailMsg, candidateList[i].Email, false);
                    mail.SendInThread(193);
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}

public class STCStudentList
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
    //Added templateid for SMS
    public string templateid
    {
        get;
        set;
    }
}