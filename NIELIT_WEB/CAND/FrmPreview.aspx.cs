using System;
using System.Data.Objects;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

public partial class FrmPreview : BasePage
{
    UserType loginUserType;
    String status = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            if (Request.UrlReferrer == null && Request.QueryString["Appid"] == null)
            {
                // Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
                //string urldecoded=HttpUtility.HtmlDecode (GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
                //Response.Write(urldecoded);

                Response.End();
                return;
            }
            if (!Page.IsPostBack)
            {
                showdata();
            }
        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }
    protected Int32 GetExamID(Int32 ApplicantTypeId, Int32 courseID)
    {
        try
        {
            Int32 NormalactivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
            Int32 LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee);
            using (EConnectContext context = new EConnectContext())
            {
                Int32 examID = 0;

                var LateFeeExam = (from e in context.CutOffDates
                                   join i in context.Exams on e.ExamID equals i.ID
                                   where e.CourseID == courseID
                                   && e.ApplicantTypeID == ApplicantTypeId
                                   && e.ActivityID == LateFeeActivityId
                                   && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                   orderby e.EfferctiveDate ascending
                                   select new { ExamID = e.ExamID }).Take(1);

                if (LateFeeExam.Count() > 0)//If  applicable for late fee ?
                {
                    if (LateFeeExam != null)
                    { examID = LateFeeExam.FirstOrDefault().ExamID; }
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
                                         select new { ExamID = e.ExamID }).Take(1);
                    if (NormalFeeExam.Count() > 0)
                    { examID = NormalFeeExam.FirstOrDefault().ExamID; }
                }
                return examID;
            };
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
                    int LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee);
                    int NormalFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
                    int LateFeeTypeId = Convert.ToInt32(enmFeeType.LateFeeRegistration);
                    var Fee = (from f in context.CutOffDates
                               where f.CourseID == courseID
                               && f.ExamID == examID
                               && f.ApplicantTypeID == applicantTypeID
                               select new { EffectiveDate = f.EfferctiveDate, f.ActivityID }).ToList();

                    var NormalFee = Fee.Where(l => l.ActivityID == NormalFeeActivityId);
                    var lateFee = Fee.Where(l => l.ActivityID == LateFeeActivityId);

                    if (lateFee.Count() > 0 && lateFee != null)
                    {
                        if (NormalFee.Count() > 0)
                        {
                            if (NormalFee.FirstOrDefault().EffectiveDate <= DateTime.Now)
                            {
                                DateTime LastExamDate = (from f in context.FeeDetails
                                                         where f.CourseID == courseID
                                                         && f.FeeTypeID == LateFeeTypeId
                                                         && f.EffectiveFromDate == (from c in context.FeeDetails
                                                                                    where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                                                                                    select c.EffectiveFromDate).Max()
                                                         select new { LastExamDate = f.EffectiveFromDate }).FirstOrDefault().LastExamDate;
                                if (LastExamDate != null)
                                {
                                    Lastdate = LastExamDate.ToString("dd-MMM-yyyy");
                                }
                            }
                        }
                    }
                    else
                        if (NormalFee.Count() > 0)
                    {
                        Lastdate = NormalFee.FirstOrDefault().EffectiveDate.ToString("dd-MMM-yyyy");
                    }
                }
                ;

            }
            return Lastdate;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected Int64 GetFeeAmount(Int32 examID, Int32 applicantTypeID, Int32 courseID)
    {
        try
        {
            Int64 feeAmt = 0; Int32 FeeTypeID = 0;

            using (EConnectContext context = new EConnectContext())
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Status"]) && Request.QueryString["Status"] == "R") // Re-Registration 
                { FeeTypeID = Convert.ToInt32(enmFeeType.ReRegistrationFee); }
                else
                {
                    //FeeTypeID = Convert.ToInt32(enmFeeType.RegistrationFee); 
                    string appId = Request.QueryString["Appid"].ToString();


                    // added by amit start
                    bool reregistrationcase = false;
                    var reregistration = (from a in context.CourseRegistrationApplications
                                          where a.ID.ToString() == appId
                                          select new
                                          {
                                              registrationtypeid = a.RegistrationTypeID

                                          }).FirstOrDefault();


                    if (reregistration != null)
                    {
                        // condition to check for re - registration case
                        //if (reregistration.registrationtypeid == Convert.ToInt32(enmRegistrationType.ReRegistration))
                        if (reregistration.registrationtypeid == Convert.ToInt32(enmFeeType.ReRegistrationFee)) 
                       {
                            FeeTypeID = Convert.ToInt32(enmFeeType.ReRegistrationFee);
                            reregistrationcase = true;

                       }
                    }

                    // added by amit end

                    if (!reregistrationcase)  // false 
                    {
                        // old logic 
                        var FeeType = (from a in context.CourseRegistrationApplications
                                       where a.ID.ToString() == appId
                                       select new
                                       {
                                           feetype = a.FeeTypeID

                                       }).FirstOrDefault();

                        if (FeeType != null)
                            FeeTypeID = Convert.ToInt32(FeeType.feetype);
                        //old logic
                    }
                   
                }

                if (FeeTypeID == 0)
                    FeeTypeID = 1;


                // added By  amit Start

                Int64 feeAmount = (from r in context.FeeDetails
                                   where r.CourseID == courseID//1
                                      && r.FeeTypeID == FeeTypeID//3
                                      && r.EffectiveFromDate <= System.Data.Entity.DbFunctions.CreateDateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
                                      && (r.EffectiveToDate == null || r.EffectiveToDate >= System.Data.Entity.DbFunctions.CreateDateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0))
                                      && r.projectid == null
                                   orderby r.EffectiveFromDate descending
                                   select r.FeeAmount).FirstOrDefault();

                // Added by  Amit End

                feeAmt = feeAmount;

                Int32 LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee);
                Int32 NormalFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
                Int32 LateFeeTypeId = Convert.ToInt32(enmFeeType.LateFeeRegistration);

                var Fee = (from f in context.CutOffDates
                           where f.CourseID == courseID && f.ExamID == examID
                           select new { EffectiveDate = f.EfferctiveDate, f.ActivityID, f.ApplicantTypeID }).ToList();

                var NormalFee = Fee.Where(l => l.ActivityID == NormalFeeActivityId);
                var lateFee = Fee.Where(l => l.ActivityID == LateFeeActivityId && l.ApplicantTypeID == applicantTypeID);

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
                        if (LatefeeAmount != 0)
                        {
                            if (LatefeeAmount > 0)
                            {
                                feeAmt = LatefeeAmount;
                            }
                        }
                    }
                }
            }
            ;
            return feeAmt;
        }
        catch (Exception ex) { throw ex; }
    }
    //Added_UP_Project_For_New_Fee_Calculation_12_02_2025_Start Amit added
    protected Int64 GetFeeAmount(Int32 examID, Int32 applicantTypeID, Int32 courseID, Int32? projectId)
    {
        try
        {
            Int64 feeAmt = 0; Int32 FeeTypeID = 0;

            using (EConnectContext context = new EConnectContext())
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Status"]) && Request.QueryString["Status"] == "R") // Re-Registration 
                { 
                    
                    FeeTypeID = Convert.ToInt32(enmFeeType.ReRegistrationFee); 
                
                }
                else
                {
                    //FeeTypeID = Convert.ToInt32(enmFeeType.RegistrationFee); 
                    //Modified on 11 jan 2022
                    string appId = Request.QueryString["Appid"].ToString();



                    var FeeType = (from a in context.CourseRegistrationApplications
                                   where a.ID.ToString() == appId
                                   select new
                                   {
                                       feetype = a.FeeTypeID

                                   }).FirstOrDefault();

           

                    if (FeeType != null)
                        FeeTypeID = Convert.ToInt32(FeeType.feetype);
                }

                if (FeeTypeID == 0)
                    FeeTypeID = 1;

                var feeAmount = (from f in context.FeeDetails
                                 where f.CourseID == courseID && f.FeeTypeID == FeeTypeID && f.projectid == projectId
                                 && (f.EffectiveToDate == null || f.EffectiveToDate >= System.Data.Entity.DbFunctions.CreateDateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0))
                                 && f.EffectiveFromDate == (from c in context.FeeDetails
                                                            where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                                                            && c.projectid == projectId
                                                            select c.EffectiveFromDate).Max()
                                 select new { FeeAmount = f.FeeAmount }).FirstOrDefault();
                //                feeAmt = feeAmount;
                if (feeAmount == null)
                {
                    feeAmt = GetFeeAmount(examID, applicantTypeID, courseID);
                }
                else
                {
                    feeAmt = Convert.ToInt64(feeAmount.FeeAmount);

                    Int32 LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee);
                    Int32 NormalFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
                    Int32 LateFeeTypeId = Convert.ToInt32(enmFeeType.LateFeeRegistration);

                    var Fee = (from f in context.CutOffDates
                               where f.CourseID == courseID && f.ExamID == examID
                               select new { EffectiveDate = f.EfferctiveDate, f.ActivityID, f.ApplicantTypeID }).ToList();

                    var NormalFee = Fee.Where(l => l.ActivityID == NormalFeeActivityId);
                    var lateFee = Fee.Where(l => l.ActivityID == LateFeeActivityId && l.ApplicantTypeID == applicantTypeID);

                    if (lateFee.Count() > 0 && lateFee != null)
                    {
                        if (NormalFee.FirstOrDefault().EffectiveDate <= DateTime.Now)
                        {
                            Int64 LatefeeAmount = (from f in context.FeeDetails
                                                   where f.CourseID == courseID && f.FeeTypeID == LateFeeTypeId && f.projectid == projectId &&
                                                       f.EffectiveFromDate == (from c in context.FeeDetails
                                                                               where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                                                                               select c.EffectiveFromDate).Max()
                                                   select new { FeeAmount = f.FeeAmount }).FirstOrDefault().FeeAmount;
                            if (LatefeeAmount != 0)
                            {
                                if (LatefeeAmount > 0)
                                {
                                    feeAmt = LatefeeAmount;
                                }
                            }
                        }
                    }
                }
            }
            ;
            return feeAmt;
        }
        catch (Exception ex) { throw ex; }

    }
    protected void showdata()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                Int64 appID = Convert.ToInt64(Request.QueryString["Appid"]);
                //ShowAlert(appID.ToString());
                var application = context.CourseRegistrationApplications.Find(appID);
                Int32 examID = application.ApplicableExamID.HasValue ? application.ApplicableExamID.Value : 0;
                Int32 ShortermCoursesCategory = context.CourseCategories.Where(s => s.Code.ToUpper() == "STC").FirstOrDefault().ID;

                LblCourse.Text = application.Course.Name;
                //configuring declaration for STC{
                if (application.CourseCategoryID == ShortermCoursesCategory)
                {
                    tddeclaration1.Visible = false;
                    tddeclaration2.Visible = true;
                    lbldeccoursecode.Text = GetInitCap(application.Course.Name);
                    lbldeccoursecode1.Text = GetInitCap(application.Course.Name);
                    lblname.Text = GetInitCap(application.Name);
                    trnote.Visible = true;
                }
                else
                {
                    tddeclaration1.Visible = true;
                    tddeclaration2.Visible = false;
                    trnote.Visible = false;
                }

               //aadhaar_enc_mapping_april_2025_start

              /*  if (application.AadharNumber.HasValue)
                {
                    // added_by_amit_audit_april_2026_start
                    string lastfour =  application.AadharNumber.Value.ToString().Substring(8);

                    lblaadhar.Text = new string('X', 8) + lastfour;
                     // added_by_amit_audit_april_2026_end
                }
              */



                if (application.aencID.HasValue)
                {
                    var aadhar_enc_table = context.CourseENCAaddhar.Find(Convert.ToInt64(application.aencID.Value));
                    string decryped_aadhar = EncryptDecrypt.DecryptString(aadhar_enc_table.encAddh);
                    string lastfour = decryped_aadhar.Substring(8);
                    lblaadhar.Text = new string('X', 8) + lastfour;
                }
                else
                   lblaadhar.Text = "N/A";

                //aadhaar_enc_mapping_april_2025_end

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
                    Btnback.Visible = false;
                    BtnPrint.Visible = true;
                    NormalHeader1.Visible = true;
                    TrPaymentDetail.Visible = true;
                    LblCourse.Text = application.Course.Name + "  (" + application.ApplicableExam.Name + ")";
                    if (application.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct) || (application.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && application.enmPaymentSource == enmPaymentSource.Candidate))
                    {
                        TrDemandnote.Visible = true;
                        TrInstitute.Visible = false;
                        TrDemandnote1.Visible = true;
                        LblDemandNoteID.Text = application.DemandNote.ID.ToString();
                        LblDemandNoteDate.Text = application.DemandNote.ApplicationDate.ToString("dd-MMM-yyyy");
                        TrPayment_not.Visible = true;
                        TrPaymentMode.Visible = true;
                        TrPaymentModeInstruction.Visible = true;
                        TrPaymentModeTransactionNo.Visible = true;

                        #region Payment Option - CSC/SPV
                        if (application.DemandNote.PaymentModeID == Convert.ToInt32(enmPaymentMode.CSCSPV))
                        {

                            LblPaymentMode.Text = enmPaymentMode.CSCSPV.ToString();
                            LblFeeType.Text = "Registration Fee (in Rupees)";
                            // Added by  Amit Start
                            Int64 feeAmount = 0;
                            if (application.projectID == null)
                                feeAmount = GetFeeAmount(examID, application.ApplicantTypeID, application.CourseID);
                            else if (application.projectID != null)
                                feeAmount = GetFeeAmount(examID, application.ApplicantTypeID, application.CourseID, Convert.ToInt32(application.projectID));

                            // Added by  Amit End
                            LblFeeAmount.Text = feeAmount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeAmount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                            LblPaymentSrc.Text = "by CSC / E-Mitra Center./ सीएससी /ई - मित्र केंद्र से";
                            LblPaymentDescription.Text = "Last date of payment is " + GetLastExamDate(examID, application.ApplicantTypeID, application.CourseID) + " . Please go to nearest CSC / E-Mitra Center and make your Payment / कृपया अपने निकटतम सीएससी / ई - मित्र केंद्र पर जाए और अपना भुगतान करें| ";

                            if (application.DemandNote != null)
                            {
                                if (application.DemandNote.enmPaymentStatus == enmPaymentStatus.Paid || application.DemandNote.enmPaymentStatus == enmPaymentStatus.PaidButNotVerified)
                                    lblTransactionNumber.Text = application.DemandNote.CSCTransaction.ResponseTransactionNumber.ToString();
                            }
                            if (application.BatchItemID != null)
                            {
                                BatchItem batchitem = context.BatchItems.Find(application.BatchItemID);
                                if (batchitem != null)
                                {
                                    lblBatchNo.Text = batchitem.Batch.Number;
                                }
                            }
                            var registrationNo = context.RegistrationDetails.Where(s => s.CourseRegistrationApplicationID == application.ID).OrderByDescending(s => s.RegistrationDate).FirstOrDefault();
                            if (registrationNo != null)
                            { lblRegistrationNo.Text = registrationNo.RegistrationNo.ToString(); }
                        }
                        #endregion

                        #region Payment Option - Demand Draft
                        else if (application.DemandNote.PaymentModeID == Convert.ToInt32(enmPaymentMode.DemandDraft))
                        {
                            LblPaymentMode.Text = enmPaymentMode.DemandDraft.ToString();
                            LblFeeType.Text = "Registration Fee (in Rupees)";
                            // Added by  Amit Start
                            Int64 feeAmount = 0;
                            if (application.projectID == null)
                                feeAmount = GetFeeAmount(examID, application.ApplicantTypeID, application.CourseID);
                            else if (application.projectID != null) 
                                feeAmount = GetFeeAmount(examID, application.ApplicantTypeID, application.CourseID, Convert.ToInt32(application.projectID));
                            // Added by  Amit End
                            LblFeeAmount.Text = feeAmount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeAmount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                            LblPaymentSrc.Text = "after making payment through Demand Draft";
                            LblPaymentDescription.Text = "Last date of payment is " + GetLastExamDate(examID, application.ApplicantTypeID, application.CourseID);

                            if (application.DemandNote != null)
                            {
                                if (application.DemandNote.enmPaymentStatus == enmPaymentStatus.Paid || application.DemandNote.enmPaymentStatus == enmPaymentStatus.PaidButNotVerified)
                                    lblTransactionNumber.Text = application.DemandNote.DemandDraftTransaction.DemandDraftNumber.ToString();
                            }
                            if (application.BatchItemID != null)
                            {
                                BatchItem batchitem = context.BatchItems.Find(application.BatchItemID);
                                if (batchitem != null)
                                {
                                    lblBatchNo.Text = batchitem.Batch.Number;
                                }
                            }
                            var registrationNo = context.RegistrationDetails.Where(s => s.CourseRegistrationApplicationID == application.ID).OrderByDescending(s => s.RegistrationDate).FirstOrDefault();
                            if (registrationNo != null)
                            { lblRegistrationNo.Text = registrationNo.RegistrationNo.ToString(); }
                        }
                        #endregion

                        #region Payment Option - Online
                        else if (application.DemandNote.PaymentModeID == Convert.ToInt32(enmPaymentMode.Online))
                        {
                            // Added by  Amit Start
                            Int64 feeAmount = 0;
                            if (application.projectID == null)
                                feeAmount = GetFeeAmount(examID, application.ApplicantTypeID, application.CourseID);
                            else if (application.projectID != null)
                                feeAmount = GetFeeAmount(examID, application.ApplicantTypeID, application.CourseID, Convert.ToInt32(application.projectID));

                            // Added by  Amit End
                            LblPaymentMode.Text = enmPaymentMode.Online.ToString();
                            LblFeeType.Text = "Registration Fee (in Rupees)";
                            LblFeeAmount.Text = feeAmount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeAmount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                            LblPaymentSrc.Text = "after making Online Payment. / ऑनलाइन से";
                            LblPaymentDescription.Text = "Last date of payment is " + GetLastExamDate(examID, application.ApplicantTypeID, application.CourseID) + " .Please make a Online Payment / कृपया ऑनलाइन भुगतान करें|";

                            if (application.DemandNote != null)
                            {
                                if (application.DemandNote.enmPaymentStatus == enmPaymentStatus.Paid || application.DemandNote.enmPaymentStatus == enmPaymentStatus.PaidButNotVerified)
                                    lblTransactionNumber.Text = application.DemandNote.OnlineTransaction.ReferenceNumber.ToString();
                            }
                            if (application.BatchItemID != null)
                            {
                                BatchItem batchitem = context.BatchItems.Find(application.BatchItemID);
                                if (batchitem != null)
                                {
                                    lblBatchNo.Text = batchitem.Batch.Number;
                                }
                            }
                            var registrationNo = context.RegistrationDetails.Where(s => s.CourseRegistrationApplicationID == application.ID).OrderByDescending(s => s.RegistrationDate).FirstOrDefault();
                            if (registrationNo != null)
                            { lblRegistrationNo.Text = registrationNo.RegistrationNo.ToString(); }
                        }
                        #endregion

                        #region Payment Option - NEFT/RTGS
                        else if (application.DemandNote.PaymentModeID == Convert.ToInt32(enmPaymentMode.NEFTRTGS))
                        {
                            // Added by  Amit Start
                            Int64 feeAmount = 0;
                            // Added by  Amit End
                            LblPaymentMode.Text = enmPaymentMode.NEFTRTGS.ToString();
                            LblFeeType.Text = "Registration Fee (in Rupees)";
                            // Added by  Amit Start
                            if (application.projectID == null)
                                feeAmount = GetFeeAmount(examID, application.ApplicantTypeID, application.CourseID);
                            else if (application.projectID != null)
                                feeAmount = GetFeeAmount(examID, application.ApplicantTypeID, application.CourseID, Convert.ToInt32(application.projectID));
                            // Added by  Amit End
                            LblFeeAmount.Text = feeAmount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeAmount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                            LblPaymentSrc.Text = "after making payment through NEFT/ RTGS. / एनईएफटी / आरटीजीएस से";
                            LblPaymentDescription.Text = "Last date of payment is " + GetLastExamDate(examID, application.ApplicantTypeID, application.CourseID) + " .Please make a  NEFT/ RTGS Payment / कृपया एनईएफटी / आरटीजीएस से भुगतान करें|";

                            if (application.DemandNote != null)
                            {
                                if (application.DemandNote.enmPaymentStatus == enmPaymentStatus.Paid || application.DemandNote.enmPaymentStatus == enmPaymentStatus.PaidButNotVerified)
                                    lblTransactionNumber.Text = application.DemandNote.NEFTTransaction.TransactionNumber.ToString();
                            }
                            if (application.BatchItemID != null)
                            {
                                BatchItem batchitem = context.BatchItems.Find(application.BatchItemID);
                                if (batchitem != null)
                                {
                                    lblBatchNo.Text = batchitem.Batch.Number;
                                }
                            }
                            var registrationNo = context.RegistrationDetails.Where(s => s.CourseRegistrationApplicationID == application.ID).OrderByDescending(s => s.RegistrationDate).FirstOrDefault();
                            if (registrationNo != null)
                            { lblRegistrationNo.Text = registrationNo.RegistrationNo.ToString(); }
                        }
                        #endregion
                    }
                    else
                    {
                        TrDemandnote.Visible = false;
                        TrDemandnote1.Visible = false;
                        TrPaymentMode.Visible = false;
                        TrPaymentModeInstruction.Visible = false;
                        TrPaymentModeTransactionNo.Visible = false;
                        TrInstitute.Visible = true;

                        var institute = application.Institute;
                        TdInstituteInfo.InnerHtml = " <strong>Note</strong> * Please  deposit  fee / submit (as applicable) for registration form at the following Address till last date / अंतिम तारीख तक कृपया निम्न पते पर शुल्क / पंजीकरण फार्म (जो लागू हो) जमा करें:- Accredited Centre Name:- " + GetInitCap(institute.Name) + " , " + institute.AddressLine1 + "  ,  " + institute.AddressLine2 + institute.AddressLine3 +
                         "  " + institute.CityName + " ( " + institute.State.Name + " )  " + "<br>Pin Code / (पिन कोड): " + (institute.PinCode.HasValue ? institute.PinCode.Value.ToString() : "NA") + ", Phone Number / (फोन नंबर) : ";
                        //if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                        //{
                        //    TdInstituteInfo.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber1.Value;
                        //    if (institute.PhoneNumber2.HasValue)
                        //        TdInstituteInfo.InnerHtml += ", " + institute.PhoneNumber2.Value;
                        //}
                        //else
                        //    TdInstituteInfo.InnerHtml += "NA";
                        //deep add code on 8 aug 2018 and comment above line of code
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

                        //deep end add code on 8 aug 2018
                        if (application.BatchItemID != null)
                        {
                            BatchItem batchitem = context.BatchItems.Find(application.BatchItemID);
                            if (batchitem != null)
                            {
                                lblBatchNo.Text = batchitem.Batch.Number;
                            }
                        }

                        //RegistrationDetail registration = context.RegistrationDetails.Where(s => s.CourseRegistrationApplicationID == application.ID).OrderByDescending(s => s.RegistrationDate).FirstOrDefault();
                        //if (registration != null)
                        //{
                        //    lblRegistrationNo.Text = registration.RegistrationNo.ToString();
                        //}
                        var registrationNo = context.RegistrationDetails.Where(s => s.CourseRegistrationApplicationID == application.ID).OrderByDescending(s => s.RegistrationDate).FirstOrDefault();
                        if (registrationNo != null)
                        { lblRegistrationNo.Text = registrationNo.RegistrationNo.ToString(); }
                    }
                }
                else
                {
                    Lblerror.Text = "Dear Candidate, this is preview of registration form you are applying for. Please check all details in preview form and click on 'Final Submit' button if all the details are correct and you are satisfied with details shown in preview form. If you want to change the details click on 'Back' button to go back to registration form.<br> Once you click on 'Final Submit button, you can not modify the details and you will be asked for payment in next page(if payment will be paid to NIELIT).";
                    Lblerror.Visible = true;
                    NormalHeader1.Visible = false;
                    TrDemandnote.Visible = false;
                    TrDemandnote1.Visible = false;
                    Btnsubmit.Visible = true;
                    Btnback.Visible = true;
                    BtnPrint.Visible = false;
                    TrPaymentDetail.Visible = false;
                    TrPaymentMode.Visible = false;
                    TrPaymentModeInstruction.Visible = false;
                    TrPaymentModeTransactionNo.Visible = false;
                    TrPayment_not.Visible = false;
                }

                LblAppNumber.Text = application.Number.ToString();
                LblAppDataTime.Text = application.ApplicationDate.ToString("dd-MMM-yyyy hh:mm:ss tt");
                appid.Value = application.ID.ToString();
                courseid.Value = application.Course.ID.ToString();
                Lblhead.Text = "Online Registration Application Form:Software" + " " + application.Course.Name + "(" + application.Course.Code + ")";

                if (application.AlreadyRegistered.ToString() == "True")
                {
                    Lblprecourse.Visible = true;
                    LblPreDoeaccCourse.Visible = true;
                    TrPreRegLevel1.Visible = true;
                    TdPreRegLevel2.RowSpan = 2;
                    Tdpreregister.RowSpan = 2;
                    LblPreDoeaccCourse.Text = application.RegisteredCourse.Name;
                    LblPreRegno.Text = Convert.ToString(application.RegisteredCourseRegistrationNo);
                    Lblpreregister.Text = "Yes";
                }
                else
                {
                    Lblpreregister.Text = "No";
                    Lblprecourse.Visible = false;
                    LblPreDoeaccCourse.Visible = false;
                    TrPreRegLevel1.Visible = false;
                    TdPreRegLevel2.RowSpan = 1;
                    Tdpreregister.RowSpan = 1;
                }

                if (application.enmApplicantType == enmApplicantType.Institute)
                {
                    var instituteDetail = (from a in context.AccreditationDetails
                                           join i in context.Institutes on a.InstituteID equals i.ID
                                           where i.ID == application.InstituteID && a.CourseID == application.CourseID
                                           select new
                                           {
                                               Name = a.AccreditationNumber + "-" + i.Name + ", " + i.CityName,
                                               AccNo = a.AccreditationNumber
                                           }).FirstOrDefault();

                    Lblundergoing.Text = "Institute";
                    Tdundergoing.RowSpan = 2;
                    Tdpreundergoing.RowSpan = 2;
                    LblAcc.Visible = true;
                    LblAccNo.Visible = true;
                    LblExperience.Visible = false;
                    LblExp.Visible = false;
                    TrLastCenterInstiName.Visible = true;
                    LblAccNo.Text = instituteDetail.AccNo;
                    LblInstitute.Text = instituteDetail.Name;
                    //Added By  Amit Start
                    if (application.projectID != null && application.projectID != 0)
                    {
                        //Added_UP_Project_30_12_2024_Start

                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ToString()))
                        {

                            SqlCommand scCommand = new SqlCommand("select projectName from NielitProjects where id=@projID", new SqlConnection(con.ConnectionString));

                            scCommand.Parameters.AddWithValue("@projID", application.projectID.ToString());

                            if (scCommand.Connection.State == ConnectionState.Closed)
                            {
                                scCommand.Connection.Open();
                            }
                            lblProjectId.Visible = true;
                            lblProjectIdContent.Visible = true;
                            lblProjectIdContent.Text = string.IsNullOrWhiteSpace(scCommand.ExecuteScalar().ToString()) ? " NA " : scCommand.ExecuteScalar().ToString();

                        }

                        //if(lblProjectIdContent.Text.Contains("UPMSP"))
                        //{
                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ToString()))
                        {

                            SqlCommand scCommand1 = new SqlCommand("SELECT [ID],[field1],[field2],[field3],[field4],[field5]  FROM [projCriteriaMaster] where projID=@projID", new SqlConnection(conn.ConnectionString));
                            scCommand1.Parameters.AddWithValue("@projID", application.projectID.ToString());

                            if (scCommand1.Connection.State == ConnectionState.Closed)
                            {
                                scCommand1.Connection.Open();
                            }

                            DataTable dt = new DataTable();
                            SqlDataAdapter da = new SqlDataAdapter(scCommand1);
                            //DataSet ds = new DataSet();
                            da.Fill(dt);

                            foreach (DataRow row in dt.Rows)
                            {
                                if (row["field1"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field1"].ToString()))
                                {
                                    lblField1.Visible = true;
                                    lblField1.Text = row["field1"].ToString();
                                    lblField1Content.Visible = true;
                                    lblField1Content.Text = string.IsNullOrWhiteSpace(application.field1.ToString()) ? " NA " : application.field1.ToString();

                                }
                                else
                                {
                                    lblField1.Visible = false;
                                    lblField1.Text = "Field 1";
                                    lblField1Content.Visible = false;
                                    lblField1Content.Text = "";
                                }
                                if (row["field2"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field2"].ToString()))
                                {

                                    lblField2.Visible = true;
                                    lblField2.Text = row["field2"].ToString();
                                    lblField2Content.Visible = true;
                                    lblField2Content.Text = string.IsNullOrWhiteSpace(application.field2.ToString()) ? " NA " : application.field2.ToString();
                                }
                                else
                                {
                                    lblField2.Visible = false;
                                    lblField2.Text = "Field 2";
                                    lblField2Content.Visible = false;
                                    lblField2Content.Text = "";
                                }
                                if (row["field3"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field3"].ToString()))
                                {
                                    lblField3.Visible = true;
                                    lblField3.Text = row["field3"].ToString();
                                    lblField3Content.Visible = true;
                                    lblField3Content.Text = string.IsNullOrWhiteSpace(application.field3.ToString()) ? " NA " : application.field3.ToString();
                                }
                                else
                                {
                                    lblField3.Visible = false;
                                    lblField3.Text = "Field 3";
                                    lblField3Content.Visible = false;
                                    lblField3Content.Text = "";
                                }
                                if (row["field4"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field4"].ToString()))
                                {
                                    lblField4.Visible = true;
                                    lblField4.Text = row["field4"].ToString();
                                    lblField4Content.Visible = true;
                                    lblField4Content.Text = string.IsNullOrWhiteSpace(application.field4.ToString()) ? " NA " : application.field4.ToString();
                                }
                                else
                                {
                                    lblField4.Visible = false;
                                    lblField4.Text = "Field 4";
                                    lblField4Content.Visible = false;
                                    lblField4Content.Text = "";
                                }
                                if (row["field5"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field5"].ToString()))
                                {
                                    lblField5.Visible = true;
                                    lblField5.Text = row["field5"].ToString();
                                    lblField5Content.Visible = true;
                                    lblField5Content.Text = string.IsNullOrWhiteSpace(application.field5.ToString()) ? " NA " : application.field5.ToString();
                                }
                                else
                                {
                                    lblField5.Visible = false;
                                    lblField5.Text = "Field 5";
                                    lblField5Content.Visible = false;
                                    lblField5Content.Text = "";
                                }
                            }


                            if (!lblProjectId.Visible && !lblField1.Visible)
                                TrProject.Visible = false;

                            if (!lblField2.Visible && !lblField3.Visible)
                                TrField23.Visible = false;

                            if (!lblField4.Visible && !lblField5.Visible)
                                TrField45.Visible = false;
                        }
                        //}
                        //Added_UP_Project__30_12_2024_End  Amit Added

                    }
                    else
                    {
                        lblProjectId.Visible = false;
                        lblProjectIdContent.Visible = false;

                        TrProject.Visible = false;
                        TrField23.Visible = false;
                        TrField45.Visible = false;
                    }
                    //Added By  Amit End
                }
                else
                {
                    //Added for BSB
                    if (application.isBSB)
                    {
                        Lblundergoing.Text = "Direct(BSB)" + "<br/>School Code:" + application.BSB_U_DISECode.ToString();
                    }
                    else
                    {
                        Lblundergoing.Text = "Direct";
                    }

                    // Lblundergoing.Text = "Direct";
                    Tdundergoing.RowSpan = 1;
                    Tdpreundergoing.RowSpan = 1;
                    LblAcc.Visible = false;
                    LblAccNo.Visible = false;
                    LblExperience.Text = Convert.ToString(application.ExperienceInYears.Value);
                    TrLastCenterInstiName.Visible = false;
                }

                if (application.AlreadyQualified.ToString() == "True")
                {
                    Lblqualified.Text = "Yes";
                    Tdqualified.RowSpan = 3;
                    Tdlblqualified.RowSpan = 3;
                    Lbldcourse.Visible = true;
                    LblDoeaccCourse.Visible = true;
                    Trregno.Visible = true;
                    Tryp.Visible = true;
                    LblDoeaccCourse.Text = application.QualifiedCourse.Name;
                    LblDoeaccRegno.Text = application.QualifiedCourseRegistrationNo.ToString();
                    LblDoeaccYopass.Text = application.QualifiedCoursePassingYear.ToString();
                }
                else
                { 
                    Lblqualified.Text = "No";
                    Tdqualified.RowSpan = 1;
                    Tdlblqualified.RowSpan = 1;
                    Lbldcourse.Visible = false;
                    LblDoeaccCourse.Visible = false;
                    Trregno.Visible = false;
                    Tryp.Visible = false;
                }

                imgPhotoBarcode.Src = "../Handlers/BarcodeHandler.ashx?Code=" + application.Number.ToString();

                LblAppName.Text = application.Salutation + " " + GetInitCap(application.Name);
                if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName))
                {
                    TrFatherName.Visible = true;
                    TrMotherName.Visible = true;
                    TrGardianName.Visible = false;
                    //Added 17 Mar 2019
                    TrAffidavit.Visible = false;
                    //
                    LblFName.Text = string.IsNullOrEmpty(application.FatherName) == false && !string.IsNullOrWhiteSpace(application.FatherName) ? "Mr. " + GetInitCap(application.FatherName) : "";
                    LblMName.Text = string.IsNullOrEmpty(application.MotherName) == false && !string.IsNullOrWhiteSpace(application.MotherName) ? "Mrs. " + GetInitCap(application.MotherName) : "";
                }
                else
                {
                    TrFatherName.Visible = false;
                    TrMotherName.Visible = false;
                    TrGardianName.Visible = true;
                    //Added 17 Mar 2019
                    TrAffidavit.Visible = true;
                    lblAffidavitNo.Text = application.affidavitNo;
                    if (application.affidavitDate != null)
                        lblAffidavitDate.Text = application.affidavitDate.Value.ToString("dd-MMM-yy");
                    //
                    LblGuardianName.Text = GetInitCap(application.GuardianName);
                }

                LblGender.Text = GetInitCap(application.Gender);
                LblMaritalStatus.Text = GetInitCap(application.MaritalStatus.Name);
                LblDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");
                LblCategory.Text = GetInitCap(application.CastCategory.Name) + " / " + GetInitCap(application.CastCategory.NameRegional);

                if (application.IsHandicaped.ToString() == "True")
                {
                    LblHandicapped.Text = "Yes";
                }
                else
                {
                    LblHandicapped.Text = "No";
                }
                if (application.IsExServicemane.ToString() == "True")
                {
                    LblExService.Text = "Yes";
                }
                else
                {
                    LblExService.Text = "No";
                }
                LblReligion.Text = GetInitCap(application.Religion.Name);
                LblBodyMark.Text = GetInitCap(application.BodyMark);

                #region Images
                ImgApplicantPhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])application.Photo);
                imgThumbImpression.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])application.LeftThumb);
                imgSignature.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])application.Signature);
                imgBarCode.Src = "../Handlers/BarcodeHandler.ashx?Code=" + application.Number.ToString();
                #endregion

                #region Contact
                LblMobile.Text = application.MobileNumber.ToString();
                LblLandLine.Text = string.IsNullOrEmpty(application.StdNumber.ToString()) == false && !string.IsNullOrWhiteSpace(application.PhoneNumber.ToString()) ? "0" + application.StdNumber.ToString() + "-" + application.PhoneNumber.ToString() : "";
                //   LblEmail.Text = application.EmailAddress.ToString(); 
                LblEmail.Text = CommonFunctions.ChangeEmailDisplay(application.EmailAddress.ToString());
                #endregion

                #region Permanant Address
                lblPerAddressLine1.Text = string.IsNullOrEmpty(application.PerAddressLine1) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine1) ? GetInitCap(application.PerAddressLine1) : "";
                lblPerAddressLine2.Text = string.IsNullOrEmpty(application.PerAddressLine2) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine2) ? GetInitCap(application.PerAddressLine2) : "";
                lblPerAddressLine3.Text = string.IsNullOrEmpty(application.PerAddressLine3) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine3) ? GetInitCap(application.PerAddressLine3) : "";
                lblPerCity.Text = string.IsNullOrEmpty(application.PerCityName) == false && !string.IsNullOrWhiteSpace(application.PerCityName) ? GetInitCap(application.PerCityName) : "";
                LblPerState.Text = application.PerStateID != 0 ? GetInitCap(application.PerState.Name) : "";
                LblPerDistrict.Text = application.PerDistrictID.HasValue && application.PerDistrictID != 0 ? GetInitCap(application.PerDistrict.Name) : "";
                LblPerPinCode.Text = application.PerPinCode != 0 ? GetInitCap(application.PerPinCode.ToString()) : "";
                #endregion

                #region Correspondence Address
                lblCorAddressLine1.Text = string.IsNullOrEmpty(application.CorAddressLine1) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine1) ? GetInitCap(application.CorAddressLine1) : "";
                lblCorAddressLine2.Text = string.IsNullOrEmpty(application.CorAddressLine2) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine2) ? GetInitCap(application.CorAddressLine2) : "";
                lblCorAddressLine3.Text = string.IsNullOrEmpty(application.CorAddressLine3) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine3) ? GetInitCap(application.CorAddressLine3) : "";
                lblCorCity.Text = string.IsNullOrEmpty(application.CorCityName) == false && !string.IsNullOrWhiteSpace(application.CorCityName) ? GetInitCap(application.CorCityName.ToUpper()) : "";
                LblState.Text = application.CorStateID != 0 ? GetInitCap(application.CorState.Name) : "";
                LblDistrict.Text = application.CorStateID != 0 ? GetInitCap(application.CorDistrict.Name) : "";
                LblPincode.Text = application.CorPinCode != 0 ? GetInitCap(application.CorPinCode.ToString()) : "";
                #endregion

                #region Educational Qualification
                LblHeighEducation.Text = GetInitCap(application.EducationalQualification.Name);
                LblHeighestEduYrofPass.Text = application.PassingYear.ToString();
                #endregion
            }
            ;
        }
        catch (Exception ex) {
            //throw ex;
            ShowAlert(ex.ToString());
        }
    }
    protected void Btnback_Click(object sender, EventArgs e)
    {
        if (Session["UserType"] != null)
        {
            loginUserType = (UserType)Session["UserType"];
            if (loginUserType == UserType.Candidate)
            {
                //when come from CurrentRegistrationStatus.aspx(candidate login) 
                if (!String.IsNullOrEmpty(Request.QueryString["Status"]))
                {
                    status = Request.QueryString["Status"];
                }
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("CurrentRegistrationStatus.aspx?id=" + courseid.Value + "&Appid=" + appid.Value + "&Status=" + status));
            }
        }
        else
        {
            if ((!String.IsNullOrEmpty(Request.QueryString["Appid"])) && (Request.QueryString["Src"] == "CSC"))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitRegistration.aspx?id=" + courseid.Value + "&Appid=" + appid.Value + "&Src=CSC"));
            }
            else
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitRegistration.aspx?id=" + courseid.Value + "&Appid=" + appid.Value));
            }
        }

    }
    protected void Btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 applID = Convert.ToInt64(Request.QueryString["Appid"]);
            int courseid = 0, ApplicationFlag = 0;
            Int64 demandID = 0, mobileNumber;
            string emailAddress = "", mobileMsg = "", msg = "";
            Boolean allowSendingEmail = false, allowSendingSms = false;

            LblAppNumber.Text = applID.ToString();
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 activitycutoffdateID = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);

                    var application = context.CourseRegistrationApplications.Find(applID);
                    var cuttoffdate = context.CutOffDates.Where(s => s.ExamID == application.ApplicableExamID && s.CourseID == application.CourseID && s.ApplicantTypeID == application.ApplicantTypeID && s.ActivityID == activitycutoffdateID).FirstOrDefault().EfferctiveDate;
                    Int32 examID = GetExamID(application.ApplicantTypeID, application.CourseID);
                    // Added by  Amit Start
                    Int64 feeAmount = 0;
                    if (application.projectID == null)
                        feeAmount = GetFeeAmount(examID, application.ApplicantTypeID, application.CourseID);
                    else if (application.projectID != null)
                        feeAmount = GetFeeAmount(examID, application.ApplicantTypeID, application.CourseID, Convert.ToInt32(application.projectID));
                    // Added by  Amit End
                    //Check CuttOffDate
                    if (DateTime.Now.Date > cuttoffdate.Date)
                    {
                        ShowAlert("Last date for Filling Registration Application Form is over.");
                        showdata();
                        return;
                    }
                    //Check whether payment attempt made already
                    if (application.DemandNoteID.HasValue == true && !CommonFunctions.IsDemandNoteCancellable(application.DemandNoteID.Value))
                    {
                        showdata();
                        NormalHeader1.Visible = false;
                        return;
                    }
                    //Get FeeType
                    if (!String.IsNullOrEmpty(Request.QueryString["Status"]) && Request.QueryString["Status"] == "R") //Re-Registration
                    { application.FeeTypeID = Convert.ToInt32(enmFeeType.ReRegistrationFee); }

                    //Modified 12 Jan 2022
                    //			application.FeeTypeID = Convert.ToInt32(enmFeeType.RegistrationFee); }
                    else
                    {
                        //Modified 12 Jan 2022
                        string appId = Request.QueryString["Appid"].ToString();

                        // added by amit start
                        bool reregistrationcase = false;
                        var reregistration = (from a in context.CourseRegistrationApplications
                                              where a.ID.ToString() == appId
                                              select new
                                              {
                                                  registrationtypeid = a.RegistrationTypeID

                                              }).FirstOrDefault();


                        if (reregistration != null)
                        {
                            // condition to check for re - registration case
                            //if (reregistration.registrationtypeid == Convert.ToInt32(enmRegistrationType.ReRegistration))
                            if (reregistration.registrationtypeid == Convert.ToInt32(enmFeeType.ReRegistrationFee))
                            {
                                application.FeeTypeID = Convert.ToInt32(enmFeeType.ReRegistrationFee);
                                reregistrationcase = true;

                            }
                        }

                        // added by amit end

                        if (!reregistrationcase)
                        {
                            //old code
                            var FeeType = (from a in context.CourseRegistrationApplications
                                           where a.ID.ToString() == appId
                                           select new
                                           {
                                               feetype = a.FeeTypeID

                                           }).FirstOrDefault();



                            application.FeeTypeID = Convert.ToInt32(FeeType.feetype);
                            //old code
                            //application.FeeTypeID = Convert.ToInt32(enmFeeType.RegistrationFee); }
                        }

                    }
                    if (application.FeeTypeID == 0)
                        application.FeeTypeID = 1;


                    if (application.apaarID == "")
                    {
                        ShowAlert("Please go back and check apaar id");
                        return;
                    }
                    //Added on 21 Sep 2025 for Apaar validation on Final Submit
                    string apaar = EncryptDecrypt.DecryptString(application.apaarID);
                    checkApaar(apaar, application.DateOfBirth.ToString("dd/MMM/yyyy"), application.Name.ToString(), application.Gender.ToString());
                    //Check for duplicate Apaar for Exam and Course
                    string apaarEncryptedCheck = EncryptDecrypt.EncryptString(apaar);
                    Int64 applId = Convert.ToInt64(Request.QueryString["Appid"]);
                    //EConnectContext context = new EConnectContext();
                    Int32 CourseId = Convert.ToInt32(application.CourseID);
                    Int32 ExamId = Convert.ToInt32(application.ApplicableExamID);
                    //Int32 ExamId = Convert.ToInt32(application.ApplicableExamID);
                    string duplicate = checkDuplicateApaar(apaarEncryptedCheck, CourseId, ExamId);

                    if (duplicate != "0")
                    {
                        if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                        {
                            Int64 applId1 = Convert.ToInt64(Request.QueryString["Appid"]);
                            //This Query will get all the information of Applied Canditate by generated Application id
                            CourseRegistrationApplication oldApplication = context.CourseRegistrationApplications.Find(applId1);
                            if (oldApplication.Number != duplicate)
                            //throw new Exception("Duplicate application for the exam cycle not allowed, Check status for " + duplicate.ToString() + "Application");
                            {
                                ShowAlert("Duplicate application for the exam cycle not allowed, Check status for " + duplicate.ToString() + " Application");
                                return;
                            }
                        }
                        else
                        //throw new Exception("Duplicate application for the exam cycle not allowed, Check status for " + duplicate.ToString() + " Application");
                        {
                            ShowAlert("Duplicate application for the exam cycle not allowed, Check status for " + duplicate.ToString() + " Application");
                            return;
                        }
                    }

                    application.FinalSubmitted = true;
                    application.FinalSubmissionDate = DateTime.Now;
                    courseid = application.CourseID;
                    mobileNumber = application.MobileNumber;
                    emailAddress = application.EmailAddress;
                    application.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                    application.FeeAmount = feeAmount;
                    application.ApplicableExamID = examID;
                    context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    if (!application.DemandNoteID.HasValue)
                    {
                        DemandNote demand = new DemandNote();

                        #region Demand Note - Direct
                        if (application.enmApplicantType == enmApplicantType.Direct)
                        {
                            demand.ApplicationDate = DateTime.Now;
                            demand.FeeTypeID = application.FeeTypeID.Value;
                            demand.ApplicationTypeID = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                            demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.CSCSPV);
                            demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Single);
                            demand.Amount = application.FeeAmount.Value;
                            demand.CreatedBy = 1;
                            demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                            demand.CourseCategoryID = application.CourseCategoryID;
                            demand.CourseID = application.CourseID;
                            demand.ServiceID = application.Course.RegistrationServiceID;
                            context.DemandNotes.Add(demand);
                            context.SaveChanges();
                            application.DemandNoteID = demand.ID;
                            application.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.AppliedButFeeNotDepositedByCandidate);

                            demandID = demand.ID;
                            ApplicationFlag = 1;
                        }
                        #endregion

                        #region Demand Note - Institute
                        else if (application.enmApplicantType == enmApplicantType.Institute)
                        {
                            if (application.enmPaymentSource == enmPaymentSource.Institute)
                            {
                                if (application.CourseCategoryID == 6 && application.FeeAmount == 0) //STC
                                {
                                    Int64 demandnoteId = STCregistration.STCPaymentProcess(application.ID);
                                    if (demandnoteId != 0)
                                    {
                                        application.DemandNoteID = demandnoteId;
                                        application.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                        application.ApplicationStatusID = Convert.ToInt32(enmCertificateExamApplicationStatus.AppliedButPendingForInstituteVerification);

                                        ApplicationFlag = 0;
                                    }
                                    else
                                    { Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitRegistration.aspx?id=" + courseid + "&Appid=" + applID)); }
                                }
                                else
                                {
                                    application.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.AppliedButPendingForInstituteVerification);
                                    ApplicationFlag = 0;
                                }
                            }
                            else
                            {
                                demand.ApplicationDate = DateTime.Now;
                                demand.FeeTypeID = application.FeeTypeID.Value;
                                demand.ApplicationTypeID = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                                demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.CSCSPV);
                                demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Single);
                                demand.Amount = application.FeeAmount.Value;
                                demand.CreatedBy = 1;
                                demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                                // Added by vivek on 13-6-2015
                                demand.CourseCategoryID = application.CourseCategoryID;
                                demand.CourseID = application.CourseID;
                                demand.ServiceID = application.Course.RegistrationServiceID;
                                //end----------
                                context.DemandNotes.Add(demand);
                                context.SaveChanges();

                                application.DemandNoteID = demand.ID;
                                application.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.AppliedButFeeNotDepositedByCandidate);

                                demandID = demand.ID;
                                ApplicationFlag = 1;
                            }
                        }
                        #endregion
                    }
                    context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    var evnt = context.NotificationEvent.Find(Convert.ToInt32(enmNotificationEvents.AfterSubmittingTheCourseRegistrationApplicationForm));
                    if (evnt.SendEmail == true)
                        allowSendingEmail = true;
                    if (evnt.SendSms == true)
                        allowSendingSms = true;
                    mobileMsg = "You have applied for " + application.Course.Name + " on " + application.ApplicationDate.ToString("dd-MMM-yyyy hh:mm") + ". Your application no." +
                      " is " + application.Number + "-NIELIT";
                    msg = "Dear " + GetInitCap(application.Salutation + " " + application.Name) + ",<br/><br/>" + "You have successfully applied for " + GetInitCap(application.Course.Name) +
                        " registration through Online Student Information and Enrollment System of NIELIT. <br/>" +
                        " Your online application details are as following: <br/><br/>" +
                        "Application Number &nbsp;:  " + application.Number + "<br/>" +
                         "Application Date &nbsp;:  " + application.ApplicationDate.ToString("dd-MMM-yyyy") + "";
                    if (application.DemandNoteID.HasValue)
                        msg += "<br/>DemandNote Number &nbsp;  " + application.DemandNoteID.Value + "";
                    else
                        msg += "<br/>DemandNote Number &nbsp;  Not generated";
                    msg += "<br/><br/><br/>Please note your application number.It will be used for further correspondence with NIELIT.";
                    if (application.enmApplicantType == enmApplicantType.Direct)
                    {
                        msg += "<br><br><I>Note: You have not paid the applicable registration fee at the time of form sumbmission. " +
                            " Please pay you applicable registration fee by the last date mentioned in the form using any of the available payment options.<br><br>" +
                            " Important: Please refer your Demand Note Number mentioned in the form at the time of payment at CSC/SPV.";
                    }
                    else if (application.enmApplicantType == enmApplicantType.Institute)
                    {

                        msg += "<br><br><I>Note: You have not paid the applicable registration fee at the time of form sumbmission. " +
                        " Please pay you applicable registration fee to the institute (" + application.Institute.Name + ") by the last date mentioned in the form.<br><br>";
                        // " Important: Please refer your Demand Note Number mentioned in the form at the time of payment at CSC/SPV. Your Demand Note Number is: " + appl.DemandNote.ID.ToString();

                    }
                    //"You can know status of your application by clicking on following link : <br/><br/> <a href=" + HttpContext.Current.Request.UrlReferrer.ToString().Substring(0,HttpContext.Current.Request.UrlReferrer.ToString().LastIndexOf('/')).Replace("CAND", "") + "index.aspx>Know your application status.</a>";
                    scope.Complete();
                }
                ;
            }
            ;
            //sending E-mail Message
            if (allowSendingEmail == true)
            {
                try
                {
                    //sending Email 
                    if (emailAddress.Trim().Length > 0)
                    {
                        EConnect.NIELIT.Email mail = new Email("Online Registration Form:NIELIT", msg, emailAddress);
                        mail.Send();
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
            }
            //sending Mobile Message
            if (allowSendingSms == true)
            {
                try
                {
                    //Sending Mobile Message
                    if (mobileNumber != 0)
                    {
                        EConnect.NIELIT.SMS message = new SMS(mobileMsg, mobileNumber.ToString(), "1307160931586951565", SmsServiceType.SignleSMS);
                        int sentMessageCount;
                        message.sendSingleSMS(out sentMessageCount);
                    }
                }
                catch (Exception ex)
                {
                    // throw ex;
                }
            }
            if ((!String.IsNullOrEmpty(Request.QueryString["Appid"])) && (Request.QueryString["Src"] == "CSC"))
                Response.Redirect("../nieletpaymentservices.aspx?ServiceID=4&DID=" + demandID.ToString());
            else if (ApplicationFlag == 1)//Application type: Direct
                Response.Redirect("~/CAND/FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CourseRegistrationApplication).ToString() + "&Appid=" + applID.ToString() + "&DemandID=" + demandID.ToString());
            else if (ApplicationFlag == 0)//Application type:Institute
                Response.Redirect("~/CAND/FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CourseRegistrationApplication).ToString() + "&Appid=" + applID.ToString() + "&DemandID=0");

        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }
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


}