
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
//-------------- added on 25-08-2021 for Image API ----------------------/
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Microsoft.SqlServer.Server;

public partial class Certificate : BasePage
{
    Int64 CandId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;

            //-------------- added on 06-10-2021 by vishal----------------------/
            Page.Header.DataBind();
            ImgUpload.Attributes["onchange"] = "UploadFile(this)";
            Page.Header.DataBind();
            ImgUploadSignature.Attributes["onchange"] = "imageSignpreview(this)";
            Page.Header.DataBind();
            ImgUploadThumb.Attributes["onchange"] = "imageThumbpreview(this)";
            //-------------- added on 06-10-2021 by vishal----------------------/

            /*
               if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in") && (Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "https://nielit.gov.in"))
               {

                   string urldecoded = HttpUtility.HtmlDecode(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
                   Response.Write(urldecoded);
                   Response.End();
                   return;
               }

               else
               {
                   string pageRef = Request.UrlReferrer.ToString();
                   if (!pageRef.Contains("https://student.nielit.gov.in/"))
                   {
                       //                  //16.02.2022
                       //                  //Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
                       //                  //Response.End();
                       //                  //return;
                   }
               }
               */
            Course CurrentCourse;
            CourseCategory CourseCategory;
            Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
            using (EConnectContext vContext = new EConnectContext())
            { CurrentCourse = vContext.Courses.Find(courseID); CourseCategory = CurrentCourse.CourseCategory; }

            //----------by vivek on 23-02-2016---------------
            string candtype = !String.IsNullOrEmpty(Request.QueryString["candtype"]) ? Request.QueryString["candtype"] : "NA";

            if (((candtype == "External") && Session["EntityID"] != null) || (candtype == "Internal" && (Session["EntityID"] == null || Convert.ToInt32(Session["UserType"]) != Convert.ToInt32(UserType.Candidate))))
            {
                Session.Abandon();
                Response.Redirect("~/CAND/CertificateRegistration.aspx?ID=" + Request.QueryString["id"].ToString() + "&CoursecategoryID=" + CurrentCourse.CourseCategoryID.ToString() + "&candtype=" + Request.QueryString["candtype"].ToString());
            }

            if ((candtype == "Internal") && Session["EntityID"] != null && CurrentCourse.CourseCategory.Name == "IRDA" && Convert.ToInt32(Session["UserType"]) == Convert.ToInt32(UserType.Candidate))
            { CandId = Convert.ToInt64(Session["EntityID"]); }

            if ((candtype == "NA") && Session["EntityID"] != null && CurrentCourse.CourseCategory.Name == "IRDA" && Convert.ToInt32(Session["UserType"]) == Convert.ToInt32(UserType.Candidate))
            { CandId = Convert.ToInt64(Session["EntityID"]); }
            //------------------------------------------------
            lblerror.Visible = false;

            if (Request.QueryString["Src"] != null)
            {
                if (Request.QueryString["Src"] == "CSC")
                {
                    RdoAppliedAs.SelectedValue = "D";
                    RdoAppliedAs.Enabled = false;
                    NormalHeader1.Visible = true;
                }
            }
            if (!Page.IsPostBack)
            {

                //-------------- added on 06-10-2021 API----------------------/
                Session["ImgUpload"] = null;
                Session["ImgUploadSignature"] = null;
                Session["ImgUploadThumb"] = null;
                //-------------- added on 06-10-2021 API----------------------/

                divPopup.Style.Add("display", "none");

                if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["id"])))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    LblCourseinEnglish.Text = CurrentCourse.Code;
                    spnCourseinHindi.InnerText = CurrentCourse.Code;
                    lblhdeccoursecode.Text = CurrentCourse.NameRegional;
                    lbldeccoursecode.Text = CurrentCourse.Code.ToUpper();
                    Lblhead.Text = CurrentCourse.Name + "(" + CurrentCourse.Code + ")";
                    Rdoownertype_SelectedIndexChanged(Rdoownertype.SelectedValue, EventArgs.Empty);
                    //Disability Type
                    DisabilityType();
                    //Binding Cast Category Name Dropdownlist
                    CastCategory();
                    //Label49.Text = "Aadhar Card Number / आधार कार्ड संख्या"; //Aadhar card number not mandatory
                    //Binding Occupation
                    Occupation(CurrentCourse);
                    //Binding Educational Qualification Dropdownlist
                    EducationalQualification(CurrentCourse);
                    //Binding State Name Dropdownlist
                    BindState(CurrentCourse.ID);
                    BindGender();

                    ShowAttachment(CurrentCourse.CourseCategoryID);
                    BindExamCycle(CurrentCourse.ID);
                    GetApplicantType(CurrentCourse);
                    if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                    {
                        using (var context = new EConnectContext())
                        {
                            var status = context.CertificateExamApplications.Find(Convert.ToInt64(Request.QueryString["Appid"]));
                            if (status.FinalSubmitted == true)
                            { Response.Redirect("Index.aspx"); }
                        }
                        btnback.Visible = false;
                        btnSave.Text = "Update";
                        ShowData();
                        DdlExamCentreState1.Enabled = false;
                    }
                    else
                        btnSave.Text = "Submit";
                }
                //------Added for IRDA on on 30-01-2016
                if (CourseCategory.Name == "IRDA")
                {
                    BindCandDetail(CurrentCourse);
                    AlertModalPopUp.Show();
                }
                //----------End for IRDA on 30-01-2016
            }

            //-------------- added on 06-10-2021 API----------------------/
            else
            {

                string UploadPhoto = string.Empty, UploadPhoto1 = string.Empty;
                string UploadSing = string.Empty, UploadSing1 = string.Empty;
                string UploadLeftTh = string.Empty, UploadLeftTh1 = string.Empty;

                #region Images API - Session
                // Photo
                if (Session["ImgUpload"] == null && ImgUpload.HasFile)
                {
                    Session["ImgUpload"] = ImgUpload;
                    string ImgNamePhoto = string.Empty;
                    ImgNamePhoto = "P" + Path.GetFileName(ImgUpload.FileName);          //Photo_
                    ImagePathPh.Text = ImgNamePhoto;
                    UploadPhoto1 = ImgNamePhoto;
                }
                else if (Session["ImgUpload"] != null && (!ImgUpload.HasFile))
                {
                    string ImgNamePhoto = string.Empty;
                    if (ImgUpload.HasFile)
                    {
                        ImgNamePhoto = "P" + Path.GetFileName(ImgUpload.FileName);          //Photo_
                    }
                    UploadPhoto = ImgNamePhoto;
                    ImgUpload = (FileUpload)Session["ImgUpload"]; //exception
                    ImagePathPh.Text = ImgNamePhoto; // 
                }
                else if (ImgUpload.HasFile)
                {
                    string ImgNamePhoto = string.Empty;
                    ImgNamePhoto = "P" + Path.GetFileName(ImgUpload.FileName);          //Photo_
                    Session["ImgUpload"] = ImgUpload;
                    ImgUpload = (FileUpload)Session["ImgUpload"];
                    ImagePathPh.Text = ImgNamePhoto;
                }
                Page.Header.DataBind();
                ImgUpload.Attributes["onchange"] = "UploadFile(this)";

                // Signature
                if (Session["ImgUploadSignature"] == null && ImgUploadSignature.HasFile)
                {
                    string ImgNameSig = string.Empty;
                    ImgNameSig = "S" + Path.GetFileName(ImgUploadSignature.FileName);         //Sig_
                    Session["ImgUploadSignature"] = ImgUploadSignature;
                    ImagePathSig.Text = ImgNameSig;
                    UploadSing1 = ImgNameSig;
                }
                else if (Session["ImgUploadSignature"] != null && (!ImgUploadSignature.HasFile))
                {
                    string ImgNameSig = string.Empty;
                    if (ImgUploadSignature.HasFile)
                    {
                        ImgNameSig = "S" + Path.GetFileName(ImgUploadSignature.FileName);         //Sig_
                    }
                    UploadSing = ImgNameSig;
                    ImgUploadSignature = (FileUpload)Session["ImgUploadSignature"];
                    ImagePathSig.Text = ImgNameSig;
                }
                else if (ImgUploadSignature.HasFile)
                {
                    string ImgNameSig = string.Empty;
                    ImgNameSig = "S" + Path.GetFileName(ImgUploadSignature.FileName);         //Sig_
                    Session["ImgUploadSignature"] = ImgUploadSignature;
                    ImagePathSig.Text = ImgNameSig;
                }
                Page.Header.DataBind();
                ImgUploadSignature.Attributes["onchange"] = "imageSignpreview(this)";

                // Left Thump
                if (Session["ImgUploadThumb"] == null && ImgUploadThumb.HasFile)
                {
                    string ImgNameLth = string.Empty;
                    ImgNameLth = "L" + Path.GetFileName(ImgUploadThumb.FileName);         //Lth_
                    Session["ImgUploadThumb"] = ImgUploadThumb;
                    ImagePathLt.Text = ImgNameLth;
                    UploadLeftTh1 = ImgNameLth;
                }
                else if (Session["ImgUploadThumb"] != null && (!ImgUploadThumb.HasFile))
                {
                    string ImgNameLth = string.Empty;
                    if (ImgUploadThumb.HasFile)
                    {
                        ImgNameLth = "L" + Path.GetFileName(ImgUploadThumb.FileName);         //Lth_
                    }
                    UploadLeftTh = ImgNameLth;
                    ImgUploadThumb = (FileUpload)Session["ImgUploadThumb"];
                    ImagePathLt.Text = ImgNameLth;  //
                }
                else if (ImgUploadThumb.HasFile)
                {
                    string ImgNameLth = string.Empty;
                    ImgNameLth = "L" + Path.GetFileName(ImgUploadThumb.FileName);         //Lth_
                    Session["ImgUploadThumb"] = ImgUploadThumb;
                    ImagePathLt.Text = ImgNameLth;
                }
                Page.Header.DataBind();
                ImgUploadThumb.Attributes["onchange"] = "imageThumbpreview(this)";
            }
            #endregion
            //-------------- added on 06-10-2021 API----------------------/

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
        }
    }

    #region Private Methods
    protected void ActiveInActive(Boolean isactive)
    {
        txtAppName.Enabled = isactive;
        txtFatherName.Enabled = isactive;
        txtMotherName.Enabled = isactive;
        TxtGuardianName.Enabled = isactive;
        txtPinCode.Enabled = isactive;
        TxtSTDcode.Enabled = isactive;
        TxtYearOfPassing.Enabled = isactive;
        TxtAddressLine1.Enabled = isactive;
        TxtAddressLine2.Enabled = isactive;
        TxtAddressLine3.Enabled = isactive;
        TxtCity.Enabled = isactive;
        txtcode.Enabled = isactive;
        txtCorPhoneNo.Enabled = isactive;
        txtDob.Enabled = isactive;
        txtEmailId.Enabled = isactive;
        txtCorMobileNo.Enabled = isactive;

        DdlAccCentre.Enabled = isactive;
        DdlAccState.Enabled = isactive;
        DdlApplied4Exam.Enabled = isactive;
        ddlCategory.Enabled = isactive;
        ddlCorState.Enabled = isactive;
        Ddldistrict.Enabled = isactive;
        DDLeducode.Enabled = isactive;
        DdlExamCentre1.Enabled = isactive;
        DdlExamCentre2.Enabled = isactive;
        DdlExamCentreState1.Enabled = isactive;
        DdlExamCentreState2.Enabled = isactive;
        DdlExamCycle.Enabled = isactive;
        ddlOccupation.Enabled = isactive;
        ddlSalutaionName.Enabled = isactive;
        chkdisclamier.Enabled = isactive;
        chkApaarDeclaration.Enabled = isactive; // amit_apaar_may_2026_start
        RdoAlreadyAppeared4Exam.Enabled = isactive;
        RdoAppliedAs.Enabled = isactive;
        RdDisability.Enabled = isactive;
        DdlDisabilityType.Enabled = isactive;
        TxtDisabilityPercent.Enabled = isactive;
    }
    protected void ActiveInactivePrsnlDetail(Boolean isActive)
    {
        TxtRno.Enabled = isActive;
        ddlSalutaionName.Enabled = isActive;
        txtAppName.Enabled = isActive;
        Rdoownertype.Enabled = isActive;
        txtFatherName.Enabled = isActive;
        txtMotherName.Enabled = isActive;
        TxtGuardianName.Enabled = isActive;
        //ddl_gender.Enabled = isActive;
        ddl_gender.Enabled = isActive;
        txtDob.Enabled = isActive;
        ddlCategory.Enabled = isActive;
        imgDob.Visible = isActive;
        RdoAlreadyAppeared4Exam.Enabled = isActive;
        ImgUpload.Enabled = isActive;
        ImgUploadSignature.Enabled = isActive;
        ImgUploadThumb.Enabled = isActive;
        RdDisability.Enabled = isActive;
        DdlDisabilityType.Enabled = isActive;
        TxtDisabilityPercent.Enabled = isActive;
    }
    protected void ActiveInactivePrsnlDetail2(Boolean isActive)
    {
        TxtRno.Enabled = isActive;
        ddlSalutaionName.Enabled = isActive;
        txtAppName.Enabled = isActive;
        Rdoownertype.Enabled = isActive;
        txtFatherName.Enabled = isActive;
        txtMotherName.Enabled = isActive;
        TxtGuardianName.Enabled = isActive;
        //ddl_gender.Enabled = isActive;
        ddl_gender.Enabled = isActive;
        txtDob.Enabled = isActive;
        ddlCategory.Enabled = isActive;
        imgDob.Visible = isActive;
        RdoAlreadyAppeared4Exam.Enabled = isActive;
        RdDisability.Enabled = isActive;
        DdlDisabilityType.Enabled = isActive;
        TxtDisabilityPercent.Enabled = isActive;
    }
    protected void BindExamCycle(Int32 courseID)
    {
        try
        {
            ListItem lst = new ListItem("--Select Exam Cycle--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                var ExamCycle = context.ExaminationCycles.Where(s => s.CourseID == courseID && s.ID != 7).OrderBy(s => s.Name).Select(s => new { ValueField = s.ID, TextField = s.Name });
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlExamCycle, ExamCycle, lst);
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message, true); }
    }
    protected void BindCandDetail(Course CurrentCourse)
    {
        //----IRDA-POS-EXAM
        try
        {
            ResetAll();
            Aadhaartr.Visible = false;
            UIDtr.Visible = true;
            using (EConnectContext context = new EConnectContext())
            {
                DateTime previousDate = DateTime.Now.AddMonths(-2);
                var applicationName = context.Candidates.Find(CandId);
                var application = context.CourseRegistrationApplications.Where(p => p.CandidateID == CandId && p.CourseCategoryID == CurrentCourse.CourseCategoryID).FirstOrDefault();

                if (applicationName != null)
                {
                    ActiveInactivePrsnlDetail(false);
                    TblFormDetail.Visible = true;
                    //Candidate Personal Detail...
                    ddlSalutaionName.SelectedValue = applicationName.Salutation.ToString();
                    txtAppName.Text = GetInitCap(applicationName.Name.ToString());

                    if (string.IsNullOrEmpty(applicationName.GuardianName) == true && string.IsNullOrWhiteSpace(applicationName.GuardianName) == true)
                    {
                        if (String.IsNullOrEmpty(applicationName.FatherName))
                            txtFatherName.Enabled = true;
                        else
                            txtFatherName.Text = GetInitCap(applicationName.FatherName);
                        if (String.IsNullOrEmpty(applicationName.MotherName))
                            txtMotherName.Enabled = true;
                        else
                            txtMotherName.Text = GetInitCap(applicationName.MotherName);
                    }
                    else
                    {
                        Rdoownertype.SelectedIndex = 1;
                        Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                        ActiveInactivePrsnlDetail(false);
                        TxtGuardianName.Text = GetInitCap(applicationName.GuardianName);
                    }
                    if (!string.IsNullOrEmpty(applicationName.Gender))
                    {
                        //ddl_gender.SelectedValue = applicationName.Gender; 
                        ddl_gender.SelectedValue = applicationName.Gender;
                    }
                    else
                    { //ddl_gender.Enabled = true;
                        ddl_gender.Enabled = true;
                    }

                    txtDob.Text = applicationName.DateOfBirth.ToString("dd-MMM-yyyy");

                    ddlCategory.SelectedValue = applicationName.CastCategoryID.ToString();

                    // Candidate Contact Detail...
                    TxtSTDcode.Text = application.StdNumber != null ? "0" + application.StdNumber.ToString() : "";
                    if (TxtSTDcode.Text == "0")
                        TxtSTDcode.Text = "";
                    if (application.PhoneNumber.HasValue)
                        txtCorPhoneNo.Text = application.PhoneNumber.Value.ToString();
                    //if (application.MobileNumber != null)
                    txtCorMobileNo.Text = application.MobileNumber.ToString();
                    if (application.EmailAddress != null)
                        txtEmailId.Text = application.EmailAddress.ToString();

                    lblphotoShow.Visible = true;
                    lblsignShow.Visible = true;
                    lblthumbshow.Visible = true;
                    lblphotoShow.Text = "Already Exists... ";

                    //-------------- added on 05-10-2021 by vishal----------------------/  photoPreview.Src replace by photoPreview.ImageUrl for all three
                    photoPreview.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String((byte[])applicationName.Photo.BlobFile);
                    lblsignShow.Text = "Already Exists... ";
                    signPreview.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String((byte[])applicationName.Signature.BlobFile);
                    lblthumbshow.Text = "Already Exists... ";
                    thumbPreview.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String((byte[])applicationName.LeftThumbImpression.BlobFile);
                    //-------------- added on 05-10-2021 by vishal----------------------/

                    //Candidate (By default Corespondance here) Address Detail...
                    TxtAddressLine1.Text = GetInitCap(application.CorAddressLine1);
                    TxtAddressLine2.Text = !string.IsNullOrEmpty(application.CorAddressLine2) && !string.IsNullOrWhiteSpace(application.CorAddressLine2) ? GetInitCap(application.CorAddressLine2) : "";
                    TxtAddressLine3.Text = !string.IsNullOrEmpty(application.CorAddressLine3) && !string.IsNullOrWhiteSpace(application.CorAddressLine3) ? GetInitCap(application.CorAddressLine3) : "";
                    if (application.CorCityName != null)
                        TxtCity.Text = GetInitCap(application.CorCityName);
                    if (application.CorStateID != 1)
                    {
                        ddlCorState.SelectedValue = application.CorStateID.ToString();
                        int id1 = Convert.ToInt32(ddlCorState.SelectedValue);
                        BindDistrict(id1);
                        if (application.CorDistrictID.HasValue)
                            Ddldistrict.SelectedValue = application.CorDistrictID.ToString();
                    }
                    txtPinCode.Text = application.CorPinCode.ToString();

                    //Candidate Educational Qualification detail
                    DDLeducode.SelectedValue = application.EducationalQualificationID.ToString();
                    TxtYearOfPassing.Text = application.PassingYear.ToString();
                    DdlAccState.Enabled = false;
                    DdlAccCentre.Enabled = false;


                    if (application.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                        RdoAppliedAs.SelectedValue = "D";
                    else
                    {
                        RdoAppliedAs.SelectedValue = "I";
                        TrAccState.Visible = true;
                        TrAccCentre.Visible = true;
                        DdlAccState.SelectedValue = application.Institute.StateID.ToString();
                        DdlAccState_SelectedIndexChanged(this, null);
                        DdlAccCentre.SelectedValue = application.InstituteID.ToString();
                    }
                    lblApplicantType.Text = RdoAppliedAs.SelectedItem.Text;
                    RdoAppliedAs.Visible = false;
                    lblApplicantType.Visible = true;

                    if (application.UIDType != null)
                    {
                        UidTypeDdl.SelectedValue = application.UIDType.ToString();
                        UidNumberTxt.Text = application.UIDNumber.ToString().ToUpper();
                    }
                    else
                    {
                        UidTypeDdl.Enabled = true;
                        UidNumberTxt.ReadOnly = false;
                    }

                    ImgBtnSearch.Visible = false;
                    ImgBtnReset.Visible = true;

                }
                else
                {
                    TxtRno.Text = "";
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    ImgBtnSearch.Visible = true;
                    ImgBtnReset.Visible = false;
                }
            }
            ;
        }
        catch (Exception ex) { ShowAlert(ex.Message, true); }
    }
    protected void BindState(Int32 CurrentCourseId)
    {
        try
        {
            Int32 withdrawlid = Convert.ToInt32(enmAccreditationStatus.Withdrawal);
            using (EConnectContext context = new EConnectContext())
            {
                var corstate = from s in context.Locations
                               where s.LocationTypeID == 2
                               select new { ValueField = s.ID, TextField = s.Name };
                corstate = corstate.OrderBy(s => s.TextField);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCorState, corstate, new ListItem("--Select One--", "0"));

                var accstate = (from s in context.Locations
                                join i in context.Institutes on s.ID equals i.StateID
                                join a in context.AccreditationDetails on i.ID equals a.InstituteID
                                where s.LocationTypeID == 2
                                && s.ParentLocationID == 1
                                && a.CourseID == CurrentCourseId
                                && a.AccreditationStatusID != withdrawlid
                                select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                accstate = accstate.OrderBy(s => s.TextField);
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlAccState, accstate, new ListItem("--Select One--", "0"));

                var examState = (from g in context.CourseWiseStates
                                 join s in context.Locations on g.StateID equals s.ID
                                 where s.LocationTypeID == 2
                                 && s.ParentLocationID == 1
                                 && g.CourseID == CurrentCourseId
                                 select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                examState = examState.OrderBy(s => s.TextField);
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlExamCentreState1, examState, new ListItem("--Select State--", "0"));
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlExamCentreState2, examState, new ListItem("--Select State--", "0"));
            }
            ;
        }
        catch (Exception ex) { throw ex; }
    }
    protected void BindDistrict(long stateID)
    {
        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                if (stateID != 0)
                {
                    var district = context.Locations.Where(s => s.LocationTypeID == 4 && s.ParentLocationID == stateID).OrderBy(s => s.Name).Select(s => new { ValueField = s.ID, TextField = s.Name });
                    EConnect.Utils.Common.ControlUtility.BindListObject(Ddldistrict, district, lst);
                }
            }
            ;
        }
        catch (Exception ex) { throw ex; }
    }
    protected void BindAccCentre(int stateid)
    {
        try
        {
            Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
            Int32 withdrawlid = Convert.ToInt32(enmAccreditationStatus.Withdrawal);
            Int32 rejectedid = Convert.ToInt16(enmAccreditationStatus.Rejected);
            Int32 deferredid = Convert.ToInt16(enmAccreditationStatus.Deferred);
            Int32 acknowledgeid = Convert.ToInt16(enmAccreditationStatus.Acknowledged);
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                var AccCentre = (from i in context.Institutes
                                 join d in context.AccreditationDetails on i.ID equals d.InstituteID
                                 where i.StateID == stateid && d.CourseID == courseID && d.AccreditationStatusID != withdrawlid
                && d.AccreditationStatusID != rejectedid
                                     && d.AccreditationStatusID != deferredid
                                       && d.AccreditationStatusID != acknowledgeid
                                 //Added 22 May 2020 for instt blocking
                                 && d.tempBlocked == false
                                 && (d.BlockedFromDate >= System.DateTime.Now || d.BlockedFromDate == null)
                                 //
                                 orderby i.Name
                                 select new { ValueField = i.ID, TextField = i.Name + ", " + (!string.IsNullOrEmpty(i.CityName) ? i.CityName : "") + " ( " + d.AccreditationNumber + " ) " });
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlAccCentre, AccCentre, lst);
            }
            ;
        }
        catch (Exception ex) { throw ex; }
    }

    //amit_apaar_may_2026_start

    // added_by_amit_audit_april_2026_start 
    protected void btnClearAadhar_Click(object sender, EventArgs e)
    {
        try
        {
            hfaadhaar.Value = "";
            txtaadhar.Text = "";
            txtaadhar.ReadOnly = false;
            txtaadhar.Style["pointer-events"] = "auto";
            txtaadhar.BackColor = System.Drawing.Color.White;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnSaveAadhar_Click(object sender, EventArgs e)
    {
        try
        {
            string rawAadhaar = txtaadhar.Text.Trim();

            if (!String.IsNullOrEmpty(hfaadhaar.Value) && !String.IsNullOrWhiteSpace(hfaadhaar.Value))
            {
                ShowAlert("Aadhaar Already Saved.");
                return;
            }

            if (rawAadhaar.Length != 12)
            {
                ShowAlert("Please enter valid 12 digit Aadhaar");
                return;
            }

            if (!IsNumeric(rawAadhaar))
            {
                txtaadhar.Text = "";
                ShowAlert("Not valid Aadhaar Number.");
                return;
            }

            string encryptedAadhaar = EncryptDecrypt.EncryptString(rawAadhaar);
            hfaadhaar.Value = encryptedAadhaar;

            txtaadhar.Text = "XXXXXXXX" + rawAadhaar.Substring(8);
            txtaadhar.ReadOnly = true;
            txtaadhar.BackColor = System.Drawing.Color.LightGray;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    // added_by_amit_audit_april_2026_end


    protected void GenerateNewCaptchaImage()
    {
        try
        {
            // ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateRandomNumber(6);
            ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateCaptchaCode(6);
            EConnect.CaptchaImage captcha = new CaptchaImage(ViewState["CaptchCode"].ToString(), 200, 50, "Century Schoolbook");
            imgcap.Src = captcha.ImageSource;
        }
        catch (Exception ex) { ShowAlert(ex.Message, true); }
    }
    protected void GetApplicantType(Course CurrentCourse)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                if (CurrentCourse != null)
                {
                    if (!String.IsNullOrEmpty(CurrentCourse.ApplicantTypeID.ToString()))
                    {
                        RdoAppliedAs.SelectedValue = (CurrentCourse.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct)) ? "D" : "I";

                        lblApplicantType.Text = RdoAppliedAs.SelectedItem.Text;
                        RdoAppliedAs.Visible = false;
                        lblApplicantType.Visible = true;
                        RdoAppliedAs_SelectedIndexChanged(this, null);
                    }
                }
            }
            ;
        }
        catch (Exception ex) { ShowAlert(ex.Message, true); }
    }
    protected bool isValidDob(TextBox txtBox)
    {
        try
        {
            DateTime todaydate = DateTime.Now;
            DateTime Inputdate = Convert.ToDateTime(txtBox.Text);
            int courseID = Convert.ToInt32(Request.QueryString["id"]);

            string catgName = "";
            using (EConnectContext ctx = new EConnectContext())
            {
                var CrsName = ctx.Courses.Find(courseID); catgName = CrsName.CourseCategory.Name;
            }
            int result1 = DateTime.Compare(todaydate, Inputdate);
            int result2 = 0;
            int result3 = 0;
            if (catgName == "IRDA") // IRDA
            { result3 = DateTime.Compare(todaydate.AddYears(-18), Inputdate); }
            else
            { result2 = DateTime.Compare(todaydate.AddYears(-4), Inputdate); }

            if (result2 == -1)
            {
                lblerror.Text = "Invalid date of birth";
                return false;
            }
            else if (result3 == -1)
            {
                lblerror.Text = "Age should be at least 18 years";
                return false;
            }
            else
            { return true; }
        }
        catch (Exception ex) { throw ex; }
    }
    protected bool IsValidForm(string CatgName)
    {
        try
        {
            Int32 ExamId = Convert.ToInt32(ViewState["ExamID"]);
            int resvalue = utility.CheckCandEmailMobile(txtCorMobileNo.Text, txtEmailId.Text, ExamId, 1, -99);
            if (resvalue != 99)
            {
                if (resvalue == 1)
                {

                    ShowAlert("Entered E-mail already exists for 3 records in existing Exam Cycle, Not Saved");
                    txtEmailId.Focus();
                    return false;
                }
                else if (resvalue == 2)
                {

                    ShowAlert("Entered Mobile already exists for 3 records in existing Exam Cycle, Not Saved");
                    txtCorMobileNo.Focus();
                    return false;
                }
                else if (resvalue == 3)
                {
                    // return resvalue;
                    ShowAlert("Email and Mobile already exists for 3 records in existing Exam Cycle, Not Saved");
                    return false;
                }
            }

            if (RdoAlreadyAppeared4Exam.SelectedValue == "Y")
            {
                if (String.IsNullOrWhiteSpace(TxtRno.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Roll Number can not be left blank");
                }
            }
            if (ddlSalutaionName.SelectedValue == "0")
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Please select Salutation");
            }

            if (String.IsNullOrWhiteSpace(txtAppName.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Applicant Name can not be left blank");
            }
            if (!Char.IsLetter(txtAppName.Text, 0))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Applicant Name should start with an alphabet.");
            }

            // if (!Char.IsLetter(txtAppName.Text, txtAppName.Text.Length - 1))
            if (!(Char.IsLetter(txtAppName.Text, txtAppName.Text.Length - 1) || (txtAppName.Text.Trim().EndsWith(".")) || (txtAppName.Text.Trim().EndsWith(")"))))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Applicant Name should end with an alphabet.");
            }

            if (txtAppName.Text.Length == 1)
            {
                txtAppName.Text = "";
                txtAppName.Focus();
                throw new Exception("Applicant Name should be single Character.");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtAppName.Text, "^[a-zA-Z().'\u00FC\u00DC ]*$"))
            {
                txtAppName.Text = "";
                txtAppName.Focus();
                throw new Exception("Applicant Name should be with an English Alphabets(e.g - a-zA-Z)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtAddressLine1.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtAddressLine1.Text = "";
                TxtAddressLine1.Focus();
                throw new Exception("Address Line1 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtAddressLine2.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtAddressLine2.Text = "";
                TxtAddressLine2.Focus();
                throw new Exception("Address Line2 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtAddressLine3.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtAddressLine3.Text = "";
                TxtAddressLine3.Focus();
                throw new Exception("Address Line3 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCity.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
            {
                TxtCity.Text = "";
                TxtCity.Focus();
                throw new Exception("City Name should be with an English Alphabets(e.g - a-zA-Z)");
            }

            //if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorAddressLine1.Text, "^[a-zA-Z0-9 ]*$"))
            //{
            //    TxtCorAddressLine1.Text = "";
            //    TxtCorAddressLine1.Focus();
            //    throw new Exception("Correspondence Address Line1 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            //}

            //if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorAddressLine2.Text, "^[a-zA-Z0-9 ]*$"))
            //{
            //    TxtCorAddressLine2.Text = "";
            //    TxtCorAddressLine2.Focus();
            //    throw new Exception("Correspondence Address Line2 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            //}

            //if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorAddressLine3.Text, "^[a-zA-Z0-9 ]*$"))
            //{
            //    TxtCorAddressLine3.Text = "";
            //    TxtCorAddressLine3.Focus();
            //    throw new Exception("Correspondence Address Line3 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            //}
            //if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorCity.Text, "^[a-zA-Z ]*$"))
            //{
            //    TxtCorCity.Text = "";
            //    TxtCorCity.Focus();
            //    throw new Exception("Correspondence City Name should be with an English Alphabets(e.g - a-zA-Z0-9)");
            //}



            if (Rdoownertype.SelectedValue == "P")//Parents
            {
                if (String.IsNullOrWhiteSpace(txtFatherName.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Please enter Father's Name.");
                }

                if (String.IsNullOrWhiteSpace(txtMotherName.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Please enter Mother's Name");
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtFatherName.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
                {
                    txtFatherName.Text = "";
                    txtFatherName.Focus();
                    throw new Exception("Father Name should be with an English Alphabets(e.g - a-zA-Z)");
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtMotherName.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
                {
                    txtMotherName.Text = "";
                    txtMotherName.Focus();
                    throw new Exception("Mother Name should be with an English Alphabets(e.g - a-zA-Z)");
                }

                if (txtFatherName.Text.Length == 1)
                {
                    txtFatherName.Text = "";
                    txtFatherName.Focus();
                    throw new Exception("Father Name should be single Character.");
                }

                if (txtMotherName.Text.Length == 1)
                {
                    txtMotherName.Text = "";
                    txtMotherName.Focus();
                    throw new Exception("Mother Name should be single Character.");
                }

                if (!String.IsNullOrWhiteSpace(txtFatherName.Text))
                {
                    if (!Char.IsLetter(txtFatherName.Text, 0))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Father Name should start with an alphabet.");
                    }
                    if (!Char.IsLetter(txtFatherName.Text, txtFatherName.Text.Length - 1))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Father Name should end with an alphabet.");
                    }
                }

                if (!String.IsNullOrWhiteSpace(txtMotherName.Text))
                {
                    if (!Char.IsLetter(txtMotherName.Text, 0))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Mother Name should start with an alphabet.");
                    }
                    if (!Char.IsLetter(txtMotherName.Text, txtMotherName.Text.Length - 1))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Mother Name should end with an alphabet.");
                    }
                }
            }
            else if (Rdoownertype.SelectedValue == "G")
            {
                if (String.IsNullOrWhiteSpace(TxtGuardianName.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Please enter Guardian Name.");
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(TxtGuardianName.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
                {
                    TxtGuardianName.Text = "";
                    TxtGuardianName.Focus();
                    throw new Exception("Guardian Name should be with an English Alphabets(e.g - a-zA-Z)");
                }

                if (TxtGuardianName.Text.Length == 1)
                {
                    TxtGuardianName.Text = "";
                    TxtGuardianName.Focus();
                    throw new Exception("Guardian Name should be single Character.");
                }

                if (!String.IsNullOrWhiteSpace(TxtGuardianName.Text))
                {
                    if (!Char.IsLetter(TxtGuardianName.Text, 0))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Guardian Name should start with an alphabet.");
                    }

                    if (!Char.IsLetter(TxtGuardianName.Text, TxtGuardianName.Text.Length - 1))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Guardian Name should end with an alphabet.");
                    }
                }
            }

            if (String.IsNullOrWhiteSpace(txtDob.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Date Of Birth can not be left blank");
            }
            if (!IsDate(txtDob.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Invalid Date of Birth");
            }

            if (!isValidDob(txtDob))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                //throw new Exception("Invalid Date of Birth. Candidate should be at least 4 years.");
                throw new Exception("Invalid Date of Birth. " + lblerror.Text);
            }
            if (ddlCategory.SelectedValue == "0")
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Please select Cast Category");
            }
            if (ddlOccupation.SelectedValue == "0")
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Please select Occupation");
            }

            if (ddlOccupation.SelectedValue == "5") //if occupation is gujarat gov.
            {
                if (String.IsNullOrWhiteSpace(Txtdept.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Department Name can not be left blank");
                }

                if (String.IsNullOrWhiteSpace(Txtempcode.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Employee Code can not be left blank");
                }

                if (String.IsNullOrWhiteSpace(txtdesg.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Designation can not be left blank");
                }

                if (String.IsNullOrWhiteSpace(txtpostcity.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Place of Posting can not be left blank");
                }

                if (String.IsNullOrWhiteSpace(txtDojoin.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Date Of Joining can not be left blank");
                }
                if (!IsDate(txtDojoin.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Invalid Date Of Joining");
                }

                if (String.IsNullOrWhiteSpace(txtDoretment.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Date Of Retirement can not be left blank");
                }
                if (!IsDate(txtDoretment.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Invalid Date of Retirement");
                }

                DateTime dtojoin = Convert.ToDateTime(txtDojoin.Text);
                DateTime dtretire = Convert.ToDateTime(txtDoretment.Text);

                if (dtojoin > dtretire)
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Date of Joining cannot be greater than Date of Retirement");
                }

            }
            if (RdDisability.SelectedValue == "Y")
            {
                if (DdlDisabilityType.SelectedValue == "0")
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Please select disability type");
                }
                if (String.IsNullOrWhiteSpace(TxtDisabilityPercent.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Please enter Disability percent");
                }
            }
            if (String.IsNullOrWhiteSpace(TxtSTDcode.Text) == false && string.IsNullOrWhiteSpace(txtCorPhoneNo.Text) == false)
            {
                if (TxtSTDcode.Text.Contains('.'))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Dot(.) not allowed in STD Code");
                }
                if (txtCorPhoneNo.Text.Contains('.'))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Dot(.) not allowed in Phone Number");
                }
                if (!IsNumeric(TxtSTDcode.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Invalid STD Code");
                }
                if (!IsNumeric(txtCorPhoneNo.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Invalid Phone Number");
                }
            }
            if (String.IsNullOrWhiteSpace(txtCorMobileNo.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Mobile Number can not be left blank");
            }
            if (txtCorMobileNo.Text.Contains('.'))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Dot(.) not allowed in Mobile Number");
            }
            if (!IsNumeric(txtCorMobileNo.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Invalid Mobile Number");
            }
            if (String.IsNullOrWhiteSpace(txtEmailId.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Email Address can not be left blank");
            }
            if (!IsValidEmailAddress(txtEmailId.Text.Trim()))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Invalid Email Address");
            }

            if (String.IsNullOrWhiteSpace(TxtAddressLine1.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Address Line1 can not be left blank");
            }
            if (String.IsNullOrWhiteSpace(TxtAddressLine2.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Address Line2 can not be left blank");
            }
            if (String.IsNullOrWhiteSpace(TxtCity.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("City can not be left blank");
            }
            if (ddlCorState.SelectedValue == "0")
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Please select State in address section");
            }
            if (Ddldistrict.SelectedValue == "0")
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Please select District in address section");
            }
            if (String.IsNullOrWhiteSpace(txtPinCode.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Pin Code can not be left blank");
            }
            if (!IsNumeric(txtPinCode.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Invalid Pin Code");
            }
            if (DDLeducode.SelectedValue == "0")
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Please select Highest Education");
            }
            if (String.IsNullOrWhiteSpace(TxtYearOfPassing.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Passing Year can not be left blank");
            }
            if (!IsNumeric(TxtYearOfPassing.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Invalid Passing Year");
            }
            Int32 PassingYear = Convert.ToInt32(TxtYearOfPassing.Text);
            Int32 birthYear = Convert.ToDateTime(txtDob.Text).Year;
            if (PassingYear > DateTime.Now.Year || PassingYear <= (birthYear + 4))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Invalid Passing Year");
            }
            if (DdlExamCycle.SelectedValue == "0")
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Please select Exam Cycle");
            }
            //if (DdlApplied4Exam.SelectedValue == "0")
            //{
            //    GenerateNewCaptchaImage();
            //    txtcode.Text = "";
            //    throw new Exception("Please select Applied for Exam");
            //}
            if (RdoAppliedAs.SelectedValue == "I")//Institute
            {
                if (DdlAccState.SelectedValue == "0")
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Please select State of Accredited Institite");
                }
                if (DdlAccCentre.SelectedValue == "0")
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Please select Accredited Institite");
                }
            }
            if (DdlExamCentre1.SelectedValue == "0")
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Please select Exam Centre 1");
            }
            if (DdlExamCentre2.SelectedValue == "0")
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Please select Exam Centre 2");
            }

            if (DdlExamCentre1.SelectedValue == DdlExamCentre2.SelectedValue)
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Both Exam Centre Choices cannot be same.");
            }



            //amit aadhar apaaar
            if (!String.IsNullOrEmpty(txtaadhar.Text))
            {
                string decryptAadhaar = "";
                if (!String.IsNullOrEmpty(hfaadhaar.Value))
                {
                    decryptAadhaar = EncryptDecrypt.DecryptString(hfaadhaar.Value);

                    if (!System.Text.RegularExpressions.Regex.IsMatch(decryptAadhaar, @"^[0-9]{12}$"))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Not valid Aadhar Number.");
                    }

                    if (!IsNumeric(decryptAadhaar))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Not valid Aadhaar Number.");
                    }

                }
                else
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("if Aadhaar number is entered. Please Click Save Aadhar button");
                }
            }


            // amit_apaar_api_changes_may_2026_end
            if (String.IsNullOrEmpty(txtapaar.Text))
            {
                GenerateNewCaptchaImage();
                throw new Exception("Apaar ID  should not be Blank.");

            }

            if (txtapaar.Text.Length != 12)
            {
                txtapaar.Text = "";
                txtapaar.Focus();
                throw new Exception("Invalid ApaarID.");
            }


            //amit_apaar_may_2026_start
            if (TrConsentRelation.Visible)
            {
                if (string.IsNullOrEmpty(ddlConsentRelation.SelectedValue))
                {
                    throw new Exception("Please select Consent Relation");

                }
            }
            if (TrAuthMode.Visible)
            {
                if (string.IsNullOrEmpty(ddlAuthMode.SelectedValue) || ddlAuthMode.SelectedValue == "0")
                {
                    throw new Exception("Please select Authentication Mode");
                }
            }

            if (string.IsNullOrWhiteSpace(txtAuthenticationIdNo.Text.Trim()))
            {
                throw new Exception("Authentication ID cannot be blank");
            }
            else
            {
                bool isValid = false;
                switch (ddlAuthMode.SelectedValue)
                {
                    case "3": // PAN 
                        isValid = Regex.IsMatch(txtAuthenticationIdNo.Text.Trim(), @"^[A-Za-z0-9]{10}$");
                        if (!isValid) throw new Exception("Enter Valid 10-digit PAN card number only. No space allowed");
                        break;
                    case "4": // DL
                        isValid = Regex.IsMatch(txtAuthenticationIdNo.Text.Trim(), @"^[A-Za-z0-9/-]{10,15}$");
                        if (!isValid) throw new Exception("Enter Valid DL card number only.No space allowed");
                        break;
                    case "5": // Passport
                        isValid = Regex.IsMatch(txtAuthenticationIdNo.Text.Trim(), @"^[A-Za-z0-9]{8}$");
                        if (!isValid) throw new Exception("Enter Valid 8-digit Passport ID number only. No space allowed");
                        break;
                    case "6": // EPIC
                        isValid = Regex.IsMatch(txtAuthenticationIdNo.Text.Trim(), @"^[A-Za-z0-9]{10}$");
                        if (!isValid) throw new Exception("Enter Valid EPIC card number only. Only numbers and alphabets are allowed. No space allowed");
                        break;
                }
            }


            if (string.IsNullOrWhiteSpace(txtConsentPlace.Text))
            {
                txtConsentPlace.Focus();
                throw new Exception("Consent Place cannot be blank");
            }

            if (!Regex.IsMatch(
                    txtConsentPlace.Text.Trim(),
                    @"^(?=.{5,30}$)[A-Za-z]+(?:'[A-Za-z]+)*(?: [A-Za-z]+(?:'[A-Za-z]+)*)?$"))
            {
                throw new Exception("Consent Place must contain one or two words, using only letters and apostrophes.");
            }

            if (chkApaarDeclaration.Checked != true)
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Please check the Apaar Declaration Statement");
            }

            //amit_apaar_may_2026_end

            if (CatgName != "IRDA")
            {

                //-------------- added on August-2021 by vishal----------------------/
                // Photo
                if (btnSave.Text == "Submit")
                {
                    if (ImgUpload.HasFile == false && !string.IsNullOrEmpty(lblphotoShow.Text))  // there is some text in lblphotoShow
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        photoPreview.ImageUrl = "";
                        lblphotoShow.Text = "Photo can not be left blank.";
                        lblphotoShow.Visible = true;
                        throw new Exception("Photo can not be left blank.");
                    }
                }

                if (ImgUpload.HasFile == false && string.IsNullOrEmpty(lblphotoShow.Text))  // there is no text in lblphotoShow
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    photoPreview.ImageUrl = "";
                    lblphotoShow.Text = "Photo can not be left blank.";
                    lblphotoShow.Visible = true;
                    throw new Exception("Photo can not be left blank.");
                }
                //16.02.2022
                if (ImgUpload.HasFile && !System.Text.RegularExpressions.Regex.IsMatch(Convert.ToString(ImgUpload.FileName), "^[a-zA-Z0-9()-_.\u00FC\u00DC ]*$"))
                {

                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    photoPreview.ImageUrl = "";
                    lblphotoShow.Text = "Photo file name should be English Alphabets and Numbers only(like a-zA-Z0-9).";
                    lblphotoShow.Visible = true;
                    throw new Exception("Photo file name should be English Alphabets and Numbers only(like a-zA-Z0-9).");
                }
                //
                if (ImgUpload.HasFile && ImgUpload.FileName.Length > 49)
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    photoPreview.ImageUrl = "";
                    lblphotoShow.Text = "Photo file name should be less than 44 characters.";
                    lblphotoShow.Visible = true;
                    throw new Exception("Photo file name should be less than 44 characters.");
                }
                if (ImgUpload.HasFile && !isvalidFileExtension1(ImgUpload))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    photoPreview.ImageUrl = "";
                    lblphotoShow.Text = "Only jpeg/jpg extensions image is allowed.";
                    lblphotoShow.Visible = true;
                    throw new Exception("Only jpeg/jpg extensions image is allowed.");
                }
                if (ImgUpload.HasFile && !isvalidFileSize1(ImgUpload, 5120, 51200))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    photoPreview.ImageUrl = "";
                    lblphotoShow.Text = "Photograph file size should be 5 KB to 50 KB.";
                    lblphotoShow.Visible = true;
                    throw new Exception("Photograph file size should be 5 KB to 50 KB.");
                }
                // Signature lblsignShow
                if (btnSave.Text == "Submit")
                {
                    if (ImgUploadSignature.HasFile == false && !string.IsNullOrEmpty(lblsignShow.Text))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        signPreview.ImageUrl = "";
                        lblsignShow.Visible = true;
                        lblsignShow.Text = "Signature can not be left blank.";
                        throw new Exception("Signature can not be left blank.");
                    }
                }
                if (ImgUploadSignature.HasFile == false && string.IsNullOrEmpty(lblsignShow.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    signPreview.ImageUrl = "";
                    lblsignShow.Visible = true;
                    lblsignShow.Text = "Signature can not be left blank.";
                    throw new Exception("Signature can not be left blank.");
                }
                //16.02.2022
                if (ImgUploadSignature.HasFile && !System.Text.RegularExpressions.Regex.IsMatch(Convert.ToString(ImgUploadSignature.FileName), "^[a-zA-Z0-9()-_.\u00FC\u00DC ]*$"))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    signPreview.ImageUrl = "";
                    lblsignShow.Visible = true;
                    lblsignShow.Text = "Signature file name should be English Alphabets and Numbers only(like a-zA-Z0-9).";
                    throw new Exception("Signature file name should be English Alphabets and Numbers only(like a-zA-Z0-9).");
                }
                //
                if (ImgUploadSignature.HasFile && ImgUploadSignature.FileName.Length > 49)
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    signPreview.ImageUrl = "";
                    lblsignShow.Visible = true;
                    lblsignShow.Text = "Signature file name should be less than 44 characters.";
                    throw new Exception("Signature file name should be less than 44 characters.");
                }
                if (ImgUploadSignature.HasFile && !isvalidFileExtension1(ImgUploadSignature))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    signPreview.ImageUrl = "";
                    lblsignShow.Visible = true;
                    lblsignShow.Text = "Only jpeg/jpg extensions image is allowed.";
                    throw new Exception("Only jpegjpg extensions image is allowed.");
                }
                if (ImgUploadSignature.HasFile && !isvalidFileSize1(ImgUploadSignature, 5120, 20480))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    signPreview.ImageUrl = "";
                    lblsignShow.Visible = true;
                    lblsignShow.Text = "Signature file size should be 5 KB to 20 KB.";
                    throw new Exception("Signature file size should be 5 KB to 20 KB.");
                }

                //Left Thumb
                if (btnSave.Text == "Submit")
                {
                    if (ImgUploadThumb.HasFile == false && !string.IsNullOrEmpty(lblthumbshow.Text))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        thumbPreview.ImageUrl = "";
                        lblthumbshow.Visible = true;
                        lblthumbshow.Text = "Left thumb impression can not be left blank.";
                        throw new Exception("Left thumb Impression can not be left blank.");
                    }
                }

                if (ImgUploadThumb.HasFile == false && string.IsNullOrEmpty(lblthumbshow.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    thumbPreview.ImageUrl = "";
                    lblthumbshow.Visible = true;
                    lblthumbshow.Text = "Left thumb impression can not be left blank.";
                    throw new Exception("Left thumb impression can not be left blank.");
                }
                //
                if (ImgUploadThumb.HasFile && !System.Text.RegularExpressions.Regex.IsMatch(Convert.ToString(ImgUploadThumb.FileName), "^[a-zA-Z0-9()-_.\u00FC\u00DC ]*$"))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    thumbPreview.ImageUrl = "";
                    lblthumbshow.Visible = true;
                    lblthumbshow.Text = "Left Thumb Impression file name should be English Alphabets and Numbers only(like a-zA-Z0-9).";
                    throw new Exception("Left Thumb Impression file name should be English Alphabets and Numbers only(like a-zA-Z0-9).");
                }
                //
                if (ImgUploadThumb.HasFile && ImgUploadThumb.FileName.Length > 49)
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    thumbPreview.ImageUrl = "";
                    lblthumbshow.Visible = true;
                    lblthumbshow.Text = "Left thumb impression file name should be less than 44 characters.";
                    throw new Exception("Left thumb impression file name should be less than 44 characters.");
                }
                if (ImgUploadThumb.HasFile && !isvalidFileExtension1(ImgUploadThumb))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    thumbPreview.ImageUrl = "";
                    lblthumbshow.Visible = true;
                    lblthumbshow.Text = "Only jpeg/jpg extensions image is allowed.";
                    throw new Exception("Only jpeg extensions image is allowed.");
                }
                if (ImgUploadThumb.HasFile && !isvalidFileSize1(ImgUploadThumb, 5120, 20480))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    thumbPreview.ImageUrl = "";
                    lblthumbshow.Visible = true;
                    lblthumbshow.Text = "Left thump impression file size should be 5 KB to 20 KB.";
                    throw new Exception("Left thump impression file size should be 5 KB to 20 KB.");
                }
                //-------------- added on August-2021 by vishal----------------------/


            }
            if (String.IsNullOrWhiteSpace(txtcode.Text))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Captcha Code can not be left blank");
            }


            if (txtcode.Text != ViewState["CaptchCode"].ToString())
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Captcha Code does not match with the code shown in image above.");
            }


            if (chkdisclamier.Checked != true)
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Please check the Declaration Statement");
            }
            if (!IsNumeric(txtapaar.Text))
            {
                // lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtapaar.Text = "";
                // lblerror.Text = "Invalid Apaar, Please generate and enter";
                throw new Exception("Invalid Apaar, Please generate and enter");

            }



            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ResetAll()
    {
        try
        {

            txtAppName.Text = "";
            txtFatherName.Text = "";
            txtMotherName.Text = "";
            TxtGuardianName.Text = "";
            txtPinCode.Text = "";
            TxtSTDcode.Text = "";
            TxtYearOfPassing.Text = "";
            TxtAddressLine1.Text = "";
            TxtAddressLine2.Text = "";
            TxtAddressLine3.Text = "";
            TxtCity.Text = "";
            txtcode.Text = "";
            txtCorPhoneNo.Text = "";
            txtDob.Text = "";
            txtEmailId.Text = "";

            DdlAccCentre.SelectedValue = "0";
            DdlAccState.SelectedValue = "0";
            DdlApplied4Exam.SelectedValue = "0";
            ddlCategory.SelectedValue = "0";
            ddlCorState.SelectedValue = "0";
            DDLeducode.SelectedValue = "0";
            DdlExamCentre1.SelectedValue = "0";
            DdlExamCentre2.SelectedValue = "0";
            DdlExamCentreState1.SelectedValue = "0";
            DdlExamCentreState2.SelectedValue = "0";
            DdlExamCycle.SelectedValue = "0";
            ddlOccupation.SelectedValue = "0";
            ddlSalutaionName.SelectedValue = "0";
            RdDisability.SelectedValue = "N";
            DdlDisabilityType.SelectedValue = "0";
            TxtDisabilityPercent.Text = "";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowApplicatantType()
    {
        try
        {
            if (RdoAppliedAs.SelectedValue == "I")//Institute
            {
                TrAccState.Visible = true;
                TrAccCentre.Visible = true;
                TrAccState.Attributes.Add("class", "gdrow1");
                TrAccCentre.Attributes.Add("class", "gdalternate1");
                TrExamCentre1.Attributes.Add("class", "gdalternate1");
                TrExamCentre2.Attributes.Add("class", "gdrow1");
            }
            if (RdoAppliedAs.SelectedValue == "D")//Direct
            {
                TrAccState.Visible = false;
                TrAccCentre.Visible = false;
                TrExamCentre1.Attributes.Add("class", "gdalternate1");
                TrExamCentre2.Attributes.Add("class", "gdrow1");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowData()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int64 applID = Convert.ToInt64(Request.QueryString["Appid"]);
                //This Query will get all the information of Applied Canditate by generated Application id
                var application = context.CertificateExamApplications.Find(applID);
                if (application != null)
                {
                    //if Already appeared for Exam then it will show Previous Roll Number and Previous exam Cycle else not
                    if (application.AlreadyApplied)
                    {
                        RdoAlreadyAppeared4Exam.SelectedValue = "Y";
                        TxtRno.Text = application.PreviousRollNumber.ToString();
                        ImgBtnSearch_Click(ImgBtnSearch, null);
                        TrPreExamRno.Visible = true;
                        ImgBtnSearch.Visible = false;
                        ImgBtnReset.Visible = false;
                    }
                    else
                    {
                        RdoAlreadyAppeared4Exam.SelectedValue = "N";
                        TrPreExamRno.Visible = false;
                    }

                    //Candidate Personal Detail...
                    ddlSalutaionName.SelectedValue = application.Salutation.ToString();
                    txtAppName.Text = GetInitCap(application.Name.ToString());
                    if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName) == true)
                    {
                        trfather.Visible = true;
                        trmother.Visible = true;
                        trguardian.Visible = false;
                        Rdoownertype.SelectedValue = "P";
                        Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                        txtFatherName.Text = GetInitCap(application.FatherName);
                        txtMotherName.Text = GetInitCap(application.MotherName);
                    }
                    else
                    {
                        trfather.Visible = false;
                        trmother.Visible = false;
                        trguardian.Visible = true;
                        Rdoownertype.SelectedValue = "G";
                        Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                        divgurdian.Style.Add("display", "none");
                        ActiveInActive(true);
                        if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                        {
                            DdlExamCentre1.Enabled = false;

                            DdlExamCentreState1.Enabled = false;

                        }
                        TxtGuardianName.Text = GetInitCap(application.GuardianName);
                    }

                    //ddl_gender.SelectedValue = application.Gender;
                    //ddl_gender.Enabled = false;
                    ddl_gender.SelectedValue = application.Gender;
                    ddl_gender.Enabled = false;
                    txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");
                    ddlCategory.SelectedValue = application.CastCategoryID.ToString();
                    ddlOccupation.SelectedValue = application.OccupationID.ToString();

                    RdDisability.SelectedValue = (application.IsDisability == false) ? "N" : "Y";
                    if (application.IsDisability == true)
                    {
                        DdlDisabilityType.SelectedValue = application.DisabilityTypeID.Value.ToString();
                        TxtDisabilityPercent.Text = application.DisabilityPercentage.Value.ToString();
                    }
                    //jk sah on 13-Jan-2021------
                    if (application.Is_EWS.ToString() == "True")
                    {
                        RdisEWS.SelectedValue = "Y";
                    }
                    else
                    {
                        RdisEWS.SelectedValue = "N";
                    }
                    //----------------------
                    //checking the occuption if it is gujarat govt.
                    if (application.OccupationID == 5)
                    {
                        Txtdept.Text = GetInitCap(application.Department); ;
                        txtdesg.Text = GetInitCap(application.Designation); ;
                        Txtempcode.Text = GetInitCap(application.EmployeeCode); ;
                        txtDojoin.Text = application.DateofJoining.HasValue ? application.DateofJoining.Value.ToString("dd-MMM-yyyy") : "N/A";
                        txtDoretment.Text = application.DateofRetirement.HasValue ? application.DateofRetirement.Value.ToString("dd-MMM-yyyy") : "N/A";
                        txtpostcity.Text = GetInitCap(application.PostingCity);
                        //ddlOccupation_SelectedIndexChanged(ddlOccupation, EventArgs.Empty);
                    }

                    // Candidate Contact Detail...

                    TxtSTDcode.Text = application.PhoneNumber.HasValue && application.PhoneNumber != 0 ? "0" + application.StdNumber.ToString() : "";
                    txtCorPhoneNo.Text = application.PhoneNumber.ToString();
                    txtCorMobileNo.Text = application.MobileNumber.ToString();
                    txtEmailId.Text = application.EmailAddress.ToString();

                    //Candidate (By default Corespondance here) Address Detail...

                    TxtAddressLine1.Text = GetInitCap(application.CorAddressLine1);
                    TxtAddressLine2.Text = !string.IsNullOrEmpty(application.CorAddressLine2) && !string.IsNullOrWhiteSpace(application.CorAddressLine2) ? GetInitCap(application.CorAddressLine2) : "";
                    TxtAddressLine3.Text = !string.IsNullOrEmpty(application.CorAddressLine3) && !string.IsNullOrWhiteSpace(application.CorAddressLine3) ? GetInitCap(application.CorAddressLine3) : "";
                    TxtCity.Text = GetInitCap(application.CorCityName);
                    ddlCorState.SelectedValue = application.CorStateID.ToString();
                    int id1 = Convert.ToInt32(ddlCorState.SelectedValue);
                    BindDistrict(id1);
                    Ddldistrict.SelectedValue = application.CorDistrictID.ToString();
                    txtPinCode.Text = application.CorPinCode.ToString();


                    //Candidate Educational Qualification detail                  
                    DDLeducode.SelectedValue = application.EducationalQualificationID.ToString();
                    DdlExamCycle.SelectedValue = application.Exam.ExaminationCycleID.ToString();
                    DdlExamCycle_SelectedIndexChanged(DdlExamCycle, EventArgs.Empty);
                    DdlApplied4Exam.SelectedValue = application.ExamID.ToString();
                    DdlApplied4Exam_SelectedIndexChanged(DdlApplied4Exam, EventArgs.Empty);
                    TxtYearOfPassing.Text = application.PassingYear.ToString();

                    //amit_apaar_may_2026_start
                    if (application.ApaarRequestID != null && application.ApaarRequestID != 0)
                    {
                        ddlSalutaionName.Enabled = false;
                        txtAppName.Enabled = false;

                        if (Rdoownertype.SelectedValue == "G")
                        {
                            TxtGuardianName.Enabled = false;
                        }
                        else
                        {
                            txtFatherName.Enabled = false;
                            txtMotherName.Enabled = false;
                        }



                        Rdoownertype.Enabled = false;
                        txtDob.Enabled = false;
                        ddl_gender.Enabled = false;

                        Int64 apaarRequestID = Convert.ToInt64(application.ApaarRequestID);

                        var apaarData = context.ApaarRequest
                            .Where(a => a.ID == apaarRequestID)
                            .FirstOrDefault();

                        if (apaarData != null)
                        {
                            // Apaar ID
                            if (!String.IsNullOrEmpty(application.apaarID))
                            {
                                txtapaar.Text = EncryptDecrypt.DecryptString(application.apaarID);
                            }



                            txtapaar.Enabled = false;
                            //txtapaar.Style["pointer-events"] = "none";

                            // Provider Present
                            txtIsProviderPresent.Text = "True";
                            txtIsProviderPresent.Enabled = false;

                            // Consent Relation
                            ddlConsentRelation.Items.FindByText(apaarData.consentRelation).Selected = true;
                            ddlConsentRelation.Enabled = false;

                            // Provider Name
                            txtproviderName.Text =
                                !String.IsNullOrEmpty(apaarData.providerName)
                                ? apaarData.providerName
                                : "";

                            txtproviderName.Enabled = false;

                            // Auth Mode
                            // logic to populate 
                            DateTime todaydate = DateTime.Now;
                            DateTime Inputdate = Convert.ToDateTime(txtDob.Text);
                            int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);
                            BindAuthMode(countAge);

                            lblApaarDeclaration.Text = apaarData.undertakingTextChecked;
                            lblApaarDeclaration.Visible = true;
                            tdApaarDeclaration.Visible = true;

                            ddlAuthMode.SelectedValue =
                                apaarData.authModeID.HasValue
                                ? apaarData.authModeID.Value.ToString()
                                : "0";

                            ddlAuthMode.Enabled = false;
                            ddlAuthMode_SelectedIndexChanged(null, null);

                            // Authentication ID No
                            txtAuthenticationIdNo.Text =
                                !String.IsNullOrEmpty(apaarData.authModeIDNo)
                                ? apaarData.authModeIDNo
                                : "";

                            txtAuthenticationIdNo.Enabled = false;

                            //txtDob_TextChanged(null, null);

                            txtapaar.Enabled = false;
                            chkApaarDeclaration.Checked = true;
                            chkApaarDeclaration.Enabled = false;
                            // Consent Date
                            if (apaarData != null && apaarData.consentDate.HasValue)
                            {
                                txtConsentDate.Text = apaarData.consentDate.Value.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                txtConsentDate.Text = "";
                            }

                            txtConsentDate.Enabled = false;

                            // Consent Time
                            txtConsentTime.Text =
           apaarData.consentTime.HasValue
           ? apaarData.consentTime.Value.ToString(@"hh\:mm")
           : "";

                            txtConsentTime.Enabled = false;

                            // Consent Place
                            txtConsentPlace.Text =
                                !String.IsNullOrEmpty(apaarData.consentPlace)
                                ? apaarData.consentPlace
                                : "";

                            txtConsentPlace.Enabled = false;
                            //  bindDeclaration(application.ApplicantTypeID.ToString(), countAge);

                        }
                    }
                    //amit_apaar_may_2026_end

                    //showing photo, sign,thumb name
                    if (application.Photo != null)
                    {
                        lblphotoShow.Visible = true;
                        lblphotoShow.Text = "Already Exists... ";
                        lblphoto.Text = "Upload Photo / फोटो अपलोड ";
                        photoPreview.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String((byte[])application.Photo);  // by vishal 05-10-2021

                    }
                    else
                    {
                        lblphotoShow.Visible = false;
                        lblphotoShow.Text = "";
                        lblphoto.Text = "Upload Photo / फोटो अपलोड <font color='RED'>*</font>";
                    }

                    if (application.Signature != null)
                    {
                        lblsignShow.Visible = true;
                        lblsignShow.Text = "Already Exists... ";
                        lblsign.Text = "Upload Signature / हस्ताक्षर अपलोड ";
                        signPreview.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String((byte[])application.Signature);   // by vishal 05-10-2021
                    }
                    else
                    {
                        lblsignShow.Visible = false;
                        lblsignShow.Text = "";
                        lblsign.Text = "Upload Signature / हस्ताक्षर अपलोड <font color='RED'>*</font>";
                    }

                    if (application.LeftThumb != null)
                    {
                        lblthumbshow.Visible = true;
                        lblthumbshow.Text = " Already Exists... ";
                        lblThumb.Text = "Upload Left Hand Thumb Impression / बांए हाथ के अंगूठे का निशान अपलोड ";
                        thumbPreview.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String((byte[])application.LeftThumb);   // by vishal 05-10-2021
                    }
                    else
                    {
                        lblthumbshow.Visible = false;
                        lblthumbshow.Text = "";
                        lblThumb.Text = "Upload Left Hand Thumb Impression / बांए हाथ के अंगूठे का निशान अपलोड <font color='RED'>*</font>";
                    }

                    //Making Enclosures Configurable
                    if (context.Exams.Where(s => s.ID == application.ExamID).FirstOrDefault().IsBatchProcessable)
                    {
                        trenclosure.Visible = true;
                        trenclosure1.Visible = true;
                        tdencheading.InnerText = " 8. Enclosures / भेजें ";
                        tddeclarartion.InnerText = " 9. Declaration / घोषणा ";
                    }
                    else
                    {
                        trenclosure.Visible = false;
                        trenclosure1.Visible = false;
                        tddeclarartion.InnerText = " 8. Declaration / घोषणा ";
                    }

                    //Making Declaration
                    if (!string.IsNullOrEmpty(application.Salutation))
                    {
                        LblName.Text = txtAppName.Text;
                        LblhName.Text = txtAppName.Text;
                        if (application.Salutation.Trim() == "Mrs.".Trim() || application.Salutation.Trim() == "Miss".Trim() || application.Salutation.Trim() == "Ms.".Trim())
                        {
                            if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName))
                            {
                                Lblsalutation.Text = " daughter  of ";
                                Lblhsalutation.Text = " की पुत्री ";
                                Lblhdectype.Text = " करती ";
                            }
                            else
                            {
                                Lblhsalutation.Text = "की देखभाल";
                                Lblsalutation.Text = "care of";
                                Lblhdectype.Text = " करती ";
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName))
                            {
                                Lblsalutation.Text = " son  of ";
                                Lblhsalutation.Text = " का पुत्र ";
                                Lblhdectype.Text = " करता ";
                            }
                            else
                            {
                                Lblhsalutation.Text = "की देखभाल";
                                Lblsalutation.Text = "care of";
                                Lblhdectype.Text = " करता ";
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName))
                    {
                        LblDecFname.Text = " and Shri " + GetInitCap(application.FatherName);
                        LblDechfName.Text = " और श्री " + GetInitCap(application.FatherName);
                        LblDecMName.Text = " Smt " + GetInitCap(application.MotherName);
                        LblDechmName.Text = " श्रीमती " + GetInitCap(application.MotherName);
                    }
                    else
                    {
                        LblDecFname.Text = "";
                        LblDechfName.Text = "";
                        LblDecMName.Text = "";
                        LblDechmName.Text = "";
                    }

                    lbldeccoursecode.Text = application.Course.Code.ToUpper();

                    //If Applied as Institute then This code will get all the Location Details of Institute
                    if (application.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute))
                    {
                        //var intituteDetail = context.Institutes.Find(application.InstituteID);

                        RdoAppliedAs.SelectedValue = "I"; // Applied as institute
                        ShowApplicatantType();
                        DdlAccCentre.SelectedValue = Convert.ToString(application.InstituteID);
                        DdlAccState.SelectedValue = Convert.ToString(application.Institute.StateID);
                        BindAccCentre(Convert.ToInt32(DdlAccState.SelectedValue));
                    }
                    else if (application.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                    {
                        RdoAppliedAs.SelectedValue = "D"; // Applied as Direct
                        ShowApplicatantType();
                    }
                    DdlExamCentreState1.SelectedValue = application.ExamCenter1.StateID.ToString();
                    DdlExamCentreState1_SelectedIndexChanged(DdlExamCentreState1, EventArgs.Empty);
                    DdlExamCentre1.SelectedValue = application.ExamCenter1ID.ToString();
                    DdlExamCentreState2.SelectedValue = application.ExamCenter2.StateID.ToString();
                    DdlExamCentreState2_SelectedIndexChanged(DdlExamCentreState2, EventArgs.Empty);
                    DdlExamCentre2.SelectedValue = application.ExamCenter2ID.ToString();

                    //aadhar card details
                    //txtaadhar.Text = (application.AadharNumber.HasValue) ? application.AadharNumber.Value.ToString() : "";

                    //added_by_amit_audit_april_2026_start
                    if (application.AadharNumber.HasValue)
                    {
                        //aadhaar change
                        string encryptedAadhaar = EncryptDecrypt.EncryptString(application.AadharNumber.Value.ToString());
                        hfaadhaar.Value = encryptedAadhaar;
                        txtaadhar.Text = "XXXXXXXX" + application.AadharNumber.Value.ToString().Substring(8);
                        txtaadhar.ReadOnly = true;
                        txtaadhar.Style["pointer-events"] = "none";
                        txtaadhar.BackColor = System.Drawing.Color.LightGray;

                    }
                    else
                    {
                        txtaadhar.Text = "";
                        txtaadhar.ReadOnly = false;
                        txtaadhar.Style["pointer-events"] = "auto";
                    }
                    // added_by_amit_audit_april_2026_end

                    if (application.CourseCategory.Name == "IRDA")
                    {
                        ActiveInactivePrsnlDetail(false);
                    }
                }
            }
            ;
        }
        catch (Exception ex) { throw ex; }
    }
    protected void ShowAttachment(Int32 CourseCategoryId)
    {
        try
        {
            // DateTime todayDate = DateTime.Now;            
            Int32 DownloadableTypeID = Convert.ToInt32(enmDownloadableType.Notice);
            using (EConnectContext context = new EConnectContext())
            {
                var objData = (from s in context.Downloadables
                               join c in context.UploadedFiles on s.DownloadableFileID equals c.ID
                               where s.CourseCategoryID == CourseCategoryId
                               && s.DownloadableTypeID == DownloadableTypeID
                               && s.ShowOnWeb == true
                               && s.EffectiveFromDate <= DateTime.Now
                               orderby s.EffectiveFromDate descending
                               select new
                               {
                                   ID = s.ID,
                                   filename = c.BlobFile,
                                   fname = c.OriginalName,
                                   linkname = s.LinkName,
                                   fileID = s.DownloadableFileID.Value,
                                   effectiveDate = s.EffectiveFromDate
                               }).FirstOrDefault();
                if (objData != null)
                { link.HRef = "../Handlers/UploadedFileHandler.ashx?ID=" + objData.fileID; }
                else
                { link.HRef = ""; }
            }
            ;
        }
        catch (Exception ex) { ShowAlert(ex.Message, true); }
    }
    private void Occupation(Course CurrentCourse)
    {
        using (EConnectContext vContext = new EConnectContext())
        {
            var OccupationList = vContext.CourseWiseOccupationMappings.Where(a => a.CourseID == CurrentCourse.ID).Select(k => k.OccupationID).Distinct();
            var Occupation = vContext.Occupations.Where(s => s.IsEnabled == true && OccupationList.Contains(s.ID)).
                              OrderBy(s => s.DisplayOrder).
                              Select(s => new { ValueField = s.ID, TextField = s.Name + "/" + s.NameRegional });
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlOccupation, Occupation, new ListItem("--Select One--", "0"));
        }
    }
    private void EducationalQualification(Course CurrentCourse)
    {
        using (EConnectContext vContext = new EConnectContext())
        {
            var EducationalQualification = vContext.EducationalQualifications.OrderBy(s => s.DisplayOrder).
                                            Join(vContext.QualificationEligibility.Where(q => q.CourseID == CurrentCourse.ID),
                                            e => e.QualificationLevelID, q => q.QualificationLevelID,
                                            (e, q) => new { ValueField = e.ID, TextField = e.Name }
                                            ).Distinct();
            EConnect.Utils.Common.ControlUtility.BindListObject(DDLeducode, EducationalQualification, new ListItem("--Select One--", "0"));
        }
    }
    private void CastCategory()
    {
        using (EConnectContext vContext = new EConnectContext())
        {
            var CastCategoryType = vContext.CastCategories.Where(s => s.ID != 5).OrderBy(s => s.DisplayOrder).Select(s => new { ValueField = s.ID, TextField = s.Name + "/" + s.NameRegional });
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCategory, CastCategoryType, new ListItem("--Select One--", "0"));
        }
    }
    private void DisabilityType()
    {
        using (EConnectContext vContext = new EConnectContext())
        {
            var DisabilityType = vContext.DisabilityTypes.Select(s => new { ValueField = s.ID, TextField = s.Code + " - " + s.Name });
            EConnect.Utils.Common.ControlUtility.BindListObject(DdlDisabilityType, DisabilityType, new ListItem("--Select One--", "0"));
        }
    }
    protected void BindExamCentre(Int64 StateID, Int32 CentreId, DropDownList ddl, enmExamCenterType examCentreType)
    {
        try
        {
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["id"])))
            {
                Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
                Int32 typeID = Convert.ToInt32(examCentreType);
                ListItem lst = new ListItem("--Select Location--", "0");
                using (EConnectContext vContext = new EConnectContext())
                {
                    int courseCategoryID = vContext.Courses.Find(courseID).CourseCategoryID;
                    var examcentre = vContext.ExamCenters.Where(s => s.ID != CentreId && s.StateID == StateID && s.CourseCategoryID == courseCategoryID && s.IsEnabled == true && (s.CourseID == courseID || s.CourseID == null)) //Change by Gaurav Chaurasia for Course wise centre option on 02 March 2021
                                                        .OrderBy(s => s.Name).Select(s => new { ValueField = s.ID, TextField = s.Code.ToUpper() + " - " + s.Name.ToUpper(), examCentreTypeID = s.ExamCentreTypeID });
                    if (examCentreType == enmExamCenterType.Primary || examCentreType == enmExamCenterType.Secondary)
                    { examcentre = examcentre.Where(e => e.examCentreTypeID == typeID); }
                    ddl.Items.Clear();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddl, examcentre, lst);
                }
                ;
            }
        }
        catch (Exception ex) { throw ex; }
    }
    protected IEnumerable<Object> GetNextExam(Int32 applicantTypeID, Int32 courseID, Int32 examCycleID)
    {
        try
        {
            IEnumerable<Object> Exams;
            IQueryable<CutOffDate> CuttOffDates;
            Int32 StartDateofFormFilling = Convert.ToInt32(enmActivity.DateFfCommencementOfOnlineFillInExaminationApplicationForm);
            Int32 LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
            Int32 NormalFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
            using (EConnectContext vContext = new EConnectContext())
            {
                CuttOffDates = vContext.CutOffDates.Where(s => s.CourseID == courseID && s.ApplicantTypeID == applicantTypeID);
                var ExamAll = vContext.Exams.Where(s => s.CourseID == courseID
                                                   && s.ExaminationCycleID == examCycleID
                                                   && s.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                                   && s.DateOfPublishingOfRollNumber == null
                                                  );
                if (ExamAll.Count() > 0 && CuttOffDates.Where(s => s.ActivityID == StartDateofFormFilling && s.EfferctiveDate <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)).Count() > 0)
                {
                    CuttOffDates = CuttOffDates.Where(s => s.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && ExamAll.Select(t => t.ID).Contains(s.ExamID));

                    if (CuttOffDates.Where(s => s.ActivityID == NormalFeeActivityId).Count() > 0)
                    { CuttOffDates = CuttOffDates.Where(s => s.ActivityID == NormalFeeActivityId); }
                    else
                    { CuttOffDates = CuttOffDates.Where(s => s.ActivityID == LateFeeActivityId); }

                    CuttOffDates = CuttOffDates.OrderBy(s => s.Exam.ExamStartDate).Distinct();
                    Exams = CuttOffDates.Select(c => new { ValueField = c.Exam.ID, TextField = c.Exam.Name }).ToList();
                }
                else
                { Exams = ExamAll.Select(c => new { ValueField = c.ID, TextField = c.Name }).ToList(); }
            }
            ;
            return Exams;
        }
        catch (Exception ex) { throw ex; }
    }
    protected Int32 GetNextExamID(Int32 applicantTypeID, Int32 courseID, Int32 examCycleID)
    {
        try
        {
            Int32 ExamId = 0;
            IQueryable<CutOffDate> CuttOffDates;
            Int32 StartDateofFormFilling = Convert.ToInt32(enmActivity.DateFfCommencementOfOnlineFillInExaminationApplicationForm);
            Int32 LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
            Int32 NormalFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);

            using (EConnectContext vContext = new EConnectContext())
            {
                CuttOffDates = vContext.CutOffDates.Where(s => s.CourseID == courseID && s.ApplicantTypeID == applicantTypeID);
                var ExamAll = vContext.Exams.Where(s => s.CourseID == courseID
                                                   && s.ExaminationCycleID == examCycleID
                                                   && s.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                                   && s.DateOfPublishingOfRollNumber == null
                                                  ).Select(s => s.ID);
                if (ExamAll.Count() > 0 && CuttOffDates.Where(s => s.ActivityID == StartDateofFormFilling && s.EfferctiveDate <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)).Count() > 0)
                {
                    CuttOffDates = CuttOffDates.Where(s => s.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && ExamAll.Contains(s.ExamID));

                    if (CuttOffDates.Where(s => s.ActivityID == NormalFeeActivityId).Count() > 0)
                    { CuttOffDates = CuttOffDates.Where(s => s.ActivityID == NormalFeeActivityId); }
                    else
                    { CuttOffDates = CuttOffDates.Where(s => s.ActivityID == LateFeeActivityId); }

                    CuttOffDates = CuttOffDates.OrderBy(s => s.Exam.ExamStartDate).Distinct();
                    ExamId = CuttOffDates.FirstOrDefault().ExamID;
                }
                else
                { ExamId = ExamAll.FirstOrDefault(); }
            }
            ;
            return ExamId;
        }
        catch (Exception ex) { throw ex; }
    }
    public string FilterWhiteSpaces(string input)
    {
        if (input == null)
            return string.Empty;

        StringBuilder stringBuilder = new StringBuilder(input.Length);

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            if (i == 0 || c != ' ' || (c == ' ' && input[i - 1] != ' '))
                stringBuilder.Append(c);
        }
        return stringBuilder.ToString();
    }
    protected String DuplicateApplication(Int32 examID, Int32 NewFlag, Int32 UpdateFlag, Int64 applId)
    {
        try
        {

            string appName = FilterWhiteSpaces(txtAppName.Text.Trim());
            string guardianName = FilterWhiteSpaces(TxtGuardianName.Text.Trim());
            string fatherName = FilterWhiteSpaces(txtFatherName.Text.Trim());
            string motherName = FilterWhiteSpaces(txtMotherName.Text.Trim());


            String ApplicationNumber = string.Empty;
            DateTime dob = Convert.ToDateTime(txtDob.Text);
            using (EConnectContext vContext = new EConnectContext())
            {
                IQueryable<CertificateExamApplication> Application = vContext.CertificateExamApplications
                                                      .Where(
                                                                s => s.ExamID == examID
                                                                && s.Name.Equals(appName, StringComparison.OrdinalIgnoreCase)
                                                                && s.Gender == ddl_gender.SelectedValue
                                                                && System.Data.Entity.DbFunctions.TruncateTime(s.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                                             );
                if (TxtGuardianName.Text.Trim().Length > 0)
                { Application = Application.Where(s => s.GuardianName.Equals(guardianName, StringComparison.OrdinalIgnoreCase)); }
                else
                { Application = Application.Where(s => s.FatherName.Equals(fatherName, StringComparison.OrdinalIgnoreCase) && s.MotherName.Equals(motherName, StringComparison.OrdinalIgnoreCase)); }

                if (NewFlag == 1)
                { var appl = Application.FirstOrDefault(); if (appl != null) { ApplicationNumber = appl.Number; } }
                else if (UpdateFlag == 1)
                { var appl = Application.Where(s => s.ID != applId).FirstOrDefault(); if (appl != null) { ApplicationNumber = appl.Number; } }
            }
            ;
            return ApplicationNumber;
        }
        catch (Exception ex) { throw ex; }
    }
    #endregion

    #region Events: Click
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 applId = 0;
            Int32 gradeID = 0;
            Int32 feeAmount = 0;
            Int32 lateFeeAmount = 0;
            Int64 ExamCentreTypeId1StateId = 0;
            Boolean IsExemptedCase = false;
            String DuplicateApplicationNumber = string.Empty;
            Int32 CourseId = Convert.ToInt32(Request.QueryString["id"]);
            Int32 ExamId = Convert.ToInt32(DdlApplied4Exam.SelectedValue);
            Int32 ExamCycleId = Convert.ToInt32(DdlExamCycle.SelectedValue);
            Int32 ExamCentreID1 = Convert.ToInt32(DdlExamCentre1.SelectedValue);
            Int32 ExamCentreID2 = Convert.ToInt32(DdlExamCentre2.SelectedValue);
            Int32 applicantTypeId = (RdoAppliedAs.SelectedValue == "I") ? Convert.ToInt32(enmApplicantType.Institute) : Convert.ToInt32(enmApplicantType.Direct);

            using (EConnectContext context = new EConnectContext())
            {
                Course currentCourse = context.Courses.Find(CourseId);

                if (IsValidForm(currentCourse.CourseCategory.Name))
                {

                    // Apaar Duplicacy check code start  //Check for duplicate Apaar for Exam and Course

                    string apaarEncryptedCheck = EncryptDecrypt.EncryptString(txtapaar.Text);

                    string duplicate = checkDuplicateApaar(apaarEncryptedCheck, CourseId, ExamId);

                    if (duplicate != "-99")
                    {
                        if (duplicate != "0")
                        {
                            if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                            {
                                Int64 applId1 = Convert.ToInt64(Request.QueryString["Appid"]);
                                //This Query will get all the information of Applied Canditate by generated Application id
                                CertificateExamApplication oldApplication = context.CertificateExamApplications.Find(applId1);
                                if (oldApplication.Number != duplicate)
                                    throw new Exception("Duplicate application for the exam cycle not allowed, Check status for " + duplicate.ToString() + " Application");
                            }
                            else
                                throw new Exception("Duplicate application for the exam cycle not allowed, Check status for " + duplicate.ToString() + " Application");

                        }
                    }
                    else
                    {
                        throw new Exception("Error occured, Please try again");
                    }

                    // Apaar Duplicacy check code end


                    // if duplicacy check passes, that is no duplicate application number , found , proceed with apaar validation api

                    //Validation of apaar
                    // amit_apaar_may_2026_start

                    string gender = ddl_gender.SelectedValue.Substring(0, 1).ToUpper();
                    long apaarRequestId = 0;

                    //try
                    //{
                    string validatedApaarData = validateApaar.ConvertApaarDatatoJSONandEncrypt(txtapaar.Text.Trim(), txtAppName.Text.Trim(), txtDob.Text, gender, txtproviderName.Text.Trim(), ddlAuthMode.SelectedItem.Text, txtAuthenticationIdNo.Text, ddlConsentRelation.SelectedItem.Text, txtConsentPlace.Text.Trim(), lblApaarDeclaration.Text, out apaarRequestId);

                    if (String.IsNullOrWhiteSpace(validatedApaarData))
                    {
                        txtcode.Text = "";
                        GenerateNewCaptchaImage();

                        throw new Exception("Apaar could not be validated.");
                    }

                    JObject apaarObj = JObject.Parse(validatedApaarData);


                    string status =
                        apaarObj["status"] == null
                        ? ""
                        : apaarObj["status"].ToString().Trim().ToLower();

                    string statusCode =
                        apaarObj["status_code"] == null
                        ? ""
                        : apaarObj["status_code"].ToString().Trim();

                    string messageCode =
                        apaarObj["message_code"] == null
                        ? ""
                        : apaarObj["message_code"].ToString().Trim();

                    string message =
                        apaarObj["message"] == null
                        ? ""
                        : apaarObj["message"].ToString().Trim();

                    using (EConnectContext db = new EConnectContext())
                    {
                        apaarResponseRecd responseObj = new apaarResponseRecd();

                        // ApaarID
                        responseObj.abc_account_id = txtapaar.Text;

                        if (apaarObj["message"].ToString() != "Records not found")
                        {
                            responseObj.abc_account_id = txtapaar.Text;
                            // cname

                            if (apaarObj["CNAME"] != null)
                                responseObj.cname = apaarObj["CNAME"].ToString();


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

                            // dob
                            if (apaarObj["DOB"] != null)
                            {
                                DateTime parsedDob;

                                if (DateTime.TryParseExact(apaarObj["DOB"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDob))
                                {
                                    responseObj.dob = parsedDob;

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
                            DateTime dob = Convert.ToDateTime(txtDob.Text);
                            string dateofbirth = dob.ToString("dd/MM/yyyy");

                            if (dateofbirth.Contains('-'))
                            {
                                string[] vdob = dateofbirth.Split('-');
                                dateofbirth = vdob[0] + "/" + vdob[1] + "/" + vdob[2];
                            }

                            string dateofbirth1 = apaarObj["DOB"].ToString();

                            if (dateofbirth1.Length < 10)
                            {
                                string[] s = dateofbirth1.Split('/');
                                if (s[0].Length < 2)
                                    s[0] = "0" + s[0];

                                if (s[1].Length < 2)
                                    s[1] = "0" + s[1];



                                dateofbirth1 = s[0] + "/" + s[1] + "/" + s[2];
                            }


                            if (dateofbirth1 != dateofbirth)
                            {
                                txtcode.Text = "";
                                GenerateNewCaptchaImage();
                                throw new Exception("Invalid Apaar or Date of birth Mismatch, if apaar not generated, please generate or correct and enter");
                            }


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
                        txtcode.Text = "";
                        GenerateNewCaptchaImage();
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
                    // amit_apaar_may_2026_end




                    CertificateExamApplication application = new CertificateExamApplication();

                    #region New Record Entry i.e. AppId is null
                    // New Record
                    if (String.IsNullOrEmpty(Request.QueryString["Appid"]))
                    {

                        btnSave.Text = "Submit";
                        if ((DuplicateApplicationNumber = DuplicateApplication(ExamId, 1, 0, applId)) == string.Empty)
                        {
                            //Finding Course id in Course Table to get Course Related information.                          
                            application.CourseCategoryID = currentCourse.CourseCategoryID;
                            application.CourseID = currentCourse.ID;
                            application.ApplicationDate = DateTime.Now;
                            //if Already appeared for Exam then it will Save Previous Roll Number and Previous exam Cycle else not
                            if (RdoAlreadyAppeared4Exam.SelectedValue == "Y")
                            {
                                var PreviousApplication = context.CertificateExamApplications.Where(s => s.RollNumber.Equals(TxtRno.Text.Trim(), StringComparison.OrdinalIgnoreCase) && s.CourseID == CourseId).FirstOrDefault();
                                if (PreviousApplication != null)
                                {
                                    application.AlreadyApplied = true;
                                    application.PreviousRollNumber = TxtRno.Text.Trim();
                                    application.PreviousApplicationID = PreviousApplication.ID;
                                    application.PreviousExamID = PreviousApplication.ExamID;
                                }
                                //to check for * cases
                                if (PreviousApplication.ExamID != 0)
                                { gradeID = context.ResultGrades.Where(r => r.Code == "*" && r.VersionID == PreviousApplication.Exam.ResultGradeVersionID.Value).FirstOrDefault().ID; }
                                else
                                { gradeID = context.ResultGrades.Where(r => r.Code == "*" && r.VersionID == 1).FirstOrDefault().ID; }// By Default
                                if (gradeID != 0)
                                {
                                    if (PreviousApplication.ResultGradeID.HasValue && PreviousApplication.ResultUpdatedOn.HasValue)
                                    {
                                        DateTime resultdeclareDate = PreviousApplication.ResultUpdatedOn.Value.AddDays(7);
                                        if (PreviousApplication.ExemptedApplicationID == null && PreviousApplication.ResultGradeID == gradeID)
                                        {
                                            //now candidate is exempted for all exams other than previous exams.                                            
                                            IsExemptedCase = (GetNextExamID(applicantTypeId, CourseId, ExamCycleId) == ExamId) ? true : false;
                                        }
                                    }
                                }
                            }
                            else if (RdoAlreadyAppeared4Exam.SelectedValue == "N")
                            { application.AlreadyApplied = false; }
                        }
                        else
                        {
                            btnSave.Visible = false;
                            throw new Exception("You have already applied for this exam.Please check your application status using your Application number which is :- " + DuplicateApplicationNumber);
                        }
                    }
                    #endregion

                    #region Old Record Update i.e. AppId is not null
                    else
                    {
                        btnSave.Text = "Update";
                        applId = Convert.ToInt64(Request.QueryString["Appid"]);
                        //This Query will get all the information of Applied Canditate by generated Application id
                        application = context.CertificateExamApplications.Find(applId);
                    }
                    #endregion

                    // Applicant Type
                    application.ApplicantTypeID = applicantTypeId;
                    // Applicant Institute
                    if (RdoAppliedAs.SelectedValue == "I")//Is Applied as Institute
                    { application.InstituteID = Convert.ToInt32(DdlAccCentre.SelectedValue); }
                    else if (RdoAppliedAs.SelectedValue == "D")//Is Applied as Direct
                    { application.InstituteID = null; }
                    // ExamId
                    application.ExamID = ExamId;
                    // Exam Center
                    application.ExamCenter1ID = ExamCentreID1;
                    application.ExamCenter2ID = ExamCentreID2;
                    // Regional Center  
                    //Commented natasha email 21 Dec 2022                   
                    //ExamCentreTypeId1StateId = context.ExamCenters.Where(c => c.ID == ExamCentreID1).FirstOrDefault().StateID;
                    //  ExamCentreTypeId1StateId = context.ExamCenters.Where(c => c.ID == ExamCentreID1).OrderBy(s => s.StateID).OrderBy(s => s.StateID).FirstOrDefault().StateID;
                    //Added on 23 Dec 2022 after comment            
                    ExamCentreTypeId1StateId = context.ExamCenters.Where(c => c.ID == ExamCentreID1).OrderBy(s => s.StateID).FirstOrDefault().StateID;
                    //Added 2 Jan 2023
                    var WhetherDistributed = (from c in context.CourseWiseStates
                                              where c.CourseID == CourseId && c.StateID == ExamCentreTypeId1StateId
                                              select new { wd = c.Whetherdistributedistrictwise }).FirstOrDefault();

                    if (WhetherDistributed.wd == 1)
                    {
                        application.RegionalCenterID = context.ExamCenters.Where(c => c.StateID == ExamCentreTypeId1StateId && c.ID == ExamCentreID1).FirstOrDefault().RegionalCentreId;
                    }
                    else
                    {
                        application.RegionalCenterID = context.CourseWiseStates.Where(c => c.CourseID == CourseId && c.StateID == ExamCentreTypeId1StateId).OrderBy(s => s.RegionalCenterID).FirstOrDefault().RegionalCenterID;
                    }


                    //Commented on 2 Jan 2023 application.RegionalCenterID = context.CourseWiseStates.Where(c => c.CourseID == CourseId && c.StateID == ExamCentreTypeId1StateId).OrderBy(s => s.RegionalCenterID).FirstOrDefault().RegionalCenterID;
                    // application.RegionalCenterID = context.CourseWiseStates.Where(c => c.CourseID == CourseId && c.StateID == ExamCentreTypeId1StateId).FirstOrDefault().RegionalCenterID;
                    //  application.RegionalCenterID = context.CourseWiseStates.Where(c => c.CourseID == CourseId && c.StateID == ExamCentreTypeId1StateId).OrderBy(s => s.RegionalCenterID).FirstOrDefault().RegionalCenterID;
                    //
                    // Candidate Name.

                    ////if (!String.IsNullOrEmpty(Request.QueryString["Appid"])) //amit_apaar_may_2026_start
                    if (String.IsNullOrEmpty(Request.QueryString["Appid"])) //amit_apaar_may_2026_start
                    {
                        // only update these fields in NEW REGISTRATION , not update
                        string salutation = ddlSalutaionName.SelectedItem.Text;
                        application.Salutation = salutation.Substring(0, salutation.IndexOf('/')).Trim();
                        application.Name = txtAppName.Text.Trim();
                        // Parent/Guardian
                        if (Rdoownertype.SelectedValue == "P")
                        {
                            application.FatherName = txtFatherName.Text.Trim();
                            application.MotherName = txtMotherName.Text.Trim();
                            application.GuardianName = null;
                        }
                        else if (Rdoownertype.SelectedValue == "G")
                        {
                            application.GuardianName = TxtGuardianName.Text.Trim();
                            application.FatherName = null;
                            application.MotherName = null;
                        }
                        application.Gender = ddl_gender.SelectedValue;
                        application.DateOfBirth = Convert.ToDateTime(txtDob.Text.ToString());
                    }

                    // Gender
                    //string gender =ddl_gender.SelectedItem.Text.ToString();
                    //application.Gender = gender.Substring(0, gender.IndexOf('/')).Trim();

                    // DOB

                    // CastCategory
                    application.CastCategoryID = Convert.ToInt32(ddlCategory.SelectedValue);
                    // Disability
                    if (RdDisability.SelectedValue == "Y")
                    {
                        application.IsDisability = true;
                        application.DisabilityTypeID = Convert.ToInt32(DdlDisabilityType.SelectedValue);
                        application.DisabilityPercentage = Convert.ToInt32(TxtDisabilityPercent.Text.Trim());
                    }
                    else { application.IsDisability = false; }
                    // jk sah on 14-1-2021
                    if (RdisEWS.SelectedValue == "Y")
                    {
                        application.Is_EWS = true;
                    }
                    else
                    {
                        application.Is_EWS = false;
                    }
                    // Occupation
                    application.OccupationID = Convert.ToInt32(ddlOccupation.SelectedValue);
                    if (ddlOccupation.SelectedValue == "5")//if occupation is gujarat gov.
                    {
                        application.Department = Txtdept.Text.Trim();
                        application.Designation = txtdesg.Text.Trim();
                        application.EmployeeCode = Txtempcode.Text.Trim();
                        application.DateofJoining = Convert.ToDateTime(txtDojoin.Text);
                        application.DateofRetirement = Convert.ToDateTime(txtDoretment.Text);
                        application.PostingCity = txtpostcity.Text.Trim();
                    }
                    else
                    {
                        application.Department = null;
                        application.Designation = null;
                        application.EmployeeCode = null;
                        application.DateofJoining = null;
                        application.DateofRetirement = null;
                        application.PostingCity = null;
                    }
                    // Aadhar card details

                    if (!String.IsNullOrEmpty(hfaadhaar.Value.ToString()))
                    {
                        string decryptAadhar = EncryptDecrypt.DecryptString(hfaadhaar.Value.ToString());
                        application.AadharNumber = Convert.ToInt64(decryptAadhar);
                    }
                    else
                        application.AadharNumber = null;


                    //if (!String.IsNullOrEmpty(txtaadhar.Text))
                    //    { application.AadharNumber = Convert.ToInt64(txtaadhar.Text); }
                    //    else
                    //    { application.AadharNumber = null; }
                    application.AadharVerfied = false;
                    // UID Type & Number
                    if (UidTypeDdl.SelectedValue != "0")
                    {
                        application.UIDType = Convert.ToInt32(UidTypeDdl.SelectedValue.ToString());
                        application.UIDNumber = UidNumberTxt.Text.ToUpper();
                    }

                    if (!String.IsNullOrEmpty(txtapaar.Text))
                    {
                        //added on 12 Sept 2024
                        if (String.IsNullOrEmpty(Request.QueryString["Appid"])) //amit_apaar_may_2026_start
                        {
                            // only update in new case
                            string apaarEncrypted = EncryptDecrypt.EncryptString(txtapaar.Text);
                            application.apaarID = apaarEncrypted;


                        }

                        application.ApaarRequestID = apaarRequestId; // update apaarrequestid in every case //amit_apaar_may_2026_start 

                        //added on 12 Sept 2024
                    }
                    else
                        application.apaarID = null;

                    // Photo Upload
                    #region Photo
                    if (ImgUpload.HasFile)
                    {
                        if (ImgUpload.FileName.Length > 51200) //(50*1024)
                        { application.PhotoFileName = ImgUpload.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUpload.FileName).ToLower(); }
                        else
                        {
                            //application.PhotoFileName = ImgUpload.FileName;
                            string ImgNamePhoto = string.Empty;
                            ImgNamePhoto = "P" + Path.GetFileName(ImgUpload.FileName);          //Photo_
                            application.PhotoFileName = ImgNamePhoto;
                        }
                        //
                        application.Photo = ImgUpload.FileBytes;
                    }
                    else
                    {
                        application.Photo = application.Photo;
                        application.PhotoFileName = application.PhotoFileName;
                    }
                    #endregion
                    // Signature Upload
                    #region Signature
                    if (ImgUploadSignature.HasFile)
                    {
                        if (ImgUploadSignature.FileName.Length > 51200) //(50 * 1024)
                        { application.SignatureFileName = ImgUploadSignature.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUploadSignature.FileName).ToLower(); }
                        else
                        {
                            //application.SignatureFileName = ImgUploadSignature.FileName;
                            string ImgNameSig = string.Empty;
                            ImgNameSig = "S" + Path.GetFileName(ImgUploadSignature.FileName);          //Sig_
                            application.SignatureFileName = ImgNameSig;
                        }
                        application.Signature = ImgUploadSignature.FileBytes;
                    }
                    else
                    {
                        application.Signature = application.Signature;
                        application.SignatureFileName = application.SignatureFileName;
                    }
                    #endregion
                    // Thumb Upload
                    #region Thumb
                    if (ImgUploadThumb.HasFile)
                    {
                        if (ImgUploadThumb.FileName.Length > 51200) //(50 * 1024)
                        { application.LeftThumbFileName = ImgUploadThumb.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUploadThumb.FileName).ToLower(); }
                        else
                        {
                            //application.LeftThumbFileName = ImgUploadThumb.FileName;
                            string ImgNameLth = string.Empty;
                            ImgNameLth = "L" + Path.GetFileName(ImgUploadThumb.FileName);         //Lth_
                            application.LeftThumbFileName = ImgNameLth;
                        }
                        application.LeftThumb = ImgUploadThumb.FileBytes;
                    }
                    else
                    {
                        application.LeftThumb = application.LeftThumb;
                        application.LeftThumbFileName = application.LeftThumbFileName;
                    }
                    #endregion
                    // Candidate Contact Detail.
                    if (String.IsNullOrEmpty(TxtSTDcode.Text) == false && string.IsNullOrEmpty(txtCorPhoneNo.Text) == false)
                    {
                        application.StdNumber = Convert.ToInt32(TxtSTDcode.Text);
                        application.PhoneNumber = Convert.ToInt32(txtCorPhoneNo.Text);
                    }
                    application.MobileNumber = Convert.ToInt64(txtCorMobileNo.Text);
                    application.EmailAddress = txtEmailId.Text.Trim();
                    application.SMSSent = false;
                    application.EmailSent = false;
                    // Candidate Address Detail.
                    application.CorAddressLine1 = TxtAddressLine1.Text;
                    application.CorAddressLine2 = string.IsNullOrEmpty(TxtAddressLine2.Text) == false && !string.IsNullOrWhiteSpace(TxtAddressLine2.Text) ? TxtAddressLine2.Text : "";
                    application.CorAddressLine3 = string.IsNullOrEmpty(TxtAddressLine3.Text) == false && !string.IsNullOrWhiteSpace(TxtAddressLine3.Text) ? TxtAddressLine3.Text : "";
                    application.CorCityName = TxtCity.Text;
                    application.CorStateID = Convert.ToInt32(ddlCorState.SelectedValue);
                    application.CorDistrictID = Convert.ToInt32(Ddldistrict.SelectedValue);
                    application.CorPinCode = Convert.ToInt32(txtPinCode.Text);
                    // Candidate Education Qualification Detail.                            
                    application.EducationalQualificationID = Convert.ToInt32(DDLeducode.SelectedValue);
                    application.PassingYear = Convert.ToInt32(TxtYearOfPassing.Text);
                    // Payment Status
                    application.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                    // Fee Details                    
                    CourseManager.GetCertificateExamFee(CourseId, ExamId, applicantTypeId, out feeAmount, out lateFeeAmount);
                    application.FeeAmount = (IsExemptedCase == false) ? feeAmount : 0;
                    if (lateFeeAmount > 0) { application.LateFeeAmount = lateFeeAmount; }
                    else { application.LateFeeAmount = null; }
                    application.TotalFeeAmount = feeAmount + lateFeeAmount;
                    // Application Source
                    if ((!String.IsNullOrEmpty(Request.QueryString["Src"])) && (Request.QueryString["Src"] == "CSC"))
                    { application.ApplicationSourceID = Convert.ToInt32(enmApplicationSource.CSC); }
                    else { application.ApplicationSourceID = Convert.ToInt32(enmApplicationSource.Candidate); }

                    #region Additional for IRDA
                    //----------for IRDA--on 30-01-2016
                    if (CandId != 0 && currentCourse.CourseCategory.Name == "IRDA")
                    {
                        application.CandidateID = CandId;

                        var GetPhotoFileName = context.CourseRegistrationApplications.Where(s => s.CandidateID == CandId).Select(p => p).FirstOrDefault();
                        var GetPhoto = context.Candidates.Find(CandId);

                        application.Photo = GetPhoto.Photo.BlobFile;
                        application.PhotoFileName = GetPhotoFileName.PhotoFileName;

                        application.Signature = GetPhoto.Signature.BlobFile;
                        application.SignatureFileName = GetPhotoFileName.SignatureFileName;

                        application.LeftThumb = GetPhoto.LeftThumbImpression.BlobFile;
                        application.LeftThumbFileName = GetPhotoFileName.LeftThumbFileName;

                        int vCheckCand = context.CertificateExamApplications.Where(p => p.CandidateID == CandId && p.RollNumber != null).Count();

                        if (vCheckCand > 0)
                            application.IsExempted = false;
                        else
                        {
                            application.IsExempted = true;
                            application.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);

                        }
                    }
                    //--------End for IRDA--------- 
                    #endregion

                    // Save / Update
                    if (String.IsNullOrEmpty(Request.QueryString["Appid"])) //If new record
                    {
                        //application.OnlineRefID = txtMIS.Text;
                        //16.02.2022
                        if (txtMIS.Text.Trim() != "")
                        {
                            application.OnlineRefID = txtMIS.Text;
                        }
                        //
                        context.CertificateExamApplications.Add(application);
                        context.SaveChanges();
                        applId = application.ID;
                        // start 03-11-2021 vishal for to delete the image from server
                        //string fileNamePhoto = Server.MapPath("~/ImageUpload/" + Path.GetFileName(ImgUpload.FileName));
                        //if (File.Exists(fileNamePhoto))
                        //{ File.Delete(fileNamePhoto); }
                        //string fileNameSig = Server.MapPath("~/ImageUpload/" + Path.GetFileName(ImgUploadSignature.FileName));
                        //if (File.Exists(fileNameSig))
                        //{ File.Delete(fileNameSig); }
                        //string fileNameth = Server.MapPath("~/ImageUpload/" + Path.GetFileName(ImgUploadThumb.FileName));
                        //if (File.Exists(fileNameth))
                        //{ File.Delete(fileNameth); }
                        //
                        string ImgNamePhoto = string.Empty;
                        if (ImgUpload.HasFile)
                        { ImgNamePhoto = "P" + Path.GetFileName(ImgUpload.FileName); }          //Photo_
                        string fileNamePhoto = Server.MapPath("~/ImageUpload/" + ImgNamePhoto);
                        if (File.Exists(fileNamePhoto))
                        { File.Delete(fileNamePhoto); }

                        string ImgNameSig = string.Empty;
                        if (ImgUploadSignature.HasFile)
                        { ImgNameSig = "S" + Path.GetFileName(ImgUploadSignature.FileName); }         //Sig_
                        string fileNameSig = Server.MapPath("~/ImageUpload/" + ImgNameSig);
                        if (File.Exists(fileNameSig))
                        { File.Delete(fileNameSig); }

                        string ImgNameLth = string.Empty;
                        if (ImgUploadThumb.HasFile)
                        { ImgNameLth = "L" + Path.GetFileName(ImgUploadThumb.FileName); }         //Lth_
                        string fileNameth = Server.MapPath("~/ImageUpload/" + ImgNameLth);
                        if (File.Exists(fileNameth))
                        { File.Delete(fileNameth); }
                        // end 03-11-2021 vishal for to delete the image from server
                    }
                    else
                    {
                        if ((DuplicateApplicationNumber = DuplicateApplication(ExamId, 0, 1, applId)) == string.Empty)// Duplicate Check
                        {
                            context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                            // start 03-11-2021 vishal for to delete the image from server
                            //string fileNamePhoto = Server.MapPath("~/ImageUpload/" + Path.GetFileName(ImgUpload.FileName));
                            //if (File.Exists(fileNamePhoto))
                            //{ File.Delete(fileNamePhoto); }
                            //string fileNameSig = Server.MapPath("~/ImageUpload/" + Path.GetFileName(ImgUploadSignature.FileName));
                            //if (File.Exists(fileNameSig))
                            //{ File.Delete(fileNameSig); }
                            //string fileNameth = Server.MapPath("~/ImageUpload/" + Path.GetFileName(ImgUploadThumb.FileName));
                            //if (File.Exists(fileNameth))
                            //{ File.Delete(fileNameth); }
                            //
                            string ImgNamePhoto = string.Empty;
                            if (ImgUpload.HasFile)
                            { ImgNamePhoto = "P" + Path.GetFileName(ImgUpload.FileName); }          //Photo_
                            string fileNamePhoto = Server.MapPath("~/ImageUpload/" + ImgNamePhoto);
                            if (File.Exists(fileNamePhoto))
                            { File.Delete(fileNamePhoto); }

                            string ImgNameSig = string.Empty;
                            if (ImgUploadSignature.HasFile)
                            { ImgNameSig = "S" + Path.GetFileName(ImgUploadSignature.FileName); }         //Sig_
                            string fileNameSig = Server.MapPath("~/ImageUpload/" + ImgNameSig);
                            if (File.Exists(fileNameSig))
                            { File.Delete(fileNameSig); }

                            string ImgNameLth = string.Empty;
                            if (ImgUploadThumb.HasFile)
                            { ImgNameLth = "L" + Path.GetFileName(ImgUploadThumb.FileName); }         //Lth_
                            string fileNameth = Server.MapPath("~/ImageUpload/" + ImgNameLth);
                            if (File.Exists(fileNameth))
                            { File.Delete(fileNameth); }
                            // end 03-11-2021 vishal for to delete the image from server

                        }
                        else { throw new Exception("You have already applied for this exam.Please check your application status using your Application number which is :- " + DuplicateApplicationNumber); }
                    }
                }
                else
                {
                    ShowAlert("Missing Fields");
                    return;
                }
            }
                ;

            if ((!String.IsNullOrEmpty(Request.QueryString["id"])) && (Request.QueryString["Src"] == "CSC"))
            {
                Response.Redirect("CertificatePreview.aspx?Appid=" + applId + "&Src=CSC&RU=" + Request.QueryString["RU"].ToString(), false);
            }
            else
            {
                Response.Redirect("CertificatePreview.aspx?Appid=" + applId + "&id=" + Request.QueryString["id"] + "&candtype=" + Request.QueryString["candtype"], false);
            }
            Context.ApplicationInstance.CompleteRequest();

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
            ShowAlert(ex.Message);
        }


        // catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
        //{
        //    Exception raise = dbEx;
        //    foreach (var validationErrors in dbEx.EntityValidationErrors)
        //    {
        //        foreach (var validationError in validationErrors.ValidationErrors)
        //        {
        //            string message = string.Format("{0}:{1}",
        //                validationErrors.Entry.Entity.ToString(),
        //                validationError.ErrorMessage);
        //            // raise a new exception nesting  
        //            // the current instance as InnerException  
        //            raise = new InvalidOperationException(message, raise);
        //        }
        //    }
        //    throw raise;
        //}
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        ActiveInActive(true);
        if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
        {
            DdlExamCentre1.Enabled = false;

            DdlExamCentreState1.Enabled = false;

        }
        divgurdian.Style.Add("display", "none");
    }
    //protected void btnCancel_Click(object sender, EventArgs e)
    //{
    //    ActiveInActive(true);
    //     if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
    //            {
    //                DdlExamCentre1.Enabled = false;

    //                DdlExamCentreState1.Enabled = false;

    //            }
    //    divgurdian.Style.Add("display", "none");
    //    Rdoownertype.SelectedValue = "P";
    //    Rdoownertype_SelectedIndexChanged(Rdoownertype.SelectedValue, EventArgs.Empty);
    //}
    //Added 14 Feb 2019
    protected void btnOKD_Click(object sender, EventArgs e)
    {
        ActiveInActive(true);
        if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
        {
            DdlExamCentre1.Enabled = false;

            DdlExamCentreState1.Enabled = false;

        }
        divDisability.Style.Add("display", "none");
        divPopup.Style.Add("display", "none");
    }
    protected void btnCancelD_Click(object sender, EventArgs e)
    {
        ActiveInActive(true);
        if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
        {
            DdlExamCentre1.Enabled = false;

            DdlExamCentreState1.Enabled = false;

        }
        divDisability.Style.Add("display", "none");
        divPopup.Style.Add("display", "none");
        RdDisability.SelectedValue = "N";
        RdDisability_SelectedIndexChanged(RdDisability.SelectedValue, EventArgs.Empty);
    }
    protected void btnback_Click(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(Request.QueryString["RU"]))
            {
                Response.Redirect(Request.QueryString["RU"].ToString());
            }
            else
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../WEB/RulesForOnlineRegistration.aspx?" + Request.QueryString), false);
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
        }
    }
    protected void ImgBtnReset_Click(object sender, ImageClickEventArgs e)
    {
        GenerateNewCaptchaImage();
        txtcode.Text = "";
        Response.Redirect("CertificateRegistration.aspx?" + Request.QueryString.ToString());
    }
    protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (TxtRno.Text.Trim() == "")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Enter Roll Number.";
            }
            else
            {
                ResetAll();
                int courseID = Convert.ToInt32(Request.QueryString["id"]);
                // string RollNo = TxtRno.Text.Trim();
                //Int64 applID = Convert.ToInt64(Request.QueryString["Appid"]);
                using (EConnectContext context = new EConnectContext())
                {
                    Int64 applID = (from r in context.CertificateExamApplications where r.RollNumber == TxtRno.Text orderby r.ID descending select r.ID).FirstOrDefault();  // change line on dated 08062023 by Abhi Singh (CRP)
                                                                                                                                                                            //var application = context.CertificateExamApplications.Where(p => p.RollNumber.Equals(RollNo, StringComparison.OrdinalIgnoreCase) && p.CourseID == courseID).FirstOrDefault();
                                                                                                                                                                            //var application = context.CertificateExamApplications.Where(p => p.ID == applID && p.CourseID == courseID).FirstOrDefault();
                    var application = context.CertificateExamApplications.Where(p => p.ID == applID && p.CourseID == courseID).FirstOrDefault();

                    if (application != null)
                    {
                        ActiveInactivePrsnlDetail2(false);
                        TblFormDetail.Visible = true;
                        //Candidate Personal Detail...
                        if (!string.IsNullOrEmpty(application.Salutation))
                        {
                            if (application.Salutation == "Others")
                                ddlSalutaionName.SelectedValue = "X";
                            else
                                ddlSalutaionName.SelectedValue = application.Salutation.ToString();
                        }
                        else { ddlSalutaionName.Enabled = true; }
                        txtAppName.Text = GetInitCap(application.Name.ToString());
                        if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName) == true)
                        {
                            if (String.IsNullOrEmpty(application.FatherName))
                            { txtFatherName.Enabled = true; }
                            else { txtFatherName.Text = GetInitCap(application.FatherName); }
                            if (String.IsNullOrEmpty(application.MotherName))
                            { txtMotherName.Enabled = true; }
                            else { txtMotherName.Text = GetInitCap(application.MotherName); }
                        }
                        else
                        { TxtGuardianName.Text = GetInitCap(application.GuardianName); }
                        if (!string.IsNullOrEmpty(application.Gender))
                        {
                            //ddl_gender.SelectedValue = application.Gender; 
                            ddl_gender.SelectedValue = application.Gender;
                        }
                        else
                        {
                            //ddl_gender.Enabled = true; }
                            ddl_gender.Enabled = true;
                        }
                        txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");
                        if (!string.IsNullOrEmpty(application.CastCategoryID.ToString()))
                        { ddlCategory.SelectedValue = application.CastCategoryID.ToString(); }
                        else { ddlCategory.Enabled = true; }
                        if (application.IsDisability.HasValue)
                        { RdDisability.SelectedValue = (application.IsDisability == false) ? "N" : "Y"; }
                        else { RdDisability.Enabled = true; RdDisability.SelectedValue = "N"; }
                        if (application.IsDisability == true && application.DisabilityTypeID == null)
                        {
                            trDisability1.Visible = true; trDisability2.Visible = true;
                            DdlDisabilityType.Enabled = true; TxtDisabilityPercent.Enabled = true;
                        }
                        else if (application.IsDisability == true && application.DisabilityTypeID != null)
                        {
                            DdlDisabilityType.SelectedValue = application.DisabilityTypeID.Value.ToString();
                            TxtDisabilityPercent.Text = application.DisabilityPercentage.Value.ToString();
                        }
                        if (ddlCategory.SelectedValue == "0")
                        { ddlCategory.Enabled = true; }
                        if (!string.IsNullOrEmpty(application.OccupationID.ToString()))
                        { ddlOccupation.SelectedValue = application.OccupationID.ToString(); }

                        // Candidate Contact Detail...
                        TxtSTDcode.Text = application.StdNumber != null ? "0" + application.StdNumber.ToString() : "";
                        if (TxtSTDcode.Text == "0")
                        { TxtSTDcode.Text = ""; }
                        if (application.PhoneNumber.HasValue)
                        { txtCorPhoneNo.Text = application.PhoneNumber.Value.ToString(); }
                        txtCorMobileNo.Text = application.MobileNumber.ToString();
                        if (application.EmailAddress != null)
                        { txtEmailId.Text = application.EmailAddress.ToString(); }

                        //Candidate (By default Corespondance here) Address Detail...
                        TxtAddressLine1.Text = GetInitCap(application.CorAddressLine1);
                        TxtAddressLine2.Text = !string.IsNullOrEmpty(application.CorAddressLine2) && !string.IsNullOrWhiteSpace(application.CorAddressLine2) ? GetInitCap(application.CorAddressLine2) : "";
                        TxtAddressLine3.Text = !string.IsNullOrEmpty(application.CorAddressLine3) && !string.IsNullOrWhiteSpace(application.CorAddressLine3) ? GetInitCap(application.CorAddressLine3) : "";
                        if (application.CorCityName != null)
                        { TxtCity.Text = GetInitCap(application.CorCityName); }
                        if (application.CorStateID != 1)
                        {
                            ddlCorState.SelectedValue = application.CorStateID.ToString();
                            int id1 = Convert.ToInt32(ddlCorState.SelectedValue);
                            BindDistrict(id1);
                            if (application.CorDistrictID.HasValue)
                            { Ddldistrict.SelectedValue = application.CorDistrictID.ToString(); }
                        }
                        txtPinCode.Text = application.CorPinCode.ToString();

                        //Candidate Educational Qualification detail 
                        DDLeducode.SelectedValue = application.EducationalQualificationID.ToString();
                        if (application.PassingYear.HasValue)
                        { TxtYearOfPassing.Text = application.PassingYear.Value.ToString(); }
                        ImgBtnSearch.Visible = false;
                        ImgBtnReset.Visible = true;
                    }
                    else
                    {

                        lblerror.Visible = true;
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        lblerror.Text = "No details found for given roll number " + TxtRno.Text + ". Please check roll number and search again.";
                        TxtRno.Text = "";
                        //lblerror.Text = "No details found for given Application Number " + application.Number + ". Please check Application Number and search again.";
                        ImgBtnSearch.Visible = true;
                        ImgBtnReset.Visible = false;
                    }
                }
                ;
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }
    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GenerateNewCaptchaImage();
            txtcode.Text = "";
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
        }
    }
    #endregion

    #region Events: SelectedIndexChanged
    //amit_apaar_api_changes_may_2026_start
    protected void ddlAuthMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            if (ddlAuthMode.SelectedItem.Text.ToLower() == "self")
            {
                txtAuthenticationIdNo.Text = txtapaar.Text.ToString();
                txtAuthenticationIdNo.Enabled = false;
                authidrule.InnerText = "Apaar ID";
            }
            else
            {
                txtAuthenticationIdNo.Text = "";
                lblauthidrules.Visible = true;
                txtAuthenticationIdNo.Enabled = true;

                switch (ddlAuthMode.SelectedValue)
                {
                    case "3": // PAN
                        authidrule.InnerText = "10 characters only. Alphabets and numbers allowed. No spaces.";
                        break;

                    case "4": // DL
                        authidrule.InnerText = "10 to 15 characters only. Alphabets, numbers, hyphen (-). No spaces.";
                        break;

                    case "5": // Passport
                        authidrule.InnerText = "8 characters only. Alphabets and numbers allowed. No spaces.";
                        break;

                    case "6": // EPIC
                        authidrule.InnerText = "10 characters only. Alphabets and numbers allowed. No spaces.";
                        break;

                    default:
                        authidrule.InnerText = string.Empty;
                        break;
                }
            }

            //DateTime todaydate = DateTime.Now;
            //DateTime Inputdate = Convert.ToDateTime(txtDob.Text);
            //int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);
            //BindApaarDeclaration(countAge);
        }
        catch (Exception ex)
        {
            throw new Exception("ER101 , Date Of Birth is not valid.");
        }
    }
    protected void txtAppName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtDob.Text = "";
            txtapaar.Text = "";
            txtAuthenticationIdNo.Text = "";

        }
        catch (Exception ex)
        {
            throw new Exception("AP01, Error related to Applicant Name. Contact NIELIT HO");
        }
    }
    protected void txtapaar_TextChanged(object sender, EventArgs e)
    {
        txtDob_TextChanged(sender, e);
        GenerateNewCaptchaImage();
        txtcode.Text = "";
    }
    protected void txtDob_TextChanged(object sender, EventArgs e)
    {
        try
        {

            txtAuthenticationIdNo.Text = "";
            ddlAuthMode.SelectedIndex = 0;
            ddlConsentRelation.SelectedIndex = 0;
            txtproviderName.Text = "";
            //txtConsentDate.Text = "";
            //txtConsentTime.Text = "";
            txtConsentPlace.Text = "";

            DateTime todaydate = DateTime.Now;
            DateTime Inputdate = DateTime.ParseExact(txtDob.Text, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);

            txtapaar.Enabled = true;
            //added for Apaar Api validation check ashutosh start
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
                txtproviderName.Text = txtAppName.Text;
                txtproviderName.Enabled = false;

                BindAuthMode(countAge);
                ddlAuthMode.SelectedValue = "1";

                ddlAuthMode.Enabled = false;
                //ddlAuthMode.Enabled = false;
                txtAuthenticationIdNo.Text = txtapaar.Text;
                txtAuthenticationIdNo.Enabled = false;
                ddlAuthMode_SelectedIndexChanged(null, null);

            }
            else
            {
                txtIsProviderPresent.Text = "True";
                txtIsProviderPresent.Enabled = false;

                ddlConsentRelation.Enabled = true;
                txtproviderName.Enabled = true;
                ddlAuthMode.Enabled = true;
                txtAuthenticationIdNo.Enabled = true;
                if (Rdoownertype.SelectedValue == "P")
                {
                    ddlConsentRelation.Items.Clear();
                    ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
                    ddlConsentRelation.Items.Add(new ListItem("Father", "3"));
                    ddlConsentRelation.Items.Add(new ListItem("Mother", "4"));
                }
                else
                {
                    ddlConsentRelation.Items.Clear();
                    ddlConsentRelation.Items.Add(new ListItem("--Select--", "0"));
                    ddlConsentRelation.Items.Add(new ListItem("Guardian", "2"));
                }
                ddlConsentRelation.SelectedIndex = 0;
                txtproviderName.Text = "";
                // txtproviderName.Text = txtproviderName.Text;
                // txtproviderName.ReadOnly = true;

                BindAuthMode(countAge);
                ddlAuthMode.SelectedIndex = 0;
                //ddlAuthMode.Enabled =true;
                //txtproviderName.Enabled=true;
                //txtAuthenticationIdNo.Enabled = true;
                // ddlConsentRelation.Enabled=true;
            }
            txtConsentDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtConsentTime.Text = DateTime.Now.ToString("HH:mm");

            //added for Apaar Api validation check ashutosh end 
            //BindApaarDeclaration(countAge);
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = "Please enter proper Date of Birth e.g. 10-Jan-2001";
            ShowAlert("Please enter proper Date of Birth");
            //throw new Exception("Please enter proper Date of Birth");

        }
    }
    protected void txtFatherName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtDob.Text = "";
            txtapaar.Text = "";
            txtAuthenticationIdNo.Text = "";
            txtMotherName.Focus();
            GenerateNewCaptchaImage();
            txtcode.Text = "";
        }
        catch (Exception ex)
        {
            string err = "Enter Father name properly";
            ShowAlert(err, true);
            lblerror.Text = err;
        }
    }
    protected void txtMotherName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtDob.Text = "";
            txtapaar.Text = "";
            txtAuthenticationIdNo.Text = "";
            txtDob.Focus();
            GenerateNewCaptchaImage();
            txtcode.Text = "";
        }
        catch (Exception ex)
        {
            string err = "Enter Mother name properly";
            ShowAlert(err, true);
            lblerror.Text = err;
        }
    }
    protected void txtGuardianName_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txtDob.Text = "";
            txtapaar.Text = "";
            txtAuthenticationIdNo.Text = "";
            GenerateNewCaptchaImage();
            txtcode.Text = "";
            txtDob.Focus();
        }
        catch (Exception ex)
        {
            string err = "Enter Guardian name properly";
            ShowAlert(err, true);
            lblerror.Text = err;
        }
    }

    // apaar change start
    protected void ddlConsentRelation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Rdoownertype.SelectedValue == "P")
            {
                if (String.IsNullOrEmpty(txtAppName.Text) ||
               String.IsNullOrEmpty(txtFatherName.Text) ||
               String.IsNullOrEmpty(txtMotherName.Text))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Empty Name fields are not allowed. Check All Name fields properly";
                    txtAppName.Focus();
                    ShowAlert(lblerror.Text, true);
                    return;
                }
            }
            else
            {
                if (String.IsNullOrEmpty(TxtGuardianName.Text))
                {
                    TxtGuardianName.Focus();
                    lblerror.Visible = true;
                    lblerror.Text = "Guardian Name is Empty";
                    ShowAlert(lblerror.Text, true);
                    return;

                }
            }

            txtproviderName.Text = "";

            switch (ddlConsentRelation.SelectedValue)
            {
                case "1":
                    txtproviderName.Text = txtAppName.Text;
                    break;

                case "2":
                    txtproviderName.Text = TxtGuardianName.Text;
                    break;

                case "3":
                    txtproviderName.Text = txtFatherName.Text;
                    break;

                case "4":
                    txtproviderName.Text = txtMotherName.Text;
                    break;

                default:
                    txtproviderName.Text = "";
                    break;
            }
            txtproviderName.Enabled = false;
            DateTime todaydate = DateTime.Now;
            DateTime inputdate = Convert.ToDateTime(txtDob.Text);

            int countAge = DateTime.Compare(todaydate.AddYears(-18), inputdate);

            BindApaarDeclaration(countAge);
        }
        catch (Exception ex)
        {
            throw new Exception("Enter Proper Date of Birth , 01-Jan-2005");
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

        authDoc = txtAuthenticationIdNo.Text.Trim();

        lblApaarDeclaration.Text =
                                  "I "
                                  + relationText +
                                  ", hereby voluntarily give my consent to NIELIT to use APAAR ID of "
                                  + txtAppName.Text +
                                  " (" + wardText + ") with APAAR ID as "
                                  + txtapaar.Text +
                                  " for validation of personal details."
                                  + "I understand that the APAAR ID may be used and shared only for limited, authorized purposes, "
                                  + "and that the information provided by me shall be kept confidential."
                                  + "The information w.r.t authentication document no. "
                                  + authDoc +
                                  " provided by me, is correct and valid to the best of my knowledge.";
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
            throw ex;
        }
    }
    // amit_apaar_api_changes_may_2026_end
    protected void ddlSalutaionName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSalutaionName.SelectedValue.ToUpper() == "MRS." || ddlSalutaionName.SelectedValue.ToUpper() == "MISS" || ddlSalutaionName.SelectedValue.ToUpper() == "MS.")
            {
                ddl_gender.SelectedValue = "Female";
                ddl_gender.Enabled = false;
            }
            else if (ddlSalutaionName.SelectedValue.ToUpper() == "MR.")
            {
                ddl_gender.SelectedValue = "Male";
                ddl_gender.Enabled = false;
            }
            //Added for transgender
            else if (ddlSalutaionName.SelectedValue == "X")
            {
                ddl_gender.SelectedValue = "Trans";
                ddl_gender.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
        }
    }
    protected void DdlApplied4Exam_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
            Int32 examcycleID = Convert.ToInt32(DdlExamCycle.SelectedValue);
            Int32 examid = Convert.ToInt32(DdlApplied4Exam.SelectedValue);
            Int32 applicantTypeID = (RdoAppliedAs.SelectedValue == "D") ? 1 : 2;

            if (DdlApplied4Exam.SelectedValue != "0")
            {
                Boolean isExempted = false;
                using (EConnectContext context = new EConnectContext())
                {
                    if (RdoAlreadyAppeared4Exam.SelectedValue == "Y")
                    {
                        string previousRollNo = TxtRno.Text.Trim();
                        if (string.IsNullOrEmpty(previousRollNo))
                        {
                            ShowAlert("Please enter previous roll number.");
                            return;
                        }
                        else
                        {
                            var rnoObj = context.CertificateExamApplications.Where(s => s.RollNumber.Equals(previousRollNo, StringComparison.OrdinalIgnoreCase) && s.CourseID == courseID).FirstOrDefault();
                            if (rnoObj != null)
                            {
                                int gradeID = 0;
                                if (rnoObj.ExamID != 0)
                                { gradeID = context.ResultGrades.Where(r => r.Code == "*" && r.VersionID == rnoObj.Exam.ResultGradeVersionID.Value).FirstOrDefault().ID; }
                                else
                                { gradeID = context.ResultGrades.Where(r => r.Code == "*" && r.VersionID == 1).FirstOrDefault().ID; } // By Default
                                if (gradeID > 0)
                                {
                                    if (rnoObj.ResultGradeID.HasValue && rnoObj.ResultUpdatedOn.HasValue)
                                    {
                                        DateTime resultdeclareDate = rnoObj.ResultUpdatedOn.Value.AddDays(7);
                                        if (rnoObj.ExemptedApplicationID == null && rnoObj.ResultGradeID == gradeID)
                                        {
                                            //now candidate is exempted for all exams other than previous exams.
                                            Int32 nextExamID = GetNextExamID(applicantTypeID, courseID, examcycleID);
                                            isExempted = (nextExamID == examid) ? true : false;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    ImgBtnPopupFee.Enabled = true;
                    ImgBtnPopupFee.ImageUrl = "~/images/popup1.jpg";
                    ImgBtnPopupFee.ToolTip = "Click here to view Fee Detail.";
                    int FeeTypeID = Convert.ToInt32(enmFeeType.RegistrationCumExaminationFee);
                    LblFeeTypeName.Text = Convert.ToString(enmFeeType.ExaminationFee);
                    int ExamId = Convert.ToInt32(DdlApplied4Exam.SelectedValue);
                    int ExamCycleId = Convert.ToInt32(DdlExamCycle.SelectedValue);
                    Int32 cscProcessingFee = 0;
                    if (Request.QueryString["Src"] != null)
                    {
                        if (Request.QueryString["Src"] == "CSC")
                        {
                            Int32 applicationtypeid = Convert.ToInt32(enmApplicationType.CertificateExamApplication);
                            Int32 activityid = Convert.ToInt32(enmCSCActivity.FillFormandDepositFee);
                            cscProcessingFee = CourseManager.GetCSCProcessingCharge(context, applicationtypeid, activityid);
                            InfoDiv.Attributes.Add("class", "modalPopup1");
                        }
                    }
                    int ApplicantTypeId = RdoAppliedAs.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
                    Int32 feeAmount = 0; Int32 lateFeeAmount = 0;
                    CourseManager.GetCertificateExamFee(courseID, ExamId, ApplicantTypeId, out feeAmount, out lateFeeAmount);
                    if (isExempted == true)
                        feeAmount = 0;
                    lblFeeDetail.Text = "Fee : Rs/- " + (feeAmount + lateFeeAmount + cscProcessingFee) + ".00";
                    LblNormalFee.Text = feeAmount.ToString("F");
                    LblLateFee.Text = lateFeeAmount.ToString("F");
                    lblProcessingFee.Text = cscProcessingFee.ToString("F");
                    LblTotalFee.Text = (feeAmount + lateFeeAmount + cscProcessingFee).ToString("F");
                    LblAmountInWords.Text = " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(LblTotalFee.Text.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";

                    //checking whether enclosures is applicable or not
                    if (context.Exams.Where(s => s.ID == ExamId).FirstOrDefault().IsBatchProcessable)
                    {
                        trenclosure.Visible = true;
                        trenclosure1.Visible = true;
                        tdencheading.InnerText = " 8. Enclosures / भेजें ";
                        tdencheading.ColSpan = 3;
                        tddeclarartion.InnerText = " 9. Declaration / घोषणा ";
                    }
                    else
                    {
                        trenclosure.Visible = false;
                        trenclosure1.Visible = false;
                        tddeclarartion.InnerText = " 8. Declaration / घोषणा ";
                    }
                }
                ;
            }
            else
            {
                ImgBtnPopupFee.ToolTip = "";
                ImgBtnPopupFee.Enabled = false;
                ImgBtnPopupFee.ImageUrl = "~/images/DisablePopup.PNG";
                lblFeeDetail.Text = "";

                trenclosure.Visible = false;
                trenclosure1.Visible = false;
                tddeclarartion.InnerText = " 8. Declaration / घोषणा ";
            }

            //Making Declaration
            if (ddlSalutaionName.SelectedValue != "0")
            {
                LblName.Text = txtAppName.Text;
                LblhName.Text = txtAppName.Text;
                if (ddlSalutaionName.SelectedValue == "Mrs.".Trim() || ddlSalutaionName.SelectedValue.Trim() == "Miss".Trim() || ddlSalutaionName.SelectedValue.Trim() == "Ms.".Trim())
                {
                    if (string.IsNullOrEmpty(TxtGuardianName.Text) == true && string.IsNullOrWhiteSpace(TxtGuardianName.Text))
                    {
                        Lblsalutation.Text = " daughter  of ";
                        Lblhsalutation.Text = " की पुत्री ";
                        Lblhdectype.Text = " करती ";
                    }
                    else
                    {
                        Lblhsalutation.Text = "की देखभाल";
                        Lblsalutation.Text = "in care of";
                        Lblhdectype.Text = " करती ";
                    }
                }
                else if (ddlSalutaionName.SelectedValue == "Others.".Trim())
                {
                    if (string.IsNullOrEmpty(TxtGuardianName.Text) == true && string.IsNullOrWhiteSpace(TxtGuardianName.Text))
                    {
                        Lblsalutation.Text = " child  of ";
                        Lblhsalutation.Text = " की संतान ";
                        Lblhdectype.Text = " करता ";
                    }
                    else
                    {
                        Lblhsalutation.Text = "की देखभाल";
                        Lblsalutation.Text = "in care of";
                        Lblhdectype.Text = " करता ";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(TxtGuardianName.Text) == true && string.IsNullOrWhiteSpace(TxtGuardianName.Text))
                    {
                        Lblsalutation.Text = " son of ";
                        Lblhsalutation.Text = " का पुत्र ";
                        Lblhdectype.Text = " करता ";
                    }
                    else
                    {
                        Lblhsalutation.Text = "की देखभाल";
                        Lblsalutation.Text = "in care of";
                        Lblhdectype.Text = " करता ";
                    }
                }
            }
            else
            {
                Lblhdectype.Text = " करता/करती ";
            }
            if (string.IsNullOrEmpty(TxtGuardianName.Text) == true && string.IsNullOrWhiteSpace(TxtGuardianName.Text))
            {
                LblDecFname.Text = " and Shri " + GetInitCap(txtFatherName.Text);
                LblDechfName.Text = " और श्री " + GetInitCap(txtFatherName.Text);
                LblDecMName.Text = " Smt " + GetInitCap(txtMotherName.Text);
                LblDechmName.Text = " श्रीमती " + GetInitCap(txtMotherName.Text);
            }
            else
            {
                LblDecFname.Text = "";
                LblDechfName.Text = "";
                LblDecMName.Text = "";
                LblDechmName.Text = "";
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message, true); }
    }
    protected void DdlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ImgBtnPopupFee.ToolTip = "";
            ImgBtnPopupFee.Enabled = false;
            ImgBtnPopupFee.ImageUrl = "~/images/DisablePopup.PNG";
            lblFeeDetail.Text = "";
            DdlApplied4Exam.Items.Clear();
            int courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
            int examCycleID = Convert.ToInt32(DdlExamCycle.SelectedValue);
            ListItem lst = new ListItem("--Select Exam Name--", "0");

            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["id"])))
            {
                Int32 applicantType = (RdoAppliedAs.SelectedValue.ToUpper() == "D") ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlApplied4Exam, GetNextExam(applicantType, courseID, examCycleID), lst);
                if (DdlApplied4Exam.Items.Count == 1)
                { throw new Exception("Cycle not open, Cannot Apply"); }
            }
            //Added for DVP-BCC 14 Jan 2023
            if (courseID == 175)
            {
                lblExamDate.Visible = true;
                DdlApplied4Exam.SelectedIndex = 1;
                DdlApplied4Exam.Enabled = false;
            }
            else
            {
                lblExamDate.Visible = false;
                DdlApplied4Exam.SelectedIndex = 0;
                DdlApplied4Exam.Enabled = true;
            }
            //

        }
        catch (Exception ex) { ShowAlert(ex.Message, true); }
    }
    protected void DdlExamCentre1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //DdlExamCentreState2.SelectedValue = "0";
            //DdlExamCentre2.Items.Clear();
            //DdlExamCentreState2_SelectedIndexChanged(DdlExamCentreState2, EventArgs.Empty);
            Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
            Int32 examCentreID1 = Convert.ToInt32(DdlExamCentre1.SelectedValue);
            using (var context = new EConnectContext())
            {
                //Commented Natasha email 21 Dec 2022
                //var ExamCentreTypeId1 = context.ExamCenters.Where(c => c.ID == examCentreID1).FirstOrDefault();
                // Int32 RegionalCenterID = context.CourseWiseStates.Where(c => c.CourseID == courseID && c.StateID == ExamCentreTypeId1.StateID).FirstOrDefault().RegionalCenterID;
                var ExamCentreTypeId1 = context.ExamCenters.Where(c => c.ID == examCentreID1).OrderBy(s => s.StateID).FirstOrDefault();
                //Added on 2 Jan 2023
                var WhetherDistributed = (from c in context.CourseWiseStates
                                          where c.CourseID == courseID && c.StateID == ExamCentreTypeId1.StateID
                                          select new { wd = c.Whetherdistributedistrictwise }).FirstOrDefault();
                Int32? RegionalCenterID;
                if (WhetherDistributed.wd == 1)
                {
                    RegionalCenterID = context.ExamCenters.Where(c => c.StateID == ExamCentreTypeId1.StateID && c.ID == examCentreID1).FirstOrDefault().RegionalCentreId;
                }
                else
                {
                    RegionalCenterID = context.CourseWiseStates.Where(c => c.CourseID == courseID && c.StateID == ExamCentreTypeId1.StateID).OrderBy(s => s.RegionalCenterID).FirstOrDefault().RegionalCenterID;
                }
                // Commented on 2 Jan 2023 Int32 RegionalCenterID = context.CourseWiseStates.Where(c => c.CourseID == courseID && c.StateID == ExamCentreTypeId1.StateID).OrderBy(s=> s.RegionalCenterID).FirstOrDefault().RegionalCenterID;
                var examState = (from g in context.CourseWiseStates
                                 join s in context.Locations on g.StateID equals s.ID
                                 where s.LocationTypeID == 2
                                 && s.ParentLocationID == 1
                                 && g.CourseID == courseID
                                 && g.RegionalCenterID == RegionalCenterID
                                 select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                examState = examState.OrderBy(s => s.TextField);
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlExamCentreState2, examState, new ListItem("--Select State--", "0"));
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
        }
    }
    protected void DdlExamCentreState1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindExamCentre(Convert.ToInt64(DdlExamCentreState1.SelectedValue), 0, DdlExamCentre1, enmExamCenterType.Both);
            DdlExamCentreState2.SelectedValue = "0";
            DdlExamCentre2.Items.Clear();
            DdlExamCentre2.Items.Add(new ListItem("--Select Location--", "0"));
        }
        catch (Exception ex)
        { lblerror.Text = ex.Message; }
    }
    protected void DdlExamCentreState2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            enmExamCenterType ExamCentreType = enmExamCenterType.Both;
            //Commented 2 Jan 2023  BindExamCentre(Convert.ToInt64(DdlExamCentreState2.SelectedValue), Convert.ToInt32(DdlExamCentre1.SelectedValue), DdlExamCentre2, ExamCentreType);
            BindExamCentre2(Convert.ToInt64(DdlExamCentreState2.SelectedValue), Convert.ToInt32(DdlExamCentre1.SelectedValue), DdlExamCentre2, ExamCentreType);
        }
        catch (Exception ex)
        { lblerror.Text = ex.Message; }
    }
    protected void ddlCorState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id1 = Convert.ToInt32(ddlCorState.SelectedValue);
            Ddldistrict.Items.Clear();
            BindDistrict(id1);
        }
        catch (Exception ex)
        { lblerror.Text = ex.Message; }
    }
    protected void DdlAccState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 stateid = Convert.ToInt32(DdlAccState.SelectedValue);
            BindAccCentre(stateid);
        }
        catch (Exception ex) { lblerror.Text = ex.Message; }
    }
    protected void Rdoownertype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Rdoownertype.SelectedValue == "P")//Parents
            {
                trfather.Visible = true;
                trmother.Visible = true;
                TxtGuardianName.Text = "";
                trguardian.Visible = false;
                trfather.Attributes.Add("class", "gdalternate1");
                trmother.Attributes.Add("class", "gdrow1");
                trgender.Attributes.Add("class", "gdalternate1");
                trdob.Attributes.Add("class", "trgdrow1calendar");
                trcategory.Attributes.Add("class", "gdalternate1");
                troccupation.Attributes.Add("class", "gdrow1");
                divgurdian.Style.Add("display", "none");
                ActiveInActive(true);
                if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                {
                    DdlExamCentre1.Enabled = false;

                    DdlExamCentreState1.Enabled = false;

                }
            }
            if (Rdoownertype.SelectedValue == "G")//Guardian
            {
                trguardian.Visible = true;
                txtFatherName.Text = "";
                txtMotherName.Text = "";
                trfather.Visible = false;
                trmother.Visible = false;
                trgender.Attributes.Add("class", "gdrow1");
                trdob.Attributes.Add("class", "trgdalternate1calendar");
                trcategory.Attributes.Add("class", "gdrow1");
                troccupation.Attributes.Add("class", "gdalternate1");
                divgurdian.Style.Add("display", "block");
                ActiveInActive(true);
                if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                {
                    DdlExamCentre1.Enabled = false;

                    DdlExamCentreState1.Enabled = false;

                }
            }

            txtDob.Text = "";//amit_apaar_may_2026_start


            //Added for DVP-BCC 14 Jan 2023
            Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
            if (courseID == 175)
            {
                divgurdian.Visible = false;
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message, true); }
    }
    protected void RdoAppliedAs_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
            BindExamCycle(courseID);
            DdlExamCycle_SelectedIndexChanged(DdlApplied4Exam, EventArgs.Empty);
            ShowApplicatantType();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
        }
    }
    protected void RdoAlreadyAppeared4Exam_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (RdoAlreadyAppeared4Exam.SelectedValue == "Y") // If Appeared is Yes
            {
                TrPreExamRno.Visible = true;
                TblFormDetail.Visible = false;
            }
            if (RdoAlreadyAppeared4Exam.SelectedValue == "N")//If Appeared is No
            {
                TrPreExamRno.Visible = false;
                TblFormDetail.Visible = true;

                Response.Redirect("~/CAND/CertificateRegistration.aspx?" + Request.QueryString.ToString());
            }
        }
        catch (Exception ex) { lblerror.Text = ex.Message; }
    }
    protected void RdDisability_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (RdDisability.SelectedValue == "Y")
            {
                trDisability1.Visible = true;
                trDisability2.Visible = true;

                divDisability.Style.Add("display", "block");
            }
            else
            {
                trDisability1.Visible = false;
                trDisability2.Visible = false;
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message, true); }
    }
    //protected void ddlOccupation_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlOccupation.SelectedValue == "5")//if occupation is gujarat gov.
    //        {
    //            Troccupation1.Visible = true;
    //            Troccupation2.Visible = true;
    //            Troccupation3.Visible = true;
    //            Troccupation4.Visible = true;
    //            Troccupation5.Visible = true;
    //            Troccupation6.Visible = true;
    //        }
    //        else
    //        {
    //            Troccupation1.Visible = false;
    //            Troccupation2.Visible = false;
    //            Troccupation3.Visible = false;
    //            Troccupation4.Visible = false;
    //            Troccupation5.Visible = false;
    //            Troccupation6.Visible = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}


    protected void RadioButtonListMIS_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (RadioButtonListMIS.SelectedValue == "Y") // If Appeared is Yes
            {
                TRMIS.Visible = true;
                TrPreExamRno.Visible = false;
                TblFormDetail.Visible = false;
            }
            if (RadioButtonListMIS.SelectedValue == "N")//If Appeared is No
            {
                TRMIS.Visible = false;
                TrPreExamRno.Visible = true;
                TblFormDetail.Visible = true;

                Response.Redirect("~/CAND/CertificateRegistration.aspx?" + Request.QueryString.ToString());
            }
        }
        catch (Exception ex) { lblerror.Text = ex.Message; }
    }

    protected void ImageButtonMIS_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (txtMIS.Text.Trim() == "")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                //txtMIS.Text = "";
                //16.02.2022
                txtMIS.Text = null;
                //
                lblerror.Text = "Please Enter Reference Number.";
                TrPreExamRno.Visible = false;
            }
            else
            {
                TrPreExamRno.Visible = false;
                BindMISState();
                ResetAll();
                //int courseID = Convert.ToInt32(Request.QueryString["id"]);
                string Number = txtMIS.Text.Trim();
                Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    var application = context.NielitCentreStudent.Where(p => p.Number.Trim() == Number).FirstOrDefault();
                    if (application != null && application.CourseID.ToString() != courseID.ToString())
                    {
                        lblerror.Text = "No details found for given reference number " + Number + " for selected course. Please check reference number and search again.";
                        return;
                    }
                    if (application != null)
                    {
                        ActiveInactivePrsnlDetail2(false);
                        TblFormDetail.Visible = true;
                        //Candidate Personal Detail...
                        if (!string.IsNullOrEmpty(application.Salutation))
                        { ddlSalutaionName.SelectedValue = application.Salutation.ToString(); }
                        else { ddlSalutaionName.Enabled = true; }
                        txtAppName.Text = GetInitCap(application.Name.ToString());
                        if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName) == true)
                        {
                            if (String.IsNullOrEmpty(application.FatherName))
                            { txtFatherName.Enabled = true; }
                            else { txtFatherName.Text = GetInitCap(application.FatherName); }
                            if (String.IsNullOrEmpty(application.MotherName))
                            { txtMotherName.Enabled = true; }
                            else { txtMotherName.Text = GetInitCap(application.MotherName); }
                        }
                        else
                        { TxtGuardianName.Text = GetInitCap(application.GuardianName); }
                        if (!string.IsNullOrEmpty(application.Gender))
                        { ddl_gender.SelectedValue = application.Gender; }
                        else { ddl_gender.Enabled = true; }
                        txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");
                        if (!string.IsNullOrEmpty(application.CastCategoryID.ToString()))
                        { ddlCategory.SelectedValue = application.CastCategoryID.ToString(); }
                        else { ddlCategory.Enabled = true; }
                        if (application.IsHandicaped)
                        { RdDisability.SelectedValue = (application.IsHandicaped == false) ? "N" : "Y"; }
                        else { RdDisability.Enabled = true; RdDisability.SelectedValue = "N"; }
                        if (application.IsHandicaped == true && application.IsHandicaped == null)
                        {
                            trDisability1.Visible = true; trDisability2.Visible = true;
                            DdlDisabilityType.Enabled = true; TxtDisabilityPercent.Enabled = true;
                        }
                        //jk sah on 13-Jan-2021------
                        if (application.Is_EWS.ToString() == "True")
                        {
                            RdisEWS.SelectedValue = "Y";
                        }
                        else
                        {
                            RdisEWS.SelectedValue = "N";
                        }
                        //----------------------

                        //else if (application.IsDisability == true && application.DisabilityTypeID != null)
                        //{
                        //    DdlDisabilityType.SelectedValue = application.DisabilityTypeID.Value.ToString();
                        //    TxtDisabilityPercent.Text = application.DisabilityPercentage.Value.ToString();
                        //}
                        if (ddlCategory.SelectedValue == "0")
                        { ddlCategory.Enabled = true; }
                        //if (!string.IsNullOrEmpty(application.OccupationID.ToString()))
                        //{ ddlOccupation.SelectedValue = application.OccupationID.ToString(); }

                        // Candidate Contact Detail...
                        TxtSTDcode.Text = application.StdNumber != null ? "0" + application.StdNumber.ToString() : "";
                        if (TxtSTDcode.Text == "0")
                        { TxtSTDcode.Text = ""; }
                        if (application.PhoneNumber.HasValue)
                        { txtCorPhoneNo.Text = application.PhoneNumber.Value.ToString(); }
                        txtCorMobileNo.Text = application.MobileNumber.ToString();
                        if (application.EmailAddress != null)
                        { txtEmailId.Text = application.EmailAddress.ToString(); }

                        //Candidate (By default Corespondance here) Address Detail...
                        TxtAddressLine1.Text = GetInitCap(application.CorAddressLine1);
                        TxtAddressLine2.Text = !string.IsNullOrEmpty(application.CorAddressLine2) && !string.IsNullOrWhiteSpace(application.CorAddressLine2) ? GetInitCap(application.CorAddressLine2) : "";
                        TxtAddressLine3.Text = !string.IsNullOrEmpty(application.CorAddressLine3) && !string.IsNullOrWhiteSpace(application.CorAddressLine3) ? GetInitCap(application.CorAddressLine3) : "";
                        if (application.CorCityName != null)
                        { TxtCity.Text = GetInitCap(application.CorCityName); }
                        if (application.CorStateID != 1)
                        {
                            ddlCorState.SelectedValue = application.CorStateID.ToString();
                            int id1 = Convert.ToInt32(ddlCorState.SelectedValue);
                            BindDistrict(id1);
                            if (application.CorDistrictID.HasValue)
                            { Ddldistrict.SelectedValue = application.CorDistrictID.ToString(); }
                        }
                        txtPinCode.Text = application.CorPinCode.ToString();

                        //Candidate Educational Qualification detail 
                        //DDLeducode.SelectedValue = application.EducationalQualificationID.ToString();
                        //if (application.PassingYear.HasValue)
                        //{ TxtYearOfPassing.Text = application.PassingYear.Value.ToString(); }
                        ImgBtnSearch.Visible = false;
                        ImgBtnReset.Visible = true;
                    }
                    else
                    {
                        RadioButtonListMIS.SelectedValue = "N";
                        TrPreExamRno.Visible = false;
                        lblerror.Visible = true;
                        txtMIS.Focus();
                        GenerateNewCaptchaImage();
                        //txtMIS.Text = "";
                        //16.02.2022
                        txtMIS.Text = null;
                        //
                        lblerror.Text = "No details found for given reference number " + Number + ". Please check reference number and search again.";
                        ImgBtnSearch.Visible = true;
                        ImgBtnReset.Visible = false;
                    }
                }
                ;
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }

    protected void BindMISState()
    {
        using (EConnectContext context = new EConnectContext())
        {
            var corstate = from s in context.Locations
                           where s.LocationTypeID == 2
                           select new { ValueField = s.ID, TextField = s.Name };
            corstate = corstate.OrderBy(s => s.TextField);
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCorState, corstate, new ListItem("--Select One--", "0"));
        }
    }

    protected void BindGender()
    {
        try
        {
            using (EConnectContext vContext = new EConnectContext())
            {
                var Gender = (from s in
                                 vContext.tblGender
                              select new { ValueField = s.genderCode, TextField = s.name }).ToList();

                EConnect.Utils.Common.ControlUtility.BindListObject(ddl_gender, Gender, new ListItem("--Select Gender--", "0"));

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    #endregion

    #region Images Validation through API on Upload Button

    //----------------Add by vishal------------------------------------/
    protected void UploadPhoto_Click(object sender, EventArgs e)
    {
        if (ImgUpload.HasFile)
        {
            string fileNamePhoto = "";
            photoPreview.ImageUrl = "";
            lblphotoShow.Text = "";
            lblerror.Text = "";
            string error = "";
            string status = "";
            string message = "";
            try
            {
                //16.02.2022
                if (ImgUpload.FileName.Length > 49)
                {
                    lblphotoShow.Visible = true;
                    lblerror.Visible = true;
                    photoPreview.ImageUrl = "";
                    lblphotoShow.Text = "Photo file name should be less than 44 characters.";
                    lblerror.Text = "Photo file name should be less than 44 characters.";
                    if (File.Exists(fileNamePhoto))
                    { File.Delete(fileNamePhoto); }
                    ImgUpload.PostedFile.InputStream.Dispose();
                    Session["ImgUpload"] = null;
                    throw new Exception("Photo file name should be less than 44 characters.");
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(Convert.ToString(ImgUpload.FileName), "^[a-zA-Z0-9()-_.\u00FC\u00DC ]*$"))
                {
                    lblphotoShow.Visible = true;
                    lblerror.Visible = true;
                    photoPreview.ImageUrl = "";
                    lblphotoShow.Text = "Photo file name should be English Alphabets and Numbers only(like a-zA-Z0-9).";
                    lblerror.Text = "Photo file name should be English Alphabets and Numbers only(like a-zA-Z0-9).";
                    if (File.Exists(fileNamePhoto))
                    { File.Delete(fileNamePhoto); }
                    ImgUpload.PostedFile.InputStream.Dispose();
                    Session["ImgUpload"] = null;
                    throw new Exception("Photo file name should be English Alphabets and Numbers only(like a-zA-Z0-9).");
                }
                if (!isvalidFileExtension1(ImgUpload))
                {
                    lblphotoShow.Visible = true;
                    lblerror.Visible = true;
                    photoPreview.ImageUrl = "";
                    lblphotoShow.Text = "Only jpeg/jpg extensions image is allowed.";
                    lblerror.Text = "Only jpeg/jpg extensions image is allowed.";
                    if (File.Exists(fileNamePhoto))
                    { File.Delete(fileNamePhoto); }
                    ImgUpload.PostedFile.InputStream.Dispose();
                    Session["ImgUpload"] = null;
                    throw new Exception("Only jpeg/jpg extensions image is allowed.");
                }
                if (!isvalidFileSize1(ImgUpload, 5120, 51200))
                {
                    lblphotoShow.Visible = true;
                    lblerror.Visible = true;
                    photoPreview.ImageUrl = "";
                    lblphotoShow.Text = "Photo file size should be 5 KB to 50 KB.";
                    lblerror.Text = "Photo file size should be 5 KB to 50 KB.";
                    if (File.Exists(fileNamePhoto))
                    { File.Delete(fileNamePhoto); }
                    ImgUpload.PostedFile.InputStream.Dispose();
                    Session["ImgUpload"] = null;
                    throw new Exception("Photo file size should be 5 KB to 50 KB.");
                }
                //16.02.2022

                //  string requestURL = "https://student.nielit.gov.in/nielitImageApi/index.php/ImageValidate";  // live URL of API
                string requestURL = "https://uat.nielit.in/nielitImageApi/v1/index.php/ImageValidate";  // local URL of API



                string folderPath = Server.MapPath("~/ImageUpload/");
                if (!Directory.Exists(folderPath))
                { Directory.CreateDirectory(folderPath); }
                string ImgNamePhoto = string.Empty;
                ImgNamePhoto = "P" + Path.GetFileName(ImgUpload.FileName);          //Photo_
                ImgUpload.SaveAs(folderPath + "/" + ImgNamePhoto);
                fileNamePhoto = Server.MapPath("~/ImageUpload/" + ImgNamePhoto);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                try
                {
                    WebClient wc = new WebClient();
                    byte[] bytess = wc.DownloadData(fileNamePhoto);
                    Dictionary<string, object> postParameters = new Dictionary<string, object>();
                    postParameters.Add("image", new FormUpload.FileParameter(bytess, Path.GetFileName(fileNamePhoto), "image/jpeg"));
                    string userAgent = "Someone";
                    HttpWebResponse imageApiResponse = FormUpload.MultipartFormPost(requestURL, userAgent, postParameters, "Authorization", "Test@123");

                    StreamReader apiResponseReader = new StreamReader(imageApiResponse.GetResponseStream());
                    lblphotoShow.Text = apiResponseReader.ReadToEnd();
                    DataTable messageTable = FormUpload.JSONToDataTable(lblphotoShow.Text);
                    if (messageTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < messageTable.Rows.Count; i++)
                        {
                            error = messageTable.Rows[i].Field<string>("error");
                            status = messageTable.Rows[i].Field<string>("status");
                            message = messageTable.Rows[i].Field<string>("message");
                        }
                        lblphotoShow.Text = message;
                    }
                    imageApiResponse.Close();
                }
                catch (Exception ex)
                {
                    message = "Invalid Photo/Image File.";
                    lblphotoShow.Visible = true;
                    lblerror.Visible = true;
                    photoPreview.ImageUrl = "";
                    lblphotoShow.Text = message;
                    lblerror.Text = message;
                    if (File.Exists(fileNamePhoto))
                    { File.Delete(fileNamePhoto); }
                    ImgUpload.PostedFile.InputStream.Dispose();
                    Session["ImgUpload"] = null;
                    throw new Exception(message);
                }
                if (status == "true" && message == "Image validated successfully")
                {
                    string ImgNamePhoto1 = string.Empty;
                    ImgNamePhoto1 = "P" + Path.GetFileName(ImgUpload.FileName);          //Photo_
                    photoPreview.ImageUrl = "";
                    photoPreview.ImageUrl = "~/ImageUpload/" + ImgNamePhoto1;
                    ImgUpload.ResolveUrl(photoPreview.ImageUrl);
                    lblphotoShow.Visible = true;
                    lblphotoShow.Text = "Photo successfully Uploaded.";
                    Session["ImgUpload"] = ImgUpload;
                }
                else
                {
                    lblphotoShow.Visible = true;
                    lblerror.Visible = true;
                    photoPreview.ImageUrl = "";
                    lblphotoShow.Text = message;
                    lblerror.Text = message;
                    if (File.Exists(fileNamePhoto))
                    { File.Delete(fileNamePhoto); }
                    ImgUpload.PostedFile.InputStream.Dispose();
                    Session["ImgUpload"] = null;
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(fileNamePhoto))
                { File.Delete(fileNamePhoto); }
                lblerror.Text = ex.Message;
                lblerror.Visible = true;
                ImgUpload.PostedFile.InputStream.Dispose();
                Session["ImgUpload"] = null;
                ShowAlert(ex.Message);
            }
        }
    }
    protected void UploadSignature_Click(object sender, EventArgs e)
    {
        if (ImgUploadSignature.HasFile)
        {
            string fileName = "";
            signPreview.ImageUrl = "";
            lblsignShow.Text = "";
            lblerror.Text = "";
            string status = "";
            string message = "";
            try
            {
                //16.02.2022
                if (ImgUploadSignature.FileName.Length > 49)
                {
                    lblsignShow.Visible = true;
                    lblerror.Visible = true;
                     
                    signPreview.ImageUrl = "";
                    lblsignShow.Text = "Signature file name should be less than 44 characters.";
                    lblerror.Text = "Signature file name should be less than 44 characters.";
                    if (File.Exists(fileName))
                    { File.Delete(fileName); }
                    ImgUploadSignature.PostedFile.InputStream.Dispose();
                    Session["ImgUploadSignature"] = null;
                    throw new Exception("Signature file name should be less than 44 characters.");
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(Convert.ToString(ImgUploadSignature.FileName), "^[a-zA-Z0-9()-_.\u00FC\u00DC ]*$"))
                {
                    lblsignShow.Visible = true;
                    lblerror.Visible = true;
                    signPreview.ImageUrl = "";
                    lblsignShow.Text = "Signature file name should be English Alphabets and Numbers only(like a-zA-Z0-9).";
                    lblerror.Text = "Signature file name should be English Alphabets and Numbers only(like a-zA-Z0-9).";
                    if (File.Exists(fileName))
                    { File.Delete(fileName); }
                    ImgUploadSignature.PostedFile.InputStream.Dispose();
                    Session["ImgUploadSignature"] = null;
                    throw new Exception("Signature file name should be English Alphabets and Numbers only(like a-zA-Z0-9).");
                }
                if (!isvalidFileExtension1(ImgUploadSignature))
                {
                    lblsignShow.Visible = true;
                    lblerror.Visible = true;
                    signPreview.ImageUrl = "";
                    lblsignShow.Text = "Only jpeg/jpg extensions image is allowed";
                    lblerror.Text = "Only jpeg/jpg extensions image is allowed";
                    if (File.Exists(fileName))
                    { File.Delete(fileName); }
                    ImgUploadSignature.PostedFile.InputStream.Dispose();
                    Session["ImgUploadSignature"] = null;
                    throw new Exception("Only jpeg/jpg extensions image is allowed");
                }
                if (!isvalidFileSize1(ImgUploadSignature, 5120, 20480))
                {
                    lblsignShow.Visible = true;
                    lblerror.Visible = true;
                    signPreview.ImageUrl = "";
                    lblsignShow.Text = "Signature file size should be 5 KB to 20 KB.";
                    lblerror.Text = "Signature file size should be 5 KB to 20 KB.";
                    if (File.Exists(fileName))
                    { File.Delete(fileName); }
                    ImgUploadSignature.PostedFile.InputStream.Dispose();
                    Session["ImgUploadSignature"] = null;
                    throw new Exception("Signature file size should be 5 KB to 20 KB.");
                }
                //16.02.2022

                //string requestURL = "https://student.nielit.gov.in/nielitImageApi/v1/index.php/ThumbSignatureValidate";  // live URL of API
                string requestURL = "https://uat.nielit.in/nielitImageApi/v1/index.php/ThumbSignatureValidate";  // local URL of API

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string folderPath = Server.MapPath("~/ImageUpload/");
                if (!Directory.Exists(folderPath))
                { Directory.CreateDirectory(folderPath); }
                string ImgNameSig = string.Empty;
                ImgNameSig = "S" + Path.GetFileName(ImgUploadSignature.FileName);         //Sig_
                ImgUploadSignature.SaveAs(folderPath + "/" + ImgNameSig);
                fileName = Server.MapPath("~/ImageUpload/" + ImgNameSig);
                try
                {
                    WebClient wc = new WebClient();
                    byte[] bytess = wc.DownloadData(fileName);
                    Dictionary<string, object> postParameters = new Dictionary<string, object>();
                    postParameters.Add("image", new FormUpload.FileParameter(bytess, Path.GetFileName(fileName), "image/jpeg"));
                    string userAgent = "Someone";
                    HttpWebResponse imageApiResponse = FormUpload.MultipartFormPost(requestURL, userAgent, postParameters, "Authorization", "Test@123");
                    StreamReader apiResponseReader = new StreamReader(imageApiResponse.GetResponseStream());
                    lblsignShow.Text = apiResponseReader.ReadToEnd();
                    DataTable messageTable = FormUpload.JSONToDataTable(lblsignShow.Text);
                    if (messageTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < messageTable.Rows.Count; i++)
                        {
                            status = messageTable.Rows[i].Field<string>("status");
                            message = messageTable.Rows[i].Field<string>("message");
                        }
                        lblsignShow.Text = message;
                    }
                    imageApiResponse.Close();
                }
                catch (Exception ex)
                {
                    message = "Invalid Signature/Image File.";
                    lblsignShow.Visible = true;
                    lblerror.Visible = true;
                    signPreview.ImageUrl = "";
                    lblsignShow.Text = message;
                    lblerror.Text = message;
                    if (File.Exists(fileName))
                    { File.Delete(fileName); }
                    ImgUploadSignature.PostedFile.InputStream.Dispose();
                    Session["ImgUploadSignature"] = null;
                    throw new Exception(message);
                }

                if (status == "true" && message == "Image validated successfully")
                {
                    string ImgNamesig1 = string.Empty;
                    ImgNamesig1 = "S" + Path.GetFileName(ImgUploadSignature.FileName);         //Sig_
                    signPreview.ImageUrl = "";
                    signPreview.ImageUrl = "~/ImageUpload/" + ImgNamesig1;
                    ImgUploadSignature.ResolveUrl(signPreview.ImageUrl);
                    lblsignShow.Visible = true;
                    lblsignShow.Text = "Signature successfully Uploaded.";
                    Session["ImgUploadSignature"] = ImgUploadSignature;
                }
                else
                {
                    lblsignShow.Visible = true;
                    lblerror.Visible = true;
                    signPreview.ImageUrl = "";
                    lblsignShow.Text = message;
                    lblerror.Text = message;
                    if (File.Exists(fileName))
                    { File.Delete(fileName); }
                    ImgUploadSignature.PostedFile.InputStream.Dispose();
                    Session["ImgUploadSignature"] = null;
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(fileName))
                { File.Delete(fileName); }
                lblerror.Text = ex.Message;
                lblerror.Visible = true;
                ImgUploadSignature.PostedFile.InputStream.Dispose();
                Session["ImgUploadSignature"] = null;
                ShowAlert(ex.Message);
            }
        }
    }
    protected void UploadThumb_Click(object sender, EventArgs e)
    {
        if (ImgUploadThumb.HasFile)
        {
            string fileName = "";
            thumbPreview.ImageUrl = "";
            lblthumbshow.Text = "";
            lblerror.Text = "";
            string status = "";
            string message = "";
            try
            {
                // 16.02.2022
                if (ImgUploadThumb.FileName.Length > 49)
                {
                    lblthumbshow.Visible = true;
                    lblerror.Visible = true;
                    thumbPreview.ImageUrl = "";
                    lblthumbshow.Text = "Thumb Impression file name should be less than 44 characters.";
                    lblerror.Text = "Thumb Impression file name should be less than 44 characters.";
                    if (File.Exists(fileName))
                    { File.Delete(fileName); }
                    ImgUploadThumb.PostedFile.InputStream.Dispose();
                    Session["ImgUploadThumb"] = null;
                    throw new Exception("Thumb Impression file name should be less than 44 characters.");
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(Convert.ToString(ImgUploadThumb.FileName), "^[a-zA-Z0-9()-_.\u00FC\u00DC ]*$"))
                {
                    lblthumbshow.Visible = true;
                    lblerror.Visible = true;
                    thumbPreview.ImageUrl = "";
                    lblthumbshow.Text = "Thumb Impression file name should be English Alphabets and Numbers only(like a-zA-Z0-9).";
                    lblerror.Text = "Thumb Impression file name should be English Alphabets and Numbers only(like a-zA-Z0-9).";
                    if (File.Exists(fileName))
                    { File.Delete(fileName); }
                    ImgUploadThumb.PostedFile.InputStream.Dispose();
                    Session["ImgUploadThumb"] = null;
                    throw new Exception("Thumb Impression file name should be English Alphabets and Numbers only(like a-zA-Z0-9).");
                }
                if (!isvalidFileExtension1(ImgUploadThumb))
                {
                    lblthumbshow.Visible = true;
                    lblerror.Visible = true;
                    thumbPreview.ImageUrl = "";
                    lblthumbshow.Text = "Only jpeg/jpg extensions image is allowed.";
                    lblerror.Text = "Only jpeg/jpg extensions image is allowed.";
                    if (File.Exists(fileName))
                    { File.Delete(fileName); }
                    ImgUploadThumb.PostedFile.InputStream.Dispose();
                    Session["ImgUploadThumb"] = null;
                    throw new Exception("Only jpeg/jpg extensions image is allowed.");
                }
                if (!isvalidFileSize1(ImgUploadThumb, 5120, 20480))
                {
                    lblthumbshow.Visible = true;
                    lblerror.Visible = true;
                    thumbPreview.ImageUrl = "";
                    lblthumbshow.Text = "Thumb Impression file size should be 5 KB to 20 KB.";
                    lblerror.Text = "Thumb Impression file size should be 5 KB to 20 KB.";
                    if (File.Exists(fileName))
                    { File.Delete(fileName); }
                    ImgUploadThumb.PostedFile.InputStream.Dispose();
                    Session["ImgUploadThumb"] = null;
                    throw new Exception("Thumb Impression file size should be 5 KB to 20 KB.");
                }
                //16.02.2022

                //string requestURL = "https://student.nielit.gov.in/nielitImageApi/v1/index.php/ThumbSignatureValidate";  // live URL of API
                string requestURL = "https://uat.nielit.in/nielitImageApi/v1/index.php/ThumbSignatureValidate";  // local URL of API

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string folderPath = Server.MapPath("~/ImageUpload/");
                if (!Directory.Exists(folderPath))
                { Directory.CreateDirectory(folderPath); }
                string ImgNameLth = string.Empty;
                ImgNameLth = "L" + Path.GetFileName(ImgUploadThumb.FileName);         //Lth_
                ImgUploadThumb.SaveAs(folderPath + "/" + ImgNameLth);
                fileName = Server.MapPath("~/ImageUpload/" + ImgNameLth);
                try
                {
                    WebClient wc = new WebClient();
                    byte[] bytess = wc.DownloadData(fileName);
                    Dictionary<string, object> postParameters = new Dictionary<string, object>();
                    postParameters.Add("image", new FormUpload.FileParameter(bytess, Path.GetFileName(fileName), "image/jpeg"));
                    string userAgent = "Someone";
                    HttpWebResponse imageApiResponse = FormUpload.MultipartFormPost(requestURL, userAgent, postParameters, "Authorization", "Test@123");
                    StreamReader apiResponseReader = new StreamReader(imageApiResponse.GetResponseStream());
                    lblthumbshow.Text = apiResponseReader.ReadToEnd();
                    DataTable messageTable = FormUpload.JSONToDataTable(lblthumbshow.Text);
                    if (messageTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < messageTable.Rows.Count; i++)
                        {
                            status = messageTable.Rows[i].Field<string>("status");
                            message = messageTable.Rows[i].Field<string>("message");
                        }
                        lblthumbshow.Text = message;
                    }
                    imageApiResponse.Close();
                }
                catch (Exception ex)
                {
                    message = "Invalid Thumb Impression/Image File.";
                    lblthumbshow.Visible = true;
                    lblerror.Visible = true;
                    thumbPreview.ImageUrl = "";
                    lblthumbshow.Text = message;
                    lblerror.Text = message;
                    if (File.Exists(fileName))
                    { File.Delete(fileName); }
                    ImgUploadThumb.PostedFile.InputStream.Dispose();
                    Session["ImgUploadThumb"] = null;
                    throw new Exception(message);
                }

                if (status == "true" && message == "Image validated successfully")
                {
                    string ImgNameLth1 = string.Empty;
                    ImgNameLth1 = "L" + Path.GetFileName(ImgUploadThumb.FileName);         //Lth_
                    thumbPreview.ImageUrl = "";
                    thumbPreview.ImageUrl = "~/ImageUpload/" + ImgNameLth1;
                    ImgUploadThumb.ResolveUrl(thumbPreview.ImageUrl);
                    lblthumbshow.Visible = true;
                    lblthumbshow.Text = "Thumb Impression successfully Uploaded.";
                    Session["ImgUploadThumb"] = ImgUploadThumb;
                }
                else
                {
                    lblthumbshow.Visible = true;
                    lblerror.Visible = true;
                    thumbPreview.ImageUrl = "";
                    lblthumbshow.Text = message;
                    lblerror.Text = message;
                    if (File.Exists(fileName))
                    { File.Delete(fileName); }
                    ImgUploadThumb.PostedFile.InputStream.Dispose();
                    Session["ImgUploadThumb"] = null;
                    throw new Exception(message);
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(fileName))
                { File.Delete(fileName); }
                lblerror.Text = ex.Message;
                lblerror.Visible = true;
                ImgUploadThumb.PostedFile.InputStream.Dispose();
                Session["ImgUploadThumb"] = null;
                ShowAlert(ex.Message);
            }
        }
    }
    //----------------Add by vishal------------------------------------/
    #endregion Images Validation through API on Upload Button

    #region Images Validation through API on Upload Button---- old

    //----------------Add by vishal------------------------------------/
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

    //        try
    //        {
    //            if (isvalidFileSize1(ImgUpload, 5120, 51200))
    //            {
    //                if (isvalidFileExtension1(ImgUpload))
    //                {
    //                    try
    //                    {
    //                        string requestURL = "https://student.nielit.gov.in/nielitImageApi/v1/index.php/ImageValidate";  // live URL of API
    //                        //string requestURL = "http://14.139.53.83:8088/nielitImageApi/v1/ImageValidate";  // local URL of API
    //                        string folderPath = Server.MapPath("~/ImageUpload/");

    //                        if (!Directory.Exists(folderPath))
    //                        { Directory.CreateDirectory(folderPath); }
    //                        ImgUpload.SaveAs(folderPath + Path.GetFileName(ImgUpload.FileName));
    //                        fileNamePhoto = Server.MapPath("~/ImageUpload/" + Path.GetFileName(ImgUpload.FileName));
    //                        WebClient wc = new WebClient();
    //                        byte[] bytess = wc.DownloadData(fileNamePhoto);
    //                        Dictionary<string, object> postParameters = new Dictionary<string, object>();
    //                        postParameters.Add("image", new FormUpload.FileParameter(bytess, Path.GetFileName(fileNamePhoto), "image/jpeg"));
    //                        string userAgent = "Someone";
    //                        HttpWebResponse imageApiResponse = FormUpload.MultipartFormPost(requestURL, userAgent, postParameters, "Authorization", "Test@123");
    //                        StreamReader apiResponseReader = new StreamReader(imageApiResponse.GetResponseStream());
    //                        lblphotoShow.Text = apiResponseReader.ReadToEnd();
    //                        DataTable messageTable = FormUpload.JSONToDataTable(lblphotoShow.Text);
    //                        if (messageTable.Rows.Count > 0)
    //                        {
    //                            for (int i = 0; i < messageTable.Rows.Count; i++)
    //                            {
    //                                error = messageTable.Rows[i].Field<string>("error");
    //                                status = messageTable.Rows[i].Field<string>("status");
    //                                message = messageTable.Rows[i].Field<string>("message");
    //                            }
    //                            lblphotoShow.Text = message;
    //                        }
    //                        imageApiResponse.Close();
    //                        if (status == "true" && message == "Image validated successfully")
    //                        {
    //                            photoPreview.ImageUrl = "";
    //                            photoPreview.ImageUrl = "~/ImageUpload/" + Path.GetFileName(ImgUpload.FileName);
    //                            ImgUpload.ResolveUrl(photoPreview.ImageUrl);
    //                            lblphotoShow.Visible = true;
    //                            lblphotoShow.Text = "Photo successfully Uploaded.";
    //                            Session["ImgUpload"] = ImgUpload;
    //                        }
    //                        else
    //                        {
    //                            lblphotoShow.Visible = true;
    //                            lblerror.Visible = true;
    //                            photoPreview.ImageUrl = "";
    //                            lblphotoShow.Text = message;
    //                            lblerror.Text = message;
    //                            if (File.Exists(fileNamePhoto))
    //                            { File.Delete(fileNamePhoto); }
    //                            ImgUpload.PostedFile.InputStream.Dispose();
    //                            Session["ImgUpload"] = "";
    //                            throw new Exception(message);
    //                        }
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        if (File.Exists(fileNamePhoto))
    //                        { File.Delete(fileNamePhoto); }
    //                        lblerror.Text = ex.Message;
    //                        lblerror.Visible = true;
    //                        ImgUpload.PostedFile.InputStream.Dispose();
    //                        Session["ImgUpload"] = "";
    //                        throw ex;
    //                    }
    //                }
    //                else
    //                {
    //                    lblphotoShow.Visible = true;
    //                    lblerror.Visible = true;
    //                    photoPreview.ImageUrl = "";
    //                    lblphotoShow.Text = "Only jpeg/jpg extensions image is allowed.";
    //                    lblerror.Text = "Only jpeg/jpg extensions image is allowed.";
    //                    if (File.Exists(fileNamePhoto))
    //                    { File.Delete(fileNamePhoto); }
    //                    throw new Exception("Only jpeg/jpg extensions image is allowed.");
    //                }
    //            }
    //            else if (!isvalidFileExtension1(ImgUpload))
    //            {
    //                lblphotoShow.Visible = true;
    //                lblerror.Visible = true;
    //                photoPreview.ImageUrl = "";
    //                lblphotoShow.Text = "Only jpeg/jpg extensions image is allowed.";
    //                lblerror.Text = "Only jpeg/jpg extensions image is allowed.";
    //                if (File.Exists(fileNamePhoto))
    //                { File.Delete(fileNamePhoto); }
    //                ImgUpload.PostedFile.InputStream.Dispose();
    //                Session["ImgUpload"] = "";
    //                throw new Exception("Only jpeg/jpg extensions image is allowed.");

    //            }
    //            else
    //            {
    //                lblphotoShow.Visible = true;
    //                lblerror.Visible = true;
    //                photoPreview.ImageUrl = "";
    //                lblphotoShow.Text = "Photo file size should be 5 KB to 50 KB.";
    //                lblerror.Text = "Photo file size should be 5 KB to 50 KB.";
    //                if (File.Exists(fileNamePhoto))
    //                { File.Delete(fileNamePhoto); }
    //                ImgUpload.PostedFile.InputStream.Dispose();
    //                Session["ImgUpload"] = "";
    //                throw new Exception("Photo file size should be 5 KB to 50 KB.");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            if (File.Exists(fileNamePhoto))
    //            { File.Delete(fileNamePhoto); }
    //            lblerror.Text = ex.Message;
    //            lblerror.Visible = true;
    //            ImgUpload.PostedFile.InputStream.Dispose();
    //            Session["ImgUpload"] = "";
    //            //
    //            ShowAlert(ex.Message);
    //        }
    //    }
    //    else
    //    {
    //        lblerror.Text = "Photo can not be left blank.";
    //        lblerror.Visible = true;
    //        lblphotoShow.Text = "Photo can not be left blank.";
    //        photoPreview.ImageUrl = "";
    //        lblphotoShow.Visible = true;
    //        throw new Exception("Photo can not be left blank.");
    //    }
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

    //        try
    //        {
    //            if (isvalidFileSize1(ImgUploadSignature, 5120, 20480))
    //            {
    //                if (isvalidFileExtension1(ImgUploadSignature))
    //                {
    //                    try
    //                    {
    //                        string requestURL = "https://student.nielit.gov.in/nielitImageApi/v1/index.php/ThumbSignatureValidate";  // live URL of API
    //                        //string requestURL = "http://14.139.53.83:8088/nielitImageApi/v1/ThumbSignatureValidate";  // local URL of API
    //                        string folderPath = Server.MapPath("~/ImageUpload/");
    //                        if (!Directory.Exists(folderPath))
    //                        { Directory.CreateDirectory(folderPath); }
    //                        ImgUploadSignature.SaveAs(folderPath + Path.GetFileName(ImgUploadSignature.FileName));
    //                        fileName = Server.MapPath("~/ImageUpload/" + Path.GetFileName(ImgUploadSignature.FileName));
    //                        WebClient wc = new WebClient();
    //                        byte[] bytess = wc.DownloadData(fileName);
    //                        Dictionary<string, object> postParameters = new Dictionary<string, object>();
    //                        postParameters.Add("image", new FormUpload.FileParameter(bytess, Path.GetFileName(fileName), "image/jpeg"));
    //                        string userAgent = "Someone";
    //                        HttpWebResponse imageApiResponse = FormUpload.MultipartFormPost(requestURL, userAgent, postParameters, "Authorization", "Test@123");
    //                        StreamReader apiResponseReader = new StreamReader(imageApiResponse.GetResponseStream());
    //                        lblsignShow.Text = apiResponseReader.ReadToEnd();
    //                        DataTable messageTable = FormUpload.JSONToDataTable(lblsignShow.Text);
    //                        if (messageTable.Rows.Count > 0)
    //                        {
    //                            for (int i = 0; i < messageTable.Rows.Count; i++)
    //                            {
    //                                status = messageTable.Rows[i].Field<string>("status");
    //                                message = messageTable.Rows[i].Field<string>("message");
    //                            }
    //                            lblsignShow.Text = message;
    //                        }
    //                        imageApiResponse.Close();
    //                        if (status == "true" && message == "Image validated successfully")
    //                        {
    //                            signPreview.ImageUrl = "";
    //                            signPreview.ImageUrl = "~/ImageUpload/" + Path.GetFileName(ImgUploadSignature.FileName);
    //                            ImgUploadSignature.ResolveUrl(signPreview.ImageUrl);
    //                            lblsignShow.Visible = true;
    //                            lblsignShow.Text = "Signature successfully Uploaded.";
    //                            Session["ImgUploadSignature"] = ImgUploadSignature;
    //                        }
    //                        else
    //                        {
    //                            lblsignShow.Visible = true;
    //                            lblerror.Visible = true;
    //                            signPreview.ImageUrl = "";
    //                            lblsignShow.Text = message;
    //                            lblerror.Text = message;
    //                            if (File.Exists(fileName))
    //                            { File.Delete(fileName); }
    //                            ImgUploadSignature.PostedFile.InputStream.Dispose();
    //                            Session["ImgUploadSignature"] = "";
    //                            throw new Exception(message);
    //                        }
    //                    }
    //                    catch (Exception ex)
    //                    {

    //                        if (File.Exists(fileName))
    //                        { File.Delete(fileName); }
    //                        lblerror.Text = ex.Message;
    //                        lblerror.Visible = true;
    //                        ImgUploadSignature.PostedFile.InputStream.Dispose();
    //                        Session["ImgUploadSignature"] = "";
    //                        throw ex;
    //                    }
    //                }
    //                else
    //                {
    //                    lblsignShow.Visible = true;
    //                    lblerror.Visible = true;
    //                    signPreview.ImageUrl = "";
    //                    lblsignShow.Text = "Only jpeg/jpg extensions image is allowed";
    //                    lblerror.Text = "Only jpeg/jpg extensions image is allowed";
    //                    if (File.Exists(fileName))
    //                    { File.Delete(fileName); }
    //                    ImgUploadSignature.PostedFile.InputStream.Dispose();
    //                    Session["ImgUploadSignature"] = "";
    //                    throw new Exception("Only jpeg/jpg extensions image is allowed");
    //                }
    //            }
    //            else if (!isvalidFileExtension1(ImgUploadSignature))
    //            {

    //                lblsignShow.Visible = true;
    //                lblerror.Visible = true;
    //                signPreview.ImageUrl = "";
    //                lblsignShow.Text = "Only jpeg/jpg extensions image is allowed";
    //                lblerror.Text = "Only jpeg/jpg extensions image is allowed";
    //                if (File.Exists(fileName))
    //                { File.Delete(fileName); }
    //                ImgUploadSignature.PostedFile.InputStream.Dispose();
    //                Session["ImgUploadSignature"] = "";
    //                throw new Exception("Only jpeg/jpg extensions image is allowed");
    //            }
    //            else
    //            {
    //                lblsignShow.Visible = true;
    //                lblerror.Visible = true;
    //                signPreview.ImageUrl = "";
    //                lblsignShow.Text = "Signature file size should be 5 KB to 20 KB.";
    //                lblerror.Text = "Signature file size should be 5 KB to 20 KB.";
    //                if (File.Exists(fileName))
    //                { File.Delete(fileName); }
    //                ImgUploadSignature.PostedFile.InputStream.Dispose();
    //                Session["ImgUploadSignature"] = "";
    //                throw new Exception("Signature file size should be 5 KB to 20 KB.");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            if (File.Exists(fileName))
    //            { File.Delete(fileName); }
    //            lblerror.Text = ex.Message;
    //            lblerror.Visible = true;
    //            ImgUploadSignature.PostedFile.InputStream.Dispose();
    //            Session["ImgUploadSignature"] = "";
    //            ShowAlert(ex.Message);
    //        }
    //    }
    //    else
    //    {
    //        lblerror.Text = "Signature can not be left blank.";
    //        lblerror.Visible = true;
    //        lblsignShow.Visible = true;
    //        lblsignShow.Text = "Signature can not be left blank.";
    //        txtAppName.Focus();
    //        throw new Exception("Signature can not be left blank.");
    //    }
    //}
    //protected void UploadThumb_Click(object sender, EventArgs e)
    //{
    //    if (ImgUploadThumb.HasFile)
    //    {
    //        string fileName = "";
    //        thumbPreview.ImageUrl = "";
    //        lblthumbshow.Text = "";
    //        lblerror.Text = "";
    //        string status = "";
    //        string message = "";

    //        try
    //        {
    //            if (isvalidFileSize1(ImgUploadThumb, 5120, 20480))
    //            {
    //                if (isvalidFileExtension1(ImgUploadThumb))
    //                {
    //                    try
    //                    {
    //                        string requestURL = "https://student.nielit.gov.in/nielitImageApi/v1/index.php/ThumbSignatureValidate";  // live URL of API
    //                        //string requestURL = "http://14.139.53.83:8088/nielitImageApi/v1/ThumbSignatureValidate";  // local URL of API
    //                        string folderPath = Server.MapPath("~/ImageUpload/");
    //                        if (!Directory.Exists(folderPath))
    //                        { Directory.CreateDirectory(folderPath); }
    //                        ImgUploadThumb.SaveAs(folderPath + Path.GetFileName(ImgUploadThumb.FileName));
    //                        fileName = Server.MapPath("~/ImageUpload/" + Path.GetFileName(ImgUploadThumb.FileName));
    //                        WebClient wc = new WebClient();
    //                        byte[] bytess = wc.DownloadData(fileName);
    //                        Dictionary<string, object> postParameters = new Dictionary<string, object>();
    //                        postParameters.Add("image", new FormUpload.FileParameter(bytess, Path.GetFileName(fileName), "image/jpeg"));
    //                        string userAgent = "Someone";
    //                        HttpWebResponse imageApiResponse = FormUpload.MultipartFormPost(requestURL, userAgent, postParameters, "Authorization", "Test@123");
    //                        StreamReader apiResponseReader = new StreamReader(imageApiResponse.GetResponseStream());
    //                        lblthumbshow.Text = apiResponseReader.ReadToEnd();
    //                        DataTable messageTable = FormUpload.JSONToDataTable(lblthumbshow.Text);
    //                        if (messageTable.Rows.Count > 0)
    //                        {
    //                            for (int i = 0; i < messageTable.Rows.Count; i++)
    //                            {
    //                                status = messageTable.Rows[i].Field<string>("status");
    //                                message = messageTable.Rows[i].Field<string>("message");
    //                            }
    //                            lblthumbshow.Text = message;
    //                        }
    //                        imageApiResponse.Close();
    //                        if (status == "true" && message == "Image validated successfully")
    //                        {
    //                            thumbPreview.ImageUrl = "";
    //                            thumbPreview.ImageUrl = "~/ImageUpload/" + Path.GetFileName(ImgUploadThumb.FileName);
    //                            ImgUploadThumb.ResolveUrl(thumbPreview.ImageUrl);
    //                            lblthumbshow.Visible = true;
    //                            lblthumbshow.Text = "Thumb Impression successfully Uploaded.";
    //                            Session["ImgUploadThumb"] = ImgUploadThumb;
    //                        }
    //                        else
    //                        {
    //                            lblthumbshow.Visible = true;
    //                            lblerror.Visible = true;
    //                            thumbPreview.ImageUrl = "";
    //                            lblthumbshow.Text = message;
    //                            lblerror.Text = message;
    //                            if (File.Exists(fileName))
    //                            { File.Delete(fileName); }
    //                            ImgUploadThumb.PostedFile.InputStream.Dispose();
    //                            Session["ImgUploadThumb"] = "";
    //                            throw new Exception(message);
    //                        }
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        if (File.Exists(fileName))
    //                        { File.Delete(fileName); }
    //                        ImgUploadThumb.PostedFile.InputStream.Dispose();
    //                        Session["ImgUploadThumb"] = "";
    //                        lblerror.Text = ex.Message;
    //                        lblerror.Visible = true;
    //                        throw ex;
    //                    }
    //                }
    //                else
    //                {
    //                    lblthumbshow.Visible = true;
    //                    lblerror.Visible = true;
    //                    thumbPreview.ImageUrl = "";
    //                    lblthumbshow.Text = "Only jpeg/jpg extensions image is allowed.";
    //                    lblerror.Text = "Only jpeg/jpg extensions image is allowed.";
    //                    if (File.Exists(fileName))
    //                    { File.Delete(fileName); }
    //                    ImgUploadThumb.PostedFile.InputStream.Dispose();
    //                    Session["ImgUploadThumb"] = "";
    //                    throw new Exception("Only jpeg/jpg extensions image is allowed.");
    //                }
    //            }
    //            else if (!isvalidFileExtension1(ImgUploadThumb))
    //            {
    //                lblthumbshow.Visible = true;
    //                lblerror.Visible = true;
    //                thumbPreview.ImageUrl = "";
    //                lblthumbshow.Text = "Only jpeg/jpg extensions image is allowed.";
    //                lblerror.Text = "Only jpeg/jpg extensions image is allowed.";
    //                if (File.Exists(fileName))
    //                { File.Delete(fileName); }
    //                ImgUploadThumb.PostedFile.InputStream.Dispose();
    //                Session["ImgUploadThumb"] = "";
    //                throw new Exception("Only jpeg/jpg extensions image is allowed.");
    //            }
    //            else
    //            {
    //                lblthumbshow.Visible = true;
    //                lblerror.Visible = true;
    //                thumbPreview.ImageUrl = "";
    //                lblthumbshow.Text = "Thumb Impression file size should be 5 KB to 20 KB.";
    //                lblerror.Text = "Thumb Impression file size should be 5 KB to 20 KB.";
    //                if (File.Exists(fileName))
    //                { File.Delete(fileName); }
    //                ImgUploadThumb.PostedFile.InputStream.Dispose();
    //                Session["ImgUploadThumb"] = "";
    //                throw new Exception("Thumb Impression file size should be 5 KB to 20 KB.");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            if (File.Exists(fileName))
    //            { File.Delete(fileName); }
    //            lblerror.Text = ex.Message;
    //            lblerror.Visible = true;
    //            ImgUploadThumb.PostedFile.InputStream.Dispose();
    //            Session["ImgUploadThumb"] = "";
    //            ShowAlert(ex.Message);
    //        }
    //    }
    //    else
    //    {
    //        lblerror.Text = "Thumb impression can not be left blank.";
    //        lblerror.Visible = true;
    //        lblthumbshow.Visible = true;
    //        lblthumbshow.Text = "Thumb impression can not be left blank.";
    //        throw new Exception("Thumb impression can not be left blank.");
    //    }
    //}
    //----------------Add by vishal------------------------------------/
    #endregion

    protected void btnNotification_Click1(object sender, EventArgs e)
    {
        try
        {
            divPopup.Style.Add("display", "block");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    //Added 2 Jan 2023
    protected void BindExamCentre2(Int64 StateID, Int32 CentreId, DropDownList ddl, enmExamCenterType examCentreType)
    {
        try
        {
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["id"])))
            {
                Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
                Int32 typeID = Convert.ToInt32(examCentreType);
                ListItem lst = new ListItem("--Select Location--", "0");
                using (EConnectContext vContext = new EConnectContext())
                {
                    var WhetherDistributed = (from c in vContext.CourseWiseStates
                                              where c.CourseID == courseID && c.StateID == StateID
                                              select new { wd = c.Whetherdistributedistrictwise }).FirstOrDefault();

                    if (WhetherDistributed.wd == 1)
                    {

                        int courseCategoryID = vContext.Courses.Find(courseID).CourseCategoryID;

                        var examcentre1 = vContext.ExamCenters.Where(s => s.ID == CentreId && s.StateID == StateID && s.CourseCategoryID == courseCategoryID && s.IsEnabled == true && (s.CourseID == courseID || s.CourseID == null)) //Change by Gaurav Chaurasia for Course wise centre option on 02 March 2021
                                                        .OrderBy(s => s.Name).Select(s => new { regionalCentreId = s.RegionalCentreId }).FirstOrDefault();

                        var examcentre = vContext.ExamCenters.Where(s => s.ID != CentreId && s.StateID == StateID && s.CourseCategoryID == courseCategoryID && s.IsEnabled == true && (s.CourseID == courseID || s.CourseID == null) && s.RegionalCentreId == examcentre1.regionalCentreId) //Change by Gaurav Chaurasia for Course wise centre option on 02 March 2021
                                                      .OrderBy(s => s.Name).Select(s => new { ValueField = s.ID, TextField = s.Code.ToUpper() + " - " + s.Name.ToUpper(), examCentreTypeID = s.ExamCentreTypeID });


                        if (examCentreType == enmExamCenterType.Primary || examCentreType == enmExamCenterType.Secondary)
                        { examcentre = examcentre.Where(e => e.examCentreTypeID == typeID); }
                        ddl.Items.Clear();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddl, examcentre, lst);



                    }
                    else
                    {
                        int courseCategoryID = vContext.Courses.Find(courseID).CourseCategoryID;
                        var examcentre = vContext.ExamCenters.Where(s => s.ID != CentreId && s.StateID == StateID && s.CourseCategoryID == courseCategoryID && s.IsEnabled == true && (s.CourseID == courseID || s.CourseID == null)) //Change by Gaurav Chaurasia for Course wise centre option on 02 March 2021
                                                            .OrderBy(s => s.Name).Select(s => new { ValueField = s.ID, TextField = s.Code.ToUpper() + " - " + s.Name.ToUpper(), examCentreTypeID = s.ExamCentreTypeID });
                        if (examCentreType == enmExamCenterType.Primary || examCentreType == enmExamCenterType.Secondary)
                        { examcentre = examcentre.Where(e => e.examCentreTypeID == typeID); }
                        ddl.Items.Clear();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddl, examcentre, lst);

                    }

                }
                ;
            }
        }
        catch (Exception ex) { throw ex; }
    }
    string checkDuplicateApaar(string apaarEncryptedCheck, int CourseId, int ExamId)
    {
        string duplicate = "0";
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var candRecord = (from c in context.CertificateExamApplications
                                  where c.CourseID == CourseId && c.ExamID == ExamId
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