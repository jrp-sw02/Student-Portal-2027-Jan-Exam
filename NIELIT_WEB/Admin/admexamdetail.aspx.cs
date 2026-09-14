using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
public partial class admexamdetail : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    DateTime examDate;
    DateTime examToDate;
    Int32 CourseID = 0;
    Int32 ExamID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            CourseID = Convert.ToInt32(Request.QueryString["CourseId"]); 
            ExamID = Convert.ToInt32(Request.QueryString["ExamID"]);
            
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    fillModuleType();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    fillFilterModuleType();
                    fillFilterExamSession();
                    fillModuleType();
                    fillExamSession();
                    BindGridView();
                    PagingBar1.CurrentPageSize = 0;
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Time Table", "Admin/admexamdetail.aspx?" + Request.QueryString.ToString(), ""));
                    //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Qualification Eligibility", "Admin/QualificationEligiblity.aspx", ""));
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

    protected void fillFilterModuleType()
    {
        try
        {
            Int32 TheoryID = Convert.ToInt32(enmModuleType.Theory);
            Int32 PracticalID = Convert.ToInt32(enmModuleType.Practical);
            Int32 BridgeCourseID = Convert.ToInt32(enmModuleType.Bridge);
            using (EConnectContext context = new EConnectContext())
            {
                var fillModuleType = (from p in context.ModuleTypes
                                      where p.ID == TheoryID || p.ID == PracticalID || p.ID == BridgeCourseID
                                     orderby p.DisplayOrder
                                     select new { ValueField = p.ID, TextField = p.Name });
                ListItem lst = new ListItem("--All--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlFlterModuletype, fillModuleType, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void fillModuleType()
    {
        try
        {
            Int32 TheoryID=Convert.ToInt32(enmModuleType.Theory);
            Int32 PracticalID=Convert.ToInt32(enmModuleType.Practical);
            Int32 BridgeCourseID=Convert.ToInt32(enmModuleType.Bridge);
            using (EConnectContext context = new EConnectContext())
            {
                var fillModuleType = (from p in context.ModuleTypes
                                      where p.ID == TheoryID || p.ID == PracticalID || p.ID == BridgeCourseID
                                     orderby p.DisplayOrder
                                     select new { ValueField = p.ID, TextField = p.Name });
                ListItem lst = new ListItem("--Select One--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlModuleNType, fillModuleType, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void fillExamSession()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var fillExamSession = (from p in context.ExamSessions
                                      orderby p.Name
                                      select new { ValueField = p.ID, TextField = p.Name });
                ListItem lst = new ListItem("--Select--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamSession, fillExamSession, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void fillFilterExamSession()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var fillSession = (from p in context.ExamSessions
                                         orderby p.Name
                                         select new { ValueField = p.ID, TextField = p.Name });
                ListItem lst = new ListItem("--All--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlFilterExamSession, fillSession, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void fillModuleName()
    {
        try
        {
            Int32 CourseID=Convert.ToInt32(Request.QueryString["CourseID"]);
            Int32 ModuleTypeID=Convert.ToInt32(ddlModuleNType.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                var RevisionNoList = (from p in context.Modules
                                      select new { ValueField = p.RevisionNumber, TextField = p.RevisionNumber }).Distinct().ToList();
                Int32 RevisionNo = RevisionNoList.Max(p => p.TextField);
               
                var fillModuleName = (from p in context.Modules
                                      where p.CourseID == CourseID && p.RevisionNumber == RevisionNo && p.ModuleTypeID == ModuleTypeID
                                         orderby p.Code
                                         select new { ValueField = p.ID, TextField =p.ShortName + "    " + p.Name});
                ListItem lst = new ListItem("--Select One--", "0");
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlModuleName, fillModuleName, lst);
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
            lblHeading.Text = "Form Header Detail";
            tblNavLinks.Visible = true;
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
            
            context = new EConnectContext();
            Exam exam = context.Exams.Find(ExamID);
            examDate = exam.ExamStartDate;
            Int32 CourseID = 0;
            Int32 ModuleTypeID = 0;
            Int32 ProjectType = Convert.ToInt32(enmModuleType.Project);
            Int32 preRevisionNumber =0;
            
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
            }
            Course objCourse = context.Courses.Find(CourseID);
            if (objCourse.enmCourseType == enmCourseType.CertificationCourse)
            {    
               // ==============================================================================
                var Isrevisionchoice_effective = (from p in context.RevisionChoices
                                                  where p.course_id == CourseID && p.whether_show_revision_choice == "Y" && p.show_revision_choice_till_date >= DateTime.Now
                                                  orderby p.revision_choice_effective_date descending
                                                  select p).Distinct().FirstOrDefault();

                preRevisionNumber=Isrevisionchoice_effective!=null?Isrevisionchoice_effective.previous_revision_number:-1;
                //==============================================================================
                Int32 RevisionNo = (from p in context.Modules
                                    where p.CourseID == CourseID
                                    select p.RevisionNumber).Distinct().Max();
              
                    if (ddlFlterModuletype.SelectedValue != "0")
                        ModuleTypeID = Convert.ToInt32(ddlFlterModuletype.SelectedValue);

                    if (context.ExamTimeTables.Any(s => s.ExamID == ExamID))
                    {
                        var query = from s in context.ExamTimeTables
                                    join c in context.Modules on s.ModuleID equals c.ID
                                    //where s.ModuleID == c.ID && s.ExamID == ExamID && (s.ID == 4178 || s.ID == 4179 || s.ID == 4180 || s.ID == 4181 || s.ID == 4182 || s.ID == 4183 || s.ID == 4184 || s.ID == 4185 || s.ID == 4186 || s.ID == 4187)
                                    where s.ModuleID == c.ID && s.ExamID == ExamID 
                                    orderby c.RevisionNumber
                                    select new
                                    {
                                        ID = c.ID,
                                        TimeTableID = s.ID,
                                        ModuleTypeID = s.Module.ModuleTypeID,
                                        ModuleType = s.Module.ModuleType.Name,
                                        ModuleName = s.Module.Name,
                                        ShortName = s.Module.ShortName,
                                        ExamSession = s.ExamSessiionID == null ? 0 : s.ExamSessiionID,
                                        ExamDate = s.ExamFomDate,
                                        ExamToDate = s.ExamToDates

                                    };

                        if (ModuleTypeID != 0)
                        {
                            query = query.Where(s => s.ModuleTypeID == ModuleTypeID);
                        }

                        PagingBar1.Bind(query, ref gvMain);
                        foreach (GridViewRow row in gvMain.Rows)
                        {
                            Module module = context.Modules.Find(Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Values[0]));
                            DropDownList ddlExamSession = (DropDownList)row.FindControl("ddlExamSession");
                            TextBox txtExamFromDate = (TextBox)row.FindControl("txtExamDate");
                            TextBox txtExamToDate = (TextBox)row.FindControl("txtExamToDate");
                            ddlExamSession.SelectedValue = gvMain.DataKeys[row.RowIndex].Values[1].ToString();
                            if (module.enmModuleType == enmModuleType.Practical)
                            {
                                ddlExamSession.Enabled = false;
                            }

                                // else if (module.enmModuleType == enmModuleType.Theory)
                                //else if (module.enmModuleType == enmModuleType.Theory && module.ID != 938 && module.ID != 939 && module.ID != 940 && module.ID != 941 && module.ID != 929 && module.ID != 930 && module.ID != 931 && module.ID != 932)
                             else if ((module.enmModuleType == enmModuleType.Theory||module.enmModuleType == enmModuleType.Bridge) && module.ExamModeId!=1)
                            {
                                txtExamToDate.Text = "";
                                txtExamToDate.Enabled = false;
                            }
                            //if (module.ID == 938 || module.ID == 939 || module.ID == 940 || module.ID == 941 || module.ID == 929 || module.ID == 930 || module.ID == 931 || module.ID == 932)
                            if (module.ExamModeId==1)
                            {
                               
                                ddlExamSession.Enabled = false;
                            }

                        }
                        //if (IsPostBack)
                        //{
                        //    btnSaveList.Text = "Update";
                        //}
                        //else
                        //{
                        //   // btnSaveList.Text = "Edit";
                        //}
                        if (btnSaveList.Text == "Save")
                        {
                            btnSaveList.Text = "Edit";
                            gvMain.Enabled = false;
                        }
                        btnPublish.Visible = true;
                        txtPublish.Visible = true;
                        lblPublish.Visible = true;
                        txtPublish.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                    }
                    else
                    {
                        var query = from s in context.Modules
                                    where s.CourseID == CourseID && (s.RevisionNumber == RevisionNo || s.RevisionNumber == preRevisionNumber) && s.ModuleTypeID != ProjectType
                                    orderby s.RevisionNumber
                                    select new
                                    {
                                        ID = s.ID,
                                        TimeTableID = 0,
                                        ModuleTypeID = s.ModuleType.ID,
                                        ModuleType = s.ModuleType.Name,
                                        ModuleName = s.Name,
                                        ShortName = s.ShortName,
                                        courseID = s.CourseID,
                                        ExamSession = 0,
                                        ExamDate = examDate,
                                        ExamToDate = 0
                                    };                      

                        if (ModuleTypeID != 0)
                        {
                            query = query.Where(s => s.ModuleTypeID == ModuleTypeID);
                        }

                        PagingBar1.Bind(query, ref gvMain);

                        //Boolean electiveFlag = false;
                        DateTime dt = DateTime.MinValue;
                        foreach (GridViewRow row in gvMain.Rows)
                        {
                            Int32 moduleID=Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Values[0]);
                            Module module = context.Modules.Find(moduleID);
                            
                            TextBox txtDate = (TextBox)row.FindControl("txtExamDate");
                            TextBox txtToDate = (TextBox)row.FindControl("txtExamToDate");
                            TextBox txtExamToDate = (TextBox)row.FindControl("txtExamToDate");
                            DropDownList gvddlExamSession = (DropDownList)row.FindControl("ddlExamSession");

                            var query1 = (from s in context.TimeTablePatterns
                                         where (s.CourseID == CourseID &&
                                                (s.RevisionNumber == RevisionNo || s.RevisionNumber == preRevisionNumber) && s.ModuleID == moduleID 
                                                )
                                          //select new {examDay = s.ExamDay }).FirstOrDefault();
                                          select new { examSessionID = s.ExamSessionID, examDay = s.ExamDay }).FirstOrDefault();

                            if (query1 != null)
                            {
                                Int32 examDayToAdd = Convert.ToInt32(query1.examDay.ToString());
                                //gvddlExamSession.SelectedValue = query1.examSessionID.ToString();
                                gvddlExamSession.SelectedValue = "0";
                                dt = examDate.AddDays(examDayToAdd-1);
                                txtDate.Text = dt.ToString("dd-MMM-yyyy");

                            }
                            //if (dt == DateTime.MinValue)
                            //    dt = Convert.ToDateTime(txtDate.Text);
                            //elsebtnSaveList
                            //{
                            //    if (module.enmSelectionType == enmSelectionType.Compulsory)
                            //    {
                            //        electiveFlag = false;
                            //        dt = dt.AddDays(1);
                            //    }

                            //    else
                            //    {
                            //        if (electiveFlag == false)
                            //            dt = dt.AddDays(1);
                            //        electiveFlag = true;
                            //    }
                            //}
                            //txtDate.Text = dt.ToString("dd-MMM-yyyy");

                            //commented by abhi singh dated on 13102023
                            //if (module.enmModuleType == enmModuleType.Practical)
                            //{
                            //    var query2 = (from s in context.TimeTablePatterns
                            //                  where (s.CourseID == CourseID &&
                            //                         (s.RevisionNumber == RevisionNo || s.RevisionNumber == preRevisionNumber)
                            //                         )
                            //                  orderby s.ExamDay descending
                            //                  select new { examDay = s.ExamDay }).Take(1);

                            //    if (query2.Count() > 0)
                            //    {
                            //        Int32 examDayToAdd = Convert.ToInt32(query2.FirstOrDefault().examDay.ToString());
                            //        dt = examDate.AddDays(examDayToAdd);
                            //        txtDate.Text = dt.ToString("dd-MMM-yyyy");
                            //        txtToDate.Text = dt.ToString("dd-MMM-yyyy");
                            //    }
                               
                            //    gvddlExamSession.Enabled = false;
                            //}
                            //else if (module.enmModuleType == enmModuleType.Theory || module.enmModuleType == enmModuleType.Bridge)
                            //{
                            //    txtExamToDate.Text = "";
                            //    txtExamToDate.Enabled = false;
                            //}

                            // below code added by abhi singh dated on 13102023
                            ddlExamSession.SelectedValue = gvMain.DataKeys[row.RowIndex].Values[1].ToString();
                            if (module.enmModuleType == enmModuleType.Practical)
                            {
                                gvddlExamSession.Enabled = false;
                            }
                            //else if (module.enmModuleType == enmModuleType.Theory && module.ID != 938 && module.ID != 939 && module.ID != 940 && module.ID != 941 && module.ID != 929 && module.ID != 930 && module.ID != 931 && module.ID != 932)
                            // else if (module.enmModuleType == enmModuleType.Theory)
                           else if ((module.enmModuleType == enmModuleType.Theory||module.enmModuleType == enmModuleType.Bridge) && module.ExamModeId!=1)
                            {
                                txtExamToDate.Text = "";
                                txtExamToDate.Enabled = false;
                            }
                            //if (module.ID == 938 || module.ID == 939 || module.ID == 940 || module.ID == 941 || module.ID == 929 || module.ID == 930 || module.ID == 931 || module.ID == 932)
                            if (module.ExamModeId==1)
                            {

                                gvddlExamSession.Enabled = false;
                            }
                        }
                        btnPublish.Visible = false;
                        txtPublish.Visible = false;
                        lblPublish.Visible = false;
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                    }
                    if (exam.DateOfPublishingOfTimeTable.HasValue == true)
                    {
                        //btnSaveList.Visible = false;
                         btnPublish.Visible = false;
                         lblPublish.Visible = false;
                         txtPublish.Visible = false;
                         //lblPublish.Enabled = false;
                         //txtPublish.Enabled = false;
                        btnPrint.Visible = true;
                    }
                    else
                    {
                        btnPrint.Visible = false;
                    }

                    //BreadCrumb1.Render();
                    btnPrint.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("ExamTimeTableReport.aspx?CourseId=" + Request.QueryString["CourseID"].ToString() + "&ExamId=" + Request.QueryString["ExamID"].ToString() + "&Type=Print") + "');");
                }
                else
                {
                    lblError1.Visible = true;
                    lblError1.Text = "No Modules found so Time Table can not be created";
                    btnSaveList.Visible = false;
                    btnPrint.Visible = false;
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
            fillModuleType();
            fillExamSession();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New Exam Time Table";
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Time Table", "", ""));
            //BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("New Qualification Eligibility", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]) && !String.IsNullOrEmpty(Request.QueryString["ExamId"]))
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("admexamdetail.aspx?" + Request.QueryString.ToString()), true);
            }
            else
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                Response.Redirect("admexamdetail.aspx", true);
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
            if (String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                Int32 ExamID = Convert.ToInt32(Request.QueryString["ExamID"]);
                Int32 ModuleTypeID = Convert.ToInt32(ddlModuleNType.SelectedValue);
                Int32 ModuleID = Convert.ToInt32(ddlModuleNType.SelectedValue);
                Int32 ExamSessionID=Convert.ToInt32(ddlExamSession.SelectedValue);
                using (EConnectContext context = new EConnectContext())
                {                  
                    Course objCourse = new EConnect.NIELIT.Course();
                    Int32 couID = Convert.ToInt32(Request.QueryString["CourseId"]);
                    objCourse = context.Courses.Find(couID);
                    ExamTimeTable ExamTimeTable = new ExamTimeTable();
                    ExamTimeTable.CourseCategoryID = objCourse.CourseCategoryID;
                    ExamTimeTable.CourseID = objCourse.ID;
                    ExamTimeTable.ExamID = ExamID;
                    //ExamTimeTable.ModuleTypeID = ModuleTypeID;
                    ExamTimeTable.ModuleID = ModuleID;
                    ExamTimeTable.ExamSessiionID = ExamSessionID;
                    ExamTimeTable.ExamFomDate = Convert.ToDateTime(txtDateOfExam.Text);
                    context.ExamTimeTables.Add(ExamTimeTable);
                    context.SaveChanges();
                    strMessage = "New record saved.";
                };
          
            }
            else
            {
                ////Initialize current object by loading it and get its current modified date
                strMessage = "Record updated.";
            }
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("admexamdetail.aspx?msg=" + strMessage), true);
            //if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            //{
            //    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("QualificationEligiblity.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&msg=" + strMessage), true);
            //}
            //else
            //{
            //    Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("QualificationEligiblity.aspx?msg=" + strMessage), true);
            //}
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
            BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Exam Time Table Eligibility", "", ""));
            upBreadCrumb.Update();
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            PagingBar1.CurrentPageSize = 0;
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
            ddlFlterModuletype.SelectedValue = "0";
            ddlFilterExamSession.SelectedValue = "0";
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
                String recordID = hfActionID.Value.Split('$')[0].ToString();
                LinkButton btnAction = (LinkButton)sender;
                if (btnAction.CommandName == "Delete")
                {
                    //Load the object and apply validateion if required
                    //call delete function
                    //bind the grid again
                    BindGridView();
                    ShowAlert("Record deleted successfully.", true);
                    hfActionID.Value = "";
                }
                else if (btnAction.CommandName == "Action")
                {
                    //Load the object and apply validateion if required
                    //call function to perform required action
                    //bind the grid again
                    BindGridView();
                    ShowAlert("Record Action1 successfully.", true);
                    hfActionID.Value = "";
                }
                uPnlGrid.Update();
            }
        }
        catch (Exception ex)
        {
            hfActionID.Value = "";
            ShowAlert(ex.Message, true);
        }
        finally { context.Dispose(); }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                DropDownList ddlExamSession = (DropDownList)e.Row.FindControl("ddlExamSession");
                using (EConnectContext context = new EConnectContext())
                {
                    var fillExamSession = (from p in context.CourseExamSessions
                                           where p.CourseID == CourseID
                                           select new { ValueField = p.ExamSession.ID, TextField = p.ExamSession.Name });

                    ListItem lst = new ListItem("--Select--", "0");
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamSession, fillExamSession, lst);
                };              
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]) && !String.IsNullOrEmpty(Request.QueryString["ExamID"]))
        {
            BreadCrumb1.RemoveLastBreadCrumbItem();
            BreadCrumb1.RemoveLastBreadCrumbItem();
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("admexamdetail.aspx?ExamID="+Request.QueryString["ExamID"] + "&CourseId=" + Request.QueryString["CourseId"].ToString()));
        }
        else
        {
            Response.Redirect("admexamdetail.aspx", true);
        }
    }
    protected void btnSaveList_Click(object sender, EventArgs e)
    {
        try
        {
            if (btnSaveList.Text == "Edit")
            {
                btnSaveList.Text = "Update";
                gvMain.Enabled = true;
                uPnlGrid.Update();
            }
            else
            {
                DateTime lastDate = DateTime.MinValue;
                Int32 lastExSessionID = 0;
                context = new EConnectContext();
                foreach (GridViewRow row in gvMain.Rows)
                {
                    Int32 ExSessionID = Convert.ToInt32(((DropDownList)row.FindControl("ddlExamSession")).SelectedValue);

                    Int32 moduleID = Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Values[0]);
                    //enmModuleType moduleTypeID = (enmModuleType)Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Values[0]);
                    Module module = context.Modules.Find(moduleID);
                    //if (module.enmModuleType != enmModuleType.Practical && module.ID != 938 && module.ID != 939 && module.ID != 940 && module.ID != 941 && module.ID != 929 && module.ID != 930 && module.ID != 931 && module.ID != 932)
                    if (module.enmModuleType != enmModuleType.Practical && module.ExamModeId!=1)
                    {
                        if (ExSessionID == 0)
                            throw new Exception("Please select exam session in case of theory type module");
                    }
                    //validating date
                    DateTime exDate = DateTime.MinValue;
                    try
                    {
                        exDate = Convert.ToDateTime(((TextBox)row.FindControl("txtExamDate")).Text);
                        //examToDate = Convert.ToDateTime(((TextBox)row.FindControl("txtExamToDate")).Text);
                        //if (module.enmModuleType == enmModuleType.Practical)
                        //{
                        //    if(exDate>examToDate)
                        //         throw new Exception("Please Enter from date less than exam to date");
                        //}

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Invalid date of exam");
                    }
                    //validate date of exam.
                    //if (lastDate != DateTime.MinValue)
                    //{
                    //    if (lastDate > exDate)
                    //        throw new Exception("Exam date can not be less than exam date of previous module in list");
                    //    else { 
                    //    if(lastExSessionID !=0)
                    //    {
                    //        if(lastExSessionID == ExSessionID && lastDate == exDate)
                    //            throw new Exception("Exam can not be scheduled in the same date at same exam session for 2 different Elective modules.");
                    //    }
                    //    }
                    //}
                    lastDate = exDate;
                    lastExSessionID = ExSessionID;
                    // BreadCrumb1.Render();
                }
                Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseId"]);
                Int32 ExamID = Convert.ToInt32(Request.QueryString["ExamID"]);
                Int32 ProjectType = Convert.ToInt32(enmModuleType.Project);
                Int32 courseCategoryID = context.Courses.Select(c => c.CourseCategoryID).First();
                if (btnSaveList.Text == "Save")
                {
                    foreach (GridViewRow row in gvMain.Rows)
                    {
                        Int32 ExSessionID = Convert.ToInt32(((DropDownList)row.FindControl("ddlExamSession")).SelectedItem.Value);
                        DateTime exDate = Convert.ToDateTime(((TextBox)row.FindControl("txtExamDate")).Text);
                        //DateTime exToDate = Convert.ToDateTime(((TextBox)row.FindControl("txtExamToDate")).Text);
                        Int32 moduleID = Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Values[0]);
                        //Int32 moduleTypeID = Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Values[1]);
                        Module module = context.Modules.Find(moduleID);
                        ExamTimeTable ExamTimeTable = new ExamTimeTable();
                        ExamTimeTable.CourseCategoryID = courseCategoryID;
                        ExamTimeTable.CourseID = CourseID;
                        ExamTimeTable.ExamID = ExamID;
                        //ExamTimeTable.ModuleTypeID = moduleTypeID;
                        ExamTimeTable.ModuleID = moduleID;
                        if (module.enmModuleType != enmModuleType.Practical)
                            ExamTimeTable.ExamSessiionID = ExSessionID;
                        if (module.enmModuleType == enmModuleType.Practical || module.ExamModeId==1)
                            ExamTimeTable.ExamToDates = Convert.ToDateTime(((TextBox)row.FindControl("txtExamToDate")).Text);

                        ExamTimeTable.ExamFomDate = exDate;
                        context.ExamTimeTables.Add(ExamTimeTable);
                    }
                    context.SaveChanges();
                }
                else if (btnSaveList.Text == "Update")
                {
                    foreach (GridViewRow row in gvMain.Rows)
                    {
                        Int32 ExSessionID = Convert.ToInt32(((DropDownList)row.FindControl("ddlExamSession")).SelectedItem.Value);
                        DateTime exDate = Convert.ToDateTime(((TextBox)row.FindControl("txtExamDate")).Text);
                        Int32 moduleID = Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Values[0]);
                        Int32 TimeTableID = Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Values[2]);
                        Module module = context.Modules.Find(moduleID);
                        ExamTimeTable ExamTimeTable = new ExamTimeTable();
                        ExamTimeTable = context.ExamTimeTables.Find(TimeTableID);
                        ExamTimeTable.CourseCategoryID = courseCategoryID;
                        ExamTimeTable.CourseID = CourseID;
                        ExamTimeTable.ExamID = ExamID;
                        ExamTimeTable.ModuleID = moduleID;
                        if (module.enmModuleType != enmModuleType.Practical)
                            ExamTimeTable.ExamSessiionID = ExSessionID;
                        //if (module.enmModuleType == enmModuleType.Practical || module.ID == 938 || module.ID == 939 || module.ID == 940 || module.ID == 941 || module.ID == 929 || module.ID == 930 || module.ID == 931 || module.ID == 932)
                        if (module.enmModuleType == enmModuleType.Practical || module.ExamModeId==1)
                            ExamTimeTable.ExamToDates = Convert.ToDateTime(((TextBox)row.FindControl("txtExamToDate")).Text);
                        ExamTimeTable.ExamFomDate = exDate;
                    }
                    context.SaveChanges();
                }
                //};
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("admexamdetail.aspx?" + Request.QueryString.ToString()), true);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally
        {
          //  context.Dispose();
        }
    }
    protected void btnPublish_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            context = new EConnectContext();
            //DateTime PublishDate;
            Exam exam = context.Exams.Find(ExamID);
            examDate = exam.ExamStartDate;
            if (txtPublish.Text=="")
            {
                ShowAlert("Publish Date can not be blank",true);
                return;
            }
            else if (base.IsDate(txtPublish.Text)==false)
            {
                ShowAlert("Invalid Publish Date", true);
                return;
            }
            else if (examDate < Convert.ToDateTime(txtPublish.Text))
            {
                ShowAlert("Publishing Date should be less than Exam Start Date", true);
                return;
            }
            else
            {
                exam.DateOfPublishingOfTimeTable = Convert.ToDateTime(txtPublish.Text);
                context.SaveChanges();
            }
            ShowAlert("Time Table has been published", true);
            //Response.Redirect("admexamdetail.aspx?" + Request.QueryString.ToString());
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("admexamdetail.aspx?" + Request.QueryString.ToString()), true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
            context.Dispose();
        }
    }
    
}