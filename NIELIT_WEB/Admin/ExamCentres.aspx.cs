using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Common;
using EConnect.NIELIT;
public partial class ExamCentres : BasePage
{
    String strMessage = string.Empty;
    //EConnectContext context = new EConnectContext();
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(Request.QueryString.ToString());
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
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
                    BindEditNewModeData();
                    ShowEditMode();
                }
                else
                {
                    BindListData();
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BindGridView();
                    if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Centres", "Admin/ExamCentres.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Centres", "Admin/ExamCentres.aspx", ""));
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
    protected void BindEditNewModeData()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.CourseCategories
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    Category = Category.Where(a => roleCourses.Contains(a.ValueField));
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategory, Category.Distinct(), lst);
                //if (Convert.ToInt32(ddlCourseCategory.SelectedValue) == 2)
                //{
                //    tdRc.Visible = true;
                //}
                //else
                //{
                //    tdRc.Visible = false;
                //}

                ListItem lst1 = new ListItem("--Select One--", "0");
                var state = from s in context.Locations
                            where s.LocationTypeID == 2
                            select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlState, state, lst1);

                EnumUtility.BindListObject(ref ddlExamCentreType, typeof(EConnect.NIELIT.enmExamCenterType), null);
            }
            if (!String.IsNullOrEmpty(Request.QueryString["CategoryID"]))
            {
                ddlCourseCategory.SelectedValue = Request.QueryString["CategoryID"].ToString();
                ddlCourseCategory.Enabled = false;
                if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]) && ddlCourseCategory.SelectedValue != "1")
                {
                   // ddlcoursecategory_SelectedIndexChanged(ddlficoursecategory, EventArgs.Empty);
                   // ddlCourse.SelectedValue = Request.QueryString["CourseId"].ToString();
                   // ddlCourse.Enabled = false;
                }
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
                ListItem lst = new ListItem("--All--", "0");
                var Category = from p in context.CourseCategories
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    Category = Category.Where(a => roleCourses.Contains(a.ValueField));
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlficoursecategory, Category.Distinct(), lst);

                ListItem lst1 = new ListItem("--All--", "0");
                var state = from s in context.Locations
                            where s.LocationTypeID == 2
                            select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlflstates, state, lst1);

                //EnumUtility.BindListObject(ref ddlflexcentretype, typeof(EConnect.NIELIT.enmExamCenterType), new ListItem("--All--", "0"));
            }
            if (!String.IsNullOrEmpty(Request.QueryString["CategoryID"]))
            {
                ddlficoursecategory.SelectedValue = Request.QueryString["CategoryID"].ToString();
                ddlficoursecategory.Enabled = false;
                if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]) && ddlficoursecategory.SelectedValue !="1")
                {
                   // ddlficoursecategory_SelectedIndexChanged(ddlficoursecategory, EventArgs.Empty);
                   // ddlflcourse.SelectedValue = Request.QueryString["CourseId"].ToString();
                   // ddlflcourse.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillCourses(DropDownList ddl, Int32 courseCategoryID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ddl.Items.Clear();
                ListItem lst = new ListItem("--All--", "0");
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == courseCategoryID
                                 orderby p.DisplayOrder
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, CourseList, lst);
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
            lblHeading.Text = "Exam Centres";
            tblNavLinks.Visible = true;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 ExcentreId = Convert.ToInt32(Request.QueryString["Key"]);
                var Examcentre = (from p in context.ExamCenters
                                  where p.ID == ExcentreId
                                  select p).FirstOrDefault();
                ddlCourseCategory.SelectedValue = Examcentre.CourseCategoryID.ToString();
                ddlcoursecategory_SelectedIndexChanged(ddlCourse, EventArgs.Empty);
                if(Examcentre.CourseID.HasValue)
                    ddlCourse.SelectedValue = Examcentre.CourseID.Value.ToString();
                ddlCourseCategory.Enabled = false;
                ddlCourse.Enabled = false;
                //ddlExamCentreType.SelectedValue = Examcentre.ExamCentreTypeID.ToString();
                txtCenterName.Text = Examcentre.Name.ToString().ToUpper();
                txtCode.Text = Examcentre.Code.ToString().ToUpper();
                ddlState.SelectedValue = Examcentre.StateID.ToString();
                ddlState.Enabled = false;
                if (!string.IsNullOrEmpty(Request.QueryString["CourseId"]))
                {
                    tblNavLinks.Visible = true;
                    hlExamMenu.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("ExamCentreVenu.aspx?ExamCentreId=" + Request.QueryString["Key"] + "&CourseId=" + Request.QueryString["CourseId"]);
                }
                else
                {
                    tblNavLinks.Visible = false;
                }
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(Examcentre.Name, "Admin/ExamCentres.aspx?" + Request.QueryString.ToString(), ""));
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                //Create an object of record to be modified and assign properties to relevant fields.
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
            //Int64 instituteId = Convert.ToInt64(hfAccreID.Value);
           using(EConnectContext context = new EConnectContext())
           {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                Int64 stateID = 0;
                Int32 CourseCategoryID = 0;
                Int32 courseID = 0;
                Int32 examCenterTypeID = 0;
                if (ddlflstates.SelectedValue != "0")
                    stateID = Convert.ToInt64(ddlflstates.SelectedValue);
                if (ddlficoursecategory.SelectedValue != "0")
                    CourseCategoryID = Convert.ToInt32(ddlficoursecategory.SelectedValue);
                if (ddlflcourse.SelectedValue != "0")
                    courseID = Convert.ToInt32(ddlflcourse.SelectedValue);
                //if (ddlflexcentretype.SelectedValue != "0")
                //    examCenterTypeID = Convert.ToInt32(ddlflexcentretype.SelectedValue);
                var ExamCentre = from s in context.ExamCenters
                                 select new { ID = s.ID, Name = s.Name, Code = s.Code, State =  s.State.Name,
                                              StateID = s.StateID,
                                              Course = s.CourseCategory.Code + " - " + (s.CourseID.HasValue ? s.Course.Code : "All"),
                                              CourseCategoryID = s.CourseCategoryID,
                                              CourseId = s.CourseID.HasValue ? s.CourseID.Value :0,
                                              CentreType = s.ExamCentreTypeID,
                                              DistrictID = s.DistrictID,
                                              IsActive =   s.IsEnabled ? "Active" : "InActive",
                                 };
                if (!String.IsNullOrEmpty(searchString))
                {
                    ExamCentre = ExamCentre.Where(s => s.Name.ToUpper().Contains(searchString) || s.Code.ToUpper().Contains(searchString));
                }

                if (stateID != 0)
                {
                    ExamCentre = ExamCentre.Where(s => s.StateID == stateID);
                }

                if (CourseCategoryID != 0)
                {
                    ExamCentre = ExamCentre.Where(s => s.CourseCategoryID == CourseCategoryID);
                }
                if (courseID != 0)
                {
                    ExamCentre = ExamCentre.Where(s => s.CourseId == courseID);
                }
                if (examCenterTypeID != 0)
                {
                    ExamCentre = ExamCentre.Where(s => s.CentreType == examCenterTypeID);
                }

                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ID":
                            if (sortOrder == "DESC")
                                ExamCentre = ExamCentre.OrderByDescending(s => s.ID);
                            else
                                ExamCentre = ExamCentre.OrderBy(s => s.ID);
                            break;
                        case "Name":
                            if (sortOrder == "DESC")
                                ExamCentre = ExamCentre.OrderByDescending(s => s.Name);
                            else
                                ExamCentre = ExamCentre.OrderBy(s => s.Name);
                            break;
                        case "Code":
                            if (sortOrder == "DESC")
                                ExamCentre = ExamCentre.OrderByDescending(s => s.Code);
                            else
                                ExamCentre = ExamCentre.OrderBy(s => s.Code);
                            break;
                        case "State":
                            if (sortOrder == "DESC")
                                ExamCentre = ExamCentre.OrderByDescending(s => s.State);
                            else
                                ExamCentre = ExamCentre.OrderBy(s => s.State);
                            break;
                        case "Course":
                            if (sortOrder == "DESC")
                                ExamCentre = ExamCentre.OrderByDescending(s => s.Course);
                            else
                                ExamCentre = ExamCentre.OrderBy(s => s.Course);
                            break;
                        case "CentreType":
                            if (sortOrder == "DESC")
                                ExamCentre = ExamCentre.OrderByDescending(s => s.CentreType);
                            else
                                ExamCentre = ExamCentre.OrderBy(s => s.CentreType);
                            break;
                        default:
                            ExamCentre = ExamCentre.OrderBy(s => s.Name);
                            break;
                    }
                }
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    ExamCentre = ExamCentre.Where(a => roleCourses.Contains(a.CourseCategoryID));
                }
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    ExamCentre = ExamCentre.Where(a => roleCourses.Contains(a.CourseId));
                }
                PagingBar1.Bind(ExamCentre, ref gvMain);
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
            if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to add new record.", true);
                return;
            }
            BindEditNewModeData();
            ddlExamCentreType.SelectedValue = "3";
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Exam Centres";
            //Updating Breadcrumb
            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("ExamCentres.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString()), true);
            }
            else
            {
                Response.Redirect("ExamCentres.aspx", true);
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
            using (EConnectContext context = new EConnectContext())
            {
                Int32 centerID = 0;
                //if (Context.Menores.Any(s => s.Solicitud.fiExpEmpleado == someValue))
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    centerID = Convert.ToInt32(Request.QueryString["Key"]);
                if (context.ExamCenters.Any(s => (s.Code.ToUpper() == txtCode.Text.ToUpper() && s.ID != centerID)))
                {
                    throw new Exception("This Code Is Already Exist");
                }
                else
                {
                    ExamCenter objExCentre;
                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {

                        objExCentre = new EConnect.NIELIT.ExamCenter();
                        objExCentre.CourseCategoryID = Convert.ToInt32(ddlCourseCategory.SelectedValue);
                        if(ddlCourse.SelectedValue !="0")
                            objExCentre.CourseID = Convert.ToInt32(ddlCourse.SelectedValue);
                        objExCentre.ExamCentreTypeID = Convert.ToInt32(enmExamCenterType.Both);
                        objExCentre.Name = txtCenterName.Text.ToString().ToUpper();
                        objExCentre.Code = txtCode.Text.ToString().ToUpper();
                        objExCentre.StateID = Convert.ToInt64(ddlState.SelectedValue);

                        objExCentre.DistrictID = Convert.ToInt64(ddlDistrict.SelectedValue);
                        objExCentre.RegionalCentreId = Convert.ToInt32(ddlRC.SelectedValue);
                        objExCentre.IsEnabled = true;
                        context.ExamCenters.Add(objExCentre);
                        context.SaveChanges();
                        strMessage = "New record saved.";
                    }
                    else
                    {
                        objExCentre = context.ExamCenters.Find(Convert.ToInt32(Request.QueryString["key"]));
                        objExCentre.CourseCategoryID = Convert.ToInt32(ddlCourseCategory.SelectedValue);
                        if (ddlCourse.SelectedValue != "0")
                            objExCentre.CourseID = Convert.ToInt32(ddlCourse.SelectedValue);
                        //objExCentre.ExamCentreTypeID = Convert.ToInt32(ddlExamCentreType.SelectedValue);
                        objExCentre.Name = txtCenterName.Text.ToString().ToUpper();
                        objExCentre.Code = txtCode.Text.ToString().ToUpper();
                        objExCentre.StateID = Convert.ToInt64(ddlState.SelectedValue);

                        objExCentre.RegionalCentreId = Convert.ToInt32(ddlRC.SelectedValue);
                        objExCentre.DistrictID = Convert.ToInt64(ddlDistrict.SelectedValue);

                        strMessage = "Record updated.";
                        context.SaveChanges();
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                    {
                        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("ExamCentres.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString()), true);
                    }
                    else
                    {
                        Response.Redirect("ExamCentres.aspx", true);
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
            ddlflstates.SelectedValue = "0";
            ddlficoursecategory.SelectedValue = "0";
            ddlficoursecategory_SelectedIndexChanged(ddlficoursecategory, EventArgs.Empty);
            //ddlflexcentretype.SelectedValue = "0";
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
    //protected void PerformPopupAction(object sender, EventArgs e)
    //{
    //   try
    //    {
    //        //using (EConnectContext context = new EConnectContext())
    //        //{
    //            if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
    //            {
    //                BreadCrumb1.Render();
    //                ShowAlert("Sorry! You don't have rights to delete the records.",true);
    //                return;
    //             }
    //        //    ExamCenter examcenter = context.ExamCenters.Find(Convert.ToInt32(hfActionID.Value.ToString()));
    //        //    context.ExamCenters.Remove(examcenter);
    //        //    context.SaveChanges();
    //        //    BindGridView();
    //        //    ShowAlert("Record deleted successfully.",true);
    //        //    hfActionID.Value = "";
    //        //};
    //        uPnlGrid.Update();
    //    }
    //    catch (Exception ex)
    //    {
    //        BindGridView();
    //        uPnlGrid.Update();
    //        ShowAlert("Record can not be deleted!", true);
    //    }
    //}
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                string href = hl.NavigateUrl;
                if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                {
                    href += "&CourseId=" + Request.QueryString["CourseId"].ToString();
                }
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = hl.NavigateUrl;
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = hl.NavigateUrl;
                HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                h4.NavigateUrl = hl.NavigateUrl;

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
            //Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();

            var Examcentre = from s in context.ExamCenters
                             select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                Examcentre = Examcentre.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            Examcentre = Examcentre.OrderBy(s => s.Name);

            var ExamCode = from c in context.ExamCenters
                           select new { Name = c.Code };
            if (!String.IsNullOrEmpty(searchString))
            {
                ExamCode = ExamCode.Where(c => c.Name.ToUpper().Contains(searchString));
            }
            Examcentre = Examcentre.Union(ExamCode).Take(count);
            foreach (var c in Examcentre)
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
        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
        {
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("ExamCentres.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString() ), true);
        }
        else
        {
            Response.Redirect("ExamCentres.aspx", true);
        }
    }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try 
        {
            if ( Convert.ToInt32(ddlCourseCategory.SelectedValue) == 2)
            {
                tdRc.Visible = true;
                ddlRC.Visible = true;
            }
            else
            {
                tdRc.Visible = false;
                ddlRC.Visible = false;
            }

            FillCourses(ddlCourse, Convert.ToInt32(ddlCourseCategory.SelectedValue));
            if (ddlCourseCategory.SelectedValue == "0")
            {
                ddlState.SelectedIndex = 0;
                txtCode.Text = "";
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message,true);
        }
    }
    protected void ddlficoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try 
        { 
            FillCourses(ddlflcourse, Convert.ToInt32(ddlficoursecategory.SelectedValue));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message,true);
        }
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int64 stateID = Convert.ToInt64(ddlState.SelectedValue);
                Int32 coursecategoryid = Convert.ToInt32(ddlCourseCategory.SelectedValue);

                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("-- Select --", "0");

                var districtList = from l in context.Locations
                                   where l.ParentLocationID == stateID && l.LocationTypeID == 4
                                   select new { ValueField = l.ID, TextField = l.Name };

                districtList = districtList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlDistrict, districtList, lst);                                
                //if (coursecategoryid == 1)
                //    txtCode.Text = context.Locations.Where(l => l.ID == stateID).Select(l => l.Code).FirstOrDefault().ToUpper();
                //else
                //    txtCode.Text = "";
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            //Int32 courseCategoryId = Convert.ToInt32(ddlCourseCategory.SelectedValue);
            //if (courseCategoryId == 2)
            //{
            //    ddlRC.Visible = true;
            //    lblRc.Visible = true;
            //    tdRc.Visible = true;
            //}
            //else
            //{
            //    ddlRC.Visible = false;
            //    lblRc.Visible = false;
            //    tdRc.Visible = false;
            //}
               
            Int32 stateId = Convert.ToInt32(ddlState.SelectedValue); 


            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("-- Select --", "0");
            using (EConnectContext context = new EConnectContext())
            {
                var reginalCentreList = from c in context.CourseWiseStates
                                        join l in context.Locations on c.StateID equals l.ParentLocationID
                                        where l.ParentLocationID == stateId
                                        select new { ValueField = c.RegionalCenter.ID, TextField = c.RegionalCenter.Name };



                reginalCentreList = reginalCentreList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlRC, reginalCentreList, lst);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void lbdisable_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to edit the records.", true);
                    return;
                }
                ExamCenter examcenter = context.ExamCenters.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                if (examcenter.IsEnabled == true)
                    examcenter.IsEnabled = false;
                else
                    examcenter.IsEnabled = true;
                context.Entry(examcenter).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                BindGridView();
                ShowAlert("You have successfully changed the status of the Exam Center.", true);
                hfActionID.Value = "";
            };
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be edited!", true);
        }
    }
    protected void ddlRC_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int64 stateID = Convert.ToInt64(ddlState.SelectedValue);
                Int32 coursecategoryid = Convert.ToInt32(ddlCourseCategory.SelectedValue);

                if (coursecategoryid == 1)
                    txtCode.Text = context.Locations.Where(l => l.ID == stateID).Select(l => l.Code).FirstOrDefault().ToUpper();
                else
                    txtCode.Text = "";
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}