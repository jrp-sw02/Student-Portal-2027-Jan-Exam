
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using ZXing;

public partial class SpecialReRegistrationPreview : BasePage
{

    int stateid;
    // int courseId;
    int applicant_type_id;
    // int id;
    private long candidateId;
    private int courseId;
    string APPLNum;
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            Page.Header.DataBind();
            //  ImgUpload.Attributes["onchange"] = "UploadFile(this)";
            Page.Header.DataBind();

            using (EConnectContext context = new EConnectContext())
            {
                currentRoleId = Convert.ToInt32(Session["RoleID"]);
                if (!string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
                {
                    candidateId = Convert.ToInt64(Request.QueryString["CandidateId"]);
                    hdnCandidateID.Value = candidateId.ToString();
                    if (!string.IsNullOrEmpty(Request.QueryString["CourseId"]))
                    {
                        courseId = Convert.ToInt16(Request.QueryString["CourseId"]);
                    }
                }
                else if (currentRoleId == 6 || currentRoleId == 4)
                {
                    candidateId = Convert.ToInt64(Request.QueryString["CandidateId"]);
                    hdnCandidateID.Value = candidateId.ToString();
                    courseId = Convert.ToInt16(Request.QueryString["CourseId"]);
                }
                else
                {
                    candidateId = Convert.ToInt64(Session["EntityID"]);
                    //  var application = context.RegistrationDetails.Find(id);
                    var registration = context.RegistrationDetails.FirstOrDefault(x => x.CandidateID == candidateId);
                    // long id = application.CandidateID;
                    if (registration != null)
                    {
                        hdnCandidateID.Value = candidateId.ToString();
                        courseId = registration.CourseID;

                    }
                }
            }
            //  ImgUploadSignature.Attributes["onchange"] = "imageSignpreview(this)";
            //   divPopup.Style.Add("display", "none");
            // string APPLNum = "ROIT0813000014";
            //string APPLNum = Request.QueryString["APPLNum"];
            //LblAppNumber.Text = APPLNum;
            trexam.Visible = true;
            string ExamCycle = Request.QueryString["ExamCycle"];
            string ExamFee = Request.QueryString["ExamFee"];
            //ViewState["ExamCycle"] = 
            Int32 applicant_type_id = !string.IsNullOrEmpty(Request.QueryString["applicant_type_id"]) ? Convert.ToInt32(Request.QueryString["applicant_type_id"]) : 0;
            lblExamcycle.Text = ExamCycle;
            lblFeeDetail.Text = ExamFee;
            // int applicant_type_id = 1;
            if (applicant_type_id == 1 && LblCourse.Text != "A Level")
            {
                TrStateCenterAccno.Visible = false;
                TrCenterNameAccno.Visible = false;
                TrInstitutedetails.Visible = false;
              //  trexpcerti.Visible = true;
            }
            else
            {
                TrStateCenterAccno.Visible = true;
                TrCenterNameAccno.Visible = true;
                TrInstitutedetails.Visible = true;
               // trexpcerti.Visible = false;
            }
            if (Request.QueryString["appliedAs"] != null && Request.QueryString["appliedAs"].ToString() == "I")
            {
                lblapplicantTypeName.Text = "Institute";
            }
            else
            {
                lblapplicantTypeName.Text = "Direct";
            }

            if (!IsPostBack)
            {

                // bindState();

                // Clear session initially
                //Session["ImgUpload"] = null;
                //Session["ImgUploadSignature"] = null;
                //Session["PhotoFileName"] = null;
                //Session["SignFileName"] = null;

                //  long id = 885810;
                hdnCandidateID.Value = candidateId.ToString();
                //   int courseId = 1;
                string AccNo = Request.QueryString["AccNo"];
                BindMercyData(candidateId, courseId);
                if (String.IsNullOrEmpty(LblFName.Text) && String.IsNullOrEmpty(LblMName.Text))
                {
                    trguardian.Visible = true;
                    TrFatherName.Visible = false;
                    TrMotherName.Visible = false;
                }
                else
                {
                    trguardian.Visible = false;
                    TrFatherName.Visible = true;
                    TrMotherName.Visible = true;
                }
                if (Request.QueryString["Mode"] == "View" || Request.QueryString["Mode"] == "Preview" || Request.QueryString["Type"] == "Print" || !string.IsNullOrEmpty(Request.QueryString["CandidateId"]))
                {
                    Btnsubmit.Visible = false;
                    Btnback.Visible = false;
                }
                if (Request.QueryString["Type"] == "Print")
                {
                    showdata();
                    // return;
                }

            }
            SetSectionNumbers();


        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    public void BindMercyData(long candidateId, int courseId)
    {
        string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("sp_get_MercyData_Preview", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", candidateId);
                cmd.Parameters.AddWithValue("@Course_ID", courseId);
                // cmd.Parameters.AddWithValue("@Applicant_Type_ID", applicant_type_id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    // Response.Write("<br/>Institute = [" + dr["Institute_Name"] + "]");
                    //for (int i = 0; i < dr.FieldCount; i++)
                    //{
                    //    Response.Write(dr.GetName(i) + " = [" + dr[i] + "]<br/>");
                    //}
                    //Response.End();
                    // 🔸 Applicant Details
                    LblAppName.Text = dr["Name"].ToString();
                    LblAppNumber.Text = dr["Number"].ToString();
                    LblAppDataTime.Text = dr["Date"].ToString();
                    ViewState["AppID"] = LblAppNumber.Text.ToString();
                    LblFName.Text = dr["Father_Name"].ToString();
                    LblMName.Text = dr["Mother_Name"].ToString();
                    LblDob.Text = Convert.ToDateTime(dr["Dob"]).ToString("dd/MM/yyyy");

                    // 🔸 Category / Gender / Status
                    LblCategory.Text = dr["Category"].ToString();
                    LblGender.Text = dr["Gender"].ToString();
                    LblMaritalStatus.Text = dr["Marital_Status"].ToString();

                    // 🔸 Flags
                    LblExService.Text = dr["Is_Ex_Servicemane"].ToString();
                    LblHandicapped.Text = dr["Is_Handicaped"].ToString();

                    // 🔸 Religion
                    LblReligion.Text = dr["Religion_Name"].ToString();

                    // 🔸 Contact Details

                    LblLandLine.Text = String.IsNullOrEmpty(dr["Phone"].ToString()) ? "NA" : dr["Std"].ToString() + " - " + dr["Phone"].ToString();
                    //LblLandLine.Text = dr["Phone"].ToString();
                    lblMobile.Text = dr["Mobile"].ToString();
                    lblEmail.Text = dr["Email"].ToString();

                    // 🔸correspondence Address
                    lblCorAddressLine1.Text = dr["Corr_Address1"].ToString();
                    lblCorAddressLine2.Text = dr["Corr_Address2"].ToString();
                    lblCorAddressLine3.Text = dr["Corr_Address3"].ToString();
                    lblCorCity.Text = dr["Corr_City"].ToString();
                    //  ddlCorState.SelectedValue = dr["State_ID"].ToString();

                    lblCorState.Text = dr["Corr_State"].ToString();
                    LblDistrict.Text = dr["Cor_DistrictName"].ToString();
                    //if (DdlAccState.SelectedValue != "0")
                    //{
                    //    BindAccCentre(Convert.ToInt32(DdlAccState.SelectedValue), courseId);
                    //}
                    // lblInstName.Text = dr["Institute_Name"].ToString();
                    lblInstName.Text = dr["Institute_Name"] == DBNull.Value ? "" : dr["Institute_Name"].ToString();
                    //LblAccNo.Text = Request.QueryString["AccNo"] ?? "";
                    ////LblAccNo.Text = dr["Accreditation_Number"].ToString();
                    //if (Request.QueryString["AccNo"] != null)
                    //{
                    //    LblAccNo.Text = Request.QueryString["AccNo"].ToString();
                    //}
                    LblAccNo.Text = dr["Accreditation_Number"] == DBNull.Value
                    ? ""
                    : dr["Accreditation_Number"].ToString();
                    lblPincode.Text = dr["Corr_Pin"].ToString();

                    // 🔸Permanent Address
                    lblPerAddressLine1.Text = dr["Per_Address1"].ToString();
                    lblPerAddressLine2.Text = dr["Per_Address2"].ToString();
                    lblPerAddressLine3.Text = dr["Per_Address3"].ToString();
                    lblPerCity.Text = dr["Per_City"].ToString();

                    lblPState.Text = dr["Per_State"].ToString();
                    LblPerDistrict.Text = dr["Per_DistrictName"].ToString();
                    LblPerPinCode.Text = dr["Per_Pin"].ToString();

                    // 🔸 Course & Institute
                    LblCourse.Text = dr["Course_Name"].ToString();

                    lblRegistrationNo.Text = dr["Registration_No"].ToString();
                    // Apaar

                    string encryptedApaar = Convert.ToString(dr["Apaar_ID"]);
                    string apaarPlainText = EncryptDecrypt.DecryptString(encryptedApaar);
                    EConnectContext context = new EConnectContext();
                    var apaarRequest = context.ApaarRequest
                        .Where(x => x.apaarId == apaarPlainText)
                        .OrderByDescending(x => x.ID)
                        .FirstOrDefault();

                    lblApaarId.Text = apaarPlainText;
                    if (!string.IsNullOrEmpty(lblApaarId.Text))
                    {
                        tblapaar.Visible = true;
                    }
                    else
                    {
                        tblapaar.Visible = false;
                    }
                    lblproviderName.Text = apaarRequest.providerName;
                    lblIsProviderPresent.Text = "True";
                    lblAuthId.Text = apaarRequest.authModeIDNo.ToString();
                    lblIsConsentRelation.Text = apaarRequest.consentRelation;
                    lblConsentPlace.Text = apaarRequest.consentPlace;
                    lblConsentDate.Text = apaarRequest.consentDate.Value.ToString("dd/MM/yyyy");
                    lblConsentTime.Text = apaarRequest.consentTime.Value.ToString(@"hh\:mm");
                    // lblAuthMode.Text = Convert.ToString(apaarRequest.authModeID); //context.AuthMode.Where(x => x.ID == apaarRequest.authModeID).Select(x => x.authCode).FirstOrDefault();
                    lblAuthMode.Text = GetAuthModeName(apaarRequest.authModeID);
                    //  lblApaarId.Text = dr["Apaar_ID"].ToString();
                    if (dr["Photo"] != DBNull.Value)
                    {
                        byte[] bytes = (byte[])dr["Photo"];
                        string base64String = Convert.ToBase64String(bytes);
                        ImgApplicantPhoto.ImageUrl = "data:image/jpg;base64," + base64String;
                    }
                    if (dr["Signature"] != DBNull.Value)
                    {
                        byte[] bytes = (byte[])dr["Signature"];
                        string base64String = Convert.ToBase64String(bytes);
                        ImgApplicantSign.ImageUrl = "data:image/jpg;base64," + base64String;
                    }
                    LblregDate.Text = Convert.ToDateTime(dr["Registration_Date"]).ToString("dd/MM/yyyy");
                    LblValidityDate.Text = Convert.ToDateTime(dr["Valid_Upto_Date"]).ToString("dd/MM/yyyy");
                    lblRegistrationType.Text = dr["REG_TYPE"].ToString();
                }


                //  ImgApplicantPhoto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])application.Photo);
                con.Close();
            }
        }
        // For Documents
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("sp_GetMercyDocuments", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@candidate_id", candidateId);

                conn.Open();
                SqlDataReader drDoc = cmd.ExecuteReader();

                // Default values (important)
                //lblEduStatus.Text = "Not Uploaded";
                //lblExpCertiFormStatus.Text = "Not Uploaded";
                lblMedicalStatus.Text = "Not Uploaded";
                //lblIdCardstatus.Text = "Not Uploaded";

                while (drDoc.Read())   // ✅ IMPORTANT CHANGE
                {
                    string docType = drDoc["Doc_Type"].ToString().Trim();
                    string status = drDoc["Upload_Status"].ToString();

                    //if (docType == "EDUCATION CERTIFICATE")
                    //{
                    //    lblEduStatus.Text = status;
                    //}
                    //else if (docType == "EXPERIENCE CERTIFICATE")
                    //{
                    //    lblExpCertiFormStatus.Text = status;
                    //}
                    if (docType == "PROOF OF MEDICAL ISSUE/AFFIDAVIT")
                    {
                        lblMedicalStatus.Text = status;
                    }
                    //else if (docType == "ID CARD")
                    //{
                    //    lblIdCardstatus.Text = status;
                    //}
                }

                drDoc.Close();

                //SqlDataReader dr = cmd.ExecuteReader();

                //if (dr.Read())
                //{

                //}
            }
        }
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
                //  var apaar = context.ApaarRequest.Find(appID);
                //   lblConsentDate.Text=apaar.consentDate.ToString();
                //  Int32 examID = application.ApplicableExamID.HasValue ? application.ApplicableExamID.Value : 0;
                //LblCourse.Text = application.Course.Name;
                //if (application.apaarID != null)
                //{
                //    string apaarDecrypt = EncryptDecrypt.DecryptString(application.apaarID);
                //    lblApaarId.Text = apaarDecrypt;
                //}
                //else
                //    lblApaarId.Text = "N/A";
                LblAppDataTime.Text = application.ApplicationDate.ToString();
                LblAppNumber.Text = application.Number.ToString();
                //  lblConsentDate.Text=application.con
                //lblRegistrationNo.Text=application.RegisteredCourseRegistrationNo.ToString();
                //lblExamcycle.Text=application.ApplicableExamID.ToString();
                //lblFeeDetail.Text=application.FeeAmount.ToString();
                //var institute = application.Institute;
                //var instituteDetail = (from a in context.AccreditationDetails
                //                       join i in context.Institutes on a.InstituteID equals i.ID
                //                       where i.ID == application.InstituteID && a.CourseID == application.CourseID
                //                       select new
                //                       {
                //                           Name = a.AccreditationNumber + "-" + i.Name + ", " + i.CityName,
                //                           AccNo = a.AccreditationNumber
                //                       }).FirstOrDefault();
                //lblInstName.Text=instituteDetail.Name;
                //LblAccNo.Text=instituteDetail.AccNo;
                if (application.InstituteID != null && application.InstituteID > 0)
                {
                    var instituteDetail = (from a in context.AccreditationDetails
                                           join i in context.Institutes
                                               on a.InstituteID equals i.ID
                                           where a.InstituteID == application.InstituteID
                                              && a.CourseID == application.CourseID
                                           select new
                                           {
                                               Name = a.AccreditationNumber + "-" + i.Name + ", " + i.CityName,
                                               AccNo = a.AccreditationNumber
                                           }).FirstOrDefault();

                    if (instituteDetail != null)
                    {
                        lblInstName.Text = instituteDetail.Name;
                        LblAccNo.Text = instituteDetail.AccNo;
                    }
                    TrStateCenterAccno.Visible = true;
                    TrCenterNameAccno.Visible = true;
                    TrInstitutedetails.Visible = true;
                }
                else
                {
                    TrStateCenterAccno.Visible = false;
                    TrCenterNameAccno.Visible = false;
                    TrInstitutedetails.Visible = false;

                }
                trexam.Visible = false;
                if (application.enmApplicantType == enmApplicantType.Institute)
                {
                    lblapplicantTypeName.Text = "Institute";
                }
                else
                {
                    lblapplicantTypeName.Text = "Direct";
                }
                Btnsubmit.Visible = false;
                Btnback.Visible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }

    private string GetAuthModeName(long? authModeID)
    {
        string authModeName = "";

        using (SqlConnection con = new SqlConnection(
            ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
        {
            string query = "SELECT authCode FROM authModes WHERE ID = @ID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ID", authModeID);

                con.Open();

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    authModeName = result.ToString();
                }
            }
        }

        return authModeName;
    }

    protected void Btnback_Click(object sender, EventArgs e)
    {
        string AccStateID = Request.QueryString["AccStateID"];
        string AccID = Request.QueryString["AccID"];
        string CorrStateID = Request.QueryString["CorrStateID"];
        string CorrDistrictID = Request.QueryString["CorrDistrictID"];
        string PerDistrictID = Request.QueryString["PerDistrictID"];
        string appID = Convert.ToString(ViewState["AppID"]);
        string appliedAs = Request.QueryString["AppliedAs"];
        Response.Redirect("../CAND/SpecialReRegistration.aspx?id=" + candidateId + "&courseId=" + courseId + "&applicant_type_id=" + applicant_type_id + "&AccStateID=" + AccStateID
        + "&AccID=" + AccID + "&CorrStateID=" + CorrStateID + "&CorrDistrictID=" + CorrDistrictID + "&PerDistrictID=" + PerDistrictID + "&APPLNum=" + appID + "&AppliedAs=" + appliedAs);
    }

    protected void Btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            long id = Convert.ToInt64(hdnCandidateID.Value);
            // int applicant_type_id = 2;
            int applicant_type_id = Convert.ToInt32(Session["ApplicantTypeID"]);
            string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            //  int courseId = 1;
            int courseId = Convert.ToInt32(Session["CourseID"]);
            long Appid = 0;
            int ApplicationFlag = 0;


            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("sp_Update_FinalMercyApplication", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Candidate_ID", id);
                cmd.Parameters.AddWithValue("@Course_ID", courseId);
                // Output Parameter : DemandNoteID
                SqlParameter demandParam = new SqlParameter("@DemandNoteID", SqlDbType.BigInt);
                demandParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(demandParam);

                // Output Parameter : LatestID
                SqlParameter latestParam = new SqlParameter("@LatestID", SqlDbType.BigInt);
                latestParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(latestParam);

                con.Open();
                cmd.ExecuteNonQuery();
                long latestID = Convert.ToInt64(cmd.Parameters["@LatestID"].Value);
                long demandID = Convert.ToInt64(cmd.Parameters["@DemandNoteID"].Value);
                using (EConnectContext context = new EConnectContext())
                {
                    var application = context.CourseRegistrationApplications.Find(latestID);
                    if (application.DemandNoteID.HasValue)
                    {
                        //  DemandNote demand = new DemandNote();
                        // Decide status here

                        if (application.ApplicantTypeID == (int)enmApplicantType.Direct)
                        {
                            //  demand.ApplicationDate = DateTime.Now;
                            // // demand.FeeTypeID = application.FeeTypeID.Value;
                            //  demand.ApplicationTypeID = Convert.ToInt32(enmApplicationType.MercyCaseRegistration);
                            //  demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.Online);
                            //  demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Single);
                            ////  demand.Amount = application.FeeAmount.Value;
                            //  demand.CreatedBy = 1;
                            //  demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                            //  demand.CourseCategoryID = application.CourseCategoryID;
                            //  demand.CourseID = application.CourseID;
                            //  demand.ServiceID = application.Course.RegistrationServiceID;
                            //  context.DemandNotes.Add(demand);
                            //  context.SaveChanges();
                            //   application.DemandNoteID = demand.ID;
                            application.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.AppliedButFeeNotDepositedByCandidate);

                            //  demandID = demand.ID;
                            ApplicationFlag = 1;
                            //  application.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.AppliedButFeeNotDepositedByCandidate);

                        }
                        //else
                        //{
                        //    application.ApplicationStatusID =
                        //        (int)enmCourseApplicationStatus.YYYYY;
                        //}
                        else if (application.enmApplicantType == enmApplicantType.Institute)
                        {
                            if (application.enmPaymentSource == enmPaymentSource.Candidate)
                            {
                                //demand.ApplicationDate = DateTime.Now;
                                ////   demand.FeeTypeID = application.FeeTypeID.Value;
                                //demand.ApplicationTypeID = Convert.ToInt32(enmApplicationType.MercyCaseRegistration);
                                //demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.Online);
                                //demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Single);
                                //// demand.Amount = application.FeeAmount.Value;
                                //demand.CreatedBy = 1;
                                //demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);

                                //demand.CourseCategoryID = application.CourseCategoryID;
                                //demand.CourseID = application.CourseID;
                                //demand.ServiceID = application.Course.RegistrationServiceID;
                                ////end----------
                                //context.DemandNotes.Add(demand);
                                //context.SaveChanges();

                                //application.DemandNoteID = demand.ID;
                                application.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.AppliedButFeeNotDepositedByCandidate);

                                //  demandID = demand.ID;
                                ApplicationFlag = 1;
                            }
                        }

                        context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                    }
                }


                //   con.Open();
                // cmd.Parameters.Add("@DemandNoteID", SqlDbType.BigInt);
                //  cmd.Parameters["@DemandNoteID"].Direction = ParameterDirection.Output;
                //   cmd.ExecuteNonQuery();
                // long demandID =Convert.ToInt64(cmd.Parameters["@DemandNoteID"].Value);
                if ((!String.IsNullOrEmpty(Request.QueryString["Appid"])) && (Request.QueryString["Src"] == "CSC"))
                    Response.Redirect("../nieletpaymentservices.aspx?ServiceID=4&DID=" + demandID.ToString());
                else if (ApplicationFlag == 1)//Application type: Direct
                    Response.Redirect("~/CAND/FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.MercyCaseRegistration).ToString() + "&Appid=" + latestID.ToString() + "&DemandID=" + demandID.ToString());
                //else if (applicant_type_id == 2)//Application type:Institute
                //    Response.Redirect("~/CAND/FrmConfirm.aspx?TypeID=" + Convert.ToInt32(enmApplicationType.MercyCaseRegistration).ToString() + "&Appid=" + latestID.ToString() + "&DemandID=" + demandID.ToString());
                //  if ((!String.IsNullOrEmpty(Request.QueryString["id"])) && (!String.IsNullOrEmpty(Request.QueryString["courseId"])) && (!String.IsNullOrEmpty(Request.QueryString["applicant_type_id"])))
                //  {

                //Response.Redirect("../CAND/MercyCasePreview.aspx?id=" + id
                //    + "&courseId=" + courseId
                //    + "&applicant_type_id=" + applicant_type_id
                //    + "&APPLNum=" + APPLNum);                //  }
                //else
                //{
                //    Response.Redirect("../CAND/MercyCaseRegistration.aspx?id=" + id + "&courseId=" + courseId + "&applicant_type_id=" + applicant_type_id + "");
                //}

            }

        }

        catch (SqlException ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    private void SetSectionNumbers()
    {
        int sectionNo = 5;

        // ==============================
        // INSTITUTE DETAILS
        // ==============================
        if (TrInstitutedetails.Visible)
        {
            sectionNo++;

            LblSectionInstituteNo.Text = sectionNo + ".";

            Lbl2.Text = sectionNo + ".1";
            Lbl3.Text = sectionNo + ".2";
        }

        // ==============================
        // IDENTIFICATION DETAILS
        // ==============================
        sectionNo++;

        LblSectionIdentificationNo.Text = sectionNo + ".";

        LblIdentification1.Text = sectionNo + ".1";
        LblIdentification2.Text = sectionNo + ".2";
        LblIdentification3.Text = sectionNo + ".3";
        LblIdentification4.Text = sectionNo + ".4";
        LblIdentification5.Text = sectionNo + ".5";
        LblIdentification6.Text = sectionNo + ".6";
        LblIdentification7.Text = sectionNo + ".7";
        LblIdentification8.Text = sectionNo + ".8";

        // ==============================
        // DOCUMENTS
        // ==============================
        sectionNo++;

        LblSectionDocumentsNo.Text = sectionNo + ".";

        int documentNo = 1;

        lblDocNo1.Text = sectionNo + "." + documentNo++;

        //if (trexpcerti.Visible)
        //{
        //    lblDocNo2.Text = sectionNo + "." + documentNo++;
        //}

        //lblDocNo3.Text = sectionNo + "." + documentNo++;
        //lblDocNo4.Text = sectionNo + "." + documentNo++;
    }
}