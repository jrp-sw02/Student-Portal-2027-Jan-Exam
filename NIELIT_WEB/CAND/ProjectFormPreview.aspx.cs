using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class ProjectFormPreview : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentCourseID = 0;
    Int64 applID = 0;
    Course currentcourse;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Lblerror.Text = "";
            Lblerror.Visible = false;
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            loginUserType = (UserType)Session["UserType"];
            if (!String.IsNullOrEmpty(Request.QueryString["candidateID"]))
            {
                entityID = Convert.ToInt64(Request.QueryString["candidateID"]);
            }
            else
            {
                entityID = Convert.ToInt64(Session["EntityID"]);
            }
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            applID = Convert.ToInt64(Request.QueryString["AppID"]);
            //if (Request.UrlReferrer == null)
            //{
            //    Response.Write(GeInvalidRequestMessage("Goto Home Page", Server.MapPath("../Index.aspx")));
            //    Response.End();
            //    return;
            //}


            using (EConnectContext context = new EConnectContext())
            {
                currentcourse = context.CourseProjectApplications.Find(applID).Course;
                currentCourseID = currentcourse.ID;
                lbllevel.Text = currentcourse.Name;
            }
            if (!Page.IsPostBack)
            {
                showdata();
            }
            base.ReWriteAction(this.Form);

        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
            Lblerror.Visible = true;
        }
    }

    protected void showdata()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                CourseProjectApplication appl = context.CourseProjectApplications.Find(applID);
                if (appl.FinalSubmitted == true)
                    NormalHeader1.Visible = true;
                else
                {
                    BtnPrint.Visible = false;
                    lbllevel.Text = "PREVIEW: " + lbllevel.Text;
                    Lblerror.Text = "Dear Candidate, this is preview of Project form you are applying for. Please check all details in preview form and click on 'Final Submit' button if all the details are correct and you are satisfied with details shown in preview form. If you want to change the details click on 'Back' button to go back to Project form.<br> Once you click on 'Final Submit button, you can not modify the details and you will be asked for payment in next page.";
                    Lblerror.Visible = true;
                }
                Candidate candidate = appl.Candidate;
                //if (appl.IsImprovementApplication)
                //    lblImprovement.Text = "Yes";
                //else
                //    lblImprovement.Text = "No";
                if (appl != null)
                {
                    LblAppNumber.Text = appl.Number.ToUpper();
                    LblAppDataTime.Text = appl.ApplicationDate.ToString("dd-MMM-yyyy hh:mm:ss tt");
                }
                currentCourseID = appl.CourseID;
                var registered = (from rg in context.RegistrationDetails
                                  where rg.CandidateID == entityID && rg.CourseID == currentCourseID && rg.RegistrationNo == appl.RegistrationNumber
                                  orderby rg.CommencementFromDate descending
                                  select rg).FirstOrDefault();
                ICollection<CandidateContactDetail> contactDetails = candidate.ContactDetails;
                if (contactDetails != null)
                {
                    CandidateContactDetail cd = contactDetails.OrderByDescending(d => d.EffectiveFromDate).FirstOrDefault();
                    LblLandLine.Text = (cd.StdNumber.HasValue ? cd.StdNumber.Value : 0).ToString() + "-" + (cd.PhoneNumber.HasValue ? cd.PhoneNumber.Value : 0).ToString();
                    if (LblLandLine.Text == "0-0")
                        LblLandLine.Text = "";
                    LblEmail.Text = cd.EmailAddress;
                    LblMobile.Text = (cd.MobileNumber.HasValue ? cd.MobileNumber.Value : 0).ToString();
                }
                Int32 corresspondence = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                Address add = candidate.Addresses.Where(d => d.AddressTypeID == corresspondence).OrderByDescending(d => d.EffectiveDateFrom).FirstOrDefault();
                if (add != null)
                {
                    Lblfulladd.Text = FullCorAddress(add);
                }

                //Personal Details
                if (candidate.FatherName != null && candidate.FatherName != "")
                    LblFName.Text = "Mr. " + GetInitCap(candidate.FatherName);
                else
                {
                    if (candidate.GuardianName != null && candidate.GuardianName != "")
                        LblFName.Text = GetInitCap(candidate.GuardianName);
                    else
                        LblFName.Text = "NA";
                }

                if (candidate.MotherName != null && candidate.MotherName != "")
                    LblMName.Text = "Mrs. " + GetInitCap(candidate.MotherName);
                else
                    LblMName.Text = "NA";
                LblDob.Text = candidate.DateOfBirth.ToString("dd-MMM-yyyy");
                LblAppName.Text = candidate.Salutation + GetInitCap(candidate.Name);
                Lblregno.Text = registered.RegistrationNo.ToString();
                Lblcname.Text = registered.Course.Name;


                imgPhotoBarcode.Src = "../Handlers/BarcodeHandler.ashx?Code=" + appl.Number;//need to change it
                imgBarCode.Src = "../Handlers/BarcodeHandler.ashx?Code=" + appl.Number;//need to change it

                if (candidate.Photo != null)
                    ImgApplicantPhoto.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.Photo.BlobFile);
                if (candidate.LeftThumbImpression != null)
                    imgThumbImpression.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.LeftThumbImpression.BlobFile);
                if (candidate.Signature != null)
                    imgSignature.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])candidate.Signature.BlobFile);

                lblName.Text = GetInitCap(candidate.Name);
                if (candidate.Gender.ToUpper() == "MALE")
                {
                    if (candidate.FatherName != null && candidate.FatherName != "")
                        lblName.Text += " " + "S/o" + GetInitCap(candidate.FatherName);
                    else
                    {
                        if (candidate.GuardianName != null && candidate.GuardianName != "")
                            lblName.Text += " " + "C/o" + GetInitCap(candidate.GuardianName);
                        else
                            lblName.Text += " " + "C/o" + GetInitCap(candidate.GuardianName);
                    }
                    //lblName.Text += " " + "S/o" +GetInitCap( candidate.FatherName);
                }
                else
                {
                    if (candidate.FatherName != null && candidate.FatherName != "")
                        lblName.Text += " " + "D/o" + GetInitCap(candidate.FatherName);
                    else
                    {
                        if (candidate.GuardianName != null && candidate.GuardianName != "")
                            lblName.Text += " " + "C/o" + GetInitCap(candidate.GuardianName);
                        else
                            lblName.Text += " " + "C/o" + GetInitCap(candidate.GuardianName);
                    }
                    //lblName.Text += candidate.Name + " " + "D/o" + candidate.FatherName;
                }
                if (registered.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute))
                {

                    lblCandidateType.Text = " through Accredited Institute namely " + registered.Institute.Name + " ACCR No. " + currentcourse.Code + "-" + registered.Institute.AccreditationDetails.Where(d => d.CourseID == currentCourseID).FirstOrDefault().AccreditationNumber;
                    Lblctype1.Text = currentcourse.Code + "-" + registered.Institute.AccreditationDetails.Where(d => d.CourseID == currentCourseID).FirstOrDefault().AccreditationNumber + " - " + registered.Institute.Name;
                }
                else if (registered.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                {

                    Lblctype1.Text = "Direct Candidate";
                    lblCandidateType.Text = "as a Direct Candidate";
                }

                ////Exam Details
                //Lblexam.Text = appl.Exam.Name;
                //lblExamName.Text = Lblexam.Text.ToUpper();
                //ExamCenter centre1 = appl.ExamCenter1;
                //ExamCenter centre2 = appl.ExamCenter2;
                //lblCentre1.Text = centre1.Code.ToUpper() + " (" + GetInitCap(centre1.Name) + ")";
                //lblCentre2.Text = centre2.Code.ToUpper() + " (" + GetInitCap(centre2.Name) + ")";
                var modules = (from a in context.CourseProjectApplicationDetails
                               join m in context.Modules on a.ModuleID equals m.ID
                               where a.CourseExamApplicationID == applID
                               orderby m.Code
                               select new { m.ShortName, m.Name, m.ModuleTypeID, a.FeeAmount,a.ProjectTitle  });
                Int32 project = Convert.ToInt32(enmModuleType.Project);
                //lblPracticals.Text = "";
                lblprojectmodules.Text = "";
                Int32 projectfee = 0;
                //Int32 theoryFee = 0;
                //Int32 practicalCount = 0;
                Int32 projectCount = 0;
                foreach (var module in modules.ToList())
                {
                    if (module.ModuleTypeID == project)
                    {
                        projectCount++;
                        if (lblprojectmodules.Text.Length > 0)
                            lblprojectmodules.Text += "<br>";
                        lblprojectmodules.Text += projectCount.ToString().PadLeft(2, '0') + ". &nbsp;" + module.ShortName + " &nbsp;&nbsp;" + module.Name;
                        //Added for CHM(t)-O level project 17 May 2024
                        if (lblProjectTitle.Text.Length > 0)
                            lblProjectTitle.Text += "<br>";
                        lblProjectTitle.Text += projectCount.ToString().PadLeft(2, '0') + ". &nbsp;" + module.ProjectTitle;

                        projectfee += (Int32)module.FeeAmount;
                    }
                }
                //if (lblPracticals.Text == "")
                //    lblPracticals.Text = "NA";
                //else
                //    lblPractialCount.Text = " (" + practicalCount.ToString() + " x " + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee).ToString("F") + ")";
                //if ( lblprojectmodules.Text == "")
                //    lblprojectmodules.Text = "NA";
                //else
                //{
                //    if (appl.IsImprovementApplication == true)
                //        lblTheoryCount.Text = " (" + theoryCount.ToString() + " x " + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ImprovementOfPaperFee).ToString("F") + ")";
                //    else
                //        lblTheoryCount.Text = " (" + theoryCount.ToString() + " x " + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee).ToString("F") + ")";
                //}

                //Fee Details
                lblprojectCount.Text = projectCount.ToString();
                lblprojectFee.Text = projectfee.ToString("F");
                //lblPracticalFee.Text = practicalFee.ToString("F");
                //tdProcessingFee.InnerText = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PostageFeeChargedTowardExaminationCorrespondence).ToString("F");
                //if (appl.LateFeeImposed)
                //    Lbllatefee.Text = appl.LateFeeAmount.Value.ToString("F");

                lblTotalFee.Text = appl.FeeAmount.ToString("F");
                lblFeeInWords.Text = EConnect.Utils.Conversion.ConversionUtility.NumberToText(appl.FeeAmount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English);
                LblPaymentDescription.Text += "NA";

                //////Payment Details
                LblPaymentMode.Text = "NA";
                //////////////////////////////////////////DateTime lastDate = (from c in context.CutOffDates
                //////////////////////////////////////////                     where c.ActivityID == submisssionDateActivityID && c.ExamID == appl.ExamID && c.ApplicantTypeID == appl.ApplicantTypeID
                //////////////////////////////////////////                     select c.EfferctiveDate).FirstOrDefault();
                //////////////////////////////////////////DateTime lastDateFormSubmition = lastDate;
                ////////////////////////////////////////////lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");              
                //Commented on 1 May 2024 string lastDate = appl.DemandNoteValiditydateupto.HasValue ? appl.DemandNoteValiditydateupto.Value.ToString("dd-MMM-yyyy") : string.Empty;

                /*Commented on 1 May 2024 var demandnotevalidity = (from f in context.ProjectFees
                                          where f.course_level_id == currentCourseID && f.whether_effective == "Y"
                                          orderby f.project_sequence_number
                                          select f).FirstOrDefault();                                                   
                 if (lastDate != "")
                 {
                     lblLastDate.Text = lastDate;
                 }
                 else
                 {
                     lblLastDate.Text = "Payment must be done within " + demandnotevalidity.demandnote_validity_in_days + " Days from the date of final submit.";
                 }*/

                var demandnotevalidity = (from r in context.RegistrationDetails
                                          where r.CourseID == currentCourseID && (r.RegistrationStatusCode == "R" || r.RegistrationStatusCode == "G")
                    && r.RegistrationNo == appl.RegistrationNumber
                                          select r).FirstOrDefault();

                lblLastDate.Text = "Payment must be done within " + demandnotevalidity.ValidUptoDate;

                if (appl.enmApplicantType == enmApplicantType.Direct)
                {
                    trPaymentNoteInstitute.Visible = false;
                    trPaymentNoteDirect.Visible = true;
                    lblAppliedAs.Text = "As Direct Candidate";
                }
                else
                {
                    lblAppliedAs.Text = "Through Accredited Institute";
                    trPaymentNoteDirect.Visible = false;
                }
                if (appl.enmPaymentSource == enmPaymentSource.Candidate)
                {
                    trPaymentNoteInstitute.Visible = false;
                    trPaymentNoteInstituteSelf.Visible = true;
                    lblFeeSource.Text = "Candidate";
                }
                else if (appl.enmPaymentSource == enmPaymentSource.Institute)
                {
                    trPaymentNoteInstitute.Visible = true;
                    trPaymentNoteInstituteSelf.Visible = false;
                    lblFeeSource.Text = "Accredited Institute (" + Lblctype1.Text + ")";
                }
                else
                {
                    lblCandidateType.Visible = false;
                }

                var demandnotestatus = (from c in context.CourseProjectApplications
                                        where c.CourseID == currentCourseID && c.ID == applID && c.RegistrationNumber == appl.RegistrationNumber
                                        select c).FirstOrDefault();

                if (demandnotestatus.WhetherDemandNoteCanceled == true)
                {
                    Lblerror.Text = "The Demand Note against this application Number was expired. Hence Project Application Form will not be considered.";
                    Lblerror.Visible = true;
                }


                if (appl.FinalSubmitted)
                {
                    if (appl.DemandNote != null)
                    {
                        DemandNote demandNote = appl.DemandNote;
                        lblDemandNoteNo.Text = demandNote.ID.ToString();
                        lblDemandNoteDate.Text = demandNote.ApplicationDate.ToString("dd-MMM-yyyy"); ;
                        LblPaymentMode.Text = demandNote.PaymentMode.Name;
                        if (demandNote.enmPaymentStatus == enmPaymentStatus.Paid || demandNote.enmPaymentStatus == enmPaymentStatus.PaidButNotVerified)
                        {
                            if (demandNote.enmPaymentMode == enmPaymentMode.CSCSPV)
                                LblPaymentDescription.Text = demandNote.CSCTransaction.ResponseTransactionNumber.ToString() + " Transaction Date " + demandNote.CSCTransaction.Date.ToString("dd-MMM-yyyy hh:mm");
                            else if (demandNote.enmPaymentMode == enmPaymentMode.DemandDraft)
                                LblPaymentDescription.Text = demandNote.DemandDraftTransaction.DemandDraftNumber + " Transaction Dated " + demandNote.DemandDraftTransaction.Date.ToString("dd-MMM-yyyy hh:mm");
                            else if (demandNote.enmPaymentMode == enmPaymentMode.Online)
                                LblPaymentDescription.Text = demandNote.OnlineTransaction.ReferenceNumber + " Transaction Date " + demandNote.OnlineTransaction.ResponseDate.Value.ToString("dd-MMM-yyyy hh:mm");
                            else if (demandNote.enmPaymentMode == enmPaymentMode.NEFTRTGS)
                                LblPaymentDescription.Text = demandNote.NEFTTransaction.TransactionNumber + " Transaction Date " + demandNote.NEFTTransaction.TransactionDate.ToString("dd-MMM-yyyy hh:mm");
                        }
                        else
                        {
                            LblPaymentDescription.Text = "NA";
                        }
                        if (demandNote.enmPaymentStatus == enmPaymentStatus.Paid)
                        {
                            lbn_generate_receipt.NavigateUrl = "";
                            lbn_generate_receipt.Attributes.Add("onclick", "window.top.location.href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../PaymentAcknowledgementReceipt.aspx?DemandNoteId=" + appl.DemandNote.ID.ToString() + "&AppID=" + applID.ToString() + "&RegnNumber=" + appl.RegistrationNumber.ToString()) + "'");
                            tr_receipt.Visible = true;
                        }
                    }
                    else
                    {
                        LblPaymentMode.Text = "NA";

                    }
                    Btnsubmit.Visible = false;
                    btnback.Visible = false;
                }
            };

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private string FullCorAddress(Address crs)
    {
        string str = crs.AddressLine1;
        if (crs.AddressLine2 != null)
            if (crs.AddressLine2.Trim().Length > 1)
                str += ", " + crs.AddressLine2.Trim().Trim(',');
        if (crs.AddressLine3 != null)
            if (crs.AddressLine3.Trim().Length > 1)
                str += ", " + crs.AddressLine3.Trim().Trim(',');
        if (crs.CityName != null)
            str += "<br>" + crs.CityName;
        if (crs.DistrictID.HasValue)
            str += "<br/> District:- " + crs.District.Name + ", ";
        if (crs.State.Name != null)
            str += "<br/> State:- " + crs.State.Name;
        if (crs.PinCode.HasValue)
            str += ",&nbsp; Pin:- " + crs.PinCode.Value.ToString();
        return str;
    }

    protected void Btnback_Click(object sender, EventArgs e)
    {
        using (EConnectContext context = new EConnectContext())
        {
            CourseProjectApplication appl = context.CourseProjectApplications.Find(Convert.ToInt64(Request.QueryString["AppID"]));
            currentCourseID = appl.Course.ID;
            Int64 registrationNumber = appl.RegistrationNumber;
            context.SaveChanges();

            if ((!String.IsNullOrEmpty(Request.QueryString["Appid"])) && (Request.QueryString["Src"] == "CSC"))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("FrmProjectForm.aspx?RegNo=" + registrationNumber.ToString() + "&CourseID=" + currentCourseID.ToString() + "&Appid=" + Request.QueryString["AppID"] + "&Src=CSC"));
            }
            else
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("FrmProjectForm.aspx?RegNo=" + registrationNumber.ToString() + "&CourseID=" + currentCourseID.ToString() + "&Appid=" + Request.QueryString["AppID"]));
            }
        };
    }

    protected void Btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            int ApplicationFlag = 0;
            //Int32 paymentstatusid = 0;
            Int64 demandID = 0; string emailAddress = "";
            Int64 mobileNumber = 0; string mobileMsg = "";
            string msg = "";
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    CourseProjectApplication appl = context.CourseProjectApplications.Find(applID);
                    Candidate candidate = appl.Candidate;
                    ICollection<CandidateContactDetail> contactDetails = candidate.ContactDetails;
                    if (contactDetails != null)
                    {
                        CandidateContactDetail cd = contactDetails.OrderByDescending(d => d.EffectiveFromDate).FirstOrDefault();
                        mobileNumber = (cd.MobileNumber.HasValue ? cd.MobileNumber.Value : 0);
                        emailAddress = cd.EmailAddress;
                    }

                    //Check whether payment attempt made already
                    if (appl.DemandNoteID.HasValue == true && !CommonFunctions.IsDemandNoteCancellable(appl.DemandNoteID.Value))
                    {
                        showdata();
                        NormalHeader1.Visible = false;
                        return;
                    }

                    CourseProjectApplication app = new CourseProjectApplication();
                    var application = (from a in context.CourseProjectApplications
                                       where a.ID == applID
                                       select a).FirstOrDefault();
                    var demandnotevalidity = (from f in context.ProjectFees
                                              where f.course_level_id == currentCourseID && f.whether_effective == "Y"
                                              orderby f.project_sequence_number
                                              select f).FirstOrDefault();

                    application.FinalSubmitted = true;
                    application.FinalSubmissionDate = DateTime.Now;
                    application.DemandNoteValiditydateupto = DateTime.Now.AddDays(demandnotevalidity.demandnote_validity_in_days);

                    application.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                    context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    //CourseRegistrationApplication  currentCourse = context.CourseRegistrationApplications.Find(Convert.ToInt64(Request.QueryString["Appid"]));
                    //courseid = currentCourse.CourseID;
                    if (!application.DemandNoteID.HasValue)
                    {
                        DemandNote demand = new DemandNote();
                        if (application.enmApplicantType == enmApplicantType.Direct)
                        {
                            demand.ApplicationDate = DateTime.Now;
                            demand.FeeTypeID = application.FeeTypeID.Value;
                            demand.ApplicationTypeID = Convert.ToInt32(enmApplicationType.CourseProjectApplication);
                            demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.CSCSPV);
                            demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Single);
                            demand.Amount = application.FeeAmount;
                            demand.CreatedBy = 1;
                            demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                            // Added by vivek on 13-6-2015
                            demand.CourseCategoryID = application.CourseCategoryID;
                            demand.CourseID = application.CourseID;
                            demand.ServiceID = application.Course.ProjectServiceID;
                            //end----------
                            context.DemandNotes.Add(demand);
                            context.SaveChanges();

                            application.DemandNoteID = demand.ID;
                            application.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate);

                            demandID = demand.ID;
                            ApplicationFlag = 1;
                        }
                        else if (application.enmApplicantType == enmApplicantType.Institute)
                        {
                            /* Commented for CHMT(O Level) project 15 May 2024 if (appl.enmPaymentSource == enmPaymentSource.Institute)
                             {
                                 application.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification);
                                 ApplicationFlag = 0;
                             }*/
                            if (appl.enmPaymentSource == enmPaymentSource.Institute)
                            {
                                application.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification);
                                //Lblerror.Visible = true;
                                //Lblerror.Text = "Your application is submitted successfully.Please contact your institute for further processing. ";
                                Tblpreview.Visible = false;
                                btnback.Visible = false;
                                Btnsubmit.Visible = false;
                                lblErrorMsg.Visible = true;
                                lblErrorMsg.Text = "Your application is submitted successfully. Please contact institute for further processing. ";

                                //  ApplicationFlag = -1;
                            }
                            else
                            {
                                demand.ApplicationDate = DateTime.Now;
                                demand.FeeTypeID = application.FeeTypeID.Value;
                                demand.ApplicationTypeID = Convert.ToInt32(enmApplicationType.CourseProjectApplication);
                                demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.CSCSPV);
                                demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Single);
                                demand.Amount = application.FeeAmount;
                                demand.CreatedBy = 1;
                                demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                                // Added by vivek on 13-6-2015
                                demand.CourseCategoryID = application.CourseCategoryID;
                                demand.CourseID = application.CourseID;
                                demand.ServiceID = application.Course.ProjectServiceID;
                                //end----------
                                context.DemandNotes.Add(demand);
                                context.SaveChanges();

                                application.DemandNoteID = demand.ID;
                                application.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate);

                                demandID = demand.ID;
                                ApplicationFlag = 1;
                            }
                        }
                    }
                    context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    mobileMsg = "You have applied for (" + application.Course.Name + ") project on " + application.ApplicationDate.ToString("dd-MMM-yyyy hh:mm") + ". Your application no." +
                      " is " + application.Number + " and your Registration Number is:" + application.RegistrationNumber;


                    msg = "Dear " + GetInitCap(candidate.Salutation + " " + candidate.Name) + ",<br/><br/>" + "You have successfully applied for " +
                        " (" + application.Course.Name + ") Project through Online Student Information and Enrollment System of NIELIT. <br/>" +
                        " Your online application details are as following: <br/><br/>" +
                        "Application Number &nbsp;:  " + application.Number + "<br/>" +
                         "Application Date &nbsp;:  " + application.ApplicationDate.ToString("dd-MMM-yyyy") + "<br/><br/><br/>" +
                       "Please note your application number.It will be used for further correspondence with NIELIT.";
                    if (application.enmApplicantType == enmApplicantType.Direct)
                    {
                        msg += "<br><br><I>Note: You have not paid the applicable project fee at the time of form submission. " +
                            " Please pay you applicable project fee by the last date mentioned in the form using any of the available payment options.<br><br>" +
                            " Important: Please refer your Demand Note Number mentioned in the form at the time of payment at CSC/SPV. Your Demand Note Number is: " + appl.DemandNote.ID.ToString();
                    }
                    else if (application.enmApplicantType == enmApplicantType.Institute)
                    {
                        if (appl.enmPaymentSource == enmPaymentSource.Institute)
                        {
                            msg += "<br><br><I>Note: You have not paid the applicable project fee at the time of form submission. " +
                            " Please pay you applicable project fee to the institute (" + appl.Institute.Name + ") by the last date mentioned in the form.<br><br>";
                            // " Important: Please refer your Demand Note Number mentioned in the form at the time of payment at CSC/SPV. Your Demand Note Number is: " + appl.DemandNote.ID.ToString();
                        }
                        else
                        {
                            msg += "<br><br><I>Note: You have not paid the applicable project fee at the time of form submission. " +
                             " Please pay you applicable project fee by the last date mentioned in the form using any of the available payment options.<br><br>" +
                             " Important: Please refer your Demand Note Number mentioned in the form at the time of payment at CSC/SPV. Your Demand Note Number is: " + appl.DemandNote.ID.ToString();
                        }
                    }
                    scope.Complete();
                };
            };
            //sending E-mail Message
            try
            {
                if (emailAddress.Trim().Length > 0)
                {
                    //sending Email 
                    EConnect.NIELIT.Email mail = new Email("Online Examination Form:NIELIT", msg, emailAddress);
                    mail.Send();
                }
            }
            catch (Exception ex)
            {
            }
            //sending Mobile Message
            try
            {
                if (mobileNumber != 0)
                {
                    //Sending Mobile Message
                    EConnect.NIELIT.SMS message = new SMS(mobileMsg, mobileNumber.ToString(), "", SmsServiceType.SignleSMS);
                    int sentMessageCount;
                    message.sendSingleSMS(out sentMessageCount);
                }
            }
            catch (Exception ex)
            {
            }
            //if ((!String.IsNullOrEmpty(Request.QueryString["Appid"])) && (Request.QueryString["Src"] == "CSC"))
            //    Response.Redirect("../nieletpaymentservices.aspx?ServiceID=4&DID=" + demandID.ToString());
            if (ApplicationFlag == 1)//Application type: Direct
                Response.Redirect("~/CAND/FrmConfirmProject.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CourseProjectApplication).ToString() + "&Appid=" + applID.ToString() + "&DemandID=" + demandID.ToString(), true);
            //else if (ApplicationFlag == 0)//Application type:Institute
            //    Response.Redirect("~/CAND/FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CourseProjectApplication).ToString() + "&Appid=" + applID.ToString() + "&DemandID=0", true);
        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
            Lblerror.Visible = true;

            Console.WriteLine(ex.InnerException.Message);
        }

    }

    //protected void lbn_generate_receipt_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("../PaymentAcknowledgementReceipt.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CourseProjectApplication).ToString() + "&Appid=" + applID.ToString() + "&DemandID=" + demandID.ToString(), true);
    //}
}