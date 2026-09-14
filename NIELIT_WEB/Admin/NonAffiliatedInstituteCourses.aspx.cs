using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Transactions;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using EConnect.Utils.Common;
using System.Configuration;
using System.Data.SqlClient;
public partial class NonAffiliatedInstituteCourses : BasePage
{
    String strMessage = string.Empty;
    NIELITMISContext context;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 NielitCentreId = 0;
       
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/NonAffInstitute.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["key1"]))
            {
                hfAccreID.Value = Request.QueryString["key1"];
            }
            if (!Page.IsPostBack)

            {
                User objUser;
                NIELITMISContext context1 = new NIELITMISContext();
                EConnectContext context = new EConnectContext();
                objUser = new EConnect.URM.User();

                User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                //var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                //NielitCentreIdFilter = NielitCentreId;               
                //NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);

                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {                   
                    FillProjects();
                    FillProjectsF();
                    ShowEditMode();
                    BindGridView();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    FillProjectsF();                  
                    BindGridView();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Non Accredited Institute Courses", "Admin/NonAffiliatedInstituteCourses.aspx?" + Request.QueryString.ToString(), ""));
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
    protected void FillProjects()
    {
        try
        {

            User objUser;
           
            EConnectContext context1 = new EConnectContext();
            objUser = new EConnect.URM.User();

            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            //var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
            NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Projects = from p in context.NielitProjectss
                               join c in context.projectMainCentres on p.ID equals c.projectID
                               where c.centreID == NielitCentreId
                               orderby (p.ProjectName)
                               select new { ValueField = p.ID, TextField = p.ProjectName };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlProject, Projects.Distinct(), lst);

               
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillProjectsF()
    {
        try
        {

            User objUser;

            EConnectContext context1 = new EConnectContext();
            objUser = new EConnect.URM.User();

            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            //var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
            NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Projects = from p in context.NielitProjectss
                               join c in context.projectMainCentres on p.ID equals c.projectID
                               where c.centreID == NielitCentreId
                               orderby (p.ProjectName)
                               select new { ValueField = p.ID, TextField = p.ProjectName }; 
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlProjectF, Projects.Distinct(), lst);
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillCategories()
    {
        try
        {
            int ProjectId = Convert.ToInt32(ddlProject.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.NielitCentreCourseCategorys
                               join c in context.NielitCentreCourses on p.ID equals c.CourseCategoryID
                               join d in context.NielitCourseDurations on c.ID equals d.courseID
                               join k in context.NielitProjCoursess on d.ID equals k.courseID
                               where p.IsActive == true && k.projID == ProjectId
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void FillCourseEdit()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {

                ListItem lst = new ListItem("--Select One--", "0");

                int id = Convert.ToInt32(ddlcoursecategory.SelectedValue);

                var CourseList = from p in context.NielitCentreCourses
                                 join d in context.NielitCourseDurations on p.ID equals d.courseID
                                 where p.CourseCategoryID == id                                     
                                && p.ShowOnWeb
                                 select new { ValueField = d.ID, TextField = p.Name + " (" + p.Code + ")" };
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
   
   
    protected void FillCourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                
                ListItem lst = new ListItem("--Select One--", "0");

                int id = Convert.ToInt32(ddlcoursecategory.SelectedValue);

                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == id && (p.ID == 115 || p.ID == 149 || p.ID == 133 || p.ID == 107 )				
                                && p.ShowOnWeb
                                 select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" };
                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, CourseList, lst);
            };
           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
   
    protected void FillStatus()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var StatusList = from p in context.AccreditationStatus
                                 where p.ID == 4 || p.ID == 5
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlstatus, StatusList, lst);
            };
            ddlstatus.SelectedValue = "4";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    //protected void FillFilterCategory()
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            ListItem lst = new ListItem("--Select One--", "0");
    //            var category = from p in context.CourseCategories
    //                           select new { ValueField = p.ID, TextField = p.Name };

    //            if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
    //            {
    //                var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
    //                category = category.Where(a => roleCourses.Contains(a.ValueField));
    //            }
    //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlcategry, category.Distinct(), lst);
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}
    //protected void FillFilterCourse(Int32 CategoryId)
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            ListItem lst = new ListItem("--Select One--", "0");
    //            var statelist = from p in context.Courses
    //                            where p.CourseCategoryID == CategoryId
    //                            orderby p.DisplayOrder
    //                            select new { ValueField = p.ID, TextField = p.Name };
    //            if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
    //            {
    //                var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
    //                statelist = statelist.Where(a => roleCourses.Contains(a.ValueField));
    //            }
    //            statelist = statelist.Distinct();
    //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlcour, statelist, lst);
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}
    //protected void FillFilterStatus()
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            ListItem lst = new ListItem("--Select One--", "0");
    //            var statelist = from p in context.AccreditationStatus
    //                            select new { ValueField = p.ID, TextField = p.Name };

    //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlsts, statelist, lst);
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}
   
    protected void chkBlocked_CheckedChanged(object sender, EventArgs e)
    {
        Session["blockFlag"] = true;
        if (chkBlocked.Checked == false)
        {
            txtBlockDate.Text = "";
            txtBlockDate.Visible = false;
            lblBlockDate.Visible = false;
        }
        else
        {
            lblBlockDate.Visible = true;
            txtBlockDate.Visible = true;
        }
    }   

    protected void ShowEditMode()
    {
        try
        {
            FillProjects();          
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Accredited Centres";
            tblNavLinks.Visible = true;            
            Session["blockFlag"] = false;
           
            Int64 linkcentreID = 0;
            using (NIELITMISContext context = new NIELITMISContext())
            {               
                Int32 AccId = Convert.ToInt32(Request.QueryString["Key"]);
                var NonAffiliatedInstituteCoursesDetail = (from p in context.NonAffiliatedInstituteCoursesDetails
                              where p.ID == AccId
                              select new
                              {
                                  ID = p.ID,
                                  CourseCategoryID=p.CourseCategoryID,
                                  CourseID=p.CourseID,
                                  CourseLinkedNumber=p.courseLinkedNumber,
                                  EffectiveFromDate=p.effectiveFromDate,
                                  EffectiveToDate=p.effectiveToDate,
                                  LinkStatusID=p.linkedStatusID,
								   ProjectID=p.projectId,
                                  WithdrawlDate=p.withdrawlDate ,
                                  instituteID=p.InstituteID,								 
	                			    whetherBlock=p.tempBlocked,
				                    BlockedFromDate=p.BlockedFromDate

                              }).FirstOrDefault();

                Int64 ccid = Convert.ToInt64(NonAffiliatedInstituteCoursesDetail.CourseID.ToString());
                Int64 instid = Convert.ToInt64(NonAffiliatedInstituteCoursesDetail.instituteID.ToString());
                ddlProject.SelectedValue = NonAffiliatedInstituteCoursesDetail.ProjectID.ToString();
                ddlProject.Enabled = false;
                FillCategories();
                ddlcoursecategory.SelectedValue = NonAffiliatedInstituteCoursesDetail.CourseCategoryID.ToString();
               // ddlcoursecategory_SelectedIndexChanged(ddlcoursecategory, EventArgs.Empty);  
                ddlcoursecategory.Enabled = false;
                FillCourseEdit();
                ddlCourse.SelectedValue = NonAffiliatedInstituteCoursesDetail.CourseID.ToString();
                ddlCourse.Enabled = false;
                ddlstatus.SelectedValue = NonAffiliatedInstituteCoursesDetail.LinkStatusID.ToString();
                //ddlstatus.Enabled = false;
                txtCourseLindedNumber.Text = NonAffiliatedInstituteCoursesDetail.CourseLinkedNumber.ToString();
                if (NonAffiliatedInstituteCoursesDetail.EffectiveFromDate.HasValue)
                    txteffectivefrom.Text = NonAffiliatedInstituteCoursesDetail.EffectiveFromDate.Value.ToString("dd-MMM-yyyy");
                if (NonAffiliatedInstituteCoursesDetail.EffectiveToDate.HasValue)
                    txteffectiveto.Text = NonAffiliatedInstituteCoursesDetail.EffectiveToDate.Value.ToString("dd-MMM-yyyy");
					
					 
                if (NonAffiliatedInstituteCoursesDetail.WithdrawlDate.HasValue)
                    txtWithdrawldate.Text = NonAffiliatedInstituteCoursesDetail.WithdrawlDate.Value.ToString("dd-MMM-yyyy");
               
                    if (ddlstatus.SelectedValue.ToString() == "0")
                    {
						//Added 6 June 2020
                        //withdrawal.Visible = true;
                        //withdrawal1.Visible = true;
                blocking.Visible = false;
                chkBlocked.Visible = false;
                lblBlockDate.Visible = false;
                txtBlockDate.Visible = false;
                //
                        txtWithdrawldate.Visible = true;
                        lblWithdrawlDate.Visible = true;
                    }
                    else
                    {				
                blocking.Visible = true;
                chkBlocked.Visible = true;
                chkBlocked.Checked = true;
                lblBlockDate.Visible = true;
                txtBlockDate.Visible = true;               
                        txtWithdrawldate.Visible = false;
                        lblWithdrawlDate.Visible = false;
                        txtWithdrawldate.Text = "";
                    }               

                    if (NonAffiliatedInstituteCoursesDetail.LinkStatusID != 0)
                    {

                        if (NonAffiliatedInstituteCoursesDetail.whetherBlock)
                        {
                            //withdrawal.Visible = false;
                            //withdrawal1.Visible = false;
                            blocking.Visible = true;
                            chkBlocked.Visible = true;
                            chkBlocked.Checked = true;
                            lblBlockDate.Visible = true;
                            txtBlockDate.Visible = true;
                            txtBlockDate.Text = NonAffiliatedInstituteCoursesDetail.BlockedFromDate.Value.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            //withdrawal.Visible = false;
                            //withdrawal1.Visible = false;
                            blocking.Visible = true;
                            chkBlocked.Checked = false;
                            chkBlocked.Visible = true;
                            lblBlockDate.Visible = true;
                            txtBlockDate.Visible = true;
                            txtBlockDate.Text = "";
                        }

                    }
                    else
                    {
                        //If institute withdrawn , cannot be blocked.
                        //withdrawal.Visible = true;
                        //withdrawal1.Visible = true;
                        blocking.Visible = false;
                        chkBlocked.Visible = false;
                        lblBlockDate.Visible = false;
                        txtBlockDate.Visible = false;
                    }

                //Updating breadscrumb
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(NonAffiliatedInstituteCoursesDetail.CourseLinkedNumber, "Admin/NonAffiliatedInstituteCourses.aspx?" + Request.QueryString.ToString(), ""));
                ViewState["LastModifiedOn"] = DateTime.Now;

                //if (!UserManager.HasRight(currentRoleId, enmRight.Edit, "Admin/NonAffInstitute.aspx"))
                //{
                //    btnSave.Visible = false;
                //}
            };
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
            context = new NIELITMISContext();         
            Int64 instituteId = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["key1"]))
            {
                instituteId = Convert.ToInt64(Request.QueryString["key1"]);
            }
            Int32 CourseCategoryID = 0;
            Int32 courseID = 0;
            Int32 StatusID = 0;
            if (ddlcategryF.SelectedValue != "0")
                CourseCategoryID = Convert.ToInt32(ddlcategryF.SelectedValue);
            if (ddlcourF.SelectedValue != "0")
                courseID = Convert.ToInt32(ddlcourF.SelectedValue);
            //if (ddlstsF.SelectedValue != "99")
                //string statuss = ddlstsF.SelectedItem.Text;
                StatusID = Convert.ToInt32(ddlstsF.SelectedValue);
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            ucSearchBar.AutoCompleteContextKey = instituteId.ToString();          
            var centre = from s in context.NonAffiliatedInstituteCoursesDetails
                         join d in context.NielitCourseDurations on s.CourseID equals d.ID
                         join c in context.NielitCentreCourses on d.courseID equals c.ID
                         join n in context.NonAffInstitutes on s.InstituteID equals n.ID
                         join p in context.NielitProjectss on s.projectId equals p.ID
                         where s.InstituteID == instituteId
                         select new { 
                          ID = s.ID, 
                          CourseLinkNumber = s.courseLinkedNumber, 
                          EffectiveFromDate = s.effectiveFromDate, 
                          EffectiveToDate = s.effectiveToDate, 
                          Name = c.Name, 
                          ProjectId=s.projectId,
                          ProjectName=p.ProjectName,
                          CourseID = s.CourseID,
                          LinkStatusID = s.linkedStatusID != 0 ? "Running" : "Withdrawn",
                          SNAME=s.tempBlocked,
                          CourseCategoryID = s.CourseCategoryID,                         
                          Temp_Blocked=s.tempBlocked
                             
                         };
            if (CourseCategoryID != 0)
            {
                centre = centre.Where(s => s.CourseCategoryID == CourseCategoryID);
            }
            if (courseID != 0)
            {
                centre = centre.Where(s => s.CourseID == courseID);
            }
            if (StatusID == 99)
            {
                centre = centre.Where(s => s.LinkStatusID != "99");
            }
            else
            {
             string   statuss = ddlstsF.SelectedItem.Text;
                centre = centre.Where(s => s.LinkStatusID == statuss);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                centre = centre.Where(s => s.CourseLinkNumber.ToUpper().Contains(searchString));
            }        
          
            PagingBar1.Bind(centre, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();
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
        try
        {
            if (btnMode.ViewMode == ToggleView.Mode.New)
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.New, "Admin/NonAffInstitute.aspx"))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to add new record.", true);
                    return;
                }
                FillProjects();              
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                //Change the heading text as required
                lblHeading.Text = "Non-Accredited Centress";
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Non  Accredited Centres Courses", "", ""));               
            }
            else
            {
                if (!String.IsNullOrEmpty(Request.QueryString["key1"]))
                {
                    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NonAffiliatedInstituteCourses.aspx?key1=" + Request.QueryString["key1"].ToString()), true);
                }
                else
                {
                    Response.Redirect("NonAffInstitute.aspx", true);
                }
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
    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {           
            GenerateCourseLinkedNumber();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCourse.Items.Clear();
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
               
                ListItem lst = new ListItem("--Select One--", "0");

                int id = Convert.ToInt32(ddlcoursecategory.SelectedValue);
				 int ProjectId = Convert.ToInt32(ddlProject.SelectedValue);

                var CourseList = from p in context.NielitCentreCourses
                                 join d in context.NielitCourseDurations on p.ID equals d.courseID
								join c in context.NielitProjCoursess on d.ID equals c.courseID
                                 where p.CourseCategoryID == id                                      
                                && p.ShowOnWeb
                                && p.IsVerified==true
                                && p.IsActive
                                && d.isVerified==true
								 && c.projID == ProjectId
								 && c.IsActive ==true
                                 select new { ValueField = d.ID, TextField = p.Name + " (" + d.courseDurationDays.ToString() + " Days)"  };
               // CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, CourseList.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {            
            int ProjectId = Convert.ToInt32(ddlProject.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.NielitCentreCourseCategorys
                               join c in context.NielitCentreCourses on p.ID equals c.CourseCategoryID
                               join d in context.NielitCourseDurations on c.ID equals d.courseID
                               join k in context.NielitProjCoursess on d.ID equals k.courseID
                               where p.IsActive == true && k.projID == ProjectId
                               && k.IsActive == true
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category.Distinct(), lst);
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlProjectF_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlcategryF.Items.Clear();
            int ProjectId = Convert.ToInt32(ddlProjectF.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.NielitCentreCourseCategorys
                               join c in context.NielitCentreCourses on p.ID equals c.CourseCategoryID
				 join d in context.NielitCourseDurations on c.ID equals d.courseID
                               join k in context.NielitProjCoursess on d.ID equals k.courseID
                               where p.IsActive == true && k.projID == ProjectId
				&& k.IsActive == true
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcategryF, Category.Distinct(), lst);
            };

            ddlcourF.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlcategryF_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlcourF.Items.Clear();
			 int ProjectId = Convert.ToInt32(ddlProjectF.SelectedValue);
			 
            using (NIELITMISContext context = new NIELITMISContext())
            {

                ListItem lst = new ListItem("--Select One--", "0");

                int id = Convert.ToInt32(ddlcategryF.SelectedValue);
				var CourseList = from p in context.NielitCentreCourses
                                 join d in context.NielitCourseDurations on p.ID equals d.courseID
								join c in context.NielitProjCoursess on d.ID equals c.courseID
                                 where p.CourseCategoryID == id                                      
                                && p.ShowOnWeb
                                && p.IsVerified==true
                                && p.IsActive
                                && d.isVerified==true
								 && c.projID == ProjectId
								 && c.IsActive ==true
                                 select new { ValueField = d.ID, TextField = p.Name + " (" + d.courseDurationDays.ToString() + " Days)"  };
				
						
				

               /* var CourseList = from p in context.NielitCentreCourses
                                 join d in context.NielitCourseDurations on p.ID equals d.courseID
                                 where p.CourseCategoryID == id                                      
                                && p.ShowOnWeb
                                && p.IsVerified==true
                                && p.IsActive
                                && d.isVerified==true
                                 select new { ValueField = d.ID, TextField = p.Name + " (" + d.courseDurationDays.ToString() + " Days)"  };*/




                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourF, CourseList, lst);
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void GenerateCourseLinkedNumber()
    {
        using (NIELITMISContext context = new NIELITMISContext())
        {
            NonAffInstitute ins;
            ins = context.NonAffInstitutes.Find(Convert.ToInt64(Request.QueryString["key1"]));
            Int64 instId = ins.ID;
            Int64 AutoCourseLinkedID = 9000001;
            string FinalCourseLinkedNumber = "",CourseLinkedNumber = "", NonAfflCode="NAFF";           

            int ProjectId = Convert.ToInt32(ddlProject.SelectedValue);
            int Ccatid = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            int CourseId = Convert.ToInt32(ddlCourse.SelectedValue);

            var InstituteCourseLVL = (from c in context.NonAffiliatedInstituteCoursesDetails
                                      //where c.CourseCategoryID == Ccatid && c.CourseID == CourseId
                                      //&& c.projectId == ProjectId //&& c.ID == instId
                                      select new
                                      {
                                          ID = c.ID,
                                          CourseLinkedID = c.courseLinkedNumber
                                      }).ToList();       
            if (InstituteCourseLVL.Count > 0)
                {
                    var InstituteCourseLVL1 = InstituteCourseLVL.OrderByDescending(x => x.ID).First();// UPN-00001-109
                    CourseLinkedNumber = InstituteCourseLVL1.CourseLinkedID.ToString(); //NAFF-1040-9000001

                    int firsthypen = CourseLinkedNumber.IndexOf('-');
                    int secondhypen = CourseLinkedNumber.LastIndexOf('-');
                    string CourseLinkedIdSeries1 = CourseLinkedNumber.Substring(firsthypen + 1, secondhypen - 4);
                    string CourseLinkedIdSeries = CourseLinkedNumber.Substring(secondhypen + 1);
                    AutoCourseLinkedID = Convert.ToInt64(CourseLinkedIdSeries) + 1;
                    FinalCourseLinkedNumber = NonAfflCode + "-" + CourseId.ToString() + "-" + AutoCourseLinkedID.ToString();
                }
            else
            {
                FinalCourseLinkedNumber = NonAfflCode + "-" + CourseId.ToString() + "-" + AutoCourseLinkedID.ToString();
            }
            txtCourseLindedNumber.Text = FinalCourseLinkedNumber;                                   
        }
    }
   
    protected void SaveRecord(object sender, EventArgs e)
    {
        
        context = new NIELITMISContext();       
        try
        {		
            if (ddlstatus.SelectedValue != "99")
            {
                if (ddlstatus.SelectedValue == "0")
                {
                    if (txtWithdrawldate.Text.Trim().Length == 0)
                    {
                        ShowAlert("Please enter withdrawl date");
                        return;
                    }
                    if (txteffectivefrom.Text.Trim().Length != 0)
                    {
                        if (Convert.ToDateTime(txtWithdrawldate.Text) < Convert.ToDateTime(txteffectivefrom.Text))
                        {
                            ShowAlert("Withdrawl date cannot be less than effectivefrom");
                            return;
                        }
                    }
                }
            }
            else
            {
                ShowAlert("Please Select Link Status ID.");
                return;

            }           
			if(txteffectivefrom.Text.Trim().Length!=0  && txteffectiveto.Text.Trim().Length!=0 )
			{
             if (Convert.ToDateTime(txteffectiveto .Text ) < Convert.ToDateTime(txteffectivefrom.Text))
             {
                ShowAlert("Effective To date cannot be less than effectivefrom");
                return;
             }
			 }
          
            //Added for instt blocking
            if (chkBlocked.Checked)
            {
                if (txtBlockDate.Text.Trim().Length == 0)
                {
                    ShowAlert("Please enter Block From date");
                    return;
                }

                if (Convert.ToDateTime(txtBlockDate.Text) < System.DateTime.Today)
                {
                    ShowAlert("Block From date cannot be less than current date");
                    return;
                }
            }           	

            BreadCrumb1.Render();
          
            NonAffInstitute ins;
            ins = context.NonAffInstitutes.Find(Convert.ToInt64(Request.QueryString["key1"]));           
             int ProjectId = Convert.ToInt32(ddlProject.SelectedValue);
            int Ccatid = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            int CourseId = Convert.ToInt32(ddlCourse.SelectedValue);

            NonAffiliatedInstituteCoursesDetail objCourseLinkedNumberDetail;

                         
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    var existCourse = (from r in context.NonAffiliatedInstituteCoursesDetails
                                       where r.InstituteID == ins.ID && r.CourseID == CourseId
                                       && r.projectId == ProjectId && r.CourseCategoryID == Ccatid
                              select new
                               {
                                   ID = r.ID,
                                   Course = r.CourseID                                   
                               }).ToList();
                    if (existCourse.Count > 0)
                    {                      
                        lblerror.Text = "**Accreditation already granted for the course.";
                        return;
                    }
                    else 
                    {
                        lblerror.Text = "";
                        objCourseLinkedNumberDetail = new EConnect.NIELIT.NonAffiliatedInstituteCoursesDetail();
                        objCourseLinkedNumberDetail.InstituteID = ins.ID;
                        objCourseLinkedNumberDetail.CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                        objCourseLinkedNumberDetail.CourseID = Convert.ToInt32(ddlCourse.SelectedValue);
                        objCourseLinkedNumberDetail.linkedStatusID = Convert.ToInt32(ddlstatus.SelectedValue);
                        objCourseLinkedNumberDetail.courseLinkedNumber = txtCourseLindedNumber.Text.ToString();
                        objCourseLinkedNumberDetail.effectiveFromDate = Convert.ToDateTime(txteffectivefrom.Text);
                        objCourseLinkedNumberDetail.effectiveToDate = Convert.ToDateTime(txteffectiveto.Text);
                        context.NonAffiliatedInstituteCoursesDetails.Add(objCourseLinkedNumberDetail);

                        //Added for withdrawl instt
                        if (txtWithdrawldate.Text.Trim().Length != 0)
                            objCourseLinkedNumberDetail.withdrawlDate = Convert.ToDateTime(txtWithdrawldate.Text);
                       
                        //Added for blocking instt
                        if (chkBlocked.Checked)
                        {
                            objCourseLinkedNumberDetail.tempBlocked = true;

                            if (txtBlockDate.Text.Trim().Length != 0)
                                objCourseLinkedNumberDetail.BlockedFromDate = Convert.ToDateTime(txtBlockDate.Text);
                        }
                        else
                            objCourseLinkedNumberDetail.tempBlocked = false;


                      //  NIELITMISContext context1 = new NIELITMISContext();

                        objCourseLinkedNumberDetail.projectId = Convert.ToInt32(ddlProject.SelectedValue);
                        objCourseLinkedNumberDetail.enterDate = System.DateTime.Now;

                        objCourseLinkedNumberDetail.enterBy = Convert.ToInt32(Session["UserID"]);
                        context.SaveChanges();
                       strMessage = "New record saved.";

                        string CentreName = "";
                        string EmailAddress = "";
                        string CourseLinkedNumber = "";
                        string courseName = "";
                        NIELITMISContext context2 = new NIELITMISContext(); 
                     var    EmailAddressCourseLinkedNumberGenration = (from c in context2.NonAffInstitutes 
                                where c.ID==ins.ID                                 
                                  select new

                                   {
                                       ID = c.ID,
                                       EmailAddress = c.EmailAddress1,
                                       CentreName=c.Name
                                      
                                   }).ToList().FirstOrDefault();
                     if (EmailAddressCourseLinkedNumberGenration != null)
                     {
                         CentreName = EmailAddressCourseLinkedNumberGenration.CentreName.ToString();
                         EmailAddress = EmailAddressCourseLinkedNumberGenration.EmailAddress.ToString();
                         CourseLinkedNumber = txtCourseLindedNumber.Text.ToString();
                         courseName = ddlCourse.SelectedItem.Text;
                     }

                     String EmailMsg = "Dear " + CentreName.ToUpper() + ",  Course Linked number for Course : " + courseName + "  is :  " + CourseLinkedNumber + ". Please note this for future reference.";


                        //sending Email 
                     if (EmailAddress.ToString().Length > 0)
                        {
                            try
                            {
                                EConnect.NIELIT.Email mail = new Email("Online LogIn:NIELIT", EmailMsg, EmailAddress.ToString());
                                mail.Send();
                                //strMessage = "New record saved.";
                            }
                            catch { ShowAlert("CourseLinked Details are sent on E-mail."); }
                        }

                    }                   
                }
                else
                {
                    using (TransactionScope scope = new TransactionScope())
                    { 
                    objCourseLinkedNumberDetail = context.NonAffiliatedInstituteCoursesDetails.Find(Convert.ToInt32(Request.QueryString["key"]));
                    //Added for instt blocking
                    bool x = Convert.ToBoolean(Session["blockFlag"]);
                    
                    if (Convert.ToBoolean(Session["blockFlag"]))
                    {
                        //Add record in history table keeping transaction on
         NonAffiliatedInstituteCoursesDetailHistory objCourseLinkedNumberDetailhistory = new NonAffiliatedInstituteCoursesDetailHistory();
         objCourseLinkedNumberDetailhistory.ID = Convert.ToInt32(Request.QueryString["key"]);
         objCourseLinkedNumberDetailhistory.InstituteID = ins.ID;
         objCourseLinkedNumberDetailhistory.CourseCategoryID = Convert.ToInt32(objCourseLinkedNumberDetail.CourseCategoryID);
         objCourseLinkedNumberDetailhistory.projectId = Convert.ToInt32(objCourseLinkedNumberDetail.projectId);
         objCourseLinkedNumberDetailhistory.CourseID = objCourseLinkedNumberDetail.CourseID;
         objCourseLinkedNumberDetailhistory.linkedStatusID = objCourseLinkedNumberDetail.linkedStatusID;
         objCourseLinkedNumberDetailhistory.courseLinkedNumber = objCourseLinkedNumberDetail.courseLinkedNumber;
         objCourseLinkedNumberDetailhistory.effectiveFromDate = Convert.ToDateTime(objCourseLinkedNumberDetail.effectiveFromDate);
         objCourseLinkedNumberDetailhistory.effectiveToDate = Convert.ToDateTime(objCourseLinkedNumberDetail.effectiveToDate);
         if (objCourseLinkedNumberDetail.withdrawlDate != null)
             objCourseLinkedNumberDetailhistory.withdrawlDate = objCourseLinkedNumberDetail.withdrawlDate;
         objCourseLinkedNumberDetailhistory.tempBlocked = objCourseLinkedNumberDetail.tempBlocked;
         if (chkBlocked.Checked == true)
         objCourseLinkedNumberDetailhistory.BlockedFromDate = objCourseLinkedNumberDetail.BlockedFromDate;

         if (objCourseLinkedNumberDetail.tempBlocked == true && Convert.ToBoolean(Session["blockFlag"]) == true)
                        {
                            if (objCourseLinkedNumberDetail.BlockedFromDate > System.DateTime.Now)
                                objCourseLinkedNumberDetailhistory.BlockedFromDate = objCourseLinkedNumberDetail.BlockedFromDate;
                            else
                                objCourseLinkedNumberDetailhistory.BlockedToDate = System.DateTime.Now;
                        }
                        objCourseLinkedNumberDetailhistory.enterBy = Convert.ToInt32(Session["UserID"]);
                        objCourseLinkedNumberDetailhistory.enterDate = System.DateTime.Now;
                        context.NonAffiliatedInstituteCoursesDetailHistorys.Add(objCourseLinkedNumberDetailhistory);
                        context.SaveChanges();
                    }

                    objCourseLinkedNumberDetail.InstituteID = ins.ID;
                    objCourseLinkedNumberDetail.CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                    objCourseLinkedNumberDetail.CourseID = Convert.ToInt32(ddlCourse.SelectedValue);
                    objCourseLinkedNumberDetail.linkedStatusID= Convert.ToInt32(ddlstatus.SelectedValue);
                   // objAccreDetail.AccreditationNumber = txtaccno.Text.ToString();
                    objCourseLinkedNumberDetail.effectiveFromDate = Convert.ToDateTime(txteffectivefrom.Text);
                    objCourseLinkedNumberDetail.effectiveToDate = Convert.ToDateTime(txteffectiveto.Text);
                   
                    if (txtWithdrawldate.Text.Trim().Length != 0)
                        objCourseLinkedNumberDetail.withdrawlDate = Convert.ToDateTime(txtWithdrawldate.Text);
                    //

                    //Added for instt blocking
                    if (chkBlocked.Checked)
                    {
                        objCourseLinkedNumberDetail.tempBlocked = true;

                        if (txtBlockDate.Text.Trim().Length != 0)
                            objCourseLinkedNumberDetail.BlockedFromDate = Convert.ToDateTime(txtBlockDate.Text);
                    }
                    else
                    {
                        objCourseLinkedNumberDetail.tempBlocked = false;
                        objCourseLinkedNumberDetail.BlockedFromDate = null;
                    }
                    strMessage = "Record updated.";
                    context.SaveChanges();
                    scope.Complete();
                }
               
               
            }          
            if (!String.IsNullOrEmpty(Request.QueryString["key1"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NonAffiliatedInstituteCourses.aspx?key1=" + Request.QueryString["key1"].ToString()), true);
            }
            else
            {        
                Response.Redirect("NonAffiliatedInstituteCourses.aspx", true);
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
           // BindGridView();
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
            ddlProjectF.SelectedValue = "0";
            ddlcategryF.SelectedValue = "0";
            ddlcourF.SelectedValue = "0";
            ddlstsF.SelectedValue = "99";
           // BindGridView();
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
                string href = hl.NavigateUrl;

                if (!String.IsNullOrEmpty(Request.QueryString["key1"]))
                {                
                    href += "&key1=" + Request.QueryString["key1"].ToString();                 
                }
                //hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl + "&key1=" + Request.QueryString["key1"].ToString());
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = hl.NavigateUrl;
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = hl.NavigateUrl;
                HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                h4.NavigateUrl = hl.NavigateUrl;
                HyperLink h5 = (HyperLink)e.Row.Cells[5].Controls[0];
                h5.NavigateUrl = hl.NavigateUrl;
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();              
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count, string contextKey)
    {
        NIELITMISContext context = new NIELITMISContext();
        try
        {
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            Int32 instID = Convert.ToInt32(contextKey.ToString());
            string searchString = prefixText.Trim().ToUpper();
            var centre = from s in context.NonAffiliatedInstituteCoursesDetails
                         where s.InstituteID == instID
                         orderby s.courseLinkedNumber
                         select new { CourseLinkedNumber = s.courseLinkedNumber };
            if (!String.IsNullOrEmpty(searchString))
            {
                centre = centre.Where(s => s.CourseLinkedNumber.ToUpper().Contains(searchString));
            }
            centre = centre.OrderBy(s => s.CourseLinkedNumber);
            foreach (var course in centre)
            {
                items.Add(course.CourseLinkedNumber);
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
            if (!String.IsNullOrEmpty(Request.QueryString["key1"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NonAffiliatedInstituteCourses.aspx?key1=" + Request.QueryString["key1"].ToString()), true);
            }
            else
            {              
                Response.Redirect("NonAffiliatedInstituteCourses.aspx", true);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }  
   
    //protected void ddlcategry_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int courseCategoryID = Convert.ToInt32(ddlcategry.SelectedValue);
    //        FillFilterCourse(courseCategoryID);
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}

    protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlstatus.SelectedValue.ToString() == "0")
            {
				 //Added 6 June 2020
                //withdrawal.Visible = true;
                //withdrawal1.Visible = true;
                blocking.Visible = false;
                chkBlocked.Visible = false;
                lblBlockDate.Visible = false;
                txtBlockDate.Visible = false;
                //
                lblWithdrawlDate.Visible = true;
                txtWithdrawldate.Visible = true;
            }
            else
            {
				//Added 6 June 2020
                //withdrawal.Visible = false;
                //withdrawal1.Visible = false;
                blocking.Visible = true;
                chkBlocked.Visible = true;
                chkBlocked.Checked = false;
                lblBlockDate.Visible = true;
                txtBlockDate.Visible = true;
                //
                lblWithdrawlDate.Visible = false;
                txtWithdrawldate.Visible = false;
                txtWithdrawldate.Text = "";
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}