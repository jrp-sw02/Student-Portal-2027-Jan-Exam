using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.URM;
using RestSharp;

public partial class HO_FinalRefund : BasePage
{
    Int32 currentRoleId = 0;
    StringBuilder appIdList = new StringBuilder();
    StringBuilder appIdList1 = new StringBuilder();
    StringBuilder appIdList2 = new StringBuilder();

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
                    lblError.Text = "Please Select Filter Criteria to View Online Refund Records";
                    lblError.Visible = true;
                }
                bindpaymentmode();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Upload Refund File", "", ""));
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
            divValidateData.Visible = true;
            string[] arr = new string[10];
            appIdList.Clear();
            appIdList1.Clear();
            appIdList2.Clear();
            lblFailedRecords.Text = "";
            List<string> refernce3 = new List<string>();

            //Previous code
            //refernce3.Add("REGN01");
            //refernce3.Add("EXAM01");
            arr[0] = "Incorrect Data ";
            arr[1] = "No record found";
            arr[2] = "Data Already Updated";
            int NotValidateRecords = 0;
            int ValidateRecords = 0;
            int TotalRecords = 0;
            // *** Code For Creating Refund File ****//
            btnSave.Visible = true;
            BreadCrumb1.Render();
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
                ShowAlert("Please Choose ..XLS/.XLSX Excel File.", true);
                return;
            }
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = sExcelConnectionString;
            connection.Open();
            OleDbCommand command = new OleDbCommand("select * from [RefundTransaction$]", connection);
            OleDbDataReader dr = command.ExecuteReader();
            Int32 transid = 0;
            try
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 paymodeid = Convert.ToInt32(ddlpaymentmode.SelectedValue);
                    enmPaymentMode paymentMode = (enmPaymentMode)paymodeid;

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
                            if (CommonFunctions.IsNumeric(dr[6].ToString()))
                            {
                                 if (refernce3.Contains(dr[8].ToString()))
                                 {
                                      transid = Convert.ToInt32(dr[6]);
                                 }
                                 else
                                 {
                                     NotValidateRecords = NotValidateRecords + 1;
                                     appIdList.Append(dr[6].ToString() + ",");
                                     if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                        appIdList.Append(WebUtility.HtmlDecode("<br/>"));
                                    continue;
                                 }
                            }
                            else
                            {
                                NotValidateRecords = NotValidateRecords + 1;
                                appIdList.Append(dr[6].ToString() + ",");
                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                appIdList.Append(WebUtility.HtmlDecode("<br/>"));
                                 continue;
                            }
                            string[] BillDeskDate = dr[13].ToString().Substring(0, 10).Split('/');
                            DateTime BillDeskRefundDate = new DateTime(Convert.ToInt32(BillDeskDate[2]), Convert.ToInt32(BillDeskDate[1]), Convert.ToInt32(BillDeskDate[0]));
                            if(context.OnlineRefunds.Where(s => s.TransactionID == transid).Count() > 0)
                            {
                                var onrefund = context.OnlineRefunds.Where(s => s.TransactionID == transid).FirstOrDefault();
                                if(onrefund.BillDeskRefundDate.HasValue == false)
                                {
                                    onrefund.BillDeskRefundDate = BillDeskRefundDate;
                                    onrefund.BillDeskRefundID  = Convert.ToInt64(dr[12]);
                                    context.Entry(onrefund).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                    ValidateRecords = ValidateRecords + 1;
                                }
                                else
                                {
                                    NotValidateRecords = NotValidateRecords + 1;
                                    appIdList2.Append(dr[6].ToString() + ",");
                                    if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                    appIdList2.Append(WebUtility.HtmlDecode("<br/>"));
                                }
                            }
                            else
                            {
                                 NotValidateRecords = NotValidateRecords + 1;
                                appIdList1.Append(dr[6].ToString() + ",");
                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                appIdList1.Append(WebUtility.HtmlDecode("<br/>"));
                            }
                        }
                        context.SaveChanges();
                        context.Dispose();
                        command.Dispose();
                        connection.Dispose();
                        connection.Close();
                };
                lblTotalRecords.Text = TotalRecords.ToString();
                lblValidateRecords.Text = ValidateRecords.ToString();
                lblNotValidate.Text = NotValidateRecords.ToString();
                if (appIdList.Length > 0)
                    lblFailedRecords.Text = "<b>" + arr[0].ToString() + " For Following Transaction-ID:- </b><br/>" + appIdList.ToString().TrimEnd(',').ToString();
                if (appIdList1.Length > 0)
                {
                    if (lblFailedRecords.Text.Length > 0)
                        lblFailedRecords.Text += "<br/>";
                    lblFailedRecords.Text += "<b>" + arr[1].ToString() + " For Following Transaction-ID:- </b><br/>" + appIdList1.ToString().TrimEnd(',').ToString();
                }
                if (appIdList2.Length > 0)
                {
                    if (lblFailedRecords.Text.Length > 0)
                        lblFailedRecords.Text += "<br/>";
                    lblFailedRecords.Text += "<b>" + arr[2].ToString() + " For Following Transaction-ID:- </b><br/>" + appIdList2.ToString().TrimEnd(',').ToString();
                }
            }
            catch (Exception ex)
            {
                command.Dispose();
                connection.Dispose();
                connection.Close();
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
                Int32 online = Convert.ToInt32(enmPaymentMode.Online);
                ListItem lst = new ListItem("--Select One--", "0");
                var Category = from p in context.PaymentModes
                               where p.ID == online
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
                Response.Redirect("FinalRefund.aspx");
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
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        appIdList.Clear();
        appIdList1.Clear();
        appIdList2.Clear();
        lblFailedRecords.Text = "";
        txtflFromDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        Response.Redirect("FinalRefund.aspx");
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
                Int32 datetype = Convert.ToInt32(ddldatetype.SelectedValue);
                if (!string.IsNullOrEmpty(fromdate.ToString()) && !string.IsNullOrEmpty(todate.ToString()) && datetype != 0)
                {

                        Int32[] onlinetransactionid = context.Online_ChargeBackTransactions.Select(t => t.Ref1).ToArray();
                        var verified = from nbt in context.OnlineRefunds 
                                       where !onlinetransactionid.Contains(nbt.TransactionID)
                                       select new
                                       {
                                           ID = nbt.ID,
                                           bdreferenceNo = nbt.Txt_Ref_No,
                                           transid = nbt.TransactionID,
                                           refundid = nbt.BillDeskRefundID.HasValue ? nbt.BillDeskRefundID : null,
                                           refundDate = nbt.RefundDate,
                                           Amount = nbt.Refund_Amount / 100 ,
                                           createdon = nbt.TransactionDate ,
                                           billdeskdate = nbt.BillDeskRefundDate.HasValue ? nbt.BillDeskRefundDate : null,
                                           dddetails =   nbt.onlineTransaction.DemandNoteID
                                       };
                        if (datetype == 1)
                        {
                            verified = verified.Where(s => System.Data.Entity.DbFunctions.TruncateTime(s.refundDate) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate)
                                                      && System.Data.Entity.DbFunctions.TruncateTime(s.refundDate) <= System.Data.Entity.DbFunctions.TruncateTime(todate));
                        }
                        else if (datetype == 2)
                        {
                            verified = verified.Where(s => System.Data.Entity.DbFunctions.TruncateTime(s.billdeskdate) >= System.Data.Entity.DbFunctions.TruncateTime(fromdate)
                                                      && System.Data.Entity.DbFunctions.TruncateTime(s.billdeskdate) <= System.Data.Entity.DbFunctions.TruncateTime(todate));
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
                                case "transid":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.transid);
                                    else
                                        verified = verified.OrderBy(s => s.transid);
                                    break;
                                case "refundid":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.refundid);
                                    else
                                        verified = verified.OrderBy(s => s.refundid);
                                    break;
                                case "dddetails":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.dddetails);
                                    else
                                        verified = verified.OrderBy(s => s.dddetails);
                                    break;
                                case "refundDate":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.refundDate);
                                    else
                                        verified = verified.OrderBy(s => s.refundDate);
                                    break;
                                case "billdeskdate":
                                    if (sortOrder == "DESC")
                                        verified = verified.OrderByDescending(s => s.billdeskdate);
                                    else
                                        verified = verified.OrderBy(s => s.billdeskdate);
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
                else
                {
                    Response.Redirect("FinalRefund.aspx", true);
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
                Label lb0 = (Label)e.Row.Cells[2].FindControl("lbltrans");
                lb0.Text = Convert.ToDateTime(lb0.Text).ToString("dd-MMM-yyyy");
                Label lb1 = (Label)e.Row.Cells[5].FindControl("lblnrefund");
                lb1.Text = Convert.ToDateTime(lb1.Text).ToString("dd-MMM-yyyy");
                Label lb2 = (Label)e.Row.Cells[7].FindControl("lblbrefund");
                lb2.Text = Convert.ToDateTime(lb2.Text).ToString("dd-MMM-yyyy");
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
            Int32 datetype = Convert.ToInt32(ddldatetype.SelectedValue);
            string strQuery = String.Empty;

                //December_2024
                SqlParameter[] param1 = { new SqlParameter("@fromDate", fromdate.ToString("dd-MMM-yyyy")),
                                            new SqlParameter("@toDate", todate.ToString("dd-MMM-yyyy"))
                                                };
                if (datetype == 1)
                {
                    //strQuery = "select nbt.Transation_ID, REPLACE(CONVERT(Varchar, nbt.Transaction_Date, 106),' ','-') as Transaction_Date, nbt.Txt_Ref_No, nbt.Product_ID,REPLACE(CONVERT(Varchar, nbt.Refund_Date, 106),' ','-') as Refund_Date, nbt.BillDesk_Refund_ID, REPLACE(CONVERT(Varchar, nbt.BillDesk_Refund_Date, 106),' ','-') as BillDesk_Refund_Date , nbt.Refund_Amount / 100 as Refund_Amount , t.Demand_Note_ID as Demand_Note_ID from OnlineRefund nbt , Online_Transaction t    " +
                    //            " where nbt.Transation_ID = t.ID and cast(nbt.Refund_Date as DATE) >='" + fromdate.ToString("dd-MMM-yyyy") + "'and cast(nbt.Refund_Date as DATE) <='" + todate.ToString("dd-MMM-yyyy") + "' and nbt.Transation_ID not in ( select Ref_1 from Online_Charge_Back_Transaction)";
                    
                    //December_2024
                    strQuery = " select nbt.Transation_ID, REPLACE(CONVERT(Varchar, nbt.Transaction_Date, 106),' ','-') as Transaction_Date, nbt.Txt_Ref_No, nbt.Product_ID,REPLACE(CONVERT(Varchar, nbt.Refund_Date, 106),' ','-') as Refund_Date, nbt.BillDesk_Refund_ID, REPLACE(CONVERT(Varchar, nbt.BillDesk_Refund_Date, 106),' ','-') as BillDesk_Refund_Date , nbt.Refund_Amount / 100 as Refund_Amount , t.Demand_Note_ID as Demand_Note_ID from OnlineRefund nbt , Online_Transaction t    " +
                                " where nbt.Transation_ID = t.ID and cast(nbt.Refund_Date as DATE) >=@fromDate and cast(nbt.Refund_Date as DATE) <=@toDate and nbt.Transation_ID not in ( select Ref_1 from Online_Charge_Back_Transaction) ";
                }
                else if (datetype == 2)
                {
                    //strQuery = "select nbt.Transation_ID, REPLACE(CONVERT(Varchar, nbt.Transaction_Date, 106),' ','-') as Transaction_Date, nbt.Txt_Ref_No, nbt.Product_ID, REPLACE(CONVERT(Varchar, nbt.Refund_Date, 106),' ','-') as Refund_Date, nbt.BillDesk_Refund_ID, REPLACE(CONVERT(Varchar, nbt.BillDesk_Refund_Date, 106),' ','-')as BillDesk_Refund_Date, nbt.Refund_Amount / 100 as Refund_Amount , t.Demand_Note_ID as Demand_Note_ID from OnlineRefund nbt , Online_Transaction t   " +
                    //           " where nbt.Transation_ID = t.ID and cast(nbt.BillDesk_Refund_Date as DATE) >='" + fromdate.ToString("dd-MMM-yyyy") + "'and cast(nbt.BillDesk_Refund_Date as DATE) <='" + todate.ToString("dd-MMM-yyyy") + "' and nbt.Transation_ID not in ( select Ref_1 from Online_Charge_Back_Transaction)";

                    //December_2024
                    strQuery = " select nbt.Transation_ID, REPLACE(CONVERT(Varchar, nbt.Transaction_Date, 106),' ','-') as Transaction_Date, nbt.Txt_Ref_No, nbt.Product_ID, REPLACE(CONVERT(Varchar, nbt.Refund_Date, 106),' ','-') as Refund_Date, nbt.BillDesk_Refund_ID, REPLACE(CONVERT(Varchar, nbt.BillDesk_Refund_Date, 106),' ','-')as BillDesk_Refund_Date, nbt.Refund_Amount / 100 as Refund_Amount , t.Demand_Note_ID as Demand_Note_ID from OnlineRefund nbt , Online_Transaction t   " +
                                " where nbt.Transation_ID = t.ID and cast(nbt.BillDesk_Refund_Date as DATE) >=@fromDate and cast(nbt.BillDesk_Refund_Date as DATE) <=@toDate and nbt.Transation_ID not in ( select Ref_1 from Online_Charge_Back_Transaction) ";
                }
                string sheetname =  "Online_Refund_Transaction_Details";
                dt = EConnect.Utils.Data.DbUtility.GetDataTable(strQuery, con, param1, CommandType.Text, true);
                if (dt.Rows.Count > 0)
                {
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    GridView1.CaptionAlign = TableCaptionAlign.Left;
                    GridView1.Caption = "<b>" + ddldatetype.SelectedItem.Text + " From : " + fromdate.ToString("dd-MMM-yyyy") + " to " + todate.ToString("dd-MMM-yyyy") + HttpUtility.HtmlDecode("<br/>") + " Date Type :-  " + ddldatetype.SelectedItem.Text;
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
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
            con.Close();
        }
    }
}