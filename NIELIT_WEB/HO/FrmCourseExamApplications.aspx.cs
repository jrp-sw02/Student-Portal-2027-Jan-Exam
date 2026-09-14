using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class FrmCourseExamApplications : BasePage
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
                    ucSearchBar.AutoCompleteContextKey = Request.QueryString["examID"] + "," + Request.QueryString["Key"];
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";

                    FillCourseCategory();
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "Please Select Filter Criteria to View Application Records";
                        lblError.Visible = true;
                    }
                    if (Request.QueryString["categoryid"] != null || Request.QueryString["courseid"] != null || Request.QueryString["examyear"] != null || Request.QueryString["examID"] != null)
                    {
                        ddlFlCourseCategory.SelectedValue = Request.QueryString["categoryid"];
                        ddlFlCourseCategory_SelectedIndexChanged(ddlFlCourseCategory.SelectedValue, EventArgs.Empty);
                        ddlCourseName.SelectedValue = Request.QueryString["courseid"];
                        ddlCourseName_SelectedIndexChanged(ddlCourseName.SelectedValue, EventArgs.Empty);
                        ddlExamYear.SelectedValue = Request.QueryString["examyear"];
                        ddlExamYear_SelectedIndexChanged(ddlExamYear.SelectedValue, EventArgs.Empty);
                        ddlExamName.SelectedValue = Request.QueryString["examID"];
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Course Exam Applications:-" + ddlCourseName.SelectedItem.Text + "-" + "(" + ddlExamName.SelectedItem.Text + ")", "HO/FrmCourseExamApplications.aspx?courseid=" + ddlCourseName.SelectedValue + "&examID=" + ddlExamName.SelectedValue + "&examyear=" + ddlExamYear.SelectedValue + "&categoryid=" + ddlFlCourseCategory.SelectedValue, ""));
                        BreadCrumb1.Render();
                        BindGridView();
                    }
                    else
                    {
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Course Exam Applications", "HO/FrmCourseExamApplications.aspx", ""));
                        BreadCrumb1.Render();
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
    protected void FillCourseCategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 courseTypeID = Convert.ToInt32(enmCourseType.CertificationCourse);

                ListItem lst = new ListItem("--Select One--", "0");
                var CourseCategory = from p in context.CourseCategories
                                     join c in context.Courses on p.ID equals c.CourseCategoryID
                                     where c.CourseTypeID == courseTypeID
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
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = true;
            lblHeading.Text = "Course Exam Applications";
            //tblNavLinks.Visible = true;
            //Get last modified date of current record and save it in ViewState object.
            ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Course Exam Applications", "HO/FrmCourseExamApplications.aspx?key=" + Request.QueryString["key"] + "&examID=" + Request.QueryString["examID"] + "&examyear=" + Request.QueryString["examyear"] + "&categoryid=" + Request.QueryString["categoryid"] + "&courseid=" + Request.QueryString["courseid"] + "&paymentStatus=" + Request.QueryString["paymentStatus"], ""));
            BreadCrumb1.Render();
            upBread.Update();
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
            tbdata.Visible = false;
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            Int32 categoryID = 0;
            Int32 courseID = 0;
            Int32 examYear = 0;
            Int32 examName = 0;

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

            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            if (categoryID != 0 && courseID != 0 && examYear != 0 && examName != 0)
            {

                int paymentIDPending = Convert.ToInt32(enmPaymentStatus.Pending);
                int applIDMarked = Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);
                var data = (from s in context.ApplicationStatuses
                            join p in context.CourseExamApplications
                            on s.ID equals p.ApplicationStatusID
                            where (p.CourseID == courseID &&
                                   p.CourseCategoryID == categoryID &&
                                   p.Exam.ExamYear == examYear &&
                                   p.Exam.ID == examName &&
                                   p.FinalSubmitted == true)
                            select new
                            {
                                Id = p.ApplicationStatusID,
                                examName = p.Course.Name + "-" + p.Exam.Name,
                                description = (p.PaymentStatusID == paymentIDPending && p.ApplicationStatusID == applIDMarked) ? "Fee Pending to be Paid by Institute" : s.Description,
                                examID = p.ExamID,
                                categoryid = p.CourseCategoryID,
                                courseid = p.CourseID,
                                examyear = p.Exam.ExamYear,
                                paymentStatusID = p.PaymentStatusID
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
                                      paymentStatus = c.FirstOrDefault().paymentStatusID
                                  };

                //var application = (from s in context.ApplicationStatuses
                //                   join p in context.CourseExamApplications
                //                   on s.ID equals p.ApplicationStatusID
                //                   where (p.CourseID == courseID &&
                //                          p.CourseCategoryID == categoryID &&
                //                          p.Exam.ExamYear == examYear &&
                //                          p.Exam.ID == examName && p.FinalSubmitted == true)
                //                   group p by new { p.ApplicationStatusID, s.Name } into c
                //                   select new
                //                   {
                //                       Id = c.Key.ApplicationStatusID,
                //                       examName = c.FirstOrDefault().Course.Name + "-" + c.FirstOrDefault().Exam.Name,
                //                       description = c.Key.Name,
                //                       examID = c.FirstOrDefault().ExamID,
                //                       NumberOfApp = c.Count(),
                //                       categoryid = c.FirstOrDefault().CourseCategoryID,
                //                       courseid = c.FirstOrDefault().CourseID,
                //                       examyear = c.FirstOrDefault().Exam.ExamYear
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
                //uPnlGrid.Update();
                //uPnlNavigation.Update();
                if (gvMain.Rows.Count <= 0)
                {
                    lblError.Text = "No record found.";
                    lblError.Visible = true;
                }
                else
                {
                    lblError.Visible = false;
                    lblError.Text = "";
                    tbdata.Visible = true;
                    Int32 AppRecieved = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);
                    var applCount = (from s in context.CourseExamApplications
                                     where s.ExamID == examName && s.FinalSubmitted == true
                                     group s by new { s.ExamID } into c
                                     select new
                                     {
                                         ID = c.Key.ExamID,
                                         TotalApplications = c.Count(p => p.FinalSubmitted == true),
                                         ExportedApplications = c.Count(p => p.IsExported == true),
                                         PendingToExportApplications = c.Count(p => (p.IsExported == false || p.IsExported == null) && p.ApplicationStatusID == AppRecieved)
                                     }).FirstOrDefault();

                    if (applCount != null)
                    {
                        lbtot.Text = applCount.TotalApplications.ToString();
                        lbexported.Text = applCount.ExportedApplications.ToString();
                        lbnotexported.Text = applCount.PendingToExportApplications.ToString();
                        Lbpendrecieve.Text = (applCount.TotalApplications - (applCount.PendingToExportApplications + applCount.ExportedApplications)).ToString();
                        if (applCount.PendingToExportApplications == 0)
                        {
                            btnExport.Visible = false;
                            //btnexcel.Visible = true;
                        }
                        else
                        {
                            btnExport.Visible = true;
                            //btnexcel.Visible = false;
                        }
                    }
                }
            }
            else
            {
                Response.Redirect("FrmCourseExamApplications.aspx", true);
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
            Response.Redirect("FrmApplicationReciept.aspx", true);
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
            Response.Redirect("FrmApplicationReciept.aspx?msg=" + strMessage);
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
            if (ddlCourseName.SelectedValue != "0" || ddlExamName.SelectedValue != "0" || ddlExamYear.SelectedValue != "0" || ddlFlCourseCategory.SelectedValue != "0")
            {
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Course Exam Applications:-" + ddlCourseName.SelectedItem.Text + "-" + "(" + ddlExamName.SelectedItem.Text + ")", "HO/FrmCourseExamApplications.aspx?courseid=" + ddlCourseName.SelectedValue + "&examID=" + ddlExamName.SelectedValue + "&examyear=" + ddlExamYear.SelectedValue + "&categoryid=" + ddlFlCourseCategory.SelectedValue, ""));
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
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            Response.Redirect("FrmCourseExamApplications.aspx");
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
            string sortOrder = ViewState["SortOrder1"].ToString();
            string sortField = ViewState["SortField1"].ToString();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            Int32 apptypeID = Convert.ToInt32(enmApplicationType.CourseExamApplication);
            var application = (from p in context.CourseExamApplications
                               where p.ApplicationStatusID == applicationstatusid && p.ExamID == examid && p.FinalSubmitted == true
                               select new
                               {
                                   Id = p.ID,
                                   Appno = p.Number,
                                   Appdate = p.ApplicationDate,
                                   apptypeID = apptypeID,
                                   Name = p.Candidate.Salutation + p.Candidate.Name,
                                   fathername = !string.IsNullOrEmpty(p.Candidate.FatherName) ? "Mr. " + p.Candidate.FatherName : "NA",
                                   key = p.ApplicationStatusID,
                                   examID = p.ExamID,
                                   pStatusID = p.PaymentStatusID
                               });

            if ((applicationstatusid == Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT)) && (paymentStatusId == Convert.ToInt32(enmPaymentStatus.Pending)))
            {
                application = application.Where(s => s.pStatusID == paymentStatusId);
            }
            else if ((applicationstatusid == Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT)) && (paymentStatusId == Convert.ToInt32(enmPaymentStatus.Paid) || paymentStatusId == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified)))
            {
                application = application.Where(s => paymentstatus.Contains(s.pStatusID));
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
            //Updatepanellist.Update();
            // Application to be Received by NIELIT
            Int32 institutecase = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
            Int32 directcase = Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
            if (applicationstatusid == institutecase || applicationstatusid == directcase)
            {
                btnreceive.Visible = true;
                gvdetail.Columns[5].Visible = true;
            }
            else
            {
                btnreceive.Visible = false;
                gvdetail.Columns[5].Visible = false;
            }
            if (gvdetail.Rows.Count <= 0)
            {
                lbnorecord.Text = "No record Found.....";
                lbnorecord.Visible = true;
                btnreceive.Visible = false;
            }
            else
            {
                lbnorecord.Text = "";
                lbnorecord.Visible = false;
            }
            Int32 couID = Convert.ToInt32(Request.QueryString["courseid"]);
            btnPrint.Attributes.Add("Onclick", "window.open(' " + EConnect.Utils.Security.QuertStringModule.Encrypt("../HO/Rpt/ApplicationReceiptReport.aspx?StatusId=" + applicationstatusid + "&ExamId=" + examid + "&CourseId=" + couID + "&TypeId=3" + "&applicantTypeID=0" + "&InstituteID=0" + "&Gender=0" + "&Category=0" + "&Occupation=0" + "&CStateID=0" + "&ExamCentre1=0" + "&Exam1StateID=0" + "&ExamCentre2=0" + "&Exam2StateID=0" + "&paymentStatusId=" + paymentStatusId) + " ');");
            //btnPrint.CommandArgument="StatusId=" + applicationstatusid + "&ExamId=" + examid + "&CourseId=" + couID + "&TypeId=3" + "&applicantTypeID=0" + "&InstituteID=0" + "&Gender=0" + "&Category=0" + "&Occupation=0" + "&CStateID=0" + "&ExamCentre1=0" + "&Exam1StateID=0" + "&ExamCentre2=0" + "&Exam2StateID=0";
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
            String[] keys = contextKey.Split(',');
            List<String> items = new List<String>();
            if (!String.IsNullOrEmpty(keys[0]))
                examid = Convert.ToInt32(keys[0]);
            if (!String.IsNullOrEmpty(keys[1]))
                applicationstatusID = Convert.ToInt32(keys[1]);
            string searchString = prefixText.Trim().ToUpper();
            var applications = from s in context.CourseExamApplications
                               where s.ExamID == examid && s.ApplicationStatusID == applicationstatusID
                               select new { Name = s.Candidate.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                applications = applications.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            applications = applications.OrderBy(s => s.Name);

            var applications1 = from s in context.CourseExamApplications
                                where s.ExamID == examid && s.ApplicationStatusID == applicationstatusID
                                select new { Name = s.Number };
            if (!String.IsNullOrEmpty(searchString))
            {
                applications1 = applications1.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            applications = applications.Union(applications1).Take(count);
            foreach (var app in applications)
            {
                items.Add(app.Name);
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
                if (catID != 0)
                {
                    var CourseName = from p in context.Courses
                                     where p.CourseCategoryID == catID
                                     orderby (p.ID)
                                     select new { ValueField = p.ID, TextField = p.Name };
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
                    var examYear = (from p in context.Exams
                                    join q in context.CourseExamApplications
                                    on p.ID equals q.ExamID
                                    where (p.CourseID == couID && p.CourseCategoryID == catID)
                                    orderby (p.ExamYear) descending
                                    select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, examYear, lst);
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
                                    join q in context.CourseExamApplications
                                    on p.ID equals q.ExamID
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
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("FrmCourseExamApplications.aspx?categoryid=" + Request.QueryString["categoryid"] + "&courseid=" + Request.QueryString["courseid"] + "&examyear=" + Request.QueryString["examyear"] + "&examID=" + Request.QueryString["examID"]), true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnreceive_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int16 verifiedCount = 0;
                    Int64 CourseExamID = 0;
                    Int32 ApplicationReceivedByNIELIT = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);
                    for (int i = 0; i < gvdetail.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gvdetail.Rows[i].FindControl("chk");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                CourseExamID = Convert.ToInt64(gvdetail.DataKeys[i].Values[0]);
                                CourseExamApplication courseExam = context.CourseExamApplications.Find(CourseExamID);
                                if (courseExam != null)
                                {
                                    verifiedCount += 1;
                                    courseExam.ApplicationStatusID = ApplicationReceivedByNIELIT;
                                    context.Entry(courseExam).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                    scope.Complete();
                    ShowAlert(verifiedCount.ToString() + " applications have been received by NIELIT.", true);
                };
            };
            BindApplicationDetails();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string sql = "";
        string sqle = "";
        try
        {
            BreadCrumb1.Render();
            Int32 AppRecieved = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);
            Int32 examName = 0;
            Int32 counter = 0;
            Int32 ExportedBy = Convert.ToInt32(Session["UserID"]);
            List<Int64> AppList = new List<Int64>();
            if (ddlExamName.SelectedValue != "0")
            {
                examName = Convert.ToInt32(ddlExamName.SelectedValue);
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["examID"]))
                    examName = Convert.ToInt32(Request.QueryString["examID"]);
            }
            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            //con.Open();      
            //sql = "SELECT distinct top 2000 cast ( ( cast(e.Exam_Year as varchar(4) )  +'-'+  Right('00'+cast(e.Exam_Month as varchar(02)),2) +'-01' ) as date)  as exam_dt, 0 as batch ,0 as s_no, ce.ID as form_no, " +
            //       " ce.Registration_Number as Reg_no, upper(cd.Name) as name , upper(a.City_Name) as city, upper(l.Name) as state, a.Pin_Code as pin , " +
            //       " case ce.Applicant_Type_ID  when '1' then 'D' else 'I' end as app_type, " +
            //       " case when ce.Institute_ID  is null then '0' else id.Accreditation_Number end as accr_no, " +
            //       " case when ce.Institute_ID  is null then null else upper(i.Name) end as inst_nm," +
            //       " case when ce.Previous_Exam_ID is NULL  then 'N' else 'Y' end as pr_appr," +
            //       " NULL as pr_roll, upper(e1.Code) as cntr_cd1, upper(e2.Code) as cntr_cd2 , upper(e3.Code) as cntr_on1,  upper(e4.Code) as cntr_on2 , upper(e5.Code) as cntr_pr1,upper(e6.Code) as cntr_pr2 ,ce.Number_of_th_Modules as no_pap, " +
            //       " 'N' as syl_type,'N' as rej,ce.Application_Date as entry_dt,  CASE ce.Medium_of_Exam  when 1 then 'E' else 'H' end as m_o_exam , null as syl_type_a, " +
            //       " upper(c.Code) as lvl, case cd.Cast_Category_ID when '2' then '###' when '3' then '###' " +
            //       " else null end as sc_st,NULL as pra1,NULL as pra2,NULL as pra3, NULL as pra4, NULL as pra5," +
            //       " null as user_id,null as ROLL_NO,null as LOC_ALOC,null as CENTRE_CODE,null as CENTRE_NUMERIC, " +
            //       " null as VENUE_CODE,ce.Candidate_ID as candidate_code ,1 as multiple_form_sr_no , dbo.getStateCode(l.ID) as state_code , upper(a.Address1) as add1," +
            //       " upper(a.Address2) as add2, upper(a.Address3) as add3,'N' as ignore_flag , null as ignore_remark, REPLACE(CONVERT(VARCHAR(11), cd.Dob, 106), ' ', '-') AS dob ," +
            //       " ISNULL(d.Payment_Mode_ID, 6) as paymode,null as ddno1,null as dddt1,ce.Fee_Amount as ddamt1 ,null as ddno2 ,null as dddt2 , null as ddamt2 ,null as rtgsno  ,null as rtgsdt " +
            //       " , null as rtgsamt  , null as cashno , null as cashdt  ,null as cashamt ,null as filename,null as entry_date_rej ,null as user_id_rej,null as remarks_rej" +
            //       "  ,null as entry_date_edit,null as user_id_edit,null as entry_date_prac_edit ,null as user_id_prac_edit, CASE CE.Late_Fee_Imposed When 1 then 'Y' Else 'N' end as Is_Late_Fee " +
            //       " FROM  Course_Exam_Application ce left outer join Institute i on ce.Institute_ID = i.ID left outer join Intitute_Accreditation_Detail id  on ce.Course_ID = id.Course_ID and ce.Institute_ID = id.Institute_ID " +
            //       " left outer join Address a on ce.Candidate_ID = a.Candidate_ID left outer join Demand_Note d  on ce.Demand_Note_ID = d.ID, " +
            //       " Candidate cd ,Location l, Exam e , Exam_Center e1,Exam_Center e2 ,Online_Exam_Center e3, Online_Exam_Center e4,Prac_Exam_Center e5,Prac_Exam_Center e6,Course c " +
            //       " WHERE ce.Candidate_ID = cd.ID and ce.Exam_ID = e.ID and  ce.Exam_Center1_ID = e1.ID and ce.Exam_Center2_ID = e2.ID and ce.Online_Exam_Center1_ID = e3.ID and  ce.Online_Exam_Center2_ID = e4.ID and  ce.Prac_Exam_Center1_ID = e5.ID and  ce.Prac_Exam_Center2_ID = e6.ID " +
            //       " and  a.State_ID = l.ID and ce.Course_ID = c.ID and l.Location_Type_ID =2 AND ce.Application_Status_ID = " + AppRecieved + "  AND ce.Is_Exported = 'false'  AND ce.Exam_ID = " + examName + " " +
            //       " and  a.Address_Type_ID =1 and a.Effective_From_Date = ( select MAX(Effective_From_Date) from Address a where ce.Candidate_ID = a.Candidate_ID and a.Address_Type_ID =1) and " +
            //       " not exists (select * From e_fm where exam_dt =(cast ( ( cast(e.Exam_Year as varchar(4) )  +'-'+  Right('00'+cast(e.Exam_Month as varchar(02)),2) +'-01' ) as date)) and " +
            //       " reg_no =ce.Registration_Number and lvl in ('O','A','B','C' ))" +
            //       " order by ce.ID";
            sql = " SELECT distinct top 2000 cast ( ( cast(e.Exam_Year as varchar(4) )  +'-'+  Right('00'+cast(e.Exam_Month as varchar(02)),2) +'-01' ) as date)  as exam_dt, 0 as batch , 0 as s_no, ce.ID as form_no,  ce.Registration_Number as Reg_no, " +
                 " upper(cd.Name) as name , upper(a.City_Name) as city,  upper(l.Name) as state,  a.Pin_Code as pin , case ce.Applicant_Type_ID  when '1' then 'D' else 'I' end as app_type, " +
                       "case when ce.Institute_ID  is null then '0' else id.Accreditation_Number end as accr_no,  case when ce.Institute_ID  is null then null else upper(i.Name) end as inst_nm,"+
            " case when ce.Previous_Exam_ID is NULL  then 'N' else 'Y' end as pr_appr," +
            " NULL as pr_roll, upper(e1.code) as cntr_cd1, upper(e2.code) as cntr_cd2, upper(e3.Code) as cntr_on1,  upper(e4.Code) as cntr_on2 , upper(e5.Code) as cntr_pr1,upper(e6.Code) as cntr_pr2 , ce.Number_of_th_Modules as no_pap,  'N' as syl_type," +
            "'N' as rej,ce.Application_Date as entry_dt,  CASE ce.Medium_of_Exam  when 1 then 'E' else 'H' end as m_o_exam , null as syl_type_a,  upper(c.Code) as lvl, case cd.Cast_Category_ID when '2' then '###' when '3' then '###'  else null end as sc_st," +
            " NULL as pra1,NULL as pra2,NULL as pra3, NULL as pra4, NULL as pra5, null as user_id, null as ROLL_NO, null as LOC_ALOC, null as CENTRE_CODE, null as CENTRE_NUMERIC,   null as VENUE_CODE, ce.Candidate_ID as candidate_code , 1 as multiple_form_sr_no , " +
            " dbo.getStateCode(l.ID) as state_code ,  upper(a.Address1) as add1, upper(a.Address2) as add2, upper(a.Address3) as add3, 'N' as ignore_flag ,  null as ignore_remark,  REPLACE(CONVERT(VARCHAR(11), cd.Dob, 106), ' ', '-') AS dob , " +
             " ISNULL(d.Payment_Mode_ID, 6) as paymode, null as ddno1,null as dddt1, ce.Fee_Amount as ddamt1 , null as ddno2 , null as dddt2 , null as ddamt2 , null as rtgsno , null as rtgsdt  ,  null as rtgsamt  , null as cashno ," +
            " null as cashdt  ,null as cashamt ,null as filename, null as entry_date_rej , null as user_id_rej, null as remarks_rej  , null as entry_date_edit, null as user_id_edit, null as entry_date_prac_edit , null as user_id_prac_edit," +
            " CASE CE.Late_Fee_Imposed When 1 then 'Y' Else 'N' end as Is_Late_Fee" +
            " FROM  Course_Exam_Application ce  left outer join Institute i on ce.Institute_ID = i.ID" +
            " left outer join Intitute_Accreditation_Detail id  on ce.Course_ID = id.Course_ID and ce.Institute_ID = id.Institute_ID " +
            " left outer join Address a on ce.Candidate_ID = a.Candidate_ID " +
            " left outer join Demand_Note d  on ce.Demand_Note_ID = d.ID" +
             " left outer join Exam_Center e1 on ce.Exam_Center1_ID = e1.ID" +
           " left outer join Exam_Center e2  on ce.Exam_Center2_ID = e2.ID" +
            " left outer join Online_Exam_Center e3 on ce.Online_Exam_Center1_ID = e3.ID" +
            " left outer join Online_Exam_Center e4 on ce.Online_Exam_Center2_ID = e4.ID" +
            " left outer join Prac_Exam_Center e5 on ce.Prac_Exam_Center1_ID = e5.ID" +
            " left outer join Prac_Exam_Center e6 on ce.Prac_Exam_Center2_ID = e6.ID," +
            " Candidate cd ,Location l, Exam e ,Course c" +
            " WHERE ce.Candidate_ID = cd.ID and ce.Exam_ID = e.ID" +
           " and  a.State_ID = l.ID and ce.Course_ID = c.ID and l.Location_Type_ID =2 " +
           " AND ce.Application_Status_ID = " + AppRecieved + " AND ce.Is_Exported = 'false'  AND ce.Exam_ID = " + examName + " " +
           " and  a.Address_Type_ID =1 and a.Effective_From_Date = ( select MAX(Effective_From_Date) from Address a where ce.Candidate_ID = a.Candidate_ID and a.Address_Type_ID =1)  and " +
           " not exists (select * From e_fm where exam_dt =(cast ( ( cast(e.Exam_Year as varchar(4) )  +'-'+  Right('00'+cast(e.Exam_Month as varchar(02)),2) +'-01' ) as date)) and  reg_no =ce.Registration_Number and lvl in ('O','A','B','C' )) order by ce.ID";

            //sql = "SELECT distinct top 2000 DATEADD(month, DATEDIFF(month, 0,e.Exam_Start_Date), 0) as exam_dt, 0 as batch ,0 as s_no, ce.ID as form_no, " +
            //        " ce.Registration_Number as Reg_no, upper(cd.Name) as name , upper(a.City_Name) as city, upper(l.Name) as state, a.Pin_Code as pin , " +
            //        " case ce.Applicant_Type_ID  when '1' then 'D' else 'I' end as app_type, " +
            //        " case when ce.Institute_ID  is null then '0' else id.Accreditation_Number end as accr_no, " +
            //        " case when ce.Institute_ID  is null then null else upper(i.Name) end as inst_nm," +
            //        " case when ce.Previous_Exam_ID is NULL  then 'N' else 'Y' end as pr_appr," +
            //        " NULL as pr_roll, upper(e1.Code) as cntr_cd1, upper(e2.Code) as cntr_cd2 , ce.Number_of_th_Modules as no_pap, " +
            //        " 'N' as syl_type,'N' as rej,ce.Application_Date as entry_dt,  CASE ce.Medium_of_Exam  when 1 then 'E' else 'H' end as m_o_exam , null as syl_type_a, " +
            //        " upper(c.Code) as lvl, case cd.Cast_Category_ID when '2' then '###' when '3' then '###' " +
            //        " else null end as sc_st,NULL as pra1,NULL as pra2,NULL as pra3, NULL as pra4," +
            //        " null as user_id,null as ROLL_NO,null as LOC_ALOC,null as CENTRE_CODE,null as CENTRE_NUMERIC, " +
            //        " null as VENUE_CODE,ce.Candidate_ID as candidate_code ,1 as multiple_form_sr_no , dbo.getStateCode(l.ID) as state_code , upper(a.Address1) as add1," +
            //        " upper(a.Address2) as add2, upper(a.Address3) as add3,'N' as ignore_flag , null as ignore_remark, REPLACE(CONVERT(VARCHAR(11), cd.Dob, 106), ' ', '-') AS dob ," +
            //        " ISNULL(d.Payment_Mode_ID, 6) as paymode,null as ddno1,null as dddt1,ce.Fee_Amount as ddamt1 ,null as ddno2 ,null as dddt2 , null as ddamt2 ,null as rtgsno  ,null as rtgsdt " +
            //        " , null as rtgsamt  , null as cashno , null as cashdt  ,null as cashamt ,null as filename,null as entry_date_rej ,null as user_id_rej,null as remarks_rej" +
            //        "  ,null as entry_date_edit,null as user_id_edit,null as entry_date_prac_edit ,null as user_id_prac_edit, CASE CE.Late_Fee_Imposed When 1 then 'Y' Else 'N' end as Is_Late_Fee " +
            //        " FROM  Course_Exam_Application ce left outer join Institute i on ce.Institute_ID = i.ID left outer join Intitute_Accreditation_Detail id  on ce.Course_ID = id.Course_ID and ce.Institute_ID = id.Institute_ID " +
            //        " left outer join Address a on ce.Candidate_ID = a.Candidate_ID left outer join Demand_Note d  on ce.Demand_Note_ID = d.ID, " +
            //        " Candidate cd ,Location l, Exam e , Exam_Center e1,Exam_Center e2 ,Course c " +
            //        " WHERE ce.Candidate_ID = cd.ID and ce.Exam_ID = e.ID and  ce.Exam_Center1_ID = e1.ID and ce.Exam_Center2_ID = e2.ID " +
            //        " and  a.State_ID = l.ID and ce.Course_ID = c.ID and l.Location_Type_ID =2 AND ce.Application_Status_ID = " + AppRecieved + "  AND ce.Is_Exported = 'false'  AND ce.Exam_ID = " + examName + " " +
            //        " and  a.Address_Type_ID =1 and a.Effective_From_Date = ( select MAX(Effective_From_Date) from Address a where ce.Candidate_ID = a.Candidate_ID and a.Address_Type_ID =1) " +
            //        " order by ce.ID";

            //SqlDataReader dr = EConnect.Utils.Data.DbUtility.ExecuteReader(sql, con, null, CommandType.Text, true);
            DataTable dtTbl = new DataTable();
            dtTbl = EConnect.Utils.Data.DbUtility.GetDataTable(sql, con, null, CommandType.Text, false);

            if (dtTbl.Rows.Count > 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    for (int i = 0; i < dtTbl.Rows.Count; i++)
                    {
                        //sqle = "insert into e_fm ([exam_dt]"+
                        //           ",[batch],[s_no],[form_no],[reg_no],[name],[city],[state],[pin],[app_type],[accr_no],[inst_nm],[pr_appr],[pr_roll],[cntr_cd1],[cntr_cd2],[cntr_onl1],[cntr_onl2],[cntr_prac1],[cntr_prac2]"+
                        //           ",[no_pap],[syl_type],[rej],[entry_dt],[m_o_exam],[sy_type_a],[lvl],[sc_st],[pra1],[pra2],[pra3],[pra4],[pra5],[user_id],[ROLL_NO],[LOC_ALOC],[CENTRE_CODE],[CENTRE_NUMERIC]"+
                        //           ",[VENUE_CODE],[candidate_code],[multiple_form_sr_no],[state_code],[add1],[add2],[add3],[ignore_flag],[ignore_remark],[dob],[paymode],[ddno1],[dddt1],[ddamt1],[ddno2]"+
                        //           ",[dddt2],[ddamt2],[rtgsno],[rtgsdt],[rtgsamt],[cashno],[cashdt],[cashamt],[filename],[entry_date_rej],[user_id_rej],[remarks_rej],[entry_date_edit],[user_id_edit]"+
                        //           ",[entry_date_prac_edit],[user_id_prac_edit],[Is_Late_Fee]) values ("+ 
                        //       "'" + dtTbl.Rows[i]["exam_dt"].ToString() + "', " + dtTbl.Rows[i]["batch"].ToString() + ", " + dtTbl.Rows[i]["s_no"].ToString() + ", " + dtTbl.Rows[i]["form_no"].ToString() + ", " +
                        //       " " + dtTbl.Rows[i]["Reg_no"].ToString() + ", '" + dtTbl.Rows[i]["name"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["city"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["state"].ToString().Replace("'", "''") + "', " +
                        //       " " + dtTbl.Rows[i]["pin"].ToString() + ", '" + dtTbl.Rows[i]["app_type"].ToString() + "', '" + dtTbl.Rows[i]["accr_no"].ToString() + "', '" + dtTbl.Rows[i]["inst_nm"].ToString().Replace("'", "''") + "', " +
                        //       " '" + dtTbl.Rows[i]["pr_appr"].ToString() + "', null, '" + dtTbl.Rows[i]["cntr_cd1"].ToString() + "','" + dtTbl.Rows[i]["cntr_cd2"].ToString() + "',"+
                        //       " '" + dtTbl.Rows[i]["cntr_on1"].ToString().Trim() + "', '" + dtTbl.Rows[i]["cntr_on2"].ToString().Trim() + "', '" + dtTbl.Rows[i]["cntr_pr1"].ToString().Trim() + "', '" + dtTbl.Rows[i]["cntr_pr2"].ToString().Trim() + "', " +
                        //       " '" + dtTbl.Rows[i]["no_pap"].ToString() + "', '" + dtTbl.Rows[i]["syl_type"].ToString() + "', '" + dtTbl.Rows[i]["rej"].ToString() + "', '" + dtTbl.Rows[i]["entry_dt"].ToString() + "', " +
                        //       " '" + dtTbl.Rows[i]["m_o_exam"].ToString() + "', '" + dtTbl.Rows[i]["syl_type_a"].ToString() + "', '" + dtTbl.Rows[i]["lvl"].ToString() + "', '" + dtTbl.Rows[i]["sc_st"].ToString() + "', " +
                        //       " '" + dtTbl.Rows[i]["pra1"].ToString() + "', '" + dtTbl.Rows[i]["pra2"].ToString() + "', '" + dtTbl.Rows[i]["pra3"].ToString() + "', '" + dtTbl.Rows[i]["pra4"].ToString() + "','" + dtTbl.Rows[i]["pra5"].ToString() + "', " +
                        //       " '" + dtTbl.Rows[i]["user_id"].ToString() + "', null, '" + dtTbl.Rows[i]["LOC_ALOC"].ToString() + "', '" + dtTbl.Rows[i]["CENTRE_CODE"].ToString() + "', " +
                        //       " null, null, " + dtTbl.Rows[i]["candidate_code"].ToString() + ", " + dtTbl.Rows[i]["multiple_form_sr_no"].ToString() + ", " +
                        //       " '" + dtTbl.Rows[i]["state_code"].ToString() + "', '" + dtTbl.Rows[i]["add1"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["add2"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["add3"].ToString().Replace("'", "''") + "', " +
                        //       " '" + dtTbl.Rows[i]["ignore_flag"].ToString() + "', '" + dtTbl.Rows[i]["ignore_remark"].ToString() + "', '" + dtTbl.Rows[i]["dob"].ToString() + "', " + dtTbl.Rows[i]["paymode"].ToString() + ", " +
                        //       " null,null, " + dtTbl.Rows[i]["ddamt1"].ToString() + ", null, null,null,null, null, null, null, null, null, null, null, null, null, null, null, null, null, " +
                        //       " '" + dtTbl.Rows[i]["Is_Late_Fee"].ToString() + "')";

                        //December_2024
                        SqlParameter[] param1 = { new SqlParameter("@exam_dt",dtTbl.Rows[i]["exam_dt"].ToString()),
                                                    new SqlParameter("@batch", dtTbl.Rows[i]["batch"].ToString()),
                                                    new SqlParameter("@s_no", dtTbl.Rows[i]["s_no"].ToString()),
                                                    new SqlParameter("@form_no", dtTbl.Rows[i]["form_no"].ToString()),
                                                    new SqlParameter("@Reg_no", dtTbl.Rows[i]["Reg_no"].ToString()),
                                                    new SqlParameter("@name", dtTbl.Rows[i]["name"].ToString().Replace("'", "''")),
                                                    new SqlParameter("@city", dtTbl.Rows[i]["city"].ToString().Replace("'", "''")),
                                                    new SqlParameter("@state", dtTbl.Rows[i]["state"].ToString().Replace("'", "''")),
                                                    new SqlParameter("@pin", dtTbl.Rows[i]["pin"].ToString()),
                                                    new SqlParameter("@app_type", dtTbl.Rows[i]["app_type"].ToString()),
                                                    new SqlParameter("@accr_no", dtTbl.Rows[i]["accr_no"].ToString()),
                                                    new SqlParameter("@inst_nm", dtTbl.Rows[i]["inst_nm"].ToString().Replace("'", "''")),
                                                    new SqlParameter("@pr_appr", dtTbl.Rows[i]["pr_appr"].ToString()),
                                                    new SqlParameter("@cntr_cd1", dtTbl.Rows[i]["cntr_cd1"].ToString()),
                                                    new SqlParameter("@cntr_cd2", dtTbl.Rows[i]["cntr_cd2"].ToString()),
                                                    new SqlParameter("@cntr_on1", dtTbl.Rows[i]["cntr_on1"].ToString().Trim()),
                                                    new SqlParameter("@cntr_on2",dtTbl.Rows[i]["cntr_on2"].ToString().Trim()),
                                                    new SqlParameter("@cntr_pr1", dtTbl.Rows[i]["cntr_pr1"].ToString().Trim()),
                                                    new SqlParameter("@cntr_pr2", dtTbl.Rows[i]["cntr_pr2"].ToString().Trim()),
                                                    new SqlParameter("@no_pap", dtTbl.Rows[i]["no_pap"].ToString()),
                                                    new SqlParameter("@syl_type", dtTbl.Rows[i]["syl_type"].ToString()),
                                                    new SqlParameter("@rej", dtTbl.Rows[i]["rej"].ToString()),
                                                    new SqlParameter("@entry_dt", dtTbl.Rows[i]["entry_dt"].ToString()),
                                                    new SqlParameter("@m_o_exam", dtTbl.Rows[i]["m_o_exam"].ToString()),
                                                    new SqlParameter("@syl_type_a", dtTbl.Rows[i]["syl_type_a"].ToString() ),
                                                    new SqlParameter("@lvl", dtTbl.Rows[i]["lvl"].ToString()),
                                                    new SqlParameter("@sc_st",dtTbl.Rows[i]["sc_st"].ToString()),
                                                    new SqlParameter("@pra1", dtTbl.Rows[i]["pra1"].ToString()),
                                                    new SqlParameter("@pra2", dtTbl.Rows[i]["pra2"].ToString()),
                                                    new SqlParameter("@pra3", dtTbl.Rows[i]["pra3"].ToString()),
                                                    new SqlParameter("@pra4", dtTbl.Rows[i]["pra4"].ToString()),
                                                    new SqlParameter("@pra5", dtTbl.Rows[i]["pra5"].ToString()),
                                                    new SqlParameter("@user_id", dtTbl.Rows[i]["user_id"].ToString()),
                                                    new SqlParameter("@LOC_ALOC", dtTbl.Rows[i]["LOC_ALOC"].ToString()),
                                                    new SqlParameter("@CENTRE_CODE", dtTbl.Rows[i]["CENTRE_CODE"].ToString()),
                                                    new SqlParameter("@candidate_code", dtTbl.Rows[i]["candidate_code"].ToString()),
                                                    new SqlParameter("@multiple_form_sr_no", dtTbl.Rows[i]["multiple_form_sr_no"].ToString()),
                                                    new SqlParameter("@state_code", dtTbl.Rows[i]["state_code"].ToString()),
                                                    new SqlParameter("@add1", dtTbl.Rows[i]["add1"].ToString().Replace("'", "''")),
                                                    new SqlParameter("@add2", dtTbl.Rows[i]["add2"].ToString().Replace("'", "''")),
                                                    new SqlParameter("@add3", dtTbl.Rows[i]["add3"].ToString().Replace("'", "''")),
                                                    new SqlParameter("@ignore_flag", dtTbl.Rows[i]["ignore_flag"].ToString()),
                                                    new SqlParameter("@ignore_remark", dtTbl.Rows[i]["ignore_remark"].ToString()),
                                                    new SqlParameter("@dob", dtTbl.Rows[i]["dob"].ToString()),
                                                    new SqlParameter("@paymode", dtTbl.Rows[i]["paymode"].ToString()),
                                                    new SqlParameter("@ddamt1", dtTbl.Rows[i]["ddamt1"].ToString()),
                                                    new SqlParameter("@Is_Late_Fee", dtTbl.Rows[i]["Is_Late_Fee"].ToString())
                                                        };
                        sqle = "insert into e_fm ([exam_dt]" +
                                ",[batch],[s_no],[form_no],[reg_no],[name],[city],[state],[pin],[app_type],[accr_no],[inst_nm],[pr_appr],[pr_roll],[cntr_cd1],[cntr_cd2],[cntr_onl1],[cntr_onl2],[cntr_prac1],[cntr_prac2]" +
                                ",[no_pap],[syl_type],[rej],[entry_dt],[m_o_exam],[sy_type_a],[lvl],[sc_st],[pra1],[pra2],[pra3],[pra4],[pra5],[user_id],[ROLL_NO],[LOC_ALOC],[CENTRE_CODE],[CENTRE_NUMERIC]" +
                                ",[VENUE_CODE],[candidate_code],[multiple_form_sr_no],[state_code],[add1],[add2],[add3],[ignore_flag],[ignore_remark],[dob],[paymode],[ddno1],[dddt1],[ddamt1],[ddno2]" +
                                ",[dddt2],[ddamt2],[rtgsno],[rtgsdt],[rtgsamt],[cashno],[cashdt],[cashamt],[filename],[entry_date_rej],[user_id_rej],[remarks_rej],[entry_date_edit],[user_id_edit]" +
                                ",[entry_date_prac_edit],[user_id_prac_edit],[Is_Late_Fee]) values (" +
                            "@exam_dt,@batch, @s_no, @form_no , " +
                            " @Reg_no , @name, @city, @state , " +
                            " @pin , @app_type, @accr_no, @inst_nm, " +
                            " @pr_appr , null, @cntr_cd1,@cntr_cd2," +
                            " @cntr_on1, @cntr_on2, @cntr_pr1, @cntr_pr2, " +
                            " @no_pap, @syl_type, @rej, @entry_dt, " +
                            " @m_o_exam, @syl_type_a, @lvl, @sc_st, " +
                            " @pra1, @pra2, @pra3,@pra4,@pra5, " +
                            " @user_id, null, @LOC_ALOC, @CENTRE_CODE, " +
                            " null, null, @candidate_code , @multiple_form_sr_no , " +
                            " @state_code, @add1, @add2, @add3, " +
                            " @ignore_flag, @ignore_remark, @dob, @paymode, " +
                            " null,null, @ddamt1, null, null,null,null, null, null, null, null, null, null, null, null, null, null, null, null, null, " +
                            " @Is_Late_Fee)";



                        //EConnect.Utils.Data.DbUtility.ExecuteNonQuery(sqle, con, null, CommandType.Text, true);
                        // Insert into e_fm table
                        context.Database.ExecuteSqlCommand(sqle, param1);
                        Int64 applID = Convert.ToInt64(dtTbl.Rows[i]["form_no"].ToString());

                        AppList.Add(applID);
                        // update in course_Exam appplication
                        //context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Is_Exported = 'true', Exported_On = '" + DateTime.Now + "', Exported_By = '" + ExportedBy + "' where id = " + applID);
                        
                        var practicals = (from a in context.CourseExamApplicationDetails
                                          join m in context.Modules on a.ModuleID equals m.ID
                                          where m.ModuleTypeID == (int)enmModuleType.Practical && a.CourseExamApplicationID == applID
                                          orderby m.ID  select new { m.ID, m.ShortName }).ToList();

                        //December_2024
                        SqlParameter[] param2 = { new SqlParameter("@applID", applID) };
                        foreach (var module in practicals)
                        {
                            if (module.ShortName.EndsWith("1"))
                                //context.Database.ExecuteSqlCommand("Update e_fm set pra1 = 'Y' where form_no = " + applID);
                                //December_2024
                                context.Database.ExecuteSqlCommand("Update e_fm set pra1 = 'Y' where form_no = @applID " , param2);         
                            else if (module.ShortName.EndsWith("2"))
                                //context.Database.ExecuteSqlCommand("Update e_fm set pra2 = 'Y' where form_no = " + applID);
                                //December_2024
                                context.Database.ExecuteSqlCommand("Update e_fm set pra2 = 'Y' where form_no = @applID " , param2);         
                            else if (module.ShortName.EndsWith("3"))
                                //context.Database.ExecuteSqlCommand("Update e_fm set pra3 = 'Y' where form_no = " + applID);
                                //December_2024
                                context.Database.ExecuteSqlCommand("Update e_fm set pra3 = 'Y' where form_no = @applID " , param2);         
                            else if (module.ShortName.EndsWith("4"))
                                //context.Database.ExecuteSqlCommand("Update e_fm set pra4 = 'Y' where form_no = " + applID);
                                //December_2024
                                context.Database.ExecuteSqlCommand("Update e_fm set pra4 = 'Y' where form_no = @applID " , param2);         
                            else if (module.ShortName.EndsWith("5"))
                                //context.Database.ExecuteSqlCommand("Update e_fm set pra5 = 'Y' where form_no = " + applID);
                                //December_2024
                                context.Database.ExecuteSqlCommand("Update e_fm set pra5 = 'Y' where form_no = @applID " , param2);         
                            else
                                //context.Database.ExecuteSqlCommand("Update e_fm set pra1 = 'Y' where form_no = " + applID);
                                //December_2024
                                context.Database.ExecuteSqlCommand("Update e_fm set pra1 = 'Y' where form_no = @applID " , param2);         
                        }
                        context.SaveChanges();
                        counter = counter + 1;
                    }
                    AppList.TrimExcess();
                    IQueryable<CourseExamApplication> tempAppl = context.CourseExamApplications.Where(s => AppList.Contains(s.ID));
                    foreach (CourseExamApplication appl in tempAppl)
                    {
                        appl.IsExported = true;
                        appl.ExportedByID = ExportedBy;
                        appl.ExportedOn = DateTime.Now;
                    }
                    context.SaveChanges();
                };
            }         
            BindGridView();
            ShowAlert("Data Exported Successfully for " + counter + " applications. ");
        }
        catch (Exception ex)
        {          
            lblError.Text = ex.Message;
            lblError.Visible = true;
        }
    }
    //protected void btnexcel_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        BreadCrumb1.Render();
    //        Int32 examid = 0;
    //        Int32 courseid = 0;
    //        string lvl = "";
    //        string examdate = "";
    //        if (ddlExamName.SelectedValue != "0")
    //        {
    //            examid = Convert.ToInt32(ddlExamName.SelectedValue);
    //        }
    //        else
    //        {
    //            if (!string.IsNullOrEmpty(Request.QueryString["examID"]))
    //                examid = Convert.ToInt32(Request.QueryString["examID"]);
    //        }

    //        if (ddlCourseName.SelectedValue != "0")
    //        {
    //            courseid = Convert.ToInt32(ddlCourseName.SelectedValue);
    //        }
    //        else
    //        {
    //            if (!string.IsNullOrEmpty(Request.QueryString["courseid"]))
    //                courseid = Convert.ToInt32(Request.QueryString["courseid"]);
    //        }
    //        if (courseid == 1)
    //            lvl = "O";
    //        else if (courseid == 2)
    //            lvl = "A";
    //        if (courseid == 3)
    //            lvl = "B";
    //        else if (courseid == 4)
    //            lvl = "C";

    //        EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
    //        con.Open();
    //        DataTable dt = new DataTable();

    //        string strexamdate = "select top 1 REPLACE(CONVERT(VARCHAR(11),DATEADD(month, DATEDIFF(month, 0,e.Exam_Start_Date),0), 106 ), ' ','-') as exam_dt  from Course_Exam_Application ce , Exam e where ce.Exam_ID = e.ID and ce.Exam_ID = " + examid + " order by ce.Exam_ID desc";
    //        SqlDataReader dr = EConnect.Utils.Data.DbUtility.ExecuteReader(strexamdate, con, null, CommandType.Text, true);
    //        while (dr.Read())
    //        {
    //            examdate = Convert.ToString(dr["exam_dt"].ToString());
    //        }

    //        string strQuery =  " select e.exam_dt , e.batch , e.s_no, e.reg_no ,e.candidate_code, e.name,e.city,e.state,e.pin, e.app_type , e.accr_no, e.inst_nm,e.pr_appr,e.pr_roll,e.cntr_cd1,e.cntr_cd2, " +
    //                           " e.no_pap,e.syl_type ,e.rej, e.entry_dt,e.m_o_exam,e.sy_type_a,e.lvl,e.sc_st,e.pra1,e.pra2,e.pra3,e.pra4, e.ROLL_NO, e.LOC_ALOC , e.CENTRE_CODE , e.CENTRE_NUMERIC,e.VENUE_CODE,e.multiple_form_sr_no, e.state_code," +
    //                           " e.add1,e.add2, e.add3 ,e.ignore_flag , e.dob, e.paymode, e.ddamt1, e.Is_Late_Fee , s.module_code ,s.module_type, s.module_identity, s.sub_code from e_fm e , e_fm_sub s " +
    //                           " where e.form_no = s.form_no and e.lvl = '" + lvl + "'  and  REPLACE(CONVERT(VARCHAR(11), e.exam_dt, 106), ' ', '-') = '" + examdate + "' order by 2, 3 asc ";

    //        string sheetname = ddlCourseName.SelectedItem.Text.Replace("/", "_") + "_" + ddlExamName.SelectedItem.Text.Replace(" ", "").Replace(",", ""); 
    //        dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, null, CommandType.Text, true);
    //        if (dt.Rows.Count > 0)
    //        {
    //            GridView GridView1 = new GridView();
    //            GridView1.AllowPaging = false;
    //            GridView1.DataSource = dt;
    //            GridView1.DataBind();
    //            Response.Clear();
    //            Response.Buffer = true;
    //            Response.AddHeader("content-disposition", "attachment;filename=" + sheetname + "_Exam.xls");
    //            Response.Charset = "";
    //            Response.ContentType = "application/vnd.ms-excel";
    //            StringWriter sw = new StringWriter();
    //            HtmlTextWriter hw = new HtmlTextWriter(sw);
    //            for (int i = 0; i < GridView1.Rows.Count; i++)
    //            {
    //                //Apply text style to each Row
    //                GridView1.Rows[i].Attributes.Add("class", "textmode");
    //            }
    //            GridView1.RenderControl(hw);

    //            //style to format numbers to string

    //            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
    //            Response.Write(style);
    //            Response.Output.Write(sw.ToString());
    //            Response.Flush();
    //            Response.End();
    //            con.Close();
    //        }
    //        else
    //        {
    //            ShowAlert("No record found.");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}
}