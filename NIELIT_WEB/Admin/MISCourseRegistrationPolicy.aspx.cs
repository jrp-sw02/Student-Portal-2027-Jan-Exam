using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Configuration;
using System.Data.SqlClient;

public partial class Admin_MISCourseRegistrationPolicy : BasePage
{

    String strMessage = string.Empty;
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
                    FillCategories();
                    BindGridView();
                    if (!String.IsNullOrEmpty(Request.QueryString["msg"]))
                    {
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("MIS Course Registration Policy", "Admin/MISCourseRegistrationPolicy.aspx?" + Request.QueryString.ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("MIS Course Registration Policy", "Admin/MISCourseRegistrationPolicy.aspx?" + Request.QueryString.ToString(), ""));
                    }
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

    protected void FillCategories()
    {
        try
        {
            NIELITMISContext context1 = new NIELITMISContext();
            ddlcoursecategory.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context1.NielitCentreCourseCategorys
                               where p.IsActive == true
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 coursecatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            using (NIELITMISContext context1 = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context1.NielitCentreCourses
                                 join d in context1.NielitCourseDurations on p.ID equals d.courseID
                                 where p.CourseCategoryID == coursecatID && p.IsActive == true
                                 orderby (p.Name) ascending
                                 select new { ValueField = d.ID, TextField = p.Name + " (" + p.Code + ")" + " (" + d.courseDurationDays + "Days" + ")" + " (" + d.courseDurationHrs + "Hours" + ")" };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourseName, CourseList.Distinct(), lst);

            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }


    protected void Filllanguage()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var language = from p in context.Languages
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddllanguage, language, lst);
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
            using (NIELITMISContext context1 = new NIELITMISContext())
            {
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                btnSave.Text = "Update";
                lblHeading.Text = "MIS Course Registration Policy Detail";
                Filllanguage();
                FillCategories();
                ddllanguage.Items.Insert(3, "Both");
                Int32 ID = 0;
                if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    ID = Convert.ToInt32(Request.QueryString["ID"]);
                }
                var query = (from s in context1.MISCourse_Registraton_Policy
                             where s.ID == ID
                             select new
                             {
                                 ID = s.ID,
                                 CourseCat = s.CourseCategoryID,
                                 CourseID = s.CourseID,
                                 RegistrationValidity = s.RegistrationValidity,
                                 AllowedLanguage = s.AllowedLanguage,
                                 ReRegistrationChance = s.ReRegistrationChance,
                                 ReRegistrationValidity = s.ReRegistrationValidity,
                                 ReRegistrationGapInMonths = s.ReRegistrationGapInMonths,
                                 EffectiveFromDate = s.EffectiveFromDate
                             }).FirstOrDefault();
                if (query != null)
                {

                    ddlcoursecategory.SelectedValue = Convert.ToInt32(query.CourseCat).ToString();
                    ddlcoursecategory_SelectedIndexChanged(ddlcoursecategory, EventArgs.Empty);
                    ddlcourseName.SelectedValue = query.CourseID.ToString();
                    txtregperiod.Text = query.RegistrationValidity.ToString();
                    if (query.AllowedLanguage.HasValue)
                    {
                        ddllanguage.SelectedValue = Convert.ToInt32(query.AllowedLanguage.Value).ToString();
                    }
                    else
                    {
                        ddllanguage.SelectedIndex = 3;
                    }
                    if (query.ReRegistrationChance == true)
                    {
                        ddlregchances.SelectedValue = "1";
                    }
                    else
                    {
                        ddlregchances.SelectedValue = "2";
                    }
                    if (ddlregchances.SelectedValue == "2")
                    {
                        txtreggape.Visible = false;
                        txtvalidity.Visible = false;
                        Lbrereggap.Visible = false;
                        Lbreregperiod.Visible = false;
                    }
                    if (query.ReRegistrationValidity.HasValue)
                    {
                        txtvalidity.Text = query.ReRegistrationValidity.Value.ToString();
                    }
                    else
                    {
                        txtvalidity.Text = "NA";
                    }
                    if (query.ReRegistrationGapInMonths.HasValue)
                    {
                        txtreggape.Text = query.ReRegistrationGapInMonths.Value.ToString();
                    }
                    else
                    {
                        txtreggape.Text = "NA";
                    }
                    Txtregpolicyeffectivedate.Text = query.EffectiveFromDate.ToString("dd-MMM-yyyy");
                    if (query.EffectiveFromDate <= DateTime.Today)
                    {
                        ddlcoursecategory.Enabled = false;
                        ddlcourseName.Enabled = false;
                        ddlregchances.Enabled = false;
                        txtregperiod.Enabled = false;
                        txtvalidity.Enabled = false;
                        txtreggape.Enabled = false;
                        Txtregpolicyeffectivedate.Enabled = false;
                        Txtregpolicyeffectivedate_CalendarExtender.Enabled = false;
                        ddllanguage.Enabled = false;
                        btnSave.Visible = false;
                    }
                    else
                    {
                        ddlcoursecategory.Enabled = false;
                        ddlcourseName.Enabled = false;
                        btnSave.Visible = true;
                        ddlregchances.Enabled = true;
                        txtregperiod.Enabled = true;
                        txtvalidity.Enabled = true;
                        txtreggape.Enabled = true;
                        Txtregpolicyeffectivedate.Enabled = true;
                        Txtregpolicyeffectivedate_CalendarExtender.Enabled = true;
                    }
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("MIS Course Registration Policy Details", "Admin/MISCourseRegistrationPolicy.aspx?" + Request.QueryString.ToString(), ""));

                    ViewState["LastModifiedOn"] = DateTime.Now;
                }
            }

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
            NIELITMISContext context1 = new NIELITMISContext();

            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();

            var query = from s in context1.MISCourse_Registraton_Policy
                        // where s.CourseID == CouID
                        select new
                        {
                            ID = s.ID,
                            regperiod = s.RegistrationValidity,
                            reregchance = s.ReRegistrationChance == true ? "Yes" : "No",
                            reregperiod = s.ReRegistrationValidity.HasValue ? s.ReRegistrationValidity.Value : 0,
                            rereggap = s.ReRegistrationGapInMonths.HasValue ? s.ReRegistrationGapInMonths.Value : 0,
                            EffectiveDateFrom = s.EffectiveFromDate,
                            CourseID = s.CourseID,
                            CourseCatId = s.CourseCategoryID
                        };

            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "ID":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.ID);
                        else
                            query = query.OrderBy(s => s.ID);
                        break;
                    case "regperiod":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.regperiod);
                        else
                            query = query.OrderBy(s => s.regperiod);
                        break;
                    case "reregchance":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.reregchance);
                        else
                            query = query.OrderBy(s => s.reregchance);
                        break;
                    case "reregperiod":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.reregperiod);
                        else
                            query = query.OrderBy(s => s.reregperiod);
                        break;
                    case "rereggap":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.rereggap);
                        else
                            query = query.OrderBy(s => s.rereggap);
                        break;
                    case "EffectiveDateFrom":
                        if (sortOrder == "DESC")
                            query = query.OrderByDescending(s => s.EffectiveDateFrom);
                        else
                            query = query.OrderBy(s => s.EffectiveDateFrom);
                        break;
                    default:
                        query = query.OrderBy(s => s.ID);
                        break;
                }
            }
            PagingBar1.Bind(query, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
            if (gvMain.Rows.Count <= 0)
            {
                lblError.Text = "No record found.";
                lblError.Visible = true;
                gvMain.Visible = false;
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
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New MIS Course Registration Policy";
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New MIS Course Registration Policy", "", ""));
            Filllanguage();
            ddllanguage.Items.Insert(3, "Both");
        }
        else
        {

            if (!String.IsNullOrEmpty(Request.QueryString["key"]))
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("MISCourseRegistrationPolicy.aspx?CourseId=" + Request.QueryString["key"].ToString()), true);
            }
            else if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("MISCourseRegistrationPolicy.aspx?CourseId=" + Request.QueryString["CourseId"].ToString()), true);
            }
            else
            {
                Response.Redirect("MISCourseRegistrationPolicy.aspx", true);
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
            BreadCrumb1.Render();
            MISCourse_Registraton_Policy currentCourse;
            Int32 CouID = 0;
            Boolean chance = false;
            using (NIELITMISContext context1 = new NIELITMISContext())
            {

                if (String.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    CouID = Convert.ToInt32(ddlcourseName.SelectedValue.Trim());
                    var registeredpolicy = (from c in context1.MISCourse_Registraton_Policy
                                            where c.CourseID == CouID
                                            orderby c.EffectiveFromDate descending
                                            select c).FirstOrDefault();

                    if (registeredpolicy != null)
                    {
                        DateTime regpolicydate = Convert.ToDateTime(Txtregpolicyeffectivedate.Text);
                        if (regpolicydate <= registeredpolicy.EffectiveFromDate)
                        {
                            ShowAlert("You cannot insert the record!", true);
                            return;
                        }
                    }

                    currentCourse = new EConnect.NIELIT.MISCourse_Registraton_Policy();
                    currentCourse.CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue.Trim());
                    currentCourse.CourseID = Convert.ToInt32(ddlcourseName.SelectedValue.Trim());
                    currentCourse.RegistrationValidity = Convert.ToInt32(txtregperiod.Text);
                    currentCourse.EffectiveFromDate = Convert.ToDateTime(Txtregpolicyeffectivedate.Text);
                    if (ddllanguage.SelectedIndex != 3)
                    {
                        currentCourse.AllowedLanguage = Convert.ToInt32(ddllanguage.SelectedValue);
                    }
                    if (ddlregchances.SelectedValue == "1")
                    {
                        chance = true;
                    }
                    else if (ddlregchances.SelectedValue == "2")
                    {
                        chance = false;
                    }
                    currentCourse.ReRegistrationChance = chance;
                    if (txtvalidity.Visible == true)
                    {
                        currentCourse.ReRegistrationValidity = Convert.ToInt32(txtvalidity.Text);
                    }
                    if (txtreggape.Visible == true)
                    {
                        currentCourse.ReRegistrationGapInMonths = Convert.ToInt32(txtreggape.Text);
                    }
                    currentCourse.enterDate = DateTime.Now;
                    currentCourse.enterByID = Convert.ToInt64(Session["UserID"]);
                    context1.MISCourse_Registraton_Policy.Add(currentCourse);
                    context1.SaveChanges();
                    strMessage = "New Record Saved";
                }
                else
                {
                    currentCourse = context1.MISCourse_Registraton_Policy.Find(Convert.ToInt32(Request.QueryString["ID"]));
                    currentCourse.RegistrationValidity = Convert.ToInt32(txtregperiod.Text);
                    if (ddlregchances.SelectedValue == "1")
                    {
                        chance = true;
                    }
                    else if (ddlregchances.SelectedValue == "2")
                    {
                        chance = false;
                        currentCourse.ReRegistrationValidity = null;
                        currentCourse.ReRegistrationGapInMonths = null;
                    }
                    currentCourse.ReRegistrationChance = chance;
                    if (txtvalidity.Visible == true)
                    {
                        currentCourse.ReRegistrationValidity = Convert.ToInt32(txtvalidity.Text);
                    }
                    if (txtreggape.Visible == true)
                    {
                        currentCourse.ReRegistrationGapInMonths = Convert.ToInt32(txtreggape.Text);
                    }
                    currentCourse.EffectiveFromDate = Convert.ToDateTime(Txtregpolicyeffectivedate.Text);
                    if (ddllanguage.SelectedIndex != 3)
                    {
                        currentCourse.AllowedLanguage = Convert.ToInt32(ddllanguage.SelectedValue);
                    }
                    currentCourse.enterDate = DateTime.Now;
                    currentCourse.enterByID = Convert.ToInt64(Session["UserID"]);
                    context1.SaveChanges();
                    strMessage = "Record Updated";
                }
            }
            Response.Redirect("MISCourseRegistrationPolicy.aspx?msg=" + strMessage, false);
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
            NIELITMISContext context1 = new NIELITMISContext();
            if (hfActionID.Value != "")
            {
                Int32 couID = 0;

                Int32 ID = (Convert.ToInt32(hfActionID.Value));
                var courseregpolicy = (from c in context1.MISCourse_Registraton_Policy
                                       where c.ID == ID
                                       select c).FirstOrDefault();

                if (courseregpolicy.EffectiveFromDate <= DateTime.Today)
                {
                    ShowAlert("You cannot delete this record.", true);
                    return;
                }
                else
                {
                    MISCourse_Registraton_Policy regpolicy = context1.MISCourse_Registraton_Policy.Find(Convert.ToInt32(hfActionID.Value));
                    context1.MISCourse_Registraton_Policy.Remove(regpolicy);
                    context1.SaveChanges();
                    BindGridView();
                    ShowAlert("Record deleted successfully.", true);
                    hfActionID.Value = "";
                }
            }
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
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);

                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl);

                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);

                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);

                HyperLink hl4 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl);
              
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("MISCourseRegistrationPolicy.aspx", true);
    }

    protected void ddlregchances_SelectedIndexChanged(object sender, EventArgs e)
    {
        BreadCrumb1.Render();
        if (ddlregchances.SelectedValue == "1")
        {
            Lbreregperiod.Visible = true;
            Lbrereggap.Visible = true;
            txtvalidity.Visible = true;
            txtreggape.Visible = true;
            txtvalidity.Text = "";
            txtreggape.Text = "";
        }
        else if (ddlregchances.SelectedValue == "2")
        {
            Lbreregperiod.Visible = false;
            Lbrereggap.Visible = false;
            txtvalidity.Visible = false;
            txtreggape.Visible = false;
        }
    }
}
