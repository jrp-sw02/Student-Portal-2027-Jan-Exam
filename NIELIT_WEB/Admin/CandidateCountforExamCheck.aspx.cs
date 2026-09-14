using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Data;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;


public partial class CandidateCountforExamCheck1 : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Lblerror.Text = "";
        Lblerror.Visible = false;
        

        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (loginUserType == UserType.RegionalCenter || loginUserType == UserType.HeadOffice || loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
            {
                if (!IsPostBack)
                {
                   // ddlExamYear.SelectedIndex = 0;
                    BindExamYear();                  
                }
            }
            else
            {
                //  BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Photographs For Certificate", "#", ""));
                //  BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Photographs For Certificate", "Admin/DownloadImagesForCertificate.aspx", ""));
                //  btnView.Visible = false;
                btnReset.Visible = false;
                Lblerror.Text = "You cannot see Candidate count for exam.";
                Lblerror.Visible = true;
            }
          
        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message;
            Lblerror.Visible = true;
        }
    }
    protected void BindExamYear()
    {
        try
        {
            using (var context = new EConnectContext())
            {
               
               // System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
                
                //var previousYear = DateTime.Now.AddYears(-1);
                //var  currentYear = DateTime.Now.Year;
                //var   nextYear = DateTime.Now.AddYears(1);

                //var examYear = previousYear + 
                //examYear = examYear.Distinct().OrderByDescending(p => p.ValueField);
                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, examYear, lst);
                
                List<Int32> years = new List <Int32>();            
                Int32 nextYear = DateTime.Now.Year + 1;
                
                for (Int32 year = 2020; year <= nextYear; year++)
                {                    
                    years.Add(year);
                }
                ddlExamYear.SelectedIndex = 0;
                ddlExamYear.DataSource = years;
                ddlExamYear.DataBind();               
               
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
   
    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {           
            Int32 examYear = Convert.ToInt32(ddlExamYear.SelectedValue);
            Int32 courseId = Convert.ToInt32(ddlcourse.SelectedValue);
            using (var context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("-- Select One --", "0");
                if (examYear > 0 && courseId == 0)
                {
                    var examName = context.Exams.Where(n => n.ExamYear == examYear && n.CourseCategoryID ==2 && n.CourseID != 75).Select(n => new { ValueField = n.ExamMonth, TextField = n.Name });
                                                       
                    examName = examName.Distinct().OrderByDescending(p => p.ValueField);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, examName, lst);
                    
                }

                else if (examYear > 0 && courseId != 0)
                {                    
                    var examName = context.Exams.Where(n => n.ExamYear == examYear && n.CourseCategoryID == 2 && n.CourseID != 75 && n.CourseID == courseId).Select(n => new { ValueField = n.ID, TextField = n.Name });

                    examName = examName.Distinct().OrderByDescending(p => p.ValueField);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, examName, lst);
                }
                else
                {
                    ddlExamName.Items.Clear();
                    ddlExamName.Items.Insert(0, lst);
                }
              
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    private DataTable GetAllExamId(Int64 examIdAll)
    {
        DataTable dt1 = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        Int32 eY = Convert.ToInt32(ddlExamYear.SelectedValue);

        string sql = "SELECT DISTINCT NAME , ID FROM EXAM WHERE EXAM_YEAR ='" + eY + "' AND COURSE_CATEGORY_ID = 2 AND EXAM_MONTH = '" + examIdAll + "'";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt1);
                }
            }
        }

        return dt1;
    }

    protected void BindGridView()
    {

        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        try
        {
            DataTable DT = new DataTable();
            con.Open();

            Int32 courseId = Convert.ToInt32(ddlcourse.SelectedValue);

            if (courseId == 0)
            {

                Int32 examIdAll = Convert.ToInt32(ddlExamName.SelectedValue);
                DataTable dataTable = GetAllExamId(examIdAll);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        using (SqlCommand Cmm = new SqlCommand("select convert(varchar, Date_of_Exam,106) Date_of_Exam,count(*) total_count, (select Name from course where id= c.Course_ID)  As Course_Name from Certificate_Exam_Application c  where exam_id ='" + dr["id"].ToString().Trim() + "'  and Date_of_Exam is not null  and Roll_Number is not null  group by Date_of_Exam,Course_ID  order by Date_of_Exam, Course_Name  ", con))
                        {
                            Cmm.CommandType = CommandType.Text;
                            SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                            Sda.Fill(DT);
                        }
                    }
                }

                if (DT == null)
                {
                    Lblerror.Visible = true;
                    Lblerror.Text = " There are no Exam Dates for selected course .";
                }

                PagingBar1.Bind(DT, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
            }
            else
            {

                Int32 examId = Convert.ToInt32(ddlExamName.SelectedValue);
                using (SqlCommand Cmm = new SqlCommand("select convert(varchar, Date_of_Exam,106) Date_of_Exam,count(*) total_count, (select Name from course where id= c.Course_ID)  As Course_Name from Certificate_Exam_Application c  where exam_id ='" + examId + "'   and Date_of_Exam is not null   and Roll_Number is not null  and course_id = '" + courseId + "' group by Date_of_Exam,Course_ID order by Date_of_Exam, Course_Name ", con))
                {
                    Cmm.CommandType = CommandType.Text;
                    SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                    Sda.Fill(DT);
                }

                PagingBar1.Bind(DT, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
            }
            
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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


    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            //BreadCrumb1.Render();
            ddlExamYear.SelectedIndex = 0;
            ddlcourse.SelectedIndex = 0;
            ddlExamName.SelectedIndex = 0;
            gvMain.Visible = false;
                     
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void btn_result_Click(object sender, EventArgs e)
    {
        BindGridView();
    }

    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {       
        try
        {           
            Int32 examYear = Convert.ToInt32(ddlExamYear.SelectedValue);       
   
            using (var context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("-- All --", "0");
                if (examYear > 0)
                {
                     var courseName = (from c in context.Courses
                                       join p in context.Exams on c.ID equals p.CourseID
                                       where c.CourseCategoryID == 2 && c.ID != 75 && p.ExamYear == examYear
                                       select new
                                       {
                                           ValueField = c.ID,
                                           TextField = c.Name
                                       });

                     courseName = courseName.Distinct().OrderByDescending(p => p.ValueField);
                     EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourse, courseName, lst);

                }
                else
                {
                    ddlcourse.Items.Clear();
                    ddlcourse.Items.Insert(0, lst);
                }
              
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

  
}