using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class Admin_CutoffDates : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
         if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
              //  Response.Write("Sorry! You don't have rights  to view this page");
              //  Response.End();
            }
            if (!Page.IsPostBack)
            {
                
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    Bindcourses();
                    BindExams();
                    PopulateDataListMode();
                    //ListItem lst = new ListItem("--Select One--", "0");
                    //EConnect.Utils.Common.EnumUtility.BindListObject(ref Ddlctype, typeof(enmApplicantType), lst);
                    ShowEditMode();
                }
                else
                {
                    Populatelistmode();
                    ListItem lst = new ListItem("--All--", "0");
                    EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlcanType, typeof(enmApplicantType), lst);
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    if ((!String.IsNullOrEmpty(Request.QueryString["ExamID"])) && (!String.IsNullOrEmpty(Request.QueryString["CourseID"])))
                    {
                        Bindlistcourses();
                        using (EConnectContext context = new EConnectContext())
                        {
                            Course currentcourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["CourseID"].ToString()));
                            ddlcategory.SelectedItem.Text = currentcourse.CourseCategory.Name.ToString();
                            ddlcategory.Enabled = false;
                            Exam currentexam = context.Exams.Find(Convert.ToInt32(Request.QueryString["ExamID"].ToString()));
                            ddlexname.SelectedItem.Text = currentexam.Name + "(" + currentexam.ExaminationCycle.Name + ")".ToString();
                            ddlexname.Enabled = false;
                            ddlexamyear.SelectedItem.Text = currentexam.ExamYear.ToString();
                            ddlexamyear.Enabled = false;
                        };
                        ddlcourse.SelectedValue = Request.QueryString["CourseID"].ToString();
                        ddlcourse.Enabled = false;
                        Bindcouresgridview();
                    }
                    else
                    {
                        if (gvMain.Rows.Count <= 0)
                        {
                            lblError.Text = "Please Select Filter Criteria to View Cut-Off Dates Records";
                            lblError.Visible = true;
                        }
                        BindGridView();
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
    protected void Bindcourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //Populating Courses
                ListItem lst = new ListItem("--Select One--", "0");
                var Course = from p in context.Courses
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(Ddlcname, Course, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void Bindlistcourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //Populating Courses
                ListItem lst = new ListItem("--Select One--", "0");
                var Course = from p in context.Courses
                             orderby (p.Name)
                             select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourse, Course, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindExams()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //Populating Courses
                ListItem lst = new ListItem("--Select One--", "0");
                var Course = from p in context.Exams
                             orderby (p.Name)
                             select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(Ddlexamname, Course, lst);
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
            Int32 CourseCatID = 0;
            Int32 courseID = 0;
            Int32 appTypeID = 0;
            Int32 examid = 0;
            if (ddlcategory.SelectedValue != "0")
                CourseCatID = Convert.ToInt32(ddlcategory.SelectedValue);
            if (ddlcanType.SelectedValue != "0")
                appTypeID = Convert.ToInt32(ddlcanType.SelectedValue);
            if (ddlcourse.SelectedValue != "0")
                courseID = Convert.ToInt32(ddlcourse.SelectedValue);
            if (ddlexname.SelectedValue != "0")
                examid = Convert.ToInt32(ddlexname.SelectedValue);
            context = new EConnectContext();
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Cut-Off Dates", "Admin/CutoffDates.aspx", ""));
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            if (courseID != 0 && examid != 0 && CourseCatID != 0)
            {
                var users = from s in context.CutOffDates
                            select new
                            {
                                ID = s.ID,
                                CourseName = (!string.IsNullOrEmpty(s.CourseCategory.Code) ? s.CourseCategory.Code : "All") + "-" + (!string.IsNullOrEmpty(s.Course.Code) ? s.Course.Code : "All"),
                                activity = s.Activity.Name,
                                Type = s.ApplicantType.Name,
                                Date = s.EfferctiveDate,
                                CourseID = "",
                                ExamID = "",
                                Exid= s.ExamID,
                                CCatid = s.CourseCategoryID,
                                Cid = s.CourseID,
                                ApptypeID = s.ApplicantTypeID
                            };
                if (CourseCatID != 0)
                    users = users.Where(s => s.CCatid == CourseCatID);
                if (courseID != 0)
                    users = users.Where(s => s.Cid == courseID);
                if (appTypeID != 0)
                    users = users.Where(s => s.ApptypeID == appTypeID);
                if (examid != 0)
                    users = users.Where(s => s.Exid == examid);
                if (!string.IsNullOrEmpty(searchString))
                {
                    users = users.Where((s => s.activity.ToUpper().Contains(searchString)));
                }
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ID":
                            if (sortOrder == "DESC")
                                users = users.OrderByDescending(s => s.ID);
                            else
                                users = users.OrderBy(s => s.ID);
                            break;
                        case "CourseName":
                            if (sortOrder == "DESC")
                                users = users.OrderByDescending(s => s.CourseName);
                            else
                                users = users.OrderBy(s => s.CourseName);
                            break;
                        case "activity":
                            if (sortOrder == "DESC")
                                users = users.OrderByDescending(s => s.activity);
                            else
                                users = users.OrderBy(s => s.activity);
                            break;
                        case "Type":
                            if (sortOrder == "DESC")
                                users = users.OrderByDescending(s => s.Type);
                            else
                                users = users.OrderBy(s => s.Type);
                            break;
                        case "Date":
                            if (sortOrder == "DESC")
                                users = users.OrderByDescending(s => s.Date);
                            else
                                users = users.OrderBy(s => s.Date);
                            break;
                        default:
                            users = users.OrderBy(s => s.ID);
                            break;
                    }
                }
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    users = users.Where(a => roleCourses.Contains(a.Cid));
                }
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    users = users.Where(a => roleCourses.Contains(a.CCatid));
                }
                PagingBar1.Bind(users, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
                if (gvMain.Rows.Count <= 0)
                {
                    lblError.Text = "No record found.";
                    lblError.Visible = true;
                }
                else
                {
                    lblError.Visible = false;
                }
            }
            else
            {
               
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
    protected void PopulateCourses(Int32 CourseCategoryID, DropDownList ddl, ListItem lst)
    {
        try
        {
            ddl.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == CourseCategoryID
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void PopulateDataListMode()
    {
        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                //Populating Course Category
               
                var Category = from p in context.CourseCategories
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    Category = Category.Where(a => roleCourses.Contains(a.ValueField));
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(Ddlccat, Category.Distinct(), lst);
               
            };
           
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void Populatelistmode()
    {
        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                //Populating Course Category

                var Category = from p in context.CourseCategories
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    Category = Category.Where(a => roleCourses.Contains(a.ValueField));
                }
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcategory, Category.Distinct(), lst);
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
            Int32 Id = Convert.ToInt32(Request.QueryString["Key"]);
            var users = (from p in context.CutOffDates
                         where p.ID == Id
                         select p).FirstOrDefault();
            Ddlccat.SelectedValue = users.CourseCategoryID.ToString();
            Ddlcname.SelectedValue = users.CourseID.ToString();
            Ddlexamname.SelectedValue = users.ExamID.ToString();
            txtdate.Text = Convert.ToDateTime(users.EfferctiveDate).ToString("dd-MMM-yyyy");
            Ddlexamname_SelectedIndexChanged(Ddlexamname, EventArgs.Empty);
            Ddlactivity.SelectedValue = users.ActivityID.ToString();
            Ddlactivity.Enabled = false;
            Ddlactivity_SelectedIndexChanged(Ddlactivity, EventArgs.Empty);
            Ddlctype.SelectedValue = users.ApplicantTypeID.ToString();
            Ddlctype.Enabled = false;
            btnMode.ViewMode = ToggleView.Mode.List;
            btnSave.Text = "Update";
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            lblHeading.Text = "Cut-Off Date Detail";
            Ddlccat.Enabled = false;
            Ddlcname.Enabled = false;
            Ddlexamname.Enabled = false;
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            if ((!String.IsNullOrEmpty(Request.QueryString["ExamID"])) && (!String.IsNullOrEmpty(Request.QueryString["CourseID"])))
            {
                
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Cut-Off Date Detail", "", ""));
            }
            else
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(users.Course.Name, "", ""));
            }
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
            context.Dispose();
        }

    }
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                BreadCrumb1.Render();
               // ShowAlert("Sorry! You don't have rights to add new record.", true);
              //  return;
            }
            PopulateDataListMode();
            ListItem lst = new ListItem("--Select One--", "0");
            if ((!String.IsNullOrEmpty(Request.QueryString["ExamID"])) && (!String.IsNullOrEmpty(Request.QueryString["CourseID"])))
            {
                Bindcourses();
                BindExams();
                using (EConnectContext context = new EConnectContext())
                {
                    Course currentcourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["CourseID"].ToString()));
                    Ddlccat.SelectedValue = currentcourse.CourseCategoryID.ToString();
                };
                Ddlcname.SelectedValue = Request.QueryString["CourseID"].ToString();
                Ddlexamname.SelectedValue = Request.QueryString["ExamID"].ToString();
                Ddlexamname_SelectedIndexChanged(Ddlexamname, EventArgs.Empty);
                Ddlccat.Enabled = false;
                Ddlcname.Enabled = false;
                Ddlexamname.Enabled = false;
            }
            else
            {
                Ddlccat.Enabled = true;
                Ddlcname.Enabled = true;
                Ddlexamname.Enabled = true;
            }
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            lblHeading.Text = "New Cut-Off Date";
            //Updating Breadscrumb
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Cut-Off Date", "#", ""));
        }
        else
        {
            if ((!String.IsNullOrEmpty(Request.QueryString["ExamID"])) && (!String.IsNullOrEmpty(Request.QueryString["CourseID"])))
            {
                Response.Redirect("CutoffDates.aspx?ExamID=" + Request.QueryString["ExamID"] + "&CourseID=" + Request.QueryString["CourseID"]);
            }
            else
            {
                Response.Redirect("CutoffDates.aspx");
            }
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
            context = new EConnectContext();
            CutOffDate cutDate;
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                
                cutDate = new EConnect.NIELIT.CutOffDate();
                cutDate.CourseCategoryID = Convert.ToInt32(Ddlccat.SelectedValue);
                cutDate.CourseID = Convert.ToInt32(Ddlcname.SelectedValue);
                cutDate.ApplicantTypeID = Convert.ToInt32(Ddlctype.SelectedValue);
                cutDate.ActivityID = Convert.ToInt32(Ddlactivity.SelectedValue);
                cutDate.ExamID = Convert.ToInt32(Ddlexamname.SelectedValue);
                cutDate.EfferctiveDate = Convert.ToDateTime(txtdate.Text);

                if (context.CutOffDates.Any(c => c.CourseID == cutDate.CourseID && c.CourseCategoryID == cutDate.CourseCategoryID &&
                    c.ApplicantTypeID == cutDate.ApplicantTypeID && c.ActivityID == cutDate.ActivityID && c.ExamID == cutDate.ExamID))
                {
                    throw new Exception("Duplicate record not allowed");
                }
                context.CutOffDates.Add(cutDate);
                context.SaveChanges();
                strMessage = "New record saved.";
            }
            else
            {
                cutDate = context.CutOffDates.Find(Convert.ToInt32(Request.QueryString["Key"]));
                cutDate.CourseCategoryID = Convert.ToInt32(Ddlccat.SelectedValue);
                cutDate.CourseID = Convert.ToInt32(Ddlcname.SelectedValue);
                cutDate.ApplicantTypeID = Convert.ToInt32(Ddlctype.SelectedValue);
                cutDate.ActivityID = Convert.ToInt32(Ddlactivity.SelectedValue);
                cutDate.ExamID = Convert.ToInt32(Ddlexamname.SelectedValue);
                cutDate.EfferctiveDate = Convert.ToDateTime(txtdate.Text);
                if (context.CutOffDates.Any(c => c.CourseID == cutDate.CourseID && c.CourseCategoryID == cutDate.CourseCategoryID &&
                    c.ApplicantTypeID == cutDate.ApplicantTypeID && c.ActivityID == cutDate.ActivityID && c.ID != cutDate.ID && c.ExamID == cutDate.ExamID))
                {
                    throw new Exception("Duplicate record not allowed");
                }
                context.SaveChanges();
                strMessage = "Record updated.";
            }
            if ((!String.IsNullOrEmpty(Request.QueryString["ExamID"])) && (!String.IsNullOrEmpty(Request.QueryString["CourseID"])))
            {
                Response.Redirect("CutoffDates.aspx?ExamID=" + Request.QueryString["ExamID"] + "&CourseID=" + Request.QueryString["CourseID"]);
            }
            else
            {
                Response.Redirect("CutoffDates.aspx");
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }

    }
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            if ((!String.IsNullOrEmpty(Request.QueryString["ExamID"])) && (!String.IsNullOrEmpty(Request.QueryString["CourseID"])))
            {
                PagingBar1.CurrentPageIndex = 0;
                gvMain.PageIndex = PagingBar1.CurrentPageIndex;
                Bindcouresgridview();
            }
            else
            {
                PagingBar1.CurrentPageIndex = 0;
                gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            }
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
            if ((!String.IsNullOrEmpty(Request.QueryString["ExamID"])) && (!String.IsNullOrEmpty(Request.QueryString["CourseID"])))
            {
                ddlcanType.SelectedValue = "0";
                PagingBar1.CurrentPageIndex = 0;
                gvMain.PageIndex = PagingBar1.CurrentPageIndex;
                lblError.Visible = false;
                Bindlistcourses();
                Bindcouresgridview();
            }
            else
            {
                ddlcategory.SelectedValue = "0";
                ddlcourse.SelectedValue = "0";
                ddlcanType.SelectedValue = "0";
                ddlexamyear.SelectedValue = "0";
                ddlexname.SelectedValue = "0";
                PagingBar1.CurrentPageIndex = 0;
                gvMain.PageIndex = PagingBar1.CurrentPageIndex;
                lblError.Visible = false;
                Response.Redirect("CutoffDates.aspx");
                //BindGridView();
            }
           
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
            if ((!String.IsNullOrEmpty(Request.QueryString["ExamID"])) && (!String.IsNullOrEmpty(Request.QueryString["CourseID"])))
            {
                Bindcouresgridview();
            }
            else
            {
                BindGridView();
            }
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
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink hl1 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl1.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl1.NavigateUrl);
                HyperLink hl2 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);
                HyperLink hl3 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);
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
            var users = from s in context.CutOffDates
                        select new { Name = s.Activity.Name};
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            users = users.OrderBy(s => s.Name).Distinct();
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
        if ((!String.IsNullOrEmpty(Request.QueryString["ExamID"])) && (!String.IsNullOrEmpty(Request.QueryString["CourseID"])))
        {
            Response.Redirect("CutoffDates.aspx?ExamID="+Request.QueryString["ExamID"]+"&CourseID=" + Request.QueryString["CourseID"]);
        }
        else
        {
            Response.Redirect("CutoffDates.aspx");
        }
        
    }
    protected void ddlcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateCourses(Convert.ToInt32(ddlcategory.SelectedValue), ddlcourse, new ListItem("--Select One--", "0"));
    }
    protected void Ddlccat_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateCourses(Convert.ToInt32(Ddlccat.SelectedValue), Ddlcname, new ListItem("--Select One--", "0"));
    }
    protected void Bindcouresgridview()
    {
        try
        {
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Cut-Off Dates", "Admin/CutoffDates.aspx?ExamID=" + Request.QueryString["ExamID"] + "&CourseID=" + Request.QueryString["CourseID"], ""));
            context = new EConnectContext();
            Int32 appTypeID = 0;
            if (ddlcanType.SelectedValue != "0")
                appTypeID = Convert.ToInt32(ddlcanType.SelectedValue);
            Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 examID = Convert.ToInt32(Request.QueryString["ExamID"]);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            var users = from s in context.CutOffDates
                        where s.ExamID == examID && s.CourseID == courseID
                        select new
                        {
                            ID = s.ID,
                            CourseName = (!string.IsNullOrEmpty(s.CourseCategory.Code) ? s.CourseCategory.Code : "All") + "-" + (!string.IsNullOrEmpty(s.Course.Code) ? s.Course.Code : "All"),
                            activity = s.Activity.Name,
                            Type = s.ApplicantType.Name,
                            Date = s.EfferctiveDate,
                            CourseID = courseID,
                            ExamID = examID,
                            ApptypeID = s.ApplicantTypeID,
                            CCatid = s.CourseCategoryID
                        };
            if (appTypeID != 0)
                users = users.Where(s => s.ApptypeID == appTypeID);
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where((s => s.activity.ToUpper().Contains(searchString)));
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "ID":
                        if (sortOrder == "DESC")
                            users = users.OrderByDescending(s => s.ID);
                        else
                            users = users.OrderBy(s => s.ID);
                        break;
                    case "CourseName":
                        if (sortOrder == "DESC")
                            users = users.OrderByDescending(s => s.CourseName);
                        else
                            users = users.OrderBy(s => s.CourseName);
                        break;
                    case "activity":
                        if (sortOrder == "DESC")
                            users = users.OrderByDescending(s => s.activity);
                        else
                            users = users.OrderBy(s => s.activity);
                        break;
                    case "Type":
                        if (sortOrder == "DESC")
                            users = users.OrderByDescending(s => s.Type);
                        else
                            users = users.OrderBy(s => s.Type);
                        break;
                    case "Date":
                        if (sortOrder == "DESC")
                            users = users.OrderByDescending(s => s.Date);
                        else
                            users = users.OrderBy(s => s.Date);
                        break;
                    default:
                        users = users.OrderBy(s => s.ID);
                        break;
                }
            }
            PagingBar1.Bind(users, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
            if (gvMain.Rows.Count <= 0)
            {
                lblError.Text = "No record found.";
                lblError.Visible = true;
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
    protected void PopulateExam(Int32 CourseID, DropDownList ddl, ListItem lst)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 daysofMonth = Convert.ToInt32(DateTime.Now.Month);
                Int32 currentyear = Convert.ToInt32(DateTime.Now.Year);
                Course currentCourse = context.Courses.Find(CourseID);
                if (currentCourse.enmCourseType == enmCourseType.CertificationExam)
                {
                    var examname = from p in context.Exams
                                   where p.CourseID == CourseID && p.ExamYear >= currentyear
                                   orderby p.ExaminationCycle.Name , p.ExamMonth
                                   select new { ValueField = p.ID, TextField = p.ExaminationCycle.Name + "-" + p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddl, examname, lst);
                }
                else if (currentCourse.enmCourseType == enmCourseType.CertificationCourse)
                {
                    var examcycle = from p in context.Exams
                                    where p.CourseID == CourseID && p.ExamYear >= currentyear
                                    orderby p.ExamMonth, p.Name
                                    select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddl, examcycle, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
                   // ShowAlert("Sorry! You don't have rights to delete the records.", true);
                    //return;
                }
                CutOffDate cutoffdates = context.CutOffDates.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                context.CutOffDates.Remove(cutoffdates);
                context.SaveChanges();
                if ((!String.IsNullOrEmpty(Request.QueryString["ExamID"])) && (!String.IsNullOrEmpty(Request.QueryString["CourseID"])))
                {
                    Response.Redirect("CutoffDates.aspx?ExamID=" + Request.QueryString["ExamID"] + "&CourseID=" + Request.QueryString["CourseID"]);
                }
                else
                {
                    Response.Redirect("CutoffDates.aspx");
                }
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
    protected void Ddlcname_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateExam(Convert.ToInt32(Ddlcname.SelectedValue), Ddlexamname, new ListItem("--Select One--", "0"));
    }
    protected void BindExamname(Int32 cid, Int32 examyear)
    {

        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var courses = (from s in context.Exams join c in context.CutOffDates
                                   on s.ID equals c.ExamID
                                   where s.CourseID == cid && s.ExamYear == examyear
                                   select new { ValueField = s.ID, TextField = s.Name + "(" + s.ExaminationCycle.Name + ")" }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlexname, courses, lst);
                }
                else
                {
                    ddlexname.Items.Insert(0, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void BindExamYear(Int32 cid)
    {

        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var examyear = (from s in context.Exams
                                    join c in context.CutOffDates
                                    on s.ID equals c.ExamID
                                    where s.CourseID == cid 
                                    orderby (s.ExamYear) descending
                                    select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlexamyear, examyear, lst);
                }
                else
                {
                    ddlexname.Items.Insert(0, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 cid = Convert.ToInt32(ddlcourse.SelectedValue);
            BindExamYear(cid);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlexamyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int32 courseid = Convert.ToInt32(ddlcourse.SelectedValue);
            Int32 examyear = Convert.ToInt32(ddlexamyear.SelectedValue);
            BindExamname(courseid, examyear);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void Ddlexamname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
            Int32 examID= Convert.ToInt32(Ddlexamname.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                var activity = (from c in context.CutOffDates
                                group c by new { c.ActivityID } into d
                                select new
                                {
                                    ActivityID = d.Key,
                                    count = d.Count(s => s.ExamID == examID)
                                }).Where(d => d.count >= 2).Select(d => d.ActivityID);


                EConnect.Utils.Common.EnumUtility.BindListObject(ref Ddlactivity, typeof(enmActivity), lst);
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    foreach (var activityID in activity.ToList())
                    {
                        foreach (ListItem item in Ddlactivity.Items)
                        {
                            if (item.Value == activityID.ActivityID.ToString())
                            {
                                Ddlactivity.Items.Remove(item);
                                break;
                            }
                        }
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void Ddlactivity_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ListItem lst = new ListItem("--Select One--", "0");
            Int32 examID = Convert.ToInt32(Ddlexamname.SelectedValue);
            Int32 ActivityID = Convert.ToInt32(Ddlactivity.SelectedValue);
            EConnect.Utils.Common.EnumUtility.BindListObject(ref Ddlctype, typeof(enmApplicantType), lst);
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                using (EConnectContext context = new EConnectContext())
                {
                    var activity = (from c in context.CutOffDates
                                    where c.ActivityID == ActivityID && c.ExamID == examID
                                    select c.ApplicantTypeID);

                    if (activity != null)
                    {
                        foreach (var applicant in activity.ToList())
                        {
                            foreach (ListItem item in Ddlctype.Items)
                            {
                                if (item.Value == applicant.ToString())
                                {
                                    Ddlctype.Items.Remove(item);
                                    break;
                                }
                            }
                        }
                    }
                };
            }  
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}