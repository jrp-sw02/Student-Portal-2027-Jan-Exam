using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class Admin_RegistrationPolicy : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
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
                    BindGridView();
                    if (!String.IsNullOrEmpty(Request.QueryString["msg"]))
                    {
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registration Policy", "Admin/RegistrationPolicy.aspx?CourseId=" + Request.QueryString["CourseId"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registration Policy", "Admin/RegistrationPolicy.aspx?" + Request.QueryString.ToString(), ""));
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
            context = new EConnectContext();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Registration Policy Detail";
            Filllanguage();
            ddllanguage.Items.Insert(3, "Both");
            Int32 CouID = 0;
            Int32 ID = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                CouID = Convert.ToInt32(Request.QueryString["Key"]);
            }
            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                ID = Convert.ToInt32(Request.QueryString["ID"]);
            }
             var query = (from s in context.CourseRegistrationPolicies
                          where s.CourseID == CouID && s.ID == ID
                            select s).FirstOrDefault();
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
                 btnSave.Visible = true;
                 ddlregchances.Enabled = true;
                 txtregperiod.Enabled = true;
                 txtvalidity.Enabled = true;
                 txtreggape.Enabled = true;
                 Txtregpolicyeffectivedate.Enabled = true;
                 Txtregpolicyeffectivedate_CalendarExtender.Enabled = true;
             }
             BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Registration Policy Details", "Admin/RegistrationPolicy.aspx?" + Request.QueryString.ToString(), ""));
            //tblNavLinks.Visible = true;
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
            Int32 CouID = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                CouID = Convert.ToInt32(Request.QueryString["CourseId"]);
            }
            
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            Course currentCourse = context.Courses.Find(CouID);
            if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
            {
                var query = from s in context.CourseRegistrationPolicies
                            where s.CourseID == CouID
                            select new
                            {
                                ID = s.ID,
                                regperiod = s.RegistrationValidity,
                                reregchance = s.ReRegistrationChance == true ? "Yes" : "No",
                                reregperiod = s.ReRegistrationValidity.HasValue ? s.ReRegistrationValidity.Value: 0,
                                rereggap = s.ReRegistrationGapInMonths.HasValue ? s.ReRegistrationGapInMonths.Value:0,
                                EffectiveDateFrom = s.EffectiveFromDate,
                                CourseID = s.CourseID
                            };

                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
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
                if (gvMain.Rows.Count == 0)
                {
                    btnMode.Visible = true;
                }
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "No Record Found!";
                btnMode.Visible = false;
            }
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
            lblHeading.Text = "New Registration Policy";
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Registration Policy", "", ""));
            Filllanguage();
            ddllanguage.Items.Insert(3, "Both");
        }
        else
        {

            if (!String.IsNullOrEmpty(Request.QueryString["key"]))
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("RegistrationPolicy.aspx?CourseId=" + Request.QueryString["key"].ToString()), true);
            }
            else if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("RegistrationPolicy.aspx?CourseId=" + Request.QueryString["CourseId"].ToString()), true);
            }
            else
            {
                Response.Redirect("RegistrationPolicy.aspx", true);
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
            CourseRegistrationPolicy currentCourse;
            Int32 CouID = 0;
            Boolean chance= false;
            EConnectContext context = new EConnectContext();
            if (String.IsNullOrEmpty(Request.QueryString["ID"]))
            {
                CouID = Convert.ToInt32(Request.QueryString["CourseId"]);
                var registeredpolicy = (from c in context.CourseRegistrationPolicies
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
                Course regcourse = context.Courses.Find(CouID);
                currentCourse = new EConnect.NIELIT.CourseRegistrationPolicy();
                currentCourse.CourseID = regcourse.ID;
                currentCourse.CourseCategoryID = regcourse.CourseCategoryID;
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
                context.CourseRegistrationPolicies.Add(currentCourse);
                context.SaveChanges();
                strMessage = "New Record Saved";
            }
            else
            {
                currentCourse = context.CourseRegistrationPolicies.Find(Convert.ToInt32(Request.QueryString["ID"]));
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
                if (ddllanguage.SelectedIndex!=3)
                {
                    currentCourse.AllowedLanguage = Convert.ToInt32(ddllanguage.SelectedValue);
                }
                context.Entry(currentCourse).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                strMessage = "Record Updated";
            }
            if (!String.IsNullOrEmpty(Request.QueryString["key"]))
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("RegistrationPolicy.aspx?CourseId=" + Request.QueryString["key"].ToString()+"&msg=" + strMessage), true);
            }
            else if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("RegistrationPolicy.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&msg=" + strMessage), true);
            }
            else
            {
                Response.Redirect("RegistrationPolicy.aspx", true);
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
                Int32 couID=0;
                if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                {
                    couID = Convert.ToInt32(Request.QueryString["CourseId"]);
                }
                Int32 ID = (Convert.ToInt32(hfActionID.Value));
                var courseregpolicy =  (from c in context.CourseRegistrationPolicies
                                       where c.ID == ID 
                                       select c).FirstOrDefault();
               
                if (courseregpolicy.EffectiveFromDate <= DateTime.Today)
                {
                    ShowAlert("You cannot delete this record.", true);
                    return;
                }
                else
                {
                    CourseRegistrationPolicy regpolicy = context.CourseRegistrationPolicies.Find(Convert.ToInt32(hfActionID.Value));
                    context.CourseRegistrationPolicies.Remove(regpolicy);
                    context.SaveChanges();
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
        finally { context.Dispose(); }
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

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                Image imgAction = (Image)e.Row.FindControl("imgAction");
                imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();

                CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (!String.IsNullOrEmpty(Request.QueryString["key"]))
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("RegistrationPolicy.aspx?CourseId=" + Request.QueryString["key"].ToString()), true);
            }
            else if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("RegistrationPolicy.aspx?CourseId=" + Request.QueryString["CourseId"].ToString()), true);
            }
            else
            {
                Response.Redirect("RegistrationPolicy.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
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
        else if(ddlregchances.SelectedValue == "2")
        {
            Lbreregperiod.Visible = false;
            Lbrereggap.Visible = false;
            txtvalidity.Visible = false;
            txtreggape.Visible = false;
        }
    }
}