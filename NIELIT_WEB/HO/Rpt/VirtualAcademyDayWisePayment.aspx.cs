using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Linq;
using EConnect.Utils.Common;
using System.Collections.Generic;
using System.Web;
using System.Data.Objects;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

public partial class HO_Rpt_VirtualAcademyDayWisePayment : BasePage
    {
    string courseName = "";
    string courseCatName = "";
    SqlConnection con;
    SqlCommand com;
    SqlDataAdapter da;
    DataSet ds;
    Table tbl = new Table();    
    DataRow dtRow;
    public static int j;
    String PayStatusId = "0";
    String DateType = "0";
    Int32 currentRoleId = 0;
    int PayModeId = 0;
    Int32 practicalfeetype = Convert.ToInt32(enmFeeType.PracticalFee);
    Int32 theoryfeetype = Convert.ToInt32(enmFeeType.ExaminationFee);
    Int32 improvementfeetype = Convert.ToInt32(enmFeeType.ImprovementOfPaperFee);
    Int32 processingfeetype = Convert.ToInt32(enmFeeType.PostageFeeChargedTowardExaminationCorrespondence);


    protected void Page_Load(object sender, EventArgs e)
        {
        lblError.Visible = false;
        try
            {
            if (!IsSessionAlive())
                {
                Response.Redirect("~/index.aspx");
                }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/VirtualAcademyDayWisePaymentReport.aspx"))
                {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
                }
            if (!Page.IsPostBack)
                {
                //int CourseCatId = Convert.ToInt32(Request.QueryString["CourseCatId"]);
                newShowData();
                tbl.CssClass = "sample3";
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(100); ;
                divReportData.Controls.Add(tbl);
                }
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }
    protected void ShowTableHeader()
        {
        try
            {
            int AppTypeId = Convert.ToInt32(Request.QueryString["AppTypeId"]);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(2);
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            tcCol1.Text = "#";
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol15 = new TableHeaderCell();
            tcCol15.Width = Unit.Percentage(15);
            tcCol15.HorizontalAlign = HorizontalAlign.Center;
            tcCol15.Text = "Course";
            th.Cells.Add(tcCol15);

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(6);
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            tcCol.Text = "Payment Date";
            th.Cells.Add(tcCol);

            if (PayModeId == Convert.ToInt32(enmPaymentMode.Online))
                {
                TableHeaderCell tcCol16 = new TableHeaderCell();
                tcCol16.Width = Unit.Percentage(6);
                tcCol16.HorizontalAlign = HorizontalAlign.Center;
                tcCol16.Text = "Settled Date";
                th.Cells.Add(tcCol16);
                }

            //if (PayModeId == Convert.ToInt32(enmPaymentMode.NEFTRTGS))
            //    {
            //    TableHeaderCell tcCol16 = new TableHeaderCell();
            //    tcCol16.Width = Unit.Percentage(12);
            //    tcCol16.HorizontalAlign = HorizontalAlign.Center;
            //    tcCol16.Text = "Verified Date";
            //    th.Cells.Add(tcCol16);
            //    }

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(5);
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            tcCol2.Text = "No of Candidates";
            th.Cells.Add(tcCol2);

            //if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            //    {
            //    TableHeaderCell tcCol3 = new TableHeaderCell();
            //    tcCol3.HorizontalAlign = HorizontalAlign.Center;
            //    tcCol3.Width = Unit.Percentage(5);
            //    tcCol3.Text = "No of Theory Modules";
            //    th.Cells.Add(tcCol3);

            //    TableHeaderCell tcCol4 = new TableHeaderCell();
            //    tcCol4.HorizontalAlign = HorizontalAlign.Center;
            //    tcCol4.Width = Unit.Percentage(5);
            //    tcCol4.Text = "No of Practical Modules";
            //    th.Cells.Add(tcCol4);

            //    TableHeaderCell tcCol24 = new TableHeaderCell();
            //    tcCol24.HorizontalAlign = HorizontalAlign.Center;
            //    tcCol24.Width = Unit.Percentage(10);
            //    tcCol24.Text = "Theory Amount (Rs.)";
            //    th.Cells.Add(tcCol24);

            //    TableHeaderCell tcCol25 = new TableHeaderCell();
            //    tcCol25.HorizontalAlign = HorizontalAlign.Center;
            //    tcCol25.Width = Unit.Percentage(10);
            //    tcCol25.Text = "Practical Amount (Rs.)";
            //    th.Cells.Add(tcCol25);

            //    TableHeaderCell tcCol26 = new TableHeaderCell();
            //    tcCol26.HorizontalAlign = HorizontalAlign.Center;
            //    tcCol26.Width = Unit.Percentage(10);
            //    tcCol26.Text = "Processing Amount (Rs.)";
            //    th.Cells.Add(tcCol26);

            //    TableHeaderCell tcCol28 = new TableHeaderCell();
            //    tcCol28.Width = Unit.Percentage(10);
            //    tcCol28.HorizontalAlign = HorizontalAlign.Center;
            //    tcCol28.Text = "Late Fee Amount (Rs.)";
            //    th.Cells.Add(tcCol28);
            //    }



            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(10);
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            tcCol5.Text = "Total Fee Amount (Rs.)";
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(12);
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            tcCol9.Text = "Payment Status";
            th.Cells.Add(tcCol9);

            tbl.Rows.Add(th);
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }

        }
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
        {
        try
            {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
            divReportData.Visible = true;
            newShowData();
            divReportData.Controls.Add(tbl);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=VirtualAcademyDayWisePaymentReport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            htmlWrite = new Html32TextWriter(StringWrite);
            divReportData.RenderControl(htmlWrite);
            Response.Write(StringWrite.ToString());
            Response.End();
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }

        }
    protected void newShowData()
        {
        EConnectContext context = new EConnectContext(); ;
        NIELITMISContext contextmis = new NIELITMISContext();
        try
            {
            int i = 0;
            Decimal totamt = 0;
            Int64 noofCandidates = 0;
            Int64 noofTheoryModules = 0;
            Int64 noofPracticalModules = 0;
            Decimal TotaltheoryFee = 0;
            Decimal TotalPracticalFee = 0;
            Decimal TotalProcessingFee = 0;
            Decimal TotalLateFee = 0;
            Decimal TotalAmount = 0;
            DataTable dt;
            PayModeId = Convert.ToInt32(Request.QueryString["PayModeId"]);
            enmPaymentMode paymentMode = (enmPaymentMode)PayModeId;
            int TransTypeId = Convert.ToInt32(Request.QueryString["TransTypeId"]);
            int CourseCatId = Convert.ToInt32(Request.QueryString["CourseCatId"]);
            int CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            int AppTypeId = Convert.ToInt32(Request.QueryString["AppTypeId"]);
            //Int64 transId = Convert.ToInt64(Request.QueryString["Transno"]);
            enmApplicationType applicationType = (enmApplicationType)AppTypeId;
            PayStatusId = Request.QueryString["PayStatusId"];
            DateType = Request.QueryString["Datetype"];
            DateTime PayFromDate = Convert.ToDateTime(Request.QueryString["PayFromDate"]);
            DateTime PayToDate = Convert.ToDateTime(Request.QueryString["PayToDate"]);
            string strHead = "";
            string strHead1 = "";
            StringBuilder mySql = new StringBuilder();
            StringBuilder mySql1 = new StringBuilder();
            if (DateType != "0")
                {
                if (DateType == "SV")
                    {
                    if (PayModeId == Convert.ToInt32(enmPaymentMode.Online))
                        strHead += "</br><b> Settled Date From :</b>" + PayFromDate.ToString("dd-MMM-yyyy") + " to " + PayToDate.ToString("dd-MMM-yyyy");
                    else if (PayModeId == Convert.ToInt32(enmPaymentMode.NEFTRTGS))
                        strHead += "</br><b> Verified Date From :</b>" + PayFromDate.ToString("dd-MMM-yyyy") + " to " + PayToDate.ToString("dd-MMM-yyyy");
                    }
                else
                    strHead += "</br><b> Payment Date From :</b>" + PayFromDate.ToString("dd-MMM-yyyy") + " to " + PayToDate.ToString("dd-MMM-yyyy");
                }
            else
                {
                strHead += "</br><b> Payment Date From :</b>" + PayFromDate.ToString("dd-MMM-yyyy") + " to " + PayToDate.ToString("dd-MMM-yyyy");
                }
            strHead += "<br/> <b>Payment Mode :  </b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.enmPaymentMode)(PayModeId)).ToString();
            strHead += "<br/> <b>Application Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicationType)(AppTypeId)).ToString();
            if (PayStatusId == "0")
                strHead += "<br/> <b>Payment Status :</b> All";
            else if (PayStatusId.Trim().ToUpper() == "F".ToUpper().Trim())
                strHead += "<br/> <b>Payment Status :</b> Failed";
            else if (PayStatusId.Trim().ToUpper() == "S".ToUpper().Trim())
                strHead += "<br/> <b>Payment Status :</b> Success";

            // Course Category
            if (CourseCatId != 0)
                {
                var category = context.CourseCategories.Find(CourseCatId);
                if (category != null)
                    {
                    courseCatName = category.Name.ToString();
                    strHead += "<br/><b>Course Category :</b>" + courseCatName;
                    }
                else
                    {
                    var category1 = contextmis.NielitCentreCourseCategorys.Find(CourseCatId);
                    if (category1 != null)
                        {
                        courseCatName = category1.Name.ToString();
                        strHead += "<br/><b>Course Category :</b>" + courseCatName;
                        }
                    }
                }
            else
                {
                strHead += "<br/><b>Course Category :</b> All";
                }

            // Course
            if (CourseId != 0)
                {
                //var courses = context.Courses.Find(CourseId);
                var course = contextmis.NielitCourseDurations.Find(CourseId);
                if (course != null)
                    {
                    Int32 courses1 = course.courseID;
                    var courses = context.Courses.Find(courses1);
                    if (courses != null)
                        {
                        courseName = courses.Name.ToString();
                        strHead += "<br/><b>Course Name :</b>" + courseName;
                        }
                    else
                        {
                        var mm = contextmis.NielitCentreCourses.Find(courses1);
                        if (mm != null)
                            {
                            courseName = mm.Name.ToString();
                            strHead += "<br/><b>Course Name :</b>" + courseName;
                            }
                        //var mm = (from p in contextmis.NielitCentreCourses
                        //          join q in contextmis.NielitCourseDurations on p.ID equals q.courseID
                        //          where q.ID == courses1
                        //          select new { id = p.ID, cName1 = p.Name }).FirstOrDefault();
                        //if (mm != null)
                        //    {
                        //    courseName = mm.cName1.ToString();
                        //    strHead += "<br/><b>Course Name :</b>" + courseName;
                        //    }
                        }
                    }
                else
                    {
                    strHead += "<br/><b>Course Name :</b> Not Found";
                    }
                }
            else
                {
                strHead += "<br/><b>Course Name :</b> All";
                }

            if (TransTypeId == 0)
                strHead += "<br/><b>Transaction Type :</b>  All";
            else if (TransTypeId == 1)
                strHead += "<br/><b>Transaction Type :</b> Single";
            else if (TransTypeId == 2)
                strHead += "<br/><b>Transaction Type :</b> Multiple";
            if (paymentMode == enmPaymentMode.Online)
                {
                if (DateType == "P")
                    {
                    strHead += "<br/><b> Date Type :</b> Payment Date";
                    }
                if (DateType == "SV")
                    {
                    strHead += "<br/><b> Date Type :</b> Settled Date";
                    }

                if (CourseCatId != 0)
                    {
                    string paymentDate = "";
                    string settledDate = "";
                    string trTypeId = "";
                    string CourseId1 = "";
                    string CourseCatId1 = "";
                    string PayStatusId1 = "";
                    var category = context.CourseCategories.Find(CourseCatId);
                    if (category != null)
                        {

                        if (applicationType == enmApplicationType.CourseRegistrationApplication)
                            {
                            #region NIELIT_DB
                            con = new SqlConnection();
                            con.ConnectionString = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
                            con.Open();
                            if (DateType != "0")
                                {
                                if (DateType == "P") { paymentDate = " and CAST(T.Request_Date as DATE) >=@PayFromDate and CAST(T.Request_Date as DATE) <=@PayToDate "; }
                                if (DateType == "SV") { settledDate = " and CAST(T.Settled_On as DATE) >=@PayFromDate and CAST(T.Settled_On as DATE) <=@PayToDate "; }
                                }
                            if (TransTypeId != 0)
                                {
                                trTypeId = " and D.Demand_Note_Type_ID =@TransTypeId ";
                                }
                            if (CourseId != 0)
                                {
                                CourseId1 = " and C.CourseDurationID =@CourseId ";
                                }
                            if (CourseCatId != 0)
                                {
                                CourseCatId1 = "and C.Course_Category_ID =@CourseCatId ";
                                }

                            if (PayStatusId != "0")
                                {
                                if (PayStatusId == "S")
                                    {
                                    PayStatusId1 = " and T.Response_Status_Code = '0300' ";
                                    }
                                if (PayStatusId == "F")
                                    {
                                    PayStatusId1 = "and (T.Response_Status_Code <> '0300' or T.Response_Status_Code is NULL )";
                                    }
                                }

                            string qs = " select  CAST(T.Request_Date as DATE) as TransactionDate, COUNT(*) as Count, C.CourseDurationID as coursecode, CAST(T.Settled_On as DATE) as SettledDate, sum(C.Fee_Amt) as FeeAmount, T.Response_Status_Message as StatusMessage" +
                            " from [NIELITMIS].[dbo].[virtualAcademyOnline_Transaction] T,  [NIELITMIS].[dbo].[virtualAcademyDemand_Note] D, [NIELITMIS].[dbo].[NielitCourseDuration] R, [NIELITMIS].[dbo].[virtualAcademyRegistration] C " +
                            "Where  D.ID = T.Demand_Note_ID and C.Demand_Note_ID = D.ID and C.CourseDurationID = R.ID  and T.Amount=D.Amount and C.Fee_Amt=D.Amount and C.Fee_Amt >0   and T.Demand_Note_ID = D.ID " +  // Added on 21.07.2022 to avoid the zero fee candidates
                            " and D.Application_Type_ID =@AppTypeId " +
                             paymentDate + settledDate + trTypeId + CourseId1 + CourseCatId1 + PayStatusId1 +
                            " group by CAST(T.Request_Date as DATE) , T.Response_Status_Message, C.CourseDurationID, CAST(T.Settled_On as DATE) order by  CAST(T.Request_Date as DATE)";

                            //For NIELITMIS_LOCALTEST Testing
                            //string qs = " select  CAST(T.Request_Date as DATE) as TransactionDate, COUNT(*) as Count, C.CourseDurationID as coursecode, CAST(T.Settled_On as DATE) as SettledDate, sum(C.Fee_Amt) as FeeAmount, T.Response_Status_Message as StatusMessage" +
                            //" from [NIELITMIS_LOCALTEST].[dbo].[virtualAcademyOnline_Transaction] T,  [NIELITMIS_LOCALTEST].[dbo].[virtualAcademyDemand_Note] D, [NIELITMIS_LOCALTEST].[dbo].[NielitCourseDuration] R, [NIELITMIS_LOCALTEST].[dbo].[virtualAcademyRegistration] C " +
                            //"Where  D.ID = T.Demand_Note_ID and C.Demand_Note_ID = D.ID and C.CourseDurationID = R.ID and T.Demand_Note_ID = D.ID " +
                            //" and D.Application_Type_ID =@AppTypeId " +
                            // paymentDate + settledDate + trTypeId + CourseId1 + CourseCatId1 + PayStatusId1 +
                            //" group by CAST(T.Request_Date as DATE) , T.Response_Status_Message, C.CourseDurationID, CAST(T.Settled_On as DATE) order by  CAST(T.Request_Date as DATE)";

                            com = new SqlCommand(qs, con);

                            //Add parameters
                            com.Parameters.AddWithValue("AppTypeId", AppTypeId);
                            if (DateType != "0")
                                {
                                if (DateType == "P")
                                    {
                                    com.Parameters.AddWithValue("PayFromDate", PayFromDate);
                                    com.Parameters.AddWithValue("PayToDate", PayToDate);
                                    }
                                if (DateType == "SV")
                                    {
                                    com.Parameters.AddWithValue("PayFromDate", PayFromDate);
                                    com.Parameters.AddWithValue("PayToDate", PayToDate);
                                    }
                                }
                            if (TransTypeId != 0)
                                {
                                com.Parameters.AddWithValue("TransTypeId", TransTypeId);
                                }
                            if (CourseId != 0)
                                {
                                com.Parameters.AddWithValue("CourseId", CourseId);
                                }
                            if (CourseCatId != 0)
                                {
                                com.Parameters.AddWithValue("CourseCatId", CourseCatId);
                                }
                            if (com.Connection.State == ConnectionState.Closed)
                                {
                                com.Connection.Open();
                                }
                            da = new SqlDataAdapter(com);
                            ds = new DataSet();
                            da.Fill(ds);
                        #endregion
                            }
                        }
                    else
                        {
                        var category1 = contextmis.NielitCentreCourseCategorys.Find(CourseCatId);
                        if (category1 != null)
                            {
                            if (applicationType == enmApplicationType.CourseRegistrationApplication)
                                {
                                #region NIELITMIS_DB
                                con = new SqlConnection();
                                con.ConnectionString = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
                                con.Open();
                                if (DateType != "0")
                                    {
                                    if (DateType == "P") { paymentDate = " and CAST(T.Request_Date as DATE) >=@PayFromDate and CAST(T.Request_Date as DATE) <=@PayToDate "; }
                                    if (DateType == "SV") { settledDate = " and CAST(T.Settled_On as DATE) >=@PayFromDate and CAST(T.Settled_On as DATE) <=@PayToDate "; }
                                    }
                                if (TransTypeId != 0)
                                    {
                                    trTypeId = " and D.Demand_Note_Type_ID =@TransTypeId ";
                                    }
                                if (CourseId != 0)
                                    {
                                    CourseId1 = " and C.CourseDurationID =@CourseId ";
                                    }
                                if (CourseCatId != 0)
                                    {
                                    CourseCatId1 = "and C.Course_Category_ID =@CourseCatId ";
                                    }

                                if (PayStatusId != "0")
                                    {
                                    if (PayStatusId == "S")
                                        {
                                        PayStatusId1 = " and T.Response_Status_Code = '0300' ";
                                        }
                                    if (PayStatusId == "F")
                                        {
                                        PayStatusId1 = "and (T.Response_Status_Code <> '0300' or T.Response_Status_Code is NULL )";
                                        }
                                    }
                                //
                                string qs = " select  CAST(T.Request_Date as DATE) as TransactionDate, COUNT(*) as Count, C.CourseDurationID as coursecode, CAST(T.Settled_On as DATE) as SettledDate, sum(C.Fee_Amt) as FeeAmount, T.Response_Status_Message as StatusMessage" +
                                " from [NIELITMIS].[dbo].[virtualAcademyOnline_Transaction] T,  [NIELITMIS].[dbo].[virtualAcademyDemand_Note] D, [NIELITMIS].[dbo].[NielitCourseDuration] R, [NIELITMIS].[dbo].[virtualAcademyRegistration] C " +
                                "Where  D.ID = T.Demand_Note_ID and C.Demand_Note_ID = D.ID and C.CourseDurationID = R.ID  and T.Amount=D.Amount and C.Fee_Amt=D.Amount and C.Fee_Amt >0   and T.Demand_Note_ID = D.ID " +  // Added on 21.07.2022 to avoid the zero fee candidates
                                " and D.Application_Type_ID =@AppTypeId " +
                                 paymentDate + settledDate + trTypeId + CourseId1 + CourseCatId1 + PayStatusId1 +
                                " group by CAST(T.Request_Date as DATE) , T.Response_Status_Message, C.CourseDurationID, CAST(T.Settled_On as DATE) order by  CAST(T.Request_Date as DATE)";
                                com = new SqlCommand(qs, con);

                                //Add parameters
                                com.Parameters.AddWithValue("AppTypeId", AppTypeId);
                                if (DateType != "0")
                                    {
                                    if (DateType == "P")
                                        {
                                        com.Parameters.AddWithValue("PayFromDate", PayFromDate);
                                        com.Parameters.AddWithValue("PayToDate", PayToDate);
                                        }
                                    if (DateType == "SV")
                                        {
                                        com.Parameters.AddWithValue("PayFromDate", PayFromDate);
                                        com.Parameters.AddWithValue("PayToDate", PayToDate);
                                        }
                                    }
                                if (TransTypeId != 0)
                                    {
                                    com.Parameters.AddWithValue("TransTypeId", TransTypeId);
                                    }
                                if (CourseId != 0)
                                    {
                                    com.Parameters.AddWithValue("CourseId", CourseId);
                                    }
                                if (CourseCatId != 0)
                                    {
                                    com.Parameters.AddWithValue("CourseCatId", CourseCatId);
                                    }
                                if (com.Connection.State == ConnectionState.Closed)
                                    {
                                    com.Connection.Open();
                                    }
                                da = new SqlDataAdapter(com);
                                ds = new DataSet();
                                da.Fill(ds);
                            #endregion
                                }
                            }
                        }

                    if (ds.Tables[0].Rows.Count > 0)
                        {
                        ShowTableHeader();
                        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                            TableRow tr = new TableRow();
                            if (i % 2 == 0)
                                tr.CssClass = "gdalternate1";
                            else
                                tr.CssClass = "gdrow1";

                            TableHeaderCell tcCol1 = new TableHeaderCell();
                            tcCol1.Width = Unit.Percentage(2);
                            tcCol1.HorizontalAlign = HorizontalAlign.Left;
                            tcCol1.Text = i.ToString();
                            tr.Cells.Add(tcCol1);

                            TableCell tcCol25 = new TableCell();
                            tcCol25.Width = Unit.Percentage(13);
                            tcCol25.HorizontalAlign = HorizontalAlign.Center;
                            //tcCol25.Text = ""; // vishal// EConnect.Utils.Data.DbUtility.ExecuteScaller("select code from course where id=" + dtRow["coursecode"].ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false).ToString();
                            tcCol25.Text = courseName;
                            tr.Cells.Add(tcCol25);


                            TableCell tcCol = new TableCell();
                            tcCol.Width = Unit.Percentage(5);
                            tcCol.HorizontalAlign = HorizontalAlign.Center;
                            tcCol.Text = Convert.ToDateTime(ds.Tables[0].Rows[i]["TransactionDate"]).ToString("dd-MMM-yyyy");
                            tr.Cells.Add(tcCol);

                            TableCell tcCol16 = new TableCell();
                            tcCol16.Width = Unit.Percentage(5);
                            tcCol16.HorizontalAlign = HorizontalAlign.Center;
                            tcCol16.Text = String.IsNullOrEmpty(ds.Tables[0].Rows[i]["SettledDate"].ToString()) ? "NA" : Convert.ToDateTime(ds.Tables[0].Rows[i]["SettledDate"]).ToString("dd-MMM-yyyy");
                            tr.Cells.Add(tcCol16);

                            TableCell tcCol2 = new TableCell();
                            tcCol2.Width = Unit.Percentage(8);
                            tcCol2.HorizontalAlign = HorizontalAlign.Right;
                            tcCol2.Text = ds.Tables[0].Rows[i]["Count"].ToString();
                            noofCandidates += Convert.ToInt64(ds.Tables[0].Rows[i]["Count"]);
                            tr.Cells.Add(tcCol2);

                            TableCell tcCol8 = new TableCell();
                            tcCol8.Width = Unit.Percentage(8);
                            tcCol8.HorizontalAlign = HorizontalAlign.Right;
                            tcCol8.Text = Convert.ToInt64(ds.Tables[0].Rows[i]["FeeAmount"]).ToString("F");
                            TotalAmount += Convert.ToDecimal(tcCol8.Text);
                            tr.Cells.Add(tcCol8);


                            TableCell tcCol9 = new TableCell();
                            tcCol9.Width = Unit.Percentage(18);
                            tcCol9.HorizontalAlign = HorizontalAlign.Left;
                            tcCol9.Text = (String.IsNullOrEmpty(ds.Tables[0].Rows[i]["StatusMessage"].ToString()) ? "Failed" : ds.Tables[0].Rows[i]["StatusMessage"].ToString());
                            tr.Cells.Add(tcCol9);

                            tbl.Rows.Add(tr);

                            if (ds.Tables[0].Rows[i]["StatusMessage"].ToString().ToUpper().Trim() == "Success".ToUpper().Trim())
                                {
                                totamt += Convert.ToDecimal(ds.Tables[0].Rows[i]["FeeAmount"]);
                                }
                            }

                        //showing total
                        TableRow trNew = new TableRow();
                        if (i % 2 == 0)
                            trNew.CssClass = "gdalternate1";
                        else
                            trNew.CssClass = "gdrow1";

                        TableCell tdNewRow1 = new TableCell();
                        tdNewRow1.Width = Unit.Percentage(1);
                        tdNewRow1.Text = "<b>Total</b>";
                        tdNewRow1.ColumnSpan = 4;
                        tdNewRow1.HorizontalAlign = HorizontalAlign.Center;
                        trNew.Cells.Add(tdNewRow1);

                        TableCell tdNewRow5 = new TableCell();
                        tdNewRow5.Width = Unit.Percentage(1);
                        tdNewRow5.Text = "<b>" + noofCandidates.ToString() + "</b>";
                        tdNewRow5.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow5);

                        TableCell tdNewRow12 = new TableCell();
                        tdNewRow12.Width = Unit.Percentage(1);
                        tdNewRow12.Text = "<b>" + TotalAmount.ToString() + "</b>";
                        tdNewRow12.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow12);

                        TableCell tdNewRow13 = new TableCell();
                        tdNewRow13.Width = Unit.Percentage(1);
                        tdNewRow13.Text = "";
                        tdNewRow13.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow13);

                        tbl.Rows.Add(trNew);
                        }
                    else
                        {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found !";
                        }
                    }
                strHead1 += "<br/><b>Total Amount Of Successful Transactions :</b>" + totamt.ToString("F");
                }

            LblRptSubHeader.Text = strHead;
            LblRptSubHeader1.Text = strHead1;
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message.ToString());
            }
        finally { context.Dispose(); contextmis.Dispose(); con.Dispose(); com.Dispose(); }
        }

    }