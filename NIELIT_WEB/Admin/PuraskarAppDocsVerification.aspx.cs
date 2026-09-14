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
using System.Data.SqlClient;
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
using System.Web.Security;
using System.Data.OleDb;

public partial class Admin_PuraskarAppDocsVerification : BasePage  
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/PuraskarDocsUploadedFormVerification.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }   
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
                    if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Online Puraskar Placement Application", "Admin/PuraskarDocsUploadedFormVerification.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Online Puraskar Placement Application", "Admin/PuraskarDocsUploadedFormVerification.aspx", ""));
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
   
    protected void ddlVerifiedsAppointmentLetter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            DropDownList duty = (DropDownList)gvr.FindControl("ddlVerifiedsAppointmentLetter");
            Int32 candStatus = Convert.ToInt32(duty.SelectedItem.Value);
            Label VerifyStatus = (Label)gvr.FindControl("Label2");
        }
        catch (Exception ex)
        {
            throw ex;
        } 
    }
    protected void ddlVerifiedsSalarySlipFile_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            DropDownList duty = (DropDownList)gvr.FindControl("ddlVerifiedsSalarySlipFile");
            Int32 candStatus = Convert.ToInt32(duty.SelectedItem.Value);
            Label VerifyStatus = (Label)gvr.FindControl("Label2");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlVerifiedsBankStatementFile_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            DropDownList duty = (DropDownList)gvr.FindControl("ddlVerifiedsBankStatementFile");
            Int32 candStatus = Convert.ToInt32(duty.SelectedItem.Value);
            Label VerifyStatus = (Label)gvr.FindControl("Label2");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
        
   

    public DataTable FillNestedGridViewRegNoAndExamiId(Int64 ExamID, Int64 regno)
    {

        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetUpdateNestedGridViewRegNoAndExamiIdDocs", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;             
                cmd.Parameters.Add(new SqlParameter("@Regno", SqlDbType.BigInt));
                cmd.Parameters["@Regno"].Value = regno;
                cmd.Parameters.Add(new SqlParameter("@pExamID", SqlDbType.BigInt));

                cmd.Parameters["@pExamID"].Value = ExamID;
                cmd.Parameters.Add(new SqlParameter("@Action", SqlDbType.Int));
                cmd.Parameters["@Action"].Value = 21;
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

            using (DataTable dt = FillNestedGridViewRegNoAndExamiId(ExamId, RegistrationNo))
            {
                if (dt.Rows.Count > 0)
                {
                    var DocsData = (from p in dt.AsEnumerable()
                                       select new
                                       {                                           
                                           slno = p.Field<Int64>("slno"),                                          
                                           Examid = p.Field<Int64>("ExamID"),
                                           RegnNo = p.Field<Int64>("Regno"),
                                           Name = p.Field<string>("Name"),
                                           FatherName = p.Field<string>("FatherName"),
                                           AppointmentLetterFileVerified = p.Field<string>("AppointmentLetterFileVerified"),
                                           SalarySlipFileVerified = p.Field<string>("SalarySlipFileVerified"),
                                           BankStatementFileVerified = p.Field<string>("BankStatementFileVerified"),
                                       }).ToList();                  
                    
                    PagingBar1.Bind(DocsData, ref gvMain);
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
    }   

    //for verify Documents  
    protected void btnVerifyDocuments_Click(object sender, EventArgs e)
    {
        string str = string.Empty;
        string strname = string.Empty;
        lblRecord.Text = "";        
        BreadCrumb1.Render();
        Int64 RegistrationNumber = 0,Examid = 0;        
        string  candStatus1 = string.Empty;          
                
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
                     Examid = Convert.ToInt64(gvMain.DataKeys[row.RowIndex].Values["Examid"].ToString());
                    
                     int AppStatus = Convert.ToInt32(FetchData(RegistrationNumber, 0, Examid, 25));
                    
                    if (AppStatus > 0)
                     {
                         lblRecord.Text = "Candidates status already Processed, Documents can not be verify!";
                         lblRecord.ForeColor = System.Drawing.Color.Red;                        
                         return;
                     }                    

                    string AppointmentLetter = Convert.ToString(row.Cells[4].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem);
                    string SalarySlip = Convert.ToString(row.Cells[5].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem);
                    string BankStatment = Convert.ToString(row.Cells[6].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem);

                    string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                    SqlConnection Conn = new SqlConnection(constr);
                    SqlCommand cmd = new SqlCommand("GetUpdateNestedGridViewRegNoAndExamiIdDocs", Conn);
                    Conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Regno", RegistrationNumber);
                    cmd.Parameters.AddWithValue("@pExamID", Examid);                    
                    cmd.Parameters.AddWithValue("@Action", 22);
                    if (AppointmentLetter == "Verified")
                    {
                        cmd.Parameters.AddWithValue("@AppointmentLetterFileVerified", 1);  
                    }
                    if (AppointmentLetter == "NO")
                    {
                        cmd.Parameters.AddWithValue("@AppointmentLetterFileVerified", 0);
                    }
                    if (SalarySlip == "Verified")
                    {
                        cmd.Parameters.AddWithValue("@SalarySlipFileVerified", 1);
                    }
                    if (SalarySlip == "NO")
                    {
                        cmd.Parameters.AddWithValue("@SalarySlipFileVerified", 0);
                    }
                    if (BankStatment == "Verified")
                    {
                        cmd.Parameters.AddWithValue("@BankStatementFileVerified", 1);
                    }
                    if (BankStatment == "NO")
                    {
                        cmd.Parameters.AddWithValue("@BankStatementFileVerified", 0);
                    }
                   
                    cmd.Parameters.AddWithValue("@VerifiedBy", Convert.ToInt32(Session["UserID"]));
                    cmd.ExecuteNonQuery();
                    Conn.Close();

                    int AllDocsVerify = Convert.ToInt32(FetchData(RegistrationNumber, 0, Examid, 26));
                    if (AllDocsVerify==0)
                        lblMessage.Text = "Documents updated for Protsahan Puraskar Placement. Kindly verify the candidate to continue..!";
                    else
                        lblMessage.Text = "Documents updated for Protsahan Puraskar Placement. Kindly verify the remaining documents..!";
                        BindGridView();
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        btnVerifyDocuments.Visible = false;
                    }                 
                }            
        }       
             
    }
    public string FetchData(Int64 RegnNo, int courseid, Int64 ExamId, int Action)
    {
        string DataValue = "0";
        SqlConnection sqlCon = null;
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

        using (sqlCon = new SqlConnection(constr))
        {
            sqlCon.Open();
            SqlCommand Cmd = new SqlCommand("GetRecordForPuraskarDocsUpload", sqlCon);
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.AddWithValue("@RegnNo", RegnNo);
            Cmd.Parameters.AddWithValue("@ExamId", ExamId);
            Cmd.Parameters.AddWithValue("@courseid", courseid);
            Cmd.Parameters.AddWithValue("@Action", Action);
            string DataValues = Cmd.ExecuteScalar().ToString();
            if (DataValues != null && DataValues != "")
            {
                DataValue = DataValues.ToString();
            }
            sqlCon.Close();
        }
        return DataValue;
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
                for (int i = 4; i < 7; i++)
                {
                    if (row.Cells[i].Controls.OfType<Label>().ToList().Count > 0)
                    {
                        row.Cells[i].Controls.OfType<DropDownList>().FirstOrDefault().Visible = isChecked;
                        row.Cells[i].Controls.OfType<Label>().FirstOrDefault().Visible = !isChecked;
                    } 
                    GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
                    DropDownList AppLetter = (DropDownList)gvr.FindControl("ddlVerifiedsAppointmentLetter");
                    DropDownList SalarySlip = (DropDownList)gvr.FindControl("ddlVerifiedsSalarySlipFile");
                    DropDownList BankStatement = (DropDownList)gvr.FindControl("ddlVerifiedsBankStatementFile");
                    Label BankStatementStatus = (Label)gvr.FindControl("Label5");
                    Label SalarySlipStatus = (Label)gvr.FindControl("Label4");
                    Label AppLetterStatus = (Label)gvr.FindControl("Label1");

                    if (AppLetterStatus.Text == "Verified")
                    {
                        AppLetter.SelectedValue = "1";
                        AppLetter.Enabled = false;
                    }
                    if (SalarySlipStatus.Text == "Verified")
                    {
                        SalarySlip.SelectedValue = "1";
                        SalarySlip.Enabled = false;
                    }
                    if (BankStatementStatus.Text == "Verified")
                    {
                        BankStatement.SelectedValue = "1";
                        BankStatement.Enabled = false;
                    }

                    if (isChecked && !isUpdateVisible)  
                    {
                        isUpdateVisible = true;
                       // btnCancel.Visible = true;
                    }                   
                }              
            }
        }
        btnVerifyDocuments.Visible = isUpdateVisible;
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
              
            using (DataTable dt = GetCertificateViewUploadedByCandidate(Examid, RegistrationNo))
            {
                if (dt.Rows.Count > 0)
                {
                    int i;
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        lblAppointmentLetter.Text = dt.Rows[i]["AppointmentLetterFile"].ToString();
                        lblAppointmentLetterDate.Text = dt.Rows[i]["AppointmentLetterUploadDate"].ToString();
                        lblSalarySlip.Text = dt.Rows[i]["SalarySlipFile"].ToString();
                        lblSalarySlipDate.Text = dt.Rows[i]["SalarySlipUploadDate"].ToString();
                        lblBankStatement.Text = dt.Rows[i]["BankStatementFile"].ToString();
                        lblBankStatementDate.Text = dt.Rows[i]["BankStatementUploadDate"].ToString();                       
                    }
                }
            }
            DivDocs.Visible = true;
       
    }
    protected void BindDocumentsUploaded( Int64 Examid, Int64 RegistrationNo)
    {
        try
        {          
                using (DataTable dt = GetCertificateViewUploadedByCandidate(Examid, RegistrationNo))
                {
                    if (dt.Rows.Count > 0)
                    {
                        int i;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            lblAppointmentLetter.Text = dt.Rows[i]["AppointmentLetterFile"].ToString();
                            lblAppointmentLetterDate.Text = dt.Rows[i]["AppointmentLetterUploadDate"].ToString();
                            lblSalarySlip.Text = dt.Rows[i]["SalarySlipFile"].ToString();
                            lblSalarySlipDate.Text = dt.Rows[i]["SalarySlipUploadDate"].ToString();
                            lblBankStatement.Text = dt.Rows[i]["BankStatementFile"].ToString();
                            lblBankStatementDate.Text = dt.Rows[i]["BankStatementUploadDate"].ToString();
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
    public DataTable GetCertificateViewUploadedByCandidate(Int64 ExamID, Int64 regno)
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("select  AppointmentLetterFile,AppointmentLetterUpload,AppointmentLetterUploadDate,SalarySlipFile, SalarySlipUpload,SalarySlipUploadDate,BankStatementFile,BankStatementUpload,BankStatementUploadDate from [NIELIT].[dbo].[ProtsahanDocsUploadDetails] where RegnNo=  @Regno  and ExamID=@pExamID", con))
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
    protected void btnAppointmentLetter_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 ExamId = Convert.ToInt64(Request.QueryString["Examid"]);
            Int64 RegistrationNo = Convert.ToInt64(Request.QueryString["RegistrationNo"]);        
            byte[] bytes;
            string fileName;
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select  AppointmentLetterFile,AppointmentLetterUpload,AppointmentLetterUploadDate from [NIELIT].[dbo].[ProtsahanDocsUploadDetails]  where RegnNo=  @Regno  and ExamID=@pExamID";
                    cmd.Parameters.AddWithValue("@Regno", RegistrationNo);
                    cmd.Parameters.AddWithValue("@pExamID", ExamId);  
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["AppointmentLetterUpload"];                       
                        fileName = sdr["AppointmentLetterFile"].ToString();
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
    protected void btnSalarySlip_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 ExamId = Convert.ToInt64(Request.QueryString["Examid"]);
            Int64 RegistrationNo = Convert.ToInt64(Request.QueryString["RegistrationNo"]);          
            byte[] bytes;
            string fileName;
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select  SalarySlipFile, SalarySlipUpload,SalarySlipUploadDate from [NIELIT].[dbo].[ProtsahanDocsUploadDetails] where RegnNo=  @Regno  and ExamID=@pExamID";
                    cmd.Parameters.AddWithValue("@Regno", RegistrationNo);
                    cmd.Parameters.AddWithValue("@pExamID", ExamId);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["SalarySlipUpload"];                        
                        fileName = sdr["SalarySlipFile"].ToString();
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
    protected void btnBankStatement_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 ExamId = Convert.ToInt64(Request.QueryString["Examid"]);
            Int64 RegistrationNo = Convert.ToInt64(Request.QueryString["RegistrationNo"]);           
            byte[] bytes;
            string fileName;
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select  BankStatementFile,BankStatementUpload,BankStatementUploadDate from [NIELIT].[dbo].[ProtsahanDocsUploadDetails] where RegnNo=  @Regno  and ExamID=@pExamID";
                    cmd.Parameters.AddWithValue("@Regno", RegistrationNo);
                    cmd.Parameters.AddWithValue("@pExamID", ExamId);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["BankStatementUpload"];                        
                        fileName = sdr["BankStatementFile"].ToString();
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
                Label lbl2 = (e.Row.FindControl("Label4") as Label);
                Label lbl3 = (e.Row.FindControl("Label5") as Label);
              
                if (lbl1.Text == "Verified" && lbl2.Text == "Verified" && lbl3.Text == "Verified")              
                {                    
                    CheckBox1.Enabled = false;
                    CheckBox1.Visible = false;
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