using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Transactions;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.IO.Compression;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Data.Objects;
using System.Text.RegularExpressions;
using EConnect;
using DocumentFormat.OpenXml.Spreadsheet;
using System.ServiceModel.Activities;


public partial class Admin_NielitStudentFormalEdit_HO : BasePage
{
    int stateid = 0;
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 entityID = 0;
    Int32 lnkID = 0;
    Int32 UserRefNumber = 0;
    Int32 UserTypeid = 0;
    Int32 NielitCentreId = 0;
    Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
    Int64 NielitCentrelinkedToCentreId = 0;
    String NameCenter = "";
    protected void Page_Load(object sender, EventArgs e)
    {


        //txtrefno.Text = "NIELIT Chandigarh-56";
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");

            //commented 12 august
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);

            //commented 12 august
            entityID = Convert.ToInt32(Session["EntityID"]);
            UserTypeid = Convert.ToInt32(Session["UserType"]);

            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            //  User objUser1;
            using (EConnectContext context = new EConnectContext())
            {
                // objUser1 = new EConnect.URM.User();

                User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);


            }

            Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
            if (!Page.IsPostBack)
            {
                //User objUser;
                BindCourses();
                using (EConnectContext context = new EConnectContext())
                {
         //           objUser = new EConnect.URM.User();

         //           User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
         //           UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);
         //           NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                    using (NIELITMISContext context1 = new NIELITMISContext())
                    {

         //               if (UserTypeid == 10)
             //           {


         //                   var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
         //                   NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
         //                   lnkID = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
             //               if (lnkID != 0)
             //               {
             //                   NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
             //                   txtInstitute.Text = intitutesName.Name;
             //                   Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);

                 //           }
                 //           else
                 //           {
                 //               NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                 //               txtInstitute.Text = intitutesName.Name;
                 //               Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                 
                 //           }
                    }
                     //   else if (UserTypeid == 11)
                     //   {
                          //  var intituteslinkedToCentre = context1.NonAffInstitutes.Find(loginUser.UserRefNumber);
                          //  lnkID = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                     //       NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                     //       if (institutesName != null)
                     //       {
                     //           txtInstitute.Text = institutesName.Name;
                     //           Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                     //       }

                }
                     //   else if (UserTypeid == 4)
             //           {
             //               var intituteslinkedToCentre = from s in context1.AffInstitutes
                                                          //where s.instituteID == loginUser.UserRefNumber
             //                                             select new { ID = s.ID, linkedToCentre = s.linkedToCentre };
             //               if (intituteslinkedToCentre.Count() == 0)
             //               {
             //                   ShowAlert("Menu is not available for the institute");
             //                   return;
             //               }
             //               NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().ID);

             //               NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
             //               lnkID = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
             //               NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
             //               if (institutesName != null)
             //               {
             //                   txtInstitute.Text = institutesName.Name;
             //                   Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
             //              }
             //           }
             //           }

                    //Added
                    if (ddlWhetherPlaced.SelectedValue.ToString() == "1")
                        btn.Visible = true;
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        BindGender();
                        BindEditNewModeData();
                        BindEditCourse();

                    }
                    else
                    {
                        BindGender();
                        BindEditNewModeData();
                        BindEditCourse();
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";

                        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Nielit Student Formal Edit", "Admin/NielitStudentFormalEdit_HO aspx?CourseId=" + Request.QueryString["CourseId"].ToString(), ""));
                        }
                        else
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Nielit Student Formal Edit", "Admin/NielitStudentFormalEdit_HO aspx", ""));
                        }

                    }
                    if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                        ShowAlert(Request.QueryString["msg"].ToString());
                 // }
            }
            BreadCrumb1.Render();
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void BindEditCourse()
    {
        try
        {


            //commented because i deleted 1.3 (course drop down)
            Int32 nielitcentreid = Convert.ToInt32(Session["EntityID"]);

            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                using (DataTable dt = GetCoursesForSemesterMaster())       //bind course dropdown
                {
                    //if (dt.Rows.Count > 0)
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        ddlCourse.Items.Clear();
                        ddlCourse.DataSource = dt;
                        ddlCourse.DataTextField = "Name";
                        ddlCourse.DataValueField = "ID";
                        ddlCourse.DataBind();
                        ddlCourse.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    
    public DataTable GetCoursesForSemesterMaster()
    {
        Int32 nielitcentreid = Convert.ToInt32(Session["EntityID"]);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetCoursesForSemester", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@centerid", SqlDbType.Int));
                    cmd.Parameters["@centerid"].Value = nielitcentreid;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    protected void BindEditNewModeData()
    {
        try
        {
            bindMaritalStatus();
            bindCastCategory();
            bindReligion();
            bindState();
            bindCompanyName();
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");

                var CorState = (from s in context.Locations
                                orderby (s.Name)
                                where s.LocationTypeID == 2
                                && s.ParentLocationID == 1
                                select new { ValueField = s.ID, TextField = s.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCmpState, CorState, lst);

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("NielitStudentFormalEdit_HO.aspx", true);

    }

    protected void bindPaymentOption()
    {
        using (EConnectContext context = new EConnectContext())
        {
            Course currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["id"]));
            string CourseCatg = currentCourse.CourseCategory.Name.ToString();

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
                        //LblExamName.Text = LateFeeExam.FirstOrDefault().ExamName;
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
                        //LblExamName.Text = NormalFeeExam.FirstOrDefault().ExamName;
                        examID = NormalFeeExam.FirstOrDefault().ExamID;
                        btnSave.Visible = true;
                    }

                }
                ViewState["ExamID"] = examID.ToString();
                //ShowFeeDetail(examID);
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
            if (!String.IsNullOrEmpty(Request.QueryString["key"]))
            {
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    Int64 applID = Convert.ToInt64(Request.QueryString["key"]);
                    //This Query will get all the information of Applied Canditate by generated Application id                                     
                    var application = context.NielitCentreStudent.Find(applID);


                    //var instituteDetail = (from a in context.AccreditationDetails
                    //                       join i in context.Institutes
                    //                           on a.InstituteID equals i.ID
                    //                       where i.ID == application.InstituteID
                    //                       select new
                    //                       {
                    //                           Name = i.Name,
                    //                           CentreID = i.ID,
                    //                           StateId = i.StateID,
                    //                           DistrictId = i.DistrictID,
                    //                           StateName = i.State.Name,
                    //                           DistrictName = i.District.Name,
                    //                           AccNo = a.AccreditationNumber
                    //                       }).FirstOrDefault();

                    //RdoUndergngDOEACC.SelectedValue = "I";


                    //DdlAccState.SelectedValue = Convert.ToString(instituteDetail.StateId);
                    //int id = Convert.ToInt32(DdlAccState.SelectedValue);
                    //DdlAccState_SelectedIndexChanged(DdlAccState, EventArgs.Empty);
                    //DdlAccCentre.SelectedValue = Convert.ToString(instituteDetail.CentreID);
                    //}
                    //else
                    //{
                    //RdoUndergngDOEACC.SelectedValue = "D";
                    //TxtExperienceInYears.Text = application.ExperienceInYears.ToString();
                    //}
                    // ddlPaymentOption.SelectedValue = application.PaymentSourceID.HasValue ? application.PaymentSourceID.Value.ToString() : (RdoUndergngDOEACC.SelectedValue == "D" ? "1" : "2");
                    //Candidate Personal Detail...

                    ddlSalutaionName.SelectedValue = application.Salutation.ToString();
                    txtAppName.Text = GetInitCap(application.Name.ToString());
                    lblname.Text = txtAppName.Text;

                    // ddlCenter.SelectedValue = application.InstituteID.ToString();

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
                    }

                    ddl_gender.SelectedValue = application.Gender;
                    txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");
                    //txtBodyMark.Text = string.IsNullOrEmpty(application.BodyMark) == false && !string.IsNullOrWhiteSpace(application.BodyMark) ? application.BodyMark : "";
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
                    ddlReligion.SelectedValue = application.ReligionID.ToString();
                    //Candidate Contact Detail...

                    TxtSTDcode.Text = application.PhoneNumber.HasValue && application.PhoneNumber != 0 ? "0" + application.StdNumber.ToString() : "";
                    txtCorPhoneNo.Text = application.PhoneNumber.ToString();
                    txtCorMobileNo.Text = application.MobileNumber.ToString();
                    txtEmailId.Text = application.EmailAddress.ToString();
                    RdisEWS.SelectedValue = application.Is_EWS.ToString();

                    //aadhar details
                    //if (application.AadharNumber.HasValue)
                    //    txtaadhar.Text = application.AadharNumber.Value.ToString();
                    //else
                    //    txtaadhar.Text = "";


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
                    ddlcordistrict.SelectedValue = application.CorDistrictID.ToString();
                    txtCorPinCode.Text = application.CorPinCode.ToString();

                    //btnback.Visible = false;
                };

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
            // UpdateData1();
            DataTable dt = new DataTable();
            SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString);
            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            cnn.Open();
            string name = txtAppName.Text;
            string fathername = txtFatherName.Text;
            string motherName = txtMotherName.Text;
            string gaurdian = TxtGuardianName.Text;
            string genderName = ddl_gender.SelectedValue.Trim();
            DateTime dob = Convert.ToDateTime(txtDob.Text);
            Int64 courseid = Convert.ToInt64(ddlCourse.SelectedValue.Trim());  // commented because course dropdown(1.3) i deleted
            Int64 batchid = Convert.ToInt64(ddlBatch.SelectedValue.Trim());
            Int32 SemNumber = Convert.ToInt32(ddlsemester.SelectedItem.Text);
            Int32 semno = SemNumber + 1;
            EConnectContext context1 = new EConnectContext();

            using (NIELITMISContext context = new NIELITMISContext())
            {
                string Number = txtrefno.Text;
               
                using (SqlCommand cmd = new SqlCommand("select ID  FROM [NIELITMIS].[dbo].[NielitCentreStudent] where Number= '" + @Number + "' "))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.AddWithValue("@Number", Number);
                        sda.SelectCommand = cmd;

                        sda.Fill(dt);


                    }
                }

                Int64 applID = Convert.ToInt64(dt.Rows[0]["ID"].ToString());

                var objRegistration = context.NielitCentreStudent.Find(applID);
                objRegistration.CourseID = Convert.ToInt64(ddlCourse.SelectedValue);
                objRegistration.batch_ID = Convert.ToInt64(ddlBatch.SelectedValue);

                //Added_21_08_2024
                objRegistration.University_RegistrationNo = Convert.ToString(UniversityRegNo.Text); // added by amit
               // objRegistration.University_RegistrationNo = UniversityRegNo.Text;

                //Added_21_08_2024


                if (RdisEWS.SelectedValue == "Y")
                    objRegistration.Is_EWS = true;
                else
                    objRegistration.Is_EWS = false;

                objRegistration.AlreadyRegistered = false;
                  objRegistration.RegisteredCourseID = Convert.ToInt32(ddlCourse.SelectedValue);             

                string salutation = ddlSalutaionName.SelectedItem.Text;
                salutation = salutation.Substring(0, salutation.IndexOf('/'));

                objRegistration.Salutation = salutation.Trim();
                objRegistration.Name = txtAppName.Text.Trim().ToString();
                if (Rdoownertype.SelectedValue == "P")
                {
                    objRegistration.FatherName = txtFatherName.Text.Trim();
                    objRegistration.MotherName = txtMotherName.Text.Trim();
                    objRegistration.GuardianName = null;
                }
                else if (Rdoownertype.SelectedValue == "G")
                {
                    objRegistration.GuardianName = TxtGuardianName.Text.Trim();
                    objRegistration.FatherName = null;
                    objRegistration.MotherName = null;
                }

             /*  var applicationcheck = (from a in context.NielitCentreStudent

                                     where a.semesterid == semno && a.batch_ID == batchid && a.CourseID == courseid && a.Name == name && a.Gender == genderName && a.DateOfBirth == dob && ((a.FatherName == fathername && a.MotherName == motherName) || a.GuardianName == gaurdian)
                                        select new
                                        {
                                            ID = a.ID,
                                            CourseId = a.CourseID,
                                            SemNo = a.semesterid,

                                       }).FirstOrDefault();




             //   if (applicationcheck == null)
                if (applicationcheck != null)
                    {
                    txtcode.Text = "";
                    ShowAlert("Updation for this semester number not allowed because next semester already started.");
                    return;
                }*/

                string gender = ddl_gender.SelectedValue;
                objRegistration.Gender = gender.Trim();

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
                objRegistration.PerCountryID = 0;
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
                    objRegistration.CorCountryID = 0;
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
                    objRegistration.CorCountryID = 0;
                    objRegistration.CorCityName = TxtCorCity.Text;
                    objRegistration.CorStateID = Convert.ToInt32(ddlCorState.SelectedValue);
                    objRegistration.CorDistrictID = Convert.ToInt32(ddlcordistrict.SelectedValue);
                    objRegistration.CorPinCode = Convert.ToInt32(txtCorPinCode.Text);
                }

                objRegistration.IsVerifiedByInstitute = false;
                DateTime Dob = Convert.ToDateTime(txtDob.Text);
                Int32 ReligionId = Convert.ToInt32(ddlReligion.SelectedValue);
                Int32 castCategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                Boolean IsHandicapped = Rdhandicapped.SelectedValue == "Y" ? true : false;

                objRegistration.ReligionID = ReligionId;
                objRegistration.UIDType = Convert.ToInt32(UidTypeDdl.SelectedValue);


                //Added for aadhaar Encryption
                if (UidTypeDdl.SelectedValue.ToString() == "1")
                    objRegistration.UIDNumber = EConnect.Utils.Security.Encryption.Encrypt(UidNumberTxt.Text.Trim().ToUpper());
                else
                    objRegistration.UIDNumber = UidNumberTxt.Text.Trim().ToUpper();

                objRegistration.AadharVerfied = false;

                if (ddlCourseComplete.SelectedValue == "1")
                {
                    objRegistration.whether_Course_Complete = true;
                }
                else
                {
                    objRegistration.whether_Course_Complete = false;
                }

                if (ddlWheatherPojectStu.SelectedValue == "1")
                {
                    objRegistration.whetherProjectStudent = true;
                    objRegistration.projectId = Convert.ToInt64(ddlProcname.SelectedValue);
                }
                else
                {
                    objRegistration.whetherProjectStudent = false;
                }

               
                if (ddlwhetherCertificateIssued.SelectedValue == "1")
                {
                    objRegistration.whether_Certificate_Issued = true;

                    if (isBlank(txtcertificateIssueDate))
                    {
                        DateTime certDate = Convert.ToDateTime(txtcertificateIssueDate.Text.ToString());
                        Int64 batchID = Convert.ToInt64(ddlBatch.SelectedValue);

                        if (context.NielitCentreBatchs.Where(s => s.ID == batchID && (s.endDate <= certDate)).Count() > 0)
                        {

                            objRegistration.certificate_Issue_Date = Convert.ToDateTime(txtcertificateIssueDate.Text);
                            //objRegistration.certificate_Issue_Date = Convert.ToDateTime(PlacementDates.ToString());
                            
                        }
                        else
                        {
                            var CenterBatch = (from s in context.NielitCentreBatchs
                                               where s.ID == batchID
                                               select s).FirstOrDefault();

                            lblerror.Visible = true;
                            lblerror.Text = "Certificate Issue date greater than or equal to " + CenterBatch.endDate.ToString("dd-MMM-yyyy");
                            txtcertificateIssueDate.Focus();
                            ShowAlert("Certificate Issue date grater than or equal to " + CenterBatch.endDate.ToString("dd-MMM-yyyy"));
                            return;
                        }
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Please Enter Certificate Issue Date.";
                        txtcertificateIssueDate.Focus();
                        return;
                    }
                }
                else
                {
                    if (txtcertificateIssueDate.Text == "")
                    {
                        objRegistration.whether_Certificate_Issued = false;
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Please select Whether Certificate Issue.";
                        ddlwhetherCertificateIssued.Focus();
                        return;
                    }
                }


                if (ddlWhetherPlaced.SelectedValue == "1")
                {
                    objRegistration.whetherPlaced = true;
                    if (isBlank(txtPlacementDate))
                    {
                        Int64 batchID = Convert.ToInt64(ddlBatch.SelectedValue);
                        DateTime certDate = Convert.ToDateTime(txtPlacementDate.Text.ToString());
                        if (context.NielitCentreBatchs.Where(s => s.ID == batchID && (s.endDate <= certDate)).Count() > 0)
                        {
                            objRegistration.placementDate = Convert.ToDateTime(txtPlacementDate.Text);
                        }                       
                        else
                        {

                            var CenterBatch = (from s in context.NielitCentreBatchs
                                               where s.ID == batchID
                                               select s).FirstOrDefault();

                            lblerror.Visible = true;
                            lblerror.Text = "Placement date greater than or equal to " + CenterBatch.endDate.ToString("dd-MMM-yyyy");
                            txtPlacementDate.Focus();
                            return;
                        }
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Please Enter Whether Placement Date with addition of Placement Details";
                        txtPlacementDate.Focus();
                        return;
                    }
                }
                else
                {
                    if (txtPlacementDate.Text == "")
                    {
                        objRegistration.whetherPlaced = false;
                        objRegistration.placementDate = null;
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Please select Wheather placed.";
                        txtPlacementDate.Focus();
                        return;
                    }


                }

                if (ddlWhetherPlaced.SelectedValue == "1")
                {
                    if (ddlCompanyName.SelectedValue != "0")
                    {
                        //objRegistration.company_Name = txtCompanyName.Text;
                        objRegistration.company_Name = Convert.ToInt64(ddlCompanyName.SelectedValue);
                        objRegistration.comapny_Address = txtCmpAdd1.Text;
                        objRegistration.comapny_Address2 = txtCmpAdd2.Text;
                        objRegistration.comapny_Address3 = txtCmpAdd3.Text;
                        objRegistration.CompnyCityName = txtCmyCityName.Text;
                        objRegistration.CompnyCountry_ID = 0;
                        objRegistration.compnyState_ID = Convert.ToInt32(ddlCmpState.SelectedValue);
                        objRegistration.compnyDistrict_ID = Convert.ToInt32(ddlCmpDistrict.SelectedValue);
                        objRegistration.CompnyPinCode = Convert.ToInt32(txtCmpPinCode.Text);
                        objRegistration.CompnyPinCode = Convert.ToInt32(txtCmpPinCode.Text);
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Please Select Company Name.";
                        ddlCompanyName.Focus();
                        return;
                    }
                }
                objRegistration.affidavit_Date = null;
                objRegistration.affidavit_No = "";
                objRegistration.affidavit_Verified = false;

                objRegistration.FinalSubmissionDate = DateTime.Now;
                objRegistration.FinalSubmitted = true;
         //       objRegistration.enter_By = Convert.ToInt32(Session["UserID"]);
                objRegistration.enter_Date = DateTime.Now;

                context.Entry(objRegistration).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                long Appid = objRegistration.ID;
                NIELITStudentCentreHistory();
                strMessage = "Record Updated Successfully.";

                Response.Redirect("NielitStudentFormalEdit_HO.aspx?msg=" + strMessage, true);
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void NIELITStudentCentreHistory()
    {
        try
        {
            //string constr = ConfigurationManager.ConnectionStrings["NIELITMISContextt"].ConnectionString;
            string refno = txtrefno.Text;
            string remarks = txtRemarks.Text;
            int EditBy = Convert.ToInt32(Session["UserID"]);
            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection Conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("InsertNIELITStudentCenterHistory", Conn))
                {
                    Conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@refnumber", refno);
                    cmd.Parameters.AddWithValue("@remarks", remarks);
                    cmd.Parameters.AddWithValue("@editby", EditBy);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
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
                //Aadhaartr.Visible = false;
            }
            else
            {
                UIDtr.Visible = false;
                //Aadhaartr.Visible = true;
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

    protected void bindCourses()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");

                //var Courses = from s in context.NielitCentreCourses
                //              where s.IsVerified == true
                //              orderby (s.Name)
                //              select new { ValueField = s.ID, TextField = s.Name };

                //if (!IsPostBack)
                //{
                //    // Bind the data to the dropdown list
                //    ddlCourse.DataSource = ID; // Replace with your actual data source
                //    ddlCourse.DataTextField = "TextField"; // Replace with your actual property names
                //    ddlCourse.DataValueField = "ValueField";
                //    ddlCourse.DataBind();
                //}
                //string selectedValue = "SomeValue"; // Replace with the actual value you want to set
                //ListItem selectedItem = ddlCourse.Items.FindByValue(selectedValue);
                //if (selectedItem != null)
                //{
                //    ddlCourse.SelectedValue = selectedValue;
                //}
                //else
                //{
                //    // Handle the case where the selected value doesn't exist in the list
                //    // You may want to set a default value or display a message to the user
                //}

                var Courses = from s in context.NielitCentreCourses
                              where s.IsVerified == true
                              orderby s.Name
                              select new { ValueField = s.ID, TextField = s.Name };

                //var Courses = context.NielitCentreCourse
                //   .Where(s => s.IsVerified==true)
                //   .OrderBy(s => s.Name)
                //   .Select(s => new { ValueField = s.ID, TextField = s.Name })
                //   .ToList();


                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, Courses, lst);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindCompanyName()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");

                var companyMas = from t in context.companyMasters
                                 orderby (t.company_Name)
                                 select new { ValueField = t.ID, TextField = t.company_Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCompanyName, companyMas, lst);
            };

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void bindBatches()
    {
        //try
        //{
        //    string Number = txtrefno.Text;
        //    using (NIELITMISContext context = new NIELITMISContext())
        //    {
        //        ListItem lst = new ListItem("--Select One--", "0");

        //        //       var Batch = from s in context.NielitCentreBatchs
        //        //                   where s.IsVerified == true && s.enterBy == loginUserNo
        //        //                   (s.centreID.ToString().Trim() == UserRefNumber.ToString().Trim() || s.subCentreID.ToString().Trim() == UserRefNumber.ToString().Trim())
        //        //                   orderby (s.Name)
        //        //                   select new { ValueField = s.ID, TextField = s.Name };
        //        //      EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);

        //        var Batch = (from a in context.NielitCentreStudent
        //                         // join b in context.NielitCourseDurations on a.CourseID equals b.ID
        //                         //  join n in context.NielitCentreCourses on b.courseID equals n.ID
        //                     join d in context.NielitCentreBatchs on a.batch_ID equals d.ID
        //                     where a.Number == Number && a.
        //                     //&& d.centreID.ToString().Trim() == 
        //                     orderby d.Name
        //                     select new { ValueField = d.ID, TextField = d.Name });
        //        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);




        //    }
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
        string Number = txtrefno.Text;

        using (NIELITMISContext context = new NIELITMISContext())
        
            try
            {
            ListItem lst = new ListItem("--Select One--", "0");

                // Uncomment and complete your LINQ query based on your requirements
                // var Batch = from s in context.NielitCentreBatchs
                //             where s.IsVerified == true && s.enterBy == loginUserNo &&
                //                   (s.centreID.ToString().Trim() == UserRefNumber.ToString().Trim() ||
                //                    s.subCentreID.ToString().Trim() == UserRefNumber.ToString().Trim())
                //             orderby s.Name
                //             select new { ValueField = s.ID, TextField = s.Name };
                // EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);

                // Modify the LINQ query based on your data model
                //var Batch = (from a in context.NielitCentreStudent
                //             join d in context.NielitCentreBatchs on a.batch_ID equals d.ID
                //             where a.Number == Number && a.batch_ID == d.ID 
                //             orderby d.Name

                //             select new { ValueField = d.ID, TextField = d.Name });

                var Batch = (from a in context.NielitCentreStudent
                             join d in context.NielitCentreBatchs on a.batch_ID equals d.ID
                            where d.CourseDurationID == a.CourseID
                            // where d.CourseDurationID == 1
                             orderby d.Name
                             select new { ValueField = d.ID, TextField = d.Name });

                //var result = (from a in context.NielitCentreStudent
                //              join d in context.NielitCentreBatchs on a.batch_ID equals d.ID
                //              where a.Number == Number && a.batch_ID == d.ID
                //              orderby d.Name
                //              select new { ValueField = a.ID, TextField = d.Name }).ToList();



                //var Batch = (from a in context.NielitCentreStudent
                //             join d in context.NielitCentreBatchs on a.batch_ID equals d.ID
                //             where a.Number == Number && a.batch_ID == d.ID
                //             orderby d.Name
                //             select new { ValueField = a.ID, TextField = d.Name });
                // Bind the DropDownList with the result of the LINQ query
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
            }
            catch (Exception ex)
            {
            throw ex;
            }

    }
    protected void bindProject()
    {
        try
        {
            string Number = txtrefno.Text;
            Int64 cID = Convert.ToInt64(ddlCourse.SelectedValue);
          //  Int64 cID = 6044;
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");

                          


                    var Proc = from a in context.NielitCentreStudent
                               join t in context.NielitProjectss on a.projectId equals t.ID
                               join k in context.NielitProjCoursess on t.ID equals k.projID
                               join d in context.NielitCourseDurations on k.courseID equals d.ID
                               where d.ID == cID && a.Number == Number
                               && k.IsActive
                               orderby (t.ProjectName)
                               select new { ValueField = t.ID, TextField = t.ProjectName };

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlProcname, Proc, lst);
               
                

            }

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
	 string categoryId="";
            if (txtrefno.Text != null)
            {
                string Number = txtrefno.Text;
                using (var c1 = new NIELITMISContext())
                {
                    var app = (from a in c1.NielitCentreStudent
                               where a.Number == Number
                               select new
                               {
                                   castCat = a.CastCategoryID
                               }).FirstOrDefault();
                    if (app != null)
                        categoryId = app.castCat.ToString();
                }
            }
            using (var context = new EConnectContext())
            {
                ddlCategory.Items.Clear();
                ListItem lst = new ListItem("--Select One--", "0");
                if (ddlProcname.SelectedValue.ToString() == "2")
                {
                    var castcategory = from p in context.CastCategories
                                       where (p.Code.Equals("SC") || p.Code.Equals("ST"))
                                       orderby (p.DisplayOrder)
                                       select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCategory, castcategory, lst);
                }
                else
                {
                    var castcategory = from p in context.CastCategories
                                       orderby (p.DisplayOrder)
                                       select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCategory, castcategory, lst);
                }
            };
		if (categoryId != "")
                ddlCategory.SelectedValue = categoryId;
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

    protected void bindEducational()
    {
        try
        {
            //Int32 ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
            int courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
            using (var context = new EConnectContext())
            {
                //List<Int32> qualificationLevels = context.QualificationEligibility.Where(s => s.CourseID == courseID && s.ApplicantTypeID == ApplicantTypeId && s.EffectiveDateFrom <= DateTime.Now).OrderByDescending(c => c.EffectiveDateFrom).Select(c => c.QualificationLevelID).ToList();
                ListItem lst = new ListItem("--Select One--", "0");

                //var education = from p in context.EducationalQualifications
                //                join q in context.QualificationEligibility on p.QualificationLevelID equals q.QualificationLevelID
                //                where q.CourseID == courseID
                //                && q.ApplicantTypeID == ApplicantTypeId
                //                select new { ValueField = p.ID, TextField = p.Name };
                //EConnect.Utils.Common.ControlUtility.BindListObject(DDLeducode, education, lst);
            };
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
                var CorState = (from s in context.Locations
                                orderby (s.Name)
                                where s.LocationTypeID == 2
                                && s.ParentLocationID == 1
                                select new { ValueField = s.ID, TextField = s.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCorState, CorState, lst);
                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlCmpState, CorState, lst);
                //var AccState = (from s in context.Locations
                //                join i in context.Institutes on s.ID equals i.StateID
                //                join a in context.AccreditationDetails on i.ID equals a.InstituteID
                //                orderby (s.Name)
                //                where s.LocationTypeID == 2
                //                && s.ParentLocationID == 1
                //                && a.CourseID == courseID
                //                && a.AccreditationStatusID != withdrawlid
                //                select new { ValueField = s.ID, TextField = s.Name }).Distinct().ToList();
                //EConnect.Utils.Common.ControlUtility.BindListObject(DdlAccState, AccState, lst);

                var PState = (from s in context.Locations
                              orderby (s.Name)
                              where s.LocationTypeID == 2
                              && s.ParentLocationID == 1
                              select new { ValueField = s.ID, TextField = s.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlPState, PState, lst);
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
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlcordistrict, district, lst);
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
            //using (var context = new EConnectContext())
            //{
            //    var AccCentre = (from i in context.Institutes
            //                     join d in context.AccreditationDetails on i.ID equals d.InstituteID
            //                     orderby i.Name
            //                     where i.StateID == stateid
            //                     && d.CourseID == courseID
            //                     && d.AccreditationStatusID != withdrawlid
            //                     select new { ValueField = i.ID, TextField = d.AccreditationNumber + " - " + i.Name + ", " + (!string.IsNullOrEmpty(i.CityName) ? i.CityName : "") }).ToList();
            //    EConnect.Utils.Common.ControlUtility.BindListObject(DdlAccCentre, AccCentre.Distinct(), lst);
            //};
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
                    //if (!String.IsNullOrEmpty(course.ApplicantTypeID.ToString()))
                    //{
                    //    if (course.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                    //        RdoUndergngDOEACC.SelectedValue = "D";
                    //    else
                    //        RdoUndergngDOEACC.SelectedValue = "I";
                    //    lblApplicantType.Text = RdoUndergngDOEACC.SelectedItem.Text;
                    //    RdoUndergngDOEACC.Visible = false;
                    //    lblApplicantType.Visible = true;
                    //    RdoUndergngDOEACC_SelectedIndexChanged(this, null);
                    //}
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
            ddl_gender.Enabled = isActive;
            txtDob.Enabled = isActive;
            ddlCategory.Enabled = isActive;
            //imgDob.Visible = isActive;

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
            UniversityRegNo.Text = "";
            txtAppName.Text = "";
            txtFatherName.Text = "";
            txtMotherName.Text = "";
            TxtGuardianName.Text = "";
            TxtCorAddressLine1.Text = "";
            TxtCorAddressLine2.Text = "";
            TxtCorAddressLine3.Text = "";
            TxtCorCity.Text = "";
            TxtPerAddressLine1.Text = "";
            TxtPerAddressLine2.Text = "";
            TxtPerAddressLine3.Text = "";
            TxtPerCity.Text = "";
            TxtSTDcode.Text = "";
            // txtRegNo.Text = "";
            //TxtYearOfPassing2.Text = "";
            //txtBodyMark.Text = "";
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
            //DDLeducode.SelectedValue = "0";
            ddlSalutaionName.SelectedValue = "0";

            //txtCompanyName.Text = "";
            ddlCompanyName.SelectedValue = "0";
            txtCmpAdd1.Text = "";
            txtCmpAdd2.Text = "";
            txtCmpAdd3.Text = "";
            txtCmyCityName.Text = "";
            ddlCmpState.SelectedValue = "0";
            ddlCmpDistrict.SelectedValue = "0";
            ddlwhetherCertificateIssued.SelectedValue = "0";
            ddlWhetherPlaced.SelectedValue = "0";
            ddlCourseComplete.SelectedValue = "0";
            txtcertificateIssueDate.Text = "";
            txtCmpPinCode.Text = "";
            UidNumberTxt.Text = "";
            // rdbCourseType.Enabled = true;
            // txtRegNo.Enabled = true;
            // rdbCourseType.ClearSelection();
            UidTypeDdl.SelectedValue = "0";
            lblerror.Text = "";

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

                //DDLRegForCourse.DataSource = cname.ToList();
                //DDLRegForCourse.DataValueField = "ValueField";
                //DDLRegForCourse.DataTextField = "TextField";
                //DDLRegForCourse.DataBind();
                //DDLRegForCourse.Enabled = false;


                //showing declaration
                if (currentCourse.CourseCategoryID == ShortermCoursesCategory)
                {
                    //Label90.Text = "Registration Cycle / पंजीकरण चक्र";
                    tddeclaration1.Style.Add("display", "none");
                    tddeclaration2.Style.Add("display", "block");
                    trnote.Visible = true;
                    lbldeccoursecode.Text = GetInitCap(currentCourse.Name);
                    lbldeccoursecode.Text = GetInitCap(currentCourse.Name);
                    LblHCourseCode.Text = GetInitCap(currentCourse.Name);
                    //ddlPaymentOption.Items.RemoveAt(0);
                }
                else if (currentCourse.Code == "ACC")
                {
                    //Label90.Text = "Registration Cycle / पंजीकरण चक्र";
                }
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
   
    protected bool IsValidForm()
    {
        try
        {
            if (txtrefno.Text == "")
            {
                tderror.Visible = true;
                lblerror.Visible = true;
                lblerror.Text = "Please Enter Reference Number.";
                return false;
            }

            if (txtRemarks.Text == "")
            {
                lblerror.Text = "Please Enter Reason for Editing.";
                return false;
            }
            if (ddlCourse.SelectedValue == "0")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Course";
                ddlCourse.Focus();
                return false;
            }

            if (ddlBatch.SelectedValue == "0")
            {
                tderror.Visible = true;
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Batch";
                ddlBatch.Focus();
                return false;
            }

            if (ddlsemester.SelectedValue == "0")
            {
                tderror.Visible = true;
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Semester";
                ddlsemester.Focus();
                return false;
            }

            if (ddlWheatherPojectStu.SelectedValue == "0")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Project Student";
                ddlBatch.Focus();
                return false;
            }

            if (ddlWheatherPojectStu.SelectedValue == "1")
            {
                if (ddlProcname.SelectedValue == "0")
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Please Select Project Name";
                    ddlBatch.Focus();
                    return false;
                }
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

            if (!Char.IsLetter(txtAppName.Text, txtAppName.Text.Length - 1))
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

            if (TxtSTDcode.Text == "")
            {
                if (txtCorPhoneNo.Text != "")
                {
                    TxtSTDcode.Text = "";
                    TxtSTDcode.Focus();
                    throw new Exception("Please enter STD Code.");
                }
            }

            if (txtCorPhoneNo.Text == "")
            {
                if (TxtSTDcode.Text != "")
                {
                    txtCorPhoneNo.Text = "";
                    txtCorPhoneNo.Focus();
                    throw new Exception("Please enter Phone No.");
                }
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtAppName.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
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
            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorCity.Text, "^[a-zA-Z\u00FC\u00DC ]"))
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
                if (!isSelected(ddlcordistrict))
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


            if (UIDtr.Visible == true)
            {

                if (isSelected(UidTypeDdl))
                {
                    if (UidTypeDdl.SelectedValue == "1")  // ----Aadhaar
                    {
                        //Added for update mode 
                        if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                        {
                        }
                        else
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

            if (ddlwhetherCertificateIssued.SelectedValue == "1")//Parents
            {
                if (!isBlank(txtcertificateIssueDate))
                {
                    GenerateNewCaptchaImage();
                    txtcertificateIssueDate.Text = "";
                    throw new Exception("Certificate Issue Date can not be left blank");
                }
            }
            if (!isBlank(txtcode))
            {
                tderror.Visible = true;                
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Captcha Code can not be left blank";
                return false;
            }
            if (txtcode.Text != ViewState["CaptchCode"].ToString())
            {
                tderror.Visible = true;    
                lblerror.Visible = true;
                lblerror.Text = "Invalid Captcha Code";
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                return false;
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

                btnSave.Text = "Update";
                btnback.Visible = true;
                UpdateData();
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
                ddlcordistrict.Enabled = false;
                TxtCorAddressLine1.Text = TxtPerAddressLine1.Text;
                TxtCorAddressLine2.Text = TxtPerAddressLine2.Text;
                TxtCorAddressLine3.Text = TxtPerAddressLine3.Text;
                TxtCorCity.Text = TxtPerCity.Text;
                txtCorPinCode.Text = TxtPpincode.Text;
                ddlCorState.SelectedValue = ddlPState.SelectedValue;
                ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                ddlcordistrict.SelectedValue = ddlPdistrict.SelectedValue;
            }
            else
            {
                TxtCorAddressLine1.Enabled = true;
                TxtCorAddressLine2.Enabled = true;
                TxtCorAddressLine3.Enabled = true;
                TxtCorCity.Enabled = true;
                txtCorPinCode.Enabled = true;
                ddlCorState.Enabled = true;
                ddlcordistrict.Enabled = true;
                TxtCorAddressLine1.Text = "";
                TxtCorAddressLine2.Text = "";
                TxtCorAddressLine3.Text = "";
                TxtCorCity.Text = "";
                txtCorPinCode.Text = "";
                ddlCorState.SelectedValue = "0";
                ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                ddlcordistrict.SelectedValue = "0";
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

            //stateid = Convert.ToInt32(DdlAccState.SelectedValue);
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
            Response.Redirect("NielitStudentFormalEdit_HO.aspx?msg=" + strMessage, true);
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
            //LblExamName.Text = "";
            //if (RdoUndergngDOEACC.SelectedValue == "I")
            //{
            //    TrLastCenterAccno.Visible = true;
            //    TrAccCentre.Visible = true;
            //    TrExperience.Visible = false;
            //    ddlPaymentOption.SelectedValue = "2";
            //    ddlPaymentOption.Enabled = true;
            //    //DdlAccState.SelectedValue = "0";
            //    DdlAccState_SelectedIndexChanged(DdlAccState, EventArgs.Empty);
            //    bindDeclaration("I", 0);

            //}
            //else
            //{
            //    TrExperience.Visible = true;
            //    TrLastCenterAccno.Visible = false;
            //    TrAccCentre.Visible = false;
            //    ddlPaymentOption.SelectedValue = "1";
            //    ddlPaymentOption.Enabled = false;
            //    txtDob.Text = "";
            //    bindDeclaration("D", 0);
            //}
            //Int32 ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
            bindEducational();
            //bindExamName(ApplicantTypeId);
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
            ddlcordistrict.Items.Clear();
            bindDistrict(id1, ref ddlcordistrict);
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
                else if (ddlSalutaionName.SelectedValue == "X")
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
        //string applicantType = RdoUndergngDOEACC.SelectedValue.ToString();
        //if (courseType < 5)
        //    bindDeclaration(applicantType, countAge);

    }

    protected void DdlAccCentre_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (DdlAccCentre.SelectedValue != "0")
        //    AlertModalPopUp.Show();
    }

    protected void ddlCmpState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id1 = Convert.ToInt32(ddlCmpState.SelectedValue);
            //ddlcordistrict.Items.Clear();
            bindDistrict(id1, ref ddlCmpDistrict);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void ddlWheatherPojectStu_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlWheatherPojectStu.SelectedValue == "1")
        {

            ddlProcname.Enabled = true;
		   bindAllProject();
        }
        else
        {
            ddlProcname.SelectedValue = "0";
            ddlProcname.Enabled = false;
        }
        // bindCastCategory();
    }

    protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCompanyName.SelectedValue != "0")
            {
                Int64 iID = Convert.ToInt64(ddlCompanyName.SelectedValue);
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    var application = context.companyMasters.Find(iID);

                    //ddlCompanyName.SelectedItem = application.company_Name;
                    txtCmpAdd1.Text = application.comapny_Address.ToString();

                    if (application.comapny_Address2 != null)
                    {
                        txtCmpAdd2.Text = application.comapny_Address2.ToString();
                    }

                    if (application.comapny_Address3 != null)
                    {
                        txtCmpAdd3.Text = application.comapny_Address3.ToString();
                    }

                    if (application.CompnyCityName != null)
                    {
                        txtCmyCityName.Text = application.CompnyCityName.ToString();
                    }

                    if (application.compnyState_ID != null)
                    {
                        ddlCmpState.SelectedValue = application.compnyState_ID.ToString();
                    }
                    ddlCmpState_SelectedIndexChanged(ddlCmpState, EventArgs.Empty);
                    if (application.compnyDistrict_ID != null)
                    {
                        ddlCmpDistrict.SelectedValue = application.compnyDistrict_ID.ToString();
                    }

                    if (application.CompnyPinCode != null)
                    {
                        txtCmpPinCode.Text = application.CompnyPinCode.ToString();
                    }

                    if (application.contactPersonName != null)
                    {
                        txtPersonName.Text = application.contactPersonName.ToString();
                    }

                    if (application.contactPersonMobile != null)
                    {
                        txtMob.Text = application.contactPersonMobile.ToString();
                    }

                    //ddlCorState_SelectedIndexChanged(ddlCmpState, EventArgs.Empty);
                    //ddlCmpDistrict.SelectedValue = application.compnyDistrict_ID.ToString();
                    //txtCmpPinCode.Text = application.CompnyPinCode.ToString();

                    //txtPersonName.Text = application.contactPersonName.ToString();
                    //txtMob.Text = application.contactPersonMobile.ToString();

                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlWhetherPlaced_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlWhetherPlaced.SelectedValue == "1")
        {
            btn.Visible = true;
            //trPersonDetails.Visible = true;
            //tr2.Visible = true;
            //trCompanyHead.Visible = true;
            //trCompanyName.Visible = true;
            //trCmpAdd1.Visible = true;
            //trCmpAdd2.Visible = true;
            //trCmpAdd3.Visible = true;
            //trCmyCityName.Visible = true;
            //trCmpState.Visible = true;
            //trCmpDistrict.Visible = true;
            //trCmpPinCode.Visible = true;

        }

        else
        {
            btn.Visible = false;
            //trPersonDetails.Visible = false;
            //tr2.Visible = false;
            //trCompanyHead.Visible = false;
            //trCompanyName.Visible = false;
            //trCmpAdd1.Visible = false;
            //trCmpAdd2.Visible = false;
            //trCmpAdd3.Visible = false;
            //trCmyCityName.Visible = false;
            //trCmpState.Visible = false;
            //trCmpDistrict.Visible = false;
            //trCmpPinCode.Visible = false;

        }
    }
    protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem lst = new ListItem("--Select One--", "0");
        Int64 NelitCentreLinkId = 0;
        Int64 NielitCentrelinkedToCentreId = 0;
        // Int64 subcentreid = 0;
        Int64 centreID = 0;
        string Seleted = "";
      //  User objUser;

        //ddlBatch.ClearSelection();
        //ddlCourse.ClearSelection();
 //       ddlCenter.ClearSelection();
       // ddlBatch.Items.Clear();
        ddlCourse.Items.Clear();
        try
        {
            using (EConnectContext context1 = new EConnectContext())
            {
               // objUser = new EConnect.URM.User();
               // User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
               // Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context = new NIELITMISContext())
                {
 //                   Seleted = RdoAffInstOrNonAffInst.SelectedValue;
 //                   centreID = Convert.ToInt64(ddlCenter.SelectedValue);
                    //if (UserTypeid == 10)
                    {
                     //   var instituteslinkedToCentre = context.NielitCentres.Find(loginUser.UserRefNumber);
                     //   NielitCentrelinkedToCentreId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                   // if (UserTypeid == 11)
                    {
                       // var instituteslinkedToCentre = context.NonAffInstitutes.Find(loginUser.UserRefNumber);
                      //  NielitCentrelinkedToCentreId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }

 //                   if (NielitCentrelinkedToCentreId != 0)
 //                   {
 //                       NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
 //                       txtInstitute.Text = institutesName.Name;
 //                       Int32 NelitCentreLinkId1 = Convert.ToInt32(institutesName.ID);
 //
 //                   }



 //                   if (Seleted == "2")
                        //BindCourses();
 //                   BindCourses(Convert.ToInt32(centreID), Convert.ToInt16(Seleted));
 //                         BindCourses(Convert.ToInt32(), Convert.ToInt16(Seleted));
                          

 //                   else
 //                       BindCourses(Convert.ToInt32(centreID), Convert.ToInt16(Seleted));
              //            BindCourses(Convert.ToInt32(), Convert.ToInt16(Seleted));

         //                  ddlCourse.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Error:" + ex.Message);
        }

    }

    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 Courseid = 0;
       // ddlBatch.ClearSelection();
        //ddlBatch.Items.Clear();
        ddlsemester.ClearSelection();
        ddlsemester.Items.Clear();
        try
        {
            Int64 nielitcentreid = Convert.ToInt32(Session["EntityID"]);
            Courseid = Convert.ToInt64(ddlCourse.SelectedValue.Trim());
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var BatchCode = from s in context.NielitCentreBatchs
                                where s.IsVerified == true && s.IsSemBased == true && s.centreID == nielitcentreid
                                        && s.CourseDurationID == Courseid  && (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
                                orderby (s.Name)
                                select new { ValueField = s.ID, TextField = s.BatchCode };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchCode, lst);


            }
            ddlWheatherPojectStu.SelectedValue = "0";
            ddlWheatherPojectStu_SelectedIndexChanged(sender, e);
        }

        catch (Exception ex)
        {
            throw ex;
        }


    }

    protected void BindBatch()
    {
        try
        {
           // User objUser;
            using (EConnectContext context = new EConnectContext())
            {
              //  objUser = new EConnect.URM.User();
               // User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                using (NIELITMISContext context1 = new NIELITMISContext())
                {
                    ListItem lst = new ListItem("--Select One--", "0");
                  //  if (UserTypeid == 10)
                    {
                       // var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                       // Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                       // NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        if (NielitCentrelinkedToCentreId != 0)
                        {
                            NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
 //                            RdoAffInstOrNonAffInst.SelectedValue = "2";
 //                             ddlCenter.Enabled = false;
                            //     ListItem lst = new ListItem("--Select One--", "0");
                            var BatchName = from p in context1.NielitCentreBatchs
                                            where p.IsVerified == true && (p.startDate <= System.DateTime.Now && p.endDate >= System.DateTime.Now)
                                            && p.centreID == NelitCentreLinkId
                                            orderby (p.Name)
                                            select new { ValueField = p.ID, TextField = p.Name };
                              EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                            //EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                        }
                        else
                        {
                            NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
 //                             RdoAffInstOrNonAffInst.SelectedValue = "2";
 //                             ddlCenter.Enabled = false;
 //                            ListItem lst = new ListItem("--Select One--", "0");
                            var BatchName = from p in context1.NielitCentreBatchs
                                            where p.IsVerified == true && (p.startDate <= System.DateTime.Now && p.endDate >= System.DateTime.Now)
                                            && p.centreID == NelitCentreLinkId
                                            orderby (p.Name)
                                            select new { ValueField = p.ID, TextField = p.Name };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                        }
                    }
                   // else if (UserTypeid == 11)
                    {
                      //  var intituteslinkedToCentre = context1.NonAffInstitutes.Find(loginUser.UserRefNumber);
                      //  NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                      //  Int32 subcentreId = Convert.ToInt32(intituteslinkedToCentre.ID);
                        NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        }
                        // FillddlSubcentreName();
                        //ListItem lst = new ListItem("--Select One--", "0");
                        var BatchName = from p in context1.NielitCentreBatchs
                                        where p.IsVerified == true && (p.startDate <= System.DateTime.Now && p.endDate >= System.DateTime.Now)
                     //                   && p.centreID == centreID
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                    }
                   // else if (UserTypeid == 4)
                    {
                       // var intituteslinkedToCentre = context1.AffInstitutes.Find(loginUser.UserRefNumber);
                       // NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                       //
                       //Int32 subcentreId = Convert.ToInt32(intituteslinkedToCentre.ID);

                        NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        }
                        // FillddlSubcentreName();
                        //   ListItem lst = new ListItem("--Select One--", "0");
                        var BatchName = from p in context1.NielitCentreBatchs
                                        where p.IsVerified == true && (p.startDate <= System.DateTime.Now && p.endDate >= System.DateTime.Now)
                     //                   && p.subCentreID == subcentreId
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);

                    }
                }
                //  ListItem lst = new ListItem("--Select One--", "0");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void certificateIssueDate_Calender_SelectionChanged(object sender, System.EventArgs e)
    {

        NIELITMISContext context = new NIELITMISContext();
        Int64 applID = Convert.ToInt64(Request.QueryString["Key"]);

        var objRegistration = context.NielitCentreStudent.Find(applID);

        if (objRegistration.whether_Certificate_Issued.ToString() == "1")
        {
            //txtcertificateIssueDate.Text = Convert.ToDateTime(certificateIssueDate_Calender.SelectedDate, CultureInfo.GetCultureInfo("en-US")).ToString("dd-MMM-yyyy");
        }
        else
        {
            lblerror.Visible = true;
            lblerror.Text = "Not allowed to change date!!.";
            txtcertificateIssueDate.Focus();
            return;
        }
    }

    protected void CalendarPlacementDate_SelectionChanged(object sender, System.EventArgs e)
    {
        //txtPlacementDate.Text = Convert.ToDateTime(CalendarPlacementDate.SelectedDate, CultureInfo.GetCultureInfo("en-US")).ToString("dd-MMM-yyyy");
    }

    protected void TextBox1_TextChanged(object sender, System.EventArgs e)
    {
        DateTime todaydate = DateTime.Now;
        DateTime Inputdate = Convert.ToDateTime(txtDob.Text);
        int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);
        //string applicantType = RdoUndergngDOEACC.SelectedValue.ToString();
        //if (courseType < 5)
            //bindDeclaration(applicantType, countAge);
    }

    protected void btn_Click(object sender, System.EventArgs e)
    {
        using (NIELITMISContext context = new NIELITMISContext())
        {

            Int64 applID = Convert.ToInt64(Request.QueryString["Key"]);
            var objRegistration = context.NielitCentreStudent.Find(applID);

            Session["StudentID"] = objRegistration.Number;
            Session["batch_ID"] = objRegistration.batch_ID;
        }

        Response.Redirect("../Admin/StudentPlacementDetails.aspx", true);
    }
    protected void ddlProcname_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindCastCategory();

    }
    protected bool checkDuplicateAAdhaar()
    {
        try
        {
            DateTime dob = Convert.ToDateTime(txtDob.Text);
            Int32 currentCourseID = Convert.ToInt32(ddlCourse.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int32 count;

                string EncAadhaar = EConnect.Utils.Security.Encryption.Encrypt(UidNumberTxt.Text.Trim().ToUpper());

                // if (Rdoownertype.SelectedValue == "G")
                // {

                var stuBatch = (from a in context.NielitCentreBatchs
                                where a.ID.ToString() != ddlBatch.SelectedValue.ToString()
                                && (System.Data.Entity.DbFunctions.TruncateTime(System.DateTime.Today) >= System.Data.Entity.DbFunctions.TruncateTime(a.startDate) && System.Data.Entity.DbFunctions.TruncateTime(System.DateTime.Today) <= System.Data.Entity.DbFunctions.TruncateTime(a.endDate))
                                select new
                                {
                                    batchID = a.ID
                                }).ToList();


                var studentList = (from c in context.NielitCentreStudent

                                   where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)

                                   && (c.GuardianName.ToUpper() == TxtGuardianName.Text.ToUpper().Trim() || c.FatherName.ToUpper() == txtFatherName.Text.ToUpper().Trim())
                                   && c.Gender == ddl_gender.SelectedValue
                                   && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                                       // && c.CourseID != currentCourseID
                                   && c.UIDNumber == EncAadhaar
                                   && c.UIDType.ToString() == "1"

                                   select new
                                   {
                                       batch = c.batch_ID
                                   }).Distinct().ToList();

                if (studentList.Count() == 0)
                    return false;

                foreach (var batch1 in stuBatch)
                {
                    foreach (var studentList1 in studentList)
                    {
                        if (batch1.batchID == studentList1.batch)
                        {
                            return true;
                        }
                    }
                }
                return false;
                // }


            };
        }
        catch (Exception ex)
        {
            throw ex;
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
    // protected void BindCourses(Int32 CentreId, int CentreType)
    protected void BindCourses() 
    {
        // 2- Centre, 0--Non Aff Instt 1--Aff Instt

        using (NIELITMISContext context = new NIELITMISContext())
        {
            string Number = txtrefno.Text;
            ListItem lst = new ListItem("--Select One--", "0");
            //   using (DataTable dt = GetBatchCourseRecord(CentreId, CentreType))
            // using (DataTable  dt = new DataTable())
            // {
            //    //var Course = from p in context.NielitCentreCourses
            //    //             join k in context.NielitCourseDurations on p.ID equals k.courseID
            //    //             where p.IsVerified == true && k.isVerified==true
            //    //             orderby (p.Name)
            //    //             select new { ValueField = k.ID, TextField = p.Name+ " ( " + k.courseDurationDays +" Days )" };
            //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, Course, lst);

            //       if (dt.Rows.Count > 0)
            //       {
            //           ddlCourse.DataSource = dt;
            //           ddlCourse.DataTextField = "Name";
            //           ddlCourse.DataValueField = "ID";
            //           ddlCourse.DataBind();
            //           ddlCourse.Items.Insert(0, new ListItem("--Select One--", "0"));
            //       }
            var Course = (from a in context.NielitCentreStudent
                          join b in context.NielitCourseDurations on a.CourseID equals b.ID
                          join n in context.NielitCentreCourses on b.courseID equals n.ID

                          where a.Number == Number && n.CourseCategoryID == 101

                          select new { ValueField = b.ID, TextField = n.Name });
            //var Course = (from a in context.NielitCourseDurations
            //              from b in context.NielitCentreStudent
            //              join n in context.NielitCentreCourses on a.courseID equals n.ID
            //              where b.Number == Number 
            //              select new { ValueField = n.ID, TextField = n.Name });
            //var Course = (from a in context.NielitCourseDurations
            //              from b in context.NielitCentreStudent
            //              join n in context.NielitCentreCourses on a.courseID equals n.ID
            //              where b.Number == Number && n.CourseCategoryID == 101
            //              select new { ValueField = n.ID, TextField = n.Name }).ToList();
            // Add ToList() if you want to materialize the query.
            //var Course = (from a in context.NielitCourseDurations
            //              join n in context.NielitCentreCourses on a.CourseID equals n.ID
            //              where a.number == Number && n.CourseCategoryID == 101
            //              select new { ValueField = n.ID, TextField = n.Name })
            // .ToList(); // Add ToList() if you want to materialize the query.

            if (Course == null)
            {
             //   lblerror.Visible = true;
                //lblerror.Text = "Message Does not belong to formal course!!.";
                ShowAlert("Course  Does not belong to formal Category!!.");
                return;
            }
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, Course, lst);


            // }
            
        }
    }

    
    protected DataTable GetBatchCourseRecord(Int32 CentreId, int CentreType)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetBatchCourseRecord", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pCentreId", SqlDbType.BigInt).Value = CentreId;
                    cmd.Parameters.Add("@pCentreType", SqlDbType.Int).Value = CentreType;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    public DataTable GetBatchesForSemesterMaster()
    {
        Int64 coursenameid = Convert.ToInt64(ddlCourse.SelectedValue);
        Int64 batchid = Convert.ToInt64(ddlBatch.SelectedValue);


        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSemesterforFormalCourse", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@courseId", SqlDbType.Int));
                    cmd.Parameters["@courseId"].Value = coursenameid;
                    cmd.Parameters.Add(new SqlParameter("@BatchId", SqlDbType.Int));
                    cmd.Parameters["@BatchId"].Value = batchid;


                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlsemester.Items.Clear();
        ListItem lst = new ListItem("--Select--", "0");
        using (NIELITMISContext context = new NIELITMISContext())
        {
            Int64 coursenameid = Convert.ToInt64(ddlCourse.SelectedValue);
            Int64 batchid = Convert.ToInt64(ddlBatch.SelectedValue);

            if (coursenameid != 0)
            {

                using (DataTable dt = GetBatchesForSemesterMaster())
                {
                    if (dt.Rows.Count > 0)
                    {
                        int i = Convert.ToInt32(dt.Rows[0]["SemNo"]);
                        int n = i;
                        for (i = 0; i <= n; i++)
                        {
                            if (i == 0)
                            {
                                ddlsemester.Items.Add(new ListItem("Select", "0"));
                            }
                            else
                            {
                                ddlsemester.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                ddlsemester.DataBind();
                            }
                        }
                    }
                }
            }
        }
    }

    protected void btnShow_Click(object sender, System.EventArgs e)
    {
        try
        {                     
            //bindProject();            
            lblerror.Text = "t1";
            lblerror.Visible = true;
            tralready.Visible = false;
            if (txtrefno.Text.Trim() == "")
            {
                lblerror.Visible = true;
                txtrefno.Text = "";
                lblerror.Text = "Please Enter Reference Number.";
            } 
            else
            {
                btnSave.Visible = true;
                btnSaveNext.Visible = false;
              //  ResetAll();
                string Number = txtrefno.Text;
              // User objUser;
                using (EConnectContext context1 = new EConnectContext())
                {
                   // objUser = new EConnect.URM.User();

                   // User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                   // UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);

                    using (NIELITMISContext context = new NIELITMISContext())
                    {

                        var application = (from a in context.NielitCentreStudent
                                           join b in context.NielitCourseDurations on a.CourseID equals b.ID
                                           join n in context.NielitCentreCourses on b.courseID equals n.ID
                                           join d in context.NielitCentreBatchs on a.batch_ID equals d.ID
                                           where a.Number == Number
                                           select new
                                           {

                                               CourseId = a.CourseID,
                                               BatchID = a.batch_ID,
                                               semNo = a.semesterid,
                                               UniversityRegNo= a.University_RegistrationNo,
                                               Salutation = a.Salutation,
                                               project = a.projectId,
                                               Name = a.Name,
                                               GuardianName = a.GuardianName,
                                               FatherName = a.FatherName,
                                               MotherName = a.MotherName,
                                               Gender = a.Gender,
                                               DateOfBirth = a.DateOfBirth,
                                               CastCategoryID = a.CastCategoryID,
                                               MaritalStatusID = a.MaritalStatusID,
                                               IsHandicaped = a.IsHandicaped ? 1 : 0,
                                               IsExServicemane = a.IsExServicemane ? 1 : 0,
                                               Is_EWS = a.Is_EWS ? 1 : 0,
                                               ReligionID = a.ReligionID,
                                               PhoneNumber = a.PhoneNumber,
                                               StdNumber = a.StdNumber,
                                               MobileNumber = a.MobileNumber,
                                               EmailAddress = a.EmailAddress,
                                               PerAddressLine1 = a.PerAddressLine1,
                                               PerAddressLine2 = a.PerAddressLine2,
                                               PerAddressLine3 = a.PerAddressLine3,
                                               PerCityName = a.PerCityName,
                                               PerStateID = a.PerStateID,
                                               PerDistrictID = a.PerDistrictID,
                                               PerPinCode = a.PerPinCode,
                                               CorAddressLine1 = a.CorAddressLine1,
                                               CorAddressLine2 = a.CorAddressLine2,
                                               CorAddressLine3 = a.CorAddressLine3,
                                               CorCityName = a.PerCityName,
                                               CorStateID = a.CorStateID,
                                               CorDistrictID = a.CorDistrictID,
                                               CorPinCode = a.CorPinCode,
                                               UIDType = a.UIDType,
                                               UIDNumber = a.UIDNumber,
                                               CenterId = a.InstituteID,
                                               whetherProject = a.whetherProjectStudent ? 1 : 0
                                               //  affidavit_No=a.affidavitNo,
                                               // affidavit_Date=a.affidavitDate,
                                               //affidavit_Verified=a.affidavitVerified

                                           }).FirstOrDefault();



                        //Candidate Personal Detail...
                        if (application != null)
                        {
                            trremarks.Visible = true;
                             BindCourses();
                             bindBatches();
                            // bindProject();
           



                          //if (UserRefNumber == application.CenterId)
                            //if (!string.IsNullOrEmpty(application.CenterId.ToString()))
                                if (!(string.IsNullOrEmpty(application.CenterId.ToString())))
                                    //if (txtrefno.Text.rim() == "")  

                                lblerror.Visible = true;
                                lblerror.Text = "Warning !! Please enter correct data and ensure to cross check the data before saving.";
                                //btnNext.Visible = true;
                                TblFormDetail.Visible = true;
                                trcourse.Visible = true;
                                trproject.Visible = true;
                                ddlCourse.Enabled = true;
                                ddlBatch.Enabled = true;
                                ddlsemester.Enabled = true;
                                //Console.WriteLine("application.CourseId123", application.CourseId);
                                ddlCourse.SelectedValue = application.CourseId.ToString();
                                ddlCourse.SelectedValue = application.CourseId != null && application.CourseId != 0 ? application.CourseId.ToString() : "0";

                            //if (application.CourseId != null && application.CourseId != 0)
                            //{
                            //    // Convert application.CourseId to string
                            //    string selectedValue = application.CourseId.ToString();

                            //    // Check if the selected value exists in the dropdown list items
                            //    ListItem selectedItem = ddlCourse.Items.FindByValue(selectedValue);

                            //    if (selectedItem != null)
                            //    {
                            //        // Set the SelectedValue only if it exists in the list
                            //        ddlCourse.SelectedValue = selectedValue;
                            //    }
                            //    else
                            //    {
                            //        // Handle the case where the selected value doesn't exist in the list
                            //        // You may want to set a default value or display a message to the user
                            //    }
                            //}
                            //else
                            //{
                            //    // Handle the case where application.CourseId is null or zero
                            //    // You may want to set a default value or display a message to the user
                            //    ddlCourse.SelectedValue = "0";  // or some default value
                            //}

                            //string selectedValue = application.CourseId != null ? application.CourseId.ToString() : "0";

                            // Check if the selectedValue exists in the DropDownList items
                            //ListItem listItem = ddlCourse.Items.FindByValue(selectedValue);

                            //if (listItem != null)
                            //{
                            //    // If the value exists, set it as the SelectedValue
                            //    ddlCourse.SelectedValue = selectedValue;
                            //}
                            //else
                            //{
                            //    // If the value doesn't exist, handle it accordingly
                            //    // You might want to set a default value or show an error message
                            //    ddlCourse.SelectedIndex = 0; // Set a default value or any other appropriate action
                            //}

                            //try
                            //{
                            //    string selectedValue = application.CourseId != null ? application.CourseId.ToString() : "0";

                            //    // Check if the value exists in the list before setting SelectedValue
                            //    ListItem listItem = ddlCourse.Items.FindByValue(selectedValue);

                            //    if (listItem != null)
                            //    {
                            //        ddlCourse.SelectedValue = selectedValue;
                            //    }
                            //    else
                            //    {
                            //        // Handle the case where the value does not exist in the list
                            //        // For example, set a default value or show an error message
                            //        ddlCourse.SelectedIndex = 0; // Set a default value or any other appropriate action
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //    // Log the exception or take appropriate action
                            // //   Console.WriteLine($"An error occurred while setting the SelectedValue: {ex.Message}");
                            //    throw; // Rethrow the original exception
                            //}

                            //if (ddlCourse.Items.Count > 0)
                            //{
                            //    // The DropDownList is populated; proceed to set SelectedValue
                            //}
                            //else
                            //{
                            //    // Handle the case where the DropDownList is empty
                            //}
                            //string selectedValue = application.CourseId != null ? application.CourseId.ToString() : "0";
                            //ListItem listItem = ddlCourse.Items.FindByValue(selectedValue);

                            //if (listItem != null)
                            //{
                            //    ddlCourse.SelectedValue = selectedValue;
                            //}
                            //else
                            //{
                            //    // Handle the case where the value is not in the list
                            //    // You might set a default value or show an error message
                            //}
                            //try
                            //{
                            //    string selectedValue = application.CourseId != null ? application.CourseId.ToString() : "0";
                            //    ListItem listItem = ddlCourse.Items.FindByValue(selectedValue);

                            //    if (listItem != null)
                            //    {
                            //        ddlCourse.SelectedValue = selectedValue;
                            //    }
                            //    else
                            //    {
                            //        // Handle the case where the value is not in the list
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //    // Log the exception or handle it appropriately
                            // //   Console.WriteLine($"An error occurred while setting the SelectedValue: {ex.Message}");
                            //}

                            //string selectedValue = application.CourseId != null ? application.CourseId.ToString() : "0";


                            //// Find the ListItem with the selected value
                            //ListItem listItem = ddlCourse.Items.FindByValue(selectedValue);

                            //try
                            //{
                            //    if (listItem != null)
                            //    {
                            //        // Set the SelectedValue if the item is found in the list
                            //        ddlCourse.SelectedValue = selectedValue;
                            //    }
                            //    else
                            //    {
                            //        // Handle the case where the value is not in the list
                            //        // You might set a default value or show an error message
                            //        ddlCourse.SelectedIndex = 0; // Set a default value or any other appropriate action
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //    // Log the exception or handle it appropriately
                            // //   Console.WriteLine($"An error occurred while setting the SelectedValue: {ex.Message}");
                            //    // Optionally, rethrow the exception if needed
                            //    throw;
                            //}
                            //try
                            //{
                            //    // Code before line 3645

                            //    // Check for null references, if necessary
                            //    if (application != null)
                            //    {
                            //        string selectedValue = application.CourseId != null ? application.CourseId.ToString() : "0";

                            //        // Check if the value exists in the DropDownList before setting SelectedValue
                            //        ListItem listItem = ddlCourse.Items.FindByValue(selectedValue);

                            //        if (listItem != null)
                            //        {
                            //            ddlCourse.SelectedValue = selectedValue;
                            //        }
                            //        else
                            //        {
                            //            // Handle the case where the value does not exist in the list
                            //            ddlCourse.SelectedIndex = 0; // Set a default value or any other appropriate action
                            //        }
                            //    }
                            //    else
                            //    {
                            //        // Handle the case where 'application' is null
                            //    }

                            //    // Code after line 3645
                            //}
                            //catch (Exception ex)
                            //{
                            //    // Log the exception or handle it appropriately
                            // //   Console.WriteLine($"An error occurred: {ex.Message}");
                            //    // Rethrow the exception if needed
                            //    throw;
                            //}

                                int id3 = Convert.ToInt32(ddlCourse.SelectedValue);
                            bindProject();
                            //ddlCourse_SelectedIndexChanged(ddlCourse, EventArgs.Empty);
                            if (application.BatchID != null)
                                ddlBatch.SelectedValue = application.BatchID.ToString();
                                ddlBatch_SelectedIndexChanged(ddlBatch, EventArgs.Empty);
                                if (application.semNo != null)
                                ddlsemester.SelectedValue = application.semNo.ToString();
             //                   ddlCourse_SelectedIndexChanged(ddlCourse, EventArgs.Empty);
                                ddlBatch.Enabled = true;
                                ddlBatch.SelectedValue = application.BatchID.ToString();
                                if (application.whetherProject.ToString() == "1")
                                {
                                    ddlWheatherPojectStu.SelectedValue = "1";
                                    ddlProcname.SelectedValue = application.project.ToString();
                                }
                                else if (application.whetherProject.ToString() == "0")
                                {
                                    ddlWheatherPojectStu.SelectedValue = "2";
                                }

                                //ddlWheatherPojectStu.SelectedValue = application.whetherProject;
                                ddlSalutaionName.SelectedValue = application.Salutation.ToString();
                                //ddlSalutaionName.Enabled = false;
                                ddlSalutaionName.Enabled = true;
                                txtAppName.Text = GetInitCap(application.Name.ToString());
                                lblname.Text = txtAppName.Text;
                                txtAppName.Enabled = true;
                                //UniversityRegNo.Text = application.UniversityRegNo.ToString();
                                UniversityRegNo.Text = application.UniversityRegNo.ToString();
                                lblname.Text = UniversityRegNo.Text;
                                UniversityRegNo.Enabled=true;
                                if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName) == true)
                                {
                                    trfather.Visible = true;
                                    trmother.Visible = true;
                                    trguardian.Visible = false;
                                    Rdoownertype.SelectedValue = "P";
                                    Rdoownertype.Enabled = true;
                                    Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                                    txtFatherName.Text = string.IsNullOrEmpty(application.FatherName) == false && !string.IsNullOrWhiteSpace(application.FatherName) ? GetInitCap(application.FatherName) : "";
                                    txtMotherName.Text = string.IsNullOrEmpty(application.MotherName) == false && !string.IsNullOrWhiteSpace(application.MotherName) ? GetInitCap(application.MotherName) : "";
                                    txtFatherName.Enabled = true;
                                    txtMotherName.Enabled = true;
                                }
                                else
                                {
                                    trfather.Visible = false;
                                    trmother.Visible = false;
                                    trguardian.Visible = true;
                                    Rdoownertype.SelectedValue = "G";
                                    Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                                    TxtGuardianName.Text = GetInitCap(application.GuardianName);
                                    TxtGuardianName.Enabled = false;
                                }

                                ddl_gender.SelectedValue = application.Gender;
                                txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");

                                ddlCategory.SelectedValue = application.CastCategoryID.ToString();
                                ddlMStatus.SelectedValue = application.MaritalStatusID.ToString();
                                ddl_gender.Enabled = true;
                                txtDob.Enabled = true;
                                ddlCategory.Enabled = true;
                                ddlMStatus.Enabled = true;
                                if (application.IsHandicaped.ToString() == "1")
                                {
                                    Rdhandicapped.SelectedValue = "Y";
                                }
                                else
                                {
                                    Rdhandicapped.SelectedValue = "N";
                                }
                                //Rdhandicapped.Enabled = false;
                                Rdhandicapped.Enabled = true;
                                if (application.IsExServicemane.ToString() == "1")
                                {
                                    Rdexserviceman.SelectedValue = "Y";
                                }
                                else
                                {
                                    Rdexserviceman.SelectedValue = "N";
                                }
                                Rdexserviceman.Enabled = true;
                                if (application.Is_EWS.ToString() == "1")
                                {
                                    RdisEWS.SelectedValue = "Y";
                                }
                                else
                                {
                                    RdisEWS.SelectedValue = "N";
                                }
                                RdisEWS.Enabled = true;
                                ddlReligion.SelectedValue = application.ReligionID.ToString();
                                ddlReligion.Enabled = true;

                                TxtSTDcode.Text = application.PhoneNumber.HasValue && application.PhoneNumber != 0 ? "0" + application.StdNumber.ToString() : "";

                                txtCorPhoneNo.Text = application.PhoneNumber.ToString();
                                txtCorMobileNo.Text = application.MobileNumber.ToString();
                                txtEmailId.Text = application.EmailAddress.ToString();


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
                                ddlcordistrict.SelectedValue = application.CorDistrictID.ToString();
                                txtCorPinCode.Text = application.CorPinCode.ToString();

                                UidTypeDdl.SelectedValue = application.UIDType.ToString();

                                if (UidTypeDdl.SelectedValue.ToString() == "1")
                                {
                                    UidNumberTxt.Text = application.UIDNumber.ToString();
                                    string decr = EConnect.Utils.Security.Decryption.Decrypt(UidNumberTxt.Text);
                                    
                                
                                UidNumberTxt.Text = decr;
                                }
                                else
                                UidNumberTxt.Text = application.UIDNumber.ToString();
                                UidTypeDdl.Enabled = true;
                                UidNumberTxt.Enabled = true;
                               }
                        if (application == null)
                        {
                            TblFormDetail.Visible = false;
                            trcourse.Visible = false;
                            trproject.Visible = false;
                            lblerror.Visible = true;
                            //btnNext.Visible = false;
                            lblerror.Text = "Reference Number does not exist for Formal Course!!.";
                            strMessage = "Reference Number does not exist for Formal Course!!.";
                        }
                    }


                }
            }
        }
        catch (Exception ex)
        {
          throw ex ;
        }
    }

    protected void SaveData()
    {
        string OnlineRefNo = "";
        try
        {
            Boolean success = false;
            Int64 CourseId = Convert.ToInt64(ddlCourse.SelectedValue.Trim());
            Int64 batchid = Convert.ToInt64(ddlBatch.SelectedValue.Trim());

            Int32 Number = Convert.ToInt32(ddlsemester.SelectedItem.Text);
            Int32 semno = Number - 1;
 //           string AfflinstituteLinkCentreName = "";
            string StudentName = txtAppName.Text;
            string fatherName = txtFatherName.Text;
            string motherName = txtMotherName.Text;
            string Guardian = TxtGuardianName.Text;
            string genderName = ddl_gender.SelectedValue.Trim();
            DateTime DateOfBirth = Convert.ToDateTime(txtDob.Text);

            //Added for refNo
            EConnectContext context1 = new EConnectContext();
           // User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();

           // if (UserTypeid == 4)
 //           {
                NIELITMISContext context2 = new NIELITMISContext();
 //               var intituteslinkedToCentre = from s in context2.AffInstitutes
             //                                 where s.instituteID == loginUser.UserRefNumber
 //                                             select new { ID = s.ID, linkedToCentre = s.linkedToCentre };
 
 //               NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().ID);
                //HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
 //                NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
 //               lnkID = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
 //               NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
 //               if (institutesName != null)
 //               {
 //                   txtInstitute.Text = institutesName.Name;
 //                   AfflinstituteLinkCentreName = institutesName.Name;
 //                   Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);

 //               }
 //           }

            //CheckDuplicate AAdhaar for Fee reimbursement scheme
            if (ddlProcname.SelectedValue.ToString() == "2")
            {
                bool duplicateAAdhaar = false;
                duplicateAAdhaar = checkDuplicateAAdhaar();
                if (duplicateAAdhaar)
                {
                    ShowAlert("Aadhar already enrolled in some other running batch, Cannot register");
                    return;
                }
            }
            // else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (NIELITMISContext context = new NIELITMISContext())
                    {
                        int courseid = Convert.ToInt32(Request.QueryString["id"]);
                        // if (UserTypeid == 4)
             //           { 
             //               NameCenter = AfflinstituteLinkCentreName;
             //           }
             //           else
             //            {
                           // NielitCentres nielitcentre = context.NielitCentres.Find(loginUser.UserRefNumber);
             //               NameCenter = nielitcentre.Name;                         
             //           }

                        Int64 centreId = Convert.ToInt32(Session["EntityID"]);
                        NielitCentreStudent objRegistration;

                        objRegistration = new NielitCentreStudent();

                        var application = (from a in context.NielitCentreStudent

                                           where a.semesterid == Number && a.batch_ID == batchid && a.CourseID == CourseId && a.Name == StudentName && a.Gender == genderName && a.DateOfBirth == DateOfBirth && ((a.FatherName == fatherName && a.MotherName == motherName) || a.GuardianName == Guardian)
                                           select new
                                           {
                                               ID = a.ID,
                                               CourseId = a.CourseID,
                                               SemNo = a.semesterid

                                           }).FirstOrDefault();

                        if (application != null)
                        {
                            txtcode.Text = "";
                            ShowAlert("Record already exist for this semester No. Please select another semester No.");
                            return;
                        }

                        if (Number != 1)
                        {
                            var applicationcheck = (from a in context.NielitCentreStudent

                                                    where a.semesterid == semno && a.batch_ID == batchid && a.CourseID == CourseId && a.Name == StudentName && a.Gender == genderName && a.DateOfBirth == DateOfBirth && ((a.FatherName == fatherName && a.MotherName == motherName) || a.GuardianName == Guardian)
                                                    select new
                                                    {
                                                        ID = a.ID,
                                                        CourseId = a.CourseID,
                                                        SemNo = a.semesterid

                                                    }).FirstOrDefault();

                            if (applicationcheck == null)
                            {
                                txtcode.Text = "";
                                ShowAlert("Record not exist for previous semester No. Please select Previous semester No.");
                                return;
                            }
                        }

                        var updateLateralId = (from a in context.NielitCentreStudent

                                        where  a.batch_ID == batchid && a.CourseID == CourseId && a.WhetherLateralEntry==1 && a.Name == StudentName && a.Gender == genderName && a.DateOfBirth == DateOfBirth && ((a.FatherName == fatherName && a.MotherName == motherName) || a.GuardianName == Guardian)
                                        select new
                                        {
                                            ID = a.ID,
                                            CourseId = a.CourseID,
                                            SemNo = a.semesterid,
                                            UniversityRegNo = a.University_RegistrationNo,
                                           LateralEntry= a.WhetherLateralEntry

                                        }).FirstOrDefault();

                        if (updateLateralId != null)
                        {
                            if (updateLateralId.LateralEntry == 1)
                            {
                                var updateIdlate = (from a in context.NielitCentreStudent

                                                    where a.semesterid == 3 && a.batch_ID == batchid && a.CourseID == CourseId && a.Name == StudentName && a.Gender == genderName && a.DateOfBirth == DateOfBirth && ((a.FatherName == fatherName && a.MotherName == motherName) || a.GuardianName == Guardian)
                                                    select new
                                                    {
                                                        ID = a.ID,
                                                        CourseId = a.CourseID,
                                                        SemNo = a.semesterid,
                                                        UniversityRegNo = a.University_RegistrationNo,
                                                        LateralEntry = a.WhetherLateralEntry

                                                    }).FirstOrDefault();
                                if (updateIdlate != null)
                                {
                                    objRegistration.CandidateID = updateIdlate.ID;
                                    objRegistration.University_RegistrationNo = updateIdlate.UniversityRegNo;
                                    objRegistration.WhetherLateralEntry = updateIdlate.LateralEntry;
                                    objRegistration.CourseID = Convert.ToInt64(ddlCourse.SelectedValue);
                                    objRegistration.batch_ID = Convert.ToInt64(ddlBatch.SelectedValue);
                                    objRegistration.ApplicationDate = DateTime.Now;
                                    objRegistration.InstituteID = centreId;
                                    objRegistration.whetherAffiliated = "O";

                                    objRegistration.AlreadyRegistered = false;

                                    objRegistration.AlreadyQualified = false;
                                    objRegistration.Roll_No = null;
                                    objRegistration.semesterid = Convert.ToInt32(ddlsemester.SelectedItem.Text);
                                    string salutation = ddlSalutaionName.SelectedItem.Text;
                                    salutation = salutation.Substring(0, salutation.IndexOf('/'));

                                    if (ddlWheatherPojectStu.SelectedValue == "1")
                                    {
                                        objRegistration.whetherProjectStudent = true;
                                        objRegistration.projectId = Convert.ToInt64(ddlProcname.SelectedValue);
                                    }
                                    else
                                    {
                                        objRegistration.whetherProjectStudent = false;
                                    }

                                    objRegistration.Salutation = salutation.Trim();
                                    objRegistration.Name = txtAppName.Text.Trim().ToString();
                                    if (Rdoownertype.SelectedValue == "P")
                                    {
                                        objRegistration.FatherName = txtFatherName.Text.Trim();
                                        objRegistration.MotherName = txtMotherName.Text.Trim();
                                        objRegistration.GuardianName = null;
                                    }
                                    else if (Rdoownertype.SelectedValue == "G")
                                    {
                                        objRegistration.GuardianName = TxtGuardianName.Text.Trim();
                                        objRegistration.FatherName = null;
                                        objRegistration.MotherName = null;
                                    }

                                    string gender = ddl_gender.SelectedValue;
                                    objRegistration.Gender = gender.Trim();

                                    if (RdisEWS.SelectedValue == "Y")
                                    objRegistration.Is_EWS = true;
                                    else
                                    objRegistration.Is_EWS = false;
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
                                    objRegistration.PerCountryID = 0;
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
                                        objRegistration.CorCountryID = 0;
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
                                        objRegistration.CorCountryID = 0;
                                        objRegistration.CorCityName = TxtCorCity.Text;
                                        objRegistration.CorStateID = Convert.ToInt32(ddlCorState.SelectedValue);
                                        objRegistration.CorDistrictID = Convert.ToInt32(ddlcordistrict.SelectedValue);
                                        objRegistration.CorPinCode = Convert.ToInt32(txtCorPinCode.Text);
                                    }

                                    objRegistration.IsVerifiedByInstitute = false;
                                    DateTime Dob = Convert.ToDateTime(txtDob.Text);
                                    Int32 ReligionId = Convert.ToInt32(ddlReligion.SelectedValue);
                                    Int32 castCategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                                    Boolean IsHandicapped = Rdhandicapped.SelectedValue == "Y" ? true : false;

                                    objRegistration.ReligionID = ReligionId;

                                    //aadhar details     
                                    objRegistration.UIDType = Convert.ToInt32(UidTypeDdl.SelectedValue);
                                    //Added for Aadhar Encryption
                                    if (UidTypeDdl.SelectedValue.ToString() == "1")
                                        objRegistration.UIDNumber = EConnect.Utils.Security.Encryption.Encrypt(UidNumberTxt.Text.Trim().ToUpper());
                                    else
                                        objRegistration.UIDNumber = UidNumberTxt.Text.Trim().ToUpper();

                                    objRegistration.AadharVerfied = false;

                                    objRegistration.whether_Course_Complete = false;
                                    objRegistration.whether_Certificate_Issued = false;
                                    objRegistration.certificate_Issue_Date = null;
                                    objRegistration.whetherPlaced = false;
                                    objRegistration.company_Name = 0;
                                    objRegistration.comapny_Address = "";
                                    objRegistration.comapny_Address2 = "";
                                    objRegistration.comapny_Address3 = "";
                                    objRegistration.CompnyCityName = "";
                                    objRegistration.CompnyCountry_ID = 0;
                                    objRegistration.compnyState_ID = 0;
                                    objRegistration.compnyDistrict_ID = 0;
                                    objRegistration.CompnyPinCode = 0;
                                    objRegistration.affidavit_Date = null;
                                    objRegistration.affidavit_No = "";
                                    objRegistration.affidavit_Verified = false;

                                    objRegistration.FinalSubmissionDate = DateTime.Now;
                                    objRegistration.FinalSubmitted = true;
                                  //  objRegistration.enter_By = Convert.ToInt32(Session["UserID"]);
                                    objRegistration.enter_Date = DateTime.Now;

                                    context.NielitCentreStudent.Add(objRegistration);
                                    context.SaveChanges();

                                    long Appid = objRegistration.ID;
                                    success = true;
                                    if (success == true)
                                    {
                                        objRegistration = context.NielitCentreStudent.Find(Convert.ToInt32(Appid));
                                        objRegistration.Number = NameCenter + "-" + Appid.ToString();
                                        OnlineRefNo = NameCenter + "-" + Appid.ToString();
                                        context.SaveChanges();
                                        scope.Complete();
                                        strMessage = "New record saved. Reference No. for Student Registered : " + NameCenter + "-" + Appid.ToString();
                                    }
                                }
                            }
                        }
                        else  if (updateLateralId == null)
                        {
                             
                                var updateId = (from a in context.NielitCentreStudent

                                                where a.semesterid == 1 && a.batch_ID == batchid && a.CourseID == CourseId && a.Name == StudentName && a.Gender == genderName && a.DateOfBirth == DateOfBirth && ((a.FatherName == fatherName && a.MotherName == motherName) || a.GuardianName == Guardian)
                                                select new
                                                {
                                                    ID = a.ID,
                                                    CourseId = a.CourseID,
                                                    SemNo = a.semesterid,
                                                    UniversityRegNo = a.University_RegistrationNo,
                                                    LateralEntry = a.WhetherLateralEntry

                                                }).FirstOrDefault();
                                if (updateId != null)
                                {
                                    objRegistration.CandidateID = updateId.ID;
                                    objRegistration.University_RegistrationNo = updateId.UniversityRegNo;
                                    objRegistration.WhetherLateralEntry = updateId.LateralEntry;
                                    objRegistration.CourseID = Convert.ToInt64(ddlCourse.SelectedValue);
                                    objRegistration.batch_ID = Convert.ToInt64(ddlBatch.SelectedValue);
                                    objRegistration.ApplicationDate = DateTime.Now;
                                    objRegistration.InstituteID = centreId;
                                    objRegistration.whetherAffiliated = "O";

                                    objRegistration.AlreadyRegistered = false;

                                    objRegistration.AlreadyQualified = false;
                                    objRegistration.Roll_No = null;
                                    objRegistration.semesterid = Convert.ToInt32(ddlsemester.SelectedItem.Text);
                                    string salutation = ddlSalutaionName.SelectedItem.Text;
                                    salutation = salutation.Substring(0, salutation.IndexOf('/'));

                                    if (ddlWheatherPojectStu.SelectedValue == "1")
                                    {
                                        objRegistration.whetherProjectStudent = true;
                                        objRegistration.projectId = Convert.ToInt64(ddlProcname.SelectedValue);
                                    }
                                    else
                                    {
                                        objRegistration.whetherProjectStudent = false;
                                    }

                                    objRegistration.Salutation = salutation.Trim();
                                    objRegistration.Name = txtAppName.Text.Trim().ToString();
                                    if (Rdoownertype.SelectedValue == "P")
                                    {
                                        objRegistration.FatherName = txtFatherName.Text.Trim();
                                        objRegistration.MotherName = txtMotherName.Text.Trim();
                                        objRegistration.GuardianName = null;
                                    }
                                    else if (Rdoownertype.SelectedValue == "G")
                                    {
                                        objRegistration.GuardianName = TxtGuardianName.Text.Trim();
                                        objRegistration.FatherName = null;
                                        objRegistration.MotherName = null;
                                    }

                                    string gender = ddl_gender.SelectedValue;
                                    objRegistration.Gender = gender.Trim();

                                    if (RdisEWS.SelectedValue == "Y")
                                        objRegistration.Is_EWS = true;
                                    else
                                        objRegistration.Is_EWS = false;
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


                                    //objRegistration.BodyMark = txtBodyMark.Text;

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
                                    objRegistration.PerCountryID = 0;
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
                                        objRegistration.CorCountryID = 0;
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
                                        objRegistration.CorCountryID = 0;
                                        objRegistration.CorCityName = TxtCorCity.Text;
                                        objRegistration.CorStateID = Convert.ToInt32(ddlCorState.SelectedValue);
                                        objRegistration.CorDistrictID = Convert.ToInt32(ddlcordistrict.SelectedValue);
                                        objRegistration.CorPinCode = Convert.ToInt32(txtCorPinCode.Text);
                                    }

                                    objRegistration.IsVerifiedByInstitute = false;
                                    DateTime Dob = Convert.ToDateTime(txtDob.Text);
                                    Int32 ReligionId = Convert.ToInt32(ddlReligion.SelectedValue);
                                    Int32 castCategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                                    Boolean IsHandicapped = Rdhandicapped.SelectedValue == "Y" ? true : false;

                                    objRegistration.ReligionID = ReligionId;

                                    //aadhar details     
                                    objRegistration.UIDType = Convert.ToInt32(UidTypeDdl.SelectedValue);
                                    //Added for Aadhar Encryption
                                    if (UidTypeDdl.SelectedValue.ToString() == "1")
                                        objRegistration.UIDNumber = EConnect.Utils.Security.Encryption.Encrypt(UidNumberTxt.Text.Trim().ToUpper());
                                    else
                                        objRegistration.UIDNumber = UidNumberTxt.Text.Trim().ToUpper();

                                    objRegistration.AadharVerfied = false;

                                    objRegistration.whether_Course_Complete = false;
                                    objRegistration.whether_Certificate_Issued = false;
                                    objRegistration.certificate_Issue_Date = null;
                                    objRegistration.whetherPlaced = false;
                                    objRegistration.company_Name = 0;
                                    objRegistration.comapny_Address = "";
                                    objRegistration.comapny_Address2 = "";
                                    objRegistration.comapny_Address3 = "";
                                    objRegistration.CompnyCityName = "";
                                    objRegistration.CompnyCountry_ID = 0;
                                    objRegistration.compnyState_ID = 0;
                                    objRegistration.compnyDistrict_ID = 0;
                                    objRegistration.CompnyPinCode = 0;
                                    objRegistration.affidavit_Date = null;
                                    objRegistration.affidavit_No = "";
                                    objRegistration.affidavit_Verified = false;
                                    objRegistration.FinalSubmissionDate = DateTime.Now;
                                    objRegistration.FinalSubmitted = true;
                                   // objRegistration.enter_By = Convert.ToInt32(Session["UserID"]);
                                    objRegistration.enter_Date = DateTime.Now;
                                    context.NielitCentreStudent.Add(objRegistration);
                                    context.SaveChanges();
                                    long Appid = objRegistration.ID;
                                    success = true;
                                    if (success == true)
                                    {
                                        objRegistration = context.NielitCentreStudent.Find(Convert.ToInt32(Appid));
                                        objRegistration.Number = NameCenter + "-" + Appid.ToString();
                                        OnlineRefNo = NameCenter + "-" + Appid.ToString();
                                        context.SaveChanges();
                                        scope.Complete();

                                        strMessage = "New record saved. Reference No. for Student Registered : " + NameCenter + "-" + Appid.ToString();
                                    }
                                }                            
                        }
                    };

                    Response.Redirect("NielitStudentFormalEdit_HO.aspx?msg=" + strMessage, true);
                }
            }
        }
        catch (DbEntityValidationException e)
        {
            foreach (var eve in e.EntityValidationErrors)
            {
                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        ve.PropertyName, ve.ErrorMessage);
                }
            }
            //throw;

            throw e;
        }
    }

    protected void btnSaveNext_Click(object sender, System.EventArgs e)
    {
        try
        {
            if (IsValidForm())
            {
                SaveData();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnNext_Click(object sender, System.EventArgs e)
    {
        try
        {
            ddlsemester.Enabled = true;
            btnSave.Visible = false;
            btnSaveNext.Visible = true;
            ddlsemester.Items.Clear();
            ListItem lst = new ListItem("--Select--", "0");
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int64 coursenameid = Convert.ToInt64(ddlCourse.SelectedValue);
                Int64 batchid = Convert.ToInt64(ddlBatch.SelectedValue);
                string OnlineRefNo = Convert.ToString(txtrefno.Text);

                var applicationcheckLateral = (from a in context.NielitCentreStudent

                                                        where a.WhetherLateralEntry == 1 && a.Number==OnlineRefNo
                                                        select new
                                                        {
                                                            ID = a.ID,
                                                            WetherLateralEntry=a.WhetherLateralEntry

                                                        }).FirstOrDefault();

                if (applicationcheckLateral == null)
                {
                    if (coursenameid != 0)
                    {
                        using (DataTable dt = GetBatchesForSemesterMaster())
                        {
                            if (dt.Rows.Count > 0)
                            {
                                int i = Convert.ToInt32(dt.Rows[0]["SemNo"]);
                                int n = i;
                                for (i = 0; i <= n; i++)
                                {
                                    if (i == 0)
                                    {
                                        ddlsemester.Items.Add(new ListItem("Select", "0"));
                                    }
                                    else
                                    {
                                        ddlsemester.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                        ddlsemester.DataBind();
                                    }
                                }
                            }
                        }
                    }
                }
                else if (applicationcheckLateral != null)
                    if (coursenameid != 0)
                    {
                        using (DataTable dt = GetBatchesForSemesterMaster())
                        {
                            if (dt.Rows.Count > 0)
                            {
                                int i = Convert.ToInt32(dt.Rows[0]["SemNo"]);
                                int n = i;
                                for (i = 3; i <= n; i++)
                                {
                                    if (i == 0)
                                    {
                                        ddlsemester.Items.Add(new ListItem("Select", "0"));
                                    }
                                    else
                                    {
                                        ddlsemester.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                        ddlsemester.DataBind();
                                    }
                                }
                            }
                        }
                    }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
 protected void bindAllProject()
    {
        try
        {
            string Number = txtrefno.Text;
            Int64 cID = Convert.ToInt64(ddlCourse.SelectedValue);
            //  Int64 cID = 6044;
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");




                var Proc = from t in context.NielitProjectss 
                           join k in context.NielitProjCoursess on t.ID equals k.projID
                           join d in context.NielitCourseDurations on k.courseID equals d.ID
                           where d.ID == cID 
                           && k.IsActive
                           orderby (t.ProjectName)
                           select new { ValueField = t.ID, TextField = t.ProjectName };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlProcname, Proc, lst);



            }

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void HiddenField2_ValueChanged(object sender, EventArgs e)
    {

    }
}