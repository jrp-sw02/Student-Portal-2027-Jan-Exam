using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;

public partial class ChangeAppStatus : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {       
        lblError.Visible = false;
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
                if (!String.IsNullOrEmpty(Request.QueryString["courseID"]))
                {
                    ucSearchBar.AutoCompleteContextKey = Request.QueryString["courseID"] + "," + Request.QueryString["status"];
                    UpdatebreadCrumb();
                    ShowEditMode();
                }
                else
                {
                    AddBreadCrumb();
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";

                    BindGridView();
                    ucSearchBar.Visible = false;
                    btnMode.Visible = false;
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
    protected void AddBreadCrumb()
    {
        try
        {
            DateTime DateFrom;
            DateTime DateTo;
            if (!string.IsNullOrEmpty(Request.QueryString["StatusFromDate"]) && !string.IsNullOrEmpty(Request.QueryString["StatusToDate"]))
            {
                if (IsDate(Request.QueryString["StatusFromDate"].ToString()) && IsDate(Request.QueryString["StatusToDate"].ToString()))
                {
                    DateFrom = Convert.ToDateTime(Request.QueryString["StatusFromDate"]).Date;
                    DateTo = Convert.ToDateTime(Request.QueryString["StatusToDate"]).Date;
                    txtDateFrom.Text = DateFrom.ToString("dd-MMM-yyyy");
                    txtToDate.Text = DateTo.ToString("dd-MMM-yyyy");
                    BreadCrumb1.RemoveLastBreadCrumbItem();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Change Application Status: From " + DateFrom.ToString("dd-MMM-yyyy") + " To " + DateTo.ToString("dd-MMM-yyyy"), "HO/ChangeAppStatus.aspx?StatusFromDate=" + DateFrom + "&StatusToDate=" + DateTo, ""));
                }
            }
            else if (!string.IsNullOrEmpty(txtDateFrom.Text) && !string.IsNullOrEmpty(txtToDate.Text))
            {
                DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                DateTo = Convert.ToDateTime(txtToDate.Text);
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Change Application Status: From " + DateFrom.ToString("dd-MMM-yyyy") + " To " + DateTo.ToString("dd-MMM-yyyy"), "HO/ChangeAppStatus.aspx?StatusFromDate=" + DateFrom + "&StatusToDate=" + DateTo, ""));
            }
            else
            {
                BreadCrumb1.RemoveLastBreadCrumbItem();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Change Application Status", "HO/ChangeAppStatus.aspx", ""));
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
            ucSearchBar.Visible = true;
            lblHeading.Text = "Change Application Status";
            this.gvMain1.Columns[5].HeaderText = Convert.ToString(Request.QueryString["status"]);
            string Status = Convert.ToString(Request.QueryString["status"]);
            switch (Status)
            {
                case "LockedOn":
                    BtnVerify.Visible = true;
                    BtnRevertVerify.Visible = false;
                    break;
                case "VerifiedOn":
                    BtnRevertVerify.Visible = true;
                    BtnVerify.Visible = false;
                    break;
                case "SyncedOn":
                    BtnVerify.Visible = false;
                    BtnRevertVerify.Visible = false;
                    this.gvMain1.Columns[6].Visible = false;
                    break;
            }
            BindGridViewMain();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void UpdatebreadCrumb()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                DateTime? DateFrom = null;
                if (!string.IsNullOrEmpty(Request.QueryString["StatusFromDate"]))
                    DateFrom = Convert.ToDateTime(Request.QueryString["StatusFromDate"]).Date;
                DateTime? DateTo = null;
                if (!string.IsNullOrEmpty(Request.QueryString["StatusToDate"]))
                    DateTo = Convert.ToDateTime(Request.QueryString["StatusToDate"]).Date;
                string Status = Convert.ToString(Request.QueryString["status"]);
                Int32 courseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                string courseName = "";
                if (courseID != 0)
                    courseName = context.Courses.Find(courseID).Name;
                if (DateFrom != null && DateTo != null)
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(courseName, "HO/ChangeAppStatus.aspx?CourseID=" + Request.QueryString["CourseID"].ToString() + "&status=" + Request.QueryString["status"].ToString() + "&StatusFromDate=" + DateFrom.Value + "&StatusToDate=" + DateTo.Value, ""));
                else
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(courseName, "HO/ChangeAppStatus.aspx?CourseID=" + Request.QueryString["CourseID"].ToString() + "&status=" + Request.QueryString["status"].ToString(), ""));

            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void BindGridView()
    {
        try
        {
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            string date = txtToDate.Text.Trim();
            DateTime? DateFrom = null;
            if (!string.IsNullOrEmpty(txtDateFrom.Text.Trim()))
                DateFrom = Convert.ToDateTime(txtDateFrom.Text).Date;
            DateTime? DateTo = null;
            if (!string.IsNullOrEmpty(date))
                DateTo = Convert.ToDateTime(date).Date;
            string strDateFrom = txtDateFrom.Text.Trim();
            string strDateTo = txtToDate.Text.Trim();
            Int32[] RegStatus = { 1, 2 };
            var candidate = (from a in context.Candidates
                             join g in context.RegistrationDetails on a.ID equals g.CandidateID
                             where RegStatus.Contains(g.RegistrationStatusID.Value)
                             group a by new { g.CourseID, g.Course.Name } into s
                             select new
                             {
                                 CourseID = s.Key.CourseID,
                                 CourseName = s.Key.Name,
                                 LockedCount = s.Count(q => q.IsLocked == true && q.IsVerified == false &&
                                       (DateFrom != null && DateTo != null ? System.Data.Entity.DbFunctions.TruncateTime(q.LockedOn) >= DateFrom.Value && System.Data.Entity.DbFunctions.TruncateTime(q.LockedOn) <= DateTo.Value : 1 == 1)),
                                 VerifiedCount = s.Count(q => q.IsVerified == true && q.IsSynced == false &&
                                       (DateFrom != null && DateTo != null ? System.Data.Entity.DbFunctions.TruncateTime(q.VerifiedOn) >= DateFrom.Value && System.Data.Entity.DbFunctions.TruncateTime(q.VerifiedOn) <= DateTo.Value : true)),
                                 SyncedCount = s.Count(q => q.IsSynced == true &&
                                       (DateFrom != null && DateTo != null ? System.Data.Entity.DbFunctions.TruncateTime(q.SyncedOn) >= DateFrom.Value && System.Data.Entity.DbFunctions.TruncateTime(q.SyncedOn) <= DateTo.Value : true)),
                                 StatusDateFrom = !string.IsNullOrEmpty(strDateFrom) ? strDateFrom : string.Empty,
                                 StatusDateTo = !string.IsNullOrEmpty(strDateTo) ? strDateTo : string.Empty
                             });

            if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseHeadOffice))
            {
                var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                candidate = candidate.Where(a => roleCourses.Contains(a.CourseID));
            }
            candidate = candidate.OrderBy(o => o.CourseID);
            PagingBar1.Bind(candidate, ref gvMain);
            uPnlGridSummery.Update();
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
    protected void BindGridViewMain()
    {
        try
        {
            //this is the sample code how to bind the grid control
            context = new EConnectContext();
            string searchString = ucSearchBar.SearchText.Trim().ToUpper();
            //string sortOrder = ViewState["SortOrder"].ToString();
            //string sortField = ViewState["SortField"].ToString();
            //string date = txtToDate.Text.Trim();
            DateTime? DateFrom = null;
            if (!string.IsNullOrEmpty(Request.QueryString["StatusFromDate"]))
                DateFrom = Convert.ToDateTime(Request.QueryString["StatusFromDate"]);
            DateTime? DateTo = null;
            if (!string.IsNullOrEmpty(Request.QueryString["StatusToDate"]))
                DateTo = Convert.ToDateTime(Request.QueryString["StatusToDate"]);
            string strDateFrom = Request.QueryString["StatusFromDate"];
            string strDateTo = Request.QueryString["StatusToDate"];
            string Status = Convert.ToString(Request.QueryString["status"]);
            Int32 courseID = Convert.ToInt32(Request.QueryString["courseID"]);
            Int32[] RegStatus = { 1, 2 };
            var candidate = (from a in context.Candidates
                             join g in context.RegistrationDetails on a.ID equals g.CandidateID
                             where (Status == "LockedOn" ? (a.IsLocked == true && a.IsVerified == false) :
                                               Status == "VerifiedOn" ? (a.IsVerified == true && a.IsSynced == false) :
                                               Status == "SyncedOn" ? (a.IsSynced == true) : true) && g.CourseID == courseID &&
                                               RegStatus.Contains(g.RegistrationStatusID.Value)
                             select new
                             {
                                 CourseID = g.CourseID,
                                 CourseName = g.Course.Name,
                                 CandidateID = a.ID,
                                 CandidateName = a.Name,
                                 FatherName = a.GuardianName == null ? a.FatherName : a.GuardianName,
                                 RegNo = g.RegistrationNo,
                                 StatusHead = Status,
                                 StatusDateFrom = !string.IsNullOrEmpty(strDateFrom) ? strDateFrom : string.Empty,
                                 StatusDateTo = !string.IsNullOrEmpty(strDateTo) ? strDateTo : string.Empty,
                                 StatusField = (Status == "LockedOn" ? a.LockedOn :
                                               Status == "VerifiedOn" ? a.VerifiedOn :
                                               a.SyncedOn)
                             });

            if (DateFrom != null && DateTo != null)
            {
                candidate = candidate.Where(p => (System.Data.Entity.DbFunctions.TruncateTime(p.StatusField.Value) >= DateFrom && System.Data.Entity.DbFunctions.TruncateTime(p.StatusField.Value) <= DateTo));

            }
            if (!String.IsNullOrEmpty(searchString))
            {
                if (IsNumeric(searchString))
                {
                    long SearchStr = Convert.ToInt64(searchString);
                    candidate = candidate.Where(s => s.RegNo == SearchStr);
                }
                else
                    candidate = candidate.Where(s => s.FatherName.ToUpper().Contains(searchString) || s.CandidateName.ToUpper().Contains(searchString));
            }
            if (candidate.Count() > 0)
            {
                candidate = candidate.OrderBy(o => o.CourseID);
                PagingBar2.Bind(candidate, ref gvMain1);
                uPnlGrid.Update();
            }
            else
            {
                BtnVerify.Visible = false;
                BtnRevertVerify.Visible = false;
                lblError.Visible = true;
                lblError.Text = "Sorry ! No Such Record Found .";
                gvMain1.Visible = false;
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
    protected void PageIndexChanged1(Int32 NewPageIndex)
    {
        try
        {
            gvMain1.PageIndex = PagingBar2.CurrentPageIndex;
            BindGridViewMain();
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
            Response.Redirect("ChangeAppStatus.aspx", true);
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            PagingBar2.CurrentPageIndex = 0;
            gvMain1.PageIndex = PagingBar2.CurrentPageIndex;
            UpdatebreadCrumb();
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
            PagingBar2.CurrentPageIndex = 0;
            gvMain1.PageIndex = PagingBar2.CurrentPageIndex;
            UpdatebreadCrumb();
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
            Response.Redirect("ChangeAppStatus.aspx?msg=" + strMessage);
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

            AddBreadCrumb();
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

            txtDateFrom.Text = "";
            txtToDate.Text = "";
            AddBreadCrumb();
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
            this.gvMain1.Columns[4].HeaderText = Convert.ToString(Request.QueryString["status"]);
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                //HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                //hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                //Image imgAction = (Image)e.Row.FindControl("imgAction");
                //imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();

                //CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                //imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();   //[e.Row.RowIndex].Value.ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
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
            Int32 courseid = 0;
            String Status = "";
            Int32[] RegStatus = { 1, 2 };
            String[] keys = contextKey.Split(',');
            List<String> items = new List<String>();
            if (!String.IsNullOrEmpty(keys[0]))
                courseid = Convert.ToInt32(keys[0]);
            if (!String.IsNullOrEmpty(keys[1]))
                Status = Convert.ToString(keys[1]);
            string searchString = prefixText.Trim().ToUpper();
            var applications = from g in context.RegistrationDetails
                               join a in context.Candidates
                                   on g.CandidateID equals a.ID
                               where (Status == "LockedOn" ? (a.IsLocked == true && a.IsVerified == false) :
                                              Status == "VerifiedOn" ? (a.IsVerified == true && a.IsSynced == false) :
                                              Status == "SyncedOn" ? (a.IsSynced == true) : true) && g.CourseID == courseid &&
                                              RegStatus.Contains(g.RegistrationStatusID.Value)
                               select new { Name = a.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                applications = applications.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            applications = applications.OrderBy(s => s.Name);

            var applications2 = from g in context.RegistrationDetails
                                join a in context.Candidates
                                    on g.CandidateID equals a.ID
                                where (Status == "LockedOn" ? (a.IsLocked == true && a.IsVerified == false) :
                                               Status == "VerifiedOn" ? (a.IsVerified == true && a.IsSynced == false) :
                                               Status == "SyncedOn" ? (a.IsSynced == true) : true) && g.CourseID == courseid &&
                                               RegStatus.Contains(g.RegistrationStatusID.Value)
                                select new { Name = SqlFunctions.StringConvert((double)g.RegistrationNo) };
            if (!String.IsNullOrEmpty(searchString))
            {
                applications2 = applications2.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            applications = applications.Union(applications2).Take(count);
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ChangeAppStatus.aspx", true);
    }
    protected void ImgBtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            BindGridView();
        }
        catch (Exception)
        { }
    }
    protected void gvMain1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar2.CurrentPageSize * PagingBar2.CurrentPageIndex)).ToString();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BtnVerify_Click(object sender, EventArgs e)
    {
        try
        {

            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int16 verifiedCount = 0;
                    for (int i = 0; i < gvMain1.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gvMain1.Rows[i].FindControl("chk");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                Int64 candidateID = Convert.ToInt64(gvMain1.DataKeys[i].Values[0]);

                                if (candidateID != 0)
                                {
                                    verifiedCount += 1;
                                    var candidate = (from a in context.Candidates
                                                     where a.ID == candidateID
                                                     select a).FirstOrDefault();
                                    candidate.IsVerified = true;
                                    candidate.VerifiedOn = DateTime.Now;
                                    candidate.VerifiedBy = Convert.ToInt32(Session["UserID"]);
                                    context.Entry(candidate).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                    scope.Complete();
                    ShowAlert(verifiedCount.ToString() + " candidate details has been verified successfully", true);
                };
            };
            UpdatebreadCrumb();
            BindGridViewMain();

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BtnRevertVerify_Click(object sender, EventArgs e)
    {
        try
        {

            using (TransactionScope scope = new TransactionScope())
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int16 verifiedCount = 0;
                    for (int i = 0; i < gvMain1.Rows.Count; i++)
                    {
                        CheckBox cbx = (CheckBox)gvMain1.Rows[i].FindControl("chk");
                        if (cbx != null)
                        {
                            if (cbx.Checked)
                            {
                                Int64 candidateID = Convert.ToInt64(gvMain1.DataKeys[i].Values[0]);

                                if (candidateID != 0)
                                {
                                    verifiedCount += 1;
                                    var candidate = (from a in context.Candidates
                                                     where a.ID == candidateID
                                                     select a).FirstOrDefault();
                                    candidate.IsVerified = false;
                                    candidate.VerifiedOn = null;
                                    candidate.VerifiedBy = null;
                                    context.Entry(candidate).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                    scope.Complete();
                    ShowAlert(verifiedCount.ToString() + "  candidate details has been marked as not verified successfully.", true);
                };
            };
            UpdatebreadCrumb();
            BindGridViewMain();


        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }
    protected void btnSyncApplData_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            using (EConnectContext context = new EConnectContext())
            {
                Int64 count = Convert.ToInt64(context.Database.ExecuteSqlCommand("exec sync_candidate_Data"));
                if (count > 0)
                {
                    ShowAlert("All verified records have been synced successfully.", true);
                }
            };
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}