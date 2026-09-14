using System;
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

public partial class HO_DownloadExceptionFile : BasePage
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
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Refund File", "", ""));
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
            StringBuilder appIdList = new StringBuilder();
            StringBuilder appIdList1 = new StringBuilder();
            StringBuilder appIdList2 = new StringBuilder();
            arr[0] = "Data has been already refunded.";
            arr[1] = "No record found";
            arr[2] = "Charge Back / Not Applicable for Refund ";
            String txtFilePath = "";
            int NotValidateRecords = 0; 
            int ValidateRecords = 0;
            int TotalRecords = 0;
            // *** Code For Creating Refund File ****//
            FileStream stream = null;
            StreamWriter writer = null;
            String RefundfileName = "";
            RefundfileName = "NIELIT_Refund_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            txtFilePath = Server.MapPath("~/Download/" + RefundfileName);
            if (System.IO.File.Exists(txtFilePath))
                System.IO.File.Delete(txtFilePath);
            System.IO.File.Copy(Server.MapPath("~/Download/Settlement.txt"), txtFilePath);
            stream = new FileStream(txtFilePath, FileMode.Open, FileAccess.ReadWrite);
            writer = new StreamWriter(stream);
            btnSave.Visible = true;
            BreadCrumb1.Render();
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
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = sExcelConnectionString;
            connection.Open();
            OleDbCommand command = new OleDbCommand("select * from [Pending Settlemenmt Transaction$]", connection);
            OleDbDataReader dr = command.ExecuteReader();
            Int32 transid = 0;
            try
            {
                    using (EConnectContext context = new EConnectContext())
                    {
                        Int32 paymodeid = Convert.ToInt32(ddlpaymentmode.SelectedValue);
                        enmPaymentMode paymentMode = (enmPaymentMode)paymodeid;
                        OnlineRefund refund;
                        while (dr.Read())
                        {
                            if (paymentMode == enmPaymentMode.Online)
                            {
                                if (CommonFunctions.IsNumeric(dr[4].ToString()))
                                {
                                    transid = Convert.ToInt32(dr[4]);
                                    TotalRecords = TotalRecords + 1;
                                }
                                else
                                {
                                    continue;
                                }
                                var online = (from r in context.OnlineTransaction
                                              join d in context.DemandNotes
                                              on r.DemandNoteID equals d.ID
                                              where r.ID == transid
                                              select new
                                              {
                                                  responseCode = r.ResponseStatusCode,
                                                  demannotePaymentstatus = d.PaymentStatusID,
                                                  ID = r.ID,
                                                  Amount = r.Amount
                                              }).FirstOrDefault();
                                if (online != null)
                                {
                                    string[] date = dr[8].ToString().Substring(0, 10).Split('/');
                                    DateTime date1 = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[1]), Convert.ToInt32(date[0]));
                                    Boolean makeRefund = false;
                                    if (online.responseCode != "0300" || online.responseCode == null)
                                    {
                                        makeRefund = true;
                                    }
                                    else if ((online.responseCode == "0300") && (online.demannotePaymentstatus == Convert.ToInt32(enmPaymentStatus.Pending)))
                                    {
                                        makeRefund = true;
                                    }
                                    if (makeRefund)
                                    {
                                        if (context.Online_ChargeBackTransactions.Where(a => a.Ref1 == online.ID).Count() > 0)
                                        {
                                            makeRefund = false;
                                        }
                                    }
                                    if (makeRefund)
                                    {
                                        refund = new OnlineRefund();
                                        Boolean getRefund = false;
                                        var refunRecord = context.OnlineRefunds.Where(s => s.TransactionID == online.ID);
                                        if (refunRecord == null || refunRecord.Count() == 0)
                                        {
                                            refund.BankID = Convert.ToString(dr[2]);
                                            refund.Bank_Ref_No = Convert.ToString(dr[5]);
                                            refund.ProductID = Convert.ToString(dr[6]);
                                            refund.RefundDate = DateTime.Now;
                                            refund.TransactionDate = date1;
                                            refund.TransactionAmount = Convert.ToInt64(online.Amount * 100);
                                            refund.TransactionID = online.ID;
                                            refund.Txt_Ref_No = Convert.ToString(dr[3]);
                                            //Double refundamount = Convert.ToDouble(dr[7].ToString());
                                            refund.Refund_Amount = Convert.ToInt64(online.Amount * 100);
                                            context.OnlineRefunds.Add(refund);
                                            context.SaveChanges();

                                            getRefund = true;
                                        }
                                        else
                                        {
                                            if (refunRecord.FirstOrDefault().BillDeskRefundDate.HasValue)
                                            {
                                                NotValidateRecords = NotValidateRecords + 1;
                                                appIdList.Append(transid.ToString() + ",");
                                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                                    appIdList.Append(WebUtility.HtmlDecode("<br/>"));
                                            }
                                            else
                                            {
                                                getRefund = true;
                                                refund = refunRecord.FirstOrDefault();
                                            }
                                        }
                                        if (getRefund)
                                        {
                                            //download of refund file
                                            ValidateRecords = ValidateRecords + 1;

                                            writer.Write(dr[3].ToString());
                                            writer.Write(",");
                                            string year = date1.Year.ToString();
                                            string month = date1.Month.ToString();
                                            string day = date1.Day.ToString();
                                            string finaldate = year + month + day;
                                            writer.Write(finaldate);
                                            writer.Write(",");
                                            writer.Write(dr[4].ToString());
                                            writer.Write(",");
                                            writer.Write(refund.TransactionAmount.ToString());
                                            writer.Write(",");
                                            writer.Write(refund.Refund_Amount.ToString());
                                            writer.WriteLine();
                                       }
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
                                appIdList1.Append(transid.ToString() + ",");
                                if (NotValidateRecords % 10 == 0 && NotValidateRecords > 0)
                                    appIdList1.Append(WebUtility.HtmlDecode("<br/>"));
                            }
                         }
                        else if (paymentMode == enmPaymentMode.CSCSPV)// will be implemented later
                        {
                            if (CommonFunctions.IsNumeric(dr[9].ToString()))//need to change it
                            {
                                transid = Convert.ToInt32(dr[9]);
                            }
                            else
                            {
                                continue;
                            }
                            var csc = (from r in context.CSCTransactions
                                        join d in context.DemandNotes
                                            on r.DemandNoteID equals d.ID
                                        where r.ID == transid
                                        select new
                                        {
                                            responseCode = r.ResponseStatus.Value,
                                            demannotePaymentstatus = d.PaymentStatusID,
                                            ID = r.ID,
                                            Amount = r.Amount
                                        }).FirstOrDefault();
                            if (csc != null)
                            {
                                if (csc.responseCode != 0 || csc.responseCode == null || (csc.responseCode != 100 ))
                                {




                                }
                                else if ((csc.responseCode == 0 || (csc.responseCode == 100 ) && (csc.demannotePaymentstatus == Convert.ToInt32(enmPaymentStatus.Pending))))
                                {

                                }
                            }
                            else
                            {
                                    //IsTransIdMatch = false;
                                    //FaildRecords += transid.ToString();
                                    //FaildRecords += ",";
                                    //failedRecordCount++;
                            }
                          }
                        }
                        
                        writer.Close();
                        stream.Close();
                        context.Dispose();
                        command.Dispose();
                        connection.Dispose();
                        connection.Close();
                        if (ValidateRecords != 0)
                        {
                            Response.AddHeader("content-disposition", "attachment;filename=" + RefundfileName);
                            Response.ContentType = "text/plain";
                            Response.Charset = "UTF-8";
                            Response.WriteFile(txtFilePath);
                            Response.End();
                        }

                    };
                    lblTotalRecords.Text = TotalRecords.ToString();
                    lblValidateRecords.Text = ValidateRecords.ToString() +  " ( Excluding ChargeBack Transactions ) ";
                    lblNotValidate.Text = NotValidateRecords.ToString();
                    if (appIdList.Length > 0)
                        lblFailedRecords.Text = "<b>" + arr[0].ToString() + " For Transaction-ID:- </b><br/>" + appIdList.ToString().TrimEnd(',').ToString();
                    if (appIdList1.Length > 0)
                    {
                        if (lblFailedRecords.Text.Length > 0)
                             lblFailedRecords.Text += "<br/>";
                        lblFailedRecords.Text += "<b>" + arr[1].ToString() + " For Transaction-ID:- </b><br/>" + appIdList1.ToString().TrimEnd(',').ToString();
                    }
                    if (appIdList2.Length > 0)
                    {
                        if (lblFailedRecords.Text.Length > 0)
                            lblFailedRecords.Text += "<br/>";
                        lblFailedRecords.Text += "<b>" + arr[2].ToString() + " For Transaction-ID:- </b><br/>" + appIdList2.ToString().TrimEnd(',').ToString();
                    }
            }
            catch (Exception ex)
            {
                writer.Close();
                stream.Close();
                command.Dispose();
                connection.Dispose();
                connection.Close();
                //File.Delete(txtFilePath);
                ShowAlert(ex.Message, true);
            }
            finally
            {
                dr.Close();
                dr.Dispose();
                writer.Close();
                stream.Close();
                command.Dispose();
                connection.Close();
                connection.Dispose();
                System.IO.File.Delete(path);
                //File.Delete(txtFilePath);
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
                               where p.ID != multicheque && p.ID != cash && p.ID != DemandDraft && p.ID!=NEFTRTGS
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
    protected void btnValidate_Click(object sender, EventArgs e)
    {
        try
        {
            divValidateData.Visible = true;
            btnSave.Visible = true;
            BreadCrumb1.Render();
            btnValidate.Visible = false;
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
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = sExcelConnectionString;
            connection.Open();
            OleDbCommand command = new OleDbCommand("select * from [Pending Settlemenmt Transaction$]", connection);
            OleDbDataReader dr = command.ExecuteReader();
            int TotalRecords = 0;
            int ValidateRecords = 0;
            int NotValidateRecords = 0;
            Int32 transid = 0;
            int failedRecordCount1 = 0;
            string FaildRecords1 = "No record found with TransactionID's:-";
            try
            {
                using (TransactionScope scope = new TransactionScope())
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
                                    TotalRecords = TotalRecords + 1;
                                    transid = Convert.ToInt32(dr[4]);
                                }
                                else
                                {
                                    continue;
                                }
                                var online = (from r in context.OnlineTransaction
                                              join d in context.DemandNotes
                                              on r.DemandNoteID equals d.ID
                                              where r.ID == transid
                                              select new
                                              {
                                                  responseCode = r.ResponseStatusCode,
                                                  demannotePaymentstatus = d.PaymentStatusID,
                                              }).FirstOrDefault();
                                if (online != null)
                                {
                                    if (online.responseCode != "0300" || online.responseCode == null || ((online.responseCode == "0300") && (online.demannotePaymentstatus == Convert.ToInt32(enmPaymentStatus.Pending))))
                                    {
                                        ValidateRecords = ValidateRecords + 1;
                                    }
                                    else
                                    {
                                        NotValidateRecords = NotValidateRecords + 1;
                                    }
                                }
                                else
                                {
                                    FaildRecords1 += transid.ToString();
                                    FaildRecords1 += ",";
                                    failedRecordCount1++;
                                }
                            }
                            else if (paymentMode == enmPaymentMode.CSCSPV)
                            {
                                if (CommonFunctions.IsNumeric(dr[7].ToString()))//need to change it...
                                {
                                    TotalRecords = TotalRecords + 1;
                                    transid = Convert.ToInt32(dr[5]);////need to change it...
                                }
                                else
                                {
                                    continue;
                                }
                                var csc = (from r in context.CSCTransactions
                                           join d in context.DemandNotes
                                               on r.DemandNoteID equals d.ID
                                           where r.ID == transid
                                           select new
                                           {
                                               responseCode = r.ResponseStatus.Value,
                                               demannotePaymentstatus = d.PaymentStatusID,
                                           }).FirstOrDefault();
                                if (csc != null)
                                {

                                    if (csc.responseCode == 1 || csc.responseCode == null)
                                    {
                                        ValidateRecords = ValidateRecords + 1;
                                    }
                                    else
                                    {
                                        NotValidateRecords = NotValidateRecords + 1;
                                    }
                                }
                                else
                                {
                                    FaildRecords1 += transid.ToString();
                                    FaildRecords1 += ",";
                                    failedRecordCount1++;
                                }
                            }
                        }
                        scope.Complete();
                    };
                }
                lblTotalRecords.Text = TotalRecords.ToString();
                lblValidateRecords.Text = ValidateRecords.ToString();
                lblNotValidate.Text = NotValidateRecords.ToString();
                lblFailedRecords.Text = failedRecordCount1 != 0 ? FaildRecords1.TrimEnd(',').ToString() : "";
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
}