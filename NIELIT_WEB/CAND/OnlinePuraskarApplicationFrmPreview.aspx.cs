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

public partial class OnlinePuraskarApplicationFrmPreview : BasePage
{
    String strMessage = string.Empty;
    Int64  ExamId=0;
    DataTable DtExam = new DataTable();
    DataTable DtPuraskar = new DataTable();
    DataTable DtPaper = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;

        if (IsSessionAlive() == false)
            Response.Redirect("../Index.aspx");
        if (!IsPostBack)
        {  
            ViewRecord();
            if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                ShowAlert(Request.QueryString["msg"].ToString());
        }
      
        BreadCrumb1.Render();
    }

    protected void ViewRecord()
    {
        try
        {
            string RegistrationNo = "0", OnlineRefNo = "0", Preview="N";          
            if (!String.IsNullOrEmpty(Request.QueryString["Reg"]))
            {
               RegistrationNo = Request.QueryString["Reg"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.QueryString["OnlineRefNo"]))
            {
                OnlineRefNo = Request.QueryString["OnlineRefNo"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.QueryString["Preview"]))
            {
                Preview = Request.QueryString["Preview"].ToString();
            } 
            Int32 RegistrationNumber = Convert.ToInt32(RegistrationNo);         
            Int64 CandidateID = 0;
            using (EConnectContext context = new EConnectContext())
            {
                CourseExamApplication objcandidaeID;
                objcandidaeID = context.CourseExamApplications.Where(a => a.RegistrationNumber == RegistrationNumber).FirstOrDefault();

                if (objcandidaeID.CandidateID.ToString() != "")
                {
                    CandidateID = Convert.ToInt64(objcandidaeID.CandidateID);
                }
            }
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            DataTable dt = new DataTable();
            DataTable DtAppred = new DataTable();
            using (SqlConnection conn = new SqlConnection(constr))          
            {               
                using (SqlCommand cmd = new SqlCommand("getCandidateDetails",conn))
                {                    
                    cmd.CommandType = CommandType.StoredProcedure;                    
                    cmd.Parameters.Add(new SqlParameter("@pCandidateID", SqlDbType.Int));
                    cmd.Parameters["@pCandidateID"].Value = CandidateID;                 
                    conn.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {                       
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Rows)
                            {     
                                string dateInString = row["Result_Publish_Date"].ToString();
                                if (dateInString == "")
                                {
                                    dateInString = "1900-01-01";
                                }

                                DateTime startDate = DateTime.Parse(dateInString);
                                DateTime expiryDate = startDate.AddDays(45);

				//Added for extension of Puraskar May,2024
				/*if (startDate.Date == Convert.ToDateTime("2024-03-18").Date)
                 		{
		                   expiryDate = Convert.ToDateTime("2024-05-11");
                 		}*/


                                if (DateTime.Today < expiryDate)
                                {
                                    if (row["Gender"].ToString() == "Female" || row["Cast_Category_ID"].ToString() == "2" || row["Cast_Category_ID"].ToString() == "3" || row["Is_Handicaped"].ToString() == "True")
                                    {
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
                                            LblRegnNo.Text = RegistrationNumber.ToString();
                                            Session["Course_ID"] = Convert.ToInt64(row["Course_ID"].ToString());
                                            Session["AdhaarNo"] = row["Aadhar_Number"].ToString();
                                            Session["Exam_ID"] = Convert.ToInt64(row["Exam_ID"].ToString());
                                            string handicaped =row["Is_Handicaped"].ToString();
                                            if (handicaped=="False")
                                            {
                                               LblIsHandicapped.Text="No";
                                            }
                                            else{
                                                 LblIsHandicapped.Text="Yes";
                                            }

                                            Session["CourseName"] = row["CourseName"].ToString();
                                            Session["Candidate_ID"] = row["Candidate_ID"].ToString();
                                            Session["Institute_ID"] = row["Institute_ID"].ToString();
                                            if ((row["Cast_Category_ID"].ToString() == "2" || row["Cast_Category_ID"].ToString() == "3")  && row["Gender"].ToString() != "Female")
                                            {
                                                CasteCertDetailsInputView.Visible = true;                                                
                                            }
                                            else
                                            {
                                                CasteCertDetailsInputView.Visible = false;
                                            }                                            
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
                                 ////   Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Your due date is already expired. You can't apply the application.')", true);
                                ////    return;
                                    ShowAlert("Your due date is already expired. You can not apply the application.", true);
                                    OnlinePuraskarAppForm.Visible = false;
                                    return;
                                ////    //DateTime Msessage 
                                }
                                Session["Exam_ID"] = Convert.ToInt64(row["Exam_ID"].ToString());
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
                                            //Commented for testing
                                            ////if (Convert.ToInt32(DtChkApptempt.Rows[0]["CountExam"]) != 1)
                                            ////{
                                            ////    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('You are not eligible for this Puraskar Scheme. You must have cleared all modules in First Attempt')", true);
                                            ////    OnlinePuraskarAppForm.Visible = false;
                                            ////    return;
                                            ////}
                                        }
                                    }
                                }                               
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
                                            foreach (DataRow row1 in DtPuraskar.Rows)
                                            {
                                                string OnlineRefNoFromOnlineAppTable = row1["OnlineRefNo"].ToString();
                                               // OnlineRefNo
                                                if (OnlineRefNo != OnlineRefNoFromOnlineAppTable && Preview=="P")
                                                {
                                                    Lblerror.Visible = true;
                                                    Lblerror.Text = "Puraskar Application Form Not Found.";
                                                    return;
                                                }
                                                lblOnlineRefNo.Text = OnlineRefNoFromOnlineAppTable;
                                                // decrypted aadhar number for view in
 
                                                string DecAdh = row1["AadharNumberD"].ToString();
                                                string aadharDecrypted = EncryptDecrypt.DecryptString(DecAdh);
                                              
                                                var firstDigits = aadharDecrypted.Substring(0, 2);
                                                var lastDigits = aadharDecrypted.Substring(aadharDecrypted.Length - 3, 3);
                                                var requiredMask = new String('X', aadharDecrypted.Length - firstDigits.Length - lastDigits.Length);
                                                var FinalAadharDecrypted = string.Concat(firstDigits,  requiredMask,  lastDigits);

                                                var firstDigitsWd = FinalAadharDecrypted.Substring(0, 4);
                                                var lastDigitsWd = FinalAadharDecrypted.Substring(FinalAadharDecrypted.Length - 4, 4);
                                                var requiredMaskWd = new String('X',FinalAadharDecrypted.Length - firstDigitsWd.Length - lastDigitsWd.Length);
                                                var FinalAadharDecryptedWithSpace = string.Concat(firstDigitsWd, " - " + requiredMaskWd, " - " + lastDigitsWd);
                                              
                                                // decrypted aadhar number for view in


                                                LblRegnNo.Text = row1["RegnNo"].ToString();

                                                lblAadhar.Text = FinalAadharDecryptedWithSpace;// row1["AadharNumber"].ToString();

                                                LblAccountNo1.Text = row1["AccountNo"].ToString();
                                                LblAccountHolderName1.Text = row1["AccountHolderName"].ToString();
                                                LblAccountType1.Text = row1["AccountType"].ToString();
                                                LblBankName1.Text = row1["BankName"].ToString();
                                                LblBankAddress1.Text = row1["BankAddress"].ToString();
                                                LblBankIFSC1.Text = row1["BankIFSC"].ToString();
                                               
                                                LblCasteCertNo1.Text = row1["CasteCertNo"].ToString();                                                                                               
                                                if (row1["CasteCertDate"].ToString() != "")
                                                {
                                                    LblCasteCertDate1.Text = Convert.ToDateTime(row1["CasteCertDate"]).ToString("dd-MMM-yyyy");                                                  
                                                }

                                                LblIncomeCertNo1.Text = row1["IncomeCertNo"].ToString();                                                                                             
                                                if (row1["IncomeCertDate"].ToString() != "")
                                                {
                                                    LblIncomeCertDate1.Text = Convert.ToDateTime(row1["IncomeCertDate"]).ToString("dd-MMM-yyyy");                                                    
                                                }
                                                string CasteCertUpload = row1["CasteCertUpload"].ToString(); 
                                               
                                                if (CasteCertUpload != "")
                                                {
                                                    LblCasteCertUpload1.Text = "Yes";
                                                }
                                                else
                                                {
                                                    LblCasteCertUpload1.Text = "No";
                                                }
                                                string IncomeCertUpload = row1["IncomeCertUpload"].ToString();
                                                if (IncomeCertUpload != "")
                                                {
                                                    LblIncomeCertUpload1.Text = "Yes";
                                                }
                                                else
                                                {
                                                    LblIncomeCertUpload1.Text = "No";
                                                }

                                                LblAnnualIncome1.Text = row1["AnnualIncome"].ToString();
                                                LblPHCertNo1.Text = row1["PHCertNo"].ToString();

                                                string PHCertUpload = row1["PHCertUpload"].ToString();
                                                if (PHCertUpload != "")
                                                {
                                                    LblPHCertUpload1.Text = "Yes";
                                                }
                                                else
                                                {
                                                    LblPHCertUpload1.Text = "No";
                                                }                                             

                                                if (row1["PHCertDate"].ToString() != "")
                                                {
                                                    lblPHCertDate1.Text = Convert.ToDateTime(row1["PHCertDate"]).ToString("dd-MMM-yyyy");                                                   
                                                }                                               
                                                Int64 candidateid=Convert.ToInt64( row1["CandidateID"].ToString());
                                                Session["Exam_ID"] = Convert.ToInt64(row1["ExamID"].ToString());
                                                FilledPapersPassed(candidateid);
                                                //Lblerror.Text = "Dear Candidate, this is preview of Puraskar Application form you are applying for. Please check all details in preview form and click on 'Final Submit' button if all the details are correct and you are satisfied with details shown in preview form. If you want to change the details click on 'Back' button to go back to Puraskar form.<br> Once you click on 'Final Submit button, you can not modify the details.";
                                                Lblerror.Text = "Dear Candidate, Below is the preview of Puraskar Application form applied by you. Kindly check all the details and in case any modifications are required,  click on 'Back' button and update. In case all details are correct, please submit the form by clicking at 'Final Submit' button.<br> NOTE:- No modifications will be allowed after final submission of the form.";
                                               
                                                Lblerror.Visible = true;
                                                                                          
                                                if (row1["OnlineRefNo"].ToString() != "")
                                                {
                                                    Session["onlineRefID"] = row1["OnlineRefNo"].ToString();
                                                }
                                                Btnsubmit.Text = "Final Submit";

                                                if ( Preview == "P")
                                                {
                                                    headerP.Visible = true;
                                                    lblHeading.Visible = false;
                                                    BtnPrint.Visible = true;
                                                    Lblerror.Visible = false;
                                                    divfooter.Visible = false;  
                                                }
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
            throw ex;
        }


    }
       protected void Btnsubmit_Click(object sender, EventArgs e)
        {
        try
        {
            Int64 CandidateId = 0;
            string Onlinerefno = string.Empty;
            if (!String.IsNullOrEmpty(Request.QueryString["CandidateId"]))
            {
                CandidateId =Convert.ToInt64( Request.QueryString["CandidateId"].ToString());
            }
            if (!String.IsNullOrEmpty(Request.QueryString["ExamId"]))
            {
                ExamId = Convert.ToInt64(Request.QueryString["ExamId"].ToString());
            }

            using (EConnectContext context = new EConnectContext())
            {
                var objpuraskarApplication = (from s in context.PuraskarApplicationForms
                                              where s.CandidateID == CandidateId && s.ExamID == ExamId
                                              select s).FirstOrDefault();
		 if (objpuraskarApplication.IncomeCertUpload == null)
                {
                    ShowAlert("Please upload certificates again and update ,then final submit");
                    return;

                }
         if (objpuraskarApplication.CasteCertUpload == null && objpuraskarApplication.CasteCertNo !=null)
         {
             ShowAlert("Please upload  certificates again and update ,then final submit");
             return;

         }
         if (objpuraskarApplication.PHCertUpload  == null && objpuraskarApplication.PHCertNo  != null)
         {
             ShowAlert("Please upload certificates again and update ,then final submit");
             return;

         }


                objpuraskarApplication.finalSubmit = true;
                objpuraskarApplication.Final_Submission_Date = DateTime.Now;
                context.Entry(objpuraskarApplication).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
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
                 Onlinerefno = Session["CourseName"].ToString().Trim() + "/" + ExamCycles + "/" + Id; //O LEVEL/JULY-2017/10001
                FetchIdForRef.OnlineRefNo = Onlinerefno;
                context.SaveChanges();
                Session["onlineRefID"] = "";
                strMessage = "Your Puraskar Applicattion is Successfully submitted with Online Reference ID is  <br> " +"'" + Onlinerefno +"'";

                //Response.Redirect("OnlinePuraskarApplicationForm.aspx?FinalSumbited=YES" + "&strMessage=" + strMessage + "&Reg=" + Convert.ToInt64(LblRegnNo.Text) + "&OnlineRefNo=" + Onlinerefno, true);
              // Server.Transfer("~/CAND/OnlinePuraskarApplicationForm.aspx?FinalSumbited=YES" + "&strMessage=" + strMessage + "&Reg=" + Convert.ToInt64(LblRegnNo.Text) + "&OnlineRefNo=" + Onlinerefno, true);

                // Context.ApplicationInstance.CompleteRequest(); LblRegnNo
      
            }
            string regnn = LblRegnNo.Text;
            //Response.Redirect("OnlinePuraskarApplicationForm.aspx?FinalSumbited=YES" + "&strMessage=" + strMessage + "&Reg=" + Convert.ToInt64(regnn) + "&OnlineRefNo=" + Onlinerefno);
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("OnlinePuraskarApplicationForm.aspx?FinalSumbited=YES" + "&strMessage=" + strMessage + "&Reg=" + Convert.ToInt64(regnn) + "&OnlineRefNo=" + Onlinerefno));
           // Response.Redirect("BatchItems.aspx?msg=" + strMessage);
            //Context.ApplicationInstance.CompleteRequest();
             }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message.ToString() + ex.Source.ToString(); 
        }
       }
       protected void Btnback_Click(object sender, EventArgs e)
       {           
           Response.Redirect("OnlinePuraskarApplicationForm.aspx?Reg="+Convert.ToInt64(LblRegnNo.Text), true);
       }
    public void FilledPapersPassed(Int64 CandidateID)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection Conn = new SqlConnection(constr))
            {
                ExamId = Convert.ToInt64(Session["Exam_ID"].ToString());
                //using (SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (Order by PapersPassed) AS SLNo, PapersAppeared FROM [NIELIT].[dbo].[e_temp_OnlinePuraskarApplication_PapersPassDetailsByCand]WHERE PaperspassedCheck=1  and Candidate_ID= " + CandidateID + " and Exam_ID=" + ExamId, Conn))
                //November_2024
                using (SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER (Order by PapersPassed) AS SLNo, PapersAppeared FROM [NIELIT].[dbo].[e_temp_OnlinePuraskarApplication_PapersPassDetailsByCand] WHERE PaperspassedCheck=1  and Candidate_ID= @CandidateID and Exam_ID=@pExamID " , Conn))
                {
                    Conn.Open();
                    cmd.CommandType = CommandType.Text;                  
                    cmd.Parameters.AddWithValue("@CandidateID", CandidateID);
                    cmd.Parameters.AddWithValue("@pExamID", ExamId);
                    cmd.ExecuteNonQuery();
                    using (SqlDataAdapter sd = new SqlDataAdapter(cmd))
                    {
                        sd.Fill(DtPaper);

                        if (DtPaper.Rows.Count > 0)
                        {
                            foreach (DataRow row1 in DtPaper.Rows)
                            {
                                if (lblpaperaap1.Text == "")
                                {
                                    lblpaperaap1.Text = row1["PapersAppeared"].ToString();
                                }
                                else
                                {
                                    lblpaperaap1.Text += "<br>" + row1["PapersAppeared"].ToString();
                                }
                            }
                        }
                        else
                        {
                            lblpaperaap1.Text = "NA";
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
     }
}