using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class nieletpaymentservices : BasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        tdError.InnerText = "";

        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["ServiceID"]))
            {
                Int32 serviceid = Convert.ToInt32(Request.QueryString["ServiceID"]);
                if (serviceid == 1 || serviceid == 2 || serviceid == 4)
                {
                    ddlServices.SelectedValue = serviceid.ToString();
                    ddlServices.Enabled = false;
                    ddlServices_SelectedIndexChanged(ddlServices.SelectedValue, EventArgs.Empty);
                }
                else
                {                    
                    tdError.InnerText = "Invalid ServiceID. Please select the service mentioned below.";
                }
            }            
            if (!string.IsNullOrEmpty(Request.QueryString["DID"]))
            {
                ddlServices.SelectedValue = "4";
                ddlServices_SelectedIndexChanged(ddlServices, EventArgs.Empty);
                txtno.Text = Request.QueryString["DID"].ToString();
                BtnSearch_Click(BtnSearch, EventArgs.Empty);
                btnResetDemadnNote.Visible = false;
                BtnSearch.Visible = false;
            }
        }
    }
    protected void ddlServices_SelectedIndexChanged(object sender, EventArgs e)
    {
        d1.Visible = false;
        enmApplicationType selectdService = (enmApplicationType)Convert.ToInt32(ddlServices.SelectedValue);
        if (ddlServices.SelectedValue != "0")
        {
            trDemandNote.Visible = false;
            if (selectdService == enmApplicationType.CourseRegistrationApplication)
            {
                trExams.Visible = true;
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
                    ListItem lst = new ListItem("--Select One--", "0");
                    var courses = from s in context.Courses
                                  join ct in context.CourseCategories
                                      on s.CourseTypeID equals ct.ID
                                  where s.CourseTypeID == courseType
                                  select new { ValueField = s.ID, TextField = s.Name + " (" + s.CourseCategory.Name + ")" };

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlexam, courses, lst);
                };
            }
            else if (selectdService == enmApplicationType.CertificateExamApplication)
            {
                trExams.Visible = true;
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 courseType = Convert.ToInt32(enmCourseType.CertificationExam);
                    ListItem lst = new ListItem("--Select One--", "0");
                    var courses = from s in context.Courses
                                  join ct in context.CourseCategories
                                      on s.CourseTypeID equals ct.ID
                                  where s.CourseTypeID == courseType
                                  select new { ValueField = s.ID, TextField = s.Name + " (" + s.CourseCategory.Name + ")" };

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlexam, courses, lst);
                };
            }
            else
            {
                txtno.Text = "";
                trExams.Visible = false;
                ddlexam.Items.Clear();
                trDemandNote.Visible = true;
                txtno.Focus();
            }
        }
        if (ddlServices.SelectedValue == "4")
        {
            BtnSubmit.Visible = false;
            BtnCancel.Visible = false;
            trDemandNote.Visible = true;
        }
        if (ddlServices.SelectedValue == "0")
        {
            trExams.Visible = false;
            trDemandNote.Visible = false;
            BtnSubmit.Visible = false;
            BtnCancel.Visible = false;
        }
        //else
        //{
        //    Response.Redirect("nieletpaymentservices.aspx");
        //}
    }


    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            tdError.InnerText = "";
            d1.Visible = false;
            if (txtno.Text.Trim() == "")
                throw new Exception("Please enter demand note number");
            using (EConnectContext context = new EConnectContext())
            {
                Int64 demantNoteID = Convert.ToInt64(txtno.Text);
                DemandNote demandNote = context.DemandNotes.Find(demantNoteID);
                var coursExam = context.CourseExamApplications.Where(s => s.DemandNoteID == demandNote.ID).ToList();
                var certificateExam = context.CertificateExamApplications.Where(s => s.DemandNoteID == demandNote.ID).ToList();
                var coursReg = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandNote.ID).ToList();
                if (demandNote != null && (coursExam.Count() > 0 || certificateExam.Count() > 0 || coursReg.Count() > 0))
                {
                    if (demandNote.enmPaymentMode == enmPaymentMode.CSCSPV)
                    {
                        if (demandNote.enmPaymentStatus == enmPaymentStatus.Pending)
                        {
                            tdDemandNumber.InnerHtml = demandNote.ID.ToString() + " <i>Dated:</i> " + demandNote.ApplicationDate.ToString("dd-MMM-yyyy");
                            tdAmount.InnerHtml = demandNote.Amount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(demandNote.Amount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                            tdPaymentDescription.InnerHtml = "";
                            if (demandNote.enmDemandNoteType == enmDemandNoteType.Single)
                            {
                                trextra.Visible = false;
                                if (demandNote.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                                {
                                    tdPaymentDescription.InnerText = "Registration Fee ";
                                    var payee = (from p in context.CourseRegistrationApplications
                                                 where p.DemandNoteID == demandNote.ID
                                                 select new { FeeDetail = p.Course.Name + " (" + p.Course.Code + ")", Name = p.Salutation + " " + p.Name, FatherName = p.FatherName, MotherName = p.MotherName, DOB = p.DateOfBirth, p.GuardianName, sourceid = p.ApplicationSourceID }).FirstOrDefault();
                                    if (payee != null)
                                    {
                                        Int32 applicationtypeid = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                                        Int32 activityid = 0;
                                        Int32 feeamount = 0;
                                        if (payee.sourceid == Convert.ToInt32(enmApplicationSource.Candidate))
                                        {
                                            activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                            feeamount = CourseManager.GetCSCProcessingCharge(context, applicationtypeid, activityid);
                                            tdcscamount.InnerHtml = feeamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                        }
                                        else if (payee.sourceid == Convert.ToInt32(enmApplicationSource.CSC))
                                        {
                                            activityid = Convert.ToInt32(enmCSCActivity.FillFormandDepositFee);
                                            feeamount = CourseManager.GetCSCProcessingCharge(context, applicationtypeid, activityid);
                                            tdcscamount.InnerHtml = feeamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                        }
                                        tdPayeeName.InnerText = GetInitCap(payee.Name);
                                        if (payee.GuardianName != null)
                                            tdPayeeFatherName.InnerText = GetInitCap(payee.GuardianName) + " (Guardian)";
                                        else
                                        {
                                            tdPayeeFatherName.InnerText = "Mr. " + GetInitCap(payee.FatherName);
                                            tdPayeeMotherName.InnerText = "Mrs. " + GetInitCap(payee.MotherName);
                                        }
                                        tdPaymentDescription.InnerText = "Registration Fee: " + payee.FeeDetail;
                                    }
                                    else
                                    {
                                        tdPayeeName.InnerText = "NA";
                                        tdPayeeFatherName.InnerText = "NA";
                                        tdPayeeMotherName.InnerText = "NA";
                                        tdPaymentDescription.InnerText = "NA";
                                    }
                                }
                                else if (demandNote.enmApplicationType == enmApplicationType.CertificateExamApplication)
                                {
                                    var payee = (from p in context.CertificateExamApplications
                                                 where p.DemandNoteID == demandNote.ID
                                                 select new { appl = p, FeeDetail = p.Exam.Name + " (" + p.Course.Code + ")", Name = p.Salutation + " " + p.Name, FatherName = p.FatherName, MotherName = p.MotherName, DOB = p.DateOfBirth, p.GuardianName, sourceid = p.ApplicationSourceID }).FirstOrDefault();
                                    if (payee != null)
                                    {
                                        Int32 submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                                        if (payee.appl.LateFeeAmount.HasValue)
                                            if (payee.appl.LateFeeAmount.Value > 0)
                                                submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                                        DateTime lastDate = (from c in context.CutOffDates
                                                             where c.ActivityID == submisssionDateActivityID && c.ExamID == payee.appl.ExamID && c.ApplicantTypeID == payee.appl.ApplicantTypeID
                                                             select c.EfferctiveDate).FirstOrDefault();

                                        if (lastDate.Date < DateTime.Now.Date)
                                        {
                                            throw new Exception("This demand note has been expired on " + lastDate.ToString("dd-MMM-yyy"));
                                        }
                                        Int32 applicationtypeid = Convert.ToInt32(enmApplicationType.CertificateExamApplication);
                                        Int32 activityid = 0;
                                        Int32 feeamount = 0;
                                        if (payee.sourceid == Convert.ToInt32(enmApplicationSource.Candidate))
                                        {
                                            activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                            feeamount = CourseManager.GetCSCProcessingCharge(context, applicationtypeid, activityid);
                                            tdcscamount.InnerHtml = feeamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                        }
                                        else if (payee.sourceid == Convert.ToInt32(enmApplicationSource.CSC))
                                        {
                                            activityid = Convert.ToInt32(enmCSCActivity.FillFormandDepositFee);
                                            feeamount = CourseManager.GetCSCProcessingCharge(context, applicationtypeid, activityid);
                                            tdcscamount.InnerHtml = feeamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                        }
                                        tdPayeeName.InnerText = GetInitCap(payee.Name);
                                        if (payee.GuardianName != null)
                                            tdPayeeFatherName.InnerText = GetInitCap(payee.GuardianName) + " (Guardian)";
                                        else
                                        {
                                            tdPayeeFatherName.InnerText = "Mr. " + GetInitCap(payee.FatherName);
                                            tdPayeeMotherName.InnerText = "Mrs. " + GetInitCap(payee.MotherName);
                                        }
                                        tdPaymentDescription.InnerText = "Registration Cum Examination Fee: " + payee.FeeDetail;
                                    }
                                    else
                                    {
                                        tdPayeeName.InnerText = "NA";
                                        tdPayeeFatherName.InnerText = "NA";
                                        tdPayeeMotherName.InnerText = "NA";
                                        tdPaymentDescription.InnerText = "NA";
                                    }
                                }
                                else if (demandNote.enmApplicationType == enmApplicationType.CourseExamApplication)
                                {
                                    tdPaymentDescription.InnerText = "Examination Fee ";
                                    var payee = (from p in context.CourseExamApplications
                                                 join c in context.Candidates on p.CandidateID equals c.ID
                                                 where p.DemandNoteID == demandNote.ID
                                                 select new { appl = p, FeeDetail = p.Exam.Name + " (" + p.Course.Name + ")", Name = c.Salutation + " " + c.Name, FatherName = c.FatherName, MotherName = c.MotherName, DOB = c.DateOfBirth, c.GuardianName }).FirstOrDefault();
                                    if (payee != null)
                                    {
                                        Int32 submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                                        if (payee.appl.LateFeeImposed)
                                        {
                                            submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                                        }
                                        DateTime lastDate = (from c in context.CutOffDates
                                                             where c.ActivityID == submisssionDateActivityID && c.ExamID == payee.appl.ExamID && c.ApplicantTypeID == payee.appl.ApplicantTypeID
                                                             select c.EfferctiveDate).FirstOrDefault();


                                        // Added by shivesh 06-Feb-2016
                                        int FeeSubmissionOfInstituteExtensionPeriodInDays = (from c in context.Exams
                                                                                             where c.ID == payee.appl.ExamID && (c.FeeSubmissioInstituteExtPeriod != null || c.FeeSubmissioInstituteExtPeriod != 0)
                                                                                             select c.FeeSubmissioInstituteExtPeriod).FirstOrDefault();

                                        int Applicant_Type_ID = (from c in context.CourseExamApplications
                                                                 where c.DemandNoteID == demantNoteID
                                                                 select c.ApplicantTypeID).Distinct().FirstOrDefault();

                                        if (Applicant_Type_ID == 2 && FeeSubmissionOfInstituteExtensionPeriodInDays != 0)
                                        {
                                            
                                            lastDate = lastDate.AddDays(FeeSubmissionOfInstituteExtensionPeriodInDays);
                                            
                                        }
                                        else
                                        {
                                           // throw new Exception("This demand note has been expired on " + lastDate.ToString("dd-MMM-yyy"));
                                            //lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");
						 lastDate = lastDate.AddDays (0);
                                        }
                                        //added end shivesh 06-Feb-2016





                                        if (lastDate.Date < DateTime.Now.Date)
                                        {
                                            throw new Exception("This demand note has been expired on " + lastDate.ToString("dd-MMM-yyy"));
                                        }
                                        Int32 applicationtypeid = Convert.ToInt32(enmApplicationType.CourseExamApplication);
                                        Int32 activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                        Int32 feeamount = CourseManager.GetCSCProcessingCharge(context, applicationtypeid, activityid); ;
                                        tdcscamount.InnerHtml = feeamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                    }
                                    tdPayeeName.InnerText = GetInitCap(payee.Name);
                                    if (payee.GuardianName != null)
                                        tdPayeeFatherName.InnerText = GetInitCap(payee.GuardianName) + " (Guardian)";
                                    else
                                    {
                                        tdPayeeFatherName.InnerText = "Mr. " + GetInitCap(payee.FatherName);
                                        tdPayeeMotherName.InnerText = "Mrs. " + GetInitCap(payee.MotherName);
                                    }
                                    tdPaymentDescription.InnerText = "Examination Fee: " + payee.FeeDetail;
                                }
                            }
                            else
                            {
                                tdNameCaption.InnerText = "Name of Institute";
                                tdFnameCaption.InnerText = "Address Line 1";
                                tdMNameCaption.InnerText = "Address Line 2";
                                trextra.Visible = true;
                                tdDobCaption.InnerText = "Address Line 3";
                                if (demandNote.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                                {
                                    tdPaymentDescription.InnerText = "Registration Fee ";
                                    var payee = (from p in context.CourseRegistrationApplications
                                                 where p.DemandNoteID == demandNote.ID
                                                 select new
                                                 {
                                                     FeeDetail = p.Course.Name + " (" + p.Course.Code + ")",
                                                     Name = p.Institute.Name,
                                                     Address1 = p.Institute.AddressLine1 + " " + p.Institute.AddressLine2,
                                                     Address2 = p.Institute.AddressLine3 + " " + p.Institute.CityName,
                                                     State = p.Institute.State.Name,
                                                     Pin = p.Institute.PinCode,
                                                 }).FirstOrDefault();
                                    if (payee != null)
                                    {
                                        Int32 applicationtypeid = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                                        Int32 activityid = 0;
                                        Int32 feeamount = 0;
                                        activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                        feeamount = CourseManager.GetCSCProcessingCharge(context, applicationtypeid, activityid);
                                        tdcscamount.InnerHtml = feeamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                    }
                                    tdPayeeName.InnerText = payee.Name.ToUpper();
                                    if (payee.Address1 != null)
                                        tdPayeeFatherName.InnerText = payee.Address1.ToUpper();
                                    if (payee.Address2 != null)
                                        tdPayeeMotherName.InnerText = payee.Address2.ToUpper();
                                    if (payee.State != null)
                                        tdPayeeDOB.InnerText = payee.State.ToUpper();
                                    if (payee.Pin.HasValue)
                                        tdPayeeDOB.InnerText += ", Pin: " + payee.Pin.Value.ToString();
                                    tdPaymentDescription.InnerText = "Registration Fee: " + payee.FeeDetail;


                                }
                                else if (demandNote.enmApplicationType == enmApplicationType.CertificateExamApplication)
                                {
                                    var payee = (from p in context.CertificateExamApplications
                                                 where p.DemandNoteID == demandNote.ID
                                                 select new
                                                 {
                                                     FeeDetail = p.Exam.Name + " (" + p.Course.Code + ")",
                                                     Name = p.Institute.Name,
                                                     Address1 = p.Institute.AddressLine1 + " " + p.Institute.AddressLine2,
                                                     Address2 = p.Institute.AddressLine3 + " " + p.Institute.CityName,
                                                     State = p.Institute.State.Name,
                                                     Pin = p.Institute.PinCode,
                                                     appl = p
                                                 }).FirstOrDefault();
                                    if (payee != null)
                                    {
                                        Int32 submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                                        if (payee.appl.LateFeeAmount.HasValue)
                                            if (payee.appl.LateFeeAmount.Value > 0)
                                                submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                                        DateTime lastDate = (from c in context.CutOffDates
                                                             where c.ActivityID == submisssionDateActivityID && c.ExamID == payee.appl.ExamID && c.ApplicantTypeID == payee.appl.ApplicantTypeID
                                                             select c.EfferctiveDate).FirstOrDefault();


                                        // Added for DVP-BCC 20 Jan 2023
                                        int FeeSubmissionOfInstituteExtensionPeriodInDays = (from c in context.Exams
                                                                                             where c.ID == payee.appl.ExamID && (c.FeeSubmissioInstituteExtPeriod != null || c.FeeSubmissioInstituteExtPeriod != 0)
                                                                                             && c.CourseID ==175 && c.ExaminationCycleID ==185
                                                                                             select c.FeeSubmissioInstituteExtPeriod).FirstOrDefault();

                                        int Applicant_Type_ID = (from c in context.CertificateExamApplications
                                                                 where c.DemandNoteID == demantNoteID
                                                                 select c.ApplicantTypeID).Distinct().FirstOrDefault();

                                        if (Applicant_Type_ID == 2 && FeeSubmissionOfInstituteExtensionPeriodInDays != 0)
                                        {

                                            lastDate = lastDate.AddDays(FeeSubmissionOfInstituteExtensionPeriodInDays);

                                        }
                                        else
                                        {
                                            // throw new Exception("This demand note has been expired on " + lastDate.ToString("dd-MMM-yyy"));
                                            //lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");
                                            lastDate = lastDate.AddDays(0);
                                        }
                                        //added end 




                                        if (lastDate.Date < DateTime.Now.Date)
                                        {
                                            throw new Exception("This demand note has been expired on " + lastDate.ToString("dd-MMM-yyy"));
                                        }
                                        Int32 applicationtypeid = Convert.ToInt32(enmApplicationType.CertificateExamApplication);
                                        Int32 activityid = 0;
                                        Int32 feeamount = 0;
                                        activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                        feeamount = CourseManager.GetCSCProcessingCharge(context, applicationtypeid, activityid);
                                        tdcscamount.InnerHtml = feeamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                        tdPayeeName.InnerText = payee.Name.ToUpper();
                                        if (payee.Address1 != null)
                                            tdPayeeFatherName.InnerText = payee.Address1.ToUpper();
                                        if (payee.Address2 != null)
                                            tdPayeeMotherName.InnerText = payee.Address2.ToUpper();
                                        if (payee.State != null)
                                            tdPayeeDOB.InnerText = payee.State.ToUpper();
                                        if (payee.Pin.HasValue)
                                            tdPayeeDOB.InnerText += ", Pin: " + payee.Pin.Value.ToString();
                                        tdPaymentDescription.InnerText = "Registration Cum Examination Fee: " + payee.FeeDetail;
                                    }
                                    else
                                    {
                                        tdPayeeName.InnerText = "NA";
                                        tdPayeeFatherName.InnerText = "NA";
                                        tdPayeeMotherName.InnerText = "NA";
                                        tdPayeeDOB.InnerText = "NA";
                                        tdPaymentDescription.InnerText = "NA";
                                    }
                                }
                                else if (demandNote.enmApplicationType == enmApplicationType.CourseExamApplication)
                                {
                                    var payee = (from p in context.CourseExamApplications
                                                 where p.DemandNoteID == demandNote.ID
                                                 select new
                                                 {
                                                     FeeDetail = p.Exam.Name + " (" + p.Course.Name + ")",
                                                     Name = p.Institute.Name,
                                                     Address1 = p.Institute.AddressLine1 + " " + p.Institute.AddressLine2,
                                                     Address2 = p.Institute.AddressLine3 + " " + p.Institute.CityName,
                                                     State = p.Institute.State.Name,
                                                     Pin = p.Institute.PinCode,
                                                     appl = p
                                                 }).FirstOrDefault();
                                    if (payee != null)
                                    {
                                        Int32 submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                                        if (payee.appl.LateFeeImposed)
                                        {
                                            submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                                        }
                                        DateTime lastDate = (from c in context.CutOffDates
                                                             where c.ActivityID == submisssionDateActivityID && c.ExamID == payee.appl.ExamID && c.ApplicantTypeID == payee.appl.ApplicantTypeID
                                                             select c.EfferctiveDate).FirstOrDefault();

                                        // Added by shivesh 06-Feb-2016
                                        int FeeSubmissionOfInstituteExtensionPeriodInDays = (from c in context.Exams
                                                                                             where c.ID == payee.appl.ExamID && (c.FeeSubmissioInstituteExtPeriod != null || c.FeeSubmissioInstituteExtPeriod != 0)
                                                                                             select c.FeeSubmissioInstituteExtPeriod).FirstOrDefault();

                                        int Applicant_Type_ID = (from c in context.CourseExamApplications
                                                                 where c.DemandNoteID == demantNoteID
                                                                 select c.ApplicantTypeID).Distinct().FirstOrDefault();

                                        if (Applicant_Type_ID == 2 && FeeSubmissionOfInstituteExtensionPeriodInDays != 0)
                                        {

                                            lastDate = lastDate.AddDays(FeeSubmissionOfInstituteExtensionPeriodInDays);

                                        }
                                        else
                                        {
					//Commented on 17 Nov 2022
                                        //    throw new Exception("This demand note has been expired on " + lastDate.ToString("dd-MMM-yyy"));
					//
                                            //lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");
					lastDate = lastDate.AddDays (0);
                                        }
                                        //added end shivesh 06-Feb-2016

                                        if (lastDate.Date < DateTime.Now.Date)
                                        {
                                            throw new Exception("This demand note has been expired on " + lastDate.ToString("dd-MMM-yyy"));
                                        }
                                        Int32 applicationtypeid = Convert.ToInt32(enmApplicationType.CourseExamApplication);
                                        Int32 activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                        Int32 feeamount = CourseManager.GetCSCProcessingCharge(context, applicationtypeid, activityid);
                                        tdcscamount.InnerHtml = feeamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                        tdPayeeName.InnerText = payee.Name.ToUpper();
                                        if (payee.Address1 != null)
                                            tdPayeeFatherName.InnerText = payee.Address1.ToUpper();
                                        if (payee.Address2 != null)
                                            tdPayeeMotherName.InnerText = payee.Address2.ToUpper();
                                        if (payee.State != null)
                                            tdPayeeDOB.InnerText = payee.State.ToUpper();
                                        if (payee.Pin.HasValue)
                                            tdPayeeDOB.InnerText += ", Pin: " + payee.Pin.Value.ToString();
                                        tdPaymentDescription.InnerText = "Examination Fee: " + payee.FeeDetail;
                                    }
                                    else
                                    {
                                        tdPayeeName.InnerText = "NA";
                                        tdPayeeFatherName.InnerText = "NA";
                                        tdPayeeMotherName.InnerText = "NA";
                                        tdPayeeDOB.InnerText = "NA";
                                        tdPaymentDescription.InnerText = "NA";
                                    }
                                }
                            }

                            d1.Visible = true;
                            ddlServices.Enabled = false;
                            txtno.Enabled = false;
                            //upResult.Update();
                        }
                        else
                            tdError.InnerText = "This demand note has already been paid through CSC SPV payment option.";
                    }
                    else
                    {
                        tdError.InnerText = "This demand note can not be paid through CSC SPV. Reason: Payment Option selected for this demand not is " + demandNote.enmPaymentMode.ToString();
                    }
                }
                else
                    tdError.InnerText = "Demand note number not found / Invalid demand note number.";

            };
        }
        catch (Exception ex)
        {
            tdError.InnerText = ex.Message;
        }
    }
    protected void ddlexam_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlexam.SelectedValue != "0")
        {
            trReset.Visible = true;
            BtnSubmit.Visible = true;
            BtnCancel.Visible = true;
        }
        else
        {
            trReset.Visible = false;
        }
    }
    protected void BtnBack_Click(object sender, EventArgs e)
    {
        txtno.Text = "";
        //diverror.Visible = false;
        ddlServices.Enabled = true;
        Response.Redirect("nieletpaymentservices.aspx");

    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["ServiceID"]))
        {
            Int32 serviceid = Convert.ToInt32(Request.QueryString["ServiceID"]);
            if (serviceid == 1 || serviceid == 2 || serviceid == 4)
            {
                Response.Redirect("nieletpaymentservices.aspx?Serviceid=" + Request.QueryString["ServiceID"].ToString());
            }
            else
            {
                Response.Redirect("nieletpaymentservices.aspx");
            }
        }
        else
        {
            Response.Redirect("nieletpaymentservices.aspx");
        }
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["ServiceID"]))
        {
            Int32 serviceid = Convert.ToInt32(Request.QueryString["ServiceID"]);
            if (serviceid == 1 || serviceid == 2 || serviceid == 4)
            {
                Response.Redirect("nieletpaymentservices.aspx?Serviceid=" + Request.QueryString["ServiceID"].ToString());
            }
            else
            {
                Response.Redirect("nieletpaymentservices.aspx");
            }
        }
        else
        {
            Response.Redirect("nieletpaymentservices.aspx");
        }
    }
    protected void btnResetDemadnNote_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["ServiceID"]))
        {
            Int32 serviceid = Convert.ToInt32(Request.QueryString["ServiceID"]);
            if (serviceid == 4)
            {
                txtno.Text = "";
                trExams.Visible = false;
                d1.Visible = false;
                ddlServices.Enabled = false;
                txtno.Enabled = true;
            }
            else
            {
                txtno.Text = "";
                trExams.Visible = false;
                d1.Visible = false;
                ddlServices.Enabled = true;
                txtno.Enabled = true;
            }
        }
        else
        {
            txtno.Text = "";
            trExams.Visible = false;
            d1.Visible = false;
            ddlServices.Enabled = true;
            txtno.Enabled = true;
        }
    }
    protected void BtnPay_Click(object sender, EventArgs e)
    {
        try 
        {
            if (txtno.Text.Trim() == "")
                throw new Exception("Please enter demand note number");
             if (TxtCscId.Text.Trim() == "")
                throw new Exception("Please enter CSC / VLE ID");
            Int64 CSCID = Convert.ToInt64(TxtCscId.Text);
            Int64 demantNoteID = Convert.ToInt64(txtno.Text);
            using (EConnectContext context = new EConnectContext())
            {
                DemandNote demandNote = context.DemandNotes.Find(demantNoteID);
                if (demandNote != null)
                {
                    if (demandNote.enmPaymentMode == enmPaymentMode.CSCSPV)
                    {
                        if (demandNote.enmPaymentStatus == enmPaymentStatus.Pending)
                        {
                            //ScriptManager.RegisterClientScriptBlock(this,this.GetType(), "op", "window.top.location.href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("cscPayment.aspx?DID=" + demandNote.ID.ToString()) + "'", true);
                            Response.Redirect("cscPayment.aspx?DID=" + demantNoteID.ToString() + "&CSCID=" + CSCID.ToString(), true);
                        }
                        else
                            tdError.InnerText = "This demand note has already been paid through CSC SPV payment option.";
                    }
                    else
                    {
                        tdError.InnerText = "This demand note can not be paid through CSC SPV. Reason: Payment Option selected for this demand not is " + demandNote.enmPaymentMode.ToString();
                    }
                }
                else
                    tdError.InnerText = "Demand note number not found / Invalid demand note number.";
            };
        }
        catch (Exception ex)
        {
            tdError.InnerText = ex.Message;
        }
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        enmApplicationType selectdService = (enmApplicationType)Convert.ToInt32(ddlServices.SelectedValue);
        Int32 cid = Convert.ToInt32(ddlexam.SelectedValue);
        using (EConnectContext context = new EConnectContext())
        {
            Course currentcourse = context.Courses.Find(cid);
            if (selectdService == enmApplicationType.CertificateExamApplication)
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("CAND/CertificateRegistration.aspx?id=" + cid + "&candtype=External&Src=CSC&RU=../nieletpaymentservices.aspx?" + Request.QueryString.ToString()), true);
            }

            if (selectdService == enmApplicationType.CourseRegistrationApplication)
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("CAND/NielitRegistration.aspx?id=" + cid + "&Src=CSC&RU=../nieletpaymentservices.aspx?" + Request.QueryString.ToString()), true);
            }
        };
    }
}