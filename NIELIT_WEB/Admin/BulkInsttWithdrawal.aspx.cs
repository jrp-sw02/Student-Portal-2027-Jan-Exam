using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using System.Web.Security;
using System.Data.OleDb;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;

using System.Configuration;

public partial class BulkInsttWithdrawal : BasePage
{
    String strMessage = string.Empty;
    enmLanguage pageLanguage = enmLanguage.English;
    protected string fname = "";
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
                // BindActivityGroup();
                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {

                }
                else
                {
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Withdrawal Institutes List Upload", "Admin/BilkInsttWithdrawal.aspx", ""));
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
    //protected void BindActivityGroup()
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            ListItem lst = new ListItem("--Select One--", "0");
    //            var coursecategory = from s in context.CourseCategories
    //                                 select new { ValueField = s.ID, TextField = s.Name };
    //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategory, coursecategory, lst);
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //[System.Web.Services.WebMethod(EnableSession = true)]

    //protected void ddlCourseCategory_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            Int32 catg = Convert.ToInt32(ddlCourseCategory.SelectedItem.Value);
    //            ListItem lst = new ListItem("--Select One--", "0");
    //            var course = from s in context.Courses
    //                         where s.CourseCategoryID == catg
    //                             //Added 17 Jan 2019
    //                         && s.ShowOnWeb
    //                         select new { ValueField = s.ID, TextField = s.Name + " (" + s.Code + ")" };
    //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, course, lst);
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        //if (ddlCourse.SelectedIndex == 0 || ddlCourseCategory.SelectedIndex == 0)
        //{
        //    ShowAlert("Please select relevant Course Category and Course");
        //    return;
        //}
        if (!fuInstitituteList.HasFile)
        {
            ShowAlert("Please select file to upload.");
            return;
        }
        string filepath = Server.MapPath("../UploadedFiles/DLCWithdrawal/");
        //string SavePath = filepath + "\\" + System.IO.Path.GetFileNameWithoutExtension(fuInstitituteList.FileName) + "_" + System.DateTime.Today.ToString("dd-MMM-yyyy") + System.IO.Path.GetExtension(fuInstitituteList.FileName);
        fuInstitituteList.SaveAs(filepath + System.IO.Path.GetFileNameWithoutExtension(fuInstitituteList.FileName) + "_" + System.DateTime.Today.ToString("dd-MMM-yyyy") + System.IO.Path.GetExtension(fuInstitituteList.FileName));
        string path = filepath + "\\" + System.IO.Path.GetFileNameWithoutExtension(fuInstitituteList.FileName) + "_" + System.DateTime.Today.ToString("dd-MMM-yyyy") + System.IO.Path.GetExtension(fuInstitituteList.FileName);
         fname = System.IO.Path.GetFileNameWithoutExtension(fuInstitituteList.FileName) + "_" + System.DateTime.Today.ToString("dd-MMM-yyyy") + System.IO.Path.GetExtension(fuInstitituteList.FileName);
         hfFileName.Value = fname;
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
        OleDbCommand command = new OleDbCommand("select * from [InstituteWithdrawal$]", connection);

        OleDbDataReader dr = command.ExecuteReader();
        try
        {
            int instCnt = 0;
            int instNotWith = 0;
            int userCnt = 0;
            int totCount = 0;
            int userType = Convert.ToInt32(EConnect.URM.UserType.Institute);
            EConnect.NIELIT.Institute institute;
            EConnect.NIELIT.InstituteWithdrawal InsttWithdrawal;


            while (dr.Read())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        //Adding record to institutewithdrawal table
                        try
                        {
                            if (dr[1] != null)
                            {

                                if (IsNumeric(dr[1].ToString()))
                                {

                                    Int64 instituteAccrID = Convert.ToInt64(dr[1].ToString());
                                    var insttAccreditation = (from a in context.AccreditationDetails
                                                              where a.AccreditationNumber.Equals(instituteAccrID.ToString().Trim())
                                                              //&& a.AccreditationStatusID.ToString() != "5"
                                                              //&& a.AccreditationStatusID.ToString() != "6"
                                                              //&& a.AccreditationStatusID.ToString() != "7"
                                                              //&& a.AccreditationStatusID.ToString() != "8"
                                                              select a.InstituteID).FirstOrDefault();
                                    // institute = context.Institutes.Find(instituteID);

                                    if (insttAccreditation != null)
                                    {
                                        institute = context.Institutes.Find(Convert.ToInt32(insttAccreditation));

                                        Int32 srNo = 0;
                                        if (dr[0] != null && IsNumeric(dr[0].ToString()))
                                            srNo = Convert.ToInt32(dr[0].ToString());


                                        if (institute != null)
                                        {
                                            InsttWithdrawal = new EConnect.NIELIT.InstituteWithdrawal();
                                            InsttWithdrawal.SrNO = srNo;
                                            // institute = new EConnect.NIELIT.Institute();
                                            //Create New Institute
                                            InsttWithdrawal.ccc_no = dr[1].ToString();
                                            InsttWithdrawal.Name = dr[2].ToString();
                                            InsttWithdrawal.Address = dr[3].ToString();
                                            InsttWithdrawal.CityName = dr[4].ToString();
                                            InsttWithdrawal.StateName = dr[5].ToString();
                                            if (!String.IsNullOrEmpty(dr[6].ToString()))
                                                InsttWithdrawal.PinCode = Convert.ToInt32(dr[6].ToString());
                                            if (!String.IsNullOrEmpty(dr[7].ToString()))
                                            {
                                                //DateTime cannot be future date
                                                if (Convert.ToDateTime(dr[7].ToString()) > System.DateTime.Today)
                                                {
                                                    InsttWithdrawal.withdrawalDate = Convert.ToDateTime(dr[7].ToString());
                                                    InsttWithdrawal.uploadStatus = "Uploaded,not withdrawn";
                                                    InsttWithdrawal.Remarks = "Error!!Date cannot be future date";
                                                    InsttWithdrawal.uploadFileName = fname;
                                                    InsttWithdrawal.enterByID = Convert.ToInt32(Session["UserID"]);
                                                    InsttWithdrawal.enterDate = System.DateTime.Now;
                                                    instNotWith = instNotWith + 1;
                                                    totCount++;

                                                    context.insttWithdrawal.Add(InsttWithdrawal);
                                                    context.SaveChanges();
                                                    scope.Complete();
                                                    continue;
                                                }
                                                else
                                                    InsttWithdrawal.withdrawalDate = Convert.ToDateTime(dr[7].ToString());
                                            }
                                            InsttWithdrawal.uploadFileName = fname;
                                            InsttWithdrawal.enterByID = Convert.ToInt32(Session["UserID"]);
                                            InsttWithdrawal.enterDate = System.DateTime.Now;

                                            //Check if to be withdrwan or not
                                            if (institute.Name.Trim().ToUpper() == InsttWithdrawal.Name.Trim().ToUpper() && institute.State.Name.Trim().ToUpper() == InsttWithdrawal.StateName.Trim().ToUpper() && institute.PinCode == InsttWithdrawal.PinCode)
                                            {
                                                if (InsttWithdrawal.uploadStatus == null)
                                                {
                                                    int cnt = (from a in context.AccreditationDetails
                                                               where a.AccreditationNumber.Equals(InsttWithdrawal.ccc_no)
                                                               && a.AccreditationStatusID.ToString() != "5"
                                                                && a.AccreditationStatusID.ToString() != "6"
                                                                  && a.AccreditationStatusID.ToString() != "7"
                                                                  && a.AccreditationStatusID.ToString() != "8"
                                                               select a).Count();

                                                    if (cnt > 0)
                                                    {
                                                        var insttAccr = from a in context.AccreditationDetails
                                                                        where a.AccreditationNumber.Equals(InsttWithdrawal.ccc_no)
                                                                        && a.AccreditationStatusID.ToString() != "5"
                                                                         && a.AccreditationStatusID.ToString() != "6"
                                                                  && a.AccreditationStatusID.ToString() != "7"
                                                                  && a.AccreditationStatusID.ToString() != "8"
                                                                        select a;
                                                        foreach (var insttAccrwithdraw in insttAccr)
                                                        {
                                                            insttAccrwithdraw.AccreditationStatusID = 5;
                                                            if (!String.IsNullOrEmpty(dr[7].ToString()))
                                                                insttAccrwithdraw.WithdrawlDate = Convert.ToDateTime(dr[7].ToString());
                                                        }
                                                        context.SaveChanges();
                                                        instCnt++;
                                                        InsttWithdrawal.uploadStatus = "Withdrawn";
                                                    }
                                                    else
                                                    {

                                                        InsttWithdrawal.uploadStatus = "Uploaded,not withdrawn";
                                                        InsttWithdrawal.Remarks = "No Instt accreditation available for withdrawal or already withdrawn ";
                                                        instNotWith = instNotWith + 1;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                InsttWithdrawal.uploadStatus = "Uploaded,not withdrawn";
                                                InsttWithdrawal.Remarks = "Mismatch in Name , State Name or PinCode";
                                                instNotWith = instNotWith + 1;
                                            }

                                            context.insttWithdrawal.Add(InsttWithdrawal);

                                            context.SaveChanges();
                                            totCount++;


                                        }

                                        ////Check Details
                                        //string accNumber = dr[21].ToString();
                                        //int detCount = context.AccreditationDetails.Where(a => a.InstituteID == instituteID && a.AccreditationNumber == accNumber && a.CourseCategoryID == courseCategoryID && a.CourseID == courseID).Count();
                                        //if (detCount <= 0)
                                        //{

                                        //    //Added 25 Nov 2019 for DVP course
                                        //    if (ddlCourseCategory.SelectedIndex == 2 && (ddlCourse.SelectedItem.Text == "Digital Village Project for BCC (DVP-BCC)" || ddlCourse.SelectedItem.Text == "Digital Village Project for CCC (DVP-CCC)"))
                                        //    {
                                        //        var DlcCourse = (from p in context.Courses
                                        //                         where ((p.CourseCategory.Code == "ITL" || p.CourseCategory.Code == "DLC")
                                        //                             //Added
                                        //                         && p.IsActive
                                        //                             //Added 25 Nov 2019 - Not to accrediate for DVP courses
                                        //                         && (p.ID != 5 && p.ID != 7))
                                        //                         select new { ID = p.ID, CourseCategoryID = p.CourseCategoryID, Code = p.Code });
                                        //        //DlcCourse = DlcCourse.Where(p => p.Code != "MoPR-BCC");


                                        //        EConnect.NIELIT.AccreditationDetail DlcinstDetail;
                                        //        foreach (var crs in DlcCourse)
                                        //        {
                                        //            DlcinstDetail = new EConnect.NIELIT.AccreditationDetail();
                                        //            if (!String.IsNullOrEmpty(dr[20].ToString()))
                                        //                DlcinstDetail.AccreditationStatusID = Convert.ToInt32(dr[20].ToString());
                                        //            DlcinstDetail.AccreditationNumber = dr[21].ToString();
                                        //            DlcinstDetail.CourseID = crs.ID;
                                        //            DlcinstDetail.CourseCategoryID = crs.CourseCategoryID;
                                        //            DlcinstDetail.InstituteID = instituteID;
                                        //            if (!String.IsNullOrEmpty(dr[22].ToString()))
                                        //                DlcinstDetail.EffectiveFromDate = Convert.ToDateTime(dr[22].ToString());
                                        //            if (!String.IsNullOrEmpty(dr[23].ToString()))
                                        //                DlcinstDetail.EffectiveToDate = Convert.ToDateTime(dr[23].ToString());
                                        //            context.AccreditationDetails.Add(DlcinstDetail);
                                        //        }
                                        //        context.SaveChanges();
                                        //    }
                                        //    else
                                        //    {
                                        //        if (ddlCourseCategory.SelectedIndex == 2)
                                        //        {
                                        //            ////

                                        //            EConnect.NIELIT.AccreditationDetail instDetail = new EConnect.NIELIT.AccreditationDetail();
                                        //            if (!String.IsNullOrEmpty(dr[20].ToString()))
                                        //                instDetail.AccreditationStatusID = Convert.ToInt32(dr[20].ToString());
                                        //            instDetail.AccreditationNumber = dr[21].ToString();
                                        //            instDetail.CourseID = courseID;
                                        //            instDetail.CourseCategoryID = courseCategoryID;
                                        //            instDetail.InstituteID = instituteID;
                                        //            if (!String.IsNullOrEmpty(dr[22].ToString()))
                                        //                instDetail.EffectiveFromDate = Convert.ToDateTime(dr[22].ToString());
                                        //            if (!String.IsNullOrEmpty(dr[23].ToString()))
                                        //                instDetail.EffectiveToDate = Convert.ToDateTime(dr[23].ToString());
                                        //            context.AccreditationDetails.Add(instDetail);
                                        //            context.SaveChanges();

                                        //            if (ddlCourseCategory.SelectedIndex == 2 && ddlCourse.SelectedIndex == 2)
                                        //            {
                                        //                instDetail = new EConnect.NIELIT.AccreditationDetail();
                                        //                if (!String.IsNullOrEmpty(dr[20].ToString()))
                                        //                    instDetail.AccreditationStatusID = Convert.ToInt32(dr[20].ToString());
                                        //                instDetail.AccreditationNumber = dr[21].ToString();
                                        //                instDetail.CourseID = Convert.ToInt32(ddlCourse.Items[1].Value);
                                        //                instDetail.CourseCategoryID = courseCategoryID;
                                        //                instDetail.InstituteID = instituteID;
                                        //                if (!String.IsNullOrEmpty(dr[22].ToString()))
                                        //                    instDetail.EffectiveFromDate = Convert.ToDateTime(dr[22].ToString());
                                        //                if (!String.IsNullOrEmpty(dr[23].ToString()))
                                        //                    instDetail.EffectiveToDate = Convert.ToDateTime(dr[23].ToString());
                                        //                context.AccreditationDetails.Add(instDetail);
                                        //                context.SaveChanges();
                                        //            }

                                        //        }
                                        //        //deep add code on 19 nov 2018
                                        //        else
                                        //        {
                                        //            if (ddlCourseCategory.SelectedIndex == 6)
                                        //            {
                                        //                EConnect.NIELIT.AccreditationDetail instDetail = new EConnect.NIELIT.AccreditationDetail();
                                        //                if (!String.IsNullOrEmpty(dr[20].ToString()))
                                        //                    instDetail.AccreditationStatusID = Convert.ToInt32(dr[20].ToString());
                                        //                instDetail.AccreditationNumber = dr[21].ToString();
                                        //                instDetail.CourseID = courseID;
                                        //                instDetail.CourseCategoryID = courseCategoryID;
                                        //                instDetail.InstituteID = instituteID;
                                        //                if (!String.IsNullOrEmpty(dr[22].ToString()))
                                        //                    instDetail.EffectiveFromDate = Convert.ToDateTime(dr[22].ToString());
                                        //                if (!String.IsNullOrEmpty(dr[23].ToString()))
                                        //                    instDetail.EffectiveToDate = Convert.ToDateTime(dr[23].ToString());
                                        //                context.AccreditationDetails.Add(instDetail);
                                        //                context.SaveChanges();
                                        //            }

                                        //        }
                                        //        //deep end add code on 19 nov 2018

                                        //        if (ddlCourseCategory.SelectedItem.Text == "Digital Literacy Course")
                                        //        {
                                        //            var DlcCourse = (from p in context.Courses
                                        //                             where ((p.CourseCategory.Code == "ITL" || p.CourseCategory.Code == "DLC")
                                        //                                 //Added
                                        //                             && p.IsActive
                                        //                                 //Added 25 Nov 2019 - Not to accrediate for DVP courses
                                        //                             && !p.Code.StartsWith("DVP"))
                                        //                             select new { ID = p.ID, CourseCategoryID = p.CourseCategoryID, Code = p.Code });
                                        //            //DlcCourse = DlcCourse.Where(p => p.Code != "MoPR-BCC");


                                        //            EConnect.NIELIT.AccreditationDetail DlcinstDetail;
                                        //            foreach (var crs in DlcCourse)
                                        //            {
                                        //                DlcinstDetail = new EConnect.NIELIT.AccreditationDetail();
                                        //                if (!String.IsNullOrEmpty(dr[20].ToString()))
                                        //                    DlcinstDetail.AccreditationStatusID = Convert.ToInt32(dr[20].ToString());
                                        //                DlcinstDetail.AccreditationNumber = dr[21].ToString();
                                        //                DlcinstDetail.CourseID = crs.ID;
                                        //                DlcinstDetail.CourseCategoryID = crs.CourseCategoryID;
                                        //                DlcinstDetail.InstituteID = instituteID;
                                        //                if (!String.IsNullOrEmpty(dr[22].ToString()))
                                        //                    DlcinstDetail.EffectiveFromDate = Convert.ToDateTime(dr[22].ToString());
                                        //                if (!String.IsNullOrEmpty(dr[23].ToString()))
                                        //                    DlcinstDetail.EffectiveToDate = Convert.ToDateTime(dr[23].ToString());
                                        //                context.AccreditationDetails.Add(DlcinstDetail);
                                        //            }
                                        //            context.SaveChanges();
                                        //        }
                                        //        acrrNew++;

                                        //    }


                                        //    //Create User
                                        //    if (context.Users.Where(a => a.UserTypeID == userType && a.UserRefNumber == instituteID).Count() <= 0)
                                        //    {
                                        //        User user = new User();
                                        //        user.OrganizationID = 1;
                                        //        if (ddlCourseCategory.SelectedIndex == 6)
                                        //        {
                                        //            user.LoginID = accNumber.Substring(0, 9);
                                        //        }
                                        //        else
                                        //        {
                                        //            user.LoginID = accNumber;
                                        //        }
                                        //        //user.LoginID = accNumber;
                                        //        user.UserName = dr[1].ToString();
                                        //        user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(accNumber, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());
                                        //        user.UserTypeID = userType;
                                        //        user.UserRefNumber = instituteID;
                                        //        user.EmailID = institute.EmailAddress1;
                                        //        user.PasswordExpiryDays = 0;
                                        //        user.LastPasswordChangedOn = DateTime.Now;
                                        //        user.FailedLoginAttempts = 0;
                                        //        user.CreatedBy = Convert.ToInt32(Session["UserID"]);
                                        //        user.CreatedOn = DateTime.Now;
                                        //        user.HasLoginAccess = true;
                                        //        user.DefaultRoleID = Convert.ToInt32(enmRole.AdminInstitute);
                                        //        context.Users.Add(user);
                                        //        context.SaveChanges();
                                        //        userCnt++;
                                    }


                                }

                            }
                           
                        }
                        //}
                        // }
                        // }
                        catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                        {
                            Exception raise = dbEx;
                            foreach (var validationErrors in dbEx.EntityValidationErrors)
                            {
                                foreach (var validationError in validationErrors.ValidationErrors)
                                {
                                    string message = string.Format("{0}:{1}",
                                        validationErrors.Entry.Entity.ToString(),
                                        validationError.ErrorMessage);
                                    // raise a new exception nesting  
                                    // the current instance as InnerException  
                                    raise = new InvalidOperationException(message, raise);
                                }
                            }
                            throw raise;
                        }
                    };
                    scope.Complete();
                };

            }
            //   ShowAlert(inscnt.ToString() + " records updated");
            lblCount.Text = "<b>Total uploaded institutes:</b> " + totCount + "</br> <b>Institutes Withdrawn:</b>" + instCnt + "</br>";
            PagingBar1.CurrentPageIndex = 0;
            BindGridView();
           

            ///    ddlCourseCategory.SelectedIndex = ddlCourse.SelectedIndex = 0; fuInstitituteList.Dispose();
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
    protected void BindGridView()
    {
        DataSet ds = new DataSet();
        ds = FillGridWithData(hfFileName.Value );
        // gvMain.DataSource = ds;
       //  gvMain.DataBind();
        DataTable dt = ds.Tables[0];
        if (dt.Rows.Count > 0)
        {
            PagingBar1.Bind(dt, ref gvMain);
            uPnlGrid.Update();
            uPnlNavigation.Update();

           // gvMain.DataSource = ds;
           // gvMain.DataBind();
        }
        else
        {
            ShowAlert("No upload failure");
            return;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //ddlCourseCategory.SelectedIndex = ddlCourse.SelectedIndex = 0; 
        fuInstitituteList.Dispose();
    }
    protected DataSet FillGridWithData(string fileName)
    {
       
        SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString);
        EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
        cnn.Open();
        
      
        DataSet ds = new DataSet();
        try
        {
            SqlCommand cmd = new SqlCommand("ShowBulkWithdrawalDetails", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@pFileName", SqlDbType.VarChar , 500);
            cmd.Parameters["@pFileName"].Value = fileName;
            SqlDataAdapter Sda = new SqlDataAdapter(cmd);
            Sda.Fill(ds);
           
           
        }

        catch (Exception ex)
        {
            ShowAlert( ex.Message);
            
        }
        finally
        {
            // context.Dispose();
            cnn.Close();
        }
        return ds;
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
}