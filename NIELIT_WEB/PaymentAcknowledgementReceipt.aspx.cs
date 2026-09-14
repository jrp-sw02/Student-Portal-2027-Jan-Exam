using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Transactions;
using System.Globalization;
using System.Text;

public partial class PaymentAcknowledgementReceipt : BasePage
{
        
    Int64 demandNoteID = 0;
    Int64 registrationNumber = 0;
    Int64 applID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;

        if (!(String.IsNullOrEmpty(Request.QueryString["RegnNumber"]) && String.IsNullOrEmpty(Request.QueryString["DemandNoteId"]) && String.IsNullOrEmpty(Request.QueryString["AppID"])))
            {
                registrationNumber = Convert.ToInt64(Request.QueryString["RegnNumber"]);
                demandNoteID = Convert.ToInt64(Request.QueryString["DemandNoteId"]);
                applID = Convert.ToInt32(Request.QueryString["AppID"]);  
            }           
            if (!Page.IsPostBack)
            {
                showdata();
            }            
       
    }

    protected void showdata()
    {
        if (!(String.IsNullOrEmpty(Request.QueryString["RegnNumber"]) && String.IsNullOrEmpty(Request.QueryString["DemandNoteId"]) && String.IsNullOrEmpty(Request.QueryString["AppID"])))
        {            
            using (EConnectContext context = new EConnectContext())
            {
                CourseProjectApplication appl = context.CourseProjectApplications.Find(applID);
                Candidate candidate = appl.Candidate;               

                lblCandidateName.Text = candidate.Salutation + GetInitCap(candidate.Name);
                lblRegNumber.Text = registrationNumber.ToString();
                lblapplicationnumber.Text = appl.Number;

                var project = (from p in context.CourseProjectApplicationDetails
                               join m in context.Modules
                               on p.ModuleID equals m.ID
                               where p.CourseExamApplicationID == applID
                               select new
                               {
                                   projectname = m.ShortName
                               }).ToList();
                StringBuilder bindname = new StringBuilder();
                foreach (var names in project)
                {
                    bindname.Append(names.projectname+",");
                }
                lblprojectname.Text = bindname.ToString().TrimEnd(',');

                DemandNote demand = context.DemandNotes.Find(demandNoteID);
                Int32 payment_status = Convert.ToInt32(enmPaymentStatus.Paid);

                if (demand.CSCTransaction_ID != null && context.CSCTransactions.Any(c => (c.ID == demand.CSCTransaction_ID)) && context.DemandNotes.Any(c => c.ID == demand.ID && c.PaymentStatusID == payment_status))
                {
                    CSCTransaction csc = context.CSCTransactions.Find(demand.CSCTransaction_ID);
                    PaymentMode mode = demand.PaymentMode;

                    lbldemandnote.Text = demandNoteID.ToString();
                    lblreferencenumber.Text = csc.ResponseTransactionNumber;
                    lbltransactionnumber.Text = demand.CSCTransaction_ID.ToString();
                    lblamount.Text = csc.Amount.ToString("F");
                    lblpaymentmode.Text = mode.Name;
                    lblpaymentdate.Text = csc.Date.ToString("dd-MMM-yyyy");
                    lblstatus.Text = csc.ResponseMessage;
                }
                else if (demand.OnlineTransactionID != null && context.OnlineTransaction.Any(c => c.ID == demand.OnlineTransactionID) && context.DemandNotes.Any(c => c.ID == demand.ID && c.PaymentStatusID == payment_status))
                {                    
                    OnlineTransaction online = context.OnlineTransaction.Find(demand.OnlineTransactionID);
                    PaymentMode mode = demand.PaymentMode;

                    lbldemandnote.Text = demandNoteID.ToString();
                    lblreferencenumber.Text = online.ReferenceNumber;
                    lbltransactionnumber.Text = demand.OnlineTransactionID.ToString();
                    lblamount.Text = online.Amount.ToString("F");
                    lblpaymentmode.Text = mode.Name;
                    lblpaymentdate.Text = online.ResponseDate.HasValue ? online.ResponseDate.Value.ToString("dd-MMM-yyyy") : "NA";                       
                    lblstatus.Text = online.ResponseStatusMessage;
                }
                else if (demand.NEFTTransactionID != null && context.NEFTTransactions.Any(c => c.ID == demand.NEFTTransactionID) && context.DemandNotes.Any(c => c.ID == demand.ID && c.PaymentStatusID == payment_status))
                {
                    NEFTTransaction neft = context.NEFTTransactions.Find(demand.NEFTTransactionID);
                    //NEFTBankTransaction neftbank = context.NEFTBankTransactions.Find(demand.ID);
                    PaymentMode mode = demand.PaymentMode;

                    lbldemandnote.Text = demandNoteID.ToString();
                    lblreferencenumber.Text = neft.TransactionNumber;
                    lbltransactionnumber.Text = demand.NEFTTransactionID.ToString();
                    lblamount.Text = neft.TransactionAmt.ToString("F");
                    lblpaymentmode.Text = mode.Name.Remove(9);
                    lblpaymentdate.Text = neft.TransactionDate.ToString("dd-MMM-yyyy");
                    lblstatus.Text = context.DemandNotes.Any(c => c.ID == demand.ID && c.PaymentStatusID == payment_status) ? "Success" : "Pending";
                }
                else 
                {
                    ShowAlert("Something went wrong, payment reciept can not be generated.!!Kindly contact to NIELIT HQ for more information.");
                    return;
                }
            };
        }
    }

    protected void Lnkhome_Click(object sender, EventArgs e)
    {

        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 Demandid = Convert.ToInt32(Request.QueryString["DemandNoteid"]);
                DemandNote demandnote = context.DemandNotes.Find(Demandid);
                if (demandnote.enmDemandNoteType == enmDemandNoteType.Single && demandnote.ApplicationTypeID != 4)// 4 is for module Certificate
                    Response.Redirect("Index.aspx");
                else
                    Response.Redirect("FrmDashBoard.aspx");
            };
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected string GetInitCap(string str)
    {
        try
        {
            return new CultureInfo("en").TextInfo.ToTitleCase(str.ToLower());
            //return System.Globalization.CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(str.ToLower());
            //return str.Substring(0, 1).ToUpper() + str.Substring(1, str.Length).ToLower();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}