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

public partial class Admin_NielitProjects : BasePage
{
    NIELITMISContext context;
    String strMessage = string.Empty;
    //EConnectContext context = new EConnectContext();
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
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

          /*  if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }*/
            if (!Page.IsPostBack)
            {
                User objUser;
                using (EConnectContext context = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();
                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    RegionalCenter RegName = context.RegionalCenters.Find(loginUser.UserRefNumber);
                                     
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        BindEditNewModeData();
                        ShowEditMode();
                    }
                    else
                    {
                        using (NIELITMISContext context1 = new NIELITMISContext())
                        {
                        BindListData();
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";
                        BindGridView();

                        if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Projects List", "Admin/NielitProjects.aspx?Id=" + Request.QueryString["Id"].ToString(), ""));
                        }
                        else
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Projects List", "Admin/NielitProjects.aspx", ""));
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

    protected void BindEditNewModeData()
    {
        try
        {    
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var projects = from p in context.NielitProjectss                             
                               orderby (p.ProjectName)
                               select new { ValueField = p.ID, TextField = p.ProjectName }; 
            }
        }
        catch (Exception ex)
        {
            throw ex;
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
                            select new { ValueField = s.ID, TextField = s.ProjectName };
               EConnect.Utils.Common.ControlUtility.BindListObject(ddlprojectName, projectName, lst1);
               
            }        
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlisAadharAuthenticationReqd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlisAadharAuthenticationReqd.SelectedValue == "1")
            {
                
                lblAadharOrderNo.Visible = true;
                txtAadharOrderNumber.Visible = true;
                txtAadharOrderDate.Visible = true;
                txtAadharOrderDate.Text = "";
                txtAadharOrderNumber.Text = "";
                img1.Visible = true;
                Label4.Visible = true;
            }
            else
                
            {
                lblAadharOrderNo.Visible = false;
                txtAadharOrderNumber.Visible = false;
                txtAadharOrderDate.Visible = false;
                img1.Visible = false;
                Label4.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
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
            lblHeading.Text = "NIELIT Projects";
            tblNavLinks.Visible = true;
            
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int32 Id = Convert.ToInt32(Request.QueryString["Key"]);
                if (context.NielitProjectss.Any(s => (s.ID == Id)))
                {
                    txtProjectName.Enabled = true;
                    txtProjectDescription.Enabled = true;
                    txtstartDate.Enabled = true;
                    txtendDate.Enabled = true;
                    txtprojectDurationInMonths.Enabled = true;
                    txtfundedByOrg.Enabled = true;
                    txtbudgetAllocated.Enabled = true;
                    txtSchemeCode.Enabled = true;

                    var NielitProjects = (from p in context.NielitProjectss
                                       where p.ID == Id
                                       select p).FirstOrDefault();

                    txtSchemeCode.Text = NielitProjects.schemeCode;
                    txtProjectName.Text = NielitProjects.ProjectName.ToString();
                    txtProjectDescription.Text = NielitProjects.projectDescription.ToString();
                    txtstartDate.Text = NielitProjects.projectFromDate.ToString("dd-MMM-yyyy");
                    txtendDate.Text = NielitProjects.projectTodate.ToString("dd-MMM-yyyy");
                    txtprojectDurationInMonths.Text =NielitProjects.projectDurationMonths.ToString();                  
                    if (NielitProjects.fundedByOrg != null)
                    {
                        txtfundedByOrg.Text = NielitProjects.fundedByOrg.ToString();
                    }
                    if (NielitProjects.isAadharAuthenticationReqd == true)
                    {
                        ddlisAadharAuthenticationReqd.SelectedValue = "1";

                        lblAadharOrderNo.Visible = true;
                        txtAadharOrderNumber.Visible = true;
                        txtAadharOrderDate.Visible = true;
                        DateTime aadharorderdate = Convert.ToDateTime( NielitProjects.aadharOrderDate);
                        txtAadharOrderDate.Text = aadharorderdate.ToString("dd-MMM-yyyy");
                        txtAadharOrderNumber.Text = NielitProjects.aadharOrderNo;
                        img1.Visible = true;
                        Label4.Visible = true;
                    }
                    else
                    {
                        
                        ddlisAadharAuthenticationReqd.SelectedValue = "2";
                        lblAadharOrderNo.Visible = false;
                        txtAadharOrderNumber.Visible = false;
                        txtAadharOrderDate.Visible = false;
                        img1.Visible = false;
                        Label4.Visible = false;
                    }
                    int projectIdExists = (from b in context.projectMainCentres
                                           where b.projectID == Id
                                          select b).Count();
                    if (projectIdExists > 0)
                    {
                        ddlisAadharAuthenticationReqd.Enabled = false;
                        txtSchemeCode.Enabled = false;
                    }
                    else
                    {
                        ddlisAadharAuthenticationReqd.Enabled = true;
                        txtSchemeCode.Enabled = true;
                    }
                    txtbudgetAllocated.Text = NielitProjects.budgetAllocated.ToString(); 
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
                if (String.IsNullOrEmpty(txtBudgetAllocatedSearch.Text))
                {
                    txtBudgetAllocatedSearch.Text = "0";
                }
                
                Int32 budgetAllocatedSeard = Convert.ToInt32(txtBudgetAllocatedSearch.Text);              
                if (ddlprojectName.SelectedValue != "0")
                    projID = Convert.ToInt32(ddlprojectName.SelectedValue);
                var projectNames = from s in context.NielitProjectss
                                   //where s.enterBy == loginUserNo 
                                 select new
                                 {
                                     ID = s.ID,
                                     schemeCode=s.schemeCode,
                                     projectName = s.ProjectName,
                                     projectDescription = s.projectDescription,
                                     projectFromDate =  s.projectFromDate,
                                     projectTodate = s.projectTodate,                                    
                                     fundedByOrg = s.fundedByOrg ?? "-----", 
                                     budgetAllocated=s.budgetAllocated,
                                     AadharOrderNumber = s.aadharOrderNo ?? "----------",
                                     AadharOrderDate = s.aadharOrderDate,
                                     
                                 };



                if (!String.IsNullOrEmpty(searchString))
                {
                    projectNames = projectNames.Where(s => s.projectName.ToUpper().Contains(searchString) || s.fundedByOrg.ToUpper().Contains(searchString));
                }


                if (projID != 0)
                {
                    projectNames = projectNames.Where(s => s.ID == projID);
                }
                if ((projID == 0) && !string.IsNullOrEmpty(txtBudgetAllocatedSearch.Text))
                {
                    projectNames = projectNames.Where(s => s.budgetAllocated >= budgetAllocatedSeard);

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
           /* if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to add new record.", true);
                return;
            }*/
            BindEditNewModeData();
          
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
           // lblHeading.Text = "NIELIT Projects";
            //Updating Breadcrumb           
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New NIELIT Projects", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitProjects.aspx?ID=" + Request.QueryString["ID"].ToString()), true);
            }
            else
            {
                Response.Redirect("NielitProjects.aspx", true);             
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
    protected void SaveRecord(object sender, EventArgs e)
    {        
        try
        {
            if (IsValidForm())
            {
                BreadCrumb1.Render();
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    NielitProjects objProject;
                    Boolean isAadharAuthenticationReqd = false;
                    string ProjectDescription = txtProjectDescription.Text;

                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        objProject = new NielitProjects();

                     var  objProject1 =(from s in context.NielitProjectss
                                    where s.ProjectName.Trim() == txtProjectName.Text.Trim()
                                        select s);
                     if (objProject1.Count () >0)
                     {
                         ShowAlert("Project name already exists");
                         return;
                     }
                        objProject.schemeCode = txtSchemeCode.Text;
                        objProject.ProjectName = txtProjectName.Text;
                        objProject.projectDescription = txtProjectDescription.Text;
                        objProject.projectFromDate = Convert.ToDateTime(txtstartDate.Text);
                        objProject.projectTodate = Convert.ToDateTime(txtendDate.Text);
                        objProject.projectDurationMonths = Convert.ToInt32(txtprojectDurationInMonths.Text);
                        if (!String.IsNullOrEmpty(txtfundedByOrg.Text))
                        {
                            objProject.fundedByOrg = txtfundedByOrg.Text;
                        }                       
                        //objProject.fundedByOrg = txtfundedByOrg.Text;
                        if (ddlisAadharAuthenticationReqd.SelectedValue == "1")
                        {
                            isAadharAuthenticationReqd = true;
                            objProject.aadharOrderNo = txtAadharOrderNumber.Text;
                            objProject.aadharOrderDate = Convert.ToDateTime(txtAadharOrderDate.Text);
                        }
                        if (ddlisAadharAuthenticationReqd.SelectedValue == "2")
                        {
                            isAadharAuthenticationReqd = false; ;
                        }
                        objProject.isAadharAuthenticationReqd = isAadharAuthenticationReqd; // AadharAuthenticationReqd or not AadharAuthenticationReqd

                        objProject.budgetAllocated = Convert.ToInt32(txtbudgetAllocated.Text);
                        objProject.enterDate = DateTime.Now;
                        objProject.enterBy = Convert.ToInt32(Session["UserID"]);

                        context.NielitProjectss.Add(objProject);
                        context.SaveChanges();
                        strMessage = "New record saved.";
                    }
                    else
                    {
                        objProject = new NielitProjects();
                        objProject = context.NielitProjectss.Find(Convert.ToInt32(Request.QueryString["key"]));

                        //objProject.schemeCode = txtSchemeCode.Text;
                        objProject.ProjectName = txtProjectName.Text;
                        objProject.projectDescription = txtProjectDescription.Text;
                        objProject.projectFromDate = Convert.ToDateTime(txtstartDate.Text);
                        objProject.projectTodate = Convert.ToDateTime(txtendDate.Text);
                        objProject.projectDurationMonths = Convert.ToInt32(txtprojectDurationInMonths.Text);
                        if (!String.IsNullOrEmpty(txtfundedByOrg.Text))
                        {
                            objProject.fundedByOrg = txtfundedByOrg.Text;
                        }                       
                       // objProject.fundedByOrg = txtfundedByOrg.Text; 
                        if (ddlisAadharAuthenticationReqd.SelectedValue == "1")
                        {
                            isAadharAuthenticationReqd = true;
                            objProject.aadharOrderNo = txtAadharOrderNumber.Text;
                            objProject.aadharOrderDate = Convert.ToDateTime(txtAadharOrderDate.Text);
                        }
                        if (ddlisAadharAuthenticationReqd.SelectedValue == "2")
                        {
                            isAadharAuthenticationReqd = false; ;
                        }
                        objProject.isAadharAuthenticationReqd = isAadharAuthenticationReqd; // AadharAuthenticationReqd or not AadharAuthenticationReqd
                        objProject.budgetAllocated = Convert.ToInt32(txtbudgetAllocated.Text);


                        strMessage = "Record updated.";
                        context.SaveChanges();
                    }

                }
                Response.Redirect("NielitProjects.aspx?msg=" + strMessage, true);
            }
            //Response.Redirect("NielitProjects.aspx?msg=" + strMessage, true);
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
            ddlprojectName.SelectedValue = "0";
            txtBudgetAllocatedSearch.Text = "0";
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
                lblerrordisplay.Visible = true;
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
      protected bool IsValidForm()
    {
        try
        {
            if (!isValidProjectFromTo(txtstartDate, txtendDate))
            {
                lblerror.Visible = true;
                lblerror.Text = "Project start date should be less than end date.";
                lblerrordisplay.Visible = true;
                return false;
            }
            if (!String.IsNullOrEmpty(txtprojectDurationInMonths.Text))
            {
                if (!IsNumeric(txtprojectDurationInMonths.Text))
                {
                    lblerror.Visible = true;                   
                    lblerror.Text = "Project Duration Months Not a Number,Please enter numeric value Only .";
                    txtprojectDurationInMonths.Focus();
                    lblerrordisplay.Visible = true;
                    return false;
                }
            }
           
            if (!String.IsNullOrEmpty(txtprojectDurationInMonths.Text))
            {
                if (!isNumber(txtprojectDurationInMonths))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Project Duration Months Not a Number,Please enter numeric value Only .";
                    lblerrordisplay.Visible = true;
                    txtprojectDurationInMonths.Focus();             
                    return false;
                }
            }
            
            if (!isBlank(txtprojectDurationInMonths))
            {
                lblerror.Visible = true;
                lblerror.Text = "Project Duration Months can not be left blank";
                lblerrordisplay.Visible = true;
                return false;
            }
            if (!isBlank(txtProjectName))
            {
                lblerror.Visible = true;
                lblerror.Text = "Project Name can not be left blank";
                lblerrordisplay.Visible = true;
                return false;
            }
            if (!isBlank(txtProjectDescription))
            {
                lblerror.Visible = true;
                lblerror.Text = "Project Description can not be left blank";
                lblerrordisplay.Visible = true;
                return false;
            }
            if (!isBlank(txtstartDate))
            {
                lblerror.Visible = true;
                lblerror.Text = "Project From Date can not be left blank";
                lblerrordisplay.Visible = true;
                return false;
            }
            if (!isBlank(txtendDate))
            {
                lblerror.Visible = true;
                lblerror.Text = "Project To Date can not be left blank";
                lblerrordisplay.Visible = true;
                return false;
            }
            //if (!isBlank(txtfundedByOrg))
            //{
            //    lblerror.Visible = true;
            //    lblerror.Text = "Funded by Org can not be left blank";
            //    return false;
            //}
            if (!isBlank(txtbudgetAllocated))
            {
                lblerror.Visible = true;
                lblerror.Text = "Budget allocated can not be left blank";
                lblerrordisplay.Visible = true;
                return false;
            }
            if (!String.IsNullOrEmpty(txtbudgetAllocated.Text))
            {
                if (!IsNumeric(txtbudgetAllocated.Text))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Budget allocated Not a Number,Please enter numeric value Only .";
                    lblerrordisplay.Visible = true;
                    txtbudgetAllocated.Focus();                  
                    return false;
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

                HyperLink h5 = (HyperLink)e.Row.Cells[5].Controls[0];
                h5.NavigateUrl = hl.NavigateUrl;

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

            var ProjectNameS = from s in context.NielitProjectss
                             select new { Name = s.ProjectName };
            if (!String.IsNullOrEmpty(searchString))
            {
                ProjectNameS = ProjectNameS.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            ProjectNameS = ProjectNameS.OrderBy(s => s.Name);
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
            Response.Redirect("NielitProjects.aspx", true);

    }
    protected void txtstartDate_TextChanged(object sender, EventArgs e)
    {
        calculateDate();
    }
    protected void calculateDate()
    {
        if (txtstartDate.Text.Trim().Length != 0 && txtprojectDurationInMonths.Text.Trim().Length != 0)
        txtendDate.Text = Convert.ToDateTime(txtstartDate.Text).AddMonths(Convert.ToInt32(txtprojectDurationInMonths.Text)).ToString("dd-MMM-yyyy");
    }
    protected void txtDuration_TextChanged(object sender, EventArgs e)
    {
        calculateDate();
    }
}