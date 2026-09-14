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

public partial class Admin_VirtualAcademyCutOffDate : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 loginUserNoForCenterID = 0;

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
                    FillCourseCategories();
                    FillCategories();
                    FillActivity();
                
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                     BindGridView();
                    if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Virtual Academy Cut Off Date", "Admin/VirtualAcademyCutOffDate.aspx?CourseId=" + Request.QueryString["CourseId"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Virtual Academy Cut Off Date", "Admin/VirtualAcademyCutOffDate.aspx", ""));
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

    protected void FillCourseCategories()
    {
        try
        {
           //NIELITMISContext context1 = new NIELITMISContext();
          //Ddlccat.Items.Clear();
           //using (EConnectContext context = new EConnectContext())
            //{
                //ListItem lst = new ListItem("--Select One--", "0");
                //var Category = from p in context1.NielitCentreCourseCategorys
                //               where p.IsActive == true
                //               orderby (p.Name)
                //               select new { ValueField = p.ID, TextField = p.Name };

           //     EConnect.Utils.Common.ControlUtility.BindListObject(Ddlccat, Category, lst);
           // };
           
        	//deep add code on 25 may 2022 and comment above all Line
            using (DataTable dt = FillCourseCategoryRecords())
            {
                if (dt.Rows.Count > 0)
                {
                    Ddlccat.DataSource = dt;
                    Ddlccat.DataTextField = "Name";
                    Ddlccat.DataValueField = "ID";
                    Ddlccat.DataBind();
                    Ddlccat.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
            }
            //deep add End code on 25 may 2022 and comment above all Line
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
//deep add code on 25 may 2022
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
                    Ddlcname.DataSource = dt;
                    Ddlcname.DataTextField = "Name";
                    Ddlcname.DataValueField = "ID";
                    Ddlcname.DataBind();
                    Ddlcname.Items.Insert(0, new ListItem("--Select One--", "0"));
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
    //deep add End code on 25 may 2022
    protected void FillCategories()
    {
        try
        {
            NIELITMISContext context1 = new NIELITMISContext();
            ddlcoursecategoryF.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context1.NielitCentreCourseCategorys
                               where p.IsActive == true
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategoryF, Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillActivity()
    {
        try
        {           
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.ExamActivities                              
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(Ddlactivity, Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlcoursecategoryF_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 coursecatID = Convert.ToInt32(ddlcoursecategoryF.SelectedValue);
            using (NIELITMISContext context1 = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context1.NielitCentreCourses
                                 join d in context1.NielitCourseDurations on p.ID equals d.courseID
                                 where p.CourseCategoryID == coursecatID && p.IsActive == true
                                 orderby (p.Name)
                                  select new { ValueField = d.ID, TextField = p.Name + " (" + p.Code + ")" + " (" + d.courseDurationDays + "Days" + ")" + " (" + d.courseDurationHrs + "Hours" + ")" };
                                
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseNameF, CourseList, lst);

            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    
    protected void Ddlcname_SelectedIndexChanged(object sender, EventArgs e)
    {       
        Int64 Courseid = 0;

        ddlBatch.ClearSelection();

        ddlBatch.Items.Clear();

        try
        {
            //subcentreid = Convert.ToInt64(ddlCenter.SelectedValue);
            Courseid = Convert.ToInt64(Ddlcname.SelectedValue);

            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Batch = from s in context.NielitCentreBatchs
                            where s.IsVerified == true // && s.centreID == subcentreid && s.subCentreID == 0
                                    && s.CourseDurationID == Courseid && s.learningModeID == 7
                            orderby (s.Name)
                            select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    } 
   
    public DataTable FillGridViewVirtualAcademyCutOffDate()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
		using (EConnectContext context = new EConnectContext())
                {
                   // loginUserNo = 288893; // for testing purpose               
                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    loginUserNoForCenterID = Convert.ToInt32(loginUser.UserRefNumber.ToString());
                }

                using (SqlCommand cmd = new SqlCommand("getVirtualAcdCutOffDateDetails", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pCentreID", SqlDbType.BigInt);
                    cmd.Parameters["@pCentreID"].Value = loginUserNoForCenterID; 
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
            Int32 CourseCatID = 0;
            Int32 courseID = 0;
            
            if (ddlcoursecategoryF.SelectedValue != "0")
                CourseCatID = Convert.ToInt32(ddlcoursecategoryF.SelectedValue);

            if (ddlCourseNameF.SelectedValue != "0")
                courseID = Convert.ToInt32(ddlCourseNameF.SelectedValue);
           
           NIELITMISContext context1 = new NIELITMISContext();
           string searchString = ucSearchBar.SearchText.Trim().ToUpper();
           string sortOrder = ViewState["SortOrder"].ToString();
           string sortField = ViewState["SortField"].ToString();

              using (DataTable dt = FillGridViewVirtualAcademyCutOffDate())

                    if (dt.Rows.Count > 0)
                    {
                        var courses = (from p in dt.AsEnumerable()
                                       select new
                                       {
                                           ID = p.Field<int>("ID"),
                                           coursecatName = p.Field<string>("coursecatName"),
                                           Name = p.Field<string>("Name"),
                                           ActivityName = p.Field<string>("ActivityName"),
                                           ApplicantName = p.Field<string>("ApplicantName"),
                                           regnStartDate = p.Field<string>("regnStartDate"),
                                           Efferctive_Date = p.Field<string>("Efferctive_Date"),
                                           Course_Category_ID = p.Field<int>("Course_Category_ID"),
                                           CourseID = p.Field<int>("CourseDurationID")
                                       });
            
                if (!String.IsNullOrEmpty(searchString))
                        {
                            courses = courses.Where(s => s.ActivityName.ToUpper().Contains(searchString));
                        }
                if (CourseCatID != 0)
                    courses = courses.Where(s => s.Course_Category_ID == CourseCatID);
                if (courseID != 0)
                    courses = courses.Where(s => s.CourseID == courseID);

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
            NIELITMISContext context1 = new NIELITMISContext();
            Int32 ID = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                ID = Convert.ToInt32(Request.QueryString["Key"]);
            }
            FillCourseCategories();
            FillCategories();
            FillActivity();
            var users = (from s in context1.VirtualAcademyCutOffDate
                         where s.ID == ID
                         select new
                         {
                             ID = s.ID,
                             CourseCat = s.CourseCategoryID,
                             CourseID = s.CourseDurationID,
                             BatchId=s.batchID,
                             ActivityId = s.ActivityID,
                             ApplicantType = s.ApplicantTypeID,
                             RegStartDate = s.regnStartDate,
                             EffectiveDate = s.Efferctive_Date
                             
                         }).FirstOrDefault();
            if (users != null)
            {
                Ddlccat.SelectedValue = Convert.ToInt32(users.CourseCat).ToString();
                Ddlccat_SelectedIndexChanged(Ddlccat, EventArgs.Empty);
                Ddlcname.SelectedValue = users.CourseID.ToString();
                Ddlcname_SelectedIndexChanged(Ddlcname, EventArgs.Empty);
                ddlBatch.SelectedValue = users.BatchId.ToString();
                txtRegStartDate.Text = Convert.ToDateTime(users.RegStartDate).ToString("dd-MMM-yyyy");
                Ddlactivity.SelectedValue = users.ActivityId.ToString();
                txtEffectiveDate.Text = Convert.ToDateTime(users.EffectiveDate).ToString("dd-MMM-yyyy");

                Ddlactivity.Enabled = false;

                btnMode.ViewMode = ToggleView.Mode.List;
                btnSave.Text = "Update";
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                lblHeading.Text = "Virtual Academy Cut-Off Date Detail";
                Ddlccat.Enabled = false;
                Ddlcname.Enabled = false;
                ddlBatch.Enabled = false;
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;

                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Virtual Academy Cut-Off Date Detail", "", ""));

                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    btnSave.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        

    }
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to add new record.", true);
                return;
            }
           // PopulateDataListMode();
            ListItem lst = new ListItem("--Select One--", "0");
            if ((!String.IsNullOrEmpty(Request.QueryString["ExamID"])) && (!String.IsNullOrEmpty(Request.QueryString["CourseID"])))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Course currentcourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["CourseID"].ToString()));
                    Ddlccat.SelectedValue = currentcourse.CourseCategoryID.ToString();
                };
                Ddlcname.SelectedValue = Request.QueryString["CourseID"].ToString();
                Ddlccat.Enabled = false;
                Ddlcname.Enabled = false;
            }
            else
            {
                Ddlccat.Enabled = true;
                Ddlcname.Enabled = true;
            }
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            lblHeading.Text = "New Virtual Academy Cut-Off Date";
            //Updating Breadscrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Virtual Academy Cut-Off Date", "#", ""));
        }
        else
        {
            if ((!String.IsNullOrEmpty(Request.QueryString["ExamID"])) && (!String.IsNullOrEmpty(Request.QueryString["CourseID"])))
            {
                Response.Redirect("VirtualAcademyCutOffDate.aspx?ExamID=" + Request.QueryString["ExamID"] + "&CourseID=" + Request.QueryString["CourseID"]);
            }
            else
            {
                Response.Redirect("VirtualAcademyCutOffDate.aspx");
            }
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
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            context = new EConnectContext();
            NIELITMISContext context1 = new NIELITMISContext();
            VirtualAcademyCutOffDate cutDate = new VirtualAcademyCutOffDate();



            Int32 batchID = Convert.ToInt32(ddlBatch.SelectedValue);
            NielitCentreBatch BatchDetails;
            BatchDetails = context1.NielitCentreBatchs.Find(batchID);
            DateTime batchStartdate = BatchDetails.startDate;
            DateTime regStartDate = Convert.ToDateTime(txtRegStartDate.Text);
            DateTime cutdateTo = Convert.ToDateTime(txtEffectiveDate.Text);
            DateTime extdate = cutdateTo.AddDays(5);
            if (regStartDate > cutdateTo)
            {
                ShowAlert("End Date must be greater than start date");
                return;
            }





            if (extdate > batchStartdate)
            {
                ShowAlert("Batch Start date is " + batchStartdate.ToString("dd-MMM-yyyy") + " .Cut off date should be up to max 5 days before batch start date.");
                return;
            }           

            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {

            
                cutDate.CourseCategoryID = Convert.ToInt32(Ddlccat.SelectedValue);
                cutDate.CourseDurationID = Convert.ToInt32(Ddlcname.SelectedValue);
                cutDate.batchID = Convert.ToInt32(ddlBatch.SelectedValue);
                cutDate.ActivityID = Convert.ToInt32(Ddlactivity.SelectedValue);

                if (context1.VirtualAcademyCutOffDate.Any(c => c.CourseDurationID == cutDate.CourseCategoryID && c.batchID==cutDate.batchID && c.CourseCategoryID == cutDate.CourseCategoryID && c.ActivityID == cutDate.ActivityID))
                {
                    throw new Exception("Duplicate record not allowed");
                }
                cutDate.ApplicantTypeID = 2;
                cutDate.regnStartDate = Convert.ToDateTime(txtRegStartDate.Text);
                cutDate.Efferctive_Date = Convert.ToDateTime(txtEffectiveDate.Text);

                cutDate.enterDate = DateTime.Now;
                cutDate.enterByID = loginUserNo;

                context1.VirtualAcademyCutOffDate.Add(cutDate);
                context1.SaveChanges();
                strMessage = "New record saved.";
            }
            else
            {
                cutDate = context1.VirtualAcademyCutOffDate.Find(Convert.ToInt32(Request.QueryString["Key"]));
                cutDate.CourseCategoryID = Convert.ToInt32(Ddlccat.SelectedValue);
                cutDate.CourseDurationID = Convert.ToInt32(Ddlcname.SelectedValue);
                cutDate.ActivityID = Convert.ToInt32(Ddlactivity.SelectedValue);
                cutDate.regnStartDate = Convert.ToDateTime(txtRegStartDate.Text);
                cutDate.Efferctive_Date = Convert.ToDateTime(txtEffectiveDate.Text);

                cutDate.enterDate = DateTime.Now;
                cutDate.enterByID = loginUserNo;
                context1.SaveChanges();
                strMessage = "Record updated.";
            }
            Response.Redirect("VirtualAcademyCutOffDate.aspx?msg=" + strMessage, true);
            
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }

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
           
                ddlcoursecategoryF.SelectedValue = "0";
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
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl);
                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);
                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);
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
            var users = from s in context.CutOffDates
                        select new { Name = s.Activity.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            users = users.OrderBy(s => s.Name).Distinct();
            foreach (var user in users)
            {
                items.Add(user.Name);
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
       
            Response.Redirect("VirtualAcademyCutOffDate.aspx");

    }
   
    protected void Ddlccat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            //Int32 coursecatID = Convert.ToInt32(Ddlccat.SelectedValue);
            //using (NIELITMISContext context1 = new NIELITMISContext())
            //{
             //    ListItem lst = new ListItem("--Select One--", "0");
                //    var CourseList = from p in context1.NielitCentreCourses
                 //                join d in context1.NielitCourseDurations on p.ID equals d.courseID
                 //                where p.CourseCategoryID == coursecatID && p.IsActive == true
                 //                orderby (p.Name)
                 //                select new { ValueField = d.ID, TextField = p.Name + " (" + p.Code + ")" + " (" + d.courseDurationDays + "Days" + ")" + " (" + d.courseDurationHrs + "Hours" + ")" };
              //  EConnect.Utils.Common.ControlUtility.BindListObject(Ddlcname, CourseList, lst);
            
            //};
                       
            //deep add code on 25 may 2022 and comment above Line OF CODE
            FillCourse(Convert.ToInt32(Ddlccat.SelectedValue));
            //deep add End code on 25 may 2022 
            
           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void PerformPopupAction(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to delete the records.", true);
                    return;
                }
                CutOffDate cutoffdates = context.CutOffDates.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                context.CutOffDates.Remove(cutoffdates);
                context.SaveChanges();
                if ((!String.IsNullOrEmpty(Request.QueryString["CourseID"])))
                {
                    Response.Redirect("VirtualAcademyCutOffDate.aspx?CourseID=" + Request.QueryString["CourseID"]);
                }
                else
                {
                    Response.Redirect("VirtualAcademyCutOffDate.aspx");
                }
            };
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be deleted!", true);
        }
    }
    
   
}