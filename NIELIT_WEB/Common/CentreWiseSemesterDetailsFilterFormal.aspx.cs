using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Common_CentreWiseSemesterDetailsFilterFormal : System.Web.UI.Page
    {

    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 NielitCentreId = 0;
    Int32 NielitCentreIdFilter = 0;
    Int32 UserTypeId = 0;
    string centre = "";
    protected void Page_Load(object sender, EventArgs e)
        {
        lblerror.Text = "";
        try
            {
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
                {
                //Response.Write("Sorry! You don't have rights  to view this page");
                //Response.End();
                }

            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);

            if (!IsPostBack)
                {
                bindCentre();
                }
            BreadCrumb1.Render();
            }
        catch (Exception ex)
            {
            lblerror.Text = ex.Message;
            }
        }

    #region vCode

    public void bindCentre()
        {
        try
            {
            using (NIELITMISContext context = new NIELITMISContext())
                {
                User objUser;
                using (EConnectContext context1 = new EConnectContext())
                    {
                    objUser = new EConnect.URM.User();
                    User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                    }

                if (UserTypeId == 6)
                    {
                    ListItem lst1 = new ListItem("--Select One--", "0");
                    var centreName1 = from s in context.NielitCentres
                                      select new { ValueField = s.ID, TextField = s.Name };

                    if (centreName1 != null)
                        {
                        var centreName = centreName1.OrderBy(i => i.ValueField);
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentreName, centreName.Distinct(), lst1);
                        ddlCentreName.Enabled = true;
                        }
                    }
                else
                    {
                    ListItem lst1 = new ListItem("--Select One--", "0");
                    var centreName = from s in context.NielitCentres
                                     where s.ID == NielitCentreId
                                     select new { ValueField = s.ID, TextField = s.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentreName, centreName.Distinct(), lst1);
                    ddlCentreName.SelectedValue = Convert.ToInt32(NielitCentreId).ToString();
                    ddlCentreName.Enabled = false;
                    }
                }
            }

        catch (Exception ex)
            {
            throw ex;
            }
        }

    protected void FillCourseNameFormal()
        {
        try
            {
            using (NIELITMISContext context = new NIELITMISContext())
                {
                ListItem lst = new ListItem("--Select One--", "0");

                var CourseListFormal = from d in context.NielitCourseDurations
                                       join c in context.NielitCentreCourses on d.courseID equals c.ID
                                       where c.CourseCategoryID == 101  // Only Formal COurses Display
                                       orderby (c.Name)
                                       select new { ValueField = d.ID, TextField = c.Name + "(" + d.courseDurationDays + " Day)" + d.courseDurationHrs + " Hrs" };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, CourseListFormal.Distinct(), lst);
                };
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }

    protected void FillBatchNameFormal()
        {
        try
            {
            NielitCentreId = Convert.ToInt32(ddlCentreName.SelectedValue);
            Int64 coursenameid = Convert.ToInt64(ddlCourse.SelectedValue);


            using (NIELITMISContext context = new NIELITMISContext())
                {
                ListItem lst = new ListItem("--Select One--", "0");

                var BatchListFormal = from d in context.NielitCourseDurations
                                      join c in context.NielitCentreCourses on d.courseID equals c.ID
                                      join b in context.NielitCentreBatchs on d.ID equals b.CourseDurationID
                                      where c.CourseCategoryID == 101  //Only Formal Courses Batchs Display
                                      && d.ID == coursenameid && b.centreID == NielitCentreId
                                      orderby (b.ID)
                                      select new { ValueField = b.ID, TextField = b.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, BatchListFormal.Distinct(), lst);
                };
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }

    protected void ddlCentreName_SelectedIndexChanged(object sender, EventArgs e)
        {
        //ddlReportType.Items.Clear();
        //ddlReportType.Items.Insert(0, new ListItem("Batches within dates", "D"));
        //ddlReportType.Items.Insert(0, new ListItem("Semester of a Batch", "C"));
        //ddlReportType.Items.Insert(0, new ListItem("--Select One--", "0"));

        //Trcourse.Visible = false;
        //TrcourseInput.Visible = false;
        //trBatchdate.Visible = false;
        //trbatchfrom.Visible = false;
        }

    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
        try
            {
            ddlBatch.Items.Insert(0, new ListItem("--Select One--", "0"));
            Int64 coursenameid = Convert.ToInt64(ddlCourse.SelectedValue);
            if (coursenameid != 0)
                {
                FillBatchNameFormal();
                }
            }
        catch (Exception ex)
            {
            // ShowAlert(ex.Message, true);
            }
        }

    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
        if (ddlReportType.SelectedValue == "C")
            {
            Trcourse.Visible = true;
            TrcourseInput.Visible = true;
            trBatchdate.Visible = false;
            trbatchfrom.Visible = false;
            FillCourseNameFormal();
            }
        else if (ddlReportType.SelectedValue == "D")
            {
            trBatchdate.Visible = true;
            trbatchfrom.Visible = true;
            Trcourse.Visible = false;
            TrcourseInput.Visible = false;
            }
        }

    protected void btnReset_Click(object sender, EventArgs e)
        {
        try
            {
            BreadCrumb1.Render();
            bindCentre();
            ddlReportType.SelectedValue = "0";
            ddlCourse.Items.Clear();
            ddlCourse.Items.Insert(0, new ListItem("--Select One--", "0"));
            ddlBatch.Items.Clear();
            ddlBatch.Items.Insert(0, new ListItem("--Select One--", "0"));
            txtBatchFrom.Text = "";
            txtBatchto.Text = "";
            }
        catch (Exception ex)
            {
            // ShowAlert(ex.Message, true);
            }
        }

    #endregion

    }
