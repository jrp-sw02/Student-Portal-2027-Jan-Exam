using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using System.Net;
using System.Web;
using System.Data.Objects;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using EConnect;
using EConnect.NIELIT;
using EConnect.URM;
using System.Collections.Generic;
using System.Data;
using EConnect.Utils.Common;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Security.Cryptography;

public partial class OnlinePuraskarApplicationForm : BasePage
{
    int countModules = 0;
    String strMessage = string.Empty;
    DataTable DtExam = new DataTable();
    DataTable DtPuraskar = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;

        if (IsSessionAlive() == false)
            Response.Redirect("../Index.aspx");
//	  ShowAlert("Implementation of updates for Protsahan Puraskar is under process.Dates of opening of application form shall be intimated soon.", true);
//	return;
        if (!IsPostBack)
        {
            // REQUEST recieve from OnlinePuraskarApplicationFrmPreview page
            if (!string.IsNullOrWhiteSpace(Request.QueryString["FinalSumbited"]))
            {
                string FinalSubmitted = Request.QueryString["FinalSumbited"].ToString();
                if (FinalSubmitted == "YES")
                {
                    string Reg = "0", OnlineRefNo="0";
                    string strMessage = Request.QueryString["strMessage"].ToString();
                    OnlinePuraskarAppForm.Visible = false;
                    divSumbitMsg.Visible = true;

                    LblSubmitMessage.Text = strMessage;
                    Reg = Request.QueryString["Reg"].ToString();
                    OnlineRefNo = Request.QueryString["OnlineRefNo"].ToString();
                    // OnlineRefNo = "1071656";
                    tblNavLinks.Visible = true;
                    hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("OnlinePuraskarApplicationFrmPreview.aspx?Reg=" + Reg + "&OnlineRefNo=" + OnlineRefNo + "&Preview=" + "P");
                }
               
            }
            ViewRecord();
            Session["IsUpdate"] = "No";
            if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                ShowAlert(Request.QueryString["msg"].ToString());
        }
      
        BreadCrumb1.Render();
    }

    protected void ViewRecord()
    {
        try
        {
            
            Int32 RegistrationNumber = 0; Int64 CandidateID = 0;
            string UserID = (Session["studentReg"]).ToString();
            //string UserID = "1071656";
                RegistrationNumber = Convert.ToInt32(UserID);
            using (EConnectContext context = new EConnectContext())
            {
                CourseExamApplication objcandidaeID;

		
		//Added for Cancelled registration no.
                var contactreg = (from s in context.Users
                                  join k in context.RegistrationDetails  on s.UserRefNumber    equals k.CandidateID  
                                  where k.RegistrationNo   == RegistrationNumber
                                 // && k.RegistrationStatusCode.ToString () !="C"
                                  select new { s.UserRefNumber ,k.RegistrationNo  }).FirstOrDefault();
                string candID = contactreg.UserRefNumber.ToString();

                var regnSearch = (from x in context.RegistrationDetails
                                  where x.CandidateID.ToString() == candID
                                  && x.RegistrationStatusCode.ToString() != "C"
                                  orderby x.ID descending
                                  select new { x.RegistrationNo }).FirstOrDefault();
                string RegnNo = regnSearch.RegistrationNo.ToString();
                //

		objcandidaeID = context.CourseExamApplications.Where(a => a.RegistrationNumber.ToString () == RegnNo).OrderByDescending(a=> a.ExamID).FirstOrDefault();


             //   objcandidaeID = context.CourseExamApplications.Where(a => a.RegistrationNumber == RegistrationNumber).OrderByDescending(a=> a.ExamID).FirstOrDefault();
		if (objcandidaeID == null)
                {
                    ShowAlert("No exam details found");
                    OnlinePuraskarAppForm.Visible = false;
                    return;
                }

                if (objcandidaeID.CandidateID.ToString() != "")
                {
                    CandidateID = Convert.ToInt64(objcandidaeID.CandidateID);
                }
            }
            Int64 ExamId = 0;
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            DataTable dt = new DataTable();
            DataTable DtAppred = new DataTable();
            using (SqlConnection conn = new SqlConnection(constr))          
            {
                // from course registration application fetch examid with candidate id only for institute candidates
                using (SqlCommand cmd = new SqlCommand("getCandidateDetails",conn)) 
                {                    
                    cmd.CommandType = CommandType.StoredProcedure;                    
                    cmd.Parameters.Add(new SqlParameter("@pCandidateID", SqlDbType.Int));
                    cmd.Parameters["@pCandidateID"].Value = CandidateID;
                  //  cmd.Parameters["@pCandidateID"].Value = 300320; // for testing direct pass candidateID
                    conn.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {                       
                        sda.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {       
				// DEEP ADD CODE ON 27 MAY 2022 FOR ANY PAPER FAIL WITH EXAM ID WISE, NOT ELIGIBLE FOR PURUSKAR APPLICATION
                                DataTable dt1 = new DataTable();
                                using (SqlCommand cmdPapperFailCheck = new SqlCommand("CheckAllPaperPassed", conn)) // IF O THEN PROCESS OTHER WISE NOT ELIGIBLE
                                {
                                    cmdPapperFailCheck.CommandType = CommandType.StoredProcedure;
                                    cmdPapperFailCheck.Parameters.AddWithValue("@pCandidateID", Convert.ToInt64(row["Candidate_ID"].ToString()));
                                    using (SqlDataAdapter sdaPaperFailCheck = new SqlDataAdapter(cmdPapperFailCheck))
                                    {
                                        sdaPaperFailCheck.Fill(dt1);
                                        if (dt1.Rows.Count > 0)
                                        {
                                            OnlinePuraskarAppForm.Visible = false;
                               Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('You are not eligible for Portsahan Puruskar for not fulfilling exam criteria!')", true);
                                            return;    
                                        }
                                    }
                                }

                                // DEEP END CODE ON 27 MAY 2022



                                                  
                                using (SqlCommand cmdPapper = new SqlCommand("GridPapersAppearedPassed", conn))
                                {                                   
                                    cmdPapper.CommandType = CommandType.StoredProcedure;
                                    cmdPapper.Parameters.AddWithValue("@pCandidateID", Convert.ToInt64(row["Candidate_ID"].ToString()));
                                    cmdPapper.Parameters.AddWithValue("@pExamID", Convert.ToInt64(row["Exam_ID"].ToString()));
                                    ExamId = Convert.ToInt64(row["Exam_ID"].ToString());
                                    using (SqlDataAdapter sdaPaper = new SqlDataAdapter(cmdPapper))
                                    {
                                        sdaPaper.Fill(DtAppred);
                                        GVPaper.DataSource = DtAppred;
                                        GVPaper.DataBind();
                                    }
                                }
                                  


 
                                string dateInString = row["Result_Publish_Date"].ToString();
                                if (dateInString == "")
                                {
                                    dateInString = "1900-01-01";
                                }

                                //Check Final Submitted Form
                                using (EConnectContext context = new EConnectContext())
                                {
                                    PuraskarApplicationForm ApplicationStatus;
                                    ApplicationStatus = context.PuraskarApplicationForms.Where(a => a.CandidateID == CandidateID && a.RegnNo == RegistrationNumber && a.ExamID == ExamId).FirstOrDefault();
                                    
                                    if (ApplicationStatus!=null)
                                    {
                                        if (ApplicationStatus.CandidateID.ToString() != "" && ApplicationStatus.finalSubmit.ToString() == "True") //Session["IsUpdate"] = "Yes";
                                    {                                        
                                        string OnlineRefNo = ApplicationStatus.OnlineRefNo.ToString();
                                        OnlinePuraskarAppForm.Visible = false;
                                        divSumbitMsg.Visible = true;
                                        tblNavLinks.Visible = true;
                                        //if (Session["IsUpdate"].ToString() == "Yes")
                                        //{
                                        //    strMessage = "Your Puraskar Applicattion is already submitted with Online Reference ID is " + OnlineRefNo;
                                        //    hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("OnlinePuraskarApplicationFrmPreview.aspx?Reg=" + RegistrationNumber + "&OnlineRefNo=" + OnlineRefNo + "&Preview=" + "P");

                                        //    LblSubmitMessage.Text = strMessage;
                                        //}
                                        //else
                                        //{
                                        strMessage = "Your Puraskar Applicattion is Successfully submitted with Online Reference ID is   <br> " +"'" + OnlineRefNo +"'";
                                            hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("OnlinePuraskarApplicationFrmPreview.aspx?Reg=" + RegistrationNumber + "&OnlineRefNo=" + OnlineRefNo + "&Preview=" + "P");

                                            LblSubmitMessage.Text = strMessage;
                                        //}
                                    }
                                }
                                } 
                                // Check Final Submitted Form Ends

                                DateTime startDate = DateTime.Parse(dateInString);
                                DateTime expiryDate = startDate.AddDays(45);
			//Added for new protsahan 29 Jul 2023  examid   6281  6279   6277 6275
				// expiryDate = Convert.ToDateTime("2021-09-12");
				
				//Added for extension of Puraskar May,2024
				/*if (startDate.Date == Convert.ToDateTime("2024-03-18").Date)
                 		{
		                   expiryDate = Convert.ToDateTime("2024-05-11");
                 		}*/
				//
		//                 if (startDate.Date == Convert.ToDateTime("2023-04-23").Date)
                 		//{
		//                     expiryDate = Convert.ToDateTime("2023-09-12");
                 		//}
                                // Checks 45 days not more than result publish date
                                //if (DateTime.Now < expiryDate)

				 // Original if (DateTime.Now.Date <expiryDate.Date)
				//For extension of Protsahan Puraskar
				if (DateTime.Now.Date <expiryDate.Date && DateTime.Now.Date >Convert.ToDateTime("2024-05-05").Date)
                                {
                                    // Only Female or sc or st or handicaped allowed
                                    if (row["Gender"].ToString() == "Female" || row["Cast_Category_ID"].ToString() == "2" || row["Cast_Category_ID"].ToString() == "3" || row["Is_Handicaped"].ToString() == "True")
                                    {
                                        // Checks course id O=1 , A=2, B=3, C=4 LEVEL
                                        if (row["Course_ID"].ToString() == "1" || row["Course_ID"].ToString() == "2" || row["Course_ID"].ToString() == "3" || row["Course_ID"].ToString() == "4")
                                        {
                                            lblName.Text = row["Name"].ToString();
                                            lblParent_GuardianDetails.Text = row["Father_Name"].ToString();
                                            lblGender.Text = row["Gender"].ToString();                                           
                                            lblDOB.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(row["Dob"]));
                                            LblCaste.Text = row["CasteName"].ToString();
                                            LblEmailId.Text = row["Email"].ToString();
                                            LblMobileNumber.Text = row["Mobile"].ToString();
                                            LblInstituteDetails.Text = row["InsName"].ToString();
                                           // LblInstitute.Text = row["InsName"].ToString();
                                            LblRegnNo.Text = RegistrationNumber.ToString();
                                            Session["Exam_ID"] = Convert.ToInt64(row["Exam_ID"].ToString());
                                            Session["Course_ID"] = Convert.ToInt64(row["Course_ID"].ToString());
                                            Session["AdhaarNo"] = row["Aadhar_Number"].ToString();
                                           // Session["AdhaarNo"] = "";
                                            if (Session["AdhaarNo"].ToString() == "")
                                            {
                                                //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Adhaar Number is not avialable as per Master record. Please Update Adhaar Number in registration section ')", true);
                                                OnlinePuraskarAppForm.Visible = false;
                                                Aadhar.Text = "Aadhaar Number is not available as per Master record. Please contact registration section for updation of Aadhaar";
                                                divAadhar.Visible = true;                                              
                                                
                                               // return;

                                            }


                                            ddlPWD.Items.FindByValue(row["Is_Handicaped"].ToString()).Selected = true;
                                            Session["CourseName"] = row["CourseName"].ToString();
                                            Session["Candidate_ID"] = row["Candidate_ID"].ToString();
                                            Session["Institute_ID"] = row["Institute_ID"].ToString();
                                            if ((row["Cast_Category_ID"].ToString() == "2" || row["Cast_Category_ID"].ToString() == "3") && row["Gender"].ToString() != "Female")
                                            {
                                                CasteCertDetailsInputView.Visible = true;                                                
                                            }
                                            else
                                            {
                                                CasteCertDetailsInputView.Visible = false;
                                            }

                                            if (ddlPWD.SelectedValue.ToString() == "True")
                                            {
                                                PHCertDetailsInputView.Visible = true;                                                                                          
                                            }
                                            else
                                            {
                                                PHCertDetailsInputView.Visible = false;
                                            }

                                            // mobile & Email Checks 
                                                Int64 MobileNumber = 0 ;
                                                int examID = 0;
                                                string EmailId = string.Empty;
                                                CandidateID = Convert.ToInt64(Session["Candidate_ID"].ToString());
                                                MobileNumber = Convert.ToInt64(LblMobileNumber.Text);
                                                examID = Convert.ToInt32(Session["Exam_ID"].ToString());
                                                EmailId = LblEmailId.Text;

                                                using (EConnectContext context = new EConnectContext())
                                                   {
                                                 //PuraskarAppMobileEmailOtpVerificationDetail objPuraskarAppMobileOTPVerification;
                                                 var ExistsMobileOTP = (from s in context.PuraskarAppMobileEmailOtpVerificationDetails
                                                    where s.CandidateID == CandidateID && s.ExamID == examID && s.MobileNumber == MobileNumber && s.IsMobileNumberVerified == true
                                                    select new { ID = s.CandidateID, name = s.ExamID }).Distinct();

                                                if (ExistsMobileOTP.Count() > 0)
                                                    {
                                                         btnsendOTP.Visible=false;
                                                         lblConfiramation.Text = "Verified";
                                                         lblConfiramation.Visible = true;
                                                     }


                                                var ExistsEmailOTP = (from s in context.PuraskarAppMobileEmailOtpVerificationDetails
                                                where s.CandidateID == CandidateID && s.ExamID == examID && s.EmailAddress == EmailId && s.IsEmailVerified == true
                                                select new { ID = s.CandidateID, name = s.ExamID }).Distinct();

                                                if (ExistsEmailOTP.Count() > 0)
                                                    {
                                                         btnsendOTPEmail.Visible = false;
                                                         lblConfiramationEmail.Text = "Verified";
                                                         lblConfiramationEmail.Visible = true;
                                                     }
                                                  }
                                                // mobile & Email Checks END
                                        }
                                        else
                                        {
                                            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('The course must be from O , A, B , C Level Only!')", true);
                                            return;                                       
                                        }
                                    }
                                    else
                                    {
                                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('You did not come under SC / ST / PWD / Female. Please contact your administrator.')", true);
                                        OnlinePuraskarAppForm.Visible = false;
                                        return;
                                    }
                                }
                                else
                                {
                                    //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Your due date is already expired. You can't apply the application.')", true);
                                    //return;
                                    //Commented for changes 		
					ShowAlert("Your due date is already expired. You can not apply the application.", true);
//				    // ShowAlert("Implementation of updates for Protsahan Puraskar is under process.Dates of opening of application form shall be intimated soon.", true);
                                    OnlinePuraskarAppForm.Visible = false;
                                    return;
                                    //DateTime Msessage 
                                }
                               
                                using (SqlCommand cmd1 = new SqlCommand("getExamMonthYear", conn))
                                {                                    
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.AddWithValue("@pExamID", Convert.ToInt64(row["Exam_ID"].ToString()));
                                    using (SqlDataAdapter sd = new SqlDataAdapter(cmd1))
                                    {
                                        sd.Fill(DtExam);                                        
                                        if (DtExam.Rows.Count > 0)
                                        {
                                            LblExam.Text = DtExam.Rows[0]["ExamMonth"].ToString();
                                        }
                                    }
                                }
                                //ChkFirstAttempt
                                using (SqlCommand cmd1 = new SqlCommand("ChkFirstAttempt", conn))
                                {
                                    //cmd1.Connection = Conn;
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.AddWithValue("@pCourse_ID", Convert.ToInt64(Session["Course_ID"].ToString()));
                                    cmd1.Parameters.AddWithValue("@pCandidateID", Convert.ToInt64(row["candidate_id"].ToString()));
                                    cmd1.Parameters.AddWithValue("@pExamID", Convert.ToInt64(row["Exam_ID"].ToString()));
                                    // conn.Open();
                                    using (SqlDataAdapter sd = new SqlDataAdapter(cmd1))
                                    {
                                        DataTable DtChkApptempt = new DataTable();
                                        sd.Fill(DtChkApptempt);

                                        if (DtChkApptempt.Rows.Count > 0)
                                        {
                                            int papercheckcount = Convert.ToInt32(DtChkApptempt.Rows[0]["CountExam"]);
                                            
                                            if (Convert.ToInt64(Session["Course_ID"].ToString()) == 1)
                                            {
                                                if (Convert.ToInt32(DtChkApptempt.Rows[0]["CountExam"]) > 2) // maximum attempt 2 times for O
                                                {
                                                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('You are not eligible for this Puraskar Scheme. You must have cleared all modules in First Attempt')", true);
                                                    OnlinePuraskarAppForm.Visible = false;
                                                    return;
                                                }
                                            }
                                            if (Convert.ToInt64(Session["Course_ID"].ToString()) == 2)
                                            {
                                                if (Convert.ToInt32(DtChkApptempt.Rows[0]["CountExam"]) > 3)// maximum attempt 3 times for A
                                                {
                                                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('You are not eligible for this Puraskar Scheme. You must have cleared all modules in First Attempt')", true);
                                                    OnlinePuraskarAppForm.Visible = false;
                                                    return;
                                                }
                                            }
                                            if (Convert.ToInt64(Session["Course_ID"].ToString()) == 3)
                                            {
                                                if (Convert.ToInt32(DtChkApptempt.Rows[0]["CountExam"]) > 4)// maximum attempt 4 times for B
                                                {
                                                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('You are not eligible for this Puraskar Scheme. You must have cleared all modules in First Attempt')", true);
                                                    OnlinePuraskarAppForm.Visible = false;
                                                    return;
                                                }
                                            }
                                            ////Commented for testing
                                            //if (Convert.ToInt32(DtChkApptempt.Rows[0]["CountExam"]) != 1)
                                            //{
                                            //    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('You are not eligible for this Puraskar Scheme. You must have cleared all modules in First Attempt')", true);
                                            //    OnlinePuraskarAppForm.Visible = false;
                                            //    return;
                                            //}
                                        }
                                    }
                                }

                                //Update Case
                                using (SqlCommand cmd2 = new SqlCommand("getPuruskarDetails", conn))
                                {                                   
                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd2.Parameters.AddWithValue("@pCandidateID", Convert.ToInt64(row["candidate_id"].ToString()));
                                    cmd2.Parameters.AddWithValue("@pExamID", Convert.ToInt64(Session["Exam_ID"].ToString()));
                                    using (SqlDataAdapter sd = new SqlDataAdapter(cmd2))
                                    {
                                        sd.Fill(DtPuraskar);

                                        if (DtPuraskar.Rows.Count > 0)
                                        {
                                            btnSave.Text = "Update";
                                            foreach (DataRow row1 in DtPuraskar.Rows)
                                            {
                                                LblRegnNo.Text = row1["RegnNo"].ToString();
                                                // LblApplicantName.Text = row1["Name"].ToString();
                                               // txtApplicantName.Text = row1["Name"].ToString();
                                                txtAadharNumber.Text = row1["AadharNumber"].ToString();
                                                txtAccountNo.Text = row1["AccountNo"].ToString();
                                                txtAccountHolderName.Text = row1["AccountHolderName"].ToString();
                                                txtAccountType.Text = row1["AccountType"].ToString();
                                                txtBankName.Text = row1["BankName"].ToString();
                                                txtBankAddress.Text = row1["BankAddress"].ToString();
                                                txtBankIFSC.Text = row1["BankIFSC"].ToString();
                                                txtCasteCertNo.Text = row1["CasteCertNo"].ToString();                                                                                               
                                                if (row1["CasteCertDate"].ToString() != "")
                                                {
                                                    txtCasteCertDate.Text = Convert.ToDateTime(row1["CasteCertDate"]).ToString("dd-MMM-yyyy");                                                  
                                                }
                                                txtIncomeCertNo.Text = row1["IncomeCertNo"].ToString();                                                                                             
                                                if (row1["IncomeCertDate"].ToString() != "")
                                                {
                                                    txtIncomeCertDate.Text = Convert.ToDateTime(row1["IncomeCertDate"]).ToString("dd-MMM-yyyy");                                                    
                                                }
                                                txtAnnualIncome.Text = row1["AnnualIncome"].ToString();
                                                txtPHCertNo.Text = row1["PHCertNo"].ToString();                                                                                              
                                                if (row1["PHCertDate"].ToString() != "")
                                                {
                                                    txtPHCertDate.Text = Convert.ToDateTime(row1["PHCertDate"]).ToString("dd-MMM-yyyy");                                                   
                                                }
                                                txtpaperaap.Text = row1["PapersAppeared"].ToString();
                                                txtpaperpass.Text = row1["PapersPassed"].ToString();
                                                txtpaperaap.Enabled = false;
                                                txtpaperpass.Enabled = false;
                                                paperviewcount.Visible = false;
                                                txtAadharNumber.Enabled = true;
                                                txtAccountNo.Enabled = false;    
                                         
                                               
                                                if (row1["OnlineRefNo"].ToString() != "")
                                                {
                                                    Session["onlineRefID"] = row1["OnlineRefNo"].ToString();
                                                }                                               
                                                btnSave.Text = "Update";                                                
                                            }
                                        }
                                    }
                                }
                            }

                        }// for main table data check exists or not, only for institute candidates not for direct candidates
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('You are not eligible for this Puraskar Scheme.')", true);
                            OnlinePuraskarAppForm.Visible = false;
                            return;
                        }
                    }
                }
            } 
        }
        catch (Exception ex)
        {
            //throw ex.ToString();
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Something is wrong. Please check after some times.')", true);

        }
    }
      
     protected void SaveRecord(object sender, EventArgs e)   
    {
        try
        {
            byte[] CasteCertUpload = null;//System.IO.File.ReadAllBytes(inputCasteCertUpload.Value);
            byte[] IncomeCertUpload = null;// System.IO.File.ReadAllBytes(inputIncomeCertUpload.Value);
            byte[] PHCertUploadF = null;                      
            int CountPapersAppeared = 0,CountPapersPassed = 0;            
            string IncomeCertUploadFileName = "", CasteCertUploadFileName = "", PHCertUploadFileName = "";
            Int64 CandidateId = Convert.ToInt64(Session["Candidate_ID"].ToString());
            Int64 ExamId = Convert.ToInt64(Session["Exam_ID"].ToString());
            Int64 annualincomeCriteria = 250000;
            BreadCrumb1.Render();
            if (IsValidForm())
            {
                //string val = Session["onlineRefID"].ToString();
                string AAD = Session["AdhaarNo"].ToString();
                if (Session["AdhaarNo"].ToString() != txtAadharNumber.Text)
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Aadhaar Card is not valid as per Master record. Please enter correct Aadhaar number. OR Update Aadhaar Number in registration section ')", true);
                    return;

                }
		 if (txtAadharNumber.Text.Length != 12)
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Incorrect length for Aadhaar Number ')", true);
                    return;
                }
                if (Convert.ToInt64(txtAnnualIncome.Text) > annualincomeCriteria) // annunal income 250000 greater than 
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Annual Income is not under this criteria.')", true);
                    return;

                }
                EConnectContext contextC = new EConnectContext();
                PuraskarApplicationForm RecordExistSs = contextC.PuraskarApplicationForms.Where(s => s.CandidateID == CandidateId && s.ExamID == ExamId).FirstOrDefault();
                string onlineRefId = "NA";
                if (RecordExistSs != null)
                {
                     onlineRefId = RecordExistSs.OnlineRefNo.ToString();
                }
                //if (Session["onlineRefID"] == null)
                if (onlineRefId == "NA")
                {
                    if (ddlPWD.SelectedValue.ToString() == "True")
                    {
                        if (PHCertUpload.HasFile)
                        {
                            if (PHCertUpload.FileName.Length > 50)
                            {
                                PHCertUploadFileName = PHCertUpload.FileName.Substring(0, 100) + System.IO.Path.GetExtension(PHCertUpload.FileName).ToLower();
                            }
                            else
                                PHCertUploadFileName = PHCertUpload.FileName;
                            PHCertUploadF = PHCertUpload.FileBytes;
                        }
                        else
                        {
                            ShowAlert("Please Upload the physically handicapped Certificate.", true);
                            return;
                        }
                    }
                    if ((LblCaste.Text == "Scheduled Caste" || LblCaste.Text == "Scheduled Tribe") && lblGender.Text !="Female")
                    {
                        if (CasteCertFileUpload.HasFile)
                        {
                            if (CasteCertFileUpload.FileName.Length > 50)
                            {
                                CasteCertUploadFileName = CasteCertFileUpload.FileName.Substring(0, 100) + System.IO.Path.GetExtension(CasteCertFileUpload.FileName).ToLower();
                            }
                            else
                                CasteCertUploadFileName = CasteCertFileUpload.FileName;
                            CasteCertUpload = CasteCertFileUpload.FileBytes;
                        }
                        else
                        {
                            ShowAlert("Please Upload the Caste Certificate.", true);
                            return;
                        }
                    }

                    if (IncomeCerFileUpload.HasFile)
                    {
                        if (IncomeCerFileUpload.FileName.Length > 50)
                        {
                            IncomeCertUploadFileName = IncomeCerFileUpload.FileName.Substring(0, 100) + System.IO.Path.GetExtension(IncomeCerFileUpload.FileName).ToLower();
                        }
                        else
                            IncomeCertUploadFileName = IncomeCerFileUpload.FileName;
                        IncomeCertUpload = IncomeCerFileUpload.FileBytes;
                    }
                    else
                    {
                        ShowAlert("Please Upload the Income Certificate.", true);
                        return;
                    }
                    foreach (GridViewRow row in GVPaper.Rows)
                    {
                        CheckBox chkRow = (row.Cells[2].FindControl("ChkPassed") as CheckBox);
                        if (chkRow.Checked == true)
                        {
                            CountPapersPassed = CountPapersPassed + 1;
                        }
                        CountPapersAppeared = CountPapersAppeared + 1;
                    }
                    //Session["Course_ID"] = Convert.ToInt64(row["Course_ID"].ToString());
                    Int64 courseId = Convert.ToInt64(Session["Course_ID"]);

                    foreach (GridViewRow row in GVPaper.Rows)
                    {
                        bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                         if (isChecked)
                        {
                            countModules = countModules + 1;
                        }
                            int moduleId = Convert.ToInt32(GVPaper.DataKeys[row.RowIndex].Value);
                            string PaperspassedCheck = isChecked.ToString();
                            UpdatePapersPassed(moduleId, PaperspassedCheck);

                            
                        
                    }
                    Int64 ModulesCnt = Convert.ToInt64(PapersEligibleCountCourseWise(courseId));
                    if (countModules < ModulesCnt)
                    {
                        UpdatePapersPassed(9999999, "False");
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('The Moudels count not eligible!')", true);

                        return;
                    }
                    using (EConnectContext context = new EConnectContext())
                    {

                        //EncryptDecrypt = new EncryptDecrypt();
                        string aadharEncrypted = EncryptDecrypt.EncryptString(txtAadharNumber.Text);
                        //string aadharDecrypted = EncryptDecrypt.DecryptString(txtAadharNumber.Text);
                        Int64 AadhRefId = AadharEncreptedReference(aadharEncrypted); // scope identity of aadhar valule encrypted insertion
                        PuraskarApplicationForm objpuraskarApplication;
                        objpuraskarApplication = new PuraskarApplicationForm();
                        objpuraskarApplication.CandidateID = CandidateId;
                        objpuraskarApplication.RegnNo = Convert.ToInt64(LblRegnNo.Text);
                        objpuraskarApplication.InstituteID = Convert.ToInt64(Session["Institute_ID"].ToString());
                        //objpuraskarApplication.Name = txtApplicantName.Text;
                        objpuraskarApplication.Name = lblName.Text;
                        //objpuraskarApplication.AadharNumber = Convert.ToInt64(txtAadharNumber.Text); As per Modification [NIELIT].[dbo].[PuraskarAddh] scope indentity insert this fields
                        objpuraskarApplication.AadharNumber = AadhRefId;

                        objpuraskarApplication.AccountNo = txtAccountNo.Text;
                        objpuraskarApplication.AccountHolderName = txtAccountHolderName.Text;
                        objpuraskarApplication.AccountType = txtAccountType.Text;
                        objpuraskarApplication.BankName = txtBankName.Text;
                        objpuraskarApplication.BankAddress = txtBankAddress.Text;
                        objpuraskarApplication.BankIFSC = txtBankIFSC.Text;
                        objpuraskarApplication.AnnualIncome = Convert.ToInt64(txtAnnualIncome.Text);
                        objpuraskarApplication.ExamID = ExamId;
                        objpuraskarApplication.PapersAppeared = CountPapersAppeared;
                        objpuraskarApplication.PapersPassed = CountPapersPassed;

                        if (IncomeCerFileUpload.HasFile)
                        {
			    objpuraskarApplication.IncomeCertNo = txtIncomeCertNo.Text;
                            objpuraskarApplication.IncomeCertDate = Convert.ToDateTime(txtIncomeCertDate.Text);
                            objpuraskarApplication.IncomeCertUpload = IncomeCertUpload;
                            objpuraskarApplication.IncomeCertFile = IncomeCertUploadFileName;
                            objpuraskarApplication.IncomeCertUploadedOn = Convert.ToDateTime(DateTime.Now);
                        }
                        if (CasteCertFileUpload.HasFile)
                        {
			    objpuraskarApplication.CasteCertNo = txtCasteCertNo.Text;
                            objpuraskarApplication.CasteCertDate = Convert.ToDateTime(txtCasteCertDate.Text);
                            objpuraskarApplication.CasteCertUpload = CasteCertUpload;
                            objpuraskarApplication.CasteCertFile = CasteCertUploadFileName;
                            objpuraskarApplication.CasteCertUploadedOn = Convert.ToDateTime(DateTime.Now);
                        }
                        if (PHCertUpload.HasFile)
                        {
                            objpuraskarApplication.PHCertNo = txtPHCertNo.Text;
                            objpuraskarApplication.PHCertDate = Convert.ToDateTime(txtPHCertDate.Text);
                            objpuraskarApplication.PHCertUpload = PHCertUploadF;
                            objpuraskarApplication.PHCertFile = PHCertUploadFileName;
                            objpuraskarApplication.PHCertUploadedOn = Convert.ToDateTime(DateTime.Now);
                        }
                        objpuraskarApplication.applicationStatusID = Convert.ToInt32(enmPuraskarApplicationStatus.AppliedByCandidate); // applicationStatusID= 1 for Applied
                        objpuraskarApplication.enterBy = Convert.ToInt32(Session["UserID"]);
                        objpuraskarApplication.enterDate = DateTime.Now;
                        context.PuraskarApplicationForms.Add(objpuraskarApplication);
                        context.SaveChanges();
                        strMessage = "New record saved.";

                        PuraskarApplicationForm FetchIdForRef = context.PuraskarApplicationForms.Where(s => s.CandidateID == CandidateId && s.ExamID == ExamId).FirstOrDefault();
                        string Id = FetchIdForRef.ID.ToString();
                        // FOR EXAM CYCLE ADD IN REFERENCE NUMBER
                        string monthsName = string.Empty;
                        Exam ExamDetails = context.Exams.Where(s =>  s.ID == ExamId).FirstOrDefault();
                        string ExamMonth = ExamDetails.ExamMonth.ToString();
                        string ExamYear = ExamDetails.ExamYear.ToString();
                        if (ExamMonth == "1")
                        {monthsName = "JANUARY";}
                        if (ExamMonth == "7")
                        {monthsName = "JULY";}
                        string ExamCycles = monthsName + "-" + ExamYear;
                        // FOR EXAM CYCLE

                        //string Onlinerefno = Session["CourseName"].ToString().Trim() + "-" + Id;
                        string Onlinerefno = Session["CourseName"].ToString().Trim() + "/" + ExamCycles + "/" + Id; //O LEVEL/JULY-2017/10001
                        FetchIdForRef.OnlineRefNo = Onlinerefno;
                        context.SaveChanges();
                        Session["onlineRefID"] = "";
                        strMessage = "Your Puraskar Applicattion is Successfully submitted with Online Reference ID is " + Onlinerefno;
                        Response.Redirect("OnlinePuraskarApplicationFrmPreview.aspx?Reg=" + Convert.ToInt64(LblRegnNo.Text) + "&CandidateId=" + CandidateId + "&ExamId=" + ExamId, true);
                    }
                   // ShowAlert(strMessage, true);                   
                    //}
                }
                else// For updation record by Login.
                {
                    Session["IsUpdate"] = "Yes";
                    Int64 courseId = Convert.ToInt64(Session["Course_ID"]);
                    
                    foreach (GridViewRow row in GVPaper.Rows)
                    {
                        CheckBox chkRow = (row.Cells[2].FindControl("ChkPassed") as CheckBox);
                        if (chkRow.Checked == true)
                        {
                            CountPapersPassed = CountPapersPassed + 1;
                        }
                        CountPapersAppeared = CountPapersAppeared + 1;
                    }
                    foreach (GridViewRow row in GVPaper.Rows)
                    {
                        bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                        if (isChecked)
                        {
                            countModules = countModules + 1;
                        }
                            int moduleId = Convert.ToInt32(GVPaper.DataKeys[row.RowIndex].Value);
                            string PaperspassedCheck = isChecked.ToString();
                            UpdatePapersPassed(moduleId, PaperspassedCheck);

                            
                        
                    }
                    Int64 ModulesCnt = Convert.ToInt64(PapersEligibleCountCourseWise(courseId));
                    if (countModules < ModulesCnt)
                    {
                        UpdatePapersPassed(9999999, "False");
                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('The Moudels count not eligible!')", true);
                        return;
                    }


                    EConnectContext contextR = new EConnectContext();
                    PuraskarApplicationForm RecordExists = contextR.PuraskarApplicationForms.Where(s => s.CandidateID == CandidateId && s.ExamID == ExamId).FirstOrDefault();

                    if (ddlPWD.SelectedValue.ToString() == "True")
                    {
                        if (txtPHCertNo.Text != RecordExists.PHCertNo || Convert.ToDateTime(txtPHCertDate.Text) != Convert.ToDateTime(RecordExists.PHCertDate))
                        {
                            if (PHCertUpload.HasFile)
                            {
                                if (PHCertUpload.FileName.Length > 50)
                                {
                                    PHCertUploadFileName = PHCertUpload.FileName.Substring(0, 100) + System.IO.Path.GetExtension(PHCertUpload.FileName).ToLower();
                                }
                                else
                                    PHCertUploadFileName = PHCertUpload.FileName;
                                PHCertUploadF = PHCertUpload.FileBytes;
                            }
                            else
                            {
                                ShowAlert("Please Upload the physically handicapped Certificate.", true);
                                return;
                            }
                        }
                    }
                    if ((LblCaste.Text == "Scheduled Caste" || LblCaste.Text == "Scheduled Tribe") && lblGender.Text !="Female")
                    {
                        if (txtCasteCertNo.Text != RecordExists.CasteCertNo || Convert.ToDateTime(txtCasteCertDate.Text) != Convert.ToDateTime(RecordExists.CasteCertDate))
                        {
                            if (CasteCertFileUpload.HasFile)
                            {
                                if (CasteCertFileUpload.FileName.Length > 50)
                                {
                                    CasteCertUploadFileName = CasteCertFileUpload.FileName.Substring(0, 100) + System.IO.Path.GetExtension(CasteCertFileUpload.FileName).ToLower();
                                }
                                else
                                    CasteCertUploadFileName = CasteCertFileUpload.FileName;
                                CasteCertUpload = CasteCertFileUpload.FileBytes;
                            }
                            else
                            {
                                ShowAlert("Please Upload the Caste Certificate.", true);
                                return;
                            }
                        }
                    }
                    if (txtIncomeCertNo.Text != RecordExists.IncomeCertNo || Convert.ToDateTime(txtIncomeCertDate.Text) != Convert.ToDateTime(RecordExists.IncomeCertDate))
                    {
                        if (IncomeCerFileUpload.HasFile)
                        {
                            if (IncomeCerFileUpload.FileName.Length > 50)
                            {
                                IncomeCertUploadFileName = IncomeCerFileUpload.FileName.Substring(0, 100) + System.IO.Path.GetExtension(IncomeCerFileUpload.FileName).ToLower();
                            }
                            else
                                IncomeCertUploadFileName = IncomeCerFileUpload.FileName;
                            IncomeCertUpload = IncomeCerFileUpload.FileBytes;
                        }
                        else
                        {
                            ShowAlert("Please Upload the Income Certificate.", true);
                            return;
                        }
                    }

                    using (EConnectContext context = new EConnectContext())
                    {
                        var objpuraskarApplication = (from s in context.PuraskarApplicationForms
                                                      where s.CandidateID == CandidateId && s.ExamID == ExamId
                                                      select s).FirstOrDefault();
                        objpuraskarApplication.CandidateID = CandidateId;
                        objpuraskarApplication.RegnNo = Convert.ToInt64(LblRegnNo.Text);
                        objpuraskarApplication.InstituteID = Convert.ToInt64(Session["Institute_ID"].ToString());
                        //objpuraskarApplication.Name = txtApplicantName.Text;
                        objpuraskarApplication.Name = lblName.Text;
                        //objpuraskarApplication.AadharNumber = Convert.ToInt64(txtAadharNumber.Text);
                        objpuraskarApplication.AccountNo = txtAccountNo.Text;
                        objpuraskarApplication.AccountHolderName = txtAccountHolderName.Text;
                        objpuraskarApplication.AccountType = txtAccountType.Text;
                        objpuraskarApplication.BankName = txtBankName.Text;
                        objpuraskarApplication.BankAddress = txtBankAddress.Text;
                        objpuraskarApplication.BankIFSC = txtBankIFSC.Text;
                        objpuraskarApplication.AnnualIncome = Convert.ToInt64(txtAnnualIncome.Text);
                        objpuraskarApplication.ExamID = ExamId;
                        objpuraskarApplication.PapersAppeared = CountPapersAppeared;
                        objpuraskarApplication.PapersPassed = CountPapersPassed;

                        if (IncomeCerFileUpload.HasFile)
                        {
			 if (IncomeCerFileUpload.FileName.Length > 50)
                            {
                                IncomeCertUploadFileName = IncomeCerFileUpload.FileName.Substring(0, 100) + System.IO.Path.GetExtension(IncomeCerFileUpload.FileName).ToLower();
                            }
                            else
                            {
                                IncomeCertUploadFileName = IncomeCerFileUpload.FileName;
                                IncomeCertUpload = IncomeCerFileUpload.FileBytes;
                            }
                            objpuraskarApplication.IncomeCertNo = txtIncomeCertNo.Text;
                            objpuraskarApplication.IncomeCertDate = Convert.ToDateTime(txtIncomeCertDate.Text);
                            objpuraskarApplication.IncomeCertUpload = IncomeCertUpload;
                            objpuraskarApplication.IncomeCertFile = IncomeCertUploadFileName;
                            objpuraskarApplication.IncomeCertUploadedOn = Convert.ToDateTime(DateTime.Now);
                        }
                        if (CasteCertFileUpload.HasFile)
                        { 
			if (CasteCertFileUpload.FileName.Length > 50)
                            {
                                CasteCertUploadFileName = CasteCertFileUpload.FileName.Substring(0, 100) + System.IO.Path.GetExtension(CasteCertFileUpload.FileName).ToLower();
                            }
                            else
                            {
                                CasteCertUploadFileName = CasteCertFileUpload.FileName;
                                CasteCertUpload = CasteCertFileUpload.FileBytes;
                            }

                            objpuraskarApplication.CasteCertNo = txtCasteCertNo.Text;
                            objpuraskarApplication.CasteCertDate = Convert.ToDateTime(txtCasteCertDate.Text);
                            objpuraskarApplication.CasteCertUpload = CasteCertUpload;
                            objpuraskarApplication.CasteCertFile = CasteCertUploadFileName;
                            objpuraskarApplication.CasteCertUploadedOn = Convert.ToDateTime(DateTime.Now);
                        }
                        if (PHCertUpload.HasFile)
                        {
			if (PHCertUpload.FileName.Length > 50)
                            {
                                PHCertUploadFileName = PHCertUpload.FileName.Substring(0, 100) + System.IO.Path.GetExtension(PHCertUpload.FileName).ToLower();
                            }
                            else
                            {
                                PHCertUploadFileName = PHCertUpload.FileName;
                                PHCertUploadF = PHCertUpload.FileBytes;
                            }
                            objpuraskarApplication.PHCertNo = txtPHCertNo.Text;
                            objpuraskarApplication.PHCertDate = Convert.ToDateTime(txtPHCertDate.Text);
                            objpuraskarApplication.PHCertUpload = PHCertUploadF;
                            objpuraskarApplication.PHCertFile = PHCertUploadFileName;
                            objpuraskarApplication.PHCertUploadedOn = Convert.ToDateTime(DateTime.Now);
                        }
                        objpuraskarApplication.enterBy = Convert.ToInt32(Session["UserID"]);
                        objpuraskarApplication.enterDate = DateTime.Now;
                        context.Entry(objpuraskarApplication).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();
                        strMessage = "Record updated.";

                        Response.Redirect("OnlinePuraskarApplicationFrmPreview.aspx?Reg=" + Convert.ToInt64(LblRegnNo.Text) + "&CandidateId=" + CandidateId + "&ExamId=" + ExamId);
                       // Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("OnlinePuraskarApplicationFrmPreview.aspx?Reg=" + Convert.ToInt64(LblRegnNo.Text) + "&CandidateId=" + CandidateId + "&ExamId=" + ExamId));

                        PuraskarApplicationForm FetchIdForRef = context.PuraskarApplicationForms.Where(s => s.CandidateID == CandidateId && s.ExamID == ExamId).FirstOrDefault();
                        string Id = FetchIdForRef.ID.ToString();
                        // FOR EXAM CYCLE ADD IN REFERENCE NUMBER
                        string monthsName = string.Empty;
                        Exam ExamDetails = context.Exams.Where(s => s.ID == ExamId).FirstOrDefault();
                        string ExamMonth = ExamDetails.ExamMonth.ToString();
                        string ExamYear = ExamDetails.ExamYear.ToString();
                        if (ExamMonth == "1")
                        { monthsName = "JANUARY"; }
                        if (ExamMonth == "7")
                        { monthsName = "JULY"; }
                        string ExamCycles = monthsName + "-" + ExamYear;
                        // FOR EXAM CYCLE

                        //string Onlinerefno = Session["CourseName"].ToString().Trim() + "-" + Id;
                        string Onlinerefno = Session["CourseName"].ToString().Trim() + "/" + ExamCycles + "/" + Id; //O LEVEL/JULY-2017/10001
                        FetchIdForRef.OnlineRefNo = Onlinerefno;
                        context.SaveChanges();
                        Session["onlineRefID"] = "";
                        strMessage = "Your Puraskar Applicattion is Successfully Updated with Online Reference ID is " + Onlinerefno;
                    }
                    //ShowAlert(strMessage, true);
                   // Response.Redirect("OnlinePuraskarApplicationForm.aspx?msg=" + strMessage, true);
                   
                    //OnlinePuraskarAppForm.Visible = false;
                    //divSumbitMsg.Visible = true;

                    //LblSubmitMessage.Text = strMessage;
                }
            }
        }
        catch (Exception ex)
        {
            Session["onlineRefID"] = null;
            ShowAlert(ex.Message, true);
        }
    }
     public Int64  AadharEncreptedReference(string encAddh)
     {
          Int64 encAddhRefIDD = 0; 
         try
         {
             // string RegistrationNo = (Session["studentReg"]).ToString();
             string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
             using (SqlConnection Conn = new SqlConnection(constr))
             {
                 using (SqlCommand cmd = new SqlCommand("Insert_encAddh", Conn))
                 {
                     Conn.Open();
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.Parameters.AddWithValue("@encAddh", encAddh);
                     cmd.Parameters.AddWithValue("@enterBy", Convert.ToInt64(Session["studentReg"].ToString()));
                     //cmd.Parameters.AddWithValue("@enterDate", System.DateTime.Now);
                     cmd.Parameters.Add("@encAddhRefID", SqlDbType.BigInt);
                     cmd.Parameters["@encAddhRefID"].Direction = ParameterDirection.Output; 
                     cmd.ExecuteNonQuery();
                     encAddhRefIDD = (Int64) cmd.Parameters["@encAddhRefID"].Value;
                 }
             }
            
         }
         catch (Exception ex)
         {
             ShowAlert(ex.Message, true);
         }
          return encAddhRefIDD;
     }
     public void UpdatePapersPassed(int id, string PaperspassedCheck)
     {
         try
         {
            // string RegistrationNo = (Session["studentReg"]).ToString();
             string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
             using (SqlConnection Conn = new SqlConnection(constr))
             {
                 using (SqlCommand cmd = new SqlCommand("UpdatePaperPassedByCandidates", Conn))
                 {
                     Conn.Open();
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.Parameters.AddWithValue("@MODULEID", id);
                     cmd.Parameters.AddWithValue("@CandidateID", Convert.ToInt64(Session["Candidate_ID"].ToString()));
                     cmd.Parameters.AddWithValue("@RegistrationNo", Convert.ToInt64(Session["studentReg"].ToString()));
                     cmd.Parameters.AddWithValue("@ExamID", Convert.ToInt64(Session["Exam_ID"].ToString()));
                     cmd.Parameters.AddWithValue("@PaperspassedCheck", PaperspassedCheck);
                     cmd.ExecuteNonQuery();
                 }
             }
         }
         catch (Exception ex)
         {
             ShowAlert(ex.Message, true);
         }
     }
     protected bool IsValidForm()
     {
         try
         {
             using (EConnectContext context = new EConnectContext())
             {
                 string EmailId = string.Empty;
                 Int64 MobileNumber = 0, CandidateID = 0;
                 int examID = 0;
                 CandidateID = Convert.ToInt64(Session["Candidate_ID"].ToString());
                 MobileNumber = Convert.ToInt64(LblMobileNumber.Text);
                 examID = Convert.ToInt32(Session["Exam_ID"].ToString());
                 EmailId = LblEmailId.Text;

                 // Mobile number checks

                 var ExistsMobileOTP = (from s in context.PuraskarAppMobileEmailOtpVerificationDetails
                                        where s.CandidateID == CandidateID && s.ExamID == examID && s.MobileNumber == MobileNumber && s.IsMobileNumberVerified == false
                                        select new { ID = s.CandidateID, name = s.ExamID }).Distinct();
                 var ExistsMobileOTP1 = (from s in context.PuraskarAppMobileEmailOtpVerificationDetails
                                        where s.CandidateID == CandidateID && s.ExamID == examID && s.MobileNumber == MobileNumber && s.IsMobileNumberVerified == false
                                        select new { ID = s.CandidateID, name = s.ExamID }).Distinct();
                 if (ExistsMobileOTP.Count() == 0)
                 {
                     var ExistsMobileOTP2 = (from s in context.PuraskarAppMobileEmailOtpVerificationDetails
                                             where s.CandidateID == CandidateID && s.ExamID == examID && s.MobileNumber == MobileNumber && s.IsMobileNumberVerified == true
                                             select new { ID = s.CandidateID, name = s.ExamID }).Distinct();

                     if (ExistsMobileOTP2.Count() == 0)
                     {
                         throw new Exception("Please verify the Mobile Number. Click on Send OTP and Verify Mobile Number.");
                     }
                     
                 }
                 if (ExistsMobileOTP1.Count() > 0)
                 {
                     throw new Exception("Please verify the Mobile Number. Click on Send OTP and Verify Mobile Number.");
                 }
                 // Mobile number checks END

                 // Email address checks

                 var ExistsEmailOTP = (from s in context.PuraskarAppMobileEmailOtpVerificationDetails
                                       where s.CandidateID == CandidateID && s.ExamID == examID && s.EmailAddress == EmailId && s.IsEmailVerified == false
                                        select new { ID = s.CandidateID, name = s.ExamID }).Distinct();
                 var ExistsEmailOTP1 = (from s in context.PuraskarAppMobileEmailOtpVerificationDetails
                                        where s.CandidateID == CandidateID && s.ExamID == examID && s.EmailAddress == EmailId && s.IsEmailVerified == false
                                         select new { ID = s.CandidateID, name = s.ExamID }).Distinct();
                 if (ExistsEmailOTP.Count() == 0)
                 {
                     var ExistsEmailOTP2 = (from s in context.PuraskarAppMobileEmailOtpVerificationDetails
                                            where s.CandidateID == CandidateID && s.ExamID == examID && s.EmailAddress == EmailId && s.IsEmailVerified == true
                                             select new { ID = s.CandidateID, name = s.ExamID }).Distinct();

                     if (ExistsEmailOTP2.Count() == 0)
                     {
                         throw new Exception("Please verify the Email ID. Click on Send OTP and Verify Email ID.");
                     }

                 }
                 if (ExistsEmailOTP1.Count() > 0)
                 {
                     throw new Exception("Please verify the Email ID. Click on Send OTP and Verify Email ID.");
                 }
                 // Email address checks END
             }
            
             if (IncomeCerFileUpload.HasFile)
             {
                 String fileExtension = System.IO.Path.GetExtension(IncomeCerFileUpload.FileName).ToLower();
                 if (fileExtension != ".pdf")
                 {
                     throw new Exception("Invalid Income Certificates file. Only pdf extensions are allowed.");
                 }
                 if (!isvalidFileSize(IncomeCerFileUpload, 102400))
                 {
                     throw new Exception("Income Certificates file size should be of 100 KB or less.");
                 }
             }
	 else
                 throw new Exception("Income Certificates file required.");

             if (PHCertUpload.HasFile)
             {
                 String fileExtension = System.IO.Path.GetExtension(PHCertUpload.FileName).ToLower();
                 if (fileExtension != ".pdf")
                 {
                     throw new Exception("Invalid Physically handicapped Certificates file. Only pdf extensions are allowed.");
                 }
                 if (!isvalidFileSize(PHCertUpload, 102400))
                 {
                     throw new Exception("Physically handicapped file size should be of 100 KB or less.");
                 }
             }
             if (CasteCertFileUpload.HasFile)
             {
                 String fileExtension = System.IO.Path.GetExtension(CasteCertFileUpload.FileName).ToLower();
                 if (fileExtension != ".pdf")
                 {
                     throw new Exception("Invalid Caste Certificates file. Only pdf extensions are allowed.");
                 }
                 if (!isvalidFileSize(CasteCertFileUpload, 102400))
                 {
                     throw new Exception("Caste Certificates file size should be of 100 KB or less.");
                 }
             }
             //Added for declaration
             if (chkdisclamier.Checked != true)
             {
                 
                 throw new Exception("Please check the Declaration Statement");
             }
             //Added for income certificate date
             DateTime dt=Convert.ToDateTime(txtIncomeCertDate.Text);
             if (!(dt >= System.DateTime.Today.AddYears(-3) && dt <= System.DateTime.Today))
             {
                 throw new Exception("Income Certificate cannot be older than three years");
             }
             return true;
         }
         catch (Exception ex)
         {
             throw ex;
         }
     }

     public string PapersEligibleCountCourseWise(Int64 courseID)
     {
         string ModuleCount = "0";
         SqlConnection sqlCon = null;
         string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
       
         using (sqlCon = new SqlConnection(constr))
         {
             sqlCon.Open();
             SqlCommand Cmd = new SqlCommand(" SELECT ModulesCount FROM [NIELIT].[dbo].[e_temp_PapersCountEligibleForOnlinePuraskarAppForm] where courseID=@courseID", sqlCon);
             Cmd.Parameters.AddWithValue("@courseID", courseID);
             string ModulesCount = Cmd.ExecuteScalar().ToString();
             if (ModulesCount != null)
             {
                 ModuleCount = ModulesCount.ToString();
             }
             sqlCon.Close();
         }
         return ModuleCount;
     }  

    //6 Oct 2021

     protected void btnsendOTP_Click(object sender, EventArgs e)
        {
            try
            {
                btnMobileVerify.Visible = true;
                string regNo =LblRegnNo.Text;
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 OTP = Convert.ToInt32(EConnect.CommonFunctions.GenerateRandomNumber(6));
                    var contactreg = (from s in context.Users
                                      join k in context.CandidateContactDetails on s.UserRefNumber equals k.CandidateID                                      
                                      where s.LoginID == regNo && k.IsMobileNumberVerified ==true
                                      select new { s.LoginID, s.UserTypeID, k.CandidateID, k.MobileNumber,k.EmailAddress }).FirstOrDefault();                  
			 if(contactreg==null)
                    { throw new Exception("Mobile no. is not verified, Contact Registration section to get it verified"); }

                    Int64 OtpRefNumber = GenerateMobileOTP(contactreg.LoginID, Convert.ToInt64(contactreg.MobileNumber), Convert.ToInt64(contactreg.CandidateID), Convert.ToInt32(Session["Exam_ID"].ToString()), OTP);
                    
                   // String MobileMsg = "Dear " + contactreg.LoginID + ", " + OTP + " is One Time Password for puraskar application form verification. OTP Reference Number: " + OtpRefNumber;
                    String MobileMsg = "OTP for mobile number verification is " + OTP.ToString() + ". Reference Number: " + OtpRefNumber + ". Use this OTP to complete verification process-NIELIT";
                    if (contactreg.MobileNumber != 0)
                    {
                        try
                        {
                            EConnect.NIELIT.SMS message = new SMS(MobileMsg, contactreg.MobileNumber.ToString(), "1307161052940277785", SmsServiceType.SignleSMS, false);                           
                            int sentMessageCount;                         
                            message.sendOTPMSG(out sentMessageCount);
                            TrMobileVerification.Visible = true;
                            if (sentMessageCount <= 0)
                            { throw new Exception("OTP message has not been sent. Try Again"); }
                        }
                        catch { ShowAlert("OTP is not sent on Mobile,"); }
                    }
                }
            }
            catch (Exception ex)
            {
                Label7.Text = ex.Message.ToString();
            }
        }
     protected void btnMobileVerify_Click(object sender, EventArgs e)
     {
         try
         {
             lblerror.Visible = false;
             Int64 MobileNumber = 0, CandidateID=0;
             int examID = 0 , inputOTP=0;
             CandidateID = Convert.ToInt64(Session["Candidate_ID"].ToString());
             MobileNumber = Convert.ToInt64(LblMobileNumber.Text);
             examID = Convert.ToInt32(Session["Exam_ID"].ToString());
             inputOTP = Convert.ToInt32(txtmobileverificationcode.Text);
             using (EConnectContext context = new EConnectContext())
             {
                 PuraskarAppMobileEmailOtpVerificationDetail objPuraskarAppMobileOTPVerification;
                 var ExistsMobileOTP = (from s in context.PuraskarAppMobileEmailOtpVerificationDetails
                                        where s.CandidateID == CandidateID && s.ExamID == examID && s.MobileNumber == MobileNumber && s.Mobile_OTP == inputOTP && s.IsMobileNumberVerified == false
                                        select new { ID = s.CandidateID, name = s.ExamID }).Distinct();
                 if (ExistsMobileOTP.Count() > 0)
                 {
                     objPuraskarAppMobileOTPVerification = context.PuraskarAppMobileEmailOtpVerificationDetails.Where(a => a.CandidateID == CandidateID && a.MobileNumber == MobileNumber && a.Mobile_OTP == inputOTP && a.ExamID == examID && a.IsMobileNumberVerified == false).FirstOrDefault();
                     objPuraskarAppMobileOTPVerification.IsMobileNumberVerified = true;
                     objPuraskarAppMobileOTPVerification.MobileNumberVerifiedOn = DateTime.Now;
                     context.SaveChanges();
                     btnsendOTP.Visible = false;
                     lblConfiramation.Text = "Verified";
                     lblConfiramation.Visible = true;
                     TrMobileVerification.Visible = false;
                 }
                 else
                 {
                     lblerror.Text="Invalid OTP Code";
                     lblerror.Visible = true;
                     lblConfiramation.Visible = false;
                     TrMobileVerification.Visible = true;
                 }
                
             }
         }
         catch (Exception ex)
         {
             Label7.Text = ex.Message.ToString();
         }
     }
     public static Int64 GenerateMobileOTP(string UserId, Int64 MobileNumber, Int64 CandidateID,int examID, int OTP)
     {
         Int64 MobileRefNo=0;
         using (EConnectContext context = new EConnectContext())
         {
             PuraskarAppMobileEmailOtpVerificationDetail objPuraskarAppMobileOTPVerification;

             var ExistsRecords = (from s in context.PuraskarAppMobileEmailOtpVerificationDetails
                                    where s.CandidateID == CandidateID && s.ExamID == examID 
                                    select new { ID = s.CandidateID, name = s.ExamID }).Distinct();
             if (ExistsRecords.Count() > 0)// record exists
             {
                 var ExistsMobileOTP = (from s in context.PuraskarAppMobileEmailOtpVerificationDetails
                                        where s.CandidateID == CandidateID && s.ExamID == examID && s.MobileNumber == MobileNumber && s.IsMobileNumberVerified == false
                                        select new { ID = s.CandidateID, name = s.ExamID }).Distinct();
                 if (ExistsMobileOTP.Count() > 0)
                 {


                     objPuraskarAppMobileOTPVerification = context.PuraskarAppMobileEmailOtpVerificationDetails.Where(a => a.CandidateID == CandidateID && a.MobileNumber == MobileNumber && a.LoginID == UserId && a.ExamID == examID && a.IsMobileNumberVerified == false).FirstOrDefault();
                     objPuraskarAppMobileOTPVerification.Mobile_OTP = OTP;
                     objPuraskarAppMobileOTPVerification.MobileOTPCreatedOn = DateTime.Now;
                     context.SaveChanges();
                     MobileRefNo = objPuraskarAppMobileOTPVerification.Id;
                 }
                 else
                 {
                     PuraskarAppMobileEmailOtpVerificationDetail MobileOtp;
                     MobileOtp = context.PuraskarAppMobileEmailOtpVerificationDetails.Where(a => a.CandidateID == CandidateID  && a.LoginID == UserId && a.ExamID == examID ).FirstOrDefault();                     
                     MobileOtp.MobileNumber = MobileNumber;
                     MobileOtp.Mobile_OTP = OTP;
                     MobileOtp.MobileOTPCreatedOn = DateTime.Now;                  
                     context.SaveChanges();
                     MobileRefNo = MobileOtp.Id;
                 }

             } // not found record then 
             else
             {
                 var ExistsMobileOTP = (from s in context.PuraskarAppMobileEmailOtpVerificationDetails
                                        where s.CandidateID == CandidateID && s.ExamID == examID && s.MobileNumber == MobileNumber && s.IsMobileNumberVerified == false
                                        select new { ID = s.CandidateID, name = s.ExamID }).Distinct();
                 if (ExistsMobileOTP.Count() > 0)
                 {


                     objPuraskarAppMobileOTPVerification = context.PuraskarAppMobileEmailOtpVerificationDetails.Where(a => a.CandidateID == CandidateID && a.MobileNumber == MobileNumber && a.LoginID == UserId && a.ExamID == examID && a.IsMobileNumberVerified == false).FirstOrDefault();
                     objPuraskarAppMobileOTPVerification.Mobile_OTP = OTP;
                     objPuraskarAppMobileOTPVerification.MobileOTPCreatedOn = DateTime.Now;
                     context.SaveChanges();
                     MobileRefNo = objPuraskarAppMobileOTPVerification.Id;
                 }
                 else
                 {
                     PuraskarAppMobileEmailOtpVerificationDetail MobileOtp = new PuraskarAppMobileEmailOtpVerificationDetail();
                     MobileOtp.LoginID = UserId;
                     MobileOtp.CandidateID = CandidateID;
                     MobileOtp.ExamID = examID;
                     MobileOtp.MobileNumber = MobileNumber;
                     MobileOtp.Mobile_OTP = OTP;
                     MobileOtp.MobileOTPCreatedOn = DateTime.Now;
                     context.PuraskarAppMobileEmailOtpVerificationDetails.Add(MobileOtp);
                     context.SaveChanges();
                     MobileRefNo = MobileOtp.Id;
                 }


             }
              return MobileRefNo;
         }
     }
     protected void btnsendOTPEmail_Click(object sender, EventArgs e)
     {
         try
         {
             btnEmailVerify.Visible = true;
             string regNo = LblRegnNo.Text;
             using (EConnectContext context = new EConnectContext())
             {
                 Int32 OTP = Convert.ToInt32(EConnect.CommonFunctions.GenerateRandomNumber(6));
                 var contactreg = (from s in context.Users
                                   join k in context.CandidateContactDetails on s.UserRefNumber equals k.CandidateID
                                   where s.LoginID == regNo && k.IsEmailVerified == true
                                   select new { s.LoginID, s.UserTypeID, k.CandidateID, k.MobileNumber, k.EmailAddress }).FirstOrDefault();                

		 if (contactreg == null)
                 { throw new Exception("Email Id is not verified, Contact Registration section to get it verified"); }

                 Int64 OtpRefNumber = GenerateEmailOTP(contactreg.LoginID, contactreg.EmailAddress, Convert.ToInt64(contactreg.CandidateID), Convert.ToInt32(Session["Exam_ID"].ToString()), OTP);

                 String EmailMsg = "Dear " + contactreg.LoginID + ", " + OTP + " is One Time Password for puraskar application form verification. OTP Reference Number: " + OtpRefNumber;               

                 if (contactreg.EmailAddress.Length > 0)
                 {
                     TrEmailVerification.Visible = true;
                     try
                     {
                         EConnect.NIELIT.Email mail = new Email("Online LogIn:NIELIT", EmailMsg, contactreg.EmailAddress);

                         mail.Send();

                     }
                     catch { ShowAlert("OTP is not sent on E-mail."); }
                 }
             }
         }
         catch (Exception ex)
         {
             Label7.Text = ex.Message.ToString();
         }
     }
     protected void btnEmailVerify_Click(object sender, EventArgs e)
     {
         try
         {
             lblerror.Visible = false;
             string EmailId = string.Empty;
             Int64  CandidateID = 0;
             int examID = 0, inputOTP = 0;
             CandidateID = Convert.ToInt64(Session["Candidate_ID"].ToString());
             EmailId = LblEmailId.Text;
             examID = Convert.ToInt32(Session["Exam_ID"].ToString());
             inputOTP = Convert.ToInt32(txtEmailverificationcode.Text);
             using (EConnectContext context = new EConnectContext())
             {
                 PuraskarAppMobileEmailOtpVerificationDetail objPuraskarAppEmailOTPVerification;

                 var ExistsEmailOTP = (from s in context.PuraskarAppMobileEmailOtpVerificationDetails
                                       where s.CandidateID == CandidateID && s.ExamID == examID && s.EmailAddress == EmailId && s.Email_OTP == inputOTP && s.IsEmailVerified == false
                                        select new { ID = s.CandidateID, name = s.ExamID }).Distinct();
                 if (ExistsEmailOTP.Count() > 0)
                 {
                     objPuraskarAppEmailOTPVerification = context.PuraskarAppMobileEmailOtpVerificationDetails.Where(a => a.CandidateID == CandidateID && a.EmailAddress == EmailId && a.Email_OTP == inputOTP && a.ExamID == examID && a.IsEmailVerified == false).FirstOrDefault();
                     objPuraskarAppEmailOTPVerification.IsEmailVerified = true;
                     objPuraskarAppEmailOTPVerification.EmailVerifiedOn = DateTime.Now;
                     context.SaveChanges();
                     btnsendOTPEmail.Visible = false;
                     lblConfiramationEmail.Text = "Verified";
                     lblConfiramationEmail.Visible = true;
                     TrEmailVerification.Visible = false;
                 }
                 else
                 {
                     lblerrorEmail.Text = "Invalid OTP Code";
                     lblerrorEmail.Visible = true;
                     lblConfiramationEmail.Visible = false;
                     TrEmailVerification.Visible = true;
                 }

             }
         }
         catch (Exception ex)
         {
             Label7.Text = ex.Message.ToString();
         }
     }
     public static Int64 GenerateEmailOTP(string UserId, string EMailID, Int64 CandidateID, int examID, int OTP)
     {
         Int64 EmailRefNo = 0;
         using (EConnectContext context = new EConnectContext())
         {
             PuraskarAppMobileEmailOtpVerificationDetail objPuraskarAppEmailOTPVerification;
              var ExistsRecords = (from s in context.PuraskarAppMobileEmailOtpVerificationDetails
                                    where s.CandidateID == CandidateID && s.ExamID == examID 
                                    select new { ID = s.CandidateID, name = s.ExamID }).Distinct();
              if (ExistsRecords.Count() > 0)// record exists
              {

                  var ExistsEmailOTP = (from s in context.PuraskarAppMobileEmailOtpVerificationDetails
                                        where s.CandidateID == CandidateID && s.ExamID == examID && s.EmailAddress == EMailID && s.IsMobileNumberVerified == false
                                        select new { ID = s.CandidateID, name = s.ExamID }).Distinct();
                  if (ExistsEmailOTP.Count() > 0)
                  {
                      objPuraskarAppEmailOTPVerification = context.PuraskarAppMobileEmailOtpVerificationDetails.Where(a => a.CandidateID == CandidateID && a.EmailAddress == EMailID && a.LoginID == UserId && a.ExamID == examID && a.IsMobileNumberVerified == false).FirstOrDefault();
                      objPuraskarAppEmailOTPVerification.Email_OTP = OTP;
                      objPuraskarAppEmailOTPVerification.EmailOTPCreatedOn = DateTime.Now;
                      context.SaveChanges();
                      EmailRefNo = objPuraskarAppEmailOTPVerification.Id;
                  }
                  else
                  {
                      PuraskarAppMobileEmailOtpVerificationDetail EmailOTP;
                      EmailOTP = context.PuraskarAppMobileEmailOtpVerificationDetails.Where(a => a.CandidateID == CandidateID && a.LoginID == UserId && a.ExamID == examID).FirstOrDefault();                     
                      EmailOTP.EmailAddress = EMailID;
                      EmailOTP.Email_OTP = OTP;
                      EmailOTP.EmailOTPCreatedOn = DateTime.Now;                    
                      context.SaveChanges();
                      EmailRefNo = EmailOTP.Id;
                  }
              }
              else
              {
                  var ExistsEmailOTP = (from s in context.PuraskarAppMobileEmailOtpVerificationDetails
                                        where s.CandidateID == CandidateID && s.ExamID == examID && s.EmailAddress == EMailID && s.IsMobileNumberVerified == false
                                        select new { ID = s.CandidateID, name = s.ExamID }).Distinct();
                  if (ExistsEmailOTP.Count() > 0)
                  {
                      objPuraskarAppEmailOTPVerification = context.PuraskarAppMobileEmailOtpVerificationDetails.Where(a => a.CandidateID == CandidateID && a.EmailAddress == EMailID && a.LoginID == UserId && a.ExamID == examID && a.IsMobileNumberVerified == false).FirstOrDefault();
                      objPuraskarAppEmailOTPVerification.Email_OTP = OTP;
                      objPuraskarAppEmailOTPVerification.EmailOTPCreatedOn = DateTime.Now;
                      context.SaveChanges();
                      EmailRefNo = objPuraskarAppEmailOTPVerification.Id;
                  }
                  else
                  {
                      PuraskarAppMobileEmailOtpVerificationDetail EmailOTP = new PuraskarAppMobileEmailOtpVerificationDetail();
                      EmailOTP.LoginID = UserId;
                      EmailOTP.CandidateID = CandidateID;
                      EmailOTP.ExamID = examID;
                      EmailOTP.EmailAddress = EMailID;
                      EmailOTP.Email_OTP = OTP;
                      EmailOTP.EmailOTPCreatedOn = DateTime.Now;
                      context.PuraskarAppMobileEmailOtpVerificationDetails.Add(EmailOTP);
                      context.SaveChanges();
                      EmailRefNo = EmailOTP.Id;
                  }
              }

             return EmailRefNo;
         }
     }
    //06 oct 2021 end
}