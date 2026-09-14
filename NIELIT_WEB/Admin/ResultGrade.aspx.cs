using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class Admin_ResultGrade : BasePage
{
    String strMessage = string.Empty;   
    Int32 currentRoleId = 0;

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
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    FillCourseCategory();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillFilterCourseCategory();
                    FillFilterVersions();
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Result Grade", "Admin/ResultGrade.aspx", ""));
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
    protected void FillCourseCategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.CourseCategories
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlccat, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillFilterCourseCategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                ListItem lst = new ListItem("--Select One--", "0");
                var category = from p in context.CourseCategories
                                     orderby p.ID
                                     select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlfilterccategory, category, lst);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillFilterVersions()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                ListItem lst = new ListItem("--Select One--", "0");
                var versions = from p in context.ResultGrades
                               orderby p.ID
                               select new { ValueField = p.VersionID, TextField = p.VersionID };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlflversion, versions.Distinct(), lst);
            }
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
            lblHeading.Text = "Result Grade";
            //tblNavLinks.Visible = true;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 gradeID = Convert.ToInt32(Request.QueryString["Key"].ToString());
                var objResultGrade = (from s in context.ResultGrades
                                      where s.ID == gradeID
                                      orderby s.EffectiveFromDate descending
                                     select s).FirstOrDefault();
                var Version = (from p in context.ResultGrades
                                  where p.ID == gradeID && p.CourseCategoryID == objResultGrade.CourseCategoryID
                                  select new { ValueField = p.VersionID, TextField = p.VersionID}).Distinct();

                var MaxVersion = (from p in context.ResultGrades
                                  where p.ID == gradeID && p.CourseCategoryID == objResultGrade.CourseCategoryID
                                  select new { ValueField = p.VersionID + 1, TextField = p.VersionID + 1 }).Distinct();

                if (MaxVersion.Count() != 0)
                {
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlversion, Version.Union(MaxVersion), null);
                }
                
                txtCode.Text = objResultGrade.Code.ToString();
                txtDescription.Text = objResultGrade.Description.ToString();
                if (objResultGrade.IsPassed == true)
                    ddlStatus.SelectedValue = "1";
                else
                    ddlStatus.SelectedValue = "2";
                txtPercentageFrom.Text = objResultGrade.PercentageFrom.ToString();
                txtPassedTo.Text = objResultGrade.PercentageTo.ToString();
                txtEffectiveFromDate.Text = objResultGrade.EffectiveFromDate.ToString("dd-MMM-yyyy");
                ddlccat.SelectedValue = objResultGrade.CourseCategoryID.ToString();
                ddlccat.Enabled = false;
                ddlversion.SelectedValue = objResultGrade.VersionID.Value.ToString();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(objResultGrade.Code, "#", ""));
                //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("ExamVenue", "#", ""));
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                //Create an object of record to be modified and assign properties to relevant fields.
                ViewState["SortOrder1"] = "";
                ViewState["SortField1"] = "";
                BindOldGridView();
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    btnSave.Visible = false;
                    Btnsaveas.Visible = false;
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
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
                Int32 ccatid = 0;
                if (ddlfilterccategory.SelectedValue != "0")
                {
                    ccatid = Convert.ToInt32(ddlfilterccategory.SelectedValue);
                }
                Int32 verid = 0;
                if (ddlflversion.SelectedValue != "0")
                {
                    verid = Convert.ToInt32(ddlflversion.SelectedValue);
                }
             
                var objResultGrade = (from s in context.ResultGrades
                                      where s.EffectiveFromDate == ((from t in context.ResultGrades where t.ID == s.ID select t.EffectiveFromDate).Max())
                                     orderby s.CourseCategoryID 
                                     select new
                                    {
                                        ID = s.ID,
                                        Code = s.Code,
                                        Description = s.Description,
                                        ccat = s.CourseCategory.Name,
                                        coursecatID = s.CourseCategoryID,
                                        Versionid = s.VersionID.HasValue ? s.VersionID.Value : 0
                                    }).Distinct();

                if (ccatid != 0)
                {
                    objResultGrade = objResultGrade.Where(s => s.coursecatID == ccatid);
                }
                if (verid != 0)
                {
                    objResultGrade = objResultGrade.Where(s => s.Versionid == verid);
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    objResultGrade = objResultGrade.Where(s => s.Code.ToUpper().Contains(searchString));
                }

                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ID":
                            if (sortOrder == "DESC")
                                objResultGrade = objResultGrade.OrderByDescending(s => s.ID);
                            else
                                objResultGrade = objResultGrade.OrderBy(s => s.ID);
                            break;
                        case "Code":
                            if (sortOrder == "DESC")
                                objResultGrade = objResultGrade.OrderByDescending(s => s.Code);
                            else
                                objResultGrade = objResultGrade.OrderBy(s => s.Code);
                            break;
                        case "Description":
                            if (sortOrder == "DESC")
                                objResultGrade = objResultGrade.OrderByDescending(s => s.Description);
                            else
                                objResultGrade = objResultGrade.OrderBy(s => s.Description);
                            break;
                        case "ccat":
                            if (sortOrder == "DESC")
                                objResultGrade = objResultGrade.OrderByDescending(s => s.ccat);
                            else
                                objResultGrade = objResultGrade.OrderBy(s => s.ccat);
                            break;
                        default:
                            objResultGrade = objResultGrade.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(objResultGrade, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
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
        try
        {
            if (btnMode.ViewMode == ToggleView.Mode.New)
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.New))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to add new record.", true);
                    return;
                }
                FillCourseCategory();
                btnMode.ViewMode = ToggleView.Mode.List;
                DivHistory.Visible = false;
                lblError2.Visible = false;
                GridViewOld.Visible = false;
                PagingBar1.Visible = false;
                Btnsaveas.Visible = false;
                UPanelHistory.Update();
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                ListItem li = new ListItem("1", "1");
                ddlversion.Items.Add(li);
                //Change the heading text as required
                lblHeading.Text = "Result Grade";
                //Updating Breadcrumb
                //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Result Grade", "#", ""));
            }
            else
            {
                Response.Redirect("ResultGrade.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
            if (txtPercentageFrom.Text!="")
            {
                if (Convert.ToInt32(txtPercentageFrom.Text) <= 0)
                {
                    ShowAlert("Invalid Percentage from", true);
                    txtPercentageFrom.Focus();
                }
            }
            if(txtPassedTo.Text.Trim()!="")
            {
                if (Convert.ToInt32(txtPassedTo.Text) <= 0)
                {
                    ShowAlert("Invalid Percentage to", true);
                    txtPassedTo.Focus();

                }
            }
            if (IsNumeric(txtCode.Text))
            {
                ShowAlert("Numeric Value not allowed for code", true);
                txtCode.Focus();
            }
            else
            {
                using (EConnectContext context = new EConnectContext())
                {
                    ResultGrade objResultGrade;
                    string Code = Convert.ToString(txtCode.Text);
                    Int32 courseCatID = Convert.ToInt32(ddlccat.SelectedValue);
                    Int32 versionId = Convert.ToInt32(ddlversion.SelectedValue);
                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        if (context.ResultGrades.Any(s => s.Code.ToUpper() == Code.ToUpper() && s.CourseCategoryID == courseCatID))
                        {
                            throw new Exception("This Result Grade Code Already Exist");
                        }
                        else
                        {
                            objResultGrade = new EConnect.NIELIT.ResultGrade();
                            objResultGrade.Description = Convert.ToString(txtDescription.Text.Trim());
                            objResultGrade.Code = txtCode.Text.Trim().ToString();
                            if (txtPercentageFrom.Text != "")
                                objResultGrade.PercentageFrom = Convert.ToInt32(txtPercentageFrom.Text.Trim());
                            else
                                objResultGrade.PercentageFrom = 0;
                            if (txtPassedTo.Text.Trim() != "")
                                objResultGrade.PercentageTo = Convert.ToInt32(txtPassedTo.Text.Trim());
                            else
                                objResultGrade.PercentageTo = 0;
                            objResultGrade.EffectiveFromDate = Convert.ToDateTime(txtEffectiveFromDate.Text.Trim());
                            if (ddlStatus.SelectedValue == "1")
                                objResultGrade.IsPassed = true;
                            else
                                objResultGrade.IsPassed = false;
                            objResultGrade.CourseCategoryID = Convert.ToInt32(ddlccat.SelectedValue);
                            objResultGrade.VersionID = Convert.ToInt32(ddlversion.SelectedValue);
                            context.ResultGrades.Add(objResultGrade);
                            context.SaveChanges();
                            strMessage = "New record saved.";
                        }
                    }
                    else
                    {

                        Int32 Id = Convert.ToInt32(Request.QueryString["Key"].ToString());
                        if (!(context.ResultGrades.Any(s => s.Code.ToUpper() == Code.ToUpper() && s.CourseCategoryID == courseCatID && s.ID != Id && s.VersionID == versionId)))
                        {
                            objResultGrade = (from c in context.ResultGrades
                                              where c.ID == Id
                                              orderby c.EffectiveFromDate descending
                                              select c).FirstOrDefault();

                            if(Convert.ToInt32(ddlversion.SelectedValue) != objResultGrade.VersionID)
                            {
                                ShowAlert("You cannot change the version no. To change it please save it as a new record",true);
                                return;
                            }
                            objResultGrade.Description = Convert.ToString(txtDescription.Text.Trim());
                            objResultGrade.Code = txtCode.Text.ToString().Trim();
                            if (txtPercentageFrom.Text.Trim() !="")
                                objResultGrade.PercentageFrom = Convert.ToInt32(txtPercentageFrom.Text.Trim());
                            else
                                objResultGrade.PercentageFrom = 0;
                            if (txtPassedTo.Text.Trim() != "")
                                objResultGrade.PercentageTo = Convert.ToInt32(txtPassedTo.Text.Trim());
                            else
                                objResultGrade.PercentageTo = 0;
                            objResultGrade.EffectiveFromDate = Convert.ToDateTime(txtEffectiveFromDate.Text.Trim());
                            if (ddlStatus.SelectedValue == "1")
                                objResultGrade.IsPassed = true;
                            else
                                objResultGrade.IsPassed = false;
                            objResultGrade.CourseCategoryID = Convert.ToInt32(ddlccat.SelectedValue);
                            objResultGrade.VersionID = Convert.ToInt32(ddlversion.SelectedValue);
                            strMessage = "Record updated.";
                            context.SaveChanges();
                        }
                        else
                        {
                            throw new Exception(" This result grade already exists.");
                        }
                    }

                    Response.Redirect("ResultGrade.aspx?msg=" + strMessage);
                    //Response.Redirect("ExamCentreVenu.aspx?msg=" + strMessage + "&key1=" + Request.QueryString["CourseID"]);    
                };
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
            ddlfilterccategory.SelectedValue = "0";
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
            using (EConnectContext context = new EConnectContext())
            {
                if (hfActionID.Value != "")
                {
                    String recordID = hfActionID.Value.Split('$')[0].ToString();
                    LinkButton btnAction = (LinkButton)sender;
                    if (btnAction.CommandName == "Action")
                    {
                        //Load the object and apply validateion if required
                        //call function to perform required action
                        //bind the grid again
                        BindGridView();
                        ShowAlert("Record Action1 successfully.", true);
                        hfActionID.Value = "";
                    }
                    else if (btnAction.CommandName == "Delete")
                    {
                        //Load the object and apply validateion if required
                        //call function to perform required action
                        //bind the grid again
                        if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                        {
                            BreadCrumb1.Render();
                            ShowAlert("Sorry! You don't have rights to delete the records.", true);
                            return;
                        }
                        ResultGrade resultgrade = context.ResultGrades.Find(Convert.ToInt32(recordID));
                        context.ResultGrades.Remove(resultgrade);
                        context.SaveChanges();
                        BindGridView();
                        ShowAlert("Record deleted successfully.", true);
                        hfActionID.Value = "";
                    }
                    uPnlGrid.Update();
                }
            };
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
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h2.NavigateUrl);
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h3.NavigateUrl);
                //HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                //h4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(h4.NavigateUrl);
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

            var ResultGradeList1 = from s in context.ResultGrades
                                  select new { Code = s.Code };
            if (!String.IsNullOrEmpty(searchString))
            {
                ResultGradeList1 = ResultGradeList1.Where(s => s.Code.ToUpper().Contains(searchString));
            }

            ResultGradeList1 = ResultGradeList1.OrderBy(s => s.Code);
            foreach (var c in ResultGradeList1)
            {
                items.Add(c.Code);
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
        Response.Redirect("ResultGrade.aspx", true);
    }
    protected void BindOldGridView()
    {
        try
        {
            //this is the sample code how to bind the grid control
            using (EConnectContext context = new EConnectContext())
            {
                string sortOrder = Convert.ToString(ViewState["SortOrder1"]);
                string sortField = Convert.ToString(ViewState["SortField1"]);
                Int32   gradeID = Convert.ToInt32(Request.QueryString["Key"]);
                var obj =  (from  c in context.ResultGrades
                            where c.ID == gradeID 
                            orderby c.EffectiveFromDate descending
                            select c ).FirstOrDefault();

                var grade = (from t in context.ResultGrades
                           where t.EffectiveFromDate < obj.EffectiveFromDate  && t.Code == obj.Code && t.CourseCategoryID == obj.CourseCategoryID
                           select new
                           {
                               Grade = t.Code,
                               MaxEffectiveFromDate = t.EffectiveFromDate,
                               description = t.Description,
                               ccategory = t.CourseCategory.Name + "(" + t.CourseCategory.Code + ")",
                               Version = t.VersionID.HasValue ? t.VersionID.Value : 0
                           });
                if (grade.Count() > 0)
                {
                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortField)
                        {

                            case "Grade":
                                if (sortOrder == "DESC")
                                    grade = grade.OrderByDescending(s => s.Grade);
                                else
                                    grade = grade.OrderBy(s => s.Grade);
                                break;
                            case "MaxEffectiveFromDate":
                                if (sortOrder == "DESC")
                                    grade = grade.OrderByDescending(s => s.MaxEffectiveFromDate);
                                else
                                    grade = grade.OrderBy(s => s.MaxEffectiveFromDate);
                                break;
                            case "description":
                                if (sortOrder == "DESC")
                                    grade = grade.OrderByDescending(s => s.description);
                                else
                                    grade = grade.OrderBy(s => s.description);
                                break;
                            case "Version":
                                if (sortOrder == "DESC")
                                    grade = grade.OrderByDescending(s => s.Version);
                                else
                                    grade = grade.OrderBy(s => s.Version);
                                break;
                            case "ccategory":
                                if (sortOrder == "DESC")
                                    grade = grade.OrderByDescending(s => s.ccategory);
                                else
                                    grade = grade.OrderBy(s => s.ccategory);
                                break;
                            default:
                                grade = grade.OrderBy(s => s.MaxEffectiveFromDate);
                                break;
                        }
                    }
                    PagingBar1.Bind(grade, ref GridViewOld);
                    UPanelHistory.Update();
                }
                else
                    lblError2.Visible = true;
                    lblError2.Text = "No history found for grade detail..";
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void GridViewOld_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar2.CurrentPageSize * PagingBar2.CurrentPageIndex)).ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void PageIndexChangedOld(Int32 NewPageIndex)
    {
        try
        {
            GridViewOld.PageIndex = PagingBar2.CurrentPageIndex;
            BindOldGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void GridViewOld_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField1"] = e.SortExpression;
            if (ViewState["SortOrder1"].ToString() == "DESC")
                ViewState["SortOrder1"] = "ASC";
            else
                ViewState["SortOrder1"] = "DESC";
            BindOldGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void Btnsaveas_Click(object sender, EventArgs e)
    {
            try
            {
                BreadCrumb1.Render();
                Int32 verid = Convert.ToInt32(ddlversion.SelectedValue);
                if (txtPercentageFrom.Text.Trim() !="")
                {
                    if (Convert.ToInt32(txtPercentageFrom.Text) <= 0)
                    {
                        ShowAlert("Invalid Percentage from", true);
                        txtPercentageFrom.Focus();
                    }
                }
                if (txtPassedTo.Text.Trim() !="")
                {
                    if (Convert.ToInt32(txtPassedTo.Text) <= 0)
                    {
                        ShowAlert("Invalid Percentage to", true);
                        txtPassedTo.Focus();

                    }
                }
                if (IsNumeric(txtCode.Text.Trim()))
                {
                    ShowAlert("Numeric Value not allowed for code", true);
                    txtCode.Focus();
                }
                else
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        Int32 gradeID = Convert.ToInt32(Request.QueryString["Key"]);
                        var grade = (from c in context.ResultGrades
                                        where c.ID ==  gradeID 
                                        orderby c.EffectiveFromDate descending
                                        select new
                                        {
                                            maxdate = c.EffectiveFromDate,
                                            versionid = c.VersionID.HasValue ? c.VersionID.Value : 0,
                                            code = c.Code 
                                        }).FirstOrDefault();
                        DateTime gradeeffectivedate = Convert.ToDateTime(txtEffectiveFromDate.Text);
                        Int32 count = context.ResultGrades.Where(s=>s.Code.ToUpper() == grade.code.ToUpper() && s.VersionID == verid).Count();
                        if (gradeeffectivedate <= grade.maxdate)
                        {
                            ShowAlert("You cannot save this record as a new record because effective from date should be greater than previous effective date of same result  grade", true);
                            return;
                        }
                        else if (Convert.ToInt32(ddlversion.SelectedValue) == grade.versionid)
                        {
                            ShowAlert("You cannot save this record as a new record because new version no.should be greater than previous version no.", true);
                            return;
                        }
                        else if (count > 0)
                        {
                            ShowAlert("Version no :" + ddlversion.SelectedItem.Text + " for this Result Grade already created.", true);
                            return;
                        }
                        else
                        {
                            ResultGrade objResultGrade;
                            objResultGrade = new EConnect.NIELIT.ResultGrade();
                            objResultGrade.Description = Convert.ToString(txtDescription.Text.Trim());
                            objResultGrade.Code = txtCode.Text.ToString().Trim();
                            if (txtPercentageFrom.Text != "")
                                objResultGrade.PercentageFrom = Convert.ToInt32(txtPercentageFrom.Text.Trim());
                            else
                                objResultGrade.PercentageFrom = 0;
                            if (txtPassedTo.Text != "")
                                objResultGrade.PercentageTo = Convert.ToInt32(txtPassedTo.Text.Trim());
                            else
                                objResultGrade.PercentageTo = 0;
                            objResultGrade.EffectiveFromDate = Convert.ToDateTime(txtEffectiveFromDate.Text.Trim());
                            if (ddlStatus.SelectedValue == "1")
                                objResultGrade.IsPassed = true;
                            else
                                objResultGrade.IsPassed = false;
                            objResultGrade.CourseCategoryID = Convert.ToInt32(ddlccat.SelectedValue);
                            objResultGrade.VersionID = Convert.ToInt32(ddlversion.SelectedValue);
                            context.ResultGrades.Add(objResultGrade);
                            context.SaveChanges();
                            strMessage = "New record saved.";
                        }
                        Response.Redirect("ResultGrade.aspx?msg=" + strMessage);
                    };
                }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}