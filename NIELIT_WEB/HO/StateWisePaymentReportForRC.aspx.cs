using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;

public partial class HO_StateWisePaymentReportForRC : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                FillPaymentMode();
                FillCategories();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    #region-----Private Methods--------------------
    protected void FillCategories()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.CourseCategories
                               where p.ID == 2
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillCourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                Int32 Catid = Convert.ToInt32(ddlCourseCategry.SelectedValue);
               // String[] course = {"CCC", "BCC" };
                String[] course = { "CCC", "BCC", "CCCP","ECC" };
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == Catid && p.ShowOnWeb == true && course.Contains(p.Code)
                                 orderby p.Code
                                 select new { ValueField = p.ID, TextField = p.Code };
               
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillPaymentMode()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.PaymentModes
                               where p.Visible == true
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlpaymentmode, Category, lst);
            };
        }
        catch (Exception ex) { throw ex; }
    }
    #endregion-----Private Methods--------------------

    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 Catid = Convert.ToInt32(ddlCourseCategry.SelectedValue);
        if (Catid > 0)
            FillCourses();

    }
    protected void ddlDateRange_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDateRange.SelectedValue == "2") // From Start Date
        {
            txttDateFrom.Enabled = true;
            imgdate.Visible = true;
        }
        else
        {
            imgdate.Visible = false;
            txttDateFrom.Enabled = false;
        }
    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        EConnectContext context = new EConnectContext();
        string PaymentMode = ddlpaymentmode.SelectedItem.Text;
        DataTable dt;
        int CourseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
        int CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
        String CourseCode = ddlCourseName.SelectedItem.Text;
        string ServiceCodeExam = context.Courses.Find(CourseId).ExaminationServiceID;
        //string ServiceCodeRegn = context.Courses.Find(CourseId).RegistrationServiceID;
        StringBuilder mySql = new StringBuilder();

        DateTime FromDate = new DateTime(2010, 1, 1);
        if (ddlDateRange.SelectedValue == "2") // From Start Date
        {
            if (!string.IsNullOrEmpty(txttDateFrom.Text))
            { FromDate = Convert.ToDateTime(txttDateFrom.Text.Trim()); }
            else
            {
                ShowAlert("Please enter Start Date");
                return;
            }
        }

        #region NEFT-RTGS
        if (PaymentMode == "NEFT/RTGS")
        {
            if (ddlDateRange.SelectedItem.Text == "From Start Date")
            {
                mySql.Append("select d.Name, sum(c.Fee_Amt) as Total_Amount, cast(a.[date] as date) as Neft_Settle_Date  " +
                                "from NEFT_Transaction a, Demand_Note b,Certificate_Exam_Application c, Regional_Center d   " +
                                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID " +
                                "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by d.Name ,cast(a.[date] as date) order by Neft_Settle_Date  desc ");
            }
            else {
                mySql.Append("select d.Name, sum(c.Fee_Amt) as Total_Amount, cast(a.[date] as date) as Neft_Settle_Date  " +
                                   "from NEFT_Transaction a, Demand_Note b,Certificate_Exam_Application c, Regional_Center d   " +
                                   "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID " +
                                   "group by d.Name ,cast(a.[date] as date) order by Neft_Settle_Date  desc ");
            }
        }
        #endregion

        #region ONLINE
        else if (PaymentMode == "ONLINE")
        {
            if (ddlDateRange.SelectedItem.Text == "From Start Date")
            {
                mySql.Append("select d.Name, sum(c.Fee_Amt) as Total_Amount, cast([Settled_On] as date) as Online_Settle_Date  " +
                                "from Online_Transaction a, Demand_Note b,Certificate_Exam_Application c, Regional_Center d   " +
                                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID	 " +
                                "and a.Settled_On >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by d.Name ,cast([Settled_On] as date) order by Online_Settle_Date desc ");
            }
            else {
                mySql.Append("select d.Name, sum(c.Fee_Amt) as Total_Amount,cast([Settled_On] as date) as Online_Settle_Date  " +
                                   "from Online_Transaction a, Demand_Note b,Certificate_Exam_Application c, Regional_Center d   " +
                                   "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID	 " +
                                   "group by d.Name ,cast([Settled_On] as date) order by Online_Settle_Date desc ");
            }
        }
        #endregion

        #region CSC-SPV
        else if (PaymentMode == "CSC SPV")
        {
            if (ddlDateRange.SelectedItem.Text == "From Start Date")
            {
                mySql.Append("select d.Name, sum(c.Fee_Amt) as Total_Amount, cast(a.[date] as date) as CSC_Settle_Date  " +
                                "from CSC_Transaction a, Demand_Note b,Certificate_Exam_Application c, Regional_Center d	 " +
                                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and  a.Response_Message ='success' " +
                                "and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID	" +
                                "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by d.Name ,cast(a.[date] as date) order by CSC_Settle_Date,d.Name, Total_Amount   ");
            }
            else {
                mySql.Append("select d.Name, sum(c.Fee_Amt) as Total_Amount,cast(a.[date] as date) as CSC_Settle_Date  " +
                                "from CSC_Transaction a, Demand_Note b,Certificate_Exam_Application c, Regional_Center d	 " +
                                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and  a.Response_Message ='success' " +
                                "and a.Demand_Note_ID =c.Demand_Note_ID and c.Regional_Center_ID =d.ID	" +
                                "group by d.Name ,cast(a.[date] as date) order by CSC_Settle_Date,d.Name, Total_Amount   ");
            }
        }
        #endregion

        dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
        string sheetname = "RC_" + ddlpaymentmode.SelectedItem.Text.Replace("/", "_") + "_" + ddlCourseName.SelectedItem.Text.Replace("/", "_") + "_" + DateTime.Now;
       
        if (dt.Rows.Count > 0)
        {
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + sheetname + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //Apply text style to each Row
                GridView1.Rows[i].Attributes.Add("class", "textmode");
            }
            GridView1.RenderControl(hw);
            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        else
        {
            ShowAlert("No record found.");
        }
        context.Dispose();
    }
}