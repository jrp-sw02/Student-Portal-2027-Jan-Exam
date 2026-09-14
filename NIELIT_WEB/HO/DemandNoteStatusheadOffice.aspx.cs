using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Presentation;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class DemandNoteStatusheadOffice : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;
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
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {

                if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                {
                    FillApplicationTypes();
                    ShowEditMode();
                }
                else
                {
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";
                    CourseCategory();
                    FillPaymetStatusFilter();
                    if (Request.QueryString["ApplTypeID"] != null || Request.QueryString["CoursecatID"] != null || Request.QueryString["CourseID"] != null || Request.QueryString["ExamYears"] != null || Request.QueryString["ExamID"] != null || Request.QueryString["PaymentStatus"] != null)
                    {

                        ddlCourseCategoryFilter.SelectedValue = Request.QueryString["CoursecatID"];
                        ddlCourseCategoryFilter_SelectedIndexChanged(ddlCourseCategoryFilter.SelectedValue, EventArgs.Empty);
                        ddlCourseFilter.SelectedValue = Request.QueryString["CourseID"];
                        ddlCourseFilter_SelectedIndexChanged(ddlCourseFilter.SelectedValue, EventArgs.Empty);
                        ddlAppType.SelectedValue = Request.QueryString["ApplTypeID"];
                        ddlAppType_SelectedIndexChanged(ddlAppType.SelectedValue, EventArgs.Empty);
                        ddlExamYear.SelectedValue = Request.QueryString["ExamYears"];
                        ddlExamYear_SelectedIndexChanged(ddlExamYear.SelectedValue, EventArgs.Empty);
                        ddlExamName.SelectedValue = Request.QueryString["ExamID"];
                        ddlpStatus.SelectedValue = Request.QueryString["PaymentStatus"];
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Demand Note Status: " + ddlAppType.SelectedItem.Text, "ho/DemandNoteStatusheadOffice.aspx?ApplTypeID=" + ddlAppType.SelectedValue + "&CoursecatID=" + ddlCourseCategoryFilter.SelectedValue + "&CourseID=" + ddlCourseFilter.SelectedValue + "&ExamYears=" + ddlExamYear.SelectedValue + "&ExamID=" + ddlExamName.SelectedValue + "&PaymentStatus=" + ddlpStatus.SelectedValue, ""));
                        ucSearchBar.AutoCompleteContextKey = Request.QueryString["ApplTypeID"];
                        BindGridView();

                    }
                    else
                    {
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.RemoveLastBreadCrumbItem();
                        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Demand Note Status", "ho/DemandNoteStatusheadOffice.aspx", ""));
                        BreadCrumb1.Render();
                    }
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "Please Select Filter Criteria For View Records";
                        lblError.Visible = true;
                    }
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void FillApplicationTypes()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var ApplicationList = from p in context.ApplicationTypes
                                      select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlAppType, ApplicationList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillPaymetStatusFilter()
    {
        EConnect.Utils.Common.EnumUtility.BindListObject(ref ddlpStatus, typeof(EConnect.enmPaymentStatus), new ListItem("--All--", "0"));
    }
    protected void FillFilterPaymentMode()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                Int32 cheque = Convert.ToInt32(enmPaymentMode.MultiCityCheque);
                Int32 cash = Convert.ToInt32(enmPaymentMode.Cash);
                var PaymentMode = from p in context.PaymentModes
                                  where p.ID != cheque && p.ID != cash
                                  select new { ValueField = p.ID, TextField = p.Name };
                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlPaymentMode, PaymentMode, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void FillFilterPaymentStatus()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--All--", "0");
                var PaymentStatus = from p in context.PaymentStatuss
                                    select new { ValueField = p.ID, TextField = p.Name };
                //EConnect.Utils.Common.ControlUtility.BindListObject(ddlPaymentStatus, PaymentStatus, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowEditMode()
    {
        try
        {
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            btnMode.ViewMode = ToggleView.Mode.List;
            btnMode.Visible = true;
            mltvTab.ActiveViewIndex = 1;
            pnlFilter.Visible = false;
            ucSearchBar.Visible = false;
            btnSave.Text = "Update";
            lblHeading.Text = "Demand Note Status";
            btnPrint.Visible = true;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 DemandNoteID = Convert.ToInt32(Request.QueryString["Key"]);
                hfAppID.Value = Request.QueryString["AppId"];
                hfDemandNoteID.Value = Request.QueryString["Key"].ToString();


                var DemandNote = (from p in context.DemandNotes
                                  where p.ID == DemandNoteID
                                  select new
                                  {
                                      ID = p.ID,
                                      DemandNo = p.ID,
                                      DemandDate = p.ApplicationDate,
                                      PaymentMode = p.PaymentMode.Name,
                                      PaymentStatus = p.PaymentStatus.Name,
                                      Amount = p.Amount,
                                      FeeType = p.FeeTypeID,
                                      ApplicationTypeID = p.ApplicationTypeID,
                                      DemandNoteTypeID = p.DemandNoteTypeID
                                  }).FirstOrDefault();

                lblDemandNoteNo.Text = DemandNote.DemandNo.ToString();
                lblDemandNoteDate.Text = DemandNote.DemandDate.ToString("dd-MMM-yyyy");
                lblPaymentMode.Text = DemandNote.PaymentMode.ToString();
                lblPaymentStatus.Text = DemandNote.PaymentStatus.ToString();
                lblAmount.Text = DemandNote.Amount.ToString();
                string PaymentStatus = Convert.ToString(enmPaymentStatus.Pending);

                string DemandNoteType = Request.QueryString["Dtype"];

                if (DemandNoteType == "Candidate")
                {
                    btnPrint.Visible = false;
                }
                else if (DemandNoteType == "Institute")
                {
                    btnPrint.Visible = true;
                }
                if (DemandNote.PaymentStatus == PaymentStatus)
                {
                    btnpaynow.Visible = true;
                }
                else
                {
                    btnpaynow.Visible = false;
                }

                if (DemandNote.PaymentMode.ToUpper() == "DEMAND DRAFT".ToUpper() && DemandNote.PaymentStatus.ToUpper() != "Pending".ToUpper())
                {

                    var DD = (from p in context.DemandNotes
                              join dt in context.DemandDraftTransactions on p.ID equals dt.DemandNoteID
                              where p.ID == DemandNoteID
                              select new { ID = dt.DemandDraftNumber, Date = dt.DemandDraftDate, Bank = dt.IssuingBankName, VerifiedOn = dt.VerificatonDate }).FirstOrDefault();

                    lbltd.Visible = true;
                    lbldno.Visible = true;
                    lblddate.Visible = true;
                    lblbname.Visible = true;
                    lblVdate.Visible = true;
                    ddno.InnerText = "Demand Draft Number";
                    dddate.InnerText = " Demand Draft Date";
                    ddbank.InnerText = "Bank Name";
                    ddveri.InnerText = " Date of Demand Draft Verification";
                    lblddno.Text = DD.ID.ToString();
                    lbldddate.Text = DD.Date.ToString("dd-MMM-yyyy");
                    lblBank.Text = DD.Bank.ToString();
                    LblVarificationDate.Text = DD.VerifiedOn.HasValue ? DD.VerifiedOn.Value.ToString("dd-MMM-yyyy") : "Not Yet Verified";
                }
                else if (DemandNote.PaymentMode.ToUpper() == "NEFT/RTGS".ToUpper() && DemandNote.PaymentStatus.ToUpper() != "Pending".ToUpper())
                {
                    var NEFT = (from p in context.DemandNotes
                                join dt in context.NEFTTransactions on p.ID equals dt.DemandNoteID
                                where p.ID == DemandNoteID
                                select new { ID = dt.TransactionNumber, Date = dt.TransactionDate, Bank = dt.TransactionBank, VerifiedOn = dt.VerificatonDate }).FirstOrDefault();

                    lbltd.Visible = true;
                    lbldno.Visible = true;
                    lblddate.Visible = true;
                    lblbname.Visible = true;
                    lblVdate.Visible = true;
                    ddno.InnerText = "NEFT Transaction No.";
                    dddate.InnerText = " NEFT Transaction Date";
                    ddbank.InnerText = "Bank Name";
                    ddveri.InnerText = " Date of NEFT Transaction Verification";
                    lblddno.Text = NEFT.ID.ToString();
                    lbldddate.Text = NEFT.Date.ToString("dd-MMM-yyyy");
                    lblBank.Text = NEFT.Bank.ToString();
                    LblVarificationDate.Text = NEFT.VerifiedOn.HasValue ? NEFT.VerifiedOn.Value.ToString("dd-MMM-yyyy") : "Not Yet Verified";
                }
                else if (DemandNote.PaymentMode.ToUpper() == "CSC SPV".ToUpper() && DemandNote.PaymentStatus.ToUpper() != "Pending".ToUpper())
                {

                    var DD = (from p in context.DemandNotes
                              join dt in context.CSCTransactions on p.ID equals dt.DemandNoteID
                              where p.ID == DemandNoteID && (dt.ResponseStatus.Value == 0 || dt.ResponseStatus.Value == 100)
                              select new { ID = dt.ID, Date = dt.Date, Amount = dt.Amount, Tno = (!string.IsNullOrEmpty(dt.ResponseTransactionNumber) ? dt.ResponseTransactionNumber : "NA") }).FirstOrDefault();

                    lbltd.Visible = true;
                    lbldno.Visible = true;
                    lblddate.Visible = true;
                    lblbname.Visible = true;
                    lblVdate.Visible = false;
                    ddno.InnerText = "CSC SPV Transaction No.";
                    dddate.InnerText = "CSC SPV Transaction Date";
                    ddbank.InnerText = "Amount";
                    lblddno.Text = DD.ID.ToString() + "(" + DD.Tno.ToString() + ")";
                    lbldddate.Text = DD.Date.ToString("dd-MMM-yyyy");
                    lblBank.Text = DD.Amount.ToString("F");
                }
                else if (DemandNote.PaymentMode.ToUpper() == "Online".ToUpper() && DemandNote.PaymentStatus.ToUpper() != "Pending".ToUpper())
                {
                    var DD = (from p in context.DemandNotes
                              join dt in context.OnlineTransaction on p.ID equals dt.DemandNoteID
                              where p.ID == DemandNoteID && ((dt.ResponseStatusCode =="0300"  && dt.ResponseStatusMessage == "Success") || (dt.ResponseStatusCode == "E000" || dt.IsSettled == true))  
                              select new { ID = dt.ID, Date = dt.RequestDate, Amount = dt.Amount, Tno = (!string.IsNullOrEmpty(dt.ReferenceNumber) ? dt.ReferenceNumber : "NA") }).FirstOrDefault();

                    lbltd.Visible = true;
                    lbldno.Visible = true;
                    lblddate.Visible = true;
                    lblbname.Visible = true;
                    lblVdate.Visible = false;
                    ddno.InnerText = "Online Transaction No.";
                    dddate.InnerText = "Online Transaction Date";
                    lblddno.Text = DD.ID.ToString() + "(" + DD.Tno + ")";
                    ddbank.InnerText = "Amount";
                    lbldddate.Text = DD.Date.ToString("dd-MMM-yyyy");
                    lblBank.Text = DD.Amount.ToString("F");
                }
                else
                {

                    lbltd.Visible = false;
                    lbldno.Visible = false;
                    lblddate.Visible = false;
                    lblbname.Visible = false;
                    lblVdate.Visible = false;
                }

                enmFeeType feetype = (enmFeeType)DemandNote.FeeType;
                lblFeeType.Text = EConnect.Utils.Common.EnumUtility.GetDescription(feetype);
                lblApplType.Text = EConnect.Utils.Common.EnumUtility.GetDescription((enmApplicationType)DemandNote.ApplicationTypeID);
                lblDemandNoteType.Text = EConnect.Utils.Common.EnumUtility.GetDescription((enmDemandNoteType)DemandNote.DemandNoteTypeID);

                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("DemandNote", "ho/DemandNoteStatusheadOffice.aspx?" + Request.QueryString.ToString(), ""));
                //Get last modified date of current record and save it in ViewState object.
                ViewState["LastModifiedOn"] = DateTime.Now; // objMenuObject.ModifiedOn.HasValue ? objMenuObject.ModifiedOn.Value : objMenuObject.CreatedOn;
                //Create an object of record to be modified and assign properties to relevant fields.
            };
        }
        catch (Exception ex) { throw ex; }
    }
    protected void BindGridView()
    {
        try
        {
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            string searchString = null;
            searchString = ucSearchBar.SearchText;
            Int64 ApplTypeID = 0;
            string sortOrder = ViewState["SortOrder"].ToString();
            string sortField = ViewState["SortField"].ToString();
            if (ddlAppType.SelectedItem.Text != "--Select One--")
            {
                ApplTypeID = Convert.ToInt64(ddlAppType.SelectedValue);
            }
            ucSearchBar.AutoCompleteContextKey = ApplTypeID.ToString().ToUpper();
            lblError.Visible = false;
            gvMain.Visible = true;
            Int64 CourseCategory = 0;
            Int64 Course = 0;
            Int64 ExamYear = 0;
            Int64 ExamName = 0;
            Int32 PStatus = 0;

            if (ddlCourseCategoryFilter.SelectedValue != "0")
                CourseCategory = Convert.ToInt32(ddlCourseCategoryFilter.SelectedValue);
            if (ddlCourseFilter.SelectedValue != "0")
                Course = Convert.ToInt32(ddlCourseFilter.SelectedValue);
            if (ddlExamYear.SelectedValue != "0")
                ExamYear = Convert.ToInt32(ddlExamYear.SelectedValue);
            if (ddlExamName.SelectedValue != "0")
                ExamName = Convert.ToInt32(ddlExamName.SelectedValue);
            if (ddlpStatus.SelectedValue != "0")
                PStatus = Convert.ToInt32(ddlpStatus.SelectedValue);

            context = new EConnectContext();
            if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
            {

                var DemadNote = from s in context.DemandNotes
                                join c in context.CourseRegistrationApplications on s.ID equals c.DemandNoteID

                                //added by amit start
                                join ot in context.OnlineTransaction
                                   .Where(x => x.ResponseStatusCode == "E000" || x.ResponseStatusCode == "0300")
                                   on s.ID equals ot.DemandNoteID into otGroup
                                from ot in otGroup.DefaultIfEmpty()

                                join pg in context.Paymentgateways
                                    on ot.PGCode equals pg.ID.ToString() into pgGroup
                                from pg in pgGroup.DefaultIfEmpty()
                                //added by amit end


                                select new
                                {
                                    ID = s.ID,
                                    DemandNo = SqlFunctions.StringConvert((Double)(s.ID)),
                                    DemandDate = s.ApplicationDate,
                                    PaymentMode = s.PaymentMode.Name,
                                    PaymentModeID = s.PaymentModeID,
                                    PaymentStatus = s.PaymentStatus.Name,
                                    PaymentStatusID = s.PaymentStatusID,
                                    ApplicationTypeID = s.ApplicationTypeID,
                                    Amount = s.Amount,
                                    //PaymentGateway = s.PaymentStatusID == 2 ? (ot.PGCode == null &&  ? "EFT" : pg.Description) : null,
                                    PaymentGateway =
                                      s.PaymentModeID == 2 ? (ot.PGCode == null ? "EFT" : pg.Description): null,
                                    instituteID = c.InstituteID,
                                    Coursecat = c.CourseCategoryID,
                                    courseIDs = c.CourseID,
                                    ExamYears = c.ApplicableExam.ExamYear,
                                    ExamNames = c.ApplicableExam.ID,
                                    Dtype = (s.DemandNoteTypeID == 1 ? "Candidate" : "Institute"),
                                    pName = (s.DemandNoteTypeID == 1 ? c.Name : c.Institute.Name),
                                    AppId = s.DemandNoteTypeID == 1 ? c.ID : 0
                                };



                if (loginUserType == UserType.Institute)
                {
                    DemadNote = DemadNote.Where(s => s.instituteID == entityID);
                }
                if (!string.IsNullOrEmpty(searchString))
                {
                    DemadNote = DemadNote.Where(s => s.DemandNo.ToUpper().Contains(searchString) ||
                                                s.pName.ToUpper().Contains(searchString));
                }
                if (CourseCategory != 0)
                    DemadNote = DemadNote.Where(s => s.Coursecat == CourseCategory);
                if (Course != 0)
                    DemadNote = DemadNote.Where(s => s.courseIDs == Course);
                if (ExamYear != 0)
                    DemadNote = DemadNote.Where(s => s.ExamYears == ExamYear);
                if (ExamName != 0)
                    DemadNote = DemadNote.Where(s => s.ExamNames == ExamName);
                if (PStatus != 0)
                    DemadNote = DemadNote.Where(s => s.PaymentStatusID == PStatus);
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "DemandNo":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.DemandNo);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.DemandNo);
                            break;
                        case "Dtype":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.Dtype);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.Dtype);
                            break;
                        case "pName":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.pName);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.pName);
                            break;
                        case "DemandDate":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.DemandDate);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.DemandDate);
                            break;
                        case "PaymentMode":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentMode);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentMode);
                            break;
                        case "PaymentStatus":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentStatus);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentStatus);
                            break;
                        default:
                            DemadNote = DemadNote.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(DemadNote, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
            }
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
            {
                var DemadNote = (from s in context.DemandNotes
                                 join c in context.CertificateExamApplications on s.ID equals c.DemandNoteID

                                 //added by amit start
                                 join ot in context.OnlineTransaction
                                    .Where(x => x.ResponseStatusCode == "E000" || x.ResponseStatusCode == "0300")
                                    on s.ID equals ot.DemandNoteID into otGroup
                                 from ot in otGroup.DefaultIfEmpty()

                                 join pg in context.Paymentgateways
                                     on ot.PGCode equals pg.ID.ToString() into pgGroup
                                 from pg in pgGroup.DefaultIfEmpty()
                                     //added by amit end

                                 select new
                                 {
                                     ID = s.ID,
                                     DemandNo = SqlFunctions.StringConvert((Double)(s.ID)),
                                     DemandDate = s.ApplicationDate,
                                     PaymentMode = s.PaymentMode.Name,
                                     PaymentModeID = s.PaymentModeID,
                                     PaymentStatus = s.PaymentStatus.Name,
                                     PaymentStatusID = s.PaymentStatusID,
                                     ApplicationTypeID = s.ApplicationTypeID,
                                     Amount = s.Amount,
                                     PaymentGateway =
                                                          PStatus == 2
                                                               ? (ot.PGCode == null ? "EFT" : pg.Description)
                                                               : null,
                                     instituteID = c.InstituteID,
                                     Coursecat = c.CourseCategoryID,
                                     courseIDs = c.CourseID,
                                     ExamYears = c.Exam.ExamYear,
                                     ExamNames = c.Exam.ID,
                                     Dtype = (s.DemandNoteTypeID == 1 ? "Candidate" : "Institute"),
                                     pName = (s.DemandNoteTypeID == 1 ? c.Name : c.Institute.Name),
                                     AppId = s.DemandNoteTypeID == 1 ? c.ID : 0
                                 }).Distinct();

                if (loginUserType == UserType.Institute)
                {
                    DemadNote = DemadNote.Where(s => s.instituteID == entityID);
                }
                if (!string.IsNullOrEmpty(searchString))
                {
                    DemadNote = DemadNote.Where(s => s.DemandNo.ToUpper().Contains(searchString));
                }
                if (PStatus != 0)
                    DemadNote = DemadNote.Where(s => s.PaymentStatusID == PStatus);
                if (CourseCategory != 0)
                    DemadNote = DemadNote.Where(s => s.Coursecat == CourseCategory);
                if (Course != 0)
                    DemadNote = DemadNote.Where(s => s.courseIDs == Course);
                if (ExamYear != 0)
                    DemadNote = DemadNote.Where(s => s.ExamYears == ExamYear);
                if (ExamName != 0)
                    DemadNote = DemadNote.Where(s => s.ExamNames == ExamName);
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "DemandNo":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.DemandNo);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.DemandNo);
                            break;
                        case "Dtype":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.Dtype);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.Dtype);
                            break;
                        case "pName":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.pName);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.pName);
                            break;
                        case "DemandDate":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.DemandDate);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.DemandDate);
                            break;
                        case "PaymentMode":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentMode);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentMode);
                            break;
                        case "PaymentStatus":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentStatus);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentStatus);
                            break;
                        default:
                            DemadNote = DemadNote.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(DemadNote, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
            }
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                var DemadNote = (from s in context.DemandNotes
                                 join c in context.CourseExamApplications on s.ID equals c.DemandNoteID

                                 //added by amit start
                                 join ot in context.OnlineTransaction
                                    .Where(x => x.ResponseStatusCode == "E000" || x.ResponseStatusCode == "0300")
                                    on s.ID equals ot.DemandNoteID into otGroup
                                 from ot in otGroup.DefaultIfEmpty()

                                 join pg in context.Paymentgateways
                                     on ot.PGCode equals pg.ID.ToString() into pgGroup
                                 from pg in pgGroup.DefaultIfEmpty()
                                 //added by amit end

                                 select new
                                 {
                                     ID = s.ID,
                                     DemandNo = SqlFunctions.StringConvert((Double)(s.ID)),
                                     DemandDate = s.ApplicationDate,
                                     PaymentMode = s.PaymentMode.Name,
                                     PaymentModeID = s.PaymentModeID,
                                     PaymentStatus = s.PaymentStatus.Name,
                                     PaymentStatusID = s.PaymentStatusID,
                                     ApplicationTypeID = s.ApplicationTypeID,
                                     Amount = s.Amount,
                                     PaymentGateway =
                                       PStatus == 2
                                           ? (ot.PGCode == null ? "EFT" : pg.Description)
                                           : null,
                                     instituteID = c.InstituteID,
                                     Coursecat = c.CourseCategoryID,
                                     courseIDs = c.CourseID,
                                     ExamYears = c.Exam.ExamYear,
                                     ExamNames = c.Exam.ID,
                                     Dtype = (s.DemandNoteTypeID == 1 ? "Candidate" : "Institute"),
                                     pName = (s.DemandNoteTypeID == 1 ? c.Candidate.Name : c.Institute.Name),
                                     AppId = s.DemandNoteTypeID == 1 ? c.ID : 0
                                 }).Distinct();

                if (loginUserType == UserType.Institute)
                {
                    DemadNote = DemadNote.Where(s => s.instituteID == entityID);
                }
                if (!string.IsNullOrEmpty(searchString))
                {
                    DemadNote = DemadNote.Where(s => s.DemandNo.ToUpper().Contains(searchString));
                }
                if (PStatus != 0)
                    DemadNote = DemadNote.Where(s => s.PaymentStatusID == PStatus);
                if (CourseCategory != 0)
                    DemadNote = DemadNote.Where(s => s.Coursecat == CourseCategory);
                if (Course != 0)
                    DemadNote = DemadNote.Where(s => s.courseIDs == Course);
                if (ExamYear != 0)
                    DemadNote = DemadNote.Where(s => s.ExamYears == ExamYear);
                if (ExamName != 0)
                    DemadNote = DemadNote.Where(s => s.ExamNames == ExamName);
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "DemandNo":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.DemandNo);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.DemandNo);
                            break;
                        case "Dtype":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.Dtype);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.Dtype);
                            break;
                        case "pName":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.pName);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.pName);
                            break;
                        case "DemandDate":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.DemandDate);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.DemandDate);
                            break;
                        case "PaymentMode":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentMode);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentMode);
                            break;
                        case "PaymentStatus":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentStatus);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentStatus);
                            break;
                        default:
                            DemadNote = DemadNote.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(DemadNote, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
            }
            //----------------vaf acf----------------------------------------------------------------
            
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
            {
                var DemadNote = (from s in context.DemandNotes
                                 join c in context.CourseExamVafAcfs on s.ID equals c.DemandNoteID

                                 select new
                                 {
                                     ID = s.ID,
                                     DemandNo = SqlFunctions.StringConvert((Double)(s.ID)),
                                     DemandDate = s.ApplicationDate,
                                     PaymentMode = s.PaymentMode.Name,
                                     PaymentModeID = s.PaymentModeID,
                                     PaymentStatus = s.PaymentStatus.Name,
                                     PaymentStatusID = s.PaymentStatusID,
                                     ApplicationTypeID = s.ApplicationTypeID,
                                     Amount = s.Amount,
                                     instituteID = c.InstituteID,
                                     Coursecat = c.CourseCategoryID,
                                     courseIDs = c.CourseID,
                                     ExamYears = c.Exam.ExamYear,
                                     ExamNames = c.Exam.ID,
                                     Dtype = (s.DemandNoteTypeID == 1 ? "Candidate" : "Institute"),
                                     pName = (s.DemandNoteTypeID == 1 ? c.Candidate.Name : c.Institute.Name),
                                     AppId = s.DemandNoteTypeID == 1 ? c.ID : 0
                                 }).Distinct();

                if (loginUserType == UserType.Institute)
                {
                    DemadNote = DemadNote.Where(s => s.instituteID == entityID);
                }
                if (!string.IsNullOrEmpty(searchString))
                {
                    DemadNote = DemadNote.Where(s => s.DemandNo.ToUpper().Contains(searchString));
                }
                if (PStatus != 0)
                    DemadNote = DemadNote.Where(s => s.PaymentStatusID == PStatus);
                if (CourseCategory != 0)
                    DemadNote = DemadNote.Where(s => s.Coursecat == CourseCategory);
                if (Course != 0)
                    DemadNote = DemadNote.Where(s => s.courseIDs == Course);
                if (ExamYear != 0)
                    DemadNote = DemadNote.Where(s => s.ExamYears == ExamYear);
                if (ExamName != 0)
                    DemadNote = DemadNote.Where(s => s.ExamNames == ExamName);
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "DemandNo":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.DemandNo);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.DemandNo);
                            break;
                        case "Dtype":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.Dtype);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.Dtype);
                            break;
                        case "pName":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.pName);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.pName);
                            break;
                        case "DemandDate":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.DemandDate);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.DemandDate);
                            break;
                        case "PaymentMode":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentMode);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentMode);
                            break;
                        case "PaymentStatus":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentStatus);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentStatus);
                            break;
                        default:
                            DemadNote = DemadNote.OrderBy(s => s.ID);
                            break;
                    }
                }
                PagingBar1.Bind(DemadNote, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
            }
            
            //---------------------------end - vaf acf--------------------------------------
            // Added for Course Project Application 
            else if (ApplTypeID == Convert.ToInt32(enmApplicationType.CourseProjectApplication))
            {

                Int64 demandId = Convert.ToInt64(searchString);
                var DemadNote = (from s in context.DemandNotes
                                 join c in context.CourseProjectApplications on s.ID equals c.DemandNoteID

                                 //added by amit start
                                 join ot in context.OnlineTransaction
                                    .Where(x => x.ResponseStatusCode == "E000" || x.ResponseStatusCode == "0300")
                                    on s.ID equals ot.DemandNoteID into otGroup
                                 from ot in otGroup.DefaultIfEmpty()

                                 join pg in context.Paymentgateways
                                     on ot.PGCode equals pg.ID.ToString() into pgGroup
                                 from pg in pgGroup.DefaultIfEmpty()
                                     //added by amit end

                                 where s.ID == demandId
                                 select new
                                 {
                                     ID = s.ID,
                                     DemandNo = SqlFunctions.StringConvert((Double)(s.ID)),
                                     DemandDate = s.ApplicationDate,
                                     PaymentMode = s.PaymentMode.Name,
                                     PaymentModeID = s.PaymentModeID,
                                     PaymentStatus = s.PaymentStatus.Name,
                                     PaymentStatusID = s.PaymentStatusID,
                                     PaymentGateway =
                                    PStatus == 2
                                         ? (ot.PGCode == null ? "EFT" : pg.Description)
                                         : null,
                                     ApplicationTypeID = s.ApplicationTypeID,
                                     Amount = s.Amount,
                                     instituteID = c.InstituteID,
                                     Coursecat = c.CourseCategoryID,
                                     courseIDs = c.CourseID,
                                     ExamYears = 2021,
                                     ExamNames = "NA",
                                     Dtype = (s.DemandNoteTypeID == 1 ? "Candidate" : "Institute"),
                                     pName = (s.DemandNoteTypeID == 1 ? c.Candidate.Name : c.Institute.Name),
                                     AppId = s.DemandNoteTypeID == 1 ? c.ID : 0
                                 }).Distinct();

                //if (loginUserType == UserType.Institute)
                //{
                //    DemadNote = DemadNote.Where(s => s.instituteID == entityID);
                //}
                //if (!string.IsNullOrEmpty(searchString))
                //{
                //   // DemadNote = DemadNote.Where(s => s.DemandNo.ToUpper().Contains(searchString));  
                //    DemadNote = DemadNote.Where(s =>  s.DemandNo.Contains(searchString));  
                //}
                //if (PStatus != 0)
                //    DemadNote = DemadNote.Where(s => s.PaymentStatusID == PStatus);
                //if (CourseCategory != 0)
                //    DemadNote = DemadNote.Where(s => s.Coursecat == CourseCategory);
                //if (Course != 0)
                //    DemadNote = DemadNote.Where(s => s.courseIDs == Course);
                //if (ExamYear != 0)
                //    DemadNote = DemadNote.Where(s => s.ExamYears == ExamYear);
                //if (ExamName != 0)
                //    DemadNote = DemadNote.Where(s => s.ExamNames == ExamName);
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortField)
                    {
                        case "DemandNo":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.DemandNo);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.DemandNo);
                            break;
                        case "Dtype":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.Dtype);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.Dtype);
                            break;
                        case "pName":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.pName);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.pName);
                            break;
                        case "DemandDate":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.DemandDate);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.DemandDate);
                            break;
                        case "PaymentMode":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentMode);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentMode);
                            break;
                        case "PaymentStatus":
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentStatus);
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentStatus);
                            break;
                        case "PaymentGateway": // added by amit 
                            if (sortOrder == "DESC")
                                DemadNote = DemadNote.OrderByDescending(s => s.PaymentGateway ?? "");
                            else
                                DemadNote = DemadNote.OrderBy(s => s.PaymentGateway ?? "");
                            break;


                        default:
                            DemadNote = DemadNote.OrderBy(s => s.ID);
                            break;
                    }
                }

                PagingBar1.Bind(DemadNote, ref gvMain);
                uPnlGrid.Update();
                uPnlNavigation.Update();
            }

            if (gvMain.Rows.Count <= 0)
            {
                lblError.Text = "No record found.";
                lblError.Visible = true;
            }
        }
        catch (Exception ex) { throw ex; }
        finally { context.Dispose(); }
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
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
        }
        else
        {
            //Response.Redirect("DemandNoteStatusheadOffice.aspx?ApplTypeID=" + Request.QueryString["TypeID"] + "&PModeID=" + Request.QueryString["PModeID"] + "&PstsID=" + Request.QueryString["PstsID"], true);
            Response.Redirect("DemandNoteStatusheadOffice.aspx?ApplTypeID=" + Request.QueryString["TypeID"] + "&CoursecatID=" + Request.QueryString["Coursecat"] + "&CourseID=" + Request.QueryString["courseIDs"] + "&ExamYears=" + Request.QueryString["ExamYears"] + "&ExamID=" + Request.QueryString["ExamNames"] + Request.QueryString["&Dtype"] + "&PaymentStatus=" + Request.QueryString["PaymentStatus"], true);
        }
    }
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        string searchString = null;
        Int64 ApplTypeID = 0;
        string sortOrder = ViewState["SortOrder"].ToString();
        string sortField = ViewState["SortField"].ToString();
        if (ddlAppType.SelectedItem.Text != "--Select One--")
        {
            ApplTypeID = Convert.ToInt64(ddlAppType.SelectedValue);
        }
        lblError.Visible = false;
        gvMain.Visible = true;
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                if (ucSearchBar.SearchText != "")
                {
                    searchString = ucSearchBar.SearchText.Trim().ToUpper();
                    Int64 DemandNoteID = 0;
                    DemandNote demandNote = new DemandNote();
                    if (IsNumeric(searchString))
                    {
                        DemandNoteID = Convert.ToInt64(searchString);
                        demandNote = context.DemandNotes.Find(DemandNoteID);
                    }
                    else
                    {
                        ShowAlert("Please Enter Demand Note Number", true);
                    }
                    if (demandNote != null)
                    {
                        int TypeId = Convert.ToInt32(demandNote.ApplicationTypeID);
                        ucSearchBar.AutoCompleteContextKey = TypeId.ToString().ToUpper();
                        if (TypeId == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
                        {
                            var CexApp = (from ce in context.CourseRegistrationApplications
                                          where ce.DemandNoteID == DemandNoteID
                                          select new { CourseCateID = ce.CourseCategoryID, CourseID = ce.CourseID, ExamID = ce.ApplicableExamID, ExamYear = ce.ApplicableExam.ExamYear, PaymentStatus = ce.PaymentStatusID.Value }).FirstOrDefault();

                            if (CexApp != null)
                            {
                                ddlCourseCategoryFilter.SelectedValue = CexApp.CourseCateID.ToString();
                                ddlCourseCategoryFilter_SelectedIndexChanged(ddlCourseCategoryFilter.SelectedValue, EventArgs.Empty);
                                ddlCourseFilter.SelectedValue = CexApp.CourseID.ToString();
                                ddlCourseFilter_SelectedIndexChanged(ddlCourseFilter.SelectedValue, EventArgs.Empty);
                                ddlAppType.SelectedValue = TypeId.ToString();
                                ddlAppType_SelectedIndexChanged(ddlAppType.SelectedValue, EventArgs.Empty);
                                ddlExamYear.SelectedValue = CexApp.ExamYear.ToString();
                                ddlExamYear_SelectedIndexChanged(ddlExamYear.SelectedValue, EventArgs.Empty);
                                ddlExamName.SelectedValue = CexApp.ExamID.ToString();
                                ddlpStatus.SelectedValue = CexApp.PaymentStatus.ToString();
                            }
                        }
                        //-------vaf acf-----------------------------
                        
                        else if (TypeId == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
                        {
                            var CexApp = (from ce in context.CourseExamVafAcfs
                                          where ce.DemandNoteID == DemandNoteID
                                          select new { CourseCateID = ce.CourseCategoryID, CourseID = ce.CourseID, ExamID = ce.ExamID, ExamYear = ce.Exam.ExamYear, PaymentStatus = ce.PaymentStatusID }).FirstOrDefault();
                            if (CexApp != null)
                            {
                                ddlCourseCategoryFilter.SelectedValue = CexApp.CourseCateID.ToString();
                                ddlCourseCategoryFilter_SelectedIndexChanged(ddlCourseCategoryFilter.SelectedValue, EventArgs.Empty);
                                ddlCourseFilter.SelectedValue = CexApp.CourseID.ToString();
                                ddlCourseFilter_SelectedIndexChanged(ddlCourseFilter.SelectedValue, EventArgs.Empty);
                                ddlAppType.SelectedValue = TypeId.ToString();
                                ddlAppType_SelectedIndexChanged(ddlAppType.SelectedValue, EventArgs.Empty);
                                ddlExamYear.SelectedValue = CexApp.ExamYear.ToString();
                                ddlExamYear_SelectedIndexChanged(ddlExamYear.SelectedValue, EventArgs.Empty);
                                ddlExamName.SelectedValue = CexApp.ExamID.ToString();
                                ddlpStatus.SelectedValue = CexApp.PaymentStatus.ToString();
                            }
                        }
                        
                        //------vaf acf end---------------------------
                        else if (TypeId == Convert.ToInt32(enmApplicationType.CourseExamApplication))
                        {
                            var CexApp = (from ce in context.CourseExamApplications
                                          where ce.DemandNoteID == DemandNoteID
                                          select new { CourseCateID = ce.CourseCategoryID, CourseID = ce.CourseID, ExamID = ce.ExamID, ExamYear = ce.Exam.ExamYear, PaymentStatus = ce.PaymentStatusID }).FirstOrDefault();
                            if (CexApp != null)
                            {
                                ddlCourseCategoryFilter.SelectedValue = CexApp.CourseCateID.ToString();
                                ddlCourseCategoryFilter_SelectedIndexChanged(ddlCourseCategoryFilter.SelectedValue, EventArgs.Empty);
                                ddlCourseFilter.SelectedValue = CexApp.CourseID.ToString();
                                ddlCourseFilter_SelectedIndexChanged(ddlCourseFilter.SelectedValue, EventArgs.Empty);
                                ddlAppType.SelectedValue = TypeId.ToString();
                                ddlAppType_SelectedIndexChanged(ddlAppType.SelectedValue, EventArgs.Empty);
                                ddlExamYear.SelectedValue = CexApp.ExamYear.ToString();
                                ddlExamYear_SelectedIndexChanged(ddlExamYear.SelectedValue, EventArgs.Empty);
                                ddlExamName.SelectedValue = CexApp.ExamID.ToString();
                                ddlpStatus.SelectedValue = CexApp.PaymentStatus.ToString();
                            }
                        }
                        else if (TypeId == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
                        {
                            var CexApp = (from ce in context.CertificateExamApplications
                                          where ce.DemandNoteID == DemandNoteID
                                          select new { CourseCateID = ce.CourseCategoryID, CourseID = ce.CourseID, ExamID = ce.ExamID, ExamYear = ce.Exam.ExamYear, ExamCycel = ce.Exam.ExaminationCycleID, PaymentStatus = ce.PaymentStatusID }).FirstOrDefault();
                            if (CexApp != null)
                            {
                                ddlCourseCategoryFilter.SelectedValue = CexApp.CourseCateID.ToString();
                                ddlCourseCategoryFilter_SelectedIndexChanged(ddlCourseCategoryFilter.SelectedValue, EventArgs.Empty);
                                ddlCourseFilter.SelectedValue = CexApp.CourseID.ToString();
                                ddlCourseFilter_SelectedIndexChanged(ddlCourseFilter.SelectedValue, EventArgs.Empty);
                                ddlAppType.SelectedValue = TypeId.ToString();
                                ddlAppType_SelectedIndexChanged(ddlAppType.SelectedValue, EventArgs.Empty);
                                ddlExamCycle.SelectedValue = CexApp.ExamCycel.ToString();
                                ddlExamCycle_SelectedIndexChanged(ddlExamCycle.SelectedValue, EventArgs.Empty);
                                ddlExamYear.SelectedValue = CexApp.ExamYear.ToString();
                                ddlExamYear_SelectedIndexChanged(ddlExamYear.SelectedValue, EventArgs.Empty);
                                ddlExamName.SelectedValue = CexApp.ExamID.ToString();
                                ddlpStatus.SelectedValue = CexApp.PaymentStatus.ToString();
                            }
                        }

                        else if (TypeId == Convert.ToInt32(enmApplicationType.CourseProjectApplication))
                        {
                            var CexApp = (from cp in context.CourseProjectApplications
                                          where cp.DemandNoteID == DemandNoteID
                                          select new { CourseCateID = cp.CourseCategoryID, CourseID = cp.CourseID, PaymentStatus = cp.PaymentStatusID }).FirstOrDefault();
                            if (CexApp != null)
                            {

                                ddlCourseCategoryFilter.SelectedValue = CexApp.CourseCateID.ToString();
                                ddlCourseCategoryFilter_SelectedIndexChanged(ddlCourseCategoryFilter.SelectedValue, EventArgs.Empty);
                                ddlCourseFilter.SelectedValue = CexApp.CourseID.ToString();
                                ddlCourseFilter_SelectedIndexChanged(ddlCourseFilter.SelectedValue, EventArgs.Empty);
                                ddlAppType.SelectedValue = TypeId.ToString();

                                BindGridView();

                                ddlpStatus.SelectedValue = CexApp.PaymentStatus.ToString();

                            }
                        }
                    }
                    else
                    {
                        ShowAlert("No record found for demand note number " + DemandNoteID.ToString(), true);
                    }
                }
            };
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
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
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void SaveRecord(object sender, EventArgs e)
    {
        //try
        //{
        //    context = new EConnectContext();
        //    //create and object 
        //    User objUser;
        //    if (String.IsNullOrEmpty(Request.QueryString["Key"]))
        //    {

        //        strMessage = "New record saved.";
        //    }
        //    else
        //    {
        //        ////Initialize current object by loading it and get its current modified date
        //        strMessage = "Record updated.";
        //    }

        //    //Call save method
        //    //EConnect.URM.BusinessLogic.MenuObjectManager.Save(ref objMenuObject);
        //    //Redirect it to list mode
        //    Response.Redirect("CertificateCourse.aspx?msg=" + strMessage);
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
        //finally { context.Dispose(); }

    }
    protected void AllyFilter(object sender, EventArgs e)
    {

        try
        {
            ucSearchBar.SearchText = "";
            if (ddlAppType.SelectedValue != "0" || ddlCourseCategoryFilter.SelectedValue != "0" || ddlCourseFilter.SelectedValue != "0" || ddlExamYear.SelectedValue != "0" || ddlExamName.SelectedValue != "0" || ddlpStatus.SelectedValue != "0")
            {
                BreadCrumb1.UpdateLastBreadCrumbItem(new BreadCrumbItem("Demand Note Status: " + ddlAppType.SelectedItem.Text, "ho/DemandNoteStatusheadOffice.aspx?ApplTypeID=" + ddlAppType.SelectedValue + "&CoursecatID=" + ddlCourseCategoryFilter.SelectedValue + "&CourseID=" + ddlCourseFilter.SelectedValue + "&ExamYears=" + ddlExamYear.SelectedValue + "&ExamID=" + ddlExamName.SelectedValue + "&PaymentStatus=" + ddlpStatus.SelectedValue, ""));
                BreadCrumb1.Render();
                upBread.Update();
            }
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
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
            ddlAppType.SelectedValue = "0";
            ddlCourseCategoryFilter.SelectedValue = "0";
            ddlCourseFilter.SelectedValue = "0";
            ddlExamYear.SelectedValue = "0";
            ddlExamName.SelectedValue = "0";
            ddlExamCycle.SelectedValue = "0";
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            lblError.Text = "Please Select Filter Criteria For View Records";
            lblError.Visible = true;
            uPnlGrid.Update();
            Response.Redirect("DemandNoteStatusheadOffice.aspx", true);

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            ViewState["SortField"] = e.SortExpression;
            if (ViewState["SortOrder"].ToString() == "DESC")
                ViewState["SortOrder"] = "ASC";
            else
                ViewState["SortOrder"] = "DESC";
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void PerformPopupAction(object sender, EventArgs e)
    {
        //try
        //{
        //    context = new EConnectContext();
        //    if (hfActionID.Value != "")
        //    {
        //        String recordID = hfActionID.Value.Split('$')[0].ToString();
        //        LinkButton btnAction = (LinkButton)sender;
        //        if (btnAction.CommandName == "Delete")
        //        {
        //            //Load the object and apply validateion if required
        //            //call delete function
        //            //bind the grid again
        //            BindGridView();
        //            ShowAlert("Record deleted successfully.", true);
        //            hfActionID.Value = "";
        //        }
        //        else if (btnAction.CommandName == "Action")
        //        {
        //            //Load the object and apply validateion if required
        //            //call function to perform required action
        //            //bind the grid again
        //            BindGridView();
        //            ShowAlert("Record Action1 successfully.", true);
        //            hfActionID.Value = "";
        //        }
        //        uPnlGrid.Update();
        //    }
        //}
        //catch (Exception ex)
        //{
        //    hfActionID.Value = "";
        //    ShowAlert(ex.Message, true);
        //}
        //finally { context.Dispose(); }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
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

                HyperLink hl7 = (HyperLink)e.Row.Cells[7].Controls[0];
                hl7.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt(hl7.NavigateUrl);

                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [System.Web.Services.WebMethod(EnableSession = true)]
    public static String[] GetSearchText(String prefixText, Int32 count, String contextKey)
    {
        EConnectContext context = new EConnectContext();
        try
        {
            if (count <= 0)
                count = 10;
            Int64 ApplicationType = 0;
            if (!String.IsNullOrEmpty(contextKey))
                ApplicationType = Convert.ToInt64(contextKey);


            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            if (ApplicationType == Convert.ToInt32(enmApplicationType.CourseRegistrationApplication))
            {
                var result = from s in context.DemandNotes
                             join c in context.CourseRegistrationApplications on s.ID equals c.DemandNoteID
                             select new { Name = SqlFunctions.StringConvert((Double)(s.ID)) };


                if (!string.IsNullOrEmpty(searchString))
                {
                    result = result.Where(s => s.Name.ToUpper().Contains(searchString));
                }
                var r1 = from s in context.DemandNotes
                         join c in context.CourseRegistrationApplications on s.ID equals c.DemandNoteID
                         select new { Name = s.DemandNoteTypeID == 1 ? c.Name : c.Institute.Name };


                if (!string.IsNullOrEmpty(searchString))
                {
                    r1 = r1.Where(s => s.Name.ToUpper().Contains(searchString));
                }

                result = result.Union(r1).Take(count);

                foreach (var course in result)
                {
                    items.Add(course.Name);
                }
            }
            else if (ApplicationType == Convert.ToInt32(enmApplicationType.CertificateExamApplication))
            {
                var result = from s in context.DemandNotes
                             join c in context.CertificateExamApplications on s.ID equals c.DemandNoteID
                             select new { Name = SqlFunctions.StringConvert((Double)(s.ID)) };


                if (!string.IsNullOrEmpty(searchString))
                {
                    result = result.Where(s => s.Name.ToUpper().Contains(searchString));
                }
                var r1 = from s in context.DemandNotes
                         join c in context.CertificateExamApplications on s.ID equals c.DemandNoteID
                         select new { Name = s.DemandNoteTypeID == 1 ? c.Name : c.Institute.Name };


                if (!string.IsNullOrEmpty(searchString))
                {
                    r1 = r1.Where(s => s.Name.ToUpper().Contains(searchString));
                }

                result = result.Union(r1).Take(count);

                foreach (var course in result)
                {
                    items.Add(course.Name);
                }
            }
            //-------vaf acf-----------------------------

            else if (ApplicationType == Convert.ToInt32(enmApplicationType.CourseVafAcfApplication))
            {
                var result = from s in context.DemandNotes
                             join c in context.CourseExamVafAcfs on s.ID equals c.DemandNoteID
                             select new { Name = SqlFunctions.StringConvert((Double)(s.ID)) };


                if (!string.IsNullOrEmpty(searchString))
                {
                    result = result.Where(s => s.Name.ToUpper().Contains(searchString));
                }
                var r1 = from s in context.DemandNotes
                         join c in context.CourseExamVafAcfs on s.ID equals c.DemandNoteID
                         select new { Name = s.DemandNoteTypeID == 1 ? c.Candidate.Name : c.Institute.Name };


                if (!string.IsNullOrEmpty(searchString))
                {
                    r1 = r1.Where(s => s.Name.ToUpper().Contains(searchString));
                }

                result = result.Union(r1).Take(count);

                foreach (var course in result)
                {
                    items.Add(course.Name);
                }
            }

            //------vaf acf end---------------------------
            else if (ApplicationType == Convert.ToInt32(enmApplicationType.CourseExamApplication))
            {
                var result = from s in context.DemandNotes
                             join c in context.CourseExamApplications on s.ID equals c.DemandNoteID
                             select new { Name = SqlFunctions.StringConvert((Double)(s.ID)) };


                if (!string.IsNullOrEmpty(searchString))
                {
                    result = result.Where(s => s.Name.ToUpper().Contains(searchString));
                }
                var r1 = from s in context.DemandNotes
                         join c in context.CourseExamApplications on s.ID equals c.DemandNoteID
                         select new { Name = s.DemandNoteTypeID == 1 ? c.Candidate.Name : c.Institute.Name };


                if (!string.IsNullOrEmpty(searchString))
                {
                    r1 = r1.Where(s => s.Name.ToUpper().Contains(searchString));
                }

                result = result.Union(r1).Take(count);

                foreach (var course in result)
                {
                    items.Add(course.Name);
                }
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
        Response.Redirect("DemandNoteStatusheadOffice.aspx", true);
    }
    protected void btnpaynow_Click(object sender, EventArgs e)
    {
        Response.Redirect("../CAND/FrmConfirm.aspx?Appid=" + hfAppID.Value + "&TypeID=" + Request.QueryString["TypeID"] + "&DemandID=" + Request.QueryString["Key"]);
    }
    protected void CourseCategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                //Populating Course Category
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = (from p in context.CourseCategories
                                orderby (p.Name)
                                select new { ValueField = p.ID, TextField = p.Name }).Distinct();

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategoryFilter, Category, lst);

            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlCourseCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            PupulateCourses(Convert.ToInt32(ddlCourseCategoryFilter.SelectedValue), ddlCourseFilter, new ListItem("--Select One--", "0"));
            ddlAppType.Items.Clear();
            ddlExamYear.Items.Clear();
            ddlExamName.Items.Clear();
            ListItem lst = new ListItem("--Select One--", "0");
            ddlAppType.Items.Add(lst);
            ddlExamYear.Items.Add(lst);
            ddlExamName.Items.Add(lst);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void PupulateCourses(Int32 CourseCategoryID, DropDownList ddl, ListItem lst)
    {
        try
        {
            ddl.Items.Clear();
            //Added by Deep on 11 May 2022 for Short term course(6) view all courses start
            if (CourseCategoryID == 6)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    var CourseList = from p in context.Courses
                                     where p.CourseCategoryID == CourseCategoryID
                                     select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddl, CourseList, lst);
                };
            }
            else
            {
                //Added by Deep on 11 May 2022  Short term course(6) view all courses End

                using (EConnectContext context = new EConnectContext())
                {
                    var CourseList = from p in context.Courses
                                     where p.CourseCategoryID == CourseCategoryID
                                         //Added for code
                                      && p.ShowOnWeb
                                     select new { ValueField = p.ID, TextField = p.Name + " (" + p.Code + ")" };
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddl, CourseList, lst);
                };
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlCourseFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCourseFilter.SelectedValue != "0")
                PupulateApplicationType(Convert.ToInt32(ddlCourseFilter.SelectedValue), ddlAppType, new ListItem("--Select One--", "0"));
            else
            {
                ddlAppType.Items.Clear();
                ddlExamYear.Items.Clear();
                ddlExamName.Items.Clear();
                ListItem lst = new ListItem("--Select One--", "0");
                ddlAppType.Items.Add(lst);
                ddlExamYear.Items.Add(lst);
                ddlExamName.Items.Add(lst);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

    }
    protected void FillExamYears(int catID, int couID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (couID != 0 && catID != 0)
                {

                    var examYear = (from p in context.Exams

                                    where (p.CourseID == couID && p.CourseCategoryID == catID)
                                    orderby (p.ExamYear) descending
                                    select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, examYear, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void PupulateApplicationType(Int32 CourseID, DropDownList ddl, ListItem lst)
    {
        try
        {
            ddl.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {

                Course course = context.Courses.Find(CourseID);
                if (course != null)
                {


                    var ApplicationList = from p in context.ApplicationTypes
                                          where p.CourseTypeID == course.CourseTypeID
                                          select new { ValueField = p.ID, TextField = p.Name };
                    //-------vaf acf start----------------------------------------------------------
                    if (Convert.ToInt32(ddlCourseCategoryFilter.SelectedValue) != 1)//vaf acf appl type only for informationtech 
                    {
                        ApplicationList = ApplicationList.Where(a => a.ValueField != 6);
                    }
                    //-----------vaf acf end--------------------------------------------------------
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddl, ApplicationList, lst);


                }
                else
                {
                    ddl.Items.Add(lst);
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 catID = Convert.ToInt32(ddlCourseCategoryFilter.SelectedValue);
        Int32 couID = Convert.ToInt32(ddlCourseFilter.SelectedValue);
        Int32 exmYear = Convert.ToInt32(ddlExamYear.SelectedValue);
        ddlExamName.Items.Clear();
        FillExamNames(catID, couID, exmYear);
        ListItem lst = new ListItem("--Select One--", "0");
        if (ddlExamYear.SelectedValue == "0")
        {
            ddlExamName.Items.Add(lst);
        }
    }
    protected void FillExamNames(Int32 catID, Int32 couID, Int32 exmYear)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (couID != 0 && catID != 0 && exmYear != 0)
                {
                    if (Convert.ToInt32(ddlAppType.SelectedValue) == (Int32)enmApplicationType.CourseExamApplication)
                    {

                        var examName = (from p in context.Exams
                                        join ce in context.CourseExamApplications on p.ID equals ce.ExamID
                                        where (p.CourseID == couID && p.CourseCategoryID == catID && p.ExamYear == exmYear)
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.Name }).Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, examName, lst);
                    }
                    //-------vaf acf-----------------------------

                    else if (Convert.ToInt32(ddlAppType.SelectedValue) == (Int32)enmApplicationType.CourseVafAcfApplication)
                    {
                        var examName = (from p in context.Exams
                                        join ce in context.CourseExamVafAcfs on p.ID equals ce.ExamID
                                        where (p.CourseID == couID && p.CourseCategoryID == catID && p.ExamYear == exmYear)
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.Name }).Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, examName, lst);
                    }

                    //------vaf acf end---------------------------
                    else if (Convert.ToInt32(ddlAppType.SelectedValue) == (Int32)enmApplicationType.CourseRegistrationApplication)
                    {
                        var examName = (from p in context.Exams
                                        join ce in context.CourseRegistrationApplications on p.ID equals ce.ApplicableExamID
                                        where (p.CourseID == couID && p.CourseCategoryID == catID && p.ExamYear == exmYear)
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.Name }).Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, examName, lst);
                    }
                    else if (Convert.ToInt32(ddlAppType.SelectedValue) == (Int32)enmApplicationType.CertificateExamApplication)
                    {
                        var examName = (from p in context.Exams
                                        join ce in context.CertificateExamApplications on p.ID equals ce.ExamID
                                        where (p.CourseID == couID && p.CourseCategoryID == catID && p.ExamYear == exmYear)
                                        orderby (p.Name)
                                        select new { ValueField = p.ID, TextField = p.Name }).Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, examName, lst);
                    }

                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ddlAppType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlAppType.SelectedValue != "0")
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 couID = Convert.ToInt32(ddlCourseFilter.SelectedValue);
                    if (Convert.ToInt32(ddlAppType.SelectedValue) == (Int32)enmApplicationType.CourseRegistrationApplication)
                    {
                        excycle.Visible = false;
                        var ExamYear = (from c in context.Exams
                                        join p in context.CourseRegistrationApplications
                                        on c.ID equals p.ApplicableExamID
                                        where (p.CourseID == couID)
                                        select new { ValueField = c.ExamYear, TextField = c.ExamYear }).Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, ExamYear, new ListItem("--Select One--", "0"));

                    }
                    //-------vaf acf-----------------------------

                    else if (Convert.ToInt32(ddlAppType.SelectedValue) == (Int32)enmApplicationType.CourseVafAcfApplication)
                    {
                        excycle.Visible = false;
                        var ExamYear = (from c in context.Exams
                                        join p in context.CourseExamVafAcfs
                                        on c.ID equals p.ExamID
                                        where (p.CourseID == couID)
                                        select new { ValueField = c.ExamYear, TextField = c.ExamYear }).Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, ExamYear, new ListItem("--Select One--", "0"));
                    }

                    //------vaf acf end---------------------------
                    else if (Convert.ToInt32(ddlAppType.SelectedValue) == (Int32)enmApplicationType.CourseExamApplication)
                    {
                        excycle.Visible = false;
                        var ExamYear = (from c in context.Exams
                                        join p in context.CourseExamApplications
                                        on c.ID equals p.ExamID
                                        where (p.CourseID == couID)
                                        select new { ValueField = c.ExamYear, TextField = c.ExamYear }).Distinct();
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, ExamYear, new ListItem("--Select One--", "0"));
                    }
                    else if (Convert.ToInt32(ddlAppType.SelectedValue) == (Int32)enmApplicationType.CertificateExamApplication)
                    {
                        excycle.Visible = true;
                        ListItem lst = new ListItem("--Select One--", "0");

                        var courses = (from s in context.ExaminationCycles
                                       join c in context.CertificateExamApplications
                                           on s.ID equals c.Exam.ExaminationCycleID
                                       where s.CourseID == couID
                                       select new { ValueField = s.ID, TextField = s.Name }).Distinct();

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, courses, lst);
                    }
                    else if (Convert.ToInt32(ddlAppType.SelectedValue) == (Int32)enmApplicationType.CourseProjectApplication)
                    {
                        excycle.Visible = true;
                        ListItem lst = new ListItem("--Select One--", "0");

                        var courses = (from s in context.ExaminationCycles
                                       join c in context.CertificateExamApplications
                                           on s.ID equals c.Exam.ExaminationCycleID
                                       where s.CourseID == couID
                                       select new { ValueField = s.ID, TextField = s.Name }).Distinct();

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, courses, lst);
                    }
                };
            }
            else
            {
                ddlExamYear.Items.Clear();
                ddlExamName.Items.Clear();
                ListItem lst = new ListItem("--Select One--", "0");
                ddlExamYear.Items.Add(lst);
                ddlExamName.Items.Add(lst);
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
            lblError.Visible = true;
        }
    }
    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 courseid = Convert.ToInt32(ddlCourseFilter.SelectedValue);
        Int32 examcycleid = Convert.ToInt32(ddlExamCycle.SelectedValue);
        BindExamYear(courseid, examcycleid);
    }
    protected void BindExamYear(Int32 cid, Int32 examcycleid)
    {
        try
        {
            ListItem lst = new ListItem("--Select One--", "0");
            using (EConnectContext context = new EConnectContext())
            {
                Course cr = context.Courses.Find(cid);
                if (cr != null)
                {
                    var examyear = (from s in context.Exams
                                    join c in context.CertificateExamApplications
                                     on s.ID equals c.ExamID
                                    where s.CourseID == cid && s.ExaminationCycleID == examcycleid
                                    orderby (s.ExamYear) descending
                                    select new { ValueField = s.ExamYear, TextField = s.ExamYear }).Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, examyear, lst);
                }
                else
                {
                    ddlExamName.Items.Insert(0, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
}