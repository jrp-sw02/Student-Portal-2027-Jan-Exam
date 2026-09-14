using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using BridgePG;
using ConnectPayment;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Transactions;
using System.IO;
using System.Web;

public partial class CSCResponse : BasePage
{
    //protected String xmlResponse = "";
    public string bridgeResponseMessage = "Error ", drcResponse = "Error", walletResponseMessage = "", merchant_txn = "", merchant_txn_date_time = "", csc_txn = "", product_id = "", product_name = "", merchant_id = "", csc_id = "", txn_amount = "", pay_to_email = "", amount_parameter = "", txn_mode = "", txn_type = "", merchant_receipt_no = "", csc_share_amount = "", Currency = "", Discount = "", status_message = "", txn_status = "", txn_status_message = "", DemandNoteId = "", param_2 = "", param_3 = "", param_4 = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            if (!Page.IsPostBack)
            {

                //Added scope 30 Mar 2020
                using (TransactionScope scope = new TransactionScope())
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        //Added to increase timeout 30 Mar2020
                        context.Database.CommandTimeout = 600;
                        NameValueCollection nvc = Request.Form;
                        if (!string.IsNullOrEmpty(nvc["bridgeResponseMessage"]))
                        {

                            bridgeResponseMessage = nvc["bridgeResponseMessage"];
                            string strPrivateKey = ConfigurationManager.AppSettings["PRIVATE_KEY"];
                            string strPublicKey = ConfigurationManager.AppSettings["PUBLIC_KEY"];
                            strPrivateKey = Base64Decode(strPrivateKey);
                            strPublicKey = Base64Decode(strPublicKey);
                            Crypto.privateKey = strPrivateKey;
                            drcResponse = Crypto.decrypt(bridgeResponseMessage, strPrivateKey, strPublicKey, true);
                            string[] arr = drcResponse.Split("|".ToCharArray());
                            for (int i = 0; i < arr.Length; i++)
                            {
                                string[] arr2 = arr[i].Split("=".ToCharArray());
                                if (arr2[0] == "merchant_txn") merchant_txn = arr2[1];//mtrxid as in old pg
                                if (arr2[0] == "merchant_txn_date_time") merchant_txn_date_time = arr2[1];
                                if (arr2[0] == "csc_txn") csc_txn = arr2[1]; //oxitrxid as in old pg
                                if (arr2[0] == "product_id") product_id = arr2[1];//pid as in web config
                                if (arr2[0] == "product_name") product_name = arr2[1];// as in web config
                                if (arr2[0] == "merchant_id") merchant_id = arr2[1];
                                if (arr2[0] == "csc_id") csc_id = arr2[1];
                                if (arr2[0] == "txn_amount") txn_amount = arr2[1];
                                if (arr2[0] == "pay_to_email") pay_to_email = arr2[1];
                                if (arr2[0] == "amount_parameter") amount_parameter = arr2[1];
                                if (arr2[0] == "txn_mode") txn_mode = arr2[1];
                                if (arr2[0] == "txn_type") txn_type = arr2[1];
                                if (arr2[0] == "merchant_receipt_no") merchant_receipt_no = arr2[1];
                                if (arr2[0] == "csc_share_amount") csc_share_amount = arr2[1];
                                if (arr2[0] == "Currency") Currency = arr2[1];
                                if (arr2[0] == "Discount") Discount = arr2[1];
                                if (arr2[0] == "status_message") status_message = arr2[1];
                                if (arr2[0] == "txn_status") txn_status = arr2[1];
                                if (arr2[0] == "txn_status_message") txn_status_message = arr2[1];
                                if (arr2[0] == "param_1") DemandNoteId = arr2[1];
                                if (arr2[0] == "param_2") param_2 = arr2[1];
                                if (arr2[0] == "param_3") param_3 = arr2[1];
                                if (arr2[0] == "param_4") param_4 = arr2[1];
                            }

                            CSCTransaction objtransaction = new CSCTransaction();
                            objtransaction = context.CSCTransactions.Find(Convert.ToInt64(merchant_txn));//merchant_txn
                            objtransaction.Amount = Convert.ToDecimal(txn_amount);//txn_amount
                            objtransaction.ResponseTransactionNumber = csc_txn;//csc_txn
                            objtransaction.ResponseStatus = Convert.ToInt32(txn_status);//txn_status
                            objtransaction.ResponseMessage = txn_status_message;//.Length > 20 ? trxmsg.Substring(0, 20) : trxmsg; ;//txn_status_message
                            objtransaction.CreatedByID = 1;
                            objtransaction.DemandNoteID = Convert.ToInt64(DemandNoteId);



                            if (txn_status == "100")//successfull Transaction
                            {
                                divResponse.Visible = true;
                                lblError.Visible = true;
                                lblError.Text = "Your CSC SPV transaction has been successfully completed. Please note below details for reference.";

                                DemandNote objdemandNote = context.DemandNotes.Find(objtransaction.DemandNoteID);
                                objdemandNote.CSCTransaction_ID = objtransaction.ID;
                                objdemandNote.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.PaidButNotVerified);
                                objdemandNote.PaymentModeID = Convert.ToInt32(enmPaymentMode.CSCSPV);
                                Int32 statusID = 0;

                                //if (objdemandNote.enmApplicationType == enmApplicationType.CertificateExamApplication)
                                //{
                                //    if (objdemandNote.enmDemandNoteType == enmDemandNoteType.Single)
                                //        statusID = Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre);
                                //    else
                                //    {
                                //        if (context.CertificateExamApplications.Where(s => s.DemandNoteID.Value == objdemandNote.ID).FirstOrDefault().Exam.IsDispatchable)
                                //            statusID = Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre);
                                //        else
                                //            statusID = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre);
                                //    }

                                //     //Updated to avoid SQL Injection
                                //    string sql="Update Certificate_Exam_Application set Application_Status_ID = {0}, Payment_Status_ID = {1} where Demand_Note_ID = {2}";

                                //    context.Database.ExecuteSqlCommand(sql, statusID, Convert.ToInt32(enmPaymentStatus.PaidButNotVerified),objdemandNote.ID);
                                //    //context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = " + statusID
                                //      //  + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.PaidButNotVerified) + " where Demand_Note_ID = " + objdemandNote.ID);
                                //}
                                //else if (objdemandNote.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                                //{
                                //    if (objdemandNote.enmDemandNoteType == enmDemandNoteType.Single)
                                //    {
                                //        Int32 applicantType = (from c in context.CourseRegistrationApplications
                                //                               where c.DemandNoteID == objdemandNote.ID
                                //                               select c.ApplicantTypeID).FirstOrDefault();
                                //        if ((enmApplicantType)applicantType == enmApplicantType.Institute)
                                //            statusID = Convert.ToInt32(enmCourseApplicationStatus.AppliedButPendingForInstituteVerification);
                                //        else
                                //            statusID = Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
                                //    }
                                //    else
                                //        statusID = Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);

                                //         //Updated to avoid SqlInjection
                                //    string sql="Update Course_Registration_Application set Application_Status_ID = {0}, Payment_Status_ID = {1} where Demand_Note_ID = {2}";
                                //    context.Database.ExecuteSqlCommand(sql, statusID, Convert.ToInt32(enmPaymentStatus.PaidButNotVerified), objdemandNote.ID);
                                //  //  context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + statusID
                                //    //    + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.PaidButNotVerified) + " where Demand_Note_ID = " + objdemandNote.ID);
                                //}
                                //else if (objdemandNote.enmApplicationType == enmApplicationType.CourseExamApplication)
                                //{

                                //    if (objdemandNote.enmDemandNoteType == enmDemandNoteType.Single)
                                //    {
                                //        Int32 applicantType = (from c in context.CourseExamApplications
                                //                               where c.DemandNoteID == objdemandNote.ID
                                //                               select c.ApplicantTypeID).FirstOrDefault();
                                //        if ((enmApplicantType)applicantType == enmApplicantType.Institute)
                                //            statusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification);
                                //        else
                                //            statusID = Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
                                //    }
                                //    else
                                //        statusID = Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);

                                //         string sql="Update Course_Exam_Application set Application_Status_ID ={0}, Payment_Status_ID = (1) where Demand_Note_ID = {2}";
                                //    context.Database.ExecuteSqlCommand(sql, statusID, Convert.ToInt32(enmPaymentStatus.PaidButNotVerified), objdemandNote.ID);
                                //    //context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + statusID
                                //      //  + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.PaidButNotVerified) + " where Demand_Note_ID = " + objdemandNote.ID);
                                //}
                                //Call recon log api 
                                //Added 10 Sep 2020
                                /*string dd = DateTime.Now.ToString("yyyy-MM-dd");
                                string logFile = Convert.ToString(dd + "_12.txt");
                                CommonFunctions.createLog(logFile, "Creating Response");*/
                                //

                                BridgePGUtil obj = new BridgePGUtil();
                                string value = obj.recon_log(merchant_id, merchant_txn, csc_txn, csc_id, product_id, txn_amount, merchant_txn_date_time, "S", merchant_receipt_no);

                                //Added 10 Sep 2020
                               /* dd = DateTime.Now.ToString("yyyy-MM-dd");
                                logFile = Convert.ToString(dd + "_17.txt");
                                CommonFunctions.createLog(logFile, value);*/
                                //							
                                Insert_Recon_Response(value);
                                //-------------------


                                //Updation of database after getting data from merchant
                                if (objdemandNote.enmApplicationType == enmApplicationType.CertificateExamApplication)
                                {
                                    if (objdemandNote.enmDemandNoteType == enmDemandNoteType.Single)
                                        statusID = Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre);
                                    else
                                    {
                                        if (context.CertificateExamApplications.Where(s => s.DemandNoteID.Value == objdemandNote.ID).FirstOrDefault().Exam.IsDispatchable)
                                            statusID = Convert.ToInt32(enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre);
                                        else
                                            statusID = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre);
                                    }

                                    //Updated to avoid SQL Injection
                                    string sql = "Update Certificate_Exam_Application set Application_Status_ID = {0}, Payment_Status_ID = {1} where Demand_Note_ID = {2}";

                                    context.Database.ExecuteSqlCommand(sql, statusID, Convert.ToInt32(enmPaymentStatus.PaidButNotVerified), objdemandNote.ID);
                                    //context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = " + statusID
                                    //  + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.PaidButNotVerified) + " where Demand_Note_ID = " + objdemandNote.ID);
                                }
                                else if (objdemandNote.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                                {
                                    if (objdemandNote.enmDemandNoteType == enmDemandNoteType.Single)
                                    {
                                        Int32 applicantType = (from c in context.CourseRegistrationApplications
                                                               where c.DemandNoteID == objdemandNote.ID
                                                               select c.ApplicantTypeID).FirstOrDefault();
                                        if ((enmApplicantType)applicantType == enmApplicantType.Institute)
                                            statusID = Convert.ToInt32(enmCourseApplicationStatus.AppliedButPendingForInstituteVerification);
                                        else
                                            statusID = Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
                                    }
                                    else
                                        statusID = Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);

                                    //Updated to avoid SqlInjection
                                    string sql = "Update Course_Registration_Application set Application_Status_ID = {0}, Payment_Status_ID = {1} where Demand_Note_ID = {2}";
                                    context.Database.ExecuteSqlCommand(sql, statusID, Convert.ToInt32(enmPaymentStatus.PaidButNotVerified), objdemandNote.ID);
                                    //  context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + statusID
                                    //    + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.PaidButNotVerified) + " where Demand_Note_ID = " + objdemandNote.ID);
                                }
                                else if (objdemandNote.enmApplicationType == enmApplicationType.CourseExamApplication)
                                {

                                    if (objdemandNote.enmDemandNoteType == enmDemandNoteType.Single)
                                    {
                                        Int32 applicantType = (from c in context.CourseExamApplications
                                                               where c.DemandNoteID == objdemandNote.ID
                                                               select c.ApplicantTypeID).FirstOrDefault();
                                        if ((enmApplicantType)applicantType == enmApplicantType.Institute)
                                            statusID = Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification);
                                        else
                                            statusID = Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
                                    }
                                    else
                                        statusID = Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);

                                    string sql = "Update Course_Exam_Application set Application_Status_ID ={0}, Payment_Status_ID = (1) where Demand_Note_ID = {2}";
                                    context.Database.ExecuteSqlCommand(sql, statusID, Convert.ToInt32(enmPaymentStatus.PaidButNotVerified), objdemandNote.ID);
                                    //context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + statusID
                                    //  + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.PaidButNotVerified) + " where Demand_Note_ID = " + objdemandNote.ID);
                                }
                            }
                            else//transaction failed
                            {
                                divResponse.Visible = true;
                                lblError.Visible = true;
                                lblError.Text += "Your CSC SPV transaction could not be completed. Reason: " + objtransaction.ResponseMessage + "<br>Please note below details for reference.";
                            }
                        }
                        else
                        {
                            divResponse.Visible = true;
                            lblError.Visible = true;
                            lblError.Text += "Your CSC SPV transaction could not be completed. Reason: Failed " + "<br>Please note below details for reference.";
                            // lblError.Text += "Your CSC SPV transaction could not be completed. Reason: " + txn_status_message + "<br>Please note below details for reference.";
                        }
                        context.SaveChanges();
						//Added 10 Sept 2020
               /* string dd1 = DateTime.Now.ToString("yyyy-MM-dd");

                string logFile1 = Convert.ToString(dd1 + "_17a.txt");

                CommonFunctions.createLog(logFile1, "Complete");*/
                
                //
                       
						
						//Added 10 Sept 2020
                /* dd1 = DateTime.Now.ToString("yyyy-MM-dd");

                 logFile1 = Convert.ToString(dd1 + "_17b.txt");

                CommonFunctions.createLog(logFile1, merchant_txn);*/
                
                //
                        ShowSuccessMessage(Convert.ToInt32(merchant_txn));
					 scope.Complete();
                    };
                }
            }
            else
            {
                divResponse.Visible = true;
                throw new Exception("Your online transaction could not be completed. Reason: Invalid request/No response received from online payment gateway. Please try again.");
            }
        }
        catch (Exception ex)
        {
            lblError.Text += ex.Message;
            lblError.Visible = true;
            imPrint.Visible = false;
        }
    }
    protected void ShowSuccessMessage(Int32 csdTransactionID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
				//Added 10 Sept 2020
               /* string dd = DateTime.Now.ToString("yyyy-MM-dd");

                string logFile = Convert.ToString(dd + "_18a.txt");

                CommonFunctions.createLog(logFile,"Started succes method");*/
                
                //
                CSCTransaction ot = context.CSCTransactions.Find(csdTransactionID);
				
				//Added 10 Sept 2020
                 /*dd = DateTime.Now.ToString("yyyy-MM-dd");

                 logFile = Convert.ToString(dd + "_18b.txt");

                CommonFunctions.createLog(logFile, ot.DemandNoteID.ToString());*/
                //CommonFunctions.createLog(logFile, demandNote.enmPaymentMode.ToString());
                //
                DemandNote demandNote = context.DemandNotes.Find(ot.DemandNoteID);

                //Added 10 Sept 2020
                /* dd = DateTime.Now.ToString("yyyy-MM-dd");

                 logFile = Convert.ToString(dd + "_18.txt");

                CommonFunctions.createLog(logFile, ot.DemandNoteID.ToString());
                CommonFunctions.createLog(logFile, demandNote.enmPaymentMode.ToString());*/
                //

                if (demandNote != null)
                {
                    if (demandNote.enmPaymentMode == enmPaymentMode.CSCSPV)
                    {
                        tdDemandNumber.InnerHtml = demandNote.ID.ToString() + " <i>Dated:</i> " + demandNote.ApplicationDate.ToString("dd-MMM-yyyy");
                        tdMode.InnerText = EConnect.Utils.Common.EnumUtility.GetDescription(demandNote.enmPaymentMode);
                        tdRefNo.InnerText = ot.ResponseTransactionNumber;
                        tdRefDate.InnerText = ot.Date.ToString("dd-MMM-yyyy hh:mm tt");
                        tdAmount.InnerHtml = ot.Amount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(ot.Amount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                        tdPaymentDescription.InnerHtml = "";
                        tdStatus.InnerText = ot.ResponseMessage;
                        if (demandNote.enmDemandNoteType == enmDemandNoteType.Single)
                        {
                            //Added 10 Sept 2020
                           /*  dd = DateTime.Now.ToString("yyyy-MM-dd");

                             logFile = Convert.ToString(dd + "_19.txt");


                            CommonFunctions.createLog(logFile, demandNote.enmApplicationType.ToString());*/
                            //

                            trextra.Visible = false;
                            if (demandNote.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                            {
                                tdPaymentDescription.InnerText = "Registration Fee ";
                                var payee = (from p in context.CourseRegistrationApplications
                                             where p.DemandNoteID == demandNote.ID
                                             select new { ApplNo = p.Number, ApplDate = p.ApplicationDate, FeeDetail = p.Course.Name + " (" + p.Course.Code + ")", Name = p.Salutation + " " + p.Name, FatherName = p.FatherName, MotherName = p.MotherName, DOB = p.DateOfBirth, p.GuardianName, sourceid = p.ApplicationSourceID }).FirstOrDefault();
                                if (payee != null)
                                {
                                    Int32 applicationtypeid = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                                    Int32 activityid = 0;
                                    Int32 feeamount = 0;
                                    if (payee.sourceid == Convert.ToInt32(enmApplicationSource.Candidate))
                                    {
                                        activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                        feeamount = GetFeeAmount(context, applicationtypeid, activityid);
                                        tdcscamount.InnerHtml = feeamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                    }
                                    else if (payee.sourceid == Convert.ToInt32(enmApplicationSource.CSC))
                                    {
                                        activityid = Convert.ToInt32(enmCSCActivity.FillFormandDepositFee);
                                        feeamount = GetFeeAmount(context, applicationtypeid, activityid);
                                        tdcscamount.InnerHtml = feeamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                    }
                                    tdPayeeName.InnerText = payee.Name;
                                    if (payee.GuardianName != null)
                                        tdPayeeFatherName.InnerText = payee.GuardianName + " (Guarddian)";
                                    else
                                    {
                                        if (payee.FatherName != null)
                                            tdPayeeFatherName.InnerText = "Mr. " + payee.FatherName;
                                        if (payee.MotherName != null)
                                            tdPayeeMotherName.InnerText = "Mrs. " + payee.MotherName;
                                    }

                                    //Added 10 Sept 2020
                                   /* dd = DateTime.Now.ToString("yyyy-MM-dd");

                                    logFile = Convert.ToString(dd + "_20.txt");

                                    CommonFunctions.createLog(logFile, demandNote.ID.ToString());
                                    CommonFunctions.createLog(logFile, feeamount.ToString());
                                    CommonFunctions.createLog(logFile, tdPayeeFatherName.InnerText);*/
                                    //
                                    tdPaymentDescription.InnerText = "Registration Fee: " + payee.FeeDetail;
                                    tdPaymentDescription.InnerHtml += "<br>(Application Number: " + payee.ApplNo.ToString() + "  Dated: " + payee.ApplDate.ToString("dd-MMM-yyyy") + ")";
                                    Int32 totalamount = Convert.ToInt32(ot.Amount + feeamount);
                                    tdtotalamount.InnerHtml = totalamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(totalamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                }
                                else
                                {
                                    tdPayeeName.InnerText = "NA";
                                    tdPayeeFatherName.InnerText = "NA";
                                    tdPayeeMotherName.InnerText = "NA";
                                    tdPaymentDescription.InnerText = "NA";
                                }
                                trExam.Visible = false;
                                trReg.Visible = true;

                                
                            }
                            else if (demandNote.enmApplicationType == enmApplicationType.CertificateExamApplication)
                            {
                                var payee = (from p in context.CertificateExamApplications
                                             where p.DemandNoteID == demandNote.ID
                                             select new { ApplNo = p.Number, ApplDate = p.ApplicationDate, FeeDetail = p.Course.Name + " (" + p.Course.Code + ")", Name = p.Salutation + " " + p.Name, FatherName = p.FatherName, MotherName = p.MotherName, DOB = p.DateOfBirth, p.GuardianName, sourceid = p.ApplicationSourceID }).FirstOrDefault();
                                if (payee != null)
                                {
                                    Int32 applicationtypeid = Convert.ToInt32(enmApplicationType.CertificateExamApplication);
                                    Int32 activityid = 0;
                                    Int32 feeamount = 0;
                                    if (payee.sourceid == Convert.ToInt32(enmApplicationSource.Candidate))
                                    {
                                        activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                        feeamount = GetFeeAmount(context, applicationtypeid, activityid);
                                        tdcscamount.InnerHtml = feeamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                    }
                                    else if (payee.sourceid == Convert.ToInt32(enmApplicationSource.CSC))
                                    {
                                        activityid = Convert.ToInt32(enmCSCActivity.FillFormandDepositFee);
                                        feeamount = GetFeeAmount(context, applicationtypeid, activityid);
                                        tdcscamount.InnerHtml = feeamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                    }
                                    tdPayeeName.InnerText = payee.Name;
                                    if (payee.GuardianName != null)
                                        tdPayeeFatherName.InnerText = payee.GuardianName + " (Guarddian)";
                                    else
                                    {
                                        if (payee.FatherName != null)
                                            tdPayeeFatherName.InnerText = "Mr. " + payee.FatherName;
                                        if (payee.MotherName != null)
                                            tdPayeeMotherName.InnerText = "Mrs. " + payee.MotherName;
                                    }
                                    tdPaymentDescription.InnerText = "Registration Cum Examination Fee: " + payee.FeeDetail;
                                    tdPaymentDescription.InnerHtml += "<br>Application Number: " + payee.ApplNo.ToString() + "  Dated: " + payee.ApplDate.ToString("dd-MMM-yyyy");
                                    Int32 totalamount = Convert.ToInt32(ot.Amount + feeamount);
                                    tdtotalamount.InnerHtml = totalamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(totalamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                    //Added 10 Sept 2020
                                  /*  dd = DateTime.Now.ToString("yyyy-MM-dd");

                                    logFile = Convert.ToString(dd + "_22.txt");


                                    CommonFunctions.createLog(logFile, demandNote.ID.ToString());
                                    CommonFunctions.createLog(logFile, feeamount.ToString());
                                    CommonFunctions.createLog(logFile, tdPayeeFatherName.InnerText);*/
                                    //
                                }
                                else
                                {
                                    tdPayeeName.InnerText = "NA";
                                    tdPayeeFatherName.InnerText = "NA";
                                    tdPayeeMotherName.InnerText = "NA";
                                    tdPaymentDescription.InnerText = "NA";
                                }
                                if (context.CertificateExamApplications.Where(t => t.DemandNoteID.Value == demandNote.ID).FirstOrDefault().Exam.IsBatchProcessable)
                                    trExam.Visible = true;
                                else
                                    trExam.Visible = false;

                                trReg.Visible = false;

                               

                            }
                            else if (demandNote.enmApplicationType == enmApplicationType.CourseExamApplication)
                            {
                                var payee = (from p in context.CourseExamApplications
                                             join c in context.Candidates on p.CandidateID equals c.ID
                                             where p.DemandNoteID == demandNote.ID
                                             select new { ApplNo = p.Number, ApplDate = p.ApplicationDate, FeeDetail = p.Course.Name + " (" + p.Course.Code + ")", Name = c.Salutation + " " + c.Name, FatherName = c.FatherName, MotherName = c.MotherName, DOB = c.DateOfBirth, c.GuardianName }).FirstOrDefault();
                                if (payee != null)
                                {
                                    Int32 applicationtypeid = Convert.ToInt32(enmApplicationType.CourseExamApplication);
                                    Int32 activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                    Int32 feeamount = GetFeeAmount(context, applicationtypeid, activityid);
                                    tdcscamount.InnerHtml = GetFeeAmount(context, applicationtypeid, activityid).ToString() + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                    tdPayeeName.InnerText = payee.Name;
                                    //Added 10 Sept 2020
                                   /* dd = DateTime.Now.ToString("yyyy-MM-dd");

                                    logFile = Convert.ToString(dd + "_23.txt");


                                    CommonFunctions.createLog(logFile, demandNote.ID.ToString());
                                    CommonFunctions.createLog(logFile, feeamount.ToString());
                                    CommonFunctions.createLog(logFile, tdPayeeFatherName.InnerText);*/
                                    //
                                    if (payee.GuardianName != null)
                                        tdPayeeFatherName.InnerText = payee.GuardianName + " (Guarddian)";
                                    else
                                    {
                                        if (payee.FatherName != null)
                                            tdPayeeFatherName.InnerText = "Mr. " + payee.FatherName;
                                        if (payee.MotherName != null)
                                            tdPayeeMotherName.InnerText = "Mrs. " + payee.MotherName;
                                    }
                                    tdPaymentDescription.InnerText = "Examination Fee: " + payee.FeeDetail;
                                    tdPaymentDescription.InnerHtml += "<br>Application Number: " + payee.ApplNo.ToString() + "  Dated: " + payee.ApplDate.ToString("dd-MMM-yyyy");
                                    Int32 totalamount = Convert.ToInt32(ot.Amount + feeamount);
                                    tdtotalamount.InnerHtml = totalamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(totalamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                }
                                else
                                {
                                    tdPayeeName.InnerText = "NA";
                                    tdPayeeFatherName.InnerText = "NA";
                                    tdPayeeMotherName.InnerText = "NA";
                                    tdPaymentDescription.InnerText = "NA";
                                }
                                
                            }
                        }
                        else
                        {
                            tdNameCaption.InnerText = "Name of Institute";
                            tdFnameCaption.InnerText = "Address Line 1";
                            tdMNameCaption.InnerText = "Address Line 2";
                            trextra.Visible = true;
                            tdDobCaption.InnerText = "Address Line 3";
                            if (demandNote.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                            {
                                Int32 feeamount = 0;
                                tdPaymentDescription.InnerText = "Registration Fee ";
                                var payee = (from p in context.CourseRegistrationApplications
                                             where p.DemandNoteID == demandNote.ID
                                             select new
                                             {
                                                 FeeDetail = p.Course.Name + " (" + p.Course.Code + ")",
                                                 Name = p.Institute.Name,
                                                 Address1 = p.Institute.AddressLine1 + " " + p.Institute.AddressLine2,
                                                 Address2 = p.Institute.AddressLine3 + " " + p.Institute.CityName,
                                                 State = p.Institute.State.Name,
                                                 Pin = p.Institute.PinCode,
                                             }).FirstOrDefault();
                                if (payee != null)
                                {
                                    Int32 applicationtypeid = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                                    Int32 activityid = 0;
                                    activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                    feeamount = GetFeeAmount(context, applicationtypeid, activityid);
                                    tdcscamount.InnerHtml = GetFeeAmount(context, applicationtypeid, activityid).ToString() + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                }
                                tdPayeeName.InnerText = payee.Name.ToUpper();
                                if (payee.Address1 != null)
                                    tdPayeeFatherName.InnerText = payee.Address1.ToUpper();
                                if (payee.Address2 != null)
                                    tdPayeeMotherName.InnerText = payee.Address2.ToUpper();
                                if (payee.State != null)
                                    tdPayeeDOB.InnerText = payee.State.ToUpper();
                                if (payee.Pin.HasValue)
                                    tdPayeeDOB.InnerText += ", Pin: " + payee.Pin.Value.ToString();
                                tdPaymentDescription.InnerText = "Registration Fee: " + payee.FeeDetail;
                                Int32 totalamount = Convert.ToInt32(ot.Amount + feeamount);
                                tdtotalamount.InnerHtml = totalamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(totalamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                trExam.Visible = false;
                                trReg.Visible = true;

                                //Added 10 Sept 2020
                              /*   dd = DateTime.Now.ToString("yyyy-MM-dd");

                                 logFile = Convert.ToString(dd + "_24.txt");


                                CommonFunctions.createLog(logFile, demandNote.ID.ToString());
                                CommonFunctions.createLog(logFile, feeamount.ToString());
                                CommonFunctions.createLog(logFile, tdPayeeFatherName.InnerText);*/
                                //

                            }
                            else if (demandNote.enmApplicationType == enmApplicationType.CertificateExamApplication)
                            {
                                var payee = (from p in context.CertificateExamApplications
                                             where p.DemandNoteID == demandNote.ID
                                             select new
                                             {
                                                 FeeDetail = p.Course.Name + " (" + p.Course.Code + ")",
                                                 Name = p.Institute.Name,
                                                 Address1 = p.Institute.AddressLine1 + " " + p.Institute.AddressLine2,
                                                 Address2 = p.Institute.AddressLine3 + " " + p.Institute.CityName,
                                                 State = p.Institute.State.Name,
                                                 Pin = p.Institute.PinCode,
                                             }).FirstOrDefault();
                                if (payee != null)
                                {
                                    Int32 applicationtypeid = Convert.ToInt32(enmApplicationType.CertificateExamApplication);
                                    Int32 activityid = 0;
                                    Int32 feeamount = 0;
                                    activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                    feeamount = GetFeeAmount(context, applicationtypeid, activityid);
                                    tdcscamount.InnerHtml = GetFeeAmount(context, applicationtypeid, activityid).ToString() + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                    tdPayeeName.InnerText = payee.Name.ToUpper();
                                    if (payee.Address1 != null)
                                        tdPayeeFatherName.InnerText = payee.Address1.ToUpper();
                                    if (payee.Address2 != null)
                                        tdPayeeMotherName.InnerText = payee.Address2.ToUpper();
                                    if (payee.State != null)
                                        tdPayeeDOB.InnerText = payee.State.ToUpper();
                                    if (payee.Pin.HasValue)
                                        tdPayeeDOB.InnerText += ", Pin: " + payee.Pin.Value.ToString();
                                    tdPaymentDescription.InnerText = "Examination Fee: " + payee.FeeDetail;
                                    Int32 totalamount = Convert.ToInt32(ot.Amount + feeamount);
                                    tdtotalamount.InnerHtml = totalamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(totalamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                    //Added 10 Sept 2020
                                    /*dd = DateTime.Now.ToString("yyyy-MM-dd");

                                    logFile = Convert.ToString(dd + "_25.txt");


                                    CommonFunctions.createLog(logFile, demandNote.ID.ToString());
                                    CommonFunctions.createLog(logFile, feeamount.ToString());
                                    CommonFunctions.createLog(logFile, tdPayeeFatherName.InnerText);*/
                                    //
                                }
                                else
                                {
                                    tdPayeeName.InnerText = "";
                                    tdPayeeFatherName.InnerText = "";
                                    tdPayeeMotherName.InnerText = "";
                                    tdPayeeDOB.InnerText = "";
                                    tdPaymentDescription.InnerText = "";
                                }
                                if (context.CertificateExamApplications.Where(t => t.DemandNoteID.Value == demandNote.ID).FirstOrDefault().Exam.IsBatchProcessable)
                                    trExam.Visible = true;
                                else
                                    trExam.Visible = false;

                                trReg.Visible = false;
                                
                            }
                            else if (demandNote.enmApplicationType == enmApplicationType.CourseExamApplication)
                            {
                                var payee = (from p in context.CourseExamApplications
                                             where p.DemandNoteID == demandNote.ID
                                             select new
                                             {
                                                 FeeDetail = p.Exam.Name + " (" + p.Course.Name + ")",
                                                 Name = p.Institute.Name,
                                                 Address1 = p.Institute.AddressLine1 + " " + p.Institute.AddressLine2,
                                                 Address2 = p.Institute.AddressLine3 + " " + p.Institute.CityName,
                                                 State = p.Institute.State.Name,
                                                 Pin = p.Institute.PinCode
                                             }).FirstOrDefault();
                                if (payee != null)
                                {
                                    Int32 applicationtypeid = Convert.ToInt32(enmApplicationType.CourseExamApplication);
                                    Int32 activityid = Convert.ToInt32(enmCSCActivity.DepositFee);
                                    Int32 feeamount = GetFeeAmount(context, applicationtypeid, activityid);
                                    tdcscamount.InnerHtml = GetFeeAmount(context, applicationtypeid, activityid).ToString() + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(feeamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                    tdPayeeName.InnerText = payee.Name.ToUpper();
                                    if (payee.Address1 != null)
                                        tdPayeeFatherName.InnerText = payee.Address1.ToUpper();
                                    if (payee.Address2 != null)
                                        tdPayeeMotherName.InnerText = payee.Address2.ToUpper();
                                    if (payee.State != null)
                                        tdPayeeDOB.InnerText = payee.State.ToUpper();
                                    if (payee.Pin.HasValue)
                                        tdPayeeDOB.InnerText += ", Pin: " + payee.Pin.Value.ToString();
                                    tdPaymentDescription.InnerText = "Examination Fee: " + payee.FeeDetail;
                                    Int32 totalamount = Convert.ToInt32(ot.Amount + feeamount);
                                    tdtotalamount.InnerHtml = totalamount.ToString("F") + " <i> (" + EConnect.Utils.Conversion.ConversionUtility.NumberToText(totalamount.ToString(), EConnect.Utils.Conversion.ConversionType.IndianRupees, enmLanguage.English) + ")</i>";
                                    //Added 10 Sept 2020
                                   /* dd = DateTime.Now.ToString("yyyy-MM-dd");

                                    logFile = Convert.ToString(dd + "_26.txt");

                                    CommonFunctions.createLog(logFile, demandNote.ID.ToString());
                                    CommonFunctions.createLog(logFile, feeamount.ToString());
                                    CommonFunctions.createLog(logFile, tdPayeeFatherName.InnerText);*/
                                    //
                                }
                                else
                                {
                                    tdPayeeName.InnerText = "NA";
                                    tdPayeeFatherName.InnerText = "NA";
                                    tdPayeeMotherName.InnerText = "NA";
                                    tdPayeeDOB.InnerText = "NA";
                                    tdPaymentDescription.InnerText = "NA";
                                }
                               
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            
			//Added 10 Sept 2020
              /*  string dd = DateTime.Now.ToString("yyyy-MM-dd");

                string logFile = Convert.ToString(dd + "_18ex.txt");

                CommonFunctions.createLog(logFile,ex.InnerException.ToString ());*/
                
                //
				throw ex;
        }
    }
    protected void btnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("nieletpaymentservices.aspx");
    }
    protected Int32 GetFeeAmount(EConnectContext context, Int32 applicationtypeid, Int32 activityid)
    {
        try
        {

            var feeamount = (from d in context.CSCCharges
                             where d.ApplicationTypeID == applicationtypeid &&
                             d.ActivityID == activityid
                             select d.Amount).FirstOrDefault();
            if (feeamount > 0)
                return feeamount;
            else
                return 0;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }
    private bool Insert_Recon_Response(string str)
    {
        string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = null;
        con = new SqlConnection(CS);
        SqlCommand cmd = null;
        cmd = con.CreateCommand();
        SqlTransaction trans = null;
        string strmerchant_id = string.Empty; ;
        string strmerchant_txn = string.Empty;
        string strmerchant_txn_status = string.Empty; ;
        string strcsc_txn = string.Empty;
        string strrecon_reference = string.Empty;
        string strrecon_log_status = string.Empty;


        try
        {
            string[] arr = str.Split("|".ToCharArray());
            for (int i = 0; i < arr.Length; i++)
            {
                string[] arr2 = arr[i].Split("=".ToCharArray());
                if (arr2[0] == "merchant_id") strmerchant_id = arr2[1];
                if (arr2[0] == "merchant_txn") strmerchant_txn = arr2[1];
                if (arr2[0] == "merchant_txn_status") strmerchant_txn_status = arr2[1];
                if (arr2[0] == "csc_txn") strcsc_txn = arr2[1];
                if (arr2[0] == "recon_reference") strrecon_reference = arr2[1];
                if (arr2[0] == "recon_log_status") strrecon_log_status = arr2[1];

            }
            con.Open();
            trans = con.BeginTransaction();
            cmd.Transaction = trans;
            cmd.CommandText = "[Insert_Recon_Response]";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@merchant_id", SqlDbType.VarChar).Value = strmerchant_id;
            cmd.Parameters.AddWithValue("@merchant_txn", SqlDbType.VarChar).Value = strmerchant_txn;
            cmd.Parameters.AddWithValue("@merchant_txn_status", SqlDbType.DateTime).Value = strmerchant_txn_status;
            cmd.Parameters.AddWithValue("@csc_txn", SqlDbType.VarChar).Value = strcsc_txn;
            cmd.Parameters.AddWithValue("@recon_reference", SqlDbType.VarChar).Value = strrecon_reference;
            cmd.Parameters.AddWithValue("@recon_log_status", SqlDbType.VarChar).Value = strrecon_log_status;

            cmd.ExecuteNonQuery();
            trans.Commit();
            return true;

        }
        catch (Exception ee)
        {
            trans.Rollback();
            string msg = ee.Message;
        }
        finally
        {
            trans.Dispose();
            con.Dispose();
            con.Close();
        }
        return false;

    }
}