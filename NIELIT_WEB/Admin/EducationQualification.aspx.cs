using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;

public partial class Admin_EducationQualification : BasePage
{
    String strMessage = string.Empty;
    //EConnectContext context = new EConnectContext();
    Int32 currentRoleId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(Request.QueryString.ToString());
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    FillQualificationLevel();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillFilterQualificationLevel();
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Education Qualification", "Admin/EducationQualification.aspx", ""));
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
    protected void FillFilterQualificationLevel()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                ListItem lst = new ListItem("--Select One--", "0");
                var QualificationLevelList = from p in context.QualificationLevels
                                             orderby p.ID
                                             select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlfilterQuali, QualificationLevelList, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterCity(Int32 StateID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                var City = (from s in context.ExamVenues
                            where s.StateID == StateID
                            select new { ValueField = s.City, TextField = s.City }).Distinct();
                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlflCity, City, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillQualificationLevel()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                
                ListItem lst = new ListItem("--Select One--", "0");
                var QualificationLevelList = from p in context.QualificationLevels
                                 orderby p.DisplayOrder
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlQualificationLevel, QualificationLevelList, lst);
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
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Education Qualificatin";
            //tblNavLinks.Visible = true;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 EducationQID = Convert.ToInt32(Request.QueryString["Key"]);
                var EducationQualification = (from s in context.EducationalQualifications
                                              where s.ID == EducationQID
                                 select s).FirstOrDefault();
                ddlQualificationLevel.SelectedValue = EducationQualification.QualificationLevelID.ToString();
                txtEdu.Text = EducationQualification.Name.ToString();
                txtCode.Text = EducationQualification.Code.ToString();
                txtDisplayOrder.Text = EducationQualification.DisplayOrder.ToString();
                txtDisplayOrder.Enabled = false;
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(EducationQualification.Name, "#", ""));
                //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("ExamVenue", "#", ""));
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                //Create an object of record to be modified and assign properties to relevant fields.
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    btnSave.Visible = false;
                }
            }
            ;
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
            //Int64 instituteId = Convert.ToInt64(hfAccreID.Value);
            //lblError.Visible = false;
            using (EConnectContext context = new EConnectContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                Int32 QualifID =0;

                if (ddlfilterQuali.SelectedValue != "0")
                    QualifID = Convert.ToInt32(ddlfilterQuali.SelectedValue);


                var EducationQuali = from s in context.EducationalQualifications
                                     orderby s.DisplayOrder
                                select new
                                {
                                    ID = s.ID,
                                    Name = s.Name,
                                    QualificationLevel = s.QualificationLevel.Name,
                                    QualificationLevelID=s.QualificationLevelID,
                                    Code=s.Code

                                };

                if (!String.IsNullOrEmpty(searchString))
                {
                    EducationQuali = EducationQuali.Where(s => s.Name.ToUpper().Contains(searchString)
                                                           || s.Code.ToUpper().Contains(searchString));
                }
                //if (Activestatus != "")
                //{
                //    ExamVenue = ExamVenue.Where(s => s.IsActive == Activestatus);
                //}
                if (QualifID != 0)
                {
                    EducationQuali = EducationQuali.Where(s => s.QualificationLevelID == QualifID);
                }
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ID":
                            if (sortOrder == "DESC")
                                EducationQuali = EducationQuali.OrderByDescending(s => s.ID);
                            else
                                EducationQuali = EducationQuali.OrderBy(s => s.ID);
                            break;
                        case "Name":
                            if (sortOrder == "DESC")
                                EducationQuali = EducationQuali.OrderByDescending(s => s.Name);
                            else
                                EducationQuali = EducationQuali.OrderBy(s => s.Name);
                            break;
                        case "QualificationLevel":
                            if (sortOrder == "DESC")
                                EducationQuali = EducationQuali.OrderByDescending(s => s.QualificationLevel);
                            else
                                EducationQuali = EducationQuali.OrderBy(s => s.QualificationLevel);
                            break;
                        case "Code":
                            if (sortOrder == "DESC")
                                EducationQuali = EducationQuali.OrderByDescending(s => s.Code);
                            else
                                EducationQuali = EducationQuali.OrderBy(s => s.Code);
                            break;
                        default:
                            EducationQuali = EducationQuali.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(EducationQuali, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                //if (gvMain.Rows.Count <= 0)
                //{
                //    lblError.Text = "No Record Found";
                //    lblError.Visible = true;
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
            //if (!UserManager.HasRight(currentRoleId, enmRight.New))
            //{
            //    BreadCrumb1.Render();
            //    ShowAlert("Sorry! You don't have rights to add new record.",true);
            //    return;
            //}
            FillQualificationLevel();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Education Qualification";
            //Updating Breadcrumb
            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Education Qualification", "#", ""));
        }
        else
        {
            Response.Redirect("EducationQualification.aspx", true);
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
            using (EConnectContext context = new EConnectContext())
            {
                EducationalQualification objEducationQualification;
                string qualfname = txtEdu.Text;
                string code = txtCode.Text;
                Int32 displayOrder = Convert.ToInt32(txtDisplayOrder.Text);
                Int32 qualificationlevelID = Convert.ToInt32(ddlQualificationLevel.SelectedValue);
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    if(context.EducationalQualifications.Any(s => s.Name.ToUpper() == qualfname.ToUpper()))
                    {
                        throw new Exception("This educational qualification name already exists.");
                    }
                    else if(context.EducationalQualifications.Any(s => s.Code.ToUpper() ==code.ToUpper()))
                    {
                        throw new Exception("This educational qualification code already exists.");
                    }
                    else if(context.EducationalQualifications.Any(s => s.DisplayOrder ==displayOrder))
                    {
                        throw new Exception("This educational qualification display order already exists.");
                    }
                    else
                    {
                        objEducationQualification = new EConnect.EducationalQualification();
                        objEducationQualification.QualificationLevelID = Convert.ToInt32(ddlQualificationLevel.SelectedValue);
                        objEducationQualification.Name = Convert.ToString(txtEdu.Text.Trim());
                        objEducationQualification.Code = txtCode.Text.ToString().Trim();
                        objEducationQualification.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim());
                        context.EducationalQualifications.Add(objEducationQualification);
                        context.SaveChanges();
                        strMessage = "New record saved";

                    }
                }
                else
                {
                    Int32 keyID = Convert.ToInt32(Request.QueryString["Key"]);
                    if (!(context.EducationalQualifications.Any(s => s.Name.ToUpper() == qualfname.ToUpper() && s.Code.ToUpper() == code.ToUpper() && s.ID!=keyID)))
                    {
                        objEducationQualification = context.EducationalQualifications.Find(Convert.ToInt32(Request.QueryString["key"]));
                        objEducationQualification.QualificationLevelID = Convert.ToInt32(ddlQualificationLevel.SelectedValue);
                        objEducationQualification.Name = Convert.ToString(txtEdu.Text.Trim());
                        objEducationQualification.Code = txtCode.Text.ToString().ToUpper().Trim();
                        //objEducationQualification.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim());
                        context.SaveChanges();
                        strMessage = "Record updated";
                        
                    }
                    else
                    {
                        throw new Exception("This Educational Qualification Already Exists.");
                    }
                }
                Response.Redirect("EducationQualification.aspx?msg="+strMessage);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message,true);
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
            //ddlflstates.SelectedValue = "0";
            //ddlficoursecategory.SelectedValue = "0";
            //ddlficoursecategory_SelectedIndexChanged(ddlficoursecategory, EventArgs.Empty);
            //ddlflexcentretype.SelectedValue = "0";
            ddlfilterQuali.SelectedValue = "0";
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
                if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to delete the records.", true);
                    return;
                }
                EducationalQualification qualification = context.EducationalQualifications.Find(Convert.ToInt32(hfActionID.Value));
                context.EducationalQualifications.Remove(qualification);
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
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl =EConnect.Utils.Security.QuertStringModule.Encrypt(h2.NavigateUrl);
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl =EConnect.Utils.Security.QuertStringModule.Encrypt(h3.NavigateUrl);
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

            var EduQuali = from s in context.EducationalQualifications
                                 select new { Name = s.Name };


            if (!String.IsNullOrEmpty(searchString))
            {
                EduQuali = EduQuali.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            EduQuali = EduQuali.OrderBy(s => s.Name);

            var EduQuali1 = from s in context.EducationalQualifications
                               select new { Name = s.Code };


            if (!String.IsNullOrEmpty(searchString))
            {
                EduQuali1 = EduQuali1.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            EduQuali1 = EduQuali1.OrderBy(s => s.Name);
            EduQuali = EduQuali.Union(EduQuali1).Take(count);
            foreach (var c in EduQuali)
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
        try
        {
            Response.Redirect("EducationQualification.aspx", true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
       
    }
}