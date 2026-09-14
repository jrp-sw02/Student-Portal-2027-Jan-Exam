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
//using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;
using System.Transactions;
using EConnect.Utils.Common;
using System.Drawing;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html.simpleparser;
using ClosedXML.Excel;
//using Microsoft.Office.Interop.Word;
using Independentsoft.Office;
using Independentsoft.Office.Word;
using Independentsoft.Office.Word.Sections;
using Independentsoft.Office.Word.Tables;
using Independentsoft.Office.Drawing;
using Independentsoft.Office.Word.Drawing;
using Independentsoft.Office.Word.Styles;
using System.Text;
using System.Net;



public partial class DownloadPurskarCandListForAmountReleasedToBank : BasePage
{
    String strMessage = string.Empty;
    string aadharDecrypted = "";
    EConnectContext context;
    Int32 currentRoleId = 0;
    string examName = "";
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
                foreach (System.Web.UI.WebControls.GridViewRow row in gvMain.Rows)
                {
                    if (row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
                    {
                        System.Web.UI.WebControls.HyperLink hylExamName = row.Cells[4].Controls.OfType<System.Web.UI.WebControls.HyperLink>().FirstOrDefault(); 
                        if(hylExamName!=null)
                            examName = hylExamName.Text;
                       // examName = lblExamid.Text;
                        hfcode.Value = examName;

                        
                    }
                }
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Purskar Candidates List to be sent to Bank for Amount Release", "HO/DownloadPurskarCandListForAmountReleasedToBank.aspx", ""));
                 
                 
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("DownloadPurskarCandListForAmountReleasedToBank.aspx", true);
    }
    protected void BindGridView()
    {
        try
        {
            using (System.Data.DataTable dt = FillGridViewOnlinePuraskarApplicationRecordExamiIdWise
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
                                               //DOB = p.Field<DateTime>("DOB"),
                                               AadharNumber=p.Field <string>("AadharNumber"),
                                               BankName = p.Field<string>("BankName"),
                                               AccountNumber = p.Field<string>("AccountNumber"),
                                               AmountToBeReleased = p.Field<Int64>("AmountToBeReleased").ToString() +"/-",  
                                           });
                    
                    PagingBar1.Bind(ApplicationData, ref gvMain);
                    uPnlGrid.Update();
                    uPnlNavigation.Update();
                    lblError.Visible = false;
                   // btnDownload.Visible = true;
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
                //HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                //string href = hl.NavigateUrl;
                //if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                //{
                //    href += "&ID=" + Request.QueryString["ID"].ToString();
                //}               
              
                //e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

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

    public System.Data.DataTable FillGridViewOnlinePuraskarApplicationRecordExamiIdWise()
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        System.Data.DataTable myDt = new System.Data.DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("FillGridViewOnlinePuraskarApplicationRecordExamiIdWise", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CodeForViewRecord", SqlDbType.Int));
                cmd.Parameters["@CodeForViewRecord"].Value = 14;
                cmd.Parameters.Add(new SqlParameter("@InstituteID", SqlDbType.BigInt));
                cmd.Parameters["@InstituteID"].Value = 0;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
    public System.Data.DataTable FillGridViewOnlinePuraskarApplicationRecordExamiIdWiseR()
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        System.Data.DataTable myDt = new System.Data.DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("FillGridViewOnlinePuraskarApplicationRecordExamiIdWise", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CodeForViewRecord", SqlDbType.Int));
                cmd.Parameters["@CodeForViewRecord"].Value = 97;
                cmd.Parameters.Add(new SqlParameter("@InstituteID", SqlDbType.BigInt));
                cmd.Parameters["@InstituteID"].Value = 0;
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
           // getData();
            divReportData.Controls.Add(tbl);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=PurskarCandListForAmountReleased.xls");
            Response.Charset = "";
            //Response.ContentType = "application/vnd.xls";
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
    ////protected void getData()
    ////{
    ////    try
    ////    {
    ////        ShowTableHeader();

           
    ////        EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
    ////        SqlCommand scCommand = new SqlCommand("FillGridViewOnlinePuraskarApplicationRecordExamiIdWise", new SqlConnection(con.ConnectionString));
    ////        scCommand.CommandType = CommandType.StoredProcedure;            
    ////        scCommand.Parameters.Add("@CodeForViewRecord", SqlDbType.Int).Value = 14;
    ////        scCommand.Parameters.Add(new SqlParameter("@InstituteID", SqlDbType.BigInt));
    ////        scCommand.Parameters["@InstituteID"].Value = 0;
    ////        scCommand.CommandTimeout = 50000;
    ////        if (scCommand.Connection.State == ConnectionState.Closed)
    ////        {
    ////            scCommand.Connection.Open();
    ////        }

    ////        SqlDataAdapter da = new SqlDataAdapter(scCommand);
    ////        DataSet ds = new DataSet();

    ////        da.Fill(ds);

    ////        if (ds.Tables[0].Rows.Count > 0)
    ////        {
    ////            int i;
    ////            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
    ////            {
    ////                TableRow tr = new TableRow();
    ////                if (i % 2 == 0)
    ////                    tr.CssClass = "gdalternate1";
    ////                else
    ////                    tr.CssClass = "gdrow1";

    ////                TableCell tdRow = new TableCell();
    ////                tdRow.Width = Unit.Percentage(1);
    ////                tdRow.Text = ds.Tables[0].Rows[i]["sl"].ToString();
    ////                    //(i + 1).ToString();
    ////                tdRow.HorizontalAlign = HorizontalAlign.Center;
    ////                tr.Cells.Add(tdRow);
                    
    ////                TableCell tdRow4 = new TableCell();
    ////                tdRow4.Width = Unit.Percentage(5);
    ////                tdRow4.Text = ds.Tables[0].Rows[i]["Regno"].ToString();
    ////                tdRow4.HorizontalAlign = HorizontalAlign.Left;
    ////                tr.Cells.Add(tdRow4);

    ////                //
    ////                string encAdhar = ds.Tables[0].Rows[i]["AadharNumber"].ToString();
    ////                aadharDecrypted = EncryptDecrypt.DecryptString(encAdhar);
    ////                hfActionID.Value  = aadharDecrypted;
    ////                //


    ////                TableCell tdRow2 = new TableCell();
    ////                tdRow2.Width = Unit.Percentage(40);
    ////                tdRow2.Text = ds.Tables[0].Rows[i]["Name"].ToString();
    ////                tdRow2.HorizontalAlign = HorizontalAlign.Left;
    ////                tr.Cells.Add(tdRow2);
                    
    ////                TableCell tdRow2b = new TableCell();
    ////                tdRow2b.Width = Unit.Percentage(3);
    ////                tdRow2b.Text = ds.Tables[0].Rows[i]["AmountToBeReleased"].ToString();
    ////                tdRow2b.HorizontalAlign = HorizontalAlign.Right;
    ////                tr.Cells.Add(tdRow2b);

    ////                TableCell tdRow2e = new TableCell();
    ////                tdRow2e.Width = Unit.Percentage(3);
    ////                tdRow2e.Text = aadharDecrypted;//ds.Tables[0].Rows[i]["AccountHolderName"].ToString();
    ////                tdRow2e.HorizontalAlign = HorizontalAlign.Left;
    ////                tr.Cells.Add(tdRow2e);
                   
    ////                TableCell tdRow2c = new TableCell();
    ////                tdRow2c.Width = Unit.Percentage(9);
    ////                tdRow2c.Text = "'" + ds.Tables[0].Rows[i]["AccountNumber"].ToString(); //
    ////                tdRow2c.HorizontalAlign = HorizontalAlign.Left;
    ////                tr.Cells.Add(tdRow2c);
                    
    ////                TableCell tdRow2d = new TableCell();
    ////                tdRow2d.Width = Unit.Percentage(1);
    ////                tdRow2d.Text = ds.Tables[0].Rows[i]["AccountType"].ToString();
    ////                tdRow2d.HorizontalAlign = HorizontalAlign.Center;
    ////                tr.Cells.Add(tdRow2d);
                   
    ////                TableCell tdRow2a = new TableCell();
    ////                tdRow2a.Width = Unit.Percentage(3);
    ////                tdRow2a.Text = ds.Tables[0].Rows[i]["BankName"].ToString();
    ////                tdRow2a.HorizontalAlign = HorizontalAlign.Center;
    ////                tr.Cells.Add(tdRow2a);                    

    ////                TableCell tdRow5 = new TableCell();
    ////                tdRow5.Width = Unit.Percentage(6);
    ////                tdRow5.Text = ds.Tables[0].Rows[i]["BankIfscCode"].ToString();                    
    ////                tdRow5.HorizontalAlign = HorizontalAlign.Center;
    ////                tr.Cells.Add(tdRow5);

    ////                TableCell tdRow51 = new TableCell();
    ////                tdRow51.Width = Unit.Percentage(6);
    ////                tdRow51.Text = "";
    ////                tdRow51.HorizontalAlign = HorizontalAlign.Center;
    ////                tr.Cells.Add(tdRow51);

    ////                TableCell tdRow52 = new TableCell();
    ////                tdRow52.Width = Unit.Percentage(6);
    ////                tdRow52.Text = "";
    ////                tdRow52.HorizontalAlign = HorizontalAlign.Center;
    ////                tr.Cells.Add(tdRow52);

    ////                TableCell tdRow53 = new TableCell();
    ////                tdRow53.Width = Unit.Percentage(6);
    ////                tdRow53.Text = "";
    ////                tdRow53.HorizontalAlign = HorizontalAlign.Center;
    ////                tr.Cells.Add(tdRow53);
                    
    ////                totAmountReleased = totAmountReleased+ Convert.ToInt64(ds.Tables[0].Rows[i]["AmountToBeReleased"].ToString()); 

    ////                tbl.Rows.Add(tr);
    ////            }

    ////            TableRow trNew = new TableRow();
    ////            if (i % 2 == 0)
    ////                trNew.CssClass = "gdalternate1";
    ////            else
    ////                trNew.CssClass = "gdrow1";
    ////            TableCell tdNewRow1 = new TableCell();
    ////            tdNewRow1.Width = Unit.Percentage(1);
    ////            tdNewRow1.Height = Unit.Percentage(15);
    ////            tdNewRow1.Text = "";
    ////            tdNewRow1.HorizontalAlign = HorizontalAlign.Right;
    ////            tdNewRow1.BorderWidth = 1;
    ////            tdNewRow1.ColumnSpan = 3;
    ////            tdNewRow1.BorderColor = System.Drawing.Color.White;
    ////            trNew.Cells.Add(tdNewRow1);                

    ////            TableCell tdNewRow21 = new TableCell();
    ////            tdNewRow21.Width = Unit.Percentage(8);
    ////            tdNewRow21.Height = Unit.Percentage(15);
    ////            tdNewRow21.Text = "<b>Total :</b> &nbsp;&nbsp;" + totAmountReleased.ToString();
    ////            tdNewRow21.HorizontalAlign = HorizontalAlign.Right;
    ////            tdNewRow21.BorderWidth = 1;
    ////            tdNewRow21.BorderColor = System.Drawing.Color.White;
    ////            trNew.Cells.Add(tdNewRow21);               

    ////            tbl.Rows.Add(trNew);
    ////        }
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        ShowAlert(ex.Message);
    ////    }
    ////}
    protected void getDataForPDF()
    {
        try
        {
            ShowTableHeaderForPDF();

            Int64 totAmountReleased = 0;
            EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            SqlCommand scCommand = new SqlCommand("FillGridViewOnlinePuraskarApplicationRecordExamiIdWise", new SqlConnection(con.ConnectionString));
            scCommand.CommandType = CommandType.StoredProcedure;
            scCommand.Parameters.Add("@CodeForViewRecord", SqlDbType.Int).Value = 14;
            scCommand.Parameters.Add("@InstituteID", SqlDbType.Int).Value = 0;
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

                    System.Web.UI.WebControls.TableCell tdRow2b = new System.Web.UI.WebControls.TableCell();
                    tdRow2b.Width = System.Web.UI.WebControls.Unit.Percentage(3);
                    tdRow2b.Text = ds.Tables[0].Rows[i]["AmountToBeReleased"].ToString();
                    tdRow2b.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    tr.Cells.Add(tdRow2b);

                    System.Web.UI.WebControls.TableCell tdRow2e = new System.Web.UI.WebControls.TableCell();
                    tdRow2e.Width = System.Web.UI.WebControls.Unit.Percentage(3);
                    tdRow2e.Text = ds.Tables[0].Rows[i]["AccountHolderName"].ToString();
                    tdRow2e.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    tr.Cells.Add(tdRow2e);

                    System.Web.UI.WebControls.TableCell tdRow2c = new System.Web.UI.WebControls.TableCell();
                    tdRow2c.Width = System.Web.UI.WebControls.Unit.Percentage(9);
                    tdRow2c.Text = ds.Tables[0].Rows[i]["AccountNumber"].ToString();
                    tdRow2c.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    tr.Cells.Add(tdRow2c);

                    System.Web.UI.WebControls.TableCell tdRow2d = new System.Web.UI.WebControls.TableCell();
                    tdRow2d.Width = System.Web.UI.WebControls.Unit.Percentage(1);
                    tdRow2d.Text = ds.Tables[0].Rows[i]["AccountType"].ToString();
                    tdRow2d.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tr.Cells.Add(tdRow2d);

                    System.Web.UI.WebControls.TableCell tdRow2a = new System.Web.UI.WebControls.TableCell();
                    tdRow2a.Width = System.Web.UI.WebControls.Unit.Percentage(3);
                    tdRow2a.Text = ds.Tables[0].Rows[i]["BankName"].ToString();
                    tdRow2a.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tr.Cells.Add(tdRow2a);

                    System.Web.UI.WebControls.TableCell tdRow5 = new System.Web.UI.WebControls.TableCell();
                    tdRow5.Width = System.Web.UI.WebControls.Unit.Percentage(6);
                    tdRow5.Text = ds.Tables[0].Rows[i]["BankIfscCode"].ToString();
                    tdRow5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    tr.Cells.Add(tdRow5);

                    //TableCell tdRow51 = new TableCell();
                    //tdRow51.Width = Unit.Percentage(6);
                    //tdRow51.Text = "";
                    //tdRow51.HorizontalAlign = HorizontalAlign.Center;
                    //tr.Cells.Add(tdRow51);

                    //TableCell tdRow52 = new TableCell();
                    //tdRow52.Width = Unit.Percentage(6);
                    //tdRow52.Text = "";
                    //tdRow52.HorizontalAlign = HorizontalAlign.Center;
                    //tr.Cells.Add(tdRow52);

                    //TableCell tdRow53 = new TableCell();
                    //tdRow53.Width = Unit.Percentage(6);
                    //tdRow53.Text = "";
                    //tdRow53.HorizontalAlign = HorizontalAlign.Center;
                    //tr.Cells.Add(tdRow53);

                    totAmountReleased = totAmountReleased + Convert.ToInt64(ds.Tables[0].Rows[i]["AmountToBeReleased"].ToString());

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
                tdNewRow1.ColumnSpan = 3;
                tdNewRow1.BorderColor = System.Drawing.Color.White;
                trNew.Cells.Add(tdNewRow1);

                System.Web.UI.WebControls.TableCell tdNewRow21 = new System.Web.UI.WebControls.TableCell();
                tdNewRow21.Width = System.Web.UI.WebControls.Unit.Percentage(8);
                tdNewRow21.Height = System.Web.UI.WebControls.Unit.Percentage(15);
                tdNewRow21.ColumnSpan = 6;
                tdNewRow21.Text = "<b>Total :</b> &nbsp;&nbsp;" + totAmountReleased.ToString();
                tdNewRow21.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                tdNewRow21.BorderWidth = 0;
                tdNewRow21.BorderColor = System.Drawing.Color.White;
                trNew.Cells.Add(tdNewRow21);

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
            tc1.ColumnSpan = 9;
            tc1.Text = "Purskar Candidate List For Amount Released To Bank ";
            tc1.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th1.Cells.Add(tc1);

            tbl.Rows.Add(th1);

            System.Web.UI.WebControls.TableHeaderRow th2 = new System.Web.UI.WebControls.TableHeaderRow();
            th2.CssClass = "head1";

            System.Web.UI.WebControls.TableHeaderCell tc2 = new System.Web.UI.WebControls.TableHeaderCell();
            tc2.Width = System.Web.UI.WebControls.Unit.Percentage(100);
            tc2.ColumnSpan = 9;
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

            System.Web.UI.WebControls.TableHeaderCell tcCol2b = new System.Web.UI.WebControls.TableHeaderCell();
            tcCol2b.Width = System.Web.UI.WebControls.Unit.Percentage(3);
            tcCol2b.Text = "Amount Released";
            tcCol2b.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2b);

            System.Web.UI.WebControls.TableHeaderCell tcCol2e = new System.Web.UI.WebControls.TableHeaderCell();
            tcCol2e.Width = System.Web.UI.WebControls.Unit.Percentage(5);
            tcCol2e.Text = "Account Holder Name";
            tcCol2e.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2e);

            System.Web.UI.WebControls.TableHeaderCell tcCol2c = new System.Web.UI.WebControls.TableHeaderCell();
            tcCol2c.Width = System.Web.UI.WebControls.Unit.Percentage(9);
            tcCol2c.Text = "Account Number";
            tcCol2c.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2c);

            System.Web.UI.WebControls.TableHeaderCell tcCol2d = new System.Web.UI.WebControls.TableHeaderCell();
            tcCol2d.Width = System.Web.UI.WebControls.Unit.Percentage(1);
            tcCol2d.Text = "Account Type";
            tcCol2d.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2d);

            System.Web.UI.WebControls.TableHeaderCell tcCol2a = new System.Web.UI.WebControls.TableHeaderCell();
            tcCol2a.Width = System.Web.UI.WebControls.Unit.Percentage(5);
            tcCol2a.Text = "Bank Name";
            tcCol2a.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            th.Cells.Add(tcCol2a);

            System.Web.UI.WebControls.TableHeaderCell tcCol5 = new System.Web.UI.WebControls.TableHeaderCell();
            tcCol5.Width = System.Web.UI.WebControls.Unit.Percentage(6);
            tcCol5.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
            tcCol5.Text = "Bank IFSC Code";
            th.Cells.Add(tcCol5);

            //TableHeaderCell tcCol6 = new TableHeaderCell();
            //tcCol6.Width = Unit.Percentage(6);
            //tcCol6.HorizontalAlign = HorizontalAlign.Center;
            //tcCol6.Text = "Transaction Status";
            //th.Cells.Add(tcCol6);

            //TableHeaderCell tcCol7 = new TableHeaderCell();
            //tcCol7.Width = Unit.Percentage(6);
            //tcCol7.HorizontalAlign = HorizontalAlign.Center;
            //tcCol7.Text = "Transaction Date";
            //th.Cells.Add(tcCol7);

            //TableHeaderCell tcCol8 = new TableHeaderCell();
            //tcCol8.Width = Unit.Percentage(6);
            //tcCol8.HorizontalAlign = HorizontalAlign.Center;
            //tcCol8.Text = "Remarks";
            //th.Cells.Add(tcCol8);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    ////protected void ShowTableHeader()
    ////{   
    ////    try
    ////    {
    ////        TableHeaderRow th1 = new TableHeaderRow();
    ////        th1.CssClass = "head1";
    ////        TableHeaderCell tc1 = new TableHeaderCell();
    ////        tc1.Width = Unit.Percentage(100);
    ////        tc1.ColumnSpan = 12;
    ////        tc1.Text = "Purskar Candidate List For Amount Released To Bank ";
    ////        tc1.HorizontalAlign = HorizontalAlign.Center;
    ////        th1.Cells.Add(tc1);

    ////        tbl.Rows.Add(th1);
       
    ////        TableHeaderRow th2 = new TableHeaderRow();
    ////        th2.CssClass = "head1";

    ////        TableHeaderCell tc2 = new TableHeaderCell();
    ////        tc2.Width = Unit.Percentage(100);
    ////        tc2.ColumnSpan = 12;
    ////        tc2.Text = "Report Generated on: " + System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString();
    ////        tc2.HorizontalAlign = HorizontalAlign.Right;
    ////        th2.Cells.Add(tc2);

    ////        tbl.Rows.Add(th2);
         
    ////        TableHeaderRow th = new TableHeaderRow();
    ////        th.CssClass = "head1";

    ////        TableHeaderCell tcCol = new TableHeaderCell();
    ////        tcCol.Width = Unit.Percentage(1);
    ////        tcCol.Text = "#";
    ////        tcCol.HorizontalAlign = HorizontalAlign.Center;
    ////        th.Cells.Add(tcCol);

    ////        TableHeaderCell tcCol4 = new TableHeaderCell();
    ////        tcCol4.Width = Unit.Percentage(5);
    ////        tcCol4.Text = "RegnNo";           
    ////        tcCol4.HorizontalAlign = HorizontalAlign.Center;
    ////        th.Cells.Add(tcCol4);           

    ////        TableHeaderCell tcCol2 = new TableHeaderCell();
    ////        tcCol2.Width = Unit.Percentage(20);
    ////        tcCol2.Text = "Name";
    ////        tcCol2.HorizontalAlign = HorizontalAlign.Center;
    ////        th.Cells.Add(tcCol2);

    ////        TableHeaderCell tcCol2b = new TableHeaderCell();
    ////        tcCol2b.Width = Unit.Percentage(3);
    ////        tcCol2b.Text = "Amount Released";
    ////        tcCol2b.HorizontalAlign = HorizontalAlign.Center;
    ////        th.Cells.Add(tcCol2b);
          
    ////        TableHeaderCell tcCol2e = new TableHeaderCell();
    ////        tcCol2e.Width = Unit.Percentage(5);
    ////        tcCol2e.Text = "Account Holder Name";
    ////        tcCol2e.HorizontalAlign = HorizontalAlign.Center;
    ////        th.Cells.Add(tcCol2e);

    ////        TableHeaderCell tcCol2c = new TableHeaderCell();
    ////        tcCol2c.Width = Unit.Percentage(9);
    ////        tcCol2c.Text = "Account Number";
    ////        tcCol2c.HorizontalAlign = HorizontalAlign.Center;
    ////        th.Cells.Add(tcCol2c);
 
    ////        TableHeaderCell tcCol2d = new TableHeaderCell();
    ////        tcCol2d.Width = Unit.Percentage(1);
    ////        tcCol2d.Text = "Account Type";
    ////        tcCol2d.HorizontalAlign = HorizontalAlign.Center;
    ////        th.Cells.Add(tcCol2d);
            
    ////        TableHeaderCell tcCol2a = new TableHeaderCell();
    ////        tcCol2a.Width = Unit.Percentage(5);
    ////        tcCol2a.Text = "Bank Name";
    ////        tcCol2a.HorizontalAlign = HorizontalAlign.Center;
    ////        th.Cells.Add(tcCol2a);

    ////        TableHeaderCell tcCol5 = new TableHeaderCell();
    ////        tcCol5.Width = Unit.Percentage(6);
    ////        tcCol5.HorizontalAlign = HorizontalAlign.Center;
    ////        tcCol5.Text = "Bank IFSC Code";
    ////        th.Cells.Add(tcCol5);

    ////        TableHeaderCell tcCol6 = new TableHeaderCell();
    ////        tcCol6.Width = Unit.Percentage(6);
    ////        tcCol6.HorizontalAlign = HorizontalAlign.Center;
    ////        tcCol6.Text = "Transaction Status";
    ////        th.Cells.Add(tcCol6);

    ////        TableHeaderCell tcCol7 = new TableHeaderCell();
    ////        tcCol7.Width = Unit.Percentage(6);
    ////        tcCol7.HorizontalAlign = HorizontalAlign.Center;
    ////        tcCol7.Text = "Transaction Date";
    ////        th.Cells.Add(tcCol7);

    ////        TableHeaderCell tcCol8 = new TableHeaderCell();
    ////        tcCol8.Width = Unit.Percentage(6);
    ////        tcCol8.HorizontalAlign = HorizontalAlign.Center;
    ////        tcCol8.Text = "Remarks";
    ////        th.Cells.Add(tcCol8);
            
    ////        tbl.Rows.Add(th);
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        ShowAlert(ex.Message);
    ////    }
    ////}
    
    protected void btnDownloadWithLockCell_Click(object sender, EventArgs e)
    {  
        String filename = "";
        Int64 TOTALAMOUNT = 0 ;
        int rowscount = 0;
        try
        {
            filename = "PurskarCandListForAmountReleased_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                // using (SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER ( 	ORDER BY RegnNo ) slno, RegnNo ,Name, AmountReleased, AccountHolderName, AccountNo , AccountType,BankName,BankIFSC,''TransactionStatus,''TransactionDate,''Remarks,AadharNumber         FROM [NIELIT].[dbo].[OnlinePuraskarApplication] where  applicationStatusID=6 "))
                //using (SqlCommand cmd = new SqlCommand("SELECT ROW_NUMBER() OVER ( 	ORDER BY RegnNo ) slno, RegnNo ,Name, AmountReleased, AccountHolderName, AccountNo , AccountType,BankName,BankIFSC,''TransactionStatus,''TransactionDate,''Remarks,AadharNumber         FROM [NIELIT].[dbo].[OnlinePuraskarApplication] where  applicationStatusID=6 "))
                using (SqlCommand cmd = new SqlCommand("FillGridViewOnlinePuraskarApplicationRecordExamiIdWise", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CodeForViewRecord", SqlDbType.Int).Value = 51;
                    cmd.Parameters.Add("@InstituteID", SqlDbType.Int).Value = 51;
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

                                //var ws = wb.Worksheets.Add(dt, "AmountReleasedToBank");
                                //ws.Protect("List@123$");
                                //ws.Range("J1", "L2000").Style.Protection.SetLocked(false);
                                //// ws.Cell("A2").Style.Protection.SetLocked(true);
                                //// ws.Protect("123");
                                // modified sheet as per requirement
                                var ws = wb.Worksheets.Add("AmountReleasedToBank");
                                if (dt.Rows.Count > 0)
                                {

                                    //// Create a DataTable and add two Columns to it
                                    //DataTable dt1 = new DataTable();
                                    //dt1.Columns.Add("Id", typeof(int));
                                    //dt1.Columns.Add("Status", typeof(string));


                                    //// Create a DataRow, add Name and Age data, and add to the DataTable
                                    //DataRow dr = dt1.NewRow();
                                    //dr["Status"] = "SUCCESS"; 
                                    //dr["Id"] = 1; 
                                    //dt1.Rows.Add(dr);

                                    //// Create another DataRow, add Name and Age data, and add to the DataTable
                                    //dr = dt1.NewRow();
                                    //dr["Status"] = "FAIL"; 
                                    //dr["Id"] = 2; 
                                    //dt1.Rows.Add(dr);


                                    // dt1.Rows.Add(_deep1);
                                    //testing purpose 8 sep
                                    // Adding HeaderRow.                                
                                    //ws.Cell("A1" ).Value = dt.Columns[0].ColumnName; ws.Cell("B1" ).Value = dt.Columns[1].ColumnName;
                                    //ws.Cell("C1" ).Value = dt.Columns[2].ColumnName; ws.Cell("D1" ).Value = dt.Columns[3].ColumnName;
                                    //ws.Cell("E1" ).Value = dt.Columns[4].ColumnName; ws.Cell("F1" ).Value = dt.Columns[5].ColumnName;
                                    //ws.Cell("G1" ).Value = dt.Columns[6].ColumnName; ws.Cell("H1" ).Value = dt.Columns[7].ColumnName;
                                    //ws.Cell("I1").Value = dt.Columns[8].ColumnName;  ws.Cell("J1" ).Value = dt.Columns[9].ColumnName;
                                    //ws.Cell("K1" ).Value = dt.Columns[10].ColumnName;ws.Cell("L1" ).Value = dt.Columns[11].ColumnName;   
                                    // For Report heading display
                                    ws.Cell(1, 1).Value = "Protsahan Puraskar Candidates List to be sent to Bank for Amount Release";
                                    ws.Range(1, 1, 1, 13).Merge().AddToNamed("Titles");
                                    // For Report generated date and time display
                                    ws.Cell(2, 1).Value = "Report Generated on: " + System.DateTime.Now.ToLongDateString() + " " + System.DateTime.Now.ToLongTimeString();
                                    // row, column, row, column / merge second row from first column to 12 column
                                    ws.Range(2, 1, 2, 13).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    // Prepare the style for the titles
                                    var titlesStyle = wb.Style;
                                    titlesStyle.Font.Bold = true; titlesStyle.Font.FontSize = 13;
                                    titlesStyle.Font.Underline = XLFontUnderlineValues.Single;
                                    titlesStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                    titlesStyle.Fill.BackgroundColor = XLColor.White;
                                    // Format all titles in one shot
                                    wb.NamedRanges.NamedRange("Titles").Ranges.Style = titlesStyle;
                                    //first row and second column, column Name display
                                    ws.Cell(3, 1).Value = "SL NO."; ws.Cell(3, 5).Value = "ACCOUNT HOLDER NAME"; ws.Cell(3, 9).Value = "BANK IFSC CODE";
                                    ws.Cell(3, 2).Value = "REGN NO."; ws.Cell(3, 6).Value = "A/C NUMBER"; ws.Cell(3, 10).Value = "TRANSACTION ID";
                                    ws.Cell(3, 3).Value = "NAME"; ws.Cell(3, 7).Value = "A/C TYPE"; ws.Cell(3, 11).Value = "TRANSACTION STATUS";
                                    ws.Cell(3, 4).Value = "AMOUNT TO BE RELEASED (Rs)"; ws.Cell(3, 8).Value = "BANK NAME"; ws.Cell(3, 12).Value = "TRANSACTION DATE";
                                    ws.Cell(3, 13).Value = "REMARKS";
                                    // column border on sheets
                                    ws.Range(3, 1, 3, 13).Style.Border.InsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                    ws.Range(3, 1, 3, 13).Style.Border.InsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                    ws.Range(3, 1, 3, 13).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                    ws.Range(3, 1, 3, 13).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                    // Adding DataRows.
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {

                                        //string encAdh = dt.Rows[i][12].ToString();

                                        //string encAdhar = dt.Rows[i][12].ToString();
                                        //string aadharDecrypted = EncryptDecrypt.DecryptString(encAdhar);

                                        ws.Cell("A" + (i + 4)).Value = dt.Rows[i][0]; ws.Cell("B" + (i + 4)).Value = dt.Rows[i][1];
                                        ws.Cell("c" + (i + 4)).Value = dt.Rows[i][2]; ws.Cell("D" + (i + 4)).Value = dt.Rows[i][3];
                                        ws.Cell("E" + (i + 4)).Value = dt.Rows[i][4]; ws.Cell("F" + (i + 4)).Value = "'" + dt.Rows[i][5].ToString();
                                        ws.Cell("G" + (i + 4)).Value = dt.Rows[i][6]; ws.Cell("H" + (i + 4)).Value = dt.Rows[i][7];
                                        ws.Cell("I" + (i + 4)).Value = dt.Rows[i][8]; ws.Cell("J" + (i + 4)).Value = "'";
                                        //ws.Cell("K" + (i + 4)).Value = "'"; ws.Cell("L" + (i + 4)).Value = "'"; ws.Cell("M" + (i + 4)).Value = "'";

                                        //ws.Column("K" + (i + 4)).SetDataValidation().List("\"SUCCESS,FAIL\"", true); //ws.Column(11).SetDataValidation().List(validateList, true);
                                        ws.Cell("K" + (i + 4)).SetDataValidation().List("\"SUCCESS,FAIL\"", true);

                                        ws.Cell("L" + (i + 4)).Value = "'"; ws.Cell("M" + (i + 4)).Value = "'";




                                        TOTALAMOUNT = TOTALAMOUNT + Convert.ToInt64(dt.Rows[i][3].ToString());
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
                                        ws.Cell("I" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                        ws.Cell("I" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                        // FOR EMPTY CELL BORDER
                                        ws.Cell("j" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                        ws.Cell("j" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                        ws.Cell("K" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                        ws.Cell("K" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                        ws.Cell("L" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                        ws.Cell("L" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                        ws.Cell("M" + (i + 4)).Style.Border.OutsideBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                        ws.Cell("M" + (i + 4)).Style.Border.OutsideBorderColor = ClosedXML.Excel.XLColor.Black;
                                    }
                                    // Total amount released to Bank display on report sheets.
                                    rowscount = dt.Rows.Count;
                                    ws.Cell("D" + (rowscount + 4)).Value = "Total :  " + TOTALAMOUNT;
                                    ws.Cell("D" + (rowscount + 4)).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                                    ws.Cell("D" + (rowscount + 4)).Style.Font.Bold = true;
                                    ws.Cell("D" + (rowscount + 4)).Style.Fill.BackgroundColor = XLColor.Black;
                                    ws.Cell("D" + (rowscount + 4)).Style.Font.FontColor = XLColor.White;
                                }
                                ws.Protect("List@123$");
                                ws.Range("J1", "M3000").Style.Protection.SetLocked(false);
                                // Formating cell Date display date format.
                                var col12 = ws.Column("L");
                                col12.Width = 12;
                                col12.Style.DateFormat.Format = "dd-MMM-yyyy";
                                // ws.Column(1).Cells(1,1).Style.Fill.BackgroundColor = XLColor.BeauBlue;
                                // ws.Row(3).CellsUsed(true).Style.Fill.BackgroundColor = XLColor.BattleshipGrey;
                                ws.Row(3).CellsUsed(true).Style.Fill.BackgroundColor = XLColor.White;
                                ws.Row(3).CellsUsed(true).Style.Font.SetBold(true).Font.FontSize = 12;

                                //Wrap the text display fit in Sheets.
                                ws.Column("A").Width = 5;
                                ws.Cell(3, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(1).Style.Alignment.WrapText = true;
                                ws.Column("B").Width = 10;
                                ws.Cell(3, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(2).Style.Alignment.WrapText = true;
                                ws.Column("C").Width = 20;
                                ws.Cell(3, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(3).Style.Alignment.WrapText = true;
                                ws.Column("D").Width = 14;
                                ws.Cell(3, 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(4).Style.Alignment.WrapText = true;
                                ws.Column("E").Width = 21;
                                ws.Cell(3, 5).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(5).Style.Alignment.WrapText = true;
                                ws.Column("F").Width = 12;
                                ws.Cell(3, 6).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(6).Style.Alignment.WrapText = true;
                                ws.Column("G").Width = 12;
                                ws.Cell(3, 7).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(7).Style.Alignment.WrapText = true;
                                ws.Column("H").Width = 19;
                                ws.Cell(3, 8).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(8).Style.Alignment.WrapText = true;
                                ws.Column("I").Width = 12;
                                ws.Cell(3, 9).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(9).Style.Alignment.WrapText = true;
                                ws.Column("J").Width = 14;
                                ws.Cell(3, 10).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(10).Style.Alignment.WrapText = true;
                                ws.Column("K").Width = 14;
                                ws.Cell(3, 11).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 11).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(11).Style.Alignment.WrapText = true;
                                ws.Column("L").Width = 14;
                                ws.Cell(3, 12).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 12).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(12).Style.Alignment.WrapText = true;
                                ws.Column("M").Width = 25;
                                ws.Cell(3, 13).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                                ws.Cell(3, 13).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                ws.Column(13).Style.Alignment.WrapText = true;

                                //Setting borders to each used cell in excel  
                                //ws.CellsUsed().Style.Border.BottomBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                //ws.CellsUsed().Style.Border.BottomBorderColor = ClosedXML.Excel.XLColor.Black;
                                //ws.CellsUsed().Style.Border.TopBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                //ws.CellsUsed().Style.Border.TopBorderColor = ClosedXML.Excel.XLColor.Black;
                                //ws.CellsUsed().Style.Border.LeftBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                //ws.CellsUsed().Style.Border.LeftBorderColor = ClosedXML.Excel.XLColor.Black;
                                //ws.CellsUsed().Style.Border.RightBorder = ClosedXML.Excel.XLBorderStyleValues.Thin;
                                //ws.CellsUsed().Style.Border.RightBorderColor = ClosedXML.Excel.XLColor.Black; 
                                //ws.Column(2).AdjustToContents(); //ws.Column(3).AdjustToContents(); //ws.Column(4).AdjustToContents(); 
                                //ws.Column(5).AdjustToContents(); //ws.Column(6).AdjustToContents(); //ws.Column(7).AdjustToContents();
                                //ws.Column(8).AdjustToContents(); //ws.Column(9).AdjustToContents(); //ws.Column(10).AdjustToContents(); 
                                // modified sheet as per requirement END

                                //Excel file creation is done and download .
                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                // Response.AddHeader("content-disposition", "attachment;filename=AmountReleasedToBank.xls");
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

            getDataForPDF();
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
            Response.AddHeader("content-disposition", "attachment;filename=PurskarCandListForAmountReleased.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Write(pdfDoc);
            Response.End();

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnDownload3_Click(object sender, EventArgs e)
    {
       // getData();
        CreateDocument();

        //OleDbConnection connection = new OleDbConnection();
        //OleDbCommand command = new OleDbCommand();
        //string ExcelFilePath = "";
        //String filename = "";
        // try
        //    {
        //        filename = "NIELIT_d_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
        //        //ExcelFilePath = Server.MapPath("~/Download/" + filename);
        //        ExcelFilePath = Server.MapPath("~/Download/" + filename);
        //        string fileName =Server.MapPath( "~\\Download\\test1.xls");
        //        if (System.IO.File.Exists(ExcelFilePath))
        //            System.IO.File.Delete(ExcelFilePath);

        //      //  System.IO.File.Copy(Server.MapPath("~/Download/PurskarCandListForAmountReleased.xls"), ExcelFilePath);
        
        //     //full
        //        System.IO.StringWriter StringWrite = new System.IO.StringWriter();
        //        Html32TextWriter htmlWrite;
        //        divReportData.Visible = true;
        //        getData();
        //        divReportData.Controls.Add(tbl);
        //        Response.Clear();
        //        Response.AddHeader("content-disposition", "attachment;filename=PurskarCandListForAmountReleased.xls");
        //        Response.Charset = "";
        //        //Response.ContentType = "application/vnd.xls";
        //        Response.AddHeader("Content-Type", "application/vnd.ms-excel");
        //        htmlWrite = new Html32TextWriter(StringWrite);
        //        htmlWrite.AddAttribute("border", "1");
        //        divReportData.RenderControl(htmlWrite);
        //        System.IO.File.WriteAllText(fileName, StringWrite.ToString());
        //        Response.Write(StringWrite.ToString());

        //        Response.End();
        //     //full
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowAlert(ex.Message, true);
        //    }


    }
   
    private void CreateDocument()
    {
        try
        {

            #region ---------- Page Margin ------------------------------------------------------
            //Create a missing variable for missing value  
            object missing = System.Reflection.Missing.Value;

            //Create an instance for word app                     
            WordDocument doc = new WordDocument();

            PageMargins margins = new PageMargins();
            margins.Bottom = 450;// 1440; // 1 inch 
            margins.Left = 1080; // 1 inch
            margins.Right = 1080; // 1 inch
            margins.Top = 450; // 1 inch
            margins.Footer = 90; // 1/2 inch 
            margins.Header = 90; // 1/2 inch

            PageSize pageSize = new PageSize(12140, 16416); //8.5 x 11 inch
            pageSize.PageOrientation = PageOrientation.Portrait;

            Section section = new Section();
            section.PageSize = pageSize;
            section.PageMargins = margins;
            doc.Body.Section = section;

            #endregion -----------------------------------------------------------------------------

            #region--HEADER OF REPORT--------------------------------------------
            // Heading add started
            string folderPath1 = Server.MapPath("~/images/");
            string filePath1 = folderPath1 + Path.GetFileName("LetterHeader.jpg");

            //Picture picture = new Picture("../images/LetterHeader.jpg");
            Picture picture = new Picture(filePath1);
            Unit pictureWidth = new Unit(640, UnitType.Pixel);
            Unit pictureHeight = new Unit(30, UnitType.Pixel);

            Offset offset = new Offset(0, 0);
            Extents extents = new Extents(pictureWidth, pictureHeight);

            picture.ShapeProperties.PresetGeometry = new PresetGeometry(ShapeType.Rectangle);
            picture.ShapeProperties.Transform2D = new Transform2D(offset, extents);
            picture.ID = "1";
            picture.Name = "LetterHeader.jpg";

            Stretch stretch = new Stretch(); //important to scale image
            stretch.FillRectangle = new FillRectangle();
            picture.Stretch = stretch;

            Inline inline = new Inline(picture);
            inline.Size = new DrawingObjectSize(pictureWidth, pictureHeight);
            inline.ID = "1";
            inline.Name = "Picture 1";
            inline.Description = "LetterHeader.jpg";

            DrawingObject drawingObject = new DrawingObject(inline);

            Run imageRun = new Run();
            imageRun.Add(drawingObject);

            Paragraph imageParagraph = new Paragraph();
            imageParagraph.Add(imageRun);
            imageParagraph.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            imageParagraph.Spacing = new Spacing();
            imageParagraph.Spacing.After = 14;
            doc.Body.Add(imageParagraph);


            Run R1 = new Run();
            R1.AddText("Dated: " + System.DateTime.Now.ToString("dd-MMM-yyyy"));
            R1.FontSize = 18; //12 points
            R1.AsciiFont = "Century Gothic";
            //R1.Bold = ExtendedBoolean.True; // for bold  
            Paragraph P1 = new Paragraph();
            P1.Add(R1);
            P1.HorizontalTextAlignment = HorizontalAlignmentType.Right;
            P1.Spacing = new Spacing();
            P1.Spacing.Before = 300;
            doc.Body.Add(P1);

            // Heading add end
            #endregion -----------------------------------------------------------------------------

            #region --------------- Heading of Letter -------------

            Run R15 = new Run();
            R15.AddText("Ref: NIELIT/ProtsahanPuraskar/DBT/");
            R15.FontSize = 20; //12 points
            R15.AsciiFont = "Calibri (Body)";
            Paragraph P15 = new Paragraph();
            P15.Add(R15);
            P15.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            P15.Spacing = new Spacing();
            P15.Spacing.Before = 10;
            doc.Body.Add(P15);

            Run R16 = new Run();
            R16.AddText("To,");
            R16.AddBreak();
            R16.AddText("The Assistant General Manager");
            R16.AddBreak();
            R16.AddText("Bank of India, Electronics Niketan,");
            R16.AddBreak();
            R16.AddText("CGO Complex, New Delhi-110003");
            R16.FontSize = 22; //12 points
            R16.AsciiFont = "Calibri (Body)";
            Paragraph P16 = new Paragraph();
            P16.Spacing = new Spacing();
            P16.Spacing.Before = 0;
            P16.Add(R16);
            P16.HorizontalTextAlignment = HorizontalAlignmentType.Left;

            doc.Body.Add(P16);

            // Subject add
            Run R2 = new Run();
            R2.AddText("Subject : Transfer through Aadhaar payment Bridge (APB)");
            R2.FontSize = 22; //12 points
            R2.AsciiFont = "Calibri (Body)";
            R2.Bold = ExtendedBoolean.True; // for bold  
            Paragraph P2 = new Paragraph();
            P2.Add(R2);
            P2.HorizontalTextAlignment = HorizontalAlignmentType.Left;
            doc.Body.Add(P2);
            //Subject add en

            Run R17 = new Run();
            R17.AddText("Sir/Madam,");
            R17.FontSize = 22; //12 points
            R17.AsciiFont = "Calibri (Body)";
            Paragraph P17 = new Paragraph();
            P17.Add(R17);
            P17.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            P17.Spacing = new Spacing();
            P17.Spacing.Before = 0;
            doc.Body.Add(P17);

            #endregion -----------------------------------------------------------------------------

            #region  --------------- Point one ------------------------------------------------
            // 4th Line add 
            Run R7 = new Run();
            R7.AddText("NIELIT has been allotted APB code (Current A/c 604820110000232) opened through Bank of India under "+
                    "the Direct Beneficiary Transfer (DBT) scheme. The list of candidates and the amount to be transferred "+
                    "along with their Aadhaar number are as given below:");
            R7.FontSize = 22; //12 points
            R7.AsciiFont = "Calibri (Body)";
            Paragraph P7 = new Paragraph();
            P7.Add(R7);
            P7.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            P7.Spacing = new Spacing();
            P7.Spacing.Before = 5;
            doc.Body.Add(P7);
            // 4th Line  End
            #endregion -----------------------------------------------------------------------------

            #region  --------------- Details of Candidates count and details heading ------------------------------------------------
            int countOfCandidates = 0;
            string countOfCandidatess = "0";
            using (System.Data.DataTable dt = FillGridViewOnlinePuraskarApplicationRecordExamiIdWiseR())
            {
                if (dt.Rows.Count > 0)
                {
                    countOfCandidates = dt.Rows.Count;
                    if (countOfCandidates < 10)
                    {
                        countOfCandidatess = "0" + countOfCandidates.ToString();
                    }
                    else
                    {
                        countOfCandidatess =  countOfCandidates.ToString();
                    }
                }
            }          
            Run R8 = new Run();
            R8.AddText("Details of candidates (Protsahan Puraskar to be provided under DBT Scheme)- " +countOfCandidatess + " candidates (" + gvMain.Rows[0].Cells[5].Text + ")");
            R8.FontSize = 22; //12 points
            R8.AsciiFont = "Calibri (Body)";
           // R8.Bold = ExtendedBoolean.True; // for bold  
            Paragraph P8 = new Paragraph();
            P8.Add(R8);
            P8.HorizontalTextAlignment = HorizontalAlignmentType.Left;
            doc.Body.Add(P8);
            #endregion -----------------------------------------------------------------------------

            #region--Candidate Recods in Tables --------------------------------------------
            TableGrid tableGrid = new TableGrid();
            tableGrid.Columns.Add(new TableGridColumn(1200));
            tableGrid.Columns.Add(new TableGridColumn(1200));
            tableGrid.Columns.Add(new TableGridColumn(1200));
            tableGrid.Columns.Add(new TableGridColumn(1200));
            tableGrid.Columns.Add(new TableGridColumn(1200));
            tableGrid.Columns.Add(new TableGridColumn(1200));
            tableGrid.Columns.Add(new TableGridColumn(1200));
            tableGrid.Columns.Add(new TableGridColumn(1200));
            tableGrid.Columns.Add(new TableGridColumn(1200));
            tableGrid.Columns.Add(new TableGridColumn(1200));
            tableGrid.Columns.Add(new TableGridColumn(1200));

            #region-- Header of Tables --------------------------------------------
            Run run1 = new Run("SL");
            run1.Bold = ExtendedBoolean.True;

            Paragraph paragraph1 = new Paragraph();
            paragraph1.Add(run1);

            Run run2 = new Run("Ref Number");
            run2.Bold = ExtendedBoolean.True;

            Paragraph paragraph2 = new Paragraph();
            paragraph2.Add(run2);

            Run run3 = new Run("Registration Number");
            run3.Bold = ExtendedBoolean.True;

            Paragraph paragraph3 = new Paragraph();
            paragraph3.Add(run3);

            Run run4 = new Run("Candidates Name");
            run4.Bold = ExtendedBoolean.True;

            Paragraph paragraph4 = new Paragraph();
            paragraph4.Add(run4);

            Run run5 = new Run("Level");
            run5.Bold = ExtendedBoolean.True;
            Paragraph paragraph5 = new Paragraph();
            paragraph5.Add(run5);

            Run run6 = new Run("Exam");
            run6.Bold = ExtendedBoolean.True;
            Paragraph paragraph6 = new Paragraph();
            paragraph6.Add(run6);

            Run run7 = new Run("FatherName");
            run7.Bold = ExtendedBoolean.True;
            Paragraph paragraph7 = new Paragraph();
            paragraph7.Add(run7);

            Run run8 = new Run("Aadhaar Number");
            run8.Bold = ExtendedBoolean.True;
            Paragraph paragraph8 = new Paragraph();
            paragraph8.Add(run8);

            Run run9 = new Run("Bank Name");
            run9.Bold = ExtendedBoolean.True;
            Paragraph paragraph9 = new Paragraph();
            paragraph9.Add(run9);

            Run run10 = new Run("Account Number");
            run10.Bold = ExtendedBoolean.True;
            Paragraph paragraph10 = new Paragraph();
            paragraph10.Add(run10);

            Run run11 = new Run("Amount ToBe Released");
            run11.Bold = ExtendedBoolean.True;
            Paragraph paragraph11 = new Paragraph();
            paragraph11.Add(run11);


            Cell cell1 = new Cell();
            cell1.Width = new Width(TableWidthUnit.Point, 1440);
            cell1.VerticalAlignment = VerticalAlignmentType.Center;
            cell1.Shading = new Shading(ShadingPattern.Percent10);
            cell1.Add(paragraph1);

            Cell cell2 = new Cell();
            cell2.Width = new Width(TableWidthUnit.Point, 1440);
            cell2.VerticalAlignment = VerticalAlignmentType.Center;
            cell2.Shading = new Shading(ShadingPattern.Percent10);
            cell2.Add(paragraph2);

            Cell cell3 = new Cell();
            cell3.Width = new Width(TableWidthUnit.Point, 4140);
            cell3.VerticalAlignment = VerticalAlignmentType.Center;
            cell3.Shading = new Shading(ShadingPattern.Percent10);
            cell3.Add(paragraph3);

            Cell cell4 = new Cell();
            cell4.Width = new Width(TableWidthUnit.Point, 1620);
            cell4.VerticalAlignment = VerticalAlignmentType.Center;
            cell4.Shading = new Shading(ShadingPattern.Percent10);
            cell4.Add(paragraph4);

            Cell cell5 = new Cell();
            cell5.Width = new Width(TableWidthUnit.Point, 1620);
            cell5.VerticalAlignment = VerticalAlignmentType.Center;
            cell5.Shading = new Shading(ShadingPattern.Percent10);
            cell5.Add(paragraph5);

            Cell cell6 = new Cell();
            cell6.Width = new Width(TableWidthUnit.Point, 1620);
            cell6.VerticalAlignment = VerticalAlignmentType.Center;
            cell6.Shading = new Shading(ShadingPattern.Percent10);
            cell6.Add(paragraph6);

            Cell cell7 = new Cell();
            cell7.Width = new Width(TableWidthUnit.Point, 4140);
            cell7.VerticalAlignment = VerticalAlignmentType.Center;
            cell7.Shading = new Shading(ShadingPattern.Percent10);
            cell7.Add(paragraph7);

            Cell cell8 = new Cell();
            cell8.Width = new Width(TableWidthUnit.Point, 4140);
            cell8.VerticalAlignment = VerticalAlignmentType.Center;
            cell8.Shading = new Shading(ShadingPattern.Percent10);
            cell8.Add(paragraph8);

            Cell cell9 = new Cell();
            cell9.Width = new Width(TableWidthUnit.Point, 4140);
            cell9.VerticalAlignment = VerticalAlignmentType.Center;
            cell9.Shading = new Shading(ShadingPattern.Percent10);
            cell9.Add(paragraph9);

            Cell cell10 = new Cell();
            cell10.Width = new Width(TableWidthUnit.Point, 1620);
            cell10.VerticalAlignment = VerticalAlignmentType.Center;
            cell10.Shading = new Shading(ShadingPattern.Percent10);
            cell10.Add(paragraph10);

            Cell cell11 = new Cell();
            cell11.Width = new Width(TableWidthUnit.Point, 4140);
            cell11.VerticalAlignment = VerticalAlignmentType.Center;
            cell11.Shading = new Shading(ShadingPattern.Percent10);
            cell11.Add(paragraph11);

            Row headerRow = new Row();
            headerRow.Header = ExtendedBoolean.True;
            headerRow.Add(cell1);
            headerRow.Add(cell2);
            headerRow.Add(cell3);
            headerRow.Add(cell4);
            headerRow.Add(cell5);
            headerRow.Add(cell6);
            headerRow.Add(cell7);
            headerRow.Add(cell8);
            headerRow.Add(cell9);
            headerRow.Add(cell10);
            headerRow.Add(cell11);

            Table table1 = new Table(StandardBorderStyle.SingleLine);
            table1.Width = new Width(TableWidthUnit.Percent, 105);
            table1.Alignment = HorizontalAlignmentType.Center;
            table1.Grid = tableGrid;
            table1.Add(headerRow);

            #endregion ---------------
            
            using (System.Data.DataTable dt = FillGridViewOnlinePuraskarApplicationRecordExamiIdWiseR())
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    { 
                        string CellValue="0";
                        Row row1 = new Row();
                        int j = 0;
                       
                        for (j = 0; j < dt.Columns.Count; j++)
                        {
                            DataRow dr;
                            dr = dt.Rows[i];                           
                            if (j == 7)
                            {
                                CellValue = EncryptDecrypt.DecryptString(dr[j].ToString());
                            }
                            else
                            {
                                CellValue = dr[j].ToString();
                            }
                            Run run55 = new Run(CellValue);                          
                            Paragraph paragraph55 = new Paragraph();
                            paragraph55.Add(run55);                         
                            Cell cell15 = new Cell();                           
                            cell15.VerticalAlignment = VerticalAlignmentType.Center;
                            cell15.Add(paragraph55);
                            row1.Add(cell15);                          
                        }
                        table1.Add(row1);                       
                    }


                }
            }
            doc.Body.Add(table1);
            #endregion -----------------------------------------------------------------------------
           
            #region  ------------- After Table  sentences --------------------------------

            Run R9 = new Run();
            R9.AddText("Please authorise the APB disbursal for the amount of Rs. 24000 from current Account no. 604820110000232. (opened for DBT) for the transfer of APB fund.");
            R9.FontSize = 22; //12 points
            R9.AsciiFont = "Calibri (Body)";
            Paragraph P9 = new Paragraph();
            P9.Add(R9);
            P9.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            P9.Spacing = new Spacing();
            P9.Spacing.Before = 300;            
            doc.Body.Add(P9);
           
            #endregion -------------------------------------

            #region ---------- Table 8 for Last Page context, Authorized Singnaute  -----------------------------------------------------

            TableGrid tableGrid8 = new TableGrid();
            tableGrid8.Columns.Add(new TableGridColumn(4000));
            tableGrid8.Columns.Add(new TableGridColumn(4000));

            Run CellValueS31 = new Run("Authorized Signatory2");
            CellValueS31.FontSize = 24;
            CellValueS31.Bold = ExtendedBoolean.True;
            CellValueS31.AsciiFont = "Calibri (Body)";
            Paragraph CellAddValueS31 = new Paragraph();
            CellAddValueS31.Spacing = new Spacing();
            CellAddValueS31.Spacing.Before = 1100;
            CellAddValueS31.Spacing.After = 0;
            CellAddValueS31.Add(CellValueS31);
            CellAddValueS31.HorizontalTextAlignment = HorizontalAlignmentType.Left;
            Cell cellS31 = new Cell();
            cellS31.Width = new Width(TableWidthUnit.Point, 4000);
            cellS31.VerticalAlignment = VerticalAlignmentType.Top;
            cellS31.Add(CellAddValueS31);

            Run CellValueS32 = new Run("Authorized Signatory1");
            CellValueS32.FontSize = 24; //18 points
            CellValueS32.Bold = ExtendedBoolean.True;
            CellValueS32.AsciiFont = "Calibri (Body)";
            Paragraph CellAddValueS32 = new Paragraph();
            CellAddValueS32.Spacing = new Spacing();
            CellAddValueS32.Spacing.Before = 1100;
            CellAddValueS32.Spacing.After = 0;
            CellAddValueS32.Add(CellValueS32);
            CellAddValueS32.HorizontalTextAlignment = HorizontalAlignmentType.Right;

            Cell cellS32 = new Cell();
            cellS32.Width = new Width(TableWidthUnit.Point, 4000);
            cellS32.VerticalAlignment = VerticalAlignmentType.Top;
            cellS32.Add(CellAddValueS32);

            Row rowS3 = new Row();
            rowS3.Add(cellS31);
            rowS3.Add(cellS32);

            Table table8 = new Table(StandardBorderStyle.None);
            table8.Width = new Width(TableWidthUnit.Percent, 100);
            table8.Alignment = HorizontalAlignmentType.Center;
            table8.Grid = tableGrid8;
            table8.Add(rowS3);
            doc.Body.Add(table8);
            // signature and name wing  END
            #endregion -----------------------------------------------------------------------------

            #region -------- DownLoad Report ----------------------------------------------
            //create new folder and save the file and download
            //doc.Body.Add(table1);
            string folderPath = Server.MapPath("~/PuraskarDocument/");
            string filePath = folderPath + Path.GetFileName("temp26.docx");
            if (!Directory.Exists(folderPath))
            { Directory.CreateDirectory(folderPath); }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            doc.Save(folderPath + "temp26.docx");
            WebClient wc = new WebClient();

            ShowAlert("Document created successfully !");

            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            string filePath2 = "~/PuraskarDocument/" + "temp26.docx";
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=ProtsahanPuraskarReport.docx");
            byte[] data = req.DownloadData(Server.MapPath(filePath2));
            response.BinaryWrite(data);
            // delete the file which created after download
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            response.End();

            #endregion -----------------------------------------------------------------------------


        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    } 

    ////private void CreateDocument1()
    ////{
    ////    try
    ////    {
    ////        //Create an instance for word app  
    ////        Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();

    ////        //Set animation status for word application  
    ////        winword.ShowAnimation = false;

    ////        //Set status for word application is to be visible or not.  
    ////        winword.Visible = false;

    ////        //Create a missing variable for missing value  
    ////        object missing = System.Reflection.Missing.Value;

    ////        //Create a new document  
    ////        Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

    ////        //Add header into the document  
    ////        //foreach (Microsoft.Office.Interop.Word.Section section in document.Sections)
    ////        //{
    ////        //    //Get the header range and add the header details.  
    ////        //    Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
    ////        //    headerRange.Fields.Add(headerRange, Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage);
    ////        //    //headerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
    ////        //   // headerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdBlue;
    ////        //    headerRange.Font.Size = 10;
    ////        //    headerRange.Text = "Ref: NIELIT/ProtsahanPuraskar/DBT/"+examName+"          Date:"+System.DateTime.Today.ToString("dd/mm/yyyy");
    ////        //}

    ////        //Add the footers into the document  
    ////        //foreach (Microsoft.Office.Interop.Word.Section wordSection in document.Sections)
    ////        //{
    ////        //    //Get the footer range and add the footer details.  
    ////        //    Microsoft.Office.Interop.Word.Range footerRange = wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
    ////        //    //footerRange.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdDarkRed;
    ////        //    footerRange.Font.Size = 10;
    ////        //    //footerRange.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
    ////        //    footerRange.Text = "Authorized Signatory2                           Authorized Signatory1";
    ////        //}

    ////        //adding text to document  
    ////        document.Content.SetRange(0, 0);
    ////        document.Content.Text = "Ref: NIELIT/ProtsahanPuraskar/DBT/"+examName+"\t\t\t\t\t\tDate: "+System.DateTime.Today.ToString("dd/MM/yyyy")+Environment.NewLine+"To, " + Environment.NewLine;

    ////        //Add paragraph with Heading 1 style  
    ////        Microsoft.Office.Interop.Word.Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
    ////        object styleHeading1 = "No Spacing";
            
    ////        para1.Range.set_Style(ref styleHeading1);
    ////        para1.Range.Text = "The Assistant General Manager" + Environment.NewLine + "Bank of India, Electronics Niketan," + Environment.NewLine+"CGO Complex, New Delhi-110003";
    ////        para1.Range.InsertParagraphAfter();

    ////        //Add paragraph with Heading 2 style  
    ////        Microsoft.Office.Interop.Word.Paragraph para2 = document.Content.Paragraphs.Add(ref missing);
    ////        object styleHeading2 = "Normal";
    ////        para2.Range.set_Style(ref styleHeading2);
    ////        para2.Range.Text = "Subject: Transfer through Aadhaar payment Bridge (APB)" + Environment.NewLine+"Sir/Madam,";
    ////        para2.Range.InsertParagraphAfter();

    ////        Microsoft.Office.Interop.Word.Paragraph para3 = document.Content.Paragraphs.Add(ref missing);
    ////        object styleHeading3 = "Normal";
    ////        para3.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
    ////        para3.Range.set_Style(ref styleHeading3);
    ////        para3.Range.Text = " NIELIT has been allotted APB code (Current A/c 604820110000232) opened through Bank of India under"  ;
    ////        para3.Range.Text += " the Direct Beneficiary Transfer (DBT) scheme. The list of candidates and the amount to be transferred " ;
    ////        para3.Range.Text += "along with their Aadhaar number are as given below: " ;
    ////        para3.Range.InsertParagraphAfter();

    ////        Microsoft.Office.Interop.Word.Paragraph para4 = document.Content.Paragraphs.Add(ref missing);
    ////        object styleHeading4 = "Normal";
    ////        para4.Range.set_Style(ref styleHeading4);
    ////        para4.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
    ////        para4.Range.Text = "Details of candidates (Protsahan Puraskar to be provided under DBT Scheme)-" + gvMain.Rows.Count.ToString().Trim() + " candidates (" + gvMain.Rows[0].Cells[5].Text + ")" + Environment.NewLine;
    ////        para4.Range.InsertParagraphAfter();

    ////        //Create a 5X5 table and insert some dummy record  
    ////        Microsoft.Office.Interop.Word.Table firstTable = (Microsoft.Office.Interop.Word.Table)document.Tables.Add(para1.Range,gvMain.Rows.Count+1 ,gvMain .Columns.Count );

    ////        firstTable.Borders.Enable = 1;
    ////        int r = 0;
    ////        foreach (Row row in firstTable.Rows)
    ////        {
                
    ////            int c = 0;
    ////            foreach (Cell cell in row.Cells)
    ////            {
    ////                //Header row  
    ////                if (cell.RowIndex == 1)
    ////                {
    ////                    cell.Range.Text = gvMain.Columns[c].HeaderText;
    ////                        //gvMain.Rows[r].Cells[c].Text ;
                       
    ////                    cell.Range.Font.Bold = 1;
    ////                    //other format properties goes here  
    ////                    cell.Range.Font.Name = "verdana";
    ////                    cell.Range.Font.Size = 10;
    ////                    //cell.Range.Font.ColorIndex = WdColorIndex.wdGray25;                              
    ////                    cell.Shading.BackgroundPatternColor = WdColor.wdColorGray25;
    ////                    //Center alignment for the Header cells  
    ////                    cell.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
    ////                    cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
    ////                    //cell.Range.Text = gvMain.Rows[r].Cells[c].Text;
    ////                }
    ////                //Data row  
    ////                else
    ////                {
    ////                    int x =Convert.ToInt32 ( gvMain.Rows.Count);
    ////                    cell.Range.Text = gvMain.Rows[r-1].Cells[c].Text;
    ////                        //(cell.RowIndex - 2 + cell.ColumnIndex).ToString();
    ////                }
    ////                c++;
    ////            }
    ////            r++;
    ////        }
    ////       // firstTable.Range.InsertParagraphAfter();
    ////       // firstTable.Range.InsertParagraphAfter();
    ////        Microsoft.Office.Interop.Word.Paragraph para5a = document.Content.Paragraphs.Add(ref missing);
    ////        object styleHeading5a = "Normal";
    ////        para5a.Range.set_Style(ref styleHeading5a);
    ////        para5a.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
    ////        //para5a.Range.InsertParagraphBefore();
    ////        para5a.Range.InsertParagraphBefore();
    ////        para5a.Range.Text = "Please authorise the APB disbursal for the amount of Rs. " + totAmountReleased.ToString().Trim() + " from current Account no. 604820110000232. (opened for DBT) for the transfer of APB fund."+ Environment.NewLine;
    ////       // para5a.Range.InsertParagraphAfter();
    ////        para5a.Range.InsertParagraphAfter();
       
    ////       // document.Tables.Add(firstTable.Range,firstTable.Rows.Count,firstTable .Columns.Count );

    ////        Microsoft.Office.Interop.Word.Paragraph para5 = document.Content.Paragraphs.Add(ref missing);
    ////        object styleHeading5 = "Normal";
    ////        para5.Range.set_Style(ref styleHeading5);
    ////        para5.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
    ////        para5.Range.Text = "Authorized Signatory2\t\t\t\t\t\t\tAuthorized Signatory1" + Environment.NewLine;
    ////        para5.Range.InsertParagraphAfter();

    ////        //Save the document  
    ////        string path = "~/download/BankLetter.docx";
    ////        //object filename = @"d:\temp1.docx";
    ////        object filename = Server.MapPath(path);
    ////        document.SaveAs2(ref filename);
            
    ////        document.Close(ref missing, ref missing, ref missing);
    ////        document = null;
    ////        winword.Quit(ref missing, ref missing, ref missing);
    ////        winword = null;
    ////        ShowAlert("Document created successfully ! Ready to download");
    ////        //Download word doc
    ////        string path1 =Server .MapPath (path);
    ////        System.IO.FileInfo file = new System.IO.FileInfo(path1);

    ////        if (file.Exists)
    ////        {

    ////            Response.Clear();

    ////            Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);

    ////            Response.AddHeader("Content-Length", file.Length.ToString());

    ////            Response.ContentType = "application/octet-stream";

    ////            Response.WriteFile(file.FullName);

    ////            Response.End();

    ////        }
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        ShowAlert(ex.Message);
    ////    }
    ////} 
   
     
}  
