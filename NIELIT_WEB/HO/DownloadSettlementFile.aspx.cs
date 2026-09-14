using System;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI;
using EConnect;
using EConnect.DAL;
using EConnect.Utils.Data;

public partial class DownloadSettlementFile : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;

    protected void Page_Load(object sender, EventArgs e)
    {
        
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            if (!Page.IsPostBack)
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Settlement File", "HO/DownloadSettlementFile.aspx", ""));
                 
                 
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("DownloadSettlementFile.aspx", true);
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            tblShow.Visible = true;
            showData();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void showData()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                DateTime FromDate = Convert.ToDateTime(txtDateFrom.Text);
                string responseStatus = "0300";
                Int32 paymentStatusPaid = Convert.ToInt32(enmPaymentStatus.Paid);
                Int32 paidButNotVerified = Convert.ToInt32(enmPaymentStatus.PaidButNotVerified);
                Int32 pending = Convert.ToInt32(enmPaymentStatus.Pending);
                var appcount = (from o in context.OnlineTransaction
                                where System.Data.Entity.DbFunctions.TruncateTime(o.ResponseDate) == System.Data.Entity.DbFunctions.TruncateTime(FromDate)
                                select o);
                var appcount1 = (from p in context.OnlineTransaction
                                 join d in context.DemandNotes
                                 on p.DemandNoteID equals d.ID
                                 where (p.ResponseStatusCode == responseStatus && p.ResponseStatusMessage != null
                                        &&  System.Data.Entity.DbFunctions.TruncateTime(p.ResponseDate) == System.Data.Entity.DbFunctions.TruncateTime(FromDate))
                                 select new { p, d });
                if (appcount.Count() > 0 && appcount1.Count() > 0 )
                {
                    lbltotal.Text = appcount.Count().ToString();
                    lblFailed.Text = appcount.Where(a => a.ResponseStatusCode == null && a.ResponseStatusMessage == null).Count().ToString();
                    lblSuccessfull.Text = appcount.Where(a => a.ResponseStatusCode == responseStatus && a.ResponseStatusMessage != null).Count().ToString();

                    lblPaymentRecvd.Text = appcount1.Where(s => (s.d.PaymentStatusID == paymentStatusPaid || 
                                           s.d.PaymentStatusID == paidButNotVerified) && s.d.OnlineTransactionID != null ).Select(t => t.p).Count().ToString();

                    lblPending.Text = appcount1.Where(s => s.d.PaymentStatusID == pending  && s.d.OnlineTransactionID == null
                                      ).Select(t => t.p).Count().ToString();

                    lblPaidNotVerified.Text = appcount1.Where(s => s.d.PaymentStatusID == paidButNotVerified && s.d.OnlineTransactionID != null
                                              ).Select(t => t.p).Count().ToString();

                    lblPaidVerified.Text = appcount1.Where(s => s.d.PaymentStatusID == paymentStatusPaid && s.d.OnlineTransactionID != null
                                              ).Select(t => t.p).Count().ToString();
                    btnCancel.Visible = true;
                }
                else
                {
                    lbltotal.Text = "0";
                    lblFailed.Text = "0";
                    lblSuccessfull.Text = "0";
                    lblPaymentRecvd.Text = "0";
                    lblPending.Text = "0";
                    lblPaidNotVerified.Text = "0";
                    lblPaidVerified.Text = "0";
                    trMsg.Visible = false ;
                    btnDownload.Visible = false ;
                    btnCancel.Visible = false ;
                }
                if (lblPaidNotVerified.Text != "0")
                {
                    trMsg.Visible = true;
                    btnDownload.Visible = true;
                    lblMsg.Text = "Note 1 : Settlement File for Applications having status \"Successfull transactions with Demand Note Paid and Not Verified/Not Reconciled\" can only be downloaded.";
                }
                else
                {
                    trMsg.Visible = true;
                    lblMsg.Text = "Note 1 : No transactions found.";
                }
                if (lblPending.Text != "0")
                {
                    trMsg.Visible = true;
                    btnRefund.Visible = true;
                    lblMsg.Text += "<br/>Note 2 : Refund File for Applications having successfull transactions but Demand Note not Paid can only be downloaded.";
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        OleDbConnection connection = new OleDbConnection();
        OleDbCommand command =new OleDbCommand() ;
        OleDbDataAdapter oda = new OleDbDataAdapter();
        String txtFilePath = "";
        FileStream stream=null;
        StreamWriter writer = null;
        String fileName = "";
        try
        {
            BreadCrumb1.Render();
            DateTime FromDate = Convert.ToDateTime(txtDateFrom.Text);
            string responseStatus = "0300";
            Int32 paymentStatusPaid = Convert.ToInt32(enmPaymentStatus.PaidButNotVerified);
            using (EConnectContext context = new EConnectContext())
            {
                fileName = "NIELIT_Settlement_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                txtFilePath = Server.MapPath("~/Download/" + fileName);
                if (System.IO.File.Exists(txtFilePath))
                    System.IO.File.Delete(txtFilePath);

                System.IO.File.Copy(Server.MapPath("~/Download/Settlement.txt"), txtFilePath);
                stream = new FileStream(txtFilePath, FileMode.Open, FileAccess.ReadWrite);
                writer = new StreamWriter(stream);

                //String sql = "select ol.ID as customer_id ,ol.Reference_Number as txn_id ," +
                //             " CAST(ol.Amount as decimal(10,2)) as txn_amount ,CONVERT(VARCHAR,ol.Response_Date, 112) AS txn_date " +
                //             " from Online_Transaction ol , Demand_Note d " +
                //             " where ol.Demand_Note_ID = d.ID and ol.Response_Status_Code = '" +
                //             responseStatus + "' and  d.Status_ID = " + paymentStatusPaid +
                //             " and ol.Response_Status_Message is not null  and  d.Online_Transaction_ID is not null " +
                //             " and REPLACE(CONVERT(VARCHAR,ol.Response_Date,106),' ','-') = '" + txtDateFrom.Text.Trim() + "'";

                //December_2024
                SqlParameter[] param1 = { new SqlParameter("@responseStatus", responseStatus),
                                            new SqlParameter("@paymentStatusPaid", paymentStatusPaid),
                                                new SqlParameter("@fromDate", txtDateFrom.Text.Trim())
                                                };
                String sql = " select ol.ID as customer_id ,ol.Reference_Number as txn_id ," +
                             " CAST(ol.Amount as decimal(10,2)) as txn_amount ,CONVERT(VARCHAR,ol.Response_Date, 112) AS txn_date " +
                             " from Online_Transaction ol , Demand_Note d " +
                             " where ol.Demand_Note_ID = d.ID and ol.Response_Status_Code = " +
                             " @responseStatus and  d.Status_ID = @paymentStatusPaid " + 
                             " and ol.Response_Status_Message is not null  and  d.Online_Transaction_ID is not null " +
                             " and REPLACE(CONVERT(VARCHAR,ol.Response_Date,106),' ','-') = @fromDate ";
                DataTable dtTbl = DbUtility.GetDataTable(sql, new EConnect.Connections.SqlCon(), param1, CommandType.Text, false);
                if (dtTbl.Rows.Count > 0)
                {
                    foreach (DataRow dtrow in dtTbl.Rows)
                    {
                        writer.Write(dtrow["txn_id"].ToString());
                        writer.Write(",");
                        writer.Write(dtrow["customer_id"].ToString());
                        writer.Write(",");
                        writer.Write(dtrow["txn_amount"].ToString());
                        writer.Write(",");
                        writer.Write(dtrow["txn_date"].ToString());
                        writer.WriteLine();
                    }

                    writer.Close();
                    stream.Close();
                    context.Dispose();
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();

                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.ContentType = "text/plain";
                    Response.Charset = "UTF-8";
                    Response.WriteFile(txtFilePath);
                    Response.End();
                }
                else
                {
                    ShowAlert("No applications found.",true);
                }
            };
        }
        catch (Exception ex)
        {
            writer.Close();
            stream.Close();
            context.Dispose();
            command.Dispose();
            connection.Dispose();
            connection.Close();
            //File.Delete(txtFilePath);
            ShowAlert(ex.Message, true);
        }
        finally
        {
            //File.Delete(txtFilePath);
        }
    }
    protected void btnRefund_Click(object sender, EventArgs e)
    {

        OleDbConnection connection = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();
        OleDbDataAdapter oda = new OleDbDataAdapter();
        String txtFilePath = "";
        FileStream stream = null;
        StreamWriter writer = null;
        String fileName = "";
        try
        {
            BreadCrumb1.Render();
            DateTime FromDate = Convert.ToDateTime(txtDateFrom.Text);
            string responseStatus = "0300";
            Int32 paymentStatusPending= Convert.ToInt32(enmPaymentStatus.Pending);

            using (EConnectContext context = new EConnectContext())
            {
                fileName = "NIELIT_Refund_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                txtFilePath = Server.MapPath("~/Download/" + fileName);
                if (System.IO.File.Exists(txtFilePath))
                    System.IO.File.Delete(txtFilePath);

                System.IO.File.Copy(Server.MapPath("~/Download/Settlement.txt"), txtFilePath);
                stream = new FileStream(txtFilePath, FileMode.Open, FileAccess.ReadWrite);
                writer = new StreamWriter(stream);

                //String sql = "select ol.ID as customer_id ,ol.Reference_Number as txn_id ," +
                //             " CAST(ol.Amount as decimal(10,2)) as txn_amount ,CONVERT(VARCHAR,ol.Response_Date, 112) AS txn_date , " +
                //             " CAST(ol.Amount as decimal(10,2)) as refund_amount from Online_Transaction ol , Demand_Note d " +
                //             " where ol.Demand_Note_ID = d.ID and ol.Response_Status_Code = '" +
                //             responseStatus + "' and  d.Status_ID = " + paymentStatusPending +
                //             " and ol.Response_Status_Message is not null  and  d.Online_Transaction_ID is  null " +
                //             " and REPLACE(CONVERT(VARCHAR,ol.Response_Date,106),' ','-') = '" + txtDateFrom.Text.Trim() + "'";

                //December_2024
                SqlParameter[] param2 = { new SqlParameter("@responseStatus", responseStatus),
                                            new SqlParameter("@paymentStatusPending", paymentStatusPending),
                                                new SqlParameter("@fromDate", txtDateFrom.Text.Trim())
                                                    };
                String sql = "select ol.ID as customer_id ,ol.Reference_Number as txn_id ," +
                            " CAST(ol.Amount as decimal(10,2)) as txn_amount ,CONVERT(VARCHAR,ol.Response_Date, 112) AS txn_date , " +
                            " CAST(ol.Amount as decimal(10,2)) as refund_amount from Online_Transaction ol , Demand_Note d " +
                            " where ol.Demand_Note_ID = d.ID and ol.Response_Status_Code = " +
                            " @responseStatus and  d.Status_ID = @paymentStatusPending " +
                            " and ol.Response_Status_Message is not null  and  d.Online_Transaction_ID is  null " +
                            " and REPLACE(CONVERT(VARCHAR,ol.Response_Date,106),' ','-') = @fromDate ";
                DataTable dtTbl = DbUtility.GetDataTable(sql, new EConnect.Connections.SqlCon(), param2, CommandType.Text, false);
                if (dtTbl.Rows.Count > 0)
                {
                    foreach (DataRow dtrow in dtTbl.Rows)
                    {
                        writer.Write(dtrow["txn_id"].ToString());
                        writer.Write(",");
                        writer.Write(dtrow["txn_date"].ToString());
                        writer.Write(",");
                        writer.Write(dtrow["customer_id"].ToString());
                        writer.Write(",");
                        writer.Write(dtrow["txn_amount"].ToString());
                        writer.Write(",");
                        writer.Write(dtrow["refund_amount"].ToString());
                        writer.WriteLine();
                    }

                    writer.Close();
                    stream.Close();
                    context.Dispose();
                    command.Dispose();
                    connection.Dispose();
                    connection.Close();

                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.ContentType = "text/plain";
                    Response.Charset = "UTF-8";
                    Response.WriteFile(txtFilePath);
                    Response.End();
                }
            };
        }
        catch (Exception ex)
        {
            writer.Close();
            stream.Close();
            context.Dispose();
            command.Dispose();
            connection.Dispose();
            connection.Close();
            //File.Delete(txtFilePath);
            ShowAlert(ex.Message, true);
        }
        finally
        {
            //File.Delete(txtFilePath);
        }
    }
}   