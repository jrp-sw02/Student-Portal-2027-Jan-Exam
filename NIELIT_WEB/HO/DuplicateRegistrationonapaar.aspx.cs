using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HO_DuplicateRegistrationonapaar : BasePage
{
    String strMessage = string.Empty;
    NIELITMISContext context;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 UserTypeid = 0;
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
            //loginUserNo = Convert.ToInt32(Session["UserID"]);
            //UserTypeid = Convert.ToInt32(Session["UserType"]);

            //if (!UserManager.HasRight(currentRoleId, enmRight.View))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}


            if (!Page.IsPostBack)
            {
                lblError.Text = "Please Select Filter Records to See Records.";
                lblError.Visible = true;

                User objUser;
                using (EConnectContext context = new EConnectContext())
                {
                    //objUser = new EConnect.URM.User();
                    //User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    //RegionalCenter RegName = context.RegionalCenters.Find(loginUser.UserRefNumber);

                  
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";
                        PopulateCourses();
                        //BindGridView();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Duplicate Registration On Apaar", "HO/DuplicateRegistrationonapaar.aspx", ""));
                   
                }
            }
            BreadCrumb1.Render();
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
            ddlCourseFilter.SelectedValue = "0";
            ddlExamYear.SelectedValue = "0";
            ddlExamName.SelectedValue = "0";
            ddlExamCycle.SelectedValue = "0";
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
                //HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                //hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);

                //HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                //hl1.NavigateUrl = hl.NavigateUrl;

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                //Image imgAction = (Image)e.Row.FindControl("imgAction");
                //imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   

                //CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                //imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
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

    protected void BindGridView()
    {
        try
        {
            lblError.Text = "";
            lblError.Visible = false;
            //this is the sample code how to bind the grid control
            //context = new NIELITMISContext();

            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
 
            Int64 courseId = Convert.ToInt64(ddlCourseFilter.SelectedValue);
            Int64 examId = Convert.ToInt64(ddlExamName.SelectedValue);

            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(
                   ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))

            using (SqlCommand command = new SqlCommand("GetCCCDuplicates", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CourseID", courseId);
                command.Parameters.AddWithValue("@ExamID", examId);

                command.CommandTimeout = 0;   // 0 = infinite timeout

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

            if (applications_dt.Rows.Count > 0)
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
                gvMain.DataSource = null;
                gvMain.Visible = false;
                PagingBar1.Visible = false;
                lblError.Text = "No record found.";
                lblError.Visible = true;
            }

            uPnlGrid.Update();
            uPnlNavigation.Update();
        }
        catch (Exception ex)
        {
            throw ex;
        }
       
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        NIELITMISContext context = new NIELITMISContext();

        try
        {
            if (count <= 0)
                count = 10;


            List<String> items = new List<String>();

            string searchString = prefixText.Trim().ToUpper();

            //DataTable dt = new DataTable();

            //using (SqlConnection connection = new SqlConnection(
            //       ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))
            //{
            //    string sql = "select Name, Number from Certificate_Exam_Application where Payment_Status_ID = 2";

            //    using (SqlCommand command = new SqlCommand(sql, connection))
            //    {
            //            //command.Parameters.AddWithValue("@CourseID", courseId);
            //            //command.Parameters.AddWithValue("@ExamID", examId);

            //        using (SqlDataAdapter da = new SqlDataAdapter(command))
            //        {
            //            da.Fill(dt);
            //        }
            //    }
            //}

            //// Convert DataTable to LINQ
            //var applications = dt.AsEnumerable();

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    applications = applications.Where(s => s["Name"].ToString().ToUpper().Contains(searchString));
            //}

            //applications = applications.OrderBy(s => s["Name"].ToString().ToUpper());

            //foreach (var row in applications)
            //{
            //    items.Add(row["Name"].ToString());
            //}

            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            context.Dispose();
        }
    }
    protected Boolean isvalidForm()
    {
        try
        {
            if (ddlCourseFilter.SelectedValue == "0")
            {
                strMessage = "Please select Course Name.";
                ShowAlert(strMessage, true);
                return false;
            }

            if (ddlExamCycle.SelectedValue == "0")
            {
                strMessage = "Please select Exam Cycle.";
                ShowAlert(strMessage, true);
                return false;
            }

            if (ddlExamYear.SelectedValue == "0")
            {
                strMessage = "Please select Exam Year.";
                ShowAlert(strMessage, true);
                return false;
            }

            if (ddlExamName.SelectedValue == "0")
            {
                strMessage = "Please select Exam Year.";
                ShowAlert(strMessage, true);
                return false;
            }



            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
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