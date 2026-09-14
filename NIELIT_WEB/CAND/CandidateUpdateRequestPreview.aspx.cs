using System;
using System.Data.Objects;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Collections.Generic;

public partial class CAND_CandidateUpdateRequestPreview : BasePage
{
    
    UserType loginUserType;
      Int64 RegApplID;
      Int64 candidateID;//= Convert.ToInt64(Request.QueryString["CandidateID"]); //37068; (Request.QueryString
      Int64 Request_Number;
     
   
    protected void Page_Load(object sender, EventArgs e)
    {
       
        try
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache"); 
            Response.Expires = -1500; 
            //if (Request.UrlReferrer == null && Request.QueryString["CandidateID"] == null)
            //{
            //    //Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
            //    //Response.End();
            //    //return;
            //}
            if (!Page.IsPostBack)
            {
                if ((Request.QueryString["regStatusId"] == "3")||(Request.QueryString["regStatusId"] == "12")) //Final submit
                {
                    tblDocs.Visible = true;
                    Btnback.Visible = false;
                    Btnsubmit.Visible = false;
                    BtnProcess.Visible = true;
                }

                else
                {
                    tblDocs.Visible = false;
                    Btnback.Visible = true;
                    //Btnsubmit.Text = "Process"; 
                }
                showdata();
            }
        }
        catch (Exception ex)
        {
            
            Lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        } 
    }
    protected String GeInvalidRequestMessage(String redirectText, String redirectPage)
    {
        return "<B>Invalid Request Parameters</B><br><a href='" + redirectPage + "'>" + redirectText + "</a>";
    }
    protected void showdata()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
               // Btnback.Visible = false; 
                Int64 candidateID = Convert.ToInt64(Request.QueryString["CandidateID"]); //37068; 
                Int64 RegApplID = Convert.ToInt64(Request.QueryString["RegApplID"]);
                Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);                
                Int32 regStatusId = Convert.ToInt32(Request.QueryString["regStatusId"]);
                Int64 demandID = Convert.ToInt64(Request.QueryString["demandID"]);
                ViewState["demandID"] = Convert.ToInt64(Request.QueryString["demandID"]);

                #region[Preview_Before_Final_Submit]
                 //var RequestUpdation = context.candidateUpdateRequest.Find(candidateID);
                //Int64 a = candidateID;
                RegistrationDetail RegID = context.RegistrationDetails.Find(RegApplID);
                Lblhead.Text = "Online Request for Updation/Correction Preview";

                //Int32 Doc_Upload = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);
                Int32 PayCase = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);
                Int32 FreeCase = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedforfreecases);
                var Request_Number1 = (from c in context.candidateUpdateRequest
                                       where c.candID == candidateID //&& c.requestStatusID == PayCase || c.requestStatusID == FreeCase  //2
                                       && c.requestNo == Request_Number 
                                       orderby c.enterdate descending

                                      select new{req=c.requestNo, reqdate= c.enterdate}).FirstOrDefault();
                
                //Int64 Reqid = Request_Number.req;                  

                LblCourseLevel.Text = RegID.Course.Name;
                LblRegnNumber.Text = RegID.RegistrationNo.ToString();
                LblReqNumber.Text = Request_Number.ToString();//Request_Number1.req.ToString();
                LblReqDataTime.Text = Request_Number1.reqdate.ToString("dd-MMM-yyyy");

                Int32 Upd_Field_Handi = Convert.ToInt32(enmCandidateUpdateRequestFields.Handicapped);
                Int32 App_Status = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);

                var Handicapped = (from s in context.candidateUpdateRequest
                                   orderby (s.updateFieldID)
                                   where s.updateFieldID == Upd_Field_Handi &&
                                   s.demandNoteID == null && s.requestStatusID == App_Status
                                   && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                   select new {ValueField= s.changedValue,TextField = s.enterdate }).FirstOrDefault();                            
          
                               
                if (Handicapped!= null)
                {
                    //Int64 CandQuID = Convert.ToInt64(Handicapped.ValueField);
                    LblHandicapped.Text = Handicapped.ValueField;
                    TrHandicapped.Visible = true;
                    //LblReqDataTime.Text = Handicapped.TextField.ToString("dd-MMM-yyyy");
                }
                else { LblHandicapped.Text = "--"; }

                Int32 Upd_Field_Matrial = Convert.ToInt32(enmCandidateUpdateRequestFields.MaritalStatus);

                var marital = (from s in context.candidateUpdateRequest
                               orderby (s.updateFieldID)
                               where s.updateFieldID == Upd_Field_Matrial &&
                               s.demandNoteID == null && s.requestStatusID == App_Status //1
                               && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                               select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();
                if (marital != null)
                {
                    LblMaritalStatus.Text = marital.ValueField;
                    TrMaritalStatus.Visible = true;
                }
                else { LblMaritalStatus.Text = "--"; }

                Int32 Upd_Field_category = Convert.ToInt32(enmCandidateUpdateRequestFields.CasteCategory);
                var category = (from s in context.candidateUpdateRequest
                                orderby (s.updateFieldID)
                                where s.updateFieldID == Upd_Field_category &&
                                s.demandNoteID == null && s.requestStatusID == App_Status //1
                                && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                               select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();
                if (category != null)
                {
                    LblCategory.Text = category.ValueField;
                    TrCategory.Visible = true;
                }
                else { LblCategory.Text = "--"; }

                Int32 Upd_Field_Mobile = Convert.ToInt32(enmCandidateUpdateRequestFields.MobileNumber);
                var Mobile = (from s in context.candidateUpdateRequest
                              orderby (s.updateFieldID)
                              where s.updateFieldID == Upd_Field_Mobile &&
                              s.demandNoteID == null && s.requestStatusID == App_Status //1
                              && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                              select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();
                if (Mobile != null)
                {
                    LblMobile.Text = Mobile.ValueField;
                    TrMobile.Visible = true;
                }
                else { LblMobile.Text = "--"; }

                Int32 Upd_Field_Email = Convert.ToInt32(enmCandidateUpdateRequestFields.EmailAddress);
                var Email = (from s in context.candidateUpdateRequest
                             orderby (s.updateFieldID)
                             where s.updateFieldID == Upd_Field_Email &&
                             s.demandNoteID == null && s.requestStatusID == App_Status
                             && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                             select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();
                if (Email != null)
                {
                    LblEmail.Text = Email.ValueField;
                    TrEmail.Visible = true;
                }
                else { LblEmail.Text = "--"; }

                Int32 Upd_Field_Address = Convert.ToInt32(enmCandidateUpdateRequestFields.CorrespondenceAddress);
                var CorrespondenceAddress = (from s in context.candidateUpdateRequest
                                             orderby (s.updateFieldID)
                                             where s.updateFieldID == Upd_Field_Address &&
                                             s.demandNoteID == null && s.requestStatusID == App_Status
                                             && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                             select new { changevalue = s.changedValue, Date = s.enterdate }).FirstOrDefault();
                if (CorrespondenceAddress != null)
                {
                    string complete_add = CorrespondenceAddress.changevalue;
                    string[] valuesList = complete_add.Split(';');
                    LblCorAddressLine1.Text = valuesList[0].ToString();
                    LblCorAddressLine2.Text = valuesList[1].ToString();
                    if (valuesList[2] == "")
                    { LblCorAddressLine3.Text = "--"; }
                    else
                    {
                        LblCorAddressLine3.Text = valuesList[2].ToString(); //application.CorStateID != 0 ? GetInitCap(application.CorState.Name) : "";
                    }
                    lblCorCity.Text = valuesList[3].ToString();
                    LblCorState.Text = valuesList[4].ToString();
                    LblDistrict.Text = valuesList[5].ToString();
                    LblCorPinCode.Text = valuesList[6].ToString();

                    TRheaderCorAdd.Visible = true;
                    TrAdd1.Visible = true;
                    TrAdd2.Visible = true;
                    TrAdd3.Visible = true;
                    TrCityDist.Visible = true;
                    TrStatePin.Visible = true;


                }

                Int32 Upd_Field_HighEducation = Convert.ToInt32(enmCandidateUpdateRequestFields.EducationalQualification);
                 var HighEducation = (from s in context.candidateUpdateRequest
                                      orderby (s.updateFieldID)
                                      where s.updateFieldID == Upd_Field_HighEducation &&
                                      s.demandNoteID == null && s.requestStatusID == App_Status
                                      && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                      select new { HighEdu = s.changedValue, TextField = s.enterdate }).FirstOrDefault();

                if (HighEducation != null)
                {
                    LblHeighEducation.Text = GetInitCap(HighEducation.HighEdu);
                    TRheaderEdu.Visible = true;
                    TrHighEducation.Visible = true;
                }
                else { LblHeighEducation.Text = "--"; }
                #endregion

                #region[Final_Submit_Preview_Pay_Cases]
                if (regStatusId == Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid))//3-final submit then show doc details
                {
                    #region[Claer_Text]
                    LblHandicapped.Text = "";
                    LblMaritalStatus.Text = "";
                    LblCategory.Text = "";
                    LblMobile.Text = "";
                    LblEmail.Text = "";
                    LblCorAddressLine1.Text ="";
                    LblCorAddressLine1.Text ="";
                    LblCorAddressLine1.Text = "";
                    lblCorCity.Text = "";
                    LblCorState.Text = "";
                    LblDistrict.Text = "";
                    LblCorPinCode.Text = "";
                    LblHeighEducation.Text = "";
                    
                    #endregion

                    Int32 Upd_Field_Handicapped_Final = Convert.ToInt32(enmCandidateUpdateRequestFields.Handicapped);
                    Int32 App_Status_submittedbutfeesnotpaid = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);
                    var Handicapped_Final = (from s in context.candidateUpdateRequest
                                             orderby (s.updateFieldID)
                                             where s.updateFieldID == Upd_Field_Handicapped_Final &&
                                             s.demandNoteID == demandID && s.requestStatusID == App_Status_submittedbutfeesnotpaid // s.demandNoteID != null
                                             && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                             select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();

                    
                    if (Handicapped_Final != null)
                    {
                        //Int64 CandQuID = Convert.ToInt64(Handicapped.ValueField);

                        LblHandicapped.Text = Handicapped_Final.ValueField;
                        TrHandicapped.Visible = true;
                        //LblReqDataTime.Text = Handicapped.TextField.ToString("dd-MMM-yyyy");
                    }
                    else { LblHandicapped.Text = "--"; }

                    Int32 Upd_Field_marital_Final = Convert.ToInt32(enmCandidateUpdateRequestFields.MaritalStatus);
                    var marital_Final = (from s in context.candidateUpdateRequest
                                         orderby (s.updateFieldID)
                                         where s.updateFieldID == Upd_Field_marital_Final &&
                                         s.demandNoteID == demandID && s.requestStatusID == App_Status_submittedbutfeesnotpaid
                                         && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                         select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();

                    if (marital_Final != null)
                    {
                        LblMaritalStatus.Text = marital_Final.ValueField;
                        TrMaritalStatus.Visible = true; 
                    }
                    else { LblMaritalStatus.Text = "--"; }

                    Int32 Upd_Field_category_Final = Convert.ToInt32(enmCandidateUpdateRequestFields.CasteCategory);
                    var category_Final = (from s in context.candidateUpdateRequest
                                          orderby (s.updateFieldID)
                                          where s.updateFieldID == Upd_Field_category_Final &&
                                          s.demandNoteID == demandID && s.requestStatusID == App_Status_submittedbutfeesnotpaid
                                          && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                          select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();

                    if (category_Final != null)
                    {
                        LblCategory.Text = category_Final.ValueField;
                        TrCategory.Visible = true;
                    }
                    else { LblCategory.Text = "--"; }

                    Int32 Upd_Field_Mobile_Final = Convert.ToInt32(enmCandidateUpdateRequestFields.MobileNumber);
                    var Mobile_Final = (from s in context.candidateUpdateRequest
                                        orderby (s.updateFieldID)
                                        where s.updateFieldID == Upd_Field_Mobile_Final &&
                                        s.demandNoteID == demandID && s.requestStatusID == App_Status_submittedbutfeesnotpaid
                                        && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                        select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();

                    if (Mobile_Final != null)
                    {
                        LblMobile.Text = Mobile_Final.ValueField;
                        TrMobile.Visible = true;
                    }
                    else { LblMobile.Text = "--"; }

                    Int32 Upd_Field_Email_Final = Convert.ToInt32(enmCandidateUpdateRequestFields.EmailAddress);
                    var Email_Final = (from s in context.candidateUpdateRequest
                                       orderby (s.updateFieldID)
                                       where s.updateFieldID == Upd_Field_Email_Final &&
                                       s.demandNoteID == demandID && s.requestStatusID == App_Status_submittedbutfeesnotpaid
                                       && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                       select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();
                    if (Email_Final != null)
                    {
                        LblEmail.Text = Email_Final.ValueField;
                        TrEmail.Visible = true;
                    }
                    else { LblEmail.Text = "--"; }

                    Int32 Upd_Field_Address_Final = Convert.ToInt32(enmCandidateUpdateRequestFields.CorrespondenceAddress);
                    var CorrespondenceAddress_Final = (from s in context.candidateUpdateRequest
                                                       orderby (s.updateFieldID)
                                                       where s.updateFieldID == Upd_Field_Address_Final &&
                                                       s.demandNoteID == demandID && s.requestStatusID == App_Status_submittedbutfeesnotpaid
                                                       && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                                       select new { changevalue = s.changedValue, Date = s.enterdate }).FirstOrDefault();

                    if (CorrespondenceAddress_Final != null)
                    {
                        string complete_add = CorrespondenceAddress_Final.changevalue;
                        string[] valuesList = complete_add.Split(';');
                        LblCorAddressLine1.Text = valuesList[0].ToString();
                        LblCorAddressLine2.Text = valuesList[1].ToString();
                        if (valuesList[2] == "")
                        { LblCorAddressLine3.Text = "--"; }
                        else
                        {
                            LblCorAddressLine3.Text = valuesList[2].ToString(); //application.CorStateID != 0 ? GetInitCap(application.CorState.Name) : "";
                        }
                        lblCorCity.Text = valuesList[3].ToString();
                        LblCorState.Text = valuesList[4].ToString();
                        LblDistrict.Text = valuesList[5].ToString();
                        LblCorPinCode.Text = valuesList[6].ToString();

                        TRheaderCorAdd.Visible = true;
                        TrAdd1.Visible = true;
                        TrAdd2.Visible = true;
                        TrAdd3.Visible = true;
                        TrCityDist.Visible = true;
                        TrStatePin.Visible = true;

                    }

                    Int32 Upd_Field_HighEducation_Final = Convert.ToInt32(enmCandidateUpdateRequestFields.EducationalQualification);
                    var HighEducation_Final = (from s in context.candidateUpdateRequest
                                               orderby (s.updateFieldID)
                                               where s.updateFieldID == Upd_Field_HighEducation_Final &&
                                               s.demandNoteID == demandID && s.requestStatusID == App_Status_submittedbutfeesnotpaid
                                               && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                               select new { HighEdu = s.changedValue, TextField = s.enterdate }).FirstOrDefault();
                    if (HighEducation_Final != null)
                    {
                        LblHeighEducation.Text = GetInitCap(HighEducation_Final.HighEdu);
                        TRheaderEdu.Visible = true;
                        TrHighEducation.Visible = true;
                    }
                    else { LblHeighEducation.Text = "--"; }

                    #region[old]
                    //var DocDetails = (from s in context.candidateUpdateRequest
                    //                  where s.candID == candidateID && s.requestStatusID == regStatusId && s.requestNo == Request_Number
                    //                  select new { Docname = s.FileUploadedPath, finalSubmitStatus = s.requestStatusID,Fee = s.feeAmount }).FirstOrDefault();
                    //if (DocDetails != null)
                    //{
                    //    if (DocDetails.finalSubmitStatus == 3)//check final submit regstatus & show fee and docs Deatails 
                    //    {
                    //        Lblerror.Text = "";
                    //        Btnsubmit.Visible = false;
                    //        Btnback.Visible = false;
                    //        BtnPrint.Visible = true;
                    //        NormalHeader1.Visible = true;
                    //        tblDocs.Visible = true;
                    //        TrFeeDetails.Visible = true;

                    //        if (DocDetails != null)
                    //        {
                    //            tblDocs.Visible = true;
                    //            LblDocDetails.Text = DocDetails.Docname;
                    //            LblFeeAmount.Text = DocDetails.Fee.ToString() + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(DocDetails.Fee.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";//RequestUpdation.feeAmount.ToString();

                    //        }

                    //    }
                    //}
                 #endregion

                    Int32 PaymentStatus = Convert.ToInt32(enmPaymentStatus.Pending);
                    var Display_All_Doc = context.candidateUpdateRequest.Where(x => x.candID == candidateID && x.requestNo == Request_Number && x.requestStatusID == regStatusId && x.paymentStatusID == PaymentStatus && x.requestFinalised==true).ToList();
                 
                    if (Display_All_Doc != null)
                    {
                       
                        foreach (var Docs in Display_All_Doc)
                        {
                            string str = Docs.FileUploadedPath;
                            string[] valuesList = str.Split('\\');
                            string DocList = valuesList[7].ToString();

                            //LblDocDetails.Text += DocList + " , ";
                            LblDocDetails.Text += "<br/>";
                            LblDocDetails.Text +="<b>"+ DocList + " <b/> ";  

                            
                            // Console.WriteLine(DocList);
                            //LblDocDetails.Text += list.Items.Add(DocList);
                         
                        }

                        Lblerror.Text = "";
                        Btnsubmit.Visible = false;
                        Btnback.Visible = false;
                        BtnPrint.Visible = true;
                        NormalHeader1.Visible = true;
                        tblDocs.Visible = true;
                        TrFeeDetails.Visible = true;

                 
                        //var DocDetails = (from s in context.UpdateRequestDemandNotes
                        //                  join c in context.candidateUpdateRequest
                        //                  on s.ID equals c.demandNoteID
                        //                  where s.CreatedBy == candidateID && s.PaymentStatusID == 1 
                        //                  select new { Fee = s.Amount }).FirstOrDefault();
                        var DocDetails = (from s in context.UpdateRequestDemandNotes
                                          where s.ID == demandID 
                                          select new { Fee = s.Amount }).FirstOrDefault();

                        LblFeeAmount.Text = DocDetails.Fee.ToString() + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(DocDetails.Fee.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";//RequestUpdation.feeAmount.ToString();
                        string TotFee = DocDetails.Fee.ToString();
                        ViewState["Fee"] = DocDetails.Fee;
                    }
                }
                #endregion

                #region[Final_Submit_Preview_Free_Cases]

                if (regStatusId == Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedforfreecases))//12-final submit for free cases then show doc details
                    {
                    #region[Claer_Text]
                    LblHandicapped.Text = "";
                    LblMaritalStatus.Text = "";
                    LblCategory.Text = "";
                    LblMobile.Text = "";
                    LblEmail.Text = "";
                    LblCorAddressLine1.Text = "";
                    LblCorAddressLine1.Text = "";
                    LblCorAddressLine1.Text = "";
                    lblCorCity.Text = "";
                    LblCorState.Text = "";
                    LblDistrict.Text = "";
                    LblCorPinCode.Text = "";
                    LblHeighEducation.Text = "";

                    #endregion

                    Int32 Upd_Field_Handicapped_Final = Convert.ToInt32(enmCandidateUpdateRequestFields.Handicapped);
                    Int32 App_Status_Formfinalsubmittedforfreecases = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedforfreecases);
                    var Handicapped_Final = (from s in context.candidateUpdateRequest
                                             orderby (s.updateFieldID)
                                             where s.updateFieldID == Upd_Field_Handicapped_Final &&
                                             s.requestStatusID == App_Status_Formfinalsubmittedforfreecases // s.demandNoteID != null
                                             && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                             select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();


                    if (Handicapped_Final != null)
                        {
                        //Int64 CandQuID = Convert.ToInt64(Handicapped.ValueField);

                        LblHandicapped.Text = Handicapped_Final.ValueField;
                        TrHandicapped.Visible = true;
                        //LblReqDataTime.Text = Handicapped.TextField.ToString("dd-MMM-yyyy");
                        }
                    else { LblHandicapped.Text = "--"; }

                    Int32 Upd_Field_marital_Final = Convert.ToInt32(enmCandidateUpdateRequestFields.MaritalStatus);
                    var marital_Final = (from s in context.candidateUpdateRequest
                                         orderby (s.updateFieldID)
                                         where s.updateFieldID == Upd_Field_marital_Final &&
                                         s.requestStatusID == App_Status_Formfinalsubmittedforfreecases
                                         && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                         select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();

                    if (marital_Final != null)
                        {
                        LblMaritalStatus.Text = marital_Final.ValueField;
                        TrMaritalStatus.Visible = true;
                        }
                    else { LblMaritalStatus.Text = "--"; }

                    Int32 Upd_Field_category_Final = Convert.ToInt32(enmCandidateUpdateRequestFields.CasteCategory);
                    var category_Final = (from s in context.candidateUpdateRequest
                                          orderby (s.updateFieldID)
                                          where s.updateFieldID == Upd_Field_category_Final &&
                                           s.requestStatusID == App_Status_Formfinalsubmittedforfreecases
                                          && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                          select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();

                    if (category_Final != null)
                        {
                        LblCategory.Text = category_Final.ValueField;
                        TrCategory.Visible = true;
                        }
                    else { LblCategory.Text = "--"; }

                    Int32 Upd_Field_Mobile_Final = Convert.ToInt32(enmCandidateUpdateRequestFields.MobileNumber);
                    var Mobile_Final = (from s in context.candidateUpdateRequest
                                        orderby (s.updateFieldID)
                                        where s.updateFieldID == Upd_Field_Mobile_Final &&
                                        s.requestStatusID == App_Status_Formfinalsubmittedforfreecases
                                        && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                        select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();

                    if (Mobile_Final != null)
                        {
                        LblMobile.Text = Mobile_Final.ValueField;
                        TrMobile.Visible = true;
                        }
                    else { LblMobile.Text = "--"; }

                    Int32 Upd_Field_Email_Final = Convert.ToInt32(enmCandidateUpdateRequestFields.EmailAddress);
                    var Email_Final = (from s in context.candidateUpdateRequest
                                       orderby (s.updateFieldID)
                                       where s.updateFieldID == Upd_Field_Email_Final &&
                                       s.requestStatusID == App_Status_Formfinalsubmittedforfreecases
                                       && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                       select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();
                    if (Email_Final != null)
                        {
                        LblEmail.Text = Email_Final.ValueField;
                        TrEmail.Visible = true;
                        }
                    else { LblEmail.Text = "--"; }

                    Int32 Upd_Field_Address_Final = Convert.ToInt32(enmCandidateUpdateRequestFields.CorrespondenceAddress);
                    var CorrespondenceAddress_Final = (from s in context.candidateUpdateRequest
                                                       orderby (s.updateFieldID)
                                                       where s.updateFieldID == Upd_Field_Address_Final &&
                                                       s.requestStatusID == App_Status_Formfinalsubmittedforfreecases
                                                       && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                                       select new { changevalue = s.changedValue, Date = s.enterdate }).FirstOrDefault();

                    if (CorrespondenceAddress_Final != null)
                        {
                        string complete_add = CorrespondenceAddress_Final.changevalue;
                        string[] valuesList = complete_add.Split(';');
                        LblCorAddressLine1.Text = valuesList[0].ToString();
                        LblCorAddressLine2.Text = valuesList[1].ToString();
                        if (valuesList[2] == "")
                        { LblCorAddressLine3.Text = "--"; }
                        else
                            {
                            LblCorAddressLine3.Text = valuesList[2].ToString(); //application.CorStateID != 0 ? GetInitCap(application.CorState.Name) : "";
                            }
                        lblCorCity.Text = valuesList[3].ToString();
                        LblCorState.Text = valuesList[4].ToString();
                        LblDistrict.Text = valuesList[5].ToString();
                        LblCorPinCode.Text = valuesList[6].ToString();

                        TRheaderCorAdd.Visible = true;
                        TrAdd1.Visible = true;
                        TrAdd2.Visible = true;
                        TrAdd3.Visible = true;
                        TrCityDist.Visible = true;
                        TrStatePin.Visible = true;


                    }

                    Int32 Upd_Field_HighEducation_Final = Convert.ToInt32(enmCandidateUpdateRequestFields.EducationalQualification);
                    var HighEducation_Final = (from s in context.candidateUpdateRequest
                                               orderby (s.updateFieldID)
                                               where s.updateFieldID == Upd_Field_HighEducation_Final &&
                                               s.requestStatusID == App_Status_Formfinalsubmittedforfreecases
                                               && s.candID == candidateID && Request_Number == s.requestNo && s.requestFinalised == true
                                               select new { HighEdu = s.changedValue, TextField = s.enterdate }).FirstOrDefault();
                    if (HighEducation_Final != null)
                        {
                        LblHeighEducation.Text = GetInitCap(HighEducation_Final.HighEdu);
                        TRheaderEdu.Visible = true;
                        TrHighEducation.Visible = true;
                        }
                    else { LblHeighEducation.Text = "--"; }

                    #region[old]
                    //var DocDetails = (from s in context.candidateUpdateRequest
                    //                  where s.candID == candidateID && s.requestStatusID == regStatusId && s.requestNo == Request_Number
                    //                  select new { Docname = s.FileUploadedPath, finalSubmitStatus = s.requestStatusID,Fee = s.feeAmount }).FirstOrDefault();
                    //if (DocDetails != null)
                    //{
                    //    if (DocDetails.finalSubmitStatus == 3)//check final submit regstatus & show fee and docs Deatails 
                    //    {
                    //        Lblerror.Text = "";
                    //        Btnsubmit.Visible = false;
                    //        Btnback.Visible = false;
                    //        BtnPrint.Visible = true;
                    //        NormalHeader1.Visible = true;
                    //        tblDocs.Visible = true;
                    //        TrFeeDetails.Visible = true; 

                    //        if (DocDetails != null)
                    //        {
                    //            tblDocs.Visible = true;
                    //            LblDocDetails.Text = DocDetails.Docname;
                    //            LblFeeAmount.Text = DocDetails.Fee.ToString() + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(DocDetails.Fee.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";//RequestUpdation.feeAmount.ToString();

                    //        }

                    //    }
                    //}
                    #endregion

                    //Int32 PaymentStatus = Convert.ToInt32(enmPaymentStatus.Pending);
                    var Display_All_Doc = context.candidateUpdateRequest.Where(x => x.candID == candidateID && x.requestNo == Request_Number && x.requestStatusID == regStatusId &&  x.requestFinalised == true).ToList();

                    if (Display_All_Doc != null)
                        {

                        foreach (var Docs in Display_All_Doc)
                            {
                            string str = Docs.FileUploadedPath;
                            //  string[] valuesList = str.Split('\\');
                            string DocList = System.IO.Path.GetFileName(str);//valuesList[7].ToString();

                            //LblDocDetails.Text += DocList + " , ";
                            LblDocDetails.Text += "<br/>";
                            LblDocDetails.Text += "<b>" + DocList + " <b/> ";


                            // Console.WriteLine(DocList);
                            //LblDocDetails.Text += list.Items.Add(DocList);

                            }

                        Lblerror.Text = "";
                        Btnsubmit.Visible = false;
                        Btnback.Visible = false;
                        BtnPrint.Visible = true;
                        NormalHeader1.Visible = true;
                        tblDocs.Visible = true;
                        TrFeeDetails.Visible = true;

                        BtnProcess.Visible = false;// false for the free cgases


                        //var DocDetails = (from s in context.UpdateRequestDemandNotes
                        //                  join c in context.candidateUpdateRequest
                        //                  on s.ID equals c.demandNoteID
                        //                  where s.CreatedBy == candidateID && s.PaymentStatusID == 1 
                        //                  select new { Fee = s.Amount }).FirstOrDefault();
                        var DocDetails = (from s in context.UpdateRequestDemandNotes
                                          where s.ID == demandID
                                          select new { Fee = s.Amount }).FirstOrDefault();

                        //LblFeeAmount.Text = DocDetails.Fee.ToString() + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(DocDetails.Fee.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";//RequestUpdation.feeAmount.ToString();
                        LblFeeAmount.Text = "<b><i>" + "Not Applicable" + "</i> <b/> ";
                        //string TotFee = DocDetails.Fee.ToString();
                        //ViewState["Fee"] = DocDetails.Fee;
                        }
                    }
                #endregion

                else
                    {
                        Lblerror.Text = "Dear Candidate, this is preview of Updation Request form you are applying for. Please check all details in preview form and click on 'Upload Documents' button if all the details are correct and you are satisfied with details shown in preview form. If you want to change the details click on 'Edit' button to go back to Updation Request form.<br> Once you click on 'Upload Documents button, you can not modify the details and you will be asked for Documents Upload in next page.";
                        Lblerror.Visible = true;
                        NormalHeader1.Visible = false;
                    }
               
              
            }
        }
        catch (Exception ex)
        {
                  //catch (Exception ex) { throw ex; }
            Lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        } 
    }
    protected void Btnback_Click(object sender, EventArgs e) //EditData
    {
        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);
        Int64 candidateID = Convert.ToInt64(Request.QueryString["CandidateID"]); //37068; 
        Int64 RegApplID = Convert.ToInt64(Request.QueryString["RegApplID"]);
        //Btnback_Click(null, null);
        Response.Redirect("../CAND/CandidateUpdateRequest.aspx?Request_Number=" + Request_Number +"&candidateID=" + candidateID + "&RegApplID=" + RegApplID);
       

        //if (!String.IsNullOrEmpty(Request.QueryString["RegApplID"]))
        //{
        //    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequest.aspx?RegApplID=" + RegApplID + "&Request_Number=" + Request_Number));
        //}
        //else
        //{
        //    Int64 candidateID = Convert.ToInt64(Request.QueryString["CandidateID"]); //37068; 
        //    Int64 RegApplID = Convert.ToInt64(Request.QueryString["RegApplID"]);
        //    Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

        //    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequest.aspx?RegApplID=" + RegApplID + "&Request_Number=" + Request_Number));
        //}
    }
    protected void Btnsubmit_Click(object sender, EventArgs e)
    {
        
        if (!String.IsNullOrEmpty(Request.QueryString["id"]))
        {
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestDocUpload.aspx?candidateID=" + candidateID + "&RegApplID=" + RegApplID + "&Request_Number=" + Request_Number));
        }
        else
        {
            Int64 candidateID = Convert.ToInt64(Request.QueryString["CandidateID"]); //37068; 
            Int64 RegApplID = Convert.ToInt64(Request.QueryString["RegApplID"]);
            Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

            ViewState["candidateID"] = Convert.ToInt64(Request.QueryString["CandidateID"]);
            ViewState["RegApplID"] = Convert.ToInt64(Request.QueryString["RegApplID"]);
            ViewState["Request_Number"] = Convert.ToInt64(Request.QueryString["RegApplID"]);
            

            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestDocUpload.aspx?candidateID=" + candidateID + "&RegApplID=" + RegApplID + "&Request_Number=" + Request_Number));
           // Response.Redirect("../CAND/CandidateUpdateRequestDocUpload.aspx?candidateID=" + candidateID + "&RegApplID=" + RegApplID + "&Request_Number=" + Request_Number);
        }
    }
protected void Final_Submit() //Insert data to demmand draft
{ 
    try
        {
            using (EConnectContext context = new EConnectContext())
            {}
}
    catch (Exception ex)
        {                
            Lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        } 



}

//protected void BtnProcess_Click(object sender, EventArgs e)//payment Process
//{
//    Int64 DID = Convert.ToInt64(Request.QueryString["demandID"]);
//    Response.Redirect("~/OnlinePaymentRegnUpdate.aspx?DID=" + DID);
//}
protected void BtnProcess_Click(object sender, EventArgs e)//payment Process
{
    Int64 DID = Convert.ToInt64(Request.QueryString["demandID"]);
    Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);
    Int64 candidateID = Convert.ToInt64(Request.QueryString["CandidateID"]); //37068; 
   // var TotFee = ViewState["Fee"];


    //Response.Redirect("../CAND/CandidateUpdateConfirm.aspx?DID=" + DID + "&TotFee=" + TotFee + "&Request_Number=" + Request_Number + "&candidateID=" + candidateID);
    Response.Redirect("~/CAND/CandidateUpdateConfirm.aspx?DID=" + DID + "&Request_Number=" + Request_Number + "&candidateID=" + candidateID);
}
}

