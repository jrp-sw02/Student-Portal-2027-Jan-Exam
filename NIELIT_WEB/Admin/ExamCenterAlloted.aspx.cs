using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Common;
using EConnect.NIELIT;
public partial class ExamCenterAlloted : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 CourseID = 0;
    Int32 ExamID = 0;
    Int32 currentRoleId = 0;
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View,"Admin/Examination_Cycle.aspx"))
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
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    fillFilterModuleType();
                    
                    if (!String.IsNullOrEmpty(Request.QueryString["ExamID"]))
                    {
                        using (EConnectContext context = new EConnectContext())
                        {
                            Int32 examID = Convert.ToInt32(Request.QueryString["ExamID"]);
                            var examCenters = (from s in context.ExamWiseExamCenters
                                               where s.ExamID == examID 
                                               select s).ToList();
                            if (examCenters.Count > 0)
                            {
                               
                                //Int32 activityID = (Int32)enmActivity.DateFfCommencementOfOnlineFillInExaminationApplicationForm;
                                //var cutOffDt = (from s in context.CutOffDates
                                //                where (s.ExamID == examID && s.ActivityID == activityID)
                                //                orderby s.EfferctiveDate ascending
                                //                select new { effectiveDt = s.EfferctiveDate }).Take(1);
                                //if (cutOffDt.Count() > 0)
                                //{
                                //    DateTime commencementDt = Convert.ToDateTime(cutOffDt.FirstOrDefault().effectiveDt);
                                //    if (DateTime.Now < commencementDt)
                                //    {
                                //        btnSaveList.Visible = true;
                                //        btnSaveList.Text = "Update";
                                //    }
                                //    else
                                //    {
                                //        btnSaveList.Visible = false ;
                                //    }
                                //    BindGridView();
                                //    PagingBar1.Visible = true;
                                //    PagingBar1.CurrentPageSize = 0;
                                //}
                                //Int32 activityID = (Int32)enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee ;

                                // --- start --- commmented below code by abhi singh dated on 07112023

                                //var cutOffDt = (from s in context.CutOffDates
                                //                where (s.ExamID == examID)// && s.ActivityID == activityID)
                                //                orderby s.EfferctiveDate descending 
                                //                select new { effectiveDt = s.EfferctiveDate }).Take(1);
                                //if (cutOffDt.Count() > 0)
                                //{
                                //    DateTime commencementDt = Convert.ToDateTime(cutOffDt.FirstOrDefault().effectiveDt);
                                //    if (DateTime.Now > commencementDt)
                                //    {
                                //        btnSaveList.Visible = true;
                                //        btnSaveList.Text = "Update";
                                //    }
                                //    else
                                //    {
                                //        btnSaveList.Visible = false;
                                    
                                //    }

                                //}
                                //else
                                //{
                                //    btnSaveList.Visible = true;
                                //    btnSaveList.Text = "Update";
                                //}

                                // --- end code ---

                                btnSaveList.Visible = true;
                                btnSaveList.Text = "Update";
                                BindGridView();
                                PagingBar1.Visible = true;
                                PagingBar1.CurrentPageSize = 0;
                                btnprint.Visible = true; 
                                btnprint.Attributes.Add("Onclick", "window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt("ExamCentreReport.aspx?CourseId=" + Request.QueryString["CourseID"].ToString() + "&ExamId=" + Request.QueryString["ExamID"].ToString() + "&Type=Print") + "');");
                            }
                            else
                            {
                                btnAdd.Visible = true;
                                btnprint.Visible = false;
                            }
                        };
                    }
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Exam Center Allotment", "Admin/ExamCenterAlloted.aspx?" + Request.QueryString.ToString(), ""));
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
            using (EConnectContext context = new EConnectContext())
            {
                var fillModuleType = (from p in context.ModuleTypes
                                      where p.ID == TheoryID || p.ID == PracticalID 
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
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
           
            Int32 ExamID = 0;
         
            if (!String.IsNullOrEmpty(Request.QueryString["ExamID"]))
            {
                ExamID = Convert.ToInt32(Request.QueryString["ExamID"]);
            }

            Exam exam = new Exam();
            exam = context.Exams.Find(ExamID);

            Int32 ModuleTypeID = 0;
            if (ddlFlterModuletype.SelectedValue != "0")
                ModuleTypeID = Convert.ToInt32(ddlFlterModuletype.SelectedValue);
           
            var query = from s in context.ExamCenters
                        where (s.CourseCategoryID == exam.CourseCategoryID
                              && (s.CourseID == null || s.CourseID == exam.CourseID)) && s.IsEnabled==true
                        select new
                        {
                            ID = s.ID,
                            name = s.Name,
                            code=s.Code,
                            state=s.State.Name
        
                        };
          
            if (ModuleTypeID != 0)
            {
                //query = query.Where(s => s.ModuleTypeID == ModuleTypeID);
            }

            query = query.OrderBy(s => s.state).ThenBy(s => s.name);
          
            PagingBar1.Bind(query, ref gvMain);
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
            Response.Redirect("ExamCenterAlloted.aspx", true);
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
            context = new EConnectContext();
            //create and object 
            User objUser;
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
            Response.Redirect("ExamCenterAlloted.aspx?msg=" + strMessage);
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
            //ddlSearchUserType.SelectedValue = "0";
            ddlFlterModuletype.SelectedValue = "0";
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
                Int32 examID = Convert.ToInt32(Request.QueryString["ExamID"]);
                Int32 examCenterId = Convert.ToInt32(gvMain.DataKeys[e.Row.RowIndex].Values[0]);
              
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                DropDownList ddlexamCenterType = (DropDownList)e.Row.FindControl("ddlExamCenterType");
                using (EConnectContext context = new EConnectContext())
                {
                    Exam exams = context.Exams.Find(examID);
                    EnumUtility.BindListObject(ref ddlexamCenterType, typeof(EConnect.NIELIT.enmExamCenterType), new ListItem("--Select One--", "0"));
                    //Int32 none = (int)enmExamCenterType.None;
                    //ddlexamCenterType.Items.RemoveAt(none);
                    if (context.ExamWiseExamCenters.Any(s => s.ExamID == examID))//updating exam centers
                    {
                        var query = (from s in context.ExamWiseExamCenters
                                     where (s.ExamID == examID && s.ExamCenterID == examCenterId)
                                     select new { ExamCentreTypeID = s.ExamCentreTypeID }).FirstOrDefault();
                        if (query != null)
                        {
                            ddlexamCenterType.SelectedValue = query.ExamCentreTypeID.ToString();
                        }
                    }
                    else // if previous exam exists in the tbl ExamWiseExamCenters 
                    {
                        var query1 = (from s in context.Exams
                                      where (s.CourseCategoryID == exams.CourseCategoryID &&
                                            s.CourseID == exams.CourseID && s.ExaminationCycleID == exams.ExaminationCycleID &&
                                            s.ID != examID && s.ExamStartDate < exams.ExamStartDate)
                                      orderby s.ExamStartDate descending
                                      select new { examinationID = s.ID }).Take(1);
                        if (query1.Count() > 0)
                        {
                            Int32 id = Convert.ToInt32(query1.FirstOrDefault().examinationID);
                            if (context.ExamWiseExamCenters.Any(s => s.ExamID == id))
                            {
                                var query = (from s in context.ExamWiseExamCenters
                                             where (s.ExamID == id  && s.ExamCenterID == examCenterId)
                                             select new { ExamCentreTypeID = s.ExamCentreTypeID }).FirstOrDefault();
                                if (query != null)
                                {
                                    ddlexamCenterType.SelectedValue = query.ExamCentreTypeID.ToString();
                                }
                            }
                        }
                    }
                                    
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
        Response.Redirect("ExamCenterAlloted.aspx", true);
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            //context = new EConnectContext();
            btnAdd.Visible = false;
            btnSaveList.Visible = true;
            BindGridView();
            PagingBar1.Visible = true;
            PagingBar1.CurrentPageSize = 0;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }
        finally
        {
            //context.Dispose();
        }
    }
    protected void btnSaveList_Click(object sender, EventArgs e)
    {
        try
        {
            context = new EConnectContext();
            Int32 ExamID = Convert.ToInt32(Request.QueryString["ExamID"]);
          
            if (btnSaveList.Text == "Save")
            {
                Boolean isNotSelectedExamCenterType = false;
                foreach (GridViewRow row in gvMain.Rows)
                {
                    Int32 examCenterType = Convert.ToInt32(((DropDownList)row.FindControl("ddlExamCenterType")).SelectedValue);
                    if (examCenterType != 0)
                    {
                        isNotSelectedExamCenterType = true;
                    }
                }
                if (isNotSelectedExamCenterType == false)
                {
                    throw new Exception("Please Select at least one exam center type");
                }
                foreach (GridViewRow  row in gvMain.Rows)
                {
                    Int32 ExamCenterId = Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Values[0].ToString());
                    Int32 examCenterTypeID = Convert.ToInt32(((DropDownList)row.FindControl("ddlExamCenterType")).SelectedValue);
                    
                    ExamWiseExamCenter objEWEC = new ExamWiseExamCenter();
                    objEWEC.ExamID = ExamID;
                    objEWEC.ExamCenterID = ExamCenterId;
                    objEWEC.ExamCentreTypeID = examCenterTypeID;
                   // objEWEC.ExamModeID = examCenterTypeID;
                    objEWEC.CreatedByID = Convert.ToInt32(Session["UserID"]);
                    objEWEC.CreatedOn = Convert.ToDateTime(DateTime.Now);
                    if (examCenterTypeID != 0)
                    {
                        context.ExamWiseExamCenters.Add(objEWEC);
                    }
                   
                }
                context.SaveChanges();
            }
            else if (btnSaveList.Text == "Update")
            {
                Boolean isNotSelectedExamCenterType = false;
                foreach (GridViewRow row in gvMain.Rows)
                {
                    Int32 examCenterType = Convert.ToInt32(((DropDownList)row.FindControl("ddlExamCenterType")).SelectedValue);
                    if (examCenterType != 0)
                    {
                        isNotSelectedExamCenterType = true;
                    }
                }
                if (isNotSelectedExamCenterType == false)
                {
                    throw new Exception("Please Select at least one exam center type");
                }

                context.Database.ExecuteSqlCommand("delete from Exam_Wise_Exam_Center where Exam_ID='" + ExamID.ToString() + "'");
                foreach (GridViewRow row in gvMain.Rows)
                {
                    Int32 ExamCenterId = Convert.ToInt32(gvMain.DataKeys[row.RowIndex].Values[0].ToString());
                    Int32 examCenterTypeID = Convert.ToInt32(((DropDownList)row.FindControl("ddlExamCenterType")).SelectedValue);

                    ExamWiseExamCenter objEWEC = new ExamWiseExamCenter();
                    objEWEC.ExamID = ExamID;
                    objEWEC.ExamCenterID = ExamCenterId;
                    objEWEC.ExamCentreTypeID = examCenterTypeID;
                    //objEWEC.ExamModeID = examCenterTypeID;
                    objEWEC.CreatedByID = Convert.ToInt32(Session["UserID"]);
                    objEWEC.CreatedOn = Convert.ToDateTime(DateTime.Now);
                    if (examCenterTypeID != 0)
                    {
                        context.ExamWiseExamCenters.Add(objEWEC);
                    }
                }
                context.SaveChanges();                      
            }
            Response.Redirect("ExamCenterAlloted.aspx?" + Request.QueryString.ToString());
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally
        {
            context.Dispose();
        }
    }
}