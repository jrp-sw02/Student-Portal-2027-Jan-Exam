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
using System.Data.SqlClient;
using System.Configuration;

public partial class Admin_projectSubCentre : BasePage
{
    NIELITMISContext context;
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int32 NielitCentreId = 0;
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

            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                User objUser;
                using (EConnectContext context = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();
                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    RegionalCenter RegName = context.RegionalCenters.Find(loginUser.UserRefNumber);
                     NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                     hddnielitId.Value = NielitCentreId.ToString();
                   BindListDataProjName();

                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {                        
                        ShowEditMode();                  
                       // BindListDataProjName();
                    }
                    else
                    {
                        using (NIELITMISContext context1 = new NIELITMISContext())
                        {
                        BindListData();
                     
                       //BindListDataProjName();
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";
                        BindGridView();

                        if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Project SubCentre", "Admin/projectSubCentre.aspx?Id=" + Request.QueryString["Id"].ToString(), ""));
                        }
                        else
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Project SubCentre", "Admin/projectSubCentre.aspx", ""));
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

  
    protected void BindListData()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
               ListItem lst1 = new ListItem("--All--", "0");
                var projectName = from s in context.NielitProjectss 
                                  join c in context.projectSubCentres on s.ID equals c.projectID
                            select new { ValueField = s.ID, TextField = s.ProjectName };
               EConnect.Utils.Common.ControlUtility.BindListObject(ddlprojectName, projectName.Distinct(), lst1);
               
            }        
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void BindListDataProjName()
    {
        try
        {
            Int32 NielitCentreId = 0, NonAfflInst = 0;
            using (NIELITMISContext context = new NIELITMISContext())
            {
                User objUser;
                using (EConnectContext context1 = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();
                    User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    RegionalCenter RegName = context1.RegionalCenters.Find(loginUser.UserRefNumber);
                    NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                }
                ListItem lst1 = new ListItem("--Select One--", "0");
                var projectName = from s in context.NielitProjectss
                                  join p in context.projectMainCentres on s.ID equals p.projectID
                                  where p.centreID == NielitCentreId
                                  select new { ValueField = s.ID, TextField = s.ProjectName };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlProjName, projectName, lst1);
            
                ddlwhetherAffiliated.SelectedValue = "1";                
                using (DataTable dt = GetAfflOrNonAfflInstWithStateName(NielitCentreId, NonAfflInst))
                {
                    if (dt.Rows.Count > 0)
                    {
                        ddlcentreName.DataSource = dt;
                        ddlcentreName.DataTextField = "Name";
                        ddlcentreName.DataValueField = "ID";
                        ddlcentreName.DataBind();
                        ddlcentreName.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                } 
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetAfflOrNonAfflInstWithStateName(int NielitCentreId1, int NonAfflInst)
    {
       
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetAfflOrNonAfflInstWithStateName", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@linkedToCentre", SqlDbType.Int));
                    cmd.Parameters["@linkedToCentre"].Value = NielitCentreId1;
                    cmd.Parameters.Add(new SqlParameter("@NonAfflInst", SqlDbType.Int));
                    cmd.Parameters["@NonAfflInst"].Value = NonAfflInst;
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
    protected void RdoAffInstOrNonAffInst_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 NielitCentreId = 0, NonAfflInst=0;
            using (NIELITMISContext context = new NIELITMISContext())
            {
                
                 User objUser;
                 using (EConnectContext context1 = new EConnectContext())
                 {
                     objUser = new EConnect.URM.User();
                     User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                     RegionalCenter RegName = context1.RegionalCenters.Find(loginUser.UserRefNumber);
                     NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                 }
                ListItem lst1 = new ListItem("--Select One--", "0");
                if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                {
                    ddlcentreName.ClearSelection();
                    //var centreName = from s in context.AffInstitutes
                    //                 select new { ValueField = s.ID, TextField = s.Name };
                    //EConnect.Utils.Common.ControlUtility.BindListObject(ddlcentreName, centreName, lst1);
                    ddlwhetherAffiliated.SelectedValue = "1";
                }
                else
                {
                    ddlcentreName.ClearSelection();
                    NonAfflInst = 1;
                     //var centreName = from s in context.NonAffInstitutes                                    
                    //                 select new { ValueField = s.ID, TextField = s.Name };
                   // EConnect.Utils.Common.ControlUtility.BindListObject(ddlcentreName, centreName, lst1);
                    ddlwhetherAffiliated.SelectedValue = "2";                   
                }
                using (DataTable dt = GetAfflOrNonAfflInstWithStateName(NielitCentreId, NonAfflInst))
                {
                    if (dt.Rows.Count > 0)
                    {
                        ddlcentreName.DataSource = dt;
                        ddlcentreName.DataTextField = "Name";
                        ddlcentreName.DataValueField = "ID";
                        ddlcentreName.DataBind();
                        ddlcentreName.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                    else
                    {
                        ddlcentreName.ClearSelection();
                        ddlcentreName.Items.Clear();
                        ddlcentreName.Items.Insert(0, new ListItem("--Select  One--", "0"));
                    }
                }  
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }
    }
    protected void ShowEditMode()
    {
        try
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Project Sub Centre";
            tblNavLinks.Visible = true;
            
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int32 Id = Convert.ToInt32(Request.QueryString["Key"]);
                projectSubCentre editCourse = context.projectSubCentres.Find(Id);  
                if (context.projectSubCentres.Any(s => (s.ID == Id)))
                {
                    RdoAffInstOrNonAffInst.Enabled = false;
                    ddlProjName.Enabled = false;
                    ddlcentreName.Enabled = false;
                    txtallocatedFromDate.Enabled = true;
                    txtallocatedTodate.Enabled = true;                   
                    txtbudgetAllocated.Enabled = true;
                    var projectSubCentress = (from p in context.projectSubCentres
                                  where p.ID == Id
                                  select new
                                  {
                                      ID = p.ID,
                                      projectID = p.projectID,
                                      centreID = p.centreID,
                                      allocatedFromDate = p.allocatedFromDate,
                                      allocatedTodate = p.allocatedTodate,
                                      whetherAffiliated = p.whetherAffiliated,
                                      budgetAllocated = p.budgetAllocated                                     
                                  }).FirstOrDefault();

                    ddlProjName.SelectedValue = projectSubCentress.projectID.ToString();
                    ddlcentreName.SelectedValue = projectSubCentress.centreID.ToString();
                    txtallocatedFromDate.Text = projectSubCentress.allocatedFromDate.ToString("dd-MMM-yyyy");
                    txtallocatedTodate.Text = projectSubCentress.allocatedTodate.ToString("dd-MMM-yyyy");

                    if (projectSubCentress.whetherAffiliated == true)
                    {
                        ddlwhetherAffiliated.SelectedValue = "1";
                        RdoAffInstOrNonAffInst.SelectedValue = "1";
                    }
                    else
                    {
                        ddlwhetherAffiliated.SelectedValue = "2";
                        RdoAffInstOrNonAffInst.SelectedValue = "0";
                        RdoAffInstOrNonAffInst_SelectedIndexChanged(RdoAffInstOrNonAffInst.SelectedValue, EventArgs.Empty);
                    }
                    //int projectIdExists = (from b in context.projectMainCentres
                    //                       where b.projectID == Id
                    //                      select b).Count();
                    //if (projectIdExists > 0)
                    //{
                    //    ddlwhetherAffiliated.Enabled = false;
                    //}
                    //else
                    //{
                    //    ddlwhetherAffiliated.Enabled = true;
                    //}
                    txtbudgetAllocated.Text = projectSubCentress.budgetAllocated.ToString(); 
                }
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
        finally
        {
            // context.Dispose();
        }
    }
    protected void BindGridView()
    {
        try
        { 
            using (NIELITMISContext context = new NIELITMISContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                Int32 projID = 0;
                string projectName = string.Empty;
                if (String.IsNullOrEmpty(txtBudgetAllocatedSearch.Text))
                {
                    txtBudgetAllocatedSearch.Text = "0";
                }
                
                Int32 budgetAllocatedSeard = Convert.ToInt32(txtBudgetAllocatedSearch.Text);
                if (ddlprojectName.SelectedValue != "0")
                    //projID = Convert.ToInt32(ddlprojectName.SelectedValue);
                    projectName = ddlprojectName.SelectedItem.Text;
                var projectNames = from s in context.projectSubCentres
                                   join a in context.NielitProjectss on s.projectID equals a.ID
                                   join c in context.AffInstitutes on s.centreID equals c.ID
                                   where s.enterBy == loginUserNo 
                                   select new
                                   {
                                       ID = s.ID,
                                       projectName = a.ProjectName,
                                       centreName = c.Name,
                                       allocatedFromDate = s.allocatedFromDate,
                                       allocatedTodate = s.allocatedTodate,
                                       budgetAllocated = s.budgetAllocated.HasValue ? s.budgetAllocated.Value :0 ,
                                       whetherAfflInst = s.whetherAffiliated
                                   };
                var projectNames1 = from s in context.projectSubCentres
                                   join a in context.NielitProjectss on s.projectID equals a.ID
                                   join c in context.NonAffInstitutes on s.centreID equals c.ID
                                   where s.enterBy == loginUserNo 
                                   select new
                                   {
                                       ID = s.ID,
                                       projectName = a.ProjectName,
                                       centreName = c.Name,
                                       allocatedFromDate = s.allocatedFromDate,
                                       allocatedTodate = s.allocatedTodate,
                                       budgetAllocated = s.budgetAllocated,
                                       whetherAfflInst = s.whetherAffiliated
                                   };
                if (!String.IsNullOrEmpty(searchString))
                {
                    projectNames = projectNames.Where(s => s.projectName.ToUpper().Contains(searchString));
                    projectNames1 = projectNames1.Where(s => s.projectName.ToUpper().Contains(searchString));
                }


                if (!String.IsNullOrEmpty(projectName))
                {
                    projectNames = projectNames.Where(s => s.projectName == projectName);
                    projectNames1 = projectNames1.Where(s => s.projectName == projectName);
                }
                if ( !string.IsNullOrEmpty(txtBudgetAllocatedSearch.Text))
                {
                    projectNames = projectNames.Where(s => s.budgetAllocated >= budgetAllocatedSeard);
                    projectNames1 = projectNames1.Where(s => s.budgetAllocated >= budgetAllocatedSeard);

                }
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ID":
                            if (sortOrder == "DESC")
                                projectNames = projectNames.OrderByDescending(s => s.ID);
                            else
                                projectNames = projectNames.OrderBy(s => s.ID);
                            break;
                        case "projectName":
                            if (sortOrder == "DESC")
                                projectNames = projectNames.OrderByDescending(s => s.projectName);
                            else
                                projectNames = projectNames.OrderBy(s => s.projectName);
                            break;
                        default:
                            projectNames = projectNames.OrderBy(s => s.projectName);
                            break;
                    }
                }

                PagingBar1.Bind(projectNames, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                gvMain.Visible = true;
                PagingBar1.Visible = true;
                lblErrMsg.Visible = false;
                if (gvMain.Rows.Count <= 0)
                {
                    lblErrMsg.Text = "No record found for Accredited Centres.";
                    lblErrMsg.Visible = true;
                    gvMain.Visible = false;
                    PagingBar1.Visible = false;
                }
                PagingBar2.Bind(projectNames1, ref gvMainNonAflInt);
                uPnlGrid1.Update();
                uPnlNavigation1.Update();
                gvMainNonAflInt.Visible = true;
                lblErrMsg1.Visible = false;
                PagingBar2.Visible = true;
                if (gvMainNonAflInt.Rows.Count <= 0)
                {
                    lblErrMsg1.Text = "No record found for Non Accredited Institute.";
                    lblErrMsg1.Visible = true;
                    gvMainNonAflInt.Visible = false;
                    PagingBar2.Visible = false;
                }
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    gvMain.Columns[7].Visible = false;
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
            gvMainNonAflInt.PageIndex = PagingBar2.CurrentPageIndex; 
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void PageIndexChanged1(Int32 NewPageIndex)
    {
        try
        {
           // gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            gvMainNonAflInt.PageIndex = PagingBar2.CurrentPageIndex;
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
                ShowAlert("Sorry! You don't have rights to add new record.", true);
                return;
            }                    
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Updating Breadcrumb           
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New  Project Sub Centre", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("projectSubCentre.aspx?ID=" + Request.QueryString["ID"].ToString()), true);
            }
            else
            {
                Response.Redirect("projectSubCentre.aspx", true);             
            }
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            PagingBar2.CurrentPageIndex = 0;
            gvMainNonAflInt.PageIndex = PagingBar2.CurrentPageIndex;
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

            PagingBar2.CurrentPageIndex = 0;
            gvMainNonAflInt.PageIndex = PagingBar2.CurrentPageIndex;
            BindGridView();
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
            lblerror.Text = "";
            if (IsValidForm())
            {
                BreadCrumb1.Render();
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    projectSubCentre objProject;
                    Boolean whetherAffiliated = false;
                    DateTime AllocatedFrmDate, AllocatedToDate, NielitProjectFromDate, NielitProjectToDate;
                    Int32 projid = Convert.ToInt32(ddlProjName.SelectedValue);
                    AllocatedFrmDate = Convert.ToDateTime(txtallocatedFromDate.Text);
                    AllocatedToDate = Convert.ToDateTime(txtallocatedTodate.Text);
                    var ProjectDateRange = (from s in context.NielitProjectss
                                            where s.ID == projid
                                            select new
                                            {
                                                ProjectFromDate = s.projectFromDate,
                                                ProjectToDate = s.projectTodate
                                            }).FirstOrDefault();

                    NielitProjectFromDate = Convert.ToDateTime(ProjectDateRange.ProjectFromDate);
                    NielitProjectToDate = Convert.ToDateTime(ProjectDateRange.ProjectToDate);
                   // if (AllocatedFrmDate >= NielitProjectFromDate && AllocatedToDate <= NielitProjectToDate)
                    if ((AllocatedFrmDate >= NielitProjectFromDate && AllocatedFrmDate <= NielitProjectToDate) && (AllocatedToDate <= NielitProjectToDate && AllocatedToDate >= NielitProjectFromDate))
                    {
                        if (AllocatedFrmDate <= AllocatedToDate)
                        {
                            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                            {


                                objProject = new projectSubCentre();

                                objProject.projectID = Convert.ToInt32(ddlProjName.SelectedValue);
                                objProject.centreID = Convert.ToInt32(ddlcentreName.SelectedValue);
                                objProject.allocatedFromDate = Convert.ToDateTime(txtallocatedFromDate.Text);
                                objProject.allocatedTodate = Convert.ToDateTime(txtallocatedTodate.Text);
                                if (!String.IsNullOrEmpty(txtbudgetAllocated.Text))
                                {
                                    objProject.budgetAllocated = Convert.ToInt32(txtbudgetAllocated.Text);
                                }
                                if (ddlwhetherAffiliated.SelectedValue == "1")
                                {
                                    whetherAffiliated = true;
                                }
                                if (ddlwhetherAffiliated.SelectedValue == "2")
                                {
                                    whetherAffiliated = false; ;
                                }
                                objProject.whetherAffiliated = whetherAffiliated; // whetherAffiliated or not whetherAffiliated                       
                                objProject.enterDate = DateTime.Now;
                                objProject.enterBy = Convert.ToInt32(Session["UserID"]);

                                context.projectSubCentres.Add(objProject);
                                context.SaveChanges();
                                strMessage = "New record saved.";
                            }

                            else
                            {
                                objProject = new projectSubCentre();
                                objProject = context.projectSubCentres.Find(Convert.ToInt32(Request.QueryString["key"]));

                                objProject.projectID = Convert.ToInt32(ddlProjName.SelectedValue);
                                objProject.centreID = Convert.ToInt32(ddlcentreName.SelectedValue);
                                objProject.allocatedFromDate = Convert.ToDateTime(txtallocatedFromDate.Text);
                                objProject.allocatedTodate = Convert.ToDateTime(txtallocatedTodate.Text);
                                //if (!String.IsNullOrEmpty(txtbudgetAllocated.Text))
                                //{
                                //    objProject.budgetAllocated = Convert.ToInt32(txtbudgetAllocated.Text);
                                //}
                                if (txtbudgetAllocated.Text.Trim() == "")
                                {
                                    objProject.budgetAllocated = null;
                                }
                                else
                                {
                                    objProject.budgetAllocated = Convert.ToInt32(txtbudgetAllocated.Text);
                                }
                                if (ddlwhetherAffiliated.SelectedValue == "1")
                                {
                                    whetherAffiliated = true;
                                }
                                if (ddlwhetherAffiliated.SelectedValue == "2")
                                {
                                    whetherAffiliated = false; ;
                                }
                                objProject.whetherAffiliated = whetherAffiliated; // whetherAffiliated or not whetherAffiliated

                                strMessage = "Record updated.";
                                context.SaveChanges();
                            }
                            Response.Redirect("projectSubCentre.aspx?msg=" + strMessage, true);
                        }
                        else
                        {
                            strMessage = "Allocated date range should be " + NielitProjectFromDate.ToString("dd-MMM-yyyy") + " Less than " + NielitProjectToDate.ToString("dd-MMM-yyyy");
                            Response.Redirect("projectSubCentre.aspx?msg=" + strMessage, true);
                        }
                }
                    else
                    {
                        strMessage = "Allocated date range should be " + NielitProjectFromDate.ToString("dd-MMM-yyyy") + " between " + NielitProjectToDate.ToString("dd-MMM-yyyy");
                        Response.Redirect("projectSubCentre.aspx?msg=" + strMessage, true);
                        
                    }
                }
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
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;

            PagingBar2.CurrentPageIndex = 0;
            gvMainNonAflInt.PageIndex = PagingBar2.CurrentPageIndex;
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
            ddlprojectName.SelectedValue = "0";
            txtBudgetAllocatedSearch.Text = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            gvMainNonAflInt.PageIndex = PagingBar2.CurrentPageIndex;
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
    protected void gvMainNonAflInt_Sorting(object sender, GridViewSortEventArgs e)
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
    protected bool isBlank(TextBox txtBox)
    {
        try
        {
            if (txtBox.Text.Trim() == "")
            {
                txtBox.Focus();
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isSelected(DropDownList ddlProjName)
    {
        try
        {
            if (ddlProjName.SelectedValue == "0")
            {
                ddlProjName.Focus();
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool isNumber(TextBox txtBox)
    {
        try
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            if (txtBox.Text.Trim() != "")
            {
                if (!regex.IsMatch(txtBox.Text.Trim()))
                {
                    txtBox.Text = "";
                    txtBox.Focus();
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
    protected bool isValidProjectFromTo(TextBox txtprojectFrom, TextBox txtprojectto)
    {
        try
        {
            DateTime todaydate = DateTime.Now;
            DateTime Inputdate = Convert.ToDateTime(txtprojectFrom.Text);
            DateTime Outputdate = Convert.ToDateTime(txtprojectto.Text);

            int result1 = DateTime.Compare(Inputdate, Outputdate);
           
            if (result1>0)
            {
                lblerror.Text = "Project start date should be less than end date.";
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected bool isValidbudgetAllocatedInput(TextBox txtbudgetAllocated)
    {
        try
        {
            Int32 budgetAllocated = 0;
            NielitCentreId = Convert.ToInt32(hddnielitId.Value);
            Int32 projid = Convert.ToInt32(ddlProjName.SelectedValue);
            Int32 budgetAllocatedAmountInput = Convert.ToInt32(txtbudgetAllocated.Text);

            using (NIELITMISContext context = new NIELITMISContext())
            {
                var budgetAllocatedAmount = (from s in context.projectMainCentres
                                             where s.projectID == projid && s.centreID == NielitCentreId
                                        select new
                                        {
                                            budgetAllocated = s.budgetAllocated
                                            
                                        }).FirstOrDefault();

                budgetAllocated = Convert.ToInt32(budgetAllocatedAmount.budgetAllocated);
                
            }

            if ((budgetAllocatedAmountInput <= budgetAllocated) && (budgetAllocatedAmountInput >= 0))
            {
                return true;
            }
            else
            {
                lblerror.Text = "Project budgetAllocated amount should not be exceeds from Rs " + budgetAllocated.ToString() + " /- .";
                return false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected bool isValidAllocatedFromLessTo(TextBox txtprojectFrom, TextBox txtprojectto)
    {
        try
        {
            DateTime AllocatedFrmDate, AllocatedToDate, NielitProjectFromDate, NielitProjectToDate;
            Int32 projid = Convert.ToInt32(ddlProjName.SelectedValue);
            NielitCentreId = Convert.ToInt32(hddnielitId.Value);
            AllocatedFrmDate = Convert.ToDateTime(txtallocatedFromDate.Text);
            AllocatedToDate = Convert.ToDateTime(txtallocatedTodate.Text);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                var ProjectDateRange = (from s in context.projectMainCentres
                                        where s.projectID == projid && s.centreID == NielitCentreId
                                        select new
                                        {
                                            ProjectFromDate = s.allocatedFromDate,
                                            ProjectToDate = s.allocatedTodate
                                        }).FirstOrDefault();

                NielitProjectFromDate = Convert.ToDateTime(ProjectDateRange.ProjectFromDate);
                NielitProjectToDate = Convert.ToDateTime(ProjectDateRange.ProjectToDate);
            }
            if (AllocatedFrmDate <= AllocatedToDate)
            {
                return true;
            }
            else
            {

                lblerror.Text = "Allocated date range should be " + NielitProjectFromDate.ToString("dd-MMM-yyyy") + " between " + NielitProjectToDate.ToString("dd-MMM-yyyy");
                return false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected bool isValidAllocatedFromTo(TextBox txtprojectFrom, TextBox txtprojectto)
    {
        try
        {
            NielitCentreId = Convert.ToInt32( hddnielitId.Value);
            DateTime AllocatedFrmDate, AllocatedToDate, NielitProjectFromDate, NielitProjectToDate;
            Int32 projid = Convert.ToInt32(ddlProjName.SelectedValue);
            AllocatedFrmDate = Convert.ToDateTime(txtallocatedFromDate.Text);
            AllocatedToDate = Convert.ToDateTime(txtallocatedTodate.Text);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                var ProjectDateRange = (from s in context.projectMainCentres
                                        where s.projectID == projid && s.centreID == NielitCentreId
                                        select new
                                        {
                                            ProjectFromDate = s.allocatedFromDate,
                                            ProjectToDate = s.allocatedTodate
                                        }).FirstOrDefault();

                NielitProjectFromDate = Convert.ToDateTime(ProjectDateRange.ProjectFromDate);
                NielitProjectToDate = Convert.ToDateTime(ProjectDateRange.ProjectToDate);
            }
            if ((AllocatedFrmDate >= NielitProjectFromDate && AllocatedFrmDate <= NielitProjectToDate) && (AllocatedToDate <= NielitProjectToDate && AllocatedToDate >= NielitProjectFromDate))
            {               
                return true; 
            }
           else  {
              
               lblerror.Text = "Allocated date range should be " + NielitProjectFromDate.ToString("dd-MMM-yyyy") + " between " + NielitProjectToDate.ToString("dd-MMM-yyyy");
               return false;
            }           
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
            
            if(!isSelected(ddlProjName))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select the Project Name";
                return false;

            }            
            if (!isSelected(ddlcentreName))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select the Centre Name";
                return false;

            }
            if (!isSelected(ddlwhetherAffiliated))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select the Whether Affiliated";
                return false;

            }
            if (!isBlank(txtallocatedFromDate))
            {
                lblerror.Visible = true;
                lblerror.Text = "Allocated From Date can not be left blank";
                return false;
            }
            if (!isBlank(txtallocatedTodate))
            {
                lblerror.Visible = true;
                lblerror.Text = "Allocated To Date can not be left blank";
                return false;
            }           
            //if (!isBlank(txtbudgetAllocated))
            //{
            //    lblerror.Visible = true;
            //    lblerror.Text = "Budget allocated can not be left blank";
            //    return false;
            //}
            if (!isValidAllocatedFromTo(txtallocatedFromDate, txtallocatedTodate))
            {
                DateTime AllocatedFrmDate, AllocatedToDate, NielitProjectFromDate, NielitProjectToDate;
                Int32 projid = Convert.ToInt32(ddlProjName.SelectedValue);
                NielitCentreId = Convert.ToInt32(hddnielitId.Value);
                AllocatedFrmDate = Convert.ToDateTime(txtallocatedFromDate.Text);
                AllocatedToDate = Convert.ToDateTime(txtallocatedTodate.Text);
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    var ProjectDateRange = (from s in context.projectMainCentres
                                            where s.projectID == projid && s.centreID == NielitCentreId
                                            select new
                                            {
                                                ProjectFromDate = s.allocatedFromDate,
                                                ProjectToDate = s.allocatedTodate
                                            }).FirstOrDefault();

                    NielitProjectFromDate = Convert.ToDateTime(ProjectDateRange.ProjectFromDate);
                    NielitProjectToDate = Convert.ToDateTime(ProjectDateRange.ProjectToDate);
                }
                lblerror.Visible = true;
                lblerror.Text = "Allocated date range should be " + NielitProjectFromDate.ToString("dd-MMM-yyyy") + " between " + NielitProjectToDate.ToString("dd-MMM-yyyy");
                return false;
            }
            if (!isValidAllocatedFromLessTo(txtallocatedFromDate, txtallocatedTodate))
            {
                DateTime AllocatedFrmDate, AllocatedToDate, NielitProjectFromDate, NielitProjectToDate;
                Int32 projid = Convert.ToInt32(ddlProjName.SelectedValue);
                NielitCentreId = Convert.ToInt32(hddnielitId.Value);
                AllocatedFrmDate = Convert.ToDateTime(txtallocatedFromDate.Text);
                AllocatedToDate = Convert.ToDateTime(txtallocatedTodate.Text);
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    var ProjectDateRange = (from s in context.projectMainCentres
                                            where s.projectID == projid && s.centreID == NielitCentreId
                                            select new
                                            {
                                                ProjectFromDate = s.allocatedFromDate,
                                                ProjectToDate = s.allocatedTodate
                                            }).FirstOrDefault();

                    NielitProjectFromDate = Convert.ToDateTime(ProjectDateRange.ProjectFromDate);
                    NielitProjectToDate = Convert.ToDateTime(ProjectDateRange.ProjectToDate);
                }
                if (AllocatedFrmDate <= AllocatedToDate)
                {
                    return true;
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Allocated date range should be " + NielitProjectFromDate.ToString("dd-MMM-yyyy") + " between " + NielitProjectToDate.ToString("dd-MMM-yyyy");
                    return false;
                }               
            }  
            if (!String.IsNullOrEmpty(txtbudgetAllocated.Text))
            {
                if (!IsNumeric(txtbudgetAllocated.Text))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Budget allocated Not a Number,Please enter numeric value Only .";
                    txtbudgetAllocated.Focus();                  
                    return false;
                }
                if (!isValidbudgetAllocatedInput(txtbudgetAllocated))
                {
                    Int32 budgetAllocated = 0;
                    Int32 projid = Convert.ToInt32(ddlProjName.SelectedValue);
                    Int32 budgetAllocatedAmountInput = Convert.ToInt32(txtbudgetAllocated.Text);
                    NielitCentreId = Convert.ToInt32(hddnielitId.Value);
                    using (NIELITMISContext context = new NIELITMISContext())
                    {
                        var budgetAllocatedAmount = (from s in context.projectMainCentres
                                                     where s.projectID == projid && s.centreID== NielitCentreId
                                                     select new
                                                     {
                                                         budgetAllocated = s.budgetAllocated

                                                     }).FirstOrDefault();

                        budgetAllocated = Convert.ToInt32(budgetAllocatedAmount.budgetAllocated);

                    }
                    if ((budgetAllocatedAmountInput <= budgetAllocated) && (budgetAllocatedAmountInput >= 0))
                    {
                        return true;
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Project budgetAllocated amount should not be exceeds from Rs " + budgetAllocated.ToString() + " /- .";
                        txtbudgetAllocated.Focus();
                        return false;                      
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
                NIELITMISContext context1 = new NIELITMISContext();
                NielitProjects NielitProjects = context1.NielitProjectss.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                context1.NielitProjectss.Remove(NielitProjects);
                context.SaveChanges();
                BindGridView();
                ShowAlert("Record deleted successfully.", true);
                hfActionID.Value = "";
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
    protected void PerformPopupAction1(object sender, EventArgs e)
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
                NIELITMISContext context1 = new NIELITMISContext();
                NielitProjects NielitProjects = context1.NielitProjectss.Find(Convert.ToInt32(hfActionID1.Value.ToString()));
                context1.NielitProjectss.Remove(NielitProjects);
                context.SaveChanges();
                BindGridView();
                ShowAlert("Record deleted successfully.", true);
                hfActionID1.Value = "";
            };
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            BindGridView();
            uPnlGrid1.Update();
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
    protected void gvMainNonAflInt_RowDataBound(object sender, GridViewRowEventArgs e)
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
                //HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                //h2.NavigateUrl = hl.NavigateUrl;
                //HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                //h3.NavigateUrl = hl.NavigateUrl;
                //HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                //h4.NavigateUrl = hl.NavigateUrl;

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar2.CurrentPageSize * PagingBar2.CurrentPageIndex)).ToString();

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

            var ProjectNameS = from s in context.NielitProjectss
                               join  c in context.projectSubCentres on s.ID equals c.projectID                              
                             select new { Name = s.ProjectName };
            if (!String.IsNullOrEmpty(searchString))
            {
                ProjectNameS = ProjectNameS.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            ProjectNameS = ProjectNameS.Distinct().OrderBy(s => s.Name);
           // ProjectNameS = ProjectNameS.OrderBy(s => s.Name);           
            foreach (var c in ProjectNameS)
            {
                items.Add(c.Name);
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
        Response.Redirect("projectSubCentre.aspx", true);

    }  
}