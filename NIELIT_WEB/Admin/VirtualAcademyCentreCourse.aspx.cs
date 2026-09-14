using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Common;
using System.Text.RegularExpressions;
using EConnect.NIELIT;
using System.Web;
using System.Transactions;
using System.Data.Objects;
using EConnect;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO.Compression;


public partial class Admin_VirtualAcademyCentreCourse : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 UserTypeId = 0;
    Int32 entityID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        cbNielitCentres.Attributes.Add("onclick", "checkBoxList1OnCheck(this);");
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            entityID = Convert.ToInt32(Session["EntityID"]);
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights to view this page");
                Response.End();
            }

            if (!Page.IsPostBack)
            {
                User objUser;
                using (EConnectContext context = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();
                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    // RegionalCenter RegName = context.RegionalCenters.Find(loginUser.UserRefNumber);
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        FillCourseCategory();
                        FillCategoriesF();
                        BindGridNielitCentres();
                        ShowEditMode();
                        //FillddlcentreName();
                       
                    }
                    else
                    {
                        using (NIELITMISContext context1 = new NIELITMISContext())
                        {
                            ViewState["SortField"] = "";
                            ViewState["SortOrder"] = "";
                            FillCourseCategory();
                            FillCategoriesF();
                            BindGridView();
                           // FillddlcentreName();
                            BindGridNielitCentres();
                            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                            {
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Virtual Academy Centre Course", "Admin/VirtualAcademyCentreCourse.aspx?Id=" + Request.QueryString["Id"].ToString(), ""));
                            }
                            else
                            {
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Virtual Academy Centre Course", "Admin/VirtualAcademyCentreCourse.aspx", ""));
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                        ShowAlert(Request.QueryString["msg"].ToString());
                }
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
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

    protected void FillCategoriesF()
    {
        try
        {
            using (DataTable dt = FillCourseCategoryFilter())
            {
                if (dt.Rows.Count > 0)
                {
                    ddlcoursecategoryF.DataSource = dt;
                    ddlcoursecategoryF.DataTextField = "Name";
                    ddlcoursecategoryF.DataValueField = "ID";
                    ddlcoursecategoryF.DataBind();
                    ddlcoursecategoryF.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable FillCourseCategoryFilter()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetCourseCategoryForVirtualFilter", con))
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

    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCourse(Convert.ToInt32(ddlcoursecategory.SelectedValue));
    }

    protected void FillCourse(int pCourseCategory)
    {
        try
        {
            using (DataTable dt = FillCourseRecords(pCourseCategory))
            {
                if (dt.Rows.Count > 0)
                {
                    ddlcourseName.DataSource = dt;
                    ddlcourseName.DataTextField = "Name";
                    ddlcourseName.DataValueField = "ID";
                    ddlcourseName.DataBind();
                    ddlcourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
                BreadCrumb1.Render();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillCourseFilter(int pCourseCategory)
    {
        try
        {
            using (DataTable dt = FillCourseRecordsFilter(pCourseCategory))
            {
                if (dt.Rows.Count > 0)
                {
                    ddlCourseNameF.DataSource = dt;
                    ddlCourseNameF.DataTextField = "Name";
                    ddlCourseNameF.DataValueField = "ID";
                    ddlCourseNameF.DataBind();
                    ddlCourseNameF.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
                BreadCrumb1.Render();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    ////protected void FillddlcentreName()
    ////{
    ////    try
    ////    {
    ////        ListItem lst1 = new ListItem("--Select One--", "0");
    ////            using (NIELITMISContext context = new NIELITMISContext())
    ////            {                    
    ////                ddlCenter.ClearSelection();

    ////                var Center = from t in context.NielitCentres                                 
    ////                             orderby (t.Name)
    ////                             select new { ValueField = t.ID, TextField = t.Name };
    ////                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, Center, lst1);
    ////            }
            
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        ShowAlert(ex.Message, true);
    ////    }
    ////}
    protected void RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");
        }
    }
    protected void BindGridNielitCentres()
    {
        try
        {
            lblError.Visible = false;
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            DataTable DT = new DataTable();
            
            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            using (SqlCommand Cmm = new SqlCommand("select ID, Name FROM [NIELITMIS].[dbo].[NielitCentres] ORDER BY  Name", con))
            {
                Cmm.CommandType = CommandType.Text;
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                Sda.Fill(DT);
            }
            con.Close();

            cbNielitCentres.DataSource = DT;
            cbNielitCentres.DataTextField = "Name";
            cbNielitCentres.DataValueField = "ID";
            cbNielitCentres.DataBind();
            uPnlGrid.Update();
            uPnlNavigation.Update();

            cbNielitCentres.Visible = true;
            allChkBox.Visible = false;

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
            //context.Dispose();
        }
    }
    protected void allChkBox_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListItem chkitem in cbNielitCentres.Items)
        {
            if (allChkBox.Checked == true)
            {
                chkitem.Selected = true;
                cbNielitCentres.BackColor = System.Drawing.Color.LightGreen;

            }
            else
            {
                chkitem.Selected = false;
                cbNielitCentres.BackColor = System.Drawing.Color.White;
            }
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
                using (SqlCommand cmd = new SqlCommand("getNIELITCoursesAll", con))
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


    public DataTable FillCourseRecordsFilter(int pCourseCategory)
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("getNIELITCoursesALLFilter", con))
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
    protected bool validateupdate()
    {
        int count = 0;
        bool update = false;
        foreach (ListItem li in cbNielitCentres.Items)
        {
            if (li.Selected)
            {
                count = count + 1;
                if (count > 1)
                {
                    update = false;
                    break;
                }
                else
                {
                    update = true;
                }
            }
        }
        return update;

    }
    protected void ShowEditMode()
    {
        try
        {
           
            using (NIELITMISContext context = new NIELITMISContext())
            {
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;

                btnSave.Text = "Update";
                lblHeading.Text = "Virtual Academy Centre Course";
                Int64 Id = 0;
                BindGridNielitCentres();
                Id = Convert.ToInt32(Request.QueryString["Key"]);
                VirtualAcademyCentreCourse editCourse = context.VirtualAcademyCentreCourse.Find(Id);
                var courseCnt = (from c in context.VirtualAcademyCentreCourse
                                 where c.ID == Id
                                 select new
                                         {
                                             ID = c.ID,
                                             CourseCat = c.courseCatID,
                                             CourseID = c.courseID,
                                             CentreId=c.centreID,
                                             IsActive=c.isActive==true ? "1" :"0"
                                         }).FirstOrDefault();
                if (courseCnt != null)
                {
                    trisactive.Visible = true;
                    trisactive1.Visible = true;
                    ddlcoursecategory.SelectedValue = (courseCnt.CourseCat != null && courseCnt.CourseCat != 0) ? courseCnt.CourseCat.ToString() : "0";
                    // int id3 = Convert.ToInt32(ddlCourse.SelectedValue);
                    ddlcoursecategory_SelectedIndexChanged(ddlcoursecategory, EventArgs.Empty);
                    ddlcourseName.SelectedValue = courseCnt.CourseID.ToString();
                    ddlcoursecategory.Enabled = false;
                    ddlcourseName.Enabled = false;
                    cbNielitCentres.Enabled = false;
                    if (courseCnt.IsActive.ToString() == "1")
                    {
                        ddlIsActive.SelectedValue="1";
                    }
                    else
                    {
                        ddlIsActive.SelectedValue = "0";
                    }
                    foreach (ListItem li in cbNielitCentres.Items)
                    {
                        if (li.Value == courseCnt.CentreId.ToString())
                        {
                            li.Selected = true;
                            //cbQualifications.SelectedItem.Attributes["style"] = "color:red";
                            cbNielitCentres.SelectedItem.Attributes["style"] = "BackGround-color: LightGreen";
                        }
                    }
                   // ddlCenter.SelectedValue = courseCnt.CentreId.ToString();
                }
                else if (courseCnt == null)
                {
                    trisactive.Visible = false;
                    trisactive1.Visible = false;
                }

                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Virtual Academy Centre Course", "Admin/VirtualAcademyCentreCourse.aspx?" + Request.QueryString.ToString(), ""));

                ViewState["LastModifiedOn"] = DateTime.Now;

            };
            if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            {
                btnSave.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("VirtualAcademyCentreCourse.aspx", true);
    }

    public DataTable FillGridViewVirtualAcademyCentreCourse()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("getVirtualAcdCentreCourseDetails", con))
                {
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@pCentreID", SqlDbType.BigInt);
                    //cmd.Parameters["@pCentreID"].Value = Convert.ToInt64(Session["EntityID"]);
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

    protected void BindGridView()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                int courseNameF = 0, courseCatF = 0;

                if (ddlCourseNameF.SelectedValue != "0")
                    courseNameF = Convert.ToInt32(ddlCourseNameF.SelectedValue);

                if (ddlcoursecategoryF.SelectedValue != "0")
                    courseCatF = Convert.ToInt32(ddlcoursecategoryF.SelectedValue);
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();

                using (DataTable dt = FillGridViewVirtualAcademyCentreCourse())

                    if (dt.Rows.Count > 0)
                    {
                        var courses = (from p in dt.AsEnumerable()
                                       select new
                                       {
                                           ID = p.Field<Int64>("ID"),
                                           coursecatName = p.Field<string>("coursecatName"),
                                           Name = p.Field<string>("Name"),
                                           IsActive = p.Field<string>("isActive"),
                                           CourseCatId = p.Field<Int64>("courseCatID"),
                                           CourseId = p.Field<Int64>("courseID"),
                                           CentreId = p.Field<Int64>("CentreID"),
                                           CentreName = p.Field<string>("CentreName")
                                       });

                        if (!String.IsNullOrEmpty(searchString))
                        {
                            courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
                        }
                        if (courseCatF != 0)
                            courses = courses.Where(s => s.CourseCatId == courseCatF);
                        if (courseNameF != 0)
                            courses = courses.Where(s => s.CourseId == courseNameF);

                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "Name":
                                    if (sortOrder == "DESC")
                                        courses = courses.OrderByDescending(s => s.ID);
                                    else
                                        courses = courses.OrderBy(s => s.ID);
                                    break;

                                case "Code":
                                    if (sortOrder == "DESC")
                                        courses = courses.OrderByDescending(s => s.ID);
                                    else
                                        courses = courses.OrderBy(s => s.ID);
                                    break;

                                default:
                                    courses = courses.OrderBy(s => s.ID);
                                    break;
                            }

                        }
                        PagingBar1.Bind(courses, ref gvMain);
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                        lblError.Visible = false;
                        gvMain.Visible = true;
                        if (gvMain.Rows.Count <= 0)
                        {
                            lblError.Text = "No record found.";
                            lblError.Visible = true;
                            gvMain.Visible = false;
                        }
                        if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                        {
                            gvMain.Columns[8].Visible = false;
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
            if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to add new record.");
                return;
            }
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;

            //Change the heading text as required
            lblHeading.Text = "New Virtual Academy Centre Course";
            //Updating Breadcrumb
            //  BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Virtual Academy Centre Course", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("VirtualAcademyCentreCourse.aspx?ID=" + Request.QueryString["ID"].ToString()), true);
            }
            else
            {
                Response.Redirect("VirtualAcademyCentreCourse.aspx", true);
            }
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

    protected void SearchBar_Reset(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlCourseNameF.SelectedValue = "0";
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
            BreadCrumb1.Render();
            using (NIELITMISContext context = new NIELITMISContext())
            {
                //create and object 
                VirtualAcademyCentreCourse VirtualCentreCourse = new VirtualAcademyCentreCourse(); ;

                Int32 courseId = 0, courseCatId = 0;
                courseCatId = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                courseId = Convert.ToInt32(ddlcourseName.SelectedValue);

                string courseName = ddlcourseName.SelectedItem.Text;



                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    foreach (ListItem li in cbNielitCentres.Items)
                    {
                        if (li.Selected)
                        {
                            Int64 NielitCentresID = Convert.ToInt32(li.Value);

                    var courseCheck = (from p in context.VirtualAcademyCentreCourse
                                       where p.courseCatID == courseCatId && p.courseID == courseId && p.centreID == NielitCentresID

                                       select p).ToList();
                    if (courseCheck.Count != 0)
                    {                   
                        throw new Exception("Course already exists.");
                        return;
                    }
                    else
                    {                       
                                //VirtualCentreCourse.centreID = Convert.ToInt64(ddlCenter.SelectedValue);
                                VirtualCentreCourse.centreID = NielitCentresID;
                                VirtualCentreCourse.courseCatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                                VirtualCentreCourse.courseID = Convert.ToInt32(ddlcourseName.SelectedValue);//course  id                   
                                VirtualCentreCourse.enterDate = DateTime.Now;
                                VirtualCentreCourse.enterByID = Convert.ToInt32(Session["UserID"]);

                                context.VirtualAcademyCentreCourse.Add(VirtualCentreCourse);
                                context.SaveChanges();

                                strMessage = "New Record Saved";
                            }


                        }
                    }

                }

                else
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        foreach (ListItem li in cbNielitCentres.Items)
                        {
                            if (li.Selected)
                            {
                                Int32 NielitCentresID = Convert.ToInt32(li.Value);
                        var courseUpdate = (from p in context.VirtualAcademyCentreCourse
                                            where p.courseCatID == courseCatId && p.courseID == courseId && p.centreID == NielitCentresID

                                            select p).FirstOrDefault();

                       
                                //courseUpdate.centreID = Convert.ToInt32(ddlCenter.SelectedValue);
                                courseUpdate.centreID = NielitCentresID;

                                if (ddlIsActive.SelectedItem.Text == "Select")
                                {
                                    strMessage = "Please select IsActive";
                                    lblactiveerror.Text = "Please select IsActive";
                                    ddlIsActive.Focus();
                                    return;
                                }
                                else
                                {
                                    courseUpdate.isActive = ddlIsActive.SelectedValue == "1" ? true : false;
                                }

                                courseUpdate.enterDate = DateTime.Now;
                                courseUpdate.enterByID = Convert.ToInt32(Session["UserID"]);
                                context.SaveChanges();

                                scope.Complete();
                                strMessage = "Virtual Academy Centre Course Updated";
                            }
                        }
                    }
                }
            }
            Response.Redirect("VirtualAcademyCentreCourse.aspx?msg=" + strMessage, true);
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

    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            ddlcoursecategoryF.SelectedValue = "0";
            ddlCourseNameF.SelectedValue = "0";
            FillCourseCategory();
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

    protected void PerformPopupAction(object sender, EventArgs e)
    {
        try
        {
            if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
            {
                BindGridView();
                uPnlGrid.Update();
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to delete the records.", true);
                return;
            }
            using (TransactionScope scope = new TransactionScope())
            {
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    Int32 courseid = Convert.ToInt32(hfActionID.Value);
                    EConnect.NIELIT.VirtualAcademyCentreCourse course = context.VirtualAcademyCentreCourse.Find(courseid);

                    context.VirtualAcademyCentreCourse.Remove(course);
                    context.SaveChanges();
                    scope.Complete();
                    ShowAlert("Record deleted successfully.", true);
                    hfActionID.Value = "";
                };
            }
            BindGridView();
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
                if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    href += "&Id=" + Request.QueryString["Id"].ToString();
                }
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
                HyperLink hl2 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);
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
        Response.Redirect("VirtualAcademyCentreCourse.aspx", true);
    }

    protected void ddlcoursecategoryF_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCourseFilter(Convert.ToInt32(ddlcoursecategoryF.SelectedValue));
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        Int32 loginUserNo = 0, UserTypeId = 0;
        EConnectContext context = new EConnectContext();
        NIELITMISContext context1 = new NIELITMISContext();
        try
        {
            loginUserNo = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            UserTypeId = Convert.ToInt32(HttpContext.Current.Session["UserTypeId"]);
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();

            string searchString = prefixText.Trim().ToUpper();
            var courses = from c in context.Courses                        
                          join w in context.CourseCategories on c.CourseCategoryID equals w.ID
                          select new { Name = c.Name, CatName = w.Name };
                        
            courses = courses.Distinct();

            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString) || s.CatName.ToUpper().Contains(searchString));
            }
            courses = courses.OrderBy(s => s.Name);
           
            foreach (var course in courses)
            {
                items.Add(course.Name);              
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