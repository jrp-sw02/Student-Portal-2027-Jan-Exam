using System;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.UI;
using EConnect;
using EConnect.DAL;
using EConnect.Utils.Data;
using System.Data.SqlClient;
using System.Configuration;
using EConnect.NIELIT;
using EConnect.URM;
using System.Collections.Generic;
using System.Web;
using System.Transactions;
using EConnect.Utils.Common;
using System.Drawing;
using ClosedXML.Excel;
using System.Text;
using System.Net;

public partial class DownloadPurskarCandListForPlacementAmtReleasedToBank : BasePage
{
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    int DownloadS = 0;  
    protected Int64 totAmountReleased = 0;
    System.Web.UI.WebControls.Table tbl = new System.Web.UI.WebControls.Table();
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
            if (!Page.IsPostBack)
            {
                BindGridView();

                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Purskar Candidates List to be sent to Bank for Amount Release", "HO/DownloadPurskarCandListForPlacementAmtReleasedToBank.aspx", ""));
                 
                 
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("DownloadPurskarCandListForPlacementAmtReleasedToBank.aspx", true);
    }
    protected void BindGridView()
    {
        try
        {
            using (System.Data.DataTable dt = FillGridViewOnlinePuraskarAppPlacmentAmtReleased
                ())
            {
                if (dt.Rows.Count > 0)
                {   
                    var ApplicationData = (from p in dt.AsEnumerable()
                                           select new
                                           {
                                               //ID = p.Field<Int64>("ID"),
                                               sl = p.Field<Int64>("sl"),
                                               onlinerefno=p.Field <string>("onlinerefno"),
                                               Regno = p.Field<Int64>("Regno"),
                                               Name = p.Field<string>("Name"),
                                               Level = p.Field<string>("Level"),
                                               Examid = p.Field<Int64>("ExamID"),
                                               Exams = p.Field<string>("ExamName"),
                                               FatherName = p.Field<string>("FatherName"),                                               
                                              AadharNumber=p.Field <string>("AadharNumber"),
                                               TotalAmtAsPerPaperPassed = p.Field<Int64>("TotalAmtAsPerPaperPassed").ToString() + "/-",
                                               PreviousAmountReleased = p.Field<Int64>("PreviousAmountReleased").ToString() + "/-",
                                               AmountToBeReleased = p.Field<Int64>("RemainsAmtToBeReleased").ToString() + "/-",                                               
                                           });
                    
                    PagingBar1.Bind(ApplicationData, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    lblError.Visible = false;                
                    btnDownloadWithLockCell.Visible = true;
                    btnPdfDownload.Visible = true;
                    if (gvMain.Rows.Count <= 0)
                    {
                        lblError.Text = "No record found.";
                        lblError.Visible = true;
                        btnDownload.Visible = false;
                        btnDownloadWithLockCell.Visible = false;
                        btnPdfDownload.Visible = false;
                    }
                   
                }
                else
                {
                    lblError.Text = "No record found.";
                    lblError.Visible = true;
                    btnDownload.Visible = false;
                    btnDownloadWithLockCell.Visible = false;
                    btnPdfDownload.Visible = false;
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvMain_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
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

    protected void gvMain_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            { 
                System.Web.UI.WebControls.Label l1 = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblaadhaar");
                System.Web.UI.WebControls.Label l2 = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblad");
                string vAadhaar = "";
                if(l2!=null)
                     vAadhaar = EncryptDecrypt.DecryptString(l2.Text);
                if(l1!=null)
                    l1.Text = vAadhaar;
            }
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
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    public System.Data.DataTable FillGridViewOnlinePuraskarAppPlacmentAmtReleased()
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        System.Data.DataTable myDt = new System.Data.DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetRecordForProtsahanDocsUploadedAmountReleased", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CodeForViewRecord", SqlDbType.Int).Value = 1;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
    
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        try
        {

            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
            divReportData.Visible = true;
         
            divReportData.Controls.Add(tbl);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=PurskarCandListForAmountReleased.xls");
            Response.Charset = "";
           
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            htmlWrite = new Html32TextWriter(StringWrite);
            htmlWrite.AddAttribute("border", "1");
            divReportData.RenderControl(htmlWrite);

            Response.Write(StringWrite.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
   
    protected void getDataForPDF()
    {
        try
        {
            ShowTableHeaderForPDF();

            Int64 totAmountReleased = 0, PtotAmountReleased = 0, RtotAmountReleased = 0;
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);          
            SqlCommand scCommand = new SqlCommand("GetRecordForProtsahanDocsUploadedAmountReleased", con);
            scCommand.CommandType = CommandType.StoredProcedure;
            scCommand.Parameters.Add("@CodeForViewRecord", SqlDbType.Int).Value = 3;           
            scCommand.CommandTimeout = 50000;
            if (scCommand.Connection.State == ConnectionState.Closed)
            {
                scCommand.Connection.Open();
            }
            SqlDataAdapter da = new SqlDataAdapter(scCommand);
            DataSet ds = new DataSet();
            da.Fill(ds);           
            if (ds.Tables[0].Rows.Count > 0)
            {
                int i;
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {    
                    System.Web.UI.WebControls.TableRow tr = new System.Web.UI.WebControls.TableRow();
                    if (i % 2 == 0)
                        tr.CssClass = "gdalternate1";
                    else
                        tr.CssClass = "gdrow1";

                    System.Web.UI.WebControls.TableCell tdRow = new System.Web.UI.WebControls.TableCell();
                    tdRow.Width = System.Web.UI.WebControls.Unit.Percentage(1);
                    tdRow.Text = (i + 1).ToString();
                    tdRow.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tr.Cells.Add(tdRow);

                    System.Web.UI.WebControls.TableCell tdRow4 = new System.Web.UI.WebControls.TableCell();
                    tdRow4.Width = System.Web.UI.WebControls.Unit.Percentage(5);
                    tdRow4.Text = ds.Tables[0].Rows[i]["Regno"].ToString();
                    tdRow4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    tr.Cells.Add(tdRow4);

                    System.Web.UI.WebControls.TableCell tdRow2 = new System.Web.UI.WebControls.TableCell();
                    tdRow2.Width = System.Web.UI.WebControls.Unit.Percentage(40);
                    tdRow2.Text = ds.Tables[0].Rows[i]["Name"].ToString();
                    tdRow2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    tr.Cells.Add(tdRow2);

                    System.Web.UI.WebControls.TableCell tdRow2e = new System.Web.UI.WebControls.TableCell();
                    tdRow2e.Width = System.Web.UI.WebControls.Unit.Percentage(3);
                    tdRow2e.Text =EncryptDecrypt.DecryptString( ds.Tables[0].Rows[i]["AadharNumber"].ToString()).ToString();
                    tdRow2e.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    tr.Cells.Add(tdRow2e);

                    System.Web.UI.WebControls.TableCell tdRow2b = new System.Web.UI.WebControls.TableCell();
                    tdRow2b.Width = System.Web.UI.WebControls.Unit.Percentage(3);
                    tdRow2b.Text = ds.Tables[0].Rows[i]["TotalAmtAsPerPaperPassed"].ToString() + "/-";
                    tdRow2b.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    tr.Cells.Add(tdRow2b);

                    System.Web.UI.WebControls.TableCell tdRow2c = new System.Web.UI.WebControls.TableCell();
                    tdRow2c.Width = System.Web.UI.WebControls.Unit.Percentage(3);
                    tdRow2c.Text = ds.Tables[0].Rows[i]["PreviousAmountReleased"].ToString() + "/-";
                    tdRow2c.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    tr.Cells.Add(tdRow2c);

                    System.Web.UI.WebControls.TableCell tdRow2d = new System.Web.UI.WebControls.TableCell();
                    tdRow2d.Width = System.Web.UI.WebControls.Unit.Percentage(3);
                    tdRow2d.Text = ds.Tables[0].Rows[i]["RemainsAmtToBeReleased"].ToString() + "/-";
                    tdRow2d.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    tr.Cells.Add(tdRow2d);

                    totAmountReleased = totAmountReleased + Convert.ToInt64(ds.Tables[0].Rows[i]["TotalAmtAsPerPaperPassed"].ToString());
                    PtotAmountReleased = PtotAmountReleased + Convert.ToInt64(ds.Tables[0].Rows[i]["PreviousAmountReleased"].ToString());
                    RtotAmountReleased = RtotAmountReleased + Convert.ToInt64(ds.Tables[0].Rows[i]["RemainsAmtToBeReleased"].ToString());

                    tbl.Rows.Add(tr);
                 
                }

                System.Web.UI.WebControls.TableRow trNew = new System.Web.UI.WebControls.TableRow();
                if (i % 2 == 0)
                    trNew.CssClass = "gdalternate1";
                else
                    trNew.CssClass = "gdrow1";
                System.Web.UI.WebControls.TableCell tdNewRow1 = new System.Web.UI.WebControls.TableCell();
                tdNewRow1.Width = System.Web.UI.WebControls.Unit.Percentage(1);
                tdNewRow1.Height = System.Web.UI.WebControls.Unit.Percentage(15);
                tdNewRow1.Text = "";
                tdNewRow1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                tdNewRow1.BorderWidth = 0;
                tdNewRow1.ColumnSpan = 4;
                tdNewRow1.BorderColor = System.Drawing.Color.White;
                trNew.Cells.Add(tdNewRow1);

                System.Web.UI.WebControls.TableCell tdNewRow21 = new System.Web.UI.WebControls.TableCell();
                tdNewRow21.Width = System.Web.UI.WebControls.Unit.Percentage(8);
                tdNewRow21.Height = System.Web.UI.WebControls.Unit.Percentage(15);
                tdNewRow21.ColumnSpan = 1;
                tdNewRow21.Text = "<b>Total :</b> &nbsp;&nbsp;" + totAmountReleased.ToString();
                tdNewRow21.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tdNewRow21.BorderWidth = 0;
                tdNewRow21.BorderColor = System.Drawing.Color.White;
                trNew.Cells.Add(tdNewRow21);

                System.Web.UI.WebControls.TableCell tdNewRow22 = new System.Web.UI.WebControls.TableCell();
                tdNewRow22.Width = System.Web.UI.WebControls.Unit.Percentage(8);
                tdNewRow22.Height = System.Web.UI.WebControls.Unit.Percentage(15);
                tdNewRow22.ColumnSpan = 1;
                tdNewRow22.Text = "<b>Total :</b> &nbsp;&nbsp;" + PtotAmountReleased.ToString();
                tdNewRow22.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tdNewRow22.BorderWidth = 0;
                tdNewRow22.BorderColor = System.Drawing.Color.White;
                trNew.Cells.Add(tdNewRow22);

                System.Web.UI.WebControls.TableCell tdNewRow23 = new System.Web.UI.WebControls.TableCell();
                tdNewRow23.Width = System.Web.UI.WebControls.Unit.Percentage(8);
                tdNewRow23.Height = System.Web.UI.WebControls.Unit.Percentage(15);
                tdNewRow23.ColumnSpan = 1;
                tdNewRow23.Text = "<b>Total :</b> &nbsp;&nbsp;" + RtotAmountReleased.ToString();
                tdNewRow23.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                tdNewRow23.BorderWidth = 0;
                tdNewRow23.BorderColor = System.Drawing.Color.White;
                trNew.Cells.Add(tdNewRow23);

                tbl.Rows.Add(trNew);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowTableHeaderForPDF()
    {
        try
        {
            System.Web.UI.WebControls.TableHeaderRow th1 = new System.Web.UI.WebControls.TableHeaderRow();
            th1.CssClass = "head1";
            System.Web.UI.WebControls.TableHeaderCell tc1 = new System.Web.UI.WebControls.TableHeaderCell();
            tc1.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            tc1.ColumnSpan = 7;
            tc1.Text = "Purskar Placement Candidate List For Amount Released To Bank ";
            tc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th1.Cells.Add(tc1);
            tbl.Rows.Add(th1);

            System.Web.UI.WebControls.TableHeaderRow th2 = new System.Web.UI.WebControls.TableHeaderRow();
            th2.CssClass = "head1";

            System.Web.UI.WebControls.TableHeaderCell tc2 = new System.Web.UI.WebControls.TableHeaderCell();
            tc2.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            tc2.ColumnSpan = 7;
            tc2.Text = "Report Generated on: " + System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString();
            tc2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
            th2.Cells.Add(tc2);
            tbl.Rows.Add(th2);

            System.Web.UI.WebControls.TableHeaderRow th = new System.Web.UI.WebControls.TableHeaderRow();
            th.CssClass = "head1";

            System.Web.UI.WebControls.TableHeaderCell tcCol = new System.Web.UI.WebControls.TableHeaderCell();
            tcCol.Width = System.Web.UI.WebControls.Unit.Percentage(1);
            tcCol.Text = "#";
            tcCol.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            System.Web.UI.WebControls.TableHeaderCell tcCol4 = new System.Web.UI.WebControls.TableHeaderCell();
            tcCol4.Width = System.Web.UI.WebControls.Unit.Percentage(5);
            tcCol4.Text = "RegnNo";
            tcCol4.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            System.Web.UI.WebControls.TableHeaderCell tcCol2 = new System.Web.UI.WebControls.TableHeaderCell();
            tcCol2.Width = System.Web.UI.WebControls.Unit.Percentage(20);
            tcCol2.Text = "Name";
            tcCol2.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            System.Web.UI.WebControls.TableHeaderCell tcCol2e = new System.Web.UI.WebControls.TableHeaderCell();
            tcCol2e.Width = System.Web.UI.WebControls.Unit.Percentage(5);
            tcCol2e.Text = "Aadhaar Number";
            tcCol2e.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2e);

            System.Web.UI.WebControls.TableHeaderCell tcCol2b = new System.Web.UI.WebControls.TableHeaderCell();
            tcCol2b.Width = System.Web.UI.WebControls.Unit.Percentage(3);
            tcCol2b.Text = "Total Amount Released As Per Module ";
            tcCol2b.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2b);

            System.Web.UI.WebControls.TableHeaderCell tcCol2f = new System.Web.UI.WebControls.TableHeaderCell();
            tcCol2f.Width = System.Web.UI.WebControls.Unit.Percentage(5);
            tcCol2f.Text = "Already Amount Released";
            tcCol2f.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2f);

            System.Web.UI.WebControls.TableHeaderCell tcCol2g = new System.Web.UI.WebControls.TableHeaderCell();
            tcCol2g.Width = System.Web.UI.WebControls.Unit.Percentage(5);
            tcCol2g.Text = "To Be Amount Released";
            tcCol2g.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2g);
            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
   
    public void PuraskarApplicationHistory(Int64 id, Int64 examid,int act)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection Conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("OnlinePuraskarReleasedAmountDownloadHistory", Conn))
                {
                    Conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ExamID", SqlDbType.BigInt));
                    cmd.Parameters["@ExamID"].Value = examid;
                    cmd.Parameters.Add(new SqlParameter("@Actions", SqlDbType.Int));
                    cmd.Parameters["@Actions"].Value = act; //act= 21  for history of application 
                    cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.BigInt));
                    cmd.Parameters["@ID"].Value = id;
                    cmd.Parameters.Add(new SqlParameter("@EnterBy", SqlDbType.BigInt));
                    cmd.Parameters["@EnterBy"].Value = Convert.ToInt64(Session["UserID"]);
                    cmd.Parameters.Add("@pReturnStatus", SqlDbType.VarChar, 500);
                    cmd.Parameters["@pReturnStatus"].Direction = ParameterDirection.Output; 
                    cmd.ExecuteNonQuery();
                  
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnDownloadWithLockCell_Click(object sender, EventArgs e)
    {  
        String filename = "";
        Int64 TOTALAMOUNT = 0, PTOTALAMOUNT = 0, RTOTALAMOUNT = 0;
        int rowscount = 0;
        try
        {
            filename = "PurskarPlacementCandListForAmountReleased_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("GetRecordForProtsahanDocsUploadedAmountReleased", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CodeForViewRecord", SqlDbType.Int).Value = 2;                   
                    cmd.CommandTimeout = 50000;

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (System.Data.DataTable dt = new System.Data.DataTable())
                        {
                            sda.Fill(dt);
                            using (XLWorkbook wb = new XLWorkbook())
                            {
                                // modified sheet as per requirement
                                var ws = wb.Worksheets.Add("PlacementAmountReleasedToBank");
                                if (dt.Rows.Count > 0)
                                {                                   
                                    //// Create a DataTable and add two Columns to it 
                                    // For Report heading display
                                    ws.Cell(1, 1).Value = "Protsahan Puraskar Placement Candidates List to be sent to Bank for Amount Release";
                                    ws.Range(1, 1, 1, 12).Merge().AddToNamed("Titles");
                                    // For Report generated date and time display
                                    ws.Cell(2, 1).Value = "Report Generated on: " + System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString();
                                    // row, column, row, column / merge second row from first column to 12 column
                                    ws.Range(2, 1, 2, 12).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    // Prepare the style for the titles
                                    var titlesStyle = wb.Style;
                                    titlesStyle.Font.Bold = true; titlesStyle.Font.FontSize = 13;
                                    titlesStyle.Font.Underline = XLFontUnderlineValues.Single;
                                    titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    titlesStyle.Fill.BackgroundColor = XLColor.White;
                                    // Format all titles in one shot
                                    wb.NamedRanges.NamedRange("Titles").Ranges.Style = titlesStyle;
                                   

                                    ws.Cell(3, 1).Value = "SL NO."; ws.Cell(3, 7).Value = "PREVIOUS AMOUNT RELEASED (Rs)";
                                    ws.Cell(3, 2).Value = "OnlineRefNo."; ws.Cell(3, 8).Value = "AMOUNT TO BE RELEASED (Rs)";
                                    ws.Cell(3, 3).Value = "REGN NO.";  ws.Cell(3, 9).Value = "TRANSACTION ID";
                                    ws.Cell(3, 4).Value = "NAME";  ws.Cell(3, 10).Value = "TRANSACTION STATUS";
                                    ws.Cell(3, 5).Value = "Aadhaar Number"; ws.Cell(3, 11).Value = "TRANSACTION DATE";
                                    ws.Cell(3, 6).Value = "TOTAL AMT AS PER PAPER PASSED (Rs)";
                                    ws.Cell(3, 12).Value = "REMARKS";

                                    // column border on sheets
                                    ws.Range(3, 1, 3, 12).Style.Border.InsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                    ws.Range(3, 1, 3, 12).Style.Border.InsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                    ws.Range(3, 1, 3, 12).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                    ws.Range(3, 1, 3, 12).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                    // Adding DataRows.
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        Int64 OnlinePuraskarAppID = Convert.ToInt64(dt.Rows[i][12].ToString());
                                        Int64 ExamID = Convert.ToInt64(dt.Rows[i][13].ToString());                                       

                                            ws.Cell("A" + (i + 4)).Value = dt.Rows[i][0]; ws.Cell("B" + (i + 4)).Value = dt.Rows[i][1];
                                            ws.Cell("c" + (i + 4)).Value = dt.Rows[i][2]; ws.Cell("D" + (i + 4)).Value =dt.Rows[i][3] ;
                                            ws.Cell("E" + (i + 4)).Value = "'" + EncryptDecrypt.DecryptString(dt.Rows[i][11].ToString()).ToString();
                                            ws.Cell("F" + (i + 4)).Value = dt.Rows[i][6];
                                            ws.Cell("G" + (i + 4)).Value = dt.Rows[i][5];
                                            ws.Cell("H" + (i + 4)).Value = dt.Rows[i][7];
                                            ws.Cell("I" + (i + 4)).Value = "'";                                            
                                            ws.Cell("J" + (i + 4)).SetDataValidation().List("\"SUCCESS,FAIL\"", true);
                                            ws.Cell("K" + (i + 4)).Value = "'"; ws.Cell("L" + (i + 4)).Value = "'";

                                            TOTALAMOUNT = TOTALAMOUNT + Convert.ToInt64(dt.Rows[i][6].ToString()); // amount to be realsed 5 6  7
                                            PTOTALAMOUNT = PTOTALAMOUNT + Convert.ToInt64(dt.Rows[i][5].ToString());
                                            RTOTALAMOUNT = RTOTALAMOUNT + Convert.ToInt64(dt.Rows[i][7].ToString());
                                            // Display for border
                                            ws.Cell("A" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                            ws.Cell("A" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                            ws.Cell("B" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                            ws.Cell("B" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                            ws.Cell("C" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                            ws.Cell("C" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                            ws.Cell("D" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                            ws.Cell("D" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                            ws.Cell("E" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                            ws.Cell("E" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;

                                            ws.Cell("F" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                            ws.Cell("F" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                            ws.Cell("G" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                            ws.Cell("G" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                            ws.Cell("H" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                            ws.Cell("H" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                            // FOR EMPTY CELL BORDER 20 apr 22
                                            ws.Cell("I" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                            ws.Cell("I" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                            ws.Cell("J" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                            ws.Cell("J" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                            ws.Cell("K" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                            ws.Cell("K" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                            ws.Cell("L" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                            ws.Cell("L" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;                                                                                 
                                    }

                                    // Total amount released to Bank display on report sheets.
                                    rowscount = dt.Rows.Count;
                                    ws.Cell("F" + (rowscount + 4)).Value = "Total :  " + TOTALAMOUNT;
                                    ws.Cell("F" + (rowscount + 4)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    ws.Cell("F" + (rowscount + 4)).Style.Font.Bold = true;
                                    ws.Cell("F" + (rowscount + 4)).Style.Fill.BackgroundColor = XLColor.Black;
                                    ws.Cell("F" + (rowscount + 4)).Style.Font.FontColor = XLColor.White;

                                    ws.Cell("G" + (rowscount + 4)).Value = "Total :  " + PTOTALAMOUNT;
                                    ws.Cell("G" + (rowscount + 4)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    ws.Cell("G" + (rowscount + 4)).Style.Font.Bold = true;
                                    ws.Cell("G" + (rowscount + 4)).Style.Fill.BackgroundColor = XLColor.Black;
                                    ws.Cell("G" + (rowscount + 4)).Style.Font.FontColor = XLColor.White;

                                    ws.Cell("H" + (rowscount + 4)).Value = "Total :  " + RTOTALAMOUNT;
                                    ws.Cell("H" + (rowscount + 4)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    ws.Cell("H" + (rowscount + 4)).Style.Font.Bold = true;
                                    ws.Cell("H" + (rowscount + 4)).Style.Fill.BackgroundColor = XLColor.Black;
                                    ws.Cell("H" + (rowscount + 4)).Style.Font.FontColor = XLColor.White;
                                }
                                ws.Protect("Shammy@123$");
                                ws.Range("F1", "M3000").Style.Protection.SetLocked(false);
                                // Formating cell Date display date format.
                                var col12 = ws.Column("K");
                                col12.Width = 12;
                                col12.Style.DateFormat.Format = "dd-MMM-yyyy";
                             
                                ws.Row(3).CellsUsed(true).Style.Fill.BackgroundColor = XLColor.White;
                                ws.Row(3).CellsUsed(true).Style.Font.SetBold(true).Font.FontSize = 12;

                                //Wrap the text display fit in Sheets.
                                ws.Column("A").Width = 5;
                                ws.Cell(3, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(1).Style.Alignment.WrapText = true;
                                ws.Column("B").Width = 25;
                                ws.Cell(3, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(2).Style.Alignment.WrapText = true;
                                ws.Column("C").Width = 10;
                                ws.Cell(3, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(3).Style.Alignment.WrapText = true;
                                ws.Column("D").Width = 21;
                                ws.Cell(3, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(4).Style.Alignment.WrapText = true;
                                ws.Column("E").Width = 14;
                                ws.Cell(3, 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(5).Style.Alignment.WrapText = true;
                                ws.Column("F").Width = 21;
                                ws.Cell(3, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(6).Style.Alignment.WrapText = true;
                                ws.Column("G").Width = 20;
                                ws.Cell(3, 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(7).Style.Alignment.WrapText = true;
                                ws.Column("H").Width = 18;
                                ws.Cell(3, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(8).Style.Alignment.WrapText = true;
                                ws.Column("I").Width = 18;
                                ws.Cell(3, 9).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(9).Style.Alignment.WrapText = true;
                                ws.Column("J").Width = 16;
                                ws.Cell(3, 10).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(10).Style.Alignment.WrapText = true;
                                ws.Column("K").Width = 21;
                                ws.Cell(3, 11).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(11).Style.Alignment.WrapText = true;
                                ws.Column("L").Width = 21;
                                ws.Cell(3, 12).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(12).Style.Alignment.WrapText = true;                               

                                //Excel file creation is done and download .
                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";                              
                                Response.AddHeader("content-disposition", "attachment;filename=" + filename);

                                using (MemoryStream MyMemoryStream = new MemoryStream())
                                {
                                    wb.SaveAs(MyMemoryStream);
                                    MyMemoryStream.WriteTo(Response.OutputStream);
                                    Response.Flush();
                                    Response.End();
                                }
                            }
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);           
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void btnPdfDownload_Click(object sender, EventArgs e)
    {
        try
        {
            getDataForPDF();           
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            hw.AddAttribute("border", "1");
            hw.RenderBeginTag(HtmlTextWriterTag.Font);
            hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "10");

            divReportData.Visible = true;
            tbl.BorderStyle = System.Web.UI.WebControls.BorderStyle.Solid;
            tbl.CssClass = "sample3";
            tbl.CellPadding = 2;
            tbl.CellSpacing = 1;
            tbl.Width = System.Web.UI.WebControls.Unit.Percentage(200);
           
            divReportData.Controls.Add(tbl);           
            divReportData.RenderControl(hw);
            hw.RenderEndTag();
            Response.Clear();
           
            StringReader sr = new StringReader(sw.ToString());
            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);

            iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);
            iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
           
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=PurskarPlacementCandListForAmountReleased.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            
            Response.Write(pdfDoc);
            Response.End();

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }   
} 