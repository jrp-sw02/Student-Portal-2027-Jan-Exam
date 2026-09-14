using System;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class ApplyModuleCertificate : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    string CourseCatId = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsSessionAlive() == false)
            Response.Redirect("../Index.aspx");
        loginUserType = (UserType)Session["UserType"];
        entityID = Convert.ToInt64(Session["EntityID"]);
        if (!IsPostBack)
        {
            ModuleCertificateRequestEntryAuto();
            BindCourseLevel();
            //BindPaymentOptions();
            BindNewRequest();
        }
        if (!String.IsNullOrEmpty(Request.QueryString["RequestId"]))
        {
            Int32 RequestId = Convert.ToInt32(Request.QueryString["RequestId"]);
            using (var context = new EConnectContext())
            {
                var Detail = context.ModuleCertificateRequests.Find(RequestId);
                DisplayView(Detail.PaymentStatusID);
            }
        }
    }

    void ModuleCertificateRequestEntryAuto()
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection Conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("ModuleCertificateRequestEntryAuto", Conn))
                {
                    Conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    private void BindCourseLevel()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int64 RegistrationNo = Convert.ToInt64(Request.QueryString["RegnNo"]);
                Int64 CandidateID = Convert.ToInt64(Request.QueryString["CandidateID"]);
                ListItem lst = new ListItem("--Select One--", "0");
                int[] validRegistrationId = { 1, 2, 3, 5 };  //Registered, Re-registered, Project-Pending, Expired

                var course = (from s in context.RegistrationDetails
                              join c in context.Courses on s.CourseID equals c.ID
                              where s.RegistrationNo == RegistrationNo
                              && validRegistrationId.Contains(s.RegistrationStatusID.Value)  //Reg, Re-Reg, Project Pending
                              orderby (c.ID)
                              select new { ValueField = s.CourseID, TextField = c.Name }).ToList();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseLevel, course, lst);
            }
        }
        catch (Exception ex)
        { ShowAlert(ex.Message); }
    }

    //commented code by abhi singh on dated  05072023
    //protected void BindPaymentOptions()
    //{
    //    try
    //    {
    //        using (EConnectContext context = new EConnectContext())
    //        {
    //            var paymentTypes = from p in context.PaymentModes
    //                               where p.Visible == true && p.ID != 5
    //                               orderby p.Name
    //                               select new { ValueField = p.ID, TextField = p.Name };

    //            RdoPaymentMode.DataSource = paymentTypes.ToList();
    //            RdoPaymentMode.DataValueField = "ValueField";
    //            RdoPaymentMode.DataTextField = "TextField";

    //            RdoPaymentMode.DataBind();
    //            foreach (ListItem li in RdoPaymentMode.Items)
    //            {
    //                li.Selected = true;
    //                break;
    //            }
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}

    //commented code by abhi singh on dated  05072023
    //protected bool isValidElectronicTransfer()
    //{
    //    try
    //    {
    //        Int32 appID = 0; 
    //        DateTime todaydate = DateTime.Now;
    //        var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
    //       // DateTime Neftdate = Convert.ToDateTime(txtneftdate.Text);
    //      //  string utnumber = Txtnefttransno.Text.ToString();
    //        if (!String.IsNullOrEmpty(Convert.ToString(Request.QueryString["RequestId"])))
    //            appID = Convert.ToInt32(Request.QueryString["RequestId"]);
    //       //commented code by abhi singh dated on 05072023
    //        //if (Neftdate > DateTime.Now.Date)
    //        //{
    //        //    Lbnefterror.Text = "NEFT Transaction date should not be greater than Current Date!";
    //        //    txtneftdate.Focus();
    //        //    return false;
    //        //}
    //        //if (!regexItem.IsMatch(utnumber))
    //        //{
    //        //    Lbnefterror.Text = " Not a valid NEFT Transaction number!";
    //        //    Txtnefttransno.Focus();
    //        //    return false;
    //        //}
    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //commented code by abhi singh on dated  05072023
    //protected void SaveElectronicTransfer()
    //{
    //    try
    //    {
    //        if (isValidElectronicTransfer())
    //        {
    //            Int64 applID = Convert.ToInt64(Request.QueryString["RequestId"]);
    //            //Int64 DemandID = Convert.ToInt64(Request.QueryString["DemandNoteID"]);
    //            StringBuilder mySql = new StringBuilder();
    //            StringBuilder mySql1 = new StringBuilder();
    //            //DateTime Neftdate = Convert.ToDateTime(txtneftdate.Text);
    //           //// String utrNumber = Txtnefttransno.Text.ToString();
    //            //Txtneftamount.Enabled = false;
    //            // amount = Convert.ToDecimal(Txtneftamount.Text.ToString());
    //            using (TransactionScope scope = new TransactionScope())
    //            {
    //                using (EConnectContext context = new EConnectContext())
    //                {
    //                    //DemandNote demand = context.DemandNotes.Find(DemandID);
    //                    var ModuleCertificate = context.ModuleCertificateRequests.Where(s => s.ID == applID).ToList();

    //                    if (ModuleCertificate.Count() > 0)
    //                    {
    //                        //commented code by abhi singh dated on 05072023
    //                        // Verify whether amount is refunded or not
    //                        //if (context.NEFTRefund.Where(s => s.NeftBankTransaction.UTRNUMBER.ToUpper().Trim() == utrNumber.ToUpper().Trim() && System.Data.Entity.DbFunctions.TruncateTime(s.NeftBankTransaction.TransactionDate) == System.Data.Entity.DbFunctions.TruncateTime(Neftdate) && s.NeftBankTransaction.DemandNoteID == null && s.NeftBankTransaction.TransactionAmt == amount && s.NeftBankTransaction.TransactionAmt == demand.Amount).Count() == 0)
    //                        //{
    //                        //    // Verify NEFT Transaction Details
    //                        //    if (context.NEFTBankTransactions.Any(s => s.UTRNUMBER.ToUpper().Trim() == utrNumber.ToUpper().Trim() && s.TransactionAmt == amount && s.TransactionAmt == demand.Amount && System.Data.Entity.DbFunctions.TruncateTime(s.TransactionDate) == System.Data.Entity.DbFunctions.TruncateTime(Neftdate) && s.DemandNoteID == null))
    //                        //    {

    //                        //        //to check same demand note no in two neft transactions
    //                        //        //if (context.NEFTTransactions.Where(s => s.DemandNoteID == demand.ID).Count() > 0)
    //                        //        //{
    //                        //          //  throw new Exception("You have already made the payment for this demand-note.");
    //                        //        //}

    //                        //        //NEFTTransaction neft = new NEFTTransaction();
    //                        //        //neft.TransactionDate = Convert.ToDateTime(txtneftdate.Text.ToString());
    //                        //        //neft.TransactionNumber = Txtnefttransno.Text;
    //                        //        //neft.TransactionAmt = Convert.ToDecimal(Txtneftamount.Text);
    //                        //        //neft.TransactionBank = Txtneftbank.Text.ToUpper();
    //                        //        //neft.CreatedByID = 1;//Convert.ToInt32(Session["UserID"]);
    //                        //        //neft.Date = DateTime.Now;
    //                        //        //neft.DemandNoteID = DemandID;
    //                        //        //context.NEFTTransactions.Add(neft);
    //                        //        //context.SaveChanges();

    //                        //        //demand.NEFTTransactionID = neft.ID;
    //                        //        //demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.NEFTRTGS);
    //                        //        //demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid); // If NEFT transaction Matches
    //                        //        //context.Entry(demand).State = System.Data.Entity.EntityState.Modified;

    //                        //        //// Updating Neft_Bank_Transaction Details
    //                        //        //NEFTBankTransaction neftbank = context.NEFTBankTransactions.Where(s => s.UTRNUMBER.ToUpper().Trim() == utrNumber.ToUpper().Trim() && s.TransactionAmt == amount && s.TransactionAmt == demand.Amount && System.Data.Entity.DbFunctions.TruncateTime(s.TransactionDate) == System.Data.Entity.DbFunctions.TruncateTime(Neftdate) && s.DemandNoteID == null).FirstOrDefault();
    //                        //        //neftbank.DemandNoteID = DemandID;
    //                        //        //context.Entry(neftbank).State = System.Data.Entity.EntityState.Modified;

    //                        //        ////object application = null;
    //                        //        //if (demand.enmApplicationType == enmApplicationType.ModuleCertificateRequest)
    //                        //        //{
    //                        //        //    if (demand.enmDemandNoteType == enmDemandNoteType.Single)
    //                        //        //        context.Database.ExecuteSqlCommand("Update ModuleCertificateRequest set Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demand.ID);

    //                        //        //    context.SaveChanges();
    //                        //        //}

    //                        //        context.SaveChanges();
    //                        //        scope.Complete();
    //                        //        Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("../PaymentReceipt.aspx?DemandNoteid=" + DemandID + "&Src=NEFT"));
    //                        //    }
    //                        //    else
    //                        //    {
    //                        //        Lbnefterror.Text = "Invalid NEFT/RTGS Transaction details. Please enter a valid transaction detail if you have made payment through NEFT/RTGS..";
    //                        //        return;
    //                        //    }
    //                        //}
    //                        //else
    //                        //{
    //                        //    Lbnefterror.Text = "This NEFT/RTGS Transaction Amount is Refunded. So You cannot verify this details.";
    //                        //    return;
    //                        //}
    //                    }
    //                    else
    //                    {
    //                        throw new Exception("You have attempted to edit the application. Kindly re-submit to make payment.");
    //                    }
    //                };
    //            };
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}

    private void DisplayView(Int32 PayStatusId)
    {
        //if (PayStatusId == Convert.ToInt32(enmPaymentStatus.Pending))
        if (PayStatusId == 5)
        {
            using (var context = new EConnectContext())
            {
                var RequestDetail = context.ModuleCertificateRequests.Find(Convert.ToInt32(Request.QueryString["RequestId"]));
                LblSubmitConfirm.Text = "ThankYou, You have submitted the request. " +
                            "Please note the following for future reference.<br /> ";

                //commented code by abhi singh on dated  05072023
                //LblRequestDetail.Text = ("<ul style='font-size: 18px; font-family: Arial Baltic; color: Navy;'> " +
                //    "<li>Request-Id : " + RequestDetail.ID +
                //    "</li><li>DemandNote-Id : " + RequestDetail.DemandNoteID.Value +
                //    "</li><li>Amount Payable (&#8377;) : " + RequestDetail.DemandNote.Amount +
                //    "</li><li>Payment Status : " + RequestDetail.enumPaymentStatus + "</li></ul>").ToString();

                LblRequestDetail.Text = ("<ul style='font-size: 18px; font-family: Arial Baltic; color: Navy;'> " +
                  "<li>Request-Id : " + RequestDetail.ID +
                  "</li></ul>").ToString();

                //commented code by abhi singh on dated  05072023
                //nPayNow.Attributes.Add("onclick", "window.top.location.href='" + EConnect.Utils.Security.QuertStringModule.Encrypt("../OnlinePayment.aspx?DID=" + RequestDetail.DemandNoteID.ToString()) + "'");
                //Txtneftamount.Text = RequestDetail.DemandNote.Amount.ToString("F");
                //mltvTab.ActiveViewIndex = 1; 
                //SubmltvTab.ActiveViewIndex = 1;
            }
        }

        //commented code by abhi singh on dated  05072023
        //if (PayStatusId == Convert.ToInt32(enmPaymentStatus.Paid))
        //{
        //    using (var context = new EConnectContext())
        //    {
        //        var RequestDetail = context.ModuleCertificateRequests.Find(Convert.ToInt32(Request.QueryString["RequestId"]));
        //        string status = RequestDetail.CertificateNumber != null ? "Certificate Number :" + RequestDetail.CertificateNumber : "Under-processing";
        //        //commented code by abhi singh dated on 05072023
        //        //LblRequestStatus.Text = ("<ul style='font-size: 18px; font-family: Arial Baltic; color: Navy;'> " +
        //        //    "<li>Request-Id : " + RequestDetail.ID +
        //        //    "</li><li>Module Details : " + RequestDetail.Module.Code + " - " + RequestDetail.Module.Name +
        //        //    "</li><li>DemandNote-Id : " + RequestDetail.DemandNoteID.Value +
        //        //    "</li><li>Amount Payable (&#8377;) : " + RequestDetail.DemandNote.Amount +
        //        //    "</li><li>Payment Status : " + RequestDetail.enumPaymentStatus +
        //        //     "</li><li>Request Status : " + status + "</li></ul>").ToString();

        //        LblRequestStatus.Text = ("<ul style='font-size: 18px; font-family: Arial Baltic; color: Navy;'> " +
        //           "<li>Request-Id : " + RequestDetail.ID +
        //           "</li><li>Module Details : " + RequestDetail.Module.Code + " - " + RequestDetail.Module.Name +
        //           "</li><li>Request Status : " + status + "</li></ul>").ToString();

        //        mltvTab.ActiveViewIndex = 2;
        //    }

        //}
    }

    //commented code by abhi singh on dated  05072023
    //private void BindModuleFee(Int32 CourseLevelId)
    //{
    //    Int32 FeeTypeId = Convert.ToInt32(enmFeeType.ModuleCertificate);
    //    using (EConnectContext context = new EConnectContext())
    //    {
    //        Int32 FeePayable = context.FeeDetails.Where(s => s.FeeTypeID == FeeTypeId && s.CourseID == CourseLevelId).FirstOrDefault().FeeAmount;
    //        //TxtPayableAmount.Text = FeePayable.ToString();
    //    }
    //}

    private void CourseModule(Int64 RegistrationNo, Int32 CourseLevelId)
    {
       
        using (EConnectContext context = new EConnectContext())
        {
            int[] validGrade = { 1, 2, 3, 4, 6 }; //Pass grade
            int[] validRegistrationId = { 1, 2, 3, 5 };  //Registered, Re-registered, Project-Pending, Expired
            ListItem lst = new ListItem("--Select One--", "0");
            System.Collections.Generic.List<int> ModuleList = (from p in context.CourseExamApplicationDetails
                              join q in context.Modules on p.ModuleID equals q.ID
                              join r in context.RegistrationDetails on p.CandidateID equals r.CandidateID
                              where p.RegistrationNumber == RegistrationNo
                              && p.CourseID == CourseLevelId
                              && validGrade.Contains(p.ResultGradeID.Value)
                              && q.ModuleSubCode != null
                              && validRegistrationId.Contains(r.RegistrationStatusID.Value)
                              //&& q.ModuleTypeID == 1 && p.RegistrationNumber<=4
                              && q.ModuleTypeID == 1 &&  p.ExamYear < 2021
                              select  q.ID).ToList();
           System.Collections.Generic.List<int> SubmittedModule = (from m in context.ModuleCertificateRequests where m.CourseID == CourseLevelId && m.RegistrationNo == RegistrationNo && m.PaymentStatusID!=1  select m.ModuleID).ToList();
           System.Collections.Generic.List<int> finalModuleList = ModuleList.Except(SubmittedModule).ToList();
            var finalmodulelist1 = (from m in context.Modules join q in finalModuleList on m.ID equals q select new { ValueField = m.ID, TextField = m.ShortName + ": " + m.Name }).ToList();
           // EConnect.Utils.Common.ControlUtility.BindListObject(ddlModule, ModuleList, lst);
            
            EConnect.Utils.Common.ControlUtility.BindListObject(ddlModule, finalmodulelist1, lst);

            if (ModuleList.Count() == 0)
            {
                //ShowAlert("You are not eligible for a module certificate");
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('Module-Wise Certificate from Revision 5 onwards will be processed automatically upon module completion.Candidate need not to apply for that !!')", true);
            }
            else
            {
                if (finalmodulelist1.Count < 1)
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('No more module remaining to apply for Module-Wise Certificate!')", true);
                }
            }
        }
    }
    private Int32 IsAlreadyApplied(Int64 RegistrationNo, Int32 ModuleId)
    {
        Int32 RequestId = 0;
        using (var context = new EConnectContext())
        {
            var Request = context.ModuleCertificateRequests.Where(p => p.RegistrationNo == RegistrationNo && p.ModuleID == ModuleId && p.PaymentStatusID!=1).FirstOrDefault();
            RequestId = (Request == null) ? 0 : Request.ID;
        }
        return RequestId;
    }
    protected void BindNewRequest()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                Int64 RegistrationNo = Convert.ToInt64(Request.QueryString["RegnNo"]);
                Int32 PayStatus = Convert.ToInt32(enmPaymentStatus.Paid);
                var application = (from p in context.ModuleCertificateRequests
                                   where p.RegistrationNo == RegistrationNo
                                   && p.PaymentStatusID != 1
                                   select new
                                   {
                                       CourseId = p.CourseID,
                                       CourseName = p.Course.Name,
                                       ModuleName = p.Module.ShortName + " - " + p.Module.Name,
                                       PaymentStatus = p.PaymentStatus.Name,
                                       RequestType= p.RequestType=="Auto"?"Auto":"By Candidate",
                                       Status = string.IsNullOrEmpty(p.CertificateNumber) ? "Under Processing" : "Certificate No. " + p.CertificateNumber
                                   });

                application = application.OrderBy(p => p.CourseId);
                PagingBar1.Bind(application, ref gbModule);
                uPnlGrid1.Update();
                uPnlNavigation.Update();
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }
    //commented code by abhi singh on dated  05072023
    //protected void Btnneftsubmit_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        SaveElectronicTransfer();
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        Int64 RegistrationNo = Convert.ToInt64(Request.QueryString["RegnNo"]);
        Int32 CourseLevelId = Convert.ToInt32(ddlCourseLevel.SelectedValue);
        Int32 ModuleId = Convert.ToInt32(ddlModule.SelectedValue);
        Int32 RequestId = 0; Int64 DemandId = 0;
        if (CourseLevelId <= 0 || ModuleId <= 0)
        { throw new Exception("You have not selected Course Level and/or Course Module"); return; }
        RequestId = IsAlreadyApplied(RegistrationNo, ModuleId);
        if (RequestId != 0)
        {
           // using (var context = new EConnectContext())
                //DemandId = context.ModuleCertificateRequests.Find(RequestId).DemandNoteID.Value;
        }

        try
        {
            if (RequestId == 0)
            {

                using (EConnectContext context = new EConnectContext())
                {
                    String serviceId = context.Courses.Find(CourseLevelId).CertificateServiceID;
                    using (TransactionScope scope = new TransactionScope())
                    {
                        ModuleCertificateRequest NewRequest = new ModuleCertificateRequest();
                        NewRequest.RegistrationNo = RegistrationNo;
                        NewRequest.CourseID = CourseLevelId;
                        NewRequest.ModuleID = ModuleId;
                        NewRequest.RequestDate = DateTime.Now;
                        //NewRequest.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                        NewRequest.PaymentStatusID = 5;
                        NewRequest.IsDownloaded = false;
                        NewRequest.IsDuplicate = false;
                        context.ModuleCertificateRequests.Add(NewRequest);
                        context.SaveChanges();

                        RequestId = NewRequest.ID;

                        //commented code by abhi singh on dated  05072023
                        //DemandNote demand = new DemandNote();
                        //demand.ApplicationDate = DateTime.Now;
                        //demand.FeeTypeID = Convert.ToInt32(enmFeeType.ModuleCertificate);
                        //demand.ApplicationTypeID = Convert.ToInt32(enmApplicationType.ModuleCertificateRequest);
                        //demand.PaymentModeID = Convert.ToInt32(enmPaymentMode.CSCSPV);
                        //demand.DemandNoteTypeID = Convert.ToInt32(enmDemandNoteType.Single);
                        //demand.Amount = Convert.ToInt32(TxtPayableAmount.Text);
                        //demand.CreatedBy = 1;
                        //demand.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                        //demand.CourseCategoryID = 1;
                        //demand.CourseID = CourseLevelId;
                        //demand.ServiceID = serviceId;
                        //context.DemandNotes.Add(demand);
                        //context.SaveChanges();

                        //NewRequest.DemandNoteID = demand.ID;

                        //DemandId = demand.ID;
                        LblError.Visible = false;
                        context.Entry(NewRequest).State = System.Data.Entity.EntityState.Modified;
                        context.SaveChanges();

                        scope.Complete();
                        Response.Redirect("ApplyModuleCertificate.aspx?RegnNo=" + RegistrationNo.ToString(), true);
                    }
                }
            }
            else {
                //LblError.Visible = true;
                //LblError.Text = "You have already applied for this course module";
                //ShowAlert("You have already applied for this course module");
               System.Web.UI.ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('You have already applied for this course module')", true);
            }
            
        }
        catch (Exception ex)
        { ShowAlert(ex.Message); }

    }
    protected void ddlCourseLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Int64 RegistrationNo = Convert.ToInt64(Request.QueryString["RegnNo"]);
            Int32 CourseLevelId = Convert.ToInt32(ddlCourseLevel.SelectedValue);

            ddlModule.Items.Clear();
            //TxtPayableAmount.Text = "";

            CourseModule(RegistrationNo, CourseLevelId);
           // BindModuleFee(CourseLevelId);
        }
        catch (Exception ex)
        { ShowAlert(ex.Message); }
    }

    //commented code by abhi singh on dated  05072023
    //protected void RdoPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        switch (Convert.ToInt32(RdoPaymentMode.SelectedValue))
    //        {
    //            //case 5:
    //            //    SubmltvTab.ActiveViewIndex = 0;
    //            //    break;
    //            case 2:
    //                SubmltvTab.ActiveViewIndex = 0;
    //                break;
    //            case 6:
    //                SubmltvTab.ActiveViewIndex = 1;
    //                break;
    //            default:
    //                SubmltvTab.ActiveViewIndex = 0;
    //                break;
    //        }
    //    }
    //    catch (Exception)
    //    {

    //    }
    //}
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gbModule.PageIndex = PagingBar1.CurrentPageIndex;
            BindNewRequest();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gbModule_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }

}