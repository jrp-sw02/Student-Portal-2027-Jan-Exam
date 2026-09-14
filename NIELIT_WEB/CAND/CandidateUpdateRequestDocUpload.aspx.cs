using System;
using System.Data.Objects;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data;
using System.Net;
using System.IO;
using System.Web;

public partial class CAND_CandidateUpdateRequestDocUpload : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0 ;
    Int32 currentCourseID = 0;
    Int64 RegApplID = 2754656;//1951796;
    Course currentcourse;
   // Int64 requestNo = 0;    
    String updateFieldID = string.Empty;
    String strMessage = string.Empty;
       
    Int64 Request_Number = 0;
    EConnectContext context1 = new EConnectContext();
   


    #region[Page_Load_Event]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Lblerror.Text = "";
            Lblerror.Visible = false;
            NormalHeader1.Visible = true;                    

            Lblmsg.Text = "Dear Candidate,Plaese Upload all document carefully, Once you click on 'Final submit button, you can not Upload again.";
            Lblmsg.Visible = true;
            //NormalHeader1.Visible = false;


           // Int64 CandiID = Convert.ToInt64(Request.QueryString["candidateID"]);
            Int32 courseid = Convert.ToInt32(Request.QueryString["ID"]);

           
            
               // Int64 candidateID = Convert.ToInt64(Request.QueryString["CandidateID"]); //37068; 
                Int64 RegApplID = Convert.ToInt64(Request.QueryString["RegApplID"]);
                Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);
        

            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Index.aspx");
            //loginUserType = (UserType)Session["UserType"];
            //entityID = Convert.ToInt64(Session["EntityID"]);
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

            // RegApplID  = Convert.ToInt64(Request.QueryString["RegApplID"]);
            using (EConnectContext context = new EConnectContext())
            {
                Lblhead.Text = "Upload Required Document";
                currentcourse = context.RegistrationDetails.Find(RegApplID).Course;
                currentCourseID = currentcourse.ID;
                // LblCourseLevel.Text = currentcourse.Name;

                if (entityID == 0)
                {
                    entityID = context.RegistrationDetails.Where(s => s.ID == RegApplID && s.CourseCategoryID == 1).Select(s => s.CandidateID).SingleOrDefault();
                }
            }
            if (!IsPostBack)
            {
               // if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["entityID"])))
                
                if (entityID != 0)
                {
                   UploadRequireDocVisible();
                }

                else
                {
                    //DisableAll();
                    //DisplayAll();
                    ////Lbregno.Text = Reg.RegistrationNo.ToString();                  
                    //bindMaritalStatus();
                    //bindCastCategory();
                    //bindState();
                    //bindEducationalQualification();
                }
            }
            //UpdateSelection(); 
           
        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message;
        }
    }
    #endregion

    #region[Dispaly_Require_Document_To_Upload_As_Per_Request]
    protected void UploadRequireDocVisible()
    {
        using (EConnectContext context = new EConnectContext())
        {
            Int64 RegApplID = Convert.ToInt64(Request.QueryString["RegApplID"]);
            Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

            //Checking Data exists for the request field, if exists updateFieldID then enable filed for Upload Docs
            Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);
            Int32 App_Status_DocumentsUploaded = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);
            Int32 App_Status_submittedbutfeesnotpaid = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);

            Int32 Upd_Field_Handi = Convert.ToInt32(enmCandidateUpdateRequestFields.Handicapped);
            Int32 FieldID_handi = context.candidateUpdateRequest.Where(s => s.candID == entityID && s.updateFieldID == Upd_Field_Handi &&
                                  s.requestStatusID == App_Status_RequestReceived && s.requestFinalised == true &&
                                  s.requestNo == Request_Number).Select(s => s.updateFieldID).SingleOrDefault();
            if (FieldID_handi != 0)
            { trHandicapped.Visible = true; }

            Int32 Upd_Field_Matrial = Convert.ToInt32(enmCandidateUpdateRequestFields.MaritalStatus);
            Int32 FieldID_Marital = context.candidateUpdateRequest.Where(s => s.candID == entityID && s.updateFieldID == Upd_Field_Matrial &&
                                    s.requestStatusID == App_Status_RequestReceived && s.requestFinalised == true ).Select(s => s.updateFieldID).SingleOrDefault();
            if (FieldID_Marital != 0)
            { trMarital.Visible = true; }

            Int32 Upd_Field_Caste = Convert.ToInt32(enmCandidateUpdateRequestFields.CasteCategory);
            Int32 FieldID_Caste = context.candidateUpdateRequest.Where(s => s.candID == entityID && s.updateFieldID == Upd_Field_Caste &&
                                  s.requestStatusID == App_Status_RequestReceived && s.requestFinalised == true).Select(s => s.updateFieldID).SingleOrDefault();
            if (FieldID_Caste != 0)
            { trCasteCerti.Visible = true; }

            Int32 Upd_Field_Mobile = Convert.ToInt32(enmCandidateUpdateRequestFields.MobileNumber);
            Int32 FieldID_Mobile = context.candidateUpdateRequest.Where(s => s.candID == entityID && s.updateFieldID == Upd_Field_Mobile &&
                                   s.requestStatusID == App_Status_RequestReceived && s.requestFinalised == true).Select(s => s.updateFieldID).SingleOrDefault();
            if (FieldID_Mobile != 0)
            { trMobile.Visible = true; }

            Int32 Upd_Field_Email = Convert.ToInt32(enmCandidateUpdateRequestFields.EmailAddress);
            Int32 FieldID_Email = context.candidateUpdateRequest.Where(s => s.candID == entityID && s.updateFieldID == Upd_Field_Email &&
                                  s.requestStatusID == App_Status_RequestReceived && s.requestFinalised == true).Select(s => s.updateFieldID).SingleOrDefault();
            if (FieldID_Email != 0)
            { trEmail.Visible = true; }

            Int32 Upd_Field_Address = Convert.ToInt32(enmCandidateUpdateRequestFields.CorrespondenceAddress);
            Int32 FieldID_CorAddress = context.candidateUpdateRequest.Where(s => s.candID == entityID && s.updateFieldID == Upd_Field_Address &&
                                       s.requestStatusID == App_Status_RequestReceived && s.requestFinalised == true).Select(s => s.updateFieldID).SingleOrDefault();
            if (FieldID_CorAddress != 0)
            { trCorAddress.Visible = true; }

            Int32 Upd_Field_HighEdu = Convert.ToInt32(enmCandidateUpdateRequestFields.EducationalQualification);
            Int32 FieldID_HighEdu = context.candidateUpdateRequest.Where(s => s.candID == entityID && s.updateFieldID == Upd_Field_HighEdu &&
                                    s.requestStatusID == App_Status_RequestReceived && s.requestFinalised == true).Select(s => s.updateFieldID).SingleOrDefault();
            if (FieldID_HighEdu != 0)
            { trHighEducation.Visible = true; }


            Int32 Upd_Candi_Cert = Convert.ToInt32(enmCandidateUpdateRequestFields.Name);
            Int32 FieldID_Name = context.candidateUpdateRequest.Where(s => s.candID == entityID && s.updateFieldID == Upd_Candi_Cert &&
                                    s.requestStatusID == App_Status_RequestReceived && s.requestFinalised == true).Select(s => s.updateFieldID).SingleOrDefault();
            if (FieldID_Name != 0)
            { trCandidate.Visible = true; }

            Int32 Upd_Father_Cert = Convert.ToInt32(enmCandidateUpdateRequestFields.FatherName);
            Int32 FieldID_FatherName = context.candidateUpdateRequest.Where(s => s.candID == entityID && s.updateFieldID == Upd_Father_Cert &&
                                    s.requestStatusID == App_Status_RequestReceived && s.requestFinalised == true).Select(s => s.updateFieldID).SingleOrDefault();
            if (FieldID_FatherName != 0)
            { trFather.Visible = true; }

            Int32 Upd_Mother_Cert = Convert.ToInt32(enmCandidateUpdateRequestFields.MotherName);
            Int32 FieldID_MotherName = context.candidateUpdateRequest.Where(s => s.candID == entityID && s.updateFieldID == Upd_Mother_Cert &&
                                    s.requestStatusID == App_Status_RequestReceived && s.requestFinalised == true).Select(s => s.updateFieldID).SingleOrDefault();
            if (FieldID_MotherName != 0)
            { trMother.Visible = true; } 

            Int32 Upd_DOB_Cert = Convert.ToInt32(enmCandidateUpdateRequestFields.DOB);
            Int32 FieldID_DOB = context.candidateUpdateRequest.Where(s => s.candID == entityID && s.updateFieldID == Upd_DOB_Cert &&
                                    s.requestStatusID == App_Status_RequestReceived && s.requestFinalised == true).Select(s => s.updateFieldID).SingleOrDefault();
            if (FieldID_DOB != 0)
            { trDob.Visible = true; }

            Int32 Upd_Gender_Cert = Convert.ToInt32(enmCandidateUpdateRequestFields.Gender);
            Int32 FieldID_Gender = context.candidateUpdateRequest.Where(s => s.candID == entityID && s.updateFieldID == Upd_Gender_Cert &&
                                    s.requestStatusID == App_Status_RequestReceived && s.requestFinalised == true).Select(s => s.updateFieldID).SingleOrDefault();
            if (FieldID_Gender != 0)
            { trGender.Visible = true; }


            Int32 Upd_PermanentAddress_Cert = Convert.ToInt32(enmCandidateUpdateRequestFields.PermanentAddress);
            Int32 FieldID_PermanentAddress = context.candidateUpdateRequest.Where(s => s.candID == entityID && s.updateFieldID == Upd_PermanentAddress_Cert &&
                                    s.requestStatusID == App_Status_RequestReceived && s.requestFinalised == true).Select(s => s.updateFieldID).SingleOrDefault();
            if (FieldID_PermanentAddress != 0)
            { trPerAddress.Visible = true; } 


            Int32 Upd_RegType_Cert = Convert.ToInt32(enmCandidateUpdateRequestFields.RegistrationType);
            //Int32 FieldID_RegType = context.candidateUpdateRequest.Where(s => s.candID == entityID && s.updateFieldID == Upd_RegType_Cert &&
            //                        s.requestStatusID == App_Status_RequestReceived && s.requestFinalised == true).Select(s=> s.updateFieldID).SingleOrDefault();
            //if (FieldID_RegType != 0)
            //{ trRegType.Visible = true; }

            //var CandiRegType = (from s in context.candidateUpdateRequest
            //                    orderby (s.updateFieldID)
            //                    where s.updateFieldID == Upd_RegType_Cert 
            //                       && s.requestStatusID == App_Status_RequestReceived
            //                      && s.candID == entityID &&  s.requestFinalised == true && s.requestNo == Request_Number
            //                    select new { ValueField = s.changedValue, TextField = s.enterdate }).FirstOrDefault();

            //string complete_add = CandiRegType.ValueField;
            //string[] valuesList = complete_add.Split(';');
           

            //if (valuesList[0] == "Direct to Institute")
            //{ trRegType.Visible = false;
            //    Int32 statusID = 0; //add by me 02/02/23
            //    statusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);
            //    string path = "NA";
            //    context.Database.ExecuteSqlCommand(" Update candUpdateRequest set requestStatusID = " + statusID + ",FileUploadedPath = " + path + " Where regnNo = " + Request_Number);
            //    context.SaveChanges();
            //}
            //else { trRegType.Visible = true; }

           
        }
    }
    #endregion
    protected void UploadRequireDocAll() //Trying to validate wihout docs data could not be Final Submit 
    {
        if (trHandicapped.Visible == true)
        {
            if (showHandicapped.Text == "")
            {
                ShowAlert("Please Upload All Require Documents");
                //showHandicapped.Text = "Upload File";
            }
            
        //    if (fileHandicapped.FileName == "")
        //    { ShowAlert("Please Upload All Require Documents"); }
        }
        if (trMarital.Visible == true) 
        {
            if (fileMarital.FileName == "") 
            { ShowAlert("Please Upload All Require Documents"); }
        }
    }
    #region[After_Doc_Upload_final_submit_Generate_Demand_Note]
    protected void btnSave_Click(object sender, EventArgs e) //After Doc Upload final submit Button
    {
       
       // ScriptManager.RegisterStartupScript(this, this.GetType(), "redirect", "var r = confirm('After Final Submission, Changes/Edit Request are not allowed.'); if (r == true) var str= '../CAND/CandidateUpdateRequestPreview.aspx'; location.href = '../CAND/CandidateUpdateRequestDocUpload.aspx';", true);
        try
        {
            // UploadRequireDocAll();
         
            

           //if (trHandicapped.Visible == true || trMarital.Visible == true || trCasteCerti.Visible == true || trMobile.Visible == true || trMobile.Visible == true || trCorAddress.Visible == true || trHighEducation.Visible == true)
           // {
           // if (trHandicapped.Visible == true)
           // {
           //     if (showHandicapped.Text == "")
           //     {
           //         ShowAlert("Please Upload All Require Documents");
           //         //showHandicapped.Text = "Upload File";
           //     }
           // }
          
           //      if (trMarital.Visible == true)
           //     {
           //           if (showMarital.Text == "")
           //     {
           //         ShowAlert("Please Upload All Require Documents");
           //         //showHandicapped.Text = "Upload File";
           //     }
           // }
           //if (trCasteCerti.Visible == true)
           // {
           //     if (showcaste.Text == "")
           //     {
           //         ShowAlert("Please Upload All Require Documents");
           //         //showHandicapped.Text = "Upload File";
           //     }
           // }

           //   if (trMobile.Visible == true)
           // {
           //     if (showMobile.Text == "")
           //     {
           //         ShowAlert("Please Upload All Require Documents");
           //         //showHandicapped.Text = "Upload File";
           //     }
           // }
           //   if (trEmail.Visible == true)
           // {
           //     if (showEmail.Text == "")
           //     {
           //         ShowAlert("Please Upload All Require Documents");
           //         //showHandicapped.Text = "Upload File";
           //     }
           // }
           //   if (trCorAddress.Visible == true)
           // {
           //     if (showAddress.Text == "")
           //     {
           //         ShowAlert("Please Upload All Require Documents");
           //         //showHandicapped.Text = "Upload File";
           //     }
           // }
           //  if (trHighEducation.Visible == true)
           // {
           //     if (showHighEdu.Text == "")
           //     {
           //         ShowAlert("Please Upload All Require Documents");
           //         //showHandicapped.Text = "Upload File";
           //     }
           // }
           // }

           
                //if (trHandicapped.Visible == true || trMarital.Visible == true || trCasteCerti.Visible == true || trMobile.Visible == true || trMobile.Visible == true || trCorAddress.Visible == true || trHighEducation.Visible == true)
                //{
                    //if (showHandicapped.Text != "" || showMarital.Text != "" || showcaste.Text != "" || showMobile.Text != "" || showEmail.Text != "" || showAddress.Text != "" || showHighEdu.Text != "")
                    //{


                        Int64 candidateID = Convert.ToInt64(Request.QueryString["CandidateID"]); //37068; 
                        Int64 RegApplID = Convert.ToInt64(Request.QueryString["RegApplID"]);
                        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

                        Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);
                        Int32 App_Status_DocumentsUploaded = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);
                        Int32 App_Status_submittedbutfeesnotpaid = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);
                        Int32 App_Status_Formfinalsubmittedforfreecases = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedforfreecases);


                        var Display_All_Doc = context1.candidateUpdateRequest.Where(x => x.candID == candidateID && x.requestNo == Request_Number && x.requestStatusID == App_Status_RequestReceived && x.requestFinalised == true).ToList();
                        if(Display_All_Doc.Count == 0)
                        {
                            Int64 Reqno = Request_Number;//Convert.ToInt64(Request.QueryString["Request_Number"]);

                            int courseid = 0; Int64 demandID = 0;

                            using (TransactionScope scope = new TransactionScope())
                            {
                                using (EConnectContext context = new EConnectContext())
                                {

                                    #region[old]
                                    //(Working) var candFinalSubmit = context.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID  && c.requestStatusID == 2 && c.requestNo == Reqno);                
                                    //(working)var candFinalSubmit = context.candidateUpdateRequest.Find(Reqno);
                                    // var candFinalSubmit = context.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID && c.requestStatusID == 2 && c.requestNo == Reqno);

                                    //Int64 feeAmount = GetFeeAmount(candFinalSubmit.requestNo, candFinalSubmit.courseID);
                                    //Int64 RequestNo = Convert.ToInt64(Request.QueryString["Request_Number"]);
                                    // if (candFinalSubmit.requestStatusID == 2)
                                    // { candFinalSubmit.feeTypeID = Convert.ToInt32(enmFeeType.UpdationinCandidateData); }

                                    // else
                                    // {
                                    //     //Int64 RequestNo = Convert.ToInt64(Request.QueryString["Request_Number"]);
                                    //     var FeeType = (from a in context.candidateUpdateRequest
                                    //                    where a.requestNo == RequestNo
                                    //                    select new
                                    //                    {
                                    //                        feetype = a.feeTypeID

                                    //                    }).FirstOrDefault();

                                    //     candFinalSubmit.feeTypeID = Convert.ToInt32(FeeType.feetype);
                                    // }
                                    #endregion


                                var items = context.candidateUpdateRequest.Where(c => c.candID == entityID &&
                                            c.requestStatusID == App_Status_DocumentsUploaded && c.requestNo == Reqno && c.requestFinalised == true).ToList();

                        //var items1 = context.candidateUpdateRequest.Join(context.UpdateRegnMasters, c => c.updateFieldID, u => u.ID, (c, u) => new { c, u }).Where(c => c.candID == entityID &&
                        //            c.requestStatusID == App_Status_DocumentsUploaded && c.requestNo == Reqno && c.requestFinalised == true).ToList();

                        //Int32 Upd_RegType_Cert = Convert.ToInt32(enmCandidateUpdateRequestFields.RegistrationType);
                        //if (Upd_RegType_Cert == 18)//check witdrawlcase and not witdrawlcase
                        //{
                        //    var PayCases_RegTyp = (from c in context.candidateUpdateRequest
                        //                             join u in context.UpdateRegnMasters on c.updateFieldID equals u.ID
                        //                             join R in context.RegistrationDetails on c.candID equals R.CandidateID
                        //                             join i in context.Institutes on R.InstituteID equals i.ID
                        //                             join Ins in context.AccreditationDetails on i.ID equals Ins.InstituteID
                        //                             where c.candID == entityID && c.requestStatusID == App_Status_DocumentsUploaded &&
                        //                             c.requestNo == Reqno && c.requestFinalised == true && u.isFeeApplicable == "Y" &&
                        //                             Ins.AccreditationStatusID != 5
                        //                             select c
                        //                          //select new { Status = c.requestStatusID, fiedid = c.updateFieldID }
                        //                          ).ToList();
                        //}
                        //else {
                        //    var NoPay_witdrawl_Cases = (from c in context.candidateUpdateRequest
                        //                             join u in context.UpdateRegnMasters on c.updateFieldID equals u.ID
                        //                             join R in context.RegistrationDetails on c.candID equals R.CandidateID
                        //                             join i in context.Institutes on R.InstituteID equals i.ID
                        //                             join Ins in context.AccreditationDetails on i.ID equals Ins.InstituteID
                        //                             where c.candID == entityID && c.requestStatusID == App_Status_DocumentsUploaded &&
                        //                             c.requestNo == Reqno && c.requestFinalised == true && u.isFeeApplicable == "Y" &&
                        //                             Ins.AccreditationStatusID == 5
                        //                             select c
                        //                          //select new { Status = c.requestStatusID, fiedid = c.updateFieldID }
                        //                          ).ToList();

                        //}
                       
                            var PayCases = (from c in context.candidateUpdateRequest
                                            join u in context.UpdateRegnMasters on c.updateFieldID equals u.ID
                                            where c.candID == entityID && c.requestStatusID == App_Status_DocumentsUploaded &&
                                            c.requestNo == Reqno && c.requestFinalised == true && u.isFeeApplicable == "Y"
                                            select c
                                                  //select new { Status = c.requestStatusID, fiedid = c.updateFieldID }
                                                  ).ToList();
                   

                                    var FreeCases = (from c in context.candidateUpdateRequest
                                                     join u in context.UpdateRegnMasters on c.updateFieldID equals u.ID
                                                     where c.candID == entityID && c.requestStatusID == App_Status_DocumentsUploaded &&
                                                     c.requestNo == Reqno && c.requestFinalised == true && u.isFeeApplicable == "N"
                                                     select c
                                                     //select new { Status = c.requestStatusID, fiedid = c.updateFieldID }
                                               ).ToList(); 

                                    if (FreeCases.Count > 0) //no fee cases
                                        { 
                                        foreach (var cases in FreeCases)
                                            {
                                            candUpdateRequest can = new candUpdateRequest();
                                            cases.requestStatusID  = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedforfreecases);
                                            cases.feeAmount = 0;                                         
                                            //can.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedforfreecases);
                                            //can.feeAmount = 0; //not applicable to pay
                                            context.SaveChanges();

                                            }
                                        context.SaveChanges();
                                        scope.Complete();

                                        Int32 regStatusId = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedforfreecases);// 12;
                                        if (regStatusId == 12)
                                            {
                                            // Convert.ToInt32(items.requestStatusID);
                                            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestPreview.aspx?candidateID=" + candidateID + "&RegApplID=" + RegApplID + "&regStatusId=" + regStatusId + "&Request_Number=" + Request_Number));

                                            }
                                        }

                                    else
                                        {


                                        foreach (var item in PayCases) //loop logic 4 handling (demand note genrate only one on the basis of muliple request by one candidate)
                                            {
                                            //if (candFinalSubmit.feeTypeID == 0)
                                            //    candFinalSubmit.feeTypeID = 24;

                                            Int64 feeAmount = GetFeeAmount(item.requestNo, item.courseID);//FeeAmount for each request
                                            Int64 feeAmtTot = GetFeeTotalAmount(item.requestNo, item.courseID); //TotalAmout for all request
                                            item.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);//3-Form final submitted but fees not paid
                                            courseid = item.courseID;
                                            item.feeAmount = feeAmount; //feeAmtTot;
                                            item.paymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                                            item.feeTypeID = Convert.ToInt32(enmFeeType.UpdationinCandidateData);
                                            //item.demandNoteID = demandID; 
                                            context.SaveChanges();

                                            //Logic create to check total amount is 0 then fee is not applicable
                                            // if (feeAmtTot == 0)
                                            //End Logic
                                            // {


                                            if (!item.demandNoteID.HasValue && item.requestStatusID == App_Status_submittedbutfeesnotpaid)
                                                {
                                                UpdateRequestDemandNote demand = new UpdateRequestDemandNote();

                                                //Int64 feeAmtTot = GetFeeTotalAmount(item.requestNo, item.courseID); //TotalAmout for all request
                                                // if (item.feeTypeID == Convert.ToInt32(enmFeeType.UpdationinCandidateData))
                                                var DemandExist = context.UpdateRequestDemandNotes.Find(demandID);
                                                if (DemandExist == null)//checkind Demand Note is exists for request
                                                    {
                                                    demand.UpdateRequestDate = DateTime.Now;
                                                    demand.FeeTypeID = item.feeTypeID.Value;
                                                    demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.Online);
                                                    demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Single);
                                                    demand.Amount = feeAmtTot;//item.feeAmount.Value;//feeAmtTot;//item.feeAmount.Value;
                                                    demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                                                    demand.CreatedBy = Convert.ToInt32(entityID);//candFinalSubmit.enterBy.Value;                             
                                                    demand.CourseCategoryID = 1;
                                                    demand.CourseID = item.courseID;
                                                    demand.ServiceID = item.Course.UpdationServiceID;
                                                    context.UpdateRequestDemandNotes.Add(demand);
                                                    context.SaveChanges();

                                                    item.demandNoteID = demand.ID;
                                                    demandID = demand.ID;
                                                    }
                                                else { item.demandNoteID = demandID; }
                                                }
                                            }

                                        context.SaveChanges();
                                        scope.Complete();

                                        Int32 regStatusId = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedbutfeesnotpaid);// 3;
                                        Int32 Upd_RegType_Cert1 = Convert.ToInt32(enmCandidateUpdateRequestFields.RegistrationType);

                               //Int32 FieldID_RegType = context.candidateUpdateRequest.Where(s => s.candID == entityID && s.updateFieldID == Upd_RegType_Cert &&
                               //s.requestStatusID == App_Status_RequestReceived && s.requestFinalised == true && s.requestNo == Request_Number).Select(s=> s.updateFieldID).SingleOrDefault();
                            if (regStatusId == 3 || Upd_RegType_Cert1 == 18)
                            {
                                // Convert.ToInt32(items.requestStatusID);
                                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestPreviewBeforeExam.aspx?candidateID=" + candidateID + "&RegApplID=" + RegApplID + "&regStatusId=" + regStatusId + "&Request_Number=" + Request_Number + "&demandID=" + demandID));

                            }
                            else
                            {
                                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestPreviewBeforeExam.aspx?candidateID=" + candidateID + "&RegApplID=" + RegApplID + "&regStatusId=" + regStatusId + "&Request_Number=" + Request_Number + "&demandID=" + demandID));
                            }
                          }


                                    //context.SaveChanges();
                                    //scope.Complete();
                                    //Int32 regStatusId = Convert.ToInt32(enmCandidateUpdateRequestStatus.Formfinalsubmittedforfreecases);// 12;
                                    //if (regStatusId == 12)
                                    //    {
                                    //    // Convert.ToInt32(items.requestStatusID);
                                    //    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestPreview.aspx?candidateID=" + candidateID + "&RegApplID=" + RegApplID + "&regStatusId=" + regStatusId + "&Request_Number=" + Request_Number));

                                    //    }
                                    // else
                                    //  { 
                                    //    //    // Convert.ToInt32(items.requestStatusID);
                                    //        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestPreview.aspx?candidateID=" + candidateID + "&RegApplID=" + RegApplID + "&regStatusId=" + regStatusId + "&Request_Number=" + Request_Number + "&demandID=" + demandID));
                                    //   }
                                };
                            };
                        }
                        else { ShowAlert("Upload all supportive Documents"); }
                    //}
               // }
           
        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
       
         
           #region(FinalSumit-old)
        ////logic of final submit update status of restatusId=3      
        //    using (var db = new EConnectContext())
        //    {
        //        var objCandUploadDoc1 = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID  & c.requestStatusID == 2);
        //        if (objCandUploadDoc1 != null)
        //        {
                   
        //            objCandUploadDoc1.requestStatusID = 3;//finalsubmit but fee not paid.
        //            objCandUploadDoc1.feeTypeID = Convert.ToInt32(enmFeeType.UpdationinCandidateData);//24;
        //            objCandUploadDoc1.feeAmount = GetFeeAmount(objCandUploadDoc1.requestNo, objCandUploadDoc1.courseID);
                  
        //            db.SaveChanges();
        //            Int32 regStatusId = Convert.ToInt32(objCandUploadDoc1.requestStatusID);
        //            if (regStatusId == 3)
        //            {
        //                long CandidateID = entityID;
        //                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestPreview.aspx?candidateID=" + CandidateID + "&RegApplID=" + RegApplID + "&regStatusId=" + regStatusId));
        //            }
        //        }
        //        else { ShowAlert("Please Upload Rquest Documments"); }

        //    }
           #endregion
      

    }
    #endregion

    protected void btnback_Click(object sender, EventArgs e)
    {
        //long CandidateID = entityID;
        //if (!String.IsNullOrEmpty(Request.QueryString["id"]))
        //{
        //    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestPreview.aspx?candidateID=" + CandidateID + "&RegApplID=" + RegApplID));
        //}
        //else
        //{
        //    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/CandidateUpdateRequestPreview.aspx?candidateID=" + CandidateID + "&RegApplID=" + RegApplID));
        //}
    }

    #region[Validation_Upload_Docs]
    protected bool IsValidForm()
    {
        try
        {
            if (fileHandicapped.HasFile && fileHandicapped.FileName.Length > 50)
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (fileHandicapped.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Handicapped Certificate file name can not be left blank";
                return false;
            }
            String fileExtension = System.IO.Path.GetExtension(fileHandicapped.FileName).ToLower();
            if (fileExtension != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Invalid Handicapped Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(fileHandicapped, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Handicapped Certificate File size should be of 100 KB or less.";
                return false;
            }

            if (fileMarital.HasFile && fileMarital.FileName.Length > 50)
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (fileMarital.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Marriage Certificate File name can not be left blank";
                return false;
            }
            String fileExtension1 = System.IO.Path.GetExtension(fileMarital.FileName).ToLower();
            if (fileExtension1 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Invalid Marriage Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(fileMarital, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Marriage Certificate File size should be of 100 KB or less.";
                return false;
            }

            if (FileCaste.HasFile && FileCaste.FileName.Length > 50)
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FileCaste.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Caste Certificate File name can not be left blank";
                return false;
            }
            String fileExtension2 = System.IO.Path.GetExtension(FileCaste.FileName).ToLower();
            if (fileExtension2 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Invalid Caste Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileCaste, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Caste Certificate File size should be of 100 KB or less.";
                return false;
            }
            if (FileMobile.HasFile && FileMobile.FileName.Length > 50)
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FileMobile.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Mobile File name can not be left blank";
                return false;
            }
            String fileExtension3 = System.IO.Path.GetExtension(FileMobile.FileName).ToLower();
            if (fileExtension3 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Invalid Mob Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileMobile, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Mobile Certificate File size should be of 100 KB or less.";
                return false;
            }
            if (FileEmail.HasFile && FileEmail.FileName.Length > 50)
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FileEmail.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Email File name can not be left blank";
                return false;
            }
            String fileExtension4 = System.IO.Path.GetExtension(FileEmail.FileName).ToLower();
            if (fileExtension4 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Invalid Email Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileEmail, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Email Certificate File size should be of 100 KB or less.";
                return false;
            }
            if (FileCorAddress.HasFile && FileCorAddress.FileName.Length > 50)
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FileCorAddress.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Address File name can not be left blank";
                return false;
            }
            String fileExtension5 = System.IO.Path.GetExtension(FileCorAddress.FileName).ToLower();
            if (fileExtension5 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Invalid Address Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileCorAddress, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Address Certificate File size should be of 100 KB or less.";
                return false;
            }
            if (FileHighEducation.HasFile && FileHighEducation.FileName.Length > 50)
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FileHighEducation.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "High Qualification File name can not be left blank";
                return false;
            }
            String fileExtension6 = System.IO.Path.GetExtension(FileHighEducation.FileName).ToLower();
            if (fileExtension6 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Invalid High Qualification Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileHighEducation, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "High Qualification Certificate File size should be of 100 KB or less.";
                return false;
            }
            /////////////////////////////
            if (FileRegTypeCert.HasFile && FileRegTypeCert.FileName.Length > 50) 
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FileRegTypeCert.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "NOC File name can not be left blank";
                return false;
            }
            String fileExtension7 = System.IO.Path.GetExtension(FileRegTypeCert.FileName).ToLower();
            if (fileExtension7 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Invalid NOC Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileRegTypeCert, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "NOC Certificate File size should be of 100 KB or less.";
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool IsValidHandicapped()
    {
        try
        {
            if (fileHandicapped.HasFile && fileHandicapped.FileName.Length > 50)
            {

                //throw new Exception("file name should be less than 50 characters.");
                Lblerror.Text = "file name should be less than 50 characters.";
            }
            if (fileHandicapped.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Handicapped Certificate file name can not be left blank";
                return false;
            }
            String fileExtension = System.IO.Path.GetExtension(fileHandicapped.FileName).ToLower();
            if (fileExtension != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Invalid Handicapped Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(fileHandicapped, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Handicapped Certificate File size should be of 100 KB or less.";
                return false;
            }


            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool IsValidMarital()
    {
        try
        {
            if (fileMarital.HasFile && fileMarital.FileName.Length > 50)
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (fileMarital.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Marriage Certificate File name can not be left blank";
                return false;
            }
            String fileExtension1 = System.IO.Path.GetExtension(fileMarital.FileName).ToLower();
            if (fileExtension1 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Invalid Marriage Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(fileMarital, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Marriage Certificate File size should be of 100 KB or less.";
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool IsValidCaste()
    {
        try
        {
            if (FileCaste.HasFile && FileCaste.FileName.Length > 50)
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FileCaste.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Caste Certificate File name can not be left blank";
                return false;
            }
            String fileExtension2 = System.IO.Path.GetExtension(FileCaste.FileName).ToLower();
            if (fileExtension2 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Invalid Caste Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileCaste, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Caste Certificate File size should be of 100 KB or less.";
                return false;
            }
            return true;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool IsValidMobile()
    {
        try
        {
            if (FileMobile.HasFile && FileMobile.FileName.Length > 50) 
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FileMobile.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Mobile File name can not be left blank";
                return false;
            }
            String fileExtension2 = System.IO.Path.GetExtension(FileMobile.FileName).ToLower();
            if (fileExtension2 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Invalid Mob Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileMobile, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Mobile Certificate File size should be of 100 KB or less.";
                return false;
            }
            return true;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool IsValidEmail() 
    {
        try
        {
            if (FileEmail.HasFile && FileEmail.FileName.Length > 50)
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FileEmail.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Email File name can not be left blank";
                return false;
            }
            String fileExtension2 = System.IO.Path.GetExtension(FileEmail.FileName).ToLower();
            if (fileExtension2 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Invalid Email Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileEmail, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Email Certificate File size should be of 100 KB or less.";
                return false;
            }
            return true;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool IsValidCorAddress()
    {
        try
        {
            if (FileCorAddress.HasFile && FileCorAddress.FileName.Length > 50)
            {
                Lblerror.Visible = true;
                Lblerror.Text = "file name should be less than 50 characters.";
                return false;
                //throw new Exception("file name should be less than 50 characters.");
            }
            if (FileCorAddress.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Address File name can not be left blank";
                return false;
            }
            String fileExtension2 = System.IO.Path.GetExtension(FileCorAddress.FileName).ToLower();
            if (fileExtension2 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Invalid Address Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileCorAddress, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Address Certificate File size should be of 100 KB or less.";
                return false;
            }
            return true;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool IsValidHighEducation()
    {
        try
        {
            if (FileHighEducation.HasFile && FileHighEducation.FileName.Length > 50)
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FileHighEducation.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "High Qualification File name can not be left blank";
                return false;
            }
            String fileExtension2 = System.IO.Path.GetExtension(FileHighEducation.FileName).ToLower();
            if (fileExtension2 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Invalid High Qualification Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileHighEducation, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "High Qualification Certificate File size should be of 100 KB or less.";
                return false;
            }
            return true;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool IsValidCnameCert()
    {
        try
        {
            if (FileCnameCert.HasFile && FileCnameCert.FileName.Length > 50) 
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FileCnameCert.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Candidate File name can not be left blank";
                return false;
            }
            String fileExtension2 = System.IO.Path.GetExtension(FileCnameCert.FileName).ToLower();
            if (fileExtension2 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Invalid  file,Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileCnameCert, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Candidate Self-Attested copy of File size should be of 100 KB or less.";
                return false;
            }
            return true;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool IsValidFatherCert()
    {
        try
        {
            if (FileFatherCert.HasFile && FileFatherCert.FileName.Length > 50) 
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FileFatherCert.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "File name can not be left blank";
                return false;
            }
            String fileExtension2 = System.IO.Path.GetExtension(FileFatherCert.FileName).ToLower();
            if (fileExtension2 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileFatherCert, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "File size should be of 100 KB or less.";
                return false;
            }
            return true;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool IsValidMotherCert()
    {
        try
        {
            if (FileMotherCert.HasFile && FileMotherCert.FileName.Length > 50) 
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FileMotherCert.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "File name can not be left blank";
                return false;
            }
            String fileExtension2 = System.IO.Path.GetExtension(FileMotherCert.FileName).ToLower();
            if (fileExtension2 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileMotherCert, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "File size should be of 100 KB or less.";
                return false;
            }
            return true;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool IsValidDobCert()
    {
        try
        {
            if (FileDobCert.HasFile && FileDobCert.FileName.Length > 50) 
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FileDobCert.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "File name can not be left blank";
                return false;
            }
            String fileExtension2 = System.IO.Path.GetExtension(FileDobCert.FileName).ToLower();
            if (fileExtension2 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileDobCert, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "File size should be of 100 KB or less.";
                return false;
            }
            return true;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool IsValidGenderCert()
    {
        try
        {
            if (FileGenderCert.HasFile && FileGenderCert.FileName.Length > 50)
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FileGenderCert.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "File name can not be left blank";
                return false;
            }
            String fileExtension2 = System.IO.Path.GetExtension(FileGenderCert.FileName).ToLower();
            if (fileExtension2 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileGenderCert, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "File size should be of 100 KB or less.";
                return false;
            }
            return true;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool IsValidPerAddress()
    {
        try
        {
            if (FilePerAddressCert.HasFile && FilePerAddressCert.FileName.Length > 50) 
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FilePerAddressCert.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "File name can not be left blank";
                return false;
            }
            String fileExtension2 = System.IO.Path.GetExtension(FilePerAddressCert.FileName).ToLower();
            if (fileExtension2 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "Certificate file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FilePerAddressCert, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "File size should be of 100 KB or less.";
                return false;
            }
            return true;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool IsValidRegType()
    {
        try
        {
            if (FileRegTypeCert.HasFile && FileRegTypeCert.FileName.Length > 50) 
            {

                throw new Exception("file name should be less than 50 characters.");
            }
            if (FileRegTypeCert.FileName.ToString() == "")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "File name can not be left blank";
                return false;
            }
            String fileExtension2 = System.IO.Path.GetExtension(FileRegTypeCert.FileName).ToLower();
            if (fileExtension2 != ".pdf")
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "NOC file .Only pdf extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(FileRegTypeCert, 102400))
            {
                Lblerror.Visible = true;
                //GenerateNewCaptchaImage();
                //txtcode.Text = "";
                Lblerror.Text = "File size should be of 100 KB or less.";
                return false;
            }
            return true;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
    protected void UploadDocuments()
    {
        using (EConnectContext context = new EConnectContext())
        {
            candUpdateRequest objCandUploadDoc = context.candidateUpdateRequest.Find(Convert.ToInt32(Request.QueryString["candidateID"]));

            using (var db = new EConnectContext())
            {
                try
                {
                    #region[Upload Handicapped Certificate]
                    var objCandUploadDoc1 = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.updateFieldID == 1);

                    if (objCandUploadDoc1 != null)
                    {

                        string fileName = System.IO.Path.GetFileName(fileHandicapped.PostedFile.FileName);
                        //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                        string filePath = Server.MapPath(@"" + "~/Candidate_Update_Req_Docs/" + entityID + "/");
                        if (!Directory.Exists(filePath))
                        {
                            //If Directory (Folder) does not exists. Create it.
                            Directory.CreateDirectory(filePath);
                        }
                        string filePathadd = Server.MapPath(@"" + "~/Candidate_Update_Req_Docs/" + entityID + "/" + fileName);
                        // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                        fileHandicapped.SaveAs(filePath + fileName);

                        objCandUploadDoc1.FileUploadedPath = filePathadd;

                        objCandUploadDoc1.docsVerified = false;
                        db.SaveChanges();
                        lblHandicappedFile.Enabled = true;
                        lblHandicappedFile.Text = fileName;
                        fileHandicapped.Enabled = false;
                    }
                    else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                    #endregion

                    #region[Upload Marital Certificate]
                    var objCandUploadDoc_Matrital = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.updateFieldID == 2);

                    if (objCandUploadDoc_Matrital != null)
                    {

                        string fileName = System.IO.Path.GetFileName(fileMarital.PostedFile.FileName);
                        //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                        string filePath = Server.MapPath(@"" + "~/Candidate_Update_Req_Docs/" + entityID + "/");
                        if (!Directory.Exists(filePath))
                        {
                            //If Directory (Folder) does not exists. Create it.
                            Directory.CreateDirectory(filePath);
                        }
                        string filePathadd = Server.MapPath(@"" + "~/Candidate_Update_Req_Docs/" + entityID + "/" + fileName);
                        // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                        fileMarital.SaveAs(filePath + fileName);

                        objCandUploadDoc_Matrital.FileUploadedPath = filePathadd;

                        objCandUploadDoc_Matrital.docsVerified = false;
                        db.SaveChanges();
                        lblHandicappedFile.Enabled = true;
                        lblHandicappedFile.Text = fileName;
                        fileMarital.Enabled = false;
                    }
                    else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                    #endregion

                    #region[Upload Caste Certificate]
                    var objCandUploadDoc_Caste = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.updateFieldID == 3);

                    if (objCandUploadDoc_Caste != null)
                    {

                        string fileName = System.IO.Path.GetFileName(FileCaste.PostedFile.FileName);
                        //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                        string filePath = Server.MapPath(@"" + "~/Candidate_Update_Req_Docs/" + entityID + "/");
                        if (!Directory.Exists(filePath))
                        {
                            //If Directory (Folder) does not exists. Create it.
                            Directory.CreateDirectory(filePath);
                        }
                        string filePathadd = Server.MapPath(@"" + "~/Candidate_Update_Req_Docs/" + entityID + "/" + fileName);
                        // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                        FileCaste.SaveAs(filePath + fileName);

                        objCandUploadDoc_Caste.FileUploadedPath = filePathadd;

                        objCandUploadDoc_Caste.docsVerified = false;
                        db.SaveChanges();
                        lblCasteCertFile.Enabled = true;
                        lblCasteCertFile.Text = fileName;
                        FileCaste.Enabled = false;
                    }
                    else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                    #endregion
                }
                catch (Exception ex) { ShowAlert(ex.Message); }
            }
            ShowAlert("Upload Successfully");
        }

    }

    #region[Upload & Save_All_Documents]
    protected void UploadDisabiliy_Click(object sender, EventArgs e)
    {
        if (IsValidHandicapped())
        {
            if (fileHandicapped.HasFile)
            {
                using (var db = new EConnectContext())
                {
                    try
                    {
                        lblHandicappedFile.Text = "";
                        Lblerror.Text = "";
                        #region[Upload Handicapped Certificate]
                        Int32 Upd_Field_Handi = Convert.ToInt32(enmCandidateUpdateRequestFields.Handicapped);  
                        Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);

                        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

                        var objCandUploadDoc1 = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.requestNo == Request_Number &
                                                c.updateFieldID == Upd_Field_Handi &  c.requestFinalised == true  &
                                                c.requestStatusID == App_Status_RequestReceived);

                        if (objCandUploadDoc1 != null)
                        {

                            string fileName = "Handicapped_"+(System.IO.Path.GetFileName(fileHandicapped.PostedFile.FileName));
                            //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                            string filePath = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/");
                            if (!Directory.Exists(filePath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(filePath);
                            }
                            string filePathadd = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/" + fileName);
                            // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                            fileHandicapped.SaveAs(filePath + fileName);

                            objCandUploadDoc1.FileUploadedPath = filePathadd;
                            objCandUploadDoc1.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);//2;//(DocsUpload)
                            objCandUploadDoc1.paymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending); //(1.pending Fee)
                            objCandUploadDoc1.docsVerified = false;
                            db.SaveChanges();
                            lblHandicappedFile.Enabled = true;
                            lblHandicappedFile.Text = fileName;
                            showHandicapped.Text = "Upload Successfully";
                            //fileHandicapped.Enabled = false;
                            //UploadDisabiliy.Visible = false;
                            ShowAlert("Upload Successfully");


                        }
                        else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        Lblerror.Text = ex.Message;
                        Lblerror.Visible = true;
                        //txtAppName.Focus();
                        ShowAlert(ex.Message);
                    }
                }

            }
            else
            {
                Lblerror.Text = "Handicapped Certificate can not be left blank.";
                Lblerror.Visible = true;
                lblHandicappedFile.Visible = true;
                lblHandicappedFile.Text = "Handicapped Certificate can not be left blank.";
                // txtAppName.Focus();
               // ShowAlert("Handicapped Certificate can not be left blank.");
            }
        }
    }
    protected void UploadMaritalCert_Click(object sender, EventArgs e)
    {
        if (IsValidMarital())
        {
            if (fileMarital.HasFile)
            {
                using (var db = new EConnectContext())
                {
                    try
                    {
                        lblMaritalFile.Text = ""; 
                        Lblerror.Text = "";
                        #region[Upload Marital Certificate]
                        Int32 Upd_Field_Marital = Convert.ToInt32(enmCandidateUpdateRequestFields.MaritalStatus);
                        Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);

                        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);


                        var objCandUploadDoc_Marital = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.requestNo == Request_Number &
                                                        c.updateFieldID == Upd_Field_Marital & c.requestFinalised == true &
                                                        c.requestStatusID == App_Status_RequestReceived);

                        if (objCandUploadDoc_Marital != null)
                        {

                            string fileName = "Marital_" + (System.IO.Path.GetFileName(fileMarital.PostedFile.FileName));
                            //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                            string filePath = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/");
                            if (!Directory.Exists(filePath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(filePath);
                            }
                            string filePathadd = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/" +  fileName);
                            // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                            fileMarital.SaveAs(filePath + fileName);

                            objCandUploadDoc_Marital.FileUploadedPath = filePathadd;
                            objCandUploadDoc_Marital.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);//2;//(DocsUpload)
                            objCandUploadDoc_Marital.paymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);//1; //(pending Fee)
                            objCandUploadDoc_Marital.docsVerified = false;
                            db.SaveChanges();                          

                            lblMaritalFile.Enabled = true;
                            lblMaritalFile.Text = fileName;
                            showMarital.Text = "Upload Successfully";
                            //fileMarital.Enabled = false;
                            //UploadMaritalCert.Visible = false;                           
                            ShowAlert("Upload Successfully");

                        }
                        else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Lblerror.Text = ex.Message;
                        Lblerror.Visible = true;
                        //txtAppName.Focus();
                        ShowAlert(ex.Message);
                    }
                }

            }
            else
            {
                Lblerror.Text = "Marital Certificate can not be left blank.";
                Lblerror.Visible = true;
                lblMaritalFile.Visible = true;
                lblMaritalFile.Text = "Marital Certificate can not be left blank.";
                // txtAppName.Focus();
                ShowAlert("Marital Certificate can not be left blank.");
            }
        }
    }
    protected void UploadCasteCerti_Click(object sender, EventArgs e)
    {
        if (IsValidCaste())
        {
            if (FileCaste.HasFile)
            {

                using (var db = new EConnectContext())
                {
                    try
                    {
                        lblCasteCertFile.Text = ""; 
                        Lblerror.Text = "";
                        #region[Upload Caste Certificate]
                        Int32 Upd_Field_CasteCategory = Convert.ToInt32(enmCandidateUpdateRequestFields.CasteCategory);
                        Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);

                        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);


                        var objCandUploadDoc_Caste = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.requestNo == Request_Number &
                                                     c.updateFieldID == Upd_Field_CasteCategory & c.requestFinalised == true &
                                                     c.requestStatusID == App_Status_RequestReceived);

                        if (objCandUploadDoc_Caste != null)
                        {

                            string fileName = "Caste_" + (System.IO.Path.GetFileName(FileCaste.PostedFile.FileName));
                            //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                            string filePath = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/");
                            if (!Directory.Exists(filePath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(filePath);
                            }
                            string filePathadd = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/" +  fileName);
                            // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                            FileCaste.SaveAs(filePath + fileName);

                            objCandUploadDoc_Caste.FileUploadedPath = filePathadd;
                            objCandUploadDoc_Caste.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);//2; //(DocUpload)
                            objCandUploadDoc_Caste.paymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending); //1; //(pending Fee)
                            objCandUploadDoc_Caste.docsVerified = false;
                            db.SaveChanges();

                            lblCasteCertFile.Enabled = true;
                            lblCasteCertFile.Text = fileName;
                            showcaste.Text = "Upload Successfully";
                            //FileCaste.Enabled = false;                            
                            //UploadCasteCerti.Visible = false; 
                            ShowAlert("Upload Successfully"); 
                            
                        }
                        else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Lblerror.Text = ex.Message;
                        Lblerror.Visible = true;
                        //txtAppName.Focus();
                        ShowAlert(ex.Message);
                    }
                }
            }
        }
    }
    protected void UploadMobile_Click(object sender, EventArgs e)
    {
        if (IsValidMobile()) 
        {
            if (FileMobile.HasFile) 
            {

                using (var db = new EConnectContext())
                {
                    try
                    {
                        lblMobileFile.Text = ""; 
                        Lblerror.Text = "";
                        #region[Upload Caste Certificate]
                        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

                        Int32 Upd_Field_MobileNumber = Convert.ToInt32(enmCandidateUpdateRequestFields.MobileNumber);
                        Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);

                        var objCandUploadDoc_Mob = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.requestNo == Request_Number &
                                                    c.updateFieldID == Upd_Field_MobileNumber & c.requestFinalised == true &
                                                    c.requestStatusID == App_Status_RequestReceived);

                        if (objCandUploadDoc_Mob != null)
                        {

                            string fileName = "Mob_" + (System.IO.Path.GetFileName(FileMobile.PostedFile.FileName));
                            //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                            string filePath = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/");
                            if (!Directory.Exists(filePath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(filePath);
                            }
                            string filePathadd = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/" +  fileName);
                            // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                            FileMobile.SaveAs(filePath + fileName);

                            objCandUploadDoc_Mob.FileUploadedPath = filePathadd;
                            objCandUploadDoc_Mob.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);// 2; //(DocUpload)
                            objCandUploadDoc_Mob.paymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending); //1; //(pending Fee)
                            objCandUploadDoc_Mob.docsVerified = false;
                            db.SaveChanges();

                            lblMobileFile.Enabled = true;
                            lblMobileFile.Text = fileName;
                            showMobile.Text = "Upload Successfully"; 
                            //FileMobile.Enabled = false;
                            //UploadMobile.Visible = false;  
                            ShowAlert("Upload Successfully");

                        }
                        else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Lblerror.Text = ex.Message;
                        Lblerror.Visible = true;
                        //txtAppName.Focus();
                        ShowAlert(ex.Message);
                    }
                }
            }
        }

    }
    protected void UploadEmail_Click(object sender, EventArgs e)
    {
        if (IsValidEmail()) 
        {
            if (FileEmail.HasFile) 
            {

                using (var db = new EConnectContext())
                {
                    try
                    {
                        lblEmailFile.Text = ""; 
                        Lblerror.Text = "";
                        #region[Upload Caste Certificate]
                        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

                        Int32 Upd_Field_EmailAddress = Convert.ToInt32(enmCandidateUpdateRequestFields.EmailAddress);
                        Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);

                        var objCandUploadDoc_Email = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.requestNo == Request_Number &
                                                      c.updateFieldID == Upd_Field_EmailAddress & c.requestFinalised == true &
                                                      c.requestStatusID == App_Status_RequestReceived);

                        if (objCandUploadDoc_Email != null)
                        {

                            string fileName = "Email_" + (System.IO.Path.GetFileName(FileEmail.PostedFile.FileName));
                            //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                            string filePath = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/");
                            if (!Directory.Exists(filePath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(filePath);
                            }
                            string filePathadd = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/" +  fileName);
                            // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                            FileEmail.SaveAs(filePath + "Email_" + fileName);

                            objCandUploadDoc_Email.FileUploadedPath = filePathadd;
                            objCandUploadDoc_Email.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded); //(DocUpload)
                            objCandUploadDoc_Email.paymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);//1; //(pending Fee)
                            objCandUploadDoc_Email.docsVerified = false;
                            db.SaveChanges();

                            lblEmailFile.Enabled = true;
                            lblEmailFile.Text = fileName;
                            showEmail.Text = "Upload Successfully"; 
                            //FileEmail.Enabled = false;                            
                            //UploadEmail.Visible = false;  
                            ShowAlert("Upload Successfully");

                        }
                        else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Lblerror.Text = ex.Message;
                        Lblerror.Visible = true;
                        //txtAppName.Focus();
                        ShowAlert(ex.Message);
                    }
                }
            }
        }

    }
    protected void UploadCorAddress_Click(object sender, EventArgs e)
    {
        if (IsValidCorAddress()) 
        {
            if (FileCorAddress.HasFile) 
            {

                using (var db = new EConnectContext())
                {
                    try
                    {
                        lblCorAddress.Text = ""; 
                        Lblerror.Text = "";
                        #region[Upload Caste Certificate]
                        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

                        Int32 Upd_Field_Address = Convert.ToInt32(enmCandidateUpdateRequestFields.CorrespondenceAddress);
                        Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);

                        var objCandUploadDoc_CorAddress = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.requestNo == Request_Number &
                                                           c.updateFieldID == Upd_Field_Address & c.requestFinalised == true &
                                                           c.requestStatusID == App_Status_RequestReceived);

                        if (objCandUploadDoc_CorAddress != null)
                        {

                            string fileName = "CorAddress_" + (System.IO.Path.GetFileName(FileCorAddress.PostedFile.FileName));
                            //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                            string filePath = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/");
                            if (!Directory.Exists(filePath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(filePath);
                            }
                            string filePathadd = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/" +  fileName);
                            // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                            FileCorAddress.SaveAs(filePath + fileName);

                            objCandUploadDoc_CorAddress.FileUploadedPath = filePathadd;
                            objCandUploadDoc_CorAddress.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded); //(DocUpload)
                            objCandUploadDoc_CorAddress.paymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending); //(pending Fee)
                            objCandUploadDoc_CorAddress.docsVerified = false;
                            db.SaveChanges();

                            lblCorAddressFile.Enabled = true;
                            lblCorAddressFile.Text = fileName;
                            showAddress.Text = "Upload Successfully"; 
                            //FileCorAddress.Enabled = false;                            
                            //UploadCorAddress.Visible = false;  
                            ShowAlert("Upload Successfully");

                        }
                        else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Lblerror.Text = ex.Message;
                        Lblerror.Visible = true;
                        //txtAppName.Focus();
                        ShowAlert(ex.Message);
                    }
                }
            }
        }
    }
    protected void UploadHighEdu_Click(object sender, EventArgs e)
    {
        if (IsValidHighEducation()) 
        {
            if (FileHighEducation.HasFile) 
            {

                using (var db = new EConnectContext())
                {
                    try
                    {
                        lblHighEducation.Text = ""; 
                        Lblerror.Text = "";
                        #region[Upload Caste Certificate]
                        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

                        Int32 Upd_Field_HighEdu = Convert.ToInt32(enmCandidateUpdateRequestFields.EducationalQualification);
                        Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);

                        var objCandUploadDoc_HighEdu = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.requestNo == Request_Number &
                                                           c.updateFieldID == Upd_Field_HighEdu & c.requestFinalised == true &
                                                           c.requestStatusID == App_Status_RequestReceived);
                           // db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.updateFieldID == 7);

                        if (objCandUploadDoc_HighEdu != null)
                        {

                            string fileName = "HighEdu_" + (System.IO.Path.GetFileName(FileHighEducation.PostedFile.FileName));
                            //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                            string filePath = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/");
                            if (!Directory.Exists(filePath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(filePath);
                            }
                            string filePathadd = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/" +  fileName);
                            // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                            FileHighEducation.SaveAs(filePath + fileName);

                            objCandUploadDoc_HighEdu.FileUploadedPath = filePathadd;
                            objCandUploadDoc_HighEdu.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded); //(DocUpload)
                            objCandUploadDoc_HighEdu.paymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending); //(Payment Panding) 
                            objCandUploadDoc_HighEdu.docsVerified = false;
                            db.SaveChanges();

                            lblHighEducationFile.Enabled = true;
                            lblHighEducationFile.Text = fileName;
                            showHighEdu.Text = "Upload Successfully"; 
                            //FileHighEducation.Enabled = false;                            
                            //UploadHighEdu.Visible = false;  
                            ShowAlert("Upload Successfully");

                        }
                        else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Lblerror.Text = ex.Message;
                        Lblerror.Visible = true;
                        //txtAppName.Focus();
                        ShowAlert(ex.Message);
                    }
                }
            }
        }
    }
    protected void UploadCnameCerti_Click(object sender, EventArgs e)
    {
        
             if (IsValidCnameCert())
        {
            if (FileCnameCert.HasFile) 
            {

                using (var db = new EConnectContext())
                {
                    try
                    {
                        lblCname.Text = ""; 
                        Lblerror.Text = "";
                        #region[Upload Candidate Certificate]
                        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

                        Int32 Upd_Field_Candi = Convert.ToInt32(enmCandidateUpdateRequestFields.Name);
                        Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);

                        var objCandUploadDoc_CandiCname = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.requestNo == Request_Number &
                                                           c.updateFieldID == Upd_Field_Candi & c.requestFinalised == true &
                                                           c.requestStatusID == App_Status_RequestReceived);
                        // db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.updateFieldID == 7);

                        if (objCandUploadDoc_CandiCname != null)
                        {

                            string fileName = "CandidateName_" + (System.IO.Path.GetFileName(FileCnameCert.PostedFile.FileName)); 
                            //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                            string filePath = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/");
                            if (!Directory.Exists(filePath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(filePath);
                            }
                            string filePathadd = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/" +  fileName);
                            // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                            FileCnameCert.SaveAs(filePath + fileName);

                            objCandUploadDoc_CandiCname.FileUploadedPath = filePathadd;
                            objCandUploadDoc_CandiCname.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded); //(DocUpload)
                            objCandUploadDoc_CandiCname.paymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending); //(Payment Panding) 
                            objCandUploadDoc_CandiCname.docsVerified = false;
                            db.SaveChanges();

                            lblCnameFile.Enabled = true;
                            lblCnameFile.Text = fileName;
                            showCnameCert.Text = "Upload Successfully"; 
                            //FileHighEducation.Enabled = false;                            
                            //UploadHighEdu.Visible = false;  
                            ShowAlert("Upload Successfully");

                        }
                        else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Lblerror.Text = ex.Message;
                        Lblerror.Visible = true;
                        //txtAppName.Focus();
                        ShowAlert(ex.Message);
                    }
                }
            }
        }
    }
    protected void UploadFatherCerti_Click(object sender, EventArgs e)
    {
        if (IsValidFatherCert()) 
        {
            if (FileFatherCert.HasFile) 
            {

                using (var db = new EConnectContext())
                {
                    try
                    {
                        lblFather.Text = ""; 
                        Lblerror.Text = "";
                        #region[Upload Father Certificate]
                        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

                        Int32 Upd_Field_FatherName = Convert.ToInt32(enmCandidateUpdateRequestFields.FatherName);
                        Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);

                        var objCandUploadDoc_FatherName = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.requestNo == Request_Number &
                                                           c.updateFieldID == Upd_Field_FatherName & c.requestFinalised == true &
                                                           c.requestStatusID == App_Status_RequestReceived);
                        // db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.updateFieldID == 7);

                        if (objCandUploadDoc_FatherName != null)
                        {

                            string fileName = "FatherName_" + (System.IO.Path.GetFileName(FileFatherCert.PostedFile.FileName)); 
                            //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                            string filePath = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/");
                            if (!Directory.Exists(filePath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(filePath);
                            }
                            string filePathadd = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/" +  fileName);
                            // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                            FileFatherCert.SaveAs(filePath + fileName);

                            objCandUploadDoc_FatherName.FileUploadedPath = filePathadd;
                            objCandUploadDoc_FatherName.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded); //(DocUpload)
                            objCandUploadDoc_FatherName.paymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending); //(Payment Panding) 
                            objCandUploadDoc_FatherName.docsVerified = false;
                            db.SaveChanges();

                            lblFatherFile.Enabled = true;
                            lblFatherFile.Text = fileName;
                            showFatherCert.Text = "Upload Successfully"; 
                            //FileHighEducation.Enabled = false;                            
                            //UploadHighEdu.Visible = false;  
                            ShowAlert("Upload Successfully");

                        }
                        else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Lblerror.Text = ex.Message;
                        Lblerror.Visible = true;
                        //txtAppName.Focus();
                        ShowAlert(ex.Message);
                    }
                }
            }
        }
    }
    protected void UploadMotherCerti_Click(object sender, EventArgs e)
    {
        if (IsValidMotherCert()) 
        {
            if (FileMotherCert.HasFile) 
            {

                using (var db = new EConnectContext())
                {
                    try
                    {
                        lblMother.Text = ""; 
                        Lblerror.Text = "";
                        #region[Upload Mother Certificate]
                        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

                        Int32 Upd_Field_MotherName = Convert.ToInt32(enmCandidateUpdateRequestFields.MotherName);
                        Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);

                        var objCandUploadDoc_MotherName = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.requestNo == Request_Number &
                                                           c.updateFieldID == Upd_Field_MotherName & c.requestFinalised == true &
                                                           c.requestStatusID == App_Status_RequestReceived);
                        // db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.updateFieldID == 7);

                        if (objCandUploadDoc_MotherName != null)
                        {

                            string fileName = "MotherName_" + (System.IO.Path.GetFileName(FileMotherCert.PostedFile.FileName)); 
                            //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                            string filePath = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/");
                            if (!Directory.Exists(filePath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(filePath);
                            }
                            string filePathadd = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/" +  fileName);
                            // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                            FileMotherCert.SaveAs(filePath + fileName);

                            objCandUploadDoc_MotherName.FileUploadedPath = filePathadd;
                            objCandUploadDoc_MotherName.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded); //(DocUpload)
                            objCandUploadDoc_MotherName.paymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending); //(Payment Panding) 
                            objCandUploadDoc_MotherName.docsVerified = false;
                            db.SaveChanges();

                            lblMotherFile.Enabled = true;
                            lblMotherFile.Text = fileName;
                            showMotherCert.Text = "Upload Successfully";
                            //FileHighEducation.Enabled = false;                            
                            //UploadHighEdu.Visible = false;  
                            ShowAlert("Upload Successfully");

                        }
                        else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Lblerror.Text = ex.Message;
                        Lblerror.Visible = true;
                        //txtAppName.Focus();
                        ShowAlert(ex.Message);
                    }
                }
            }
        }
    }
    protected void UploadDobCerti_Click(object sender, EventArgs e)
    {
        if (IsValidDobCert())
        {
            if (FileDobCert.HasFile) 
            {

                using (var db = new EConnectContext())
                {
                    try
                    {
                       // lblDob.Text = ""; 
                        Lblerror.Text = "";
                        #region[Upload DOB Certificate]
                        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

                        Int32 Upd_Field_DOB = Convert.ToInt32(enmCandidateUpdateRequestFields.DOB);
                        Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);

                        var objCandUploadDoc_DOB = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.requestNo == Request_Number &
                                                           c.updateFieldID == Upd_Field_DOB & c.requestFinalised == true &
                                                           c.requestStatusID == App_Status_RequestReceived);
                        // db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.updateFieldID == 7);

                        if (objCandUploadDoc_DOB != null)
                        {

                            string fileName = "Dob_" + (System.IO.Path.GetFileName(FileDobCert.PostedFile.FileName)); 
                            //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                            string filePath = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/");
                            if (!Directory.Exists(filePath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(filePath);
                            }
                            string filePathadd = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/" +  fileName);
                            // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                            FileDobCert.SaveAs(filePath + fileName);

                            objCandUploadDoc_DOB.FileUploadedPath = filePathadd;
                            objCandUploadDoc_DOB.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded); //(DocUpload)
                            objCandUploadDoc_DOB.paymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending); //(Payment Panding) 
                            objCandUploadDoc_DOB.docsVerified = false;
                            db.SaveChanges();

                            lblDobFile.Enabled = true;
                            lblDobFile.Text = fileName;
                            showDobCert.Text = "Upload Successfully"; 
                            //FileHighEducation.Enabled = false;                            
                            //UploadHighEdu.Visible = false;  
                            ShowAlert("Upload Successfully");

                        }
                        else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Lblerror.Text = ex.Message;
                        Lblerror.Visible = true;
                        //txtAppName.Focus();
                        ShowAlert(ex.Message);
                    }
                }
            }
        }
    }

    protected void UploadGenderCerti_Click(object sender, EventArgs e)
    {
        if (IsValidGenderCert()) 
        {
            if (FileGenderCert.HasFile) 
            {

                using (var db = new EConnectContext())
                {
                    try
                    {
                        lblGender.Text = ""; 
                        Lblerror.Text = "";
                        #region[Upload Gender Certificate]
                        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

                        Int32 Upd_Field_Gender = Convert.ToInt32(enmCandidateUpdateRequestFields.Gender);
                        Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);

                        var objCandUploadDoc_Gender = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.requestNo == Request_Number &
                                                           c.updateFieldID == Upd_Field_Gender & c.requestFinalised == true &
                                                           c.requestStatusID == App_Status_RequestReceived);
                        // db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.updateFieldID == 7);

                        if (objCandUploadDoc_Gender != null)
                        {

                            string fileName = "Gender_" + (System.IO.Path.GetFileName(FileGenderCert.PostedFile.FileName));
                            //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                            string filePath = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/");
                            if (!Directory.Exists(filePath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(filePath);
                            }
                            string filePathadd = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/" +  fileName);
                            // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                            FileGenderCert.SaveAs(filePath + fileName);

                            objCandUploadDoc_Gender.FileUploadedPath = filePathadd;
                            objCandUploadDoc_Gender.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded); //(DocUpload)
                            objCandUploadDoc_Gender.paymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending); //(Payment Panding) 
                            objCandUploadDoc_Gender.docsVerified = false;
                            db.SaveChanges();

                            lblGenderFile.Enabled = true;
                            lblGenderFile.Text = fileName;
                            showGenderCert.Text = "Upload Successfully"; 
                            //FileHighEducation.Enabled = false;                            
                            //UploadHighEdu.Visible = false;  
                            ShowAlert("Upload Successfully");

                        }
                        else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Lblerror.Text = ex.Message;
                        Lblerror.Visible = true;
                        //txtAppName.Focus();
                        ShowAlert(ex.Message);
                    }
                }
            }
        }
    }
    protected void UploadPerAddressCerti_Click(object sender, EventArgs e)
    {
        if (IsValidPerAddress()) 
        {
            if (FilePerAddressCert.HasFile) 
            {

                using (var db = new EConnectContext())
                {
                    try
                    {
                        lblPerAddress.Text = ""; 
                        Lblerror.Text = "";
                        #region[Upload Address Certificate]
                        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

                        Int32 Upd_Field_PermanentAddress = Convert.ToInt32(enmCandidateUpdateRequestFields.PermanentAddress);
                        Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);

                        var objCandUploadDoc_PermanentAddress = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.requestNo == Request_Number &
                                                           c.updateFieldID == Upd_Field_PermanentAddress & c.requestFinalised == true &
                                                           c.requestStatusID == App_Status_RequestReceived);
                        // db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.updateFieldID == 7);

                        if (objCandUploadDoc_PermanentAddress != null)
                        {

                            string fileName = "PerAddress_" + (System.IO.Path.GetFileName(FilePerAddressCert.PostedFile.FileName)); 
                            //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                            string filePath = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/");
                            if (!Directory.Exists(filePath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(filePath);
                            }
                            string filePathadd = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/" +  fileName);
                            // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                            FilePerAddressCert.SaveAs(filePath + fileName);

                            objCandUploadDoc_PermanentAddress.FileUploadedPath = filePathadd;
                            objCandUploadDoc_PermanentAddress.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded); //(DocUpload)
                            objCandUploadDoc_PermanentAddress.paymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending); //(Payment Panding) 
                            objCandUploadDoc_PermanentAddress.docsVerified = false;
                            db.SaveChanges();

                            lblPerAddressFile.Enabled = true;
                            lblPerAddressFile.Text = fileName;
                            showPerAddressCert.Text = "Upload Successfully"; 
                            //FileHighEducation.Enabled = false;                            
                            //UploadHighEdu.Visible = false;  
                            ShowAlert("Upload Successfully");

                        }
                        else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Lblerror.Text = ex.Message;
                        Lblerror.Visible = true;
                        //txtAppName.Focus();
                        ShowAlert(ex.Message);
                    }
                }
            }
        }
    }

    protected void UploadRegTypeCerti_Click(object sender, EventArgs e)
    {
        if (IsValidRegType())
        {
            if (FileRegTypeCert.HasFile) 
            {
                using (var db = new EConnectContext())
                {
                    try
                    {
                        lblRegType.Text = "";
                        Lblerror.Text = "";
                        #region[Upload RegType Certificate]
                        Int64 Request_Number = Convert.ToInt64(Request.QueryString["Request_Number"]);

                        Int32 Upd_Field_RegType = Convert.ToInt32(enmCandidateUpdateRequestFields.RegistrationType);
                        Int32 App_Status_RequestReceived = Convert.ToInt32(enmCandidateUpdateRequestStatus.RequestReceived);

                        var objCandUploadDoc_RegType = db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.requestNo == Request_Number &
                                                           c.updateFieldID == Upd_Field_RegType & c.requestFinalised == true &
                                                           c.requestStatusID == App_Status_RequestReceived);
                        // db.candidateUpdateRequest.SingleOrDefault(c => c.candID == entityID & c.updateFieldID == 7);

                        if (objCandUploadDoc_RegType != null)
                        {

                            string fileName = "RegType_" + (System.IO.Path.GetFileName(FileRegTypeCert.PostedFile.FileName));
                            //string filePath = @"D:\work\NIELIT_WEB_2012\NIELIT_WEB\Candidate_Update_Req_Docs/" + DateTime.Now.ToString("ddMMyyyy_HH_mm_ss") + "/" + entityID + "/" + fileName;
                            string filePath = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/");
                            if (!Directory.Exists(filePath))
                            {
                                //If Directory (Folder) does not exists. Create it.
                                Directory.CreateDirectory(filePath);
                            }
                            string filePathadd = Server.MapPath(@"" + "~/UploadedFiles/Candidate_Update_Req_Docs/" + entityID + "/" + fileName);
                            // fileHandicapped.SaveAs(filePath + Path.GetFileName(fileHandicapped.FileName));
                            FileRegTypeCert.SaveAs(filePath + fileName); 

                            objCandUploadDoc_RegType.FileUploadedPath = filePathadd;
                            objCandUploadDoc_RegType.requestStatusID = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded); //(DocUpload)
                            objCandUploadDoc_RegType.paymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending); //(Payment Panding) 
                            objCandUploadDoc_RegType.docsVerified = false;
                            db.SaveChanges();

                            lblRegTypeFile.Enabled = true;
                            lblRegTypeFile.Text = fileName;
                            showRegTypeCert.Text = "Upload Successfully"; 
                            //FileHighEducation.Enabled = false;                            
                            //UploadHighEdu.Visible = false;  
                            ShowAlert("Upload Successfully");

                        }
                        else { ShowAlert("No Request Available..Kindly first require to add Updation/Correction request against the fields "); }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Lblerror.Text = ex.Message;
                        Lblerror.Visible = true;
                        //txtAppName.Focus();
                        ShowAlert(ex.Message);
                    }
                }
            }
            }
    }

    #endregion
    protected void LinkDownloadHandi_Click(object sender, EventArgs e)
    {
        var filePath = "~/Candidate_Update_Req_Docs/" + entityID + "/"; //fileName;
       // string filename = "~/Downloads/msizap.exe";
        if (filePath != "")
        {
            string path = Server.MapPath(filePath);
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(file.FullName);
                Response.End();
            }
            else
            {
                ShowAlert("This file does not exist.");
               
            }
        }
    }

    #region[Method_Fee]
    protected Int64 GetFeeAmount(Int64 requestNo, Int32 courseID)//(Int32 regstatus, Int32 UpdatefieldId, Int32 courseID)
    {
        try
        {
            Int32 regstatusid = Convert.ToInt32(Request.QueryString["regStatusId"]);
            Int64 feeAmt = 0; Int32 FeeTypeID = 0;

            using (EConnectContext context = new EConnectContext())
            {
                if (!String.IsNullOrEmpty(Request.QueryString["candidateID"])) // FeeCalculation after Final submit
                //{ FeeTypeID = Convert.ToInt32(enmFeeType.UpdationinCandidateData); }
                //else
                {

                    if (FeeTypeID == 0)
                        FeeTypeID = 24;

                    Int32 App_Status_DocumentsUploaded = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);
                    Int32 FeeStatus = Convert.ToInt32(enmPaymentStatus.Pending);

                    var feeAmount = (from u in context.UpdateRegnMasters
                                     join c in context.candidateUpdateRequest on u.ID equals c.updateFieldID
                                     where c.candID == entityID && c.requestStatusID == App_Status_DocumentsUploaded &&
                                     c.courseID == courseID//(18/01/2023)currentCourseID 
                                     && c.requestNo == requestNo && c.paymentStatusID == FeeStatus && c.requestFinalised == true
                                     select new { FeeAmount = u.feesAmt }).FirstOrDefault();

                    feeAmt = Convert.ToInt64(feeAmount.FeeAmount);

                    

                }
               

            };
            return feeAmt;
        }
        catch (Exception ex) { throw ex; }
    }

    protected Int64 GetFeeTotalAmount(Int64 requestNo, Int32 courseID) //for total fee of updation request
    {
        try
        {
            Int32 regstatusid = Convert.ToInt32(Request.QueryString["regStatusId"]);
            Int64 feeAmtTot = 0; Int32 FeeTypeID = 0;

            using (EConnectContext context = new EConnectContext())
            {
                if (!String.IsNullOrEmpty(Request.QueryString["candidateID"])) // FeeCalculation after Final submit
                //{ FeeTypeID = Convert.ToInt32(enmFeeType.UpdationinCandidateData); }
                //else
                {

                    if (FeeTypeID == 0)
                        FeeTypeID = 24;

                    Int32 App_Status_DocumentsUploaded = Convert.ToInt32(enmCandidateUpdateRequestStatus.DocumentsUploaded);
                    Int32 FeeStatus = Convert.ToInt32(enmPaymentStatus.Pending);

                    var feeAmount = (from u in context.UpdateRegnMasters
                                     join c in context.candidateUpdateRequest on u.ID equals c.updateFieldID
                                     where c.candID == entityID && c.requestStatusID == App_Status_DocumentsUploaded &&
                                     c.courseID == courseID //(18/01/2023)currentCourseID 
                                     && c.requestNo == requestNo && c.requestFinalised == true
                                     && c.paymentStatusID == FeeStatus//Convert.ToInt32(enmPaymentStatus.Pending)
                                     select new { FeeAmount = u.feesAmt }).ToList();

                    var sumOfFee = feeAmount.Sum(s => s.FeeAmount);

                    feeAmtTot = Convert.ToInt64(sumOfFee);                  

                }


            };
            return feeAmtTot;
        }
        catch (Exception ex) { throw ex; }
    
    }
    #endregion












    
}