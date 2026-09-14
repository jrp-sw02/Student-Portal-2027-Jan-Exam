using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;
public partial class FeeDetail : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblError2.Visible = false;
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
                    ShowEditMode();
                }
                else
                {
                    BindCourseCategory(ddlCourseCategory, "--ALL--");
                    BindFeeType(ddlFeeType, "--ALL--", false, 0);
                    if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseId"])))
                    {
                        using (var context = new EConnectContext())
                        {
                            Int32 myCourseId1 = Convert.ToInt32(Request.QueryString["CourseId"]);
                            var courses = context.Courses.Find(myCourseId1);

                            ddlCourseCategory.SelectedValue = courses.CourseCategoryID.ToString();
                            ddlCourseCategory_SelectedIndexChanged(ddlCourseName, EventArgs.Empty);
                            ddlCourseName.SelectedValue = courses.ID.ToString();
                            ddlCourseName.Enabled = false;
                            ddlCourseCategory.Enabled = false;
                        };
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Fee Detail", "Admin/FeeDetail.aspx?CourseId=" + Request.QueryString["CourseId"], ""));
                    }
                    else
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Fee Detail", "Admin/FeeDetail.aspx", ""));
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BindGridView();
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
    protected void ShowEditMode()
    {

        try
        {

            using (EConnectContext context = new EConnectContext())
            {
                BindCourseCategory(ddlCourseCategoryE, "--Select One--");
                ddlCourseNameE.Enabled = false;
                ddlCourseCategoryE.Enabled = false;
                Int32 Id = Convert.ToInt32(Request.QueryString["Key"]);
                var obj = context.FeeDetails.Find(Id);
                ddlCourseCategoryE.SelectedValue = obj.CourseCategoryID.ToString();
                ddlCourseCategoryE_SelectedIndexChanged(ddlCourseNameE, EventArgs.Empty);
                ddlCourseNameE.SelectedValue = obj.CourseID.ToString();
                BindFeeType(ddlFeeTypeE, "--Select One--", false, 0);
                ddlFeeTypeE.SelectedValue = obj.FeeTypeID.ToString();
                ddlFeeTypeE.Enabled = false;
                TxtFeeAmount.Text = obj.FeeAmount.ToString() + ".00";
                txtEffectiveFromDate.Text = obj.EffectiveFromDate.ToString("dd-MMM-yyyy");
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                btnSave.Text = "Update";
                lblHeading.Text = "Edit Fee Detail";

                if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseId"])))
                {
                    ddlCourseCategoryE.Enabled = false;
                    ddlCourseNameE.Enabled = false;
                    ddlFeeTypeE.Enabled = false;
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(obj.FeeType.Name.ToString(), "Admin/FeeDetail.aspx?key=" + Request.QueryString["key"].ToString() + "&CourseId=" + Convert.ToString(Request.QueryString["CourseId"]), ""));
                }
                else
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(obj.FeeType.Name.ToString(), "Admin/FeeDetail.aspx?key=" + Request.QueryString["key"].ToString(), ""));
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                ViewState["SortOrder1"] = "";
                ViewState["SortField1"] = "";
                //Create an object of record to be modified and assign properties to relevant fields.
                BindOldGridView();
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    btnSave.Visible = false;
                }
            };

        }
        catch (Exception ex)
        {
            throw ex;
        }

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
                Int32 Id = Convert.ToInt32(Request.QueryString["Key"]);
                var obj = context.FeeDetails.Find(Id);
                var fee = (from t in context.FeeDetails
                           where t.EffectiveFromDate < obj.EffectiveFromDate && t.CourseID == obj.CourseID && t.FeeTypeID == obj.FeeTypeID
                           //(context.FeeDetails
                           select new
                           {
                               FeeTypeId = t.FeeTypeID,
                               FeeType = t.FeeType.Name,
                               FeeAmt = t.FeeAmount,
                               MaxEffectiveFromDate = t.EffectiveFromDate
                           });
                if (fee.Count() > 0)
                {
                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortField)
                        {

                            case "FeeType":
                                if (sortOrder == "DESC")
                                    fee = fee.OrderByDescending(s => s.FeeType);
                                else
                                    fee = fee.OrderBy(s => s.FeeType);
                                break;
                            case "MaxEffectiveFromDate":
                                if (sortOrder == "DESC")
                                    fee = fee.OrderByDescending(s => s.MaxEffectiveFromDate);
                                else
                                    fee = fee.OrderBy(s => s.MaxEffectiveFromDate);
                                break;
                            case "FeeAmt":
                                if (sortOrder == "DESC")
                                    fee = fee.OrderByDescending(s => s.FeeAmt);
                                else
                                    fee = fee.OrderBy(s => s.FeeAmt);
                                break;
                            default:
                                fee = fee.OrderBy(s => s.MaxEffectiveFromDate);
                                break;
                        }
                    }
                    PagingBar1.Bind(fee, ref GridViewOld);
                    UPanelHistory.Update();
                }
                else
                    lblError2.Visible = true;
                lblError2.Text = "No history found for fee detail..";
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void BindCourseCategory(DropDownList ddl, string lstStr)
    {

        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem(lstStr, "0");
                var courses = from s in context.CourseCategories
                              select new { ValueField = s.ID, TextField = s.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, courses.Distinct(), lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void BindFeeType(DropDownList ddl, string lstStr, Boolean isNewMode, int CourseId)
    {

        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem(lstStr, "0");
                var feeType = from s in context.FeeTypes
                              select new { ValueField = s.ID, TextField = s.Name };

                if (isNewMode)
                {

                    if (CourseId != 0)
                    {
                        var exceptionList = context.FeeDetails.Where(f => f.CourseID == CourseId).Select(f => f.FeeTypeID).Distinct();
                        var feeType1 = from s in context.FeeTypes
                                       where !exceptionList.Contains(s.ID)
                                       select new { ValueField = s.ID, TextField = s.Name };

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddl, feeType1, lst);
                    }

                }
                else
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddl, feeType, lst);

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    public void bindCourse(int courseCatId, DropDownList ddl, string lstStr)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem(lstStr, "0");
                var courses = from s in context.Courses
                              where s.CourseCategoryID == courseCatId
                              select new { ValueField = s.ID, TextField = s.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    courses = courses.Where(a => roleCourses.Contains(a.ValueField));
                }
                courses = courses.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, courses, lst);
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
            //this is the sample code how to bind the grid control
            using (EConnectContext context = new EConnectContext())
            {
                int CourseCategoryId = Convert.ToInt32(ddlCourseCategory.SelectedValue);
                int CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
                int FeeTypeId = Convert.ToInt32(ddlFeeType.SelectedValue);

                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();


                var fee = (from t in context.FeeDetails
                           where t.EffectiveFromDate == (from c in context.FeeDetails
                                                         where c.CourseID == t.CourseID && c.FeeTypeID == t.FeeTypeID
                                                         select c.EffectiveFromDate).Max()

                           orderby t.Course.DisplayOrder, t.FeeTypeID, t.EffectiveFromDate
                           select new
                           {

                               CourseId = t.CourseID,
                               CourseName = (!string.IsNullOrEmpty(t.CourseCategory.Code) ? t.CourseCategory.Code : "All") + "-" + (!string.IsNullOrEmpty(t.Course.Code) ? t.Course.Code : "All"),
                               CourseCategoryId = t.CourseCategoryID,
                               CourseCategory = t.CourseCategory.Name,
                               FeeTypeId = t.FeeTypeID,
                               FeeType = t.FeeType.Name,
                               FeeAmount = t.FeeAmount,
                               ID = t.ID,
                               MaxEffectiveFromDate = t.EffectiveFromDate
                           });

                if (CourseCategoryId != 0)
                    fee = fee.Where(s => s.CourseCategoryId == CourseCategoryId);
                if (CourseId != 0)
                    fee = fee.Where(s => s.CourseId == CourseId);
                if (FeeTypeId != 0)
                    fee = fee.Where(s => s.FeeTypeId == FeeTypeId);

                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "CourseCategory":
                            if (sortOrder == "DESC")
                                fee = fee.OrderByDescending(s => s.CourseCategory);
                            else
                                fee = fee.OrderBy(s => s.CourseCategory);
                            break;
                        case "CourseName":
                            if (sortOrder == "DESC")
                                fee = fee.OrderByDescending(s => s.CourseName);
                            else
                                fee = fee.OrderBy(s => s.CourseName);
                            break;
                        case "FeeType":
                            if (sortOrder == "DESC")
                                fee = fee.OrderByDescending(s => s.FeeType);
                            else
                                fee = fee.OrderBy(s => s.FeeType);
                            break;
                        case "FeeAmount":
                            if (sortOrder == "DESC")
                                fee = fee.OrderByDescending(s => s.FeeAmount);
                            else
                                fee = fee.OrderBy(s => s.FeeAmount);
                            break;
                        case "MaxEffectiveFromDate":
                            if (sortOrder == "DESC")
                                fee = fee.OrderByDescending(s => s.MaxEffectiveFromDate);
                            else
                                fee = fee.OrderBy(s => s.MaxEffectiveFromDate);
                            break;
                        default:
                            fee = fee.OrderBy(s => s.MaxEffectiveFromDate);
                            break;
                    }
                }
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    fee = fee.Where(a => roleCourses.Contains(a.CourseId));
                }
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    fee = fee.Where(a => roleCourses.Contains(a.CourseCategoryId));
                }
                PagingBar1.Bind(fee, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
            };
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
                //DisableFilter();
                BindCourseCategory(ddlCourseCategoryE, "--Select One--");
                DivHistory.Visible = false;
                lblError2.Visible = false;
                GridViewOld.Visible = false;
                PagingBar1.Visible = false;
                UPanelHistory.Update();
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                //BindGridView();
                //Change the heading text as required
                lblHeading.Text = "New Fee Detail";
                if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseId"])))
                {
                    using (var context = new EConnectContext())
                    {
                        Int32 myCourseId1 = Convert.ToInt32(Request.QueryString["CourseId"]);

                        var courses = (from s in context.Courses
                                       where s.ID == myCourseId1
                                       select s).FirstOrDefault();
                        ddlCourseCategoryE.SelectedValue = courses.CourseCategoryID.ToString();
                        ddlCourseCategoryE_SelectedIndexChanged(ddlCourseNameE, EventArgs.Empty);
                        ddlCourseNameE.SelectedValue = courses.ID.ToString();
                        ddlCourseNameE.Enabled = false;
                        ddlCourseCategoryE.Enabled = false;
                        BindFeeType(ddlFeeTypeE, "-Select One--", true, Convert.ToInt32(ddlCourseNameE.SelectedValue));
                    };
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Fee Detail", "", ""));
                }
                else
                {
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Fee Detail", "", ""));
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseId"])))
                    Response.Redirect("FeeDetail.aspx?CourseId=" + Convert.ToString(Request.QueryString["CourseId"]));
                else
                    Response.Redirect("FeeDetail.aspx");
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
    protected bool isDuplicate()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                int CourseCatId = Convert.ToInt32(ddlCourseCategoryE.SelectedValue);
                int CourseId = Convert.ToInt32(ddlCourseNameE.SelectedValue);
                int FeeType = Convert.ToInt32(ddlFeeTypeE.SelectedValue);
                DateTime Inputdate = Convert.ToDateTime(txtEffectiveFromDate.Text);
                if (TxtFeeAmount.Text.Contains("."))
                    TxtFeeAmount.Text = TxtFeeAmount.Text.Remove(TxtFeeAmount.Text.LastIndexOf('.'), 3);
                int FeeAmt = Convert.ToInt32(TxtFeeAmount.Text);
                var fee = context.FeeDetails.Where(c => c.CourseCategoryID == CourseCatId &&
                    c.CourseID == CourseId &&
                    c.FeeTypeID == FeeType && c.EffectiveFromDate == Inputdate && c.FeeAmount == FeeAmt).FirstOrDefault();
                if (fee != null)
                    return true;
                else
                    return false;
            };

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected string Save()
    {
        try
        {
            if (!isDuplicate())
            {
                using (EConnectContext context = new EConnectContext())
                {

                    var fee = new EConnect.NIELIT.FeeDetail();

                    int CourseName =  Convert.ToInt32(ddlCourseNameE.SelectedValue);
                    int FeeType =  Convert.ToInt32(ddlFeeTypeE.SelectedValue);
                    //Update Record 

                    var tempID = (from c in context.FeeDetails
                                  where c.CourseID == CourseName &&
                                  c.FeeTypeID == FeeType && c.EffectiveToDate == null
                                  select c).FirstOrDefault();
				//Added 25-Nov-2019
				 if (tempID != null)
                    {
					//
                    if (tempID.ID != 0)
                    {
                        fee = context.FeeDetails.Single(reg => reg.ID == tempID.ID && reg.EffectiveToDate == null);
                        fee.EffectiveToDate = Convert.ToDateTime(txtEffectiveFromDate.Text).AddDays(-1);
                        context.SaveChanges();
                    }
					}

                    fee.CourseCategoryID = Convert.ToInt32(ddlCourseCategoryE.SelectedValue);
                    fee.CourseID = Convert.ToInt32(ddlCourseNameE.SelectedValue);
                    fee.FeeTypeID = Convert.ToInt32(ddlFeeTypeE.SelectedValue);
                    if (TxtFeeAmount.Text.Contains("."))
                        TxtFeeAmount.Text = TxtFeeAmount.Text.Remove(TxtFeeAmount.Text.LastIndexOf('.'), 3);
                    fee.FeeAmount = Convert.ToInt32(TxtFeeAmount.Text);
                    fee.EffectiveToDate = null;
                    fee.EffectiveFromDate = Convert.ToDateTime(txtEffectiveFromDate.Text);
                    context.FeeDetails.Add(fee);
                    context.SaveChanges();
                    strMessage = "New record saved.";

                };
            }
            else
                strMessage = "Record Already Exist.";
            return strMessage;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //create and object 

                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    if (!isDuplicate())
                    {
                        strMessage = Save();
                    }
                    else
                        LblError.Text = "Record Already Exist.";
                }
                else
                {
                    DateTime Inputdate = Convert.ToDateTime(txtEffectiveFromDate.Text);
                    int courseCatid = Convert.ToInt32(ddlCourseCategoryE.SelectedValue);
                    int courseid = Convert.ToInt32(ddlCourseNameE.SelectedValue);
                    int Feetypeid = Convert.ToInt32(ddlFeeTypeE.SelectedValue);
                    var fee1 = (from s in context.FeeDetails
                                where s.CourseID == courseid && s.CourseCategoryID == courseCatid
                                && s.FeeTypeID == Feetypeid && s.EffectiveFromDate == (from c in context.FeeDetails
                                                                                       where c.CourseID == s.CourseID && c.FeeTypeID == s.FeeTypeID
                                                                                       select c.EffectiveFromDate).Max()
                                select new
                                {
                                    Result = (
                                        s.EffectiveFromDate < Inputdate ? "Save" :
                                        s.EffectiveFromDate == Inputdate ? "Edit" :
                                        s.EffectiveFromDate > Inputdate ? "Invalid" : "Unknown"
                                        )
                                }).FirstOrDefault();
                    if (fee1.Result == "Save")
                    {
                        strMessage = Save();
                    }
                    else if (fee1.Result == "Edit")
                    {
                        //var fee = context.FeeDetails.Find(Convert.ToInt32(Request.QueryString["key"]));

                        //fee.CourseCategoryID = Convert.ToInt32(ddlCourseCategoryE.SelectedValue);
                        //fee.CourseID = Convert.ToInt32(ddlCourseNameE.SelectedValue);
                        //fee.FeeTypeID = Convert.ToInt32(ddlFeeTypeE.SelectedValue);
                        //if (TxtFeeAmount.Text.Contains("."))
                        //    TxtFeeAmount.Text = TxtFeeAmount.Text.Remove(TxtFeeAmount.Text.LastIndexOf('.'), 3);
                        //fee.FeeAmount = Convert.ToInt32(TxtFeeAmount.Text);
                        //fee.EffectiveFromDate = Convert.ToDateTime(txtEffectiveFromDate.Text);
                        //strMessage = "Record updated.";
                        //context.SaveChanges();.

                        strMessage = "Please Contact to Administrator.";

                    }
                    else if (fee1.Result == "Invalid")
                    {
                        strMessage = "Invalid";

                    }
                }
                if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseId"])))
                {
                    if (strMessage != "Invalid")
                        Response.Redirect("FeeDetail.aspx?msg=" + strMessage + "&CourseId=" + Convert.ToString(Request.QueryString["CourseId"]));
                    else
                        LblError.Text = "Invalid Effective from Date";
                }
                else
                {
                    if (strMessage != "Invalid")
                        Response.Redirect("FeeDetail.aspx?msg=" + strMessage);
                    else
                        LblError.Text = "Invalid Effective from Date";
                }

            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }
    protected void ApplyFilter(object sender, EventArgs e)
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
            if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["CourseId"])))
            {
                ddlFeeType.SelectedValue = "0";
            }
            else
            {
                ddlFeeType.SelectedValue = "0";
                ddlCourseCategory.SelectedValue = "0";
                ddlCourseName.SelectedValue = "0";
            }
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
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                {
                    hl.NavigateUrl += "&CourseId=" + Request.QueryString["CourseId"].ToString();
                }
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
        {
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("FeeDetail.aspx?CourseId=" + Request.QueryString["CourseId"].ToString()), true);
        }
        else
        {
            Response.Redirect("FeeDetail.aspx", true);
        }
    }
    protected void ddlCourseCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int CourseCatId = Convert.ToInt32(ddlCourseCategory.SelectedValue);
            bindCourse(CourseCatId, ddlCourseName, "--ALL--");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlCourseNameE_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int CourseId = Convert.ToInt32(ddlCourseNameE.SelectedValue);
            int CourseCatId = Convert.ToInt32(ddlCourseCategoryE.SelectedValue);
            ddlFeeTypeE.Items.Clear();
            ddlFeeTypeE.Items.Insert(0, "--Select One--");
            BindFeeType(ddlFeeTypeE, "--Select One--", true, CourseId);
            using (EConnectContext context = new EConnectContext())
            {
                var fee = context.FeeDetails
                       .Where(f => f.CourseID == CourseId && f.CourseCategoryID == CourseCatId)
                       .GroupBy(p => new { p.CourseID })
                       .Select(g => new { MaxEffectiveFromDate = g.Max(s => s.EffectiveFromDate) });
                txtEffectiveFromDate.Text = fee.FirstOrDefault().MaxEffectiveFromDate.ToString("dd-MMM-yyyy");
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ddlCourseCategoryE_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int courseCatid = Convert.ToInt32(ddlCourseCategoryE.SelectedValue);
            ddlFeeTypeE.Items.Clear();
            ddlFeeTypeE.Items.Insert(0, "--Select One--");
            ddlCourseNameE.Items.Clear();
            bindCourse(courseCatid, ddlCourseNameE, "--Select One--");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void GridViewOld_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar2.CurrentPageSize * PagingBar2.CurrentPageIndex)).ToString();
            Image imgAction = (Image)e.Row.FindControl("imgAction");
            imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();

            CheckBox chk = (CheckBox)e.Row.FindControl("chk");
            imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
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
                EConnect.NIELIT.FeeDetail feedetail = context.FeeDetails.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                context.FeeDetails.Remove(feedetail);
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
}