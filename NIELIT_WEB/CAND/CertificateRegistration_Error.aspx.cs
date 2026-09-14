using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

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
            //if (Request.UrlReferrer == null)
				if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in" ) && (Request.UrlReferrer == null  || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "https://nielit.gov.in"))
            {
                Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
                Response.End();
                return;
            }

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
                    //Binding Occupation
                    Occupation(CurrentCourse);
                    //Binding Educational Qualification Dropdownlist
                    EducationalQualification(CurrentCourse);
                    //Binding State Name Dropdownlist
                    BindState(CurrentCourse.ID);
                    //Attachment
                    ShowAttachment(CurrentCourse.CourseCategoryID);
                    //ExamCycle
                    BindExamCycle(CurrentCourse.ID);
                    //ApplicantType
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
        RdoGender.Enabled = isActive;
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
        RdoGender.Enabled = isActive;
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
            using (EConnectContext context = new EConnectContext())
            {
                var ExamCycle = context.ExaminationCycles.Where(s => s.CourseID == courseID && s.ID != 7).Select(s => new { ValueField = s.ID, TextField = s.Name }).OrderBy(s => s.TextField);
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlExamCycle, ExamCycle, new ListItem("--Select Exam Cycle--", "0"));
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
                    { RdoGender.SelectedValue = applicationName.Gender; }
                    else
                    { RdoGender.Enabled = true; }

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
                    photoPreview.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])applicationName.Photo.BlobFile);
                    lblsignShow.Text = "Already Exists... ";
                    signPreview.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])applicationName.Signature.BlobFile);
                    lblthumbshow.Text = "Already Exists... ";
                    thumbPreview.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])applicationName.LeftThumbImpression.BlobFile);

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
            };
        }
        catch (Exception ex) { ShowAlert(ex.Message, true); }
    }
    protected void BindState(Int32 CurrentCourseId)
    {
        try
        {
            Int32 withdrawlid = Convert.ToInt32(enmAccreditationStatus.Withdrawal);
            ListItem lst = new ListItem("--Select State--", "0");
            using (EConnectContext vContext = new EConnectContext())
            {
                // Correspondence State
                var CorState = vContext.Locations.Where(l => l.LocationTypeID == 2).Select(l => new { ValueField = l.ID, TextField = l.Name }).OrderBy(s => s.TextField);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCorState, CorState, lst);
                // Accreditation State
                var AccState = (from l in vContext.Locations.Where(s => s.LocationTypeID == 2 && s.ParentLocationID == 1)
                                join i in vContext.Institutes on l.ID equals i.StateID
                                join aa in vContext.AccreditationDetails.Where(a => a.CourseID == CurrentCourseId && a.AccreditationStatusID != withdrawlid) on i.ID equals aa.InstituteID
                                select new { ValueField = l.ID, TextField = l.Name }).Distinct().OrderBy(s => s.TextField);
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlAccState, AccState, lst);
                // Exam State
                var ExamState = vContext.CourseWiseStates.Where(c => c.CourseID == CurrentCourseId)
                               .Join(vContext.Locations.Where(l => l.LocationTypeID == 2 && l.ParentLocationID == 1), c => c.StateID, l => l.ID, (c, l) => new { ValueField = l.ID, TextField = l.Name }).Distinct().OrderBy(s => s.TextField);
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlExamCentreState1, ExamState, lst);
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlExamCentreState2, ExamState, lst);
            };
        }
        catch (Exception ex) { throw ex; }
    }
    protected void BindDistrict(int stateID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                if (stateID != 0)
                {
                    var district = context.Locations.Where(s => s.LocationTypeID == 4 && s.ParentLocationID == stateID).Select(s => new { ValueField = s.ID, TextField = s.Name }).OrderBy(s => s.TextField);
                    EConnect.Utils.Common.ControlUtility.BindListObject(Ddldistrict, district, new ListItem("--Select District--", "0"));
                }
            };
        }
        catch (Exception ex) { throw ex; }
    }
    protected void BindAccCentre(int stateid)
    {
        try
        {
            Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
            Int32 withdrawlid = Convert.ToInt32(enmAccreditationStatus.Withdrawal);
            using (EConnectContext context = new EConnectContext())
            {
                var AccCentre = context.Institutes.Where(i => i.StateID == stateid)
                               .Join(context.AccreditationDetails.Where(d => d.CourseID == courseID && d.AccreditationStatusID != withdrawlid), i => i.ID, d => d.InstituteID,
                                (i, d) => new { ValueField = i.ID, TextField = i.Name + ", " + (!string.IsNullOrEmpty(i.CityName) ? i.CityName : "") + " ( " + d.AccreditationNumber + " ) " }).OrderBy(s => s.TextField);
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlAccCentre, AccCentre, new ListItem("--Select Institute--", "0"));
            };
        }
        catch (Exception ex) { throw ex; }
    }
    protected void GenerateNewCaptchaImage()
    {
        try
        {
            ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateRandomNumber(6);
            EConnect.CaptchaImage captcha = new CaptchaImage(ViewState["CaptchCode"].ToString(), 200, 50, "Century Schoolbook");
            imgcap.Src = captcha.ImageSource;
        }
        catch (Exception ex) { ShowAlert(ex.Message, true); }
    }
    protected void GetApplicantType(Course CurrentCourse)
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

            if (!Char.IsLetter(txtAppName.Text, txtAppName.Text.Length - 1))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Applicant Name should end with an alphabet.");
            }

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
            if (!String.IsNullOrEmpty(txtaadhar.Text))
            {
                if (!IsNumeric(txtaadhar.Text) || !(txtaadhar.Text.Length == 12))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Not valid Aadhar Number.");
                }
            }

            if (CatgName != "IRDA")
            {

                if (ImgUpload.HasFile == false && string.IsNullOrEmpty(lblphotoShow.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Please select your photograph to upload");
                }
                if (ImgUpload.HasFile && !isvalidFileExtension(ImgUpload))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Invalid Photograph file. Only files with jpg, gif, jpeg ,png extensions are allowed.");
                }
                if (ImgUpload.HasFile && !isvalidFileSize(ImgUpload, 51200))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Photograph file size should be of 50 KB or less.");
                }
                if (ImgUpload.HasFile && ImgUpload.FileName.Length > 50)
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Photograph file name should be less than 50 characters.");
                }
                if (ImgUploadSignature.HasFile == false && string.IsNullOrEmpty(lblsignShow.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Please select your signature file to upload");
                }
                if (ImgUploadSignature.HasFile && !isvalidFileExtension(ImgUploadSignature))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Invalid Signature file. Only files with jpg, gif, jpeg ,png extensions are allowed.");
                }
                if (ImgUploadSignature.HasFile && !isvalidFileSize(ImgUploadSignature, 51200))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Signature file size should be of 50 KB or less.");
                }
                if (ImgUploadSignature.HasFile && ImgUploadSignature.FileName.Length > 50)
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Signature file name should be less than 50 characters.");
                }
                if (ImgUploadThumb.HasFile == false && string.IsNullOrEmpty(lblthumbshow.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Please select your Thumb Impression file to upload");
                }
                if (ImgUploadThumb.HasFile && !isvalidFileExtension(ImgUploadThumb))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Invalid Thumb Impression file. Only files with jpg, gif, jpeg ,png extensions are allowed.");
                }
                if (ImgUploadThumb.HasFile && !isvalidFileSize(ImgUploadThumb, 51200))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Thumb Impression file size should be of 50 KB or less.");
                }
                if (ImgUploadThumb.HasFile && ImgUploadThumb.FileName.Length > 50)
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Thumb Impression file name should be less than 50 characters.");
                }
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
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ResetAll()
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
    protected void ShowApplicatantType()
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
                        TxtGuardianName.Text = GetInitCap(application.GuardianName);
                    }
                    RdoGender.SelectedValue = application.Gender;
                    RdoGender.Enabled = false;
                    txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");
                    ddlCategory.SelectedValue = application.CastCategoryID.ToString();
                    ddlOccupation.SelectedValue = application.OccupationID.ToString();
                    RdDisability.SelectedValue = (application.IsDisability == false) ? "N" : "Y";
                    if (application.IsDisability == true)
                    {
                        DdlDisabilityType.SelectedValue = application.DisabilityTypeID.Value.ToString();
                        TxtDisabilityPercent.Text = application.DisabilityPercentage.Value.ToString();
                    }
                    //checking the occuption if it is gujarat govt.
                    if (application.OccupationID == 5)
                    {
                        Txtdept.Text = GetInitCap(application.Department); ;
                        txtdesg.Text = GetInitCap(application.Designation); ;
                        Txtempcode.Text = GetInitCap(application.EmployeeCode); ;
                        txtDojoin.Text = application.DateofJoining.HasValue ? application.DateofJoining.Value.ToString("dd-MMM-yyyy") : "N/A";
                        txtDoretment.Text = application.DateofRetirement.HasValue ? application.DateofRetirement.Value.ToString("dd-MMM-yyyy") : "N/A";
                        txtpostcity.Text = GetInitCap(application.PostingCity);                       
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
                    //showing photo, sign,thumb name
                    if (application.Photo != null)
                    {
                        lblphotoShow.Visible = true;
                        lblphotoShow.Text = "Already Exists... ";
                        lblphoto.Text = "Upload Photo / फोटो अपलोड ";                       
                        photoPreview.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])application.Photo);
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
                        signPreview.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])application.Signature);
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
                        thumbPreview.Src = "data:image/jpg;base64," + Convert.ToBase64String((byte[])application.LeftThumb);
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
                    txtaadhar.Text = (application.AadharNumber.HasValue) ? application.AadharNumber.Value.ToString() : "";
                    if (application.CourseCategory.Name == "IRDA")
                    {
                        ActiveInactivePrsnlDetail(false);
                    }
                }
            };
        }
        catch (Exception ex) { throw ex; }
    }
    protected void ShowAttachment(Int32 CourseCategoryId)
    {
        try
        {
            Int32 DownloadableTypeID = Convert.ToInt32(enmDownloadableType.Notice);
            using (EConnectContext context = new EConnectContext())
            {
                var objData = context.Downloadables.Where(s => s.CourseCategoryID == CourseCategoryId && s.DownloadableTypeID == DownloadableTypeID && s.ShowOnWeb == true && s.EffectiveFromDate <= DateTime.Now)
                             .Join(context.UploadedFiles, d => d.DownloadableFileID, u => u.ID, (d, u) => new { ID = d.ID, filename = u.BlobFile, fname = u.OriginalName, linkname = d.LinkName, fileID = d.DownloadableFileID.Value, effectiveDate = d.EffectiveFromDate })
                             .OrderByDescending(s => s.effectiveDate).FirstOrDefault();
                if (objData != null)
                { link.HRef = "../Handlers/UploadedFileHandler.ashx?ID=" + objData.fileID; }
                else
                { link.HRef = ""; }
            };
        }
        catch (Exception ex) { ShowAlert(ex.Message, true); }
    }
    private void Occupation(Course CurrentCourse)
    {
        using (EConnectContext vContext = new EConnectContext())
        {
            var OccupationList = vContext.CourseWiseOccupationMappings.Where(a => a.CourseID == CurrentCourse.ID).Select(k => k.OccupationID).Distinct();
            var Occupation = vContext.Occupations.Where(s => s.IsEnabled == true && OccupationList.Contains(s.ID)).OrderBy(s => s.DisplayOrder).
                              Select(s => new { ValueField = s.ID, TextField = s.Name + "/" + s.NameRegional });
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlOccupation, Occupation, new ListItem("--Select Occupation--", "0"));
        }
    }
    private void EducationalQualification(Course CurrentCourse)
    {
        using (EConnectContext vContext = new EConnectContext())
        {
            var EducationalQualification = vContext.EducationalQualifications.OrderBy(s => s.DisplayOrder)
                                           .Join(vContext.QualificationEligibility.Where(q => q.CourseID == CurrentCourse.ID), e => e.QualificationLevelID, q => q.QualificationLevelID, (e, q) => new { ValueField = e.ID, TextField = e.Name })
                                           .Distinct();
            EConnect.Utils.Common.ControlUtility.BindListObject(DDLeducode, EducationalQualification, new ListItem("--Select Qualification--", "0"));
        }
    }
    private void CastCategory()
    {
        using (EConnectContext vContext = new EConnectContext())
        {
            var CastCategoryType = vContext.CastCategories.Where(s => s.ID != 5).OrderBy(s => s.DisplayOrder).Select(s => new { ValueField = s.ID, TextField = s.Name + "/" + s.NameRegional });
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCategory, CastCategoryType, new ListItem("--Select Category--", "0"));
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
                    var examcentre = vContext.ExamCenters.Where(s => s.ID != CentreId && s.StateID == StateID && s.CourseCategoryID == courseCategoryID && s.IsEnabled == true)
                                                        .OrderBy(s => s.Name).Select(s => new { ValueField = s.ID, TextField = s.Code.ToUpper() + " - " + s.Name.ToUpper(), examCentreTypeID = s.ExamCentreTypeID });
                    if (examCentreType == enmExamCenterType.Primary || examCentreType == enmExamCenterType.Secondary)
                    { examcentre = examcentre.Where(e => e.examCentreTypeID == typeID); }
                    ddl.Items.Clear();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddl, examcentre, lst);
                };
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
            };
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
            };
            return ExamId;
        }
        catch (Exception ex) { throw ex; }
    }
    protected String DuplicateApplication(Int32 examID, Int32 NewFlag, Int32 UpdateFlag, Int64 applId)
    {
        try
        {
            String ApplicationNumber = string.Empty;
            DateTime dob = Convert.ToDateTime(txtDob.Text);
            using (EConnectContext vContext = new EConnectContext())
            {
                IQueryable<CertificateExamApplication> Application = vContext.CertificateExamApplications
                                                      .Where(
                                                                s => s.ExamID == examID
                                                                && s.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                                                && s.Gender == RdoGender.SelectedValue
                                                                && System.Data.Entity.DbFunctions.TruncateTime(s.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                                             );
                if (TxtGuardianName.Text.Trim().Length > 0)
                { Application = Application.Where(s => s.GuardianName.Equals(TxtGuardianName.Text.Trim(), StringComparison.OrdinalIgnoreCase)); }
                else
                { Application = Application.Where(s => s.FatherName.Equals(txtFatherName.Text.Trim(), StringComparison.OrdinalIgnoreCase) && s.MotherName.Equals(txtMotherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)); }

                if (NewFlag == 1)
                { var appl = Application.FirstOrDefault(); if (appl != null) { ApplicationNumber = appl.Number; } }
                else if (UpdateFlag == 1)
                { var appl = Application.Where(s => s.ID != applId).FirstOrDefault(); if (appl != null) { ApplicationNumber = appl.Number; } }
            };
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
            Boolean IsExemptedCase = false;
            Int64 ExamCentreTypeId1StateId = 0;            
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
                    ExamCentreTypeId1StateId = context.ExamCenters.Where(c => c.ID == ExamCentreID1).FirstOrDefault().StateID;
                    application.RegionalCenterID = context.CourseWiseStates.Where(c => c.CourseID == CourseId && c.StateID == ExamCentreTypeId1StateId).FirstOrDefault().RegionalCenterID;
                    // Candidate Name.
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
                    // Gender
                    string gender = RdoGender.SelectedItem.Text.ToString();
                    application.Gender = gender.Substring(0, gender.IndexOf('/')).Trim();
                    // DOB
                    application.DateOfBirth = Convert.ToDateTime(txtDob.Text.ToString());
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
                    if (!String.IsNullOrEmpty(txtaadhar.Text))
                    { application.AadharNumber = Convert.ToInt64(txtaadhar.Text); }
                    else
                    { application.AadharNumber = null; }
                    application.AadharVerfied = false;
                    // UID Type & Number
                    if (UidTypeDdl.SelectedValue != "0")
                    {
                        application.UIDType = Convert.ToInt32(UidTypeDdl.SelectedValue.ToString());
                        application.UIDNumber = UidNumberTxt.Text.ToUpper();
                    }
                    // Photo Upload
                    #region Photo
                    if (ImgUpload.HasFile)
                    {
                        if (ImgUpload.FileName.Length > 51200) //(50 * 1024)
                        { application.PhotoFileName = ImgUpload.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUpload.FileName).ToLower(); }
                        else
                        { application.PhotoFileName = ImgUpload.FileName; }
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
                        { application.SignatureFileName = ImgUploadSignature.FileName; }
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
                        { application.LeftThumbFileName = ImgUploadThumb.FileName; }
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
                        context.CertificateExamApplications.Add(application);
                        context.SaveChanges();
                        applId = application.ID;
                    }
                    else
                    {
                        if ((DuplicateApplicationNumber = DuplicateApplication(ExamId, 0, 1, applId)) == string.Empty)// Duplicate Check
                        {
                            context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                        else { throw new Exception("You have already applied for this exam.Please check your application status using your Application number which is :- " + DuplicateApplicationNumber); }
                    }
                }
            };

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
            txtAppName.Focus();
            ShowAlert(ex.Message);
        }
    }
    protected void btnOK_Click(object sender, EventArgs e)
    {
        ActiveInActive(true);
        divgurdian.Style.Add("display", "none");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ActiveInActive(true);
        divgurdian.Style.Add("display", "none");
        Rdoownertype.SelectedValue = "P";
        Rdoownertype_SelectedIndexChanged(Rdoownertype.SelectedValue, EventArgs.Empty);
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
                string RollNo = TxtRno.Text.Trim();
                using (EConnectContext context = new EConnectContext())
                {
                    var application = context.CertificateExamApplications.Where(p => p.RollNumber.Equals(RollNo, StringComparison.OrdinalIgnoreCase) && p.CourseID == courseID).FirstOrDefault();

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
                        { RdoGender.SelectedValue = application.Gender; }
                        else { RdoGender.Enabled = true; }
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
                        TxtRno.Text = "";
                        lblerror.Visible = true;
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        lblerror.Text = "No details found for given roll number " + RollNo + ". Please check roll number and search again.";
                        ImgBtnSearch.Visible = true;
                        ImgBtnReset.Visible = false;
                    }
                };
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
    protected void ddlSalutaionName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSalutaionName.SelectedValue.ToUpper() == "MRS." || ddlSalutaionName.SelectedValue.ToUpper() == "MISS" || ddlSalutaionName.SelectedValue.ToUpper() == "MS.")
            {
                RdoGender.SelectedValue = "Female";
                RdoGender.Enabled = false;
            }
            else if (ddlSalutaionName.SelectedValue.ToUpper() == "MR.")
            {
                RdoGender.SelectedValue = "Male";
                RdoGender.Enabled = false;
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
                };
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
                if (DdlApplied4Exam.Items.Count == 0)
                { DdlApplied4Exam.Items.Add(lst); }
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message, true); }
    }
    protected void DdlExamCentre1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
            Int32 examCentreID1 = Convert.ToInt32(DdlExamCentre1.SelectedValue);
            using (var vContext = new EConnectContext())
            {
                Int32 RegionalCenterID = vContext.CourseWiseStates.Where(c => c.CourseID == courseID && c.StateID == vContext.ExamCenters.Where(s => s.ID == examCentreID1).FirstOrDefault().StateID).FirstOrDefault().RegionalCenterID;
                var examState = vContext.CourseWiseStates.Where(g => g.CourseID == courseID && g.RegionalCenterID == RegionalCenterID)
                               .Join(vContext.Locations.Where(s => s.LocationTypeID == 2 && s.ParentLocationID == 1), c => c.StateID, l => l.ID, (c, l) => new { ValueField = l.ID, TextField = l.Name }).Distinct();
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
            BindExamCentre(Convert.ToInt64(DdlExamCentreState2.SelectedValue), Convert.ToInt32(DdlExamCentre1.SelectedValue), DdlExamCentre2, ExamCentreType);
        }
        catch (Exception ex)
        { lblerror.Text = ex.Message; }
    }

    protected void ddlCorState_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id1 = Convert.ToInt32(ddlCorState.SelectedValue);
        Ddldistrict.Items.Clear();
        BindDistrict(id1);        
    }
    protected void DdlAccState_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 stateid = Convert.ToInt32(DdlAccState.SelectedValue);
        BindAccCentre(stateid);
    }
    protected void Rdoownertype_SelectedIndexChanged(object sender, EventArgs e)
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
            ActiveInActive(false);
        }
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
        if (RdDisability.SelectedValue == "Y")
        {
            trDisability1.Visible = true;
            trDisability2.Visible = true;
        }
        else
        {
            trDisability1.Visible = false;
            trDisability2.Visible = false;
        }
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
    #endregion
}