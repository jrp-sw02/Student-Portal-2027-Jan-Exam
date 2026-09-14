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
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

public partial class SemesterSubjectMaster : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int64 NielitCentrelinkedToCentreId = 0;
    Int32 NielitCentreIdFilter = 0, NonAfflAfflInstID = 0;
    Int32 UserTypeId = 0;
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
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);

            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }


            if (!Page.IsPostBack)
            {
                User objUser;
                using (EConnectContext context = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();
                    ///Login for all  today
                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    using (NIELITMISContext context1 = new NIELITMISContext())
                    {

                        if (UserTypeId == 10)
                        {
                            var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                            NielitCentreIdFilter = NielitCentreId;
                            HNonAfflAfflInst.Value = "99";
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            if (NielitCentrelinkedToCentreId != 0)
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                                //txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                NielitCentreIdFilter = NelitCentreLinkId;
                                ///Login for all  todayyy
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                //RdoAffInstOrNonAffInst.SelectedValue = "2";
                                //ddlSubcentreName.Enabled = false;
                            }
                            else
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                                //txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                NielitCentreIdFilter = NelitCentreLinkId;
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                            }
                        }
                        else if (UserTypeId == 11)
                        {
                            var intituteslinkedToCentre = context1.NonAffInstitutes.Find(loginUser.UserRefNumber); //HNonAfflAfflInst
                            NonAfflAfflInstID = Convert.ToInt32(intituteslinkedToCentre.ID);
                            HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                //txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                NielitCentreIdFilter = NelitCentreLinkId;
                            }
                        }
                        else if (UserTypeId == 4)
                        {
                            var intituteslinkedToCentre = context1.AffInstitutes.Find(loginUser.UserRefNumber);
                            NonAfflAfflInstID = Convert.ToInt32(intituteslinkedToCentre.ID);
                            HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                NielitCentreIdFilter = NelitCentreLinkId;
                            }

                        }
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        BindEditNewModeData();
                        ShowEditMode();
                    }
                    else
                    {
                        FillBatch();
                        FillSubject();
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";
                        BindGridView();
                        FillBatchFilter();
                        //FillBatch();
                        if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Semester Subject Details", "HO/SemesterSubjectMaster.aspx?Id=" + Request.QueryString["Id"].ToString() + ""));
                        }
                        else
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Semester Subject Details", "HO/SemesterSubjectMaster.aspx", ""));
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

    protected void BindEditNewModeData()
    {

        try
        {
            int id = -1;
            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                id = int.Parse(Request.QueryString["Key"].ToString());
            }
            //Logic: Get Batch code and Batch Id , SubId and Subject name and no of credits based on this Id

            //Who+ile update check for zero count of students for this batch and if zero then pass update query--to update no of credits
            {
                ListItem lst = new ListItem("--Select--", "0");
                using (DataTable dt = GetSemesterSubjectMasterDataForUpdate(id))    //get course for course dropdown
                {
                    if (dt.Rows.Count > 0)
                    {
                        ddlBatch.DataSource = dt;
                        ddlBatch.DataTextField = "BatchCode";
                        ddlBatch.DataValueField = "batchid";
                        ddlBatch.DataBind();
                        ddlBatch.Items.Insert(0, new ListItem(dt.Rows[0]["BatchCode"].ToString(), dt.Rows[0]["batchid"].ToString()));
                        ddlSubjects.Items.Insert(0, new ListItem(dt.Rows[0]["SubjectName"].ToString(), dt.Rows[0]["SubjectId"].ToString()));
                        ddlSemester.Items.Insert(0, new ListItem(dt.Rows[0]["SemNo"].ToString(), dt.Rows[0]["batchid"].ToString()));
                        txtCredits.Text = dt.Rows[0]["NoOfCredits"].ToString();

                    }
                }

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //Binding For insert
    protected void FillBatch()
    {


        try
        {
            Int64 batchid = Convert.ToInt32(ddlBatch.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ddlBatch.Items.Clear();

                
                ListItem lst = new ListItem("--Select--", "0");
                var batchdata = from p in context.SemesterMaster
                                join c in context.NielitCourseDurations on p.CourseId equals c.ID
                                join s in context.NielitCentreCourses on c.courseID equals s.ID
                                join b in context.NielitCentreBatchs on c.ID equals b.CourseDurationID
                                where b.centreID == entityID
                                //   orderby (p.Result)
                                select new { ValueField = b.ID, TextField = s.Name + " (" + b.BatchCode + ")" };
                batchdata = batchdata.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, batchdata.Distinct(), lst);
                btnSave.Visible = true;
                btnCancel.Visible = true;
                // lblMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }

    protected void FillSubject()
    {


        try
        {
            Int32 subid = Convert.ToInt32(ddlSubjects.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ddlSubjects.Items.Clear();
                ListItem lst = new ListItem("--Select--", "0");
                var subjectdata = from p in context.SubjectMaster

                                  select new { ValueField = p.ID, TextField = p.Name };
                subjectdata = subjectdata.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubjects, subjectdata.Distinct(), lst);
                btnSave.Visible = true;
                btnCancel.Visible = true;
                // lblMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }
    protected bool IsValidForm()
    {
        try
        {
            return true;
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
            lblHeading.Text = "Semester Subject Details";
            tblNavLinks.Visible = false;
            EConnectContext context1 = new EConnectContext();
            User objUser = new EConnect.URM.User();
            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int64 Id = Convert.ToInt32(Request.QueryString["Key"]);
                ddlBatch.Enabled = false;
                ddlSubjects.Enabled = false;
                ddlSemester.Enabled = false;

            };
            if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
            {
                btnSave.Visible = true;
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
            using (NIELITMISContext context = new NIELITMISContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                Int64 batchname = 0;
            

                if (ddlbatchname.SelectedValue != "0")
                    batchname = Convert.ToInt64(ddlbatchname.SelectedValue.Trim());

                

                using (DataTable dt = GetSemesterSubjectMasterDataForGrid())
                {
                    if (dt.Rows.Count > 0)
                    {
                        var SemesterSubjectDetails = (from p in dt.AsEnumerable()
                                                      select new
                                                      {
                                                          Id = p.Field<Int64>("Id"),
                                                          SId = p.Field<int>("SId"),
                                                          ///CourseId = p.Field<Int64>("CourseId"),
                                                          BatchCode = p.Field<string>("BatchCode"),
                                                          semno = p.Field<int>("semno"),
                                                          //  SubjectId = p.Field<int>("SubjectId"),
                                                          SubjectName = p.Field<string>("SubjectName"),
                                                          NoOfCredits = p.Field<int>("NoOfCredits"),

                                                      });

                        if (!String.IsNullOrEmpty(searchString))
                        {
                            SemesterSubjectDetails = SemesterSubjectDetails.Where(s => s.BatchCode.ToUpper().Contains(searchString));
                        }


                        if (batchname > 0)
                        {
                            SemesterSubjectDetails = SemesterSubjectDetails.Where(s => s.Id == batchname);
                        }


                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "Id":
                                    if (sortOrder == "DESC")
                                        SemesterSubjectDetails = SemesterSubjectDetails.OrderByDescending(s => s.Id);
                                    else
                                        SemesterSubjectDetails = SemesterSubjectDetails.OrderBy(s => s.Id);
                                    break;
                                case "BatchCode":
                                    if (sortOrder == "DESC")
                                        SemesterSubjectDetails = SemesterSubjectDetails.OrderByDescending(s => s.BatchCode);
                                    else
                                        SemesterSubjectDetails = SemesterSubjectDetails.OrderBy(s => s.BatchCode);
                                    break;

                                case "SubjectName":
                                    if (sortOrder == "DESC")
                                        SemesterSubjectDetails = SemesterSubjectDetails.OrderByDescending(s => s.SubjectName);
                                    else
                                        SemesterSubjectDetails = SemesterSubjectDetails.OrderBy(s => s.SubjectName);
                                    break;
                                case "NoOfCredits":
                                    if (sortOrder == "DESC")
                                        SemesterSubjectDetails = SemesterSubjectDetails.OrderByDescending(s => s.NoOfCredits);
                                    else
                                        SemesterSubjectDetails = SemesterSubjectDetails.OrderBy(s => s.NoOfCredits);
                                    break;

                                default:
                                    SemesterSubjectDetails = SemesterSubjectDetails.OrderBy(s => s.Id);
                                    break;
                            }
                        }

                        PagingBar1.Bind(SemesterSubjectDetails, ref gvMain);
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                        // gvMain.Columns[5].Visible = false;
                        //if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                        //{
                        //    gvMain.Columns[7].Visible = false;
                        //}
                    }
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
            //if (!UserManager.HasRight(currentRoleId, enmRight.New))   fresh
            //{
            //    BreadCrumb1.Render();
            //    ShowAlert("Sorry! You don't have rights to add new record.", true);
            //    return;
            //}
            BindEditNewModeData();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //this.Rview.Visible = false;
            //this.Rview1.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Semester Subject Master";
            //Updating Breadcrumb         
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Semester Subject ", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("SemesterSubjectMaster.aspx?ID=" + Request.QueryString["Id"].ToString()), true);
            }
            else
            {
                Response.Redirect("SemesterSubjectMaster.aspx", true);
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
            lblerrorddlbatch.Text = "";
            if (IsValidForm())
            {
                BreadCrumb1.Render();
                EConnectContext context1 = new EConnectContext();
                User objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                Int32 BatchId = Convert.ToInt32(ddlBatch.SelectedValue.Trim());
           
                Int32 Number = Convert.ToInt32(ddlSemester.SelectedItem.Text);
                Int32 semno = Number - 1;
                  if (ddlSemester.SelectedItem.Text == "Select")
                  {
                     
                      strMessage = "Semester No. is required";
                      lblsemerror.Text = "Semester No. is required";
                      ddlSemester.Focus();
                      return;
                  }
                  
                Int32 SubId = Convert.ToInt32(ddlSubjects.SelectedValue.Trim());
                Int32 credits = Convert.ToInt32(txtCredits.Text);
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    if (credits == 0 || credits <= 0)
                    {
                        trcredits.Visible = true;
                        strMessage = "No. of Credits can not be zero or negative";
                        lblerrorcredits.Text = "No. of Credits can not be zero or negative";
                        txtCredits.Focus();
                        return;
                    }
                    else
                    {
                        trcredits.Visible = false;
                    }
                     EConnect.NIELIT.SemesterSubjectMaster objSemSub= new EConnect.NIELIT.SemesterSubjectMaster();                  
                 
                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        if (context.SemesterSubjectMaster.Any(s => s.BatchId == BatchId && s.SubId == SubId && s.SemNo == Number))
                        {                            
                            throw new Exception("Subject already exists for the selected Semester.");                            
                        }

                        var application = (from a in context.SemesterSubjectMaster

                                           where a.SemNo == Number && a.BatchId == BatchId && a.SubId == SubId
                                           select new
                                           {
                                               ID = a.Id,
                                               BatchId = a.BatchId

                                           }).FirstOrDefault();

                        if (application != null)
                        {
                            ShowAlert("Record already exist for this semester No. Please select another semester No.");
                            return;
                        }
                        if (Number > 1)
                        {
                            var applicationcheck = (from a in context.SemesterSubjectMaster

                                                    where a.SemNo == semno && a.BatchId == BatchId 
                                                    select new
                                                    {
                                                        ID = a.Id,
                                                        BatchId = a.BatchId

                                                    }).FirstOrDefault();

                            if (applicationcheck == null)
                            {
                                ShowAlert("Record not exist for previous semester No. Please select Previous semester No.");
                                return;
                            }
                        }
                        
                        objSemSub.BatchId = Convert.ToInt32(ddlBatch.SelectedValue.Trim());
                        objSemSub.SubId = Convert.ToInt32(ddlSubjects.SelectedValue.Trim());
                        objSemSub.SemNo = Convert.ToInt32(ddlSemester.SelectedItem.Text);
                        objSemSub.NoOfCredits = Convert.ToInt32(txtCredits.Text);
                        objSemSub.enterBy = Convert.ToInt32(Session["UserID"]);
                        objSemSub.enterDate = DateTime.Now;
                        context.SemesterSubjectMaster.Add(objSemSub);
                        context.SaveChanges();
                        strMessage = "New record saved.";

                    }
                    else
                    {
                        NielitCentreBatch objBatchCentre = new NielitCentreBatch();
                        objBatchCentre = context.NielitCentreBatchs.Find(Convert.ToInt32(Request.QueryString["key"]));

                        int id = -1;
                        if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                        {
                            id = int.Parse(Request.QueryString["Key"].ToString());
                        }
                        DataTable dt = CheckCountForUpdateSemesterSubject(id);
                        int count = -1;
                        if (dt.Rows.Count > 0)
                        {
                            count = int.Parse(dt.Rows[0]["CountOfBatches"].ToString());
                        }

                        var application = (from a in context.SemesterSubjectMaster
                                           join r in context.StudentSemesterAcademicDetails on a.Id equals r.SemSubId
                                           where a.SemNo == Number && a.BatchId == BatchId
                                           select new
                                           {
                                               ID = a.Id,
                                               BatchId = a.BatchId

                                           }).FirstOrDefault();

                        if (application != null)
                        {
                            ShowAlert("Updation is not allowed if semester subject details is used in result details.");
                            return;
                        }

                        if (count > 0)
                        {
                            int val = UpdateSemesterSubjectRecord(id);
                            if (val == 1 || val == -1)
                            {
                                strMessage = "Record updated.";
                            }
                            else
                            {
                                strMessage = "Record not updated.";
                            }
                        }
                        else
                        {
                            strMessage = "This record can't be modified as students are already registered in this batch.";
                        }

                    }
                }
                Response.Redirect("SemesterSubjectMaster.aspx?msg=" + strMessage, true);
            }

        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void ApllyFilter(object sender, EventArgs e)
    {
        try
        {
            // BindGridViewOnFilter();
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
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
            ddlbatchname.SelectedValue = "0";
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
            using (NIELITMISContext context = new NIELITMISContext())
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to delete the records.", true);
                    return;
                }
                NielitCentreBatch Batchcenter = context.NielitCentreBatchs.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                context.NielitCentreBatchs.Remove(Batchcenter);
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
                string href = hl.NavigateUrl;
                if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                {
                    href += "&Id=" + Request.QueryString["Id"].ToString();
                }
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = hl.NavigateUrl;
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = hl.NavigateUrl;
                //HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                //h4.NavigateUrl = hl.NavigateUrl;

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
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
        Int32 loginUserNo = 0, UserTypeId = 0, NielitCentreIdSearch = 0, NielitCentrelinkedToCentreId = 0;
        NIELITMISContext context = new NIELITMISContext();
        try
        {
            loginUserNo = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            UserTypeId = Convert.ToInt32(HttpContext.Current.Session["UserTypeId"]);
            User objUser;
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                if (UserTypeId == 10)
                {
                    var intituteslinkedToCentre = context.NielitCentres.Find(loginUser.UserRefNumber);
                    Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                    NielitCentreIdSearch = NielitCentreId;
                    NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                    if (NielitCentrelinkedToCentreId != 0)
                    {
                        NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                        NielitCentreIdSearch = NielitCentreId;
                    }
                    else
                    {
                        NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                        Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                        NielitCentreIdSearch = NielitCentreId;
                    }
                }
            }
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            EConnectContext context2 = new EConnectContext();

            var Batchcode = from s in context.NielitCentreBatchs
                            join i in context.SemesterSubjectMaster
                            on s.ID equals i.BatchId
                            where s.centreID == NielitCentreIdSearch && s.IsVerified == true
                            select new { Name = s.BatchCode };
            Batchcode = Batchcode.Distinct();

            if (!String.IsNullOrEmpty(searchString))
            {
                Batchcode = Batchcode.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            Batchcode = Batchcode.OrderBy(s => s.Name);

            // BatchName = BatchName.Union(Batchcode).Take(count);
            foreach (var c in Batchcode)
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
        Response.Redirect("SemesterSubjectMaster.aspx", true);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("SemesterSubjectMaster.aspx", true);
    }

    protected void FillBatchFilter()
    {


        try
        {
            Int64 batchidfilter = Convert.ToInt32(ddlbatchname.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ddlbatchname.Items.Clear();
                ListItem lst = new ListItem("--Select--", "0");

                var batchfilterdata =  from p in context.SemesterMaster
                                join c in context.NielitCourseDurations on p.CourseId equals c.ID
                                join s in context.NielitCentreCourses on c.courseID equals s.ID
                                join b in context.NielitCentreBatchs on c.ID equals b.CourseDurationID
                                join m in context.SemesterSubjectMaster   on b.ID equals m.BatchId
                                where b.centreID == entityID
                                //   orderby (p.Result)
                                select new { ValueField = b.ID, TextField = s.Name + " (" + b.BatchCode + ")" };
                                                                                                            
                batchfilterdata = batchfilterdata.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, batchfilterdata.Distinct(), lst);
                btnSave.Visible = true;
                btnCancel.Visible = true;
                // lblMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }

    public DataTable GetBatchCodesForSemesterSubjectMaster()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetBatchCodesForSemesterSubjectMaster", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pCentreID", SqlDbType.BigInt);
                    cmd.Parameters["@pCentreID"].Value = Convert.ToInt64(Session["EntityID"]);
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    public DataTable GetSemesterSubjectMasterDataForUpdate(int id)
    {

        Int64 coursenameid = Convert.ToInt64(ddlBatch.SelectedValue);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSemesterSubjectMasterDataForUpdate", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int));
                    cmd.Parameters["@Id"].Value = id;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    public int SaveRecord()
    {

        int rVal = -1;
        //Int64 coursenameid = Convert.ToInt64(ddlCourseName.SelectedValue);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SaveSemesterSubjectInfo", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SubId", SqlDbType.BigInt);
                    cmd.Parameters["@SubId"].Value = int.Parse(ddlSubjects.SelectedValue);// Convert.ToInt64(Session["EntityID"]);
                    cmd.Parameters.Add("@batchId", SqlDbType.BigInt);
                    cmd.Parameters["@batchId"].Value = int.Parse(ddlBatch.SelectedValue);
                    cmd.Parameters.Add("@Semno", SqlDbType.BigInt);
                    cmd.Parameters["@Semno"].Value = int.Parse(ddlSemester.Text);
                    cmd.Parameters.Add("@NoOfCredits", SqlDbType.BigInt);
                    cmd.Parameters["@NoOfCredits"].Value = int.Parse(txtCredits.Text);

                    cmd.Parameters.Add("@enterBy", SqlDbType.BigInt);
                    cmd.Parameters["@enterBy"].Value = Convert.ToInt32(Session["UserID"]);

                    cmd.Parameters.Add("@enterDate", SqlDbType.Date);
                    cmd.Parameters["@enterDate"].Value = DateTime.Now;


                    cmd.Parameters.Add("@Exists", SqlDbType.Int);
                    cmd.Parameters["@Exists"].Direction = ParameterDirection.ReturnValue;
                    con.Open();
                    rVal = cmd.ExecuteNonQuery();
                    rVal = Convert.ToInt32(cmd.Parameters["@Exists"].Value);

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return rVal;
    }

    public int UpdateSemesterSubjectRecord(int id)
    {

        int rVal = -1;

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("UpdateSemesterSubjectRecord", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.BigInt);
                    cmd.Parameters["@Id"].Value = id;// Convert.ToInt64(Session["EntityID"]);

                    cmd.Parameters.Add("@NoOfCredits", SqlDbType.Int);
                    cmd.Parameters["@NoOfCredits"].Value = int.Parse(txtCredits.Text);

                    cmd.Parameters.Add("@enterBy", SqlDbType.BigInt);
                    cmd.Parameters["@enterBy"].Value = Convert.ToInt32(Session["UserID"]);

                    cmd.Parameters.Add("@enterDate", SqlDbType.Date);
                    cmd.Parameters["@enterDate"].Value = DateTime.Now;
                    con.Open();
                    rVal = cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return rVal;
    }
    public DataTable CheckCountForUpdateSemesterSubject(int id)
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("CheckCountForUpdateSemesterSubject", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.BigInt);
                    cmd.Parameters["@Id"].Value = id;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    public DataTable GetSemesterSubjectMasterDataForGrid()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSemesterSubjectMasterDataForGrid", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pCentreID", SqlDbType.BigInt);
                    cmd.Parameters["@pCentreID"].Value = Convert.ToInt64(Session["EntityID"]);
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    public DataTable GetSemester()
    {
        Int64 batchid = Convert.ToInt64(ddlBatch.SelectedValue);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSemester", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@batchid", SqlDbType.BigInt);
                    cmd.Parameters["@batchid"].Value = batchid;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                con.Close();
            }
        }
        return myDt;
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlSemester.Items.Clear();
            txtCredits.Text = "";
            using (NIELITMISContext context = new NIELITMISContext())
            {
               // ListItem lst = new ListItem("--All--", "0");
                using (DataTable dt = GetSemester())
                {
                    if (dt.Rows.Count > 0)
                    {                       
                        int i = Convert.ToInt32(dt.Rows[0]["NoOfSems"]);
                        int n = i;
                        for (i = 0; i <= n; i++)
                        {
                            if (i == 0)
                            {
                                ddlSemester.Items.Add(new ListItem("Select", "0"));
                            }
                            else
                            {
                                ddlSemester.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                ddlSemester.DataBind();
                            }
                        }                                     
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlSemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblsemerror.Text = "";
    }
}