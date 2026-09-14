using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
public partial class ModuleExemption : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;

    IEnumerable<Object> modules;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
           
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    Int32 CourseID = 0;
                    Int32 currentRevisionNo=0;
                    if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                    {
                        CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
                    }
                    if(!String.IsNullOrEmpty(Request.QueryString["RevisionNo"]))
                    {
                      currentRevisionNo = Convert.ToInt32(Request.QueryString["RevisionNo"]);
                    }
                   
                   
                    using (EConnectContext context = new EConnectContext())
                    {
                        Course currentcourse = context.Courses.Find(CourseID);
                        Course lowerCourse = context.Courses.Find(currentcourse.LowerCourseID);
                        if (currentcourse.LowerCourseID != null)
                        {
                            tblShow.Visible = true;
                            trMsg.Visible = false;
                            lblMsg.Text = "";
                            lblNewRevNo.Text = "Current Course (" + currentcourse.Code + " Level) Revision No.";
                            lblOldRevNo.Text = "Lower Course (" + lowerCourse.Code +" Level) Revision No.";
                        }
                        else
                        {
                            trMsg.Visible = true;
                            lblMsg.Text = "Lower Course details for selected course level is not found.";
                            PagingBar1.Visible = false;
                            tblShow.Visible = false;
                        }
                        fillCurrentCourseRevisionNo(CourseID, currentcourse.CourseCategoryID);
                    };
                    ddlCurrentCourseRevNo_SelectedIndexChanged(ddlCurrentCourseRevNo, EventArgs.Empty);
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Modules Exemption", "", ""));
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void fillCurrentCourseRevisionNo(Int32 couID, Int32 catID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var query = (from p in context.CourseRevisions
                             where (p.CourseCategoryID == catID && p.CourseID == couID)
                             orderby p.RevisionNumber descending
                             select new { ValueField = p.RevisionNumber, TextField = p.RevisionNumber }).Take(1);
                var query1 = (from p in context.ExemptionRules 
                              where (p.CourseCategoryID == catID && p.CurrentCourseID == couID)
                              select new { ValueField = p.CurrentCourseRevisionNumber, TextField = p.CurrentCourseRevisionNumber }).Distinct();
                var query2 = query.Union(query1);

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCurrentCourseRevNo, query2, null );
                ddlCurrentCourseRevNo.SelectedValue = query.FirstOrDefault().ValueField.ToString();
            };
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
            context = new EConnectContext();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Form Header Detail";
            tblNavLinks.Visible = true;
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            //Create an object of record to be modified and assign properties to relevant fields.
             
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            context.Dispose();
        }
    }
    protected void BindGridView()
    {
        try
        {
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            Int32 CourseID = 0;
            Int32 currentRevisionNo = 0;
            CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
            currentRevisionNo = Convert.ToInt32(Request.QueryString["RevisionNo"]);
            Int32 dropDownCurrentCourseRevNo = Convert.ToInt32(ddlCurrentCourseRevNo.SelectedValue);
            Int32 dropDownLowerCourseRevNo = Convert.ToInt32(ddlLowerCourseRevNo.SelectedValue);

            Course currentcourse = context.Courses.Find(CourseID);
            Course prevCourse = context.Courses.Find(currentcourse.LowerCourseID);
            var prevModules = from m in context.Modules
                              where (m.CourseID == currentcourse.LowerCourseID && m.RevisionNumber == dropDownLowerCourseRevNo)
                              orderby m.Code
                              select new
                              {
                                  ID = m.ID,
                                  oldModuleName = m.ShortName + "-" + m.Name 
                              };

            gvMain.Columns[1].HeaderText = prevCourse.Code + " Level Revision " + dropDownLowerCourseRevNo + " : Module Names";
            gvMain.Columns[2].HeaderText = currentcourse.Code + " Level Revision " + dropDownCurrentCourseRevNo + " : Module Names";
            PagingBar1.Bind(prevModules, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            context.Dispose();
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
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New User";
        }
        else
        {
            Response.Redirect("ModuleParity.aspx", true);
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
            //ddlSearchUserType.SelectedValue = "0";
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
            context = new EConnectContext();
            if (hfActionID.Value != "")
            {
                String recordID = hfActionID.Value.Split('$')[0].ToString();
                LinkButton btnAction = (LinkButton)sender;
                if (btnAction.CommandName == "Delete")
                {
                    //Load the object and apply validateion if required
                    //call delete function
                    //bind the grid again
                    BindGridView();
                    ShowAlert("Record deleted successfully.", true);
                    hfActionID.Value = "";
                }
                else if (btnAction.CommandName == "Action")
                {
                    //Load the object and apply validateion if required
                    //call function to perform required action
                    //bind the grid again
                    BindGridView();
                    ShowAlert("Record Action1 successfully.", true);
                    hfActionID.Value = "";
                }
                uPnlGrid.Update();
            }
        }
        catch (Exception ex)
        {
            hfActionID.Value = "";
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                using (EConnectContext context = new EConnectContext())
                {
                    int[] moduleTypesBridgeTheory = { Convert.ToInt32(enmModuleType.Theory), Convert.ToInt32(enmModuleType.Bridge)};
                    int[] moduleTypePractical = { Convert.ToInt32(enmModuleType.Practical) };
                    int[] moduleTypeProject = { Convert.ToInt32(enmModuleType.Project) };
                    Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
                    Course currentcourse = context.Courses.Find(couID);
                    Int32 ddlCurrentCourseRevNumber = Convert.ToInt32(ddlCurrentCourseRevNo.SelectedValue);
                    Int32 ddlLowerCourseRevNumber = Convert.ToInt32(ddlLowerCourseRevNo.SelectedValue);
                    Int32 prevModuleID = Convert.ToInt32(gvMain.DataKeys[e.Row.RowIndex].Values[0]);
                    Module oldModuleTypeID = context.Modules.Find(prevModuleID);
                    if (oldModuleTypeID.ModuleTypeID == Convert.ToInt32(enmModuleType.Theory) || oldModuleTypeID.ModuleTypeID == Convert.ToInt32(enmModuleType.Bridge))
                    {
                        var currentModules = (from s in context.Modules
                                              where (s.CourseID == couID && s.RevisionNumber == ddlCurrentCourseRevNumber
                                              && moduleTypesBridgeTheory.Contains(s.ModuleTypeID))
                                              orderby s.Code
                                              select new
                                              {
                                                  ValueField = s.ID,
                                                  TextField = s.ShortName + "-" + s.Name
                                              }).ToList();

                        DropDownList ddlNewRevNo = (DropDownList)e.Row.FindControl("ddlNew");
                        //CheckBox checkParity = (CheckBox)e.Row.FindControl("chkSelect");
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlNewRevNo, currentModules, new ListItem("--Select One--", "0"));

                        var query = (from s in context.ExemptionRules 
                                     where ( s.CourseCategoryID == currentcourse.CourseCategoryID && s.CurrentCourseID == couID 
                                             && s.LowerCourseID == currentcourse.LowerCourseID
                                             && s.LowerModuleID == prevModuleID
                                             && s.CurrentCourseRevisionNumber == ddlCurrentCourseRevNumber
                                             && s.LowerCourseRevisionNumber == ddlLowerCourseRevNumber)
                                     select new { newmoduleID = s.CurrentModuleID ,allowParity=s.AllowParity}).FirstOrDefault();
                        if (query != null)
                        {
                            ddlNewRevNo.SelectedValue = query.newmoduleID.ToString();
                            //if (query.allowParity == true)
                            //{
                            //    checkParity.Checked = true;
                            //}
                        }
                    }
                    else if (oldModuleTypeID.ModuleTypeID == Convert.ToInt32(enmModuleType.Practical))
                    {
                        var currentModules = (from s in context.Modules
                                              where (s.CourseID == couID && s.RevisionNumber == ddlCurrentCourseRevNumber
                                              && moduleTypePractical.Contains(s.ModuleTypeID))
                                              orderby s.Code
                                              select new
                                              {
                                                  ValueField = s.ID,
                                                  TextField = s.ShortName + "-" + s.Name
                                              }).ToList();

                        DropDownList ddlNewRevNo = (DropDownList)e.Row.FindControl("ddlNew");
                        //CheckBox checkParity = (CheckBox)e.Row.FindControl("chkSelect");
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlNewRevNo, currentModules, new ListItem("--Select One--", "0"));

                        var query = (from s in context.ExemptionRules
                                     where (s.CourseCategoryID == currentcourse.CourseCategoryID && s.CurrentCourseID == couID
                                             && s.LowerCourseID == currentcourse.LowerCourseID
                                             && s.LowerModuleID == prevModuleID
                                             && s.CurrentCourseRevisionNumber == ddlCurrentCourseRevNumber
                                             && s.LowerCourseRevisionNumber == ddlLowerCourseRevNumber)
                                     select new { newmoduleID = s.CurrentModuleID, allowParity = s.AllowParity }).FirstOrDefault();
                        if (query != null)
                        {
                            ddlNewRevNo.SelectedValue = query.newmoduleID.ToString();
                            //if (query.allowParity == true)
                            //{
                            //    checkParity.Checked = true;
                            //}
                        }
                    }
                    else if (oldModuleTypeID.ModuleTypeID == Convert.ToInt32(enmModuleType.Project))
                    {
                        var currentModules = (from s in context.Modules
                                              where (s.CourseID == couID && s.RevisionNumber == ddlCurrentCourseRevNumber
                                              && moduleTypeProject.Contains(s.ModuleTypeID))
                                              orderby s.Code
                                              select new
                                              {
                                                  ValueField = s.ID,
                                                  TextField = s.ShortName + "-" + s.Name
                                              }).ToList();

                        DropDownList ddlNewRevNo = (DropDownList)e.Row.FindControl("ddlNew");
                        //CheckBox checkParity = (CheckBox)e.Row.FindControl("chkSelect");
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlNewRevNo, currentModules, new ListItem("--Select One--", "0"));

                        var query = (from s in context.ExemptionRules
                                     where (s.CourseCategoryID == currentcourse.CourseCategoryID && s.CurrentCourseID == couID
                                             && s.LowerCourseID == currentcourse.LowerCourseID
                                             && s.LowerModuleID == prevModuleID
                                             && s.CurrentCourseRevisionNumber == ddlCurrentCourseRevNumber
                                             && s.LowerCourseRevisionNumber == ddlLowerCourseRevNumber)
                                     select new { newmoduleID = s.CurrentModuleID, allowParity = s.AllowParity }).FirstOrDefault();
                        if (query != null)
                        {
                            ddlNewRevNo.SelectedValue = query.newmoduleID.ToString();
                            //if (query.allowParity == true)
                            //{
                            //    checkParity.Checked = true;
                            //}
                        }
                    }
                };
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
            var users = from s in context.Users
                        select new { Name = s.UserName };
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            users = users.OrderBy(s => s.Name);

            var users1 = from s in context.Users
                         select new { Name = s.LoginID };
            if (!String.IsNullOrEmpty(searchString))
            {
                users1 = users1.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            users = users.Union(users1).Take(count);
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
        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
        {
            if (!String.IsNullOrEmpty(Request.QueryString["RevisionNo"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("ModuleExemption.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&RevisionNo=" + Request.QueryString["RevisionNo"]), true);
            }
        }
        else
        {
            Response.Redirect("ModuleExemption.aspx", true);
        }
       
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CurrentCourseMaxRevisionNo = (from c in context.CourseRevisions
                                                    where c.CourseID == CourseID
                                                    select c.RevisionNumber).Distinct().Max();
                ddlCurrentCourseRevNo.SelectedValue = CurrentCourseMaxRevisionNo.ToString();
            };
            ddlLowerCourseRevNo.SelectedValue = "-1";
            divGrid.Visible = false;
            divNavigation.Visible = false;
            divSave.Visible = false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
  
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            context = new EConnectContext();
            int i = 0;
            Boolean isNotSelectedCurrentModule = false;
            Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
            Course currentcourse = context.Courses.Find(couID);
            Int32 currentCourseMaxRevisionNo = Convert.ToInt32(ddlCurrentCourseRevNo.SelectedValue);
            Int32 LowerCourseRevisionNo = Convert.ToInt32(ddlLowerCourseRevNo.SelectedValue);
            int[] arr=new int[gvMain.Rows.Count];
            foreach (GridViewRow row in gvMain.Rows)
            {
                Int32 newModuleID = Convert.ToInt32(((DropDownList)row.FindControl("ddlNew")).SelectedValue);
                arr[i] = newModuleID;
               
                if (newModuleID != 0)
                {
                    isNotSelectedCurrentModule = true;
                    if (i > 0)
                    {
                        for (int j = 0; j < i; j++)
                        {
                            if (newModuleID == arr[j])
                                throw new Exception("Please Select another  Module Name");
                        }
                    }
                }
                i++;
            }
            
            if (isNotSelectedCurrentModule == false)
            {
                throw new Exception("Please Select at least one Module Name");
            }
            context.Database.ExecuteSqlCommand("delete from Exemption_Rule where Course_Category_ID='" + currentcourse.CourseCategoryID.ToString() + "'" +
                                               " and Current_Course_ID='" + couID.ToString() + "'" +
                                               " and Current_Course_Revision='" + currentCourseMaxRevisionNo.ToString() + "'" +
                                               " and Lower_Course_ID='" + currentcourse.LowerCourseID.ToString() + "'" +
                                               " and Lower_Course_Revision='" + LowerCourseRevisionNo.ToString() + "'");
            foreach (GridViewRow row in gvMain.Rows)
            {
                ExemptionRule objExemptionRule = new ExemptionRule();
                objExemptionRule.CourseCategoryID = currentcourse.CourseCategoryID;
                objExemptionRule.CurrentCourseID = couID;
                objExemptionRule.CurrentCourseRevisionNumber = currentCourseMaxRevisionNo;
                Int32 currentModuleID = Convert.ToInt32(((DropDownList)row.FindControl("ddlNew")).SelectedValue);
                objExemptionRule.CurrentModuleID = currentModuleID;
                objExemptionRule.LowerCourseID = currentcourse.LowerCourseID.Value;
                objExemptionRule.LowerCourseRevisionNumber = LowerCourseRevisionNo;
                Int32 lowerModuleID = Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Values[0]);
                objExemptionRule.LowerModuleID = lowerModuleID;
                objExemptionRule.CreatedByID = Convert.ToInt32(Session["UserID"]);
                objExemptionRule.CreatedOn = DateTime.Now;
                //if (((CheckBox)row.FindControl("chkSelect")).Checked == true)
                //    objExemptionRule.AllowParity = true;
                //else
                objExemptionRule.AllowParity = false;
                if (currentModuleID != 0)
                {
                    context.ExemptionRules.Add(objExemptionRule);
                }
            }
            context.SaveChanges();
            strMessage = "Record saved";
            //btnShow_Click("",EventArgs.Empty);
            //Response.Redirect("ModuleExemption.aspx?" + Request.QueryString.ToString() + "&msg=" + strMessage);
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                if (!String.IsNullOrEmpty(Request.QueryString["RevisionNo"]))
                {
                    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("ModuleExemption.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&RevisionNo=" + Request.QueryString["RevisionNo"]), true);
                }
            }
            else
            {
                Response.Redirect("ModuleExemption.aspx?msg=" + strMessage);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally
        {
            context.Dispose();
        }
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
            if (ddlLowerCourseRevNo.SelectedValue == "-1")
            {
                throw new Exception("Please select Lower Course Revision Number");
            }

           
            using (EConnectContext context = new EConnectContext())
            {
                //Course currentcourse = context.Courses.Find(CourseID);
                //Int32 CurrentCourseMaxRevisionNo = (from c in context.CourseRevisions
                //                                    where c.CourseID == CourseID
                //                                    select c.RevisionNumber).Distinct().Max();
                //Int32 LowerCourseMaxRevisionNo = (from c in context.CourseRevisions
                //                                    where c.CourseID == currentcourse.LowerCourseID 
                //                                    select c.RevisionNumber).Distinct().Max();
                //if (Convert.ToInt32(ddlLowerCourseRevNo.SelectedValue) == LowerCourseMaxRevisionNo
                //     && Convert.ToInt32(ddlCurrentCourseRevNo.SelectedValue) == CurrentCourseMaxRevisionNo)
                //    btnSave.Visible = true;
                //else
                //    btnSave.Visible = false;

                divGrid.Visible = true;
                divNavigation.Visible = true;
                PagingBar1.CurrentPageSize = 0;
                BindGridView();
                PagingBar1.Visible = true;
                divSave.Visible = true;
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCurrentCourseRevNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlLowerCourseRevNo.Items.Clear();
        fillLowerCourseRevisionNo(Convert.ToInt32(ddlCurrentCourseRevNo.SelectedValue));
    }
    protected void fillLowerCourseRevisionNo(Int32 currentCourseRevisionNumber)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
                Course currentcourse = context.Courses.Find(couID);
                Int32 currentCourseRevNo = Convert.ToInt32(ddlCurrentCourseRevNo.SelectedValue);
                ListItem lst = new ListItem("--Select One--", "-1");
             
                //var query = (from p in context.CourseRevisions
                //             where (p.CourseCategoryID == currentcourse.CourseCategoryID  && p.CourseID == currentcourse.LowerCourseID)
                //             orderby p.RevisionNumber descending
                //             select new { ValueField = p.RevisionNumber, TextField = p.RevisionNumber }).Take(1);
                //var query1 = (from p in context.ExemptionRules
                //              where (p.CourseCategoryID == currentcourse.CourseCategoryID && p.CurrentCourseID == couID
                //                     && p.CurrentCourseRevisionNumber == currentCourseRevNo)
                //              select new { ValueField = p.CurrentCourseRevisionNumber, TextField = p.CurrentCourseRevisionNumber }).Distinct();
                //var query2 = query.Union(query1);

                var query = (from p in context.CourseRevisions
                             where (p.CourseCategoryID == currentcourse.CourseCategoryID && p.CourseID == currentcourse.LowerCourseID)
                             orderby (p.RevisionNumber)
                             select new { ValueField = p.RevisionNumber, TextField = p.RevisionNumber }).Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlLowerCourseRevNo, query, lst);
            };

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //this function returns list of exempted modules in current course version
    protected List<Module> GetExemptedModules(Int32 passedCourseID, Int32 newCourseID, Int64 registrationNumber, Int64 candidateID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //validate whether passedCourseID & newCourseID are consecutive or not
                Course currentcourse = context.Courses.Find(newCourseID);
                List<Module> exemptedModulesOfCurrentRevision = new List<Module>();
                if (currentcourse.LowerCourseID != null)
                {
                    if (currentcourse.LowerCourseID == passedCourseID)
                    {
                        Int32 passedCourseRegistrationRevisionNo = EConnect.NIELIT.CourseManager.GetCourseRevisionNumberAtRegistrationCommenced(context, passedCourseID, registrationNumber, candidateID);
                        Int32 passedCourseCurrentRevisionNo = EConnect.NIELIT.CourseManager.GetCurrentCourseRevisionNumber(context, passedCourseID);
                        Int32 currentCourseCurrentRevisionNo = EConnect.NIELIT.CourseManager.GetCurrentCourseRevisionNumber(context, newCourseID);

                        //get passed modules list of passed course Registration Revision Number
                        List<Int32> lowerCoursePassedModulesOfRegistrationRevisionNo = (from s in context.CourseExamApplicationDetails
                                                                                                join g in context.ResultGrades
                                                                                                on s.ResultGradeID equals g.ID
                                                                                                join m in context.Modules on s.ModuleID equals m.ID
                                                                                                where (s.CourseID == passedCourseID &&
                                                                                                       s.CandidateID == candidateID &&
                                                                                                       s.RegistrationNumber == registrationNumber &&
                                                                                                       m.RevisionNumber == passedCourseRegistrationRevisionNo &&
                                                                                                       g.IsPassed == true)
                                                                                                orderby m.RevisionNumber, m.Code
                                                                                                select m.ID).ToList();

                        // Exempted modules list of current course revision
                        List<Int32> exemptedModuleListOfCurrentCourseCurrentRevisionNo = (from e in context.ExemptionRules
                                                                                        where (e.CurrentCourseID == newCourseID &&
                                                                                               e.LowerCourseID == passedCourseID &&
                                                                                               e.CurrentCourseRevisionNumber == currentCourseCurrentRevisionNo &&
                                                                                               e.LowerCourseRevisionNumber == passedCourseRegistrationRevisionNo &&
                                                                                               lowerCoursePassedModulesOfRegistrationRevisionNo.Contains(e.LowerModuleID)
                                                                                                )
                                                                                        select e.CurrentModuleID.Value).ToList();
                       //List  of modules which are to be attempted in current course revision
                        exemptedModulesOfCurrentRevision = (from m in context.Modules
                                                            where (m.CourseID == newCourseID &&
                                                                   m.RevisionNumber == currentCourseCurrentRevisionNo &&
                                                                   exemptedModuleListOfCurrentCourseCurrentRevisionNo.Contains(m.ID) 
                                                                   )
                                                            orderby m.Code
                                                            select m).ToList();
                    }
                }
                return exemptedModulesOfCurrentRevision.ToList();
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}