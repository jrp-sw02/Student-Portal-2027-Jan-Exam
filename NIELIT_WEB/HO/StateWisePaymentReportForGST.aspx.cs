

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;
using System.Data.SqlClient;

public partial class HO_StateWisePaymentReportForGST : BasePage
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
                FillGateways();
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
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, Category, lst);
            }
            ;
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

                if (Catid == 6)
                {
                    var CourseList = from p in context.Courses
                                     where p.CourseCategoryID == Catid && p.ShowOnWeb == true
                                     orderby p.Code
                                     select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" };


                    ddlAppType.Enabled = true;
                    //ddlAppType.SelectedValue = "0";
                    ddlAppType.Items.Clear();
                    ddlAppType.Items.Add(new ListItem("--Select One--", "0"));
                    ddlAppType.Items.Add(new ListItem("Registration", "1"));
                    ddlAppType.Items.Add(new ListItem("Examination", "2"));



                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, CourseList, lst);
                }
                else
                {
                    var CourseList = from p in context.Courses
                                     where p.CourseCategoryID == Catid && p.ShowOnWeb == true
                                     orderby p.Code
                                     select new
                                     {
                                         ValueField = p.ID,
                                         //Commented for DLC error 7 June 2024 TextField = (p.ID == 4 ? p.Code : p.Name + "(" + p.Code + ")")
                                         TextField = ((p.ID == 1 || p.ID == 2 || p.ID == 3 || p.ID == 4 || p.ID == 5 || p.ID == 7 || p.ID == 98 || p.ID == 99 || p.ID == 174 || p.ID == 175 || p.ID == 1226) ? p.Code : p.Name + "(" + p.Code + ")")
                                     };
                    //Changed by Madhur 21 May 2024 
                    //select new { ValueField = p.ID, TextField = p.Code };
                    if (Catid == 1)
                    { //CourseList = CourseList.Where(s => s.TextField == "C"); ddlAppType.Enabled = true; 
                    }
                    //Added for CHM(T)-O 24 May 2024
                    else if (Catid == 3)
                    {
                        ddlAppType.Items.Clear();
                        //changed on 06062025--- to show drop down in appl type
                        ddlAppType.Items.Add(new ListItem("--Select One--", "0"));
                        ddlAppType.Items.Add(new ListItem("Registration", "1"));
                        ddlAppType.Items.Add(new ListItem("Examination", "2"));
                        //changed on 06062025---end
                        ddlAppType.Enabled = true;
                    }

                    else
                    {
                        ddlAppType.Items.Clear();

                        ddlAppType.SelectedValue = "0";

                        ddlAppType.Enabled = false;
                    }





                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, CourseList, lst);
                }
            }
            ;
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
            }
            ;
        }
        catch (Exception ex) { throw ex; }
    }

    // added by amit start
    protected void FillGateways()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("-- ALL Gateways --", "0");
                var gateways = from p in context.Paymentgateways
                               orderby (p.Description)
                               select new { ValueField = p.ID, TextField = p.Description };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlgateway, gateways, lst);
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    // added by amit end

    #endregion-----Private Methods--------------------
    //vaf acf --------------to change application type when course_cat=1 if course= 1,2,3 then only show vaf acf else show reg exam vaf-acf
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCourseCategry.SelectedValue == "1")
        {

            if (ddlCourseName.SelectedValue == "4")
            {
                ddlAppType.Enabled = true;
                ddlAppType.Items.Clear();
                ddlAppType.Items.Add(new ListItem("--Select One--", "0"));
                ddlAppType.Items.Add(new ListItem("Registration", "1"));
                ddlAppType.Items.Add(new ListItem("Examination", "2"));
                ddlAppType.Items.Add(new ListItem("VAF ACF", "3"));
            }
            else
            {
                ddlAppType.Enabled = true;
                ddlAppType.Items.Clear();
                ddlAppType.Items.Add(new ListItem("--Select One--", "0"));
                ddlAppType.Items.Add(new ListItem("VAF ACF"));


            }
        }



    }
    //---------vaf acf end ------------------------
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
        string ServiceCodeVafAcf = context.Courses.Find(CourseId).VAFACFServiceID;
        string ServiceCodeRegn = context.Courses.Find(CourseId).RegistrationServiceID;
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
        if (CourseCode == "C")
        {
            if (ddlAppType.SelectedValue == "0")
            {
                ShowAlert("Please Select Application Type");
                return;
            }
        }

        //Added for Audit
        SqlParameter dtable = new SqlParameter("@fromDate", SqlDbType.DateTime);
        dtable.Value = FromDate.ToString("dd-MMM-yyyy");

        SqlParameter serviceParam = new SqlParameter("@ServiceCodeExam", SqlDbType.VarChar);
        serviceParam.Value = ServiceCodeExam;

        SqlParameter RegParam = new SqlParameter("@ServiceCodeRegn", SqlDbType.VarChar);
        if (ServiceCodeRegn == null)
            RegParam.Value = 0;
        else
            RegParam.Value = ServiceCodeRegn;

        SqlParameter courseParam = new SqlParameter("@CourseCatId", SqlDbType.Int);
        courseParam.Value = CourseCatId;


        SqlParameter courseIDParam = new SqlParameter("@CourseId", SqlDbType.Int);
        courseIDParam.Value = CourseId;


        SqlParameter[] parameters = { dtable, serviceParam, RegParam, courseParam, courseIDParam };

        #region NEFT-RTGS
        if (PaymentMode.Contains("NEFT/RTGS"))
        {
            #region DLC Course
            //Commented for DVP
            //if (CourseCode == "CCC" || CourseCode == "BCC" || CourseCode == "CCCP" || CourseCode == "ECC")
            if (CourseCode == "CCC" || CourseCode == "BCC" || CourseCode == "CCCP" || CourseCode == "ECC" || CourseCode == "DVP-CCC" || CourseCode == "DVP-BCC")
            {
                if (ddlDateRange.SelectedItem.Text == "From Start Date")
                {
                    mySql.Append("select e.Name, sum(c.Fee_Amt) as Total_Amount, cast(a.[date] as date) as Neft_Settle_Date " +
                                "from NEFT_Transaction a, Demand_Note b, Certificate_Exam_Application c, Exam_Center  d, Location e " +
                                "where a.Demand_Note_ID =b.ID and b.ServiceID =  @ServiceCodeExam  and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID = d.ID and d.State_ID =e.ID  " +
                                "and a.date >= CAST(@fromDate as DATE) group by e.Name , cast(a.[date] as date) order by Neft_Settle_Date desc");
                    //mySql.Append("select e.Name, sum(c.Fee_Amt) as Total_Amount, cast(a.[date] as date) as Neft_Settle_Date " +
                    //             "from NEFT_Transaction a, Demand_Note b, Certificate_Exam_Application c, Exam_Center  d, Location e " +
                    //             "where a.Demand_Note_ID =b.ID and b.ServiceID = '" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID = d.ID and d.State_ID =e.ID  " +
                    //             "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name , cast(a.[date] as date) order by Neft_Settle_Date desc");
                }
                else
                {
                    mySql.Append("select e.Name, sum(c.Fee_Amt) as Total_amt, cast(a.[date] as date) as Neft_Settle_Date " +
                                   "from NEFT_Transaction a, Demand_Note b, Certificate_Exam_Application c, Exam_Center  d, Location e " +
                                   "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam  and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID  " +
                                   "group by e.Name , cast(a.[date] as date) order by Neft_Settle_Date desc");
                    //mySql.Append("select e.Name, sum(c.Fee_Amt) as Total_amt, cast(a.[date] as date) as Neft_Settle_Date " +
                    //                "from NEFT_Transaction a, Demand_Note b, Certificate_Exam_Application c, Exam_Center  d, Location e " +
                    //                "where a.Demand_Note_ID =b.ID and b.ServiceID = '" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID  " +
                    //                "group by e.Name , cast(a.[date] as date) order by Neft_Settle_Date desc");

                }
            }
            else if (CourseCode == "ACC")
            {
                if (ddlDateRange.SelectedItem.Text == "From Start Date")
                {
                    mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'Neft_Settle_Date' " +
                                    "from NEFT_Transaction a, Demand_Note b, Course_Registration_Application c, Institute  d, Location e	  " +
                                    "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID and d.ID =c.Institute_ID  and d.State_ID =e.ID  " +
                                    "and c.Course_Category_Id = @CourseCatId and c.Candidate_ID is not null and c.Demand_Note_ID is not null  " +
                                    "and a.date >= CAST(@fromDate as DATE) group by e.Name ,  cast (a.[date] as date) order by Neft_Settle_Date, Total_Amount ");
                    //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'Neft_Settle_Date' " +
                    //                "from NEFT_Transaction a, Demand_Note b, Course_Registration_Application c, Institute  d, Location e	  " +
                    //                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeRegn + "' and a.Demand_Note_ID =c.Demand_Note_ID and d.ID =c.Institute_ID  and d.State_ID =e.ID  " +
                    //                "and c.Course_Category_Id ='" + CourseCatId + "' and c.Candidate_ID is not null and c.Demand_Note_ID is not null  " +
                    //                "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name ,  cast (a.[date] as date) order by Neft_Settle_Date, Total_Amount ");
                }
                else
                {
                    mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'Neft_Settle_Date' " +
                                      "from NEFT_Transaction a, Demand_Note b, Course_Registration_Application c, Institute  d, Location e	  " +
                                      "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID and d.ID =c.Institute_ID  and d.State_ID =e.ID  " +
                                      "and c.Course_Category_Id = @CourseCatId and c.Candidate_ID is not null and c.Demand_Note_ID is not null  " +
                                      "group by e.Name ,  cast (a.[date] as date) order by Neft_Settle_Date, Total_Amount");
                    //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'Neft_Settle_Date' " +
                    //                  "from NEFT_Transaction a, Demand_Note b, Course_Registration_Application c, Institute  d, Location e	  " +
                    //                  "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeRegn + "' and a.Demand_Note_ID =c.Demand_Note_ID and d.ID =c.Institute_ID  and d.State_ID =e.ID  " +
                    //                  "and c.Course_Category_Id ='" + CourseCatId + "' and c.Candidate_ID is not null and c.Demand_Note_ID is not null  " +
                    //                  "group by e.Name ,  cast (a.[date] as date) order by Neft_Settle_Date, Total_Amount");

                }
            }
            #endregion

            #region C Level Course
            else if (CourseCode == "C")
            {
                if (ddlAppType.SelectedItem.Text == "Registration")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                                        "from NEFT_Transaction a, Demand_Note b, Course_Registration_Application c, Location e	" +
                                        "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID	  " +
                                        "and a.date >= CAST(@fromDate as DATE) group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                        //                "from NEFT_Transaction a, Demand_Note b, Course_Registration_Application c, Location e	" +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID = '" + ServiceCodeRegn + "' and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID	  " +
                        //                "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                    }
                    else
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                                           "from NEFT_Transaction a, Demand_Note b, Course_Registration_Application c, Location e	" +
                                           "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID	  " +
                                           "group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                        //                   "from NEFT_Transaction a, Demand_Note b, Course_Registration_Application c, Location e	" +
                        //                   "where a.Demand_Note_ID =b.ID and b.ServiceID = '" + ServiceCodeRegn + "' and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID	  " +
                        //                   "group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                    }
                }
                else if (ddlAppType.SelectedItem.Text == "Examination")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                                        "from NEFT_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                                        "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID	" +
                                        "and d.State_ID =e.ID  AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number	" +
                                        "and a.date >= CAST(@fromDate as DATE) group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                        //                "from NEFT_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID	" +
                        //                "and d.State_ID =e.ID  AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number	" +
                        //                "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                    }
                    else
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                                            "from NEFT_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                                            "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID	" +
                                            "and d.State_ID =e.ID  AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number	" +
                                            "group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                        //                    "from NEFT_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                        //                    "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID	" +
                        //                    "and d.State_ID =e.ID  AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number	" +
                        //                    "group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                    }
                }
                //vaf acf added------------------------------
                else if (ddlAppType.SelectedItem.Text == "VAF ACF")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {

                        mySql.Append("with base as (select dn.ID, dn.Amount, t.date from Demand_Note dn join NEFT_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                                 "  , base2 as (                                                                                                                                                                                                       " +
                                 "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.date Settle_Date, cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                                 "      FROM                                                                                                                                                                                                           " +
                                 "      Course_Exam_VAF_ACF  cvcf                                                                                                                                                             " +
                                 "      inner join  Institute ins                                                                                                                                                             " +
                                 "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                                 "      inner join  Location loc                                                                                                                                                              " +
                                 "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                                 "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                                 "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount'                                                                                                                 " +
                                 "  from base2                                                                                                                                                                                                         " +
                                 "  where CourseID = @CourseId and Settle_Date >= CAST(@fromDate as DATE)                                                                                                                     " +
                                 "  group by  state_name, Settle_Date order by state_name, Online_Settle_Date");
                        //   mySql.Append("with base as (SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, dn.[ID] as demandnote, cast(ot.[date] as date) Settle_Date, cvcf.[TotalFee_Amount]   as fee ,                                   " +
                        //"  cvcf.[Course_ID] as CourseID FROM [NIELIT_PREPROD].[dbo].[Course_Exam_VAF_ACF]  cvcf  inner join  [NIELIT_PREPROD].[dbo].[Institute] ins  on cvcf.Institute_ID = ins.ID  inner join  [NIELIT_PREPROD].[dbo].[Location] loc           " +
                        //"      on ins.State_ID = loc.id inner join  [NIELIT_PREPROD].[dbo].[Demand_Note] dn  on dn.ID = cvcf.Demand_Note_ID   inner join  [NIELIT_PREPROD].[dbo].[NEFT_Transaction] ot on ot.ID = dn.Online_Transaction_ID )                    " +
                        //"  select base.state_name as 'State_Name',base.Settle_Date as 'NEFT_Settle_Date' , sum(fee) as 'Total_Amount' from base where base.CourseID =  " + CourseId + "  and  base.Settle_Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE)   group by  base.state_name,       " +
                        //"  base.Settle_Date  order by  base.state_name,base.Settle_Date");

                        //mySql.Append("select e.Name as 'State_Name', sum(b.Amount) as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date'" +
                        //                    " from NEFT_Transaction a, Demand_Note b, Course_Exam_VAF_ACF c, Course_Exam_Application_Detail ce, Institute i, Location e " +
                        //                    " where c.Course_ID = " + CourseId + " and a.Demand_Note_ID = b.ID and b.ServiceID ='" + ServiceCodeVafAcf + "' and a.Demand_Note_ID = c.Demand_Note_ID and c.Institute_ID = i.ID " +
                        //                    " and i.State_ID = e.ID AND c.Exam_ID = ce.Exam_ID AND c.Registration_Number = ce.Registration_Number " +
                        //                    " and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc ");
                    }
                    else
                    {

                        mySql.Append("with base as (select dn.ID, dn.Amount, t.date from Demand_Note dn join NEFT_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                  " +
                               "  , base2 as (                                                                                                                                                                                                       " +
                               "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.date Settle_Date, cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                               "      FROM                                                                                                                                                                                                           " +
                               "     Course_Exam_VAF_ACF  cvcf                                                                                                                                                             " +
                               "      inner join  Institute ins                                                                                                                                                             " +
                               "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                               "      inner join  Location loc                                                                                                                                                              " +
                               "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                               "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                               "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount'                                                                                                                 " +
                               "  from base2                                                                                                                                                                                                         " +
                               "  where CourseID = @CourseId and Settle_Date is not null                                                                                                                    " +
                               "  group by  state_name, Settle_Date order by state_name, Online_Settle_Date");
                        //    mySql.Append("with base as (SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, dn.[ID] as demandnote, cast(ot.[date] as date) Settle_Date, cvcf.[TotalFee_Amount]   as fee ,                                   " +
                        //"  cvcf.[Course_ID] as CourseID FROM [NIELIT_PREPROD].[dbo].[Course_Exam_VAF_ACF]  cvcf  inner join  [NIELIT_PREPROD].[dbo].[Institute] ins  on cvcf.Institute_ID = ins.ID  inner join  [NIELIT_PREPROD].[dbo].[Location] loc           " +
                        //"      on ins.State_ID = loc.id inner join  [NIELIT_PREPROD].[dbo].[Demand_Note] dn  on dn.ID = cvcf.Demand_Note_ID   inner join  [NIELIT_PREPROD].[dbo].[NEFT_Transaction] ot on ot.ID = dn.Online_Transaction_ID )                    " +
                        //"  select base.state_name as 'State_Name',base.Settle_Date as 'NEFT_Settle_Date' , sum(fee) as 'Total_Amount' from base where base.CourseID =  " + CourseId + "    group by  base.state_name,       " +
                        //"  base.Settle_Date  order by  base.state_name,base.Settle_Date");
                        //mySql.Append("select e.Name as 'State_Name', sum(b.Amount) as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                        //                    " from NEFT_Transaction a, Demand_Note b, Course_Exam_VAF_ACF c, Course_Exam_Application_Detail ce, Institute i, Location e " +
                        //                    " where c.Course_ID = " + CourseId + " and a.Demand_Note_ID = b.ID and b.ServiceID ='" + ServiceCodeVafAcf + "' and a.Demand_Note_ID = c.Demand_Note_ID and c.Institute_ID = i.ID " +
                        //                    " and i.State_ID = e.ID AND c.Exam_ID = ce.Exam_ID AND c.Registration_Number = ce.Registration_Number " +
                        //                    " group by e.Name, cast(a.[date] as date) order by 'NEFT_Settle_Date' desc ");
                    }
                }
            }
            else if (CourseCode == "O" || CourseCode == "A" || CourseCode == "B")
            {
                if (ddlAppType.SelectedItem.Text == "VAF ACF")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        mySql.Append("with base as (select dn.ID, dn.Amount, t.date from Demand_Note dn join NEFT_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                                "  , base2 as (                                                                                                                                                                                                       " +
                                "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.date Settle_Date, cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                                "      FROM                                                                                                                                                                                                           " +
                                "      Course_Exam_VAF_ACF  cvcf                                                                                                                                                             " +
                                "      inner join  Institute ins                                                                                                                                                             " +
                                "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                                "      inner join  Location loc                                                                                                                                                              " +
                                "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                                "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                                "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount'                                                                                                                 " +
                                "  from base2                                                                                                                                                                                                         " +
                                "  where CourseID = @CourseId and Settle_Date >= CAST(@fromDate as DATE)                                                                                                                                                      " +
                                "  group by  state_name, Settle_Date order by state_name, Online_Settle_Date");
                        //    mySql.Append("with base as (SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, dn.[ID] as demandnote, cast(ot.[date] as date) Settle_Date, cvcf.[TotalFee_Amount]   as fee ,                                   " +
                        //      "  cvcf.[Course_ID] as CourseID FROM [NIELIT_PREPROD].[dbo].[Course_Exam_VAF_ACF]  cvcf  inner join  [NIELIT_PREPROD].[dbo].[Institute] ins  on cvcf.Institute_ID = ins.ID  inner join  [NIELIT_PREPROD].[dbo].[Location] loc           " +
                        //      "      on ins.State_ID = loc.id inner join  [NIELIT_PREPROD].[dbo].[Demand_Note] dn  on dn.ID = cvcf.Demand_Note_ID   inner join  [NIELIT_PREPROD].[dbo].[NEFT_Transaction] ot on ot.ID = dn.Online_Transaction_ID )                    " +
                        //      "  select base.state_name as 'State_Name',base.Settle_Date as 'NEFT_Settle_Date' , sum(fee) as 'Total_Amount' from base where base.CourseID =  " + CourseId + "  and  base.Settle_Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE)   group by  base.state_name,       " +
                        //      "  base.Settle_Date  order by  base.state_name,base.Settle_Date");
                        //mySql.Append("select e.Name as 'State_Name', sum(b.Amount) as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date'" +
                        //                    " from NEFT_Transaction a, Demand_Note b, Course_Exam_VAF_ACF c, Course_Exam_Application_Detail ce, Institute i, Location e " +
                        //                    " where c.Course_ID = " + CourseId + " and a.Demand_Note_ID = b.ID and b.ServiceID ='" + ServiceCodeVafAcf + "' and a.Demand_Note_ID = c.Demand_Note_ID and c.Institute_ID = i.ID " +
                        //                    " and i.State_ID = e.ID AND c.Exam_ID = ce.Exam_ID AND c.Registration_Number = ce.Registration_Number " +
                        //                    " and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc ");
                    }
                    else
                    {
                        mySql.Append("with base as (select dn.ID, dn.Amount, t.date from Demand_Note dn join NEFT_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                                "  , base2 as (                                                                                                                                                                                                       " +
                                "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.date Settle_Date, cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                                "      FROM                                                                                                                                                                                                           " +
                                "      Course_Exam_VAF_ACF  cvcf                                                                                                                                                             " +
                                "      inner join Institute ins                                                                                                                                                             " +
                                "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                                "      inner join  Location loc                                                                                                                                                              " +
                                "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                                "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                                "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount'                                                                                                                 " +
                                "  from base2                                                                                                                                                                                                         " +
                                "  where CourseID = @CourseId and Settle_Date is not null                                                                                                                    " +
                                "  group by  state_name, Settle_Date order by state_name, Online_Settle_Date");
                        //mySql.Append("with base as (SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, dn.[ID] as demandnote, cast(ot.[date] as date) Settle_Date, cvcf.[TotalFee_Amount]   as fee ,                                   " +
                        //   "  cvcf.[Course_ID] as CourseID FROM [NIELIT_PREPROD].[dbo].[Course_Exam_VAF_ACF]  cvcf  inner join  [NIELIT_PREPROD].[dbo].[Institute] ins  on cvcf.Institute_ID = ins.ID  inner join  [NIELIT_PREPROD].[dbo].[Location] loc           " +
                        //   "      on ins.State_ID = loc.id inner join  [NIELIT_PREPROD].[dbo].[Demand_Note] dn  on dn.ID = cvcf.Demand_Note_ID   inner join  [NIELIT_PREPROD].[dbo].[NEFT_Transaction] ot on ot.ID = dn.Online_Transaction_ID )                    " +
                        //   "  select base.state_name as 'State_Name',base.Settle_Date as 'NEFT_Settle_Date' , sum(fee) as 'Total_Amount' from base where base.CourseID =  " + CourseId + "    group by  base.state_name,       " +
                        //   "  base.Settle_Date  order by  base.state_name,base.Settle_Date");
                        //mySql.Append("select e.Name as 'State_Name', sum(b.Amount) as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                        //                    " from NEFT_Transaction a, Demand_Note b, Course_Exam_VAF_ACF c, Course_Exam_Application_Detail ce, Institute i, Location e " +
                        //                    " where c.Course_ID = " + CourseId + " and a.Demand_Note_ID = b.ID and b.ServiceID ='" + ServiceCodeVafAcf + "' and a.Demand_Note_ID = c.Demand_Note_ID and c.Institute_ID = i.ID " +
                        //                    " and i.State_ID = e.ID AND c.Exam_ID = ce.Exam_ID AND c.Registration_Number = ce.Registration_Number " +
                        //                    " group by e.Name, cast(a.[date] as date) order by 'NEFT_Settle_Date' desc ");
                    }
                }
            }
            //vaf acf finish -----------------------------------


            #endregion
            #region NSQF Courses
            if (CourseCatId == 6)
            {
                if (ddlAppType.SelectedItem.Text == "Registration")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                                     "from NEFT_Transaction a, Demand_Note b, Course_Registration_Application c, Location e	" +
                                     "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID	  " +
                                     "and a.date >= CAST(@fromDate as DATE) group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                        //                "from NEFT_Transaction a, Demand_Note b, Course_Registration_Application c, Location e	" +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID = '" + ServiceCodeRegn + "' and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID	  " +
                        //                "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                    }
                    else
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                                           "from NEFT_Transaction a, Demand_Note b, Course_Registration_Application c, Location e	" +
                                           "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID	  " +
                                           "group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                        //                   "from NEFT_Transaction a, Demand_Note b, Course_Registration_Application c, Location e	" +
                        //                   "where a.Demand_Note_ID =b.ID and b.ServiceID = '" + ServiceCodeRegn + "' and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID	  " +
                        //                   "group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                    }
                }
                else if (ddlAppType.SelectedItem.Text == "Examination")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                                        "from NEFT_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                                        "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID	" +
                                        "and d.State_ID =e.ID  AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number	" +
                                        "and a.date >= CAST( @fromDate as DATE) group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                        //                "from NEFT_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID	" +
                        //                "and d.State_ID =e.ID  AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number	" +
                        //                "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                    }
                    else
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                                            "from NEFT_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                                            "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID	" +
                                            "and d.State_ID =e.ID  AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number	" +
                                            "group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                        //                    "from NEFT_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                        //                    "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID	" +
                        //                    "and d.State_ID =e.ID  AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number	" +
                        //                    "group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                    }
                }

            }
            #endregion
            // Added 24 May 2024
            #region CHMT-O Courses
            else if (CourseCatId == 3)
            {
                if (ddlAppType.SelectedItem.Text == "Registration")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                                  "from NEFT_Transaction a, Demand_Note b, Course_Registration_Application c, Location e	" +
                                  "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID	  " +
                                  "and a.date >= CAST(@fromDate as DATE) group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                        //          "from NEFT_Transaction a, Demand_Note b, Course_Registration_Application c, Location e	" +
                        //          "where a.Demand_Note_ID =b.ID and b.ServiceID = '" + ServiceCodeRegn + "' and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID	  " +
                        //          "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");

                    }
                    else
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                                      "from NEFT_Transaction a, Demand_Note b, Course_Registration_Application c, Location e	" +
                                      "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID	  " +
                                      "group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                        //               "from NEFT_Transaction a, Demand_Note b, Course_Registration_Application c, Location e	" +
                        //               "where a.Demand_Note_ID =b.ID and b.ServiceID = '" + ServiceCodeRegn + "' and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID	  " +
                        //               "group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                    }

                }
                else if (ddlAppType.SelectedItem.Text == "Examination")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                                      "from NEFT_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                                      "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID	" +
                                      "and d.State_ID =e.ID  AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number	" +
                                      "and a.date >= CAST(@fromDate as DATE) group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                        //              "from NEFT_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                        //              "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID	" +
                        //              "and d.State_ID =e.ID  AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number	" +
                        //              "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                    }
                    else
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                                       "from NEFT_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                                       "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID	" +
                                       "and d.State_ID =e.ID  AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number	" +
                                       "group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(a.[date] as date) as 'NEFT_Settle_Date' " +
                        //                "from NEFT_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID	" +
                        //                "and d.State_ID =e.ID  AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number	" +
                        //                "group by e.Name ,cast(a.[date] as date) order by 'NEFT_Settle_Date' desc");
                    }
                }
            }
            #endregion

            /////////////End 24 May 2024

        }
        #endregion

        #region ONLINE
        else if (PaymentMode == "ONLINE")
        {
            #region DLC Course
            if (CourseCode == "CCC" || CourseCode == "BCC" || CourseCode == "CCCP" || CourseCode == "ECC" || CourseCode == "DVP-CCC" || CourseCode == "DVP-BCC")
            //Modified for DVP
            //if (CourseCode == "CCC" || CourseCode == "BCC" || CourseCode == "CCCP" || CourseCode == "ECC")
            {
                if (ddlDateRange.SelectedItem.Text == "From Start Date")
                {

                    if (ddlgateway.SelectedItem.Value == "2") // ICICI
                    {
                        mySql.Append("select e.Name, sum(c.Fee_Amt) as Total_Amount, cast(Request_Date as date) as PaymentDate,  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                   " from Online_Transaction a, Demand_Note b, Certificate_Exam_Application c, Exam_Center d, Location e " +
                                   " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  = d.ID and d.State_ID =e.ID and a.PGCode = 2	 and a.id not in (select transation_id from OnlineRefund) " +
                                   " and (a.Request_Date >= CAST(@fromDate as date)) and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000') group by e.Name, cast(Request_Date as date), a.PgCode order by PaymentDate desc ");
                    }
                    else if (ddlgateway.SelectedItem.Value == "1") // BillDesk 
                    {
                        mySql.Append("select e.Name, sum(c.Fee_Amt) as Total_Amount, cast(Settled_On as date) as Online_Settle_Date,  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                    " from Online_Transaction  a, Demand_Note b,Certificate_Exam_Application c, Exam_Center  d, Location e " +
                                    " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID and (a.PGCode = 1 OR a.PGCode is NULL) and a.id not in (select transation_id from OnlineRefund) " +
                                    " and (a.Settled_On >= CAST(@fromDate as date)) group by e.Name ,cast(Settled_On as date), a.PgCode order by Online_Settle_Date desc ");
                    }
                    else // ALL
                    {
                        mySql.Append("select e.Name, sum(c.Fee_Amt) as Total_Amount, CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE) AS PaymentDate,  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]" +
                                  " from Online_Transaction a, Demand_Note b, Certificate_Exam_Application c, Exam_Center  d, Location e " +
                                  " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID and a.id not in (select transation_id from OnlineRefund) " +
                                  "   AND (( a.PGCode = 2 AND a.Request_Date >= CAST(@fromDate as date) AND ( a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')) " +
                                  "   OR ((a.PGCode = 1 OR a.PGCode IS NULL) AND a.Settled_On >= CAST(@fromDate as date))) " +
                                  " group by e.Name ,CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ), a.Pgcode order by PaymentDate desc ");
                    }

                }
                else
                {

                    if (ddlgateway.SelectedItem.Value == "2") // ICICI
                    {
                        mySql.Append("select e.Name, sum(c.Fee_Amt) as Total_Amount, cast(Request_Date as date) as Payment_Date , isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                       " from Online_Transaction  a, Demand_Note b, Certificate_Exam_Application c, Exam_Center  d, Location e" +
                                       " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID=d.ID and d.State_ID =e.ID  and a.PGCode = 2 and a.id not in (select transation_id from OnlineRefund) " +
                                       " and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000') " +
                                       " group by e.Name, cast(Request_Date as date), a.PgCode order by Payment_Date desc ");
                    }
                    else if (ddlgateway.SelectedItem.Value == "1") // BILL DESK
                    {
                        mySql.Append("select e.Name, sum(c.Fee_Amt) as Total_Amount, cast(Settled_On as date) as Online_Settle_Date , isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]" +
                                     " from Online_Transaction  a, Demand_Note b,Certificate_Exam_Application c, Exam_Center  d, Location e" +
                                     " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID = d.ID and d.State_ID =e.ID and (a.PGCode = 1 or a.PGCode is NULL) and a.id not in (select transation_id from OnlineRefund) " +
                                     " group by e.Name ,cast(Settled_On as date), a.PgCode order by Online_Settle_Date desc ");
                    }
                    else
                    {
                        mySql.Append("select e.Name, sum(c.Fee_Amt) as Total_Amount, CAST(CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE) as Payment_Date,  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]" +
                                    " from Online_Transaction  a, Demand_Note b, Certificate_Exam_Application c, Exam_Center  d, Location e" +
                                    " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID and a.id not in (select transation_id from OnlineRefund) " +
                                    " and ( (a.PGCode = 2 and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')) OR ((a.PGCode = 1 OR a.PGCode is NULL) and a.Settled_On is not null) ) " +
                                    " group by e.Name, CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ), a.PgCode order by Payment_Date desc ");
                    }

                }
            }
            else if (CourseCode == "ACC")
            {
                if (ddlDateRange.SelectedItem.Text == "From Start Date")
                {

                    if (ddlgateway.SelectedItem.Value == "2") // ICICI
                    {
                        mySql.Append("select e.Name , sum(c.Fee_Amt) as Total_Amount, cast(Request_Date as date) as Payment_Date ,  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]" +
                                 " from Online_Transaction  a, Demand_Note b,Course_Registration_Application c, Institute  d, Location e " +
                                 " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and d.ID =c.Institute_ID  and d.State_ID =e.ID and a.PGCode = 2 and a.id not in (select transation_id from OnlineRefund) " +
                                 " and c.Course_Category_Id = @CourseCatId and c.Candidate_ID is not null  and c.Demand_Note_ID is not null  " +
                                 " and (a.Request_Date >= CAST(@fromDate as date)) and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000') group by e.Name, cast(Request_Date as date), a.PgCode order by Payment_Date, e.Name ,Total_Amount ");
                    }
                    else if (ddlgateway.SelectedItem.Value == "1") // BillDesk 
                    {

                        mySql.Append("select e.Name , sum(c.Fee_Amt) as Total_Amount, cast(Settled_On as date) as Online_Settle_Date ,  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]" +
                                 " from Online_Transaction  a, Demand_Note b,Course_Registration_Application c, Institute  d, Location e " +
                                 " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and d.ID =c.Institute_ID and a.id not in (select transation_id from OnlineRefund)  " +
                                 " and c.Course_Category_Id = @CourseCatId and c.Candidate_ID is not null  and c.Demand_Note_ID is not null and (a.PGCode IS NULL OR a.PGCode = 1)  " +
                                 " and (a.Settled_On >= CAST(@fromDate as date)) group by e.Name, cast(Settled_On as date), a.PgCode order by Online_Settle_Date, e.Name ,Total_Amount ");
                    }
                    else // ALL
                    {
                        mySql.Append("select e.Name , sum(c.Fee_Amt) as Total_Amount, CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE) AS Payment_Date,  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]" +
                                    " from Online_Transaction  a, Demand_Note b, Course_Registration_Application c, Institute  d, Location e  " +
                                    " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and d.ID =c.Institute_ID  and d.State_ID =e.ID and a.id not in (select transation_id from OnlineRefund) " +
                                    " and c.Course_Category_Id = @CourseCatId and c.Candidate_ID is not null  and c.Demand_Note_ID is not null  " +
                                      "   AND (( a.PGCode = 2 AND a.Request_Date >= CAST(@fromDate as date) AND ( a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')) " +
                                          "   OR ((a.PGCode = 1 OR a.PGCode IS NULL) AND a.Settled_On >= CAST(@fromDate as date))) " +
                                    " group by e.Name,CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ), a.PgCode order by Payment_Date, e.Name ,Total_Amount ");
                    }

                }
                else
                {
                    if (ddlgateway.SelectedItem.Value == "2") // ICICI
                    {
                        mySql.Append("select e.Name , sum(c.Fee_Amt) as Total_Amount, cast(Request_Date as date) as Online_Settle_Date,  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]" +
                                " from Online_Transaction  a, Demand_Note b,Course_Registration_Application c, Institute  d, Location e" +
                                " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and d.ID =c.Institute_ID  and d.State_ID =e.ID  and a.PGCode = 2 and a.id not in (select transation_id from OnlineRefund)  " +
                                " and c.Course_Category_Id = @CourseCatId and c.Candidate_ID is not null  and c.Demand_Note_ID is not null  " +
                                " and ( a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000' ) " +
                                " group by e.Name ,  cast(a.Request_Date as date), a.PGCode order by Online_Settle_Date , e.Name ,Total_Amount ");


                    }
                    else if (ddlgateway.SelectedItem.Value == "1")
                    { // BillDesk 
                        mySql.Append("select e.Name , sum(c.Fee_Amt) as Total_Amount, cast(Settled_On as date) as Online_Settle_Date,  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]" +
                         " from Online_Transaction  a, Demand_Note b,Course_Registration_Application c, Institute  d, Location e" +
                         " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and d.ID =c.Institute_ID  and d.State_ID =e.ID and (a.PGCode = 1 OR a.PGCode is NULL) and a.id not in (select transation_id from OnlineRefund) " +
                         " and c.Course_Category_Id = @CourseCatId and c.Candidate_ID is not null  and c.Demand_Note_ID is not null  " +
                         " group by e.Name, cast(a.Settled_On as date), a.PGCode order by Online_Settle_Date , e.Name ,Total_Amount ");


                    }
                    else
                    {
                        mySql.Append("select e.Name , sum(c.Fee_Amt) as Total_Amount, CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ) as 'Online_Settle_Date',  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]" +
                         " from Online_Transaction  a, Demand_Note b, Course_Registration_Application c, Institute  d, Location e" +
                         " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and d.ID =c.Institute_ID  and d.State_ID =e.ID  " +
                         " and c.Course_Category_Id = @CourseCatId and c.Candidate_ID is not null  and c.Demand_Note_ID is not null and a.id not in (select transation_id from OnlineRefund) " +
                         " and ( (a.PGCode = 2 and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')) OR ((a.PGCode = 1 OR a.PGCode is NULL) and a.Settled_On is not null) ) " +
                         " group by e.Name, CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ), a.PGCode order by 'Online_Settle_Date', e.Name ,Total_Amount ");


                    }
                }
            }
            #endregion

            #region C Level Course
            else if (CourseCode == "C")
            {
                if (ddlAppType.SelectedItem.Text == "Registration")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {

                        if (ddlgateway.SelectedItem.Value == "2") //ICICI
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(Request_Date as date) as 'PaymentDate', isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PaymentGateway]" +
                                        " from Online_Transaction a, Demand_Note b, Course_Registration_Application c, Location e" +
                                        " where a.Demand_Note_ID =b.ID and b.ServiceID = '@ServiceCodeRegn'" +
                                        " and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and a.PGCode = 2 and a.id not in (select transation_id from OnlineRefund) " +
                                        " and a.Request_Date >= CAST(@fromDate as DATE) and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000') " +
                                        " group by e.Name ,cast(Request_Date as date), a.PgCode order by PaymentDate desc");
                        }
                        else if (ddlgateway.SelectedItem.Value == "1") // BIll desk
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(Settled_On as date) as 'Online_Settle_Date', isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PaymentGateway]" +
                                        " from Online_Transaction a, Demand_Note b,Course_Registration_Application c, Location e" +
                                        " where a.Demand_Note_ID =b.ID and b.ServiceID = '@ServiceCodeRegn'" +
                                        " and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID  and (a.PGCode IS NULL OR a.PGCode = 1) and a.id not in (select transation_id from OnlineRefund) " +
                                        " and a.Settled_On >= CAST(@fromDate as DATE) group by e.Name ,cast(Settled_On as date), a.PgCode order by Online_Settle_Date desc");
                        }
                        else // ALL CASE
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ) as 'Online_Settle_Date', isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PaymentGateway] " +
                                      " from Online_Transaction a, Demand_Note b,Course_Registration_Application c, Location e	 " +
                                      " where a.Demand_Note_ID =b.ID and b.ServiceID = '@ServiceCodeRegn' " +
                                      " and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and a.id not in (select transation_id from OnlineRefund)  " +
                                    "   AND (( a.PGCode = 2 AND a.Request_Date >= CAST(@fromDate as date) AND ( a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')) " +
                                        "   OR ((a.PGCode = 1 OR a.PGCode IS NULL) AND a.Settled_On >= CAST(@fromDate as date))) " +
                                      " group by e.Name ,CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ), a.PgCode order by Online_Settle_Date desc");
                        }
                    }


                    else
                    {
                        if (ddlgateway.SelectedItem.Value == "2")
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount',  cast(Request_Date as date) as 'Payment_Date', isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]" +
                                    " from Online_Transaction a, Demand_Note b,Course_Registration_Application c, Location e" +
                                    " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and a.PGcode = 2 and a.id not in (select transation_id from OnlineRefund) " +
                                    " and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000') " +
                                    " group by e.Name ,cast(Request_Date as date),a.PgCode order by Payment_Date desc ");
                        }
                        else if (ddlgateway.SelectedItem.Value == "1")
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount',  cast(Settled_On as date) as 'Online_Settle_Date', isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]" +
                                " from Online_Transaction a, Demand_Note b,Course_Registration_Application c, Location e" +
                                " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and (a.PGCode is NULL OR a.PGCode = 1) and a.id not in (select transation_id from OnlineRefund) " +
                                " group by e.Name ,cast(Settled_On as date), a.PgCode order by Online_Settle_Date desc");
                        }
                        else
                        {
                            mySql.Append(" select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount',  CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ) as 'Online_Settle_Date', isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]" +
                                " from Online_Transaction a, Demand_Note b,Course_Registration_Application c, Location e" +
                                " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and a.id not in (select transation_id from OnlineRefund) " +
                                " and ( (a.PGCode = 2 and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')) OR ((a.PGCode = 1 OR a.PGCode is NULL) and a.Settled_On is not null) ) " +
                                " group by e.Name ,CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ), a.PgCode order by Online_Settle_Date desc");
                        }

                    }
                }

                else if (ddlAppType.SelectedItem.Text == "Examination")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        if (ddlgateway.SelectedItem.Value == "2")
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount',  cast(Request_Date as date) as 'Payment_Date',   isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                       " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e" +
                                       " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  = d.ID and  a.PGCode = 2 and a.id not in (select transation_id from OnlineRefund) " +
                                       " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number " +
                                       " and a.Request_Date >= CAST(@fromDate as DATE) and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')   group by e.Name , cast(Request_Date as date), a.PGCode order by Payment_Date desc");
                        }
                        else if (ddlgateway.SelectedItem.Value == "1")
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount',  cast(Settled_On as date) as 'Online_Settle_Date', isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                       " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e" +
                                       " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  = d.ID and ( a.PGCode = 1 OR a.PGCode is NULL ) and a.id not in (select transation_id from OnlineRefund) " +
                                       " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number = CE.Registration_Number " +
                                       " and a.Settled_On >= CAST(@fromDate as DATE) group by e.Name, cast(Settled_On as date), a.PgCode order by Online_Settle_Date desc  ");
                        }
                        else
                        {
                            mySql.Append(" select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount',  CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ) as 'Online_Settle_Date',  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                       " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e" +
                                       " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID = d.ID and a.id not in (select transation_id from OnlineRefund) " +
                                       " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number " +
                                       "   AND (( a.PGCode = 2 AND a.Request_Date >= CAST(@fromDate as date) AND ( a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')) " +
                                       "   OR ((a.PGCode = 1 OR a.PGCode IS NULL) AND a.Settled_On >= CAST(@fromDate as date))) " +
                                       " group by e.Name, CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ), a.PgCode  order by Online_Settle_Date desc  ");
                        }
                    }
                    else
                    {
                        if (ddlgateway.SelectedItem.Value == "2")
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(Request_Date as date) as 'Payment_date' , isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                           " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                                           " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and a.PGCode = 2	and a.id not in (select transation_id from OnlineRefund) " +
                                           " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number " +
                                           " and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000') " +
                                           " group by e.Name , cast(Request_Date as date), a.PgCode  order by Payment_date desc  ");
                        }
                        else if (ddlgateway.SelectedItem.Value == "1")
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(Settled_On as date) as 'Online_Settle_Date' ,  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                         " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e" +
                                         " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and (a.PGCode IS NULL OR a.PGCode = 1) and a.id not in (select transation_id from OnlineRefund) " +
                                         " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number " +
                                         " group by e.Name , cast(Settled_On as date), a.PgCode  order by Online_Settle_Date desc  ");
                        }
                        else
                        {
                            mySql.Append(" select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE) AS PaymentDate ,  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                        " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e" +
                                        " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and a.id not in (select transation_id from OnlineRefund)  " +
                                        " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number " +
                                        " and ( (a.PGCode = 2 and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')) OR ((a.PGCode = 1 OR a.PGCode is NULL) and a.Settled_On is not null) ) " +
                                        " group by e.Name , CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ), a.PgCode order by PaymentDate desc  ");
                        }

                    }
                }
                else if (ddlAppType.SelectedItem.Text == "VAF ACF")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        if (ddlgateway.SelectedItem.Value == "2") // ICICI
                        {
                            mySql.Append("with base as (select dn.ID, dn.Amount, t.Settled_On,t.Request_Date, t.PGCode from Demand_Note dn join Online_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                                   "  , base2 as (                                                                                                                                                                                                       " +
                                   "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.Request_Date, b.PGCode ,cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                                   "      FROM                                                                                                                                                                                                           " +
                                   "      [Course_Exam_VAF_ACF]  cvcf                                                                                                                                                             " +
                                   "      inner join  [Institute] ins                                                                                                                                                             " +
                                   "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                                   "      inner join  [Location] loc                                                                                                                                                              " +
                                   "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                                   "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                                   "  select state_name as 'State_Name', CAST(Request_Date AS DATE) as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount' ,isnull((select Description from PaymentGateways where id = PGCode),'EFT') as PAYMENTGATEWAY                                                                                                                " +
                                   "  from base2                                                                                                                                                                                                         " +
                                   "  where CourseID = @CourseId and Request_Date >= CAST(@fromDate as DATE) and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000') and Pgcode  = 2                                                                                                                    " +
                                   "  group by  state_name, CAST(Request_Date AS DATE), PGCode order by state_name, Online_Settle_Date");
                        }
                        else if (ddlgateway.SelectedItem.Value == "1") //billdesk
                        {
                            mySql.Append("with base as (select dn.ID, dn.Amount, t.Settled_On,t.Request_Date, t.PGCode from Demand_Note dn join Online_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                                                "  , base2 as (                                                                                                                                                                                                       " +
                                                "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.Settled_On as Settle_Date, b.Request_Date, b.PGCode ,cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                                                "      FROM                                                                                                                                                                                                           " +
                                                "      [Course_Exam_VAF_ACF]  cvcf                                                                                                                                                             " +
                                                "      inner join  [Institute] ins                                                                                                                                                             " +
                                                "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                                                "      inner join  [Location] loc                                                                                                                                                              " +
                                                "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                                                "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                                                "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount' ,isnull((select Description from PaymentGateways where id = PGCode),'EFT') as PAYMENTGATEWAY                                                                                                                " +
                                                "  from base2                                                                                                                                                                                                         " +
                                                "  where CourseID = @CourseId and Settle_Date >= CAST(@fromDate as DATE)  and ( pgcode =1 OR pgcode is null  )                                                                                                                   " +
                                                "  group by  state_name, Settle_Date, PGCode order by state_name, Online_Settle_Date");
                        }
                        else
                        {
                            mySql.Append("with base as (select dn.ID, dn.Amount, t.Settled_On,t.Request_Date, t.PGCode from Demand_Note dn join Online_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                                                       "  , base2 as (                                                                                                                                                                                                       " +
                                                       "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.Settled_On as Settle_Date, b.Request_Date, b.PGCode ,cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                                                       "      FROM                                                                                                                                                                                                           " +
                                                       "      [Course_Exam_VAF_ACF]  cvcf                                                                                                                                                             " +
                                                       "      inner join  [Institute] ins                                                                                                                                                             " +
                                                       "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                                                       "      inner join  [Location] loc                                                                                                                                                              " +
                                                       "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                                                       "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                                                       "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount' ,isnull((select Description from PaymentGateways where id = PGCode),'EFT') as PAYMENTGATEWAY                                                                                                                " +
                                                       "  from base2                                                                                                                                                                                                         " +
                                                       "  where CourseID = @CourseId and (PGCode = 2 and (Request_Date >= CAST(@fromDate as date)) or ( (PGCode = 1 or PGCode is null) and Settled_On >= CAST(@fromDate as date) ) )                                                                                                                   " +
                                                       "  group by  state_name," +
                                                       " Settle_Date, PGCode " +
                                                       "order by state_name, Online_Settle_Date");
                        }

                        //  mySql.Append("with base as (SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, dn.[ID] as demandnote, ot.Settled_On Settle_Date, cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID " +
                        //" FROM  [NIELIT_PREPROD].[dbo].[Course_Exam_VAF_ACF]  cvcf  inner join  [NIELIT_PREPROD].[dbo].[Institute] ins  on cvcf.Institute_ID = ins.ID  inner join  [NIELIT_PREPROD].[dbo].[Location] loc  on ins.State_ID = loc.id " +
                        //"inner join  [NIELIT_PREPROD].[dbo].[Demand_Note] dn  on dn.ID = cvcf.Demand_Note_ID   inner join  [NIELIT_PREPROD].[dbo].[Online_Transaction] ot on ot.ID = dn.Online_Transaction_ID )select base.state_name as 'State_Name'" +
                        //",base.Settle_Date as 'Online_Settle_Date' , sum(fee) as 'Total_Amount' from base where base.CourseID = " + CourseId + " and  base.Settle_Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE)   group by  base.state_name,base.Settle_Date  order by  base.state_name,base.Settle_Date");
                        //mySql.Append("select e.Name as 'State_Name', sum(b.Amount) as 'Total_Amount', cast(Settled_On as date) as 'Online_Settle_Date' " +
                        //                    " from Online_Transaction a, Demand_Note b, Course_Exam_VAF_ACF c, Course_Exam_Application_Detail ce, Institute i, Location e " +
                        //                    " where c.Course_ID = " + CourseId + " and a.Demand_Note_ID = b.ID and b.ServiceID ='" + ServiceCodeVafAcf + "' and a.Demand_Note_ID = c.Demand_Note_ID and c.Institute_ID = i.ID " +
                        //                    " and i.State_ID = e.ID AND c.Exam_ID = ce.Exam_ID AND c.Registration_Number = ce.Registration_Number " +
                        //                    " and a.Settled_On >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name , cast(Settled_On as date) order by Online_Settle_Date desc");
                    }
                    else
                    {
                        if (ddlgateway.SelectedItem.Value == "2") // ICICI
                        {
                            mySql.Append("with base as (select dn.ID, dn.Amount, t.Settled_On, t.PGCode, t.Request_Date from Demand_Note dn join Online_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                                "  , base2 as (                                                                                                                                                                                                       " +
                                "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.Settled_On as Settle_Date, b.Request_Date, b.PGCode, cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                                "      FROM                                                                                                                                                                                                           " +
                                "      [Course_Exam_VAF_ACF]  cvcf                                                                                                                                                             " +
                                "      inner join  [Institute] ins                                                                                                                                                             " +
                                "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                                "      inner join  [Location] loc                                                                                                                                                              " +
                                "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                                "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                                "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount', isnull((select Description from PaymentGateways where id = PgCode),'EFT') AS [PAYMENTGATEWAY]                                                                                                                  " +
                                "  from base2                                                                                                                                                                                                         " +
                                "  where CourseID = @CourseId and Settle_Date is not null and pgcode  = 1                                                                                                                   " +
                                "  group by  state_name, Settle_Date, PGcode order by state_name, Online_Settle_Date");

                        }
                        else if (ddlgateway.SelectedItem.Value == "1")
                        {
                            mySql.Append("with base as (select dn.ID, dn.Amount, t.Settled_On, t.PGCode, t.Request_Date from Demand_Note dn join Online_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                            "  , base2 as (                                                                                                                                                                                                       " +
                            "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.Settled_On as Settle_Date, b.Request_Date, b.PGCode, cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                            "      FROM                                                                                                                                                                                                           " +
                            "      [Course_Exam_VAF_ACF]  cvcf                                                                                                                                                             " +
                            "      inner join  [Institute] ins                                                                                                                                                             " +
                            "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                            "      inner join  [Location] loc                                                                                                                                                              " +
                            "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                            "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                            "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount', isnull((select Description from PaymentGateways where id = PgCode),'EFT') AS [PAYMENTGATEWAY]                                                                                                                  " +
                            "  from base2                                                                                                                                                                                                         " +
                            "  where CourseID = @CourseId and Settle_Date is not null  and (pgcode  = 1 or pgcode is null)                                                                                                                   " +
                            "  group by  state_name, Settle_Date, PGcode order by state_name, Online_Settle_Date");
                        }
                        else
                        {
                            mySql.Append("with base as (select dn.ID, dn.Amount, t.Settled_On, t.PGCode, t.Request_Date from Demand_Note dn join Online_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                            "  , base2 as (                                                                                                                                                                                                       " +
                            "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.Settled_On as Settle_Date, b.Request_Date, b.PGCode, cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                            "      FROM                                                                                                                                                                                                           " +
                            "      [Course_Exam_VAF_ACF]  cvcf                                                                                                                                                             " +
                            "      inner join  [Institute] ins                                                                                                                                                             " +
                            "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                            "      inner join  [Location] loc                                                                                                                                                              " +
                            "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                            "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                            "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount', isnull((select Description from PaymentGateways where id = PgCode),'EFT') AS [PAYMENTGATEWAY]                                                                                                                  " +
                            "  from base2                                                                                                                                                                                                         " +
                            "  where CourseID = @CourseId and Settle_Date is not null                                                                                                               " +
                            "  group by  state_name, Settle_Date, PGcode order by state_name, Online_Settle_Date");
                        }

                        //mySql.Append("with base as (SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, dn.[ID] as demandnote, ot.Settled_On Settle_Date, cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID " +
                        //" FROM [NIELIT_PREPROD].[dbo].[Course_Exam_VAF_ACF]  cvcf  inner join  [NIELIT_PREPROD].[dbo].[Institute] ins  on cvcf.Institute_ID = ins.ID  inner join  [NIELIT_PREPROD].[dbo].[Location] loc  on ins.State_ID = loc.id " +
                        //"inner join  [NIELIT_PREPROD].[dbo].[Demand_Note] dn  on dn.ID = cvcf.Demand_Note_ID   inner join  [NIELIT_PREPROD].[dbo].[Online_Transaction] ot on ot.ID = dn.Online_Transaction_ID )select base.state_name as 'State_Name'" +
                        //",base.Settle_Date as 'Online_Settle_Date' , sum(fee) as 'Total_Amount' from base  where base.CourseID = " + CourseId + "   group by  base.state_name,base.Settle_Date  order by  base.state_name,base.Settle_Date");
                        //mySql.Append("select e.Name as 'State_Name', sum(b.Amount) as 'Total_Amount', cast(Settled_On as date) as 'Online_Settle_Date' " +
                        //                    " from Online_Transaction a, Demand_Note b, Course_Exam_VAF_ACF c, Course_Exam_Application_Detail ce, Institute i, Location e " +
                        //                    " where c.Course_ID = " + CourseId + " and a.Demand_Note_ID = b.ID and b.ServiceID ='" + ServiceCodeVafAcf + "' and a.Demand_Note_ID = c.Demand_Note_ID and c.Institute_ID = i.ID " +
                        //                    " and i.State_ID = e.ID AND c.Exam_ID = ce.Exam_ID AND c.Registration_Number = ce.Registration_Number " +
                        //                    " group by e.Name, cast(Settled_On as date) order by Online_Settle_Date desc ");
                    }
                }

            }
            #endregion

            else if (CourseCode == "O" || CourseCode == "A" || CourseCode == "B")
            {
                if (ddlAppType.SelectedItem.Text == "VAF ACF")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {


                        if (ddlgateway.SelectedItem.Value == "2") // ICICI
                        {
                            mySql.Append("with base as (select dn.ID, dn.Amount, t.Settled_On,t.Request_Date, t.PGCode from Demand_Note dn join Online_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                                   "  , base2 as (                                                                                                                                                                                                       " +
                                   "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.Settled_On as Settle_Date, b.Request_Date, b.PGCode ,cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                                   "      FROM                                                                                                                                                                                                           " +
                                   "      [Course_Exam_VAF_ACF]  cvcf                                                                                                                                                             " +
                                   "      inner join  [Institute] ins                                                                                                                                                             " +
                                   "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                                   "      inner join  [Location] loc                                                                                                                                                              " +
                                   "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                                   "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                                   "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount' ,isnull((select Description from PaymentGateways where id = PGCode),'EFT') as PAYMENTGATEWAY                                                                                                                " +
                                   "  from base2                                                                                                                                                                                                         " +
                                   "  where CourseID = @CourseId and Request_Date >= CAST(@fromDate as DATE) and Pgcode  = 2                                                                                                                    " +
                                   "  group by  state_name, Settle_Date, PGCode order by state_name, Online_Settle_Date");
                        }
                        else if (ddlgateway.SelectedItem.Value == "1") //billdesk
                        {
                            mySql.Append("with base as (select dn.ID, dn.Amount, t.Settled_On,t.Request_Date, t.PGCode from Demand_Note dn join Online_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                                                "  , base2 as (                                                                                                                                                                                                       " +
                                                "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.Settled_On as Settle_Date, b.Request_Date, b.PGCode ,cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                                                "      FROM                                                                                                                                                                                                           " +
                                                "      [Course_Exam_VAF_ACF]  cvcf                                                                                                                                                             " +
                                                "      inner join  [Institute] ins                                                                                                                                                             " +
                                                "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                                                "      inner join  [Location] loc                                                                                                                                                              " +
                                                "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                                                "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                                                "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount' ,isnull((select Description from PaymentGateways where id = PGCode),'EFT') as PAYMENTGATEWAY                                                                                                                " +
                                                "  from base2                                                                                                                                                                                                         " +
                                                "  where CourseID = @CourseId and Settle_Date >= CAST(@fromDate as DATE)  and ( pgcode is null or pgcode =1 )                                                                                                                   " +
                                                "  group by  state_name, Settle_Date, PGCode order by state_name, Online_Settle_Date");
                        }
                        else
                        {
                            mySql.Append("with base as (select dn.ID, dn.Amount, t.Settled_On,t.Request_Date, t.PGCode from Demand_Note dn join Online_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                                                       "  , base2 as (                                                                                                                                                                                                       " +
                                                       "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.Settled_On as Settle_Date, b.Request_Date, b.PGCode ,cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                                                       "      FROM                                                                                                                                                                                                           " +
                                                       "      [Course_Exam_VAF_ACF]  cvcf                                                                                                                                                             " +
                                                       "      inner join  [Institute] ins                                                                                                                                                             " +
                                                       "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                                                       "      inner join  [Location] loc                                                                                                                                                              " +
                                                       "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                                                       "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                                                       "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount' ,isnull((select Description from PaymentGateways where id = PGCode),'EFT') as PAYMENTGATEWAY                                                                                                                " +
                                                       "  from base2                                                                                                                                                                                                         " +
                                                       "  where CourseID = @CourseId and and (PGCode = 2 and (Request_Date >= CAST(@fromDate as date)) or ((PGCode = 1 or PGCode is null) and Settled_On >= CAST(@fromDate as date)))                                                                                                                   " +
                                                       "  group by  state_name, Settle_Date, PGCode order by state_name, Online_Settle_Date");
                        }




                    }
                    else
                    {

                        if (ddlgateway.SelectedItem.Value == "2") // ICICI
                        {
                            mySql.Append("with base as (select dn.ID, dn.Amount, t.Settled_On, t.PGCode, t.Request_Date from Demand_Note dn join Online_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                                "  , base2 as (                                                                                                                                                                                                       " +
                                "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.Settled_On as Settle_Date, b.Request_Date, b.PGCode, cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                                "      FROM                                                                                                                                                                                                           " +
                                "      [Course_Exam_VAF_ACF]  cvcf                                                                                                                                                             " +
                                "      inner join  [Institute] ins                                                                                                                                                             " +
                                "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                                "      inner join  [Location] loc                                                                                                                                                              " +
                                "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                                "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                                "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount', isnull((select Description from PaymentGateways where id = PgCode),'EFT') AS [PAYMENTGATEWAY]                                                                                                                  " +
                                "  from base2                                                                                                                                                                                                         " +
                                "  where CourseID = @CourseId and Settle_Date is not null and pgcode  = 2                                                                                                                   " +
                                "  group by  state_name, Settle_Date, PGcode order by state_name, Online_Settle_Date");

                        }
                        else if (ddlgateway.SelectedItem.Value == "1")
                        {
                            mySql.Append("with base as (select dn.ID, dn.Amount, t.Settled_On, t.PGCode, t.Request_Date from Demand_Note dn join Online_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                            "  , base2 as (                                                                                                                                                                                                       " +
                            "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.Settled_On as Settle_Date, b.Request_Date, b.PGCode, cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                            "      FROM                                                                                                                                                                                                           " +
                            "      [Course_Exam_VAF_ACF]  cvcf                                                                                                                                                             " +
                            "      inner join  [Institute] ins                                                                                                                                                             " +
                            "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                            "      inner join  [Location] loc                                                                                                                                                              " +
                            "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                            "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                            "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount', isnull((select Description from PaymentGateways where id = PgCode),'EFT') AS [PAYMENTGATEWAY]                                                                                                                  " +
                            "  from base2                                                                                                                                                                                                         " +
                            "  where CourseID = @CourseId and Settle_Date is not null  and (pgcode  = 1 or pgcode is null)                                                                                                                   " +
                            "  group by  state_name, Settle_Date, PGcode order by state_name, Online_Settle_Date");
                        }
                        else
                        {
                            mySql.Append("with base as (select dn.ID, dn.Amount, t.Settled_On, t.PGCode, t.Request_Date from Demand_Note dn join Online_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                            "  , base2 as (                                                                                                                                                                                                       " +
                            "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.Settled_On as Settle_Date, b.Request_Date, b.PGCode, cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                            "      FROM                                                                                                                                                                                                           " +
                            "      [Course_Exam_VAF_ACF]  cvcf                                                                                                                                                             " +
                            "      inner join  [Institute] ins                                                                                                                                                             " +
                            "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                            "      inner join  [Location] loc                                                                                                                                                              " +
                            "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                            "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                            "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount', isnull((select Description from PaymentGateways where id = PgCode),'EFT') AS [PAYMENTGATEWAY]                                                                                                                  " +
                            "  from base2                                                                                                                                                                                                         " +
                            "  where CourseID = @CourseId and Settle_Date is not null                                                                                                               " +
                            "  group by  state_name, Settle_Date, PGcode order by state_name, Online_Settle_Date");
                        }




                    }
                }
            }

            #region NSQF Courses
            if (CourseCatId == 6)
            {

                if (ddlAppType.SelectedItem.Text == "Registration")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {

                        if (ddlgateway.SelectedItem.Value == "2") // ICICI
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(Request_Date as date) as 'Online_Settle_Date', isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]  " +
                                       " from Online_Transaction a, Demand_Note b,Course_Registration_Application c, Location e	 " +
                                       " where a.Demand_Note_ID =b.ID and b.ServiceID =  @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and a.PGCode = 2	 and a.id not in (select transation_id from OnlineRefund)   " +
                                       " and a.Request_Date >= CAST(@fromDate as date) and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000') group by e.Name ,cast(Request_Date as date) order by Online_Settle_Date desc");

                        }
                        else if (ddlgateway.SelectedItem.Value == "1") // BillDesk 
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(Settled_On as date) as 'Online_Settle_Date',  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]  " +
                                      " from Online_Transaction a, Demand_Note b,Course_Registration_Application c, Location e	 " +
                                      " where a.Demand_Note_ID =b.ID and b.ServiceID =  @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and (a.PGCode IS NULL OR a.PGCode = 1) and a.id not in (select transation_id from OnlineRefund)	 " +
                                      " and (a.Settled_On >= CAST(@fromDate as date))  group by e.Name ,cast(Settled_On as date), f.Description order by Online_Settle_Date desc");
                        }
                        else
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE) as 'Online_Settle_Date',  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                     " from Online_Transaction a, Demand_Note b,Course_Registration_Application c, Location e	 " +
                                     " where a.Demand_Note_ID =b.ID and b.ServiceID =  @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and a.id not in (select transation_id from OnlineRefund) " +
                                     "   AND (( a.PGCode = 2 AND a.Request_Date >= CAST(@fromDate as date) AND ( a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')) " +
                                     "   OR ((a.PGCode = 1 OR a.PGCode IS NULL) AND a.Settled_On >= CAST(@fromDate as date))) " +
                                     " group by e.Name ,CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE),a.pgcode order by Online_Settle_Date desc");
                        }
                    }
                    else
                    {

                        if (ddlgateway.SelectedItem.Value == "2") // ICICI
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount',  cast(Request_Date as date) as 'Online_Settle_Date',  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                         " from Online_Transaction a, Demand_Note b,Course_Registration_Application c, Location e	 " +
                                         " where a.Demand_Note_ID =b.ID and b.ServiceID =  @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID  and a.PGCode = 2	and a.id not in (select transation_id from OnlineRefund) " +
                                         " and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000') " +
                                         " group by e.Name ,cast(Request_Date as date),a.PGCode order by Online_Settle_Date desc");
                        }
                        else if (ddlgateway.SelectedItem.Value == "1") // BillDesk 
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount',  cast(Settled_On as date) as 'Online_Settle_Date', isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]  " +
                                        " from Online_Transaction a, Demand_Note b,Course_Registration_Application c, Location e " +
                                        " where a.Demand_Note_ID =b.ID and b.ServiceID =  @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID  and  (a.PGCode IS NULL OR a.PGCode = 1) and a.id not in (select transation_id from OnlineRefund) " +
                                        " group by e.Name ,cast(Settled_On as date),a.PGCode order by Online_Settle_Date desc");
                        }
                        else
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount',  CAST(CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE) as 'Online_Settle_Date',  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]  " +
                                        " from Online_Transaction a, Demand_Note b,Course_Registration_Application c, Location e " +
                                        " where a.Demand_Note_ID =b.ID and b.ServiceID =  @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID " +
                                        " and ( (a.PGCode = 2 and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')) OR ((a.PGCode = 1 OR a.PGCode is NULL) and a.Settled_On is not null) ) and a.id not in (select transation_id from OnlineRefund) " +
                                        " group by e.Name,  CAST(CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE) ,a.PGCode order by Online_Settle_Date desc ");
                        }

                    }
                }
                else if (ddlAppType.SelectedItem.Text == "Examination")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {

                        if (ddlgateway.SelectedItem.Value == "2") // ICICI
                        {

                            mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount',  cast(Request_Date as date) as 'Online_Settle_Date',  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                          " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e" +
                                          " where a.Demand_Note_ID =b.ID and b.ServiceID =  @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and a.PGCode = 2	and a.id not in (select transation_id from OnlineRefund)  " +
                                          " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number " +
                                          " and  a.Request_Date >= CAST(@fromDate as DATE) and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000') group by e.Name , cast(Request_Date as date), a.PGCode order by Online_Settle_Date desc  ");
                        }
                        else if (ddlgateway.SelectedItem.Value == "1") // BillDesk 
                        {

                            mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount',  cast(Settled_On as date) as 'Online_Settle_Date', isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                          " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e" +
                                          " where a.Demand_Note_ID =b.ID and b.ServiceID =  @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and (a.PGCode IS NULL OR a.PGCode = 1)	and a.id not in (select transation_id from OnlineRefund) " +
                                          " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number  " +
                                          "  and a.Settled_On >= CAST(@fromDate as DATE) group by e.Name , cast(Settled_On as date), a.PGCode order by Online_Settle_Date desc  ");
                        }
                        else
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount',CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE) as 'Online_Settle_Date',  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                      "from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e" +
                                      " where a.Demand_Note_ID =b.ID and b.ServiceID =  @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID " +
                                      " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number and a.id not in (select transation_id from OnlineRefund) " +
                                      "   AND (( a.PGCode = 2 AND a.Request_Date >= CAST(@fromDate as date) AND ( a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')) " +
                                        "   OR ((a.PGCode = 1 OR a.PGCode IS NULL) AND a.Settled_On >= CAST(@fromDate as date))) " +
                                      " group by e.Name ,CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE), a.PGCode order by Online_Settle_Date desc  ");
                        }

                    }
                    else
                    {


                        if (ddlgateway.SelectedItem.Value == "2") // ICICI
                        {

                            mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(Request_Date as date) as 'Online_Settle_Date',  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                           " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e" +
                                           " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and  a.PGCode = 2	 " +
                                           " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number and a.id not in (select transation_id from OnlineRefund) " +
                                           " and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000') " +
                                           " group by e.Name , cast(Request_Date as date), a.PGCode order by Online_Settle_Date desc  ");

                        }
                        else if (ddlgateway.SelectedItem.Value == "1") // BillDesk 
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(Settled_On as date) as 'Online_Settle_Date',  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                       " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e" +
                                       " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and (a.PGCode IS NULL OR a.PGCode = 1)	 " +
                                       " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number and a.id not in (select transation_id from OnlineRefund) " +
                                       " group by e.Name , cast(Settled_On as date), a.PGCode order by Online_Settle_Date desc  ");
                        }
                        else
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ) as 'Online_Settle_Date',  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY] " +
                                       " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e" +
                                       " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID " +
                                       " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number and a.id not in (select transation_id from OnlineRefund) " +
                                       " and ( (a.PGCode = 2 and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')) OR ((a.PGCode = 1 OR a.PGCode is NULL) and a.Settled_On is not null) ) " +
                                       " group by e.Name , CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ), a.Pgcode order by Online_Settle_Date desc  ");
                        }

                    }
                }
            }
            #endregion

            //Added 24-May-2024
            #region CHMT-O Courses
            else if (CourseCatId == 3)
            {  
                if (ddlAppType.SelectedItem.Text == "Registration")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        if (ddlgateway.SelectedItem.Value == "2") // ICICI
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(Request_Date as date) as 'Online_Settle_Date', isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') as 'Payment Gateway Used'  from Online_Transaction a, Demand_Note b, Course_Registration_Application c, Location e " +
                                        " where a.Demand_Note_ID =b.ID and b.ServiceID =  @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID  and a.PGCode = 2 and a.id not in (select transation_id from OnlineRefund) " +
                                         " and a.Request_Date >= CAST(@fromDate as DATE)  and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')  group by e.Name ,cast(Request_Date as date),a.PGCode order by Online_Settle_Date desc");
                        }
                        else if (ddlgateway.SelectedItem.Value == "1") // BillDesk 
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast(Settled_On as date) as 'Online_Settle_Date',isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') as 'Payment Gateway Used'   from Online_Transaction a, Demand_Note b, Course_Registration_Application c, Location e " +
                                        " where a.Demand_Note_ID =b.ID and b.ServiceID =  @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and  (a.PGCode = 1 or a.PGCode is NULL) and a.id not in (select transation_id from OnlineRefund) " +
                                        "and a.Settled_On >= CAST(@fromDate as DATE) group by e.Name ,cast(Settled_On as date), a.PGCode order by Online_Settle_Date desc");

                        }
                        else
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount',CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ) as 'Online_Settle_Date',isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') as 'Payment Gateway Used' from Online_Transaction a, Demand_Note b, Course_Registration_Application c, Location e " +
                                        " where a.Demand_Note_ID =b.ID and b.ServiceID =  @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and a.id not in (select transation_id from OnlineRefund) " +
                                        " and (a.PGCode = 2 and (a.Request_Date >= CAST(@fromDate as date)) or (  (a.PGCode = 1 or a.PGCode is null) and a.Settled_On >= CAST(@fromDate as date)))" +
                                        " group by e.Name ,CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ),a.PGCode order by Online_Settle_Date desc");

                        }

                    }
                    else
                    {



                        if (ddlgateway.SelectedItem.Value == "2") // ICICI
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount',  cast(Request_Date as date) as 'Online_Settle_Date', isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') as 'Payment Gateway Used'  " +
                                          " from Online_Transaction a, Demand_Note b,Course_Registration_Application c, Location e" +
                                          " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn  and a.Demand_Note_ID =c.Demand_Note_ID and C.Cor_State_ID =e.ID  and (a.PGCode = 2) and a.id not in (select transation_id from OnlineRefund)  " +
                                          " and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000') " +
                                          " group by e.Name, cast(Request_Date as date), a.PGCode order by Online_Settle_Date desc");
                        }
                        else if (ddlgateway.SelectedItem.Value == "1") // BillDesk 
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount',  cast(Settled_On as date) as 'Online_Settle_Date', isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') as 'Payment Gateway Used'  " +
                                            " from Online_Transaction a, Demand_Note b,Course_Registration_Application c, Location e" +
                                            " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn  and a.Demand_Note_ID =c.Demand_Note_ID and C.Cor_State_ID =e.ID and a.id not in (select transation_id from OnlineRefund) and  (a.PGCode IS NULL OR a.PGCode = 1) " +
                                            " group by e.Name ,cast(Settled_On as date),a.PGCode order by Online_Settle_Date desc");

                        }
                        else
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount',  CAST(CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE) as 'Online_Settle_Date',isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') as 'Payment Gateway Used'  " +
                                        " from Online_Transaction a, Demand_Note b,Course_Registration_Application c, Location e" +
                                        " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn  and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and a.id not in (select transation_id from OnlineRefund) " +
                                        " and ( (a.PGCode = 2 and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')) OR ((a.PGCode = 1 OR a.PGCode is NULL) and a.Settled_On is not null) ) " +
                                        " group by e.Name ,CAST(CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE),a.PGCode order by Online_Settle_Date desc ");

                        }

                    }
                }
                else if (ddlAppType.SelectedItem.Text == "Examination")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {


                        if (ddlgateway.SelectedItem.Value == "2") // ICICI
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount',  cast(Request_Date as date) as 'Online_Settle_Date', isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]" +
                                        " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                                        " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID  and a.PGCode = 2 and a.id not in (select transation_id from OnlineRefund) " +
                                        " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number and " +
                                        " a.Request_Date >= CAST(@fromDate as DATE) and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000') group by e.Name , cast(Request_Date as date), a.PGCode order by Online_Settle_Date desc  ");

                        }
                        else if (ddlgateway.SelectedItem.Value == "1") // BillDesk 
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount',  cast(Settled_On as date) as 'Online_Settle_Date', isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]" +
                                        " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                                        " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and (a.PGCode = 1 or a.PGCode is NULL) " +
                                        " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number and a.id not in (select transation_id from OnlineRefund) AND " +
                                        " a.Settled_On >= CAST(@fromDate as DATE) group by e.Name , cast(Settled_On as date), a.PGCode order by Online_Settle_Date desc  ");

                        }
                        else
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount',  CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ), isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') AS [PAYMENTGATEWAY]" +
                                        " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                                        " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID " +
                                        " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number and a.id not in (select transation_id from OnlineRefund) " +
                                        "   AND (( a.PGCode = 2 AND a.Request_Date >= CAST(@fromDate as date) AND ( a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')) " +
                                        "   OR ((a.PGCode = 1 OR a.PGCode IS NULL) AND a.Settled_On >= CAST(@fromDate as date))) " +
                                        " group by e.Name , CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ), a.PGCode order by Online_Settle_Date desc  ");
                        }

                    }
                    else
                    {
                        if (ddlgateway.SelectedItem.Value == "2") // ICICI
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(Request_Date as date) as 'Online_Settle_Date', isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') as 'Payment Gateway Used'  " +
                                    " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                                    " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID = d.ID  and a.PGCode = 2  and a.id not in (select transation_id from OnlineRefund) " +
                                    " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number " +
                                    " group by e.Name , cast(Request_Date as date), a.PGCode  order by Online_Settle_Date desc  ");
                        }
                        else if (ddlgateway.SelectedItem.Value == "1") // BillDesk 
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', cast(Settled_On as date) as 'Online_Settle_Date',  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') as 'Payment Gateway Used'  " +
                                    " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                                    " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID = d.ID  and  (a.PGCode IS NULL OR a.PGCode = 1)	and a.id not in (select transation_id from OnlineRefund) " +
                                    " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number " +
                                    " group by e.Name , cast(Settled_On as date), a.PGCode  order by Online_Settle_Date desc  ");
                        }
                        else
                        {
                            mySql.Append("select e.Name as 'State_Name', sum(ce.Fee_Amount)  as 'Total_Amount', CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ) as 'Online_Settle_Date',  isnull((select Description from PaymentGateways where id = a.PgCode),'EFT') as 'Payment Gateway Used'  " +
                                " from Online_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e " +
                                " where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID = d.ID and a.id not in (select transation_id from OnlineRefund) " +
                                " and d.State_ID =e.ID AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number " +
                                " and ( (a.PGCode = 2 and (a.Response_Status_Message = 'Success' OR a.Response_Status_Code = 'E000')) OR ((a.PGCode = 1 OR a.PGCode is NULL) and a.Settled_On is not null) ) " +
                                " group by e.Name ,  CAST( CASE WHEN a.PGCode = 2 THEN a.Request_Date ELSE a.Settled_On END AS DATE ), a.PGCode order by Online_Settle_Date desc  ");
                        }


                    }
                }
            }
            #endregion
        }
        #endregion

        #region CSC-SPV
        else if (PaymentMode == "CSC SPV")
        {
            #region DLC Course
            //Commented for DVP
            //if (CourseCode == "CCC" || CourseCode == "BCC" || CourseCode == "CCCP" || CourseCode == "ECC")
            if (CourseCode == "CCC" || CourseCode == "BCC" || CourseCode == "CCCP" || CourseCode == "ECC" || CourseCode == "DVP-CCC" || CourseCode == "DVP-BCC")
            {
                if (ddlDateRange.SelectedItem.Text == "From Start Date")
                {
                    mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date'   " +
                                     "from CSC_Transaction a, Demand_Note b,Certificate_Exam_Application c, Exam_Center d, Location e	 " +
                                     "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam  and a.Demand_Note_ID =c.Demand_Note_ID	" +
                                     "and d.ID =c.Exam_Center1_ID and d.State_ID =e.ID  and  a.Response_Message ='success'	" +
                                     "and a.date >= CAST( @fromDate as DATE) group by e.Name ,  cast (a.[date] as date) order by   cast (a.[date] as date), e.Name ,'Total_Amount'");
                    //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date'   " +
                    //                 "from CSC_Transaction a, Demand_Note b,Certificate_Exam_Application c, Exam_Center d, Location e	 " +
                    //                 "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID	" +
                    //                 "and d.ID =c.Exam_Center1_ID and d.State_ID =e.ID  and  a.Response_Message ='success'	" +
                    //                 "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name ,  cast (a.[date] as date) order by   cast (a.[date] as date), e.Name ,'Total_Amount'");
                }
                else
                {
                    mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date'   " +
                                      "from CSC_Transaction a, Demand_Note b,Certificate_Exam_Application c, Exam_Center d, Location e	 " +
                                      "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam  and a.Demand_Note_ID =c.Demand_Note_ID	" +
                                      "and d.ID =c.Exam_Center1_ID and d.State_ID =e.ID  and  a.Response_Message ='success'	" +
                                      "group by e.Name ,  cast (a.[date] as date) order by   cast (a.[date] as date), e.Name ,'Total_Amount'");
                    //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date'   " +
                    //                   "from CSC_Transaction a, Demand_Note b,Certificate_Exam_Application c, Exam_Center d, Location e	 " +
                    //                   "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID	" +
                    //                   "and d.ID =c.Exam_Center1_ID and d.State_ID =e.ID  and  a.Response_Message ='success'	" +
                    //                   "group by e.Name ,  cast (a.[date] as date) order by   cast (a.[date] as date), e.Name ,'Total_Amount'");
                }
            }
            else if (CourseCode == "ACC")
            {
                if (ddlDateRange.SelectedItem.Text == "From Start Date")
                {
                    mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date'	  " +
                                     "from CSC_Transaction a, Demand_Note b,Course_Registration_Application c, Institute  d, Location e	 " +
                                     "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID	  " +
                                     "and d.ID =c.Institute_ID  and d.State_ID =e.ID and c.Course_Category_Id = @CourseCatId and c.Candidate_ID is not null and c.Demand_Note_ID is not null  " +
                                     "and a.date >= CAST( @fromDate  as DATE) group by e.Name , cast (a.[date] as date) order by cast (a.[date] as date), e.Name ,'Total_Amount'");
                    //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date'	  " +
                    //                 "from CSC_Transaction a, Demand_Note b,Course_Registration_Application c, Institute  d, Location e	 " +
                    //                 "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeRegn + "' and a.Demand_Note_ID =c.Demand_Note_ID	  " +
                    //                 "and d.ID =c.Institute_ID  and d.State_ID =e.ID and c.Course_Category_Id ='" + CourseCatId + "' and c.Candidate_ID is not null and c.Demand_Note_ID is not null  " +
                    //                 "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name , cast (a.[date] as date) order by cast (a.[date] as date), e.Name ,'Total_Amount'");
                }
                else
                {
                    mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date'	  " +
                                     "from CSC_Transaction a, Demand_Note b,Course_Registration_Application c, Institute  d, Location e	 " +
                                     "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID	  " +
                                     "and d.ID =c.Institute_ID  and d.State_ID =e.ID and c.Course_Category_Id = @CourseCatId and c.Candidate_ID is not null and c.Demand_Note_ID is not null  " +
                                     "group by e.Name , cast (a.[date] as date) order by cast (a.[date] as date), e.Name ,'Total_Amount'");
                    //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date'	  " +
                    //                 "from CSC_Transaction a, Demand_Note b,Course_Registration_Application c, Institute  d, Location e	 " +
                    //                 "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeRegn + "' and a.Demand_Note_ID =c.Demand_Note_ID	  " +
                    //                 "and d.ID =c.Institute_ID  and d.State_ID =e.ID and c.Course_Category_Id ='" + CourseCatId + "' and c.Candidate_ID is not null and c.Demand_Note_ID is not null  " +
                    //                 "group by e.Name , cast (a.[date] as date) order by cast (a.[date] as date), e.Name ,'Total_Amount'");
                }
            }
            #endregion

            #region C Level Course
            else if (CourseCode == "C")
            {
                if (ddlAppType.SelectedItem.Text == "Registration")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                                        "from CSC_Transaction a, Demand_Note b,Course_Registration_Application c, Location e   " +
                                        "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and  a.Response_Message ='success' " +
                                        "and a.date >= CAST( @fromDate   as DATE) group by e.Name , cast (a.[date] as date) order by cast (a.[date] as date) desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                        //                "from CSC_Transaction a, Demand_Note b,Course_Registration_Application c, Location e   " +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeRegn + "' and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and  a.Response_Message ='success' " +
                        //                "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name , cast (a.[date] as date) order by cast (a.[date] as date) desc");
                    }
                    else
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                                       "from CSC_Transaction a, Demand_Note b, Course_Registration_Application c, Location e   " +
                                       "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and  a.Response_Message ='success' " +
                                       "group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                        //                "from CSC_Transaction a, Demand_Note b, Course_Registration_Application c, Location e   " +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeRegn + "' and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and  a.Response_Message ='success' " +
                        //                "group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) desc");
                    }

                }
                else if (ddlAppType.SelectedItem.Text == "Examination")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(CE.Fee_Amount)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                                        "from CSC_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e   " +
                                        "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID " +
                                        "AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number  and  a.Response_Message ='success'	 " +
                                        "and a.date >= CAST(@fromDate  as DATE) group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) ");
                        //mySql.Append("select e.Name as 'State_Name', sum(CE.Fee_Amount)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                        //                "from CSC_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e   " +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID " +
                        //                "AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number  and  a.Response_Message ='success'	 " +
                        //                "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) ");
                    }
                    else
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(CE.Fee_Amount)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                                        "from CSC_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e   " +
                                        "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID " +
                                        "AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number  and  a.Response_Message ='success'	 " +
                                        "group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) ");
                        //mySql.Append("select e.Name as 'State_Name', sum(CE.Fee_Amount)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                        //                "from CSC_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e   " +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID " +
                        //                "AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number  and  a.Response_Message ='success'	 " +
                        //                "group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) ");
                    }
                }

                //vaf acf added------------------------------
                else if (ddlAppType.SelectedItem.Text == "VAF ACF")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {

                        mySql.Append("with base as (select dn.ID, dn.Amount, t.Date from Demand_Note dn join CSC_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                                "  , base2 as (                                                                                                                                                                                                       " +
                                "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.Date Settle_Date, cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                                "      FROM                                                                                                                                                                                                           " +
                                "      Course_Exam_VAF_ACF  cvcf                                                                                                                                                             " +
                                "      inner join  Institute ins                                                                                                                                                             " +
                                "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                                "      inner join Location loc                                                                                                                                                              " +
                                "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                                "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                                "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount'                                                                                                                 " +
                                "  from base2                                                                                                                                                                                                         " +
                                "  where CourseID = @CourseId and Settle_Date >= CAST(@fromDate as DATE)                                                                                                                     " +
                                "  group by  state_name, Settle_Date order by state_name, Online_Settle_Date");
                        //mySql.Append("with base as (SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, dn.[ID] as demandnote, cast(ot.[date] as date) Settle_Date, cvcf.[TotalFee_Amount]   as fee                                  " +
                        //"      , cvcf.[Course_ID] as CourseID FROM [NIELIT_PREPROD].[dbo].[Course_Exam_VAF_ACF]  cvcf  inner join  [NIELIT_PREPROD].[dbo].[Institute] ins  on cvcf.Institute_ID = ins.ID                                                     " +
                        //"  inner join  [NIELIT_PREPROD].[dbo].[Location] loc  on ins.State_ID = loc.id inner join  [NIELIT_PREPROD].[dbo].[Demand_Note] dn  on dn.ID = cvcf.Demand_Note_ID   inner join  [NIELIT_PREPROD].[dbo].[CSC_Transaction] ot         " +
                        //"      on ot.ID = dn.Online_Transaction_ID )select base.state_name as 'State_Name',base.Settle_Date as 'CSC_Payment_Settle_Date' , sum(fee) as 'Total_Amount' from base where base.CourseID =  " + CourseId + "   and                                  " +
                        //"  base.Settle_Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE)   group by  base.state_name,base.Settle_Date  order by  base.state_name,base.Settle_Date");
                    }
                    else
                    {

                        mySql.Append("with base as (select dn.ID, dn.Amount, t.Date from Demand_Note dn join CSC_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                                "  , base2 as (                                                                                                                                                                                                       " +
                                "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.Date Settle_Date, cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                                "      FROM                                                                                                                                                                                                           " +
                                "      Course_Exam_VAF_ACF  cvcf                                                                                                                                                             " +
                                "      inner join  Institute ins                                                                                                                                                             " +
                                "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                                "      inner join  Location loc                                                                                                                                                              " +
                                "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                                "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                                "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount'                                                                                                                 " +
                                "  from base2                                                                                                                                                                                                         " +
                                "  where CourseID = @CourseId and Settle_Date is not null                                                                                                                    " +
                                "  group by  state_name, Settle_Date order by state_name, Online_Settle_Date");
                        //    mySql.Append("with base as (SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, dn.[ID] as demandnote, cast(ot.[date] as date) Settle_Date, cvcf.[TotalFee_Amount]   as fee                                  " +
                        //"      , cvcf.[Course_ID] as CourseID FROM [NIELIT_PREPROD].[dbo].[Course_Exam_VAF_ACF]  cvcf  inner join  [NIELIT_PREPROD].[dbo].[Institute] ins  on cvcf.Institute_ID = ins.ID                                                     " +
                        //"  inner join  [NIELIT_PREPROD].[dbo].[Location] loc  on ins.State_ID = loc.id inner join  [NIELIT_PREPROD].[dbo].[Demand_Note] dn  on dn.ID = cvcf.Demand_Note_ID   inner join  [NIELIT_PREPROD].[dbo].[CSC_Transaction] ot         " +
                        //"      on ot.ID = dn.Online_Transaction_ID )select base.state_name as 'State_Name',base.Settle_Date as 'CSC_Payment_Settle_Date' , sum(fee) as 'Total_Amount' from base where base.CourseID =  " + CourseId + "  " +
                        //  "  group by  base.state_name,base.Settle_Date  order by  base.state_name,base.Settle_Date");
                    }
                }
            }
            #endregion
            else if (CourseCode == "O" || CourseCode == "A" || CourseCode == "B")
            {
                if (ddlAppType.SelectedItem.Text == "VAF ACF")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        mySql.Append("with base as (select dn.ID, dn.Amount, t.Date from Demand_Note dn join CSC_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                                "  , base2 as (                                                                                                                                                                                                       " +
                                "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.Date Settle_Date, cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                                "      FROM                                                                                                                                                                                                           " +
                                "      Course_Exam_VAF_ACF  cvcf                                                                                                                                                             " +
                                "      inner join  Institute ins                                                                                                                                                             " +
                                "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                                "      inner join  Location loc                                                                                                                                                              " +
                                "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                                "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                                "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount'                                                                                                                 " +
                                "  from base2                                                                                                                                                                                                         " +
                                "  where CourseID = @CourseId and Settle_Date >= CAST(@fromDate as DATE)                                                                                                                     " +
                                "  group by  state_name, Settle_Date order by state_name, Online_Settle_Date");
                        //    mySql.Append("with base as (SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, dn.[ID] as demandnote, cast(ot.[date] as date) Settle_Date, cvcf.[TotalFee_Amount]   as fee                                  " +
                        //"      , cvcf.[Course_ID] as CourseID FROM [NIELIT_PREPROD].[dbo].[Course_Exam_VAF_ACF]  cvcf  inner join  [NIELIT_PREPROD].[dbo].[Institute] ins  on cvcf.Institute_ID = ins.ID                                                     " +
                        //"  inner join  [NIELIT_PREPROD].[dbo].[Location] loc  on ins.State_ID = loc.id inner join  [NIELIT_PREPROD].[dbo].[Demand_Note] dn  on dn.ID = cvcf.Demand_Note_ID   inner join  [NIELIT_PREPROD].[dbo].[CSC_Transaction] ot         " +
                        //"      on ot.ID = dn.Online_Transaction_ID )select base.state_name as 'State_Name',base.Settle_Date as 'CSC_Payment_Settle_Date' , sum(fee) as 'Total_Amount' from base where base.CourseID =  " + CourseId + "   and                                  " +
                        //"  base.Settle_Date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE)   group by  base.state_name,base.Settle_Date  order by  base.state_name,base.Settle_Date");
                    }
                    else
                    {
                        mySql.Append("with base as (select dn.ID, dn.Amount, t.Date from Demand_Note dn join CSC_Transaction t on dn.ID=t.Demand_Note_ID )                                                                                    " +
                                "  , base2 as (                                                                                                                                                                                                       " +
                                "      SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, b.[ID] as demandnote, b.Date Settle_Date, cvcf.[TotalFee_Amount]   as fee , cvcf.[Course_ID] as CourseID                        " +
                                "      FROM                                                                                                                                                                                                           " +
                                "      Course_Exam_VAF_ACF cvcf                                                                                                                                                             " +
                                "      inner join  Institute ins                                                                                                                                                             " +
                                "      on cvcf.Institute_ID = ins.ID                                                                                                                                                                                  " +
                                "      inner join  Location loc                                                                                                                                                              " +
                                "      on ins.State_ID = loc.id                                                                                                                                                                                       " +
                                "      inner join base b on b.ID = cvcf.Demand_Note_ID)                                                                                                                                                               " +
                                "  select state_name as 'State_Name', Settle_Date as 'Online_Settle_Date' ,sum(fee) as 'Total_Amount'                                                                                                                 " +
                                "  from base2                                                                                                                                                                                                         " +
                                "  where CourseID = @CourseId and Settle_Date is not null                                                                                                                     " +
                                "  group by  state_name, Settle_Date order by state_name, Online_Settle_Date");
                        //   mySql.Append("with base as (SELECT distinct  loc.Name as state_name, cvcf.Institute_ID as institute, dn.[ID] as demandnote, cast(ot.[date] as date) Settle_Date, cvcf.[TotalFee_Amount]   as fee                                  " +
                        //"      , cvcf.[Course_ID] as CourseID FROM [NIELIT_PREPROD].[dbo].[Course_Exam_VAF_ACF]  cvcf  inner join  [NIELIT_PREPROD].[dbo].[Institute] ins  on cvcf.Institute_ID = ins.ID                                                     " +
                        //"  inner join  [NIELIT_PREPROD].[dbo].[Location] loc  on ins.State_ID = loc.id inner join  [NIELIT_PREPROD].[dbo].[Demand_Note] dn  on dn.ID = cvcf.Demand_Note_ID   inner join  [NIELIT_PREPROD].[dbo].[CSC_Transaction] ot         " +
                        //"      on ot.ID = dn.Online_Transaction_ID )select base.state_name as 'State_Name',base.Settle_Date as 'CSC_Payment_Settle_Date' , sum(fee) as 'Total_Amount' from base where base.CourseID =  " + CourseId + "  " +
                        //  "  group by  base.state_name,base.Settle_Date  order by  base.state_name,base.Settle_Date");
                    }
                }
            }
            //vaf acf finish -----------------------------------

            #region NSQF Courses
            if (CourseCatId == 6)
            {
                if (ddlAppType.SelectedItem.Text == "Registration")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                                        "from CSC_Transaction a, Demand_Note b,Course_Registration_Application c, Location e   " +
                                        "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and  a.Response_Message ='success' " +
                                        "and a.date >= CAST(@fromDate  as DATE) group by e.Name , cast (a.[date] as date) order by cast (a.[date] as date) desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                        //                "from CSC_Transaction a, Demand_Note b,Course_Registration_Application c, Location e   " +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeRegn + "' and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and  a.Response_Message ='success' " +
                        //                "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name , cast (a.[date] as date) order by cast (a.[date] as date) desc");
                    }
                    else
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                                       "from CSC_Transaction a, Demand_Note b, Course_Registration_Application c, Location e   " +
                                       "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and  a.Response_Message ='success' " +
                                       "group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                        //                "from CSC_Transaction a, Demand_Note b, Course_Registration_Application c, Location e   " +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeRegn + "' and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and  a.Response_Message ='success' " +
                        //                "group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) desc");
                    }

                }
                else if (ddlAppType.SelectedItem.Text == "Examination")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(CE.Fee_Amount)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                                        "from CSC_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e   " +
                                        "where a.Demand_Note_ID =b.ID and b.ServiceID =@ServiceCodeExam  and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID " +
                                        "AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number  and  a.Response_Message ='success'	 " +
                                        "and a.date >= CAST(@fromDate  as DATE) group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) ");
                        //mySql.Append("select e.Name as 'State_Name', sum(CE.Fee_Amount)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                        //                "from CSC_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e   " +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID " +
                        //                "AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number  and  a.Response_Message ='success'	 " +
                        //                "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) ");
                    }
                    else
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(CE.Fee_Amount)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                                        "from CSC_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e   " +
                                        "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID " +
                                        "AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number  and  a.Response_Message ='success'	 " +
                                        "group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) ");
                        //mySql.Append("select e.Name as 'State_Name', sum(CE.Fee_Amount)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                        //                "from CSC_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e   " +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID " +
                        //                "AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number  and  a.Response_Message ='success'	 " +
                        //                "group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) ");
                    }
                }

            }


            #endregion
            ///Added 24 May 2024
            #region CHMT-O Courses
            else if (CourseCatId == 3)
            {
                if (ddlAppType.SelectedItem.Text == "Registration")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                                        "from CSC_Transaction a, Demand_Note b,Course_Registration_Application c, Location e   " +
                                        "where a.Demand_Note_ID =b.ID and b.ServiceID =     @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and  a.Response_Message ='success' " +
                                        "and a.date >= CAST(@fromDate  as DATE) group by e.Name , cast (a.[date] as date) order by cast (a.[date] as date) desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                        //                "from CSC_Transaction a, Demand_Note b,Course_Registration_Application c, Location e   " +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeRegn + "' and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and  a.Response_Message ='success' " +
                        //                "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name , cast (a.[date] as date) order by cast (a.[date] as date) desc");
                    }
                    else
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                                        "from CSC_Transaction a, Demand_Note b, Course_Registration_Application c, Location e   " +
                                        "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeRegn and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and  a.Response_Message ='success' " +
                                        "group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) desc");
                        //mySql.Append("select e.Name as 'State_Name', sum(c.Fee_Amt)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                        //                "from CSC_Transaction a, Demand_Note b, Course_Registration_Application c, Location e   " +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeRegn + "' and a.Demand_Note_ID =c.Demand_Note_ID  and C.Cor_State_ID =e.ID and  a.Response_Message ='success' " +
                        //                "group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) desc");
                    }

                }
                else if (ddlAppType.SelectedItem.Text == "Examination")
                {
                    if (ddlDateRange.SelectedItem.Text == "From Start Date")
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(CE.Fee_Amount)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                                        "from CSC_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e   " +
                                        "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID " +
                                        "AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number  and  a.Response_Message ='success' " +
                                        "and a.date >= CAST(@fromDate  as DATE) group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) ");
                        //mySql.Append("select e.Name as 'State_Name', sum(CE.Fee_Amount)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                        //                "from CSC_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e   " +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID " +
                        //                "AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number  and  a.Response_Message ='success' " +
                        //                "and a.date >= CAST('" + FromDate.ToString("dd-MMM-yyyy") + "' as DATE) group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) ");
                    }
                    else
                    {
                        mySql.Append("select e.Name as 'State_Name', sum(CE.Fee_Amount)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                                        "from CSC_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e   " +
                                        "where a.Demand_Note_ID =b.ID and b.ServiceID = @ServiceCodeExam and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID " +
                                        "AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number  and  a.Response_Message ='success' " +
                                        "group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) ");
                        //mySql.Append("select e.Name as 'State_Name', sum(CE.Fee_Amount)  as 'Total_Amount', cast (a.[date] as date) as 'CSC_Payment_Settle_Date' " +
                        //                "from CSC_Transaction a, Demand_Note b,Course_Exam_Application c,Course_Exam_Application_Detail CE, Exam_Center  d, Location e   " +
                        //                "where a.Demand_Note_ID =b.ID and b.ServiceID ='" + ServiceCodeExam + "' and a.Demand_Note_ID =c.Demand_Note_ID and c.Exam_Center1_ID  =d.ID and d.State_ID =e.ID " +
                        //                "AND C.Exam_ID =CE.Exam_ID AND C.Registration_Number =CE.Registration_Number  and  a.Response_Message ='success' " +
                        //                "group by e.Name, cast (a.[date] as date) order by cast (a.[date] as date) ");
                    }
                }
            }
            #endregion
            /////////////End 24 May 2024
        }
        #endregion
        dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), parameters, CommandType.Text, false);
        // dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
        string sheetname = "GST_" + ddlpaymentmode.SelectedItem.Text.Replace("/", "_") + "_" + ddlCourseName.SelectedItem.Text.Replace("/", "_") + "_" + DateTime.Now;
        if (CourseCatId == 1)
        {
            if (ddlAppType.SelectedItem.Text == "Registration")
                sheetname = "GST_" + ddlpaymentmode.SelectedItem.Text.Replace("/", "_") + "_" + ddlCourseName.SelectedItem.Text.Replace("/", "_") + "_Regn_" + DateTime.Now;
            else if (ddlAppType.SelectedItem.Text == "Examination")
                sheetname = "GST_" + ddlpaymentmode.SelectedItem.Text.Replace("/", "_") + "_" + ddlCourseName.SelectedItem.Text.Replace("/", "_") + "_Exam_" + DateTime.Now;
            else if (ddlAppType.SelectedItem.Text == "VAF ACF")
                sheetname = "GST_" + ddlpaymentmode.SelectedItem.Text.Replace("/", "_") + "_" + ddlCourseName.SelectedItem.Text.Replace("/", "_") + "_VAF_ACF_" + DateTime.Now;
        }
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



    protected void ddlpaymentmode_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlpaymentmode.SelectedValue == "2")
            {
                // added by amit start
                ddlgateway.Visible = true;
                // added by amit end
            }
            else
            {
                //added by amit start
                ddlgateway.Visible = false;
                ddlgateway.ClearSelection();
                //added by amit end
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

}