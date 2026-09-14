using System;
using System.Data.Objects;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data.SqlClient;
using System.Web;

public partial class CertificatePreview : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        NormalHeader1.Visible = false;
        try
        {
            if (Request.QueryString["Appid"] == null)
            {
                //Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
		string urldecoded=HttpUtility.HtmlDecode (GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
		Response.Write(urldecoded);
                Response.End();
                return;
            }
            if (Request.QueryString["Src"] != null)
            {
                if (Request.QueryString["Src"] == "CSC")
                {
                    NormalHeader1.Visible = true;
                }
            }
            if (!Page.IsPostBack)
            {

                //Show all the filled Information as a Preview           
                ShowData();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }


    #region Click Events
    //Finally Submit The Form
    protected void Btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            int courseid = 0;
            int ApplicationFlag = 0;
            Int32 paymentstatusid = 0;
            Int64 applID = Convert.ToInt64(Request.QueryString["Appid"]);
            LblAppNumber.Text = applID.ToString();
            Int64 demandID = 0; string msg = "";
            string emailAddress = "";
            Int64 mobileNumber; string mobileMsg = "";
             Boolean isExempted = false;
            Boolean allowSendingEmail = false;
            Boolean allowSendingSms = false;
            Int32[] activityID = { 1, 2, 3 };

            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    var application = context.CertificateExamApplications.Find(applID);
                    Int32 activitycutoffdateID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                    if (application.LateFeeAmount.HasValue)
                    {
                        if (application.LateFeeAmount.Value > 0)
                            activitycutoffdateID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                    }
                    var cuttoffdate = context.CutOffDates.Where(s => s.ExamID == application.ExamID && s.CourseID == application.CourseID && s.ApplicantTypeID == application.ApplicantTypeID && s.ActivityID == activitycutoffdateID).FirstOrDefault().EfferctiveDate;
                    if (DateTime.Now.Date > cuttoffdate.Date)
                    {
                        ShowAlert("Last date for Filling Examination Application Form is over.");
                        ShowData();
                        return;
                    }



                    // added by amit start by amit 25-02-26


                    if (application.apaarID == "")
                    {
                        ShowAlert("Please go back and check apaar id");
                        return;
                    }

                    Int32 CourseId = Convert.ToInt32(application.CourseID);
                    Int32 ExamId = Convert.ToInt32(application.ExamID);

                    string duplicate = checkDuplicateApaar(application.apaarID, CourseId, ExamId, applID);

                    if (duplicate != "-99")
                    {
                        if (duplicate != "0")
                        {
                            ShowAlert("Duplicate application for the exam cycle not allowed, Check status for " + duplicate.ToString() + " Application");
                            ShowData();
                            return;
                        }
                    }
                    else
                    {
                        ShowAlert("Error Occurred. Try Again");
                        ShowData();
                        return;
                    }


                    // added by amit end by amit 25-02-26



                    if (application.AlreadyApplied == true)
                    {
                        var previousApplObj = context.CertificateExamApplications.Find(application.PreviousApplicationID);
                        int gradeID = 0;
                        if (previousApplObj.ExamID != 0)
                            gradeID = context.ResultGrades.Where(r => r.Code == "*" && r.VersionID == previousApplObj.Exam.ResultGradeVersionID.Value).FirstOrDefault().ID;
                        else
                            gradeID = context.ResultGrades.Where(r => r.Code == "*" && r.VersionID == 1).FirstOrDefault().ID; // By Default
                        if (previousApplObj.ResultGradeID.HasValue && previousApplObj.ResultUpdatedOn.HasValue)
                        {
                            DateTime resultdeclareDate = previousApplObj.ResultUpdatedOn.Value.AddDays(7);
                            if (previousApplObj.ExemptedApplicationID == null && previousApplObj.ResultGradeID == gradeID)
                            {
                                previousApplObj.ExemptedApplicationID = application.ID;

                                // now candidate is exempted for all exams other than previous exams. 
                                Int32 nextExamID = GetNextExamID(application.ApplicantTypeID, application.CourseID, application.Exam.ExaminationCycleID);
                                if (nextExamID == application.ExamID)
                                {
                                    isExempted = true;
                                    application.IsExempted = true;
                                }
                                else
                                {
                                    isExempted = false;
                                    application.IsExempted = false;
                                }
                            }
                        }
                    }
                    emailAddress = application.EmailAddress;
                    mobileNumber = application.MobileNumber;
                    application.FeeTypeID = Convert.ToInt32(enmFeeType.RegistrationCumExaminationFee);
                    application.FinalSubmitted = true;
                    application.FinalSubmissionDate = DateTime.Now;
                    application.ApplicationDate = DateTime.Now;
                    courseid = application.CourseID;
                    isExempted = application.IsExempted;
                    Int32 feeAmount = 0; Int32 lateFeeAmount = 0;
                    CourseManager.GetCertificateExamFee(application.CourseID, application.ExamID, application.ApplicantTypeID, out feeAmount, out lateFeeAmount);
                    if (isExempted == true)
                        feeAmount = 0;
                    application.FeeAmount = feeAmount;
                    application.LateFeeAmount = lateFeeAmount;
                    application.TotalFeeAmount = lateFeeAmount + feeAmount;
                    if (!application.DemandNoteID.HasValue)
                    {
                        DemandNote demand = new DemandNote();
                        if (application.enmApplicantType == enmApplicantType.Direct)
                        {
                            demand.ApplicationDate = DateTime.Now;
                            demand.FeeTypeID = application.FeeTypeID.Value;
                            demand.ApplicationTypeID = Convert.ToInt32(enmApplicationType.CertificateExamApplication);

                            if ((feeAmount + lateFeeAmount) == 0 && application.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct) && isExempted == true)
                            {
                                demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.DemandDraft);
                                demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                            }
                            else
                            {
                                demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.CSCSPV);
                                demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                            }


                            demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Single);
                            demand.Amount = application.TotalFeeAmount.Value;
                            demand.CreatedBy = 1;
                            demand.CourseCategoryID = application.CourseCategoryID;
                            demand.CourseID = application.CourseID;
                            demand.ServiceID = application.Course.ExaminationServiceID;
                            context.DemandNotes.Add(demand);
                            context.SaveChanges();



                            application.DemandNoteID = demand.ID;
                            if ((feeAmount + lateFeeAmount) == 0 && application.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct) && isExempted == true)
                            {

                                application.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                application.ApplicationStatusID = Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre);
                            }
                            else
                            {
                                application.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                                application.ApplicationStatusID = Convert.ToInt32(enmCertificateExamApplicationStatus.AppliedButFeeNotDepositedByCandidate);
                            }

                            demandID = demand.ID;
                            ApplicationFlag = 1;
                        }
                        else if (application.enmApplicantType == enmApplicantType.Institute)
                        {
                            if ((feeAmount + lateFeeAmount) == 0 && application.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && isExempted == true)
                            {
                                demand.ApplicationDate = DateTime.Now;
                                demand.FeeTypeID = application.FeeTypeID.Value;
                                demand.ApplicationTypeID = Convert.ToInt32(enmApplicationType.CertificateExamApplication);
                                demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.DemandDraft);
                                demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Multiple);
                                demand.Amount = application.TotalFeeAmount.Value;
                                demand.CreatedBy = 1;
                                demand.CourseCategoryID = application.CourseCategoryID;
                                demand.CourseID = application.CourseID;
                                demand.ServiceID = application.Course.ExaminationServiceID;
                                context.DemandNotes.Add(demand);
                                context.SaveChanges();
                                application.DemandNoteID = demand.ID;
                                application.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                            }
                            application.ApplicationStatusID = Convert.ToInt32(enmCertificateExamApplicationStatus.AppliedButPendingForInstituteVerification);
                            ApplicationFlag = 0;
                        }
                    }

                    context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    var evnt = context.NotificationEvent.Find(Convert.ToInt32(enmNotificationEvents.AfterSubmittingTheCertificateExamApplicationForm));
                    if (evnt.SendEmail == true)
                        allowSendingEmail = true;
                    if (evnt.SendSms == true)
                        allowSendingSms = true;
                    mobileMsg = "You have applied for " + application.Course.Code + " Exam " + application.Exam.Name + " on " + application.ApplicationDate.ToString("dd-MMM-yyyy hh:mm") + ". Your application no." +
                        " is " + application.Number;


                    msg = "Dear " + GetInitCap(application.Salutation + " " + application.Name) + ",<br/><br/>" + "You have successfully applied for " + GetInitCap(application.Course.Name) +
                        " examination through Online Student Information and Enrollment System of NIELIT. <br/>" +
                        " Your online application details are as following: <br/><br/>Examination Cycle Name  &nbsp;&nbsp;&nbsp;&nbsp;: " + application.Exam.ExaminationCycle.Name + "<br/>" +
                        "Name of Examination &nbsp;:  " + application.Exam.Name + "<br/>" +
                        "Application Number &nbsp;:  " + application.Number + "<br/>" +
                       "Application Date &nbsp;:  " + application.ApplicationDate.ToString("dd-MMM-yyyy") + "";

                    if (application.DemandNoteID.HasValue)
                        msg += "<br/>DemandNote Number &nbsp;:  " + application.DemandNoteID.Value + "";
                    else
                        msg += "<br/>DemandNote Number &nbsp;:  Not generated";
                    msg += "<br/><br/><br/>Please note your application number.It will be used for further correspondence with NIELIT.";


                    if (application.enmApplicantType == enmApplicantType.Direct)
                    {
                        msg += "<br><br><I>Note: You have not paid the applicable examination fee at the time of form sumbmission. " +
                            " Please pay you applicable examination fee by the last date mentioned in the form using any of the available payment options.<br><br>" +
                            " Important: Please refer your Demand Note Number mentioned in the form at the time of payment at CSC/SPV. ";
                    }
                    else if (application.enmApplicantType == enmApplicantType.Institute)
                    {
                        msg += "<br><br><I>Note: You have not paid the applicable examination fee at the time of form sumbmission. " +
                        " Please pay you applicable examination fee to the institute (" + application.Institute.Name + ") by the last date mentioned in the form.<br><br>";
                        // " Important: Please refer your Demand Note Number mentioned in the form at the time of payment at CSC/SPV. Your Demand Note Number is: " + appl.DemandNote.ID.ToString();

                    }


                    //"You can know status of your application by clicking on following link : <br/><br/> <a href=" + HttpContext.Current.Request.UrlReferrer.ToString().Substring(0, HttpContext.Current.Request.UrlReferrer.ToString().LastIndexOf('/')).Replace("CAND", "") + "index.aspx>Know your application status.</a>";
                    paymentstatusid = application.PaymentStatusID;
                    scope.Complete();
                };
            };

            if (allowSendingEmail == true)
            {
                try
                {
                    //sending Email 
                    if (emailAddress.Trim().Length > 0)
                    {
                        EConnect.NIELIT.Email mail = new Email("Online Examination Form:NIELIT", msg, emailAddress);
                        mail.Send(); // long time taking
                    }
                }
                catch (Exception ex) { ShowAlert(ex.Message); }
            }
            if (allowSendingSms == true)
            {
                try
                {
                    //Sending Mobile Message
                    if (mobileNumber != 0)
                    {
                        EConnect.NIELIT.SMS message = new SMS(mobileMsg, mobileNumber.ToString(), "1307159090363039343", SmsServiceType.SignleSMS);
                        int sentMessageCount;
                        message.sendSingleSMS(out sentMessageCount);
                    }
                }
                catch (Exception ex) { ShowAlert(ex.Message); }
            }

            if ((paymentstatusid != Convert.ToInt32(enmPaymentStatus.Pending)) && (paymentstatusid != 0))
            {
                ShowData();
                NormalHeader1.Visible = false;
                return;
            }

            if ((!String.IsNullOrEmpty(Request.QueryString["Appid"])) && (Request.QueryString["Src"] == "CSC"))
                Response.Redirect("../nieletpaymentservices.aspx?ServiceID=4&DID=" + demandID.ToString());
            else if (ApplicationFlag == 1)
                Response.Redirect("FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CertificateExamApplication).ToString() + "&Appid=" + applID.ToString() + "&DemandID=" + demandID.ToString(), false);
            else if (ApplicationFlag == 0)
                Response.Redirect("FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CertificateExamApplication).ToString() + "&Appid=" + applID.ToString() + "&DemandID=0", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }
    //Back to Registration Form if the filled details are to be updated.
    protected void BtnBack_Click(object sender, EventArgs e)
    {
        try
        {
            int applID = Convert.ToInt32(Request.QueryString["Appid"]);
            int courseid;

            using (EConnectContext context = new EConnectContext())
            {
                CertificateExamApplication currentCourse = context.CertificateExamApplications.Find(Convert.ToInt64(Request.QueryString["Appid"]));
                //-------Insert backButton history in Certificate_ExamA_pplication_Backbutton_history----

                //String backbuttonsql = "INSERT INTO Certificate_Exam_Application_BackButton_History(Certificate_Exam_Application_ID,Date,Number,Candidate_ID,Already_Applied,Previous_Exam_ID ,Previous_Roll_Number,Previous_Application_ID,Course_Category_ID,Course_ID,Applicant_Type_ID,Exam_ID,Institute_ID ,Exam_Center1_ID,Exam_Center2_ID,Regional_Center_ID,Salutaion,Name,Father_Name,Mother_Name,Gender,Dob,Cast_Category_ID,Occupation_ID,Educational_Qualification_ID,Passing_Year,Mobile,Std,Phone,Email,Cor_Address1,Cor_Address2,Cor_Address3,Cor_Country_ID,Cor_State_ID,Cor_District_ID,Cor_City_Name,Cor_Pin_Code,Final_Submitted,Final_Submission_Date,Demand_Note_ID,Course_Duration_From,Course_Duration_To,Fee_Type_ID,Fee_Amt,Late_Fee_Amt,Total_Fee_Amt,Is_Verified_By_Institute,Verified_On_By_Institute,Payment_Status_ID,Application_Status_ID,Batch_Item_ID,Roll_Number,Exam_Centre_Name,Exam_Centre_Address,Date_of_Exam,Exam_Batch_Number,Reporting_Time,Updated_On,Updated_By,Guardian_Name,Result_Grade_ID,Result_Updated_On,Result_Updated_By,Exempted_Application_ID,Is_Exempted,App_Source,Is_Synced,Synced_On,Exam_Month,Exam_Year,Examn_Cycle_ID,Previous_Exam_Name,Created_On,Created_By,CAND_TYPE)" +
                //                     "(select ID,Date,Number,Candidate_ID,Already_Applied,Previous_Exam_ID ,Previous_Roll_Number,Previous_Application_ID,Course_Category_ID,Course_ID,Applicant_Type_ID,Exam_ID,Institute_ID ,Exam_Center1_ID,Exam_Center2_ID,Regional_Center_ID,Salutaion,Name,Father_Name,Mother_Name,Gender,Dob,Cast_Category_ID,Occupation_ID,Educational_Qualification_ID,Passing_Year,Mobile,Std,Phone,Email,Cor_Address1,Cor_Address2,Cor_Address3,Cor_Country_ID,Cor_State_ID,Cor_District_ID,Cor_City_Name,Cor_Pin_Code,Final_Submitted,Final_Submission_Date,Demand_Note_ID,Course_Duration_From,Course_Duration_To,Fee_Type_ID,Fee_Amt,Late_Fee_Amt,Total_Fee_Amt,Is_Verified_By_Institute,Verified_On_By_Institute,Payment_Status_ID,Application_Status_ID,Batch_Item_ID,Roll_Number,Exam_Centre_Name,Exam_Centre_Address,Date_of_Exam,Exam_Batch_Number,Reporting_Time,Updated_On,Updated_By,Guardian_Name,Result_Grade_ID,Result_Updated_On,Result_Updated_By,Exempted_Application_ID,Is_Exempted,App_Source,Is_Synced,Synced_On,Exam_Month,Exam_Year,Examn_Cycle_ID,Previous_Exam_Name,'" + DateTime.Now + "'," + currentCourse.ID + ", '" + Request.QueryString["candtype"] + "' from Certificate_Exam_Application Where ID=" + currentCourse.ID + ")";
                String backbuttonsql = "INSERT INTO Certificate_Exam_Application_BackButton_History(Certificate_Exam_Application_ID,Date,Number,Candidate_ID,Already_Applied,Previous_Exam_ID ,Previous_Roll_Number,Previous_Application_ID,Course_Category_ID,Course_ID,Applicant_Type_ID,Exam_ID,Institute_ID ,Exam_Center1_ID,Exam_Center2_ID,Regional_Center_ID,Salutaion,Name,Father_Name,Mother_Name,Gender,Dob,Cast_Category_ID,Occupation_ID,Educational_Qualification_ID,Passing_Year,Mobile,Std,Phone,Email,Cor_Address1,Cor_Address2,Cor_Address3,Cor_Country_ID,Cor_State_ID,Cor_District_ID,Cor_City_Name,Cor_Pin_Code,Final_Submitted,Final_Submission_Date,Demand_Note_ID,Course_Duration_From,Course_Duration_To,Fee_Type_ID,Fee_Amt,Late_Fee_Amt,Total_Fee_Amt,Is_Verified_By_Institute,Verified_On_By_Institute,Payment_Status_ID,Application_Status_ID,Batch_Item_ID,Roll_Number,Exam_Centre_Name,Exam_Centre_Address,Date_of_Exam,Exam_Batch_Number,Reporting_Time,Updated_On,Updated_By,Guardian_Name,Result_Grade_ID,Result_Updated_On,Result_Updated_By,Exempted_Application_ID,Is_Exempted,App_Source,Is_Synced,Synced_On,Exam_Month,Exam_Year,Examn_Cycle_ID,Previous_Exam_Name,Created_On,Created_By,CAND_TYPE)" +
                                      "(select ID,Date,Number,Candidate_ID,Already_Applied,Previous_Exam_ID ,Previous_Roll_Number,Previous_Application_ID,Course_Category_ID,Course_ID,Applicant_Type_ID,Exam_ID,Institute_ID ,Exam_Center1_ID,Exam_Center2_ID,Regional_Center_ID,Salutaion,Name,Father_Name,Mother_Name,Gender,Dob,Cast_Category_ID,Occupation_ID,Educational_Qualification_ID,Passing_Year,Mobile,Std,Phone,Email,Cor_Address1,Cor_Address2,Cor_Address3,Cor_Country_ID,Cor_State_ID,Cor_District_ID,Cor_City_Name,Cor_Pin_Code,Final_Submitted,Final_Submission_Date,Demand_Note_ID,Course_Duration_From,Course_Duration_To,Fee_Type_ID,Fee_Amt,Late_Fee_Amt,Total_Fee_Amt,Is_Verified_By_Institute,Verified_On_By_Institute,Payment_Status_ID,Application_Status_ID,Batch_Item_ID,Roll_Number,Exam_Centre_Name,Exam_Centre_Address,Date_of_Exam,Exam_Batch_Number,Reporting_Time,Updated_On,Updated_By,Guardian_Name,Result_Grade_ID,Result_Updated_On,Result_Updated_By,Exempted_Application_ID,Is_Exempted,App_Source,Is_Synced,Synced_On,Exam_Month,Exam_Year,Examn_Cycle_ID,Previous_Exam_Name,@CreatedOn,@CreatedBy, @candType from Certificate_Exam_Application Where ID=@ID)";
                string candType;
                if (Request.QueryString["candtype"] != null)
                    candType = Request.QueryString["candtype"].ToString();
                else
                    candType = "";

                                SqlParameter[] para1 ={

                    new SqlParameter("@CreatedOn", DateTime.Now),
                                        new SqlParameter("@CreatedBy", currentCourse.ID),
                                          new SqlParameter("@candType", candType),
                                        new SqlParameter("@ID", currentCourse.ID)
                                                        };

                context.Database.ExecuteSqlCommand(backbuttonsql, para1);
                context.SaveChanges();
                //------------------------End Insert---------------------------------------------------------
                if (currentCourse.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending))
                {
                    if (CommonFunctions.IsDemandNoteCancellable(currentCourse.DemandNoteID.HasValue ? currentCourse.DemandNoteID.Value : 0))
                    {
                        currentCourse.FinalSubmitted = false;
                        currentCourse.FinalSubmissionDate = null;
                        currentCourse.DemandNoteID = null;
                        currentCourse.IsVerifiedByInstitute = false;
                        currentCourse.DateOfVerificationByInstitute = null;
                        currentCourse.CourseDurationFrom = null;
                        currentCourse.CourseDurationTo = null;
                        context.Entry(currentCourse).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                else
                {
                    ShowData();
                    NormalHeader1.Visible = false;
                }
                // inserting into history table if candidate change its exam after final submitted for previous exam not current exam
                Int32 currentExamID = GetCurrentExamID(currentCourse.ApplicantTypeID, currentCourse.CourseID);
                if (currentCourse.ExamID != currentExamID)
                {
                    String sql = "INSERT INTO Certificate_Exam_Application_History(Certificate_Exam_Application_ID,Date,Number,Candidate_ID,Already_Applied,Previous_Exam_ID ,Previous_Roll_Number,Previous_Application_ID,Course_Category_ID,Course_ID,Applicant_Type_ID,Exam_ID,Institute_ID ,Exam_Center1_ID,Exam_Center2_ID,Regional_Center_ID,Salutaion,Name,Father_Name,Mother_Name,Gender,Dob,Cast_Category_ID,Occupation_ID,Photo_File_Name,Photo,Signature_File_Name,Signature,Left_Thumb_File_Name,Left_Thumb,Educational_Qualification_ID,Passing_Year,Mobile,Std,Phone,Email,Cor_Address1,Cor_Address2,Cor_Address3,Cor_Country_ID,Cor_State_ID,Cor_District_ID,Cor_City_Name,Cor_Pin_Code,Final_Submitted,Final_Submission_Date,Demand_Note_ID,Course_Duration_From,Course_Duration_To,Fee_Type_ID,Fee_Amt,Late_Fee_Amt,Total_Fee_Amt,Is_Verified_By_Institute,Verified_On_By_Institute,Payment_Status_ID,Application_Status_ID,Batch_Item_ID,Roll_Number,Exam_Centre_Name,Exam_Centre_Address,Date_of_Exam,Exam_Batch_Number,Reporting_Time,Updated_On,Updated_By,Guardian_Name,Result_Grade_ID,Result_Updated_On,Result_Updated_By,Exempted_Application_ID,Is_Exempted,App_Source,Is_Synced,Synced_On,Exam_Month,Exam_Year,Examn_Cycle_ID,Previous_Exam_Name,Created_On,Created_By)" +
                                 "(select ID,Date,Number,Candidate_ID,Already_Applied,Previous_Exam_ID ,Previous_Roll_Number,Previous_Application_ID,Course_Category_ID,Course_ID,Applicant_Type_ID,Exam_ID,Institute_ID ,Exam_Center1_ID,Exam_Center2_ID,Regional_Center_ID,Salutaion,Name,Father_Name,Mother_Name,Gender,Dob,Cast_Category_ID,Occupation_ID,Photo_File_Name,Photo,Signature_File_Name,Signature,Left_Thumb_File_Name,Left_Thumb,Educational_Qualification_ID,Passing_Year,Mobile,Std,Phone,Email,Cor_Address1,Cor_Address2,Cor_Address3,Cor_Country_ID,Cor_State_ID,Cor_District_ID,Cor_City_Name,Cor_Pin_Code,Final_Submitted,Final_Submission_Date,Demand_Note_ID,Course_Duration_From,Course_Duration_To,Fee_Type_ID,Fee_Amt,Late_Fee_Amt,Total_Fee_Amt,Is_Verified_By_Institute,Verified_On_By_Institute,Payment_Status_ID,Application_Status_ID,Batch_Item_ID,Roll_Number,Exam_Centre_Name,Exam_Centre_Address,Date_of_Exam,Exam_Batch_Number,Reporting_Time,Updated_On,Updated_By,Guardian_Name,Result_Grade_ID,Result_Updated_On,Result_Updated_By,Exempted_Application_ID,Is_Exempted,App_Source,Is_Synced,Synced_On,Exam_Month,Exam_Year,Examn_Cycle_ID,Previous_Exam_Name,@CreatedOn,@CreatedBy from Certificate_Exam_Application Where ID=@ID)";
                    SqlParameter[] para2 ={

                                        new SqlParameter("@CreatedOn",DateTime.Now),
                                        new SqlParameter("@CreatedBy",currentCourse.ID),
                                        new SqlParameter("@ID",currentCourse.ID)
                                        };
                    context.Database.ExecuteSqlCommand(sql, para2);
                    context.SaveChanges();
                }
                courseid = currentCourse.CourseID;
            };
            if ((!String.IsNullOrEmpty(Request.QueryString["Appid"])) && (Request.QueryString["Src"] == "CSC"))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("CertificateRegistration.aspx?id=" + courseid + "&Appid=" + applID + "&candtype=" + Request.QueryString["candtype"] + "&Src=CSC&RU=" + Request.QueryString["RU"].ToString()));
            }
            else
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("CertificateRegistration.aspx?" + Request.QueryString), false);
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }
    #endregion

    #region Private Methods


    protected void checkApaar(string apaar, string dob, string name, string gender)
    {//Validation of apaar
     //     string token = validateApaar.getToken();
     //     if (token == "")
     //     {

        //         throw new Exception("Apaar ID not validated, Try again");
        //     }
        //     string validateData = validateApaar.getApaarData(apaar, token);
        //     if (validateData != null && validateData != "error")
        //     {
        //         apaarResponse apaarReturn = JsonConvert.DeserializeObject<apaarResponse>(validateData);
        //         if (apaarReturn == null || apaarReturn.status == "Token is Invalid")
        //         {

        //             throw new Exception("Apaar ID not validated, Try again");
        //         }
        //         //return ("status" + biometricReturn.status);
        //         bool statusId = false;

        //         string errorCode = "";
        //         if (apaarReturn.status == "1")
        //         {
        //             statusId = true;
        //             errorCode = apaarReturn.statuscode;
        //         }
        //         if (apaarReturn.status == "0")
        //         {
        //             statusId = false;
        //             errorCode = apaarReturn.status_code;
        //         }

        //         int result = apaarResponse.saveResponse(apaar, apaarReturn.status, errorCode, apaarReturn.message, apaarReturn.cname, validateData);
        //         if (apaarReturn.status == "0")
        //         {

        //             throw new Exception("Invalid Apaar, if not generated, please generate or correct and enter");
        //         }
        //       //  DateTime dob = Convert.ToDateTime(LblDob.Text);
        //        // string dateofbirth = dob.ToString("dd/MM/yyyy");
        //string dateofbirth = Convert.ToDateTime( dob).ToString("dd/MM/yyyy");

        //         string dateofbirth1 = apaarReturn.dob;
        //         if (dateofbirth1.Length < 10)
        //         {
        //             string[] s = dateofbirth1.Split('/');
        //             if (s[0].Length < 2)
        //                 s[0] = "0" + s[0];

        //             if (s[1].Length < 2)
        //                 s[1] = "0" + s[1];



        //             dateofbirth1 = s[0] + "/" + s[1] + "/" + s[2];
        //         }
        //         if (dateofbirth1 != dateofbirth)
        //         {

        //             throw new Exception("Invalid Apaar or Date of birth Mismatch, if apaar not generated, please generate or correct and enter");
        //         }
        //         string vname = Regex.Replace(apaarReturn.cname, @"\s+", " ");

        //         if (vname.ToLower() != name.ToLower())
        //         //   if (apaarReturn.cname.Trim().ToLower() != txtAppName.Text.Trim().ToLower())
        //         {

        //             throw new Exception("Invalid Apaar or Name Mismatch, if apaar not generated, please generate or correct and enter");
        //         }
        //         if (apaarReturn.gender.ToLower() != gender.Trim().ToLower())
        //         {

        //             throw new Exception("Invalid Apaar or Personal Data Mismatch, if apaar not generated, please generate or correct and enter");
        //         }
        //}
        //else
        //{

        //    throw new Exception("Apaar could not be validated, Try again");
        //}
    }
    string checkDuplicateApaar(string apaarEncryptedCheck, int CourseId, int ExamId, Int64 applid)
    {
        string duplicate = "0";
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var candRecord = (from c in context.CertificateExamApplications
                                  where c.CourseID == CourseId && c.ExamID == ExamId
                                        && c.apaarID == apaarEncryptedCheck
                                        && c.ID != applid
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

    protected void ShowData()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int64 applID = Convert.ToInt64(Request.QueryString["Appid"]);
                var application = context.CertificateExamApplications.Find(applID);

                if (application.AadharNumber.HasValue && application.AadharNumber.ToString().Trim().Length == 12)
                {
                    string s = application.AadharNumber.Value.ToString();

                    lbladhar.Text = s.Substring(0, 3) + "XXXXX" + s.Substring(8, 4);
                }
                else
                    lbladhar.Text = "N/A";

                if (application.apaarID != null)
                {
                    string apaarDecrypt = EncryptDecrypt.DecryptString(application.apaarID);
                    lblApaar.Text = apaarDecrypt;
                }
                else
                    lblApaar.Text = "N/A";

                if (application.FinalSubmitted == true)
                {
                    Btnsubmit.Visible = false;
                    BtnBack.Visible = false;
                    //code to enable final submit button for previous exam
                    if ((application.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending)) && (application.ApplicationStatusID != Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre)) && (application.ApplicationStatusID != Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre)))
                    {
                        if (CommonFunctions.IsDemandNoteCancellable(application.DemandNoteID.HasValue ? application.DemandNoteID.Value : 0))
                        {
                            Int32 currentExamID = GetCurrentExamID(application.ApplicantTypeID, application.CourseID);
                            if (application.ExamID != currentExamID)
                            {
                                Btnsubmit.Visible = true;
                                BtnBack.Visible = true;
                            }
                            else
                            {
                                BtnBack.Visible = true;
                            }
                        }
                    }
                    BtnPrint.Visible = true;
                    NormalHeader1.Visible = true;
                    if (context.Exams.Where(s => s.ID == application.ExamID).FirstOrDefault().IsBatchProcessable)
                    {

                        var regional = context.RegionalCenters.Where(r => r.ID == application.RegionalCenterID).FirstOrDefault();
                        LblRegionalAddressE.Text = "NIELIT Centre, " + regional.Name + " , " + regional.Address + " <br/> Contact Number :" + regional.ContactNumbers + ",  Email Id : " + regional.EmailAddresses + "" +
                                           "   website : " + regional.Website + "<br/>";

                        LblRegionalAddressH.Text = "NIELIT Centre, " + regional.Name + " , " + regional.Address + " <br/> फोन नंबर :" + regional.ContactNumbers + ",   ईमेल आईडी : " + regional.EmailAddresses + "" +
                                           "  वेबसाइट : " + regional.Website + "<br/>";
                    }
                    else
                    {
                        TrPaymentMsg.Style.Add("display", "none");
                        TrPayment_not.Style.Add("display", "none");
                    }
                    if (application.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                    {

                        TrDemandnote.Visible = true;
                        TrDemandnote1.Visible = true;
                        TrInstitute.Visible = false;
                        if (application.DemandNoteID.HasValue == true)
                        {
                            LblDemandNoteID.Text = application.DemandNote.ID.ToString();
                            LblDemandNoteDate.Text = application.DemandNote.ApplicationDate.ToString("dd-MMM-yyyy");
                        }
                    }
                    else
                    {
                        TrDD.Visible = false;
                        TrDemandnote.Visible = false;
                        TrDemandnote1.Visible = false;
                        TrPayment_not.Visible = false;

                        if (application.enmPaymentStatus == enmPaymentStatus.Pending)
                        {
                            var institute = application.Institute;
                            TdInstituteInfo.InnerHtml = "<strong>Note</strong> * Please  deposit  fee / submit (as applicable) for exam form at the following Address till last date / अंतिम तारीख तक कृपया निम्न पते पर शुल्क / परीक्षा फार्म (जो लागू हो) जमा करें:- Approved Centre Name:- " + GetInitCap(institute.Name) + " , " + institute.AddressLine1 + "  ,  " + institute.AddressLine2 + institute.AddressLine3 +
                                "  " + institute.CityName + " ( " + institute.State.Name + " )  " + "<br>Pin Code / (पिन कोड): " + (institute.PinCode.HasValue ? institute.PinCode.Value.ToString() : "NA") + ", Phone Number / (फोन नंबर) : ";
                            //if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                            //{
                            //    TdInstituteInfo.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber1.Value;
                            //    if (institute.PhoneNumber2.HasValue)
                            //        TdInstituteInfo.InnerHtml += ", " + institute.PhoneNumber2.Value;
                            //}
                            //else
                            //    TdInstituteInfo.InnerHtml += "NA";
                            //deep add code after comment above  line on 2 aug 2018
                            if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                            {
                                TdInstituteInfo.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber1.Value + ", ";
                            }
                            else if (institute.StdNumber.HasValue && institute.PhoneNumber2.HasValue)
                            {
                                if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                                {
                                    TdInstituteInfo.InnerHtml += institute.PhoneNumber2.Value + ", ";
                                }
                                else
                                {
                                    TdInstituteInfo.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber2.Value + ", ";
                                }
                            }
                            else if (institute.StdNumber.HasValue && institute.FaxNumber.HasValue)
                            {
                                TdInstituteInfo.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.FaxNumber.Value + ", ";
                            }
                            else if (institute.MobileNumber.HasValue)
                            {
                                TdInstituteInfo.InnerHtml += institute.MobileNumber.Value;
                            }
                            else
                                TdInstituteInfo.InnerHtml += "NA";
                            //dee add code end on 2 aug 2018
                        }
                        else
                        {
                            TrInstitute.Style.Add("display", "none");
                        }
                        TrPaymentMsg.Visible = true;
                    }
                }
                else
                {
                    NormalHeader1.Visible = false;
                    if (Request.QueryString["Src"] != null)
                    {
                        if (Request.QueryString["Src"] == "CSC")
                        {
                            NormalHeader1.Visible = true;
                        }
                    }
                    TrDD.Visible = false;
                    TrDemandnote.Visible = false;
                    TrDemandnote1.Visible = false;
                    Btnsubmit.Visible = true;
                    BtnBack.Visible = true;
                    BtnPrint.Visible = false;
                    TrPaymentModeInstruction.Visible = false;
                    TrPayment_not.Visible = false;
                    TrPaymentMsg.Visible = false;
                }
                LblFeeAmount.Text = "Exam Fee:   " + application.TotalFeeAmount.Value.ToString("F");// 
                if (application.LateFeeAmount.HasValue)
                    if (application.LateFeeAmount.Value > 0)
                        LblFeeAmount.Text += " (Including Late Fee: " + application.LateFeeAmount.Value.ToString("F") + ")";
                LblFeeAmount.Text += "  [" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(application.TotalFeeAmount.Value.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + "]";

                if (application.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute))
                {
                    LblPaymentDescription.Text = "<b>Submission of examination form  is " + GetLastExamDate(application.Exam.ID, application.ApplicantTypeID, application.CourseID) + " and Last date of fee payment is " + GetLastPaymentDate(application.Exam.ID, application.ApplicantTypeID, application.CourseID) + "  </b>.";
                }
                else
                {
                    LblPaymentDescription.Text = "<b>Last date of fee payment and submission of examination form  is " + GetLastExamDate(application.Exam.ID, application.ApplicantTypeID, application.CourseID) + " after this date you will not be allowed to submit the examination form</b>.";
                }



                if (application.enmApplicationSource == enmApplicationSource.CSC)
                {
                    Int32 applicationtypeid = Convert.ToInt32(enmApplicationType.CertificateExamApplication);
                    Int32 activityid = Convert.ToInt32(enmCSCActivity.FillFormandDepositFee);
                    LblPaymentMode.Text = enmPaymentMode.CSCSPV.ToString();
                    int amt = CourseManager.GetCSCProcessingCharge(context, applicationtypeid, activityid);
                    LblFeeAmount.Text += "<br><i><B>Note: Rs. " + amt.ToString("F") + " [" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(amt.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + "] will be charged by CSC SPV for their services.";
                }
                if (application.DemandNote != null)
                {
                    DemandNote demandNote = application.DemandNote;
                    if (application.DemandNote.PaymentModeID == Convert.ToInt32(enmPaymentMode.CSCSPV))
                    {
                        TrDD.Visible = false;
                        LblPaymentMode.Text = enmPaymentMode.CSCSPV.ToString();
                        if (demandNote.enmPaymentStatus == enmPaymentStatus.PaidButNotVerified || demandNote.enmPaymentStatus == enmPaymentStatus.Paid)
                        {
                            LblPaymentSrc.Text = demandNote.CSCTransaction.ResponseTransactionNumber;
                        }
                        else
                        {
                            LblPaymentMode.Text = "";
                            LblPaymentDescription.Text += " Please pay your fee before last date through CSC SPV or Online Payment or NEFT/RTGS / अंतिम तारीख से पहले अपनी फीस का भुगतान सीएससी केंद्र पर/ऑनलाइन/ एनईएफटी/आरटीजीएस से करें.";
                        }
                    }
                    else if (application.DemandNote.PaymentModeID == Convert.ToInt32(enmPaymentMode.DemandDraft))
                    {
                        TrDD.Visible = true;
                        LblPaymentMode.Text = enmPaymentMode.DemandDraft.ToString();
                        if (demandNote.enmPaymentStatus == enmPaymentStatus.PaidButNotVerified || demandNote.enmPaymentStatus == enmPaymentStatus.Paid)
                        {
                            LblPaymentSrc.Text = demandNote.DDTransactionID.HasValue ? demandNote.DemandDraftTransaction.DemandDraftNumber : " NA (Allowed with Exemption.)";
                        }
                        else
                        {
                            LblPaymentMode.Text = "";
                        }
                    }
                    else if (application.DemandNote.PaymentModeID == Convert.ToInt32(enmPaymentMode.Online))
                    {
                        TrDD.Visible = false;
                        LblPaymentMode.Text = enmPaymentMode.Online.ToString();
                        LblFeeType.Text = "Exam Fee (in Rupees)";
                        LblFeeAmount.Text = application.TotalFeeAmount.Value.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(application.TotalFeeAmount.Value.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                        LblPaymentSrc.Text = "after making Online Payment. / ऑनलाइन से";
                        LblPaymentDescription.Text = " Please pay through Online Payment / कृपया ऑनलाइन भुगतान करें|";
                        if (demandNote.enmPaymentStatus == enmPaymentStatus.PaidButNotVerified || demandNote.enmPaymentStatus == enmPaymentStatus.Paid)
                        {
                            LblPaymentSrc.Text = demandNote.OnlineTransaction.ReferenceNumber;
                        }
                        else
                        {
                            LblPaymentMode.Text = "";
                            LblPaymentDescription.Text += " Please pay your fee before last date through CSC SPV or Online Payment or NEFT/RTGS / अंतिम तारीख से पहले अपनी फीस का भुगतान सीएससी केंद्र पर/ऑनलाइन/एनईएफटी/आरटीजीएस से करें.";
                        }
                    }
                    else if (application.DemandNote.PaymentModeID == Convert.ToInt32(enmPaymentMode.NEFTRTGS))
                    {
                        TrDD.Visible = false;
                        LblPaymentMode.Text = enmPaymentMode.NEFTRTGS.ToString();
                        LblFeeType.Text = "Exam Fee (in Rupees)";
                        LblFeeAmount.Text = application.TotalFeeAmount.Value.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(application.TotalFeeAmount.Value.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                        LblPaymentSrc.Text = "after making payment through NEFT/ RTGS. / एनईएफटी/ आरटीजीएस से";
                        LblPaymentDescription.Text = " Please pay through NEFT/ RTGS / कृपया एनईएफटी/ आरटीजीएस  से भुगतान करें|";
                        if (demandNote.enmPaymentStatus == enmPaymentStatus.PaidButNotVerified || demandNote.enmPaymentStatus == enmPaymentStatus.Paid)
                        {
                            LblPaymentSrc.Text = demandNote.NEFTTransaction.TransactionNumber;
                        }
                        else
                        {
                            LblPaymentMode.Text = "";
                            LblPaymentDescription.Text += " Please pay your fee before last date through CSC SPV or Online Payment or NEFT/RTGS / अंतिम तारीख से पहले अपनी फीस का भुगतान सीएससी केंद्र पर/ऑनलाइन/एनईएफटी/आरटीजीएस से करें.";
                        }
                    }
                }

                // Making Enclosures configurable 
                if (context.Exams.Where(s => s.ID == application.ExamID).FirstOrDefault().IsBatchProcessable)
                {
                    tddeclaration.InnerText = " 7. Declaration / घोषणा ";
                    tdpdetail.InnerText = " 8. Payment Detail ";
                }
                else
                {
                    trattested.Style.Add("display", "none");
                    trattestedheder.Style.Add("display", "none");
                    TrDD.Style.Add("display", "none");
                    tddeclaration.InnerText = " 6. Declaration / घोषणा ";
                    tdpdetail.InnerText = " 7. Payment Detail ";
                }

                LblAppNumber.Text = application.Number;
                LblAppDataTime.Text = application.ApplicationDate.ToString("dd-MMM-yyyy hh:mm:ss tt");
                Lblhead.Text = "Online Examination Application Form:" + " " + application.Course.Name + "(" + application.Course.Code + ")";
                lbldeccoursecode.Text = application.Course.Code.ToUpper();
                if (application.Course.Code.ToUpper() == "BCC")
                    lblhdeccoursecode.Text = " बीसीसी ";
                else
                    lblhdeccoursecode.Text = " सीसीसी ";
                LblCourseinEnglish.Text = application.Course.Code;
                spnCourseinHindi.InnerText = application.Course.Code;
                if (application.AlreadyApplied.ToString() == "True")
                {
                    LblIsPreExamined.Text = "Yes";
                    Lblexam.Visible = true;
                    LblLastMonthYear.Visible = true;
                    TrPreRollno.Visible = true;
                    if (application.PreviousExam != null)
                        LblLastMonthYear.Text = application.PreviousExam.Name;//ExamCycle
                    LblPreRno.Text = application.PreviousRollNumber.ToString();
                    TdAlreadyAppeared.RowSpan = 2;
                    TdLblAlreadyAppeared.RowSpan = 2;
                }
                else if (application.AlreadyApplied.ToString() == "False")
                {
                    LblIsPreExamined.Text = "No";
                    Lblexam.Visible = false;
                    LblLastMonthYear.Visible = false;
                    TrPreRollno.Visible = false;
                    TdAlreadyAppeared.RowSpan = 1;
                    TdLblAlreadyAppeared.RowSpan = 1;
                }
                LblExamCycle.Text = application.Course.Code.ToUpper() + " - " + application.Exam.Name + " (" + application.Exam.ExaminationCycle.Name + " )";
                LblExamCentre1.Text = application.ExamCenter1.Code + "-" + GetInitCap(application.ExamCenter1.Name); //Name of Exam Centre1
                LblExamCentre2.Text = application.ExamCenter2.Code + "-" + GetInitCap(application.ExamCenter2.Name);//Name of Exam Centre2
                imgPhotoBarcode.Src = "../Handlers/BarcodeHandler.ashx?Code=" + application.Number.ToString(); // Show Barcode on Photo
                imgBarCode.Src = "../Handlers/BarcodeHandler.ashx?Code=" + application.Number.ToString(); // Show Barcode on Footer

                //Candidate Personal Detail...
                LblAppName.Text = application.Salutation + " " + GetInitCap(application.Name);

                if (!string.IsNullOrEmpty(application.Salutation))
                {

                    if (application.Salutation.Trim() == "Mrs.".Trim() || application.Salutation.Trim() == "Miss".Trim() || application.Salutation.Trim() == "Ms.".Trim())
                    {
                        if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName))
                        {
                            Lblhsalutation.Visible = Lblsalutation.Visible = true;
                            Lblsalutation.Text = " daughter  of ";
                            Lblhsalutation.Text = " की पुत्री ";
                            Lblhdectype.Text = " करती ";
                        }
                        else
                        {
                            Lblhsalutation.Visible = Lblsalutation.Visible = false;
                            Lblhdectype.Text = " करती ";
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName))
                        {
                            Lblhsalutation.Visible = Lblsalutation.Visible = true;
                            Lblsalutation.Text = " son  of ";
                            Lblhsalutation.Text = " का पुत्र ";
                            Lblhdectype.Text = " करता ";
                        }
                        else
                        {
                            Lblhsalutation.Visible = Lblsalutation.Visible = false;
                            Lblhdectype.Text = " करता ";
                        }
                    }
                }
                if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName))
                {
                    TrFatherName.Visible = true;
                    TrMotherName.Visible = true;
                    TrGardianName.Visible = false;
                    LblFName.Text = " Mr. " + GetInitCap(application.FatherName);
                    LblDecFname.Visible = Lblhdecfathername.Visible = true;
                    LblDecFname.Text = " and <b> Shri " + GetInitCap(application.FatherName) + "</b>";
                    Lblhdecfathername.Text = " और <b> श्री " + GetInitCap(application.FatherName) + "</b>";
                    LblMName.Text = " Mrs. " + GetInitCap(application.MotherName);
                    LblMName.Visible = LblhDecmname.Visible = true;
                    LblDecMName.Text = " <b> Smt " + GetInitCap(application.MotherName) + "</b>";
                    LblhDecmname.Text = " <b> श्रीमती " + GetInitCap(application.MotherName) + "</b>";
                }
                else
                {
                    TrFatherName.Visible = false;
                    TrMotherName.Visible = false;
                    TrGardianName.Visible = true;
                    LblGuardianName.Text = GetInitCap(application.GuardianName);
                    LblDecFname.Visible = Lblhdecfathername.Visible = false;
                    LblDecMName.Visible = LblhDecmname.Visible = false;


                }
                LblGender.Text = application.Gender;
                LblDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");
                LblCategory.Text = GetInitCap(application.CastCategory.Name) + " / " + GetInitCap(application.CastCategory.NameRegional);
                LblOccuption.Text = GetInitCap(application.Occupation.Name) + " / " + GetInitCap(application.Occupation.NameRegional);
                if (application.IsDisability.HasValue)
                {
                    TrDisability.Visible = true;
                    LblDisability.Text = (application.IsDisability == true) ? "Yes" : "No";
                    LblDisabilityType.Text = (application.IsDisability == true) ? application.DisabilityType.Name : "Not Applicable";
                }

                if (application.OccupationID == 5) // Gujgovt
                {
                    Troccupation1.Visible = true;
                    Troccupation2.Visible = true;
                    Troccupation3.Visible = true;
                    lbldesg.Text = GetInitCap(application.Designation);
                    lbempcode.Text = GetInitCap(application.EmployeeCode);
                    lbdepartment.Text = GetInitCap(application.Department);
                    lblplace.Text = GetInitCap(application.PostingCity);
                    lbldjoin.Text = application.DateofJoining.HasValue ? application.DateofJoining.Value.ToString("dd-MMM-yyyy") : "N/A";
                    lbldretment.Text = application.DateofRetirement.HasValue ? application.DateofRetirement.Value.ToString("dd-MMM-yyyy") : "N/A";
                }
                else
                {
                    Troccupation1.Visible = false;
                    Troccupation2.Visible = false;
                    Troccupation3.Visible = false;
                }

                ImgApplicantPhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])application.Photo); // Show  Photo
                imgThumbImpression.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])application.LeftThumb); // Show Left Thumb Impression image
                imgSignature.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])application.Signature);// Show Signature image

                //Candidate (By default Corespondance here) Address Detail...
                LblAddressLine1.Text = GetInitCap(application.CorAddressLine1.ToString());
                LblAddressLine2.Text = GetInitCap(application.CorAddressLine2.ToString());
                LblAddressLine3.Text = string.IsNullOrEmpty(application.CorAddressLine3) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine3) ? GetInitCap(application.CorAddressLine3) : "";
                LblCity.Text = GetInitCap(application.CorCityName);
                LblState.Text = GetInitCap(application.CorState.Name);
                LblDistrict.Text = GetInitCap(application.CorDistrict.Name);
                LblPinCode.Text = application.CorPinCode.ToString();


                //Candidate Contact Detail...
                LblMobile.Text = application.MobileNumber.ToString();
                if (String.IsNullOrEmpty(application.StdNumber.ToString()) == false && string.IsNullOrEmpty(application.PhoneNumber.ToString()) == false)
                    LblLandLine.Text = "0" + application.StdNumber.ToString() + "-" + application.PhoneNumber.ToString();
                //                LblEmail.Text = application.EmailAddress.ToString();
                LblEmail.Text = CommonFunctions.ChangeEmailDisplay(application.EmailAddress.ToString());


                //Candidate Educational Qualification detail
                LblHeighEducation.Text = GetInitCap(application.EducationalQualification.Name);
                LblYearofPassing.Text = application.PassingYear.ToString();

                //If Applied as Institute then This code will get all the Location Details of Institute
                if (application.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute))
                {

                    var intituteDetail = (from a in context.AccreditationDetails
                                          join i in context.Institutes on a.InstituteID equals i.ID
                                          where i.ID == application.InstituteID
                                          select new
                                          {
                                              Name = a.AccreditationNumber + " - " + i.Name + ", " + i.CityName,
                                              StateName = i.StateID != 0 ? i.State.Name : "NA",
                                              DistrictName = i.DistrictID.HasValue ? i.District.Name : "NA",
                                              AccNo = a.AccreditationNumber
                                          }).FirstOrDefault();

                    LblAccState.Text = GetInitCap(intituteDetail.StateName.ToString());
                    LblAccCentre.Text = GetInitCap(intituteDetail.Name.ToString());
                    LblAppliedAs.Text = enmApplicantType.Institute.ToString();
                    TdAppliedAs.RowSpan = 2;
                    TrLblAppliedAs.RowSpan = 2;
                    TrAccCentre.Visible = true;
                    LblAccstateHead.Visible = true;
                }
                else if (application.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                {

                    LblAppliedAs.Text = enmApplicantType.Direct.ToString();
                    TrAccCentre.Visible = false;
                    LblAccstateHead.Visible = false;
                    TrLblAppliedAs.RowSpan = 1;
                    TdAppliedAs.RowSpan = 1;
                }
            };
            if (LblPaymentMode.Text.Length <= 0)
            {
                LblPaymentMode.Text = "Not Paid"; LblPaymentMode.Font.Italic = true;
                LblPaymentSrc.Text = "Not Paid"; LblPaymentSrc.Font.Italic = true;
            }

        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }
    protected Int64 GetApplicationID(string rollNumber, Int32 courseID)
    {
        try
        {
            Int64 applID = 0;
            using (EConnectContext context = new EConnectContext())
            {
                var rnoObj = context.CertificateExamApplications.Where(s => s.RollNumber.ToUpper() == rollNumber && s.CourseID == courseID).FirstOrDefault();
                if (rnoObj != null)
                    applID = rnoObj.ID;
            };
            return applID;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected Int32 GetCurrentExamID(Int32 applicantTypeID, Int32 courseID)
    {
        try
        {
            Int32 ExamID = 0;
            Int32 NormalactivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
            Int32 LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
            if (courseID != 0)
            {
                using (EConnectContext context = new EConnectContext())
                {

                    var LateFeeExam = (from e in context.CutOffDates
                                       join i in context.Exams on e.ExamID equals i.ID
                                       where e.CourseID == courseID
                                       && e.ApplicantTypeID == applicantTypeID
                                       && e.ActivityID == LateFeeActivityId
                                       && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                       orderby e.EfferctiveDate ascending
                                       select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
                    if (LateFeeExam.Count() > 0)//If  applicable for late fee ?
                    {

                        if (LateFeeExam != null)
                        {
                            ExamID = LateFeeExam.FirstOrDefault().ExamID;
                        }
                    }
                    else if (LateFeeExam.Count() <= 0)//If  not applicable for late fee ?
                    {

                        var NormalFeeExam = (from e in context.CutOffDates
                                             join i in context.Exams on e.ExamID equals i.ID
                                             where e.CourseID == courseID
                                             && e.ApplicantTypeID == applicantTypeID
                                             && e.ActivityID == NormalactivityId
                                             && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                             orderby e.EfferctiveDate ascending
                                             select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
                        if (NormalFeeExam.Count() > 0)
                        {
                            ExamID = NormalFeeExam.FirstOrDefault().ExamID;
                        }
                    }

                };
            }
            return ExamID;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected Int32 GetNextExamID(Int32 applicantTypeID, Int32 courseID, Int32 examCycleID)
    {
        try
        {
            Int32 examID = 0;
            using (EConnectContext context = new EConnectContext())
            {
                int StartDateofFormFilling = Convert.ToInt32(enmActivity.DateFfCommencementOfOnlineFillInExaminationApplicationForm);
                var examsAll = (from f in context.Exams
                                where f.CourseID == courseID && f.ExaminationCycleID == examCycleID &&
                                f.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && f.DateOfPublishingOfRollNumber == null
                                select f);

                examsAll = examsAll.Where(a => a.CutOffDates.Where(k => k.ActivityID == StartDateofFormFilling && k.ApplicantTypeID == applicantTypeID).FirstOrDefault().EfferctiveDate <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now));

                if (examsAll.Count() > 0)
                {
                    int LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                    int NormalFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                    var exmasWithNormalLastDate = (from t in context.CutOffDates
                                                   where examsAll.Select(d => d.ID).Contains(t.ExamID) && t.ApplicantTypeID == applicantTypeID &&
                                                   t.ActivityID == NormalFeeActivityId && t.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                                   orderby t.Exam.ExamStartDate
                                                   select t.Exam).Distinct();
                    if (exmasWithNormalLastDate.Count() == 0)
                    {
                        var exmasWithLateFeeLastDate = (from t in context.CutOffDates
                                                        where examsAll.Select(d => d.ID).Contains(t.ExamID) && t.ApplicantTypeID == applicantTypeID &&
                                                        t.ActivityID == LateFeeActivityId && t.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                                        orderby t.Exam.ExamStartDate
                                                        select t.Exam).Distinct();
                        examID = exmasWithLateFeeLastDate.Select(c => c.ID).FirstOrDefault();
                    }
                    else
                    {
                        examID = exmasWithNormalLastDate.Select(c => c.ID).FirstOrDefault();
                    }
                }
                else
                    examID = examsAll.Select(c => c.ID).FirstOrDefault();
            };
            return examID;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected string GetLastPaymentDate(Int32 examID, Int32 applicantTypeID, Int32 courseID)
    {
        try
        {
            string Lastdate = "";
            if (examID != 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    int LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                    int NormalFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                    int LateFeeTypeId = Convert.ToInt32(enmFeeType.LateFeeRegistration);
                    var Fee = (from f in context.CutOffDates
                               where f.CourseID == courseID && f.ExamID == examID && f.ApplicantTypeID == applicantTypeID
                               select new { EffectiveDate = f.EfferctiveDate, f.ActivityID }).ToList();

                    var NormalFee = Fee.Where(l => l.ActivityID == NormalFeeActivityId);
                    var lateFee = Fee.Where(l => l.ActivityID == LateFeeActivityId);
                    if (lateFee.Count() > 0 && lateFee != null)
                    {
                        if (NormalFee.FirstOrDefault().EffectiveDate <= DateTime.Now)
                        {

                            DateTime LastExamDate = (from f in context.FeeDetails
                                                     where f.CourseID == courseID && f.FeeTypeID == LateFeeTypeId &&
                                                     f.EffectiveFromDate == (from c in context.FeeDetails
                                                                             where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                                                                             select c.EffectiveFromDate).Max()
                                                     select new { LastExamDate = f.EffectiveFromDate }).FirstOrDefault().LastExamDate;

                            if (LastExamDate != null)
                            {
                                Lastdate = LastExamDate.ToString("dd-MMM-yyyy");
                            }
                        }
                    }
                    else
                        Lastdate = NormalFee.FirstOrDefault().EffectiveDate.ToString("dd-MMM-yyyy");

                    Int64 applID = Convert.ToInt64(Request.QueryString["Appid"]);
                    var application = context.CertificateExamApplications.Find(applID);


                    if (application.enmApplicantType == enmApplicantType.Institute)
                    {
                        int FeeSubmissionOfInstituteExtensionPeriodInDays = (from c in context.Exams
                                                                             where c.ID == application.ExamID && c.CourseID == application.CourseID
                                                                             select c.FeeSubmissioInstituteExtPeriod).FirstOrDefault();
                        Lastdate = (Convert.ToDateTime(Lastdate).AddDays(FeeSubmissionOfInstituteExtensionPeriodInDays)).ToString("dd-MMM-yyyy");
                    }

                };

            }
            return Lastdate;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected string GetLastExamDate(Int32 examID, Int32 applicantTypeID, Int32 courseID)
    {
        try
        {
            string Lastdate = "";
            if (examID != 0)
            {
                using (EConnectContext context = new EConnectContext())
                {


                    int LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                    int NormalFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                    int LateFeeTypeId = Convert.ToInt32(enmFeeType.LateFeeRegistration);
                    var Fee = (from f in context.CutOffDates
                               where f.CourseID == courseID &&
                               f.ExamID == examID &&
                               f.ApplicantTypeID == applicantTypeID
                               select new { EffectiveDate = f.EfferctiveDate, f.ActivityID }).ToList();

                    var NormalFee = Fee.Where(l => l.ActivityID == NormalFeeActivityId);
                    var lateFee = Fee.Where(l => l.ActivityID == LateFeeActivityId);
                    if (lateFee.Count() > 0 && lateFee != null)
                    {
                        if (NormalFee.FirstOrDefault().EffectiveDate <= DateTime.Now)
                        {
                            DateTime LastExamDate = (from f in context.FeeDetails
                                                     where f.CourseID == courseID && f.FeeTypeID == LateFeeTypeId &&
                                                     f.EffectiveFromDate == (from c in context.FeeDetails
                                                                             where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                                                                             select c.EffectiveFromDate).Max()
                                                     select new { LastExamDate = f.EffectiveFromDate }).FirstOrDefault().LastExamDate;
                            if (LastExamDate != null)
                            {
                                Lastdate = LastExamDate.ToString("dd-MMM-yyyy");
                            }
                        }
                    }
                    else
                        Lastdate = NormalFee.FirstOrDefault().EffectiveDate.ToString("dd-MMM-yyyy");
                };

            }
            return Lastdate;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion





}