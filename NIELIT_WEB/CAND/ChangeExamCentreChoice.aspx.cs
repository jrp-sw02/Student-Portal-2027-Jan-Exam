using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Mail;
using System.Data.Entity.Validation;
using System.Drawing;


public partial class ReportPgae : BasePage
{
    EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
    SqlDataAdapter da = new SqlDataAdapter();   


    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.CacheControl = "no-cache";
        //Response.AddHeader("Progra", "no-cache");
        //Response.Expires = -1500;
        try
        {
           
            //if (!IsSessionAlive())
            //{
            //    Response.Redirect("~/index.aspx");
            //}
            //currentRoleId = Convert.ToInt32(Session["RoleID"]);           
            ////if (Request.UrlReferrer == null)
            ////{
            ////    Response.Write(GeInvalidRequestMessage("Goto Home Page", "../MainPage.aspx"));
            ////    Response.End();
            ////    return;
            ////}
            ////else
            ////{
            ////    string pageRef = Request.UrlReferrer.ToString();
            ////    //if (!pageRef.Contains ("https://student.nielit.gov.in/"))
            ////    //{
            ////    //    Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
            ////    //    Response.End();
            ////    //    return;
            ////    //}
            ////}
            //dataDownloadedSequenceId = Convert.ToInt32(Request.QueryString["dataDownloadedSequenceId"]);
            //courseId = Convert.ToInt32(Request.QueryString["courseId"]);
            //phaseId = Convert.ToInt32(Request.QueryString["phaseId"]);
            //dataDownloadedSequenceId = 1212534;
            //courseId = 0; //115
            //phaseId = 0; //13

            if (!Page.IsPostBack)
            {                
                BindState();                
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }  
    
    public static void GenerateMobileOTP(string entityID)
	{
		try
		{

			Int32 OTP = Convert.ToInt32(EConnect.CommonFunctions.GenerateRandomNumber(6));
			using (EConnectContext context = new EConnectContext())
			{
                        var save_otp = context.ExamCentreChoiceRefillings.Where(s => s.ApplicationId == entityID).FirstOrDefault();
					    var mobile = context.CertificateExamApplications.Where(s => s.Number == entityID).FirstOrDefault();
					    Int64 mobile_number = Convert.ToInt64(mobile.MobileNumber);
                        var name = mobile.Name;
                        var email = mobile.EmailAddress;
                        save_otp.Otp_Number = OTP;
                        context.Entry(save_otp).State = System.Data.Entity.EntityState.Modified;
						EConnect.NIELIT.SMS message = new SMS("OTP for Exam Cycle/Centre change verification is " + OTP.ToString() + ". Reference Number:" + DateTime.Now.ToString("hhmmss") + ". Use this OTP to complete verification process",mobile_number.ToString(),"1307161053015859885", SmsServiceType.SignleSMS);
						int sentMessageCount;
						message.sendOTPMSG(out sentMessageCount);
                        if (sentMessageCount == 1)
                        {
                            context.SaveChanges();
                            //ShowAlert("Please select the declaration....");
                        }
						if (sentMessageCount != 1)													
						{
                            String msg = "Dear Admin<SMS could not be sent to the mobile number " + mobile_number.ToString() + " on " + DateTime.Now.ToString() + ".<BR>Reaon: 0 message sent.";
							SendEMailToAdminOnSmsFailed(msg);
							throw new Exception("OTP could not be sent due to some technical problem. Please try again later");
						}
                        string email_msg = "Dear " + name.ToString() + ",<br/><br/>" + "OTP for Exam Cycle/Centre change verification is " + OTP.ToString() + ". Reference Number:" + DateTime.Now.ToString("hhmmss") + ". Use this OTP to complete verification process<br/><br/><br/><br/><br/>Thank You,<br/> NIELIT";
                        EConnect.NIELIT.Email mail = new Email("OTP for Exam Cycle/Centre change verification :NIELIT", email_msg, email);
                        try
                        {
                            mail.Send();
                        }
                        catch (Exception)
                        {
                            throw new Exception("OTP has been sent to your registered E-mail ID. Please check junk/spam mail if you not received this mail in your inbox");
                            //throw new Exception("OTP could not be sent due to some technical problem. Please try again later");
                        }
			};

		}
		catch (Exception ex)
		{ throw ex; }
	}

    public static void SendEMailToAdminOnSmsFailed(String msg)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var emailAddress = context.Organizations.Find(1).EmailTechnicalPerson;
                if (emailAddress != null)
                {
                    EConnect.NIELIT.Email mail = new Email("New Exam Centre allocation for DLC Candidate:NIELIT", msg, emailAddress);
                    mail.Send();
                }
            };
        }
        catch (Exception)
        {
        }
    }

    public static void SendSmsToAdminOnEmailFailed(String msg)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var mobileNumber = context.Organizations.Find(1).MobileNumberTechnicalPerson;
                if (mobileNumber.HasValue)
                {
                    EConnect.NIELIT.SMS message = new SMS(msg, mobileNumber.Value.ToString(),"test", SmsServiceType.SignleSMS);
                    int sentMessageCount;
                    message.sendSingleSMS(out sentMessageCount);
                }
            };
        }
        catch (Exception) { }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                btnSubmit.Enabled = false;
                string applicationNo = appId.Text.ToString();
                DateTime dob = Convert.ToDateTime(txtDob.Text);               

                BindExamCycle(applicationNo);
                lbl_message.Text = lbl_save_option.Text == "O" ? "Do you wish to change exam cycle?" : "Do you wish to change exam centre?";

                var verify_candidate = (from ce in context.CertificateExamApplications
                                        where ce.Number == applicationNo                                        
                                        select new {dateofBirth = ce.DateOfBirth }).FirstOrDefault();

                if (verify_candidate != null)
                {
                    if (!(verify_candidate.dateofBirth == dob))
                    {
                        Lblerror.Visible = true;
                        Lblerror.Text = "Kindly verify your Date of Birth in online examination form.";
                        appId.Text = "";
                        txtDob.Text = "";
                        btnSubmit.Enabled = true; 
                        return;
                    }
                }
                else
                {
                    Lblerror.Visible = true;
                    Lblerror.Text = "Kindly verify your Application Number.";
                    appId.Text = "";
                    txtDob.Text = "";
                    btnSubmit.Enabled = true;
                    return;
                }
                
                if (context.ExamCentreChoiceRefillings.Any(c => c.ApplicationId == applicationNo && c.Validation_Flag == "Y" && (c.Agreed_Disagreed_Flag=="N"||c.Agreed_Disagreed_Flag=="Y")))
                {
                    Lblerror.Visible = true;
                    Lblerror.Text = "You have already availed this facility.";
                    return;
                    //Lblerror.BackColor = Color.LightGreen;
                    //Lblerror.ForeColor = Color.DarkRed;                    
                }

                else if (!context.ExamCentreChoiceRefillings.Any(c => c.ApplicationId == applicationNo && c.Validation_Flag == "Y"))             
                {
                    //if (!context.ExamCentreChoiceRefillings.Any(c => c.ApplicationId == applicationNo && c.Agreed_Disagreed_Flag == "N"))
                    //{
                        var result = (from ce in context.CertificateExamApplications
                                      join ex in context.Exams on ce.ExamID equals ex.ID
                                      where ce.ResultGradeID == null
                                      && ce.RollNumber == null
                                      && (ce.PaymentStatusID == 2 || ce.PaymentStatusID == 4)
                                      && (ex.ExamMonth == 4 || ex.ExamMonth == 5 || ex.ExamMonth == 6 || ex.ExamMonth == 8 || ex.ExamMonth == 9)
                                      && ex.ExamYear == 2020
                                      && ex.CourseCategoryID == 2
                                      && ce.ApplicationStatusID == 17
                                      && ce.FinalSubmitted == true
                                      && ce.DemandNoteID != null
                                      && ce.Number == applicationNo
                                      && System.Data.Entity.DbFunctions.TruncateTime(ce.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)

                                      select new { name = ce.Number, dateofBirth = ce.DateOfBirth, centre1 = ce.ExamCenter1ID, centre2 = ce.ExamCenter2ID, examid = ce.ExamID, candidatename = ce.Name }).FirstOrDefault();
                        ExamCenter centre1 = context.ExamCenters.Find(result.centre1);
                        ExamCenter centre2 = context.ExamCenters.Find(result.centre2);

                        Exam exam_id = context.Exams.Find(result.examid);

                        if (result == null)
                        {
                            Lblerror.Visible = true;
                            Lblerror.Text = "You are not eligible for avail this facility.";
                            return;
                        }
                        else
                        {
                            Lblerror.Visible = false;
                            Lblerror.Text = "";
                            lblcentre1.Text = centre1.Name;
                            tr_centre1.Visible = true;
                            lblcentre2.Text = centre2.Name;
                            tr_centre2.Visible = true;
                            //tr_dec.Visible = true;
                            //tr_dec_check.Visible = true;
                            tr_change_centre.Visible = true;
                            lbl_candidate_name.Text = result.candidatename.ToString().Trim();
                            tr_candidate_name.Visible = true;
                            tr_exam_id.Visible = true;                            
                            lbl_examid.Text = exam_id.Name;
                            lbl_candidate_name0.Text = result.candidatename.ToString().Trim();
                        }
                    //}
                    //else 
                    //{
                    //    Lblerror.Visible = true;
                    //    Lblerror.Text = "Earlier, You opted No changes in Exam Centre. So, Now you are not eligible for any changes.";
                    //}
                }
                else
                {
                    Lblerror.Visible = true;
                    Lblerror.Text = "You have already availed this facility.";
                    return;
                    //Lblerror.BackColor = Color.LightGreen;
                    //Lblerror.ForeColor = Color.DarkRed;
                }
                
            }
        }
        catch
        {

        }
    }

    protected void BindState()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {                                
                var examState = (from g in context.CourseWiseStates
                                 join s in context.Locations on g.StateID equals s.ID
                                 join c in context.Courses on g.CourseID  equals c.ID 
                                 where s.LocationTypeID == 2
                                 && s.ParentLocationID == 1
                                 && c.CourseCategoryID == 2
                                
                                 select new { ValueField = s.ID, TextField = s.Name }).Distinct();
                examState = examState.OrderBy(s => s.TextField);                
                EConnect.Utils.Common.ControlUtility.BindListObject(DdlExamCentreState1, examState, new ListItem("--Select State--", "0"));

            }
        }
        catch
        {

        }     
    }

    protected void BindExamCycle(string appNum)
    {
        //string applicationNo = appId.Text.ToString();
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var examcycle = (from c in context.CertificateExamApplications
                                 join e in context.Exams on c.CourseID equals e.CourseID                                 
                                 where (e.ExamMonth==10 || e.ExamMonth==11)
                                 && e.ExamYear == 2020 && c.Number == appNum
                                 select new { ValueField = e.ID, TextField = e.Name }).Distinct();
                examcycle = examcycle.OrderBy(s => s.ValueField);                
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl_examcycle, examcycle, new ListItem("--Select ExamCycle--", "0"));
                
            }
        }
        catch
        {

        }

    }

    protected void BindExamCentre(Int64 StateID, Int32 CentreId, DropDownList ddl, enmExamCenterType examCentreType)
    {
        try
        {     
            //Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);            
            Int32 typeID = Convert.ToInt32(examCentreType);
            ListItem lst = new ListItem("--Select Location--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                string applicationNo = appId.Text.ToString();
                DateTime dob = Convert.ToDateTime(txtDob.Text);
                //lbl_message.Text = lbl_save_option.Text == "O" ? "Do you wish to change exam cycle?" : "Do you wish to change exam centre?";


                var result = (from ce in context.CertificateExamApplications
                              join ex in context.Exams on ce.ExamID equals ex.ID
                              where ce.ResultGradeID == null
                              && ce.RollNumber == null
                              && (ce.PaymentStatusID == 2 || ce.PaymentStatusID == 4)
                              && (ex.ExamMonth == 4 || ex.ExamMonth == 5 || ex.ExamMonth == 6 || ex.ExamMonth == 8 || ex.ExamMonth == 9)
                              && ex.ExamYear == 2020
                              && ex.CourseCategoryID == 2
                              && ce.ApplicationStatusID == 17
                              && ce.FinalSubmitted == true
                              && ce.DemandNoteID != null
                              && ce.Number == applicationNo
                              && System.Data.Entity.DbFunctions.TruncateTime(ce.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                              select new {primarycentre = ce.ExamCenter1ID}).FirstOrDefault();

                //Int32 primeCentre = result.primaycentre;
                Int32 primeCentre = lbl_save_option.Text == "C" ? result.primarycentre : 0;


                int courseCategoryID = 2;
                var examcentre = context.ExamCenters.Where(s => s.StateID == StateID && s.CourseCategoryID == courseCategoryID && s.IsEnabled == true && s.ID != primeCentre)
                                                    .OrderBy(s => s.Name).Select(s => new { ValueField = s.ID, TextField = s.Code.ToUpper() + " - " + s.Name.ToUpper(), examCentreTypeID = s.ExamCentreTypeID });
                examcentre = examcentre.Where(e => e.examCentreTypeID == typeID);
                //var test2 = examcentre.ToList();
                ddl.Items.Clear();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, examcentre, lst);
            };
        }
        catch (Exception ex) { throw ex; }
    }    

    protected void DdlExamCentreState1_SelectedIndexChanged(object sender, EventArgs e)
    {        
       //        BindLocation(Convert.ToInt32(DdlExamCentreState1.SelectedValue.Trim()));
        BindExamCentre(Convert.ToInt64(DdlExamCentreState1.SelectedValue), 0, DdlExamCentre1, enmExamCenterType.Both);        
    }

    protected void btn_yes_Click(object sender, EventArgs e)
    {
            tr11.Visible = true;
            tr12.Visible = true;
            tr_new_loc.Visible = true;
            tr_final_save.Visible = true;
            btn_no.Enabled = false;
            btn_yes.Enabled = false;
            tr_change_centre.Visible = false;
            //tr_examcycle.Visible = true;
            if (lbl_save_option.Text == "O")
            {
                tr_examcycle.Visible = true;
            }
            else
            {
                tr_examcycle.Visible = false;
            }
    }

    protected void btn_no_Click(object sender, EventArgs e)
    {        
            //tr_change_centre.Visible = false;
            btn_yes.Enabled = false;
            btn_no.Enabled = false;
            tr_examcycle.Visible = false;
            string applNumber = appId.Text.ToString().Trim(); 
            try
            {
                using (EConnectContext context = new EConnectContext())
                {
                    if (!context.ExamCentreChoiceRefillings.Any(c => c.ApplicationId == applNumber))
                    {
                        string applicationNo = appId.Text.ToString();
                        DateTime dob = Convert.ToDateTime(txtDob.Text);

                        var result = (from ce in context.CertificateExamApplications
                                      join ex in context.Exams on ce.ExamID equals ex.ID
                                      where ce.ResultGradeID == null
                                      && ce.RollNumber == null
                                      && (ce.PaymentStatusID == 2 || ce.PaymentStatusID == 4)
                                      && (ex.ExamMonth == 4 || ex.ExamMonth == 5 || ex.ExamMonth == 6 || ex.ExamMonth == 8 || ex.ExamMonth == 9)
                                      && ex.ExamYear == 2020
                                      && ex.CourseCategoryID == 2
                                      && ce.ApplicationStatusID == 17
                                      && ce.FinalSubmitted == true
                                      && ce.DemandNoteID != null
                                      && ce.Number == applicationNo
                                      && System.Data.Entity.DbFunctions.TruncateTime(ce.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)

                                      select new { name = ce.Number, dateofBirth = ce.DateOfBirth, centre1 = ce.ExamCenter1ID, centre2 = ce.ExamCenter2ID, examid = ce.ExamID, candidatename = ce.Name }).FirstOrDefault();

                        ExamCentreChoiceRefilling application = new ExamCentreChoiceRefilling();

                        application.ApplicationId = applNumber;
                        application.ExamId = result.examid;
                        application.ExamCentre1_ID = result.centre1;
                        application.ExamCentre2_ID = result.centre2;
                        application.NewExamCentre_ID = 0;
                        application.New_Exam_Id = result.examid;
                        application.Agreed_Disagreed_Flag = "N";
                        application.Agreed_Disagreed_Timestamp = DateTime.Now;
                        application.Centrechange_Optout = lbl_save_option.Text == "O" ? "O" : "C";
                        context.ExamCentreChoiceRefillings.Add(application);
                        context.SaveChanges();
                        GenerateMobileOTP(applNumber);
                        //Lblerror.Visible = true;
                        //Lblerror.Text = "You have not opted any new centre. Thank You....";
                        tr_enter_otp.Visible = true;
                        tr_validate.Visible = true;
                        tr_validate_otp.Visible = true;
                       
                    }
                    else if (context.ExamCentreChoiceRefillings.Any(c => c.ApplicationId == applNumber && (c.Agreed_Disagreed_Flag == "Y" || c.Agreed_Disagreed_Flag == "N") && (c.Validation_Flag == "N" || c.Validation_Flag == null)))
                    {
                        string applicationNo = appId.Text.ToString();
                        DateTime dob = Convert.ToDateTime(txtDob.Text);

                        var result = (from ce in context.CertificateExamApplications
                                      join ex in context.Exams on ce.ExamID equals ex.ID
                                      where ce.ResultGradeID == null
                                      && ce.RollNumber == null
                                      && (ce.PaymentStatusID == 2 || ce.PaymentStatusID == 4)
                                      && (ex.ExamMonth == 4 || ex.ExamMonth == 5 || ex.ExamMonth == 6 || ex.ExamMonth == 8 || ex.ExamMonth == 9)
                                      && ex.ExamYear == 2020
                                      && ex.CourseCategoryID == 2
                                      && ce.ApplicationStatusID == 17
                                      && ce.FinalSubmitted == true
                                      && ce.DemandNoteID != null
                                      && ce.Number == applicationNo
                                      && System.Data.Entity.DbFunctions.TruncateTime(ce.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)

                                      select new { name = ce.Number, dateofBirth = ce.DateOfBirth, centre1 = ce.ExamCenter1ID, centre2 = ce.ExamCenter2ID, examid = ce.ExamID, candidatename = ce.Name }).FirstOrDefault();

                        

                        var application_change = context.ExamCentreChoiceRefillings.Where(s => s.ApplicationId == applNumber).FirstOrDefault();
                        application_change.Agreed_Disagreed_Flag = "N";
                        application_change.NewExamCentre_ID = 0;
                        application_change.New_Exam_Id = result.examid;
                        application_change.Agreed_Disagreed_Timestamp = DateTime.Now;
                        application_change.Centrechange_Optout = lbl_save_option.Text == "O" ? "O" : "C";
                        context.Entry(application_change).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        GenerateMobileOTP(applNumber);
                        tr_enter_otp.Visible = true;
                        tr_validate.Visible = true;
                        tr_validate_otp.Visible = true;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message);
            }
        
    }

    //protected void chkdisclamier_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chk.Checked == true)
    //    {
    //        tr_change_centre.Visible = true;
    //    }
    //}

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (chk.Checked == false)
            {
                ShowAlert("Please select the declaration....");
                return;
            }
            
            if (ddl_examcycle.SelectedValue == "0")
            {
                if (tr_examcycle.Visible == true)
                {
                    ShowAlert("Please select the Exam Cycle....");
                    return;
                }
            }
            if (DdlExamCentreState1.SelectedValue== "0")
            {                
                ShowAlert("Please select the Exam State....");
                return;
            }
            if (DdlExamCentre1.SelectedValue == "0")
            {
                ShowAlert("Please select the Exam Location....");
                return;
            }
            else
            {
                btnSave.Enabled = false;
                string applNumber = appId.Text.ToString().Trim();
                Int32 newCentreID = Convert.ToInt32(DdlExamCentre1.SelectedValue.Trim());
                Int32 newExamCycle = Convert.ToInt32(ddl_examcycle.SelectedValue.Trim());
                string agreedDisagreedFlag = "Y";
                //Int32 applicantTypeId = (RdoAppliedAs.SelectedValue == "I") ? Convert.ToInt32(enmApplicantType.Institute) : Convert.ToInt32(enmApplicantType.Direct);

                using (EConnectContext context = new EConnectContext())
                {
                    string applicationNo = appId.Text.ToString();
                    DateTime dob = Convert.ToDateTime(txtDob.Text);

                    var result = (from ce in context.CertificateExamApplications
                                  join ex in context.Exams on ce.ExamID equals ex.ID
                                  where ce.ResultGradeID == null
                                  && ce.RollNumber == null
                                  && (ce.PaymentStatusID == 2 || ce.PaymentStatusID == 4)
                                  && (ex.ExamMonth == 4 || ex.ExamMonth == 5 || ex.ExamMonth == 6 || ex.ExamMonth == 8 || ex.ExamMonth == 9)
                                  && ex.ExamYear == 2020
                                  && ex.CourseCategoryID == 2
                                  && ce.ApplicationStatusID == 17
                                  && ce.FinalSubmitted == true
                                  && ce.DemandNoteID != null
                                  && ce.Number == applicationNo
                                  && System.Data.Entity.DbFunctions.TruncateTime(ce.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)

                                  //&& c.DateOfBirth = dob
                                  select new { name = ce.Number, dateofBirth = ce.DateOfBirth, centre1 = ce.ExamCenter1ID, centre2 = ce.ExamCenter2ID, examid = ce.ExamID, candidatename = ce.Name }).FirstOrDefault();

                    if (!context.ExamCentreChoiceRefillings.Any(c => c.ApplicationId == applicationNo))
                    {                     

                        ExamCentreChoiceRefilling application = new ExamCentreChoiceRefilling();

                        application.ApplicationId = applNumber;
                        application.ExamId = result.examid;
                        application.ExamCentre1_ID = result.centre1;
                        application.ExamCentre2_ID = result.centre2;
                        application.NewExamCentre_ID = newCentreID;
                        application.New_Exam_Id = newExamCycle == 0 ? result.examid : newExamCycle;
                        application.Agreed_Disagreed_Flag = agreedDisagreedFlag;
                        application.Agreed_Disagreed_Timestamp = DateTime.Now;
                        application.Centrechange_Optout = lbl_save_option.Text == "O" ? "O" : "C";
                        //application.Validation_Timestamp = null;
                        context.ExamCentreChoiceRefillings.Add(application);
                        context.SaveChanges();
                        Context.ApplicationInstance.CompleteRequest();
                        GenerateMobileOTP(applNumber);

                        DdlExamCentreState1.Enabled = false;
                        DdlExamCentre1.Enabled = false;
                        ddl_examcycle.Enabled = false;
                        btnSubmit.Enabled = false;
                        btn_yes.Enabled = false;
                        btn_no.Enabled = false;
                        tr_enter_otp.Visible = true;
                        tr_validate.Visible = true;
                    }

                    else if (context.ExamCentreChoiceRefillings.Any(c => c.ApplicationId == applicationNo && (c.Agreed_Disagreed_Flag == "N" || c.Agreed_Disagreed_Flag == "Y") && (c.Validation_Flag == "N" || c.Validation_Flag == null)))
                    {
                        var application_change = context.ExamCentreChoiceRefillings.Where(s => s.ApplicationId == applNumber).FirstOrDefault();                       
                        application_change.NewExamCentre_ID = newCentreID;
                        application_change.New_Exam_Id = newExamCycle == 0 ? result.examid : newExamCycle;
                        application_change.Agreed_Disagreed_Flag = "Y";
                        application_change.Agreed_Disagreed_Timestamp = DateTime.Now;
                        application_change.Centrechange_Optout = lbl_save_option.Text == "O" ? "O" : "C";
                        context.Entry(application_change).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        GenerateMobileOTP(applNumber);
                        DdlExamCentreState1.Enabled = false;
                        DdlExamCentre1.Enabled = false;
                        ddl_examcycle.Enabled = false;
                        btnSubmit.Enabled = false;
                        btn_yes.Enabled = false;
                        btn_no.Enabled = false;
                        tr_enter_otp.Visible = true;
                        tr_validate.Visible = true;
                    }
                    
                }
            }
       }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void btn_validate_Click(object sender, EventArgs e)
    {
        try
        {
            btn_validate.Enabled = false;
            string applNumber = appId.Text.ToString().Trim();
            using (EConnectContext context = new EConnectContext())
            {
                var candidate_validation = context.ExamCentreChoiceRefillings.Where(s => s.ApplicationId == applNumber).FirstOrDefault();
                if (candidate_validation.Agreed_Disagreed_Flag=="N" && candidate_validation.Otp_Number == Convert.ToInt32(txt_otp.Text.ToString().Trim()))
                {
                    //ExamCentreChoiceRefilling application = new ExamCentreChoiceRefilling();
                    var application = context.ExamCentreChoiceRefillings.Where(s => s.ApplicationId == applNumber).FirstOrDefault();
                    application.Validation_Flag = "Y";
                    application.Validation_Timestamp = DateTime.Now;
                    context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                    //context.ExamCentreChoiceRefillings.Add(application);
                    context.SaveChanges();
                    tr_validate_otp.Visible = true;
                    //lbl_validate_msg.Visible = true;
                    //lbl_validate_msg.Text = "You have not selected any new exam centre. ";
                    Lblerror.Visible = true;
                    Lblerror.Text = lbl_save_option.Text == "O" ? "Validated successfully that You have not opted any new exam cycle." : "Validated successfully that You have not opted any new exam centre.";
                    //Lblerror.Text = "Validated successfully that You have not opted any new exam centre.....";
                    Lblerror.BackColor = Color.LightGreen;
                    Lblerror.ForeColor = Color.DarkRed;
                }  
                else if (candidate_validation.Agreed_Disagreed_Flag=="Y" && candidate_validation.Otp_Number == Convert.ToInt32(txt_otp.Text.ToString().Trim()))
                {
                    //ExamCentreChoiceRefilling application = new ExamCentreChoiceRefilling();
                    var application = context.ExamCentreChoiceRefillings.Where(s => s.ApplicationId == applNumber).FirstOrDefault();
                    application.Validation_Flag = "Y";
                    application.Validation_Timestamp = DateTime.Now;
                    context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                    //context.ExamCentreChoiceRefillings.Add(application);
                    context.SaveChanges();
                    tr_validate_otp.Visible = true;
                    //lbl_validate_msg.Visible = true;
                    //lbl_validate_msg.Text = "Your new Exam Centre has been changed successfully.";
                    Lblerror.Visible = true;
                    //Lblerror.Text = "Your new Exam Centre has been changed and validated successfully.....";
                    Lblerror.Text = lbl_save_option.Text == "O" ? "Your new Exam Centre and Exam Cycle has been changed and validated successfully." : "Your new Exam Centre has been changed and validated successfully.";
                    Lblerror.BackColor = Color.LightGreen;
                    Lblerror.ForeColor = Color.DarkRed;
                }
                else 
                {
                    //ExamCentreChoiceRefilling application = new ExamCentreChoiceRefilling();
                    var application = context.ExamCentreChoiceRefillings.Where(s => s.ApplicationId == applNumber).FirstOrDefault();
                    application.Validation_Flag = "N";
                    application.Validation_Timestamp = DateTime.Now;
                    context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                    //context.ExamCentreChoiceRefillings.Add(application);
                    context.SaveChanges();
                    tr_validate_otp.Visible = true;
                    //lbl_validate_msg.Visible = true;
                    //lbl_validate_msg.Text = "You have entered wrong OTP number.Kindly enter correct OTP and validate again.";
                    Lblerror.Visible = true;
                    Lblerror.Text = "You have entered wrong OTP number.Kindly enter correct OTP and validate again.";
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void chk_CheckedChanged(object sender, EventArgs e)
    {
        
    }   

    protected void btn_center_change_Click(object sender, EventArgs e)
    {
        //15-sep-2020 is cutoff date for centre change and optout---------
        DateTime cutoffdate = new DateTime(2020, 09, 15);
        if (DateTime.Now.Date > cutoffdate)
        {
            ShowAlert("Date is over to avail this facility.!!");
            return;
        }
        lbl_save_option.Text = "C";
        divReportData.Visible = true;
        tr_consent_row.Visible = false;
        tr_consent_row1.Visible = false;
        tr_consent_row2.Visible = false;
        tr_consent_row3.Visible = false;
    }

    protected void btn_opt_out_Click(object sender, EventArgs e)
    {
        //15-sep-2020 is cutoff date for centre change and optout---------
        DateTime cutoffdate = new DateTime(2020, 09, 15);
        if (DateTime.Now.Date > cutoffdate.Date)
        {            
           ShowAlert( "Date is over to avail this facility.!!");
            return;
        }
        lbl_save_option.Text = "O";
        divReportData.Visible = true;
        tr_consent_row.Visible = false;
        tr_consent_row1.Visible = false;
        tr_consent_row2.Visible = false;
        tr_consent_row3.Visible = false;
        
    }
    
}


