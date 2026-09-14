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
using System.Text;

public partial class Admin_PuraskarApplicationFormVerificationByInstitute : BasePage
{
    NIELITMISContext  context;
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 InstituesIDRefNo = 0;
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
            InstituesIDRefNo = Convert.ToInt32(Session["EntityID"]); 
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
		if (System.DateTime.Today > Convert.ToDateTime("2022-12-25"))
                {
                    ShowAlert("Date for verification is over, You can only view the details");
                   btnSave.Visible = false;
                 
                }
                else
                {
                    btnSave.Visible = true;
                    
                }
		//		btnSave.Visible = true;
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    BindGridView(); 
                }
                else
                {                   
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";                  

                    BindGridView();
                    if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Online Puraskar Application", "Admin/PuraskarApplicationFormVerificationByInstitute.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Online Puraskar Application", "Admin/PuraskarApplicationFormVerificationByInstitute.aspx", ""));
                    }

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
            TextBox VerifyStatus = (TextBox)gvr.FindControl("txtInsttRejectionReason");
            if (candStatus == 0)
            {
                // TextBox VerifyStatus = (TextBox)gvr.FindControl("txtInsttRejectionReason");
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
                cmd.Parameters["@CodeForViewRecord"].Value = 11;
                cmd.Parameters.Add(new SqlParameter("@InstituteID", SqlDbType.BigInt));
                cmd.Parameters["@InstituteID"].Value = Convert.ToInt32(HttpContext.Current.Session["EntityID"]);
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
    protected void BindGridView()
    {
        try
        {
            //using (EConnectContext context = new EConnectContext())
            //{
            using (DataTable dt = FillGridViewOnlinePuraskarApplicationRecordExamiIdWise())
            {
                if (dt.Rows.Count > 0)
                {
                    Int32 VerifiedStatus = 0;
                    VerifiedStatus = Convert.ToInt32(ddlVerifiedStatus.SelectedValue);
                    string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                    ////var ApplicationData = from s in context.PuraskarApplicationForms
                    ////                      join p in context.CourseRegistrationApplications
                    ////                      on s.CandidateID equals p.CandidateID join k in context.CastCategories on p.CastCategoryID  equals k.ID
                    ////                      join  m in context.Courses on p.CourseID equals m.ID join e in context.Exams on s.ExamID equals e.ID
                    ////                      where s.finalSubmit==true 
                    ////                      select new
                    ////                      {
                    ////                          ID = s.ID,
                    ////                          Regno=s.RegnNo,
                    ////                          Name = s.Name,
                    ////                          Level=m.Code,
                    ////                          Examid=s.ExamID,
                    ////                          Exams=e.Name,
                    ////                          FatherName = p.FatherName,
                    ////                          DOB = p.DateOfBirth,
                    ////                          Gender=p.Gender,
                    ////                          AadharNumber=s.AadharNumber,
                    ////                          Caste=k.Name,
                    ////                          VerifiedStatusFilter = s.verifiedByInstt == true ? 1 : s.verifiedByInstt == false ? 0 : 999, 
                    ////                          VerifiedStatus = s.verifiedByInstt == true ? "Verified" : s.verifiedByInstt == false ? "Rejected" :"Pending", 
                    ////                          InsttRejectionReason =s.verifiedByInstt == false ? s.InsttRejectionReason :"" 
                    ////                      };
                    var ApplicationData = (from p in dt.AsEnumerable()
                                        select new
                                        {
                                            ID = p.Field<Int64>("ID"),
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
                                            InsttRejectionReason = p.Field<string>("InsttRejectionReason"),
                                           
                                        });     
                    if (VerifiedStatus != 99)
                    {
                        ApplicationData = ApplicationData.Where(s => s.VerifiedStatusFilter == VerifiedStatus);
                    }
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        ApplicationData = ApplicationData.Where(s => s.Name.ToUpper().Contains(searchString));
                    }
                    PagingBar1.Bind(ApplicationData, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    lblError.Visible = false;
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "No record found.";
                        lblError.Visible = true;
                    }
                    // }
                }
                else
                {
                    lblError.Text = "No record found.";
                    lblError.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
  
    protected void btnCancel_Click(object sender, EventArgs e)
    {        
        Response.Redirect("PuraskarApplicationFormVerificationByInstitute.aspx", true);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("PuraskarApplicationFormVerificationByInstitute.aspx", true);
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
                           int RegnNo =0;
                             RegnNo = Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Value);
                          //deep on 18 May 2020
                            Label lblExamid = row.Cells[12].Controls.OfType<Label>().FirstOrDefault();
                            Examid = Convert.ToInt64(lblExamid.Text);
                            //deep 18 May 2020 end
                            var RecordsVerified = (from s in context.PuraskarAppDocsVerificationAndDeclarationByInstts
                                                   where s.RegnNo == RegnNo && s.ExamID == Examid && s.DocsVerifiedByInstt == true && s.DeclarationByInstt == true
                                                   select new { ID = s.RegnNo, verifyStatus = s.DeclarationByInstt }).Distinct();
                            string Instverifystatus2 = Convert.ToString(row.Cells[10].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem);
                            if (RecordsVerified.Count() == 0)
                            {
                                if (Instverifystatus2 != "Rejected")
                                {
                                    lblMessage.Text = "Please update the documents verification, Click on View, Then after verify the Candidates Status !!";
                                    lblMessage.ForeColor = System.Drawing.Color.Red;
                                    return;
                                }
                            }
                           // PuraskarApplicationForm objpuraskarApplication1 = context.PuraskarApplicationForms.Find(RegnNo);    
                            objpuraskarApplication = context.PuraskarApplicationForms.Where(a => a.RegnNo == RegnNo && a.ExamID == Examid).FirstOrDefault();
                            string InsttRejectionReason= row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Text;
                            if (InsttRejectionReason != "")
                            {
                                objpuraskarApplication.InsttRejectionReason = row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Text;
                            }
                            string Instverifystatus =Convert.ToString( row.Cells[10].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem);
                            if (Instverifystatus == "Verified")
                            {
                                objpuraskarApplication.verifiedByInstt=true;
                                objpuraskarApplication.applicationStatusID = Convert.ToInt32(enmPuraskarApplicationStatus.VerifiedByInstituteButNIELITVerificationPending);
                            }
                            else if (Instverifystatus == "Rejected")
                            {
                                objpuraskarApplication.verifiedByInstt = false;
                                objpuraskarApplication.applicationStatusID = Convert.ToInt32(enmPuraskarApplicationStatus.RejectedByInstitute);
                            }                          
                            objpuraskarApplication.verifiedByInsttUser = Convert.ToInt32(Session["UserID"]);
                            objpuraskarApplication.VerifiedInsttOn = DateTime.Now;                          
                            context.SaveChanges();
                            strMessage = "Record updated.";                          
                                        BindGridView();
                                        lblMessage.Text = "Data updated successfully!";
                                        lblMessage.ForeColor = System.Drawing.Color.Green;
                                        btnSave.Visible = false;
                                        btnCancel.Visible = false;

                                        UpdateProcessedDataStart(RegnNo, Examid);
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
                //for (int i = 10; i < row.Cells.Count; i++)
                for (int i = 10; i < 11; i++)
                {
                    // row.Cells[i].Controls.OfType<Label>().FirstOrDefault().Visible = !isChecked;
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
                    //deep 31
                    GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
                    DropDownList duty = (DropDownList)gvr.FindControl("ddlVerifieds");
                    Int32 candStatus1 = Convert.ToInt32(duty.SelectedItem.Value);
                    Label VerifyStatus = (Label)gvr.FindControl("Label1");
                    Label InsttRejectionReasonLbl = (Label)gvr.FindControl("Label2");

                    TextBox InsttRejectionReasonTXT = (TextBox)gvr.FindControl("txtInsttRejectionReason");
                   // Int32 candStatus = Convert.ToInt32(VerifyStatus.Text);
                    string candstatus = VerifyStatus.Text;
                    if (duty.SelectedItem.Text == "Rejected")
                    {
                        row.Cells[i].Controls.OfType<TextBox>().FirstOrDefault().Visible = isChecked;
                    }
                    if (VerifyStatus.Text == "Rejected")
                    {
                        // TextBox VerifyStatus = (TextBox)gvr.FindControl("VerifiedStatus");
                       // VerifyStatus.Visible = true;
                        duty.SelectedValue = "0";
                        duty.Enabled = false;
                       
                        //if (InsttRejectionReasonTXT.Text == "" || InsttRejectionReasonTXT.Text != "")
                        //{
                        //    row.Cells[9].Controls.OfType<TextBox>().FirstOrDefault().Enabled = true;
                        //    row.Cells[9].Controls.OfType<TextBox>().FirstOrDefault().Visible = true;
                        //}
                    }
                    else if (VerifyStatus.Text == "Verified")
                    {
                        // VerifyStatus.Visible = false;
                        duty.SelectedValue = "1";
                    }
                    else
                    {
                        duty.SelectedValue = "-1";
                    }
                    String InsttRejectionReasonLbl1 = InsttRejectionReasonLbl.Text;
                    if (VerifyStatus.Text == "Verified" || VerifyStatus.Text == "Pending")
                    {
                        row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Enabled = false;
                        row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Visible = false;
                    }
                   

                    //deep end 31

                }

                String InsttRejectionReason1 = row.Cells[10].Controls.OfType<DropDownList>().FirstOrDefault().Text;

                if (InsttRejectionReason1 == "0")
                {
                    row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Enabled = true;
                    row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Visible = true;
                }    
               String InsttRejectionReason = row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Text;

                if (InsttRejectionReason != "")
                {
                    row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Enabled = false;
                    row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Visible = false;
                }               
              
            }
        }
        btnSave.Visible = isUpdateVisible;      
	/*if (System.DateTime.Today > Convert.ToDateTime("2022-04-15"))
        {
            ShowAlert("Date for verification is over, You can only view the details");
            btnSave.Visible = false;

        }
        else
        {
            btnSave.Visible = isUpdateVisible;  

        }*/
    }

    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {

        GridViewRow row = gvMain.SelectedRow;
        //Find label id from gridview cell data
        Label lblRegno = row.FindControl("lblRegno") as Label;
        Label lblExamid = row.FindControl("lblExamid") as Label;
        //For expiry date
        int expired;
        CheckBox chk = row.FindControl("CheckBox1") as CheckBox;
        if (chk.Text.Contains("Expiry"))
            expired = 1;
        else
            expired = 0;
		
        Int64 Examid = 0;
        Examid = Convert.ToInt64(lblExamid.Text);
        Int64 RegistrationNo = Convert.ToInt64(lblRegno.Text);  
        string url = "PuraskarAppDocsVerAndDecByInstt.aspx?Examid=" + Examid + "&RegistrationNo=" + RegistrationNo+"&expired="+expired;
        EConnect.Utils.Security.QuertStringModule.Encrypt(url);
        string url2 = EConnect.Utils.Security.QuertStringModule.Encrypt(url);
       System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('" + url2 + "' ,'_blank');", true);
       
        //Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("PuraskarAppDocsVerAndDecByInstt.aspx?Examid=" + Examid + "&RegistrationNo=" + RegistrationNo), true); 
        lblMessage.Text = "";

    }  
    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {            
            ddlVerifiedStatus.SelectedValue = "99";
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
            BindGridView();
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
            BindGridView();
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
                //if (e.Row.Cells[9].Text == "Rejected")
                if (lbl1.Text == "Rejected" || lbl1.Text == "Verified")
                {
                    CheckBox1.Enabled = false;
                }
				 else
                {
                    //Added for institute verification expiry date
                    Int32 RegistrationNumber = 0; Int64 CandidateID = 0;
                    RegistrationNumber = Convert.ToInt32(hl.Text);
                    using (EConnectContext context = new EConnectContext())
                    {
                        CourseExamApplication objcandidaeID;
                        objcandidaeID = context.CourseExamApplications.Where(a => a.RegistrationNumber == RegistrationNumber).FirstOrDefault();
                        if (objcandidaeID == null)
                        {
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
                                        string dateInString = row["Result_Publish_Date"].ToString();
                                        if (dateInString == "")
                                        {
                                            dateInString = "1900-01-01";
                                        }


                                        DateTime startDate = DateTime.Parse(dateInString);
                                        DateTime expiryDate = startDate.AddDays(45);
                                        //DateTime expiryDate=Convert.ToDateTime ("2022-04-15");
                                        //DateTime expiryDate = Convert.ToDateTime("2022-04-16"); ;
                                        //if (startDate.Date == Convert.ToDateTime("2022-01-24").Date)
                                        //{
                                        //    expiryDate = Convert.ToDateTime("2022-04-15");
                                        //}

                                        // Checks 45 days not more than result publish date
                                        if (DateTime.Now.Date > expiryDate)
                                        {
                                            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Your due date is already expired. You can't apply the application.')", true);
                                            //return;
                                            CheckBox1.Enabled = false;
                                            CheckBox1.Text = "Expiry Date Over";
                                           // var button = (Button)e.Row.Cells[e.Row.Cells.Count - 1].Controls[0];
                                           // button.Enabled = false;
                                            continue;
                                            //DateTime Msessage 
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //Ends Expiry Date check




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
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    public void UpdateProcessedDataStart(int RegNo, Int64 ExamID)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection Conn = new SqlConnection(constr))
            {
                //SqlCommand Cmd = new SqlCommand(" SELECT ModulesCount FROM [NIELIT].[dbo].[e_temp_PapersCountEligibleForOnlinePuraskarAppForm] where courseID=@courseID", sqlCon);
                //Cmd.Parameters.AddWithValue("@courseID", courseID);
                using (SqlCommand cmd = new SqlCommand(" Update [NIELIT].[dbo].[e_temp_OnlinePuraskarApplication_PapersPassDetailsByCand] set IsProcessed=1 where RegistrationNumber=" + RegNo + "and Exam_ID=" + ExamID, Conn))
                {
                    Conn.Open();
                    cmd.CommandType = CommandType.Text;
                    //cmd.Parameters.AddWithValue("@RegNo", Convert.ToInt64(RegNo));
                    //cmd.Parameters.AddWithValue("@ExamID", Convert.ToInt64(ExamID));

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
       
      Int32  InstituesIDRefNo = Convert.ToInt32(HttpContext.Current.Session["EntityID"]);
        EConnectContext  context = new EConnectContext ();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var canddidateName = from s in context.PuraskarApplicationForms
                                 where s.InstituteID == InstituesIDRefNo
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