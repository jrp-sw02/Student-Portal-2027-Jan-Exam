using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data;
using System.Configuration;
using System.Web;
using System.Data.SqlClient;
using System.Collections;

public partial class HO_SemesterFeeMaster : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int64 NielitCentrelinkedToCentreId = 0;
    Int32 NielitCentreIdFilter = 0, NonAfflAfflInstID = 0;
    Int32 UserRefNumber = 0;
    Int32 UserTypeid = 0;
    Int64 courseid = 0;

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
            UserTypeid = Convert.ToInt32(Session["UserTypeId"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
          

            if (!Page.IsPostBack)
            {
                User objUsers;
                using (EConnectContext context = new EConnectContext())
                {
                    objUsers = new EConnect.URM.User();

                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);

                    using (NIELITMISContext context1 = new NIELITMISContext())
                    {

                        if (UserTypeid == 10)
                        {
                            var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                            NielitCentreIdFilter = NielitCentreId;
                            HNonAfflAfflInst.Value = "99";
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            if (NielitCentrelinkedToCentreId != 0)
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                                txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                NielitCentreIdFilter = NelitCentreLinkId;
                                ///Login for all  todayyy
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();

                            }
                            else
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                                txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                NielitCentreIdFilter = NelitCentreLinkId;
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();

                            }
                        }
                        else if (UserTypeid == 11)
                        {
                            var intituteslinkedToCentre = context1.NonAffInstitutes.Find(loginUser.UserRefNumber); //HNonAfflAfflInst
                            NonAfflAfflInstID = Convert.ToInt32(intituteslinkedToCentre.ID);
                            HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                NielitCentreIdFilter = NelitCentreLinkId;
                            }

                        }
                        else if (UserTypeid == 4)
                        {
                            var intituteslinkedToCentre = context1.AffInstitutes.Find(loginUser.UserRefNumber);
                            NonAfflAfflInstID = Convert.ToInt32(intituteslinkedToCentre.ID);
                            HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                NielitCentreIdFilter = NelitCentreLinkId;
                            }

                        }
                    }
                }

                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {

                    BindListData();
                    ShowEditMode();
                }
                else
                {
                    BindListData();
                    BindCourse();
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";

                    RdoAffInstOrNonAffInst.Enabled = false;

                    if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Semester Fee", "HO/SemesterFeeMaster.aspx?BatchID=" + Request.QueryString["BatchId"].ToString() + "&feeTypeID=" + Request.QueryString["feeTypeID"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Semester Fee", "HO/SemesterFeeMaster.aspx", ""));
                    }

                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
            else
            {

            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void RdoAffInstOrNonAffInst_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divGrid.Visible = false;
            ddlCourse.ClearSelection();
            ddlbatchSession.ClearSelection();
            ddlCourse.Items.Clear();
            ddlbatchSession.Items.Clear();
            BindCourse();
            ddlsemester.ClearSelection();
            ddlsemester.Items.Clear();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }

    }

    protected void BindCourse()
    {
        Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);

        try
        {
            // Int32 subid = Convert.ToInt32(ddlSubjects.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ddlCourse.Items.Clear();
                ListItem lst = new ListItem("--Select--", "0");
                var semestercourse = from p in context.SemesterMaster
                                     join c in context.NielitCourseDurations on p.CourseId equals c.ID
                                     join s in context.NielitCentreCourses on c.courseID equals s.ID
                                     join b in context.NielitCentreBatchs on c.ID equals b.CourseDurationID
                                     where b.centreID == nielitcentreid
                                     select new { ValueField = c.ID, TextField = s.Name + " (" + s.Code + ")" + " (" + c.courseDurationDays + "Days" + ")" + " (" + c.courseDurationHrs + "Hours" + ")" };
                semestercourse = semestercourse.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, semestercourse.Distinct(), lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void BindListData()
    {

        try
        {
            ListItem lst = new ListItem("--Select--", "0");
            using (NIELITMISContext context = new NIELITMISContext())
            {

                var CourseName = from p in context.NielitCentreCourses
                                 join k in context.NielitCourseDurations on p.ID equals k.courseID
                                 join d in context.NielitCentreBatchs on k.ID equals d.CourseDurationID
                                 join s in context.SemesterFeeMaster on d.ID equals s.batchID
                                 where p.IsVerified == true && k.isVerified == true && d.centreID == NielitCentreIdFilter
                                 orderby (d.Name)
                                 select new { ValueField = k.ID, TextField = p.Name + " (" + p.Code + ")" + " (" + k.courseDurationDays + "Days" + ")" + " (" + k.courseDurationHrs + "Hours" + ")" };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, CourseName.Distinct(), lst);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);

        Int64 Course = 0;

        ddlbatchSession.ClearSelection();
        ddlbatchSession.Items.Clear();
        ddlsemester.ClearSelection();
        ddlbatchSession.Items.Clear();
        gvMain.Visible = false;

        try
        {

            Course = Convert.ToInt64(ddlCourse.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select--", "0");
                var Batch = from s in context.NielitCentreBatchs
                            where s.IsVerified == true && s.centreID == nielitcentreid
                                    && s.CourseDurationID == Course // && (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
                            orderby (s.Name)
                            select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchSession, Batch, lst);
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
            
            // mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Semester Details";
            //  tblNavLinks.Visible = true;

            EConnectContext context1 = new EConnectContext();
            User objUser = new EConnect.URM.User();
            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();

            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int64 Id = Convert.ToInt32(Request.QueryString["Key"]);
                ddlCourse.Enabled = false;
                ddlbatchSession.Enabled = false;

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

    protected void ShowRecord(object sender, EventArgs e)
    {
        divGrid.Visible = true;
        Int64 SemId = 0;
        if (ddlsemester.SelectedItem.Text == "Select")
        {

            strMessage = "Semester No. is required";
            lblsemerror.Text = "Semester No. is required";
            ddlsemester.Focus();
            return;
        }
        else
        {
            SemId = Convert.ToInt32(ddlsemester.SelectedItem.Text);
        }
        BindGridView();
        btnCancel.Visible = false;
        btnSave.Visible = false;
        lblMessage.Text = "";
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
                Int64 BatchID = 0, BatchIDFilter = 0, Semid = 0;

                if (ddlCourse.SelectedValue != "0")
                    courseid = Convert.ToInt64(ddlCourse.SelectedValue);
                if (ddlbatchSession.SelectedValue != "0")
                    BatchID = Convert.ToInt64(ddlbatchSession.SelectedValue);
                if (ddlsemester.SelectedItem.Text != "0"
                    || ddlsemester.SelectedItem.Text != "" || ddlsemester.SelectedItem.Text != "Select")
                    Semid = Convert.ToInt64(ddlsemester.SelectedItem.Text);
                Int64 semno = Semid - 1;

                if (Semid == 1)
                {
                    var semidcheck = (from s in context.SemesterFeeMaster
                                      where s.batchID == BatchID && s.SemId == 1
                                      select new
                                      {
                                          ID = s.ID
                                      }).FirstOrDefault();

                    if (semidcheck == null)
                    {

                        var CentreBatchs = from s in context.feeTypeMas
                                           // join c in context.NielitCentreBatchFees on s.batch_ID equals c.batchID
                                           // // join f in context.feeTypeMas on c.feeTypeID equals f.ID
                                           //  from p in context.SemesterFeeMaster
                                           //   where s.batch_ID == c.batchID && s.batch_ID == BatchID && SemId == 1 //&& s.InstituteID == NielitCentreLinkid
                                           //orderby s.ID descending
                                           select new
                                           {
                                               ID = s.ID,
                                               feetype = s.feeType,
                                               Amount = "",
                                               effectivefrom = "",
                                               effectiveto = "",
                                           };
                        if (BatchIDFilter != 0)
                        {
                            // PagingBar1.Bind(CentreBatchsFilter, ref gvMain);
                            uPnlGrid.Update();
                            uPnlNavigation.Update();
                            lblError.Visible = false;
                            PagingBar1.Visible = true;
                            gvMain.Visible = true;
                            lblsemsterfeelist.Visible = true;
                            divNavigation.Visible = true;
                            divGrid.Visible = true;
                            if (gvMain.Rows.Count <= 0)
                            {
                                if (BatchIDFilter == 0)
                                {
                                    lblError.Visible = false;
                                    lblError.Text = "Please enter master fee details for Previous semester, then consequent semesters may be allowed.";
                                    lblsemsterfeelist.Visible = false;
                                }
                                else
                                {
                                    lblError.Text = "Please enter master fee details for Previous semester, then consequent semesters may be allowed.";
                                    lblError.Visible = true;
                                    lblsemsterfeelist.Visible = false;
                                }
                                lblError.Visible = true;
                                gvMain.Visible = false;
                                lblsemsterfeelist.Visible = false;
                                PagingBar1.Visible = false;
                            }
                        }

                        else
                        {
                            PagingBar1.Bind(CentreBatchs, ref gvMain);
                            uPnlGrid.Update();
                            uPnlNavigation.Update();
                            lblError.Visible = false;
                            PagingBar1.Visible = true;
                            gvMain.Visible = true;
                            lblsemsterfeelist.Visible = true;
                            divGrid.Visible = true;
                            if (gvMain.Rows.Count <= 0)
                            {
                                if (ddlsemester.SelectedItem.Text == "0")
                                {
                                    lblError.Text = "Please enter master fee details for Previous semester, then consequent semesters may be allowed.";
                                    lblError.Visible = false;
                                    lblsemsterfeelist.Visible = false;
                                }
                                else
                                {
                                    lblError.Text = "Please enter master fee details for Previous semester, then consequent semesters may be allowed.";
                                    lblError.Visible = true;
                                    lblsemsterfeelist.Visible = false;
                                }
                                gvMain.Visible = false;
                                PagingBar1.Visible = true;
                                lblsemsterfeelist.Visible = false;
                            }
                        }

                    }

                    else
                    {
                        var CentreBatchs = from s in context.SemesterFeeMaster
                                           // join c in context.NielitCentreBatchFees on s.batch_ID equals c.batchID
                                           // from f in context.feeTypeMas
                                           join f in context.feeTypeMas on s.feeTypeID equals f.ID
                                           //  from p in context.NIELITStudentFeePaids.Where(x => s.ID == x.studentID && x.feeTypeID == c.feeTypeID).DefaultIfEmpty()
                                           where s.batchID == BatchID && s.courseid == courseid && s.SemId == 1 //&& s.InstituteID == NielitCentreLinkid

                                           select new
                                           {
                                               ID = f.ID,
                                               feetype = f.feeType,
                                               Amount = (int?)s.feeAmount,
                                               effectivefrom = (DateTime?)s.effectiveFromDate,
                                               effectiveto = (DateTime?)s.effectiveToDate,
                                           };
                        if (BatchIDFilter != 0)
                        {
                            // PagingBar1.Bind(CentreBatchsFilter, ref gvMain);
                            uPnlGrid.Update();
                            uPnlNavigation.Update();
                            lblError.Visible = false;
                            PagingBar1.Visible = true;
                            gvMain.Visible = true;
                            lblsemsterfeelist.Visible = true;
                            divNavigation.Visible = true;
                            divGrid.Visible = true;
                            if (gvMain.Rows.Count <= 0)
                            {
                                if (BatchIDFilter == 0)
                                {
                                    lblError.Visible = false;
                                    lblError.Text = "Please enter master fee details for Previous semester, then consequent semesters may be allowed.";
                                    lblsemsterfeelist.Visible = false;
                                }
                                else
                                {
                                    lblError.Text = "Please enter master fee details for Previous semester, then consequent semesters may be allowed.";
                                    lblError.Visible = true;
                                    lblsemsterfeelist.Visible = false;
                                }
                                lblError.Visible = true;
                                gvMain.Visible = false;
                                PagingBar1.Visible = false;
                                lblsemsterfeelist.Visible = false;
                            }
                        }
                        else
                        {
                            PagingBar1.Bind(CentreBatchs, ref gvMain);
                            uPnlGrid.Update();
                            uPnlNavigation.Update();
                            lblError.Visible = false;
                            PagingBar1.Visible = true;
                            gvMain.Visible = true;
                            lblsemsterfeelist.Visible = true;
                            divGrid.Visible = true;
                            if (gvMain.Rows.Count <= 0)
                            {
                                if (ddlsemester.SelectedItem.Text == "0")
                                {
                                    lblError.Text = "Please enter master fee details for Previous semester, then consequent semesters may be allowed.";
                                    lblError.Visible = false;
                                    lblsemsterfeelist.Visible = false;
                                }
                                else
                                {
                                    lblError.Text = "Please enter master fee details for Previous semester, then consequent semesters may be allowed.";
                                    lblError.Visible = true;
                                    lblsemsterfeelist.Visible = false;
                                }
                                gvMain.Visible = false;
                                PagingBar1.Visible = false;
                                lblsemsterfeelist.Visible = false;
                            }
                        }

                    }

                }
                else
                {
                    var semidcheck = (from s in context.SemesterFeeMaster
                                      where s.batchID == BatchID && s.SemId == Semid
                                      select new
                                      {
                                          ID = s.ID
                                      }).FirstOrDefault();

                    if (semidcheck != null)
                    {
                        var CentreBatchs = from s in context.SemesterFeeMaster
                                           // join c in context.NielitCentreBatchFees on s.batch_ID equals c.batchID
                                           // from f in context.feeTypeMas
                                           join f in context.feeTypeMas on s.feeTypeID equals f.ID
                                           //  from p in context.NIELITStudentFeePaids.Where(x => s.ID == x.studentID && x.feeTypeID == c.feeTypeID).DefaultIfEmpty()
                                           where s.batchID == BatchID && s.courseid == courseid && s.SemId == Semid //&& s.InstituteID == NielitCentreLinkid

                                           select new
                                           {
                                               ID = f.ID,
                                               feetype = f.feeType,
                                               Amount = (int?)s.feeAmount,
                                               effectivefrom = (DateTime?)s.effectiveFromDate,
                                               effectiveto = (DateTime?)s.effectiveToDate,
                                           };
                        if (BatchIDFilter != 0)
                        {
                            // PagingBar1.Bind(CentreBatchsFilter, ref gvMain);
                            uPnlGrid.Update();
                            uPnlNavigation.Update();
                            lblError.Visible = false;
                            PagingBar1.Visible = true;
                            gvMain.Visible = true;
                            lblsemsterfeelist.Visible = true;
                            divNavigation.Visible = true;
                            divGrid.Visible = true;
                            if (gvMain.Rows.Count <= 0)
                            {
                                if (BatchIDFilter == 0)
                                {
                                    lblError.Visible = false;
                                    lblError.Text = "Please enter master fee details for Previous semester, then consequent semesters may be allowed.";
                                    lblsemsterfeelist.Visible = false;
                                }
                                else
                                {
                                    lblError.Text = "Please enter master fee details for Previous semester, then consequent semesters may be allowed.";
                                    lblError.Visible = true;
                                    lblsemsterfeelist.Visible = false;
                                }
                                lblError.Visible = true;
                                gvMain.Visible = false;
                                PagingBar1.Visible = false;
                                lblsemsterfeelist.Visible = false;
                            }
                        }
                        else
                        {
                            PagingBar1.Bind(CentreBatchs, ref gvMain);
                            uPnlGrid.Update();
                            uPnlNavigation.Update();
                            lblError.Visible = false;
                            PagingBar1.Visible = true;
                            gvMain.Visible = true;
                            lblsemsterfeelist.Visible = true;
                            divGrid.Visible = true;
                            if (gvMain.Rows.Count <= 0)
                            {
                                if (ddlsemester.SelectedItem.Text == "0")
                                {
                                    lblError.Text = "Please enter master fee details for Previous semester, then consequent semesters may be allowed.";
                                    lblError.Visible = false;
                                    lblsemsterfeelist.Visible = false;
                                }
                                else
                                {
                                    lblError.Text = "Please enter master fee details for Previous semester, then consequent semesters may be allowed.";
                                    lblError.Visible = true;
                                    lblsemsterfeelist.Visible = false;
                                }
                                gvMain.Visible = false;
                                PagingBar1.Visible = false;
                                lblsemsterfeelist.Visible = false;
                            }
                        }

                    }
                    else
                    {
                        var CentreBatchs = from s in context.SemesterFeeMaster
                                           // join c in context.NielitCentreBatchFees on s.batch_ID equals c.batchID
                                           // from f in context.feeTypeMas
                                           join f in context.feeTypeMas on s.feeTypeID equals f.ID
                                           //  from p in context.NIELITStudentFeePaids.Where(x => s.ID == x.studentID && x.feeTypeID == c.feeTypeID).DefaultIfEmpty()
                                           where s.batchID == BatchID && s.courseid == courseid && s.SemId == semno //&& s.InstituteID == NielitCentreLinkid

                                           select new
                                           {
                                               ID = f.ID,
                                               feetype = f.feeType,
                                               Amount = (int?)s.feeAmount,
                                               effectivefrom = (DateTime?)s.effectiveFromDate,
                                               effectiveto = (DateTime?)s.effectiveToDate,
                                           };
                        if (BatchIDFilter != 0)
                        {
                            // PagingBar1.Bind(CentreBatchsFilter, ref gvMain);
                            uPnlGrid.Update();
                            uPnlNavigation.Update();
                            lblError.Visible = false;
                            PagingBar1.Visible = true;
                            gvMain.Visible = true;
                            lblsemsterfeelist.Visible = true;
                            divNavigation.Visible = true;
                            divGrid.Visible = true;
                            if (gvMain.Rows.Count <= 0)
                            {
                                if (BatchIDFilter == 0)
                                {
                                    lblError.Visible = false;
                                    lblError.Text = "Please enter master fee details for Previous semester, then consequent semesters may be allowed.";
                                    lblsemsterfeelist.Visible = false;
                                }
                                else
                                {
                                    lblError.Text = "Please enter master fee details for Previous semester, then consequent semesters may be allowed.";
                                    lblError.Visible = true;
                                    lblsemsterfeelist.Visible = false;
                                }
                                lblError.Visible = true;
                                gvMain.Visible = false;
                                PagingBar1.Visible = false;
                                lblsemsterfeelist.Visible = false;
                            }
                        }
                        else
                        {
                            PagingBar1.Bind(CentreBatchs, ref gvMain);
                            uPnlGrid.Update();
                            uPnlNavigation.Update();
                            lblError.Visible = false;
                            PagingBar1.Visible = true;
                            gvMain.Visible = true;
                            lblsemsterfeelist.Visible = true;
                            divGrid.Visible = true;
                            if (gvMain.Rows.Count <= 0)
                            {
                                if (ddlsemester.SelectedItem.Text == "0")
                                {
                                    lblError.Text = "Please enter master fee details for Previous semester, then consequent semesters may be allowed.";
                                    lblError.Visible = false;
                                    lblsemsterfeelist.Visible = false;
                                }
                                else
                                {
                                    lblError.Text = "Please enter master fee details for Previous semester, then consequent semesters may be allowed.";
                                    lblError.Visible = true;
                                    lblsemsterfeelist.Visible = false;
                                }
                                gvMain.Visible = false;
                                PagingBar1.Visible = false;
                                lblsemsterfeelist.Visible = false;
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
    protected void BindGridViewFilter()
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                Int64 CourseName = 0, batchname = 0, SemId = 0;
                if (ddlCourseName.SelectedValue != "0")
                    CourseName = Convert.ToInt64(ddlCourseName.SelectedValue);
                if (ddlbatchname.SelectedValue != "0")
                    batchname = Convert.ToInt64(ddlbatchname.SelectedValue);
                //  if (ddlsem.SelectedItem.Text != "0" || ddlsem.SelectedItem.Text != "")
                //  SemId = Convert.ToInt64(ddlsem.SelectedItem.Text);
                DataTable dt = new DataTable();
                con.Open();
                if (searchString != "")
                {
                    using (dt = GetSemesterFeeFilterForGrid())
                    {                     
                        PagingBar2.Bind(dt, ref gridSemesterFeeType);
                        UpdatePanel1.Update();                        
                        lblError.Visible = false;
                        PagingBar2.Visible = true;
                        gridSemesterFeeType.Visible = true;
                        if (gridSemesterFeeType.Rows.Count <= 0)
                        {
                            lblError.Text = "No record found.";
                            lblError.Visible = true;
                        }
                        lblError.Visible = false;
                        gridSemesterFeeType.Visible = true;
                        PagingBar2.Visible = true;
                    }
                }
                using (dt = GetSemesterFeeMasterDetailsForGrid())
                {
                    PagingBar2.Bind(dt, ref gridSemesterFeeType);
                    UpdatePanel1.Update();
                    lblError.Visible = false;
                    PagingBar2.Visible = true;
                    gridSemesterFeeType.Visible = true;

                    if (gridSemesterFeeType.Rows.Count <= 0)
                    {
                        lblError.Text = "No record found.";
                        lblError.Visible = true;
                    }
                    lblError.Visible = false;
                    gridSemesterFeeType.Visible = true;
                    PagingBar2.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
            con.Close();
            //context1.Dispose();
        }

    }

    protected void BindEditNewModeData()
    {
        try
        {
            User objUser;
            using (EConnectContext context = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                using (NIELITMISContext context1 = new NIELITMISContext())
                {
                    if (UserTypeid == 10)
                    {
                        var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                        Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        if (NielitCentrelinkedToCentreId != 0)
                        {
                            NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            RdoAffInstOrNonAffInst.SelectedValue = "2";
                            // ddlSubcentreName.Enabled = false;
                            ListItem lst = new ListItem("--Select--", "0");
                            var BatchName = from p in context1.NielitCentreBatchs
                                            where p.IsVerified == true &&
                                                //(p.startDate <= System.DateTime.Now && 
                                            (p.endDate >= System.DateTime.Now)
                                            && p.subCentreID == NelitCentreLinkId
                                            orderby (p.Name)
                                            select new { ValueField = p.ID, TextField = p.Name };
                            //  EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                        }
                        else
                        {
                            NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            RdoAffInstOrNonAffInst.SelectedValue = "2";
                            //  ddlSubcentreName.Enabled = false;
                            ListItem lst = new ListItem("--Select--", "0");
                            var BatchName = from p in context1.NielitCentreBatchs
                                            where p.IsVerified == true &&
                                                //(p.startDate <= System.DateTime.Now &&
                                            (p.endDate >= System.DateTime.Now)
                                            && p.centreID == NelitCentreLinkId
                                            orderby (p.Name)
                                            select new { ValueField = p.ID, TextField = p.Name };
                            //  EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                        }
                    }
                    else if (UserTypeid == 11)
                    {
                        var intituteslinkedToCentre = context1.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        Int32 subcentreId = Convert.ToInt32(intituteslinkedToCentre.ID);
                        NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        }
                        //   FillddlSubcentreName();
                        ListItem lst = new ListItem("--Select--", "0");
                        var BatchName = from p in context1.NielitCentreBatchs
                                        where p.IsVerified == true &&
                                            //(p.startDate <= System.DateTime.Now && 
                                        (p.endDate >= System.DateTime.Now)
                                        && p.subCentreID == subcentreId
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.Name };
                        //  EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                    }
                    else if (UserTypeid == 4)
                    {
                        var intituteslinkedToCentre = context1.AffInstitutes.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        Int32 subcentreId = Convert.ToInt32(intituteslinkedToCentre.ID);

                        NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        }
                        //    FillddlSubcentreName();
                        ListItem lst = new ListItem("--Select--", "0");
                        var BatchName = from p in context1.NielitCentreBatchs
                                        where p.IsVerified == true &&
                                            //(p.startDate <= System.DateTime.Now && 
                                        (p.endDate >= System.DateTime.Now)
                                        && p.subCentreID == subcentreId
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.Name };
                        // EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable GetSemesterFeeFilterForGrid()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        string searchString = ucSearchBar.SearchText.Trim().ToUpper();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSemesterFeeSearch", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@batchcode", SqlDbType.VarChar);
                    cmd.Parameters["@batchcode"].Value = searchString;

                    cmd.Parameters.Add("@batchName", SqlDbType.VarChar);
                    cmd.Parameters["@batchName"].Value = searchString;
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
    public DataTable GetSemesterFeeMasterDetailsForGrid()
    {
        Int64 batchid = Convert.ToInt64(ddlbatchname.SelectedValue);
        Int64 Semid = Convert.ToInt64(ddlsem.SelectedItem.Text);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSemesterFeeMasterDetailsForGrid", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@batchid", SqlDbType.BigInt);
                    cmd.Parameters["@batchid"].Value = batchid;

                    cmd.Parameters.Add("@sem", SqlDbType.BigInt);
                    cmd.Parameters["@sem"].Value = Semid;
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

    public DataTable GetSemesterMasterDetailsForGrid()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSemesterMasterDetailsForGrid", con))
                {
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@pCentreID", SqlDbType.BigInt);
                    //cmd.Parameters["@pCentreID"].Value =Convert.ToInt64 (Session["EntityID"]);
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

    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            PagingBar2.CurrentPageIndex = 0;
            gridSemesterFeeType.PageIndex = PagingBar2.CurrentPageIndex;
            BindGridViewFilter();
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
            gridSemesterFeeType.PageIndex = PagingBar2.CurrentPageIndex;
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
            PagingBar2.CurrentPageIndex = 0;
            gridSemesterFeeType.PageIndex = PagingBar2.CurrentPageIndex;
            BindGridViewFilter();
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
            ddlCourseName.SelectedValue = "0";
            ddlbatchname.SelectedValue = "0";
            ddlsem.SelectedItem.Text = "";
            PagingBar2.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar2.CurrentPageIndex;

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    public DataTable GetSemester()
    {
        Int64 coursenameid = Convert.ToInt64(ddlCourse.SelectedValue);
        Int64 batchid = Convert.ToInt64(ddlbatchSession.SelectedValue);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSemesterforFormalCourse", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@courseId", SqlDbType.Int));
                    cmd.Parameters["@courseId"].Value = coursenameid;
                    cmd.Parameters.Add(new SqlParameter("@BatchId", SqlDbType.Int));
                    cmd.Parameters["@BatchId"].Value = batchid;


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
    public DataTable GetSemesterfilter()
    {
        Int64 coursenameid = Convert.ToInt64(ddlCourseName.SelectedValue);
        Int64 batchid = Convert.ToInt64(ddlbatchname.SelectedValue);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSemesterforFormalCourse", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@courseId", SqlDbType.Int));
                    cmd.Parameters["@courseId"].Value = coursenameid;
                    cmd.Parameters.Add(new SqlParameter("@BatchId", SqlDbType.Int));
                    cmd.Parameters["@BatchId"].Value = batchid;


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

    protected void ddlbatchSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlsemester.Items.Clear();
        ListItem lst = new ListItem("--Select--", "0");
        using (NIELITMISContext context = new NIELITMISContext())
        {
            Int64 coursenameid = Convert.ToInt64(ddlCourse.SelectedValue);
            Int64 batchid = Convert.ToInt64(ddlbatchSession.SelectedValue);
            if (coursenameid != 0)
            {

                using (DataTable dt = GetSemester())
                {
                    if (dt.Rows.Count > 0)
                    {
                        int i = Convert.ToInt32(dt.Rows[0]["SemNo"]);
                        int n = i;
                        for (i = 0; i <= n; i++)
                        {
                            if (i == 0)
                            {
                                ddlsemester.Items.Add(new ListItem("Select", "0"));
                            }
                            else
                            {
                                ddlsemester.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                ddlsemester.DataBind();
                            }
                        }
                    }
                }
            }
        }
    }


    protected void OnCheckedChanged(object sender, EventArgs e)
    {
        bool isUpdateVisible = false;
        lblMessage.Text = string.Empty;
        //Loop through all rows in GridView
        foreach (GridViewRow row in gvMain.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                bool isChecked = row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                if (isChecked)
                    row.RowState = DataControlRowState.Edit;
                for (int i = 1; i < row.Cells.Count; i++)
                {
                    // row.Cells[i].Controls.OfType<Label>().FirstOrDefault().Visible = !isChecked;
                    if (row.Cells[i].Controls.OfType<TextBox>().ToList().Count > 0)
                    {
                        row.Cells[i].Controls.OfType<TextBox>().FirstOrDefault().Visible = isChecked;
                        row.Cells[i].Controls.OfType<Label>().FirstOrDefault().Visible = !isChecked;
                    }
                    if (isChecked && !isUpdateVisible && ddlsemester.SelectedItem.Text == "1")
                    {
                        isUpdateVisible = true;
                        btnCancel.Visible = true;
                        divNavigation.Visible = true;
                    }
                    else if (isChecked && !isUpdateVisible && ddlsemester.SelectedItem.Text != "1")
                    {
                        isUpdateVisible = true;
                        btnCancel.Visible = true;
                        divNavigation.Visible = true;
                        btnSave.Text = "Update";
                    }
                }
                String Amount = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                if (Amount != "")
                {
                    //row.Cells[5].Controls.OfType<TextBox>().FirstOrDefault().Enabled = false;
                    row.Cells[1].Controls.OfType<TextBox>().FirstOrDefault().Enabled = true;
                }
                String Effectivefrom = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                if (Effectivefrom != "")
                {
                    // row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Enabled = false;
                    row.Cells[2].Controls.OfType<TextBox>().FirstOrDefault().Enabled = true;
                }
                String Effectiveto = row.Cells[3].Controls.OfType<Label>().FirstOrDefault().Text;
                if (Effectiveto != "")
                {
                    // row.Cells[6].Controls.OfType<TextBox>().FirstOrDefault().Enabled = false;
                    row.Cells[3].Controls.OfType<TextBox>().FirstOrDefault().Enabled = true;
                }
            }
        }
        btnSave.Visible = isUpdateVisible;
        btnCancel.Visible = isUpdateVisible;
        lblMessage.Text = "";
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
    protected void PageIndexChangedOld(Int32 NewPageIndex)
    {
        try
        {
            gridSemesterFeeType.PageIndex = PagingBar2.CurrentPageIndex;
            BindGridViewFilter();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gridSemesterFeeType_Sorting(object sender, GridViewSortEventArgs e)
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

    protected void gridSemesterFeeType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar2.CurrentPageSize * PagingBar2.CurrentPageIndex)).ToString();
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

    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);
        Int64 Courseid = 0;
        ddlbatchname.ClearSelection();
        ddlbatchname.Items.Clear();
        try
        {
            Courseid = Convert.ToInt64(ddlCourseName.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            { 
                ListItem lst = new ListItem("--Select--", "0");
                var Batch = from s in context.NielitCentreBatchs
                            where s.IsVerified == true && s.centreID == nielitcentreid
                                    && s.CourseDurationID == Courseid // && (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
                            orderby (s.Name)
                            select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, Batch, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetBatchesForSemesterMasterFilter()
    {

        Int64 coursenameid = Convert.ToInt64(ddlCourseName.SelectedValue);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetBatchesForSemesterMaster", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@courseDurationId", SqlDbType.Int));
                    cmd.Parameters["@courseDurationId"].Value = coursenameid;
                    cmd.Parameters.Add(new SqlParameter("@centreId", SqlDbType.Int));
                    cmd.Parameters["@centreId"].Value = NielitCentreIdFilter;// coursenameid;  // Hardcoded 507..as session is not maintained yet
                    cmd.Parameters.Add(new SqlParameter("@centreType", SqlDbType.Char));

                    cmd.Parameters["@centreType"].Value = 'm'; //need to apply condition for main or sub centre


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
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            NIELITMISContext context = new NIELITMISContext();
            Nullable<DateTime> Todate = null;
            Int32 userid = Convert.ToInt32(Session["UserId"]);
            DateTime entryDate = DateTime.Now;
            Int64 CourseId = Convert.ToInt64(ddlCourse.SelectedValue);
            Int64 batchID = Convert.ToInt64(ddlbatchSession.SelectedValue);

            Int64 SemId = 0;
            SemId = Convert.ToInt32(ddlsemester.SelectedItem.Text);
            Int64 semno = SemId - 1;
            Int64 semnocheck = SemId + 1;
            if (ddlsemester.SelectedItem.Text == "Select")
            {
                strMessage = "Semester No. is required";
                lblsemerror.Text = "Semester No. is required";
                ddlsemester.Focus();
                return;
            }
            else
            {
                SemId = Convert.ToInt32(ddlsemester.SelectedItem.Text);
            }
            SemesterFeeMaster ObjSemFee = new SemesterFeeMaster();


            if (SemId == 1)
            {
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    foreach (GridViewRow row in gvMain.Rows)
                    {

                        bool isChecked = row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                        if (isChecked)
                        {

                            if (row.Cells[1].Controls.OfType<TextBox>().FirstOrDefault().Text == "")
                            {
                                lblMessage.Visible = true;
                                strMessage = "Please enter fee amount.";
                                lblMessage.Text = "Please enter fee amount.";
                                return;

                            }


                            if (row.Cells[2].Controls.OfType<TextBox>().FirstOrDefault().Text == "")
                            {
                                lblMessage.Visible = true;
                                strMessage = "Please enter effective from date.";
                                lblMessage.Text = "Please enter effective from date.";
                                return;

                            }
                            if (row.Cells[3].Controls.OfType<TextBox>().FirstOrDefault().Text == "")
                            {
                                lblMessage.Visible = true;
                                strMessage = "Please enter effective To Date .";
                                lblMessage.Text = "Please enter effective To Date.";
                                return;

                            }

                            Int64 feeamount = Convert.ToInt32(row.Cells[1].Controls.OfType<TextBox>().FirstOrDefault().Text);
                            DateTime Efrm = Convert.ToDateTime(row.Cells[2].Controls.OfType<TextBox>().FirstOrDefault().Text);

                            if (feeamount < 0)
                            {
                                lblMessage.Visible = true;
                                lblMessage.Text = "Amount can not be negative";
                                return;
                            }

                            DateTime Eto = Convert.ToDateTime(row.Cells[3].Controls.OfType<TextBox>().FirstOrDefault().Text);
                            string feetypeid = (row.FindControl("lblfeetypeid") as Label).Text;
                            ObjSemFee.batchID = batchID;
                            ObjSemFee.courseid = CourseId;

                            var SemFeeExist = from batchFee1 in context.SemesterFeeMaster
                                              where batchFee1.batchID.ToString().Trim() == batchID.ToString().Trim()
                                             && batchFee1.courseid.ToString().Trim() == CourseId.ToString().Trim()
                                              && batchFee1.feeTypeID.ToString().Trim() == feetypeid

                                              select batchFee1;
                            if (SemFeeExist.Count() > 0)
                            {
                                Int64 feeId = Convert.ToInt64(feetypeid);
                                var SemNoExist = (from x in context.SemesterFeeMaster where x.courseid == CourseId && x.batchID == batchID && x.feeTypeID == feeId && x.SemId == semnocheck select x).FirstOrDefault();

                                if (SemNoExist != null)
                                {
                                    lblMessage.Visible = true;
                                    strMessage = "Updation is not allowed if next semester fee details is available.";
                                    lblMessage.Text = "Updation is not allowed if next semester fee details is available.";
                                    return;
                                }
                                else
                                {

                                    var EffectiveFromDates = (from x in context.SemesterFeeMaster where x.courseid == CourseId && x.batchID == batchID && x.feeTypeID == feeId && x.SemId == SemId select x).First();
                                    DateTime EffectiveFromDate = Convert.ToDateTime(EffectiveFromDates.effectiveFromDate);
                                    DateTime EffectiveToDate = Convert.ToDateTime(EffectiveFromDates.effectiveToDate);
                                    if (Efrm < EffectiveFromDate && Eto < EffectiveToDate)
                                    {
                                        lblMessage.Visible = true;
                                        strMessage = "Semester Effective from date should be greater than previous Semester Effective To Date.";
                                        lblMessage.Text = "Semester Effective from date should be greater than previous Semester Effective To Date.";
                                        return;

                                    }
                                    else if (Efrm != EffectiveFromDate)
                                    {
                                        if (Efrm < EffectiveToDate)
                                        {
                                            lblMessage.Visible = true;
                                            strMessage = "Semester Effective from date should be greater than previous Semester Effective To Date.";
                                            lblMessage.Text = "Semester Effective from date should be greater than previous Semester Effective To Date.";
                                            return;
                                        }
                                    }

                                    if (Efrm > Eto)
                                    {
                                        lblMessage.Visible = true;
                                        strMessage = "Effective from date should be less than Effective To Date.";
                                        lblMessage.Text = "Effective from date should be less than Effective To Date.";
                                        return;
                                    }

                                    using (DataTable dt = GetSemesterFeeupdate())
                                    {
                                        if (dt.Rows.Count <= 0)
                                        {
                                            strMessage = "Record updated.";
                                        }
                                    }
                                }
                            }


                            else if (SemFeeExist.Count() == 0)
                            {
                                if (Efrm > Eto)
                                {
                                    lblMessage.Visible = true;
                                    strMessage = "Effective from date should be less than Effective To Date.";
                                    lblMessage.Text = "Effective from date should be less than Effective To Date.";
                                    return;
                                }
                                ObjSemFee.courseid = CourseId;
                                ObjSemFee.batchID = batchID;
                                ObjSemFee.SemId = SemId;
                                ObjSemFee.feeTypeID = Convert.ToInt64(feetypeid);
                                ObjSemFee.feeAmount = feeamount;
                                ObjSemFee.effectiveFromDate = Efrm;
                                ObjSemFee.effectiveToDate = Eto;
                                ObjSemFee.enterDate = entryDate;
                                ObjSemFee.enterBy = userid;

                                context.SemesterFeeMaster.Add(ObjSemFee);
                                context.SaveChanges();
                                strMessage = "New Record Saved";
                            }

                        }

                    }
                }
            }
            else
            {
                foreach (GridViewRow row in gvMain.Rows)
                {
                    if (SemId > 1)
                    {

                        string feetypeid = (row.FindControl("lblfeetypeid") as Label).Text;
                        var SemFee = from batchFee1 in context.SemesterFeeMaster
                                     where batchFee1.batchID.ToString().Trim() == batchID.ToString().Trim()
                                    && batchFee1.courseid.ToString().Trim() == CourseId.ToString().Trim()
                                     && batchFee1.feeTypeID.ToString().Trim() == feetypeid && batchFee1.SemId == SemId

                                     select batchFee1;
                        if (SemFee.Count() <= 0)
                        {

                            bool isChecked = row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                            if (isChecked)
                            {
                                Int64 feeamount = Convert.ToInt32(row.Cells[1].Controls.OfType<TextBox>().FirstOrDefault().Text);
                                DateTime Efrm = Convert.ToDateTime(row.Cells[2].Controls.OfType<TextBox>().FirstOrDefault().Text);

                                DateTime Eto = Convert.ToDateTime(row.Cells[3].Controls.OfType<TextBox>().FirstOrDefault().Text);
                                if (feeamount < 0)
                                {
                                    lblMessage.Visible = true;
                                    lblMessage.Text = "Amount can not be negative";
                                    return;
                                }
                                Int64 feeId = Convert.ToInt64(feetypeid);
                                var EffectiveFromDates = (from x in context.SemesterFeeMaster where x.courseid == CourseId && x.batchID == batchID && x.feeTypeID == feeId && x.SemId == semno select x).First();
                                DateTime EffectiveFromDate = Convert.ToDateTime(EffectiveFromDates.effectiveFromDate);
                                DateTime EffectiveToDate = Convert.ToDateTime(EffectiveFromDates.effectiveToDate);
                                if (Efrm < EffectiveFromDate && Eto < EffectiveToDate)
                                {
                                    lblMessage.Visible = true;
                                    strMessage = "Semester Effective from date should be greater than previous Semester Effective To Date.";
                                    lblMessage.Text = "Semester Effective from date should be greater than previous Semester Effective To Date.";
                                    return;

                                }
                                else if (Efrm != EffectiveFromDate)
                                {
                                    if (Efrm < EffectiveToDate)
                                    {
                                        lblMessage.Visible = true;
                                        strMessage = "Semester Effective from date should be greater than previous Semester Effective To Date.";
                                        lblMessage.Text = "Semester Effective from date should be greater than previous Semester Effective To Date.";
                                        return;
                                    }
                                }

                                if (Efrm > Eto)
                                {
                                    lblMessage.Visible = true;
                                    strMessage = "Effective from date should be less than Effective To Date.";
                                    lblMessage.Text = "Effective from date should be less than Effective To Date.";
                                    return;
                                }
                                ObjSemFee.courseid = CourseId;
                                ObjSemFee.batchID = batchID;
                                ObjSemFee.SemId = SemId;
                                ObjSemFee.feeTypeID = Convert.ToInt64(feetypeid);
                                ObjSemFee.feeAmount = feeamount;
                                ObjSemFee.effectiveFromDate = Efrm;
                                ObjSemFee.effectiveToDate = Eto;
                                ObjSemFee.enterDate = entryDate;
                                ObjSemFee.enterBy = userid;

                                context.SemesterFeeMaster.Add(ObjSemFee);
                                context.SaveChanges();
                                strMessage = "New Record Saved";
                            }
                        }
                        else if (SemFee.Count() > 0)
                        {
                            bool isChecked = row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                            if (isChecked)
                            {
                                Int64 feeamount = Convert.ToInt32(row.Cells[1].Controls.OfType<TextBox>().FirstOrDefault().Text);
                                DateTime Efrm = Convert.ToDateTime(row.Cells[2].Controls.OfType<TextBox>().FirstOrDefault().Text);

                                DateTime Eto = Convert.ToDateTime(row.Cells[3].Controls.OfType<TextBox>().FirstOrDefault().Text);
                                if (feeamount < 0)
                                {
                                    lblMessage.Visible = true;
                                    lblMessage.Text = "Amount can not be negative";
                                    return;
                                }

                                Int64 feeId = Convert.ToInt64(feetypeid);
                                var SemNoExist = (from x in context.SemesterFeeMaster where x.courseid == CourseId && x.batchID == batchID && x.feeTypeID == feeId && x.SemId == semnocheck select x).FirstOrDefault();

                                if (SemNoExist != null)
                                {
                                    lblMessage.Visible = true;
                                    strMessage = "Updation is not allowed if next semester fee details is available.";
                                    lblMessage.Text = "Updation is not allowed if next semester fee details is available.";
                                    return;
                                }
                                else
                                {
                                    var EffectiveFromDates = (from x in context.SemesterFeeMaster where x.courseid == CourseId && x.batchID == batchID && x.feeTypeID == feeId && x.SemId == semno select x).First();
                                    DateTime EffectiveFromDate = Convert.ToDateTime(EffectiveFromDates.effectiveFromDate);
                                    DateTime EffectiveToDate = Convert.ToDateTime(EffectiveFromDates.effectiveToDate);
                                    if (Efrm < EffectiveFromDate && Eto < EffectiveToDate)
                                    {
                                        lblMessage.Visible = true;
                                        strMessage = "Semester Effective from date should be greater than previous Semester Effective To Date.";
                                        lblMessage.Text = "Semester Effective from date should be greater than previous Semester Effective To Date.";
                                        return;

                                    }
                                    else if (Efrm != EffectiveFromDate)
                                    {
                                        if (Efrm < EffectiveToDate)
                                        {
                                            lblMessage.Visible = true;
                                            strMessage = "Semester Effective from date should be greater than previous Semester Effective To Date.";
                                            lblMessage.Text = "Semester Effective from date should be greater than previous Semester Effective To Date.";
                                            return;
                                        }
                                    }
                                    if (Efrm > Eto)
                                    {
                                        lblMessage.Visible = true;
                                        strMessage = "Effective from date should be less than Effective To Date.";
                                        lblMessage.Text = "Effective from date should be less than Effective To Date.";
                                        return;
                                    }

                                    using (DataTable dt = GetSemesterFeeupdate())
                                    {
                                        if (dt.Rows.Count <= 0)
                                        {
                                            strMessage = "Record updated.";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            Response.Redirect("SemesterFeeMaster.aspx?msg=" + strMessage);

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    public DataTable GetSemesterFeeupdate()
    {

        Int64 courseid = Convert.ToInt64(ddlCourse.SelectedValue);
        Int64 batchid = Convert.ToInt64(ddlbatchSession.SelectedValue);

        DataTable myDt = new DataTable();
        foreach (GridViewRow row in gvMain.Rows)
        {

            bool isChecked = row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
            if (isChecked)
            {

                string feetypeid = (row.FindControl("lblfeetypeid") as Label).Text;
                Int64 feeamount = Convert.ToInt32(row.Cells[1].Controls.OfType<TextBox>().FirstOrDefault().Text);
                DateTime Efrm = Convert.ToDateTime(row.Cells[2].Controls.OfType<TextBox>().FirstOrDefault().Text);
                DateTime Eto = Convert.ToDateTime(row.Cells[3].Controls.OfType<TextBox>().FirstOrDefault().Text);
                Int64 semid = Convert.ToInt64(ddlsemester.SelectedItem.Text);
                string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;

                using (SqlConnection con = new SqlConnection(constr))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("updateSemsterFeeMaster", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@courseid", SqlDbType.BigInt);
                            cmd.Parameters["@courseid"].Value = courseid;

                            cmd.Parameters.Add("@batchid", SqlDbType.BigInt);
                            cmd.Parameters["@batchid"].Value = batchid;

                            cmd.Parameters.Add("@semid", SqlDbType.BigInt);
                            cmd.Parameters["@semid"].Value = semid;

                            cmd.Parameters.Add("@feetypeid", SqlDbType.BigInt);
                            cmd.Parameters["@feetypeid"].Value = Convert.ToInt64(feetypeid);

                            cmd.Parameters.Add("@amount", SqlDbType.BigInt);
                            cmd.Parameters["@amount"].Value = feeamount;

                            cmd.Parameters.Add("@efffrom", SqlDbType.DateTime);
                            cmd.Parameters["@efffrom"].Value = Efrm;

                            cmd.Parameters.Add("@effto", SqlDbType.DateTime);
                            cmd.Parameters["@effto"].Value = Eto;

                            cmd.Parameters.Add("@entryby", SqlDbType.Int);
                            cmd.Parameters["@entryby"].Value = Convert.ToInt32(Session["UserId"]);

                            cmd.Parameters.Add("@entrydate", SqlDbType.DateTime);
                            cmd.Parameters["@entrydate"].Value = DateTime.Now;
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
            }
        }
        return myDt;

    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        Int32 loginUserNo = 0, UserTypeId = 0, NielitSubCentreIdd = 0, NielitCentreIdSearch = 0, NielitCentrelinkedToCentreId = 0;
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
                else if (UserTypeId == 11)
                {
                    var intituteslinkedToCentre = context.NonAffInstitutes.Find(loginUser.UserRefNumber); //HNonAfflAfflInst
                    Int32 NielitSubCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                    NielitSubCentreIdd = NielitSubCentreId;
                    NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                    NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                    if (institutesName != null)
                    {
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        NielitCentreIdSearch = NielitSubCentreId;
                    }
                }
                else if (UserTypeId == 4)
                {
                    var intituteslinkedToCentre = context.AffInstitutes.Find(loginUser.UserRefNumber);
                    Int32 NielitSubCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                    NielitSubCentreIdd = NielitSubCentreId;
                    NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                    NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                    if (institutesName != null)
                    {
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        NielitCentreIdSearch = NielitSubCentreId;
                    }
                }
            }
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            EConnectContext context2 = new EConnectContext();
            //var BatchName =  from s in context.NielitCentreBatchs
            //                         join c in context.NielitCourseDurations on s.CourseDurationID equals c.ID 
            //                         join p in context.NielitCentreCourses on c.courseID equals p.ID
            //                 where s.centreID == NielitCentreIdSearch && c.isVerified==true && c.isVerified==true
            //                 select new { Name = s.Name };

            var BatchName = from s in context.NielitCentreBatchs
                            join b in context.SemesterFeeMaster on s.ID equals b.batchID
                            where s.centreID == NielitCentreIdSearch && s.IsVerified == true
                            select new { Name = s.Name };

            if (!String.IsNullOrEmpty(searchString))
            {
                BatchName = BatchName.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            BatchName = BatchName.OrderBy(s => s.Name);

            //var Batchcode = from s in context.NielitCentreBatchs
            //               join c in context.NielitCourseDurations on s.CourseDurationID equals c.ID
            //               join p in context.NielitCentreCourses on c.courseID equals p.ID
            //                where s.centreID == NielitCentreIdSearch && c.isVerified == true && c.isVerified == true
            //               select new { Name = s.BatchCode };
            var Batchcode = from s in context.NielitCentreBatchs
                            join b in context.SemesterFeeMaster on s.ID equals b.batchID
                            where s.centreID == NielitCentreIdSearch && s.IsVerified == true
                            select new { Name = s.BatchCode };

            if (!String.IsNullOrEmpty(searchString))
            {
                Batchcode = Batchcode.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            BatchName = BatchName.Union(Batchcode).Take(count);

            foreach (var c in BatchName)
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
                SemesterFeeMaster feemaster = context.SemesterFeeMaster.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                context.SemesterFeeMaster.Remove(feemaster);
                context.SaveChanges();
                BindGridViewFilter();
                ShowAlert("Record deleted successfully.", true);
                hfActionID.Value = "";
            };
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            BindGridViewFilter();
            uPnlGrid.Update();
            ShowAlert("Record can not be deleted!", true);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BindGridView();
        btnSave.Visible = false;
        btnCancel.Visible = false;
        lblMessage.Text = "";
        lblMessage.Visible = false;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("SemesterFeeMaster.aspx", true);

    }
    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblsemerror.Text = "";
    }
    protected void ddlbatchname_SelectedIndexChanged(object sender, EventArgs e)
    {

        ddlsem.Items.Clear();
        ListItem lst = new ListItem("--Select--", "0");
        using (NIELITMISContext context = new NIELITMISContext())
        {
            Int64 coursenameid = Convert.ToInt64(ddlCourseName.SelectedValue);
            Int64 batchid = Convert.ToInt64(ddlbatchname.SelectedValue);
            if (coursenameid != 0)
            {

                using (DataTable dt = GetSemesterfilter())
                {
                    if (dt.Rows.Count > 0)
                    {
                        int i = Convert.ToInt32(dt.Rows[0]["SemNo"]);
                        int n = i;
                        for (i = 0; i <= n; i++)
                        {
                            if (i == 0)
                            {
                                ddlsem.Items.Add(new ListItem("Select", "Select"));
                            }
                            else
                            {
                                ddlsem.Items.Add(new ListItem(i.ToString(), i.ToString()));
                                ddlsem.DataBind();
                            }
                        }
                    }
                }
            }
        }
    }
}





