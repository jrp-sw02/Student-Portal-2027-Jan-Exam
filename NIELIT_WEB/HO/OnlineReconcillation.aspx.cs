using System;
using System.Data.OleDb;
using System.Data.SqlClient;                             //November_2024
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

public partial class HO_OnlineReconcillation : BasePage
{
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                bindpaymentmode();
                ddlpaymentmode.Items.RemoveAt(1); // temporary
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Online Transaction Settlement", "", ""));
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void bindpaymentmode()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 multicheque = Convert.ToInt32(enmPaymentMode.MultiCityCheque);
                Int32 cash = Convert.ToInt32(enmPaymentMode.Cash);
                Int32 DemandDraft = Convert.ToInt32(enmPaymentMode.DemandDraft);
                Int32 NEFTRTGS = Convert.ToInt32(enmPaymentMode.NEFTRTGS);
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.PaymentModes
                               where p.ID != multicheque && p.ID != cash && p.ID != DemandDraft && p.ID != NEFTRTGS && p.ID !=7
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlpaymentmode, Category, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlpaymentmode.SelectedValue = "0";
            divValidateData.Visible = false;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            divValidateData.Visible = true;
            BreadCrumb1.Render();
            StringBuilder FaildRecords = new StringBuilder();
            FaildRecords.Append("Failed TransactionIDs:-");
            int failedRecordCount = 0;
            int ValidateRecords = 0;
            int TotalRecords = 0;
            string filepath = Server.MapPath("../UploadedFiles");
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + flUpload.FileName;
            flUpload.SaveAs(filepath + "/" + filename);
            string path = (filepath + "/" + filename);
            string ext = System.IO.Path.GetExtension(this.flUpload.PostedFile.FileName);
            string sExcelConnectionString = "";
            if (ext.ToUpper() == ".XLS")
                sExcelConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";", path);
            else if (ext.ToUpper() == ".XLSX")
                sExcelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=No;IMEX=1\";", path);
            else
            {
                ShowAlert("Please Choose ..XLS/.XLSX Excel File.", true);
                return;
            }
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = sExcelConnectionString;
            connection.Open();
            OleDbCommand command = new OleDbCommand("select * from [Pending Settlemenmt Transaction$]", connection);
            OleDbDataReader dr = command.ExecuteReader();
            Int64 transid = 0;
            try
            {

                using (EConnectContext context = new EConnectContext())
                {
                    Int32 paymodeid = Convert.ToInt32(ddlpaymentmode.SelectedValue);
                    enmPaymentMode paymentMode = (enmPaymentMode)paymodeid;
                    while (dr.Read())
                    {
                        if (paymentMode == enmPaymentMode.Online)
                        {
                            if (CommonFunctions.IsNumeric(dr[4].ToString()))
                            {
                                transid = Convert.ToInt64(dr[4]);
                                TotalRecords = TotalRecords + 1;
                            }
                            else
                            {
                                continue;
                            }
                            using (TransactionScope scope = new TransactionScope())
                            {
                                var online = (from r in context.OnlineTransaction
                                              where r.ID == transid
                                              select r).FirstOrDefault();

                                if (online != null && online.ResponseStatusCode != "0300" && online.ResponseStatusMessage != "Success" && !online.IsSettled)
                                {
                                    try
                                    {
                                        DemandNote demandNote = context.DemandNotes.Find(online.DemandNoteID);
                                        if (demandNote != null)
                                        {
                                            Int32 coursexamcount = context.CourseExamApplications.Where(s => s.DemandNoteID == demandNote.ID).Count();
                                            Int32 certificateexamcount = context.CertificateExamApplications.Where(s => s.DemandNoteID == demandNote.ID).Count();
                                            Int32 coursregcount = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandNote.ID).Count();

                                            if (demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending) && (coursexamcount > 0 || coursregcount > 0 || certificateexamcount > 0))
                                            {
                                                var trans = (from r in context.OnlineTransaction
                                                             where r.DemandNoteID == demandNote.ID && r.ResponseStatusCode == "0300" && r.ResponseStatusMessage == "Success"
                                                             select r);
                                                if (trans.Count() <= 0)
                                                {
                                                    string[] date = dr[8].ToString().Substring(0, 10).Split('/');
                                                    DateTime date1 = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));
                                                    online.ResponseDate = date1;
                                                    online.ReferenceNumber = Convert.ToString(dr[3]).Trim();
                                                    online.ResponseStatusCode = "0300";
                                                    online.ResponseStatusMessage = "Success";
                                                    online.IsSettled = true;
                                                    online.SettledBy = Convert.ToInt32(Session["UserID"]);
                                                    online.SettledOn = DateTime.Now;
                                                    online.SettledFileName = filename;

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

                                                        //context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = " + statusID
                                                        //    + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " where Demand_Note_ID = " + objdemandnote.ID);

                                                        //November_2024
                                                        SqlParameter[] param1 = { new SqlParameter("@statusID", statusID),
                                                                                        new SqlParameter("@paymentStatus", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                                            new SqlParameter("@demandNoteID",  objdemandnote.ID)
                                                                                                    };
                                                        context.Database.ExecuteSqlCommand("Update Certificate_Exam_Application set Application_Status_ID = @statusID "
                                                                                        + ", Payment_Status_ID = @paymentStatus where Demand_Note_ID = @demandNoteID " , param1);
                                                    }
                                                    else if (objdemandnote.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                                                    {
                                                        if (objdemandnote.enmDemandNoteType == enmDemandNoteType.Single)
                                                            statusID = Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
                                                        else
                                                            statusID = Convert.ToInt32(enmCourseApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);

                                                        //context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = " + statusID
                                                        //    + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " where Demand_Note_ID = " + objdemandnote.ID);

                                                        //November_2024
                                                        SqlParameter[] param2 = { new SqlParameter("@statusID", statusID),
                                                                                        new SqlParameter("@paymentStatus", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                                            new SqlParameter("@demandNoteID",  objdemandnote.ID)    
                                                                                                    };
                                                        context.Database.ExecuteSqlCommand("Update Course_Registration_Application set Application_Status_ID = @statusID "
                                                                                            + ", Payment_Status_ID = @paymentStatus where Demand_Note_ID = @demandNoteID ", param2);

                                                    }
                                                    else if (objdemandnote.enmApplicationType == enmApplicationType.CourseExamApplication)
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
                                                            statusID = Convert.ToInt32(enmCourseExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToNIELIT);

                                                        //context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = " + statusID
                                                        //    + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " where Demand_Note_ID = " + objdemandnote.ID);

                                                        //November_2024
                                                        SqlParameter[] param3 = { new SqlParameter("@statusID", statusID),
                                                                                        new SqlParameter("@paymentStatus", Convert.ToInt32(enmPaymentStatus.Paid)),
                                                                                            new SqlParameter("@demandNoteID",  objdemandnote.ID)
                                                                                                    };
                                                        context.Database.ExecuteSqlCommand("Update Course_Exam_Application set Application_Status_ID = @statusID "
                                                                                            + ", Payment_Status_ID = @paymentStatus where Demand_Note_ID = @demandNoteID ", param3);
                                                    }
                                                    context.SaveChanges();
                                                    ValidateRecords = ValidateRecords + 1;
                                                }
                                            }
                                            else
                                            {
                                                FaildRecords.Append(transid.ToString() + ",");
                                                failedRecordCount++;
                                                if (failedRecordCount % 10 == 0 && failedRecordCount > 0)
                                                    FaildRecords.Append(WebUtility.HtmlDecode("<br/>"));
                                            }
                                        }
                                        else
                                        {
                                            FaildRecords.Append(transid.ToString() + ",");
                                            failedRecordCount++;
                                            if (failedRecordCount % 10 == 0 && failedRecordCount > 0)
                                                FaildRecords.Append(WebUtility.HtmlDecode("<br/>"));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        FaildRecords.Append(transid.ToString() + ",");
                                        failedRecordCount++;
                                        if (failedRecordCount % 10 == 0 && failedRecordCount > 0)
                                            FaildRecords.Append(WebUtility.HtmlDecode("<br/>"));
                                    }
                                }
                                else
                                {
                                    FaildRecords.Append(transid.ToString() + ",");
                                    failedRecordCount++;
                                    if (failedRecordCount % 10 == 0 && failedRecordCount > 0)
                                        FaildRecords.Append(WebUtility.HtmlDecode("<br/>"));
                                }
                                scope.Complete();
                            };
                        }
                    }
                };
                failedRecordCount = TotalRecords - ValidateRecords;
                lblTotalRecords.Text = TotalRecords.ToString();
                lblValidateRecords.Text = ValidateRecords.ToString();
                lblFailedRecords.Text = failedRecordCount.ToString() + WebUtility.HtmlDecode("<br/>") + FaildRecords.ToString().TrimEnd(',');
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
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}