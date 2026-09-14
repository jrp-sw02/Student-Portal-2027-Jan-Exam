using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public partial class Admin_VirtualAcademyTentativeStartDate : BasePage
{
    String strMessage = string.Empty;   
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                // for testing vishal
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {

                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    ShowEditMode();
                }
                else
                {
                    FillCourseF();

                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BindGridView();
                    if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Virtual Academy Tentative Start Date", "Admin/VirtualAcademyTentativeStartDate.aspx?CourseId=" + Request.QueryString["CourseId"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Virtual Academy Tentative Start Date", "Admin/VirtualAcademyTentativeStartDate.aspx", ""));
                    }
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
            ddlIsActive.Enabled = false;
           // BreadCrumb1.Render();
            
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    public DataTable FillGridViewVirtualAcademyTentativeStartDate()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
		
        using (SqlCommand cmd = new SqlCommand("Get_VirtualAcademyTentativeStartDate", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@pCentreID", SqlDbType.BigInt);
                    //cmd.Parameters["@pCentreID"].Value = loginUserNoForCenterID; 
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
            Int32 IsActive = 0;
            Int32 courseID = 0;

            if (ddlIsActiveF.SelectedValue != "0")
                IsActive = Convert.ToInt32(ddlIsActiveF.SelectedValue);

            if (ddlCourseNameF.SelectedValue != "0")
                courseID = Convert.ToInt32(ddlCourseNameF.SelectedValue);
           
           NIELITMISContext context1 = new NIELITMISContext();
           string searchString = ucSearchBar.SearchText.Trim().ToUpper();
           string sortOrder = ViewState["SortOrder"].ToString();
           string sortField = ViewState["SortField"].ToString();

           using (DataTable dt = FillGridViewVirtualAcademyTentativeStartDate())

                    if (dt.Rows.Count > 0)
                    {
                        var courses = (from p in dt.AsEnumerable()
                                       select new
                                       {
                                           ID = p.Field<Int64>("CourseID"),
                                           CourseID = p.Field<Int64>("CourseID"),
                                           courseName = p.Field<string>("CourseName"),
                                           TentativeStartDate = p.Field<DateTime>("tentativeStartDate"),
                                           IsActive = p.Field<string>("IsActive"),
                                           IActiveID=  p.Field<int>("IActiveID")
                                       });

                        if (!String.IsNullOrEmpty(searchString))
                        {
                            courses = courses.Where(s => s.courseName.ToUpper().Contains(searchString));
                        }
               
                        if (courseID != 0)
                            courses = courses.Where(s => s.CourseID == courseID);

                        if (IsActive != 99)
                            courses = courses.Where(s => s.IActiveID == IsActive);
                       

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
						uPnlNavigation.Visible = true;
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
					else
                  {
                      lblError.Text = "No record found.";
                      lblError.Visible = true;
                      gvMain.Visible = false;
                      uPnlNavigation.Visible = false;
                  }
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
            //NIELITMISContext context1 = new NIELITMISContext();
            //Int32 ID = 0;
            //if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            //{
            //    ID = Convert.ToInt32(Request.QueryString["Key"]);
            //}
            //FillCourseCategories();
            //FillCategories();
            //FillActivity();
            //var users = (from s in context1.VirtualAcademyCutOffDate
            //             where s.ID == ID
            //             select new
            //             {
            //                 ID = s.ID,
            //                 CourseCat = s.CourseCategoryID,
            //                 CourseID = s.CourseDurationID,
            //                 BatchId=s.batchID,
            //                 ActivityId = s.ActivityID,
            //                 ApplicantType = s.ApplicantTypeID,
            //                 RegStartDate = s.regnStartDate,
            //                 EffectiveDate = s.Efferctive_Date
                             
            //             }).FirstOrDefault();
            //if (users != null)
            //{
            //ddlCourseName.SelectedValue = Convert.ToInt32(users.CourseCat).ToString();
            //ddlCourseName_SelectedIndexChanged(ddlCourseName, EventArgs.Empty);
            //    Ddlcname.SelectedValue = users.CourseID.ToString();
            //    Ddlcname_SelectedIndexChanged(Ddlcname, EventArgs.Empty);
            //    ddlBatch.SelectedValue = users.BatchId.ToString();
            //    txtRegStartDate.Text = Convert.ToDateTime(users.RegStartDate).ToString("dd-MMM-yyyy");
            //    Ddlactivity.SelectedValue = users.ActivityId.ToString();
            //    //txtEffectiveDate.Text = Convert.ToDateTime(users.EffectiveDate).ToString("dd-MMM-yyyy");

            //    Ddlactivity.Enabled = false;

            //    btnMode.ViewMode = ToggleView.Mode.List;
            //    btnSave.Text = "Update";
            //    mltvTab.ActiveViewIndex = 1;
            //    pnlFilter.Visible = false;
            //    ucSearchBar.Visible = false;
            //    lblHeading.Text = "Virtual Academy Cut-Off Date Detail";
            //    ddlCourseName.Enabled = false;
            //    Ddlcname.Enabled = false;
            //    ddlBatch.Enabled = false;
            //    //Get last modified date of current record and save it in ViewState object.
            //    ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;

            //    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Virtual Academy Cut-Off Date Detail", "", ""));

            //    if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            //    {
            //        btnSave.Visible = false;
            //    }
            //}
        }
        catch (Exception ex)
        {
            throw ex;
        }
        

    }
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
    FillCourse();
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                // testing vishal
                //BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to add new record.", true);
                return;
            }
            ListItem lst = new ListItem("--Select One--", "0");
            if ((!String.IsNullOrEmpty(Request.QueryString["CourseID"])))
            {                
                ddlCourseName.Enabled = false;
            }
            else
            {            
            ddlCourseName.Enabled = true;
            }
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            lblHeading.Text = "New Virtual Academy Tentative Start Date";
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Virtual Academy Tentative Start Date", "#", ""));
        }
        else
        {            
            Response.Redirect("VirtualAcademyTentativeStartDate.aspx");           
        }
    }
    public string GetExistsRecord(string myQuery)
    {
        string result = "0";
        try
        {                   
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString);
            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            SqlCommand cmd = new SqlCommand(myQuery, conn);
            conn.Open();
            string getValue = cmd.ExecuteScalar().ToString();
            if (getValue != null)
            {
                result = getValue.ToString();
            }
            conn.Close();
            return result;
        }
        catch (Exception exx)
        {
            return result;
        }
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        l1.Visible = false;
        l2.Visible = false;
        Rvalue.Value = "X";
        lblmsg.Visible = false;
    }

    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {

            DateTime TentativeStartDate = Convert.ToDateTime(txtTentativeStartDate.Text);
            DateTime cDate = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());

            if (TentativeStartDate < cDate)
            {
                ShowAlert("Tentative Start Date must be greater or equal current Date !!");
                return;
            } 
           
            lblmsg.Visible = false;
            int MessageID = 0;
            string Choice = "NA";          
            string getValue = " SELECT courseDurationID  FROM [NIELITMIS].[dbo].[virtualAcademyTentativeStartDate] WHERE courseDurationID= '" + ddlCourseName.SelectedValue + "'";
            string CID = GetExistsRecord(getValue);
            if (CID != "0" && Rvalue.Value != "P")
            {     
                l1.Visible = true;
                l2.Visible = true;
                Rvalue.Value = "P";
                return;
            }
            Choice = RdoChoice.SelectedValue.ToString();
            if ((Choice == "1" && Rvalue.Value == "P") || (Choice == "0" && Rvalue.Value != "P"))
            {               
                string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
                using (SqlConnection Conn = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Insert_VirtualAcademyTentativeStartDate", Conn))
                    {
                        Conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@courseDurationID", ddlCourseName.SelectedValue);
                        cmd.Parameters.AddWithValue("@tentativeStartDate", Convert.ToDateTime(txtTentativeStartDate.Text));
                        cmd.Parameters.AddWithValue("@isActive", ddlIsActive.SelectedValue);
                        cmd.Parameters.AddWithValue("@Options", Choice);
                        cmd.Parameters.AddWithValue("@enterBy", loginUserNo);
                        cmd.Parameters.Add("@MessageID", SqlDbType.Int);
                        cmd.Parameters["@MessageID"].Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        MessageID = (Int32)cmd.Parameters["@MessageID"].Value;
                    }
                }
                Rvalue.Value = "X";
                if (MessageID == 9999)
                {
                    strMessage = "New record saved.";
                    Response.Redirect("VirtualAcademyTentativeStartDate.aspx?msg=" + strMessage, true);
                }
            }
            else
            {                
                lblmsg.Visible = true;               
            } 
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
                Image imgAction = (Image)e.Row.FindControl("imgAction");
                imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();

                CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    

    #region vCode
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

    protected void AllyFilter(object sender, EventArgs e)
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

    protected void ResetFilterPanel(object sender, EventArgs e)
        {
        try
            {

            ddlIsActiveF.SelectedValue = "99";
            ddlCourseNameF.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message, true);
            }
        }

    protected void btnCancel_Click(object sender, EventArgs e)
        {
        Response.Redirect("VirtualAcademyTentativeStartDate.aspx");
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

    protected void FillCourseF()
        {
        try
            {
            using (DataTable dt = FillCourseRecordsF())
                {
                if (dt.Rows.Count > 0)
                    {
                    ddlCourseNameF.DataSource = dt;
                    ddlCourseNameF.DataTextField = "Name";
                    ddlCourseNameF.DataValueField = "ID";
                    ddlCourseNameF.DataBind();
                    ddlCourseNameF.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                }
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }

    public DataTable FillCourseRecordsF()
        {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
            {
            try
                {
                using (SqlCommand cmd = new SqlCommand("GetvirtualAcademyCourseTentativeStartDateF", con))
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
                throw ex;
                }
            finally
                {
                con.Close();
                }
            }
        return myDt;
        }

    protected void FillCourse()
        {
        try
            {
            using (DataTable dt = FillCourseRecords())
                {
                if (dt.Rows.Count > 0)
                    {
                    ddlCourseName.DataSource = dt;
                    ddlCourseName.DataTextField = "CourseName";
                    ddlCourseName.DataValueField = "courseId";
                    ddlCourseName.DataBind();
                    ddlCourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                }
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }

    public DataTable FillCourseRecords()
        {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
            {
            try
                {
                using (SqlCommand cmd = new SqlCommand("GetvirtualAcademyCourseTentativeStartDate", con))
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
                throw ex;
                }
            finally
                {
                con.Close();
                }
            }
        return myDt;
        }

    #endregion 

    #region other
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
        {      
        try
            {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {            
              using (SqlCommand cmd = new SqlCommand("GetvirtualAcademyCourseTentativeStartDateF", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }

        if (myDt.Rows.Count > 0)
        {
            var courses = (from p in myDt.AsEnumerable()
                           select new
                           {
                               ID = p.Field<Int64>("ID"),
                               Name = p.Field<string>("Name")
                           });
            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
            }
           
            foreach (var user in courses)
            {
                items.Add(user.Name);
            }
        }
            return items.ToArray();
            }
    
        catch (Exception ex)
            {
            throw ex;
            }       
        }  
    #endregion
    }