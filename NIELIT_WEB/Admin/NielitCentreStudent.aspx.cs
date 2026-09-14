using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Transactions;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.IO.Compression;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Data.Objects;
using System.Text.RegularExpressions;
using EConnect;
using System.Data.Entity;



public partial class Admin_NielitCentreStudent : BasePage
{
    int stateid = 0;

    String strMessage = string.Empty;
    Boolean whetherFileUpload = false;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 entityID = 0;
    Int32 lnkID = 0;
    Int32 UserRefNumber = 0;
    Int32 UserTypeid = 0;
    Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
    Int64 NielitCentrelinkedToCentreId = 0;
    String NameCenter = "";
    //Dynamic_Array_for_Paramters_November_2024
    List<SqlParameter> paramList = new List<SqlParameter>();

    //Fields_disable_for_batch_06-07-2026_amit_start
    private static readonly HashSet<long> excluded_batch = new HashSet<long>
    {
        1010042,
        1010043,
        1010044
    };
    //Fields_disable_for_batch_06-07-2026_amit_start
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
            entityID = Convert.ToInt32(Session["EntityID"]);
            UserTypeid = Convert.ToInt32(Session["UserType"]);
            whetherFileUpload = false;


            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            //bindCenter();
            User objUser1;
            using (EConnectContext context = new EConnectContext())
            {
                objUser1 = new EConnect.URM.User();

                User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);
            }
            Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
            if (!Page.IsPostBack)
            {
                GenerateNewCaptchaImage();
                User objUser;

                using (EConnectContext context = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();

                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);

                    using (NIELITMISContext context1 = new NIELITMISContext())
                    {

                        if (UserTypeid == 10)
                        {
                            ListItem lst = new ListItem("--Select One--", "0");
                            var Center = from t in context1.NielitCentres
                                         where t.ID == UserRefNumber
                                         orderby (t.Name)
                                         select new { ValueField = t.ID, TextField = t.Name };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, Center, lst);

                            var intituteslinkedToCentre = context1.NielitCentres.Find(loginUser.UserRefNumber);
                            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                            lnkID = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            if (lnkID != 0)
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                                txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                RdoAffInstOrNonAffInst.SelectedValue = "2";
                                // ddlSubcentreName.Enabled = false;
                            }
                            else
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                                txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                RdoAffInstOrNonAffInst.SelectedValue = "2";
                                ddlCenter.SelectedValue = NielitCentreId.ToString();
                                ddlCenter.Enabled = false;
                                ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                            }
                        }
                        else if (UserTypeid == 11)
                        {
                            var intituteslinkedToCentre = context1.NonAffInstitutes.Find(loginUser.UserRefNumber);
                            lnkID = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                            if (institutesName != null)
                            {
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            }
                            RdoAffInstOrNonAffInst.Items.RemoveAt(2);
                            FillddlcentreName();
                            RdoAffInstOrNonAffInst.Items.RemoveAt(0);
                            ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                        }
                        else if (UserTypeid == 4)
                        {
                            var intituteslinkedToCentre = from s in context1.AffInstitutes
                                                          where s.instituteID == loginUser.UserRefNumber
                                                          select new { ID = s.ID, linkedToCentre = s.linkedToCentre };
                            if (intituteslinkedToCentre.Count() == 0)
                            {
                                ShowAlert("Menu is not available for the institute");
                                return;
                            }
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().ID);
                            //HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
                            lnkID = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
                            // NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                            if (institutesName != null)
                            {
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                //hcentreID.Value = NelitCentreLinkId.ToString();


                                //Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            }
                            RdoAffInstOrNonAffInst.Items.RemoveAt(2);
                            FillddlcentreName();
                            RdoAffInstOrNonAffInst.Items.RemoveAt(1);


                        }
                    }
                    //Added
                    if (ddlWhetherPlaced.SelectedValue.ToString() == "1")
                        btn.Visible = true;
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        BindGender();
                        BindEditNewModeData();

                        ShowEditMode(sender, e);
                    }
                    else
                    {
                        BindGender();
                        BindEditNewModeData();
                        BindListData();
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";
                        // Done by Jyoti 06 May 2021
                        // BindGridView();

                        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Nielit Centre Student", "Admin/NielitCentreStudent.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                        }
                        else
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Nielit Centre Student", "Admin/NielitCentreStudent.aspx", ""));
                        }

                    }
                    if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                        ShowAlert(Request.QueryString["msg"].ToString());
                }
            }


            BreadCrumb1.Render();
            //  RadioButtonListCentre.Enabled = true;
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }


    }

    protected void FillddlcentreName()
    {
        try
        {
            User objUser;
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                ListItem lst1 = new ListItem("--Select One--", "99");
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);


                using (NIELITMISContext context = new NIELITMISContext())
                {
                    NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                    txtInstitute.Text = institutesName.Name;
                    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                    ddlCenter.ClearSelection();
                    if (UserTypeid == 11)
                    {
                        var centreName = from s in context.NonAffInstitutes
                                         where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                         select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                        ddlCenter.Enabled = false;
                        var NonAfflcentre = (from p in context.NonAffInstitutes
                                             where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                             select p).FirstOrDefault();
                        ddlCenter.SelectedValue = NonAfflcentre.ID.ToString();
                        RdoAffInstOrNonAffInst.SelectedValue = "0";
                        ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                    }
                    else
                    {
                        var centreName = from s in context.AffInstitutes
                                         where s.linkedToCentre == NelitCentreLinkId && s.instituteID == loginUser.UserRefNumber
                                         select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                        //Changed above
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName, lst1);
                        ddlCenter.Enabled = false;
                        var NonAfflcentre = (from p in context.AffInstitutes
                                             where p.linkedToCentre == NelitCentreLinkId && p.instituteID == loginUser.UserRefNumber
                                             select p).FirstOrDefault();
                        ddlCenter.SelectedValue = NonAfflcentre.instituteID.ToString();
                        RdoAffInstOrNonAffInst.SelectedValue = "1";
                        ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                    }
                }
            }
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
            ddlCourse.ClearSelection();
            ddlBatch.ClearSelection();
            ddlCenter.Items.Clear();
            ddlCourse.Items.Clear();
            ddlBatch.Items.Clear();
            User objUser;
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context2 = new NIELITMISContext())
                {
                    if (UserTypeid == 10)
                    {
                        var instituteslinkedToCentre = context2.NielitCentres.Find(loginUser.UserRefNumber);
                        lnkID = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (UserTypeid == 11)
                    {
                        var instituteslinkedToCentre = context2.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        lnkID = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (lnkID != 0)
                    {
                        NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                        txtInstitute.Text = institutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        using (NIELITMISContext context = new NIELITMISContext())
                        {
                            ListItem lst1 = new ListItem("--Select One--", "99");
                            if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                            {
                                ddlCenter.ClearSelection();
                                var centreName = from s in context.AffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName.Distinct(), lst1);
                                ddlCenter.Enabled = true;
                            }
                            else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                            {
                                if (UserTypeid == 11)//Non AffInstitutes by user refNumber
                                {
                                    ddlCenter.ClearSelection();
                                    var centreName = from s in context.NonAffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName.Distinct(), lst1);
                                    //ddlCenter.Enabled = false;
                                    var NonAfflcentre = (from p in context.NonAffInstitutes
                                                         where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                                         select p).FirstOrDefault();
                                    ddlCenter.SelectedValue = NonAfflcentre.ID.ToString();
                                }
                                else //NonAffInstitutes for Nielit Centres
                                {
                                    ddlCenter.ClearSelection();
                                    var centreName = from s in context.NonAffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName.Distinct(), lst1);
                                    ddlCenter.Enabled = true;
                                }
                                if (UserTypeid == 4)//AffInstitutes by user refNumber
                                {
                                    ddlCenter.ClearSelection();
                                    var centreName = from s in context.AffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId && s.instituteID == loginUser.UserRefNumber
                                                     select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName.Distinct(), lst1);
                                    //ddlCenter.Enabled = false;
                                    var Afflcentre = (from p in context.AffInstitutes
                                                      where p.linkedToCentre == NelitCentreLinkId && p.instituteID == loginUser.UserRefNumber
                                                      select p).FirstOrDefault();
                                    ddlCenter.SelectedValue = Afflcentre.instituteID.ToString();
                                }
                                else // AffInstitutes for Nielit Centres
                                {
                                    ddlCenter.ClearSelection();
                                    var centreName = from s in context.AffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId
                                                     select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName.Distinct(), lst1);
                                    ddlCenter.Enabled = true;
                                }
                            }
                            else
                            {
                                ddlCenter.Items.Add(new ListItem("--Select One--", "99"));
                                ddlCenter.SelectedValue = "99";
                                //ddlCenter.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                        txtInstitute.Text = institutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        using (NIELITMISContext context = new NIELITMISContext())
                        {
                            ListItem lst1 = new ListItem("--Select One--", "99");
                            if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                            {
                                ddlCenter.ClearSelection();
                                var centreName = from s in context.AffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.instituteID, TextField = s.Name + "(" + s.Accr_No + ")" };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName.Distinct(), lst1);
                                ddlCenter.Enabled = true;
                            }
                            else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                            {
                                ddlCenter.ClearSelection();
                                ddlCenter.Items.Clear();
                                var centreName = from s in context.NonAffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.ID, TextField = s.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, centreName.Distinct(), lst1);
                                ddlCenter.Enabled = true;
                            }

                            else
                            {
                                ddlCenter.ClearSelection();

                                var Center = from t in context.NielitCentres
                                             where t.ID == UserRefNumber
                                             orderby (t.Name)
                                             select new { ValueField = t.ID, TextField = t.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, Center.Distinct(), lst1);

                                ddlCenter.SelectedValue = NielitCentreId.ToString();
                                ddlCenter.Enabled = false;
                                ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }
    }

    protected void BindEditNewModeData()
    {
        try
        {
            bindMaritalStatus();
            bindCastCategory();
            bindReligion();
            bindState();

            bindCompanyName();
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");

                //var Courses = from s in context.NielitCentreCourses
                //              where s.ShowOnWeb == true && s.IsActive == true
                //              orderby (s.Name)
                //              select new { ValueField = s.ID, TextField = s.Name };
                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, Courses, lst);

                //var Batch = from s in context.NielitCentreBatchs
                //            where s.Show_On_Web == true && s.IsActive == true
                //            orderby (s.Name)
                //            select new { ValueField = s.ID, TextField = s.Name };
                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);

                var CorState = (from s in context.Locations
                                orderby (s.Name)
                                where s.LocationTypeID == 2
                                && s.ParentLocationID == 1
                                select new { ValueField = s.ID, TextField = s.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCmpState, CorState, lst);

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
            // Done by Jyoti 06 May 2021
            User objUser;

            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();

                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    ListItem lst = new ListItem("--All--", "0");


                    ListItem lst1 = new ListItem("--All--", "0");
                    var state = from s in context.NielitCentreBatchs
                                where s.centreID == UserRefNumber || s.subCentreID == UserRefNumber
                                orderby s.BatchCode
                                select new { ValueField = s.ID, TextField = s.BatchCode + " (" + s.Name + ")" };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlName, state, lst1);

                    /*
                    ListItem lst1 = new ListItem("--All--", "0");
                    var state = from s in context.NielitCentreStudent
                                //where s.LocationTypeID == 2
                                select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlName, state, lst1);
                    */

                    //EnumUtility.BindListObject(ref ddlflexcentretype, typeof(EConnect.NIELIT.enmExamCenterType), new ListItem("--All--", "0"));
                }


                // Done by Jyoti 06 May 2021
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillCourses(DropDownList ddl, Int32 courseCategoryID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ddl.Items.Clear();
                ListItem lst = new ListItem("--All--", "0");
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == courseCategoryID
                                 orderby p.DisplayOrder
                                 select new { ValueField = p.ID, TextField = p.Name };

                if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                {
                    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                    CourseList = CourseList.Where(a => roleCourses.Contains(a.ValueField));
                }
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, CourseList, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ShowEditMode(object sender, EventArgs e)
    {
        try
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Nielit Centre Student";
            tblNavLinks.Visible = true;

            ddlBatch.Enabled = false;
            ddlCourse.Enabled = false;
            //GenerateNewCaptchaImage();
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int32 Id = Convert.ToInt32(Request.QueryString["Key"]);

                //added by amit start
                hfapplcode.Value = Convert.ToString(Request.QueryString["Key"]);
                //added by amit end

                Int64 applID = Convert.ToInt64(Request.QueryString["key"]);
                //This Query will get all the information of Applied Canditate by generated Application id                                     
                var application = context.NielitCentreStudent.Find(applID);

                if (application.whetherAffiliated == "O")
                {
                    ddlCenter.ClearSelection();
                    RdoAffInstOrNonAffInst.SelectedValue = "2";
                    RdoAffInstOrNonAffInst_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);

                    ddlCenter.SelectedValue = application.InstituteID.ToString();
                    ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                    ddlCourse.SelectedValue = application.CourseID.ToString();
                    ddlCourse_SelectedIndexChanged(ddlCourse, EventArgs.Empty);
                    ddlBatch.SelectedValue = application.batch_ID.ToString();
                    ddlCourse.Enabled = false;
                    ddlCenter.Enabled = false;
                    ddlBatch.Enabled = false;

                }
                else if (application.whetherAffiliated == "Y")
                {
                    ddlCenter.ClearSelection();
                    RdoAffInstOrNonAffInst.SelectedValue = "1";
                    RdoAffInstOrNonAffInst_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);

                    ddlCenter.SelectedValue = application.InstituteID.ToString();
                    ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                    ddlCourse.SelectedValue = application.CourseID.ToString();
                    ddlCourse_SelectedIndexChanged(ddlCourse, EventArgs.Empty);
                    ddlBatch.SelectedValue = application.batch_ID.ToString();
                    ddlCourse.Enabled = false;
                    ddlCenter.Enabled = false;
                    ddlBatch.Enabled = false;
                }
                else if (application.whetherAffiliated == "N")
                {
                    ddlCenter.ClearSelection();
                    RdoAffInstOrNonAffInst.SelectedValue = "0";
                    RdoAffInstOrNonAffInst_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);

                    ddlCenter.SelectedValue = application.InstituteID.ToString();
                    ddlCenter_SelectedIndexChanged(ddlCenter, EventArgs.Empty);
                    ddlCourse.SelectedValue = application.CourseID.ToString();
                    ddlCourse_SelectedIndexChanged(ddlCourse, EventArgs.Empty);

                    ddlBatch.SelectedValue = application.batch_ID.ToString();

                    ddlCourse.Enabled = false;
                    ddlCenter.Enabled = false;
                    ddlBatch.Enabled = false;
                }

                if (application.whetherProjectStudent == true)
                {
                    ddlWheatherPojectStu.SelectedValue = "1";
                    ddlProcname.Visible = true;
                    LblProjectname.Visible = true;

                    //					bindProject1();

                    ddlProcname.SelectedValue = application.projectId.ToString();
                    ddlProcname_SelectedIndexChanged(sender, e);
                }
                else
                {
                    ddlWheatherPojectStu.SelectedValue = "2";
                    //ddlProcname.Visible = false;
                    //LblProjectname.Visible = false;
                    ddlProcname.SelectedValue = "0";
                }

                ddlSalutaionName.SelectedValue = application.Salutation.ToString();
                txtAppName.Text = GetInitCap(application.Name.ToString());
                lblname.Text = txtAppName.Text;



                if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName) == true)
                {
                    trfather.Visible = true;
                    trmother.Visible = true;
                    trguardian.Visible = false;
                    Rdoownertype.SelectedValue = "P";
                    Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                    txtFatherName.Text = string.IsNullOrEmpty(application.FatherName) == false && !string.IsNullOrWhiteSpace(application.FatherName) ? GetInitCap(application.FatherName) : "";
                    txtMotherName.Text = string.IsNullOrEmpty(application.MotherName) == false && !string.IsNullOrWhiteSpace(application.MotherName) ? GetInitCap(application.MotherName) : "";
                }
                else
                {
                    trfather.Visible = false;
                    trmother.Visible = false;
                    trguardian.Visible = true;
                    Rdoownertype.SelectedValue = "G";
                    Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                    TxtGuardianName.Text = GetInitCap(application.GuardianName);
                }

                ddl_gender.SelectedValue = application.Gender;
                txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");
                //Added jksah
                //jk sah on 13-Jan-2021------
                if (application.Is_EWS.ToString() == "True")
                {
                    RdisEWS.SelectedValue = "Y";
                }
                else
                {
                    RdisEWS.SelectedValue = "N";
                }
                RdisEWS_SelectedIndexChanged(sender, e);
                //----------------------                                   
                //txtBodyMark.Text = string.IsNullOrEmpty(application.BodyMark) == false && !string.IsNullOrWhiteSpace(application.BodyMark) ? application.BodyMark : "";
                ddlCategory.SelectedValue = application.CastCategoryID.ToString();
                ddlCategory_SelectedIndexChanged(sender, e);
                ddlMStatus.SelectedValue = application.MaritalStatusID.ToString();
                if (application.IsHandicaped.ToString() == "True")
                {
                    Rdhandicapped.SelectedValue = "Y";
                }
                else
                {
                    Rdhandicapped.SelectedValue = "N";
                }
                if (application.IsExServicemane.ToString() == "True")
                {
                    Rdexserviceman.SelectedValue = "Y";
                }
                else
                {
                    Rdexserviceman.SelectedValue = "N";
                }
                ddlReligion.SelectedValue = application.ReligionID.ToString();
                //Candidate Contact Detail...

                TxtSTDcode.Text = application.PhoneNumber.HasValue && application.PhoneNumber != 0 ? "0" + application.StdNumber.ToString() : "";
                txtCorPhoneNo.Text = application.PhoneNumber.ToString();
                txtCorMobileNo.Text = application.MobileNumber.ToString();
                txtEmailId.Text = application.EmailAddress.ToString();


                //aadhar details
                //if (application.AadharNumber.HasValue)
                //    txtaadhar.Text = application.AadharNumber.Value.ToString();
                //else
                //    txtaadhar.Text = "";


                //Candidate Address Detail...
                TxtPerAddressLine1.Text = string.IsNullOrEmpty(application.PerAddressLine1) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine1) ? GetInitCap(application.PerAddressLine1) : "";
                TxtPerAddressLine2.Text = string.IsNullOrEmpty(application.PerAddressLine2) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine2) ? GetInitCap(application.PerAddressLine2) : "";
                TxtPerAddressLine3.Text = string.IsNullOrEmpty(application.PerAddressLine3) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine3) ? GetInitCap(application.PerAddressLine3) : "";
                TxtPerCity.Text = string.IsNullOrEmpty(application.PerCityName) == false && !string.IsNullOrWhiteSpace(application.PerCityName) ? GetInitCap(application.PerCityName) : "";
                ddlPState.SelectedValue = application.PerStateID != 0 ? application.PerStateID.ToString() : "0";
                int id1 = Convert.ToInt32(ddlPState.SelectedValue);
                ddlPState_SelectedIndexChanged(ddlPState, EventArgs.Empty);
                ddlPdistrict.SelectedValue = application.PerDistrictID.ToString();
                TxtPpincode.Text = application.PerPinCode.ToString();

                TxtCorAddressLine1.Text = string.IsNullOrEmpty(application.CorAddressLine1) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine1) ? GetInitCap(application.CorAddressLine1) : "";
                TxtCorAddressLine2.Text = string.IsNullOrEmpty(application.CorAddressLine2) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine2) ? GetInitCap(application.CorAddressLine2) : "";
                TxtCorAddressLine3.Text = string.IsNullOrEmpty(application.CorAddressLine3) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine3) ? GetInitCap(application.CorAddressLine3) : "";
                TxtCorCity.Text = string.IsNullOrEmpty(application.CorCityName) == false && !string.IsNullOrWhiteSpace(application.CorCityName) ? GetInitCap(application.CorCityName) : "";
                ddlCorState.SelectedValue = application.CorStateID.ToString();
                int id2 = Convert.ToInt32(ddlCorState.SelectedValue);
                ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                ddlcordistrict.SelectedValue = application.CorDistrictID.ToString();
                txtCorPinCode.Text = application.CorPinCode.ToString();

                //aadhar details
                //if (application.AadharNumber.HasValue)
                //    txtaadhar.Text = application.AadharNumber.Value.ToString();
                //else
                //    txtaadhar.Text = "";

                if (application.UIDType != null)
                {
                    UidTypeDdl.SelectedValue = application.UIDType.ToString();
                    UidNumberTxt.Text = application.UIDNumber.ToString().ToUpper();
                    if (UidTypeDdl.SelectedValue.ToString() == "1")
                    {

                        UidNumberTxt.Text = EConnect.Utils.Security.Decryption.Decrypt(application.UIDNumber.ToString().ToUpper());
                        UidNumberTxt.Visible = false;
                    }
                    else
                    {
                        UidNumberTxt.Text = application.UIDNumber.ToString().ToUpper();
                        UidNumberTxt.Visible = true;
                    }

                }


                if (application.whether_Course_Complete == true)
                {
                    ddlCourseComplete.SelectedValue = "1";
                }
                else
                {
                    ddlCourseComplete.SelectedValue = "2";
                }

                if (application.whether_Certificate_Issued == true)
                {
                    ddlwhetherCertificateIssued.SelectedValue = "1";

                }
                else
                {
                    ddlwhetherCertificateIssued.SelectedValue = "2";
                }

                if (application.whetherPlaced == true)
                {
                    ddlWhetherPlaced.SelectedValue = "1";
                    ddlWhetherPlaced_SelectedIndexChanged(ddlWhetherPlaced, EventArgs.Empty);

                }
                else
                {
                    ddlWhetherPlaced.SelectedValue = "2";
                    ddlWhetherPlaced_SelectedIndexChanged(ddlWhetherPlaced, EventArgs.Empty);
                }

                if (!string.IsNullOrEmpty(application.certificate_Issue_Date.ToString()) == true)
                {
                    string Date = application.certificate_Issue_Date.ToString();
                    if (Date == "")
                    {
                        txtcertificateIssueDate.Text = application.certificate_Issue_Date.ToString();
                    }
                    else
                    {
                        txtcertificateIssueDate.Text = Convert.ToDateTime(Date).ToString("dd-MMM-yyyy");
                    }

                }

                if (!string.IsNullOrEmpty(application.placementDate.ToString()) == true)
                {
                    var PlacementDates = (from d in context.studentPlacementDetails
                                          where d.studentID == applID
                                          select d.EffectiveFromDate).Min();

                    string Date = PlacementDates.ToString();
                    if (Date == "")
                    {
                        txtPlacementDate.Text = application.placementDate.ToString();
                    }
                    else
                    {
                        txtPlacementDate.Text = Convert.ToDateTime(Date).ToString("dd-MMM-yyyy");
                    }
                    txtPlacementDate.Enabled = false;
                }

                //txtCompanyName.Text = application.company_Name.ToString();
                ddlCompanyName.SelectedValue = application.company_Name.ToString();
                txtCmpAdd1.Text = application.comapny_Address.ToString();
                txtCmpAdd2.Text = application.comapny_Address2.ToString();
                txtCmpAdd3.Text = application.comapny_Address3.ToString();
                txtCmyCityName.Text = application.CompnyCityName.ToString();

                ddlCmpState.SelectedValue = application.compnyState_ID.ToString();
                ddlCmpState_SelectedIndexChanged(ddlCmpState, EventArgs.Empty);
                ddlCmpDistrict.SelectedValue = application.compnyDistrict_ID.ToString();
                txtCmpPinCode.Text = application.CompnyPinCode.ToString();


                // btnback.Visible = false;
                Int64 batchID = Convert.ToInt64(ddlBatch.SelectedValue);
                //    if (context.NielitCentreBatchs.Where(s => s.ID == batchID && (s.startDate < System.DateTime.Now && s.endDate > System.DateTime.Now)).Count() == 0)
                if (context.NielitCentreBatchs.Where(s => s.ID == batchID).Count() == 0)
                {
                    //ddlCourseComplete.Enabled = false;
                    RdoAffInstOrNonAffInst.Enabled = false;
                    ddlSalutaionName.Enabled = false;
                    txtAppName.Enabled = false;
                    ddlWheatherPojectStu.Enabled = false;
                    if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName) == true)
                    {
                        trfather.Visible = true;
                        trmother.Visible = true;
                        txtFatherName.Enabled = false;
                        txtMotherName.Enabled = false;
                        Rdoownertype.SelectedValue = "P";
                        Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                        txtFatherName.Text = string.IsNullOrEmpty(application.FatherName) == false && !string.IsNullOrWhiteSpace(application.FatherName) ? GetInitCap(application.FatherName) : "";
                        txtMotherName.Text = string.IsNullOrEmpty(application.MotherName) == false && !string.IsNullOrWhiteSpace(application.MotherName) ? GetInitCap(application.MotherName) : "";
                    }
                    else
                    {
                        trfather.Visible = false;
                        trmother.Visible = false;
                        TxtGuardianName.Enabled = false;
                        Rdoownertype.SelectedValue = "G";
                        Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                        TxtGuardianName.Text = GetInitCap(application.GuardianName);
                    }

                    ddl_gender.Enabled = false;
                    txtDob.Enabled = false;

                    ddlCategory.Enabled = false;
                    ddlMStatus.Enabled = false;
                    Rdhandicapped.Enabled = false;
                    Rdexserviceman.Enabled = false;
                    ddlReligion.Enabled = false;
                    RdisEWS.Enabled = false;

                    TxtSTDcode.Enabled = false;
                    txtCorPhoneNo.Enabled = false;
                    txtCorMobileNo.Enabled = false;
                    txtEmailId.Enabled = false;
                    //txtaadhar.Enabled = false;

                    TxtPerAddressLine1.Enabled = false;
                    TxtPerAddressLine2.Enabled = false;
                    TxtPerAddressLine3.Enabled = false;
                    TxtPerCity.Enabled = false;
                    ddlPState.Enabled = false;


                    ddlPdistrict.Enabled = false;
                    TxtPpincode.Enabled = false;

                    TxtCorAddressLine1.Enabled = false;
                    TxtCorAddressLine2.Enabled = false;
                    TxtCorAddressLine3.Enabled = false;
                    TxtCorCity.Enabled = false;
                    ddlCorState.Enabled = false;

                    UidTypeDdl.Enabled = false;
                    UidNumberTxt.Enabled = false;
                    ddlcordistrict.Enabled = false;
                    txtCorPinCode.Enabled = false;

                    trCompanyDetails.Visible = true;
                    trwhetherCourseComplete.Visible = true;
                    trwhetherCertificateIssued.Visible = true;
                    trcertificateIssueDate.Visible = true;

                    txtcertificateIssueDate.Visible = true;
                    trWhetherPlaced.Visible = true;
                    trWhetherPlacedDate.Visible = true;
                    //trCompanyHead.Visible = true;
                    //trCompanyName.Visible = true;
                    //trCmpAdd1.Visible = true;
                    //trCmpAdd2.Visible = true;
                    //trCmpAdd3.Visible = true;
                    //trCmyCityName.Visible = true;
                    //trCmpState.Visible = true;
                    //trCmpDistrict.Visible = true;
                    //trCmpPinCode.Visible = true;
                    //throw new Exception("This Course is Already Verifed");
                }

                if (context.NielitCentreStudent.Any(s => (s.whether_Course_Complete == true && s.whether_Certificate_Issued == false && s.ID == Id)))
                {
                    ddlCourseComplete.Enabled = false;
                    RdoAffInstOrNonAffInst.Enabled = false;
                    ddlSalutaionName.Enabled = false;
                    txtAppName.Enabled = false;
                    ddlWheatherPojectStu.Enabled = false;
                    if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName) == true)
                    {
                        trfather.Visible = true;
                        trmother.Visible = true;
                        txtFatherName.Enabled = false;
                        txtMotherName.Enabled = false;
                        Rdoownertype.SelectedValue = "P";
                        Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                        txtFatherName.Text = string.IsNullOrEmpty(application.FatherName) == false && !string.IsNullOrWhiteSpace(application.FatherName) ? GetInitCap(application.FatherName) : "";
                        txtMotherName.Text = string.IsNullOrEmpty(application.MotherName) == false && !string.IsNullOrWhiteSpace(application.MotherName) ? GetInitCap(application.MotherName) : "";
                    }
                    else
                    {
                        trfather.Visible = false;
                        trmother.Visible = false;
                        TxtGuardianName.Enabled = false;
                        Rdoownertype.SelectedValue = "G";
                        Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                        TxtGuardianName.Text = GetInitCap(application.GuardianName);
                    }

                    ddl_gender.Enabled = false;
                    txtDob.Enabled = false;

                    ddlCategory.Enabled = false;
                    ddlMStatus.Enabled = false;
                    Rdhandicapped.Enabled = false;
                    Rdexserviceman.Enabled = false;
                    ddlReligion.Enabled = false;

                    TxtSTDcode.Enabled = false;
                    txtCorPhoneNo.Enabled = false;
                    txtCorMobileNo.Enabled = false;
                    txtEmailId.Enabled = false;
                    RdisEWS.Enabled = false;
                    //txtaadhar.Enabled = false;

                    TxtPerAddressLine1.Enabled = false;
                    TxtPerAddressLine2.Enabled = false;
                    TxtPerAddressLine3.Enabled = false;
                    TxtPerCity.Enabled = false;
                    ddlPState.Enabled = false;


                    ddlPdistrict.Enabled = false;
                    TxtPpincode.Enabled = false;

                    TxtCorAddressLine1.Enabled = false;
                    TxtCorAddressLine2.Enabled = false;
                    TxtCorAddressLine3.Enabled = false;
                    TxtCorCity.Enabled = false;
                    ddlCorState.Enabled = false;

                    UidTypeDdl.Enabled = false;
                    UidNumberTxt.Enabled = false;
                    ddlcordistrict.Enabled = false;
                    txtCorPinCode.Enabled = false;

                    trCompanyDetails.Visible = true;
                    trwhetherCourseComplete.Visible = true;
                    trwhetherCertificateIssued.Visible = true;
                    trcertificateIssueDate.Visible = true;
                    txtcertificateIssueDate.Visible = true;

                    trWhetherPlaced.Visible = true;
                    trWhetherPlacedDate.Visible = true;
                    //trCompanyHead.Visible = true;
                    //trCompanyName.Visible = true;
                    //trCmpAdd1.Visible = true;
                    //trCmpAdd2.Visible = true;
                    //trCmpAdd3.Visible = true;
                    //trCmyCityName.Visible = true;
                    //trCmpState.Visible = true;
                    //trCmpDistrict.Visible = true;
                    //trCmpPinCode.Visible = true;
                    //throw new Exception("This Course is Already Verifed");
                }
                else if (context.NielitCentreStudent.Any(s => (s.whether_Course_Complete == true && s.whether_Certificate_Issued == true && s.ID == Id)))
                {
                    ddlCourseComplete.Enabled = false;
                    ddlSalutaionName.Enabled = false;
                    txtAppName.Enabled = false;

                    if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName) == true)
                    {
                        trfather.Visible = true;
                        trmother.Visible = true;
                        txtFatherName.Enabled = false;
                        txtMotherName.Enabled = false;
                        Rdoownertype.SelectedValue = "P";
                        Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                        txtFatherName.Text = string.IsNullOrEmpty(application.FatherName) == false && !string.IsNullOrWhiteSpace(application.FatherName) ? GetInitCap(application.FatherName) : "";
                        txtMotherName.Text = string.IsNullOrEmpty(application.MotherName) == false && !string.IsNullOrWhiteSpace(application.MotherName) ? GetInitCap(application.MotherName) : "";
                    }
                    else
                    {
                        trfather.Visible = false;
                        trmother.Visible = false;
                        TxtGuardianName.Enabled = false;
                        Rdoownertype.SelectedValue = "G";
                        Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                        TxtGuardianName.Text = GetInitCap(application.GuardianName);
                    }

                    ddl_gender.Enabled = false;
                    txtDob.Enabled = false;
                    RdisEWS.Enabled = false;

                    ddlCategory.Enabled = false;
                    ddlMStatus.Enabled = false;
                    Rdhandicapped.Enabled = false;
                    Rdexserviceman.Enabled = false;
                    ddlReligion.Enabled = false;
                    ddlCenter.Enabled = false;
                    RdoAffInstOrNonAffInst.Enabled = false;

                    TxtSTDcode.Enabled = false;
                    txtCorPhoneNo.Enabled = false;
                    txtCorMobileNo.Enabled = false;
                    txtEmailId.Enabled = false;
                    //txtaadhar.Enabled = false;

                    TxtPerAddressLine1.Enabled = false;
                    TxtPerAddressLine2.Enabled = false;
                    TxtPerAddressLine3.Enabled = false;
                    TxtPerCity.Enabled = false;
                    ddlPState.Enabled = false;


                    ddlPdistrict.Enabled = false;
                    TxtPpincode.Enabled = false;

                    TxtCorAddressLine1.Enabled = false;
                    TxtCorAddressLine2.Enabled = false;
                    TxtCorAddressLine3.Enabled = false;
                    TxtCorCity.Enabled = false;
                    ddlCorState.Enabled = false;

                    UidTypeDdl.Enabled = false;
                    UidNumberTxt.Enabled = false;
                    ddlcordistrict.Enabled = false;
                    txtCorPinCode.Enabled = false;
                    txtcertificateIssueDate.Enabled = false;

                    trwhetherCourseComplete.Visible = true;
                    trwhetherCertificateIssued.Visible = true;
                    trcertificateIssueDate.Visible = true;
                    txtcertificateIssueDate.Visible = true;

                    ddlCourseComplete.Enabled = false;
                    ddlwhetherCertificateIssued.Enabled = false;
                    txtcertificateIssueDate.Enabled = false;

                    trCompanyDetails.Visible = true;
                    trWhetherPlaced.Visible = true;
                    trWhetherPlacedDate.Visible = true;
                    //trPersonDetails.Visible = true;
                    //tr2.Visible = true;
                    //trCompanyHead.Visible = true;
                    //trCompanyName.Visible = true;
                    //trCmpAdd1.Visible = true;
                    //trCmpAdd2.Visible = true;
                    //trCmpAdd3.Visible = true;
                    //trCmyCityName.Visible = true;
                    //trCmpState.Visible = true;
                    //trCmpDistrict.Visible = true;
                    //trCmpPinCode.Visible = true;

                    ddlWhetherPlaced_SelectedIndexChanged(ddlWhetherPlaced, EventArgs.Empty);


                }

                else
                {
                    //trCompanyDetails.Visible = true;
                    trwhetherCourseComplete.Visible = true;
                    trwhetherCertificateIssued.Visible = true;
                    trcertificateIssueDate.Visible = true;
                    txtcertificateIssueDate.Visible = true;

                    trWhetherPlaced.Visible = true;
                    trWhetherPlacedDate.Visible = true;
                    //trCompanyHead.Visible = true;
                    //trCompanyName.Visible = true;
                    //trCmpAdd1.Visible = true;
                    //trCmpAdd2.Visible = true;
                    //trCmpAdd3.Visible = true;
                    //trCmyCityName.Visible = true;
                    //trCmpState.Visible = true;
                    //trCmpDistrict.Visible = true;
                    //trCmpPinCode.Visible = true;

                }
                //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(application.Name, "Admin/NielitCentreStudent.aspx?" + Request.QueryString.ToString(), ""));
                //Get last modified date of current record and save it in ViewState object.
                //ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                //Create an object of record to be modified and assign properties to relevant fields.

            }
            ;
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
            // context.Dispose();
        }
    }


    protected void BindGridView()
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        try
        {
            //Int64 instituteId = Convert.ToInt64(hfAccreID.Value);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                Int64 stateID = 0;
                Int32 CourseCategoryID = 0;
                Int32 courseID = 0;
                Int32 examCenterTypeID = 0;
                if (ddlName.SelectedValue != "0")
                    stateID = Convert.ToInt64(ddlName.SelectedValue);
                //if (ddlficoursecategory.SelectedValue != "0")
                //    CourseCategoryID = Convert.ToInt32(ddlficoursecategory.SelectedValue);
                //if (ddlflcourse.SelectedValue != "0")
                //    courseID = Convert.ToInt32(ddlflcourse.SelectedValue);
                //if (ddlflexcentretype.SelectedValue != "0")
                //    examCenterTypeID = Convert.ToInt32(ddlflexcentretype.SelectedValue);
                var CentreBatchs = from s in context.NielitCentreStudent
                                   where s.InstituteID == UserRefNumber
                                   select new
                                   {
                                       ID = s.ID,
                                       Name = s.Name,
                                       FatherName = s.FatherName,
                                       MotherName = s.MotherName,
                                       GuardianName = s.GuardianName,
                                       DateOfBirth = s.DateOfBirth,
                                       //IsActive = s.IsVerified ? "Active" : "InActive", 
                                   };

                DataTable DT = new DataTable();


                con.Open();
                SqlParameter param;
                using (SqlCommand Cmm = new SqlCommand("BindGridNIELITStudent", con))
                {
                    Cmm.CommandType = CommandType.StoredProcedure;
                    param = new SqlParameter("@enterby", loginUserNo);
                    Cmm.Parameters.Add(param);
                    SqlDataAdapter Sda = new SqlDataAdapter(Cmm);


                    Sda.Fill(DT);
                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    string expression = "[Batch] like '%" + searchString + "%'";

                    if (DT.Select(expression).Count() > 0)
                        DT = DT.Select(expression).CopyToDataTable();
                    else
                    {
                        ShowAlert("No record found", true);

                    }

                }

                if (ddlName.SelectedValue.ToString() != "0")
                {
                    string expression = "[BatchId]=" + Convert.ToInt32(ddlName.SelectedValue);
                    //ShowAlert(DT.Select(expression).Count().ToString(),true);
                    if (DT.Select(expression).Count() > 0)
                    {
                        DT = DT.Select(expression).CopyToDataTable();
                        // ShowAlert(DT.Select().Count().ToString(), true);
                    }
                    else
                    {
                        ShowAlert("No record found", true);

                    }
                }

                PagingBar1.Bind(DT, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();


                if (!String.IsNullOrEmpty(searchString))
                {
                    CentreBatchs = CentreBatchs.Where(s => s.Name.ToUpper().Contains(searchString));
                }


                if (courseID != 0)
                {
                    CentreBatchs = CentreBatchs.Where(s => s.ID == courseID);
                }


                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "ID":
                            if (sortOrder == "DESC")
                                CentreBatchs = CentreBatchs.OrderByDescending(s => s.ID);
                            else
                                CentreBatchs = CentreBatchs.OrderBy(s => s.ID);
                            break;
                        case "Name":
                            if (sortOrder == "DESC")
                                CentreBatchs = CentreBatchs.OrderByDescending(s => s.Name);
                            else
                                CentreBatchs = CentreBatchs.OrderBy(s => s.Name);
                            break;
                        case "FatherName":
                            if (sortOrder == "DESC")
                                CentreBatchs = CentreBatchs.OrderByDescending(s => s.FatherName);
                            else
                                CentreBatchs = CentreBatchs.OrderBy(s => s.FatherName);
                            break;
                        case "MotherName":
                            if (sortOrder == "DESC")
                                CentreBatchs = CentreBatchs.OrderByDescending(s => s.MotherName);
                            else
                                CentreBatchs = CentreBatchs.OrderBy(s => s.MotherName);
                            break;
                        case "GuardianName":
                            if (sortOrder == "DESC")
                                CentreBatchs = CentreBatchs.OrderByDescending(s => s.GuardianName);
                            else
                                CentreBatchs = CentreBatchs.OrderBy(s => s.GuardianName);
                            break;
                        case "DateOfBirth":
                            if (sortOrder == "DESC")
                                CentreBatchs = CentreBatchs.OrderByDescending(s => s.DateOfBirth);
                            else
                                CentreBatchs = CentreBatchs.OrderBy(s => s.DateOfBirth);
                            break;
                        default:
                            CentreBatchs = CentreBatchs.OrderBy(s => s.Name);
                            break;
                    }
                }
                //if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                //{
                //    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseCategoryID).Distinct();
                //    CentreBatchs = CentreBatchs.Where(a => roleCourses.Contains(a.));
                //}
                //if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
                //{
                //    var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                //    CentreBatchs = CentreBatchs.Where(a => roleCourses.Contains(a.CourseID));
                //}
                //PagingBar1.Bind(CentreBatchs, ref gvMain);
                //uPnlGrid.Update();
                //uPnlNavigation.Update();

                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    gvMain.Columns[7].Visible = false;
                }
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
        finally
        {
            con.Close();
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
            if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to add new record.", true);
                return;
            }
            BindEditNewModeData();
            //ddlExamCentreType.SelectedValue = "3";
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Nielit Centre Student";

            //Updating Breadcrumb
            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Exam Centre", "#", ""));
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Nielit Centre Student", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitCentreStudent.aspx?ID=" + Request.QueryString["CourseID"].ToString()), true);
            }
            else
            {
                Response.Redirect("NielitCentreStudent.aspx", true);
            }
        }
    }

    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            // Done by Jyoti 06 May 2021
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
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

    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            // Done by Jyoti 06 May 2021

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
            ddlName.SelectedValue = "0";
            //ddlficoursecategory.SelectedValue = "0";
            //ddlficoursecategory_SelectedIndexChanged(ddlficoursecategory, EventArgs.Empty);
            //ddlflexcentretype.SelectedValue = "0";
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
                NielitCentreStudent CenterStudent = context.NielitCentreStudent.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                context.NielitCentreStudent.Remove(CenterStudent);
                context.SaveChanges();
                BindGridView();
                ShowAlert("Record deleted successfully.", true);
                hfActionID.Value = "";
            }
            ;
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
                if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                {
                    href += "&CourseId=" + Request.QueryString["CourseId"].ToString();
                }
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(href);
                HyperLink h2 = (HyperLink)e.Row.Cells[2].Controls[0];
                h2.NavigateUrl = hl.NavigateUrl;
                HyperLink h3 = (HyperLink)e.Row.Cells[3].Controls[0];
                h3.NavigateUrl = hl.NavigateUrl;
                HyperLink h4 = (HyperLink)e.Row.Cells[4].Controls[0];
                h4.NavigateUrl = hl.NavigateUrl;

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
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        // Done by Jyoti 07 May 2021

        User objUser;
        int loginUserNo = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        using (EConnectContext context1 = new EConnectContext())
        {
            objUser = new EConnect.URM.User();


            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            int UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);

            NIELITMISContext context = new NIELITMISContext();
            try
            {
                if (count <= 0)
                    count = 10;
                //Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
                List<String> items = new List<String>();
                string searchString = prefixText.Trim().ToUpper();

                var Examcentre = from s in context.NielitCentreBatchs
                                 where s.centreID == UserRefNumber || s.subCentreID == UserRefNumber
                                 select new { Name = s.Name };


                if (!String.IsNullOrEmpty(searchString))
                {
                    Examcentre = Examcentre.Where(s => s.Name.ToUpper().Contains(searchString));
                }
                Examcentre = Examcentre.OrderBy(s => s.Name);

                foreach (var c in Examcentre)
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
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("NielitCentreStudent.aspx", true);

    }

    protected void bindPaymentOption()
    {
        using (EConnectContext context = new EConnectContext())
        {
            Course currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["id"]));
            string CourseCatg = currentCourse.CourseCategory.Name.ToString();

        }
    }

    protected void bindExamName(Int32 ApplicantTypeId)
    {
        try
        {
            Int32 courseID = 0;
            Int32 NormalactivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
            Int32 LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplicationWithLateFee);
            using (EConnectContext context = new EConnectContext())
            {
                Int32 examID = 0;
                if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    courseID = Convert.ToInt32(Request.QueryString["id"]);
                }
                else if (!String.IsNullOrEmpty(Request.QueryString["Appid"]))
                {
                    Int64 appID = Convert.ToInt64(Request.QueryString["Appid"]);
                    var app = context.CourseRegistrationApplications.Find(appID);
                    courseID = app.CourseID;

                }
                var LateFeeExam = (from e in context.CutOffDates
                                   join i in context.Exams on e.ExamID equals i.ID
                                   where e.CourseID == courseID
                                   && e.ApplicantTypeID == ApplicantTypeId
                                   && e.ActivityID == LateFeeActivityId
                                   && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                   orderby e.EfferctiveDate ascending
                                   select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
                if (LateFeeExam.Count() > 0)//If  applicable for late fee ?
                {

                    if (LateFeeExam != null)
                    {
                        //LblExamName.Text = LateFeeExam.FirstOrDefault().ExamName;
                        examID = LateFeeExam.FirstOrDefault().ExamID;
                        btnSave.Visible = true;
                    }
                }
                else if (LateFeeExam.Count() <= 0)//If  not applicable for late fee ?
                {

                    var NormalFeeExam = (from e in context.CutOffDates
                                         join i in context.Exams on e.ExamID equals i.ID
                                         where e.CourseID == courseID
                                         && e.ApplicantTypeID == ApplicantTypeId
                                         && e.ActivityID == NormalactivityId
                                         && e.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                         orderby e.EfferctiveDate ascending
                                         select new { ExamID = e.ExamID, ExamName = i.Name, ExamDate = i.ExamStartDate }).Take(1);
                    if (NormalFeeExam.Count() > 0)
                    {
                        //LblExamName.Text = NormalFeeExam.FirstOrDefault().ExamName;
                        examID = NormalFeeExam.FirstOrDefault().ExamID;
                        btnSave.Visible = true;
                    }

                }
                ViewState["ExamID"] = examID.ToString();
                //ShowFeeDetail(examID);
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ShowData()
    {
        try
        {
            if (!String.IsNullOrEmpty(Request.QueryString["key"]))
            {
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    Int64 applID = Convert.ToInt64(Request.QueryString["key"]);
                    //This Query will get all the information of Applied Canditate by generated Application id                                     
                    var application = context.NielitCentreStudent.Find(applID);

                    //Candidate Personal Detail...

                    ddlSalutaionName.SelectedValue = application.Salutation.ToString();
                    txtAppName.Text = GetInitCap(application.Name.ToString());
                    lblname.Text = txtAppName.Text;

                    ddlCenter.SelectedValue = application.InstituteID.ToString();

                    if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName) == true)
                    {
                        trfather.Visible = true;
                        trmother.Visible = true;
                        trguardian.Visible = false;
                        Rdoownertype.SelectedValue = "P";
                        Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                        txtFatherName.Text = string.IsNullOrEmpty(application.FatherName) == false && !string.IsNullOrWhiteSpace(application.FatherName) ? GetInitCap(application.FatherName) : "";
                        txtMotherName.Text = string.IsNullOrEmpty(application.MotherName) == false && !string.IsNullOrWhiteSpace(application.MotherName) ? GetInitCap(application.MotherName) : "";
                    }
                    else
                    {
                        trfather.Visible = false;
                        trmother.Visible = false;
                        trguardian.Visible = true;
                        Rdoownertype.SelectedValue = "G";
                        Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                        TxtGuardianName.Text = GetInitCap(application.GuardianName);
                    }

                    ddl_gender.SelectedValue = application.Gender;
                    txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");
                    //txtBodyMark.Text = string.IsNullOrEmpty(application.BodyMark) == false && !string.IsNullOrWhiteSpace(application.BodyMark) ? application.BodyMark : "";
                    ddlCategory.SelectedValue = application.CastCategoryID.ToString();
                    ddlMStatus.SelectedValue = application.MaritalStatusID.ToString();
                    if (application.IsHandicaped.ToString() == "True")
                    {
                        Rdhandicapped.SelectedValue = "Y";
                    }
                    else
                    {
                        Rdhandicapped.SelectedValue = "N";
                    }
                    if (application.IsExServicemane.ToString() == "True")
                    {
                        Rdexserviceman.SelectedValue = "Y";
                    }
                    else
                    {
                        Rdexserviceman.SelectedValue = "N";
                    }
                    ddlReligion.SelectedValue = application.ReligionID.ToString();
                    //Candidate Contact Detail...

                    TxtSTDcode.Text = application.PhoneNumber.HasValue && application.PhoneNumber != 0 ? "0" + application.StdNumber.ToString() : "";
                    txtCorPhoneNo.Text = application.PhoneNumber.ToString();
                    txtCorMobileNo.Text = application.MobileNumber.ToString();
                    txtEmailId.Text = application.EmailAddress.ToString();
                    RdisEWS.SelectedValue = application.Is_EWS.ToString();

                    //Candidate Address Detail...
                    TxtPerAddressLine1.Text = string.IsNullOrEmpty(application.PerAddressLine1) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine1) ? GetInitCap(application.PerAddressLine1) : "";
                    TxtPerAddressLine2.Text = string.IsNullOrEmpty(application.PerAddressLine2) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine2) ? GetInitCap(application.PerAddressLine2) : "";
                    TxtPerAddressLine3.Text = string.IsNullOrEmpty(application.PerAddressLine3) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine3) ? GetInitCap(application.PerAddressLine3) : "";
                    TxtPerCity.Text = string.IsNullOrEmpty(application.PerCityName) == false && !string.IsNullOrWhiteSpace(application.PerCityName) ? GetInitCap(application.PerCityName) : "";
                    ddlPState.SelectedValue = application.PerStateID != null && application.PerStateID != 0 ? application.PerStateID.ToString() : "0";
                    int id1 = Convert.ToInt32(ddlPState.SelectedValue);
                    ddlPState_SelectedIndexChanged(ddlPState, EventArgs.Empty);
                    ddlPdistrict.SelectedValue = application.PerDistrictID.ToString();
                    TxtPpincode.Text = application.PerPinCode.ToString();

                    TxtCorAddressLine1.Text = string.IsNullOrEmpty(application.CorAddressLine1) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine1) ? GetInitCap(application.CorAddressLine1) : "";
                    TxtCorAddressLine2.Text = string.IsNullOrEmpty(application.CorAddressLine2) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine2) ? GetInitCap(application.CorAddressLine2) : "";
                    TxtCorAddressLine3.Text = string.IsNullOrEmpty(application.CorAddressLine3) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine3) ? GetInitCap(application.CorAddressLine3) : "";
                    TxtCorCity.Text = string.IsNullOrEmpty(application.CorCityName) == false && !string.IsNullOrWhiteSpace(application.CorCityName) ? GetInitCap(application.CorCityName) : "";
                    ddlCorState.SelectedValue = application.CorStateID.ToString();
                    int id2 = Convert.ToInt32(ddlCorState.SelectedValue);
                    ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                    ddlcordistrict.SelectedValue = application.CorDistrictID.ToString();
                    txtCorPinCode.Text = application.CorPinCode.ToString();

                    //btnback.Visible = false;
                }
                ;

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void UpdateData()
    {
        try
        {
            // using (TransactionScope scope = new TransactionScope())
            {
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    Int64 applID = Convert.ToInt64(Request.QueryString["Key"]);
                    //Course currentCourse = context.Courses.Find(Convert.ToInt32(Request.QueryString["id"]));

                    //This Query will get all the information of Applied Canditate by generated Application id
                    var objRegistration = context.NielitCentreStudent.Find(applID);

                    objRegistration.CourseID = Convert.ToInt64(ddlCourse.SelectedValue);
                    objRegistration.batch_ID = Convert.ToInt64(ddlBatch.SelectedValue);
                    objRegistration.InstituteID = Convert.ToInt64(ddlCenter.SelectedValue);

                    if (RdisEWS.SelectedValue == "Y")
                        objRegistration.Is_EWS = true;
                    else
                        objRegistration.Is_EWS = false;

                    string salutation = ddlSalutaionName.SelectedItem.Text;
                    salutation = salutation.Substring(0, salutation.IndexOf('/'));

                    objRegistration.Salutation = salutation.Trim();
                    objRegistration.Name = txtAppName.Text.Trim().ToString();

                 

                if (Rdoownertype.SelectedValue == "P")
                        {
                            objRegistration.FatherName = txtFatherName.Text.Trim();
                            objRegistration.MotherName = txtMotherName.Text.Trim();
                            objRegistration.GuardianName = null;
                        }
                        else if (Rdoownertype.SelectedValue == "G")
                        {
                            objRegistration.GuardianName = TxtGuardianName.Text.Trim();
                            objRegistration.FatherName = null;
                            objRegistration.MotherName = null;
                        }
                 


                    string gender = ddl_gender.SelectedValue;
                    objRegistration.Gender = gender.Trim();
                    //                    gender = gender.Substring(0, gender.IndexOf('/'));
                    //                   objRegistration.Gender = gender.Trim();
                    objRegistration.MaritalStatusID = Convert.ToInt32(ddlMStatus.SelectedValue);
                    objRegistration.DateOfBirth = Convert.ToDateTime(txtDob.Text.ToString());
                    objRegistration.CastCategoryID = Convert.ToInt32(ddlCategory.SelectedValue);

                    //To_Enable_Update_Caste_Certificate_23_10_2024_Start

                    //using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
                    //{
                    //    using (var command = new SqlCommand("getUploadedCasteCertificate", con))
                    //    {

                    //        string output = string.Empty;


                    //        command.CommandType = CommandType.StoredProcedure;

                    //        command.Parameters.Clear();

                    //        command.Parameters.AddWithValue("@refno", objRegistration.Number);


                    //        SqlParameter outputParam = new SqlParameter("@Status", SqlDbType.NVarChar, 100);
                    //        outputParam.Direction = ParameterDirection.Output;
                    //        command.Parameters.Add(outputParam);

                    //        con.Open();

                    //        byte[] data = (byte[])command.ExecuteScalar();
                    //        con.Close();

                    //        output = command.Parameters["@Status"].Value.ToString();
                    //        if (output == "Success")
                    //        {
                    //            if (currentRoleId != 28)
                    //            {
                    //                fileCertificate.Enabled = false;
                    //                // Response.Write("Sorry! You don't have rights  to Update Caste Certificate");
                    //                // Response.End();
                    //                regCertificate.ErrorMessage = "Certificate Already Uploaded";
                    //            }

                    //        }
                    //        else if (output == "Exception")
                    //            ShowAlert("Exception Occur");
                    //    }
                    //}

                    //To_Enable_Update_Caste_Certificate_23_10_2024_End

                    //To_Upload_Caste_Certificate_18_10_2024_Start

                    //if (fileCertificate.Enabled == true)
                    //{
                    //if (whetherFileUpload == true)

                    //}

                    //To_Upload_Caste_Certificate_18_10_2024_End

                    if (Rdhandicapped.SelectedValue == "Y")
                    {
                        objRegistration.IsHandicaped = true;
                    }
                    else
                    {
                        objRegistration.IsHandicaped = false;
                    }
                    if (Rdexserviceman.SelectedValue == "Y")
                    {
                        objRegistration.IsExServicemane = true;
                    }
                    else
                    {
                        objRegistration.IsExServicemane = false;
                    }

                    ////objRegistration.BodyMark = txtBodyMark.Text;

                    //Contact Details
                    if (String.IsNullOrEmpty(TxtSTDcode.Text) == false && string.IsNullOrEmpty(txtCorPhoneNo.Text) == false)
                    {
                        objRegistration.StdNumber = Convert.ToInt32(TxtSTDcode.Text);
                        objRegistration.PhoneNumber = Convert.ToInt32(txtCorPhoneNo.Text);
                    }
                    objRegistration.MobileNumber = Convert.ToInt64(txtCorMobileNo.Text);
                    objRegistration.EmailAddress = txtEmailId.Text;


                    //Permanent Address Details
                    objRegistration.PerAddressLine1 = TxtPerAddressLine1.Text;
                    objRegistration.PerAddressLine2 = TxtPerAddressLine2.Text;
                    objRegistration.PerAddressLine3 = TxtPerAddressLine3.Text;
                    objRegistration.PerCountryID = 0;
                    objRegistration.PerCityName = TxtPerCity.Text;
                    objRegistration.PerStateID = Convert.ToInt32(ddlPState.SelectedValue);
                    objRegistration.PerDistrictID = Convert.ToInt32(ddlPdistrict.SelectedValue);
                    objRegistration.PerPinCode = Convert.ToInt32(TxtPpincode.Text);


                    //Correspondence Address Details
                    if (chkSame.Checked)
                    {
                        objRegistration.CorAddressLine1 = TxtPerAddressLine1.Text;
                        objRegistration.CorAddressLine2 = TxtPerAddressLine2.Text;
                        objRegistration.CorAddressLine3 = TxtPerAddressLine3.Text;
                        objRegistration.CorCountryID = 0;
                        objRegistration.CorCityName = TxtPerCity.Text;
                        objRegistration.CorStateID = Convert.ToInt32(ddlPState.SelectedValue);
                        objRegistration.CorDistrictID = Convert.ToInt32(ddlPdistrict.SelectedValue);
                        objRegistration.CorPinCode = Convert.ToInt32(TxtPpincode.Text);
                    }
                    else
                    {
                        objRegistration.CorAddressLine1 = Server.HtmlEncode(TxtCorAddressLine1.Text);
                        objRegistration.CorAddressLine2 = TxtCorAddressLine2.Text;
                        objRegistration.CorAddressLine3 = TxtCorAddressLine3.Text;
                        objRegistration.CorCountryID = 0;
                        objRegistration.CorCityName = TxtCorCity.Text;
                        objRegistration.CorStateID = Convert.ToInt32(ddlCorState.SelectedValue);
                        objRegistration.CorDistrictID = Convert.ToInt32(ddlcordistrict.SelectedValue);
                        objRegistration.CorPinCode = Convert.ToInt32(txtCorPinCode.Text);
                    }
                    if (ddlProcname.SelectedValue == "1")
                    {
                        int x = checkAspirationalDistrict();
                        if (x != 1)
                        {
                            ShowAlert("Project is aspirational, wrong district selected, Please check");
                            return;
                        }
                    }

                    // objRegistration.IsVerifiedByInstitute = false;
                    DateTime Dob = Convert.ToDateTime(txtDob.Text);
                    Int32 ReligionId = Convert.ToInt32(ddlReligion.SelectedValue);
                    Int32 castCategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                    Boolean IsHandicapped = Rdhandicapped.SelectedValue == "Y" ? true : false;

                    objRegistration.ReligionID = ReligionId;

                    //aadhar details               

                    objRegistration.UIDType = Convert.ToInt32(UidTypeDdl.SelectedValue);


                    //Added for aadhaar Encryption
                    // if (UidTypeDdl.SelectedValue.ToString() == "1")
                    //     objRegistration.UIDNumber = EConnect.Utils.Security.Encryption.Encrypt(UidNumberTxt.Text.Trim().ToUpper());
                    // else
                    //     objRegistration.UIDNumber = UidNumberTxt.Text.Trim().ToUpper();
                    //  objRegistration.UIDNumber = UidNumberTxt.Text.Trim().ToUpper();

                    // objRegistration.AadharVerfied = false;

                    if (ddlCourseComplete.SelectedValue == "1")
                    {
                        objRegistration.whether_Course_Complete = true;
                    }
                    else
                    {
                        objRegistration.whether_Course_Complete = false;
                    }

                    if (ddlwhetherCertificateIssued.SelectedValue == "1")
                    {
                        objRegistration.whether_Certificate_Issued = true;

                        if (isBlank(txtcertificateIssueDate))
                        {
                            DateTime certDate = Convert.ToDateTime(txtcertificateIssueDate.Text.ToString());
                            Int64 batchID = Convert.ToInt64(ddlBatch.SelectedValue);

                            if (context.NielitCentreBatchs.Where(s => s.ID == batchID && (s.endDate <= certDate)).Count() > 0)
                            {

                                objRegistration.certificate_Issue_Date = Convert.ToDateTime(txtcertificateIssueDate.Text);
                                //objRegistration.certificate_Issue_Date = Convert.ToDateTime(PlacementDates.ToString());
                            }
                            else
                            {
                                var CenterBatch = (from s in context.NielitCentreBatchs
                                                   where s.ID == batchID
                                                   select s).FirstOrDefault();

                                lblerror.Visible = true;
                                lblerror.Text = "Certificate Issue date greater than or equal to " + CenterBatch.endDate.ToString("dd-MMM-yyyy");
                                txtcertificateIssueDate.Focus();
                                //ShowAlert("Certificate Issue date grater than or equal to " + CenterBatch.endDate.ToString("dd-MMM-yyyy"));
                                return;
                            }
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "Please Enter Certificate Issue Date.";
                            txtcertificateIssueDate.Focus();
                            return;
                        }
                    }
                    else
                    {
                        if (txtcertificateIssueDate.Text == "")
                        {
                            objRegistration.whether_Certificate_Issued = false;
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "Please select Whether Certificate Issue.";
                            ddlwhetherCertificateIssued.Focus();
                            return;
                        }
                    }


                    if (ddlWhetherPlaced.SelectedValue == "1")
                    {
                        objRegistration.whetherPlaced = true;
                        if (isBlank(txtPlacementDate))
                        {
                            Int64 batchID = Convert.ToInt64(ddlBatch.SelectedValue);
                            DateTime certDate = Convert.ToDateTime(txtPlacementDate.Text.ToString());
                            if (context.NielitCentreBatchs.Where(s => s.ID == batchID && (s.endDate <= certDate)).Count() > 0)
                            {
                                objRegistration.placementDate = Convert.ToDateTime(txtPlacementDate.Text);
                            }
                            else
                            {

                                var CenterBatch = (from s in context.NielitCentreBatchs
                                                   where s.ID == batchID
                                                   select s).FirstOrDefault();

                                lblerror.Visible = true;
                                lblerror.Text = "Placement date greater than or equal to " + CenterBatch.endDate.ToString("dd-MMM-yyyy");
                                txtPlacementDate.Focus();
                                return;
                            }
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "Please Enter Whether Placement Date with addition of Placement Details";
                            txtPlacementDate.Focus();
                            return;
                        }
                    }
                    else
                    {
                        if (txtPlacementDate.Text == "")
                        {
                            objRegistration.whetherPlaced = false;
                            objRegistration.placementDate = null;
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "Please select Wheather placed.";
                            txtPlacementDate.Focus();
                            return;
                        }


                    }

                    if (ddlWhetherPlaced.SelectedValue == "1")
                    {
                        if (ddlCompanyName.SelectedValue != "0")
                        {
                            //objRegistration.company_Name = txtCompanyName.Text;
                            objRegistration.company_Name = Convert.ToInt64(ddlCompanyName.SelectedValue);
                            objRegistration.comapny_Address = txtCmpAdd1.Text;
                            objRegistration.comapny_Address2 = txtCmpAdd2.Text;
                            objRegistration.comapny_Address3 = txtCmpAdd3.Text;
                            objRegistration.CompnyCityName = txtCmyCityName.Text;
                            objRegistration.CompnyCountry_ID = 0;
                            objRegistration.compnyState_ID = Convert.ToInt32(ddlCmpState.SelectedValue);
                            objRegistration.compnyDistrict_ID = Convert.ToInt32(ddlCmpDistrict.SelectedValue);
                            objRegistration.CompnyPinCode = Convert.ToInt32(txtCmpPinCode.Text);
                            objRegistration.CompnyPinCode = Convert.ToInt32(txtCmpPinCode.Text);
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "Please Select Company Name.";
                            ddlCompanyName.Focus();
                            return;
                        }
                    }




                    objRegistration.affidavit_Date = null;
                    objRegistration.affidavit_No = "";
                    objRegistration.affidavit_Verified = false;

                    objRegistration.FinalSubmissionDate = DateTime.Now;
                    objRegistration.FinalSubmitted = true;
                    objRegistration.enter_By = Convert.ToInt32(Session["UserID"]);
                    objRegistration.enter_Date = DateTime.Now;

                    context.Entry(objRegistration).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    long Appid = objRegistration.ID;
                    //  scope.Complete();
                    objRegistration = context.NielitCentreStudent.Find(applID);
                    // DataTable dt = checkProjectMapping();

                    // added by amit start
                    DataTable dtRequired = checkProjectMapping();
                    DataTable dtUploaded = CheckDocstatus(); // documents already uploaded

                    // Build pending docs table
                    DataTable dtPending = dtRequired.Clone();

                    // Get uploaded IDs (as string to avoid type mismatch)
                    List<string> uploadedIDs = new List<string>();
                    foreach (DataRow r in dtUploaded.Rows)
                        uploadedIDs.Add(Convert.ToString(r["documentID"]));

                    // Copy only rows not uploaded yet
                    foreach (DataRow r in dtRequired.Rows)
                    {
                        if (!uploadedIDs.Contains(Convert.ToString(r["documentID"])))
                            dtPending.ImportRow(r);
                    }

                    DataTable dt = dtPending;

                    // added by amit end




                    //if(checkProjectMapping().Rows.Count>0)
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row1;
                        try
                        {
                            //String fileExtension = System.IO.Path.GetExtension(fileCertificate.FileName).ToLower();
                            if (objRegistration.Number != null)                       //Changed_15_10_2024
                            {
                                string fileName = "";
                                int i = 0;
                                foreach (GridViewRow row in GridView1.Rows)
                                {
                                    row1 = dt.Rows[i];
                                    if (row.RowType == DataControlRowType.DataRow)
                                    {
                                        // Get FileUpload control from GridView row
                                        FileUpload fileUpload = (FileUpload)row.FindControl("fileCertificate");
                                        Label lblSerialNumber = (Label)row.FindControl("lblSerialNumber");
                                        string docName = row.Cells[1].Text; // Get document name from second column
                                        if (fileUpload != null)
                                        {
                                            if (objRegistration.CastCategoryID != 1)
                                            {
                                                string[] category = ddlCategory.SelectedItem.Text.Split('/');
                                                fileName = category[0].Trim().Replace(" ", "_") + "_" + objRegistration.ID;           //Changed_15_10_2024
                                            }
                                            else if (RdisEWS.SelectedValue == "Y")
                                                fileName = "EWS_" + objRegistration.ID;                                 //Changed_15_10_2024

                                            //Code_Added_For_CastCategory_05_09_2024_Start
                                            string ConnectionString = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;

                                            using (SqlConnection con = new SqlConnection(ConnectionString))
                                            {
                                                string filePath = fileUpload.PostedFile.FileName;
                                                // getting the file path of uploaded file  
                                                string filenameO = Path.GetFileName(filePath);
                                                // getting the file name of uploaded file  
                                                string ext = Path.GetExtension(filenameO);
                                                // getting the file extension of uploaded file  
                                                if (row1["doc_Name"].ToString().Contains("Caste"))
                                                {
                                                    if (objRegistration.CastCategoryID != 1)
                                                    {
                                                        string[] category = ddlCategory.SelectedItem.Text.Split('/');
                                                        fileName = category[0].Trim().Replace(" ", "_") + "_" + objRegistration.ID;
                                                    }
                                                    else if (RdisEWS.SelectedValue == "Y")
                                                        fileName = "EWS_" + objRegistration.ID;
                                                }
                                                else if (row1["doc_Name"].ToString() != null)
                                                    fileName = row1["doc_Name"].ToString().Substring(0, row1["doc_Name"].ToString().IndexOf('-')) + "_" + objRegistration.ID;

                                                //else if(lblCertificate.Text.ToString().Contains("Income") && dt.Rows.Count > 0)
                                                //{
                                                //    fileName="Income_"+ objRegistration.ID;
                                                //}

                                                // ShowAlert(fileName0);
                                                using (var command = new SqlCommand("sp_UploadDocs", con))
                                                {
                                                    string status = string.Empty;
                                                    command.CommandType = CommandType.StoredProcedure;
                                                    command.Parameters.Clear();

                                                    command.Parameters.AddWithValue("@studentID", objRegistration.ID);              //Changed_15_10_2024

                                                    command.Parameters.AddWithValue("@Name", fileName);
                                                    command.Parameters.AddWithValue("@originalFileName", filenameO.ToString());
                                                    command.Parameters.AddWithValue("@extension", ext);
                                                    command.Parameters.AddWithValue("@uploadedFile", fileUpload.FileBytes);
                                                    //command.Parameters.AddWithValue("@documentTypeID", checkProjectMapping());

                                                    //command.Parameters.AddWithValue("@documentTypeID", docName.ToString().Contains("Caste") ? 1 : docName.ToString().Contains("Income") ? 2 : 3);
                                                    //command.Parameters.AddWithValue("@documentTypeID", row["documentID"].ToString());
                                                    command.Parameters.AddWithValue("@documentTypeID", row1["documentID"].ToString());


                                                    SqlParameter outputParam = new SqlParameter("@Status", SqlDbType.NVarChar, 100);
                                                    outputParam.Direction = ParameterDirection.Output;
                                                    command.Parameters.Add(outputParam);
                                                    con.Open();

                                                    command.ExecuteNonQuery();
                                                    con.Close();

                                                    status = command.Parameters["@Status"].Value.ToString();

                                                    if (status == "Inserted")
                                                    {
                                                        ShowAlert("Inserted Succesfully");
                                                    }
                                                    else if (status == "Exception")
                                                    {
                                                        ShowAlert("Exception Occured");
                                                        return;
                                                    }
                                                    else
                                                        ShowAlert("Updated Succesfully");

                                                }
                                            }
                                        }
                                    }

                                    i++;

                                }
                                //DataTable dt = checkProjectMapping();
                                //DataRow row;
                                //if (dt.Rows.Count > 0)
                                //{
                                //    row = dt.Rows[0];
                                //    if (lblCertificate.Text.ToString().Contains("Caste") && (row["doc_Name"].ToString().Contains("Caste")))
                                //    {
                                //        if (objRegistration.CastCategoryID != 1)
                                //        {
                                //            string[] category = ddlCategory.SelectedItem.Text.Split('/');
                                //            fileName = category[0].Trim().Replace(" ", "_") + "_" + objRegistration.ID;           //Changed_15_10_2024
                                //        }
                                //        else if (RdisEWS.SelectedValue == "Y")
                                //            fileName = "EWS_" + objRegistration.ID;                                 //Changed_15_10_2024
                                //    }
                                //    else if (row["doc_Name"].ToString() != null)
                                //        fileName = row["doc_Name"].ToString() + "_" + objRegistration.ID;

                                //    //else if (lblCertificate.Text.ToString().Contains("Income"))
                                //    //{
                                //    //    fileName = "Income_Certificate_" + objRegistration.ID;
                                //    //}
                                //    //else
                                //    //    fileName = "Temp_Certificate_" + objRegistration.ID;


                                //    //Code_Added_For_CastCategory_05_09_2024_Start
                                //    string ConnectionString = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;

                                //    using (SqlConnection con = new SqlConnection(ConnectionString))
                                //    {
                                //        string filePath = fileCertificate.PostedFile.FileName;
                                //        // getting the file path of uploaded file  
                                //        string filenameO = Path.GetFileName(filePath);
                                //        // getting the file name of uploaded file  
                                //        string ext = Path.GetExtension(filenameO);
                                //        // getting the file extension of uploaded file

                                //        using (var command = new SqlCommand("sp_UploadDocs", con))
                                //        {
                                //            string status = string.Empty;
                                //            command.CommandType = CommandType.StoredProcedure;
                                //            command.Parameters.Clear();

                                //            //int documentType = lblCertificate.Text.ToString().Contains("Caste") ? 1 : lblCertificate.Text.ToString().Contains("Income") ? 2 : 3;

                                //            command.Parameters.AddWithValue("@studentID", objRegistration.ID);              //Changed_15_10_2024

                                //            command.Parameters.AddWithValue("@Name", fileName.ToString());
                                //            command.Parameters.AddWithValue("@originalFileName", filenameO.ToString());
                                //            command.Parameters.AddWithValue("@extension", ext.ToString());
                                //            command.Parameters.AddWithValue("@uploadedFile", fileCertificate.FileBytes);
                                //            //command.Parameters.AddWithValue("@documentTypeID", checkProjectMapping());

                                //            //command.Parameters.AddWithValue("@documentTypeID", lblCertificate.Text.ToString().Contains("Caste") ? 1 : lblCertificate.Text.ToString().Contains("Income") ? 2 : 3);
                                //            command.Parameters.AddWithValue("@documentTypeID", row["documentID"].ToString());

                                //            SqlParameter outputParam = new SqlParameter("@Status", SqlDbType.NVarChar, 100);
                                //            outputParam.Direction = ParameterDirection.Output;
                                //            command.Parameters.Add(outputParam);
                                //            con.Open();

                                //            command.ExecuteNonQuery();
                                //            con.Close();

                                //            status = command.Parameters["@Status"].Value.ToString();

                                //            if (status == "Inserted")
                                //            {
                                //                ShowAlert("Inserted Succesfully");
                                //            }
                                //            else if (status == "Exception")
                                //            {
                                //                ShowAlert("Exception Occured");
                                //                return;
                                //            }
                                //            else
                                //                ShowAlert("Updated Succesfully");

                                //        }
                                //    }
                                //}
                            }
                            else
                            {
                                lblerror.Text = "Student ID does Not Exist";
                                lblerror.Visible = true;
                                return;
                            }

                        }
                        catch (Exception ex)
                        {
                            lblerror.Text = "Error: " + ex.Message.ToString();
                            lblerror.Visible = true;
                            return;
                        }
                    }

                    strMessage = "Record Updated.";
                    Response.Redirect("NielitCentreStudent.aspx?msg=" + strMessage, true);

                }
                ;
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void SaveData()
    {
        string OnlineRefNo = "";
        try
        {
            Boolean success = false;
            string AfflinstituteLinkCentreName = "", NonAfflinstituteLinkCentreName = "";
            //Added for refNo
            EConnectContext context1 = new EConnectContext();
            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            // Pre registration required for aspirational and India AI project
            if (ddlProcname.SelectedValue.ToString() == "10021")
            {
                if (txtregno.Text == "" && RadioButtonListCentre.SelectedValue.Trim() != "Y")
                {
                    ShowAlert("Please register the candidate at Student Portal and then fetch details at MIS Portal");
                    return;
                }
            }
            if (UserTypeid == 4)
            {
                NIELITMISContext context2 = new NIELITMISContext();
                var intituteslinkedToCentre = from s in context2.AffInstitutes
                                              where s.instituteID == loginUser.UserRefNumber
                                              select new { ID = s.ID, linkedToCentre = s.linkedToCentre };

                NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().ID);
                //HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
                lnkID = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
                NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                if (institutesName != null)
                {
                    txtInstitute.Text = institutesName.Name;
                    AfflinstituteLinkCentreName = institutesName.Name;
                    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);

                }
                if (ddlProcname.SelectedValue.ToString() == "9")
                {
                    ShowAlert("Project not allowed, Please cross check");
                    return;
                }
            }
            if (UserTypeid == 11)
            {
                NIELITMISContext context2 = new NIELITMISContext();
                var intituteslinkedToCentre = from s in context2.NonAffInstitutes
                                              where s.ID == loginUser.UserRefNumber
                                              select new { ID = s.ID, linkedToCentre = s.linkedToCentre };

                NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().ID);
                //HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
                lnkID = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
                NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == lnkID).FirstOrDefault();
                if (institutesName != null)
                {
                    txtInstitute.Text = institutesName.Name;
                    NonAfflinstituteLinkCentreName = institutesName.Name;
                    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);

                }
                if (ddlProcname.SelectedValue.ToString() == "9")
                {
                    ShowAlert("Project not allowed, Please cross check");
                    return;
                }
            }
            //CheckDuplicate AAdhaar for Fee reimbursement scheme
            if (ddlProcname.SelectedValue.ToString() == "9" || ddlProcname.SelectedValue.ToString() == "10020")
            {

                bool duplicateAAdhaar = false;
                duplicateAAdhaar = checkDuplicateAAdhaar();
                if (duplicateAAdhaar)
                {
                    ShowAlert("Aadhar already enrolled in some other running batch, Cannot register");
                    return;
                }
            }
            //
            //
            // else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (NIELITMISContext context = new NIELITMISContext())
                    {
                        //int courseid = Convert.ToInt32(Request.QueryString["id"]);
                        if (UserTypeid == 4)
                        {
                            NameCenter = AfflinstituteLinkCentreName;
                        }
                        else if (UserTypeid == 11)
                        {
                            NameCenter = NonAfflinstituteLinkCentreName;
                        }

                        else
                        {
                            NielitCentres nielitcentre = context.NielitCentres.Find(loginUser.UserRefNumber);
                            NameCenter = nielitcentre.Name;
                        }
                        NielitCentreStudent objRegistration;
                        string aff = RdoAffInstOrNonAffInst.SelectedValue.ToString();
                        string affVal = "";
                        if (aff == "0")
                            affVal = "N";
                        if (aff == "1")
                            affVal = "Y";
                        if (aff == "2")
                            affVal = "O";
                        //NielitCentreStudent objRegistration = new EConnect.NIELIT.NielitCentreStudent();
                        objRegistration = new NielitCentreStudent();
                        string CheckAffOrNot = RdoAffInstOrNonAffInst.SelectedValue.ToString();

                        objRegistration.whetherAffiliated = affVal;
                        objRegistration.CourseID = Convert.ToInt64(ddlCourse.SelectedValue);
                        objRegistration.batch_ID = Convert.ToInt64(ddlBatch.SelectedValue);
                        objRegistration.InstituteID = Convert.ToInt64(ddlCenter.SelectedValue);
                        //User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                        //objRegistration.InstituteID = loginUser.UserRefNumber;




                        objRegistration.ApplicationDate = DateTime.Now;

                        if (RadioButtonListCentre.SelectedValue.Trim() == "Y")
                        {
                            if (RadioButtonListRegType.SelectedValue.Trim() == "R")
                            {
                                objRegistration.AlreadyRegistered = true;
                                objRegistration.RegisteredCourseID = Convert.ToInt32(ddlCourse.SelectedValue);
                                objRegistration.RegisteredCourseRegistrationNo = Convert.ToInt32(txtregno.Text);
                            }
                            else if (RadioButtonListRegType.SelectedValue.Trim() == "A")
                            {
                                objRegistration.AlreadyRegistered = true;
                                objRegistration.RegisteredCourseID = Convert.ToInt32(ddlCourse.SelectedValue);
                            }
                        }
                        else
                        {
                            objRegistration.AlreadyRegistered = false;
                        }
                        // objRegistration.AlreadyRegistered = false;
                        objRegistration.AlreadyQualified = false;
                        objRegistration.Roll_No = null;

                        string salutation = ddlSalutaionName.SelectedItem.Text;
                        salutation = salutation.Substring(0, salutation.IndexOf('/'));

                        if (ddlWheatherPojectStu.SelectedValue == "1")
                        {
                            int projectId = Convert.ToInt32(ddlProcname.SelectedValue);
                            int courseId = Convert.ToInt32(ddlCourse.SelectedValue);
                            int centreId = Convert.ToInt32(ddlCenter.SelectedValue);
                            int batchId = Convert.ToInt32(ddlBatch.SelectedValue);


                            // DIFFERENT AT LOCAL , PLS IGNORE THIS CHANGE
                            //bool batchAllowed = validateProjectBatchSize
                            //                        .IsBatchLimitAvailable(projectId, courseId, centreId, batchId);
                            //if (batchAllowed == false)
                            //{
                            //    ShowAlert("Batch limit is already full for selected Project + Course + Centre. Registration not allowed.");
                            //    return;
                            //}
                            objRegistration.whetherProjectStudent = true;
                            objRegistration.projectId = Convert.ToInt64(ddlProcname.SelectedValue);
                        }
                        else
                        {
                            objRegistration.whetherProjectStudent = false;
                            objRegistration.projectId = 0;
                            //objRegistration.projectId = Convert.ToInt64(ddlProcname.SelectedValue);
                        }

                        objRegistration.Salutation = salutation.Trim();
                        objRegistration.Name = txtAppName.Text.Trim().ToString();
                        if (Rdoownertype.SelectedValue == "P")
                        {
                            objRegistration.FatherName = txtFatherName.Text.Trim();
                            objRegistration.MotherName = txtMotherName.Text.Trim();
                            objRegistration.GuardianName = null;
                        }
                        else if (Rdoownertype.SelectedValue == "G")
                        {
                            objRegistration.GuardianName = TxtGuardianName.Text.Trim();
                            objRegistration.FatherName = null;
                            objRegistration.MotherName = null;
                        }

                        string gender = ddl_gender.SelectedValue;
                        objRegistration.Gender = gender.Trim();
                        //gender = gender.Substring(0, gender.IndexOf('/'));
                        //objRegistration.Gender = gender.Trim();
                        if (RdisEWS.SelectedValue == "Y")
                            objRegistration.Is_EWS = true;
                        else
                            objRegistration.Is_EWS = false;
                        objRegistration.MaritalStatusID = Convert.ToInt32(ddlMStatus.SelectedValue);
                        objRegistration.DateOfBirth = Convert.ToDateTime(txtDob.Text.ToString());
                        objRegistration.CastCategoryID = Convert.ToInt32(ddlCategory.SelectedValue);
                        if (Rdhandicapped.SelectedValue == "Y")
                        {
                            objRegistration.IsHandicaped = true;
                        }
                        else
                        {
                            objRegistration.IsHandicaped = false;
                        }
                        if (Rdexserviceman.SelectedValue == "Y")
                        {
                            objRegistration.IsExServicemane = true;
                        }
                        else
                        {
                            objRegistration.IsExServicemane = false;
                        }


                        //objRegistration.BodyMark = txtBodyMark.Text;

                        //Contact Details
                        if (String.IsNullOrEmpty(TxtSTDcode.Text) == false && string.IsNullOrEmpty(txtCorPhoneNo.Text) == false)
                        {
                            objRegistration.StdNumber = Convert.ToInt32(TxtSTDcode.Text);
                            objRegistration.PhoneNumber = Convert.ToInt32(txtCorPhoneNo.Text);
                        }
                        objRegistration.MobileNumber = Convert.ToInt64(txtCorMobileNo.Text);
                        objRegistration.EmailAddress = txtEmailId.Text;


                        //Permanent Address Details
                        objRegistration.PerAddressLine1 = TxtPerAddressLine1.Text;
                        objRegistration.PerAddressLine2 = TxtPerAddressLine2.Text;
                        objRegistration.PerAddressLine3 = TxtPerAddressLine3.Text;
                        objRegistration.PerCountryID = 0;
                        objRegistration.PerCityName = TxtPerCity.Text;
                        objRegistration.PerStateID = Convert.ToInt32(ddlPState.SelectedValue);
                        objRegistration.PerDistrictID = Convert.ToInt32(ddlPdistrict.SelectedValue);
                        objRegistration.PerPinCode = Convert.ToInt32(TxtPpincode.Text);


                        //Correspondence Address Details
                        if (chkSame.Checked)
                        {
                            objRegistration.CorAddressLine1 = TxtPerAddressLine1.Text;
                            objRegistration.CorAddressLine2 = TxtPerAddressLine2.Text;
                            objRegistration.CorAddressLine3 = TxtPerAddressLine3.Text;
                            objRegistration.CorCountryID = 0;
                            objRegistration.CorCityName = TxtPerCity.Text;
                            objRegistration.CorStateID = Convert.ToInt32(ddlPState.SelectedValue);
                            objRegistration.CorDistrictID = Convert.ToInt32(ddlPdistrict.SelectedValue);
                            objRegistration.CorPinCode = Convert.ToInt32(TxtPpincode.Text);
                        }
                        else
                        {
                            objRegistration.CorAddressLine1 = Server.HtmlEncode(TxtCorAddressLine1.Text);
                            objRegistration.CorAddressLine2 = TxtCorAddressLine2.Text;
                            objRegistration.CorAddressLine3 = TxtCorAddressLine3.Text;
                            objRegistration.CorCountryID = 0;
                            objRegistration.CorCityName = TxtCorCity.Text;
                            objRegistration.CorStateID = Convert.ToInt32(ddlCorState.SelectedValue);
                            objRegistration.CorDistrictID = Convert.ToInt32(ddlcordistrict.SelectedValue);
                            objRegistration.CorPinCode = Convert.ToInt32(txtCorPinCode.Text);
                        }
                        if (ddlProcname.SelectedValue == "1")
                        {

                            int x = checkAspirationalDistrict();
                            if (x != 1)
                            {
                                ShowAlert("Project is aspirational, wrong district selected, Please check");
                                return;
                            }
                        }

                        objRegistration.IsVerifiedByInstitute = false;
                        DateTime Dob = Convert.ToDateTime(txtDob.Text);
                        Int32 ReligionId = Convert.ToInt32(ddlReligion.SelectedValue);
                        Int32 castCategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                        Boolean IsHandicapped = Rdhandicapped.SelectedValue == "Y" ? true : false;

                        objRegistration.ReligionID = ReligionId;

                        //aadhar details     
                        objRegistration.UIDType = Convert.ToInt32(UidTypeDdl.SelectedValue);
                        //Added for Aadhar Encryption
                        if (UidTypeDdl.SelectedValue.ToString() == "1")
                            objRegistration.UIDNumber = EConnect.Utils.Security.Encryption.Encrypt(UidNumberTxt.Text.Trim().ToUpper());
                        else
                            objRegistration.UIDNumber = UidNumberTxt.Text.Trim().ToUpper();

                        objRegistration.AadharVerfied = false;

                        objRegistration.whether_Course_Complete = false;
                        objRegistration.whether_Certificate_Issued = false;
                        objRegistration.certificate_Issue_Date = null;
                        objRegistration.whetherPlaced = false;
                        objRegistration.company_Name = 0;
                        objRegistration.comapny_Address = "";
                        objRegistration.comapny_Address2 = "";
                        objRegistration.comapny_Address3 = "";
                        objRegistration.CompnyCityName = "";
                        objRegistration.CompnyCountry_ID = 0;
                        objRegistration.compnyState_ID = 0;
                        objRegistration.compnyDistrict_ID = 0;
                        objRegistration.CompnyPinCode = 0;
                        objRegistration.affidavit_Date = null;
                        objRegistration.affidavit_No = "";
                        objRegistration.affidavit_Verified = false;

                        objRegistration.FinalSubmissionDate = DateTime.Now;
                        objRegistration.FinalSubmitted = true;
                        objRegistration.enter_By = Convert.ToInt32(Session["UserID"]);
                        objRegistration.enter_Date = DateTime.Now;

                        context.NielitCentreStudent.Add(objRegistration);
                        context.SaveChanges();



                        long Appid = objRegistration.ID;
                        success = true;
                        if (success == true)
                        {

                            objRegistration = context.NielitCentreStudent.Find(Convert.ToInt32(Appid));
                            objRegistration.Number = NameCenter + "-" + Appid.ToString();
                            if (NameCenter.Length > 20)
                            {
                                NameCenter = NameCenter.Substring(0, 20);
                            }

                            OnlineRefNo = NameCenter + "-" + Appid.ToString();

                            objRegistration.Number = OnlineRefNo;
                            ShowAlert(OnlineRefNo);
                            context.SaveChanges();

                            if (RadioButtonListRegType.SelectedValue.Trim() == "R")
                            {
                                UpdateOnlineRefId("0", OnlineRefNo);
                            }
                            if (RadioButtonListRegType.SelectedValue.Trim() == "A")
                            {
                                UpdateOnlineRefId(txtregno.Text, OnlineRefNo);

                            }
                            scope.Complete();    //Commented due to current transaction is already completed_17_10_2024

                            DataTable dt = checkProjectMapping();

                            //if (whetherFileUpload == true)
                            //if (checkProjectMapping().Rows.Count>0)
                            if (dt.Rows.Count > 0)
                            {
                                DataRow row1;
                                try
                                {
                                    string fileName = "";
                                    int i = 0;
                                    using (TransactionScope tr = new TransactionScope())
                                    {
                                        foreach (GridViewRow row in GridView1.Rows)
                                        {
                                            row1 = dt.Rows[i];
                                            if (row.RowType == DataControlRowType.DataRow)
                                            {
                                                // Get FileUpload control from GridView row
                                                FileUpload fileUpload = (FileUpload)row.FindControl("fileCertificate");
                                                Label lblSerialNumber = (Label)row.FindControl("lblSerialNumber");
                                                string docName = row.Cells[1].Text; // Get document name from second column

                                                if (fileUpload != null)
                                                {
                                                    //Code_Added_For_CastCategory_05_09_2024_Start
                                                    string ConnectionString = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;


                                                    using (SqlConnection con = new SqlConnection(ConnectionString))
                                                    {

                                                        string filePath = fileUpload.PostedFile.FileName;
                                                        // getting the file path of uploaded file  
                                                        string filenameO = Path.GetFileName(filePath);
                                                        // getting the file name of uploaded file  
                                                        string ext = Path.GetExtension(filenameO);
                                                        // getting the file extension of uploaded file  



                                                        if (row1["doc_Name"].ToString().Contains("Caste"))
                                                        {

                                                            if (objRegistration.CastCategoryID != 1)
                                                            {
                                                                string[] category = ddlCategory.SelectedItem.Text.Split('/');
                                                                fileName = category[0].Trim().Replace(" ", "_") + "_" + objRegistration.ID;
                                                            }
                                                            else if (RdisEWS.SelectedValue == "Y")
                                                                fileName = "EWS_" + objRegistration.ID;
                                                        }
                                                        else if (row1["doc_Name"].ToString() != null)
                                                            fileName = row1["doc_Name"].ToString().Substring(0, row1["doc_Name"].ToString().IndexOf('-')) + "_" + objRegistration.ID;
                                                        //  fileName = row1["doc_Name"].ToString() + "_" + objRegistration.ID;
                                                        using (var command = new SqlCommand("sp_UploadDocs", con))
                                                        {

                                                            string status = string.Empty;


                                                            command.CommandType = CommandType.StoredProcedure;
                                                            command.Parameters.Clear();

                                                            command.Parameters.AddWithValue("@Name", fileName.ToString());
                                                            command.Parameters.AddWithValue("@studentID", objRegistration.ID.ToString());               //Changed_15_10_2024
                                                            command.Parameters.AddWithValue("@originalFileName", filenameO.ToString());
                                                            command.Parameters.AddWithValue("@extension", ext.ToString());
                                                            command.Parameters.AddWithValue("@uploadedFile", fileUpload.FileBytes);
                                                            //command.Parameters.AddWithValue("@documentTypeID", checkProjectMapping());

                                                            //command.Parameters.AddWithValue("@documentTypeID", docName.ToString().Contains("Caste") ? 1 : docName.ToString().Contains("Income") ? 2 : 3);
                                                            command.Parameters.AddWithValue("@documentTypeID", row1["documentID"].ToString());
                                                            //command.Parameters.AddWithValue("@documentTypeID", docName.ToString().Contains("Caste") ? 1 : docName.ToString().Contains("Income") ? 2 : 3);

                                                            SqlParameter outputParam = new SqlParameter("@Status", SqlDbType.NVarChar, 100);
                                                            outputParam.Direction = ParameterDirection.Output;
                                                            command.Parameters.Add(outputParam);

                                                            con.Open();
                                                            command.ExecuteNonQuery();
                                                            con.Close();

                                                            status = command.Parameters["@Status"].Value.ToString();
                                                            if (status == "Inserted")
                                                            {
                                                                ShowAlert("Inserted Succesfully");
                                                            }
                                                            else if (status == "Exception")
                                                            {
                                                                ShowAlert("Exception Occured");
                                                                return;
                                                            }
                                                            else
                                                                ShowAlert("Updated Succesfully");
                                                        }
                                                    }
                                                }

                                            }
                                            i++;

                                        }
                                        tr.Complete();
                                    }
                                    // }
                                }
                                catch (Exception ex)
                                {
                                    lblerror.Text = "Error: " + ex.Message.ToString();
                                    lblerror.Visible = true;
                                    return;
                                }

                            }

                            //     scope.Complete();

                            strMessage = "New record saved. Reference No. for Student Registered : " + NameCenter + "-" + Appid.ToString();
                        }


                    }
                    ;



                    RadioButtonListCentre.SelectedValue = "N";
                    txtregno.Text = "";

                    Response.Redirect("NielitCentreStudent.aspx?msg=" + strMessage, true);

                    //scope.Complete();
                }
                ;
            }
        }
        catch (DbEntityValidationException e)
        {
            foreach (var eve in e.EntityValidationErrors)
            {
                ShowAlert("Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:");
                //, eve.Entry.State);
                //  Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                //    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                foreach (var ve in eve.ValidationErrors)
                {
                    ShowAlert("- Property: " + ve.PropertyName + ", Error: " + ve.ErrorMessage);
                    //ve.PropertyName, ve.ErrorMessage);
                    //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                    //  ve.PropertyName, ve.ErrorMessage);
                }
            }
            //throw;

            throw e;
        }
    }

    public void UpdateOnlineRefId(string pRegnNo, string pOnlineRefNo)
    {
        try
        {
            Int64 Number;
            if (RadioButtonListRegType.SelectedValue.Trim() == "A")
            {
                Number = 0;
            }
            else
                Number = Convert.ToInt64(txtregno.Text);

            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection Conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateOnlineRefId", Conn))
                {
                    Conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@number", Number.ToString().Trim());
                    cmd.Parameters.AddWithValue("@regno", pRegnNo);
                    cmd.Parameters.AddWithValue("@pOnlineRefNo", pOnlineRefNo);


                    cmd.ExecuteNonQuery();
                    Conn.Close();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);

        }
    }

    /* public void UpdateOnlineRefId()
     {
         try
         {
             Int64 Number = Convert.ToInt64(txtregno.Text);
             string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
             using (SqlConnection Conn = new SqlConnection(constr))
             {
                 using (SqlCommand cmd = new SqlCommand("UpdateOnlineRefId", Conn))
                 {
                     Conn.Open();
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.Parameters.AddWithValue("@number", Number);

                     cmd.ExecuteNonQuery();
                 }
             }
         }
         catch (Exception ex)
         {
             ShowAlert(ex.Message, true);
         }
     }*/
    protected void GenerateNewCaptchaImage()
    {
        try
        {
            ViewState["CaptchCode"] = EConnect.CommonFunctions.GenerateRandomNumber(6);
            EConnect.CaptchaImage captcha = new CaptchaImage(ViewState["CaptchCode"].ToString(), 200, 50, "Century Schoolbook");
            imgcap.Src = captcha.ImageSource;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }

    protected void disableAll()
    {
        try
        {
            //TrLastCenterAccno.Visible = false;
            //TrLastCenterInstiName.Visible = false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool isSelected(DropDownList Dropdown)
    {
        try
        {
            if (Dropdown.SelectedValue == "0")
            {
                Dropdown.Focus();
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool isBlank(TextBox txtBox)
    {
        try
        {
            if (txtBox.Text.Trim() == "")
            {
                txtBox.Focus();
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool isBlankNumber(TextBox txtBox)
    {
        try
        {
            int zero = 0;

            if (txtBox.Text.Trim() == "" || txtBox.Text.Trim() == zero.ToString() || txtBox.Text.Trim() == ".")
            {
                txtBox.Text = "";
                txtBox.Focus();
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected bool isNumber(TextBox txtBox)
    {
        try
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            if (txtBox.Text.Trim() != "")
            {
                if (!regex.IsMatch(txtBox.Text.Trim()))
                {
                    txtBox.Text = "";
                    txtBox.Focus();
                    return false;
                }
                else
                    return true;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool isValidPassingYear(TextBox txtPassingYear, TextBox txtDob)
    {
        try
        {
            Int64 PassingYear = Convert.ToInt64(txtPassingYear.Text);
            string Dobdate = txtDob.Text;
            string[] dobYear = Dobdate.Split('-');


            if (DateTime.Now.Year < PassingYear || PassingYear <= (Convert.ToInt64(dobYear[2]) + 10))
            {
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected bool isValidDob(TextBox txtBox)
    {
        try
        {
            DateTime todaydate = DateTime.Now;
            DateTime Inputdate = Convert.ToDateTime(txtBox.Text);

            int result1 = DateTime.Compare(todaydate, Inputdate);
            int result2 = DateTime.Compare(todaydate.AddYears(-10), Inputdate);

            if (result2 == -1)
            {
                lblerror.Text = "Invalid date of birth";
                return false;
            }
            else
                return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindCourse()
    {
        try
        {
            int CourseId = Convert.ToInt32(Convert.ToString(Request.QueryString["id"]));
            int courseCategoryId = 0;
            using (var context = new EConnectContext())
            {
                courseCategoryId = context.Courses.Find(CourseId).CourseCategoryID;
            }
            if (courseCategoryId == 8)
            {
                UIDtr.Visible = true;
                //Aadhaartr.Visible = false;
            }
            else
            {
                UIDtr.Visible = false;
                //Aadhaartr.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void bindMaritalStatus()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var maritalStatus = from p in context.MaritalStatus
                                    orderby (p.DisplayOrder)
                                    select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlMStatus, maritalStatus, lst);
            }
            ;

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    //protected void bindCenter()
    //{
    //    try
    //    {
    //        using (NIELITMISContext context = new NIELITMISContext())
    //        {

    //            if (UserTypeid == 10)
    //            {
    //                ListItem lst = new ListItem("--Select One--", "0");
    //                var Center = from t in context.NielitCentres
    //                             where t.ID == UserRefNumber
    //                             orderby (t.Name)
    //                             select new { ValueField = t.ID, TextField = t.Name };
    //                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, Center, lst);
    //                //RdoAffInstOrNonAffInst.SelectedValue = "2";
    //            }

    //        };

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }

    //}

    protected void bindCompanyName()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");

                var companyMas = from t in context.companyMasters
                                 orderby (t.company_Name)
                                 select new { ValueField = t.ID, TextField = t.company_Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCompanyName, companyMas, lst);
            }
            ;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void bindBatches()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                //DateTime checkDate=System .DateTime.Today.AddDays(-7);

                var Batch = from s in context.NielitCentreBatchs
                            where s.IsVerified == true &&  //s.enterBy == loginUserNo
                            (s.centreID.ToString().Trim() == UserRefNumber.ToString().Trim() || s.subCentreID.ToString().Trim() == UserRefNumber.ToString().Trim())
                            //                            && DbFunctions.TruncateTime(s.startDate)>=DbFunctions.TruncateTime(checkDate)
                            orderby (s.Name)
                            select new { ValueField = s.ID, TextField = s.BatchCode + "(" + s.Name + ")" };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
            }
            ;

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindProject()
    {
        try
        {
            Int64 cID = Convert.ToInt64(ddlCourse.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                string dataName = returnDataName(cID);
                // if (cID.ToString().Length > 3)
                if (dataName == "NIELITMIS")
                {
                    var Proc = from t in context.NielitProjectss
                               join k in context.NielitProjCoursess on t.ID equals k.projID
                               join d in context.NielitCourseDurations on k.courseID equals d.ID
                               where d.ID == cID
                               && k.IsActive
                               orderby (t.ProjectName)
                               select new { ValueField = t.ID, TextField = t.ProjectName };

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlProcname, Proc, lst);
                }
                else
                {
                    var Proc = from t in context.NielitProjectss
                               join k in context.NielitProjCoursess on t.ID equals k.projID
                               join d in context.CourseFeetypes on k.courseID equals d.courseID
                               where d.courseID == cID
                               && k.IsActive
                               && d.isActive
                               orderby (t.ProjectName)
                               select new { ValueField = t.ID, TextField = t.ProjectName };

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlProcname, Proc.Distinct(), lst);
                }

            }
            ;

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void bindProject1()
    {
        try
        {
            ddlProcname.Items.Clear();
            Int64 cID = Convert.ToInt64(ddlCourse.SelectedValue);
            ListItem lst = new ListItem("--Select One--", "0");
            using (NIELITMISContext context = new NIELITMISContext())
            {


                string dataName = returnDataName(cID);
                // if (cID.ToString().Length > 3)
                if (dataName == "NIELITMIS")
                {
                    var Proc = from t in context.NielitProjectss
                               join k in context.NielitProjCoursess on t.ID equals k.projID
                               join d in context.NielitCourseDurations on k.courseID equals d.ID
                               where d.ID == cID
                               // && k.IsActive
                               orderby (t.ProjectName)
                               select new { ValueField = t.ID, TextField = t.ProjectName };

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlProcname, Proc, lst);
                }
                else
                {
                    var Proc = from t in context.NielitProjectss
                               join k in context.NielitProjCoursess on t.ID equals k.projID
                               join d in context.CourseFeetypes on k.courseID equals d.courseID
                               where d.courseID == cID
                               // && k.IsActive
                               //  && d.isActive
                               orderby (t.ProjectName)
                               select new { ValueField = t.ID, TextField = t.ProjectName };

                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlProcname, Proc.Distinct(), lst);
                }



            }
            ;

        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindCastCategory()
    {
        try
        {
            ddlCategory.Items.Clear();
            //ddlCategory.SelectedIndex = 0;
            ListItem lst = new ListItem("--Select One--", "0");
            using (var context = new EConnectContext())
            {

                if (ddlProcname.SelectedValue.ToString() == "2")
                {
                    var castcategory = from p in context.CastCategories
                                       where (p.Code.Equals("SC") || p.Code.Equals("ST"))
                                       orderby (p.DisplayOrder)
                                       select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCategory, castcategory, lst);
                }
                else
                {
                    var castcategory = from p in context.CastCategories
                                       orderby (p.DisplayOrder)
                                       select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCategory, castcategory, lst);
                }
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void bindReligion()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var religion = from p in context.Religions
                               orderby (p.DisplayOrder)
                               select new { ValueField = p.ID, TextField = p.Name + " / " + p.NameRegional };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlReligion, religion, lst);
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindEducational()
    {
        try
        {
            //Int32 ApplicantTypeId = RdoUndergngDOEACC.SelectedValue == "D" ? Convert.ToInt32(enmApplicantType.Direct) : Convert.ToInt32(enmApplicantType.Institute);
            int courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
            using (var context = new EConnectContext())
            {
                //List<Int32> qualificationLevels = context.QualificationEligibility.Where(s => s.CourseID == courseID && s.ApplicantTypeID == ApplicantTypeId && s.EffectiveDateFrom <= DateTime.Now).OrderByDescending(c => c.EffectiveDateFrom).Select(c => c.QualificationLevelID).ToList();
                ListItem lst = new ListItem("--Select One--", "0");


            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindState()
    {
        Int32 courseID = Convert.ToInt32(Request.QueryString["id"]);
        Int32 withdrawlid = Convert.ToInt32(enmAccreditationStatus.Withdrawal);
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CorState = (from s in context.Locations
                                orderby (s.Name)
                                where s.LocationTypeID == 2
                                && s.ParentLocationID == 1
                                select new { ValueField = s.ID, TextField = s.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCorState, CorState, lst);

                var PState = (from s in context.Locations
                              orderby (s.Name)
                              where s.LocationTypeID == 2
                              && s.ParentLocationID == 1
                              select new { ValueField = s.ID, TextField = s.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlPState, PState, lst);
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    protected void bindDistrict(long stateID, ref DropDownList ddl)
    {
        try
        {
            int locationTypeID = Convert.ToInt32(enmLocationType.District);
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var district = from s in context.Locations
                               orderby (s.Name)
                               where s.LocationTypeID == locationTypeID
                               && s.ParentLocationID == stateID
                               select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, district.ToList(), lst);

                if (ddl.Items.Count == 0)
                    ddl.Items.Add(lst);
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void bindCorespondDistrict(int id)
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (id != null & id != 0)
                {
                    var district = from s in context.Locations
                                   orderby (s.Name)
                                   where s.LocationTypeID == 4
                                   && s.ParentLocationID == id
                                   select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlcordistrict, district, lst);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void BindAccCentre(int stateid, int courseID)
    {
        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
            Int32 withdrawlid = Convert.ToInt32(enmAccreditationStatus.Withdrawal);

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    private void bindDeclaration(string applicantType, int agecount)
    {

        if (agecount == -1)
        {
            LblGuard.Text = txtFatherName.Text;
            LblHGuard.Text = txtFatherName.Text;
            lblname.Text = txtAppName.Text;
            LblHName.Text = txtAppName.Text;
            tddeclaration1.Style.Add("display", "none");
            tddeclaration2.Style.Add("display", "block");
        }
        else
        {
            tddeclaration1.Style.Add("display", "block");
            tddeclaration2.Style.Add("display", "none");
        }
    }

    protected void GetApplicantType()
    {
        try
        {
            if (Request.QueryString["id"] == null)
                return;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
                var course = context.Courses.Find(courseID);
                if (course != null)
                {

                }
            }
            ;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }

    protected void activeInactivePrsnlAndAddressDetail(Boolean isActive)
    {
        try
        {
            ddlSalutaionName.Enabled = isActive;
            txtAppName.Enabled = isActive;
            txtFatherName.Enabled = isActive;
            txtMotherName.Enabled = isActive;
            TxtGuardianName.Enabled = isActive;
            ddl_gender.Enabled = isActive;
            txtDob.Enabled = isActive;
            ddlCategory.Enabled = isActive;
            //imgDob.Visible = isActive;

            TxtPerAddressLine1.Enabled = isActive;
            TxtPerAddressLine2.Enabled = isActive;
            TxtPerAddressLine3.Enabled = isActive;
            TxtPerCity.Enabled = isActive;
            ddlPState.Enabled = isActive;
            TxtPpincode.Enabled = isActive;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void ResetAll()
    {
        try
        {


            txtAppName.Text = "";
            txtFatherName.Text = "";
            txtMotherName.Text = "";
            TxtGuardianName.Text = "";
            TxtCorAddressLine1.Text = "";
            TxtCorAddressLine2.Text = "";
            TxtCorAddressLine3.Text = "";
            TxtCorCity.Text = "";
            TxtPerAddressLine1.Text = "";
            TxtPerAddressLine2.Text = "";
            TxtPerAddressLine3.Text = "";
            TxtPerCity.Text = "";
            TxtSTDcode.Text = "";
            //TxtYearOfPassing2.Text = "";
            //txtBodyMark.Text = "";
            txtCorMobileNo.Text = "";
            txtCorPhoneNo.Text = "";
            txtCorPinCode.Text = "";
            TxtPpincode.Text = "";
            txtCorPhoneNo.Text = "";
            txtCorPinCode.Text = "";
            txtcode.Text = "";
            txtCorPhoneNo.Text = "";
            txtDob.Text = "";
            txtEmailId.Text = "";
            GenerateNewCaptchaImage();
            ddlCategory.SelectedValue = "0";
            ddlCorState.SelectedValue = "0";
            //DDLeducode.SelectedValue = "0";
            ddlSalutaionName.SelectedValue = "0";
            Rdexserviceman.SelectedValue = "N";
            Rdhandicapped.SelectedValue = "N";
            RdisEWS.SelectedValue = "N";


            //txtCompanyName.Text = "";
            ddlCompanyName.SelectedValue = "0";
            txtCmpAdd1.Text = "";
            txtCmpAdd2.Text = "";
            txtCmpAdd3.Text = "";
            txtCmyCityName.Text = "";
            ddlCmpState.SelectedValue = "0";
            ddlCmpDistrict.SelectedValue = "0";
            ddlwhetherCertificateIssued.SelectedValue = "0";
            ddlWhetherPlaced.SelectedValue = "0";
            ddlCourseComplete.SelectedValue = "0";
            txtcertificateIssueDate.Text = "";
            txtCmpPinCode.Text = "";
            UidNumberTxt.Text = "";
            UidTypeDdl.SelectedValue = "0";
            lblerror.Text = "";

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected bool isDuplicate()
    {
        try
        {
            DateTime dob = Convert.ToDateTime(txtDob.Text);
            Int32 currentCourseID = Convert.ToInt32(ddlCourse.SelectedValue);
            Int32 currentbatchID = Convert.ToInt32(ddlBatch.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int32 count = 0;
                EConnectContext context1 = new EConnectContext();
                Int32 courseID = 0;
                //Course currentCourse = context.Courses.Find(currentCourseID); 
                NielitCentreBatch currentCourseDurationId = context.NielitCentreBatchs.Find(currentbatchID);
                Int32 courseDurationWithCourseId = Convert.ToInt32(currentCourseDurationId.CourseDurationID);
                string checkDataExists = returnDataName(courseDurationWithCourseId);
                if (checkDataExists == "NIELITMIS")
                {
                    if (courseDurationWithCourseId == currentCourseID)
                        courseID = courseDurationWithCourseId;
                }
                else
                {
                    Course currentCourse = context1.Courses.Find(courseDurationWithCourseId);
                    Int32 courseCourseId = Convert.ToInt32(currentCourse.ID);
                    if (courseCourseId == currentCourseID)
                        courseID = courseCourseId;

                }

                if (Rdoownertype.SelectedValue == "G" && courseID == currentCourseID && courseID != 0)
                {
                    count = (from c in context.NielitCentreStudent
                             where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                              && (c.GuardianName.ToUpper() == TxtGuardianName.Text.ToUpper().Trim() || c.FatherName.ToUpper() == txtFatherName.Text.ToUpper().Trim())
                              && c.Gender == ddl_gender.SelectedValue
                              && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                              && c.batch_ID == currentbatchID
                             select c).Count();
                }
                else if (courseID == currentCourseID && courseID != 0)
                {

                    count = (from c in context.NielitCentreStudent
                             where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                             && c.FatherName.Equals(txtFatherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                             && c.Gender == ddl_gender.SelectedValue
                             && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                             && c.batch_ID == currentbatchID
                             select c).Count();
                }
                if (count > 0)

                    return true;
                else

                    return false;

            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void RenderPage(Int32 currentCourseID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                int id = Convert.ToInt32(Request.QueryString["id"].ToString());
                Course currentCourse = context.Courses.Find(currentCourseID);
                Int32 ShortermCoursesCategory = context.CourseCategories.Where(s => s.Code.ToUpper() == "STC").FirstOrDefault().ID;
                ListItem lst = new ListItem("--Select One--", "0");
                Lblhead.Text = currentCourse.Name;
                var cname = from c in context.Courses
                            where c.ID == id
                            select new { ValueField = c.ID, TextField = c.Name };

                //showing declaration
                if (currentCourse.CourseCategoryID == ShortermCoursesCategory)
                {
                    //Label90.Text = "Registration Cycle / पंजीकरण चक्र";
                    tddeclaration1.Style.Add("display", "none");
                    tddeclaration2.Style.Add("display", "block");
                    trnote.Visible = true;
                    lbldeccoursecode.Text = GetInitCap(currentCourse.Name);
                    lbldeccoursecode.Text = GetInitCap(currentCourse.Name);
                    LblHCourseCode.Text = GetInitCap(currentCourse.Name);
                    //ddlPaymentOption.Items.RemoveAt(0);
                }
                else if (currentCourse.Code == "ACC")
                {
                    //Label90.Text = "Registration Cycle / पंजीकरण चक्र";
                }
                else
                {
                    tddeclaration1.Style.Add("display", "block");
                    tddeclaration2.Style.Add("display", "none");
                    trnote.Visible = false;
                }

                //DDLRegForCourse.Enabled = false;

            }
            ;

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
            if (txtcode.Text != ViewState["CaptchCode"].ToString())
            {
                lblerror.Visible = true;
                lblerror.Text = "Invalid Captcha Code";
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                return false;
            }


            if (RadioButtonListCentre.SelectedValue.Trim() == "Y")
            {
                if (RadioButtonListRegType.SelectedValue.Trim() == "R")
                {
                    // Int64 regno = Convert.ToInt64(txtregno.Text);
                    if (txtregno.Text == "")
                    {
                        lblerror.Text = "Please Enter Registration Number.";
                        return false;
                    }
                }
                else if (RadioButtonListRegType.SelectedValue.Trim() == "A")
                {
                    if (txtregno.Text == "")
                    {
                        lblerror.Text = "Please Enter Application Number.";
                        return false;
                    }
                }
            }
            //DDLeducode_SelectedIndexChanged(DDLeducode, EventArgs.Empty);
            if (ddlCenter.SelectedValue == "0")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Center";
                ddlCenter.Focus();
                return false;
            }

            if (ddlCourse.SelectedValue == "0")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Course";
                ddlCourse.Focus();
                return false;
            }

            if (ddlBatch.SelectedValue == "0")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Batch";
                ddlBatch.Focus();
                return false;
            }

            if (ddlCategory.SelectedValue != "1" && RdisEWS.SelectedValue == "Y")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = " Wrong category chosen with EWS. ";
                RdisEWS.Focus();
                return false;
            }

            if (ddlWheatherPojectStu.SelectedValue == "0")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Project Student";
                ddlBatch.Focus();
                return false;
            }

            if (ddlWheatherPojectStu.SelectedValue == "1")
            {
                if (ddlProcname.SelectedValue == "0")
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Please Select Project Name";
                    ddlBatch.Focus();
                    return false;
                }

                int x = checkAspirationalDistrict();
                if (x == 0)
                {
                    ShowAlert("Correspondence district is not an Aspirational District,Check Project/District");
                    ddlcordistrict.Focus();
                    return false;
                }
            }

            if (ddlSalutaionName.SelectedValue == "0")
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Salutation";
                ddlSalutaionName.Focus();
                return false;
            }

            if (!isBlank(txtAppName))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Applicant Name can not be left blank";
                return false;
            }

            if (!Char.IsLetter(txtAppName.Text, 0))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Applicant Name should start with an alphabet.");
            }

            if (!Char.IsLetter(txtAppName.Text, txtAppName.Text.Length - 1))
            {
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                throw new Exception("Applicant Name should end with an alphabet.");
            }

            if (txtAppName.Text.Length == 1)
            {
                txtAppName.Text = "";
                txtAppName.Focus();
                throw new Exception("Applicant Name should be single Character.");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(txtAppName.Text, "^[a-zA-Z.\u00FC\u00DC ]*$"))
            {
                txtAppName.Text = "";
                txtAppName.Focus();
                throw new Exception("Applicant Name should be with an English Alphabets(e.g - a-zA-Z)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtPerAddressLine1.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtPerAddressLine1.Text = "";
                TxtPerAddressLine1.Focus();
                throw new Exception("Address Line1 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtPerAddressLine2.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtPerAddressLine2.Text = "";
                TxtPerAddressLine2.Focus();
                throw new Exception("Address Line2 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtPerAddressLine3.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtPerAddressLine3.Text = "";
                TxtPerAddressLine3.Focus();
                throw new Exception("Address Line3 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtPerCity.Text, "^[a-zA-Z\u00FC\u00DC ]"))
            {
                TxtPerCity.Text = "";
                TxtPerCity.Focus();

                throw new Exception("City Name should be with an English Alphabets(e.g - a-zA-Z)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorAddressLine1.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtCorAddressLine1.Text = "";
                TxtCorAddressLine1.Focus();
                throw new Exception("Correspondence Address Line1 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorAddressLine2.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtCorAddressLine2.Text = "";
                TxtCorAddressLine2.Focus();
                throw new Exception("Correspondence Address Line2 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorAddressLine3.Text, "^[a-zA-Z0-9()/#-_.\u00FC\u00DC ]*$"))
            {
                TxtCorAddressLine3.Text = "";
                TxtCorAddressLine3.Focus();
                throw new Exception("Correspondence Address Line3 should be with an English Alphabets and Numbers(e.g - a-zA-Z0-9)");
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(TxtCorCity.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
            {
                TxtCorCity.Text = "";
                TxtCorCity.Focus();
                throw new Exception("Correspondence City Name should be with an English Alphabets(e.g - a-zA-Z)");
            }


            //validation skip for batch.
            //Fields_disable_for_batch_06-07-2026_amit_start


            if (!excluded_batch.Contains(Convert.ToInt64(ddlBatch.SelectedValue)))
            {

                if (Rdoownertype.SelectedValue == "P")//Parents
                {
                    if (String.IsNullOrWhiteSpace(txtFatherName.Text))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Please enter Father's Name.");
                    }

                    if (txtFatherName.Text.Length == 1)
                    {
                        txtFatherName.Text = "";
                        txtFatherName.Focus();
                        throw new Exception("Father Name should be single Character.");
                    }

                    if (txtMotherName.Text.Length == 1)
                    {
                        txtMotherName.Text = "";
                        txtMotherName.Focus();
                        throw new Exception("Mother Name should be single Character.");
                    }



                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtFatherName.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
                    {
                        txtFatherName.Text = "";
                        txtFatherName.Focus();
                        throw new Exception("Father Name should be with an English Alphabets(e.g - a-zA-Z)");
                    }

                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtMotherName.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
                    {
                        txtMotherName.Text = "";
                        txtMotherName.Focus();
                        throw new Exception("Mother Name should be with an English Alphabets(e.g - a-zA-Z)");
                    }

                    if (String.IsNullOrWhiteSpace(txtMotherName.Text))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Please enter Mother's Name");
                    }

                    if (!String.IsNullOrWhiteSpace(txtFatherName.Text))
                    {
                        if (!Char.IsLetter(txtFatherName.Text, 0))
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            throw new Exception("Father Name should start with an alphabet.");
                        }
                        if (!Char.IsLetter(txtFatherName.Text, txtFatherName.Text.Length - 1))
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            throw new Exception("Father Name should end with an alphabet.");
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(txtMotherName.Text))
                    {
                        if (!Char.IsLetter(txtMotherName.Text, 0))
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            throw new Exception("Mother Name should start with an alphabet.");
                        }
                        if (!Char.IsLetter(txtMotherName.Text, txtMotherName.Text.Length - 1))
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            throw new Exception("Mother Name should end with an alphabet.");
                        }
                    }
                }
                else if (Rdoownertype.SelectedValue == "G")
                {
                    if (String.IsNullOrWhiteSpace(TxtGuardianName.Text))
                    {
                        GenerateNewCaptchaImage();
                        txtcode.Text = "";
                        throw new Exception("Please enter Guardian Name.");
                    }

                    if (!String.IsNullOrWhiteSpace(TxtGuardianName.Text))
                    {
                        if (!Char.IsLetter(TxtGuardianName.Text, 0))
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            throw new Exception("Guardian Name should start with an alphabet.");
                        }

                        if (TxtGuardianName.Text.Length == 1)
                        {
                            TxtGuardianName.Text = "";
                            TxtGuardianName.Focus();
                            throw new Exception("Guardian Name should be single Character.");
                        }

                        if (!System.Text.RegularExpressions.Regex.IsMatch(TxtGuardianName.Text, "^[a-zA-Z\u00FC\u00DC ]*$"))
                        {
                            TxtGuardianName.Text = "";
                            TxtGuardianName.Focus();
                            throw new Exception("Guardian Name should be with an English Alphabets(e.g - a-zA-Z)");
                        }

                        if (!Char.IsLetter(TxtGuardianName.Text, TxtGuardianName.Text.Length - 1))
                        {
                            GenerateNewCaptchaImage();
                            txtcode.Text = "";
                            throw new Exception("Guardian Name should end with an alphabet.");
                        }
                    }
                }
            }

            if (!isBlank(txtDob))
            {
                lblerror.Visible = true;
                lblerror.Text = "Date Of Birth can not be left blank";
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                return false;
            }
            if (!isValidDob(txtDob))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Date of Birth";
                return false;
            }
            if (!isSelected(ddlMStatus))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Marital Status";
                return false;
            }
            if (!isSelected(ddlCategory))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Cast Category";
                return false;
            }
            if (!isSelected(ddlReligion))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Religion";
                return false;
            }
            if (String.IsNullOrEmpty(TxtSTDcode.Text) == false && string.IsNullOrEmpty(txtCorPhoneNo.Text) == false)
            {

                if (!isNumber(TxtSTDcode))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Invalid STD Code";
                    return false;
                }

                if (!isNumber(txtCorPhoneNo))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Invalid Phone Number";
                    return false;
                }
            }
            if (!isBlank(txtCorMobileNo))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Mobile Number can not be left blank";
                return false;
            }
            if (!isNumber(txtCorMobileNo))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Mobile Number";
                return false;
            }
            if (!isBlank(txtEmailId))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Email Id can not be left blank";
                return false;
            }
            if (!isBlank(TxtPerAddressLine1))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Permanent Address Line1 can not be left blank";
                return false;
            }
            if (!isBlank(TxtPerAddressLine2))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Permanent Address Line2 can not be left blank";
                return false;
            }
            if (!isBlank(TxtPerCity))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Permanent City can not be left blank";
                return false;
            }

            if (!isSelected(ddlPState))
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select Permanent State";
                return false;
            }
            if (!isSelected(ddlPdistrict))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Please Select Permanent District";
                return false;
            }
            if (!isBlank(TxtPpincode))
            {
                lblerror.Visible = true;
                lblerror.Text = "Pin Code can not be left blank";
                return false;
            }
            if (!isNumber(TxtPpincode))
            {
                lblerror.Visible = true;
                GenerateNewCaptchaImage();
                txtcode.Text = "";
                lblerror.Text = "Invalid Pin Code Number";
                return false;
            }
            if (chkSame.Checked == false)
            {
                if (!isBlank(TxtCorAddressLine1))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "correspondence Address Line1 can not be left blank";
                    return false;
                }
                if (!isBlank(TxtCorAddressLine2))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "correspondence Address Line2 can not be left blank";
                    return false;
                }
                if (!isBlank(TxtCorCity))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "correspondence  City can not be left blank";
                    return false;
                }

                if (!isSelected(ddlCorState))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Please Select correspondence State";
                    return false;
                }
                if (!isSelected(ddlcordistrict))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Please Select correspondence District";
                    return false;
                }
                if (!isBlank(txtCorPinCode))
                {
                    lblerror.Visible = true;
                    lblerror.Text = "Pin Code can not be left blank";
                    return false;
                }
                if (!isNumber(txtCorPinCode))
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    lblerror.Text = "Invalid  Pin Code Number";
                    return false;
                }
            }

            if (UIDtr.Visible == true)
            {

                if (isSelected(UidTypeDdl))
                {
                    //Fields_disable_for_batch_06-07-2026_amit_start
                    if (!excluded_batch.Contains(Convert.ToInt64(ddlBatch.SelectedValue)))
                    {
                        //Fields_disable_for_batch_06-07-2026_amit_end
                        if (UidTypeDdl.SelectedValue == "1")  // ----Aadhaar
                        {
                            //Added for update mode 
                            if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                            {
                            }
                            else
                            {
                                if (!IsNumeric(UidNumberTxt.Text))
                                {
                                    GenerateNewCaptchaImage();
                                    txtcode.Text = "";
                                    throw new Exception("Not valid Aadhaar Number.");
                                }
                                if (UidNumberTxt.Text.Length != 12)
                                {
                                    GenerateNewCaptchaImage();
                                    txtcode.Text = "";
                                    throw new Exception("Aadhaar Number should of 12 digits.");
                                }
                            }
                        }
                        else if (UidTypeDdl.SelectedValue == "2")  // ----PAN
                        {
                            if (!Char.IsLetter(UidNumberTxt.Text, 0))
                            {
                                GenerateNewCaptchaImage();
                                txtcode.Text = "";
                                throw new Exception("PAN should start with an alphabet.");
                            }
                            if (UidNumberTxt.Text.Length != 10)
                            {
                                GenerateNewCaptchaImage();
                                txtcode.Text = "";
                                throw new Exception("PAN should of 10 digits.");
                            }
                        }
                    }

                }

            }
            if (ddlwhetherCertificateIssued.SelectedValue == "1")//Parents
            {
                if (!isBlank(txtcertificateIssueDate))
                {
                    GenerateNewCaptchaImage();
                    txtcertificateIssueDate.Text = "";
                    throw new Exception("Certificate Issue Date can not be left blank");
                }
            }

            //New_Added_For_Documents_Upload_07_02_2025_Start

            //if(checkProjectMapping().Rows.Count>0)                    //if (fileCertificate.Enabled == true)
            DataTable dt = checkProjectMapping();
            if (dt.Rows.Count > 0)
            {
                DataRow row1;
                int i = 0;
                whetherFileUpload = true;
                List<string> missingFiles = new List<string>();
                lblerror.Text = "";
                foreach (GridViewRow row in GridView1.Rows)
                {
                    row1 = dt.Rows[i];
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        // Get FileUpload control from GridView row
                        FileUpload fileUpload = (FileUpload)row.FindControl("fileCertificate");
                        string docName = row.Cells[1].Text;
                        Label lblSerialNumber = (Label)row.FindControl("lblSerialNumber");

                        if (fileUpload != null)
                        {
                            String fileExtension = System.IO.Path.GetExtension(fileUpload.FileName).ToLower();
                            Int32 fileSize = Convert.ToInt32(row1["Size"].ToString()) * 1024;
                            // Check if file is uploaded
                            if (!fileUpload.HasFile)
                            {
                                // Add missing document name to the list
                                missingFiles.Add("Row " + lblSerialNumber.Text + ": " + docName);
                                // ShowAlert("a");
                            }
                            else if (fileUpload.FileName.Length > 49)
                            {
                                lblerror.Visible = true;
                                //lblerror.Text = str[1] +" Certificate file name should be less than 44 characters.";
                                lblerror.Text = lblerror.Text + " " + docName + " file name should be less than 44 characters.<br/>";
                                fileUpload.PostedFile.InputStream.Dispose();
                                //GenerateNewCaptchaImage();
                                //return false;
                                //throw new Exception("Certificate file name should be less than 44 characters.");

                            }
                            else if (!System.Text.RegularExpressions.Regex.IsMatch(Convert.ToString(fileUpload.FileName), "^[a-zA-Z0-9()-_.\u00FC\u00DC ]*$"))
                            {
                                lblerror.Visible = true;
                                //lblerror.Text = str[1]+ " Certificate file name should be English Alphabets and Numbers only(like a-zA-Z0-9).";
                                lblerror.Text = lblerror.Text + " " + docName + " file name should be English Alphabets and Numbers only(like a-zA-Z0-9). <br/>";
                                fileUpload.PostedFile.InputStream.Dispose();
                                //GenerateNewCaptchaImage();
                                //return false;
                                //throw new Exception("Certificate file name should be English Alphabets and Numbers only(like a-zA-Z0-9).");

                            }
                            else if (fileExtension != "." + row1["Extension"].ToString())                            //else if (fileExtension != ".pdf")
                            {
                                lblerror.Visible = true;
                                //lblerror.Text = ("Invalid "+ str[1] + " Certificate file .Only pdf extensions are allowed.");
                                //lblerror.Text = lblerror.Text + " " + ("Invalid " + docName + " file .Only pdf extensions are allowed.");
                                lblerror.Text = lblerror.Text + " " + ("Invalid " + docName + " file Only " + row1["Extension"].ToString() + " extensions are allowed.<br/>");
                                //GenerateNewCaptchaImage();
                                //return false;
                            }
                            else if (!isvalidFileSize(fileUpload, fileSize)) //else if (!isvalidFileSize(fileUpload, 102400))
                            {
                                lblerror.Visible = true;
                                //lblerror.Text = (str[1] + " Certificate File size should be of 100 KB or less.");
                                //lblerror.Text = lblerror.Text+" "+(docName + " File size should be of 100 KB or less.");
                                lblerror.Text = lblerror.Text + " " + (docName + " File size should be of " + Convert.ToInt32(row1["Size"].ToString()) + " KB or less.<br/>");
                                //GenerateNewCaptchaImage();
                                //return false;
                            }
                            //else
                            //{
                            //    // Save the uploaded file
                            //    string filePath = Server.MapPath("~/Uploads/" + fileUpload.FileName);
                            //    fileUpload.SaveAs(filePath);
                            //}
                        }
                    }

                    i++;
                }
                // Display status message
                if (missingFiles.Count > 0 && !(string.IsNullOrWhiteSpace(lblerror.Text.ToString())))
                {
                    string tempError = lblerror.Text.ToString();
                    lblerror.Text = "The following files are missing:<br/>" + string.Join("<br/>", missingFiles) + " " + tempError;
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    return false;
                    //throw new Exception("Certificate file name should be English Alphabets and Numbers only(like a-zA-Z0-9).");

                }
                else if (missingFiles.Count > 0)
                {
                    lblerror.Text = "The following files are missing:<br/>" + string.Join("<br/>", missingFiles);
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    return false;
                }
                else if (!string.IsNullOrWhiteSpace(lblerror.Text.ToString()))
                {
                    lblerror.Visible = true;
                    string tempError = lblerror.Text.ToString();
                    lblerror.Text = " There is mismatch in file format as : <br/>" + tempError;
                    GenerateNewCaptchaImage();
                    return false;
                }
            }

            //    }
            //    //Code_Added_For_CastCategory_05_09_2024_Start
            //}

            //Added_ForCertificateUpload_18_10_2024_End

            return true;
        }
        catch (Exception ex)
        {
            GenerateNewCaptchaImage();
            txtcode.Text = "";
            throw ex;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsValidForm())
            {

                // added by amit start

                String name = txtAppName.Text.Trim();
                String fname = txtFatherName.Text.Trim();
                String gender = ddl_gender.SelectedItem.Text;
                DateTime dob = DateTime.ParseExact(
                    txtDob.Text.Trim(),
                    "dd-MMM-yyyy",
                    CultureInfo.InvariantCulture
                );
                Int64 courseId = Convert.ToInt64(ddlCourse.SelectedValue);
                Int64 projectId = ddlWheatherPojectStu.SelectedValue == "1" ? Convert.ToInt64(ddlProcname.SelectedValue) : -99;
                String ID = !String.IsNullOrEmpty(Request.QueryString["Key"]) ? Request.QueryString["Key"] : "-99";
                Int32 isHO = 0;


                // added by amit start
                if (ddlWheatherPojectStu.SelectedValue == "1" && utility.checkprojectduplicate(name, fname, gender, dob, courseId, projectId, ID, isHO) == 1)
                {
                    ShowAlert("This candidate already exists for this project and course.");
                    return;
                }
                // added by amit end



                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    btnSave.Text = "Update";
                    btnback.Visible = true;
                    UpdateData();
                }
                else
                {
                    if (excluded_batch.Contains(Convert.ToInt64(ddlBatch.SelectedValue)) || !isDuplicate())
                    {
                        btnSave.Text = "Submit";
                        btnback.Visible = true;
                        SaveData();
                    }
                    else
                    {
                        ShowAlert("Candidate is already registered for " + ddlCourse.SelectedItem + " .");
                        return;

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected bool isDuplicate1()
    {
        try
        {
            DateTime dob = Convert.ToDateTime(txtDob.Text);
            Int32 currentCourseID = Convert.ToInt32(Request.QueryString["id"].ToString());
            using (EConnectContext context = new EConnectContext())
            {
                Int32 count;
                Int32 ShortermCoursesCategory = context.CourseCategories.Where(s => s.Code.ToUpper() == "STC").FirstOrDefault().ID;
                Int32 IRDACategory = context.CourseCategories.Where(s => s.Code.ToUpper() == "IRDA").FirstOrDefault().ID;
                Course currentCourse = context.Courses.Find(currentCourseID);

                if (Rdoownertype.SelectedValue == "G")
                {
                    count = (from c in context.Candidates
                             join r in context.RegistrationDetails
                                 on c.ID equals r.CandidateID
                             where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                             && (c.GuardianName.ToUpper() == TxtGuardianName.Text.ToUpper().Trim() || c.FatherName.ToUpper() == txtFatherName.Text.ToUpper().Trim())
                             && c.Gender == ddl_gender.SelectedValue
                             && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                             && r.CourseID == currentCourseID
                             select c).Count();
                }
                else
                {
                    count = (from c in context.Candidates
                             join r in context.RegistrationDetails
                                 on c.ID equals r.CandidateID
                             where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                             && c.FatherName.Equals(txtFatherName.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                             && c.Gender == ddl_gender.SelectedValue
                             && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                             && r.CourseID == currentCourseID
                             select c).Count();
                }

                if (count > 0)
                    return true;
                else
                    return false;
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void chkSame_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkSame.Checked)
            {
                bool isComplete = true;
                if (!isBlank(TxtPerAddressLine1))
                {
                    lblerror.Text = "Permanent Address Line1 can not be left blank";
                    isComplete = false;
                }
                else if (!isBlank(TxtPerAddressLine2))
                {
                    lblerror.Text = "Permanent Address Line2 can not be left blank";
                    isComplete = false;
                }
                else if (!isBlank(TxtPerCity))
                {
                    lblerror.Text = "Permanent City can not be left blank";
                    isComplete = false;
                }

                else if (!isSelected(ddlPState))
                {
                    lblerror.Text = "Please Select Permanent State";
                    isComplete = false;
                }
                else if (!isSelected(ddlPdistrict))
                {
                    lblerror.Text = "Please Select Permanent District";
                    isComplete = false;
                }
                else if (!isBlank(TxtPpincode))
                {
                    lblerror.Text = "Pin Code can not be left blank";
                    isComplete = false;
                }
                else if (!isNumber(TxtPpincode))
                {
                    lblerror.Text = "Invalid Pin Code Number";
                    isComplete = false;
                }
                else if (TxtPpincode.Text.Length != 6)
                {
                    lblerror.Text = "Invalid Pin Code Number";
                    isComplete = false;
                }
                if (isComplete == false)
                {
                    lblerror.Visible = true;
                    GenerateNewCaptchaImage();
                    txtcode.Text = "";
                    chkSame.Checked = false;
                    return;
                }
                TxtCorAddressLine1.Enabled = false;
                TxtCorAddressLine2.Enabled = false;
                TxtCorAddressLine3.Enabled = false;
                TxtCorCity.Enabled = false;
                txtCorPinCode.Enabled = false;
                ddlCorState.Enabled = false;
                ddlcordistrict.Enabled = false;
                TxtCorAddressLine1.Text = TxtPerAddressLine1.Text;
                TxtCorAddressLine2.Text = TxtPerAddressLine2.Text;
                TxtCorAddressLine3.Text = TxtPerAddressLine3.Text;
                TxtCorCity.Text = TxtPerCity.Text;
                txtCorPinCode.Text = TxtPpincode.Text;
                ddlCorState.SelectedValue = ddlPState.SelectedValue;
                ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                ddlcordistrict.SelectedValue = ddlPdistrict.SelectedValue;
            }
            else
            {
                TxtCorAddressLine1.Enabled = true;
                TxtCorAddressLine2.Enabled = true;
                TxtCorAddressLine3.Enabled = true;
                TxtCorCity.Enabled = true;
                txtCorPinCode.Enabled = true;
                ddlCorState.Enabled = true;
                ddlcordistrict.Enabled = true;
                TxtCorAddressLine1.Text = "";
                TxtCorAddressLine2.Text = "";
                TxtCorAddressLine3.Text = "";
                TxtCorCity.Text = "";
                txtCorPinCode.Text = "";
                ddlCorState.SelectedValue = "0";
                ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                ddlcordistrict.SelectedValue = "0";
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void DdlAccState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            //stateid = Convert.ToInt32(DdlAccState.SelectedValue);
            Int32 courseID = Convert.ToInt32(Request.QueryString["id"].ToString());
            BindAccCentre(stateid, courseID);


        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("NielitCentreStudent.aspx?msg=" + strMessage, true);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }

    }

    protected void RdoUndergngDOEACC_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindEducational();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }
    }

    protected void ddlCorState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id1 = Convert.ToInt32(ddlCorState.SelectedValue);
            ddlcordistrict.Items.Clear();
            bindDistrict(id1, ref ddlcordistrict);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void ddlPState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id2 = Convert.ToInt32(ddlPState.SelectedValue);
            bindDistrict(id2, ref ddlPdistrict);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void ImgBtnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GenerateNewCaptchaImage();
            txtcode.Text = "";
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void ddlSalutaionName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSalutaionName.SelectedValue != "0")
            {
                if (ddlSalutaionName.SelectedValue.Trim().ToUpper() == "MS.")
                {
                    ddl_gender.SelectedValue = "Female";
                    ddl_gender.Enabled = false;
                }
                else if (ddlSalutaionName.SelectedValue == "Mr.")
                {
                    ddl_gender.SelectedValue = "Male";
                    ddl_gender.Enabled = false;
                }
                //Added for transgender
                else if (ddlSalutaionName.SelectedValue == "X")
                {
                    ddl_gender.SelectedValue = "Trans";
                    ddl_gender.Enabled = false;
                }

            }

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void Rdoownertype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Rdoownertype.SelectedValue == "P")//Parents
            {
                trfather.Visible = true;
                trmother.Visible = true;
                trguardian.Visible = false;
                trfather.Attributes.Add("class", "gdrow1");
                trmother.Attributes.Add("class", "gdalternate1");
                trgender.Attributes.Add("class", "gdrow1");
                trdob.Attributes.Add("class", "trgdalternate1calendar");
                trmaritalstatus.Attributes.Add("class", "gdrow1");
                trcategory.Attributes.Add("class", "gdalternate1");
                trhandicapped.Attributes.Add("class", "gdrow1");
                trexserviceman.Attributes.Add("class", "gdalternate1");
                trreligion.Attributes.Add("class", "gdrow1");
            }
            if (Rdoownertype.SelectedValue == "G")//Guardian
            {
                trguardian.Visible = true;
                trfather.Visible = false;
                trmother.Visible = false;
                trgender.Attributes.Add("class", "gdalternate1");
                trdob.Attributes.Add("class", "trgdrow1calendar");
                trmaritalstatus.Attributes.Add("class", "gdalternate1");
                trcategory.Attributes.Add("class", "gdrow1");
                trhandicapped.Attributes.Add("class", "gdalternate1");
                trexserviceman.Attributes.Add("class", "gdrow1");
                trreligion.Attributes.Add("class", "gdalternate1");
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }
    }

    protected void txtDob_TextChanged(object sender, EventArgs e)
    {
        DateTime todaydate = DateTime.Now;
        DateTime Inputdate = Convert.ToDateTime(txtDob.Text);
        int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);
        //string applicantType = RdoUndergngDOEACC.SelectedValue.ToString();
        //if (courseType < 5)
        //    bindDeclaration(applicantType, countAge);

    }

    protected void DdlAccCentre_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (DdlAccCentre.SelectedValue != "0")
        //    AlertModalPopUp.Show();
    }

    protected void ddlCmpState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int id1 = Convert.ToInt32(ddlCmpState.SelectedValue);
            //ddlcordistrict.Items.Clear();
            bindDistrict(id1, ref ddlCmpDistrict);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message.ToString() + ex.Source.ToString();
        }
    }

    protected void ddlWheatherPojectStu_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlWheatherPojectStu.SelectedValue == "1")
        {

            // bindProject();
            ddlProcname.Enabled = true;
        }
        else
        {
            bindProject();
            ddlProcname.SelectedValue = "0";
            ddlProcname.Enabled = false;
            ddlProcname_SelectedIndexChanged(sender, e);
            GridView1.DataSource = null;
            GridView1.DataBind();
            updPanel1.Update();
            // added by amit start
            GridView_uploaded.DataSource = null;
            GridView_uploaded.DataBind();
            upnluploaded.Update();
            // added by amit end

        }
        // bindCastCategory();
    }

    protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCompanyName.SelectedValue != "0")
            {
                Int64 iID = Convert.ToInt64(ddlCompanyName.SelectedValue);
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    var application = context.companyMasters.Find(iID);

                    //ddlCompanyName.SelectedItem = application.company_Name;
                    txtCmpAdd1.Text = application.comapny_Address.ToString();

                    if (application.comapny_Address2 != null)
                    {
                        txtCmpAdd2.Text = application.comapny_Address2.ToString();
                    }

                    if (application.comapny_Address3 != null)
                    {
                        txtCmpAdd3.Text = application.comapny_Address3.ToString();
                    }

                    if (application.CompnyCityName != null)
                    {
                        txtCmyCityName.Text = application.CompnyCityName.ToString();
                    }

                    if (application.compnyState_ID != null)
                    {
                        ddlCmpState.SelectedValue = application.compnyState_ID.ToString();
                    }
                    ddlCmpState_SelectedIndexChanged(ddlCmpState, EventArgs.Empty);
                    if (application.compnyDistrict_ID != null)
                    {
                        ddlCmpDistrict.SelectedValue = application.compnyDistrict_ID.ToString();
                    }

                    if (application.CompnyPinCode != null)
                    {
                        txtCmpPinCode.Text = application.CompnyPinCode.ToString();
                    }

                    if (application.contactPersonName != null)
                    {
                        txtPersonName.Text = application.contactPersonName.ToString();
                    }

                    if (application.contactPersonMobile != null)
                    {
                        txtMob.Text = application.contactPersonMobile.ToString();
                    }

                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlWhetherPlaced_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlWhetherPlaced.SelectedValue == "1")
        {
            btn.Visible = true;
            //trPersonDetails.Visible = true;
            //tr2.Visible = true;
            //trCompanyHead.Visible = true;
            //trCompanyName.Visible = true;
            //trCmpAdd1.Visible = true;
            //trCmpAdd2.Visible = true;
            //trCmpAdd3.Visible = true;
            //trCmyCityName.Visible = true;
            //trCmpState.Visible = true;
            //trCmpDistrict.Visible = true;
            //trCmpPinCode.Visible = true;

        }

        else
        {
            btn.Visible = false;
            //trPersonDetails.Visible = false;
            //tr2.Visible = false;
            //trCompanyHead.Visible = false;
            //trCompanyName.Visible = false;
            //trCmpAdd1.Visible = false;
            //trCmpAdd2.Visible = false;
            //trCmpAdd3.Visible = false;
            //trCmyCityName.Visible = false;
            //trCmpState.Visible = false;
            //trCmpDistrict.Visible = false;
            //trCmpPinCode.Visible = false;

        }
    }
    protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListItem lst = new ListItem("--Select One--", "0");
        Int64 NelitCentreLinkId = 0;
        Int64 NielitCentrelinkedToCentreId = 0;
        Int64 subcentreid = 0;
        string Seleted = "";
        User objUser;

        ddlBatch.ClearSelection();
        ddlCourse.ClearSelection();

        ddlBatch.Items.Clear();
        ddlCourse.Items.Clear();
        try
        {
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    Seleted = RdoAffInstOrNonAffInst.SelectedValue;
                    subcentreid = Convert.ToInt64(ddlCenter.SelectedValue);
                    if (UserTypeid == 10)
                    {
                        var instituteslinkedToCentre = context.NielitCentres.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (UserTypeid == 11)
                    {
                        var instituteslinkedToCentre = context.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }

                    if (NielitCentrelinkedToCentreId != 0)
                    {
                        NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        txtInstitute.Text = institutesName.Name;
                        Int32 NelitCentreLinkId1 = Convert.ToInt32(institutesName.ID);

                    }

                    if (Seleted == "2")
                        BindCourses(Convert.ToInt32(NielitCentreId), Convert.ToInt16(Seleted));
                    else
                        BindCourses(Convert.ToInt32(subcentreid), Convert.ToInt16(Seleted));

                    ddlCourse.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Error:" + ex.Message);
        }

    }

    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 NelitCentreLinkId = 0;
        Int64 subcentreid = 0;
        Int64 Courseid = 0;



        ddlBatch.ClearSelection();

        ddlBatch.Items.Clear();


        try
        {
            subcentreid = Convert.ToInt64(ddlCenter.SelectedValue);
            Courseid = Convert.ToInt64(ddlCourse.SelectedValue);

            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                //   DateTime checkDate = System.DateTime.Today.AddDays(-7);
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    if (RdoAffInstOrNonAffInst.SelectedValue == "2")
                    {

                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.centreID == subcentreid && s.subCentreID == 0
                                            && s.CourseDurationID == Courseid
                                    //  && (DbFunctions.TruncateTime(s.startDate) >= DbFunctions.TruncateTime(checkDate)
                                    // && DbFunctions.TruncateTime(s.endDate) >= DbFunctions.TruncateTime(System.DateTime.Today))
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.BatchCode + "(" + s.Name + ")" };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
                        if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                        {

                            bindProject1();
                        }
                        else
                            bindProject();
                    }
                    else
                    {
                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.subCentreID == subcentreid
                                            && s.CourseDurationID == Courseid //&& (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
                                                                              //   && (DbFunctions.TruncateTime(s.startDate) >= DbFunctions.TruncateTime(checkDate)
                                                                              //&& DbFunctions.TruncateTime(s.endDate) >= DbFunctions.TruncateTime(System.DateTime.Today))
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.BatchCode + "(" + s.Name + ")" };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
                        if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                        {

                            bindProject1();
                        }
                        else
                            bindProject();

                    }
                }
                else
                {
                    if (RdoAffInstOrNonAffInst.SelectedValue == "2")
                    {

                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.centreID == subcentreid && s.subCentreID == 0
                                            && s.CourseDurationID == Courseid
                                    //  && (DbFunctions.TruncateTime(s.startDate) >= DbFunctions.TruncateTime(checkDate)	
                                    //&& DbFunctions.TruncateTime(s.endDate) >= DbFunctions.TruncateTime(System.DateTime.Today))
                                    //(s.startDate <= System.DateTime.Now && 
                                    // && (s.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.BatchCode + "(" + s.Name + ")" };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
                        if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                        {

                            bindProject1();
                        }
                        else
                            bindProject();
                    }
                    else
                    {
                        var Batch = from s in context.NielitCentreBatchs
                                    where s.IsVerified == true && s.subCentreID == subcentreid
                                            && s.CourseDurationID == Courseid
                                    //  && (DbFunctions.TruncateTime(s.startDate) >= DbFunctions.TruncateTime(checkDate)  
                                    //	&& DbFunctions.TruncateTime(s.endDate) >= DbFunctions.TruncateTime(System.DateTime.Now))
                                    //(s.startDate <= System.DateTime.Now && 

                                    // && (s.endDate >= System.DateTime.Now)// comment this line for previous date entry batch code and student records on 15 march 2021
                                    orderby (s.Name)
                                    select new { ValueField = s.ID, TextField = s.BatchCode + "(" + s.Name + ")" };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, Batch, lst);
                        if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                        {

                            bindProject1();
                        }
                        else
                            bindProject();

                    }
                }
            }
            ;

            ddlWheatherPojectStu.SelectedValue = "0";
            ddlWheatherPojectStu_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void BindBatch()
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
                    // DateTime checkDate = System.DateTime.Today.AddDays(-7);
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
                            ddlCenter.Enabled = false;
                            ListItem lst = new ListItem("--Select One--", "0");
                            var BatchName = from p in context1.NielitCentreBatchs
                                            where p.IsVerified == true
                                            //  && (p.startDate >= checkDate  && p.endDate >= System.DateTime.Now)
                                            && p.subCentreID == NelitCentreLinkId
                                            orderby (p.Name)
                                            select new { ValueField = p.ID, TextField = p.BatchCode + "(" + p.Name + ")" };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
                            //EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName, lst);
                        }
                        else
                        {
                            NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            RdoAffInstOrNonAffInst.SelectedValue = "2";
                            ddlCenter.Enabled = false;
                            ListItem lst = new ListItem("--Select One--", "0");
                            var BatchName = from p in context1.NielitCentreBatchs
                                            where p.IsVerified == true
                                            // && (p.startDate >= checkDate && p.endDate >= System.DateTime.Now)
                                            && p.centreID == NelitCentreLinkId
                                            orderby (p.Name)
                                            select new { ValueField = p.ID, TextField = p.BatchCode + "(" + p.Name + ")" };
                            EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
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
                        // FillddlSubcentreName();
                        ListItem lst = new ListItem("--Select One--", "0");
                        var BatchName = from p in context1.NielitCentreBatchs
                                        where p.IsVerified == true
                                        // && (p.startDate >= checkDate  && p.endDate >= System.DateTime.Now)
                                        && p.subCentreID == subcentreId
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.BatchCode + "(" + p.Name + ")" };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);
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
                        // FillddlSubcentreName();
                        ListItem lst = new ListItem("--Select One--", "0");
                        var BatchName = from p in context1.NielitCentreBatchs
                                        where p.IsVerified == true
                                        //&& (p.startDate >= checkDate  && p.endDate >= System.DateTime.Now)
                                        && p.subCentreID == subcentreId
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.BatchCode + "(" + p.Name + ")" };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchName, lst);

                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void certificateIssueDate_Calender_SelectionChanged(object sender, System.EventArgs e)
    {

        NIELITMISContext context = new NIELITMISContext();
        Int64 applID = Convert.ToInt64(Request.QueryString["Key"]);

        var objRegistration = context.NielitCentreStudent.Find(applID);

        if (objRegistration.whether_Certificate_Issued.ToString() == "1")
        {
            //txtcertificateIssueDate.Text = Convert.ToDateTime(certificateIssueDate_Calender.SelectedDate, CultureInfo.GetCultureInfo("en-US")).ToString("dd-MMM-yyyy");
        }
        else
        {
            lblerror.Visible = true;
            lblerror.Text = "Not allowed to change date!!.";
            txtcertificateIssueDate.Focus();
            return;
        }
    }

    protected void CalendarPlacementDate_SelectionChanged(object sender, System.EventArgs e)
    {
        //txtPlacementDate.Text = Convert.ToDateTime(CalendarPlacementDate.SelectedDate, CultureInfo.GetCultureInfo("en-US")).ToString("dd-MMM-yyyy");
    }

    protected void TextBox1_TextChanged(object sender, System.EventArgs e)
    {
        DateTime todaydate = DateTime.Now;
        DateTime Inputdate = Convert.ToDateTime(txtDob.Text);
        int countAge = DateTime.Compare(todaydate.AddYears(-18), Inputdate);
        //string applicantType = RdoUndergngDOEACC.SelectedValue.ToString();
        //if (courseType < 5)
        //    bindDeclaration(applicantType, countAge);
    }

    protected void btn_Click(object sender, System.EventArgs e)
    {
        using (NIELITMISContext context = new NIELITMISContext())
        {

            Int64 applID = Convert.ToInt64(Request.QueryString["Key"]);
            var objRegistration = context.NielitCentreStudent.Find(applID);

            Session["StudentID"] = objRegistration.Number;
            Session["batch_ID"] = objRegistration.batch_ID;
        }

        Response.Redirect("../Admin/StudentPlacementDetails.aspx", true);
    }
    protected void ddlProcname_SelectedIndexChanged(object sender, EventArgs e)
    {
        /* if (ddlWheatherPojectStu.SelectedIndex == 0)
             return;
         bindCastCategory();
         int x = checkAspirationalDistrict();
         if (x == 0)
         {
             ShowAlert("Correspondence district is not an Aspirational District,Check Project/District");
             ddlcordistrict.Focus();
             return;
         }
         DataTable dt = checkProjectMapping();
         //DataRow row;
         if (dt.Rows.Count > 0)
         {
             //trCertificateHead.Visible = true;
             // trCertificate.Visible = true;
             GridView1.DataSource = dt;
             GridView1.DataBind();
             GridView1.Visible = true;
             updPanel1.Update();
             //  gridRow.Visible = true;
             //  trCertificateHead.Visible = true;
             //  DocsPanel.Visible = true;
         }
         else
         {

             GridView1.DataSource = null;
             // GridView1.Rows[0].Cells.Clear();
             GridView1.DataBind();
             updPanel1.Update();
             // added by amit start

             GridView_uploaded.DataSource = null;
             GridView_uploaded.DataBind();
             upnluploaded.Update();

             // added by amit end

             // gridRow.Visible = false;
             //  GridView1.Visible = false;
             //  trCertificateHead.Visible = false;
             //  trCertificate.Visible = false;
             //  DocsPanel.Visible = false;
         }*/
        // sanity checks
        if (ddlWheatherPojectStu.SelectedIndex == 0)
            return;

        bindCastCategory();

        if (checkAspirationalDistrict() == 0)
        {
            ShowAlert("Correspondence district is not an Aspirational District. Check Project/District");
            ddlcordistrict.Focus();
            return;
        }

        // get required documents for selected project
        DataTable dtRequired = checkProjectMapping();

        if (dtRequired == null || dtRequired.Rows.Count == 0)
        {
            GridView1.DataSource = GridView_uploaded.DataSource = null;
            GridView1.DataBind();
            GridView_uploaded.DataBind();
            updPanel1.Update();
            upnluploaded.Update();
            return;
        }

        // -------- FIRST TIME CASE --------
        if (string.IsNullOrEmpty(hfapplcode.Value))
        {
            GridView1.DataSource = dtRequired;
            GridView1.DataBind();

            GridView1.Visible = true;
            GridView_uploaded.Visible = false;

            updPanel1.Update();
            return;
        }

        // -------- EDIT CASE --------
        long applID = Convert.ToInt64(hfapplcode.Value);
        DataTable dtUploaded = CheckDocstatus(); // documents already uploaded

        // Build pending docs table
        DataTable dtPending = dtRequired.Clone();

        // Get uploaded IDs (as string to avoid type mismatch)
        List<string> uploadedIDs = new List<string>();
        foreach (DataRow r in dtUploaded.Rows)
            uploadedIDs.Add(Convert.ToString(r["documentID"]));

        // Copy only rows not uploaded yet
        foreach (DataRow r in dtRequired.Rows)
        {
            if (!uploadedIDs.Contains(Convert.ToString(r["documentID"])))
                dtPending.ImportRow(r);
        }

        // Bind Uploaded
        GridView_uploaded.DataSource = dtUploaded.Rows.Count > 0 ? dtUploaded : null;
        GridView_uploaded.DataBind();
        GridView_uploaded.Visible = dtUploaded.Rows.Count > 0;
        upnluploaded.Update();

        // Bind Pending
        GridView1.DataSource = dtPending.Rows.Count > 0 ? dtPending : null;
        GridView1.DataBind();
        GridView1.Visible = dtPending.Rows.Count > 0;
        updPanel1.Update();


    }
    protected bool checkDuplicateAAdhaar()
    {
        try
        {
            DateTime dob = Convert.ToDateTime(txtDob.Text);
            Int32 currentCourseID = Convert.ToInt32(ddlCourse.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int32 count;

                string EncAadhaar = EConnect.Utils.Security.Encryption.Encrypt(UidNumberTxt.Text.Trim().ToUpper());

                // if (Rdoownertype.SelectedValue == "G")
                // {

                var stuBatch = (from a in context.NielitCentreBatchs
                                where a.ID.ToString() != ddlBatch.SelectedValue.ToString()
                                 //Uncommented on 27 Oct 2025
                                 && (System.Data.Entity.DbFunctions.TruncateTime(System.DateTime.Today) >= System.Data.Entity.DbFunctions.TruncateTime(a.startDate) && System.Data.Entity.DbFunctions.TruncateTime(System.DateTime.Today) <= System.Data.Entity.DbFunctions.TruncateTime(a.endDate))
                                select new
                                {
                                    batchID = a.ID
                                }).ToList();


                var studentList = (from c in context.NielitCentreStudent

                                   where c.Name.Equals(txtAppName.Text.Trim(), StringComparison.OrdinalIgnoreCase)

                                   && (c.GuardianName.ToUpper() == TxtGuardianName.Text.ToUpper().Trim() || c.FatherName.ToUpper() == txtFatherName.Text.ToUpper().Trim())
                                   && c.Gender == ddl_gender.SelectedValue
                                   && System.Data.Entity.DbFunctions.TruncateTime(c.DateOfBirth) == System.Data.Entity.DbFunctions.TruncateTime(dob)
                   && c.whether_Course_Complete == false
                                   // && c.CourseID != currentCourseID
                                   && c.UIDNumber == EncAadhaar
                   && c.projectId.ToString() == "9"
                                   && c.UIDType.ToString() == "1"

                                   select new
                                   {
                                       batch = c.batch_ID
                                   }).Distinct().ToList();

                if (studentList.Count() == 0)
                    return false;

                foreach (var batch1 in stuBatch)
                {
                    foreach (var studentList1 in studentList)
                    {
                        if (batch1.batchID == studentList1.batch)
                        {
                            return true;
                        }
                    }
                }
                return false;
                // }


            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindGender()
    {
        try
        {
            using (EConnectContext vContext = new EConnectContext())
            {
                var Gender = from s in
                                 vContext.tblGender
                             select new { ValueField = s.genderCode, TextField = s.name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddl_gender, Gender, new ListItem("--Select Gender--", "0"));

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindCourses(Int32 CentreId, int CentreType)
    {
        // 2- Centre, 0--Non Aff Instt 1--Aff Instt

        using (NIELITMISContext context = new NIELITMISContext())
        {
            ListItem lst = new ListItem("--Select One--", "0");
            using (DataTable dt = GetBatchCourseRecord(CentreId, CentreType))
            {
                //    //var Course = from p in context.NielitCentreCourses
                //    //             join k in context.NielitCourseDurations on p.ID equals k.courseID
                //    //             where p.IsVerified == true && k.isVerified==true
                //    //             orderby (p.Name)
                //    //             select new { ValueField = k.ID, TextField = p.Name+ " ( " + k.courseDurationDays +" Days )" };
                //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, Course, lst);

                if (dt.Rows.Count > 0)
                {
                    ddlCourse.DataSource = dt;
                    ddlCourse.DataTextField = "Name";
                    ddlCourse.DataValueField = "ID";
                    ddlCourse.DataBind();
                    ddlCourse.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
            }
        }
    }
    protected DataTable GetBatchCourseRecord(Int32 CentreId, int CentreType)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetBatchCourseRecord", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pCentreId", SqlDbType.BigInt).Value = CentreId;
                    cmd.Parameters.Add("@pCentreType", SqlDbType.Int).Value = CentreType;
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

    protected void RadioButtonListCentre_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        if (RadioButtonListCentre.SelectedValue.Trim() == "Y")
        {
            trappno.Visible = true;
            // trregno.Visible = true;
        }
        else
        {
            trappno.Visible = false;
            ResetAll();
        }
    }

    protected void RadioButtonListRegType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RadioButtonListRegType.SelectedValue.Trim() == "R")
        {

            trregno.Visible = true;
        }
        else if (RadioButtonListRegType.SelectedValue.Trim() == "A")
        {
            trregno.Visible = true;
            lblregno.Text = "Application No.";
            //  ResetAll();
        }

    }
    protected void btnShow_Click(object sender, System.EventArgs e)
    {
        try
        {

            if (txtregno.Text.Trim() == "")
            {
                lblerror.Visible = true;
                txtregno.Text = "";
                if (RadioButtonListRegType.SelectedValue.Trim() == "R")
                {
                    lblerror.Text = "Please Enter Registration Number.";
                }
                else
                {
                    lblerror.Text = "Please Enter Application Number.";
                }

            }
            else
            {

                bindCastCategory();
                ResetAll();

                using (EConnectContext context = new EConnectContext())
                {
                    //  var applicationcentre = context.RegistrationDetails.Where(p => p.RegistrationNo == Number).FirstOrDefault();
                    if (RadioButtonListRegType.SelectedValue.Trim() == "R")
                    {
                        Int64 Number = Convert.ToInt64(txtregno.Text);

                        var application = (from a in context.CourseRegistrationApplications
                                           join i in context.RegistrationDetails
                                               on a.CandidateID equals i.CandidateID
                                           where i.RegistrationNo == Number
                                           select new
                                           {
                                               CourseId = a.CourseID,
                                               //courseName=a.Name,
                                               BatchID = a.BatchItemID,
                                               Salutation = a.Salutation,
                                               Name = a.Name,
                                               GuardianName = a.GuardianName,
                                               FatherName = a.FatherName,
                                               MotherName = a.MotherName,
                                               Gender = a.Gender,
                                               DateOfBirth = a.DateOfBirth,
                                               CastCategoryID = a.CastCategoryID,
                                               MaritalStatusID = a.MaritalStatusID,
                                               IsHandicaped = a.IsHandicaped ? 1 : 0,
                                               IsExServicemane = a.IsExServicemane ? 1 : 0,
                                               // IsHandicaped = a.IsHandicaped ? 0 : 1,
                                               // IsExServicemane = a.IsExServicemane ? 0 : 1,
                                               Is_EWS = a.Is_EWS ? 1 : 0,
                                               ReligionID = a.ReligionID,
                                               PhoneNumber = a.PhoneNumber,
                                               StdNumber = a.StdNumber,
                                               MobileNumber = a.MobileNumber,
                                               EmailAddress = a.EmailAddress,
                                               PerAddressLine1 = a.PerAddressLine1,
                                               PerAddressLine2 = a.PerAddressLine2,
                                               PerAddressLine3 = a.PerAddressLine3,
                                               PerCityName = a.PerCityName,
                                               PerStateID = a.PerStateID,
                                               PerDistrictID = a.PerDistrictID,
                                               PerPinCode = a.PerPinCode,
                                               CorAddressLine1 = a.CorAddressLine1,
                                               CorAddressLine2 = a.CorAddressLine2,
                                               CorAddressLine3 = a.CorAddressLine3,
                                               CorCityName = a.PerCityName,
                                               CorStateID = a.CorStateID,
                                               CorDistrictID = a.CorDistrictID,
                                               CorPinCode = a.CorPinCode,
                                               UIDType = a.UIDType,
                                               UIDNumber = a.UIDNumber
                                               //  affidavit_No=a.affidavitNo,
                                               // affidavit_Date=a.affidavitDate,
                                               //affidavit_Verified=a.affidavitVerified

                                           }).FirstOrDefault();



                        //Candidate Personal Detail...
                        if (application != null)
                        {
                            // var cname = (from c in context.Courses
                            //           where c.ID == application.CourseId
                            //          select new { name = c.Name }).FirstOrDefault();
                            //if(!ddlCourse.Items.Contains(new ListItem(cname.name)))
                            ListItem item = null;
                            item = ddlCourse.Items.FindByValue(application.CourseId.ToString());
                            if (item != null)
                            {
                                ShowAlert("Processing");
                            }
                            else
                            {
                                ShowAlert("Registration no. doesnot belong to courses of the centre");
                                return;

                            }
                            ddlCourse.SelectedValue = (application.CourseId != null && application.CourseId != 0) ? application.CourseId.ToString() : "0";
                            // int id3 = Convert.ToInt32(ddlCourse.SelectedValue);
                            ddlCourse_SelectedIndexChanged(ddlCourse, EventArgs.Empty);

                            // ddlBatch.SelectedValue = application.BatchID.ToString();


                            ddlSalutaionName.SelectedValue = application.Salutation.ToString();
                            txtAppName.Text = GetInitCap(application.Name.ToString());
                            lblname.Text = txtAppName.Text;
                            if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName) == true)
                            {
                                trfather.Visible = true;
                                trmother.Visible = true;
                                trguardian.Visible = false;
                                Rdoownertype.SelectedValue = "P";
                                Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                                txtFatherName.Text = string.IsNullOrEmpty(application.FatherName) == false && !string.IsNullOrWhiteSpace(application.FatherName) ? GetInitCap(application.FatherName) : "";
                                txtMotherName.Text = string.IsNullOrEmpty(application.MotherName) == false && !string.IsNullOrWhiteSpace(application.MotherName) ? GetInitCap(application.MotherName) : "";
                            }
                            else
                            {
                                trfather.Visible = false;
                                trmother.Visible = false;
                                trguardian.Visible = true;
                                Rdoownertype.SelectedValue = "G";
                                Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                                TxtGuardianName.Text = GetInitCap(application.GuardianName);


                            }

                            ddl_gender.SelectedValue = application.Gender;
                            txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");

                            ddlCategory.SelectedValue = application.CastCategoryID.ToString();
                            ddlMStatus.SelectedValue = application.MaritalStatusID.ToString();
                            if (application.IsHandicaped.ToString() == "1")
                            {
                                Rdhandicapped.SelectedValue = "Y";
                            }
                            else
                            {
                                Rdhandicapped.SelectedValue = "N";
                            }
                            if (application.IsExServicemane.ToString() == "1")
                            {
                                Rdexserviceman.SelectedValue = "Y";
                            }
                            else
                            {
                                Rdexserviceman.SelectedValue = "N";
                            }

                            if (application.Is_EWS.ToString() == "1")
                            {
                                RdisEWS.SelectedValue = "Y";
                            }
                            else
                            {
                                RdisEWS.SelectedValue = "N";
                            }

                            //Added_For_Upload_Certificate_In_Show_16_10_2024_Start

                            //if (RdisEWS.SelectedValue == "Y" || ddlCategory.SelectedValue != "1")
                            //{
                            //    if (checkProjectMapping() == 1)
                            //    {
                            //        trCertificate.Visible = true;
                            //        trCertificateHead.Visible = true;
                            //    }
                            //    else
                            //    {
                            //        trCertificate.Visible = false;
                            //        trCertificateHead.Visible = false;
                            //    }
                            //}
                            //else
                            //{
                            //    trCertificate.Visible = false;
                            //    trCertificateHead.Visible = false;
                            //}

                            //Added_For_Upload_Certificate_In_Show_16_10_2024_End



                            ddlReligion.SelectedValue = application.ReligionID.ToString();


                            TxtSTDcode.Text = application.PhoneNumber.HasValue && application.PhoneNumber != 0 ? "0" + application.StdNumber.ToString() : "";
                            txtCorPhoneNo.Text = application.PhoneNumber.ToString();
                            txtCorMobileNo.Text = application.MobileNumber.ToString();
                            txtEmailId.Text = application.EmailAddress.ToString();


                            //Candidate Address Detail...
                            TxtPerAddressLine1.Text = string.IsNullOrEmpty(application.PerAddressLine1) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine1) ? GetInitCap(application.PerAddressLine1) : "";
                            TxtPerAddressLine2.Text = string.IsNullOrEmpty(application.PerAddressLine2) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine2) ? GetInitCap(application.PerAddressLine2) : "";
                            TxtPerAddressLine3.Text = string.IsNullOrEmpty(application.PerAddressLine3) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine3) ? GetInitCap(application.PerAddressLine3) : "";
                            TxtPerCity.Text = string.IsNullOrEmpty(application.PerCityName) == false && !string.IsNullOrWhiteSpace(application.PerCityName) ? GetInitCap(application.PerCityName) : "";
                            ddlPState.SelectedValue = application.PerStateID != null && application.PerStateID != 0 ? application.PerStateID.ToString() : "0";
                            int id1 = Convert.ToInt32(ddlPState.SelectedValue);
                            ddlPState_SelectedIndexChanged(ddlPState, EventArgs.Empty);
                            ddlPdistrict.SelectedValue = application.PerDistrictID.ToString();
                            TxtPpincode.Text = application.PerPinCode.ToString();

                            TxtCorAddressLine1.Text = string.IsNullOrEmpty(application.CorAddressLine1) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine1) ? GetInitCap(System.Web.HttpUtility.HtmlDecode(application.CorAddressLine1)) : "";
                            TxtCorAddressLine2.Text = string.IsNullOrEmpty(application.CorAddressLine2) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine2) ? GetInitCap(System.Web.HttpUtility.HtmlDecode(application.CorAddressLine2)) : "";
                            TxtCorAddressLine3.Text = string.IsNullOrEmpty(application.CorAddressLine3) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine3) ? GetInitCap(application.CorAddressLine3) : "";
                            TxtCorCity.Text = string.IsNullOrEmpty(application.CorCityName) == false && !string.IsNullOrWhiteSpace(application.CorCityName) ? GetInitCap(application.CorCityName) : "";
                            ddlCorState.SelectedValue = application.CorStateID.ToString();
                            int id2 = Convert.ToInt32(ddlCorState.SelectedValue);
                            ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                            ddlcordistrict.SelectedValue = application.CorDistrictID.ToString();
                            txtCorPinCode.Text = application.CorPinCode.ToString();
                            if (application.UIDType != null)
                            {
                                UidTypeDdl.SelectedValue = application.UIDType.ToString();
                                UidNumberTxt.Text = application.UIDNumber.ToString();
                            }

                            //txtAffidiavitNo.Text = application.affidavit_No.ToString();
                            // txtAffidiavitDate.Text = application.affidavit_Date.GetValueOrDefault().ToString("dd-MMM-yyyy");
                            //   ddlAffidiavitVerified.SelectedValue = application.affidavit_Verified.ToString();


                        }

                        if (application == null)
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "Registration Number does not exist!!.";
                        }
                    }

                    else if (RadioButtonListRegType.SelectedValue.Trim() == "A")
                    {
                        Rdhandicapped.Enabled = true;
                        string Number = Convert.ToString(txtregno.Text);
                        var application = (from a in context.CertificateExamApplications
                                               // join i in context.RegistrationDetails
                                               //   on a.CandidateID equals i.CandidateID
                                           where a.Number == Number
                                           select new
                                           {
                                               CourseId = a.CourseID,
                                               //courseName=a.Name,
                                               BatchID = a.BatchItemID,
                                               Salutation = a.Salutation,
                                               Name = a.Name,
                                               GuardianName = a.GuardianName,
                                               FatherName = a.FatherName,
                                               MotherName = a.MotherName,
                                               Gender = a.Gender,
                                               DateOfBirth = a.DateOfBirth,
                                               CastCategoryID = a.CastCategoryID,
                                               // MaritalStatusID = a.MaritalStatusID,
                                               IsHandicaped = a.IsDisability.HasValue ? (a.IsDisability.Value ? 1 : 0) : 0,
                                               // IsExServicemane = a.IsExServicemane ? 0 : 1,
                                               Is_EWS = a.Is_EWS ? 1 : 0,
                                               // ReligionID = a.ReligionID,
                                               PhoneNumber = a.PhoneNumber,
                                               StdNumber = a.StdNumber,
                                               MobileNumber = a.MobileNumber,
                                               EmailAddress = a.EmailAddress,
                                               PerAddressLine1 = a.CorAddressLine1,
                                               PerAddressLine2 = a.CorAddressLine2,
                                               PerAddressLine3 = a.CorAddressLine3,
                                               PerCityName = a.CorCityName,
                                               PerStateID = a.CorStateID,
                                               PerDistrictID = a.CorDistrictID,
                                               PerPinCode = a.CorPinCode,
                                               CorAddressLine1 = a.CorAddressLine1,
                                               CorAddressLine2 = a.CorAddressLine2,
                                               CorAddressLine3 = a.CorAddressLine3,
                                               CorCityName = a.CorCityName,
                                               CorStateID = a.CorStateID,
                                               CorDistrictID = a.CorDistrictID,
                                               CorPinCode = a.CorPinCode,
                                               UIDType = a.UIDType,
                                               UIDNumber = a.UIDNumber
                                               //  affidavit_No=a.affidavitNo,
                                               // affidavit_Date=a.affidavitDate,
                                               //affidavit_Verified=a.affidavitVerified

                                           }).FirstOrDefault();



                        //Candidate Personal Detail...
                        if (application != null)
                        {
                            ListItem item = null;
                            item = ddlCourse.Items.FindByValue(application.CourseId.ToString());
                            if (item != null)
                            {
                                ShowAlert("Processing");
                            }
                            else
                            {
                                ShowAlert("Application no. doesnot belong to courses of the centre");
                                return;

                            }
                            ddlCourse.SelectedValue = (application.CourseId != null && application.CourseId != 0) ? application.CourseId.ToString() : "0";
                            // int id3 = Convert.ToInt32(ddlCourse.SelectedValue);
                            ddlCourse_SelectedIndexChanged(ddlCourse, EventArgs.Empty);

                            // ddlBatch.SelectedValue = application.BatchID.ToString();


                            ddlSalutaionName.SelectedValue = application.Salutation.ToString();
                            txtAppName.Text = GetInitCap(application.Name.ToString());
                            lblname.Text = txtAppName.Text;
                            if (string.IsNullOrEmpty(application.GuardianName) == true && string.IsNullOrWhiteSpace(application.GuardianName) == true)
                            {
                                trfather.Visible = true;
                                trmother.Visible = true;
                                trguardian.Visible = false;
                                Rdoownertype.SelectedValue = "P";
                                Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                                txtFatherName.Text = string.IsNullOrEmpty(application.FatherName) == false && !string.IsNullOrWhiteSpace(application.FatherName) ? GetInitCap(application.FatherName) : "";
                                txtMotherName.Text = string.IsNullOrEmpty(application.MotherName) == false && !string.IsNullOrWhiteSpace(application.MotherName) ? GetInitCap(application.MotherName) : "";
                            }
                            else
                            {
                                trfather.Visible = false;
                                trmother.Visible = false;
                                trguardian.Visible = true;
                                Rdoownertype.SelectedValue = "G";
                                Rdoownertype_SelectedIndexChanged(Rdoownertype, EventArgs.Empty);
                                TxtGuardianName.Text = GetInitCap(application.GuardianName);


                            }

                            ddl_gender.SelectedValue = application.Gender;
                            // ddl_gender.Enabled=false;
                            txtDob.Text = application.DateOfBirth.ToString("dd-MMM-yyyy");

                            ddlCategory.SelectedValue = application.CastCategoryID.ToString();
                            /*
                            ddlMStatus.SelectedValue = application.MaritalStatusID.ToString();*/
                            if (application.IsHandicaped.ToString() == "1")
                            {
                                Rdhandicapped.SelectedValue = "Y";
                            }
                            else
                            {
                                Rdhandicapped.SelectedValue = "N";
                            }
                            /*if (application.IsExServicemane.ToString() == "1")
                            {
                                Rdexserviceman.SelectedValue = "Y";
                            }
                            else
                            {
                                Rdexserviceman.SelectedValue = "N";
                            }

                            */

                            if (application.Is_EWS.ToString() == "1")
                            {
                                RdisEWS.SelectedValue = "Y";
                            }
                            else
                            {
                                RdisEWS.SelectedValue = "N";
                            }

                            // ddlReligion.SelectedValue = application.ReligionID.ToString();


                            TxtSTDcode.Text = application.PhoneNumber.HasValue && application.PhoneNumber != 0 ? "0" + application.StdNumber.ToString() : "";
                            txtCorPhoneNo.Text = application.PhoneNumber.ToString();
                            txtCorMobileNo.Text = application.MobileNumber.ToString();
                            txtEmailId.Text = application.EmailAddress.ToString();


                            //Candidate Address Detail...
                            TxtPerAddressLine1.Text = string.IsNullOrEmpty(application.PerAddressLine1) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine1) ? GetInitCap(application.PerAddressLine1) : "";
                            TxtPerAddressLine2.Text = string.IsNullOrEmpty(application.PerAddressLine2) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine2) ? GetInitCap(application.PerAddressLine2) : "";
                            TxtPerAddressLine3.Text = string.IsNullOrEmpty(application.PerAddressLine3) == false && !string.IsNullOrWhiteSpace(application.PerAddressLine3) ? GetInitCap(application.PerAddressLine3) : "";
                            TxtPerCity.Text = string.IsNullOrEmpty(application.PerCityName) == false && !string.IsNullOrWhiteSpace(application.PerCityName) ? GetInitCap(application.PerCityName) : "";
                            ddlPState.SelectedValue = application.PerStateID != null && application.PerStateID != 0 ? application.PerStateID.ToString() : "0";
                            int id1 = Convert.ToInt32(ddlPState.SelectedValue);
                            ddlPState_SelectedIndexChanged(ddlPState, EventArgs.Empty);
                            ddlPdistrict.SelectedValue = application.PerDistrictID.ToString();
                            TxtPpincode.Text = application.PerPinCode.ToString();

                            TxtCorAddressLine1.Text = string.IsNullOrEmpty(application.CorAddressLine1) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine1) ? GetInitCap(application.CorAddressLine1) : "";
                            TxtCorAddressLine2.Text = string.IsNullOrEmpty(application.CorAddressLine2) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine2) ? GetInitCap(application.CorAddressLine2) : "";
                            TxtCorAddressLine3.Text = string.IsNullOrEmpty(application.CorAddressLine3) == false && !string.IsNullOrWhiteSpace(application.CorAddressLine3) ? GetInitCap(application.CorAddressLine3) : "";
                            TxtCorCity.Text = string.IsNullOrEmpty(application.CorCityName) == false && !string.IsNullOrWhiteSpace(application.CorCityName) ? GetInitCap(application.CorCityName) : "";
                            ddlCorState.SelectedValue = application.CorStateID.ToString();
                            int id2 = Convert.ToInt32(ddlCorState.SelectedValue);
                            ddlCorState_SelectedIndexChanged(ddlCorState, EventArgs.Empty);
                            ddlcordistrict.SelectedValue = application.CorDistrictID.ToString();
                            txtCorPinCode.Text = application.CorPinCode.ToString();
                            if (application.UIDType != null)
                            {
                                UidTypeDdl.SelectedValue = application.UIDType.ToString();
                                UidNumberTxt.Text = application.UIDNumber.ToString();
                            }

                            //txtAffidiavitNo.Text = application.affidavit_No.ToString();
                            // txtAffidiavitDate.Text = application.affidavit_Date.GetValueOrDefault().ToString("dd-MMM-yyyy");
                            //   ddlAffidiavitVerified.SelectedValue = application.affidavit_Verified.ToString();
                            //lblerror.Visible = true;
                            //   lblerror.Text = "Record Linked.";
                        }

                        if (application == null)
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "Application Number does not exist!!.";

                        }
                    }

                }

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected string returnDataName(Int64 vCourseID)
    {
        //START -- CODE Added on 16 Nov 2022 by DEEP NARAYAN  NIELITMIS 
        Int32 courseCatId = 0;
        string checkDbExistsData = "NA";
        if (vCourseID != 0)
        {
            // string Query1 = "select courseID from NIELITMIS.dbo.NielitCourseDuration where id=" + vCourseID;
            //  Int32 courseId = GetCourseCategoryID(Query1);

            // string Query2 = "select Course_Category_ID from NIELITMIS.dbo.NielitCentreCourse where id=" + courseId;
            //November_2024
            string Query1 = "select courseID from NIELITMIS.dbo.NielitCourseDuration where id= @vCourseID ";
            paramList.Add(new SqlParameter("@vCourseID", vCourseID));

            Int32 courseId = GetCourseCategoryID(Query1);

            //string Query2 = "select Course_Category_ID from NIELITMIS.dbo.NielitCentreCourse where id=" + courseId;

            //November_2024
            paramList.Clear();
            string Query2 = "select Course_Category_ID from NIELITMIS.dbo.NielitCentreCourse where id=@courseId ";
            paramList.Add(new SqlParameter("@courseId", courseId));
            courseCatId = GetCourseCategoryID(Query2);
            checkDbExistsData = "NIELITMIS";

            if (courseCatId == 0)
            {
                checkDbExistsData = "NIELIT";
            }
        }
        return (checkDbExistsData);
        //END -- CODE Added on 16 Nov 2022 by DEEP NARAYAN  
    }
    //START -- CODE Added on 16 Nov 2022 by DEEP NARAYAN 

    public int GetCourseCategoryID(string myQuery)
    {
        Int32 result = 0;
        try
        {
            // string result = "0";          
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            SqlCommand cmd = new SqlCommand(myQuery, conn);
            //November_2024
            cmd.Parameters.AddRange(paramList.ToArray());

            conn.Open();
            var CourseCategoryId = cmd.ExecuteScalar();
            if (CourseCategoryId != null)
            {
                result = Convert.ToInt32(CourseCategoryId.ToString());
            }
            conn.Close();
            return result;
        }
        catch (Exception exx)
        {
            return result;
        }
    }
    //END -- CODE Added on 16 Nov 2022 by DEEP NARAYAN
    protected void ddlcordistrict_SelectedIndexChanged(object sender, EventArgs e)
    {

        int x = checkAspirationalDistrict();
        if (x == 0)
        {
            ShowAlert("Correspondence district is not an Aspirational District,Check Project/District");
            ddlcordistrict.Focus();
        }
    }
    int checkAspirationalDistrict()
    {
        if (ddlProcname.SelectedValue == "1" && ddlcordistrict.SelectedValue != "0")
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString);
            try
            {
                EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "GetAspirationalDistrict";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                conn.Open();
                cmd.Parameters.Add("@DistrictId", SqlDbType.Int);
                cmd.Parameters["@DistrictId"].Value = Convert.ToInt32(ddlcordistrict.SelectedValue);
                SqlDataReader dr;
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string districtID = dr["DistrictsExists"].ToString();
                    if (districtID != null)
                    {
                        if (ddlcordistrict.SelectedValue.ToString() != districtID.ToString())
                        {

                            conn.Close();
                            dr.Dispose();
                            cmd.Dispose();
                            return 0;
                        }
                        else
                        {
                            conn.Close();
                            dr.Dispose();
                            cmd.Dispose();
                            return 1;
                        }
                    }
                    else
                    {

                        conn.Close();
                        dr.Dispose();
                        cmd.Dispose();
                        return 0;
                    }
                }
                else
                {

                    conn.Close();
                    dr.Dispose();
                    cmd.Dispose();
                    return 0;
                }

            }
            catch (Exception ex)
            {
                ShowAlert("Error, Contact Administrator");
                conn.Close();
                return 0;
            }
        }
        else
            return 1;
    }
    //Added_For_Certificate_Upload_05_09_2024_Start
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = checkProjectMapping();
        //DataRow row;
        //int i = 0;
        if (dt.Rows.Count > 0)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                //row = dt.Rows[i];

                if (row.RowType == DataControlRowType.DataRow)
                {
                    string docName = row.Cells[1].Text;
                    //if (row["doc_Name"].ToString().Contains("Caste"))
                    if (docName.ToString().Contains("Caste"))
                    {
                        if (ddlCategory.SelectedValue != "1" || (RdisEWS.SelectedValue == "Y"))
                        {

                            //trCertificate.Visible = true;
                            //trCertificateHead.Visible = true;
                            row.Enabled = true;
                        }
                        else
                        {
                            // trCertificate.Visible = false;
                            //trCertificateHead.Visible = false;
                            row.Enabled = false;
                        }
                    }
                    else
                    {
                        // trCertificate.Visible = false;
                        // trCertificateHead.Visible = false;
                        row.Enabled = true;
                    }
                }
            }
        }
    }

    protected void RdisEWS_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = checkProjectMapping();
        //DataRow row;
        //int i = 0;
        if (dt.Rows.Count > 0)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                //row = dt.Rows[i];

                if (row.RowType == DataControlRowType.DataRow)
                {
                    string docName = row.Cells[1].Text;
                    //if (row["doc_Name"].ToString().Contains("Caste"))
                    if (docName.ToString().Contains("Caste"))
                    {
                        if (ddlCategory.SelectedValue != "1" || (RdisEWS.SelectedValue == "Y"))
                        {
                            // trCertificate.Visible = true;
                            //  trCertificateHead.Visible = true;
                            row.Enabled = true;
                        }
                        else
                        {
                            // trCertificate.Visible = false;
                            //trCertificateHead.Visible = false;
                            row.Enabled = false;
                        }
                    }
                    else
                    {
                        // trCertificate.Visible = false;
                        // trCertificateHead.Visible = false;
                        row.Enabled = true;
                    }
                    //else
                    //    trCertificate.Visible = false;
                }
            }
        }
    }
    // added by amit start
    protected DataTable CheckDocstatus()
    {
        DataTable dt = new DataTable();

        try
        {
            if (string.IsNullOrEmpty(hfapplcode.Value))
                return dt;

            long applID = Convert.ToInt64(hfapplcode.Value);
            string connectionString = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;

            string sql = @"
            SELECT DISTINCT 
                dm.id as documentID,
                dm.DocumentName + '- Extension ' + dm.Type + ' (Size ' + STR(dm.SizeInKb) + ' KB)' AS doc_Name,
                dm.SizeInKb AS Size,
                dm.Type AS Extension,
                dm.docDownloadUrl
            FROM NielitCentreStudent ncs
            INNER JOIN UploadedDocs ud ON ud.Student_Id = ncs.ID
            INNER JOIN DocumentMas dm ON ud.DocumentTypeID = dm.id
            WHERE ncs.id = @applID";

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@applID", applID);
                da.Fill(dt);
            }
        }
        catch (Exception)
        {
            ShowAlert("Error occurred while checking document status.");
        }

        return dt;
    }
    // added by amit end

    protected DataTable checkProjectMapping()
    {
        DataTable dataTable = new DataTable();
        try
        {
            //Added_18_09_2024_Start
            // int documentID = 0;
            string ConnectionString1 = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;

            using (SqlConnection con = new SqlConnection(ConnectionString1))
            using (TransactionScope tr = new TransactionScope())
            {
                using (var command = new SqlCommand("CheckProjectMapping", con))
                {

                    string fileStatus = string.Empty;


                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@projectId", ddlProcname.SelectedItem.Value);
                    //  con.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    // Fill the DataTable
                    adapter.Fill(dataTable);
                    //     con.Close();
                }
                tr.Complete();
                return dataTable;

            }

        }



        catch (Exception ex)
        {
            ShowAlert("Exception Occur");
        }
        return dataTable;
    }


    //New_Added_For_Gridview_03_02_2025_Start
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            int serialNumber = e.Row.RowIndex + 1;

            // Find the Label control inside the GridView
            Label lblSerial = (Label)e.Row.FindControl("lblSerialNumber");
            if (lblSerial != null)
            {
                lblSerial.Text = serialNumber.ToString();
            }


            // Retrieve data from the current row
            DataRowView rowView = (DataRowView)e.Row.DataItem;
            string certificateName = rowView["doc_Name"].ToString();
            int certificateID = Convert.ToInt32(rowView["documentID"]);
            HyperLink hylFormat = (HyperLink)e.Row.FindControl("hylFormat");
            if (hylFormat != null)
            {
                if (rowView["docDownloadUrl"].ToString() != "")
                {
                    hylFormat.NavigateUrl = rowView["docDownloadUrl"].ToString();
                    hylFormat.Text = "Click to download format";
                }
                else
                    hylFormat.Text = "";
            }

            // Find Label inside the TemplateField
            //Label lblCertificate = (Label)e.Row.FindControl("lblCertificate");
            //if (lblCertificate != null)
            //{
            //    lblCertificate.Text = "Upload " + certificateName + " (PDF, max 100KB)";
            //}

            // Find FileUpload inside the TemplateField
            FileUpload fileCertificate = (FileUpload)e.Row.FindControl("fileCertificate");
            if (fileCertificate != null)
            {
                fileCertificate.ID = "fileCertificate" + certificateID; // Unique ID
                fileCertificate.ToolTip = "Select a valid PDF file";
            }
        }
    }

    //New_Added_For_Gridview_03_02_2025_End

}