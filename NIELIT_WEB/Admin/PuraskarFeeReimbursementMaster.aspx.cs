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

public partial class Admin_PuraskarFeeReimbursementMaster : BasePage
{    
    String strMessage = string.Empty;   
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
        //    if (IsSessionAlive() == false)
        //        Response.Redirect("../Index.aspx");
        //    currentRoleId = Convert.ToInt32(Session["RoleID"]);
        //    loginUserNo = Convert.ToInt32(Session["UserID"]);
        //    entityID = Convert.ToInt64(Session["EntityID"]);

            //if (!UserManager.HasRight(currentRoleId, enmRight.View))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}
            if (!Page.IsPostBack)
            {
              
                using (EConnectContext context = new EConnectContext())
                {
                    BindListData();
                    BindListDataFilter();
                                  
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {                        
                        ShowEditMode();                      
                    }
                    else
                    {                       
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";
                        BindGridView();

                        if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Puraskar Fee Reimbursement Master", "Admin/PuraskarFeeReimbursementMaster.aspx?Id=" + Request.QueryString["Id"].ToString(), ""));
                        }
                        else
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Puraskar Fee Reimbursement Master", "Admin/PuraskarFeeReimbursementMaster.aspx", ""));
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

    protected void BindListDataFilter()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {               
                ddlcourseName.Items.Clear();
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == 1
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name }; 
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourseName, CourseList, lst);
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
            using (EConnectContext context = new EConnectContext())
            {
                ddlcourses.Items.Clear();
                ddlcourseName.Items.Clear();
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.Courses where p.CourseCategoryID==1
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };
               EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourses, CourseList, lst);
            }        
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlcourses_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 courseID = Convert.ToInt32(ddlcourses.SelectedValue);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
            lblHeading.Text = "Puraskar Fee Reimbursement Master";
            tblNavLinks.Visible = true;

            using (EConnectContext context = new EConnectContext())
            {
                Int32 Id = Convert.ToInt32(Request.QueryString["Key"]);
                if (context.PuraskarFeeReimbursementMasters.Any(s => (s.ID == Id)))
                {
                    ddlcourses.Enabled = false;

                    var DisplayPuraskarFeeReimbursementRecord = (from p in context.PuraskarFeeReimbursementMasters
                                                                 //join s in context.Courses on p.courseID equals s.ID
                                          where p.ID == Id
                                          select p).FirstOrDefault();

                    ddlcourses.SelectedValue = DisplayPuraskarFeeReimbursementRecord.courseID.ToString();
                    txtinstalments.Text = DisplayPuraskarFeeReimbursementRecord.TotalInstalments.ToString();
                    txtTotalAmount.Text = DisplayPuraskarFeeReimbursementRecord.TotalAmt.ToString();
                    txteffectivefromDate.Text = DisplayPuraskarFeeReimbursementRecord.effectiveFrom.ToString("dd-MMM-yyyy");
                   // txteffectiveEndDate.Text = DisplayPuraskarFeeReimbursementRecord.effectiveTo.ToString("dd-MMM-yyyy");
                   
                    
                }
            };
            //if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            //{
            //    btnSave.Visible = false;
            //}
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
            using (EConnectContext context = new EConnectContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                Int32 courseID = 0;
                if (ddlcourseName.SelectedValue != "0")
                    courseID = Convert.ToInt32(ddlcourseName.SelectedValue);
                var PuraskarFeeReimbursementMaster = from s in context.PuraskarFeeReimbursementMasters
                                   join p in context.Courses on s.courseID equals p.ID                                   
                                 select new
                                 {
                                     ID = s.ID,
                                     courseName = p.Name,
                                     courseid=s.courseID,
                                     TotalInstalments = s.TotalInstalments,
                                     TotalAmt=s.TotalAmt,
                                     effectiveFrom = s.effectiveFrom,
                                     effectiveTo = s.effectiveTo,
                                 };

                if (!String.IsNullOrEmpty(searchString))
                {
                    PuraskarFeeReimbursementMaster = PuraskarFeeReimbursementMaster.Where(s => s.courseName.ToUpper().Contains(searchString));
                }
                if (courseID != 0)
                {
                    PuraskarFeeReimbursementMaster = PuraskarFeeReimbursementMaster.Where(s => s.courseid == courseID);
                }               
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ID":
                            if (sortOrder == "DESC")
                                PuraskarFeeReimbursementMaster = PuraskarFeeReimbursementMaster.OrderByDescending(s => s.ID);
                            else
                                PuraskarFeeReimbursementMaster = PuraskarFeeReimbursementMaster.OrderBy(s => s.ID);
                            break;
                        case "CourseName":
                            if (sortOrder == "DESC")
                                PuraskarFeeReimbursementMaster = PuraskarFeeReimbursementMaster.OrderByDescending(s => s.courseName);
                            else
                                PuraskarFeeReimbursementMaster = PuraskarFeeReimbursementMaster.OrderBy(s => s.courseName);
                            break;
                        default:
                            PuraskarFeeReimbursementMaster = PuraskarFeeReimbursementMaster.OrderBy(s => s.courseName);
                            break;
                    }
                }

                PagingBar1.Bind(PuraskarFeeReimbursementMaster, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                if (gvMain.Rows.Count <= 0)
                {
                    lblerrorG.Text = "No record found.";
                    lblerrorG.Visible = true;
                    gvMain.Visible = false;
                    PagingBar1.Visible = false;
                }
                //if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                //{
                //    gvMain.Columns[7].Visible = false;
                //}
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
            ////if (!UserManager.HasRight(currentRoleId, enmRight.New))
            ////{
            ////    BreadCrumb1.Render();
            ////    ShowAlert("Sorry! You don't have rights to add new record.", true);
            ////    return;
            ////}            
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required           
            //Updating Breadcrumb           
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Puraskar Fee Reimbursement Master", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("PuraskarFeeReimbursementMaster.aspx?ID=" + Request.QueryString["ID"].ToString()), true);
            }
            else
            {
                Response.Redirect("PuraskarFeeReimbursementMaster.aspx", true);             
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
            Int32 courseID = Convert.ToInt32(ddlcourses.SelectedValue);
            if (IsValidForm())
            {
                BreadCrumb1.Render();
                using (EConnectContext context = new EConnectContext())
                {
                    PuraskarFeeReimbursementMaster objPuraskarFeeReimbursementMaster;
                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        objPuraskarFeeReimbursementMaster = new PuraskarFeeReimbursementMaster();

                        var objPuraskarFeeReimbursementMaster1 = (from s in context.PuraskarFeeReimbursementMasters
                                                                  where s.courseID == courseID && s.effectiveTo != null
                                                             select s);
                        if (objPuraskarFeeReimbursementMaster1.Count() > 0)
                        {
                            ShowAlert("Course name already exists");
                            return;
                        }
                        objPuraskarFeeReimbursementMaster.courseID = courseID;
                        objPuraskarFeeReimbursementMaster.TotalInstalments = Convert.ToInt32(txtinstalments.Text.Trim());
                        objPuraskarFeeReimbursementMaster.TotalAmt = Convert.ToInt64(txtTotalAmount.Text.Trim());
                        objPuraskarFeeReimbursementMaster.effectiveFrom = Convert.ToDateTime(txteffectivefromDate.Text);
                       // objPuraskarFeeReimbursementMaster.effectiveTo= Convert.ToDateTime(txteffectiveEndDate.Text);
                        objPuraskarFeeReimbursementMaster.enterDate = DateTime.Now;
                        objPuraskarFeeReimbursementMaster.enterBy = Convert.ToInt32(Session["UserID"]);

                        context.PuraskarFeeReimbursementMasters.Add(objPuraskarFeeReimbursementMaster);
                        context.SaveChanges();
                        strMessage = "New record saved.";
                    }
                    else
                    {
                        objPuraskarFeeReimbursementMaster = new PuraskarFeeReimbursementMaster();
                        objPuraskarFeeReimbursementMaster = context.PuraskarFeeReimbursementMasters.Find(Convert.ToInt32(Request.QueryString["key"]));

                        objPuraskarFeeReimbursementMaster.courseID = courseID;
                        objPuraskarFeeReimbursementMaster.TotalInstalments = Convert.ToInt32(txtinstalments.Text.Trim());
                        objPuraskarFeeReimbursementMaster.TotalAmt = Convert.ToInt64(txtTotalAmount.Text.Trim());
                        objPuraskarFeeReimbursementMaster.effectiveFrom = Convert.ToDateTime(txteffectivefromDate.Text);
                        if (txteffectiveEndDate.Text != "")
                        {
                            objPuraskarFeeReimbursementMaster.effectiveTo = Convert.ToDateTime(txteffectiveEndDate.Text);
                        }
                        //objPuraskarFeeReimbursementMaster.enterDate = DateTime.Now;
                        //objPuraskarFeeReimbursementMaster.enterBy = Convert.ToInt32(Session["UserID"]);
                        strMessage = "Record updated.";
                        context.SaveChanges();
                    }
                }
                Response.Redirect("PuraskarFeeReimbursementMaster.aspx?msg=" + strMessage, true);
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
            ddlcourseName.SelectedValue = "0";           
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
  
   protected bool IsValidForm()
    {
        try
        {            
           
            if (!String.IsNullOrEmpty(txtinstalments.Text))
            {
                if (!IsNumeric(txtinstalments.Text))
                {
                    lblerror.Visible = true;                   
                    lblerror.Text = "Total Instalments Not a Number,Please enter numeric value Only .";
                    txtinstalments.Focus();
                    lblerrordisplay.Visible = true;
                    return false;
                }
            }
           
            if (!String.IsNullOrEmpty(txtTotalAmount.Text))
            {
                if (!isNumber(txtTotalAmount))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Total Amount Not a Number,Please enter numeric value Only .";
                    lblerrordisplay.Visible = true;
                    txtTotalAmount.Focus();             
                    return false;
                }
            }
            
            if (!isBlank(txtTotalAmount))
            {
                lblerror.Visible = true;
                lblerror.Text = "Total Amount can not be left blank";
                lblerrordisplay.Visible = true;
                return false;
            }
           
            if (!isBlank(txtinstalments))
            {
                lblerror.Visible = true;
                lblerror.Text = "Total Instalments can not be left blank";
                lblerrordisplay.Visible = true;
                return false;
            }
            if (!isBlank(txteffectivefromDate))
            {
                lblerror.Visible = true;
                lblerror.Text = "Effective From Date can not be left blank";
                lblerrordisplay.Visible = true;
                return false;
            }
            //if (!isBlank(txteffectiveEndDate))
            //{
            //    lblerror.Visible = true;
            //    lblerror.Text = "Effective End Date can not be left blank";
            //    lblerrordisplay.Visible = true;
            //    return false;
            //}           
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

            var courseNameS = from s in context.PuraskarFeeReimbursementMasters 
                              join p in context.Courses on s.courseID equals p.ID
                             select new { Name = p.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                courseNameS = courseNameS.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            courseNameS = courseNameS.OrderBy(s => s.Name);
            foreach (var c in courseNameS)
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
        Response.Redirect("PuraskarFeeReimbursementMaster.aspx", true);
    }     
}