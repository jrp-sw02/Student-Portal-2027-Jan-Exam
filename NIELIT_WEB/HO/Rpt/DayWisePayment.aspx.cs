using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data.SqlClient;
using System.Web;

public partial class HO_Rpt_DayWisePayment : BasePage
{
    Table tbl = new Table();
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
            //if (!IsSessionAlive())
            //{
            //    Response.Redirect("~/index.aspx");
            //}
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            //if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/DayWisePaymentReport.aspx"))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}
            if (!Page.IsPostBack)
            {
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
            tcCol15.Width = Unit.Percentage(5);
            tcCol15.HorizontalAlign = HorizontalAlign.Center;
            tcCol15.Text = "Course";
            th.Cells.Add(tcCol15);

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(10);
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            tcCol.Text = "Payment Date";
            th.Cells.Add(tcCol);

            if (PayModeId == Convert.ToInt32(enmPaymentMode.Online))
            {
                TableHeaderCell tcCol16 = new TableHeaderCell();
                tcCol16.Width = Unit.Percentage(12);
                tcCol16.HorizontalAlign = HorizontalAlign.Center;
                tcCol16.Text = "Settled Date";
                th.Cells.Add(tcCol16);
            }

            if (PayModeId == Convert.ToInt32(enmPaymentMode.NEFTRTGS))
            {
                TableHeaderCell tcCol16 = new TableHeaderCell();
                tcCol16.Width = Unit.Percentage(12);
                tcCol16.HorizontalAlign = HorizontalAlign.Center;
                tcCol16.Text = "Verified Date";
                th.Cells.Add(tcCol16);
            }

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(5);
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            tcCol2.Text = "No of Candidates";
            th.Cells.Add(tcCol2);

            if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                TableHeaderCell tcCol3 = new TableHeaderCell();
                tcCol3.HorizontalAlign = HorizontalAlign.Center;
                tcCol3.Width = Unit.Percentage(5);
                tcCol3.Text = "No of Theory Modules";
                th.Cells.Add(tcCol3);

                TableHeaderCell tcCol4 = new TableHeaderCell();
                tcCol4.HorizontalAlign = HorizontalAlign.Center;
                tcCol4.Width = Unit.Percentage(5);
                tcCol4.Text = "No of Practical Modules";
                th.Cells.Add(tcCol4);

                TableHeaderCell tcCol24 = new TableHeaderCell();
                tcCol24.HorizontalAlign = HorizontalAlign.Center;
                tcCol24.Width = Unit.Percentage(10);
                tcCol24.Text = "Theory Amount (Rs.)";
                th.Cells.Add(tcCol24);

                TableHeaderCell tcCol25 = new TableHeaderCell();
                tcCol25.HorizontalAlign = HorizontalAlign.Center;
                tcCol25.Width = Unit.Percentage(10);
                tcCol25.Text = "Practical Amount (Rs.)";
                th.Cells.Add(tcCol25);

                TableHeaderCell tcCol26 = new TableHeaderCell();
                tcCol26.HorizontalAlign = HorizontalAlign.Center;
                tcCol26.Width = Unit.Percentage(10);
                tcCol26.Text = "Processing Amount (Rs.)";
                th.Cells.Add(tcCol26);

                TableHeaderCell tcCol28 = new TableHeaderCell();
                tcCol28.Width = Unit.Percentage(10);
                tcCol28.HorizontalAlign = HorizontalAlign.Center;
                tcCol28.Text = "Late Fee Amount (Rs.)";
                th.Cells.Add(tcCol28);
            }



            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(10);
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            tcCol5.Text = "Total Fee Amount (Rs.)";
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(6);
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            tcCol9.Text = "Payment Status";
            th.Cells.Add(tcCol9);


            // added by amit start
            TableHeaderCell tcColn1 = new TableHeaderCell();
            tcColn1.Width = Unit.Percentage(6);
            tcColn1.HorizontalAlign = HorizontalAlign.Center;
            tcColn1.Text = "Payment Gateway Used";
            th.Cells.Add(tcColn1);
            // added by amit end



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
            //Added for Audit
            foreach (TableRow row in tbl.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    cell.Text = HttpUtility.HtmlEncode(cell.Text); // Sanitize cell content
                }
            }

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Day_Wise_Payment_Report.xls");
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
            String gateway = Convert.ToString(Request.QueryString["Gateway"]); // added by amit 
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


            if (CourseCatId != 0)
            {
                var category = context.CourseCategories.Find(CourseCatId);
                if (category != null)
                {
                    strHead += "<br/><b>Course Category :</b>" + category.Name;
                }
            }
            else
            {
                strHead += "<br/><b>Course Category :</b> All";
            }


            if (CourseId != 0)
            {
                var courses = context.Courses.Find(CourseId);
                if (courses != null)
                {
                    strHead += "<br/><b>Course Name :</b>" + courses.Name;
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

                /* added by amit start */
                if (gateway == "1")
                    strHead += "<br/><b>Gateway Used : </b>BillDesk ";
                else if (gateway == "2")
                    strHead += "<br/><b>Gateway Used : </b>ICIC ";
                else
                    strHead += "<br/><b>Gateway Used : </b>ALL";
                /* added by amit end */



                mySql.Append(" select  CAST(T.Request_Date as DATE) as TransactionDate," +
                    " COUNT(*) as Count, C.Course_ID as coursecode, CAST( CASE WHEN T.PGCode = 2 THEN T.Request_Date ELSE T.Settled_On END AS DATE ) as SettledDate," +
                    //"  CASE WHEN T.PGCode IS NULL THEN 'EFT' ELSE PG.Description END AS [PaymentGatewayUsed] "); // added by amit start
                    "isnull((select description from paymentgateways pg where id = T.PGCode), 'EFT') as [PaymentGatewayUsed]");

                if (applicationType == enmApplicationType.CourseExamApplication)
                    mySql.Append(",  sum(C.Fee_Amount) as  FeeAmount, sum(C.Number_of_th_Modules) as Theory , sum(C.Number_of_Pr_Modules) as Practical, sum(IsNull(C.Late_Fee_Amount,0)) as late_fee, C.Is_Improvement as Improvement ");
                else if (applicationType == enmApplicationType.CertificateExamApplication)
                    mySql.Append(", sum(C.Total_Fee_Amt) as FeeAmount");
                else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                    mySql.Append(", sum(C.Fee_Amt) as FeeAmount ");
                mySql.Append(" , T.Response_Status_Message as StatusMessage, T.Response_Status_Code as StatusCode  from Online_Transaction T,  Demand_Note d, Course R ");
                if (applicationType == enmApplicationType.CourseExamApplication)
                    mySql.Append(", Course_Exam_Application C ");
                else if (applicationType == enmApplicationType.CertificateExamApplication)
                    mySql.Append(", Certificate_Exam_Application C ");
                else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                    mySql.Append(", Course_Registration_Application C ");

                if (DateType != "0")
                {
                    if (DateType == "P")
                    {
                        mySql.Append(" Where  D.ID = T.Demand_Note_ID and d.Application_Type_ID = " + AppTypeId.ToString() + " and " +
                          "CAST(T.Request_Date as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and CAST(T.Request_Date as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' ");
                    }
                    if (DateType == "SV")
                    {
                        mySql.Append(" WHERE D.ID = T.Demand_Note_ID AND d.Application_Type_ID = " + AppTypeId);
                        if (gateway == "1") // billdesk
                        {
                            mySql.Append(" AND CAST(T.Settled_On AS DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "'" +
                                            "AND CAST(T.Settled_On AS DATE) <='" + PayToDate.ToString("dd-MMM-yyyy") + "'");
                        }
                        else if (gateway == "2") // icici
                        {
                            mySql.Append(" AND CAST(T.Request_Date AS DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy" + "'") +
                                              "AND CAST(T.Request_Date AS DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy" + "'"));
                        }
                        else
                        {
                            mySql.Append(
                                 " AND ( " +
                                 "       ( (T.PGCode = 1 OR T.PGCode IS NULL) " +
                                 "         AND CAST(T.Settled_On AS DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' " +
                                 "         AND CAST(T.Settled_On AS DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' ) " +
                                 "    OR " +
                                 "       ( T.PGCode = 2 " +
                                 "         AND CAST(T.Request_Date AS DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' " +
                                 "         AND CAST(T.Request_Date AS DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' ) " +
                                 "     ) "
                             );
                        }
                    }
                }

                mySql.Append(" and C.Demand_Note_ID = D.ID and C.Course_ID = R.ID ");


                /* added by amit start */

                if (gateway == "2")
                    mySql.Append(" and T.PGCode = 2"); // ICICI
                else if (gateway == "1")
                    mySql.Append(" and (T.PGCode = 1 OR T.PGCode IS NULL)"); // Billdesk 

                /* added by amit end */


                if (TransTypeId != 0)
                    mySql.Append(" and D.Demand_Note_Type_ID = " + TransTypeId.ToString());
                if (CourseId != 0)
                    mySql.Append(" and C.Course_ID = " + CourseId.ToString());
                if (CourseCatId != 0)
                    mySql.Append(" and C.Course_Category_ID = " + CourseCatId.ToString());

                if (PayStatusId != "0")
                {
                    if (PayStatusId == "S")
                        mySql.Append(" and (T.Response_Status_Code = '0300' or T.Response_Status_Code = 'E000') ");  // or T.Response_Status_Code = 'E000' added by amit
                    if (PayStatusId == "F")
                        mySql.Append(" and (T.Response_Status_Code not in ('0300', 'E000') or T.Response_Status_Code is NULL) ");
                }
                if (applicationType == enmApplicationType.CourseExamApplication)
                {
                    mySql.Append(" group by CAST(T.Request_Date as DATE) , t.Response_Status_Message, T.Response_Status_Code, C.Course_ID, CAST( CASE WHEN T.PGCode = 2 THEN T.Request_Date ELSE T.Settled_On END AS DATE ), C.Is_Improvement, T.pgcode ");
                }
                else if (applicationType == enmApplicationType.CertificateExamApplication)
                {
                    mySql.Append(" group by CAST(T.Request_Date as DATE) , t.Response_Status_Message,T.Response_Status_Code, C.Course_ID, CAST( CASE WHEN T.PGCode = 2 THEN T.Request_Date ELSE T.Settled_On END AS DATE ), T.pgcode ");
                }
                else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                {
                    mySql.Append(" group by CAST(T.Request_Date as DATE) , t.Response_Status_Message,T.Response_Status_Code, C.Course_ID, CAST( CASE WHEN T.PGCode = 2 THEN T.Request_Date ELSE T.Settled_On END AS DATE ), T.pgcode ");
                }
                mySql.Append(" order by CAST( CASE WHEN T.PGCode = 2 THEN T.Request_Date ELSE T.Settled_On END AS DATE ) ");
                dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false);

                if (dt.Rows.Count > 0)
                {
                    ShowTableHeader();
                    foreach (DataRow dtRow in dt.Rows)
                    {
                        TableRow tr = new TableRow();
                        if (i % 2 == 0)
                            tr.CssClass = "gdalternate1";
                        else
                            tr.CssClass = "gdrow1";
                        i++;
                        TableHeaderCell tcCol1 = new TableHeaderCell();
                        tcCol1.Width = Unit.Percentage(2);
                        tcCol1.HorizontalAlign = HorizontalAlign.Left;
                        tcCol1.Text = i.ToString();
                        tr.Cells.Add(tcCol1);

                        TableCell tcCol25 = new TableCell();
                        tcCol25.Width = Unit.Percentage(5);
                        tcCol25.HorizontalAlign = HorizontalAlign.Center;
                        //tcCol25.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller("select code from course where id=" + dtRow["coursecode"].ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false).ToString();
                        //December_2024
                        SqlParameter[] param1 = { new SqlParameter("@courseCode", dtRow["coursecode"].ToString()) };
                        tcCol25.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller("select code from course where id=@courseCode ", new EConnect.Connections.SqlCon(), param1, CommandType.Text, false).ToString();
                        tr.Cells.Add(tcCol25);


                        TableCell tcCol = new TableCell();
                        tcCol.Width = Unit.Percentage(8);
                        tcCol.HorizontalAlign = HorizontalAlign.Center;
                        tcCol.Text = Convert.ToDateTime(dtRow["TransactionDate"]).ToString("dd-MMM-yyyy");
                        tr.Cells.Add(tcCol);

                        TableCell tcCol16 = new TableCell();
                        tcCol16.Width = Unit.Percentage(8);
                        tcCol16.HorizontalAlign = HorizontalAlign.Center;
                        tcCol16.Text = String.IsNullOrEmpty(dtRow["SettledDate"].ToString()) ? "NA" : Convert.ToDateTime(dtRow["SettledDate"]).ToString("dd-MMM-yyyy");
                        tr.Cells.Add(tcCol16);

                        TableCell tcCol2 = new TableCell();
                        tcCol2.Width = Unit.Percentage(8);
                        tcCol2.HorizontalAlign = HorizontalAlign.Right;
                        tcCol2.Text = dtRow["Count"].ToString();
                        noofCandidates += Convert.ToInt64(dtRow["Count"]);
                        tr.Cells.Add(tcCol2);

                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {
                            Int32 theoryfee = 0;
                            Int32 practicalfee = 0;
                            Boolean isImprovement = Convert.ToBoolean(dtRow["Improvement"]);
                            if (isImprovement == true)
                            {
                                //    theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount  from Fee_Detail where Course_ID =" + dtRow["coursecode"].ToString() +
                                //        "and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" +
                                //    "and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

                                //    //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() + "and Effective_To_Date is null  and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() + 
                                //    "and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" + 
                                //    "and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //December_2024
                                SqlParameter[] param2 = { new SqlParameter("@courseCode", dtRow["coursecode"].ToString()),
                                                            new SqlParameter("@TransactionDate", dtRow["TransactionDate"].ToString()),
                                                                new SqlParameter("@improvementFeeType", improvementfeetype)
                                                                };
                                theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller(" select Fee_Amount  from Fee_Detail where Course_ID =@courseCode " +
                                   "and" + " (( @TransactionDate >= [Effective_From_Date] and @TransactionDate <= Effective_To_Date) " + " OR (Effective_To_Date IS NULL AND @TransactionDate>= [Effective_From_Date]))" +
                               "and Fee_Type_ID =@improvementFeeType ", new EConnect.Connections.SqlCon(), param2, CommandType.Text, false));

                                //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() + "and Effective_To_Date is null  and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() + 
                                //"and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" + 
                                //"and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

                                //December_2024
                                SqlParameter[] param3 = { new SqlParameter("@courseCode",  dtRow["coursecode"].ToString()),
                                                            new SqlParameter("@TransactionDate", dtRow["TransactionDate"].ToString()),
                                                                new SqlParameter("@improvementFeeType", improvementfeetype)
                                                                };
                                practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller(" select Fee_Amount from Fee_Detail where Course_ID = @courseCode " +
                                "and " + " (( @TransactionDate >= [Effective_From_Date] and @TransactionDate <= Effective_To_Date) " + " OR (Effective_To_Date IS NULL AND @TransactionDate >= [Effective_From_Date]))" +
                                "and Fee_Type_ID = @improvementFeeType ", new EConnect.Connections.SqlCon(), param3, CommandType.Text, false));




                            }
                            else
                            {
                                //Shivesh
                                //theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount  from Fee_Detail where Course_ID =" + dtRow["coursecode"].ToString() + " and Effective_To_Date is null  and Fee_Type_ID =" + theoryfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID =" + dtRow["coursecode"].ToString() +
                                //    "and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" +
                                //" and Fee_Type_ID =" + theoryfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

                                //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() + " and Effective_To_Date is null and Fee_Type_ID =" + practicalfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() +
                                //    "and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" +
                                //    "and Fee_Type_ID =" + practicalfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //December_2024
                                SqlParameter[] param4 = { new SqlParameter("@courseCode",  dtRow["coursecode"].ToString()),
                                                            new SqlParameter("@TransactionDate", dtRow["TransactionDate"].ToString()),
                                                                new SqlParameter("@theoryFeeType", theoryfeetype)
                                                                };
                                theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = @courseCode " +
                                    "and" + "(( @TransactionDate >= [Effective_From_Date] and @TransactionDate <= Effective_To_Date) " + " OR (Effective_To_Date IS NULL AND @TransactionDate >= [Effective_From_Date])) " +
                                " and Fee_Type_ID = @theoryFeeType ", new EConnect.Connections.SqlCon(), param4, CommandType.Text, false));
                                //December_2024
                                SqlParameter[] param5 = { new SqlParameter("@courseCode",  dtRow["coursecode"].ToString()),
                                                            new SqlParameter("@TransactionDate", dtRow["TransactionDate"].ToString()),
                                                                new SqlParameter("@practicalFeeType", practicalfeetype)
                                                                };
                                practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = @courseCode " +
                                "and " + " (( @TransactionDate >= [Effective_From_Date] and @TransactionDate <= Effective_To_Date) " + " OR (Effective_To_Date IS NULL AND @TransactionDate >= [Effective_From_Date])) " +
                                "and Fee_Type_ID =@practicalFeeType ", new EConnect.Connections.SqlCon(), param5, CommandType.Text, false));

                            }

                            //  Int32 processingfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() + " and Effective_To_Date is null and Fee_Type_ID =" + processingfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                            //December_2024
                            SqlParameter[] param6 = {
                                                        new SqlParameter("@courseCode", dtRow["coursecode"].ToString()),
                                                            new SqlParameter("@practicalFeeType", processingfeetype)
                                                                };
                            Int32 processingfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = @courseCode and Effective_To_Date is null and Fee_Type_ID = @practicalFeeType ", new EConnect.Connections.SqlCon(), param6, CommandType.Text, false));

                            TableCell tcCol3 = new TableCell();
                            tcCol3.HorizontalAlign = HorizontalAlign.Right;
                            tcCol3.Width = Unit.Percentage(6);
                            tcCol3.Text = dtRow["Theory"].ToString();
                            noofTheoryModules += Convert.ToInt64(dtRow["Theory"]);
                            tr.Cells.Add(tcCol3);

                            TableCell tcCol4 = new TableCell();
                            tcCol4.HorizontalAlign = HorizontalAlign.Right;
                            tcCol4.Width = Unit.Percentage(6);
                            tcCol4.Text = dtRow["Practical"].ToString();
                            noofPracticalModules += Convert.ToInt64(dtRow["Practical"]);
                            tr.Cells.Add(tcCol4);

                            TableCell tcCol34 = new TableCell();
                            tcCol34.HorizontalAlign = HorizontalAlign.Right;
                            tcCol34.Width = Unit.Percentage(6);
                            tcCol34.Text = (Convert.ToInt32(dtRow["Theory"]) * theoryfee).ToString("F");
                            TotaltheoryFee += Convert.ToDecimal(tcCol34.Text);
                            tr.Cells.Add(tcCol34);

                            TableCell tcCol35 = new TableCell();
                            tcCol35.HorizontalAlign = HorizontalAlign.Right;
                            tcCol35.Width = Unit.Percentage(6);
                            tcCol35.Text = (Convert.ToInt32(dtRow["Practical"]) * practicalfee).ToString("F");
                            TotalPracticalFee += Convert.ToDecimal(tcCol35.Text);
                            tr.Cells.Add(tcCol35);

                            TableCell tcCol36 = new TableCell();
                            tcCol36.HorizontalAlign = HorizontalAlign.Right;
                            tcCol36.Width = Unit.Percentage(6);
                            tcCol36.Text = (Convert.ToInt32(dtRow["Count"]) * processingfee).ToString("F");
                            TotalProcessingFee += Convert.ToDecimal(tcCol36.Text);
                            tr.Cells.Add(tcCol36);

                            TableCell tcCol37 = new TableCell();
                            tcCol37.HorizontalAlign = HorizontalAlign.Right;
                            tcCol37.Width = Unit.Percentage(6);
                            tcCol37.Text = Convert.ToInt32(dtRow["late_fee"]).ToString("F");
                            TotalLateFee += Convert.ToDecimal(tcCol37.Text);
                            tr.Cells.Add(tcCol37);
                        }


                        TableCell tcCol8 = new TableCell();
                        tcCol8.Width = Unit.Percentage(8);
                        tcCol8.HorizontalAlign = HorizontalAlign.Right;
                        tcCol8.Text = Convert.ToInt64(dtRow["FeeAmount"]).ToString("F");
                        TotalAmount += Convert.ToDecimal(tcCol8.Text);
                        tr.Cells.Add(tcCol8);


                        TableCell tcCol9 = new TableCell();
                        tcCol9.Width = Unit.Percentage(20);
                        tcCol9.HorizontalAlign = HorizontalAlign.Left;
                        tcCol9.Text = (String.IsNullOrEmpty(dtRow["StatusMessage"].ToString()) ? "Failed" : dtRow["StatusMessage"].ToString());
                        tr.Cells.Add(tcCol9);

                        TableCell tdRow30 = new TableCell();
                        tdRow30.Width = Unit.Percentage(5);
                        if (!String.IsNullOrEmpty(dtRow["PaymentGatewayUsed"].ToString()))
                            tdRow30.Text = dtRow["PaymentGatewayUsed"].ToString();
                        else
                            tdRow30.Text = "NA";

                        tr.Cells.Add(tdRow30);
                        tbl.Rows.Add(tr);
                        
                        if (dtRow["StatusMessage"].ToString().ToUpper().Trim() == "Success".ToUpper().Trim() || dtRow["StatusCode"].ToString() == "E000")
                        {
                            totamt += Convert.ToDecimal(dtRow["FeeAmount"]);
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

                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    {
                        TableCell tdNewRow6 = new TableCell();
                        tdNewRow6.Width = Unit.Percentage(1);
                        tdNewRow6.Text = "<b>" + noofTheoryModules.ToString() + "</b>";
                        tdNewRow6.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow6);

                        TableCell tdNewRow7 = new TableCell();
                        tdNewRow7.Width = Unit.Percentage(1);
                        tdNewRow7.Text = "<b>" + noofPracticalModules.ToString() + "</b>";
                        tdNewRow7.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow7);

                        TableCell tdNewRow8 = new TableCell();
                        tdNewRow8.Width = Unit.Percentage(1);
                        tdNewRow8.Text = "<b>" + TotaltheoryFee.ToString() + "</b>";
                        tdNewRow8.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow8);

                        TableCell tdNewRow9 = new TableCell();
                        tdNewRow9.Width = Unit.Percentage(1);
                        tdNewRow9.Text = "<b>" + TotalPracticalFee.ToString() + "</b>";
                        tdNewRow9.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow9);

                        TableCell tdNewRow10 = new TableCell();
                        tdNewRow10.Width = Unit.Percentage(1);
                        tdNewRow10.Text = "<b>" + TotalProcessingFee.ToString() + "</b>";
                        tdNewRow10.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow10);

                        TableCell tdNewRow11 = new TableCell();
                        tdNewRow11.Width = Unit.Percentage(1);
                        tdNewRow11.Text = "<b>" + TotalLateFee.ToString() + "</b>";
                        tdNewRow11.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow11);
                    }

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

                    //added by amit start
                    TableCell tdNewRown1 = new TableCell();
                    tdNewRown1.Width = Unit.Percentage(1);
                    tdNewRown1.Text = "";
                    tdNewRown1.HorizontalAlign = HorizontalAlign.Right;
                    trNew.Cells.Add(tdNewRown1);
                    //added by amit end


                    tbl.Rows.Add(trNew);
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "No Record Found !";
                }
                strHead1 += "<br/><b>Total Amount Of Successful Transactions :</b>" + totamt.ToString("F");
            }
            else if (paymentMode == enmPaymentMode.CSCSPV)
            {
                strHead += "<br/><b> Date Type :</b> Payment Date";
                mySql.Append(" select CAST(T.Date as DATE) as TransactionDate, COUNT(*) as Count,C.Course_ID as coursecode");
                if (applicationType == enmApplicationType.CourseExamApplication)
                    mySql.Append(",  sum(C.Fee_Amount) as  FeeAmount, sum(C.Number_of_th_Modules) as Theory , sum(C.Number_of_Pr_Modules) as Practical, sum(IsNull(C.Late_Fee_Amount,0)) as late_fee , C.Is_Improvement as Improvement ");
                else if (applicationType == enmApplicationType.CertificateExamApplication)
                    mySql.Append(", sum(C.Total_Fee_Amt) as FeeAmount");
                else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                    mySql.Append(", sum(C.Fee_Amt) as FeeAmount ");
                mySql.Append(" , T.Response_Message as StatusMessage  from CSC_Transaction T,  Demand_Note d, Course R ");
                if (applicationType == enmApplicationType.CourseExamApplication)
                    mySql.Append(", Course_Exam_Application C ");
                else if (applicationType == enmApplicationType.CertificateExamApplication)
                    mySql.Append(", Certificate_Exam_Application C ");
                else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                    mySql.Append(", Course_Registration_Application C ");

                mySql.Append(" Where  D.ID = T.Demand_Note_ID and d.Application_Type_ID = " + AppTypeId.ToString() + " and " +
                            " CAST(t.Date as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and CAST(T.Date as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' ");
                mySql.Append(" and C.Demand_Note_ID = D.ID and C.Course_ID = R.ID");

                if (TransTypeId != 0)
                    mySql.Append(" and D.Demand_Note_Type_ID = " + TransTypeId.ToString());
                if (CourseId != 0)
                    mySql.Append(" and C.Course_ID = " + CourseId.ToString());
                if (CourseCatId != 0)
                    mySql.Append(" and C.Course_Category_ID = " + CourseCatId.ToString());

                if (PayStatusId != "0")
                {
                    if (PayStatusId == "S")
                        mySql.Append(" and (T.Response_Status = '0' or T.Response_Status = '100')");
                    if (PayStatusId == "F")
                        mySql.Append(" and (T.Response_Status = '1' or T.Response_Status is NULL)");
                }
                if (applicationType == enmApplicationType.CourseExamApplication)
                {
                    mySql.Append(" group by CAST(T.Date as DATE) , T.Response_Message,C.Course_ID,  C.Is_Improvement ");
                }
                else if (applicationType == enmApplicationType.CertificateExamApplication)
                {
                    mySql.Append(" group by CAST(T.Date as DATE) , T.Response_Message,C.Course_ID ");
                }
                else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                {
                    mySql.Append(" group by CAST(T.Date as DATE) , T.Response_Message,C.Course_ID ");
                }
                mySql.Append(" order by  CAST(T.Date as DATE) ");
                dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
                if (dt.Rows.Count > 0)
                {
                    ShowTableHeader();
                    foreach (DataRow dtRow in dt.Rows)
                    {
                        TableRow tr = new TableRow();
                        if (i % 2 == 0)
                            tr.CssClass = "gdalternate1";
                        else
                            tr.CssClass = "gdrow1";
                        i++;
                        TableHeaderCell tcCol1 = new TableHeaderCell();
                        tcCol1.Width = Unit.Percentage(2);
                        tcCol1.HorizontalAlign = HorizontalAlign.Left;
                        tcCol1.Text = i.ToString();
                        tr.Cells.Add(tcCol1);

                        TableCell tcCol25 = new TableCell();
                        tcCol25.Width = Unit.Percentage(5);
                        tcCol25.HorizontalAlign = HorizontalAlign.Center;
                        //  tcCol25.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller("select code from course where id=" + dtRow["coursecode"].ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false).ToString();
                        //December_2024
                        SqlParameter[] param7 = { new SqlParameter("@courseCode", dtRow["coursecode"].ToString()) };
                        tcCol25.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller("select code from course where id=@courseCode ", new EConnect.Connections.SqlCon(), param7, CommandType.Text, false).ToString();
                        tr.Cells.Add(tcCol25);

                        TableCell tcCol = new TableCell();
                        tcCol.Width = Unit.Percentage(5);
                        tcCol.HorizontalAlign = HorizontalAlign.Center;
                        tcCol.Text = Convert.ToDateTime(dtRow["TransactionDate"]).ToString("dd-MMM-yyyy");
                        tr.Cells.Add(tcCol);

                        TableCell tcCol2 = new TableCell();
                        tcCol2.Width = Unit.Percentage(8);
                        tcCol2.HorizontalAlign = HorizontalAlign.Right;
                        tcCol2.Text = dtRow["Count"].ToString();
                        noofCandidates += Convert.ToInt64(dtRow["Count"]);
                        tr.Cells.Add(tcCol2);


                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {
                            Int32 theoryfee = 0;
                            Int32 practicalfee = 0;
                            Boolean isImprovement = Convert.ToBoolean(dtRow["Improvement"]);
                            if (isImprovement == true)
                            {
                                //theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID =" + dtRow["coursecode"].ToString() + " and Effective_To_Date is null  and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID =" + dtRow["coursecode"].ToString() +

                                //"and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" +

                                //" and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //December_2024
                                SqlParameter[] param8 = { new SqlParameter("@courseCode", dtRow["coursecode"].ToString()),
                                                            new SqlParameter("@TransactionDate",dtRow["TransactionDate"].ToString()),
                                                                new SqlParameter("@improvementFeeType", improvementfeetype)
                                                            };
                                theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = @courseCode " +

                                " and " + " (( @TransactionDate >= [Effective_From_Date] and @TransactionDate <= Effective_To_Date) " + " OR (Effective_To_Date IS NULL AND @TransactionDate>= [Effective_From_Date])) " +

                                " and Fee_Type_ID = @improvementFeeType ", new EConnect.Connections.SqlCon(), param8, CommandType.Text, false));


                                //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() + "and Effective_To_Date is null  and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() +
                                //    "and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" +
                                //    "and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //December_2024
                                SqlParameter[] param9 = { new SqlParameter("@courseCode",  dtRow["coursecode"].ToString()),
                                                            new SqlParameter("@TransactionDate", dtRow["TransactionDate"].ToString()),
                                                                    new SqlParameter("@improvementFeeType", improvementfeetype)
                                                                };
                                practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = @courseCode " +
                                "and " + " (( @TransactionDate >= [Effective_From_Date] and @TransactionDate <= Effective_To_Date) " + " OR (Effective_To_Date IS NULL AND @TransactionDate >= [Effective_From_Date])) " +
                                " and Fee_Type_ID =@improvementFeeType ", new EConnect.Connections.SqlCon(), param9, CommandType.Text, false));

                            }
                            else
                            {
                                //theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount  from Fee_Detail where Course_ID =" + dtRow["coursecode"].ToString() + " and Effective_To_Date is null  and Fee_Type_ID =" + theoryfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount  from Fee_Detail where Course_ID =" + dtRow["coursecode"].ToString() +
                                //    "and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" +

                                //"and Fee_Type_ID =" + theoryfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //December_2024
                                SqlParameter[] param10 = { new SqlParameter("@courseCode",  dtRow["coursecode"].ToString()),
                                                            new SqlParameter("@TransactionDate", dtRow["TransactionDate"].ToString()),
                                                                new SqlParameter("@theoryFeeType", theoryfeetype)
                                                                    };
                                theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount  from Fee_Detail where Course_ID =@courseCode " +
                                    "and " + "(( @TransactionDate >= [Effective_From_Date] and @TransactionDate <= Effective_To_Date) " + " OR (Effective_To_Date IS NULL AND @TransactionDate >= [Effective_From_Date])) " +

                                " and Fee_Type_ID = @theoryFeeType ", new EConnect.Connections.SqlCon(), param10, CommandType.Text, false));


                                //     practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() + " and Effective_To_Date is null  and Fee_Type_ID =" + practicalfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() +
                                //    "and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" +  
                                //     "and Fee_Type_ID =" + practicalfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //December_2024
                                SqlParameter[] param11 = { new SqlParameter("@courseCode",  dtRow["coursecode"].ToString()),
                                                            new SqlParameter("@TransactionDate", dtRow["TransactionDate"].ToString()),
                                                                new SqlParameter("@practicalFeeType", practicalfeetype)
                                                                    };
                                practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = @courseCode " +
                                            "and " + " (( @TransactionDate >= [Effective_From_Date] and @TransactionDate <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND @TransactionDate >= [Effective_From_Date])) " +
                                             "and Fee_Type_ID =@practicalFeeType ", new EConnect.Connections.SqlCon(), param11, CommandType.Text, false));


                            }

                            // Int32 processingfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() + " and Effective_To_Date is null  and Fee_Type_ID =" + processingfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                            //December_2024
                            SqlParameter[] param12 = {
                             new SqlParameter("@courseCode", dtRow["coursecode"].ToString()),
                                 new SqlParameter("@processingFeeType", processingfeetype)
                                     };
                            Int32 processingfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = @courseCode and Effective_To_Date is null  and Fee_Type_ID = @processingFeeType ", new EConnect.Connections.SqlCon(), param12, CommandType.Text, false));


                            TableCell tcCol3 = new TableCell();
                            tcCol3.HorizontalAlign = HorizontalAlign.Right;
                            tcCol3.Width = Unit.Percentage(6);
                            tcCol3.Text = dtRow["Theory"].ToString();
                            noofTheoryModules += Convert.ToInt64(dtRow["Theory"]);
                            tr.Cells.Add(tcCol3);

                            TableCell tcCol4 = new TableCell();
                            tcCol4.HorizontalAlign = HorizontalAlign.Right;
                            tcCol4.Width = Unit.Percentage(6);
                            tcCol4.Text = dtRow["Practical"].ToString();
                            noofPracticalModules += Convert.ToInt64(dtRow["Practical"]);
                            tr.Cells.Add(tcCol4);

                            TableCell tcCol34 = new TableCell();
                            tcCol34.HorizontalAlign = HorizontalAlign.Right;
                            tcCol34.Width = Unit.Percentage(6);
                            tcCol34.Text = (Convert.ToInt32(dtRow["Theory"]) * theoryfee).ToString("F");
                            TotaltheoryFee += Convert.ToDecimal(tcCol34.Text);
                            tr.Cells.Add(tcCol34);

                            TableCell tcCol35 = new TableCell();
                            tcCol35.HorizontalAlign = HorizontalAlign.Right;
                            tcCol35.Width = Unit.Percentage(6);
                            tcCol35.Text = (Convert.ToInt32(dtRow["Practical"]) * practicalfee).ToString("F");
                            TotalPracticalFee += Convert.ToDecimal(tcCol35.Text);
                            tr.Cells.Add(tcCol35);

                            TableCell tcCol36 = new TableCell();
                            tcCol36.HorizontalAlign = HorizontalAlign.Right;
                            tcCol36.Width = Unit.Percentage(6);
                            tcCol36.Text = (Convert.ToInt32(dtRow["Count"]) * processingfee).ToString("F");
                            TotalProcessingFee += Convert.ToDecimal(tcCol36.Text);
                            tr.Cells.Add(tcCol36);

                            TableCell tcCol37 = new TableCell();
                            tcCol37.HorizontalAlign = HorizontalAlign.Right;
                            tcCol37.Width = Unit.Percentage(6);
                            tcCol37.Text = Convert.ToInt32(dtRow["late_fee"]).ToString("F");
                            TotalLateFee += Convert.ToDecimal(tcCol37.Text);
                            tr.Cells.Add(tcCol37);
                        }


                        TableCell tcCol8 = new TableCell();
                        tcCol8.Width = Unit.Percentage(8);
                        tcCol8.HorizontalAlign = HorizontalAlign.Right;
                        tcCol8.Text = Convert.ToInt64(dtRow["FeeAmount"]).ToString("F");
                        TotalAmount += Convert.ToDecimal(tcCol8.Text);
                        tr.Cells.Add(tcCol8);

                        TableCell tcCol9 = new TableCell();
                        tcCol9.Width = Unit.Percentage(20);
                        tcCol9.HorizontalAlign = HorizontalAlign.Left;
                        tcCol9.Text = (String.IsNullOrEmpty(dtRow["StatusMessage"].ToString()) ? "Failed" : dtRow["StatusMessage"].ToString());
                        tr.Cells.Add(tcCol9);


                        tbl.Rows.Add(tr);
                        if (dtRow["StatusMessage"].ToString().ToUpper().Trim() == "Success".ToUpper().Trim())
                        {
                            totamt += Convert.ToDecimal(dtRow["FeeAmount"]);
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
                    tdNewRow1.ColumnSpan = 3;
                    tdNewRow1.HorizontalAlign = HorizontalAlign.Center;
                    trNew.Cells.Add(tdNewRow1);

                    TableCell tdNewRow5 = new TableCell();
                    tdNewRow5.Width = Unit.Percentage(1);
                    tdNewRow5.Text = "<b>" + noofCandidates.ToString() + "</b>";
                    tdNewRow5.HorizontalAlign = HorizontalAlign.Right;
                    trNew.Cells.Add(tdNewRow5);

                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    {
                        TableCell tdNewRow6 = new TableCell();
                        tdNewRow6.Width = Unit.Percentage(1);
                        tdNewRow6.Text = "<b>" + noofTheoryModules.ToString() + "</b>";
                        tdNewRow6.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow6);

                        TableCell tdNewRow7 = new TableCell();
                        tdNewRow7.Width = Unit.Percentage(1);
                        tdNewRow7.Text = "<b>" + noofPracticalModules.ToString() + "</b>";
                        tdNewRow7.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow7);

                        TableCell tdNewRow8 = new TableCell();
                        tdNewRow8.Width = Unit.Percentage(1);
                        tdNewRow8.Text = "<b>" + TotaltheoryFee.ToString() + "</b>";
                        tdNewRow8.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow8);

                        TableCell tdNewRow9 = new TableCell();
                        tdNewRow9.Width = Unit.Percentage(1);
                        tdNewRow9.Text = "<b>" + TotalPracticalFee.ToString() + "</b>";
                        tdNewRow9.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow9);

                        TableCell tdNewRow10 = new TableCell();
                        tdNewRow10.Width = Unit.Percentage(1);
                        tdNewRow10.Text = "<b>" + TotalProcessingFee.ToString() + "</b>";
                        tdNewRow10.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow10);

                        TableCell tdNewRow11 = new TableCell();
                        tdNewRow11.Width = Unit.Percentage(1);
                        tdNewRow11.Text = "<b>" + TotalLateFee.ToString() + "</b>";
                        tdNewRow11.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow11);
                    }

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
                strHead1 += "<br/><b>Total Amount Of Successful Transactions :</b>" + totamt.ToString("F");
            }
            else if (paymentMode == enmPaymentMode.DemandDraft)
            {

                strHead += "<br/><b> Date Type :</b> Payment Date";
                mySql.Append(" select CAST(T.Date as DATE) as TransactionDate, COUNT(*) as Count,C.Course_ID as coursecode");
                if (applicationType == enmApplicationType.CourseExamApplication)
                    mySql.Append(",  sum(C.Fee_Amount) as  FeeAmount, sum(C.Number_of_th_Modules) as Theory , sum(C.Number_of_Pr_Modules) as Practical , sum(IsNull(C.Late_Fee_Amount,0)) as late_fee , C.Is_Improvement as Improvement ");
                else if (applicationType == enmApplicationType.CertificateExamApplication)
                    mySql.Append(", sum(C.Total_Fee_Amt) as FeeAmount");
                else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                    mySql.Append(", sum(C.Fee_Amt) as FeeAmount ");
                mySql.Append(" , D.Status_ID as StatusMessage from DemandDraftTransaction T,  Demand_Note d, Course R ");
                if (applicationType == enmApplicationType.CourseExamApplication)
                    mySql.Append(", Course_Exam_Application C ");
                else if (applicationType == enmApplicationType.CertificateExamApplication)
                    mySql.Append(", Certificate_Exam_Application C ");
                else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                    mySql.Append(", Course_Registration_Application C ");

                mySql.Append(" Where  D.ID = T.Demand_Note_ID and d.Application_Type_ID = " + AppTypeId.ToString() + " and " +
                            " CAST(t.Date as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and CAST(T.Date as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' ");
                mySql.Append(" and C.Demand_Note_ID = D.ID and C.Course_ID = R.ID");

                if (TransTypeId != 0)
                    mySql.Append(" and D.Demand_Note_Type_ID = " + TransTypeId.ToString());
                if (CourseId != 0)
                    mySql.Append(" and C.Course_ID = " + CourseId.ToString());
                if (CourseCatId != 0)
                    mySql.Append(" and C.Course_Category_ID = " + CourseCatId.ToString());

                if (PayStatusId != "0")
                {
                    if (PayStatusId == "S")
                        mySql.Append(" and D.Status_ID =" + Convert.ToInt32(enmPaymentStatus.Paid));
                    if (PayStatusId == "F")
                        mySql.Append(" and (D.Status_ID =" + Convert.ToInt32(enmPaymentStatus.Failed) + ")");
                }
                if (applicationType == enmApplicationType.CourseExamApplication)
                {
                    mySql.Append(" group by CAST(t.Date as DATE) , D.Status_ID,C.Course_ID , C.Is_Improvement ");
                }
                else if (applicationType == enmApplicationType.CertificateExamApplication)
                {
                    mySql.Append(" group by CAST(t.Date as DATE) , D.Status_ID,C.Course_ID ");
                }
                else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                {
                    mySql.Append(" group by CAST(t.Date as DATE) , D.Status_ID,C.Course_ID ");
                }
                mySql.Append(" order by  CAST(t.Date as DATE) ");
                dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
                if (dt.Rows.Count > 0)
                {
                    ShowTableHeader();
                    foreach (DataRow dtRow in dt.Rows)
                    {
                        TableRow tr = new TableRow();
                        if (i % 2 == 0)
                            tr.CssClass = "gdalternate1";
                        else
                            tr.CssClass = "gdrow1";
                        i++;
                        TableHeaderCell tcCol1 = new TableHeaderCell();
                        tcCol1.Width = Unit.Percentage(2);
                        tcCol1.HorizontalAlign = HorizontalAlign.Left;
                        tcCol1.Text = i.ToString();
                        tr.Cells.Add(tcCol1);

                        TableCell tcCol25 = new TableCell();
                        tcCol25.Width = Unit.Percentage(5);
                        tcCol25.HorizontalAlign = HorizontalAlign.Center;
                        // tcCol25.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller("select code from course where id=" + dtRow["coursecode"].ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false).ToString();
                        //December_2024
                        SqlParameter[] param13 = { new SqlParameter("@courseCode", dtRow["coursecode"].ToString()) };
                        tcCol25.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller("select code from course where id= @courseCode ", new EConnect.Connections.SqlCon(), param13, CommandType.Text, false).ToString();
                        tr.Cells.Add(tcCol25);

                        TableCell tcCol = new TableCell();
                        tcCol.Width = Unit.Percentage(5);
                        tcCol.HorizontalAlign = HorizontalAlign.Center;
                        tcCol.Text = Convert.ToDateTime(dtRow["TransactionDate"]).ToString("dd-MMM-yyyy");
                        tr.Cells.Add(tcCol);

                        TableCell tcCol2 = new TableCell();
                        tcCol2.Width = Unit.Percentage(8);
                        tcCol2.HorizontalAlign = HorizontalAlign.Right;
                        tcCol2.Text = dtRow["Count"].ToString();
                        noofCandidates += Convert.ToInt64(dtRow["Count"]);
                        tr.Cells.Add(tcCol2);

                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {
                            Int32 theoryfee = 0;
                            Int32 practicalfee = 0;
                            Boolean isImprovement = Convert.ToBoolean(dtRow["Improvement"]);
                            if (isImprovement == true)
                            {
                                //theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount  from Fee_Detail where Course_ID =" + dtRow["coursecode"].ToString() + " and Effective_To_Date is null  and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount  from Fee_Detail where Course_ID =" + dtRow["coursecode"].ToString() +

                                //    "and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" +

                                //"and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //December_2024
                                SqlParameter[] param14 = { new SqlParameter("@courseCode", dtRow["coursecode"].ToString()),
                                                            new SqlParameter("@TransactionDate", dtRow["TransactionDate"].ToString()),
                                                                new SqlParameter("@improvementFeeType", improvementfeetype)
                                                                    };
                                theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller(" select Fee_Amount  from Fee_Detail where Course_ID = @courseCode " +

                                    "and" + " (( @TransactionDate >= [Effective_From_Date] and @TransactionDate <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND @TransactionDate >= [Effective_From_Date])) " +

                                    "and Fee_Type_ID =@improvementFeeType ", new EConnect.Connections.SqlCon(), param14, CommandType.Text, false));


                                //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() + " and Effective_To_Date is null  and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() +
                                //    "and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" +
                                //    "and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //December_2024
                                SqlParameter[] param15 = { new SqlParameter("@courseCode",  dtRow["coursecode"].ToString()),
                                                             new SqlParameter("@TransactionDate", dtRow["TransactionDate"].ToString()),
                                                                new SqlParameter("@improvementFeeType", improvementfeetype)
                                                                    };
                                practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller(" select Fee_Amount from Fee_Detail where Course_ID = @courseCode " +
                               "and " + " (( @TransactionDate >= [Effective_From_Date] and @TransactionDate <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND @TransactionDate >= [Effective_From_Date]))" +
                               "and Fee_Type_ID = @improvementFeeType ", new EConnect.Connections.SqlCon(), param15, CommandType.Text, false));



                            }
                            else
                            {
                                //theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount  from Fee_Detail where Course_ID =" + dtRow["coursecode"].ToString() + " and Effective_To_Date is null  and Fee_Type_ID =" + theoryfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount  from Fee_Detail where Course_ID =" + dtRow["coursecode"].ToString() +
                                //   "and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" +
                                //"and Fee_Type_ID =" + theoryfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //December_2024
                                SqlParameter[] param16 = { new SqlParameter("@courseCode",  dtRow["coursecode"].ToString()),
                                                                new SqlParameter("@TransactionDate", dtRow["TransactionDate"].ToString()),
                                                                    new SqlParameter("@theoryFeeType", theoryfeetype)
                                                                        };
                                theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount  from Fee_Detail where Course_ID =@courseCode " +
                                   "and" + " (( @TransactionDate >= [Effective_From_Date] and @TransactionDate <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND @TransactionDate >= [Effective_From_Date])) " +
                                "and Fee_Type_ID =@theoryFeeType ", new EConnect.Connections.SqlCon(), param16, CommandType.Text, false));


                                //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() + " and Effective_To_Date is null  and Fee_Type_ID =" + practicalfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() +
                                //    "and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" +
                                //    "and Fee_Type_ID =" + practicalfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //December_2024
                                SqlParameter[] param17 = { new SqlParameter("@courseCode",  dtRow["coursecode"].ToString()),
                                                                new SqlParameter("@TransactionDate", dtRow["TransactionDate"].ToString()),
                                                                        new SqlParameter("@practicalFeeType", practicalfeetype)
                                                                            };
                                practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = @courseCode " +
                                "and" + "(( @TransactionDate >= [Effective_From_Date] and @TransactionDate <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND @TransactionDate>= [Effective_From_Date]))" +
                                "and Fee_Type_ID = @practicalFeeType ", new EConnect.Connections.SqlCon(), param17, CommandType.Text, false));


                            }

                            // Int32 processingfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() + " and Effective_To_Date is null  and Fee_Type_ID =" + processingfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                            //December_2024
                            SqlParameter[] param18 = {
                                                        new SqlParameter("@courseCode", dtRow["coursecode"].ToString()),
                                                            new SqlParameter("@practicalFeeType", processingfeetype)
                                                                };
                            Int32 processingfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = @courseCode and Effective_To_Date is null  and Fee_Type_ID = @practicalFeeType ", new EConnect.Connections.SqlCon(), param18, CommandType.Text, false));


                            TableCell tcCol3 = new TableCell();
                            tcCol3.HorizontalAlign = HorizontalAlign.Right;
                            tcCol3.Width = Unit.Percentage(6);
                            tcCol3.Text = dtRow["Theory"].ToString();
                            noofTheoryModules += Convert.ToInt64(dtRow["Theory"]);
                            tr.Cells.Add(tcCol3);

                            TableCell tcCol4 = new TableCell();
                            tcCol4.HorizontalAlign = HorizontalAlign.Right;
                            tcCol4.Width = Unit.Percentage(6);
                            tcCol4.Text = dtRow["Practical"].ToString();
                            noofPracticalModules += Convert.ToInt64(dtRow["Practical"]);
                            tr.Cells.Add(tcCol4);

                            TableCell tcCol34 = new TableCell();
                            tcCol34.HorizontalAlign = HorizontalAlign.Right;
                            tcCol34.Width = Unit.Percentage(6);
                            tcCol34.Text = (Convert.ToInt32(dtRow["Theory"]) * theoryfee).ToString("F");
                            TotaltheoryFee += Convert.ToDecimal(tcCol34.Text);
                            tr.Cells.Add(tcCol34);

                            TableCell tcCol35 = new TableCell();
                            tcCol35.HorizontalAlign = HorizontalAlign.Right;
                            tcCol35.Width = Unit.Percentage(6);
                            tcCol35.Text = (Convert.ToInt32(dtRow["Practical"]) * practicalfee).ToString("F");
                            TotalPracticalFee += Convert.ToDecimal(tcCol35.Text);
                            tr.Cells.Add(tcCol35);

                            TableCell tcCol36 = new TableCell();
                            tcCol36.HorizontalAlign = HorizontalAlign.Right;
                            tcCol36.Width = Unit.Percentage(6);
                            tcCol36.Text = (Convert.ToInt32(dtRow["Count"]) * processingfee).ToString("F");
                            TotalProcessingFee += Convert.ToDecimal(tcCol36.Text);
                            tr.Cells.Add(tcCol36);

                            TableCell tcCol37 = new TableCell();
                            tcCol37.HorizontalAlign = HorizontalAlign.Right;
                            tcCol37.Width = Unit.Percentage(6);
                            tcCol37.Text = Convert.ToInt32(dtRow["late_fee"]).ToString("F");
                            TotalLateFee += Convert.ToDecimal(tcCol37.Text);
                            tr.Cells.Add(tcCol37);
                        }

                        TableCell tcCol8 = new TableCell();
                        tcCol8.Width = Unit.Percentage(8);
                        tcCol8.HorizontalAlign = HorizontalAlign.Right;
                        tcCol8.Text = Convert.ToInt64(dtRow["FeeAmount"]).ToString("F");
                        TotalAmount += Convert.ToDecimal(tcCol8.Text);
                        tr.Cells.Add(tcCol8);

                        TableCell tcCol9 = new TableCell();
                        tcCol9.Width = Unit.Percentage(20);
                        tcCol9.HorizontalAlign = HorizontalAlign.Left;
                        tcCol9.Text = (String.IsNullOrEmpty(dtRow["StatusMessage"].ToString()) ? "Failed" : EConnect.Utils.Common.EnumUtility.GetDescription((enmPaymentStatus)(Convert.ToInt32(dtRow["StatusMessage"]))).ToString());
                        tr.Cells.Add(tcCol9);


                        tbl.Rows.Add(tr);
                        totamt += Convert.ToDecimal(dtRow["FeeAmount"]);
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
                    tdNewRow1.ColumnSpan = 3;
                    tdNewRow1.HorizontalAlign = HorizontalAlign.Center;
                    trNew.Cells.Add(tdNewRow1);

                    TableCell tdNewRow5 = new TableCell();
                    tdNewRow5.Width = Unit.Percentage(1);
                    tdNewRow5.Text = "<b>" + noofCandidates.ToString() + "</b>";
                    tdNewRow5.HorizontalAlign = HorizontalAlign.Right;
                    trNew.Cells.Add(tdNewRow5);

                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    {
                        TableCell tdNewRow6 = new TableCell();
                        tdNewRow6.Width = Unit.Percentage(1);
                        tdNewRow6.Text = "<b>" + noofTheoryModules.ToString() + "</b>";
                        tdNewRow6.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow6);

                        TableCell tdNewRow7 = new TableCell();
                        tdNewRow7.Width = Unit.Percentage(1);
                        tdNewRow7.Text = "<b>" + noofPracticalModules.ToString() + "</b>";
                        tdNewRow7.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow7);

                        TableCell tdNewRow8 = new TableCell();
                        tdNewRow8.Width = Unit.Percentage(1);
                        tdNewRow8.Text = "<b>" + TotaltheoryFee.ToString() + "</b>";
                        tdNewRow8.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow8);

                        TableCell tdNewRow9 = new TableCell();
                        tdNewRow9.Width = Unit.Percentage(1);
                        tdNewRow9.Text = "<b>" + TotalPracticalFee.ToString() + "</b>";
                        tdNewRow9.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow9);

                        TableCell tdNewRow10 = new TableCell();
                        tdNewRow10.Width = Unit.Percentage(1);
                        tdNewRow10.Text = "<b>" + TotalProcessingFee.ToString() + "</b>";
                        tdNewRow10.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow10);

                        TableCell tdNewRow11 = new TableCell();
                        tdNewRow11.Width = Unit.Percentage(1);
                        tdNewRow11.Text = "<b>" + TotalLateFee.ToString() + "</b>";
                        tdNewRow11.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow11);
                    }

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
                strHead1 += "<b>Total Amount Of Successful Transactions:</b> " + totamt.ToString("F");
            }
            else if (paymentMode == enmPaymentMode.NEFTRTGS)
            {
                if (DateType == "P")
                {
                    strHead += "<br/><b> Date Type :</b> Payment Date";
                }
                if (DateType == "SV")
                {
                    strHead += "<br/><b> Date Type :</b> Verified Date";
                }

                mySql.Append(" select CAST(T.Transaction_Date as Date) as TransactionDate,  COUNT(*) as Count,C.Course_ID as coursecode, CAST(T.Date as Date) as VerifiedDate ");
                if (applicationType == enmApplicationType.CourseExamApplication)
                    mySql.Append(",  sum(C.Fee_Amount) as  FeeAmount, sum(C.Number_of_th_Modules) as Theory , sum(C.Number_of_Pr_Modules) as Practical , sum(IsNull(C.Late_Fee_Amount,0)) as late_fee , C.Is_Improvement as Improvement ");
                else if (applicationType == enmApplicationType.CertificateExamApplication)
                    mySql.Append(", sum(C.Total_Fee_Amt) as FeeAmount");
                else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                    mySql.Append(", sum(C.Fee_Amt) as FeeAmount ");

                mySql.Append(" , D.Status_ID as StatusMessage from NEFT_Transaction T,  Demand_Note d, Course R ");
                if (applicationType == enmApplicationType.CourseExamApplication)
                    mySql.Append(", Course_Exam_Application C ");
                else if (applicationType == enmApplicationType.CertificateExamApplication)
                    mySql.Append(", Certificate_Exam_Application C ");
                else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                    mySql.Append(", Course_Registration_Application C ");

                if (DateType != "0")
                {
                    if (DateType == "P")
                    {
                        mySql.Append(" Where  D.ID = T.Demand_Note_ID and d.Application_Type_ID = " + AppTypeId.ToString() + " and " +
                             " CAST(t.Transaction_Date as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and CAST(T.Transaction_Date as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' ");
                    }
                    if (DateType == "SV")
                    {
                        mySql.Append(" Where  D.ID = T.Demand_Note_ID and d.Application_Type_ID = " + AppTypeId.ToString() + " and " +
                             " CAST(t.Date as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and CAST(T.Date as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' ");
                    }
                }

                mySql.Append(" and C.Demand_Note_ID = D.ID and C.Course_ID = R.ID");

                if (TransTypeId != 0)
                    mySql.Append(" and D.Demand_Note_Type_ID = " + TransTypeId.ToString());
                if (CourseId != 0)
                    mySql.Append(" and C.Course_ID = " + CourseId.ToString());
                if (CourseCatId != 0)
                    mySql.Append(" and C.Course_Category_ID = " + CourseCatId.ToString());
                if (PayStatusId != "0")
                {
                    if (PayStatusId == "S")
                        mySql.Append(" and D.Status_ID =" + Convert.ToInt32(enmPaymentStatus.Paid));
                    if (PayStatusId == "F")
                        mySql.Append(" and (D.Status_ID =" + Convert.ToInt32(enmPaymentStatus.Failed) + ")");
                }
                if (applicationType == enmApplicationType.CourseExamApplication)
                {
                    mySql.Append(" group by CAST(t.Date as DATE) ,  D.Status_ID , C.Course_ID, CAST(T.Transaction_Date as Date) , C.Is_Improvement ");
                }
                else if (applicationType == enmApplicationType.CertificateExamApplication)
                {
                    mySql.Append(" group by CAST(t.Date as DATE) ,  D.Status_ID , C.Course_ID, CAST(T.Transaction_Date as Date) ");
                }
                else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                {
                    mySql.Append(" group by CAST(t.Date as DATE) ,  D.Status_ID , C.Course_ID, CAST(T.Transaction_Date as Date) ");
                }
                mySql.Append(" order by  CAST(t.Date as DATE) ");
                dt = EConnect.Utils.Data.DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
                if (dt.Rows.Count > 0)
                {
                    ShowTableHeader();
                    foreach (DataRow dtRow in dt.Rows)
                    {
                        TableRow tr = new TableRow();
                        if (i % 2 == 0)
                            tr.CssClass = "gdalternate1";
                        else
                            tr.CssClass = "gdrow1";
                        i++;
                        TableHeaderCell tcCol1 = new TableHeaderCell();
                        tcCol1.Width = Unit.Percentage(2);
                        tcCol1.HorizontalAlign = HorizontalAlign.Left;
                        tcCol1.Text = i.ToString();
                        tr.Cells.Add(tcCol1);


                        TableCell tcCol25 = new TableCell();
                        tcCol25.Width = Unit.Percentage(5);
                        tcCol25.HorizontalAlign = HorizontalAlign.Center;
                        // tcCol25.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller("select code from course where id=" + dtRow["coursecode"].ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false).ToString();
                        //December_2024
                        SqlParameter[] param19 = { new SqlParameter("@courseCode", dtRow["coursecode"].ToString()) };
                        tcCol25.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller("select code from course where id=@courseCode ", new EConnect.Connections.SqlCon(), param19, CommandType.Text, false).ToString();
                        tr.Cells.Add(tcCol25);

                        TableCell tcCol = new TableCell();
                        tcCol.Width = Unit.Percentage(5);
                        tcCol.HorizontalAlign = HorizontalAlign.Center;
                        tcCol.Text = Convert.ToDateTime(dtRow["TransactionDate"]).ToString("dd-MMM-yyyy");
                        tr.Cells.Add(tcCol);


                        TableCell tcCol16 = new TableCell();
                        tcCol16.Width = Unit.Percentage(5);
                        tcCol16.HorizontalAlign = HorizontalAlign.Center;
                        tcCol16.Text = Convert.ToDateTime(dtRow["VerifiedDate"]).ToString("dd-MMM-yyyy");
                        tr.Cells.Add(tcCol16);

                        TableCell tcCol2 = new TableCell();
                        tcCol2.Width = Unit.Percentage(8);
                        tcCol2.HorizontalAlign = HorizontalAlign.Right;
                        tcCol2.Text = dtRow["Count"].ToString();
                        noofCandidates += Convert.ToInt64(dtRow["Count"]);
                        tr.Cells.Add(tcCol2);


                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {
                            Int32 theoryfee = 0;
                            Int32 practicalfee = 0;
                            Boolean isImprovement = Convert.ToBoolean(dtRow["Improvement"]);
                            if (isImprovement == true)
                            {
                                //theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount  from Fee_Detail where Course_ID =" + dtRow["coursecode"].ToString() + " and Effective_To_Date is null  and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount  from Fee_Detail where Course_ID =" + dtRow["coursecode"].ToString() +
                                //    "and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" +
                                //"and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //December_2024
                                SqlParameter[] param20 = { new SqlParameter("@courseCode", dtRow["coursecode"].ToString()),
                                                            new SqlParameter("@TransactionDate", dtRow["TransactionDate"].ToString()),
                                                                new SqlParameter("@improvementFeeType", improvementfeetype)
                                                                    };
                                theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount  from Fee_Detail where Course_ID = @courseCode " +
                                    "and" + " (( @TransactionDate >= [Effective_From_Date] and @TransactionDate <= Effective_To_Date) " + " OR (Effective_To_Date IS NULL AND @TransactionDate>= [Effective_From_Date]))" +
                                " and Fee_Type_ID =@improvementFeeType ", new EConnect.Connections.SqlCon(), param20, CommandType.Text, false));


                                //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() + " and Effective_To_Date is null  and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() +
                                //    "and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" +
                                //    "and Fee_Type_ID =" + improvementfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //December_2024
                                SqlParameter[] param21 = { new SqlParameter("@courseCode",  dtRow["coursecode"].ToString()),
                                                            new SqlParameter("@TransactionDate", dtRow["TransactionDate"].ToString()),
                                                                new SqlParameter("@improvementFeeType", improvementfeetype)
                                                                    };
                                practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = @courseCode " +
                                "and" + " (( @TransactionDate >= [Effective_From_Date] and @TransactionDate <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND @TransactionDate>= [Effective_From_Date]))" +
                                "and Fee_Type_ID =@improvementFeeType ", new EConnect.Connections.SqlCon(), param21, CommandType.Text, false));


                            }
                            else
                            {
                                //theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount  from Fee_Detail where Course_ID =" + dtRow["coursecode"].ToString() + " and Effective_To_Date is null  and Fee_Type_ID =" + theoryfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount  from Fee_Detail where Course_ID =" + dtRow["coursecode"].ToString() +
                                //    "and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" +
                                //"and Fee_Type_ID =" + theoryfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //December_2024
                                SqlParameter[] param22 = { new SqlParameter("@courseCode",  dtRow["coursecode"].ToString()),
                                                            new SqlParameter("@TransactionDate", dtRow["TransactionDate"].ToString()),
                                                                new SqlParameter("@theoryFeeType", theoryfeetype)
                                                                    };
                                theoryfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount  from Fee_Detail where Course_ID =@courseCode " +
                                    "and" + " (( @TransactionDate >= [Effective_From_Date] and @TransactionDate <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND @TransactionDate >= [Effective_From_Date]))" +
                                "and Fee_Type_ID = @theoryFeeType ", new EConnect.Connections.SqlCon(), param22, CommandType.Text, false));


                                //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() + " and Effective_To_Date is null  and Fee_Type_ID =" + practicalfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() + 
                                //    "and" + "(( '" + dtRow["TransactionDate"].ToString() + "' >= [Effective_From_Date] and '" + dtRow["TransactionDate"].ToString() + "' <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND '" + dtRow["TransactionDate"].ToString() + "'>= [Effective_From_Date]))" + 
                                //    "and Fee_Type_ID =" + practicalfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                //December_2024
                                SqlParameter[] param23 = { new SqlParameter("@courseCode",  dtRow["coursecode"].ToString()),
                                                            new SqlParameter("@TransactionDate", dtRow["TransactionDate"].ToString()),
                                                                new SqlParameter("@practicalFeeType", practicalfeetype)
                                                                    };
                                practicalfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = @courseCode " +
                                "and" + " (( @TransactionDate >= [Effective_From_Date] and @TransactionDate <= Effective_To_Date) " + "OR (Effective_To_Date IS NULL AND @TransactionDate >= [Effective_From_Date]))" +
                                "and Fee_Type_ID =@practicalFeeType ", new EConnect.Connections.SqlCon(), param23, CommandType.Text, false));
                            }

                            // Int32 processingfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = " + dtRow["coursecode"].ToString() + " and Effective_To_Date is null  and Fee_Type_ID =" + processingfeetype, new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                            //December_2024
                            SqlParameter[] param24 = {
                                                        new SqlParameter("@courseCode", dtRow["coursecode"].ToString()),
                                                            new SqlParameter("@processingFeeType", processingfeetype)
                                                                };
                            Int32 processingfee = Convert.ToInt32(EConnect.Utils.Data.DbUtility.ExecuteScaller("select Fee_Amount from Fee_Detail where Course_ID = @courseCode and Effective_To_Date is null  and Fee_Type_ID =@processingFeeType ", new EConnect.Connections.SqlCon(), param24, CommandType.Text, false));


                            TableCell tcCol3 = new TableCell();
                            tcCol3.HorizontalAlign = HorizontalAlign.Right;
                            tcCol3.Width = Unit.Percentage(6);
                            tcCol3.Text = dtRow["Theory"].ToString();
                            noofTheoryModules += Convert.ToInt64(dtRow["Theory"]);
                            tr.Cells.Add(tcCol3);

                            TableCell tcCol4 = new TableCell();
                            tcCol4.HorizontalAlign = HorizontalAlign.Right;
                            tcCol4.Width = Unit.Percentage(6);
                            tcCol4.Text = dtRow["Practical"].ToString();
                            noofPracticalModules += Convert.ToInt64(dtRow["Practical"]);
                            tr.Cells.Add(tcCol4);

                            TableCell tcCol34 = new TableCell();
                            tcCol34.HorizontalAlign = HorizontalAlign.Right;
                            tcCol34.Width = Unit.Percentage(6);
                            tcCol34.Text = (Convert.ToInt32(dtRow["Theory"]) * theoryfee).ToString("F");
                            TotaltheoryFee += Convert.ToDecimal(tcCol34.Text);
                            tr.Cells.Add(tcCol34);

                            TableCell tcCol35 = new TableCell();
                            tcCol35.HorizontalAlign = HorizontalAlign.Right;
                            tcCol35.Width = Unit.Percentage(6);
                            tcCol35.Text = (Convert.ToInt32(dtRow["Practical"]) * practicalfee).ToString("F");
                            TotalPracticalFee += Convert.ToDecimal(tcCol35.Text);
                            tr.Cells.Add(tcCol35);

                            TableCell tcCol36 = new TableCell();
                            tcCol36.HorizontalAlign = HorizontalAlign.Right;
                            tcCol36.Width = Unit.Percentage(6);
                            tcCol36.Text = (Convert.ToInt32(dtRow["Count"]) * processingfee).ToString("F");
                            TotalProcessingFee += Convert.ToDecimal(tcCol36.Text);
                            tr.Cells.Add(tcCol36);

                            TableCell tcCol37 = new TableCell();
                            tcCol37.HorizontalAlign = HorizontalAlign.Right;
                            tcCol37.Width = Unit.Percentage(6);
                            tcCol37.Text = Convert.ToInt32(dtRow["late_fee"]).ToString("F");
                            TotalLateFee += Convert.ToDecimal(tcCol37.Text);
                            tr.Cells.Add(tcCol37);
                        }


                        TableCell tcCol8 = new TableCell();
                        tcCol8.Width = Unit.Percentage(8);
                        tcCol8.HorizontalAlign = HorizontalAlign.Right;
                        tcCol8.Text = Convert.ToInt64(dtRow["FeeAmount"]).ToString("F");
                        TotalAmount += Convert.ToDecimal(tcCol8.Text);
                        tr.Cells.Add(tcCol8);

                        TableCell tcCol9 = new TableCell();
                        tcCol9.Width = Unit.Percentage(20);
                        tcCol9.HorizontalAlign = HorizontalAlign.Left;
                        tcCol9.Text = (String.IsNullOrEmpty(dtRow["StatusMessage"].ToString()) ? "Failed" : EConnect.Utils.Common.EnumUtility.GetDescription((enmPaymentStatus)(Convert.ToInt32(dtRow["StatusMessage"]))).ToString());
                        tr.Cells.Add(tcCol9);


                        tbl.Rows.Add(tr);
                        totamt += Convert.ToDecimal(dtRow["FeeAmount"]);
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

                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    {
                        TableCell tdNewRow6 = new TableCell();
                        tdNewRow6.Width = Unit.Percentage(1);
                        tdNewRow6.Text = "<b>" + noofTheoryModules.ToString() + "</b>";
                        tdNewRow6.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow6);

                        TableCell tdNewRow7 = new TableCell();
                        tdNewRow7.Width = Unit.Percentage(1);
                        tdNewRow7.Text = "<b>" + noofPracticalModules.ToString() + "</b>";
                        tdNewRow7.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow7);

                        TableCell tdNewRow8 = new TableCell();
                        tdNewRow8.Width = Unit.Percentage(1);
                        tdNewRow8.Text = "<b>" + TotaltheoryFee.ToString() + "</b>";
                        tdNewRow8.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow8);

                        TableCell tdNewRow9 = new TableCell();
                        tdNewRow9.Width = Unit.Percentage(1);
                        tdNewRow9.Text = "<b>" + TotalPracticalFee.ToString() + "</b>";
                        tdNewRow9.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow9);

                        TableCell tdNewRow10 = new TableCell();
                        tdNewRow10.Width = Unit.Percentage(1);
                        tdNewRow10.Text = "<b>" + TotalProcessingFee.ToString() + "</b>";
                        tdNewRow10.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow10);

                        TableCell tdNewRow11 = new TableCell();
                        tdNewRow11.Width = Unit.Percentage(1);
                        tdNewRow11.Text = "<b>" + TotalLateFee.ToString() + "</b>";
                        tdNewRow11.HorizontalAlign = HorizontalAlign.Right;
                        trNew.Cells.Add(tdNewRow11);
                    }

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
                strHead1 += "<b>Total Amount Of Successful Transactions: </b>" + totamt.ToString("F");
            }
            LblRptSubHeader.Text = strHead;
            LblRptSubHeader1.Text = strHead1;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }
        finally { context.Dispose(); }
    }
}