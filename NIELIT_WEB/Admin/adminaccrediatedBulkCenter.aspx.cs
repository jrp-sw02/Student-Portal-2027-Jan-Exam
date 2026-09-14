using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using System.Web.Security;
using System.Data.OleDb;

public partial class adminaccrediatedBulkCenter : BasePage
{
    String strMessage = string.Empty;
    enmLanguage pageLanguage = enmLanguage.English;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");

            if (!Page.IsPostBack)
            {
                BindActivityGroup();
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {

                }
                else
                {
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Institutes List Upload", "Admin/adminaccrediatedBulkCenter.aspx", ""));
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";

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
    protected void BindActivityGroup()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var coursecategory = from s in context.CourseCategories
                                     select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategory, coursecategory, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]

    protected void ddlCourseCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 catg = Convert.ToInt32(ddlCourseCategory.SelectedItem.Value);
                ListItem lst = new ListItem("--Select One--", "0");
                var course = from s in context.Courses
                             where s.CourseCategoryID == catg
                                 //Added 17 Jan 2019
                             && s.ShowOnWeb
                             select new { ValueField = s.ID, TextField = s.Name + " (" + s.Code + ")" };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, course, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (ddlCourse.SelectedIndex == 0 || ddlCourseCategory.SelectedIndex == 0)
        {
            ShowAlert("Please select relevant Course Category and Course");
            return;
        }
        if (!fuInstitituteList.HasFile)
        {
            ShowAlert("Please select file to upload.");
            return;
        }
        string filepath = Server.MapPath("../UploadedFiles");
        fuInstitituteList.SaveAs(filepath + "/" + fuInstitituteList.FileName);
        string path = (filepath + "/" + fuInstitituteList.FileName);
        string ext = System.IO.Path.GetExtension(this.fuInstitituteList.PostedFile.FileName);
        string excelConnectionString = "";
        if (ext.ToUpper() == ".XLS")
            excelConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";", path);
        else if (ext.ToUpper() == ".XLSX")
            excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=No;IMEX=1\";", path);
        else
        {
            ShowAlert("Please Choose .XLS/.XLSX Extension File", true);
            return;
        }
        OleDbConnection connection = new OleDbConnection();
        connection.ConnectionString = excelConnectionString;
        connection.Open();
        OleDbCommand command = new OleDbCommand("select * from [Institutes$]", connection);
        OleDbDataReader dr = command.ExecuteReader();
        try
        {
            int instCnt = 0;
            int acrrNew = 0;
            int userCnt = 0;
            int totCount = 0;
            int userType = Convert.ToInt32(EConnect.URM.UserType.Institute);
            EConnect.NIELIT.Institute institute;


            while (dr.Read())
            {
                // using (TransactionScope scope = new TransactionScope()
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        try
                        {
                            if (dr[0] != null)
                            {

                                if (IsNumeric(dr[0].ToString()))
                                {
				
                                    totCount++;
                                    Int64 instituteID = Convert.ToInt64(dr[0].ToString());
                                    int courseID = Convert.ToInt32(ddlCourse.SelectedItem.Value);
                                    int courseCategoryID = Convert.ToInt32(ddlCourseCategory.SelectedItem.Value);
                                    institute = context.Institutes.Find(instituteID);
                                    if (institute == null)
                                    {
                                        institute = new EConnect.NIELIT.Institute();
                                        //Create New Institute
                                        institute.ID = Convert.ToInt64(dr[0].ToString());
                                        institute.Name = dr[1].ToString();
                                        institute.ContactPersonName = dr[2].ToString();
                                        institute.ContactPersonPost = dr[3].ToString();
                                        if (!String.IsNullOrEmpty(dr[4].ToString()))
                                            institute.StdNumber = Convert.ToInt32(dr[4].ToString());
                                        if (!String.IsNullOrEmpty(dr[5].ToString()))
                                            institute.PhoneNumber1 = Convert.ToInt32(dr[5].ToString());
                                        if (!String.IsNullOrEmpty(dr[6].ToString()))
                                            institute.PhoneNumber2 = Convert.ToInt32(dr[6].ToString());
                                        if (!String.IsNullOrEmpty(dr[7].ToString()))
                                            institute.MobileNumber = Convert.ToInt64(dr[7].ToString());
                                        if (!String.IsNullOrEmpty(dr[8].ToString()))
                                            institute.FaxNumber = Convert.ToInt32(dr[8].ToString());
                                        institute.EmailAddress1 = dr[9].ToString();
                                        institute.EmailAddress2 = dr[10].ToString();
                                        institute.WebAddress = dr[11].ToString();
                                        institute.AddressLine1 = dr[12].ToString();
                                        institute.AddressLine2 = dr[13].ToString();
                                        institute.AddressLine3 = dr[14].ToString();
                                        if (!String.IsNullOrEmpty(dr[15].ToString()))
                                            institute.StateID = Convert.ToInt32(dr[15].ToString());
                                        if (!String.IsNullOrEmpty(dr[16].ToString()))
                                            institute.DistrictID = Convert.ToInt32(dr[16].ToString());
                                        institute.CityName = dr[17].ToString();
                                        if (!String.IsNullOrEmpty(dr[18].ToString()))
                                            institute.PinCode = Convert.ToInt32(dr[18].ToString());
                                        //institute. = Convert.ToInt32(dr[19].ToString()); 
										//Added 24 April 2019
                                        institute.cityTypeID = Convert.ToInt32( dr[24].ToString());
                                        //
                                        context.Institutes.Add(institute);
                                        context.SaveChanges();
                                        instCnt++;
					ShowAlert("x");
                                    }
                                   
                                    //Check Details
                                    string accNumber = dr[21].ToString();
                                    int detCount = context.AccreditationDetails.Where(a => a.InstituteID == instituteID && a.AccreditationNumber == accNumber && a.CourseCategoryID == courseCategoryID && a.CourseID == courseID).Count();
                                    if (detCount <= 0)
                                    {

                                        //Added 25 Nov 2019 for DVP course
                                        if (ddlCourseCategory.SelectedIndex == 2 && (ddlCourse.SelectedItem.Text == "Digital Village Project for BCC (DVP-BCC)" || ddlCourse.SelectedItem.Text == "Digital Village Project for CCC (DVP-CCC)"))
                                        {
                                            var DlcCourse = (from p in context.Courses
                                                             where ((p.CourseCategory.Code == "ITL" || p.CourseCategory.Code == "DLC")
                                                                 //Added
                                                             && p.IsActive
                                                                 //Added 25 Nov 2019 - Not to accrediate for DVP courses
                                                             && (p.ID != 5 && p.ID != 7))
                                                             select new { ID = p.ID, CourseCategoryID = p.CourseCategoryID, Code = p.Code });
                                            //DlcCourse = DlcCourse.Where(p => p.Code != "MoPR-BCC");


                                            EConnect.NIELIT.AccreditationDetail DlcinstDetail;
                                            foreach (var crs in DlcCourse)
                                            {
                                                DlcinstDetail = new EConnect.NIELIT.AccreditationDetail();
                                                if (!String.IsNullOrEmpty(dr[20].ToString()))
                                                    DlcinstDetail.AccreditationStatusID = Convert.ToInt32(dr[20].ToString());
                                                DlcinstDetail.AccreditationNumber = dr[21].ToString();
                                                DlcinstDetail.CourseID = crs.ID;
                                                DlcinstDetail.CourseCategoryID = crs.CourseCategoryID;
                                                DlcinstDetail.InstituteID = instituteID;
                                                if (!String.IsNullOrEmpty(dr[22].ToString()))
                                                    DlcinstDetail.EffectiveFromDate = Convert.ToDateTime(dr[22].ToString());
                                                if (!String.IsNullOrEmpty(dr[23].ToString()))
                                                    DlcinstDetail.EffectiveToDate = Convert.ToDateTime(dr[23].ToString());
                                                context.AccreditationDetails.Add(DlcinstDetail);
                                            }
                                            context.SaveChanges();
                                        }
                                        else
                                        {


                                            if (ddlCourseCategory.SelectedIndex == 2)
                                            {
                                                EConnect.NIELIT.AccreditationDetail instDetail = new EConnect.NIELIT.AccreditationDetail();
                                                if (!String.IsNullOrEmpty(dr[20].ToString()))
                                                    instDetail.AccreditationStatusID = Convert.ToInt32(dr[20].ToString());
                                                instDetail.AccreditationNumber = dr[21].ToString();
                                                instDetail.CourseID = courseID;
                                                instDetail.CourseCategoryID = courseCategoryID;
                                                instDetail.InstituteID = instituteID;
                                                if (!String.IsNullOrEmpty(dr[22].ToString()))
                                                    instDetail.EffectiveFromDate = Convert.ToDateTime(dr[22].ToString());
                                                if (!String.IsNullOrEmpty(dr[23].ToString()))
                                                    instDetail.EffectiveToDate = Convert.ToDateTime(dr[23].ToString());
                                                context.AccreditationDetails.Add(instDetail);
                                                context.SaveChanges();

                                                if (ddlCourseCategory.SelectedIndex == 2 && ddlCourse.SelectedIndex == 2)
                                                {
                                                    instDetail = new EConnect.NIELIT.AccreditationDetail();
                                                    if (!String.IsNullOrEmpty(dr[20].ToString()))
                                                        instDetail.AccreditationStatusID = Convert.ToInt32(dr[20].ToString());
                                                    instDetail.AccreditationNumber = dr[21].ToString();
                                                    instDetail.CourseID = Convert.ToInt32(ddlCourse.Items[1].Value);
                                                    instDetail.CourseCategoryID = courseCategoryID;
                                                    instDetail.InstituteID = instituteID;
                                                    if (!String.IsNullOrEmpty(dr[22].ToString()))
                                                        instDetail.EffectiveFromDate = Convert.ToDateTime(dr[22].ToString());
                                                    if (!String.IsNullOrEmpty(dr[23].ToString()))
                                                        instDetail.EffectiveToDate = Convert.ToDateTime(dr[23].ToString());
                                                    context.AccreditationDetails.Add(instDetail);
                                                    context.SaveChanges();
                                                }

                                            }
                                            //deep add code on 19 nov 2018
                                            else
                                            {
                                                if (ddlCourseCategory.SelectedIndex == 6)
                                                {
                                                    EConnect.NIELIT.AccreditationDetail instDetail = new EConnect.NIELIT.AccreditationDetail();
                                                    if (!String.IsNullOrEmpty(dr[20].ToString()))
                                                        instDetail.AccreditationStatusID = Convert.ToInt32(dr[20].ToString());
                                                    instDetail.AccreditationNumber = dr[21].ToString();
                                                    instDetail.CourseID = courseID;
                                                    instDetail.CourseCategoryID = courseCategoryID;
                                                    instDetail.InstituteID = instituteID;
                                                    if (!String.IsNullOrEmpty(dr[22].ToString()))
                                                        instDetail.EffectiveFromDate = Convert.ToDateTime(dr[22].ToString());
                                                    if (!String.IsNullOrEmpty(dr[23].ToString()))
                                                        instDetail.EffectiveToDate = Convert.ToDateTime(dr[23].ToString());
                                                    context.AccreditationDetails.Add(instDetail);
                                                    context.SaveChanges();
                                                }

                                            }
                                            //deep end add code on 19 nov 2018

                                            if (ddlCourseCategory.SelectedItem.Text == "Digital Literacy Course")
                                            {
                                                var DlcCourse = (from p in context.Courses
                                                                 where ((p.CourseCategory.Code == "ITL" || p.CourseCategory.Code == "DLC")
                                                                     //Added 21 Jun2019
                                                                 && p.IsActive
                                                                   //Added 25 Nov 2019 - Not to accrediate for DVP courses
                                                                 && !p.Code.StartsWith("DVP"))
                                                                 select new { ID = p.ID, CourseCategoryID = p.CourseCategoryID, Code = p.Code });

                                                //Commented 21 june2019
                                                //DlcCourse = DlcCourse.Where(p => p.Code != "MoPR-BCC");


                                                EConnect.NIELIT.AccreditationDetail DlcinstDetail;
                                                foreach (var crs in DlcCourse)
                                                {
                                                    DlcinstDetail = new EConnect.NIELIT.AccreditationDetail();
                                                    if (!String.IsNullOrEmpty(dr[20].ToString()))
                                                        DlcinstDetail.AccreditationStatusID = Convert.ToInt32(dr[20].ToString());
                                                    DlcinstDetail.AccreditationNumber = dr[21].ToString();
                                                    DlcinstDetail.CourseID = crs.ID;
                                                    DlcinstDetail.CourseCategoryID = crs.CourseCategoryID;
                                                    DlcinstDetail.InstituteID = instituteID;
                                                    if (!String.IsNullOrEmpty(dr[22].ToString()))
                                                        DlcinstDetail.EffectiveFromDate = Convert.ToDateTime(dr[22].ToString());
                                                    if (!String.IsNullOrEmpty(dr[23].ToString()))
                                                        DlcinstDetail.EffectiveToDate = Convert.ToDateTime(dr[23].ToString());
                                                    context.AccreditationDetails.Add(DlcinstDetail);
                                                }
                                                context.SaveChanges();
                                            }
                                            acrrNew++;

                                        }


                                        //Create User
                                        if (context.Users.Where(a => a.UserTypeID == userType && a.UserRefNumber == instituteID).Count() <= 0)
                                        {
                                            User user = new User();
                                            user.OrganizationID = 1;
                                            if (ddlCourseCategory.SelectedIndex == 6)
                                            {
                                                user.LoginID = accNumber.Substring(0, 9);
                                            }
                                            else
                                            {
                                                user.LoginID = accNumber;
                                            }
                                            //user.LoginID = accNumber;
                                            user.UserName = dr[1].ToString();
                                            user.Password =UserManager.ComputeSha256Hash(accNumber).ToUpper();//FormsAuthentication.HashPasswordForStoringInConfigFile(accNumber, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
                                            user.UserTypeID = userType;
                                            user.UserRefNumber = instituteID;
                                            user.EmailID = institute.EmailAddress1;
                                            user.PasswordExpiryDays = 0;
                                            user.LastPasswordChangedOn = DateTime.Now;
                                            user.FailedLoginAttempts = 0;
                                            user.CreatedBy = Convert.ToInt32(Session["UserID"]);
                                            user.CreatedOn = DateTime.Now;
                                            user.HasLoginAccess = true;
                                            user.DefaultRoleID = Convert.ToInt32(enmRole.AdminInstitute);
                                            context.Users.Add(user);
                                            context.SaveChanges();
                                            userCnt++;
                                        }
                                        // scope.Complete();
                                        //Extra bracket for DVP
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
				ShowAlert(ex.Message);
                        }
                    };

                };
            }
            //   ShowAlert(inscnt.ToString() + " records updated");
            lblCount.Text = "<b>Total uploaded records:</b> " + totCount + "</br> <b>Institutes added:</b>" + instCnt + "</br><b>New Accreditations added:</b>" + acrrNew + "</br> <b>Users Created:</b>" + userCnt + "</br> <i>The unsaved records may be due to data discrepancies or duplicacy.</i>";

            ddlCourseCategory.SelectedIndex = ddlCourse.SelectedIndex = 0; fuInstitituteList.Dispose();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally
        {
            dr.Close();
            dr.Dispose();
            command.Dispose();
            connection.Close();
            connection.Dispose();
            System.IO.File.Delete(path);
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ddlCourseCategory.SelectedIndex = ddlCourse.SelectedIndex = 0; fuInstitituteList.Dispose();
    }
}