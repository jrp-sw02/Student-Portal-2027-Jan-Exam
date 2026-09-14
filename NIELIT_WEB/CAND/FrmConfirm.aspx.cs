using System;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data.SqlClient;
using System.Web;
using System.Collections.Generic;

public partial class FrmConfirm : BasePage
{
    //EConnectContext context = new EConnectContext();
    Int64 appID = 0;
    int TypeID = 0;
    Int64 demandID = 0;
    Int64 candidateid = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
	// to prevent page from going back . 
            string script = @"
   	<script type='text/javascript'>
       history.pushState(null, null, location.href);
       window.onpopstate = function () {
           alert('Session out alert');
           window.location.href = 'https://student.nielit.gov.in';
       };
   	</script>";

            ClientScript.RegisterStartupScript(this.GetType(), "preventBack", script);
            //Response.Write(Request.QueryString.ToString());
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            if (Request.UrlReferrer == null)
          //  if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in") && (Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "https://nielit.gov.in"))
            {
                // Response.Write(GeInvalidRequestMessage("Goto Home Page", "../MainPage.aspx"));
                string urldecoded = HttpUtility.HtmlDecode(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
                Response.Write(urldecoded);
                Response.End();
                return;
            }
            else
            {
                appID = Convert.ToInt64(Request.QueryString["Appid"]);
                TypeID = Convert.ToInt32(Request.QueryString["TypeID"]);
                // if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["DemandID"])))
                demandID = Convert.ToInt64(Request.QueryString["DemandID"]); // added on 21 Jan 2025
                if (appID != 0 && TypeID != 0)
                {
                    if (TypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    {
                        using (EConnectContext context = new EConnectContext())
                        { candidateid = context.CourseExamApplications.Where(s => s.ID == appID).FirstOrDefault().CandidateID; }
                    }
                }
                if (TypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                    LnkBtnPrintForm.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("CertificatePreview.aspx?Appid=" + appID) + "');");
                else if (TypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                    LnkBtnPrintForm.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("FrmPreview.aspx?Appid=" + appID) + "');");
                else if (TypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    LnkBtnPrintForm.Attributes.Add("Onclick", "return showForm('" + EConnect.Utils.Security.QuertStringModule.Encrypt("ExamFormPreview.aspx?Appid=" + appID + "&candidateID=" + candidateid) + "');");
            }
            if (!IsPostBack)
            {
                BindPaymentOptions();
                MultiView1.Visible = true;
                MultiView1.ActiveViewIndex = 0;
                if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["TypeID"])))
                    TypeID = Convert.ToInt32(Request.QueryString["TypeID"]);
                if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["Appid"])))
                    appID = Convert.ToInt64(Request.QueryString["Appid"]);
                if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["DemandID"])))
                    demandID = Convert.ToInt64(Request.QueryString["DemandID"]);
                nPayNow.NavigateUrl = "";// = EConnect.Utils.Security.QuertStringModule.Encrypt("../OnlinePayment.aspx?DID=" + demandID.ToString());
                nPayNow.Attributes.Add("onclick", "window.top.location.href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../OnlinePayment.aspx?DID=" + demandID.ToString()) + "'");
                //Added for NEFT/RTGS
                nPayOffline.Attributes.Add("onclick", "window.top.location.href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../OfflinePayment.aspx?DID=" + demandID.ToString()) + "'");
                ShowData(appID, TypeID, demandID);
            }
            if (Request.QueryString["RU"] != null)
            {
                //LnkBtnBack.Visible = true;
            }
        }
        catch (Exception ex)
        {
            LblError.Text = ex.Message.ToString();
        }
    }
    public class PaymentType
    {
        public int ValueField { get; set; }
        public string TextField { get; set; }
    }
    private void BindPaymentModes(IEnumerable<PaymentType> paymentTypes)
    {

        if (paymentTypes == null)
        {
            paymentTypes = new List<PaymentType>();
        }
        RdoPaymentMode.DataSource = paymentTypes.ToList();
        RdoPaymentMode.DataValueField = "ValueField";
        RdoPaymentMode.DataTextField = "TextField";
        RdoPaymentMode.DataBind();

        if (RdoPaymentMode.Items.Count > 0)
        {
            RdoPaymentMode.Items[0].Selected = true;
        }
    }
    protected void BindPaymentOptions()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var paymentTypes = from p in context.PaymentModes
                                   where p.Visible == true
                                   orderby p.Name
                                   select new PaymentType { ValueField = p.ID, TextField = p.Name };
                // code to check that NEFT option is visible in only courseregistration and course exam applications
                //if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["TypeID"])))
                //    TypeID = Convert.ToInt32(Request.QueryString["TypeID"]);
                // added on 21 jan 2025              

                if (TypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(demandID)) && demandID != 0)
                    {
                        var application = (from a in context.CertificateExamApplications
                                           where a.DemandNoteID == demandID
                                           select a).FirstOrDefault();

                        if (application.InstituteID != null || application.InstituteID != 0)
                        {
                            var cscFlag = (from i in context.Institutes
                                           where i.ID == application.InstituteID
                                           select (bool?)i.whetherCSC).FirstOrDefault();


                            if (cscFlag.HasValue && cscFlag.Value)
                            {
                                Int32 cscspv = Convert.ToInt32(enmPaymentMode.CSCSPV);
                                paymentTypes = paymentTypes.Where(s => s.ValueField == cscspv);
                                BindPaymentModes(paymentTypes);
                            }
                            else
                            {
                                BindPaymentModes(paymentTypes);
                            }
                        }
                        else  // institute id is null or 0
                        {
                            BindPaymentModes(paymentTypes);
                        }
                    }

                    else // demandID is null or 0
                    {
                        BindPaymentModes(paymentTypes);
                    }
                }
                else // other  exams
                {
                    BindPaymentModes(paymentTypes);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void showCertificateAppData(CertificateExamApplication application, bool isMultipleDemandNote)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                decimal totalAmount;
                var regional = context.RegionalCenters.Where(r => r.ID == application.RegionalCenterID).FirstOrDefault();
                LblAccountName.Text = regional.BankAccountName;
                LblPayableAt.Text = regional.BankBranchName;
                if (isMultipleDemandNote == false)
                {
                    Exam exam = application.Exam;
                    LblECourseName.Text = application.Course.Name + "(" + application.Course.Code + ") " + exam.Name + " Examination ";
                    LblHCourseName.Text = application.Course.NameRegional + ", " + exam.Name;
                    TxtAmount.Text = application.TotalFeeAmount.Value.ToString("F");
                    //Txtneftamount.Text = application.TotalFeeAmount.Value.ToString("F"); // NEFT Amount
                    txtNeftAmt2.Text = application.TotalFeeAmount.Value.ToString("F"); // NEFT Amount
                    LblAppType.Text = "परीक्षा";
                    LblAppType.Text = "परीक्षा";
                    LblPayAppTypeE1.Text = "Examination";
                    LblPayAppTypeH2.Text = "परीक्षा";
                }
                else if (isMultipleDemandNote == true)
                {
                    LnkBtnPrintForm.Visible = false;
                    LblPrintForm.Visible = false;
                    Int64 DemandNoteId = Convert.ToInt64(Request.QueryString["DemandID"]);
                    var appCount = context.CertificateExamApplications.Where(c => c.DemandNoteID == DemandNoteId)
                                                        .GroupBy(p => new { p.ID, p.TotalFeeAmount }).Select(g => new { NoOfApp = g.Distinct().Count(), Total = g.Sum(s => s.TotalFeeAmount) }).ToList();


                    int noOfApp = appCount.Count;
                    totalAmount = appCount.Sum(s => s.Total.Value);
                    TxtAmount.Text = totalAmount.ToString("F");
                    txtNeftAmt2.Text = totalAmount.ToString("F"); // NEFT Amount
                    TDApplicationHead.InnerHtml = "";
                    TDApplicationHead.InnerHtml = "<font color='#000099'>Institute Name :- " + application.Institute.Name + ",<br>" + noOfApp.ToString() + " applications  for the " +
                                                    " " + application.Course.Name + "(" + application.Course.Code + ") Examination have been successfully marked for payment." +
                                                    "<br/>Total No. of Applications marked for payment: <a href='#'>" + noOfApp + "</a> <br/>" +
                                                    " आप " + application.Course.NameRegional + " परीक्षा के लिए " + noOfApp + " आवेदन पत्रों को सफलतापूर्वक भुगतान के लिए चिन्हित कर चुके हैं.<br/> कुल आवेदन पत्रों की संख्या : <a href='#'>" + noOfApp + "</a> <br/></font>";

                    LblEExamAmount.Text = totalAmount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(totalAmount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                    LblHExamAmount.Text = totalAmount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(totalAmount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";

                }
                if ((application.enmApplicantType == enmApplicantType.Direct) || (isMultipleDemandNote == true))
                {
                    tblpaymentmode.Visible = true;
                    LblPayAppTypeE2.Text = "Examination";
                    LblPayAppTypeH1.Text = "परीक्षा";

                    if (application.DemandNoteID.HasValue == true)
                    {
                        trpaydetails.Visible = true;
                        trnpaydetails.Visible = false;
                        LblEDemandNoteID.Text = application.DemandNote.ID.ToString();
                        LblHDemandNoteID.Text = application.DemandNote.ID.ToString();
                        LblEDemandDate.Text = application.DemandNote.ApplicationDate.ToString("dd-MMM-yyyy");
                        LblHDemandDate.Text = application.DemandNote.ApplicationDate.ToString("dd-MMM-yyyy");

                        //to make message configurable
                        if (context.Exams.Where(s => s.ID == application.ExamID).FirstOrDefault().IsBatchProcessable)
                        {

                            LblCentreAddressE.Text = "After making payment/depositing fees please submit this form with required attachments to the following Address: <br/>" +
                                                     "NIELIT Centre," + regional.Name + " , " + regional.Address + " <br/> Contact Number :" + regional.ContactNumbers + ",<br/>Email Id : " + regional.EmailAddresses + ", " +
                                                       "  website : " + regional.Website + "<br/>  ";


                            LblCentreAddressH.Text = "शुल्क जमा करने के बाद आवश्यक संलग्नकों के साथ यह प्रपत्र निम्न पते पर  जमा करें:  <br/>" +
                                                       "NIELIT Centre," + regional.Name + " , " + regional.Address + " <br/> फोन नंबर :" + regional.ContactNumbers + ",<br/> ईमेल आईडी : " + regional.EmailAddresses + ", " +
                                                       "  वेबसाइट : " + regional.Website + "</font>";
                        }
                        else
                        {
                            LblCentreAddressE.Text = "";
                            LblCentreAddressH.Text = "";
                        }
                        if (isMultipleDemandNote == false)
                        {
                            LblEExamAmount.Text = application.DemandNote.Amount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(application.DemandNote.Amount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                            LblHExamAmount.Text = application.DemandNote.Amount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(application.DemandNote.Amount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                        }
                    }
                    else
                    {
                        trpaydetails.Visible = false;
                        trnpaydetails.Visible = true;
                        lbldemanderror.Text = "<font color='red'> Demand Note not generated. Please generate Demand Note to make payment. </font> ";
                        RdoPaymentMode.Enabled = false;
                    }
                }
                else if (application.enmApplicantType == enmApplicantType.Institute && isMultipleDemandNote == false)
                {
                    tblpaymentmode.Visible = false;
                    MultiView1.Visible = false;
                    trpaydetails.Visible = true;
                    TdPaymentDetail.InnerHtml = "";
                    var institute = application.Institute;
                    /* To be done???????/ */
                    TdPaymentDetail.InnerHtml = " <font color='#000099'><br>Please deposit fee/submit(as applicable) for exam form at the following Address till last date: <br/> Approved Centre : " +
                          GetInitCap(institute.Name) + "  <br>  " + institute.AddressLine1 + "  ,  " + institute.AddressLine2 +
                        institute.AddressLine3 +
                        "<br/>" + institute.CityName + " ( " + institute.State.Name + " ) ,  " + " Pin Code :" + institute.PinCode.Value.ToString() +
                        "<br/> Phone Number  :";
                    //if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                    //{
                    //    TdPaymentDetail.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber1.Value;
                    //    if (institute.PhoneNumber2.HasValue)
                    //        TdPaymentDetail.InnerHtml += ", " + institute.PhoneNumber2.Value;
                    //}
                    //else
                    //    TdPaymentDetail.InnerHtml += "NA";
                    //deep test above comment on 2 aug 2018
                    if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                    {
                        TdPaymentDetail.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber1.Value + ", ";
                    }
                    else if (institute.StdNumber.HasValue && institute.PhoneNumber2.HasValue)
                    {
                        if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                        {
                            TdPaymentDetail.InnerHtml += institute.PhoneNumber2.Value + ", ";
                        }
                        else
                        {
                            TdPaymentDetail.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber2.Value + ", ";
                        }
                    }
                    else if (institute.StdNumber.HasValue && institute.FaxNumber.HasValue)
                    {
                        TdPaymentDetail.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.FaxNumber.Value + ", ";
                    }
                    else if (institute.MobileNumber.HasValue)
                    {
                        TdPaymentDetail.InnerHtml += institute.MobileNumber.Value;
                    }
                    else
                        TdPaymentDetail.InnerHtml += "NA";
                    //deep test TdPaymentDetail on 2 aug 2018
                    TdPaymentDetail.InnerHtml += "<br/><br>अंतिम तारीख तक कृपया निम्न पते पर शुल्क/परीक्षा फार्म (जो लागू हो) जमा करें : <br/> Approved Centre : " +
                        GetInitCap(institute.Name) + " <br> " + institute.AddressLine1 + " ,  " + institute.AddressLine2 +
                        institute.AddressLine3 +
                        "<br/> " + institute.CityName + "( " + institute.State.Name + ") , " + " पिन कोड :" + institute.PinCode.Value.ToString() +
                        "<br/> फोन नंबर  :";


                    //if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                    //{
                    //    TdPaymentDetail.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber1.Value;
                    //    if (institute.PhoneNumber2.HasValue)
                    //        TdPaymentDetail.InnerHtml += ", " + institute.PhoneNumber2.Value;
                    //}
                    //else
                    //    TdPaymentDetail.InnerHtml += "NA";

                    //deep add code on 2 aug 2018
                    if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                    {
                        TdPaymentDetail.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber1.Value + ", ";
                    }
                    else if (institute.StdNumber.HasValue && institute.PhoneNumber2.HasValue)
                    {
                        if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                        {
                            TdPaymentDetail.InnerHtml += institute.PhoneNumber2.Value + ", ";
                        }
                        else
                        {
                            TdPaymentDetail.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber2.Value + ", ";
                        }
                    }
                    else if (institute.StdNumber.HasValue && institute.FaxNumber.HasValue)
                    {
                        TdPaymentDetail.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.FaxNumber.Value + ", ";
                    }
                    else if (institute.MobileNumber.HasValue)
                    {
                        TdPaymentDetail.InnerHtml += institute.MobileNumber.Value;
                    }
                    else
                        TdPaymentDetail.InnerHtml += "NA";
                    //deep code end on 2 aug 2018

                    TdPaymentDetail.InnerHtml += "</font>";

                }
                Int32 submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                if (application.LateFeeAmount.HasValue)
                {
                    if (application.LateFeeAmount.Value > 0)
                        submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                }
                DateTime lastDate = (from c in context.CutOffDates
                                     where c.ActivityID == submisssionDateActivityID && c.ExamID == application.ExamID && c.ApplicantTypeID == application.ApplicantTypeID
                                     select c.EfferctiveDate).FirstOrDefault();
                // Added by deep 06-Feb-2016
                int FeeSubmissionOfInstituteExtensionPeriodInDays = (from c in context.Exams
                                                                     where c.ID == application.ExamID && c.CourseID == application.CourseID && c.CourseCategoryID == application.CourseCategoryID
                                                                     select c.FeeSubmissioInstituteExtPeriod).FirstOrDefault();

                if (application.enmApplicantType == enmApplicantType.Institute)// && application.enmPaymentSource == enmPaymentSource.Institute)
                {
                    lastDate = lastDate.AddDays(FeeSubmissionOfInstituteExtensionPeriodInDays);
                    lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");
                }
                else
                {
                    lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");
                }
                //added end deep 06-Feb-2016

                lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");
                if (lastDate.Date < DateTime.Now.Date)
                {
                    //Added 15 May 2021
                    RdoPaymentMode.Enabled = false;

                    /*if (RdoPaymentMode.Items.Equals("DEMAND DRAFT"))
                        RdoPaymentMode.Items.FindByText("DEMAND DRAFT").Enabled = false;
                    if (RdoPaymentMode.Items.Equals("NEFT/RTGS"))
                        RdoPaymentMode.Items.FindByText("NEFT/RTGS").Enabled = false;
                    RdoPaymentMode.Items.FindByText("CSC SPV").Enabled = false;
                    RdoPaymentMode.Items.FindByText("ONLINE").Enabled = false;*/
                    //throw new Exception("This demand note has been expired on " + lastDate.ToString("dd-MMM-yyy"));
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void showCourseRegistrationData(CourseRegistrationApplication registration, bool isMultipleDemandNote)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                decimal totalAmount; int noOfApp = 0;

                var org = (from o in context.Organizations
                           where o.ID == 1
                           select o).FirstOrDefault();
                LblAccountName.Text = org.BankAccountName;
                LblPayableAt.Text = org.BankBranchName;

                if (isMultipleDemandNote == false)
                {
                    LblECourseName.Text = registration.Course.Name + "(" + registration.Course.Code + ") Registration";
                    LblHCourseName.Text = registration.Course.NameRegional;
                    TxtAmount.Text = registration.FeeAmount.Value.ToString("F");
                    txtNeftAmt2.Text = registration.FeeAmount.Value.ToString("F"); // NEFT Amount
                    LblAppType.Text = "पंजीकरण";
                    LblPayAppTypeE1.Text = "Registration";
                    LblPayAppTypeH2.Text = "पंजीकरण";
                }
                else if (isMultipleDemandNote == true)
                {
                    LnkBtnPrintForm.Visible = false;
                    LblPrintForm.Visible = false;
                    Int64 DemandNoteId = Convert.ToInt64(Request.QueryString["DemandID"]);
                    var appCount = context.CourseRegistrationApplications.Where(c => c.DemandNoteID == DemandNoteId)
                                                        .GroupBy(p => new { p.ID, p.FeeAmount }).Select(g => new { NoOfApp = g.Distinct().Count(), Total = g.Sum(s => s.FeeAmount) }).ToList();

                    noOfApp = appCount.Count;
                    totalAmount = appCount.Sum(s => s.Total.Value);
                    TxtAmount.Text = totalAmount.ToString("F");
                    txtNeftAmt2.Text = totalAmount.ToString("F"); // NEFT Amount
                    TDApplicationHead.InnerHtml = "";
                    TDApplicationHead.InnerHtml = "<font color='#000099'>Dear  " + registration.Institute.Name + ",<br>" + noOfApp.ToString() + " applications  for the " +
                                                    "  " + registration.Course.Name + "(" + registration.Course.Code + ") Examination have been successfully marked for payment." +
                                                    "<br/>Total No. of Applications  : <a href='#'>" + noOfApp + "</a> <br/>" +
                                                    " आपने सफलतापूर्वक " + registration.Course.NameRegional + " पंजीकरण के लिए आवेदन किया है <br/> कुल आवेदनों की संख्या : <a href='#'>" + noOfApp + "</a> <br/></font>";
                    LblEExamAmount.Text = totalAmount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(totalAmount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                    LblHExamAmount.Text = totalAmount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(totalAmount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";

                }

                if (registration.enmApplicantType == enmApplicantType.Direct || (isMultipleDemandNote == true) || (registration.enmApplicantType == enmApplicantType.Institute && registration.enmPaymentSource == enmPaymentSource.Candidate))
                {
                    LblPayAppTypeE2.Text = "Registration";
                    LblPayAppTypeH1.Text = "पंजीकरण";
                    tblpaymentmode.Visible = true;
                    if (registration.DemandNoteID.HasValue == true)
                    {
                        trpaydetails.Visible = true;
                        trnpaydetails.Visible = false;
                        LblEDemandNoteID.Text = registration.DemandNote.ID.ToString();
                        LblHDemandNoteID.Text = registration.DemandNote.ID.ToString();
                        LblEDemandDate.Text = registration.DemandNote.ApplicationDate.ToString("dd-MMM-yyyy");
                        LblHDemandDate.Text = registration.DemandNote.ApplicationDate.ToString("dd-MMM-yyyy");
                        if (isMultipleDemandNote == false)
                        {
                            LblEExamAmount.Text = registration.DemandNote.Amount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(registration.DemandNote.Amount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                            LblHExamAmount.Text = registration.DemandNote.Amount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(registration.DemandNote.Amount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                        }
                    }
                    else
                    {
                        trpaydetails.Visible = false;
                        trnpaydetails.Visible = true;
                        lbldemanderror.Text = "<font color='red'> Demand Note not generated. Please generate Demand Note to make payment. </font> ";
                        RdoPaymentMode.Enabled = false;
                    }
                }
                else if (registration.enmApplicantType == enmApplicantType.Institute && isMultipleDemandNote == false)
                {
                    tblpaymentmode.Visible = false;
                    MultiView1.Visible = false;
                    TdPaymentDetail.InnerText = "";
                    var institute = registration.Institute;
                    string InstitutePinCode = institute.PinCode.HasValue ? institute.PinCode.Value.ToString() : "";
                    string stdNumber = institute.StdNumber.HasValue ? institute.StdNumber.Value.ToString() : "";
                    string PhoneNumber1 = institute.PhoneNumber1.HasValue ? institute.PhoneNumber1.Value.ToString() : "";
                    string PhoneNumber2 = institute.PhoneNumber2.HasValue ? institute.PhoneNumber2.Value.ToString() : "";
                    TdPaymentDetail.InnerHtml = " <font color='#000099'> Please deposit fee at the following Address within 7 days : <br/> Accredited Centre :  " +
                                               GetInitCap(institute.Name) + "  <br>  " + institute.AddressLine1 + " , " + institute.AddressLine2 +
                                               institute.AddressLine3 +
                                               "<br/> " + institute.CityName + "(" + institute.State.Name + ") ,  " + "  Pin Code :" + InstitutePinCode +
                                               "<br/> Phone Number  :" + stdNumber + " - " + PhoneNumber1 +
                                               ", " + PhoneNumber2 +
                                               "<br/><br>7 दिनों के भीतर कृपया निम्न पते पर शुल्क जमा करें : <br/> Accredited Centre : " +
                                               GetInitCap(institute.Name) + "<br> " + institute.AddressLine1 + " , " + institute.AddressLine2 +
                                               institute.AddressLine3 +
                                               "<br/> " + institute.CityName + "(" + institute.State.Name + ") , " + " पिन कोड :" + InstitutePinCode +
                                               "<br/>  फोन नंबर  :" + stdNumber + " - " + PhoneNumber1 +
                                               ", " + PhoneNumber2 +
                                                "</font>";

                }
                Int32 submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
                //if (registration)
                //{
                //    submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                //}
                DateTime lastDate = (from c in context.CutOffDates
                                     where c.ActivityID == submisssionDateActivityID && c.ExamID == registration.ApplicableExamID && c.ApplicantTypeID == registration.ApplicantTypeID
                                     select c.EfferctiveDate).FirstOrDefault();


                //  Added by deep 02-Feb-2016 1 

                int FeeSubmissionOfInstituteExtensionPeriodInDays = (from c in context.Exams
                                                                     where c.ID == registration.ApplicableExamID && c.CourseID == registration.CourseID && c.CourseCategoryID == registration.CourseCategoryID
                                                                     select c.FeeSubmissioInstituteExtPeriod).FirstOrDefault();

                if (registration.enmApplicantType == enmApplicantType.Institute && registration.enmPaymentSource == enmPaymentSource.Institute)
                {
                    lastDate = lastDate.AddDays(FeeSubmissionOfInstituteExtensionPeriodInDays);
                    lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");
                }
                else
                {
                    lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");
                }
                // //deep end  1
                if (lastDate.Date < DateTime.Now.Date)
                {
                    //Added 15 May 2021
                    RdoPaymentMode.Enabled = false;
                    /* if (RdoPaymentMode.Items.Equals("DEMAND DRAFT"))
                         RdoPaymentMode.Items.FindByText("DEMAND DRAFT").Enabled = false;
                     if (RdoPaymentMode.Items.Equals("NEFT/RTGS"))
                         RdoPaymentMode.Items.FindByText("NEFT/RTGS").Enabled = false;
                     RdoPaymentMode.Items.FindByText("CSC SPV").Enabled = false;
                     RdoPaymentMode.Items.FindByText("ONLINE").Enabled = false;*/
                    throw new Exception("This demand note has been expired on " + lastDate.ToString("dd-MMM-yyy"));
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowCourseExamApplicationData(CourseExamApplication application, bool isMultipleDemandNote)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                decimal totalAmount; int noOfApp = 0;
                var org = (from o in context.Organizations
                           where o.ID == 1
                           select o).FirstOrDefault();
                LblAccountName.Text = org.BankAccountName;
                LblPayableAt.Text = org.BankBranchName;

                if (isMultipleDemandNote == false)
                {
                    LblECourseName.Text = application.Exam.Name + "(" + application.Course.Name + ") Examination ";
                    LblHCourseName.Text = application.Exam.Name + "(" + application.Course.Name + ") परीक्षा ";
                    TxtAmount.Text = application.FeeAmount.ToString("F");
                    txtNeftAmt2.Text = application.FeeAmount.ToString("F"); // NEFT Amount
                    LblPayAppTypeE1.Text = "Examination";
                    LblPayAppTypeH2.Text = "परीक्षा";
                }
                else if (isMultipleDemandNote == true)
                {
                    LnkBtnPrintForm.Visible = false;
                    LblPrintForm.Visible = false;
                    Int64 DemandNoteId = Convert.ToInt64(Request.QueryString["DemandID"]);
                    var appCount = context.CourseExamApplications.Where(c => c.DemandNoteID == DemandNoteId)
                                                        .GroupBy(p => new { p.ID, p.FeeAmount }).Select(g => new { NoOfApp = g.Distinct().Count(), Total = g.Sum(s => s.FeeAmount) }).ToList();

                    noOfApp = appCount.Count;
                    totalAmount = appCount.Sum(s => s.Total);
                    TxtAmount.Text = totalAmount.ToString("F");
                    txtNeftAmt2.Text = totalAmount.ToString("F"); // NEFT Amount
                    TDApplicationHead.InnerHtml = "";
                    TDApplicationHead.InnerHtml = "<font color='#000099'>Dear " + application.Institute.Name + ",<br>" + noOfApp.ToString() + " applications  for the " +
                                                    " " + application.Course.Name + "(" + application.Course.Code + ") Examination have been successfully marked for payment." +
                                                    "<br/>Total No. of Applications  : <a href='#'>" + noOfApp + "</a> <br/>" +
                                                    " आपने सफलतापूर्वक " + application.Course.NameRegional + " पंजीकरण के लिए आवेदन किया है <br/> कुल आवेदनों की संख्या : <a href='#'>" + noOfApp + "</a> <br/></font>";
                    LblEExamAmount.Text = totalAmount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(totalAmount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                    LblHExamAmount.Text = totalAmount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(totalAmount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";

                }

                if (application.enmApplicantType == enmApplicantType.Direct || (isMultipleDemandNote == true) || (application.enmApplicantType == enmApplicantType.Institute && application.enmPaymentSource == enmPaymentSource.Candidate))
                {

                    LblPayAppTypeE2.Text = "Examination";
                    LblPayAppTypeH1.Text = "परीक्षा";
                    tblpaymentmode.Visible = true;
                    if (application.DemandNoteID.HasValue == true)
                    {
                        trpaydetails.Visible = true;
                        trnpaydetails.Visible = false;
                        LblEDemandNoteID.Text = application.DemandNote.ID.ToString();
                        LblHDemandNoteID.Text = application.DemandNote.ID.ToString();
                        LblEDemandDate.Text = application.DemandNote.ApplicationDate.ToString("dd-MMM-yyyy");
                        LblHDemandDate.Text = application.DemandNote.ApplicationDate.ToString("dd-MMM-yyyy");
                        if (isMultipleDemandNote == false)
                        {
                            LblEExamAmount.Text = application.DemandNote.Amount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(application.DemandNote.Amount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                            LblHExamAmount.Text = application.DemandNote.Amount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(application.DemandNote.Amount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.Hindi) + ")</i>";
                        }
                    }
                    else
                    {
                        trpaydetails.Visible = false;
                        trnpaydetails.Visible = true;
                        lbldemanderror.Text = "<font color='red'> Demand Note not generated. Please generate Demand Note to make payment. </font> ";
                        RdoPaymentMode.Enabled = false;
                    }
                }
                else if ((application.enmApplicantType == enmApplicantType.Institute && isMultipleDemandNote == false))
                {
                    tblpaymentmode.Visible = false;
                    MultiView1.Visible = false;
                    TdPaymentDetail.InnerText = "";
                    var institute = application.Institute;
                    /* To be done???????/ */
                    TdPaymentDetail.InnerHtml = " <font color='#000099'> Please deposit fee/submit(as applicable) for exam form at the following Address till last date: <br/> Accredited Centre : " +
                        GetInitCap(institute.Name) + " <br> " + institute.AddressLine1 + " , " + institute.AddressLine2 +
                        institute.AddressLine3 +
                        "<br/>" + institute.CityName + "(" + institute.State.Name + ") , " + " Pin Code :" + institute.PinCode.Value.ToString() +
                        "<br/> Phone Number  :";
                    //if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                    //{
                    //    TdPaymentDetail.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber1.Value;
                    //    if (institute.PhoneNumber2.HasValue)
                    //        TdPaymentDetail.InnerHtml += ", " + institute.PhoneNumber2.Value;
                    //}
                    //else
                    //    TdPaymentDetail.InnerHtml += "NA";
                    //deep add code on 09 aug 2018 and comment above line
                    if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                    {
                        TdPaymentDetail.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber1.Value + ", ";
                    }
                    else if (institute.StdNumber.HasValue && institute.PhoneNumber2.HasValue)
                    {
                        if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                        {
                            TdPaymentDetail.InnerHtml += institute.PhoneNumber2.Value + ", ";
                        }
                        else
                        {
                            TdPaymentDetail.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber2.Value + ", ";
                        }
                    }
                    else if (institute.StdNumber.HasValue && institute.FaxNumber.HasValue)
                    {
                        TdPaymentDetail.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.FaxNumber.Value + ", ";
                    }
                    else if (institute.MobileNumber.HasValue)
                    {
                        TdPaymentDetail.InnerHtml += institute.MobileNumber.Value;
                    }
                    else
                        TdPaymentDetail.InnerHtml += "NA";
                    //deep end add code on 9 aug 2018
                    TdPaymentDetail.InnerHtml += "<br/><br>अंतिम तारीख तक कृपया निम्न पते पर शुल्क/परीक्षा फार्म (जो लागू हो) जमा करें : <br/> Accredited Centre : " +
                        GetInitCap(institute.Name) + "<br>" + institute.AddressLine1 + " , " + institute.AddressLine2 +
                        institute.AddressLine3 +
                        "<br/>" + institute.CityName + "(" + institute.State.Name + ") , " + " पिन कोड :" + institute.PinCode.Value.ToString() +
                        "<br/> फोन नंबर  :";
                    //if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                    //{
                    //    TdPaymentDetail.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber1.Value;
                    //    if (institute.PhoneNumber2.HasValue)
                    //        TdPaymentDetail.InnerHtml += ", " + institute.PhoneNumber2.Value;
                    //}
                    //else
                    //    TdPaymentDetail.InnerHtml += "NA";
                    //deep add code on 09 aug 2018 and comment above line
                    if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                    {
                        TdPaymentDetail.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber1.Value + ", ";
                    }
                    else if (institute.StdNumber.HasValue && institute.PhoneNumber2.HasValue)
                    {
                        if (institute.StdNumber.HasValue && institute.PhoneNumber1.HasValue)
                        {
                            TdPaymentDetail.InnerHtml += institute.PhoneNumber2.Value + ", ";
                        }
                        else
                        {
                            TdPaymentDetail.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.PhoneNumber2.Value + ", ";
                        }
                    }
                    else if (institute.StdNumber.HasValue && institute.FaxNumber.HasValue)
                    {
                        TdPaymentDetail.InnerHtml += institute.StdNumber.Value.ToString() + " - " + institute.FaxNumber.Value + ", ";
                    }
                    else if (institute.MobileNumber.HasValue)
                    {
                        TdPaymentDetail.InnerHtml += institute.MobileNumber.Value;
                    }
                    else
                        TdPaymentDetail.InnerHtml += "NA";
                    //deep end add code on 9 aug 2018
                    TdPaymentDetail.InnerHtml += "</font>";

                }
                Int32 submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                if (application.LateFeeImposed)
                {
                    submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                }
                DateTime lastDate = (from c in context.CutOffDates
                                     where c.ActivityID == submisssionDateActivityID && c.ExamID == application.ExamID && c.ApplicantTypeID == application.ApplicantTypeID
                                     select c.EfferctiveDate).FirstOrDefault();

                int latefeeID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                DateTime lastDateWithLateFee = (from c in context.CutOffDates
                                                where c.ActivityID == latefeeID && c.ExamID == application.ExamID && c.ApplicantTypeID == application.ApplicantTypeID
                                                select c.EfferctiveDate).FirstOrDefault();

                //  Added by deep 30-Jan-2016 1 
                // // lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");
                int FeeSubmissionOfInstituteExtensionPeriodInDays = (from c in context.Exams
                                                                     where c.ID == application.ExamID && c.CourseID == application.CourseID && c.CourseCategoryID == application.CourseCategoryID
                                                                     select c.FeeSubmissioInstituteExtPeriod).FirstOrDefault();

                if (application.enmApplicantType == enmApplicantType.Institute && application.enmPaymentSource == enmPaymentSource.Institute)
                {
                    lastDate = lastDate.AddDays(FeeSubmissionOfInstituteExtensionPeriodInDays);
                    lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");
                }
                else
                {
                    lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");
                }
                // //deep end  1

                if (lastDate.Date < DateTime.Now.Date)
                {
                    if ((application.FinalSubmitted && application.FinalSubmissionDate.Value.Date > lastDate.Date) || (lastDateWithLateFee.Date < DateTime.Now.Date))
                    {
                        //Added 15 May 2021
                        RdoPaymentMode.Enabled = false;
                        /*if (RdoPaymentMode.Items.Equals("DEMAND DRAFT"))
                            RdoPaymentMode.Items.FindByText("DEMAND DRAFT").Enabled = false;
                        if (RdoPaymentMode.Items.Equals("NEFT/RTGS"))
                            RdoPaymentMode.Items.FindByText("NEFT/RTGS").Enabled = false;
                        RdoPaymentMode.Items.FindByText("CSC SPV").Enabled = false;
                        RdoPaymentMode.Items.FindByText("ONLINE").Enabled = false;*/
                        throw new Exception("This demand note has been expired on " + lastDate.ToString("dd-MMM-yyy"));
                    }
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowCourseExamVafAcfData(CourseExamVafAcf application, bool isMultipleDemandNote)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                decimal totalAmount; int noOfApp = 0;
                var org = (from o in context.Organizations
                           where o.ID == 1
                           select o).FirstOrDefault();
                LblAccountName.Text = org.BankAccountName;
                LblPayableAt.Text = org.BankBranchName;

                //if (isMultipleDemandNote == false)
                //{
                //    LblECourseName.Text = application.Exam.Name + "(" + application.Course.Name + ") Examination ";
                //    LblHCourseName.Text = application.Exam.Name + "(" + application.Course.Name + ") परीक्षा ";
                //    TxtAmount.Text = application.FeeAmount.ToString("F");
                //    txtNeftAmt2.Text = application.FeeAmount.ToString("F"); // NEFT Amount
                //    LblPayAppTypeE1.Text = "VAF ACF";
                //    LblPayAppTypeH2.Text = "VAF ACF";
                //}
                if (isMultipleDemandNote == true)
                {
                    LnkBtnPrintForm.Visible = false;
                    LblPrintForm.Visible = false;
                    Int64 DemandNoteId = Convert.ToInt64(Request.QueryString["DemandID"]);
                    var appCount = context.CourseExamVafAcfs.Where(c => c.DemandNoteID == DemandNoteId).ToList();
                    // .GroupBy(p => new { p.ID, p.FeeAmount }).Select(g => new { NoOfApp = g.Distinct().Count(), Total = g.Sum(s => s.FeeAmount) }).ToList();

                    noOfApp = appCount.Count;
                    //totalAmount = appCount.Sum(s => s.Total);
                    totalAmount = context.DemandNotes.Where(c => c.ID == DemandNoteId).Select(c => c.Amount).FirstOrDefault();
                    TxtAmount.Text = totalAmount.ToString("F");
                    txtNeftAmt2.Text = totalAmount.ToString("F"); // NEFT Amount
                    TDApplicationHead.InnerHtml = "";
                    TDApplicationHead.InnerHtml = "<font color='#000099'>Dear " + application.Institute.Name + ",<br>" + noOfApp.ToString() + " applications  for the " +
                                                    " " + application.Course.Name + "(" + application.Course.Code + ") Examination have been successfully marked for payment." +
                                                    "<br/>Total No. of Applications  : <a href='#'>" + noOfApp + "</a> <br/>" +
                                                    " आपने सफलतापूर्वक " + application.Course.NameRegional + " पंजीकरण के लिए आवेदन किया है <br/> कुल आवेदनों की संख्या : <a href='#'>" + noOfApp + "</a> <br/></font>";
                    LblEExamAmount.Text = totalAmount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(totalAmount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                    LblHExamAmount.Text = totalAmount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(totalAmount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";

                }

                // if (application.enmApplicantType == enmApplicantType.Direct || (isMultipleDemandNote == true) || (application.enmApplicantType == enmApplicantType.Institute && application.enmPaymentSource == enmPaymentSource.Candidate))
                if (isMultipleDemandNote == true)
                {

                    LblPayAppTypeE2.Text = "VAF ACF";
                    LblPayAppTypeH1.Text = "VAF ACF";
                    tblpaymentmode.Visible = true;
                    if (application.DemandNoteID.HasValue == true)
                    {
                        trpaydetails.Visible = true;
                        trnpaydetails.Visible = false;
                        LblEDemandNoteID.Text = application.DemandNote.ID.ToString();
                        LblHDemandNoteID.Text = application.DemandNote.ID.ToString();
                        LblEDemandDate.Text = application.DemandNote.ApplicationDate.ToString("dd-MMM-yyyy");
                        LblHDemandDate.Text = application.DemandNote.ApplicationDate.ToString("dd-MMM-yyyy");
                        if (isMultipleDemandNote == false)
                        {
                            LblEExamAmount.Text = application.DemandNote.Amount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(application.DemandNote.Amount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                            LblHExamAmount.Text = application.DemandNote.Amount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(application.DemandNote.Amount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.Hindi) + ")</i>";
                        }
                    }
                    else
                    {
                        trpaydetails.Visible = false;
                        trnpaydetails.Visible = true;
                        lbldemanderror.Text = "<font color='red'> Demand Note not generated. Please generate Demand Note to make payment. </font> ";
                        RdoPaymentMode.Enabled = false;
                    }
                }

                Int32 submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                //if (application.LateFeeImposed)
                //{
                //    submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                //}
                DateTime lastDate = (from c in context.CutOffDates
                                     where c.ActivityID == submisssionDateActivityID && c.ExamID == application.ExamID && c.ApplicantTypeID == 2
                                     select c.EfferctiveDate).FirstOrDefault();

                //int latefeeID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                //DateTime lastDateWithLateFee = (from c in context.CutOffDates
                //                                where c.ActivityID == latefeeID && c.ExamID == application.ExamID && c.ApplicantTypeID == 2
                //                                select c.EfferctiveDate).FirstOrDefault();

                //  Added by deep 30-Jan-2016 1 
                // // lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");
                int FeeSubmissionOfInstituteExtensionPeriodInDays = (from c in context.Exams
                                                                     where c.ID == application.ExamID && c.CourseID == application.CourseID && c.CourseCategoryID == application.CourseCategoryID
                                                                     select c.FeeSubmissioInstituteExtPeriod).FirstOrDefault();

                // if (application.enmApplicantType == enmApplicantType.Institute && application.enmPaymentSource == enmPaymentSource.Institute)
                // {
                lastDate = lastDate.AddDays(FeeSubmissionOfInstituteExtensionPeriodInDays);
                lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");
                //}
                //else
                //{
                //    lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");
                //}
                // //deep end  1

                if (lastDate.Date < DateTime.Now.Date)
                {
                    // if ((application.FinalSubmitted && application.FinalSubmissionDate.Value.Date > lastDate.Date) || (lastDateWithLateFee.Date < DateTime.Now.Date))
                    if ((application.FinalSubmitted && application.FinalSubmissionDate.Date > lastDate.Date))
                    {

                        RdoPaymentMode.Enabled = false;
                        /*if (RdoPaymentMode.Items.Equals("DEMAND DRAFT"))
                            RdoPaymentMode.Items.FindByText("DEMAND DRAFT").Enabled = false;
                        if (RdoPaymentMode.Items.Equals("NEFT/RTGS"))
                            RdoPaymentMode.Items.FindByText("NEFT/RTGS").Enabled = false;
                        RdoPaymentMode.Items.FindByText("CSC SPV").Enabled = false;
                        RdoPaymentMode.Items.FindByText("ONLINE").Enabled = false;*/
                        throw new Exception("This demand note has been expired on " + lastDate.ToString("dd-MMM-yyy"));
                    }
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ShowData(Int64 appID, int TypeID, Int64 demandID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //if(Direct case with demand note id and Instiute Case without Demand Note ID
                if ((appID != 0 && TypeID != 0 && demandID != 0) || (appID != 0 && TypeID != 0 && demandID == 0))
                {
                    if (TypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                    {
                        var application = (from a in context.CertificateExamApplications
                                           where a.ID == appID
                                           select a).FirstOrDefault();
                        // if (application.enmApplicationSource == enmApplicationSource.CSC || application.InstituteID.ToString().StartsWith("7000"))//
                        if (application.enmApplicationSource == enmApplicationSource.CSC)
                        {
                            RdoPaymentMode.SelectedValue = Convert.ToInt32(enmPaymentMode.CSCSPV).ToString();
                            RdoPaymentMode.Enabled = false;
                        }
                        LblAppID1.Text = application.Number.ToString();
                        LblAppID2.Text = application.Number.ToString();
                        showCertificateAppData(application, false);

                    }
                    else if (TypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                    {
                        var registration = (from a in context.CourseRegistrationApplications
                                            where a.ID == appID
                                            select a).FirstOrDefault();
                        if (registration.enmApplicationSource == enmApplicationSource.CSC)
                        {
                            RdoPaymentMode.SelectedValue = Convert.ToInt32(enmPaymentMode.CSCSPV).ToString();
                            RdoPaymentMode.Enabled = false;
                        }
                        LblAppID1.Text = registration.Number.ToString();
                        LblAppID2.Text = registration.Number.ToString();
                        showCourseRegistrationData(registration, false);

                    }
                    else if (TypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    {
                        var registration = (from a in context.CourseExamApplications
                                            where a.ID == appID
                                            select a).FirstOrDefault();
                        LblAppID1.Text = registration.Number.ToString();
                        LblAppID2.Text = registration.Number.ToString();
                        ShowCourseExamApplicationData(registration, false);
                    }

                }
                else if (appID == 0 && TypeID != 0 && demandID != 0)//Multiple Application Demand note
                {
                    if (TypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                    {
                        var application = (from a in context.CertificateExamApplications
                                           where a.DemandNoteID == demandID
                                           select a).FirstOrDefault();

                        showCertificateAppData(application, true);
                    }
                    else if (TypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                    {
                        var registration = (from a in context.CourseRegistrationApplications
                                            where a.DemandNoteID == demandID
                                            select a).FirstOrDefault();
                        showCourseRegistrationData(registration, true);
                    }
                    else if (TypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    {
                        if (Request.QueryString["IsVafAcf"] == null)
                        {
                            var registration = (from a in context.CourseExamApplications
                                                where a.DemandNoteID == demandID
                                                select a).FirstOrDefault();
                            ShowCourseExamApplicationData(registration, true);
                        }
                        //vaf acf ------------------------------------------------------
                        else if (Convert.ToInt32(Request.QueryString["IsVafAcf"]) == 1)
                        {
                            var registration = (from a in context.CourseExamVafAcfs
                                                where a.DemandNoteID == demandID
                                                select a).FirstOrDefault();
                            ShowCourseExamVafAcfData(registration, true);
                        }
                        //end vaf acf---------------------------------------------------



                    }
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isValidDD()
    {
        try
        {
            DateTime todaydate = DateTime.Now;
            DateTime DDdate = Convert.ToDateTime(txtDDdate.Text);

            int CurrentDateresult = DateTime.Compare(todaydate, DDdate);
            int ThreeMonthSatus = DateTime.Compare(todaydate.AddMonths(-3), DDdate);
            if (DDdate > DateTime.Now.Date)
            {
                LblDDerror.Text = "Demand Draft date should not be greater than Current Date!";
                txtDDdate.Focus();
                return false;
            }

            if (DDdate < DateTime.Now.AddDays(-30).Date)
            {
                LblDDerror.Text = "Demand Draft should not be more than 30 days old!";
                txtDDdate.Focus();
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isValidElectronicTransfer()
    {
        try
        {
            DateTime todaydate = DateTime.Now;
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            DateTime Neftdate = Convert.ToDateTime(txtneftdate.Text);
            string utnumber = Txtnefttransno.Text.ToString();
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["TypeID"])))
                TypeID = Convert.ToInt32(Request.QueryString["TypeID"]);
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["Appid"])))
                appID = Convert.ToInt64(Request.QueryString["Appid"]);

            if (Neftdate > DateTime.Now.Date)
            {
                //  Lbnefterror.Text = "NEFT Transaction date should not be greater than Current Date!";
                txtneftdate.Focus();
                return false;
            }
            //if (appID != 0 && TypeID != 0)
            //{

            //    if (TypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
            //    {
            //        var registration = (from a in context.CourseRegistrationApplications
            //                            where a.ID == appID
            //                            select a).FirstOrDefault();
            //        if (Neftdate.Date < registration.ApplicationDate.Date)
            //        {
            //            Lbnefterror.Text = "NEFT Transaction date should not be less than the Application Date!";
            //            txtneftdate.Focus();
            //            return false;
            //        }
            //    }
            //    else if (TypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            //    {
            //        var registration = (from a in context.CourseExamApplications
            //                            where a.ID == appID
            //                            select a).FirstOrDefault();
            //        if (Neftdate.Date < registration.ApplicationDate.Date)
            //        {
            //            Lbnefterror.Text = "NEFT Transaction date should not be less than the Application Date!";
            //            txtneftdate.Focus();
            //            return false;
            //        }

            //    }
            //    else if (TypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
            //    {
            //        var registration = (from a in context.CertificateExamApplications
            //                            where a.ID == appID
            //                            select a).FirstOrDefault();
            //        if (Neftdate.Date < registration.ApplicationDate.Date)
            //        {
            //            Lbnefterror.Text = "NEFT Transaction date should not be less than the Application Date!";
            //            txtneftdate.Focus();
            //            return false;
            //        }
            //    }
            //}

            if (!regexItem.IsMatch(utnumber))
            {
                //  Lbnefterror.Text = " Not a valid NEFT Transaction number!";
                Txtnefttransno.Focus();
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void SaveDD()
    {
        try
        {
            if (isValidDD())
            {
                Int64 applID = Convert.ToInt64(Request.QueryString["Appid"]);
                Int64 DemandID = Convert.ToInt64(Request.QueryString["DemandID"]);
                using (TransactionScope scope = new TransactionScope())
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        Int32 submisssionDateActivityID = Convert.ToInt32(enmActivity.DateFfCommencementOfOnlineFillInExaminationApplicationForm);
                        DemandNote demand = context.DemandNotes.Find(DemandID);
                        if (demand.enmApplicationType == enmApplicationType.CourseExamApplication)
                        {
                            if (applID > 0)
                            {
                                CourseExamApplication appl = context.CourseExamApplications.Find(applID);
                                DateTime startDate = (from c in context.CutOffDates
                                                      where c.ActivityID == submisssionDateActivityID && c.ExamID == appl.ExamID && c.ApplicantTypeID == appl.ApplicantTypeID
                                                      select c.EfferctiveDate).FirstOrDefault();
                                if (Convert.ToDateTime(txtDDdate.Text.ToString()) < startDate)
                                {
                                    LblDDerror.Text = "Demand note date can not be less than " + startDate.ToString("dd-MMM-yyyy");
                                    return;
                                }
                            }
                        }
                        DemandDraftTransaction dd = new DemandDraftTransaction();
                        dd.DemandDraftDate = Convert.ToDateTime(txtDDdate.Text.ToString());
                        dd.DemandDraftNumber = TxtDDnumber.Text;
                        dd.DemandDraftAmount = Convert.ToDecimal(TxtAmount.Text);
                        dd.IssuingBankName = TxtBank.Text.ToUpper();
                        dd.CreatedByID = 1;// Convert.ToInt32(Session["UserID"]);
                        dd.Date = DateTime.Now;
                        dd.DemandNoteID = DemandID;
                        context.DemandDraftTransactions.Add(dd);
                        context.SaveChanges();

                        demand.DDTransactionID = dd.ID;
                        demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.DemandDraft);
                        demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.PaidButNotVerified);
                        context.Entry(demand).State = System.Data.Entity.EntityState.Modified;


                        //object application = null;
                        if (demand.enmApplicationType == enmApplicationType.CertificateExamApplication)
                        {
                            if (demand.enmDemandNoteType == enmDemandNoteType.Single)
                            //  context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre) + ", Payment_Status_ID = " + demand.PaymentStatusID + " Where Demand_Note_ID = " + demand.ID);
                            {    //December_2024
                                SqlParameter[] param1 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre)),
                                                            new SqlParameter("@demandNotePaymentStatus", demand.PaymentStatusID),
                                                                new SqlParameter("@demandNoteId", demand.ID)
                                                                        };
                                context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = @applicationStatus, Payment_Status_ID = @demandNotePaymentStatus Where Demand_Note_ID = @demandNoteId ", param1);
                            }

                            else
                            //context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre) + ", Payment_Status_ID = " + demand.PaymentStatusID + " Where Demand_Note_ID = " + demand.ID);
                            {
                                SqlParameter[] param2 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre)),
                                                            new SqlParameter("@demandNotePaymentStatus", demand.PaymentStatusID),
                                                                new SqlParameter("@demandNoteId", demand.ID)
                                                                        };
                                context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @demandNotePaymentStatus Where Demand_Note_ID = @demandNoteId ", param2);
                            }
                            context.SaveChanges();
                        }

                        else if (demand.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                        {
                            if (demand.enmDemandNoteType == enmDemandNoteType.Single)
                            {
                                CourseRegistrationApplication appl = context.CourseRegistrationApplications.Find(applID);
                                if (appl.enmApplicantType == enmApplicantType.Institute)
                                {
                                    //context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseApplicationStatus.AppliedButPendingForInstituteVerification) + ", Payment_Status_ID = " + demand.PaymentStatusID + " Where Demand_Note_ID = " + demand.ID);
                                    ////December_2024
                                    SqlParameter[] param3 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseApplicationStatus.AppliedButPendingForInstituteVerification)),
                                                            new SqlParameter("@demandNotePaymentStatus", demand.PaymentStatusID),
                                                                new SqlParameter("@demandNoteId", demand.ID)
                                                                        };
                                    context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @demandNotePaymentStatus Where Demand_Note_ID = @demandNoteId ", param3);
                                }
                                else
                                {
                                    // context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) + ", Payment_Status_ID = " + demand.PaymentStatusID + " Where Demand_Note_ID = " + demand.ID);
                                    //December_2024
                                    SqlParameter[] param4 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT)),
                                                            new SqlParameter("@demandNotePaymentStatus", demand.PaymentStatusID),
                                                                new SqlParameter("@demandNoteId", demand.ID)
                                                                        };
                                    context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @demandNotePaymentStatus Where Demand_Note_ID = @demandNoteId ", param4);


                                }
                            }
                            else
                            // context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT) + ", Payment_Status_ID = " + demand.PaymentStatusID + " Where Demand_Note_ID = " + demand.ID);
                            {
                                //December_2024
                                SqlParameter[] param5 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT)),
                                                            new SqlParameter("@demandNotePaymentStatus", demand.PaymentStatusID),
                                                                new SqlParameter("@demandNoteId", demand.ID)
                                                                        };
                                context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @demandNotePaymentStatus Where Demand_Note_ID = @demandNoteId ", param5);

                            }
                            context.SaveChanges();
                        }
                        else if (demand.enmApplicationType == enmApplicationType.CourseExamApplication)
                        {
                            if (demand.enmDemandNoteType == enmDemandNoteType.Single)
                            {
                                CourseExamApplication appl = context.CourseExamApplications.Find(applID);
                                if (appl.enmApplicantType == enmApplicantType.Institute)
                                {
                                    // context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) + ", Payment_Status_ID = " + demand.PaymentStatusID + " Where Demand_Note_ID = " + demand.ID);
                                    //December_2024
                                    SqlParameter[] param6 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification)),
                                                            new SqlParameter("@demandNotePaymentStatus", demand.PaymentStatusID),
                                                                new SqlParameter("@demandNoteId", demand.ID)
                                                                        };
                                    context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @demandNotePaymentStatus Where Demand_Note_ID = @demandNoteId ", param6);

                                }
                                else
                                {
                                    // context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) + ", Payment_Status_ID = " + demand.PaymentStatusID + " Where Demand_Note_ID = " + demand.ID);
                                    //December_2024
                                    SqlParameter[] param7 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT)),
                                                            new SqlParameter("@demandNotePaymentStatus", demand.PaymentStatusID),
                                                                new SqlParameter("@demandNoteId", demand.ID)
                                                                        };
                                    context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @demandNotePaymentStatus Where Demand_Note_ID = @demandNoteId ", param7);
                                }
                            }
                            else
                            // context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT) + ", Payment_Status_ID = " + demand.PaymentStatusID + " Where Demand_Note_ID = " + demand.ID);
                            {
                                //December_2024
                                SqlParameter[] param8 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT)),
                                                            new SqlParameter("@demandNotePaymentStatus", demand.PaymentStatusID),
                                                                new SqlParameter("@demandNoteId", demand.ID)
                                                                        };
                                context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @demandNotePaymentStatus Where Demand_Note_ID = @demandNoteId ", param8);
                            }


                            context.SaveChanges();
                        }
                        context.SaveChanges();
                        scope.Complete();
                        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../PaymentReceipt.aspx?DemandNoteid=" + DemandID + "&Src=DD"));
                    };
                };
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void RdoPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            MultiView1.Visible = true;
            switch (Convert.ToInt32(RdoPaymentMode.SelectedValue))
            {
                case 5:
                    MultiView1.ActiveViewIndex = 0;
                    break;
                case 2:
                    MultiView1.ActiveViewIndex = 1;
                    break;
                case 1:
                    MultiView1.ActiveViewIndex = 2;
                    break;
                case 6:
                    MultiView1.ActiveViewIndex = 3;
                    break;
                case 7:
                    MultiView1.ActiveViewIndex = 4;
                    break;
                default:
                    MultiView1.Visible = false;
                    break;
            }
        }
        catch (Exception ex)
        {
            LblError.Text = ex.Message;
        }
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SaveDD();
        }
        catch (Exception ex)
        {
            LblError.Text = ex.Message;
        }
    }
    protected void LnkBtnBack_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["RU"] != null)
            {
                //string link = HttpContext.Current.Request.UrlReferrer.ToString().Substring(0, HttpContext.Current.Request.UrlReferrer.ToString().LastIndexOf('/'));
                //link = link.Substring(0, link.LastIndexOf('/'));
                //link = link.Substring(0, link.LastIndexOf('/'));
                //link = link.Replace("CAND".ToUpper(), "WEB".ToUpper());
                //string url =  Request.QueryString["RU"];
                //url =  url.Substring(url.LastIndexOf('/'));
                //link = link + url;
                //Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt(link));
            }
        }
        catch (Exception ex)
        {
            LblError.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }
    protected void SaveElectronicTransfer()
    {
        try
        {
            if (isValidElectronicTransfer())
            {
                /*//Added for Neft check
                if (!validNEFTDate())
                {
                    throw new Exception("NEFT date must be before 15 Jan 2020, New payments through NEFT/RTGS is available in Online option");
                   
                }
                //*/
                Int64 applID = Convert.ToInt64(Request.QueryString["Appid"]);
                Int64 DemandID = Convert.ToInt64(Request.QueryString["DemandID"]);
                StringBuilder mySql = new StringBuilder();
                StringBuilder mySql1 = new StringBuilder();
                DateTime Neftdate = Convert.ToDateTime(txtneftdate.Text);
                String utrNumber = Txtnefttransno.Text.ToString();
                Txtneftamount.Enabled = false;
                Decimal amount = Convert.ToDecimal(Txtneftamount.Text.ToString());
                using (TransactionScope scope = new TransactionScope())
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        DemandNote demand = context.DemandNotes.Find(DemandID);
                        var coursExam = context.CourseExamApplications.Where(s => s.DemandNoteID == demand.ID).ToList();
                        var certificateExam = context.CertificateExamApplications.Where(s => s.DemandNoteID == demand.ID).ToList();
                        var coursReg = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demand.ID).ToList();
                        if (coursExam.Count() > 0 || certificateExam.Count() > 0 || coursReg.Count() > 0)
                        {
                            // Verify whether amount is refunded or not
                            if (context.NEFTRefund.Where(s => s.NeftBankTransaction.UTRNUMBER.ToUpper().Trim() == utrNumber.ToUpper().Trim() && System.Data.Entity.DbFunctions.TruncateTime(s.NeftBankTransaction.TransactionDate) == System.Data.Entity.DbFunctions.TruncateTime(Neftdate) && s.NeftBankTransaction.DemandNoteID == null && s.NeftBankTransaction.TransactionAmt == amount && s.NeftBankTransaction.TransactionAmt == demand.Amount).Count() == 0)
                            {
                                // Verify NEFT Transaction Details
                                if (context.NEFTBankTransactions.Any(s => s.UTRNUMBER.ToUpper().Trim() == utrNumber.ToUpper().Trim() && s.TransactionAmt == amount && s.TransactionAmt == demand.Amount && System.Data.Entity.DbFunctions.TruncateTime(s.TransactionDate) == System.Data.Entity.DbFunctions.TruncateTime(Neftdate) && s.DemandNoteID == null))
                                {
                                    if (demand.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                                    {
                                        var payee = (from p in context.CourseRegistrationApplications
                                                     where p.DemandNoteID == demand.ID
                                                     select new
                                                     {
                                                         appl = p,
                                                     }).FirstOrDefault();

                                        if (payee != null)
                                        {
                                            Int32 submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
                                            //if (payee.appl.LateFeeImposed) // no property for latefee in courseregistration
                                            //{
                                            //    submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                                            //}
                                            DateTime lastDate = (from c in context.CutOffDates
                                                                 where c.ActivityID == submisssionDateActivityID && c.ExamID == payee.appl.ApplicableExamID && c.ApplicantTypeID == payee.appl.ApplicantTypeID
                                                                 select c.EfferctiveDate).FirstOrDefault();

                                            Int32 NeftExtensionPeriod = payee.appl.ApplicableExam.NeftExtPeriod;

                                            // //deep add code on 13 April 2017

                                            int ApplicantTypeId = (from p in context.CourseRegistrationApplications where p.DemandNoteID == demand.ID select p.ApplicantTypeID).FirstOrDefault();
                                            if (ApplicantTypeId == 2) //2 for Institute Candidate, 1 for Direct Candidate
                                            {
                                                int FeeSubmissionOfInstituteExtensionPeriodInDays = payee.appl.ApplicableExam.FeeSubmissioInstituteExtPeriod;
                                                lastDate = lastDate.AddDays(FeeSubmissionOfInstituteExtensionPeriodInDays);

                                                if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                                                {
                                                    throw new Exception("You cannot verify your NEFT Transaction Details because NEFT Details Verification Period is Over.");
                                                }
                                                else
                                                {
                                                    if (Neftdate.Date > lastDate.Date)
                                                    {
                                                        throw new Exception("NEFT Transaction date cannot be greater than the last date of payment of registration fees ie:-" + lastDate.ToString("dd-MMM-yyyy"));
                                                    }
                                                }

                                            }
                                            else
                                            {

                                                if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                                                {
                                                    throw new Exception("You cannot verify your NEFT Transaction Details because NEFT Details Verification Period is Over.");
                                                }
                                                else
                                                {
                                                    if (Neftdate.Date > lastDate.Date)
                                                    {
                                                        throw new Exception("NEFT Transaction date cannot be greater than the last date of payment of registration fees ie:-" + lastDate.ToString("dd-MMM-yyyy"));
                                                    }
                                                }
                                            }
                                            //   //deep end code on 13 April 2017

                                            //if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                                            //{
                                            //    throw new Exception("You cannot verify your NEFT Transaction Details because NEFT Details Verification Period is Over.");
                                            //}
                                            //else
                                            //{
                                            //    if (Neftdate.Date > lastDate.Date)
                                            //    {
                                            //        throw new Exception("NEFT Transaction date cannot be greater than the last date of payment of registration fees ie:-" + lastDate.ToString("dd-MMM-yyyy"));
                                            //    }
                                            //}
                                        }
                                    }
                                    else if (demand.enmApplicationType == enmApplicationType.CourseExamApplication)
                                    {
                                        var payee = (from p in context.CourseExamApplications
                                                     where p.DemandNoteID == demand.ID
                                                     select new
                                                     {
                                                         appl = p,
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

                                            int latefeeID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                                            DateTime lastDateWithLateFee = (from c in context.CutOffDates
                                                                            where c.ActivityID == latefeeID && c.ExamID == payee.appl.ExamID && c.ApplicantTypeID == payee.appl.ApplicantTypeID
                                                                            select c.EfferctiveDate).FirstOrDefault();

                                            Int32 NeftExtensionPeriod = payee.appl.Exam.NeftExtPeriod;

                                            //deep add code on 19 april 2017

                                            int ApplicantTypeId = (from p in context.CourseExamApplications where p.DemandNoteID == demand.ID select p.ApplicantTypeID).FirstOrDefault();
                                            if (ApplicantTypeId == 2) //2 for Institute Candidate, 1 for Direct Candidate
                                            {
                                                int FeeSubmissionOfInstituteExtensionPeriodInDays = payee.appl.Exam.FeeSubmissioInstituteExtPeriod;
                                                lastDate = lastDate.AddDays(FeeSubmissionOfInstituteExtensionPeriodInDays);

                                                if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                                                {
                                                    throw new Exception("You cannot verify your NEFT Transaction Details because NEFT Details Verification Period is Over.");
                                                }
                                                else
                                                {
                                                    if (Neftdate.Date > lastDate.Date)
                                                    {
                                                        if ((payee.appl.FinalSubmitted && payee.appl.FinalSubmissionDate.Value.Date > lastDate.Date))
                                                        {
                                                            throw new Exception("NEFT Transaction date cannot be greater than the last date of payment of examination fees ie:-" + lastDate.ToString("dd-MMM-yyyy"));
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {

                                                if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                                                {
                                                    throw new Exception("You cannot verify your NEFT Transaction Details because NEFT Details Verification Period is Over.");
                                                }
                                                else
                                                {
                                                    if (Neftdate.Date > lastDate.Date)
                                                    {
                                                        if ((payee.appl.FinalSubmitted && payee.appl.FinalSubmissionDate.Value.Date > lastDate.Date))
                                                        {
                                                            throw new Exception("NEFT Transaction date cannot be greater than the last date of payment of examination fees ie:-" + lastDate.ToString("dd-MMM-yyyy"));
                                                        }
                                                    }
                                                }
                                            }

                                            //deep end add code on 19 april 2017

                                            // if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                                            // {
                                            //     throw new Exception("You cannot verify your NEFT Transaction Details because //NEFT Details Verification Period is Over.");
                                            // }
                                            // else
                                            // {
                                            //     if (Neftdate.Date > lastDate.Date)
                                            //    {
                                            //       if ((payee.appl.FinalSubmitted && payee.appl.FinalSubmissionDate.Value.Date //> lastDate.Date))
                                            //       {
                                            //           throw new Exception("NEFT Transaction date cannot be greater than the //last date of payment of examination fees ie:-" + lastDate.ToString("dd-MMM-yyyy"));
                                            //       }
                                            //   }
                                            // }
                                        }
                                    }
                                    else if (demand.enmApplicationType == enmApplicationType.CertificateExamApplication)
                                    {
                                        var payee = (from p in context.CertificateExamApplications
                                                     where p.DemandNoteID == demand.ID
                                                     select new
                                                     {
                                                         appl = p,
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

                                            Int32 NeftExtensionPeriod = payee.appl.Exam.NeftExtPeriod;

                                            if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                                            {
                                                throw new Exception("You cannot verify your NEFT Transaction Details because NEFT Details Verification Period is Over.");
                                            }
                                            else
                                            {
                                                if (Neftdate.Date > lastDate.Date)
                                                {
                                                    throw new Exception("NEFT Transaction date cannot be greater than the last date of payment of examination fees ie:-" + lastDate.ToString("dd-MMM-yyyy"));
                                                }
                                            }
                                        }
                                    }

                                    //to check same demand note no in two neft transactions
                                    if (context.NEFTTransactions.Where(s => s.DemandNoteID == demand.ID).Count() > 0)
                                    {
                                        throw new Exception("You have already made the payment for this demand-note.");
                                    }

                                    NEFTTransaction neft = new NEFTTransaction();
                                    neft.TransactionDate = Convert.ToDateTime(txtneftdate.Text.ToString());
                                    neft.TransactionNumber = Txtnefttransno.Text;
                                    neft.TransactionAmt = Convert.ToDecimal(Txtneftamount.Text);
                                    neft.TransactionBank = Txtneftbank.Text.ToUpper();
                                    neft.CreatedByID = 1;//Convert.ToInt32(Session["UserID"]);
                                    neft.Date = DateTime.Now;
                                    neft.DemandNoteID = DemandID;
                                    context.NEFTTransactions.Add(neft);
                                    context.SaveChanges();

                                    demand.NEFTTransactionID = neft.ID;
                                    demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.NEFTRTGS);
                                    //demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.PaidButNotVerified);
                                    demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid); // If NEFT transaction Matches
                                    context.Entry(demand).State = System.Data.Entity.EntityState.Modified;

                                    // Updating Neft_Bank_Transaction Details
                                    NEFTBankTransaction neftbank = context.NEFTBankTransactions.Where(s => s.UTRNUMBER.ToUpper().Trim() == utrNumber.ToUpper().Trim() && s.TransactionAmt == amount && s.TransactionAmt == demand.Amount && System.Data.Entity.DbFunctions.TruncateTime(s.TransactionDate) == System.Data.Entity.DbFunctions.TruncateTime(Neftdate) && s.DemandNoteID == null).FirstOrDefault();
                                    neftbank.DemandNoteID = DemandID;
                                    context.Entry(neftbank).State = System.Data.Entity.EntityState.Modified;

                                    //object application = null;
                                    if (demand.enmApplicationType == enmApplicationType.CertificateExamApplication)
                                    {
                                        if (demand.enmDemandNoteType == enmDemandNoteType.Single)
                                        //  context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                        {
                                            //December_2024
                                            SqlParameter[] param9 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                            context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param9);

                                        }
                                        else
                                        {
                                            if (context.CertificateExamApplications.Where(s => s.DemandNoteID.Value == demand.ID).FirstOrDefault().Exam.IsDispatchable)
                                            // context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                            {
                                                //December_2024
                                                SqlParameter[] param10 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                                context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param10);

                                            }
                                            else
                                            //   context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                            {
                                                //December_2024
                                                SqlParameter[] param11 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                                context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param11);

                                            }
                                        }
                                        context.SaveChanges();
                                    }
                                    else if (demand.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                                    {
                                        if (demand.enmDemandNoteType == enmDemandNoteType.Single)
                                        {
                                            CourseRegistrationApplication appl = context.CourseRegistrationApplications.Find(applID);
                                            if (appl.enmApplicantType == enmApplicantType.Institute)
                                            {
                                                // context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseApplicationStatus.AppliedButPendingForInstituteVerification) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                                //December_2024
                                                SqlParameter[] param12 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseApplicationStatus.AppliedButPendingForInstituteVerification)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                                context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param12);

                                            }
                                            else
                                            {
                                                //context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                                //December_2024
                                                SqlParameter[] param13 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                                context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param13);
                                            }
                                            context.SaveChanges();
                                        }
                                        else
                                        {
                                            //context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                            //  context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                            //December_2024
                                            SqlParameter[] param14 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                            context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param14);
                                            context.SaveChanges();

                                            ///////-------------------------Add here for registration number generation----------------------
                                            //if (demand.CourseCategoryID == 6) // for STC
                                            //{
                                            //    int crs = coursReg.FirstOrDefault().CourseID;                                                
                                            //    int examid = coursReg.FirstOrDefault().ApplicableExamID.Value;

                                            //    STCregistration.STCregistrationNumberAllocation(demand.ID, crs, demand.CourseCategoryID, examid);
                                            //}
                                            //else if (demand.CourseCategoryID == 0)
                                            //{
                                            //    int crs = coursReg.FirstOrDefault().CourseID;
                                            //    int crscatg = coursReg.FirstOrDefault().CourseCategoryID;
                                            //    int examid = coursReg.FirstOrDefault().ApplicableExamID.Value;

                                            //    if (coursReg.FirstOrDefault().CourseCategoryID == 6)   // for STC
                                            //        STCregistration.STCregistrationNumberAllocation(demand.ID, crs, crscatg, examid);
                                            //}
                                            //else { }
                                        }

                                        //    --
                                        //    --
                                        //    --
                                        ///////-----------------------------------------------------------------
                                    }
                                    else if (demand.enmApplicationType == enmApplicationType.CourseExamApplication)
                                    {
                                        if (demand.enmDemandNoteType == enmDemandNoteType.Single)
                                        {
                                            CourseExamApplication appl = context.CourseExamApplications.Find(applID);
                                            if (appl.enmApplicantType == enmApplicantType.Institute)
                                            {
                                                // context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                                //December_2024
                                                SqlParameter[] param15 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification)),
                                                                            new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                                new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                                context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param15);

                                            }
                                            else
                                            {
                                                // context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                                //December_2024
                                                SqlParameter[] param16 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                                context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param16);


                                            }
                                        }
                                        else
                                        //context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                        // context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                        {
                                            //December_2024
                                            SqlParameter[] param17 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                            context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param17);

                                        }
                                        context.SaveChanges();
                                    }
                                    context.SaveChanges();
                                    scope.Complete();
                                    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../PaymentReceipt.aspx?DemandNoteid=" + DemandID + "&Src=NEFT"));
                                }
                                else
                                {
                                    //Lbnefterror.Text = "Invalid NEFT/RTGS Transaction details. Please enter a valid transaction detail if you have made payment through NEFT/RTGS..";
                                    return;
                                }
                            }
                            else
                            {
                                //  Lbnefterror.Text = "This NEFT/RTGS Transaction Amount is Refunded. So You cannot verify this details.";
                                return;
                            }
                        }
                        else
                        {
                            throw new Exception("You have attempted to edit the application. Kindly re-submit to make payment.");
                        }
                    };
                };
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    //Function added for NEFT check
    protected bool validNEFTStartDate()
    {
        DateTime Neftdate = Convert.ToDateTime(txtneftdate.Text);

        if (Neftdate > Convert.ToDateTime("15-Jan-2020"))
            return (true);
        else
            return (false);

    }
    //
    //
    protected void Btnneftsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            SaveElectronicTransfer();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    //Function added for NEFT check
    protected bool validateNEFTDate()
    {
        DateTime Neftdate = Convert.ToDateTime(txtNeftDate2.Text);

        if (Neftdate <= Convert.ToDateTime("02/15/2021"))
            return (true);
        else
            return (false);

    }
    //
    //For NEFT validate
    protected void Btnneftsubmit2_Click(object sender, EventArgs e)
    {

        try
        {
            SaveElectronicTransfer2();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    //For NEFT Validate
    protected void SaveElectronicTransfer2()
    {
        try
        {
            if (isValidElectronicTransfer2())
            {
                //Added for Neft check
                if (!validateNEFTDate())
                {
                    throw new Exception("NEFT date must be before or equal to 15 Feb 2021, New payments through NEFT/RTGS is available in Other option w.e.f 16.02.2021");

                }
                //*/
                Int64 applID = Convert.ToInt64(Request.QueryString["Appid"]);
                Int64 DemandID = Convert.ToInt64(Request.QueryString["DemandID"]);
                StringBuilder mySql = new StringBuilder();
                StringBuilder mySql1 = new StringBuilder();
                DateTime Neftdate = Convert.ToDateTime(txtNeftDate2.Text);
                String utrNumber = txtNeftTransNo2.Text.ToString();
                txtNeftAmt2.Enabled = false;
                Decimal amount = Convert.ToDecimal(txtNeftAmt2.Text.ToString());
                using (TransactionScope scope = new TransactionScope())
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        DemandNote demand = context.DemandNotes.Find(DemandID);
                        var coursExam = context.CourseExamApplications.Where(s => s.DemandNoteID == demand.ID).ToList();
                        var certificateExam = context.CertificateExamApplications.Where(s => s.DemandNoteID == demand.ID).ToList();
                        var coursReg = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demand.ID).ToList();
                        if (coursExam.Count() > 0 || certificateExam.Count() > 0 || coursReg.Count() > 0)
                        {
                            // Verify whether amount is refunded or not
                            if (context.NEFTRefund.Where(s => s.NeftBankTransaction.UTRNUMBER.ToUpper().Trim() == utrNumber.ToUpper().Trim() && System.Data.Entity.DbFunctions.TruncateTime(s.NeftBankTransaction.TransactionDate) == System.Data.Entity.DbFunctions.TruncateTime(Neftdate) && s.NeftBankTransaction.DemandNoteID == null && s.NeftBankTransaction.TransactionAmt == amount && s.NeftBankTransaction.TransactionAmt == demand.Amount).Count() == 0)
                            {
                                // Verify NEFT Transaction Details
                                if (context.NEFTBankTransactions.Any(s => s.UTRNUMBER.ToUpper().Trim() == utrNumber.ToUpper().Trim() && s.TransactionAmt == amount && s.TransactionAmt == demand.Amount && System.Data.Entity.DbFunctions.TruncateTime(s.TransactionDate) == System.Data.Entity.DbFunctions.TruncateTime(Neftdate) && s.DemandNoteID == null))
                                {
                                    if (demand.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                                    {
                                        var payee = (from p in context.CourseRegistrationApplications
                                                     where p.DemandNoteID == demand.ID
                                                     select new
                                                     {
                                                         appl = p,
                                                     }).FirstOrDefault();

                                        if (payee != null)
                                        {
                                            Int32 submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
                                            //if (payee.appl.LateFeeImposed) // no property for latefee in courseregistration
                                            //{
                                            //    submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                                            //}
                                            DateTime lastDate = (from c in context.CutOffDates
                                                                 where c.ActivityID == submisssionDateActivityID && c.ExamID == payee.appl.ApplicableExamID && c.ApplicantTypeID == payee.appl.ApplicantTypeID
                                                                 select c.EfferctiveDate).FirstOrDefault();

                                            Int32 NeftExtensionPeriod = payee.appl.ApplicableExam.NeftExtPeriod;

                                            // //deep add code on 13 April 2017

                                            int ApplicantTypeId = (from p in context.CourseRegistrationApplications where p.DemandNoteID == demand.ID select p.ApplicantTypeID).FirstOrDefault();
                                            if (ApplicantTypeId == 2) //2 for Institute Candidate, 1 for Direct Candidate
                                            {
                                                int FeeSubmissionOfInstituteExtensionPeriodInDays = payee.appl.ApplicableExam.FeeSubmissioInstituteExtPeriod;
                                                lastDate = lastDate.AddDays(FeeSubmissionOfInstituteExtensionPeriodInDays);

                                                if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                                                {
                                                    throw new Exception("You cannot verify your NEFT Transaction Details because NEFT Details Verification Period is Over.");
                                                }
                                                else
                                                {
                                                    if (Neftdate.Date > lastDate.Date)
                                                    {
                                                        throw new Exception("NEFT Transaction date cannot be greater than the last date of payment of registration fees ie:-" + lastDate.ToString("dd-MMM-yyyy"));
                                                    }
                                                }

                                            }
                                            else
                                            {

                                                if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                                                {
                                                    throw new Exception("You cannot verify your NEFT Transaction Details because NEFT Details Verification Period is Over.");
                                                }
                                                else
                                                {
                                                    if (Neftdate.Date > lastDate.Date)
                                                    {
                                                        throw new Exception("NEFT Transaction date cannot be greater than the last date of payment of registration fees ie:-" + lastDate.ToString("dd-MMM-yyyy"));
                                                    }
                                                }
                                            }
                                            //   //deep end code on 13 April 2017

                                            //if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                                            //{
                                            //    throw new Exception("You cannot verify your NEFT Transaction Details because NEFT Details Verification Period is Over.");
                                            //}
                                            //else
                                            //{
                                            //    if (Neftdate.Date > lastDate.Date)
                                            //    {
                                            //        throw new Exception("NEFT Transaction date cannot be greater than the last date of payment of registration fees ie:-" + lastDate.ToString("dd-MMM-yyyy"));
                                            //    }
                                            //}
                                        }
                                    }
                                    else if (demand.enmApplicationType == enmApplicationType.CourseExamApplication)
                                    {
                                        var payee = (from p in context.CourseExamApplications
                                                     where p.DemandNoteID == demand.ID
                                                     select new
                                                     {
                                                         appl = p,
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

                                            int latefeeID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                                            DateTime lastDateWithLateFee = (from c in context.CutOffDates
                                                                            where c.ActivityID == latefeeID && c.ExamID == payee.appl.ExamID && c.ApplicantTypeID == payee.appl.ApplicantTypeID
                                                                            select c.EfferctiveDate).FirstOrDefault();

                                            Int32 NeftExtensionPeriod = payee.appl.Exam.NeftExtPeriod;

                                            //deep add code on 19 april 2017

                                            int ApplicantTypeId = (from p in context.CourseExamApplications where p.DemandNoteID == demand.ID select p.ApplicantTypeID).FirstOrDefault();
                                            if (ApplicantTypeId == 2) //2 for Institute Candidate, 1 for Direct Candidate
                                            {
                                                int FeeSubmissionOfInstituteExtensionPeriodInDays = payee.appl.Exam.FeeSubmissioInstituteExtPeriod;
                                                lastDate = lastDate.AddDays(FeeSubmissionOfInstituteExtensionPeriodInDays);

                                                if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                                                {
                                                    throw new Exception("You cannot verify your NEFT Transaction Details because NEFT Details Verification Period is Over.");
                                                }
                                                else
                                                {
                                                    if (Neftdate.Date > lastDate.Date)
                                                    {
                                                        if ((payee.appl.FinalSubmitted && payee.appl.FinalSubmissionDate.Value.Date > lastDate.Date))
                                                        {
                                                            throw new Exception("NEFT Transaction date cannot be greater than the last date of payment of examination fees ie:-" + lastDate.ToString("dd-MMM-yyyy"));
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {

                                                if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                                                {
                                                    throw new Exception("You cannot verify your NEFT Transaction Details because NEFT Details Verification Period is Over.");
                                                }
                                                else
                                                {
                                                    if (Neftdate.Date > lastDate.Date)
                                                    {
                                                        if ((payee.appl.FinalSubmitted && payee.appl.FinalSubmissionDate.Value.Date > lastDate.Date))
                                                        {
                                                            throw new Exception("NEFT Transaction date cannot be greater than the last date of payment of examination fees ie:-" + lastDate.ToString("dd-MMM-yyyy"));
                                                        }
                                                    }
                                                }
                                            }

                                            //deep end add code on 19 april 2017

                                            // if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                                            // {
                                            //     throw new Exception("You cannot verify your NEFT Transaction Details because //NEFT Details Verification Period is Over.");
                                            // }
                                            // else
                                            // {
                                            //     if (Neftdate.Date > lastDate.Date)
                                            //    {
                                            //       if ((payee.appl.FinalSubmitted && payee.appl.FinalSubmissionDate.Value.Date //> lastDate.Date))
                                            //       {
                                            //           throw new Exception("NEFT Transaction date cannot be greater than the //last date of payment of examination fees ie:-" + lastDate.ToString("dd-MMM-yyyy"));
                                            //       }
                                            //   }
                                            // }
                                        }
                                    }
                                    else if (demand.enmApplicationType == enmApplicationType.CertificateExamApplication)
                                    {
                                        var payee = (from p in context.CertificateExamApplications
                                                     where p.DemandNoteID == demand.ID
                                                     select new
                                                     {
                                                         appl = p,
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

                                            Int32 NeftExtensionPeriod = payee.appl.Exam.NeftExtPeriod;

                                            if (DateTime.Now.Date > lastDate.AddDays(NeftExtensionPeriod).Date)
                                            {
                                                throw new Exception("You cannot verify your NEFT Transaction Details because NEFT Details Verification Period is Over.");
                                            }
                                            else
                                            {
                                                if (Neftdate.Date > lastDate.Date)
                                                {
                                                    throw new Exception("NEFT Transaction date cannot be greater than the last date of payment of examination fees ie:-" + lastDate.ToString("dd-MMM-yyyy"));
                                                }
                                            }
                                        }
                                    }

                                    //to check same demand note no in two neft transactions
                                    if (context.NEFTTransactions.Where(s => s.DemandNoteID == demand.ID).Count() > 0)
                                    {
                                        throw new Exception("You have already made the payment for this demand-note.");
                                    }

                                    NEFTTransaction neft = new NEFTTransaction();
                                    neft.TransactionDate = Convert.ToDateTime(txtNeftDate2.Text.ToString());
                                    neft.TransactionNumber = txtNeftTransNo2.Text;
                                    neft.TransactionAmt = Convert.ToDecimal(txtNeftAmt2.Text);
                                    neft.TransactionBank = txtNeftBank2.Text.ToUpper();
                                    neft.CreatedByID = Convert.ToInt32(Session["UserID"]);
                                    neft.Date = DateTime.Now;
                                    neft.DemandNoteID = DemandID;
                                    context.NEFTTransactions.Add(neft);
                                    context.SaveChanges();

                                    demand.NEFTTransactionID = neft.ID;
                                    demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.NEFTRTGS);
                                    //demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.PaidButNotVerified);
                                    demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid); // If NEFT transaction Matches
                                    context.Entry(demand).State = System.Data.Entity.EntityState.Modified;

                                    // Updating Neft_Bank_Transaction Details
                                    NEFTBankTransaction neftbank = context.NEFTBankTransactions.Where(s => s.UTRNUMBER.ToUpper().Trim() == utrNumber.ToUpper().Trim() && s.TransactionAmt == amount && s.TransactionAmt == demand.Amount && System.Data.Entity.DbFunctions.TruncateTime(s.TransactionDate) == System.Data.Entity.DbFunctions.TruncateTime(Neftdate) && s.DemandNoteID == null).FirstOrDefault();
                                    neftbank.DemandNoteID = DemandID;
                                    context.Entry(neftbank).State = System.Data.Entity.EntityState.Modified;

                                    //object application = null;
                                    if (demand.enmApplicationType == enmApplicationType.CertificateExamApplication)
                                    {
                                        if (demand.enmDemandNoteType == enmDemandNoteType.Single)
                                        //  context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                        {
                                            //December_2024
                                            SqlParameter[] param18 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                            context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param18);
                                        }
                                        else
                                        {
                                            if (context.CertificateExamApplications.Where(s => s.DemandNoteID.Value == demand.ID).FirstOrDefault().Exam.IsDispatchable)
                                            // context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                            {
                                                //December_2024
                                                SqlParameter[] param19 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                                context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param19);
                                            }
                                            else
                                            //  context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                            {
                                                //December_2024
                                                SqlParameter[] param20 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                                context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param20);

                                            }
                                        }
                                        context.SaveChanges();
                                    }
                                    else if (demand.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                                    {
                                        if (demand.enmDemandNoteType == enmDemandNoteType.Single)
                                        {
                                            CourseRegistrationApplication appl = context.CourseRegistrationApplications.Find(applID);
                                            if (appl.enmApplicantType == enmApplicantType.Institute)
                                            {
                                                //  context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseApplicationStatus.AppliedButPendingForInstituteVerification) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                                //December_2024
                                                SqlParameter[] param21 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseApplicationStatus.AppliedButPendingForInstituteVerification)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                                context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param21);

                                            }
                                            else
                                            {
                                                // context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                                //December_2024
                                                SqlParameter[] param22 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                                context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param22);

                                            }
                                            context.SaveChanges();
                                        }
                                        else
                                        {
                                            //context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                            //   context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                            //December_2024
                                            SqlParameter[] param23 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                            context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param23);

                                            context.SaveChanges();

                                            ///////-------------------------Add here for registration number generation----------------------
                                            //if (demand.CourseCategoryID == 6) // for STC
                                            //{
                                            //    int crs = coursReg.FirstOrDefault().CourseID;                                                
                                            //    int examid = coursReg.FirstOrDefault().ApplicableExamID.Value;

                                            //    STCregistration.STCregistrationNumberAllocation(demand.ID, crs, demand.CourseCategoryID, examid);
                                            //}
                                            //else if (demand.CourseCategoryID == 0)
                                            //{
                                            //    int crs = coursReg.FirstOrDefault().CourseID;
                                            //    int crscatg = coursReg.FirstOrDefault().CourseCategoryID;
                                            //    int examid = coursReg.FirstOrDefault().ApplicableExamID.Value;

                                            //    if (coursReg.FirstOrDefault().CourseCategoryID == 6)   // for STC
                                            //        STCregistration.STCregistrationNumberAllocation(demand.ID, crs, crscatg, examid);
                                            //}
                                            //else { }
                                        }

                                        //    --
                                        //    --
                                        //    --
                                        ///////-----------------------------------------------------------------
                                    }
                                    else if (demand.enmApplicationType == enmApplicationType.CourseExamApplication)
                                    {
                                        if (demand.enmDemandNoteType == enmDemandNoteType.Single)
                                        {
                                            CourseExamApplication appl = context.CourseExamApplications.Find(applID);
                                            if (appl.enmApplicantType == enmApplicantType.Institute)
                                            {
                                                // context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                                //December_2024
                                                SqlParameter[] param24 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                                context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param24);
                                            }
                                            else
                                            {
                                                // context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                                //December_2024
                                                SqlParameter[] param25 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                                context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param25);


                                            }
                                        }
                                        else
                                        //context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                        // context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);
                                        {
                                            //December_2024
                                            SqlParameter[] param26 = { new SqlParameter("@applicationStatus", Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT)),
                                                                        new SqlParameter("@paymentStatusPaid", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                            new SqlParameter("@demandNoteId", demand.ID)
                                                                                    };
                                            context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = @applicationStatus , Payment_Status_ID = @paymentStatusPaid Where Demand_Note_ID = @demandNoteId ", param26);
                                        }
                                        context.SaveChanges();
                                    }
                                    context.SaveChanges();
                                    scope.Complete();
                                    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../PaymentReceipt.aspx?DemandNoteid=" + DemandID + "&Src=NEFT"));
                                }
                                else
                                {
                                    lblNeftError2.Text = "Invalid NEFT/RTGS Transaction details. Please enter a valid transaction detail if you have made payment through NEFT/RTGS..";
                                    return;
                                }
                            }
                            else
                            {
                                lblNeftError2.Text = "This NEFT/RTGS Transaction Amount is Refunded. So You cannot verify this details.";
                                return;
                            }
                        }
                        else
                        {
                            throw new Exception("You have attempted to edit the application. Kindly re-submit to make payment.");
                        }
                    };
                };
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    //Added for Neft validate
    protected bool isValidElectronicTransfer2()
    {
        try
        {
            DateTime todaydate = DateTime.Now;
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            DateTime Neftdate = Convert.ToDateTime(txtNeftDate2.Text);
            string utnumber = txtNeftTransNo2.Text.ToString();
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["TypeID"])))
                TypeID = Convert.ToInt32(Request.QueryString["TypeID"]);
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["Appid"])))
                appID = Convert.ToInt64(Request.QueryString["Appid"]);

            if (Neftdate > DateTime.Now.Date)
            {
                // Lbnefterror.Text = "NEFT Transaction date should not be greater than Current Date!";
                txtneftdate.Focus();
                return false;
            }


            if (!regexItem.IsMatch(utnumber))
            {
                //  Lbnefterror.Text = " Not a valid NEFT Transaction number!";
                Txtnefttransno.Focus();
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}