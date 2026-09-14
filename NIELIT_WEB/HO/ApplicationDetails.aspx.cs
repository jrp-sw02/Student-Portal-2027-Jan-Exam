using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class HO_ApplicationDetails : BasePage
{
  
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/frmCourseRegistrationApplication.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            if (!Page.IsPostBack)
            {
                Bind();
            }
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Application Detail", "", ""));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void Bind()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                CertificateExamApplication cr = new CertificateExamApplication();
                CourseRegistrationApplication crs = new CourseRegistrationApplication();
                CourseExamApplication cea = new CourseExamApplication();
                StringBuilder deficiencyname = new StringBuilder();
                Int32 counter = 0;
                Int32 applicationTypeID = Convert.ToInt32(Request.QueryString["ApplicationTypeID"].ToString());
                if (applicationTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    string applicationNo = Request.QueryString["Appno"].ToString();
                    cr = context.CertificateExamApplications.Where(s => s.Number.ToUpper() == applicationNo.ToUpper()).FirstOrDefault();
                    if (cr != null)
                    {
                        lblAppno.Text = cr.Number;
                        lblDreceive.Text = cr.ApplicationDate.ToString("dd-MMM-yyyy");
                        lblCourse.Text = Convert.ToString(cr.Course.Name) + "-" + Convert.ToString(cr.CourseCategory.Name);
                        Lblname.Text = cr.Salutation + CommonFunctions.GetInitCap(Convert.ToString(cr.Name));
                        lblExamName.Text = cr.Exam.Name;
                        if (cr.FeeAmount.HasValue)
                            lblFeeAmt.Text = cr.FeeAmount.Value.ToString("F");
                        else
                            lblFeeAmt.Text = "Not Paid";

                        if (string.IsNullOrEmpty(cr.GuardianName) == true && string.IsNullOrWhiteSpace(cr.GuardianName) == true)
                        {
                            TrFatherName.Visible = true;
                            TrMotherName.Visible = true;
                            TrGardianName.Visible = false;
                            if (string.IsNullOrEmpty(cr.FatherName) == false && !string.IsNullOrWhiteSpace(cr.FatherName))
                                LblFatherName.Text = "Mr. " + CommonFunctions.GetInitCap(cr.FatherName);
                            else
                                LblFatherName.Text = "NA";
                            if (string.IsNullOrEmpty(cr.MotherName) == false && string.IsNullOrWhiteSpace(cr.MotherName) == false)
                                LblMotherName.Text = "Mrs. " + CommonFunctions.GetInitCap(cr.MotherName);
                            else
                                LblMotherName.Text = "NA";
                            trgender.Attributes.Add("class", "gdalternate1");
                            trdob.Attributes.Add("class", "gdrow1");
                        }
                        else
                        {
                            LblGuardianName.Text = string.IsNullOrEmpty(cr.GuardianName) == false && string.IsNullOrWhiteSpace(cr.GuardianName) == false ? CommonFunctions.GetInitCap(cr.GuardianName) : "NA";
                            TrFatherName.Visible = false;
                            TrMotherName.Visible = false;
                            TrGardianName.Visible = true;
                            trgender.Attributes.Add("class", "gdrow1");
                            trdob.Attributes.Add("class", "gdalternate1");
                            tdphoto.RowSpan = 9;
                        }
                        trperaddress.Visible = false;
                        trperaddressname.Visible = false;
                        trmaritalstatus.Visible = false;
                        lblGender.Text = CommonFunctions.GetInitCap(Convert.ToString(cr.Gender));
                        enmCertificateExamApplicationStatus applStatus = (enmCertificateExamApplicationStatus)cr.ApplicationStatusID;
                        if ((applStatus == enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre) && (cr.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending)))
                            lblStatus.Text = "Fee Pending to be Paid by the Institute";
                        else
                            lblStatus.Text = CommonFunctions.GetInitCap(EConnect.Utils.Common.EnumUtility.GetDescription(applStatus));
                        lblMstatus.Visible = false;
                        lblDob.Text = cr.DateOfBirth.ToString("dd-MMM-yyyy");
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cr.Photo);
                        lblCaddress.Text = FullCoAddressExam(cr);
                        lblCcontact.Text = Convert.ToString(cr.StdNumber) + "-" + Convert.ToString(cr.PhoneNumber);
                        lblCmobile.Text = Convert.ToString(cr.MobileNumber);
                        lblCemail.Text = Convert.ToString(cr.EmailAddress);
                        lblHqualification.Text = CommonFunctions.GetInitCap(Convert.ToString(cr.EducationalQualification.Name));
                        hlklink.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CertificatePreview.aspx?ID=" + cr.CourseID + "&Appid=" + cr.ID + "&Dob=" + cr.DateOfBirth + "&Type=Print");
                        hlklink.Target = "_blank";
                    }

                }
                else if (applicationTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    string applicationNo = Request.QueryString["Appno"].ToString();
                    crs = context.CourseRegistrationApplications.Where(s => s.Number.ToUpper() == applicationNo.ToUpper()).FirstOrDefault();
                    if (crs != null)
                    {
                        lblAppno.Text = crs.Number;
                        lblDreceive.Text = crs.ApplicationDate.ToString("dd-MMM-yyyy");
                        lblCourse.Text = Convert.ToString(crs.Course.Name) + "-" + Convert.ToString(crs.CourseCategory.Name);
                        Lblname.Text = crs.Salutation + CommonFunctions.GetInitCap(Convert.ToString(crs.Name));
                        lblExamName.Text = crs.ApplicableExam.Name;
                        if (crs.FeeAmount.HasValue)
                            lblFeeAmt.Text = crs.FeeAmount.Value.ToString("F");
                        else
                            lblFeeAmt.Text = "Not Paid";
                        if (string.IsNullOrEmpty(crs.GuardianName) == true && string.IsNullOrWhiteSpace(crs.GuardianName) == true)
                        {
                            TrFatherName.Visible = true;
                            TrMotherName.Visible = true;
                            TrGardianName.Visible = false;
                            if (string.IsNullOrEmpty(crs.FatherName) == false && !string.IsNullOrWhiteSpace(crs.FatherName))
                                LblFatherName.Text = "Mr. " + CommonFunctions.GetInitCap(crs.FatherName);
                            else
                                LblFatherName.Text = "NA";
                            if (string.IsNullOrEmpty(crs.MotherName) == false && string.IsNullOrWhiteSpace(crs.MotherName) == false)
                                LblMotherName.Text = "Mrs. " + CommonFunctions.GetInitCap(crs.MotherName);
                            else
                                LblMotherName.Text = "NA";
                        }
                        else
                        {
                            LblGuardianName.Text = string.IsNullOrEmpty(crs.GuardianName) == false && string.IsNullOrWhiteSpace(crs.GuardianName) == false ? CommonFunctions.GetInitCap(crs.GuardianName) : "NA";
                            TrFatherName.Visible = false;
                            TrMotherName.Visible = false;
                            TrGardianName.Visible = true;
                        }

                        lblGender.Text = CommonFunctions.GetInitCap(Convert.ToString(crs.Gender));
                        enmCourseApplicationStatus applStatus = (enmCourseApplicationStatus)crs.ApplicationStatusID;
                        if ((applStatus == enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT) && (crs.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending)))
                            lblStatus.Text = "Fee Pending to be Paid by the Institute";
                        else
                            lblStatus.Text = CommonFunctions.GetInitCap(EConnect.Utils.Common.EnumUtility.GetDescription(applStatus));
                        lblMstatus.Text = CommonFunctions.GetInitCap(Convert.ToString(crs.MaritalStatus.Name));
                        lblDob.Text = crs.DateOfBirth.ToString("dd-MMM-yyyy");
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])crs.Photo);
                        lblCaddress.Text = FullCoAddress(crs);
                        lblPaddress.Text = FullPerAddress(crs);
                        lblCcontact.Text = Convert.ToString(crs.StdNumber) + "-" + Convert.ToString(crs.PhoneNumber);
                        lblCmobile.Text = Convert.ToString(crs.MobileNumber);
                        lblCemail.Text = Convert.ToString(crs.EmailAddress);
                        lblHqualification.Text = CommonFunctions.GetInitCap(Convert.ToString(crs.EducationalQualification.Name));
                        hlklink.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/FrmPreview.aspx?ID=" + crs.CourseID + "&Appid=" + crs.ID + "&Dob=" + crs.DateOfBirth + "&Type=Print");
                        hlklink.Target = "_blank";
                        if (crs.ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted))
                        {
                            trreg1.Visible = true;
                            trreg2.Visible = true;
                            trreg3.Visible = true;
                            trreg4.Visible = true;
                            trreg5.Visible = true;
                            trreg6.Visible = true;
                            trreg7.Visible = true;
                            trreg8.Visible = false;
                            trreg9.Visible = false;
                            var registrationdata = (from r in context.RegistrationDetails
                                                    where r.CourseRegistrationApplicationID == crs.ID
                                                    orderby r.CommencementFromDate descending
                                                    select r).FirstOrDefault();
                            if (registrationdata != null)
                            {
                                if (registrationdata.RegistrationTypeID.Value == 4)
                                {
                                    Lbreg.Text = "Re-Registration Number";
                                    if (registrationdata.ReRegistrationNumber.HasValue)
                                        lblCurrntRegNo.Text = registrationdata.ReRegistrationNumber.ToString() + " ( " + crs.RegistrationType.Name + " ) ";
                                    Lbregdate.Text = "Re-Registration Date";
                                    if (registrationdata.ReRegistrationDate.HasValue)
                                        lblCurrntRegDate.Text = registrationdata.ReRegistrationDate.Value.ToString("dd-MMM-yyyy");
                                }
                                else
                                {
                                    Lbreg.Text = "Registration Number";
                                    lblCurrntRegNo.Text = registrationdata.RegistrationNo.ToString() + " ( " + crs.RegistrationType.Name + " ) ";
                                    Lbregdate.Text = "Registration Date";
                                    lblCurrntRegDate.Text = registrationdata.RegistrationDate.ToString("dd-MMM-yyyy");
                                }
                                lblCurrntRegCourse.Text = context.Courses.Find(crs.CourseID).Name;
                                lblCommncmntFrmDate.Text = registrationdata.CommencementFromDate.ToString("dd-MMM-yyyy");
                                lblValidUpToDate.Text = registrationdata.ValidUptoDate.ToString("dd-MMM-yyyy");
                                var exams = context.Exams.Find(crs.ApplicableExamID);
                                lblexamname1.Text = exams != null ? exams.Name : "";
                            }
                        }
                        else if (crs.ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance))
                        {
                            trreg1.Visible = true;
                            trreg2.Visible = false;
                            trreg3.Visible = false;
                            trreg4.Visible = false;
                            trreg5.Visible = false;
                            trreg6.Visible = false;
                            trreg7.Visible = false;
                            trreg8.Visible = true;
                            trreg9.Visible = false;
                            var deficiencydetails = (from p in context.BatchItemDeficiencyDetails
                                                     join t in context.DeficiencyCodes
                                                         on p.DeficiencyID equals t.ID
                                                     join c in context.BatchItems
                                                         on p.BatchItemID equals c.ID
                                                     where p.BatchItemID == crs.BatchItemID && p.IsFullFilled == false
                                                     orderby p.DeficiencyID
                                                     select new
                                                     {
                                                         deficiencydesc = c.KeptInAbeyanceReason,
                                                         defficiencydetail = t.Description
                                                     }).ToList();
                            if (deficiencydetails.Count() > 0)
                            {
                                foreach (var deficiency in deficiencydetails)
                                {
                                    counter = counter + 1;
                                    deficiencyname.Append(counter + ". " + deficiency.defficiencydetail + "<br/>");
                                }
                                Lbabeyance.Text = "Note:- This Application has been Kept In Abeyance due to the following deficiencies found in the application form :- <br/> " + deficiencyname.ToString() + " Remarks:- <br/>" + GetInitCap(deficiencydetails.FirstOrDefault().deficiencydesc.ToString()) + "";
                            }
                        }
                        else if (crs.ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedWithReason))
                        {
                            trreg1.Visible = true;
                            trreg2.Visible = false;
                            trreg3.Visible = false;
                            trreg4.Visible = false;
                            trreg5.Visible = false;
                            trreg6.Visible = false;
                            trreg7.Visible = false;
                            trreg8.Visible = false;
                            trreg9.Visible = true;
                            var rejectedreason = (from p in context.BatchItemDeficiencyDetails
                                                  join t in context.DeficiencyCodes
                                                      on p.DeficiencyID equals t.ID
                                                  join c in context.BatchItems
                                                      on p.BatchItemID equals c.ID
                                                  where p.BatchItemID == crs.BatchItemID && p.IsFullFilled == false
                                                  orderby p.DeficiencyID
                                                  select new
                                                  {
                                                      reasondesc = c.RejectedReason,
                                                      reasondetail = t.Description
                                                  }).ToList();
                            if (rejectedreason.Count() > 0)
                            {
                                foreach (var reasons in rejectedreason)
                                {
                                    counter = counter + 1;
                                    deficiencyname.Append(counter + ". " + reasons.reasondetail + "<br/>");
                                }
                                lbrejected.Text = "Note:- This Application has been Rejected due to the following deficiencies found in the application form :- <br/> " + deficiencyname.ToString() + " Remarks:- <br/>" + GetInitCap(rejectedreason.FirstOrDefault().reasondesc.ToString()) + "";
                            }
                        }
                    }
                }
                else if (applicationTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    string applicationNo = Request.QueryString["Appno"].ToString();
                    cea = context.CourseExamApplications.Where(s => s.Number.ToUpper() == applicationNo.ToUpper()).FirstOrDefault();

                    if (cea != null)
                    {
                        lblAppno.Text = cea.Number;
                        lblDreceive.Text = cea.ApplicationDate.ToString("dd-MMM-yyyy");
                        lblCourse.Text = Convert.ToString(cea.Course.Name) + "-" + Convert.ToString(cea.CourseCategory.Name);
                        Lblname.Text = cea.Candidate.Salutation + CommonFunctions.GetInitCap(Convert.ToString(cea.Candidate.Name));
                        lblExamName.Text = cea.Exam.Name;
                        if (cea.FeeAmount != 0)
                            lblFeeAmt.Text = cea.FeeAmount.ToString("F");
                        else
                            lblFeeAmt.Text = "Not Paid";
                        if (string.IsNullOrEmpty(cea.Candidate.GuardianName) == true && string.IsNullOrWhiteSpace(cea.Candidate.GuardianName) == true)
                        {
                            TrFatherName.Visible = true;
                            TrMotherName.Visible = true;
                            TrGardianName.Visible = false;
                            if (string.IsNullOrEmpty(cea.Candidate.FatherName) == false && !string.IsNullOrWhiteSpace(cea.Candidate.FatherName))
                                LblFatherName.Text = "Mr. " + CommonFunctions.GetInitCap(cea.Candidate.FatherName);
                            else
                                LblFatherName.Text = "NA";
                            if (string.IsNullOrEmpty(cea.Candidate.MotherName) == false && string.IsNullOrWhiteSpace(cea.Candidate.MotherName) == false)
                                LblMotherName.Text = "Mrs. " + CommonFunctions.GetInitCap(cea.Candidate.MotherName);
                            else
                                LblMotherName.Text = "NA";
                        }
                        else
                        {
                            LblGuardianName.Text = string.IsNullOrEmpty(cea.Candidate.GuardianName) == false && string.IsNullOrWhiteSpace(cea.Candidate.GuardianName) == false ? CommonFunctions.GetInitCap(cea.Candidate.GuardianName) : "NA";
                            TrFatherName.Visible = false;
                            TrMotherName.Visible = false;
                            TrGardianName.Visible = true;
                        }
                        lblGender.Text = CommonFunctions.GetInitCap(Convert.ToString(cea.Candidate.Gender));
                        enmCourseExamApplicationStatus applStatus = (enmCourseExamApplicationStatus)cea.ApplicationStatusID;
                        if ((applStatus == enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT) && (cea.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending)))
                            lblStatus.Text = "Fee Pending to be Paid by the Institute";
                        else if (applStatus == enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT)
                        {
                            lblStatus.Text = CommonFunctions.GetInitCap(EConnect.Utils.Common.EnumUtility.GetDescription(applStatus));
                            if (cea.FormForwardedByInstituteOn.HasValue)
                                lblStatus.Text += "<br/> On :- " + cea.FormForwardedByInstituteOn.Value.ToString("dd-MMM-yyyy");
                        }
                        else
                            lblStatus.Text = CommonFunctions.GetInitCap(EConnect.Utils.Common.EnumUtility.GetDescription(applStatus));
                        lblMstatus.Text = CommonFunctions.GetInitCap(Convert.ToString(cea.Candidate.MaritalStatus.Name));
                        lblDob.Text = cea.Candidate.DateOfBirth.ToString("dd-MMM-yyyy");
                        ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cea.Candidate.Photo.BlobFile);
                        Int32 corresspondence = Convert.ToInt32(enmAddressType.CorrespondenceAddress);
                        Address corrAdd = cea.Candidate.Addresses.Where(d => d.AddressTypeID == corresspondence).OrderByDescending(d => d.EffectiveDateFrom).FirstOrDefault();
                        if (corrAdd != null)
                        {
                            lblPaddress.Text = FullCorAddressCourseExamApplication(corrAdd);
                        }
                        Int32 permanent = Convert.ToInt32(enmAddressType.PermanentAddress);
                        Address perAdd = cea.Candidate.Addresses.Where(d => d.AddressTypeID == permanent).OrderByDescending(d => d.EffectiveDateFrom).FirstOrDefault();
                        if (perAdd != null)
                        {
                            lblCaddress.Text = FullPermanentAddressCourseExamApplication(perAdd);
                        }
                        ICollection<CandidateContactDetail> contactDetails = cea.Candidate.ContactDetails;
                        if (contactDetails != null)
                        {
                            CandidateContactDetail cd = contactDetails.OrderByDescending(d => d.EffectiveFromDate).FirstOrDefault();
                            lblCcontact.Text = (cd.StdNumber.HasValue ? cd.StdNumber.Value : 0).ToString() + "-" + (cd.PhoneNumber.HasValue ? cd.PhoneNumber.Value : 0).ToString();
                            lblCemail.Text = cd.EmailAddress;
                            lblCmobile.Text = (cd.MobileNumber.HasValue ? cd.MobileNumber.Value : 0).ToString();
                        }
                        CandidateQualificationDetail qd = context.CandidateQualificationDetails.Where(s => s.CandidateID == cea.CandidateID).OrderByDescending(d => d.EffectiveFromDate).FirstOrDefault();
                        lblHqualification.Text = (qd.EducationalQualificationID != 0) ? CommonFunctions.GetInitCap(qd.EducationalQualification.Name) : "NA";
                        // Payment details
                        trpayment.Visible = true;
                        trpaymentmode.Visible = true;
                        trpaymentsource.Visible = true;
                        trappliedas.Visible = true;
                        btnDemadNote.Visible = false;
                        lbpaymtmode.Text = cea.DemandNoteID.HasValue ? cea.DemandNote.PaymentMode.Name : "NA" + " ( " + GetInitCap("As DemandNote not generated") + " )";
                        if (cea.PaymentSourceID != 0)
                        {
                            if (cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate))
                            {//Generate demand note
                                if (cea.DemandNoteID.HasValue == false)
                                {
                                    btnDemadNote.Visible = true;
                                }
                                lbpaymtsource.Text = "Candidate";
                                if (cea.PaymentSourceChanged.ToString() == "True")
                                {
                                    lbpaymtsource.Text += " ( Payment option changed from institute to candidate on " + cea.PaymentSourceChangedOn.Value.ToString("dd-MMM-yyyy") + " )";
                                }
                            }
                            else if (cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Institute))
                            {
                                lbpaymtsource.Text = "Institute";
                                if (cea.PaymentSourceChanged.ToString() == "True")
                                {
                                    lbpaymtsource.Text += " ( Payment option changed from candidate to institute on " + cea.PaymentSourceChangedOn.Value.ToString("dd-MMM-yyyy") + " )";
                                }
                            }
                        }
                        else
                        {
                            lbpaymtsource.Text = "---";
                        }
                        lbappliedas.Text = GetInitCap(cea.ApplicantType.Name);
                        if (cea.ApplicantTypeChanged.ToString() == "True")
                        {
                            lbappliedas.Text += " ( Applicant type changed from institute to direct on " + cea.ApplicantTypeChangedOn.Value.ToString("dd-MMM-yyyy") + " )";
                        }
                        if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate) && (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedbyInstitute)))
                        {
                            if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate) && (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedbyInstitute)) && CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.HasValue ? cea.DemandNoteID.Value : 0))
                            {
                                tdradio.Visible = true;
                                Rdserachby.Visible = true;
                                if (cea.PaymentSourceChanged == true)
                                {
                                    Rdserachby.Items.FindByText("Change Payment Option").Attributes.Add("style", "display:none");
                                }
                                if (cea.FinalSubmitted == false && cea.FinalSubmissionDate == null)
                                {
                                    Rdserachby.Items.FindByText("Cancel / Mark Application as Editable").Attributes.Add("style", "display:none");
                                }
                            }
                            else if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate) && (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedbyInstitute)))
                            {
                                Rdserachby.Visible = true;
                                tdradio.Visible = true;
                                Rdserachby.Items.FindByText("Change Payment Option").Attributes.Add("style", "display:none");
                            }
                        }
                        else if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Institute) && cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification))
                        {
                            Rdserachby.Visible = true;
                            tdradio.Visible = true;
                            if (cea.PaymentSourceChanged == true)
                            {
                                Rdserachby.Items.FindByText("Change Payment Option").Attributes.Add("style", "display:none");
                            }
                            if (cea.FinalSubmitted == false && cea.FinalSubmissionDate == null)
                            {
                                Rdserachby.Items.FindByText("Cancel / Mark Application as Editable").Attributes.Add("style", "display:none");
                            }
                        }
                        else if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Institute) && cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate))
                        {
                            Rdserachby.Visible = false;
                            tdradio.Visible = false;
                        }
                        else if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct) && cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) && cea.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending) && CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.HasValue ? cea.DemandNoteID.Value : 0))
                        {
                            Rdserachby.Visible = true;
                            tdradio.Visible = true;
                            Rdserachby.Items.FindByText("Change Payment Option").Attributes.Add("style", "display:none");
                            Rdserachby.Items.FindByText("Change Applicant Type").Attributes.Add("style", "display:none");
                            if (cea.FinalSubmitted == false && cea.FinalSubmissionDate == null)
                            {
                                Rdserachby.Items.FindByText("Cancel / Mark Application as Editable").Attributes.Add("style", "display:none");
                            }
                        }
                        else if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                        {
                            Rdserachby.Visible = false;
                            tdradio.Visible = false;
                        }
                        hlklink.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/ExamFormPreview.aspx?ID=" + cea.CourseID + "&Appid=" + cea.ID + "&Dob=" + cea.Candidate.DateOfBirth + "&candidateID=" + cea.CandidateID + "&Type=Print");
                        hlklink.Target = "_blank";
                    }

                }

            };

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private string FullCorAddressCourseExamApplication(Address add)
    {
        string str = add.AddressLine1;
        if (add.AddressLine2 != null)
            if (add.AddressLine2.Trim().Length > 1)
                str += ", " + add.AddressLine2.Trim().Trim(',');
        if (add.AddressLine3 != null)
            if (add.AddressLine3.Trim().Length > 1)
                str += ", " + add.AddressLine3.Trim().Trim(',');
        if (add.CityName != null)
            str += "<br>" + add.CityName;
        if (add.DistrictID.HasValue)
            str += "<br/> District:- " + add.District.Name + ", ";
        if (add.State.Name != null)
            str += "<br/> State:- " + add.State.Name;
        if (add.PinCode.HasValue)
            str += ",&nbsp; Pin:- " + add.PinCode.Value.ToString();
        return str;
    }
    private string FullPermanentAddressCourseExamApplication(Address add)
    {
        string str = add.AddressLine1;
        if (add.AddressLine2 != null)
            if (add.AddressLine2.Trim().Length > 1)
                str += ", " + add.AddressLine2.Trim().Trim(',');
        if (add.AddressLine3 != null)
            if (add.AddressLine3.Trim().Length > 1)
                str += ", " + add.AddressLine3.Trim().Trim(',');
        if (add.CityName != null)
            str += "<br>" + add.CityName;
        if (add.DistrictID.HasValue)
            str += "<br/> District:- " + add.District.Name + ", ";
        if (add.State.Name != null)
            str += "<br/> State:- " + add.State.Name;
        if (add.PinCode.HasValue)
            str += ",&nbsp; Pin:- " + add.PinCode.Value.ToString();
        return str;
    }
    private String FullCoAddressExam(CertificateExamApplication cr)
    {
        string str = CommonFunctions.GetInitCap(cr.CorAddressLine1);
        if (cr.CorAddressLine2 != null)
            str += "<br>" + CommonFunctions.GetInitCap(cr.CorAddressLine2);
        if (cr.CorAddressLine3 != null)
            str += "<br>" + CommonFunctions.GetInitCap(cr.CorAddressLine3);
        if (cr.CorCityName != null)
            str += "<br>" + CommonFunctions.GetInitCap(cr.CorCityName);
        if (cr.CorDistrictID.HasValue && cr.CorStateID != 0)
            str += "<br>District: " + CommonFunctions.GetInitCap(cr.CorDistrict.Name) + ", " + CommonFunctions.GetInitCap(cr.CorState.Name);
        if (cr.CorPinCode != 0)
            str += "<br>Pin: " + cr.CorPinCode.ToString();
        return str;
    }
    private String FullPerAddress(CourseRegistrationApplication crs)
    {
        string str = CommonFunctions.GetInitCap(crs.PerAddressLine1);
        if (crs.PerAddressLine2 != null)
            str += "<br>" + CommonFunctions.GetInitCap(crs.PerAddressLine2);
        if (crs.PerAddressLine3 != null)
            str += "<br>" + CommonFunctions.GetInitCap(crs.PerAddressLine3);
        if (crs.PerCityName != null)
            str += "<br>" + CommonFunctions.GetInitCap(crs.PerCityName);
        if (crs.PerDistrictID.HasValue && crs.PerStateID != 0)
            str += "<br>District: " + CommonFunctions.GetInitCap(crs.PerDistrict.Name) + ", " + CommonFunctions.GetInitCap(crs.PerState.Name);
        if (crs.PerPinCode != 0)
            str += "<br>Pin: " + crs.PerPinCode.ToString();
        return str;
    }
    private String FullCoAddress(CourseRegistrationApplication crs)
    {
        string str = CommonFunctions.GetInitCap(crs.CorAddressLine1);
        if (crs.CorAddressLine2 != null)
            str += "<br>" + CommonFunctions.GetInitCap(crs.CorAddressLine2);
        if (crs.CorAddressLine3 != null)
            str += "<br>" + CommonFunctions.GetInitCap(crs.CorAddressLine3);
        if (crs.CorCityName != null)
            str += "<br>" + CommonFunctions.GetInitCap(crs.CorCityName);
        if (crs.CorDistrictID.HasValue && crs.CorStateID != 0)
            str += "<br>District: " + CommonFunctions.GetInitCap(crs.CorDistrict.Name) + ", " + CommonFunctions.GetInitCap(crs.CorState.Name);
        if (crs.CorPinCode != 0)
            str += "<br>Pin: " + crs.PerPinCode.ToString();
        return str;
    }
    protected void ToggleImage(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton btn = (ImageButton)sender;
            string arg = btn.CommandArgument.ToLower();
            using (EConnectContext context = new EConnectContext())
            {
                BatchItem batchItem = new BatchItem();
                CertificateExamApplication cr = new CertificateExamApplication();
                CourseRegistrationApplication crs = new CourseRegistrationApplication();
                CourseExamApplication cea = new CourseExamApplication();
                Int32 applicationTypeID = Convert.ToInt32(Request.QueryString["ApplicationTypeID"].ToString());
                if (applicationTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    string applicationNo = Request.QueryString["Appno"].ToString();
                    cr = context.CertificateExamApplications.Where(s => s.Number.ToUpper() == applicationNo.ToUpper()).FirstOrDefault();
                    if (arg == "p")
                    {
                        if (LblPhotoCaption.Text.ToLower() == "signature")
                        {
                            LblPhotoCaption.Text = "Photograph";
                            ImgBtnPrevious.Enabled = false;
                            ImgBtnNext.Enabled = true;
                        }
                        else if (LblPhotoCaption.Text.ToLower() == "thumb")
                        {
                            LblPhotoCaption.Text = "Signature";
                            ImgBtnPrevious.Enabled = true;
                            ImgBtnNext.Enabled = true;
                        }
                    }
                    else
                    {
                        if (LblPhotoCaption.Text.ToLower() == "photograph")
                        {
                            LblPhotoCaption.Text = "Signature";
                            ImgBtnPrevious.Enabled = true;
                            ImgBtnNext.Enabled = true;
                        }
                        else if (LblPhotoCaption.Text.ToLower() == "signature")
                        {
                            LblPhotoCaption.Text = "Thumb";
                            ImgBtnNext.Enabled = false;
                            ImgBtnPrevious.Enabled = true;
                        }
                    }
                    ImgCandidatePhoto.ImageUrl = "../images/photo.jpg";
                    ImgCandidatePhoto.Height = 130;
                    ImgCandidatePhoto.Width = 112;
                    if (LblPhotoCaption.Text.ToLower() == "photograph")
                    {
                        ImgBtnPrevious.ToolTip = "";
                        ImgBtnNext.ToolTip = "Click to view signature.";
                        if (!string.IsNullOrEmpty(cr.PhotoFileName))
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cr.Photo);
                    }
                    if (LblPhotoCaption.Text.ToLower() == "signature")
                    {
                        ImgBtnPrevious.ToolTip = "Click to view photograph";
                        ImgBtnNext.ToolTip = "Click to view left thumb impression.";
                        if (!string.IsNullOrEmpty(cr.SignatureFileName))
                        {
                            ImgCandidatePhoto.Height = 50;
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cr.Signature);
                        }
                    }
                    else if (LblPhotoCaption.Text.ToLower() == "thumb")
                    {
                        ImgBtnPrevious.ToolTip = "Click to view signature";
                        ImgBtnNext.ToolTip = "";
                        if (!string.IsNullOrEmpty(cr.LeftThumbFileName))
                        {
                            ImgCandidatePhoto.Height = 60;
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cr.LeftThumb);
                        }
                    }
                }
                else if (applicationTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {

                    string applicationNo = Request.QueryString["Appno"].ToString();
                    crs = context.CourseRegistrationApplications.Where(s => s.Number.ToUpper() == applicationNo.ToUpper()).FirstOrDefault();
                    if (arg == "p")
                    {
                        if (LblPhotoCaption.Text.ToLower() == "signature")
                        {
                            LblPhotoCaption.Text = "Photograph";
                            ImgBtnPrevious.Enabled = false;
                            ImgBtnNext.Enabled = true;
                        }
                        else if (LblPhotoCaption.Text.ToLower() == "thumb")
                        {
                            LblPhotoCaption.Text = "Signature";
                            ImgBtnPrevious.Enabled = true;
                            ImgBtnNext.Enabled = true;
                        }
                    }
                    else
                    {
                        if (LblPhotoCaption.Text.ToLower() == "photograph")
                        {
                            LblPhotoCaption.Text = "Signature";
                            ImgBtnPrevious.Enabled = true;
                            ImgBtnNext.Enabled = true;
                        }
                        else if (LblPhotoCaption.Text.ToLower() == "signature")
                        {
                            LblPhotoCaption.Text = "Thumb";
                            ImgBtnNext.Enabled = false;
                            ImgBtnPrevious.Enabled = true;
                        }
                    }
                    ImgCandidatePhoto.ImageUrl = "../images/photo.jpg";
                    ImgCandidatePhoto.Height = 130;
                    ImgCandidatePhoto.Width = 112;
                    if (LblPhotoCaption.Text.ToLower() == "photograph")
                    {
                        ImgBtnPrevious.ToolTip = "";
                        ImgBtnNext.ToolTip = "Click to view signature.";
                        if (!string.IsNullOrEmpty(crs.PhotoFileName))
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])crs.Photo);
                    }
                    if (LblPhotoCaption.Text.ToLower() == "signature")
                    {
                        ImgBtnPrevious.ToolTip = "Click to view photograph";
                        ImgBtnNext.ToolTip = "Click to view left thumb impression.";
                        if (!string.IsNullOrEmpty(crs.SignatureFileName))
                        {
                            ImgCandidatePhoto.Height = 50;
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])crs.Signature);
                        }
                    }
                    else if (LblPhotoCaption.Text.ToLower() == "thumb")
                    {
                        ImgBtnPrevious.ToolTip = "Click to view signature";
                        ImgBtnNext.ToolTip = "";
                        if (!string.IsNullOrEmpty(crs.LeftThumbFileName))
                        {
                            ImgCandidatePhoto.Height = 60;
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])crs.LeftThumb);
                        }
                    }
                }
                else if (applicationTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {

                    string applicationNo = Request.QueryString["Appno"].ToString();
                    cea = context.CourseExamApplications.Where(s => s.Number.ToUpper() == applicationNo.ToUpper()).FirstOrDefault();
                    Candidate cand = cea.Candidate;
                    if (arg == "p")
                    {
                        if (LblPhotoCaption.Text.ToLower() == "signature")
                        {
                            LblPhotoCaption.Text = "Photograph";
                            ImgBtnPrevious.Enabled = false;
                            ImgBtnNext.Enabled = true;
                        }
                        else if (LblPhotoCaption.Text.ToLower() == "thumb")
                        {
                            LblPhotoCaption.Text = "Signature";
                            ImgBtnPrevious.Enabled = true;
                            ImgBtnNext.Enabled = true;
                        }
                    }
                    else
                    {
                        if (LblPhotoCaption.Text.ToLower() == "photograph")
                        {
                            LblPhotoCaption.Text = "Signature";
                            ImgBtnPrevious.Enabled = true;
                            ImgBtnNext.Enabled = true;
                        }
                        else if (LblPhotoCaption.Text.ToLower() == "signature")
                        {
                            LblPhotoCaption.Text = "Thumb";
                            ImgBtnNext.Enabled = false;
                            ImgBtnPrevious.Enabled = true;
                        }
                    }
                    ImgCandidatePhoto.ImageUrl = "../images/photo.jpg";
                    ImgCandidatePhoto.Height = 130;
                    ImgCandidatePhoto.Width = 112;
                    if (LblPhotoCaption.Text.ToLower() == "photograph")
                    {
                        ImgBtnPrevious.ToolTip = "";
                        ImgBtnNext.ToolTip = "Click to view signature.";
                        if (cand.Photo != null)
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cand.Photo.BlobFile);
                    }
                    if (LblPhotoCaption.Text.ToLower() == "signature")
                    {
                        ImgBtnPrevious.ToolTip = "Click to view photograph";
                        ImgBtnNext.ToolTip = "Click to view left thumb impression.";
                        if (cand.Signature != null)
                        {
                            ImgCandidatePhoto.Height = 50;
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cand.Signature.BlobFile);
                        }
                    }
                    else if (LblPhotoCaption.Text.ToLower() == "thumb")
                    {
                        ImgBtnPrevious.ToolTip = "Click to view signature";
                        ImgBtnNext.ToolTip = "";
                        if (cand.LeftThumbImpression != null)
                        {
                            ImgCandidatePhoto.Height = 60;
                            ImgCandidatePhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])cand.LeftThumbImpression.BlobFile);
                        }
                    }



                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void Btnback_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            BreadCrumb1.RemoveLastBreadCrumbItem();
            BreadCrumb1.RemoveLastBreadCrumbItem();
            using (EConnectContext context = new EConnectContext())
            {
                string origin = "";
                CourseExamApplication cea = new CourseExamApplication();
                CertificateExamApplication cr = new CertificateExamApplication();
                CourseRegistrationApplication crs = new CourseRegistrationApplication();
                Int32 key = Convert.ToInt32(Request.QueryString["key"].ToString());
                Int32 examID = Convert.ToInt32(Request.QueryString["examID"].ToString());
                Int32 applicationTypeID = Convert.ToInt32(Request.QueryString["ApplicationTypeID"].ToString());
                if (!string.IsNullOrEmpty(Request.QueryString["Src"]))
                    origin = Request.QueryString["Src"].ToString();
                if (applicationTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    string applicationNo = Request.QueryString["Appno"].ToString();
                    cr = context.CertificateExamApplications.Where(s => s.Number.ToUpper() == applicationNo.ToUpper()).FirstOrDefault();
                    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("FrmApplicationReciept.aspx?key=" + Request.QueryString["key"] + "&examID=" + Request.QueryString["examID"] + "&categoryid=" + cr.CourseCategoryID + "&courseid=" + cr.CourseID + "&examyear=" + cr.Exam.ExamYear + "&examcycle=" + cr.Exam.ExaminationCycleID + "&paymentStatus=" + cr.PaymentStatusID), true);
                }
                else if (applicationTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    string applicationNo = Request.QueryString["Appno"].ToString();
                    cea = context.CourseExamApplications.Where(s => s.Number.ToUpper() == applicationNo.ToUpper()).FirstOrDefault();
                    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("FrmCourseExamApplications.aspx?key=" + Request.QueryString["key"] + "&examID=" + Request.QueryString["examID"] + "&categoryid=" + cea.CourseCategoryID + "&courseid=" + cea.CourseID + "&examyear=" + cea.Exam.ExamYear + "&paymentStatus=" + cea.PaymentStatusID), true);
                }
                else if (applicationTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    string applicationNo = Request.QueryString["Appno"].ToString();
                    crs = context.CourseRegistrationApplications.Where(s => s.Number.ToUpper() == applicationNo.ToUpper()).FirstOrDefault();
                    if (origin.Trim().ToUpper() == "CRS")
                    {
                        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("SearchCourseRegistrationApplication.aspx?key=" + Request.QueryString["key"] + "&examID=" + Request.QueryString["examID"] + "&categoryid=" + crs.CourseCategoryID + "&courseid=" + crs.CourseID + "&examyear=" + crs.ApplicableExam.ExamYear + "&regtype=" + crs.RegistrationTypeID), true);
                    }
                    else
                    {
                        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("frmCourseRegistrationApplication.aspx?key=" + Request.QueryString["key"] + "&examID=" + Request.QueryString["examID"] + "&categoryid=" + crs.CourseCategoryID + "&courseid=" + crs.CourseID + "&examyear=" + crs.ApplicableExam.ExamYear + "&regtype=" + crs.RegistrationTypeID + "&paymentStatus=" + crs.PaymentStatusID), true);
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void lnkchangeapptype_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            lbfilter.Text = "Are you sure you want to change the applicant type of the candidate";
            divradio.Visible = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void Rdserachby_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            using (EConnectContext context = new EConnectContext())
            {
                divoutput.Visible = false;
                CourseExamApplication cea = new CourseExamApplication();
                Int32 applicationTypeID = Convert.ToInt32(Request.QueryString["ApplicationTypeID"].ToString());
                if (applicationTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    string applicationNo = Request.QueryString["Appno"].ToString();
                    cea = context.CourseExamApplications.Where(s => s.Number.ToUpper() == applicationNo.ToUpper()).FirstOrDefault();
                    if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate) && (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedbyInstitute)))
                    {
                        if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate) && (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedbyInstitute)) && CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.HasValue ? cea.DemandNoteID.Value : 0))
                        {
                            Rdserachby.Visible = true;
                            if (cea.PaymentSourceChanged == true)
                            {
                                Rdserachby.Items.FindByText("Change Payment Option").Attributes.Add("style", "display:none");
                            }
                        }
                        else if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate) && (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedbyInstitute)))
                        {
                            Rdserachby.Visible = true;
                            Rdserachby.Items.FindByText("Change Payment Option").Attributes.Add("style", "display:none");
                        }
                    }
                    else if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Institute) && cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification))
                    {
                        Rdserachby.Visible = true;
                        tdradio.Visible = true;
                        if (cea.PaymentSourceChanged == true)
                        {
                            Rdserachby.Items.FindByText("Change Payment Option").Attributes.Add("style", "display:none");
                        }
                    }
                    else if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct) && cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) && cea.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending) && CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.HasValue ? cea.DemandNoteID.Value : 0))
                    {
                        Rdserachby.Visible = true;
                        tdradio.Visible = true;
                        Rdserachby.Items.FindByText("Change Payment Option").Attributes.Add("style", "display:none");
                        Rdserachby.Items.FindByText("Change Applicant Type").Attributes.Add("style", "display:none");
                    }
                }
                if (Rdserachby.SelectedValue == "1")
                {
                    lbfilter.Text = "Are you sure you want to change the payment option of the candidate?<br/> Payment option once changed you cannot revert back the payment option again.";
                    divradio.Visible = true;
                }
                else if (Rdserachby.SelectedValue == "2")
                {
                    lbfilter.Text = "Are you sure you want to change the applicant type of the candidate?<br/> Applicant Type once changed you cannot revert back the applicant type again.";
                    divradio.Visible = true;
                }
                else if (Rdserachby.SelectedValue == "3")
                {
                    lbfilter.Text = "Are you sure you want to mark the application as editable/cancelled.";
                    divradio.Visible = true;
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);

        }
    }
    protected void Btnyes_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            string Msg = "";
            string emailAddress = "";
            //Int64 demandID = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    CourseExamApplication cea = new CourseExamApplication();
                    Int32 applicationTypeID = Convert.ToInt32(Request.QueryString["ApplicationTypeID"].ToString());
                    if (applicationTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    {
                        string applicationNo = Request.QueryString["Appno"].ToString();
                        cea = context.CourseExamApplications.Where(s => s.Number.ToUpper() == applicationNo.ToUpper()).FirstOrDefault();
                        ICollection<CandidateContactDetail> contactDetails = cea.Candidate.ContactDetails;
                        if (contactDetails != null)
                        {
                            CandidateContactDetail cd = contactDetails.OrderByDescending(d => d.EffectiveFromDate).FirstOrDefault();
                            emailAddress = cd.EmailAddress;
                        }
                        DemandNote demand = new DemandNote();
                        // code when applicant type is institute and payment source is Institute
                        if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Institute) && (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute)) && cea.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending))
                        {
                            if (Rdserachby.SelectedValue == "1")//Change Payment Option to CAnaidate
                            {
                                if (cea.PaymentSourceChanged == false)
                                {
                                    demand.ApplicationDate = DateTime.Now;
                                    demand.FeeTypeID = cea.FeeTypeID.Value;
                                    demand.ApplicationTypeID = Convert.ToInt32(enmApplicationType.CourseExamApplication);
                                    demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.CSCSPV);
                                    demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Single);
                                    demand.Amount = cea.FeeAmount;
                                    demand.CreatedBy = 1;
                                    demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                                    demand.CourseCategoryID = cea.CourseCategoryID;
                                    demand.CourseID = cea.CourseID;
                                    demand.ServiceID = cea.Course.ExaminationServiceID;
                                    context.DemandNotes.Add(demand);
                                    context.SaveChanges();

                                    cea.DemandNoteID = demand.ID;
                                    cea.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate);
                                    cea.PaymentSourceID = Convert.ToInt32(enmPaymentSource.Candidate);
                                    cea.PaymentSourceChanged = true;
                                    cea.PaymentSourceChangedBy = Convert.ToInt32(Session["UserID"]);
                                    cea.PaymentSourceChangedOn = DateTime.Now;

                                    context.Entry(cea).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();

                                    scope.Complete();

                                    Msg = "Dear " + GetInitCap(cea.Candidate.Salutation + " " + cea.Candidate.Name) + ",<br/><br/>" + "<br><br>Your payment option has been changed successfully on your request from institute to candidate on " + cea.PaymentSourceChangedOn.Value.ToString("dd-MMM-yyyy hh:mm") + ". and  Your application no is." + " " + cea.Number + "Important: Please refer your Demand Note Number mentioned in the form at the time of payment at CSC/SPV. Your Demand Note Number is:- " + cea.DemandNote.ID.ToString();
                                    try
                                    {
                                        if (emailAddress.Trim().Length > 0)
                                        {
                                            //sending Email 
                                            EConnect.NIELIT.Email mail = new Email("Change Payment Option:NIELIT", Msg, emailAddress);
                                            mail.Send();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ShowAlert(ex.Message);
                                    }
                                    lboutput.Text = "Payment option of the candidate has been successfully changed...";
                                    divoutput.Visible = true;
                                }
                                else
                                {
                                    ShowAlert("Payment option for this application:- " + cea.Number + " has already been changed", true);
                                    return;
                                }

                            }
                            else if (Rdserachby.SelectedValue == "2")//Change Applicant Type
                            {

                                if (cea.ApplicantTypeChanged == false)
                                {
                                    demand.ApplicationDate = DateTime.Now;
                                    demand.FeeTypeID = cea.FeeTypeID.Value;
                                    demand.ApplicationTypeID = Convert.ToInt32(enmApplicationType.CourseExamApplication);
                                    demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.CSCSPV);
                                    demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Single);
                                    demand.Amount = cea.FeeAmount;
                                    demand.CreatedBy = 1;
                                    demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                                    demand.CourseCategoryID = cea.CourseCategoryID;
                                    demand.CourseID = cea.CourseID;
                                    demand.ServiceID = cea.Course.ExaminationServiceID;
                                    context.DemandNotes.Add(demand);
                                    context.SaveChanges();

                                    cea.DemandNoteID = demand.ID;
                                    cea.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate);
                                    cea.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Direct);
                                    cea.InstituteID = null;
                                    cea.PaymentSourceID = Convert.ToInt32(enmPaymentSource.Candidate);
                                    cea.ApplicantTypeChanged = true;
                                    cea.ApplicantTypeChangedBy = Convert.ToInt32(Session["UserID"]);
                                    cea.ApplicantTypeChangedOn = DateTime.Now;


                                    context.Entry(cea).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();

                                    scope.Complete();

                                    Msg = "Dear " + GetInitCap(cea.Candidate.Salutation + " " + cea.Candidate.Name) + ",<br/><br/>" + "Your applicant type has been changed successfully on your request from Institute to Direct on " + cea.ApplicantTypeChangedOn.Value.ToString("dd-MMM-yyyy hh:mm") + ". and Your application no is." + " " + cea.Number + "Important: Please refer your Demand Note Number mentioned in the form at the time of payment at CSC/SPV. Your Demand Note Number is:- " + cea.DemandNote.ID.ToString();
                                    try
                                    {
                                        if (emailAddress.Trim().Length > 0)
                                        {
                                            //sending Email 
                                            EConnect.NIELIT.Email mail = new Email("Change Applicant Type:NIELIT", Msg, emailAddress);
                                            mail.Send();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ShowAlert(ex.Message);
                                    }
                                    lboutput.Text = "Applicant Type of the candidate has been successfully changed...";
                                    divoutput.Visible = true;
                                }
                                else
                                {
                                    ShowAlert("Applicant Type for this application:- " + cea.Number + " has already been changed", true);
                                    return;
                                }
                            }
                            else if (Rdserachby.SelectedValue == "3")//Mark Appplication as Editable
                            {

                                cea.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification);
                                cea.FinalSubmitted = false;
                                cea.FinalSubmissionDate = null;
                                context.Entry(cea).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();

                                Msg = "Dear " + GetInitCap(cea.Candidate.Salutation + " " + cea.Candidate.Name) + ",<br/><br/>Your" + cea.Exam.Name +
                               " (" + cea.Course.Name + ") Examination application has been successsfully marked as editable on your request on " + DateTime.Now.ToString("dd-MMM-yyy hh:mm:ss") + ". Your application number is:" + " " + cea.Number;

                                scope.Complete();
                                try
                                {
                                    if (emailAddress.Trim().Length > 0)
                                    {
                                        //sending Email 
                                        EConnect.NIELIT.Email mail = new Email("Mark Application as Editable/ Cancelled :NIELIT", Msg, emailAddress);
                                        mail.Send();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ShowAlert(ex.Message);
                                }

                                lboutput.Text = "Application with the Application number:" + cea.Number + " has been successfully marked as editable/cancelled...";
                                divoutput.Visible = true;

                            }

                        }
                        // code when applicant type is institute and payment source is candidate
                        else if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate))
                        {
                            if (Rdserachby.SelectedValue == "1")//Change Payment Option to Institute
                            {
                                if (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) && CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.HasValue ? cea.DemandNoteID.Value : 0) && cea.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending))
                                {
                                    if (cea.PaymentSourceChanged == false)
                                    {
                                        demand = context.DemandNotes.Find(cea.DemandNoteID);
                                        if (demand != null)
                                        {

                                            // delete from DemandDraftTransactions
                                            //var demanddraft = from r in context.DemandDraftTransactions
                                            //                  where r.DemandNoteID == demand.ID
                                            //                  select r;
                                            //if (demanddraft != null)
                                            //{
                                            //    demanddraft.ToList().ForEach(d => context.DemandDraftTransactions.Remove(d));
                                            //    context.SaveChanges();
                                            //}
                                            //// delete from CSCTransactions
                                            //var CSC = from r in context.CSCTransactions
                                            //          where r.DemandNoteID == demand.ID
                                            //          select r;
                                            //if (CSC != null)
                                            //{
                                            //    CSC.ToList().ForEach(d => context.CSCTransactions.Remove(d));
                                            //    context.SaveChanges();
                                            //}
                                            //// delete from OnlineTransaction
                                            //var online = from r in context.OnlineTransaction
                                            //             where r.DemandNoteID == demand.ID
                                            //             select r;
                                            //if (online != null)
                                            //{
                                            //    online.ToList().ForEach(d => context.OnlineTransaction.Remove(d));
                                            //    context.SaveChanges();
                                            //}
                                            //// delete from NEFTTransactions
                                            //var neft = from r in context.NEFTTransactions
                                            //           where r.DemandNoteID == demand.ID
                                            //           select r;
                                            //if (neft != null)
                                            //{
                                            //    neft.ToList().ForEach(d => context.NEFTTransactions.Remove(d));
                                            //    context.SaveChanges();
                                            //}

                                            cea.PaymentSourceID = Convert.ToInt32(enmPaymentSource.Institute);
                                            cea.PaymentSourceChanged = true;
                                            cea.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification);
                                            cea.PaymentSourceChangedBy = Convert.ToInt32(Session["UserID"]);
                                            cea.PaymentSourceChangedOn = DateTime.Now;
                                            if (CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.Value))
                                            {
                                                cea.DemandNoteID = null;
                                            }
                                            context.Entry(cea).State = System.Data.Entity.EntityState.Modified;
                                            context.SaveChanges();

                                            //context.DemandNotes.Remove(demand);
                                            //context.SaveChanges();

                                            scope.Complete();

                                            Msg = "Dear " + GetInitCap(cea.Candidate.Salutation + " " + cea.Candidate.Name) + ",<br/><br/>" + "<br><br>Your payment option has been changed successfully on your request from candidate to institute on " + cea.PaymentSourceChangedOn.Value.ToString("dd-MMM-yyyy hh:mm") + ". and  Your application no is." + " " + cea.Number;
                                            try
                                            {
                                                if (emailAddress.Trim().Length > 0)
                                                {
                                                    //sending Email 
                                                    EConnect.NIELIT.Email mail = new Email("Change Payment Option:NIELIT", Msg, emailAddress);
                                                    mail.Send();
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                ShowAlert(ex.Message);
                                            }
                                            lboutput.Text = "Payment option of the candidate has been successfully changed...";
                                            divoutput.Visible = true;
                                        }
                                        else
                                        {
                                            ShowAlert("DemandNote not found for this application:-" + cea.Number + ", payment option cannot be changed", true);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        ShowAlert("Payment option for this application:- " + cea.Number + " has already been changed", true);
                                        return;
                                    }

                                }
                                else
                                {
                                    ShowAlert("Payment option for this application:- " + cea.Number + " cannot be changed because the candidate has already made the payment request.", true);
                                    return;
                                }
                            }
                            else if (Rdserachby.SelectedValue == "2")//Change applicant Type
                            {
                                // change applicant type when applicant type is institute and payment source is candidate
                                if (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate))
                                {
                                    if (cea.ApplicantTypeChanged == false)
                                    {
                                        cea.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Direct);
                                        cea.InstituteID = null;
                                        cea.ApplicantTypeChanged = true;
                                        cea.ApplicantTypeChangedOn = DateTime.Now;
                                        cea.ApplicantTypeChangedBy = Convert.ToInt32(Session["UserID"]);
                                        context.Entry(cea).State = System.Data.Entity.EntityState.Modified;
                                        context.SaveChanges();
                                        scope.Complete();

                                        Msg = "Dear " + GetInitCap(cea.Candidate.Salutation + " " + cea.Candidate.Name) + ",<br/><br/>" + " Your applicant type has been changed successfully on your request from Institute to Direct on " + cea.ApplicantTypeChangedOn.Value.ToString("dd-MMM-yyyy hh:mm") + "and Your application no is." + " " + cea.Number;
                                        try
                                        {
                                            if (emailAddress.Trim().Length > 0)
                                            {
                                                //sending Email 
                                                EConnect.NIELIT.Email mail = new Email("Applicant Type Change:NIELIT", Msg, emailAddress);
                                                mail.Send();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ShowAlert(ex.Message);
                                        }
                                        lboutput.Text = "Applicant type of the candidate has been successfully changed....";
                                        divoutput.Visible = true;
                                    }
                                    else
                                    {
                                        ShowAlert("Applicant type for this application:- " + cea.Number + " has already been changed", true);
                                        return;
                                    }
                                }
                                else if (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification))
                                {
                                    if (cea.ApplicantTypeChanged == false)
                                    {
                                        cea.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Direct);
                                        cea.InstituteID = null;
                                        cea.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
                                        cea.ApplicantTypeChanged = true;
                                        cea.ApplicantTypeChangedOn = DateTime.Now;
                                        cea.ApplicantTypeChangedBy = Convert.ToInt32(Session["UserID"]);
                                        context.Entry(cea).State = System.Data.Entity.EntityState.Modified;
                                        context.SaveChanges();
                                        scope.Complete();

                                        Msg = "Dear " + GetInitCap(cea.Candidate.Salutation + " " + cea.Candidate.Name) + ",<br/><br/>" + " Your applicant type has been changed successfully on your request from Institute to Direct on " + cea.ApplicantTypeChangedOn.Value.ToString("dd-MMM-yyyy hh:mm") + "and Your application no is." + " " + cea.Number;
                                        try
                                        {
                                            if (emailAddress.Trim().Length > 0)
                                            {
                                                //sending Email 
                                                EConnect.NIELIT.Email mail = new Email("Applicant Type Change:NIELIT", Msg, emailAddress);
                                                mail.Send();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ShowAlert(ex.Message);
                                        }
                                        lboutput.Text = "Applicant type of the candidate has been successfully changed....";
                                        divoutput.Visible = true;
                                    }
                                    else
                                    {
                                        ShowAlert("Applicant type for this application:- " + cea.Number + " has already been changed", true);
                                        return;
                                    }
                                }
                            }
                            else if (Rdserachby.SelectedValue == "3") // Mark appplication as editable
                            {
                                // Mark appplication as editable when applicant type is institute and payment source is candidate
                                if ((cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationRejectedbyInstitute)) && cea.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending) && CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.HasValue ? cea.DemandNoteID.Value : 0))
                                {
                                    demand = context.DemandNotes.Find(cea.DemandNoteID);
                                    if (demand != null)
                                    {
                                        // delete from DemandDraftTransactions
                                        //var demanddraft = from r in context.DemandDraftTransactions
                                        //                  where r.DemandNoteID == demand.ID
                                        //                  select r;
                                        //if (demanddraft != null)
                                        //{
                                        //    demanddraft.ToList().ForEach(d => context.DemandDraftTransactions.Remove(d));
                                        //    context.SaveChanges();
                                        //}
                                        //// delete from CSCTransactions
                                        //var CSC = from r in context.CSCTransactions
                                        //          where r.DemandNoteID == demand.ID
                                        //          select r;
                                        //if (CSC != null)
                                        //{
                                        //    CSC.ToList().ForEach(d => context.CSCTransactions.Remove(d));
                                        //    context.SaveChanges();
                                        //}
                                        //// delete from OnlineTransaction
                                        //var online = from r in context.OnlineTransaction
                                        //             where r.DemandNoteID == demand.ID
                                        //             select r;
                                        //if (online != null)
                                        //{
                                        //    online.ToList().ForEach(d => context.OnlineTransaction.Remove(d));
                                        //    context.SaveChanges();
                                        //}
                                        //// delete from NEFTTransactions
                                        //var neft = from r in context.NEFTTransactions
                                        //           where r.DemandNoteID == demand.ID
                                        //           select r;
                                        //if (neft != null)
                                        //{
                                        //    neft.ToList().ForEach(d => context.NEFTTransactions.Remove(d));
                                        //    context.SaveChanges();
                                        //}

                                        cea.FinalSubmitted = false;
                                        cea.FinalSubmissionDate = null;
                                        if (CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.Value))
                                        {
                                            cea.DemandNoteID = null;
                                        }
                                        context.Entry(cea).State = System.Data.Entity.EntityState.Modified;
                                        context.SaveChanges();

                                        //context.DemandNotes.Remove(demand);
                                        //context.SaveChanges();

                                        Msg = "Dear " + GetInitCap(cea.Candidate.Salutation + " " + cea.Candidate.Name) + ",<br/><br/>Your" + cea.Exam.Name +
                                       " (" + cea.Course.Name + ") Examination application has been successsfully marked as editable on your request on " + DateTime.Now.ToString("dd-MMM-yyy hh:mm:ss") + ". Your application number is:" + " " + cea.Number;

                                        scope.Complete();

                                        try
                                        {
                                            if (emailAddress.Trim().Length > 0)
                                            {
                                                //sending Email 
                                                EConnect.NIELIT.Email mail = new Email("Mark Application as Editable/ Cancelled :NIELIT", Msg, emailAddress);
                                                mail.Send();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ShowAlert(ex.Message);
                                        }

                                        lboutput.Text = "Application with the Application number:-" + cea.Number + " has been successfully marked as editable/cancelled...";
                                        divoutput.Visible = true;
                                    }
                                }
                                else
                                {
                                    String transactionNo = "";
                                    demand = context.DemandNotes.Find(cea.DemandNoteID);
                                    if (demand.enmPaymentMode == enmPaymentMode.DemandDraft)
                                        transactionNo = demand.DemandDraftTransaction.DemandDraftNumber.ToString();
                                    else if (demand.enmPaymentMode == enmPaymentMode.CSCSPV)
                                        transactionNo = demand.CSCTransaction.ResponseTransactionNumber.ToString();
                                    else if (demand.enmPaymentMode == enmPaymentMode.NEFTRTGS)
                                        transactionNo = demand.NEFTTransaction.TransactionNumber.ToString();
                                    else if (demand.enmPaymentMode == enmPaymentMode.Online)
                                        transactionNo = demand.OnlineTransaction.ReferenceNumber.ToString();

                                    ShowAlert("You cannot mark this application as editable/cancelled because payment is already made by the candidate using " + demand.PaymentMode.Name + "  payment mode with transaction number : " + transactionNo, true);
                                    return;
                                }

                            }
                        }
                        else if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct) && cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate))
                        {
                            if (Rdserachby.SelectedValue == "3") // Mark appplication as editable
                            {
                                // Mark appplication as editable when applicant type is direct
                                if (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) && cea.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending) && CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.HasValue ? cea.DemandNoteID.Value : 0))
                                {
                                    demand = context.DemandNotes.Find(cea.DemandNoteID);
                                    if (demand != null)
                                    {
                                        // delete from DemandDraftTransactions
                                        //var demanddraft = from r in context.DemandDraftTransactions
                                        //                  where r.DemandNoteID == demand.ID
                                        //                  select r;
                                        //if (demanddraft != null)
                                        //{
                                        //    demanddraft.ToList().ForEach(d => context.DemandDraftTransactions.Remove(d));
                                        //    context.SaveChanges();
                                        //}
                                        //// delete from CSCTransactions
                                        //var CSC = from r in context.CSCTransactions
                                        //          where r.DemandNoteID == demand.ID
                                        //          select r;
                                        //if (CSC != null)
                                        //{
                                        //    CSC.ToList().ForEach(d => context.CSCTransactions.Remove(d));
                                        //    context.SaveChanges();
                                        //}
                                        //// delete from OnlineTransaction
                                        //var online = from r in context.OnlineTransaction
                                        //             where r.DemandNoteID == demand.ID
                                        //             select r;
                                        //if (online != null)
                                        //{
                                        //    online.ToList().ForEach(d => context.OnlineTransaction.Remove(d));
                                        //    context.SaveChanges();
                                        //}
                                        //// delete from NEFTTransactions
                                        //var neft = from r in context.NEFTTransactions
                                        //           where r.DemandNoteID == demand.ID
                                        //           select r;
                                        //if (neft != null)
                                        //{
                                        //    neft.ToList().ForEach(d => context.NEFTTransactions.Remove(d));
                                        //    context.SaveChanges();
                                        //}

                                        cea.FinalSubmitted = false;
                                        cea.FinalSubmissionDate = null;
                                        if (CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.Value))
                                        {
                                            cea.DemandNoteID = null;
                                        }
                                        context.Entry(cea).State = System.Data.Entity.EntityState.Modified;
                                        context.SaveChanges();

                                        //context.DemandNotes.Remove(demand);
                                        //context.SaveChanges();

                                        Msg = "Dear " + GetInitCap(cea.Candidate.Salutation + " " + cea.Candidate.Name) + ",<br/><br/>Your" + cea.Exam.Name +
                                        " (" + cea.Course.Name + ") Examination application has been successsfully marked as editable on your request on " + DateTime.Now.ToString("dd-MMM-yyy hh:mm:ss") + ". Your application number is:" + " " + cea.Number;

                                        scope.Complete();
                                        try
                                        {
                                            if (emailAddress.Trim().Length > 0)
                                            {
                                                //sending Email 
                                                EConnect.NIELIT.Email mail = new Email("Mark Application as Editable/ Cancelled :NIELIT", Msg, emailAddress);
                                                mail.Send();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ShowAlert(ex.Message);
                                        }

                                        lboutput.Text = "Application with the Application number:-" + cea.Number + " has been successfully marked as editable/cancelled...";
                                        divoutput.Visible = true;
                                    }
                                }
                                else
                                {
                                    String transactionNo = "";
                                    demand = context.DemandNotes.Find(cea.DemandNoteID);
                                    if (demand.enmPaymentMode == enmPaymentMode.DemandDraft)
                                        transactionNo = demand.DemandDraftTransaction.DemandDraftNumber.ToString();
                                    else if (demand.enmPaymentMode == enmPaymentMode.CSCSPV)
                                        transactionNo = demand.CSCTransaction.ResponseTransactionNumber.ToString();
                                    else if (demand.enmPaymentMode == enmPaymentMode.NEFTRTGS)
                                        transactionNo = demand.NEFTTransaction.TransactionNumber.ToString();
                                    else if (demand.enmPaymentMode == enmPaymentMode.Online)
                                        transactionNo = demand.OnlineTransaction.ReferenceNumber.ToString();

                                    ShowAlert("You cannot mark this application as editable/cancelled because payment is already made by the candidate using " + demand.PaymentMode.Name + "  payment mode with transaction number : " + transactionNo, true);
                                    return;
                                }
                            }
                        }
                    }
                };
            };
            divradio.Visible = false;
            Rdserachby.ClearSelection();
            Bind();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void Btnno_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            using (EConnectContext context = new EConnectContext())
            {
                divradio.Visible = false;
                CourseExamApplication cea = new CourseExamApplication();
                Int32 applicationTypeID = Convert.ToInt32(Request.QueryString["ApplicationTypeID"].ToString());
                if (applicationTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    string applicationNo = Request.QueryString["Appno"].ToString();
                    cea = context.CourseExamApplications.Where(s => s.Number.ToUpper() == applicationNo.ToUpper()).FirstOrDefault();
                    if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate) && (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedbyInstitute)))
                    {
                        if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate) && (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedbyInstitute)) && CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.HasValue ? cea.DemandNoteID.Value : 0))
                        {
                            Rdserachby.Visible = true;
                            if (cea.PaymentSourceChanged == true)
                            {
                                Rdserachby.Items.FindByText("Change Payment Option").Attributes.Add("style", "display:none");
                            }
                        }
                        else if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate) && (cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) || cea.ApplicationStatusID == Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedbyInstitute)))
                        {
                            Rdserachby.Visible = true;
                            Rdserachby.Items.FindByText("Change Payment Option").Attributes.Add("style", "display:none");
                        }
                    }
                    else if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute) && cea.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Institute) && cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification))
                    {
                        Rdserachby.Visible = true;
                        tdradio.Visible = true;
                        if (cea.PaymentSourceChanged == true)
                        {
                            Rdserachby.Items.FindByText("Change Payment Option").Attributes.Add("style", "display:none");
                        }
                    }
                    else if (cea.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct) && cea.ApplicationStatusID == Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate) && cea.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending) && CommonFunctions.IsDemandNoteCancellable(cea.DemandNoteID.HasValue ? cea.DemandNoteID.Value : 0))
                    {
                        Rdserachby.Visible = true;
                        tdradio.Visible = true;
                        Rdserachby.Items.FindByText("Change Payment Option").Attributes.Add("style", "display:none");
                        Rdserachby.Items.FindByText("Change Applicant Type").Attributes.Add("style", "display:none");
                    }
                }
                Rdserachby.ClearSelection();
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnDemadNote_Click(object sender, EventArgs e)
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    string applicationNo = Request.QueryString["Appno"].ToString();
                    CourseExamApplication cea = context.CourseExamApplications.Where(s => s.Number.ToUpper() == applicationNo.ToUpper()).FirstOrDefault();
                    if (cea.DemandNoteID.HasValue == false)
                    {
                        DemandNote demand = new DemandNote();
                        demand.ApplicationDate = DateTime.Now;
                        demand.FeeTypeID = cea.FeeTypeID.Value;
                        demand.ApplicationTypeID = Convert.ToInt32(enmApplicationType.CourseExamApplication);
                        demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.CSCSPV);
                        demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Single);
                        demand.Amount = cea.FeeAmount;
                        demand.CreatedBy = 1;
                        demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                        context.DemandNotes.Add(demand);
                        context.SaveChanges();

                        cea.DemandNoteID = demand.ID;
                        cea.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButFeeNotDepositedByCandidate);
                        cea.PaymentSourceID = Convert.ToInt32(enmPaymentSource.Candidate);

                        context.Entry(cea).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        scope.Complete();
                        ShowAlert("Demand Note generated successfully. Demand Note number: " + demand.ID.ToString(), true);
                    }
                }
            };
            Bind();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}