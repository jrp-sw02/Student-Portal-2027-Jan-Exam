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
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO.Compression;
//
using System.Net.Mail;
using System.Net;

public partial class Admin_NSQFFreeCourseMapping : BasePage
    {
    String strMessage = string.Empty, totalRecords = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 UserTypeId = 0;
    Int32 entityID = 0;
    #region other
    protected void Page_Load(object sender, EventArgs e)
        {
        //SentEmail(64);
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        //cbNielitCentres.Attributes.Add("onclick", "checkBoxList1OnCheck(this);");
        try
            {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            entityID = Convert.ToInt32(Session["EntityID"]);
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
                {
                Response.Write("Sorry! You don't have rights to view this page");
                Response.End();
                }

            if (!Page.IsPostBack)
                {
                User objUser;
                using (EConnectContext context = new EConnectContext())
                    {
                    objUser = new EConnect.URM.User();
                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    // RegionalCenter RegName = context.RegionalCenters.Find(loginUser.UserRefNumber);
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                        {
                        FillCourseCategory(1);
                        ddlcoursecategory.SelectedValue = "1";
                        ddlcoursecategory.Enabled = false;
                        FillCategoriesF(1);
                        FillCourse();
                        FillCourseFilter();
                        FillCourseBucket();
                        BindGridCoursemapping();
                        ShowEditMode();
                        //FillddlcentreName();

                        }
                    else
                        {
                        using (NIELITMISContext context1 = new NIELITMISContext())
                            {
                            ViewState["SortField"] = "";
                            ViewState["SortOrder"] = "";
                            FillCourseCategory(1);
                            ddlcoursecategory.SelectedValue = "1";
                            ddlcoursecategory.Enabled = false;
                            FillCategoriesF(1);
                            FillCourse();
                            FillCourseFilter();
                            FillCourseBucket();
                            BindGridCoursemapping();

                            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                                {
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Free NSQF Course Bucket", "Admin/NSQFFreeCourseMapping.aspx?Id=" + Request.QueryString["Id"].ToString(), ""));
                                }
                            else
                                {
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Free NSQF Course Bucket", "Admin/NSQFFreeCourseMapping.aspx", ""));
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

    protected void FillCourseCategory(int category)
        {
        try
            {
            ddlcoursecategory.Items.Clear();
            using (EConnectContext context = new EConnectContext())
                {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.CourseCategories
                               where p.IsActive == true
                               && p.ID == category
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


    protected void FillCategoriesF(int category)
        {
        try
            {
            ddlcoursecategory.Items.Clear();
            using (EConnectContext context = new EConnectContext())
                {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.CourseCategories
                               where p.IsActive == true
                                && p.ID == category
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategoryF, Category, lst);
                };
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }
    
    protected void FillCourse()
        {
        try
            {
            ddlcourseName.Items.Clear();
            using (EConnectContext context = new EConnectContext())
                {
                ListItem lst = new ListItem("--Select One--", "0");
                var course = from p in context.Courses
                             where p.IsActive == true
                              && p.CourseCategoryID == 1
                             orderby (p.DisplayOrder)
                             select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourseName, course, lst);
                };
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }

    protected void FillCourseFilter()
        {
        try
            {
            ddlCourseNameF.Items.Clear();
            using (EConnectContext context = new EConnectContext())
                {
                ListItem lst = new ListItem("--Select One--", "0");
                var course = from p in context.Courses
                             where p.IsActive == true
                              && p.CourseCategoryID == 1
                             orderby (p.DisplayOrder)
                             select new { ValueField = p.ID, TextField = p.Name + " ( " + p.Code + ")" };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseNameF, course, lst);
                };
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }
    protected void FillCourseBucket()
        {
        try
            {
            ddlCourseMapped.Items.Clear();
            using (EConnectContext context = new EConnectContext())
                {
                ListItem lst = new ListItem("--Select One--", "0");
                var course = from p in context.Courses
                             where p.IsActive == true
                              && p.CourseCategoryID == 6
                             orderby (p.Name)
                             select new { ValueField = p.ID, TextField = p.Name + "( " + p.Code + " )" };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseMapped, course, lst);
                };
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }

    protected void FillCourseReplaced()
        {
        try
            {
            ddlCourseReplaced.Items.Clear();
            using (EConnectContext context = new EConnectContext())
                {
                ListItem lst = new ListItem("--Select One--", "0");
                var course = from p in context.Courses
                             join s in context.NSQFFreeCourseMapping on p.ID equals s.mappedCourseID
                             where
                                 //p.IsActive == true
                                 // &&
                             p.CourseCategoryID == 6
                             orderby (p.Name)
                             select new { ValueField = p.ID, TextField = p.Name + "( " + p.Code + " )" };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseReplaced, course, lst);
                };
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }

    ////protected void FillddlcentreName()
    ////{
    ////    try
    ////    {
    ////        ListItem lst1 = new ListItem("--Select One--", "0");
    ////            using (NIELITMISContext context = new NIELITMISContext())
    ////            {                    
    ////                ddlCenter.ClearSelection();

    ////                var Center = from t in context.NielitCentres                                 
    ////                             orderby (t.Name)
    ////                             select new { ValueField = t.ID, TextField = t.Name };
    ////                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, Center, lst1);
    ////            }

    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        ShowAlert(ex.Message, true);
    ////    }
    ////}
    /*  protected void RowDataBound(object sender, GridViewRowEventArgs e)
      {
          if (e.Row.RowType == DataControlRowType.DataRow)
          {
              e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)");
              e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)");
          }
      }*/
    protected void BindGridCoursemapping()
        {
        try
            {
            lblError.Visible = false;
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string CourseName = "NA";
            string CCat = "NA";
            if (ddlcoursecategoryF.SelectedValue != "0")
                CCat = ddlcoursecategoryF.SelectedItem.Text;

            if (ddlCourseNameF.SelectedValue != "0")
                CourseName = ddlCourseNameF.SelectedItem.Text;

            DataTable dt = new DataTable();

            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            using (SqlCommand Cmm = new SqlCommand("getGridNSQFCourseMapping", con))
                {
                Cmm.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                Sda.Fill(dt);
                }
            con.Close();

            if (dt.Rows.Count > 0)
                {
                var NSQFCoursesMapped = (from p in dt.AsEnumerable()
                                         select new
                                         {
                                             Id = p.Field<Int64>("ID"),
                                             Name = p.Field<string>("Name"),
                                             CourseMapped = p.Field<string>("CourseMapped"),
                                             effectiveFrom = p.Field<DateTime>("effectiveFrom"),
                                             effectiveTo = p.Field<DateTime?>("effectiveTo"),
                                             mappingApprovalDate = p.Field<DateTime?>("mappingApprovalDate"),
                                             eFileNo = p.Field<string>("eFileNo"),
                                             //isReplacement = p.Field<Int64>("isReplacement"),
                                             isReplacement = p.Field<string>("isReplacement"),
                                             CourseReplaced = p.Field<string>("CourseReplaced"),
                                             Coursevalidity = p.Field<DateTime>("CourseValidity"),

                                         });


                if (CCat != "NA" && CourseName != "NA")
                    {
                    NSQFCoursesMapped = NSQFCoursesMapped.Where(s => s.Name == CourseName);
                    }

                PagingBar1.Bind(NSQFCoursesMapped, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                divNavigation.Visible = true;
                PagingBar1.Visible = true;
                lblError.Visible = false;
                gvMain.Visible = true;
                uPnlNavigation.Visible = true;
                if (gvMain.Rows.Count <= 0)
                    {
                    lblError.Text = "No record found.";
                    lblError.Visible = true;
                    gvMain.Visible = false;
                    //divNavigation.Visible = false;
                    }

                }
            else
                {
                lblError.Text = "No record found.";
                lblError.Visible = true;
                gvMain.Visible = false;
                uPnlNavigation.Visible = false;
                //divNavigation.Visible = false;
                }

            //gvMain.DataSource = dt;
            //gvMain.DataBind();
            //uPnlGrid.Update();
            //uPnlNavigation.Update();



            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        finally
            {
            //context.Dispose();
            }
        }

    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        try
            {
            if (e.Row.RowType == DataControlRowType.DataRow)
                {

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                HyperLink h1 = ((HyperLink)e.Row.Cells[3].Controls[0]);
                if (h1 != null)
                    if (h1.Text == "01-Jan-1900")
                        h1.Text = "-";
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

            using (EConnectContext context = new EConnectContext())
                {
                Int32 CourseMappedID = 0;
                CourseMappedID = Convert.ToInt32(Request.QueryString["Key"]);
                NSQFFreeCourseMapping editCourse = context.NSQFFreeCourseMapping.Find(CourseMappedID);
                var courseCnt = from c in context.NSQFFreeCourseMapping
                                where c.effectiveTo == null
                                && c.ID == CourseMappedID
                                select c;
                if (courseCnt != null)
                    {
                    int cnt = courseCnt.Count();
                    if (cnt == 0)
                        {
                        btnMode.ViewMode = ToggleView.Mode.List;


                        ShowAlert("Record cannot be modified, Already effective To date is available");
                        //throw new Exception("Record cannot be modified, Already effective To date is available");

                        return;
                        }
                    }
                else
                    {
                    btnMode.ViewMode = ToggleView.Mode.List;
                    ShowAlert("Record cannot be modified, Already effective To date is available");
                    //throw new Exception("Record cannot be modified, Already effective To date is available");

                    return;
                    }

                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;

                Label10.Visible = true;
                txtEffectiveToDate.Visible = true;
                FillCourseCategory(1);
                ddlcoursecategory.SelectedValue = "1";
                ddlcoursecategory.Enabled = false;

                btnSave.Text = "Update";
                lblHeading.Text = "Course Level Durations";

                var course = (from p in context.NSQFFreeCourseMapping
                              where p.ID == CourseMappedID
                              select new
                              {
                                  ID = p.ID,
                                  //Name = c.Name,
                                  CourseCatID = 1,
                                  CourseID = p.CourseID,
                                  courseMapped = p.mappedCourseID,
                                  mappingApprovalDate = p.mappingApprovalDate,
                                  eFileNo = p.eFileNo,
                                  EffectiveFromDate = p.effectiveFrom,
                                  EffectivetoDate = p.effectiveTo,
                                  whetheReplacement = p.isReplacement,
                                  replacedCourse = p.replacedCourseID,

                              }).FirstOrDefault();


                if (course.mappingApprovalDate.ToString() != "")
                    {
                    txtMappingApprovalDate.Text = Convert.ToDateTime(course.mappingApprovalDate).ToString("dd-MMM-yyyy");
                    }

                txtEffectiveFromDate.Text = Convert.ToDateTime(course.EffectiveFromDate).ToString("dd-MMM-yyyy");
                txtEffectiveFromDate.Enabled = false;

                if (course.EffectivetoDate.ToString() != "")
                    {
                    txtEffectiveToDate.Text = Convert.ToDateTime(course.EffectivetoDate).ToString("dd-MMM-yyyy");
                    }
                ddlcoursecategory.SelectedValue = course.CourseCatID.ToString();


                Int32 coursecatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);

                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == coursecatID && p.ShowOnWeb == true && p.IsActive == true
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourseName, CourseList, lst);



                ddlcourseName.SelectedValue = course.CourseID.ToString();
                ddlcoursecategory.Enabled = false;
                ddlcourseName.Enabled = false;
                FillCourseBucket();
                ddlCourseMapped.SelectedValue = course.courseMapped.ToString();
                ddlCourseMapped.Enabled = false;
                txtEFileNo.Text = course.eFileNo.ToString();
                txtMappingApprovalDate.Text = course.mappingApprovalDate.ToString();

                ddlIsReplacement.Enabled = false;
                if (course.whetheReplacement == true)
                    {
                    ddlIsReplacement.SelectedValue = "1";
                    FillCourseReplaced();
                    lblCourseReplaced.Visible = true;
                    ddlCourseReplaced.Visible = true;
                    }
                else
                    {
                    ddlIsReplacement.SelectedValue = "0";
                    lblCourseReplaced.Visible = false;
                    ddlCourseReplaced.Visible = false;
                    }

                ddlCourseReplaced.SelectedValue = course.replacedCourse.ToString();
                ddlCourseReplaced.Enabled = false;

                //   txtEffectiveFromDate.Enabled = false;

                };
            //if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            //{
            //    btnSave.Visible = false;
            //}
            }
        catch (Exception ex)
            {
            BreadCrumb1.Render();
            throw ex;
            }
        }
    
    protected void btnback_Click(object sender, EventArgs e)
        {
        Response.Redirect("NSQFFreeCourseMapping.aspx", true);
        }

    protected void PageIndexChanged(Int32 NewPageIndex)
        {
        try
            {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridCoursemapping();
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
                ShowAlert("Sorry! You don't have rights to add new record.");
                return;
                }
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            FillCourseCategory(1);
            ddlcoursecategory.SelectedValue = "1";
            ddlcoursecategory.Enabled = false;

            //Change the heading text as required
            lblHeading.Text = "New NSQF Mapping Course";

            }
        else
            {
            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NSQFFreeCourseMapping.aspx?ID=" + Request.QueryString["ID"].ToString()), true);
                }
            else
                {
                Response.Redirect("NSQFFreeCourseMapping.aspx", true);
                }
            }

        }

    protected void SearchBar_ApplySearch(object sender, EventArgs e)
        {
        try
            {
            BreadCrumb1.Render();
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
            BreadCrumb1.Render();
            ddlCourseNameF.SelectedValue = "0";
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
            ddlcoursecategoryF.SelectedValue = "0";
            ddlCourseNameF.SelectedValue = "0";
            FillCourseCategory(1);
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
            BindGridCoursemapping();
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
            if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                {
                BindGridCoursemapping();
                uPnlGrid.Update();
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to delete the records.", true);
                return;
                }

            BindGridCoursemapping();
            uPnlGrid.Update();
            }
        catch (Exception ex)
            {
            // BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be deleted!", true);
            }
        }
    
    protected void btnCancel_Click(object sender, EventArgs e)
        {
        Response.Redirect("NSQFFreeCourseMapping.aspx", true);
        }
    protected void IsReplacement(object sender, EventArgs e)
        {
        if (ddlIsReplacement.SelectedValue.ToString() == "1")
            {
            lblCourseReplaced.Visible = true;
            ddlCourseReplaced.Visible = true;
            FillCourseReplaced();
            }
        else
            {
            lblCourseReplaced.Visible = false;
            ddlCourseReplaced.Visible = false;
            }
        }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
        {
        Int32 loginUserNo = 0, UserTypeId = 0;
        EConnectContext context = new EConnectContext();
        NIELITMISContext context1 = new NIELITMISContext();
        try
            {
            loginUserNo = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            UserTypeId = Convert.ToInt32(HttpContext.Current.Session["UserTypeId"]);
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();

            string searchString = prefixText.Trim().ToUpper();
            var courses = from c in context.Courses
                          join w in context.CourseCategories on c.CourseCategoryID equals w.ID
                          select new { Name = c.Name, CatName = w.Name };

            courses = courses.Distinct();

            if (!String.IsNullOrEmpty(searchString))
                {
                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString) || s.CatName.ToUpper().Contains(searchString));
                }
            courses = courses.OrderBy(s => s.Name);

            foreach (var course in courses)
                {
                items.Add(course.Name);
                }
            return items.ToArray();
            }
        catch (Exception ex)
            {
            throw ex;
            }
        finally { context.Dispose(); }
        }

    #endregion
    //vishal
    protected void btnSave_Click(object sender, EventArgs e)
        {
        try
            {
            BreadCrumb1.Render();
            using (EConnectContext context = new EConnectContext())
                {
                //create and object 
                NSQFFreeCourseMapping vNSQFFreeCoursemapping = new NSQFFreeCourseMapping(); ;

                Int32 courseId = 0, courseCatId = 0, bucketCourseID = 0;

                courseId = Convert.ToInt32(ddlcourseName.SelectedValue);
                bucketCourseID = Convert.ToInt32(ddlCourseMapped.SelectedValue);

                string courseName = ddlcourseName.SelectedItem.Text;

                if (ddlIsReplacement.SelectedItem.Text == "Select")
                    {
                    strMessage = "Please select whether replacement";
                    lblactiveerror.Text = "Please select IsReplacement";
                    ddlIsReplacement.Focus();
                    return;
                    }

                if (txtEffectiveFromDate.Text == "")
                    {
                    ShowAlert("Effective From date cannot be blank");
                    return;
                    }

                if (txtMappingApprovalDate.Text == "")
                    {
                    ShowAlert("Mapping Approval date cannot be blank");
                    return;
                    }

                if (txtEFileNo.Text == "")
                    {
                    ShowAlert("EFile No. cannot be blank");
                    return;
                    }

                if (Convert.ToDateTime(txtMappingApprovalDate.Text).Date > System.DateTime.Today)
                    {
                    ShowAlert("Mapping Approval date cannot be future date");
                    return;
                    }
                
                if (txtEffectiveToDate.Text != "")
                    {
                    if (Convert.ToDateTime(txtEffectiveFromDate.Text).Date > Convert.ToDateTime(txtEffectiveToDate.Text).Date)
                        {
                        ShowAlert("Effective From date cannot be more than effective to date");
                        return;
                        }
                    }

                if (ddlIsReplacement.SelectedValue.ToString() == "1")
                    {
                    if (ddlCourseReplaced.SelectedValue.ToString() == "0")
                        {
                        ShowAlert("Please select a course replaced");
                        return;
                        }
                    }

                if (Convert.ToDateTime(txtEffectiveFromDate.Text).Date < Convert.ToDateTime(txtMappingApprovalDate.Text).Date)
                    {
                    ShowAlert("Approval Date should be before Effective From date");
                    return;
                    }

                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                    #region Save
                    if ((txtEffectiveFromDate.Enabled && Convert.ToDateTime(txtEffectiveFromDate.Text).Date < System.DateTime.Today) || Convert.ToDateTime(txtEffectiveFromDate.Text).Date < Convert.ToDateTime(txtMappingApprovalDate.Text).Date)
                        {
                        ShowAlert("Effective From date cannot be old date or before approval date");
                        return;
                        }
                    //Check duplicate
                    courseId = Convert.ToInt32(ddlcourseName.SelectedValue);
                    int mappedCourseId = Convert.ToInt32(ddlCourseMapped.SelectedValue);
                    int countDuplicate = (from p in context.NSQFFreeCourseMapping
                                          where
                                              //p.CourseID == courseId
                                              //&&
                                          p.mappedCourseID == mappedCourseId
                                          && (p.effectiveTo == null || p.effectiveTo > DateTime.Today)
                                          orderby p.effectiveTo descending
                                          select p
                                            ).Count();
                    if (countDuplicate > 0)
                        {
                        ShowAlert("Course is already mapped to some course with effective to date null or currently effective");
                        return;
                        }

                    DateTime effectivefromDateS, effectiveToDateS;
                    effectivefromDateS = Convert.ToDateTime(txtEffectiveFromDate.Text);
                    effectiveToDateS = Convert.ToDateTime("01/01/1900");
                    if (txtEffectiveToDate.Text != "")
                        effectiveToDateS = Convert.ToDateTime(txtEffectiveToDate.Text);


                    int oldCount = (from p in context.NSQFFreeCourseMapping
                                    where p.mappedCourseID == mappedCourseId
                                    || p.replacedCourseID == mappedCourseId
                                    orderby p.effectiveTo descending
                                    select p
                                            ).Count();
                    if (oldCount > 0)
                        {
                        var effectiveDateOld = (from p in context.NSQFFreeCourseMapping
                                                where p.mappedCourseID == mappedCourseId
                                                || p.replacedCourseID == mappedCourseId
                                                orderby p.effectiveTo descending
                                                select p
                                                ).FirstOrDefault();

                        if (effectivefromDateS < effectiveDateOld.effectiveFrom || effectivefromDateS < effectiveDateOld.effectiveTo)
                            {
                            ShowAlert("Effective from Date invalid,Check earlier entered dates");
                            return;
                            }
                        }


                    if (ddlIsReplacement.SelectedValue.ToString() == "0")
                        {
                        #region IsRelpcement No
                        vNSQFFreeCoursemapping.CourseID = Convert.ToInt32(ddlcourseName.SelectedValue);//course  id                   
                        vNSQFFreeCoursemapping.mappedCourseID = Convert.ToInt32(ddlCourseMapped.SelectedValue);
                        vNSQFFreeCoursemapping.effectiveFrom = effectivefromDateS;
                        vNSQFFreeCoursemapping.mappingApprovalDate = Convert.ToDateTime(txtMappingApprovalDate.Text);
                        vNSQFFreeCoursemapping.eFileNo = txtEFileNo.Text;
                        if (txtEffectiveToDate.Text != "")
                            vNSQFFreeCoursemapping.effectiveTo = effectiveToDateS;
                        if (ddlIsReplacement.SelectedValue.ToString() == "0")
                            vNSQFFreeCoursemapping.isReplacement = false;
                        else
                            {
                            vNSQFFreeCoursemapping.isReplacement = true;
                            vNSQFFreeCoursemapping.replacedCourseID = Convert.ToInt32(ddlCourseReplaced.SelectedValue);
                            }
                        vNSQFFreeCoursemapping.enterDate = DateTime.Now;
                        vNSQFFreeCoursemapping.enterBy = Convert.ToInt32(Session["UserID"]);
                        context.NSQFFreeCourseMapping.Add(vNSQFFreeCoursemapping);
                        context.SaveChanges();
                        #endregion
                        }
                    else // vishal on 07-10-2022
                        {
                        #region #region IsRelpcement Yes
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(900)))
                            {
                            int totalRecordsUpdated = 0;
                            Int64 AccrCourseIdM = Convert.ToInt64(ddlcourseName.SelectedValue);
                            Int64 courseIdM = Convert.ToInt64(ddlCourseMapped.SelectedValue);
                            string eFileNoM = txtEFileNo.Text;
                            DateTime MappingApprovalDate = Convert.ToDateTime(txtMappingApprovalDate.Text);
                            string MappingApprovalDate1 = Convert.ToDateTime(txtMappingApprovalDate.Text).ToShortDateString();
                            string effectivefromDateS1 = effectivefromDateS.ToShortDateString();
                            string effectiveToDateS1 = effectiveToDateS.ToShortDateString();
                            Int64 courseReplacedM = Convert.ToInt32(ddlCourseReplaced.SelectedValue);
                            Int64 enterBy = Convert.ToInt32(Session["UserID"]);
                            DateTime enterDate = DateTime.Now;

                            if (txtEffectiveToDate.Text != "")
                                {
                                context.Database.ExecuteSqlCommand("insert into [NIELIT].[dbo].[NSQFfreeCourseMapping] (courseID, bucketCourseID, eFileNo, isReplacement, replacedCourseID, enterBy, effectiveFrom, effectiveTo, mappingApprovalDate ) " +
                                    " values (  " + AccrCourseIdM.ToString() + "," + courseIdM.ToString() + ", '" + eFileNoM.ToString() + "' ," + 1 + "," + courseReplacedM.ToString() + "," + enterBy.ToString() + ", '" + effectivefromDateS1.ToString() + "' , '" + effectiveToDateS1.ToString() + "' , '" + MappingApprovalDate1.ToString() + "' ) ");
                                context.SaveChanges();
                                }
                            else
                                {
                                context.Database.ExecuteSqlCommand("insert into [NIELIT].[dbo].[NSQFfreeCourseMapping] (courseID, bucketCourseID, eFileNo, isReplacement, replacedCourseID, enterBy, effectiveFrom,  mappingApprovalDate ) " +
                                    " values (  " + AccrCourseIdM.ToString() + "," + courseIdM.ToString() + ", '" + eFileNoM.ToString() + "' ," + 1 + "," + courseReplacedM.ToString() + "," + enterBy.ToString() + ", '" + effectivefromDateS1.ToString() + "' , '" + MappingApprovalDate1.ToString() + "' ) ");
                                context.SaveChanges();
                                }

                            var MappedCourseNewEntry = (from p in context.NSQFFreeCourseMapping
                                                        where p.CourseID == AccrCourseIdM
                                                         && p.mappedCourseID == courseIdM
                                                         && p.replacedCourseID == courseReplacedM
                                                         && p.isReplacement == true
                                                        select p
                                                    ).FirstOrDefault();
                            if (MappedCourseNewEntry != null)
                                {
                                var GrantCourseOld1 = (from m in context.NSQFFreeCourseMapping
                                                       join g in context.NSQFFreeCourseGrants on m.replacedCourseID equals g.mappedCourseID
                                                       join i in context.AccreditationDetails on g.mappedCourseID equals i.CourseID
                                                       where m.isReplacement == true && m.replacedCourseID == MappedCourseNewEntry.replacedCourseID && m.CourseID == MappedCourseNewEntry.CourseID && m.mappedCourseID == MappedCourseNewEntry.mappedCourseID
                                                       && g.isActive == true && g.mappedCourseID == m.replacedCourseID
                                                       && (i.AccreditationStatusID != 5 || i.AccreditationStatusID != 6 || i.AccreditationStatusID != 7 || i.AccreditationStatusID != 8)
                                                       && i.InstituteID == g.InstituteID && i.CourseID == g.mappedCourseID //&& i.EffectiveToDate != null || i.EffectiveToDate > DateTime.Now
                                                       select new
                                                       {
                                                           IdM = m.ID,
                                                           AccrCourseIdM = m.CourseID,
                                                           mappedCourseIdM = m.mappedCourseID,
                                                           replacedCourseIdM = m.replacedCourseID,
                                                           eFileNoM = m.eFileNo,
                                                           appDate = m.mappingApprovalDate,
                                                           IdG = g.ID,
                                                           instituteIdG = g.InstituteID,
                                                           mappedCourseIdG = g.mappedCourseID,
                                                           accrCourseIdG = g.accrCourseID,
                                                           remarks = g.Remarks,
                                                           IdInstD = i.ID
                                                       }).ToList();

                                if (GrantCourseOld1.Count() > 0)
                                    {
                                    Int64 IdGrant = 0, instituteIdGrant = 0, replacedCourseIdGrant = 0, accrCourseIDGrant = 0, AccrCourseIdM1 = 0, mappedCourseIdM1 = 0, idInstDetails=0;
                                    string efileM1 = "", appDateG ="", newRemarks="";
                                    for (int x = 0; x < GrantCourseOld1.Count(); x++)
                                        {
                                        AccrCourseIdM1 = GrantCourseOld1[x].AccrCourseIdM;
                                        mappedCourseIdM1 = GrantCourseOld1[x].mappedCourseIdM;
                                        efileM1 = GrantCourseOld1[x].eFileNoM;
                                        appDateG = Convert.ToString(GrantCourseOld1[x].appDate);
                                        IdGrant = GrantCourseOld1[x].IdG;
                                        instituteIdGrant = GrantCourseOld1[x].instituteIdG;
                                        replacedCourseIdGrant = GrantCourseOld1[x].mappedCourseIdG;
                                        accrCourseIDGrant = GrantCourseOld1[x].accrCourseIdG;
                                        newRemarks = GrantCourseOld1[x].remarks + " and";
                                        idInstDetails = GrantCourseOld1[x].IdInstD;


                                        var AccrEffectiveDateTo = (from p in context.AccreditationDetails
                                                                   where p.CourseCategoryID == 1 // O/A/B/C Level
                                                                   && p.CourseID == AccrCourseIdM1 && p.InstituteID == instituteIdGrant
                                                                   && (p.AccreditationStatusID != 5 || p.AccreditationStatusID != 6 || p.AccreditationStatusID != 7 || p.AccreditationStatusID != 8)
                                                                   orderby p.EffectiveToDate descending
                                                                   select new { EffToDate = p.EffectiveToDate }).FirstOrDefault();

                                        if (AccrEffectiveDateTo.EffToDate != null)
                                            {
                                            if (Convert.ToDateTime(AccrEffectiveDateTo.EffToDate) >= DateTime.Now)
                                                {
                                                var GrantCourseCountForInst = (from p in context.NSQFFreeCourseGrants
                                                                               where p.InstituteID == instituteIdGrant && p.isActive == true
                                                                               select p).ToList();

                                                //to Check the duplicate entery into grant table
                                                var duplicateGrant = (from m in context.NSQFFreeCourseGrants
                                                                      where m.InstituteID == instituteIdGrant && m.mappedCourseID == mappedCourseIdM1 && m.isActive == true
                                                                      select m).ToList();
                                                //if


                                                if (GrantCourseCountForInst.Count() <= 5 && duplicateGrant.Count() < 1 && newRemarks.Length < 420)
                                                    {
                                                    string remarked = "";
                                                    remarked = newRemarks + " disactivated on " + DateTime.Now.ToString() + " due to replacement";

                                                    //Update the NSQFFreeCourseGrants IsActive false and Remarks
                                                    context.Database.ExecuteSqlCommand("update NSQFFreeCourseGrant set isActive=0, Remarks= '" + remarked + "' " + " where ID= " + IdGrant);
                                                    context.SaveChanges();

                                                    // Update Intitute_Accreditation_Detail Accr status id Withdrawal
                                                    int accrStatus = Convert.ToInt32(enmAccreditationStatus.Withdrawal);
                                                    context.Database.ExecuteSqlCommand("update Intitute_Accreditation_Detail set Accreditation_Status_ID= " + accrStatus.ToString() + ", Withdrawl_Date = '" + DateTime.Now.ToString() + "' " + " where id = " + idInstDetails);
                                                    context.SaveChanges();

                                                    var maxCountInstitute5 = (from m in context.NSQFFreeCourseGrants
                                                                              where m.InstituteID == instituteIdGrant && m.isActive == true
                                                                              select m).ToList();

                                                    if (maxCountInstitute5.Count() < 5)     //To count the max number (5) of NSQF courses Grant for a Institute
                                                        {
                                                        context.Database.ExecuteSqlCommand("INSERT INTO [dbo].[NSQFFreeCourseGrant] (instituteID, accrCourseID, mappedCourseID, grantDate, isActive, eFileNo, approvalDate, enterBy, Remarks) values ( " +
                                                        instituteIdGrant.ToString() + "," + AccrCourseIdM1.ToString() + "," + mappedCourseIdM1.ToString() + ", '" + DateTime.Now.ToString() + "' ," + 1 + ", '" + efileM1.ToString() + "' , '" + appDateG + "' ," + enterBy.ToString() + " , '" + "NsqfFreeAccrCourseMappedAndReplaced" + "' ) ");
                                                        context.SaveChanges();

                                                        var NewGrantEntry = (from t in context.NSQFFreeCourseGrants
                                                                             where t.mappedCourseID == mappedCourseIdM1 && t.accrCourseID == AccrCourseIdM1
                                                                             && t.InstituteID == instituteIdGrant && t.isActive == true
                                                                             select t).FirstOrDefault();

                                                        if (NewGrantEntry != null)
                                                            {
                                                            string accrNo = GenerateNsqfFreeAccrNo(NewGrantEntry.InstituteID, NewGrantEntry.mappedCourseID);
                                                            if (accrNo == "") { ShowAlert("Not Generate Accr No."); return; }
                                                            string ToDate = EffectiveToDate(instituteIdGrant, mappedCourseIdM1, AccrCourseIdM1);
                                                            if (ToDate == "") { ShowAlert("To Date Not found"); return; }
                                                            int accrStatusProvisional = Convert.ToInt32(enmAccreditationStatus.Provisional);

                                                            context.Database.ExecuteSqlCommand("insert into Intitute_Accreditation_Detail ([Institute_ID],[Course_Category_ID],[Course_ID],[Accreditation_Status_ID],[Accreditation_Number],[Effective_From_Date],[Effective_To_Date]) values ( " +
                                                 NewGrantEntry.InstituteID.ToString() + "," + 6 + "," + NewGrantEntry.mappedCourseID.ToString() + "," + accrStatusProvisional.ToString() + ", '" + accrNo.ToString() + "' , '" + DateTime.Now.ToString() + "' , '" + ToDate.ToString() + "' ) ");
                                                            context.SaveChanges();                                                            
                                                            SentEmail(IdGrant, NewGrantEntry.ID, NewGrantEntry.InstituteID);
                                                            totalRecordsUpdated = totalRecordsUpdated + 1;  
                                                            }
                                                        }
                                                    else { ShowAlert("The Institute can be granted only 5 NSQF Free Courses."); return; }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                else { ShowAlert("Record not Found."); return; }
                                }
                            else { ShowAlert("Record not saved for NSQF Course Mapping."); return; }
                            scope.Complete();
                            totalRecords = Convert.ToString(totalRecordsUpdated);
                            };
                        #endregion
                        }

                    if (ddlIsReplacement.SelectedValue.ToString() == "0")
                        {
                        strMessage = "New Record Saved";
                        }
                    else
                        {
                        strMessage = "New " + totalRecords + " institute(s) accredited by replacement.";
                        }

                    #endregion
                    }

                else
                    {
                    #region Update
                    int NSQFFreeCourseID = Convert.ToInt32(Request.QueryString["Key"]);
                    using (TransactionScope scope = new TransactionScope())
                        {

                        // Int32 NielitCentresID = Convert.ToInt32(li.Value);
                        var courseUpdate = (from p in context.NSQFFreeCourseMapping
                                            where p.ID == NSQFFreeCourseID
                                            //&& p.centreID == NielitCentresID

                                            select p).FirstOrDefault();


                        //courseUpdate.centreID = Convert.ToInt32(ddlCenter.SelectedValue);
                        // courseUpdate.centreID = NielitCentresID;
                        if (txtEffectiveToDate.Text != "")
                            {
                            DateTime effectivefromDateS, effectiveToDateS;
                            effectivefromDateS = Convert.ToDateTime(txtEffectiveFromDate.Text);
                            effectiveToDateS = Convert.ToDateTime(txtEffectiveToDate.Text);
                            if (effectiveToDateS < effectivefromDateS)
                                {
                                ShowAlert("Date To cannot be less than date from");
                                return;
                                }


                            courseUpdate.effectiveTo = Convert.ToDateTime(txtEffectiveToDate.Text);

                            }
                        courseUpdate.mappingApprovalDate = Convert.ToDateTime(txtMappingApprovalDate.Text);
                        courseUpdate.eFileNo = txtEFileNo.Text;

                        // courseUpdate.enterDate = DateTime.Now;
                        // courseUpdate.enterBy = Convert.ToInt32(Session["UserID"]);
                        context.SaveChanges();

                        scope.Complete();
                        strMessage = "NSQF Course Mapping Updated";

                        }
                    #endregion
                    }


                Response.Redirect("NSQFFreeCourseMapping.aspx?msg=" + strMessage, true);
                }
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message, true);
            }
        }

    protected void SentEmail(Int64 GrantId_Old, Int64 GrantId_New, Int64 InstId)
        {
        using (EConnectContext context = new EConnectContext())
            {
            #region New Institute Details
            var InstDetail = (from p in context.NSQFFreeCourseGrants
                              join i in context.Institutes on p.InstituteID equals i.ID
                              join l in context.Locations on i.StateID equals l.ID
                              join c in context.Courses on p.mappedCourseID equals c.ID
                              join d in context.Courses on p.accrCourseID equals d.ID
                              join a in context.AccreditationDetails on i.ID equals a.InstituteID
                              where a.CourseID == p.accrCourseID && p.ID == GrantId_New && i.ID == InstId
                              select new
                              {
                                  ID = p.ID,
                                  instituteID = p.InstituteID,
                                  CourseID = p.mappedCourseID,
                                  InstituteName = i.Name,
                                  instAdd = i.AddressLine1 + (!string.IsNullOrEmpty(i.AddressLine2) ? ", " + i.AddressLine2 : ""),
                                  InstCityPin = i.CityName + " - " + i.PinCode,
                                  InstState = l.Name,
                                  CourseName = c.Name,
                                  Instemail = i.EmailAddress1,
                                  InstAccrLvl = d.Name,
                                  InstAccNumber = a.AccreditationNumber,
                              }).FirstOrDefault();


            var InstDetail1 = (from p in context.NSQFFreeCourseGrants
                               join i in context.Institutes on p.InstituteID equals i.ID
                               join l in context.Locations on i.StateID equals l.ID
                               join c in context.Courses on p.mappedCourseID equals c.ID
                               join d in context.Courses on p.accrCourseID equals d.ID
                               join a in context.AccreditationDetails on i.ID equals a.InstituteID
                               where a.CourseID == p.mappedCourseID && p.ID == GrantId_New && i.ID == InstId
                               select new
                               {
                                   //ID = p.ID,
                                   InstAccrNSQF = a.AccreditationNumber,
                                   EffToDateNew = a.EffectiveToDate
                               }).FirstOrDefault();
            #endregion

            #region Old Institute Details

            var InstDetail1_Old = (from p in context.NSQFFreeCourseGrants
                               join i in context.Institutes on p.InstituteID equals i.ID
                               join l in context.Locations on i.StateID equals l.ID
                               join c in context.Courses on p.mappedCourseID equals c.ID
                               join d in context.Courses on p.accrCourseID equals d.ID
                               join a in context.AccreditationDetails on i.ID equals a.InstituteID
                                   where a.CourseID == p.mappedCourseID && p.ID == GrantId_Old && i.ID == InstId
                               select new
                               {
                                   //ID = p.ID,
                                   CourseName = c.Name,
                                   InstAccrNSQF = a.AccreditationNumber,
                               }).FirstOrDefault();
            #endregion 


            //by vishal Email
            DateTime ToDateNew = Convert.ToDateTime(InstDetail1.EffToDateNew);
            string subject = "NIELIT NSQF Course Replacment - Reg.";
            String EmailMsg = "<pre>" + InstDetail.InstituteName + ",<br/>" + InstDetail.instAdd + ",<br/>" + InstDetail.InstCityPin + ",<br/>" + InstDetail.InstState + "<br/>" + "<br/> <br/>Sir/Madam, <br/><br/>This is in reference to your NIELIT NSQF Aligned course - <b>" + InstDetail1_Old.CourseName + " (Accr No - " + InstDetail1_Old.InstAccrNSQF + ")</b> replaced with New NSQF Course - <b>" + InstDetail.CourseName + " (Accr No - " + InstDetail1.InstAccrNSQF + ")</b> and it is granted till " + ToDateNew.ToString("dd-MMM-yyyy") + ". <br/><br/><br/>This is a Computer generated letter and does not need signature. <b><br/><br/><br/><br/> NIELIT HQ, New Delhi ";

            if (InstDetail.Instemail.Length > 0)
                {
                try
                    {
                    //tesing on outlook 
                    //Email(subject, EmailMsg, InstDetail.Instemail);
                    //
                    EConnect.NIELIT.Email mail = new Email(subject, EmailMsg, InstDetail.Instemail);
                    mail.Send();

                    }
                catch { ShowAlert("Something is wrong!! please contact administrtor!!"); }
                }
            };
        }
        
    #region EffectiveToDate, Email and Accr No Generation

    //private void Emailwithattachment(String subject, String body, String emailAddressTo, Attachment attachmentData)
   
    
    protected string EffectiveToDate(Int64 instituteID, Int64 mappedCourseID, Int64 accrCourseid)
        {
        string effectiveToDate = "";
        using (EConnectContext context = new EConnectContext())
            {
            var courseDurationLevel = (from d in context.CourseLevelDurationss
                                       where d.CourseID == mappedCourseID && d.Effective_To_Date != null
                                       select d).ToList();

            if (courseDurationLevel.Count() != 0)
                {
                var MaxEffToDate = (from m in context.CourseLevelDurationss
                                    where m.CourseID == mappedCourseID && m.Effective_To_Date != null
                                    orderby m.Effective_To_Date descending
                                    select new { maxToDate = m.Effective_To_Date }).FirstOrDefault();
                //var examCycleName1 = examCycleName.OrderByDescending(c => c.year);
                if (MaxEffToDate != null)
                    {
                    if (Convert.ToDateTime(MaxEffToDate.maxToDate) >= DateTime.Now)
                        {
                        var MaxEffToDate1 = (from m in context.CourseLevelDurationss
                                             where m.CourseID == mappedCourseID && m.Effective_To_Date != null
                                             orderby m.Effective_To_Date descending
                                             select new { maxToDate = m.Effective_To_Date }).FirstOrDefault();
                        if (MaxEffToDate1 != null)
                            {
                            string returnValue = Convert.ToString(MaxEffToDate1.maxToDate);
                            effectiveToDate = returnValue;
                            }
                        }
                    else
                        {

                        var MaxEffToDate2 = (from i in context.AccreditationDetails
                                             where i.InstituteID == instituteID && i.CourseID == accrCourseid
                                             && (i.AccreditationStatusID != 5 || i.AccreditationStatusID != 6 || i.AccreditationStatusID != 7 || i.AccreditationStatusID != 8)
                                             orderby i.EffectiveToDate descending
                                             select new { maxToDate = i.EffectiveToDate }).FirstOrDefault();
                        if (MaxEffToDate2 != null)
                            {
                            if (Convert.ToDateTime(MaxEffToDate2.maxToDate) >= DateTime.Now)
                                {
                                string returnValue = Convert.ToString(MaxEffToDate2.maxToDate);
                                effectiveToDate = returnValue;
                                }
                            }
                        }
                    }
                }
            else
                {
                //
                var MaxEffToDate2 = (from i in context.AccreditationDetails
                                     where i.InstituteID == instituteID && i.CourseID == accrCourseid
                                     && (i.AccreditationStatusID != 5 || i.AccreditationStatusID != 6 || i.AccreditationStatusID != 7 || i.AccreditationStatusID != 8)
                                     orderby i.EffectiveToDate descending
                                     select new { maxToDate = i.EffectiveToDate }).FirstOrDefault();
                if (MaxEffToDate2 != null)
                    {
                    if (Convert.ToDateTime(MaxEffToDate2.maxToDate) >= DateTime.Now)
                        {
                        string returnValue = Convert.ToString(MaxEffToDate2.maxToDate);
                        effectiveToDate = returnValue;
                        }
                    }
                //
                }

            return effectiveToDate;
            }
        }   

    protected string GenerateNsqfFreeAccrNo(Int64 pInstID, Int64 courseid)
        {

        using (EConnectContext context = new EConnectContext())
            {
            Institute ins;
            ins = context.Institutes.Find(pInstID);
            Int64 instId = ins.ID;
            Int64 AutoAccrID = 00001;
            string FinalAccNumber = "";
            string statecode = "AA";
            string newTP = "R";

            var InstituteIId2 = (from c in context.Institutes
                                 join s in context.Locations on c.StateID equals s.ID

                                 where c.ID == instId  //&& c.ID > 62100000 && c.ID.ToString().StartsWith("621")
                                 select new
                                 {
                                     ID = c.ID,
                                     StateCode = s.Code
                                 }).FirstOrDefault();

            if (InstituteIId2 != null)
                {
                statecode = InstituteIId2.StateCode.ToString();
                }

            var InstituteIId1 = (from c in context.Institutes
                                 join a in context.AccreditationDetails on c.ID equals a.InstituteID
                                 join s in context.Locations on c.StateID equals s.ID

                                 where a.CourseCategoryID == 6 && c.ID == instId
                                 select new
                                 {
                                     ID = c.ID,
                                     StateCode = s.Code
                                 }).FirstOrDefault();


            if (InstituteIId1 != null)
                {
                statecode = InstituteIId1.StateCode.ToString();
                }

            string accrStart = statecode + "R";
            //Case 1To check if already accrediated for any free course
            string FullAccr = "";
            var AccrNumber = (from c in context.AccreditationDetails
                              where c.CourseCategoryID == 6 && c.AccreditationNumber.StartsWith(accrStart) && c.InstituteID.ToString().Trim() == pInstID.ToString().Trim()
                              select new
                              {
                                  ID = c.ID,
                                  AccrNum = c.AccreditationNumber
                              }).ToList();

            if (AccrNumber.Count > 0)
                {
                var AccrNumber1 = AccrNumber.OrderByDescending(x => x.ID).First();// UPN-00001-109
                FullAccr = AccrNumber1.AccrNum.ToString();

                int firsthypen = FullAccr.IndexOf('-');
                int secondhypen = FullAccr.LastIndexOf('-');

                string InstituteIdSeries = FullAccr.Substring(firsthypen + 1, secondhypen - 4);
                string accr = "";
                for (int i = InstituteIdSeries.Length; i < 5; i++)
                    accr = accr + "0";
                accr = accr + InstituteIdSeries;

                FinalAccNumber = statecode + newTP + "-" + accr + "-" + courseid.ToString();
                }
            else
                {
                //Case 2 if any other institute for free course in that state
                var AccrNumberAuto = (from c in context.AccreditationDetails
                                      join i in context.Institutes on c.InstituteID equals i.ID
                                      join l in context.Locations on i.StateID equals l.ID
                                      where c.CourseCategoryID == 6 && c.AccreditationNumber.StartsWith(accrStart) && l.Code == statecode
                                      select new
                                      {
                                          ID = c.ID,
                                          AccrNum = c.AccreditationNumber
                                      }).ToList();
                if (AccrNumberAuto.Count > 0)
                    {
                    var AccrNumber1 = AccrNumberAuto.OrderByDescending(x => x.ID).First();// UPN-00001-109
                    FullAccr = AccrNumber1.AccrNum.ToString();

                    int firsthypen = FullAccr.IndexOf('-');
                    int secondhypen = FullAccr.LastIndexOf('-');

                    string InstituteIdSeries = FullAccr.Substring(firsthypen + 1, secondhypen - 4);
                    AutoAccrID = Convert.ToInt64(InstituteIdSeries) + 1;

                    string accr = "";
                    for (int i = AutoAccrID.ToString().Trim().Length; i < 5; i++)
                        accr = accr + "0";
                    accr = accr + AutoAccrID.ToString().Trim();

                    FinalAccNumber = statecode + newTP + "-" + accr + "-" + courseid.ToString();
                    }
                else
                    {
                    string accr = "";
                    for (int i = AutoAccrID.ToString().Trim().Length; i < 5; i++)
                        accr = accr + "0";
                    accr = accr + AutoAccrID.ToString().Trim();
                    // Case 3 if no institute for that state
                    FinalAccNumber = statecode + newTP + "-" + accr + "-" + courseid.ToString();
                    }
                }
            return FinalAccNumber;
            }
        }

    #endregion

    }