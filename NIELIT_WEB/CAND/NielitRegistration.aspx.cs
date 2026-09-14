using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Globalization;

// added by amit start
using System.Configuration;
using System.Data;

using System.Data.SqlClient;
using System.IO;

using System.Net;


using System.Web;
using System.Web.DynamicData;


// added by amit end

public partial class NielitRegistration : BasePage
{
    Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
    int stateid;

    private int UploadRandomNumber
    {
        get
        {
            return Convert.ToInt32(ViewState["UploadRandomNumber"]);
        }
        set
        {
            ViewState["UploadRandomNumber"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(1);
            lblerror.Visible = false;
            //-------------- added_by_amit_audit_april_2026_start image upload code----------------------/
            Page.Header.DataBind();
            ImgUpload.Attributes["onchange"] = "UploadFile(this)";
            Page.Header.DataBind();
            ImgUploadSignature.Attributes["onchange"] = "imageSignpreview(this)";
            Page.Header.DataBind();
            ImgUploadThumb.Attributes["onchange"] = "imageThumbpreview(this)";
            //-------------- added_by_amit_audit_april_2026_start image upload code----------------------/

            // if (Request.UrlReferrer == null)
            //if ((Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 29).Trim() != "https://student.nielit.gov.in") && (Request.UrlReferrer == null || Request.UrlReferrer.AbsoluteUri.ToString().Substring(0, 20).Trim() != "https://nielit.gov.in"))
            //{
            //    //Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
            //    string urldecoded = HttpUtility.HtmlDecode(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
            //    Response.Write(urldecoded);
            //    Response.End();
            //    return;
            //}
            //else
            //{
            //    string pageRef = Request.UrlReferrer.ToString();
            //    if (!pageRef.Contains("https://student.nielit.gov.in/"))
            //    {
            //        // Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
            //        string urldecoded = HttpUtility.HtmlDecode(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
            //        Response.Write(urldecoded);
            //        Response.End();
            //    }
            //}
            if (Request.QueryString["Src"] != null)
            {
                if (Request.QueryString["Src"] == "CSC")
                {
                    RdoUndergngDOEACC.SelectedValue = "D";
                    RdoUndergngDOEACC.Enabled = false;
                }
            }
            if (!IsPostBack)
            {
                Random rnd = new Random();
                UploadRandomNumber = rnd.Next(100000, 1000000);
                //added by ashutosh start

                txtConsentDate.Text = DateTime.Today.ToString("dd-MM-yyyy");

                txtConsentTime.Text = DateTime.Now.ToString("HH:mm");
                //added by ashutosh end


                //Added for BSB
                string pageRef = Request.UrlReferrer.ToString().ToLower();

                if (!pageRef.Contains("rulesforonlineregistrationolevel"))
                {
                    Session["isBSB"] = false;
                }

                //-------------- added_by_amit_audit_april_2026_start image upload code----------------------/
                //added by amit start
                hfaadhaar.Value = "";
                //added by amit end


                Session["ImgUpload"] = null;
                Session["ImgUploadSignature"] = null;
                Session["ImgUploadThumb"] = null;

                divPopup.Style.Add("display", "none");
                //-------------- added_by_amit_audit_april_2026_start image upload code----------------------/

                // Added by Amit start
                if (Session["isBSB"].ToString() == "1" || checkBSB())
                {
                    Label13.Text = "1.2.1";
                }
                // Added by Amit end
                ImgBtnPopupFee.ToolTip = "";
                ImgBtnPopupFee.Enabled = false;
                ImgBtnPopupFee.ImageUrl = "~/images/DisablePopup.PNG";
                lblFeeDetail.Text = "";
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    RenderPage(Convert.ToInt32(Request.QueryString["id"].ToString()));
                    Int32 ApplicantTypeId = Convert.ToInt32(enmApplicantType.Direct);
                    bindExamName(ApplicantTypeId);
                }
                Rdoownertype_SelectedIndexChanged(Rdoownertype.SelectedValue, EventArgs.Empty);
                GenerateNewCaptchaImage();
                bindMaritalStatus();
                bindCastCategory();
                bindReligion();
                bindCourse();
                bindState();
                //Added jksah
                BindGender();
                //SetConsentRelation();  // added for appar check
                //Added 19 Aug 2019
                using (EConnectContext context = new EConnectContext())
                {
                    Course currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["id"]));
                    ShowAttachment(currentCourse.CourseCategoryID);
                }
                //


                disableAll();
                GetApplicantType();
                bindEducational();
                bindPaymentOption();

                if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                {
                    ShowData();
                    Int32 ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
                    bindExamName(ApplicantTypeId);
                    btnSave.Text = "Update";
                }
                else
                {
                    btnSave.Text = "Submit";
                }
            }
            //  added_by_amit_audit_april_2026_start image upload code
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
                    ImgNamePhoto = "P" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUpload.FileName);          //Photo_
                    ImagePathPh.Text = ImgNamePhoto;
                    UploadPhoto1 = ImgNamePhoto;
                }
                else if (Session["ImgUpload"] != null && (!ImgUpload.HasFile))
                {
                    string ImgNamePhoto = string.Empty;
                    if (ImgUpload.HasFile)
                    {
                        ImgNamePhoto = "P" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUpload.FileName);          //Photo_
                    }
                    UploadPhoto = ImgNamePhoto;
                    ImgUpload = (FileUpload)Session["ImgUpload"]; //exception
                    ImagePathPh.Text = ImgNamePhoto; // 
                }
                else if (ImgUpload.HasFile)
                {
                    string ImgNamePhoto = string.Empty;
                    ImgNamePhoto = "P" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUpload.FileName);          //Photo_
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
                    ImgNameSig = "S" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadSignature.FileName);         //Sig_
                    Session["ImgUploadSignature"] = ImgUploadSignature;
                    ImagePathSig.Text = ImgNameSig;
                    UploadSing1 = ImgNameSig;
                }
                else if (Session["ImgUploadSignature"] != null && (!ImgUploadSignature.HasFile))
                {
                    string ImgNameSig = string.Empty;
                    if (ImgUploadSignature.HasFile)
                    {
                        ImgNameSig = "S" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadSignature.FileName);         //Sig_
                    }
                    UploadSing = ImgNameSig;
                    ImgUploadSignature = (FileUpload)Session["ImgUploadSignature"];
                    ImagePathSig.Text = ImgNameSig;
                }
                else if (ImgUploadSignature.HasFile)
                {
                    string ImgNameSig = string.Empty;
                    ImgNameSig = "S" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadSignature.FileName);         //Sig_
                    Session["ImgUploadSignature"] = ImgUploadSignature;
                    ImagePathSig.Text = ImgNameSig;
                }
                Page.Header.DataBind();
                ImgUploadSignature.Attributes["onchange"] = "imageSignpreview(this)";

                // Left Thump
                if (Session["ImgUploadThumb"] == null && ImgUploadThumb.HasFile)
                {
                    string ImgNameLth = string.Empty;
                    ImgNameLth = "L" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadThumb.FileName);         //Lth_
                    Session["ImgUploadThumb"] = ImgUploadThumb;
                    ImagePathLt.Text = ImgNameLth;
                    UploadLeftTh1 = ImgNameLth;
                }
                else if (Session["ImgUploadThumb"] != null && (!ImgUploadThumb.HasFile))
                {
                    string ImgNameLth = string.Empty;
                    if (ImgUploadThumb.HasFile)
                    {
                        ImgNameLth = "L" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadThumb.FileName);         //Lth_
                    }
                    UploadLeftTh = ImgNameLth;
                    ImgUploadThumb = (FileUpload)Session["ImgUploadThumb"];
                    ImagePathLt.Text = ImgNameLth;  //
                }
                else if (ImgUploadThumb.HasFile)
                {
                    string ImgNameLth = string.Empty;
                    ImgNameLth = "L" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadThumb.FileName);         //Lth_
                    Session["ImgUploadThumb"] = ImgUploadThumb;
                    ImagePathLt.Text = ImgNameLth;
                }
                Page.Header.DataBind();
                ImgUploadThumb.Attributes["onchange"] = "imageThumbpreview(this)";
            }
            #endregion
            // added_by_amit_audit_april_2026_end image upload code
            //base.ReWriteAction(this.Form);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }

    protected void bindPaymentOption()
    {
        using (EConnectContext context = new EConnectContext())
        {
            Course currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["id"]));
            string CourseCatg = currentCourse.CourseCategory.Name.ToString();
            if (CourseCatg == "IRDA" && RdoUndergngDOEACC.SelectedValue == "I")
            { ddlPaymentOption.Items.RemoveAt(0); }
            if (CourseCatg == "Digital Literacy Course" && RdoUndergngDOEACC.SelectedValue == "I")
            { ddlPaymentOption.Items.RemoveAt(0); }
        }
    }
    protected void bindExamName(Int32 ApplicantTypeId)
    {
        try
        {
            Int32 courseID = 0;
            Int32 NormalactivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
            Int32 LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee);
            using (EConnectContext context = new EConnectContext())
            {
                Int32 examID = 0;
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    courseID = Convert.ToInt32(Request.QueryString["id"]);
                }
                else if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                {
                    Int64 appID = Convert.ToInt64(Request.QueryString["Appid"]);
                    var app = context.CourseRegistrationApplications.Find(appID);
                    courseID = app.CourseID;

                }
                var LateFeeExam = (from e in context.CutOffDates
                                   join i in context.Exams on e.ExamID equals i.ID
                                   where e.CourseID == courseID
                                   && e.ApplicantTypeID == ApplicantTypeId
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
                        btnSave.Visible = true;
                    }
                }
                else if (LateFeeExam.Count() <= 0)//If  not applicable for late fee ?
                {

                    var NormalFeeExam = (from e in context.CutOffDates
                                         join i in context.Exams on e.ExamID equals i.ID
                                         where e.CourseID == courseID
                                         && e.ApplicantTypeID == ApplicantTypeId
                                         && e.ActivityID == NormalactivityId
                                         && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                         orderby e.EfferctiveDate ascending
                                         select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
                    if (NormalFeeExam.Count() > 0)
                    {
                        LblExamName.Text = NormalFeeExam.FirstOrDefault().ExamName;
                        examID = NormalFeeExam.FirstOrDefault().ExamID;
                        btnSave.Visible = true;
                    }

                }
                ViewState["ExamID"] = examID.ToString();
                // Added by Amit start
                if (DdlProject.SelectedValue != "0")
                    getFeeDetailForProjectCourse();
                else
                    ShowFeeDetail(examID);
                // Added by Amit end
            }
            ;
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
            if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int64 applID = Convert.ToInt64(Request.QueryString["Appid"]);
                    //This Query will get all the information of Applied Canditate by generated Application id                                     
                    var application = context.CourseRegistrationApplications.Find(applID);

                    //Added for BSB
                    if (application.isBSB.ToString() == "True")
                    {
                        trBSB.Visible = true;
                        txtUDISECode.Text = application.BSB_U_DISECode;
                        RdoUndergngDOEACC.SelectedValue = "D";
                        RdoUndergngDOEACC.SelectedItem.Text = "Direct(BSB)";
                    }
                    else
                        trBSB.Visible = false;
                    /////
                    if (application.enmApplicantType == enmApplicantType.Institute)
                    {
                        var instituteDetail = (from a in context.AccreditationDetails
                                               join i in context.Institutes
                                                   on a.InstituteID equals i.ID
                                               where i.ID == application.InstituteID
                                               select new
                                               {
                                                   Name = i.Name,
                                                   CentreID = i.ID,
                                                   StateId = i.StateID,
                                                   DistrictId = i.DistrictID,
                                                   StateName = i.State.Name,
                                                   DistrictName = i.District.Name,
                                                   AccNo = a.AccreditationNumber
                                               }).FirstOrDefault();

                        RdoUndergngDOEACC.SelectedValue = "I";


                        DdlAccState.SelectedValue = Convert.ToString(instituteDetail.StateId);
                        int id = Convert.ToInt32(DdlAccState.SelectedValue);
                        DdlAccState_SelectedIndexChanged(DdlAccState, EventArgs.Empty);
                        DdlAccCentre.SelectedValue = Convert.ToString(instituteDetail.CentreID);
                        //Added for NSQF Courses
                        if (application.CourseCategoryID.ToString() == "6" && Convert.ToInt32(application.CourseID.ToString()) > 102)
                        {
                            TxtExperienceInYears.Text = application.ExperienceInYears.ToString();
                        }
                        // Added by Amit start, 

                        if (application.projectID != null)
                        {


                            radProject.SelectedValue = "1";

                            if (DdlAccCentre.SelectedItem.Text.Contains("UPMSP"))
                            {
                                DdlAccState.SelectedValue = "28";
                                DdlAccState.Enabled = false;
                                bindUPProjectAccCentre(Convert.ToInt32(DdlAccState.SelectedValue), Convert.ToInt32(Request.QueryString["id"]));
                            }


                            // for radio project 
                            TrUPBoard.Visible = true;

                            // DdlAccState.SelectedValue = Convert.ToString(instituteDetail.StateId);
                            //DdlAccState_SelectedIndexChanged(DdlAccState, EventArgs.Empty);
                            //DdlAccCentre.SelectedValue = Convert.ToString(instituteDetail.CentreID);


                            if (application.CourseID != 0)
                            {
                                trUPProject.Visible = true;
                                bindUPProject(application.CourseID);
                                DdlProject.SelectedValue = Convert.ToString(application.projectID);
                                //DdlProject_SelectedIndexChanged(DdlProject, EventArgs.Empty);


                                //if (DdlProject.SelectedValue != "0")
                                //    getFeeDetailForProjectCourse();



                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ToString()))
                                {

                                    SqlCommand scCommand1 = new SqlCommand("SELECT [ID],[field1],[field2],[field3],[field4],[field5]  FROM [projCriteriaMaster] where projID=@projID", new SqlConnection(conn.ConnectionString));

                                    scCommand1.Parameters.AddWithValue("@projID", application.projectID.ToString());

                                    if (scCommand1.Connection.State == ConnectionState.Closed)
                                    {
                                        scCommand1.Connection.Open();
                                    }

                                    //DdlAccState.SelectedValue = application

                                    DataTable dt = new DataTable();
                                    SqlDataAdapter da = new SqlDataAdapter(scCommand1);
                                    //DataSet ds = new DataSet();
                                    da.Fill(dt);

                                    foreach (DataRow row in dt.Rows)
                                    {
                                        if (row["field1"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field1"].ToString()))
                                        {
                                            trBSB.Visible = true;
                                            Label14.Text = row["field1"].ToString();
                                            txtUDISECode.Text = application.field1.ToString();

                                        }
                                        else
                                        {
                                            trBSB.Visible = false;
                                            Label14.Text = "Field 1";
                                            txtUDISECode.Text = "";
                                        }


                                        if (row["field2"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field2"].ToString()))
                                        {
                                            trUPField2.Visible = true;
                                            lblField2.Text = row["field2"].ToString();
                                            txtField2.Text = application.field2.ToString();
                                        }
                                        else
                                        {
                                            trUPField2.Visible = false;
                                            lblField2.Text = "Field 2";
                                            txtField2.Text = "";
                                        }
                                        if (row["field3"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field3"].ToString()))
                                        {
                                            trUPField3.Visible = true;
                                            lblField3.Text = row["field3"].ToString();
                                            txtField3.Text = application.field3.ToString();
                                        }
                                        else
                                        {
                                            trUPField3.Visible = false;
                                            lblField3.Text = "Field 3";
                                            txtField3.Text = "";
                                        }
                                        if (row["field4"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field4"].ToString()))
                                        {
                                            trUPField4.Visible = true;
                                            lblField4.Text = row["field4"].ToString();
                                            txtField4.Text = application.field4.ToString();
                                        }
                                        else
                                        {
                                            trUPField4.Visible = false;
                                            lblField4.Text = "Field 4";
                                            txtField4.Text = "";
                                        }
                                        if (row["field5"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field5"].ToString()))
                                        {
                                            trUPField5.Visible = true;
                                            lblField5.Text = row["field5"].ToString();
                                            txtField5.Text = application.field5.ToString();
                                        }
                                        else
                                        {
                                            trUPField5.Visible = false;
                                            lblField5.Text = "Field 5";
                                            txtField5.Text = "";
                                        }
                                    }
                                }
                            }
                        }
                        // Added by Amit end , Added_06_01_2025_End 
                    }
                    else
                    {
                        RdoUndergngDOEACC.SelectedValue = "D";
                        TxtExperienceInYears.Text = application.ExperienceInYears.ToString();
                        //Added by Amit start code 20-6

                        trUPProject.Visible = false;

                        //Added by Amit end 
                    }
                    ddlPaymentOption.SelectedValue = application.PaymentSourceID.HasValue ? application.PaymentSourceID.Value.ToString() : (RdoUndergngDOEACC.SelectedValue == "D" ? "1" : "2");
                    //Candidate Personal Detail...
                    ddlSalutaionName.SelectedValue = application.Salutation.ToString();
                    txtAppName.Text = GetInitCap(application.Name.ToString());
                    lblname.Text = txtAppName.Text;
                    if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName) == true)
                    {
                        trfather.Visible = true;
                        trmother.Visible = true;
                        trguardian.Visible = false;
                        Rdoownertype.SelectedValue = "P";
                        Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                        txtFatherName.Text = string.IsNullOrEmpty(application.FatherName) == false && !string.IsNullOrWhiteSpace(application.FatherName) ? GetInitCap(application.FatherName) : "";
                        txtMotherName.Text = string.IsNullOrEmpty(application.MotherName) == false && !string.IsNullOrWhiteSpace(application.MotherName) ? GetInitCap(application.MotherName) : "";
                    }
                    else
                    {
                        trfather.Visible = false;
                        trmother.Visible = false;
                        trguardian.Visible = true;
                        Rdoownertype.SelectedValue = "G";
                        Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                        TxtGuardianName.Text = GetInitCap(application.GuardianName);
                        //Added 22 Feb 2019
                        txtAffidavitNo.Text = application.affidavitNo.ToString();
                        txtAffidavitDate.Text = application.affidavitDate.GetValueOrDefault().ToString("dd-MMM-yyyy");
                        // fileAffidavit.FileName = application.affidavitFile;
                        lblAffidavitFile.Text = application.affidavitFile;


                    }

                    //ddl_gender.SelectedValue = application.Gender;
                    //Added jksah
                    ddl_gender.SelectedValue = application.Gender;
                    txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");
                    txtBodyMark.Text = string.IsNullOrEmpty(application.BodyMark) == false && !string.IsNullOrWhiteSpace(application.BodyMark) ? application.BodyMark : "";
                    ddlCategory.SelectedValue = application.CastCategoryID.ToString();
                    ddlMStatus.SelectedValue = application.MaritalStatusID.ToString();
                    if (application.IsHandicaped.ToString() == "True")
                    {
                        Rdhandicapped.SelectedValue = "Y";
                    }
                    else
                    {
                        Rdhandicapped.SelectedValue = "N";
                    }
                    if (application.IsExServicemane.ToString() == "True")
                    {
                        Rdexserviceman.SelectedValue = "Y";
                    }
                    else
                    {
                        Rdexserviceman.SelectedValue = "N";
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
                    ddlReligion.SelectedValue = application.ReligionID.ToString();
                    //Candidate Contact Detail...

                    TxtSTDcode.Text = application.PhoneNumber.HasValue && application.PhoneNumber != 0 ? "0" + application.StdNumber.ToString() : "";
                    txtCorPhoneNo.Text = application.PhoneNumber.ToString();
                    txtCorMobileNo.Text = application.MobileNumber.ToString();
                    txtEmailId.Text = application.EmailAddress.ToString();



                    //added_by_amit_audit_april_2026_start
                    if (application.AadharNumber.HasValue)
                    {
                        //aadhaar change
                        string encryptedAadhaar = EncryptDecrypt.EncryptString(application.AadharNumber.Value.ToString());
                        hfaadhaar.Value = encryptedAadhaar;
                        txtaadhar.Text = "XXXXXXXX" + application.AadharNumber.Value.ToString().Substring(8);
                        txtaadhar.ReadOnly = true;
                        txtaadhar.Style["pointer-events"] = "none";
                    }
                    else
                    {
                        txtaadhar.Text = "";
                        txtaadhar.ReadOnly = false;
                        txtaadhar.Style["pointer-events"] = "auto";
                    }
                    // added_by_amit_audit_april_2026_end


                    /*
                    //aadhar details
                    if (application.AadharNumber.HasValue)
                        txtaadhar.Text = application.AadharNumber.Value.ToString();
                    else
                        txtaadhar.Text = "";
					*/

                    //Candidate Address Detail...
                    TxtPerAddressLine1.Text = string.IsNullOrEmpty(application.PerAddressLine1) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine1) ? GetInitCap(application.PerAddressLine1) : "";
                    TxtPerAddressLine2.Text = string.IsNullOrEmpty(application.PerAddressLine2) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine2) ? GetInitCap(application.PerAddressLine2) : "";
                    TxtPerAddressLine3.Text = string.IsNullOrEmpty(application.PerAddressLine3) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine3) ? GetInitCap(application.PerAddressLine3) : "";
                    TxtPerCity.Text = string.IsNullOrEmpty(application.PerCityName) == false && !string.IsNullOrWhiteSpace(application.PerCityName) ? GetInitCap(application.PerCityName) : "";
                    ddlPState.SelectedValue = application.PerStateID != null && application.PerStateID != 0 ? application.PerStateID.ToString() : "0";
                    int id1 = Convert.ToInt32(ddlPState.SelectedValue);
                    ddlPState_SelectedIndexChanged(ddlPState, EventArgs.Empty);
                    ddlPdistrict.SelectedValue = application.PerDistrictID.ToString();
                    TxtPpincode.Text = application.PerPinCode.ToString();

                    TxtCorAddressLine1.Text = string.IsNullOrEmpty(application.CorAddressLine1) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine1) ? GetInitCap(application.CorAddressLine1) : "";
                    TxtCorAddressLine2.Text = string.IsNullOrEmpty(application.CorAddressLine2) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine2) ? GetInitCap(application.CorAddressLine2) : "";
                    TxtCorAddressLine3.Text = string.IsNullOrEmpty(application.CorAddressLine3) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine3) ? GetInitCap(application.CorAddressLine3) : "";
                    TxtCorCity.Text = string.IsNullOrEmpty(application.CorCityName) == false && !string.IsNullOrWhiteSpace(application.CorCityName) ? GetInitCap(application.CorCityName) : "";
                    ddlCorState.SelectedValue = application.CorStateID.ToString();
                    int id2 = Convert.ToInt32(ddlCorState.SelectedValue);
                    ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                    ddldistrict.SelectedValue = application.CorDistrictID.ToString();
                    txtCorPinCode.Text = application.CorPinCode.ToString();

                    DDLeducode.SelectedValue = application.EducationalQualification.ID.ToString();
                    TxtYearOfPassing2.Text = application.PassingYear.ToString();
                    DDLeducode_SelectedIndexChanged(DDLeducode, EventArgs.Empty);
                    // // added_by_amit_audit_april_2026_start image upload code


                    //code for binding showdata apaar block added bby ashutosh start

                    if (application.ApaarRequestID != null && application.ApaarRequestID != 0)
                    {
                        ddlSalutaionName.Enabled = false;
                        txtAppName.Enabled = false;
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

                            // Authentication ID No
                            txtAuthenticationIdNo.Text =
                                !String.IsNullOrEmpty(apaarData.authModeIDNo)
                                ? apaarData.authModeIDNo
                                : "";

                            txtAuthenticationIdNo.Enabled = false;

                            // Consent Date
                            txtConsentDate.Text =
                                apaarData.consentDate.HasValue
                                ? apaarData.consentDate.Value.ToString("dd/MM/yyyy")
                                : "";

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
                            bindDeclaration(application.ApplicantTypeID.ToString(), countAge);

                        }
                    }

                    //code for binding showdata apaar block added bby ashutosh end
                    if (application.Photo != null && application.Photo.Length != 0)
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

                    if (application.Signature != null && application.Signature.Length != 0)
                    {
                        lblsignShow.Visible = true;
                        lblsignShow.Text = "Already Exists... ";
                        lblSign.Text = "Upload Signature / हस्ताक्षर अपलोड ";
                        signPreview.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String((byte[])application.Signature);   // by vishal 05-10-2021
                    }
                    else
                    {
                        lblsignShow.Visible = false;
                        lblsignShow.Text = "";
                        lblSign.Text = "Upload Signature / हस्ताक्षर अपलोड <font color='RED'>*</font>";
                    }

                    if (application.LeftThumb != null && application.LeftThumb.Length != 0)
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

                    // added_by_amit_audit_april_2026_end image upload code
                    btnback.Visible = false;
                }
                ;
                if (RdoUndergngDOEACC.SelectedValue == "I")
                {
                    TrLastCenterAccno.Visible = true;
                    //TrLastCenterInstiName.Visible = true;
                    TrAccCentre.Visible = true;
                    TrExperience.Visible = false;
                    //Added for NSQF Courses
                    Int64 applID = Convert.ToInt64(Request.QueryString["Appid"]);
                    //This Query will get all the information of Applied Canditate by generated Application id                                     
                    EConnectContext context = new EConnectContext();
                    var application = context.CourseRegistrationApplications.Find(applID);
                    if (application.CourseCategoryID.ToString() == "6" && Convert.ToInt32(application.CourseID.ToString()) > 102)
                    {
                        TrExperience.Visible = true;
                        Label63.Text = "Experience in years / वर्षों का अनुभव";
                    }
                    //
                }
                else
                {
                    TrExperience.Visible = true;
                    //Added for NSQF Courses
                    Label63.Text = "If direct, experience in years / यदि  डायरेक्ट,वर्षों का अनुभव";
                    TrLastCenterAccno.Visible = false;
                    //TrLastCenterInstiName.Visible = false;
                    TrAccCentre.Visible = false;
                }
                //added by amit start
                bindEducational();
                // added by amit end
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void UpdateData(Int64 apaarRequestId)
    {
        try
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int64 applID = Convert.ToInt64(Request.QueryString["Appid"]);
                    Course currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["id"]));

                    //This Query will get all the information of Applied Canditate by generated Application id
                    var application = context.CourseRegistrationApplications.Find(applID);


                    //Added for BSB
                    if (application.isBSB.ToString() == "True")
                    {
                        // if(string.IsNullOrWhiteSpace(txtUDISECode.Text))
                        // Added on  31 Dec 2024
                        if (string.IsNullOrEmpty(txtUDISECode.Text))
                        {
                            ShowAlert("Please enter  School Code.");
                            return;
                        }
                        //Added on 31 Dec 2024
                        application.isBSB = true;
                        application.BSB_U_DISECode = txtUDISECode.Text;

                    }
                    else
                    {
                        application.isBSB = false;
                        application.BSB_U_DISECode = null;
                    }
                    ///////////
                    application.CourseCategoryID = currentCourse.CourseCategoryID;
                    //application.apaarID =currentCourse.apaarID ;
                    application.CourseID = currentCourse.ID;
                    application.ApplicationDate = DateTime.Now;
                    application.ApplicableExamID = Convert.ToInt32(ViewState["ExamID"]);
                    if (RdoUndergngDOEACC.SelectedValue == "I")
                    {
                        application.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Institute);
                        application.InstituteID = Convert.ToInt64(DdlAccCentre.SelectedValue);
                        //Added for NSQF Courses
                        if (application.CourseCategoryID.ToString() == "6" && Convert.ToInt32(application.CourseID.ToString()) > 102)
                        {
                            application.ExperienceInYears = !string.IsNullOrEmpty(TxtExperienceInYears.Text) && !string.IsNullOrWhiteSpace(TxtExperienceInYears.Text) ? Convert.ToDecimal(TxtExperienceInYears.Text) : 0;
                        }
                        //Added_UP_Project_30_12_2024_Start Added by Amit start
                        if (radProject.SelectedValue == "1")
                        {
                            if (DdlProject.SelectedValue != "0")
                            {
                                application.projectID = Convert.ToInt32(DdlProject.SelectedValue);
                                //if (trUPField1.Visible)
                                //    application.field1 = txtField1.Text;
                                if (trBSB.Visible)
                                    application.field1 = txtUDISECode.Text.Trim();
                                if (trUPField2.Visible)
                                    application.field2 = txtField2.Text.Trim();
                                if (trUPField3.Visible)
                                    application.field3 = txtField3.Text.Trim();
                                if (trUPField4.Visible)
                                    application.field4 = txtField4.Text.Trim();
                                if (trUPField5.Visible)
                                    application.field5 = txtField5.Text.Trim();
                            }
                            else
                            {
                                application.projectID = null;
                                application.field1 = null;
                                application.field2 = null;
                                application.field3 = null;
                                application.field4 = null;
                                application.field5 = null;
                            }



                        }
                        // amit changed code
                        else
                        {
                            application.projectID = null;
                            application.field1 = null;
                            application.field2 = null;
                            application.field3 = null;
                            application.field4 = null;
                            application.field5 = null;
                        }

                        //Added_UP_Project_30_12_2024_End Added by Amit end
                    }
                    else
                    {
                        application.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Direct);
                        application.ExperienceInYears = !string.IsNullOrEmpty(TxtExperienceInYears.Text) && !string.IsNullOrWhiteSpace(TxtExperienceInYears.Text) ? Convert.ToDecimal(TxtExperienceInYears.Text) : 0;
                        application.InstituteID = null;
                    }
                    //Static as no required in case of fresh
                    application.AlreadyRegistered = false;

                    //Static as no required in case of fresh
                    application.AlreadyQualified = false;
                    application.PaymentSourceID = Convert.ToInt32(ddlPaymentOption.SelectedValue);
                    //Update Candidate Personal Detail...
                    string salutation = ddlSalutaionName.SelectedItem.Text;
                    salutation = salutation.Substring(0, salutation.IndexOf('/'));
                    application.Salutation = salutation.Trim();
                    application.Name = txtAppName.Text.Trim().ToString();
                    if (Rdoownertype.SelectedValue == "P")
                    {
                        application.FatherName = txtFatherName.Text.Trim();
                        application.MotherName = txtMotherName.Text.Trim();
                        application.GuardianName = null;

                        //Added 22 Feb 2019
                        application.affidavitUpload = null;
                        application.affidavitDate = null;
                        application.affidavitNo = null;
                        application.affidavitFile = null;
                        application.GuardianFlagStatusID = 1;
                        //
                    }
                    else if (Rdoownertype.SelectedValue == "G")
                    {
                        application.GuardianName = TxtGuardianName.Text.Trim();
                        //Added 22 feb 2019
                        application.affidavitDate = Convert.ToDateTime(txtAffidavitDate.Text);
                        application.affidavitNo = txtAffidavitNo.Text;
                        if (fileAffidavit.FileName.Length > 50)
                        {
                            application.affidavitFile = fileAffidavit.FileName.Substring(0, 45) + System.IO.Path.GetExtension(fileAffidavit.FileName).ToLower();
                        }
                        else
                            application.affidavitFile = fileAffidavit.FileName;

                        application.affidavitUpload = fileAffidavit.FileBytes;
                        application.affidavitVerified = false;
                        application.GuardianFlagStatusID = 2;
                        //
                        application.FatherName = null;
                        application.MotherName = null;
                    }

                    //string gender = ddl_gender.SelectedItem.Text.ToString();
                    //gender = gender.Substring(0, gender.IndexOf('/'));
                    //string gender = ddl_gender.SelectedItem.Text.ToString();
                    //gender = gender.Substring(0, gender.IndexOf('/'));
                    //application.Gender = gender.Trim();

                    application.Gender = ddl_gender.SelectedValue;//jksah

                    //application.Gender = gender.Trim();
                    application.MaritalStatusID = Convert.ToInt32(ddlMStatus.SelectedValue);
                    application.DateOfBirth = Convert.ToDateTime(txtDob.Text.ToString());
                    application.CastCategoryID = Convert.ToInt32(ddlCategory.SelectedValue);
                    if (Rdhandicapped.SelectedValue == "Y")
                    {
                        application.IsHandicaped = true;
                    }
                    else
                    {
                        application.IsHandicaped = false;
                    }
                    if (Rdexserviceman.SelectedValue == "Y")
                    {
                        application.IsExServicemane = true;
                    }
                    else
                    {
                        application.IsExServicemane = false;
                    }

                    // jk sah on 14-1-2021
                    if (RdisEWS.SelectedValue == "Y")
                    {
                        application.Is_EWS = true;
                    }
                    else
                    {
                        application.Is_EWS = false;
                    }
                    application.ReligionID = Convert.ToInt32(ddlReligion.SelectedValue);
                    application.BodyMark = txtBodyMark.Text;

                    //Commented 15 Apr 2026
                    //// added_by_amit_audit_april_2026_start image upload code start

                    //if (lblphotoShow.Text.ToLower().Contains("success"))
                    //{

                    //    if (ImgUpload.FileName.Length > 50 && Session["ImgUpload"] == null)
                    //    {
                    //        application.PhotoFileName = ImgUpload.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUpload.FileName).ToLower();

                    //    }
                    //    else
                    //        application.PhotoFileName = ImgUpload.FileName;

                    //    application.Photo = ImgUpload.FileBytes;

                    //}

                    //if (lblsignShow.Text.ToLower().Contains("success"))
                    //{
                    //    if (ImgUploadSignature.FileName.Length > 50 && Session["ImgUploadSignature"] == null)
                    //    {
                    //        application.SignatureFileName = ImgUploadSignature.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUploadSignature.FileName).ToLower();

                    //    }
                    //    else
                    //        application.SignatureFileName = ImgUploadSignature.FileName;

                    //    application.Signature = ImgUploadSignature.FileBytes;

                    //}


                    //if (lblthumbshow.Text.ToLower().Contains("success"))
                    //{

                    //    if (ImgUploadThumb.FileName.Length > 50 && Session["ImgUploadThumb"] == null)
                    //    {
                    //        application.LeftThumbFileName = ImgUploadThumb.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUploadThumb.FileName).ToLower();

                    //    }
                    //    else
                    //        application.LeftThumbFileName = ImgUploadThumb.FileName;

                    //    application.LeftThumb = ImgUploadThumb.FileBytes;

                    //}


                    ////  added_by_amit_audit_april_2026_end image upload code end

                    /*
                    if (ImgUpload.FileName.Length > 50)
                    {
                        application.PhotoFileName = ImgUpload.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUpload.FileName).ToLower();
                    }
                    else
                        application.PhotoFileName = ImgUpload.FileName;
                    application.Photo = ImgUpload.FileBytes;
                    if (ImgUploadSignature.FileName.Length > 50)
                    {
                        application.SignatureFileName = ImgUploadSignature.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUploadSignature.FileName).ToLower();
                    }
                    else
                        application.SignatureFileName = ImgUploadSignature.FileName;
                    application.Signature = ImgUploadSignature.FileBytes;
                    if (ImgUploadThumb.FileName.Length > 50)
                    {
                        application.LeftThumbFileName = ImgUploadThumb.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUploadThumb.FileName).ToLower();
                    }
                    else
                        application.LeftThumbFileName = ImgUploadThumb.FileName;
                    application.LeftThumb = ImgUploadThumb.FileBytes;

*/
                    #region Photo
                    if (ImgUpload.HasFile)
                    {
                        if (ImgUpload.FileName.Length > 50)
                        {
                            application.PhotoFileName = ImgUpload.FileName.Substring(0, 45) +
                                System.IO.Path.GetExtension(ImgUpload.FileName).ToLower();
                        }
                        else
                        {
                            //application.PhotoFileName = ImgUpload.FileName; 
                            string ImgNamePhoto = string.Empty;
                            ImgNamePhoto = "P" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUpload.FileName);          //Photo_ 
                            application.PhotoFileName = ImgNamePhoto;
                        }
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
                        if (ImgUploadSignature.FileName.Length > 50)
                        {
                            application.SignatureFileName =
                                ImgUploadSignature.FileName.Substring(0, 45) +
                                System.IO.Path.GetExtension(ImgUploadSignature.FileName).ToLower();
                        }
                        else
                        {
                            //application.SignatureFileName = ImgUploadSignature.FileName; 
                            string ImgNameSig = string.Empty;
                            ImgNameSig = "S" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadSignature.FileName);          //Sig_ 
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
                        if (ImgUploadThumb.FileName.Length > 50)
                        {
                            application.LeftThumbFileName =
                                ImgUploadThumb.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUploadThumb.FileName).ToLower();
                        }
                        else
                        {
                            //application.LeftThumbFileName = ImgUploadThumb.FileName; 
                            string ImgNameLth = string.Empty;
                            ImgNameLth = "L" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadThumb.FileName);         //Lth_ 
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


                    //Update Candidate Contact Detail.
                    if (String.IsNullOrEmpty(TxtSTDcode.Text) == false && string.IsNullOrEmpty(txtCorPhoneNo.Text) == false)
                    {
                        application.StdNumber = Convert.ToInt32(TxtSTDcode.Text);
                        application.PhoneNumber = Convert.ToInt32(txtCorPhoneNo.Text);
                    }
                    application.MobileNumber = Convert.ToInt64(txtCorMobileNo.Text);
                    application.EmailAddress = txtEmailId.Text;


                    //Permanent Address Details
                    application.PerAddressLine1 = TxtPerAddressLine1.Text;
                    application.PerAddressLine2 = TxtPerAddressLine2.Text;
                    application.PerAddressLine3 = TxtPerAddressLine3.Text;
                    application.PerCityName = TxtPerCity.Text;
                    application.PerStateID = Convert.ToInt32(ddlPState.SelectedValue);
                    application.PerDistrictID = Convert.ToInt32(ddlPdistrict.SelectedValue);
                    application.PerPinCode = Convert.ToInt32(TxtPpincode.Text);


                    //Correspondence Address Details
                    if (chkSame.Checked)
                    {
                        application.CorAddressLine1 = TxtPerAddressLine1.Text;
                        application.CorAddressLine2 = TxtPerAddressLine2.Text;
                        application.CorAddressLine3 = TxtPerAddressLine3.Text;
                        application.CorCityName = TxtPerCity.Text;
                        application.CorStateID = Convert.ToInt32(ddlPState.SelectedValue);
                        application.CorDistrictID = Convert.ToInt32(ddlPdistrict.SelectedValue);
                        application.CorPinCode = Convert.ToInt32(TxtPpincode.Text);
                    }
                    else
                    {
                        application.CorAddressLine1 = TxtCorAddressLine1.Text;
                        application.CorAddressLine2 = TxtCorAddressLine2.Text;
                        application.CorAddressLine3 = TxtCorAddressLine3.Text;
                        application.CorCityName = TxtCorCity.Text;
                        application.CorStateID = Convert.ToInt32(ddlCorState.SelectedValue);
                        application.CorDistrictID = Convert.ToInt32(ddldistrict.SelectedValue);
                        application.CorPinCode = Convert.ToInt32(txtCorPinCode.Text);
                    }

                    //Qualification Details
                    application.EducationalQualificationID = Convert.ToInt32(DDLeducode.SelectedValue);
                    application.PassingYear = Convert.ToInt32(TxtYearOfPassing2.Text);

                    //added_by_amit_audit_april_2026_start aadhaar change

                    if (!String.IsNullOrEmpty(application.AadharNumber.ToString()))
                    {
                        string decryptedAadhaar = EncryptDecrypt.DecryptString(hfaadhaar.Value);
                        application.AadharNumber = Convert.ToInt64(decryptedAadhaar);
                    }
                    else
                        application.AadharNumber = null;
                    // added_by_amit_audit_april_2026_end

                    application.AadharVerfied = false;

                    /*if (!String.IsNullOrEmpty(txtapaar.Text))
                            {
                                //added on 12  Sept 2024
                                string apaarEncrypted = EncryptDecrypt.EncryptString(txtapaar.Text);
                                application.apaarID = apaarEncrypted;
                                //added on 12 Sept 2024                    
                            }
                            else
                                application.apaarID = null;*/
                    if (application.apaarID == null || application.apaarID == "")
                    {
                        ShowAlert("Please enter Apaar ID");

                        return;
                    }
                    application.ApaarRequestID = apaarRequestId;

                    context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();

                    // new added for server images deletion 

                    string ImgNamePhoto2 = string.Empty;
                    if (ImgUpload.HasFile)
                    { ImgNamePhoto2 = "P" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUpload.FileName); }          //Photo_ 
                    string fileNamePhoto = Server.MapPath("~/ImageUpload/" + ImgNamePhoto2);
                    if (File.Exists(fileNamePhoto))
                    { File.Delete(fileNamePhoto); }

                    string ImgNameSig2 = string.Empty;
                    if (ImgUploadSignature.HasFile)
                    { ImgNameSig2 = "S" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadSignature.FileName); }         //Sig_ 
                    string fileNameSig = Server.MapPath("~/ImageUpload/" + ImgNameSig2);
                    if (File.Exists(fileNameSig))
                    { File.Delete(fileNameSig); }

                    string ImgNameLth2 = string.Empty;
                    if (ImgUploadThumb.HasFile)
                    { ImgNameLth2 = "L" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadThumb.FileName); }         //Lth_ 
                    string fileNameth = Server.MapPath("~/ImageUpload/" + ImgNameLth2);
                    if (File.Exists(fileNameth))
                    { File.Delete(fileNameth); }

                    // new added for server images deletion 

                    //  added_by_amit_audit_april_2026_end image upload code end 

                    long Appid = application.ID;
                    scope.Complete();
                    lblerror.Text = "Record Updated.";
                    if ((!String.IsNullOrEmpty(Request.QueryString["Appid"])) && (Request.QueryString["Src"] == "CSC"))
                    {
                        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/FrmPreview.aspx?Appid=" + Appid + "&Src=CSC"));
                    }
                    else
                    {
                        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/FrmPreview.aspx?Appid=" + Appid));
                    }
                }
                ;
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void SaveData(Int64 apaarRequestId)
    {
        try
        {
            Boolean success = false;
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    int courseid = Convert.ToInt32(Request.QueryString["id"]);
                    Course currentCourse = context.Courses.Find(courseid);
                    string CourseCatg = currentCourse.CourseCategory.Name.ToString();
                    Int32 ShortermCoursesCategory = context.CourseCategories.Where(s => s.Code.ToUpper() == "STC").FirstOrDefault().ID;
                    Int32 IRDACategory = context.CourseCategories.Where(s => s.Code.ToUpper() == "IRDA").FirstOrDefault().ID;
                    CourseRegistrationApplication objRegistration = new EConnect.NIELIT.CourseRegistrationApplication();
                    //objRegistration.CandidateID = application.ID;
                    if (!isDuplicate())
                    {
                        objRegistration.CourseCategoryID = currentCourse.CourseCategoryID;
                        objRegistration.CourseID = currentCourse.ID;
                        objRegistration.ApplicationDate = DateTime.Now;
                        objRegistration.ApplicableExamID = Convert.ToInt32(ViewState["ExamID"]);
                        if (RdoUndergngDOEACC.SelectedValue == "I")
                        {
                            objRegistration.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Institute);
                            objRegistration.InstituteID = Convert.ToInt64(DdlAccCentre.SelectedValue);
                            //Added for NSQF Courses

                            if (CourseCatg == "6" && Convert.ToInt32(currentCourse.ID.ToString()) > 102)
                            {
                                objRegistration.ExperienceInYears = !string.IsNullOrEmpty(TxtExperienceInYears.Text) && !string.IsNullOrWhiteSpace(TxtExperienceInYears.Text) ? Convert.ToDecimal(TxtExperienceInYears.Text) : 0;
                            }
                            // Added by Amit
                            // ONLY UP code , not work for BSB
                            if (radProject.SelectedValue == "1")
                            {
                                if (DdlProject.SelectedValue != "0")
                                {
                                    objRegistration.projectID = Convert.ToInt32(DdlProject.SelectedValue);
                                    //if (trUPField1.Visible)
                                    //    objRegistration.field1 = txtField1.Text;
                                    if (trBSB.Visible)
                                    {
                                        objRegistration.field1 = txtUDISECode.Text.Trim();
                                    }

                                    if (trUPField2.Visible)
                                        objRegistration.field2 = txtField2.Text.Trim();
                                    if (trUPField3.Visible)
                                        objRegistration.field3 = txtField3.Text.Trim();
                                    if (trUPField4.Visible)
                                        objRegistration.field4 = txtField4.Text.Trim();
                                    if (trUPField5.Visible)
                                        objRegistration.field5 = txtField5.Text.Trim();
                                }
                            }

                            //new code for bsb flow
                            //  if (trBSB.Visible == true && (checkBSB() || Session["isBSB"].ToString() == "1"))
                            //  {
                            //      objRegistration.field1 = txtUDISECode.Text.Trim();
                            //ddlUDISECode.SelectedValue.Trim();
                            // }
                            // Added by Amit end
                        }
                        else
                        {
                            objRegistration.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Direct);
                            objRegistration.ExperienceInYears = (!string.IsNullOrEmpty(TxtExperienceInYears.Text) && !string.IsNullOrWhiteSpace(TxtExperienceInYears.Text)) ? Convert.ToDecimal(TxtExperienceInYears.Text) : 0;
                            objRegistration.InstituteID = null;
                        }
                        objRegistration.AlreadyRegistered = false;
                        objRegistration.AlreadyQualified = false;
                        objRegistration.PaymentSourceID = Convert.ToInt32(ddlPaymentOption.SelectedValue);
                        //Saving Personal Details
                        string salutation = ddlSalutaionName.SelectedItem.Text;
                        salutation = salutation.Substring(0, salutation.IndexOf('/'));

                        objRegistration.Salutation = salutation.Trim();
                        objRegistration.Name = txtAppName.Text.Trim().ToString();
                        if (Rdoownertype.SelectedValue == "P")
                        {
                            objRegistration.FatherName = txtFatherName.Text.Trim();
                            objRegistration.MotherName = txtMotherName.Text.Trim();
                            objRegistration.GuardianName = null;
                            //Added 22 Feb 2019
                            objRegistration.affidavitUpload = null;
                            objRegistration.affidavitDate = null;
                            objRegistration.affidavitNo = null;
                            objRegistration.affidavitFile = null;
                            objRegistration.GuardianFlagStatusID = 1;
                            //
                        }
                        else if (Rdoownertype.SelectedValue == "G")
                        {
                            objRegistration.GuardianName = TxtGuardianName.Text.Trim();
                            objRegistration.FatherName = null;
                            objRegistration.MotherName = null;
                            //Added 22 feb 2019
                            objRegistration.affidavitDate = Convert.ToDateTime(txtAffidavitDate.Text);
                            objRegistration.affidavitNo = txtAffidavitNo.Text;
                            if (fileAffidavit.FileName.Length > 50)
                            {
                                objRegistration.affidavitFile = fileAffidavit.FileName.Substring(0, 45) + System.IO.Path.GetExtension(fileAffidavit.FileName).ToLower();
                            }
                            else
                                objRegistration.affidavitFile = fileAffidavit.FileName;

                            objRegistration.affidavitUpload = fileAffidavit.FileBytes;
                            objRegistration.affidavitVerified = false;

                            objRegistration.affidavitUploadedOn = DateTime.Now;
                            objRegistration.affidavitUploadedBy = -99;

                            objRegistration.GuardianFlagStatusID = 2;
                            //Convert.ToInt32(Session["UserID"]);
                            //
                        }
                        //Added for BSB
                        if (Session["isBSB"] != null)
                        {
                            if (Session["isBSB"].ToString() == "1")
                            {
                                //Added on  31 Dec  2024

                                if (string.IsNullOrEmpty(txtUDISECode.Text))
                                {
                                    ShowAlert("Please enter School Code.");
                                    return;
                                }

                                // Added on  31 Dec  2024
                                objRegistration.isBSB = true;
                                objRegistration.BSB_U_DISECode = txtUDISECode.Text;
                            }
                            else
                            {
                                objRegistration.isBSB = false;
                                objRegistration.BSB_U_DISECode = null;
                            }

                        }
                        else
                        {
                            objRegistration.isBSB = false;
                            objRegistration.BSB_U_DISECode = null;
                        }
                        ////////////////

                        ////string gender = ddl_gender.SelectedItem.Text.ToString();
                        ////gender = gender.Substring(0, gender.IndexOf('/'));
                        ////objRegistration.Gender = gender.Trim();

                        //The above 3 lines of code is OK for Male & Female but the string variable gender will pick the value Transgender (instead of Trans which is required as per the genderCode in the DB)

                        objRegistration.Gender = ddl_gender.SelectedValue;//jksah
                        objRegistration.MaritalStatusID = Convert.ToInt32(ddlMStatus.SelectedValue);
                        objRegistration.DateOfBirth = Convert.ToDateTime(txtDob.Text.ToString());
                        objRegistration.CastCategoryID = Convert.ToInt32(ddlCategory.SelectedValue);
                        if (Rdhandicapped.SelectedValue == "Y")
                        {
                            objRegistration.IsHandicaped = true;
                        }
                        else
                        {
                            objRegistration.IsHandicaped = false;
                        }
                        if (Rdexserviceman.SelectedValue == "Y")
                        {
                            objRegistration.IsExServicemane = true;
                        }
                        else
                        {
                            objRegistration.IsExServicemane = false;
                        }
                        // added by amit start
                        if (RdisEWS.SelectedValue == "Y")
                        {
                            objRegistration.Is_EWS = true;
                        }
                        else
                        {
                            objRegistration.Is_EWS = false;
                        }
                        // added by amit end
                        objRegistration.ReligionID = Convert.ToInt32(ddlReligion.SelectedValue);
                        objRegistration.BodyMark = txtBodyMark.Text;
                        // Commented 15 APr 2026
                        //if (ImgUpload.FileName.Length > (50 * 1024))
                        //{
                        //    objRegistration.PhotoFileName = ImgUpload.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUpload.FileName).ToLower();
                        //}
                        //else
                        //    objRegistration.PhotoFileName = ImgUpload.FileName;
                        //objRegistration.Photo = ImgUpload.FileBytes;

                        //if (ImgUploadSignature.FileName.Length > (50 * 1024))
                        //{
                        //    objRegistration.SignatureFileName = ImgUploadSignature.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUploadSignature.FileName).ToLower();
                        //}
                        //else
                        //    objRegistration.SignatureFileName = ImgUploadSignature.FileName;
                        //objRegistration.Signature = ImgUploadSignature.FileBytes;

                        //if (ImgUploadThumb.FileName.Length > (50 * 1024))
                        //{
                        //    objRegistration.LeftThumbFileName = ImgUploadThumb.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUploadThumb.FileName).ToLower();
                        //}
                        //else
                        //    objRegistration.LeftThumbFileName = ImgUploadThumb.FileName;
                        //objRegistration.LeftThumb = ImgUploadThumb.FileBytes;
                        // added_by_amit_audit_april_2026_start 
                        // Photo Upload 
                        #region Photo
                        if (ImgUpload.HasFile)
                        {
                            if (ImgUpload.FileName.Length > 50) //(50*1024) 
                            {
                                objRegistration.PhotoFileName = ImgUpload.FileName.Substring(0, 45) +
                                    System.IO.Path.GetExtension(ImgUpload.FileName).ToLower();
                            }
                            else
                            {
                                //application.PhotoFileName = ImgUpload.FileName; 
                                string ImgNamePhoto = string.Empty;
                                ImgNamePhoto = "P" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUpload.FileName);          //Photo_ 
                                objRegistration.PhotoFileName = ImgNamePhoto;
                            }
                            // 
                            objRegistration.Photo = ImgUpload.FileBytes;
                        }
                        else
                        {
                            objRegistration.Photo = objRegistration.Photo;
                            objRegistration.PhotoFileName = objRegistration.PhotoFileName;
                        }
                        #endregion
                        // Signature Upload 
                        #region Signature
                        if (ImgUploadSignature.HasFile)
                        {
                            if (ImgUploadSignature.FileName.Length > 50) //(50 * 1024) 
                            {
                                objRegistration.SignatureFileName = ImgUploadSignature.FileName.Substring(0, 45) +
                                    System.IO.Path.GetExtension(ImgUploadSignature.FileName).ToLower();
                            }
                            else
                            {
                                //application.SignatureFileName = ImgUploadSignature.FileName; 
                                string ImgNameSig = string.Empty;
                                ImgNameSig = "S" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadSignature.FileName);          //Sig_ 
                                objRegistration.SignatureFileName = ImgNameSig;
                            }
                            objRegistration.Signature = ImgUploadSignature.FileBytes;
                        }
                        else
                        {
                            objRegistration.Signature = objRegistration.Signature;
                            objRegistration.SignatureFileName = objRegistration.SignatureFileName;
                        }
                        #endregion


                        // Thumb Upload 
                        #region Thumb
                        if (ImgUploadThumb.HasFile)
                        {
                            if (ImgUploadThumb.FileName.Length > 50) //(50 * 1024) 
                            {
                                objRegistration.LeftThumbFileName = ImgUploadThumb.FileName.Substring(0, 45) +
                                    System.IO.Path.GetExtension(ImgUploadThumb.FileName).ToLower();
                            }
                            else
                            {
                                //application.LeftThumbFileName = ImgUploadThumb.FileName; 
                                string ImgNameLth = string.Empty;
                                ImgNameLth = "L" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadThumb.FileName);         //Lth_ 
                                objRegistration.LeftThumbFileName = ImgNameLth;
                            }
                            objRegistration.LeftThumb = ImgUploadThumb.FileBytes;
                        }
                        else
                        {
                            objRegistration.LeftThumb = objRegistration.LeftThumb;
                            objRegistration.LeftThumbFileName = objRegistration.LeftThumbFileName;
                        }
                        #endregion 

                        if (objRegistration.Signature == null || objRegistration.Signature.Length == 0)
                        {
                            ShowAlert("Please reupload Signature");
                            return;
                        }
                        if (objRegistration.Photo == null || objRegistration.Photo.Length == 0)
                        {
                            ShowAlert("Please reupload photograph");
                            return;
                        }
                        if (objRegistration.LeftThumb == null || objRegistration.LeftThumb.Length == 0)
                        {
                            ShowAlert("Please reupload Left Thumb impression");
                            return;
                        }
                        //Contact Details
                        if (String.IsNullOrEmpty(TxtSTDcode.Text) == false && string.IsNullOrEmpty(txtCorPhoneNo.Text) == false)
                        {
                            objRegistration.StdNumber = Convert.ToInt32(TxtSTDcode.Text);
                            objRegistration.PhoneNumber = Convert.ToInt32(txtCorPhoneNo.Text);
                        }
                        objRegistration.MobileNumber = Convert.ToInt64(txtCorMobileNo.Text);
                        objRegistration.EmailAddress = txtEmailId.Text;


                        //Permanent Address Details
                        objRegistration.PerAddressLine1 = TxtPerAddressLine1.Text;
                        objRegistration.PerAddressLine2 = TxtPerAddressLine2.Text;
                        objRegistration.PerAddressLine3 = TxtPerAddressLine3.Text;
                        objRegistration.PerCityName = TxtPerCity.Text;
                        objRegistration.PerStateID = Convert.ToInt32(ddlPState.SelectedValue);
                        objRegistration.PerDistrictID = Convert.ToInt32(ddlPdistrict.SelectedValue);
                        objRegistration.PerPinCode = Convert.ToInt32(TxtPpincode.Text);


                        //Correspondence Address Details
                        if (chkSame.Checked)
                        {
                            objRegistration.CorAddressLine1 = TxtPerAddressLine1.Text;
                            objRegistration.CorAddressLine2 = TxtPerAddressLine2.Text;
                            objRegistration.CorAddressLine3 = TxtPerAddressLine3.Text;
                            objRegistration.CorCityName = TxtPerCity.Text;
                            objRegistration.CorStateID = Convert.ToInt32(ddlPState.SelectedValue);
                            objRegistration.CorDistrictID = Convert.ToInt32(ddlPdistrict.SelectedValue);
                            objRegistration.CorPinCode = Convert.ToInt32(TxtPpincode.Text);
                        }
                        else
                        {
                            objRegistration.CorAddressLine1 = Server.HtmlEncode(TxtCorAddressLine1.Text);
                            objRegistration.CorAddressLine2 = TxtCorAddressLine2.Text;
                            objRegistration.CorAddressLine3 = TxtCorAddressLine3.Text;
                            objRegistration.CorCityName = TxtCorCity.Text;
                            objRegistration.CorStateID = Convert.ToInt32(ddlCorState.SelectedValue);
                            objRegistration.CorDistrictID = Convert.ToInt32(ddldistrict.SelectedValue);
                            objRegistration.CorPinCode = Convert.ToInt32(txtCorPinCode.Text);
                        }

                        //Qualification Details
                        objRegistration.EducationalQualificationID = Convert.ToInt32(DDLeducode.SelectedValue);
                        objRegistration.PassingYear = Convert.ToInt32(TxtYearOfPassing2.Text);
                        objRegistration.IsVerifiedByInstitute = false;
                        DateTime Dob = Convert.ToDateTime(txtDob.Text);
                        Int32 ReligionId = Convert.ToInt32(ddlReligion.SelectedValue);
                        Int32 castCategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                        Boolean IsHandicapped = Rdhandicapped.SelectedValue == "Y" ? true : false;

                        //aadhar details
                        // added_by_amit_audit_april_2026_start aadhaar changed 
                        if (!String.IsNullOrEmpty(hfaadhaar.Value.ToString()))
                        {
                            string decryptAadhar = EncryptDecrypt.DecryptString(hfaadhaar.Value.ToString());
                            objRegistration.AadharNumber = Convert.ToInt64(decryptAadhar);
                        }
                        else
                            objRegistration.AadharNumber = null;
                        // added_by_amit_audit_april_2026_end aadhaar changed
                        if (CourseCatg == "IRDA") //----IRDA
                        {
                            objRegistration.UIDType = Convert.ToInt32(UidTypeDdl.SelectedValue);
                            objRegistration.UIDNumber = UidNumberTxt.Text.Trim().ToUpper();
                        }
                        else
                        {
                            objRegistration.UIDType = null;
                            objRegistration.UIDNumber = null;
                        }
                        objRegistration.AadharVerfied = false;

                        if (!String.IsNullOrEmpty(txtapaar.Text))
                        {
                            //added on 12 Sept 2024
                            string apaarEncrypted = EncryptDecrypt.EncryptString(txtapaar.Text);
                            objRegistration.apaarID = apaarEncrypted;
                            objRegistration.ApaarRequestID = apaarRequestId;

                            //added on 12 Sept 2024
                        }
                        else
                        {
                            ShowAlert("Enter Apaar");
                            return;
                        }
                        // objRegistration.apaarID = null;
                        if (objRegistration.apaarID == null || objRegistration.apaarID == "")
                        {
                            ShowAlert("Please enter Apaar ID");
                            return;
                        }


                        objRegistration.RegistrationTypeID = Convert.ToInt32(enmRegistrationType.New);
                        if (Request.QueryString["Src"] != null)
                        {
                            if (Request.QueryString["Src"] == "CSC")
                                objRegistration.ApplicationSourceID = Convert.ToInt32(enmApplicationSource.CSC);
                            else
                                objRegistration.ApplicationSourceID = Convert.ToInt32(enmApplicationSource.Candidate);
                        }
                        else
                            objRegistration.ApplicationSourceID = Convert.ToInt32(enmApplicationSource.Candidate);
                        //Added Onlinerefid
                        objRegistration.OnlineRefID = txtMIS.Text;
                        context.CourseRegistrationApplications.Add(objRegistration);
                        context.SaveChanges();

                        // new added for server images deletion 

                        string ImgNamePhoto2 = string.Empty;
                        if (ImgUpload.HasFile)
                        { ImgNamePhoto2 = "P" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUpload.FileName); }          //Photo_ 
                        string fileNamePhoto = Server.MapPath("~/ImageUpload/" + ImgNamePhoto2);
                        if (File.Exists(fileNamePhoto))
                        { File.Delete(fileNamePhoto); }

                        string ImgNameSig2 = string.Empty;
                        if (ImgUploadSignature.HasFile)
                        { ImgNameSig2 = "S" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadSignature.FileName); }         //Sig_ 
                        string fileNameSig = Server.MapPath("~/ImageUpload/" + ImgNameSig2);
                        if (File.Exists(fileNameSig))
                        { File.Delete(fileNameSig); }

                        string ImgNameLth2 = string.Empty;
                        if (ImgUploadThumb.HasFile)
                        { ImgNameLth2 = "L" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadThumb.FileName); }         //Lth_ 
                        string fileNameth = Server.MapPath("~/ImageUpload/" + ImgNameLth2);
                        if (File.Exists(fileNameth))
                        { File.Delete(fileNameth); }
                        // added_by_amit_audit_april_2026_end 

                        long Appid = objRegistration.ID;
                        success = true;
                        if (success == true)
                            scope.Complete();
                        lblerror.Text = "New record saved.";
                        if ((!String.IsNullOrEmpty(Request.QueryString["id"])) && (Request.QueryString["Src"] == "CSC"))
                        {
                            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/FrmPreview.aspx?Appid=" + Appid + "&Src=CSC"));
                        }
                        else
                        {
                            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../CAND/FrmPreview.aspx?Appid=" + Appid));
                        }
                    }

                    else
                    {
                        btnSave.Visible = false;
                        DateTime dob = Convert.ToDateTime(txtDob.Text);
                        Int64 candidateID = 0;
                        if (currentCourse.CourseCategoryID == ShortermCoursesCategory)
                        {
                            if (Rdoownertype.SelectedValue == "G")
                            {
                                candidateID = (from c in context.Candidates
                                               join r in context.RegistrationDetails
                                                   on c.ID equals r.CandidateID
                                               where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                               && c.GuardianName.Equals(TxtGuardianName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                               && c.Gender == ddl_gender.SelectedValue
                                               && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                               && r.CourseCategoryID == currentCourse.CourseCategoryID && r.CourseID == currentCourse.ID
                                               select c.ID).FirstOrDefault();
                            }
                            else if (Rdoownertype.SelectedValue == "P")
                            {
                                candidateID = (from c in context.Candidates
                                               join r in context.RegistrationDetails
                                                   on c.ID equals r.CandidateID
                                               where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                               && c.FatherName.Equals(txtFatherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                               && c.Gender == ddl_gender.SelectedValue
                                               && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                               && r.CourseCategoryID == currentCourse.CourseCategoryID
                                               && r.CourseID == currentCourse.ID
                                               select c.ID).FirstOrDefault();
                            }
                            if (candidateID != null)
                            {
                                var registration = (from c in context.RegistrationDetails
                                                    where c.CandidateID == candidateID
                                                    orderby c.ValidUptoDate descending
                                                    select new
                                                    {
                                                        regno = c.RegistrationNo,
                                                        coursename = c.Course.Name,
                                                    }).FirstOrDefault();
                                if (registration != null)
                                {
                                    throw new Exception("You have already registered for the :" + CommonFunctions.GetInitCap(registration.coursename) + " course  and your registration no is :" + registration.regno);
                                }
                            }
                        }
                        else if (currentCourse.CourseCategoryID == IRDACategory)
                        {
                            string AppNumber = "";
                            Int32 ExamId = Convert.ToInt32(ViewState["ExamID"]);
                            if (Rdoownertype.SelectedValue == "G")
                            {
                                AppNumber = (from c in context.CourseRegistrationApplications
                                             where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                              && c.GuardianName.Equals(TxtGuardianName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                              && c.ApplicableExamID == ExamId
                                              && c.Gender == ddl_gender.SelectedValue
                                              && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                              && c.CourseCategoryID == currentCourse.CourseCategoryID
                                              && c.CourseID == currentCourse.ID
                                             select c.Number).FirstOrDefault();
                            }
                            else
                            {
                                AppNumber = (from c in context.CourseRegistrationApplications
                                             where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                              && c.FatherName.Equals(txtFatherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                              && c.MotherName.Equals(txtMotherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                              && c.ApplicableExamID == ExamId
                                              && c.Gender == ddl_gender.SelectedValue
                                              && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                              && c.CourseCategoryID == currentCourse.CourseCategoryID
                                              && c.CourseID == currentCourse.ID
                                             select c.Number).FirstOrDefault();
                            }
                            if (AppNumber != null)
                            { throw new Exception("You have already applied for the :" + CommonFunctions.GetInitCap(currentCourse.Name) + " course and your Application Number is :" + AppNumber); }

                        }
                        else
                        {
                            if (Rdoownertype.SelectedValue == "G")
                            {
                                candidateID = (from c in context.Candidates
                                               join r in context.RegistrationDetails
                                                 on c.ID equals r.CandidateID
                                               where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                                && c.GuardianName.Equals(TxtGuardianName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                                && c.Gender == ddl_gender.SelectedValue
                                                && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                                && r.CourseCategoryID == currentCourse.CourseCategoryID
                                               select c.ID).FirstOrDefault();
                            }
                            else if (Rdoownertype.SelectedValue == "P")
                            {
                                candidateID = (from c in context.Candidates
                                               join r in context.RegistrationDetails
                                                 on c.ID equals r.CandidateID
                                               where c.Name == txtAppName.Text.Trim().ToUpper()
                                                && c.FatherName.Equals(txtFatherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                                && c.Gender == ddl_gender.SelectedValue
                                                && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                                && r.CourseCategoryID == currentCourse.CourseCategoryID
                                               select c.ID).FirstOrDefault();
                            }
                            if (candidateID != null)
                            {
                                var registration = (from c in context.RegistrationDetails
                                                    where c.CandidateID == candidateID
                                                    orderby c.ValidUptoDate descending
                                                    select new
                                                    {
                                                        regno = c.RegistrationNo,
                                                        coursename = c.Course.Name,
                                                    }).FirstOrDefault();
                                if (registration != null)
                                {
                                    throw new Exception("You have already registered for the :" + CommonFunctions.GetInitCap(registration.coursename) + " course  and your registration no is :" + registration.regno);
                                }
                            }
                        }
                    }
                }
                ;
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void GenerateNewCaptchaImage()
    {
        try
        {
            //ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateRandomNumber(6);    //commented by ashutosh comment before live //
            ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateCaptchaCode(6);
            EConnect.CaptchaImage captcha = new CaptchaImage(ViewState["CaptchCode"].ToString(), 200, 50, "Century Schoolbook");
            imgcap.Src = captcha.ImageSource;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
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
    protected void disableAll()
    {
        try
        {
            TrLastCenterAccno.Visible = false;
            //TrLastCenterInstiName.Visible = false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    // Added by amit start
    protected bool WhetherAccInstitutesInProject()
    {
        try
        {
            Int64 selectedInstituteId = Convert.ToInt64(DdlAccCentre.SelectedValue);
            int projId = Convert.ToInt32(DdlProject.SelectedValue);

            using (NIELITMISContext context = new NIELITMISContext())
            {

                bool existsInCentre = false;
                // Check in AffInstitute + projectSubCentre
                existsInCentre = (from aff in context.AffInstitutes
                                  join sub in context.projectSubCentres on aff.ID equals sub.centreID
                                  where aff.instituteID == selectedInstituteId
                                        && sub.projectID == projId
                                  select aff).Any();

                if (existsInCentre == true) return existsInCentre;

                // Check in NielitCentres + projectMainCentre
                existsInCentre = (from nc in context.NielitCentres
                                  join main in context.projectMainCentres on nc.instituteID equals main.centreID
                                  where nc.instituteID == selectedInstituteId
                                        && main.projectID == projId
                                  select nc).Any();

                return existsInCentre;
            }
        }
        catch (Exception ex)
        {

            return false;
        }
    }
    // Added by amit end
    protected bool isSelected(DropDownList Dropdown)
    {
        try
        {
            if (Dropdown.SelectedValue == "0")
            {
                Dropdown.Focus();
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isBlank(TextBox txtBox)
    {
        try
        {
            if (txtBox.Text.Trim() == "")
            {
                txtBox.Focus();
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isBlankNumber(TextBox txtBox)
    {
        try
        {
            int zero = 0;

            if (txtBox.Text.Trim() == "" || txtBox.Text.Trim() == zero.ToString() || txtBox.Text.Trim() == ".")
            {
                txtBox.Text = "";
                txtBox.Focus();
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected bool isNumber(TextBox txtBox)
    {
        try
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            if (txtBox.Text.Trim() != "")
            {
                if (!regex.IsMatch(txtBox.Text.Trim()))
                {
                    txtBox.Text = "";
                    txtBox.Focus();
                    return false;
                }
                else
                    return true;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isValidPassingYear(TextBox txtPassingYear, TextBox txtDob)
    {
        try
        {
            Int64 PassingYear = Convert.ToInt64(txtPassingYear.Text);
            string Dobdate = txtDob.Text;
            string[] dobYear = Dobdate.Split('-');


            EConnectContext context = new EConnectContext();
            var allowFutureYears = (from c in context.EducationalQualifications
                                    where c.ID.ToString() == DDLeducode.SelectedValue.ToString()
                                    select new { allowed = c.allowFutureYear.ToString() }).FirstOrDefault();
            if (allowFutureYears.allowed == "True")
            {
                if (DateTime.Now.Year > PassingYear)
                {
                    return false;
                }
                else
                    return true;
            }


            if (DateTime.Now.Year < PassingYear || PassingYear <= (Convert.ToInt64(dobYear[2]) + 10))
            {
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected bool isValidDob(TextBox txtBox)
    {
        try
        {
            DateTime todaydate = DateTime.Now;
            DateTime Inputdate = Convert.ToDateTime(txtBox.Text);

            int result1 = DateTime.Compare(todaydate, Inputdate);
            int result2 = DateTime.Compare(todaydate.AddYears(-10), Inputdate);

            if (result2 == -1)
            {
                lblerror.Text = "Invalid date of birth";
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void bindCourse()
    {
        try
        {
            int CourseId = Convert.ToInt32(Convert.ToString(Request.QueryString["id"]));
            int courseCategoryId = 0;
            using (var context = new EConnectContext())
            {
                courseCategoryId = context.Courses.Find(CourseId).CourseCategoryID;
            }
            if (courseCategoryId == 8)
            {
                UIDtr.Visible = true;
                Aadhaartr.Visible = false;
            }
            else
            {
                UIDtr.Visible = false;
                Aadhaartr.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void bindMaritalStatus()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var maritalStatus = from p in context.MaritalStatus
                                    orderby (p.DisplayOrder)
                                    select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlMStatus, maritalStatus, lst);
            }
            ;

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void bindCastCategory()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var castcategory = from p in context.CastCategories
                                   orderby (p.DisplayOrder)
                                   select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCategory, castcategory, lst);
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void bindReligion()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var religion = from p in context.Religions
                               orderby (p.DisplayOrder)
                               select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlReligion, religion, lst);
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    /*  protected void bindEducational()
      {
          try
          {
              Int32 ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
              int courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
              using (var context = new EConnectContext())
              {
                  List<Int32> qualificationLevels = context.QualificationEligibility.Where(s => s.CourseID == courseID && s.ApplicantTypeID == ApplicantTypeId && s.EffectiveDateFrom <= DateTime.Now && (s.EffectiveDateTo >=DateTime .Now || s.EffectiveDateTo ==null)).OrderByDescending(c => c.EffectiveDateFrom).Select(c => c.QualificationLevelID).ToList();
                  ListItem lst = new ListItem("--Select One--", "0");

                  var education = from p in context.EducationalQualifications 
                                  join q in context.QualificationEligibility on p.QualificationLevelID equals q.QualificationLevelID 
                                  where q.CourseID == courseID 
                                  && q.ApplicantTypeID == ApplicantTypeId
                  //Added for qualification 20 Jna 2023
                                  && q.EffectiveDateFrom <= System.DateTime .Now 
                                  && (q.EffectiveDateTo >=System .DateTime .Now || q.EffectiveDateTo ==null)
                                  select new{ ValueField = p.ID, TextField = p.Name };
                      EConnect.Utils.Common.ControlUtility.BindListObject(DDLeducode, education, lst);              
              };
          }
          catch (Exception ex)
          {
              throw ex;
          }

      }*/
    /// <summary>
    /// /Changed for BSB
    /// </summary>
    protected void bindEducational()
    {
        try
        {
            Int32 ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
            int courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
            using (var context = new EConnectContext())
            {
                List<Int32> qualificationLevels = context.QualificationEligibility.Where(s => s.CourseID == courseID && s.ApplicantTypeID == ApplicantTypeId && s.EffectiveDateFrom <= DateTime.Now && (s.EffectiveDateTo >= DateTime.Now || s.EffectiveDateTo == null)).OrderByDescending(c => c.EffectiveDateFrom).Select(c => c.QualificationLevelID).ToList();
                ListItem lst = new ListItem("--Select One--", "0");
                //Added for BSB
                if (Session["isBSB"] != null || DdlAccCentre.SelectedValue == "10009177")
                {
                    if (Session["isBSB"].ToString() == "1" || DdlProject.SelectedItem.Text.IndexOf("School", StringComparison.OrdinalIgnoreCase) >= 0 || DdlAccCentre.SelectedValue == "10009177")
                    {
                        var education = (from p in context.EducationalQualifications
                                         join q in context.QualificationEligibility on p.QualificationLevelID equals q.QualificationLevelID
                                         where q.CourseID == courseID
                                         && q.ApplicantTypeID == ApplicantTypeId
                                         //Added for qualification 20 Jna 2023
                                         && q.EffectiveDateFrom <= System.DateTime.Now
                                         && (q.EffectiveDateTo >= System.DateTime.Now || q.EffectiveDateTo == null)
                                         select new { ValueField = p.ID, TextField = p.Name })
                                        .Union
                                    (from p in context.EducationalQualifications
                                     where p.ID == 300 || p.ID == 301 || p.ID == 302 || p.ID == 303
                                     select new { ValueField = p.ID, TextField = p.Name })

                                        ;


                        EConnect.Utils.Common.ControlUtility.BindListObject(DDLeducode, education, lst);
                    }
                    else
                    {
                        if (checkBSB() || DdlProject.SelectedItem.Text.IndexOf("School", StringComparison.OrdinalIgnoreCase) >= 0 || DdlAccCentre.SelectedValue == "10009177")
                        {
                            var education = (from p in context.EducationalQualifications
                                             join q in context.QualificationEligibility on p.QualificationLevelID equals q.QualificationLevelID
                                             where q.CourseID == courseID
                                             && q.ApplicantTypeID == ApplicantTypeId
                                             //Added for qualification 20 Jna 2023
                                             && q.EffectiveDateFrom <= System.DateTime.Now
                                             && (q.EffectiveDateTo >= System.DateTime.Now || q.EffectiveDateTo == null)
                                             select new { ValueField = p.ID, TextField = p.Name })
                                         .Union
                                     (from p in context.EducationalQualifications
                                      where p.ID == 300 || p.ID == 301 || p.ID == 302 || p.ID == 303
                                      select new { ValueField = p.ID, TextField = p.Name })

                                         ;


                            EConnect.Utils.Common.ControlUtility.BindListObject(DDLeducode, education, lst);
                        }


                        else
                        {
                            var education = from p in context.EducationalQualifications
                                            join q in context.QualificationEligibility on p.QualificationLevelID equals q.QualificationLevelID
                                            where q.CourseID == courseID
                                            && q.ApplicantTypeID == ApplicantTypeId
                                            //Added for qualification 20 Jna 2023
                                            && q.EffectiveDateFrom <= System.DateTime.Now
                                            && (q.EffectiveDateTo >= System.DateTime.Now || q.EffectiveDateTo == null)
                                            select new { ValueField = p.ID, TextField = p.Name };


                            EConnect.Utils.Common.ControlUtility.BindListObject(DDLeducode, education, lst);
                        }
                    }
                }
                else
                {
                    if (checkBSB() || DdlProject.SelectedItem.Text.IndexOf("School", StringComparison.OrdinalIgnoreCase) >= 0 || DdlAccCentre.SelectedValue == "10009177")
                    {
                        var education = (from p in context.EducationalQualifications
                                         join q in context.QualificationEligibility on p.QualificationLevelID equals q.QualificationLevelID
                                         where q.CourseID == courseID
                                         && q.ApplicantTypeID == ApplicantTypeId
                                         //Added for qualification 20 Jna 2023
                                         && q.EffectiveDateFrom <= System.DateTime.Now
                                         && (q.EffectiveDateTo >= System.DateTime.Now || q.EffectiveDateTo == null)
                                         select new { ValueField = p.ID, TextField = p.Name })
                                     .Union
                                 (from p in context.EducationalQualifications
                                  where p.ID == 300 || p.ID == 301 || p.ID == 302 || p.ID == 303
                                  select new { ValueField = p.ID, TextField = p.Name })

                                     ;


                        EConnect.Utils.Common.ControlUtility.BindListObject(DDLeducode, education, lst);
                    }


                    else
                    {
                        var education = from p in context.EducationalQualifications
                                        join q in context.QualificationEligibility on p.QualificationLevelID equals q.QualificationLevelID
                                        where q.CourseID == courseID
                                        && q.ApplicantTypeID == ApplicantTypeId
                                        //Added for qualification 20 Jna 2023
                                        && q.EffectiveDateFrom <= System.DateTime.Now
                                        && (q.EffectiveDateTo >= System.DateTime.Now || q.EffectiveDateTo == null)
                                        select new { ValueField = p.ID, TextField = p.Name };


                        EConnect.Utils.Common.ControlUtility.BindListObject(DDLeducode, education, lst);
                    }
                }
                ;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindState()
    {
        Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
        Int32 withdrawlid = Convert.ToInt32(enmAccreditationStatus.Withdrawal);
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                ListItem lst1 = new ListItem("--Select One--", "0");
                ListItem lst2 = new ListItem("--Select One--", "0");

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
                                && a.CourseID == courseID
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
    protected void BindAccCentre(int stateid, int courseID)
    {
        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
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
                                 && d.CourseID == courseID
                                 && d.AccreditationStatusID != withdrawlid
                 && d.AccreditationStatusID != rejectedid
                                     && d.AccreditationStatusID != deferredid
                                       && d.AccreditationStatusID != acknowledgeid
                                 //Added 22 May 2020 for instt blocking
                                 && d.tempBlocked == false
                                 && (d.BlockedFromDate >= System.DateTime.Now || d.BlockedFromDate == null)
                                 //
                                 select new { ValueField = i.ID, TextField = d.AccreditationNumber + " - " + i.Name + ", " + (!string.IsNullOrEmpty(i.CityName) ? i.CityName : "") }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlAccCentre, AccCentre.Distinct(), lst);
            }
            ;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindGender()
    {
        try
        {
            using (EConnectContext vContext = new EConnectContext())
            {
                var Gender = from s in
                                 vContext.tblGender
                             select new { ValueField = s.genderCode, TextField = s.name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddl_gender, Gender, new ListItem("--Select Gender--", "0"));

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    //added by ashutosh for apaar validation start

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
    //protected void ddlAuthMode_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    // If SELF is selected then candidate Aadhaar auto fill otherwise id of whichever mode selected
    //    if (ddlAuthMode.SelectedValue == "SELF")
    //    {
    //        txtAuthenticationIdNo.Text = txtaadhar.Text; // Candidate Aadhaar textbox
    //    }
    //    else
    //    {
    //        txtAuthenticationIdNo.Text = "";
    //    }

    //    SetConsentRelation();
    //}

    //private void SetConsentRelation()
    //{
    //    DateTime dob;

    //    if (DateTime.TryParse(txtDob.Text, out dob)) //Pick from Candidate entered DOB above 
    //    {
    //        int age = DateTime.Now.Year - dob.Year;

    //        if (dob > DateTime.Now.AddYears(-age))
    //        {
    //            age--;
    //        }

    //        if (age >= 18)
    //        {
    //            txtConsentRelation.Text = "Self";
    //        }
    //        else
    //        {
    //            if (Rdoownertype.SelectedValue == "P")
    //            {
    //                txtConsentRelation.Text = "Parents";
    //            }
    //            else if (Rdoownertype.SelectedValue == "G")
    //            {
    //                txtConsentRelation.Text = "Guardian";
    //            }
    //            else
    //            {
    //                txtConsentRelation.Text = "";
    //            }
    //        }
    //    }
    //}

    //added by ashutosh for apaar validation end

    private void bindDeclaration(string applicantType, int agecount)
    {

        if (agecount == -1)
        {
            LblGuard.Text = txtFatherName.Text;
            LblHGuard.Text = txtFatherName.Text;
            lblname.Text = txtAppName.Text;
            LblHName.Text = txtAppName.Text;
            tddeclaration1.Style.Add("display", "none");
            tddeclaration2.Style.Add("display", "block");
        }
        else
        {
            tddeclaration1.Style.Add("display", "block");
            tddeclaration2.Style.Add("display", "none");
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
                        //Added for BSB
                        if (Session["isBSB"] != null && courseID == 1)
                        {
                            if (Session["isBSB"].ToString() == "1")
                            {
                                RdoUndergngDOEACC.SelectedValue = "D";
                                RdoUndergngDOEACC.SelectedItem.Text = "Direct(BSB)";
                                RdoUndergngDOEACC.Enabled = false;
                                trBSB.Visible = true;
                                lblApplicantType.Text = RdoUndergngDOEACC.SelectedItem.Text;
                                RdoUndergngDOEACC.Visible = false;
                                lblApplicantType.Visible = true;
                            }
                        }
                        if (checkBSB())
                        {
                            RdoUndergngDOEACC.SelectedValue = "D";
                            RdoUndergngDOEACC.SelectedItem.Text = "Direct(BSB)";
                            RdoUndergngDOEACC.Enabled = false;
                            trBSB.Visible = true;
                            lblApplicantType.Text = RdoUndergngDOEACC.SelectedItem.Text;
                            RdoUndergngDOEACC.Visible = false;
                            lblApplicantType.Visible = true;
                        }
                        //////////////



                        RdoUndergngDOEACC_SelectedIndexChanged(this, null);
                    }
                    else
                    {
                        //Added for BSB
                        if (Session["isBSB"] != null && courseID == 1)
                        {
                            if (Session["isBSB"].ToString() == "1")
                            {
                                RdoUndergngDOEACC.SelectedValue = "D";
                                RdoUndergngDOEACC.SelectedItem.Text = "Direct(BSB)";
                                RdoUndergngDOEACC.Enabled = false;
                                trBSB.Visible = true;
                                lblApplicantType.Text = RdoUndergngDOEACC.SelectedItem.Text;
                                RdoUndergngDOEACC.Visible = false;
                                lblApplicantType.Visible = true;
                            }
                        }
                        if (checkBSB())
                        {
                            RdoUndergngDOEACC.SelectedValue = "D";
                            RdoUndergngDOEACC.SelectedItem.Text = "Direct(BSB)";
                            RdoUndergngDOEACC.Enabled = false;
                            trBSB.Visible = true;
                            lblApplicantType.Text = RdoUndergngDOEACC.SelectedItem.Text;
                            RdoUndergngDOEACC.Visible = false;
                            lblApplicantType.Visible = true;
                        }
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
    protected void ShowFeeDetail(Int32 examID)
    {
        try
        {
            int courseID = Convert.ToInt32(Request.QueryString["id"]);

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
                    int FeeTypeID = Convert.ToInt32(enmFeeType.RegistrationFee);
                    LblFeeTypeName.Text = Convert.ToString(enmFeeType.RegistrationFee);
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

                    int ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
                    int LateFeeTypeId = Convert.ToInt32(enmFeeType.LateFeeRegistration);

                    var Fee = (from f in context.CutOffDates
                               where f.CourseID == courseID
                               && f.ExamID == ExamId
                               && f.ApplicantTypeID == ApplicantTypeId
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
                        }
                    }
                    else
                    {
                        LblNormalFee.Text = feeAmount.ToString("F");
                        LblTotalFee.Text = (feeAmount + cscProcessingFee).ToString("F");
                        lblProcessingFee.Text = cscProcessingFee.ToString("F");
                        lblFeeDetail.Text = "Fee : Rs/- " + (feeAmount + cscProcessingFee) + ".00";
                        if (course.CourseCategoryID == 6) //STC
                        {
                            lblFeeDetail.Text = "Fee will be paid to concerned regional center.";
                            ImgBtnPopupFee.Enabled = false;
                        }
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
    protected void activeInactivePrsnlAndAddressDetail(Boolean isActive)
    {
        try
        {
            ddlSalutaionName.Enabled = isActive;
            txtAppName.Enabled = isActive;
            txtFatherName.Enabled = isActive;
            txtMotherName.Enabled = isActive;
            TxtGuardianName.Enabled = isActive;

            //Added 22 Feb 2019
            txtAffidavitNo.Enabled = isActive;
            txtAffidavitDate.Enabled = isActive;
            fileAffidavit.Enabled = isActive;
            //

            //ddl_gender.Enabled = isActive;
            //Added jksah
            ddl_gender.Enabled = isActive;
            txtDob.Enabled = isActive;
            ddlCategory.Enabled = isActive;
            imgDob.Visible = isActive;

            TxtPerAddressLine1.Enabled = isActive;
            TxtPerAddressLine2.Enabled = isActive;
            TxtPerAddressLine3.Enabled = isActive;
            TxtPerCity.Enabled = isActive;
            ddlPState.Enabled = isActive;
            TxtPpincode.Enabled = isActive;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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

            //Added 22 Feb 2019
            txtAffidavitDate.Text = "";
            txtAffidavitNo.Text = "";
            lblAffidavitFile.Text = "";

            //

            TxtCorAddressLine1.Text = "";
            TxtCorAddressLine2.Text = "";
            TxtCorAddressLine3.Text = "";
            TxtCorCity.Text = "";
            TxtPerAddressLine1.Text = "";
            TxtPerAddressLine2.Text = "";
            TxtPerAddressLine3.Text = "";
            TxtPerCity.Text = "";
            TxtSTDcode.Text = "";
            TxtYearOfPassing2.Text = "";
            txtBodyMark.Text = "";
            txtCorMobileNo.Text = "";
            txtCorPhoneNo.Text = "";
            txtCorPinCode.Text = "";
            TxtPpincode.Text = "";
            txtCorPhoneNo.Text = "";
            txtCorPinCode.Text = "";
            txtcode.Text = "";
            txtCorPhoneNo.Text = "";
            txtDob.Text = "";
            txtEmailId.Text = "";
            GenerateNewCaptchaImage();
            ddlCategory.SelectedValue = "0";
            ddlCorState.SelectedValue = "0";
            DDLeducode.SelectedValue = "0";
            ddlSalutaionName.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isDuplicate()
    {
        try
        {
            DateTime dob = Convert.ToDateTime(txtDob.Text);
            Int32 currentCourseID = Convert.ToInt32(Request.QueryString["id"].ToString());
            using (EConnectContext context = new EConnectContext())
            {
                Int32 count;
                Int32 ShortermCoursesCategory = context.CourseCategories.Where(s => s.Code.ToUpper() == "STC").FirstOrDefault().ID;
                Int32 IRDACategory = context.CourseCategories.Where(s => s.Code.ToUpper() == "IRDA").FirstOrDefault().ID;
                Course currentCourse = context.Courses.Find(currentCourseID);
                if (currentCourse.CourseCategoryID == ShortermCoursesCategory)
                {

                    //Added NSQF case - 22 Aug 2019
                    if (currentCourseID > 102)
                    {

                        if (Rdoownertype.SelectedValue == "G")
                        {
                            count = (from c in context.Candidates
                                     join r in context.RegistrationDetails
                                         on c.ID equals r.CandidateID
                                     where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                     && (c.GuardianName.ToUpper() == TxtGuardianName.Text.ToUpper().Trim() || c.FatherName.ToUpper() == txtFatherName.Text.ToUpper().Trim())
                                     && c.Gender == ddl_gender.SelectedValue
                                     && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                     && r.CourseID == currentCourseID
                                     && r.ValidUptoDate >= System.DateTime.Today
                                     select c).Count();
                        }
                        else
                        {

                            count = (from c in context.Candidates
                                     join r in context.RegistrationDetails
                                         on c.ID equals r.CandidateID
                                     where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                     && c.FatherName.Equals(txtFatherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)

                                     && c.Gender == ddl_gender.SelectedValue
                                     && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                     && r.CourseID == currentCourseID
                                     && r.ValidUptoDate >= System.DateTime.Today
                                     select c).Count();

                        }


                    }

                    else
                    {
                        //



                        if (Rdoownertype.SelectedValue == "G")
                        {
                            count = (from c in context.Candidates
                                     join r in context.RegistrationDetails
                                         on c.ID equals r.CandidateID
                                     where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                     && (c.GuardianName.ToUpper() == TxtGuardianName.Text.ToUpper().Trim() || c.FatherName.ToUpper() == txtFatherName.Text.ToUpper().Trim())
                                     && c.Gender == ddl_gender.SelectedValue
                                     && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                     && r.CourseID == currentCourseID
                                     select c).Count();
                        }
                        else
                        {
                            count = (from c in context.Candidates
                                     join r in context.RegistrationDetails
                                         on c.ID equals r.CandidateID
                                     where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                     && c.FatherName.Equals(txtFatherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                 && c.MotherName.Equals(txtMotherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                     && c.Gender == ddl_gender.SelectedValue
                                     && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                     && r.CourseID == currentCourseID
                                     select c).Count();
                        }
                    }
                    if (count > 0)
                        return true;
                    else
                        return false;
                }
                else if (currentCourse.CourseCategoryID == IRDACategory)
                {
                    Int32 ExamId = Convert.ToInt32(ViewState["ExamID"]);
                    if (Rdoownertype.SelectedValue == "G")
                    {
                        count = (from c in context.CourseRegistrationApplications
                                 where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                  && c.GuardianName.Equals(TxtGuardianName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                  && c.ApplicableExamID == ExamId
                                  && c.Gender == ddl_gender.SelectedValue
                                  && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                  && c.CourseID == currentCourseID
                                 select c).Count();
                    }
                    else
                    {
                        count = (from c in context.CourseRegistrationApplications
                                 where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                 && c.FatherName.Equals(txtFatherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                 && c.MotherName.Equals(txtMotherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                 && c.ApplicableExamID == ExamId
                                 && c.Gender == ddl_gender.SelectedValue
                                 && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                 && c.CourseID == currentCourseID
                                 select c).Count();
                    }
                    if (count > 0)
                        return true;
                    else
                        return false;
                }
                else
                {
                    if (Rdoownertype.SelectedValue == "G")
                    {
                        count = (from c in context.Candidates
                                 join r in context.RegistrationDetails
                                     on c.ID equals r.CandidateID
                                 where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                  && (c.GuardianName.ToUpper() == TxtGuardianName.Text.ToUpper().Trim() || c.FatherName.ToUpper() == txtFatherName.Text.ToUpper().Trim())
                                  && c.Gender == ddl_gender.SelectedValue
                                  && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                  && r.CourseCategoryID == currentCourse.CourseCategoryID
                                 select c).Count();
                    }
                    else
                    {
                        count = (from c in context.Candidates
                                 join r in context.RegistrationDetails
                                     on c.ID equals r.CandidateID
                                 where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                 && c.FatherName.Equals(txtFatherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                && c.MotherName.Equals(txtMotherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                                 && c.Gender == ddl_gender.SelectedValue
                                 && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                 && r.CourseCategoryID == currentCourse.CourseCategoryID
                                 select c).Count();
                    }
                    if (count > 0)
                        return true;
                    else
                        return false;
                }
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void RenderPage(Int32 currentCourseID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                int id = Convert.ToInt32(Request.QueryString["id"].ToString());
                Course currentCourse = context.Courses.Find(currentCourseID);
                Int32 ShortermCoursesCategory = context.CourseCategories.Where(s => s.Code.ToUpper() == "STC").FirstOrDefault().ID;
                ListItem lst = new ListItem("--Select One--", "0");
                Lblhead.Text = currentCourse.Name;
                var cname = from c in context.Courses
                            where c.ID == id
                            select new { ValueField = c.ID, TextField = c.Name };

                DDLRegForCourse.DataSource = cname.ToList();
                DDLRegForCourse.DataValueField = "ValueField";
                DDLRegForCourse.DataTextField = "TextField";
                DDLRegForCourse.DataBind();
                DDLRegForCourse.Enabled = false;


                //showing declaration
                if (currentCourse.CourseCategoryID == ShortermCoursesCategory)
                {
                    Label90.Text = "Registration Cycle / पंजीकरण चक्र";
                    tddeclaration1.Style.Add("display", "none");
                    tddeclaration2.Style.Add("display", "block");
                    trnote.Visible = true;
                    lbldeccoursecode.Text = GetInitCap(currentCourse.Name);
                    lbldeccoursecode.Text = GetInitCap(currentCourse.Name);
                    LblHCourseCode.Text = GetInitCap(currentCourse.Name);
                    ddlPaymentOption.Items.RemoveAt(0);
                }
                else if (currentCourse.Code == "ACC")
                { Label90.Text = "Registration Cycle / पंजीकरण चक्र"; }
                else
                {
                    tddeclaration1.Style.Add("display", "block");
                    tddeclaration2.Style.Add("display", "none");
                    trnote.Visible = false;
                }

                //DDLRegForCourse.Enabled = false;

            }
            ;

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
            Int32 ExamId = Convert.ToInt32(ViewState["ExamID"]);
            // added_by_amit_april_changed
            Int32 projectId = (string.IsNullOrEmpty(DdlProject.SelectedValue) || DdlProject.SelectedValue == "0") ? -99 : Convert.ToInt32(DdlProject.SelectedValue);

            Int32 resvalue = utility.CheckCandEmailMobile(
                txtCorMobileNo.Text,
                txtEmailId.Text,
                ExamId,
                1,
                projectId
            );

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
                ShowAlert("Email and Mobile already exists for 3 records in existing Exam Cycle, Not Saved");
                return false;
            }

            // added_by_amit_april_changed

            DDLeducode_SelectedIndexChanged(DDLeducode, EventArgs.Empty);
            if (RdoUndergngDOEACC.SelectedValue == "I")
            {
                // added by amit  Start
                if (radProject.SelectedValue == "1")
                {
                    if (!WhetherAccInstitutesInProject())
                    {
                        ShowAlert("Selected Institute Does not have project");
                        return false;
                    }

                    //if (trUPField1.Visible)
                    if (trBSB.Visible)
                    {
                        //if (!isBlank(txtField1))
                        if (!isBlank(txtUDISECode))
                        {
                            lblerror.Visible = true;
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            //lblerror.Text = "SchoolCode can not be left blank";
                            lblerror.Text = Label14.Text + " can not be left blank";
                            return false;
                        }
                        //if (String.IsNullOrWhiteSpace(txtField1.Text))
                        if (String.IsNullOrWhiteSpace(txtUDISECode.Text))
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            //throw new Exception("Please enter SchoolCode");
                            throw new Exception("Please enter " + Label14.Text);
                        }
                    }

                    if (trUPField2.Visible)
                    {
                        if (!isBlank(txtField2))
                        {
                            lblerror.Visible = true;
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            //lblerror.Text = "Class can not be left blank";
                            lblerror.Text = lblField2.Text + " can not be left blank";
                            return false;
                        }
                        if (String.IsNullOrWhiteSpace(txtField2.Text))
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            //throw new Exception("Please enter Class");
                            throw new Exception("Please enter" + lblField2.Text);
                        }
                    }

                    if (trUPField2.Visible)
                    {
                        if (!isBlank(txtField2))
                        {
                            lblerror.Visible = true;
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            lblerror.Text = lblField2.Text + " can not be left blank";
                            return false;
                        }

                        if (string.IsNullOrWhiteSpace(txtField2.Text))
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            throw new Exception("Please enter " + lblField2.Text);
                        }

                        if (!Regex.IsMatch(txtField2.Text.Trim(), @"^\d{1,2}$"))
                        {
                            txtField2.Focus();
                            throw new Exception(lblField2.Text + " should be a valid 1- or 2-digit number.");
                        }
                    }

                    // Validate Roll Number (Field3)
                    if (trUPField3.Visible)
                    {
                        if (!isBlank(txtField3))
                        {
                            lblerror.Visible = true;
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            lblerror.Text = lblField3.Text + " can not be left blank";
                            return false;
                        }

                        if (string.IsNullOrWhiteSpace(txtField3.Text))
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            throw new Exception("Please enter " + lblField3.Text);
                        }

                        if (!Regex.IsMatch(txtField3.Text.Trim(), @"^[a-zA-Z0-9\-]+$"))
                        {
                            txtField3.Focus();
                            throw new Exception(lblField3.Text + " should be alphanumeric only (letters and numbers).");
                        }
                    }

                    if (trUPField5.Visible)
                    {

                        if (!isBlank(txtField5))
                        {
                            lblerror.Visible = true;
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            //lblerror.Text = "Roll Number can not be left blank";
                            lblerror.Text = lblField5.Text + " can not be left blank";
                            return false;
                        }
                        if (String.IsNullOrWhiteSpace(txtField5.Text))
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            //throw new Exception("Please enter Roll Number");
                            throw new Exception("Please enter " + lblField5.Text);
                        }
                    }
                    if (DdlProject.SelectedValue == "0")
                    {
                        lblerror.Visible = true;
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        lblerror.Text = "Please Select Project of Accredited Institute";
                        DdlAccState.Focus();
                        return false;
                    }




                    if (radProject.SelectedValue == "1" && DdlProject.SelectedItem.Text.IndexOf("school", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        if (txtapaar.Text == "")
                        {
                            ShowAlert("Please enter Apaar again");
                            return false;
                        }
                        int validationResult = validateDataFromUploadedExcel();

                        if (validationResult == 0)
                        {
                            ShowAlert("Data mismatch with the uploaded data.Kindly Fill correct data only.");

                            return false;
                        }
                        else if (validationResult == -1)
                        {
                            ShowAlert("Data Not Found. Kindly contact respective Center");

                            return false;
                        }
                        // else if == 1 → valid, so continue as normal
                    }






                }
                // added by amit End
                if (DdlAccState.SelectedValue == "0")
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Please Select State of Accredited Institute";
                    DdlAccState.Focus();
                    return false;
                }
                if (DdlAccCentre.SelectedValue == "0")
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Please Select Centre of Accredited Institite";
                    DdlAccCentre.Focus();
                    return false;
                }
            }

            if (string.IsNullOrEmpty(LblExamName.Text))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "You cannot apply as no Exam Name found";
                btnSave.Visible = false;
                return false;
            }
            if (ddlSalutaionName.SelectedValue == "0")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Salutation";
                ddlSalutaionName.Focus();
                return false;
            }
            if (!isBlank(txtAppName))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Applicant Name can not be left blank";
                return false;
            }

            if (!Char.IsLetter(txtAppName.Text, 0))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Applicant Name should start with an alphabet.");
            }

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

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtAppName.Text, "^[a-zA-Z().\u00FC\u00DC ]*$"))
            {
                txtAppName.Text = "";
                txtAppName.Focus();
                throw new Exception("Applicant Name should be with an English Alphabets(e.g - a-zA-Z)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtPerAddressLine1.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtPerAddressLine1.Text = "";
                TxtPerAddressLine1.Focus();
                throw new Exception("Address Line1 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtPerAddressLine2.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtPerAddressLine2.Text = "";
                TxtPerAddressLine2.Focus();
                throw new Exception("Address Line2 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtPerAddressLine3.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtPerAddressLine3.Text = "";
                TxtPerAddressLine3.Focus();
                throw new Exception("Address Line3 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtPerCity.Text, "^[a-zA-Z\u00FC\u00DC ]"))
            {
                TxtPerCity.Text = "";
                TxtPerCity.Focus();

                throw new Exception("City Name should be with an English Alphabets(e.g - a-zA-Z)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorAddressLine1.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtCorAddressLine1.Text = "";
                TxtCorAddressLine1.Focus();
                throw new Exception("Correspondence Address Line1 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorAddressLine2.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtCorAddressLine2.Text = "";
                TxtCorAddressLine2.Focus();
                throw new Exception("Correspondence Address Line2 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorAddressLine3.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtCorAddressLine3.Text = "";
                TxtCorAddressLine3.Focus();
                throw new Exception("Correspondence Address Line3 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorCity.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
            {
                TxtCorCity.Text = "";
                TxtCorCity.Focus();
                throw new Exception("Correspondence City Name should be with an English Alphabets(e.g - a-zA-Z)");
            }



            if (Rdoownertype.SelectedValue == "P")//Parents
            {
                if (String.IsNullOrWhiteSpace(txtFatherName.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Please enter Father's Name.");
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



                if (!System.Text.RegularExpressions.Regex.IsMatch(txtFatherName.Text, "^[a-zA-Z.\u00FC\u00DC ]*$"))
                {
                    txtFatherName.Text = "";
                    txtFatherName.Focus();
                    throw new Exception("Father Name should be with an English Alphabets(e.g - a-zA-Z)");
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(txtMotherName.Text, "^[a-zA-Z.\u00FC\u00DC ]*$"))
                {
                    txtMotherName.Text = "";
                    txtMotherName.Focus();
                    throw new Exception("Mother Name should be with an English Alphabets(e.g - a-zA-Z)");
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

                    if (TxtGuardianName.Text.Length == 1)
                    {
                        TxtGuardianName.Text = "";
                        TxtGuardianName.Focus();
                        throw new Exception("Guardian Name should be single Character.");
                    }

                    if (!System.Text.RegularExpressions.Regex.IsMatch(TxtGuardianName.Text, "^[a-zA-Z.\u00FC\u00DC ]*$"))
                    {
                        TxtGuardianName.Text = "";
                        TxtGuardianName.Focus();
                        throw new Exception("Guardian Name should be with an English Alphabets(e.g - a-zA-Z)");
                    }

                    if (!Char.IsLetter(TxtGuardianName.Text, TxtGuardianName.Text.Length - 1))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Guardian Name should end with an alphabet.");
                    }
                }

                //Added 22 Feb 2019
                if (String.IsNullOrWhiteSpace(txtAffidavitNo.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Please enter Affidavit No.");
                }
                if (String.IsNullOrWhiteSpace(txtAffidavitDate.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Please enter Affidavit Date.");
                }
                DateTime affidavitDate = Convert.ToDateTime(txtAffidavitDate.Text);
                if (affidavitDate > System.DateTime.Today)
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception(" Affidavit Date cannot be more than today's date");

                }
                //Added 18 june 2019
                if (affidavitDate < System.DateTime.Today.AddYears(-1))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception(" Affidavit Date should not be older than 1 year");

                }
                //
                if (fileAffidavit.HasFile && fileAffidavit.FileName.Length > 50)
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Affidavit file name should be less than 50 characters.");
                }
                if (fileAffidavit.FileName.ToString() == "")
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Affidavit File name can not be left blank";
                    return false;
                }
                String fileExtension = System.IO.Path.GetExtension(fileAffidavit.FileName).ToLower();
                if (fileExtension != ".pdf")
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Invalid Affidavit file .Only pdf extensions are allowed.";
                    return false;
                }
                if (!isvalidFileSize(fileAffidavit, 102400))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Affidavit File size should be of 100 KB or less.";
                    return false;
                }
                //
            }

            if (!isBlank(txtDob))
            {
                lblerror.Visible = true;
                lblerror.Text = "Date Of Birth can not be left blank";
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                return false;
            }
            if (!isValidDob(txtDob))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Date of Birth";
                return false;
            }
            if (!isSelected(ddlMStatus))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Marital Status";
                return false;
            }
            if (!isSelected(ddlCategory))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Cast Category";
                return false;
            }
            if (!isSelected(ddlReligion))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Religion";
                return false;
            }
            if (String.IsNullOrEmpty(TxtSTDcode.Text) == false && string.IsNullOrEmpty(txtCorPhoneNo.Text) == false)
            {

                if (!isNumber(TxtSTDcode))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Invalid STD Code";
                    return false;
                }

                if (!isNumber(txtCorPhoneNo))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Invalid Phone Number";
                    return false;
                }
            }
            if (!isBlank(txtCorMobileNo))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Mobile Number can not be left blank";
                return false;
            }
            if (!isNumber(txtCorMobileNo))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Mobile Number";
                return false;
            }
            if (!isBlank(txtEmailId))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Email Id can not be left blank";
                return false;
            }
            if (!isBlank(TxtPerAddressLine1))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Permanent Address Line1 can not be left blank";
                return false;
            }
            if (!isBlank(TxtPerAddressLine2))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Permanent Address Line2 can not be left blank";
                return false;
            }
            if (!isBlank(TxtPerCity))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Permanent City can not be left blank";
                return false;
            }

            if (!isSelected(ddlPState))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select Permanent State";
                return false;
            }
            if (!isSelected(ddlPdistrict))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Permanent District";
                return false;
            }
            if (!isBlank(TxtPpincode))
            {
                lblerror.Visible = true;
                lblerror.Text = "Pin Code can not be left blank";
                return false;
            }
            if (!isNumber(TxtPpincode))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Pin Code Number";
                return false;
            }
            if (chkSame.Checked == false)
            {
                if (!isBlank(TxtCorAddressLine1))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "correspondence Address Line1 can not be left blank";
                    return false;
                }
                if (!isBlank(TxtCorAddressLine2))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "correspondence Address Line2 can not be left blank";
                    return false;
                }
                if (!isBlank(TxtCorCity))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "correspondence  City can not be left blank";
                    return false;
                }

                if (!isSelected(ddlCorState))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Please Select correspondence State";
                    return false;
                }
                if (!isSelected(ddldistrict))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Please Select correspondence District";
                    return false;
                }
                if (!isBlank(txtCorPinCode))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Pin Code can not be left blank";
                    return false;
                }
                if (!isNumber(txtCorPinCode))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Invalid  Pin Code Number";
                    return false;
                }
            }
            if (!isSelected(DDLeducode))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Highest Education";
                return false;
            }

            if (!isBlank(TxtYearOfPassing2))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Passing Year can not be left blank";
                return false;
            }
            if (!isNumber(TxtYearOfPassing2))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Highest Education Passing Year ";
                return false;
            }

            if (!isValidPassingYear(TxtYearOfPassing2, txtDob))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Highest Education Passing Year ";
                return false;
            }
            if (RdoUndergngDOEACC.SelectedValue == "D")
            {
                if (HfExperience.Value != "0")
                {

                    if (!isBlank(TxtExperienceInYears))
                    {
                        lblerror.Visible = true;
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        lblerror.Text = "Candidate Experience can not be left blank";
                        return false;
                    }
                    if (!IsNumeric(TxtExperienceInYears.Text))
                    {
                        lblerror.Visible = true;
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        lblerror.Text = "Only Numeric Experience allowed!";
                        return false;
                    }
                    if (Convert.ToDecimal(TxtExperienceInYears.Text) >= 10)
                    {
                        lblerror.Visible = true;
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        lblerror.Text = "Experience should be less than 10 years.";
                        return false;
                    }
                    if (Convert.ToDecimal(TxtExperienceInYears.Text) < Convert.ToDecimal(HfExperience.Value))
                    {
                        lblerror.Visible = true;
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        lblerror.Text = "Minimum " + HfExperience.Value + " years of experience required";
                        return false;
                    }
                }
                else
                {
                    if (!isBlank(TxtExperienceInYears))
                    {
                        lblerror.Visible = true;
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        lblerror.Text = "Candidate Experience can not be left blank";
                        return false;
                    }
                    if (!IsNumeric(TxtExperienceInYears.Text))
                    {
                        lblerror.Visible = true;
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        lblerror.Text = "Only Numeric Experience allowed!";
                        return false;
                    }
                    if (Convert.ToDecimal(TxtExperienceInYears.Text) >= 10)
                    {
                        lblerror.Visible = true;
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        lblerror.Text = "Experience should be less than 10 years.";
                        return false;
                    }
                }
            }
            //Added for NSQF Courses
            else
            {
                Int64 courseId = Convert.ToInt64(DDLRegForCourse.SelectedValue);
                //This Query will get all the information of Applied Canditate by generated Application id                                     
                EConnectContext context = new EConnectContext();
                var course = context.Courses.Find(courseId);

                if (course.CourseCategoryID.ToString() == "6" && Convert.ToInt32(course.ID.ToString()) > 102)
                {
                    if (HfExperience.Value != "0")
                    {

                        if (!isBlank(TxtExperienceInYears))
                        {
                            lblerror.Visible = true;
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            lblerror.Text = "Candidate Experience can not be left blank";
                            return false;
                        }
                        if (!IsNumeric(TxtExperienceInYears.Text))
                        {
                            lblerror.Visible = true;
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            lblerror.Text = "Only Numeric Experience allowed!";
                            return false;
                        }
                        if (Convert.ToDecimal(TxtExperienceInYears.Text) >= 10)
                        {
                            lblerror.Visible = true;
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            lblerror.Text = "Experience should be less than 10 years.";
                            return false;
                        }
                        if (Convert.ToDecimal(TxtExperienceInYears.Text) < Convert.ToDecimal(HfExperience.Value))
                        {
                            lblerror.Visible = true;
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            lblerror.Text = "Minimum " + HfExperience.Value + " years of experience required";
                            return false;
                        }
                    }

                }
            }

            /*
                        if (!String.IsNullOrEmpty(txtaadhar.Text))
                        {
                            if (!IsNumeric(txtaadhar.Text))
                            {
                                GenerateNewCaptchaImage();
                                txtcode.Text = "";
                                throw new Exception("Not valid Aadhaar Number.");
                            }
                        }
            */


            // added_by_amit_audit_april_2026_start
            if (!String.IsNullOrEmpty(hfaadhaar.Value))
            {
                string decryptAadhaar = EncryptDecrypt.DecryptString(hfaadhaar.Value);

                if (!IsNumeric(decryptAadhaar))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Not valid Aadhaar Number.";
                    return false;
                }

                //if (decryptAadhaar.Length != 12)
                //{
                //    GenerateNewCaptchaImage();
                //    txtcode.Text = "";
                //    throw new Exception("Aadhaar Number should of 12 digits.");
                //}

                //if (String.IsNullOrEmpty(decryptAadhaar.Trim()))
                //{
                //    GenerateNewCaptchaImage();
                //    throw new Exception("Aadhaar ID should not be Blank.");
                //}
            }
            else
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please enter Aadhaar number and click the 'Save Aadhaar' button.";
                return false;
            }
            // added_by_amit_audit_april_2026_end
            if (UIDtr.Visible == true)
            {
                if (isSelected(UidTypeDdl))
                {
                    if (UidTypeDdl.SelectedValue == "1")  // ----Aadhaar
                    {
                        if (!IsNumeric(UidNumberTxt.Text))
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            throw new Exception("Not valid Aadhaar Number.");
                        }
                        if (UidNumberTxt.Text.Length != 12)
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            throw new Exception("Aadhaar Number should of 12 digits.");
                        }
                    }
                    else if (UidTypeDdl.SelectedValue == "2")  // ----PAN
                    {
                        if (!Char.IsLetter(UidNumberTxt.Text, 0))
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            throw new Exception("PAN should start with an alphabet.");
                        }
                        if (UidNumberTxt.Text.Length != 10)
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            throw new Exception("PAN should of 10 digits.");
                        }
                    }
                }

            }
            if (txtaadhar.Text.Length != 12)
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Aadhaar should be of 12 digits.");
            }
            if (String.IsNullOrEmpty(txtapaar.Text.Trim()))
            {
                GenerateNewCaptchaImage();
                throw new Exception("Apaar ID  should not be Blank.");

            }
            if (txtapaar.Text.Length != 12)
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Apaar should be of 12 digits.");
            }
            //added validation for new fields start
            if (TrConsentRelation.Visible)
            {
                if (string.IsNullOrEmpty(ddlConsentRelation.SelectedValue))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please select Consent Relation";
                    return false;
                }
            }
            if (TrAuthMode.Visible)
            {
                if (string.IsNullOrEmpty(ddlAuthMode.SelectedValue) || ddlAuthMode.SelectedValue == "0")
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Please select Authentication Mode";
                    return false;
                }
            }
            if (string.IsNullOrWhiteSpace(txtAuthenticationIdNo.Text))
            {
                lblerror.Visible = true;
                lblerror.Text = "Authentication ID cannot be blank";
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtConsentDate.Text))
            {
                lblerror.Text = "Consent Date cannot be blank";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtConsentTime.Text))
            {
                lblerror.Visible = true;
                lblerror.Text = "Consent Time cannot be blank";
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtConsentPlace.Text))
            {
                lblerror.Visible = true;
                lblerror.Text = "Consent Place cannot be blank";
                return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtConsentPlace.Text, @"^[A-Za-z'-]+( [A-Za-z'-]+)*$"))
            {
                lblerror.Visible = true;
                lblerror.Text = "Consent Place can contain only alphabets, one space, and apostrophe.";
                return false;
            }
            //added validation for new fields end
            if (ImgUpload.FileName.ToString() == "" && string.IsNullOrEmpty(lblphotoShow.Text)) //image upload code added_by_amit_audit_april_2026_start
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Photo can not be left blank";
                return false;
            }
            if (!isvalidFileExtension(ImgUpload) && string.IsNullOrEmpty(lblphotoShow.Text)) //image upload code added_by_amit_audit_april_2026_start
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Photo .Only jpg, gif, jpeg ,png extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(ImgUpload, 51200) && string.IsNullOrEmpty(lblphotoShow.Text)) //image upload code added_by_amit_audit_april_2026_start
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "File size should be of 50 KB or less.";
                return false;
            }
            if (ImgUpload.HasFile && ImgUpload.FileName.Length > 50)
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Photograph file name should be less than 50 characters.");
            }
            if (ImgUploadSignature.FileName.ToString() == "" && string.IsNullOrEmpty(lblsignShow.Text)) //image upload code added_by_amit_audit_april_2026_start
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Signature can not be left blank";
                return false;
            }
            if (!isvalidFileExtension(ImgUploadSignature) && string.IsNullOrEmpty(lblsignShow.Text)) //image upload code added_by_amit_audit_april_2026_start
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Signature .Only jpg, gif, jpeg ,png extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(ImgUploadSignature, 51200) && string.IsNullOrEmpty(lblsignShow.Text)) //image upload code added_by_amit_audit_april_2026_start
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Signature File size should be of 50 KB or less.";
                return false;
            }
            if (ImgUploadSignature.HasFile && ImgUploadSignature.FileName.Length > 50)
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Signature file name should be less than 50 characters.");
            }
            if (ImgUploadThumb.FileName.ToString() == "" && string.IsNullOrEmpty(lblthumbshow.Text)) //image upload code added_by_amit_audit_april_2026_start
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Thumb Impression can not be left blank";
                return false;
            }
            if (!isvalidFileExtension(ImgUploadThumb) && string.IsNullOrEmpty(lblthumbshow.Text)) //image upload code added_by_amit_audit_april_2026_start
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Thumb Impression .Only jpg, gif, jpeg ,png extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(ImgUploadThumb, 51200) && string.IsNullOrEmpty(lblthumbshow.Text)) //image upload code added_by_amit_audit_april_2026_start
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Thumb Impression File size should be of 50 KB or less.";
                return false;
            }

            if (ImgUploadThumb.HasFile && ImgUploadThumb.FileName.Length > 50)
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Thumb Impression file name should be less than 50 characters.");
            }
            try
            {
                byte[] imgbytes = ImgUpload.FileBytes;
            }
            catch (Exception ex)
            {
                ShowAlert("Photo Upload Failed, Pls rename file and try again.");
                return false;
            }

            try
            {
                byte[] signbytes = ImgUploadSignature.FileBytes;
            }
            catch (Exception ex)
            {
                ShowAlert("Sign Upload Failed, Pls rename file and try again.");
                return false;
            }

            try
            {
                byte[] thumbnbytes = ImgUploadThumb.FileBytes;
            }
            catch (Exception ex)
            {
                ShowAlert("Thumb Upload Failed, Pls rename file and try again.");
                return false;
            }



            if (!isBlank(txtcode))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Captcha Code can not be left blank";
                return false;
            }
            if (txtcode.Text != ViewState["CaptchCode"].ToString())
            {
                lblerror.Visible = true;
                lblerror.Text = "Invalid Captcha Code";
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                return false;
            }
            //Added 18 june 2019

            if (chkdisclamier.Checked != true)
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Please check the Declaration Statement");
            }

            //added by ashutosh start 
            if (chkApaarDeclaration.Checked != true)
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Please check the Apaar Declaration Statement");
            }
            //added by ashutosh end 

            if (!isNumber(txtapaar))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtapaar.Text = "";
                lblerror.Text = "Invalid Apaar, Please generate and enter";
                throw new Exception("Invalid Apaar, Please generate and enter");

            }
            if (txtapaar.Text.Length != 12)
            {
                txtapaar.Text = "";
                txtapaar.Focus();
                throw new Exception("Invalid ApaarID.");
            }
            return true;

        }
        catch (Exception ex)
        {
            GenerateNewCaptchaImage();
            txtcode.Text = "";
            throw ex;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidForm())
            {

                //Validation of apaar start
                string gender = ddl_gender.SelectedValue.Substring(0, 1).ToUpper();
                long apaarRequestId;

                string validatedApaarData = validateApaar.ConvertApaarDatatoJSONandEncrypt(txtapaar.Text, txtAppName.Text, txtDob.Text, gender, txtproviderName.Text, ddlAuthMode.SelectedItem.Text, txtAuthenticationIdNo.Text, ddlConsentRelation.SelectedItem.Text, txtConsentPlace.Text, lblApaarDeclaration.Text, out apaarRequestId);


                if (String.IsNullOrWhiteSpace(validatedApaarData))
                {
                    txtcode.Text = "";
                    GenerateNewCaptchaImage();

                    throw new Exception("Apaar ID could not be validated.");
                }
                List<string> invalidList = new List<string>
        {
            "error","Error",  "Server",
                     "server"
        };

                if (invalidList.Any(keyword =>
                    validatedApaarData.Contains(keyword)))
                {
                    throw new Exception("Response not received from APAAR. Kindly contact the APAAR team.");
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
                    if (apaarObj["message"].ToString().ToLower().Trim() != "records not found")
                    {
                        responseObj.abc_account_id = txtapaar.Text;
                        // cname
                        if (apaarObj["CNAME"] != null)
                        {
                            responseObj.cname = apaarObj["CNAME"].ToString();
                        }
                        /* responseObj.cname =
                             apaarObj["CNAME"] != null
                             ? apaarObj["CNAME"].ToString()
                             : "";*/

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

                        //// dob
                        if (apaarObj["DOB"] != null)
                        {
                            DateTime parsedDob;

                            if (DateTime.TryParseExact(apaarObj["DOB"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDob))

                            {
                                responseObj.dob = parsedDob;
                            }
                        }
                        //if (apaarObj["DOB"] != null)
                        // {
                        //     DateTime parsedDob;
                        //     DateTime enteredDob;

                        //     if (
                        //         DateTime.TryParse(apaarObj["DOB"].ToString(), out parsedDob)
                        //         &&
                        //         DateTime.TryParse(txtDob.Text, out enteredDob)
                        //     )
                        //     {
                        //         // Compare complete date (day/month/year)
                        //         if (parsedDob.Date == enteredDob.Date)
                        //         {
                        //             responseObj.dob = parsedDob;

                        //         }
                        //         else
                        //         {  
                        //             throw new Exception("DOB does not match with APAAR record.");  
                        //         }
                        //     }
                        //     else
                        //     {
                        //         throw new Exception("Invalid DOB format.");
                        //     }

                        // }
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

                    }
                    else
                    {
                        responseObj.status = false;
                        if (apaarObj["message"].ToString().ToLower().Trim() == "records not found")
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

                                //if (matchObj["student_name_match"] != null)
                                //{
                                //    responseObj.nameMatch =
                                //        Convert.ToBoolean(
                                //            matchObj["student_name_match"]
                                //        );
                                //}
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
                    /* if (apaarObj["DOB"] != null)
                     {
                         DateTime parsedDob;
                         DateTime enteredDob;

                         if (
                             DateTime.TryParse(apaarObj["DOB"].ToString(), out parsedDob)
                             &&
                             DateTime.TryParse(txtDob.Text, out enteredDob)
                         )
                         {
                             // Compare complete date (day/month/year)
                             if (parsedDob.Date == enteredDob.Date)
                             {
                                 responseObj.dob = parsedDob;

                             }
                             else
                             {
                                 throw new Exception("DOB does not match with APAAR record.");
                             }
                         }
                         else
                         {
                             throw new Exception("Invalid DOB format.");
                         }

                     }*/

                }

                if (message.ToLower().Trim() == "records not found")
                    throw new Exception("Apaar Details Not found. Please Enter Correct 12 Digit Number.");

                if (status == "fail")
                {
                    txtcode.Text = "";
                    GenerateNewCaptchaImage();
                    // mismatch validations

                    if (apaarObj["match_data_status"] != null)
                    {
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
                    }

                    throw new Exception(message);
                }



                //else
                //{
                //    txtcode.Text = "";
                //    GenerateNewCaptchaImage();
                //    throw new Exception("Apaar could not be validated, Try again");
                //}

                //Check for duplicate Apaar for Exam and Course
                string apaarEncryptedCheck = EncryptDecrypt.EncryptString(txtapaar.Text);
                Int64 applId = Convert.ToInt64(Request.QueryString["Appid"]);
                EConnectContext context = new EConnectContext();
                Course currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["id"]));
                Int32 CourseId = currentCourse.ID;
                Int32 ExamId = Convert.ToInt32(ViewState["ExamID"]);
                string duplicate = checkDuplicateApaar(apaarEncryptedCheck, CourseId, ExamId);

                if (duplicate != "0" && duplicate != "-99")
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                    {
                        Int64 applId1 = Convert.ToInt64(Request.QueryString["Appid"]);
                        //This Query will get all the information of Applied Canditate by generated Application id
                        CourseRegistrationApplication oldApplication = context.CourseRegistrationApplications.Find(applId1);
                        if (oldApplication.Number != duplicate)
                            throw new Exception("Duplicate application for the exam cycle not allowed, Check status for " + duplicate.ToString() + " Application");
                    }
                    else
                        throw new Exception("Duplicate application for the exam cycle not allowed, Check status for " + duplicate.ToString() + " Application");

                }

                if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                {
                    btnSave.Text = "Update";
                    btnback.Visible = false;
                    UpdateData(apaarRequestId);
                }
                else
                {
                    btnSave.Text = "Submit";
                    btnback.Visible = true;
                    SaveData(apaarRequestId);
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        /*catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
        {
            Exception raise = dbEx;
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    string message = string.Format("{0}:{1}",
                        validationErrors.Entry.Entity.ToString(),
                        validationError.ErrorMessage);
                    // raise a new exception nesting  
                    // the current instance as InnerException  
                    raise = new InvalidOperationException(message, raise);
                }
            }
            throw raise;
        }*/
    }


    protected void chkSame_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkSame.Checked)
            {
                bool isComplete = true;
                if (!isBlank(TxtPerAddressLine1))
                {
                    lblerror.Text = "Permanent Address Line1 can not be left blank";
                    isComplete = false;
                }
                else if (!isBlank(TxtPerAddressLine2))
                {
                    lblerror.Text = "Permanent Address Line2 can not be left blank";
                    isComplete = false;
                }
                else if (!isBlank(TxtPerCity))
                {
                    lblerror.Text = "Permanent City can not be left blank";
                    isComplete = false;
                }

                else if (!isSelected(ddlPState))
                {
                    lblerror.Text = "Please Select Permanent State";
                    isComplete = false;
                }
                else if (!isSelected(ddlPdistrict))
                {
                    lblerror.Text = "Please Select Permanent District";
                    isComplete = false;
                }
                else if (!isBlank(TxtPpincode))
                {
                    lblerror.Text = "Pin Code can not be left blank";
                    isComplete = false;
                }
                else if (!isNumber(TxtPpincode))
                {
                    lblerror.Text = "Invalid Pin Code Number";
                    isComplete = false;
                }
                else if (TxtPpincode.Text.Length != 6)
                {
                    lblerror.Text = "Invalid Pin Code Number";
                    isComplete = false;
                }
                if (isComplete == false)
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    chkSame.Checked = false;
                    return;
                }
                TxtCorAddressLine1.Enabled = false;
                TxtCorAddressLine2.Enabled = false;
                TxtCorAddressLine3.Enabled = false;
                TxtCorCity.Enabled = false;
                txtCorPinCode.Enabled = false;
                ddlCorState.Enabled = false;
                ddldistrict.Enabled = false;
                TxtCorAddressLine1.Text = TxtPerAddressLine1.Text;
                TxtCorAddressLine2.Text = TxtPerAddressLine2.Text;
                TxtCorAddressLine3.Text = TxtPerAddressLine3.Text;
                TxtCorCity.Text = TxtPerCity.Text;
                txtCorPinCode.Text = TxtPpincode.Text;
                ddlCorState.SelectedValue = ddlPState.SelectedValue;
                ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                ddldistrict.SelectedValue = ddlPdistrict.SelectedValue;
            }
            else
            {
                TxtCorAddressLine1.Enabled = true;
                TxtCorAddressLine2.Enabled = true;
                TxtCorAddressLine3.Enabled = true;
                TxtCorCity.Enabled = true;
                txtCorPinCode.Enabled = true;
                ddlCorState.Enabled = true;
                ddldistrict.Enabled = true;
                TxtCorAddressLine1.Text = "";
                TxtCorAddressLine2.Text = "";
                TxtCorAddressLine3.Text = "";
                TxtCorCity.Text = "";
                txtCorPinCode.Text = "";
                ddlCorState.SelectedValue = "0";
                ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                ddldistrict.SelectedValue = "0";
            }
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
            Int32 courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
            BindAccCentre(stateid, courseID);


        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }
    protected void btnback_Click(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(Request.QueryString["RU"]))
            {
                Response.Redirect("../nieletpaymentservices.aspx");
            }
            else
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../WEB/RulesForOnlineRegistration.aspx?ID=" + Request.QueryString["id"]), false);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }

    }
    protected void RdoUndergngDOEACC_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            LblExamName.Text = "";
            if (RdoUndergngDOEACC.SelectedValue == "I")
            {
                TrLastCenterAccno.Visible = true;
                TrAccCentre.Visible = true;
                TrExperience.Visible = false;
                //Added for NSQF
                EConnectContext context = new EConnectContext();
                Int64 courseId = Convert.ToInt64(Request.QueryString["Id"]);
                var course = context.Courses.Find(courseId);
                if (course.CourseCategoryID.ToString() == "6" && Convert.ToInt32(course.ID.ToString()) > 102)
                {
                    TrExperience.Visible = true;
                    Label63.Text = "Experience in years / वर्षों का अनुभव";
                }
                //
                ddlPaymentOption.SelectedValue = "2";
                ddlPaymentOption.Enabled = true;
                //DdlAccState.SelectedValue = "0";
                DdlAccState_SelectedIndexChanged(DdlAccState, EventArgs.Empty);
                bindDeclaration("I", 0);
                //added by amit start
                if (courseId == 1)
                    TrUPBoard.Visible = true;


                // to reset the fields set by Select Project, added 25/06/25
                radProject.SelectedValue = "0";
                DdlAccState.SelectedValue = "0";
                DdlAccCentre.SelectedValue = "0";
                DdlAccState.Enabled = true;

                //added by amit end

            }
            else
            {
                TrExperience.Visible = true;
                //Added for NSQF courses
                Label63.Text = " If direct, experience in years / यदि  डायरेक्ट, वर्षों का अनुभव";
                TrLastCenterAccno.Visible = false;
                TrAccCentre.Visible = false;
                ddlPaymentOption.SelectedValue = "1";
                ddlPaymentOption.Enabled = false;
                txtDob.Text = "";
                bindDeclaration("D", 0);
                // added by amit start
                TrUPBoard.Visible = false;
                trUPProject.Visible = false; // radio button

                if (Session["isBSB"].ToString() != "1" || !checkBSB())
                    trBSB.Visible = false;

                trUPField2.Visible = false;
                trUPField3.Visible = false;
                trUPField4.Visible = false;
                trUPField5.Visible = false;


                // amit added 23-6-2025
                DdlProject.SelectedIndex = 0;
                // added by amit end
            }
            Int32 ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
            bindEducational();
            bindExamName(ApplicantTypeId);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
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
    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GenerateNewCaptchaImage();
            txtcode.Text = "";
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }
    protected void DDLeducode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            HfExperience.Value = "0";
            lblExperienceInYears.Text = "";
            int courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
            using (EConnectContext context = new EConnectContext())
            {
                Int32 qualificationID = Convert.ToInt32(DDLeducode.SelectedValue);
                Int32 ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
                //var experience = context.QualificationEligibility.Where(s => s.CourseID == courseID && s.ApplicantTypeID == ApplicantTypeId && s.EffectiveDateFrom <= DateTime.Now).FirstOrDefault();
                EducationalQualification qual = (from c in context.EducationalQualifications
                                                 where c.ID == qualificationID
                                                 select c).FirstOrDefault();
                if (qual != null)
                {
                    decimal experience = (from k in context.QualificationEligibility
                                          where k.CourseID == courseID && k.ApplicantTypeID == ApplicantTypeId && k.QualificationLevelID == qual.QualificationLevelID
                                          orderby k.EffectiveDateFrom descending
                                          select new { k.Experience, k.EffectiveDateFrom }).Select(k => k.Experience).FirstOrDefault();
                    if (experience != null)
                    {
                        HfExperience.Value = experience > 0 ? experience.ToString() : "0";
                        if (HfExperience.Value != "0")
                        {
                            TrExperience.Visible = true;
                            lblExperienceInYears.Visible = true;
                            lblExperienceInYears.Text = "<font color='red'>* </font>(Minimum " + HfExperience.Value + " year(s) of experience required)";
                        }
                    }
                    else
                    {
                        HfExperience.Value = "0";
                    }
                }

            }
            ;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlSalutaionName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSalutaionName.SelectedValue != "0")
            {
                if (ddlSalutaionName.SelectedValue.Trim().ToUpper() == "MS.")
                {
                    ddl_gender.SelectedValue = "Female";
                    ddl_gender.Enabled = false;
                }
                else if (ddlSalutaionName.SelectedValue == "Mr.")
                {
                    ddl_gender.SelectedValue = "Male";
                    ddl_gender.Enabled = false;
                }
                //Added for transgender
                else if (ddlSalutaionName.SelectedValue == "Others")
                {
                    ddl_gender.SelectedValue = "Trans";
                    ddl_gender.Enabled = false;
                }
            }

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }
    protected void Rdoownertype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Rdoownertype.SelectedValue == "P")//Parents
            {
                trfather.Visible = true;
                trmother.Visible = true;
                trguardian.Visible = false;
                //Added 22 Feb 2019
                trAffidavit.Visible = false;
                trAffidavitNo.Visible = false;
                trAffidavitDate.Visible = false;
                divgurdian.Style.Add("display", "none");
                //

                trfather.Attributes.Add("class", "gdrow1");
                trmother.Attributes.Add("class", "gdalternate1");
                trgender.Attributes.Add("class", "gdrow1");
                trdob.Attributes.Add("class", "trgdalternate1calendar");
                trmaritalstatus.Attributes.Add("class", "gdrow1");
                trcategory.Attributes.Add("class", "gdalternate1");
                trhandicapped.Attributes.Add("class", "gdrow1");
                trexserviceman.Attributes.Add("class", "gdalternate1");
                trreligion.Attributes.Add("class", "gdrow1");
                txtDob.Text = "";
            }
            if (Rdoownertype.SelectedValue == "G")//Guardian
            {
                trguardian.Visible = true;
                //Added 22 Feb 2019
                trAffidavit.Visible = true;
                trAffidavitNo.Visible = true;
                trAffidavitDate.Visible = true;
                divgurdian.Style.Add("display", "block");
                //

                trfather.Visible = false;
                trmother.Visible = false;
                trgender.Attributes.Add("class", "gdalternate1");
                trdob.Attributes.Add("class", "trgdrow1calendar");
                trmaritalstatus.Attributes.Add("class", "gdalternate1");
                trcategory.Attributes.Add("class", "gdrow1");
                trhandicapped.Attributes.Add("class", "gdalternate1");
                trexserviceman.Attributes.Add("class", "gdrow1");
                trreligion.Attributes.Add("class", "gdalternate1");
                txtDob.Text = "";
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }
    }
    protected void txtapaar_TextChanged(object sender, EventArgs e)
    {
        txtDob_TextChanged(sender, e);
    }
    protected void txtDob_TextChanged(object sender, EventArgs e)
    {
        DateTime todaydate = DateTime.Now;
        DateTime Inputdate = Convert.ToDateTime(txtDob.Text);
        int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);
        string applicantType = RdoUndergngDOEACC.SelectedValue.ToString();
        if (courseType < 5)
            bindDeclaration(applicantType, countAge);

        //added for Apaar Api validation check ashutosh start

        txtAuthenticationIdNo.Text = "";
        //ddlAuthMode.SelectedIndex = 0;
        ddlConsentRelation.SelectedIndex = 0;
        txtproviderName.Text = "";
        txtConsentDate.Text = "";
        txtConsentTime.Text = "";
        txtConsentPlace.Text = "";


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
        txtConsentDate.ReadOnly = true;

        txtConsentTime.Text = DateTime.Now.ToString("HH:mm");
        txtConsentTime.ReadOnly = true;

        txtConsentPlace.Text = txtConsentPlace.Text.Trim();
        //added for Apaar Api validation check ashutosh end 
        BindApaarDeclaration(countAge);

    }
    protected void DdlAccCentre_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DdlAccCentre.SelectedValue != "0")
            AlertModalPopUp.Show();
        if (DdlAccCentre.SelectedValue == "10009177")
        {
            Session["isBSB"] = "1";
            bindEducational();
            trBSB.Visible = true;
        }
    }


    //Added
    protected void btnOK_Click(object sender, EventArgs e)
    {
        ActiveInActive(true);
        divgurdian.Style.Add("display", "none");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ActiveInActive(true);
        divgurdian.Style.Add("display", "none");
        Rdoownertype.ClearSelection();
        Rdoownertype.SelectedValue = "P";
        Rdoownertype_SelectedIndexChanged(Rdoownertype.SelectedValue, EventArgs.Empty);
    }

    protected void ActiveInActive(Boolean isactive)
    {
        txtAppName.Enabled = isactive;
        txtFatherName.Enabled = isactive;
        txtMotherName.Enabled = isactive;
        TxtGuardianName.Enabled = isactive;
        //Added 22 Mar 2019
        txtAffidavitNo.Enabled = isactive;
        txtAffidavitDate.Enabled = isactive;
        fileAffidavit.Enabled = isactive;
        //
        TxtPpincode.Enabled = isactive;
        TxtSTDcode.Enabled = isactive;

        TxtPerAddressLine1.Enabled = isactive;
        TxtPerAddressLine2.Enabled = isactive;
        TxtPerAddressLine3.Enabled = isactive;
        TxtPerCity.Enabled = isactive;
        txtcode.Enabled = isactive;
        txtCorPhoneNo.Enabled = isactive;
        txtDob.Enabled = isactive;
        txtEmailId.Enabled = isactive;
        txtCorMobileNo.Enabled = isactive;

        DdlAccCentre.Enabled = isactive;
        DdlAccState.Enabled = isactive;

        ddlCategory.Enabled = isactive;
        ddlCorState.Enabled = isactive;
        ddlPdistrict.Enabled = isactive;
        DDLeducode.Enabled = isactive;
        ddlSalutaionName.Enabled = isactive;
        chkdisclamier.Enabled = isactive;
        chkApaarDeclaration.Enabled = isactive;

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
    protected void RadioButtonListMIS_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (RadioButtonListMIS.SelectedValue == "Y") // If Appeared is Yes
            {
                TRMIS.Visible = true;

                TblFormDetail.Visible = false;
            }
            if (RadioButtonListMIS.SelectedValue == "N")//If Appeared is No
            {
                TRMIS.Visible = false;
                TblFormDetail.Visible = true;

                Response.Redirect("~/CAND/NielitRegistration.aspx?" + Request.QueryString.ToString());
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
                txtMIS.Text = "";
                lblerror.Text = "Please Enter Reference Number.";
                //TrPreExam.Visible = false;
            }
            else
            {
                TblFormDetail.Visible = true;
                BindMISState();
                ResetAll();
                //int courseID = Convert.ToInt32(Request.QueryString["id"]);
                string Number = txtMIS.Text.Trim();
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    var application = context.NielitCentreStudent.Where(p => p.Number == Number).FirstOrDefault();

                    //if (application.enmApplicantType == enmApplicantType.Institute)
                    //{
                    //    var instituteDetail = (from a in context.AccreditationDetails
                    //                           join i in context.Institutes
                    //                               on a.InstituteID equals i.ID
                    //                           where i.ID == application.InstituteID
                    //                           select new
                    //                           {
                    //                               Name = i.Name,
                    //                               CentreID = i.ID,
                    //                               StateId = i.StateID,
                    //                               DistrictId = i.DistrictID,
                    //                               StateName = i.State.Name,
                    //                               DistrictName = i.District.Name,
                    //                               AccNo = a.AccreditationNumber
                    //                           }).FirstOrDefault();

                    //RdoUndergngDOEACC.SelectedValue = "I";


                    //    DdlAccState.SelectedValue = Convert.ToString(instituteDetail.StateId);
                    //    int id = Convert.ToInt32(DdlAccState.SelectedValue);
                    //    DdlAccState_SelectedIndexChanged(DdlAccState, EventArgs.Empty);
                    //    DdlAccCentre.SelectedValue = Convert.ToString(instituteDetail.CentreID);
                    //}
                    //else
                    //{
                    //    RdoUndergngDOEACC.SelectedValue = "D";
                    //    TxtExperienceInYears.Text = application.ExperienceInYears.ToString();
                    //}
                    //ddlPaymentOption.SelectedValue = application.PaymentSourceID.HasValue ? application.PaymentSourceID.Value.ToString() : (RdoUndergngDOEACC.SelectedValue == "D" ? "1" : "2");
                    //Candidate Personal Detail...
                    if (application != null)
                    {
                        ddlSalutaionName.SelectedValue = application.Salutation.ToString();
                        txtAppName.Text = GetInitCap(application.Name.ToString());
                        lblname.Text = txtAppName.Text;
                        if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName) == true)
                        {
                            trfather.Visible = true;
                            trmother.Visible = true;
                            trguardian.Visible = false;
                            Rdoownertype.SelectedValue = "P";
                            Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                            txtFatherName.Text = string.IsNullOrEmpty(application.FatherName) == false && !string.IsNullOrWhiteSpace(application.FatherName) ? GetInitCap(application.FatherName) : "";
                            txtMotherName.Text = string.IsNullOrEmpty(application.MotherName) == false && !string.IsNullOrWhiteSpace(application.MotherName) ? GetInitCap(application.MotherName) : "";
                        }
                        else
                        {
                            trfather.Visible = false;
                            trmother.Visible = false;
                            trguardian.Visible = true;
                            Rdoownertype.SelectedValue = "G";
                            Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                            TxtGuardianName.Text = GetInitCap(application.GuardianName);
                            //Added 22 Feb 2019
                            //txtAffidavitNo.Text = application.affidavitNo.ToString();
                            //txtAffidavitDate.Text = application.affidavitDate.GetValueOrDefault().ToString("dd-MMM-yyyy");
                            //// fileAffidavit.FileName = application.affidavitFile;
                            //lblAffidavitFile.Text = application.affidavitFile;


                        }

                        ddl_gender.SelectedValue = application.Gender;
                        txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");
                        txtBodyMark.Text = string.IsNullOrEmpty(application.BodyMark) == false && !string.IsNullOrWhiteSpace(application.BodyMark) ? application.BodyMark : "";
                        ddlCategory.SelectedValue = application.CastCategoryID.ToString();
                        ddlMStatus.SelectedValue = application.MaritalStatusID.ToString();
                        if (application.IsHandicaped.ToString() == "True")
                        {
                            Rdhandicapped.SelectedValue = "Y";
                        }
                        else
                        {
                            Rdhandicapped.SelectedValue = "N";
                        }
                        if (application.IsExServicemane.ToString() == "True")
                        {
                            Rdexserviceman.SelectedValue = "Y";
                        }
                        else
                        {
                            Rdexserviceman.SelectedValue = "N";
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
                        ddlReligion.SelectedValue = application.ReligionID.ToString();
                        //Candidate Contact Detail...

                        TxtSTDcode.Text = application.PhoneNumber.HasValue && application.PhoneNumber != 0 ? "0" + application.StdNumber.ToString() : "";
                        txtCorPhoneNo.Text = application.PhoneNumber.ToString();
                        txtCorMobileNo.Text = application.MobileNumber.ToString();
                        txtEmailId.Text = application.EmailAddress.ToString();


                        // added_by_amit_audit_april_2026_start
                        if (application.AadharNumber.HasValue)
                        {
                            txtaadhar.Text = "XXXXXXXX" + application.AadharNumber.Value.ToString().Substring(8);
                            string encryptedAadhaar = EncryptDecrypt.EncryptString(application.AadharNumber.Value.ToString());
                            hfaadhaar.Value = encryptedAadhaar;
                        }
                        else
                            txtaadhar.Text = "";
                        // added_by_amit_audit_april_2026_end


                        //Candidate Address Detail...
                        TxtPerAddressLine1.Text = string.IsNullOrEmpty(application.PerAddressLine1) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine1) ? GetInitCap(application.PerAddressLine1) : "";
                        TxtPerAddressLine2.Text = string.IsNullOrEmpty(application.PerAddressLine2) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine2) ? GetInitCap(application.PerAddressLine2) : "";
                        TxtPerAddressLine3.Text = string.IsNullOrEmpty(application.PerAddressLine3) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine3) ? GetInitCap(application.PerAddressLine3) : "";
                        TxtPerCity.Text = string.IsNullOrEmpty(application.PerCityName) == false && !string.IsNullOrWhiteSpace(application.PerCityName) ? GetInitCap(application.PerCityName) : "";
                        ddlPState.SelectedValue = application.PerStateID != null && application.PerStateID != 0 ? application.PerStateID.ToString() : "0";
                        int id1 = Convert.ToInt32(ddlPState.SelectedValue);
                        ddlPState_SelectedIndexChanged(ddlPState, EventArgs.Empty);
                        ddlPdistrict.SelectedValue = application.PerDistrictID.ToString();
                        TxtPpincode.Text = application.PerPinCode.ToString();

                        TxtCorAddressLine1.Text = string.IsNullOrEmpty(application.CorAddressLine1) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine1) ? GetInitCap(application.CorAddressLine1) : "";
                        TxtCorAddressLine2.Text = string.IsNullOrEmpty(application.CorAddressLine2) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine2) ? GetInitCap(application.CorAddressLine2) : "";
                        TxtCorAddressLine3.Text = string.IsNullOrEmpty(application.CorAddressLine3) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine3) ? GetInitCap(application.CorAddressLine3) : "";
                        TxtCorCity.Text = string.IsNullOrEmpty(application.CorCityName) == false && !string.IsNullOrWhiteSpace(application.CorCityName) ? GetInitCap(application.CorCityName) : "";
                        ddlCorState.SelectedValue = application.CorStateID.ToString();
                        int id2 = Convert.ToInt32(ddlCorState.SelectedValue);
                        ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                        ddldistrict.SelectedValue = application.CorDistrictID.ToString();
                        txtCorPinCode.Text = application.CorPinCode.ToString();

                        //DDLeducode.SelectedValue = application.EducationalQualification.ID.ToString();
                        //TxtYearOfPassing2.Text = application.PassingYear.ToString();
                        DDLeducode_SelectedIndexChanged(DDLeducode, EventArgs.Empty);
                        btnback.Visible = false;
                    }
                    else
                    {
                        RadioButtonListMIS.SelectedValue = "N";
                        lblerror.Visible = true;
                        txtMIS.Focus();
                        GenerateNewCaptchaImage();
                        txtMIS.Text = "";
                        lblerror.Text = "No details found for given roll number " + Number + ". Please check reference number and search again.";
                        ImgBtnReset.Visible = false;
                    }
                    if (RdoUndergngDOEACC.SelectedValue == "I")
                    {
                        TrLastCenterAccno.Visible = true;
                        //TrLastCenterInstiName.Visible = true;
                        TrAccCentre.Visible = true;
                        TrExperience.Visible = false;
                        //Added for NSQF Courses
                        Int64 applID = Convert.ToInt64(Request.QueryString["Appid"]);
                        //This Query will get all the information of Applied Canditate by generated Application id                                     
                        EConnectContext context1 = new EConnectContext();
                        var application1 = context1.CourseRegistrationApplications.Find(applID);
                        //Added to handle exception 16 May 2023			
                        if (application1 != null)
                        {
                            if (application1.CourseCategoryID.ToString() == "6" && Convert.ToInt32(application1.CourseID.ToString()) > 102)
                            {
                                TrExperience.Visible = true;
                                Label63.Text = "Experience in years / वर्षों का अनुभव";
                            }
                        }
                        //
                    }
                    else
                    {
                        TrExperience.Visible = true;
                        //Added for NSQF Courses
                        Label63.Text = "If direct, experience in years / यदि  डायरेक्ट, वर्षों का अनुभव";
                        TrLastCenterAccno.Visible = false;
                        //TrLastCenterInstiName.Visible = false;
                        TrAccCentre.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void ImgBtnReset_Click(object sender, ImageClickEventArgs e)
    {
        GenerateNewCaptchaImage();
        txtcode.Text = "";
        Response.Redirect("CertificateRegistration.aspx?" + Request.QueryString.ToString());
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
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlPState, corstate, new ListItem("--Select One--", "0"));
        }
    }
    protected bool checkBSB()
    {
        if (Request.QueryString["Appid"] != null)
        {
            Int64 appID = Convert.ToInt64(Request.QueryString["Appid"]);
            EConnectContext context = new EConnectContext();
            var app = context.CourseRegistrationApplications.Find(appID);
            string isBSB = app.isBSB.ToString();
            if (isBSB == "True")
                return true;
            else
                return false;

        }
        else
            return false;

    }
    string checkDuplicateApaar(string apaarEncryptedCheck, int CourseId, int ExamId)
    {
        string duplicate = "0";
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var candRecord = (from c in context.CourseRegistrationApplications
                                  where c.CourseID == CourseId && c.ApplicableExamID == ExamId
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
    //Added by Amit
    protected void radProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlPaymentOption.Enabled = true; // added_by_amit_april
        if (radProject.SelectedValue == "1")
        {
            //trBSB.Visible = true;
            TrUPBoard.Visible = true;
            //trUPField1.Visible = true;

            trUPProject.Visible = true;

            bindUPProject(Convert.ToInt32(Request.QueryString["id"]));

        }
        else
        {
            trUPProject.Visible = false;
            DdlAccState.Enabled = true;
            TrUPBoard.Visible = true;
            //TrUPBoard.Visible = false;
            //trUPField1.Visible = false;
            trBSB.Visible = false;

            trUPField2.Visible = false;

            trUPField3.Visible = false;
            trUPField4.Visible = false;
            trUPField5.Visible = false;

            txtUDISECode.Text = "";
            txtField2.Text = "";
            txtField3.Text = "";
            txtField4.Text = "";
            txtField5.Text = "";

            if (DdlAccState.SelectedValue != "0")
                DdlAccState_SelectedIndexChanged(DdlAccState, EventArgs.Empty);
            ShowFeeDetail(Convert.ToInt32(ViewState["ExamID"]));

            // amit added
            DdlAccState.SelectedIndex = 0;
            DdlProject.SelectedIndex = 0;

        }

        bindEducational();
    }
    protected void bindUPProject(int CourseID)
    {
        //Int32 CourseID = Convert.ToInt32(Request.QueryString["id"]);
        ListItem lst1 = new ListItem("--Select One--", "0");
        using (NIELITMISContext context = new NIELITMISContext())
        {
            var Proc1 = (from t in context.NielitProjectss
                         join k in context.NielitProjCoursess on t.ID equals k.projID
                         join x in context.projectMainCentres on t.ID equals x.projectID
                         where k.courseID == CourseID
                         //				&& t.ID==10023
                         //&& x.centreID == entityID
                         && k.IsActive
                         && t.projectTodate >= DateTime.Today
                         orderby (t.ProjectName)
                         select new { ValueField = t.ID, TextField = t.ProjectName })

                   .Union(from t in context.NielitProjectss
                          join k in context.NielitProjCoursess on t.ID equals k.projID
                          join x in context.projectSubCentres on t.ID equals x.projectID
                          join i in context.AffInstitutes on x.centreID equals i.ID
                          where k.courseID == CourseID
                              //&& t.ID==10023
                              //&& i.instituteID == entityID
                              && k.IsActive
                              && t.projectTodate >= DateTime.Today
                          orderby (t.ProjectName)
                          select new { ValueField = t.ID, TextField = t.ProjectName });

            //EConnect.Utils.Common.ControlUtility.BindListObject(ddlProject, Proc1, lst1);
            EConnect.Utils.Common.ControlUtility.BindListObject(DdlProject, Proc1, lst1);
        }
    }

    protected void bindUPProjectAccCentre(int stateid, int courseid)
    {
        //stateid = Convert.ToInt32(DdlAccState.SelectedValue);
        //Int32 courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
        Int32 withdrawlid = Convert.ToInt32(enmAccreditationStatus.Withdrawal);
        Int32 rejectedid = Convert.ToInt16(enmAccreditationStatus.Rejected);
        Int32 deferredid = Convert.ToInt16(enmAccreditationStatus.Deferred);
        Int32 acknowledgeid = Convert.ToInt16(enmAccreditationStatus.Acknowledged);

        using (var context = new EConnectContext())
        {
            ListItem lst = new ListItem("--Select One--", "0");
            var AccCentre = (from i in context.Institutes
                             join d in context.AccreditationDetails on i.ID equals d.InstituteID
                             orderby i.Name
                             where i.StateID == stateid
                             && d.CourseID == courseid
                             && d.AccreditationStatusID != withdrawlid
                               && d.AccreditationStatusID != rejectedid
                                 && d.AccreditationStatusID != deferredid
                                   && d.AccreditationStatusID != acknowledgeid
                             //Added 22 May 2020 for instt blocking
                             && d.tempBlocked == false
                             && (d.BlockedFromDate >= System.DateTime.Now || d.BlockedFromDate == null)
                             && d.EffectiveToDate >= System.DateTime.Now
                             && d.AccreditationNumber.Contains("UPMSP")
                             select new { ValueField = i.ID, TextField = d.AccreditationNumber + " - " + i.Name + ", " + (!string.IsNullOrEmpty(i.CityName) ? i.CityName : "") }).ToList();


            EConnect.Utils.Common.ControlUtility.BindListObject(DdlAccCentre, AccCentre.Distinct(), lst);
        }
        ;
    }
    //Added by Amit start

    protected void DdlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ToString();

            // Always reset first
            trUPField2.Visible = false;
            trUPField3.Visible = false;
            trUPField4.Visible = false;
            trUPField5.Visible = false;
            trBSB.Visible = false;

            if (DdlProject.SelectedItem.Text.Contains("UP") && DdlProject.SelectedItem.Text.Contains("MSP"))
            {
                DdlAccState.SelectedValue = "28";
                DdlAccState.Enabled = false;
                bindUPProjectAccCentre(Convert.ToInt32(DdlAccState.SelectedValue), Convert.ToInt32(Request.QueryString["id"]));

                // added by amit start
                // freezing payment option for the case of UP
                ddlPaymentOption.SelectedValue = "2";
                ddlPaymentOption.Enabled = false;
                // added by amit end
            }
            else
            {
                DdlAccState.SelectedValue = "0";
                DdlAccCentre.SelectedValue = "0";
                DdlAccState.Enabled = true;
                // added by amit start
                // freezing payment option for the case of UP
                ddlPaymentOption.SelectedValue = "2";
                ddlPaymentOption.Enabled = true;
                // added by amit end
            }

            // Only execute school-related UI if "School" is found in the text
            if (DdlProject.SelectedItem.Text.IndexOf("School", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string query = "SELECT [ID], [field1], [field2], [field3], [field4], [field5] FROM [projCriteriaMaster] WHERE projID = @projID";

                    using (SqlCommand scCommand = new SqlCommand(query, con))
                    {
                        scCommand.Parameters.Add("@projID", SqlDbType.VarChar).Value = DdlProject.SelectedValue;

                        SqlDataAdapter da = new SqlDataAdapter(scCommand);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        int hdCount = 4;

                        if (dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];

                            trBSB.Visible = !string.IsNullOrWhiteSpace(row["field1"].ToString());
                            if (trBSB.Visible)
                            {
                                Label14.Text = row["field1"].ToString() + "<span style='color:red'>*</span>";
                                txtUDISECode.Text = "";
                                //if (ddlUDISECode.Items.Count > 0)
                                //    ddlUDISECode.SelectedIndex = 0;
                            }

                            trUPField2.Visible = !string.IsNullOrWhiteSpace(row["field2"].ToString());
                            if (trUPField2.Visible)
                            {
                                lblField2.Text = row["field2"].ToString() + "<span style='color:red'>*</span>";
                                lblSrField2.Text = "1.2." + (++hdCount);
                                txtField2.Text = "";
                            }

                            trUPField3.Visible = !string.IsNullOrWhiteSpace(row["field3"].ToString());
                            if (trUPField3.Visible)
                            {
                                lblField3.Text = row["field3"].ToString() + "<span style='color:red'>*</span>";
                                lblSrField3.Text = "1.2." + (++hdCount);
                                txtField3.Text = "";
                            }

                            trUPField4.Visible = !string.IsNullOrWhiteSpace(row["field4"].ToString());
                            if (trUPField4.Visible)
                            {
                                lblField4.Text = row["field4"].ToString() + "<span style='color:red'>*</span>";
                                lblSrField4.Text = "1.2." + (++hdCount);
                                txtField4.Text = "";
                            }

                            trUPField5.Visible = !string.IsNullOrWhiteSpace(row["field5"].ToString());
                            if (trUPField5.Visible)
                            {
                                lblField5.Text = row["field5"].ToString() + "<span style='color:red'>*</span>";
                                lblSrField5.Text = "1.2." + (++hdCount);
                                txtField5.Text = "";
                            }
                        }
                    }
                }

            }
            else
            {
                // Explicitly hide all fields again if not "School"
                trUPField2.Visible = false;
                trUPField3.Visible = false;
                trUPField4.Visible = false;
                trUPField5.Visible = false;
                trBSB.Visible = false;
            }

            // it is called so that educational qualifications are in accordance with schools, added by amit
            bindEducational();

            if (DdlProject.SelectedValue != "0")
                getFeeDetailForProjectCourse();

            if (DdlProject.SelectedValue == "0")
                ShowFeeDetail(Convert.ToInt32(ViewState["ExamID"]));

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected bool checkAccrforCand()
    {
        try
        {
            string project_id = DdlProject.SelectedValue.Trim();
            string acccentercode = DdlAccCentre.SelectedItem.Text.Trim().Split(' ')[0];

            // UPMSP case
            if (project_id == "10023")
            {
                string udiseVal = txtUDISECode.Text;


                if ("UPMSP-" + udiseVal != acccentercode)
                {
                    ShowAlert("UDISE Code is Invalid.");
                    return false;
                }

                return true;

            }
            else
            {


                string query2 = @"select um.USER_REF_NO from nielit.dbo.URM_USER_MASTER um
                                 inner join nielit.dbo.projStudentsMaster pm on um.USER_NO = pm.enterBy
                                 where pm.field5 = @apaar_id";

                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ToString()))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    using (var cmd2 = new SqlCommand(query2, connection))
                    {
                        cmd2.Parameters.AddWithValue("@apaar_id",
                            string.IsNullOrWhiteSpace(txtapaar.Text) ? (object)DBNull.Value : txtapaar.Text.Trim());

                        object result = cmd2.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            string inst_id = result.ToString().Trim();

                            if (inst_id == DdlAccCentre.SelectedValue)
                            {
                                return true;
                            }
                            else
                            {
                                ShowAlert("Accreditation Number not matched with uploaded record");
                                return false;
                            }
                        }
                        else
                        {
                            ShowAlert("No Accreditation record found for the given candidate");
                            return false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

            ShowAlert("Error occurred while checking Accreditation");
            return false;
        }
    }


    /*Commented for ApaarID protected int validateDataFromUploadedExcel()
     {
         try
         {
             int status = 0;
             using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ToString()))
             {
                 if (connection.State == ConnectionState.Closed)
                 {
                     connection.Open();
                 }


                 using (var command = new SqlCommand("validateDataFromUploadedExcelData", connection))
                 {
                     command.CommandType = CommandType.StoredProcedure;
                     command.Parameters.Clear();

                     //string udiseVal = Session["isBSB"].ToString() == "1"
                     //    ? ddlUDISECode.SelectedValue
                     //    : txtUDISECode.Text;

                     string udiseVal = txtUDISECode.Text;


                     string acccentercode = DdlAccCentre.SelectedItem.Text.Trim().Split(' ')[0];
                     //bool projIdVal = DdlProject.SelectedItem.Text.Contains("UP");
                     int projid = Convert.ToInt32(DdlProject.SelectedValue);
                     string accCode = string.IsNullOrWhiteSpace(txtField5.Text) ? acccentercode : txtField5.Text.Trim();

                     command.Parameters.AddWithValue("@projID", projid);
                     command.Parameters.AddWithValue("@field1", string.IsNullOrWhiteSpace(udiseVal) ? (object)DBNull.Value : udiseVal.Trim());
                     command.Parameters.AddWithValue("@field2", string.IsNullOrWhiteSpace(txtField2.Text) ? (object)DBNull.Value : txtField2.Text.Trim());
                     command.Parameters.AddWithValue("@field3", string.IsNullOrWhiteSpace(txtField3.Text) ? (object)DBNull.Value : txtField3.Text.Trim());
                     command.Parameters.AddWithValue("@field5", string.IsNullOrWhiteSpace(accCode) ? (object)DBNull.Value : accCode);

                     if (projid == 10023 || projid==10025)
                     {
                         // Special logic for project 10023
                         string fatherName = string.IsNullOrWhiteSpace(txtField4.Text) ? txtFatherName.Text.Trim() : txtField4.Text.Trim();

                         if (Rdoownertype.SelectedValue == "G")
                             command.Parameters.AddWithValue("@field4", "-99");
                         else
                             command.Parameters.AddWithValue("@field4", string.IsNullOrWhiteSpace(fatherName) ? (object)DBNull.Value : fatherName);
                         //command.Parameters.AddWithValue("@field5", string.IsNullOrWhiteSpace(accCode) ? (object)DBNull.Value : accCode);
                     }
                     else
                     {
                         // General case

                         command.Parameters.AddWithValue("@field4", string.IsNullOrWhiteSpace(txtField4.Text) ? (object)DBNull.Value : txtField4.Text.Trim());
                         //command.Parameters.AddWithValue("@field5", string.IsNullOrWhiteSpace(txtField5.Text) ? (object)DBNull.Value : txtField5.Text.Trim());
                     }


                     command.Parameters.AddWithValue("@name", txtAppName.Text.Trim());
                     command.Parameters.AddWithValue("@dob", Convert.ToDateTime(txtDob.Text));
                     command.Parameters.Add("@status", SqlDbType.Int).Direction = ParameterDirection.Output;

                     command.ExecuteNonQuery();
                     status = Convert.ToInt32(command.Parameters["@status"].Value);
                 }

                 connection.Close();
             }

             return status;
         }
         catch (Exception e)
         {
             ShowAlert("Exception Occurred: " + e.Message);
             return 0;
         }
     }*/
    protected int validateDataFromUploadedExcel()
    {
        try
        {

            string udiseVal = txtUDISECode.Text;
            string acccentercode = DdlAccCentre.SelectedItem.Text.Trim().Split(' ')[0];

            if (!checkAccrforCand())
            {
                return 0;
            }
            /*  if("UPMSP-"+udiseVal != acccentercode)
              {
                  ShowAlert("UDISE Code is Invalid.");
                  return 0;
              }*/


            int status = 0;
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ToString()))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }


                using (var command = new SqlCommand("validateDataFromUploadedExcelData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();

                    //string udiseVal = Session["isBSB"].ToString() == "1"
                    //    ? ddlUDISECode.SelectedValue
                    //    : txtUDISECode.Text;


                    //bool projIdVal = DdlProject.SelectedItem.Text.Contains("UP");
                    int projid = Convert.ToInt32(DdlProject.SelectedValue);
                    string accCode = string.IsNullOrWhiteSpace(txtField5.Text) ? acccentercode : txtField5.Text.Trim();

                    command.Parameters.AddWithValue("@projID", projid);
                    command.Parameters.AddWithValue("@field1", string.IsNullOrWhiteSpace(udiseVal) ? (object)DBNull.Value : udiseVal.Trim());
                    command.Parameters.AddWithValue("@field2", string.IsNullOrWhiteSpace(txtField2.Text) ? (object)DBNull.Value : txtField2.Text.Trim());
                    command.Parameters.AddWithValue("@field3", string.IsNullOrWhiteSpace(txtField3.Text) ? (object)DBNull.Value : txtField3.Text.Trim());
                    //command.Parameters.AddWithValue("@field5", string.IsNullOrWhiteSpace(accCode) ? (object)DBNull.Value : accCode);
                    command.Parameters.AddWithValue("@field5", string.IsNullOrWhiteSpace(txtapaar.Text) ? (object)DBNull.Value : txtapaar.Text.Trim());


                    //Removed because student validation will be done through APAAR code
                    //if (projid == 36)
                    //{
                    //    // Special logic for project 36
                    //    string fatherName = string.IsNullOrWhiteSpace(txtField4.Text) ? txtFatherName.Text.Trim() : txtField4.Text.Trim();

                    //    if (Rdoownertype.SelectedValue == "G")
                    //        command.Parameters.AddWithValue("@field4", "-99");
                    //    else
                    //        command.Parameters.AddWithValue("@field4", string.IsNullOrWhiteSpace(fatherName) ? (object)DBNull.Value : fatherName);
                    //    //command.Parameters.AddWithValue("@field5", string.IsNullOrWhiteSpace(accCode) ? (object)DBNull.Value : accCode);
                    //}
                    //else
                    //{
                    //    // General case

                    //    command.Parameters.AddWithValue("@field4", string.IsNullOrWhiteSpace(txtField4.Text) ? (object)DBNull.Value : txtField4.Text.Trim());
                    //    //command.Parameters.AddWithValue("@field5", string.IsNullOrWhiteSpace(txtField5.Text) ? (object)DBNull.Value : txtField5.Text.Trim());
                    //}


                    //command.Parameters.AddWithValue("@name", txtAppName.Text.Trim());
                    //command.Parameters.AddWithValue("@dob", Convert.ToDateTime(txtDob.Text));
                    command.Parameters.Add("@status", SqlDbType.Int).Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();
                    status = Convert.ToInt32(command.Parameters["@status"].Value);
                }

                connection.Close();
            }

            return status;
        }
        catch (Exception e)
        {
            ShowAlert("Error Occurred during Validation. Kindly contact NIELIT HO");
            return 0;
        }
    }

    protected void getFeeDetailForProjectCourse()
    {
        int courseID = Convert.ToInt32(Request.QueryString["id"]);
        //ViewState["ExamID"] = examID.ToString();
        Int32 examID = Convert.ToInt32(ViewState["ExamID"] != null ? ViewState["ExamID"].ToString() : "0");
        Int32 projectId = Convert.ToInt32(DdlProject.SelectedValue);                                                   //Added_30_12_2024
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
                int FeeTypeID = Convert.ToInt32(enmFeeType.RegistrationFee);
                LblFeeTypeName.Text = Convert.ToString(enmFeeType.RegistrationFee);
                int ExamId = examID;
                Int32 feeAmount = 0;

                //feeAmount = (from f in context.FeeDetails
                //             where f.CourseID == courseID
                //             && f.FeeTypeID == FeeTypeID
                //             && f.projectid == projectId     //Added_30_12_2024
                //             && f.EffectiveFromDate == (from c in context.FeeDetails
                //                                        where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                //                                          && f.projectid == projectId     //Added_30_12_2024
                //                                        select c.EffectiveFromDate).Max()
                //             select new { FeeAmount = f.FeeAmount }).FirstOrDefault().FeeAmount;

                // working fees code
                //var dfeeAmount = (from f in context.FeeDetails
                //                  where f.CourseID == courseID
                //                  && f.FeeTypeID == FeeTypeID
                //                  && f.projectid == projectId     //Added_30_12_2024
                //                  && f.EffectiveFromDate == (from c in context.FeeDetails
                //                                             where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                //                                               && c.projectid == projectId     //Added_30_12_2024
                //                                             select c.EffectiveFromDate).Max()
                //                  select new { FeeAmount = f.FeeAmount }).FirstOrDefault();

                var dfeeAmount = (from f in context.FeeDetails
                                  where f.CourseID == courseID
                                  && f.FeeTypeID == FeeTypeID
                                  && f.projectid == projectId     //Added_30_12_2024
                                                                  //&& f.EffectiveFromDate <= DateTime.Now
                                  && (f.EffectiveToDate == null || f.EffectiveToDate >= System.Data.Entity.DbFunctions.CreateDateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0))
                                  && f.EffectiveFromDate == (from c in context.FeeDetails
                                                             where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                                                               && c.projectid == projectId
                                                             //Added_30_12_2024
                                                             select c.EffectiveFromDate).Max()

                                  select new { FeeAmount = f.FeeAmount }).FirstOrDefault();

                //Added_30_12_2024_Start

                if (dfeeAmount == null)
                {
                    //feeAmount = Convert.ToInt32(ViewState["defaultFeeOLevel"]);
                    //lblFeeDetail.Text = ViewState["defaultFeeOLevel"].ToString();


                    feeAmount = Convert.ToInt32(ViewState["defaultFeeAmount"]);
                    //Int32 app_type_id = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
                    //bindExamName(app_type_id);


                }
                else
                {
                    feeAmount = Convert.ToInt32(dfeeAmount.FeeAmount);
                }

                //Added_30_12_2024_End
                //5
                int LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee);
                //4
                int NormalFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
                //2
                int ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
                //19
                int LateFeeTypeId = Convert.ToInt32(enmFeeType.LateFeeRegistration);

                var Fee = (from f in context.CutOffDates
                           where f.CourseID == courseID
                           && f.ExamID == ExamId
                           && f.ApplicantTypeID == ApplicantTypeId
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
                                                && f.projectid == projectId     //Added_30_12_2024
                                               && f.EffectiveFromDate == (from c in context.FeeDetails
                                                                          where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                                                                          && f.projectid == projectId     //Added_30_12_2024
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
                    }
                }
                else
                {
                    LblNormalFee.Text = feeAmount.ToString("F");
                    LblTotalFee.Text = (feeAmount + cscProcessingFee).ToString("F");
                    lblProcessingFee.Text = cscProcessingFee.ToString("F");
                    lblFeeDetail.Text = "Fee : Rs/- " + (feeAmount + cscProcessingFee) + ".00";
                    if (course.CourseCategoryID == 6) //STC
                    {
                        lblFeeDetail.Text = "Fee will be paid to concerned regional center.";
                        ImgBtnPopupFee.Enabled = false;
                    }
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
    //Added by amit end

    //  added_by_amit_audit_april_2026_start image upload code
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
                if (String.IsNullOrEmpty(txtDob.Text)) throw new Exception("Please fill Application Details First.");
                //16.02.2022
                if (ImgUpload.FileName.Length > 19)
                {
                    lblphotoShow.Visible = true;
                    lblerror.Visible = true;
                    photoPreview.ImageUrl = "";
                    lblphotoShow.Text = "Photo file name should be less than 14 characters.";
                    lblerror.Text = "Photo file name should be less than 14 characters.";
                    if (File.Exists(fileNamePhoto))
                    { File.Delete(fileNamePhoto); }
                    ImgUpload.PostedFile.InputStream.Dispose();
                    Session["ImgUpload"] = null;
                    throw new Exception("Photo file name should be less than 14 characters.");
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

                string requestURL = "https://uat.nielit.in/nielitImageApi/v1/index.php/ImageValidate";  // live URL of API
                // string requestURL = "http://14.139.53.83:8088/nielitImageApi/v1/ImageValidate";  // local URL of API
                //string requestURL = "https://student.nielit.gov.in/nielitImageApi/v1/index.php/ImageValidate";  // live URL of API

                string folderPath = Server.MapPath("~/ImageUpload/");
                if (!Directory.Exists(folderPath))
                { Directory.CreateDirectory(folderPath); }
                string ImgNamePhoto = string.Empty;
                ImgNamePhoto = "P" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUpload.FileName);          //Photo_
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
                    ImgNamePhoto1 = "P" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUpload.FileName);          //Photo_
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
                if (String.IsNullOrEmpty(txtDob.Text)) throw new Exception("Please fill Application Details First.");
                //16.02.2022
                if (ImgUploadSignature.FileName.Length > 19)
                {
                    lblsignShow.Visible = true;
                    lblerror.Visible = true;
                    signPreview.ImageUrl = "";
                    lblsignShow.Text = "Signature file name should be less than 14 characters.";
                    lblerror.Text = "Signature file name should be less than 14 characters.";
                    if (File.Exists(fileName))
                    { File.Delete(fileName); }
                    ImgUploadSignature.PostedFile.InputStream.Dispose();
                    Session["ImgUploadSignature"] = null;
                    throw new Exception("Signature file name should be less than 14 characters.");
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
                // live URL of API
                string requestURL = "https://uat.nielit.in/nielitImageApi/v1/index.php/ThumbSignatureValidate";  // live URL of API
                //string requestURL = "http://14.139.53.83:8088/nielitImageApi/v1/ThumbSignatureValidate";  // local URL of API
                //string requestURL = "https://student.nielit.gov.in/nielitImageApi/v1/index.php/ThumbSignatureValidate";  // live URL of API
                string folderPath = Server.MapPath("~/ImageUpload/");
                if (!Directory.Exists(folderPath))
                { Directory.CreateDirectory(folderPath); }
                string ImgNameSig = string.Empty;
                ImgNameSig = "S" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadSignature.FileName);         //Sig_
                ImgUploadSignature.SaveAs(folderPath + "/" + ImgNameSig);
                 ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
                    ImgNamesig1 = "S" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadSignature.FileName);         //Sig_
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
                if (ImgUploadThumb.FileName.Length > 19)
                {
                    if (String.IsNullOrEmpty(txtDob.Text)) throw new Exception("Please fill Application Details First.");
                    lblthumbshow.Visible = true;
                    lblerror.Visible = true;
                    thumbPreview.ImageUrl = "";
                    lblthumbshow.Text = "Thumb Impression file name should be less than 14 characters.";
                    lblerror.Text = "Thumb Impression file name should be less than 14 characters.";
                    if (File.Exists(fileName))
                    { File.Delete(fileName); }
                    ImgUploadThumb.PostedFile.InputStream.Dispose();
                    Session["ImgUploadThumb"] = null;
                    throw new Exception("Thumb Impression file name should be less than 14 characters.");
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

                string requestURL = "https://uat.nielit.in/nielitImageApi/v1/index.php/ThumbSignatureValidate";  // live URL of API
                // string requestURL = "http://14.139.53.83:8088/nielitImageApi/v1/ThumbSignatureValidate";  // local URL of API
               // string requestURL = "https://student.nielit.gov.in/nielitImageApi/v1/index.php/ThumbSignatureValidate";  // live URL of API
                string folderPath = Server.MapPath("~/ImageUpload/");
                if (!Directory.Exists(folderPath))
                { Directory.CreateDirectory(folderPath); }
                string ImgNameLth = string.Empty;
                ImgNameLth = "L" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadThumb.FileName);         //Lth_
                ImgUploadThumb.SaveAs(folderPath + "/" + ImgNameLth);
                fileName = Server.MapPath("~/ImageUpload/" + ImgNameLth);
                 ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
                    ImgNameLth1 = "L" + UploadRandomNumber + Convert.ToDateTime(txtDob.Text).ToString("yyyyMMdd") + Path.GetFileName(ImgUploadThumb.FileName);         //Lth_
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
    protected void btnOKD_Click(object sender, EventArgs e)
    {
        //ActiveInActive(true);
        divPopup.Style.Add("display", "none");
    }


    //----------------Add by vishal------------------------------------/
    #endregion Images Validation through API on Upload Button

    //  added_by_amit_audit_april_2026_end


    //  added_by_ashutosh_may_2026_start
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
            DateTime Inputdate = Convert.ToDateTime(txtDob.Text);
            int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);
            BindApaarDeclaration(countAge);
        }
        catch (Exception ex)
        {
            throw ex;
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
                                  + txtAppName.Text +
                                  " (" + wardText + ") with APAAR ID as "
                                  + txtapaar.Text +
                                  " for validation of personal details.<br/><br/>"

                                  + "I understand that the APAAR ID may be used and shared only for limited, authorized purposes, "
                                  + "and that the information provided by me shall be kept confidential.<br/><br/>"

                                  + "The information w.r.t authentication document no. "
                                  + authDoc +
                                  " provided by me, is correct and valid to the best of my knowledge.";
    }
    //  added_by_ashutosh_may_2026_end
    //added by ashutosh start
    protected void txtAppName_TextChanged(object sender, EventArgs e)
    {
        txtapaar.Text = "";
        txtAuthenticationIdNo.Text = "";

    }

    protected void ddlConsentRelation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
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
            throw ex;
        }
    }
    //added by ashutosh end
}