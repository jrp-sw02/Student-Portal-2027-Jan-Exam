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

public partial class Admin_NielitCentreBatch : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int64 NielitCentrelinkedToCentreId = 0;
    Int32 NielitCentreIdFilter = 0, NonAfflAfflInstID = 0;
    Int32 UserTypeId = 0;
    Int64 aflInstId = 0;
    Int64 NonaflInstId = 0;
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
            UserTypeId = Convert.ToInt32(Session["UserType"]);

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
                                txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                NielitCentreIdFilter = NelitCentreLinkId;
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                RdoAffInstOrNonAffInst.SelectedValue = "2";
                                ddlSubcentreName.Enabled = false;
                            }
                            else
                            {
                                NielitCentres intitutesName = context1.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                                txtInstitute.Text = intitutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                                NielitCentreIdFilter = NelitCentreLinkId;
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                RdoAffInstOrNonAffInst.SelectedValue = "2";
                                ddlSubcentreName.Enabled = false;
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
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                NielitCentreIdFilter = NelitCentreLinkId;
                            }
                            RdoAffInstOrNonAffInst.Items.RemoveAt(0);
                            FillddlSubcentreName();
                            RdoAffInstOrNonAffInst.Items.RemoveAt(1);
                        }
                        else if (UserTypeId == 4)
                        {
                            var intituteslinkedToCentre = from s in context1.AffInstitutes
                                                          where s.instituteID == loginUser.UserRefNumber
                                                          select new { ID = s.ID, linkedToCentre = s.linkedToCentre, instid = s.instituteID };
                            if (intituteslinkedToCentre.Count() == 0)
                            {
                                ShowAlert("Menu is not available for the institute");
                                return;
                            }
                            NonAfflAfflInstID = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().ID);
                            AFLINSTID.Value = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().instid).ToString();
                            HNonAfflAfflInst.Value = Convert.ToString(NonAfflAfflInstID);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().linkedToCentre);
                            NielitCentres institutesName = context1.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                                NIELITCentreId.Value = NelitCentreLinkId.ToString();
                                NielitCentreIdFilter = NelitCentreLinkId;
                            }
                            RdoAffInstOrNonAffInst.Items.RemoveAt(2);
                            FillddlSubcentreName();
                            RdoAffInstOrNonAffInst.Items.RemoveAt(1);
                        }
                    }
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        BindEditNewModeData();
                        ShowEditMode();
                    }
                    else
                    {
                        BindListData();
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";
                        BindGridView();

                        if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Batch Course Centre", "Admin/NielitCentreBatch.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                        }
                        else
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Batch Course Centre", "Admin/NielitCentreBatch.aspx", ""));
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
            ddlCorporate.Items.Clear();
            ddlCorporate.Items.Add(new ListItem("--Select--", "0"));
            ddlCorporate.Items.Add(new ListItem("Yes", "Y"));
            ddlCorporate.Items.Add(new ListItem("No", "N"));
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                using (DataTable dt = GetCourseNielitCourseRecord())
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
                ListItem lst1 = new ListItem("--Select One--", "0");
                var Session = from s in context.SessionMass
                              orderby (s.ID)
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchSession, Session, lst1);


                //Added for learning Mode
                ListItem lstLearn = new ListItem("--Select One--", "0");
                var learnMode = from a in context.LearningModeMass
                                orderby (a.Name)
                                select new { ValueField = a.ID, TextField = a.Name + "(" + a.Code + ")" };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlLearningMode, learnMode, lstLearn);

                //

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetCourseNielitCourseRecord()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                if (UserTypeId == 4)
                {

                    aflInstId = Convert.ToInt64(AFLINSTID.Value);

                    using (SqlCommand cmd = new SqlCommand("TP_GetNIELITMISCourseNielitCourseRecord", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@instituteID", aflInstId);
                        cmd.Parameters.AddWithValue("@CentreType", "AFL");
                        con.Open();
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(myDt);
                        }
                    }

                }
                //else if (UserTypeId == 11 || RdoAffInstOrNonAffInst.SelectedItem.Text == "Non Accredited Centre")
                else if (RdoAffInstOrNonAffInst.SelectedItem.Text == "Non Accredited Centre")
                {

                    NonaflInstId = Convert.ToInt64(HNonAfflAfflInst.Value);
                    using (SqlCommand cmd = new SqlCommand("GetNIELITMISCourseNielitCourseForNonAfl", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(myDt);
                        }
                    }
                    //using (SqlCommand cmd = new SqlCommand("TP_GetNIELITMISCourseNielitCourseRecord", con))
                    //{
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.AddWithValue("@instituteID", NonaflInstId);
                    //    cmd.Parameters.AddWithValue("@CentreType", "AFL");
                    //    con.Open();
                    //    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    //    {
                    //        sda.Fill(myDt);
                    //    }
                    //}

                }
                else
                {

                    using (SqlCommand cmd = new SqlCommand("GetNIELITMISCourseNielitCourseRecord", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(myDt);
                        }
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
    public DataTable FillBatchNIELITMISCourseNielitCourse()
    {
        NielitCentreIdFilter = Convert.ToInt32(NIELITCentreId.Value.ToString());
        NonAfflAfflInstID = Convert.ToInt32(HNonAfflAfflInst.Value.ToString());
        Int64 coursenameid = Convert.ToInt64(ddlCourseName.SelectedValue);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("FillBatchNIELITMISCourseNielitCourse", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@NonAfflAfflInstID", SqlDbType.Int));
                    cmd.Parameters["@NonAfflAfflInstID"].Value = NonAfflAfflInstID;
                    cmd.Parameters.Add(new SqlParameter("@NielitCentreIdFilter", SqlDbType.Int));
                    cmd.Parameters["@NielitCentreIdFilter"].Value = NielitCentreIdFilter;
                    cmd.Parameters.Add(new SqlParameter("@coursenameid", SqlDbType.Int));
                    cmd.Parameters["@coursenameid"].Value = coursenameid;
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
    protected void BindListData()
    {
        try
        {
            NonAfflAfflInstID = Convert.ToInt32(HNonAfflAfflInst.Value.ToString());
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                //if (NonAfflAfflInstID == 99)
                //{
                //    var BatchName = from p in context.NielitCentreCourses
                //                    join k in context.NielitCourseDurations on p.ID equals k.courseID
                //                    join d in context.NielitCentreBatchs on k.ID equals d.CourseDurationID
                //                    where p.IsVerified == true && k.isVerified == true && d.centreID == NielitCentreIdFilter
                //                    orderby (d.Name)
                //                    select new { ValueField = d.CourseDurationID, TextField = d.Name };
                //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName.Distinct(), lst);
                //}
                //else
                //{
                //    var BatchName = from p in context.NielitCentreCourses
                //                    join k in context.NielitCourseDurations on p.ID equals k.courseID
                //                    join d in context.NielitCentreBatchs on k.ID equals d.CourseDurationID
                //                    where p.IsVerified == true && k.isVerified == true && d.centreID == NielitCentreIdFilter && d.subCentreID==NonAfflAfflInstID
                //                    orderby (d.Name)
                //                    select new { ValueField = d.CourseDurationID, TextField = d.Name };
                //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName.Distinct(), lst);
                //}
                using (DataTable dt = FillBatchNIELITMISCourseNielitCourse())
                {
                    if (dt.Rows.Count > 0)
                    {
                        ddlbatchname.DataSource = dt;
                        ddlbatchname.DataTextField = "Name";
                        ddlbatchname.DataValueField = "ID";
                        ddlbatchname.DataBind();
                        ddlbatchname.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                }
                //var coursename = from p in context.NielitCentreCourses
                //             join k in context.NielitCourseDurations on p.ID equals k.courseID
                //             where p.IsVerified == true && k.isVerified == true 
                //             orderby (p.Name)
                //                 select new { ValueField = k.ID, TextField = p.Name + " ( " + k.courseDurationDays + " Days )" };
                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, coursename, lst);  
                using (DataTable dt = GetCourseNielitCourseRecord())
                {
                    if (dt.Rows.Count > 0)
                    {

                        ddlCourseName.DataSource = dt;
                        ddlCourseName.DataTextField = "Name";
                        ddlCourseName.DataValueField = "ID";
                        ddlCourseName.DataBind();
                        ddlCourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void RdoAffInstOrNonAffInst_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            User objUser;
            fillCourseWithDuration();
            if (ddlCourse.SelectedValue == "0")
            {
                ddlSubcentreName.Enabled = false;
            }
            else
            {
                ddlSubcentreName.Enabled = true;
            }
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context2 = new NIELITMISContext())
                {
                    if (UserTypeId == 10)
                    {
                        var instituteslinkedToCentre = context2.NielitCentres.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (UserTypeId == 11)
                    {
                        var instituteslinkedToCentre = context2.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    }
                    if (NielitCentrelinkedToCentreId != 0)
                    {
                        NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        txtInstitute.Text = institutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        NIELITCentreId.Value = NelitCentreLinkId.ToString();
                        using (NIELITMISContext context = new NIELITMISContext())
                        {
                            ListItem lst1 = new ListItem("--Select One--", "0");
                            if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                            {
                                ddlSubcentreName.ClearSelection();
                                var centreName = from s in context.AffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.instituteID, TextField = s.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName.Distinct(), lst1);
                                // ddlSubcentreName.Enabled = true;
                            }
                            else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                            {
                                if (UserTypeId == 11)//Non AffInstitutes by user refNumber
                                {
                                    ddlSubcentreName.ClearSelection();
                                    var centreName = from s in context.NonAffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName.Distinct(), lst1);
                                    ddlSubcentreName.Enabled = false;
                                    var NonAfflcentre = (from p in context.NonAffInstitutes
                                                         where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                                         select p).FirstOrDefault();
                                    ddlSubcentreName.SelectedValue = NonAfflcentre.ID.ToString();
                                    //HNonAfflAfflInst.Value = NonAfflcentre.ID.ToString();
                                }
                                else //NonAffInstitutes for Nielit Centres
                                {
                                    ddlSubcentreName.ClearSelection();
                                    var centreName = from s in context.NonAffInstitutes
                                                     // join c in context.NonAffiliatedInstituteCoursesDetails on s.ID equals c.InstituteID

                                                     where s.linkedToCentre == NelitCentreLinkId
                                                     select new { ValueField = s.ID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName.Distinct(), lst1);
                                    //ddlSubcentreName.Enabled = true;

                                    var NonAfflcentre = (from p in context.NonAffInstitutes
                                                         where p.linkedToCentre == NelitCentreLinkId
                                                         select p).FirstOrDefault();
                                    if (NonAfflcentre.ToString() != "")
                                    {
                                        HNonAfflAfflInst.Value = NonAfflcentre.ID.ToString();
                                    }
                                }
                                if (UserTypeId == 4)//AffInstitutes by user refNumber
                                {
                                    ddlSubcentreName.ClearSelection();
                                    var centreName = from s in context.AffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId && s.instituteID == loginUser.UserRefNumber
                                                     select new { ValueField = s.instituteID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName.Distinct(), lst1);
                                    ddlSubcentreName.Enabled = false;
                                    var Afflcentre = (from p in context.AffInstitutes
                                                      where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                                      select p).FirstOrDefault();
                                    ddlSubcentreName.SelectedValue = Afflcentre.ID.ToString();
                                }
                                else // AffInstitutes for Nielit Centres
                                {
                                    ddlSubcentreName.ClearSelection();
                                    var centreName = from s in context.AffInstitutes
                                                     where s.linkedToCentre == NelitCentreLinkId
                                                     select new { ValueField = s.instituteID, TextField = s.Name };
                                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName.Distinct(), lst1);
                                    //ddlSubcentreName.Enabled = true;
                                }
                            }
                            else
                            {
                                ddlSubcentreName.Items.Add(new ListItem("--Select One--", "0"));
                                ddlSubcentreName.SelectedValue = "0";
                                ddlSubcentreName.Enabled = false;
                                txtName.Text = "";
                                txtBatchCode.Text = "";
                                fillCourseWithDuration();
                            }
                        }
                    }

                        // for test on 15 june 2021

                    else
                    {
                        NielitCentres institutesName = context2.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                        txtInstitute.Text = institutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        using (NIELITMISContext context = new NIELITMISContext())
                        {
                            ListItem lst1 = new ListItem("--Select One--", "0");
                            if (RdoAffInstOrNonAffInst.SelectedValue == "1")
                            {
                                ddlSubcentreName.ClearSelection();
                                var centreName = from s in context.AffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.instituteID, TextField = s.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName.Distinct(), lst1);
                                //ddlSubcentreName.Enabled = true;
                                txtName.Text = "";
                                txtBatchCode.Text = "";
                            }
                            else if (RdoAffInstOrNonAffInst.SelectedValue == "0")
                            {

                                ddlSubcentreName.ClearSelection();
                                var centreName = from s in context.NonAffInstitutes
                                                 where s.linkedToCentre == NelitCentreLinkId
                                                 select new { ValueField = s.ID, TextField = s.Name };
                                EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName.Distinct(), lst1);
                                //ddlSubcentreName.Enabled = true;
                                txtName.Text = "";
                                txtBatchCode.Text = "";
                            }
                            else
                            {
                                ddlSubcentreName.Items.Add(new ListItem("--Select One--", "0"));
                                ddlSubcentreName.SelectedValue = "0";
                                ddlSubcentreName.Enabled = false;
                            }
                        }
                    }
                }
            }
            //if (RdoAffInstOrNonAffInst.SelectedItem.Text == "Non Accredited Centre")
            //{
            BindEditNewModeData();
            //BindListData();
            //}
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        }
    }
    protected void FillddlSubcentreName()
    {
        try
        {
            User objUser;
            using (EConnectContext context1 = new EConnectContext())
            {
                objUser = new EConnect.URM.User();
                ListItem lst1 = new ListItem("--Select One--", "0");
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                    txtInstitute.Text = institutesName.Name;
                    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                    //lblCentreId.Text = NelitCentreLinkId.ToString();
                    ddlSubcentreName.ClearSelection();
                    if (UserTypeId == 11)
                    {
                        var centreName = from s in context.NonAffInstitutes
                                         where s.linkedToCentre == NelitCentreLinkId && s.ID == loginUser.UserRefNumber
                                         select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName.Distinct(), lst1);
                        ddlSubcentreName.Enabled = false;
                        var NonAfflcentre = (from p in context.NonAffInstitutes
                                             where p.linkedToCentre == NelitCentreLinkId && p.ID == loginUser.UserRefNumber
                                             select p).FirstOrDefault();
                        ddlSubcentreName.SelectedValue = NonAfflcentre.ID.ToString();
                        RdoAffInstOrNonAffInst.SelectedValue = "0";
                    }
                    else
                    {
                        var centreName = from s in context.AffInstitutes
                                         where s.linkedToCentre == NelitCentreLinkId && s.instituteID == loginUser.UserRefNumber
                                         select new { ValueField = s.instituteID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName.Distinct(), lst1);
                        ddlSubcentreName.Enabled = false;
                        var NonAfflcentre = (from p in context.AffInstitutes
                                             where p.linkedToCentre == NelitCentreLinkId && p.instituteID == loginUser.UserRefNumber
                                             select p).FirstOrDefault();
                        ddlSubcentreName.SelectedValue = NonAfflcentre.ID.ToString();
                        RdoAffInstOrNonAffInst.SelectedValue = "1";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected bool IsValidForm()
    {
        try
        {
            if (!isSelected(ddlSubcentreName))
            {
                //lblerrorddlcouse.Visible = true;
                //lblerrorddlcouse.Text = "Please Select the Project Name";
                return false;
            }
            if (ddlCorporate.SelectedValue == "Y")
            {
                if (!isBlandTextBox(txtOrgTrained))
                {
                    //lblerrorddlcouse.Visible = true;
                    //lblerrorddlcouse.Text = "Please enter the Organisation Trained.";                   
                    return false;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool isSelected(DropDownList ddlSubcentreName)
    {
        try
        {
            if (RdoAffInstOrNonAffInst.SelectedValue != "2")
            {
                if (ddlSubcentreName.SelectedValue == "0")
                {
                    ddlSubcentreName.Focus();
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
    protected bool isBlandTextBox(TextBox txtOrgTrained)
    {
        try
        {

            if (txtOrgTrained.Text == "")
            {
                ddlSubcentreName.Focus();
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
    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            fillCourseWithDuration();
            // Add for isSemBased
            trsemyearBased.Visible = false;
            trsemyearBased1.Visible = false;
            ddlIsSemBased.ClearSelection();

            //end
            ddlSubcentreName.Enabled = true;
            NIELITMISContext context2 = new NIELITMISContext();
            if (RdoAffInstOrNonAffInst.SelectedItem.Text == "Non Accredited Centre")
            {

                EConnectContext context = new EConnectContext();
                User objUser;
                Int64 NonAfflLinkId = 0;
                objUser = new EConnect.URM.User();
                User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 CourseIdCumDurationId = Convert.ToInt32(ddlCourse.SelectedValue);
                ListItem lst1 = new ListItem("--Select One--", "0");
                Int32 courseId = Convert.ToInt32(ddlCourse.SelectedValue);
                ddlSubcentreName.ClearSelection();
                ddlSubcentreName.Items.Clear();
                if (UserTypeId == 10)
                {
                    var instituteslinkedToCentre = context2.NielitCentres.Find(loginUser.UserRefNumber);
                    NonAfflLinkId = Convert.ToInt32(instituteslinkedToCentre.ID);
                    var centreName = from s in context2.NonAffInstitutes
                                     join c in context2.NonAffiliatedInstituteCoursesDetails on s.ID equals c.InstituteID
                                     where c.CourseID == CourseIdCumDurationId && s.linkedToCentre == NonAfflLinkId
                                     && c.CourseID == courseId
                                     select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName.Distinct(), lst1);
                }
                if (UserTypeId == 11)
                {
                    var instituteslinkedToCentre = context2.NonAffInstitutes.Find(loginUser.UserRefNumber);
                    NonAfflLinkId = Convert.ToInt32(instituteslinkedToCentre.linkedToCentre);
                    Int32 NonAfflId = Convert.ToInt32(instituteslinkedToCentre.ID);
                    var centreName = from s in context2.NonAffInstitutes
                                     join c in context2.NonAffiliatedInstituteCoursesDetails on s.ID equals c.InstituteID
                                     where c.CourseID == CourseIdCumDurationId && s.ID == NonAfflId
                                     && c.CourseID == courseId
                                     select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName.Distinct(), lst1);
                    // ddlSubcentreName.SelectedValue = NonAfflId.ToString();
                    //ddlSubcentreName.Enabled = false;
                }



            }

            // Add for isSemBased

            Int32 CourseId = Convert.ToInt32(ddlCourse.SelectedValue);

            var application = (from p in context2.NielitCentreCourses
                               join d in context2.NielitCourseDurations on p.ID equals d.courseID
                               where d.ID == CourseId && p.IsVerified == true && p.CourseCategoryID == 101
                               select new
                               {
                                   ID = p.ID,
                                   Name = p.Name,
                                   CourseCatId = p.CourseCategoryID

                               }).FirstOrDefault();

            if (application != null)
            {
                trsembased.Visible = true;
                trsembased1.Visible = true;
                truniversity.Visible = true;
                truniversity1.Visible = true;
                
            }
            else if (application == null)
            {
                trsembased.Visible = false;
                trsembased1.Visible = false;
                truniversity.Visible = false;
                truniversity1.Visible = false;
                
            }


            //end

        }

        catch (Exception ex)
        {
            txtName.Text = "";
            txtBatchCode.Text = "";
            ShowAlert(ex.Message, true);
        }
    }

    protected void ddlIsSemBased_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlIsSemBased.SelectedValue.Trim() == "Y")
            {
                trsemyearBased.Visible = true;
                trsemyearBased1.Visible = true;
            }
            else if (ddlIsSemBased.SelectedValue.Trim() == "N")
            {
                trsemyearBased.Visible = false;
                trsemyearBased1.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void fillCourseWithNielitCourse()
    {
        try
        {
            Int32 courseid = Convert.ToInt32(ddlCourse.SelectedValue);
            if (courseid != 0)
            {
                lblerrorddlcouse.Text = "";
                Int32 ddlsubcentreId = Convert.ToInt32(ddlSubcentreName.SelectedValue);
                if (ddlsubcentreId == 0)
                {
                    Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);
                    using (NIELITMISContext context = new NIELITMISContext())
                    {
                        EConnectContext context1 = new EConnectContext();
                        int courseIdExists = (from b in context.NielitCentreBatchs
                                              where b.centreID == nielitcentreid && b.CourseDurationID == courseid
                                              select b).Count();
                        if (courseIdExists > 0)
                        {
                            var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                     where p.centreID == nielitcentreid && p.CourseDurationID == courseid
                                                     orderby p.ID descending
                                                     select new
                                                     {
                                                         BatchName = p.Name,
                                                         BatchCode = p.BatchCode
                                                     }).First();
                            var nielitCourse = (from p in context1.Courses
                                                where p.ID == courseid
                                                select new
                                                {
                                                    CourseName = p.Name,
                                                    CourseCode = p.Code
                                                }).FirstOrDefault();
                            string batchcode = nielitCentreBatch.BatchCode;
                            string batchname = nielitCentreBatch.BatchName;
                            string BatchNameStr = batchname.Substring(batchname.LastIndexOf("/") + 1);

                            Int32 batchNameSrNum = Convert.ToInt32(BatchNameStr) + 1;
                            string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCourse.CourseCode.ToString() + "/" + batchNameSrNum.ToString();

                            txtName.Text = batchNameFormate;
                            // txtBatchCode.Text = batchcodeFormate;
                        }
                        else
                        {
                            int BatchNameExists = (from b in context.NielitCentreBatchs
                                                   where b.CourseDurationID == courseid //&& b.centreID==nielitcentreid
                                                   select b).Count();
                            if (BatchNameExists == 0)
                            {
                                var nielitCentreCourse = (from p in context1.Courses
                                                          where p.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code
                                                          }).FirstOrDefault();
                                string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                txtName.Text = batchNameFormate;
                            }
                            else
                            {
                                var nielitCentreCourse = (from p in context1.Courses
                                                          where p.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code
                                                          }).FirstOrDefault();
                                string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                txtName.Text = batchNameFormate;
                            }

                        }// Batch name autro fill end code

                        int BatchCodeExists = (from b in context.NielitCentreBatchs
                                               where b.CourseDurationID == courseid
                                               select b).Count();
                        if (BatchCodeExists > 0)
                        {
                            var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                     where p.CourseDurationID == courseid
                                                     orderby p.ID descending
                                                     select new
                                                     {
                                                         BatchName = p.Name,
                                                         BatchCode = p.BatchCode
                                                     }).First();
                            var nielitCentreCourse = (from p in context1.Courses
                                                      where p.ID == courseid
                                                      select new
                                                      {
                                                          CourseName = p.Name,
                                                          CourseCode = p.Code
                                                      }).FirstOrDefault();
                            string batchcode = nielitCentreBatch.BatchCode;
                            //string batchname = nielitCentreBatch.BatchName;
                            string BatchCodeStr = batchcode.Substring(batchcode.LastIndexOf("/") + 1);

                            Int32 batchCodeSrNum = Convert.ToInt32(BatchCodeStr) + 1;
                            string batchCodeFormate = nielitCentreCourse.CourseCode.ToString() + "/" + batchCodeSrNum.ToString();
                            //txtName.Text = batchNameFormate;
                            txtBatchCode.Text = batchCodeFormate;

                        }
                        else
                        {
                            var nielitCourse = (from p in context1.Courses
                                                where p.ID == courseid
                                                select new
                                                {
                                                    CourseName = p.Name,
                                                    CourseCode = p.Code
                                                }).FirstOrDefault();
                            string batchCodeFormate = nielitCourse.CourseCode.ToString() + "/" + 1;
                            //txtName.Text = batchNameFormate;
                            txtBatchCode.Text = batchCodeFormate;
                        }
                    }
                }
                else
                {
                    using (NIELITMISContext context = new NIELITMISContext())
                    {
                        EConnectContext context1 = new EConnectContext();
                        int courseIdExists = (from b in context.NielitCentreBatchs
                                              where b.subCentreID == ddlsubcentreId && b.CourseDurationID == courseid
                                              select b).Count();
                        if (courseIdExists > 0)//Exists subcentreid and courseid                        
                        {
                            var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                     where p.subCentreID == ddlsubcentreId && p.CourseDurationID == courseid
                                                     orderby p.ID descending
                                                     select new
                                                     {
                                                         BatchName = p.Name,
                                                         BatchCode = p.BatchCode
                                                     }).First();
                            var nielitCentreCourse = (from p in context1.Courses
                                                      where p.ID == courseid
                                                      select new
                                                      {
                                                          CourseName = p.Name,
                                                          CourseCode = p.Code
                                                      }).FirstOrDefault();
                            string batchcode = nielitCentreBatch.BatchCode;
                            string batchname = nielitCentreBatch.BatchName;
                            string BatchNameStr = batchname.Substring(batchname.LastIndexOf("/") + 1);

                            Int32 batchNameSrNum = Convert.ToInt32(BatchNameStr) + 1;
                            string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode.ToString() + "/" + batchNameSrNum.ToString();

                            txtName.Text = batchNameFormate;
                            // txtBatchCode.Text = batchcodeFormate;
                        }
                        else
                        {
                            int BatchNameExists = (from b in context.NielitCentreBatchs
                                                   where b.CourseDurationID == courseid
                                                   select b).Count();
                            if (BatchNameExists > 0)
                            {
                                int BatchNameAndSubCentreIDExists = (from b in context.NielitCentreBatchs
                                                                     where b.centreID == ddlsubcentreId && b.CourseDurationID == courseid
                                                                     select b).Count();
                                if (BatchNameAndSubCentreIDExists == 0)
                                {
                                    var nielitCentreCourse = (from p in context1.Courses
                                                              where p.ID == courseid
                                                              select new
                                                              {
                                                                  CourseName = p.Name,
                                                                  CourseCode = p.Code
                                                              }).FirstOrDefault();

                                    string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                    txtName.Text = batchNameFormate;
                                }
                            }
                            else
                            {
                                var nielitCentreCourse = (from p in context1.Courses
                                                          where p.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code
                                                          }).FirstOrDefault();
                                string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                txtName.Text = batchNameFormate;
                            }
                        }
                        int BatchCodeExists = (from b in context.NielitCentreBatchs
                                               where b.CourseDurationID == courseid
                                               select b).Count();
                        if (BatchCodeExists > 0)
                        {
                            var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                     where p.CourseDurationID == courseid
                                                     orderby p.ID descending
                                                     select new
                                                     {
                                                         BatchName = p.Name,
                                                         BatchCode = p.BatchCode
                                                     }).First();
                            var nielitCentreCourse = (from p in context1.Courses
                                                      where p.ID == courseid
                                                      select new
                                                      {
                                                          CourseName = p.Name,
                                                          CourseCode = p.Code
                                                      }).FirstOrDefault();
                            string batchcode = nielitCentreBatch.BatchCode;
                            string BatchCodeStr = batchcode.Substring(batchcode.LastIndexOf("/") + 1);
                            Int32 batchCodeSrNum = Convert.ToInt32(BatchCodeStr) + 1;
                            string batchCodeFormate = nielitCentreCourse.CourseCode.ToString() + "/" + batchCodeSrNum.ToString();
                            txtBatchCode.Text = batchCodeFormate;
                        }
                        else
                        {
                            var nielitCourse = (from p in context1.Courses
                                                where p.ID == courseid
                                                select new
                                                {
                                                    CourseName = p.Name,
                                                    CourseCode = p.Code
                                                }).FirstOrDefault();
                            string batchCodeFormate = nielitCourse.CourseCode.ToString() + "/" + 1;
                            txtBatchCode.Text = batchCodeFormate;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            txtName.Text = "";
            txtBatchCode.Text = "";
            ShowAlert(ex.Message, true);
        }
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
            conn.Open();
            string CourseCategoryId = cmd.ExecuteScalar().ToString();
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


    // Deep Modified function on 17 Aug 2020
    protected void fillCourseWithDuration()
    {
        try
        {            
            Int32 courseid = Convert.ToInt32(ddlCourse.SelectedValue);

            //START -- CODE Added on 16 Nov 2022 by DEEP NARAYAN  NIELITMIS 
            Int32 courseCatId = 0;
            string checkDbExistsData = "NA";
            if (courseid != 0)
            {
                string Query1 = "select courseID from NIELITMIS.dbo.NielitCourseDuration where id=" + courseid;
                Int32 courseId = GetCourseCategoryID(Query1);

                string Query2 = "select Course_Category_ID from NIELITMIS.dbo.NielitCentreCourse where id=" + courseId;
                courseCatId = GetCourseCategoryID(Query2);
                checkDbExistsData = "NIELITMIS";
                
                if (courseCatId == 0)
                {
                    checkDbExistsData = "NIELIT";
                }
            }
            //END -- CODE Added on 16 Nov 2022 by DEEP NARAYAN                    

            Int32 CourseType = courseid.ToString().Length;
            //if (CourseType > 3)

            if (checkDbExistsData == "NIELITMIS")
            {
                if (courseid != 0)
                {
                    lblerrorddlcouse.Text = "";
                    Int32 ddlsubcentreId = Convert.ToInt32(ddlSubcentreName.SelectedValue);
                    if (ddlsubcentreId == 0)
                    {
                        Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);
                        using (NIELITMISContext context = new NIELITMISContext())
                        {

                            int courseIdExists = (from b in context.NielitCentreBatchs
                                                  where b.centreID == nielitcentreid && b.CourseDurationID == courseid
                                                  select b).Count();
                            if (courseIdExists > 0)
                            {
                                var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                         where p.centreID == nielitcentreid && p.CourseDurationID == courseid
                                                         orderby p.ID descending
                                                         select new
                                                         {
                                                             BatchName = p.Name,
                                                             BatchCode = p.BatchCode
                                                         }).First();
                                var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                          join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                          where k.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code,
                                                              CourseDurationID = k.ID

                                                          }).FirstOrDefault();
                                string batchcode = nielitCentreBatch.BatchCode;
                                string batchname = nielitCentreBatch.BatchName;
                                string BatchNameStr = batchname.Substring(batchname.LastIndexOf("/") + 1);

                                Int32 batchNameSrNum = Convert.ToInt32(BatchNameStr) + 1;
                                //string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCentreCourse.CourseCode.ToString() + "/" + batchNameSrNum.ToString();
                                string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCentreCourse.CourseDurationID.ToString() + "/" + batchNameSrNum.ToString();
                                txtName.Text = batchNameFormate;
                                // txtBatchCode.Text = batchcodeFormate;
                            }
                            else
                            {
                                int BatchNameExists = (from b in context.NielitCentreBatchs
                                                       where b.CourseDurationID == courseid //&& b.centreID==nielitcentreid
                                                       select b).Count();
                                if (BatchNameExists == 0)
                                {
                                    var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                              join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                              where k.ID == courseid
                                                              select new
                                                              {
                                                                  CourseName = p.Name,
                                                                  CourseCode = p.Code,
                                                                  CourseDurationID = k.ID
                                                              }).FirstOrDefault();
                                    //string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                    string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCentreCourse.CourseDurationID.ToString() + "/" + 1;
                                    txtName.Text = batchNameFormate;
                                }
                                else
                                {
                                    var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                              join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                              where k.ID == courseid
                                                              select new
                                                              {
                                                                  CourseName = p.Name,
                                                                  CourseCode = p.Code,
                                                                  CourseDurationID = k.ID
                                                              }).FirstOrDefault();
                                    //string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                    string batchNameFormate = nielitcentreid.ToString() + "/" + nielitCentreCourse.CourseDurationID.ToString() + "/" + 1;
                                    txtName.Text = batchNameFormate;
                                }

                            }// Batch name autro fill end code

                            int BatchCodeExists = (from b in context.NielitCentreBatchs
                                                   where b.CourseDurationID == courseid
                                                   select b).Count();
                            if (BatchCodeExists > 0)
                            {
                                var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                         where p.CourseDurationID == courseid
                                                         orderby p.ID descending
                                                         select new
                                                         {
                                                             BatchName = p.Name,
                                                             BatchCode = p.BatchCode
                                                         }).First();
                                var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                          join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                          where k.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code,
                                                              CourseDurationID = k.ID
                                                          }).FirstOrDefault();
                                string batchcode = nielitCentreBatch.BatchCode;
                                //string batchname = nielitCentreBatch.BatchName;
                                string BatchCodeStr = batchcode.Substring(batchcode.LastIndexOf("/") + 1);

                                Int32 batchCodeSrNum = Convert.ToInt32(BatchCodeStr) + 1;
                                //string batchCodeFormate = nielitCentreCourse.CourseCode.ToString() + "/" + batchCodeSrNum.ToString();
                                string batchCodeFormate = nielitCentreCourse.CourseDurationID.ToString() + "/" + batchCodeSrNum.ToString();
                                //txtName.Text = batchNameFormate;
                                txtBatchCode.Text = batchCodeFormate;

                            }
                            else
                            {
                                var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                          join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                          where k.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code,
                                                              CourseDurationID = k.ID
                                                          }).FirstOrDefault();
                                //string batchCodeFormate = nielitCentreCourse.CourseCode.ToString() + "/" + 1;
                                string batchCodeFormate = nielitCentreCourse.CourseDurationID.ToString() + "/" + 1;
                                //txtName.Text = batchNameFormate;
                                txtBatchCode.Text = batchCodeFormate;
                            }
                        }
                    }
                    else
                    {
                        using (NIELITMISContext context = new NIELITMISContext())
                        {
                            int courseIdExists = (from b in context.NielitCentreBatchs
                                                  where b.subCentreID == ddlsubcentreId && b.CourseDurationID == courseid
                                                  select b).Count();
                            if (courseIdExists > 0)//Exists subcentreid and courseid                        
                            {
                                var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                         where p.subCentreID == ddlsubcentreId && p.CourseDurationID == courseid
                                                         orderby p.ID descending
                                                         select new
                                                         {
                                                             BatchName = p.Name,
                                                             BatchCode = p.BatchCode
                                                         }).First();
                                var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                          join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                          where k.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code,
                                                              CourseDurationID = k.ID
                                                          }).FirstOrDefault();
                                string batchcode = nielitCentreBatch.BatchCode;
                                string batchname = nielitCentreBatch.BatchName;
                                string BatchNameStr = batchname.Substring(batchname.LastIndexOf("/") + 1);

                                Int32 batchNameSrNum = Convert.ToInt32(BatchNameStr) + 1;
                                //string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode.ToString() + "/" + batchNameSrNum.ToString();
                                string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseDurationID.ToString() + "/" + batchNameSrNum.ToString();
                                txtName.Text = batchNameFormate;
                                // txtBatchCode.Text = batchcodeFormate;
                            }
                            else
                            {
                                int BatchNameExists = (from b in context.NielitCentreBatchs
                                                       where b.CourseDurationID == courseid
                                                       select b).Count();
                                if (BatchNameExists > 0)
                                {
                                    int BatchNameAndSubCentreIDExists = (from b in context.NielitCentreBatchs
                                                                         where b.centreID == ddlsubcentreId && b.CourseDurationID == courseid
                                                                         select b).Count();
                                    if (BatchNameAndSubCentreIDExists == 0)
                                    {
                                        var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                                  join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                                  where k.ID == courseid
                                                                  select new
                                                                  {
                                                                      CourseName = p.Name,
                                                                      CourseCode = p.Code,
                                                                      CourseDurationID = k.ID
                                                                  }).FirstOrDefault();

                                        //string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                        string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseDurationID.ToString() + "/" + 1;
                                        txtName.Text = batchNameFormate;
                                    }
                                }
                                else
                                {
                                    var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                              join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                              where k.ID == courseid
                                                              select new
                                                              {
                                                                  CourseName = p.Name,
                                                                  CourseCode = p.Code,
                                                                  CourseDurationID = k.ID
                                                              }).FirstOrDefault();
                                    //string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                    string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseDurationID.ToString() + "/" + 1;
                                    txtName.Text = batchNameFormate;
                                }
                            }
                            int BatchCodeExists = (from b in context.NielitCentreBatchs
                                                   where b.CourseDurationID == courseid
                                                   select b).Count();
                            if (BatchCodeExists > 0)
                            {
                                var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                         where p.CourseDurationID == courseid
                                                         orderby p.ID descending
                                                         select new
                                                         {
                                                             BatchName = p.Name,
                                                             BatchCode = p.BatchCode
                                                         }).First();
                                var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                          join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                          where k.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code,
                                                              CourseDurationID = k.ID
                                                          }).FirstOrDefault();
                                string batchcode = nielitCentreBatch.BatchCode;
                                string BatchCodeStr = batchcode.Substring(batchcode.LastIndexOf("/") + 1);
                                Int32 batchCodeSrNum = Convert.ToInt32(BatchCodeStr) + 1;
                                //string batchCodeFormate = nielitCentreCourse.CourseCode.ToString() + "/" + batchCodeSrNum.ToString();
                                string batchCodeFormate = nielitCentreCourse.CourseDurationID.ToString() + "/" + batchCodeSrNum.ToString();
                                txtBatchCode.Text = batchCodeFormate;
                            }
                            else
                            {
                                var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                          join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                          where k.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code,
                                                              CourseDurationID = k.ID
                                                          }).FirstOrDefault();
                                // string batchCodeFormate = nielitCentreCourse.CourseCode.ToString() + "/" + 1;
                                string batchCodeFormate = nielitCentreCourse.CourseDurationID.ToString() + "/" + 1;
                                txtBatchCode.Text = batchCodeFormate;
                            }
                        }
                    }
                }
            }

            //else if (CourseType <= 3) 
            else if (checkDbExistsData == "NIELIT")
            {
                fillCourseWithNielitCourse();
            }
            else
            {
                ShowAlert("Please select one of  the Course.");
                txtName.Text = "";
                txtBatchCode.Text = "";
                lblerrorddlcouse.Text = "Please select one of  the Course.";
            }
        }
        catch (Exception ex)
        {
            txtName.Text = "";
            txtBatchCode.Text = "";
            ShowAlert(ex.Message, true);
        }
    }

    // Deep Modified function END on 17 Aug 2020
    protected void ddlSubcentreName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            Int32 courseid = Convert.ToInt32(ddlCourse.SelectedValue);
            Int32 ddlsubcentreId = Convert.ToInt32(ddlSubcentreName.SelectedValue);
            lblerrorddlcouse.Text = "";
            Int32 CourseType = courseid.ToString().Length;

            if (courseid == 0)
            {
                ShowAlert("Please select course.", true);
                return;
            }

            if (ddlsubcentreId != 0)
            {
                //START -- CODE Added on 16 Nov 2022 by DEEP NARAYAN  NIELITMIS
                Int32 courseCatId = 0;
                string checkDbExistsData = "NA";
                if (courseid != 0)
                {
                    string Query1 = "select courseID from NIELITMIS.dbo.NielitCourseDuration where id=" + courseid;
                    Int32 courseId = GetCourseCategoryID(Query1);

                    string Query2 = "select Course_Category_ID from NIELITMIS.dbo.NielitCentreCourse where id=" + courseId;
                    courseCatId = GetCourseCategoryID(Query2);
                    checkDbExistsData = "NIELITMIS";

                    if (courseCatId == 0)
                    {
                        checkDbExistsData = "NIELIT";
                    }
                }
                //END -- CODE Added on 16 Nov 2022 by DEEP NARAYAN   


                //if (CourseType > 3)

                if (checkDbExistsData == "NIELITMIS")
                {
                    using (NIELITMISContext context = new NIELITMISContext())
                    {

                        int courseIdExists = (from b in context.NielitCentreBatchs
                                              where b.subCentreID == ddlsubcentreId && b.CourseDurationID == courseid
                                              select b).Count();
                        if (courseIdExists > 0)//Exists subcentreid and courseid                        
                        {
                            var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                     where p.subCentreID == ddlsubcentreId && p.CourseDurationID == courseid
                                                     orderby p.ID descending
                                                     select new
                                                     {
                                                         BatchName = p.Name,
                                                         BatchCode = p.BatchCode
                                                     }).First();
                            var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                      join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                      where k.ID == courseid
                                                      select new
                                                      {
                                                          CourseName = p.Name,
                                                          CourseCode = p.Code,
                                                          CourseDurationID = k.ID
                                                      }).FirstOrDefault();
                            string batchcode = nielitCentreBatch.BatchCode;
                            string batchname = nielitCentreBatch.BatchName;
                            string BatchNameStr = batchname.Substring(batchname.LastIndexOf("/") + 1);

                            Int32 batchNameSrNum = Convert.ToInt32(BatchNameStr) + 1;
                            //string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode.ToString() + "/" + batchNameSrNum.ToString();
                            string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseDurationID.ToString() + "/" + batchNameSrNum.ToString();

                            txtName.Text = batchNameFormate;
                            // txtBatchCode.Text = batchcodeFormate;
                        }
                        else
                        {
                            int BatchNameExists = (from b in context.NielitCentreBatchs
                                                   where b.CourseDurationID == courseid
                                                   select b).Count();
                            if (BatchNameExists > 0)
                            {
                                int BatchNameAndSubCentreIDExists = (from b in context.NielitCentreBatchs
                                                                     where b.subCentreID == ddlsubcentreId && b.CourseDurationID == courseid
                                                                     select b).Count();
                                if (BatchNameAndSubCentreIDExists == 0)
                                {
                                    var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                              join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                              where k.ID == courseid
                                                              select new
                                                              {
                                                                  CourseName = p.Name,
                                                                  CourseCode = p.Code,
                                                                  CourseDurationID = k.ID
                                                              }).FirstOrDefault();
                                    //string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                    string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseDurationID.ToString() + "/" + 1;

                                    txtName.Text = batchNameFormate;
                                }
                            }
                            else
                            {
                                var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                          join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                          where k.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code,
                                                              CourseDurationID = k.ID
                                                          }).FirstOrDefault();
                                //string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseDurationID.ToString() + "/" + 1;
                                txtName.Text = batchNameFormate;
                            }
                        }
                        int BatchCodeExists = (from b in context.NielitCentreBatchs
                                               where b.CourseDurationID == courseid
                                               select b).Count();
                        if (BatchCodeExists > 0)
                        {
                            var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                     where p.CourseDurationID == courseid
                                                     orderby p.ID descending
                                                     select new
                                                     {
                                                         BatchName = p.Name,
                                                         BatchCode = p.BatchCode
                                                     }).First();
                            var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                      join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                      where k.ID == courseid
                                                      select new
                                                      {
                                                          CourseName = p.Name,
                                                          CourseCode = p.Code,
                                                          CourseDurationID = k.ID
                                                      }).FirstOrDefault();
                            string batchcode = nielitCentreBatch.BatchCode;
                            string BatchCodeStr = batchcode.Substring(batchcode.LastIndexOf("/") + 1);
                            Int32 batchCodeSrNum = Convert.ToInt32(BatchCodeStr) + 1;

                            //string batchCodeFormate = nielitCentreCourse.CourseCode.ToString() + "/" + batchCodeSrNum.ToString();
                            string batchCodeFormate = nielitCentreCourse.CourseDurationID.ToString() + "/" + batchCodeSrNum.ToString();
                            txtBatchCode.Text = batchCodeFormate;
                        }
                        else
                        {
                            var nielitCentreCourse = (from p in context.NielitCentreCourses
                                                      join k in context.NielitCourseDurations on p.ID equals k.courseID
                                                      where k.ID == courseid
                                                      select new
                                                      {
                                                          CourseName = p.Name,
                                                          CourseCode = p.Code,
                                                          CourseDurationID = k.ID
                                                      }).FirstOrDefault();
                            //string batchCodeFormate = nielitCentreCourse.CourseCode.ToString() + "/" + 1;
                            string batchCodeFormate = nielitCentreCourse.CourseDurationID.ToString() + "/" + 1;
                            txtBatchCode.Text = batchCodeFormate;
                        }
                    }
                }
                //else if (CourseType <= 3)
                else if (checkDbExistsData == "NIELIT")
                {
                    using (NIELITMISContext context = new NIELITMISContext())
                    {
                        EConnectContext context1 = new EConnectContext();
                        int courseIdExists = (from b in context.NielitCentreBatchs
                                              where b.subCentreID == ddlsubcentreId && b.CourseDurationID == courseid
                                              select b).Count();
                        if (courseIdExists > 0)//Exists subcentreid and courseid                        
                        {
                            var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                     where p.subCentreID == ddlsubcentreId && p.CourseDurationID == courseid
                                                     orderby p.ID descending
                                                     select new
                                                     {
                                                         BatchName = p.Name,
                                                         BatchCode = p.BatchCode
                                                     }).First();
                            var nielitCentreCourse = (from p in context1.Courses
                                                      where p.ID == courseid
                                                      select new
                                                      {
                                                          CourseName = p.Name,
                                                          CourseCode = p.Code
                                                      }).FirstOrDefault();
                            string batchcode = nielitCentreBatch.BatchCode;
                            string batchname = nielitCentreBatch.BatchName;
                            string BatchNameStr = batchname.Substring(batchname.LastIndexOf("/") + 1);

                            Int32 batchNameSrNum = Convert.ToInt32(BatchNameStr) + 1;
                            string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode.ToString() + "/" + batchNameSrNum.ToString();

                            txtName.Text = batchNameFormate;
                            // txtBatchCode.Text = batchcodeFormate;
                        }
                        else
                        {
                            int BatchNameExists = (from b in context.NielitCentreBatchs
                                                   where b.CourseDurationID == courseid
                                                   select b).Count();
                            if (BatchNameExists > 0)
                            {
                                int BatchNameAndSubCentreIDExists = (from b in context.NielitCentreBatchs
                                                                     where b.subCentreID == ddlsubcentreId && b.CourseDurationID == courseid
                                                                     select b).Count();
                                if (BatchNameAndSubCentreIDExists == 0)
                                {
                                    var nielitCentreCourse = (from p in context1.Courses
                                                              where p.ID == courseid
                                                              select new
                                                              {
                                                                  CourseName = p.Name,
                                                                  CourseCode = p.Code
                                                              }).FirstOrDefault();
                                    string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                    txtName.Text = batchNameFormate;
                                }
                            }
                            else
                            {
                                var nielitCentreCourse = (from p in context1.Courses
                                                          where p.ID == courseid
                                                          select new
                                                          {
                                                              CourseName = p.Name,
                                                              CourseCode = p.Code
                                                          }).FirstOrDefault();
                                string batchNameFormate = ddlsubcentreId.ToString() + "/" + nielitCentreCourse.CourseCode + "/" + 1;
                                txtName.Text = batchNameFormate;
                            }
                        }
                        int BatchCodeExists = (from b in context.NielitCentreBatchs
                                               where b.CourseDurationID == courseid
                                               select b).Count();
                        if (BatchCodeExists > 0)
                        {
                            var nielitCentreBatch = (from p in context.NielitCentreBatchs
                                                     where p.CourseDurationID == courseid
                                                     orderby p.ID descending
                                                     select new
                                                     {
                                                         BatchName = p.Name,
                                                         BatchCode = p.BatchCode
                                                     }).First();
                            var nielitCentreCourse = (from p in context1.Courses
                                                      where p.ID == courseid
                                                      select new
                                                      {
                                                          CourseName = p.Name,
                                                          CourseCode = p.Code
                                                      }).FirstOrDefault();
                            string batchcode = nielitCentreBatch.BatchCode;
                            string BatchCodeStr = batchcode.Substring(batchcode.LastIndexOf("/") + 1);
                            Int32 batchCodeSrNum = Convert.ToInt32(BatchCodeStr) + 1;
                            string batchCodeFormate = nielitCentreCourse.CourseCode.ToString() + "/" + batchCodeSrNum.ToString();
                            txtBatchCode.Text = batchCodeFormate;
                        }
                        else
                        {
                            var nielitCentreCourse = (from p in context1.Courses
                                                      where p.ID == courseid
                                                      select new
                                                      {
                                                          CourseName = p.Name,
                                                          CourseCode = p.Code
                                                      }).FirstOrDefault();
                            string batchCodeFormate = nielitCentreCourse.CourseCode.ToString() + "/" + 1;
                            txtBatchCode.Text = batchCodeFormate;
                        }
                    }

                }
            }

            else
            {
                txtName.Text = "";
                txtBatchCode.Text = "";
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void txtstartDate_TextChanged(object sender, EventArgs e)
    {
        Int32 courseid = Convert.ToInt32(ddlCourse.SelectedValue);
        using (NIELITMISContext context = new NIELITMISContext())
        {
            int CourseIdExistinDurationList = (from b in context.NielitCourseDurations
                                               where b.ID == courseid
                                               select b).Count();
            if (CourseIdExistinDurationList > 0)
            {
                var courseRunningDays = (from p in context.NielitCourseDurations
                                         where p.ID == courseid
                                         select new
                                         {
                                             CourseRunningDays = p.courseDurationDays,
                                             CourseCode = p.courseID
                                         }).FirstOrDefault();
                Int32 coursDays = courseRunningDays.CourseRunningDays;
                DateTime EndDate = Convert.ToDateTime(txtstartDate.Text);
                EndDate = EndDate.AddDays(-1);
                txtendDate.Text = EndDate.AddDays(coursDays).ToString("dd-MMM-yyyy"); ;
            }
        }
    }

    protected void FillBatchNameAndBatchCode(Int32 courseCategoryID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == courseCategoryID
                                 orderby p.DisplayOrder
                                 select new { ValueField = p.ID, TextField = p.Name };
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
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Batch Course Centre";
            tblNavLinks.Visible = false;
            lblShowOnWeb.Visible = true;
            lblIsVerifieds.Visible = true;
            lblIsActive.Visible = true;
            RdoAffInstOrNonAffInst.Enabled = false;
            ddlVerifieds.Visible = true;
            ddlShowOnWeb.Visible = true;
            ddlActive.Visible = true;
            ddlShowOnWeb.Enabled = false;
            ddlActive.Enabled = false;
            EConnectContext context1 = new EConnectContext();
            User objUser = new EConnect.URM.User();
            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int64 Id = Convert.ToInt32(Request.QueryString["Key"]);
                if (context.NielitCentreBatchs.Any(s => (s.IsVerified == true) && s.ID == Id))
                {

                    //Added for learningMode
                    ddlLearningMode.Enabled = false;
                    txtRemarks.Enabled = false;
                    //
                    ddlCourse.Enabled = false;
                    ddlbatchSession.Enabled = false;
                    ddlActive.Enabled = false;
                    ddlCorporate.Enabled = false;
                    ddlVerifieds.Enabled = false;
                    ddlShowOnWeb.Enabled = false;
                    ddlSubcentreName.Enabled = false;
                    RdoAffInstOrNonAffInst.Enabled = false;
                    txtName.Enabled = false;
                    txtBatchCode.Enabled = false;
                    txtstartDate.Enabled = false;
                    txtendDate.Enabled = false;
                    txtbatchDurationPracticalHours.Enabled = false;
                    txtbatchDurationTheoryHours.Enabled = false;
                    txtOrgTrained.Enabled = false;
                    txtFName.Enabled = false;
                    txtFEmail.Enabled = false;
                    ddlShowOnWeb.Enabled = false;
                    ddlActive.Enabled = false;
                    RdoAffInstOrNonAffInst.Enabled = false;

                    if (UserTypeId == 10)
                    {
                        this.Rview.Visible = true;
                        this.Rview1.Visible = true;
                    }

                    var Batchcentre = (from p in context.NielitCentreBatchs
                                       where p.ID == Id
                                       select p).FirstOrDefault();
                    NIELITMISContext context2 = new NIELITMISContext();

                    if (UserTypeId == 11) //NonAffInstitutes
                    {
                        var intituteslinkedToCentre = context.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        }
                    }
                    if (UserTypeId == 4) //AffInstitutes
                    {

                        var intituteslinkedToCentre = context.AffInstitutes.Where(s => s.instituteID == loginUser.UserRefNumber).FirstOrDefault();
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        }

                        //
                        //var intituteslinkedToCentre = context.AffInstitutes.Find(loginUser.UserRefNumber);
                        //NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        //NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        //if (institutesName != null)
                        //{
                        //    txtInstitute.Text = institutesName.Name;
                        //    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        //}
                    }
                    if (NielitCentrelinkedToCentreId != 0)
                    {
                        NielitCentres intitutesName = context2.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        txtInstitute.Text = intitutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                    }
                    else
                    {
                        NielitCentres intitutesName = context2.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                        txtInstitute.Text = intitutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                    }
                    int AfflCentreExists = (from b in context.AffInstitutes
                                            where b.ID == Batchcentre.subCentreID
                                            select b).Count();
                    if (AfflCentreExists > 0)
                    {
                        ListItem lst1 = new ListItem("--Select One--", "0");
                        RdoAffInstOrNonAffInst.SelectedValue = "1";
                        var centreName = from s in context.AffInstitutes
                                             //where s.linkedToCentre == NelitCentreLinkId
                                         select new { ValueField = s.instituteID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName.Distinct(), lst1);
                    }
                    int NonAfflCentreExists = (from b in context.NonAffInstitutes
                                               where b.ID == Batchcentre.subCentreID
                                               select b).Count();
                    if (NonAfflCentreExists > 0)
                    {
                        ListItem lst1 = new ListItem("--Select One--", "0");
                        RdoAffInstOrNonAffInst.SelectedValue = "0";
                        var centreName = from s in context.NonAffInstitutes
                                             // where s.ID == NelitCentreLinkId
                                         select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName.Distinct(), lst1);
                    }
                    if (AfflCentreExists == 0 && NonAfflCentreExists == 0)
                    {
                        RdoAffInstOrNonAffInst.SelectedValue = "99";
                        ddlSubcentreName.Items.Add(new ListItem("--Select One--", "0"));
                        ddlSubcentreName.SelectedValue = "0";
                        ddlSubcentreName.Enabled = false;
                    }


                    //Added for Is Sem Based
                    ddlCourse.SelectedValue = Batchcentre.CourseDurationID.ToString();

                    Int32 CourseId = Convert.ToInt32(ddlCourse.SelectedValue);
                    var application = (from p in context.NielitCentreCourses
                                       join d in context.NielitCourseDurations on p.ID equals d.courseID
                                       where d.ID == CourseId && p.IsVerified == true && p.CourseCategoryID == 101
                                       select new
                                       {
                                           ID = p.ID,
                                           Name = p.Name,
                                           CourseCatId = p.CourseCategoryID

                                       }).FirstOrDefault();

                    if (application != null)
                    {
                        trsembased.Visible = true;
                        trsembased1.Visible = true;
                        if (Batchcentre.IsSemBased == true)
                        {
                            ddlIsSemBased.SelectedValue = "Y";
                            trsemyearBased.Visible = true;
                            trsemyearBased1.Visible = true;
                            truniversity.Visible = true;
                            truniversity1.Visible = true;
                            // txtUniversity.Text = Batchcentre.AffUniversity.ToString();
                            // modify  vishal on 08-03-2022
                            if (Batchcentre.AffUniversity != null)
                            {
                                txtUniversity.Text = Batchcentre.AffUniversity.ToString();
                                txtRegDuration.Text = Convert.ToString(Batchcentre.RegistrationDurationYears);
                                txtRegDuration.Enabled = false;
                                txtUniversity.Enabled = false;
                                if (Batchcentre.SemYearBased == 0)
                                {
                                    ddlSemYearBased.SelectedValue = "0";
                                }
                                else if (Batchcentre.SemYearBased == 1)
                                {
                                    ddlSemYearBased.SelectedValue = "1";
                                }
                                ddlSemYearBased.Enabled = false;
                            }
                            else
                            {
                                ddlIsSemBased.SelectedValue = "N";
                                trsemyearBased.Visible = false;
                                trsemyearBased1.Visible = false;
                            }
                            ddlIsSemBased.Enabled = false;

                        }
                    }

                        //End for Is Sem Based
                        //Added for learning Mode
                        ddlLearningMode.SelectedValue = Batchcentre.learningModeID.ToString();

                        //txtRemarks.Text = Batchcentre.Remarks.ToString();

                        // vishal 08-03-2022
                        if (Batchcentre.Remarks != null)
                        {
                            txtRemarks.Text = Batchcentre.Remarks.ToString();
                        }
                        // vishal 08-03-2022
                        ddlSubcentreName.SelectedValue = Batchcentre.subCentreID.ToString();

                        txtName.Text = Batchcentre.Name.ToString();
                        txtBatchCode.Text = Batchcentre.BatchCode.ToString();
                        txtstartDate.Text = Batchcentre.startDate.ToString("dd-MMM-yyyy");
                        txtendDate.Text = Batchcentre.endDate.ToString("dd-MMM-yyyy");
                        //txtendDate.Text = Batchcentre.ToString();
                        ddlbatchSession.SelectedValue = Batchcentre.batchSession.ToString();

                        if (Batchcentre.whetherCorporate == true)
                        {
                            ddlCorporate.SelectedValue = "Y";
                            txtOrgTrained.Text = Batchcentre.OrgTrained.ToString();
                        }
                        else
                        {
                            ddlCorporate.SelectedValue = "N";
                        }

                        if (Batchcentre.IsActive == true)
                        {
                            ddlActive.SelectedValue = "1";
                        }
                        else
                        {
                            ddlActive.SelectedValue = "0";
                        }

                        if (Batchcentre.IsVerified == true)
                        {
                            ddlVerifieds.SelectedValue = "1";
                        }
                        else
                        {
                            ddlVerifieds.SelectedValue = "0";
                        }

                        if (Batchcentre.Show_On_Web == true)
                            ddlShowOnWeb.SelectedValue = "1";
                        else
                        {
                            ddlShowOnWeb.SelectedValue = "0";
                        }
                        txtbatchDurationPracticalHours.Text = Batchcentre.batchDurationPracticalHours.ToString();
                        txtbatchDurationTheoryHours.Text = Batchcentre.batchDurationTheoryHours.ToString();
                        txtFName.Text = Batchcentre.facultyName.ToString();
                        txtFEmail.Text = Batchcentre.facultyEmail.ToString();
                        //txtOrgTrained.Text = Batchcentre.OrgTrained.ToString();
                        btnCancel.Visible = false;
                        btnSave.Visible = false;
                        btnBack.Visible = true;
                        //throw new Exception("This Course is Already Verifed");

                        // vishal 07-03-2022
                        int learningMode = 0;
                        learningMode = Convert.ToInt32(Batchcentre.learningModeID);
                        if (learningMode == 7)  // start date is editable only for Virtual Academy.
                        {
                            txtstartDate.Enabled = true;
                            btnSave.Visible = true;
                            btnSave.Text = "Update";
                        }
                        else
                        {
                            throw new Exception("This Batch is Already Verifed");
                        }
                        // vishal 07-03-2022
                    }
                
                else
                {
                    //Added for learning Mode
                    ddlLearningMode.Enabled = true;
                    txtRemarks.Enabled = true;
                    //

                    //Added for Is Sem Based
                    ddlIsSemBased.Enabled = false;
                    //end
                    ddlCourse.Enabled = true;
                    ddlbatchSession.Enabled = true;
                    ddlCorporate.Enabled = true;
                    ddlVerifieds.Enabled = true;
                    txtName.Enabled = true;
                    txtBatchCode.Enabled = false;
                    txtstartDate.Enabled = true;
                    txtendDate.Enabled = true;
                    txtbatchDurationPracticalHours.Enabled = true;
                    txtbatchDurationTheoryHours.Enabled = true;
                    txtOrgTrained.Enabled = true;
                    txtFName.Enabled = true;
                    txtFEmail.Enabled = true;
                    this.Rview.Visible = false;
                    this.Rview1.Visible = false;
                    var Batchcentre = (from p in context.NielitCentreBatchs
                                       where p.ID == Id
                                       select new
                                       {
                                           ID = p.ID,
                                           BatchName = p.Name,
                                           BatchCode = p.BatchCode,
                                           learningModeID = p.learningModeID,
                                           remarks = p.Remarks,
                                           CourseID = p.CourseDurationID,
                                           CentreId = p.centreID,
                                           SubCentreId = p.subCentreID,
                                           showOnWeb = p.Show_On_Web,
                                           IsActive = p.IsActive,
                                           Isverified = p.IsVerified,
                                           whetherCorporate = p.whetherCorporate,
                                           BatchDurationTheoryHours = p.batchDurationTheoryHours,
                                           BatchDurationPracticalHours = p.batchDurationPracticalHours,
                                           orgTrained = p.OrgTrained,
                                           startDate = p.startDate,
                                           endDate = p.endDate,
                                           FacultyName = p.facultyName,
                                           FacultyEmail = p.facultyEmail,
                                           enterBy = p.enterBy,
                                           BatchSession = p.batchSession,
                                           // addIsSemBased on
                                           IsSemBased = p.IsSemBased,
                                           SemYearBased = p.SemYearBased,
                                           UniversityName = p.AffUniversity,
                                           RegistrationDuration = p.RegistrationDurationYears
                                           //
                                       }).FirstOrDefault();
                    //Added Learning Mode
                    ddlLearningMode.SelectedValue = Batchcentre.learningModeID.ToString();
                    txtRemarks.Text = Batchcentre.remarks.ToString();
                    //Added for Is Sem Based
                    ddlCourse.SelectedValue = Batchcentre.CourseID.ToString();
                    Int32 CourseId = Convert.ToInt32(ddlCourse.SelectedValue);
                    var application = (from p in context.NielitCentreCourses
                                       join d in context.NielitCourseDurations on p.ID equals d.courseID
                                       where d.ID == CourseId && p.IsVerified == true && p.CourseCategoryID == 101
                                       select new
                                       {
                                           ID = p.ID,
                                           Name = p.Name,
                                           CourseCatId = p.CourseCategoryID

                                       }).FirstOrDefault();


                    if (application != null)
                    {
                        trsembased.Visible = true;
                        trsembased1.Visible = true;
                        if (Batchcentre.IsSemBased == true)
                        {
                            ddlIsSemBased.SelectedValue = "Y";
                            trsemyearBased.Visible = true;
                            trsemyearBased1.Visible = true;
                            truniversity.Visible = true;
                            truniversity1.Visible = true;
                            txtUniversity.Text = Batchcentre.UniversityName.ToString();
                            txtRegDuration.Text = Convert.ToString(Batchcentre.RegistrationDuration);
                            if (Batchcentre.SemYearBased == 0)
                            {
                                ddlSemYearBased.SelectedValue = "0";
                            }
                            else if (Batchcentre.SemYearBased == 1)
                            {
                                ddlSemYearBased.SelectedValue = "1";
                            }
                        }
                        else
                        {
                            ddlIsSemBased.SelectedValue = "N";
                            trsemyearBased.Visible = false;
                            trsemyearBased1.Visible = false;
                        }
                        ddlIsSemBased.Enabled = false;

                    }

                    //end for Is Sem Based
                    ddlbatchSession.SelectedValue = Batchcentre.BatchSession.ToString();
                    NIELITMISContext context2 = new NIELITMISContext();
                    if (UserTypeId == 11) //NonAffInstitutes
                    {
                        var intituteslinkedToCentre = context.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        }
                    }
                    if (UserTypeId == 4) //AffInstitutes
                    {
                        var intituteslinkedToCentre = context.AffInstitutes.Where(s => s.instituteID == loginUser.UserRefNumber).FirstOrDefault();
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        }
                        //var intituteslinkedToCentre = context.AffInstitutes.Find(loginUser.UserRefNumber);
                        //NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        //NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        //if (institutesName != null)
                        //{
                        //    txtInstitute.Text = institutesName.Name;
                        //    Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        //}
                    }
                    if (NielitCentrelinkedToCentreId != 0)
                    {
                        NielitCentres intitutesName = context2.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        txtInstitute.Text = intitutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                    }
                    else
                    {
                        NielitCentres intitutesName = context2.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                        txtInstitute.Text = intitutesName.Name;
                        Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                    }

                    int AfflCentreExists = (from b in context.AffInstitutes
                                            where b.instituteID == Batchcentre.SubCentreId
                                            select b).Count();
                    if (AfflCentreExists > 0)
                    {
                        ListItem lst1 = new ListItem("--Select One--", "0");
                        RdoAffInstOrNonAffInst.SelectedValue = "1";
                        var centreName = from s in context.AffInstitutes
                                             //where s.linkedToCentre == NelitCentreLinkId
                                         select new { ValueField = s.instituteID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName.Distinct(), lst1);
                    }
                    int NonAfflCentreExists = (from b in context.NonAffInstitutes
                                               where b.ID == Batchcentre.SubCentreId
                                               select b).Count();
                    if (NonAfflCentreExists > 0)
                    {
                        ListItem lst1 = new ListItem("--Select One--", "0");
                        RdoAffInstOrNonAffInst.SelectedValue = "0";
                        var centreName = from s in context.NonAffInstitutes
                                             // where s.linkedToCentre == NelitCentreLinkId
                                         select new { ValueField = s.ID, TextField = s.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlSubcentreName, centreName.Distinct(), lst1);
                    }
                    if (AfflCentreExists == 0 && NonAfflCentreExists == 0)
                    {
                        RdoAffInstOrNonAffInst.SelectedValue = "99";
                        ddlSubcentreName.Items.Add(new ListItem("--Select One--", "0"));
                        ddlSubcentreName.SelectedValue = "0";
                        ddlSubcentreName.Enabled = false;
                    }
                    ddlSubcentreName.SelectedValue = Batchcentre.SubCentreId.ToString();
                    txtName.Text = Batchcentre.BatchName.ToString();
                    txtBatchCode.Text = Batchcentre.BatchCode.ToString();
                    txtstartDate.Text = Batchcentre.startDate.ToString("dd-MMM-yyyy");
                    txtendDate.Text = Batchcentre.endDate.ToString("dd-MMM-yyyy");

                    if (Batchcentre.whetherCorporate == true)
                    {
                        ddlCorporate.SelectedValue = "Y";
                        txtOrgTrained.Text = Batchcentre.orgTrained.ToString();
                    }
                    else
                    {
                        ddlCorporate.SelectedValue = "N";
                    }
                    if (UserTypeId == 10)
                    {
                        this.Rview.Visible = true;
                        this.Rview1.Visible = true;
                    }
                    if (Batchcentre.IsActive == true)
                        ddlActive.SelectedValue = "1";
                    else
                    {
                        ddlActive.SelectedValue = "0";
                    }


                    if (UserTypeId == 10 && RdoAffInstOrNonAffInst.SelectedValue != "2")
                    {
                        ddlCourse.Enabled = false;
                        ddlbatchSession.Enabled = false;
                        ddlCorporate.Enabled = false;
                        ddlVerifieds.Enabled = true;
                        txtName.Enabled = false;
                        txtBatchCode.Enabled = false;
                        txtstartDate.Enabled = false;
                        txtendDate.Enabled = false;
                        txtbatchDurationPracticalHours.Enabled = false;
                        txtbatchDurationTheoryHours.Enabled = false;
                        txtOrgTrained.Enabled = false;
                        txtFName.Enabled = false;
                        txtFEmail.Enabled = false;
                        this.Rview.Visible = true;
                        this.Rview1.Visible = true;
                    }
                    if (Batchcentre.Isverified == true)
                    {
                        ddlVerifieds.SelectedValue = "1";
                        ddlVerifieds.Enabled = false;
                    }
                    else
                    {
                        ddlVerifieds.SelectedValue = "0";
                    }

                    if (Batchcentre.showOnWeb == true)
                        ddlShowOnWeb.SelectedValue = "1";
                    else
                    {
                        ddlShowOnWeb.SelectedValue = "0";
                    }
                    txtbatchDurationPracticalHours.Text = Batchcentre.BatchDurationPracticalHours.ToString();
                    txtbatchDurationTheoryHours.Text = Batchcentre.BatchDurationTheoryHours.ToString();
                    txtFName.Text = Batchcentre.FacultyName.ToString();
                    txtFEmail.Text = Batchcentre.FacultyEmail.ToString();
                    //txtOrgTrained.Text = Batchcentre.orgTrained.ToString();
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(Batchcentre.BatchName, "Admin/NielitCentreBatch.aspx?" + Request.QueryString.ToString(), ""));
                    //Get last modified date of current record and save it in ViewState object.
                    ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;                  
                }
                };
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
    public DataTable FillGridViewNIELITMISCourseNielitCourseRecord()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                Int64 ent = Convert.ToInt64(Session["EntityID"]);
                using (SqlCommand cmd = new SqlCommand("FillGridViewNIELITMISCourseNielitCourseRecord", con))
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
    protected void BindGridView()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                Int64 CourseName = 0;
                Int32 batchname = 0;
                Int32 VerifiedStatus = 0;
                Int32 NielitCentreIdRefAfflorNonAffl = 0, subcenteridLoginRef = 0;
                if (ddlCourseName.SelectedValue != "0")
                    CourseName = Convert.ToInt64(ddlCourseName.SelectedValue);
                if (ddlbatchname.SelectedValue != "0")
                    batchname = Convert.ToInt32(ddlbatchname.SelectedValue);
                //if (ddlBatchVerifiedStatus.SelectedValue != "0")
                VerifiedStatus = Convert.ToInt32(ddlBatchVerifiedStatus.SelectedValue);
                //if (ddlbatchcode.SelectedValue != "0") 
                //    Batchcodeid = Convert.ToInt32(ddlbatchcode.SelectedValue);
                //    


                User objUser;
                using (EConnectContext context1 = new EConnectContext())
                {
                    objUser = new EConnect.URM.User();

                    User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    if (UserTypeId == 11)
                    {
                        var intituteslinkedToCentre = context.NonAffInstitutes.Find(loginUser.UserRefNumber);
                        subcenteridLoginRef = Convert.ToInt32(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            NielitCentreIdFilter = Convert.ToInt32(institutesName.ID);
                        }
                    }
                    else if (UserTypeId == 4)
                    {
                        var intituteslinkedToCentre = from s in context.AffInstitutes
                                                      where s.instituteID == loginUser.UserRefNumber
                                                      select new { ID = s.ID, linkedToCentre = s.linkedToCentre };

                        // NonAfflAfflInstID = Convert.ToInt32(intituteslinkedToCentre.FirstOrDefault().ID);
                        subcenteridLoginRef = Convert.ToInt32(loginUser.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.ToList().FirstOrDefault().linkedToCentre);
                        NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            txtInstitute.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            NielitCentreIdFilter = Convert.ToInt32(institutesName.ID);
                        }
                    }
                    if (UserTypeId == 10)
                    {
                        var intituteslinkedToCentre = context.NielitCentres.Find(loginUser.UserRefNumber);
                        subcenteridLoginRef = Convert.ToInt32(loginUser.UserRefNumber);
                        Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                        NielitCentreIdRefAfflorNonAffl = NielitCentreId;
                        NielitCentreIdFilter = NielitCentreId;
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        if (NielitCentrelinkedToCentreId != 0)
                        {
                            NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            NielitCentreIdRefAfflorNonAffl = NelitCentreLinkId;
                            NielitCentreIdFilter = NielitCentreId;
                        }
                        else
                        {
                            NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            NIELITCentreId.Value = NelitCentreLinkId.ToString();
                            NielitCentreIdRefAfflorNonAffl = NelitCentreLinkId;
                            NielitCentreIdFilter = NielitCentreId;
                        }
                    }
                }
                //var CentreBatchs = from s in context.NielitCentreBatchs
                //                   join c in context.NielitCourseDurations on s.CourseDurationID equals c.ID 
                //                   join p in context.NielitCentreCourses on c.courseID equals p.ID                                 
                //                 select new
                //                 {
                //                     ID = s.ID,
                //                     Name = s.Name,
                //                     CourseName=p.Name,
                //                     BatchCode = s.BatchCode,
                //                     subcenterid=s.subCentreID,
                //                     startDate = s.startDate,
                //                     endDate = s.endDate,
                //                     CourseID = s.CourseDurationID,
                //                     CentreID=s.centreID,
                //                     enterBy=s.enterBy,
                //                     IsVerified = s.IsVerified ? "YES" : "NO",
                //                 };
                using (DataTable dt = FillGridViewNIELITMISCourseNielitCourseRecord())
                {
                    if (dt.Rows.Count > 0)
                    {
                        var CentreBatchs = (from p in dt.AsEnumerable()
                                            select new
                                            {
                                                ID = p.Field<Int64>("ID"),
                                                Name = p.Field<string>("Name"),
                                                CourseName = p.Field<string>("CourseName"),
                                                BatchCode = p.Field<string>("BatchCode"),
                                                subcenterid = p.Field<Int64>("subcenterid"),
                                                startDate = p.Field<DateTime>("startDate"),
                                                endDate = p.Field<DateTime>("endDate"),
                                                CourseID = p.Field<Int64>("CourseID"),
                                                CentreID = p.Field<Int64>("CentreID"),
                                                enterBy = p.Field<int>("enterBy"),
                                                IsVerified = p.Field<string>("IsVerified"),
                                                IsVerifiedid = p.Field<int>("IsVerifiedid"),
                                                learningModeName = p.Field<string>("learningModeName")
                                            });
                        if (NielitCentreIdRefAfflorNonAffl != 0)
                        {
                            CentreBatchs = CentreBatchs.Where(s => s.CentreID == NielitCentreIdRefAfflorNonAffl);
                        }
                        if (UserTypeId != 10)
                        {
                            CentreBatchs = CentreBatchs.Where(s => s.enterBy == loginUserNo || s.subcenterid == subcenteridLoginRef);
                        }
                        if (!String.IsNullOrEmpty(searchString))
                        {
                            CentreBatchs = CentreBatchs.Where(s => s.Name.ToUpper().Contains(searchString) || s.BatchCode.ToUpper().Contains(searchString));
                        }
                        if (CourseName != 0 && batchname != 0)
                        {
                            CentreBatchs = CentreBatchs.Where(s => s.CourseID == CourseName && s.ID == batchname);
                        }
                        else if (CourseName != 0)
                        {
                            CentreBatchs = CentreBatchs.Where(s => s.CourseID == CourseName);
                        }
                        else if (batchname != 0)
                        {
                            CentreBatchs = CentreBatchs.Where(s => s.ID == batchname);
                        }
                        if (VerifiedStatus != 99)
                        {
                            CentreBatchs = CentreBatchs.Where(s => s.IsVerifiedid == VerifiedStatus);
                        }
                        //if (Batchcodeid != 0)
                        //{
                        //    CentreBatchs = CentreBatchs.Where(s => s.ID == Batchcodeid);
                        //}              

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
                                case "BatchCode":
                                    if (sortOrder == "DESC")
                                        CentreBatchs = CentreBatchs.OrderByDescending(s => s.BatchCode);
                                    else
                                        CentreBatchs = CentreBatchs.OrderBy(s => s.BatchCode);
                                    break;
                                case "startDate":
                                    if (sortOrder == "DESC")
                                        CentreBatchs = CentreBatchs.OrderByDescending(s => s.startDate);
                                    else
                                        CentreBatchs = CentreBatchs.OrderBy(s => s.startDate);
                                    break;
                                case "endDate":
                                    if (sortOrder == "DESC")
                                        CentreBatchs = CentreBatchs.OrderByDescending(s => s.endDate);
                                    else
                                        CentreBatchs = CentreBatchs.OrderBy(s => s.endDate);
                                    break;
                                default:
                                    CentreBatchs = CentreBatchs.OrderBy(s => s.Name);
                                    break;
                            }
                        }

                        PagingBar1.Bind(CentreBatchs, ref gvMain);
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                        // gvMain.Columns[5].Visible = false;
                        if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                        {
                            gvMain.Columns[7].Visible = false;
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
        //ShowAlert("Inside Code");
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            if (!UserManager.HasRight(currentRoleId, enmRight.New))
            {
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to add new record.", true);
                return;
            }
            BindEditNewModeData();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            this.Rview.Visible = false;
            this.Rview1.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Batch Course Centre";
            //Updating Breadcrumb         
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Batch Course Centre", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitCentreBatch.aspx?ID=" + Request.QueryString["CourseID"].ToString()), true);
            }
            else
            {
                Response.Redirect("NielitCentreBatch.aspx", true);
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
            lblerrorddlcouse.Text = "";
            if (IsValidForm())
            {
                BreadCrumb1.Render();
                EConnectContext context1 = new EConnectContext();
                User objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                Int64 courseId=Convert.ToInt64 (ddlCourse .SelectedValue );

                 //Added for virtual academy
                if(ddlLearningMode.SelectedValue.ToString () =="7")
                {
                NIELITMISContext context2=new NIELITMISContext();
                int vaCount = (from p in context2.VirtualAcademyCentreCourse
                               where p.centreID == NielitCentreId && p.courseID==courseId 
                               && p.isActive==true
                               select p).Count();
                if (vaCount == 0)
                {
                    ShowAlert("Course not available for virtual academy for this centre");
                    return;
                }
                 }
                ///
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    NielitCentreBatch objBatchCentre;
                    string sBatchCode = txtBatchCode.Text;

                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        objBatchCentre = new NielitCentreBatch();
                        HNANFL.Value = "O";
                        if (RdoAffInstOrNonAffInst.SelectedItem.Text == "Accredited Centre")
                        {
                            HNANFL.Value = "Y";
                        }
                        if (RdoAffInstOrNonAffInst.SelectedItem.Text == "Non Accredited Centre")
                        {
                            HNANFL.Value = "N";
                        }

                        if (context.NielitCentreBatchs.Where(s => s.BatchCode == sBatchCode).Count() != 0)
                        {
                            fillCourseWithDuration();
                        }
                        objBatchCentre.BatchCode = txtBatchCode.Text;

                        objBatchCentre.subCentreID = Convert.ToInt32(ddlSubcentreName.SelectedValue);
                        if (UserTypeId == 11) // NonAffInstitutes
                        {
                            var intituteslinkedToCentre = context.NonAffInstitutes.Find(loginUser.UserRefNumber);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            }
                            HNANFL.Value = "N";
                        }
                        if (UserTypeId == 4) //AffInstitutes
                        {
                            //var intituteslinkedToCentre = context.AffInstitutes.Find(loginUser.UserRefNumber);
                            var intituteslinkedToCentre = context.AffInstitutes.Where(s => s.instituteID == loginUser.UserRefNumber).FirstOrDefault();
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            }
                            HNANFL.Value = "Y";
                        }
                        if (NielitCentrelinkedToCentreId != 0)
                        {
                            NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            objBatchCentre.centreID = NelitCentreLinkId;
                        }
                        else
                        {
                            NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            objBatchCentre.centreID = NelitCentreLinkId;
                        }
                        //learningmode
                        objBatchCentre.learningModeID = Convert.ToInt32(ddlLearningMode.SelectedValue);
                        objBatchCentre.Remarks = txtRemarks.Text;
                        //


                      //  AddedControl for isSemBased
                        Int32 CourseId = Convert.ToInt32(ddlCourse.SelectedValue);
                         var application = (from p in context.NielitCentreCourses
                               join d in context.NielitCourseDurations on p.ID equals d.courseID
                               where d.ID == CourseId && p.IsVerified == true && p.CourseCategoryID == 101
                               select new
                               {
                                   ID = p.ID,
                                   Name = p.Name,
                                   CourseCatId = p.CourseCategoryID

                               }).FirstOrDefault();

                         if (application != null)
                         {
                             if (ddlIsSemBased.SelectedValue.Trim()=="0")
                             {

                                 strMessage = "Please select IsSemBased";
                                 lblwhetherSembased.Text = "Please select IsSemBased";
                                 ddlIsSemBased.Focus();
                                 return;
                             }
                             if (txtUniversity.Text == "")
                             {

                                 strMessage = "Please enter University Name";
                                 lbluniversity.Text = "Please enter University Name";
                                 txtUniversity.Focus();
                                 return;
                             }
                             if (txtRegDuration.Text == "")
                             {

                                 strMessage = "Please enter Registration Duration";
                                 lblRegDuration.Text = "Please enter Registration Duration";
                                 txtRegDuration.Focus();
                                 return;
                             }
                  
                             //IsSemBased On
                             if (ddlIsSemBased.SelectedValue == "Y")
                             {
                                 if (ddlSemYearBased.SelectedValue.Trim() == "-1")
                                 {

                                     strMessage = "Please select Year/Sem Based";
                                     lblyearSem.Text = "Please select Year/Sem Based";
                                     ddlSemYearBased.Focus();
                                     return;
                                 }
                  

                                 objBatchCentre.IsSemBased = true;
                                 objBatchCentre.SemYearBased = Convert.ToInt32(ddlSemYearBased.SelectedValue.Trim());
                             }
                             else
                             {
                                 objBatchCentre.IsSemBased = false;
                             }
                             objBatchCentre.AffUniversity = txtUniversity.Text;
                             objBatchCentre.RegistrationDurationYears = Convert.ToInt32(txtRegDuration.Text);
                         }
                        // End for Is SemBased

                        objBatchCentre.CourseDurationID = Convert.ToInt32(ddlCourse.SelectedValue);
                        objBatchCentre.Name = txtName.Text;
                        objBatchCentre.startDate = Convert.ToDateTime(txtstartDate.Text);
                        objBatchCentre.endDate = Convert.ToDateTime(txtendDate.Text);
                        objBatchCentre.batchSession = Convert.ToInt32(ddlbatchSession.SelectedValue);
                        objBatchCentre.batchDurationTheoryHours = Convert.ToDecimal(txtbatchDurationTheoryHours.Text);
                        objBatchCentre.batchDurationPracticalHours = Convert.ToDecimal(txtbatchDurationPracticalHours.Text);
                        if (ddlCorporate.SelectedValue == "Y")
                        {
                            objBatchCentre.whetherCorporate = true;
                            objBatchCentre.OrgTrained = txtOrgTrained.Text;
                        }
                        else
                        {
                            objBatchCentre.whetherCorporate = false;
                        }
                        objBatchCentre.whetherAffiliated = HNANFL.Value.ToString();
                        objBatchCentre.facultyName = txtFName.Text;
                        objBatchCentre.facultyEmail = txtFEmail.Text;
                        objBatchCentre.enterDate = DateTime.Now;
                        objBatchCentre.enterBy = Convert.ToInt32(Session["UserID"]);
                        objBatchCentre.IsActive = true;
                        objBatchCentre.IsVerified = false;
                        objBatchCentre.Show_On_Web = true;
                        context.NielitCentreBatchs.Add(objBatchCentre);
                        context.SaveChanges();
                        strMessage = "New record saved.";
                        // }
                        //else
                        //{
                        //    txtBatchCode.Text = "";
                        //    txtBatchCode.Focus();
                        //    throw new Exception("This Batch Code is Already Exists");
                        //}                        
                    }
                    else
                    {
                        objBatchCentre = context.NielitCentreBatchs.Find(Convert.ToInt32(Request.QueryString["key"]));

                        if (UserTypeId == 11) // NonAffInstitutes
                        {
                            var intituteslinkedToCentre = context.NonAffInstitutes.Find(loginUser.UserRefNumber);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            }
                        }
                        if (UserTypeId == 4) //AffInstitutes
                        {
                            var intituteslinkedToCentre = context.AffInstitutes.Find(loginUser.UserRefNumber);
                            NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                            NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            if (institutesName != null)
                            {
                                txtInstitute.Text = institutesName.Name;
                                Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                            }
                        }
                        if (NielitCentrelinkedToCentreId != 0)
                        {
                            NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            objBatchCentre.centreID = NelitCentreLinkId;
                        }
                        else
                        {
                            NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                            txtInstitute.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                            objBatchCentre.centreID = NelitCentreLinkId;
                        }
                        //Added learning mode
                        objBatchCentre.learningModeID = Convert.ToInt32(ddlLearningMode.SelectedValue);
                        objBatchCentre.Remarks = txtRemarks.Text;
                        //
                        objBatchCentre.CourseDurationID = Convert.ToInt32(ddlCourse.SelectedValue);
                        objBatchCentre.Name = txtName.Text;
                        objBatchCentre.BatchCode = txtBatchCode.Text;
                        objBatchCentre.startDate = Convert.ToDateTime(txtstartDate.Text);
                        objBatchCentre.endDate = Convert.ToDateTime(txtendDate.Text);
                        objBatchCentre.batchSession = Convert.ToInt32(ddlbatchSession.SelectedValue);
                        objBatchCentre.batchDurationTheoryHours = Convert.ToDecimal(txtbatchDurationTheoryHours.Text);
                        objBatchCentre.batchDurationPracticalHours = Convert.ToDecimal(txtbatchDurationPracticalHours.Text);
                        if (ddlCorporate.SelectedValue == "Y")
                        {
                            objBatchCentre.whetherCorporate = true;
                            objBatchCentre.OrgTrained = txtOrgTrained.Text;
                        }
                        else
                        {
                            objBatchCentre.whetherCorporate = false;
                        }

                        Int32 CourseId = Convert.ToInt32(ddlCourse.SelectedValue);
                         var application = (from p in context.NielitCentreCourses
                               join d in context.NielitCourseDurations on p.ID equals d.courseID
                               where d.ID == CourseId && p.IsVerified == true && p.CourseCategoryID == 101
                               select new
                               {
                                   ID = p.ID,
                                   Name = p.Name,
                                   CourseCatId = p.CourseCategoryID

                               }).FirstOrDefault();

                         if (application != null)
                         {
                             if (ddlIsSemBased.SelectedValue == "Y")
                             {                                 
                                 objBatchCentre.IsSemBased = true;
                                 objBatchCentre.SemYearBased = Convert.ToInt32(ddlSemYearBased.SelectedValue.Trim());
                             }
                             else
                             {
                                 objBatchCentre.IsSemBased = false;
                             }
                             objBatchCentre.AffUniversity = txtUniversity.Text;
                             objBatchCentre.RegistrationDurationYears = Convert.ToInt32(txtRegDuration.Text);
                         }

                        objBatchCentre.facultyName = txtFName.Text;
                        objBatchCentre.facultyEmail = txtFEmail.Text;
                        objBatchCentre.enterDate = DateTime.Now;
                        if (ddlActive.SelectedValue == "1")
                            objBatchCentre.IsActive = true;
                        else
                        {
                            objBatchCentre.IsActive = false;
                        }

                        if (ddlVerifieds.SelectedValue == "1")
                            objBatchCentre.IsVerified = true;
                        else
                        {
                            objBatchCentre.IsVerified = false;
                        }

                        if (ddlShowOnWeb.SelectedValue == "1")
                            objBatchCentre.Show_On_Web = true;
                        else
                        {
                            objBatchCentre.Show_On_Web = false;
                        }
                        strMessage = "Record updated.";
                        context.SaveChanges();
                    }
                }
                Response.Redirect("NielitCentreBatch.aspx?msg=" + strMessage, true);
            }
            else if (ddlCorporate.SelectedValue == "Y")
            {
                ShowAlert("Please enter the Organisation Trained.");
                txtOrgTrained.Focus();
            }
            else
            {
                ShowAlert("Please select the Sub Centre Name.");
                ddlSubcentreName.Focus();
            }
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCorporate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            string sValue = ddlCorporate.SelectedValue;
            if (sValue != "Y")
            {
                txtOrgTrained.Enabled = false;
            }
            else
                txtOrgTrained.Enabled = true;

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
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
            ddlCourseName.SelectedValue = "0";
            ddlbatchname.SelectedValue = "0";
            ddlBatchVerifiedStatus.SelectedValue = "99";
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
            //var BatchName =  from s in context.NielitCentreBatchs
            //                         join c in context.NielitCourseDurations on s.CourseDurationID equals c.ID 
            //                         join p in context.NielitCentreCourses on c.courseID equals p.ID
            //                 where s.centreID == NielitCentreIdSearch && c.isVerified==true && c.isVerified==true
            //                 select new { Name = s.Name };

            var BatchName = from s in context.NielitCentreBatchs
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("NielitCentreBatch.aspx", true);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("NielitCentreBatch.aspx", true);
    }

    protected void ddlbatchname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ListItem lst = new ListItem("--All--", "0");
            //using (NIELITMISContext context = new NIELITMISContext())
            //{
            //    Int64 batchnameid = Convert.ToInt64(ddlbatchname.SelectedValue);
            //    //Int32 coursecategoryid = Convert.ToInt32(ddlCourseCategory.SelectedValue);
            //    if (batchnameid != 0)
            //    {
            //        var BatchName = from p in context.NielitCentreCourses
            //                        join k in context.NielitCourseDurations on p.ID equals k.courseID
            //                        join d in context.NielitCentreBatchs on k.ID equals d.CourseDurationID
            //                        where p.IsVerified == true && k.isVerified == true && k.ID == batchnameid
            //                        orderby (d.Name)
            //                        select new { ValueField = d.ID, TextField = d.BatchCode };

            //        EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchcode, BatchName.Distinct(), lst);
            // }
            // }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            NielitCentreIdFilter = Convert.ToInt32(NIELITCentreId.Value.ToString());
            NonAfflAfflInstID = Convert.ToInt32(HNonAfflAfflInst.Value.ToString());
            ListItem lst = new ListItem("--All--", "0");
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int64 coursenameid = Convert.ToInt64(ddlCourseName.SelectedValue);
                if (coursenameid != 0)
                {
                    //if (NonAfflAfflInstID == 99)
                    //{
                    //    var BatchName = from p in context.NielitCentreCourses
                    //                    join k in context.NielitCourseDurations on p.ID equals k.courseID
                    //                    join d in context.NielitCentreBatchs on k.ID equals d.CourseDurationID
                    //                    where p.IsVerified == true && k.isVerified == true && k.ID == coursenameid && d.centreID == NielitCentreIdFilter
                    //                    orderby (d.Name)
                    //                    select new { ValueField = d.ID, TextField = d.Name };
                    //    ddlbatchname.Items.Clear();
                    //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName.Distinct(), lst);
                    //}
                    //else
                    //{
                    //    var BatchName = from p in context.NielitCentreCourses
                    //                    join k in context.NielitCourseDurations on p.ID equals k.courseID
                    //                    join d in context.NielitCentreBatchs on k.ID equals d.CourseDurationID
                    //                    where p.IsVerified == true && k.isVerified == true && k.ID == coursenameid 
                    //                    && d.centreID == NielitCentreIdFilter && d.subCentreID==NonAfflAfflInstID
                    //                    orderby (d.Name)
                    //                    select new { ValueField = d.ID, TextField = d.Name };
                    //    ddlbatchname.Items.Clear();
                    //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlbatchname, BatchName.Distinct(), lst);
                    //}
                    using (DataTable dt = FillBatchNIELITMISCourseNielitCourse())
                    {
                        if (dt.Rows.Count > 0)
                        {
                            ddlbatchname.DataSource = dt;
                            ddlbatchname.DataTextField = "Name";
                            ddlbatchname.DataValueField = "ID";
                            ddlbatchname.DataBind();
                            ddlbatchname.Items.Insert(0, new ListItem("--Select One--", "0"));
                        }
                        else
                        {
                            ddlbatchname.Items.Clear();
                            ddlbatchname.Items.Insert(0, new ListItem("--Select One--", "0"));
                        }
                    }
                }
                else
                {
                    using (DataTable dt = FillBatchNIELITMISCourseNielitCourse())
                    {
                        if (dt.Rows.Count > 0)
                        {
                            ddlbatchname.DataSource = dt;
                            ddlbatchname.DataTextField = "Name";
                            ddlbatchname.DataValueField = "ID";
                            ddlbatchname.DataBind();
                            ddlbatchname.Items.Insert(0, new ListItem("--Select One--", "0"));
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void lbdisable_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    BreadCrumb1.Render();
                    ShowAlert("Sorry! You don't have rights to edit the records.", true);
                    return;
                }
                ExamCenter examcenter = context.ExamCenters.Find(Convert.ToInt32(hfActionID.Value.ToString()));
                if (examcenter.IsEnabled == true)
                    examcenter.IsEnabled = false;
                else
                    examcenter.IsEnabled = true;
                context.Entry(examcenter).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                BindGridView();
                ShowAlert("You have successfully changed the status of the Exam Center.", true);
                hfActionID.Value = "";
            };
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be edited!", true);
        }
    }
}