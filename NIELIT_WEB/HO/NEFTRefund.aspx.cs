using System;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.Data.SqlClient;                            //November_2024
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class HO_NEFTRefund : BasePage
{
    Int32 currentRoleId = 0;
    Int32 total = 0;
    EConnectContext context;

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
                    lblError.Text = "Please Select Filter Criteria to View NEFT/RTGS Refund Records";
                    lblError.Visible = true;

                }
                bindpaymentmode();               
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("NEFT/RTGS Refund", "", ""));
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
       
    }
    protected void bindpaymentmode()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
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
            rblServiceType.ClearSelection();
            divmultiple.Visible = false;
            divsingle.Visible = false;
            Response.Redirect("NEFTRefund.aspx");
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
                rblServiceType_SelectedIndexChanged(sender, EventArgs.Empty);
                pnlFilter.Visible = false;
                btnMode.ViewMode = ToggleView.Mode.List;
            }
            else
            {
                mltvTab.ActiveViewIndex = 0;
                pnlFilter.Visible = true;
                btnMode.ViewMode = ToggleView.Mode.New;
                Response.Redirect("NEFTRefund.aspx");
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
        Response.Redirect("NEFTRefund.aspx");
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
                DateTime fromdate = Convert.ToDateTime(txtflFromDate.Text);
                DateTime todate = Convert.ToDateTime(txtToDate.Text);
                Int32 refundmodefilter = Convert.ToInt32(ddlRefundMode.SelectedValue);
                if (refundmodefilter != 0 && !string.IsNullOrEmpty(fromdate.ToString()) && !string.IsNullOrEmpty(todate.ToString()))
                {
                    var verified = from nbt in context.NEFTRefund
                                   where nbt.RefundMode == refundmodefilter &&
                                   System.Data.Entity.DbFunctions.TruncateTime(nbt.Date) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate)
                                   && System.Data.Entity.DbFunctions.TruncateTime(nbt.Date) <= System.Data.Entity.DbFunctions.TruncateTime(todate)
                                   select new
                                   {
                                       ID = nbt.ID,
                                       Chequeno = nbt.ChequeNumber.HasValue ? nbt.ChequeNumber.Value : 0,
                                       accno = nbt.AccountNumber,
                                       IFSC = nbt.IFSCCode,
                                       cname = nbt.ChequeIssuerName,
                                       accholderName = nbt.AccountHolderName,
                                       acctype = nbt.AccountType,
                                       refundDate = nbt.RefundDate,
                                       Amount = nbt.RefundAmount,
                                       reason = nbt.RefundReason,
                                       utrno = nbt.UTRNumber
                                   };

                    if (!string.IsNullOrEmpty(sortOrder))
                    {
                        switch (sortField)
                        {
                            case "Chequeno":
                                if (sortOrder == "DESC")
                                    verified = verified.OrderByDescending(s => s.Chequeno);
                                else
                                    verified = verified.OrderBy(s => s.Chequeno);
                                break;
                            case "accno":
                                if (sortOrder == "DESC")
                                    verified = verified.OrderByDescending(s => s.accno);
                                else
                                    verified = verified.OrderBy(s => s.accno);
                                break;
                            case "IFSC":
                                if (sortOrder == "DESC")
                                    verified = verified.OrderByDescending(s => s.IFSC);
                                else
                                    verified = verified.OrderBy(s => s.IFSC);
                                break;
                            case "cname":
                                if (sortOrder == "DESC")
                                    verified = verified.OrderByDescending(s => s.cname);
                                else
                                    verified = verified.OrderBy(s => s.cname);
                                break;
                            case "accholderName":
                                if (sortOrder == "DESC")
                                    verified = verified.OrderByDescending(s => s.accholderName);
                                else
                                    verified = verified.OrderBy(s => s.accholderName);
                                break;
                            case "acctype":
                                if (sortOrder == "DESC")
                                    verified = verified.OrderByDescending(s => s.acctype);
                                else
                                    verified = verified.OrderBy(s => s.acctype);
                                break;
                            case "refundDate":
                                if (sortOrder == "DESC")
                                    verified = verified.OrderByDescending(s => s.refundDate);
                                else
                                    verified = verified.OrderBy(s => s.refundDate);
                                break;
                            case "Amount":
                                if (sortOrder == "DESC")
                                    verified = verified.OrderByDescending(s => s.Amount);
                                else
                                    verified = verified.OrderBy(s => s.Amount);
                                break;
                            case "reason":
                                if (sortOrder == "DESC")
                                    verified = verified.OrderByDescending(s => s.reason);
                                else
                                    verified = verified.OrderBy(s => s.reason);
                                break;
                            case "utrno":
                                if (sortOrder == "DESC")
                                    verified = verified.OrderByDescending(s => s.utrno);
                                else
                                    verified = verified.OrderBy(s => s.utrno);
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
                        if (refundmodefilter == 3)
                        {
                            gvMain.Columns[2].Visible = false;
                            gvMain.Columns[3].Visible = false;
                            gvMain.Columns[5].Visible = false;
                            gvMain.Columns[6].Visible = false;
                            gvMain.Columns[1].Visible = true;
                            gvMain.Columns[4].Visible = true;
                        }
                        else
                        {
                            gvMain.Columns[1].Visible = false;
                            gvMain.Columns[4].Visible = false;
                            gvMain.Columns[2].Visible = true;
                            gvMain.Columns[3].Visible = true;
                            gvMain.Columns[5].Visible = true;
                            gvMain.Columns[6].Visible = true;
                        }
                    }
                }
                else
                {
                    Response.Redirect("NEFTRefund.aspx", true);
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
                Label lb0 = (Label)e.Row.Cells[7].FindControl("lblTran");
                lb0.Text = Convert.ToDateTime(lb0.Text).ToString("dd-MMM-yyyy");
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
           
            if (ddlRefundMode.SelectedValue == "3")
            {
                //string strQuery = "select Cheque_Issuer_Name as Name , Refund_Amount,REPLACE(CONVERT(Varchar, Refund_Date, 106),' ','-')as Refund_Date,Cheque_Number,Refund_Reason, UTR_Number from NEFT_Refund  " +
                //               " where cast(Date as DATE) >='" + fromdate.ToString("dd-MMM-yyyy") + "'and cast(Date as DATE) <='" + todate.ToString("dd-MMM-yyyy") + "'";


                // November_2024
                    SqlParameter[] para1 = { new SqlParameter("@fromDate", fromdate.ToString("dd-MMM-yyyy")),
                                               new SqlParameter("@toDate", todate.ToString("dd-MMM-yyyy"))};
                string strQuery = "select Cheque_Issuer_Name as Name , Refund_Amount,REPLACE(CONVERT(Varchar, Refund_Date, 106),' ','-')as Refund_Date,Cheque_Number,Refund_Reason, UTR_Number from NEFT_Refund  " +
                               " where cast(Date as DATE) >=@fromDate and cast(Date as DATE) <=@toDate";

                string sheetname = "NEFT_RTGS_REFUND";
                strQuery += " and Refund_Mode = 3 ";
                //dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, null, CommandType.Text, true);
                // November_2024
                dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, para1, CommandType.Text, true);
                if (dt.Rows.Count > 0)
                {
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    GridView1.Caption = "NEFT/RTGS_REFUND_DETAILS_" + DateTime.Now.ToLongDateString() + "( Through Cheque ). ";
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
            else if (ddlRefundMode.SelectedValue == "6")
            {
                //string strQuery = "select Refund_Amount,REPLACE(CONVERT(Varchar, Refund_Date, 106),' ','-')as Refund_Date,Account_Number,Account_Holder_Name,Account_Type,IFSC_Code,Refund_Reason, UTR_Number from NEFT_Refund  " +
                //               " where cast(Date as DATE) >='" + fromdate.ToString("dd-MMM-yyyy") + "'and cast(Date as DATE) <='" + todate.ToString("dd-MMM-yyyy") + "'";

                // November_2024
                SqlParameter[] para2 = { new SqlParameter("@fromDate", fromdate.ToString("dd-MMM-yyyy")),
                                               new SqlParameter("@toDate", todate.ToString("dd-MMM-yyyy"))};
                string strQuery = "select Refund_Amount,REPLACE(CONVERT(Varchar, Refund_Date, 106),' ','-')as Refund_Date,Account_Number,Account_Holder_Name,Account_Type,IFSC_Code,Refund_Reason, UTR_Number from NEFT_Refund  " +
                              " where cast(Date as DATE) >=@fromDate and cast(Date as DATE) <=@toDate";

                string sheetname = "NEFT_RTGS_REFUND";
                strQuery += " and Refund_Mode = 6 ";
                //dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, null, CommandType.Text, true);
                // November_2024
                dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, para2, CommandType.Text, true);
                if (dt.Rows.Count > 0)
                {
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    GridView1.Caption = "NEFT/RTGS_REFUND_DETAILS_" + DateTime.Now.ToLongDateString() + "( Through NEFT ). "; ;
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
    protected void rblServiceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (rblServiceType.SelectedValue == "1")
            {
                divmultiple.Visible = false;
                divsingle.Visible = true;
            }
            else
            {
                divmultiple.Visible = true;
                divsingle.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            context = new EConnectContext();
            BreadCrumb1.Render();
            String utrnumber = txtutrnumber.Text.ToString();
            DateTime transactionDate = Convert.ToDateTime(txtdate.Text);
            Int64 creditamount = Convert.ToInt64(txtamount.Text);
            if (context.NEFTBankTransactions.Any(s => s.UTRNUMBER.ToUpper().Trim() == utrnumber.ToUpper().Trim() && System.Data.Entity.DbFunctions.TruncateTime(s.TransactionDate) == System.Data.Entity.DbFunctions.TruncateTime(transactionDate) && s.DemandNoteID == null && s.TransactionAmt == creditamount))
            {
                if (context.NEFTRefund.Where(s => s.NeftBankTransaction.UTRNUMBER.ToUpper().Trim() == utrnumber.ToUpper().Trim() && System.Data.Entity.DbFunctions.TruncateTime(s.NeftBankTransaction.TransactionDate) == System.Data.Entity.DbFunctions.TruncateTime(transactionDate) && s.NeftBankTransaction.DemandNoteID == null && s.NeftBankTransaction.TransactionAmt == creditamount).Count() == 0)
                {
                    divmoepayment.Visible = true;
                    btnSubmit.Visible = false;
                    btnSCancel.Visible = false;
                    txtutrnumber.Enabled = false;
                    txtdate.Enabled = false;
                    txtamount.Enabled = false;
                }
                else
                {
                    divmoepayment.Visible = false;
                    btnSubmit.Visible = true;
                    btnSCancel.Visible = true;
                    txtutrnumber.Enabled = true;
                    txtdate.Enabled = true;
                    txtamount.Enabled = true;
                    throw new Exception("No record found/ Already Refunded.");
                }
            }
            else
            {
                throw new Exception("Incorrect NEFT Details / Cannot be refunded because Already Verified.");
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally { context.Dispose(); }
    }
    protected void rblmodeofpayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            if (rblmodeofpayment.SelectedValue == "1")
            {
                divcheque.Visible = true;
                divneft.Visible = false;
            }
            else
            {
                divcheque.Visible = false;
                divneft.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void btnchequeSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            context = new EConnectContext();
            BreadCrumb1.Render();
            NEFTRefund refund = new NEFTRefund();
            String utrnumber = txtutrnumber.Text.ToString();
            DateTime transactionDate = Convert.ToDateTime(txtdate.Text);
            Int64 creditamount = Convert.ToInt64(txtamount.Text);
            if (context.NEFTRefund.Where(s => s.NeftBankTransaction.UTRNUMBER.ToUpper().Trim() == utrnumber.ToUpper().Trim() && System.Data.Entity.DbFunctions.TruncateTime(s.NeftBankTransaction.TransactionDate) == System.Data.Entity.DbFunctions.TruncateTime(transactionDate) && s.NeftBankTransaction.DemandNoteID == null && s.NeftBankTransaction.TransactionAmt == creditamount).Count() == 0)
            {
                //through cheque
                Int64 refunamount = Convert.ToInt64(txtcrefundamount.Text);
                String refundreason = txtcrefundreason.Text.ToString();
                DateTime refundDate = Convert.ToDateTime(txtcrefunddate.Text);
                Int64 chequenumber = Convert.ToInt64(txtcheqnumber.Text.ToString());
                String ownername = txtcname.Text.ToString();
                // Retriving NEFTBANKTransaction id
                Int32 neftbanktransactionid = context.NEFTBankTransactions.Where(s => s.UTRNUMBER.ToUpper().Trim() == utrnumber.ToUpper().Trim() && System.Data.Entity.DbFunctions.TruncateTime(s.TransactionDate) == System.Data.Entity.DbFunctions.TruncateTime(transactionDate) && s.DemandNoteID == null && s.TransactionAmt == creditamount).FirstOrDefault().ID;
                // SavinG Data
                refund.Date = DateTime.Now;
                refund.RefundDate = refundDate;
                refund.RefundAmount = refunamount;
                refund.NeftBankTransactionID = neftbanktransactionid;
                refund.ChequeNumber = chequenumber;
                refund.ChequeIssuerName = ownername.ToString();
                refund.RefundReason = refundreason;
                refund.UTRNumber = utrnumber;
                refund.RefundMode = Convert.ToInt32(enmPaymentMode.MultiCityCheque);
                refund.CreatedBy = Convert.ToInt32(Session["UserID"]);
                context.NEFTRefund.Add(refund);
                context.SaveChanges();
            }
            else
            {

            } 
         Response.Redirect("NEFTRefund.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally { context.Dispose(); }
    }
    protected void btnneftSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            context = new EConnectContext();
            NEFTRefund refund = new NEFTRefund();
            BreadCrumb1.Render();
            String utrnumber = txtutrnumber.Text.ToString();
            DateTime transactionDate = Convert.ToDateTime(txtdate.Text);
            Int64 creditamount = Convert.ToInt64(txtamount.Text);
            if (context.NEFTRefund.Where(s => s.NeftBankTransaction.UTRNUMBER.ToUpper().Trim() == utrnumber.ToUpper().Trim() && System.Data.Entity.DbFunctions.TruncateTime(s.NeftBankTransaction.TransactionDate) == System.Data.Entity.DbFunctions.TruncateTime(transactionDate) && s.NeftBankTransaction.DemandNoteID == null && s.NeftBankTransaction.TransactionAmt == creditamount).Count() == 0)
            {
                //through neft
                Int64 refunamount = Convert.ToInt64(txtnrefamount.Text);
                String refundreason = txtnrefundreason.Text.ToString();
                DateTime refundDate = Convert.ToDateTime(txtneftrefunddate.Text);
                String accnumber = txtnaccnumber.Text.ToString();
                String ownername = txtnaccholdername.Text.ToString();
                String acctype = txtnacctype.Text.ToString();
                String ifsccode = txtnifsccode.Text.ToString();
                // Retriving NEFTBANKTransaction id
                Int32 neftbanktransactionid = context.NEFTBankTransactions.Where(s => s.UTRNUMBER.ToUpper().Trim() == utrnumber.ToUpper().Trim() && System.Data.Entity.DbFunctions.TruncateTime(s.TransactionDate) == System.Data.Entity.DbFunctions.TruncateTime(transactionDate) && s.DemandNoteID == null && s.TransactionAmt == creditamount).FirstOrDefault().ID;
                refund.Date = DateTime.Now;
                refund.RefundDate = refundDate;
                refund.RefundAmount = refunamount;
                refund.NeftBankTransactionID = neftbanktransactionid;
                refund.AccountHolderName = ownername;
                refund.AccountNumber = accnumber;
                refund.AccountType = acctype;
                refund.IFSCCode = ifsccode;
                refund.RefundReason = refundreason;
                refund.RefundMode = Convert.ToInt32(enmPaymentMode.NEFTRTGS);
                refund.CreatedBy = Convert.ToInt32(Session["UserID"]);
                refund.UTRNumber = utrnumber;
                context.NEFTRefund.Add(refund);
                context.SaveChanges();
            }
            else
            {

            }
            Response.Redirect("NEFTRefund.aspx");
        }
        catch (Exception ex)
        { ShowAlert(ex.Message); }
        finally { context.Dispose(); }
    }
    protected void btnSCancel_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            rblServiceType.ClearSelection();
            divmultiple.Visible = false;
            divsingle.Visible = false;
            divmoepayment.Visible = false;
            Response.Redirect("NEFTRefund.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnchequeCancel_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            rblmodeofpayment.ClearSelection();
            divmoepayment.Visible = false;
            divmultiple.Visible = false;
            divsingle.Visible = true;
            divneft.Visible = false;
            divcheque.Visible = false;
            txtutrnumber.Enabled = true;
            txtdate.Enabled = true;
            txtamount.Enabled = true;
            txtutrnumber.Text = "";
            txtdate.Text = "";
            txtamount.Text = "";
            btnSubmit.Visible = true;
            btnSCancel.Visible = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnneftCancel_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            rblmodeofpayment.ClearSelection();
            divmoepayment.Visible = false;
            divmultiple.Visible = false;
            divsingle.Visible = true;
            divneft.Visible = false;
            divcheque.Visible = false;
            txtutrnumber.Enabled = true;
            txtdate.Enabled = true;
            txtamount.Enabled = true;
            txtutrnumber.Text = "";
            txtdate.Text = "";
            txtamount.Text = "";
            btnSubmit.Visible = true;
            btnSCancel.Visible = true;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}