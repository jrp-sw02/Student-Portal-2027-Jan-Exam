using System;
using System.Data;
using System.Text;
using EConnect;
using EConnect.NIELIT;
using EConnect.Utils.Data;

public partial class HO_TransactionDetails : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["demandNoteID"]) && (!String.IsNullOrEmpty(Request.QueryString["TranID"])) && (!String.IsNullOrEmpty(Request.QueryString["PayModeId"])) && (!String.IsNullOrEmpty(Request.QueryString["AppTypeId"])))
                {
                    ShowDetail();
                }
                else
                {
                    Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Home.aspx")));
                    Response.End();
                    return;
                }
            }
        }
        catch (Exception ex)
        { ShowAlert(ex.Message); }
    }
    private void ShowDetail()
    {
        try
        {
            int PayModeId = Convert.ToInt32(Request.QueryString["PayModeId"]);
            int AppTypeId = Convert.ToInt32(Request.QueryString["AppTypeId"]);
            Int32 TranID = Convert.ToInt32(Request.QueryString["TranID"]);
            Int32 CSCSPV = Convert.ToInt32(enmPaymentMode.CSCSPV);
            Int32 Online = Convert.ToInt32(enmPaymentMode.Online);
            Int32 DemandDraft = Convert.ToInt32(enmPaymentMode.DemandDraft);
            Int32 NEFTRTGS = Convert.ToInt32(enmPaymentMode.NEFTRTGS);
            StringBuilder mySql = new StringBuilder();

            if (PayModeId == Online)
            {
                lblTransactionDetail.Text = "Online Transaction Detail";
                LblTransactionDateHead.Text = "Transaction \\ Response Date";
                mySql.Append(" select distinct o.Reference_Number as TransactionNumber, o.Response_Date as TransactionDate,o.Amount as TransactionAmount," +
                               " d.ID as DemandNoteNo,replace(convert(char(11),d.Date,113),' ','-') as DemandNoteDate,d.Amount as DemandNoteAmount, a.Name as ApplicationType," +
                               " f.Name as FeeType, s.Name as PaymentStatus,aps.Name as ApplicationStatus,o.Response_Status_Message as ResponseMsg  ");

                if (AppTypeId != 0)
                {
                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication) || AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        mySql.Append(" ,cr.Number as ApplicationNumber,replace(convert(char(11),cr.Date,113),' ','-') as ApplicationDate");
                    else if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        mySql.Append(" ,cr.Appl_Number as ApplicationNumber,replace(convert(char(11),cr.Application_Date,113),' ','-') as ApplicationDate");
                }

                mySql.Append(" from Online_Transaction o,Fee_Type f,Payment_Status s, Application_Type a, Application_Status aps, Demand_Note d ");
                if (AppTypeId != 0)
                {
                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        mySql.Append(" right outer join Course_Exam_Application cr" +
                                   " on d.ID = cr.Demand_Note_ID");
                    else if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                        mySql.Append(" right outer join Certificate_Exam_Application cr" +
                                  " on d.ID = cr.Demand_Note_ID");
                    else if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        mySql.Append(" right outer join Course_Registration_Application cr" +
                                 " on d.ID = cr.Demand_Note_ID");

                }

                mySql.Append(" where aps.ID = cr.Application_Status_ID and " +
                            "  o.Demand_Note_ID = d.ID and cr.Demand_Note_ID = o.Demand_Note_ID " +
                              " and d.Application_Type_ID = a.ID and cr.Demand_Note_ID = d.ID and d.Fee_Type_ID = f.ID and s.ID = d.Status_ID" +
                              " and o.ID =" + TranID);

            }
            else if (PayModeId == CSCSPV)
            {
                lblTransactionDetail.Text = "CSCSPV Transaction Detail";
                LblTransactionDateHead.Text = "Transaction Date";
                mySql.Append(" select distinct o.Response_Number as TransactionNumber, o.Date as TransactionDate,o.Amount as TransactionAmount," +
                                " d.ID as DemandNoteNo,replace(convert(char(11),d.Date,113),' ','-') as DemandNoteDate,d.Amount as DemandNoteAmount, a.Name as ApplicationType," +
                                " f.Name as FeeType, s.Name as PaymentStatus,aps.Name as ApplicationStatus,o.Response_Message as ResponseMsg  ");

                if (AppTypeId != 0)
                {
                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication) || AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        mySql.Append(" ,cr.Number as ApplicationNumber,replace(convert(char(11),cr.Date,113),' ','-') as ApplicationDate");
                    else if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        mySql.Append(" ,cr.Appl_Number as ApplicationNumber,replace(convert(char(11),cr.Application_Date,113),' ','-') as ApplicationDate");
                }

                mySql.Append(" from CSC_Transaction o,Fee_Type f,Payment_Status s, Application_Type a, Application_Status aps, Demand_Note d ");
                if (AppTypeId != 0)
                {
                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        mySql.Append(" right outer join Course_Exam_Application cr" +
                                   " on d.ID = cr.Demand_Note_ID");
                    else if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                        mySql.Append(" right outer join Certificate_Exam_Application cr" +
                                  " on d.ID = cr.Demand_Note_ID");
                    else if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        mySql.Append(" right outer join Course_Registration_Application cr" +
                                 " on d.ID = cr.Demand_Note_ID");

                }
                mySql.Append(" where aps.ID = cr.Application_Status_ID and " +
                            "  o.Demand_Note_ID = d.ID and cr.Demand_Note_ID = o.Demand_Note_ID " +
                              " and d.Application_Type_ID = a.ID and cr.Demand_Note_ID = d.ID and d.Fee_Type_ID = f.ID and s.ID = d.Status_ID" +
                              " and o.ID =" + TranID);
            }
            else if (PayModeId == DemandDraft)
            {
                lblTransactionDetail.Text = "Demand Draft Transaction Detail";
                lblTransactionNoHead.Text = "Transaction \\ Demand Draft Number";
                LblTransactionDateHead.Text = "Transaction Date";
                mySql.Append(" select distinct o.Dd_Number as TransactionNumber, o.Date as TransactionDate,o.DD_Amt as TransactionAmount," +
                                " d.ID as DemandNoteNo,replace(convert(char(11),d.Date,113),' ','-') as DemandNoteDate,d.Amount as DemandNoteAmount, a.Name as ApplicationType," +
                                " f.Name as FeeType, s.Name as PaymentStatus,aps.Name as ApplicationStatus,replace(convert(char(11),o.DD_Date,113),' ','-') as DemandDraftDate  ");

                if (AppTypeId != 0)
                {
                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication) || AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        mySql.Append(" ,cr.Number as ApplicationNumber,replace(convert(char(11),cr.Date,113),' ','-') as ApplicationDate");
                    else if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        mySql.Append(" ,cr.Appl_Number as ApplicationNumber,replace(convert(char(11),cr.Application_Date,113),' ','-') as ApplicationDate");
                }

                mySql.Append(" from DemandDraftTransaction o,Fee_Type f,Payment_Status s, Application_Type a, Application_Status aps, Demand_Note d ");
                if (AppTypeId != 0)
                {
                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        mySql.Append(" right outer join Course_Exam_Application cr" +
                                   " on d.ID = cr.Demand_Note_ID");
                    else if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                        mySql.Append(" right outer join Certificate_Exam_Application cr" +
                                  " on d.ID = cr.Demand_Note_ID");
                    else if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        mySql.Append(" right outer join Course_Registration_Application cr" +
                                 " on d.ID = cr.Demand_Note_ID");

                }
                mySql.Append(" where aps.ID = cr.Application_Status_ID and " +
                            "  o.Demand_Note_ID = d.ID and cr.Demand_Note_ID = o.Demand_Note_ID " +
                              " and d.Application_Type_ID = a.ID and cr.Demand_Note_ID = d.ID and d.Fee_Type_ID = f.ID and s.ID = d.Status_ID" +
                              " and o.ID =" + TranID);


            }
            else if (PayModeId == NEFTRTGS)
            {
                lblTransactionDetail.Text = "NEFT/ RTGS Transaction Detail";
                lblTransactionNoHead.Text = "NEFT / RTGS Transaction Number";
                LblTransactionDateHead.Text = "Transaction Date";
                mySql.Append(" select distinct o.Transaction_Number as TransactionNumber, o.Date as TransactionDate,o.Transaction_Amt as TransactionAmount," +
                                " d.ID as DemandNoteNo,replace(convert(char(11),d.Date,113),' ','-') as DemandNoteDate,d.Amount as DemandNoteAmount, a.Name as ApplicationType," +
                                " f.Name as FeeType, s.Name as PaymentStatus,aps.Name as ApplicationStatus,replace(convert(char(11),o.Transaction_Date,113),' ','-') as DemandDraftDate  ");

                if (AppTypeId != 0)
                {
                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication) || AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        mySql.Append(" ,cr.Number as ApplicationNumber,replace(convert(char(11),cr.Date,113),' ','-') as ApplicationDate");
                    else if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        mySql.Append(" ,cr.Appl_Number as ApplicationNumber,replace(convert(char(11),cr.Application_Date,113),' ','-') as ApplicationDate");
                }

                mySql.Append(" from NEFT_Transaction o,Fee_Type f,Payment_Status s, Application_Type a, Application_Status aps, Demand_Note d ");
                if (AppTypeId != 0)
                {
                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        mySql.Append(" right outer join Course_Exam_Application cr" +
                                   " on d.ID = cr.Demand_Note_ID");
                    else if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                        mySql.Append(" right outer join Certificate_Exam_Application cr" +
                                  " on d.ID = cr.Demand_Note_ID");
                    else if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        mySql.Append(" right outer join Course_Registration_Application cr" +
                                 " on d.ID = cr.Demand_Note_ID");

                }
                mySql.Append(" where aps.ID = cr.Application_Status_ID and " +
                            "  o.Demand_Note_ID = d.ID and cr.Demand_Note_ID = o.Demand_Note_ID " +
                              " and d.Application_Type_ID = a.ID and cr.Demand_Note_ID = d.ID and d.Fee_Type_ID = f.ID and s.ID = d.Status_ID" +
                              " and o.ID =" + TranID);


            }
            DataTable dt = DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    lblTransactionNo.Text = dtRow["TransactionNumber"].ToString();
                    DateTime transactionDate = Convert.ToDateTime(dtRow["TransactionDate"]);
                    lblTransactionDate.Text = transactionDate.ToString("dd-MMM-yyyy hh:mm:ss tt");
                    lblTransactionAmt.Text = string.Format("{0:0.00}", dtRow["DemandNoteAmount"]);
                    if (PayModeId == DemandDraft || PayModeId == NEFTRTGS)
                    {
                        lblTransaction.Text = "Demand Draft Date";
                        lblResponseStatus.Text = dtRow["DemandDraftDate"].ToString();
                    }
                    else
                    {
                        lblTransaction.Text = "Response Status";
                        lblResponseStatus.Text = dtRow["ResponseMsg"].ToString();
                    }
                    lblDemandNoteNo.Text = dtRow["DemandNoteNo"].ToString();
                    lblDemandNoteDate.Text = dtRow["DemandNoteDate"].ToString();
                    lblDemandNoteAmt.Text = string.Format("{0:0.00}", dtRow["DemandNoteAmount"]);
                    lblApplicationType.Text = dtRow["ApplicationType"].ToString();
                    lblFeeType.Text = dtRow["FeeType"].ToString();
                    lblApplicationNo.Text = dtRow["ApplicationNumber"].ToString();
                    lblApplicationDate.Text = dtRow["ApplicationDate"].ToString();
                    lblDemandNoteStatus.Text = dtRow["PaymentStatus"].ToString();
                    lblApplicationStatus.Text = dtRow["ApplicationStatus"].ToString();
                }
            }

        }
        catch (Exception ex)
        { ShowAlert(ex.Message.ToString()); }
    }
}