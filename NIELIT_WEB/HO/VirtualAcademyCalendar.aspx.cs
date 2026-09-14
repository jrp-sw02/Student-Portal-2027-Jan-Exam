using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Common;
using EConnect.NIELIT;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

public partial class HO_VirtualAcademyCalendar : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int64 NielitCentrelinkedToCentreId = 0;
    Int32 NielitCentreIdFilter = 0, NonAfflAfflInstID = 0;
    Int32 UserTypeId = 0;

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
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);

            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {

                FillCourseCategory();
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    BindEditNewModeData();
                    ShowEditMode();
                }
                else
                {
                    // BindListData();
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BindGridView();
                    bindLanguage();
                    ddlLanguage.SelectedValue = "1";
                    PopulateHoursFrom();
                    PopulateMinutesFrom();
                    PopulateHoursTo();
                    PopulateMinutesTo();
                    if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Virtual Academy Calendar", "HO/VirtualAcademyCalendar.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Virtual Academy Calendar", "HO/VirtualAcademyCalendar.aspx", ""));
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

    protected void BindEditNewModeData()
    {
        try
        {

            FillCourseCategory();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void PopulateHoursFrom()
    {
        for (int i = 0; i <= 23; i++)
        {
            ddlFromHours.Items.Add(i.ToString("D2"));
        }
    }
    private void PopulateMinutesFrom()
    {
        for (int i = 0; i <= 59; i++)
        {
            ddlFromMinutes.Items.Add(i.ToString("D2"));
        }
    }

    private void PopulateHoursTo()
    {
        for (int i = 0; i <= 23; i++)
        {
            ddlToHours.Items.Add(i.ToString("D2"));
        }
    }
    private void PopulateMinutesTo()
    {
        for (int i = 0; i <= 59; i++)
        {
            ddlToMinutes.Items.Add(i.ToString("D2"));
        }
    }


    public DataTable GetCourseNielitCourseRecord()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetNIELITMISCourseNielitCourseRecord", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    protected void bindLanguage()
    {
        try
        {
            using (var context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var language = from p in context.teachingLanguage
                               select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlLanguage, language, lst);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSecLanguage , language, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected bool IsValidForm()
    {
        try
        {
            if (ddlNSQFAligned.SelectedValue.ToString() == "99")
            {
                return false;
            }
            if ((ddlAdmissionStatus.SelectedValue.ToString() == "0"))
                return false;
            if (ddlcoursecategory.SelectedValue.ToString() == "0")
                return false;
            if (ddlCourse.SelectedValue.ToString() == "0")
                return false;

            if (ddlNSQFAligned.SelectedValue.ToString() == "1")
                if (txtNSQFLevel.Text.Trim().Length == 0)
                    return false;

            if (Convert.ToDateTime(txtstartDate.Text) < System.DateTime.Today && String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                ShowAlert("Calendar for old dates not allowed");
                return false;
            }

            if ((Convert.ToDateTime(txtendDate.Text) < System.DateTime.Today) && !String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                ShowAlert("Calendar for old dates or Admission Closed, Cannot modify");
                return false;
            }

            if (Convert.ToDateTime(txtstartDate.Text) > System.DateTime.Today.AddMonths(12))
            {
                ShowAlert("Only upto twelve months calendar");
                return false;
            }

            if (!Char.IsLetter(txtName.Text, txtName.Text.Length - 1))
            {
                ShowAlert("Coordinator Name should end with an alphabet.");             
                return false;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtName.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
            {    
                ShowAlert("Coordinator Name should be with an English Alphabets(e.g - a-zA-Z)");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ShowEditMode()
    {
        try
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Virtual Academy Calendar";

            EConnectContext context1 = new EConnectContext();
            User objUser = new EConnect.URM.User();
            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int64 Id = Convert.ToInt32(Request.QueryString["Key"]);
                var TrgCal = (from p in context.virtualAcademyCalendar
                              where p.ID == Id
                              //  && p.admissionStatus =="O"
                              select p).Count();

                if (TrgCal > 0)
                {
                    ddlCourse.Enabled = false;
                    ddlbatch.Enabled = false;
                    ddlAdmissionStatus.Enabled = true;
                    ddlNSQFAligned.Enabled = true;
                    txtLevelParticipant.Enabled = true;
                    txtstartDate.Enabled = true;
                    txtendDate.Enabled = true;
                    txtDurationMonths.Enabled = false;
                    txtDurationHours.Enabled = false;
                    txtName.Enabled = true;
                    txtEmail.Enabled = true;
                    bindLanguage();
                    PopulateHoursFrom();
                    PopulateMinutesFrom();
                    PopulateHoursTo();
                    PopulateMinutesTo();
                    var centreTrgCal = (from p in context.virtualAcademyCalendar
                                        where p.ID == Id
                                        select p).FirstOrDefault();


                    ddlcoursecategory.SelectedValue = centreTrgCal.courseCategoryID.ToString();
                    FillCourse(Convert.ToInt32(ddlcoursecategory.SelectedValue));
                    ddlCourse.SelectedValue = centreTrgCal.courseId.ToString();
                    ddlCourse_SelectedIndexChanged(ddlCourse, EventArgs.Empty);
                    ddlbatch.SelectedValue = centreTrgCal.batchId.ToString();
                    ddlbatch_SelectedIndexChanged(ddlbatch, EventArgs.Empty);

                    if (centreTrgCal.whetherNSQFAligned)
                        ddlNSQFAligned.SelectedValue = "1";
                    else
                        ddlNSQFAligned.SelectedValue = "0";


                    if (centreTrgCal.NSQFLevel != null)
                        txtNSQFLevel.Text = centreTrgCal.NSQFLevel.ToString();

                    txtLevelParticipant.Text = centreTrgCal.participantEligibility;

                   // txtstartDate.Text = centreTrgCal.startDate.ToString("dd-MMM-yyyy");
                   // txtendDate.Text = centreTrgCal.endDate.ToString("dd-MMM-yyyy");

                    if (centreTrgCal.durationHrs != null)
                        txtDurationHours.Text = centreTrgCal.durationHrs.ToString();
                    if (centreTrgCal.durationMths != null)
                        txtDurationMonths.Text = centreTrgCal.durationMths.ToString();

                    ddlAdmissionStatus.SelectedValue = centreTrgCal.admissionStatus;

                    txtName.Text = centreTrgCal.courseInchargeName;
                    txtDesignation.Text = centreTrgCal.courseInchargeDesig;
                    txtSTDCode.Text = centreTrgCal.courseInchargeSTDCode;
                    txtPhone1.Text = centreTrgCal.courseInchargePhone1;
                    txtPhone2.Text = centreTrgCal.courseInchargePhone2;
                    txtMobile.Text = centreTrgCal.courseInchargeMobile;
                    txtEmail.Text = centreTrgCal.courseInchargeEmail.ToString();

                    ddlLanguage.SelectedValue = centreTrgCal.teachingLanguage.ToString();
                   ddlSecLanguage.SelectedValue = centreTrgCal.secTeachingLanguage.ToString();

                    var strFrom = centreTrgCal.TimingsFrom.ToString();
                    var hoursFrom = strFrom.Split('-')[0].Split(':')[0];
                    var minutesFrom = strFrom.Split('-')[0].Split(':')[1];
                    ddlFromHours.SelectedValue = hoursFrom.ToString();
                    ddlFromMinutes.SelectedValue = minutesFrom.ToString();

                    var strTo = centreTrgCal.TimingsTo.ToString();
                    var hoursTo = strTo.Split('-')[0].Split(':')[0];
                    var minutesTo = strTo.Split('-')[0].Split(':')[1];
                    ddlToHours.SelectedValue = hoursTo.ToString();
                    ddlToMinutes.SelectedValue = minutesTo.ToString();
                    btnCancel.Visible = false;
                    btnSave.Visible = true;
                    btnBack.Visible = true;

                    if (Convert.ToDateTime(txtendDate.Text) < System.DateTime.Today)
                    {
                        ShowAlert("Cannot Update, Course is over");
                        btnSave.Visible = false;
                    }
                    if (ddlAdmissionStatus.SelectedValue == "C")
                    {
                        ShowAlert("This Course Admission is Already Closed, Cannot Alter details in calendar now");
                        btnSave.Visible = false;
                    }
                }


            };

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            // context.Dispose();
        }
    }

    //public DataTable FillGridView(Int64 pCentreId, DateTime pStartDateFrom, DateTime pStartDateTo, string pAdmissionStatus)
    public DataTable FillGridView(Int64 pCentreId, DateTime pStartDateFrom, DateTime pStartDateTo)
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("getVirtualCalRecords", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    cmd.Parameters.Add("@pCentreId", SqlDbType.BigInt);
                    cmd.Parameters["@pCentreId"].Value = pCentreId;

                    cmd.Parameters.Add("@pStartDateFrom", SqlDbType.DateTime);
                    cmd.Parameters["@pStartDateFrom"].Value = pStartDateFrom;

                    cmd.Parameters.Add("@pStartDateTo", SqlDbType.DateTime);
                    cmd.Parameters["@pStartDateTo"].Value = pStartDateTo;

                    //cmd.Parameters.Add("@pAdmissionStatus", SqlDbType.VarChar, 1);
                    //cmd.Parameters["@pAdmissionStatus"].Value = pAdmissionStatus;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    protected void BindGridView()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                DateTime startDateF, endDateF;
                string vAdmissionStatus;

                startDateF = System.DateTime.Now.AddMonths(2);
                endDateF = System.DateTime.Now.AddMonths(2);

                if (txtStartDatefilter.Text.Trim().Length != 0)
                    startDateF = Convert.ToDateTime(txtStartDatefilter.Text);
                if (txtStartDateToFilter.Text.Trim().Length != 0)
                    endDateF = Convert.ToDateTime(txtStartDateToFilter.Text);
                vAdmissionStatus = ddlAdmissionStatusFilter.SelectedValue.ToString().Trim();


                int loginCentre = Convert.ToInt32(entityID);
                //int loginCentre = 5007;

                var trgCal = from a in context.virtualAcademyCalendar
                             where a.startDate >= startDateF
                             && a.startDate <= endDateF
                             select a;

                //using (DataTable dt = FillGridView(loginCentre, startDateF, endDateF, vAdmissionStatus))
                using (DataTable dt = FillGridView(loginCentre, startDateF, endDateF))
                {
                    if (dt.Rows.Count > 0)
                    {
                        var VirtualAcdCal = (from p in dt.AsEnumerable()
                                             select new
                                             {
                                                 ID = p.Field<Int64>("ID"),
                                                 CourseName = p.Field<string>("CourseName"),
                                                 AdmissionStatus = p.Field<string>("AdmissionStatus"),
                                                 startDate = p.Field<DateTime>("startDate"),
                                                 endDate = p.Field<DateTime>("endDate"),
                                                 CourseID = p.Field<Int64>("CourseID"),
                                                 durationHrs = p.Field<string>("duration1"),
                                                 mobEmail = p.Field<string>("mobEmail"),
                                                 courseInchargeName = p.Field<string>("courseInchargeName"),

                                             });

                        PagingBar1.Bind(VirtualAcdCal, ref gvMain);
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                    }
                }
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

    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            //    if (!UserManager.HasRight(currentRoleId, enmRight.New))
            //    {
            //        BreadCrumb1.Render();
            //        ShowAlert("Sorry! You don't have rights to add new record.", true);
            //        return;
            //    }
            BindEditNewModeData();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;

            lblHeading.Text = "Virtual Academy Calendar";
            BreadCrumb1.Render();
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("VirtualAcademyCalendar.aspx?ID=" + Request.QueryString["CourseID"].ToString()), true);
            }
            else
            {
                Response.Redirect("VirtualAcademyCalendar.aspx", true);
            }
        }
    }

    protected void SearchBar_ApplySearch(object sender, EventArgs e)
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblerrorddlcouse.Text = "";
            if (IsValidForm())
            {
                BreadCrumb1.Render();
                EConnectContext context1 = new EConnectContext();
                User objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                //Int32 NielitCentreId = 5007;
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    virtualAcademyCalendar virtualAcdCal;

                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        //Added to keep batchid unique while adding record
                        Int64 batchId=Convert.ToInt64(ddlbatch.SelectedValue);
                        var existBatch = (from a in context.virtualAcademyCalendar
                                          where a.batchId == batchId
                                          select a).FirstOrDefault();
                        if (existBatch != null)
                        {
                            ShowAlert("Batch is already in calendar");
                            return;
                        }



                        virtualAcdCal = new virtualAcademyCalendar();

                        virtualAcdCal.CentreId = NielitCentreId;
                        HNANFL.Value = "O";

                        virtualAcdCal.courseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                        virtualAcdCal.courseId = Convert.ToInt32(ddlCourse.SelectedValue);
                        virtualAcdCal.batchId = Convert.ToInt64(ddlbatch.SelectedValue);
                        virtualAcdCal.admissionStatus = ddlAdmissionStatus.SelectedValue.ToString();
                        virtualAcdCal.durationHrs = Convert.ToInt32(txtDurationHours.Text);
                        virtualAcdCal.durationMths = Convert.ToInt32(txtDurationMonths.Text);

                        virtualAcdCal.startDate = Convert.ToDateTime(txtstartDate.Text);
                        virtualAcdCal.endDate = Convert.ToDateTime(txtendDate.Text);

                        virtualAcdCal.participantEligibility = txtLevelParticipant.Text;

                        if (ddlNSQFAligned.SelectedValue.ToString() == "N")
                            virtualAcdCal.whetherNSQFAligned = false;
                        else
                            virtualAcdCal.whetherNSQFAligned = true;
                        if (ddlNSQFAligned.SelectedValue.ToString() == "1")
                            virtualAcdCal.NSQFLevel = Convert.ToInt32(txtNSQFLevel.Text);

                        //Course Cordinator details
                        virtualAcdCal.courseInchargeName = txtName.Text;
                        virtualAcdCal.courseInchargeDesig = txtDesignation.Text;
                        virtualAcdCal.courseInchargePhone1 = txtPhone1.Text;
                        virtualAcdCal.courseInchargePhone2 = txtPhone2.Text;
                        virtualAcdCal.courseInchargeSTDCode = txtSTDCode.Text;
                        virtualAcdCal.courseInchargeMobile = txtMobile.Text;
                        virtualAcdCal.courseInchargeEmail = txtEmail.Text;
                        virtualAcdCal.teachingLanguage = Convert.ToInt64(ddlLanguage.SelectedValue);
                        virtualAcdCal.secTeachingLanguage = Convert.ToInt64(ddlSecLanguage.SelectedValue);   
                        virtualAcdCal.TimingsFrom = ddlFromHours.SelectedValue + ":" + ddlFromMinutes.SelectedValue.Trim(); 
                        virtualAcdCal.TimingsTo = ddlToHours.SelectedValue + ":" + ddlToMinutes.SelectedValue.Trim();
                        if (String.Compare(virtualAcdCal.TimingsFrom, virtualAcdCal.TimingsTo) > 0)
                        {

                            throw new Exception("Timings From is less than from Timings To");
                        }
                        virtualAcdCal.enterDate = DateTime.Now;
                        virtualAcdCal.enterByID = loginUserNo;

                        context.virtualAcademyCalendar.Add(virtualAcdCal);
                        context.SaveChanges();
                        strMessage = "New record saved.";

                    }
                    else
                    {
                        virtualAcdCal = context.virtualAcademyCalendar.Find(Convert.ToInt32(Request.QueryString["key"]));



                        virtualAcdCal.admissionStatus = ddlAdmissionStatus.SelectedValue.ToString();
                        virtualAcdCal.durationHrs = Convert.ToInt32(txtDurationHours.Text);
                        virtualAcdCal.durationMths = Convert.ToInt32(txtDurationMonths.Text);

                        virtualAcdCal.startDate = Convert.ToDateTime(txtstartDate.Text);
                        virtualAcdCal.endDate = Convert.ToDateTime(txtendDate.Text);

                        virtualAcdCal.participantEligibility = txtLevelParticipant.Text;

                        if (ddlNSQFAligned.SelectedValue.ToString() == "N")
                            virtualAcdCal.whetherNSQFAligned = false;
                        else
                            virtualAcdCal.whetherNSQFAligned = true;
                        if (ddlNSQFAligned.SelectedValue.ToString() == "1")
                            virtualAcdCal.NSQFLevel = Convert.ToInt32(txtNSQFLevel.Text);

                        //Course Cordinator details
                        virtualAcdCal.courseInchargeName = txtName.Text;
                        virtualAcdCal.courseInchargeDesig = txtDesignation.Text;
                        virtualAcdCal.courseInchargePhone1 = txtPhone1.Text;
                        virtualAcdCal.courseInchargePhone2 = txtPhone2.Text;
                        virtualAcdCal.courseInchargeSTDCode = txtSTDCode.Text;
                        virtualAcdCal.courseInchargeMobile = txtMobile.Text;
                        virtualAcdCal.courseInchargeEmail = txtEmail.Text;
                        virtualAcdCal.teachingLanguage = Convert.ToInt64(ddlLanguage.SelectedValue);
                       virtualAcdCal.secTeachingLanguage = Convert.ToInt64(ddlSecLanguage.SelectedValue);  
                        virtualAcdCal.TimingsFrom = ddlFromHours.SelectedValue + ":" + ddlFromMinutes.SelectedValue.Trim(); 
                        virtualAcdCal.TimingsTo = ddlToHours.SelectedValue + ":" + ddlToMinutes.SelectedValue.Trim();

                        if (String.Compare(virtualAcdCal.TimingsFrom, virtualAcdCal.TimingsTo) > 0)
                        {

                            throw new Exception("Timings From is less than from Timings To");
                        }
                        strMessage = "Record updated.";
                        context.SaveChanges();
                    }
                }
                Response.Redirect("VirtualAcademyCalendar.aspx?msg=" + strMessage, true);
            }

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

    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            txtStartDatefilter.Text = "";
            txtStartDateToFilter.Text = "";
            ddlAdmissionStatusFilter.SelectedValue = "0";
            //         ddlbatchname.SelectedValue = "0";
            //       ddlBatchVerifiedStatus.SelectedValue = "99";
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
            //     BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void PerformPopupAction(object sender, EventArgs e)
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to delete the records.", true);
                    return;
                }
                NielitCentreBatch Batchcenter = context.NielitCentreBatchs.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                context.NielitCentreBatchs.Remove(Batchcenter);
                context.SaveChanges();
                //       BindGridView();
                ShowAlert("Record deleted successfully.", true);
                hfActionID.Value = "";
            };
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            // BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be deleted!", true);
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
                if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                {
                    href += "&CourseId=" + Request.QueryString["CourseId"].ToString();
                }
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = hl.NavigateUrl;
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = hl.NavigateUrl;
                HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                h4.NavigateUrl = hl.NavigateUrl;

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("VirtualAcademyCalendar.aspx", true);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("VirtualAcademyCalendar.aspx", true);
    }

    protected void lbdisable_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to edit the records.", true);
                    return;
                }
                ExamCenter examcenter = context.ExamCenters.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                if (examcenter.IsEnabled == true)
                    examcenter.IsEnabled = false;
                else
                    examcenter.IsEnabled = true;
                context.Entry(examcenter).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                // BindGridView();
                ShowAlert("You have successfully changed the status of the Exam Center.", true);
                hfActionID.Value = "";
            };
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            // BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be edited!", true);
        }
    }

    protected void ddlAdmissionStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCourse(Convert.ToInt32(ddlcoursecategory.SelectedValue));
    }

    protected void FillCourseCategory()
    {
        try
        {
            using (DataTable dt = FillCourseCategoryRecords())
            {
                if (dt.Rows.Count > 0)
                {
                    ddlcoursecategory.DataSource = dt;
                    ddlcoursecategory.DataTextField = "Name";
                    ddlcoursecategory.DataValueField = "ID";
                    ddlcoursecategory.DataBind();
                    ddlcoursecategory.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable FillCourseCategoryRecords()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetCourseCategoryNIELITMISCourseCatNielitCourseCatRecord", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    protected void FillCourse(int pCourseCategory)
    {
        try
        {
            using (DataTable dt = FillCourseRecords(pCourseCategory))
            {
                if (dt.Rows.Count > 0)
                {
                    ddlCourse.DataSource = dt;
                    ddlCourse.DataTextField = "Name";
                    ddlCourse.DataValueField = "ID";
                    ddlCourse.DataBind();
                    ddlCourse.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
                BreadCrumb1.Render();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable FillCourseRecords(int pCourseCategory)
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("VAF_getNIELITCoursesAll", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.Add("@pCourseCat", SqlDbType.BigInt);
                    cmd.Parameters["@pCourseCat"].Value = pCourseCategory;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    protected void ddlNSQFAligned_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlNSQFAligned.SelectedValue.ToString() == "1")
        { //txtNSQFLevel.Enabled = true;
            //Added for NSQF level / Duration
            DataTable dt = getNSQFLevelHours(Convert.ToInt32(ddlCourse.SelectedValue));
            if (dt.Rows.Count > 0)
            {
                txtNSQFLevel.Text = dt.Rows[0]["Level"].ToString();
                txtDurationHours.Text = dt.Rows[0]["Duration"].ToString();
                txtDurationHours.Visible = true;
                txtNSQFLevel.Enabled = false;
                txtDurationHours.Enabled = false;
            }
            else
            {
                txtNSQFLevel.Text = "";
                txtDurationHours.Text = "";
                txtDurationHours.Visible = true;
                txtNSQFLevel.Enabled = true;
                txtDurationHours.Enabled = true;
            }
        }



        else
        {
            txtNSQFLevel.Text = "";
            txtNSQFLevel.Enabled = false;

            txtDurationHours.Enabled = true;
        }
    }

    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlNSQFAligned.SelectedValue.ToString() == "1")
        {
            ddlNSQFAligned_SelectedIndexChanged(sender, e);
        }

        else
        {
            txtNSQFLevel.Text = "";
            txtNSQFLevel.Enabled = false;
            txtDurationHours.Text = "";
            txtDurationHours.Enabled = true;
        }

        //        Int64 subcentreid = 0;
        Int64 Courseid = 0;

        ddlbatch.ClearSelection();

        ddlbatch.Items.Clear();

        try
        {
            //subcentreid = Convert.ToInt64(ddlCenter.SelectedValue);
            Courseid = Convert.ToInt64(ddlCourse.SelectedValue);

            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Batch = from s in context.NielitCentreBatchs
                            where s.IsVerified == true // && s.centreID == subcentreid && s.subCentreID == 0
                                    && s.CourseDurationID == Courseid && s.learningModeID == 7
                            orderby (s.Name)
                            select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatch, Batch, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        Int64 Batchid = 0;

        //ddlbatch.ClearSelection();

        //ddlbatch.Items.Clear();

        try
        {
            //subcentreid = Convert.ToInt64(ddlCenter.SelectedValue);
            Batchid = Convert.ToInt64(ddlbatch.SelectedValue);
            txtstartDate.Text = "";
            txtendDate.Text = "";
            txtDurationHours.Text = "";
            txtDurationHours.Enabled = false;
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Batch = (from s in context.NielitCentreBatchs
                            where s.IsVerified == true 
                                    && s.ID == Batchid                            
                            select new
                         {
                             ID = s.ID,
                             StartDate = s.startDate,
                             EndDate = s.endDate,
                             CourseDurationId = s.CourseDurationID                            
                         }).FirstOrDefault();

                txtstartDate.Text = Batch.StartDate.ToString("dd-MMM-yyyy");
                txtendDate.Text = Batch.EndDate.ToString("dd-MMM-yyyy");

                Int64 coursesdurationid = 0;

                coursesdurationid = Batch.CourseDurationId;

             //  var CourseDurationID = (from s in context.NielitCourseDurations
                     //                   where s.ID == coursesdurationid
                     //        select new
                     //        {
                     //            ID = s.ID,
                      //           courseDurationHrs = s.courseDurationHrs,
                     //            courseDurationDays = s.courseDurationDays                                
                       //      }).FirstOrDefault();
              //  txtDurationHours.Text = CourseDurationID.courseDurationHrs.ToString();
//deep add code on 25 may 2022 and comment above CourseDurationID var
                Int32 courseid = Convert.ToInt32(ddlCourse.SelectedValue);
             Int32 CourseType = courseid.ToString().Length;
             if (CourseType > 3)
             {
                 var CourseDurationID = (from s in context.NielitCourseDurations
                                         where s.ID == coursesdurationid
                                         select new
                                         {
                                             ID = s.ID,
                                             courseDurationHrs = s.courseDurationHrs,
                                             courseDurationDays = s.courseDurationDays
                                         }).FirstOrDefault();
                 txtDurationHours.Text = CourseDurationID.courseDurationHrs.ToString();
                 txtDurationHours.Enabled = false;
             }
             else if (CourseType <= 3)
             {
                 txtDurationHours.Text = "";
                 txtDurationHours.Enabled = true;
             }

                // deep add end code on 25 may 2022
                //Double totalDays = Convert.ToDouble( CourseDurationID.courseDurationDays);
                //const double daysToMonths = 30.4368499;             

               // var totalMonths = Math.Truncate((totalDays % 365) / 30);
               // txtDurationMonths.Text = Convert.ToInt32(Math.Floor(totalDays / daysToMonths)).ToString();

            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable getNSQFLevelHours(int pCourseId)
    {

        string constr = ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("getNSQFLevelHours", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.Add("@pCourseId", SqlDbType.BigInt);
                    cmd.Parameters["@pCourseId"].Value = pCourseId;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }
}