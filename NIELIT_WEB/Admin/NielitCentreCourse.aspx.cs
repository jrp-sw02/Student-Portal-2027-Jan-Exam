using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.URM;
using System.Configuration;
using System.Data.SqlClient;
using EConnect.DAL;
using EConnect.Utils.Common;
using System.Text.RegularExpressions;
using EConnect.NIELIT;
using System.Web;
using System.Transactions;
using System.Data.Objects;
using EConnect;

public partial class Admin_NielitCentreCourse : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 UserTypeId = 0;
    Int32 NielitCentreId = 0;
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
                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    RegionalCenter RegName = context.RegionalCenters.Find(loginUser.UserRefNumber);
                    NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                    hddnielitId.Value = loginUserNo.ToString();
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                    {
                        FillCourseCategory();
                        FillNielitTrgSpecialization();
                        ShowEditMode();
                    }
                    else
                    {
                        using (NIELITMISContext context1 = new NIELITMISContext())
                        {
                            ViewState["SortField"] = "";
                            ViewState["SortOrder"] = "";
                            FillCourseCategory();
                            FillNielitTrgSpecialization();
                            FillFilter();
                            FillFilterCourseCategory();
                            BindGridView();

                            //BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Courses", "Admin/NielitCentreCourse.aspx", ""));
                            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                            {
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Course List", "Admin/NielitCentreCourse.aspx?Id=" + Request.QueryString["Id"].ToString(), ""));
                            }
                            else
                            {
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Course List", "Admin/NielitCentreCourse.aspx", ""));
                            }
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
    protected void FillFilter()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                var CourseList = from p in context.NielitCentreCourses
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourses, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillCourseCategory()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.NielitCentreCourseCategorys
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, CourseList, lst);

                ListItem lst1 = new ListItem("--Select One--", "99");
                var verifyStatus = from p in context.verifyStatusMass
                                   orderby (p.verifyDescription)
                                   select new { ValueField = p.ID, TextField = p.verifyDescription };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlverifiedStatus, verifyStatus, lst1);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillNielitTrgSpecialization()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var NielitTrgSpecializationList = from p in context.NielitTrgSpecializations
                                                  orderby (p.specializationName)
                                                  select new { ValueField = p.ID, TextField = p.specializationName + "  ( " + p.specializationCode + " ) " };
                NielitTrgSpecializationList = NielitTrgSpecializationList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlNielitTrgSpecialization, NielitTrgSpecializationList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterCourseCategory()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                var CourseList = from p in context.NielitCentreCourseCategorys
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };

                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategoryName, CourseList, lst);
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
            Int32 NielitCentreCourseId1 = Convert.ToInt32(Request.QueryString["Key"]);
            Int32 NielitMisRecord = NielitCentreCourseId1.ToString().Length;
            if (NielitMisRecord > 3)
            {
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                btnSave.Text = "Update";
                lblHeading.Text = "NIELIT Centre Course Details";
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    Int32 NielitCentreCourseId = 0;
                    //Int32 NielitCentreCourseId = Convert.ToInt32(Request.QueryString["Id"]);
                    NielitCentreCourseId = Convert.ToInt32(Request.QueryString["Key"]);
                    NielitCentreCourse editCourse = context.NielitCentreCourses.Find(NielitCentreCourseId);
                    var course = (from p in context.NielitCentreCourses
                                  where p.ID == NielitCentreCourseId
                                  select new
                                  {
                                      ID = p.ID,
                                      Name = p.Name,
                                      Code = p.Code,
                                      CourseCategoryID = p.CourseCategoryID,
                                      NielitTrgSpclID = p.NielitTrgSpclID,
                                      displayorder = p.DisplayOrder,
                                      showOnWeb = p.ShowOnWeb,
                                      IsActive = p.IsActive,
                                      Isverified = p.IsVerified,
                                      IsverifiedV = p.IsVerified == true ? "YES" : p.IsVerified == false ? "NO" : "Pending",
                                      VerifiedStatus = p.verifiedStatus == null ? "99" : p.verifiedStatus.ToString(),
                                      VerificationMessage = p.verificationMessage == null ? "" : p.verificationMessage.ToString(),
                                      whetherShortTerm = p.whetherShortTerm,
                                      enterByRefId = p.enterBy,
                                      remarks = p.remarks
                                  }).FirstOrDefault();
                    //int courseIdExists = (from b in context.NielitCentreBatchs
                    //              where b.CourseID == courseId
                    //              select b).Count();               
                    txtdisplay.Text = course.displayorder.ToString();
                    txtdisplay.Enabled = false;
                    txtcoursename.Text = course.Name;
                    txtcoursecode.Text = course.Code;
                    TxtRemarks.Text = course.remarks;
                    ddlcoursecategory.SelectedValue = course.CourseCategoryID.ToString();
                    ddlcoursecategory.Enabled = false;
                    ddlNielitTrgSpecialization.SelectedValue = course.NielitTrgSpclID.ToString();
                    ddlNielitTrgSpecialization.Enabled = false;
                    ddlwhetherShortTerm.SelectedValue = course.whetherShortTerm.ToString();
                    ddlIsVerified.SelectedValue = course.Isverified.ToString();
                    ddlIsActive.SelectedValue = course.IsActive.ToString();
                    ddlShowOnWeb.SelectedValue = course.showOnWeb.ToString();
                    ddlverifiedStatus.SelectedValue = course.VerifiedStatus.ToString();
                    TxtverificationMessage.Text = course.VerificationMessage.ToString();

                    txtcoursename.Enabled = false;
                    txtcoursecode.Enabled = false;
                    TxtRemarks.Enabled = false;

                    // deep add on 25 feb 2021
                    TxtRequestFrom.Enabled = false;
                    if (UserTypeId == 6 || UserTypeId == 1 || UserTypeId == 9)
                    {
                        lblRequestFrom.Visible = true;
                        TxtRequestFrom.Visible = true;
                    }
                    else
                    {
                        lblRequestFrom.Visible = false;
                        TxtRequestFrom.Visible = false;
                    }
                    Int64 NielitCentrelinkedToCentreId = 0;
                    User objUser;
                    EConnectContext context1 = new EConnectContext();
                    objUser = new EConnect.URM.User();
                    Int32 UserTypeIdView = 0;
                    User loginUserView = context1.Users.Where(s => s.UserID == course.enterByRefId).FirstOrDefault();
                    UserTypeIdView = Convert.ToInt32(loginUserView.UserTypeID);
                    if (UserTypeIdView == 10)
                    {
                        var intituteslinkedToCentre = context.NielitCentres.Find(loginUserView.UserRefNumber);
                        Int32 NielitCentreId = Convert.ToInt32(loginUserView.UserRefNumber);

                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        if (NielitCentrelinkedToCentreId != 0)
                        {
                            NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                            TxtRequestFrom.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                        }
                        else
                        {
                            NielitCentres intitutesName = context.NielitCentres.Where(s => s.ID == NielitCentreId).FirstOrDefault();
                            TxtRequestFrom.Text = intitutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(intitutesName.ID);
                        }
                    }
                    else if (UserTypeIdView == 11)
                    {
                        var intituteslinkedToCentre = context.NonAffInstitutes.Find(loginUserView.UserRefNumber); //HNonAfflAfflInst                        
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            TxtRequestFrom.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        }
                    }
                    else if (UserTypeIdView == 4)
                    {
                        var intituteslinkedToCentre = context.AffInstitutes.Find(loginUserView.UserRefNumber);
                        NielitCentrelinkedToCentreId = Convert.ToInt32(intituteslinkedToCentre.linkedToCentre);
                        NielitCentres institutesName = context.NielitCentres.Where(s => s.ID == NielitCentrelinkedToCentreId).FirstOrDefault();
                        if (institutesName != null)
                        {
                            TxtRequestFrom.Text = institutesName.Name;
                            Int32 NelitCentreLinkId = Convert.ToInt32(institutesName.ID);
                        }
                    }
                    // deep end add on 25 feb 2021





                    if (course.whetherShortTerm == true)
                    {
                        ddlwhetherShortTerm.SelectedValue = "1";
                    }
                    else
                    {
                        ddlwhetherShortTerm.SelectedValue = "2";
                    }
                    ddlwhetherShortTerm.Enabled = false;
                    if (course.Isverified == true)
                    {
                        ddlIsVerified.SelectedValue = "1";
                    }
                    else
                    {
                        ddlIsVerified.SelectedValue = "2";
                    }
                    ddlIsVerified.Enabled = false;
                    if (course.IsActive == true)
                    {
                        ddlIsActive.SelectedValue = "1";
                    }
                    else
                    {
                        ddlIsActive.SelectedValue = "2";
                    }
                    ddlIsActive.Enabled = false;

                    if (course.showOnWeb == true)
                    {
                        ddlShowOnWeb.SelectedValue = "1";
                    }
                    else
                    {
                        ddlShowOnWeb.SelectedValue = "2";
                    }
                    ddlShowOnWeb.Enabled = false;
                    if (UserTypeId == 10)
                    {
                        ddlShowOnWeb.Enabled = false;
                        ddlIsActive.Enabled = false;
                        ddlIsVerified.Enabled = false;
                        this.Rview.Visible = false;
                        this.Rview1.Visible = false;
                        if (course.VerifiedStatus == "3")
                        {
                            TxtRemarks.Enabled = true;
                            ddlwhetherShortTerm.Enabled = true;
                            btnback.Visible = false;
                        }
                        if (course.IsverifiedV == "YES")
                        {
                            ddlIsVerified.SelectedValue = "1";
                            TxtRemarks.Enabled = false;
                            ddlwhetherShortTerm.Enabled = false;
                            btnSave.Visible = false;
                            btnCancel.Visible = false;
                            ddlwhetherShortTerm.Enabled = false;
                            btnback.Visible = true;
                        }
                        else if (course.IsverifiedV == "NO")
                        {
                            ddlIsVerified.SelectedValue = "2";
                            TxtRemarks.Enabled = false;
                            ddlwhetherShortTerm.Enabled = false;
                            btnSave.Visible = false;
                            btnCancel.Visible = false;
                            ddlwhetherShortTerm.Enabled = false;
                            btnback.Visible = true;
                        }
                        else if (course.IsverifiedV == "Pending")
                        {
                            if (course.VerifiedStatus != "99")
                            {
                                ddlverifiedStatus.SelectedValue = course.VerifiedStatus;
                            }
                            else
                            {
                                ddlverifiedStatus.SelectedItem.Text = "Not Available";

                            }
                            if (course.VerificationMessage == "")
                            {
                                TxtverificationMessage.Text = "Not Available";
                            }

                            TxtRemarks.Enabled = true;
                            ddlwhetherShortTerm.Enabled = true;
                            btnback.Visible = false;
                        }
                    }
                    else
                    {
                        if (course.Isverified == true)
                        {
                            ddlIsVerified.SelectedValue = "1";
                            int courseIdExists = (from b in context.NielitCourseDurations
                                                  join s in context.NielitCentreBatchs on b.ID equals s.CourseDurationID
                                                  where b.courseID == NielitCentreCourseId
                                                  select b).Count();
                            if (courseIdExists == 0)
                            {
                                ddlverifiedStatus.Enabled = true;
                                TxtverificationMessage.Enabled = true;
                            }
                        }
                        else
                        {
                            ddlIsVerified.SelectedValue = "2";
                            ddlverifiedStatus.Enabled = true;
                            TxtverificationMessage.Enabled = true;
                        }
                        ddlIsVerified.Enabled = true;

                        if (course.IsActive == true)
                        {
                            ddlIsActive.SelectedValue = "1";
                        }
                        else
                        {
                            ddlIsActive.SelectedValue = "2";
                        }
                        ddlIsActive.Enabled = true;

                        if (course.showOnWeb == true)
                        {
                            ddlShowOnWeb.SelectedValue = "1";
                        }
                        else
                        {
                            ddlShowOnWeb.SelectedValue = "2";
                        }
                        ddlShowOnWeb.Enabled = true;
                        //////////if (courseIdExists > 0)
                        //////////{
                        //////////    ddlIsVerified.Enabled = false;
                        //////////}
                        //////////else
                        //////////{
                        //////////    ddlIsVerified.Enabled = true;
                        //////////}

                        //if (course.whetherShortTerm == true)
                        //{
                        //    ddlwhetherShortTerm.SelectedValue = "1";
                        //}
                        //else
                        //{
                        //    ddlwhetherShortTerm.SelectedValue = "2";
                        //}
                        // ddlwhetherShortTerm.Enabled = true;                    
                    }
                    // hfAccID.Value = Request.QueryString["Id"];
                    //hfName.Value = Request.QueryString["Name"];    
                    //////Updating breadscrumb
                    ////BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(course.Name, "Admin/NielitCentreCourse.aspx?" + Request.QueryString.ToString(), ""));               
                };
                if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                {
                    btnSave.Visible = false;


                }
            }
            else
            {
                strMessage = "Sorry! You don't have rights to edit specific record.";
                Response.Redirect("NielitCentreCourse.aspx?msg=" + strMessage, true);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("NielitCentreCourse.aspx", true);

    }
    // deep add
   public DataTable GetCourseNielitCourseRecord()
   {

       string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
       DataTable myDt = new DataTable();
       using (SqlConnection con = new SqlConnection(constr))
       {
          try
           {
               using (SqlCommand cmd = new SqlCommand("GetCourseNielitCourseRecord", con))
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

    public DataTable getCentreWiseCourseRequest()
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            try
            {

                con.Open();

                // cmdg.Connection = con;
                SqlCommand cmd = new SqlCommand("getCentreWiseCourseRequest", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@centreId", SqlDbType.VarChar);
                cmd.Parameters["@centreId"].Value = NielitCentreId;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);

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
        return dt;
    }

    protected void BindGridView()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                EConnectContext context1 = new EConnectContext();
                User objUser;
                objUser = new EConnect.URM.User();
                User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                RegionalCenter RegName = context1.RegionalCenters.Find(loginUser.UserRefNumber);
                NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                // int courseType = 0;
                int NielitTrgSplID = 0;
                int courseCategory1 = 0;
                Int32 courseverifiedStatus = 99;
                int courseCategory = 0;
                int courseName = 0;
                //  NielitCentreId = Convert.ToInt32(hddnielitId.Value);
                if (ddlCourses.SelectedValue != "0")
                    courseName = Convert.ToInt32(ddlCourses.SelectedValue);
                if (ddlCourseName.SelectedValue != "0")
                    courseCategory1 = Convert.ToInt32(ddlCourseName.SelectedValue);
                if (ddlCourseVerifiedStatus.SelectedValue != "99")
                    courseverifiedStatus = Convert.ToInt32(ddlCourseVerifiedStatus.SelectedValue);
                if (ddlCourseCategoryName.SelectedValue != "0")
                    courseCategory = Convert.ToInt32(ddlCourseCategoryName.SelectedValue);
                if (ddlNielitTrgSpecialization.SelectedValue != "0")
                    NielitTrgSplID = Convert.ToInt32(ddlNielitTrgSpecialization.SelectedValue);
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();

                //var courses = (from s in context.NielitCentreCourses
                //               join c in context.NielitTrgSpecializations
                //                   on s.NielitTrgSpclID equals c.ID join p in context.NielitCentreCourseCategorys on s.CourseCategoryID equals p.ID
                //               join w in context.verifyStatusMass on s.verifiedStatus equals w.ID
                //                 into k  from verifyStatusMass in k.DefaultIfEmpty()
                //               select new
                //               {
                //                   ID = s.ID,
                //                   Name = s.Name,
                //                   Code = s.Code,
                //                   CategoryName = p.Name,
                //                   CourseCategoryID = s.CourseCategoryID,
                //                   DisplayOrder=s.DisplayOrder.ToString(),
                //                   WhetherShortTerm = s.whetherShortTerm ? "Yes" : "No",
                //                   Verified = s.IsVerified == true ? "YES" : s.IsVerified == false ? "NO" : "Pending",
                //                   VerifiedOn=s.verifiedOn,
                //                   verifiedId = s.IsVerified == true ? 1 : s.IsVerified == false ? 0 : -1,
                //                   // VerifiedStatus =  s.verifiedStatus,
                //                   VerifiedStatus = verifyStatusMass.verifyDescription,
                //                   NielitTrgSpecializationID = s.NielitTrgSpclID,
                //                   NielitTrgSplID = c.specializationCode,
                //                   EnterBy = s.enterBy,

                //               });                              
                if (UserTypeId == 6 || UserTypeId == 1 || UserTypeId == 9)
                {
                    using (DataTable dt = GetCourseNielitCourseRecord())
                    {
                        if (dt.Rows.Count > 0)
                        {
                            var courses = (from p in dt.AsEnumerable()
                                           select new
                    {
                        ID = p.Field<int>("ID"),
                        Name = p.Field<string>("Name"),
                        Code = p.Field<string>("Code"),
                        CategoryName = p.Field<string>("CategoryName"),
                        CourseCategoryID = p.Field<int>("CourseCategoryID"),
                        DisplayOrder = p.Field<int>("DisplayOrder").ToString(),
                        WhetherShortTerm = p.Field<string>("WhetherShortTerm"),
                        Verified = p.Field<string>("Verified"),
                        VerifiedOn = p.Field<DateTime?>("VerifiedOn"),
                        verifiedId = p.Field<int>("verifiedId"),
                        // // VerifiedStatus =  s.verifiedStatus,
                        VerifiedStatus = p.Field<string>("VerifiedStatus"),
                        NielitTrgSpecializationID = p.Field<Int64>("NielitTrgSpecializationID"),
                        NielitTrgSplID = p.Field<string>("NielitTrgSplID"),
                        EnterBy = p.Field<int>("EnterBy"),
                        IDC = p.Field<string>("IDC"), // 1 for NIELITMIS COURSES AND 0 FOR NIELIT COURSES
                    });
                            //    }
                            //}   
                        
                        if (!String.IsNullOrEmpty(searchString))
                        {
                            courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
                        }
                        if (courseverifiedStatus != 99)
                        {
                            courses = courses.Where(s => s.verifiedId == courseverifiedStatus);
                        }
                        if (courseCategory != 0)
                            courses = courses.Where(s => s.CourseCategoryID == courseCategory);
                        if (courseCategory != 0 && courseName != 0)
                            courses = courses.Where(s => s.CourseCategoryID == courseCategory && s.ID == courseName);
                        if (courseName != 0)
                            courses = courses.Where(s => s.ID == courseName);
                        if (NielitTrgSplID != 0)
                            courses = courses.Where(s => s.NielitTrgSpecializationID == NielitTrgSplID);
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "Name":
                                    if (sortOrder == "DESC")
                                        courses = courses.OrderByDescending(s => s.Name);
                                    else
                                        courses = courses.OrderBy(s => s.Name);
                                    break;

                                case "Code":
                                    if (sortOrder == "DESC")
                                        courses = courses.OrderByDescending(s => s.Code);
                                    else
                                        courses = courses.OrderBy(s => s.Code);
                                    break;

                                default:
                                    courses = courses.OrderBy(s => s.Name);
                                    break;
                            }
                        }
                        PagingBar1.Bind(courses, ref gvMain);
						 uPnlGrid.Update();
                        uPnlNavigation.Update();
                        gvMain.Visible = true;
                        lblErrMsg.Visible = false;
                        PagingBar1.Visible = true;
                        if (gvMain.Rows.Count <= 0)
                        {
                            lblErrMsg.Text = "No record found.";
                            lblErrMsg.Visible = true;
                            gvMain.Visible = false;
                            PagingBar1.Visible = false;
                        }
                        if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                        {
                            gvMain.Columns[7].Visible = false;
                        }
                    }
					
                    }

                    //deep add end
                }
                else
                {






                    using (DataTable dt = getCentreWiseCourseRequest())
                    {
                        if (dt.Rows.Count > 0)
                        {
                            var courses = (from p in dt.AsEnumerable()
                                           select new
                                           {
                                               ID = p.Field<int>("ID"),
                                               Name = p.Field<string>("Name"),
                                               Code = p.Field<string>("Code"),
                                               CategoryName = p.Field<string>("CategoryName"),
                                               CourseCategoryID = p.Field<int>("CourseCategoryID"),
                                               DisplayOrder = p.Field<int>("DisplayOrder").ToString(),
                                               WhetherShortTerm = p.Field<string>("WhetherShortTerm"),
                                               Verified = p.Field<string>("Verified"),
                                               VerifiedOn = p.Field<DateTime?>("VerifiedOn"),
                                               verifiedId = p.Field<int>("verifiedId"),
                                               // // VerifiedStatus =  s.verifiedStatus,
                                               VerifiedStatus = p.Field<string>("VerifiedStatus"),
                                               NielitTrgSpecializationID = p.Field<Int64>("NielitTrgSpecializationID"),
                                               NielitTrgSplID = p.Field<string>("NielitTrgSplID"),
                                               EnterBy = p.Field<int>("EnterBy"),
                                               // centreID = p.Field<Int64?>("centreID"),
                                               IDC = p.Field<string>("IDC"), // 1 for NIELITMIS COURSES AND 0 FOR NIELIT COURSES
                                           });
                            if (!String.IsNullOrEmpty(searchString))
                            {
                                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
                            }
                            if (courseverifiedStatus != 99)
                            {
                                courses = courses.Where(s => s.verifiedId == courseverifiedStatus);
                            }
                            if (courseCategory != 0)
                                courses = courses.Where(s => s.CourseCategoryID == courseCategory);
                            if (courseCategory != 0 && courseName != 0)
                                courses = courses.Where(s => s.CourseCategoryID == courseCategory && s.ID == courseName);
                            if (courseName != 0)
                                courses = courses.Where(s => s.ID == courseName);
                            if (NielitTrgSplID != 0)
                                courses = courses.Where(s => s.NielitTrgSpecializationID == NielitTrgSplID);
                            if (!string.IsNullOrEmpty(sortOrder))
                            {
                                switch (sortField)
                                {
                                    case "Name":
                                        if (sortOrder == "DESC")
                                            courses = courses.OrderByDescending(s => s.Name);
                                        else
                                            courses = courses.OrderBy(s => s.Name);
                                        break;

                                    case "Code":
                                        if (sortOrder == "DESC")
                                            courses = courses.OrderByDescending(s => s.Code);
                                        else
                                            courses = courses.OrderBy(s => s.Code);
                                        break;

                                    default:
                                        courses = courses.OrderBy(s => s.Name);
                                        break;
                                }
                            }
                            PagingBar1.Bind(courses, ref gvMain);
							 uPnlGrid.Update();
                        uPnlNavigation.Update();
                        gvMain.Visible = true;
                        lblErrMsg.Visible = false;
                        PagingBar1.Visible = true;
                        if (gvMain.Rows.Count <= 0)
                        {
                            lblErrMsg.Text = "No record found.";
                            lblErrMsg.Visible = true;
                            gvMain.Visible = false;
                            PagingBar1.Visible = false;
                        }
                        if (!UserManager.HasRight(currentRoleId, enmRight.Edit))
                        {
                            gvMain.Columns[7].Visible = false;
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
                ShowAlert("Sorry! You don't have rights to add new record.");
                return;
            }
            //FillCourseCategory();
            //FillNielitTrgSpecialization();
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;


            if (UserTypeId == 10)
            {
                ddlShowOnWeb.Enabled = false;
                ddlIsActive.Enabled = false;
                ddlIsVerified.Enabled = false;
                this.Rview.Visible = false;
                this.Rview1.Visible = false;
                this.Rview2.Visible = false;
                this.Rview3.Visible = false;
            }
            //Change the heading text as required
            lblHeading.Text = " NIELIT Course";
            //Updating Breadcrumb           
            BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT New Course", "", ""));
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
            {
                Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("NielitCentreCourse.aspx?ID=" + Request.QueryString["ID"].ToString()), true);
            }
            else
            {
                Response.Redirect("NielitCentreCourse.aspx", true);
            }
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
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
    protected void SaveRecordNielitCourse(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            using (NIELITMISContext context = new NIELITMISContext())
            {
                //create and object 
                NielitCentreCourse currentCourse;
                Boolean whetherShortTerm = false;
                Boolean showonweb = false;
                Boolean isactive = false;
                Boolean isverified = false;
                string sCourseCode = txtcoursecode.Text;
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    currentCourse = new NielitCentreCourse();
                    if (context.NielitCentreCourses.Where(s => s.Code == sCourseCode).Count() == 0)
                    {
                        Int32 NielitTrgSpecializationID = Convert.ToInt32(ddlNielitTrgSpecialization.SelectedValue);//NielitTrgSpecialization

                        currentCourse.CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);//course category id
                        currentCourse.NielitTrgSpclID = Convert.ToInt32(ddlNielitTrgSpecialization.SelectedValue);// course type id
                        currentCourse.Name = txtcoursename.Text;// course name
                        currentCourse.Code = txtcoursecode.Text;// course code
                        currentCourse.DisplayOrder = Convert.ToInt32(txtdisplay.Text);// dispaly order

                        if (ddlwhetherShortTerm.SelectedValue == "1")
                        {
                            whetherShortTerm = true;
                        }
                        if (ddlwhetherShortTerm.SelectedValue == "2")
                        {
                            whetherShortTerm = false;
                        }
                        currentCourse.whetherShortTerm = whetherShortTerm; // whetherShortTerm

                        if (ddlShowOnWeb.SelectedValue == "1")
                        {
                            showonweb = true;
                        }
                        if (ddlShowOnWeb.SelectedValue == "2")
                        {
                            showonweb = false; ;
                        }
                        currentCourse.ShowOnWeb = showonweb; // show on web   
                        if (ddlIsActive.SelectedValue == "1")
                        {
                            isactive = true;
                        }
                        if (ddlIsActive.SelectedValue == "2")
                        {
                            isactive = false; ;
                        }
                        currentCourse.IsActive = isactive; // Active or not Active course                   
                        currentCourse.remarks = TxtRemarks.Text;
                        // currentCourse.IsVerified = isverified; // verified or not verified course
                        currentCourse.enterDate = DateTime.Now; // entered course date by which
                        currentCourse.enterBy = Convert.ToInt32(Session["UserID"]); ;
                        context.NielitCentreCourses.Add(currentCourse);//save
                        context.SaveChanges();
                        strMessage = "New Record Saved";
                    }
                    else
                    {
                        txtcoursecode.Text = "";
                        txtcoursecode.Focus();
                        throw new Exception("This Course Code is Already Exists");
                    }
                }
                else
                {
                    currentCourse = new NielitCentreCourse();
                    currentCourse = context.NielitCentreCourses.Find(Convert.ToInt32(Request.QueryString["key"]));
                    currentCourse.Name = txtcoursename.Text;
                    currentCourse.Code = txtcoursecode.Text;

                    if (ddlwhetherShortTerm.SelectedValue == "1")
                    {
                        whetherShortTerm = true;
                    }
                    if (ddlwhetherShortTerm.SelectedValue == "2")
                    {
                        whetherShortTerm = false;
                    }
                    currentCourse.whetherShortTerm = whetherShortTerm; // whetherShortTerm
                    if (ddlIsActive.SelectedValue == "1")
                    {
                        isactive = true;
                    }
                    if (ddlIsActive.SelectedValue == "2")
                    {
                        isactive = false; ;
                    }
                    currentCourse.IsActive = isactive; // Active or not Active course                   
                    if (ddlverifiedStatus.SelectedValue == "1")
                    {
                        isverified = true;
                    }
                    if (ddlverifiedStatus.SelectedValue == "2")
                    {
                        isverified = false; ;
                    }
                    else
                    {
                    }
                    if (UserTypeId != 10)
                    {
                        currentCourse.verifiedBy = Convert.ToInt32(Session["UserID"]);
                        //currentCourse.IsVerified = isverified; // verified or not verified course
                        //currentCourse.verifiedOn = DateTime.Now;  
                        currentCourse.verifiedStatus = Convert.ToInt32(ddlverifiedStatus.SelectedValue);// verified Status
                        currentCourse.verificationStatusDate = DateTime.Now;
                        currentCourse.verificationMessage = TxtverificationMessage.Text;
                        if (ddlverifiedStatus.SelectedValue == "1")
                        {
                            currentCourse.IsVerified = true;
                            currentCourse.verifiedOn = DateTime.Now;
                        }
                        else if (ddlverifiedStatus.SelectedValue == "2")
                        {
                            currentCourse.IsVerified = false;
                            currentCourse.verifiedOn = DateTime.Now;
                        }
                        else
                        {
                            //currentCourse.IsVerified = false;
                        }
                        Int32 enterByRefId = currentCourse.enterBy;
                        string veridiedStatus = ddlverifiedStatus.SelectedItem.Text;
                        string courseName = txtcoursename.Text;
                        Int32 courseId = currentCourse.ID;
                        string entryDate = currentCourse.enterDate.ToString("dd-MMM-yyyy");

                        using (EConnectContext context1 = new EConnectContext())
                        {
                            if (enterByRefId != 0)
                            {
                                int contactregExists = (from s in context1.Users
                                                        //join p in context1.ExternalEntities on s.UserRefNumber equals p.ID
                                                        where s.UserID == enterByRefId
                                                        select s).Count();
                                if (contactregExists > 0)
                                {
                                    var contactreg = (from s in context1.Users
                                                      //  join p in context1.ExternalEntities on s.UserRefNumber equals p.ID
                                                      where s.UserID == enterByRefId
                                                      select new { s.LoginID, s.UserTypeID, s.EmailID, s.MobileNumber }).FirstOrDefault();
                                    if (ddlverifiedStatus.SelectedValue == "1") // COURSE  VERIFIED STATUS
                                    {
                                        //String EmailMsg = "Dear " + contactreg.LoginID + ", " + "This is regarding Course (" + courseName + ")"
                                        //    + " creation requested by you. The status of the course is " + veridiedStatus + ".";
                                        //sending Email
                                        if (contactreg.EmailID == null)
                                        {
                                            ShowAlert("Status is not sent on E-mail,");
                                        }
                                        else
                                        {
                                            if (contactreg.EmailID.Length > 0)
                                            {
                                                try
                                                {
                                                    String EmailMsg = "Dear " + contactreg.LoginID + ", " + "This is regarding Course (" + courseName + ")"
                                            + " creation requested by you. The status of the course is " + veridiedStatus + ".";
                                                    EConnect.NIELIT.Email mail = new Email("COURSE STATUS :NIELIT", EmailMsg, contactreg.EmailID);
                                                    mail.Send();
                                                }
                                                catch { ShowAlert("OTP is not sent on E-mail,"); }
                                            }
                                        }
                                    }
                                    else if (ddlverifiedStatus.SelectedValue == "2" || ddlverifiedStatus.SelectedValue == "3")  // COURSE WITHHELD OR NOT VERIFIED STATUS
                                    {
                                        //String EmailMsg = "Dear " + contactreg.LoginID + ", " + "This is regarding Course (" + courseName + ")"
                                        //    + " creation requested by you. The status of the course is " + veridiedStatus + "." + " The Remarks corresponding to the status is "
                                        // + TxtverificationMessage.Text + "," + " for further correspondence the reference number is "
                                        //+ courseId + " Date:-" + entryDate;
                                        //sending Email
                                        if (contactreg.EmailID == null)
                                        {
                                            ShowAlert("Status is not sent on E-mail,");
                                        }
                                        else
                                        {
                                            if (contactreg.EmailID.Length > 0)
                                            {
                                                try
                                                {
                                                    String EmailMsg = "Dear " + contactreg.LoginID + ", " + "This is regarding Course (" + courseName + ")"
                                           + " creation requested by you. The status of the course is " + veridiedStatus + "." + " The Remarks corresponding to the status is "
                                        + TxtverificationMessage.Text + "," + " for further correspondence the reference number is "
                                       + courseId + " Date:-" + entryDate;
                                                    EConnect.NIELIT.Email mail = new Email("COURSE STATUS :NIELIT", EmailMsg, contactreg.EmailID);
                                                    mail.Send();
                                                }
                                                catch { ShowAlert("OTP is not sent on E-mail,"); }
                                            }
                                        }
                                    }

                                }// exist user reference number in db                                

                            }// entery by record is 0.

                        }
                    }
                    if (ddlShowOnWeb.SelectedValue == "1")
                    {
                        showonweb = true;
                    }
                    if (ddlShowOnWeb.SelectedValue == "2")
                    {
                        showonweb = false; ;
                    }
                    currentCourse.ShowOnWeb = showonweb;
                    currentCourse.DisplayOrder = Convert.ToInt32(txtdisplay.Text);
                    currentCourse.remarks = TxtRemarks.Text;
                    context.SaveChanges();

                    strMessage = "Record Updated";
                }
            }
            Response.Redirect("NielitCentreCourse.aspx?msg=" + strMessage, true);
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
            ddlCourseCategoryName.SelectedValue = "0";
            // ddlCourses.SelectedValue = "0";
            FillFilter();
            ddlCourseName.SelectedValue = "0";
            ddlCourseVerifiedStatus.SelectedValue = "99";
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
            if (!UserManager.HasRight(currentRoleId, enmRight.Delete))
            {
                BindGridView();
                uPnlGrid.Update();
                BreadCrumb1.Render();
                ShowAlert("Sorry! You don't have rights to delete the records.", true);
                return;
            }
            using (TransactionScope scope = new TransactionScope())
            {
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    Int32 courseid = Convert.ToInt32(hfActionID.Value);
                    EConnect.NIELIT.NielitCentreCourse course = context.NielitCentreCourses.Find(courseid);
                    context.NielitCentreCourses.Remove(course);
                    context.SaveChanges();
                    scope.Complete();
                    ShowAlert("Record deleted successfully.", true);
                    hfActionID.Value = "";
                };
            }
            BindGridView();
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
                HyperLink hl2 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

                Image imgAction = (Image)e.Row.FindControl("imgAction");
                imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();

                CheckBox chk = (CheckBox)e.Row.FindControl("chk");
                imgAction.ID = "chk_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("NielitCentreCourse.aspx", true);
    }
    protected void ddlCourses_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id2 = 0;
        id2 = Convert.ToInt32(ddlCourses.SelectedValue);
        ddlCourseName.Items.Clear();
        FillCourseNames(id2);
    }

    protected void ddlCourseCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id2 = 0;
        id2 = Convert.ToInt32(ddlCourseCategoryName.SelectedValue);
        ddlCourses.Items.Clear();
        FillCourses(id2);
    }
    protected void FillCourses(int catID)
    {
        try
        {
            Int32 EnterbyRefNo = 0;
            EnterbyRefNo = Convert.ToInt32(hddnielitId.Value);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                if (catID != null || catID != 0)
                {

                    if (UserTypeId == 10)
                    {
                        var CourseName = from p in context.NielitCentreCourses
                                         where p.CourseCategoryID == catID && p.enterBy == EnterbyRefNo
                                         orderby (p.Name)
                                         select new { ValueField = p.ID, TextField = p.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourses, CourseName, lst);
                    }
                    else
                    {
                        var CourseName = from p in context.NielitCentreCourses
                                         where p.CourseCategoryID == catID
                                         orderby (p.Name)
                                         select new { ValueField = p.ID, TextField = p.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourses, CourseName, lst);
                    }

                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void FillCourseNames(int catID)
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                if (catID != null || catID != 0)
                {
                    var CourseName = from p in context.NielitCentreCourses
                                     where p.CourseCategoryID == catID
                                     orderby (p.Name)
                                     select new { ValueField = p.ID, TextField = p.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, CourseName, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlNielitTrgSpecialization_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 coursetypeID = Convert.ToInt32(ddlNielitTrgSpecialization.SelectedValue);
            Int32 ccatid = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            using (NIELITMISContext context = new NIELITMISContext())
            {
                // var DisplayOrder_max = 0;
                var countRecord = from s in context.NielitCentreCourses
                                  select new { Name = s.Name };

                var count = countRecord.Count();
                if (count != 0)
                {
                    var DisplayOrder_max = (from m in context.NielitCentreCourses
                                            select m).Max(m => m.DisplayOrder);
                    txtdisplay.Text = Convert.ToInt32(DisplayOrder_max + 1).ToString();
                }
                else
                {
                    txtdisplay.Text = Convert.ToInt32(1).ToString();
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 coursecatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        NIELITMISContext context = new NIELITMISContext();
        try
        {
            Int32 loginUserNo = 0, UserTypeId = 0;
            loginUserNo = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            UserTypeId = Convert.ToInt32(HttpContext.Current.Session["UserTypeId"]);
            if (count <= 0)
                count = 10;
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();

            var courses = from s in context.NielitCentreCourses
                          //where s.enterBy == loginUserNo
                          select new { Name = s.Name, Enterby = s.enterBy };
            if (UserTypeId == 10)
            {
                courses = courses.Where(s => s.Enterby == loginUserNo);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            courses = courses.OrderBy(s => s.Name).Take(count);

            foreach (var course in courses)
            {
                items.Add(course.Name);
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