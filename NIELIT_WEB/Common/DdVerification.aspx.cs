using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Transactions;
using System.Text.RegularExpressions;

public partial class DdVerification : BasePage
{
    String strMessage = string.Empty;
    //EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;
    //Boolean isListModeAfterEditMode = false;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
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
            lblError.Text = "";
            lblError.Visible = false;
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {

                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    btnMode.Visible = true;
                    ShowEditMode();
                }
                else
                {
                    btnMode.Visible = false;
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    PopulateDataListMode();

                    if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                    {
                        using (EConnectContext context = new EConnectContext())
                        {
                            var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                            //courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                            if (roleCourses.Count() == 1)
                            {
                                ddlCategory.SelectedValue = roleCourses.FirstOrDefault().ToString();
                                ddlCategory.Enabled = false;
                            }
                        };

                    }
                    else
                    {
                        if (loginUserType == UserType.RegionalCenter)
                            ddlCategory.SelectedValue = "2";

                        else if (loginUserType == UserType.HeadOffice)
                            ddlCategory.SelectedValue = "1";

                        ddlCategory.Enabled = false;

                    }
                    PupulateCourses(Convert.ToInt32(ddlCategory.SelectedValue), ddlCourse, new ListItem("--Select One--", "0"));

                    if (!String.IsNullOrEmpty(Request.QueryString["courseID"]) && !String.IsNullOrEmpty(Request.QueryString["ApplTypeId"]) && !String.IsNullOrEmpty(Request.QueryString["DemandNoteTypeId"]))
                    {
                        ddlCourse.SelectedValue = Request.QueryString["courseID"].ToString();
                        FillApplicationTypes(Convert.ToInt32(ddlCourse.SelectedValue), ddlApplicationType, new ListItem("--Select One--", "0"));
                        ddlApplicationType.SelectedValue = Request.QueryString["ApplTypeId"].ToString();
                        ddlDemandNoteType.SelectedValue = Request.QueryString["DemandNoteTypeId"].ToString();
                        BindGridView();
                    }

                    string courseID = ddlCourse.SelectedValue;
                    string ApplTypeId = ddlApplicationType.SelectedValue;
                    string DemandNoteTypeId = ddlDemandNoteType.SelectedValue;
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("DD Verification", "Common/DdVerification.aspx?courseID=" + courseID + "&ApplTypeId=" + ApplTypeId + "&DemandNoteTypeId=" + DemandNoteTypeId, ""));

                }
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    public class result
    {
        public Int64 DemandNoteNo { get; set; }
        public Int64 ID { get; set; }
        public DateTime DemandNoteDate { get; set; }
        public Int32 FeeName { get; set; }
        public String PayeeName { get; set; }
        public Int32 DemandNoteType { get; set; }
        public DateTime DDdate { get; set; }
        public String DDnumber { get; set; }
        public Decimal DDamount { get; set; }
        public Int32 courseID { get; set; }
    }
    protected void ShowEditMode()
    {

        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int64 id = Convert.ToInt32(Request.QueryString["Key"]);
                BatchItem obj = context.BatchItems.Find(id);
                btnMode.Visible = true;
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                lblHeading.Text = "DD Verification";
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn; 
                string courseID = Request.QueryString["courseID"].ToString();
                string ApplTypeId = Request.QueryString["ApplTypeId"].ToString();
                string DemandNoteTypeId = Request.QueryString["DemandNoteTypeId"].ToString();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(obj.Batch.Number, "Common/DdVerification.aspx?Key=" + Request.QueryString["Key"].ToString() + "&courseID=" + courseID + "&ApplTypeId=" + ApplTypeId + "&DemandNoteTypeId=" + DemandNoteTypeId, ""));
                candidateDetail.BatchItemID = Convert.ToInt64(Request.QueryString["Key"]);
                candidateDetail.ApplicationTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
                candidateDetail.Bind();
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        try
        {
            if (btnMode.ViewMode == ToggleView.Mode.List)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int64 id = Convert.ToInt32(Request.QueryString["Key"]);
                    string courseID = Request.QueryString["courseID"];
                    string ApplTypeId = Request.QueryString["ApplTypeId"];
                    string DemandNoteTypeId = Request.QueryString["DemandNoteTypeId"];
                    mltvTab.ActiveViewIndex = 0;
                    btnMode.Visible = false;
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    Response.Redirect("DdVerification.aspx?courseID=" + courseID + "&ApplTypeId=" + ApplTypeId + "&DemandNoteTypeId=" + DemandNoteTypeId, true);
                };
            }
        }
        catch (Exception ex)
        {
        }

    }
    protected void BindGridView()
    {
        try
        {
            //this is the sample code how to bind the grid control                 
            int CourseCatgry = Convert.ToInt32(ddlCategory.SelectedValue);
            int CourseName = 0;
            int AppType = 0;
            int DemandNoteType = 0;
            if (ddlCourse.SelectedValue != "0")
                CourseName = Convert.ToInt32(ddlCourse.SelectedValue);
            if (ddlApplicationType.SelectedValue != "0")
                AppType = Convert.ToInt32(ddlApplicationType.SelectedValue);
            if (ddlDemandNoteType.SelectedValue != "0")
                DemandNoteType = Convert.ToInt32(ddlDemandNoteType.SelectedValue);
            if (CourseName != 0 && AppType != 0 && DemandNoteType != 0)
            {
                trlinks.Visible = true;
                lblError.Visible = false;
                gvMain.Visible = true;
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                Int32 demandNoteTypeId = Convert.ToInt32(enmDemandNoteType.Single);
                using (EConnectContext context = new EConnectContext())
                {
                    if (AppType == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                    {
                        int AppStatusId = Convert.ToInt32(enmCertificateExamApplicationStatus.PaymentVerificationPending);
                        var demand = from s in context.DemandNotes
                                     join d in context.DemandDraftTransactions on s.ID equals d.DemandNoteID
                                     join ad in context.CertificateExamApplications on s.ID equals ad.DemandNoteID
                                     join b in context.BatchItems on ad.ID equals b.CertificateExamApplicationID
                                     where ad.ApplicationStatusID == AppStatusId && ad.CourseID == CourseName && s.DemandNoteTypeID == DemandNoteType
                                     select new
                                     {
                                         ID = b.ID,
                                         DemandNoteNo = s.ID,
                                         DemandNoteDate = s.ApplicationDate,
                                         DemandNoteType = s.DemandNoteTypeID,
                                         FeeName = s.FeeType.Name,
                                         PayeeName = (s.DemandNoteTypeID == demandNoteTypeId) ? ad.Name : ad.Institute.Name,
                                         DDnumber = d.DemandDraftNumber,
                                         DDdate = d.DemandDraftDate,
                                         DDamount = d.DemandDraftAmount,
                                         courseID = ad.CourseID,
                                         BatchId = b.Batch.ID,
                                         ApplicationTypeId = s.ApplicationTypeID,
                                         DemandNoteTypeId = s.DemandNoteTypeID
                                     };
                        if (!String.IsNullOrEmpty(searchString))
                        {
                            demand = demand.Where(s => s.DemandNoteNo.ToString().Contains(searchString));
                        }
                        demand = demand.OrderBy(s => s.DemandNoteNo);
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "DemandNoteNo":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.DemandNoteNo);
                                    else
                                        demand = demand.OrderBy(s => s.DemandNoteNo);
                                    break;
                                case "DemandNoteDate":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.DemandNoteDate);
                                    else
                                        demand = demand.OrderBy(s => s.DemandNoteDate);
                                    break;
                                case "FeeName":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.FeeName);
                                    else
                                        demand = demand.OrderBy(s => s.FeeName);
                                    break;
                                case "PayeeName":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.PayeeName);
                                    else
                                        demand = demand.OrderBy(s => s.PayeeName);
                                    break;
                                case "DDnumber":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.DDnumber);
                                    else
                                        demand = demand.OrderBy(s => s.DDnumber);
                                    break;
                                case "DDdate":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.DDdate);
                                    else
                                        demand = demand.OrderBy(s => s.DDdate);
                                    break;
                                case "DDamount":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.DDamount);
                                    else
                                        demand = demand.OrderBy(s => s.DDamount);
                                    break;
                                default:
                                    demand = demand.OrderBy(s => s.DemandNoteNo);
                                    break;
                            }
                        }

                        PagingBar1.Bind(demand, ref gvMain);
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                        if (gvMain.Rows.Count <= 0)
                        {
                            lblError.Text = "No record found.";
                            lblError.Visible = true;
                            gvMain.Visible = false;
                            trlinks.Visible = false;
                        }
                    }
                    else if (AppType == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                    {
                        int AppStatusId = Convert.ToInt32(enmCourseApplicationStatus.PaymentVerificationPending);
                        var demand = (from s in context.DemandNotes
                                      join d in context.DemandDraftTransactions on s.ID equals d.DemandNoteID
                                      join ad in context.CourseRegistrationApplications on s.ID equals ad.DemandNoteID
                                      join b in context.BatchItems on ad.ID equals b.CourseRegistrationApplicationID
                                      where ad.ApplicationStatusID == AppStatusId && ad.CourseID == CourseName && s.DemandNoteTypeID == DemandNoteType
                                      select new
                                      {
                                          ID = b.ID,
                                          DemandNoteNo = s.ID,
                                          DemandNoteType = s.DemandNoteTypeID,
                                          DemandNoteID = d.ID,
                                          DemandNoteDate = s.ApplicationDate,
                                          FeeName = s.FeeType.Name,
                                          PayeeName = (s.DemandNoteTypeID == demandNoteTypeId) ? ad.Name : ad.Institute.Name,
                                          DDnumber = d.DemandDraftNumber,
                                          DDdate = d.DemandDraftDate,
                                          DDamount = d.DemandDraftAmount,
                                          courseID = ad.CourseID,
                                          BatchId = b.Batch.ID,
                                          ApplicationTypeId = s.ApplicationTypeID,
                                          DemandNoteTypeId = s.DemandNoteTypeID
                                      }).Distinct();
                        if (!String.IsNullOrEmpty(searchString))
                        {
                            demand = demand.Where(s => s.DDnumber.Contains(searchString));
                        }
                        demand = demand.OrderBy(s => s.DemandNoteNo);
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "DemandNoteNo":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.DemandNoteNo);
                                    else
                                        demand = demand.OrderBy(s => s.DemandNoteNo);
                                    break;
                                case "DemandNoteDate":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.DemandNoteDate);
                                    else
                                        demand = demand.OrderBy(s => s.DemandNoteDate);
                                    break;
                                case "FeeName":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.FeeName);
                                    else
                                        demand = demand.OrderBy(s => s.FeeName);
                                    break;
                                case "PayeeName":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.PayeeName);
                                    else
                                        demand = demand.OrderBy(s => s.PayeeName);
                                    break;
                                case "DDnumber":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.DDnumber);
                                    else
                                        demand = demand.OrderBy(s => s.DDnumber);
                                    break;
                                case "DDdate":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.DDdate);
                                    else
                                        demand = demand.OrderBy(s => s.DDdate);
                                    break;
                                case "DDamount":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.DDamount);
                                    else
                                        demand = demand.OrderBy(s => s.DDamount);
                                    break;
                                default:
                                    demand = demand.OrderBy(s => s.DemandNoteNo);
                                    break;
                            }
                        }
                        PagingBar1.Bind(demand, ref gvMain);
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                        if (gvMain.Rows.Count <= 0)
                        {
                            lblError.Text = "No record found.";
                            lblError.Visible = true;
                            gvMain.Visible = false;
                            trlinks.Visible = false;
                        }
                    }
                    else if (AppType == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    {
                        int AppStatusId = Convert.ToInt32(enmCourseExamApplicationStatus.PaymentVerificationPending);
                        var demand = (from s in context.DemandNotes
                                      join d in context.DemandDraftTransactions on s.ID equals d.DemandNoteID
                                      join ad in context.CourseExamApplications on s.ID equals ad.DemandNoteID
                                      join b in context.BatchItems on ad.ID equals b.CourseRegistrationApplicationID
                                      where ad.ApplicationStatusID == AppStatusId && ad.CourseID == CourseName && s.DemandNoteTypeID == DemandNoteType
                                      select new
                                      {
                                          ID = b.ID,
                                          DemandNoteNo = s.ID,
                                          DemandNoteType = s.DemandNoteTypeID,
                                          DemandNoteID = d.ID,
                                          DemandNoteDate = s.ApplicationDate,
                                          FeeName = s.FeeType.Name,
                                          PayeeName = (s.DemandNoteTypeID == demandNoteTypeId) ? ad.Candidate.Name : ad.Institute.Name,
                                          DDnumber = d.DemandDraftNumber,
                                          DDdate = d.DemandDraftDate,
                                          DDamount = d.DemandDraftAmount,
                                          courseID = ad.CourseID,
                                          BatchId = b.Batch.ID,
                                          ApplicationTypeId = s.ApplicationTypeID,
                                          DemandNoteTypeId = s.DemandNoteTypeID
                                      }).Distinct();
                        if (!String.IsNullOrEmpty(searchString))
                        {
                            demand = demand.Where(s => s.DDnumber.Contains(searchString));
                        }
                        demand = demand.OrderBy(s => s.DemandNoteNo);
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "DemandNoteNo":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.DemandNoteNo);
                                    else
                                        demand = demand.OrderBy(s => s.DemandNoteNo);
                                    break;
                                case "DemandNoteDate":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.DemandNoteDate);
                                    else
                                        demand = demand.OrderBy(s => s.DemandNoteDate);
                                    break;
                                case "FeeName":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.FeeName);
                                    else
                                        demand = demand.OrderBy(s => s.FeeName);
                                    break;
                                case "PayeeName":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.PayeeName);
                                    else
                                        demand = demand.OrderBy(s => s.PayeeName);
                                    break;
                                case "DDnumber":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.DDnumber);
                                    else
                                        demand = demand.OrderBy(s => s.DDnumber);
                                    break;
                                case "DDdate":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.DDdate);
                                    else
                                        demand = demand.OrderBy(s => s.DDdate);
                                    break;
                                case "DDamount":
                                    if (sortOrder == "DESC")
                                        demand = demand.OrderByDescending(s => s.DDamount);
                                    else
                                        demand = demand.OrderBy(s => s.DDamount);
                                    break;
                                default:
                                    demand = demand.OrderBy(s => s.DemandNoteNo);
                                    break;
                            }
                        }
                        PagingBar1.Bind(demand, ref gvMain);
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                        if (gvMain.Rows.Count <= 0)
                        {
                            lblError.Text = "No record found.";
                            lblError.Visible = true;
                            gvMain.Visible = false;
                            trlinks.Visible = false;
                        }
                    }
                };
            }
            else if (gvMain.Rows.Count <= 0)
            {
                lblError.Text = "No record found.";
                lblError.Visible = true;
                gvMain.Visible = false;
                trlinks.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void PupulateCourses(Int32 CourseCategoryID, DropDownList ddl, ListItem lst)
    {
        try
        {
            ddl.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == CourseCategoryID
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void PopulateDataListMode()
    {
        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                var Category = from p in context.CourseCategories
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCategory, Category, lst);

            };
            ListItem lst1 = new ListItem("--Select One--", "0");
            EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlDemandNoteType, typeof(enmDemandNoteType), lst1);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillApplicationTypes(Int32 CourseID, DropDownList ddl, ListItem lst)
    {
        try
        {
            ddl.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {
                Int32 courseTypeID = context.Courses.Where(c => c.ID == CourseID).Select(c => c.CourseTypeID).FirstOrDefault();
                var ApplicationList = from p in context.ApplicationTypes
                                      where p.CourseTypeID == courseTypeID
                                      select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, ApplicationList, lst);
            };
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
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            string courseID = ddlCourse.SelectedValue;
            string ApplTypeId = ddlApplicationType.SelectedValue;
            string DemandNoteTypeId = ddlDemandNoteType.SelectedValue;
            Response.Redirect("DdVerification.aspx?courseID=" + courseID + "&ApplTypeId=" + ApplTypeId + "&DemandNoteTypeId=" + DemandNoteTypeId, true);
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
            ddlDemandNoteType.SelectedValue = "0";
            ddlCourse.SelectedValue = "0";
            ddlApplicationType.SelectedValue = "0";
            BindGridView();
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
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var demand = from s in context.DemandDraftTransactions
                         select new { ddnumber = s.DemandDraftNumber };
            if (!String.IsNullOrEmpty(searchString))
            {
                demand = demand.Where(s => s.ddnumber.Contains(searchString));
            }
            demand = demand.OrderBy(s => s.ddnumber).Distinct();
            foreach (var dd in demand)
            {
                items.Add(dd.ddnumber);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("DdVerification.aspx", true);
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillApplicationTypes(Convert.ToInt32(ddlCourse.SelectedValue), ddlApplicationType, new ListItem("--Select One--", "0"));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlApplicationType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected Boolean IsValidRecord()
    {
        try
        {
            if (gvMain.Rows.Count > 0)
            {
                for (int i = 0; i < gvMain.Rows.Count; i++)
                {
                    CheckBox chk = (CheckBox)gvMain.Rows[i].FindControl("chk");
                    TextBox txtDDnumber = (TextBox)gvMain.Rows[i].FindControl("txtDDnumber");
                    TextBox txtDDdate = (TextBox)gvMain.Rows[i].FindControl("txtDDdate");

                    if (chk != null)
                    {
                        if (chk.Checked)
                        {

                            if (txtDDnumber.Text == "")
                            {
                                ShowAlert("Please select Demand Draft Number", true);
                                txtDDnumber.Focus();
                                return false;
                            }
                            else if (!IsNumeric(txtDDnumber.Text))
                            {
                                ShowAlert("Invalid Demand Draft Number", true);
                                txtDDnumber.Focus();
                                return false;
                            }
                            else if (txtDDdate.Text == "")
                            {
                                ShowAlert("Please Demand Draft Date", true);
                                txtDDdate.Focus();
                                return false;
                            }
                            else if (!IsDate(txtDDdate.Text))
                            {
                                ShowAlert("Invalid Date", true);
                                txtDDdate.Focus();
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void BtnVerify_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidRecord())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        Int16 verifiedCount = 0;
                        Int64 batchItemID = 0;

                        int applicationType = Convert.ToInt32(ddlApplicationType.SelectedValue);
                        for (int i = 0; i < gvMain.Rows.Count; i++)
                        {
                            CheckBox cbx = (CheckBox)gvMain.Rows[i].FindControl("chk");
                            if (cbx != null)
                            {
                                if (cbx.Checked)
                                {
                                    batchItemID = Convert.ToInt64(gvMain.DataKeys[i].Values[0]);
                                    BatchItem batchItem = context.BatchItems.Find(batchItemID);
                                    if (batchItem != null)
                                    {
                                        verifiedCount += 1;
                                        if (batchItem.StatusID == Convert.ToInt32(enmCertificateExamApplicationStatus.PaymentVerificationPending))
                                        {

                                        }
                                        if (applicationType == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                                        {
                                            Int32 ApplicationReceivedByNIELIT = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                                            batchItem.CourseRegistrationApplication.DemandNote.DemandDraftTransaction.IsVerified = true;
                                            batchItem.CourseRegistrationApplication.DemandNote.DemandDraftTransaction.VerificatonDate = DateTime.Now;
                                            batchItem.CourseRegistrationApplication.DemandNote.DemandDraftTransaction.VerifiedByID = Convert.ToInt32(Session["UserID"]);
                                            batchItem.StatusID = ApplicationReceivedByNIELIT;
                                            batchItem.CourseRegistrationApplication.ApplicationStatusID = ApplicationReceivedByNIELIT;
                                            batchItem.CourseRegistrationApplication.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                            batchItem.CourseRegistrationApplication.DemandNote.PaymentStatusID = batchItem.CourseRegistrationApplication.PaymentStatusID.Value;
                                        }
                                        else if (applicationType == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                                        {
                                            Int32 ApplicationReceivedByRegionalCentre = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationReceivedByRegionalCentre);
                                            batchItem.CertificateExamApplication.DemandNote.DemandDraftTransaction.IsVerified = true;
                                            batchItem.CertificateExamApplication.DemandNote.DemandDraftTransaction.VerificatonDate = DateTime.Now;
                                            batchItem.CertificateExamApplication.DemandNote.DemandDraftTransaction.VerifiedByID = Convert.ToInt32(Session["UserID"]);
                                            batchItem.StatusID = ApplicationReceivedByRegionalCentre;
                                            batchItem.CertificateExamApplication.ApplicationStatusID = ApplicationReceivedByRegionalCentre;
                                            batchItem.CertificateExamApplication.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                            batchItem.CertificateExamApplication.DemandNote.PaymentStatusID = batchItem.CertificateExamApplication.PaymentStatusID;
                                        }
                                        else if (applicationType == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                                        {
                                            Int32 ApplicationReceivedByNIELIT = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);
                                            batchItem.CourseExamApplication.DemandNote.DemandDraftTransaction.IsVerified = true;
                                            batchItem.CourseExamApplication.DemandNote.DemandDraftTransaction.VerificatonDate = DateTime.Now;
                                            batchItem.CourseExamApplication.DemandNote.DemandDraftTransaction.VerifiedByID = Convert.ToInt32(Session["UserID"]);
                                            batchItem.StatusID = ApplicationReceivedByNIELIT;
                                            batchItem.CourseExamApplication.ApplicationStatusID = ApplicationReceivedByNIELIT;
                                            batchItem.CourseExamApplication.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                            batchItem.CourseExamApplication.DemandNote.PaymentStatusID = batchItem.CertificateExamApplication.PaymentStatusID;
                                        };
                                        context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                                        context.SaveChanges();
                                    }
                                }
                            }
                        }
                        scope.Complete();
                        ShowAlert("Demand Draft details has been verified of " + verifiedCount.ToString() + " applications.", true);
                    };
                };
                BindGridView();
                gvMain.Visible = true;

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BtnKA_Click(object sender, EventArgs e)
    {
        try
        {
            Int32 successCount = 0;
            Int32 failedCount = 0;
            Int32 totalcount = 0;
            Int64 demandNoteID = 0;
            Int64 batchItemID = 0;
            string demandDraftNumber = "";
            DateTime? previousdate = null;
            DateTime? ddCreationDate = null;
            DateTime demandDraftDate;
            if (IsValidRecord())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        for (int i = 0; i < gvMain.Rows.Count; i++)
                        {
                            CheckBox cbx = (CheckBox)gvMain.Rows[i].FindControl("chk");
                            if (cbx != null)
                            {
                                if (cbx.Checked)
                                {
                                    demandNoteID = Convert.ToInt64(gvMain.DataKeys[i].Values[4]);
                                    DemandNote demandNote = context.DemandNotes.Find(demandNoteID);
                                    DemandDraftTransaction dd = context.DemandDraftTransactions.Where(s => s.DemandNoteID == demandNoteID).FirstOrDefault();
                                    batchItemID = Convert.ToInt64(Convert.ToInt64(gvMain.DataKeys[i].Values[0]));
                                    BatchItem batchItem = context.BatchItems.Find(batchItemID);
                                    if (demandNote != null)
                                    {
                                        totalcount = totalcount + 1;
                                        if (dd != null)
                                        {
                                            previousdate = Convert.ToDateTime(dd.Date).AddDays(-75);
                                            ddCreationDate = dd.Date;
                                            demandDraftNumber = ((TextBox)gvMain.Rows[i].FindControl("txtDDnumber")).Text;
                                            demandDraftDate = Convert.ToDateTime(((TextBox)gvMain.Rows[i].FindControl("txtDDdate")).Text);
                                            if (demandNote.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                                            {
                                                if ((!isNumber(demandDraftNumber)) || ((demandDraftDate.Date < previousdate.Value.Date || demandDraftDate.Date > ddCreationDate.Value.Date)))
                                                {
                                                    failedCount += 1;
                                                }
                                                else
                                                {
                                                    successCount = successCount + 1;
                                                    dd.DemandDraftDate = demandDraftDate;
                                                    dd.DemandDraftNumber = demandDraftNumber;
                                                    context.Entry(dd).State = System.Data.Entity.EntityState.Modified;
                                                    context.SaveChanges();
                                                }
                                            }
                                            else if (demandNote.enmApplicationType == enmApplicationType.CertificateExamApplication)
                                            {
                                                if ((!isNumber(demandDraftNumber)) || ((demandDraftDate.Date < previousdate.Value.Date || demandDraftDate.Date > ddCreationDate.Value.Date)))
                                                {
                                                    failedCount += 1;
                                                }
                                                else
                                                {
                                                    successCount = successCount + 1;
                                                    dd.DemandDraftDate = demandDraftDate;
                                                    dd.DemandDraftNumber = demandDraftNumber;
                                                    context.Entry(dd).State = System.Data.Entity.EntityState.Modified;
                                                    context.SaveChanges();
                                                }
                                            }
                                            else if (demandNote.enmApplicationType == enmApplicationType.CourseExamApplication)
                                            {
                                                if ((!isNumber(demandDraftNumber)) || ((demandDraftDate.Date < previousdate.Value.Date || demandDraftDate.Date > ddCreationDate.Value.Date)))
                                                {
                                                    failedCount += 1;
                                                }
                                                else
                                                {
                                                    successCount = successCount + 1;
                                                    dd.DemandDraftDate = demandDraftDate;
                                                    dd.DemandDraftNumber = demandDraftNumber;
                                                    context.Entry(dd).State = System.Data.Entity.EntityState.Modified;
                                                    context.SaveChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        scope.Complete();
                        ShowAlert("Out of " + totalcount.ToString() + " applications marked for updating Demand-Draft Details , " + successCount + " applications Demand-Draft details has been updated and  " + failedCount + " applications  Demand-Draft details has not been updated.", true);
                    };
                };
            }
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected bool isNumber(string txtBox)
    {
        try
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            if (txtBox.Trim() != "")
            {
                if (!regex.IsMatch(txtBox.Trim()))
                {
                    return false;
                }
                else
                    return true;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}