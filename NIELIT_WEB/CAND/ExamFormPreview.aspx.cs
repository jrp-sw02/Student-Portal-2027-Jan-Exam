using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web;
using System.Net;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class ExamFormPreview : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentCourseID = 0;
    Int64 applID = 0;
    Course currentcourse;
    DateTime examStartDate ;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {        
            Lblerror.Text = "";
            Lblerror.Visible = false;
            string path = Request.Url.ToString();            
            Int64 cand_id = Convert.ToInt64(Request.QueryString["candidateID"]);
            Int32 courseid = Convert.ToInt32(Request.QueryString["ID"]);
            examStartDate = Convert.ToDateTime(Request.QueryString["examStartDate"]);
           
            if (path.Contains("https://localhost:1447/NIELIT_WEB/CAND/ExamFormPreview.aspx") && cand_id != 0 && courseid == 0 && Convert.ToInt64(Request.QueryString["Appid"]) != 0)
            //if (path.Contains("https://student.nielit.gov.in/NIELIT_WEB/CAND/ExamFormPreview.aspx") && cand_id != 0 && courseid == 0 && Convert.ToInt64(Request.QueryString["Appid"]) != 0)
            {
                if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
                loginUserType = (UserType)Session["UserType"];
            }
            //if (IsSessionAlive() == false)
            //Response.Redirect("../Index.aspx");
            //loginUserType = (UserType)Session["UserType"];
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
                currentcourse = context.CourseExamApplications.Find(applID).Course;
                currentCourseID = currentcourse.ID;
                lbllevel.Text = currentcourse.Name;
                if (entityID == 0)
                {
                    entityID = context.CourseExamApplications.Where(s => s.ID == applID && s.CourseCategoryID == 1).Select(s => s.CandidateID).SingleOrDefault();
                }
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
                CourseExamApplication appl = context.CourseExamApplications.Find(applID);

                Int16 isEligible_oldExamPattern = CourseManager.Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL(appl.RegistrationNumber);


                if (appl.FinalSubmitted == true)
                    NormalHeader1.Visible = true;
                else
                {
                    BtnPrint.Visible = false;
                    lbllevel.Text = "PREVIEW: " + lbllevel.Text;
                    Lblerror.Text = "Dear Candidate, this is preview of examination form you are applying for. Please check all details in preview form and click on 'Final Submit' button if all the details are correct and you are satisfied with details shown in preview form. If you want to change the details click on 'Back' button to go back to examination form.<br> Once you click on 'Final Submit button, you can not modify the details and you will be asked for payment in next page.";
                    Lblerror.Visible = true;
                }
                Candidate candidate = appl.Candidate;
                if (appl.IsImprovementApplication)
                    lblImprovement.Text = "Yes";
                else
                    lblImprovement.Text = "No";
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


        
                //Exam Details
                Lblexam.Text = appl.Exam.Name;
                lblExamName.Text = Lblexam.Text.ToUpper();
                if (appl.ExamCenter1 != null)
                {
                    ExamCenter centre1 = appl.ExamCenter1;
                    ExamCenter centre2 = appl.ExamCenter2;
                    lblCentre1.Text = centre1.Code.ToUpper() + " (" + GetInitCap(centre1.Name) + ")";
                    lblCentre2.Text = centre2.Code.ToUpper() + " (" + GetInitCap(centre2.Name) + ")";

                    TrOfflineExam1.Visible = true;
                    TrOfflineExam2.Visible = true;

                }
                //Added by Reena Practical Centre
                if (appl.PracExamCenter1 != null)
                {
                    PracExamCenter Praccentre1 = appl.PracExamCenter1;
                    PracExamCenter Praccentre2 = appl.PracExamCenter2;
                    lblPracCentre1.Text = Praccentre1.Code.ToUpper() + " (" + GetInitCap(Praccentre1.Name) + ")";
                    lblPracCentre2.Text = Praccentre2.Code.ToUpper() + " (" + GetInitCap(Praccentre2.Name) + ")";

                    TrPracExam1.Visible = true;
                    TrPracExam2.Visible = true;
                }
                //End

                //Added by Reena Online Exam as on Centre (04/05/2023)
                if (appl.OnlineExamCenter1 != null)
                {
                    OnlineExamCenter Onlinecentre1 = appl.OnlineExamCenter1;
                    OnlineExamCenter Onlinecentre2 = appl.OnlineExamCenter2;
                    lblOnlCentre1.Text = Onlinecentre1.Code.ToUpper() + " (" + GetInitCap(Onlinecentre1.Name) + ")";
                    lblOnlCentre2.Text = Onlinecentre2.Code.ToUpper() + " (" + GetInitCap(Onlinecentre2.Name) + ")";

                    TrOnlineExam1.Visible = true;
                    TrOnlineExam2.Visible = true;
                }
                //End

                var modules = (from a in context.CourseExamApplicationDetails
                               join m in context.Modules on a.ModuleID equals m.ID
                               where a.CourseExamApplicationID == applID
                               orderby m.Code
                               select new { m.ShortName, m.Name, m.ModuleTypeID, a.FeeAmount,m.TheoryWithPractical });
                Int32 practical = Convert.ToInt32(enmModuleType.Practical);
                Int32 theory = Convert.ToInt32(enmModuleType.Theory);            
                Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
                lblPracticals.Text = "";
                lblTheoryModules.Text = "";
                Int32 practicalFee = 0;
                Int32 theoryFee = 0;
                Int32 theoryWithPractical = 0;
                Int32 practicalCount = 0;
                Int32 theoryCount = 0;
                Int32 practicalWithTheory = 0;
                foreach (var module in modules.ToList())
                {
                    if (module.ModuleTypeID == practical && module.TheoryWithPractical == 0)
                    {
                        practicalCount++;
                        if (lblPracticals.Text.Length > 0)
                            lblPracticals.Text += "<br>";
                        lblPracticals.Text += practicalCount.ToString().PadLeft(2, '0') + ". &nbsp;" + module.ShortName + " &nbsp;&nbsp;" + module.Name;
                        practicalFee += (Int32)module.FeeAmount;
                    }
                    else if ((module.ModuleTypeID == theory || module.ModuleTypeID == bridge) && module.TheoryWithPractical == 0)
                    {
                        theoryCount++;
                        if (lblTheoryModules.Text.Length > 0)
                            lblTheoryModules.Text += "<br>";
                        lblTheoryModules.Text += theoryCount.ToString().PadLeft(2, '0') + ". &nbsp;" + module.ShortName + " &nbsp;&nbsp;" + module.Name;
                        theoryFee += (Int32)module.FeeAmount;
                    }
                    else if (module.ModuleTypeID == theory && module.TheoryWithPractical != 0)
                    {
                        theoryWithPractical++;
                        if (lblTheoryModules.Text.Length > 0)
                            lblTheoryModules.Text += "<br>";
                        lblTheoryModules.Text += theoryWithPractical.ToString().PadLeft(2, '0') + ". &nbsp;" + module.ShortName + " &nbsp;&nbsp;" + module.Name;
                      //  lblPracticals.Text += practicalCount.ToString().PadLeft(2, '0') + ". &nbsp;" + module.ShortName + " &nbsp;&nbsp;" + module.Name;
                        theoryFee += (Int32)module.FeeAmount;
                    }
                    else if (module.ModuleTypeID == practical && module.TheoryWithPractical != 0)
                    {
                       // practicalCount++;
                        practicalWithTheory++;
                        if (lblPracticals.Text.Length > 0)
                            lblPracticals.Text += "<br>";
                        lblPracticals.Text += practicalWithTheory.ToString().PadLeft(2, '0') + ". &nbsp;" + module.ShortName + " &nbsp;&nbsp;" + module.Name;
                       // practicalFee += (Int32)module.FeeAmount;
                    }                    
                }
               
                if (lblPracticals.Text == "")
                    lblPracticals.Text = "NA";
                else
                {
                    if (appl.CourseID == 2 || appl.CourseID == 1)
                    {
                        if (isEligible_oldExamPattern == 0)
                        {
                            lblPractialCount.Text = " (" + practicalCount.ToString() + " x " + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee).ToString("F") + ")";
                        }
                        else
                        {
                            lblPractialCount.Text = " (" + practicalCount.ToString() + " x " + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee).ToString("F") + ")";
                        }
                    }
                    else
                    {
                      //  if (isEligible_oldExamPattern == 0)
                       // {
                            lblPractialCount.Text = " (" + practicalCount.ToString() + " x " + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee).ToString("F") + ")";
                      //  }
                      //  else
                      //  {
                      //      lblPractialCount.Text = " (" + practicalCount.ToString() + " x " + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PracticalFee).ToString("F") + ")";
                       // }
                    }
                }
                if (lblTheoryModules.Text == "")
                    lblTheoryModules.Text = "NA";
                else
                {
                    if (appl.IsImprovementApplication == true)
                        lblTheoryCount.Text = " (" + theoryCount.ToString() + " x " + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ImprovementOfPaperFee).ToString("F") + ")";
                    else
                    {
                        if (appl.CourseID == 2 || appl.CourseID == 1)
                        {

                            if (isEligible_oldExamPattern == 0)
                            {
                                lblTheoryCount.Text = " (" + theoryCount.ToString() + " x " + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee).ToString("F") +" + " +  theoryWithPractical.ToString() + " x " + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.TheorywithPracticalFee).ToString("F") +")";
                            }
                            else
                            {
                                lblTheoryCount.Text = " (" + theoryCount.ToString() + " x " + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee).ToString("F") + ")";
                            }
                        }
                        else
                        {
                            if (appl.CourseID == 1213)
                            {
                                lblTheoryCount.Text = " (" + theoryCount.ToString() + " x " + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee).ToString("F") + " + " + theoryWithPractical.ToString() + " x " + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.TheorywithPracticalFee).ToString("F") + ")";
                            }
                            else
                            {
                                //if (isEligible_oldExamPattern == 0)
                                // {
                                lblTheoryCount.Text = " (" + theoryCount.ToString() + " x " + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee).ToString("F") + ")";
                                // }
                                //else
                                // {
                                // lblTheoryCount.Text = " (" + theoryCount.ToString() + " x " + CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.ExaminationFee).ToString("F") + ")";
                                //  }
                            }
                        }
                    }
                }
                //Fee Details
                lblTheoryFee.Text = theoryFee.ToString("F");
                lblPracticalFee.Text = practicalFee.ToString("F");
                tdProcessingFee.InnerText = CourseManager.GetAppliableFee(context, currentCourseID, enmFeeType.PostageFeeChargedTowardExaminationCorrespondence).ToString("F");
                if (appl.LateFeeImposed)
                    Lbllatefee.Text = appl.LateFeeAmount.Value.ToString("F");

                lblTotalFee.Text = appl.FeeAmount.ToString("F");
                lblFeeInWords.Text = EConnect.Utils.Conversion.ConversionUtility.NumberToText(appl.FeeAmount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English);
                LblPaymentDescription.Text += "NA";

                //Payment Details
                LblPaymentMode.Text = "NA";
                Int32 submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                if (appl.LateFeeImposed)
                {
                    submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                }
                DateTime lastDate = (from c in context.CutOffDates
                                     where c.ActivityID == submisssionDateActivityID && c.ExamID == appl.ExamID && c.ApplicantTypeID == appl.ApplicantTypeID
                                     select c.EfferctiveDate).FirstOrDefault();
                DateTime lastDateFormSubmition = lastDate;
                //lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");

                //   Adde by deep on 31-Jan-2016  1  
                int FeeSubmissionOfInstituteExtensionPeriodInDays = (from c in context.Exams
                                                                     where c.ID == appl.ExamID && c.CourseID == appl.CourseID && c.CourseCategoryID == appl.CourseCategoryID
                                                                     select c.FeeSubmissioInstituteExtPeriod).FirstOrDefault();
                // appl.CourseID=1 only for the O A B C LEVEL ONLINE EXAMINATION FORM 
                if (appl.enmApplicantType == enmApplicantType.Institute && appl.enmPaymentSource == enmPaymentSource.Institute && appl.CourseID == 1)
                {
                    lastDate = lastDate.AddDays(FeeSubmissionOfInstituteExtensionPeriodInDays);
                    lblLastDate.Text = lastDateFormSubmition.ToString("dd-MMM-yyyy");
                }
                else
                {
                    lblLastDate.Text = lastDate.ToString("dd-MMM-yyyy");
                }
                // //deep end  1
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
                            LblPaymentDescription.Text = "NA";
                    }
                    else
                    {
                        LblPaymentMode.Text = "NA";
                    }
                    Btnsubmit.Visible = false;
                    btnback.Visible = false;
                    //if (!string.IsNullOrEmpty(Request.QueryString["Chk"]))
                    //{
                    //    if (Request.QueryString["Chk"] == "Y" && (currentCourseID == 1 || currentCourseID == 2))
                    //        //Btnsubmit12.Visible = true;
                    //}
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
            CourseExamApplication appl = context.CourseExamApplications.Find(Convert.ToInt64(Request.QueryString["AppID"]));
            currentCourseID = appl.Course.ID;
            Int64 registrationNumber = appl.RegistrationNumber;
            context.SaveChanges();
            
            if ((!String.IsNullOrEmpty(Request.QueryString["Appid"])) && (Request.QueryString["Src"] == "CSC"))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("FrmExamForm.aspx?RegNo=" + registrationNumber.ToString() + "&CourseID=" + currentCourseID.ToString() + "&Appid=" + Request.QueryString["AppID"] + "&Src=CSC"));
            }
            else
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("FrmExamForm.aspx?RegNo=" + registrationNumber.ToString() + "&CourseID=" + currentCourseID.ToString()  +"&examStartDate=" + examStartDate.ToString() + "&Appid=" + Request.QueryString["AppID"]));
            }
        };
    }

    //protected void Btnsubmit12_Click(object sender, EventArgs e)
    //{
    //    //Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("frmDashboard.aspx"));
    //    using (TransactionScope scope = new TransactionScope())
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            CourseExamApplication appl = context.CourseExamApplications.Find(applID);
    //            CourseExamApplication app = new CourseExamApplication();
    //            var application = (from a in context.CourseExamApplications
    //                               where a.ID == applID
    //                               select a).FirstOrDefault();
    //            application.NSQFRollNo = "Y";
    //            context.Entry(application).State = System.Data.Entity.EntityState.Modified;
    //            context.SaveChanges();
    //            scope.Complete();
    //        };
    //    };
    //    Response.Redirect("../Index.aspx");
    //}

    protected void Btnsubmit_Click(object sender, EventArgs e)
    {
        string nsqfRollno = string.Empty;
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
                    CourseExamApplication appl = context.CourseExamApplications.Find(applID);
                    
                   
                       
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

                        CourseExamApplication app = new CourseExamApplication();
                        var application = (from a in context.CourseExamApplications
                                           where a.ID == applID
                                           select a).FirstOrDefault();
                        application.FinalSubmitted = true;
                        application.FinalSubmissionDate = DateTime.Now;
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
                                demand.ApplicationTypeID = Convert.ToInt32(enmApplicationType.CourseExamApplication);
                                demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.CSCSPV);
                                demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Single);
                                demand.Amount = application.FeeAmount;
                                demand.CreatedBy = 1;
                                demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                                // Added by vivek on 13-6-2015
                                demand.CourseCategoryID = application.CourseCategoryID;
                                demand.CourseID = application.CourseID;
                                demand.ServiceID = application.Course.ExaminationServiceID;
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
                                if (appl.enmPaymentSource == enmPaymentSource.Institute)
                                {
                                    application.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification);
                                    ApplicationFlag = 0;
                                }
                                else
                                {
                                    demand.ApplicationDate = DateTime.Now;
                                    demand.FeeTypeID = application.FeeTypeID.Value;
                                    demand.ApplicationTypeID = Convert.ToInt32(enmApplicationType.CourseExamApplication);
                                    demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.CSCSPV);
                                    demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Single);
                                    demand.Amount = application.FeeAmount;
                                    demand.CreatedBy = 1;
                                    demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                                    // Added by vivek on 13-6-2015
                                    demand.CourseCategoryID = application.CourseCategoryID;
                                    demand.CourseID = application.CourseID;
                                    demand.ServiceID = application.Course.ExaminationServiceID;
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
                        mobileMsg = "You have applied for  " + application.Exam.Name + " exam (" + application.Course.Name + ") on " + application.ApplicationDate.ToString("dd-MMM-yyyy hh:mm") + " . Your application no." +
                          " is " + application.Number + " and your Registration Number is: " + application.RegistrationNumber + "-NIELIT";


                        msg = "Dear " + GetInitCap(candidate.Salutation + " " + candidate.Name) + ",<br/><br/>" + "You have successfully applied for " + application.Exam.Name +
                            " (" + application.Course.Name + ") Examination through Online Student Information and Enrollment System of NIELIT. <br/>" +
                            " Your online application details are as following: <br/><br/>" +
                            "Application Number &nbsp;:  " + application.Number + "<br/>" +
                            "Application Date &nbsp;:  " + application.ApplicationDate.ToString("dd-MMM-yyyy") + "<br/><br/><br/>" +
                            "Please note your application number.It will be used for further correspondence with NIELIT.";
                        if (application.enmApplicantType == enmApplicantType.Direct)
                        {
                            msg += "<br><br><I>Note: You have not paid the applicable examination fee at the time of form submission. " +
                                " Please pay you applicable examination fee by the last date mentioned in the form using any of the available payment options.<br><br>" +
                                " Important: Please refer your Demand Note Number mentioned in the form at the time of payment at CSC/SPV. Your Demand Note Number is: " + appl.DemandNote.ID.ToString();
                        }
                        else if (application.enmApplicantType == enmApplicantType.Institute)
                        {
                            if (appl.enmPaymentSource == enmPaymentSource.Institute)
                            {
                                msg += "<br><br><I>Note: You have not paid the applicable examination fee at the time of form submission. " +
                                " Please pay you applicable examination fee to the institute (" + appl.Institute.Name + ") by the last date mentioned in the form.<br><br>";
                                // " Important: Please refer your Demand Note Number mentioned in the form at the time of payment at CSC/SPV. Your Demand Note Number is: " + appl.DemandNote.ID.ToString();
                            }
                            else
                            {
                                msg += "<br><br><I>Note: You have not paid the applicable examination fee at the time of form submission. " +
                                 " Please pay you applicable examination fee by the last date mentioned in the form using any of the available payment options.<br><br>" +
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
                    ShowAlert(ex.Message);
                }
                //sending Mobile Message
                try
                {
                    if (mobileNumber != 0)
                    {
                        //Sending Mobile Message
                        EConnect.NIELIT.SMS message = new SMS(mobileMsg, mobileNumber.ToString(), "1307161053022237088", SmsServiceType.SignleSMS);
                        int sentMessageCount;
                        message.sendSingleSMS(out sentMessageCount);
                    }
                }
                catch (Exception ex)
                {
                    ShowAlert(ex.Message);
                }

                if ((!String.IsNullOrEmpty(Request.QueryString["Appid"])) && (Request.QueryString["Src"] == "CSC"))
                    Response.Redirect("../nieletpaymentservices.aspx?ServiceID=4&DID=" + demandID.ToString());
                else if (ApplicationFlag == 1)//Application type: Direct
                    Response.Redirect("~/CAND/FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CourseExamApplication).ToString() + "&Appid=" + applID.ToString() + "&DemandID=" + demandID.ToString(), true);
                else if (ApplicationFlag == 0)//Application type:Institute
                    Response.Redirect("~/CAND/FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.CourseExamApplication).ToString() + "&Appid=" + applID.ToString() + "&DemandID=0", true);
            
        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
            Lblerror.Visible = true;
        }
    }
}