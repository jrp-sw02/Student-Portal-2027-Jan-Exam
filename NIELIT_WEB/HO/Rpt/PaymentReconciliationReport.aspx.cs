using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Data;

public partial class ReportPgae : BasePage
{
    Table tbl = new Table();
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View,"HO/PaymentReconciliationFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                tbl.CssClass = "sample3";
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(100);

                ShowData();
                divReportData.Controls.Add(tbl);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowTableHeader(int appTypeID)
    {
        try
        {
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(3);
            tcCol1.HorizontalAlign = HorizontalAlign.Right;
            tcCol1.Text = "#";
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            //tcCol6.Width = Unit.Percentage(10);
            tcCol6.Text = "Transaction No.";
            tcCol6.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tcCol6);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            //tcCol7.Width = Unit.Percentage(12);
            tcCol7.Text = "Transaction Date.";
            tcCol7.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tcCol7);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            //tcCol2.Width = Unit.Percentage(10);
            tcCol2.Text = "DemandNoteNo./Date";
            tcCol2.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tcCol2);

            //TableHeaderCell tcCol8 = new TableHeaderCell();
            ////tcCol8.Width = Unit.Percentage(12);
            //tcCol8.Text = "DemandNoteAmount";
            //tcCol8.HorizontalAlign = HorizontalAlign.Right;
            //th.Cells.Add(tcCol8);

            if (appTypeID == 0)
            {
                TableHeaderCell tcCol5 = new TableHeaderCell();
                //tcCol5.Width = Unit.Percentage(20);
                tcCol5.Text = "Application Type";
                th.Cells.Add(tcCol5);
            }

            TableHeaderCell tcCol4 = new TableHeaderCell();
            //tcCol4.Width = Unit.Percentage(10);
            tcCol4.Text = "Application No.";
            tcCol4.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol9 = new TableHeaderCell();
            //tcCol9.Width = Unit.Percentage(20);
            tcCol9.Text = "TransactionAmount";
            tcCol9.HorizontalAlign = HorizontalAlign.Right;
            th.Cells.Add(tcCol9);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowData()
     {
        try
        {

            int PayModeId = Convert.ToInt32(Request.QueryString["PayModeId"]);

            int AppTypeId = Convert.ToInt32(Request.QueryString["AppTypeId"]);
            string TransactionStatus = Convert.ToString(Request.QueryString["TransactionStatus"]);
            DateTime PayFromDate = Convert.ToDateTime(Request.QueryString["PayFromDate"]);
            DateTime PayToDate = Convert.ToDateTime(Request.QueryString["PayToDate"]);
            string strHead = "";
            if (PayModeId != 0)
                strHead += "<b>Payment Mode :  </b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.enmPaymentMode)(PayModeId)).ToString();
            strHead += "</br> <b>Payment Date:  From :  </b> " + PayFromDate.ToString("dd-MMM-yyyy");
            strHead += "    <b>To :  </b> " + PayToDate.ToString("dd-MMM-yyyy");
            if (AppTypeId != 0)
            {
                strHead += "</br> <b>Application Type :</b> " + EConnect.Utils.Common.EnumUtility.GetDescription((EConnect.NIELIT.enmApplicationType)(AppTypeId)).ToString();
            }
            else
                strHead += "</br> <b>Application Type :</b> ALL";
            switch (TransactionStatus)
            {
                case "SNU": //Success and not Updated
                    strHead += "</br> <b>Transaction Status :</b>Transaction success but not updated";
                    break;
                case "SUV"://Success Updated and Verified
                    strHead += "</br> <b>Transaction Status :</b>Transaction success, updated and verified ";
                    break;
                case "SUNV"://Success and not Updated and Not Verified
                    strHead += "</br> <b>Transaction Status :</b>Transaction success and updated and not verified";
                    break;
                case "F"://Failed
                    strHead += "</br> <b>Transaction Status :</b>Transaction failed";
                    break;
            }

            LblRptSubHeader.Text = strHead;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CSCSPV = Convert.ToInt32(enmPaymentMode.CSCSPV);
                Int32 Online = Convert.ToInt32(enmPaymentMode.Online);
                Int32 DemandDraft = Convert.ToInt32(enmPaymentMode.DemandDraft);
                Int32 NEFTRTGS = Convert.ToInt32(enmPaymentMode.NEFTRTGS);
                //String strSql = "";
                StringBuilder mySql = new StringBuilder();
                if (PayModeId == Online)
                {
                    mySql.Append(" select distinct o.Reference_Number as TransactionNumber,");
                    if (AppTypeId != 0)
                    {
                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication) || AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                            mySql.Append("cr.Number as ApplicationNumber,");
                        else
                            mySql.Append("cr.Appl_Number as ApplicationNumber,");
                    }

                    mySql.Append(" o.Response_Date as TransactionDate,d.ID as DemandNoteNo," +
                                    " d.Date as DemandNoteDate,d.Amount as DemandNoteAmount," +
                                    " o.Amount as TransactionAmount" +
                                    " from Online_Transaction o, Application_Type a ,Demand_Note d ");
                    if (AppTypeId != 0)
                    {
                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {
                            mySql.Append(" right outer join Course_Exam_Application cr" +
                                         " on d.ID = cr.Demand_Note_ID");
                            if (TransactionStatus == "SUNV")
                                mySql.Append(" where cr.Application_Status_ID in (" + Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) + "," +
                                      Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT) + "," + Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) + ") and ");
                            else
                                mySql.Append(" where ");
                        }
                        else if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                        {
                            mySql.Append("right outer join Certificate_Exam_Application cr" +
                                       " on d.ID = cr.Demand_Note_ID ");
                            if (TransactionStatus == "SUNV")
                                mySql.Append(" where cr.Application_Status_ID in (" + Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre) + "," +
                                     Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre) + "," + Convert.ToInt32(enmCertificateExamApplicationStatus.AppliedButPendingForInstituteVerification) + "," + Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing) + ") and ");
                            else
                                mySql.Append(" where ");
                        }
                        else if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        {
                            mySql.Append("right outer join Course_Registration_Application cr" +
                                     " on d.ID = cr.Demand_Note_ID ");
                            if (TransactionStatus == "SUNV")
                                mySql.Append(" where cr.Application_Status_ID in (" + Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) + "," +
                                     Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT) + "," + Convert.ToInt32(enmCourseApplicationStatus.AppliedButPendingForInstituteVerification) +
                                      "," + Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance) + ") and ");
                            else
                                mySql.Append(" where ");
                        }

                    }

                    mySql.Append("" +
                      "  o.Demand_Note_ID = d.ID and  CAST(o.Request_Date as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and CAST(o.Request_Date as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' and cr.Demand_Note_ID = o.Demand_Note_ID");
                    if (TransactionStatus == "SNU") //Success and not Updated
                        mySql.Append(" and o.Response_Status_Code = '0300' and o.Response_Status_Message is not null" +
                            " and d.Online_Transaction_ID is null");
                    if (TransactionStatus == "SUV")//Success Updated and Verified
                        mySql.Append(" and o.Response_Status_Code = '0300' and o.Response_Status_Message is not null" +
                            " and d.Online_Transaction_ID is not null and d.Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid));
                    if (TransactionStatus == "SUNV")//Success and not Updated and Not Verified
                        mySql.Append(" and o.Response_Status_Code = '0300' and o.Response_Status_Message is not null" +
                            " and d.Online_Transaction_ID is not null and d.Status_ID = " + Convert.ToInt32(enmPaymentStatus.PaidButNotVerified));
                    if (TransactionStatus == "F")//Failed
                        mySql.Append(" and o.Response_Status_Code is null and o.Response_Status_Message is null");
                           
                }

                if (PayModeId == CSCSPV)
                {
                     mySql.Append(" select distinct o.Response_Number as TransactionNumber," );
                    if (AppTypeId != 0)
                    {
                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication) || AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                            mySql.Append("cr.Number as ApplicationNumber,");
                        else
                            mySql.Append("cr.Appl_Number as ApplicationNumber,");
                    }

                    mySql.Append(" o.Date as TransactionDate,d.ID as DemandNoteNo," +
                                " d.Date as DemandNoteDate,d.Amount as DemandNoteAmount," +
                                " o.Amount as TransactionAmount" +
                                " from CSC_Transaction o,Application_Type a, Demand_Note d ");
                    if (AppTypeId != 0)
                    {
                        if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {
                            mySql.Append(" right outer join Course_Exam_Application cr" +
                                         " on d.ID = cr.Demand_Note_ID");
                            if (TransactionStatus == "SUNV")
                                mySql.Append(" where cr.Application_Status_ID in (" + Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) + "," +
                                      Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT) + "," + Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) + ") and ");
                            else
                                mySql.Append(" where ");
                        }
                        else if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                        {
                            mySql.Append("right outer join Certificate_Exam_Application cr" +
                                       " on d.ID = cr.Demand_Note_ID ");
                            if (TransactionStatus == "SUNV")
                                mySql.Append(" where cr.Application_Status_ID in (" + Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre) + "," +
                                     Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre) + "," + Convert.ToInt32(enmCertificateExamApplicationStatus.AppliedButPendingForInstituteVerification) + ") and ");
                            else
                                mySql.Append(" where ");
                        }
                        else if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        {
                            mySql.Append("right outer join Course_Registration_Application cr" +
                                     " on d.ID = cr.Demand_Note_ID ");
                            if (TransactionStatus == "SUNV")
                                mySql.Append(" where cr.Application_Status_ID in (" + Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) + "," +
                                     Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT) + "," + Convert.ToInt32(enmCourseApplicationStatus.AppliedButPendingForInstituteVerification) +
                                      "," + Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance) + ") and ");
                            else
                                mySql.Append(" where ");
                        }
                    }

                    mySql.Append(" " +
                      "  o.Demand_Note_ID = d.ID and  CAST(o.Date as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and  CAST(o.Date as DATE) <= '" + PayToDate.ToString("dd-MMM-yyy") + "' and cr.Demand_Note_ID = o.Demand_Note_ID ");
                    if (TransactionStatus == "SNU")
                        mySql.Append(" and o.Response_Status = 0 and o.Response_Message is not null and o.Response_Number is not null" +
                            " and d.CSC_Transaction_ID is null");
                    if (TransactionStatus == "SUV")
                        mySql.Append(" and o.Response_Status = 0 and o.Response_Message is not null and o.Response_Number is not null" +
                            " and d.CSC_Transaction_ID is not null and d.Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid));
                    if (TransactionStatus == "SUNV")
                        mySql.Append(" and o.Response_Status = 0 and o.Response_Message is not null and o.Response_Number is not null" +
                            " and d.CSC_Transaction_ID is not null and d.Status_ID = " + Convert.ToInt32(enmPaymentStatus.PaidButNotVerified));
                    if (TransactionStatus == "F")
                        mySql.Append(" and o.Response_Status = 1 ");
                }
                if (PayModeId == DemandDraft)
                {
                    if (TransactionStatus == "F")//Failed
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found.";
                    }
                    else
                    {
                        mySql.Append(" select distinct o.Dd_Number as TransactionNumber,");
                        if (AppTypeId != 0)
                        {
                            if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication) || AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                                mySql.Append("cr.Number as ApplicationNumber,");
                            else
                                mySql.Append("cr.Appl_Number as ApplicationNumber,");
                        }

                        mySql.Append(" o.Date as TransactionDate,d.ID as DemandNoteNo," +
                                        " d.Date as DemandNoteDate,d.Amount as DemandNoteAmount," +
                                        " o.DD_Amt as TransactionAmount" +
                                        " from DemandDraftTransaction o,Application_Type a ,Demand_Note d ");
                        if (AppTypeId != 0)
                        {
                            if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                            {
                                mySql.Append(" right outer join Course_Exam_Application cr" +
                                             " on d.ID = cr.Demand_Note_ID");
                                if (TransactionStatus == "SUNV")
                                    mySql.Append(" where cr.Application_Status_ID in (" + Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) + "," +
                                          Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT) + "," + Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) + ") and ");
                                else
                                    mySql.Append(" where ");
                            }
                            else if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                            {
                                mySql.Append("right outer join Certificate_Exam_Application cr" +
                                           " on d.ID = cr.Demand_Note_ID ");
                                if (TransactionStatus == "SUNV")
                                    mySql.Append(" where cr.Application_Status_ID in (" + Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre) + "," +
                                         Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre) + "," + Convert.ToInt32(enmCertificateExamApplicationStatus.AppliedButPendingForInstituteVerification) + ") and ");
                                else
                                    mySql.Append(" where ");
                            }
                            else if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                            {
                                mySql.Append("right outer join Course_Registration_Application cr" +
                                         " on d.ID = cr.Demand_Note_ID ");
                                if (TransactionStatus == "SUNV")
                                    mySql.Append(" where cr.Application_Status_ID in (" + Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) + "," +
                                         Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT) + "," + Convert.ToInt32(enmCourseApplicationStatus.AppliedButPendingForInstituteVerification) +
                                          "," + Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance) + ") and ");
                                else
                                    mySql.Append(" where ");
                            }
                        }

                        mySql.Append("" +
                          "  o.Demand_Note_ID = d.ID and CAST(o.Date as DATE)>= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and  CAST(o.Date as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' and cr.Demand_Note_ID = o.Demand_Note_ID");
                        if (TransactionStatus == "SNU") //Success and not Updated
                            mySql.Append("" +
                                " and d.DD_Transaction_ID is null");
                        if (TransactionStatus == "SUV")//Success Updated and Verified
                            mySql.Append("" +
                                " and d.DD_Transaction_ID is not null and d.Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid));
                        if (TransactionStatus == "SUNV")//Success and not Updated and Not Verified
                            mySql.Append("" +
                                " and d.DD_Transaction_ID is not null and d.Status_ID = " + Convert.ToInt32(enmPaymentStatus.PaidButNotVerified));
                    }
                }
                if (PayModeId == NEFTRTGS)
                {
                    if (TransactionStatus == "F")//Failed
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found.";
                    }
                    else
                    {
                        mySql.Append(" select distinct o.Transaction_Number as TransactionNumber,");
                        if (AppTypeId != 0)
                        {
                            if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication) || AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                                mySql.Append("cr.Number as ApplicationNumber,");
                            else
                                mySql.Append("cr.Appl_Number as ApplicationNumber,");
                        }

                        mySql.Append(" o.Date as TransactionDate,d.ID as DemandNoteNo," +
                                        " d.Date as DemandNoteDate,d.Amount as DemandNoteAmount," +
                                        " o.Transaction_Amt as TransactionAmount" +
                                        " from NEFT_Transaction o,Application_Type a ,Demand_Note d ");
                        if (AppTypeId != 0)
                        {
                            if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                            {
                                mySql.Append(" right outer join Course_Exam_Application cr" +
                                             " on d.ID = cr.Demand_Note_ID");
                                if (TransactionStatus == "SUNV")
                                    mySql.Append(" where cr.Application_Status_ID in (" + Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) + "," +
                                          Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT) + "," + Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) + ") and ");
                                else
                                    mySql.Append(" where ");
                            }
                            else if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                            {
                                mySql.Append("right outer join Certificate_Exam_Application cr" +
                                           " on d.ID = cr.Demand_Note_ID ");
                                if (TransactionStatus == "SUNV")
                                    mySql.Append(" where cr.Application_Status_ID in (" + Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre) + "," +
                                         Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre) + "," + Convert.ToInt32(enmCertificateExamApplicationStatus.AppliedButPendingForInstituteVerification) + ") and ");
                                else
                                    mySql.Append(" where ");
                            }
                            else if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                            {
                                mySql.Append("right outer join Course_Registration_Application cr" +
                                         " on d.ID = cr.Demand_Note_ID ");
                                if (TransactionStatus == "SUNV")
                                    mySql.Append(" where cr.Application_Status_ID in (" + Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) + "," +
                                         Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT) + "," + Convert.ToInt32(enmCourseApplicationStatus.AppliedButPendingForInstituteVerification) +
                                          "," + Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance) + ") and ");
                                else
                                    mySql.Append(" where ");
                            }
                        }

                        mySql.Append("" +
                          "  o.Demand_Note_ID = d.ID and CAST(o.Date as DATE)>= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and  CAST(o.Date as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' and cr.Demand_Note_ID = o.Demand_Note_ID");
                        if (TransactionStatus == "SNU") //Success and not Updated
                            mySql.Append("" +
                                " and d.NEFT_Transaction_ID is null");
                        if (TransactionStatus == "SUV")//Success Updated and Verified
                            mySql.Append("" +
                                " and d.NEFT_Transaction_ID is not null and d.Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid));
                        if (TransactionStatus == "SUNV")//Success and not Updated and Not Verified
                            mySql.Append("" +
                                " and d.NEFT_Transaction_ID is not null and d.Status_ID = " + Convert.ToInt32(enmPaymentStatus.PaidButNotVerified));
                    }
                }
                if (PayModeId == CSCSPV || PayModeId == Online || (PayModeId == DemandDraft && TransactionStatus != "F") || (PayModeId == NEFTRTGS && TransactionStatus != "F"))
                {
                    DataTable dt = DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false);

                    if (dt.Rows.Count > 0)
                    {

                        ShowTableHeader(AppTypeId);
                        int i = 0;
                        foreach (DataRow dtRow in dt.Rows)
                        {
                            TableRow tr = new TableRow();
                            if (i % 2 == 0)
                                tr.CssClass = "gdalternate1";
                            else
                                tr.CssClass = "gdrow1";

                            TableCell tdRow0 = new TableCell();
                            //tdRow0.Width = Unit.Percentage(3);
                            tdRow0.Text = (i + 1).ToString();
                            tdRow0.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow0);

                            TableCell tdRow5 = new TableCell();
                            //tdRow5.Width = Unit.Percentage(10);
                            tdRow5.Text = dtRow["TransactionNumber"].ToString();
                            tdRow5.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow5);

                            TableCell tdRow6 = new TableCell();
                            //tdRow6.Width = Unit.Percentage(12);
                            tdRow6.Text = Convert.ToDateTime(dtRow["TransactionDate"]).ToString("dd-MMM-yyyy hh:mm: tt");
                            tdRow6.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow6);

                            TableCell tdRow1 = new TableCell();
                            //tdRow1.Width = Unit.Percentage(10);
                            DateTime demandNoteDate = Convert.ToDateTime(dtRow["DemandNoteDate"]);
                            tdRow1.Text = dtRow["DemandNoteNo"].ToString() + " / " + demandNoteDate.ToString("dd-MMM-yyyy");
                            tdRow1.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow1);

                            //TableCell tdRow7 = new TableCell();
                            ////tdRow7.Width = Unit.Percentage(12);
                            //tdRow7.HorizontalAlign = HorizontalAlign.Right;
                            //tdRow7.Text = string.Format("{0:0.00}", dtRow["DemandNoteAmount"]);
                            //tr.Cells.Add(tdRow7);

                            if (AppTypeId == 0)
                            {
                                TableCell tdRow4 = new TableCell();
                                //tdRow4.Width = Unit.Percentage(20);
                                tdRow4.Text = dtRow["ApplicantType"].ToString();
                                tdRow4.HorizontalAlign = HorizontalAlign.Left;
                                tr.Cells.Add(tdRow4);
                            }

                            TableCell tdRow3 = new TableCell();
                            //tdRow3.Width = Unit.Percentage(10);
                            tdRow3.Text = dtRow["ApplicationNumber"].ToString();
                            tdRow3.HorizontalAlign = HorizontalAlign.Right;
                            tr.Cells.Add(tdRow3);

                            TableCell tdRow8 = new TableCell();
                            //tdRow8.Width = Unit.Percentage(20);
                            tdRow8.HorizontalAlign = HorizontalAlign.Right;
                            tdRow8.Text = string.Format("{0:0.00}", dtRow["TransactionAmount"]);
                            tr.Cells.Add(tdRow8);

                            tbl.Rows.Add(tr);
                            i++;

                        }
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found !";
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "No Record Found !";
                }
            };
        }

        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
            divReportData.Visible = true;
            ShowData();
            divReportData.Controls.Add(tbl);
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
}