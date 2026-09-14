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
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.DynamicData;
using System.Windows.Forms;

public partial class NielitRegistration : BasePage
{
    Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
    int stateid;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            Response.CacheControl = "no-cache";
            Response.AddHeader("Progra", "no-cache");
            Response.Expires = -1500;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(1);
            lblerror.Visible = false;
            // if (Request.UrlReferrer == null)
            //commented for Testing
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
                //Added for BSB
                string pageRef = Request.UrlReferrer.ToString().ToLower();

                if (!pageRef.Contains("rulesforonlineregistrationolevel"))
                {
                    Session["isBSB"] = false;
                }


                // Added by Amit start
                // Only bind dropdown on first load if BSB is active
                if (Session["isBSB"].ToString() == "1")
                {
                    BindBSBUDISECodes();
                }


                // Visibility logic must always run (every time)
                if (Session["isBSB"].ToString() == "1")
                {

                    ddlUDISECode.Visible = true;
                }
                else
                {
                    txtUDISECode.Visible = true;
                    // ddlUDISECode.Visible = false;
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
            //base.ReWriteAction(this.Form);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }


    // Added by Amit start
    protected void BindBSBUDISECodes()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var schools = from s in context.BSBSchools
                              orderby (s.ID)
                              select new { ValueField = s.UDISECode, TextField = "(" + s.UDISECode + ") " + s.SchoolName };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlUDISECode, schools, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    // Added by Amit end


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
                //Int32 examID = 9296; // Added for  testing
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
            };
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


                        // part for BSB 
                    }
                    else
                        trBSB.Visible = false;

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
                        // to fiill centers for the selected state
                        DdlAccState_SelectedIndexChanged(DdlAccState, EventArgs.Empty);
                        DdlAccCentre.SelectedValue = Convert.ToString(instituteDetail.CentreID);
                        //Added for NSQF Courses
                        if (application.CourseCategoryID.ToString() == "6" && Convert.ToInt32(application.CourseID.ToString()) > 102)
                        {
                            TxtExperienceInYears.Text = application.ExperienceInYears.ToString();
                        }


                        // Added by Amit start, added bindeducational();

                        if (application.projectID != null)
                        {
                            //Added_06_01_2025_Start

                            radProject.SelectedValue = "1";

                            /*
                            if (DdlAccCentre.SelectedItem.Text.Contains("UPMSP"))
                            {
                                DdlAccState.Enabled = false;                                                                                               
                                bindUPProjectAccCentre(Convert.ToInt32(DdlAccState.SelectedValue), application.CourseID);
           
                            }*/

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


                    //aadhar details
                    if (application.AadharNumber.HasValue)
                        txtaadhar.Text = application.AadharNumber.Value.ToString();
                    else
                        txtaadhar.Text = "";


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
                    btnback.Visible = false;
                };
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
    protected void UpdateData()
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
                        // Added by amit start
                        if (string.IsNullOrEmpty(txtUDISECode.Text) || txtUDISECode.Text.Trim() == "")
                        {
                            ShowAlert("Please enter  School Code.");
                            return;
                        }
                        //Added by amit end
                        application.isBSB = true;
                        application.BSB_U_DISECode = txtUDISECode.Text;

                    }
                    else
                    {
                        application.isBSB = false;
                        application.BSB_U_DISECode = null;
                    }

                    application.CourseCategoryID = currentCourse.CourseCategoryID;
                    //application.apaarID =currentCourse.apaarID ;
                    application.CourseID = currentCourse.ID;
                    application.ApplicationDate = DateTime.Now;
                    application.ApplicableExamID = Convert.ToInt32(ViewState["ExamID"]);

                    //through institute
                    if (RdoUndergngDOEACC.SelectedValue == "I")
                    {
                        application.ApplicantTypeID = Convert.ToInt32(enmApplicantType.Institute);
                        application.InstituteID = Convert.ToInt32(DdlAccCentre.SelectedValue);
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


                    if (!String.IsNullOrEmpty(txtaadhar.Text))
                        application.AadharNumber = Convert.ToInt64(txtaadhar.Text);
                    else
                        application.AadharNumber = null;

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

                    context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
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
                };
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void SaveData()
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
                            objRegistration.InstituteID = Convert.ToInt32(DdlAccCentre.SelectedValue);
                            //Added for NSQF Courses

                            if (CourseCatg == "6" && Convert.ToInt32(currentCourse.ID.ToString()) > 102)
                            {
                                objRegistration.ExperienceInYears = !string.IsNullOrEmpty(TxtExperienceInYears.Text) && !string.IsNullOrWhiteSpace(TxtExperienceInYears.Text) ? Convert.ToDecimal(TxtExperienceInYears.Text) : 0;
                            }
                            // Added by Amit
                            //Added-UP_Project_30_12_2024_Start
                            // whether student is from project
                            if (radProject.SelectedValue == "1")
                            {
                                if (DdlProject.SelectedValue != "0")
                                {
                                    objRegistration.projectID = Convert.ToInt32(DdlProject.SelectedValue);
                                    //if (trUPField1.Visible)
                                    //    objRegistration.field1 = txtField1.Text;
                                    if (trBSB.Visible)
                                    {
                                        // if (txtUDISECode.Visible)
                                        objRegistration.field1 = txtUDISECode.Text.Trim();
                                        // else
                                        //  objRegistration.field1 = ddlUDISECode.SelectedValue.Trim();
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
                            //Added_UP_Project_30_12_2024_End
                            // Added by Amit
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


                        ////string gender = ddl_gender.SelectedItem.Text.ToString();
                        ////gender = gender.Substring(0, gender.IndexOf('/'));
                        ////objRegistration.Gender = gender.Trim();

                        //The above 3 lines of code is OK for Male & Female but the string variable gender will pick the value Transgender (instead of Trans which is required as per the genderCode in the DB)

                        objRegistration.Gender = ddl_gender.SelectedValue;//jksah
                        string s = ddl_gender.SelectedItem.Text;
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

                        objRegistration.ReligionID = Convert.ToInt32(ddlReligion.SelectedValue);
                        objRegistration.BodyMark = txtBodyMark.Text;

                        if (ImgUpload.FileName.Length > (50 * 1024))
                        {
                            objRegistration.PhotoFileName = ImgUpload.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUpload.FileName).ToLower();
                        }
                        else
                            objRegistration.PhotoFileName = ImgUpload.FileName;
                        objRegistration.Photo = ImgUpload.FileBytes;
                        if (ImgUploadSignature.FileName.Length > (50 * 1024))
                        {
                            objRegistration.SignatureFileName = ImgUploadSignature.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUploadSignature.FileName).ToLower();
                        }
                        else
                            objRegistration.SignatureFileName = ImgUploadSignature.FileName;
                        objRegistration.Signature = ImgUploadSignature.FileBytes;
                        if (ImgUploadThumb.FileName.Length > (50 * 1024))
                        {
                            objRegistration.LeftThumbFileName = ImgUploadThumb.FileName.Substring(0, 45) + System.IO.Path.GetExtension(ImgUploadThumb.FileName).ToLower();
                        }
                        else
                            objRegistration.LeftThumbFileName = ImgUploadThumb.FileName;
                        objRegistration.LeftThumb = ImgUploadThumb.FileBytes;

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
                        if (!String.IsNullOrEmpty(txtaadhar.Text))
                            objRegistration.AadharNumber = Convert.ToInt64(txtaadhar.Text);
                        else
                            objRegistration.AadharNumber = null;
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

                        // commented out by amit for testing
                        //if (!String.IsNullOrEmpty(txtapaar.Text))
                        // {
                        //     //added on 12 Sept 2024
                        //    string apaarEncrypted = EncryptDecrypt.EncryptString(txtapaar.Text);
                        //     objRegistration.apaarID = apaarEncrypted;
                        //      //added on 12 Sept 2024
                        //  }
                        //  else
                        //	{	
                        //ShowAlert("Enter Apaar");
                        //return;
                        //}
                        // objRegistration.apaarID = null;

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
                };
            };
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
            ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateRandomNumber(6);
            EConnect.CaptchaImage captcha = new CaptchaImage(ViewState["CaptchCode"].ToString(), 200, 50, "Century Schoolbook");
            imgcap.Src = captcha.ImageSource;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
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
            int selectedInstituteId = Convert.ToInt32(DdlAccCentre.SelectedValue);
            int projId = Convert.ToInt32(DdlProject.SelectedValue);

            using (NIELITMISContext context = new NIELITMISContext()) // assuming this context maps NIELITMIS
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
            };

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
            };
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
            };
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
    /// /Changed for BSB



    // added by amit start
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
                if (Session["isBSB"] != null)
                {
                    //if (Session["isBSB"].ToString() == "1")
                    if (Session["isBSB"].ToString() == "1" || DdlProject.SelectedItem.Text.IndexOf("School", StringComparison.OrdinalIgnoreCase) >= 0)
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
                                     where p.ID == 55 || p.ID == 56 || p.ID == 57 || p.ID == 58
                                     select new { ValueField = p.ID, TextField = p.Name });


                        EConnect.Utils.Common.ControlUtility.BindListObject(DDLeducode, education, lst);
                    }
                    else
                    {
                        if (checkBSB() || DdlProject.SelectedItem.Text.IndexOf("School", StringComparison.OrdinalIgnoreCase) >= 0)
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
                    if (checkBSB() || DdlProject.SelectedItem.Text.IndexOf("School", StringComparison.OrdinalIgnoreCase) >= 0)
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
                };
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    // added by amit end

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
            };
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
            };
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
            };
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
                };
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

                    // Old Working  code 
                    //feeAmount = (from f in context.FeeDetails
                    //             where f.CourseID == courseID
                    //             && f.FeeTypeID == FeeTypeID
                    //             && f.projectid == null
                    //             && f.EffectiveFromDate <= DateTime.Now 
                    //             //&& f.EffectiveFromDate == (from c in context.FeeDetails
                    //             //                           where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                    //             //                           && c.projectid == 999
                    //             //                           select c.EffectiveFromDate).Max()
                    //             select new { FeeAmount = f.FeeAmount }).FirstOrDefault().FeeAmount;

                    //added by amit start
                    feeAmount = (from r in context.FeeDetails
                                 where r.CourseID == courseID
                                    && r.FeeTypeID == FeeTypeID
                                    && r.EffectiveFromDate <= System.Data.Entity.DbFunctions.CreateDateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
                                    && r.projectid == null
                                 orderby r.EffectiveFromDate descending
                                 select r.FeeAmount).FirstOrDefault();



                    ViewState["defaultFeeAmount"] = feeAmount;
                    //Session["defaultFeeAmount"] = feeAmount;
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
                };
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
            };
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

            };

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //mixed code
    protected bool IsValidForm()
    {
        try
        {
            //Added_UP_Project_30_12_2024_Start Added by Amit
            //bool status = true;
            //Added_UP_Project_30_12_2024_End


            DDLeducode_SelectedIndexChanged(DDLeducode, EventArgs.Empty);


            if (RdoUndergngDOEACC.SelectedValue == "I")
            {
                // added by amit
                if (radProject.SelectedValue == "1")
                {
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
                    //if (!isNumber(txtField1))
                    //{
                    //    lblerror.Visible = true;
                    //    GenerateNewCaptchaImage();
                    //    txtField1.Text = "";
                    //    lblerror.Text = "Invalid Class";
                    //    return false;
                    //}
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
                    //if (trUPField3.Visible)
                    //{
                    //    if (!isBlank(txtField3))
                    //    {
                    //        lblerror.Visible = true;
                    //        GenerateNewCaptchaImage();
                    //        txtcode.Text = "";
                    //        //lblerror.Text = "Roll Number can not be left blank";
                    //        lblerror.Text = lblField3.Text + " can not be left blank";
                    //        return false;
                    //    }
                    //    if (String.IsNullOrWhiteSpace(txtField3.Text))
                    //    {
                    //        GenerateNewCaptchaImage();
                    //        txtcode.Text = "";
                    //        //throw new Exception("Please enter Roll Number");
                    //        throw new Exception("Please enter " + lblField3.Text);
                    //    }
                    //}
                    //if (trUPField4.Visible)
                    //{

                    //    if (!isBlank(txtField4))
                    //    {
                    //        lblerror.Visible = true;
                    //        GenerateNewCaptchaImage();
                    //        txtcode.Text = "";
                    //        //lblerror.Text = "Field4 can not be left blank";
                    //        lblerror.Text = lblField4.Text + " can not be left blank";
                    //        return false;
                    //    }
                    //    if (String.IsNullOrWhiteSpace(txtField4.Text))
                    //    {
                    //        GenerateNewCaptchaImage();
                    //        txtcode.Text = "";
                    //        //throw new Exception("Please enter Field4");
                    //        throw new Exception("Please enter " + lblField4.Text);
                    //    }
                    //}
                    // Validate Class (Field2)
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

                        if (!Regex.IsMatch(txtField3.Text.Trim(), @"^[a-zA-Z0-9]+$"))
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
                    // added by amit
                    //if (radProject.SelectedValue == "1" && DdlProject.SelectedItem.Text.IndexOf("school", StringComparison.OrdinalIgnoreCase) >= 0)
                    //{

                    //    if (!validateDataFromUploadedExcel())
                    //    {
                    //        //status = false;
                    //        //ShowAlert("Some Condition Does not Matched");
                    //        ShowAlert("Data Mismatch with the Uploaded");
                    //        return false;

                    //        // if candidate is from UP board, the data should be matched.
                    //    }

                    //}
                    if (radProject.SelectedValue == "1" && DdlProject.SelectedItem.Text.IndexOf("school", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
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
            //// amit added validation for up fields
            //if (trBSB.Visible == true)
            //{
            //    if (!isBlank(txtUDISECode))
            //    {
            //        lblerror.Visible = true;
            //        GenerateNewCaptchaImage();
            //        txtcode.Text = "";
            //        lblerror.Text = Label14.Text + "cannot be left blank";
            //    }
            //}
            //if (trUPField2.Visible == true)
            //{
            //    if (!isBlank(txtField2))
            //    {
            //        lblerror.Visible = true;
            //        GenerateNewCaptchaImage();
            //        txtcode.Text = "";
            //        lblerror.Text = lblField2.Text + "cannot be left blank";
            //    }
            //}
            //if (trUPField3.Visible == true)
            //{
            //    if (!isBlank(txtField3))
            //    {
            //        lblerror.Visible = true;
            //        GenerateNewCaptchaImage();
            //        txtcode.Text = "";
            //        lblerror.Text = lblField3.Text + "cannot be left blank";
            //    }
            //}
            //if (trUPField4.Visible == true)
            //{
            //    if (!isBlank(txtField4))
            //    {
            //        lblerror.Visible = true;
            //        GenerateNewCaptchaImage();
            //        txtcode.Text = "";
            //        lblerror.Text = lblField4.Text + "cannot be left blank";
            //    }
            //}
            //if (trUPField5.Visible == true)
            //{
            //    if (!isBlank(txtField5))
            //    {
            //        lblerror.Visible = true;
            //        GenerateNewCaptchaImage();
            //        txtcode.Text = "";
            //        lblerror.Text = lblField5.Text + "cannot be left blank";
            //    }
            //}


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

                    if (!System.Text.RegularExpressions.Regex.IsMatch(TxtGuardianName.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
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


            if (!String.IsNullOrEmpty(txtaadhar.Text))
            {
                if (!IsNumeric(txtaadhar.Text))
                {
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    throw new Exception("Not valid Aadhaar Number.");
                }
            }

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

            // commented out by Amit
            if (String.IsNullOrEmpty(txtapaar.Text))
            {
                GenerateNewCaptchaImage();
                throw new Exception("Apaar ID  should not be Blank.");

            }
            if (String.IsNullOrEmpty(txtapaar.Text))
            {
                GenerateNewCaptchaImage();
                throw new Exception("Apaar ID  should not be Blank.");

            }
            if (ImgUpload.FileName.ToString() == "")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Photo can not be left blank";
                return false;
            }
            if (!isvalidFileExtension(ImgUpload))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Photo .Only jpg, gif, jpeg ,png extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(ImgUpload, 51200))
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
            if (ImgUploadSignature.FileName.ToString() == "")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Signature can not be left blank";
                return false;
            }
            if (!isvalidFileExtension(ImgUploadSignature))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Signature .Only jpg, gif, jpeg ,png extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(ImgUploadSignature, 51200))
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
            if (ImgUploadThumb.FileName.ToString() == "")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Thumb Impression can not be left blank";
                return false;
            }
            if (!isvalidFileExtension(ImgUploadThumb))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Thumb Impression .Only jpg, gif, jpeg ,png extensions are allowed.";
                return false;
            }
            if (!isvalidFileSize(ImgUploadThumb, 51200))
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
                //Validation of apaar stopeped
                //string token = validateApaar.getToken();
                //if (token == "")
                //{
                //    txtcode.Text = "";
                //    GenerateNewCaptchaImage();
                //    throw new Exception("Apaar ID not validated, Try again");
                //}
                //string validateData = validateApaar.getApaarData(txtapaar.Text, token);
                //if (validateData != null && validateData != "error")
                //{
                //    apaarResponse apaarReturn = JsonConvert.DeserializeObject<apaarResponse>(validateData);
                //    if (apaarReturn == null || apaarReturn.status == "Token is Invalid")
                //    {
                //        txtcode.Text = "";
                //        GenerateNewCaptchaImage();
                //        throw new Exception("Apaar ID not validated, Try again");
                //    }
                //    //return ("status" + biometricReturn.status);
                //    bool statusId = false;

                //    string errorCode = "";
                //    if (apaarReturn.status == "1")
                //    {
                //        statusId = true;
                //        errorCode = apaarReturn.statuscode;
                //    }
                //    if (apaarReturn.status == "0")
                //    {
                //        statusId = false;
                //        errorCode = apaarReturn.status_code;
                //    }

                //    int result = apaarResponse.saveResponse(txtapaar.Text, apaarReturn.status, errorCode, apaarReturn.message, apaarReturn.cname, validateData);
                //    if (apaarReturn.status == "0")
                //    {
                //        txtcode.Text = "";
                //        GenerateNewCaptchaImage();
                //        throw new Exception("Invalid Apaar, if not generated, please generate or correct and enter");
                //    }
                //    DateTime dob = Convert.ToDateTime(txtDob.Text);
                //    string dateofbirth = dob.ToString("dd/MM/yyyy");

                //    string dateofbirth1 = apaarReturn.dob;
                //    if (dateofbirth1.Length < 10)
                //    {
                //        string[] s = dateofbirth1.Split('/');
                //        if (s[0].Length < 2)
                //            s[0] = "0" + s[0];

                //        if (s[1].Length < 2)
                //            s[1] = "0" + s[1];



                //        dateofbirth1 = s[0] + "/" + s[1] + "/" + s[2];
                //    }
                //    if (dateofbirth1 != dateofbirth)
                //    {
                //        txtcode.Text = "";
                //        GenerateNewCaptchaImage();
                //        throw new Exception("Invalid Apaar or Date of birth Mismatch, if apaar not generated, please generate or correct and enter");
                //    }
                //    string vname = Regex.Replace(apaarReturn.cname, @"\s+", " ");

                //    if (vname.ToLower().Trim() != txtAppName.Text.ToLower())
                //    //   if (apaarReturn.cname.Trim().ToLower() != txtAppName.Text.Trim().ToLower())
                //    {
                //        txtcode.Text = "";
                //        GenerateNewCaptchaImage();
                //        throw new Exception("Invalid Apaar or Name Mismatch, if apaar not generated, please generate or correct and enter");
                //    }
                //    if (apaarReturn.gender.ToLower() != ddl_gender.SelectedItem.Text.Trim().ToLower())
                //    {
                //        txtcode.Text = "";
                //        GenerateNewCaptchaImage();
                //        throw new Exception("Invalid Apaar or Personal Data Mismatch, if apaar not generated, please generate or correct and enter");
                //    }
                //}
                //else
                //{
                //    txtcode.Text = "";
                //    GenerateNewCaptchaImage();
                //    throw new Exception("Apaar could not be validated, Try again");
                //}

                // commented out for testing

                if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                {
                    btnSave.Text = "Update";
                    btnback.Visible = false;
                    UpdateData();
                }
                else
                {
                    btnSave.Text = "Submit";
                    btnback.Visible = true;
                    SaveData();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
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

                //added by amit
                if (courseId == 1)
                    TrUPBoard.Visible = true;


                // to reset the fields set by Select Project, added 25/06/25
                radProject.SelectedValue = "0";
                DdlAccState.SelectedValue = "0";
                DdlAccCentre.SelectedValue = "0";
                DdlAccState.Enabled = true;
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
                TrUPBoard.Visible = false;

                trUPProject.Visible = false;
                trBSB.Visible = false;
                trUPField2.Visible = false;
                trUPField3.Visible = false;
                trUPField4.Visible = false;
                trUPField5.Visible = false;


                // amit added 23-6-2025
                DdlProject.SelectedIndex = 0;
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
                            lblExperienceInYears.Visible = true;
                            lblExperienceInYears.Text = "<font color='red'>* </font>(Minimum " + HfExperience.Value + " year(s) of experience required)";
                        }
                    }
                    else
                    {
                        HfExperience.Value = "0";
                    }
                }

            };
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
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }
    }
    protected void txtDob_TextChanged(object sender, EventArgs e)
    {
        DateTime todaydate = DateTime.Now;
        DateTime Inputdate = Convert.ToDateTime(txtDob.Text);
        int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);
        string applicantType = RdoUndergngDOEACC.SelectedValue.ToString();
        if (courseType < 5)
            bindDeclaration(applicantType, countAge);

    }
    protected void DdlAccCentre_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DdlAccCentre.SelectedValue != "0")
            AlertModalPopUp.Show();
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
            };
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


                        //aadhar details
                        if (application.AadharNumber.HasValue)
                            txtaadhar.Text = application.AadharNumber.Value.ToString();
                        else
                            txtaadhar.Text = "";


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

    //Added by Amit
    protected void radProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (radProject.SelectedValue == "1")
        {
            //trBSB.Visible = true;
            TrUPBoard.Visible = true;
            //trUPField1.Visible = true;


            /* changed by amit_codes on 28-03-2025
            trUPField2.Visible = true;
            trUPField3.Visible = true;
            trUPField4.Visible = true;
            trUPField5.Visible = true;
            */
            trUPProject.Visible = true;
            // DdlAccState.SelectedValue = "28";
            // DdlAccState.Enabled = false;
            bindUPProject(Convert.ToInt32(Request.QueryString["id"]));
            //bindUPProjectAccCentre(Convert.ToInt32(DdlAccState.SelectedValue), Convert.ToInt32(Request.QueryString["id"]));
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
        };
    }
    //Added by Amit

    //Added_UP_Project_New_18_02_2025_Start

    /* Old code
    protected void DdlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand scCommand = new SqlCommand("SELECT [ID],[field1],[field2],[field3],[field4],[field5]  FROM [NIELIT].[dbo].[projCriteriaMaster] where projID=@projID", new SqlConnection(con.ConnectionString));

                int hdCount = 4;
                //scCommand.Parameters.AddWithValue("@projID", ddlProject.SelectedValue);
                scCommand.Parameters.AddWithValue("@projID", DdlProject.SelectedValue);

                if (scCommand.Connection.State == ConnectionState.Closed)
                {
                    scCommand.Connection.Open();
                }

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(scCommand);
                //DataSet ds = new DataSet();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["field1"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field1"].ToString()))
                        {
                            //trUPField1.Visible = true;
                            //lblField1.Text = row["field1"].ToString();
                            trBSB.Visible = true;
                            Label14.Text = row["field1"].ToString() + "<font color='red'>*</font>";
                            txtUDISECode.Text = "";
                        }
                        else
                            trBSB.Visible = false;
                        //trUPField1.Visible = false;

                        if (row["field2"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field2"].ToString()))
                        {
                            trUPField2.Visible = true;
                            lblField2.Text = row["field2"].ToString() + "<font color='red'>*</font>";
                            hdCount++;
                            lblSrField2.Text = "1.2." + hdCount;
                            txtField2.Text = "";
                        }
                        else
                            trUPField2.Visible = false;
                        if (row["field3"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field3"].ToString()))
                        {
                            trUPField3.Visible = true;
                            lblField3.Text = row["field3"].ToString() + "<font color='red'>*</font>";
                            hdCount++;
                            lblSrField3.Text = "1.2." + hdCount;
                            txtField3.Text = "";
                        }
                        else
                            trUPField3.Visible = false;
                        if (row["field4"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field4"].ToString()))
                        {
                            trUPField4.Visible = true;
                            lblField4.Text = row["field4"].ToString() + "<font color='red'>*</font>";
                            hdCount++;
                            lblSrField4.Text = "1.2." + hdCount;
                            txtField4.Text = "";
                        }
                        else
                            trUPField4.Visible = false;
                        if (row["field5"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["field5"].ToString()))
                        {
                            trUPField5.Visible = true;
                            lblField5.Text = row["field5"].ToString() + "<font color='red'>*</font>";
                            hdCount++;
                            lblSrField5.Text = "1.2." + hdCount;
                            txtField5.Text = "";
                        }
                        else
                            trUPField5.Visible = false;
                    }
                }
                else
                {
                    //trUPField1.Visible = false;
                    trUPField2.Visible = false;
                    trUPField3.Visible = false;
                    trUPField4.Visible = false;
                    trUPField5.Visible = false;
                }
            }
            if (DdlProject.SelectedValue != "0")
                getFeeDetailForProjectCourse();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }
    }
    */
    //protected void DdlProject_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ToString();

    //        // this function works on the PROJECT CHANGE ie. Tech Project , UP MSP , etc, etc


    //        if (DdlProject.SelectedValue == "36")
    //        {
    //            DdlAccState.SelectedValue = "28";
    //            DdlAccState.Enabled = false;
    //            // bindUPProject(Convert.ToInt32(Request.QueryString["id"]));
    //            bindUPProjectAccCentre(Convert.ToInt32(DdlAccState.SelectedValue), Convert.ToInt32(Request.QueryString["id"]));
    //        }
    //        else
    //        {
    //            DdlAccState.SelectedValue = "0";
    //            DdlAccCentre.SelectedValue = "0";
    //            DdlAccState.Enabled = true;
    //            //bindUPProjectAccCentre(Convert.ToInt32(DdlAccState.SelectedValue), Convert.ToInt32(Request.QueryString["id"]));
    //        }

    //        using (SqlConnection con = new SqlConnection(constr))
    //        {
    //            string query = "SELECT [ID], [field1], [field2], [field3], [field4], [field5] FROM [NIELIT].[dbo].[projCriteriaMaster] WHERE projID = @projID";

    //            using (SqlCommand scCommand = new SqlCommand(query, con))
    //            {
    //                scCommand.Parameters.AddWithValue("@projID", DdlProject.SelectedValue);

    //                SqlDataAdapter da = new SqlDataAdapter(scCommand);
    //                DataTable dt = new DataTable();
    //                da.Fill(dt);

    //                int hdCount = 4;

    //                // Hide all fields by default
    //                trUPField2.Visible = false;
    //                trUPField3.Visible = false;
    //                trUPField4.Visible = false;
    //                trUPField5.Visible = false;
    //                trBSB.Visible = false;

    //                if (dt.Rows.Count > 0)
    //                {
    //                    DataRow row = dt.Rows[0]; // Since projID is unique, get first row only.

    //                    trBSB.Visible = !string.IsNullOrWhiteSpace(row["field1"].ToString());
    //                    if (trBSB.Visible)
    //                    {
    //                        Label14.Text = row["field1"].ToString() + "<font color='red'>*</font>";
    //                        txtUDISECode.Text = "";
    //                        // ddlUDISECode.SelectedIndex = 0;
    //                    }

    //                    trUPField2.Visible = !string.IsNullOrWhiteSpace(row["field2"].ToString());
    //                    if (trUPField2.Visible)
    //                    {
    //                        lblField2.Text = row["field2"].ToString() + "<font color='red'>*</font>";
    //                        lblSrField2.Text = "1.2." + (++hdCount);
    //                        txtField2.Text = "";
    //                    }

    //                    trUPField3.Visible = !string.IsNullOrWhiteSpace(row["field3"].ToString());
    //                    if (trUPField3.Visible)
    //                    {
    //                        lblField3.Text = row["field3"].ToString() + "<font color='red'>*</font>";
    //                        lblSrField3.Text = "1.2." + (++hdCount);
    //                        txtField3.Text = "";
    //                    }

    //                    trUPField4.Visible = !string.IsNullOrWhiteSpace(row["field4"].ToString());
    //                    if (trUPField4.Visible)
    //                    {
    //                        lblField4.Text = row["field4"].ToString() + "<font color='red'>*</font>";
    //                        lblSrField4.Text = "1.2." + (++hdCount);
    //                        txtField4.Text = "";
    //                    }

    //                    trUPField5.Visible = !string.IsNullOrWhiteSpace(row["field5"].ToString());
    //                    if (trUPField5.Visible)
    //                    {
    //                        lblField5.Text = row["field5"].ToString() + "<font color='red'>*</font>";
    //                        lblSrField5.Text = "1.2." + (++hdCount);
    //                        txtField5.Text = "";
    //                    }
    //                }
    //            }
    //        }

    //        // Call additional function only if a valid project is selected
    //        if (DdlProject.SelectedValue != "0")
    //            getFeeDetailForProjectCourse();
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message.ToString() + ex.Source.ToString());
    //    }
    //}

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

            if (DdlProject.SelectedItem.Text.Contains("UP"))
            {
                DdlAccState.SelectedValue = "28";
                DdlAccState.Enabled = false;
                bindUPProjectAccCentre(Convert.ToInt32(DdlAccState.SelectedValue), Convert.ToInt32(Request.QueryString["id"]));
            }
            else
            {
                DdlAccState.SelectedValue = "0";
                DdlAccCentre.SelectedValue = "0";
                DdlAccState.Enabled = true;
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


    protected int validateDataFromUploadedExcel()
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
                    if (projid == 10023)
                    {
                        // Special logic for project 36
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

                                  && f.EffectiveFromDate == (from c in context.FeeDetails
                                                             where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                                                               && c.projectid == projectId     //Added_30_12_2024
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
            };
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
    //Added_UP_Project_New_18_02_2025_End
}