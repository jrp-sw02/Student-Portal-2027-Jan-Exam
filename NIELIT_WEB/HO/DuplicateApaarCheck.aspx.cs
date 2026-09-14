using DocumentFormat.OpenXml.Presentation;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DuplicateApaarCheck : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Index.aspx");
            //currentRoleId = Convert.ToInt32(Session["RoleID"]);
            //if (!UserManager.HasRight(currentRoleId, enmRight.View))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}
            //loginUserType = (UserType)Session["UserType"];
            //entityID = Convert.ToInt64(Session["EntityID"]);

            if (!Page.IsPostBack)
            {
                if (gvMain.Rows.Count <= 0)
                {
                    lblMessage.Text = "Please Select Filter Criteria For View Records";
                    lblMessage.Visible = true;
                }

                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";

                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Duplicate Apaar List", "DuplicateApaarCheck.aspx", ""));
                PopulateCourses();  
            }

    


            if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                ShowAlert(Request.QueryString["msg"].ToString());



            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindGridView()
    {
        try
        {
            lblError.Visible = false;
            lblMessage.Visible = false;

            string searchString = ucSearchBar.SearchText.Trim().ToUpper();

            Int64 courseId = Convert.ToInt64(ddlCourseFilter.SelectedValue);
            Int64 examId = Convert.ToInt64(ddlExamName.SelectedValue);

     

            //if (courseId == 0 && examId == 0)
            //{
            //    lblError.Visible = true;
            //    lblError.Text = "Please Filter Records First, Then use Search option.";
            //    return;
            //}

            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(
                   ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))

            using (SqlCommand command = new SqlCommand("GetCCCDuplicates", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CourseID", courseId);
                command.Parameters.AddWithValue("@ExamID", examId);

                using (SqlDataAdapter da = new SqlDataAdapter(command))
                {
                    da.Fill(dt);
                }
            }

            //  Convert DataTable to LINQ queryable form
            var applications = dt.AsEnumerable();

            // Apply Search 
            if (!string.IsNullOrEmpty(searchString))
            {
                applications = applications.Where(row =>
                    row["Name"].ToString().ToUpper().Contains(searchString) ||
                    row["Number"].ToString().ToUpper().Contains(searchString)
                    );
            }

            DataTable applications_dt = (applications.Any()) ? applications.CopyToDataTable() : dt.Clone();


            if (dt.Rows.Count > 0)
            {
                gvMain.Visible = true;
                PagingBar1.Visible = true;

                PagingBar1.Bind(applications_dt, ref gvMain);

                //for (int i = 0; i < gvMain.Rows.Count; i++)
                //{
                //    gvMain.Rows[i].CssClass = (i % 2 == 0) ? "gdalternate1" : "gdrow1";
                //}
            }
            else
            {
                //gvMain.Visible = false;
                //PagingBar1.Visible = false;

                lblMessage.Text = "No record found.";
                lblMessage.Visible = true;
            }

            uPnlGrid.Update();
            uPnlNavigation.Update();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    //protected void BindGridView()
    //{
    //    try
    //    {

    //        string searchString = null;
    //        searchString = ucSearchBar.SearchText;
    //        Int64 ApplTypeID = 0;
    //        string sortOrder = ViewState["SortOrder"].ToString();
    //        string sortField = ViewState["SortField"].ToString();

    //        ucSearchBar.AutoCompleteContextKey = ApplTypeID.ToString().ToUpper();
    //        lblError.Visible = false;
    //        gvMain.Visible = true;

    //        Int64 Course = 0;
    //        Int64 ExamYear = 0;
    //        Int64 ExamName = 0;

    //        if (ddlCourseFilter.SelectedValue != "0")
    //            Course = Convert.ToInt32(ddlCourseFilter.SelectedValue);
    //        if (ddlExamYear.SelectedValue != "0")
    //            ExamYear = Convert.ToInt32(ddlExamYear.SelectedValue);
    //        if (ddlExamName.SelectedValue != "0")
    //            ExamName = Convert.ToInt32(ddlExamName.SelectedValue);

    //        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))
    //        {
    //            connection.Open();
    //            using (var command = new SqlCommand("GetCCCDuplicates", connection))
    //            {

    //                command.CommandType = CommandType.StoredProcedure;
    //                command.Parameters.Clear();
    //                //command.Parameters.AddWithValue("@course_catid", Convert.ToInt64(ddlcourse_cat.SelectedValue));
    //                command.Parameters.AddWithValue("@CourseID", Convert.ToInt64(ddlCourseFilter.SelectedValue));
    //                command.Parameters.AddWithValue("@ExamID", Convert.ToInt64(ddlExamName.SelectedValue));

    //                DataTable dt = new DataTable();

    //                using (SqlDataAdapter da = new SqlDataAdapter(command))
    //                {
    //                    da.Fill(dt);
    //                }


    //                if (dt.Rows.Count > 0)
    //                {
    //                    gvMain.Visible = true;
    //                    PagingBar1.Visible = true;
    //                    //gvMain.DataSource = dt;
    //                    //gvMain.DataBind();

    //                    PagingBar1.Bind(dt, ref gvMain);

    //                    // Apply alternating row styles
    //                    for (int i = 0; i < gvMain.Rows.Count; i++)
    //                    {
    //                        gvMain.Rows[i].CssClass = (i % 2 == 0) ? "gdalternate1" : "gdrow1";
    //                    }

    //                    //btnShowData.Visible = false;
    //                    //lblerror.Visible = false;

    //                }
    //                else
    //                {
    //                    //lblMessage.Text = dt.Rows.Count == 0 ? "No record found." : "";
    //                    gvMain.Visible = false;
    //                    PagingBar1.Visible = false;
    //                    //lblerror.Visible = true;
    //                    //lblerror.Text = "No record found";
    //                }

    //                uPnlGrid.Update();
    //                uPnlNavigation.Update();
    //            }
    //        }

    //        if (gvMain.Rows.Count <= 0)
    //        {
    //            lblError.Text = "No record found.";
    //            lblError.Visible = true;
    //        }
    //    }
    //    catch (Exception ex) { throw ex; }
    //    finally { context.Dispose(); }
    //}
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
    //protected void ToggleViewMode_Changed(object sender, EventArgs e)
    //{
    //    if (btnMode.ViewMode == ToggleView.Mode.New)
    //    {
    //        btnMode.ViewMode = ToggleView.Mode.List;
    //        mltvTab.ActiveViewIndex = 1;
    //        pnlFilter.Visible = false;
    //        ucSearchBar.Visible = false;
    //    }
  
    //}

    //protected void SearchBar_ApplySearch(object sender, EventArgs e)
    //{
    //    //string searchString = null;
    //    //string sortOrder = ViewState["SortOrder"].ToString();
    //    //string sortField = ViewState["SortField"].ToString();

    //    //lblError.Visible = false;
    //    //gvMain.Visible = true;
    //    try
    //    {
    //    //    using (EConnectContext context = new EConnectContext())
    //    //    {
    //    //        if (ucSearchBar.SearchText != "")
    //    //        {
    //    //            searchString = ucSearchBar.SearchText.Trim().ToUpper();

    //    //            Int64 DemandNoteID = 0;
    //    //            DemandNote demandNote = new DemandNote();


    //    //            if (IsNumeric(searchString))
    //    //            {
    //    //                DemandNoteID = Convert.ToInt64(searchString);
    //    //                demandNote = context.DemandNotes.Find(DemandNoteID);
    //    //            }
    //    //            else
    //    //            {
    //    //                ShowAlert("Please Enter Application Number", true);
    //    //            }
    //    //            if (demandNote != null)
    //    //            {
    //    //                int TypeId = Convert.ToInt32(demandNote.ApplicationTypeID);
    //    //                ucSearchBar.AutoCompleteContextKey = TypeId.ToString().ToUpper();


    //    //                var CexApp = (from ce in context.CertificateExamApplications
    //    //                              where ce.DemandNoteID == DemandNoteID
    //    //                              select new { CourseCateID = ce.CourseCategoryID, CourseID = ce.CourseID, ExamID = ce.ExamID, ExamYear = ce.Exam.ExamYear, ExamCycel = ce.Exam.ExaminationCycleID, PaymentStatus = ce.PaymentStatusID }).FirstOrDefault();
    //    //                if (CexApp != null)
    //    //                {


    //    //                    ddlCourseFilter.SelectedValue = CexApp.CourseID.ToString();
    //    //                    ddlCourseFilter_SelectedIndexChanged(ddlCourseFilter.SelectedValue, EventArgs.Empty);

    //    //                    ddlExamCycle.SelectedValue = CexApp.ExamCycel.ToString();
    //    //                    ddlExamCycle_SelectedIndexChanged(ddlExamCycle.SelectedValue, EventArgs.Empty);
    //    //                    ddlExamYear.SelectedValue = CexApp.ExamYear.ToString();
    //    //                    ddlExamYear_SelectedIndexChanged(ddlExamYear.SelectedValue, EventArgs.Empty);
    //    //                    ddlExamName.SelectedValue = CexApp.ExamID.ToString();

    //    //                }

    //    //            }
    //    //            else
    //    //            {
    //    //                ShowAlert("No record found for demand note number " + DemandNoteID.ToString(), true);
    //    //            }
    //    //        }
    //    //    }
    //    //    ;
    //    //    PagingBar1.CurrentPageIndex = 0;
    //    //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;


    //    PagingBar1.CurrentPageIndex = 0;
    //    gvMain.PageIndex = PagingBar1.CurrentPageIndex;
    //} 
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message, true);
    //    }
    //}
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
    protected void SaveRecord(object sender, EventArgs e)
    {
        //try
        //{
        //    context = new EConnectContext();
        //    //create and object 
        //    User objUser;
        //    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
        //    {

        //        strMessage = "New record saved.";
        //    }
        //    else
        //    {
        //        ////Initialize current object by loading it and get its current modified date
        //        strMessage = "Record updated.";
        //    }

        //    //Call save method
        //    //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
        //    //Redirect it to list mode
        //    Response.Redirect("CertificateCourse.aspx?msg=" + strMessage);
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
        //finally { context.Dispose(); }

    }
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            //ucSearchBar.SearchText = "";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            //BindGridView();
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
            //ddlCourseFilter.SelectedValue = "0";
            //ddlExamYear.SelectedValue = "0";
            //ddlExamName.SelectedValue = "0";
            //ddlExamCycle.SelectedValue = "0";
            //PagingBar1.CurrentPageIndex = 0;
            //gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            //lblError.Text = "Please Select Filter Criteria For View Records";
            //lblError.Visible = true;
            //uPnlGrid.Update();
            Response.Redirect("DuplicateRegistrationonapaar.aspx", true);

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
    protected void PerformPopupAction(object sender, EventArgs e)
    {
        //try
        //{
        //    context = new EConnectContext();
        //    if (hfActionID.Value != "")
        //    {
        //        String recordID = hfActionID.Value.Split('$')[0].ToString();
        //        LinkButton btnAction = (LinkButton)sender;
        //        if (btnAction.CommandName == "Delete")
        //        {
        //            //Load the object and apply validateion if required
        //            //call delete function
        //            //bind the grid again
        //            BindGridView();
        //            ShowAlert("Record deleted successfully.", true);
        //            hfActionID.Value = "";
        //        }
        //        else if (btnAction.CommandName == "Action")
        //        {
        //            //Load the object and apply validateion if required
        //            //call function to perform required action
        //            //bind the grid again
        //            BindGridView();
        //            ShowAlert("Record Action1 successfully.", true);
        //            hfActionID.Value = "";
        //        }
        //        uPnlGrid.Update();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    hfActionID.Value = "";
        //    ShowAlert(ex.Message, true);
        //}
        //finally { context.Dispose(); }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                //HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                //hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);

                //HyperLink hl2 = (HyperLink)e.Row.Cells[2].Controls[0];
                //hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);

                //HyperLink hl3 = (HyperLink)e.Row.Cells[3].Controls[0];
                //hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);

                //HyperLink hl4 = (HyperLink)e.Row.Cells[4].Controls[0];
                //hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl);

                //HyperLink hl5 = (HyperLink)e.Row.Cells[5].Controls[0];
                //hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl);

                //HyperLink hl6 = (HyperLink)e.Row.Cells[6].Controls[0];
                //hl6.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl6.NavigateUrl);

                //HyperLink hl7 = (HyperLink)e.Row.Cells[7].Controls[0];
                //hl7.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl7.NavigateUrl);

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count, String contextKey)
    {
        EConnectContext context = new EConnectContext();
        try
        {

            if (count <= 0) count = 10;
            string search = (prefixText ?? "").Trim().ToUpper();

            string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

            var items = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            using (var conn = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("GetCCCDuplicates", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = (reader["name"] as string ?? "").Trim();
                        // Match logic
                        if (string.IsNullOrEmpty(search) ||
                            name.ToUpper().Contains(search)
                            // || code.ToUpper().Contains(search))
                            )
                        {
                            if (!string.IsNullOrEmpty(name))
                                items.Add(name);
                            //if (!string.IsNullOrEmpty(code))
                            //    items.Add(code);
                        }

                        // Stop early if we already have enough
                        if (items.Count >= count)
                            break;
                    }

                }
            }

            //if (count <= 0)
            //    count = 10;
            //Int64 ApplicationType = 0;
            //if (!String.IsNullOrEmpty(contextKey))
            //    ApplicationType = Convert.ToInt64(contextKey);


            //List<String> items = new List<String>();
            //string searchString = prefixText.Trim().ToUpper();


            //    var result = from s in context.DemandNotes
            //                 join c in context.CertificateExamApplications on s.ID equals c.DemandNoteID
            //                 select new { Name = SqlFunctions.StringConvert((Double)(s.ID)) };


            //    if (!string.IsNullOrEmpty(searchString))
            //    {
            //        result = result.Where(s => s.Name.ToUpper().Contains(searchString));
            //    }
            //    var r1 = from s in context.DemandNotes
            //             join c in context.CertificateExamApplications on s.ID equals c.DemandNoteID
            //             select new { Name = s.DemandNoteTypeID == 1 ? c.Name : c.Institute.Name };


            //    if (!string.IsNullOrEmpty(searchString))
            //    {
            //        r1 = r1.Where(s => s.Name.ToUpper().Contains(searchString));
            //    }

            //    result = result.Union(r1).Take(count);

            //    foreach (var course in result)
            //    {
            //        items.Add(course.Name);
            //    }

            //return items.ToArray();\
            return items.Take(count).ToArray();

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("HO/DuplicateApaarCheck.aspx", true);
    }
    protected void PopulateCourses()
    {
        try
        {

            //Added by Deep on 11 May 2022 for Short term course(6) view all courses start
            //if (CourseCategoryID == 6)
            //{
            //    using (EConnectContext context = new EConnectContext())
            //    {
            //        var CourseList = from p in context.Courses
            //                         where p.CourseCategoryID == CourseCategoryID
            //                         select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" };
            //        EConnect.Utils.Common.ControlUtility.BindListObject(ddl, CourseList, lst);
            //    }
            //    ;
            //}
            //else
            //{
            //Added by Deep on 11 May 2022  Short term course(6) view all courses End

            //ListItem lst = new ListItem("--Select One--", "0");

            //using (EConnectContext context = new EConnectContext())
            //{
            //    var CourseList = from p in context.Courses
            //                        where p.CourseCategoryID == 2
            //                        //Added for code
            //                        && p.ShowOnWeb
            //                        select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" };
            //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseFilter, CourseList, lst);
            //};

            ListItem lst = new ListItem("-Select One-", "0");
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("sp_GetCourse", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@pCourseCategory", 2);
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                      
                        da.Fill(dt);
                    }
                    //EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseFilter, dt, lst);
                    ddlCourseFilter.DataSource = dt;

                    ddlCourseFilter.DataTextField = "Name";
                    ddlCourseFilter.DataValueField = "ID";
                    ddlCourseFilter.DataBind();
                    ddlCourseFilter.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlCourseFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindExamCycle(Convert.ToInt32(ddlCourseFilter.SelectedValue));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }
    protected void FillExamYears(int catID, int couID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (couID != 0 && catID != 0)
                {

                    var examYear = (from p in context.Exams

                                    where (p.CourseID == couID && p.CourseCategoryID == catID)
                                    orderby (p.ExamYear) descending
                                    select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, examYear, lst);
                }
            }
            ;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {

        Int32 couID = Convert.ToInt32(ddlCourseFilter.SelectedValue);
        Int32 exmYear = Convert.ToInt32(ddlExamYear.SelectedValue);
        //ddlExamName.Items.Clear();
        FillExamNames(2, couID, exmYear);
        ListItem lst = new ListItem("--Select One--", "0");
        if (ddlExamYear.SelectedValue == "0")
        {
            ddlExamName.Items.Add(lst);
        }
    }
    protected void FillExamNames(Int32 catID, Int32 couID, Int32 exmYear)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (couID != 0 && catID != 0 && exmYear != 0)
                {
                    
                        var examName = (from p in context.Exams
                                        join ce in context.CertificateExamApplications on p.ID equals ce.ExamID
                                        where (p.CourseID == couID && p.CourseCategoryID == catID && p.ExamYear == exmYear)
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.Name }).Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, examName, lst);
               

                }
            }
            ;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 courseid = Convert.ToInt32(ddlCourseFilter.SelectedValue);
        Int32 examcycleid = Convert.ToInt32(ddlExamCycle.SelectedValue);

        BindExamYear(courseid, examcycleid);
    }
    protected void BindExamYear(Int32 cid, Int32 examcycleid)
    {
        try
        {
            //ListItem lst = new ListItem("--Select One--", "0");
            //using (EConnectContext context = new EConnectContext())
            //{
    
            //        var examyear = (from s in context.Exams
            //                        join c in context.CertificateExamApplications
            //                         on s.ID equals c.ExamID
            //                        where s.CourseID == cid && s.ExaminationCycleID == examcycleid
            //                        orderby (s.ExamYear) descending
            //                        select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();

            //        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, examyear, lst);
            
            //        ddlExamName.Items.Insert(0, lst);
            //};


            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand("GetExamYearebyCourseandCourseCategory", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Course_cat", cid);
                    cmd.Parameters.AddWithValue("@Exam_Cycle_ID", examcycleid);
                    DataTable dt = new DataTable();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    //EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseFilter, dt, lst);
                    ddlExamYear.DataSource = dt;
                    ddlExamYear.DataTextField = "ExamYear";
                    ddlExamYear.DataValueField = "ExamYear";
                    ddlExamYear.DataBind();
                    ddlExamYear.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
            }



        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void BindExamCycle(int courseId)
    {
        try
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("GetExamCyclebyCourseandCourseCategory", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Course_cat", 2);
                cmd.Parameters.AddWithValue("@Course_Id", courseId);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            ddlExamCycle.DataSource = dt;
            ddlExamCycle.DataTextField = "Name";
            ddlExamCycle.DataValueField = "ID";
            ddlExamCycle.DataBind();

            ddlExamCycle.Items.Insert(0, new ListItem("--Select One--", "0"));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

}