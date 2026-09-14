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
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Data.Objects;
using EConnect;

public partial class Admin_SemesterFeePaid : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int64 entityID = 0;
    Int64 NielitCentrelinkedToCentreId = 0;
    Int32 NielitCentreIdFilter = 0, NonAfflAfflInstID = 0;
    Int32 UserRefNumber = 0;
    Int32 UserTypeid = 0;

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
            User objUser1;
            using (EConnectContext context = new EConnectContext())
            {
                objUser1 = new EConnect.URM.User();

                User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);


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


                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        BindEditNewModeData();
                    }
                    else
                    {
                        BindEditNewModeData();
                        BindListData();
                        BindCourse();
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";
                        //    BindGridView();
                        if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Semester Fee Paid", "Admin/SemesterFeePaid.aspx?Id=" + Request.QueryString["Id"].ToString(), ""));
                        }
                        else
                        {
                            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Semester Fee Paid", "Admin/SemesterFeePaid.aspx", ""));
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

    protected void BindListData()
    {
        try
        {

            using (NIELITMISContext context = new NIELITMISContext())
            {
                //  ListItem lst = new ListItem("--All--", "0");

                using (DataTable dt = GetCoursesForSemesterFeePaid())       //bind course dropdown
                {
                    if (dt.Rows.Count > 0)
                    {


                        ddlCourseName.DataSource = dt;
                        ddlCourseName.DataTextField = "Name";
                        ddlCourseName.DataValueField = "ID";
                        ddlCourseName.DataBind();
                        ddlCourseName.Items.Insert(0, new ListItem("--Select--", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //protected void RdoAffInstOrNonAffInst_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        divGrid.Visible = false;
    //        ddlCourse.ClearSelection();
    //        ddlbatchSession.ClearSelection();
    //        ddlCourse.Items.Clear();
    //        ddlbatchSession.Items.Clear();
    //        BindCourse();
    //        ddlsemester.ClearSelection();
    //        ddlsemester.Items.Clear();
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message.ToString() + ex.Source.ToString());
    //    }
    //}

    protected void BindCourse()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                using (DataTable dt = GetCoursesForSemesterFee())       //bind course dropdown
                {
                    if (dt.Rows.Count > 0)
                    {
                        ddlCourse.DataSource = dt;
                        ddlCourse.DataTextField = "Name";
                        ddlCourse.DataValueField = "ID";
                        ddlCourse.DataBind();
                        ddlCourse.Items.Insert(0, new ListItem("--Select--", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable GetCoursesForSemesterFee()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (EConnectContext context = new EConnectContext())
        {
            User objUser1;
            objUser1 = new EConnect.URM.User();

            User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            UserRefNumber = Convert.ToInt32(loginUser.UserRefNumber);
            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
            NielitCentreIdFilter = NielitCentreId;


            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("GetCoursesForSemester", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@centerid", SqlDbType.Int));
                        cmd.Parameters["@centerid"].Value = NielitCentreIdFilter;
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
        return myDt;
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

    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int64 nielitcentreid = Convert.ToInt64(Session["EntityID"]);

        Int64 Courseiddata = 0;
        ddlbatchSession.ClearSelection();
        ddlsemester.ClearSelection();
        ddlStudent.ClearSelection();
        ddlfeetypemasid.ClearSelection();
        txtAmountPaid.Text = "";
        txtPaymentDate.Text = "";
        txtRemarks.Text = "";
        gvMain.Visible = false;

        try
        {

            Courseiddata = Convert.ToInt64(ddlCourse.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select--", "0");
                var Batch = from s in context.NielitCentreBatchs
                            where s.IsVerified == true && s.centreID == nielitcentreid
                                    && s.CourseDurationID == Courseiddata // && (s.startDate <= System.DateTime.Now && s.endDate >= System.DateTime.Now)
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



    public DataTable GetCoursesForSemesterFeePaid()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetCoursesForSemesterfilter", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@centerid", SqlDbType.Int));
                    cmd.Parameters["@centerid"].Value = NielitCentreIdFilter;
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

    public DataTable GetBatchesForSemesterMaster()
    {
        //  NielitCentreIdFilter = Convert.ToInt32(NIELITCentreId.Value.ToString());
        // NonAfflAfflInstID = Convert.ToInt32(HNonAfflAfflInst.Value.ToString());

        Int64 coursenameid = Convert.ToInt64(ddlCourse.SelectedValue);
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
                    cmd.Parameters["@centreId"].Value = 150;// coursenameid;  // Hardcoded 507..as session is not maintained yet
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

    protected void ddlbatchSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlsemester.ClearSelection();
        ddlsemester.Items.Clear();
        ddlStudent.ClearSelection();
        ddlStudent.ClearSelection();
        ddlfeetypemasid.ClearSelection();
        txtAmountPaid.Text = "";
        txtPaymentDate.Text = "";
        txtRemarks.Text = "";
        ListItem lst = new ListItem("--Select--", "0");

        using (NIELITMISContext context = new NIELITMISContext())
        {

            try
            {
                Int32 Batchid = Convert.ToInt32(ddlbatchSession.SelectedValue);
                Int32 Courseid = Convert.ToInt32(ddlCourse.SelectedValue);

               // FillFeeTypeMasId(ddlfeetypemasid, Convert.ToInt32(ddlbatchSession.SelectedValue));
               // FillStudentId();

               // ddlbatchname.SelectedValue = "0";

                  Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillFeeTypeMasId(ddlfeetypemasid, Convert.ToInt32(ddlbatchSession.SelectedValue));
        FillStudentId();
       // ddlfeetypemasid.ClearSelection();
        txtAmountPaid.Text = "";
        txtPaymentDate.Text = "";
        txtRemarks.Text = "";


    }
    protected void ddlStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
      //  ddlsemester.ClearSelection();
       // ddlfeetypemasid.ClearSelection();
        txtAmountPaid.Text = "";
        txtStudent.Text = "";
        txtPaymentDate.Text = "";
        txtRemarks.Text = "";
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int64 coursenameid = Convert.ToInt64(ddlCourse.SelectedValue);
                Int64 batchid = Convert.ToInt64(ddlbatchSession.SelectedValue);
                Int64 StudentId = Convert.ToInt64(ddlStudent.SelectedValue);
                Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);

                //ListItem lst = new ListItem("--Select--", "0");
                //if (coursenameid != 0)
                //{
                //    var Batch = from s in context.NielitCentreStudent
                //                where s.InstituteID == nielitcentreid && s.CourseID == coursenameid && s.batch_ID == batchid
                //                        && s.ID == StudentId
                //                orderby (s.Name)
                //                select new { ValueField = s.ID, TextField = s.semesterid };
                //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlsemester, Batch, lst);
                //}

            }

            FillStudentName();
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillStudentName()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int64 coursenameid = Convert.ToInt64(ddlCourse.SelectedValue);
                Int64 batchid = Convert.ToInt64(ddlbatchSession.SelectedValue);
                Int64 StudentId = Convert.ToInt64(ddlStudent.SelectedValue);
                Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);
                var applicationcheck = (from s in context.NielitCentreStudent
                                        where s.InstituteID == nielitcentreid && s.CourseID == coursenameid && s.batch_ID == batchid
                                                && s.ID == StudentId
                                        select new
                                        {
                                            ID = s.ID,
                                            Name = s.Name

                                        }).FirstOrDefault();

                if (applicationcheck != null)
                {

                    txtStudent.Text = applicationcheck.Name.ToString();
                }
            }
        }

        catch (Exception ex)
        {
            throw ex;
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
                            //RdoAffInstOrNonAffInst.SelectedValue = "2";
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
                            //  RdoAffInstOrNonAffInst.SelectedValue = "2";
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
    protected void FillFeeTypeMasId(DropDownList ddl, Int32 batchid)
    {
        try
        {
            Int32 Batchid = Convert.ToInt32(ddlbatchSession.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ddl.Items.Clear();
                ListItem lst = new ListItem("--Select--", "0");
                var FeeTypeMas = from p in context.NielitCentreBatchs
                                 join s in context.SemesterFeeMaster on p.ID equals s.batchID
                                 join f in context.feeTypeMas on s.feeTypeID equals f.ID
                                 where p.IsVerified == true && p.ID == Batchid
                                 orderby (p.Name)
                                 select new { ValueField = s.feeTypeID, TextField = f.feeType };
                FeeTypeMas = FeeTypeMas.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddl, FeeTypeMas.Distinct(), lst);
                btnSave.Visible = true;
                btnCancel.Visible = true;
                lblMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //protected void ToggleViewMode_Changed(object sender, EventArgs e)
    //{
    //    if (btnMode.ViewMode == ToggleView.Mode.New)
    //    {
    //        //if (!UserManager.HasRight(currentRoleId, enmRight.New))   fresh
    //        //{
    //        //    BreadCrumb1.Render();
    //        //    ShowAlert("Sorry! You don't have rights to add new record.", true);
    //        //    return;
    //        //}
    //        BindEditNewModeData();
    //        btnMode.ViewMode = ToggleView.Mode.List;
    //        mltvTab.ActiveViewIndex = 0;
    //        pnlFilter.Visible = false;
    //        ucSearchBar.Visible = false;
    //        //this.Rview.Visible = false;
    //        //this.Rview1.Visible = false;
    //        //Change the heading text as required
    //        lblHeading.Text = "Semester Fee Paid";
    //        //Updating Breadcrumb         
    //        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Semester Fee Paid ", "", ""));
    //    }
    //    else
    //    {
    //        if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
    //        {
    //            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("SemesterFeePaid.aspx?ID=" + Request.QueryString["Id"].ToString()), true);
    //        }
    //        else
    //        {
    //            Response.Redirect("SemesterFeePaid.aspx", true);
    //        }
    //    }
    //}

    protected void FillStudentId()
    {
        try
        {
            Int32 courseid = Convert.ToInt32(ddlCourse.SelectedValue);
            Int32 Batchid = Convert.ToInt32(ddlbatchSession.SelectedValue);
            Int32 semesterId = Convert.ToInt32(ddlsemester.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ddlStudent.Items.Clear();
                ListItem lst = new ListItem("--Select--", "0");
                var StudentNumber = from p in context.NielitCentreStudent

                                    where p.CourseID == courseid && p.batch_ID == Batchid
                                    && p.semesterid==semesterId
                                    orderby (p.Name)
                                    select new { ValueField = p.ID, TextField = p.Number };
                StudentNumber = StudentNumber.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlStudent, StudentNumber.Distinct(), lst);
                btnSave.Visible = true;
                btnCancel.Visible = true;
                lblMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillStudentIdFilter()
    {
        try
        {
            Int32 courseid = Convert.ToInt32(ddlCourseName.SelectedValue);
            Int32 Batchid = Convert.ToInt32(ddlbatchname.SelectedValue);
            Int32 semId = Convert.ToInt32(ddlsem.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ddlstudentF.Items.Clear();
                ListItem lst = new ListItem("--Select--", "0");
                var StudentFilter = from p in context.NielitCentreStudent
                                    join s in context.SemesterFeePaid
                                    on p.ID equals s.studentID
                                    where p.CourseID == courseid && p.batch_ID == Batchid
                                    && p.semesterid ==semId
                                    orderby (p.Name)
                                    select new { ValueField = p.ID, TextField = p.Number };
                StudentFilter = StudentFilter.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlstudentF, StudentFilter.Distinct(), lst);
                lblMessage.Text = "";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillAmountPaid()
    {
        try
        {
            Int32 courseid = Convert.ToInt32(ddlCourse.SelectedValue);
            Int32 Batchid = Convert.ToInt32(ddlbatchSession.SelectedValue);
            Int32 SemId = Convert.ToInt32(ddlsemester.SelectedItem.Text);
            Int32 FeetypeId = Convert.ToInt32(ddlfeetypemasid.SelectedValue.Trim());

            using (NIELITMISContext context = new NIELITMISContext())
            {
                txtAmountPaid.Text = "";
                var AmountPaid = (from p in context.SemesterFeeMaster
                                  where p.courseid == courseid && p.batchID == Batchid && p.SemId == SemId && p.feeTypeID == FeetypeId
                                  select new
                                  {
                                      ID = p.ID,
                                      Amount = p.feeAmount,
                                      FeetypeId = p.feeTypeID,
                                      //
                                  }).FirstOrDefault();
                if (AmountPaid != null)
                {
                    txtAmountPaid.Text = AmountPaid.Amount.ToString();
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    lblMessage.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void BindGridView()
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
                Int64 BatchIDFilter = 0, CourseIDFilter = 0, semfilter = 0, studentid = 0;
                if (ddlCourseName.SelectedValue != "0")
                    CourseIDFilter = Convert.ToInt64(ddlCourseName.SelectedValue);
                if (ddlbatchname.SelectedValue != "0")
                    BatchIDFilter = Convert.ToInt64(ddlbatchname.SelectedValue);
                if (ddlstudentF.SelectedValue != "0")
                    studentid = Convert.ToInt64(ddlstudentF.SelectedValue);
                DataTable dt = new DataTable();
                con.Open();
                if (searchString != "")
                {
                    using (dt = GetSemesterFeeFilterForGrid())
                    {

                        PagingBar1.Bind(dt, ref gvMain);
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                        lblError.Visible = false;
                        PagingBar1.Visible = true;
                        gvMain.Visible = true;
                        divGrid.Visible = true;
                        trpage.Visible = true;
                        divsave.Visible = false;
                        if (gvMain.Rows.Count <= 0)
                        {
                            lblError.Text = "No record found.";
                            lblError.Visible = true;
                        }
                        lblError.Visible = false;
                        gvMain.Visible = true;
                        PagingBar1.Visible = true;
                    }
                }
                else
                {
                    if (ddlCourseName.SelectedValue != "0")
                    {
                        using (dt = GetSemesterFeePaidForGrid())
                        {
                            if (dt.Rows.Count > 0)
                            {
                                var CentreBatchs = (from p in dt.AsEnumerable()
                                                    select new
                                                    {
                                                        ID = p.Field<Int64>("ID"),
                                                        stdId = p.Field<Int64>("stdId"),
                                                        Name = p.Field<string>("Name"),
                                                        CourseName = p.Field<string>("CourseName"),
                                                        BatchName = p.Field<string>("BatchName"),
                                                        SemId = p.Field<Int32>("SemId"),
                                                        Number = p.Field<string>("Number"),
                                                        feeType = p.Field<string>("feeType"),
                                                        AmtPaid = p.Field<Int32>("AmtPaid"),
                                                        paymentdate = p.Field<string>("paymentdate"),
                                                        Remarks = p.Field<string>("Remarks"),
                                                        BatchCode = p.Field<string>("BatchCode"),
                                                        CourseID = p.Field<Int64>("CourseID")

                                                    });

                                 if (CourseIDFilter != 0 && BatchIDFilter != 0 && studentid!=0)
                                {
                                    CentreBatchs = CentreBatchs.Where(s => s.CourseID == CourseIDFilter && s.ID == BatchIDFilter && s.stdId==studentid);
                                }
                                 else if (CourseIDFilter != 0 && BatchIDFilter != 0)
                                {
                                    CentreBatchs = CentreBatchs.Where(s => s.CourseID == CourseIDFilter && s.ID == BatchIDFilter);
                                }
                                
                                else if (CourseIDFilter != 0)
                                {
                                    CentreBatchs = CentreBatchs.Where(s => s.CourseID == CourseIDFilter);
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
                                        case "BatchCode":
                                            if (sortOrder == "DESC")
                                                CentreBatchs = CentreBatchs.OrderByDescending(s => s.BatchCode);
                                            else
                                                CentreBatchs = CentreBatchs.OrderBy(s => s.BatchCode);
                                            break;
                                       
                                        default:
                                            CentreBatchs = CentreBatchs.OrderBy(s => s.Name);
                                            break;
                                    }
                                }

                                PagingBar1.Bind(CentreBatchs, ref gvMain);
                                uPnlGrid.Update();
                                uPnlNavigation.Update();
                                lblError.Visible = false;
                                trpage.Visible = true;
                                divsave.Visible = false;
                                PagingBar1.Visible = true;
                                gvMain.Visible = true;
                                divGrid.Visible = true;
                                if (gvMain.Rows.Count <= 0)
                                {
                                    lblError.Text = "No record found.";
                                    lblError.Visible = true;
                                }
                                lblError.Visible = false;
                                gvMain.Visible = true;
                                PagingBar1.Visible = true;
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

    public DataTable GetSemesterFeeFilterForGrid()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        string searchString = ucSearchBar.SearchText.Trim().ToUpper();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSemesterFeeSearchForGrid", con))
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
    public DataTable GetSemesterFeePaidForGrid()
    {
        Int64 ent = Convert.ToInt64(Session["EntityID"]);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetSemesterFeePaidDetailsForGrid", con))
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

    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
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
            BreadCrumb1.Render();
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
            BreadCrumb1.Render();
            EConnectContext context1 = new EConnectContext();
            User objUser = new EConnect.URM.User();
            Int64 feetypemasid = Convert.ToInt64(ddlfeetypemasid.SelectedValue);
            Int64 AmountPaid = Convert.ToInt64(txtAmountPaid.Text);
            Int64 batchid = Convert.ToInt64(ddlbatchSession.SelectedValue);
            Int64 CourseId = Convert.ToInt64(ddlCourse.SelectedValue);
            Int64 StudentID = Convert.ToInt64(ddlStudent.SelectedValue);
            Int32 Number = Convert.ToInt32(ddlsemester.SelectedItem.Text);
            Int32 semno = Number - 1;
            using (NIELITMISContext context = new NIELITMISContext())
            {
                SemesterFeePaid objSemesterFeePaid;

                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    objSemesterFeePaid = new SemesterFeePaid();

                    if (AmountPaid < 0)
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Amount can not be negative";
                        return;
                    }

                    var application = (from a in context.SemesterFeePaid
                                       join s in context.NielitCentreStudent on a.studentID equals s.ID
                                       where s.semesterid == Number && s.batch_ID == batchid && s.CourseID == CourseId && a.feeTypeID == feetypemasid
                                       select new
                                       {
                                           ID = a.ID,
                                           CourseId = s.CourseID,
                                           SemNo = s.semesterid

                                       }).FirstOrDefault();

                    if (application != null)
                    {
                        lblMessage.Visible = true;
                        strMessage = "Record already exist for this semester No. Please select another semester No.";
                        lblMessage.Text = "Record already exist for this semester No. Please select another semester No.";
                        return;
                    }

                    if (Number != 1)
                    {
                        var applicationcheck = (from a in context.NielitCentreStudent
                                                join s in context.SemesterFeePaid on a.ID equals s.studentID

                                                where a.semesterid == semno && a.batch_ID == batchid && a.CourseID == CourseId
                                                select new
                                                {
                                                    ID = a.ID,
                                                    CourseId = a.CourseID,
                                                    SemNo = a.semesterid

                                                }).FirstOrDefault();

                        if (applicationcheck == null)
                        {
                            lblMessage.Visible = true;
                            strMessage = "Record not exist for previous semester No. Please select Previous semester No.";
                            lblMessage.Text = "Record not exist for previous semester No. Please select Previous semester No.";
                            return;
                        }
                    }

                    int StudentIDExists = (from b in context.SemesterFeePaid
                                           where b.studentID == StudentID && b.feeTypeID == feetypemasid
                                           select b).Count();

                    var item = (from x in context.SemesterFeeMaster where x.feeTypeID == feetypemasid && x.courseid == CourseId && x.batchID == batchid select x).First();

                    DateTime effectiveFrom = Convert.ToDateTime(item.effectiveFromDate);
                    DateTime effectiveTo = Convert.ToDateTime(item.effectiveToDate);
                    DateTime paymentDate = Convert.ToDateTime(txtPaymentDate.Text);

                    if (paymentDate > effectiveTo || paymentDate < effectiveFrom)
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Payment Date should be within effective from and effective to.!";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        strMessage = "Payment Date should be within effective from and effective to.!";
                        return;
                    }
                    Int64 feeamount = item.feeAmount;

                    if (AmountPaid <= feeamount && AmountPaid >= 0)
                    {

                        if (StudentIDExists == 0)
                        {
                            objSemesterFeePaid.AmtPaid = Convert.ToInt32(txtAmountPaid.Text);
                            objSemesterFeePaid.paymentDate = Convert.ToDateTime(txtPaymentDate.Text);
                            objSemesterFeePaid.Remarks = txtRemarks.Text;
                            objSemesterFeePaid.feeTypeID = feetypemasid;
                            objSemesterFeePaid.studentID = Convert.ToInt64(StudentID);
                            objSemesterFeePaid.enterBy = Convert.ToInt32(Session["UserID"]);
                            objSemesterFeePaid.enterDate = DateTime.Now;
                            context.SemesterFeePaid.Add(objSemesterFeePaid);
                            context.SaveChanges();
                            strMessage = "Data Saved successfully.";
                            lblMessage.Visible = true;
                            lblMessage.Text = "Data Saved successfully!";
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            var previousAmtPaid = (from x in context.SemesterFeePaid where x.feeTypeID == feetypemasid && x.studentID == StudentID select x).First();
                            Int32 PreviousAmtPaid = previousAmtPaid.AmtPaid;
                            DateTime predate = previousAmtPaid.paymentDate;
                            if (predate > paymentDate)
                            {
                                lblMessage.Visible = true;
                                lblMessage.Text = "Payment date must be greater than equal to last payment date  !";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                strMessage = "Payment date must be greater than equal to last payment date  !";
                                return;
                            }

                            if (AmountPaid > PreviousAmtPaid && AmountPaid >= 0)
                            {

                                SemesterFeePaid objSemesterFeePaidUpdate;
                                objSemesterFeePaidUpdate = new SemesterFeePaid();
                                StudentID = Convert.ToInt64(ddlStudent.SelectedValue);
                                objSemesterFeePaidUpdate = context.SemesterFeePaid.Single(k => k.studentID == StudentID && k.feeTypeID == feetypemasid);

                                objSemesterFeePaidUpdate.AmtPaid = Convert.ToInt32(txtAmountPaid.Text);

                                objSemesterFeePaidUpdate.paymentDate = Convert.ToDateTime(txtPaymentDate.Text);

                                objSemesterFeePaidUpdate.Remarks = txtRemarks.Text;
                                int amountpaid = Convert.ToInt32(txtAmountPaid.Text);
                                objSemesterFeePaidUpdate.enterBy = Convert.ToInt32(Session["UserID"]);
                                objSemesterFeePaidUpdate.enterDate = DateTime.Now;
                                SemesterFeePaidHistory(StudentID, amountpaid, feetypemasid);
                                context.SaveChanges();
                                strMessage = "Data updated successfully!";
                                lblMessage.Visible = true;
                                lblMessage.Text = "Data updated successfully!";
                            }

                            else
                            {
                                lblMessage.Visible = true;
                                lblMessage.Text = "Amount can not be less than previous amount!";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                                strMessage = "Amount can not be less than previous amount!";
                            }



                        }

                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Fee Amount should be positive and less than or equal to " + feeamount + "/- !";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        strMessage = "Fee Amount should be positive and less than or equal to " + feeamount + "/- !";
                    }
                }


            }
            Response.Redirect("SemesterFeePaid.aspx?msg=" + strMessage, true);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    public void SemesterFeePaidHistory(Int64 id, int amountpaid, Int64 feetypeid)
    {
        try
        {
            //string constr = ConfigurationManager.ConnectionStrings["NIELITMISContextt"].ConnectionString;
            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection Conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("InsertSemesterFeePaidHistory", Conn))
                {
                    Conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@amountpaid", amountpaid);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@feetypeid", feetypeid);
                    cmd.ExecuteNonQuery();
                }
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
            ddlCourseName.SelectedValue = "0";
            ddlbatchname.SelectedValue = "0";
            ddlsem.SelectedValue = "0";
            ddlstudentF.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
           // BindGridView();
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

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

            var BatchName = from s in context.NielitCentreBatchs
                            join b in context.SemesterFeeMaster on s.ID equals b.batchID
                            join f in context.SemesterFeePaid on b.feeTypeID equals f.feeTypeID
                            where s.centreID == NielitCentreIdSearch && s.IsVerified == true && b.feeAmount == f.AmtPaid
                            select new { Name = s.Name };

            if (!String.IsNullOrEmpty(searchString))
            {
                BatchName = BatchName.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            BatchName = BatchName.OrderBy(s => s.Name);

            var Batchcode = from s in context.NielitCentreBatchs
                            join b in context.SemesterFeeMaster on s.ID equals b.batchID
                            join f in context.SemesterFeePaid on b.feeTypeID equals f.feeTypeID
                            where s.centreID == NielitCentreIdSearch && s.IsVerified == true && b.feeAmount == f.AmtPaid
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
        Response.Redirect("SemesterFeePaid.aspx", true);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("SemesterFeePaid.aspx", true);
    }

    protected void ddlfeetypemasid_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtAmountPaid.Text = "";
        txtPaymentDate.Text = "";
        txtRemarks.Text = "";
        FillAmountPaid();
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
       // FillStudentIdFilter();
    }

    //protected void ddlstudent_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        using (NIELITMISContext context = new NIELITMISContext())
    //        {
    //            ListItem lst = new ListItem("--Select--", "0");
    //            Int64 coursenameid = Convert.ToInt64(ddlCourseName.SelectedValue);
    //            Int64 batchid = Convert.ToInt64(ddlbatchname.SelectedValue);
    //            Int64 StudentId = Convert.ToInt64(ddlstudent.SelectedValue);
    //            Int32 nielitcentreid = Convert.ToInt32(NIELITCentreId.Value);
    //            if (coursenameid != 0)
    //            {
    //                var Batch = from s in context.NielitCentreStudent
    //                            where s.InstituteID == nielitcentreid && s.CourseID == coursenameid && s.batch_ID == batchid
    //                                    && s.ID == StudentId
    //                            orderby (s.Name)
    //                            select new { ValueField = s.ID, TextField = s.semesterid };
    //                EConnect.Utils.Common.ControlUtility.BindListObject(ddlsem, Batch, lst);
    //            }
    //            //using (DataTable dt = GetSemester())
    //            //{
    //            //    if (dt.Rows.Count > 0)
    //            //    {
    //            //        int i = Convert.ToInt32(dt.Rows[0]["SemNo"]);
    //            //        int n = i;
    //            //        for (i = 0; i <= n; i++)
    //            //        {
    //            //            if (i == 0)
    //            //            {
    //            //                ddlsemester.Items.Add(new ListItem("Select", "0"));
    //            //            }
    //            //            else
    //            //            {
    //            //                ddlsemester.Items.Add(new ListItem(i.ToString(), i.ToString()));
    //            //                ddlsemester.DataBind();
    //            //            }
    //            //        }
    //            //    }
    //        }
    //    }


    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    // }
    //}

    protected void ddlsem_SelectedIndexchanged(object sender, EventArgs e)
    {
         
        FillStudentIdFilter();
    }
    }

