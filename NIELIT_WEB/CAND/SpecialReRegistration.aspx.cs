using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IdentityModel.Metadata;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
//using static System.Runtime.CompilerServices.RuntimeHelpers;


public partial class SpecialReRegistration : BasePage
{
  
    int stateid;
   // int courseId;
    int applicant_type_id;
    private long candidateId;
    private int courseId;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;

            if (IsSessionAlive() == false)
            {
                Response.Redirect("../Index.aspx");
                return;
            }

            Page.Header.DataBind();

         //   ImgUpload.Attributes["onchange"] = "UploadFile(this)";
          //  ImgUploadSignature.Attributes["onchange"] = "imageSignpreview(this)";

          //  divPopup.Style.Add("display", "none");
           // tddeclaration1.Style.Add("display", "block");

            ImgBtnPopupFee.ToolTip = "";
            ImgBtnPopupFee.Enabled = false;
            ImgBtnPopupFee.ImageUrl = "~/images/DisablePopup.PNG";

            lblFeeDetail.Text = "";
            RdoUndergngDOEACC.Enabled = false;

            using (EConnectContext context = new EConnectContext())
            {
                candidateId = Convert.ToInt64(Session["EntityID"]);

                var registration = context.RegistrationDetails
                    .FirstOrDefault(x => x.CandidateID == candidateId);

                if (registration != null)
                {
                    courseId = registration.CourseID;
                    Session["CourseID"] = courseId;
                }
            }

            // =====================================================
            // CHECK WHETHER CANDIDATE HAS ANY ON-HOLD DOCUMENT
            // =====================================================

            //if (IsDocumentReUploadRequired(candidateId))
            //{
            //    // Candidate has at least one document with status 4
            //    SetReUploadMode(true);

            //    // Load only those documents having status 4
            //    LoadHeldDocumentSection(candidateId);

            //    return;
            //}
            bool isReUpload = Request.QueryString["ReUpload"] == "1";

            if (isReUpload)
            {
                // URL is already:
                // MercyCaseRegistration.aspx?ReUpload=1
                BindMercyData(candidateId, courseId, "SR", true);
                SetReUploadMode(true);
                if (!IsPostBack)
                {
                    // Show only documents which are currently On Hold
                    LoadHeldDocumentSection(candidateId);
                }
                return;
            }

            // =========================================================
            // CHECK WHETHER CANDIDATE HAS ANY ON-HOLD DOCUMENT
            // =========================================================

            if (IsDocumentReUploadRequired(candidateId))
            {
                // Candidate has at least one document with status 4.
                // Redirect once with ReUpload=1.

                Response.Redirect("SpecialReRegistration.aspx?ReUpload=1");
                return;
            }


            // =========================================================
            // NORMAL PAGE
            // =========================================================

            SetReUploadMode(false);

            if (!IsPostBack)
            {
                txtConsentDate.Text =
                    DateTime.Today.ToString("dd-MM-yyyy");

                txtConsentTime.Text =
                    DateTime.Now.ToString("HH:mm");

                bindState(courseId);

                string appliedAs =
                    Request.QueryString["AppliedAs"];

                if (!string.IsNullOrEmpty(appliedAs))
                {
                    // Returning from Preview
                    RdoUndergngDOEACC.SelectedValue = appliedAs;

                    if (appliedAs == "I")
                    {
                        SetApplicantType(2);
                    }
                    else
                    {
                        SetApplicantType(1);
                    }
                }
                else
                {
                    // Fresh load
                    int applicantTypeId =
                        GetApplicantType(candidateId, courseId);

                    SetApplicantType(applicantTypeId);
                   
                }

                GetApplicantType(candidateId, courseId);

                string APPLNum =
                    Request.QueryString["APPLNum"];

                if (APPLNum != null)
                {
                    UpdateData();

                    Btnsubmit.Visible = false;
                    Btnupdate.Visible = true;
                }
                else
                {
                    bool isEligible =
                        BindMercyData(candidateId, courseId, "SR");

                    if (!isEligible)
                    {
                        lblerror.Text =
                            "You are not eligible for Special Re-Registration.";

                        Btnsubmit.Visible = false;
                        Btnupdate.Visible = false;
                        LblGuardianName.Enabled = false;
                        txtproviderName.Enabled = false;
                        txtIsProviderPresent.Enabled = false;
                        txtAuthenticationIdNo.Enabled = false;
                        ddlConsentRelation.Enabled = false;
                        txtConsentPlace.Enabled = false;
                        txtConsentDate.Enabled = false;
                        txtConsentTime.Enabled = false;
                        txtapaar.Enabled = false;
                        tdapaartext.Attributes.Add("style", "pointer-events:none;");
                        ddlAuthMode.Enabled = false;
                        chkApaarDeclaration.Enabled = false;
                        chkdisclamier.Enabled = false;
                      //  ImgUpload.Enabled = false;
                      //  ImgUploadSignature.Enabled = false;
                       // fuEduquacertiForm.Enabled = false;
                      //  fuexpcertiForm.Enabled = false;
                      //  FuIdCard.Enabled = false;
                        Fumedical.Enabled = false;
                        lnkContactUpdate.Enabled = false;
                        hypcandcontact.Enabled = false;
                        hypcandupdatereg.Enabled = false;
                      //  btnNotification.Enabled = false;
                        return;
                    }

                    Btnsubmit.Visible = true;
                    Btnupdate.Visible = false;

                    BindConsentRelationDropdown();
                }

                if (ViewState["ApplicantTypeID"] != null)
                {
                    int applicant_type_id =
                        Convert.ToInt32(ViewState["ApplicantTypeID"]);

                    bindExamName(
                        applicant_type_id,
                        courseId);
                }

                SetConsentRelationVisibility();
                SetSectionNumbers();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text =
                ex.Message.ToString() +
                ex.Source.ToString();
        }
    }

    private bool IsDocumentReUploadRequired(long candidateId)
    {
        trsecdoc1.Visible = false;
        //trexpcerti.Visible = false;
        //trsecdoc3.Visible = false;
        //trsecdoc4.Visible = false;

        string connStr =
            ConfigurationManager.ConnectionStrings["EConnectContext"]
            .ConnectionString;

        string sql = @"
        SELECT COUNT(1)
        FROM MercyCase_UploadedDocs
        WHERE candidate_id = @CandidateID
          AND Request_Status_Id = 4";

        using (SqlConnection con = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(sql, con))
        {
            cmd.Parameters.Add("@CandidateID", SqlDbType.BigInt).Value = candidateId;

            con.Open();

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }
    }

    private void SetApplicantType(int applicantTypeId)
    {
        bool isInstitute = (applicantTypeId == 2);

        // Keep the radio button in sync
        RdoUndergngDOEACC.SelectedValue = isInstitute ? "I" : "D";

        // Show/Hide institute details
        TrStateCenterAccno.Visible = isInstitute;
        TrCenterNameAccno.Visible = isInstitute;
        TrInstitutedetails.Visible = isInstitute;

        // Enable/Disable controls (if required)
        DdlAccState.Enabled = !isInstitute;
        DdlAccCentre.Enabled = !isInstitute;

        //if (isInstitute)
        //{
        //    trexpcerti.Visible = false;
        //    //lblDocNo1.Text = "8.1";
        //    //lblDocNo3.Text = "8.2";
        //}
        //else
        //{
        //    trexpcerti.Visible = true;
        //    //lblDocNo1.Text = "8.1";
        //    //lblDocNo2.Text = "8.2";
        //    //lblDocNo3.Text = "8.3";
        //}

        ViewState["ApplicantTypeID"] = applicantTypeId;
    }

    private int GetApplicantType(long candidateId, int courseId)
    {
        string query = "SELECT TOP 1 Applicant_Type_ID FROM Registration_Detail WHERE Candidate_ID=@ID AND Course_ID=@Course_ID";

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ID", candidateId);
                cmd.Parameters.AddWithValue("@Course_ID", courseId);

                con.Open();

                object result = cmd.ExecuteScalar();

                return (result != null) ? Convert.ToInt32(result) : 1; // Default to Direct
            }
        }
    }

    // For Declaration

    //private void bindDeclaration(Int32 applicantType, int agecount)
    //{
    //    try
    //    {

    //        if (agecount == -1)
    //        {
    //            //LblGuard.Text = LblFName.Text;
    //            //LblHGuard.Text = LblFName.Text;
    //            //lblname.Text = LblAppName.Text;
    //            //LblHName.Text = LblAppName.Text;
    //            tddeclaration1.Style.Add("display", "none");
    //            //tddeclaration2.Style.Add("display", "block");
    //        }
    //        else
    //        {
    //            tddeclaration1.Style.Add("display", "block");
    //           // tddeclaration2.Style.Add("display", "none");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblerror.Text = ex.Message;
    //        lblerror.Visible = true;
    //        ShowAlert(ex.Message);
    //    }
    //}

    public bool BindMercyData(long candidateId, int courseId, string mercyRegType, bool isReUpload = false)
    { 
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("sp_get_MercyData", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", candidateId);
                cmd.Parameters.AddWithValue("@Course_ID", courseId);
                cmd.Parameters.AddWithValue("@MercyRegType", mercyRegType);
                // cmd.Parameters.AddWithValue("@Applicant_Type_ID", applicant_type_id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (isReUpload)
                    {
                        LblCourse.Text = dr["Course_Name"].ToString();
                        lblRegistrationNo.Text = dr["Registration_No"].ToString();
                        lblreupappliname.Text = dr["Name"].ToString();
                        lblreupapplidob.Text = Convert.ToDateTime(dr["Dob"]).ToString("dd/MM/yyyy");
                        return true;
                    }
                    // 🔸 Applicant Details
                    LblAppName.Text = dr["Name"].ToString();
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

                    hdnMaritalStatusID.Value = dr["Marital_Status_ID"].ToString();
                    hdnCategoryID.Value = dr["Cast_Category_ID"].ToString();
                    hdnReligionID.Value = dr["Religion_ID"].ToString();
                    hdnExServiceManID.Value = dr["Is_Ex_Servicemane_ID"].ToString();
                    hdnIsHandicapedID.Value = dr["Is_Handicaped_ID"].ToString();
                    // 🔸 Religion
                    LblReligion.Text = dr["Religion_Name"].ToString();

                    // 🔸 Contact Details
                    LblSTD.Text = dr["Std"].ToString();
                    LblPhone.Text = dr["Phone"].ToString();
                    //LblLandLine.Text = dr["Phone"].ToString();
                    lblMobile.Text = dr["Mobile"].ToString();
                    lblEmail.Text = dr["Email"].ToString();

                    // 🔸correspondence Address
                    lblCorAddressLine1.Text = dr["Corr_Address1"].ToString();
                    lblCorAddressLine2.Text = dr["Corr_Address2"].ToString();
                    lblCorAddressLine3.Text = dr["Corr_Address3"].ToString();
                    lblCorCity.Text = dr["Corr_City"].ToString();
                  //  ddlCorState.SelectedValue = dr["State_ID"].ToString();

                    ddlCorState.SelectedValue = dr["Corr_State_ID"].ToString();
                    bindCorespondDistrict(Convert.ToInt32(ddlCorState.SelectedValue));
                    ddldistrict.SelectedValue = dr["Corr_District_ID"].ToString();

                  //  ddlPdistrict.SelectedItem.Text = dr["Per_District_Name"].ToString();
                    //if (DdlAccState.SelectedValue != "0")
                    //{
                    //    BindAccCentre(Convert.ToInt32(DdlAccState.SelectedValue), courseId);
                    //}
                    lblPincode.Text = dr["Corr_Pin"].ToString();
                    DdlAccState.SelectedValue = dr["State_ID"].ToString();
                    DdlAccState_SelectedIndexChanged(DdlAccState, EventArgs.Empty);
                    //  DdlAccCentre.SelectedItem.Text = dr["Institute_Accreditation"].ToString();
                    DdlAccCentre.SelectedValue = dr["ID"].ToString();

                    // 🔸Permanent Address
                    lblPerAddressLine1.Text = dr["Per_Address1"].ToString();
                    lblPerAddressLine2.Text = dr["Per_Address2"].ToString();
                    lblPerAddressLine3.Text = dr["Per_Address3"].ToString();
                    lblPerCity.Text = dr["Per_City"].ToString();
                    // ddlPState.SelectedValue = dr["State_ID"].ToString();
                    ddlPState.SelectedValue = dr["Per_State_ID"].ToString();
                    bindDistrict(Convert.ToInt64(ddlPState.SelectedValue), ref ddlPdistrict);
                    ddlPdistrict.SelectedValue = dr["Per_District_ID"].ToString();

                   
                 //   ddldistrict.SelectedItem.Text = dr["Corr_District_Name"].ToString();
                    LblPerPinCode.Text = dr["Per_Pin"].ToString();

                    // 🔸 Course & Institute
                    LblCourse.Text = dr["Course_Name"].ToString();
                 //   LblInstitute.Text = dr["Institute_Name"].ToString();
                 //   LblAccNo.Text = dr["Accreditation_Number"].ToString();
                    lblRegistrationNo.Text = dr["Registration_No"].ToString();
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
                    if (dr["Apaar_ID"] != DBNull.Value && !string.IsNullOrEmpty(dr["Apaar_ID"].ToString()))
                    {
                        trapaar.Visible = false;
                    }
                    else
                    {
                        trapaar.Visible = true;
                    }
                    return true;
                }
                return false;
                //ddlCorState.SelectedValue = dr["State_ID"].ToString();
                //int idacc = Convert.ToInt32(DdlAccState.SelectedValue);
                //  ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                //  DdlAccCentre.SelectedValue = Convert.ToString(instituteDetail.CentreID);

                con.Close();
            }
        }
    }

    protected void bindState(int courseId)
    {
       // Int32 courseID=0;// Convert.ToInt32(Request.QueryString["id"]);
        Int32 withdrawlid = Convert.ToInt32(enmAccreditationStatus.Withdrawal);
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ddlCorState.Items.Clear();
                DdlAccState.Items.Clear();
                ddlPState.Items.Clear();
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                System.Web.UI.WebControls.ListItem lst1 = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                System.Web.UI.WebControls.ListItem lst2 = new System.Web.UI.WebControls.ListItem("--Select One--", "0");

                var CorState = (from s in context.Locations
                                orderby (s.Name)
                                where s.LocationTypeID == 2
                                && s.ParentLocationID == 1
                                select new { ValueField = s.ID, TextField = s.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCorState, CorState, lst);

                var AccState = (from s in context.Locations
                                join i in context.Institutes on s.ID equals i.StateID
                                join a in context.AccreditationDetails on i.ID equals a.InstituteID
                                orderby (s.Name)
                                where s.LocationTypeID == 2
                                && s.ParentLocationID == 1
                                && a.CourseID == courseId
                                && a.AccreditationStatusID != withdrawlid
                                select new { ValueField = s.ID, TextField = s.Name }).Distinct().ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlAccState, AccState, lst1);

                var PState = (from s in context.Locations
                              orderby (s.Name)
                              where s.LocationTypeID == 2
                              && s.ParentLocationID == 1
                              select new { ValueField = s.ID, TextField = s.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlPState, PState, lst2);
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindDistrict(long stateID, ref DropDownList ddl)
    {
        try
        {
            int locationTypeID = Convert.ToInt32(enmLocationType.District);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var district = from s in context.Locations
                               orderby (s.Name)
                               where s.LocationTypeID == locationTypeID
                               && s.ParentLocationID == stateID
                               select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, district.ToList(), lst);
                if (ddl.Items.Count == 0)
                    ddl.Items.Add(lst);
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void bindCorespondDistrict(int id)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (id != null & id != 0)
                {
                    var district = from s in context.Locations
                                   orderby (s.Name)
                                   where s.LocationTypeID == 4
                                   && s.ParentLocationID == id
                                   select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddldistrict, district, lst);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlPState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id2 = Convert.ToInt32(ddlPState.SelectedValue);
            bindDistrict(id2, ref ddlPdistrict);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void ddlCorState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id1 = Convert.ToInt32(ddlCorState.SelectedValue);
            ddldistrict.Items.Clear();
            bindDistrict(id1, ref ddldistrict);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void DdlAccState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            stateid = Convert.ToInt32(DdlAccState.SelectedValue);
            // Int32 courseId = 1;
            int courseId = Convert.ToInt32(Session["CourseID"]);
            BindAccCentre(stateid, courseId);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void DdlAccCentre_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DdlAccCentre.SelectedValue != "0")
            AlertModalPopUp.Show();
    }

    protected void BindAccCentre(int stateid, int courseId)
    {
        try
        {
            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
            Int32 withdrawlid = Convert.ToInt32(enmAccreditationStatus.Withdrawal);
            Int32 rejectedid = Convert.ToInt16(enmAccreditationStatus.Rejected);
            Int32 deferredid = Convert.ToInt16(enmAccreditationStatus.Deferred);
            Int32 acknowledgeid = Convert.ToInt16(enmAccreditationStatus.Acknowledged);
            using (var context = new EConnectContext())
            {
                var AccCentre = (from i in context.Institutes
                                 join d in context.AccreditationDetails on i.ID equals d.InstituteID
                                 orderby i.Name
                                 where i.StateID == stateid
                                 && d.CourseID == courseId
                                 && d.AccreditationStatusID != withdrawlid
                 && d.AccreditationStatusID != rejectedid
                                     && d.AccreditationStatusID != deferredid
                                       && d.AccreditationStatusID != acknowledgeid
                                 //Added 22 May 2020 for instt blocking
                                 && d.tempBlocked == false
                                 && (d.BlockedFromDate >= System.DateTime.Now || d.BlockedFromDate == null)
                                 //
                                 select new { ValueField = i.ID, TextField = d.AccreditationNumber + " - " + i.Name + ", " + (!string.IsNullOrEmpty(i.CityName) ? i.CityName : "") }).ToList();
                DdlAccCentre.Items.Clear();
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlAccCentre, AccCentre.Distinct(), lst);
//                ScriptManager.RegisterStartupScript(this, GetType(), "msg",
//"alert('Centres: " + AccCentre.Count + "');", true);
            }
            UpdatePanel2.Update();
            ;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    //protected void UploadPhoto_Click(object sender, EventArgs e)
    //{
    //    if (ImgUpload.HasFile)
    //    {
    //        string fileNamePhoto = "";
    //        photoPreview.ImageUrl = "";
    //        lblphotoShow.Text = "";
    //        lblerror.Text = "";
    //        string error = "";
    //        string status = "";
    //        string message = "";
    //        int applicantTypeId = GetApplicantType(candidateId, courseId);
    //        SetApplicantType(applicantTypeId);
    //        SetConsentRelationVisibility();
    //        try
    //        {
    //            //16.02.2022
    //            if (ImgUpload.FileName.Length > 49)
    //            {
    //                lblphotoShow.Visible = true;
    //                lblerror.Visible = true;
    //                photoPreview.ImageUrl = "";
    //                lblphotoShow.Text = "Photo file name should be less than 44 characters.";
    //                lblerror.Text = "Photo file name should be less than 44 characters.";
    //                if (File.Exists(fileNamePhoto))
    //                { File.Delete(fileNamePhoto); }
    //                ImgUpload.PostedFile.InputStream.Dispose();
    //                Session["ImgUpload"] = null;
    //                throw new Exception("Photo file name should be less than 44 characters.");
    //            }
    //            if (!System.Text.RegularExpressions.Regex.IsMatch(Convert.ToString(ImgUpload.FileName), "^[a-zA-Z0-9()-_.\u00FC\u00DC ]*$"))
    //            {
    //                lblphotoShow.Visible = true;
    //                lblerror.Visible = true;
    //                photoPreview.ImageUrl = "";
    //                lblphotoShow.Text = "Photo file name should be English Alphabets and Numbers only(like a-zA-Z0-9).";
    //                lblerror.Text = "Photo file name should be English Alphabets and Numbers only(like a-zA-Z0-9).";
    //                if (File.Exists(fileNamePhoto))
    //                { File.Delete(fileNamePhoto); }
    //                ImgUpload.PostedFile.InputStream.Dispose();
    //                Session["ImgUpload"] = null;
    //                throw new Exception("Photo file name should be English Alphabets and Numbers only(like a-zA-Z0-9).");
    //            }
    //            if (!isvalidFileExtension1(ImgUpload))
    //            {
    //                lblphotoShow.Visible = true;
    //                lblerror.Visible = true;
    //                photoPreview.ImageUrl = "";
    //                lblphotoShow.Text = "Only jpeg/jpg extensions image is allowed.";
    //                lblerror.Text = "Only jpeg/jpg extensions image is allowed.";
    //                if (File.Exists(fileNamePhoto))
    //                { File.Delete(fileNamePhoto); }
    //                ImgUpload.PostedFile.InputStream.Dispose();
    //                Session["ImgUpload"] = null;
    //                throw new Exception("Only jpeg/jpg extensions image is allowed.");
    //            }
    //            if (!isvalidFileSize1(ImgUpload, 5120, 51200))
    //            {
    //                lblphotoShow.Visible = true;
    //                lblerror.Visible = true;
    //                photoPreview.ImageUrl = "";
    //                lblphotoShow.Text = "Photo file size should be 5 KB to 50 KB.";
    //                lblerror.Text = "Photo file size should be 5 KB to 50 KB.";
    //                if (File.Exists(fileNamePhoto))
    //                { File.Delete(fileNamePhoto); }
    //                ImgUpload.PostedFile.InputStream.Dispose();
    //                Session["ImgUpload"] = null;
    //                throw new Exception("Photo file size should be 5 KB to 50 KB.");
    //            }
    //            //16.02.2022

    //            string requestURL = "https://uat.nielit.in/nielitImageApi/v1/index.php/ImageValidate";//"http://127.0.0.1:8088/index.php/ImageValidate";  // live URL of API
                                                                                                         

    //            string folderPath = Server.MapPath("~/ImageUpload/");
    //            if (!Directory.Exists(folderPath))
    //            { Directory.CreateDirectory(folderPath); }
    //            string ImgNamePhoto = string.Empty;
    //            ImgNamePhoto = "P" + Path.GetFileName(ImgUpload.FileName);          //Photo_
    //            ImgUpload.SaveAs(folderPath + "/" + ImgNamePhoto);
    //            fileNamePhoto = Server.MapPath("~/ImageUpload/" + ImgNamePhoto);

    //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

    //            try
    //            {
    //                WebClient wc = new WebClient();
    //                byte[] bytess = wc.DownloadData(fileNamePhoto);
    //                Dictionary<string, object> postParameters = new Dictionary<string, object>();
    //                postParameters.Add("image", new FormUpload.FileParameter(bytess, Path.GetFileName(fileNamePhoto), "image/jpeg"));
    //                string userAgent = "Someone";
    //                HttpWebResponse imageApiResponse = FormUpload.MultipartFormPost(requestURL, userAgent, postParameters, "Authorization", "Test@123");
    //                StreamReader apiResponseReader = new StreamReader(imageApiResponse.GetResponseStream());
    //                lblphotoShow.Text = apiResponseReader.ReadToEnd();
    //                DataTable messageTable = FormUpload.JSONToDataTable(lblphotoShow.Text);
    //                if (messageTable.Rows.Count > 0)
    //                {
    //                    for (int i = 0; i < messageTable.Rows.Count; i++)
    //                    {
    //                        error = messageTable.Rows[i].Field<string>("error");
    //                        status = messageTable.Rows[i].Field<string>("status");
    //                        message = messageTable.Rows[i].Field<string>("message");
    //                    }
    //                    lblphotoShow.Text = message;
    //                }
    //                imageApiResponse.Close();
    //            }
    //            catch (Exception ex)
    //            {
    //                message = "Invalid Photo/Image File.";
    //                lblphotoShow.Visible = true;
    //                lblerror.Visible = true;
    //                photoPreview.ImageUrl = "";
    //                lblphotoShow.Text = message;
    //                lblerror.Text = message;
    //                if (File.Exists(fileNamePhoto))
    //                { File.Delete(fileNamePhoto); }
    //                ImgUpload.PostedFile.InputStream.Dispose();
    //                Session["ImgUpload"] = null;
    //                throw new Exception(message);
    //            }
    //            if (status == "true" && message == "Image validated successfully")
    //            {
    //                string ImgNamePhoto1 = string.Empty;
    //                ImgNamePhoto1 = "P" + Path.GetFileName(ImgUpload.FileName);          //Photo_
    //                photoPreview.ImageUrl = "";
    //                photoPreview.ImageUrl = "~/ImageUpload/" + ImgNamePhoto1;
    //                ImgUpload.ResolveUrl(photoPreview.ImageUrl);
    //                lblphotoShow.Visible = true;
    //                lblphotoShow.Text = "Photo successfully Uploaded.";
    //                //  Session["ImgUpload"] = ImgUpload;
    //                Session["ImgUpload"] = ImgUpload.FileBytes;       // store bytes
    //                Session["PhotoFileName"] = ImgUpload.FileName;    // store name
    //            }
    //            else
    //            {
    //                lblphotoShow.Visible = true;
    //                lblerror.Visible = true;
    //                photoPreview.ImageUrl = "";
    //                lblphotoShow.Text = message;
    //                lblerror.Text = message;
    //                if (File.Exists(fileNamePhoto))
    //                { File.Delete(fileNamePhoto); }
    //                ImgUpload.PostedFile.InputStream.Dispose();
    //                Session["ImgUpload"] = null;
    //                throw new Exception(message);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            if (File.Exists(fileNamePhoto))
    //            { File.Delete(fileNamePhoto); }
    //            lblerror.Text = ex.Message;
    //            lblerror.Visible = true;
    //            ImgUpload.PostedFile.InputStream.Dispose();
    //            Session["ImgUpload"] = null;
    //            ShowAlert(ex.Message);
    //        }
    //    }
    //}

    //protected void btnNotification_Click1(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        divPopup.Style.Add("display", "block");
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}

    //protected void btnOKD_Click(object sender, EventArgs e)
    //{
    //    //ActiveInActive(true);
    //    divPopup.Style.Add("display", "none");
    //}

    //protected void UploadSignature_Click(object sender, EventArgs e)
    //{
    //    if (ImgUploadSignature.HasFile)
    //    {
    //        string fileName = "";
    //        signPreview.ImageUrl = "";
    //        lblsignShow.Text = "";
    //        lblerror.Text = "";
    //        string status = "";
    //        string message = "";
    //        int applicantTypeId = GetApplicantType(candidateId, courseId);
    //        SetApplicantType(applicantTypeId);
    //        SetConsentRelationVisibility();
    //        try
    //        {
    //            //16.02.2022
    //            if (ImgUploadSignature.FileName.Length > 49)
    //            {
    //                lblsignShow.Visible = true;
    //                lblerror.Visible = true;
    //                signPreview.ImageUrl = "";
    //                lblsignShow.Text = "Signature file name should be less than 44 characters.";
    //                lblerror.Text = "Signature file name should be less than 44 characters.";
    //                if (File.Exists(fileName))
    //                { File.Delete(fileName); }
    //                ImgUploadSignature.PostedFile.InputStream.Dispose();
    //                Session["ImgUploadSignature"] = null;
    //                throw new Exception("Signature file name should be less than 44 characters.");
    //            }
    //            if (!System.Text.RegularExpressions.Regex.IsMatch(Convert.ToString(ImgUploadSignature.FileName), "^[a-zA-Z0-9()-_.\u00FC\u00DC ]*$"))
    //            {
    //                lblsignShow.Visible = true;
    //                lblerror.Visible = true;
    //                signPreview.ImageUrl = "";
    //                lblsignShow.Text = "Signature file name should be English Alphabets and Numbers only(like a-zA-Z0-9).";
    //                lblerror.Text = "Signature file name should be English Alphabets and Numbers only(like a-zA-Z0-9).";
    //                if (File.Exists(fileName))
    //                { File.Delete(fileName); }
    //                ImgUploadSignature.PostedFile.InputStream.Dispose();
    //                Session["ImgUploadSignature"] = null;
    //                throw new Exception("Signature file name should be English Alphabets and Numbers only(like a-zA-Z0-9).");
    //            }
    //            if (!isvalidFileExtension1(ImgUploadSignature))
    //            {
    //                lblsignShow.Visible = true;
    //                lblerror.Visible = true;
    //                signPreview.ImageUrl = "";
    //                lblsignShow.Text = "Only jpeg/jpg extensions image is allowed";
    //                lblerror.Text = "Only jpeg/jpg extensions image is allowed";
    //                if (File.Exists(fileName))
    //                { File.Delete(fileName); }
    //                ImgUploadSignature.PostedFile.InputStream.Dispose();
    //                Session["ImgUploadSignature"] = null;
    //                throw new Exception("Only jpeg/jpg extensions image is allowed");
    //            }
    //            if (!isvalidFileSize1(ImgUploadSignature, 5120, 20480))
    //            {
    //                lblsignShow.Visible = true;
    //                lblerror.Visible = true;
    //                signPreview.ImageUrl = "";
    //                lblsignShow.Text = "Signature file size should be 5 KB to 20 KB.";
    //                lblerror.Text = "Signature file size should be 5 KB to 20 KB.";
    //                if (File.Exists(fileName))
    //                { File.Delete(fileName); }
    //                ImgUploadSignature.PostedFile.InputStream.Dispose();
    //                Session["ImgUploadSignature"] = null;
    //                throw new Exception("Signature file size should be 5 KB to 20 KB.");
    //            }
    //            //16.02.2022
    //            // live URL of API
    //          //  string requestURL = "http://127.0.0.1:8088/index.php/ThumbSignatureValidate";  // live URL of API
    //            string requestURL = "https://uat.nielit.in/nielitImageApi/v1/index.php/ThumbSignatureValidate";  // live URL of API
               

    //            string folderPath = Server.MapPath("~/ImageUpload/");
    //            if (!Directory.Exists(folderPath))
    //            { Directory.CreateDirectory(folderPath); }
    //            string ImgNameSig = string.Empty;
    //            ImgNameSig = "S" + Path.GetFileName(ImgUploadSignature.FileName);         //Sig_
    //            ImgUploadSignature.SaveAs(folderPath + "/" + ImgNameSig);
    //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    //            fileName = Server.MapPath("~/ImageUpload/" + ImgNameSig);
    //            try
    //            {
    //                WebClient wc = new WebClient();
    //                byte[] bytess = wc.DownloadData(fileName);
    //                Dictionary<string, object> postParameters = new Dictionary<string, object>();
    //                postParameters.Add("image", new FormUpload.FileParameter(bytess, Path.GetFileName(fileName), "image/jpeg"));
    //                string userAgent = "Someone";
    //                HttpWebResponse imageApiResponse = FormUpload.MultipartFormPost(requestURL, userAgent, postParameters, "Authorization", "Test@123");
    //                StreamReader apiResponseReader = new StreamReader(imageApiResponse.GetResponseStream());
    //                lblsignShow.Text = apiResponseReader.ReadToEnd();
    //                DataTable messageTable = FormUpload.JSONToDataTable(lblsignShow.Text);
    //                if (messageTable.Rows.Count > 0)
    //                {
    //                    for (int i = 0; i < messageTable.Rows.Count; i++)
    //                    {
    //                        status = messageTable.Rows[i].Field<string>("status");
    //                        message = messageTable.Rows[i].Field<string>("message");
    //                    }
    //                    lblsignShow.Text = message;
    //                }
    //                imageApiResponse.Close();
    //            }
    //            catch (Exception ex)
    //            {
    //                message = "Invalid Signature/Image File.";
    //                lblsignShow.Visible = true;
    //                lblerror.Visible = true;
    //                signPreview.ImageUrl = "";
    //                lblsignShow.Text = message;
    //                lblerror.Text = message;
    //                if (File.Exists(fileName))
    //                { File.Delete(fileName); }
    //                ImgUploadSignature.PostedFile.InputStream.Dispose();
    //                Session["ImgUploadSignature"] = null;
    //                throw new Exception(message);
    //            }

    //            if (status == "true" && message == "Image validated successfully")
    //            {
    //                string ImgNamesig1 = string.Empty;
    //                ImgNamesig1 = "S" + Path.GetFileName(ImgUploadSignature.FileName);         //Sig_
    //                signPreview.ImageUrl = "";
    //                signPreview.ImageUrl = "~/ImageUpload/" + ImgNamesig1;
    //                ImgUploadSignature.ResolveUrl(signPreview.ImageUrl);
    //                lblsignShow.Visible = true;
    //                lblsignShow.Text = "Signature successfully Uploaded.";
    //                //  Session["ImgUploadSignature"] = ImgUploadSignature;
    //                Session["ImgUploadSignature"] = ImgUploadSignature.FileBytes;       // store bytes
    //                Session["signFileName"] = ImgUploadSignature.FileName;    // store name
    //            }
    //            else
    //            {
    //                lblsignShow.Visible = true;
    //                lblerror.Visible = true;
    //                signPreview.ImageUrl = "";
    //                lblsignShow.Text = message;
    //                lblerror.Text = message;
    //                if (File.Exists(fileName))
    //                { File.Delete(fileName); }
    //                ImgUploadSignature.PostedFile.InputStream.Dispose();
    //                Session["ImgUploadSignature"] = null;
    //                throw new Exception(message);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            if (File.Exists(fileName))
    //            { File.Delete(fileName); }
    //            lblerror.Text = ex.Message;
    //            lblerror.Visible = true;
    //            ImgUploadSignature.PostedFile.InputStream.Dispose();
    //            Session["ImgUploadSignature"] = null;
    //            ShowAlert(ex.Message);
    //        }
    //    }
    //}   
    
    protected void Btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //if (IsValidForm())
            //{
            //  long id = 885810;
            candidateId = Convert.ToInt64(Session["EntityID"]);
            hdnCandidateID.Value = candidateId.ToString();
            // int courseId = 1;
            int courseId = Convert.ToInt32(Session["CourseID"]);
            //if (Request.QueryString["ReUpload"] == "1")
            //{
            //ReUploadHeldDocuments();
            //return;
            //  }
             bool isReUpload = Request.QueryString["ReUpload"] == "1";

        if (isReUpload)
        {
            ReUploadHeldDocuments();

            // Reload page after successful re-upload
          //  Response.Redirect("MercyCaseRegistration.aspx");
            return;
        }

            long apaarRequestId = 0;
                bool isApaarRequired = trapaar.Visible;
                if (isApaarRequired)
                {
                    // For Apaar
                    //Validation of apaar start
                    string gender = LblGender.Text.Substring(0, 1).ToUpper();//ddl_gender.SelectedValue.Substring(0, 1).ToUpper();
                   
                    string validatedApaarData = validateApaar.ConvertApaarDatatoJSONandEncrypt(txtapaar.Text, LblAppName.Text, LblDob.Text, gender, txtproviderName.Text, ddlAuthMode.SelectedItem.Text, txtAuthenticationIdNo.Text, ddlConsentRelation.SelectedItem.Text, txtConsentPlace.Text, lblApaarDeclaration.Text, out apaarRequestId);


                    if (String.IsNullOrWhiteSpace(validatedApaarData))
                    {
                        //txtcode.Text = "";
                        //GenerateNewCaptchaImage();
                        ShowAlert("Apaar could not be validated.");
                        return;
                       // throw new Exception("Apaar could not be validated.");
                    }
                    if (String.IsNullOrWhiteSpace(txtapaar.Text))
                    {
                        //txtcode.Text = "";
                        //GenerateNewCaptchaImage();
                        ShowAlert("Please enter Apaar ID");
                        return;
                        // throw new Exception("Apaar could not be validated.");
                    }
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtapaar.Text, @"^\d{12}$"))
                    {
                        ShowAlert("APAAR ID must contain exactly 12 digits.");                       
                        return;
                    }

                    if (chkApaarDeclaration.Checked != true)
                    {
                        ShowAlert("Please check the Apaar Declaration Statement");
                        return;
                       // throw new Exception("Please check the Apaar Declaration Statement");
                    }
                    if (txtapaar.Text.Length != 12)
                    {
                        txtapaar.Text = "";
                        txtapaar.Focus();
                        ShowAlert("Invalid ApaarID.");
                        return;
                       // throw new Exception("Invalid ApaarID.");
                    }
                    if (String.IsNullOrWhiteSpace(txtConsentPlace.Text))
                    {
                        //txtcode.Text = "";
                        //GenerateNewCaptchaImage();
                        ShowAlert("Please enter Consent Place of Apaar");
                        return;
                        // throw new Exception("Apaar could not be validated.");
                    }

                    JObject apaarObj = JObject.Parse(validatedApaarData);

                    string status =
                      apaarObj["status"] != null
                       ? apaarObj["status"].ToString().Trim().ToLower()
                         : "";

                    string statusCode =
                        apaarObj["status_code"] != null
                        ? apaarObj["status_code"].ToString().Trim()
                        : "";

                    string messageCode =
                        apaarObj["message_code"] != null
                        ? apaarObj["message_code"].ToString().Trim()
                        : "";

                    string message =
                        apaarObj["message"] != null
                        ? apaarObj["message"].ToString().Trim()
                        : "";


                    using (EConnectContext db = new EConnectContext())
                    {
                        apaarResponseRecd responseObj = new apaarResponseRecd();

                        // ApaarID
                        responseObj.abc_account_id = txtapaar.Text;
                        if (apaarObj["message"].ToString() != "Records not found")
                        {
                            responseObj.abc_account_id = txtapaar.Text;
                            // cname
                            responseObj.cname =
                                apaarObj["CNAME"] != null
                                ? apaarObj["CNAME"].ToString()
                                : "";

                            // gender
                            if (apaarObj["GENDER"] != null)
                            {
                                string g =
                                    apaarObj["GENDER"]
                                    .ToString()
                                    .Trim()
                                    .ToUpper();

                                if (g == "M")
                                    responseObj.genderID = 1;

                                else if (g == "F")
                                    responseObj.genderID = 2;

                                else if (g == "T")
                                    responseObj.genderID = 3;
                            }

                            if (apaarObj["DOB"] != null)
                            {
                                DateTime parsedDob;
                                DateTime enteredDob;

                                if (
                                    DateTime.TryParse(apaarObj["DOB"].ToString(), out parsedDob)
                                    &&
                                    DateTime.TryParse(LblDob.Text, out enteredDob)
                                )
                                {
                                    // Compare complete date (day/month/year)
                                    if (parsedDob.Date == enteredDob.Date)
                                    {
                                        responseObj.dob = parsedDob;

                                    }
                                    else
                                    {
                                        // throw new Exception("DOB does not match with APAAR record.");
                                        ShowAlert("DOB does not match with APAAR record.");
                                        return;
                                    }
                                }
                                else
                                {
                                    // throw new Exception("Invalid DOB format.");
                                    ShowAlert("Invalid DOB format.");
                                    return;
                                }

                            }
                        }
                        else
                        {
                            responseObj.status = false;
                            //bit values insert data according to api rsponse
                            responseObj.nameMatch = false;
                            responseObj.birthYearMatch = false;
                            responseObj.genderMatch = false;
                        }
                        // status insert , db takes bit value
                        string apiStatus =
                            apaarObj["status"] != null
                            ? apaarObj["status"].ToString().Trim().ToLower()
                            : "";

                        if (apiStatus == "success")
                        {
                            responseObj.status = true;
                            //bit values insert data according to api rsponse
                            responseObj.nameMatch = true;
                            responseObj.birthYearMatch = true;
                            responseObj.genderMatch = true;
                        }
                        else
                        {
                            responseObj.status = false;
                            if (apaarObj["message"].ToString() == "Records not found")
                            {
                                responseObj.status = false;
                                //bit values insert data according to api rsponse
                                responseObj.nameMatch = false;
                                responseObj.birthYearMatch = false;
                                responseObj.genderMatch = false;
                            }
                            else
                            {
                                //bit values insert data according to api rsponse
                                if (apaarObj["match_data_status"] != null)
                                {
                                    JObject matchObj =
                                        (JObject)apaarObj["match_data_status"];


                                    if (matchObj["student_name_match"] != null)
                                    {
                                        responseObj.nameMatch =
                                            Convert.ToBoolean(
                                                matchObj["student_name_match"].ToString()
                                            );
                                    }

                                    if (matchObj["year_of_birth"] != null)
                                    {
                                        responseObj.birthYearMatch =
                                            Convert.ToBoolean(
                                                matchObj["year_of_birth"].ToString()
                                            );
                                    }

                                    if (matchObj["gender_match"] != null)
                                    {
                                        responseObj.genderMatch =
                                            Convert.ToBoolean(
                                                matchObj["gender_match"].ToString()
                                            );
                                    }
                                }
                            }
                        }

                        // status code 
                        responseObj.statuscode =
                            apaarObj["status_code"] != null
                            ? apaarObj["status_code"].ToString()
                            : "";

                        // status code 
                        responseObj.status_code =
                            apaarObj["status_code"] != null
                            ? apaarObj["status_code"].ToString()
                            : "";

                        // message
                        responseObj.message =
                            apaarObj["message"] != null
                            ? apaarObj["message"].ToString()
                            : "";

                        // full json response
                        responseObj.responseContent =
                            validatedApaarData;

                        // ENTER BY
                        responseObj.enterByID = 99;

                        // enter date
                        responseObj.enterDate =
                            DateTime.Now;


                        // messageCode
                        responseObj.messageCode =
                            apaarObj["message_code"] != null
                            ? apaarObj["message_code"].ToString()
                            : "";


                        // apaar requestID
                        responseObj.apaarReqID =
                            apaarRequestId;

                        db.apaarResponseRecd.Add(responseObj);
                        db.SaveChanges();
                    }

                    if (message == "Records not found")
                        throw new Exception(message);

                    if (status == "fail")
                    {
                        //txtcode.Text = "";
                        //GenerateNewCaptchaImage();
                        // mismatch validations

                        JObject matchObj =
                            (JObject)apaarObj["match_data_status"];

                        bool nameMatch =
                            matchObj["student_name_match"] != null
                            ? Convert.ToBoolean(
                                matchObj["student_name_match"].ToString()
                              )
                            : false;

                        bool dobMatch =
                            matchObj["year_of_birth"] != null
                            ? Convert.ToBoolean(
                                matchObj["year_of_birth"].ToString()
                              )
                            : false;

                        bool genderMatch =
                            matchObj["gender_match"] != null
                            ? Convert.ToBoolean(
                                matchObj["gender_match"].ToString()
                              )
                            : false;

                        if (!nameMatch)
                        {
                            throw new Exception(
                                "APAAR validation failed : Name does not match."
                            );
                        }

                        if (!dobMatch)
                        {
                            throw new Exception(
                                "APAAR validation failed : Date of Birth does not match."
                            );
                        }

                        if (!genderMatch)
                        {
                            throw new Exception(
                                "APAAR validation failed : Gender does not match."
                            );
                        }

                        throw new Exception(message);
                    }



                    //else
                    //{
                    //    txtcode.Text = "";
                    //    GenerateNewCaptchaImage();
                    //    throw new Exception("Apaar could not be validated, Try again");
                    //}

                    //Check for duplicate Apaar for CandidateId and CourseId
                   
                    string apaarEncryptedCheck = EncryptDecrypt.EncryptString(txtapaar.Text);
                    // Int64 applId = Convert.ToInt64(Request.QueryString["Appid"]);
                    EConnectContext context = new EConnectContext();
                //  Course currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["id"]));

                // Int32 CourseId = 1;//currentCourse.ID;
                // Int32 ExamId = Convert.ToInt32(ViewState["ExamID"]);
                int CourseId = Convert.ToInt32(Session["CourseID"]);
                string duplicate = checkDuplicateApaar(apaarEncryptedCheck, CourseId, candidateId);

                    //if (duplicate != "0" && duplicate != "-99")
                    //{
                    //    if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                    //    {
                    //        Int64 applId1 = Convert.ToInt64(Request.QueryString["Appid"]);
                    //        //This Query will get all the information of Applied Canditate by generated Application id
                    //        CourseRegistrationApplication oldApplication = context.CourseRegistrationApplications.Find(applId1);
                    //        if (oldApplication.Number != duplicate)
                    //            throw new Exception("Duplicate application for the exam cycle not allowed, Check status for " + duplicate.ToString() + " Application");
                    //    }
                    //    else
                    //        throw new Exception("Duplicate application for the exam cycle not allowed, Check status for " + duplicate.ToString() + " Application");

                    //}
                    if (duplicate != "0" && duplicate != "-99")
                    {
                        ShowAlert("Duplicate APAAR ID already exists. Application No: " + duplicate);
                        //throw new Exception(
                        //    "Duplicate APAAR ID already exists. Application No: "
                        //    + duplicate
                        //);
                    }
                }

              
              
                string apaarEncrypted = "";

                if (trapaar.Visible)
                {
                    if (!String.IsNullOrEmpty(txtapaar.Text))
                    {
                        apaarEncrypted = EncryptDecrypt.EncryptString(txtapaar.Text);
                    }
                    else
                    {
                        ShowAlert("Enter Apaar");
                        return;
                    }
                }
            // Apaar End
            if (!IsValidForm())
                return;

            // long id = Convert.ToInt64(hdnCandidateID.Value);
            string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                //   Int64 enterBy = Convert.ToInt64(Session["UserID"]);
                string ApplicantFname = LblAppName.Text.Trim();
                string Fname = LblFName.Text.Trim();
                string Mname = LblMName.Text.Trim();
                string Guardianname = LblGuardianName.Text.Trim();
                string Gender = LblGender.Text.Trim();
                string MaritalStatus = LblMaritalStatus.Text.Trim();
                //  string DOB = LblDob.Text.Trim();
                DateTime dob = DateTime.ParseExact(LblDob.Text.Trim(),"dd-MM-yyyy",CultureInfo.InvariantCulture);
                string Category = LblCategory.Text.Trim();
                string Handicaped = LblHandicapped.Text.Trim();
                string ExServiceman = LblExService.Text.Trim();
                string Religion = LblReligion.Text.Trim();
                string LandlineStd = LblSTD.Text.Trim();
                string LandlinePhone = LblPhone.Text.Trim();
                string Per_address1 = lblPerAddressLine1.Text.Trim();
                string Per_address2 = lblPerAddressLine2.Text.Trim();
                string Per_address3 = lblPerAddressLine3.Text.Trim();
                string Per_city = lblPerCity.Text.Trim();
                string Per_State = ddlPState.SelectedValue;
                string Per_District = ddlPdistrict.SelectedValue;
                string Per_PinCoe = LblPerPinCode.Text.Trim();
                string Registration_No = lblRegistrationNo.Text.Trim();
                Int32 Exam_Id = Convert.ToInt32(ViewState["ExamID"]);
              //  ViewState["ExamID"] = examID.ToString();
                // Editable Fields
                string Corr_address1 = lblCorAddressLine1.Text.Trim();
                string Corr_address2 = lblCorAddressLine2.Text.Trim();
                string Corr_address3 = lblCorAddressLine3.Text.Trim();
                string Corr_city = lblCorCity.Text.Trim();
                string Corr_state = ddlCorState.SelectedValue;
                string Corr_District = ddldistrict.SelectedValue;
                string Corr_PinCode = lblPincode.Text.Trim();
                string MobileNo = lblMobile.Text.Trim();
                string Email = lblEmail.Text.Trim();
            //  string InstituteId = DdlAccCentre.SelectedValue;         
            //  string accNo = DdlAccCentre.SelectedItem.Text.Split(new string[] { " - " }, StringSplitOptions.None)[0];
            Int32 applicant_type_id = Convert.ToInt32(ViewState["ApplicantTypeID"]);
            string InstituteId = null;
            string accNo = null;

            if (RdoUndergngDOEACC.SelectedValue == "I")   // Through Institute
            {
                InstituteId = DdlAccCentre.SelectedValue;

                if (DdlAccCentre.SelectedIndex > 0)
                {
                    accNo = DdlAccCentre.SelectedItem.Text.Split(new string[] { " - " }, StringSplitOptions.None)[0];
                }
            }
            // Session["ImgUpload"] = ImgUpload;
            byte[] ImgUpload = null;
                string photoFileName = null;

                byte[] ImgUploadSignature = null;
                string signFileName = null;           


                if (Session["ImgUpload"] != null)
                {
                    ImgUpload = (byte[])Session["ImgUpload"];
                    // photoFileName = Session["PhotoFileName"]?.ToString();
                    if (Session["PhotoFileName"] != null)
                    {
                        photoFileName = Session["PhotoFileName"].ToString();
                    }
                    else
                    {
                        photoFileName = null;
                    }
                }

                if (Session["ImgUploadSignature"] != null)
                {
                    ImgUploadSignature = (byte[])Session["ImgUploadSignature"];
                    // signFileName = Session["SignFileName"]?.ToString();
                    if (Session["SignFileName"] != null)
                    {
                        signFileName = Session["SignFileName"].ToString();
                    }
                    else
                    {
                        signFileName = null;
                    }
                }
              
                using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("sp_Insert_MercyApplication", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Candidate_ID", candidateId);
                cmd.Parameters.AddWithValue("@Course_ID", courseId);
                cmd.Parameters.AddWithValue("@Cor_Address1", Corr_address1);
                cmd.Parameters.AddWithValue("@Cor_Address2", Corr_address2);
                cmd.Parameters.AddWithValue("@Cor_Address3", Corr_address3);
                cmd.Parameters.AddWithValue("@Cor_City_Name", Corr_city);
                cmd.Parameters.AddWithValue("@Cor_State_ID", Corr_state);
                cmd.Parameters.AddWithValue("@Cor_District_ID", Corr_District);
                cmd.Parameters.AddWithValue("@Cor_Pin_Code", Corr_PinCode);
                cmd.Parameters.AddWithValue("@Mobile", MobileNo);
                cmd.Parameters.AddWithValue("@Email", Email);
                //  cmd.Parameters.AddWithValue("@Institute_ID", InstituteId);
                cmd.Parameters.AddWithValue("@Institute_ID", string.IsNullOrEmpty(InstituteId) ? (object)DBNull.Value : InstituteId);
                cmd.Parameters.AddWithValue("@Photo_File_Name", (object)photoFileName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Apaar_ID", apaarEncrypted);   // Apaar Id
                cmd.Parameters.AddWithValue("@ApaarRequestID", apaarRequestId);   // Apaar Request Id

                //Labels
                cmd.Parameters.AddWithValue("@Name", ApplicantFname);
                cmd.Parameters.AddWithValue("@Father_Name", Fname);
                cmd.Parameters.AddWithValue("@Mother_Name", Mname);
                cmd.Parameters.AddWithValue("@Guardian_Name", Guardianname);
                cmd.Parameters.AddWithValue("@Gender", Gender);
                cmd.Parameters.AddWithValue("@Marital_Status_ID", Convert.ToInt32(hdnMaritalStatusID.Value));
                //  cmd.Parameters.AddWithValue("@Dob", DOB);
                cmd.Parameters.Add("@Dob", SqlDbType.DateTime).Value = dob;
                cmd.Parameters.AddWithValue("@Cast_Category_ID", Convert.ToInt32(hdnCategoryID.Value));
                cmd.Parameters.AddWithValue("@Is_Handicaped", Convert.ToBoolean(hdnIsHandicapedID.Value));
                cmd.Parameters.AddWithValue("@Is_Ex_Servicemane", Convert.ToBoolean(hdnExServiceManID.Value));
                cmd.Parameters.AddWithValue("@Religion_ID", Convert.ToInt32(hdnReligionID.Value));
                cmd.Parameters.AddWithValue("@Std", LandlineStd);
                cmd.Parameters.AddWithValue("@Phone", LandlinePhone);
                cmd.Parameters.AddWithValue("@Per_Address1", Per_address1);
                cmd.Parameters.AddWithValue("@Per_Address2", Per_address2);
                cmd.Parameters.AddWithValue("@Per_Address3", Per_address3);
                cmd.Parameters.AddWithValue("@Per_City_Name", Per_city);
                cmd.Parameters.AddWithValue("@Per_State_ID", Per_State);
                cmd.Parameters.AddWithValue("@Per_District_ID", Per_District);
                cmd.Parameters.AddWithValue("@Per_Pin_Code", Per_PinCoe);
                cmd.Parameters.AddWithValue("@Registered_Course_Registration_No", Registration_No);
                cmd.Parameters.AddWithValue("@Exam_ID", Exam_Id);
                cmd.Parameters.AddWithValue("@Applicant_Type_Id", applicant_type_id);
                //if (ImgUpload != null)
                //    cmd.Parameters.Add("@Photo", SqlDbType.VarBinary).Value = ImgUpload;
                //else
                //    cmd.Parameters.Add("@Photo", SqlDbType.VarBinary).Value = DBNull.Value;

                //cmd.Parameters.AddWithValue("@Signature_File_Name", (object)signFileName ?? DBNull.Value);

                //if (ImgUploadSignature != null)
                //    cmd.Parameters.Add("@Signature", SqlDbType.VarBinary).Value = ImgUploadSignature;
                //else
                //    cmd.Parameters.Add("@Signature", SqlDbType.VarBinary).Value = DBNull.Value;
                // For Documnets
                //if (fuEduquacertiForm.HasFile)
                //{
                //    InsertDocument(candidateId, fuEduquacertiForm, "EDUCATION CERTIFICATE");
                //}
                //if (fuexpcertiForm.HasFile)
                //{
                //    InsertDocument(candidateId, fuexpcertiForm, "EXPERIENCE CERTIFICATE");
                //}
                if (Fumedical.HasFile)
                {
                    InsertDocument(candidateId, Fumedical, "PROOF OF MEDICAL ISSUE/AFFIDAVIT");
                }
                //if (FuIdCard.HasFile)
                //{
                //    InsertDocument(candidateId, FuIdCard, "ID CARD");
                //}
                byte[] eduBytes = null;

                if (Session["EduFile"] != null)
                {
                    eduBytes = (byte[])Session["EduFile"];
                }

                con.Open();
                cmd.ExecuteNonQuery();
                //  if ((!String.IsNullOrEmpty(Request.QueryString["id"])) && (!String.IsNullOrEmpty(Request.QueryString["courseId"])) && (!String.IsNullOrEmpty(Request.QueryString["applicant_type_id"])))
                //  {
                //  Int32 applicant_type_id = Convert.ToInt32(ViewState["ApplicantTypeID"]);
                string ExamCycle = Convert.ToString(ViewState["ExamName"]);
                string ExamFee = Convert.ToString(ViewState["defaultFeeAmount"]);
                string accState = "";
                string accId = "";

                if (RdoUndergngDOEACC.SelectedValue == "I")
                {
                    accState = DdlAccState.SelectedValue;
                    accId = DdlAccCentre.SelectedValue;

                    if (DdlAccCentre.SelectedIndex > 0)
                    {
                        accNo = DdlAccCentre.SelectedItem.Text.Split(new string[] { " - " }, StringSplitOptions.None)[0];
                    }
                }
                string appliedAs = RdoUndergngDOEACC.SelectedValue;
                Response.Redirect("../CAND/SpecialReRegistrationPreview.aspx?id=" + candidateId
                        + "&courseId=" + courseId
                        + "&applicant_type_id=" + applicant_type_id
                        + "&CorrDistrictID=" + ddldistrict.SelectedValue + "&AccStateID=" + accState
        + "&AccID=" + accId
        + "&AccNo=" + accNo + "&CorrStateID=" + ddlCorState.SelectedValue + "&ExamCycle=" + LblExamName.Text.ToString() + "&ExamFee=" + ExamFee + "&PerDistrictID=" + ddlPdistrict.SelectedValue + "&AppliedAs=" + appliedAs);


            }

        }

        catch (SqlException ex)
        {
            ShowAlert(ex.Message, true);
        }

    }

    string checkDuplicateApaar(string apaarEncryptedCheck, int CourseId, long id)
    {
        string duplicate = "0";
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var candRecord = context.CourseRegistrationApplications
     .Where(c => c.CourseID == CourseId
              && c.apaarID == apaarEncryptedCheck
              && c.CandidateID != id)
     .Select(c => new { c.Number })
     .FirstOrDefault();
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

    protected void Btnupdate_Click(object sender, EventArgs e)
    {
        try
        {          

            //lblcoraddline1.Text = "";
            //if (string.IsNullOrEmpty(txtCorAddressLine1.Text))
            //{
            //    lblcoraddline1.Visible = true;
            //    lblcoraddline1.Text = "Address Line1 can not be left blank";
            //    return;
            //}

            //lblcoraddline2.Text = "";
            //if (string.IsNullOrEmpty(txtCorAddressLine2.Text))
            //{
            //    lblcoraddline2.Visible = true;
            //    lblcoraddline2.Text = "Address Line2 can not be left blank";
            //    return;
            //}

            //lblcoraddline3.Text = "";
            //if (string.IsNullOrEmpty(txtCorAddressLine3.Text))
            //{
            //    lblcoraddline3.Visible = true;
            //    lblcoraddline3.Text = "Address Line3 can not be left blank";
            //    return;
            //}

            //lblcorcity.Text = "";
            //if (string.IsNullOrEmpty(txtCorCity.Text))
            //{
            //    lblcorcity.Visible = true;
            //    lblcorcity.Text = "City Name can not be left blank";
            //    return;
            //}

            //lblmobile.Text = "";
            //if (string.IsNullOrEmpty(txtMobile.Text))
            //{
            //    lblmobile.Visible = true;
            //    lblmobile.Text = "Mobile Number can not be left blank";
            //    return;
            //}

            //lblEmail.Text = "";
            //if (string.IsNullOrEmpty(lblEmail.Text))
            //{
            //    lblEmail.Visible = true;
            //    lblEmail.Text = "Email Address can not be left blank";
            //    return;
            //}           

            //lblPincode.Text = "";
            //if (string.IsNullOrEmpty(lblPincode.Text))
            //{
            //    lblPincode.Visible = true;
            //    lblPincode.Text = "Pincode can not be left blank";
            //    return;
            //}

            // long id = 885810;
            candidateId = Convert.ToInt64(Session["EntityID"]);
            hdnCandidateID.Value = candidateId.ToString();
                int courseId = 1;

                // long id = Convert.ToInt64(hdnCandidateID.Value);
                string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                //   Int64 enterBy = Convert.ToInt64(Session["UserID"]);
                string Corr_address1 = lblCorAddressLine1.Text.Trim();
                string Corr_address2 = lblCorAddressLine2.Text.Trim();
                string Corr_address3 = lblCorAddressLine3.Text.Trim();
                string Corr_city = lblCorCity.Text.Trim();
                string Corr_state = ddlCorState.SelectedValue;
                string Corr_District = ddldistrict.SelectedValue;
                string Corr_PinCode = lblPincode.Text.Trim();
                string MobileNo = lblMobile.Text.Trim();
                string Email = lblEmail.Text.Trim();
            // string InstituteId = DdlAccCentre.SelectedValue;             
            // string accNo = DdlAccCentre.SelectedItem.Text.Split(new string[] { " - " }, StringSplitOptions.None)[0];
            string InstituteId = null;
            string accNo = null;

            if (RdoUndergngDOEACC.SelectedValue == "I")   // Through Institute
            {
                InstituteId = DdlAccCentre.SelectedValue;

                if (DdlAccCentre.SelectedIndex > 0)
                {
                    accNo = DdlAccCentre.SelectedItem.Text.Split(new string[] { " - " }, StringSplitOptions.None)[0];
                }
            }
            // Session["ImgUpload"] = ImgUpload;
            //byte[] ImgUpload = null;
            //    string photoFileName = null;

            //    byte[] ImgUploadSignature = null;
            //    string signFileName = null;            

            //    if (Session["ImgUpload"] != null)
            //    {
            //        ImgUpload = (byte[])Session["ImgUpload"];
            //        // photoFileName = Session["PhotoFileName"]?.ToString();
            //        if (Session["PhotoFileName"] != null)
            //        {
            //            photoFileName = Session["PhotoFileName"].ToString();
            //        }
            //        else
            //        {
            //            photoFileName = null;
            //        }
            //    }

            //    if (Session["ImgUploadSignature"] != null)
            //    {
            //        ImgUploadSignature = (byte[])Session["ImgUploadSignature"];
            //        // signFileName = Session["SignFileName"]?.ToString();
            //        if (Session["SignFileName"] != null)
            //        {
            //            signFileName = Session["SignFileName"].ToString();
            //        }
            //        else
            //        {
            //            signFileName = null;
            //        }
            //    }

                using (SqlConnection con = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand("sp_Update_MercyApplication", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Candidate_ID", candidateId);
                    cmd.Parameters.AddWithValue("@Course_ID", courseId);
                    cmd.Parameters.AddWithValue("@Cor_Address1", Corr_address1);
                    cmd.Parameters.AddWithValue("@Cor_Address2", Corr_address2);
                    cmd.Parameters.AddWithValue("@Cor_Address3", Corr_address3);
                    cmd.Parameters.AddWithValue("@Cor_City_Name", Corr_city);
                    cmd.Parameters.AddWithValue("@Cor_State_ID", Corr_state);
                    cmd.Parameters.AddWithValue("@Cor_District_ID", Corr_District);
                    cmd.Parameters.AddWithValue("@Cor_Pin_Code", Corr_PinCode);
                    cmd.Parameters.AddWithValue("@Mobile", MobileNo);
                    cmd.Parameters.AddWithValue("@Email", Email);
                 //   cmd.Parameters.AddWithValue("@Institute_ID", InstituteId);
                cmd.Parameters.AddWithValue("@Institute_ID", string.IsNullOrEmpty(InstituteId) ? (object)DBNull.Value : InstituteId);
              //  cmd.Parameters.AddWithValue("@Photo_File_Name", (object)photoFileName ?? DBNull.Value);
                  //  cmd.Parameters.AddWithValue("@Apaar_ID", apaarEncrypted);   // Apaar Id
                    //if (ImgUpload != null)
                    //    cmd.Parameters.Add("@Photo", SqlDbType.VarBinary).Value = ImgUpload;
                    //else
                    //    cmd.Parameters.Add("@Photo", SqlDbType.VarBinary).Value = DBNull.Value;

                    //cmd.Parameters.AddWithValue("@Signature_File_Name", (object)signFileName ?? DBNull.Value);

                    //if (ImgUploadSignature != null)
                    //    cmd.Parameters.Add("@Signature", SqlDbType.VarBinary).Value = ImgUploadSignature;
                    //else
                    //    cmd.Parameters.Add("@Signature", SqlDbType.VarBinary).Value = DBNull.Value;
                    // For Documnets
                    //if (fuEduquacertiForm.HasFile)
                    //{
                    //    InsertDocument(candidateId, fuEduquacertiForm, "EDUCATION CERTIFICATE");
                    //}
                    //if (fuexpcertiForm.HasFile)
                    //{
                    //    InsertDocument(candidateId, fuexpcertiForm, "EXPERIENCE CERTIFICATE");
                    //}
                    if (Fumedical.HasFile)
                    {
                        InsertDocument(candidateId, Fumedical, "PROOF OF MEDICAL ISSUE/AFFIDAVIT");
                    }
                    //if (FuIdCard.HasFile)
                    //{
                    //    InsertDocument(candidateId, FuIdCard, "ID CARD");
                    //}
                byte[] eduBytes = null;

                    if (Session["EduFile"] != null)
                    {
                        eduBytes = (byte[])Session["EduFile"];
                    }

                    con.Open();
                    cmd.ExecuteNonQuery();
                //  if ((!String.IsNullOrEmpty(Request.QueryString["id"])) && (!String.IsNullOrEmpty(Request.QueryString["courseId"])) && (!String.IsNullOrEmpty(Request.QueryString["applicant_type_id"])))
                //  {
                Int32 applicant_type_id = Convert.ToInt32(ViewState["ApplicantTypeID"]);
                string ExamCycle = Convert.ToString(ViewState["ExamName"]);
                string ExamFee = Convert.ToString(ViewState["defaultFeeAmount"]);
                string accState = "";
                string accId = "";

                if (RdoUndergngDOEACC.SelectedValue == "I")
                {
                    accState = DdlAccState.SelectedValue;
                    accId = DdlAccCentre.SelectedValue;

                    if (DdlAccCentre.SelectedIndex > 0)
                    {
                        accNo = DdlAccCentre.SelectedItem.Text.Split(new string[] { " - " }, StringSplitOptions.None)[0];
                    }
                }
                string appliedAs = RdoUndergngDOEACC.SelectedValue;
                Response.Redirect("../CAND/SpecialReRegistrationPreview.aspx?id=" + candidateId
                        + "&courseId=" + courseId
                        + "&applicant_type_id=" + applicant_type_id + "&AccStateID=" + accState
        + "&AccID=" + accId
        + "&AccNo=" + accNo + "&CorrStateID=" + ddlCorState.SelectedValue + "&CorrDistrictID=" + ddldistrict.SelectedValue + "&ExamCycle=" + LblExamName.Text.ToString() + "&ExamFee=" + ExamFee + "&AppliedAs=" + appliedAs);                //  }
                                                                                           //else
                                                                                           //{
                                                                                           //    Response.Redirect("../CAND/MercyCaseRegistration.aspx?id=" + id + "&courseId=" + courseId + "&applicant_type_id=" + applicant_type_id + "");
                                                                                           //}

                }

         //   }

        }

        catch (SqlException ex)
        {
            ShowAlert(ex.Message, true);
        }

    }

    private void InsertDocument(long id, FileUpload fu, string docType)
    {
        string fileName = Path.GetFileName(fu.FileName);
        string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand("sp_SaveMercyDocument", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@candidate_id", id);
            cmd.Parameters.AddWithValue("@Original_File_Name", Path.GetFileName(fu.FileName));
            cmd.Parameters.AddWithValue("@Extension", Path.GetExtension(fileName));
            cmd.Parameters.Add("@Uploaded_File", SqlDbType.VarBinary).Value = fu.FileBytes;
            cmd.Parameters.AddWithValue("@Doc_Type", docType);
            cmd.Parameters.AddWithValue("@CourseID", CourseID);
            SqlParameter outputMsg = new SqlParameter("@ResultMessage", SqlDbType.VarChar, 100);
            outputMsg.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(outputMsg);
            con.Open();
            cmd.ExecuteNonQuery();
            //cmd.ExecuteNonQuery();

            string message = outputMsg.Value.ToString();
            lbldocument.Text = message;

            //con.Open();
            //cmd.ExecuteNonQuery();
        }
    }

    private void SetReUploadMode(bool isReUpload)
    {       
        if (isReUpload)
        {
            // Re-upload heading
            lblMainHeading.InnerText = "DOCUMENT RE-UPLOAD FORM";
            lblRegFCourse.InnerText = "Course / पाठ्यक्रम";

            trreupappliname.Visible = true;
            trreupapplidob.Visible=true;
            // Hide other sections
            trpreview.Visible = false;
            trlblnote.Visible = false;
            TrPreRegLevel1.Visible = false;
            trapplidet.Visible = false;
            trapplifn.Visible = false;
            TrFatherName.Visible = false;
            TrMotherName.Visible = false;
            trguardian.Visible = false;
            trgender.Visible = false;
            trdobct.Visible = false;
            trctsr.Visible = false;
            trrelg.Visible = false;
            trcnt.Visible = false;
            trphmob.Visible = false;
            tremail.Visible = false;
            trper.Visible = false;
            trperadd1.Visible = false;
            trperadd2.Visible = false;
            trperadd3.Visible = false;
            trpercity.Visible = false;
            trperstate.Visible = false;
            trpercorr.Visible = false;
            trcorradd1.Visible = false;
            trcorradd2.Visible = false;
            trcorradd3.Visible = false;
            trcorrcity.Visible = false;
            trcorrstate.Visible = false;
            TrApplicantType.Visible = false;
            TrInstitutedetails.Visible = false;
            TrStateCenterAccno.Visible = false;
            TrCenterNameAccno.Visible = false;
            trIDenti.Visible = false;
            trapaar.Visible = false;
           // tridentif2.Visible = false;
          //  tridentf3.Visible = false;


            // Keep Documents visible
            trsecdoc.Visible = true;
            trsecdoc1.Visible = false;
            //trexpcerti.Visible = false;
            //trsecdoc3.Visible = false;
            //trsecdoc4.Visible = false;
            tddeclaration1.Style.Add("display", "none");
            chkdisclamier.Visible = false;
            // Show only required held documents
            LoadHeldDocumentSection(candidateId);
        }
        else
        {
            lblMainHeading.InnerText = "SPECIAL RE REGISTRATION FORM";
            trreupappliname.Visible = false;
            trreupapplidob.Visible = false;
            trpreview.Visible = true;
            trlblnote.Visible = true;
            TrPreRegLevel1.Visible = true;
            trapplidet.Visible = true;
            trapplifn.Visible = true;
            TrFatherName.Visible = true;
            TrMotherName.Visible = true;
            trguardian.Visible = true;
            trgender.Visible = true;
            trdobct.Visible = true;
            trctsr.Visible = true;
            trrelg.Visible = true;
            trcnt.Visible = true;
            trphmob.Visible = true;
            tremail.Visible = true;
            trper.Visible = true;
            trperadd1.Visible = true;
            trperadd2.Visible = true;
            trperadd3.Visible = true;
            trpercity.Visible = true;
            trperstate.Visible = true;
            trpercorr.Visible = true;
            trcorradd1.Visible = true;
            trcorradd2.Visible = true;
            trcorradd3.Visible = true;
            trcorrcity.Visible = true;
            trcorrstate.Visible = true;
            TrApplicantType.Visible = true;
            TrInstitutedetails.Visible = true;
            TrStateCenterAccno.Visible = true;
            TrCenterNameAccno.Visible = true;
            trIDenti.Visible = true;
            trapaar.Visible = true;
          //  tridentif2.Visible = true;
          //  tridentf3.Visible = true;         
            trsecdoc.Visible = true;
            trsecdoc1.Visible = true;
            //trexpcerti.Visible = true;
            //trsecdoc3.Visible = true;
            //trsecdoc4.Visible = true;
            chkdisclamier.Visible = true;
            tddeclaration1.Style.Add("display", "block");
            int applicantTypeId = GetApplicantType(candidateId, courseId);
            SetApplicantType(applicantTypeId);
            SetConsentRelationVisibility();
        }
       
    }

    private void LoadHeldDocumentSection(long candidateId)
    {
        DataTable dt = GetHeldDocuments(candidateId);

        // Initially hide everything
      //  trsecdoc.Visible = false;
        trsecdoc1.Visible = false;
        //trexpcerti.Visible = false;
        //trsecdoc3.Visible = false;
        //trsecdoc4.Visible = false;

        if (dt.Rows.Count == 0)
        {
            // phDocumentSection.Visible = false;
            trsecdoc.Visible = false;
            trsecdoc1.Visible = false;
            //trexpcerti.Visible = false;
            //trsecdoc3.Visible = false;
            //trsecdoc4.Visible = false;
            return;
        }
        trsecdoc.Visible = true;
        Btnsubmit.Visible = true;
        int docNumber = 1;
        //trsecdoc1.Visible = true;
        //trexpcerti.Visible = true;
        //trsecdoc3.Visible = true;
        //trsecdoc4.Visible = true;

        // phDocumentSection.Visible = true;

        foreach (DataRow row in dt.Rows)
        {
            string docType = row["Doc_Type"].ToString();

            //if (docType == "EDUCATION CERTIFICATE")
            //{
            //    // trEducation.Visible = true;
            //    trsecdoc1.Visible = true;
            //    lblDocNo1.Text = docNumber.ToString();

            //    lblEduStatus.Text =
            //        "Document is on hold. Please re-upload.";
            //    docNumber++;
            //}
            //else if (docType == "EXPERIENCE CERTIFICATE")
            //{
            //    trexpcerti.Visible = true;
            //    lblDocNo2.Text = docNumber.ToString();

            //    lblExpCertiFormStatus.Text =
            //        "Document is on hold. Please re-upload.";
            //    docNumber++;
            //}
            if (docType == "PROOF OF MEDICAL ISSUE/AFFIDAVIT")
            {
                trsecdoc1.Visible = true;
                lblDocNo1.Text = docNumber.ToString();

                lblMedicalStatus.Text =
                    "Document is on hold. Please re-upload.";
                docNumber++;
            }
            //else if (docType == "ID CARD")
            //{
            //    trsecdoc4.Visible = true;
            //    lblDocNo4.Text = docNumber.ToString();

            //    lblIDCardStatus.Text =
            //        "Document is on hold. Please re-upload.";
            //    docNumber++;
            //}
        }
    }

    private DataTable GetHeldDocuments(long candidateId)
    {
        DataTable dt = new DataTable();

        string connStr =
            ConfigurationManager.ConnectionStrings["EConnectContext"]
            .ConnectionString;

        string sql = @"
        SELECT
            ID,
            candidate_id,
            Doc_Type,
            Remarks,
            Request_Status_Id
        FROM MercyCase_UploadedDocs
        WHERE candidate_id = @CandidateID
          AND Request_Status_Id = 4";

        using (SqlConnection con = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(sql, con))
        {
            cmd.Parameters.Add("@CandidateID", SqlDbType.BigInt)
                          .Value = candidateId;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dt);
        }

        return dt;
    }

    // For Reupload Documents

    //private void ReUploadHeldDocuments()
    //{
    //    bool uploaded = false;

    //    if (fuEduquacertiForm.HasFile)
    //    {
    //        ReUploadDocument(
    //            fuEduquacertiForm,
    //            "EDUCATION CERTIFICATE");

    //        uploaded = true;
    //    }

    //    if (fuexpcertiForm.HasFile)
    //    {
    //        ReUploadDocument(
    //            fuexpcertiForm,
    //            "EXPERIENCE CERTIFICATE");

    //        uploaded = true;
    //    }

    //    if (Fumedical.HasFile)
    //    {
    //        ReUploadDocument(
    //            Fumedical,
    //            "PROOF OF MEDICAL ISSUE");

    //        uploaded = true;
    //    }

    //    if (FuIdCard.HasFile)
    //    {
    //        ReUploadDocument(
    //            FuIdCard,
    //            "ID CARD");

    //        uploaded = true;
    //    }

    //    if (!uploaded)
    //    {
    //        ShowAlert("Please upload at least one document.", true);
    //        return;
    //    }

    //    ShowAlert("Document re-uploaded successfully.", false);
    //}

    private void ReUploadHeldDocuments()
    {
        bool uploaded = false;

        //if (fuEduquacertiForm.HasFile)
        //{
        //    if (!ValidateReUploadDocument(
        //       fuEduquacertiForm,
        //       lblEduStatus,
        //       "Education Form"))
        //    {
        //        return;
        //    }

        //    string message = ReUploadDocument(
        //        fuEduquacertiForm,
        //        "EDUCATION CERTIFICATE");

        //    lblEduStatus.Text = message;
        //    uploaded = true;
        //}

        //if (fuexpcertiForm.HasFile)
        //{
        //    if (!ValidateReUploadDocument(
        //       fuexpcertiForm,
        //       lblExpCertiFormStatus,
        //       "Experience Certificate"))
        //    {
        //        return;
        //    }

        //    string message = ReUploadDocument(
        //        fuexpcertiForm,
        //        "EXPERIENCE CERTIFICATE");

        //    lblExpCertiFormStatus.Text = message;
        //    uploaded = true;
        //}

        if (Fumedical.HasFile)
        {
            if (!ValidateReUploadDocument(
               Fumedical,
               lblMedicalStatus,
               "Medical Certificate"))
            {
                return;
            }

            string message = ReUploadDocument(
                Fumedical,
                "PROOF OF MEDICAL ISSUE");

            lblMedicalStatus.Text = message;
            uploaded = true;
        }

        //if (FuIdCard.HasFile)
        //{
        //    if (!ValidateReUploadDocument(
        //       FuIdCard,
        //       lblIDCardStatus,
        //       "ID Card"))
        //    {
        //        return;
        //    }

        //    string message = ReUploadDocument(
        //        FuIdCard,
        //        "ID CARD");

        //    lblIDCardStatus.Text = message;
        //    uploaded = true;
        //}

        if (!uploaded)
        {
            ShowAlert("Please upload at least one document.", true);
            return;
        }
        //ShowAlert("Document re-uploaded successfully.", false);
        //Response.Redirect("../Index.aspx");
        ScriptManager.RegisterStartupScript(this,GetType(),"ReUploadSuccess","alert('Documents uploaded successfully.'); window.location.href='../Index.aspx';",true);
        return;
    }

    private string ReUploadDocument(FileUpload fileUpload, string docType)
    {
        if (fileUpload == null || !fileUpload.HasFile)
            return "Please select a document.";
        string connStr =
            ConfigurationManager.ConnectionStrings["EConnectContext"]
            .ConnectionString;

        byte[] fileBytes = fileUpload.FileBytes;

        using (SqlConnection con = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand(
            "sp_ReUploadMercyDocument", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@candidate_id", SqlDbType.BigInt)
                .Value = candidateId;

            cmd.Parameters.Add("@Original_File_Name", SqlDbType.VarChar, 255)
                .Value = fileUpload.FileName;

            cmd.Parameters.Add("@Extension", SqlDbType.VarChar, 10)
                .Value = Path.GetExtension(fileUpload.FileName);

            cmd.Parameters.Add("@Uploaded_File", SqlDbType.VarBinary, -1)
                .Value = fileBytes;

            cmd.Parameters.Add("@Doc_Type", SqlDbType.VarChar, 50)
                .Value = docType;

            SqlParameter resultParam =
                cmd.Parameters.Add(
                    "@ResultMessage",
                    SqlDbType.VarChar,
                    100);

            resultParam.Direction = ParameterDirection.Output;

            con.Open();

            cmd.ExecuteNonQuery();

            //string resultMessage =
            //    resultParam.Value.ToString();

            //if (resultMessage != "Document re-uploaded successfully")
            //{
            //    throw new Exception(resultMessage);
            //}
            string resultMessage = resultParam.Value == DBNull.Value
         ? "Document upload completed."
         : resultParam.Value.ToString();

            return resultMessage;
        }
    }

    private bool ValidateReUploadDocument(FileUpload fileUpload,Label statusLabel,string documentName)
    {
        // No new file selected
        if (!fileUpload.HasFile)
        {
            statusLabel.ForeColor = System.Drawing.Color.Red;
            statusLabel.Text = "Please upload " + documentName + ".";
            return false;
        }

        // Validate extension
        string extension =
            Path.GetExtension(fileUpload.FileName).ToLower();

        if (extension != ".pdf")
        {
            statusLabel.ForeColor = System.Drawing.Color.Red;
            statusLabel.Text = "Only PDF file allowed.";
            return false;
        }

        // Validate file size - 1 MB
        if (fileUpload.PostedFile.ContentLength > 1048576)
        {
            statusLabel.ForeColor = System.Drawing.Color.Red;
            statusLabel.Text = "File size should not exceed 1 MB.";
            return false;
        }

        // Validation successful
        statusLabel.ForeColor = System.Drawing.Color.Green;
        statusLabel.Text =
            fileUpload.FileName + " is ready to upload.";

        return true;
    }

    //protected void UploadEdu_Click(object sender, EventArgs e)
    //{
    //    if (fuEduquacertiForm.HasFile)
    //    {
    //        Session["EduFile"] = fuEduquacertiForm.FileBytes;
    //        Session["EduFileName"] = fuEduquacertiForm.FileName;

    //     //   lblStatus.Text = "Education file uploaded";
    //    }
    //}

    protected void UpdateData()
    {
        try
        {
            long Id = Convert.ToInt64(Session["EntityID"]);//Convert.ToInt64(hdnCandidateID.Value);
            int CourseId =Convert.ToInt16(Session["CourseID"]);//1;      

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_get_MercyDataUpdated", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID", Id);
                    cmd.Parameters.AddWithValue("@Course_ID", CourseId);
                    // cmd.Parameters.AddWithValue("@Applicant_Type_ID", applicant_type_id);

                    con.Open();

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        // 🔸 Applicant Details
                        LblAppName.Text = dr["Name"].ToString();
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
                        LblReligion.Text = dr["Religion_Code"].ToString();

                        // 🔸 Contact Details
                        LblSTD.Text = dr["Std"].ToString();
                        LblPhone.Text = dr["Phone"].ToString();
                        lblMobile.Text = dr["Mobile"].ToString();
                        lblEmail.Text = dr["Email"].ToString();

                        // 🔸correspondence Address
                        lblCorAddressLine1.Text = dr["Corr_Address1"].ToString();
                        lblCorAddressLine2.Text = dr["Corr_Address2"].ToString();
                        lblCorAddressLine3.Text = dr["Corr_Address3"].ToString();
                        lblCorCity.Text = dr["Corr_City"].ToString();
                        //  ddlCorState.SelectedValue = dr["State_ID"].ToString();

                        // ddlCorState.SelectedValue = dr["Corr_State_ID"].ToString();
                        string qsCorrStateID = Request.QueryString["CorrStateID"];
                        if (!string.IsNullOrEmpty(qsCorrStateID))
                        {
                            ddlCorState.SelectedValue = qsCorrStateID;                        
                        }
                        else
                        {                           
                            ddlCorState.SelectedValue = dr["Corr_State_ID"].ToString();
                        }

                        ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);

                        string qsCorrDistrictID = Request.QueryString["CorrDistrictID"];
                        if (!string.IsNullOrEmpty(qsCorrDistrictID))
                        {
                            ddldistrict.SelectedValue = qsCorrDistrictID;
                        }
                        else
                        {
                            ddldistrict.SelectedValue = dr["Corr_District_ID"].ToString();
                        }
                        lblPincode.Text = dr["Corr_Pin"].ToString();
                       
                        string qsStateID = Request.QueryString["AccStateID"];
                        string qsAccID = Request.QueryString["AccID"];

                        // Apaar

                        string encryptedApaar = Convert.ToString(dr["Apaar_ID"]);
                        string apaarPlainText = EncryptDecrypt.DecryptString(encryptedApaar);
                        EConnectContext context = new EConnectContext();
                        var apaarRequest = context.ApaarRequest
                            .Where(x => x.apaarId == apaarPlainText)
                            .OrderByDescending(x => x.ID)
                            .FirstOrDefault();

                        txtapaar.Text = apaarPlainText;
                        if (!string.IsNullOrEmpty(txtapaar.Text))
                        {
                            trapaar.Visible = true;
                        }
                        else
                        {
                            trapaar.Visible = false;
                        }
                        txtproviderName.Text = apaarRequest.providerName;
                        txtIsProviderPresent.Text = "True";//apaarRequest.authModeID.ToString();
                        txtAuthenticationIdNo.Text = apaarRequest.authModeIDNo.ToString();
                        chkApaarDeclaration.Checked = true;
                        chkdisclamier.Checked = true;
                        //  ddlConsentRelation.SelectedValue = apaarRequest.consentRelation;
                        if (!string.IsNullOrEmpty(apaarRequest.consentRelation))
                        {
                            switch (apaarRequest.consentRelation.Trim().ToLower())
                            {
                                case "self":
                                    ddlConsentRelation.SelectedValue = "1";
                                    break;

                                case "guardian":
                                    ddlConsentRelation.SelectedValue = "2";
                                    break;

                                case "father":
                                    ddlConsentRelation.SelectedValue = "3";
                                    break;

                                case "mother":
                                    ddlConsentRelation.SelectedValue = "4";
                                    break;
                            }
                        }
                        ddlConsentRelation_SelectedIndexChanged(ddlConsentRelation, EventArgs.Empty);
                        txtConsentPlace.Text = apaarRequest.consentPlace;
                        txtConsentDate.Text = apaarRequest.consentDate.Value.ToString("dd/MM/yyyy");
                        txtConsentTime.Text = apaarRequest.consentTime.Value.ToString(@"hh\:mm");
                        DateTime todaydate = DateTime.Now;
                        DateTime inputdate = Convert.ToDateTime(LblDob.Text);

                        int countAge =
                            DateTime.Compare(todaydate.AddYears(-18), inputdate);

                        BindAuthMode(countAge);
                        ddlAuthMode.SelectedValue = Convert.ToString(apaarRequest.authModeID);
                        ddlAuthMode_SelectedIndexChanged(ddlAuthMode,EventArgs.Empty);

                        txtproviderName.Enabled = false;
                        txtIsProviderPresent.Enabled = false;
                        txtAuthenticationIdNo.Enabled = false;
                        ddlConsentRelation.Enabled=false;
                        txtConsentPlace.Enabled = false;
                        txtConsentDate.Enabled = false;
                        txtConsentTime.Enabled = false;
                        txtapaar.Enabled = false;
                        tdapaartext.Attributes.Add("style", "pointer-events:none;");
                        ddlAuthMode.Enabled = false;
                        chkApaarDeclaration.Enabled = false;
                        chkdisclamier.Enabled = false;

                        if (!string.IsNullOrEmpty(qsStateID))
                        {
                            DdlAccState.SelectedValue = qsStateID;

                            DdlAccState_SelectedIndexChanged(DdlAccState, EventArgs.Empty);

                            if (!string.IsNullOrEmpty(qsAccID))
                            {
                                DdlAccCentre.SelectedValue = qsAccID;
                            }
                        }
                        else
                        {
                            // Default DB binding
                            DdlAccState.SelectedValue = dr["State_ID"].ToString();

                            DdlAccState_SelectedIndexChanged(DdlAccState, EventArgs.Empty);

                            DdlAccCentre.SelectedItem.Text = dr["Institute_Accreditation"].ToString();
                        }
                        // 🔸Permanent Address
                        lblPerAddressLine1.Text = dr["Per_Address1"].ToString();
                        lblPerAddressLine2.Text = dr["Per_Address2"].ToString();
                        lblPerAddressLine3.Text = dr["Per_Address3"].ToString();
                        lblPerCity.Text = dr["Per_City"].ToString();
                        // ddlPState.SelectedValue = dr["State_ID"].ToString();
                        ddlPState.SelectedValue = dr["Per_State_ID"].ToString();
                        LblPerPinCode.Text = dr["Per_Pin"].ToString();
                        bindDistrict(Convert.ToInt64(ddlPState.SelectedValue), ref ddlPdistrict);
                        string qsPerDistrictID = Request.QueryString["PerDistrictID"];
                        if (!string.IsNullOrEmpty(qsPerDistrictID))
                        {
                            ddlPdistrict.SelectedValue = qsPerDistrictID;
                        }
                        else
                        {
                            ddlPdistrict.SelectedValue = dr["Per_District_ID"].ToString();
                        }
                        // 🔸 Course & Institute
                        LblCourse.Text = dr["Course_Name"].ToString();                       
                        lblRegistrationNo.Text = dr["Registered_Course_Registration_No"].ToString();
                        // For Apaar

                        if (dr["Photo"] != DBNull.Value)
                        {
                            byte[] bytes = (byte[])dr["Photo"];
                            string base64String = Convert.ToBase64String(bytes);                            
                        }
                        if (dr["Signature"] != DBNull.Value)
                        {
                            byte[] bytes = (byte[])dr["Signature"];
                            string base64String = Convert.ToBase64String(bytes);                           
                        }
                    }
                   
                    con.Close();
                }
            }

            // For Documents
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetMercyDocuments", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@candidate_id", Id);

                    conn.Open();
                    SqlDataReader drDoc = cmd.ExecuteReader();

                    // Default values (important)
                    //lblEduStatus.Text = "Not Uploaded";
                    //lblExpCertiFormStatus.Text = "Not Uploaded";
                    lblMedicalStatus.Text = "Not Uploaded";
                   // lblIDCardStatus.Text = "Not Uploaded";

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
                        //    lblIDCardStatus.Text = status;
                        //}
                    }

                    drDoc.Close();
                   
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool IsValidForm()
    {
        try
        {
            //lblcoraddline1.Text = "";
            //if (string.IsNullOrEmpty(txtCorAddressLine1.Text))
            //{
            //    lblcoraddline1.Visible = true;
            //    lblcoraddline1.Text = "Address Line1 can not be left blank";
            //    return false;
            //}

            //lblcoraddline2.Text = "";
            //if (string.IsNullOrEmpty(txtCorAddressLine2.Text))
            //{
            //    lblcoraddline2.Visible = true;
            //    lblcoraddline2.Text = "Address Line2 can not be left blank";
            //    return false;
            //}

            //lblcoraddline3.Text = "";
            //if (string.IsNullOrEmpty(txtCorAddressLine3.Text))
            //{
            //    lblcoraddline3.Visible = true;
            //    lblcoraddline3.Text = "Address Line3 can not be left blank";
            //    return false;
            //}

            //lblcorcity.Text = "";
            //if (string.IsNullOrEmpty(txtCorCity.Text))
            //{
            //    lblcorcity.Visible = true;
            //    lblcorcity.Text = "City Name can not be left blank";
            //    return false;
            //}

            //lblmobile.Text = "";
            //if (string.IsNullOrEmpty(txtMobile.Text))
            //{
            //    lblmobile.Visible = true;
            //    lblmobile.Text = "Mobile Number can not be left blank";
            //    return false;
            //}

            //lblEmail.Text = "";
            //if (string.IsNullOrEmpty(txtEmail.Text))
            //{
            //    lblEmail.Visible = true;
            //    lblEmail.Text = "Email Address can not be left blank";
            //    return false;
            //}

            //lblPincode.Text = "";
            //if (string.IsNullOrEmpty(txtPincode.Text))
            //{
            //    lblPincode.Visible = true;
            //    lblPincode.Text = "Pincode can not be left blank";
            //    return false;
            //}

            //if (ImgUpload.FileName.ToString() == "" && string.IsNullOrEmpty(lblphotoShow.Text))
            //{
            //    lblphotoShow.Visible = true;
            //    // GenerateNewCaptchaImage();
            //    //  txtcode.Text = "";
            //    lblphotoShow.Text = "Photo can not be left blank";
            //    return false;
            //}
            //if (!isvalidFileExtension(ImgUpload) && string.IsNullOrEmpty(lblphotoShow.Text))
            //{
            //    lblphotoShow.Visible = true;
            //    //  GenerateNewCaptchaImage();
            //    // txtcode.Text = "";
            //    lblphotoShow.Text = "Invalid Photo .Only jpg, gif, jpeg ,png extensions are allowed.";
            //    return false;
            //}
            //if (!isvalidFileSize(ImgUpload, 51200) && string.IsNullOrEmpty(lblphotoShow.Text))
            //{
            //    lblphotoShow.Visible = true;
            //    //GenerateNewCaptchaImage();
            //    //txtcode.Text = "";
            //    lblphotoShow.Text = "File size should be of 50 KB or less.";
            //    return false;
            //}
            //if (ImgUpload.HasFile && ImgUpload.FileName.Length > 50)
            //{
            //    //GenerateNewCaptchaImage();
            //    //txtcode.Text = "";
            //    lblphotoShow.Text = "Photograph file name should be less than 50 characters.";
            //    // throw new Exception("Photograph file name should be less than 50 characters.");
            //}
            //if (ImgUploadSignature.FileName.ToString() == "" && string.IsNullOrEmpty(lblsignShow.Text))
            //{
            //    lblsignShow.Visible = true;
            //    //GenerateNewCaptchaImage();
            //    //txtcode.Text = "";
            //    lblsignShow.Text = "Signature can not be left blank";
            //    return false;
            //}
            //if (!isvalidFileExtension(ImgUploadSignature) && string.IsNullOrEmpty(lblsignShow.Text)) //image upload code added_by_amit_audit_april_2026_start
            //{
            //    lblsignShow.Visible = true;
            //    //GenerateNewCaptchaImage();
            //    //txtcode.Text = "";
            //    lblsignShow.Text = "Invalid Signature .Only jpg, gif, jpeg ,png extensions are allowed.";
            //    return false;
            //}
            //if (!isvalidFileSize(ImgUploadSignature, 51200) && string.IsNullOrEmpty(lblsignShow.Text)) //image upload code added_by_amit_audit_april_2026_start
            //{
            //    lblsignShow.Visible = true;
            //    //GenerateNewCaptchaImage();
            //    //txtcode.Text = "";
            //    lblsignShow.Text = "Signature File size should be of 50 KB or less.";
            //    return false;
            //}
            //if (ImgUploadSignature.HasFile && ImgUploadSignature.FileName.Length > 50)
            //{
            //    //GenerateNewCaptchaImage();
            //    //txtcode.Text = "";
            //    lblsignShow.Text = "Signature file name should be less than 50 characters.";
            //    //  throw new Exception("Signature file name should be less than 50 characters.");
            //}

            if (trapaar.Visible == true)
            {
                if (String.IsNullOrEmpty(txtapaar.Text.Trim()))
                {
                    ShowAlert("Apaar ID  should not be Blank.");
                    return false;
                    // GenerateNewCaptchaImage();
                    //  throw new Exception("Apaar ID  should not be Blank.");

                }
            }

            // For Documents          

            //bool isEduValid = false;

            //if (fuEduquacertiForm.HasFile)
            //{
            //    string extension = Path.GetExtension(fuEduquacertiForm.FileName).ToLower();

            //    // Validate extension
            //    if (extension != ".pdf")
            //    {
            //        lblEduStatus.ForeColor = System.Drawing.Color.Red;
            //        lblEduStatus.Text = "Only PDF file allowed.";
            //        return false;
            //    }

            //    // Validate size (1 MB)
            //    if (fuEduquacertiForm.PostedFile.ContentLength > 1048576)
            //    {
            //        lblEduStatus.ForeColor = System.Drawing.Color.Red;
            //        lblEduStatus.Text = "File size should not exceed 1 MB.";
            //        return false;
            //    }

            //    // Store file in Session
            //    Session["EduFile"] = fuEduquacertiForm.FileBytes;
            //    Session["EduFileName"] = fuEduquacertiForm.FileName;

            //    lblEduStatus.ForeColor = System.Drawing.Color.Green;
            //    lblEduStatus.Text = fuEduquacertiForm.FileName + " uploaded successfully.";

            //    isEduValid = true;
            //}
            //else if (Session["EduFile"] != null)
            //{
            //    lblEduStatus.ForeColor = System.Drawing.Color.Green;
            //    lblEduStatus.Text = Session["EduFileName"].ToString() + " already uploaded.";

            //    isEduValid = true;
            //}
            //else
            //{
            //    lblEduStatus.ForeColor = System.Drawing.Color.Red;
            //    lblEduStatus.Text = "Please upload Education Form file.";

            //    return false;
            //}

        //    bool isExpValid = false;
        //    if (trexpcerti.Visible == true)
        //    { 
        //        if (fuexpcertiForm.HasFile)
        //        {
        //            string extension = Path.GetExtension(fuexpcertiForm.FileName).ToLower();

        //            if (extension != ".pdf")
        //            {
        //                lblExpCertiFormStatus.ForeColor = System.Drawing.Color.Red;
        //                lblExpCertiFormStatus.Text = "Only PDF file allowed.";
        //                return false;
        //            }

        //            if (fuexpcertiForm.PostedFile.ContentLength > 1048576)
        //            {
        //                lblExpCertiFormStatus.ForeColor = System.Drawing.Color.Red;
        //                lblExpCertiFormStatus.Text = "File size should not exceed 1 MB.";
        //                return false;
        //            }

        //            Session["ExpFile"] = fuexpcertiForm.FileBytes;
        //            Session["ExpFileName"] = fuexpcertiForm.FileName;

        //            lblExpCertiFormStatus.ForeColor = System.Drawing.Color.Green;
        //            lblExpCertiFormStatus.Text = fuexpcertiForm.FileName + " uploaded successfully.";

        //            isExpValid = true;
        //        }
        //        else if (Session["ExpFile"] != null)
        //        {
        //            lblExpCertiFormStatus.ForeColor = System.Drawing.Color.Green;
        //            lblExpCertiFormStatus.Text = Session["ExpFileName"].ToString() + " already uploaded.";

        //            isExpValid = true;
        //        }
        //        else
        //        {
        //            lblExpCertiFormStatus.ForeColor = System.Drawing.Color.Red;
        //            lblExpCertiFormStatus.Text = "Please upload Experience Certificate file.";

        //            return false;
        //        }
        //}

            bool isMedicalValid = false;

            if (Fumedical.HasFile)
            {
                string extension = Path.GetExtension(Fumedical.FileName).ToLower();

                if (extension != ".pdf")
                {
                    lblMedicalStatus.ForeColor = System.Drawing.Color.Red;
                    lblMedicalStatus.Text = "Only PDF file allowed.";
                    return false;
                }

                if (Fumedical.PostedFile.ContentLength > 1048576)
                {
                    lblMedicalStatus.ForeColor = System.Drawing.Color.Red;
                    lblMedicalStatus.Text = "File size should not exceed 1 MB.";
                    return false;
                }

                Session["MedicalFile"] = Fumedical.FileBytes;
                Session["MedicalFileName"] = Fumedical.FileName;

                lblMedicalStatus.ForeColor = System.Drawing.Color.Green;
                lblMedicalStatus.Text = Fumedical.FileName + " uploaded successfully.";

                isMedicalValid = true;
            }
            else if (Session["MedicalFile"] != null)
            {
                lblMedicalStatus.ForeColor = System.Drawing.Color.Green;
                lblMedicalStatus.Text = Session["MedicalFileName"].ToString() + " already uploaded.";

                isMedicalValid = true;
            }
            else
            {
                lblMedicalStatus.ForeColor = System.Drawing.Color.Red;
                lblMedicalStatus.Text = "Please upload Medical Certificate file.";

                return false;
            }
            // ID Card
            //bool isIdCardValid = false;

            //if (FuIdCard.HasFile)
            //{
            //    string extension = Path.GetExtension(FuIdCard.FileName).ToLower();

            //    if (extension != ".pdf")
            //    {
            //        lblIDCardStatus.ForeColor = System.Drawing.Color.Red;
            //        lblIDCardStatus.Text = "Only PDF file allowed.";
            //        return false;
            //    }

            //    if (FuIdCard.PostedFile.ContentLength > 1048576)
            //    {
            //        lblIDCardStatus.ForeColor = System.Drawing.Color.Red;
            //        lblIDCardStatus.Text = "File size should not exceed 1 MB.";
            //        return false;
            //    }

            //    Session["IdCardFile"] = FuIdCard.FileBytes;
            //    Session["IdCardFileName"] = FuIdCard.FileName;

            //    lblIDCardStatus.ForeColor = System.Drawing.Color.Green;
            //    lblIDCardStatus.Text = FuIdCard.FileName + " uploaded successfully.";

            //    isIdCardValid = true;
            //}
            //else if (Session["IdCardFile"] != null)
            //{
            //    lblIDCardStatus.ForeColor = System.Drawing.Color.Green;
            //    lblIDCardStatus.Text = Session["IdCardFileName"].ToString() + " already uploaded.";

            //    isIdCardValid = true;
            //}
            //else
            //{
            //    lblIDCardStatus.ForeColor = System.Drawing.Color.Red;
            //    lblIDCardStatus.Text = "Please upload ID Card file.";

            //    return false;
            //}
          //  For Declaration
            if (chkdisclamier.Checked != true)
            {
                // GenerateNewCaptchaImage();
                // txtcode.Text = "";
                // throw new Exception("Please check the Declaration Statement");
                ShowAlert("Please check the Declaration Statement.");
                return false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return true;
    }

    private void SetConsentRelationVisibility()
    {
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
    }

    protected void txtapaar_TextChanged(object sender, EventArgs e)
    {        
        try
        {

            SetConsentRelationVisibility();        
            int applicantTypeId = GetApplicantType(candidateId, courseId);
            SetApplicantType(applicantTypeId);
            DateTime todaydate = DateTime.Now;
            DateTime Inputdate = Convert.ToDateTime(LblDob.Text);
            int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);
            int applicantType = applicant_type_id;          

            txtAuthenticationIdNo.Text = "";          
            ddlConsentRelation.SelectedIndex = 0;
            txtproviderName.Text = "";
            txtConsentDate.Text = "";
            txtConsentTime.Text = "";
            txtConsentPlace.Text = "";

         //   bindDeclaration(applicantTypeId, countAge);
            if (countAge >= 0)
            {
                txtIsProviderPresent.Text = "True";
                txtIsProviderPresent.Enabled = false;

                ddlConsentRelation.Items.Clear();
                ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
                ddlConsentRelation.Items.Add(new ListItem("Self", "1"));

                ddlConsentRelation.SelectedIndex = 1;
                ddlConsentRelation.Enabled = false;
                ddlConsentRelation_SelectedIndexChanged(sender, e);

                txtproviderName.Text = LblAppName.Text;
                txtproviderName.Enabled = false;

                BindAuthMode(countAge);
                ddlAuthMode.SelectedValue = "1";

                ddlAuthMode.Enabled = false;               
                txtAuthenticationIdNo.Text = txtapaar.Text;
                txtAuthenticationIdNo.Enabled = false;
            }
            else
            {
                txtIsProviderPresent.Text = "True";
                txtIsProviderPresent.Enabled = false;
                ddlConsentRelation.Enabled = true;
                txtproviderName.Enabled = true;
                ddlAuthMode.Enabled = true;
                txtAuthenticationIdNo.Enabled = true;

                bool isGuardianRequired =String.IsNullOrEmpty(LblFName.Text) && String.IsNullOrEmpty(LblMName.Text);
                if (isGuardianRequired)
                {
                    ddlConsentRelation.Items.Clear();
                    ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
                    ddlConsentRelation.Items.Add(new ListItem("Guardian", "2"));
                }
                else
                {
                    ddlConsentRelation.Items.Clear();
                    ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
                    ddlConsentRelation.Items.Add(new ListItem("Father", "3"));
                    ddlConsentRelation.Items.Add(new ListItem("Mother", "4"));
                } 
                ddlConsentRelation.SelectedIndex = 0;
                txtproviderName.Text = "";   

                BindAuthMode(countAge);
                ddlAuthMode.SelectedIndex = 0;

            }
            txtConsentDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtConsentDate.ReadOnly = true;
            txtConsentTime.Text = DateTime.Now.ToString("HH:mm");
            txtConsentTime.ReadOnly = true;
            txtConsentPlace.Text = txtConsentPlace.Text;
           
            BindApaarDeclaration(countAge);
           
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
            ShowAlert(ex.Message);
        }
    } 

    protected void ddlConsentRelation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txtproviderName.Text = "";

            switch (ddlConsentRelation.SelectedValue)
            {
                case "1":
                    txtproviderName.Text = LblAppName.Text;
                    break;

                case "2":
                    txtproviderName.Text = LblGuardianName.Text;
                    break;

                case "3":
                    txtproviderName.Text = LblFName.Text;
                    break;

                case "4":
                    txtproviderName.Text = LblMName.Text;
                    break;

                default:
                    txtproviderName.Text = "";
                    break;
            }
            txtproviderName.Enabled = false;
            DateTime todaydate = DateTime.Now;
            DateTime inputdate = Convert.ToDateTime(LblDob.Text);

            int countAge = DateTime.Compare(todaydate.AddYears(-18), inputdate);

            BindApaarDeclaration(countAge);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
            ShowAlert(ex.Message);
        }
    }

    private void BindApaarDeclaration(int countAge)
    {
        string relationText = "";
        string wardText = "";
        string authDoc = "";

        if (countAge >= 0)
        {
            relationText = txtproviderName.Text + "(" + ddlConsentRelation.SelectedItem.Text + ")";
            wardText = "self";
        }
        else
        {
            relationText = txtproviderName.Text + "(" + ddlConsentRelation.SelectedItem.Text + ")";
            wardText = "ward";
        }

        authDoc = txtAuthenticationIdNo.Text;

        lblApaarDeclaration.Text =
                                  "I "
                                  + relationText +
                                  ", hereby voluntarily give my consent to NIELIT to use APAAR ID of "
                                  + LblAppName.Text +
                                  " (" + wardText + ") with APAAR ID as "
                                  + txtapaar.Text +
                                  " for validation of personal details."

                                  + "I understand that the APAAR ID may be used and shared only for limited, authorized purposes, "
                                  + "and that the information provided by me shall be kept confidential."

                                  + "The information w.r.t authentication document no. "
                                  + authDoc +
                                  " provided by me, is correct and valid to the best of my knowledge.";
    }

    protected void txtAppName_TextChanged(object sender, EventArgs e)
    {
        txtapaar.Text = "";
        txtAuthenticationIdNo.Text = "";
    }

    protected void ddlAuthMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (ddlAuthMode.SelectedItem.Text.ToLower() == "self")
            {
                txtAuthenticationIdNo.Text = txtapaar.Text.ToString();
                txtAuthenticationIdNo.ReadOnly = true;
            }
            else
            {
                txtAuthenticationIdNo.Text = "";
                txtAuthenticationIdNo.ReadOnly = false;
            }

            DateTime todaydate = DateTime.Now;
            DateTime Inputdate = Convert.ToDateTime(LblDob.Text);
            int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);
            BindApaarDeclaration(countAge);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
            ShowAlert(ex.Message);
        }
    }

    protected void txtAuthenticationIdNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (String.IsNullOrEmpty(LblDob.Text))
            {
                throw new Exception("Invalid Date of Birth. Fill in correct format example : 01-Jan-2000");

            }
            int countAge = 0;

            if (ViewState["CountAge"] != null)
            {
                countAge = Convert.ToInt32(ViewState["CountAge"]);
            }

            BindApaarDeclaration(countAge);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
            ShowAlert(ex.Message);
        }
    }

    protected void BindAuthMode(int whether18)
    {
        try
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);

            SqlCommand cmd = new SqlCommand("BIND_AUTH_MODE", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@whether18", whether18);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            da.Fill(dt);

            ddlAuthMode.DataSource = dt;
            ddlAuthMode.DataTextField = "AuthModeName";
            ddlAuthMode.DataValueField = "AuthModeValue";
            ddlAuthMode.DataBind();

            ddlAuthMode.Items.Insert(0, new ListItem("--Select--", "0"));

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
            ShowAlert(ex.Message);
        }
    }

    protected void bindExamName(Int32 applicant_type_id,Int32 courseId)
    {
        try
        {
              Int32 examID = 0;
            // Int32 courseId = 0;
            Int32 NormalactivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
            Int32 LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee);

            using (EConnectContext context = new EConnectContext())
            {
                string appNumber = Convert.ToString(ViewState["AppID"]);

                var app = context.CourseRegistrationApplications
                                 .FirstOrDefault(x => x.Number == appNumber);

                if (app != null)
                {
                    courseId = app.CourseID;
                }
                
                var LateFeeExam = (from e in context.CutOffDates
                                   join i in context.Exams on e.ExamID equals i.ID
                                   where e.CourseID == courseId
                                   && e.ApplicantTypeID == applicant_type_id
                                   && e.ActivityID == LateFeeActivityId
                                   && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                   orderby e.EfferctiveDate ascending
                                   select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
                if (LateFeeExam.Count() > 0)//If  applicable for late fee ?
                {

                    if (LateFeeExam != null)
                    {
                        LblExamName.Text = LateFeeExam.FirstOrDefault().ExamName;
                        examID = LateFeeExam.FirstOrDefault().ExamID;
                      //  btnSave.Visible = true;
                    }
                }
                else if (LateFeeExam.Count() <= 0)//If  not applicable for late fee ?
                {

                    var NormalFeeExam = (from e in context.CutOffDates
                                         join i in context.Exams on e.ExamID equals i.ID
                                         where e.CourseID == courseId
                                         && e.ApplicantTypeID == applicant_type_id
                                         && e.ActivityID == NormalactivityId
                                         && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                         orderby e.EfferctiveDate ascending
                                         select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
                    if (NormalFeeExam.Count() > 0)
                    {
                        LblExamName.Text = NormalFeeExam.FirstOrDefault().ExamName;
                        ViewState["ExamName"] = LblExamName.Text.ToString();
                        examID = NormalFeeExam.FirstOrDefault().ExamID;
                      //  btnSave.Visible = true;
                    }

                }
                ViewState["ExamID"] = examID.ToString();
                ShowFeeDetail(examID);
                // Added by Amit start
                //if (DdlProject.SelectedValue != "0")
                //    getFeeDetailForProjectCourse();
                //else
                //    ShowFeeDetail(examID);
                // Added by Amit end
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    } 

    protected void btnOK_Click(object sender, EventArgs e)
    {
      //  ActiveInActive(true);
        divgurdian.Style.Add("display", "none");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
      //  ActiveInActive(true);
        divgurdian.Style.Add("display", "none");
        //Rdoownertype.ClearSelection();
        //Rdoownertype.SelectedValue = "P";
        //Rdoownertype_SelectedIndexChanged(Rdoownertype.SelectedValue, EventArgs.Empty);
    }

    private void BindConsentRelationDropdown()
    {
        ddlConsentRelation.Items.Clear();

        if (trguardian.Visible)
        {
            ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
            ddlConsentRelation.Items.Add(new ListItem("Guardian", "2"));
        }
        else
        {
            ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
            ddlConsentRelation.Items.Add(new ListItem("Father", "3"));
            ddlConsentRelation.Items.Add(new ListItem("Mother", "4"));
        }
    }

    protected void ShowFeeDetail(Int32 examID)
    {
        try
        {
            int courseID = Convert.ToInt16(Session["CourseID"]);//1;//Convert.ToInt32(Request.QueryString["id"]);

            if (examID != 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 cscProcessingFee = 0;
                    if (Request.QueryString["Src"] != null)
                    {
                        if (Request.QueryString["Src"] == "CSC")
                        {
                            Int32 applicationtypeid = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                            Int32 activityid = Convert.ToInt32(enmCSCActivity.FillFormandDepositFee);
                            cscProcessingFee = CourseManager.GetCSCProcessingCharge(context, applicationtypeid, activityid);
                            InfoDiv.Attributes.Add("class", "modalPopup1");
                        }
                    }
                    var course = context.Courses.Find(courseID);
                    ImgBtnPopupFee.Enabled = true;
                    ImgBtnPopupFee.ImageUrl = "~/images/popup1.jpg";
                    ImgBtnPopupFee.ToolTip = "Click here to view Fee Detail.";
                    int FeeTypeID = Convert.ToInt32(enmFeeType.MercyRegistrationFee);
                    LblFeeTypeName.Text = Convert.ToString(enmFeeType.MercyRegistrationFee);
                    int ExamId = examID;
                    Int32 feeAmount = 0;

                    /*  feeAmount = (from f in context.FeeDetails
                                   where f.CourseID == courseID 
                                   && f.FeeTypeID == FeeTypeID 
                                   && f.EffectiveFromDate == (from c in context.FeeDetails
                                                            where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                                                            select c.EffectiveFromDate).Max()
                                   select new { FeeAmount = f.FeeAmount }).FirstOrDefault().FeeAmount;*/
                    //added by amit start                
                    feeAmount = (from r in context.FeeDetails
                                 where r.CourseID == courseID
                                    && r.FeeTypeID == FeeTypeID
                                    && r.EffectiveFromDate <= System.Data.Entity.DbFunctions.CreateDateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)

                                    && (r.EffectiveToDate == null || r.EffectiveToDate >= System.Data.Entity.DbFunctions.CreateDateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0))
                                    && r.projectid == null
                                 orderby r.EffectiveFromDate descending
                                 select r.FeeAmount).FirstOrDefault();



                    ViewState["defaultFeeAmount"] = feeAmount;
                    //added by amit end

                    int LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee);

                    int NormalFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
                    int applicant_type_id = Convert.ToInt32(ViewState["ApplicantTypeID"]);
                  //  int ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
                    int LateFeeTypeId = Convert.ToInt32(enmFeeType.LateFeeRegistration);

                    var Fee = (from f in context.CutOffDates
                               where f.CourseID == courseID
                               && f.ExamID == ExamId
                               && f.ApplicantTypeID == applicant_type_id
                               select new { EffectiveDate = f.EfferctiveDate, f.ActivityID }).ToList();

                    var NormalFee = Fee.Where(l => l.ActivityID == NormalFeeActivityId);
                    var lateFee = Fee.Where(l => l.ActivityID == LateFeeActivityId);
                    if (lateFee.Count() > 0 && lateFee != null && NormalFee.Count() > 0)
                    {
                        if (NormalFee.FirstOrDefault().EffectiveDate <= DateTime.Now)
                        {
                            Int32 LatefeeAmount = (from f in context.FeeDetails
                                                   where f.CourseID == courseID
                                                   && f.FeeTypeID == LateFeeTypeId
                                                   && f.EffectiveFromDate == (from c in context.FeeDetails
                                                                              where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                                                                              select c.EffectiveFromDate).Max()
                                                   select new { FeeAmount = f.FeeAmount }).FirstOrDefault().FeeAmount;
                            if (LatefeeAmount != 0 && LatefeeAmount != null)
                            {
                                if (LatefeeAmount > 0)
                                {
                                    lblFeeDetail.Text = "Fee : Rs/- " + (feeAmount + LatefeeAmount + cscProcessingFee) + ".00";
                                    LblNormalFee.Text = feeAmount.ToString("F");
                                    LblLateFee.Text = LatefeeAmount.ToString("F");
                                    lblProcessingFee.Text = cscProcessingFee.ToString("F");
                                    LblTotalFee.Text = (feeAmount + LatefeeAmount + cscProcessingFee).ToString("F");

                                }
                            }
                        }
                        else
                        {
                            lblFeeDetail.Text = "Fee : Rs/- " + (feeAmount + cscProcessingFee) + ".00";
                            LblNormalFee.Text = feeAmount.ToString("F");
                            lblProcessingFee.Text = cscProcessingFee.ToString("F");
                            LblTotalFee.Text = (feeAmount + cscProcessingFee).ToString("F");
                            ViewState["ExamFee"] = lblFeeDetail.Text.ToString();
                        }
                    }
                    else
                    {
                        LblNormalFee.Text = feeAmount.ToString("F");
                        LblTotalFee.Text = (feeAmount + cscProcessingFee).ToString("F");
                        lblProcessingFee.Text = cscProcessingFee.ToString("F");
                        lblFeeDetail.Text = "Fee : Rs/- " + (feeAmount + cscProcessingFee) + ".00";
                        ViewState["ExamFee"] = lblFeeDetail.Text.ToString();
                        //if (course.CourseCategoryID == 6) //STC
                        //{
                        //    lblFeeDetail.Text = "Fee will be paid to concerned regional center.";
                        //    ImgBtnPopupFee.Enabled = false;
                        //}
                    }
                }
                ;
                LblAmountInWords.Text = " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(LblTotalFee.Text.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
            }
            else
            {
                ImgBtnPopupFee.ToolTip = "";
                ImgBtnPopupFee.Enabled = false;
                ImgBtnPopupFee.ImageUrl = "~/images/DisablePopup.PNG";
                lblFeeDetail.Text = "";
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }
    }

    protected void GetApplicantType()
    {
        try
        {
            if (Request.QueryString["id"] == null)
                return;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
                var course = context.Courses.Find(courseID);
                if (course != null)
                {
                    if (!String.IsNullOrEmpty(course.ApplicantTypeID.ToString()))
                    {
                        if (course.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                            RdoUndergngDOEACC.SelectedValue = "D";
                        else
                            RdoUndergngDOEACC.SelectedValue = "I";
                        lblApplicantType.Text = RdoUndergngDOEACC.SelectedItem.Text;
                        RdoUndergngDOEACC.Visible = false;
                        lblApplicantType.Visible = true;
                       
                        RdoUndergngDOEACC_SelectedIndexChanged(this, null);
                    }
                   
                }
                ;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }

    protected void RdoUndergngDOEACC_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  LblExamName.Text = "";
        // int courseId = 1;
        int courseId = Convert.ToInt32(Session["CourseID"]);
        int applicantTypeId = (RdoUndergngDOEACC.SelectedValue == "D") ? 1 : 2;
        Session["ApplicantTypeID"] = applicantTypeId;
        SetApplicantType(applicantTypeId);
        bindExamName(applicantTypeId, courseId);
        SetSectionNumbers();
        //try
        //{
        //    LblExamName.Text = "";
        //    if (RdoUndergngDOEACC.SelectedValue == "I")
        //    {
        //        TrStateCenterAccno.Visible = true;
        //        TrCenterNameAccno.Visible = true;
        //       // TrExperience.Visible = false;
        //        //Added for NSQF
        //        EConnectContext context = new EConnectContext();
        //        this.courseId = 1;// Convert.ToInt64(Request.QueryString["Id"]);
        //       // var course = context.Courses.Find(courseId);
        //        //if (course.CourseCategoryID.ToString() == "6" && Convert.ToInt32(course.ID.ToString()) > 102)
        //        //{
        //        //    TrExperience.Visible = true;
        //        //    Label63.Text = "Experience in years / वर्षों का अनुभव";
        //        //}
        //        //
        //       // ddlPaymentOption.SelectedValue = "2";
        //       // ddlPaymentOption.Enabled = true;
        //        //DdlAccState.SelectedValue = "0";
        //        DdlAccState_SelectedIndexChanged(DdlAccState, EventArgs.Empty);
        //     //   bindDeclaration("I", 0);
        //        //added by amit start
        //        //if (courseId == 1)
        //        //    TrUPBoard.Visible = true;


        //        // to reset the fields set by Select Project, added 25/06/25
        //      //  radProject.SelectedValue = "0";
        //        DdlAccState.SelectedValue = "0";
        //        DdlAccCentre.SelectedValue = "0";
        //        DdlAccState.Enabled = true;

        //        //added by amit end

        //    }
        //    else
        //    {
        //      //  TrExperience.Visible = true;
        //        //Added for NSQF courses
        //      //  Label63.Text = " If direct, experience in years / यदि  डायरेक्ट, वर्षों का अनुभव";
        //        TrStateCenterAccno.Visible = false;
        //        TrCenterNameAccno.Visible = false;
        //      //  ddlPaymentOption.SelectedValue = "1";
        //     //   ddlPaymentOption.Enabled = false;
        //     //   txtDob.Text = "";
        //     //   bindDeclaration("D", 0);
        //        // added by amit start
        //      //  TrUPBoard.Visible = false;
        //      //  trUPProject.Visible = false; // radio button

        //        //if (Session["isBSB"].ToString() != "1" || !checkBSB())
        //        //    trBSB.Visible = false;

        //        //trUPField2.Visible = false;
        //        //trUPField3.Visible = false;
        //        //trUPField4.Visible = false;
        //        //trUPField5.Visible = false;


        //        // amit added 23-6-2025
        //      //  DdlProject.SelectedIndex = 0;
        //        // added by amit end
        //    }
        //    Int32 ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
        //  //  bindEducational();
        //    bindExamName(ApplicantTypeId,courseId);
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        //}
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

        LblIdentification11.Text = sectionNo + ".1.1";
        LblIdentification12.Text = sectionNo + ".1.2";
        LblIdentification13.Text = sectionNo + ".1.3";
        LblIdentification14.Text = sectionNo + ".1.4";
        LblIdentification15.Text = sectionNo + ".1.5";
        LblIdentification16.Text = sectionNo + ".1.6";
        LblIdentification17.Text = sectionNo + ".1.7";
        LblIdentification18.Text = sectionNo + ".1.8";

       // LblIdentification2.Text = sectionNo + ".2";
      //  LblIdentification3.Text = sectionNo + ".3";

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