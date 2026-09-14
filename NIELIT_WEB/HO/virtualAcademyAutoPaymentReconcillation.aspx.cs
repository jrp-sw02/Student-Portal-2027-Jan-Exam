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
//
using System.Configuration;
using System.Data.SqlClient;

public partial class HO_virtualAcademyAutoPaymentReconcillation : BasePage
    {
    Int32 currentRoleId = 0;
    Int64 CourseRegAppID = 0;
    Int64 candidateID = 0;
    Int64 candidateIDs = 0;
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

    protected void Page_Load(object sender, EventArgs e)
        {
        try
            {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
                {
                //Response.Write("Sorry! You don't have rights  to view this page");
                //Response.End();
                }
            if (!Page.IsPostBack)
                {
                bindpaymentmode();
                ddlpaymentmode.Enabled = false;
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Virtual Academy Auto Payment Reconcillation", "", ""));
                }
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }

    protected void ProcessTransactionData()
        {
        try
            {
            BreadCrumb1.Render();
            int PaymentPaid = Convert.ToInt32(enmPaymentStatus.Paid);
            List<string> refernce3 = new List<string>();
            string[] arr = new string[10];
            arr[4] = "<font color='#A81FE9'>Incorrect Data for following Transaction-ID's </font>";
            arr[0] = "<font color='Red'> ChargeBack for following Transaction-ID's </font> ";
            arr[1] = "<font color='#E214B9'> Already Refunded for following Transaction-ID's </font>";
            arr[2] = "<font color='Green'> Status other than \"Paid not verified\" / Already Reconcilled for following Transaction-ID's </font>";
            arr[3] = "<font color='Chocolate'>No record found for following Transaction-ID's </font>";
            arr[5] = "<font color='SlateBlue'> Already Settled for following Transaction-ID's </font>";
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
                ShowAlert("Please Choose ..XLS/.XLSX Excel File.", true);
                return;
                }
            divValidateData.Visible = true;

            btnSave.Visible = false;
            RefundfileName = "NIELIT_Refund_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            hffilename.Value = RefundfileName;
            txtFilePath = Server.MapPath("~/Download/" + RefundfileName);
            hffilepath.Value = txtFilePath;
            if (System.IO.File.Exists(txtFilePath))
                System.IO.File.Delete(txtFilePath);
            System.IO.File.Copy(Server.MapPath("~/Download/Settlement.txt"), txtFilePath);
            stream = new FileStream(txtFilePath, FileMode.Open, FileAccess.ReadWrite);
            writer = new StreamWriter(stream);

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
            //else if (paymentMode == enmPaymentMode.CSCSPV)
            //{
            //    command = new OleDbCommand("select * from [NIELIT Transaction Report$]", connection);
            //}
            OleDbDataReader dr = command.ExecuteReader();
            Int32 transid = 0;
            try
                {
                //using (EConnectContext context = new EConnectContext())
                using (NIELITMISContext context = new NIELITMISContext())
                    {
                    // to use separate service-id for courses.
                    var ServiceID = (from r in context.VirtualAcademyCourseServiceIDs
                                     select new
                                     {
                                         RegistrationServiceID = r.RegistrationServiceID,
                                         //ExaminationServiceID = r.ExaminationServiceID,
                                         //CertificateServiceID = r.CertificateServiceID
                                     }).ToList();

                    foreach (var service in ServiceID)
                        {
                        refernce3.Add(service.RegistrationServiceID);
                        //refernce3.Add(service.ExaminationServiceID);
                        //refernce3.Add(service.CertificateServiceID);
                        }

                    while (dr.Read())
                        {

                        # region Online Payment Mode  ---------------  For VA
                        if (paymentMode == enmPaymentMode.Online)
                            {
                            if (CommonFunctions.IsNumeric(dr[7].ToString()))
                                {
                                totalRecordsCount = totalRecordsCount + 1;
                                if (!refernce3.Contains(dr[9].ToString()))
                                    {
                                    NotValidateRecords = NotValidateRecords + 1;
                                    appIdList4.Append(dr[7].ToString() + ",");
                                    if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                        appIdList4.Append(WebUtility.HtmlDecode("<br/>"));
                                    continue;
                                    }
                                else
                                    {
                                    transid = Convert.ToInt32(dr[7]);
                                    }
                                }
                            else
                                {
                                continue;
                                }
                            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(900)))
                                {
                                var online = (from r in context.VirtualAcademyOnlineTransaction
                                              join d in context.VirtualAcademyDemandNotes
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
                                    TxReferencenumber = dr[5].ToString();
                                    Productcode = dr[4].ToString();
                                    BankID = "";
                                    BankReferenceNo = "";
                                    TransactionDate = dr[11].ToString();
                                    if ((online.responseCode.Trim() != "0300" && online.ResponseStatusMessage != "Success") || (online.demannotePaymentstatus == Convert.ToInt32(enmPaymentStatus.Pending)))
                                        {
                                        if (IsRefundable(online.DemandNoteID, online.IsSettled))
                                            {
                                            try
                                                {
                                                AddInRefund(online.ID, TxReferencenumber, Productcode, BankID, BankReferenceNo, TransactionDate);
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
                                                SettleManually(online.ID, TxReferencenumber, Productcode, BankID, BankReferenceNo, TransactionDate);
                                                }
                                            catch
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
                                            if (online.responseCode.Trim() == "0300")
                                                {
                                                if (IsSuccessRefundable(online.ID))
                                                    {
                                                    try
                                                        {
                                                        AddInRefund(online.ID, TxReferencenumber, Productcode, BankID, BankReferenceNo, TransactionDate);
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
                                                    if (context.VirtualAcademyOnlineTransaction.Any(s => s.ID == online.ID && s.ReferenceNumber == online.ReferenceNumber && s.Amount == online.Amount && System.Data.Entity.DbFunctions.TruncateTime(s.RequestDate) == System.Data.Entity.DbFunctions.TruncateTime(online.RequestDate)))
                                                        {
                                                        VirtualAcademyDemandNote demandNote = context.VirtualAcademyDemandNotes.Find(online.DemandNoteID);
                                                        if (demandNote != null)
                                                            {
                                                            int coursregcount = context.virtualAcademyRegistration.Where(s => s.DemandNoteID == demandNote.ID).Count();
                                                            if (demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified) && coursregcount > 0)
                                                                {
                                                                reconcilledRecordsCount = reconcilledRecordsCount + 1;
                                                                demandNote.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                                                VirtualAcademyOnlineTransaction onlinetransaction = context.VirtualAcademyOnlineTransaction.Find(online.ID);
                                                                onlinetransaction.SettledOn = DateTime.Now;
                                                                onlinetransaction.IsSettled = true;
                                                                onlinetransaction.SettledBy = Convert.ToInt32(Session["UserID"]);
                                                                onlinetransaction.SettledFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + flUpload.FileName;

                                                                if (demandNote.ApplicationTypeID == Convert.ToInt32(EConnect.NIELIT.enmApplicationTypeVirtualAcademy.VirtualAcademyCourseRegistrationApplication))
                                                                    {
                                                                    Int32 RegistrationNumberAlloted = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                                                                    context.Database.ExecuteSqlCommand("Update [NIELITMIS].[dbo].[virtualAcademyRegistration] set Application_Status_ID = " + RegistrationNumberAlloted + ", Payment_Status_ID = " + PaymentPaid + " where Demand_Note_ID = " + demandNote.ID);
                                                                   context.SaveChanges();

                                                                   

                                                                    // to update the student details in NIELITCentreStudent and NIELITCentreStudentFeePaid
                                                                    # region Record insert into NIELITCentreStudent

                                                                    var applicationNcs = context.virtualAcademyRegistration.Where(c => c.DemandNoteID == demandNote.ID).FirstOrDefault();
                                                                  
                                                                    if (applicationNcs != null)
                                                                        {
                                                                        int NielitCentreStudentCount = context.NielitCentreStudent.Where(c => c.Number == applicationNcs.Number).Count();

                                                                        if (NielitCentreStudentCount == 0)
                                                                            {
                                                                            NielitCentreStudent ncs = new NielitCentreStudent();
                                                                            ncs.AadharNumber = Convert.ToInt64(EncryptDecrypt.DecryptString(applicationNcs.AadharNumber.ToString()));
                                                                            ncs.AadharVerfied = applicationNcs.AadharVerfied;
                                                                            ncs.affidavit_Date = applicationNcs.affidavitDate;
                                                                            ncs.affidavit_No = applicationNcs.affidavitNo;
                                                                            ncs.affidavit_Verified = applicationNcs.affidavitVerified;
                                                                            ncs.AlreadyQualified = applicationNcs.AlreadyQualified;
                                                                            ncs.AlreadyRegistered = applicationNcs.Already_Registered;
                                                                            int appTypeId = 0;
                                                                            appTypeId = Convert.ToInt32(applicationNcs.Applicant_Type_Id);
                                                                            if (appTypeId != 0) { ncs.ApplicantTypeID = appTypeId; }
                                                                            ncs.ApplicationDate = applicationNcs.ApplicationDate;
                                                                            ncs.batch_ID = applicationNcs.batchID;
                                                                            ncs.BodyMark = applicationNcs.BodyMark;
                                                                            ncs.CandidateID = applicationNcs.Candidate_ID;
                                                                            ncs.CastCategoryID = applicationNcs.CastCategoryID;
                                                                            ncs.certificate_Issue_Date = applicationNcs.certificateIssueDate;
                                                                            ncs.InstituteID = applicationNcs.centreID;
                                                                            //ncs.comapny_Address
                                                                            ncs.comapny_Address = applicationNcs.comapnyAddress;
                                                                            ncs.comapny_Address2 = applicationNcs.comapnyAddress2;
                                                                            ncs.comapny_Address3 = applicationNcs.comapnyAddress3;
                                                                            ncs.company_Name = applicationNcs.companyNameID;
                                                                            ncs.CompnyCityName = applicationNcs.comapnyCity_Name;
                                                                            ncs.compnyState_ID = applicationNcs.comapnyState_ID;
                                                                            ncs.compnyDistrict_ID = applicationNcs.comapnyDistrict_ID;
                                                                            ncs.CompnyPinCode = applicationNcs.comapnyPin_Code;
                                                                            //
                                                                            ncs.CorAddressLine1 = applicationNcs.CorAddressLine1;
                                                                            ncs.CorAddressLine2 = applicationNcs.CorAddressLine2;
                                                                            ncs.CorAddressLine3 = applicationNcs.CorAddressLine3;
                                                                            ncs.CorCityName = applicationNcs.CorCityName;
                                                                            ncs.CorDistrictID = applicationNcs.CorDistrictID;
                                                                            ncs.CorPinCode = applicationNcs.CorPinCode;
                                                                            ncs.CorStateID = applicationNcs.CorStateID;
                                                                            //
                                                                            ncs.CourseID = applicationNcs.CourseDurationID;
                                                                            ncs.DateOfBirth = applicationNcs.DateOfBirth;
                                                                            ncs.DateOfVerificationByInstitute = applicationNcs.DateOfVerificationByInstitute;
                                                                            ncs.EmailAddress = applicationNcs.EmailAddress;
                                                                            Int32 enterBy1 = 0;
                                                                            //enterBy1 = Convert.ToInt32(applicationNcs.enterBy);
                                                                            enterBy1 = Convert.ToInt32(Session["UserID"]);
                                                                            if (enterBy1 != 0) { ncs.enter_By = enterBy1; }
                                                                            ncs.enter_Date = DateTime.Now;
                                                                            ncs.ExperienceInYears = applicationNcs.ExperienceInYears;
                                                                            ncs.FatherName = applicationNcs.FatherName;
                                                                            ncs.FinalSubmissionDate = applicationNcs.FinalSubmissionDate;
                                                                            ncs.FinalSubmitted = applicationNcs.FinalSubmitted;
                                                                            ncs.Gender = applicationNcs.Gender;
                                                                            ncs.GuardianName = applicationNcs.GuardianName;
                                                                            ncs.Is_EWS = applicationNcs.Is_EWS;
                                                                            ncs.IsExServicemane = applicationNcs.IsExServicemane;
                                                                            ncs.IsHandicaped = applicationNcs.IsHandicaped;
                                                                            ncs.IsVerifiedByInstitute = applicationNcs.IsVerifiedByInstitute;
                                                                            ncs.MaritalStatusID = applicationNcs.MaritalStatusID;
                                                                            ncs.MobileNumber = applicationNcs.MobileNumber;
                                                                            ncs.MotherName = applicationNcs.MotherName;
                                                                            ncs.Name = applicationNcs.Name;
                                                                            ncs.Number = applicationNcs.Number;
                                                                            //
                                                                            ncs.PerAddressLine1 = applicationNcs.PerAddressLine1;
                                                                            ncs.PerAddressLine2 = applicationNcs.PerAddressLine2;
                                                                            ncs.PerAddressLine3 = applicationNcs.PerAddressLine3;
                                                                            ncs.PerCityName = applicationNcs.PerCityName;
                                                                            ncs.PerDistrictID = applicationNcs.PerDistrictID;
                                                                            ncs.PerStateID = applicationNcs.PerStateID;
                                                                            ncs.PerPinCode = applicationNcs.PerPinCode;
                                                                            ncs.PhoneNumber = applicationNcs.PhoneNumber;
                                                                            ncs.placementDate = applicationNcs.placementDate;
                                                                            int projectId1 = 0;
                                                                            projectId1 = Convert.ToInt32(applicationNcs.projectId);
                                                                            if (projectId1 != 0) { ncs.projectId = projectId1; }
                                                                            ncs.QualifiedCourseID = applicationNcs.QualifiedCourseID;
                                                                            ncs.QualifiedCoursePassingYear = applicationNcs.QualifiedCoursePassingYear;
                                                                            ncs.QualifiedCourseRegistrationNo = applicationNcs.QualifiedCourseRegistrationNo;
                                                                            ncs.RegisteredCourseID = applicationNcs.Registered_Course_ID;
                                                                            ncs.RegisteredCourseRegistrationNo = applicationNcs.Registered_Course_Registration_No;
                                                                            ncs.ReligionID = applicationNcs.ReligionID;
                                                                            ncs.Salutation = applicationNcs.Salutation;
                                                                            ncs.StdNumber = applicationNcs.StdNumber;
                                                                            ncs.UIDNumber = applicationNcs.UIDNumber;
                                                                            ncs.UIDType = applicationNcs.UIDType;
                                                                            ncs.whether_Certificate_Issued = applicationNcs.whetherCertificateIssued;
                                                                            ncs.whether_Course_Complete = applicationNcs.whetherCourseComplete;
                                                                            ncs.whetherPlaced = applicationNcs.whetherPlaced;
                                                                            ncs.whetherProjectStudent = applicationNcs.whetherProjectStudent;
                                                                            // To save NIELIT Centre Student
                                                                            context.NielitCentreStudent.Add(ncs);
                                                                            context.SaveChanges();
                                                                            }
                                                                        }
                                                                    # endregion

                                                                    # region Record insert into NIELITCentreStudentFeePaid

                                                                    var application2 = context.virtualAcademyRegistration.Where(c => c.DemandNoteID == demandNote.ID).FirstOrDefault();
                                                                    if (application2 != null)
                                                                        {
                                                                        string number = application2.Number;
                                                                        var NIELITStudentFeePaid1 = context.NielitCentreStudent.Where(i => i.CourseID == application2.CourseDurationID && i.batch_ID == application2.batchID && i.Number == number).FirstOrDefault();
                                                                        if (NIELITStudentFeePaid1 != null)
                                                                            {
                                                                            string batchdetailsfeetype = "select batchID,feeTypeId, feeAmount FROM [NIELITMIS].[dbo].[NielitCentreBatchFee] nb, [NIELITMIS].[dbo].[feeTypeMas] ft where nb.feeTypeId=ft.ID and (ft.feeType='Registration Fee' or ft.feeType='GST on Registration Fee') and batchID= " + application2.batchID;

                                                                            using (DataTable dt = GetApplicationDetails(batchdetailsfeetype))
                                                                                {
                                                                                if (dt.Rows.Count > 0)
                                                                                    {
                                                                                    int j;
                                                                                    for (j = 0; j < dt.Rows.Count; j++)
                                                                                        {
                                                                                        Int64 NielitCentreStudentId = NIELITStudentFeePaid1.ID;
                                                                                        Int64 feeType = dt.Rows[j].Field<Int64>("feeTypeId");
                                                                                        int NIELITStudentFeePaidsCount = context.NIELITStudentFeePaids.Where(f => f.feeTypeID == feeType && f.studentID == NIELITStudentFeePaid1.ID).Count();
                                                                                        if (NIELITStudentFeePaidsCount == 0)
                                                                                            {
                                                                                            NIELITStudentFeePaid nsf = new NIELITStudentFeePaid();
                                                                                            nsf.enterDate = DateTime.Now;
                                                                                            //nsf.enterBy = Convert.ToInt32(NielitCentreStudentId);
                                                                                            Int32 enterby = 0;
                                                                                            enterby = Convert.ToInt32(Session["UserID"]);
                                                                                            if (enterby != 0) { nsf.enterBy = enterby; }
                                                                                            nsf.feeTypeID = dt.Rows[j].Field<Int64>("feeTypeId");
                                                                                            nsf.AmtPaid = dt.Rows[j].Field<int>("feeAmount");
                                                                                            nsf.studentID = NielitCentreStudentId;
                                                                                            nsf.paymentDate = DateTime.Now;
                                                                                            nsf.Remarks = "Online Payment of Virtual Academy Registration";
                                                                                            context.NIELITStudentFeePaids.Add(nsf);
                                                                                            context.SaveChanges();
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    # endregion

                                                                    }

                                                                //saving demand note 
                                                                context.Entry(demandNote).State = System.Data.Entity.EntityState.Modified;
                                                                context.SaveChanges();

                                                                //saving online transaction
                                                                context.Entry(onlinetransaction).State = System.Data.Entity.EntityState.Modified;
                                                               context.SaveChanges();

                                                               #region Registration Process for NSQF Courses
                                                               // deep 08 june start
                                                                Int64 AadhaarNumberDecrypted = 0;
                                                                var VAaPP = context.virtualAcademyRegistration.Where(c => c.DemandNoteID == demandNote.ID).FirstOrDefault();
                                                                string virtualAcademyAppNumber = VAaPP.Number.ToString();
                                                                string courseCatId = VAaPP.CourseCategoryID.ToString();
                                                                                                                              
                                                                if (courseCatId == "6") // 6 for Short Term Course (NSQF Aligned)
                                                                {
                                                                    Int64 CraID = 0;
                                                                    AadhaarNumberDecrypted = Convert.ToInt64(EncryptDecrypt.DecryptString(VAaPP.AadharNumber.ToString()));

                                                                    CraID = InsertVARegDataToCourseRegApplication(virtualAcademyAppNumber, AadhaarNumberDecrypted);
                                                                }
                                                                    //deep 08 june end
                                                               #endregion

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
                        # endregion

                        # region Online Payment Mode--------------- Old
                        //if (paymentMode == enmPaymentMode.Online)
                        //{
                        //    if (CommonFunctions.IsNumeric(dr[7].ToString()))
                        //    {
                        //        totalRecordsCount = totalRecordsCount + 1;
                        //        if (!refernce3.Contains(dr[9].ToString()))
                        //        {
                        //            NotValidateRecords = NotValidateRecords + 1;
                        //            appIdList4.Append(dr[7].ToString() + ",");
                        //            if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                        //                appIdList4.Append(WebUtility.HtmlDecode("<br/>"));
                        //            continue;
                        //        }
                        //        else
                        //        {
                        //            transid = Convert.ToInt32(dr[7]);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        continue;
                        //    }
                        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(900)))
                        //    {
                        //        var online = (from r in context.OnlineTransaction
                        //                      join d in context.DemandNotes
                        //                      on r.DemandNoteID equals d.ID
                        //                      where r.ID == transid
                        //                      select new
                        //                      {
                        //                          responseCode = r.ResponseStatusCode == null ? "" : r.ResponseStatusCode,
                        //                          demannotePaymentstatus = d.PaymentStatusID,
                        //                          ID = r.ID,
                        //                          Amount = r.Amount,
                        //                          DemandNoteID = r.DemandNoteID,
                        //                          ReferenceNumber = r.ReferenceNumber,
                        //                          RequestDate = r.RequestDate,
                        //                          ResponseStatusMessage = r.ResponseStatusMessage,
                        //                          IsSettled = r.IsSettled
                        //                      }).FirstOrDefault();

                        //        if (online != null)
                        //        {
                        //            TxReferencenumber = dr[5].ToString();
                        //            Productcode = dr[4].ToString();
                        //            BankID = "";
                        //            BankReferenceNo = "";
                        //            TransactionDate = dr[11].ToString();
                        //            if ((online.responseCode.Trim() != "0300" && online.ResponseStatusMessage != "Success") || (online.demannotePaymentstatus == Convert.ToInt32(enmPaymentStatus.Pending)))
                        //            {
                        //                if (IsRefundable(online.DemandNoteID, online.IsSettled))
                        //                {
                        //                    try
                        //                    {
                        //                        AddInRefund(online.ID, TxReferencenumber, Productcode, BankID, BankReferenceNo, TransactionDate);
                        //                    }
                        //                    catch
                        //                    {
                        //                        NotValidateRecords = NotValidateRecords + 1;
                        //                        appIdList4.Append(transid.ToString() + ",");
                        //                        if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                        //                            appIdList4.Append(WebUtility.HtmlDecode("<br/>"));
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    try
                        //                    {
                        //                        SettleManually(online.ID, TxReferencenumber, Productcode, BankID, BankReferenceNo, TransactionDate);
                        //                    }
                        //                    catch
                        //                    {
                        //                        NotValidateRecords = NotValidateRecords + 1;
                        //                        appIdList4.Append(transid.ToString() + ",");
                        //                        if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                        //                            appIdList4.Append(WebUtility.HtmlDecode("<br/>"));
                        //                    }
                        //                }
                        //            }
                        //            else
                        //            {
                        //                try
                        //                {
                        //                    if (online.responseCode.Trim() == "0300")
                        //                    {
                        //                        if (IsSuccessRefundable(online.ID))
                        //                        {
                        //                            try
                        //                            {
                        //                                AddInRefund(online.ID, TxReferencenumber, Productcode, BankID, BankReferenceNo, TransactionDate);
                        //                            }
                        //                            catch
                        //                            {
                        //                                NotValidateRecords = NotValidateRecords + 1;
                        //                                appIdList4.Append(transid.ToString() + ",");
                        //                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                        //                                    appIdList4.Append(WebUtility.HtmlDecode("<br/>"));
                        //                            }
                        //                        }
                        //                        else
                        //                        {
                        //                            if (context.OnlineTransaction.Any(s => s.ID == online.ID && s.ReferenceNumber == online.ReferenceNumber && s.Amount == online.Amount && System.Data.Entity.DbFunctions.TruncateTime(s.RequestDate) == System.Data.Entity.DbFunctions.TruncateTime(online.RequestDate)))
                        //                            {
                        //                                DemandNote demandNote = context.DemandNotes.Find(online.DemandNoteID);
                        //                                if (demandNote != null)
                        //                                {
                        //                                    if (demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified))
                        //                                    {
                        //                                        reconcilledRecordsCount = reconcilledRecordsCount + 1;
                        //                                        demandNote.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);

                        //                                        OnlineTransaction onlinetransaction = context.OnlineTransaction.Find(online.ID);
                        //                                        onlinetransaction.SettledOn = DateTime.Now;

                        //                                        if (demandNote.ApplicationTypeID == Convert.ToInt32(EConnect.NIELIT.enmApplicationType.CertificateExamApplication))
                        //                                        {
                        //                                            var application = context.CertificateExamApplications.Where(s => s.DemandNoteID == demandNote.ID);
                        //                                            if (application != null)
                        //                                            {
                        //                                                context.Database.ExecuteSqlCommand(" Update Certificate_Exam_Application set  Payment_Status_ID = " + PaymentPaid + " Where Demand_Note_ID = " + demandNote.ID);
                        //                                                context.SaveChanges();
                        //                                            }
                        //                                        }
                        //                                        else if (demandNote.ApplicationTypeID == Convert.ToInt32(EConnect.NIELIT.enmApplicationType.CourseRegistrationApplication))
                        //                                        {

                        //                                            var application = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandNote.ID);
                        //                                            if (application != null)
                        //                                            {
                        //                                                context.Database.ExecuteSqlCommand(" Update Course_Registration_Application set  Payment_Status_ID = " + PaymentPaid + " Where Demand_Note_ID = " + demandNote.ID);
                        //                                                context.SaveChanges();
                        //                                            }

                        //                                        }
                        //                                        else if (demandNote.ApplicationTypeID == Convert.ToInt32(EConnect.NIELIT.enmApplicationType.CourseExamApplication))
                        //                                        {

                        //                                            var application = context.CourseExamApplications.Where(s => s.DemandNoteID == demandNote.ID);
                        //                                            if (application != null)
                        //                                            {
                        //                                                context.Database.ExecuteSqlCommand(" Update Course_Exam_Application set  Payment_Status_ID = " + PaymentPaid + " Where Demand_Note_ID = " + demandNote.ID);
                        //                                                context.SaveChanges();
                        //                                            }
                        //                                        }
                        //                                        else if (demandNote.ApplicationTypeID == Convert.ToInt32(EConnect.NIELIT.enmApplicationType.ModuleCertificateRequest))
                        //                                        {

                        //                                            var application = context.ModuleCertificateRequests.Where(s => s.DemandNoteID == demandNote.ID);
                        //                                            if (application != null)
                        //                                            {
                        //                                                context.Database.ExecuteSqlCommand(" Update ModuleCertificateRequest set  Payment_Status_ID = " + PaymentPaid + " Where Demand_Note_ID = " + demandNote.ID);
                        //                                                context.SaveChanges();
                        //                                            }
                        //                                        }

                        //                                        //saving demand note 
                        //                                        context.Entry(demandNote).State = System.Data.Entity.EntityState.Modified;
                        //                                        context.SaveChanges();

                        //                                        //saving online transaction
                        //                                        context.Entry(onlinetransaction).State = System.Data.Entity.EntityState.Modified;
                        //                                        context.SaveChanges();
                        //                                    }
                        //                                    else
                        //                                    {
                        //                                        NotValidateRecords = NotValidateRecords + 1;
                        //                                        appIdList2.Append(transid.ToString() + ",");
                        //                                        if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                        //                                            appIdList2.Append(WebUtility.HtmlDecode("<br/>"));
                        //                                    }
                        //                                }
                        //                                else
                        //                                {
                        //                                    NotValidateRecords = NotValidateRecords + 1;
                        //                                    appIdList3.Append(transid.ToString() + ",");
                        //                                    if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                        //                                        appIdList3.Append(WebUtility.HtmlDecode("<br/>"));
                        //                                }
                        //                            }
                        //                            else
                        //                            {
                        //                                NotValidateRecords = NotValidateRecords + 1;
                        //                                appIdList4.Append(transid.ToString() + ",");
                        //                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                        //                                    appIdList4.Append(WebUtility.HtmlDecode("<br/>"));
                        //                            }
                        //                        }
                        //                    }
                        //                }
                        //                catch (Exception ex)
                        //                {
                        //                    NotValidateRecords = NotValidateRecords + 1;
                        //                    appIdList4.Append(transid.ToString() + ",");
                        //                    if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                        //                        appIdList4.Append(WebUtility.HtmlDecode("<br/>"));
                        //                }
                        //            }
                        //        }
                        //        else
                        //        {
                        //            NotValidateRecords = NotValidateRecords + 1;
                        //            appIdList3.Append(transid.ToString() + ",");
                        //            if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                        //                appIdList3.Append(WebUtility.HtmlDecode("<br/>"));
                        //        }
                        //        scope.Complete();
                        //    };
                        //}
                        # endregion

                        # region CSC Payment Mode ----------------Old
                        //else if (paymentMode == enmPaymentMode.CSCSPV)
                        //{
                        //    if (CommonFunctions.IsNumeric(dr[8].ToString()))
                        //    {
                        //        transid = Convert.ToInt32(dr[8]);
                        //        totalRecordsCount = totalRecordsCount + 1;
                        //    }
                        //    else
                        //    {
                        //        continue;
                        //    }
                        //    using (TransactionScope scope = new TransactionScope())
                        //    {
                        //        //var csc = (from r in context.CSCTransactions
                        //        //           where r.ID == transid && (r.ResponseStatus.Value == 100 || r.ResponseStatus.Value == 0)
                        //        //           select r).FirstOrDefault();
                        //        var csc = context.CSCTransactions.Find(transid);
                        //        if (csc.ResponseStatus.Value == 100 || csc.ResponseStatus.Value == 0)
                        //        {
                        //            if (context.CSCTransactions.Any(s => s.ID == csc.ID && s.ResponseTransactionNumber == csc.ResponseTransactionNumber && s.Amount == csc.Amount && System.Data.Entity.DbFunctions.TruncateTime(s.Date) == System.Data.Entity.DbFunctions.TruncateTime(csc.Date)))
                        //            {
                        //                DemandNote demandNote = context.DemandNotes.Find(csc.DemandNoteID);
                        //                if (demandNote != null)
                        //                {
                        //                    if (demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified))
                        //                    {
                        //                        reconcilledRecordsCount += 1;
                        //                        demandNote.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                        //                        if (demandNote.ApplicationTypeID == Convert.ToInt32(EConnect.NIELIT.enmApplicationType.CertificateExamApplication))
                        //                        {
                        //                            var application = context.CertificateExamApplications.Where(s => s.DemandNoteID == demandNote.ID);
                        //                            if (application != null)
                        //                            {
                        //                                context.Database.ExecuteSqlCommand(" Update Certificate_Exam_Application set  Payment_Status_ID = " + PaymentPaid + " Where Demand_Note_ID = " + demandNote.ID);
                        //                                context.SaveChanges();
                        //                            }
                        //                        }
                        //                        else if (demandNote.ApplicationTypeID == Convert.ToInt32(EConnect.NIELIT.enmApplicationType.CourseRegistrationApplication))
                        //                        {

                        //                            var application = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandNote.ID);
                        //                            if (application != null)
                        //                            {
                        //                                context.Database.ExecuteSqlCommand(" Update Course_Registration_Application set  Payment_Status_ID = " + PaymentPaid + " Where Demand_Note_ID = " + demandNote.ID);
                        //                                context.SaveChanges();
                        //                            }
                        //                        }
                        //                        else if (demandNote.ApplicationTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        //                        {

                        //                            var application = context.CourseExamApplications.Where(s => s.DemandNoteID == demandNote.ID);
                        //                            if (application != null)
                        //                            {
                        //                                context.Database.ExecuteSqlCommand(" Update Course_Exam_Application set  Payment_Status_ID = " + PaymentPaid + " Where Demand_Note_ID = " + demandNote.ID);
                        //                                context.SaveChanges();
                        //                            }
                        //                        }
                        //                        else if (demandNote.ApplicationTypeID == Convert.ToInt32(enmApplicationType.ModuleCertificateRequest))
                        //                        {

                        //                            var application = context.ModuleCertificateRequests.Where(s => s.DemandNoteID == demandNote.ID);
                        //                            if (application != null)
                        //                            {
                        //                                context.Database.ExecuteSqlCommand(" Update ModuleCertificateRequest set  Payment_Status_ID = " + PaymentPaid + " Where Demand_Note_ID = " + demandNote.ID);
                        //                                context.SaveChanges();
                        //                            }
                        //                        };
                        //                        context.Entry(demandNote).State = System.Data.Entity.EntityState.Modified;
                        //                        context.SaveChanges();
                        //                    }
                        //                    else
                        //                    {
                        //                        NotValidateRecords = NotValidateRecords + 1;
                        //                        appIdList2.Append(transid.ToString() + ",");
                        //                        if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                        //                            appIdList2.Append(WebUtility.HtmlDecode("<br/>"));
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    NotValidateRecords = NotValidateRecords + 1;
                        //                    appIdList3.Append(transid.ToString() + ",");
                        //                    if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                        //                        appIdList3.Append(WebUtility.HtmlDecode("<br/>"));
                        //                }
                        //            }
                        //            else
                        //            {
                        //                NotValidateRecords = NotValidateRecords + 1;
                        //                appIdList4.Append(transid.ToString() + ",");
                        //                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                        //                    appIdList4.Append(WebUtility.HtmlDecode("<br/>"));
                        //            }
                        //        }
                        //        else
                        //        {
                        //            NotValidateRecords = NotValidateRecords + 1;
                        //            appIdList3.Append(transid.ToString() + ",");
                        //            if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                        //                appIdList3.Append(WebUtility.HtmlDecode("<br/>"));
                        //        }
                        //        scope.Complete();
                        //    };
                        //}
                        # endregion

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

    protected Boolean IsRefundable(Int64 DemandNoteID, Boolean IsSettled)
        {
        if (!IsSettled)
            {
            try
                {
                using (NIELITMISContext context = new NIELITMISContext())
                    {
                    VirtualAcademyDemandNote demandNote = context.VirtualAcademyDemandNotes.Find(DemandNoteID);

                    if (demandNote != null)
                        {
                        if (demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending))
                            {
                            if (demandNote.ApplicationTypeID == Convert.ToInt32(EConnect.NIELIT.enmApplicationTypeVirtualAcademy.VirtualAcademyCourseRegistrationApplication))
                                {
                                Int32 paid = Convert.ToInt32(enmPaymentStatus.Paid);
                                Int32 paidNotVerified = Convert.ToInt32(enmPaymentStatus.PaidButNotVerified);
                                int coursregcount = context.virtualAcademyRegistration.Where(s => s.DemandNoteID == demandNote.ID && (s.PaymentStatusID == paidNotVerified || s.PaymentStatusID == paid)).Count();
                                if (coursregcount > 0)
                                    {
                                    return true;
                                    }
                                }
                            }
                        else
                            {
                            if (context.VirtualAcademyOnlineTransaction.Where(a => a.DemandNoteID == demandNote.ID && a.ResponseStatusCode == "0300").Count() > 0)
                                return true;
                            }
                        }
                    //return true;  //in case of the demand note deleted or cancelled
                    };
                }
            catch (Exception ex)
                {
                return false;
                }
            }
        return false;
        }

    protected Boolean IsSuccessRefundable(Int64 onlineTransactionID)
        {
        try
            {
            using (NIELITMISContext context = new NIELITMISContext())
                {
                Int64 demandID = context.VirtualAcademyOnlineTransaction.Find(onlineTransactionID).DemandNoteID;
                //var coursReg = context.virtualAcademyRegistration.Where(s => s.DemandNoteID == demandID).ToList();
                int coursReg = context.virtualAcademyRegistration.Where(s => s.DemandNoteID == demandID).Count();

                if (coursReg > 0)
                    {
                    var demandNote = context.VirtualAcademyDemandNotes.Find(demandID);
                    if (demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified) && demandNote.OnlineTransactionID == onlineTransactionID)
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
            }
        return false;
        }

    protected void AddInRefund(Int64 OnlineTransactionId, String TxReferencenumber, String Productcode, String BankID, String BankReferenceNo, String TransactionDate)
        {
        // *** Code For Creating Refund File ****//
        BreadCrumb1.Render();
        try
            {
            using (NIELITMISContext context = new NIELITMISContext())
                {
                virtualAcademyOnlineRefund refund;
                var online = context.VirtualAcademyOnlineTransaction.Find(OnlineTransactionId);
                var demandnote = context.VirtualAcademyDemandNotes.Find(online.DemandNoteID);
                //var coursReg = context.virtualAcademyRegistration.Where(s => s.DemandNoteID == demandnote.ID).ToList();
                int coursReg = context.virtualAcademyRegistration.Where(s => s.DemandNoteID == demandnote.ID).Count();

                if (online != null)
                    {
                    string[] date = TransactionDate.ToString().Substring(0, 10).Split('/');
                    DateTime date1 = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));
                    Boolean makeRefund = false;
                    Boolean ChargeBack = false;
                    if (online.ResponseStatusCode != "0300" || online.ResponseStatusCode == null)
                        {
                        makeRefund = true;
                        }
                    else if ((online.ResponseStatusCode == "0300") && (demandnote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending)))
                        {
                        makeRefund = true;
                        }
                    else if ((online.ResponseStatusCode == "0300") && (demandnote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified)))
                        {
                        makeRefund = true;
                        }
                    //else if ((online.ResponseStatusCode == "0300") && (demandnote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Paid)) && (coursExam.Count() == 0 && certificateExam.Count() == 0 && coursReg.Count() == 0))
                    else if ((online.ResponseStatusCode == "0300") && (demandnote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Paid)) && coursReg == 0)
                        {
                        makeRefund = true;
                        }
                    //else if (demandnote.CSCTransaction_ID.HasValue == true || demandnote.DDTransactionID.HasValue == true || demandnote.NEFTTransactionID.HasValue == true)
                    //{
                    //    makeRefund = true;
                    //}

                    if (makeRefund)
                        {
                        //if (context.Online_ChargeBackTransactions.Where(a => a.Ref1 == online.ID).Count() > 0)
                        //{
                        //    ChargeBack = true;
                        //    makeRefund = false;
                        //    NotValidateRecords = NotValidateRecords + 1;
                        //    appIdList.Append(OnlineTransactionId.ToString() + ",");
                        //    if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                        //        appIdList.Append(WebUtility.HtmlDecode("<br/>"));
                        //}
                        }

                    if (makeRefund)
                        {
                        refund = new virtualAcademyOnlineRefund();
                        Boolean getRefund = false;
                        var refunRecord = context.virtualAcademyOnlineRefund.Where(s => s.TransactionID == online.ID);
                        if (refunRecord == null || refunRecord.Count() == 0)
                            {
                            refund.BankID = BankID;
                            refund.TransactionID = online.ID;
                            refund.Txt_Ref_No = TxReferencenumber;
                            refund.Bank_Ref_No = BankReferenceNo;
                            refund.ProductID = Productcode;
                            refund.RefundDate = DateTime.Now;
                            refund.TransactionDate = date1;
                            refund.TransactionAmount = Convert.ToInt64(online.Amount * 100);
                            //Double refundamount = Convert.ToDouble(dr[7].ToString());
                            refund.Refund_Amount = Convert.ToInt64(online.Amount * 100);
                            //context.OnlineRefunds.Add(refund);
                            context.virtualAcademyOnlineRefund.Add(refund);
                            context.SaveChanges();
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
                            //download of refund file
                            writer.Write(TxReferencenumber.ToString());
                            writer.Write(",");
                            string year = date1.Year.ToString();
                            string month = date1.Month.ToString();
                            string day = date1.Day.ToString();
                            string finaldate = year + month + day;
                            writer.Write(finaldate);
                            writer.Write(",");
                            writer.Write(OnlineTransactionId.ToString());
                            writer.Write(",");
                            writer.Write(refund.TransactionAmount.ToString());
                            writer.Write(",");
                            writer.Write(refund.Refund_Amount.ToString());
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

    protected void btnSave_Click(object sender, EventArgs e)
        {

        try
            {
            ProcessTransactionData();
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message.ToString());
            }

        }

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
                Int32 csc = Convert.ToInt32(enmPaymentMode.CSCSPV);
                Int32 onlineBilldesk = Convert.ToInt32(enmPaymentMode.Online);

                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.PaymentModes
                               where p.ID != multicheque && p.ID != cash && p.ID != DemandDraft && p.ID != NEFTRTGS && p.ID != csc
                               && p.ID == onlineBilldesk
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlpaymentmode, Category, lst);
                ddlpaymentmode.SelectedValue = Convert.ToInt32(enmPaymentMode.Online).ToString();  // To select the online payment option
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
            //ddlpaymentmode.SelectedValue = "0";
            bindpaymentmode();
            divValidateData.Visible = false;
            btndownload.Visible = false;
            btnSave.Visible = true;
            lblFailedRecords.Text = string.Empty;
            lblRefundRecords.Text = string.Empty;
            lblSettledRecords.Text = lblManSet.Text = lblTotalRecords.Text = lblFailRecordsCount.Text = string.Empty;
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }
    // Deep Add code on 10 June 2022 start         
    private Int64 InsertVARegDataToCourseRegApplication(string VANumber, Int64 AadhaarNumberDecrypted)
    {
        try
        {
            Int64 candidateID = 0;
            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString; 
            using (SqlConnection Conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("VA_RegNoGenerateNSQFCandidateProcess", Conn))
                {
                    Conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pNumber", VANumber);                   
                    cmd.Parameters.Add(new SqlParameter("@AadhaarNumberDecrypted", SqlDbType.BigInt));
                    cmd.Parameters["@AadhaarNumberDecrypted"].Value = AadhaarNumberDecrypted;
                    cmd.Parameters.Add("@CourseRegnAppIDD", SqlDbType.VarChar, 30);
                    cmd.Parameters["@CourseRegnAppIDD"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    candidateID = Convert.ToInt64(cmd.Parameters["@CourseRegnAppIDD"].Value.ToString());
                }
            }

            return candidateID;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    // Deep code end on 10 June 2022 end
    
    public DataTable GetApplicationDetails(string squery)
        {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
            {
            try
                {
                using (SqlCommand cmd = new SqlCommand(squery, con))
                    {
                    cmd.CommandType = CommandType.Text;
                    //cmd.Parameters.Add(new SqlParameter("@pApplID", SqlDbType.Int));
                    //cmd.Parameters["@pApplID"].Value = appId; // 1 for course record from database
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                        sda.Fill(myDt);
                        }
                    }
                }
            catch (Exception ex)
                {
                }
            finally
                {
                con.Close();
                }
            }
        return myDt;
        }

    protected void SettleManually(Int64 OnlineTransactionId, String TxReferencenumber, String Productcode, String BankID, String BankReferenceNo, String TransactionDate)
        {
        try
            {
            using (NIELITMISContext context = new NIELITMISContext())
                {
                var online = context.VirtualAcademyOnlineTransaction.Find(OnlineTransactionId);
                if (online != null && online.ResponseStatusCode != "0300" && online.ResponseStatusMessage != "Success" && !online.IsSettled)
                    {
                    try
                        {
                        VirtualAcademyDemandNote demandNote = context.VirtualAcademyDemandNotes.Find(online.DemandNoteID);
                        if (demandNote != null)
                            {
                            Int32 coursregcount = context.virtualAcademyRegistration.Where(s => s.DemandNoteID == demandNote.ID).Count();

                            if ((demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.Pending) || demandNote.PaymentStatusID == Convert.ToInt32(enmPaymentStatus.PaidButNotVerified)) && coursregcount > 0)
                                {
                                DateTime createdDate = DateTime.Now;
                                Int32 createdByID = Convert.ToInt32(Session["UserID"]);
                                var trans = (from r in context.VirtualAcademyOnlineTransaction
                                             where r.DemandNoteID == demandNote.ID && r.ResponseStatusCode == "0300" && r.ResponseStatusMessage == "Success"
                                             select r);
                                if (trans.Count() <= 0)
                                    {
                                    //creating history of the online_transaction
                                    context.Database.ExecuteSqlCommand("insert into [NIELITMIS].[dbo].[virtualAcademyOnline_TransactionHistory] (Request_Date,Demand_Note_ID,Amount,Request_Parameters,Response_Parameters,Response_Date,Reference_Number,Response_Status_Code,Response_Status_Message,Created_By,Is_Settled,Settled_By,Settled_On,Settled_FileName,Transaction_ID,History_Created_By,History_Created_on) (select s.Request_Date,s.Demand_Note_ID,s.Amount,s.Request_Parameters,s.Response_Parameters,s.Response_Date,s.Reference_Number,s.Response_Status_Code,s.Response_Status_Message,s.Created_By, s.Is_Settled, s.Settled_By, s.Settled_On,s.Settled_FileName,s.ID, " + createdByID + " , '" + createdDate + "' from [NIELITMIS].[dbo].[virtualAcademyOnline_Transaction] s where s.id = " + online.ID + ")");
                                    context.SaveChanges();

                                    string[] date = TransactionDate.ToString().Substring(0, 10).Split('/');
                                    DateTime date1 = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));
                                    online.ResponseDate = date1;
                                    online.ReferenceNumber = Convert.ToString(TxReferencenumber).Trim();
                                    online.ResponseStatusCode = "0300";
                                    online.ResponseStatusMessage = "Success";
                                    online.IsSettled = true;
                                    online.SettledBy = Convert.ToInt32(Session["UserID"]);
                                    online.SettledOn = DateTime.Now;
                                    online.SettledFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + flUpload.FileName;

                                    // Updating Demand Note and application Status
                                    VirtualAcademyDemandNote objdemandnote = context.VirtualAcademyDemandNotes.Find(online.DemandNoteID);

                                    objdemandnote.OnlineTransactionID = online.ID;
                                    objdemandnote.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                    objdemandnote.PaymentModeID = Convert.ToInt32(enmPaymentMode.Online);

                                    if (objdemandnote.enmApplicationTypeVirtualAcademy == EConnect.NIELIT.enmApplicationTypeVirtualAcademy.VirtualAcademyCourseRegistrationApplication)
                                        {
                                        context.Database.ExecuteSqlCommand("Update [NIELITMIS].[dbo].[virtualAcademyRegistration] set Application_Status_ID = " + Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted) + ", Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " where Demand_Note_ID = " + objdemandnote.ID);
                                       context.SaveChanges();

                                        // to update the student details in NIELITCentreStudent and NIELITCentreStudentFeePaid
                                        # region Entrt into NIELITCentreStudent

                                        var applicationNcs = context.virtualAcademyRegistration.Where(c => c.DemandNoteID == demandNote.ID).FirstOrDefault();

                                        if (applicationNcs != null)
                                            {
                                            int NielitCentreStudentCount = context.NielitCentreStudent.Where(c => c.Number == applicationNcs.Number).Count();

                                            if (NielitCentreStudentCount == 0)
                                                {
                                                NielitCentreStudent ncs = new NielitCentreStudent();
                                                ncs.AadharNumber = Convert.ToInt64(EncryptDecrypt.DecryptString(applicationNcs.AadharNumber.ToString()));
                                                ncs.AadharVerfied = applicationNcs.AadharVerfied;
                                                ncs.affidavit_Date = applicationNcs.affidavitDate;
                                                ncs.affidavit_No = applicationNcs.affidavitNo;
                                                ncs.affidavit_Verified = applicationNcs.affidavitVerified;
                                                ncs.AlreadyQualified = applicationNcs.AlreadyQualified;
                                                ncs.AlreadyRegistered = applicationNcs.Already_Registered;
                                                int appTypeId = 0;
                                                appTypeId = Convert.ToInt32(applicationNcs.Applicant_Type_Id);
                                                if (appTypeId != 0) { ncs.ApplicantTypeID = appTypeId; }
                                                ncs.ApplicationDate = applicationNcs.ApplicationDate;
                                                ncs.batch_ID = applicationNcs.batchID;
                                                ncs.BodyMark = applicationNcs.BodyMark;
                                                ncs.CandidateID = applicationNcs.Candidate_ID;
                                                ncs.CastCategoryID = applicationNcs.CastCategoryID;
                                                ncs.certificate_Issue_Date = applicationNcs.certificateIssueDate;
                                                ncs.InstituteID = applicationNcs.centreID;
                                                //ncs.comapny_Address
                                                ncs.comapny_Address = applicationNcs.comapnyAddress;
                                                ncs.comapny_Address2 = applicationNcs.comapnyAddress2;
                                                ncs.comapny_Address3 = applicationNcs.comapnyAddress3;
                                                ncs.company_Name = applicationNcs.companyNameID;
                                                ncs.CompnyCityName = applicationNcs.comapnyCity_Name;
                                                ncs.compnyState_ID = applicationNcs.comapnyState_ID;
                                                ncs.compnyDistrict_ID = applicationNcs.comapnyDistrict_ID;
                                                ncs.CompnyPinCode = applicationNcs.comapnyPin_Code;
                                                //
                                                ncs.CorAddressLine1 = applicationNcs.CorAddressLine1;
                                                ncs.CorAddressLine2 = applicationNcs.CorAddressLine2;
                                                ncs.CorAddressLine3 = applicationNcs.CorAddressLine3;
                                                ncs.CorCityName = applicationNcs.CorCityName;
                                                ncs.CorDistrictID = applicationNcs.CorDistrictID;
                                                ncs.CorPinCode = applicationNcs.CorPinCode;
                                                ncs.CorStateID = applicationNcs.CorStateID;
                                                //
                                                ncs.CourseID = applicationNcs.CourseDurationID;
                                                ncs.DateOfBirth = applicationNcs.DateOfBirth;
                                                ncs.DateOfVerificationByInstitute = applicationNcs.DateOfVerificationByInstitute;
                                                ncs.EmailAddress = applicationNcs.EmailAddress;
                                                Int32 enterBy1 = 0;
                                                //enterBy1 = Convert.ToInt32(applicationNcs.enterBy);
                                                enterBy1 = Convert.ToInt32(Session["UserID"]);
                                                if (enterBy1 != 0) { ncs.enter_By = enterBy1; }
                                                ncs.enter_Date = DateTime.Now;
                                                ncs.ExperienceInYears = applicationNcs.ExperienceInYears;
                                                ncs.FatherName = applicationNcs.FatherName;
                                                ncs.FinalSubmissionDate = applicationNcs.FinalSubmissionDate;
                                                ncs.FinalSubmitted = applicationNcs.FinalSubmitted;
                                                ncs.Gender = applicationNcs.Gender;
                                                ncs.GuardianName = applicationNcs.GuardianName;
                                                ncs.Is_EWS = applicationNcs.Is_EWS;
                                                ncs.IsExServicemane = applicationNcs.IsExServicemane;
                                                ncs.IsHandicaped = applicationNcs.IsHandicaped;
                                                ncs.IsVerifiedByInstitute = applicationNcs.IsVerifiedByInstitute;
                                                ncs.MaritalStatusID = applicationNcs.MaritalStatusID;
                                                ncs.MobileNumber = applicationNcs.MobileNumber;
                                                ncs.MotherName = applicationNcs.MotherName;
                                                ncs.Name = applicationNcs.Name;
                                                ncs.Number = applicationNcs.Number;
                                                //
                                                ncs.PerAddressLine1 = applicationNcs.PerAddressLine1;
                                                ncs.PerAddressLine2 = applicationNcs.PerAddressLine2;
                                                ncs.PerAddressLine3 = applicationNcs.PerAddressLine3;
                                                ncs.PerCityName = applicationNcs.PerCityName;
                                                ncs.PerDistrictID = applicationNcs.PerDistrictID;
                                                ncs.PerStateID = applicationNcs.PerStateID;
                                                ncs.PerPinCode = applicationNcs.PerPinCode;
                                                ncs.PhoneNumber = applicationNcs.PhoneNumber;
                                                ncs.placementDate = applicationNcs.placementDate;
                                                int projectId1 = 0;
                                                projectId1 = Convert.ToInt32(applicationNcs.projectId);
                                                if (projectId1 != 0) { ncs.projectId = projectId1; }
                                                ncs.QualifiedCourseID = applicationNcs.QualifiedCourseID;
                                                ncs.QualifiedCoursePassingYear = applicationNcs.QualifiedCoursePassingYear;
                                                ncs.QualifiedCourseRegistrationNo = applicationNcs.QualifiedCourseRegistrationNo;
                                                ncs.RegisteredCourseID = applicationNcs.Registered_Course_ID;
                                                ncs.RegisteredCourseRegistrationNo = applicationNcs.Registered_Course_Registration_No;
                                                ncs.ReligionID = applicationNcs.ReligionID;
                                                ncs.Salutation = applicationNcs.Salutation;
                                                ncs.StdNumber = applicationNcs.StdNumber;
                                                ncs.UIDNumber = applicationNcs.UIDNumber;
                                                ncs.UIDType = applicationNcs.UIDType;
                                                ncs.whether_Certificate_Issued = applicationNcs.whetherCertificateIssued;
                                                ncs.whether_Course_Complete = applicationNcs.whetherCourseComplete;
                                                ncs.whetherPlaced = applicationNcs.whetherPlaced;
                                                ncs.whetherProjectStudent = applicationNcs.whetherProjectStudent;
                                                // To save NIELIT Centre Student
                                                context.NielitCentreStudent.Add(ncs);
                                                context.SaveChanges();
                                                }
                                            }

                                        //var applicationNcs = context.virtualAcademyRegistration.Where(c => c.DemandNoteID == demandNote.ID).FirstOrDefault();
                                        ////var app1 = context.NielitCentreStudent.Where(c => c.Number == applicationNcs.Number).ToList();
                                        //if (applicationNcs != null)
                                        //    {
                                        //    //if (app1.Count() == 0)
                                        //    //{
                                        //    NielitCentreStudent ncs = new NielitCentreStudent();
                                        //    ncs.AadharNumber = Convert.ToInt64(EncryptDecrypt.DecryptString(applicationNcs.AadharNumber.ToString()));
                                        //    ncs.AadharVerfied = applicationNcs.AadharVerfied;
                                        //    ncs.affidavit_Date = applicationNcs.affidavitDate;
                                        //    ncs.affidavit_No = applicationNcs.affidavitNo;
                                        //    ncs.affidavit_Verified = applicationNcs.affidavitVerified;
                                        //    ncs.AlreadyQualified = applicationNcs.AlreadyQualified;
                                        //    ncs.AlreadyRegistered = applicationNcs.Already_Registered;
                                        //    int appTypeId = 0;
                                        //    appTypeId = Convert.ToInt32(applicationNcs.Applicant_Type_Id);
                                        //    if (appTypeId != 0) { ncs.ApplicantTypeID = appTypeId; }
                                        //    ncs.ApplicationDate = applicationNcs.ApplicationDate;
                                        //    ncs.batch_ID = applicationNcs.batchID;
                                        //    ncs.BodyMark = applicationNcs.BodyMark;
                                        //    ncs.CandidateID = applicationNcs.Candidate_ID;
                                        //    ncs.CastCategoryID = applicationNcs.CastCategoryID;
                                        //    ncs.certificate_Issue_Date = applicationNcs.certificateIssueDate;
                                        //    ncs.InstituteID = applicationNcs.centreID;
                                        //    //ncs.comapny_Address
                                        //    ncs.comapny_Address = applicationNcs.comapnyAddress;
                                        //    ncs.comapny_Address2 = applicationNcs.comapnyAddress2;
                                        //    ncs.comapny_Address3 = applicationNcs.comapnyAddress3;
                                        //    ncs.company_Name = applicationNcs.companyNameID;
                                        //    ncs.CompnyCityName = applicationNcs.comapnyCity_Name;
                                        //    ncs.compnyState_ID = applicationNcs.comapnyState_ID;
                                        //    ncs.compnyDistrict_ID = applicationNcs.comapnyDistrict_ID;
                                        //    ncs.CompnyPinCode = applicationNcs.comapnyPin_Code;
                                        //    //
                                        //    ncs.CorAddressLine1 = applicationNcs.CorAddressLine1;
                                        //    ncs.CorAddressLine2 = applicationNcs.CorAddressLine2;
                                        //    ncs.CorAddressLine3 = applicationNcs.CorAddressLine3;
                                        //    ncs.CorCityName = applicationNcs.CorCityName;
                                        //    ncs.CorDistrictID = applicationNcs.CorDistrictID;
                                        //    ncs.CorPinCode = applicationNcs.CorPinCode;
                                        //    ncs.CorStateID = applicationNcs.CorStateID;
                                        //    //
                                        //    ncs.CourseID = applicationNcs.CourseDurationID;
                                        //    ncs.DateOfBirth = applicationNcs.DateOfBirth;
                                        //    ncs.DateOfVerificationByInstitute = applicationNcs.DateOfVerificationByInstitute;
                                        //    ncs.EmailAddress = applicationNcs.EmailAddress;
                                        //    Int32 enterBy1 = 0;
                                        //    //enterBy1 = Convert.ToInt32(applicationNcs.enterBy);
                                        //    enterBy1 = Convert.ToInt32(Session["UserID"]);
                                        //    if (enterBy1 != 0) { ncs.enter_By = enterBy1; }
                                        //    ncs.enter_Date = DateTime.Now;
                                        //    ncs.ExperienceInYears = applicationNcs.ExperienceInYears;
                                        //    ncs.FatherName = applicationNcs.FatherName;
                                        //    ncs.FinalSubmissionDate = applicationNcs.FinalSubmissionDate;
                                        //    ncs.FinalSubmitted = applicationNcs.FinalSubmitted;
                                        //    ncs.Gender = applicationNcs.Gender;
                                        //    ncs.GuardianName = applicationNcs.GuardianName;
                                        //    ncs.Is_EWS = applicationNcs.Is_EWS;
                                        //    ncs.IsExServicemane = applicationNcs.IsExServicemane;
                                        //    ncs.IsHandicaped = applicationNcs.IsHandicaped;
                                        //    ncs.IsVerifiedByInstitute = applicationNcs.IsVerifiedByInstitute;
                                        //    ncs.MaritalStatusID = applicationNcs.MaritalStatusID;
                                        //    ncs.MobileNumber = applicationNcs.MobileNumber;
                                        //    ncs.MotherName = applicationNcs.MotherName;
                                        //    ncs.Name = applicationNcs.Name;
                                        //    ncs.Number = applicationNcs.Number;
                                        //    //
                                        //    ncs.PerAddressLine1 = applicationNcs.PerAddressLine1;
                                        //    ncs.PerAddressLine2 = applicationNcs.PerAddressLine2;
                                        //    ncs.PerAddressLine3 = applicationNcs.PerAddressLine3;
                                        //    ncs.PerCityName = applicationNcs.PerCityName;
                                        //    ncs.PerDistrictID = applicationNcs.PerDistrictID;
                                        //    ncs.PerStateID = applicationNcs.PerStateID;
                                        //    ncs.PerPinCode = applicationNcs.PerPinCode;
                                        //    ncs.PhoneNumber = applicationNcs.PhoneNumber;
                                        //    ncs.placementDate = applicationNcs.placementDate;
                                        //    int projectId1 = 0;
                                        //    projectId1 = Convert.ToInt32(applicationNcs.projectId);
                                        //    if (projectId1 != 0) { ncs.projectId = projectId1; }
                                        //    ncs.QualifiedCourseID = applicationNcs.QualifiedCourseID;
                                        //    ncs.QualifiedCoursePassingYear = applicationNcs.QualifiedCoursePassingYear;
                                        //    ncs.QualifiedCourseRegistrationNo = applicationNcs.QualifiedCourseRegistrationNo;
                                        //    ncs.RegisteredCourseID = applicationNcs.Registered_Course_ID;
                                        //    ncs.RegisteredCourseRegistrationNo = applicationNcs.Registered_Course_Registration_No;
                                        //    ncs.ReligionID = applicationNcs.ReligionID;
                                        //    ncs.Salutation = applicationNcs.Salutation;
                                        //    ncs.StdNumber = applicationNcs.StdNumber;
                                        //    ncs.UIDNumber = applicationNcs.UIDNumber;
                                        //    ncs.UIDType = applicationNcs.UIDType;
                                        //    ncs.whether_Certificate_Issued = applicationNcs.whetherCertificateIssued;
                                        //    ncs.whether_Course_Complete = applicationNcs.whetherCourseComplete;
                                        //    ncs.whetherPlaced = applicationNcs.whetherPlaced;
                                        //    ncs.whetherProjectStudent = applicationNcs.whetherProjectStudent;
                                        //    // To save NIELIT Centre Student
                                        //    context.NielitCentreStudent.Add(ncs);
                                        //    context.SaveChanges();
                                        //    }
                                        ////}
                                        # endregion

                                        # region Entrt into NIELITCentreStudentFeePaid

                                        var application2 = context.virtualAcademyRegistration.Where(c => c.DemandNoteID == demandNote.ID).FirstOrDefault();
                                        if (application2 != null)
                                            {
                                            string number = application2.Number;
                                            var NIELITStudentFeePaid1 = context.NielitCentreStudent.Where(i => i.CourseID == application2.CourseDurationID && i.batch_ID == application2.batchID && i.Number == number).FirstOrDefault();
                                            if (NIELITStudentFeePaid1 != null)
                                                {
                                                string batchdetailsfeetype = "select batchID,feeTypeId, feeAmount FROM [NIELITMIS].[dbo].[NielitCentreBatchFee] nb, [NIELITMIS].[dbo].[feeTypeMas] ft where nb.feeTypeId=ft.ID and (ft.feeType='Registration Fee' or ft.feeType='GST on Registration Fee') and batchID= " + application2.batchID;
                                                //string batchdetailsfeetype = "select batchID,feeTypeId, feeAmount FROM [NIELITMIS].[dbo].[NielitCentreBatchFee] where batchID= " + application2.batchID;

                                                using (DataTable dt = GetApplicationDetails(batchdetailsfeetype))
                                                    {
                                                    if (dt.Rows.Count > 0)
                                                        {
                                                        int j;
                                                        for (j = 0; j < dt.Rows.Count; j++)
                                                            {
                                                            Int64 NielitCentreStudentId = NIELITStudentFeePaid1.ID;
                                                            Int64 feeType = dt.Rows[j].Field<Int64>("feeTypeId");
                                                            int NIELITStudentFeePaidsCount = context.NIELITStudentFeePaids.Where(f => f.feeTypeID == feeType && f.studentID == NIELITStudentFeePaid1.ID).Count();
                                                            if (NIELITStudentFeePaidsCount == 0)
                                                                {
                                                                NIELITStudentFeePaid nsf = new NIELITStudentFeePaid();
                                                                nsf.enterDate = DateTime.Now;
                                                                //nsf.enterBy = Convert.ToInt32(NielitCentreStudentId);
                                                                Int32 enterby = 0;
                                                                enterby = Convert.ToInt32(Session["UserID"]);
                                                                if (enterby != 0) { nsf.enterBy = enterby; }
                                                                nsf.feeTypeID = dt.Rows[j].Field<Int64>("feeTypeId");
                                                                nsf.AmtPaid = dt.Rows[j].Field<int>("feeAmount");
                                                                nsf.studentID = NielitCentreStudentId;
                                                                nsf.paymentDate = DateTime.Now;
                                                                nsf.Remarks = "Online Payment of Virtual Academy Registration";
                                                                context.NIELITStudentFeePaids.Add(nsf);
                                                                context.SaveChanges();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        //var application2 = context.virtualAcademyRegistration.Where(c => c.DemandNoteID == demandNote.ID).FirstOrDefault();
                                        ////var app2 = context.NIELITStudentFeePaids.Where(c => c.studentID == application2.ID).ToList();
                                        //if (application2 != null)
                                        //    {
                                        //    //if (app2.Count() == 0)
                                        //    //{
                                        //    string OnlineRefNo = application2.Number;
                                        //    var nielitcentreStudentData = context.NielitCentreStudent.Where(k => k.batch_ID == application2.batchID && k.Number == OnlineRefNo).FirstOrDefault();
                                        //    Int64 NielitCentreStudentId = nielitcentreStudentData.ID;
                                        //    string batchdetailsfeetype = "select batchID,feeTypeId, feeAmount FROM [NIELITMIS].[dbo].[NielitCentreBatchFee] nb, [NIELITMIS].[dbo].[feeTypeMas] ft where nb.feeTypeId=ft.ID and (ft.feeType='Registration Fee' or ft.feeType='GST on Registration Fee') and batchID= " + application2.batchID;
                                        //    using (DataTable dt = GetApplicationDetails(batchdetailsfeetype))
                                        //        {
                                        //        if (dt.Rows.Count > 0)
                                        //            {
                                        //            int j;
                                        //            for (j = 0; j < dt.Rows.Count; j++)
                                        //                {
                                        //                NIELITStudentFeePaid nsf = new NIELITStudentFeePaid();
                                        //                nsf.enterDate = DateTime.Now;
                                        //                //nsf.enterBy = Convert.ToInt32(NielitCentreStudentId);
                                        //                Int32 enterby = 0;
                                        //                enterby = Convert.ToInt32(Session["UserID"]);
                                        //                if (enterby != 0) { nsf.enterBy = enterby; }
                                        //                nsf.feeTypeID = dt.Rows[j].Field<Int64>("feeTypeId");
                                        //                nsf.AmtPaid = dt.Rows[j].Field<int>("feeAmount");
                                        //                nsf.studentID = NielitCentreStudentId;
                                        //                nsf.paymentDate = DateTime.Now;
                                        //                nsf.Remarks = "Online Payment of Virtual Academy Registration";
                                        //                context.NIELITStudentFeePaids.Add(nsf);
                                        //                context.SaveChanges();
                                        //                }
                                        //            }
                                        //        }
                                        //    //}
                                        //    }


                                        //var application2 = context.virtualAcademyRegistration.Where(c => c.DemandNoteID == demandNote.ID).FirstOrDefault();
                                        ////var app2 = context.NIELITStudentFeePaids.Where(c => c.studentID == application2.ID).ToList();
                                        //if (application2 != null)
                                        //{
                                        //    //if (app2.Count() == 0)
                                        //    //{
                                        //    string OnlineRefNo = application2.Number;
                                        //    var nielitcentreStudentData = context.NielitCentreStudent.Where(k => k.batch_ID == application2.batchID && k.Number == OnlineRefNo).FirstOrDefault();
                                        //    Int64 NielitCentreStudentId = nielitcentreStudentData.ID;
                                        //    string batchdetailsfeetype = "select batchID,feeTypeId, feeAmount FROM [NIELITMIS].[dbo].[NielitCentreBatchFee] where batchID= " + application2.batchID;
                                        //    using (DataTable dt = GetApplicationDetails(batchdetailsfeetype))
                                        //    {
                                        //        if (dt.Rows.Count > 0)
                                        //        {
                                        //            int j;
                                        //            for (j = 0; j < dt.Rows.Count; j++)
                                        //            {
                                        //                NIELITStudentFeePaid nsf = new NIELITStudentFeePaid();
                                        //                nsf.enterDate = DateTime.Now;
                                        //                //nsf.enterBy = Convert.ToInt32(NielitCentreStudentId);
                                        //                Int32 enterby = 0;
                                        //                enterby = Convert.ToInt32(Session["UserID"]);
                                        //                if (enterby != 0) { nsf.enterBy = enterby; }
                                        //                nsf.feeTypeID = dt.Rows[j].Field<Int64>("feeTypeId");
                                        //                nsf.AmtPaid = dt.Rows[j].Field<int>("feeAmount");
                                        //                nsf.studentID = NielitCentreStudentId;
                                        //                nsf.paymentDate = DateTime.Now;
                                        //                nsf.Remarks = "Online Payment of Virtual Academy Registration";
                                        //                context.NIELITStudentFeePaids.Add(nsf);
                                        //                context.SaveChanges();
                                        //            }
                                        //        }
                                        //    }
                                        //    //}
                                        //}
                                        # endregion

                                        }                                    

                                    //saving demand note 
                                    context.Entry(demandNote).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();

                                    //saving online transaction
                                    context.Entry(online).State = System.Data.Entity.EntityState.Modified;
                                   context.SaveChanges();

                                    isManualSettledRecordcount = isManualSettledRecordcount + 1;


                                    #region Registration Process for NSQF Courses
                                    // deep 10 june start
                                    Int64 AadhaarNumberDecrypted = 0;
                                    var VAaPP = context.virtualAcademyRegistration.Where(c => c.DemandNoteID == demandNote.ID).FirstOrDefault();
                                    string virtualAcademyAppNumber = VAaPP.Number.ToString();
                                    string courseCatId = VAaPP.CourseCategoryID.ToString();

                                    if (courseCatId == "6") // 6 for Short Term Course (NSQF Aligned)
                                    {
                                        Int64 CraID = 0;
                                        AadhaarNumberDecrypted = Convert.ToInt64(EncryptDecrypt.DecryptString(VAaPP.AadharNumber.ToString()));

                                        CraID = InsertVARegDataToCourseRegApplication(virtualAcademyAppNumber, AadhaarNumberDecrypted);
                                    }
                                        //deep 10 june end
                                    #endregion

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

    }

