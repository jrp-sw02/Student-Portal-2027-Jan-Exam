using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.Objects;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlClient;
using System.Data.SqlClient;
using System.Drawing;
using System.IdentityModel;
using System.IdentityModel.Metadata;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;
using System.Web.Security;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class Admin_PuraskarAppModulesVerificationByExam : BasePage  
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
         /*   if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/PuraskarApplicationFormVerificationByExam.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }   */
            ArrayList CheckBoxArray;
            if (ViewState["CheckBoxArray"] != null)
            {
                CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
            }
            else
            {
                CheckBoxArray = new ArrayList();
            }
            
            if (!Page.IsPostBack)
            {
                BindGridView();
                int CheckBoxIndex;
                bool CheckAllWasChecked = false;
                CheckBox chkAll = (CheckBox)gvMain.HeaderRow.Cells[7].FindControl("chkAll");
                string checkAllIndex = "chkAll-" + gvMain.PageIndex;
                if (chkAll.Checked)
                {
                    if (CheckBoxArray.IndexOf(checkAllIndex) == -1)
                    {
                        CheckBoxArray.Add(checkAllIndex);
                    }
                }
                else
                {
                    if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
                    {
                        CheckBoxArray.Remove(checkAllIndex);
                        CheckAllWasChecked = true;
                    }
                }
                for (int i = 0; i < gvMain.Rows.Count; i++)
                {
                    if (gvMain.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chk = (CheckBox)gvMain.Rows[i].Cells[7].FindControl("CheckBox1");                       
                        CheckBoxIndex = gvMain.PageSize * PagingBar1.CurrentPageIndex + (i + 1);
                        if (chk.Checked)
                        {
                            if (CheckBoxArray.IndexOf(CheckBoxIndex) == -1 && !CheckAllWasChecked)
                            {
                                CheckBoxArray.Add(CheckBoxIndex);                               
                            }
                        }
                        else
                        {                            
                            if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1 || CheckAllWasChecked)
                            {
                                CheckBoxArray.Remove(CheckBoxIndex);
                            }
                        }
                    }
                }                             
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";                  

                   /// BindGridView();
                    if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Online Puraskar Application", "Admin/PuraskarApplicationFormVerificationByExam.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Online Puraskar Application", "Admin/PuraskarApplicationFormVerificationByExam.aspx", ""));
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

    // added by amit  start
    protected void PaidBy_Status(Int64 RegistrationNo, Int64 ExamId)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            using (SqlCommand command = new SqlCommand("usp_Protsahan_payment_mode", con))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@exam_id", SqlDbType.Int).Value = ExamId;
                command.Parameters.Add("@reg_no", SqlDbType.BigInt).Value = RegistrationNo;
                command.Parameters.Add("@msg_payment_source_found", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                command.Parameters.Add("@msg", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                con.Open();
                command.ExecuteNonQuery();

                string paidByParam = Convert.ToString(command.Parameters["@msg_payment_source_found"].Value);
                string paidBy = String.IsNullOrEmpty(paidByParam) ? "Not Known": paidByParam.ToString() ;

                lblpaidby.Visible = true;
                lblpaidby.Text = "Paid By : " + paidBy;
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    // added by amit end

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
    protected void BindGridView()
    {
        try
        {            
            Int64 ExamId = Convert.ToInt64(Request.QueryString["Examid"]);
            Int64 RegistrationNo = Convert.ToInt64(Request.QueryString["RegistrationNo"]);

            // added by amit start
            PaidBy_Status(RegistrationNo, ExamId);
            // added by amit end

            string ViewFor = Request.QueryString["VF"];
            using (DataTable dt = FillNestedGridViewRegNoAndExamiId(ExamId, RegistrationNo))
            {
                if (dt.Rows.Count > 0)
                {
                    var ModulesData = (from p in dt.AsEnumerable()
                                       select new
                                       {
                                           //ID = p.Field<Int64>("ID"), 
                                           slno = p.Field<Int64>("slno"),
                                           ExamCycle = p.Field<string>("ExamCycle"),
                                           Examid = p.Field<Int64>("ExamID"),
                                           RegnNo = p.Field<Int64>("RegnNo"),
                                           Candidate_ID = p.Field<Int64>("Candidate_ID"),
                                           ModuleID = p.Field<Int64>("ModuleID"),
                                           modulesApp = p.Field<string>("modulesApp"),
                                           modulesPass = p.Field<string>("modulesPass"),
                                           isProcessed = p.Field<string>("isProcessed"),
                                           isVerifiedByExam=p.Field<string>("isVerifiedByExam"),
                                           IsPreModules = p.Field<string>("IsPreModules"),

                                       });

                    PagingBar1.Bind(ModulesData, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    lblError.Visible = false;                   
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "No record found.";
                        lblError.Visible = true;
                    }
                }
                else
                {
                    lblError.Text = "No record found.";
                    lblError.Visible = true;
                }
            }
            BindDocumentsUploaded( ExamId, RegistrationNo);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
   
    protected void OnPageIndexChanging(object sender, DetailsViewPageEventArgs e)
    {
        //DetailsView1.PageIndex = e.NewPageIndex;
        //this.BindGridViewRecord(8,300335);
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
        foreach (GridViewRow row in gvMain.Rows)
        {
            bool isChecked = row.Cells[7].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
            if (isChecked)
            {
                GridView gv = new GridView();
                gv = (GridView)(row.FindControl("gvMain"));
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string isVerifiedByExam = isChecked.ToString();
                     candStatus1 = Convert.ToString(row.Cells[6].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem); 
                    // fetch the datakey values for updation 
                     RegistrationNumber = Convert.ToInt64(gvMain.DataKeys[row.RowIndex].Values["RegnNo"].ToString());                    
                     ModuleId = Convert.ToInt64(gvMain.DataKeys[row.RowIndex].Values["ModuleID"].ToString());
                     Examid = Convert.ToInt64(gvMain.DataKeys[row.RowIndex].Values["Examid"].ToString());
                     IsPreModuless = gvMain.DataKeys[row.RowIndex].Values["IsPreModules"].ToString();
                     if (IsPreModuless == "NO")
                     {
                         ExamidForPreviousModules = Convert.ToInt64(Request.QueryString["Examid"]); 
                         //HValueForPrevModulesExamId.Value = "0";
                         //HValueForPrevModulesExamId.Value = Request.QueryString["Examid"];
                     }
                     if (IsPreModuless == "YES")
                     {
                         Examid = Convert.ToInt64(Request.QueryString["Examid"]);
                         ExamidForPreviousModules = Convert.ToInt64(gvMain.DataKeys[row.RowIndex].Values["Examid"].ToString());
                     }
                     // deep add on 20 sep 2021
                     objpuraskarAppStatus = new PuraskarApplicationForm();
                     var AppStatus = (from s in context.PuraskarApplicationForms
                                      where s.RegnNo == RegistrationNumber && s.ExamID == Examid && s.verifiedByFinance == true
                                                  select new { ID = s.ID, name = s.Name}).Distinct();
                     if (AppStatus.Count() > 0)
                     {
                         lblRecord.Text = "Candidates status already Processed, Modules can not be verify!";
                         lblRecord.ForeColor = System.Drawing.Color.Red;
                         //Response.Redirect("PuraskarAppModulesVerificationByExam.aspx?msg=" + strMessage + "&RegistrationNo=" + RegistrationNumber + "&Examid=" + Examid);
                         return;
                     }
                     // deep add end on 20 Sep 2021
                     string OnlineRefNum = "";                     
                         objpuraskarApplication = context.PuraskarApplicationForms.Where(a => a.RegnNo == RegistrationNumber && a.ExamID == Examid).FirstOrDefault();
                          OnlineRefNum = objpuraskarApplication.OnlineRefNo;
                         Int64 CandidateId = Convert.ToInt64(objpuraskarApplication.CandidateID);
                    
                     if (IsPreModuless == "YES")
                     {
                         Examid = ExamidForPreviousModules;                         
                     }                     

            // Modules updated by exam section  [NIELIT].[dbo].[OnlineProtsahanExamModules]          
            objOnlineProtsahanExamModules = context.OnlineProtsahanExamModuless.Where(a => a.RegnNo == RegistrationNumber && a.ExamID == Examid && a.ModuleID == ModuleId).FirstOrDefault();
            //objOnlineProtsahanExamModules.isVerifiedByExam = true;
            if (candStatus1 == "YES")
            {
                objOnlineProtsahanExamModules.isVerifiedByExam = true;
                //ModulesCount = ModulesCount + 1;
            }
            if (candStatus1 == "NO")
            {
                objOnlineProtsahanExamModules.isVerifiedByExam = false;
                TotalModuleIdMinus = TotalModuleIdMinus + 1;
            }

            objOnlineProtsahanExamModules.OnlineRefNo = OnlineRefNum;
            objOnlineProtsahanExamModules.enterBy =  Convert.ToInt32(Session["UserID"]);
            objOnlineProtsahanExamModules.enterDate = DateTime.Now;
            context.SaveChanges();                        
                        strMessage = "Modules updated for Protsahan Puraskar. Kindly verify the candidate to continue..!";
                        ModulesCount = ModulesCount + 1;
                    }                 
                }            
        }
        // fetch modules count for updation more modules at the same time 
        Int64 previousModulesCount = Convert.ToInt64(objpuraskarApplication.moduleCountProcessed == null ? 0 : objpuraskarApplication.moduleCountProcessed);
        // Moudules count updated on main table i.e.  [NIELIT].[dbo].[OnlinePuraskarApplication]
        objpuraskarApplication.moduleCountProcessed = previousModulesCount + ModulesCount - TotalModuleIdMinus;
        context.SaveChanges();
        Examid = Convert.ToInt64(Request.QueryString["Examid"]); ;
        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("PuraskarAppModulesVerificationByExam.aspx?msg=" + strMessage + "&RegistrationNo=" + RegistrationNumber + "&Examid=" + Examid), true); 
            }            
        }        
    }
   
     protected void OnCheckedChanged(object sender, EventArgs e)
    {
        bool isUpdateVisible = false;
        lblRecord.Text = string.Empty;
       lblMessage.Text = string.Empty;
        //Loop through all rows in GridView
        foreach (GridViewRow row in gvMain.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[7].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                    row.RowState = DataControlRowState.Edit;               
                for (int i = 6; i < 7; i++)
                {
                    if (row.Cells[i].Controls.OfType<Label>().ToList().Count > 0)
                    {
                        row.Cells[i].Controls.OfType<DropDownList>().FirstOrDefault().Visible = isChecked;
                        row.Cells[i].Controls.OfType<Label>().FirstOrDefault().Visible = !isChecked;
                    }                    
                    //Label AlreadyPaid = (Label)gvr.FindControl("Label1").ToString();
                    if (isChecked && !isUpdateVisible)  
                    {
                        isUpdateVisible = true;
                       // btnCancel.Visible = true;
                    }
                   
                }              
            }
        }
        btnUpdateModules.Visible = isUpdateVisible;
        lblRecord.Text = "";
    }  
   

    protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }


    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        
        GridViewRow row = gvMain.SelectedRow;
        //Find label id from gridview cell data  ds.Tables[0].Rows[i]["Name"].ToString();
        Label lblRegno = row.FindControl("lblRegno") as Label;
        Label lblExamid = row.FindControl("lblExamid") as Label;
        Int64 Examid = 0;
        Examid = Convert.ToInt64(lblExamid.Text);
        Int64 RegistrationNo = Convert.ToInt64(lblRegno.Text);

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
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            string ViewFor = Request.QueryString["VF"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                string href = hl.NavigateUrl;
                if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    href += "&ID=" + Request.QueryString["ID"].ToString();
                }
                CheckBox CheckBox1 = (e.Row.FindControl("CheckBox1") as CheckBox);
                Label lbl1 = (e.Row.FindControl("Label1") as Label); 
               // string AlreadyPaidSelectData = e.Row.Cells[5].Text;
                if (lbl1.Text == "YES")//||  "Verified")                {
                {
                    //CheckBox1.Checked = true;
                    CheckBox1.Enabled = false;
                    CheckBox1.Visible = false;

                    //CheckBox ChkBoxHeader = (CheckBox)gvMain.HeaderRow.FindControl("chkAll");
                    //ChkBoxHeader.Checked = true;
                    //ChkBoxHeader.Enabled = false;
                }
                if (ViewFor == "E")
                {
                    //gvMain.Columns[9].Visible = true; 
                    gvMain.Columns[9].Visible = false;
                    gvMain.Columns[7].Visible = false;
                }  
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
   
     protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            //gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            //BindGridView();

            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            int NewIndex = NewPageIndex;
            BindGridView();

            if (ViewState["CheckBoxArray"] != null)
            {
                ArrayList CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
                string checkAllIndex = "chkAll-" + gvMain.PageIndex;

                if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
                {
                    CheckBox chkAll = (CheckBox)gvMain.HeaderRow.Cells[0].FindControl("chkAll");
                    chkAll.Checked = true;
                }
                for (int i = 0; i < gvMain.Rows.Count; i++)
                {

                    if (gvMain.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
                        {
                            CheckBox chk = (CheckBox)gvMain.Rows[i].Cells[0].FindControl("CheckBox1");
                            chk.Checked = true;
                            gvMain.Rows[i].Attributes.Add("style", "background-color:aqua");
                        }
                        else
                        {

                            int CheckBoxIndex = gvMain.PageSize * (NewIndex) + (i + 1);
                            Label lblID = (Label)gvMain.Rows[i].Cells[0].FindControl("lblID");
                            Int64 ID = Convert.ToInt64(lblID.Text);
                            //if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1)
                            if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1)
                            {
                                CheckBox chk = (CheckBox)gvMain.Rows[i].Cells[0].FindControl("CheckBox1");
                                chk.Checked = true;
                                gvMain.Rows[i].Attributes.Add("style", "background-color:aqua");
                            }
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