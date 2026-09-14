using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.IdentityModel.Protocols.WSTrust;
using DocumentFormat.OpenXml.Drawing;

public class ApiResponse
{
    public string status { get; set; }
    public string ezpaytranid { get; set; }
    public string amount { get; set; }
    public string trandate { get; set; }
    public string pgrefno { get; set; }
    public string sdt { get; set; }
    public string BA { get; set; }
    public string PF { get; set; }
    public string TAX { get; set; }
    public string PaymentMode { get; set; }
}

public partial class HO_AutoPaymentReconcillationICICI : System.Web.UI.Page
{
    Int32 currentRoleId = 0;
    Int32 matchedDuration = 10;
    Int32 totalRecordsCount = 0;
    Int32 reconcilledRecordsCount = 0;
    Int32 refundValidateRecordCount = 0;
    Int32 isManualSettledRecordcount = 0;
    String TxReferencenumber = "";
    String Productcode = "";
    String BankID = "";
    String BankReferenceNo = "";
    String RefundfileName;
    String txtFilePath;
    String TransactionDate = "";
    FileStream stream = null;
    StreamWriter writer = null;
    StringBuilder appIdList = new StringBuilder();
    StringBuilder appIdList1 = new StringBuilder();
    StringBuilder appIdList2 = new StringBuilder();
    StringBuilder appIdList3 = new StringBuilder();
    StringBuilder appIdList4 = new StringBuilder();
    StringBuilder appIdList5 = new StringBuilder();
    int NotValidateRecords = 0;
    
    protected String requestURL = "https://eazypay.icicibank.com/EazyPGVerify?";
    protected String msg = "";
    
    String merchant_ID = "394294";

   
    String ServiceID = "";
   
   
    //string merchant_id = "111111";
    string key = "3923571242901002";
    string ref_no = "";
    
    
    private async Task<string> GetJsonAsync(string url)
    {
try{
	ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
	
        using (HttpClient client = new HttpClient())
        {
	client.DefaultRequestHeaders.ConnectionClose = true;
            string json = await client.GetStringAsync(url);

            return json;
        }
}
catch (HttpRequestException ex)
{
    
    if (ex.InnerException != null)
    {
        return("Inner Exception:"+ex.InnerException.Message.ToString());
    }
else

	return("Error occurred:"+ex.Message.ToString());
    // Log the full exception details
}
}
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            //if (IsSessionAlive() == false)
             //  Response.Redirect("../Index.aspx");
              currentRoleId = Convert.ToInt32(Session["RoleID"]);
              if (!UserManager.HasRight(currentRoleId, enmRight.View))
              {
                  Response.Write("Sorry! You don't have rights  to view this page");
                  Response.End();
              }
            if (!Page.IsPostBack)
            {


                //ddlpaymentmode.Items.RemoveAt(1); // temporary
                //  BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Auto Payment Reconcillation", "", ""));
            }
        }
        catch (Exception ex)
        {
            //  ShowAlert(ex.Message);
        }
    }
    //payment reconciliation
    protected void btnSave_Click(object sender, EventArgs e)
    {

        try
        {
            ProcessTransactionData();

        }
        catch (Exception ex)
        {
            //  ShowAlert(ex.Message.ToString());
        }

    }
    protected async void ProcessTransactionData()
    {
        try
        {
            //  BreadCrumb1.Render();
            int PaymentPaid = Convert.ToInt32(enmPaymentStatus.Paid);
            List<string> refernce3 = new List<string>();
            //Previous code
            //refernce3.Add("REGN01");
            //refernce3.Add("EXAM01");
            string[] arr = new string[10];
            arr[4] = "<font color='#A81FE9'>Incorrect Data for following Transaction-ID's </font>";
            arr[0] = "<font color='Red'> ChargeBack for following Transaction-ID's </font> ";
            arr[1] = "<font color='#E214B9'> Already Refunded for following Transaction-ID's </font>";
            arr[2] = "<font color='Green'> Status other than \"Pending\" / Already Reconcilled for following Transaction-ID's </font>";
            arr[3] = "<font color='Chocolate'>No record found for following Transaction-ID's </font>";
            arr[5] = "<font color='SlateBlue'> Already Settled for following Transaction-ID's </font>";
            divValidateData.Visible = true;

            btnSave.Visible = false;
            RefundfileName = "NIELITICICI_Refund_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            hffilename.Value = RefundfileName;
            txtFilePath = Server.MapPath("~/Download/" + RefundfileName);
            hffilepath.Value = txtFilePath;
            if (System.IO.File.Exists(txtFilePath))
                System.IO.File.Delete(txtFilePath);
            System.IO.File.Copy(Server.MapPath("~/Download/SettlementICICI.txt"), txtFilePath);
            stream = new FileStream(txtFilePath, FileMode.Open, FileAccess.ReadWrite);
            writer = new StreamWriter(stream);

            /* OleDbConnection connection = new OleDbConnection();
             OleDbCommand command = new OleDbCommand();
             connection.ConnectionString = sExcelConnectionString;
             connection.Open();
             Int32 paymodeid = Convert.ToInt32(ddlpaymentmode.SelectedValue);
             enmPaymentMode paymentMode = (enmPaymentMode)paymodeid;
             if (paymentMode == enmPaymentMode.Online)
             {
                 command = new OleDbCommand("select * from [ICICI$]", connection);
             }

             OleDbDataReader dr = command.ExecuteReader();*/
            DataSet ds = new DataSet();
            ds = utility.getICICIPendingPayments(Convert.ToDateTime(txtDateFrom.Text), Convert.ToDateTime(txtDateto.Text));
            Int32 transid = 0;
            try
            {
                using (EConnectContext context = new EConnectContext())
                {
                    //code changed on 14-Oct-2014 to use separate service-id for courses.
                    var ServiceID = (from r in context.Courses
                                     select new
                                     {
                                         RegistrationServiceID = r.RegistrationServiceID,
                                         ExaminationServiceID = r.ExaminationServiceID,
                                         CertificateServiceID = r.CertificateServiceID,
                                         VafAcfServiceID = r.VAFACFServiceID,
                                         projectServiceID = r.ProjectServiceID
                                     }).ToList();

                    foreach (var service in ServiceID)
                    {
                        refernce3.Add(service.ExaminationServiceID);
                        refernce3.Add(service.RegistrationServiceID);
                        refernce3.Add(service.CertificateServiceID);
                        refernce3.Add(service.VafAcfServiceID);
                        refernce3.Add(service.projectServiceID);
                    }
                    int cnt = Convert.ToInt32(ds.Tables[0].Rows.Count);
                    totalRecordsCount = 0;
                    while (totalRecordsCount < cnt)
                    {
                        totalRecordsCount = totalRecordsCount + 1;

                        transid = Convert.ToInt32(ds.Tables[0].Rows[totalRecordsCount - 1].ItemArray[0].ToString());
                        String str1 = "ezpaytranid=&amount=&paymentmode=&merchantid=" + merchant_ID + "&trandate=&pgreferenceno=I" + transid.ToString();
                        string apiUrl = requestURL + str1; // the API endpoint
                                                          // lblTotalRecords.Text = apiUrl;
//return;
                        
			string result1 = "";
                        result1 = await GetJsonAsync(apiUrl);
			// lblTotalRecords.Text = result1;
			//return;
                        ApiResponse result = new ApiResponse();
                        result = ParseQueryToClass(result1);

                        if (result1 != null)
                        {
                            if (result.status != "Success")
                            {
                                NotValidateRecords = NotValidateRecords + 1;
                                appIdList4.Append(transid.ToString() + ",");
                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                    appIdList4.Append(WebUtility.HtmlDecode("<br/>"));
                                continue;
                            }
                            //status,ezpaytranid,amount,trandate YYYY-MM-DD,pgreferenceno,sdt,ba,pf,tax,paymontmode
                            //status - RIP- Reconciliation in progress, SIP - Settlement in progress ,Success, Failed

                            // Use the deserialized response
                            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(900)))
                            {
                                var online = (from r in context.OnlineTransaction
                                              join d in context.DemandNotes
                                              on r.DemandNoteID equals d.ID
                                              where r.ID == transid
                                              select new
                                              {
                                                  responseCode = r.ResponseStatusCode == null ? "" : r.ResponseStatusCode,
                                                  demannotePaymentstatus = d.PaymentStatusID,
                                                  ID = r.ID,
                                                  Amount = r.Amount,
                                                  DemandNoteID = r.DemandNoteID,
                                                  ReferenceNumber = r.ReferenceNumber,
                                                  RequestDate = r.RequestDate,
                                                  ResponseStatusMessage = r.ResponseStatusMessage,
                                                  IsSettled = r.IsSettled
                                              }).FirstOrDefault();

                                if (online != null)
                                {
                                    TxReferencenumber = result.ezpaytranid;
                                    Productcode = result.PaymentMode;
                                    BankID = "";
                                    BankReferenceNo = "";
                                    TransactionDate = result.trandate;
                                    if ((online.responseCode.Trim() != "E000") || (online.demannotePaymentstatus == Convert.ToInt32(enmPaymentStatus.Pending)))
                                    {

                                        if (IsRefundable(online.DemandNoteID, online.IsSettled, TransactionDate))
                                        {
                                            try
                                            {

                                                AddInRefund(online.ID, TxReferencenumber, Productcode, BankID, BankReferenceNo, TransactionDate.Substring(0, 10));
                                            }
                                            catch
                                            {
                                                NotValidateRecords = NotValidateRecords + 1;
                                                appIdList4.Append(transid.ToString() + ",");
                                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                                    appIdList4.Append(WebUtility.HtmlDecode("<br/>"));
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {

                                                SettleManually(online.ID, TxReferencenumber, Productcode, BankID, BankReferenceNo, TransactionDate.Substring(0, 10));
                                            }
                                            catch (Exception ex)
                                            {
                                                NotValidateRecords = NotValidateRecords + 1;
                                                appIdList4.Append(transid.ToString() + ",");
                                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                                    appIdList4.Append(WebUtility.HtmlDecode("<br/>"));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (online.responseCode.Trim() == "0300" || online.responseCode.Trim() == "E000")
                                            {
                                                //if (IsSuccessRefundable(online.ID))
                                                if (IsSuccessRefundable(online.ID, online.IsSettled))    //vcode  22-12-2022
                                                {
                                                    try
                                                    {
                                                        AddInRefund(online.ID, TxReferencenumber, Productcode, BankID, BankReferenceNo, TransactionDate.Substring(0, 10));
                                                    }
                                                    catch
                                                    {
                                                        NotValidateRecords = NotValidateRecords + 1;
                                                        appIdList4.Append(transid.ToString() + ",");
                                                        if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                                            appIdList4.Append(WebUtility.HtmlDecode("<br/>"));
                                                    }
                                                }
                                                else
                                                {
                                                    if (context.OnlineTransaction.Any(s => s.ID == online.ID && s.ReferenceNumber == online.ReferenceNumber && s.Amount == online.Amount && System.Data.Entity.DbFunctions.TruncateTime(s.RequestDate) == System.Data.Entity.DbFunctions.TruncateTime(online.RequestDate)))
                                                    {
                                                        DemandNote demandNote = context.DemandNotes.Find(online.DemandNoteID);
                                                        if (demandNote != null)
                                                        {
                                                            if (demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending))
                                                            {
                                                                reconcilledRecordsCount = reconcilledRecordsCount + 1;
                                                                demandNote.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);


                                                                OnlineTransaction onlinetransaction = context.OnlineTransaction.Find(online.ID);
                                                                //onlinetransaction.SettledOn = DateTime.Now;
                                                                //vcode 22-12-2022
                                                                onlinetransaction.ReferenceNumber = TxReferencenumber;
                                                                onlinetransaction.IsSettled = true;
                                                                onlinetransaction.SettledBy = Convert.ToInt32(Session["UserID"]);
                                                                onlinetransaction.SettledOn = DateTime.Now;
                                                                onlinetransaction.SettledFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_ICICIAPI";
                                                                //22-12-2022
                                                                if (demandNote.ApplicationTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                                                                {
                                                                    var application = context.CertificateExamApplications.Where(s => s.DemandNoteID == demandNote.ID);
                                                                    if (application != null)
                                                                    {
                                                                        SqlParameter[] para1 ={
                                                                            new SqlParameter("@PaymentPaid",PaymentPaid),
                                                                            new SqlParameter("@ID",demandNote.ID)

                                                                            };
                                                                        //context.Database.ExecuteSqlCommand(" Update Certificate_Exam_Application set  Payment_Status_ID = " + PaymentPaid + " Where Demand_Note_ID = " + demandNote.ID);
                                                                        context.Database.ExecuteSqlCommand(" Update Certificate_Exam_Application set  Payment_Status_ID = @PaymentPaid  Where Demand_Note_ID = @ID", para1);
                                                                        //context.Database.ExecuteSqlCommand(" Update Certificate_Exam_Application set  Payment_Status_ID = " + PaymentPaid + " Where Demand_Note_ID = " + demandNote.ID);
                                                                        context.SaveChanges();
                                                                    }
                                                                }
                                                                else if (demandNote.ApplicationTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                                                                {

                                                                    var application = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandNote.ID);
                                                                    if (application != null)
                                                                    {
                                                                        SqlParameter[] para2 ={
                                                                            new SqlParameter("@PaymentPaid",PaymentPaid),
                                                                            new SqlParameter("@ID",demandNote.ID)

                                                                            };
                                                                        context.Database.ExecuteSqlCommand(" Update Course_Registration_Application set  Payment_Status_ID = @PaymentPaid Where Demand_Note_ID = @ID", para2);
                                                                        //context.Database.ExecuteSqlCommand(" Update Course_Registration_Application set  Payment_Status_ID = " + PaymentPaid + " Where Demand_Note_ID = " + demandNote.ID);
                                                                        context.SaveChanges();
                                                                    }

                                                                }
                                                                else if (demandNote.ApplicationTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                                                                {
                                                                    //Shivam Comment for vaf_acf----------------------------------
                                                                    if (demandNote.FeeTypeID == Convert.ToInt32(enmFeeType.ACF_VAF_fee))
                                                                    {
                                                                        var VerifiedAndVafAcfPaidByInstitute = Convert.ToInt32(enmVaf_AcfApplicationStatus.VerifiedAndVafAcfPaidByInstitute);
                                                                        var Paid = Convert.ToInt32(enmPaymentStatus.Paid);

                                                                        SqlParameter[] para2 ={
                                                                            new SqlParameter("@StatusID",VerifiedAndVafAcfPaidByInstitute),
                                                                            new SqlParameter("@PaidStatus",Paid),
                                                                            new SqlParameter("@DemandNoteID",demandNote.ID)};

                                                                        context.Database.ExecuteSqlCommand("Update Course_Exam_Vaf_ACF set Application_Status_ID = @StatusID " +
                                                                        ", Payment_Status_ID = @PaidStatus where Demand_Note_ID = @DemandNoteID", para2);
                                                                        context.SaveChanges();
                                                                        //IQueryable<CourseExamVafAcf> tempAppl = context.CourseExamVafAcfs.Where(s => s.DemandNoteID == demandNote.ID);

                                                                        //foreach (CourseExamVafAcf appl in tempAppl)
                                                                        //{
                                                                        //    //var sqlquerytest = "Update Course_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute)
                                                                        //    //  + ",Is_Verified_By_Institute = 1 ,Verified_On_By_Institute=" + DateTime.Now + " where Candidate_ID = " + appl.CandidateID + " and Course_Category_ID = " + appl.CourseCategoryID + " and Course_ID =" + appl.CourseID + " and Exam_ID=" + appl.ExamID + " and Institute_ID=" + appl.InstituteID;


                                                                        //    var paymentsourceID = context.CourseExamApplications.Where(c => c.CandidateID == appl.CandidateID && c.CourseID == appl.CourseID && c.CourseCategoryID == appl.CourseCategoryID && c.ExamID == appl.ExamID && c.InstituteID == appl.InstituteID).Select(c => c.PaymentSourceID).FirstOrDefault();
                                                                        //    if (paymentsourceID == 2)
                                                                        //    {
                                                                        //        SqlParameter[] para3 ={
                                                                        //          new SqlParameter("@StatusID",Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute)),
                                                                        //          new SqlParameter("@PaidStatus",Paid),
                                                                        //          new SqlParameter("@CandID",appl.CandidateID),
                                                                        //          new SqlParameter("@CourseCatID",appl.CourseCategoryID),
                                                                        //          new SqlParameter("@CourseID",appl.CourseID),
                                                                        //          new SqlParameter("@ExamID",appl.ExamID),
                                                                        //          new SqlParameter("@InstituteID",appl.InstituteID)
                                                                        //    };
                                                                        //        //context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute)
                                                                        //        // + ",Is_Verified_By_Institute = 1 ,Verified_On_By_Institute= GETDATE() where Candidate_ID = " + appl.CandidateID + " and Course_Category_ID = " + appl.CourseCategoryID + " and Course_ID =" + appl.CourseID + " and Exam_ID=" + appl.ExamID + " and Institute_ID=" + appl.InstituteID);
                                                                        //        context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = @StatusID " +
                                                                        //        " ,Is_Verified_By_Institute = 1 ,Verified_On_By_Institute= GETDATE() where Candidate_ID = @CandID and Course_Category_ID = @CourseCatID and Course_ID =@CourseID and Exam_ID = @ExamID and Institute_ID=@InstituteID", para3);
                                                                        //    }
                                                                        //    else if (paymentsourceID == 1) 
                                                                        //    {
                                                                        //        SqlParameter[] para3 ={
                                                                        //         new SqlParameter("@StatusID",Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT)),
                                                                        //         new SqlParameter("@PaidStatus",Paid),
                                                                        //         new SqlParameter("@CandID",appl.CandidateID),
                                                                        //         new SqlParameter("@CourseCatID",appl.CourseCategoryID),
                                                                        //         new SqlParameter("@CourseID",appl.CourseID),
                                                                        //         new SqlParameter("@ExamID",appl.ExamID),
                                                                        //         new SqlParameter("@InstituteID",appl.InstituteID)
                                                                        //    };
                                                                        //        context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = @StatusID " +
                                                                        //        " ,Is_Verified_By_Institute = 1 ,Verified_On_By_Institute= GETDATE() where Candidate_ID = @CandID and Course_Category_ID = @CourseCatID and Course_ID =@CourseID and Exam_ID = @ExamID and Institute_ID=@InstituteID", para3);

                                                                        //    }
                                                                        //}

                                                                    }
                                                                    //-----------------------------------shivam comment end
                                                                    else
                                                                    {

                                                                        var application = context.CourseExamApplications.Where(s => s.DemandNoteID == demandNote.ID);
                                                                        if (application != null)
                                                                        {
                                                                            // context.Database.ExecuteSqlCommand(" Update Course_Exam_Application set  Payment_Status_ID = " + PaymentPaid + " Where Demand_Note_ID = " + demandNote.ID);
                                                                            SqlParameter[] para3 ={
                                                                            new SqlParameter("@PaymentPaid",PaymentPaid),
                                                                            new SqlParameter("@ID",demandNote.ID)

                                                                            };

                                                                            context.Database.ExecuteSqlCommand(" Update Course_Exam_Application set  Payment_Status_ID = @PaymentPaid  Where Demand_Note_ID = @ID", para3);
                                                                            context.SaveChanges();
                                                                        }
                                                                    }
                                                                }
                                                                else if (demandNote.ApplicationTypeID == Convert.ToInt32(enmApplicationType.CourseProjectApplication))
                                                                {

                                                                    var application = context.CourseProjectApplications.Where(s => s.DemandNoteID == demandNote.ID);
                                                                    if (application != null)
                                                                    {
                                                                        //  context.Database.ExecuteSqlCommand(" Update Course_Project_Application set  Payment_Status_ID = " + PaymentPaid + " Where Demand_Note_ID = " + demandNote.ID);
                                                                        SqlParameter[] para4 ={
                                                                            new SqlParameter("@PaymentPaid",PaymentPaid),
                                                                            new SqlParameter("@ID",demandNote.ID)

                                                                            };

                                                                        context.Database.ExecuteSqlCommand(" Update Course_Project_Application set  Payment_Status_ID = @PaymentPaid  Where Demand_Note_ID = @ID", para4);
                                                                        context.SaveChanges();
                                                                    }
                                                                }
                                                                else if (demandNote.ApplicationTypeID == Convert.ToInt32(enmApplicationType.ModuleCertificateRequest))
                                                                {

                                                                    var application = context.ModuleCertificateRequests.Where(s => s.DemandNoteID == demandNote.ID);
                                                                    if (application != null)
                                                                    {
                                                                        //context.Database.ExecuteSqlCommand(" Update ModuleCertificateRequest set  Payment_Status_ID = " + PaymentPaid + " Where Demand_Note_ID = " + demandNote.ID);
                                                                        SqlParameter[] para5 ={
                                                                            new SqlParameter("@PaymentPaid",PaymentPaid),
                                                                            new SqlParameter("@ID",demandNote.ID)

                                                                            };
                                                                        context.Database.ExecuteSqlCommand(" Update ModuleCertificateRequest set  Payment_Status_ID = @PaymentPaid  Where Demand_Note_ID = @ID", para5);
                                                                        context.SaveChanges();
                                                                    }
                                                                }
                                                                else if (demandNote.ApplicationTypeID == Convert.ToInt32(enmApplicationType.MercyCaseRegistration))   // Added by Komal for Special Registration
                                                                {

                                                                    var application = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandNote.ID);
                                                                    if (application != null)
                                                                    {
                                                                        SqlParameter[] para6 ={
                                                                            new SqlParameter("@PaymentPaid",PaymentPaid),
                                                                            new SqlParameter("@ID",demandNote.ID)

                                                                            };
                                                                        context.Database.ExecuteSqlCommand(" Update Course_Registration_Application set  Payment_Status_ID = @PaymentPaid Where Demand_Note_ID = @ID", para6);
                                                                        //context.Database.ExecuteSqlCommand(" Update Course_Registration_Application set  Payment_Status_ID = " + PaymentPaid + " Where Demand_Note_ID = " + demandNote.ID);
                                                                        context.SaveChanges();
                                                                    }

                                                                }
                                                                else if (demandNote.ApplicationTypeID == Convert.ToInt32(enmApplicationType.SpecialExtension))   // Added by Komal for Special Extension
                                                                {

                                                                    var application = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandNote.ID);
                                                                    if (application != null)
                                                                    {
                                                                        SqlParameter[] para7 ={
                                                                            new SqlParameter("@PaymentPaid",PaymentPaid),
                                                                            new SqlParameter("@ID",demandNote.ID)

                                                                            };
                                                                        context.Database.ExecuteSqlCommand(" Update Course_Registration_Application set  Payment_Status_ID = @PaymentPaid Where Demand_Note_ID = @ID", para7);
                                                                        //context.Database.ExecuteSqlCommand(" Update Course_Registration_Application set  Payment_Status_ID = " + PaymentPaid + " Where Demand_Note_ID = " + demandNote.ID);
                                                                        context.SaveChanges();
                                                                    }

                                                                }

                                                                //saving demand note 
                                                                context.Entry(demandNote).State = System.Data.Entity.EntityState.Modified;
                                                                context.SaveChanges();

                                                                //saving online transaction
                                                                context.Entry(onlinetransaction).State = System.Data.Entity.EntityState.Modified;
                                                                context.SaveChanges();
                                                            }
                                                            else
                                                            {
                                                                NotValidateRecords = NotValidateRecords + 1;
                                                                appIdList2.Append(transid.ToString() + ",");
                                                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                                                    appIdList2.Append(WebUtility.HtmlDecode("<br/>"));
                                                            }
                                                        }
                                                        else
                                                        {
                                                            NotValidateRecords = NotValidateRecords + 1;
                                                            appIdList3.Append(transid.ToString() + ",");
                                                            if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                                                appIdList3.Append(WebUtility.HtmlDecode("<br/>"));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        NotValidateRecords = NotValidateRecords + 1;
                                                        appIdList4.Append(transid.ToString() + ",");
                                                        if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                                            appIdList4.Append(WebUtility.HtmlDecode("<br/>"));
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            NotValidateRecords = NotValidateRecords + 1;
                                            appIdList4.Append(transid.ToString() + ",");
                                            if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                                appIdList4.Append(WebUtility.HtmlDecode("<br/>"));
                                        }
                                    }
                                }
                                else
                                {
                                    NotValidateRecords = NotValidateRecords + 1;
                                    appIdList3.Append(transid.ToString() + ",");
                                    if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                        appIdList3.Append(WebUtility.HtmlDecode("<br/>"));
                                }
                                scope.Complete();
                            };
                        }

                    }


                };

                lblTotalRecords.Text = totalRecordsCount.ToString();
                lblSettledRecords.Text = reconcilledRecordsCount.ToString();
                lblRefundRecords.Text = refundValidateRecordCount.ToString() + " <b> ( Excluding ChargeBack Transactions ) <b/> ";
                if (refundValidateRecordCount > 0)
                    btndownload.Visible = true;
                else
                    btndownload.Visible = false;
                lblManSet.Text = isManualSettledRecordcount.ToString();
                lblFailRecordsCount.Text = ((totalRecordsCount) - (reconcilledRecordsCount + refundValidateRecordCount + isManualSettledRecordcount)).ToString();

                //showing Summary
                if (appIdList4.Length > 0)
                    lblFailedRecords.Text = "<b>" + arr[4].ToString() + ":-<br/>" + appIdList4.ToString().TrimEnd(',').ToString() + "<b/>";

                if (appIdList3.Length > 0)
                {
                    if (lblFailedRecords.Text.Length > 0)
                        lblFailedRecords.Text += "<br/>";
                    lblFailedRecords.Text += "<b>" + arr[3].ToString() + ":-<br/>" + appIdList3.ToString().TrimEnd(',').ToString() + "<b/>";
                }

                if (appIdList2.Length > 0)
                {
                    if (lblFailedRecords.Text.Length > 0)
                        lblFailedRecords.Text += "<br/>";
                    lblFailedRecords.Text += "<b>" + arr[2].ToString() + ":-<br/>" + appIdList2.ToString().TrimEnd(',').ToString() + "<b/>";
                }

                if (appIdList1.Length > 0)
                {
                    if (lblFailedRecords.Text.Length > 0)
                        lblFailedRecords.Text += "<br/>";
                    lblFailedRecords.Text += "<b>" + arr[1].ToString() + ":-<br/>" + appIdList1.ToString().TrimEnd(',').ToString() + "<b/>";
                }

                if (appIdList.Length > 0)
                {
                    if (lblFailedRecords.Text.Length > 0)
                        lblFailedRecords.Text += "<br/>";
                    lblFailedRecords.Text += "<b>" + arr[0].ToString() + "</b>" + ":-<br/>" + appIdList.ToString().TrimEnd(',').ToString() + "<b/>";
                }

                if (appIdList5.Length > 0)
                {
                    if (lblFailedRecords.Text.Length > 0)
                        lblFailedRecords.Text += "<br/>";
                    lblFailedRecords.Text += "<b>" + arr[5].ToString() + "</b>" + ":-<br/>" + appIdList5.ToString().TrimEnd(',').ToString() + "<b/>";
                }

                writer.Close();
                stream.Close();
            }
            catch (Exception ex)
            {
                // ShowAlert(ex.Message, true);
                lblTotalRecords.Text = ex.Message.ToString();
            }
            finally
            {

                ds.Dispose();
                // System.IO.File.Delete(path);
            }
        }
        catch (Exception ex)
        {
            lblTotalRecords.Text = ex.Message.ToString();
        }
    }

    protected Boolean IsRefundable(Int64 DemandNoteID, Boolean IsSettled, string transactionDate)
    {
        if (!IsSettled)
        {
            try
            {
                using (EConnectContext context = new EConnectContext())
                {
                    DemandNote demandNote = context.DemandNotes.Find(DemandNoteID);

                    if (demandNote != null)
                    {
                        if (demandNote.CSCTransaction_ID.HasValue == true || demandNote.DDTransactionID.HasValue == true || demandNote.NEFTTransactionID.HasValue == true || demandNote.OnlineTransactionID.HasValue == true)
                        {
                            return true;
                        }
                        Int32 courseID = 0;
                        Int32 examID = 0;
                        Int32 ApplicantTypeID = 0;

                        Int32 latedateOfExaminationform = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                        Int32 latedateOfRegistrationformfee = Convert.ToInt32(enmActivity.LastDateOfSubmissionOfOnlineRegistrationApplication);
                        //TransactionDate.ToString().Replace("-", "/");
                        DateTime date1 = DateTime.ParseExact(TransactionDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                        // string[] date = TransactionDate.ToString().Substring(0, 10).Split('-');
                        // DateTime date1 = new DateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]));
                        //if (coursExam.Count() > 0 || certificateExam.Count() > 0 || coursReg.Count() > 0 || moduleCertificate.Count() > 0)
                        //{

                        if (demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending))
                        {
                            if (demandNote.ApplicationTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                            {
                                var certificateExam = context.CertificateExamApplications.Where(s => s.DemandNoteID == demandNote.ID).ToList();
                                courseID = certificateExam.FirstOrDefault().CourseID;
                                examID = certificateExam.FirstOrDefault().ExamID;
                                ApplicantTypeID = certificateExam.FirstOrDefault().ApplicantTypeID;
                                var FeeSubmissionInstituteExtPeriod = (from s in context.Exams where s.ID == examID select s.FeeSubmissioInstituteExtPeriod).FirstOrDefault(); //this line added by abhi singh and dated on 21062023
                                var cutoffdates = context.CutOffDates.Where(s => s.ActivityID == latedateOfExaminationform && s.CourseID == courseID && s.ExamID == examID && s.ApplicantTypeID == ApplicantTypeID).FirstOrDefault().EfferctiveDate;
                                // if (cutoffdates.AddDays(FeeSubmissionInstituteExtPeriod).Date < DateTime.Now.Date)
                                //if (cutoffdates.AddDays(matchedDuration).Date < DateTime.Now.Date)
                                // 28 Jul 2023
                                if (cutoffdates.AddDays(FeeSubmissionInstituteExtPeriod).Date < date1)
                                    return true;
                            }
                            else if (demandNote.ApplicationTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                            {
                                //-----vaf acf-------------------------
                                if (demandNote.FeeTypeID == Convert.ToInt32(enmFeeType.ACF_VAF_fee))
                                {
                                    var courseVAFACF = context.CourseExamVafAcfs.Where(s => s.DemandNoteID == demandNote.ID).ToList();
                                    courseID = courseVAFACF.FirstOrDefault().CourseID;
                                    examID = courseVAFACF.FirstOrDefault().ExamID;
                                    ApplicantTypeID = Convert.ToInt16(enmApplicantType.Institute);
                                    var FeeSubmissionInstituteExtPeriod = (from s in context.Exams where s.ID == examID select s.FeeSubmissioInstituteExtPeriod).FirstOrDefault();
                                    var cutoffdates = context.CutOffDates.Where(s => s.ActivityID == latedateOfExaminationform && s.CourseID == courseID && s.ExamID == examID && s.ApplicantTypeID == ApplicantTypeID).FirstOrDefault().EfferctiveDate;
                                    //if (cutoffdates.AddDays(matchedDuration).Date < DateTime.Now.Date)
                                    //    return true;
                                    if (cutoffdates.AddDays(FeeSubmissionInstituteExtPeriod).Date > date1)//removemust
                                        return true;
                                }
                                else
                                {
                                    //-----end vaf acf---------------------
                                    var coursExam = context.CourseExamApplications.Where(s => s.DemandNoteID == demandNote.ID).ToList();
                                    courseID = coursExam.FirstOrDefault().CourseID;
                                    examID = coursExam.FirstOrDefault().ExamID;
                                    ApplicantTypeID = coursExam.FirstOrDefault().ApplicantTypeID;
                                    var FeeSubmissionInstituteExtPeriod = (from s in context.Exams where s.ID == examID select s.FeeSubmissioInstituteExtPeriod).FirstOrDefault(); //this line added by abhi singh and dated on 21062023
                                    var cutoffdates = context.CutOffDates.Where(s => s.ActivityID == latedateOfExaminationform && s.CourseID == courseID && s.ExamID == examID && s.ApplicantTypeID == ApplicantTypeID).FirstOrDefault().EfferctiveDate;
                                    // if (cutoffdates.AddDays(matchedDuration).Date < DateTime.Now.Date)
                                    // if (cutoffdates.AddDays(FeeSubmissionInstituteExtPeriod).Date < DateTime.Now.Date)
                                    //28 Jul 2023
                                    if (cutoffdates.AddDays(FeeSubmissionInstituteExtPeriod).Date < date1)
                                        return true;
                                }
                            }
                            else if (demandNote.ApplicationTypeID == Convert.ToInt32(enmApplicationType.CourseProjectApplication))
                            {
                                var courseProject = context.CourseProjectApplications.Where(s => s.DemandNoteID == demandNote.ID).ToList();

                                var courseProject1 = (from p in context.CourseProjectApplications
                                                      where p.DemandNoteID == demandNote.ID
                                                      select p).FirstOrDefault();
                                courseID = courseProject.FirstOrDefault().CourseID;
                                //examID = courseProject.FirstOrDefault().ExamID;
                                ApplicantTypeID = courseProject.FirstOrDefault().ApplicantTypeID;
                                //var cutoffdates = context.CutOffDates.Where(s => s.ActivityID == latedateOfExaminationform && s.CourseID == courseID && s.ExamID == examID && s.ApplicantTypeID == ApplicantTypeID).FirstOrDefault().EfferctiveDate;

                                // if (cutoffdates.AddDays(matchedDuration).Date < DateTime.Now.Date)

                                // if (courseProject1.DemandNoteValiditydateupto <   DateTime.Now.Date)
                                // 28 Jul 2023
                                if (courseProject1.DemandNoteValiditydateupto < date1)
                                    return true;
                            }

                            else if (demandNote.ApplicationTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                            {
                                var coursReg = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandNote.ID).ToList();
                                courseID = coursReg.FirstOrDefault().CourseID;
                                examID = coursReg.FirstOrDefault().ApplicableExamID.Value;
                                ApplicantTypeID = coursReg.FirstOrDefault().ApplicantTypeID;
                                var FeeSubmissionInstituteExtPeriod = (from s in context.Exams where s.ID == examID select s.FeeSubmissioInstituteExtPeriod).FirstOrDefault(); //this line added by abhi singh and dated on 21062023
                                var cutoffdates = context.CutOffDates.Where(s => s.ActivityID == latedateOfRegistrationformfee && s.CourseID == courseID && s.ExamID == examID && s.ApplicantTypeID == ApplicantTypeID).FirstOrDefault().EfferctiveDate;
                                //if (cutoffdates.AddDays(matchedDuration).Date < DateTime.Now.Date)
                                // if (cutoffdates.AddDays(FeeSubmissionInstituteExtPeriod).Date < DateTime.Now.Date)
                                //28 Jul 2023
                                if (cutoffdates.AddDays(FeeSubmissionInstituteExtPeriod).Date < date1)
                                    return true;
                            }
                            else if (demandNote.ApplicationTypeID == Convert.ToInt32(enmApplicationType.ModuleCertificateRequest))
                            {
                                var moduleCertificate = context.ModuleCertificateRequests.Where(s => s.DemandNoteID == demandNote.ID).ToList();
                                //courseID = moduleCertificate.FirstOrDefault().CourseID;
                                //examID = coursReg.FirstOrDefault().ApplicableExamID.Value;
                                //ApplicantTypeID = coursReg.FirstOrDefault().ApplicantTypeID;
                                //var cutoffdates = context.CutOffDates.Where(s => s.ActivityID == latedateOfRegistrationformfee && s.CourseID == courseID && s.ExamID == examID && s.ApplicantTypeID == ApplicantTypeID).FirstOrDefault().EfferctiveDate;
                                //if (cutoffdates.AddDays(matchedDuration).Date < DateTime.Now.Date)
                                return true;
                            }
                            else if (demandNote.ApplicationTypeID == Convert.ToInt32(enmApplicationType.MercyCaseRegistration)) // Special Registration
                            {
                                var coursReg = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandNote.ID).ToList();
                                courseID = coursReg.FirstOrDefault().CourseID;
                                examID = coursReg.FirstOrDefault().ApplicableExamID.Value;
                                ApplicantTypeID = coursReg.FirstOrDefault().ApplicantTypeID;
                                var FeeSubmissionInstituteExtPeriod = (from s in context.Exams where s.ID == examID select s.FeeSubmissioInstituteExtPeriod).FirstOrDefault(); //this line added by abhi singh and dated on 21062023
                                var cutoffdates = context.CutOffDates.Where(s => s.ActivityID == latedateOfRegistrationformfee && s.CourseID == courseID && s.ExamID == examID && s.ApplicantTypeID == ApplicantTypeID).FirstOrDefault().EfferctiveDate;
                                //if (cutoffdates.AddDays(matchedDuration).Date < DateTime.Now.Date)
                                // if (cutoffdates.AddDays(FeeSubmissionInstituteExtPeriod).Date < DateTime.Now.Date)
                                //28 Jul 2023
                                if (cutoffdates.AddDays(FeeSubmissionInstituteExtPeriod).Date < date1)
                                    return true;
                            }
                            else if (demandNote.ApplicationTypeID == Convert.ToInt32(enmApplicationType.SpecialExtension)) // Special Extension
                            {
                                var coursReg = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandNote.ID).ToList();
                                courseID = coursReg.FirstOrDefault().CourseID;
                                examID = coursReg.FirstOrDefault().ApplicableExamID.Value;
                                ApplicantTypeID = coursReg.FirstOrDefault().ApplicantTypeID;
                                var FeeSubmissionInstituteExtPeriod = (from s in context.Exams where s.ID == examID select s.FeeSubmissioInstituteExtPeriod).FirstOrDefault(); //this line added by abhi singh and dated on 21062023
                                var cutoffdates = context.CutOffDates.Where(s => s.ActivityID == latedateOfRegistrationformfee && s.CourseID == courseID && s.ExamID == examID && s.ApplicantTypeID == ApplicantTypeID).FirstOrDefault().EfferctiveDate;
                                //if (cutoffdates.AddDays(matchedDuration).Date < DateTime.Now.Date)
                                // if (cutoffdates.AddDays(FeeSubmissionInstituteExtPeriod).Date < DateTime.Now.Date)
                                //28 Jul 2023
                                if (cutoffdates.AddDays(FeeSubmissionInstituteExtPeriod).Date < date1)
                                    return true;
                            }
                        }
                        else
                        {
                            if (context.OnlineTransaction.Where(a => a.DemandNoteID == demandNote.ID && (a.ResponseStatusCode == "0300" || a.ResponseStatusCode == "E000")).Count() > 0)
                                return true;
                        }

                        //}
                        //else
                        //{
                        //    return true;
                        //}
                    }
                };
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        return false;
    }
    //protected Boolean IsSuccessRefundable(Int64 onlineTransactionID)
    protected Boolean IsSuccessRefundable(Int64 onlineTransactionID, Boolean IsSettled)   //vcode 22-12-2022
    {
        if (!IsSettled)
        {
            try
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int64 demandID = context.OnlineTransaction.Find(onlineTransactionID).DemandNoteID;
                    var coursVAFACF = context.CourseExamVafAcfs.Where(s => s.DemandNoteID == demandID).ToList();
                    var coursExam = context.CourseExamApplications.Where(s => s.DemandNoteID == demandID).ToList();
                    var certificateExam = context.CertificateExamApplications.Where(s => s.DemandNoteID == demandID).ToList();
                    var coursReg = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandID).ToList();
                    var moduleCertificate = context.ModuleCertificateRequests.Where(s => s.DemandNoteID == demandID).ToList();
                    var courseProject = context.CourseProjectApplications.Where(s => s.DemandNoteID == demandID).ToList();

                    //if (coursExam.Count() > 0 || coursVAFACF.Count() > 0 || certificateExam.Count() > 0 || coursReg.Count() > 0 || moduleCertificate.Count() > 0 || courseProject.Count() > 0)
                    if (coursExam.Count() > 0 || certificateExam.Count() > 0 || coursVAFACF.Count() > 0 || coursReg.Count() > 0 || moduleCertificate.Count() > 0 || courseProject.Count() > 0 || moduleCertificate.Count() > 0)    //vcode 22-12-2022
                    {
                        var demandNote = context.DemandNotes.Find(demandID);
                        if ((demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified) || demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending)) && demandNote.OnlineTransactionID == onlineTransactionID)
                            return false;
                        else
                            return true;
                    }
                    else
                    {
                        return true;
                    }
                };
            }
            catch
            {
                return false;
            }
        }
        return false;
    }
    protected void SettleManually(Int64 OnlineTransactionId, String TxReferencenumber, String Productcode, String BankID, String BankReferenceNo, String TransactionDate)
    {
        try
        {

            using (EConnectContext context = new EConnectContext())
            {
                var online = context.OnlineTransaction.Find(OnlineTransactionId);
                if (online != null && (online.ResponseStatusCode != "0300" || online.ResponseStatusCode != "E000") && !online.IsSettled)
                {
                    try
                    {
                        DemandNote demandNote = context.DemandNotes.Find(online.DemandNoteID);
                        if (demandNote != null)
                        {
                            Int32 coursexamcount = context.CourseExamApplications.Where(s => s.DemandNoteID == demandNote.ID).Count();
                            Int32 coursevafacfcount = context.CourseExamVafAcfs.Where(s => s.DemandNoteID == demandNote.ID).Count();
                            Int32 certificateexamcount = context.CertificateExamApplications.Where(s => s.DemandNoteID == demandNote.ID).Count();
                            Int32 coursregcount = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandNote.ID).Count();
                            Int32 CertificateCount = context.ModuleCertificateRequests.Where(s => s.DemandNoteID == demandNote.ID).Count();
                            Int32 courseprojectcount = context.CourseProjectApplications.Where(s => s.DemandNoteID == demandNote.ID).Count();

                            //if ((demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending) || demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified)) && (coursexamcount > 0 || coursregcount > 0 || certificateexamcount > 0 || courseprojectcount > 0))
                            if ((demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending) || demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified)) && (coursexamcount > 0 || coursregcount > 0 || certificateexamcount > 0 || courseprojectcount > 0 || CertificateCount > 0 || coursevafacfcount > 0))   // vcode 22-12-2022
                            {
                                DateTime createdDate = DateTime.Now;
                                Int32 createdByID = Convert.ToInt32(Session["UserID"]);
                                var trans = (from r in context.OnlineTransaction
                                             where r.DemandNoteID == demandNote.ID && r.ResponseStatusCode == "E000"
                                             select r);
                                if (trans.Count() <= 0)
                                {
                                    //creating history of the online_transaction
                                    SqlParameter[] para1 ={
                                    new SqlParameter("@createdByID",createdByID),
                                     new SqlParameter("@createdDate",createdDate),
                                      new SqlParameter("@ID",online.ID)
                                       };
                                    context.Database.ExecuteSqlCommand("insert into Online_Transaction_History (Request_Date,Demand_Note_ID,Amount,Request_Parameters,Response_Parameters,Response_Date,Reference_Number,Response_Status_Code,Response_Status_Message,Created_By,Is_Settled,Settled_By,Settled_On,Settled_FileName,Transaction_ID,History_Created_By,History_Created_on) (select s.Request_Date,s.Demand_Note_ID,s.Amount,s.Request_Parameters,s.Response_Parameters,s.Response_Date,s.Reference_Number,s.Response_Status_Code,s.Response_Status_Message,s.Created_By, s.Is_Settled, s.Settled_By, s.Settled_On,s.Settled_FileName,s.ID, @createdByID  , @createdDate  from online_transaction s where s.id = @ID)", para1);
                                    //    context.Database.ExecuteSqlCommand("insert into Online_Transaction_History (Request_Date,Demand_Note_ID,Amount,Request_Parameters,Response_Parameters,Response_Date,Reference_Number,Response_Status_Code,Response_Status_Message,Created_By,Is_Settled,Settled_By,Settled_On,Settled_FileName,Transaction_ID,History_Created_By,History_Created_on) (select s.Request_Date,s.Demand_Note_ID,s.Amount,s.Request_Parameters,s.Response_Parameters,s.Response_Date,s.Reference_Number,s.Response_Status_Code,s.Response_Status_Message,s.Created_By, s.Is_Settled, s.Settled_By, s.Settled_On,s.Settled_FileName,s.ID, " + createdByID + " , '" + createdDate + "' from online_transaction s where s.id = " + online.ID + ")");
                                    context.SaveChanges();
                                    // TransactionDate.Replace("-", "/");
                                    //string[] date = TransactionDate.ToString().Substring(0, 10).Split('-');
                                    //DateTime date1 = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));
                                    DateTime date1 = DateTime.ParseExact(TransactionDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                                    online.ResponseDate = date1;
                                    online.ReferenceNumber = Convert.ToString(TxReferencenumber).Trim();
                                    online.ResponseStatusCode = "E000";
                                    online.ResponseStatusMessage = "Success";
                                    online.IsSettled = true;
                                    online.SettledBy = Convert.ToInt32(Session["UserID"]);
                                    online.SettledOn = DateTime.Now;
                                    online.SettledFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_API";

                                    // Updating Demand Note and application Status
                                    DemandNote objdemandnote = context.DemandNotes.Find(online.DemandNoteID);

                                    objdemandnote.OnlineTransactionID = online.ID;
                                    objdemandnote.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                    objdemandnote.PaymentModeID = Convert.ToInt32(enmPaymentMode.Online);
                                    Int32 statusID = 0;

                                    if (objdemandnote.enmApplicationType == enmApplicationType.CertificateExamApplication)
                                    {
                                        if (objdemandnote.enmDemandNoteType == enmDemandNoteType.Single)
                                        {
                                            statusID = Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre);
                                        }
                                        else
                                        {
                                            if (context.CertificateExamApplications.Where(t => t.DemandNoteID.Value == objdemandnote.ID).FirstOrDefault().Exam.IsDispatchable)
                                            {
                                                statusID = Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre);
                                            }
                                            else
                                            {
                                                statusID = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre);
                                            }
                                        }
                                        SqlParameter[] para2 ={
                                             new SqlParameter("@statusID",statusID),
                                                             new SqlParameter("@PaymentStatusID",Convert.ToInt32(enmPaymentStatus.Paid)),
                                                             new SqlParameter("@ID",objdemandnote.ID)
                                                               };
                                        context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = @statusID"
                                                                   + ", Payment_Status_ID = @PaymentStatusID where Demand_Note_ID = @ID", para2);
                                        /* context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = " + statusID
                                             + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " where Demand_Note_ID = " + objdemandnote.ID);*/
                                        context.SaveChanges();
                                    }
                                    else if (objdemandnote.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                                    {
                                        if (objdemandnote.enmDemandNoteType == enmDemandNoteType.Single)
                                            statusID = Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
                                        else
                                            //statusID = Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);
                                            statusID = Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
                                        SqlParameter[] para3 ={
                                                            new SqlParameter("@statusID",statusID),
                                                             new SqlParameter("@PaymentStatusID",Convert.ToInt32(enmPaymentStatus.Paid)),
                                                             new SqlParameter("@ID",objdemandnote.ID)
                                                               };
                                        context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = @statusID"
                                                            + ", Payment_Status_ID = @PaymentStatusID where Demand_Note_ID = @ID", para3);
                                        /*                                        context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + statusID
                                                                                    + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " where Demand_Note_ID = " + objdemandnote.ID);*/
                                        context.SaveChanges();
                                    }
                                    else if (objdemandnote.enmApplicationType == enmApplicationType.CourseExamApplication)
                                    {
                                        //Shivam Comment for vaf_acf----------------------------------
                                        if (demandNote.FeeTypeID == Convert.ToInt32(enmFeeType.ACF_VAF_fee))
                                        {
                                            var VerifiedAndVafAcfPaidByInstitute = Convert.ToInt32(enmVaf_AcfApplicationStatus.VerifiedAndVafAcfPaidByInstitute);
                                            var Paid = Convert.ToInt32(enmPaymentStatus.Paid);

                                            SqlParameter[] para2 ={
                                                                            new SqlParameter("@StatusID",VerifiedAndVafAcfPaidByInstitute),
                                                                            new SqlParameter("@PaidStatus",Paid),
                                                                            new SqlParameter("@DemandNoteID",demandNote.ID)};

                                            context.Database.ExecuteSqlCommand("Update Course_Exam_Vaf_ACF set Application_Status_ID = @StatusID " +
                                            ", Payment_Status_ID = @PaidStatus where Demand_Note_ID = @DemandNoteID", para2);

                                            IQueryable<CourseExamVafAcf> tempAppl = context.CourseExamVafAcfs.Where(s => s.DemandNoteID == demandNote.ID);

                                            foreach (CourseExamVafAcf appl in tempAppl)
                                            {
                                                var sqlquerytest = "Update Course_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute)
                                                  + ",Is_Verified_By_Institute = 1 ,Verified_On_By_Institute=" + DateTime.Now + " where Candidate_ID = " + appl.CandidateID + " and Course_Category_ID = " + appl.CourseCategoryID + " and Course_ID =" + appl.CourseID + " and Exam_ID=" + appl.ExamID + " and Institute_ID=" + appl.InstituteID;

                                                SqlParameter[] para3 ={
                                                                            new SqlParameter("@StatusID",Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute)),
                                                                            new SqlParameter("@PaidStatus",Paid),
                                                                            new SqlParameter("@CandID",appl.CandidateID),
                                                                            new SqlParameter("@CourseCatID",appl.CourseCategoryID),
                                                                            new SqlParameter("@CourseID",appl.CourseID),
                                                                            new SqlParameter("@ExamID",appl.ExamID),
                                                                            new SqlParameter("@InstituteID",appl.InstituteID)
                                                                            };

                                                //context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute)
                                                // + ",Is_Verified_By_Institute = 1 ,Verified_On_By_Institute= GETDATE() where Candidate_ID = " + appl.CandidateID + " and Course_Category_ID = " + appl.CourseCategoryID + " and Course_ID =" + appl.CourseID + " and Exam_ID=" + appl.ExamID + " and Institute_ID=" + appl.InstituteID);
                                                context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = @StatusID " +
                                                " ,Is_Verified_By_Institute = 1 ,Verified_On_By_Institute= GETDATE() where Candidate_ID = @CandID and Course_Category_ID = @CourseCatID and Course_ID =@CourseID and Exam_ID = @ExamID and Institute_ID=@InstituteID", para3);
                                            }

                                        }
                                        //-----------------------------------shivam comment end
                                        else
                                        {
                                            if (objdemandnote.enmDemandNoteType == enmDemandNoteType.Single)
                                            {
                                                Int32 applicantType = (from c in context.CourseExamApplications
                                                                       where c.DemandNoteID == objdemandnote.ID
                                                                       select c.ApplicantTypeID).FirstOrDefault();
                                                if ((enmApplicantType)applicantType == enmApplicantType.Institute)
                                                    statusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification);
                                                else
                                                    statusID = Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
                                            }
                                            else
                                                //statusID = Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);
                                                statusID = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);

                                            SqlParameter[] para4 ={
                                                            new SqlParameter("@statusID",statusID),
                                                             new SqlParameter("@PaymentStatusID",Convert.ToInt32(enmPaymentStatus.Paid)),
                                                             new SqlParameter("@ID",objdemandnote.ID)
                                                               };
                                            context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = @statusID"
                                                                   + ", Payment_Status_ID = @PaymentStatusID where Demand_Note_ID = @ID", para4);
                                            /*context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + statusID
                                                + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " where Demand_Note_ID = " + objdemandnote.ID);*/
                                            context.SaveChanges();
                                        }
                                    }
                                    else if (objdemandnote.enmApplicationType == enmApplicationType.CourseProjectApplication)
                                    {
                                        SqlParameter[] para5 ={
                                         new SqlParameter("@PaymentStatusID",Convert.ToInt32(enmPaymentStatus.Paid)),
                                                             new SqlParameter("@ID",objdemandnote.ID)
                                                               };
                                        if (objdemandnote.enmDemandNoteType == enmDemandNoteType.Single)
                                            context.Database.ExecuteSqlCommand("Update Course_Project_Application set  Payment_Status_ID =@PaymentStatusID where Demand_Note_ID = @ID", para5);
                                        //context.Database.ExecuteSqlCommand("Update Course_Project_Application set  Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " where Demand_Note_ID = " + objdemandnote.ID);
                                        context.SaveChanges();
                                    }

                                    else if (objdemandnote.enmApplicationType == enmApplicationType.ModuleCertificateRequest)
                                    {
                                        SqlParameter[] para6 ={
                                             new SqlParameter("@PaymentStatusID",Convert.ToInt32(enmPaymentStatus.Paid)),
                                                             new SqlParameter("@ID",objdemandnote.ID)
                                                               };
                                        if (objdemandnote.enmDemandNoteType == enmDemandNoteType.Single)
                                            context.Database.ExecuteSqlCommand("Update ModuleCertificateRequest set  Payment_Status_ID = @PaymentStatusID where Demand_Note_ID = @ID", para6);
                                        // context.Database.ExecuteSqlCommand("Update ModuleCertificateRequest set  Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " where Demand_Note_ID = " + objdemandnote.ID);
                                        context.SaveChanges();
                                    }
                                    else if (objdemandnote.enmApplicationType == enmApplicationType.MercyCaseRegistration)   // Added by Komal for Special Registration
                                    {
                                        if (objdemandnote.enmDemandNoteType == enmDemandNoteType.Single)
                                            statusID = Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
                                        else
                                            //statusID = Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);
                                            statusID = Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
                                        SqlParameter[] para7 ={
                                                            new SqlParameter("@statusID",statusID),
                                                             new SqlParameter("@PaymentStatusID",Convert.ToInt32(enmPaymentStatus.Paid)),
                                                             new SqlParameter("@ID",objdemandnote.ID)
                                                               };
                                        context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = @statusID"
                                                            + ", Payment_Status_ID = @PaymentStatusID where Demand_Note_ID = @ID", para7);
                                        /*                                        context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + statusID
                                                                                    + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " where Demand_Note_ID = " + objdemandnote.ID);*/
                                        context.SaveChanges();
                                    }
                                    else if (objdemandnote.enmApplicationType == enmApplicationType.SpecialExtension)   // Added by Komal for Special Extension
                                    {
                                        if (objdemandnote.enmDemandNoteType == enmDemandNoteType.Single)
                                            statusID = Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
                                        else
                                            //statusID = Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);
                                            statusID = Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
                                        SqlParameter[] para8 ={
                                                            new SqlParameter("@statusID",statusID),
                                                             new SqlParameter("@PaymentStatusID",Convert.ToInt32(enmPaymentStatus.Paid)),
                                                             new SqlParameter("@ID",objdemandnote.ID)
                                                               };
                                        context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = @statusID"
                                                            + ", Payment_Status_ID = @PaymentStatusID where Demand_Note_ID = @ID", para8);
                                        /*                                        context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + statusID
                                                                                    + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " where Demand_Note_ID = " + objdemandnote.ID);*/
                                        context.SaveChanges();
                                    }

                                    //saving demand note 
                                    context.Entry(demandNote).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();

                                    //saving online transaction
                                    context.Entry(online).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();

                                    isManualSettledRecordcount = isManualSettledRecordcount + 1;
                                }
                                else
                                {
                                    NotValidateRecords = NotValidateRecords + 1;
                                    appIdList5.Append(OnlineTransactionId.ToString() + ",");
                                    if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                        appIdList5.Append(WebUtility.HtmlDecode("<br/>"));
                                }
                            }
                            else
                            {
                                NotValidateRecords = NotValidateRecords + 1;
                                appIdList2.Append(OnlineTransactionId.ToString() + ",");
                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                    appIdList2.Append(WebUtility.HtmlDecode("<br/>"));
                            }
                        }
                        else
                        {
                            NotValidateRecords = NotValidateRecords + 1;
                            appIdList3.Append(OnlineTransactionId.ToString() + ",");
                            if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                appIdList3.Append(WebUtility.HtmlDecode("<br/>"));
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    NotValidateRecords = NotValidateRecords + 1;
                    appIdList3.Append(OnlineTransactionId.ToString() + ",");
                    if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                        appIdList3.Append(WebUtility.HtmlDecode("<br/>"));
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void AddInRefund(Int64 OnlineTransactionId, String TxReferencenumber, String Productcode, String BankID, String BankReferenceNo, String TransactionDate)
    {
        // *** Code For Creating Refund File ****//
        // BreadCrumb1.Render();
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                OnlineRefund refund;
                var online = context.OnlineTransaction.Find(OnlineTransactionId);
                var demandnote = context.DemandNotes.Find(online.DemandNoteID);
                var coursExam = context.CourseExamApplications.Where(s => s.DemandNoteID == demandnote.ID).ToList();
                var courseVAFACF = context.CourseExamVafAcfs.Where(s => s.DemandNoteID == demandnote.ID).ToList();
                var certificateExam = context.CertificateExamApplications.Where(s => s.DemandNoteID == demandnote.ID).ToList();
                var coursReg = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandnote.ID).ToList();
                var moduleCertificate = context.ModuleCertificateRequests.Where(s => s.DemandNoteID == demandnote.ID).ToList();
                var courseProject = context.CourseProjectApplications.Where(s => s.DemandNoteID == demandnote.ID).ToList();

                //if (online != null && demandnote.OnlineTransactionID!=online.ID)
		if (online != null )
                {
                    // string[] date = TransactionDate.ToString().Substring(0, 10).Split('/');
                    //  DateTime date1 = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));
                    DateTime date1 = DateTime.ParseExact(TransactionDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    Boolean makeRefund = false;
                    Boolean ChargeBack = false;
                    if (online.ResponseStatusCode != "E000" || online.ResponseStatusCode == null)
                    {
                        makeRefund = true;
                    }
                    /*   else if ((online.ResponseStatusCode == "E000") || online.ResponseStatusCode == null)
                       {
                           makeRefund = true;
                       }
                       else if ((online.ResponseStatusCode == "0300") && (demandnote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified)))
                       {
                           makeRefund = true;
                       }*/
                    //else if ((online.ResponseStatusCode == "0300") && (demandnote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Paid)) && (coursExam.Count() == 0 && certificateExam.Count() == 0 && coursReg.Count() == 0) && courseProject.Count() == 0)
                    else if ((online.ResponseStatusCode == "E000") && (demandnote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Paid)) && (coursExam.Count() == 0 && certificateExam.Count() == 0 && coursReg.Count() == 0) && courseProject.Count() == 0 && moduleCertificate.Count() == 0)  //vcode 22-12-2022
                    {
                        makeRefund = true;
                    }
                    else if (demandnote.CSCTransaction_ID.HasValue == true || demandnote.DDTransactionID.HasValue == true || demandnote.NEFTTransactionID.HasValue == true || demandnote.OnlineTransactionID.HasValue == true)
                    {
                        makeRefund = true;
                    }

                    if (makeRefund)
                    {


                        if (context.Online_ChargeBackTransactions.Where(a => a.Ref1 == online.ID).Count() > 0)
                        {
                            ChargeBack = true;
                            makeRefund = false;
                            NotValidateRecords = NotValidateRecords + 1;
                            appIdList.Append(OnlineTransactionId.ToString() + ",");
                            if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                appIdList.Append(WebUtility.HtmlDecode("<br/>"));
                        }
                    }

                    if (makeRefund)
                    {
                        refund = new OnlineRefund();
                        Boolean getRefund = false;
                        var refunRecord = context.OnlineRefunds.Where(s => s.TransactionID == online.ID);
                        if (refunRecord == null || refunRecord.Count() == 0)
                        {
                            refund.BankID = BankID;
                            refund.Bank_Ref_No = BankReferenceNo;
                            refund.ProductID = Productcode;
                            refund.RefundDate = DateTime.Now;
                            refund.TransactionDate = date1;
                            refund.TransactionAmount = Convert.ToInt64(online.Amount);
                            refund.TransactionID = online.ID;
                            refund.Txt_Ref_No = TxReferencenumber;
                            //Double refundamount = Convert.ToDouble(dr[7].ToString());
                            // refund.Refund_Amount = Convert.ToInt64(online.Amount * 100);

                            // on 08-07-2022 vishal
                            if (online.ResponseParameters != null)
                            {
                                string RespPara = "", refundAmt = "";
                                decimal refundAmount = 0;
                                RespPara = Convert.ToString(online.ResponseParameters);
                                //Added additional check 4 Aug 2023 
                                //In case response is received check if req amt and resp amt match , refund else reqAmt may be refunded
                                string Reqpara = "";

                                if (online.RequestParameters != null)
                                {
                                    Reqpara = online.RequestParameters.ToString();
                                    String[] arrayRequest = Reqpara.Split('|');
                                    string amt1 = online.Amount.ToString();
                                    String[] arrayResponse = RespPara.Split('|');
                                    refundAmt = arrayResponse[5].ToString();
                                    if (amt1 != refundAmt)
                                        refundAmt = amt1;
                                }
                                refundAmount = Convert.ToDecimal(refundAmt);
                                refund.Refund_Amount = Convert.ToInt64(refundAmount);
                            }
                            else
                            {

                                refund.Refund_Amount = Convert.ToInt64(online.Amount);
                            }
                            // on 08-07-2022 vishal

                            /*// on 08-07-2022 vishal
                                            if (online.ResponseParameters != null)
                                            {
                                            string RespPara = "", refundAmt = "";
                                            decimal refundAmount = 0;
                                            RespPara = Convert.ToString(online.ResponseParameters);
                                            String[] arrayResponse = RespPara.Split('|');
                                            refundAmt = arrayResponse[4].ToString();
                                            refundAmount = Convert.ToDecimal(refundAmt);
                                            refund.Refund_Amount = Convert.ToInt64(refundAmount * 100);
                                            }
                                        else
                                            {
                                            refund.Refund_Amount = Convert.ToInt64(online.Amount * 100);
                                            }
                                        // on 08-07-2022 vishal*/
                            context.OnlineRefunds.Add(refund);
                            context.SaveChanges();
                            //}
                            //else
                            //{
                            //    refund = context.OnlineRefunds.Where(s => s.TransactionID == online.ID).FirstOrDefault();
                            //}
                            getRefund = true;
                        }
                        else
                        {
                            if (refunRecord.FirstOrDefault().BillDeskRefundDate.HasValue)
                            {
                                NotValidateRecords = NotValidateRecords + 1;
                                appIdList1.Append(OnlineTransactionId.ToString() + ",");
                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                    appIdList1.Append(WebUtility.HtmlDecode("<br/>"));
                            }
                            else
                            {
                                getRefund = true;
                                refund = refunRecord.FirstOrDefault();
                            }
                        }
                        if (getRefund)
                        {
                            refundValidateRecordCount = refundValidateRecordCount + 1;
                            if (refundValidateRecordCount == 1)
                            {
                                writer.Write("TxnType(Status)| Merchant ID| MerchantTransactionID(TranID)| Paymode| TranDate| RootRRNNo| RootAuthCode| Amount| Currency| RootTxnReferenceNumber");
                                writer.WriteLine();
                            }
                            //download of refund file
                            writer.Write("Refund");
                            writer.Write("|");
                            writer.Write(merchant_ID.ToString());
                            writer.Write("|");
                            writer.Write(TxReferencenumber.ToString());
                            writer.Write("|");
                            writer.Write(Productcode.ToString());
                            writer.Write("|");
                            string year = date1.Year.ToString();
                            int monthNumber = Convert.ToInt32(date1.Month.ToString()); // Example: August
                            // For the full month name ("August")
                            string month = new DateTime(2010, monthNumber, 1).ToString("MMMM");
                            string day = date1.Day.ToString();
                            string finaldate = day + month + year;
                            writer.Write(finaldate);
                            writer.Write("|40|40|");
                            writer.Write(refund.Refund_Amount.ToString());
                            writer.Write("|INR|I");
                            writer.Write(OnlineTransactionId.ToString());
                            writer.Write("|");
                            writer.WriteLine();
                        }
                    }
                    else
                    {
                        if (!ChargeBack)
                        {
                            NotValidateRecords = NotValidateRecords + 1;
                            appIdList2.Append(OnlineTransactionId.ToString() + ",");
                            if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                appIdList2.Append(WebUtility.HtmlDecode("<br/>"));
                        }
                    }
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            // ddlpaymentmode.SelectedValue = "0";
		txtDateFrom.Text="";
		txtDateto.Text="";
            divValidateData.Visible = false;
            btndownload.Visible = false;
            btnSave.Visible = true;
            lblFailedRecords.Text = string.Empty;
            lblRefundRecords.Text = string.Empty;
            lblSettledRecords.Text = lblManSet.Text = lblTotalRecords.Text = lblFailRecordsCount.Text = string.Empty;
        }
        catch (Exception ex)
        {
            // ShowAlert(ex.Message);
        }
    }

    #region comment22-12-2022
    /*
    protected void btnValidate_Click(object sender, EventArgs e)
    {
        try
        {
            divValidateData.Visible = true;
            btnSave.Visible = true;
            BreadCrumb1.Render();
            btnValidate.Visible = false;
            string filepath = Server.MapPath("../UploadedFiles");
            flUpload.SaveAs(filepath + "/" + flUpload.FileName);
            string path = (filepath + "/" + flUpload.FileName);
            string ext = System.IO.Path.GetExtension(this.flUpload.PostedFile.FileName);
            string sExcelConnectionString = "";
            if (ext.ToUpper() == ".XLS")
                sExcelConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";", path);
            else if (ext.ToUpper() == ".XLSX")
                sExcelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=No;IMEX=1\";", path);
            else
            {
                ShowAlert("Please Choose .XLS/.XLSX Excel File.", true);
                return;
            }
            OleDbConnection connection = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            connection.ConnectionString = sExcelConnectionString;
            connection.Open();
            Int32 paymodeid = Convert.ToInt32(ddlpaymentmode.SelectedValue);
            enmPaymentMode paymentMode = (enmPaymentMode)paymodeid;
            if (paymentMode == enmPaymentMode.Online)
            {
                command = new OleDbCommand("select * from [Payment Records$]", connection);
            }
            else if (paymentMode == enmPaymentMode.CSCSPV)
            {
                command = new OleDbCommand("select * from [NIELIT Transaction Report$]", connection);
            }
            OleDbDataReader dr = command.ExecuteReader();
            int TotalRecords = 0;
            int ValidateRecords = 0;
            int NotValidateRecords = 0;
            Int32 transid = 0;
            int failedRecordCount = 0;
            string FaildRecords = "No record found for transactionIDs:-";
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        while (dr.Read())
                        {
                            if (paymentMode == enmPaymentMode.Online)
                            {
                                if (CommonFunctions.IsNumeric(dr[7].ToString()))
                                {
                                    TotalRecords = TotalRecords + 1;
                                    transid = Convert.ToInt32(dr[7]);
                                }
                                else
                                {
                                    continue;
                                }
                                //var online = (from r in context.OnlineTransaction
                                //              where r.ID == transid && r.ResponseStatusCode.ToUpper().Trim() == "0300".ToUpper().Trim()
                                //              select r).FirstOrDefault();
                                var online = context.OnlineTransaction.Find(transid);
                                if (online.ResponseStatusCode == "0300")
                                {
                                    DemandNote demandNote = context.DemandNotes.Find(online.DemandNoteID);
                                    if (demandNote != null)
                                    {
                                        if (demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified))
                                        {
                                            if (context.OnlineTransaction.Any(s => s.ID == online.ID && s.ReferenceNumber == online.ReferenceNumber && s.Amount == online.Amount && System.Data.Entity.DbFunctions.TruncateTime(s.RequestDate) == System.Data.Entity.DbFunctions.TruncateTime(online.RequestDate)))
                                            {
                                                ValidateRecords = ValidateRecords + 1;
                                            }
                                            else
                                            {
                                                NotValidateRecords = NotValidateRecords + 1;
                                            }
                                        }
                                        else
                                        {
                                            NotValidateRecords = NotValidateRecords + 1;
                                        }
                                    }
                                }
                                else
                                {
                                    FaildRecords += transid.ToString();
                                    FaildRecords += ",";
                                    failedRecordCount++;
                                    if (failedRecordCount % 25 == 0)
                                        FaildRecords += WebUtility.HtmlDecode("<br/>");
                                }
                            }
                            else if (paymentMode == enmPaymentMode.CSCSPV)
                            {
                                if (CommonFunctions.IsNumeric(dr[8].ToString()))
                                {
                                    TotalRecords = TotalRecords + 1;
                                    transid = Convert.ToInt32(dr[8]);
                                }
                                else
                                {
                                    continue;
                                }
                                //var csc = (from r in context.CSCTransactions
                                //           where r.ID == transid && (r.ResponseStatus.Value == 100 || r.ResponseStatus.Value == 0)
                                //           select r).FirstOrDefault();
                                var csc = context.CSCTransactions.Find(transid);

                                if (csc.ResponseStatus.Value == 100 || csc.ResponseStatus.Value == 0)
                                {
                                    DemandNote demandNote = context.DemandNotes.Find(csc.DemandNoteID);
                                    if (demandNote != null)
                                    {
                                        if (demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified))
                                        {
                                            if (context.CSCTransactions.Any(s => s.ID == csc.ID && s.ResponseTransactionNumber == csc.ResponseTransactionNumber && s.Amount == csc.Amount && System.Data.Entity.DbFunctions.TruncateTime(s.Date) == System.Data.Entity.DbFunctions.TruncateTime(csc.Date)))
                                            {
                                                ValidateRecords = ValidateRecords + 1;
                                            }
                                            else
                                            {
                                                NotValidateRecords = NotValidateRecords + 1;
                                            }
                                        }
                                        else
                                        {
                                            NotValidateRecords = NotValidateRecords + 1;
                                        }
                                    }
                                }
                                else
                                {
                                    FaildRecords += transid.ToString();
                                    FaildRecords += ",";
                                    failedRecordCount++;
                                    if (failedRecordCount % 25 == 0)
                                        FaildRecords += WebUtility.HtmlDecode("<br/>");
                                }
                            }
                        }
                        scope.Complete();
                    };
                };
                lblTotalRecords.Text = TotalRecords.ToString();
                //lblValidateRecords.Text = ValidateRecords.ToString();
                //lblNotValidate.Text = NotValidateRecords.ToString();
                lblFailedRecords.Text = failedRecordCount != 0 ? FaildRecords.TrimEnd(',').ToString() : "";
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message, true);
            }
            finally
            {
                dr.Close();
                dr.Dispose();
                command.Dispose();
                connection.Close();
                connection.Dispose();
                System.IO.File.Delete(path);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    */
    #endregion

    protected void btndownload_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (lblRefundRecords.Text != "0")
            {
                Response.AddHeader("content-disposition", "attachment;filename=" + hffilename.Value);
                Response.ContentType = "text/plain";
                Response.Charset = "UTF-8";
                Response.WriteFile(hffilepath.Value);
                Response.End();
            }
        }
        catch (Exception ex)
        {
            // ShowAlert(ex.Message);
        }
    }
    public static ApiResponse ParseQueryToClass(string query)
    {
        ApiResponse result2 = new ApiResponse();
        var pairs = query.Split('&');

        foreach (var pair in pairs)
        {
            string[] parts = pair.Split(new[] { '=' }, '2');
            string key = parts[0].ToString().Trim();
            string value = parts.Length > 1 ? Uri.UnescapeDataString(parts[1].Trim()) : "";
            //parts[1].ToString();
            //lblTotalRecords.Text=lblTotalRecords.Text+key+":"+value+",";

            switch (key)
            {
                case "status":
                    result2.status = value;
                    break;
                case "ezpaytranid":
                    result2.ezpaytranid = value;
                    break;
                case "amount":
                    if (value != null || value != "NA")
                        result2.amount = value;
                    break;
                case "trandate":
                    if (value != null || value != "NA")
                        result2.trandate = value;
                    break;
                case "pgreferenceno":
                    result2.pgrefno = value;
                    break;
                case "sdt":
                    if (value != null || value != "NA")
                        result2.sdt = value;
                    break;
                case "BA":
                    if (value != null || value != "NA")
                        result2.BA = value;
                    break;
                case "PF":
                    if (value != null || value != "NA")
                        result2.PF = value;
                    break;
                case "TAX":
                    if (value != null)
                        result2.TAX = value;
                    break;
                case "PaymentMode":
                    result2.PaymentMode = value;
                    break;
            }
        }

        return result2;
    }
}

