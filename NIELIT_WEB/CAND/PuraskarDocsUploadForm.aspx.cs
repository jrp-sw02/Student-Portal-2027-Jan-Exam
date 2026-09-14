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

public partial class PuraskarDocsUploadForm : BasePage
{
    int countModules = 0;
    String strMessage = string.Empty;
    DataTable DtExam = new DataTable();
    DataTable DtPuraskar = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;

        if (IsSessionAlive() == false)
            Response.Redirect("../Index.aspx");
        if (!IsPostBack)
        {


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
                //// test for s
                //var studata = (from s in context.PuraskarApplicationForms
                //               where s.CandidateID !=null
                //             select new
                //                 {
                //                     CanID = s.CandidateID,
                //                     Name=s.Name,
                //                     Examid=s.Name
                //                     }).ToList();

                //var moduledata =(from m in context.OnlineProtsahanExamModuless
                //                 where m.Candidate_ID == 1085109 || m.Candidate_ID == 1085270
                //                select new
                //                 {
                //                     cID = m.Candidate_ID
                //                     }).ToList().Distinct();

                //var filterData = (from d in moduledata
                //                 join stu in studata on d.cID equals stu.CanID
                //             select new
                //                 {
                //                     CanID = stu.CanID,
                //                     Name = stu.Name,
                //                     Examid = stu.Examid
                //                     }).ToList();

                ////test for s




                CourseExamApplication objcandidaeID;
                objcandidaeID = context.CourseExamApplications.Where(a => a.RegistrationNumber == RegistrationNumber).FirstOrDefault();
                if (objcandidaeID == null)
                {
                    ShowAlert("No exam details found");
                    PuraskarDocUpload.Visible = false;
                    return;
                }

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
                // from course registration application fetch examid with candidate id only for institute candidates
                using (SqlCommand cmd = new SqlCommand("getCandidateDetails", conn))
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
                                //  FOR ANY PAPER FAIL WITH EXAM ID WISE, NOT ELIGIBLE FOR PURUSKAR APPLICATION
                                // DataTable dt1 = new DataTable();
                                // using (SqlCommand cmdPapperFailCheck = new SqlCommand("CheckAllModuleFirstAttempPass", conn)) // IF O THEN PROCESS OTHER WISE NOT ELIGIBLE
                                // {
                                //     cmdPapperFailCheck.CommandType = CommandType.StoredProcedure;
                                //     cmdPapperFailCheck.Parameters.AddWithValue("@pCandidateID", Convert.ToInt64(row["Candidate_ID"].ToString()));
                                //     using (SqlDataAdapter sdaPaperFailCheck = new SqlDataAdapter(cmdPapperFailCheck))
                                //     {
                                //         sdaPaperFailCheck.Fill(dt1);
                                //         if (dt1.Rows.Count > 0)
                                //         {
                                //             PuraskarDocUpload.Visible = false;
                                //             divSumbitMsg.Visible = true;
                                //             LblSubmitMessage.Text = "Documents already uploaded.";
                                ////Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('You are not eligible.!')", true);
                                //             return;    
                                //         }
                                //     }
                                // }

                                // end                                


                                // Checks course id O=1 , A=2, B=3, C=4 LEVEL
                                if (row["Course_ID"].ToString() == "1" || row["Course_ID"].ToString() == "2" || row["Course_ID"].ToString() == "3" || row["Course_ID"].ToString() == "4")
                                {
                                    lblName.Text = row["Name"].ToString();
                                    LblDecMName.Text = row["Name"].ToString();
                                    lblParent_GuardianDetails.Text = row["Father_Name"].ToString();

                                    lblGender.Text = row["Gender"].ToString();
                                    lblDOB.Text = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(row["Dob"]));
                                    LblCaste.Text = row["CasteName"].ToString();
                                    //LblEmailId.Text = row["Email"].ToString();
                                    //LblMobileNumber.Text = row["Mobile"].ToString();
                                    //LblInstituteDetails.Text = row["InsName"].ToString();
                                    // LblInstitute.Text = row["InsName"].ToString();
                                    LblRegnNo.Text = RegistrationNumber.ToString();
                                    Session["Exam_ID"] = Convert.ToInt64(row["Exam_ID"].ToString());
                                    Session["Course_ID"] = Convert.ToInt64(row["Course_ID"].ToString());
                                    Session["AdhaarNo"] = row["Aadhar_Number"].ToString();
                                    // Session["AdhaarNo"] = "";
                                    if (Session["AdhaarNo"].ToString() == "")
                                    {
					 ShowAlert("Addhaar Not available");
                                        PuraskarDocUpload.Visible = false;
                                    }


                                    //  ddlPWD.Items.FindByValue(row["Is_Handicaped"].ToString()).Selected = true;
                                    Session["CourseName"] = row["CourseName"].ToString();
                                    Session["Candidate_ID"] = row["Candidate_ID"].ToString();
                                    Session["Institute_ID"] = row["Institute_ID"].ToString();
                                   

                                    //if (ddlPWD.SelectedValue.ToString() == "True")
                                    //{
                                    //    PHCertDetailsInputView.Visible = true;                                                                                          
                                    //}
                                    //else
                                    //{
                                    //    PHCertDetailsInputView.Visible = false;
                                    //}
                                    int examID = 0;
                                    string EmailId = string.Empty;
                                    CandidateID = Convert.ToInt64(Session["Candidate_ID"].ToString());
                                    examID = Convert.ToInt32(Session["Exam_ID"].ToString());
                                }
                                else
                                {
                                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('The course must be from O , A, B , C Level Only!')", true);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('You are not eligible for Documents Upload.')", true);
                            PuraskarDocUpload.Visible = false;
                            return;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Something is wrong. Please check after some times.')", true);

        }
    }

    public string GetFinalSubmittedYear(Int64 CandID)
    {
        string AppSubmittedYear = "0";
        SqlConnection sqlCon = null;
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

        using (sqlCon = new SqlConnection(constr))
        {
            sqlCon.Open();
            SqlCommand Cmd = new SqlCommand("select top 1  year( Final_Submission_Date)  FROM [NIELIT].[dbo].[Course_Exam_Application] where Candidate_ID = @pCandidateID and Final_Submitted=1 order by year( Final_Submission_Date) desc", sqlCon);
             Cmd.Parameters.AddWithValue("@pCandidateID", CandID);
            string FSubmittedYear = Cmd.ExecuteScalar().ToString();
            if (FSubmittedYear != null)
            {
                AppSubmittedYear = FSubmittedYear.ToString();
            }
            sqlCon.Close();
        }
        return AppSubmittedYear;
    }  
    protected void ddlLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int lvl = Convert.ToInt32(ddlLevel.SelectedValue);
            Int64 CandidateId = 0, courseid = 0;
            int mCount = 0;
            CandidateId = Convert.ToInt64(Session["Candidate_ID"].ToString());
            courseid = Convert.ToInt64(Session["Course_ID"].ToString());
            DataTable dt1 = new DataTable();
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            DataTable dt = RecordExists();
            DataTable dt2 = CountModule();
            int FinalSubmittedYear =Convert.ToInt32(GetFinalSubmittedYear(CandidateId));
            if (courseid == 1)
            {
                mCount = 4;
            }
            else if (courseid == 2)
            {
                mCount = 10;
            }
            else if (courseid == 3)
            {
                mCount = 25;
            }
            
            using (SqlConnection conn = new SqlConnection(constr))
            {
                using (SqlCommand cmdPapperFailCheck = new SqlCommand("CheckAllModuleFirstAttempPass", conn))
                {
                    cmdPapperFailCheck.CommandType = CommandType.StoredProcedure;
                    cmdPapperFailCheck.Parameters.AddWithValue("@pCandidateID", CandidateId);
                    cmdPapperFailCheck.Parameters.AddWithValue("@courseid", courseid);
                    cmdPapperFailCheck.Parameters.AddWithValue("@View", 1);
                    using (SqlDataAdapter sdaPaperFailCheck = new SqlDataAdapter(cmdPapperFailCheck))
                    {
                        sdaPaperFailCheck.Fill(dt1);
                        if (lvl == 0)
                        {
                            tblDoc.Visible = false;
                            btnSave.Visible = false;
                            tblmsg.Visible = false;
                            LblSubmitMessage.Text = "";
                        }
                        else if (dt1.Rows.Count > 0)
                          //  else if (dt1.Rows.Count < 0)
                        {
                            tblDoc.Visible = false;
                            btnSave.Visible = false;
                            tblmsg.Visible = true;
                            LblSubmitMessage.Text = "You are not eligible for Documents Upload !!";
                        }
                        //else if (dt2.Rows.Count < mCount)
                        else if (dt2.Rows.Count > mCount)
                        {
                            tblDoc.Visible = false;
                            btnSave.Visible = false;
                            tblmsg.Visible = true;
                            LblSubmitMessage.Text = "You are not eligible for Documents Upload  !!";
                        }
                        else if (FinalSubmittedYear < 2023)
                        {
                            tblDoc.Visible = false;
                            btnSave.Visible = false;
                            tblmsg.Visible = true;
                            LblSubmitMessage.Text = "You are not eligible for Documents Upload  !!";
                        }
                        else if (dt.Rows.Count > 0)
                        {
                            tblDoc.Visible = false;
                            btnSave.Visible = false;
                            tblmsg.Visible = true;
                            LblSubmitMessage.Text = "Documents already uploaded !!";
                        }
                        else
                        {
                            bindState();
                            tblDoc.Visible = true;
                            btnSave.Visible = true;
                            tblmsg.Visible = false;
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
    protected void ddlContryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(ddlContryName.SelectedValue);
            if (id == 2)
            {
                txtContryName.Visible = true;
                txtContryState.Visible = true;
                txtContryDis.Visible = true;
                ddlCmpDistrict.Enabled = false;
                ddlCmpState.Enabled = false;
                ddlCmpState.SelectedValue = "0";
                ddlCmpDistrict.SelectedValue = "0";
            }
            else
            {
                txtContryName.Visible = false;
                txtContryState.Visible = false;
                txtContryDis.Visible = false;
                ddlCmpDistrict.Enabled = true;
                ddlCmpState.Enabled = true;
                txtContryName.Text = "";
                txtContryState.Text = "";
                txtContryDis.Text = "";
            }           
        }
        catch (Exception ex)
        {
            
        }
    }
    protected void ddlCmpState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id1 = Convert.ToInt32(ddlCmpState.SelectedValue);
            
            bindDistrict(id1, ref ddlCmpDistrict);
        }
        catch (Exception ex)
        {
            
        }
    }
    protected void bindState()
    {
        ListItem lst = new ListItem("--Select One--", "0");
        using (EConnectContext context = new EConnectContext())
        {
            var CorState = (from s in context.Locations
                            orderby (s.Name)
                            where s.LocationTypeID == 2
                            && s.ParentLocationID == 1
                            select new { ValueField = s.ID, TextField = s.Name }).ToList();
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCmpState, CorState, lst);
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlBankState, CorState, lst);

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
    public DataTable RecordExists()
    {
        Int64 CandidateId = 0, courseid = 0;
        CandidateId = Convert.ToInt64(Session["Candidate_ID"].ToString());
        courseid = Convert.ToInt64(Session["Course_ID"].ToString());
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new System.Data.DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("CheckAllModuleFirstAttempPass", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCandidateID", CandidateId);
                cmd.Parameters.AddWithValue("@courseid", courseid);
                cmd.Parameters.AddWithValue("@View", 2);
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
    public DataTable CountModule()
    {
        Int64 CandidateId = 0, courseid = 0;
        CandidateId = Convert.ToInt64(Session["Candidate_ID"].ToString());
        courseid = Convert.ToInt64(Session["Course_ID"].ToString());
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new System.Data.DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("CheckAllModuleFirstAttempPass", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCandidateID", CandidateId);
                cmd.Parameters.AddWithValue("@courseid", courseid);
                cmd.Parameters.AddWithValue("@View", 4);
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
    public DataTable GetData(Int64 CanId, Int64 Cid)
    {
        Int64 examID = 0;
        examID = Convert.ToInt64(Session["Exam_ID"].ToString());
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new System.Data.DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("CheckAllModuleFirstAttempPass", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCandidateID", CanId);
                cmd.Parameters.AddWithValue("@courseid", Cid);
                cmd.Parameters.AddWithValue("@View", 3);
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            if (chkdisclamier.Checked != true)
            {
                throw new Exception("Please check the Declaration Statement!!");
                return;
            }
            if (ddlContryName.SelectedValue == "2" && txtContryName.Text == "")
            {
                throw new Exception("Country Name Required !");
                return;
            }
            
            if (ddlCmpState.SelectedValue == "0" && txtContryState.Text == "")
            {
                throw new Exception("Country State Name Required !");
                return;
            }
           

            if (ddlCmpDistrict.SelectedValue == "0" && txtContryDis.Text == "")
            {
                throw new Exception("Country District Name Required !");
                return;
            }
           
            byte[] SalarySlipUpload = null;
            byte[] BankStatementUpload = null;
            byte[] AppointmentLetterUploadF = null;          
            string BankStatementUploadFileName = "", SalarySlipUploadFileName = "", AppointmentLetterUploadFileName = "";
            Int64 CandidateId = Convert.ToInt64(Session["Candidate_ID"].ToString());
            Int64 ExamId = Convert.ToInt64(Session["Exam_ID"].ToString());
            string onlineRefno = ""; Int64 examidOPA = 0;
            Int64 courseId = Convert.ToInt64(Session["Course_ID"]);              
                        if (SalarySlipFileUpload.HasFile)
                        {
                            //size
                            String fileExtension = System.IO.Path.GetExtension(SalarySlipFileUpload.FileName).ToLower();
                            if (fileExtension != ".pdf")
                            {
                                throw new Exception("Invalid Salary Slip file. Only pdf extensions are allowed.");
                            }
                            if (!isvalidFileSize(SalarySlipFileUpload, 102400))
                            {
                                throw new Exception("Income Salary Slip file size should be of 100 KB or less.");
                            }

                            if (SalarySlipFileUpload.FileName.Length > 50)
                            {
                                SalarySlipUploadFileName = SalarySlipFileUpload.FileName.Substring(0, 100) + System.IO.Path.GetExtension(SalarySlipFileUpload.FileName).ToLower();
                            }
                            else
                                SalarySlipUploadFileName = SalarySlipFileUpload.FileName; 
                           
                            SalarySlipUploadFileName = String.Concat(SalarySlipUploadFileName.Where(c => !Char.IsWhiteSpace(c)));
                            //string output = SalarySlipUploadFileName.Replace("[^a-zA-Z0-9]", " ");
                            SalarySlipUpload = SalarySlipFileUpload.FileBytes;
                        }
                        else
                        {
                            ShowAlert("Please Upload the Salary Slip.", true);
                            return;
                        }
                    

                    if (BankStatementFileUpload.HasFile)
                    {
                        //size
                        String fileExtension = System.IO.Path.GetExtension(BankStatementFileUpload.FileName).ToLower();
                        if (fileExtension != ".pdf")
                        {
                            throw new Exception("Invalid Bank Statement file. Only pdf extensions are allowed.");
                        }
                        if (!isvalidFileSize(BankStatementFileUpload, 102400))
                        {
                            throw new Exception("Income Bank Statement file size should be of 100 KB or less.");
                        }
                        if (BankStatementFileUpload.FileName.Length > 50)
                        {
                            BankStatementUploadFileName = BankStatementFileUpload.FileName.Substring(0, 100) + System.IO.Path.GetExtension(BankStatementFileUpload.FileName).ToLower();
                        }
                        else
                            BankStatementUploadFileName = BankStatementFileUpload.FileName;
                        BankStatementUpload = BankStatementFileUpload.FileBytes;
                    }
                    else
                    {
                        ShowAlert("Please Upload the Bank Statement.", true);
                        return;
                    }

                    if (AppointmentLetterUpload.HasFile)
                    {
                        //size
                        String fileExtension = System.IO.Path.GetExtension(AppointmentLetterUpload.FileName).ToLower();
                        if (fileExtension != ".pdf")
                        {
                            throw new Exception("Invalid Appointment Letter file. Only pdf extensions are allowed.");
                        }
                        if (!isvalidFileSize(AppointmentLetterUpload, 102400))
                        {
                            throw new Exception("Income Appointment Letter file size should be of 100 KB or less.");
                        }
                        if (AppointmentLetterUpload.FileName.Length > 50)
                        {
                            AppointmentLetterUploadFileName = AppointmentLetterUpload.FileName.Substring(0, 100) + System.IO.Path.GetExtension(AppointmentLetterUpload.FileName).ToLower();
                        }
                        else
                            AppointmentLetterUploadFileName = AppointmentLetterUpload.FileName;
                        AppointmentLetterUploadF = AppointmentLetterUpload.FileBytes;
                    }
                    else
                    {
                        ShowAlert("Please Upload the Appointment Letter.", true);
                        return;
                    }
                   // return;
                    using (DataTable dt = GetData(CandidateId, courseId))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int k = 0; k < dt.Rows.Count; k++)
                            {
                                onlineRefno = dt.Rows[k]["OnlineRefNo"].ToString();
                                examidOPA = Convert.ToInt64(dt.Rows[k]["ExamID"].ToString());
                            }
                        }
                        else
                        {
                            ShowAlert("Protsahan Form Not filled", true);
                            return;
                        }
                   
                    }

                    string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                    using (SqlConnection Conn = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("InsertPuraskarDocsDetails", Conn))
                        {
                            Conn.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@OnlineRefNo", onlineRefno);
                            cmd.Parameters.AddWithValue("@CandidateID", Convert.ToInt64(Session["Candidate_ID"].ToString()));
                            cmd.Parameters.AddWithValue("@RegnNo", Convert.ToInt64(Session["studentReg"].ToString()));
                            cmd.Parameters.AddWithValue("@ExamID", examidOPA);
                            cmd.Parameters.AddWithValue("@Name", lblName.Text);
                            cmd.Parameters.AddWithValue("@Course_ID", courseId);
                            cmd.Parameters.AddWithValue("@AppointmentLetterFile", AppointmentLetterUploadFileName);
                            cmd.Parameters.AddWithValue("@AppointmentLetterUpload", AppointmentLetterUploadF);
                            cmd.Parameters.AddWithValue("@AppointmentLetterUploadDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@SalarySlipFile", SalarySlipUploadFileName);
                            cmd.Parameters.AddWithValue("@SalarySlipUpload", SalarySlipUpload);
                            cmd.Parameters.AddWithValue("@SalarySlipUploadDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@BankStatementFile", BankStatementUploadFileName);
                            cmd.Parameters.AddWithValue("@BankStatementUpload", BankStatementUpload);
                            cmd.Parameters.AddWithValue("@BankStatementUploadDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@SelfCertification", 1);
                            cmd.Parameters.AddWithValue("@EnterBy", Convert.ToInt32(Session["UserID"]));
                            cmd.Parameters.AddWithValue("@EnterDate", DateTime.Now);

                            cmd.Parameters.AddWithValue("@CompanyName", TxtCompanyName.Text);
                            cmd.Parameters.AddWithValue("@CompanyAddress1", txtAddress1.Text);
                            cmd.Parameters.AddWithValue("@CompanyAddress2", txtAddress2.Text);
                            if (txtAddress3.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@CompanyAddress3", txtAddress3.Text);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@CompanyAddress3", DBNull.Value);
                            }
                            if (txtContryName.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@CompanyContryName", txtContryName.Text);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@CompanyContryName", ddlContryName.SelectedItem.Text);
                            }
                            
                            cmd.Parameters.AddWithValue("@CityName", txtCityName.Text);
                           
                            if (txtContryState.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@State", txtContryState.Text);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@State", ddlCmpState.SelectedItem.Text);
                            }
                            if (txtContryDis.Text != "")
                            {
                                cmd.Parameters.AddWithValue("@District", txtContryDis.Text);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@District", ddlCmpDistrict.SelectedItem.Text);
                            }
                           
                            cmd.Parameters.AddWithValue("@PinCode", txtCmpPinCode.Text);
                            cmd.Parameters.AddWithValue("@CompanyEmail", txtCompanyEmail.Text);
                            cmd.Parameters.AddWithValue("@CompanyPhone", txtCompanyPhone.Text);
                            cmd.Parameters.AddWithValue("@OfferLetterNo", txtOfferletterno.Text);
                            cmd.Parameters.AddWithValue("@PlacementDate", txtPlacementDate.Text);
                            cmd.Parameters.AddWithValue("@PlacementUpto", txtPlacementUpto.Text);
                            cmd.Parameters.AddWithValue("@SalarybeingGiven", txtSalarybeingGiven.Text);
                            cmd.Parameters.AddWithValue("@BankAccountNumber", txtBankAcNo.Text);
                            cmd.Parameters.AddWithValue("@BankName", txtBankName.Text);
                            cmd.Parameters.AddWithValue("@BankCity", txtBankCity.Text);
                            cmd.Parameters.AddWithValue("@BankState", ddlBankState.SelectedItem.Text);
                            int i = 0;
                              i =  cmd.ExecuteNonQuery();

                          if (i > 0)
                          {
                              tblDoc.Visible = false;
                              btnSave.Visible = false;
                              tblmsg.Visible = true;
                              LblSubmitMessage.Text = "Documents uploaded successfully !!";
                          }
                        }
                    }                  
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected bool isvalidFileSize(FileUpload ImgUpload1, int allowedSize)
    {

        String path = Server.MapPath("~/UploadedFilesFolder/");
        if (ImgUpload1.HasFile)
        {
            HttpPostedFile postedfile = ImgUpload1.PostedFile;
            if (postedfile.ContentLength > allowedSize)
            {
                return false;

            }
            else
                return true;
        }
        return true;

    }
}