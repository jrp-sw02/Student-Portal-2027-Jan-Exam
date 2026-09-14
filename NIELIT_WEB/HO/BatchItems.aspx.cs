using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class BatchItems : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.Write(Request.QueryString.ToString());
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        lblError.Text = "";
        lblError.Visible = false;
        //btnsubmit.Visible = false;
        try
        {
            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Index.aspx");
            //currentRoleId = Convert.ToInt32(Session["RoleID"]);
            //if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/hoCoursesRegStatus.aspx"))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}
            //loginUserType = (UserType)Session["UserType"];
            //entityID = Convert.ToInt64(Session["EntityID"]);

            if (!Page.IsPostBack)
            {
                string bat = Request.QueryString["batchItemID"];
                string app = Request.QueryString["Appno"];
                if (!string.IsNullOrEmpty(Request.QueryString["batchItemID"]))
                {
                    ShowEditMode();
                }
                else if (!string.IsNullOrEmpty(Request.QueryString["Appno"]))
                {
                    ShowApplicantDetail();
                }
                else if (!String.IsNullOrEmpty(Request.QueryString["BatchID"]) && !String.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    divduplicaterecords.Visible = false;
                    ListItem lst = new ListItem("--All--", "0");
                    //EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlStatus, typeof(EConnect.NIELIT.enmBatchItemStatus), new ListItem("--Select All--", "0"));
                    ddlStatus.SelectedValue = Request.QueryString["status"].ToString();
                    if (!String.IsNullOrEmpty(Request.QueryString["status"]))
                    {
                        ViewState["SortField"] = "";
                        ViewState["SortOrder"] = "";
                        if (!string.IsNullOrEmpty(Request.QueryString["src"]))
                        {
                            BreadCrumb1.RemoveLastBreadCrumbItem();
                        }
                        BreadCrumb1.Render();
                        BindGridViewApplicant();
                        BindGridView();
                        showDetail();
                    }
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void ApplicationStatus()
    {
        try
        {

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally { }
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //string applicationNumber = txtappno.Text.Trim().ToUpper();
            string applicationNumber = "";
            Int16 addedCount = 0;
            //Int32 batchId = 0;
            Int32 statusUnderProcessing = Convert.ToInt32(enmBatchStatus.UnderProcessing);
            Int32 statusID = 0;
            string msg = "";

            using (EConnectContext context = new EConnectContext())
            {
                Int32 batchID = Convert.ToInt32(Request.QueryString["BatchID"]);
                Batch batch = context.Batchs.Find(batchID);
                BatchItem batchItem;
                Int32 applicantTypeID = batch.ApplicantTypeID;
                enmApplicationType applicationType = batch.enmApplicationType;

                #region------Course Registration Application------------
                if (applicationType == enmApplicationType.CourseRegistrationApplication)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        for (int i = 0; i < gbapplicant.Rows.Count; i++)
                        {
                            CheckBox cbx = (CheckBox)gbapplicant.Rows[i].FindControl("chkchild");
                            if (cbx != null)
                            {
                                if (cbx.Checked)
                                {
                                    applicationNumber = gbapplicant.DataKeys[i].Values[0].ToString();

                                    //If application exists in Course Registration Table
                                    if (context.CourseRegistrationApplications.Any(s => s.Number == applicationNumber && s.CourseCategoryID == batch.CourseCategoryID && s.CourseID == batch.CourseID && s.ApplicantTypeID == batch.ApplicantTypeID && s.ApplicableExamID == batch.ExamID && s.FinalSubmitted == true && s.BatchItemID == null))
                                    {
                                        var appl = context.CourseRegistrationApplications.Where(s => s.Number.ToUpper() == applicationNumber.ToUpper()).FirstOrDefault();
                                        enmCourseApplicationStatus applStatus = (enmCourseApplicationStatus)appl.ApplicationStatusID;
                                        if (applStatus == enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT || applStatus == enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT)
                                        {
                                            batchItem = new EConnect.NIELIT.BatchItem();
                                            batchItem.BatchID = batchID;
                                            //batchId = batchID;
                                            batchItem.CourseRegistrationApplicationID = appl.ID;
                                            batchItem.StatusID = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                                            //statusID = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                                            batchItem.CreatedByID = Convert.ToInt32(Session["UserID"]);
                                            batchItem.CreatedOn = Convert.ToDateTime(DateTime.Now);
                                            batchItem.Remarks = "Scanned On " + Convert.ToDateTime(DateTime.Now).ToString();
                                            context.BatchItems.Add(batchItem);
                                            context.SaveChanges();

                                            appl.BatchItemID = batchItem.ID;
                                            //DEEP add code on 27 may 2018
                                           
                                            appl.Registration_Process_Flag = "N";
                                           appl.Registration_Process_Flag_N_DT = Convert.ToDateTime(DateTime.Now);
                                            //deep end code on 27 may 2018

                                            appl.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);

                                            //Check if payment mode is Demand Draft  or NEFT Transaction
                                            if (appl.DemandNote.enmPaymentMode == enmPaymentMode.DemandDraft || appl.DemandNote.enmPaymentMode == enmPaymentMode.NEFTRTGS)
                                            {
                                                if (appl.DemandNote.enmPaymentStatus == enmPaymentStatus.PaidButNotVerified)
                                                {
                                                    appl.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.PaymentVerificationPending);
                                                    batchItem.StatusID = Convert.ToInt32(enmCourseApplicationStatus.PaymentVerificationPending);
                                                    statusID = Convert.ToInt32(enmCourseApplicationStatus.PaymentVerificationPending);
                                                }
                                            }
                                            context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                            context.SaveChanges();
                                            BreadCrumb1.Render();
                                            addedCount += 1;
                                        }
                                    }
                                }
                            }
                        }
                        scope.Complete();
                    };

                    ShowAlert("Total " + addedCount.ToString() + " applications has been scanned Successfully.", true);
                }

                #endregion-----End-CourseRegistrationApplication----------------

                #region-----CertificateExamApplication----------------
                else if (applicationType == enmApplicationType.CertificateExamApplication)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        for (int i = 0; i < gbapplicant.Rows.Count; i++)
                        {
                            CheckBox cbx = (CheckBox)gbapplicant.Rows[i].FindControl("chkchild");
                            if (cbx != null)
                            {
                                if (cbx.Checked)
                                {
                                    applicationNumber = gbapplicant.DataKeys[i].Values[0].ToString();

                                    //if (context.CertificateExamApplications.Any(s => s.Number == applicationNumber))
                                    //{
                                    if (context.CertificateExamApplications.Any(s => s.Number == applicationNumber && s.CourseCategoryID == batch.CourseCategoryID && s.CourseID == batch.CourseID && s.ApplicantTypeID == batch.ApplicantTypeID && s.ExamID == batch.ExamID && s.FinalSubmitted == true && s.BatchItemID == null && s.RegionalCenterID == batch.RegionalCenterID))
                                    {
                                        //if (context.CertificateExamApplications.Any(s => s.Number == applicationNumber && s.BatchItemID == null))
                                        //{
                                        var appl = context.CertificateExamApplications.Where(s => s.Number.ToUpper() == applicationNumber.ToUpper()).FirstOrDefault();
                                        enmCertificateExamApplicationStatus applStatus = (enmCertificateExamApplicationStatus)appl.ApplicationStatusID;
                                        if (applStatus == enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre || applStatus == enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre)
                                        {
                                            batchItem = new EConnect.NIELIT.BatchItem();
                                            batchItem.BatchID = batchID;
                                            //batchId = batchID;
                                            batchItem.CertificateExamApplicationID = appl.ID;
                                            batchItem.StatusID = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationReceivedByRegionalCentre);
                                            statusID = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationReceivedByRegionalCentre);
                                            batchItem.CreatedByID = Convert.ToInt32(Session["UserID"]);
                                            batchItem.CreatedOn = Convert.ToDateTime(DateTime.Now);
                                            batchItem.Remarks = "Scanned On " + Convert.ToDateTime(DateTime.Now).ToString();
                                            context.BatchItems.Add(batchItem);
                                            context.SaveChanges();

                                            //msg = "Application received and status set to " + EConnect.Utils.Common.EnumUtility.GetDescription(enmCertificateExamApplicationStatus.ApplicationReceivedByRegionalCentre);


                                            appl.BatchItemID = batchItem.ID;
                                            appl.ApplicationStatusID = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationReceivedByRegionalCentre);
                                            //Check if payment mode is Demand Draft  or NEFT Transaction
                                            if (appl.DemandNote.enmPaymentMode == enmPaymentMode.DemandDraft || appl.DemandNote.enmPaymentMode == enmPaymentMode.NEFTRTGS)
                                            {
                                                if (appl.DemandNote.enmPaymentStatus == enmPaymentStatus.PaidButNotVerified)
                                                {
                                                    appl.ApplicationStatusID = Convert.ToInt32(enmCertificateExamApplicationStatus.PaymentVerificationPending);
                                                    batchItem.StatusID = Convert.ToInt32(enmCertificateExamApplicationStatus.PaymentVerificationPending);
                                                    statusID = Convert.ToInt32(enmCertificateExamApplicationStatus.PaymentVerificationPending);
                                                    //msg = "Application received and status set to " + EConnect.Utils.Common.EnumUtility.GetDescription(enmCertificateExamApplicationStatus.PaymentVerificationPending);
                                                    //ShowAlert(msg, true);
                                                }
                                            }
                                            //ShowAlert(msg, true);
                                            context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                            context.Entry(batch).State = System.Data.Entity.EntityState.Modified;
                                            context.SaveChanges();
                                            //txtappno.Text = "";
                                            BreadCrumb1.Render();
                                            addedCount += 1;
                                        }
                                        //    else
                                        //    {
                                        //        msg = "This application can not be received. Current Status: ";
                                        //        if (applStatus == enmCertificateExamApplicationStatus.FeePaidByInstituteButApplicationPendingToDispatchToRegionalCentre && appl.PaymentStatusID == 1)
                                        //            msg += "Fee pending to be paid by Institute";
                                        //        else
                                        //            msg += EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                                        //        ShowAlert(msg, true);
                                        //        BreadCrumb1.Render();
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    msg = "This application has already been scanned.";
                                        //    ShowAlert(msg, true);
                                        //    BreadCrumb1.Render();
                                        //}
                                    }
                                }
                            }
                        }
                        scope.Complete();
                    };
                    ShowAlert("Total " + addedCount.ToString() + " applications has been scanned Successfully.", true);
                    //else
                    //{
                    //    var appl = context.CertificateExamApplications.Where(s => s.Number.ToUpper() == applicationNumber.ToUpper()).FirstOrDefault();
                    //    msg = "This application is not related to this Batch. Application details(Course: " + appl.CourseCategory.Code + "-" + appl.Course.Name + ", Applicant  Type: " + appl.enmApplicantType.ToString() + ", Regional Centre: " + appl.RegionalCenter.Name + ", Status: Is Pending to be submitted.)";
                    //    ShowAlert(msg, true);
                    //    BreadCrumb1.Render();
                    //}
                    //}
                    //else
                    //{
                    //    msg = "This application number does not exist/Invalid application number.";
                    //    ShowAlert(msg, true);
                    //    BreadCrumb1.Render();
                    //}
                }
                #endregion-----End-CertificateExamApplication----------------

                #region-----CourseExamApplication----------------
                else if (applicationType == enmApplicationType.CourseExamApplication)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        for (int i = 0; i < gbapplicant.Rows.Count; i++)
                        {
                            CheckBox cbx = (CheckBox)gbapplicant.Rows[i].FindControl("chkchild");
                            if (cbx != null)
                            {
                                if (cbx.Checked)
                                {
                                    applicationNumber = gbapplicant.DataKeys[i].Values[0].ToString();
                                    //if (context.CourseExamApplications.Any(s => s.Number == applicationNumber))
                                    //{
                                    //If application exists in Course Exam Application Table
                                    if (context.CourseExamApplications.Any(s => s.Number == applicationNumber && s.CourseCategoryID == batch.CourseCategoryID && s.CourseID == batch.CourseID && s.ApplicantTypeID == batch.ApplicantTypeID && s.ExamID == batch.ExamID && s.FinalSubmitted == true && s.BatchItemID == null))
                                    {

                                        //If Aleary scanned
                                        //if (context.CourseExamApplications.Any(s => s.Number == applicationNumber && s.BatchItemID == null))
                                        //{
                                        var appl = context.CourseExamApplications.Where(s => s.Number.ToUpper() == applicationNumber.ToUpper()).FirstOrDefault();
                                        enmCourseExamApplicationStatus applStatus = (enmCourseExamApplicationStatus)appl.ApplicationStatusID;
                                        enmPaymentMode paymentModeType = (enmPaymentMode)appl.DemandNote.PaymentModeID;
                                        if (applStatus == enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT || applStatus == enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT)
                                        {
                                            //if (appl.DemandNote.DDTransactionID != null)
                                            //{
                                            batchItem = new EConnect.NIELIT.BatchItem();
                                            batchItem.BatchID = batchID;
                                            //batchId = batchID;
                                            batchItem.CourseExamApplicationID = appl.ID;
                                            batchItem.StatusID = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);
                                            statusID = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);
                                            batchItem.CreatedByID = Convert.ToInt32(Session["UserID"]);
                                            batchItem.CreatedOn = Convert.ToDateTime(DateTime.Now);
                                            batchItem.Remarks = "Scanned On " + Convert.ToDateTime(DateTime.Now).ToString();
                                            context.BatchItems.Add(batchItem);
                                            context.SaveChanges();

                                            //msg = "Application received and status set to " + EConnect.Utils.Common.EnumUtility.GetDescription(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);


                                            appl.BatchItemID = batchItem.ID;
                                            appl.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);

                                            //Check if payment mode is Demand Draft or NEFT Transaction
                                            if (appl.DemandNote.enmPaymentMode == enmPaymentMode.DemandDraft || appl.DemandNote.enmPaymentMode == enmPaymentMode.NEFTRTGS)
                                            {
                                                if (appl.DemandNote.enmPaymentStatus == enmPaymentStatus.PaidButNotVerified)
                                                {
                                                    appl.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.PaymentVerificationPending);
                                                    batchItem.StatusID = Convert.ToInt32(enmCourseExamApplicationStatus.PaymentVerificationPending);
                                                    statusID = Convert.ToInt32(enmCourseExamApplicationStatus.PaymentVerificationPending);
                                                    //msg = "Application received and status set to " + EConnect.Utils.Common.EnumUtility.GetDescription(enmCourseExamApplicationStatus.PaymentVerificationPending);
                                                    //ShowAlert(msg, true);
                                                }
                                            }
                                            //ShowAlert(msg, true);
                                            context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                            context.Entry(batch).State = System.Data.Entity.EntityState.Modified;
                                            context.SaveChanges();
                                            //txtappno.Text = "";
                                            BreadCrumb1.Render();
                                            addedCount += 1;
                                            //}
                                            //else
                                            //{
                                            //    msg = "This application can not be received as Payment Mode is  " + EConnect.Utils.Common.EnumUtility.GetDescription(paymentModeType);
                                            //    ShowAlert(msg, true);
                                            //    BreadCrumb1.Render();
                                            //}
                                        }
                                        //    else
                                        //    {
                                        //        msg = "This application can not be received. Current status: " + EConnect.Utils.Common.EnumUtility.GetDescription(applStatus);
                                        //        ShowAlert(msg, true);
                                        //        BreadCrumb1.Render();
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    msg = "This application has already been scanned.";
                                        //    ShowAlert(msg, true);
                                        //    BreadCrumb1.Render();
                                        //}
                                    }
                                }
                            }
                        }
                        scope.Complete();
                    };
                    ShowAlert("Total " + addedCount.ToString() + " applications has been scanned Successfully.", true);
                    //    else
                    //    {
                    //        var appl = context.CourseExamApplications.Where(s => s.Number.ToUpper() == applicationNumber.ToUpper()).FirstOrDefault();
                    //        msg = "This application can not be received. Application details(Course: " + appl.CourseCategory.Code + "-" + appl.Course.Name + ", Applicant  Type: " + appl.enmApplicantType.ToString() + ")";
                    //        ShowAlert(msg, true);
                    //        BreadCrumb1.Render();
                    //    }
                    //}
                    //else
                    //{
                    //    msg = "This application number does not exist/Invalid application number.";
                    //    ShowAlert(msg, true);
                    //    BreadCrumb1.Render();
                    //}
                }
                #endregion-----End-CourseExamApplication----------------

                #region------Mercy Case Registration Application------------added by Komal
                if (applicationType == enmApplicationType.MercyCaseRegistration)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        for (int i = 0; i < gbapplicant.Rows.Count; i++)
                        {
                            CheckBox cbx = (CheckBox)gbapplicant.Rows[i].FindControl("chkchild");
                            if (cbx != null)
                            {
                                if (cbx.Checked)
                                {
                                    applicationNumber = gbapplicant.DataKeys[i].Values[0].ToString();

                                    //If application exists in Course Registration Table
                                    if (context.CourseRegistrationApplications.Any(s => s.Number == applicationNumber && s.CourseCategoryID == batch.CourseCategoryID && s.CourseID == batch.CourseID && s.ApplicantTypeID == batch.ApplicantTypeID && s.ApplicableExamID == batch.ExamID && s.FinalSubmitted == true && s.BatchItemID == null))
                                    {
                                        var appl = context.CourseRegistrationApplications.Where(s => s.Number.ToUpper() == applicationNumber.ToUpper()).FirstOrDefault();
                                        enmCourseApplicationStatus applStatus = (enmCourseApplicationStatus)appl.ApplicationStatusID;
                                        if (applStatus == enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT || applStatus == enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT)
                                        {
                                            batchItem = new EConnect.NIELIT.BatchItem();
                                            batchItem.BatchID = batchID;
                                            //batchId = batchID;
                                            batchItem.CourseRegistrationApplicationID = appl.ID;
                                            batchItem.StatusID = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                                            //statusID = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                                            batchItem.CreatedByID = Convert.ToInt32(Session["UserID"]);
                                            batchItem.CreatedOn = Convert.ToDateTime(DateTime.Now);
                                            batchItem.Remarks = "Scanned On " + Convert.ToDateTime(DateTime.Now).ToString();
                                            context.BatchItems.Add(batchItem);
                                            context.SaveChanges();

                                            appl.BatchItemID = batchItem.ID;
                                            //DEEP add code on 27 may 2018

                                            appl.Registration_Process_Flag = "N";
                                            appl.Registration_Process_Flag_N_DT = Convert.ToDateTime(DateTime.Now);
                                            //deep end code on 27 may 2018

                                            appl.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);

                                            if (appl.DemandNote.enmPaymentMode == enmPaymentMode.DemandDraft || appl.DemandNote.enmPaymentMode == enmPaymentMode.NEFTRTGS)
                                            {
                                                if (appl.DemandNote.enmPaymentStatus == enmPaymentStatus.PaidButNotVerified)
                                                {
                                                    appl.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.PaymentVerificationPending);
                                                    batchItem.StatusID = Convert.ToInt32(enmCourseApplicationStatus.PaymentVerificationPending);
                                                    statusID = Convert.ToInt32(enmCourseApplicationStatus.PaymentVerificationPending);
                                                }
                                            }
                                            context.Entry(appl).State = System.Data.Entity.EntityState.Modified;
                                            context.SaveChanges();
                                            BreadCrumb1.Render();
                                            addedCount += 1;
                                        }
                                    }
                                }
                            }
                        }
                        scope.Complete();
                    }
                    ;

                    ShowAlert("Total " + addedCount.ToString() + " applications has been scanned Successfully.", true);
                }

                #endregion-----End-MercyCaseRegistration----------------
            }
            ;
            BindGridView();
            BindGridViewApplicant();
            gbbatch.Visible = true;
            showDetail();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ShowEditMode()
    {
        try
        {

            using (EConnectContext context = new EConnectContext())
            {
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                btnMode.Visible = true;
                //btnsave.Text = "Update";
                lblHeading.Text = "Batch Processing";
                //Updating breadscrumb
                Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
                if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    Int64 ApplNo = Convert.ToInt64(Request.QueryString["Appno"]);
                    var cr = context.CourseRegistrationApplications.Find(ApplNo);
                    //CourseRegistrationApplication cr = context.CourseRegistrationApplications.Find(Convert.ToInt64(Request.QueryString["ApplID"]));
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cr.Name, "", ""));

                }
                if (ApplTypeID == Convert.ToInt32(enmApplicationType.MercyCaseRegistration))  // Mercy Case Registration
                {
                    Int64 ApplNo = Convert.ToInt64(Request.QueryString["Appno"]);
                    var cr = context.CourseRegistrationApplications.Find(ApplNo);
                    //CourseRegistrationApplication cr = context.CourseRegistrationApplications.Find(Convert.ToInt64(Request.QueryString["ApplID"]));
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cr.Name, "", ""));

                }
                if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    Int64 ApplNo = Convert.ToInt64(Request.QueryString["Appno"]);
                    var cr = context.CourseExamApplications.Find(ApplNo);
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cr.Candidate.Name, "", ""));
                }
                if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    Int64 ApplNo = Convert.ToInt64(Request.QueryString["Appno"]);
                    var cr = context.CertificateExamApplications.Find(ApplNo);
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cr.Name, "", ""));
                }

                candidateDetail.ApplicationTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
                candidateDetail.BatchItemID = Convert.ToInt64(Request.QueryString["batchItemID"]);
                candidateDetail.Bind();

                showDetail();
                //duplicaterecordsUserControl
                if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    divduplicaterecords.Visible = true;
                    DuplicateRecords1.ApplicationTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
                    DuplicateRecords1.BatchItemID = Convert.ToInt64(Request.QueryString["batchItemID"]);
                    DuplicateRecords1.BindRecords();
                }
                else
                {
                    divduplicaterecords.Visible = false;
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowApplicantDetail()
    {
        try
        {

            using (EConnectContext context = new EConnectContext())
            {
                btnMode.ViewMode = ToggleView.Mode.List;
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                ucSearchBar.Visible = false;
                btnMode.Visible = true;
                //btnsave.Text = "Update";
                lblHeading.Text = "Applicant Details";
                //Updating breadscrumb
                Int32 ApplTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
                if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    Int64 ApplNo = Convert.ToInt64(Request.QueryString["ID"]);
                    var cr = context.CourseRegistrationApplications.Find(ApplNo);
                    //CourseRegistrationApplication cr = context.CourseRegistrationApplications.Find(Convert.ToInt64(Request.QueryString["ApplID"]));
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cr.Name, "", ""));

                }
                if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    Int64 ApplNo = Convert.ToInt64(Request.QueryString["Appno"]);
                    var cr = context.CourseExamApplications.Find(ApplNo);
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cr.Candidate.Name, "", ""));
                }
                if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    Int64 ApplNo = Convert.ToInt64(Request.QueryString["Appno"]);
                    var cr = context.CertificateExamApplications.Find(ApplNo);
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cr.Name, "", ""));
                }
                if (ApplTypeID == Convert.ToInt32(enmApplicationType.MercyCaseRegistration))  // Mercy Case Registration
                {
                    Int64 ApplNo = Convert.ToInt64(Request.QueryString["ID"]);
                    var cr = context.CourseRegistrationApplications.Find(ApplNo);
                    //CourseRegistrationApplication cr = context.CourseRegistrationApplications.Find(Convert.ToInt64(Request.QueryString["ApplID"]));
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(cr.Name, "", ""));

                }

                candidateDetail.ApplicationTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
                candidateDetail.ApplicationId = Convert.ToInt64(Request.QueryString["ID"]);
                candidateDetail.Bind();

                //showDetail();
                //duplicaterecordsUserControl
                //if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                //{
                //    divduplicaterecords.Visible = true;
                //    DuplicateRecords1.ApplicationTypeID = Convert.ToInt32(Request.QueryString["ApplTypeID"]);
                //    //DuplicateRecords1.BatchItemID = Convert.ToInt64(Request.QueryString["batchItemID"]);
                //    DuplicateRecords1.BindRecords();
                //}
                //else
                //{
                //    divduplicaterecords.Visible = false;
                //}
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindGridViewApplicant()
    {
        try
        {
            Int32 batchID = Convert.ToInt32(Request.QueryString["BatchID"]);
            Int32 statusI = 0;
            Int32 statusD = 0;
            Int32 applTypeID = 0;
            Int32 PaidStatus = Convert.ToInt32(enmPaymentStatus.Paid);
            using (EConnectContext context = new EConnectContext())
            {
                Batch batch = context.Batchs.Find(batchID);
                if (batch.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                {
                    applTypeID = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                    statusI = Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
                    statusD = Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
                    var fillApplicantitem = from s in context.CourseRegistrationApplications.AsNoTracking()
                                            where s.ApplicableExamID == batch.ExamID && (s.ApplicationStatusID == statusI || s.ApplicationStatusID == statusD) && s.PaymentStatusID == PaidStatus && s.BatchItemID == null
                                            orderby s.Number
                                            select new
                                            {
                                                Appno = s.Number,
                                                ID = s.ID,
                                                ApplTypeID = applTypeID,
                                                Name = s.Name.ToUpper(),
                                                Status = s.ApplicationStatusID,
                                                //FatherName = s.FatherName.ToUpper(),
                                                //MotherName = s.MotherName.ToUpper(),
                                                //DateOfBirth = s.DateOfBirth,
                                                Appdate = s.ApplicationDate,                                                
                                                PaymentStatus = s.PaymentStatus.Name,
                                                PaymentMode = s.DemandNote.PaymentMode.Name,
                                                PaymentDetails = s.DemandNote.NEFTTransaction == null ? (s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "Transaction No:" + s.DemandNote.NEFTTransaction.TransactionNumber,

                                            };
                    PagingBar2.Bind(fillApplicantitem, ref gbapplicant);
                    uPnlGrid1.Update();
                    uPnlNavigation1.Update();
                    UpnlShow.Update();
                }
                else if (batch.enmApplicationType == enmApplicationType.CertificateExamApplication)
                {
                    applTypeID = Convert.ToInt32(enmApplicationType.CertificateExamApplication);
                    statusI = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre);
                    statusD = Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre);
                    var fillApplicantitem = from s in context.CertificateExamApplications
                                            where s.ExamID == batch.ExamID && (s.ApplicationStatusID == statusI || s.ApplicationStatusID == statusD) && s.PaymentStatusID == PaidStatus && s.BatchItemID == null
                                            orderby s.Number
                                            select new
                                            {
                                                ID = s.ID,
                                                Appno = s.Number,
                                                ApplTypeID = applTypeID,
                                                Status = s.ApplicationStatusID,
                                                Name = s.Name.ToUpper(),
                                                //FatherName = s.FatherName.ToUpper(),
                                                //MotherName = s.MotherName.ToUpper(),
                                                //DateOfBirth = s.DateOfBirth,
                                                Appdate = s.ApplicationDate,                                                
                                                PaymentStatus = s.PaymentStatus.Name,
                                                PaymentMode = s.DemandNote.PaymentMode.Name,
                                                PaymentDetails = s.DemandNote.NEFTTransaction == null ? (s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "Transaction No:" + s.DemandNote.NEFTTransaction.TransactionNumber,
                                            };
                    PagingBar2.Bind(fillApplicantitem, ref gbapplicant);
                    uPnlGrid1.Update();
                    uPnlNavigation1.Update();
                    UpnlShow.Update();
                }
                else if (batch.enmApplicationType == enmApplicationType.CourseExamApplication)
                {
                    applTypeID = Convert.ToInt32(enmApplicationType.CourseExamApplication);
                    statusI = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
                    statusD = Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
                    var fillApplicantitem = from s in context.CourseExamApplications
                                            where s.ExamID == batch.ExamID && (s.ApplicationStatusID == statusI || s.ApplicationStatusID == statusD) && s.PaymentStatusID == PaidStatus && s.BatchItemID == null
                                            orderby s.Number
                                            select new
                                            {
                                                ID = s.ID,
                                                Appno = s.Number,
                                                ApplTypeID = applTypeID,
                                                Status = s.ApplicationStatusID,
                                                Name = s.Candidate.Name.ToUpper(),
                                                Appdate = s.ApplicationDate,                                               
                                                PaymentStatus = s.PaymentStatus.Name,
                                                PaymentMode = s.DemandNote.PaymentMode.Name,
                                                PaymentDetails = s.DemandNote.NEFTTransaction == null ? (s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "Transaction No:" + s.DemandNote.NEFTTransaction.TransactionNumber,
                                            };
                    PagingBar2.Bind(fillApplicantitem, ref gbapplicant);
                    uPnlGrid1.Update();
                    uPnlNavigation1.Update();
                    UpnlShow.Update();
                }
                if (batch.enmApplicationType == enmApplicationType.MercyCaseRegistration)  // Mercy Case Registration
                {
                    applTypeID = Convert.ToInt32(enmApplicationType.MercyCaseRegistration);
                    statusI = Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
                    statusD = Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
                    var fillApplicantitem = from s in context.CourseRegistrationApplications.AsNoTracking()
                                            where s.ApplicableExamID == batch.ExamID && (s.ApplicationStatusID == statusI || s.ApplicationStatusID == statusD) && s.PaymentStatusID == PaidStatus && s.BatchItemID == null
                                            orderby s.Number
                                            select new
                                            {
                                                Appno = s.Number,
                                                ID = s.ID,
                                                ApplTypeID = applTypeID,
                                                Name = s.Name.ToUpper(),
                                                Status = s.ApplicationStatusID,
                                                //FatherName = s.FatherName.ToUpper(),
                                                //MotherName = s.MotherName.ToUpper(),
                                                //DateOfBirth = s.DateOfBirth,
                                                Appdate = s.ApplicationDate,
                                                PaymentStatus = s.PaymentStatus.Name,
                                                PaymentMode = s.DemandNote.PaymentMode.Name,
                                                PaymentDetails = s.DemandNote.NEFTTransaction == null ? (s.DemandNote.DemandDraftTransaction == null ? (s.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "Transaction No:" + s.DemandNote.NEFTTransaction.TransactionNumber,

                                            };
                    PagingBar2.Bind(fillApplicantitem, ref gbapplicant);
                    uPnlGrid1.Update();
                    uPnlNavigation1.Update();
                    UpnlShow.Update();
                }

                if (gbapplicant.Rows.Count > 0)
                    btnsubmit.Visible = true;
                else
                {
                    lblError.Visible = true;
                    //btnsubmit.Visible = false;
                    lblError.Text = "No record Found to Add in the Batch";
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void BindGridView()
    {
        try
        {
            //enmBatchItemStatus status = (enmBatchItemStatus)Convert.ToInt32(Request.QueryString["status"]);
            Int32 statusID = 0;
            Int32 batchID = Convert.ToInt32(Request.QueryString["BatchID"]);
            object status = 0;
            Int32 applTypeID = 0;
            using (EConnectContext context = new EConnectContext())
            {
                Batch batch = context.Batchs.Find(batchID);
                if (batch.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                {
                    applTypeID = Convert.ToInt32(enmApplicationType.CourseRegistrationApplication);
                    statusID = Convert.ToInt32(Request.QueryString["status"]);
                    status = (enmCourseApplicationStatus)statusID;
                    var fillBatchitem = from s in context.BatchItems
                                        join p in context.CourseRegistrationApplications on s.CourseRegistrationApplicationID equals p.ID 
                                        where s.BatchID == batchID                                        
                                        select new
                                        {
                                            ID = s.ID,
                                            BatchID = s.Batch.ID,
                                            status = statusID,
                                            ApplTypeID = applTypeID,
                                            AppID = p.ID,
                                            Appno = p.Number,                                           
                                            Name = p.Name.ToUpper(),
                                            //FatherName = s.CourseRegistrationApplication.FatherName.ToUpper(),
                                            //MotherName = s.CourseRegistrationApplication.MotherName.ToUpper(),
                                            //DateOfBirth = s.CourseRegistrationApplication.DateOfBirth,
                                            Appdate = p.ApplicationDate,
                                            Status1 = s.StatusID,
                                            PaymentStatus = p.PaymentStatus.Name,
                                            PaymentMode = p.DemandNote.PaymentMode.Name,
                                            //PaymentDetails = s.CourseRegistrationApplication.DemandNote.DemandDraftTransaction == null ? (s.CourseRegistrationApplication.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.CourseRegistrationApplication.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.CourseRegistrationApplication.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.CourseRegistrationApplication.DemandNote.DemandDraftTransaction.DemandDraftNumber,
                                            PaymentDetails = p.DemandNote.NEFTTransaction == null ? (p.DemandNote.DemandDraftTransaction == null ? (p.DemandNote.OnlineTransaction == null ? "Rcpt No:" + p.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + p.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + p.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "Transaction No:" + p.DemandNote.NEFTTransaction.TransactionNumber,                                            
                                            dupcount = 0
                                        };
                    if (statusID != 0)
                        fillBatchitem = fillBatchitem.Where(f => f.Status1 == statusID);
                    PagingBar1.Bind(fillBatchitem, ref gbbatch);
                    uPnlGrid.Update();
                    UpnlShow.Update();
                    uPnlNavigation.Update();
                    gbbatch.Columns[7].Visible = true;
                    gbbatch.Columns[8].Visible = true;
                }
                if (batch.enmApplicationType == enmApplicationType.CertificateExamApplication)
                {
                    statusID = Convert.ToInt32(Request.QueryString["status"]);
                    applTypeID = Convert.ToInt32(enmApplicationType.CertificateExamApplication);
                    status = (enmCertificateExamApplicationStatus)statusID;
                    var fillBatchitem = from s in context.BatchItems
                                        where s.BatchID == batchID
                                        orderby s.CertificateExamApplication.ID
                                        select new
                                        {
                                            ID = s.ID,
                                            BatchID = s.Batch.ID,
                                            status = statusID,
                                            AppID = s.CertificateExamApplication.ID,
                                            ApplTypeID = applTypeID,
                                            Appno = s.CertificateExamApplication.Number,                                            
                                            Name = s.CertificateExamApplication.Name.ToUpper(),
                                            //FatherName = s.CertificateExamApplication.FatherName.ToUpper(),
                                            //MotherName = s.CertificateExamApplication.MotherName.ToUpper(),
                                            //DateOfBirth = s.CertificateExamApplication.DateOfBirth,
                                            Appdate = s.CertificateExamApplication.ApplicationDate,
                                            Status1 = s.StatusID,
                                            PaymentStatus = s.CertificateExamApplication.PaymentStatus.Name,
                                            PaymentMode = s.CertificateExamApplication.DemandNote.PaymentMode.Name,
                                            //PaymentDetails = s.CertificateExamApplication.DemandNote.DemandDraftTransaction == null ? (s.CertificateExamApplication.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.CertificateExamApplication.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.CertificateExamApplication.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.CertificateExamApplication.DemandNote.DemandDraftTransaction.DemandDraftNumber,
                                            PaymentDetails = s.CertificateExamApplication.DemandNote.NEFTTransaction == null ? (s.CertificateExamApplication.DemandNote.DemandDraftTransaction == null ? (s.CertificateExamApplication.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.CertificateExamApplication.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.CertificateExamApplication.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.CertificateExamApplication.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "Transaction No:" + s.CertificateExamApplication.DemandNote.NEFTTransaction.TransactionNumber,                                            
                                            dupcount = 0
                                        };
                    if (statusID != 0)
                        fillBatchitem = fillBatchitem.Where(f => f.Status1 == statusID);
                    PagingBar1.Bind(fillBatchitem, ref gbbatch);
                    uPnlGrid.Update();
                    UpnlShow.Update();
                    uPnlNavigation.Update();
                    gbbatch.Columns[7].Visible = false;
                    gbbatch.Columns[8].Visible = false;
                }
                if (batch.enmApplicationType == enmApplicationType.CourseExamApplication)
                {
                    applTypeID = Convert.ToInt32(enmApplicationType.CourseExamApplication);
                    statusID = Convert.ToInt32(Request.QueryString["status"]);
                    status = (enmCourseExamApplicationStatus)statusID;
                    var fillBatchitem = from s in context.BatchItems
                                        where s.BatchID == batchID
                                        orderby s.CourseExamApplication.ID
                                        select new
                                        {
                                            ID = s.ID,
                                            AppID = s.CourseExamApplication.ID,
                                            Appno = s.CourseExamApplication.Number,
                                            ApplTypeID = applTypeID,
                                            Name = s.CourseExamApplication.Candidate.Name.ToUpper(),
                                            Appdate = s.CourseExamApplication.ApplicationDate,
                                            Status1 = s.StatusID,
                                            PaymentStatus = s.CourseExamApplication.PaymentStatus.Name,
                                            PaymentMode = s.CourseExamApplication.DemandNote.PaymentMode.Name,
                                            //PaymentDetails = s.CourseExamApplication.DemandNote.DemandDraftTransaction == null ? (s.CourseExamApplication.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.CourseExamApplication.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.CourseExamApplication.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.CourseExamApplication.DemandNote.DemandDraftTransaction.DemandDraftNumber,
                                            PaymentDetails = s.CourseExamApplication.DemandNote.NEFTTransaction == null ? (s.CourseExamApplication.DemandNote.DemandDraftTransaction == null ? (s.CourseExamApplication.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.CourseExamApplication.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.CourseExamApplication.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.CourseExamApplication.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "Transaction No:" + s.CourseExamApplication.DemandNote.NEFTTransaction.TransactionNumber,
                                            BatchID = s.BatchID,
                                            status = statusID,
                                            dupcount = 0
                                        };
                    if (statusID != 0)
                        fillBatchitem = fillBatchitem.Where(f => f.Status1 == statusID);
                    PagingBar1.Bind(fillBatchitem, ref gbbatch);
                    uPnlGrid.Update();
                    UpnlShow.Update();
                    uPnlNavigation.Update();
                    gbbatch.Columns[7].Visible = false;
                    gbbatch.Columns[8].Visible = false;
                }
                if (batch.enmApplicationType == enmApplicationType.MercyCaseRegistration)  // Mercy Case Registration
                {
                    applTypeID = Convert.ToInt32(enmApplicationType.MercyCaseRegistration);
                    statusID = Convert.ToInt32(Request.QueryString["status"]);
                    status = (enmCourseApplicationStatus)statusID;
                    var fillBatchitem = from s in context.BatchItems
                                        join p in context.CourseRegistrationApplications on s.CourseRegistrationApplicationID equals p.ID
                                        where s.BatchID == batchID
                                        select new
                                        {
                                            ID = s.ID,
                                            BatchID = s.Batch.ID,
                                            status = statusID,
                                            ApplTypeID = applTypeID,
                                            RegistrationTypeID = p.RegistrationTypeID,
                                            AppID = p.ID,
                                            Appno = p.Number,
                                            Name = p.Name.ToUpper(),
                                            //FatherName = s.CourseRegistrationApplication.FatherName.ToUpper(),
                                            //MotherName = s.CourseRegistrationApplication.MotherName.ToUpper(),
                                            //DateOfBirth = s.CourseRegistrationApplication.DateOfBirth,
                                            Appdate = p.ApplicationDate,
                                            Status1 = s.StatusID,
                                            PaymentStatus = p.PaymentStatus.Name,
                                            PaymentMode = p.DemandNote.PaymentMode.Name,
                                            //PaymentDetails = s.CourseRegistrationApplication.DemandNote.DemandDraftTransaction == null ? (s.CourseRegistrationApplication.DemandNote.OnlineTransaction == null ? "Rcpt No:" + s.CourseRegistrationApplication.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + s.CourseRegistrationApplication.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + s.CourseRegistrationApplication.DemandNote.DemandDraftTransaction.DemandDraftNumber,
                                            PaymentDetails = p.DemandNote.NEFTTransaction == null ? (p.DemandNote.DemandDraftTransaction == null ? (p.DemandNote.OnlineTransaction == null ? "Rcpt No:" + p.DemandNote.CSCTransaction.ResponseTransactionNumber : "Rcpt No:" + p.DemandNote.OnlineTransaction.ReferenceNumber) : "DD No: " + p.DemandNote.DemandDraftTransaction.DemandDraftNumber) : "Transaction No:" + p.DemandNote.NEFTTransaction.TransactionNumber,
                                            dupcount = 0
                                        };
                    if (statusID != 0)
                        fillBatchitem = fillBatchitem.Where(f => f.Status1 == statusID);
                    PagingBar1.Bind(fillBatchitem, ref gbbatch);
                    uPnlGrid.Update();
                    UpnlShow.Update();
                    uPnlNavigation.Update();
                    gbbatch.Columns[7].Visible = true;
                    gbbatch.Columns[8].Visible = true;
                }
                if (statusID == 0)
                {
                    //txtappno.Visible = false;
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ": All", "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                else if ((enmCourseApplicationStatus)status == enmCourseApplicationStatus.PaymentVerificationPending && applTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                {
                    //btnReject.Visible = true;
                    btnDelete.Visible = true;
                    btnKeepInAbeyance.Visible = true;
                    btnVerifyDD.Visible = true;
                    btnKeepInAbeyance.Visible = true;
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCourseApplicationStatus.PaymentVerificationPending.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                else if ((enmCourseApplicationStatus)status == enmCourseApplicationStatus.PaymentVerificationPending && applTypeID == Convert.ToInt32(enmApplicationType.MercyCaseRegistration)) // Mercy Case
                {
                    //btnReject.Visible = true;
                    btnDelete.Visible = true;
                    btnKeepInAbeyance.Visible = true;
                    btnVerifyDD.Visible = true;
                    btnKeepInAbeyance.Visible = true;
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCourseApplicationStatus.PaymentVerificationPending.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                else if ((enmCertificateExamApplicationStatus)status == enmCertificateExamApplicationStatus.PaymentVerificationPending && applTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                {
                    //btnReject.Visible = true;
                    btnDelete.Visible = true;
                    //btnKeepInAbeyance.Visible = true;
                    btnVerifyDD.Visible = true;
                    //btnKeepInAbeyance.Visible = true;
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCertificateExamApplicationStatus.PaymentVerificationPending.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                else if ((enmCourseExamApplicationStatus)status == enmCourseExamApplicationStatus.PaymentVerificationPending && applTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                {
                    //btnReject.Visible = true;
                    btnKeepInAbeyance.Visible = true;
                    btnDelete.Visible = true;
                    btnVerifyDD.Visible = true;
                    btnKeepInAbeyance.Visible = true;
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCourseExamApplicationStatus.PaymentVerificationPending.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                //else if ((enmCourseApplicationStatus)status == enmCourseApplicationStatus.PaymentVerificationPending && applTypeID == Convert.ToInt32(enmApplicationType.MercyCaseRegistration))  // Mercy Case Registration
                //{
                //    //btnReject.Visible = true;
                //    btnDelete.Visible = true;
                //    btnKeepInAbeyance.Visible = true;
                //    btnVerifyDD.Visible = true;
                //    btnKeepInAbeyance.Visible = true;
                //    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCourseApplicationStatus.PaymentVerificationPending.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                //}
                else if ((enmCourseApplicationStatus)status == enmCourseApplicationStatus.ApplicationReceivedByNIELIT || (enmCertificateExamApplicationStatus)status == enmCertificateExamApplicationStatus.ApplicationReceivedByRegionalCentre || (enmCourseExamApplicationStatus)status == enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)
                {
                    //btnReject.Visible = true;
                    btnDelete.Visible = true;
                    if (applTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                        btnKeepInAbeyance.Visible = false;
                    else
                        btnKeepInAbeyance.Visible = true;
                    btnVerify.Visible = true;
                    //btnKeepInAbeyance.Visible = true;
                    //txtappno.Visible = true;
                    //btnsubmit.Visible = true;
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCourseApplicationStatus.ApplicationReceivedByNIELIT.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                else if ((enmCourseApplicationStatus)status == enmCourseApplicationStatus.ApplicationVerifiedByInstitute)
                {
                    btnVerify.Visible = true;
                    //btnReject.Visible = true;
                    btnKeepInAbeyance.Visible = true;
                    btnKeepInAbeyance.Visible = true;
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCourseApplicationStatus.ApplicationVerifiedByInstitute.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                else if ((enmCourseExamApplicationStatus)status == enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute)
                {
                    btnVerify.Visible = true;
                    //btnReject.Visible = true;
                    btnKeepInAbeyance.Visible = true;
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCourseExamApplicationStatus.ApplicationVerifiedByInstitute.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                else if ((enmCourseApplicationStatus)status == enmCourseApplicationStatus.ApplicationFoundDublicate)
                {
                    //btnReject.Visible = true;
                    btnKeepInAbeyance.Visible = true;
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCourseApplicationStatus.ApplicationFoundDublicate.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                else if ((enmCourseExamApplicationStatus)status == enmCourseExamApplicationStatus.ApplicationFoundDuplicate)
                {
                    //btnReject.Visible = true;
                    btnKeepInAbeyance.Visible = true;
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCourseExamApplicationStatus.ApplicationFoundDuplicate.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                else if ((enmCertificateExamApplicationStatus)status == enmCertificateExamApplicationStatus.ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing)
                {
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCertificateExamApplicationStatus.ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                else if ((enmCourseApplicationStatus)status == enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing)
                {
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                else if ((enmCourseExamApplicationStatus)status == enmCourseExamApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToExaminationWing)
                {
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCourseExamApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToExaminationWing.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                else if ((enmCourseApplicationStatus)status == enmCourseApplicationStatus.ApplicationRejectedWithReason)
                {
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCourseApplicationStatus.ApplicationRejectedWithReason.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                else if ((enmCourseExamApplicationStatus)status == enmCourseExamApplicationStatus.ApplicationRejectedWithReason)
                {
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCourseExamApplicationStatus.ApplicationRejectedWithReason.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                else if ((enmCertificateExamApplicationStatus)status == enmCertificateExamApplicationStatus.ApplicationRejectedWithReason)
                {
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCertificateExamApplicationStatus.ApplicationRejectedWithReason.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                else if ((enmCourseApplicationStatus)status == enmCourseApplicationStatus.KeptInAbeyance)
                {
                    lblErrorMsg.Visible = true;
                    lblErrorMsg.Text = "Please go to Kept In Abeyance Application Form for further processing of Kept in Abeyance applications.";
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem(batch.Course.Code + ":  " + enmCourseApplicationStatus.KeptInAbeyance.ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                }
                if (gbbatch.Rows.Count == 0)
                {
                    if ((enmCourseApplicationStatus)status == enmCourseApplicationStatus.ApplicationReceivedByNIELIT || (enmCertificateExamApplicationStatus)status == enmCertificateExamApplicationStatus.ApplicationReceivedByRegionalCentre || (enmCourseExamApplicationStatus)status == enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT)
                    {
                        //txtappno.Visible = true;
                        //btnsubmit.Visible = true;
                    }
                    else
                    {
                        //txtappno.Visible = false;
                        //btnsubmit.Visible = false;
                        lblError.Text = "No data found.";
                        lblError.Visible = true;
                    }
                    btnMprocessed.Visible = false;
                    //btnReject.Visible = false;
                    btnDelete.Visible = false;
                    btnKeepInAbeyance.Visible = false;
                    btnVerify.Visible = false;
                    btnMasNotReject.Visible = false;
                    lblErrorMsg.Visible = false;
                    btnVerifyDD.Visible = false;
                    PagingBar1.Visible = false;
                    btnDelete.Visible = false;
                }
                else
                {
                    PagingBar1.Visible = true;
                }

                //}
                //else
                //{
                //    lblError.Text = "No data found.";
                //    lblError.Visible = true;
                //    btnMprocessed.Visible = false;
                //    btnMasNotReject.Visible = false;
                //    btnReject.Visible = false;
                //    btnVerify.Visible = false;
                //    btnMduplicate.Visible = false;
                //    if (statusID == 0)
                //        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Applications ( " + batch.Course.Code + ") : All", "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                //    else
                //        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Applications ( " + batch.Course.Code + ") :  " + ((enmCourseApplicationStatus)status).ToString(), "HO/BatchItems.aspx?" + Request.QueryString.ToString(), ""));
                //}

            };

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gbbatch.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void PageIndexChanged2(Int32 NewPageIndex)
    {
        try
        {
            gbapplicant.PageIndex = PagingBar2.CurrentPageIndex;
            BindGridViewApplicant();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        if (btnMode.ViewMode == ToggleView.Mode.New)
        {
            btnMode.ViewMode = ToggleView.Mode.List;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            //Change the heading text as required
            lblHeading.Text = "New User";
        }
        else
        {
            BreadCrumb1.RemoveLastBreadCrumbItem();
            BreadCrumb1.RemoveLastBreadCrumbItem();
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("BatchItems.aspx?BatchID=" + Request.QueryString["BatchID"].ToString() + "&Status=" + Request.QueryString["Status"]), true);
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gbbatch.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SearchBar_Reset(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gbbatch.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SaveRecord(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //create and object 
                //User objUser;
                if (String.IsNullOrEmpty(Request.QueryString["Key"]))
                {

                    strMessage = "New record saved.";
                }
                else
                {
                    ////Initialize current object by loading it and get its current modified date
                    strMessage = "Record updated.";
                }
            }
            //Call save method
            //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
            //Redirect it to list mode
            Response.Redirect("BatchItems.aspx?msg=" + strMessage);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void AllyFilter(object sender, EventArgs e)
    {
        try
        {
            PagingBar1.CurrentPageIndex = 0;
            gbbatch.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ResetFilterPanel(object sender, EventArgs e)
    {
        try
        {
            //ddlSearchUserType.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gbbatch.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void PerformPopupAction(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            hfActionID.Value = "";
            ShowAlert(ex.Message, true);
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count)
    {
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            var courses = from s in context.Courses
                          where s.CourseTypeID == courseType
                          select new { Name = s.Name };
            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Name.ToUpper().Contains(searchString));
            }
            courses = courses.OrderBy(s => s.Name);

            //var users1 = from s in context.Users
            //            select new { Name = s.LoginID };
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    users1 = users1.Where(s => s.Name.ToUpper().Contains(searchString));
            //}
            //courses = courses.Union(users1).Take(count);
            foreach (var course in courses)
            {
                items.Add(course.Name);
            }
            return items.ToArray();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally { context.Dispose(); }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("BatchItems.aspx", true);
    }
    protected void gbbatch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            CourseRegistrationApplication crs = new CourseRegistrationApplication();
            BatchItem batchItem = new BatchItem();
            Int32 count = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink hl2 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);
                HyperLink hl3 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);
                HyperLink hl4 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl);
                HyperLink hl5 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl);
                HyperLink hl6 = (HyperLink)e.Row.Cells[6].Controls[0];
                hl6.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl6.NavigateUrl);

                Int64 batchItemID = Convert.ToInt64(gbbatch.DataKeys[e.Row.RowIndex].Values[0]);
                using (EConnectContext context = new EConnectContext())
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["BatchID"]))
                    {
                        Int32 batchID = Convert.ToInt32(Request.QueryString["BatchID"]);
                        Batch batch = context.Batchs.Find(batchID);
                        if (batch.enmApplicationType == enmApplicationType.CourseRegistrationApplication)
                        {
                            if (batchItemID != 0 && batchItemID != null)
                            {
                                batchItem = context.BatchItems.Find(batchItemID);
                                crs = context.CourseRegistrationApplications.Find(batchItem.CourseRegistrationApplicationID);
                            }
                            if (crs != null)
                            {

                                if (String.IsNullOrEmpty(crs.GuardianName) || String.IsNullOrWhiteSpace(crs.GuardianName))
                                {

                                    var candidatelist = (from s in context.Candidates
                                                         where (
                                                             (s.Name.ToUpper() == crs.Name.ToUpper() && s.DateOfBirth == crs.DateOfBirth && s.Gender.ToUpper() == crs.Gender.ToUpper()) ||
                                                             (s.Name.ToUpper() == crs.Name.ToUpper() && s.FatherName.ToUpper() == crs.FatherName.ToUpper() && s.Gender.ToUpper() == crs.Gender.ToUpper())
                                                             )
                                                         select s).ToList();
                                    if (crs.CandidateID != 0 && crs.CandidateID != null)
                                    {
                                        candidatelist = candidatelist.Where(s => s.ID != crs.CandidateID).ToList();
                                    }
                                    count = candidatelist.Count();
                                }
                                else
                                {

                                    var candidatelist = (from s in context.Candidates
                                                         where (
                                                             (s.Name.ToUpper() == crs.Name.ToUpper() && s.DateOfBirth == crs.DateOfBirth && s.Gender.ToUpper() == crs.Gender.ToUpper()) ||
                                                             (s.Name.ToUpper() == crs.Name.ToUpper() && s.GuardianName.ToUpper() == crs.GuardianName.ToUpper() && s.Gender.ToUpper() == crs.Gender.ToUpper())
                                                             )
                                                         select s).ToList();
                                    if (crs.CandidateID != 0 && crs.CandidateID != null)
                                    {
                                        candidatelist = candidatelist.Where(s => s.ID != crs.CandidateID).ToList();
                                    }
                                    count = candidatelist.Count();
                                }
                            }
                            HyperLink hl7 = (HyperLink)e.Row.Cells[7].Controls[0];
                            hl7.Text = count.ToString();
                            hl7.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl7.NavigateUrl);

                            //to check whether application can be linked or not
                            HyperLink hl8 = (HyperLink)e.Row.Cells[8].FindControl("hllnkapp");
                            Int32 regtypeid = Convert.ToInt32(enmRegistrationType.New);
                            Int32 RegNoAllotedStatus = Convert.ToInt32(enmCourseApplicationStatus.RegistrationNumberAlloted);
                            if (crs.AlreadyRegistered == false && crs.RegistrationTypeID.Value == regtypeid && crs.IsLinked == false && crs.ApplicationStatusID != RegNoAllotedStatus)
                            {
                                hl8.Visible = true;
                                hl8.NavigateUrl = "LinkApplication.aspx?batchItemID=" + batchItemID + "&BatchID=" + batchID + "&status=" + Request.QueryString["status"].ToString();
                            }
                            else
                            {
                                hl8.Visible = true;
                                hl8.Text = "Linked";
                            }
                        }
                    }
                };
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                //Image imgAction = (Image)e.Row.FindControl("imgAction");
                //imgAction.ID = "imgAction_" + gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString() + "$" + gvMain.DataKeys[e.Row.RowIndex].Values[1].ToString();   //[e.Row.RowIndex].Value.ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gbapplicant_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl.NavigateUrl);
                HyperLink hl2 = (HyperLink)e.Row.Cells[2].Controls[0];
                hl2.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl2.NavigateUrl);
                HyperLink hl3 = (HyperLink)e.Row.Cells[3].Controls[0];
                hl3.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl3.NavigateUrl);
                HyperLink hl4 = (HyperLink)e.Row.Cells[4].Controls[0];
                hl4.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl4.NavigateUrl);
                HyperLink hl5 = (HyperLink)e.Row.Cells[5].Controls[0];
                hl5.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl5.NavigateUrl);
                HyperLink hl6 = (HyperLink)e.Row.Cells[6].Controls[0];
                hl6.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl6.NavigateUrl);


                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar2.CurrentPageSize * PagingBar2.CurrentPageIndex)).ToString();
            }
        }

        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnVerify_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int16 verifiedCount = 0;
                //Added 19 July 2019
                Int16 verifiedNotCount = 0;
                //
                Int64 batchItemID = 0;
                Batch batch = context.Batchs.Find(Convert.ToInt32(Request.QueryString["BatchID"]));
                enmApplicationType applicationType = batch.enmApplicationType;
                for (int i = 0; i < gbbatch.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                    if (cbx != null)
                    {
                        if (cbx.Checked)
                        {
                            batchItemID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                            BatchItem batchItem = context.BatchItems.Find(batchItemID);
                            if (batchItem != null)
                            {
                                CourseRegistrationApplication cr = new CourseRegistrationApplication();

                                cr = context.CourseRegistrationApplications.Find(batchItem.CourseRegistrationApplicationID.Value);
                                int CandidateTypeId = cr.ApplicantTypeID;
                                int BatchId = batchItem.BatchID;
                                int CourseId = cr.CourseID;
                                Int64 regno = 0;
                                Int64 candidateID = 0;


                                var checkstatus = (from a in context.CourseRegistrationApplications
                                                   where a.BatchItemID == batchItemID
                                                   select a).FirstOrDefault();

                                //Added 05-Jul-2019
                                if ((checkstatus.FatherName ==null && checkstatus.MotherName ==null) && (!checkstatus.affidavitVerified || checkstatus.affidavitUpload == null))
                                {
                                    // lblMsg.Text = "Affidavit not submitted or not verified";
                                    //  lblMsg.Visible = true;
                                    checkstatus.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance);
                                    batchItem.StatusID = Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance);
                                    batchItem.CourseRegistrationApplication.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance);
                                    batchItem.KeptInAbeyanceReason = "Affidavit not verified";
                                    batchItem.IsKeptInAbeyance = true;
                                    // context.SaveChanges();
                                    context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                                    verifiedNotCount += 1;
                                    continue;
                                }
                                //

                                verifiedCount += 1;
                                if (batchItem.StatusID == Convert.ToInt32(enmCertificateExamApplicationStatus.PaymentVerificationPending))
                                {

                                }
                                if (applicationType == enmApplicationType.CourseRegistrationApplication)
                                {
                                    Int32 ApplicationVerifiedByNIELITAndForwardedToRegistrationWing = Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing);
                                    batchItem.StatusID = ApplicationVerifiedByNIELITAndForwardedToRegistrationWing;
                                    batchItem.CourseRegistrationApplication.ApplicationStatusID = ApplicationVerifiedByNIELITAndForwardedToRegistrationWing;
                                    context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                                    //context.SaveChanges();
                                }
                               else if (applicationType == enmApplicationType.MercyCaseRegistration)  // Mercy Case
                                {
                                    Int32 ApplicationVerifiedByNIELITAndForwardedToRegistrationWing = Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing);
                                    batchItem.StatusID = ApplicationVerifiedByNIELITAndForwardedToRegistrationWing;
                                    batchItem.CourseRegistrationApplication.ApplicationStatusID = ApplicationVerifiedByNIELITAndForwardedToRegistrationWing;
                                    context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                                    //context.SaveChanges();
                                }
                                else if (applicationType == enmApplicationType.CertificateExamApplication)
                                {
                                    Int32 ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing);
                                    batchItem.StatusID = ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing;
                                    batchItem.CertificateExamApplication.ApplicationStatusID = ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing;
                                    context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                                    //context.SaveChanges();
                                }
                                else if (applicationType == enmApplicationType.CourseExamApplication)
                                {
                                    Int32 ApplicationVerifiedByNIELITAndForwardedToExaminationWing = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToExaminationWing);
                                    batchItem.StatusID = ApplicationVerifiedByNIELITAndForwardedToExaminationWing;
                                    batchItem.CourseExamApplication.ApplicationStatusID = ApplicationVerifiedByNIELITAndForwardedToExaminationWing;
                                    context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                                    //context.SaveChanges();
                                }
                            }
                        }
                    }
                }
                context.SaveChanges();
                //Added 19 July 2019
                if (verifiedNotCount > 0)
                    ShowAlert(verifiedCount.ToString() + " applications verified successfully. Please check Kept in Abeyance for Batch ", true);
                else
                    ShowAlert(verifiedCount.ToString() + " applications verified successfully. ", true);
                //ShowAlert(verifiedCount.ToString() + " applications verified successfully. ", true);
            };         
            BindGridView();
            gbbatch.Visible = true;
            showDetail();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnVerifyDD_Click(object sender, EventArgs e)
    {
        try
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
            using (EConnectContext context = new EConnectContext())
            {
                Int16 verifiedCount = 0;
                Int16 demanddraft = 0;
                Int16 neft = 0;
                Int64 batchItemID = 0;
                Batch batch = context.Batchs.Find(Convert.ToInt32(Request.QueryString["BatchID"]));
                enmApplicationType applicationType = batch.enmApplicationType;
                //enmPaymentMode paymentModeType = (enmPaymentMode)appl.DemandNote.PaymentModeID;
                Int32 paymentModeType = Convert.ToInt32(enmPaymentMode.DemandDraft);
                Int32 paymentModeType1 = Convert.ToInt32(enmPaymentMode.NEFTRTGS);
                for (int i = 0; i < gbbatch.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                    if (cbx != null)
                    {
                        if (cbx.Checked)
                        {
                            batchItemID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                            BatchItem batchItem = context.BatchItems.Find(batchItemID);
                            if (batchItem != null)
                            {
                                verifiedCount += 1;
                                if (batchItem.StatusID == Convert.ToInt32(enmCertificateExamApplicationStatus.PaymentVerificationPending))
                                {

                                }
                                if (applicationType == enmApplicationType.CourseRegistrationApplication)
                                {
                                    if (batchItem.CourseRegistrationApplication.DemandNote.PaymentModeID == paymentModeType)
                                    {
                                        demanddraft += 1;
                                        Int32 ApplicationReceivedByNIELIT = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                                        batchItem.CourseRegistrationApplication.DemandNote.DemandDraftTransaction.IsVerified = true;
                                        batchItem.CourseRegistrationApplication.DemandNote.DemandDraftTransaction.VerificatonDate = DateTime.Now;
                                        batchItem.CourseRegistrationApplication.DemandNote.DemandDraftTransaction.VerifiedByID = Convert.ToInt32(Session["UserID"]);
                                        batchItem.StatusID = ApplicationReceivedByNIELIT;
                                        batchItem.CourseRegistrationApplication.ApplicationStatusID = ApplicationReceivedByNIELIT;
                                        batchItem.CourseRegistrationApplication.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                        batchItem.CourseRegistrationApplication.DemandNote.PaymentStatusID = batchItem.CourseRegistrationApplication.PaymentStatusID.Value;
                                    }
                                    else if (batchItem.CourseRegistrationApplication.DemandNote.PaymentModeID == paymentModeType1)
                                    {
                                        neft += 1;
                                        Int32 ApplicationReceivedByNIELIT = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                                        batchItem.CourseRegistrationApplication.DemandNote.NEFTTransaction.IsVerified = true;
                                        batchItem.CourseRegistrationApplication.DemandNote.NEFTTransaction.VerificatonDate = DateTime.Now;
                                        batchItem.CourseRegistrationApplication.DemandNote.NEFTTransaction.VerifiedByID = Convert.ToInt32(Session["UserID"]);
                                        batchItem.StatusID = ApplicationReceivedByNIELIT;
                                        batchItem.CourseRegistrationApplication.ApplicationStatusID = ApplicationReceivedByNIELIT;
                                        batchItem.CourseRegistrationApplication.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                        batchItem.CourseRegistrationApplication.DemandNote.PaymentStatusID = batchItem.CourseRegistrationApplication.PaymentStatusID.Value;
                                    }
                                }
                                else if (applicationType == enmApplicationType.MercyCaseRegistration)    // Mercy Case
                                {
                                    if (batchItem.CourseRegistrationApplication.DemandNote.PaymentModeID == paymentModeType)
                                    {
                                        demanddraft += 1;
                                        Int32 ApplicationReceivedByNIELIT = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                                        batchItem.CourseRegistrationApplication.DemandNote.DemandDraftTransaction.IsVerified = true;
                                        batchItem.CourseRegistrationApplication.DemandNote.DemandDraftTransaction.VerificatonDate = DateTime.Now;
                                        batchItem.CourseRegistrationApplication.DemandNote.DemandDraftTransaction.VerifiedByID = Convert.ToInt32(Session["UserID"]);
                                        batchItem.StatusID = ApplicationReceivedByNIELIT;
                                        batchItem.CourseRegistrationApplication.ApplicationStatusID = ApplicationReceivedByNIELIT;
                                        batchItem.CourseRegistrationApplication.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                        batchItem.CourseRegistrationApplication.DemandNote.PaymentStatusID = batchItem.CourseRegistrationApplication.PaymentStatusID.Value;
                                    }
                                    else if (batchItem.CourseRegistrationApplication.DemandNote.PaymentModeID == paymentModeType1)
                                    {
                                        neft += 1;
                                        Int32 ApplicationReceivedByNIELIT = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                                        batchItem.CourseRegistrationApplication.DemandNote.NEFTTransaction.IsVerified = true;
                                        batchItem.CourseRegistrationApplication.DemandNote.NEFTTransaction.VerificatonDate = DateTime.Now;
                                        batchItem.CourseRegistrationApplication.DemandNote.NEFTTransaction.VerifiedByID = Convert.ToInt32(Session["UserID"]);
                                        batchItem.StatusID = ApplicationReceivedByNIELIT;
                                        batchItem.CourseRegistrationApplication.ApplicationStatusID = ApplicationReceivedByNIELIT;
                                        batchItem.CourseRegistrationApplication.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                        batchItem.CourseRegistrationApplication.DemandNote.PaymentStatusID = batchItem.CourseRegistrationApplication.PaymentStatusID.Value;
                                    }
                                }
                                else if (applicationType == enmApplicationType.CertificateExamApplication)
                                {
                                    if (batchItem.CertificateExamApplication.DemandNote.PaymentModeID == paymentModeType)
                                    {
                                        demanddraft += 1;
                                        Int32 ApplicationReceivedByRegionalCentre = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationReceivedByRegionalCentre);
                                        batchItem.CertificateExamApplication.DemandNote.DemandDraftTransaction.IsVerified = true;
                                        batchItem.CertificateExamApplication.DemandNote.DemandDraftTransaction.VerificatonDate = DateTime.Now;
                                        batchItem.CertificateExamApplication.DemandNote.DemandDraftTransaction.VerifiedByID = Convert.ToInt32(Session["UserID"]);
                                        batchItem.StatusID = ApplicationReceivedByRegionalCentre;
                                        batchItem.CertificateExamApplication.ApplicationStatusID = ApplicationReceivedByRegionalCentre;
                                        batchItem.CertificateExamApplication.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                        batchItem.CertificateExamApplication.DemandNote.PaymentStatusID = batchItem.CertificateExamApplication.PaymentStatusID;
                                    }
                                    else if (batchItem.CertificateExamApplication.DemandNote.PaymentModeID == paymentModeType1)
                                    {
                                        neft += 1;
                                        Int32 ApplicationReceivedByRegionalCentre = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationReceivedByRegionalCentre);
                                        batchItem.CertificateExamApplication.DemandNote.NEFTTransaction.IsVerified = true;
                                        batchItem.CertificateExamApplication.DemandNote.NEFTTransaction.VerificatonDate = DateTime.Now;
                                        batchItem.CertificateExamApplication.DemandNote.NEFTTransaction.VerifiedByID = Convert.ToInt32(Session["UserID"]);
                                        batchItem.StatusID = ApplicationReceivedByRegionalCentre;
                                        batchItem.CertificateExamApplication.ApplicationStatusID = ApplicationReceivedByRegionalCentre;
                                        batchItem.CertificateExamApplication.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                        batchItem.CertificateExamApplication.DemandNote.PaymentStatusID = batchItem.CertificateExamApplication.PaymentStatusID;
                                    }
                                }
                                else if (applicationType == enmApplicationType.CourseExamApplication)
                                {
                                    if (batchItem.CourseExamApplication.DemandNote.PaymentModeID == paymentModeType)
                                    {
                                        demanddraft += 1;
                                        Int32 ApplicationReceivedByNIELIT = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);
                                        batchItem.CourseExamApplication.DemandNote.DemandDraftTransaction.IsVerified = true;
                                        batchItem.CourseExamApplication.DemandNote.DemandDraftTransaction.VerificatonDate = DateTime.Now;
                                        batchItem.CourseExamApplication.DemandNote.DemandDraftTransaction.VerifiedByID = Convert.ToInt32(Session["UserID"]);
                                        batchItem.StatusID = ApplicationReceivedByNIELIT;
                                        batchItem.CourseExamApplication.ApplicationStatusID = ApplicationReceivedByNIELIT;
                                        batchItem.CourseExamApplication.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                        batchItem.CourseExamApplication.DemandNote.PaymentStatusID = batchItem.CourseExamApplication.PaymentStatusID;
                                    }
                                    else if (batchItem.CourseExamApplication.DemandNote.PaymentModeID == paymentModeType1)
                                    {
                                        neft += 1;
                                        Int32 ApplicationReceivedByNIELIT = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);
                                        batchItem.CourseExamApplication.DemandNote.NEFTTransaction.IsVerified = true;
                                        batchItem.CourseExamApplication.DemandNote.NEFTTransaction.VerificatonDate = DateTime.Now;
                                        batchItem.CourseExamApplication.DemandNote.NEFTTransaction.VerifiedByID = Convert.ToInt32(Session["UserID"]);
                                        batchItem.StatusID = ApplicationReceivedByNIELIT;
                                        batchItem.CourseExamApplication.ApplicationStatusID = ApplicationReceivedByNIELIT;
                                        batchItem.CourseExamApplication.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                        batchItem.CourseExamApplication.DemandNote.PaymentStatusID = batchItem.CourseExamApplication.PaymentStatusID;
                                    }
                                }
                                context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                    }
                }
                //scope.Complete();
                //ShowAlert("Demand Draft / NEFT Transaction details has been verified of " + verifiedCount.ToString() + " applications.", true);
                ShowAlert("Out of " + verifiedCount.ToString() + " applications marked for payment verification, " + demanddraft + " applications of Demand Draft transaction and " + neft + " application of NEFT Transaction have been verified", true);
            };
            //};
            BindGridView();
            gbbatch.Visible = true;
            showDetail();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        try
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
            using (EConnectContext context = new EConnectContext())
            {
                Int16 RejectedCount = 0;
                Int64 batchItemID = 0;
                Batch batch = context.Batchs.Find(Convert.ToInt32(Request.QueryString["BatchID"]));
                enmApplicationType applicationType = batch.enmApplicationType;
                for (int i = 0; i < gbbatch.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                    if (cbx != null)
                    {
                        if (cbx.Checked)
                        {
                            batchItemID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                            BatchItem batchItem = context.BatchItems.Find(batchItemID);
                            if (batchItem != null)
                            {
                                RejectedCount += 1;
                                if (applicationType == enmApplicationType.CourseRegistrationApplication)
                                {
                                    Int32 ApplicationRejectedWithReason = Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedWithReason);
                                    batchItem.StatusID = ApplicationRejectedWithReason;
                                    batchItem.UpdatedByID = Convert.ToInt32(Session["UserID"]);
                                    batchItem.UpdatedOn = Convert.ToDateTime(DateTime.Now);
                                    batchItem.RejectedByID = Convert.ToInt32(Session["UserID"]);
                                    batchItem.RejectedOn = Convert.ToDateTime(DateTime.Now);
                                    batchItem.IsRejected = true;
                                    batchItem.CourseRegistrationApplication.ApplicationStatusID = ApplicationRejectedWithReason;
                                    context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                }
                                else if (applicationType == enmApplicationType.CertificateExamApplication)
                                {
                                    //Int32 ApplicationRejectedWithReason = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationRejectedWithReason);
                                    //batchItem.StatusID = ApplicationRejectedWithReason;
                                    //batchItem.UpdatedByID = Convert.ToInt32(Session["UserID"]);
                                    //batchItem.UpdatedOn = Convert.ToDateTime(DateTime.Now);
                                    //batchItem.RejectedByID = Convert.ToInt32(Session["UserID"]);
                                    //batchItem.RejectedOn = Convert.ToDateTime(DateTime.Now);
                                    //batchItem.IsRejected = true;
                                    //batchItem.CertificateExamApplication.ApplicationStatusID = ApplicationRejectedWithReason;
                                    //context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                                    //context.SaveChanges();
                                }
                                else if (applicationType == enmApplicationType.CourseExamApplication)
                                {
                                    //Int32 ApplicationRejectedWithReason = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationRejectedWithReason);
                                    //batchItem.StatusID = ApplicationRejectedWithReason;
                                    //batchItem.UpdatedByID = Convert.ToInt32(Session["UserID"]);
                                    //batchItem.UpdatedOn = Convert.ToDateTime(DateTime.Now);
                                    //batchItem.RejectedByID = Convert.ToInt32(Session["UserID"]);
                                    //batchItem.RejectedOn = Convert.ToDateTime(DateTime.Now);
                                    //batchItem.IsRejected = true;
                                    //batchItem.CourseExamApplication.ApplicationStatusID = ApplicationRejectedWithReason;
                                    //context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                                    //context.SaveChanges();
                                }
                                context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                    }
                }
                //scope.Complete();
                ShowAlert(RejectedCount.ToString() + " Applications Rejected Successfully.", true);
            };
            //};
            BindGridView();
            showDetail();
            gbbatch.Visible = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnMprocessed_Click(object sender, EventArgs e)
    {

    }
    protected void btnMasNotReject_Click(object sender, EventArgs e)
    {

    }
    protected void btnKeepInAbeyance_Click(object sender, EventArgs e)
    {
        try
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
            using (EConnectContext context = new EConnectContext())
            {
                Int16 KeepInAbeyanceCount = 0;
                Int64 batchItemID = 0;
                Batch batch = context.Batchs.Find(Convert.ToInt32(Request.QueryString["BatchID"]));
                enmApplicationType applicationType = batch.enmApplicationType;
                for (int i = 0; i < gbbatch.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                    if (cbx != null)
                    {
                        if (cbx.Checked)
                        {
                            batchItemID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                            BatchItem batchItem = context.BatchItems.Find(batchItemID);
                            if (batchItem != null)
                            {
                                KeepInAbeyanceCount += 1;
                                if (applicationType == enmApplicationType.CourseRegistrationApplication)
                                {
                                    Int32 KeptInAbeyance = Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance);
                                    batchItem.StatusID = KeptInAbeyance;
                                    batchItem.UpdatedByID = Convert.ToInt32(Session["UserID"]);
                                    batchItem.UpdatedOn = Convert.ToDateTime(DateTime.Now);
                                    batchItem.KeptInAbeyanceOn = Convert.ToDateTime(DateTime.Now);
                                    batchItem.IsKeptInAbeyance = true;
                                    batchItem.CourseRegistrationApplication.ApplicationStatusID = KeptInAbeyance;
                                    context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                }
                                else if (applicationType == enmApplicationType.CertificateExamApplication)
                                {
                                    //Int32 KeptInAbeyance = Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance);
                                    //batchItem.StatusID = KeptInAbeyance;
                                    //batchItem.UpdatedByID = Convert.ToInt32(Session["UserID"]);
                                    //batchItem.UpdatedOn = Convert.ToDateTime(DateTime.Now);
                                    //batchItem.CertificateExamApplication.ApplicationStatusID = KeptInAbeyance;
                                    //context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                                    //context.SaveChanges();
                                }
                                else if (applicationType == enmApplicationType.CourseExamApplication)
                                {
                                    //Int32 KeptInAbeyance = Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance);
                                    //batchItem.StatusID = KeptInAbeyance;
                                    //batchItem.UpdatedByID = Convert.ToInt32(Session["UserID"]);
                                    //batchItem.UpdatedOn = Convert.ToDateTime(DateTime.Now);
                                    //batchItem.CourseExamApplication.ApplicationStatusID = KeptInAbeyance;
                                    //context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                                    //context.SaveChanges();
                                }
                                context.Entry(batchItem).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                            }
                        }
                    }
                }
                //scope.Complete();
                if (KeepInAbeyanceCount > 1)
                    ShowAlert(KeepInAbeyanceCount.ToString() + " Applications are Kept in Abeyance Case.", true);
                else
                    ShowAlert(KeepInAbeyanceCount.ToString() + " Application is Kept in Abeyance Case.", true);
            };
            //};
            BindGridView();
            showDetail();
            gbbatch.Visible = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void hlkreceived_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.RemoveLastBreadCrumbItem();
            LinkButton btn = (LinkButton)sender;
            Int32 BatchId = Convert.ToInt32(Request.QueryString["BatchID"]);
            Response.Redirect(EConnect.Utils.Security.QuertStringModule.Encrypt("BatchItems.aspx?BatchID=" + BatchId + "&status=" + btn.CommandArgument));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void showDetail()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 applTypeID = 0;
                Int32 BatchId = Convert.ToInt32(Request.QueryString["BatchID"]);
                Int32 batchStatusID = (from p in context.Batchs
                                       where p.ID == BatchId
                                       select p.StatusID).FirstOrDefault();
                string batchNumber = "";
                var batch = (from p in context.Batchs
                             where p.ID == BatchId
                             select new
                             {
                                 BatchNumber = p.Number,
                                 BatchDate = p.CreatedOn,
                                 ApplicantType = p.ApplicantType.Name,
                                 CourseName = p.CourseCategory.Code + "-" + p.Course.Code,
                                 ApplicationType = p.ApplicationType.Name,
                                 applTypeID = p.ApplicationTypeID,
                                 batchStatusID = p.StatusID
                             }).FirstOrDefault();
                batchNumber = batch.BatchNumber;
                applTypeID = batch.applTypeID;
                //Retreiving Application count application status vise
                //Total Applications in current batch
                var AppCount = (from a in context.BatchItems
                                where a.BatchID == BatchId
                                select a).Count();
                hktotal.Text = AppCount.ToString();
                //hlkreceived.CommandArgument = "0";

                Int32 PendingPaymentStatusID = Convert.ToInt32(enmPaymentStatus.Pending);
                Int32 DemandDraftPaymentMode = Convert.ToInt32(enmPaymentMode.DemandDraft);
                Int32 PaymentVerificationPending = Convert.ToInt32(enmCourseApplicationStatus.PaymentVerificationPending);
                Int32 ApplicationReceivedByNIELIT = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                Int32 ApplicationReceivedByRegionalCentre = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationReceivedByRegionalCentre);
                Int32 ApplicationFoundDublicate = Convert.ToInt32(enmCourseApplicationStatus.ApplicationFoundDublicate);
                Int32 KeptInAbeyance = Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance);
                Int32 ApplicationRejectedWithReason = Convert.ToInt32(enmCourseApplicationStatus.ApplicationRejectedWithReason);
                Int32 ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing);
                Int32 ApplicationVerifiedByNIELITAndForwardedToRegistrationWing = Convert.ToInt32(enmCourseApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToRegistrationWing);
                Int32 ApplicationVerifiedByNIELITAndForwardedToExaminationWing = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationVerifiedByNIELITAndForwardedToExaminationWing);

                //ApplicationReceivedByNIELIT/RegionalCenter
                if ((enmApplicationType)applTypeID == enmApplicationType.CourseRegistrationApplication)
                {
                    var receivedCount = (from b in context.BatchItems
                                         where b.BatchID == BatchId &&
                                         b.StatusID == ApplicationReceivedByNIELIT
                                         select b).Count();
                    hlkreceived.Text = receivedCount.ToString();
                    //hlkreceived.CommandArgument = Convert.ToInt16(ApplicationReceivedByNIELIT).ToString();

                }
                else if ((enmApplicationType)applTypeID == enmApplicationType.MercyCaseRegistration)  // Mercy Case
                {
                    var receivedCount = (from b in context.BatchItems
                                         where b.BatchID == BatchId &&
                                         b.StatusID == ApplicationReceivedByNIELIT
                                         select b).Count();
                    hlkreceived.Text = receivedCount.ToString();
                    //hlkreceived.CommandArgument = Convert.ToInt16(ApplicationReceivedByNIELIT).ToString();

                }
                else if ((enmApplicationType)applTypeID == enmApplicationType.CertificateExamApplication)
                {
                    var receivedCount = (from b in context.BatchItems
                                         where b.BatchID == BatchId &&
                                         b.StatusID == ApplicationReceivedByRegionalCentre
                                         select b).Count();
                    hlkreceived.Text = receivedCount.ToString();
                    //hlkreceived.CommandArgument = Convert.ToInt16(ApplicationReceivedByRegionalCentre).ToString();

                }
                else if ((enmApplicationType)applTypeID == enmApplicationType.CourseExamApplication)
                {
                    var receivedCount = (from b in context.BatchItems
                                         where b.BatchID == BatchId &&
                                         b.StatusID == ApplicationReceivedByNIELIT
                                         select b).Count();
                    hlkreceived.Text = receivedCount.ToString();
                    //hlkreceived.CommandArgument = Convert.ToInt16(ApplicationReceivedByNIELIT).ToString();

                }

                //ApplicationRejectedWithReason
                var rejectedcount = (from b in context.BatchItems
                                     where b.BatchID == BatchId &&
                                     b.StatusID == ApplicationRejectedWithReason
                                     select b).Count();
                hlkreject.Text = rejectedcount.ToString();
                //hlkreject.CommandArgument = Convert.ToInt16(ApplicationRejectedWithReason).ToString();

                //KeptInAbeyance
                var keptInAbeyancecount = (from b in context.BatchItems
                                           where b.BatchID == BatchId &&
                                           b.StatusID == KeptInAbeyance
                                           select b).Count();
                hlKeptInAbeyance.Text = keptInAbeyancecount.ToString();
                //hlKeptInAbeyance.CommandArgument = Convert.ToInt16(KeptInAbeyance).ToString();

                //DemandDraftVerificationPending
                var demandVerificationPendingCount = (from b in context.BatchItems
                                                      where b.BatchID == BatchId &&
                                                     b.StatusID == PaymentVerificationPending
                                                      select b).Count();
                hlkpendingDDVerify.Text = demandVerificationPendingCount.ToString();
                //hlkpendingDDVerify.CommandArgument = Convert.ToInt16(PaymentVerificationPending).ToString();

                //ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing
                if ((enmApplicationType)applTypeID == enmApplicationType.CourseRegistrationApplication)
                {
                    var verifiedCount = (from b in context.BatchItems
                                         where b.BatchID == BatchId &&
                                        b.StatusID == ApplicationVerifiedByNIELITAndForwardedToRegistrationWing
                                         select b).Count();
                    hlkprocess.Text = verifiedCount.ToString();
                    //hlkprocess.CommandArgument = Convert.ToInt16(ApplicationVerifiedByNIELITAndForwardedToRegistrationWing).ToString();
                }
                else if ((enmApplicationType)applTypeID == enmApplicationType.MercyCaseRegistration)  // Mercy Case
                {
                    var verifiedCount = (from b in context.BatchItems
                                         where b.BatchID == BatchId &&
                                        b.StatusID == ApplicationVerifiedByNIELITAndForwardedToRegistrationWing
                                         select b).Count();
                    hlkprocess.Text = verifiedCount.ToString();
                    //hlkprocess.CommandArgument = Convert.ToInt16(ApplicationVerifiedByNIELITAndForwardedToRegistrationWing).ToString();
                }
                else if ((enmApplicationType)applTypeID == enmApplicationType.CertificateExamApplication)
                {
                    var verifiedCount = (from b in context.BatchItems
                                         where b.BatchID == BatchId &&
                                        b.StatusID == ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing
                                         select b).Count();
                    hlkprocess.Text = verifiedCount.ToString();
                    //hlkprocess.CommandArgument = Convert.ToInt16(ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing).ToString();
                }
                else if ((enmApplicationType)applTypeID == enmApplicationType.CourseExamApplication)
                {
                    var verifiedCount = (from b in context.BatchItems
                                         where b.BatchID == BatchId &&
                                        b.StatusID == ApplicationVerifiedByNIELITAndForwardedToExaminationWing
                                         select b).Count();
                    hlkprocess.Text = verifiedCount.ToString();
                    //hlkprocess.CommandArgument = Convert.ToInt16(ApplicationVerifiedByNIELITAndForwardedToExaminationWing).ToString();
                }
                //ApplicationFoundDublicate
                var duplicateCount = (from b in context.BatchItems
                                      where b.BatchID == BatchId &&
                                     b.StatusID == ApplicationFoundDublicate
                                      select b).Count();
                hlkduplicate.Text = duplicateCount.ToString();
                //hlkduplicate.CommandArgument = Convert.ToInt16(ApplicationFoundDublicate).ToString();
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
            using (EConnectContext context = new EConnectContext())
            {
                Int16 deletedCount = 0;
                Int64 batchItemID = 0;
                Batch batch = context.Batchs.Find(Convert.ToInt32(Request.QueryString["BatchID"]));
                enmApplicationType applicationType = batch.enmApplicationType;
                for (int i = 0; i < gbbatch.Rows.Count; i++)
                {
                    CheckBox cbx = (CheckBox)gbbatch.Rows[i].FindControl("chk");
                    if (cbx != null)
                    {
                        if (cbx.Checked)
                        {
                            batchItemID = Convert.ToInt64(gbbatch.DataKeys[i].Values[0]);
                            BatchItem batchItem = context.BatchItems.Find(batchItemID);
                            if (batchItem != null)
                            {
                                deletedCount += 1;
                                if (applicationType == enmApplicationType.CourseRegistrationApplication)
                                {
                                    CourseRegistrationApplication cr = context.CourseRegistrationApplications.Find(batchItem.CourseRegistrationApplicationID);
                                    if (batch.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                                    {
                                        Int32 FeeDepositedByCandidateButApplicationNotReceivedByNIELIT = Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
                                        cr.ApplicationStatusID = FeeDepositedByCandidateButApplicationNotReceivedByNIELIT;
                                        cr.BatchItemID = null;
                                        context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                        context.BatchItems.Remove(batchItem);
                                        context.SaveChanges();
                                    }
                                    else
                                    {
                                        Int32 ApplicationDispatchedByTheInstituteToNIELIT = Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
                                        cr.ApplicationStatusID = ApplicationDispatchedByTheInstituteToNIELIT;
                                        cr.BatchItemID = null;
                                        context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                        context.BatchItems.Remove(batchItem);
                                        context.SaveChanges();
                                    }
                                }
                                else if (applicationType == enmApplicationType.MercyCaseRegistration)   // Mercy case
                                {
                                    CourseRegistrationApplication cr = context.CourseRegistrationApplications.Find(batchItem.CourseRegistrationApplicationID);
                                    if (batch.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                                    {
                                        Int32 FeeDepositedByCandidateButApplicationNotReceivedByNIELIT = Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
                                        cr.ApplicationStatusID = FeeDepositedByCandidateButApplicationNotReceivedByNIELIT;
                                        cr.BatchItemID = null;
                                        context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                        context.BatchItems.Remove(batchItem);
                                        context.SaveChanges();
                                    }
                                    else
                                    {
                                        Int32 ApplicationDispatchedByTheInstituteToNIELIT = Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
                                        cr.ApplicationStatusID = ApplicationDispatchedByTheInstituteToNIELIT;
                                        cr.BatchItemID = null;
                                        context.Entry(cr).State = System.Data.Entity.EntityState.Modified;
                                        context.BatchItems.Remove(batchItem);
                                        context.SaveChanges();
                                    }
                                }
                                else if (applicationType == enmApplicationType.CertificateExamApplication)
                                {
                                    CertificateExamApplication ce = context.CertificateExamApplications.Find(batchItem.CertificateExamApplicationID);
                                    if (batch.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                                    {
                                        Int32 FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre = Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre);
                                        ce.ApplicationStatusID = FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre;
                                        ce.BatchItemID = null;
                                        context.Entry(ce).State = System.Data.Entity.EntityState.Modified;
                                        context.BatchItems.Remove(batchItem);
                                        context.SaveChanges();
                                    }
                                    else
                                    {
                                        Int32 ApplicationDispatchedByTheInstituteToRegionalCentre = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre);
                                        ce.ApplicationStatusID = ApplicationDispatchedByTheInstituteToRegionalCentre;
                                        ce.BatchItemID = null;
                                        context.Entry(ce).State = System.Data.Entity.EntityState.Modified;
                                        context.BatchItems.Remove(batchItem);
                                        context.SaveChanges();
                                    }
                                }
                                else if (applicationType == enmApplicationType.CourseExamApplication)
                                {
                                    CourseExamApplication cexam = context.CourseExamApplications.Find(batchItem.CourseExamApplicationID);
                                    if (batch.ApplicantTypeID == Convert.ToInt32(enmApplicantType.Direct))
                                    {
                                        Int32 FeeDepositedByCandidateButApplicationNotReceivedByNIELIT = Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT);
                                        cexam.ApplicationStatusID = FeeDepositedByCandidateButApplicationNotReceivedByNIELIT;
                                        cexam.BatchItemID = null;
                                        context.Entry(cexam).State = System.Data.Entity.EntityState.Modified;
                                        context.BatchItems.Remove(batchItem);
                                        context.SaveChanges();
                                    }
                                    else
                                    {
                                        Int32 ApplicationDispatchedByTheInstituteToNIELIT = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT);
                                        cexam.ApplicationStatusID = ApplicationDispatchedByTheInstituteToNIELIT;
                                        cexam.BatchItemID = null;
                                        context.Entry(cexam).State = System.Data.Entity.EntityState.Modified;
                                        context.BatchItems.Remove(batchItem);
                                        context.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
                //scope.Complete();
                ShowAlert(deletedCount.ToString() + " Applications Deleted Successfully.", true);
            };
            //};
            BindGridViewApplicant();
            BindGridView();
            showDetail();
            gbbatch.Visible = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
}