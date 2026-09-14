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

public partial class Admin_VirtualAcademyCourseServiceMasterForm : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
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
                    ddlServiceID.Enabled = false;
                    txtServiceId.Enabled = true;
                   // txtServiceId.Text = "VAREGNO";
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                     BindGridView();
                    if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Virtual Academy Course Service MasterForm", "Admin/VirtualAcademyCourseServiceMasterForm.aspx?CourseId=" + Request.QueryString["CourseId"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Virtual Academy Course Service MasterForm", "Admin/VirtualAcademyCourseServiceMasterForm.aspx", ""));
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
            Ddlccat.Items.Clear();           
           
            using (NIELITMISContext context = new NIELITMISContext())
            {                
                using (DataTable dt = GetCourseCategoryMISAndNielit())
                {
                    if (dt.Rows.Count > 0)
                    {
                        Ddlccat.DataSource = dt;
                        Ddlccat.DataTextField = "CourseCatName";
                        Ddlccat.DataValueField = "ID";
                        Ddlccat.DataBind();
                        Ddlccat.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillCategories()
    {
        try
        {           
            ddlcoursecategoryF.Items.Clear();
            using (NIELITMISContext context = new NIELITMISContext())
            {  
                using (DataTable dt = GetCourseCategoryMISAndNielit())
                {

                    if (dt.Rows.Count > 0)
                    {
                        ddlcoursecategoryF.DataSource = dt;
                        ddlcoursecategoryF.DataTextField = "CourseCatName";
                        ddlcoursecategoryF.DataValueField = "ID";
                        ddlcoursecategoryF.DataBind();
                        ddlcoursecategoryF.Items.Insert(0, new ListItem("--Select One--", "0")); 
                    }
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetCourseCategoryMISAndNielit()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("VAF_GetCourseCategoryNIELITMISCourseCatNielitCourseCatRecord", con))
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
                                  select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" + " (" + d.courseDurationDays + "Days" + ")" + " (" + d.courseDurationHrs + "Hours" + ")" };
                                
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
       

        try
        {
            //subcentreid = Convert.ToInt64(ddlCenter.SelectedValue);
            Courseid = Convert.ToInt64(Ddlcname.SelectedValue);

            //using (NIELITMISContext context = new NIELITMISContext())
            //{
            //    ListItem lst = new ListItem("--Select One--", "0");
            //    var Batch = from s in context.NielitCentreBatchs
            //                where s.IsVerified == true // && s.centreID == subcentreid && s.subCentreID == 0
            //                        && s.CourseDurationID == Courseid && s.learningModeID == 6
            //                orderby (s.Name)
            //                select new { ValueField = s.ID, TextField = s.Name };
            //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
            //};
        }
        catch (Exception ex)
        {
            throw ex;
        }
    } 

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //subcentreid = Convert.ToInt64(ddlCenter.SelectedValue);
            int ServiceID = Convert.ToInt32(ddlServiceID.SelectedValue);

            if (ServiceID == 1)
            {
                lblservice.Text = "Registration Service ID";
                txtServiceId.Focus();
               
                txtServiceId.Enabled = true;               
            }
            else if (ServiceID == 2)
            {
                lblservice.Text = "Examination Service ID";
                txtServiceId.Focus();
                txtServiceId.Enabled = true;               
            }
            else
            {
                lblservice.Text = "";
                txtServiceId.Enabled = false;
            }
                    }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable Fill_VAF_BindCourseSerivce()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("VAF_BindCourseSerivce", con))
                {                   
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

           using (DataTable dt = Fill_VAF_BindCourseSerivce())
           {
               if (dt.Rows.Count > 0)
               {
                   var courses = (from p in dt.AsEnumerable()
                                  select new
                                  {
                                      ID = p.Field<int>("ID"),
                                      CourseID=p.Field<int>("CourseID"),
                                      Course_Category_ID=p.Field<int>("Course_Category_ID"),
                                      coursecatName = p.Field<string>("Name"),
                                      CourseName = p.Field<string>("CourseName"),
                                      Registration_ServiceID = p.Field<string>("Registration_ServiceID"),
                                      Examination_ServiceID = p.Field<string>("Examination_ServiceID")

                                  });

                   // Deep add on 11 MAY 2022 START
                   if (CourseCatID != 0 && courseID != 0)
                   {
                       courses = courses.Where(s => s.Course_Category_ID == CourseCatID && s.CourseID == courseID);
                   }

                   if (!String.IsNullOrEmpty(searchString))
                   {
                       courses = courses.Where(s => s.CourseName.ToUpper().Contains(searchString) || s.Registration_ServiceID.ToUpper().Contains(searchString));
                   }
                   //// Deep add on 11 MAY 2022 END
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
               }
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
            ddlServiceID.Enabled = false;
            txtServiceId.Enabled = false;
            NIELITMISContext context1 = new NIELITMISContext();
            Int32 ID = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                ID = Convert.ToInt32(Request.QueryString["Key"]);
            }
            FillCourseCategories();
            FillCategories();
            var courses = (from s in context1.VirtualAcademyCourseServiceIDs
                         where s.CourseTypeIID == ID
                         select new
                         {
                             ID = s.ID,
                             CourseCat = s.CourseCategoryIID,
                             CourseID = s.CourseTypeIID,
                             ServiceIdR = !string.IsNullOrEmpty(s.RegistrationServiceID),
                             RegServiceID=s.RegistrationServiceID
                             //ServiceIdE = !string.IsNullOrEmpty(s.ExaminationServiceID) 
                         }).FirstOrDefault();

            if (courses != null)
            {
                Ddlccat.SelectedValue = Convert.ToInt32(courses.CourseCat).ToString();
                Ddlccat_SelectedIndexChanged(Ddlccat, EventArgs.Empty);
                Ddlcname.SelectedValue = courses.CourseID.ToString();
                Ddlcname_SelectedIndexChanged(Ddlcname, EventArgs.Empty);
                if (courses.ServiceIdR.ToString() != "NA")
                {
                    ddlServiceID.SelectedValue = "1";
                    lblservice.Text = "Registration Service ID";
                    txtServiceId.Text = courses.RegServiceID.ToString();
                    txtServiceId.Enabled = false;

                }
                //if (courses.ServiceIdE.ToString() != "false")
                //{
                //    ddlServiceID.SelectedValue = "2";
                //    lblservice.Text = "Examination Service ID";
                //    txtServiceId.Text = courses.ServiceIdE.ToString();
                //}
                btnMode.ViewMode = ToggleView.Mode.List;
                btnSave.Text = "Update";
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                lblHeading.Text = "Virtual Academy Course Services Detail";
                Ddlccat.Enabled = false;
                Ddlcname.Enabled = false;
                ddlServiceID.Enabled = false;
                btnSave.Visible = false;
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; 

                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Virtual Academy Course Services Detail", "", ""));

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
            lblHeading.Text = "New Virtual Academy Course Service";
            //Updating Breadscrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Virtual Academy Course Service", "#", ""));
        }
        else
        {
            if ((!String.IsNullOrEmpty(Request.QueryString["ExamID"])) && (!String.IsNullOrEmpty(Request.QueryString["CourseID"])))
            {
                Response.Redirect("VirtualAcademyCourseServiceMasterForm.aspx?ExamID=" + Request.QueryString["ExamID"] + "&CourseID=" + Request.QueryString["CourseID"]);
            }
            else
            {
                Response.Redirect("VirtualAcademyCourseServiceMasterForm.aspx");
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
            using (NIELITMISContext context = new NIELITMISContext())
            {
                VirtualAcademyCourseServiceID ccs = new VirtualAcademyCourseServiceID();
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    int ccat = Convert.ToInt32(Ddlccat.SelectedValue);
                    int cc = Convert.ToInt32(Ddlcname.SelectedValue);
                    string servicesid = txtServiceId.Text;
                    ccs.CourseCategoryIID= Convert.ToInt32(Ddlccat.SelectedValue);
                    ccs.CourseTypeIID = Convert.ToInt32(Ddlcname.SelectedValue);

                    if (context.VirtualAcademyCourseServiceIDs.Any(c => c.CourseCategoryIID == ccat && c.CourseTypeIID == cc && c.RegistrationServiceID == servicesid ))
                    {
                        throw new Exception("Duplicate record not allowed");
                    }
                    if (context.VirtualAcademyCourseServiceIDs.Any(c => c.CourseCategoryIID == ccat && c.CourseTypeIID == cc ))
                    {
                        throw new Exception("Duplicate record not allowed");
                    }
                    ccs.RegistrationServiceID = txtServiceId.Text.ToUpper();
                    ccs.Name=Ddlcname.SelectedItem.Text;
                    ccs.IsActive=true;                    
                    ccs.enterDate = DateTime.Now;
                    ccs.enterBy = loginUserNo;
                    context.VirtualAcademyCourseServiceIDs.Add(ccs);
                    context.SaveChanges();
                    strMessage = "New record saved.";
                }               
            };
            Response.Redirect("VirtualAcademyCourseServiceMasterForm.aspx?msg=" + strMessage, true);
            
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
        NIELITMISContext context = new NIELITMISContext();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            // Deep add on 11 MAY 2022 START
            var courses = from s in context.VirtualAcademyCourseServiceIDs
                          join d in context.NielitCentreCourses on s.CourseTypeIID equals d.ID
                          select new { Name = d.Name };

            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            courses = courses.OrderBy(s => s.Name).Distinct();

            var ServiceID = from s in context.VirtualAcademyCourseServiceIDs
                           join d in context.NielitCentreCourses on s.CourseTypeIID equals d.ID
                           select new { Name = s.RegistrationServiceID };

            if (!String.IsNullOrEmpty(searchString))
            {
                ServiceID = ServiceID.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            courses = courses.Union(ServiceID).Take(count);

            foreach (var course in courses)
            {
                items.Add(course.Name);
            }
            // Deep add on 11 MAY 2022 END
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

        Response.Redirect("VirtualAcademyCourseServiceMasterForm.aspx");

    }
   
    protected void Ddlccat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 coursecatID = Convert.ToInt32(Ddlccat.SelectedValue);
            if (coursecatID.ToString().Length > 2)
            {
                using (NIELITMISContext context1 = new NIELITMISContext())
                {
                    ListItem lst = new ListItem("--Select One--", "0");
                    var CourseList = from p in context1.NielitCentreCourses
                                     join d in context1.NielitCentreCourseCategorys on p.CourseCategoryID equals d.ID
                                     where d.ID == coursecatID && p.IsActive == true
                                     orderby (p.Name)
                                     select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(Ddlcname, CourseList, lst);

                };
            }
            if (coursecatID.ToString().Length <= 2)
            {
                using (EConnectContext context1 = new EConnectContext())
                {
                    ListItem lst = new ListItem("--Select One--", "0");
                    var CourseList = from p in context1.Courses
                                     join d in context1.CourseCategories on p.CourseCategoryID equals d.ID
                                     where d.ID == coursecatID && p.IsActive == true
                                     orderby (p.Name)
                                     select new { ValueField = p.ID, TextField = p.Name};
                    EConnect.Utils.Common.ControlUtility.BindListObject(Ddlcname, CourseList, lst);

                };
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }    
}