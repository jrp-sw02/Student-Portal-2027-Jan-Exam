using System;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Web;

public partial class HO_Rpt_PaymentRecieptReport : BasePage
{
    Table tbl = new Table();
    public static int j;
    String PayStatusId = "0";
    String DateType = "0";
    String ManualSettled = "0";
    Int32 currentRoleId = 0;
    Int32 regcentreid = 0;
    int PayModeId = 0;
    StringBuilder str = new StringBuilder();

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
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/PaymntTransactionReciept.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                newShowData();
                tbl.CssClass = "sample3";
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(100);
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
            tcCol1.HorizontalAlign = HorizontalAlign.Left;
            tcCol1.Text = "#";
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(5);
            tcCol.HorizontalAlign = HorizontalAlign.Left;
            tcCol.Text = "Course";
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(10);
            tcCol2.HorizontalAlign = HorizontalAlign.Left;
            tcCol2.Text = "Payee Type";
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.HorizontalAlign = HorizontalAlign.Left;
            tcCol3.Width = Unit.Percentage(22);
            tcCol3.Text = "Payee Detail";
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.HorizontalAlign = HorizontalAlign.Left;
            tcCol4.Width = Unit.Percentage(15);
            tcCol4.Text = "Demand Note Detail";
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(17);
            tcCol5.HorizontalAlign = HorizontalAlign.Left;
            tcCol5.Text = "Transaction No.";
            th.Cells.Add(tcCol5);

            if (PayModeId == Convert.ToInt32(enmPaymentMode.NEFTRTGS))
            {
                TableHeaderCell tcCol6 = new TableHeaderCell();
                tcCol6.Width = Unit.Percentage(12);
                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                tcCol6.Text = "Payment Date";
                th.Cells.Add(tcCol6);
            }
            else
            {
                TableHeaderCell tcCol6 = new TableHeaderCell();
                tcCol6.Width = Unit.Percentage(12);
                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                tcCol6.Text = "Transaction Date";
                th.Cells.Add(tcCol6);
            }


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

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(8);
            tcCol7.HorizontalAlign = HorizontalAlign.Right;
            tcCol7.Text = "Amount";
            th.Cells.Add(tcCol7);

            TableHeaderCell tcCol8 = new TableHeaderCell();
            tcCol8.Width = Unit.Percentage(8);
            tcCol8.HorizontalAlign = HorizontalAlign.Right;
            tcCol8.Text = "Fee Amount";
            th.Cells.Add(tcCol8);

            if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                TableHeaderCell tcCol10 = new TableHeaderCell();
                tcCol10.Width = Unit.Percentage(10);
                tcCol10.HorizontalAlign = HorizontalAlign.Right;
                tcCol10.Text = "Theory Fee";
                th.Cells.Add(tcCol10);

                TableHeaderCell tcCol11 = new TableHeaderCell();
                tcCol11.Width = Unit.Percentage(10);
                tcCol11.HorizontalAlign = HorizontalAlign.Right;
                tcCol11.Text = "Practical Fee";
                th.Cells.Add(tcCol11);

                TableHeaderCell tcCol12 = new TableHeaderCell();
                tcCol12.Width = Unit.Percentage(10);
                tcCol12.HorizontalAlign = HorizontalAlign.Right;
                tcCol12.Text = "Late Fee";
                th.Cells.Add(tcCol12);
            }
            //--vaf acf start--------------------------------------------------------------------
            if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
            {
                TableHeaderCell tcCol10 = new TableHeaderCell();
                tcCol10.Width = Unit.Percentage(10);
                tcCol10.HorizontalAlign = HorizontalAlign.Right;
                tcCol10.Text = "VAF Fee";
                th.Cells.Add(tcCol10);

                TableHeaderCell tcCol11 = new TableHeaderCell();
                tcCol11.Width = Unit.Percentage(10);
                tcCol11.HorizontalAlign = HorizontalAlign.Right;
                tcCol11.Text = "ACF Fee";
                th.Cells.Add(tcCol11);



            }
            //---vaf acf end--------------------------------------------------------------------

            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(6); // added by amit 
            tcCol9.HorizontalAlign = HorizontalAlign.Left;
            tcCol9.Text = "Payment Status";
            th.Cells.Add(tcCol9);

            //--vaf acf start--------------------------------------------------------------------
            if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
            {
                TableHeaderCell tcCol12 = new TableHeaderCell();
                tcCol12.Width = Unit.Percentage(10);
                tcCol12.HorizontalAlign = HorizontalAlign.Right;
                tcCol12.Text = "Count of Candidates";
                th.Cells.Add(tcCol12);

                TableHeaderCell tcCol13 = new TableHeaderCell();
                tcCol13.Width = Unit.Percentage(15);
                tcCol13.HorizontalAlign = HorizontalAlign.Right;
                tcCol13.Text = "ACCR No.";
                th.Cells.Add(tcCol13);

            }
            //---vaf acf end--------------------------------------------------------------------

            // added by amit start
            TableHeaderCell tcnCol = new TableHeaderCell();
            tcnCol.Width = Unit.Percentage(6);
            tcnCol.HorizontalAlign = HorizontalAlign.Left;
            tcnCol.Text = "Payment Gateway";
            th.Cells.Add(tcnCol);
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
            Response.AddHeader("content-disposition", "attachment;filename=Applications.xls");
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
        EConnectContext context = new EConnectContext();
        try
        {
            int i = 0;
            Decimal totamt = 0;
            DataTable dt;
            PayModeId = Convert.ToInt32(Request.QueryString["PayModeId"]);
            enmPaymentMode paymentMode = (enmPaymentMode)PayModeId;
            int TransTypeId = Convert.ToInt32(Request.QueryString["TransTypeId"]);
            int CourseCatId = Convert.ToInt32(Request.QueryString["CourseCatId"]);
            int CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            int AppTypeId = Convert.ToInt32(Request.QueryString["AppTypeId"]);
            Int64 transId = Convert.ToInt64(Request.QueryString["Transno"]);
            string ExamIDString = string.Empty;
            String Exam = string.Empty;
            if (Request.QueryString["ExamID"] != "0")
            {
                if (Request.QueryString["ExamID"].ToString().Contains("Level"))
                    Exam = Request.QueryString["ExamID"].ToString().Substring(11).ToString();
                else
                    Exam = Request.QueryString["ExamID"].ToString();
                Int32 ExamMonth = Convert.ToDateTime("01-" + Exam.ToString().Split(',')[0] + "-2011").Month;
                Int32 ExamYear = Convert.ToInt32(Exam.ToString().Split(',')[1]);
                var ExamID = context.Exams.Where(a => a.ExamYear == ExamYear && a.ExamMonth == ExamMonth && a.CourseCategoryID == CourseCatId);
                if (CourseId != 0)
                    ExamID = ExamID.Where(a => a.CourseID == CourseId);
                foreach (var aa in ExamID)
                {
                    ExamIDString += aa.ID + ",";
                }
            }
            enmApplicationType applicationType = (enmApplicationType)AppTypeId;
            PayStatusId = Request.QueryString["PayStatusId"];
            DateType = Request.QueryString["Datetype"];
            ManualSettled = Request.QueryString["ManualSettled"];
            regcentreid = Convert.ToInt32(Request.QueryString["Regcentreid"]);
            DateTime PayFromDate = Convert.ToDateTime(Request.QueryString["PayFromDate"]);
            DateTime PayToDate = Convert.ToDateTime(Request.QueryString["PayToDate"]);
            String gateway = Convert.ToString(Request.QueryString["Gateway"]); // added by amit 

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

            if (Request.QueryString["ExamID"] != "0")
            {
                strHead += "<br/><b>Exam Name :</b>" + Request.QueryString["ExamID"].ToString();
            }
            else
            {
                strHead += "<br/><b>Exam Name :</b> All";
            }

            if (applicationType == enmApplicationType.CertificateExamApplication)
            {
                if (regcentreid != 0)
                {
                    var regcenter = context.RegionalCenters.Find(regcentreid);
                    if (regcenter != null)
                    {
                        strHead += "<br/><b>Regional Center :</b>" + regcenter.Name;
                    }
                }
                else
                {
                    strHead += "<br/><b>Regional Center :</b> All";
                }
            }

            if (TransTypeId == 0)
                strHead += "<br/><b>Transaction Type :</b>  All";
            else if (TransTypeId == 1)
                strHead += "<br/><b>Transaction Type :</b> Single";
            else if (TransTypeId == 2)
                strHead += "<br/><b>Transaction Type :</b> Multiple";


            if (paymentMode == enmPaymentMode.Online)
            {

                /* added by amit start */
                if (gateway == "1")
                    strHead += "<br/><b>Gateway Used : </b>BillDesk ";
                else if (gateway == "2")
                    strHead += "<br/><b>Gateway Used : </b>ICIC ";
                else
                    strHead += "<br/><b>Gateway Used : </b>ALL";
                /* added by amit end */


                //showing only when payment mode is online
                if (ManualSettled == "F")
                {
                    ibtext.Visible = false;
                    strHead += "<br/><b> Is Settled :</b> False";
                }
                else if (ManualSettled == "T")
                {
                    ibtext.Visible = true;
                    strHead += "<br/><b> Is Settled :</b> True";
                }

                if (DateType == "P")
                {
                    strHead += "<br/><b> Date Type :</b> Payment Date";
                }
                if (DateType == "SV")
                {
                    strHead += "<br/><b> Date Type :</b> Settled Date";
                }
                //for vaf acf query has been changed (begin)
                if (applicationType == enmApplicationType.CourseVafAcfApplication)
                {
                    mySql.Append("with base as ( select Distinct T.ID as TransactionID, d.Demand_Note_Type_ID as PayeeType,  D.Application_Type_ID as applicationTypeID , D.Fee_Type_ID as FeeTypeID," +
                    "   T.Request_Date as TransactionDate,T.Settled_On as SettlementDate , T.Demand_Note_ID as DemandID, T.Amount as TransactionAmount, D.Date as DemandNoteDate, " +
                    "   CONVERT(VARCHAR,T.Response_Date, 112) AS txn_date ,  T.Reference_Number as Reference_Number, ceva.VAF_Amount as VAF , " +
                    "   ceva.ACF_Amount as ACF, ceva.Course_Category_ID as CourseCategoryID, ceva.Course_ID as CourseID , ceva.Exam_ID as ExamID ,T.Response_Status_Code as Status, T.Response_Status_Message as StatusMessage,T.Settled_On as SettledDate , T.Is_Settled as IsSettled, D.Amount as  FeeAmount, accr.Accreditation_Number as AccrNo " +
                    "   from Online_Transaction T inner join " +
                    "    Demand_Note D on T.Demand_Note_ID = D.ID inner join " +
                    "   Course_Exam_VAF_ACF ceva on ceva.Demand_Note_ID = D.ID inner join  Intitute_Accreditation_Detail accr on accr.Institute_ID = ceva.Institute_ID where accr.Course_ID = ceva.Course_ID) select TransactionID , DemandNoteDate ,applicationTypeID ,FeeTypeID, StatusMessage, Reference_Number, (select  Code from Course c where c.ID = CourseID ) as CourseName, case PayeeType when '1' then 'Candidate' else 'Institute' end as PayeeType, TransactionDate, DemandID, TransactionAmount, txn_date, Reference_Number, VAF, ACF, Status ,  SettledDate, FeeAmount, AccrNo " +
                    "   from base where  ");

                    if (DateType != "0")
                    {
                        if (DateType == "P")
                        {
                            mySql.Append(" CAST(TransactionDate as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and  CAST(TransactionDate as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "'");
                        }
                        if (DateType == "SV")
                        {
                            mySql.Append(" CAST(SettlementDate as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and  CAST(SettlementDate as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "'");
                        }
                    }
                    if (TransTypeId != 0)
                    {
                        mySql.Append("and PayeeType = '" + TransTypeId.ToString() + "'");
                    }
                    if (CourseId != 0)
                    {
                        mySql.Append(" and CourseID =" + CourseId.ToString());
                    }
                    if (CourseCatId != 0)
                    {
                        mySql.Append(" and CourseCategoryID = " + CourseCatId.ToString());
                    }
                    if (!String.IsNullOrEmpty(ExamIDString))
                    {
                        mySql.Append(" and ExamID in (" + ExamIDString.TrimEnd(',').ToString() + ")");
                    }
                    if (PayStatusId != "0")
                    {
                        if (PayStatusId == "S")
                            mySql.Append(" and Status = '0300'");
                        if (PayStatusId == "F")
                            mySql.Append(" and (Status  <> '0300' or Status is NULL)");
                    }
                    if (ManualSettled != "0")
                    {
                        if (ManualSettled == "F")
                            mySql.Append(" and IsSettled = 0");
                        if (ManualSettled == "T")
                            mySql.Append(" and IsSettled = 1");
                    }
                    if (TransTypeId != 0)
                    {
                        mySql.Append(" and PayeeType =" + TransTypeId.ToString());
                    }
                    if (transId != 0)
                    {
                        var demandnoteid = (from r in context.OnlineTransaction.AsNoTracking()
                                            where r.ID == transId && System.Data.Entity.DbFunctions.TruncateTime(r.RequestDate) >= System.Data.Entity.DbFunctions.TruncateTime(PayFromDate) && System.Data.Entity.DbFunctions.TruncateTime(r.RequestDate) <= System.Data.Entity.DbFunctions.TruncateTime(PayToDate)
                                            select r.DemandNoteID).FirstOrDefault();
                        if (demandnoteid != null)
                        {
                            mySql.Append(" and DemandID=" + demandnoteid);
                        }
                    }
                    mySql.Append("  order by TransactionDate , TransactionID  ");
                }
                //for vaf acf query has been changed (end)
                else
                {

                    mySql.Append(" select T.ID as TransactionID, case d.Demand_Note_Type_ID when '1' then 'Candidate' else 'Institute' end as PayeeType,T.Request_Date as TransactionDate, T.Demand_Note_ID, T.Amount as TransactionAmount, CONVERT(VARCHAR,T.Response_Date, 112) AS txn_date , " +
                                 " T.Reference_Number as Reference_Number, C.ID as ApplID, T.Response_Status_Code as Status, R.Code as CourseName , R.ID as CourseID, T.Settled_On as SettledDate ");

                    //added by amit start
                    mySql.Append(" ,isnull((select description from paymentgateways pg where id = T.PGCode), 'EFT') as [PaymentGatewayUsed] ");
                    // added by amit end

                    if (applicationType == enmApplicationType.CourseExamApplication)
                        mySql.Append(",  C.Fee_Amount as  FeeAmount , C.Number_of_th_Modules as Theory , C.Number_of_Pr_Modules as practical , ISNULL(C.Late_Fee_Amount, 0.00) as LateFee, C.Is_Improvement as Improvement");
                    else if (applicationType == enmApplicationType.CertificateExamApplication)
                        mySql.Append(", C.Total_Fee_Amt as FeeAmount ");
                    else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                        mySql.Append(", C.Fee_Amt as FeeAmount ");
                    //Added for project applications 10 Oct 2023

                    else if (applicationType == enmApplicationType.CourseProjectApplication)
                        mySql.Append(", C.Fee_Amount as FeeAmount ");

                    mySql.Append(", D.Date as DemandNoteDate, D.ID as DemandID, D.Fee_Type_ID, Demand_Note_Type_ID, D.Application_Type_ID, Online_Transaction_ID, T.Response_Status_Message as StatusMessage " +
                     " from Online_Transaction T,  Demand_Note d, Course R ");

                    if (applicationType == enmApplicationType.CourseExamApplication)
                        mySql.Append(", Course_Exam_Application C ");
                    else if (applicationType == enmApplicationType.CertificateExamApplication)
                        mySql.Append(", Certificate_Exam_Application C ");
                    else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                        mySql.Append(", Course_Registration_Application C ");
                    //Added for project applications 10 Oct 2023
                    else if (applicationType == enmApplicationType.CourseProjectApplication)
                        mySql.Append(", Course_Project_Application C ");
                    //




                    if (DateType != "0")
                    {
                        if (DateType == "P")
                        {
                            mySql.Append(" Where D.ID = T.Demand_Note_ID and d.Application_Type_ID = " + AppTypeId.ToString() + " and " +
                              "CAST(T.Request_Date as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and CAST(T.Request_Date as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' ");
                        }
                        if (DateType == "SV")
                        {
                            //mySql.Append(" Where  D.ID = T.Demand_Note_ID and d.Application_Type_ID = " + AppTypeId.ToString() + " and " +
                            // "CAST(T.Settled_On as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and CAST(T.Settled_On as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' ");

                            mySql.Append(" WHERE D.ID = T.Demand_Note_ID AND d.Application_Type_ID = " + AppTypeId.ToString());
                            if (gateway == "1") // billdesk
                            {
                                mySql.Append(" AND CAST(T.Settled_On AS DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "'" +
                                                " AND CAST(T.Settled_On AS DATE) <='" + PayToDate.ToString("dd-MMM-yyyy") + "'");
                            }
                            else if (gateway == "2") // icici
                            {
                                mySql.Append(" AND CAST(T.Request_Date AS DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "'" +
                                                  " AND CAST(T.Request_Date AS DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "'");

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


                    mySql.Append(" and C.Demand_Note_ID = D.ID and C.Course_ID = R.ID");

                    if (TransTypeId != 0)
                        mySql.Append(" and D.Demand_Note_Type_ID = " + TransTypeId.ToString());
                    if (CourseId != 0)
                        mySql.Append(" and C.Course_ID = " + CourseId.ToString());
                    if (CourseCatId != 0)
                        mySql.Append(" and C.Course_Category_ID = " + CourseCatId.ToString());

                    if (!String.IsNullOrEmpty(ExamIDString))
                        mySql.Append(" and C.Exam_ID in (" + ExamIDString.TrimEnd(',').ToString() + ")");

                    /* changed by amit start */
                    if (PayStatusId != "0")
                    {
                        if (PayStatusId == "S")
                            mySql.Append(" and (T.Response_Status_Code = '0300' OR (T.Response_Status_Code = 'E000' AND T.Is_Settled = 1)  ) "); // changed by amit
                        if (PayStatusId == "F")
                            mySql.Append(" and (T.Response_Status_Code not in ('0300', 'E000') or T.Response_Status_Code not like '%Success%' )");
                    }
                    /* changed by amit end */

                    /* added by amit start */
                    if (gateway == "2")
                        mySql.Append(" and T.PGCode = 2"); // ICICI
                    else if (gateway == "1")
                        mySql.Append(" and (T.PGCode = 1 OR T.PGCode IS NULL)"); // Billdesk 
                    /* added by amit end */

                    if (applicationType == enmApplicationType.CertificateExamApplication)
                    {
                        if (regcentreid != 0)
                            mySql.Append(" and C.Regional_Center_ID = " + regcentreid.ToString());
                    }

                    if (ManualSettled != "0")
                    {
                        if (ManualSettled == "F")
                            mySql.Append(" and T.Is_Settled = 0");
                        if (ManualSettled == "T")
                            mySql.Append(" and T.Is_Settled = 1");
                    }

                    if (transId != 0)
                    {
                        var demandnoteid = (from r in context.OnlineTransaction.AsNoTracking()
                                            where r.ID == transId && System.Data.Entity.DbFunctions.TruncateTime(r.RequestDate) >= System.Data.Entity.DbFunctions.TruncateTime(PayFromDate) && System.Data.Entity.DbFunctions.TruncateTime(r.RequestDate) <= System.Data.Entity.DbFunctions.TruncateTime(PayToDate)
                                            select r.DemandNoteID).FirstOrDefault();

                        if (demandnoteid != null)
                        {
                            mySql.Append(" and t.Demand_Note_ID =" + demandnoteid);
                        }


                    }
                    mySql.Append("  order by TransactionDate , TransactionID ");
                }
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

                        TableCell tcCol = new TableCell();
                        tcCol.Width = Unit.Percentage(3);
                        tcCol.HorizontalAlign = HorizontalAlign.Left;
                        tcCol.Text = dtRow["CourseName"].ToString();
                        tr.Cells.Add(tcCol);

                        TableCell tcCol2 = new TableCell();
                        tcCol2.Width = Unit.Percentage(7);
                        tcCol2.HorizontalAlign = HorizontalAlign.Left;
                        tcCol2.Text = dtRow["PayeeType"].ToString();

                        tr.Cells.Add(tcCol2);

                        //------vaf acf start------------------
                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
                        {
                            TableCell tcCol3 = new TableCell();
                            tcCol3.HorizontalAlign = HorizontalAlign.Left;
                            tcCol3.Width = Unit.Percentage(33);
                            var demandNoteID = Convert.ToInt32(dtRow["DemandID"]);
                            var applID = Convert.ToInt32(context.CourseExamVafAcfs.Where(v => v.DemandNoteID == demandNoteID).Select(v => v.ID).FirstOrDefault());
                            tcCol3.Text = GetPayename(context, applID, Convert.ToInt32(dtRow["applicationTypeID"]), Convert.ToInt16(dtRow["FeeTypeID"]));
                            tr.Cells.Add(tcCol3);
                        }
                        //-------vaf acf end-------------------
                        else
                        {
                            TableCell tcCol3 = new TableCell();
                            tcCol3.HorizontalAlign = HorizontalAlign.Left;
                            tcCol3.Width = Unit.Percentage(33);
                            tcCol3.Text = GetPayename(context, Convert.ToInt64(dtRow["ApplID"]), Convert.ToInt32(dtRow["Application_Type_ID"]), Convert.ToInt16(dtRow["Fee_Type_ID"]));
                            tr.Cells.Add(tcCol3);
                        }
                        TableCell tcCol4 = new TableCell();
                        tcCol4.HorizontalAlign = HorizontalAlign.Left;
                        tcCol4.Width = Unit.Percentage(17);
                        tcCol4.Text = dtRow["DemandID"] + " / Dated:-" + Convert.ToDateTime(dtRow["DemandNoteDate"]).ToString("dd-MMM-yyyy");
                        tr.Cells.Add(tcCol4);

                        TableCell tcCol5 = new TableCell();
                        tcCol5.Width = Unit.Percentage(11);
                        tcCol5.HorizontalAlign = HorizontalAlign.Left;
                        tcCol5.Text = dtRow["TransactionID"].ToString() + " - " + (String.IsNullOrEmpty(dtRow["Reference_Number"].ToString()) ? "NA" : dtRow["Reference_Number"].ToString());
                        tr.Cells.Add(tcCol5);



                        TableCell tcCol6 = new TableCell();
                        tcCol6.Width = Unit.Percentage(15);
                        tcCol6.HorizontalAlign = HorizontalAlign.Center;
                        tcCol6.Text = Convert.ToDateTime(dtRow["TransactionDate"]).ToString("dd-MMM-yyyy hh:mm:ss");
                        tr.Cells.Add(tcCol6);


                        TableCell tcCol16 = new TableCell();
                        tcCol16.Width = Unit.Percentage(15);
                        tcCol16.HorizontalAlign = HorizontalAlign.Center;
                        tcCol16.Text = String.IsNullOrEmpty(dtRow["SettledDate"].ToString()) ? "NA" : Convert.ToDateTime(dtRow["SettledDate"]).ToString("dd-MMM-yyyy hh:mm:ss");
                        tr.Cells.Add(tcCol16);

                        TableCell tcCol7 = new TableCell();
                        tcCol7.Width = Unit.Percentage(7);
                        tcCol7.HorizontalAlign = HorizontalAlign.Right;
                        tcCol7.Text = Convert.ToInt64(dtRow["TransactionAmount"]).ToString("F");
                        tr.Cells.Add(tcCol7);

                        TableCell tcCol8 = new TableCell();
                        tcCol8.Width = Unit.Percentage(7);
                        tcCol8.HorizontalAlign = HorizontalAlign.Right;
                        tcCol8.Text = Convert.ToInt64(dtRow["FeeAmount"]).ToString("F");
                        tr.Cells.Add(tcCol8);
                        //------vaf acf start------------------
                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
                        {
                            Int32 VafFee = Convert.ToInt32(dtRow["VAF"]);
                            Int32 AcfFee = Convert.ToInt32(dtRow["ACF"]);

                            TableCell tcCol10 = new TableCell();
                            tcCol10.Width = Unit.Percentage(8);
                            tcCol10.HorizontalAlign = HorizontalAlign.Right;
                            tcCol10.Text = VafFee.ToString();
                            tr.Cells.Add(tcCol10);

                            TableCell tcCol11 = new TableCell();
                            tcCol11.Width = Unit.Percentage(8);
                            tcCol11.HorizontalAlign = HorizontalAlign.Right;
                            tcCol11.Text = AcfFee.ToString();
                            tr.Cells.Add(tcCol11);
                        }
                        //-------vaf acf end-------------------

                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {
                            Int32 CourseID = Convert.ToInt32(dtRow["CourseID"]);
                            Int32 feeTheory = 0;
                            Int32 feePractical = 0;
                            Boolean isImprovement = Convert.ToBoolean(dtRow["Improvement"]);
                            if (isImprovement == true)
                            {
                                feeTheory = CourseManager.GetAppliableFee(context, CourseID, enmFeeType.ImprovementOfPaperFee);
                                feePractical = CourseManager.GetAppliableFee(context, CourseID, enmFeeType.ImprovementOfPaperFee);
                            }
                            else
                            {
                                feeTheory = CourseManager.GetAppliableFee(context, CourseID, enmFeeType.ExaminationFee);
                                feePractical = CourseManager.GetAppliableFee(context, CourseID, enmFeeType.PracticalFee);
                            }

                            TableCell tcCol10 = new TableCell();
                            tcCol10.Width = Unit.Percentage(8);
                            tcCol10.HorizontalAlign = HorizontalAlign.Right;
                            tcCol10.Text = (Convert.ToSingle(feeTheory) * Convert.ToInt32(dtRow["Theory"])).ToString("F");
                            tr.Cells.Add(tcCol10);

                            TableCell tcCol11 = new TableCell();
                            tcCol11.Width = Unit.Percentage(8);
                            tcCol11.HorizontalAlign = HorizontalAlign.Right;
                            tcCol11.Text = (Convert.ToSingle(feePractical) * Convert.ToInt32(dtRow["practical"])).ToString("F");
                            tr.Cells.Add(tcCol11);

                            TableCell tcCol12 = new TableCell();
                            tcCol12.Width = Unit.Percentage(8);
                            tcCol12.HorizontalAlign = HorizontalAlign.Right;
                            tcCol12.Text = Convert.ToInt64(dtRow["LateFee"]).ToString("F");
                            tr.Cells.Add(tcCol12);
                        }

                        TableCell tcCol9 = new TableCell();
                        tcCol9.Width = Unit.Percentage(10);
                        tcCol9.HorizontalAlign = HorizontalAlign.Left;
                        tcCol9.Text = (String.IsNullOrEmpty(dtRow["StatusMessage"].ToString()) ? "Failed" : dtRow["StatusMessage"].ToString());
                        tr.Cells.Add(tcCol9);

                        //------vaf acf start------------------
                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
                        {
                            var demandNoteID = Convert.ToInt32(dtRow["DemandID"]);
                            var countOfCandidateIfVafAcfTableWithTheDemandNoteID = context.CourseExamVafAcfs.Where(s => s.DemandNoteID == demandNoteID).Count();

                            TableCell tcCol12 = new TableCell();
                            tcCol12.Width = Unit.Percentage(8);
                            tcCol12.HorizontalAlign = HorizontalAlign.Center;
                            tcCol12.Text = countOfCandidateIfVafAcfTableWithTheDemandNoteID.ToString();
                            tr.Cells.Add(tcCol12);

                            TableCell tcCol13 = new TableCell();
                            tcCol13.Width = Unit.Percentage(3);
                            tcCol13.HorizontalAlign = HorizontalAlign.Center;
                            tcCol13.Text = dtRow["AccrNo"].ToString();
                            tr.Cells.Add(tcCol13);

                        }
                        //-------vaf acf end-------------------

                        // added by amit start
                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
                        {

                        }
                            TableCell tcnCol = new TableCell();
                        tcnCol.Width = Unit.Percentage(10);
                        tcnCol.HorizontalAlign = HorizontalAlign.Left;
                        tcnCol.Text = (!String.IsNullOrEmpty(dtRow["PaymentGatewayUsed"].ToString()) ? dtRow["PaymentGatewayUsed"].ToString() : "NA");
                        tr.Cells.Add(tcnCol);
                        // added by amit end

                        str.Append(dtRow["Reference_Number"].ToString() + ",");
                        str.Append(dtRow["TransactionID"].ToString() + ",");
                        str.Append(Convert.ToInt64(dtRow["TransactionAmount"]).ToString("F") + ",");
                        str.Append(dtRow["txn_date"].ToString());

                        tbl.Rows.Add(tr);
                        if (dtRow["StatusMessage"].ToString().Trim().ToUpper().Contains("SUCCESS") || (dtRow["Status"].ToString() == "E000"))
                        {
                            totamt += Convert.ToDecimal(dtRow["FeeAmount"]);
                        }
                        str.AppendLine();
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "No Record Found !";
                }
                strHead1 += "<br/><b>Total Amount Of Successful Transactions :</b>" + totamt.ToString("F");
            }
            else if (paymentMode == enmPaymentMode.CSCSPV)
            {                //---vaf acf start------------------------------------------------------------
                if (applicationType == enmApplicationType.CourseVafAcfApplication)
                {
                    strHead += "<br/><b> Date Type :</b> Payment Date";
                    ibtext.Visible = false;

                    mySql.Append(" with base as ( select Distinct T.ID as TransactionID, d.Demand_Note_Type_ID as PayeeType, T.Response_Number as Reference_Number, " +
                    "D.Application_Type_ID as applicationTypeID , D.Fee_Type_ID as FeeTypeID,   T.Date as TransactionDate, " +
                    "T.Demand_Note_ID as DemandID, T.Amount as TransactionAmount, D.Date as DemandNoteDate,   " +
                    "ceva.VAF_Amount as VAF ,    ceva.ACF_Amount as ACF, ceva.Course_Category_ID as CourseCategoryID, ceva.Course_ID as CourseID , " +
                    "ceva.Exam_ID as ExamID ,T.Response_Status as Status, T.Response_Message as StatusMessage,  D.Amount as  FeeAmount, accr.Accreditation_Number as AccrNo  " +
                    "from CSC_Transaction T inner join     Demand_Note D on T.Demand_Note_ID = D.ID inner join    Course_Exam_VAF_ACF ceva on ceva.Demand_Note_ID = D.ID inner join  Intitute_Accreditation_Detail accr on accr.Institute_ID = ceva.Institute_ID where accr.Course_ID = ceva.Course_ID) " +
                    "select TransactionID , DemandNoteDate ,applicationTypeID ,FeeTypeID, StatusMessage, Reference_Number, (select  Code from Course c where c.ID = CourseID ) " +
                    "as CourseName, case PayeeType when '1' then 'Candidate' else 'Institute' end as PayeeType, TransactionDate, DemandID, TransactionAmount,  Reference_Number, VAF, ACF, Status ,  FeeAmount , AccrNo from base where 1=1 ");

                    //if (DateType != "0")
                    //{
                    //    if (DateType == "P")
                    //    {
                    //        mySql.Append(" CAST(TransactionDate as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and  CAST(TransactionDate as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "'");
                    //    }
                    //    if (DateType == "SV")
                    //    {
                    //        mySql.Append(" CAST(SettlementDate as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and  CAST(SettlementDate as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "'");
                    //    }
                    //}
                    if (TransTypeId != 0)
                    {
                        mySql.Append("and PayeeType = '" + TransTypeId.ToString() + "'");
                    }
                    if (CourseId != 0)
                    {
                        mySql.Append(" and CourseID =" + CourseId.ToString());
                    }
                    if (CourseCatId != 0)
                    {
                        mySql.Append(" and CourseCategoryID = " + CourseCatId.ToString());
                    }
                    if (!String.IsNullOrEmpty(ExamIDString))
                    {
                        mySql.Append(" and ExamID in (" + ExamIDString.TrimEnd(',').ToString() + ")");
                    }
                    if (PayStatusId != "0")
                    {
                        if (PayStatusId == "S")
                            mySql.Append(" and (Status = '0' or Status = '100')");
                        if (PayStatusId == "F")
                            mySql.Append(" and (Status = '1' or Status is NULL)");
                    }
                    if (ManualSettled != "0")
                    {
                        if (ManualSettled == "F")
                            mySql.Append(" and IsSettled = 0");
                        if (ManualSettled == "T")
                            mySql.Append(" and IsSettled = 1");
                    }
                    if (TransTypeId != 0)
                    {
                        mySql.Append(" and PayeeType =" + TransTypeId.ToString());
                    }
                    if (transId != 0)
                    {
                        var demandnoteid = (from r in context.OnlineTransaction.AsNoTracking()
                                            where r.ID == transId && System.Data.Entity.DbFunctions.TruncateTime(r.RequestDate) >= System.Data.Entity.DbFunctions.TruncateTime(PayFromDate) && System.Data.Entity.DbFunctions.TruncateTime(r.RequestDate) <= System.Data.Entity.DbFunctions.TruncateTime(PayToDate)
                                            select r.DemandNoteID).FirstOrDefault();
                        if (demandnoteid != null)
                        {
                            mySql.Append(" and DemandID=" + demandnoteid);
                        }
                    }
                    mySql.Append("  order by TransactionDate , TransactionID  ");
                }
                else
                {
                    strHead += "<br/><b> Date Type :</b> Payment Date";
                    ibtext.Visible = false;
                    mySql.Append(" select T.ID as TransactionID, case d.Demand_Note_Type_ID when '1' then 'Candidate' else 'Institute' end as PayeeType,T.Date as TransactionDate, T.Demand_Note_ID, T.Amount as TransactionAmount, " +
                                " T.Response_Number as Reference_Number, C.ID as ApplID, T.Response_Status as Status, R.Code as CourseName , R.ID as CourseID ");
                    if (applicationType == enmApplicationType.CourseExamApplication)
                        mySql.Append(", C.Fee_Amount as  FeeAmount , C.Number_of_th_Modules as Theory , C.Number_of_Pr_Modules as practical , ISNULL(C.Late_Fee_Amount, 0.00) as LateFee , C.Is_Improvement as Improvement ");
                    else if (applicationType == enmApplicationType.CertificateExamApplication)
                        mySql.Append(", C.Total_Fee_Amt as FeeAmount");
                    else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                        mySql.Append(", C.Fee_Amt as FeeAmount ");
                    mySql.Append(", D.Date as DemandNoteDate, D.ID as DemandID, D.Fee_Type_ID, Demand_Note_Type_ID, D.Application_Type_ID, CSC_Transaction_ID, T.Response_Message as StatusMessage " +
                    " from CSC_Transaction T,  Demand_Note d, Course R ");
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

                    if (!String.IsNullOrEmpty(ExamIDString))
                        mySql.Append(" and C.Exam_ID in (" + ExamIDString.TrimEnd(',').ToString() + ")");

                    if (PayStatusId != "0")
                    {
                        if (PayStatusId == "S")
                            mySql.Append(" and (T.Response_Status = '0' or Response_Status = '100')");
                        if (PayStatusId == "F")
                            mySql.Append(" and (T.Response_Status = '1' or T.Response_Status is NULL)");
                    }

                    if (applicationType == enmApplicationType.CertificateExamApplication)
                    {
                        if (regcentreid != 0)
                            mySql.Append(" and C.Regional_Center_ID = " + regcentreid.ToString());
                    }

                    if (transId != 0)
                    {
                        var demandnoteid = (from r in context.CSCTransactions.AsNoTracking()
                                            where r.ID == transId && System.Data.Entity.DbFunctions.TruncateTime(r.Date) >= System.Data.Entity.DbFunctions.TruncateTime(PayFromDate) && System.Data.Entity.DbFunctions.TruncateTime(r.Date) <= System.Data.Entity.DbFunctions.TruncateTime(PayToDate)
                                            select r.DemandNoteID).FirstOrDefault();
                        if (demandnoteid != null)
                        {
                            mySql.Append(" and t.Demand_Note_ID =" + demandnoteid);
                        }
                    }
                    mySql.Append("  order by TransactionDate , TransactionID ");
                }
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

                        TableCell tcCol = new TableCell();
                        tcCol.Width = Unit.Percentage(3);
                        tcCol.HorizontalAlign = HorizontalAlign.Left;
                        tcCol.Text = dtRow["CourseName"].ToString();
                        tr.Cells.Add(tcCol);

                        TableCell tcCol2 = new TableCell();
                        tcCol2.Width = Unit.Percentage(7);
                        tcCol2.HorizontalAlign = HorizontalAlign.Left;
                        tcCol2.Text = dtRow["PayeeType"].ToString();
                        tr.Cells.Add(tcCol2);

                        //------vaf acf start------------------
                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
                        {
                            TableCell tcCol3 = new TableCell();
                            tcCol3.HorizontalAlign = HorizontalAlign.Left;
                            tcCol3.Width = Unit.Percentage(33);
                            var demandNoteID = Convert.ToInt32(dtRow["DemandID"]);
                            var applID = Convert.ToInt32(context.CourseExamVafAcfs.Where(v => v.DemandNoteID == demandNoteID).Select(v => v.ID).FirstOrDefault());
                            tcCol3.Text = GetPayename(context, applID, Convert.ToInt32(dtRow["applicationTypeID"]), Convert.ToInt16(dtRow["FeeTypeID"]));
                            tr.Cells.Add(tcCol3);
                        }
                        //-------vaf acf end-------------------
                        else
                        {
                            TableCell tcCol3 = new TableCell();
                            tcCol3.HorizontalAlign = HorizontalAlign.Left;
                            tcCol3.Width = Unit.Percentage(36);
                            tcCol3.Text = GetPayename(context, Convert.ToInt64(dtRow["ApplID"]), Convert.ToInt32(dtRow["Application_Type_ID"]), Convert.ToInt16(dtRow["Fee_Type_ID"]));
                            tr.Cells.Add(tcCol3);
                        }
                        TableCell tcCol4 = new TableCell();
                        tcCol4.HorizontalAlign = HorizontalAlign.Left;
                        tcCol4.Width = Unit.Percentage(17);
                        tcCol4.Text = dtRow["DemandID"] + " / Dated:-" + Convert.ToDateTime(dtRow["DemandNoteDate"]).ToString("dd-MMM-yyyy");
                        tr.Cells.Add(tcCol4);

                        TableCell tcCol5 = new TableCell();
                        tcCol5.Width = Unit.Percentage(20);
                        tcCol5.HorizontalAlign = HorizontalAlign.Left;
                        tcCol5.Text = dtRow["TransactionID"].ToString() + " - " + (String.IsNullOrEmpty(dtRow["Reference_Number"].ToString()) ? "NA" : dtRow["Reference_Number"].ToString());
                        tr.Cells.Add(tcCol5);

                        TableCell tcCol6 = new TableCell();
                        tcCol6.Width = Unit.Percentage(13);
                        tcCol6.HorizontalAlign = HorizontalAlign.Center;
                        tcCol6.Text = Convert.ToDateTime(dtRow["TransactionDate"]).ToString("dd-MMM-yyyy hh:mm:ss");
                        tr.Cells.Add(tcCol6);

                        TableCell tcCol7 = new TableCell();
                        tcCol7.Width = Unit.Percentage(7);
                        tcCol7.HorizontalAlign = HorizontalAlign.Right;
                        tcCol7.Text = Convert.ToInt64(dtRow["TransactionAmount"]).ToString("F");
                        tr.Cells.Add(tcCol7);

                        TableCell tcCol8 = new TableCell();
                        tcCol8.Width = Unit.Percentage(7);
                        tcCol8.HorizontalAlign = HorizontalAlign.Right;
                        tcCol8.Text = Convert.ToInt64(dtRow["FeeAmount"]).ToString("F");
                        tr.Cells.Add(tcCol8);
                        //------vaf acf start------------------
                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
                        {
                            Int32 VafFee = Convert.ToInt32(dtRow["VAF"]);
                            Int32 AcfFee = Convert.ToInt32(dtRow["ACF"]);

                            TableCell tcCol10 = new TableCell();
                            tcCol10.Width = Unit.Percentage(8);
                            tcCol10.HorizontalAlign = HorizontalAlign.Right;
                            tcCol10.Text = VafFee.ToString();
                            tr.Cells.Add(tcCol10);

                            TableCell tcCol11 = new TableCell();
                            tcCol11.Width = Unit.Percentage(8);
                            tcCol11.HorizontalAlign = HorizontalAlign.Right;
                            tcCol11.Text = AcfFee.ToString();
                            tr.Cells.Add(tcCol11);
                        }
                        //-------vaf acf end-------------------

                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {
                            Int32 CourseID = Convert.ToInt32(dtRow["CourseID"]);
                            Int32 feeTheory = 0;
                            Int32 feePractical = 0;
                            Boolean isImprovement = Convert.ToBoolean(dtRow["Improvement"]);
                            if (isImprovement == true)
                            {
                                feeTheory = CourseManager.GetAppliableFee(context, CourseID, enmFeeType.ImprovementOfPaperFee);
                                feePractical = CourseManager.GetAppliableFee(context, CourseID, enmFeeType.ImprovementOfPaperFee);
                            }
                            else
                            {
                                feeTheory = CourseManager.GetAppliableFee(context, CourseID, enmFeeType.ExaminationFee);
                                feePractical = CourseManager.GetAppliableFee(context, CourseID, enmFeeType.PracticalFee);
                            }


                            TableCell tcCol10 = new TableCell();
                            tcCol10.Width = Unit.Percentage(7);
                            tcCol10.HorizontalAlign = HorizontalAlign.Right;
                            tcCol10.Text = (Convert.ToSingle(feeTheory) * Convert.ToInt32(dtRow["Theory"])).ToString("F");
                            tr.Cells.Add(tcCol10);

                            TableCell tcCol11 = new TableCell();
                            tcCol11.Width = Unit.Percentage(7);
                            tcCol11.HorizontalAlign = HorizontalAlign.Right;
                            tcCol11.Text = (Convert.ToSingle(feePractical) * Convert.ToInt32(dtRow["practical"])).ToString("F");
                            tr.Cells.Add(tcCol11);

                            TableCell tcCol12 = new TableCell();
                            tcCol12.Width = Unit.Percentage(7);
                            tcCol12.HorizontalAlign = HorizontalAlign.Right;
                            tcCol12.Text = Convert.ToInt64(dtRow["LateFee"]).ToString("F");
                            tr.Cells.Add(tcCol12);
                        }
                        TableCell tcCol9 = new TableCell();
                        tcCol9.Width = Unit.Percentage(10);
                        tcCol9.HorizontalAlign = HorizontalAlign.Left;
                        tcCol9.Text = (String.IsNullOrEmpty(dtRow["StatusMessage"].ToString()) ? "Failed" : dtRow["StatusMessage"].ToString());
                        tr.Cells.Add(tcCol9);

                        //------vaf acf start------------------
                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
                        {
                            var demandNoteID = Convert.ToInt32(dtRow["DemandID"]);
                            var countOfCandidateIfVafAcfTableWithTheDemandNoteID = context.CourseExamVafAcfs.Where(s => s.DemandNoteID == demandNoteID).Count();

                            TableCell tcCol12 = new TableCell();
                            tcCol12.Width = Unit.Percentage(8);
                            tcCol12.HorizontalAlign = HorizontalAlign.Center;
                            tcCol12.Text = countOfCandidateIfVafAcfTableWithTheDemandNoteID.ToString();
                            tr.Cells.Add(tcCol12);

                            TableCell tcCol13 = new TableCell();
                            tcCol13.Width = Unit.Percentage(33);
                            tcCol13.HorizontalAlign = HorizontalAlign.Center;
                            tcCol13.Text = dtRow["AccrNo"].ToString();
                            tr.Cells.Add(tcCol13);
                        }
                        //-------vaf acf end-------------------
                        tbl.Rows.Add(tr);
                        if (dtRow["StatusMessage"].ToString().ToUpper().Trim() == "Success".ToUpper().Trim())
                        {
                            totamt += Convert.ToDecimal(dtRow["FeeAmount"]);
                        }
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "No Record Found !";
                }
                strHead1 += "<br/><b>Total Amount Of Successful Transactions :</b>" + totamt.ToString("F");
            }
            else if (paymentMode == enmPaymentMode.DemandDraft)
            { //---vaf acf start------------------------------------------------------------
                if (applicationType == enmApplicationType.CourseVafAcfApplication)
                {
                    strHead += "<br/><b> Date Type :</b> Payment Date";
                    ibtext.Visible = false;

                    mySql.Append("   with base as ( select Distinct T.ID as TransactionID, d.Demand_Note_Type_ID as PayeeType, T.Dd_Number as Reference_Number,  D.Application_Type_ID as applicationTypeID ,  " +
                    " D.Fee_Type_ID as FeeTypeID,   T.Date as TransactionDate, T.Demand_Note_ID as DemandID, T.DD_Amt as TransactionAmount,  D.Date as DemandNoteDate,   ceva.VAF_Amount as VAF ,              " +
                    " ceva.ACF_Amount as ACF, ceva.Course_Category_ID as CourseCategoryID, ceva.Course_ID as CourseID , ceva.Exam_ID as ExamID   ,  D.Amount as  FeeAmount  , accr.Accreditation_Number as AccrNo  from                              " +
                    " DemandDraftTransaction T inner join   Demand_Note D on T.Demand_Note_ID = D.ID inner join Course_Exam_VAF_ACF ceva on ceva.Demand_Note_ID = D.ID inner join  Intitute_Accreditation_Detail accr on accr.Institute_ID = ceva.Institute_ID where accr.Course_ID = ceva.Course_ID) select                                " +
                    " TransactionID , DemandNoteDate ,applicationTypeID ,FeeTypeID, Reference_Number, (select  Code from Course c where c.ID = CourseID ) as CourseName, case PayeeType when '1' then 'Candidate' else 'Institute' end as PayeeType,                            " +
                    " TransactionDate, DemandID, TransactionAmount,  Reference_Number, VAF, ACF ,  FeeAmount , AccrNo from base where 1=1    ");

                    //if (DateType != "0")
                    //{
                    //    if (DateType == "P")
                    //    {
                    //        mySql.Append(" CAST(TransactionDate as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and  CAST(TransactionDate as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "'");
                    //    }
                    //    if (DateType == "SV")
                    //    {
                    //        mySql.Append(" CAST(SettlementDate as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and  CAST(SettlementDate as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "'");
                    //    }
                    //}
                    if (TransTypeId != 0)
                    {
                        mySql.Append(" PayeeType = '" + TransTypeId.ToString() + "'");
                    }
                    if (CourseId != 0)
                    {
                        mySql.Append(" and CourseID =" + CourseId.ToString());
                    }
                    if (CourseCatId != 0)
                    {
                        mySql.Append(" and CourseCategoryID = " + CourseCatId.ToString());
                    }
                    if (!String.IsNullOrEmpty(ExamIDString))
                    {
                        mySql.Append(" and ExamID in (" + ExamIDString.TrimEnd(',').ToString() + ")");
                    }
                    if (PayStatusId != "0")
                    {
                        if (PayStatusId == "S")
                            mySql.Append(" and (Status =" + Convert.ToInt32(enmPaymentStatus.Paid));
                        if (PayStatusId == "F")
                            mySql.Append(" and (Status =" + Convert.ToInt32(enmPaymentStatus.Failed) + ")");
                    }
                    //if (ManualSettled != "0")
                    //{
                    //    if (ManualSettled == "F")
                    //        mySql.Append(" and IsSettled = 0");
                    //    if (ManualSettled == "T")
                    //        mySql.Append(" and IsSettled = 1");
                    //}

                    if (transId != 0)
                    {
                        var demandnoteid = (from r in context.OnlineTransaction.AsNoTracking()
                                            where r.ID == transId && System.Data.Entity.DbFunctions.TruncateTime(r.RequestDate) >= System.Data.Entity.DbFunctions.TruncateTime(PayFromDate) && System.Data.Entity.DbFunctions.TruncateTime(r.RequestDate) <= System.Data.Entity.DbFunctions.TruncateTime(PayToDate)
                                            select r.DemandNoteID).FirstOrDefault();
                        if (demandnoteid != null)
                        {
                            mySql.Append(" and DemandID=" + demandnoteid);
                        }
                    }
                    mySql.Append("  order by TransactionDate , TransactionID  ");
                }
                else
                {
                    strHead += "<br/><b> Date Type :</b> Payment Date";
                    ibtext.Visible = false;
                    mySql.Append(" select T.ID as TransactionID,case d.Demand_Note_Type_ID when '1' then 'Candidate' else 'Institute' end as PayeeType, T.Date as TransactionDate, T.Demand_Note_ID, T.DD_Amt as TransactionAmount, " +
                               " T.Dd_Number as Reference_Number, C.ID as ApplID, R.Code as CourseName , R.ID as CourseID ");
                    if (applicationType == enmApplicationType.CourseExamApplication)
                        mySql.Append(",  C.Fee_Amount as  FeeAmount , C.Number_of_th_Modules as Theory , C.Number_of_Pr_Modules as practical , ISNULL(C.Late_Fee_Amount, 0.00) as LateFee , C.Is_Improvement as Improvement ");
                    else if (applicationType == enmApplicationType.CertificateExamApplication)
                        mySql.Append(", C.Total_Fee_Amt as FeeAmount");
                    else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                        mySql.Append(", C.Fee_Amt as FeeAmount ");
                    //---vaf acf start------------------------------------------------------------
                    else if (applicationType == enmApplicationType.CourseVafAcfApplication)
                        mySql.Append(",  D.Amount as  FeeAmount, C.VAF_Amount as VAF , C.ACF_Amount as ACF ");
                    //----vaf acf end-------------------------------------------------------------
                    mySql.Append(", D.Date as DemandNoteDate, D.ID as DemandID, D.Fee_Type_ID, Demand_Note_Type_ID, D.Status_ID as StatusMessage, D.Application_Type_ID, DD_Transaction_ID" +
                   " from DemandDraftTransaction T,  Demand_Note d, Course R ");
                    if (applicationType == enmApplicationType.CourseExamApplication)
                        mySql.Append(", Course_Exam_Application C ");
                    else if (applicationType == enmApplicationType.CertificateExamApplication)
                        mySql.Append(", Certificate_Exam_Application C ");
                    else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                        mySql.Append(", Course_Registration_Application C ");
                    //---vaf acf start------------------------------------------------------------
                    else if (applicationType == enmApplicationType.CourseVafAcfApplication)
                    {
                        mySql.Append(", Course_Exam_VAF_ACF C ");

                    }

                    if (applicationType == enmApplicationType.CourseVafAcfApplication)
                    {
                        mySql.Append(" Where  D.ID = T.Demand_Note_ID and d.Application_Type_ID = " + Convert.ToInt32(enmApplicationType.CourseExamApplication) + " and " +
                                    " CAST(t.Date as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and CAST(T.Date as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' ");
                    }//---vaf acf end------------------------------------------------------------
                    else
                    {
                        mySql.Append(" Where  D.ID = T.Demand_Note_ID and d.Application_Type_ID = " + AppTypeId.ToString() + " and " +
                                    " CAST(t.Date as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and CAST(T.Date as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' ");
                    }
                    mySql.Append(" and C.Demand_Note_ID = D.ID and C.Course_ID = R.ID");

                    if (TransTypeId != 0)
                        mySql.Append(" and D.Demand_Note_Type_ID = " + TransTypeId.ToString());
                    if (CourseId != 0)
                        mySql.Append(" and C.Course_ID = " + CourseId.ToString());
                    if (CourseCatId != 0)
                        mySql.Append(" and C.Course_Category_ID = " + CourseCatId.ToString());

                    if (!String.IsNullOrEmpty(ExamIDString))
                        mySql.Append(" and C.Exam_ID in (" + ExamIDString.TrimEnd(',').ToString() + ")");

                    if (PayStatusId != "0")
                    {
                        if (PayStatusId == "S")
                            mySql.Append(" and D.Status_ID =" + Convert.ToInt32(enmPaymentStatus.Paid));
                        if (PayStatusId == "F")
                            mySql.Append(" and (D.Status_ID =" + Convert.ToInt32(enmPaymentStatus.Failed) + ")");
                    }

                    if (applicationType == enmApplicationType.CertificateExamApplication)
                    {
                        if (regcentreid != 0)
                            mySql.Append(" and C.Regional_Center_ID = " + regcentreid.ToString());
                    }

                    if (transId != 0)
                    {
                        var demandnoteid = (from r in context.DemandDraftTransactions.AsNoTracking()
                                            where r.ID == transId && System.Data.Entity.DbFunctions.TruncateTime(r.Date) >= System.Data.Entity.DbFunctions.TruncateTime(PayFromDate) && System.Data.Entity.DbFunctions.TruncateTime(r.Date) <= System.Data.Entity.DbFunctions.TruncateTime(PayToDate)
                                            select r.DemandNoteID).FirstOrDefault();
                        if (demandnoteid != null)
                        {
                            mySql.Append(" and t.Demand_Note_ID =" + demandnoteid);
                        }
                    }
                    mySql.Append("  order by TransactionDate , TransactionID ");
                }
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

                        TableCell tcCol = new TableCell();
                        tcCol.Width = Unit.Percentage(3);
                        tcCol.HorizontalAlign = HorizontalAlign.Left;
                        tcCol.Text = dtRow["CourseName"].ToString();
                        tr.Cells.Add(tcCol);

                        TableCell tcCol2 = new TableCell();
                        tcCol2.Width = Unit.Percentage(8);
                        tcCol2.HorizontalAlign = HorizontalAlign.Left;
                        tcCol2.Text = dtRow["PayeeType"].ToString();
                        tr.Cells.Add(tcCol2);
                        //------vaf acf start------------------
                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
                        {
                            TableCell tcCol3 = new TableCell();
                            tcCol3.HorizontalAlign = HorizontalAlign.Left;
                            tcCol3.Width = Unit.Percentage(33);
                            var demandNoteID = Convert.ToInt32(dtRow["DemandID"]);
                            var applID = Convert.ToInt32(context.CourseExamVafAcfs.Where(v => v.DemandNoteID == demandNoteID).Select(v => v.ID).FirstOrDefault());
                            tcCol3.Text = GetPayename(context, applID, Convert.ToInt32(dtRow["applicationTypeID"]), Convert.ToInt16(dtRow["FeeTypeID"]));
                            tr.Cells.Add(tcCol3);
                        }
                        //-------vaf acf end-------------------
                        else
                        {

                            TableCell tcCol3 = new TableCell();
                            tcCol3.HorizontalAlign = HorizontalAlign.Left;
                            tcCol3.Width = Unit.Percentage(36);
                            tcCol3.Text = GetPayename(context, Convert.ToInt64(dtRow["ApplID"]), Convert.ToInt32(dtRow["Application_Type_ID"]), Convert.ToInt16(dtRow["Fee_Type_ID"]));
                            tr.Cells.Add(tcCol3);
                        }
                        TableCell tcCol4 = new TableCell();
                        tcCol4.HorizontalAlign = HorizontalAlign.Left;
                        tcCol4.Width = Unit.Percentage(17);
                        tcCol4.Text = dtRow["DemandID"] + " / Dated:-" + Convert.ToDateTime(dtRow["DemandNoteDate"]).ToString("dd-MMM-yyyy");
                        tr.Cells.Add(tcCol4);

                        TableCell tcCol5 = new TableCell();
                        tcCol5.Width = Unit.Percentage(10);
                        tcCol5.HorizontalAlign = HorizontalAlign.Left;
                        tcCol5.Text = dtRow["TransactionID"].ToString() + " - " + (String.IsNullOrEmpty(dtRow["Reference_Number"].ToString()) ? "NA" : dtRow["Reference_Number"].ToString());
                        tr.Cells.Add(tcCol5);

                        TableCell tcCol6 = new TableCell();
                        tcCol6.Width = Unit.Percentage(15);
                        tcCol6.HorizontalAlign = HorizontalAlign.Center;
                        tcCol6.Text = Convert.ToDateTime(dtRow["TransactionDate"]).ToString("dd-MMM-yyyy hh:mm:ss");
                        tr.Cells.Add(tcCol6);

                        TableCell tcCol7 = new TableCell();
                        tcCol7.Width = Unit.Percentage(7);
                        tcCol7.HorizontalAlign = HorizontalAlign.Right;
                        tcCol7.Text = Convert.ToInt64(dtRow["TransactionAmount"]).ToString("F");
                        tr.Cells.Add(tcCol7);


                        TableCell tcCol8 = new TableCell();
                        tcCol8.Width = Unit.Percentage(7);
                        tcCol8.HorizontalAlign = HorizontalAlign.Right;
                        tcCol8.Text = Convert.ToInt64(dtRow["FeeAmount"]).ToString("F");
                        tr.Cells.Add(tcCol8);
                        //------vaf acf start------------------
                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
                        {
                            Int32 VafFee = Convert.ToInt32(dtRow["VAF"]);
                            Int32 AcfFee = Convert.ToInt32(dtRow["ACF"]);

                            TableCell tcCol10 = new TableCell();
                            tcCol10.Width = Unit.Percentage(8);
                            tcCol10.HorizontalAlign = HorizontalAlign.Right;
                            tcCol10.Text = VafFee.ToString();
                            tr.Cells.Add(tcCol10);

                            TableCell tcCol11 = new TableCell();
                            tcCol11.Width = Unit.Percentage(8);
                            tcCol11.HorizontalAlign = HorizontalAlign.Right;
                            tcCol11.Text = AcfFee.ToString();
                            tr.Cells.Add(tcCol11);
                        }
                        //-------vaf acf end-------------------

                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {
                            Int32 CourseID = Convert.ToInt32(dtRow["CourseID"]);
                            Int32 feeTheory = 0;
                            Int32 feePractical = 0;
                            Boolean isImprovement = Convert.ToBoolean(dtRow["Improvement"]);
                            if (isImprovement == true)
                            {
                                feeTheory = CourseManager.GetAppliableFee(context, CourseID, enmFeeType.ImprovementOfPaperFee);
                                feePractical = CourseManager.GetAppliableFee(context, CourseID, enmFeeType.ImprovementOfPaperFee);
                            }
                            else
                            {
                                feeTheory = CourseManager.GetAppliableFee(context, CourseID, enmFeeType.ExaminationFee);
                                feePractical = CourseManager.GetAppliableFee(context, CourseID, enmFeeType.PracticalFee);
                            }


                            TableCell tcCol10 = new TableCell();
                            tcCol10.Width = Unit.Percentage(7);
                            tcCol10.HorizontalAlign = HorizontalAlign.Right;
                            tcCol10.Text = (Convert.ToSingle(feeTheory) * Convert.ToInt32(dtRow["Theory"])).ToString("F");
                            tr.Cells.Add(tcCol10);

                            TableCell tcCol11 = new TableCell();
                            tcCol11.Width = Unit.Percentage(7);
                            tcCol11.HorizontalAlign = HorizontalAlign.Right;
                            tcCol11.Text = (Convert.ToSingle(feePractical) * Convert.ToInt32(dtRow["practical"])).ToString("F");
                            tr.Cells.Add(tcCol11);

                            TableCell tcCol12 = new TableCell();
                            tcCol12.Width = Unit.Percentage(7);
                            tcCol12.HorizontalAlign = HorizontalAlign.Right;
                            tcCol12.Text = Convert.ToInt64(dtRow["LateFee"]).ToString("F");
                            tr.Cells.Add(tcCol12);
                        }

                        TableCell tcCol9 = new TableCell();
                        tcCol9.Width = Unit.Percentage(10);
                        tcCol9.HorizontalAlign = HorizontalAlign.Left;
                        tcCol9.Text = (String.IsNullOrEmpty(dtRow["StatusMessage"].ToString()) ? "Failed" : EConnect.Utils.Common.EnumUtility.GetDescription((enmPaymentStatus)(Convert.ToInt32(dtRow["StatusMessage"]))).ToString());
                        tr.Cells.Add(tcCol9);

                        //------vaf acf start------------------
                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
                        {
                            var demandNoteID = Convert.ToInt32(dtRow["DemandID"]);
                            var countOfCandidateIfVafAcfTableWithTheDemandNoteID = context.CourseExamVafAcfs.Where(s => s.DemandNoteID == demandNoteID).Count();

                            TableCell tcCol12 = new TableCell();
                            tcCol12.Width = Unit.Percentage(8);
                            tcCol12.HorizontalAlign = HorizontalAlign.Center;
                            tcCol12.Text = countOfCandidateIfVafAcfTableWithTheDemandNoteID.ToString();
                            tr.Cells.Add(tcCol12);

                            TableCell tcCol13 = new TableCell();
                            tcCol13.Width = Unit.Percentage(33);
                            tcCol13.HorizontalAlign = HorizontalAlign.Center;
                            tcCol13.Text = dtRow["AccrNo"].ToString();
                            tr.Cells.Add(tcCol13);

                        }
                        //-------vaf acf end-------------------
                        tbl.Rows.Add(tr);
                        totamt += Convert.ToDecimal(dtRow["FeeAmount"]);
                    }
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
                //---vaf acf start------------------------------------------------------------
                if (applicationType == enmApplicationType.CourseVafAcfApplication)
                {
                    strHead += "<br/><b> Date Type :</b> Payment Date";
                    ibtext.Visible = false;

                    mySql.Append("   with base as ( select Distinct T.ID as TransactionID, d.Demand_Note_Type_ID as PayeeType, T.Transaction_Number as Reference_Number,  D.Application_Type_ID as applicationTypeID ,  " +
                    " D.Fee_Type_ID as FeeTypeID,   T.Date as TransactionDate, T.Demand_Note_ID as DemandID, T.Transaction_Amt as TransactionAmount,  D.Date as DemandNoteDate, D.Status_ID as Status,  ceva.VAF_Amount as VAF ,   " +
                    " ceva.ACF_Amount as ACF, ceva.Course_Category_ID as CourseCategoryID, ceva.Course_ID as CourseID , ceva.Exam_ID as ExamID   ,  D.Amount as  FeeAmount  , accr.Accreditation_Number as AccrNo  from        " +
                    " NEFT_Transaction T inner join   Demand_Note D on T.Demand_Note_ID = D.ID inner join Course_Exam_VAF_ACF ceva on ceva.Demand_Note_ID = D.ID inner join  Intitute_Accreditation_Detail accr on accr.Institute_ID = ceva.Institute_ID where accr.Course_ID = ceva.Course_ID) select  Status,               " +
                    " TransactionID , DemandNoteDate ,applicationTypeID ,FeeTypeID, Reference_Number, (select  Code from Course c where c.ID = CourseID ) as CourseName, case PayeeType when '1' then 'Candidate' else 'Institute' end as PayeeType,      " +
                    " TransactionDate, DemandID, TransactionAmount,  Reference_Number, VAF, ACF ,  FeeAmount , AccrNo from base where ");

                    if (DateType != "0")
                    {
                        if (DateType == "P")
                        {
                            mySql.Append(" CAST(TransactionDate as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and  CAST(TransactionDate as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "'");
                        }
                        if (DateType == "SV")
                        {
                            mySql.Append(" CAST(SettlementDate as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and  CAST(SettlementDate as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "'");
                        }
                    }
                    if (TransTypeId != 0)
                    {
                        mySql.Append("and PayeeType = '" + TransTypeId.ToString() + "'");
                    }
                    if (CourseId != 0)
                    {
                        mySql.Append(" and CourseID =" + CourseId.ToString());
                    }
                    if (CourseCatId != 0)
                    {
                        mySql.Append(" and CourseCategoryID = " + CourseCatId.ToString());
                    }
                    if (!String.IsNullOrEmpty(ExamIDString))
                    {
                        mySql.Append(" and ExamID in (" + ExamIDString.TrimEnd(',').ToString() + ")");
                    }
                    if (PayStatusId != "0")
                    {
                        if (PayStatusId == "S")
                            mySql.Append(" and Status =" + Convert.ToInt32(enmPaymentStatus.Paid));
                        if (PayStatusId == "F")
                            mySql.Append(" and Status = " + Convert.ToInt32(enmPaymentStatus.Failed));
                    }
                    //if (ManualSettled != "0")
                    //{
                    //    if (ManualSettled == "F")
                    //        mySql.Append(" and IsSettled = 0");
                    //    if (ManualSettled == "T")
                    //        mySql.Append(" and IsSettled = 1");
                    //}
                    if (TransTypeId != 0)
                    {
                        mySql.Append(" and PayeeType =" + TransTypeId.ToString());
                    }
                    if (transId != 0)
                    {
                        var demandnoteid = (from r in context.OnlineTransaction.AsNoTracking()
                                            where r.ID == transId && System.Data.Entity.DbFunctions.TruncateTime(r.RequestDate) >= System.Data.Entity.DbFunctions.TruncateTime(PayFromDate) && System.Data.Entity.DbFunctions.TruncateTime(r.RequestDate) <= System.Data.Entity.DbFunctions.TruncateTime(PayToDate)
                                            select r.DemandNoteID).FirstOrDefault();
                        if (demandnoteid != null)
                        {
                            mySql.Append(" and DemandID=" + demandnoteid);
                        }
                    }
                    mySql.Append("  order by TransactionDate , TransactionID  ");
                }
                else
                {
                    if (DateType == "P")
                    {
                        strHead += "<br/><b> Date Type :</b> Payment Date";
                    }
                    if (DateType == "SV")
                    {
                        strHead += "<br/><b> Date Type :</b> Verified Date";
                    }

                    ibtext.Visible = false;
                    mySql.Append(" select T.ID as TransactionID,case d.Demand_Note_Type_ID when '1' then 'Candidate' else 'Institute' end as PayeeType, T.Transaction_Date as TransactionDate, T.Demand_Note_ID, T.Transaction_Amt as TransactionAmount, " +
                               " T.Transaction_Number as Reference_Number, C.ID as ApplID, R.Code as CourseName , R.ID as CourseID, T.Date as VerifiedDate ");
                    if (applicationType == enmApplicationType.CourseExamApplication)
                        mySql.Append(",  C.Fee_Amount as  FeeAmount , C.Number_of_th_Modules as Theory , C.Number_of_Pr_Modules as practical , ISNULL(C.Late_Fee_Amount, 0.00) as LateFee , C.Is_Improvement as Improvement ");
                    else if (applicationType == enmApplicationType.CertificateExamApplication)
                        mySql.Append(", C.Total_Fee_Amt as FeeAmount");
                    else if (applicationType == enmApplicationType.CourseRegistrationApplication)
                        mySql.Append(", C.Fee_Amt as FeeAmount ");

                    mySql.Append(", D.Date as DemandNoteDate, D.ID as DemandID, D.Fee_Type_ID, Demand_Note_Type_ID, D.Status_ID as StatusMessage, D.Application_Type_ID, NEFT_Transaction_ID" +
                   " from NEFT_Transaction T,  Demand_Note d, Course R ");
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


                    if (!String.IsNullOrEmpty(ExamIDString))
                        mySql.Append(" and C.Exam_ID in (" + ExamIDString.TrimEnd(',').ToString() + ")");

                    if (PayStatusId != "0")
                    {
                        if (PayStatusId == "S")
                            mySql.Append(" and D.Status_ID =" + Convert.ToInt32(enmPaymentStatus.Paid));
                        if (PayStatusId == "F")
                            mySql.Append(" and (D.Status_ID =" + Convert.ToInt32(enmPaymentStatus.Failed) + ")");
                    }

                    if (applicationType == enmApplicationType.CertificateExamApplication)
                    {
                        if (regcentreid != 0)
                            mySql.Append(" and C.Regional_Center_ID = " + regcentreid.ToString());
                    }

                    if (transId != 0)
                    {
                        var demandnoteid = (from r in context.NEFTTransactions.AsNoTracking()
                                            where r.ID == transId && System.Data.Entity.DbFunctions.TruncateTime(r.Date) >= System.Data.Entity.DbFunctions.TruncateTime(PayFromDate) && System.Data.Entity.DbFunctions.TruncateTime(r.Date) <= System.Data.Entity.DbFunctions.TruncateTime(PayToDate)
                                            select r.DemandNoteID).FirstOrDefault();
                        if (demandnoteid != null)
                        {
                            mySql.Append(" and t.Demand_Note_ID =" + demandnoteid);
                        }
                    }
                    mySql.Append("  order by TransactionDate , TransactionID ");
                }
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

                        TableCell tcCol = new TableCell();
                        tcCol.Width = Unit.Percentage(3);
                        tcCol.HorizontalAlign = HorizontalAlign.Left;
                        tcCol.Text = dtRow["CourseName"].ToString();
                        tr.Cells.Add(tcCol);

                        TableCell tcCol2 = new TableCell();
                        tcCol2.Width = Unit.Percentage(8);
                        tcCol2.HorizontalAlign = HorizontalAlign.Left;
                        //tcCol2.Text = GetPayeeType(context, Convert.ToInt64(dtRow["ApplID"]), Convert.ToInt32(dtRow["Application_Type_ID"]));
                        tcCol2.Text = dtRow["PayeeType"].ToString();
                        tr.Cells.Add(tcCol2);

                        TableCell tcCol3 = new TableCell();
                        tcCol3.HorizontalAlign = HorizontalAlign.Left;
                        tcCol3.Width = Unit.Percentage(34);
                        tcCol3.Text = GetPayename(context, Convert.ToInt64(dtRow["ApplID"]), Convert.ToInt32(dtRow["Application_Type_ID"]), Convert.ToInt16(dtRow["Fee_Type_ID"]));
                        tr.Cells.Add(tcCol3);

                        TableCell tcCol4 = new TableCell();
                        tcCol4.HorizontalAlign = HorizontalAlign.Left;
                        tcCol4.Width = Unit.Percentage(19);
                        tcCol4.Text = dtRow["DemandID"] + " / Dated:-" + Convert.ToDateTime(dtRow["DemandNoteDate"]).ToString("dd-MMM-yyyy");
                        tr.Cells.Add(tcCol4);

                        TableCell tcCol5 = new TableCell();
                        tcCol5.Width = Unit.Percentage(10);
                        tcCol5.HorizontalAlign = HorizontalAlign.Left;
                        tcCol5.Text = dtRow["TransactionID"].ToString() + " - " + (String.IsNullOrEmpty(dtRow["Reference_Number"].ToString()) ? "NA" : dtRow["Reference_Number"].ToString());
                        tr.Cells.Add(tcCol5);

                        TableCell tcCol6 = new TableCell();
                        tcCol6.Width = Unit.Percentage(15);
                        tcCol6.HorizontalAlign = HorizontalAlign.Center;
                        tcCol6.Text = Convert.ToDateTime(dtRow["TransactionDate"]).ToString("dd-MMM-yyyy");
                        tr.Cells.Add(tcCol6);

                        TableCell tcCol16 = new TableCell();
                        tcCol16.Width = Unit.Percentage(15);
                        tcCol16.HorizontalAlign = HorizontalAlign.Center;
                        tcCol16.Text = Convert.ToDateTime(dtRow["VerifiedDate"]).ToString("dd-MMM-yyyy");
                        tr.Cells.Add(tcCol16);

                        TableCell tcCol7 = new TableCell();
                        tcCol7.Width = Unit.Percentage(7);
                        tcCol7.HorizontalAlign = HorizontalAlign.Right;
                        tcCol7.Text = Convert.ToInt64(dtRow["TransactionAmount"]).ToString("F");
                        tr.Cells.Add(tcCol7);


                        TableCell tcCol8 = new TableCell();
                        tcCol8.Width = Unit.Percentage(7);
                        tcCol8.HorizontalAlign = HorizontalAlign.Right;
                        tcCol8.Text = Convert.ToInt64(dtRow["FeeAmount"]).ToString("F");
                        tr.Cells.Add(tcCol8);
                        //------vaf acf start------------------
                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
                        {
                            Int32 VafFee = Convert.ToInt32(dtRow["VAF"]);
                            Int32 AcfFee = Convert.ToInt32(dtRow["ACF"]);

                            TableCell tcCol10 = new TableCell();
                            tcCol10.Width = Unit.Percentage(8);
                            tcCol10.HorizontalAlign = HorizontalAlign.Right;
                            tcCol10.Text = VafFee.ToString();
                            tr.Cells.Add(tcCol10);

                            TableCell tcCol11 = new TableCell();
                            tcCol11.Width = Unit.Percentage(8);
                            tcCol11.HorizontalAlign = HorizontalAlign.Right;
                            tcCol11.Text = AcfFee.ToString();
                            tr.Cells.Add(tcCol11);
                        }
                        //-------vaf acf end-------------------

                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {
                            Int32 CourseID = Convert.ToInt32(dtRow["CourseID"]);
                            Int32 feeTheory = 0;
                            Int32 feePractical = 0;
                            Boolean isImprovement = Convert.ToBoolean(dtRow["Improvement"]);
                            if (isImprovement == true)
                            {
                                feeTheory = CourseManager.GetAppliableFee(context, CourseID, enmFeeType.ImprovementOfPaperFee);
                                feePractical = CourseManager.GetAppliableFee(context, CourseID, enmFeeType.ImprovementOfPaperFee);
                            }
                            else
                            {
                                feeTheory = CourseManager.GetAppliableFee(context, CourseID, enmFeeType.ExaminationFee);
                                feePractical = CourseManager.GetAppliableFee(context, CourseID, enmFeeType.PracticalFee);
                            }

                            TableCell tcCol10 = new TableCell();
                            tcCol10.Width = Unit.Percentage(7);
                            tcCol10.HorizontalAlign = HorizontalAlign.Right;
                            tcCol10.Text = (Convert.ToSingle(feeTheory) * Convert.ToInt32(dtRow["Theory"])).ToString("F");
                            tr.Cells.Add(tcCol10);

                            TableCell tcCol11 = new TableCell();
                            tcCol11.Width = Unit.Percentage(7);
                            tcCol11.HorizontalAlign = HorizontalAlign.Right;
                            tcCol11.Text = (Convert.ToSingle(feePractical) * Convert.ToInt32(dtRow["practical"])).ToString("F");
                            tr.Cells.Add(tcCol11);

                            TableCell tcCol12 = new TableCell();
                            tcCol12.Width = Unit.Percentage(7);
                            tcCol12.HorizontalAlign = HorizontalAlign.Right;
                            tcCol12.Text = Convert.ToInt64(dtRow["LateFee"]).ToString("F");
                            tr.Cells.Add(tcCol12);
                        }
                        TableCell tcCol9 = new TableCell();
                        tcCol9.Width = Unit.Percentage(10);
                        tcCol9.HorizontalAlign = HorizontalAlign.Left;
                        tcCol9.Text = (String.IsNullOrEmpty(dtRow["StatusMessage"].ToString()) ? "Failed" : EConnect.Utils.Common.EnumUtility.GetDescription((enmPaymentStatus)(Convert.ToInt32(dtRow["StatusMessage"]))).ToString());
                        tr.Cells.Add(tcCol9);
                        //------vaf acf start------------------ added on 08052025 when adding accr no
                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
                        {
                            var demandNoteID = Convert.ToInt32(dtRow["DemandID"]);
                            var countOfCandidateIfVafAcfTableWithTheDemandNoteID = context.CourseExamVafAcfs.Where(s => s.DemandNoteID == demandNoteID).Count();

                            TableCell tcCol12 = new TableCell();
                            tcCol12.Width = Unit.Percentage(8);
                            tcCol12.HorizontalAlign = HorizontalAlign.Center;
                            tcCol12.Text = countOfCandidateIfVafAcfTableWithTheDemandNoteID.ToString();
                            tr.Cells.Add(tcCol12);

                            TableCell tcCol13 = new TableCell();
                            tcCol13.Width = Unit.Percentage(33);
                            tcCol13.HorizontalAlign = HorizontalAlign.Center;
                            tcCol13.Text = dtRow["AccrNo"].ToString();
                            tr.Cells.Add(tcCol13);

                        }
                        //-------vaf acf end-------------------

                        tbl.Rows.Add(tr);
                        totamt += Convert.ToDecimal(dtRow["FeeAmount"]);
                    }
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
            throw ex;
        }
        finally
        {
            context.Dispose();
        }
    }
    //protected String GetPayeeType(EConnectContext context, Int64 Appid, Int32 ApplicanttypeID)
    //{
    //    try
    //    {
    //        string paytype = "";
    //        if (ApplicanttypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
    //        {
    //            var payeetype = (from c in context.CourseExamApplications
    //                             where c.ID == Appid
    //                             select c).FirstOrDefault();
    //            if (payeetype != null)
    //            {
    //                if (payeetype.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate))
    //                {
    //                    paytype = "Candidate";
    //                }
    //                else if (payeetype.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Institute))
    //                {
    //                    paytype = "Institute";
    //                }

    //            }
    //        }
    //        else if (ApplicanttypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
    //        {
    //            var payeetype = (from c in context.CertificateExamApplications
    //                             where c.ID == Appid
    //                             select c).FirstOrDefault();
    //            if (payeetype != null)
    //                paytype = payeetype.ApplicantType.Name.ToString();
    //        }
    //        else if (ApplicanttypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
    //        {
    //            var payeetype = (from c in context.CourseRegistrationApplications
    //                             where c.ID == Appid
    //                             select c).FirstOrDefault();
    //            if (payeetype != null)
    //                paytype = payeetype.ApplicantType.Name.ToString();
    //        }
    //        if (paytype != null)
    //            return paytype.ToString();
    //        else
    //            return "";

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    protected String GetPayename(EConnectContext context, Int64 Appid, Int32 ApplicanttypeID, Int16 FeeTypeID)
    {
        try
        {
            string payname = "";
            if (ApplicanttypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                if (FeeTypeID == Convert.ToInt32(enmFeeType.ACF_VAF_fee))
                {
                    var payeetype = (from c in context.CourseExamVafAcfs.AsNoTracking()
                                     where c.ID == Appid
                                     select new
                                     {
                                         RegistrationNumber = c.RegistrationNumber,
                                         CandidateName = c.Candidate.Name,
                                         InstituteName = c.Institute.Name
                                     }).FirstOrDefault();
                    if (payeetype != null)
                    {
                        payname = GetInitCap(payeetype.InstituteName);
                    }
                }
                else
                {
                    var payeetype = (from c in context.CourseExamApplications.AsNoTracking()
                                     where c.ID == Appid
                                     select new
                                     {
                                         PaymentSourceID = c.PaymentSourceID,
                                         RegistrationNumber = c.RegistrationNumber,
                                         CandidateName = c.Candidate.Name,
                                         InstituteName = c.Institute.Name
                                     }).FirstOrDefault();
                    if (payeetype != null)
                    {
                        if (payeetype.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate))
                        {
                            payname = GetInitCap(payeetype.CandidateName) + "(Reg No:-" + payeetype.RegistrationNumber + ")";
                        }
                        else if (payeetype.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Institute))
                        {
                            payname = GetInitCap(payeetype.InstituteName);
                        }
                    }
                }
            }
            else if (ApplicanttypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
            {
                var payeetype = (from c in context.CertificateExamApplications.AsNoTracking()
                                 where c.ID == Appid
                                 select new
                                 {
                                     ApplicantTypeID = c.ApplicantTypeID,
                                     Name = c.Name,
                                     Number = c.Number,
                                     InstituteName = c.Institute.Name
                                 }).FirstOrDefault();
                if (payeetype != null)
                {
                    if (payeetype.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                    {
                        payname = GetInitCap(payeetype.Name) + "(App No:-" + payeetype.Number + ")";
                    }
                    else if (payeetype.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute))
                    {
                        payname = GetInitCap(payeetype.InstituteName);
                    }
                }
            }
            else if (ApplicanttypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
            {
                var payeetype = (from c in context.CourseRegistrationApplications.AsNoTracking()
                                 where c.ID == Appid
                                 select new
                                 {
                                     ApplicantTypeID = c.ApplicantTypeID,
                                     Name = c.Name,
                                     Number = c.Number,
                                     InstituteName = c.Institute.Name
                                 }).FirstOrDefault();
                if (payeetype != null)
                {
                    if (payeetype.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                    {
                        payname = GetInitCap(payeetype.Name) + "(App No.:-" + payeetype.Number + ")";
                    }
                    else if (payeetype.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute))
                    {
                        payname = GetInitCap(payeetype.InstituteName);
                    }
                }
            }
            else if (ApplicanttypeID == Convert.ToInt32(enmApplicationType.CourseProjectApplication))
            {
                var payeetype = (from c in context.CourseProjectApplications.AsNoTracking()
                                 where c.ID == Appid
                                 select new
                                 {
                                     PaymentSourceID = c.PaymentSourceID,
                                     RegistrationNumber = c.RegistrationNumber,
                                     CandidateName = c.Candidate.Name,
                                     InstituteName = c.Institute.Name
                                 }).FirstOrDefault();
                if (payeetype != null)
                {
                    if (payeetype.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate))
                    {
                        payname = GetInitCap(payeetype.CandidateName) + "(Reg No:-" + payeetype.RegistrationNumber + ")";
                    }
                    else if (payeetype.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Institute))
                    {
                        payname = GetInitCap(payeetype.InstituteName);
                    }
                }
            }

            if (payname != null)
                return payname.ToString();
            else
                return "";
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    //    protected String GetPayename(EConnectContext context, Int64 Appid, Int32 ApplicanttypeID)
    //    {
    //        try
    //        {
    //            string payname = "";
    //            if (ApplicanttypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
    //            { if (FeeTypeID == Convert.ToInt32(enmFeeType.ACF_VAF_fee))
    //                {
    //                    var payeetype = (from c in context.CourseExamVafAcfs.AsNoTracking()
    //                                     where c.ID == Appid
    //                                     select new
    //                                     {
    //                                         RegistrationNumber = c.RegistrationNumber,
    //                                         CandidateName = c.Candidate.Name,
    //                                         InstituteName = c.Institute.Name
    //                                     }).FirstOrDefault();
    //                    if (payeetype != null)
    //                    {
    //                        payname = GetInitCap(payeetype.InstituteName);
    //                    }
    //                }
    //                else
    //                {
    //                var payeetype = (from c in context.CourseExamApplications.AsNoTracking()
    //                                 where c.ID == Appid
    //                                 select new
    //                                 {
    //                                     PaymentSourceID = c.PaymentSourceID,
    //                                     RegistrationNumber = c.RegistrationNumber,
    //                                     CandidateName = c.Candidate.Name,
    //                                     InstituteName = c.Institute.Name
    //                                 }).FirstOrDefault();
    //                if (payeetype != null)
    //                {
    //                    if (payeetype.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Candidate))
    //                    {
    //                        payname = GetInitCap(payeetype.CandidateName) + "(Reg No:-" + payeetype.RegistrationNumber + ")";
    //                    }
    //                    else if (payeetype.PaymentSourceID == Convert.ToInt32(enmPaymentSource.Institute))
    //                    {
    //                        payname = GetInitCap(payeetype.InstituteName);
    //}
    //                    }
    //                }
    //            }
    //            else if (ApplicanttypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
    //            {
    //                var payeetype = (from c in context.CertificateExamApplications.AsNoTracking()
    //                                 where c.ID == Appid
    //                                 select new
    //                                 {
    //                                     ApplicantTypeID = c.ApplicantTypeID,
    //                                     Name = c.Name,
    //                                     Number = c.Number,
    //                                     InstituteName = c.Institute.Name
    //                                 }).FirstOrDefault();
    //                if (payeetype != null)
    //                {
    //                    if (payeetype.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
    //                    {
    //                        payname = GetInitCap(payeetype.Name) + "(App No:-" + payeetype.Number + ")";
    //                    }
    //                    else if (payeetype.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute))
    //                    {
    //                        payname = GetInitCap(payeetype.InstituteName);
    //                    }
    //                }
    //            }
    //            else if (ApplicanttypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
    //            {
    //                var payeetype = (from c in context.CourseRegistrationApplications.AsNoTracking()
    //                                 where c.ID == Appid
    //                                 select new
    //                                 {
    //                                     ApplicantTypeID = c.ApplicantTypeID,
    //                                     Name = c.Name,
    //                                     Number = c.Number,
    //                                     InstituteName = c.Institute.Name
    //                                 }).FirstOrDefault();
    //                if (payeetype != null)
    //                {
    //                    if (payeetype.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
    //                    {
    //                        payname = GetInitCap(payeetype.Name) + "(App No.:-" + payeetype.Number + ")";
    //                    }
    //                    else if (payeetype.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Institute))
    //                    {
    //                        payname = GetInitCap(payeetype.InstituteName);
    //                    }
    //                }
    //            }
    //            if (payname != null)
    //                return payname.ToString();
    //            else
    //                return "";
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }

    //    }
    protected void ibtext_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            String fileName = "";
            divReportData.Visible = true;
            newShowData();
            fileName = "NIELIT_Settlement_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
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
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
            Response.Charset = "";
            Response.ContentType = "text/plain";
            Response.Charset = "UTF-8";
            Response.Write(str.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}