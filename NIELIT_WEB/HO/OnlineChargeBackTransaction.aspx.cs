using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.Data.SqlClient;                        //November_2024
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class HO_OnlineChargeBackTransaction : BasePage
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
                    lblError.Text = "Please Select Filter Criteria to View Online ChargeBackTransaction Records";
                    lblError.Visible = true;

                }
                bindpaymentmode();
                //ddlpaymentmode.Items.RemoveAt(1); // temporary
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Online ChargeBackTransaction Details", "", ""));
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
                sExcelConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";", path);
            else if (ext.ToUpper() == ".XLSX")
                sExcelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\";", path);
            else
            {
                ShowAlert("Please Choose .XLS/.XLSX Excel File.", true);
                return;
            }
            //if (!IsValidData(sExcelConnectionString))
            //    return;

            List<string> refernce3 = new List<string>(); 

            //Previous code
            //refernce3.Add("REGN01");
            //refernce3.Add("EXAM01");

            divValidateData.Visible = true;
            btnSave.Visible = true;
            OleDbConnection connection = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            connection.ConnectionString = sExcelConnectionString;
            connection.Open();
            Int32 paymodeid = Convert.ToInt32(ddlpaymentmode.SelectedValue);

            command = new OleDbCommand("select * from [ChargeBack$]", connection);

            OleDbDataReader dr = command.ExecuteReader();
            String bdreferenceno = String.Empty;
            Int32 Reference1 = 0;
            Int64 RefundID = 0;
            int TotalRecords = 0;
            int ValidateRecords = 0;
            int failedRecord1 = 0;
            int failedRecord2 = 0;
            int failedRecord3 = 0;
            try
            {
                using (EConnectContext context = new EConnectContext())
                {

                    //code changed on 14-Oct-2014 to use separate service-id for courses.
                    var ServiceID = (from r in context.Courses
                                     select new
                                     {
                                         RegistrationServiceID = r.RegistrationServiceID,
                                         ExaminationServiceID = r.ExaminationServiceID
                                     }).ToList();

                    foreach (var service in ServiceID)
                    {
                        refernce3.Add(service.ExaminationServiceID);
                        refernce3.Add(service.RegistrationServiceID);
                    }

                    while (dr.Read())
                    {
                        TotalRecords = TotalRecords + 1;
                        if (CommonFunctions.IsNumeric(dr[6].ToString()) && CommonFunctions.IsNumeric(dr[7].ToString()))
                        {
                            if (refernce3.Contains(dr[8].ToString()))
                            {
                                Reference1 = Convert.ToInt32(dr[6]);
                            }
                            else
                            {
                                failedRecord1 = failedRecord1 + 1;
                                continue;
                            }
                        }
                        else
                        {
                            failedRecord1 = failedRecord1 + 1;
                            continue;
                        }
                        bdreferenceno = Convert.ToString(dr[4]).Trim().ToUpper();
                        RefundID = Convert.ToInt64(dr[12].ToString());
                        if (context.OnlineTransaction.Where(s => s.ID == Reference1).Count() > 0)
                        {
                            var charge = context.Online_ChargeBackTransactions.Where(s => s.BDReferenceNo.Trim().ToUpper() == bdreferenceno && s.RefundID == RefundID && s.Ref1 == Reference1).FirstOrDefault();
                            if (charge == null)
                            {
                                try
                                {
                                    Online_ChargeBackTransaction chargeBack = new Online_ChargeBackTransaction();
                                    chargeBack.BillerName = Convert.ToString(dr[0]).Trim();
                                    chargeBack.DebitType = Convert.ToString(dr[1]).Trim();
                                    chargeBack.PayMode = Convert.ToString(dr[2]).Trim();
                                    chargeBack.Productcode = Convert.ToString(dr[3]).Trim();
                                    chargeBack.BDReferenceNo = Convert.ToString(dr[4]).Trim();
                                    chargeBack.BillDeskID = Convert.ToString(dr[5]).Trim();
                                    chargeBack.Ref1 = Convert.ToInt32(dr[6]);
                                    chargeBack.Ref2 = Convert.ToInt32(dr[7]);
                                    chargeBack.Ref3 = Convert.ToString(dr[8]).Trim();
                                    chargeBack.Ref4 = Convert.ToString(dr[9]).Trim();
                                    //created_on date
                                    string[] date = dr[10].ToString().Substring(0, 10).Split('/');
                                    DateTime createdon = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));
                                    chargeBack.CreatedOn = createdon;
                                    chargeBack.TransactionAmount = Convert.ToInt64(System.Math.Floor(Convert.ToDecimal(dr[11])));
                                    chargeBack.RefundID = Convert.ToInt64(dr[12].ToString());
                                    //refund_date
                                    string[] date1 = dr[13].ToString().Substring(0, 10).Split('/');
                                    DateTime refunddate = new DateTime(Convert.ToInt32(date1[2]), Convert.ToInt32(date1[1]), Convert.ToInt32(date1[0]));
                                    chargeBack.RefundDate = refunddate;
                                    chargeBack.RefundAmount = Convert.ToInt64(System.Math.Floor(Convert.ToDecimal(dr[14])));
                                    chargeBack.Date = DateTime.Now;
                                    context.Online_ChargeBackTransactions.Add(chargeBack);
                                    context.SaveChanges();
                                    ValidateRecords = ValidateRecords + 1;
                                }
                                catch (Exception ex)
                                {
                                    failedRecord1 = failedRecord1 + 1;
                                }
                            }
                            else
                            {
                                failedRecord3 = failedRecord3 + 1;
                            }
                        }
                        else
                        {
                            failedRecord2 = failedRecord2 + 1;
                        }
                    }
                };
                failedRecordCount = TotalRecords - ValidateRecords;
                lblTotalRecords.Text = TotalRecords.ToString();
                lblValidateRecords.Text = ValidateRecords.ToString();
                lblFailedRecords.Text = "<b> 1. Incorrect Data for following Records :- " + failedRecord1 + "</br>" + " 2. Data not found for following Records :- " + failedRecord2 + "</br>" + " 3. Data Already Uploaded for Following Records :- </b> " + failedRecord3;
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
                Int32 Online = Convert.ToInt32(enmPaymentMode.Online);
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.PaymentModes
                               where p.ID == Online
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
            btnMode.ViewMode = ToggleView.Mode.New;
            Response.Redirect("OnlineChargeBackTransaction.aspx");
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
            }
            else
            {
                mltvTab.ActiveViewIndex = 0;
                pnlFilter.Visible = true;
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

            PagingBar1.CurrentPageIndex = 0;
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
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
        Response.Redirect("OnlineChargeBackTransaction.aspx");
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
                string sortOrder = ViewState["SortOrder"].ToString();
                string sortField = ViewState["SortField"].ToString();
                DateTime fromdate = Convert.ToDateTime(txtflFromDate.Text);
                DateTime todate = Convert.ToDateTime(txtToDate.Text);
                Int32 status = Convert.ToInt32(ddlStatus.SelectedValue);
                Int32 datetype = Convert.ToInt32(ddldatetype.SelectedValue);
                if (!string.IsNullOrEmpty(fromdate.ToString()) && !string.IsNullOrEmpty(todate.ToString()) && status != 0 && datetype != 0)
                {
                    if (status == 1)
                    {
                        var verified = from nbt in context.Online_ChargeBackTransactions join c in context.OnlineTransaction 
                                       on nbt.Ref1 equals c.ID
                                       where c.ResponseStatusCode == "0300"
                                       select new
                                       {
                                           ID = nbt.ID,
                                           bdreferenceNo = nbt.BDReferenceNo,
                                           billername = nbt.BillerName,
                                           pmode = nbt.PayMode,
                                           pcode = nbt.Productcode,
                                           refundDate = nbt.RefundDate,
                                           Amount = nbt.RefundAmount,
                                           createdon =  nbt.CreatedOn,
                                           uploadeddate = nbt.Date 
                                       };
                         if(datetype == 1)
                         {
                             verified = verified.Where(s=> System.Data.Entity.DbFunctions.TruncateTime(s.createdon) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate)
                                                       && System.Data.Entity.DbFunctions.TruncateTime(s.createdon) <= System.Data.Entity.DbFunctions.TruncateTime(todate));
                         }
                         else if(datetype == 2)
                         {
                             verified = verified.Where(s=> System.Data.Entity.DbFunctions.TruncateTime(s.uploadeddate) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate)
                                                       && System.Data.Entity.DbFunctions.TruncateTime(s.uploadeddate) <= System.Data.Entity.DbFunctions.TruncateTime(todate));
                         }
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "bdreferenceNo":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.bdreferenceNo);
                                    else
                                        verified = verified.OrderBy(s => s.bdreferenceNo);
                                    break;
                                case "billername":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.billername);
                                    else
                                        verified = verified.OrderBy(s => s.billername);
                                    break;
                                case "pmode":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.pmode);
                                    else
                                        verified = verified.OrderBy(s => s.pmode);
                                    break;
                                case "pcode":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.pcode);
                                    else
                                        verified = verified.OrderBy(s => s.pcode);
                                    break;
                                case "refundDate":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.refundDate);
                                    else
                                        verified = verified.OrderBy(s => s.refundDate);
                                    break;
                                case "createdon":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.createdon);
                                    else
                                        verified = verified.OrderBy(s => s.createdon);
                                    break;
                                case "Amount":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.Amount);
                                    else
                                        verified = verified.OrderBy(s => s.Amount);
                                    break;
                                default:
                                    verified = verified.OrderBy(s => s.ID);
                                    break;
                            }
                        }
                        PagingBar1.Bind(verified, ref gvMain);
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                        if (gvMain.Rows.Count <= 0)
                        {
                            lblError.Visible = true;
                            lblTotal.Visible = false;
                            lbltxttotal.Visible = false;
                            lblError.Text = "No Record Found";
                            pnlMain.Visible = false;
                        }
                        else
                        {
                            lblError.Visible = false;
                            lblError.Text = "";
                            lblTotal.Visible = true;
                            lbltxttotal.Visible = true;
                            pnlMain.Visible = true;
                            lblTotal.Text = verified.Sum(a => a.Amount).ToString();
                            ibExport.Visible = true;
                        }
                    }
                    else if(status == 2)
                    {
                            var verified = from nbt in context.Online_ChargeBackTransactions join c in context.OnlineTransaction 
                                       on nbt.Ref1 equals c.ID
                                       where (c.ResponseStatusCode == null || c.ResponseStatusCode != "0300")
                                       select new
                                       {
                                           ID = nbt.ID,
                                           bdreferenceNo = nbt.BDReferenceNo,
                                           billername = nbt.BillerName,
                                           pmode = nbt.PayMode,
                                           pcode = nbt.Productcode,
                                           refundDate = nbt.RefundDate,
                                           Amount = nbt.RefundAmount,
                                           createdon =  nbt.CreatedOn,
                                           uploadeddate = nbt.Date 
                                       };
                            if (datetype == 1)
                            {
                                verified = verified.Where(s => System.Data.Entity.DbFunctions.TruncateTime(s.createdon) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate)
                                                          && System.Data.Entity.DbFunctions.TruncateTime(s.createdon) <= System.Data.Entity.DbFunctions.TruncateTime(todate));
                            }
                            else if (datetype == 2)
                            {
                                verified = verified.Where(s => System.Data.Entity.DbFunctions.TruncateTime(s.uploadeddate) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate)
                                                          && System.Data.Entity.DbFunctions.TruncateTime(s.uploadeddate) <= System.Data.Entity.DbFunctions.TruncateTime(todate));
                            }
                        if (!string.IsNullOrEmpty(sortOrder))
                        {
                            switch (sortField)
                            {
                                case "bdreferenceNo":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.bdreferenceNo);
                                    else
                                        verified = verified.OrderBy(s => s.bdreferenceNo);
                                    break;
                                case "billername":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.billername);
                                    else
                                        verified = verified.OrderBy(s => s.billername);
                                    break;
                                case "pmode":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.pmode);
                                    else
                                        verified = verified.OrderBy(s => s.pmode);
                                    break;
                                case "pcode":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.pcode);
                                    else
                                        verified = verified.OrderBy(s => s.pcode);
                                    break;
                                case "refundDate":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.refundDate);
                                    else
                                        verified = verified.OrderBy(s => s.refundDate);
                                    break;
                                case "createdon":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.createdon);
                                    else
                                        verified = verified.OrderBy(s => s.createdon);
                                    break;
                                case "Amount":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.Amount);
                                    else
                                        verified = verified.OrderBy(s => s.Amount);
                                    break;
                                default:
                                    verified = verified.OrderBy(s => s.ID);
                                    break;
                            }
                        }
                        PagingBar1.Bind(verified, ref gvMain);
                        uPnlGrid.Update();
                        uPnlNavigation.Update();
                        if (gvMain.Rows.Count <= 0)
                        {
                            lblError.Visible = true;
                            lblTotal.Visible = false;
                            lbltxttotal.Visible = false;
                            lblError.Text = "No Record Found";
                            pnlMain.Visible = false;
                        }
                        else
                        {
                            lblError.Visible = false;
                            lblError.Text = "";
                            lblTotal.Visible = true;
                            lbltxttotal.Visible = true;
                            pnlMain.Visible = true;
                            lblTotal.Text = verified.Sum(a => a.Amount).ToString();
                            ibExport.Visible = true;
                        }
                    }
                }
                else
                {
                    Response.Redirect("OnlineChargeBackTransaction.aspx", true);
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
                Label lb0 = (Label)e.Row.Cells[6].FindControl("lblTran");
                lb0.Text = Convert.ToDateTime(lb0.Text).ToString("dd-MMM-yyyy");
                Label lb1 = (Label)e.Row.Cells[5].FindControl("lblcback");
                lb1.Text = Convert.ToDateTime(lb1.Text).ToString("dd-MMM-yyyy");
                //    total += Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "Amount"));
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
            DateTime fromdate = Convert.ToDateTime(txtflFromDate.Text);
            DateTime todate = Convert.ToDateTime(txtToDate.Text);
            Int32 status = Convert.ToInt32(ddlStatus.SelectedValue);
            Int32 datetype = Convert.ToInt32(ddldatetype.SelectedValue);
            string strQuery = String.Empty;
            if (status == 1)
            {
                //November_2024
                SqlParameter[] param1 = { new SqlParameter("@fromDate", fromdate.ToString("dd-MMM-yyyy")),
                                                new SqlParameter("@toDate", todate.ToString("dd-MMM-yyyy")) 
                                            };
                if (datetype == 1)
                {
                    //strQuery = "select nbt.Biller_Name, nbt.Debit_Type, nbt.Pay_Mode, nbt.Product_code, nbt.BD_Reference_No, nbt.BillDesk_ID, nbt.Ref_1,nbt.Ref_2,nbt.Ref_3,nbt.Ref_4, REPLACE(CONVERT(Varchar, nbt.Created_On, 106),' ','-') as ChargeBack_On , REPLACE(CONVERT(Varchar, nbt.Date, 106),' ','-') as Uploaded_On, nbt.Transaction_Amount, nbt.Refund_ID, REPLACE(CONVERT(Varchar, nbt.Refund_Date, 106),' ','-')as Refund_Date , nbt.Refund_Amount from Online_Charge_Back_Transaction nbt inner join Online_Transaction t on  " +
                    //            " nbt.Ref_1 = t.ID and t.Response_Status_Code = '0300' and cast(nbt.Created_On as DATE) >='" + fromdate.ToString("dd-MMM-yyyy") + "'and cast(nbt.Created_On as DATE) <='" + todate.ToString("dd-MMM-yyyy") + "'";

                    //November_2024
                    strQuery = "select nbt.Biller_Name, nbt.Debit_Type, nbt.Pay_Mode, nbt.Product_code, nbt.BD_Reference_No, nbt.BillDesk_ID, nbt.Ref_1,nbt.Ref_2,nbt.Ref_3,nbt.Ref_4, REPLACE(CONVERT(Varchar, nbt.Created_On, 106),' ','-') as ChargeBack_On , REPLACE(CONVERT(Varchar, nbt.Date, 106),' ','-') as Uploaded_On, nbt.Transaction_Amount, nbt.Refund_ID, REPLACE(CONVERT(Varchar, nbt.Refund_Date, 106),' ','-')as Refund_Date , nbt.Refund_Amount from Online_Charge_Back_Transaction nbt inner join Online_Transaction t on  " +
                                " nbt.Ref_1 = t.ID and t.Response_Status_Code = '0300' and cast(nbt.Created_On as DATE) >=@fromDate and cast(nbt.Created_On as DATE) <=@toDate ";
                }
                else if (datetype == 2)
                {
                    //strQuery = "select nbt.Biller_Name, nbt.Debit_Type, nbt.Pay_Mode, nbt.Product_code, nbt.BD_Reference_No, nbt.BillDesk_ID, nbt.Ref_1,nbt.Ref_2,nbt.Ref_3,nbt.Ref_4, REPLACE(CONVERT(Varchar, nbt.Created_On, 106),' ','-')as ChargeBack_On, REPLACE(CONVERT(Varchar, nbt.Date, 106),' ','-') as Uploaded_On , nbt.Transaction_Amount, nbt.Refund_ID, REPLACE(CONVERT(Varchar, nbt.Refund_Date, 106),' ','-')as Refund_Date , nbt.Refund_Amount from Online_Charge_Back_Transaction nbt inner join Online_Transaction t on  " +
                    //           " nbt.Ref_1 = t.ID and t.Response_Status_Code = '0300' and cast(nbt.Date as DATE) >='" + fromdate.ToString("dd-MMM-yyyy") + "'and cast(nbt.Date as DATE) <='" + todate.ToString("dd-MMM-yyyy") + "'";

                    //November_2024
                    strQuery = "select nbt.Biller_Name, nbt.Debit_Type, nbt.Pay_Mode, nbt.Product_code, nbt.BD_Reference_No, nbt.BillDesk_ID, nbt.Ref_1,nbt.Ref_2,nbt.Ref_3,nbt.Ref_4, REPLACE(CONVERT(Varchar, nbt.Created_On, 106),' ','-')as ChargeBack_On, REPLACE(CONVERT(Varchar, nbt.Date, 106),' ','-') as Uploaded_On , nbt.Transaction_Amount, nbt.Refund_ID, REPLACE(CONVERT(Varchar, nbt.Refund_Date, 106),' ','-')as Refund_Date , nbt.Refund_Amount from Online_Charge_Back_Transaction nbt inner join Online_Transaction t on  " +
                               " nbt.Ref_1 = t.ID and t.Response_Status_Code = '0300' and cast(nbt.Date as DATE) >=@fromDate and cast(nbt.Date as DATE) <=@toDate " ;
                }
                string sheetname = ddlStatus.SelectedItem.Text + "_Transaction_Details";
                //dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, null, CommandType.Text, true);
                //November_2024
                dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, param1, CommandType.Text, true);
                if (dt.Rows.Count > 0)
                {
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    GridView1.CaptionAlign = TableCaptionAlign.Left;
                    GridView1.Caption = "<b>" + ddldatetype.SelectedItem.Text + " From : " + fromdate.ToString("dd-MMM-yyyy") + " to " + todate.ToString("dd-MMM-yyyy") + HttpUtility.HtmlDecode("<br/>") + " Date Type :-  " + ddldatetype.SelectedItem.Text + HttpUtility.HtmlDecode("<br/>") + " Status :- " + ddlStatus.SelectedItem.Text + "</b>";
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
                }
            }
            else if (status == 2)
            {

                //November_2024
                SqlParameter[] param2 = { new SqlParameter("@fromDate", fromdate.ToString("dd-MMM-yyyy")),
                                                new SqlParameter("@toDate", todate.ToString("dd-MMM-yyyy"))
                                            };

                if (datetype == 1)
                {
                    //strQuery = "select nbt.Biller_Name, nbt.Debit_Type, nbt.Pay_Mode, nbt.Product_code, nbt.BD_Reference_No, nbt.BillDesk_ID, nbt.Ref_1,nbt.Ref_2,nbt.Ref_3,nbt.Ref_4, REPLACE(CONVERT(Varchar, nbt.Created_On, 106),' ','-') as ChargeBack_On , REPLACE(CONVERT(Varchar, nbt.Date, 106),' ','-') as Uploaded_On, nbt.Transaction_Amount, nbt.Refund_ID, REPLACE(CONVERT(Varchar, nbt.Refund_Date, 106),' ','-')as Refund_Date , nbt.Refund_Amount from Online_Charge_Back_Transaction nbt inner join Online_Transaction t on  " +
                    //            " nbt.Ref_1 = t.ID and (t.Response_Status_Code != '0300' or Response_Status_Code is null) and cast(nbt.Created_On as DATE) >='" + fromdate.ToString("dd-MMM-yyyy") + "'and cast(nbt.Created_On as DATE) <='" + todate.ToString("dd-MMM-yyyy") + "'";

                    //November_2024
                    strQuery = "select nbt.Biller_Name, nbt.Debit_Type, nbt.Pay_Mode, nbt.Product_code, nbt.BD_Reference_No, nbt.BillDesk_ID, nbt.Ref_1,nbt.Ref_2,nbt.Ref_3,nbt.Ref_4, REPLACE(CONVERT(Varchar, nbt.Created_On, 106),' ','-') as ChargeBack_On , REPLACE(CONVERT(Varchar, nbt.Date, 106),' ','-') as Uploaded_On, nbt.Transaction_Amount, nbt.Refund_ID, REPLACE(CONVERT(Varchar, nbt.Refund_Date, 106),' ','-')as Refund_Date , nbt.Refund_Amount from Online_Charge_Back_Transaction nbt inner join Online_Transaction t on  " +
                               " nbt.Ref_1 = t.ID and (t.Response_Status_Code != '0300' or Response_Status_Code is null) and cast(nbt.Created_On as DATE) >=@fromDate and cast(nbt.Created_On as DATE) <=@toDate ";
                }
                else if (datetype == 2)
                {
                    //strQuery = "select nbt.Biller_Name, nbt.Debit_Type, nbt.Pay_Mode, nbt.Product_code, nbt.BD_Reference_No, nbt.BillDesk_ID, nbt.Ref_1,nbt.Ref_2,nbt.Ref_3,nbt.Ref_4, REPLACE(CONVERT(Varchar, nbt.Created_On, 106),' ','-')as ChargeBack_On, REPLACE(CONVERT(Varchar, nbt.Date, 106),' ','-') as Uploaded_On , nbt.Transaction_Amount, nbt.Refund_ID, REPLACE(CONVERT(Varchar, nbt.Refund_Date, 106),' ','-')as Refund_Date , nbt.Refund_Amount from Online_Charge_Back_Transaction nbt inner join Online_Transaction t on  " +
                    //           " nbt.Ref_1 = t.ID and (t.Response_Status_Code != '0300' or Response_Status_Code is null) and cast(nbt.Date as DATE) >='" + fromdate.ToString("dd-MMM-yyyy") + "'and cast(nbt.Date as DATE) <='" + todate.ToString("dd-MMM-yyyy") + "'";

                    //November_2024
                    strQuery = "select nbt.Biller_Name, nbt.Debit_Type, nbt.Pay_Mode, nbt.Product_code, nbt.BD_Reference_No, nbt.BillDesk_ID, nbt.Ref_1,nbt.Ref_2,nbt.Ref_3,nbt.Ref_4, REPLACE(CONVERT(Varchar, nbt.Created_On, 106),' ','-')as ChargeBack_On, REPLACE(CONVERT(Varchar, nbt.Date, 106),' ','-') as Uploaded_On , nbt.Transaction_Amount, nbt.Refund_ID, REPLACE(CONVERT(Varchar, nbt.Refund_Date, 106),' ','-')as Refund_Date , nbt.Refund_Amount from Online_Charge_Back_Transaction nbt inner join Online_Transaction t on  " +
                           " nbt.Ref_1 = t.ID and (t.Response_Status_Code != '0300' or Response_Status_Code is null) and cast(nbt.Date as DATE) >=@fromDate and cast(nbt.Date as DATE) <=@toDate";
                }
                string sheetname = ddlStatus.SelectedItem.Text + "_Transaction_Details";
                //dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, null, CommandType.Text, true);

                //November_2024
                dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, param2, CommandType.Text, true);
                if (dt.Rows.Count > 0)
                {
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    GridView1.CaptionAlign = TableCaptionAlign.Left;
                    GridView1.Caption = "<b>" + ddldatetype.SelectedItem.Text + " From : " + fromdate.ToString("dd-MMM-yyyy") + " to " + todate.ToString("dd-MMM-yyyy") + HttpUtility.HtmlDecode("<br/>") + " Date Type :-  " + ddldatetype.SelectedItem.Text + HttpUtility.HtmlDecode("<br/>") + " Status :- " + ddlStatus.SelectedItem.Text + "</b>";
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