using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class HO_frmCourseRegistrationApplication : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

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
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                lblError.Visible = false;

                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    ucSearchBar.AutoCompleteContextKey = Request.QueryString["examID"] + "," + Request.QueryString["Key"] + "," + Request.QueryString["regtype"];
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillCourseCategory();
                    EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlregtype, typeof(enmRegistrationType), new ListItem("--Select One--", "0"));
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "Please Select Filter Criteria to View Application Records";
                        lblError.Visible = true;
                    }
                    if (Request.QueryString["categoryid"] != null || Request.QueryString["courseid"] != null || Request.QueryString["examyear"] != null || Request.QueryString["examID"] != null || Request.QueryString["regtype"] != null)
                    {
                        ddlFlCourseCategory.SelectedValue = Request.QueryString["categoryid"];
                        ddlFlCourseCategory_SelectedIndexChanged(ddlFlCourseCategory.SelectedValue, EventArgs.Empty);
                        ddlCourseName.SelectedValue = Request.QueryString["courseid"];
                        ddlCourseName_SelectedIndexChanged(ddlCourseName.SelectedValue, EventArgs.Empty);
                        ddlExamYear.SelectedValue = Request.QueryString["examyear"];
                        ddlExamYear_SelectedIndexChanged(ddlExamYear.SelectedValue, EventArgs.Empty);
                        ddlExamName.SelectedValue = Request.QueryString["examID"];
                        ddlregtype.SelectedValue = Request.QueryString["regtype"];
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Course Registration Applications:-" + ddlCourseName.SelectedItem.Text + "-" + "(" + ddlExamName.SelectedItem.Text + ")", "HO/frmCourseRegistrationApplication.aspx?courseid=" + ddlCourseName.SelectedValue + "&examID=" + ddlExamName.SelectedValue + "&examyear=" + ddlExamYear.SelectedValue + "&categoryid=" + ddlFlCourseCategory.SelectedValue + "&regtype=" + ddlregtype.SelectedValue, ""));
                        BreadCrumb1.Render();
                        BindGridView();
                    }
                    else
                    {
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Course Registration Applications", "HO/frmCourseRegistrationApplication.aspx", ""));
                        BreadCrumb1.Render();
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
    protected void FillCourseCategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                Int32 courseTypeID = Convert.ToInt32(enmCourseType.CertificationCourse);

                var CourseCategory = from p in context.CourseCategories
                                     join k in context.Courses on p.ID equals k.CourseCategoryID
                                     where k.CourseTypeID == courseTypeID
                                     orderby (p.Name)
                                     select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                    CourseCategory = CourseCategory.Where(a => roleCourses.Contains(a.ValueField));
                }

                CourseCategory = CourseCategory.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlFlCourseCategory, CourseCategory, lst);
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
            //btnMode.ViewMode = ToggleView.Mode.List;
            Int32 applicationstatusid = Convert.ToInt32(Request.QueryString["Key"]);
            Int32 examid = Convert.ToInt32(Request.QueryString["examID"]);
            Int32 categoryid = Convert.ToInt32(Request.QueryString["categoryid"]);
            Int32 courseid = Convert.ToInt32(Request.QueryString["courseid"]);
            Int32 examyear = Convert.ToInt32(Request.QueryString["examyear"]);
            Int32 regtype = Convert.ToInt32(Request.QueryString["regtype"]);
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = true;
            lblHeading.Text = "Course Registration Applications";
            //tblNavLinks.Visible = true;
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Course Registration Applications", "HO/frmCourseRegistrationApplication.aspx?key=" + Request.QueryString["key"] + "&examID=" + Request.QueryString["examID"] + "&examyear=" + Request.QueryString["examyear"] + "&categoryid=" + Request.QueryString["categoryid"] + "&courseid=" + Request.QueryString["courseid"] + "&regtype=" + Request.QueryString["regtype"] + "&paymentStatus=" + Request.QueryString["paymentStatus"], ""));
            //Create an object of record to be modified and assign properties to relevant fields.
            ViewState["SortOrder1"] = "";
            ViewState["SortField1"] = "";
            BindApplicationDetails();
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
            lblError.Visible = false;
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            Int32 categoryID = 0;
            Int32 courseID = 0;
            Int32 examYear = 0;
            Int32 examName = 0;
            Int32 regtype = 0;
            if (ddlFlCourseCategory.SelectedValue != "0")
            {
                categoryID = Convert.ToInt32(ddlFlCourseCategory.SelectedValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["categoryid"]))
                    categoryID = Convert.ToInt32(Request.QueryString["categoryid"]);
            }
            if (ddlCourseName.SelectedValue != "0")
            {
                courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["courseid"]))
                    courseID = Convert.ToInt32(Request.QueryString["courseid"]);
            }
            if (ddlExamYear.SelectedValue != "0")
            {
                examYear = Convert.ToInt32(ddlExamYear.SelectedValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["examyear"]))
                    examYear = Convert.ToInt32(Request.QueryString["examyear"]);
            }
            if (ddlExamName.SelectedValue != "0")
            {
                examName = Convert.ToInt32(ddlExamName.SelectedValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["examID"]))
                    examName = Convert.ToInt32(Request.QueryString["examID"]);
            }
            if (ddlregtype.SelectedValue != "0")
            {
                regtype = Convert.ToInt32(ddlregtype.SelectedValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["regtype"]))
                    regtype = Convert.ToInt32(Request.QueryString["regtype"]);
            }
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            if (categoryID != 0 && courseID != 0 && examYear != 0 && examName != 0 && regtype != 0)
            {
                int paymentIDPending = Convert.ToInt32(enmPaymentStatus.Pending);
                int applIDMarked = Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);
                var data = (from s in context.ApplicationStatuses
                            join p in context.CourseRegistrationApplications
                            on s.ID equals p.ApplicationStatusID
                            where (p.CourseID == courseID &&
                                   p.CourseCategoryID == categoryID &&
                                   p.ApplicableExam.ExamYear == examYear &&
                                   p.ApplicableExam.ID == examName &&
                                   p.RegistrationTypeID == regtype)
                            select new
                            {
                                Id = p.ApplicationStatusID,
                                examName = p.Course.Name + "-" + p.ApplicableExam.Name,
                                description = (p.PaymentStatusID == paymentIDPending && p.ApplicationStatusID == applIDMarked) ? "Fee Pending to be Paid by Institute" : s.Description,
                                examID = p.ApplicableExamID,
                                categoryid = p.CourseCategoryID,
                                courseid = p.CourseID,
                                examyear = p.ApplicableExam.ExamYear,
                                paymentStatusID = p.PaymentStatusID,
                                regtype = p.RegistrationTypeID
                            });


                var application = from p in data
                                  group p by new { p.description, p.Id } into c
                                  select new
                                  {
                                      Id = c.Key.Id,
                                      examName = c.FirstOrDefault().examName,
                                      description = c.Key.description,
                                      examID = c.FirstOrDefault().examID,
                                      NumberOfApp = c.Count(),
                                      categoryid = c.FirstOrDefault().categoryid,
                                      courseid = c.FirstOrDefault().courseid,
                                      examyear = c.FirstOrDefault().examyear,
                                      paymentStatus = c.FirstOrDefault().paymentStatusID,
                                      regtype = c.FirstOrDefault().regtype
                                  };

                //var application = (from s in context.ApplicationStatuses
                //                   join p in context.CourseRegistrationApplications
                //                   on s.ID equals p.ApplicationStatusID
                //                   where (p.CourseID == courseID &&
                //                          p.CourseCategoryID == categoryID &&
                //                          p.ApplicableExam.ExamYear == examYear &&
                //                          p.ApplicableExam.ID == examName && 
                //                          p.RegistrationTypeID == regtype )
                //                   group p by new { p.ApplicationStatusID, s.Name,p.RegistrationTypeID } into c
                //                   select new
                //                   {
                //                       Id = c.Key.ApplicationStatusID,
                //                       examName = c.FirstOrDefault().Course.Name + "-" + c.FirstOrDefault().ApplicableExam.Name,
                //                       description = c.Key.Name,
                //                       examID = c.FirstOrDefault().ApplicableExamID,
                //                       NumberOfApp = c.Count(),
                //                       categoryid = c.FirstOrDefault().CourseCategoryID,
                //                       courseid = c.FirstOrDefault().CourseID,
                //                       examyear = c.FirstOrDefault().ApplicableExam.ExamYear,
                //                       regtype= c.Key.RegistrationTypeID
                //                   });
                application = application.OrderBy(s => s.examName).ThenBy(s => s.description);
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "examName":
                            if (sortOrder == "DESC")
                                application = application.OrderByDescending(s => s.examName);
                            else
                                application = application.OrderBy(s => s.examName);
                            break;
                        case "description":
                            if (sortOrder == "DESC")
                                application = application.OrderByDescending(s => s.description);
                            else
                                application = application.OrderBy(s => s.description);
                            break;
                        case "NumberOfApp":
                            if (sortOrder == "DESC")
                                application = application.OrderByDescending(s => s.NumberOfApp);
                            else
                                application = application.OrderBy(s => s.NumberOfApp);
                            break;
                        default:
                            application = application.OrderBy(s => s.examName).ThenBy(s => s.description);
                            break;
                    }
                }
                PagingBar1.Visible = true;
                PagingBar1.Bind(application, ref gvMain);
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
                    lblError.Text = "";
                }
            }
            else
            {
                Response.Redirect("frmCourseRegistrationApplication.aspx", true);
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
            lblHeading.Text = "New User";
        }
        else
        {
            Response.Redirect("frmCourseRegistrationApplication.aspx", true);
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            PagingBar2.CurrentPageIndex = 0;
            gvdetail.PageIndex = PagingBar1.CurrentPageIndex;
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
            PagingBar2.CurrentPageIndex = 0;
            gvdetail.PageIndex = PagingBar1.CurrentPageIndex;
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
            context = new EConnectContext();
            //create and object 
            //User objUser;
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {

                strMessage = "New record saved.";
            }
            else
            {
                ////Initialize current object by loading it and get its current modified date
                strMessage = "Record updated.";
            }

            //Call save method
            //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
            //Redirect it to list mode
            //Response.Redirect("FrmApplicationReciept.aspx?msg=" + strMessage);
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
            if (ddlCourseName.SelectedValue != "0" || ddlExamName.SelectedValue != "0" || ddlExamYear.SelectedValue != "0" || ddlFlCourseCategory.SelectedValue != "0" || ddlregtype.SelectedValue != "0")
            {
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Course Registration Applications:-" + ddlCourseName.SelectedItem.Text + "-" + "(" + ddlExamName.SelectedItem.Text + ")", "HO/frmCourseRegistrationApplication.aspx?courseid=" + ddlCourseName.SelectedValue + "&examID=" + ddlExamName.SelectedValue + "&examyear=" + ddlExamYear.SelectedValue + "&categoryid=" + ddlFlCourseCategory.SelectedValue + "&regtype=" + ddlregtype.SelectedValue, ""));
                BreadCrumb1.Render();
                upBread.Update();
            }
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
            ddlFlCourseCategory.SelectedValue = "0";
            ddlCourseName.SelectedValue = "0";
            ddlExamYear.SelectedValue = "0";
            ddlExamName.SelectedValue = "0";
            ddlregtype.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            Response.Redirect("frmCourseRegistrationApplication.aspx");
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
        //try
        //{
        //    context = new EConnectContext();
        //    if (hfActionID.Value != "")
        //    {
        //        String recordID = hfActionID.Value.Split('$')[0].ToString();
        //        LinkButton btnAction = (LinkButton)sender;
        //        if (btnAction.CommandName == "Delete")
        //        {
        //            //Load the object and apply validateion if required
        //            //call delete function
        //            //bind the grid again
        //            BindGridView();
        //            ShowAlert("Record deleted successfully.", true);
        //            hfActionID.Value = "";
        //        }
        //        else if (btnAction.CommandName == "Action")
        //        {
        //            //Load the object and apply validateion if required
        //            //call function to perform required action
        //            //bind the grid again
        //            BindGridView();
        //            ShowAlert("Record Action1 successfully.", true);
        //            hfActionID.Value = "";
        //        }
        //        uPnlGrid.Update();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    hfActionID.Value = "";
        //    ShowAlert(ex.Message, true);
        //}
        //finally { context.Dispose(); }
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

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void gvdetail_RowDataBound(object sender, GridViewRowEventArgs e)
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

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar2.CurrentPageSize * PagingBar2.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void gvdetail_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField1"] = e.SortExpression;
            if (ViewState["SortOrder1"].ToString() == "DESC")
                ViewState["SortOrder1"] = "ASC";
            else
                ViewState["SortOrder1"] = "DESC";
            BindApplicationDetails();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void PageIndexChangedOld(Int32 NewPageIndex)
    {
        try
        {
            gvdetail.PageIndex = PagingBar2.CurrentPageIndex;
            BindApplicationDetails();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindApplicationDetails()
    {
        try
        {
            context = new EConnectContext();
            Int32 applicationstatusid = Convert.ToInt32(Request.QueryString["Key"]);
            Int32 paymentStatusId = Convert.ToInt32(Request.QueryString["paymentStatus"]);
            Int32[] paymentstatus = { Convert.ToInt32(enmPaymentStatus.Paid), Convert.ToInt32(enmPaymentStatus.PaidButNotVerified), Convert.ToInt32(enmPaymentStatus.Failed) };
            Int32 examid = Convert.ToInt32(Request.QueryString["examID"]);
            Int32 regtype = Convert.ToInt32(Request.QueryString["regtype"]);
            string sortOrder = ViewState["SortOrder1"].ToString();
            string sortField = ViewState["SortField1"].ToString();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            Int32 apptypeID = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
            var application = (from p in context.CourseRegistrationApplications
                               where p.ApplicationStatusID == applicationstatusid && p.ApplicableExamID == examid && p.RegistrationTypeID == regtype && p.FinalSubmitted == true
                               select new
                               {
                                   Id = p.ID,
                                   Appno = p.Number,
                                   Appdate = p.ApplicationDate,
                                   apptypeID = apptypeID,
                                   Name = p.Salutation + p.Name,
                                   fathername = !string.IsNullOrEmpty(p.FatherName) ? "Mr. " + p.FatherName : "NA",
                                   key = p.ApplicationStatusID,
                                   examID = p.ApplicableExamID,
                                   pStatusID = p.PaymentStatusID
                               });

            if ((applicationstatusid == Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT)) && (paymentStatusId == Convert.ToInt32(enmPaymentStatus.Pending)))
            {
                application = application.Where(s => s.pStatusID == paymentStatusId);
            }
            else if ((applicationstatusid == Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT)) && (paymentStatusId == Convert.ToInt32(enmPaymentStatus.Paid) || paymentStatusId == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified)))
            {
                application = application.Where(s => paymentstatus.Contains(s.pStatusID.Value));
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                application = application.Where(s => s.Appno.ToUpper().Contains(searchString)
                                       || s.Name.ToUpper().Contains(searchString));
            }
            if (!string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "Id":
                        if (sortOrder == "DESC")
                            application = application.OrderByDescending(s => s.Id);
                        else
                            application = application.OrderBy(s => s.Id);
                        break;
                    case "Appno":
                        if (sortOrder == "DESC")
                            application = application.OrderByDescending(s => s.Appno);
                        else
                            application = application.OrderBy(s => s.Appno);
                        break;
                    case "Name":
                        if (sortOrder == "DESC")
                            application = application.OrderByDescending(s => s.Name);
                        else
                            application = application.OrderBy(s => s.Name);
                        break;
                    case "fathername":
                        if (sortOrder == "DESC")
                            application = application.OrderByDescending(s => s.fathername);
                        else
                            application = application.OrderBy(s => s.fathername);
                        break;
                    case "Appdate":
                        if (sortOrder == "DESC")
                            application = application.OrderByDescending(s => s.Appdate);
                        else
                            application = application.OrderBy(s => s.Appdate);
                        break;
                    default:
                        application = application.OrderBy(s => s.Appno);
                        break;
                }
            }
            PagingBar2.Bind(application, ref gvdetail);
            Updatepanellist.Update();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }



    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count, String contextKey)
    {
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            Int32 examid = 0;
            Int32 applicationstatusID = 0;
            Int32 regtype = 0;
            String[] keys = contextKey.Split(',');
            List<String> items = new List<String>();
            if (!String.IsNullOrEmpty(keys[0]))
                examid = Convert.ToInt32(keys[0]);
            if (!String.IsNullOrEmpty(keys[1]))
                applicationstatusID = Convert.ToInt32(keys[1]);
            if (!String.IsNullOrEmpty(keys[2]))
                regtype = Convert.ToInt32(keys[2]);
            string searchString = prefixText.Trim().ToUpper();
            var applications = from s in context.CourseRegistrationApplications
                               where s.ApplicableExamID == examid && s.ApplicationStatusID == applicationstatusID && s.RegistrationTypeID == regtype
                               select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                applications = applications.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            applications = applications.OrderBy(s => s.Name);

            var applications1 = from s in context.CourseRegistrationApplications
                                where s.ApplicableExamID == examid && s.ApplicationStatusID == applicationstatusID && s.RegistrationTypeID == regtype
                                select new { Name = s.Number };
            if (!String.IsNullOrEmpty(searchString))
            {
                applications1 = applications1.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            applications = applications.Union(applications1).Take(count);
            foreach (var user in applications)
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
    protected void ddlFlCourseCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        int catID = Convert.ToInt32(ddlFlCourseCategory.SelectedValue);
        ddlCourseName.Items.Clear();
        FillCourseNames(catID);
        if (ddlFlCourseCategory.SelectedValue == "0")
        {
            ListItem lst = new ListItem("--Select One--", "0");
            ddlExamYear.Items.Clear();
            ddlExamYear.Items.Add(lst);
            ddlExamName.Items.Clear();
            ddlExamName.Items.Add(lst);
        }
    }
    protected void FillCourseNames(int catID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (catID != null || catID != 0)
                {
                    var CourseName = from p in context.Courses
                                     where p.CourseCategoryID == catID
                                     orderby (p.ID)
                                     select new { ValueField = p.ID, TextField = p.Name + " ( " + p.Code +")"};
                    if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
                    {
                        var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                        CourseName = CourseName.Where(a => roleCourses.Contains(a.ValueField));
                    }
                    CourseName = CourseName.Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, CourseName, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        int catID = Convert.ToInt32(ddlFlCourseCategory.SelectedValue);
        int couID = Convert.ToInt32(ddlCourseName.SelectedValue);
        ddlExamYear.Items.Clear();
        FillExamYears(catID, couID);
        ListItem lst = new ListItem("--Select One--", "0");
        if (ddlCourseName.SelectedValue == "0")
        {
            ddlExamYear.Items.Add(lst);
        }

        ddlExamName.Items.Clear();
        ddlExamName.Items.Add(lst);
    }
    protected void FillExamYears(int catID, int couID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (couID != 0 && catID != 0)
                {
                    //var examYear = (from p in context.Exams
                    //                join q in context.CourseRegistrationApplications
                    //                on p.ID equals q.ApplicableExamID
                    //                where (p.CourseID == couID && p.CourseCategoryID == catID)
                    //                orderby (p.ExamYear) descending
                    //                select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();
                    var ExamYear = context.Exams.Where(E => E.CourseID == couID).Select(s => new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                    ExamYear = ExamYear.OrderByDescending(s => s.ValueField).Take(6);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, ExamYear, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 catID = Convert.ToInt32(ddlFlCourseCategory.SelectedValue);
        Int32 couID = Convert.ToInt32(ddlCourseName.SelectedValue);
        Int32 exmYear = Convert.ToInt32(ddlExamYear.SelectedValue);
        ddlExamName.Items.Clear();
        FillExamNames(catID, couID, exmYear);
        ListItem lst = new ListItem("--Select One--", "0");
        if (ddlExamYear.SelectedValue == "0")
        {
            ddlExamName.Items.Add(lst);
        }
    }
    protected void FillExamNames(Int32 catID, Int32 couID, Int32 exmYear)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (couID != 0 && catID != 0 && exmYear != 0)
                {
                    var examName = (from p in context.Exams
                                    join q in context.CourseRegistrationApplications
                                    on p.ID equals q.ApplicableExamID
                                    where (p.CourseID == couID && p.CourseCategoryID == catID && p.ExamYear == exmYear)
                                    orderby (p.Name)
                                    select new { ValueField = p.ID, TextField = p.Name }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, examName, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnCancel_Click1(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("frmCourseRegistrationApplication.aspx?categoryid=" + Request.QueryString["categoryid"] + "&courseid=" + Request.QueryString["courseid"] + "&examyear=" + Request.QueryString["examyear"] + "&examID=" + Request.QueryString["examID"] + "&regtype=" + Request.QueryString["regtype"]), true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}