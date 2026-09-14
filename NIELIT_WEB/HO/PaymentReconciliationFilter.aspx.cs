using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Data;

public partial class HO_PaymentReconciliation : BasePage
{
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
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
            if (!IsPostBack)
            {

                BindApplicantType();
                BindPaymentMode();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Payment Reconciliation", "#", ""));
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindApplicantType()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.ApplicationTypes
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlApplicationType, Category, lst);
            };

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindPaymentMode()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 multicheque = Convert.ToInt32(enmPaymentMode.MultiCityCheque);
                Int32 cash = Convert.ToInt32(enmPaymentMode.Cash);
                //Int32 DemandDraft = Convert.ToInt32(enmPaymentMode.DemandDraft); && p.ID != DemandDraft
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.PaymentModes
                               where p.ID != multicheque && p.ID != cash
                               orderby (p.Name)
                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlPaymentMode, Category, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindGrid()
    {
        try
        {
            int PayModeId = Convert.ToInt32(ddlPaymentMode.SelectedValue);
            int AppTypeId = Convert.ToInt32(ddlApplicationType.SelectedValue);
            string TransactionStatus = ddlTransactionStatus.SelectedValue;
            DateTime PayFromDate = Convert.ToDateTime(txtDateFrom.Text);
            DateTime PayToDate = Convert.ToDateTime(txtToDate.Text);
            Int32 CSCSPV = Convert.ToInt32(enmPaymentMode.CSCSPV);
            Int32 Online = Convert.ToInt32(enmPaymentMode.Online);
            Int32 DemandDraft = Convert.ToInt32(enmPaymentMode.DemandDraft);
            Int32 NEFTRTGS = Convert.ToInt32(enmPaymentMode.NEFTRTGS);
            //String strSql = "";
            StringBuilder mySql = new StringBuilder();
            if (PayModeId == Online)
            {
                mySql.Append(" select distinct o.Reference_Number as TransactionNumber,d.ID as demandNoteID, o.ID as TranID,d.Payment_Mode_ID as PayModeId,d.Application_Type_ID as AppTypeId,");
                if (AppTypeId != 0)
                {
                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication) || AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        mySql.Append("cr.Number as ApplicationNumber,");
                    else
                        mySql.Append("cr.Appl_Number as ApplicationNumber,");
                }
                mySql.Append(" o.Response_Date as TransactionDate,(CONVERT(VARCHAR(80),d.ID) + ' / ' +  replace(convert(char(11),d.Date,113),' ','-')) as DemandNoteDetail" +
                                " ,d.Amount as DemandNoteAmount," +
                                " o.Amount as TransactionAmount" +
                                " from Online_Transaction o,Application_Type a ,Demand_Note d ");
                if (AppTypeId != 0)
                {
                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    {
                        mySql.Append(" right outer join Course_Exam_Application cr" +
                                     " on d.ID = cr.Demand_Note_ID");
                         if (TransactionStatus == "SUNV")
                        mySql.Append(" where cr.Application_Status_ID in (" + Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) +","+                            
                              Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT) + " ," + Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification)+ ") and ");
                         else
                              mySql.Append(" where " );
                    }
                    else if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                    {  mySql.Append("right outer join Certificate_Exam_Application cr" +
                                  " on d.ID = cr.Demand_Note_ID ");
                         if (TransactionStatus == "SUNV")
                         mySql.Append(" where cr.Application_Status_ID in (" + Convert.ToInt32(enmCertificateExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByRegionalCentre) +","+
                              Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationDispatchedByTheInstituteToRegionalCentre) + "," + Convert.ToInt32(enmCertificateExamApplicationStatus.AppliedButPendingForInstituteVerification) + "," + Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationVerifiedByRegionalCentreAndForwardedToExaminationWing) + ") and ");
                         else
                              mySql.Append(" where " );
                    }
                    else if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                    {
                        mySql.Append("right outer join Course_Registration_Application cr" +
                                 " on d.ID = cr.Demand_Note_ID ");
                         if (TransactionStatus == "SUNV")
                         mySql.Append(" where cr.Application_Status_ID in (" + Convert.ToInt32(enmCourseApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) +","+                            
                              Convert.ToInt32(enmCourseApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT) + "," + Convert.ToInt32(enmCourseApplicationStatus.AppliedButPendingForInstituteVerification) +
                               "," + Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance) + ") and ");
                        else
                              mySql.Append(" where " );
                    }

                }

                mySql.Append(" " +
                  "  o.Demand_Note_ID = d.ID and  CAST(o.Request_Date as DATE)  >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and  CAST(o.Request_Date as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' and cr.Demand_Note_ID = o.Demand_Note_ID");
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
                mySql.Append(" select distinct o.Response_Number as TransactionNumber,d.ID as demandNoteID, o.ID as TranID,d.Payment_Mode_ID as PayModeId,d.Application_Type_ID as AppTypeId,");
                if (AppTypeId != 0)
                {
                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication) || AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        mySql.Append("cr.Number as ApplicationNumber,");
                    else
                        mySql.Append("cr.Appl_Number as ApplicationNumber,");
                }
                mySql.Append(" o.Date as TransactionDate,(CONVERT(VARCHAR(80),d.ID) + ' / ' +  replace(convert(char(11),d.Date,113),' ','-')) as DemandNoteDetail" +
                            " ,d.Amount as DemandNoteAmount," +
                            " o.Amount as TransactionAmount" +
                            " from CSC_Transaction o, Application_Type a,Demand_Note d ");
                if (AppTypeId != 0)
                {
                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                    {
                        mySql.Append(" right outer join Course_Exam_Application cr" +
                                     " on d.ID = cr.Demand_Note_ID");
                        if (TransactionStatus == "SUNV")
                            mySql.Append(" where cr.Application_Status_ID in (" + Convert.ToInt32(enmCourseExamApplicationStatus.FeeDepositedByCandidateButApplicationNotReceivedByNIELIT) + "," +
                                  Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationDispatchedByTheInstituteToNIELIT) + "," + Convert.ToInt32(enmCourseExamApplicationStatus.AppliedButPendingForInstituteVerification) + " ) and ");
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
                  "  o.Demand_Note_ID = d.ID and CAST(o.Date as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and CAST(o.Date as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' and cr.Demand_Note_ID = o.Demand_Note_ID ");
                if (TransactionStatus == "SNU")//Success and not Updated
                    mySql.Append(" and o.Response_Status = 0 and o.Response_Message is not null and o.Response_Number is not null" +
                        " and d.CSC_Transaction_ID is null");
                if (TransactionStatus == "SUV")//Success Updated and Verified
                    mySql.Append(" and o.Response_Status = 0 and o.Response_Message is not null and o.Response_Number is not null" +
                        " and d.CSC_Transaction_ID is not null and d.Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid));
                if (TransactionStatus == "SUNV")//Success and not Updated and Not Verified
                    mySql.Append(" and o.Response_Status = 0 and o.Response_Message is not null and o.Response_Number is not null" +
                        " and d.CSC_Transaction_ID is not null and d.Status_ID = " + Convert.ToInt32(enmPaymentStatus.PaidButNotVerified));
                if (TransactionStatus == "F")//Failed
                    mySql.Append(" and o.Response_Status = 1 ");
            }
            if (PayModeId == DemandDraft)
            {
                mySql.Append(" select distinct o.Dd_Number as TransactionNumber,d.ID as demandNoteID, o.ID as TranID,d.Payment_Mode_ID as PayModeId,d.Application_Type_ID as AppTypeId,");
                if (AppTypeId != 0)
                {
                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication) || AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        mySql.Append("cr.Number as ApplicationNumber,");
                    else
                        mySql.Append("cr.Appl_Number as ApplicationNumber,");
                }

                mySql.Append(" o.Date as TransactionDate,(CONVERT(VARCHAR(80),d.ID) + ' / ' +  replace(convert(char(11),d.Date,113),' ','-')) as DemandNoteDetail" +
                                " ,d.Amount as DemandNoteAmount," +
                                " o.DD_Amt as TransactionAmount" +
                                " from DemandDraftTransaction o,Application_Type a,Demand_Note d ");
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
                  "  o.Demand_Note_ID = d.ID and  CAST(o.Date as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and  CAST(o.Date as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' and cr.Demand_Note_ID = o.Demand_Note_ID");
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
            if (PayModeId == NEFTRTGS)
            {
                mySql.Append(" select distinct o.Transaction_Number as TransactionNumber,d.ID as demandNoteID, o.ID as TranID,d.Payment_Mode_ID as PayModeId,d.Application_Type_ID as AppTypeId,");
                if (AppTypeId != 0)
                {
                    if (AppTypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication) || AppTypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        mySql.Append("cr.Number as ApplicationNumber,");
                    else
                        mySql.Append("cr.Appl_Number as ApplicationNumber,");
                }

                mySql.Append(" o.Date as TransactionDate,(CONVERT(VARCHAR(80),d.ID) + ' / ' +  replace(convert(char(11),d.Date,113),' ','-')) as DemandNoteDetail" +
                                " ,d.Amount as DemandNoteAmount," +
                                " o.Transaction_Amt as TransactionAmount" +
                                " from NEFT_Transaction o,Application_Type a,Demand_Note d ");
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
                  "  o.Demand_Note_ID = d.ID and  CAST(o.Date as DATE) >= '" + PayFromDate.ToString("dd-MMM-yyyy") + "' and  CAST(o.Date as DATE) <= '" + PayToDate.ToString("dd-MMM-yyyy") + "' and cr.Demand_Note_ID = o.Demand_Note_ID");
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
            if (PayModeId == CSCSPV || PayModeId == Online || (PayModeId == DemandDraft && TransactionStatus != "F") || (PayModeId == NEFTRTGS && TransactionStatus != "F"))
            {
                DataTable dt = DbUtility.GetDataTable(mySql.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
                if (dt.Rows.Count > 0)
                {
                    enableDisableControls(false);
                    Div_GridPnl.Visible = true;
                    if (TransactionStatus == "SUNV")
                    {
                        BtnShowDetail.Visible = true;
                        BtnShowDetail.Text = "Verify";
                    }
                    else
                        BtnShowDetail.Visible = false;
                    lblError.Visible = false;
                    PagingBar1.Bind(dt, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                }
                else
                {
                    BtnShowDetail.Text = "Show Detail";
                    Div_GridPnl.Visible = false;
                    lblError.Visible = true;
                    lblError.Text = "No Record Found !";
                }
            }
            else
            {
                BtnShowDetail.Text = "Show Detail";
                Div_GridPnl.Visible = false;
                lblError.Visible = true;
                lblError.Text = "No Record Found !";
            }
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGrid();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected bool isValidForm()
    {
        try
        {
            if (ddlPaymentMode.SelectedValue == "0")
            {
                lblError.Visible = true;
                lblError.Text = "Please select Payment Mode!";
                return false;
            }
            if (txtDateFrom.Text == "")
            {
                ShowAlert("Please select From Date", true);
                txtDateFrom.Focus();
                return false;
            }
            if (!IsDate(txtDateFrom.Text))
            {
                ShowAlert("Invalid Date", true);
                txtDateFrom.Focus();
                return false;
            }
            DateTime todaydate = DateTime.Now;
            DateTime fromDate = Convert.ToDateTime(txtDateFrom.Text);

            int CurrentDateresult = DateTime.Compare(todaydate, fromDate);
            if (CurrentDateresult == -1)
            {
                lblError.Visible = true;
                lblError.Text = "From date should not be greater than Current Date!";
                txtDateFrom.Focus();
                return false;
            }


            if (txtToDate.Text == "")
            {
                ShowAlert("Please To Date", true);
                txtToDate.Focus();
                return false;
            }
            if (!IsDate(txtToDate.Text))
            {
                ShowAlert("Invalid Date", true);
                txtToDate.Focus();
                return false;
            }
            DateTime toDate = Convert.ToDateTime(txtToDate.Text);
            int CurrentDateresult1 = DateTime.Compare(todaydate, toDate);
            if (CurrentDateresult1 == -1)
            {
                lblError.Visible = true;
                lblError.Text = "To date should not be greater than Current Date!";
                txtToDate.Focus();
                return false;
            }
            if (ddlApplicationType.SelectedValue == "0")
            {
                lblError.Visible = true;
                lblError.Text = "Please select Application Type!";
                return false;
            }
            if (ddlTransactionStatus.SelectedValue == "0")
            {
                lblError.Visible = true;
                lblError.Text = "Please select Transaction Status!";
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
          
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
              
                //Encryption url of hypelink field
                HyperLink hl0 = (HyperLink)e.Row.Cells[1].Controls[0];
                hl0.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl0.NavigateUrl);
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
               
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void enableDisableControls(bool flag)
    {
        try 
        {
            ddlApplicationType.Enabled = flag;
            txtDateFrom.Enabled = flag;
            txtToDate.Enabled = flag;
            ddlPaymentMode.Enabled = flag;
            ddlTransactionStatus.Enabled = flag;
            imgFrom.Visible = flag;
            imgTo.Visible = flag;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ResetAll()
    {
        try
        {
            BtnShowDetail.Visible = true;
            BtnShowDetail.Text = "Show Detail";
            lblError.Visible = false;
            ddlTransactionStatus.SelectedValue = "0";
            Div_GridPnl.Visible = false;
            txtDateFrom.Text = "";
            txtToDate.Text = "";
            ddlApplicationType.SelectedValue = "0";
            ddlPaymentMode.SelectedValue = "0";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            enableDisableControls(true);
            ResetAll();
           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);

        }

    }
    protected void BtnShowDetail_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (BtnShowDetail.Text != "Verify")
            {
                if (isValidForm())
                    BindGrid();

            }
            else if (BtnShowDetail.Text == "Verify")
            {

                using (TransactionScope scope = new TransactionScope())
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        Int16 verifiedCount = 0;
                        Int64 demandNoteID = 0;
                        int applicationType = Convert.ToInt32(ddlApplicationType.SelectedValue);
                        for (int i = 0; i < gvMain.Rows.Count; i++)
                        {
                            CheckBox cbx = (CheckBox)gvMain.Rows[i].FindControl("chk");
                            if (cbx != null)
                            {
                                if (cbx.Checked)
                                {
                                    demandNoteID = Convert.ToInt64(gvMain.DataKeys[i].Values[0]);
                                    DemandNote demandNote = context.DemandNotes.Find(demandNoteID);

                                    if (demandNote != null)
                                    {
                                        verifiedCount += 1;
                                        demandNote.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                        if (applicationType == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                                        {
                                            var application = context.CertificateExamApplications.Where(s => s.DemandNoteID == demandNoteID);
                                            if (application != null)
                                            {
                                                //application.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                                //application.ApplicationStatusID = Convert.ToInt32(enmCertificateExamApplicationStatus.ApplicationReceivedByRegionalCentre);
                                                //context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                                                context.Database.ExecuteSqlCommand(" Update Certificate_Exam_Application set  Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demandNote.ID);
                                                context.SaveChanges();
                                            }
                                        }
                                        else if (applicationType == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                                        {

                                            var application = context.CourseRegistrationApplications.Where(s => s.DemandNoteID == demandNoteID);
                                            if (application != null)
                                            {
                                                //application.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                                //application.ApplicationStatusID = Convert.ToInt32(enmCourseApplicationStatus.ApplicationReceivedByNIELIT);
                                                //context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                                                context.Database.ExecuteSqlCommand(" Update Course_Registration_Application set  Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demandNote.ID);
                                                context.SaveChanges();
                                            }
                                        }
                                        else if (applicationType == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                                        {

                                            var application = context.CourseExamApplications.Where(s => s.DemandNoteID == demandNoteID);
                                            if (application != null)
                                            {
                                                //application.PaymentStatusID = Convert.ToInt32(enmPaymentStatus.Paid);
                                                //application.ApplicationStatusID = Convert.ToInt32(enmCourseExamApplicationStatus.ApplicationReceivedByNIELIT);
                                                //context.Entry(application).State = System.Data.Entity.EntityState.Modified;
                                                context.Database.ExecuteSqlCommand(" Update Course_Exam_Application set  Payment_Status_ID = " + Convert.ToInt32(enmPaymentStatus.Paid) + " Where Demand_Note_ID = " + demandNote.ID);
                                                context.SaveChanges();
                                            }
                                        };
                                        context.Entry(demandNote).State = System.Data.Entity.EntityState.Modified;
                                        context.SaveChanges();
                                    }
                                }
                            }
                        }
                        scope.Complete();
                        ShowAlert("Transaction details has been verified successfully.", true);
                    };
                };
                BindGrid();
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}

