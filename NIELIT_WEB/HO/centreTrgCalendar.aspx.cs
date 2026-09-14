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

public partial class HO_centreTrgCalendar : BasePage
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

                FillCourseCategory();
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    BindEditNewModeData();
                    ShowEditMode();
                }
                else
                {
                    // BindListData();
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    BindGridView();

                    if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Centre Training Calendar", "HO/centreTrgCalendar.aspx?CourseId=" + Request.QueryString["CourseId"].ToString() + "&CategoryID=" + Request.QueryString["CategoryID"].ToString(), ""));
                    }
                    else
                    {
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Centre Training Calendar", "HO/centreTrgCalendar.aspx", ""));
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

    protected void BindEditNewModeData()
    {
        try
        {

            FillCourseCategory();
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


    protected bool IsValidForm()
    {
        try
        {
            if (ddlNSQFAligned.SelectedValue.ToString() == "99")
            {
		ShowAlert("Please select Whether NSQF Aligned");
                //lblerrorddlcouse.Visible = true;
                //lblerrorddlcouse.Text = "Please Select the Project Name";
                return false;
            }
            if ((ddlAdmissionStatus.SelectedValue.ToString() == "0"))
{
		ShowAlert("Please select Admission Status");
                return false;
}
            if (ddlcoursecategory.SelectedValue.ToString() == "0")
{
		ShowAlert("Please select Course Category");
                return false;
}
            if (ddlCourse.SelectedValue.ToString() == "0")
{
		ShowAlert("Please select Course");
                return false;
}

            if (ddlNSQFAligned.SelectedValue.ToString() == "1")
                if (txtNSQFLevel.Text.Trim().Length == 0)
{
		ShowAlert("Please enter level");
                    return false;
}

            if (Convert.ToDateTime(txtstartDate.Text) < System.DateTime.Today && String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                ShowAlert("Calendar for old dates not allowed");
                return false;
            }


            if ((Convert.ToDateTime(txtendDate.Text) < System.DateTime.Today) && !String.IsNullOrEmpty(Request.QueryString["Key"]))
            {
                ShowAlert("Calendar for old dates or Admission Closed, Cannot modify");
                return false;
            }
            //DateTime dtSixMths = Convert.ToDateTime(txtstartDate.Text).AddMonths(6);

            if (Convert.ToDateTime(txtstartDate.Text) > System.DateTime.Today.AddMonths(12))
            {
                ShowAlert("Only upto twelve months calendar");
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }





    //protected void txtstartDate_TextChanged(object sender, EventArgs e)
    //{
    //    Int32 courseid = Convert.ToInt32(ddlCourse.SelectedValue);
    //    using (NIELITMISContext context = new NIELITMISContext())
    //    {
    //        int CourseIdExistinDurationList = (from b in context.NielitCourseDurations
    //                               where b.ID == courseid
    //                               select b).Count();
    //        if (CourseIdExistinDurationList > 0)
    //        {
    //            var courseRunningDays = (from p in context.NielitCourseDurations
    //                                       where p.ID == courseid
    //                                       select new
    //                                       {
    //                                           CourseRunningDays = p.courseDurationDays,
    //                                           CourseCode = p.courseID
    //                                       }).FirstOrDefault();
    //            Int32 coursDays = courseRunningDays.CourseRunningDays;
    //            DateTime EndDate = Convert.ToDateTime(txtstartDate.Text );
    //            EndDate = EndDate.AddDays(-1);
    //            txtendDate.Text = EndDate.AddDays(coursDays).ToString("dd-MMM-yyyy"); ;
    //        }
    //    } 
    //}


    protected void ShowEditMode()
    {
        try
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            //  ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Centre training Calendar";
            // tblNavLinks.Visible = true;
            //lblShowOnWeb.Visible = true;
            //lblIsVerifieds.Visible = true;
            //lblIsActive.Visible = true;
            // loginUserNo = 285714;
            EConnectContext context1 = new EConnectContext();
            User objUser = new EConnect.URM.User();
            User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
            Int32 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                Int64 Id = Convert.ToInt32(Request.QueryString["Key"]);
                var TrgCal = (from p in context.centreTrgCalendar
                              where p.ID == Id
                              //  && p.admissionStatus =="O"
                              select p).Count();

                if (TrgCal > 0)
                {

                    ddlCourse.Enabled = false;

                    ddlAdmissionStatus.Enabled = true;

                    ddlNSQFAligned.Enabled = true;

                    txtLevelParticipant.Enabled = true;

                    txtstartDate.Enabled = true;
                    txtendDate.Enabled = true;
                    txtDurationMonths.Enabled = true;
                    txtDurationHours.Enabled = true;

                    txtName.Enabled = true;
                    txtEmail.Enabled = true;


                    var centreTrgCal = (from p in context.centreTrgCalendar
                                        where p.ID == Id
                                        select p).FirstOrDefault();


                    ddlcoursecategory.SelectedValue = centreTrgCal.courseCategoryID.ToString();
                    FillCourse(Convert.ToInt32(ddlcoursecategory.SelectedValue));
                    ddlCourse.SelectedValue = centreTrgCal.courseId.ToString();

                    if (centreTrgCal.whetherNSQFAligned)
                        ddlNSQFAligned.SelectedValue = "1";
                    else
                        ddlNSQFAligned.SelectedValue = "0";


                    if (centreTrgCal.NSQFLevel != null)
                        txtNSQFLevel.Text = centreTrgCal.NSQFLevel.ToString();

                    txtLevelParticipant.Text = centreTrgCal.participantEligibility;



                    txtstartDate.Text = centreTrgCal.startDate.ToString("dd-MMM-yyyy");
                    txtendDate.Text = centreTrgCal.endDate.ToString("dd-MMM-yyyy");

                    if (centreTrgCal.durationHrs != null)
                        txtDurationHours.Text = centreTrgCal.durationHrs.ToString();
                    if (centreTrgCal.durationMths != null)
                        txtDurationMonths.Text = centreTrgCal.durationMths.ToString();

                    ddlAdmissionStatus.SelectedValue = centreTrgCal.admissionStatus;

                    txtName.Text = centreTrgCal.courseInchargeName;
                    txtDesignation.Text = centreTrgCal.courseInchargeDesig;
                    txtSTDCode.Text = centreTrgCal.courseInchargeSTDCode;
                    txtPhone1.Text = centreTrgCal.courseInchargePhone1;
                    txtPhone2.Text = centreTrgCal.courseInchargePhone2;
                    txtMobile.Text = centreTrgCal.courseInchargeMobile;
                    txtEmail.Text = centreTrgCal.courseInchargeEmail.ToString();

                    btnCancel.Visible = false;
                    btnSave.Visible = true;
                    btnBack.Visible = true;
                    /* if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                        {
                        btnSave.Visible = false;
                        }*/
                    if (Convert.ToDateTime(txtendDate.Text) < System.DateTime.Today)
                    {
                        ShowAlert("Cannot Update, Course is over");
                        btnSave.Visible = false;
                    }
                    if (ddlAdmissionStatus.SelectedValue == "C")
                    {
                        ShowAlert("This Course Admission is Already Closed, Cannot Alter details in calendar now");
                        btnSave.Visible = false;
                    }
                }


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
    public DataTable FillGridView(Int64 pCentreId, DateTime pStartDateFrom, DateTime pStartDateTo, string pAdmissionStatus)
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("getTrgCalRecords", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    cmd.Parameters.Add("@pCentreId", SqlDbType.BigInt);
                    cmd.Parameters["@pCentreId"].Value = pCentreId;

                    cmd.Parameters.Add("@pStartDateFrom", SqlDbType.DateTime);
                    cmd.Parameters["@pStartDateFrom"].Value = pStartDateFrom;

                    cmd.Parameters.Add("@pStartDateTo", SqlDbType.DateTime);
                    cmd.Parameters["@pStartDateTo"].Value = pStartDateTo;

                    cmd.Parameters.Add("@pAdmissionStatus", SqlDbType.VarChar, 1);
                    cmd.Parameters["@pAdmissionStatus"].Value = pAdmissionStatus;

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
                //string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                DateTime startDateF, endDateF;
                string vAdmissionStatus;

                startDateF = System.DateTime.Now.AddMonths(2);
                endDateF = System.DateTime.Now.AddMonths(2);

                if (txtStartDatefilter.Text.Trim().Length != 0)
                    startDateF = Convert.ToDateTime(txtStartDatefilter.Text);
                if (txtStartDateToFilter.Text.Trim().Length != 0)
                    endDateF = Convert.ToDateTime(txtStartDateToFilter.Text);
                vAdmissionStatus = ddlAdmissionStatusFilter.SelectedValue.ToString().Trim();


                int loginCentre = Convert.ToInt32(entityID);
                //int loginCentre = 5007;

                var trgCal = from a in context.centreTrgCalendar
                             where a.startDate >= startDateF
                             && a.startDate <= endDateF
                             select a;

                using (DataTable dt = FillGridView(loginCentre, startDateF, endDateF, vAdmissionStatus))
                {
                    if (dt.Rows.Count > 0)
                    {
                        var centreTrgCal = (from p in dt.AsEnumerable()
                                            select new
                                            {
                                                ID = p.Field<Int64>("ID"),
                                                CourseName = p.Field<string>("CourseName"),
                                                AdmissionStatus = p.Field<string>("AdmissionStatus"),
                                                startDate = p.Field<DateTime>("startDate"),
                                                endDate = p.Field<DateTime>("endDate"),
                                                CourseID = p.Field<Int64>("CourseID"),
                                                durationHrs = p.Field<string>("duration1"),
                                                mobEmail = p.Field<string>("mobEmail"),
                                                courseInchargeName = p.Field<string>("courseInchargeName"),

                                            });

                        PagingBar1.Bind(centreTrgCal, ref gvMain);
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    ////            Int64 CourseName = 0;
    ////            Int32 batchname = 0;
    ////            Int32 VerifiedStatus = 0;               
    ////            Int32 NielitCentreIdRefAfflorNonAffl = 0, subcenteridLoginRef=0;
    ////            if (ddlCourseName.SelectedValue != "0")
    ////                CourseName = Convert.ToInt64(ddlCourseName.SelectedValue);


    ////            //if (ddlbatchcode.SelectedValue != "0") 
    ////            //    Batchcodeid = Convert.ToInt32(ddlbatchcode.SelectedValue);              
    ////            User objUser;
    ////            using (EConnectContext context1 = new EConnectContext())
    ////            {
    ////                objUser = new EConnect.URM.User();

    ////                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();

    ////            //var CentreBatchs = from s in context.NielitCentreBatchs
    ////            //                   join c in context.NielitCourseDurations on s.CourseDurationID equals c.ID 
    ////            //                   join p in context.NielitCentreCourses on c.courseID equals p.ID                                 
    ////            //                 select new
    ////            //                 {
    ////            //                     ID = s.ID,
    ////            //                     Name = s.Name,
    ////            //                     CourseName=p.Name,
    ////            //                     BatchCode = s.BatchCode,
    ////            //                     subcenterid=s.subCentreID,
    ////            //                     startDate = s.startDate,
    ////            //                     endDate = s.endDate,
    ////            //                     CourseID = s.CourseDurationID,
    ////            //                     CentreID=s.centreID,
    ////            //                     enterBy=s.enterBy,
    ////            //                     IsVerified = s.IsVerified ? "YES" : "NO",
    ////            //                 };
    ////            using (DataTable dt = FillGridViewNIELITMISCourseNielitCourseRecord())
    ////            {
    ////                if (dt.Rows.Count > 0)
    ////                {
    ////                    var CentreBatchs = (from p in dt.AsEnumerable()
    ////                                   select new
    ////                                   {
    ////                                       ID = p.Field<Int64>("ID"),
    ////                                       Name = p.Field<string>("Name"),
    ////                                       CourseName = p.Field<string>("CourseName"),
    ////                                       BatchCode = p.Field<string>("BatchCode"),
    ////                                       subcenterid = p.Field<Int64>("subcenterid"),
    ////                                       startDate = p.Field<DateTime>("startDate"),
    ////                                       endDate = p.Field<DateTime>("endDate"),
    ////                                       CourseID = p.Field<Int64>("CourseID"),
    ////                                       CentreID = p.Field<Int64>("CentreID"),
    ////                                       enterBy = p.Field<int>("enterBy"),
    ////                                       IsVerified = p.Field<string>("IsVerified"),
    ////                                       IsVerifiedid = p.Field<int>("IsVerifiedid"),
    ////                                   });         
    ////                    if (NielitCentreIdRefAfflorNonAffl != 0)
    ////                    {
    ////                        CentreBatchs = CentreBatchs.Where(s => s.CentreID == NielitCentreIdRefAfflorNonAffl);
    ////                    }
    ////                    if (UserTypeId != 10)
    ////                    {
    ////                        CentreBatchs = CentreBatchs.Where(s => s.enterBy == loginUserNo || s.subcenterid == subcenteridLoginRef);
    ////                    }
    ////                    if (!String.IsNullOrEmpty(searchString))
    ////                    {
    ////                        CentreBatchs = CentreBatchs.Where(s => s.Name.ToUpper().Contains(searchString) || s.BatchCode.ToUpper().Contains(searchString));
    ////                    }
    ////                    if (CourseName != 0 && batchname != 0)
    ////                    {
    ////                        CentreBatchs = CentreBatchs.Where(s => s.CourseID == CourseName && s.ID == batchname);
    ////                    }
    ////                    else if (CourseName != 0)
    ////                    {
    ////                        CentreBatchs = CentreBatchs.Where(s => s.CourseID == CourseName);
    ////                    }
    ////                    else if (batchname != 0)
    ////                    {
    ////                        CentreBatchs = CentreBatchs.Where(s => s.ID == batchname);
    ////                    }
    ////                    if (VerifiedStatus != 99)
    ////                    {
    ////                        CentreBatchs = CentreBatchs.Where(s => s.IsVerifiedid == VerifiedStatus);
    ////                    }
    ////                    //if (Batchcodeid != 0)
    ////                    //{
    ////                    //    CentreBatchs = CentreBatchs.Where(s => s.ID == Batchcodeid);
    ////                    //}              

    ////                    if (!string.IsNullOrEmpty(sortOrder))
    ////                    {
    ////                        switch (sortField)
    ////                        {
    ////                            case "ID":
    ////                                if (sortOrder == "DESC")
    ////                                    CentreBatchs = CentreBatchs.OrderByDescending(s => s.ID);
    ////                                else
    ////                                    CentreBatchs = CentreBatchs.OrderBy(s => s.ID);
    ////                                break;
    ////                            case "Name":
    ////                                if (sortOrder == "DESC")
    ////                                    CentreBatchs = CentreBatchs.OrderByDescending(s => s.Name);
    ////                                else
    ////                                    CentreBatchs = CentreBatchs.OrderBy(s => s.Name);
    ////                                break;
    ////                            case "BatchCode":
    ////                                if (sortOrder == "DESC")
    ////                                    CentreBatchs = CentreBatchs.OrderByDescending(s => s.BatchCode);
    ////                                else
    ////                                    CentreBatchs = CentreBatchs.OrderBy(s => s.BatchCode);
    ////                                break;
    ////                            case "startDate":
    ////                                if (sortOrder == "DESC")
    ////                                    CentreBatchs = CentreBatchs.OrderByDescending(s => s.startDate);
    ////                                else
    ////                                    CentreBatchs = CentreBatchs.OrderBy(s => s.startDate);
    ////                                break;
    ////                            case "endDate":
    ////                                if (sortOrder == "DESC")
    ////                                    CentreBatchs = CentreBatchs.OrderByDescending(s => s.endDate);
    ////                                else
    ////                                    CentreBatchs = CentreBatchs.OrderBy(s => s.endDate);
    ////                                break;
    ////                            default:
    ////                                CentreBatchs = CentreBatchs.OrderBy(s => s.Name);
    ////                                break;
    ////                        }
    ////                    }

    ////                    PagingBar1.Bind(CentreBatchs, ref gvMain);
    ////                    uPnlGrid.Update();
    ////                    uPnlNavigation.Update();
    ////                    // gvMain.Columns[5].Visible = false;
    ////                    if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
    ////                    {
    ////                        gvMain.Columns[7].Visible = false;
    ////                    }
    ////                }
    ////            }                



    ////    catch (Exception ex)
    ////    {
    ////        throw ex;
    ////    }
    ////}
    ////}

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
            //    if (!UserManager.HasRight(currentRoleId, enmRight.New))
            //    {
            //        BreadCrumb1.Render();
            //        ShowAlert("Sorry! You don't have rights to add new record.", true);
            //        return;
            //    }
            BindEditNewModeData();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            // ucSearchBar.Visible = false;
            // this.Rview.Visible = false;
            //this.Rview1.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "Centre Training Calendar";
            //Updating Breadcrumb         
            // BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("New Training Calendar", "HO/centreTrgCalendar.aspx", ""));
            BreadCrumb1.Render();
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["CourseId"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("centreTrgCalendar.aspx?ID=" + Request.QueryString["CourseID"].ToString()), true);
            }
            else
            {
                Response.Redirect("centreTrgCalendar.aspx", true);
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
                //Int32 NielitCentreId = 5007;
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    centreTrgCalendar centreTrgCal;

                    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        centreTrgCal = new centreTrgCalendar();

                        centreTrgCal.CentreId = NielitCentreId;
                        HNANFL.Value = "O";

                        centreTrgCal.courseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                        centreTrgCal.courseId = Convert.ToInt32(ddlCourse.SelectedValue);
                        centreTrgCal.admissionStatus = ddlAdmissionStatus.SelectedValue.ToString();
                        centreTrgCal.durationHrs = Convert.ToInt32(txtDurationHours.Text);
                        centreTrgCal.durationMths = Convert.ToInt32(txtDurationMonths.Text);

                        centreTrgCal.startDate = Convert.ToDateTime(txtstartDate.Text);
                        centreTrgCal.endDate = Convert.ToDateTime(txtendDate.Text);

                        centreTrgCal.participantEligibility = txtLevelParticipant.Text;

                        if (ddlNSQFAligned.SelectedValue.ToString() == "N")
                            centreTrgCal.whetherNSQFAligned = false;
                        else
                            centreTrgCal.whetherNSQFAligned = true;
                        if (ddlNSQFAligned.SelectedValue.ToString() == "1")
                            centreTrgCal.NSQFLevel = Convert.ToInt32(txtNSQFLevel.Text);

                        //Course Cordinator details
                        centreTrgCal.courseInchargeName = txtName.Text;
                        centreTrgCal.courseInchargeDesig = txtDesignation.Text;
                        centreTrgCal.courseInchargePhone1 = txtPhone1.Text;
                        centreTrgCal.courseInchargePhone2 = txtPhone2.Text;
                        centreTrgCal.courseInchargeSTDCode = txtSTDCode.Text;
                        centreTrgCal.courseInchargeMobile = txtMobile.Text;
                        centreTrgCal.courseInchargeEmail = txtEmail.Text;

                        centreTrgCal.enterDate = DateTime.Now;
                        centreTrgCal.enterByID = loginUserNo;

                        context.centreTrgCalendar.Add(centreTrgCal);
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
                        centreTrgCal = context.centreTrgCalendar.Find(Convert.ToInt32(Request.QueryString["key"]));



                        centreTrgCal.admissionStatus = ddlAdmissionStatus.SelectedValue.ToString();
                        centreTrgCal.durationHrs = Convert.ToInt32(txtDurationHours.Text);
                        centreTrgCal.durationMths = Convert.ToInt32(txtDurationMonths.Text);

                        centreTrgCal.startDate = Convert.ToDateTime(txtstartDate.Text);
                        centreTrgCal.endDate = Convert.ToDateTime(txtendDate.Text);

                        centreTrgCal.participantEligibility = txtLevelParticipant.Text;

                        if (ddlNSQFAligned.SelectedValue.ToString() == "N")
                            centreTrgCal.whetherNSQFAligned = false;
                        else
                            centreTrgCal.whetherNSQFAligned = true;
                        if (ddlNSQFAligned.SelectedValue.ToString() == "1")
                            centreTrgCal.NSQFLevel = Convert.ToInt32(txtNSQFLevel.Text);

                        //Course Cordinator details
                        centreTrgCal.courseInchargeName = txtName.Text;
                        centreTrgCal.courseInchargeDesig = txtDesignation.Text;
                        centreTrgCal.courseInchargePhone1 = txtPhone1.Text;
                        centreTrgCal.courseInchargePhone2 = txtPhone2.Text;
                        centreTrgCal.courseInchargeSTDCode = txtSTDCode.Text;
                        centreTrgCal.courseInchargeMobile = txtMobile.Text;
                        centreTrgCal.courseInchargeEmail = txtEmail.Text;



                        strMessage = "Record updated.";
                        context.SaveChanges();
                    }
                }
                Response.Redirect("centreTrgCalendar.aspx?msg=" + strMessage, true);
            }

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
            BindGridView();
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
            txtStartDatefilter.Text = "";
            txtStartDateToFilter.Text = "";
            ddlAdmissionStatusFilter.SelectedValue = "0";
            //         ddlbatchname.SelectedValue = "0";
            //       ddlBatchVerifiedStatus.SelectedValue = "99";
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
            //     BindGridView();
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
                //       BindGridView();
                ShowAlert("Record deleted successfully.", true);
                hfActionID.Value = "";
            };
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            // BindGridView();
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


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("centreTrgCalendar.aspx", true);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("centreTrgCalendar.aspx", true);
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
                // BindGridView();
                ShowAlert("You have successfully changed the status of the Exam Center.", true);
                hfActionID.Value = "";
            };
            uPnlGrid.Update();
        }
        catch (Exception ex)
        {
            // BindGridView();
            uPnlGrid.Update();
            ShowAlert("Record can not be edited!", true);
        }
    }
    protected void ddlAdmissionStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCourse(Convert.ToInt32(ddlcoursecategory.SelectedValue));
    }

    protected void FillCourseCategory()
    {
        try
        {
            using (DataTable dt = FillCourseCategoryRecords())
            {
                if (dt.Rows.Count > 0)
                {
                    ddlcoursecategory.DataSource = dt;
                    ddlcoursecategory.DataTextField = "Name";
                    ddlcoursecategory.DataValueField = "ID";
                    ddlcoursecategory.DataBind();
                    ddlcoursecategory.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable FillCourseCategoryRecords()
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("GetCourseCategoryNIELITMISCourseCatNielitCourseCatRecord", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
    protected void FillCourse(int pCourseCategory)
    {
        try
        {
            using (DataTable dt = FillCourseRecords(pCourseCategory))
            {
                if (dt.Rows.Count > 0)
                {
                    ddlCourse.DataSource = dt;
                    ddlCourse.DataTextField = "Name";
                    ddlCourse.DataValueField = "ID";
                    ddlCourse.DataBind();
                    ddlCourse.Items.Insert(0, new ListItem("--Select One--", "0"));
                }
                BreadCrumb1.Render();
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable FillCourseRecords(int pCourseCategory)
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("getNIELITCoursesAll", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.Add("@pCourseCat", SqlDbType.BigInt);
                    cmd.Parameters["@pCourseCat"].Value = pCourseCategory;

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
    protected void ddlNSQFAligned_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlNSQFAligned.SelectedValue.ToString() == "1")
        { //txtNSQFLevel.Enabled = true;
            //Added for NSQF level / Duration
            DataTable dt = getNSQFLevelHours(Convert.ToInt32(ddlCourse.SelectedValue));
            if (dt.Rows.Count > 0)
            {
                txtNSQFLevel.Text = dt.Rows[0]["Level"].ToString();
                txtDurationHours.Text = dt.Rows[0]["Duration"].ToString();
                txtDurationHours.Visible = true;
                txtNSQFLevel.Enabled = false;
                txtDurationHours.Enabled = false;
            }
            else
            {
                txtNSQFLevel.Text = "";
                txtDurationHours.Text = "";
                txtDurationHours.Visible = true;
                txtNSQFLevel.Enabled = true;
                txtDurationHours.Enabled = true;
            }
        }



        else
        {
            txtNSQFLevel.Text = "";
            txtNSQFLevel.Enabled = false;
           
            txtDurationHours.Enabled = true;
        }
    }
    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlNSQFAligned.SelectedValue.ToString() == "1")
        {
            ddlNSQFAligned_SelectedIndexChanged (sender,e);
        }



        else
        {
            txtNSQFLevel.Text = "";
            txtNSQFLevel.Enabled = false;
            txtDurationHours.Text = "";
            txtDurationHours .Enabled =true;
        }
    }
    public DataTable getNSQFLevelHours(int pCourseId)
    {

        string constr = ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("getNSQFLevelHours", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.Add("@pCourseId", SqlDbType.BigInt);
                    cmd.Parameters["@pCourseId"].Value = pCourseId;

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
}