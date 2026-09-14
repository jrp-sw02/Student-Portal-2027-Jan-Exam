using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.Entity.SqlServer;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
//November_2024
using System.Data.SqlClient;


public partial class HO_NEFTDetailupload : BasePage
{
    Int32 currentRoleId = 0;
    Int32 total = 0;

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
                ViewState["SortField"] = "";
                ViewState["SortOrder"] = "";
                txtflFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                if (gvMain.Rows.Count <= 0)
                {
                    lblError.Text = "Please Select Filter Criteria to View NEFT/RTGS Records";
                    lblError.Visible = true;

                }
                bindpaymentmode();
                //ddlpaymentmode.Items.RemoveAt(1); // temporary
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NEFT/RTGS Bank Detail Upload", "", ""));
            }
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
            BreadCrumb1.Render();
            int failedRecordCount = 0;
            string FaildRecords = "Data Already uploaded  for following UTR Number:-";
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
            if (!IsValidData(sExcelConnectionString))
                return;
            divValidateData.Visible = true;
            btnSave.Visible = true;
            OleDbConnection connection = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            connection.ConnectionString = sExcelConnectionString;
            connection.Open();
            Int32 paymodeid = Convert.ToInt32(ddlpaymentmode.SelectedValue);

            command = new OleDbCommand("select * from [NEFT_RTGS$]", connection);

            OleDbDataReader dr = command.ExecuteReader();
            String utrnumber = String.Empty;
            int TotalRecords = 0;
            int ValidateRecords = 0;
            try
            {
                //using (TransactionScope scope = new TransactionScope())
                //{
                using (EConnectContext context = new EConnectContext())
                {
                    while (dr.Read())
                    {
                        if (CommonFunctions.IsNumeric(dr[0].ToString()))
                        {
                            utrnumber = Convert.ToString(dr[1]);
                            utrnumber = utrnumber.Trim().ToUpper();
                            TotalRecords = TotalRecords + 1;
                        }
                        else
                        {
                            continue;
                        }
                        var neft = context.NEFTBankTransactions.Where(s => s.UTRNUMBER.Trim().ToUpper() == utrnumber).FirstOrDefault();
                        if (neft == null)
                        {
                            try
                            {
                                NEFTBankTransaction neftdetail = new NEFTBankTransaction();
                                neftdetail.UTRNUMBER = Convert.ToString(dr[1]).Trim();
                                neftdetail.SenderIfsc = Convert.ToString(dr[2]).Trim();
                                neftdetail.SenderName = Convert.ToString(dr[3]).Trim();
                                neftdetail.TransactionDate = Convert.ToDateTime(dr[4].ToString());
                                neftdetail.TransactionAmt = Convert.ToInt32(System.Math.Floor(Convert.ToDecimal(dr[5])));
                                neftdetail.TransactionID = "";
                                neftdetail.UploadedDate = DateTime.Now;
                                context.NEFTBankTransactions.Add(neftdetail);
                                context.SaveChanges();
                                ValidateRecords = ValidateRecords + 1;
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                };
                //scope.Complete();
                //};
                failedRecordCount = TotalRecords - ValidateRecords;
                lblTotalRecords.Text = TotalRecords.ToString();
                lblValidateRecords.Text = ValidateRecords.ToString();
                lblFailedRecords.Text = failedRecordCount.ToString() + " ( Data Already Uploaded )";
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
                               where p.ID == NEFTRTGS
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
            mltvTab.ActiveViewIndex = 0;
            btnFilter.Visible = true;
            pnlFilter.Visible = true;
            ucSearchBar.Visible = true;
            btnMode.ViewMode = ToggleView.Mode.New;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected Boolean IsValidData(string sExcelConnectionString)
    {

        OleDbConnection connection = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();
        connection.ConnectionString = sExcelConnectionString;
        connection.Open();
        Int32 paymodeid = Convert.ToInt32(ddlpaymentmode.SelectedValue);

        command = new OleDbCommand("select * from [NEFT_RTGS$]", connection);

        OleDbDataReader dr = command.ExecuteReader();
        String utrnumber = String.Empty;

        try
        {
            while (dr.Read())
            {
                if (!CommonFunctions.IsNumeric(dr[0].ToString()))
                    continue;
                try
                {
                    DateTime date = new DateTime();
                    date = Convert.ToDateTime(dr[4].ToString());

                }
                catch (Exception ex)
                {
                    ShowAlert("Please check the data. The date format must be mm/dd/yyyy.");
                    return false;
                }
            }
            return true;
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
            return false;
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
    protected void ToggleViewMode_Changed(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (btnMode.ViewMode == ToggleView.Mode.New)
            {
                mltvTab.ActiveViewIndex = 1;
                pnlFilter.Visible = false;
                btnMode.ViewMode = ToggleView.Mode.List;
                ucSearchBar.Visible = false;
            }
            else
            {
                mltvTab.ActiveViewIndex = 0;
                pnlFilter.Visible = true;
                ucSearchBar.Visible = true;
                btnMode.ViewMode = ToggleView.Mode.New;
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        try
        {
            lblOne.Text = ddlStatus.SelectedItem.ToString() + " From " + txtflFromDate.Text + " To " + txtToDate.Text;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtflFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        ddlStatus.SelectedValue = "0";

        Response.Redirect("NEFTDetailupload.aspx");
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
    protected void BindGridView()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                BreadCrumb1.Render();
                lblTotal.Text = "";
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                ucSearchBar.AutoCompleteContextKey = ddlStatus.SelectedValue.ToString();
                upbreadsearch.Update();
                DateTime fromdate = Convert.ToDateTime(txtflFromDate.Text);
                DateTime todate = Convert.ToDateTime(txtToDate.Text);
                if (ddlStatus.SelectedValue == "1")
                {
                    var verified = from nbt in context.NEFTBankTransactions
                                   join
                                       nb in context.NEFTTransactions on nbt.DemandNoteID equals
                                       nb.DemandNoteID
                                   where
                                      System.Data.Entity.DbFunctions.TruncateTime(nbt.TransactionDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate)
                                      && System.Data.Entity.DbFunctions.TruncateTime(nbt.TransactionDate) <= System.Data.Entity.DbFunctions.TruncateTime(todate)
                                      && nbt.DemandNoteID != null
                                   select new
                                   {
                                       ID = nbt.ID,
                                       UtrNo = nbt.UTRNUMBER,
                                       SenderIFSC = nbt.SenderIfsc,
                                       SenderName = nbt.SenderName,
                                       TranDate = nbt.TransactionDate,
                                       Amount = nbt.TransactionAmt,
                                       uploadeddate = nbt.UploadedDate.HasValue ? nbt.UploadedDate : null,
                                       Status = "Verified" + " ( " + SqlFunctions.StringConvert((double?)nbt.DemandNoteID) + " ) "
                                   };
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        verified = verified.Where(s => s.UtrNo.ToUpper().Trim().Contains(searchString));
                    }
                    if (verified.Count() > 0)
                    {
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "UtrNo":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.UtrNo);
                                    else
                                        verified = verified.OrderBy(s => s.UtrNo);
                                    break;
                                case "SenderIFSC":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.SenderIFSC);
                                    else
                                        verified = verified.OrderBy(s => s.SenderIFSC);
                                    break;
                                case "SenderName":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.SenderName);
                                    else
                                        verified = verified.OrderBy(s => s.SenderName);
                                    break;
                                case "TranDate":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.TranDate);
                                    else
                                        verified = verified.OrderBy(s => s.TranDate);
                                    break;
                                case "Amount":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.Amount);
                                    else
                                        verified = verified.OrderBy(s => s.Amount);
                                    break;
                                case "uploadeddate":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.uploadeddate);
                                    else
                                        verified = verified.OrderBy(s => s.uploadeddate);
                                    break;
                                default:
                                    verified = verified.OrderBy(s => s.ID);
                                    break;
                            }
                        }
                        gvMain.Visible = true;
                        lblError.Visible = false;
                        PagingBar1.Bind(verified, ref gvMain);
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                        lblTotal.Visible = true;
                        lbltxttotal.Visible = true;
                        pnlMain.Visible = true;
                        lblTotal.Text = verified.Sum(a => a.Amount).ToString();
                        ibExport.Visible = true;
                        //imPrint.Visible = true;
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblTotal.Visible = false;
                        lbltxttotal.Visible = false;
                        lblError.Text = "No Record Found";
                        gvMain.Visible = false;
                        pnlMain.Visible = false;
                    }
                }
                else if (ddlStatus.SelectedValue == "2")
                {
                    Int32[] neftbanktransactionid = context.NEFTRefund.Select(t => t.NeftBankTransactionID).ToArray();
                    var unverified = from nbt in context.NEFTBankTransactions
                                     where
                                     System.Data.Entity.DbFunctions.TruncateTime(nbt.TransactionDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate)
                                     && System.Data.Entity.DbFunctions.TruncateTime(nbt.TransactionDate) <= System.Data.Entity.DbFunctions.TruncateTime(todate)
                                     && nbt.DemandNoteID == null && !neftbanktransactionid.Contains(nbt.ID)
                                     select new
                                     {
                                         ID = nbt.ID,
                                         UtrNo = nbt.UTRNUMBER,
                                         SenderIFSC = nbt.SenderIfsc,
                                         SenderName = nbt.SenderName,
                                         TranDate = nbt.TransactionDate,
                                         Amount = nbt.TransactionAmt,
                                         uploadeddate = nbt.UploadedDate.HasValue ? nbt.UploadedDate : null,
                                         Status = "Not Verified"
                                     };

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        unverified = unverified.Where(a => a.UtrNo.ToUpper().Trim().Contains(searchString));
                    }
                    if (unverified.Count() > 0)
                    {
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "UtrNo":
                                    if (sortOrder == "DESC")
                                        unverified = unverified.OrderByDescending(s => s.UtrNo);
                                    else
                                        unverified = unverified.OrderBy(s => s.UtrNo);
                                    break;
                                case "SenderIFSC":
                                    if (sortOrder == "DESC")
                                        unverified = unverified.OrderByDescending(s => s.SenderIFSC);
                                    else
                                        unverified = unverified.OrderBy(s => s.SenderIFSC);
                                    break;
                                case "SenderName":
                                    if (sortOrder == "DESC")
                                        unverified = unverified.OrderByDescending(s => s.SenderName);
                                    else
                                        unverified = unverified.OrderBy(s => s.SenderName);
                                    break;
                                case "TranDate":
                                    if (sortOrder == "DESC")
                                        unverified = unverified.OrderByDescending(s => s.TranDate);
                                    else
                                        unverified = unverified.OrderBy(s => s.TranDate);
                                    break;
                                case "Amount":
                                    if (sortOrder == "DESC")
                                        unverified = unverified.OrderByDescending(s => s.Amount);
                                    else
                                        unverified = unverified.OrderBy(s => s.Amount);
                                    break;
                                case "uploadeddate":
                                    if (sortOrder == "DESC")
                                        unverified = unverified.OrderByDescending(s => s.uploadeddate);
                                    else
                                        unverified = unverified.OrderBy(s => s.uploadeddate);
                                    break;
                                default:
                                    unverified = unverified.OrderBy(s => s.ID);
                                    break;
                            }
                        }
                        PagingBar1.Bind(unverified, ref gvMain);
                        uPnlGrid.Update();
                        lblError.Visible = false;
                        uPnlNavigation.Update();
                        lblTotal.Visible = true;
                        lbltxttotal.Visible = true;
                        lblTotal.Text = unverified.Sum(a => a.Amount).ToString();
                        ibExport.Visible = true;
                        //imPrint.Visible = true;
                        gvMain.Visible = true;
                        pnlMain.Visible = true;

                    }
                    else
                    {
                        lblError.Visible = true;
                        lblTotal.Visible = false;
                        lbltxttotal.Visible = false;
                        lblError.Text = "No Record Found";
                        gvMain.Visible = false;
                        pnlMain.Visible = false;
                    }
                }
                else if (ddlStatus.SelectedValue == "3")
                {
                    var refunded = from nbt in context.NEFTBankTransactions
                                   join nf in context.NEFTRefund on nbt.ID equals nf.NeftBankTransactionID
                                   where
                                   System.Data.Entity.DbFunctions.TruncateTime(nbt.TransactionDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate)
                                   && System.Data.Entity.DbFunctions.TruncateTime(nbt.TransactionDate) <= System.Data.Entity.DbFunctions.TruncateTime(todate)
                                   && nbt.DemandNoteID == null
                                   select new
                                   {
                                       ID = nf.NeftBankTransactionID,
                                       UtrNo = nbt.UTRNUMBER,
                                       SenderIFSC = nbt.SenderIfsc,
                                       SenderName = nbt.SenderName,
                                       TranDate = nbt.TransactionDate,
                                       Amount = nbt.TransactionAmt,
                                       uploadeddate = nbt.UploadedDate.HasValue ? nbt.UploadedDate : null,
                                       Status = "Refunded"
                                   };
                    if (refunded.Count() > 0)
                    {
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "UtrNo":
                                    if (sortOrder == "DESC")
                                        refunded = refunded.OrderByDescending(s => s.UtrNo);
                                    else
                                        refunded = refunded.OrderBy(s => s.UtrNo);
                                    break;
                                case "SenderIFSC":
                                    if (sortOrder == "DESC")
                                        refunded = refunded.OrderByDescending(s => s.SenderIFSC);
                                    else
                                        refunded = refunded.OrderBy(s => s.SenderIFSC);
                                    break;
                                case "SenderName":
                                    if (sortOrder == "DESC")
                                        refunded = refunded.OrderByDescending(s => s.SenderName);
                                    else
                                        refunded = refunded.OrderBy(s => s.SenderName);
                                    break;
                                case "TranDate":
                                    if (sortOrder == "DESC")
                                        refunded = refunded.OrderByDescending(s => s.TranDate);
                                    else
                                        refunded = refunded.OrderBy(s => s.TranDate);
                                    break;
                                case "Amount":
                                    if (sortOrder == "DESC")
                                        refunded = refunded.OrderByDescending(s => s.Amount);
                                    else
                                        refunded = refunded.OrderBy(s => s.Amount);
                                    break;
                                case "uploadeddate":
                                    if (sortOrder == "DESC")
                                        refunded = refunded.OrderByDescending(s => s.uploadeddate);
                                    else
                                        refunded = refunded.OrderBy(s => s.uploadeddate);
                                    break;
                                default:
                                    refunded = refunded.OrderBy(s => s.ID);
                                    break;
                            }
                        }
                        PagingBar1.Bind(refunded, ref gvMain);
                        uPnlGrid.Update();
                        lblError.Visible = false;
                        uPnlNavigation.Update();
                        lblTotal.Visible = true;
                        lbltxttotal.Visible = true;
                        lblTotal.Text = refunded.Sum(a => a.Amount).ToString();
                        ibExport.Visible = true;
                        //imPrint.Visible = true;
                        gvMain.Visible = true;
                        pnlMain.Visible = true;
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblTotal.Visible = false;
                        lbltxttotal.Visible = false;
                        lblError.Text = "No Record Found";
                        gvMain.Visible = false;
                        pnlMain.Visible = false;
                    }

                }
                else if (ddlStatus.SelectedValue == "4")
                {
                    var verified = from nbt in context.NEFTBankTransactions
                                   join
                                       nb in context.NEFTTransactions on nbt.DemandNoteID equals
                                       nb.DemandNoteID
                                   where
                                      System.Data.Entity.DbFunctions.TruncateTime(nbt.TransactionDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate)
                                      && System.Data.Entity.DbFunctions.TruncateTime(nbt.TransactionDate) <= System.Data.Entity.DbFunctions.TruncateTime(todate)
                                      && nbt.DemandNoteID != null
                                   select new
                                   {
                                       ID = nbt.ID,
                                       UtrNo = nbt.UTRNUMBER,
                                       SenderIFSC = nbt.SenderIfsc,
                                       SenderName = nbt.SenderName,
                                       TranDate = nbt.TransactionDate,
                                       Amount = nbt.TransactionAmt,
                                       uploadeddate = nbt.UploadedDate.HasValue ? nbt.UploadedDate : null,
                                       Status = "Verified" + " ( " + SqlFunctions.StringConvert((double?)nbt.DemandNoteID) + " ) "
                                   };


                    Int32[] neftbanktransactionid = context.NEFTRefund.Select(t => t.NeftBankTransactionID).ToArray();
                    var unverified = from nbt in context.NEFTBankTransactions
                                     where
                                     System.Data.Entity.DbFunctions.TruncateTime(nbt.TransactionDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate)
                                     && System.Data.Entity.DbFunctions.TruncateTime(nbt.TransactionDate) <= System.Data.Entity.DbFunctions.TruncateTime(todate)
                                     && nbt.DemandNoteID == null && !neftbanktransactionid.Contains(nbt.ID)
                                     select new
                                     {
                                         ID = nbt.ID,
                                         UtrNo = nbt.UTRNUMBER,
                                         SenderIFSC = nbt.SenderIfsc,
                                         SenderName = nbt.SenderName,
                                         TranDate = nbt.TransactionDate,
                                         Amount = nbt.TransactionAmt,
                                         uploadeddate = nbt.UploadedDate.HasValue ? nbt.UploadedDate : null,
                                         Status = "Not Verified"
                                     };


                    var refunded = from nbt in context.NEFTBankTransactions
                                   join nf in context.NEFTRefund on nbt.ID equals nf.NeftBankTransactionID
                                   where
                                   System.Data.Entity.DbFunctions.TruncateTime(nbt.TransactionDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate)
                                   && System.Data.Entity.DbFunctions.TruncateTime(nbt.TransactionDate) <= System.Data.Entity.DbFunctions.TruncateTime(todate)
                                   && nbt.DemandNoteID == null
                                   select new
                                   {
                                       ID = nf.NeftBankTransactionID,
                                       UtrNo = nf.UTRNumber,
                                       SenderIFSC = nf.IFSCCode,
                                       SenderName = nbt.SenderName,
                                       TranDate = nbt.TransactionDate,
                                       Amount = nbt.TransactionAmt,
                                       uploadeddate = nbt.UploadedDate.HasValue ? nbt.UploadedDate : null,
                                       Status = "Refunded"
                                   };
                    var combinedresult = verified.Union(unverified.Union(refunded));

                    if (combinedresult.Count() > 0)
                    {
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "UtrNo":
                                    if (sortOrder == "DESC")
                                        combinedresult = combinedresult.OrderByDescending(s => s.UtrNo);
                                    else
                                        combinedresult = combinedresult.OrderBy(s => s.UtrNo);
                                    break;
                                case "SenderIFSC":
                                    if (sortOrder == "DESC")
                                        combinedresult = combinedresult.OrderByDescending(s => s.SenderIFSC);
                                    else
                                        unverified = unverified.OrderBy(s => s.SenderIFSC);
                                    break;
                                case "SenderName":
                                    if (sortOrder == "DESC")
                                        combinedresult = combinedresult.OrderByDescending(s => s.SenderName);
                                    else
                                        combinedresult = combinedresult.OrderBy(s => s.SenderName);
                                    break;
                                case "TranDate":
                                    if (sortOrder == "DESC")
                                        combinedresult = combinedresult.OrderByDescending(s => s.TranDate);
                                    else
                                        combinedresult = combinedresult.OrderBy(s => s.TranDate);
                                    break;
                                case "Amount":
                                    if (sortOrder == "DESC")
                                        combinedresult = combinedresult.OrderByDescending(s => s.Amount);
                                    else
                                        combinedresult = combinedresult.OrderBy(s => s.Amount);
                                    break;
                                case "uploadeddate":
                                    if (sortOrder == "DESC")
                                        combinedresult = combinedresult.OrderByDescending(s => s.uploadeddate);
                                    else
                                        combinedresult = combinedresult.OrderBy(s => s.uploadeddate);
                                    break;
                                default:
                                    combinedresult = combinedresult.OrderBy(s => s.ID);
                                    break;
                            }
                        }
                        PagingBar1.Bind(combinedresult, ref gvMain);
                        uPnlGrid.Update();
                        lblError.Visible = false;
                        uPnlNavigation.Update();
                        lblTotal.Visible = true;
                        lbltxttotal.Visible = true;
                        lblTotal.Text = combinedresult.Sum(a => a.Amount).ToString();
                        ibExport.Visible = true;
                        //imPrint.Visible = true;
                        gvMain.Visible = true;
                        pnlMain.Visible = true;
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblTotal.Visible = false;
                        lbltxttotal.Visible = false;
                        lblError.Text = "No Record Found";
                        gvMain.Visible = false;
                        pnlMain.Visible = false;
                    }
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
                Label lb0 = (Label)e.Row.Cells[2].FindControl("lblTran");
                lb0.Text = Convert.ToDateTime(lb0.Text).ToString("dd-MMM-yyyy");

                Label lbutrno = (Label)e.Row.Cells[1].FindControl("lblUtrNo");
                lbutrno.Text = lbutrno.Text.ToString().Substring(0, lbutrno.Text.Length / 2) + (new String('X', lbutrno.Text.Length - lbutrno.Text.Length / 2));

                Label lbuploadedDate = (Label)e.Row.Cells[6].FindControl("lbluploadeddate");
                lbuploadedDate.Text = Convert.ToDateTime(lbuploadedDate.Text).ToString("dd-MMM-yyyy");
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
            //Int32 courseType = Convert.ToInt32(enmCourseType.CertificationCourse);
            List<String> items = new List<String>();
            string searchString = prefixText.Trim().ToUpper();
            string StatusID = contextKey;
            if (StatusID == "1")
            {
                var utrno = from s in context.NEFTBankTransactions
                            where s.DemandNoteID != null
                            select new { transNo = s.UTRNUMBER };
                if (!String.IsNullOrEmpty(searchString))
                {
                    utrno = utrno.Where(s => s.transNo.ToUpper().Contains(searchString));
                }
                utrno = utrno.OrderBy(s => s.transNo).Take(count);
                foreach (var c in utrno)
                {
                    items.Add(c.transNo.Trim());
                }
            }
            else if (StatusID == "2")
            {
                var utrno = from s in context.NEFTBankTransactions
                            where s.DemandNoteID == null
                            select new { transNo = s.UTRNUMBER };
                if (!String.IsNullOrEmpty(searchString))
                {
                    utrno = utrno.Where(s => s.transNo.ToUpper().Contains(searchString));
                }
                utrno = utrno.OrderBy(s => s.transNo).Take(count);
                foreach (var c in utrno)
                {
                    items.Add(c.transNo.Trim());
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
    protected void SearchBar_ApplySearch(object sender, EventArgs e)
    {
        try
        {
            //BindGridView();
            BreadCrumb1.Render();
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            ibExport.Visible = false;
            //imPrint.Visible = false;
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
            //BindGridView();
            BreadCrumb1.Render();
            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
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
    protected void Export(object sender, ImageClickEventArgs e)
    {
        EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
        try
        {
            //EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            con.Open();
            DataTable dt = new DataTable();
            BreadCrumb1.Render();

            //Dynamic Array for Paramters_01_11_2024
            List<SqlParameter> paramList = new List<SqlParameter>();                 //November_2024

            DateTime fromdate = Convert.ToDateTime(txtflFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtToDate.Text);
            if (ddlStatus.SelectedValue == "1")
            {
                //string strQuery = "select SUBSTRING(nbt.UTR_NUMBER,1,6) + REPLICATE('X',LEN(nbt.UTR_NUMBER)-6) as UTR_Number,nbt.Sender_Ifsc,nbt.Sender_Name,REPLACE(CONVERT(Varchar, nbt.Transaction_Date, 106),' ','-')as Transaction_Date ,nbt.Transaction_Amt, REPLACE(CONVERT(Varchar, nbt.Uploaded_Date, 106),' ','-')as Uploaded_Date,'Verified'+ ' ( ' + cast(nbt.Demand_Note_ID as varchar) + ' ) ' as Status from Neft_Bank_Transaction nbt inner join NEFT_Transaction nt on " +
                //                 " nbt.Demand_Note_ID=nt.Demand_Note_ID where cast(nbt.Transaction_Date as DATE) >='" + fromdate.ToString("dd-MMM-yyyy") + "'and cast(nbt.Transaction_Date as DATE) <='" + todate.ToString("dd-MMM-yyyy") + "'and nbt.Demand_Note_ID Is Not Null";


                //November_2024
                string strQuery = "select SUBSTRING(nbt.UTR_NUMBER,1,6) + REPLICATE('X',LEN(nbt.UTR_NUMBER)-6) as UTR_Number,nbt.Sender_Ifsc,nbt.Sender_Name,REPLACE(CONVERT(Varchar, nbt.Transaction_Date, 106),' ','-')as Transaction_Date ,nbt.Transaction_Amt, REPLACE(CONVERT(Varchar, nbt.Uploaded_Date, 106),' ','-')as Uploaded_Date,'Verified'+ ' ( ' + cast(nbt.Demand_Note_ID as varchar) + ' ) ' as Status from Neft_Bank_Transaction nbt inner join NEFT_Transaction nt on " +
                                " nbt.Demand_Note_ID=nt.Demand_Note_ID where cast(nbt.Transaction_Date as DATE) >=@fromDate and cast(nbt.Transaction_Date as DATE) <=@toDate and nbt.Demand_Note_ID Is Not Null";
                paramList.Add(new SqlParameter("@fromDate", fromdate.ToString("dd-MMM-yyyy")));
                paramList.Add(new SqlParameter("@toDate", todate.ToString("dd-MMM-yyyy")));


                string sheetname = "NEFT_RTGS";
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                if (!String.IsNullOrEmpty(searchString))
                {
                    //strQuery += " and UPPER(nbt.UTR_NUMBER)='" + searchString + "'";

                    //November_2024
                    strQuery += " and UPPER(nbt.UTR_NUMBER)=@searchString";
                    paramList.Add(new SqlParameter("@searchString", searchString));

                }
                //dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, null, CommandType.Text, true);
                //November_2024
                dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, paramList.ToArray(), CommandType.Text, true);

                if (dt.Rows.Count > 0)
                {
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    GridView1.Caption = ddlStatus.SelectedItem.Text + "_NEFT/RTGS_DETAILS_" + DateTime.Now.ToLongDateString();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + sheetname + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        //Apply text style to each Row
                        GridView1.Rows[i].Attributes.Add("class", "textmode");
                    }
                    GridView1.RenderControl(hw);

                    //style to format numbers to string

                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                    con.Close();
                }
                else
                {
                    ShowAlert("No record found.");
                    con.Close();
                }

            }
            else if (ddlStatus.SelectedValue == "2")
            {
                //string strQuery = "select SUBSTRING(UTR_NUMBER,1,6) + REPLICATE('X',LEN(UTR_NUMBER)-6) as UTR_Number ,Sender_Ifsc,Sender_Name,REPLACE(CONVERT(Varchar, Transaction_Date, 106),' ','-')as Transaction_Date,Transaction_Amt, REPLACE(CONVERT(Varchar, Uploaded_Date, 106),' ','-') as Uploaded_Date, 'Not Verified' as Status from Neft_Bank_Transaction  " +
                //               " where cast(Transaction_Date as DATE) >='" + fromdate.ToString("dd-MMM-yyyy") + "'and cast(Transaction_Date as DATE) <='" + todate.ToString("dd-MMM-yyyy") + "' and Demand_Note_ID Is Null and ID not in ( select Neft_Bank_Transaction_ID from NEFT_Refund) ";

                //November_2024
                string strQuery = "select SUBSTRING(UTR_NUMBER,1,6) + REPLICATE('X',LEN(UTR_NUMBER)-6) as UTR_Number ,Sender_Ifsc,Sender_Name,REPLACE(CONVERT(Varchar, Transaction_Date, 106),' ','-')as Transaction_Date,Transaction_Amt, REPLACE(CONVERT(Varchar, Uploaded_Date, 106),' ','-') as Uploaded_Date, 'Not Verified' as Status from Neft_Bank_Transaction  " +
                               " where cast(Transaction_Date as DATE) >=@fromDate and cast(Transaction_Date as DATE) <=@toDate and Demand_Note_ID Is Null and ID not in ( select Neft_Bank_Transaction_ID from NEFT_Refund) ";


                paramList.Add(new SqlParameter("@fromDate", fromdate.ToString("dd-MMM-yyyy")));
                paramList.Add(new SqlParameter("@toDate", todate.ToString("dd-MMM-yyyy")));

                string sheetname = "NEFT_RTGS";
                string searchString = ucSearchBar.SearchText.Trim().ToUpper();
                if (!String.IsNullOrEmpty(searchString))
                {
                    //strQuery += " and Upper(UTR_NUMBER)='" + searchString + "'";

                    //November_2024
                    strQuery += " and UPPER(nbt.UTR_NUMBER)=@searchString";
                    paramList.Add(new SqlParameter("@searchString", searchString));
                }
                //dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, null, CommandType.Text, true);
                //November_2024
                dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, paramList.ToArray(), CommandType.Text, true);
                if (dt.Rows.Count > 0)
                {
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    GridView1.Caption = ddlStatus.SelectedItem.Text + "_NEFT/RTGS_DETAILS_" + DateTime.Now.ToLongDateString();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + sheetname + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    for (int i = 0; i < GridView1.Rows.Count; i++)
                    {
                        //Apply text style to each Row
                        GridView1.Rows[i].Attributes.Add("class", "textmode");
                    }
                    GridView1.RenderControl(hw);

                    //style to format numbers to string

                    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                    con.Close();
                }
                else
                {
                    ShowAlert("No record found.");
                    con.Close();
                }
            }
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
            con.Close();
        }
    }

}