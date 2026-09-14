using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data.Objects;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using EConnect;
using EConnect.Utils.Common;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

public partial class NielitCentreCourseWiseStudentsFormalFilter : BasePage
{
    UserType loginUserType;
    Int32 currentRoleId = 0;
           Int32 loginUserNo = 0;
           Int64 entityID = 0;
          Int32 UserTypeId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Lblerror.Text = "";
        Lblerror.Visible = false;
        try
        {
            if (IsSessionAlive() == false)
                //Response.Redirect("../Index.aspx");
                Response.Redirect("~/index.aspx");

            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            

            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                // for testing
                //Response.Write("Sorry! You don't have rights  to view this page");
                //Response.End();
            }

            loginUserNo = Convert.ToInt32(Session["UserID"]);
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
            loginUserType = (UserType)Session["UserType"];

            if (!Page.IsPostBack)
                {
                FillCentre();
                FillCourseCategory();
                FillCourseName();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NIELIT Center CourseWise Students Details", "#", ""));
                }

            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message;
            Lblerror.Visible = true;
        }
    }

    #region vCode
    protected void btnReset_Click(object sender, EventArgs e)
        {
        try
            {
            Response.Redirect("~/HO/Rpt/NielitCentreCourseWiseStudentsFormalFilter.aspx");
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }
        
    private string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    
    protected void FillCentre()
    {
        try
        {
        using (NIELITMISContext context = new NIELITMISContext())
            {
            if (UserTypeId == 6)  // ho user
                {
                ListItem lst1 = new ListItem("--Select One--", "0");
                var centreName1 = from s in context.NielitCentres
                                  //where s.ID == entityID
                                  select new { ValueField = s.ID, TextField = s.Name };
                if (centreName1 != null)
                    {
                    var centreName = centreName1.OrderBy(i => i.ValueField);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentreName, centreName.Distinct(), lst1);
                    ddlCentreName.Enabled = true;
                    }
                }
            else //if (UserTypeId == 10)  // centre user
                {
                ListItem lst1 = new ListItem("--Select One--", "0");
                var centreName = from s in context.NielitCentres
                                 where s.ID == entityID
                                 select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCentreName, centreName.Distinct(), lst1);
                ddlCentreName.SelectedValue = Convert.ToInt32(entityID).ToString();
                ddlCentreName.Enabled = false;
                }
            }
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

                    ListItem lst1 = new ListItem("--Select One--", "0");

                    var cCat = from c in context.NielitCentreCourseCategorys
                               where c.ID==101//"Formal Course"
                               select new { ValueField = c.ID, TextField = c.Name };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCat, cCat, lst1);
                    ddlCourseCat.SelectedValue = "101";
                    ddlCourseCat.Enabled = false;
                }
            //using (DataTable dt = FillCourseCategoryNIELITMISCourseCatNielitCourseCatRecord())
            //    {
            //    if (dt.Rows.Count > 0)
            //        {
            //        ddlCourseCat.DataSource = dt;
            //        ddlCourseCat.DataTextField = "Name";
            //        ddlCourseCat.DataValueField = "ID";
            //        ddlCourseCat.DataBind();
            //        ddlCourseCat.Items.Insert(0, new ListItem("--Select One--", "0"));
            //        }
            //    }

            }
        catch (Exception ex)
            {
            throw ex;
            }
        }
    protected void FillCourseName()
    {
        try
        {
            ddlCourseName.Items.Clear();
            Int32 nielitcentreID = 0;
             nielitcentreID = Convert.ToInt32(ddlCentreName.SelectedValue);

            Int32 coursecatID = Convert.ToInt32(ddlCourseCat.SelectedValue);
            if (coursecatID != 0 && nielitcentreID == 0)
            {
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    ListItem lst1 = new ListItem("--Select One--", "0");
                    var courseName = from c in context.NielitCentreCourses
                                     join d in context.NielitCourseDurations on c.ID equals d.courseID
                                     join b in context.NielitCentreBatchs on d.ID equals b.CourseDurationID
                                     join s in context.SemesterMaster on b.ID equals s.BatchId //b.CourseDurationID equals s.CourseId
                                     join sd in context.SemesterDetail on b.ID equals sd.batchID
                                     where c.ID == d.courseID && d.ID == b.CourseDurationID && b.CourseDurationID == s.CourseId
                                     && s.CourseId == sd.CourseDurationID && b.ID == s.BatchId && s.BatchId == sd.batchID
                                     && c.CourseCategoryID == coursecatID

                                     select new { ValueField = d.ID, TextField = c.Name + "( "+ d.courseDurationYears+" years " + d.courseDurationDays +" days)" };
                    if (courseName != null)
                    {
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courseName.Distinct(), lst1);
                    }
                }
            }
            else
            {
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    ListItem lst1 = new ListItem("--Select One--", "0");
                    var courseName = from c in context.NielitCentreCourses
                                     join d in context.NielitCourseDurations on c.ID equals d.courseID
                                     join b in context.NielitCentreBatchs on d.ID equals b.CourseDurationID
                                     join s in context.SemesterMaster on b.ID equals s.BatchId //b.CourseDurationID equals s.CourseId
                                     join sd in context.SemesterDetail on b.ID equals sd.batchID
                                     where c.ID == d.courseID && d.ID == b.CourseDurationID && b.CourseDurationID == s.CourseId
                                     && s.CourseId == sd.CourseDurationID && b.ID == s.BatchId && s.BatchId == sd.batchID
                                     && c.CourseCategoryID == coursecatID && b.centreID == nielitcentreID

                                     select new { ValueField = d.ID, TextField = c.Name + "( " + d.courseDurationYears + " years " + d.courseDurationDays + " days)" };
                    if (courseName != null)
                    {
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courseName.Distinct(), lst1);
                    }
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillBatchName()
    {
        try
        {
            Int32 coursecatID = Convert.ToInt32(ddlCourseCat.SelectedValue);
            Int32 courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
            Int32 nielitcentreID = Convert.ToInt32(ddlCentreName.SelectedValue);
            if (coursecatID != 0 && courseID != 0)
            {
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    ListItem lst = new ListItem("-- ALL --", "0");
                    var batchName = from c in context.NielitCentreCourses
                                    join cc in context.NielitCentreCourseCategorys on c.CourseCategoryID equals cc.ID
                                    join d in context.NielitCourseDurations on c.ID equals d.courseID
                                    join b in context.NielitCentreBatchs on d.ID equals b.CourseDurationID
                                    join n in context.NielitCentres on b.centreID equals n.ID
                                   join s in context.SemesterMaster on b.ID equals s.BatchId
                                    join sd in context.SemesterDetail on b.ID equals sd.batchID
                                    where c.ID == d.courseID && d.ID == b.CourseDurationID && b.CourseDurationID == s.CourseId && s.CourseId == sd.CourseDurationID
                                    && b.ID == s.BatchId && s.BatchId == sd.batchID
                                    && cc.ID == coursecatID && d.ID == courseID && n.ID == nielitcentreID  
                                    orderby (b.Name) 
                                    select new { ValueField = b.ID, TextField = b.Name };
                    if (batchName != null)
                    {
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatch, batchName.Distinct(), lst);
                    }
                };
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillSemesterNo()
    {
        try
        {
            ddlSemesterNo.Items.Clear();
            Int32 coursecatID = Convert.ToInt32(ddlCourseCat.SelectedValue);
            Int32 courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
            Int32 batchID = Convert.ToInt32(ddlBatch.SelectedValue);
            Int32 nielitcentreID = Convert.ToInt32(ddlCentreName.SelectedValue);

            if (coursecatID != 0 && courseID != 0 && batchID != 0)
            {
                using (NIELITMISContext context = new NIELITMISContext())
                {
                    ListItem lst = new ListItem("-- ALL --", "0");
                    var semesterNo = from c in context.NielitCentreCourses
                                     join cc in context.NielitCentreCourseCategorys on c.CourseCategoryID equals cc.ID
                                     join d in context.NielitCourseDurations on c.ID equals d.courseID
                                     join b in context.NielitCentreBatchs on d.ID equals b.CourseDurationID
                                     join n in context.NielitCentres on b.centreID equals n.ID
                                     join s in context.SemesterMaster on b.ID equals s.BatchId
                                     join sd in context.SemesterDetail on b.ID equals sd.batchID
                                     where c.ID == d.courseID && d.ID == b.CourseDurationID && b.CourseDurationID == s.CourseId && s.CourseId == sd.CourseDurationID
                                     && b.ID == s.BatchId && s.BatchId == sd.batchID
                                     && cc.ID == coursecatID && d.ID == courseID && b.ID == batchID && n.ID == nielitcentreID

                                     select new { ValueField = sd.semesterNo, TextField = sd.semesterNo };
                    if (semesterNo != null)
                    {
                        var semesterNo1 = semesterNo.ToList();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlSemesterNo, semesterNo.Distinct(), lst);
                    }
                };
            }
            else
            {
                ddlSemesterNo.Items.Insert(0, new ListItem("-- ALL --", "0"));
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable FillCourseCategoryNIELITMISCourseCatNielitCourseCatRecord()
        {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
            {
            try
                {
                using (SqlCommand cmd = new SqlCommand("GetCourseCategoryNielitMisAndNielitForMisReports", con))
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
            using (DataTable dt = FillCourseNIELITMISCourseNielitCourseRecord(pCourseCategory))
                {
                if (dt.Rows.Count > 0)
                    {
                    ddlCourseName.DataSource = dt;
                    ddlCourseName.DataTextField = "Name";
                    ddlCourseName.DataValueField = "ID";
                    ddlCourseName.DataBind();
                    ddlCourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                BreadCrumb1.Render();
                }
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }

    public DataTable FillCourseNIELITMISCourseNielitCourseRecord(int coursecatID)
        {
        //Int32 coursecatID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
            {
            try
                {
                using (SqlCommand cmd = new SqlCommand("[GetCourseNielitMisAndNielitForMisReports]", con))
                    {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@CourseCat", SqlDbType.Int));
                    cmd.Parameters["@CourseCat"].Value = coursecatID;
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
    
    protected void ddlCentreName_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillCourseName();

            ddlBatch.Items.Clear();
            ddlBatch.Items.Insert(0, new ListItem("-- ALL --", "0"));
            ddlSemesterNo.Items.Clear();
            ddlSemesterNo.Items.Insert(0, new ListItem("-- ALL --", "0"));
      
        }
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSemesterNo();
    }

    protected void ddlCourseCat_SelectedIndexChanged(object sender, EventArgs e)
        {
        ddlCourseName.Items.Clear();
        ddlCourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
        int Ccat = 0;
        Ccat = Convert.ToInt32(ddlCourseCat.SelectedValue);
        FillCourse(Ccat);
        }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillBatchName();
    }
    protected void txtDateto_TextChanged(object sender, EventArgs e)
        {
        //if (txttDateFrom.Text.Length != 0 && txtDateto.Text.Length != 0)
        //    {
        //    DateTime DateFrom = Convert.ToDateTime(txttDateFrom.Text);
        //    DateTime DateTo = Convert.ToDateTime(txtDateto.Text);
        //    DateFrom = DateFrom.AddDays(-1);
        //    DateTime EndDate = DateFrom.AddYears(1);
        //    if (DateTo > EndDate)
        //        {
        //        ShowAlert("DateFrom and DateTo range should be maximum 1 year", true);
        //        return;
        //        }
        //    }
        }
    
#endregion


    #region Old Code Button click

    //protected void btnView_Click(object sender, EventArgs e)
    //    {
    //    if (ddlCentreName.SelectedValue.ToString().Equals("0"))
    //        {
    //        ShowAlert("Select Centre Name");
    //        return;
    //        }
    //    if (ddlCourseCat.SelectedValue.ToString().Equals("0"))
    //        {
    //        ShowAlert("Select Course Category");
    //        return;
    //        }

    //    if (ddlCourseName.SelectedValue.ToString().Equals("0"))
    //        {
    //        ShowAlert("Select Course Name");
    //        return;
    //        }
    //    //
    //    if (String.IsNullOrWhiteSpace(txttDateFrom.Text))
    //    //if (txttDateFrom.Text.Length ==0)
    //        {
    //        ShowAlert("From Date can not be left blank");
    //        return;
    //        }
    //    if (!IsDate(txttDateFrom.Text))
    //        {
    //        ShowAlert("Invalid From Date");
    //        return;
    //        }

    //    if (String.IsNullOrWhiteSpace(txtDateto.Text))
    //        {
    //        ShowAlert("From Date can not be left blank");
    //        return;
    //        }
    //    if (!IsDate(txtDateto.Text))
    //        {
    //        ShowAlert("Invalid From Date");
    //        return;
    //        }
    //    //
    //    try
    //        {
    //        DateTime datefromC, datetoC, todayDate;
    //        datefromC = Convert.ToDateTime(txttDateFrom.Text.Trim());
    //        datetoC = Convert.ToDateTime(txtDateto.Text.Trim());
    //        todayDate = DateTime.Today;
    //        if (datefromC < datetoC)
    //        //if (datefromC <= datetoC) //For One/same day record
    //            {
    //            if (datetoC <= todayDate)
    //                {
    //                string startDate = HttpUtility.UrlEncode(Encrypt(txttDateFrom.Text.Trim()));
    //                string endDate = HttpUtility.UrlEncode(Encrypt(txtDateto.Text.Trim()));
    //                string centreID = HttpUtility.UrlEncode(Encrypt(ddlCentreName.SelectedItem.Value));
    //                string courseCat = HttpUtility.UrlEncode(Encrypt(ddlCourseCat.SelectedItem.Value));
    //                string courseID = HttpUtility.UrlEncode(Encrypt(ddlCourseName.SelectedItem.Value));

    //                string url = "NielitCentreCourseWiseStudentsRep.aspx?startDate=" + startDate + "&endDate=" + endDate + "&courseCat=" + courseCat + "&centreID=" + centreID + "&courseID=" + courseID;
    //                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "openModal", "window.open('" + url + "' ,'_blank');", true);

    //                //Response.Redirect(string.Format("../Rpt/NielitCentreCourseWiseStudentsRep.aspx?startDate={0}&endDate={1}&courseCat={2}&centreID={3}&courseID={4}", startDate, endDate, courseCat, centreID, courseID));

    //                }
    //            else
    //                {
    //                ShowAlert("DateTo should not be greater than Today date.", true);
    //                return;
    //                }
    //            }
    //        else
    //            {
    //            ShowAlert("DateFrom should not be greater than DateTo.", true);
    //            return;
    //            }
    //        }
    //    catch (Exception ex)
    //        {
    //        ShowAlert(ex.Message, true);
    //        }
    //    }

    #endregion

    }