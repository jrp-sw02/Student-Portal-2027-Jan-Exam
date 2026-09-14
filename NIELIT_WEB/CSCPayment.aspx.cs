using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.UI;
using ConnectPayment;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class CSCPayment : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string csc_share_amount = "0";
        string txn_amount = "0";
        Int32 applicationtypeid = 0;
        Int32 activityid = 0;
        //string _smer = "";
        try
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["DID"] == null)
                {
                    Response.Write("Invalid request parameters");
                    Response.End();
                }
                else
                {
                    Int64 demandNoteID = Convert.ToInt64(Request.QueryString["DID"]);
                    string csc_id = (Request.QueryString["CSCID"]);  //"500100100013";//demo csc id //                 

                    using (TransactionScope scope = new TransactionScope())
                    {
                        using (EConnectContext context = new EConnectContext())
                        {
                            DemandNote demandNote = context.DemandNotes.Find(demandNoteID);
                            var coursExam = context.CourseExamApplications.Where(s => s.DemandNoteID == demandNote.ID).ToList();
                            var certificateExam = context.CertificateExamApplications.Where(s => s.DemandNoteID == demandNote.ID).ToList();
                            var coursReg = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandNote.ID).ToList();                          
                            if (coursExam.Count() > 0 || certificateExam.Count() > 0 || coursReg.Count() > 0 )
                            {
                                if (demandNote != null)
                                {
                                    if (demandNote.enmPaymentStatus == enmPaymentStatus.Pending)
                                    {
                                        txn_amount = demandNote.Amount.ToString();
                                        //save transaction details before initiating the request.
                                        CSCTransaction objtransaction = new CSCTransaction();
                                        objtransaction.Date = DateTime.Now;
                                        objtransaction.DemandNoteID = demandNote.ID;
                                        objtransaction.Amount = Convert.ToDecimal(demandNote.Amount);
                                        context.CSCTransactions.Add(objtransaction);
                                        context.SaveChanges();

                                        if (demandNote.enmDemandNoteType == enmDemandNoteType.Single)
                                        {
                                            if (demandNote.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                                            {
                                                var payee = (from p in context.CourseRegistrationApplications
                                                             where p.DemandNoteID == demandNote.ID
                                                             select new { sourceid = p.ApplicationSourceID }).FirstOrDefault();
                                                if (payee != null)
                                                {
                                                    applicationtypeid = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);

                                                    if (payee.sourceid == Convert.ToInt32(enmApplicationSource.Candidate))
                                                    {
                                                        //_smer = "RFFCO";
                                                        //csc_share_amount = "20";
                                                        activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                                        csc_share_amount = CSCShareAmount(applicationtypeid, activityid);
                                                    }
                                                    else if (payee.sourceid == Convert.ToInt32(enmApplicationSource.CSC))
                                                    {
                                                        //_smer = "RFSFC";
                                                        //csc_share_amount = "55";
                                                        activityid = Convert.ToInt32(enmCSCActivity.FillFormandDepositFee);
                                                        csc_share_amount = CSCShareAmount(applicationtypeid, activityid);
                                                    }
                                                }
                                            }
                                            else if (demandNote.enmApplicationType == enmApplicationType.CertificateExamApplication)
                                            {

                                                var payee = (from p in context.CertificateExamApplications
                                                             where p.DemandNoteID == demandNote.ID
                                                             select new { sourceid = p.ApplicationSourceID }).FirstOrDefault();
                                                if (payee != null)
                                                {
                                                    applicationtypeid = Convert.ToInt32(enmApplicationType.CertificateExamApplication);

                                                    if (payee.sourceid == Convert.ToInt32(enmApplicationSource.Candidate))
                                                    {
                                                        //_smer = "EFFCO";
                                                        //csc_share_amount = "20";
                                                        activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                                        csc_share_amount = CSCShareAmount(applicationtypeid, activityid);
                                                    }
                                                    else if (payee.sourceid == Convert.ToInt32(enmApplicationSource.CSC))
                                                    {
                                                        //_smer = "EFSFC";
                                                        //csc_share_amount = "30";
                                                        activityid = Convert.ToInt32(enmCSCActivity.FillFormandDepositFee);
                                                        csc_share_amount = CSCShareAmount(applicationtypeid, activityid);
                                                    }
                                                }
                                            }
                                            else if (demandNote.enmApplicationType == enmApplicationType.CourseExamApplication)
                                            {
                                                //_smer = "EFFCO";
                                                //csc_share_amount = "20";
                                                applicationtypeid = Convert.ToInt32(enmApplicationType.CourseExamApplication);
                                                activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                                csc_share_amount = CSCShareAmount(applicationtypeid, activityid);
                                            }                                           
                                        }
                                        else
                                        {

                                            if (demandNote.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                                            {
                                                //_smer = "RFFCO";
                                                //csc_share_amount = "20";
                                                applicationtypeid = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                                                activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                                csc_share_amount = CSCShareAmount(applicationtypeid, activityid);
                                            }
                                            //else if (demandNote.enmApplicationType == enmApplicationType.CertificateExamApplication || demandNote.enmApplicationType == enmApplicationType.CourseExamApplication)
                                            else if (demandNote.enmApplicationType == enmApplicationType.CertificateExamApplication)
                                            {
                                                //_smer = "EFFCO";
                                                //csc_share_amount = "20";
                                                applicationtypeid = Convert.ToInt32(enmApplicationType.CertificateExamApplication);
                                                activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                                csc_share_amount = CSCShareAmount(applicationtypeid, activityid);
                                            }
                                            else if (demandNote.enmApplicationType == enmApplicationType.CourseExamApplication)
                                            {
                                                //_smer = "EFFCO";
                                                //csc_share_amount = "20";
                                                applicationtypeid = Convert.ToInt32(enmApplicationType.CourseExamApplication);
                                                activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                                csc_share_amount = CSCShareAmount(applicationtypeid, activityid);
                                            }
                                        }
                                        scope.Complete();
                                        //Request parameters that are sent to Payment Gateway.

                                        BridgePGUtil objBridgePGUtil = new BridgePGUtil();
                                        string merchant_id = ConfigurationManager.AppSettings["MERCHANT_ID"];
                                        string productid = ConfigurationManager.AppSettings["product_id1"];
                                        string productname = ConfigurationManager.AppSettings["product_name1"];
                                        string merchant_receipt_no = merchant_id + DateTime.Now.Year.ToString().PadLeft(4, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0') + DateTime.Now.Millisecond.ToString().PadLeft(4, '0');
                                        string return_url = ConfigurationManager.AppSettings["SUCCESS_URL"];
                                        string cancel_url = ConfigurationManager.AppSettings["FAILURE_URL"];
                                        string merchant_txn = objtransaction.ID.ToString();
                                        string message = objBridgePGUtil.CreateMessage(merchant_id, csc_id, txn_amount, csc_share_amount, merchant_txn, merchant_receipt_no, return_url, cancel_url, productid, productname, demandNoteID.ToString());
                                        message = ConfigurationManager.AppSettings["MERCHANT_ID"] + "|" + message;
                                        Response.Clear();
                                        StringBuilder sb = new StringBuilder();
                                        sb.Append("<html>");
                                        sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
                                        sb.AppendFormat("<form name='form' action='{0}' method='post'>", objBridgePGUtil.CreateURLappendString());
                                        sb.AppendFormat("<input type='hidden' name='message' value='{0}'>", message);
                                        sb.Append("</form>");
                                        sb.Append("</body>");
                                        sb.Append("</html>");
                                        Response.Write(sb.ToString());
                                        Response.End();
                                    }
                                    else
                                        throw new Exception("Demand note number not found / Invalid demand note number / Demand note already paid.");
                                }
                                else
                                    throw new Exception("Demand note number not found / Invalid demand note number.");
                            }
                            else
                            {
                                throw new Exception("You have attempted to edit the application. Kindly re-submit to make payment.");
                            }
                        };
                    };
                }
            }
        }
        catch (Exception)
        {
            //Response.Write("Invalid request parameters." + "<br>" + ex.Message);
            //Response.End();
        }
    }
    protected string CSCShareAmount(Int32 applicationtypeid, Int32 activityid)
    {
        EConnectContext context =  new EConnectContext();
        try
        {
           
            var feeamount = (from d in context.CSCCharges
                             where d.ApplicationTypeID == applicationtypeid &&
                             d.ActivityID == activityid
                             select d.Amount).FirstOrDefault();
            if (feeamount > 0)
                return feeamount.ToString();
            else
                return "0";
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
}