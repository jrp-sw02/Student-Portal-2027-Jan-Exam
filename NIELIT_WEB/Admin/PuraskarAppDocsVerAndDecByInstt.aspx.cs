using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Transactions;
using System.Data;
using EConnect.Utils.Common;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.IO.Compression;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data.Objects;
using System.Text;
using System.Text.RegularExpressions;
using EConnect.URM;
using System.Data.Entity.Validation;
using System.IO;
using System.Data.OleDb;
using System.Security.Cryptography;

public partial class Admin_PuraskarAppDocsVerAndDecByInstt : BasePage  
{    
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;    
    ArrayList TempDataTable = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);  
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/PuraskarApplicationFormVerificationByInstitute.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }   
            
            if (!Page.IsPostBack)
            {
		if (System.DateTime.Today > Convert.ToDateTime("2022-12-25"))
                {
                    ShowAlert("Date for verification is over, You can only view the details");
                    btnSaveStatus.Visible = false;
                    btnUpdateModules.Visible = false;
                }
                else
                {
                    btnSaveStatus.Visible = true;
                    btnUpdateModules.Visible = true;
                }
                BindAppDataView();  
                 
                    if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Online Puraskar Application", "Admin/PuraskarApplicationFormVerificationByInstitute.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Online Puraskar Application", "Admin/PuraskarApplicationFormVerificationByInstitute.aspx", ""));
                    }
               
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }  
      
    protected void ddlVerifieds_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            DropDownList duty = (DropDownList)gvr.FindControl("ddlVerifieds");
            Int32 candStatus =Convert.ToInt32( duty.SelectedItem.Value);
            Label VerifyStatus = (Label)gvr.FindControl("Label2");            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
    public DataTable FillGridViewOnlinePuraskarApplicationRecordExamiIdWise()
    {

        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("FillGridViewOnlinePuraskarApplicationRecordExamiIdWise", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CodeForViewRecord", SqlDbType.Int));
                cmd.Parameters["@CodeForViewRecord"].Value = 12;
                cmd.Parameters.Add(new SqlParameter("@InstituteID", SqlDbType.BigInt));
                cmd.Parameters["@InstituteID"].Value = 0;
                cmd.Parameters.Add(new SqlParameter("@ExamMonthYear", SqlDbType.VarChar));
                cmd.Parameters["@ExamMonthYear"].Value = "18471";
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
        
    public DataTable FillGridViewRegistraionNoAndExamiIdWise(Int64 ExamID, Int64 regno)
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {           
            using (SqlCommand cmd = new SqlCommand("getPapersAppearedPassedDetailsViewByExamFinance", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;               
                cmd.Parameters.Add(new SqlParameter("@Regno", SqlDbType.BigInt));
                cmd.Parameters["@Regno"].Value = regno;
                cmd.Parameters.Add(new SqlParameter("@pExamID", SqlDbType.BigInt));
                cmd.Parameters["@pExamID"].Value = ExamID;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }

    public DataTable FillNestedGridViewRegNoAndExamiId(Int64 ExamID, Int64 regno)
    {

        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {            
            using (SqlCommand cmd = new SqlCommand("GetUpdateNestedGridViewRegNoAndExamiIdModules", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;             
                cmd.Parameters.Add(new SqlParameter("@Regno", SqlDbType.BigInt));
                cmd.Parameters["@Regno"].Value = regno;
                cmd.Parameters.Add(new SqlParameter("@pExamID", SqlDbType.BigInt));
                cmd.Parameters["@pExamID"].Value = ExamID;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
    protected void BindAppDataView()
    {
        try
        {
            //EConnect.Utils.Security.QuertStringModule.Encrypt
            //Int64 ExamId2 = Convert.ToInt64(Decrypt(HttpUtility.UrlDecode(Request.QueryString["Examid"])));
            Int64 ExamId2 = Convert.ToInt64(EConnect.Utils.Security.QuertStringModule.Decrypt(Request.QueryString["Examid"]));
            Int64 ExamId = Convert.ToInt64(Request.QueryString["Examid"]);
            Int64 RegistrationNo = Convert.ToInt64(Request.QueryString["RegistrationNo"]);


		//Added for expiry check
            int expired = Convert.ToInt32(Request.QueryString["expired"]);
            if(expired==1)
            {
                ShowAlert("Date for verification is over, You can only view the details");
                   btnSaveStatus.Visible = false;
                    btnUpdateModules.Visible = false;
                }
                else
                {
                    btnSaveStatus.Visible = true;
                    btnUpdateModules.Visible = false;
                }
                //End Expiry check

            using (EConnectContext context = new EConnectContext())
            {
                var CandidateDetails = (from s in context.CourseExamApplications
                               join c in context.Candidates on s.CandidateID equals c.ID
                               join cc in context.Courses on s.CourseID equals cc.ID
                                        where s.RegistrationNumber == RegistrationNo
                               && s.ExamID == ExamId
                               && s.FinalSubmitted == true                               
                               select new
                               {
                                   ID = s.CandidateID,
                                   ReggnC=s.RegistrationNumber,  
                                   CandidateName=c.Name,
                                   fname = c.FatherName.ToString(),
                                   Gname = c.GuardianName.ToString(),
                                   CourseName = cc.Name                                  
                               }).FirstOrDefault();
                if (CandidateDetails != null)
                {
                    lblRegnNo.Text = CandidateDetails.ReggnC.ToString();
                    lblCanName.Text = CandidateDetails.CandidateName;
                    lbllvvl.Text = CandidateDetails.CourseName;
                    lblParent_GuardianDetails.Text = CandidateDetails.fname != "" ? CandidateDetails.fname : CandidateDetails.Gname; //CandidateDetails.CourseName;


                }
                else
                { 
                }
                btnBack.Visible = false; ;
                 var CheckDocsAndDecVerified = (from s in context.PuraskarAppDocsVerificationAndDeclarationByInstts
                                     where s.RegnNo == RegistrationNo && s.ExamID == ExamId && s.DocsVerifiedByInstt == true && s.DeclarationByInstt == true
                                                    select new { ID = s.RegnNo, Verify = s.DocsVerifiedByInstt }).Distinct();

                 if (CheckDocsAndDecVerified.Count() > 0)
                 {
                     btnSaveStatus.Visible = false;
                     chkdisclamier.Checked = true;
                     chkdisclamier.Enabled = false;
                     //chkdisclamier.Font.Size = FontUnit.XXLarge;
                     chkRecords.Checked = true;
                     chkRecords.Enabled = false;
                     //chkRecords.Font.Size = FontUnit.XXLarge;
                     //chkRecords.BackColor = Color.Green;
                     btnBack.Visible = false;
                 }
            };

            using (DataTable dt = GetCertificateViewUploadedByCandidate(ExamId, RegistrationNo))
            {
                if (dt.Rows.Count > 0)
                {
                    int i;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        lblIncom.Text = dt.Rows[i]["IncomeCertFile"].ToString();
                        lblIncomCerDate.Text = dt.Rows[i]["IncomeCertDate"].ToString();
                        lblPh.Text = dt.Rows[i]["PHCertFile"].ToString();
                        lblPhCerDate.Text = dt.Rows[i]["PHCertDate"].ToString();
                        lblCaste.Text = dt.Rows[i]["CasteCertFile"].ToString();
                        lblCasteDate.Text = dt.Rows[i]["CasteCertDate"].ToString();
                        if (lblPh.Text == "")
                        {
                            ph.Visible = false;
                        }
                        if (lblCaste.Text == "")
                        {
                            caste.Visible = false;
                        }
                    }
                }

                
            }
            DivDocs.Visible = true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
  

    //for Finalized Moudules by examination wing   
    protected void btnUpdateModules_Click(object sender, EventArgs e)
    {
        string str = string.Empty;
        string strname = string.Empty;
        lblRecord.Text = "";        
        BreadCrumb1.Render();
        Int64 RegistrationNumber = 0, ModuleId = 0, TotalModuleIdMinus = 0, Examid = 0, ExamidForPreviousModules = 0, ModulesCount = 0;        
        string IsPreModuless = "", candStatus1 = string.Empty;
        
        using (EConnectContext context = new EConnectContext())
        {
            PuraskarApplicationForm objpuraskarApplication;
            PuraskarApplicationForm objpuraskarAppStatus;
            OnlineProtsahanExamModules objOnlineProtsahanExamModules;
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                objpuraskarApplication = new PuraskarApplicationForm();
                objOnlineProtsahanExamModules = new OnlineProtsahanExamModules();
        ////foreach (GridViewRow row in gvMain.Rows)
        ////{
        ////    bool isChecked = row.Cells[7].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
        ////    if (isChecked)
        ////    {
        ////        GridView gv = new GridView();
        ////        gv = (GridView)(row.FindControl("gvMain"));
        ////        if (row.RowType == DataControlRowType.DataRow)
        ////        {
        ////            string isVerifiedByExam = isChecked.ToString();
        ////             candStatus1 = Convert.ToString(row.Cells[6].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem); 
        ////            // fetch the datakey values for updation 
        ////             RegistrationNumber = Convert.ToInt64(gvMain.DataKeys[row.RowIndex].Values["RegnNo"].ToString());                    
        ////             ModuleId = Convert.ToInt64(gvMain.DataKeys[row.RowIndex].Values["ModuleID"].ToString());
        ////             Examid = Convert.ToInt64(gvMain.DataKeys[row.RowIndex].Values["Examid"].ToString());
        ////             IsPreModuless = gvMain.DataKeys[row.RowIndex].Values["IsPreModules"].ToString();
        ////             if (IsPreModuless == "NO")
        ////             {
        ////                 ExamidForPreviousModules = Convert.ToInt64(Request.QueryString["Examid"]); 
        ////                 //HValueForPrevModulesExamId.Value = "0";
        ////                 //HValueForPrevModulesExamId.Value = Request.QueryString["Examid"];
        ////             }
        ////             if (IsPreModuless == "YES")
        ////             {
        ////                 Examid = Convert.ToInt64(Request.QueryString["Examid"]);
        ////                 ExamidForPreviousModules = Convert.ToInt64(gvMain.DataKeys[row.RowIndex].Values["Examid"].ToString());
        ////             }
        ////             // deep add on 20 sep 2021
        ////             objpuraskarAppStatus = new PuraskarApplicationForm();
        ////             var AppStatus = (from s in context.PuraskarApplicationForms
        ////                              where s.RegnNo == RegistrationNumber && s.ExamID == Examid && s.verifiedByFinance == true
        ////                                          select new { ID = s.ID, name = s.Name}).Distinct();
        ////             if (AppStatus.Count() > 0)
        ////             {
        ////                 lblRecord.Text = "Candidates status already Processed, Modules can not be verify!";
        ////                 lblRecord.ForeColor = System.Drawing.Color.Red;
        ////                 //Response.Redirect("PuraskarAppModulesVerificationByExam.aspx?msg=" + strMessage + "&RegistrationNo=" + RegistrationNumber + "&Examid=" + Examid);
        ////                 return;
        ////             }
        ////             // deep add end on 20 Sep 2021
        ////             string OnlineRefNum = "";                     
        ////                 objpuraskarApplication = context.PuraskarApplicationForms.Where(a => a.RegnNo == RegistrationNumber && a.ExamID == Examid).FirstOrDefault();
        ////                  OnlineRefNum = objpuraskarApplication.OnlineRefNo;
        ////                 Int64 CandidateId = Convert.ToInt64(objpuraskarApplication.CandidateID);
                    
        ////             if (IsPreModuless == "YES")
        ////             {
        ////                 Examid = ExamidForPreviousModules;                         
        ////             }                     

        ////    // Modules updated by exam section  [NIELIT].[dbo].[OnlineProtsahanExamModules]          
        ////    objOnlineProtsahanExamModules = context.OnlineProtsahanExamModuless.Where(a => a.RegnNo == RegistrationNumber && a.ExamID == Examid && a.ModuleID == ModuleId).FirstOrDefault();
        ////    //objOnlineProtsahanExamModules.isVerifiedByExam = true;
        ////    if (candStatus1 == "YES")
        ////    {
        ////        objOnlineProtsahanExamModules.isVerifiedByExam = true;
        ////        //ModulesCount = ModulesCount + 1;
        ////    }
        ////    if (candStatus1 == "NO")
        ////    {
        ////        objOnlineProtsahanExamModules.isVerifiedByExam = false;
        ////        TotalModuleIdMinus = TotalModuleIdMinus + 1;
        ////    }

        ////    objOnlineProtsahanExamModules.OnlineRefNo = OnlineRefNum;
        ////    objOnlineProtsahanExamModules.enterBy =  Convert.ToInt32(Session["UserID"]);
        ////    objOnlineProtsahanExamModules.enterDate = DateTime.Now;
        ////    context.SaveChanges();                        
        ////                strMessage = "Modules updated for Protsahan Puraskar. Kindly verify the candidate to continue..!";
        ////                ModulesCount = ModulesCount + 1;
        ////            }                 
        ////        }            
        ////}
        // fetch modules count for updation more modules at the same time 
        Int64 previousModulesCount = Convert.ToInt64(objpuraskarApplication.moduleCountProcessed == null ? 0 : objpuraskarApplication.moduleCountProcessed);
        // Moudules count updated on main table i.e.  [NIELIT].[dbo].[OnlinePuraskarApplication]
        objpuraskarApplication.moduleCountProcessed = previousModulesCount + ModulesCount - TotalModuleIdMinus;
        context.SaveChanges();
        Examid = Convert.ToInt64(Request.QueryString["Examid"]); ;
        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("PuraskarApplicationFormVerificationByInstitute.aspx?msg=" + strMessage + "&RegistrationNo=" + RegistrationNumber + "&Examid=" + Examid), true); 
            }            
        }        
    }  
  
    protected void BindDocumentsUploaded( Int64 Examid, Int64 RegistrationNo)
    {
        try
        {
            string ViewFor = Request.QueryString["VF"];
            if (ViewFor == "F")
            {
                using (DataTable dt = GetCertificateViewUploadedByCandidate(Examid, RegistrationNo))
                {
                    if (dt.Rows.Count > 0)
                    {
                        int i;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            lblIncom.Text = dt.Rows[i]["IncomeCertFile"].ToString();
                            lblIncomCerDate.Text = dt.Rows[i]["IncomeCertDate"].ToString();
                            lblPh.Text = dt.Rows[i]["PHCertFile"].ToString();
                            lblPhCerDate.Text = dt.Rows[i]["PHCertDate"].ToString();
                            lblCaste.Text = dt.Rows[i]["CasteCertFile"].ToString();
                            lblCasteDate.Text = dt.Rows[i]["CasteCertDate"].ToString();
                            if (lblIncom.Text == "")
                            {
                                Income.Visible = false;
                            }
                            if (lblPh.Text == "")
                            {
                                ph.Visible = false;
                            }
                            if (lblCaste.Text == "")
                            {
                                caste.Visible = false;
                            }
                        }
                    }
                }
                DivDocs.Visible = true;
            }
            else
            {
                DivDocs.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetCertificateViewUploadedByCandidate(Int64 ExamID, Int64 regno)
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {           
            using (SqlCommand cmd = new SqlCommand("select  IncomeCertFile,IncomeCertNo,IncomeCertDate,IncomeCertUpload, CasteCertFile,CasteCertNo,CasteCertDate,CasteCertUploadedOn ,PHCertFile,PHCertNo,PHCertDate,PHCertUpload    FROM [NIELIT].[dbo].[OnlinePuraskarApplication] where RegnNo=  @Regno  and ExamID=@pExamID", con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Regno", regno);
                cmd.Parameters.AddWithValue("@pExamID", ExamID);                
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
    protected void btnIncom_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 ExamId = Convert.ToInt64(Request.QueryString["Examid"]);
            Int64 RegistrationNo = Convert.ToInt64(Request.QueryString["RegistrationNo"]);        
            byte[] bytes;
            string fileName, IncomeCertNo;
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select  IncomeCertFile,IncomeCertNo,IncomeCertDate,IncomeCertUpload   FROM [NIELIT].[dbo].[OnlinePuraskarApplication] where RegnNo=  @Regno  and ExamID=@pExamID";
                    cmd.Parameters.AddWithValue("@Regno", RegistrationNo);
                    cmd.Parameters.AddWithValue("@pExamID", ExamId);  
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read(); 
                        bytes = (byte[])sdr["IncomeCertUpload"];
                        IncomeCertNo = sdr["IncomeCertNo"].ToString();
                        fileName = sdr["IncomeCertFile"].ToString();
                    }
                    con.Close();
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
           
        }
        catch (Exception ex)
        {
            string ab = ex.Message.ToString();
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnph_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 ExamId = Convert.ToInt64(Request.QueryString["Examid"]);
            Int64 RegistrationNo = Convert.ToInt64(Request.QueryString["RegistrationNo"]);          
            byte[] bytes;
            string fileName, IncomeCertNo;
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select  PHCertNo, PHCertDate, PHCertUpload, PHCertFile, PHCertUploadedOn  FROM [NIELIT].[dbo].[OnlinePuraskarApplication] where RegnNo=  @Regno  and ExamID=@pExamID";
                    cmd.Parameters.AddWithValue("@Regno", RegistrationNo);
                    cmd.Parameters.AddWithValue("@pExamID", ExamId);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["PHCertUpload"];
                        IncomeCertNo = sdr["PHCertNo"].ToString();
                        fileName = sdr["PHCertFile"].ToString();
                    }
                    con.Close();
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnCaste_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 ExamId = Convert.ToInt64(Request.QueryString["Examid"]);
            Int64 RegistrationNo = Convert.ToInt64(Request.QueryString["RegistrationNo"]);           
            byte[] bytes;
            string fileName, IncomeCertNo;
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select  CasteCertNo, CasteCertDate, CasteCertUpload, CasteCertFile, CasteCertUploadedOn   FROM [NIELIT].[dbo].[OnlinePuraskarApplication] where RegnNo=  @Regno  and ExamID=@pExamID";
                    cmd.Parameters.AddWithValue("@Regno", RegistrationNo);
                    cmd.Parameters.AddWithValue("@pExamID", ExamId);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["CasteCertUpload"];
                        IncomeCertNo = sdr["CasteCertNo"].ToString();
                        fileName = sdr["CasteCertFile"].ToString();
                    }
                    con.Close();
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void btnSaveStatus_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 ExamId = 0;
            lblError.Visible = false;
            string CheckedValue = chkRecords.Checked ? "Y" : "N"; //chkdisclamier
            string CheckedValueDeclaration = chkdisclamier.Checked ? "Y" : "N";
            if (CheckedValue == "N")
            {
                lblError.Text = "Please checked the Verify Documents!";
                lblError.Visible = true;
                lblError.ForeColor = System.Drawing.Color.Red;
                return;
            }
            if (CheckedValueDeclaration == "N")
            {
                lblError.Text = "Please checked the Declaration!";
                lblError.Visible = true;
                lblError.ForeColor = System.Drawing.Color.Red;
                return;
            }
             ExamId = Convert.ToInt64(Request.QueryString["Examid"]);
            Int64 RegistrationNo = Convert.ToInt64(Request.QueryString["RegistrationNo"]);
            using (EConnectContext context = new EConnectContext())
            {
                var RecordsVerified = (from s in context.PuraskarApplicationForms
                                       where s.RegnNo == RegistrationNo && s.ExamID == ExamId && s.verifiedByInstt == true && s.finalSubmit == true
                                    select new { ID = s.ID, WithHeldReasonExist = s.withHeldReason }).Distinct();

                if (RecordsVerified.Count() > 0)
                    {
                        strMessage = "Already verified.";
                        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("PuraskarAppDocsVerAndDecByInstt.aspx?msg=" + strMessage + "&RegistrationNo=" + RegistrationNo.ToString() + "&Examid=" + ExamId.ToString()), true);
 
                        return;
                    }
               
                PuraskarAppDocsVerificationAndDeclarationByInstt objPuraskarAppDocsVerificationAndDeclarationByInstt;
                objPuraskarAppDocsVerificationAndDeclarationByInstt = new PuraskarAppDocsVerificationAndDeclarationByInstt();
                //objPuraskarAppDocsVerificationAndDeclarationByInstt = context.PuraskarAppDocsVerificationAndDeclarationByInstts.Where(a => a.RegnNo == RegistrationNo && a.ExamID == ExamId).FirstOrDefault();
                objPuraskarAppDocsVerificationAndDeclarationByInstt.RegnNo = RegistrationNo;
                objPuraskarAppDocsVerificationAndDeclarationByInstt.ExamID = ExamId;
                objPuraskarAppDocsVerificationAndDeclarationByInstt.DeclarationByInstt = true;
                objPuraskarAppDocsVerificationAndDeclarationByInstt.DeclarationByInsttUser = loginUserNo;
                objPuraskarAppDocsVerificationAndDeclarationByInstt.DeclarationByInsttOn = DateTime.Now;
                objPuraskarAppDocsVerificationAndDeclarationByInstt.DocsVerifiedByInstt = true;
                objPuraskarAppDocsVerificationAndDeclarationByInstt.DocsVerifiedByInsttUser = loginUserNo;
                objPuraskarAppDocsVerificationAndDeclarationByInstt.DocsVerifiedByInsttOn = DateTime.Now;
                context.PuraskarAppDocsVerificationAndDeclarationByInstts.Add(objPuraskarAppDocsVerificationAndDeclarationByInstt);
                context.SaveChanges();
                strMessage = "Documents verified successfully.";

            };
           // Response.Redirect("PuraskarAppDocsVerAndDecByInstt.aspx?msg=" + strMessage, true);
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("PuraskarAppDocsVerAndDecByInstt.aspx?msg=" + strMessage + "&RegistrationNo=" + RegistrationNo.ToString() + "&Examid=" + ExamId.ToString()), true);
 
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("PuraskarApplicationFormVerificationByInstitute.aspx", true);
    }
      //datefrom = Convert.ToDateTime(Decrypt(HttpUtility.UrlDecode(Request.QueryString["DateFrom"])));
      //      dateto = Convert.ToDateTime(Decrypt(HttpUtility.UrlDecode(Request.QueryString["DateTo"])));
     //string DateFrom = HttpUtility.UrlEncode(Encrypt(txttDateFrom.Text.Trim()));
     //           string DateTo = HttpUtility.UrlEncode(Encrypt(txtDateto.Text.Trim()));
     //           string projectID = HttpUtility.UrlEncode(Encrypt(ddlProjectName.SelectedItem.Value));
     //           string projectIDD = ddlProjectName.SelectedItem.Value;
                
     //           if (projectIDD == "0")
     //           {
     //               Response.Redirect(string.Format("../HO/Rpt/StatisticsReport.aspx?DateFrom={0}&DateTo={1}", DateFrom, DateTo));
     //           }
     //           else
     //           {
     //               Response.Redirect(string.Format("../HO/Rpt/StatisticsReportProjectWise.aspx?DateFrom={0}&DateTo={1}&projectID={2}", DateFrom, DateTo, projectID));
     //           }
    ////private string Decrypt(string cipherText)
    ////{
    ////    string EncryptionKey = "MAKV2SPBNI99212";
    ////    cipherText = cipherText.Replace(" ", "+");
    ////    byte[] cipherBytes = Convert.FromBase64String(cipherText);
    ////    using (Aes encryptor = Aes.Create())
    ////    {
    ////        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
    ////        encryptor.Key = pdb.GetBytes(32);
    ////        encryptor.IV = pdb.GetBytes(16);
    ////        using (MemoryStream ms = new MemoryStream())
    ////        {
    ////            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
    ////            {
    ////                cs.Write(cipherBytes, 0, cipherBytes.Length);
    ////                cs.Close();
    ////            }
    ////            cipherText = Encoding.Unicode.GetString(ms.ToArray());
    ////        }
    ////    }
    ////    return cipherText;
    ////}

    //private string Encrypt(string clearText)
    //{
    //    string EncryptionKey = "MAKV2SPBNI99212";
    //    byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
    //    using (Aes encryptor = Aes.Create())
    //    {
    //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
    //        encryptor.Key = pdb.GetBytes(32);
    //        encryptor.IV = pdb.GetBytes(16);
    //        using (MemoryStream ms = new MemoryStream())
    //        {
    //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
    //            {
    //                cs.Write(clearBytes, 0, clearBytes.Length);
    //                cs.Close();
    //            }
    //            clearText = Convert.ToBase64String(ms.ToArray());
    //        }
    //    }
    //    return clearText;
    //}
}