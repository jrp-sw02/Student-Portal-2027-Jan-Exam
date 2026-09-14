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

public partial class Admin_PuraskarDocsUploadedFormVerification : BasePage
{    
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
                if (!String.IsNullOrEmpty(Request.QueryString["Regno"]))
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
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Puraskar Placement Docs Uploaded Verification", "Admin/PuraskarDocsUploadedFormVerification.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Puraskar Placement Docs Uploaded Verification", "Admin/PuraskarDocsUploadedFormVerification.aspx", ""));
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
            TextBox VerifyStatus = (TextBox)gvr.FindControl("txtfinanceRejectionReason");
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

    protected void ddlisWithHeldFinance_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            DropDownList duty = (DropDownList)gvr.FindControl("ddlisWithHeldFinance");
            DropDownList financeVerify = (DropDownList)gvr.FindControl("ddlVerifieds");
            Int32 candStatus = Convert.ToInt32(duty.SelectedItem.Value);
            TextBox VerifyStatus = (TextBox)gvr.FindControl("txtwithHeldReason");
            TextBox txtfinanceRejectionReason = (TextBox)gvr.FindControl("txtfinanceRejectionReason");
           
            if (candStatus == 1)
            {
                VerifyStatus.Visible = true;
                VerifyStatus.Enabled = true;
                financeVerify.SelectedValue = "-1";
                financeVerify.Enabled = false;
                txtfinanceRejectionReason.Visible = false;
            }
            else if (candStatus == 2)
            {
                VerifyStatus.Visible = true;
                VerifyStatus.Enabled = true;
                VerifyStatus.Text = "";
               
            }
            else if (candStatus == 0)
            {
                txtfinanceRejectionReason.Visible = false;
                financeVerify.Enabled = true;
                VerifyStatus.Visible = false;
                
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable FillGridViewPuraskarDocsUploaded()
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("FillGridViewPuraskarDocsUpload", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CodeForViewRecord", SqlDbType.Int));
                cmd.Parameters["@CodeForViewRecord"].Value = 11;
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
  
    protected void BindGridView()
    {
        try
        {          
            using (DataTable dt = FillGridViewPuraskarDocsUploaded())
            {
                if (dt.Rows.Count > 0)
                {
                    Int32 VerifiedStatus = 0, lvl=0;
                    Int64 regno = 0;
                    VerifiedStatus = Convert.ToInt32(ddlVerifiedStatus.SelectedValue);
                    lvl = Convert.ToInt32(drplvl.SelectedValue);
                    if (txtRegNo.Text != "" && lvl!=0)
                    {
                        regno = Convert.ToInt64(txtRegNo.Text.Trim());
                    }
                    //string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                    //if (searchString != "")
                    //{
                    //    Int64 searchRegNo = Convert.ToInt64(searchString);
                    //}
                    var ApplicationData = (from p in dt.AsEnumerable()
                                           select new
                                           {
                                               //ID = p.Field<Int64>("ID"), string aadharDecrypted = EncryptDecrypt.DecryptString(encAdhar); Course_ID
                                               Regno = p.Field<Int64>("Regno"),
                                               Name = p.Field<string>("Name"),
                                               Level = p.Field<string>("Level"),
                                               RegLevel= Convert.ToString(p.Field<Int64>("Regno"))+' '+  '(' + p.Field<string>("Level") +')',
                                               Examid = p.Field<Int64>("ExamID"),
                                               CourseID = p.Field<int>("Course_ID"),
                                               VerifyStatusID = p.Field<int>("VerifyStatusID"),
                                               Exams = p.Field<string>("ExamName"),
                                               FatherName = p.Field<string>("FatherName"),
                                               DOB = p.Field<DateTime>("DOB"),
                                               Salary = p.Field<Int64>("Salary"),
                                               PlacementDatesFromTo = p.Field<string>("PlacementDatesFromTo"),
                                               CompanyDetails = p.Field<string>("CompanyDetails"),                                              
                                               VerifiedStatus = p.Field<string>("VerifiedStatus"),
                                              financeRejectionReason = p.Field<string>("financeRejectionReason"),
                                               isWithHeldFinance = p.Field<string>("isWithHeldFinance"),
                                               withHeldReason = p.Field<string>("withHeldReason"),
                                         });

                    if (lvl == 0 && regno ==0)
                    {
                        if (VerifiedStatus != 99)
                        {
                            ApplicationData = ApplicationData.Where(s => s.VerifyStatusID == VerifiedStatus);
                        }
                        else
                        {
                            ApplicationData = ApplicationData.Where(s => s.VerifyStatusID == 999);
                        }
                    }                        
                    else
                    {
                        ApplicationData = ApplicationData.Where(s => s.CourseID == lvl && s.Regno==regno);
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
    protected void drplvl_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (drplvl.SelectedValue == "0")
            {
               // trlvl.Visible = false;
            }
            txtRegNo.Text = "";
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("PuraskarDocsUploadedFormVerification.aspx", true);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("PuraskarDocsUploadedFormVerification.aspx", true);
    }

    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            Int64 RegnNo = 0;
            string withHeldReason = string.Empty; 
            string withHeldReasonss = string.Empty;
            string financeRejectionReason = string.Empty;
            BreadCrumb1.Render();
            using (EConnectContext context = new EConnectContext())
            {   
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {                   
                    foreach (GridViewRow row in gvMain.Rows)
                    {
                        bool isChecked = row.Cells[8].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                        if (isChecked)
                        {
                            RegnNo = Convert.ToInt64(gvMain.DataKeys[row.RowIndex].Value);                          
                            Label lblExamid = row.Cells[9].Controls.OfType<Label>().FirstOrDefault();
                            Label Lvl = row.Cells[3].Controls.OfType<Label>().FirstOrDefault();                                                   
                            Examid = Convert.ToInt64(lblExamid.Text);
                            Label lblCourseId = row.Cells[10].Controls.OfType<Label>().FirstOrDefault();
                            int courseid = Convert.ToInt32(lblCourseId.Text);                       

                            // check the candidate Uploaded documents verification status                                              
                            string Data = FetchData(RegnNo, courseid, Examid, 21);
                            if (Data =="0")
                            {
                                lblMessage.Text = "Please update the Uploaded Documents, Click on View, Then after verify the Candidates Status !";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                ShowAlert("Please update the Uploaded Documents, Click on View, Then after verify the Candidates Status !", true);
                                return;
                            }
                            // check the candidate Uploaded documents verification status END 
                                 
                            // deep add with held case for Line
                            string WithHeldResolvedstatus = Convert.ToString(row.Cells[6].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem);
                            string financeWithHeldReason = row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Text;
                            if (financeWithHeldReason != "" && WithHeldResolvedstatus != "RESOLVED")
                            {
                                withHeldReason = row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Text;
                            }
                          
                            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                            SqlConnection Conn = new SqlConnection(constr); 
                            SqlCommand cmd = new SqlCommand("UpdateRecordForPuraskarDocsUpload", Conn);                                
                            Conn.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@RegnNo", RegnNo);
                            cmd.Parameters.AddWithValue("@ExamId", Examid);
                            cmd.Parameters.AddWithValue("@courseid", courseid);
                            string WithHeldResonExistsCount = FetchData(RegnNo, courseid, Examid, 22);

                            String withHeldreasons = string.Empty;
                            
                            if (WithHeldResonExistsCount!="0")
                            {   
                                string WithHeldResonExists = FetchData(RegnNo, courseid, Examid, 23);
                                withHeldreasons = WithHeldResonExists;                                                              
                            }                           
                            
                            if (WithHeldResolvedstatus == "RESOLVED")
                            {                               
                                withHeldReasonss = withHeldreasons + ", " + row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Text;

                                cmd.Parameters.AddWithValue("@WStatus", WithHeldResolvedstatus);
                                cmd.Parameters.AddWithValue("@isWithHeldFinance", 0);

                                int AllreadyResolved = Convert.ToInt32(FetchData(RegnNo, courseid, Examid, 27));
                                if (AllreadyResolved == 0)
                                {
                                    cmd.Parameters.AddWithValue("@withHeldReasons", withHeldReasonss);
                                }
                                else
                                {
                                    cmd.Parameters.AddWithValue("@withHeldReasons", withHeldreasons);
                                }
                                cmd.Parameters.AddWithValue("@withHeldResolvedDate", DateTime.Now);                                
                            }
                            
                            string WithHeldstatus = Convert.ToString(row.Cells[6].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem);
                            
                            if (WithHeldstatus == "YES")
                            {                               
                                cmd.Parameters.AddWithValue("@WStatus", WithHeldstatus);
                                cmd.Parameters.AddWithValue("@isWithHeldFinance", 1);
                                cmd.Parameters.AddWithValue("@withHeldReasons", withHeldReason);
                                cmd.Parameters.AddWithValue("@withHeldResolvedDate", DBNull.Value);

                                cmd.Parameters.AddWithValue("@AllDocumentsVerified", DBNull.Value);
                                cmd.Parameters.AddWithValue("@AllDocumentsVerifiedBy", DBNull.Value);
                                cmd.Parameters.AddWithValue("@AllDocumentsVerifiedDate", DBNull.Value);
                                cmd.Parameters.AddWithValue("@financeRejectionReason", DBNull.Value);                               
                            }
                            else if (WithHeldstatus == "NO" && withHeldreasons == "")
                            {   
                                cmd.Parameters.AddWithValue("@WStatus", WithHeldstatus);
                                cmd.Parameters.AddWithValue("@isWithHeldFinance", 0);
                                cmd.Parameters.AddWithValue("@withHeldReasons", DBNull.Value);
                                cmd.Parameters.AddWithValue("@withHeldResolvedDate", DBNull.Value);                                
                            }
                            else if (WithHeldstatus == "NO")
                            {                                
                                cmd.Parameters.AddWithValue("@WStatus", WithHeldstatus);
                                cmd.Parameters.AddWithValue("@isWithHeldFinance", 0);
                                cmd.Parameters.AddWithValue("@withHeldReasons", withHeldReasonss);
                                cmd.Parameters.AddWithValue("@withHeldResolvedDate", DBNull.Value);                              
                            }
                            else if (WithHeldstatus == "YES" && withHeldreasons != "")
                            {
                                lblMessage.Text = "Please relsoved this case, it is withHeld case !";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                Conn.Close();
                                return;
                            }                         

                            string financeRejectionReasons = row.Cells[7].Controls.OfType<TextBox>().FirstOrDefault().Text;
                            if (financeRejectionReasons != "")
                            {                              
                                financeRejectionReason = row.Cells[7].Controls.OfType<TextBox>().FirstOrDefault().Text;
                            }

                            string Financeverifystatus = Convert.ToString(row.Cells[7].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem);
                            if (Financeverifystatus == "Verified")
                            {
                                if (WithHeldstatus == "NO" || WithHeldstatus == "RESOLVED")
                                {   
                                    string CheckWhCases = FetchData(RegnNo, courseid, Examid, 24);
                                    
                                    if (WithHeldstatus != "RESOLVED")
                                    {
                                        if (CheckWhCases != "0")
                                        {
                                            lblMessage.Text = "Please relsoved this case, it is withHeld case !";
                                            lblMessage.ForeColor = System.Drawing.Color.Red;
                                            Conn.Close();
                                            return;
                                        }
                                    }                                    
                                    cmd.Parameters.AddWithValue("@AllDocumentsVerified", 1);
                                    cmd.Parameters.AddWithValue("@AllDocumentsVerifiedBy", Convert.ToInt32(Session["UserID"]));
                                    cmd.Parameters.AddWithValue("@AllDocumentsVerifiedDate", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@financeRejectionReason", DBNull.Value);
                                }
                                else
                                {
                                    lblMessage.Text = "Please relsoved this case, it is withHeld case !";
                                    lblMessage.ForeColor = System.Drawing.Color.Red;
                                    Conn.Close();
                                    return;
                                }
                            }
                            else if (Financeverifystatus == "Rejected")
                            {  
                                string CheckWhCases = FetchData(RegnNo, courseid, Examid, 24);
                                
                                if (WithHeldstatus != "RESOLVED")
                                {
                                    if (CheckWhCases != "0")
                                    {
                                        lblMessage.Text = "Please relsoved this case, it is withHeld case !";
                                        lblMessage.ForeColor = System.Drawing.Color.Red;
                                        Conn.Close();
                                        return;
                                    }
                                }  
                                cmd.Parameters.AddWithValue("@AllDocumentsVerified", 0);
                                cmd.Parameters.AddWithValue("@AllDocumentsVerifiedBy", Convert.ToInt32(Session["UserID"]));
                                cmd.Parameters.AddWithValue("@AllDocumentsVerifiedDate", DateTime.Now);
                                cmd.Parameters.AddWithValue("@financeRejectionReason", financeRejectionReason);
                            }
                            else if ((Financeverifystatus == "--Select One--" && WithHeldstatus == "NO") ||(Financeverifystatus == "--Select One--" && WithHeldstatus == "RESOLVED") )
                            {
                                cmd.Parameters.AddWithValue("@AllDocumentsVerified", DBNull.Value);
                                cmd.Parameters.AddWithValue("@AllDocumentsVerifiedBy", DBNull.Value);
                                cmd.Parameters.AddWithValue("@AllDocumentsVerifiedDate", DBNull.Value);
                                cmd.Parameters.AddWithValue("@financeRejectionReason", DBNull.Value);
                            }      
                               cmd.ExecuteNonQuery();
                               Conn.Close();                           
                            
                            strMessage = "Record updated.";
                            BindGridView();
                            lblMessage.Text = "Data updated successfully!";
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                            btnSave.Visible = false;
                            btnCancel.Visible = false;                           
                        }                       
                    }
                }
            }
        }
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
        catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
        {
            Exception raise = dbEx;
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    string message = string.Format("{0}:{1}",
                        validationErrors.Entry.Entity.ToString(),
                        validationError.ErrorMessage);
                    // raise a new exception nesting  
                    // the current instance as InnerException  
                    raise = new InvalidOperationException(message, raise);
                }
            }
            throw raise;
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
            if (DataValues != null && DataValues !="")
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
       
       lblMessage.Text = string.Empty;
        //Loop through all rows in GridView
        foreach (GridViewRow row in gvMain.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[8].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                    row.RowState = DataControlRowState.Edit;               
                for (int i = 6; i < 8; i++)
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
                    Label FinanceRejectionReasonLbl = (Label)gvr.FindControl("Label2");

                    TextBox FinanceRejectionReasonTXT = (TextBox)gvr.FindControl("txtfinanceRejectionReason");                 
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
                        duty.Enabled = false; 
                    }
                    else
                    {
                        duty.SelectedValue = "-1";
                        duty.Enabled = true;
                    }
                    String FinanceRejectionReasonLbl1 = FinanceRejectionReasonLbl.Text;
                    if (VerifyStatus.Text == "Verified" || VerifyStatus.Text == "Pending")
                    {
                        row.Cells[7].Controls.OfType<TextBox>().FirstOrDefault().Enabled = false;
                        row.Cells[7].Controls.OfType<TextBox>().FirstOrDefault().Visible = false;
                    }                   
                    DropDownList ddlisWithHeldFinance = (DropDownList)gvr.FindControl("ddlisWithHeldFinance");
                    Int32 isWithHeldcandStatus = Convert.ToInt32(duty.SelectedItem.Value);
                    Label isWithHeldFinance = (Label)gvr.FindControl("Label3");
                    Label withHeldReason = (Label)gvr.FindControl("Label4");

                    TextBox txtwithHeldReasonTXT = (TextBox)gvr.FindControl("txtwithHeldReason");
                    string candstatus2 = isWithHeldFinance.Text;
                    if (ddlisWithHeldFinance.SelectedItem.Text == "YES" || ddlisWithHeldFinance.SelectedItem.Text == "RESOLVED")
                    {
                        row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Visible = isChecked;
                    }
                    if (isWithHeldFinance.Text == "YES")
                    {
                        ddlisWithHeldFinance.SelectedValue = "1";
                        ddlisWithHeldFinance.Enabled = true;
                    }
                    else if (isWithHeldFinance.Text == "NO")
                    {
                        ddlisWithHeldFinance.SelectedValue = "0";
                        ddlisWithHeldFinance.Enabled = true;
                    }
                    else if (isWithHeldFinance.Text == "RESOLVED")
                    {
                        ddlisWithHeldFinance.SelectedValue = "2";
                        ddlisWithHeldFinance.Enabled = true;
                    }
                    else if (isWithHeldFinance.Text == "WithHeldResolved")
                    {
                        ddlisWithHeldFinance.SelectedValue = "2";
                        ddlisWithHeldFinance.Enabled = false;
                        ddlisWithHeldFinance.Visible = false;
                        isWithHeldFinance.Visible = true;
                        isWithHeldFinance.Text = "WithHeldResolved";
                    }
                    else
                    {
                        ddlisWithHeldFinance.SelectedValue = "-1";
                    }

                    String FinancewithHeldReasonLbl1 = withHeldReason.Text;
                    if (isWithHeldFinance.Text == "NO" || isWithHeldFinance.Text == "Pending" || isWithHeldFinance.Text == "WithHeldResolved")
                    {
                        row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Enabled = false;
                        row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Visible = false;
                    }                  
                }                

                String FinanceRejectionReason1 = row.Cells[7].Controls.OfType<DropDownList>().FirstOrDefault().Text;

                if (FinanceRejectionReason1 == "0")
                {
                    row.Cells[7].Controls.OfType<TextBox>().FirstOrDefault().Enabled = true;
                    row.Cells[7].Controls.OfType<TextBox>().FirstOrDefault().Visible = true;
                }    
               String FinanceRejectionReason = row.Cells[7].Controls.OfType<TextBox>().FirstOrDefault().Text;

               if (FinanceRejectionReason != "")
                {
                    row.Cells[7].Controls.OfType<TextBox>().FirstOrDefault().Enabled = false;
                    row.Cells[7].Controls.OfType<TextBox>().FirstOrDefault().Visible = false;
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
            drplvl.SelectedValue = "0";
            txtRegNo.Text = "";
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

    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {       
        GridViewRow row = gvMain.SelectedRow;
        //Find label id from gridview cell data
        Label lblRegno = row.FindControl("lblRegno") as Label;
        Label lblExamid = row.FindControl("lblExamid") as Label;
        Label lblLevelCode = row.FindControl("lblcode") as Label;       
        Int64 Examid = 0;
        Examid = Convert.ToInt64(lblExamid.Text);
        Int64 RegistrationNo = Convert.ToInt64(lblRegno.Text);       
        string url = "PuraskarAppDocsVerification.aspx?Examid=" + Examid + "&RegistrationNo=" + RegistrationNo;
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('" +url+"' ,'_blank');", true);      
        lblMessage.Text = "";       
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
            BindGridView();
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
            var canddidateName = from s in context.PuraskarApplicationForms where s.verifiedByExam==true
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