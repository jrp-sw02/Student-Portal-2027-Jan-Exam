using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System;
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


public partial class Admin_PuraskarApplicationFormVerificationByExam : BasePage
{
    NIELITMISContext  context;
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 Examid = 0;
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {                                 
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    ExamName();                  
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
      
    protected void ddlVerifieds_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            DropDownList duty = (DropDownList)gvr.FindControl("ddlVerifieds");
            Int32 candStatus =Convert.ToInt32( duty.SelectedItem.Value);
            TextBox VerifyStatus = (TextBox)gvr.FindControl("txtexamRejectionReason");
            if (candStatus == 0)
            {               
                VerifyStatus.Visible = true;
                VerifyStatus.Enabled = true;
            }
            else
            {
                VerifyStatus.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
   
    public void ExamName()
    {
        ddlflExam.Items.Clear();
        System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
        using (System.Data.DataTable dt = GetDataForPurskarApplicationCommitteeReport(1))
        {
            if (dt.Rows.Count > 0)
            {
                int k;
                for (k = 0; k < 1; k++)
                {
                    ddlflExam.DataSource = dt;
                    ddlflExam.DataTextField = "ExamName";
                    ddlflExam.DataValueField = "ExamMonthYear";
                    ddlflExam.DataBind();
                    ddlflExam.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select One--", "0"));


                }
            }
            else
            {
                ddlflExam.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select One--", "0"));
            }
        }

    }
    public System.Data.DataTable GetDataForPurskarApplicationCommitteeReport(int ViewCode)
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        System.Data.DataTable myDt = new System.Data.DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetExamNamePurskarApplication", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;             
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
    protected void ddlflExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {          
            string ExamMonthYear = "18471";
            BindGridView(ExamMonthYear);
            divGrid.Visible = true;
            lblMessage.Text = "";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnViewRecords_Click(object sender, EventArgs e)
    {
        string ExamMonthYear = ddlflExam.SelectedValue;
        BindGridView(ExamMonthYear);
        divGrid.Visible = true;
    }
    public DataTable FillGridViewOnlinePuraskarApplicationRecordExamiIdWise( string ExamMonthYear)
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
                cmd.Parameters["@ExamMonthYear"].Value = ExamMonthYear;
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
            //using (SqlCommand cmd = new SqlCommand("getPapersAppearedPassedDetails", con))
            using (SqlCommand cmd = new SqlCommand("getPapersAppearedPassedDetailsViewByExamFinance", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add(new SqlParameter("@CodeForViewRecord", SqlDbType.Int));
                //cmd.Parameters["@CodeForViewRecord"].Value = 112;
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

   protected void BindGridView( string ExamYearMonth)
    {
        try
        {
            lblError.Visible = false;
            string ExamMonthYearr = ExamYearMonth;

            using (DataTable dt = FillGridViewOnlinePuraskarApplicationRecordExamiIdWise(  ExamMonthYearr))
            {
                if (dt.Rows.Count > 0)
                {
                    Int32 VerifiedStatus = 0;
                    VerifiedStatus = Convert.ToInt32(ddlVerifiedStatus.SelectedValue);
                    string searchString = ucSearchBar.SearchText.Trim().ToUpper();

                    var ApplicationData = (from p in dt.AsEnumerable()
                                           select new
                                           {
                                               //ID = p.Field<Int64>("ID"),
                                               Regno = p.Field<Int64>("Regno"),
                                               Name = p.Field<string>("Name"),
                                               Level = p.Field<string>("Level"),
                                               Examid = p.Field<Int64>("ExamID"),
                                               Exams = p.Field<string>("ExamName"),
                                               FatherName = p.Field<string>("FatherName"),
                                               DOB = p.Field<DateTime>("DOB"),
                                               Gender = p.Field<string>("Gender"),
                                               AadharNumber = EncryptDecrypt.DecryptString(p.Field<string>("AadharNumber")),
                                               Caste = p.Field<string>("Caste"),
                                               VerifiedStatusFilter = p.Field<int>("VerifiedStatusFilter"),
                                               VerifiedStatus = p.Field<string>("VerifiedStatus"),
                                               examRejectionReason = p.Field<string>("examRejectionReason"),

                                           });
                    if (VerifiedStatus != 99)
                    {
                        ApplicationData = ApplicationData.Where(s => s.VerifiedStatusFilter == VerifiedStatus);
                    }
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        ApplicationData = ApplicationData.Where(s => s.Name.ToUpper().Contains(searchString));
                    }                 

                   if (ApplicationData.ToList().Count <= 0)
                   {
                       lblError.Text = "No record found.";
                       lblError.Visible = true;
                       uPnlGrid.Update();
                       uPnlNavigation.Update();
                       gvMain.Visible = false;

                   }
                   else
                   {
                       lblAffiliatedInst.Visible = true;
                       PagingBar1.Bind(ApplicationData, ref gvMain);
                       gvMain.Visible = true;
                       uPnlGrid.Update();
                       uPnlNavigation.Update();
                       divGrid.Visible = true;
                       lblError.Visible = false;
                        PagingBar1.Visible = true;
                   }
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "No record found.";
                        lblError.Visible = true;
                    }
                }
                else
                {                    
                    if (ExamMonthYearr != "18471")
                    {
                        lblAffiliatedInst.Visible = true;
                        lblError.Text = "No record found.";
                        lblError.Visible = true;
                    }
                    lblAffiliatedInst.Visible = false;                   
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    gvMain.Visible = false;
                }
            }
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {        
        Response.Redirect("PuraskarApplicationFormVerificationByExam.aspx", true);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("PuraskarApplicationFormVerificationByExam.aspx", true);
    }
        
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            using (EConnectContext context = new EConnectContext())
            {               
                PuraskarApplicationForm objpuraskarApplication;
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    objpuraskarApplication = new PuraskarApplicationForm();

                    foreach (GridViewRow row in gvMain.Rows)
                    {
                        bool isChecked = row.Cells[11].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                        if (isChecked)
                        {
                            int RegnNo = Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Value);

                            // cell value from gridview
                            Label lblExamid = row.Cells[12].Controls.OfType<Label>().FirstOrDefault();
                            Examid = Convert.ToInt64(lblExamid.Text);                            

                            //// check the candidate modules verification status for update the candidate status
                            //OnlineProtsahanExamModules objOnlineProtsahanExamModules;
                            //objOnlineProtsahanExamModules = new OnlineProtsahanExamModules();
                            // var CountMoudulesUpdation = (from s in context.OnlineProtsahanExamModuless
                            //                   where s.RegnNo == RegnNo && s.ExamID ==Examid && s.isVerifiedByExam==true
                            //                   select new
                            //                   { ID = s.ID, ModuleId = s.ModuleID }).Distinct();

                            // if (CountMoudulesUpdation.Count() <= 0)
                            // {
                            //     lblMessage.Text = "Please update the modules, Click on View, Then after verify the Candidates Status !";
                            //     lblMessage.ForeColor = System.Drawing.Color.Red;
                            //     return;
                            // }
                            // // check the candidate modules verification status for update the candidate status END 
                            
                            objpuraskarApplication = context.PuraskarApplicationForms.Where(a => a.RegnNo == RegnNo && a.ExamID == Examid).FirstOrDefault();
                            string examRejectionReason = row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Text;
                            if (examRejectionReason != "")
                            {
                                objpuraskarApplication.examRejectionReason = row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Text;
                            }
                            string Examverifystatus = Convert.ToString(row.Cells[10].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem);
                            if (Examverifystatus == "Verified")
                            {
                                objpuraskarApplication.verifiedByExam = true;
                                objpuraskarApplication.applicationStatusID = Convert.ToInt32(enmPuraskarApplicationStatus.VerifiedByExamWingButFinanceWingVerificationPending);
                            }
                            else if (Examverifystatus == "Rejected")
                            {
                                objpuraskarApplication.verifiedByExam = false;
                                objpuraskarApplication.applicationStatusID = Convert.ToInt32(enmPuraskarApplicationStatus.RejectedByExamWing);
                            }                          
                            objpuraskarApplication.verifiedByExamUser = Convert.ToInt32(Session["UserID"]);
                            objpuraskarApplication.VerifiedExamOn = DateTime.Now;                          
                            context.SaveChanges();
                            strMessage = "Record updated.";
                            string ExamMonthYear = ddlflExam.SelectedValue;
                            BindGridView(ExamMonthYear);
                                        //BindGridView();
                                        lblMessage.Text = "Data updated successfully!";
                                        lblMessage.ForeColor = System.Drawing.Color.Green;
                                        btnSave.Visible = false;
                                        btnCancel.Visible = false;

                                        // OnlineProtsahanExamModules isprocessed column update true regNowise and examiD for complete the process for candidate verification.
                                        //var objProtsahanExamModulesProcessed = context.OnlineProtsahanExamModuless.Where(a => a.RegnNo == RegnNo && a.ExamID == Examid && a.isVerifiedByExam==true).ToList();
                                        //if (objProtsahanExamModulesProcessed.Count > 0)
                                        //{
                                        //    objProtsahanExamModulesProcessed.ForEach(a => a.isProcessed = true);
                                        //    context.SaveChanges();
                                        //}
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
    protected void OnCheckedChanged(object sender, EventArgs e)
    {
        bool isUpdateVisible = false;       
       lblMessage.Text = string.Empty;
        //Loop through all rows in GridView
        foreach (GridViewRow row in gvMain.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[11].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                    row.RowState = DataControlRowState.Edit;               
                for (int i = 10; i < 11; i++)
                {                   
                    if (row.Cells[i].Controls.OfType<TextBox>().ToList().Count > 0)
                    {
                    row.Cells[i].Controls.OfType<DropDownList>().FirstOrDefault().Visible = isChecked;
                        row.Cells[i].Controls.OfType<Label>().FirstOrDefault().Visible = !isChecked;
                    }
                    if (isChecked && !isUpdateVisible)
                    {
                        isUpdateVisible = true;
                       // btnCancel.Visible = true;
                    }                  
                    GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
                    DropDownList duty = (DropDownList)gvr.FindControl("ddlVerifieds");
                    Int32 candStatus1 = Convert.ToInt32(duty.SelectedItem.Value);
                    Label VerifyStatus = (Label)gvr.FindControl("Label1");
                    Label ExamRejectionReasonLbl = (Label)gvr.FindControl("Label2");

                    TextBox ExamRejectionReasonTXT = (TextBox)gvr.FindControl("txtexamRejectionReason");                 
                    string candstatus = VerifyStatus.Text;
                    if (duty.SelectedItem.Text == "Rejected")
                    {
                        row.Cells[i].Controls.OfType<TextBox>().FirstOrDefault().Visible = isChecked;
                    }
                    if (VerifyStatus.Text == "Rejected")
                    {                      
                        duty.SelectedValue = "0";
                        duty.Enabled = false;   
                    }
                    else if (VerifyStatus.Text == "Verified")
                    {                        
                        duty.SelectedValue = "1";
                    }
                    else
                    {
                        duty.SelectedValue = "-1";
                    }
                    String ExamRejectionReasonLbl1 = ExamRejectionReasonLbl.Text;
                    if (VerifyStatus.Text == "Verified" || VerifyStatus.Text == "Pending")
                    {
                        row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Enabled = false;
                        row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Visible = false;
                    }
                }               

                String ExamRejectionReason1 = row.Cells[10].Controls.OfType<DropDownList>().FirstOrDefault().Text;

                if (ExamRejectionReason1 == "0")
                {
                    row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Enabled = true;
                    row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Visible = true;
                }    
               String ExamRejectionReason = row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Text;

               if (ExamRejectionReason != "")
                {
                    row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Enabled = false;
                    row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Visible = false;
                }               
              
            }
        }
        btnSave.Visible = isUpdateVisible;
      
    }  
    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            ddlVerifiedStatus.SelectedValue = "99";
            string ExamMonthYear = ddlflExam.SelectedValue;
            BindGridView(ExamMonthYear);
            divGrid.Visible = true;
            //ddlVerifiedStatus.SelectedValue = "99";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            string ExamMonthYear = ddlflExam.SelectedValue;
            BindGridView(ExamMonthYear);
            divGrid.Visible = true;
           // BindGridView();
            //string ExamMonthYear = ddlflExam.SelectedValue;
            //BindGridView(ExamMonthYear);
            //divGrid.Visible = true;
           PagingBar1.CurrentPageIndex = 0;
           gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void SearchBar_Reset(object sender, EventArgs e)
    {
        try
        {
            string ExamMonthYear = ddlflExam.SelectedValue;
            BindGridView(ExamMonthYear);
            divGrid.Visible = true;
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
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
           // BindGridView();
            string ExamMonthYear = "20178";
            BindGridView(ExamMonthYear);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
   
   
    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {        
        GridViewRow row = gvMain.SelectedRow;
        //Find label id from gridview cell data
        Label lblRegno = row.FindControl("lblRegno") as Label;
        Label lblExamid = row.FindControl("lblExamid") as Label;
        Int64 Examid = 0;
        Examid = Convert.ToInt64(lblExamid.Text);
        Int64 RegistrationNo = Convert.ToInt64(lblRegno.Text);
        string ViewFor = "E";
       // window.open("NielitCentreBatchwiseFinancialRep.aspx?CourseId=" + CourseId + "&InstId=" + InstId + "&CourseCatId=" + CourseCatId + "&BatchId=" + BatchId +"&centreType=" + centreType, 'Financial Report', 'width=1100,height=600,menubar=no,titlebar=no,toolbar=no,status=no,scrollbars=yes,dependent=yes,resizable=yes', false);
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('PuraskarAppModulesVerificationByExam.aspx?Examid=" + Examid + "&RegistrationNo=" + RegistrationNo + "&VF=" + ViewFor + "' ,'_blank');", true);

       // Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("PuraskarAppModulesVerificationByExam.aspx?msg=" + strMessage + "&RegistrationNo=" + RegistrationNumber + "&Examid=" + Examid), true);
        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('../Rpt/PreviousExamDetailsForPuraskarCand.aspx' ,'_blank')", true);
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
      
                if (lbl1.Text == "Rejected" || lbl1.Text == "Verified")
                {

                    CheckBox1.Enabled = false;  
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
            //BindGridView();
            string ExamMonthYear = "20178";
            BindGridView(ExamMonthYear);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    } 

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        EConnectContext  context = new EConnectContext ();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var canddidateName = from s in context.PuraskarApplicationForms where s.verifiedByInstt==true
                           select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                canddidateName = canddidateName.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            canddidateName = canddidateName.OrderBy(s => s.Name);           
            foreach (var linkName in canddidateName)
            {
                items.Add(linkName.Name);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
}