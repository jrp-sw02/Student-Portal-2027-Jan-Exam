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

public partial class Admin_PuraskarApplicationFormVerificationByFinance : BasePage
{
    NIELITMISContext  context;
    String strMessage = string.Empty;
    Int32 currentRoleId = 0, installmentsNoExists=0;
    Int32 loginUserNo = 0,installmentsNo=0;
    Int64 Examid = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            //if (!UserManager.HasRight(currentRoleId, enmRight.View))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Regno"]))
                {                    
                    string ExamMonthYear = ddlflExam.SelectedValue;
                    BindGridView(ExamMonthYear);
                    divGrid.Visible = true;
                }
                else
                {                   
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    ExamName();                   
                    if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Online Puraskar Application", "Admin/PuraskarApplicationFormVerificationByFinance.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Online Puraskar Application", "Admin/PuraskarApplicationFormVerificationByFinance.aspx", ""));
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

    public DataTable FillGridViewOnlinePuraskarApplicationRecordExamiIdWise(string ExamMonthYear)
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("FillGridViewOnlinePuraskarApplicationRecordExamiIdWise", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CodeForViewRecord", SqlDbType.Int));
                cmd.Parameters["@CodeForViewRecord"].Value = 13;
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
    public DataTable FillGridViewRegistraionNoPreviousInstallments(Int64 ExamID, Int64 regno, string levelcode,string viewrecord)
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("getPreviousInstallmentsDetailsViewByFinance", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Regno", SqlDbType.BigInt));
                cmd.Parameters["@Regno"].Value = regno;
                cmd.Parameters.Add(new SqlParameter("@pExamID", SqlDbType.BigInt));
                cmd.Parameters["@pExamID"].Value = ExamID;
                cmd.Parameters.Add(new SqlParameter("@Lvlcode", SqlDbType.VarChar));
                cmd.Parameters["@Lvlcode"].Value = levelcode;
                cmd.Parameters.Add(new SqlParameter("@View", SqlDbType.VarChar));
                cmd.Parameters["@View"].Value = viewrecord;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }

    protected void BindGridViewPreviousInstallments(Int64 ExamId, Int64 regno, string levelcode, string viewrecord)
    {
        try
        {
            using (DataTable dt = FillGridViewRegistraionNoPreviousInstallments(ExamId, regno, levelcode, viewrecord))
            {
                if (dt.Rows.Count > 0)
                {
                    Int32 VerifiedStatus = 0;
                    VerifiedStatus = Convert.ToInt32(ddlVerifiedStatus.SelectedValue);
                    string searchString = ucSearchBar.SearchText.Trim().ToUpper();

                    var ApplicationData = (from p in dt.AsEnumerable()
                                           select new
                                           {
                                               slno = p.Field<Int64>("slno"),
                                               PapersPassed = p.Field<int>("PapersPassed"),
                                               AmountReleased = p.Field<Int64>("AmountReleased") +"/-",
                                               Exams=p.Field<string>("Exams"),
                                               InstallmentsNumber=p.Field<int>("InstallmentsNumber")

                                           });
                    
                    PreviousInstallments.DataSource = ApplicationData;
                    PreviousInstallments.DataBind();
                    lblviewInstallments.Visible = false;
                    PreviousInstallments.Visible = false;
                    lblerrorPreviousInstallments.Text = "";
                    lblerrorPreviousInstallments.Visible = false;
                }
                else
                {
                    lblerrorPreviousInstallments.Text = "No record found.";
                    lblerrorPreviousInstallments.Visible = false;
                    lblviewInstallments.Visible = false;
                    PreviousInstallments.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindGridView(string ExamYearMonth)
    {
        try
        {
            lblError.Visible = false;
            string ExamMonthYearr = ExamYearMonth;
            using (DataTable dt = FillGridViewOnlinePuraskarApplicationRecordExamiIdWise(ExamMonthYearr))
            {
                if (dt.Rows.Count > 0)
                {
                    Int32 VerifiedStatus = 0;
                    VerifiedStatus = Convert.ToInt32(ddlVerifiedStatus.SelectedValue);
                    string searchString = ucSearchBar.SearchText.Trim().ToUpper();

                    var ApplicationData = (from p in dt.AsEnumerable()
                                           select new
                                           {
                                               //ID = p.Field<Int64>("ID"), string aadharDecrypted = EncryptDecrypt.DecryptString(encAdhar);
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
                                               AmountToBeReleased = p.Field<string>("AmountToBeReleased") ?? "-----", 
                                              // AmountToBeReleased = p.Field<string >("AmountToBeReleased"),
                                               VerifiedStatusFilter = p.Field<int>("VerifiedStatusFilter"),
                                               VerifiedStatus = p.Field<string>("VerifiedStatus"),
                                               financeRejectionReason = p.Field<string>("financeRejectionReason"),
                                               isWithHeldFinance = p.Field<string>("isWithHeldFinance"),
                                               withHeldReason = p.Field<string>("withHeldReason"),

                                           });
                    if (VerifiedStatus != 99)
                    {
                        ApplicationData = ApplicationData.Where(s => s.VerifiedStatusFilter == VerifiedStatus);
                    }
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        ApplicationData = ApplicationData.Where(s => s.Name.ToUpper().Contains(searchString));
                    }
                   /* PagingBar1.Bind(ApplicationData, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    lblError.Visible = false;*/
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
                        PagingBar1.Bind(ApplicationData, ref gvMain);
                        gvMain.Visible = true;
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                        divGrid.Visible = true;
                        lblError.Visible = false;
                        lblheading2.Visible = true;
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
                        lblheading2.Visible = true;
                        lblError.Text = "No record found.";
                        lblError.Visible = true;
                    }
                    lblheading2.Visible = false;
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
    protected void BindGridViewRecord(Int64 ExamId, Int64 regno)
    {
        try
        {
            using (DataTable dt = FillGridViewRegistraionNoAndExamiIdWise(ExamId, regno))
            {
                if (dt.Rows.Count > 0)
                {
                    Int32 VerifiedStatus = 0;
                    VerifiedStatus = Convert.ToInt32(ddlVerifiedStatus.SelectedValue);
                    string searchString = ucSearchBar.SearchText.Trim().ToUpper();

                    var ApplicationData = (from p in dt.AsEnumerable()
                                           select new
                                           {                                               
                                               slno = p.Field<Int64 >("slno"),
                                               modulesApp = p.Field<string>("modulesApp"),
                                               modulesPass = p.Field<string>("modulesPass")
                                              
                                           });
                   
                    Grid.DataSource = ApplicationData;
                    Grid.DataBind();
                    lblview.Visible = false;
                    Grid.Visible = false;
                    lblerrormoduleDetailsview.Text = "";
                    lblerrormoduleDetailsview.Visible = false;
                }
                else
                {
                    lblerrormoduleDetailsview.Text = "No record found.";
                    lblerrormoduleDetailsview.Visible = false;
                    lblview.Visible = false;
                    Grid.Visible = false;                   
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
        BreadCrumb1.Render();
        DetailsView1.PageIndex = e.NewPageIndex;
        this.BindGridViewRecord(8, 300335);
        BreadCrumb1.Render();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("PuraskarApplicationFormVerificationByFinance.aspx", true);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("PuraskarApplicationFormVerificationByFinance.aspx", true);
    }

    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            int RegnNo = 0;
            Int32 paperspasscount = 0;
            Int32 AmountReleasedToBe = 0, installmentsNo = 0;
            BreadCrumb1.Render();
            using (EConnectContext context = new EConnectContext())
            {
                PuraskarApplicationForm objpuraskarApplication;
                // objpuraskarApplication;
                CourseWiseAmtPerPaperForOnlinePuraskarApp AmtPerPaperForOnlinePuraskarApp;
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    objpuraskarApplication = new PuraskarApplicationForm();
                    AmtPerPaperForOnlinePuraskarApp = new CourseWiseAmtPerPaperForOnlinePuraskarApp();
                    foreach (GridViewRow row in gvMain.Rows)
                    {
                        bool isChecked = row.Cells[12].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                        if (isChecked)
                        {
                            RegnNo = Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Value);

                            //deep on 18 May 2020
                            Label lblExamid = row.Cells[13].Controls.OfType<Label>().FirstOrDefault();
                            Label Lvl = row.Cells[3].Controls.OfType<Label>().FirstOrDefault();
                             Label ExamNames = row.Cells[4].Controls.OfType<Label>().FirstOrDefault();
                            // Label Amt = row.Cells[9].Controls.OfType<Label>().FirstOrDefault();
                            Examid = Convert.ToInt64(lblExamid.Text);
                           
                            //AmountReleasedToBe = Convert.ToInt32(Amt.Text);
                            string lvl = Lvl.Text;
                            string ExamName = ExamNames.Text;
                            // check the candidate modules verification status for update the candidate status
                            OnlineProtsahanExamModules objOnlineProtsahanExamModules;
                            objOnlineProtsahanExamModules = new OnlineProtsahanExamModules();
                            var CountMoudulesUpdation = (from s in context.OnlineProtsahanExamModuless
                                                         where s.RegnNo == RegnNo && s.ExamID == Examid && s.isVerifiedByExam == true
                                                         select new { ID = s.ID, ModuleId = s.ModuleID }).Distinct();

                            if (CountMoudulesUpdation.Count() <= 0)
                            {
                                lblMessage.Text = "Please update the modules, Click on View, Then after verify the Candidates Status !";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                return;
                            }
                            // check the candidate modules verification status for update the candidate status END 

                            //deep 18 May 2020 end

                            objpuraskarApplication = context.PuraskarApplicationForms.Where(a => a.RegnNo == RegnNo && a.ExamID == Examid).FirstOrDefault();
                            // deep add with held case for Line
                            string WithHeldResolvedstatus = Convert.ToString(row.Cells[10].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem);
                            string financeWithHeldReason = row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Text;
                            if (financeWithHeldReason != "" && WithHeldResolvedstatus != "RESOLVED")
                            {
                                objpuraskarApplication.withHeldReason = row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Text;
                            }
                            var WithHeldResonExistsCount = (from s in context.PuraskarApplicationForms
                                                            where s.RegnNo == RegnNo && s.ExamID == Examid && s.withHeldReason != null
                                                            select new { ID = s.ID, WithHeldReasonExist = s.withHeldReason }).Distinct();
                            String withHeldreasons = string.Empty;
                            //string  resolvedDate = "01-01-1800";

                            if (WithHeldResonExistsCount.Count() > 0)
                            {
                                var WithHeldResonExists = (from s in context.PuraskarApplicationForms
                                                           where s.RegnNo == RegnNo && s.ExamID == Examid && s.withHeldReason != null
                                                           select s).FirstOrDefault();
                                withHeldreasons = WithHeldResonExists.withHeldReason;
                                // resolvedDate = WithHeldResonExists.withHeldResolvedDate.ToString() ?? "01-01-1900";
                            }
                            //string WithHeldResolvedstatus = Convert.ToString(row.Cells[10].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem);
                            if (WithHeldResolvedstatus == "RESOLVED")
                            {
                                objpuraskarApplication.isWithHeldFinance = false;
                                objpuraskarApplication.withHeldResolvedDate = DateTime.Now;
                                objpuraskarApplication.withHeldReason = withHeldreasons + ", " + row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Text;
                                //objpuraskarApplication.applicationStatusID = Convert.ToInt32(enmPuraskarApplicationStatus.RejectedByFinanceWing);
                            }
                            string WithHeldstatus = Convert.ToString(row.Cells[10].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem);
                            if (WithHeldstatus == "YES")
                            {
                                objpuraskarApplication.isWithHeldFinance = true;
                                objpuraskarApplication.withHeldDate = DateTime.Now;


                                //objpuraskarApplication.applicationStatusID = Convert.ToInt32(enmPuraskarApplicationStatus.RejectedByFinanceWing);
                            }
                            else if (WithHeldstatus == "NO" && withHeldreasons == "")
                            {
                                objpuraskarApplication.isWithHeldFinance = false;
                                //objpuraskarApplication.applicationStatusID = Convert.ToInt32(enmPuraskarApplicationStatus.RejectedByFinanceWing);
                            }
                            else if (WithHeldstatus == "YES" && withHeldreasons != "")
                            {
                                lblMessage.Text = "Please relsoved this case, it is withHeld case !";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                return;

                            }

                            // deep add with held case for line end

                            string financeRejectionReason = row.Cells[11].Controls.OfType<TextBox>().FirstOrDefault().Text;
                            if (financeRejectionReason != "")
                            {
                                objpuraskarApplication.financeRejectionReason = row.Cells[11].Controls.OfType<TextBox>().FirstOrDefault().Text;
                            }
                            string Financeverifystatus = Convert.ToString(row.Cells[11].Controls.OfType<DropDownList>().FirstOrDefault().SelectedItem);
                            if (Financeverifystatus == "Verified")
                            {
                                if (WithHeldstatus == "NO" || WithHeldstatus == "RESOLVED")
                                {
                                    // with held case check resoloved or not start
                                    var CheckWhCases = (from s in context.PuraskarApplicationForms
                                                        where s.RegnNo == RegnNo && s.ExamID == Examid && s.withHeldResolvedDate == null && s.isWithHeldFinance == true
                                                        select new { ID = s.ID, WithHeldReasonExist = s.withHeldReason }).Distinct();
                                    if (WithHeldstatus != "RESOLVED")
                                    {
                                        if (CheckWhCases.Count() > 0)
                                        {
                                            lblMessage.Text = "Please relsoved this case, it is withHeld case !";
                                            lblMessage.ForeColor = System.Drawing.Color.Red;
                                            return;
                                        }
                                    }
                                    // with held case check resoloved or not end
                                    string LevelCode = row.Cells[3].Controls.OfType<Label>().FirstOrDefault().Text;
                                    paperspasscount = Convert.ToInt32(objpuraskarApplication.PapersPassed);

                                    //DEEP on 19 May 2020
                                    using (DataTable dt = FillGridViewRegistraionNoPreviousInstallments(Examid, RegnNo, LevelCode, "I"))
                                    {
                                        if (dt.Rows.Count > 0)
                                        {
                                            installmentsNoExists = Convert.ToInt32(dt.Rows[0]["InstallmentsNumber"].ToString());
                                            installmentsNo = installmentsNoExists + 1;
                                        }
                                        else
                                        {
                                            installmentsNo = 1;
                                        }
                                    }

                                    // DEEP ADD ON 21 SEP 2021    (a.Start.Date >= startDate.Date && a.Start.Date <= endDate)
                                    DateTime isNullDate = System.DateTime.Now;
                                    CourseExamApplication AppFinalSubmitDate;
                                    AppFinalSubmitDate = new CourseExamApplication();
                                    // fetch final submission date of exam application of candidate
                                    AppFinalSubmitDate = context.CourseExamApplications.Where(a => a.RegistrationNumber == RegnNo && a.ExamID == Examid && a.FinalSubmitted == true).FirstOrDefault();
                                    DateTime finalSubmitDate = Convert.ToDateTime(AppFinalSubmitDate.FinalSubmissionDate);
                                    Int32 courseid = AppFinalSubmitDate.CourseID;
                                    FeeDetail FeeDetailsCourseWise;
                                    FeeDetailsCourseWise = new FeeDetail();

                                    // fetch fee amount as per application submission date and course id with exam id  fee type id 4  for examination fee
                                    var FeeAmount = (from a in context.FeeDetails
                                                     where a.CourseID == courseid && a.FeeTypeID == 4
                        && (a.EffectiveFromDate <= finalSubmitDate && (System.Data.Entity.DbFunctions.TruncateTime(finalSubmitDate) <= (a.EffectiveToDate == null ? System.Data.Entity.DbFunctions.TruncateTime(isNullDate) : a.EffectiveToDate)))
                                                     select new { FeeAmt = a.FeeAmount }).Take(1);

                                    Int32 AmountParPaper = FeeAmount.FirstOrDefault().FeeAmt;

                                    objpuraskarApplication = context.PuraskarApplicationForms.Where(a => a.RegnNo == RegnNo && a.ExamID == Examid).FirstOrDefault();
                                    Int32 moduleCountProcessedByExam = Convert.ToInt32(objpuraskarApplication.moduleCountProcessed);
                                    // protsahan puraskar 4 times of examination fee paid to cand
                                    AmountReleasedToBe = moduleCountProcessedByExam * AmountParPaper * 4;

                                    // DEEP ADD END ON 21 SEP 2021

                                    objpuraskarApplication.InstallmentsNumber = installmentsNo;
                                    objpuraskarApplication.verifiedByFinance = true;
                                    objpuraskarApplication.SentForBankTransferOn = DateTime.Now;
                                    objpuraskarApplication.AmountReleased = AmountReleasedToBe;
                                    objpuraskarApplication.applicationStatusID = Convert.ToInt32(enmPuraskarApplicationStatus.VerifiedByFinanceWingButPaymentToBeProcessed);

                                }
                                else
                                {
                                    lblMessage.Text = "Please relsoved this case, it is withHeld case !";
                                    lblMessage.ForeColor = System.Drawing.Color.Red;
                                    return;
                                }
                            }
                            else if (Financeverifystatus == "Rejected")
                            {
                                // with held case check resoloved or not start
                                var CheckWhCases = (from s in context.PuraskarApplicationForms
                                                    where s.RegnNo == RegnNo && s.ExamID == Examid && s.withHeldResolvedDate == null && s.isWithHeldFinance == true
                                                    select new { ID = s.ID, WithHeldReasonExist = s.withHeldReason }).Distinct();
                                if (WithHeldstatus != "RESOLVED")
                                {
                                    if (CheckWhCases.Count() > 0)
                                    {
                                        lblMessage.Text = "Please relsoved this case, it is withHeld case !";
                                        lblMessage.ForeColor = System.Drawing.Color.Red;
                                        return;
                                    }
                                }
                                // with held case check resoloved or not end

                                objpuraskarApplication.verifiedByFinance = false;

                                objpuraskarApplication.applicationStatusID = Convert.ToInt32(enmPuraskarApplicationStatus.RejectedByFinanceWing);
                            }
                            objpuraskarApplication.verifiedByFinanceUser = Convert.ToInt32(Session["UserID"]);
                            objpuraskarApplication.VerifiedByFinanceOn = DateTime.Now;

                            // check for withHeld case


                            context.SaveChanges();

                            if (objpuraskarApplication.verifiedByFinance == true)
                                SendEmail(RegnNo, "success", "", AmountReleasedToBe, lvl, ExamName);
                            if (objpuraskarApplication.verifiedByFinance == true)
                                SendEmail(RegnNo, "rejected", objpuraskarApplication.financeRejectionReason, AmountReleasedToBe, lvl, ExamName);

                            if (WithHeldstatus == "YES")
                            {
                                string status = "Withheld";
                                string reason = row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Text;
                                SendEmail(RegnNo, status, reason, 0, lvl, ExamName);
                            }
                            strMessage = "Record updated.";
                            string ExamMonthYear = ddlflExam.SelectedValue;
                            BindGridView(ExamMonthYear);                          
                            lblMessage.Text = "Data updated successfully!";
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                            btnSave.Visible = false;
                            btnCancel.Visible = false;
                            lblview.Visible = false;
                            Grid.Visible = false;
                            lblviewInstallments.Visible = false;
                            PreviousInstallments.Visible = false;
                        }

                        // OnlineProtsahanExamModules isprocessed column update true regNowise and examiD for complete the process for candidate verification.
                        var objProtsahanExamModulesProcessed = context.OnlineProtsahanExamModuless.Where(a => a.RegnNo == RegnNo && a.ExamID == Examid && a.isVerifiedByExam == true).ToList();
                        if (objProtsahanExamModulesProcessed.Count > 0)
                        {
                            objProtsahanExamModulesProcessed.ForEach(a => a.isProcessed = true);
                            context.SaveChanges();
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
    protected void OnCheckedChanged(object sender, EventArgs e)
    {
        bool isUpdateVisible = false;
       
       lblMessage.Text = string.Empty;
        //Loop through all rows in GridView
        foreach (GridViewRow row in gvMain.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[12].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                    row.RowState = DataControlRowState.Edit;               
                for (int i = 10; i < 12; i++)
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
                        row.Cells[11].Controls.OfType<TextBox>().FirstOrDefault().Enabled = false;
                        row.Cells[11].Controls.OfType<TextBox>().FirstOrDefault().Visible = false;
                    }

                    //
                    DropDownList ddlisWithHeldFinance = (DropDownList)gvr.FindControl("ddlisWithHeldFinance");
                    Int32 isWithHeldcandStatus = Convert.ToInt32(duty.SelectedItem.Value);
                    Label isWithHeldFinance = (Label)gvr.FindControl("Label3");
                    Label withHeldReason = (Label)gvr.FindControl("Label4");


                    TextBox txtwithHeldReasonTXT = (TextBox)gvr.FindControl("txtwithHeldReason");
                    string candstatus2 = isWithHeldFinance.Text;
                    if (ddlisWithHeldFinance.SelectedItem.Text == "YES" || ddlisWithHeldFinance.SelectedItem.Text == "RESOLVED")
                    {
                        row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Visible = isChecked;
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
                        ddlisWithHeldFinance.SelectedValue = "-1";
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
                    if (isWithHeldFinance.Text == "NO" || isWithHeldFinance.Text == "Pending")
                    {
                        row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Enabled = false;
                        row.Cells[10].Controls.OfType<TextBox>().FirstOrDefault().Visible = false;
                    }

                    //
                }
                lblview.Visible = false;
                Grid.Visible = false;
                lblviewInstallments.Visible = false;
                PreviousInstallments.Visible = false;


                String FinanceRejectionReason1 = row.Cells[11].Controls.OfType<DropDownList>().FirstOrDefault().Text;

                if (FinanceRejectionReason1 == "0")
                {
                    row.Cells[11].Controls.OfType<TextBox>().FirstOrDefault().Enabled = true;
                    row.Cells[11].Controls.OfType<TextBox>().FirstOrDefault().Visible = true;
                }    
               String FinanceRejectionReason = row.Cells[11].Controls.OfType<TextBox>().FirstOrDefault().Text;

               if (FinanceRejectionReason != "")
                {
                    row.Cells[11].Controls.OfType<TextBox>().FirstOrDefault().Enabled = false;
                    row.Cells[11].Controls.OfType<TextBox>().FirstOrDefault().Visible = false;
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
            //BindGridView();
            //PagingBar1.CurrentPageIndex = 0;
            //gvMain.PageIndex = PagingBar1.CurrentPageIndex;            
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
            //PagingBar1.CurrentPageIndex = 0;
            //gvMain.PageIndex = PagingBar1.CurrentPageIndex;

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
            string ExamMonthYear = ddlflExam.SelectedValue;
            BindGridView(ExamMonthYear);
            divGrid.Visible = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
   
    protected void ModuleViewByRegno()
    {
        try
        {
           // BindGridView();
            string ExamMonthYear = ddlflExam.SelectedValue;
            BindGridView(ExamMonthYear);
            divGrid.Visible = true;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;

            using (EConnectContext context = new EConnectContext())
            {
                Int64 ExamId = 0;
                ExamId = Convert.ToInt64(Request.QueryString["ID"]);
                Int64 RegistrationNo = Convert.ToInt64(Request.QueryString["Regno"]);
                BindGridViewRecord(ExamId, RegistrationNo);             
            };            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
       
        GridViewRow row = gvMain.SelectedRow;
        //Find label id from gridview cell data
        Label lblRegno = row.FindControl("lblRegno") as Label;
        Label lblExamid = row.FindControl("lblExamid") as Label;
        Label lblLevelCode = row.FindControl("lblcode") as Label;
        string levelcode = lblLevelCode.Text;
        Int64 Examid = 0;
        Examid = Convert.ToInt64(lblExamid.Text);
        Int64 RegistrationNo = Convert.ToInt64(lblRegno.Text);
       // Response.Redirect("~/admin/PuraskarAppRegNoPreviousPapersAndInstallmentsDetails.aspx?Examid=" + Examid.ToString() + "&RegistrationNo=" + RegistrationNo.ToString() + "&levelcode=" + levelcode.ToString() + "&V=" + "V", false);
        string ViewFor = "F";
        BindGridViewRecord(Examid, RegistrationNo);
        BindGridViewPreviousInstallments(Examid, RegistrationNo, levelcode, "V");

        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('PuraskarAppModulesVerificationByExam.aspx?Examid=" + Examid + "&RegistrationNo=" + RegistrationNo + "&VF=" + ViewFor + "' ,'_blank');", true);
        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('PuraskarAppModulesVerificationByExam.aspx?Examid=" + Examid + "&RegistrationNo=" + RegistrationNo + "&VF=" + ViewFor + "' ,'_blank');", true);
        string url = "PuraskarAppModulesVerificationByExam.aspx?Examid=" + Examid + "&RegistrationNo=" + RegistrationNo + "&VF=" + ViewFor;
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
            string ExamMonthYear = ddlflExam.SelectedValue;
            BindGridView(ExamMonthYear);
            divGrid.Visible = true;
            //gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            //BindGridView();
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
    protected void SendEmail(Int64 RegnNo, string vRemarks, string reason,Int64 Amount, string Level, string ExamName)
    {
        string subject = "";
        String EmailMsg = "";
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //var contactreg = (from s in context.RegistrationDetails
                //                  join p in context.CourseRegistrationApplications on s.CandidateID equals p.CandidateID
                //                  join q in context.Exams on p.ID equals q.ExaminationCycleID
                //                  join t in context.Courses on q.CourseID equals t.ID
                //                  where s.RegistrationNo == RegnNo && t.ID == s.CourseID
                //                  && t.ID == p.CourseID
                //                  select new { EmailID = p.EmailAddress, Name = p.Name, EName = q.Name, level = t.Name }).FirstOrDefault();

                var contactreg = (from s in context.RegistrationDetails
                                  join p in context.CourseRegistrationApplications on s.CandidateID equals p.CandidateID
                                 join c in context.CandidateContactDetails on p.CandidateID equals c.CandidateID
                                  where s.RegistrationNo == RegnNo
                                  select new { EmailID = p.EmailAddress, Name = p.Name, EName = ExamName, level = Level }).FirstOrDefault();

                if (contactreg.EmailID != null)
                {
                    if (vRemarks.Trim().ToLower() == "withheld")
                    {

                        EmailMsg = "Dear " + contactreg.Name + ",<br/> " + "This is with reference to your application for award of Scholarship "
                           + " under NIELIT Protsahan Puraskar Scheme for SC/ST/Physically Handicapped/Female students based on your result for "
                       + contactreg.EName + " Examination of " + contactreg.level + " pursuing O/A/B/C level courses through institutes authorized to conduct NIELIT courses. <br/>"
                       + " 2. On scrutiny of the application, following discrepancy/ies has been found <br/> Reason: "
                       + reason + " <br/> 3. Please donot attach affidavit of income certificate issued from the court,"
                       + " it is not considered as income certificate as per the norms of NIELIT Protsahan Puraskar Scheme. <br/>"
                       + "4. Kindly provide the self attested copies of the required documents for further processing of Protsahan Puraskar. <br/>NIELIT";

                        EmailMsg = EmailMsg.Replace("\r\n", "<br/>");
                        subject = "Protsahan Puraskar for SC/ST/Physically Handicapped/Female students pursuing O/A/B/C level courses";
                    }
                    if (vRemarks.Trim().ToLower() == "rejected")
                    {

                        EmailMsg = "Dear " + contactreg.Name + ",<br/> " + "This is with reference to your application for award of Scholarship "
                           + " under NIELIT Protsahan Puraskar Scheme for SC/ST/Physically Handicapped/Female students based on your result for "
                       + contactreg.EName + " Examination of " + contactreg.level + " pursuing O/A/B/C level courses through institutes authorized to conduct NIELIT courses. <br/>"
                       + " 2. On scrutiny of the application, following discrepancy/ies has been found <br/> Reason: "
                       + reason + ". <br/> ";
                        EmailMsg = EmailMsg.Replace("\r\n", "<br/>");
                        subject = "Protsahan Puraskar for SC/ST/Physically Handicapped/Female students pursuing O/A/B/C level courses";
                    }

                    if (vRemarks.Trim().ToLower() == "success")
                    {

                        EmailMsg = "Dear " + contactreg.Name + ",<br/> " + "This is with reference to your application for award of Scholarship "
                           + " under NIELIT Protsahan Puraskar Scheme for SC/ST/Physically Handicapped/Female students based on your result for "
                       + contactreg.EName + " Examination of " + contactreg.level + " pursuing O/A/B/C level coursesthrough institutes authorized to conduct NIELIT courses. <br/>"
                       + " 2. You have been found to be eligible for Scholarship as per Rules. The amount of Rs." + Amount.ToString().Trim() + " is being processed through DBT process through Bank. <br/> NIELIT";

                        EmailMsg = EmailMsg.Replace("\r\n", "<br/>");
                        subject = "Protsahan Puraskar for SC/ST/Physically Handicapped/Female students pursuing O/A/B/C level courses";
                    }

                }
                else
                {
                    ShowAlert("Email not available");
                    return;
                }
                //sending Email
                if (contactreg.EmailID.Length > 0)
                {
                    try
                    {
                        string MailTo = contactreg.EmailID + "; himanish@nielit.gov.in; chaman@nielit.gov.in";
                        EConnect.NIELIT.Email mail = new Email(subject, EmailMsg, MailTo);
                        mail.Send();
                    }
                    catch { ShowAlert(" E-mail not sent"); }
                }
            }

        }

        //catch (Exception ex)
        //{
        //    throw ex;
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
}